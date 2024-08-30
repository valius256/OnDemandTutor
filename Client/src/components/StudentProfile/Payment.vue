<template>
    <div>
        <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200 ">
            Thông tin thanh toán
        </div>
        <div class="flex justify-center mb-8">
            <div class="text-3xl font-bold py-1">
                <div class="mb-4">Số dư hiện tại </div>
                <div class="text-green-200 p-1 bg-green-600 rounded-lg text-center">
                    {{ balance.toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                    }) }} </div>
            </div>
        </div>
        <div class="flex gap-4 justify-center mt-4 text-2xl mb-6">
            <button @click="toggleRechargePopup"
                class="mr-6 px-6 py-4 font-bold text-white bg-blue-400 hover:bg-blue-200 rounded-lg">Nạp tiền</button>
            <button @click="toggleWithdrawPopup"
                class="px-6 py-4 font-bold text-white bg-green-400 hover:bg-green-200 rounded-lg">Rút
                tiền</button>
        </div>
        <div v-if="user && user.role == 0">
            <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200 ">
                Các Slot chưa thanh toán
            </div>
            <div class="mt-2 flex flex-col gap-2 p-4" v-if="this.unpaidSlots.length > 0">
                <button @click="toggleOpenSlotDetailPopup(slot)" v-for="slot in unpaidSlots" :key="slot.id"
                    class="bg-slate-100 rounded-lg shadow-md p-4">
                    <div class="flex place-content-between">
                        <div>
                            <span class="font-bold mr-2">Bắt đầu :</span>
                            <span>{{ this.beautifyDatetime(slot.slot.startTime) }}</span>
                        </div>
                        <div>
                            <span class="font-bold mr-2">Kết thúc :</span>
                            <span>{{ this.beautifyDatetime(slot.slot.endTime) }}</span>
                        </div>
                    </div>
                    <div class="flex place-content-between">
                        <div>
                            <span class="font-bold mr-2">Gia sư :</span>
                            <span>{{ (slot.slot.createdBy.firstName ?? "") + " " + (slot.slot.createdBy.lastName ?? "")
                                }}</span>
                        </div>
                        <div>
                            <span class="font-bold mr-2">Giá cả :</span>
                            <span class="p-2 text-red-500 font-bold text-xl">{{ (calcDuration(slot) *
                        slot.slot.createdBy.tutorFeePerHour).toLocaleString('vi-VN', {
                            style: 'currency',
                            currency: 'VND',
                        }) }}</span>
                        </div>
                    </div>
                    <div v-if="slot.slot.class" class="flex justify-start">
                        <span class="font-bold mr-2">Lớp học :</span>
                        <span>{{ slot.slot.class.name }}</span>
                    </div>
                </button>
            </div>

            <div v-else class="text-center italic">
                Hiện chưa có Slot nào
            </div>
        </div>
        <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200 mt-4">
            Lịch sử giao dịch
        </div>
        <div class="px-4 mb-4">
            <table class="bg-slate-50 p-6 rounded-xl text-center w-full" v-if="transactions.length > 0">
                <thead>
                    <th>Mã giao dịch</th>
                    <th>Ngày</th>
                    <th>Số lượng</th>
                    <th>Loại</th>
                    <th>Mô tả</th>
                </thead>
                <tbody>
                    <tr v-for="transaction in transactions" :key="transaction.id">
                        <td>{{ transaction.transactionCode }}</td>
                        <td>{{ this.beautifyDatetime(transaction.createdDate) }}</td>
                        <td :class="getAmountStyle(transaction)">
                            {{ Math.abs(transaction.amount).toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                    }) }}
                        </td>
                        <td v-if="transaction.transactionType == 0">Rút tiền</td>
                        <td v-if="transaction.transactionType == 1">Nạp tiền</td>
                        <td v-if="transaction.transactionType == 2">Trừ tiền</td>
                        <td v-if="transaction.transactionType == 3">Nhận tiền</td>
                        <td v-if="transaction.transactionType == 4">Thanh toán</td>
                        <td>{{ transaction.notes }}</td>
                    </tr>
                </tbody>
            </table>
            <div class="flex gap-4 justify-center mt-4" v-if="transactions.length > 0">
                <button @click="movePage(false)">
                    <i class="fa fa-arrow-left text-2xl"></i>
                </button>
                <div class="flex gap-2 ">
                    <input class="border p-1 rounded-md w-16" type="number" v-model="currentPage" min="1"
                        @change="handlePageChange">
                    <div class="p-1"> / {{ this.totalPage }}</div>
                </div>
                <button @click="movePage(true)">
                    <i class="fa fa-arrow-right text-2xl"></i>
                </button>
            </div>
            <div v-else class="text-center italic">
                Hiện chưa có giao dịch nào
            </div>
        </div>
        <generic-popup v-if="isOpenWithdrawPopup" :title="'Tạo yêu cầu rút tiền'" :closeFunction="toggleWithdrawPopup"
            :notOverflow="true">
            <request-withdraw-popup :close="toggleWithdrawPopup" :action="navigateToPayment"
                :balance="balance"></request-withdraw-popup>
        </generic-popup>
        <generic-popup v-if="isOpenSlotDetailPopup" :title="'Chi tiết slot học'"
            :closeFunction="toggleOpenSlotDetailPopup" :notOverflow="true">
            <slot-detail-popup :close="toggleOpenSlotDetailPopup" :action="getUserSlots"
                :slot="selectedSlot"></slot-detail-popup>
        </generic-popup>
        <generic-popup v-if="isOpenRechargePopup" :title="'Nạp tiền'"
            :closeFunction="toggleRechargePopup" :notOverflow="true">
            <recharge-popup :close="toggleRechargePopup" :action="fetchUser"></recharge-popup>
        </generic-popup>
    </div>

</template>

<script>
import axios from 'axios'
import GenericPopup from '../common/GenericPopup.vue'
import RequestWithdrawPopup from './RequestWithdrawPopup.vue'
import SlotDetailPopup from './SlotDetailPopup.vue'
import RechargePopup from '../TutorProfile/RechargePopup.vue'
export default {
    components: { GenericPopup, RequestWithdrawPopup, SlotDetailPopup, RechargePopup },
    props: ['id'],
    name: "StudentProfilePayment",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            balance: 0,
            user: null,
            transactions: [

            ],
            unpaidSlots: [

            ],
            selectedSlot: null,
            isOpenWithdrawPopup: false,
            isOpenSlotDetailPopup: false,
            isOpenRechargePopup : false,
        }
    },
    methods: {
        calcDuration(slot) {
            const startTime = new Date(slot.slot.startTime);
            const endTime = new Date(slot.slot.endTime);
            return (endTime - startTime) / 3600000;
        },
        getAmountStyle(transaction) {
            let css = "font-bold"
            if (transaction.transactionType == 0 || transaction.transactionType == 2) {
                return css + " text-red-400"
            } else if (transaction.transactionType == 1 || transaction.transactionType == 3) {
                return css + " text-green-400"
            } else {
                return css + " text-blue-400"
            }
        },
        async handlePageChange() {
            if (this.currentPage > this.totalPage) {
                this.currentPage = this.totalPage
            }
            if (this.currentPage < 1) {
                this.currentPage = 1
            }
            await this.fetchTranscations()
            //await this.fetchRegistration(this.currentPage, this.pageSize, this.keyword_name)
        },
        async movePage(forward) {
            if (forward && this.currentPage < this.totalPage) {
                this.currentPage++
                await this.handlePageChange()
            } else if (!forward && this.currentPage > 1) {
                this.currentPage--
                await this.handlePageChange()
            }
        },
        async fetchTranscations() {
            let query = {
                Page: this.currentPage,
                Limit: this.pageSize,
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/Transaction/all?' +
                this.jsonToQueryString(query), {
                headers: {
                    'Authorization': "Bearer " + localStorage.token
                }
            })
            if (response.data) {
                this.transactions = response.data.items
                this.totalPage = Math.ceil(response.data.total / this.pageSize)
            }
        },
        async getUserSlots() {
            var today = new Date()
            let queryString = "?"
            queryString += "From=1900-01-01"
            queryString += "&To=3000-01-01"
            queryString += "&PaymentStatus=" + 0
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/SlotStudent/get-slots-of-students' + queryString, {
                headers: {
                    'Authorization': "Bearer " + localStorage.token
                }
            })
            if (response.data) {
                this.unpaidSlots = response.data
            }
            console.log(this.unpaidSlots)
        },
        toggleWithdrawPopup() {
            this.isOpenWithdrawPopup = !this.isOpenWithdrawPopup
        },
        toggleRechargePopup() {
            this.isOpenRechargePopup = !this.isOpenRechargePopup
        },
        navigateToPayment() {
            this.$router.push('/student/withdraw')
        },
        async fetchUser() {
            console.log(this.id)
            if (this.id) {
                const response = await axios.get(import.meta.env.VITE_API_URL + '/api/User/profile?userId=' + this.id)
                if (response.data) {
                    this.user = response.data.data
                }
                const balanceResponse = await axios.get(import.meta.env.VITE_API_URL + '/api/User/balance', {
                    headers: {
                        'Authorization': "Bearer " + localStorage.token
                    }
                })
                if (balanceResponse.data) {
                    this.balance = balanceResponse.data.data.balance
                }
            }
        },
        toggleOpenSlotDetailPopup(slot) {
            this.selectedSlot = slot
            this.isOpenSlotDetailPopup = !this.isOpenSlotDetailPopup
        }

    },
    mounted() {
        this.fetchTranscations()
        this.fetchUser()
        this.getUserSlots()
    }
}
</script>

<style scoped>
tr td,
th {
    padding: 0.5rem 2rem 0.5rem 2rem;
    border: solid 1px #ffffff
}
</style>
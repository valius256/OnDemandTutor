<template>
    <div class="-ml-4 w-full" v-if="user">
        <div class="text-2xl font-bold p-8 bg-gray-300 ">
            Chào mừng trở lại, {{ (user.firstName ?? "") + " " + (user.lastName ?? "") }}
        </div>
        <div class="flex justify-end mr-4">
            <button class="text-blue-400 font-bold underline" @click="toggleOpenChangePass">Đổi mật khẩu</button>
        </div>
        <div class="text-center font-bold text-3xl mt-4">THỐNG KÊ GIAO DỊCH</div>
        <div class="px-4 mb-4">
            <table id="operator-table" v-if="transactions.length > 0"  >
                <thead>
                    <tr>
                        <th>Mã giao dịch</th>
                        <th>Ngày</th>
                        <th>Số lượng</th>
                        <th>Loại</th>
                        <th>Mô tả</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="transaction in transactions" :key="transaction.id">
                        <td class="w-48 break-all">{{ transaction.transactionCode }}</td>
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
        <generic-popup v-if="isOpenChangePassPopup" title="Đổi mật khẩu" :closeFunction="toggleOpenChangePass">
            <change-password-popup :userId="user.id" :close="toggleOpenChangePass"></change-password-popup>
        </generic-popup>
    </div>

</template>

<script>
import axios from 'axios'
import ChangePasswordPopup from '../../components/common/ChangePasswordPopup.vue'
import GenericPopup from '../../components/common/GenericPopup.vue'
export default {
    name: "AdminDashboard",
    components: { ChangePasswordPopup, GenericPopup },
    data() {

        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            user: null,
            isOpenChangePassPopup: false,
            transactions: []
        }
    },
    methods: {
        getAmountStyle(transaction) {
            let css = "font-bold"
            if (transaction.transactionType == 0  || transaction.transactionType == 3 ) {
                return css + " text-red-400"
            } else if (transaction.transactionType == 1|| transaction.transactionType == 2 || transaction.transactionType == 4 ) {
                return css + " text-green-400"
            } 
        },
        async getUser() {
            this.user = await this.getUserFromToken()
        },
        toggleOpenChangePass() {
            this.isOpenChangePassPopup = !this.isOpenChangePassPopup
        },
        async handlePageChange() {
            if (this.currentPage > this.totalPage) {
                this.currentPage = this.totalPage
            }
            if (this.currentPage < 1) {
                this.currentPage = 1
            }
            await this.fetchTranscations()
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
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/Transaction/all-admin?' +
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
    },
    mounted() {
        this.getUser()
        this.fetchTranscations()
    }
}
</script>

<style></style>
<template>

    <div class="p-4 w-full">
        <div class="text-2xl font-bold">
            Yêu cầu rút tiền
        </div>
        <table id="operator-table">
            <thead>
                <tr>
                    <th>Tên</th>
                    <th>SDT</th>
                    <th>Tạo ngày</th>
                    <th>Tài khoản</th>
                    <th>Ngân hàng</th>
                    <th>Lý do</th>
                    <th>Số lượng</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="request in requests" :key="request.id">
                    <td><div class="w-32 break-words font-bold">{{ request.user.firstName + " " + request.user.lastName }}</div></td>
                    <td>{{ request.user.phone }}</td>
                    <td>{{ this.beautifyDatetime(request.createdDate) }}</td>
                    <td>{{ request.bankAccountNumber }}</td>
                    <td>{{ request.bankName }}</td>
                    <td>{{ request.description }}</td>
                    <td class="font-bold text-green-500">{{ request.amount.toLocaleString('vi-VN', {style: 'currency',currency: 'VND',}) }}</td>
                    <td>
                        <button @click="openUpdatePopup(request)" class="p-2 bg-blue-300 rounded-lg font-bold text-white">Cập nhật</button>
                    </td>             
                </tr>
            </tbody>
        </table>
        <div class="flex gap-4 justify-center mt-4" v-if="this.requests.length > 0">
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
        <generic-popup v-if="isOpenUpdatePopup" title="Cập nhật yêu cầu rút tiền" :closeFunction="closeUpdatePopup">
            <withdraw-request-update-popup :close="closeUpdatePopup" :request="selectRequest" :action="fetchData" />
        </generic-popup>
    </div>
</template>

<script>
import axios from 'axios'
import GenericPopup from '../../components/common/GenericPopup.vue'
import WithdrawRequestUpdatePopup from '../../components/Operators/WithdrawRequestUpdatePopup.vue'
export default {
    components: { GenericPopup, WithdrawRequestUpdatePopup },
    inject : ['eventBus'],
    name: "WithdrawRequest",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            selectRequest: null,
            isOpenUpdatePopup: false,
            requests: [
            ],
        }
    },
    methods: {
        async fetchData() {
            let query = {
                Page: this.currentPage,
                Limit: this.pageSize
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/RequestWithDraw/admin-get-all?'+ this.jsonToQueryString(query),{
                headers : {
                    "Authorization" : "Bearer " + localStorage.token
                }
            })
            if (response.data) {
                this.requests = response.data.items
                this.totalPage = Math.ceil(response.data.total / this.pageSize)
            }
        },
        openUpdatePopup(request) {
            this.isOpenUpdatePopup = true
            this.selectRequest = request
        },
        closeUpdatePopup(){
            this.isOpenUpdatePopup = false
            this.selectRequest = null
        },
        
        async handlePageChange() {
            if (this.currentPage > this.totalPage) {
                this.currentPage = this.totalPage
            }
            if (this.currentPage < 1) {
                this.currentPage = 1
            }
            await this.fetchData()
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
    },
    mounted() {
        this.fetchData()
    }
}
</script>

<style scoped></style>
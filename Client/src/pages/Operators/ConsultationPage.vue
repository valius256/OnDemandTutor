<template>
    <div class="p-4 w-full" @click="setSelectId(0)">
        <div class="text-2xl font-bold">
            Yêu cầu tư vấn
        </div>
        <div class="flex justify-end italic mt-12 gap-4">
            Chỉ hiện thị những yêu cầu chưa giải quyết
            <input type="checkbox" class="w-6" v-model="isShowPending" @change="fetchData">
        </div>
        <div class="mt-4">
            <table id="operator-table" class="table-auto overflow-x-auto ">
                <thead>
                    <tr>
                        <th class="w-2/12">Tên</th>
                        <th class="w-2/12">Phone</th>
                        <th class="w-2/12">Email</th>
                        <th class="w-2/12">Ngày yêu cầu</th>
                        <th class="w-4/12">Nội dung</th>
                        <th class="w-1/12">Trạng thái</th>
                        <th class="w-1/12"></th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="request in requests" :key="request.id">
                        <td>{{ request.name }}</td>
                        <td>{{ request.phone }}</td>
                        <td class="break-all">{{ request.email }}</td>
                        <td>{{ this.beautifyDatetime(request.createdDate) }}</td>
                        <td>{{ request.consultationContent }}</td>
                        <td>
                            <div :class="getStatusStyle(request.status)">
                                <span class="p-2" v-if="request.status == 0">Đang chờ</span>
                                <span class="p-2" v-if="request.status == 1">Đã xong</span>
                            </div>
                        </td>
                        <td class="relative">
                            <button class="p-2 bg-slate-200 hover:bg-slate-400 font-bold rounded-full"
                                @click.stop="setSelectId(request.id)">
                                <i class="fa fa-ellipsis-h	"></i>
                            </button>
                            <div v-if="selectId == request.id"
                                class="absolute right-0 bg-white rounded-lg shadow-lg z-10 w-48 animate-fade-down animate-duration-[400ms] animate-normal font-bold flex flex-col">
                                <!-- Content of your menu -->
                                <button v-if='request.status == 0'
                                    class="hover:bg-slate-200 p-2 rounded-t-lg text-left text-green-400"
                                    @click="handleResolve(request.id)">
                                    <i class="fa fa-check mr-4"></i>Đã giải quyết
                                </button>
                                <button v-if='request.status == 1'
                                    class="hover:bg-slate-200 p-2 rounded-t-lg text-left text-red-400"
                                    @click="handleResolve(request.id)">
                                    <i class="fa fa-close mr-4"></i>Chưa giải quyết
                                </button>
                                <!-- <li class="hover:bg-slate-200 p-2"></li> -->
                                <button class="hover:bg-slate-200 p-2 rounded-b-lg text-left">
                                    <i class="fa fa-trash mr-4"></i>Xóa
                                </button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
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
    </div>
</template>

<script>
import axios from 'axios'

export default {
    name: "ConsultationPage",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            selectId: 0,
            isShowPending : false,
            requests: [
            ],

        }
    },
    methods: {
        async fetchData() {
            let queryString = ""
            if (this.isShowPending){
                queryString += "?status=0"
            }
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/ConsultationControllers/all' + queryString)
            if (response.data) {
                this.requests = response.data.data
                this.totalPage = 1//Math.ceil(response.data.total / this.pageSize)
            }
        },
        getStatusStyle(status) {
            let css = "text-center font-bold text-white rounded-lg flex"
            switch (status) {
                case 0:
                    return css + " bg-red-400"
                case 1:
                    return css + " bg-green-400"
            }
        },
        async handlePageChange() {
            if (this.currentPage > this.totalPage) {
                this.currentPage = this.totalPage
            }
            if (this.currentPage < 1) {
                this.currentPage = 1
            }
            await this.fetchSubject()
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
        setSelectId(id) {
            if (id == this.selectId) {
                this.selectId = 0
            } else {
                this.selectId = id
                this.isShowPopup = true
            }
        },
    },
    mounted() {
        this.fetchData()
    }
}
</script>

<style></style>
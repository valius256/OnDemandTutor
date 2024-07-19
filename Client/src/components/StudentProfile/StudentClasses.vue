<template>
    <div class="">
        <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200 ">
            Lớp học bạn đã tham gia
        </div>
        <div class="px-8 py-2" v-if="!isOpenClassDetailPopup">
            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                <div v-for="class_ in classes" :key="class_.id" class="shadow-md rounded-lg">
                    <div :class="getStatusStyleHeader(class_.status)">
                        {{ class_.name }}
                    </div>
                    <div class="p-4 max-h-72 overflow-y-auto">
                        <ul class="px-4 flex flex-col gap-2">
                            <li>
                                <span class="font-bold">Gia sư :</span>
                                <button class="ml-3 font-bold text-blue-400 underline">
                                    {{ (class_.user.firstName ?? "") ?? (class_.user.lastName ?? "") }}
                                </button>
                            </li>
                            <li>
                                <span class="font-bold">Thời gian :</span>
                                <span class="ml-3">
                                    {{ (class_.startTime?.substring(0,10)) ?? "" }} đến
                                    {{ (class_.endTime?.substring(0,10)) ?? ""}}
                                </span>
                            </li>
                            <li>
                                <span class="font-bold">Môn học :</span>
                                <span class="ml-3">{{ class_.subject.name }}</span>
                            </li>
                            <li v-if="class_.method == 'Offline'">
                                <span class="font-bold">Địa điểm :</span>
                                <span class="ml-3">{{ class_.location }}</span>
                            </li>
                            <li>
                                <span class="font-bold">Hình thức :</span>
                                <span :class="getMethodStyle(class_.method)">{{ class_.method }}</span>
                            </li>
                            <li>
                                <span class="font-bold">Trạng thái :</span>
                                <span :class="getStatusStyle(class_.status)">
                                    {{ getStatusDisplay(class_.status) }}
                                </span>
                            </li>
                            <!-- <li>
                                <span class="font-bold">Giá cả :</span>
                                <span class="ml-3 text-red-500 font-bold">
                                    {{ class_.tutor.price.toLocaleString('vi-VN', {
                            style: 'currency',
                            currency: 'VND',
                        }) }} / h
                                </span>
                            </li> -->
                        </ul>
                    </div>
                    <div class="flex justify-center gap-2 my-2" >
                        <button v-if="class_.status == 2" class="bg-blue-500 text-white font-bold p-2 rounded-lg">Đánh giá gia sư</button>
                        <button @click="toggleClassDetailPopup(class_.id)" class="bg-blue-500 text-white font-bold p-2 rounded-lg">Xem thêm</button>
                    </div>
                </div>
            </div>
            <div class="flex gap-4 justify-center mt-4" v-if="classes.length > 0">
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
        <div v-else>
            <button class="ml-8 px-8 py-2 bg-blue-400 font-bold text-white rounded-lg" @click="toggleClassDetailPopup">Trở về</button>
            <class-detail-popup :classId="selectedClass"></class-detail-popup>
        </div>
    </div>
</template>

<script>
import axios from 'axios'
import GenericPopup from '../common/GenericPopup.vue'
import ClassDetailPopup from './ClassDetailPopup.vue'
export default {
  components: { GenericPopup, ClassDetailPopup },
    name: "StudentClasses",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            selectedClass : 0,
            isOpenClassDetailPopup : false,
            classes: []
        }
    },
    methods: {
        async fetchData() {
            let query = {
                Sorts : {                 
                },
                Page: this.currentPage,
                Limit: this.pageSize
            }
            let queryStr = this.jsonToQueryString(query)
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/Class/student?'+ 
            queryStr,{
                headers : {
                    "Authorization" : "Bearer " + localStorage.token
                }
            })
            if (response.data) {
                this.classes = response.data.items
                this.totalPage = Math.ceil(response.data.total / this.pageSize)
            }

        },
        getMethodStyle(method) {
            let general = "ml-4 rounded-lg px-3 py-1 text-white font-bold"
            switch (method) {
                case "Online":
                    return general + " bg-green-400"
                default:
                    return general + " bg-gray-400"
            }
        },
        getStatusStyleHeader(status) {
            let general = "font-bold text-center py-4 rounded-t-lg text-white"
            switch (status) {
                case 0:
                    return general + " bg-cyan-500"
                case 1:
                    return general + " bg-green-400"
                default:
                    return general + " bg-gray-400"
            }
        },
        getStatusStyle(status) {
            let general = "ml-3 rounded-lg px-3 py-1 font-bold"
            switch (status) {
                case 0:
                    return general + " text-blue-400"
                case 1:
                    return general + " text-green-400"
                default:
                    return general + " text-gray-400"
            }
        },
        getStatusDisplay(status) {
            switch (status) {
                case 0:
                    return "Sắp bắt đầu"
                case 1:
                    return "Đang diễn ra"
                case 2:
                    return "Đã kết thúc"
                default:
                    return "Không rõ"
            }
        },
        async handlePageChange() {
            if (this.currentPage > this.totalPage) {
                this.currentPage = this.totalPage
            }
            if (this.currentPage < 1) {
                this.currentPage = 1
            }
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
        toggleClassDetailPopup(id){
            scrollTo(0,0)
            this.selectedClass = id
            this.isOpenClassDetailPopup = !this.isOpenClassDetailPopup
        }
    },
    mounted(){
        this.fetchData()
    }
}
</script>

<style></style>
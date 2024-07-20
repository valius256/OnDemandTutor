<template>
    <div class="p-4">
        <div class="flex gap-4" v-if="!isShowClassDetail">
            <div class="w-1/4 border-r-2">
                <div class="font-bold text-xl">Bộ lọc</div>
                <div class="border rounded-lg p-2 mt-4 mr-2 shadow-lg">
                    <div>
                        <div>Tên lớp</div>
                        <input class="border rounded-lg p-1 w-full" placeholder="Nhập tên lớp"
                            v-model="filterDto.className">
                    </div>
                    <div class="mt-4">
                        <div>Môn học</div>
                        <select class="border rounded-lg p-1 w-full" v-model="filterDto.subject">
                            <option :value="0">Tất cả</option>
                            <option :value="subject.id" v-for="subject in subjects" :key="subject.id">
                                {{ subject.name }}
                            </option>
                        </select>
                    </div>
                    <div class="mt-4">
                        <div>Tên gia sư</div>
                        <input class="border rounded-lg p-1 w-full" placeholder="Nhập gia sư"
                            v-model="filterDto.tutorName">
                    </div>
                    <div class="mt-4">
                        <div>Nhập địa chỉ</div>
                        <input class="border rounded-lg p-1 w-full" placeholder="Nhập địa chỉ, vd Thủ Đức, TPHCM,..."
                            v-model="filterDto.address">
                    </div>
                    <div class="mt-4">
                        <div>Từ ngày</div>
                        <input class="border rounded-lg p-1 w-full" type="date" v-model="filterDto.fromDate">
                    </div>
                    <div class="mt-4">
                        <div>Đến ngày</div>
                        <input class="border rounded-lg p-1 w-full" type="date" v-model="filterDto.toDate">
                    </div>
                    <div class="mt-4">
                        <div>Mức giá từ (VND/h)</div>
                        <input class="border rounded-lg p-1 w-full" type="number" placeholder="Nhập mức giả rẻ nhất"
                            v-model="filterDto.fromPrice">
                    </div>
                    <div class="mt-4">
                        <div>Đến mức giá (VND/h)</div>
                        <input class="border rounded-lg p-1 w-full" type="number" placeholder="Nhập mức giá mắc nhất"
                            v-model="filterDto.toPrice">
                    </div>
                    <div class="mt-4">
                        <div>Hình thức</div>
                        <select class="border rounded-lg p-1 w-full" v-model="filterDto.method">
                            <option :value="0">Tất cả</option>
                            <option value="Online">Online</option>
                            <option value="Offline">Offline</option>
                        </select>
                    </div>
                    <div class="flex justify-center mt-4" @click="fetchData">
                        <button class="bg-blue-500 text-white font-bold p-2 rounded-lg">Áp dụng</button>
                    </div>
                </div>

            </div>
            <div class="w-3/4 px-4 py-2">
                <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                    <div v-for="class_ in classes" :key="class_.id" class="shadow-md rounded-lg">
                        <div :class="getStatusStyleHeader(class_.status)">
                            {{ class_.name }}
                        </div>
                        <div class="p-4 h-72 overflow-y-auto">
                            <ul class="px-4 flex flex-col gap-2">
                                <li>
                                    <span class="font-bold">Gia sư :</span>
                                    <button class="ml-3 font-bold text-blue-400 underline" @click="this.$router.push('/tutor-guest/' + class_.tutor.id + '/profile')">
                                        {{ (class_.tutor.firstName ?? "") ?? (class_.tutor.lastName ?? "") }}
                                    </button>
                                </li>
                                <li>
                                    <span class="font-bold">Thời gian :</span>
                                    <span class="ml-3">
                                        {{ (class_.startTime?.substring(0, 10)) ?? "" }} đến
                                        {{ (class_.endTime?.substring(0, 10)) ?? "" }}
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
                                <li>
                                    <span class="font-bold">Giá cả :</span>
                                    <span class="ml-3 text-red-500 font-bold">
                                        {{ class_.tutor.tutorFeePerHour.toLocaleString('vi-VN', {
                                style: 'currency',
                                currency: 'VND',
                            }) }} / h
                                    </span>
                                </li>
                            </ul>
                        </div>
                        <div class="flex justify-center my-2">
                            <button class="bg-blue-500 text-white font-bold p-2 rounded-lg" @click="toggleClassDetail(class_.id)">Xem thêm</button>
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
        </div>
        <div v-else>
            <button class="ml-8 px-8 py-2 bg-blue-400 font-bold text-white rounded-lg" @click="toggleClassDetail">Trở về</button>
            <class-detail-popup :classId="selectedId" :isGuest="true"></class-detail-popup>
            
        </div>

    </div>
</template>

<script>
import axios from 'axios'
import ClassDetailPopup from '../../components/StudentProfile/ClassDetailPopup.vue'
export default {
  components: { ClassDetailPopup },
    name: "ClassesPage",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            classes: [

            ],
            subjects: [

            ],
            filterDto: {
                className: "",
                subject: 0,
                tutorName: "",
                address: "",
                fromDate: null,
                toDate: null,
                fromPrice: null,
                toPrice: null,
                method: 0
            },
            isShowClassDetail : false,
            selectedId : 0
        }
    },
    methods: {
        async refresh() {
            await this.fetchSubject()
            await this.fetchData()
        },
        async fetchData() {
            let query = {
                "Filter.Name": this.filterDto.className ?? "",
                "Filter.UserName": this.filterDto.tutorName,
                "Filter.Address": this.filterDto.address,
                Sorts: {
                },
                Page: this.currentPage,
                Limit: this.pageSize
            }
            let queryStr = this.jsonToQueryString(query)
            if (this.filterDto.fromDate) {
                queryStr += "&Filter.StartTime=" + this.filterDto.fromDate
            }
            if (this.filterDto.toDate) {
                queryStr += "&Filter.EndTime=" + this.filterDto.toDate
            }
            if (this.filterDto.fromPrice) {
                queryStr += "&Filter.MinFeePerHour=" + this.filterDto.fromPrice
            }
            if (this.filterDto.toPrice) {
                queryStr += "&Filter.MaxFeePerHour=" + this.filterDto.toPrice
            }
            if (this.filterDto.method != 0) {
                queryStr += "&Filter.Method=" + this.filterDto.method
            }
            if (this.filterDto.subject != 0) {
                queryStr += "&Filter.SubjectId=" + this.filterDto.subject
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/Class?' +
                queryStr, {
                headers: {
                    "Authorization": "Bearer " + localStorage.token
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
        async fetchSubject() {
            let query = {
                Sorts: {
                    column: "Id",
                    isDesc: true
                },
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/subject?' +
                this.jsonToQueryString(query))
            if (response.data) {
                this.subjects = response.data.items
            }
        },
        toggleClassDetail(id = 0){
            this.selectedId = id
            this.isShowClassDetail = !this.isShowClassDetail
        },
    },
    mounted() {
        this.refresh()
    }
}
</script>

<style></style>
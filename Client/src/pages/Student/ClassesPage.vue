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
                <class-list :classes="classes" :fetch="fetchData" :currentUser="null" :toggleClassDetailPopup="toggleClassDetail" :handlePageChange="handlePageChange" :movePage="movePage" :pageModel="{total : totalPage, page : currentPage}"></class-list>
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
import ClassList from '../../components/common/ClassList.vue'
export default {
  components: { ClassDetailPopup, ClassList },
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
            selectedId : 0,
            user : null
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
        async handlePageChange() {
            if (this.currentPage > this.totalPage) {
                this.currentPage = this.totalPage
            }
            if (this.currentPage < 1) {
                this.currentPage = 1
            }
            await this.fetchData()
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
    },
    mounted() {
        this.refresh()
    }
}
</script>

<style></style>
<template>
    <div>
        <div class="py-8 bg-blue-200" >
            <div class="text-center text-3xl font-bold">Danh sách gia sư</div>
            <div class="flex justify-center mt-4">
                <input class="p-2 w-96 rounded-l-2xl bg-slate-100" placeholder="Nhập tên để tìm kiếm..." v-model="filterDto.tutorName">
                <button @click="fetchData" class="py-2 px-4 rounded-r-2xl bg-slate-400"><i class="fa fa-search"></i></button>
            </div>
        </div>
        <div class="p-4 flex gap-4">
            <div class="w-1/4 border-r-2">
                <div class="font-bold text-xl">Bộ lọc</div>
                <div class="border rounded-lg p-2 mt-4 mr-2 shadow-lg">
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
                        <div>Từ độ tuổi</div>
                        <input class="border rounded-lg p-1 w-full" type="number" placeholder="Nhập độ tuổi thấp nhất"
                            v-model="filterDto.fromAge">
                    </div>
                    <div class="mt-4">
                        <div>Đến độ tuổi</div>
                        <input class="border rounded-lg p-1 w-full" type="number" placeholder="Nhập độ tuổi cao nhất"
                            v-model="filterDto.toAge">
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
                        <div>Giới tính</div>
                        <select class="border rounded-lg p-1 w-full" v-model="filterDto.gender">
                            <option :value="-1">Tất cả</option>
                            <option :value="1">Nam</option>
                            <option :value="0">Nữ</option>
                            <option :value="2">Khác</option>
                        </select>
                    </div>
                    <div class="mt-4">
                        <div>Môn dậy</div>
                        <div class="flex ">
                            <div class="flex flex-wrap gap-2 w-96">
                                <div v-for="subject in selectedSubjects" :key="subject.id" class="p-1 border rounded-xl"
                                    :style="{ 'border-color': subject.color }">
                                    {{ subject.name }}
                                    <button @click="removeSubject(subject.id)">
                                        <i class="fa fa-remove ml-2"></i>
                                    </button>
                                </div>
                                <button class="p-1 border rounded-xl" @click.stop="toggleSubjectPopup">
                                    <span>Thêm môn dậy</span>
                                    <i class="fa fa-plus ml-2"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                    <div class="flex justify-center mt-4">
                        <button class="bg-blue-500 text-white font-bold p-2 rounded-lg" @click="fetchData">Áp
                            dụng</button>
                    </div>
                </div>

            </div>
            <div class="w-3/4 px-4 py-2">
                <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                    <div v-for="tutor in tutors" :key="tutor.id" class="shadow-md rounded-lg">
                        <div class="flex justify-center">
                            <img :src="tutor.avatarImageUrl ?? '/src/assets/noavatar.jpg'" class="w-48 h-48">
                        </div>
                        <div class="text-xl font-bold text-center">
                            {{ tutor.fullName }}
                        </div>
                        <div class="flex justify-center gap-2">
                            <i class="flex items-center fa fa-map-marker"></i>
                            <span>{{ tutor.address }}</span>
                        </div>
                        <div class="text-center italic">
                            {{ tutor.dob ? (new Date().getFullYear() - new Date(tutor.dob).getFullYear()) : "" }} tuổi
                        </div>
                        <div class="flex justify-center gap-2" v-html="displaySubjects(tutor.tutorSubjects)">

                        </div>
                        <div class="text-xl font-bold text-blue-300 text-center">
                            {{ tutor.tutorFeePerHour?.toLocaleString('vi-VN', {
                                style: 'currency',
                                currency: 'VND',
                            }) }} / giờ
                        </div>
                        <star-rating class="flex justify-center" :star-size="20" :rating="tutor.rating"
                            :round-start-rating="false" :read-only="true" />
                        <div class="flex justify-center my-2">
                            <button @click="this.$router.push('/tutor-guest/' + tutor.id + '/profile')" class="bg-blue-500 text-white font-bold p-2 rounded-lg">Xem thêm</button>
                        </div>
                    </div>
                </div>
                <div class="flex gap-4 justify-center mt-4" v-if="tutors.length > 0">
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
            <generic-popup v-if="isOpenSubjectPopup" title="Chọn môn học" :closeFunction="toggleSubjectPopup">
                <subject-list-for-filter-popup :close="toggleSubjectPopup" :selectFunction="selectSubject" />
            </generic-popup>
        </div>
    </div>
</template>

<script>
import axios from 'axios'
import StarRating from 'vue-star-rating'
import GenericPopup from '../../components/common/GenericPopup.vue'
import SubjectListForFilterPopup from '../../components/Operators/SubjectListForFilterPopup.vue'

export default {
    name: "TutorPage",
    components: { StarRating, GenericPopup, SubjectListForFilterPopup },
    data() {
        return {
            totalPage: 100,
            pageSize: 12,
            currentPage: 1,
            isOpenSubjectPopup: false,
            tutors: [

            ],
            subjects: [

            ],
            selectedSubjects: [],
            filterDto: {
                gender: -1,
                subject: 0,
                tutorName: "",
                address: "",
                fromAge: null,
                toAge: null,
                fromPrice: null,
                toPrice: null,
            }
        }
    },
    methods: {
        toggleSubjectPopup() {
            this.isOpenSubjectPopup = !this.isOpenSubjectPopup
        },
        selectSubject(id, name) {
            const existedSubject = this.selectedSubjects.find(s => s.id == id)
            if (!existedSubject) {
                const randomHex = Math.floor(Math.random() * 0xFFFFFF).toString(16).padStart(6, '0');
                this.selectedSubjects.push({
                    id: id,
                    name: name,
                    color: `#${randomHex}`
                })
            }
        },
        getStatusStyleHeader(status) {
            let general = "font-bold text-center py-4 rounded-t-lg text-white"
            switch (status) {
                case "NotYet":
                    return general + " bg-cyan-500"
                case "OnGoing":
                    return general + " bg-green-400"
                default:
                    return general + " bg-gray-400"
            }
        },
        getStatusStyle(status) {
            let general = "ml-3 rounded-lg px-3 py-1 font-bold"
            switch (status) {
                case "NotYet":
                    return general + " text-blue-400"
                case "OnGoing":
                    return general + " text-green-400"
                default:
                    return general + " text-gray-400"
            }
        },
        getStatusDisplay(status) {
            switch (status) {
                case "NotYet":
                    return "Sắp bắt đầu"
                case "OnGoing":
                    return "Đang diễn ra"
                case "Finished":
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
        async fetchData() {
            try {
                let query = {
                    Name: this.filterDto.tutorName,
                    Address: this.filterDto.address,
                    Sorts: {
                        column: "Id",
                        isDesc: true
                    },
                    Page: 0,
                    Limit: 12
                }
                if (this.filterDto.fromAge != null) {
                    var date = new Date(new Date().getFullYear() - this.filterDto.fromAge, 11, 31)
                    query.DobToDate = this.toSqlDateString(date)
                }
                if (this.filterDto.toAge != null) {
                    var date = new Date(new Date().getFullYear() - this.filterDto.toAge, 0, 1)
                    query.DobFromDate = this.toSqlDateString(date)
                }
                if (this.filterDto.fromPrice != null) {
                    query.FeeFrom = this.filterDto.fromPrice
                }
                if (this.filterDto.toPrice != null) {
                    query.FeeTo = this.filterDto.toPrice
                }
                if (this.filterDto.gender != -1) {
                    query.Sex = this.filterDto.gender
                }
                let queryStr = this.jsonToQueryString(query)
                if (this.selectedSubjects.length > 0) {
                    for (var s of this.selectedSubjects) {
                        queryStr += "&Subject=" + s.id
                    }
                }
                const response = await axios.get(import.meta.env.VITE_API_URL + '/api/User/view-tutor-list?tutorStatus=3&' + queryStr)
                if (response.data) {
                    this.tutors = response.data.data.items
                    this.totalPage = Math.ceil(response.data.data.total / this.pageSize)
                }
            } catch (e) {
                console.log(e)
            }
        },
        displaySubjects(subjects) {
            let html = ""
            for (var subject of subjects) {
                if (subject.status != 3) {
                    continue;
                }
                var style = `rounded-lg text-sm py-1 px-4 border border-blue-300 text-blue-300`
                html += `<span class="${style}">${subject.subject.name}</span>`
            }
            return html
        },
        removeSubject(id) {
            this.selectedSubjects = this.selectedSubjects.filter(s => s.id != id)
        },
    },
    mounted() {
        this.fetchData();
    }
}
</script>

<style></style>
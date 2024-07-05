<template>
    <div class="p-4 w-full" @click="setSelectId(0)">
        <div class="flex justify-end gap-2">
            <button class="p-2 font-bold text-blue-400 underline" v-if="filterDto.isChanged" @click="resetFilter">
                Reset bộ lọc
            </button>
            <button class="py-2 px-4 bg-slate-500 hover:bg-slate-300 text-white font-bold rounded-lg"
                @click="toggleFilterPopup">
                <i class="fa fa-search	"></i> Filter
            </button>
        </div>
        <table id="operator-table">
            <thead>
                <tr>
                    <th class="w-2/12">Tên</th>
                    <th class="w-2/12">Ảnh</th>
                    <th class="w-3/12">Dạy môn</th>
                    <th class="w-2/12">Email</th>
                    <th class="w-2/12">SDT</th>
                    <th class="w-2/12">Trạng thái</th>
                    <th class="w-1/12"></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="tutor in tutors" :key="tutor.id">
                    <td><button class="w-32 break-words font-bold underline text-blue-400">{{ tutor.fullName }}</button></td>
                    <td><img :src="tutor.avatar" class="w-24 h-24"></td>
                    <td>
                        <div class="flex flex-wrap gap-1" v-html="displaySubjects(tutor.subject)"></div>
                    </td>
                    <td class="break-all">{{ tutor.email }}</td>
                    <td>{{ tutor.phone }}</td>
                    <td>
                        <div :class="getStatusStyle(tutor.status)">{{ tutor.status }}</div>
                    </td>
                    <td class="relative">
                        <button class="p-2 bg-slate-200 hover:bg-slate-400 font-bold rounded-full"
                            @click.stop="setSelectId(tutor.id)">
                            <i class="fa fa-ellipsis-h	"></i>
                        </button>
                        <div v-if="selectId == tutor.id"
                            class="absolute right-0 bg-white rounded-lg shadow-lg z-10 w-48 animate-fade-down animate-duration-[400ms] animate-normal font-bold flex flex-col">
                            <!-- Content of your menu -->
                            <button class="hover:bg-slate-200 p-2 rounded-t-lg text-left">
                                <i class="fa fa-user mr-4"></i>Xem hồ sơ
                            </button>
                            <!-- <li class="hover:bg-slate-200 p-2"></li> -->
                            <button v-if="tutor.status == 'Active'"
                                class="hover:bg-slate-200 p-2 rounded-b-lg text-left text-red-500">
                                <i class="fa fa-remove mr-4"></i>Đình chỉ
                            </button>
                            <button v-if="tutor.status == 'Fired'"
                                class="hover:bg-slate-200 p-2 rounded-b-lg text-left  text-green-500">
                                <i class="fa fa fa-check mr-4"></i>Kích hoạt
                            </button>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="flex gap-4 justify-center mt-4" v-if="this.tutors.length > 0">
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
        <generic-popup v-if="isOpenFilterPopup" :closeFunction="toggleFilterPopup" title="Bộ lọc gia sư"
            :notOverflow="true">
            <tutor-filter-popup :close="toggleFilterPopup" :filterDto="filterDto" :action="handleFilter" />
        </generic-popup>
    </div>
</template>

<script>
import axios from 'axios'
import GenericPopup from '../common/GenericPopup.vue'
import TutorFilterPopup from './TutorFilterPopup.vue'
export default {
    components: { GenericPopup, TutorFilterPopup },
    name: "AdminTutorList",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            selectId: 0,
            isShowPopup: false,
            isOpenFilterPopup: false,
            tutors: [
               
            ],
            filterDto: {
                gender: "All",
                status: "All",
                name: "",
                email: "",
                phone: "",
                address: "",
                fromDob: "",
                toDob: "",
                fromJoinDate : "",
                toJoinDate : "",
                selectedSubjects: [],
                isChanged : false
            }
        }
    },
    methods: {
        async fetchData(){
            let query = {
                Name : this.filterDto.name,
                Email : this.filterDto.email,
                Phone : this.filterDto.phone,
                Address : this.filterDto.address,
                DobFromDate : this.filterDto.fromDob ?? "",
                DobToDate : this.filterDto.toDob ?? "",
                JoinFromDate : this.filterDto.fromJoinDate ?? "",
                JoinToDate : this.filterDto.toJoinDate ?? "",
                Page: this.currentPage,
                Limit: this.pageSize
            }
            if (this.filterDto.gender != "All") {
                query['Sex'] = this.filterDto.gender
            }
            if (this.filterDto.status != "All") {
                query['Status'] = this.filterDto.status
            }
            if (this.filterDto.selectedSubjects.length > 0) {
                query['Subject'] = this.filterDto.selectedSubjects
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/User/view-tutor-list?'+ 
            this.jsonToQueryString(query))
            if (response.data) {
                this.tutors = response.data.data.items
                this.totalPage = Math.ceil(response.data.data.total / this.pageSize)
            }
        },
        async resetFilter(){
            this.filterDto = {
                gender: "All",
                status: "All",
                name: "",
                email: "",
                phone: "",
                address: "",
                fromDob: null,
                toDob: null,
                fromJoinDate : null,
                toJoinDate : null,
                selectedSubjects: [],
                isChanged : false
            }
            await this.fetchData()
        },
        async handleFilter(filterDto, selectedSubjects) {
            console.log(filterDto)
            this.filterDto = JSON.parse(JSON.stringify(filterDto));
            this.filterDto.selectedSubjects = selectedSubjects
            await this.fetchData()
        },
        toggleFilterPopup() {
            this.isOpenFilterPopup = !this.isOpenFilterPopup
        },
        clearSelectId() {
            if (this.isShowPopup) {
                this.selectId = 0
                this.isShowPopup = false
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
        displaySubjects(subjects) {
            let color = "gray"
            let html = ""
            for (var subject of subjects) {
                switch (subject) {
                    case "Toán":
                        color = "border-orange-400"
                        break;
                    case "Tiếng Anh":
                        color = "border-green-400"
                        break;
                    case "Tiếng Nhật":
                        color = "border-pink-400"
                        break;
                }
                var style = `rounded-lg py-2 px-6 border ${color}`
                html += `<span class="${style}">${subject}</span>`
            }
            return html
        },
        getStatusStyle(status) {
            let css = "text-center font-bold text-white rounded-lg p-1"
            switch (status) {
                case 0:
                    return css + " bg-green-400"
                case 1:
                    return css + " bg-gray-400"
                case 2:
                    return css + " bg-red-400"
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
    },
    mounted() {
        this.fetchData()
    }
}
</script>

<style scoped>
/* .slide-down-enter-active {
  transition: transform 0.3s ease-out;
}
.slide-down-leave-active {
  transition: transform 0.3s ease-in;
}
.slide-down-enter, .slide-down-leave-to {
  transform: translateY(-50%);
} */
</style>
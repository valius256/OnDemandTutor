<template>
    <div class="p-4 w-full">
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
                    <th class="w-1/12">Id</th>
                    <th class="w-3/12">Tên</th>
                    <th class="w-2/12">Ảnh</th>
                    <th class="w-3/12">Đăng ký môn</th>
                    <th class="w-2/12">Ngày tạo</th>
                    <th class="w-1/12"></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="registration in registrations" :key="registration.id">
                    <td>{{ registration.id }}</td>
                    <td>
                        <button @click="this.$router.push('/tutor-guest/' + registration.user.id + '/profile')" class="font-bold underline text-blue-400">
                            {{ (registration.user.firstName ?? "") + " " + (registration.user.lastName ?? "") }}
                        </button>
                    </td>
                    <td><img :src="registration.user.avatarImageUrl" class="w-24 h-24"></td>
                    <td>
                        <div class="flex flex-wrap gap-1" v-html="displaySubjects(registration.subject.name)"></div>
                    </td>
                    <td>{{ this.beautifyDatetime(registration.createdDate) }}</td>
                    <td class="relative">
                        <router-link :to="`/admin/subjects/detail/${registration.id}`">
                            <div class="p-2 bg-blue-400 hover:bg-blue-200 text-white font-bold rounded-lg">
                                Xem xét
                            </div>
                        </router-link>

                    </td>
                </tr>
            </tbody>
        </table>
        <div class="flex gap-4 justify-center mt-4" v-if="this.registrations.length > 0">
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
        <generic-popup v-if="isOpenFilterPopup" :closeFunction="toggleFilterPopup" title="Bộ lọc đăng ký môn"
            :notOverflow="true">
            <subject-registration-filter-popup :close="toggleFilterPopup" :filterDto="filterDto"
                :action="handleFilter" />
        </generic-popup>
    </div>
</template>

<script>
import axios from 'axios'
import GenericPopup from '../common/GenericPopup.vue'
import SubjectRegistrationFilterPopup from './SubjectRegistrationFilterPopup.vue'
export default {
    components: { GenericPopup, SubjectRegistrationFilterPopup },
    name: "SubjectRegistrationList",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            selectId: 0,
            isShowPopup: false,
            isOpenFilterPopup: false,
            registrations: [
                
            ],
            filterDto: {
                name: "",
                fromCreateDate: null,
                toCreateDate: null,
                selectedSubjects: [],
                isChanged: false
            }
        }
    },
    methods: {
        async resetFilter() {
            this.filterDto = {
                name: "",
                fromCreateDate: null,
                toCreateDate: null,
                selectedSubjects: [],
                isChanged: false
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
        displaySubjects(subject) {
            let color = "gray"
            let html = ""
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
            return html
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
            let query = {
                "Filter.TutorName": this.filterDto.name,
                "Filter.CreateFrom": this.filterDto.fromCreateDate ?? "",
                "Filter.CreateTo": this.filterDto.toCreateDate ?? "",
                "Filter.Status" : 0,
                Page: this.currentPage,
                Limit: this.pageSize,
                Sorts : {
                    column : "Id",
                    isDesc : true
                }
            }
            let queryStr = this.jsonToQueryString(query)
            console.log(this.filterDto.selectedSubjects)
            if (this.filterDto.selectedSubjects.length > 0) {
                for (var s of this.filterDto.selectedSubjects){
                    queryStr += "&Filter.SubjectIds=" + s.id
                }
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/TutorSubject?' +
                queryStr)
            if (response.data) {
                this.registrations = response.data.items
                this.totalPage = Math.ceil(response.data.total / this.pageSize)
            }
        },
    },
    mounted() {
        this.fetchData()
    }
}
</script>
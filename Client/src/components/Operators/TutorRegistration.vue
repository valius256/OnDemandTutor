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
        <table id="operator-table" class="table-auto overflow-x-auto">
            <thead>
                <tr>
                    <th class="w-1/12">Id</th>
                    <th class="w-2/12">Tên</th>
                    <th class="w-2/12">Ảnh</th>
                    <th class="w-2/12">Email</th>
                    <th class="w-2/12">SDT</th>
                    <th class="w-1/12">Tạo ngày</th>
                    <th class="w-2/12">Hành động</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="tutor in tutors" :key="tutor.id">
                    <td>{{ tutor.id }}</td>
                    <td><button @click="this.$router.push('/tutor-guest/' + tutor.id + '/profile')" class="w-32 break-words font-bold underline text-blue-400">{{ tutor.fullName }}</button>
                    </td>
                    <td><img :src="tutor.avatarImageUrl" class="w-24 h-24"></td>
                    <td class="break-all">{{ tutor.email }}</td>
                    <td>{{ tutor.phone }}</td>
                    <td>{{ this.beautifyDatetime(tutor.createdDate) }}</td>
                    <td class="flex flex-col gap-2">
                        <button class="text-white rounded-lg bg-blue-500 hover:bg-blue-200 font-bold text-lg p-2"
                            @click="this.$router.push('/tutor-guest/' + tutor.id + '/profile')">
                            Chi Tiết
                        </button>
                        <button class="text-white rounded-lg bg-lime-500 hover:bg-lime-200 font-bold text-lg p-2"
                            @click="handleApprove({ confirmation: true, id: tutor.id, isApprove: true })">
                            Duyệt
                        </button>
                        <button class="text-white rounded-lg bg-red-500 hover:bg-red-200 font-bold text-lg p-2"
                            @click="handleApprove({ confirmation: true, id: tutor.id, isApprove: false })">
                            Từ chối
                        </button>
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
        <generic-popup v-if="isOpenFilterPopup" :closeFunction="toggleFilterPopup" title="Bộ lọc gia sư">
            <tutor-registration-filter-popup :filter-dto="filterDto" :action="handleFilter"
                :close="toggleFilterPopup" />
        </generic-popup>
    </div>
</template>

<script>
import axios from 'axios';
import GenericPopup from '../common/GenericPopup.vue';
import TutorRegistrationFilterPopup from './TutorRegistrationFilterPopup.vue';
export default {
    components: { GenericPopup, TutorRegistrationFilterPopup },
    inject: ['eventBus'],
    name: "TutorRegistration",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            isOpenFilterPopup: false,
            tutors: [
            ],
            filterDto: {
                gender: "All",
                name: "",
                email: "",
                phone: "",
                address: "",
                fromDob: null,
                toDob: null,
                fromJoinDate: null,
                toJoinDate: null,
                isChanged: false,
            }
        }
    },
    methods: {
        async fetchData() {
            let query = {
                Name: this.filterDto.name,
                Email: this.filterDto.email,
                Phone: this.filterDto.phone,
                Address: this.filterDto.address,
                DobFromDate: this.filterDto.fromDob ?? "",
                DobToDate: this.filterDto.toDob ?? "",
                JoinFromDate: this.filterDto.fromJoinDate ?? "",
                JoinToDate: this.filterDto.toJoinDate ?? "",
                TutorStatus: 1,
                Page: this.currentPage,
                Limit: this.pageSize,
            }
            if (this.filterDto.gender != "All") {
                query['Sex'] = this.filterDto.gender
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/User/view-tutor-list?TutorsStatus=1' +
                this.jsonToQueryString(query))
            if (response.data) {
                this.tutors = response.data.data.items
                this.totalPage = Math.ceil(response.data.data.total / this.pageSize)
            }
        },
        async resetFilter() {
            this.filterDto = {
                gender: "All",
                name: "",
                email: "",
                phone: "",
                address: "",
                fromDob: null,
                toDob: null,
                fromJoinDate: null,
                toJoinDate: null,
                isChanged: false
            }
            await this.fetchData()
        },
        async handleFilter(filterDto) {
            this.filterDto = JSON.parse(JSON.stringify(filterDto));
            await this.fetchData()
        },
        toggleFilterPopup() {
            this.isOpenFilterPopup = !this.isOpenFilterPopup
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
        async handleApprove(option) {
            if (option.confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: `Bạn có chắc chắn muốn ${option.isApprove ? "đồng ý duyệt" : "từ chối duyệt"} tài khoản này?`,
                    method: this.handleApprove,
                    params: { confirmation: false, id: option.id, isApprove: option.isApprove }
                })
            } else {
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    const request = {
                        id: option.id,
                        status: option.isApprove ? 3 : 2
                    }
                    await axios.patch(import.meta.env.VITE_API_URL + '/api/User/change-status', request, {
                        headers: {
                            "Authorization": "Bearer " + localStorage.token
                        }
                    })
                    this.eventBus.emit("open-result-dialog", {
                        message: "Cập nhật thành công",
                        type: "Success"
                    })
                    await this.fetchData()
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Đã gặp sự cố. Vui lòng thử lại sau",
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        }
    },
    mounted() {
        this.fetchData()
    }
}
</script>

<style></style>
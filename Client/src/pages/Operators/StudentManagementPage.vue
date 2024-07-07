<template>

    <div class="p-4 w-full" @click="setSelectId(0)">
        <div v-if="!isShowProfile">
            <div class="text-2xl font-bold">
                Quản lý học sinh
            </div>
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
                        <th class="w-2/12">Tên</th>
                        <th class="w-2/12">Email</th>
                        <th class="w-2/12">SDT</th>
                        <th class="w-2/12">Tham gia ngày</th>
                        <th class="w-2/12">Địa chỉ</th>
                        <th class="w-2/12">Trạng thái</th>
                        <th class="w-1/12"></th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="student in students" :key="student.id">
                        <td>{{ student.id }}</td>
                        <td><button class="font-bold underline text-blue-400 break-words w-32">{{ student.firstName + " " + student.lastName }}</button></td>
                        <td class="break-all">{{ student.email }}</td>
                        <td>{{ student.phone }}</td>
                        <td>{{ this.beautifyDatetime(student.createdDate) }}</td>
                        <td>{{ student.address }}</td>
                        <td>
                            <div v-if="student.isActive" :class="getStatusStyle(student.isActive)">Hoạt động</div>
                            <div v-else :class="getStatusStyle(student.isActive)">Đình chỉ</div>
                        </td>
                        <td class="relative">
                            <button class="p-2 bg-slate-200 hover:bg-slate-400 font-bold rounded-full"
                                @click.stop="setSelectId(student.id)">
                                <i class="fa fa-ellipsis-h	"></i>
                            </button>
                            <div v-if="selectId == student.id"
                                class="absolute right-0 bg-white rounded-lg shadow-lg z-10 w-48 animate-fade-down animate-duration-[400ms] animate-normal font-bold flex flex-col">
                                <!-- Content of your menu -->
                                <button class="hover:bg-slate-200 p-2 rounded-t-lg text-left"
                                    @click="accessProfile(student.id)">
                                    <i class="fa fa-user mr-4"></i>Xem hồ sơ
                                </button>
                                <!-- <li class="hover:bg-slate-200 p-2"></li> -->
                                <button v-if="student.isActive"
                                    class="hover:bg-slate-200 p-2 rounded-b-lg text-left text-red-500"
                                    @click="toggleReasonPopup(student.id)">
                                    <i class="fa fa-remove mr-4"></i>Đình chỉ
                                </button>
                                <button v-else class="hover:bg-slate-200 p-2 rounded-b-lg text-left  text-green-500"
                                    @click="handleActivate({ confirmation: true, id: student.id })">
                                    <i class="fa fa fa-check mr-4"></i>Kích hoạt
                                </button>
                            </div>
                        </td>

                    </tr>
                </tbody>
            </table>
            <div class="flex gap-4 justify-center mt-4" v-if="this.students.length > 0">
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
            <button @click="isShowProfile = false"
                class="bg-blue-400 rounded-lg px-8 py-4 hover:bg-blue-200 font-bold mb-4 text-white">
                Trở lại
            </button>
            <div class="my-4 bg-red-300 rounded-lg w-full p-4 flex gap-4" v-if="!profileStatus">
                <i class="fa fa-warning	text-5xl"></i>
                <div class="">
                    <div class="font-bold text-xl">Tài khoản này đã bị vô hiệu hóa</div>
                    <div>
                        <span class="font-bold">Lý do : </span> {{ profileDeactiveReason }}
                    </div>
                </div>
            </div>
            <profile :id="activatingId"></profile>
        </div>
        <generic-popup v-if="isOpenFilterPopup" title="Bộ lọc học viên" :closeFunction="toggleFilterPopup">
            <student-filter-popup :close="toggleFilterPopup" :filterDto="filterDto" :action="handleFilter" />
        </generic-popup>
        <generic-popup v-if="isOpenReasonPopup" title="Cung cấp lý do vô hiệu hóa" :closeFunction="toggleReasonPopup">
            <account-deactivate-reason-popup :close="toggleReasonPopup" :id="activatingId" :action="fetchData" />
        </generic-popup>
    </div>

</template>

<script>
import axios from 'axios';
import GenericPopup from '../../components/common/GenericPopup.vue';
import StudentFilterPopup from '../../components/Operators/StudentFilterPopup.vue';
import AccountDeactivateReasonPopup from '../../components/Operators/AccountDeactivateReasonPopup.vue';
import Profile from '../../components/StudentProfile/Profile.vue';
export default {
    name: "StudentManagementPage",
    inject: ['eventBus'],
    components: { GenericPopup, StudentFilterPopup, AccountDeactivateReasonPopup, Profile },
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            selectId: 0,
            activatingId: 0,
            isOpenFilterPopup: false,
            isOpenReasonPopup: false,
            isShowProfile: false,
            profileStatus : true,
            profileDeactiveReason : "",
            students: [],
            filterDto: {
                gender: "All",
                status: "All",
                name: "",
                email: "",
                phone: "",
                address: "",
                fromDob: "",
                toDob: "",
                fromJoinDate: "",
                toJoinDate: "",
                isChanged: false
            },
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
                Role: 0,
                Page: this.currentPage,
                Limit: this.pageSize
            }
            if (this.filterDto.gender != "All") {
                query['Sex'] = this.filterDto.gender
            }
            if (this.filterDto.status != "All") {
                query['IsActive'] = this.filterDto.status
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/User/all?' +
                this.jsonToQueryString(query), {
                headers: {
                    "Authorization": "Bearer " + localStorage.token
                }
            })
            if (response.data) {
                this.students = response.data.data.items
                this.totalPage = Math.ceil(response.data.data.total / this.pageSize)
            }
        },
        async handleActivate(option) {
            if (option.confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có muốn kích hoạt lại tài khoản này không?",
                    method: this.handleActivate,
                    params: { confirmation: false, id: option.id }
                })
            } else {
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    const request = {
                        id: option.id,
                        isActive: true,
                        deaActiveReason: ""
                    }
                    await axios.patch(import.meta.env.VITE_API_URL + '/api/User/deaactive-account', request, {
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
        },
        async resetFilter() {
            this.filterDto = {
                gender: "All",
                status: "All",
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
        toggleReasonPopup(id) {
            if (id) {
                this.activatingId = id
            }
            this.isOpenReasonPopup = !this.isOpenReasonPopup
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
        getStatusStyle(status) {
            let css = "text-center font-bold text-white rounded-lg p-1"
            switch (status) {
                case true:
                    return css + " bg-green-400"
                case false:
                    return css + " bg-gray-400"
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
        accessProfile(id) {
            this.activatingId = id
            this.isShowProfile = true;
            const profile = (this.students.find(s => s.id == id))
            this.profileStatus = profile?.isActive ?? true
            this.profileDeactiveReason = profile?.deaActiveReason ?? ""
        }
    },
    mounted() {
        this.fetchData()
    }
}
</script>

<style scoped></style>
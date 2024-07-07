<template>

    <div class="p-4 w-full" @click="setSelectId(0)">
        <div class="text-2xl font-bold">
            Quản lý nhân viên vận hành
        </div>
        <div class="flex place-content-between mt-4">
            <button class="py-2 px-4 bg-blue-500 hover:bg-blue-300 text-white font-bold rounded-lg"
                @click="toggleOpenAddPopup">
                <i class="fa fa-plus"></i> Thêm mới tài khoản
            </button>
            <div class="flex gap-2">
                <button class="p-2 font-bold text-blue-400 underline" v-if="filterDto.isChanged" @click="resetFilter">
                    Reset bộ lọc
                </button>
                <button class="py-2 px-4 bg-slate-500 hover:bg-slate-300 text-white font-bold rounded-lg"
                    @click="toggleFilterPopup">
                    <i class="fa fa-search	"></i> Filter
                </button>
            </div>

        </div>
        <table id="operator-table">
            <thead>
                <tr>
                    <th class="w-1/12">Id</th>
                    <th class="w-2/12">Tên</th>
                    <th class="w-2/12">Email</th>
                    <th class="w-2/12">SDT</th>
                    <th class="w-2/12">Tạo ngày</th>
                    <th class="w-2/12">Quyền hạn</th>
                    <th class="w-2/12">Trạng thái</th>
                    <th class="w-1/12"></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="operator in operators" :key="operator.id">
                    <td>{{ operator.id }}</td>
                    <td><div class="w-32 break-words font-bold">{{ operator.firstName + " " + operator.lastName }}</div></td>
                    <td class="break-all">{{ operator.email }}</td>
                    <td>{{ operator.phone }}</td>
                    <td>{{ this.beautifyDatetime(operator.createdDate) }}</td>
                    <td>
                        <div v-if="operator.role == 2" :class="getRoleStyle(operator.role)">
                            Operator
                        </div>
                        <div v-if="operator.role == 3" :class="getRoleStyle(operator.role)">
                            Admin
                        </div>
                    </td>
                    <td>
                        <div v-if="operator.isActive" :class="getStatusStyle(operator.isActive)">Hoạt động</div>
                        <div v-else :class="getStatusStyle(operator.isActive)">Đình chỉ</div>
                    </td>
                    <td class="relative">
                        <button class="p-2 bg-slate-200 hover:bg-slate-400 font-bold rounded-full"
                            @click.stop="setSelectId(operator.id)">
                            <i class="fa fa-ellipsis-h	"></i>
                        </button>
                        <div v-if="selectId == operator.id"
                            class="absolute right-0 bg-white rounded-lg shadow-lg z-10 w-48 animate-fade-down animate-duration-[400ms] animate-normal font-bold flex flex-col">
                            <!-- Content of your menu -->
                            <button class="hover:bg-slate-200 p-2 rounded-t-lg text-left"
                                @click="handleEdit(operator.id)">
                                <i class="fa fa-edit mr-4"></i>Chỉnh sửa
                            </button>
                            <!-- <li class="hover:bg-slate-200 p-2"></li> -->
                            <button v-if="operator.status == 'Active'"
                                class="hover:bg-slate-200 p-2 rounded-b-lg text-left text-red-500">
                                <i class="fa fa-remove mr-4"></i>Đình chỉ
                            </button>
                            <button v-else class="hover:bg-slate-200 p-2 rounded-b-lg text-left  text-green-500">
                                <i class="fa fa fa-check mr-4"></i>Kích hoạt
                            </button>
                        </div>
                    </td>

                </tr>
            </tbody>
        </table>
        <div class="flex gap-4 justify-center mt-4" v-if="this.operators.length > 0">
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
        <generic-popup v-if="isOpenAddPopup" title="Thêm tài khoản vận hành" :closeFunction="toggleOpenAddPopup">
            <operator-edit-add-popup :close="toggleOpenAddPopup" />
        </generic-popup>
        <generic-popup v-if="isOpenEditPopup" title="Chỉnh sửa tài khoản vận hành" :closeFunction="toggleOpenEditPopup">
            <operator-edit-add-popup :close="toggleOpenEditPopup" :editDto="editDto" />
        </generic-popup>
        <generic-popup v-if="isOpenFilterPopup" title="Bộ lọc tài khoản vận hành" :closeFunction="toggleFilterPopup">
            <operator-filter-popup :close="toggleFilterPopup" :action="handleFilter" :filterDto="filterDto" />
        </generic-popup>
    </div>
</template>

<script>
import axios from 'axios'
import GenericPopup from '../../components/common/GenericPopup.vue'
import OperatorEditAddPopup from '../../components/Operators/OperatorEditAddPopup.vue'
import OperatorFilterPopup from '../../components/Operators/OperatorFilterPopup.vue'
export default {
    components: { OperatorEditAddPopup, GenericPopup, OperatorFilterPopup },
    name: "OperatorManagementPage",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            selectId: 0,
            isOpenAddPopup: false,
            isOpenEditPopup: false,
            isOpenFilterPopup: false,
            operators: [
            ],
            filterDto: {
                name: "",
                email: "",
                phone: "",
                isActive: "All",
                role: "All",
                fromJoinDate: null,
                toJoinDate: null,
                isChanged: false
            },
            editDto: {
                name: "",
                phone: "",
                email: "",
                role: ""
            }
        }
    },
    methods: {
        async fetchData() {
            let query = {
                Name : this.filterDto.name,
                Email : this.filterDto.email,
                Phone : this.filterDto.phone,
                JoinFromDate : this.filterDto.fromJoinDate ?? "",
                JoinToDate : this.filterDto.toJoinDate ?? "",
                Page: this.currentPage,
                Limit: this.pageSize
            }
            if (this.filterDto.role != "All") {
                query['Role'] = this.filterDto.role
            }
            if (this.filterDto.isActive != "All") {
                query['IsActive'] = this.filterDto.isActive
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/User/all?'+ 
            this.jsonToQueryString(query),{
                headers : {
                    "Authorization" : "Bearer " + localStorage.token
                }
            })
            if (response.data) {
                this.operators = response.data.data.items
                this.totalPage = Math.ceil(response.data.data.total / this.pageSize)
            }
        },
        async resetFilter() {
            this.filterDto = {
                name: "",
                email: "",
                phone: "",
                isActive: "All",
                role: "All",
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
        handleEdit(id) {
            const operator = this.operators.find(o => o.id == id)
            if (operator != null) {
                this.editDto.name = operator.name,
                    this.editDto.phone = operator.phone,
                    this.editDto.email = operator.email,
                    this.editDto.role = operator.role

                this.toggleOpenEditPopup()
            }
        },
        toggleOpenAddPopup() {
            this.isOpenAddPopup = !this.isOpenAddPopup
        },
        toggleOpenEditPopup() {
            this.isOpenEditPopup = !this.isOpenEditPopup
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
        getRoleStyle(role) {
            let css = "text-center font-bold text-white rounded-lg p-1"
            switch (role) {
                case 3:
                    return css + " bg-blue-400"
                case 2:
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
    },
    mounted() {
        this.fetchData()
    }
}
</script>

<style scoped></style>
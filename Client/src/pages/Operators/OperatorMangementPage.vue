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
            <button class="py-2 px-4 bg-slate-500 hover:bg-slate-300 text-white font-bold rounded-lg"
                @click="handleSearch">
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
                    <th class="w-2/12">Tạo ngày</th>
                    <th class="w-2/12">Quyền hạn</th>
                    <th class="w-2/12">Trạng thái</th>
                    <th class="w-1/12"></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="operator in operators" :key="operator.id">
                    <td>{{ operator.id }}</td>
                    <td><button class="font-bold underline text-blue-400">{{ operator.name }}</button></td>
                    <td class="break-all">{{ operator.email }}</td>
                    <td>{{ operator.phone }}</td>
                    <td>{{ this.sqlDateStringToSlashFormat(operator.joinDate) }}</td>
                    <td>
                        <div :class="getRoleStyle(operator.role)">{{ operator.role }}</div>
                    </td>
                    <td>
                        <div :class="getStatusStyle(operator.status)">{{ operator.status }}</div>
                    </td>
                    <td class="relative">
                        <button class="p-2 bg-slate-200 hover:bg-slate-400 font-bold rounded-full"
                            @click.stop="setSelectId(operator.id)">
                            <i class="fa fa-ellipsis-h	"></i>
                        </button>
                        <div v-if="selectId == operator.id"
                            class="absolute right-0 bg-white rounded-lg shadow-lg z-10 w-48 animate-fade-down animate-duration-[400ms] animate-normal font-bold flex flex-col">
                            <!-- Content of your menu -->
                                <button class="hover:bg-slate-200 p-2 rounded-t-lg text-left" @click="handleEdit(operator.id)">
                                    <i class="fa fa-edit mr-4"></i>Chỉnh sửa
                                </button>
                                <!-- <li class="hover:bg-slate-200 p-2"></li> -->
                                <button v-if="operator.status == 'Active'" class="hover:bg-slate-200 p-2 rounded-b-lg text-left text-red-500">
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
            <operator-edit-add-popup :close="toggleOpenEditPopup" :editDto="editDto"/>
        </generic-popup>
    </div>
</template>

<script>
import GenericPopup from '../../components/common/GenericPopup.vue'
import OperatorEditAddPopup from '../../components/Operators/OperatorEditAddPopup.vue'
export default {
  components: { OperatorEditAddPopup, GenericPopup },
    name: "OperatorManagementPage",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            selectId: 0,
            isOpenAddPopup : false,
            isOpenEditPopup : false,
            operators: [
                {
                    id: 1,
                    name: "Nguyen Van A",
                    email: "abc@gmail.com",
                    phone: "0987654321",
                    joinDate : "2024-01-01",
                    phone: "0987654321",
                    avatar: "/src/assets/noavatar.jpg",
                    status: "Active",
                    role : "Admin"
                },
                {
                    id: 2,
                    name: "Nguyen Van A",
                    email: "abc@gmail.com",
                    phone: "0987654321",
                    joinDate : "2024-01-01",
                    avatar: "/src/assets/noavatar.jpg",
                    status: "Active",
                    role : "Operator"
                },
                {
                    id: 3,
                    name: "Nguyen Van A",
                    email: "abc@gmail.com",
                    phone: "0987654321",
                    joinDate : "2024-01-01",
                    avatar: "/src/assets/noavatar.jpg",
                    status: "Inactive",
                    role : "Operator"
                },
                {
                    id: 4,
                    name: "Nguyen Van A",
                    email: "abc@gmail.com",
                    phone: "0987654321",
                    joinDate : "2024-01-01",
                    avatar: "/src/assets/noavatar.jpg",
                    status: "Inactive",
                    role : "Operator"
                },
            ],
            editDto : {
                name : "",
                phone : "",
                email : "",
                role : ""
            }
        }
    },
    methods: {
        handleEdit(id){
            const operator = this.operators.find(o => o.id == id)
            if (operator != null){
                this.editDto.name = operator.name,
                this.editDto.phone = operator.phone,
                this.editDto.email = operator.email,
                this.editDto.role = operator.role

                this.toggleOpenEditPopup()
            }
        },
        toggleOpenAddPopup(){
            this.isOpenAddPopup = !this.isOpenAddPopup
        },
        toggleOpenEditPopup(){
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
                case "Active":
                    return css + " bg-green-400"
                case "Inactive":
                    return css + " bg-gray-400"
            }
        },
        getRoleStyle(role) {
            let css = "text-center font-bold text-white rounded-lg p-1"
            switch (role) {
                case "Admin":
                    return css + " bg-blue-400"
                case "Operator":
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
    }
}
</script>

<style scoped>
</style>
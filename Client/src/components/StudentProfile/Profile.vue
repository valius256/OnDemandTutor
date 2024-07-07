<template>
    <div v-if="user">
        <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200 ">
            Thông tin cá nhân
        </div>
        <div class="flex justify-end" v-if="user">
            <div class="flex gap-4">
                <button @click="openEditMode" v-if="!editMode && checkOwner()"
                    class="mr-6 p-2 font-bold text-white bg-blue-400 hover:bg-blue-200 rounded-lg">Chỉnh sửa</button>
                <button @click="handleUpdate(true)" v-if="editMode"
                    class="p-2 font-bold text-white bg-green-400 hover:bg-green-200 rounded-lg">Xác nhận</button>
                <button @click="closeEditMode" v-if="editMode"
                    class="mr-6 p-2 font-bold text-white bg-red-400 hover:bg-red-200 rounded-lg">Hủy bỏ</button>
            </div>

        </div>
        <div class="flex gap-8 p-6" v-if="user">
            <div class="flex flex-col items-center">
                <img class="max-w-64 min-w-64 h-64 rounded-full" :src="user.avatar ?? '/src/assets/noavatar.jpg'">
                <button v-if="checkOwner()" class="p-2 font-bold text-white bg-blue-400 hover:bg-blue-200 rounded-lg">Cập nhật ảnh</button>
            </div>
            <table class="ml-4 bg-slate-50 p-6 rounded-xl w-full">
                <tbody v-if="!this.editMode">
                    <tr>
                        <td>Họ</td>
                        <td>{{ user.firstName }}</td>
                    </tr>
                    <tr>
                        <td>Tên</td>
                        <td>{{ user.lastName }}</td>
                    </tr>
                    <tr>
                        <td>Email</td>
                        <td>{{ user.email }}</td>
                    </tr>
                    <tr>
                        <td>Số điện thoại</td>
                        <td>{{ user.phone }}</td>
                    </tr>
                    <tr>
                        <td>Ngày sinh</td>
                        <td>{{ user.dob }}</td>
                    </tr>
                    <tr>
                        <td>Địa chỉ</td>
                        <td>{{ user.address }}</td>
                    </tr>
                    <tr>
                        <td>Giới tính</td>
                        <td>{{ user.sex }}</td>
                    </tr>
                </tbody>
                <tbody v-else>
                    <tr>
                        <td>First Name</td>
                        <td><input class="w-full rounded border border-gray-200 p-1" type="text"
                                v-model="editDto.firstName">
                        </td>
                    </tr>
                    <tr>
                        <td>Last Name</td>
                        <td><input class="w-full rounded border border-gray-200 p-1" type="text"
                                v-model="editDto.lastName">
                        </td>
                    </tr>
                    <tr>
                        <td>Email</td>
                        <td>{{ user.email }}</td>
                    </tr>
                    <tr>
                        <td>Phone</td>
                        <td><input class="w-full rounded border border-gray-200 p-1" type="text"
                                v-model="editDto.phone"></td>
                    </tr>
                    <tr>
                        <td>Date of Birth</td>
                        <td><input class="w-full rounded border border-gray-200 p-1" type="date" v-model="editDto.doB">
                        </td>
                    </tr>
                    <tr>
                        <td>Address</td>
                        <td><input class="w-full rounded border border-gray-200 p-1" type="text"
                                v-model="editDto.address">
                        </td>
                    </tr>
                    <tr>
                        <td>Gender</td>
                        <td>
                            <select class="w-full rounded border border-gray-200 p-1" v-model="editDto.gender">
                                <option :value="1">Male</option>
                                <option :value="0">Female</option>
                                <option :value="2">Other</option>
                            </select>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>


    </div>
</template>

<script>
import axios from 'axios'

export default {
    name: "StudentProfile",
    inject : ['eventBus'],
    props : ['id'],
    data() {
        return {
            user: null,
            loginedUser: null,
            editDto: {
                firstName: "",
                lastName: "",
                phone: "",
                doB: "",
                address: "",
                gender: 0,
            },
            editMode: false,

        }
    },
    methods: {
        closeEditMode() {
            this.editMode = false
        },
        openEditMode() {
            this.editMode = true
            this.editDto.firstName = this.user.firstName
            this.editDto.lastName = this.user.lastName
            this.editDto.phone = this.user.phone
            this.editDto.doB = this.user.doB
            this.editDto.address = this.user.address
            this.editDto.gender = this.user.gender == "Male" ? 1 : (this.user.gender == "Female" ? 0 : 2)
        },
        async refresh() {
            this.loginedUser = this.getUserFromToken()
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/User/profile?userId=' + this.id, {
                headers: {
                    "Authorization": "Bearer " + localStorage.token
                }
            })
            if (response.data) {
                this.user = response.data.data
            }
        },
        async handleUpdate(confirmation) {
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn cập nhật thông tin hồ sơ?",
                    method: this.handleUpdate,
                    params: false
                })
            } else {
                const request = {
                    id : this.user.id,
                    firstName: this.editDto.firstName,
                    lastName: this.editDto.lastName,
                    address: this.editDto.phone,
                    sex: this.editDto.gender,
                    dob: this.editDto.dob,
                }
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    await axios.post(import.meta.env.VITE_API_URL + '/api/User/update-profile', request,{
                        headers : {
                            "Authorization" : "Bearer " + localStorage.token
                        }
                    })
                    this.eventBus.emit("open-result-dialog", {
                        message: "Cập nhật thành công",
                        type: "Success"
                    })
                    await this.refresh()
                    this.closeEditMode()
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Có sự cố xảy ra. Vui lòng thử lại sau",
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        },
        checkOwner(){
            if (this.loginedUser == null || this.user == null)
                return false
            if (this.loginedUser.id == this.user.id)
                return true
            return false;
        }
    },
    mounted() {
        this.refresh()
    }
}
</script>

<style scoped>
tr td,
th {
    padding: 0.5rem 2rem 0.5rem 2rem;
    border: solid 1px #ffffff
}
</style>
<template>
    <div v-if="user">
        <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200 ">
            Thông tin cá nhân
        </div>
        <div class="flex justify-end">
            <div class="flex gap-4">
                <button @click="openEditMode" v-if="!editMode"
                    class="mr-6 p-2 font-bold text-white bg-blue-400 hover:bg-blue-200 rounded-lg">Chỉnh sửa</button>
                <button @click="closeEditMode" v-if="editMode"
                    class="p-2 font-bold text-white bg-green-400 hover:bg-green-200 rounded-lg">Xác nhận</button>
                <button @click="closeEditMode" v-if="editMode"
                    class="mr-6 p-2 font-bold text-white bg-red-400 hover:bg-red-200 rounded-lg">Hủy bỏ</button>
            </div>

        </div>
        <div class="flex gap-8 p-6">
            <div class="flex flex-col items-center">
                <img class="max-w-64 min-w-64 h-64 rounded-full" :src="user.avatar">
                <button class="p-2 font-bold text-white bg-blue-400 hover:bg-blue-200 rounded-lg">Cập nhật ảnh</button>
            </div>
            <table class="ml-4 bg-slate-50 p-6 rounded-xl w-full">
                <tbody v-if="!this.editMode">
                    <tr>
                        <td>First Name</td>
                        <td>{{ user.firstName }}</td>
                    </tr>
                    <tr>
                        <td>Last Name</td>
                        <td>{{ user.lastName }}</td>
                    </tr>
                    <tr>
                        <td>Email</td>
                        <td>{{ user.email }}</td>
                    </tr>
                    <tr>
                        <td>Phone</td>
                        <td>{{ user.phone }}</td>
                    </tr>
                    <tr>
                        <td>Date of Birth</td>
                        <td>{{ user.doB }}</td>
                    </tr>
                    <tr>
                        <td>Address</td>
                        <td>{{ user.address }}</td>
                    </tr>
                    <tr>
                        <td>Gender</td>
                        <td>{{ user.gender }}</td>
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
                                <option value="Male">Male</option>
                                <option value="Female">Female</option>
                                <option value="Other">Other</option>
                            </select>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
       
        
    </div>
</template>

<script>
export default {
    name: "StudentProfile",
    data() {
        return {
            user: {
                id: 1,
                firstName: "Hello",
                lastName: "World",
                email: "fake@gmail.com",
                phone: "0987654321",
                doB: "2003-01-01",
                address: "6969, Kisphophu Kingdom, Earth",
                gender: "Male",
                avatar: "/src//assets/noavatar.jpg",
                balance: 696000
            },
            editDto: {
                firstName: "",
                lastName: "",
                phone: "",
                doB: "",
                address: "",
                gender: "Male",
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
            this.editDto.gender = this.user.gender
        }
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
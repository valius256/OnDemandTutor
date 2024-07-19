<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-x-auto">
        <div class="flex gap-3">
            <div>
                <div class="flex ">
                    <span class="w-16 p-1 font-bold">Họ</span>
                    <input v-model="firstName" class="p-1 border rounded-lg" placeholder="Nhập tên" />
                </div>
                <div class="flex mt-4">
                    <span class="w-16 p-1 font-bold">Tên</span>
                    <input v-model="lastName" class="p-1 border rounded-lg" placeholder="Nhập tên" />
                </div>
                <div class="flex mt-4" v-if="id == 0">
                    <span class="w-16 p-1 font-bold">Email</span>
                    <input v-model="email" class="p-1 border rounded-lg" placeholder="Nhập email" />
                </div>
            </div>
            <div>
                <div class="flex ">
                    <span class="w-24 p-1 font-bold">SDT</span>
                    <input v-model="phone" class="p-1 border rounded-lg" placeholder="Nhập SDT" />
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Quyền</span>
                    <select v-model="role" class="p-1 border rounded-lg" placeholder="Nhập email">
                        <option :value="3">
                            Admin
                        </option>
                        <option :value="2">
                            Operator
                        </option>
                    </select>
                </div>
            </div>
        </div>
        <div class="flex justify-center mt-4 gap-3">
            <button v-if="id == 0" @click="addOperator(true)" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác nhận</button>
            <button v-else @click="editOperator(true)" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác nhận</button>
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>
    </div>
</template>

<script>
import axios from 'axios'
export default {
    name: "OperatorEditAddPopup",
    inject : ['eventBus'],
    props: ['title', 'close', 'editDto','action'],
    data() {
        return {
            id : 0,
            error: "",
            firstName: "",
            lastName: "",
            email: "",
            phone: "",
            role: 2,
        }
    },
    methods: {
        presetEdit() {
            if (this.editDto != null) {
                this.id = this.editDto.id
                this.email = this.editDto.email
                this.role = this.editDto.role
                this.phone = this.editDto.phone
                this.firstName = this.editDto.firstName
                this.lastName = this.editDto.lastName
            }
        },
        refresh() {
            if (this.editDto != null) {
                this.presetEdit();
            }
        },
        generateRandomString(length) {
            const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
            let result = '';
            const charactersLength = characters.length;
            for (let i = 0; i < length; i++) {
                result += characters.charAt(Math.floor(Math.random() * charactersLength));
            }
            return result;
        },
        async addOperator(confirmation) {
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn tạo tài khoản này?",
                    method: this.addOperator,
                    params: false
                })
            } else {
                var randomPass = this.generateRandomString(10)
                const request = {
                    password: randomPass,
                    confirmPassword: randomPass,
                    phone: this.phone,
                    email: this.email,
                    firstName: this.firstName,
                    lastName: this.lastName,
                }
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    const res = await axios.post(import.meta.env.VITE_API_URL + '/api/Auth/register', request)
                    
                    await axios.post(import.meta.env.VITE_API_URL + '/api/Auth/grant-role', {
                        email : this.email,
                        id : res.data.data.id,
                        role : this.role
                    }, {
                        headers : {
                            'Authorization' : "Bearer " + localStorage.token
                        }
                    })
                    await axios.post(import.meta.env.VITE_API_URL + '/api/User/update-profile', request, {
                        headers : {
                            'Authorization' : "Bearer " + localStorage.token
                        }
                    })
                    await axios.post(import.meta.env.VITE_API_URL + '/api/Email/send', {
                        toAddresses : [this.email],
                        ccAddresses : [],
                        subject : "Mật khẩu để đăng nhập OnDemandTutor",
                        body : `
                            <h1>Mật khẩu để đăng nhập hệ thống vận hành OnDemandTutor</h1><p>
                                Vui lòng không chia sẻ cho bất kì ai khác<p>
                                    <h2>${randomPass}</h2>
                        `
                    }, {
                        headers : {
                            'Authorization' : "Bearer " + localStorage.token
                        }
                    })
                    this.eventBus.emit("open-result-dialog", {
                        message: "Tạo thành công",
                        type: "Success"
                    })
                    this.action()
                    this.close()
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Có vấn đề xảy ra khi tạo",
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        },
        async editOperator(confirmation) {
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn cập nhật tài khoản này?",
                    method: this.editOperator,
                    params: false
                })
            } else {
                const request = {
                    id : this.id,
                    phone: this.phone,
                    email: this.email,
                    firstName: this.firstName,
                    lastName: this.lastName,
                }
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    await axios.post(import.meta.env.VITE_API_URL + '/api/User/update-profile', request, {
                        headers : {
                            'Authorization' : "Bearer " + localStorage.token
                        }
                    })
                    
                    await axios.post(import.meta.env.VITE_API_URL + '/api/Auth/grant-role', {
                        email : this.email,
                        id : this.id,
                        role : this.role
                    }, {
                        headers : {
                            'Authorization' : "Bearer " + localStorage.token
                        }
                    })
                    this.eventBus.emit("open-result-dialog", {
                        message: "Cập nhật thành công",
                        type: "Success"
                    })
                    this.action()
                    this.close()
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Có vấn đề xảy ra khi cập nhật",
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        },
    },
    mounted() {
        this.refresh()
    }
}
</script>

<style></style>
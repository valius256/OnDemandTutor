<template>
    <div class="p-4 bg-white rounded-b-lg w-full flex flex-col">
        <div class="flex flex-col">
            <div class="flex ">
                <span class="w-48 p-1 font-bold">Nhập mật khẩu cũ</span>
                <input v-model="oldPass" class="p-1 border rounded-lg" placeholder="Nhập mật khẩu cũ" type="password" />
            </div>
            <div class="flex mt-4">
                <span class="w-48 p-1 font-bold">Nhập mật khẩu mới</span>
                <input v-model="newPass" class="p-1 border rounded-lg" placeholder="Nhập mật khẩu mới"
                    type="password" />
            </div>
            <div class="flex mt-4">
                <span class="w-48 p-1 font-bold">Xác nhận mật khẩu</span>
                <input v-model="cfmPass" class="p-1 border rounded-lg" placeholder="Xác nhận mật khẩu"
                    type="password" />
            </div>
            <button @click="handleChangePassword(true)" class="mt-4  hover:bg-blue-200 rounded-lg py-2 bg-blue-400 font-bold text-white">Xác nhận</button>
        </div>

    </div>
</template>

<script>
import axios from 'axios';
export default {
    name: "ChangePasswordPopup",
    injects: ['eventBus'],
    props: ['userId','close'],
    data() {
        return {
            oldPass: "",
            newPass: "",
            cfmPass: ""
        }
    },
    methods: {
        async handleChangePassword(confirmation) {
            if (this.newPass != this.cfmPass) {
                this.eventBus.emit("open-result-dialog", {
                    message: "Mật khẩu không trùng khớp",
                    type: "Error"
                })
                return;
            }
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc muốn đổi mật khẩu không ?",
                    method: this.handleChangePassword,
                    params: false
                })
            } else {

                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    await axios.post(import.meta.env.VITE_API_URL + '/api/Auth/change-password', {
                        oldPassword: this.oldPass,
                        newPassword: this.newPass
                    }, {
                        headers: {
                            'Authorization': "Bearer " + localStorage.token
                        }
                    })
                    this.close()
                    //var paymentUrl = url.data
                    //window.location.href = paymentUrl
                    this.eventBus.emit("open-result-dialog", {
                        message: "Cập nhật thành công",
                        type: "Success"
                    })
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
    }
}
</script>

<style></style>
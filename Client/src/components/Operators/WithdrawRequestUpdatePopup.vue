<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-y-auto max-h-[30rem] overflow-x-auto">
        <div class="mb-4">
            <div>
                <span >Người yêu cầu : </span>
                <span class="font-bold">
                    {{ (this.request.user.firstName ?? "") + " " + (this.request.user.lastName ?? "") }}
                </span>
            </div>
            <div>
                <span >Số lượng : </span>
                <span class="font-bold text-green-500">{{ this.request.amount.toLocaleString('vi-VN', {style: 'currency',currency: 'VND',}) }}</span>
            </div>
            <div>
                <span >Số tài khoản : </span>
                <span class="font-bold">{{ this.request.bankAccountNumber }}</span>
            </div>
            <div>
                <span >Ngân hàng: </span>
                <span class="font-bold">{{ this.request.bankName }}</span>
            </div>
        </div>
        <hr>
        <div class="flex flex-col">
            <div class="">
                <div class="p-1 font-bold">Phản hồi về yêu cầu</div>
                <textarea v-model="reply" class=" w-96 p-1 border rounded-lg" placeholder="Nhập phản hồi..." />
            </div>
            <div class="mt-4">
                <span class="p-1 font-bold">Trạng thái</span>
                <select class="flex gap-4 p-1 border rounded-lg w-full" v-model="status">
                    <option :value="1">Thành công</option>
                    <option :value="2">Thất bại</option>
                </select>
            </div>

        </div>
        <div class="flex justify-center mt-4 gap-3">
            <button @click="handleOk(true)" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">
                Xác nhận
            </button>
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>

    </div>
</template>

<script>
import axios from 'axios'

export default {
    name: "WithdrawRequestUpdatePopup",
    inject: ['eventBus'],
    props: ['request', 'close', 'action'],
    data() {
        return {
            reply: "",
            status: 1,
        }
    },
    methods: {
        async handleOk(confirmation) {
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn cập nhật yêu cầu này không?",
                    method: this.handleOk,
                    params: false
                })
            } else {
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    await axios.post(import.meta.env.VITE_API_URL + '/api/RequestWithDraw/approve', {
                        id: this.request.id,
                        status: this.status,
                        reply : this.reply 
                    }, {
                        headers: {
                            "Authorization": "Bearer " + localStorage.token
                        }
                    })
                    this.eventBus.emit("open-result-dialog", {
                        message: "Cập nhật thành công",
                        type: "Success"
                    })
                    await this.action()
                    this.close()
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
    },
    mounted() {
    }
}
</script>

<style></style>
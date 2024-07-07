<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-y-auto max-h-[30rem] overflow-x-auto">
        <div class="flex gap-3">
            <div class="flex flex-col">
                <span class="p-1 font-bold">Vui lòng cho biết lý do đình chỉ tài khoản này</span>
                <textarea v-model="reason" class="p-1 border rounded-lg" placeholder="Nhập lý do" />
            </div>            
        </div>
        <div class="flex justify-center mt-4 gap-3">
            <button @click="handleOk(true)" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác
                nhận</button>
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>

    </div>
</template>

<script>
import axios from 'axios'

export default {
    name: "AccountDeactivateReason",
    inject : ['eventBus'],
    props: ['id','close','action'],
    data() {
        return {
            reason : ""

        }
    },
    methods: {
        async handleOk(confirmation) {
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có muốn vô hiệu hóa tài khoản này không?",
                    method: this.handleOk,
                    params: false
                })
            } else {
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    const request = {
                        id : this.id,
                        isActive : false,
                        deaActiveReason : this.reason
                    }
                    await axios.patch(import.meta.env.VITE_API_URL + '/api/User/deaactive-account',request,{
                        headers : {
                            "Authorization" : "Bearer " + localStorage.token
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
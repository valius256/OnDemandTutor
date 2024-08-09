<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-y-auto max-h-[30rem] overflow-x-auto">
        <div>
            <span class="p-1 font-bold">Hãy cho biết vì sao bạn từ chối đơn đăng ký này</span><br>
            <textarea v-model="reason" class="p-1 border rounded-lg w-full" placeholder="Nhập lý do"/>
        </div>
        <div class="flex justify-center mt-4 gap-3">
            <button @click="handleReject(true)" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác
                nhận</button>
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>

    </div>
</template>

<script>

import axios from 'axios'
export default {
    name: "SubjectRegistrationRejectReason",
    props: ['close', 'tutorId','id','subjectId'],
    inject : ['eventBus'],
    data() {
        return {
            reason: "",
        }
    },
    methods: {
        async handleReject(confirmation) {
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: `Bạn có chắc chắn muốn từ chối đơn đăng ký này?`,
                    method: this.handleReject,
                    params: false
                })
            } else {
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    const request = {
                        id: this.id,
                        userId : this.tutorId,
                        subjectId : this.subjectId,
                        reasonReject : this.reason,
                        status : 2
                    }
                    await axios.put(import.meta.env.VITE_API_URL + '/api/TutorSubject/status', request, {
                        headers: {
                            "Authorization": "Bearer " + localStorage.token
                        }
                    })
                    this.eventBus.emit("open-result-dialog", {
                        message: "Cập nhật thành công",
                        type: "Success"
                    })
                    this.$router.push("/admin/subjects/registration")
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
}
</script>

<style></style>
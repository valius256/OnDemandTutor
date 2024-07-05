<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-x-auto">
        <div class="flex flex-col">
            <div class="flex w-96">
                <span class="w-24 p-1 font-bold">Câu hỏi</span>
                <input v-model="question" class="w-full p-1 border rounded-lg" placeholder="Nhập câu hỏi" />
            </div>
            <div class="flex mt-4 w-96">
                <span class="w-24 p-1 font-bold">Trả lời</span>
                <textarea v-model="answer" class="w-full p-1 border rounded-lg" placeholder="Nhập câu trả lời" />
            </div>
        </div>
        <div class="flex justify-center mt-4 gap-3">
            <button v-if="!editDto" @click="handleAdd" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác nhận</button>
            <button v-else @click="handleUpdate" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác nhận</button>
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>

    </div>
</template>

<script>
import axios from 'axios'

export default {
    name: "FAQEditAddPopup",
    inject : ['eventBus'],
    props: ['title', 'close', 'editDto','reload'],
    data() {
        return {
            error: "",
            question: "",
            answer: "",
        }
    },
    methods: {
        presetEdit() {
            if (this.editDto != null) {
                this.question = this.editDto.question
                this.answer = this.editDto.answer
            }
        },
        refresh() {
            if (this.editDto != null) {
                this.presetEdit();
            }
        },
        async handleAdd() {
            const request = {
                question: this.question,
                answer: this.answer,
                createById : 1,
                createAt : "2024-01-01",
                createByName : "abc"
            }
            this.eventBus.emit("open-loading-popup", {
                message: "Vui lòng chờ..."
            })
            try {

                await axios.post(import.meta.env.VITE_API_URL + `/api/FAQ/create`, request, {
                    headers: {
                        "Authorization": "Bearer " + localStorage.token
                    }
                })

                this.eventBus.emit("open-result-dialog", {
                    message: "Tạo FAQ thành công",
                    type: "Success"
                })
                this.reload()
                this.close()
            } catch (e) {
                console.log(e)
                this.eventBus.emit("open-result-dialog", {
                    message: "Có vấn đề xảy ra khi thêm FAQ",
                    type: "Error"
                })
            }
            this.eventBus.emit("close-loading-popup")
        },
        async handleUpdate() {
            const request = {
                id: this.editDto.id,
                question: this.question,
                answer: this.answer,
                createById : 1,
                createAt : "2024-01-01",
                createByName : "abc"
            }
            this.eventBus.emit("open-loading-popup", {
                message: "Vui lòng chờ..."
            })
            try {

                await axios.put(import.meta.env.VITE_API_URL + `/api/FAQ/update`, request, {
                    headers: {
                        "Authorization": "Bearer " + localStorage.token
                    }
                })

                this.eventBus.emit("open-result-dialog", {
                    message: "Cập nhật FAQ thành công",
                    type: "Success"
                })
                this.reload()
                this.close()
            } catch (e) {
                console.log(e)
                this.eventBus.emit("open-result-dialog", {
                    message: "Có vấn đề xảy ra khi cập nhật FAQ",
                    type: "Error"
                })
            }
            this.eventBus.emit("close-loading-popup")
        },

    },
    mounted() {
        this.refresh()
    }
}
</script>

<style></style>
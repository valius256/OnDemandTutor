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
            <button class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác nhận</button>
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>
        
    </div>
</template>

<script>
export default {
    name: "FAQEditAddPopup",
    props: ['title', 'close', 'editDto'],
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

    },
    mounted() {
        this.refresh()
    }
}
</script>

<style></style>
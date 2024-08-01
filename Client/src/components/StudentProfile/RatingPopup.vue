<template>
    <div class="p-4 bg-white rounded-b-lg w-full">
        <div class="flex flex-col w-96">
            <div class="font-bold text-center">
                Đánh giá của bạn
            </div>
            <star-rating :rating="5" class="flex justify-center" @update:rating="rating = $event" />
        </div>
        <div class="flex flex-col w-96 mt-8">
            <div class="font-bold text-center">
                Vui lòng cho biết feedback<br> của bạn về {{ (slotId ? "buổi học" : "") + (classId ? "lớp học" : "") }}
                này
            </div>
            <textarea v-model="feedback" class="h-32 border rounded-lg p-4"></textarea>
            <button class="mt-8 font-bold text-white bg-blue-400 rounded-lg py-2" @click="handleFeedback(true)">Xác
                nhận</button>
        </div>
    </div>
</template>

<script>
import axios from 'axios'
import StarRating from 'vue-star-rating'
export default {
    name: "RatingPopup",
    props: ['slotId', 'classId', 'close', 'action'],
    inject: ['eventBus'],
    components: { StarRating },
    data() {
        return {
            rating: 5,
            feedback: "",

        }
    },
    methods: {
        async handleFeedback(confirmation) {
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc muốn đánh giá " + (this.slotId ? "buổi học" : "") + (this.classId ? "lớp học" : "") + " này?",
                    method: this.handleFeedback,
                    params: false
                })
            } else {

                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    if (this.slotId) {
                        const request = {
                            rate: this.rating,
                            feedback: this.feedback
                        }
                        await axios.put(import.meta.env.VITE_API_URL + '/api/SlotStudent/feedback-rating?slotId=' + this.slotId, request, {
                            headers: {
                                'Authorization': "Bearer " + localStorage.token
                            }
                        })
                    }
                    if (this.classId) {
                        const request = {
                            rating: this.rating,
                            feedback: this.feedback,
                            classId: this.classId
                        }
                        await axios.post(import.meta.env.VITE_API_URL + '/api/Class/rating', request, {
                            headers: {
                                'Authorization': "Bearer " + localStorage.token
                            }
                        })
                    }

                    this.action(null, null)
                    this.close()
                    //var paymentUrl = url.data
                    //window.location.href = paymentUrl
                    this.eventBus.emit("open-result-dialog", {
                        message: "Đánh giá thành công",
                        type: "Success"
                    })
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Có vấn đề xảy ra khi đánh giá",
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
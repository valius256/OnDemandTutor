<template>
    <div class="bg-white shadow-md rounded-lg p-6 w-full mt-8">
        <h2 class="text-2xl font-semibold mb-4">Đánh giá từ các học viên trước</h2>
        <div class="flex">
            <button @click="setFeedbackMode(0)" class="rounded-lg w-full p-2"
                :class="{ 'bg-gray-300': feedbackMode == 0 }">
                Đánh giá buổi học
            </button>
            <button @click="setFeedbackMode(1)" class="rounded-lg w-full p-2"
                :class="{ 'bg-gray-300': feedbackMode == 1 }">
                Đánh giá lớp học
            </button>
        </div>
        <div v-if="feedbackMode == 0">
            <div v-for="studentSlot in studentSlots" :key="studentSlot.id">
                <div class="p-4 flex gap-4">
                    <img class="w-24 h-24 rounded-full" :src="studentSlot.user.avatarImageUrl">
                    <div>
                        <div class="font-bold">{{ (studentSlot.user.firstName ?? "") + " " + (studentSlot.user.lastName ?? "")}}</div>
                        <div class="text-sm italic">
                            Phản hồi về buổi học môn {{ studentSlot.slot.subject.name }}
                            . Bắt đầu {{ this.beautifyDatetime(studentSlot.slot.startTime)  }}
                            , Kết thúc {{ this.beautifyDatetime(studentSlot.slot.endTime)  }}
                        </div>
                        <star-rating :star-size="20" :rating="studentSlot.rating" :round-start-rating="false"
                            :read-only="true" />
                        <div class="mt-2">
                            {{ studentSlot.feedback }}
                        </div>
                    </div>
                </div>
                <hr>
            </div>
        </div>
        <div v-if="feedbackMode == 1">
            <div v-for="studentClass in studentClasses" :key="studentClass.id">
                <div class="p-4 flex gap-4">
                    <img class="w-24 h-24 rounded-full" :src="studentClass.student.avatarImageUrl">
                    <div>
                        <div class="font-bold">{{ (studentClass.student.firstName ?? "") + " " +
                    (studentClass.student.lastName ?? "") }}</div>
                        <star-rating :star-size="20" :rating="studentClass.student.rating" :round-start-rating="false"
                            :read-only="true" />
                        <div class="mt-2" v-if="studentClass.student.feedback">
                            {{ studentClass.student.feedback }}
                        </div>
                    </div>
                </div>
                <hr>
            </div>
        </div>
        <div class=" flex gap-4 justify-center mt-4">
            <button @click="movePage(false)">
                <i class="fa fa-arrow-left text-2xl"></i>
            </button>
            <div class="flex gap-2 ">
                <input class="border p-1 rounded-md w-16" type="number" v-model="currentPage" min="1"
                    @change="handlePageChange">
                <div class="p-1"> / {{ this.totalPage }}</div>
            </div>
            <button @click="movePage(true)">
                <i class="fa fa-arrow-right text-2xl"></i>
            </button>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import StarRating from 'vue-star-rating'

export default {
    name: "PreviousFeedback",
    components: { StarRating },
    props: ['tutorId'],
    data() {
        return {
            totalPage: 100,
            pageSize: 5,
            currentPage: 1,
            feedbackMode: 0,
            studentSlots: [],
            studentClasses: [],
        }
    },
    methods: {
        async fetchClassFeedback() {
            const response = await axios.get(
                `${import.meta.env.VITE_API_URL
                }/api/StudentClass?Filter.TutorId=${this.tutorId}&Filter.IsRated=true&Page=${this.currentPage}&Limit=${this.pageSize}`
            )
            if (response.data && response.data.items) {
                this.studentClasses = response.data.items;
                this.totalPage = Math.ceil(response.data.total / this.pageSize)
            }
            console.log(this.studentClasses)
        },
        async fetchSlotFeedback() {
            const response = await axios.get(
                `${import.meta.env.VITE_API_URL
                }/api/SlotStudent/get-student-slots-tutor?Filter.TutorId=${this.tutorId}&Filter.IsRated=true&Page=${this.currentPage}&Limit=${this.pageSize}`
            )
            if (response.data && response.data.items) {
                this.studentSlots = response.data.items;
                this.totalPage = Math.ceil(response.data.total / this.pageSize)
            }
        },

        async handlePageChange() {
            if (this.currentPage > this.totalPage) {
                this.currentPage = this.totalPage
            }
            if (this.currentPage < 1) {
                this.currentPage = 1
            }
            if (this.feedbackMode == 0) {
                await this.fetchSlotFeedback()
            } else {
                await this.fetchClassFeedback()
            }
        },
        async movePage(forward) {
            if (forward && this.currentPage < this.totalPage) {
                this.currentPage++
                await this.handlePageChange()
            } else if (!forward && this.currentPage > 1) {
                this.currentPage--
                await this.handlePageChange()
            }
        },
        async setFeedbackMode(mode) {
            this.feedbackMode = mode
            this.currentPage = 0
            await this.handlePageChange()
        },
    },
    mounted() {
        this.setFeedbackMode(0)
    }
}
</script>

<style></style>
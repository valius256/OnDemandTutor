<template>
    <div>
        <div class="p-6" v-if="tutor">
            <div class="bg-white shadow-md rounded-lg p-6 w-full">
                <h2 class="text-2xl font-semibold mb-4">Thông tin gia sư</h2>
                <div class="grid grid-cols-3 gap-4">
                    <div class="flex flex-col">
                        <span class="font-medium text-gray-600">Họ</span>
                        <span>{{ tutor.firstName }}</span>
                    </div>
                    <div class="flex flex-col">
                        <span class="font-medium text-gray-600">Tên</span>
                        <span>{{ tutor.lastName }}</span>
                    </div>
                    <div class="flex flex-col">
                        <span class="font-medium text-gray-600">Email</span>
                        <span>{{ tutor.email }}</span>
                    </div>
                    <div class="flex flex-col">
                        <span class="font-medium text-gray-600">Số điện thoại</span>
                        <span>{{ tutor.phone }}</span>
                    </div>
                    <div class="flex flex-col">
                        <span class="font-medium text-gray-600">Ngày sinh</span>
                        <span>{{ tutor.dob?.substring(0, 10) }}</span>
                    </div>
                    <div class="flex flex-col">
                        <span class="font-medium text-gray-600">Địa chỉ</span>
                        <span>{{ tutor.address }}</span>
                    </div>
                    <div class="flex flex-col">
                        <span class="font-medium text-gray-600">Giới tính</span>
                        <span>{{ tutor.sex }}</span>
                    </div>
                    <div class="flex flex-col">
                        <span class="font-medium text-gray-600">Đánh giá</span>
                        <span> <star-rating :star-size="20" :rating="tutor.rating" :round-start-rating="false"
                                :read-only="true" /></span>
                    </div>
                    <div class="flex flex-col">
                        <span class="font-medium text-gray-600">Giá dịch vụ</span>
                        <span class="text-red-400 font-bold">{{ tutor.tutorFeePerHour.toLocaleString("vi-VN", {
            style: "currency",
            currency: "VND",
        }) }} / giờ</span>
                    </div>
                </div>
            </div>
            <div class="bg-white shadow-md rounded-lg p-6 w-full mt-8" v-if="currentUser && currentUser.role > 1">
                <h2 class="text-2xl font-semibold mb-4">Giấy tờ tùy thân</h2>
                <div class="grid grid-cols-3 gap-4">
                    <div class="flex flex-col">
                        <span class="font-medium text-gray-600">Họ</span>
                        <span>{{ tutor.firstName }}</span>
                    </div>

                </div>
            </div>
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
                    <div v-for="slot in studentSlots.slotStudents" :key="slot.id">
                        <div class="p-4 flex gap-4">
                            <img class="w-24 h-24 rounded-full" :src="slot.user.avatarImageUrl">
                            <div>
                                <div class="font-bold">{{ (slot.user.firstName ?? "") + " " + (slot.user.lastName ?? "")
                                    }}</div>
                                <star-rating :star-size="20" :rating="slot.rating" :round-start-rating="false"
                                    :read-only="true" />
                                <div class="mt-2">
                                    {{ slot.feedback }}
                                </div>
                            </div>
                        </div>
                        <hr>
                    </div>

                </div>
                <div v-if="feedbackMode == 1">
                    <div v-for="class_ in studentClasses.studentClasses" :key="class_.id">
                        <div class="p-4 flex gap-4">
                            <img class="w-24 h-24 rounded-full" :src="class_.student.avatarImageUrl">
                            <div>
                                <div class="font-bold">{{ (class_.student.firstName ?? "") + " " +
            (class_.student.lastName ?? "") }}</div>
                                <star-rating :star-size="20" :rating="class_.student.rating" :round-start-rating="false"
                                    :read-only="true" />
                                <div class="mt-2" v-if="class_.student.feedback">
                                    {{ class_.student.feedback }}
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
        </div>
    </div>
</template>

<script>
import axios from 'axios'
import StarRating from 'vue-star-rating'

export default {
    name: "StudentProfile",
    inject: ['eventBus'],
    props: ['tutor'],
    components: { StarRating },
    data() {
        return {
            totalPage: 100,
            pageSize: 5,
            currentPage: 1,
            feedbackMode: 0,
            currentUser: null,
            studentSlots: [],
            studentClasses: [],
        }
    },
    methods: {
        async refresh() {
            this.currentUser = await this.getUserFromToken()
            this.feedbackMode = 0;
            await this.handlePageChange()
        },
        async getUserSlots() {
            const column = "startTime"; // Example column name
            const isDesc = true; // Example sort order

            try {
                const response = await axios.get(
                    `${import.meta.env.VITE_API_URL
                    }/api/Slot?Filter.UserId=${this.tutor.id}&Sorts[column]=${column}&Sorts[isDesc]=${isDesc}`
                );
                console.log(response.data); // Log the response data for debugging
                if (response.data && response.data.items) {
                    this.slots = response.data.items;
                } else {
                    this.slots = []; // Ensure slots is an array even if the response is empty
                }
            } catch (error) {
                console.error("Error fetching user slots:", error);
                this.slots = []; // Handle errors by setting slots to an empty array
            }
        },
        async fetchClassFeedback() {
            const response = await axios.get(
                `${import.meta.env.VITE_API_URL
                }/api/Class/tutor-class-student?TutorId=${this.tutor.id}&Page=${this.currentPage}&Limit=${this.pageSize}`
            )
            if (response.data && response.data.items) {
                this.studentClasses = response.data.items[0];
                this.totalPage = Math.ceil(response.data.total / this.pageSize)
            }
            console.log(this.studentClasses)
        },
        async fetchSlotFeedback() {
            const response = await axios.get(
                `${import.meta.env.VITE_API_URL
                }/api/Slot/tutor-slot-student?TutorId=${this.tutor.id}&Page=${this.currentPage}&Limit=${this.pageSize}`
            )
            if (response.data && response.data.items) {
                this.studentSlots = response.data.items[0];
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
        this.refresh()
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
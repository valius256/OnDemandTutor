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
                <div>

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
            currentUser: null
        }
    },
    methods: {
        async refresh() {
            this.currentUser = await this.getUserFromToken()

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
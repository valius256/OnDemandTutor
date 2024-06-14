<template>
    <div class="p-6">
        <div class="flex flex-col lg:flex-row lg:place-content-between gap-4">
            <div class="w-full">
                <div class="text-2xl font-bold mb-6">
                    Thông tin lớp học
                </div>
                <div class="bg-white shadow-lg rounded-lg p-6 mb-6">
                    <table class="">
                        <tr>
                            <td class="font-semibold text-gray-700">Tên lớp:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">{{ classData.name }}</td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">Số lượng học sinh:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">{{ classData.numberOfStudents }}</td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">Thời gian:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">{{ classData.startDate }} - {{ classData.endDate }}</td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">Môn học:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">{{ classData.subject.name }}</td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">Địa chỉ dạy:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">{{ classData.teachAddress }}</td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">Tạo bởi:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">{{ classData.createBy.name }}</td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">Tạo lúc:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">{{ classData.createAt }}</td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">Trạng thái:</td>
                            <td class="w-32"></td>
                            <td :class="getStatusDisplay(classData.status).css">{{
                                getStatusDisplay(classData.status).display }}</td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="w-full">
                <div class="text-2xl font-bold mb-6">
                    Thông tin gia sư
                </div>
                <div class="bg-white shadow-lg rounded-lg p-6 mb-6">
                    <table class="" v-if="classData.tutor">
                        <tr>
                            <td class="font-semibold text-gray-700">Tên gia sư:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">{{ classData.tutor.name }}</td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">SDT:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">{{ classData.tutor.phone }}</td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">Email:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">{{ classData.tutor.email }}</td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">Đánh giá gia sư:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">
                                <star-rating :star-size="20" :rating="classData.tutor.rating"
                                    :round-start-rating="false" :read-only="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">Tỉ lệ áp giá:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">{{ classData.priceRatio }}</td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">Giá cả thỏa thuận:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">
                                <span class="text-red-400 font-bold">{{ (classData.salary * classData.priceRatio *
                                classData.numberOfStudents).toLocaleString('vi-VN', {
                                    style: 'currency',
                                    currency: 'VND',
                                }) }}</span> / h
                            </td>
                        </tr>
                    </table>
                    <div v-else class="italic">
                        Lớp này hiện chưa có gia sư
                    </div>
                </div>
            </div>
        </div>
        <div class="mt-8">
            <div class="text-2xl font-bold mb-6">
                Slot tiếp theo
            </div>
            <div v-if="upcomingSlot" class="p-4 bg-slate-200 rounded-lg">
                <div>
                    <span class="font-bold">Trạng thái : </span>
                    <span :class="getSlotStatus(upcomingSlot.startTime,upcomingSlot.endTime ).style">
                        {{ getSlotStatus(upcomingSlot.startTime,upcomingSlot.endTime ).display }}
                    </span>
                </div>
                <div class="flex place-content-between mt-4">
                    <div>
                        <span class="mr-4 font-bold">Bắt đầu :</span>
                        <span class="mr-4">{{ upcomingSlot.startTime }}</span>
                    </div>
                    <div>
                        <span class="mr-4 font-bold">Kết thúc :</span>
                        <span class="mr-4">{{ upcomingSlot.endTime }}</span>
                    </div>
                    <div>
                        <span class="mr-4 font-bold">Tổng thời lượng :</span>
                        <span class="mr-4">{{ (upcomingSlot.durationInHour?.toFixed(2)) }} tiếng</span>
                    </div>
                </div>
                <div class="font-bold italic mt-4 text-gray-500">
                    *Khi slot bắt đầu, hệ thống sẽ tự quét trừ tiền trong ví của quý khách. Để trách những rắc rối về sau, bạn vui lòng nạp tiền vào ví đầy đủ trước khi bắt đầu vào học nhé!<br>
                    *Dựa trên thời lượng và giá cả thỏa thuận, slot này sẽ trừ bạn :
                    <span class="text-red-500">
                        {{ (classData.salary * classData.priceRatio *
                            classData.numberOfStudents * upcomingSlot.durationInHour).toLocaleString('vi-VN', {
                                style: 'currency',
                                currency: 'VND',
                            }) }}
                    </span>
                </div>
            </div>
            <div v-else class="italic">
                Hiện không còn slot nào
            </div>

        </div>
        <div class="mt-8">
            <div class="text-2xl font-bold mb-6">
                Thời khóa biểu lớp này
            </div>
            <div class="p-16 bg-slate-400 rounded-lg">
                Comming soon...
            </div>
        </div>
        <div class="mt-8">
            <div class="text-2xl font-bold mb-2">
                Đánh giá gia sư
            </div>
            <div class="text-lg mb-6 italic text-center">
                Bạn hài lòng với gia sư này không? Hãy đánh giá gia sư của bạn
                <star-rating :rating="5" class="flex justify-center" @update:rating="tutorRating = $event" />
                <div class="flex justify-center">
                    <button @click="rateTutor"
                        class="mt-3 bg-slate-50 text-blue-600 py-3 px-6 rounded-full text-lg font-bold">
                        Đánh giá
                    </button>
                </div>
            </div>
        </div>
    </div>

</template>

<script>
import { useRoute } from 'vue-router';
import StarRating from 'vue-star-rating'

export default {
    name: "CLassDetailPage",
    components: { StarRating },
    data() {
        return {
            classId: 0,
            route: null,
            classData: {
                id: 0,
                name: 'Math 101',
                tutor: {
                    name: "Phale",
                    email: "tutor@example.com",
                    phone: "0987654321",
                    rating: 4.5
                },
                numberOfStudents: 3,
                startDate: '2024-01-01',
                endDate: '2024-06-01',
                subject: {
                    name: "Piano"
                },
                teachAddress: '123 Main St',
                createBy: {
                    name: "Hello world"
                },
                createAt: '2024-01-01',
                tutorRating: 4.5,
                status: "Active",
                salary: 100000,
                priceRatio: 0.8,
                slots: [
                    {
                        startTime: "2024-06-09 16:47:00",
                        endTime: "2024-06-09 16:48:00"
                    },
                    {
                        startTime: "2024-06-10 10:00:00",
                        endTime: "2024-06-10 11:30:00"
                    }
                ]
            },
            tutorRating: 0,
            upcomingSlot: {
                startTime: null,
                endTime: null
            }
        }
    },
    mounted() {
        this.route = useRoute();
        this.classId = this.route.params.id;
        this.upcomingSlot = this.getClosestSlot(this.classData.slots)
        console.log(this.upcomingSlot)
    },
    methods: {
        rateTutor() {
            alert("Rated : " + this.tutorRating);
        },
        getStatusDisplay(status) {
            let css = "px-4 py-1 text-white font-bold rounded-lg text-center"
            switch (status) {
                case "Active":
                    return {
                        css: css + " bg-green-500",
                        display: "Đang diễn ra"
                    }
                case "Pending":
                    return {
                        css: css + " bg-gray-500",
                        display: "Chờ gia sư"
                    }
                case "Finished":
                    return {
                        css: css + " bg-blue-500",
                        display: "Đã hoàn thành"
                    }
                default:
                    return {
                        css: css + " bg-gray-500",
                        display: "Không rõ"
                    }
            }
        },
        getClosestSlot(slots) {
            const now = new Date();

            // Filter out slots that have already ended
            const futureSlotsOnly = slots.filter((slot) => {
                const endTime = new Date(slot.endTime);
                return endTime > now;
            });

            // Sort the future slots by their distance from the current time
            const sortedSlots = futureSlotsOnly.map((slot) => {
                const startTime = new Date(slot.startTime);
                const endTime = new Date(slot.endTime);
                const startDistance = startTime - now;
                const endDistance = endTime - now;
                const durationInHour = (endDistance - startDistance) / 3600000;
                return { ...slot, startDistance, endDistance, durationInHour };
            }).sort((a, b) => a.startDistance - b.startDistance);

            // Return the slot with the closest start time in the future
            return sortedSlots.length > 0 ? sortedSlots[0] : null;
        },
        getSlotStatus(startTime, endTime){
            let generalCss = "p-2 text-white font-bold rounded-lg"
            const time = new Date(startTime)
            const timeEnd = new Date(endTime)
            const present = new Date()
            if (time > present){
                return {
                    style : generalCss + " bg-gray-500",
                    display : "Sắp bắt đầu"
                }
            } else if (time <= present && present < timeEnd) {
                return {
                    style : generalCss + " bg-green-500",
                    display : "Đang diễn ra"
                }
            } else {
                return {
                    style : generalCss + " bg-gray-500",
                    display : "Đã qua"
                }
            }
        }
    }
}
</script>

<style></style>
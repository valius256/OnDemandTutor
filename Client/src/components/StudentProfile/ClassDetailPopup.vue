<template>
    <div class="p-4 bg-white rounded-b-lg w-full">
        <div class="font-bold text-2xl">{{ this.class.name }}</div>
        <div class="mt-4 flex place-content-between gap-4">
            <div>
                <div class="flex">
                    <span>Gia sư : </span>
                    <button class="font-bold text-blue-400 underline ml-4">
                        {{ (this.class.tutor.firstName ?? "") + " " + (this.class.tutor.lastName ?? "") }}
                    </button>
                </div>
                <div class="mt-2">
                    <span class="">Thời gian :</span>
                    <span class="font-bold ml-4">
                        {{ sqlDateStringToSlashFormat(this.class.startDate) }} đến
                        {{ sqlDateStringToSlashFormat(this.class.endDate) }}
                    </span>
                </div>
                <div class="mt-2">
                    <span class="">Môn học :</span>
                    <span class="font-bold ml-4">{{ this.class.subject.name }}</span>
                </div>
            </div>
            <div>
                <div>
                    <span class="">Địa điểm :</span>
                    <span class="font-bold ml-3">{{ this.class.location }}</span>
                </div>
                <div class="mt-2">
                    <span class="">Hình thức :</span>
                    <span :class="getMethodStyle(this.class.method)">{{ this.class.method }}</span>
                </div>
                <div class="mt-2">
                    <span class="">Trạng thái :</span>
                    <span :class="getStatusStyle(this.class.status)">
                        {{ getStatusDisplay(this.class.status) }}
                    </span>
                </div>
            </div>

        </div>
        <hr class="mt-4">
        <div class="h-80 overflow-y-auto">
            <div class="font-bold ">Các buổi học :</div>
            <div v-for="slot in this.class.slots" :key="slot.id" class="p-2  border shadow-md mb-2 rounded-lg" :class="{
            'border-blue-400': this.compareDate(new Date(slot.endTime), new Date()) < 0,
        }">
                <div class="flex place-content-between">
                    <div>
                        <div class="text-sm">
                            <span class="font-bold">Bắt đầu :</span>
                            <span class="ml-4">{{ this.beautifyDatetime(slot.startTime) }}</span>
                        </div>
                        <div class="text-sm">
                            <span class="font-bold">Địa điểm :</span>
                            <span class="ml-4">{{ (slot.teachAddress) }}</span>
                        </div>
                        <div class="text-sm mt-1">
                            <span class="font-bold">Hình thức :</span>
                            <span :class="getMethodStyle(slot.isOnline ? 'Online' : 'Offline')">
                                {{ (slot.isOnline ? "Online" : "Offline") }}
                            </span>
                        </div>
                    </div>
                    <div>
                        <div class="text-sm">
                            <span class="font-bold">Kết thúc :</span>
                            <span class="ml-4">{{ this.beautifyDatetime(slot.endTime) }}</span>
                        </div>
                        <div class="text-sm">
                            <span class="font-bold">Thời lượng :</span>
                            <span class="ml-4">{{ (calcDuration(slot)) }} tiếng</span>
                        </div>
                        <!-- <div class="text-sm">
                            <span class="font-bold">Giá cả :</span>
                            <span class="ml-4 text-blue-300 font-bold">{{ (calcDuration(slot) * this.class.tutor.tutorPricePerHour).toLocaleString('vi-VN', {style: 'currency', currency: 'VND'})}}</span>
                        </div> -->
                    </div>
                </div>
                <div class="flex place-content-between">
                    <div class="flex items-center">
                        <span v-if="slot.slotStudents[0].paymentStatus == 0  && new Date(slot.startTime) < new Date()" class="p-1 text-sm italic text-red-500">Bạn đang nợ slot này</span>
                        <button v-if="slot.slotStudents[0].paymentStatus == 0" class="p-1 text-sm underline italic text-blue-400">Thanh toán ngay</button>
                    </div>
                    <button class="p-1 bg-blue-300 font-bold text-white rounded-lg">Xem chi tiết</button>
                </div>
            </div>
        </div>
        <div class="flex justify-center">
            <button v-if="this.class.status == 2" class="p-2 bg-blue-500 hover:bg-blue-300 font-bold rounded-lg text-white">
                Đánh giá gia sư
            </button>
            <button v-else class="p-2 bg-red-500 hover:bg-red-300 font-bold rounded-lg text-white">
                Rời lớp học
            </button>
        </div>

    </div>
</template>

<script>
export default {
    name: "ClassDetailPopup",
    props: ['classId', 'close'],
    data() {
        return {
            class: {
                id: 1,
                name: "Luyện thi IELTS nâng cao",
                startDate: "2024-01-01",
                endDate: "2024-12-01",
                method: "Online",
                location: "Q9, TPHCM",
                subject: {
                    name: "Tiếng Anh"
                },
                studentNumber: 10,
                maxStudentNumber: 20,
                tutor: {
                    firstName: "Thomas",
                    lastName: "Shelby",
                    avatarImgUrl: "/src/assets/noavatar.jpg",
                    tutorPricePerHour: 50000
                },
                status: 0,
                slots: [
                    {
                        startTime: "2024-01-01 7:00:00",
                        endTime: "2024-01-01 8:30:00",
                        isOnline: true,
                        teachAddress: "Somewhere, TPHCM",
                        slotStudents: [
                            {
                                paymentStatus: true
                            }
                        ]
                    },
                    {
                        startTime: "2024-01-02 7:00:00",
                        endTime: "2024-01-02 8:30:00",
                        isOnline: true,
                        teachAddress: "Somewhere, TPHCM",
                        slotStudents: [
                            {
                                paymentStatus: false
                            }
                        ]
                    },
                    {
                        startTime: "2024-07-14 18:00:00",
                        endTime: "2024-07-14 18:30:00",
                        isOnline: false,
                        teachAddress: "Somewhere, TPHCM",
                        slotStudents: [
                            {
                                paymentStatus: false
                            }
                        ]
                    }
                ]
            }
        }
    },
    methods: {
        handleClose() {
            this.close(0)
        },
        getMethodStyle(method) {
            let general = "ml-4 rounded-lg px-3 py-1 text-white font-bold"
            switch (method) {
                case "Online":
                    return general + " bg-green-400"
                default:
                    return general + " bg-gray-400"
            }
        },
        getStatusStyle(status) {
            let general = "ml-3 rounded-lg px-3 py-1 font-bold"
            switch (status) {
                case 0:
                    return general + " text-blue-400"
                case 1:
                    return general + " text-green-400"
                default:
                    return general + " text-gray-400"
            }
        },
        getStatusDisplay(status) {
            switch (status) {
                case 0:
                    return "Sắp bắt đầu"
                case 1:
                    return "Đang diễn ra"
                case 2:
                    return "Đã kết thúc"
                default:
                    return "Không rõ"
            }
        },
        calcDuration(slot) {
            const startTime = new Date(slot.startTime);
            const endTime = new Date(slot.endTime);
            return ((endTime - startTime) / 3600000).toFixed(2);
        }
    }
}
</script>

<style></style>
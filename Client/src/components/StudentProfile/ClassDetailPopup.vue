<template>
    <div class="py-4 px-8" v-if="this.class">
        <div class="font-bold text-3xl">{{ this.class.name }}</div>
        <div class="mt-4 flex flex-col md:flex-row md:place-content-between gap-4">
            <div>
                <div class="flex" v-if="this.class.user">
                    <span>Gia sư : </span>
                    <button class="font-bold text-blue-400 underline ml-4">
                        {{ (this.class.user.firstName ?? "") + " " + (this.class.user.lastName ?? "") }}
                    </button>
                </div>
                <div class="mt-2">
                    <span class="">Thời gian :</span>
                    <span class="font-bold ml-4">
                        {{ (this.class.startTime?.substring(0, 10)) ?? "" }} đến
                        {{ (this.class.endTime?.substring(0, 10)) ?? "" }}
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
        <div class="font-bold my-4">Thời khóa biểu :</div>
        <time-table :slots="this.slots" :fetching="getUserSlots" :view-detail="openSlotDetailPopup" />
        <hr class="mt-4">
        <div class="font-bold my-4">Các buổi học :</div>
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            <div v-for="(slot, index) in this.slots" :key="index" class="p-2  border-4 shadow-md mb-2 rounded-lg"
                :class="{
                    'border-blue-300': this.compareDate(new Date(slot.slot.endTime), new Date()) < 0,
                }">
                <div class="font-bold text-center text-xl">Buổi {{ index + 1 }}</div>
                <hr>
                <div class="flex flex-col gap-2">
                    <div class="flex flex-col gap-2 min-h-48">
                        <div class="text-sm">
                            <span class="font-bold">Bắt đầu :</span>
                            <span class="ml-4">{{ this.beautifyDatetime(slot.slot.startTime) }}</span>
                        </div>
                        <div class="text-sm">
                            <span class="font-bold">Kết thúc :</span>
                            <span class="ml-4">{{ this.beautifyDatetime(slot.slot.endTime) }}</span>
                        </div>
                        <div class="text-sm">
                            <span class="font-bold">Địa điểm :</span>
                            <span class="ml-4">{{ (slot.slot.teachAddress) }}</span>
                        </div>
                        <div class="text-sm mt-1">
                            <span class="font-bold">Hình thức :</span>
                            <span :class="getMethodStyle(slot.slot.isOnline ? 'Online' : 'Offline')">
                                {{ (slot.isOnline ? "Online" : "Offline") }}
                            </span>
                        </div>

                        <div class="text-sm">
                            <span class="font-bold">Thời lượng :</span>
                            <span class="ml-4">{{ (calcDuration(slot)) }} tiếng</span>
                        </div>
                        <div class="flex items-center">
                            <span
                                v-if="slot.paymentStatus == 0 && new Date(slot.slot.startTime) < new Date()"
                                class="p-1 text-sm italic text-red-500">Bạn đang nợ slot này</span>
                            <button v-if="slot.paymentStatus == 0"
                                class="p-1 text-sm underline italic text-blue-400">Thanh toán ngay</button>
                        </div>
                    </div>
                    <button class=" p-1 bg-blue-300 font-bold text-white rounded-lg">Xem chi tiết</button>
                </div>
            </div>
        </div>
        <div class="flex justify-center mt-8">
            <button v-if="this.class.status == 2"
                class="p-2 bg-blue-500 hover:bg-blue-300 font-bold rounded-lg text-white">
                Đánh giá gia sư
            </button>
            <button v-else class=" p-2 bg-red-500 hover:bg-red-300 font-bold rounded-lg text-white">
                Rời lớp học
            </button>
        </div>
        <generic-popup v-if="isOpenSlotDetailPopup" title="Chi tiết buổi học" :closeFunction="closeSlotDetailPopup">
            <slot-detail-popup :slot="selectingSlot" :close="closeSlotDetailPopup"/>
        </generic-popup>
    </div>
</template>

<script>
import axios from 'axios'
import TimeTable from './TimeTable.vue'
import GenericPopup from '../common/GenericPopup.vue'
import SlotDetailPopup from './SlotDetailPopup.vue'
export default {
    components: { TimeTable, GenericPopup, SlotDetailPopup },
    name: "ClassDetailPopup",
    props: ['classId', 'close'],
    data() {
        return {
            class: null,
            slots: [],
            selectingSlot : null,
            isOpenSlotDetailPopup : false
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
            const startTime = new Date(slot.slot.startTime);
            const endTime = new Date(slot.slot.endTime);
            return ((endTime - startTime) / 3600000).toFixed(2);
        },
        async getClassDetail() {
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/Class/' + this.classId, {
                headers: {
                    'Authorization': "Bearer " + localStorage.token
                }
            })
            if (response.data) {
                this.class = response.data
            }
        },
        async getUserSlots(from, to) {
            console.log("Getting slot")
            let queryString = ""
            if (from != null) {
                queryString += "&From=" + from
            }
            if (to != null) {
                queryString += "&To=" + to
            }
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/SlotStudent/get-slots-of-students?ClassId=' + this.classId + queryString, {
                headers: {
                    'Authorization': "Bearer " + localStorage.token
                }
            })
            if (response.data) {
                this.slots = response.data
            }
        },
        openSlotDetailPopup(slot) {
            this.selectingSlot = slot
            this.isOpenSlotDetailPopup = true
        },
        closeSlotDetailPopup() {
            this.isOpenSlotDetailPopup = false
        },
        async refresh() {
            await this.getUserSlots(null, null)
            await this.getClassDetail()
        }
    },
    mounted() {
        this.refresh()
    }
}
</script>

<style></style>
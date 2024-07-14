<template>
    <div>
        <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200 ">
            Slot học tiếp theo
        </div>
        <div class="m-8">
            <div class="flex gap-4 mb-2">
                <div class="text-xl font-bold py-1">
                    <span class="mr-4 ">Số dư hiện tại : </span>
                    <span class="text-green-200 p-1 bg-green-600 rounded-lg">{{ balance.toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                    }) }} </span>
                </div>
                <router-link to="/student/payment"
                    class="mr-6 p-1 text-xl font-bold text-white bg-blue-400 hover:bg-blue-200 rounded-lg">
                    Nạp tiền
                </router-link>

            </div>
            <div v-if="upcomingSlot">
                <div class="p-4 bg-blue-100 rounded-lg">
                    <div>
                        <span class="font-bold">Trạng thái : </span>
                        <span :class="getSlotStatus(upcomingSlot.startTime, upcomingSlot.endTime).style">
                            {{ getSlotStatus(upcomingSlot.startTime, upcomingSlot.endTime).display }}
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
                </div>
                <div class="font-bold italic mt-4 text-gray-500">
                    <div v-if="upcomingSlot.paidStatus == 'NotCharged'">
                        *Khi slot bắt đầu, hệ thống sẽ tự quét trừ tiền trong ví của quý khách. Để trách những rắc rối
                        về sau, bạn vui lòng nạp tiền vào ví đầy đủ trước khi bắt đầu vào học nhé!<br>
                        *Dựa trên thời lượng và giá cả thỏa thuận, slot này sẽ trừ bạn :
                        <span class="text-red-500">
                            {{ (upcomingSlot.user?.salary * upcomingSlot.durationInHour).toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                    }) }}
                        </span>
                    </div>
                </div>
            </div>
            <div v-else class="italic">
                Hiện không còn slot nào
            </div>

        </div>
        <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200 ">
            Thời khóa biểu
        </div>
        <time-table :slots="slots" :fetching="getUserSlots" :viewDetail="openSlotDetailPopup"/>
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
    name: "StudentProfileSchedule",
    data() {
        return {
            balance: 0,
            user: null,
            from : null,
            to : null,
            isOpenSlotDetailPopup : false,
            slots: [

            ],
            upcomingSlot: {
                startTime: null,
                endTime: null,
                user: null,
                paidStatus: ""
            }
        }

    },
    methods: {
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
                const endTime = new Date(slot.slot.endTime);
                return endTime > now;
            });

            // Sort the future slots by their distance from the current time
            const sortedSlots = futureSlotsOnly.map((slot) => {
                const startTime = new Date(slot.slot.startTime);
                const endTime = new Date(slot.slot.endTime);
                const user = slot.user
                const paidStatus = slot.paymentStatus
                const startDistance = startTime - now;
                const endDistance = endTime - now;
                const durationInHour = (endDistance - startDistance) / 3600000;
                return { ...slot, startDistance, endDistance, durationInHour, user, paidStatus };
            }).sort((a, b) => a.startDistance - b.startDistance);

            // Return the slot with the closest start time in the future
            return sortedSlots.length > 0 ? sortedSlots[0] : null;
        },
        getSlotStatus(startTime, endTime) {
            let generalCss = "p-2 text-white font-bold rounded-lg"
            const time = new Date(startTime)
            const timeEnd = new Date(endTime)
            const present = new Date()
            if (time > present) {
                return {
                    style: generalCss + " bg-gray-500",
                    display: "Sắp bắt đầu"
                }
            } else if (time <= present && present < timeEnd) {
                return {
                    style: generalCss + " bg-green-500",
                    display: "Đang diễn ra"
                }
            } else {
                return {
                    style: generalCss + " bg-gray-500",
                    display: "Đã qua"
                }
            }
        },
        async fetchBalance() {
            const balanceResponse = await axios.get(import.meta.env.VITE_API_URL + '/api/User/balance', {
                headers: {
                    'Authorization': "Bearer " + localStorage.token
                }
            })
            if (balanceResponse.data) {
                this.balance = balanceResponse.data.data.balance
            }
        },
        async getUserSlots(from, to) {
            let queryString = "?"
            if (from != null){
                queryString += "From=" + from
            }
            if (to != null){
                queryString += "&To=" + to
            }
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/SlotStudent/get-slots-of-students' + queryString, {
                headers: {
                    'Authorization': "Bearer " + localStorage.token
                }
            })
            if (response.data) {
                this.slots = response.data
                this.upcomingSlot = this.getClosestSlot(this.slots)
            }
        },
        async refresh() {
            try {
                await this.getUserSlots(this.from, this.to);
                await this.fetchBalance();
            } catch (e){
                console.log(e)
            }
        },
        openSlotDetailPopup(slot){
            this.selectingSlot = slot
            this.isOpenSlotDetailPopup = true
        },
        closeSlotDetailPopup(){
            this.isOpenSlotDetailPopup = false
        }
    },
    mounted() {
        this.refresh()
    }
}
</script>

<style></style>
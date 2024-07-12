<template>
    <div>
        <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200 ">
            Slot học tiếp theo
        </div>
        <div class="m-8">
            <div class="flex gap-4 mb-2">
                <div class="text-xl font-bold py-1">
                    <span class="mr-4 ">Số dư hiện tại : </span>
                    <span class="text-green-200 p-1 bg-green-600 rounded-lg">{{ user.balance.toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                    }) }} </span>
                </div>
                <router-link to="/student/payment" class="mr-6 p-1 text-xl font-bold text-white bg-blue-400 hover:bg-blue-200 rounded-lg">
                    Nạp tiền
                </router-link >

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
                        *Khi slot bắt đầu, hệ thống sẽ tự quét trừ tiền trong ví của quý khách. Để trách những rắc rối về sau, bạn vui lòng nạp tiền vào ví đầy đủ trước khi bắt đầu vào học nhé!<br>
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
        <time-table :slots="slots"/>
    </div>

</template>

<script>
import TimeTable from './TimeTable.vue'
export default {
    components: { TimeTable },
    name: "StudentProfileSchedule",
    data() {
        return {
            user: {
                balance: 100000
            },
            slots: [
                {
                    startTime: "2024-06-22 16:50:00",
                    endTime: "2024-06-22 17:30:00",
                    paidStatus: "NotCharged",
                    user: {
                        salary: 80000
                    }
                },
                {
                    startTime: "2024-06-18 10:00:00",
                    endTime: "2024-06-18 11:30:00",
                    paidStatus: "Charged",
                    user: {
                        salary: 120000
                    }
                },
                {
                    startTime: "2024-06-17 7:00:00",
                    endTime: "2024-06-17 9:30:00",
                    paidStatus: "InDebt",
                    user: {
                        salary: 120000
                    }
                },
                {
                    startTime: "2024-06-19 18:30:00",
                    endTime: "2024-06-19 20:00:00",
                    paidStatus: "NotCharged",
                    user: {
                        salary: 120000
                    }
                }
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
                const endTime = new Date(slot.endTime);
                return endTime > now;
            });

            // Sort the future slots by their distance from the current time
            const sortedSlots = futureSlotsOnly.map((slot) => {
                const startTime = new Date(slot.startTime);
                const endTime = new Date(slot.endTime);
                const user = slot.user
                const paidStatus = slot.paidStatus
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
        }
    },
    mounted() {
        this.upcomingSlot = this.getClosestSlot(this.slots)
    }
}
</script>

<style></style>
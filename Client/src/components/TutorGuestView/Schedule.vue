<template>
    <div>
        <div>
            <time-table :slots="slots" :fetching="getUserSlots" :viewDetail="openSlotDetailPopup" />
        </div>
        <generic-popup v-if="isOpenSlotDetailPopup" title="Chi tiết buổi học" :closeFunction="closeSlotDetailPopup"
            :notOverflow="true">
            <slot-detail-popup :slot="selectingSlot" :close="closeSlotDetailPopup" :action="refresh" />
        </generic-popup>
    </div>
</template>

<script>
import axios from "axios";
import TimeTable from "../StudentProfile/TimeTable.vue";
import GenericPopup from "../common/GenericPopup.vue";
import SlotDetailPopup from "../StudentProfile/SlotDetailPopup.vue";

export default {
    components: { TimeTable, GenericPopup, SlotDetailPopup },
    name: "TutorProfileSchedule",
    props: ["tutor"],
    data() {
        return {
            balance: 0,
            isOpenSlotDetailPopup: false,
            slots: [],
            upcomingSlot: null,
            showModal: false,
            selectingSlot: null,
        };
    },
    methods: {
        getStatusDisplay(status) {
            let css = "px-4 py-1 text-white font-bold rounded-lg text-center";
            switch (status) {
                case "Finished":
                    return {
                        css: css + " bg-blue-500",
                        display: "Đã hoàn thành",
                    };
                default:
                    return {
                        css: css + " bg-gray-500",
                        display: "Không rõ",
                    };
            }
        },
        getSlotStatus(startTime, endTime) {
            let generalCss = "p-2 text-white font-bold rounded-lg";
            const time = new Date(startTime);
            const timeEnd = new Date(endTime);
            const present = new Date();
            if (time > present) {
                return {
                    style: generalCss + " bg-gray-500",
                    display: "Sắp bắt đầu",
                };
            } else if (time <= present && present < timeEnd) {
                return {
                    style: generalCss + " bg-green-500",
                    display: "Đang diễn ra",
                };
            } else {
                return {
                    style: generalCss + " bg-gray-500",
                    display: "Đã qua",
                };
            }
        },

        async getUserSlots() {
            const userId = this.tutor.id;

            try {
                const response = await axios.get(
                    `${import.meta.env.VITE_API_URL
                    }/api/Slot?Filter.UserId=${userId}
                    &Filter.SlotStatus=0
                    &Filter.SlotStatus=1
                    &Filter.SlotStatus=3
                    &Filter.ClassId=0
                    &Page=1&Limit=100`,
                    {
                        headers: {
                            Authorization: `Bearer ${localStorage.token}`,
                        },
                    }
                );
                this.slots = []
                console.log(response.data); // Log the response data for debugging
                if (response.data && response.data.items) {
                    for (var slot of response.data.items) {
                        this.slots.push({
                            slot: {
                                id : slot.id,
                                startTime: slot.startTime,
                                endTime: slot.endTime,
                                teachAddress: slot.teachAddress,
                                isOnline: slot.isOnline,
                                createdBy: this.tutor,
                                subject: slot.subject,
                                class: slot.class,
                                numberOfStudents : slot.numberOfStudents
                            },
                            paymentStatus: -2
                        })
                    }
                }

            } catch (error) {
                console.error("Error fetching user slots:", error);
                this.slots = []; // Handle errors by setting slots to an empty array
            }
        },
        async refresh() {
            try {
                await this.getUserSlots();
            } catch (e) {
                console.log(e);
            }
        },
        openSlotDetailPopup(slot) {
            this.selectingSlot = slot;
            this.isOpenSlotDetailPopup = true;
        },
        closeSlotDetailPopup() {
            this.isOpenSlotDetailPopup = false;
        },
        calcDuration(slot) {
            const startTime = new Date(slot.startTime);
            const endTime = new Date(slot.endTime);
            return (endTime - startTime) / 3600000;
        },
    },
    mounted() {
        this.refresh();
    },
};
</script>

<style>
/* Add any additional styles if necessary */
</style>
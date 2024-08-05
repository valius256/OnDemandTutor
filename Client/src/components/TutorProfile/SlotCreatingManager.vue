<template>
    <div>
        <form @submit.prevent="addSlot(true)" class="flex flex-col lg:flex-row lg:place-content-between mt-4 mx-8">
            <div class="flex mb-4">
                <label for="date" class="block font-bold p-2 w-32">Ngày</label>
                <input type="date" id="date" v-model="newSlot.date" required
                    class="w-full p-2 border border-gray-300 rounded" />
            </div>
            <div class="flex mb-4">
                <label for="startTime" class="block font-bold w-32 p-2">Bắt đầu:</label>
                <input type="time" id="startTime" v-model="newSlot.startTime" required
                    class="w-full p-2 border border-gray-300 rounded" />
            </div>
            <div class="flex mb-4">
                <label for="endTime" class="block font-bold w-32 p-2">Kết thúc:</label>
                <input type="time" id="endTime" v-model="newSlot.endTime" required
                    class="w-full p-2 border border-gray-300 rounded" />
            </div>
            <div>
                <button class="bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg py-2 px-4" type="submit">
                    Thêm slot
                </button>
            </div>

        </form>
        <div class="mx-8 flex flex-wrap gap-4">
            <div v-for="(slot, index) in classSlots" :key="index"
                class="border border-blue-300 rounded-xl p-2 flex gap-4">
                <div>
                    <span class="font-bold mr-4">Buổi {{ index + 1 }}:</span>
                    {{ slot.startTime.substring(0, 10) }}
                    {{ (slot.startTime.substring(11, 19)) }} đến
                    {{ (slot.endTime.substring(11, 19)) }}
                </div>

                <button @click="removeSlot(slot.startTime, slot.endTime)">
                    <i class="fa fa-remove"></i>
                </button>
            </div>
        </div>
        <div class="flex flex-col lg:flex-row lg:place-content-between mx-8">
            <div class="font-bold text-orange-400 italic text-center">
                Slot của lớp sẽ có màu cam
            </div>
            <div class="flex gap-4">
                <button class="py-2 px-10 rounded-lg text-white font-bold" :disabled="!isAbleToCopy()"
                    :class="{ ' bg-blue-300 hover:bg-blue-100 ': isAbleToCopy(), 'bg-gray-500': !isAbleToCopy() }"
                    @click.stop="handleCopy">
                    Copy
                    <i class="fa fa-copy"></i>
                </button>
                <button class="py-2 px-10 rounded-lg text-white font-bold"
                    :disabled="copiedSlots.length == 0 || !pickedDay"
                    :class="{ 'bg-blue-300 hover:bg-blue-100': copiedSlots.length > 0 && pickedDay, 'bg-gray-500': copiedSlots.length == 0 || !pickedDay }"
                    @click.stop="handlePaste">
                    Dán
                    <i class="fa fa-paste"></i>
                </button>
            </div>
        </div>

        <time-table :slots="existedSlots" role="tutorCreating" :fetching="getUserSlots" :day-picked="pickedDay"
            :set-picked-day="setPickedDay"></time-table>
    </div>
</template>

<script>
import TimeTable from '../StudentProfile/TimeTable.vue';
import axios from 'axios';

export default {
    name: "SlotCreatingManager",
    inject: ['eventBus'],
    components: { TimeTable },
    props: ['slots','fetching','setClassSlot'],
    data() {
        return {
            classData: null,
            classSlots: [
            ],
            newSlot: {
                date: null,
                startTime: null,
                endTime: null,
            },
            pickedDay: "",
            copiedSlots: [

            ],
            existedSlots : [

            ]
        }
    },
    mounted(){    
        this.existedSlots = this.slots
        
        this.eventBus.on("class-creator-remove-slot", (params) => {
            this.removeSlot(params.start, params.end)
        })
        this.eventBus.on("class-creator-select-slot", (params) => {
            this.selectSlot(params.slot, params.isSelect)
        })
    },
    beforeUnmount() {
        this.eventBus.off("class-creator-remove-slot")
        this.eventBus.off("class-creator-select-slot")
    },
    methods : {
        async getUserSlots(){
            await this.fetching()
            this.appendSlot()
        },
        addSlot(showMessage) {
            const startHour = this.formatDatetime(this.newSlot.date, this.newSlot.startTime)
            const endHour = this.formatDatetime(this.newSlot.date, this.newSlot.endTime)
            const startTimeDate = new Date(startHour)
            const endTimeDate = new Date(endHour)
            
            if (startTimeDate > endTimeDate) {
                this.eventBus.emit("open-result-dialog", {
                    message: "Thời gian bắt đầu không thể lớn hơn thời gian kết thúc",
                    type: "Error",
                });
                return;
            }
            if (startTimeDate < new Date()) {
                this.eventBus.emit("open-result-dialog", {
                    message: "Thời gian bắt đầu phải ở trong tương lai",
                    type: "Error",
                });
                return;
            }
            const duration = (endTimeDate - startTimeDate) / 3600000
            if (duration < 0.25 || duration > 4) {
                this.eventBus.emit("open-result-dialog", {
                    message: "Slot chỉ có thời lượng từ 15 phút đến 1 tiếng",
                    type: "Error",
                });
                return;
            }
            for (var slot of this.slots) {
                const slotStart = new Date(slot.startTime)
                const slotEnd = new Date(slot.endTime)
                if (startTimeDate <= slotEnd && endTimeDate >= slotStart) {
                    this.eventBus.emit("open-result-dialog", {
                        message: "Slot đã bị trùng lặp với các slot có sẵn của bạn. Vui lòng kiểm tra lại",
                        type: "Error",
                    });
                    return;
                }
            }
            for (var slot of this.classSlots) {
                const slotStart = new Date(slot.startTime)
                const slotEnd = new Date(slot.endTime)
                if (startTimeDate <= slotEnd && endTimeDate >= slotStart) {
                    this.eventBus.emit("open-result-dialog", {
                        message: "Slot đã bị trùng lặp với các slot có sẵn của bạn. Vui lòng kiểm tra lại",
                        type: "Error",
                    });
                    return;
                }
            }
            const newSlot = {
                startTime: startHour,
                endTime: endHour,
                isClass: true,
                isSelected: false
            }
            this.classSlots.push(newSlot)
            this.classSlots.sort((a, b) => new Date(a.startTime) - new Date(b.startTime))
            this.existedSlots.push(newSlot)
            this.setClassSlot(this.classSlots)
            if (showMessage) {
                this.eventBus.emit("open-result-dialog", {
                    message: "Thêm thành công",
                    type: "Success",
                });
            }

        },
        removeSlot(start, end) {
            //console.log(start, end)
            this.classSlots = this.classSlots.filter(s => s.startTime != start && s.endTime != end)
            this.existedSlots = this.existedSlots.filter(s => s.startTime != start && s.endTime != end)
            this.setClassSlot(this.classSlots)
        },
        setPickedDay(day) {
            this.pickedDay = day
        },
        isAbleToCopy() {
            var selectedList = this.classSlots.filter(s => s.isSelected)
            return selectedList.length > 0
        },
        selectSlot(slot, isSelect) {
            var slot = this.classSlots.find(s => s.startTime == slot.startTime && s.endTime == slot.endTime)
            if (slot) {
                slot.isSelected = isSelect
            }
        },
        handleCopy() {
            this.copiedSlots = []
            const selectedSlots = this.classSlots.filter(s => s.isSelected)
            for (var slot of selectedSlots) {
                if (!this.copiedSlots.find(s => s.startTime == slot.startTime && s.endTime == slot.endTime)) {
                    this.copiedSlots.push(slot)
                }
            }
        },
        handlePaste() {
            const date = new Date(this.slashDateFormatToSqlDateString(this.pickedDay))
            const firstSlot = this.copiedSlots.sort((a, b) => new Date(a.startTime) - new Date(b.startTime))[0]
            if (firstSlot) {
                const startDate = new Date(firstSlot.startTime.substring(0, 10))
                const durationInDay = (date - startDate) / 3600000 / 24
                for (var slot of this.copiedSlots) {
                    const slotStartDate = new Date(slot.startTime)
                    const slotEndDate = new Date(slot.endTime)
                    slotStartDate.setDate(slotStartDate.getDate() + durationInDay)
                    this.newSlot.date = this.toSqlDateString(slotStartDate)
                    this.newSlot.startTime = this.toTimeString(slotStartDate).substring(0,5)
                    this.newSlot.endTime = this.toTimeString(slotEndDate).substring(0,5)
                    console.log(this.newSlot)
                    this.addSlot(false)
                }
            }

        },
        appendSlot() {
            for (var slot of this.classSlots) {
                this.existedSlots.push(slot)
            }
        },
    }
}
</script>

<style></style>
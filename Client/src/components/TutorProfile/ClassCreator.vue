<template>
    <div class="">
        <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200">
            Trình tạo lớp học
        </div>
        <div class="flex justify-end mr-4">
            <button @click="this.$router.go(-1)"
                class="font-bold bg-blue-400 text-white rounded-lg hover:bg-blue-200 px-12 py-2">
                Trở lại
            </button>
        </div>
        <div class="text-xl font-bold mb-2 px-8 italic">
            Thông tin chung
        </div>
        <hr>
        <div class="p-8">
            <div class="flex place-content-between gap-4">
                <div class="flex gap-4 w-full">
                    <span class="w-32 p-2">Tên lớp </span>
                    <input v-model="createDto.name" type="text" placeholder="Nhập tên lớp"
                        class="border rounded-lg p-2 w-full" />
                </div>
                <div class="flex gap-4  w-full">
                    <span class="w-64 p-2">Số lượng học sinh</span>
                    <input v-model="createDto.numberOfStudents" type="number" placeholder="Nhập số học sinh"
                        class="border rounded-lg p-2 w-full" />
                </div>
            </div>

            <div class="flex place-content-between gap-4 mt-8">
                <div class="flex gap-4 w-full">
                    <span class="w-32 p-2">Môn học</span>
                    <select v-model="createDto.subjectId" class="border rounded-lg p-2 w-full">
                        <option v-for="subject in subjects" :key="subject.id" :value="subject.id">
                            {{ subject.name }}
                        </option>
                    </select>
                </div>
                <div class="flex gap-4  w-full">
                    <span class="w-64 p-2">Địa chỉ dạy</span>
                    <input v-model="createDto.teachAddress" type="text" placeholder="Nhập địa chỉ"
                        class="border rounded-lg p-2 w-full" />
                </div>
            </div>
            <div class="flex place-content-between gap-4 mt-8">
                <div class="flex gap-4  w-full">
                    <span class="w-32 p-2">Học Online</span>
                    <input v-model="createDto.isOnline" type="checkbox" class="border rounded-lg p-2" />
                </div>
            </div>

        </div>



        <div class="text-xl font-bold mb-2 px-8 italic">
            Thời khóa biểu
        </div>
        <hr>
        <form @submit.prevent="addSlot" class="flex flex-col lg:flex-row lg:place-content-between mt-4 mx-8">
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
                <button class="py-2 px-10 bg-blue-300 hover:bg-blue-100 rounded-lg text-white font-bold">
                    Copy
                    <i class="fa fa-copy"></i>
                </button>
                <button class="py-2 px-10 bg-blue-300 hover:bg-blue-100 rounded-lg text-white font-bold">
                    Dán
                    <i class="fa fa-paste"></i>
                </button>
            </div>
        </div>

        <time-table :slots="slots" role="tutorCreating" :fetching="getUserSlots" :day-picked="pickedDay"
            :set-picked-day="setPickedDay"></time-table>
        <div class="flex justify-center gap-6">
            <button v-if="classData == null"
                class="mt-3 hover:bg-blue-200 bg-blue-400 text-white py-3 px-12 rounded-lg text-lg font-bold w-full mx-4 mb-4">
                Tạo lớp
            </button>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { useRoute } from 'vue-router';
import StarRating from 'vue-star-rating'
import TimeTable from '../StudentProfile/TimeTable.vue';

export default {
    name: "CreateClassPage",
    inject: ['eventBus'],
    components: { StarRating, TimeTable },
    props: ['id', 'currentUser'],
    data() {
        return {
            classId: 0,
            slots: [

            ],
            subjects: [
            ],
            classData: null,
            createDto: {
                name: "",
                subjectId: 0,
                numberOfStudents: 1,
                teachAddress: "",
                isOnline: false
            },
            classSlots: [
            ],
            newSlot: {
                startTime: null,
                endTime: null,
                isClass: true
            },
            pickedDay: ""
        }
    },
    mounted() {
        this.route = useRoute();
        this.classId = this.route.params.id;
        if (this.classId != 0) {
            this.getData()
        }
        this.fetchSubjects()

        this.eventBus.on("class-creator-remove-slot", (params) => {
            this.removeSlot(params.start, params.end)
        })
    },
    methods: {
        getData() {
            //This will remove later

        },
        getStatusStyle(status) {
            let css = "text-center font-bold text-white rounded-lg"
            switch (status) {
                case "Pending":
                    return css + " bg-red-400"
                case "Done":
                    return css + " bg-green-400"
            }
        },
        async fetchSubjects() {
            try {
                const response = await axios.get(
                    import.meta.env.VITE_API_URL + "/api/TutorSubject",
                    {
                        params: {
                            "Filter.TutorId": this.id,
                            Status: 3,
                        },
                        headers: {
                            Authorization: "Bearer " + localStorage.token,
                        },
                    }
                );

                // Map the response to the desired structure
                this.subjects = response.data.items.map((item) => ({
                    id: item.subject.id,
                    name: item.subject.name,
                }));
            } catch (error) {
                console.error("Error fetching subjects:", error);
            }
        },
        async getUserSlots(from, to) {
            const userId = this.currentUser.id;
            try {
                const response = await axios.get(
                    `${import.meta.env.VITE_API_URL
                    }/api/Slot?Filter.UserId=${userId}&Page=1&Limit=100`,
                    {
                        headers: {
                            Authorization: `Bearer ${localStorage.token}`,
                        },
                    }
                );
                this.slots = response.data.items
                this.appendSlot()
            } catch (error) {
                console.error("Error fetching user slots:", error);
                this.slots = []; // Handle errors by setting slots to an empty array
            }
        },
        appendSlot() {
            for (var slot of this.classSlots) {
                this.slots.push(slot)
            }
        },
        addSlot() {
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
                isClass: true
            }
            this.classSlots.push(newSlot)
            this.classSlots.sort((a, b) => new Date(a.startTime) - new Date(b.startTime))
            this.slots.push(newSlot)
            this.eventBus.emit("open-result-dialog", {
                message: "Thêm thành công",
                type: "Success",
            });
        },
        removeSlot(start, end) {
            //console.log(start, end)
            this.classSlots = this.classSlots.filter(s => s.startTime != start && s.endTime != end)
            this.slots = this.slots.filter(s => s.startTime != start && s.endTime != end)
        },
        setPickedDay(day) {
            this.pickedDay = day
        }

    }
}
</script>

<style></style>
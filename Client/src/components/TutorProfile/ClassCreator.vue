<template>
    <div class="">
        <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200">
            Trình tạo lớp học
        </div>
        <div v-if="currentUser.tutorStatus == 3">

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
            <slot-creating-manager :fetching="getUserSlots" :slots="slots"
                :setClassSlot="setClassSlot"></slot-creating-manager>
            <div class="flex justify-center gap-6">
                <button v-if="classData == null" @click="handleAdd(true)"
                    class="mt-3 hover:bg-blue-200 bg-blue-400 text-white py-3 px-12 rounded-lg text-lg font-bold w-full mx-4 mb-4">
                    Tạo lớp
                </button>
            </div>
        </div>
        <div v-else class="p-8">
            <div class="p-8 bg-red-200 rounded-lg text-center font-bold">
                Bạn cần xác thực tài khoản để sử dụng tính năng này

            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { useRoute } from 'vue-router';
import SlotCreatingManager from './SlotCreatingManager.vue';

export default {
    name: "CreateClassPage",
    inject: ['eventBus'],
    components: { SlotCreatingManager },
    props: ['id', 'currentUser'],
    data() {
        return {
            classId: 0,
            slots: [

            ],
            classSlots: [],
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
        }
    },
    mounted() {
        this.setup()
    },
    methods: {
        async setup() {
            this.route = useRoute();
            this.classId = this.route.params.id;
            if (this.classId != 0) {
                await this.getUserSlots()
            }
            await this.fetchSubjects()
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
        setClassSlot(classSlots) {
            this.classSlots = classSlots
        },
        async fetchSubjects() {
            try {
                const response = await axios.get(
                    import.meta.env.VITE_API_URL + "/api/TutorSubject",
                    {
                        params: {
                            "Filter.TutorId": this.id,
                            "Filter.Status": 3,
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
            } catch (error) {
                console.error("Error fetching user slots:", error);
                this.slots = []; // Handle errors by setting slots to an empty array
            }
        },
        async handleAdd(confirmation) {
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn tạo lớp?",
                    method: this.handleAdd,
                    params: false
                })
            } else {
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    await axios.post(import.meta.env.VITE_API_URL + '/api/Class', {
                        name: this.createDto.name,
                        subjectId: this.createDto.subjectId,
                        location: this.createDto.teachAddress,
                        method: this.createDto.isOnline ? "Online" : "Offline",
                        numberOfStudents: this.createDto.numberOfStudents,
                        slotList: this.classSlots
                    }, {
                        headers: {
                            "Authorization": "Bearer " + localStorage.token
                        }
                    })
                    this.eventBus.emit("open-result-dialog", {
                        message: "Tạo lớp thành công",
                        type: "Success"
                    })
                    await this.$router.push('/tutor/myclass/list')
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Không thể tạo lớp. Vui lòng thử lại sau",
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        }

    }
}
</script>

<style></style>
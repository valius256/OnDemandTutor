<template>
    <div class="p-4">
        <div class="text-2xl font-bold mb-6">
            Tạo lớp học
        </div>
        <div class="text-xl font-bold mb-2 px-8 italic">
            Thông tin chung
        </div>
        <div class="p-8">
            <div class="flex place-content-between gap-4">
                <div class="flex gap-4 w-full">
                    <span class="w-32 p-2">Tên lớp </span>
                    <input v-model="createDto.name" type="text" placeholder="Nhập tên lớp"
                        class="border rounded-lg p-2 w-full" />
                </div>
                <div class="flex gap-4  w-full">
                    <span class="w-32 p-2">Số lượng học sinh</span>
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
                    <span class="w-32 p-2">Địa chỉ dạy Offline</span>
                    <input v-model="createDto.teachAddress" type="text" placeholder="Nhập địa chỉ"
                        class="border rounded-lg p-2 w-full" />
                </div>
            </div>

            <div class="flex place-content-between gap-4 mt-8">
                <div class="flex gap-4 w-full">
                    <span class="w-32 p-2">Gia sư</span>
                    <div class="flex gap-4">
                        <input type="text" placeholder="Nhập tên gia sư" class="border rounded-lg p-2" />
                        <button class="py-2 px-4 text-white bg-slate-400 hover:bg-slate-600 rounded-xl">
                            <i class="fa fa-search" />
                        </button>
                    </div>
                </div>
            </div>
            <div class="flex flex-col lg:flex-row lg:place-content-between mt-4 gap-4">
                <div>
                    <div class="grid grid-cols-1 sm:grid-cols-3 gap-4 text-center w-full" v-if="tutors.length > 0">
                        <div v-for="tutor in tutors" :key="tutor.id" class="px-4 py-6 mb-8 bg-white rounded-lg shadow">
                            <img :src="tutor.avatar" />
                            <p class="mt-4 font-bold">{{ tutor.name }}</p>
                            <div class="flex justify-center">
                                <star-rating :star-size="20" :rating="tutor.rating" :round-start-rating="false"
                                    :read-only="true" />
                            </div>
                            <button @click="setSelectedTutor(tutor.id)"
                                class="mt-3 bg-slate-50 text-blue-600 py-3 px-6 rounded-full text-lg font-bold">Chọn</button>
                        </div>
                    </div>
                    <div class="italic w-full" v-else>
                        Hãy tìm một gia sư để dạy lớp này
                    </div>
                    <div class="flex gap-4 justify-center mt-2">
                        <button @click="movePage(false)">
                            <i class="fa fa-arrow-left text-2xl"></i>
                        </button>
                        <div class="flex gap-2 ">
                            <input class="border p-1 rounded-md w-16" type="number" v-model="currentPage" min="1"
                                @change="handlePageChange">
                            <div class="p-1"> / {{ this.totalPage }}</div>
                        </div>
                        <button @click="movePage(true)">
                            <i class="fa fa-arrow-right text-2xl"></i>
                        </button>
                    </div>
                </div>

                <div class="bg-white shadow-lg rounded-lg p-6 mb-6 w-full">
                    <div v-if="selectedTutor" class="flex justify-center">
                        <img class="w-32" :src="selectedTutor.avatar">
                    </div>
                    <table class="" v-if="selectedTutor">
                        <tr>
                            <td class="font-semibold text-gray-700">Tên gia sư:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">{{ selectedTutor.name }}</td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">SDT:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">{{ selectedTutor.phone }}</td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">Email:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">{{ selectedTutor.email }}</td>
                        </tr>
                        <tr>
                            <td class="font-semibold text-gray-700">Đánh giá gia sư:</td>
                            <td class="w-32"></td>
                            <td class="text-gray-900">
                                <star-rating :star-size="20" :rating="selectedTutor.rating" :round-start-rating="false"
                                    :read-only="true" />
                            </td>
                        </tr>

                    </table>
                    <div v-else class="italic">
                        Chưa chọn gia sư
                    </div>
                </div>
            </div>

        </div>

        <div class="text-xl font-bold mb-2 px-8 italic">
            Thời khóa biểu
        </div>
        <div class="p-16 bg-slate-400 rounded-lg">
            Comming soon...
        </div>
        <div class="flex justify-center gap-6">
            <button v-if="classData == null"
                class="mt-3 bg-slate-50 hover:bg-slate-300 text-blue-600 py-3 px-6 rounded-full text-lg font-bold">
                Tạo lớp
            </button>
            <button v-if="classData != null"
                class="mt-3 bg-slate-50 hover:bg-slate-300 text-blue-600 py-3 px-6 rounded-full text-lg font-bold">
                Cập nhật
            </button>
            <button v-if="classData != null"
                class="mt-3 bg-red-50 hover:bg-red-300 text-red-600 py-3 px-6 rounded-full text-lg font-bold">
                Xóa lớp
            </button>
        </div>
    </div>
</template>

<script>
import { useRoute } from 'vue-router';
import StarRating from 'vue-star-rating'

export default {
    name: "CreateClassPage",
    components: { StarRating },
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            classId: 0,
            subjects: [
                {
                    id: 1,
                    name: "Toán"
                },
                {
                    id: 2,
                    name: "Tiếng Anh"
                },
            ],
            classData: null,
            createDto: {
                name: "",
                subjectId: 0,
                numberOfStudents: 1,
                teachAddress: ""
            },
            tutors: [
                {
                    id: 1,
                    name: "Nguyen Van A",
                    avatar: '/src/assets/noavatar.jpg',
                    email: "tutor@example.com",
                    phone: "0987654321",
                    rating: 4.5
                },
                {
                    id: 2,
                    name: "Nguyen Van A",
                    avatar: '/src/assets/noavatar.jpg',
                    email: "tutor@example.com",
                    phone: "0987654321",
                    rating: 4.5
                },
                {
                    id: 3,
                    name: "Nguyen Van A",
                    avatar: '/src/assets/noavatar.jpg',
                    email: "tutor@example.com",
                    phone: "0987654321",
                    rating: 4.5
                },
            ],
            selectedTutor: null
        }
    },
    mounted() {
        this.route = useRoute();
        this.classId = this.route.params.id;
        if (this.classId != 0) {
            this.getData()
        }
    },
    methods: {
        getData() {
            //This will remove later
            this.classData = {
                id: 0,
                name: 'Math 101',
                tutor: {
                    name: "Phale",
                    email: "tutor@example.com",
                    phone: "0987654321",
                    rating: 4.5
                },
                numberOfStudents: 3,
                subject: {
                    id: 1,
                    name: "Piano"
                },
                teachAddress: '123 Main St',
                tutor : {
                    id: 1,
                    name: "Hung Deo Trai",
                    avatar: '/src/assets/noavatar.jpg',
                    email: "hung@example.com",
                    phone: "0987654321",
                    rating: 6.9
                },
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
            }
            this.createDto.name = this.classData.name
            this.createDto.subjectId = this.classData.subject.id
            this.createDto.teachAddress = this.classData.teachAddress
            this.createDto.numberOfStudents = this.classData.numberOfStudents
            this.selectedTutor = this.classData.tutor
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
        setSelectedTutor(id) {
            this.selectedTutor = this.tutors.find(t => t.id == id)
        },
        async handlePageChange() {
            if (this.currentPage > this.totalPage) {
                this.currentPage = this.totalPage
            }
            if (this.currentPage < 1) {
                this.currentPage = 1
            }
            //await this.fetchRegistration(this.currentPage, this.pageSize, this.keyword_name)
        },
        async movePage(forward) {
            if (forward && this.currentPage < this.totalPage) {
                this.currentPage++
                await this.handlePageChange()
            } else if (!forward && this.currentPage > 1) {
                this.currentPage--
                await this.handlePageChange()
            }
        },
    }
}
</script>

<style></style>
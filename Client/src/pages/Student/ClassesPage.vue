<template>
    <div class="p-4 flex gap-4">
        <div class="w-1/4 border-r-2">
            <div class="font-bold text-xl">Bộ lọc</div>
            <div class="border rounded-lg p-2 mt-4 mr-2 shadow-lg">
                <div>
                    <div>Tên lớp</div>
                    <input class="border rounded-lg p-1 w-full" placeholder="Nhập tên lớp"
                        v-model="filterDto.className">
                </div>
                <div class="mt-4">
                    <div>Môn học</div>
                    <select class="border rounded-lg p-1 w-full" v-model="filterDto.subject">
                        <option :value="0">Tất cả</option>
                        <option :value="subject.id" v-for="subject in subjects" :key="subject.id">
                            {{ subject.name }}
                        </option>
                    </select>
                </div>
                <div class="mt-4">
                    <div>Tên gia sư</div>
                    <input class="border rounded-lg p-1 w-full" placeholder="Nhập gia sư" v-model="filterDto.tutorName">
                </div>
                <div class="mt-4">
                    <div>Nhập địa chỉ</div>
                    <input class="border rounded-lg p-1 w-full" placeholder="Nhập địa chỉ, vd Thủ Đức, TPHCM,..."
                        v-model="filterDto.address">
                </div>
                <div class="mt-4">
                    <div>Từ ngày</div>
                    <input class="border rounded-lg p-1 w-full" type="date" v-model="filterDto.fromDate">
                </div>
                <div class="mt-4">
                    <div>Đến ngày</div>
                    <input class="border rounded-lg p-1 w-full" type="date" v-model="filterDto.toDate">
                </div>
                <div class="mt-4">
                    <div>Mức giá từ (VND/h)</div>
                    <input class="border rounded-lg p-1 w-full" type="number" placeholder="Nhập mức giả rẻ nhất"
                        v-model="filterDto.fromPrice">
                </div>
                <div class="mt-4">
                    <div>Đến mức giá (VND/h)</div>
                    <input class="border rounded-lg p-1 w-full" type="number" placeholder="Nhập mức giá mắc nhất"
                        v-model="filterDto.toPrice">
                </div>
                <div class="mt-4">
                    <div>Hình thức</div>
                    <select class="border rounded-lg p-1 w-full" v-model="filterDto.method">
                        <option :value="0">Tất cả</option>
                        <option :value="1">Online</option>
                        <option :value="2">Offline</option>
                    </select>
                </div>
                <div class="flex justify-center mt-4">
                    <button class="bg-blue-500 text-white font-bold p-2 rounded-lg">Áp dụng</button>
                </div>
            </div>

        </div>
        <div class="w-3/4 px-4 py-2">
            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                <div v-for="class_ in classes" :key="class_.id" class="shadow-md rounded-lg">
                    <div :class="getStatusStyleHeader(class_.status)">
                        {{ class_.name }}
                    </div>
                    <div class="p-4 h-72 overflow-y-auto">
                        <ul class="px-4 flex flex-col gap-2">
                            <li>
                                <span class="font-bold">Gia sư :</span>
                                <button class="ml-3 font-bold text-blue-400 underline">
                                    {{ class_.tutor.name }}
                                </button>
                            </li>
                            <li>
                                <span class="font-bold">Thời gian :</span>
                                <span class="ml-3">
                                    {{ sqlDateStringToSlashFormat(class_.startDate) }} đến
                                    {{ sqlDateStringToSlashFormat(class_.endDate) }}
                                </span>
                            </li>
                            <li>
                                <span class="font-bold">Môn học :</span>
                                <span class="ml-3">{{ class_.subject.name }}</span>
                            </li>
                            <li v-if="class_.method == 'Offline'">
                                <span class="font-bold">Địa điểm :</span>
                                <span class="ml-3">{{ class_.location }}</span>
                            </li>
                            <li>
                                <span class="font-bold">Hình thức :</span>
                                <span :class="getMethodStyle(class_.method)">{{ class_.method }}</span>
                            </li>
                            <li>
                                <span class="font-bold">Trạng thái :</span>
                                <span :class="getStatusStyle(class_.status)">
                                    {{ getStatusDisplay(class_.status) }}
                                </span>
                            </li>
                            <li>
                                <span class="font-bold">Giá cả :</span>
                                <span class="ml-3 text-red-500 font-bold">
                                    {{ class_.tutor.price.toLocaleString('vi-VN', {
                            style: 'currency',
                            currency: 'VND',
                        }) }} / h
                                </span>
                            </li>
                        </ul>
                    </div>
                    <div class="flex justify-center my-2">
                        <button class="bg-blue-500 text-white font-bold p-2 rounded-lg">Xem thêm</button>
                    </div>
                </div>
            </div>
            <div class="flex gap-4 justify-center mt-4" v-if="classes.length > 0">
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
    </div>
</template>

<script>
export default {
    name: "ClassesPage",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            classes: [
                {
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
                        name: "Thomas",
                        price: 50000
                    },
                    status: "NotYet"
                },
                {
                    id: 2,
                    name: "Luyện thi TOEFL cơ bản",
                    startDate: "2024-02-01",
                    endDate: "2024-10-01",
                    method: "Offline",
                    location: "Q3, TPHCM",
                    subject: {
                        name: "Tiếng Anh"
                    },
                    studentNumber: 15,
                    maxStudentNumber: 25,
                    tutor: {
                        name: "Thomas",
                        price: 50000
                    },
                    status: "NotYet"
                },
                {
                    id: 3,
                    name: "Luyện thi đại học môn Toán",
                    startDate: "2024-03-01",
                    endDate: "2024-09-01",
                    method: "Online",
                    location: "Q7, TPHCM",
                    subject: {
                        name: "Toán"
                    },
                    studentNumber: 12,
                    maxStudentNumber: 30,
                    tutor: {
                        name: "Thomas",
                        price: 50000
                    },
                    status: "OnGoing"
                },
                {
                    id: 4,
                    name: "Luyện thi đại học môn Lý",
                    startDate: "2024-04-01",
                    endDate: "2024-11-01",
                    method: "Offline",
                    location: "Q5, TPHCM",
                    subject: {
                        name: "Vật Lý",
                        price: 50000
                    },
                    studentNumber: 8,
                    maxStudentNumber: 20,
                    tutor: {
                        name: "Arthur",
                        price: 50000
                    },
                    status: "NotYet"
                },
                {
                    id: 5,
                    name: "Khóa học lập trình Python cơ bản",
                    startDate: "2024-05-01",
                    endDate: "2024-08-01",
                    method: "Online",
                    location: "Q10, TPHCM",
                    subject: {
                        name: "Khoa học máy tính"
                    },
                    studentNumber: 20,
                    maxStudentNumber: 30,
                    tutor: {
                        name: "Arthur",
                        price: 50000
                    },
                    status: "Finished"
                },
                {
                    id: 6,
                    name: "Khóa học lập trình Java nâng cao",
                    startDate: "2024-06-01",
                    endDate: "2024-12-01",
                    method: "Offline",
                    location: "Q1, TPHCM",
                    subject: {
                        name: "Khoa học máy tính"
                    },
                    studentNumber: 18,
                    maxStudentNumber: 25,
                    tutor: {
                        name: "Arthur",
                        price: 50000
                    },
                    status: "NotYet"
                },
                {
                    id: 7,
                    name: "Khóa học quản lý dự án PMP",
                    startDate: "2024-07-01",
                    endDate: "2024-12-01",
                    method: "Online",
                    location: "Q2, TPHCM",
                    subject: {
                        name: "Quản lý dự án"
                    },
                    studentNumber: 25,
                    maxStudentNumber: 30,
                    tutor: {
                        name: "John",
                        price: 50000
                    },
                    status: "NotYet"
                }
            ],
            subjects: [
                {
                    id: 1,
                    name: "Toán"
                },
                {
                    id: 2,
                    name: "Tiếng Anh"
                },
                {
                    id: 3,
                    name: "Lý"
                }
            ],
            filterDto: {
                className: "",
                subject: 0,
                tutorName: "",
                address: "",
                fromDate: null,
                toDate: null,
                fromPrice: null,
                toPrice: null,
                method: 0
            }
        }
    },
    methods: {
        getMethodStyle(method) {
            let general = "ml-4 rounded-lg px-3 py-1 text-white font-bold"
            switch (method) {
                case "Online":
                    return general + " bg-green-400"
                default:
                    return general + " bg-gray-400"
            }
        },
        getStatusStyleHeader(status) {
            let general = "font-bold text-center py-4 rounded-t-lg text-white"
            switch (status) {
                case "NotYet":
                    return general + " bg-cyan-500"
                case "OnGoing":
                    return general + " bg-green-400"
                default:
                    return general + " bg-gray-400"
            }
        },
        getStatusStyle(status) {
            let general = "ml-3 rounded-lg px-3 py-1 font-bold"
            switch (status) {
                case "NotYet":
                    return general + " text-blue-400"
                case "OnGoing":
                    return general + " text-green-400"
                default:
                    return general + " text-gray-400"
            }
        },
        getStatusDisplay(status) {
            switch (status) {
                case "NotYet":
                    return "Sắp bắt đầu"
                case "OnGoing":
                    return "Đang diễn ra"
                case "Finished":
                    return "Đã kết thúc"
                default:
                    return "Không rõ"
            }
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
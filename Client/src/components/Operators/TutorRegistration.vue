<template>
    <div class="p-4 w-full">
        <div class="flex justify-end gap-2">
            <input class="border p-1 rounded-md w-64" type="text" v-model="keyword_name" placeholder="Search by name">
            <button class="p-1 px-4 bg-slate-100 hover:bg-slate-300 rounded-lg" @click="handleSearch">
                <i class="fa fa-search	"></i>
            </button>
        </div>
        <table id="operator-table" class="table-auto overflow-x-auto">
            <thead>
                <tr>
                    <th class="w-1/12">Id</th>
                    <th class="w-2/12">Tên</th>
                    <th class="w-2/12">Ảnh</th>
                    <th class="w-4/12">Đăng ký môn</th>
                    <th class="w-2/12">Email</th>
                    <th class="w-2/12">SDT</th>
                    <th class="w-2/12">Hành động</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="tutor in tutors" :key="tutor.id">
                    <td>{{ tutor.id }}</td>
                    <td><button class="font-bold underline text-blue-400">{{ tutor.name }}</button></td>
                    <td><img :src="tutor.avatar" class="w-24 h-24"></td>
                    <td><div class="flex flex-wrap gap-1" v-html="displaySubjects(tutor.subjects)"></div></td>
                    <td class="break-all">{{ tutor.email }}</td>
                    <td>{{ tutor.phone }}</td>
                    <td class="flex flex-col gap-2">
                        <button class="text-white rounded-lg bg-lime-500 hover:bg-lime-200 font-bold text-lg p-2"  @click="handleAccept(registration.id)">
                            Duyệt
                        </button>
                        <button class="text-white rounded-lg bg-red-500 hover:bg-red-200 font-bold text-lg p-2" @click="handleReject(registration.id)">
                            Từ chối
                        </button>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="flex gap-4 justify-center mt-4" v-if="this.tutors.length > 0">
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
</template>

<script>
export default {
    name: "TutorRegistration",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            keyword_name: "",
            tutors: [
                {
                    id: 1,
                    name: "Nguyen Van A",
                    email: "abc@gmail.com",
                    phone: "0987654321",
                    avatar: "/src/assets/noavatar.jpg",
                    subjects : [
                        {
                            name : "Toán"
                        },
                        {
                            name : "Tiếng Anh"
                        },
                        {
                            name : "Vật Lý"
                        },
                    ]
                },
                {
                    id: 2,
                    name: "Nguyen Van A",
                    email: "abc@gmail.com",
                    phone: "0987654321",
                    avatar: "/src/assets/noavatar.jpg",
                    status: "Active",
                    subjects : [
                        {
                            name : "Toán"
                        },
                        {
                            name : "Tiếng Anh"
                        },
                        {
                            name : "Vật Lý"
                        },
                    ]
                },
                {
                    id: 3,
                    name: "Nguyen Van A",
                    email: "abc@gmail.com",
                    phone: "0987654321",
                    avatar: "/src/assets/noavatar.jpg",
                    status: "Fired",
                    subjects : [
                        {
                            name : "Toán"
                        },
                        {
                            name : "Piano, organ"
                        },
                    ]
                },
                {
                    id: 4,
                    name: "Nguyen Van A",
                    email: "abc@gmail.com",
                    phone: "0987654321",
                    avatar: "/src/assets/noavatar.jpg",
                    status: "Left",
                    subjects : [
                        {
                            name : "Tiếng Nhật"
                        },
                        {
                            name : "Tiếng Anh"
                        },
                    ]
                },
            ],
        }
    },
    methods: {
        displaySubjects(subjects){
            let color = "gray"
            let html = ""
            for (var subject of subjects){
                switch(subject.name){
                    case "Toán" :
                        color = "border-orange-400"
                        break;
                    case "Tiếng Anh" :
                        color = "border-green-400"
                        break;
                    case "Tiếng Nhật" :
                        color = "border-pink-400"
                        break;
                }
                var style = `rounded-lg py-2 px-6 border ${color}`
                html += `<span class="${style}">${subject.name}</span>`
            }
            return html
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
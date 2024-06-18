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
                    <th class="w-3/12">Tên</th>
                    <th class="w-2/12">Ảnh</th>
                    <th class="w-4/12">Email</th>
                    <th class="w-3/12">SDT</th>
                    <th class="w-3/12">Trạng thái</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="tutor in tutors" :key="tutor.id">
                    <td>{{ tutor.id }}</td>
                    <td><button class="font-bold underline text-blue-400">{{ tutor.name }}</button></td>
                    <td><img :src="tutor.avatar" class="w-24 h-24"></td>
                    <td class="break-all">{{ tutor.email }}</td>
                    <td>{{ tutor.phone }}</td>
                    <td>
                        <div :class="getStatusStyle(tutor.status)">{{ tutor.status }}</div>
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
    name: "AdminTutorList",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            keyword_name : "",
            tutors: [
                {
                    id: 1,
                    name: "Nguyen Van A",
                    email: "abc@gmail.com",
                    phone: "0987654321",
                    avatar: "/src/assets/noavatar.jpg",
                    status: "Active"
                },
                {
                    id: 2,
                    name: "Nguyen Van A",
                    email: "abc@gmail.com",
                    phone: "0987654321",
                    avatar: "/src/assets/noavatar.jpg",
                    status: "Active"
                },
                {
                    id: 3,
                    name: "Nguyen Van A",
                    email: "abc@gmail.com",
                    phone: "0987654321",
                    avatar: "/src/assets/noavatar.jpg",
                    status: "Fired"
                },
                {
                    id: 4,
                    name: "Nguyen Van A",
                    email: "abc@gmail.com",
                    phone: "0987654321",
                    avatar: "/src/assets/noavatar.jpg",
                    status: "Left"
                },
            ],
        }
    },
    methods: {
        getStatusStyle(status) {
            let css = "text-center font-bold text-white rounded-lg"
            switch (status) {
                case "Active":
                    return css + " bg-green-400"
                case "Left":
                    return css + " bg-gray-400"
                case "Fired":
                    return css + " bg-red-400"
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
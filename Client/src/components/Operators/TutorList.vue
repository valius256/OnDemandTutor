<template>
    <div class="p-4 w-full" @click="setSelectId(0)">
        <div class="flex justify-end gap-2">
            <button class="py-2 px-4 bg-slate-500 hover:bg-slate-300 text-white font-bold rounded-lg"
                @click="handleFilter">
                <i class="fa fa-search	"></i> Filter
            </button>
        </div>
        <table id="operator-table">
            <thead>
                <tr>
                    <th class="w-1/12">Id</th>
                    <th class="w-2/12">Tên</th>
                    <th class="w-2/12">Ảnh</th>
                    <th class="w-3/12">Dạy môn</th>
                    <th class="w-2/12">Email</th>
                    <th class="w-2/12">SDT</th>
                    <th class="w-2/12">Trạng thái</th>
                    <th class="w-1/12"></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="tutor in tutors" :key="tutor.id">
                    <td>{{ tutor.id }}</td>
                    <td><button class="font-bold underline text-blue-400">{{ tutor.name }}</button></td>
                    <td><img :src="tutor.avatar" class="w-24 h-24"></td>
                    <td>
                        <div class="flex flex-wrap gap-1" v-html="displaySubjects(tutor.subjects)"></div>
                    </td>
                    <td class="break-all">{{ tutor.email }}</td>
                    <td>{{ tutor.phone }}</td>
                    <td>
                        <div :class="getStatusStyle(tutor.status)">{{ tutor.status }}</div>
                    </td>
                    <td class="relative">
                        <button class="p-2 bg-slate-200 hover:bg-slate-400 font-bold rounded-full"
                            @click.stop="setSelectId(tutor.id)">
                            <i class="fa fa-ellipsis-h	"></i>
                        </button>
                        <div v-if="selectId == tutor.id"
                            class="absolute right-0 bg-white rounded-lg shadow-lg z-10 w-48 animate-fade-down animate-duration-[400ms] animate-normal font-bold flex flex-col">
                            <!-- Content of your menu -->
                                <button class="hover:bg-slate-200 p-2 rounded-t-lg text-left">
                                    <i class="fa fa-user mr-4"></i>Xem hồ sơ
                                </button>
                                <!-- <li class="hover:bg-slate-200 p-2"></li> -->
                                <button v-if="tutor.status == 'Active'" class="hover:bg-slate-200 p-2 rounded-b-lg text-left text-red-500">
                                    <i class="fa fa-remove mr-4"></i>Đình chỉ
                                </button>
                                <button v-if="tutor.status == 'Fired'" class="hover:bg-slate-200 p-2 rounded-b-lg text-left  text-green-500">
                                    <i class="fa fa fa-check mr-4"></i>Kích hoạt
                                </button>
                        </div>
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
        <generic-popup v-if="isOpenFilterPopup" :closeFunction="toggleFilterPopup" title="Bộ lọc gia sư" :notOverflow="true">
            <tutor-filter-popup :close="toggleFilterPopup" />
        </generic-popup>
    </div>
</template>

<script>
import GenericPopup from '../common/GenericPopup.vue'
import TutorFilterPopup from './TutorFilterPopup.vue'
export default {
  components: { GenericPopup, TutorFilterPopup },
    name: "AdminTutorList",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            selectId: 0,
            isShowPopup: false,
            isOpenFilterPopup : false,
            tutors: [
                {
                    id: 1,
                    name: "Nguyen Van A",
                    email: "abc@gmail.com",
                    phone: "0987654321",
                    avatar: "/src/assets/noavatar.jpg",
                    status: "Active",
                    subjects: [
                        {
                            name: "Toán"
                        },
                        {
                            name: "Tiếng Anh"
                        },
                        {
                            name: "Vật Lý"
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
                    subjects: [
                        {
                            name: "Toán"
                        },
                        {
                            name: "Tiếng Anh"
                        },
                        {
                            name: "Vật Lý"
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
                    subjects: [
                        {
                            name: "Toán"
                        },
                        {
                            name: "Piano, organ"
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
                    subjects: [
                        {
                            name: "Tiếng Nhật"
                        },
                        {
                            name: "Tiếng Anh"
                        },
                    ]
                },
            ],
        }
    },
    methods: {
        handleFilter(){
            this.toggleFilterPopup()
        },
        toggleFilterPopup(){
            this.isOpenFilterPopup = !this.isOpenFilterPopup
        },
        clearSelectId() {
            if (this.isShowPopup) {
                this.selectId = 0
                this.isShowPopup = false
            }
        },
        setSelectId(id) {
            if (id == this.selectId) {
                this.selectId = 0
            } else {
                this.selectId = id
                this.isShowPopup = true
            }
        },
        displaySubjects(subjects) {
            let color = "gray"
            let html = ""
            for (var subject of subjects) {
                switch (subject.name) {
                    case "Toán":
                        color = "border-orange-400"
                        break;
                    case "Tiếng Anh":
                        color = "border-green-400"
                        break;
                    case "Tiếng Nhật":
                        color = "border-pink-400"
                        break;
                }
                var style = `rounded-lg py-2 px-6 border ${color}`
                html += `<span class="${style}">${subject.name}</span>`
            }
            return html
        },
        getStatusStyle(status) {
            let css = "text-center font-bold text-white rounded-lg p-1"
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
    },
    mounted() {
    }
}
</script>

<style scoped>
/* .slide-down-enter-active {
  transition: transform 0.3s ease-out;
}
.slide-down-leave-active {
  transition: transform 0.3s ease-in;
}
.slide-down-enter, .slide-down-leave-to {
  transform: translateY(-50%);
} */
</style>
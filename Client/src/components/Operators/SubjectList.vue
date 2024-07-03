<template>
    <div class="p-4 w-full" @click="setSelectId(0)">
        <div class="flex place-content-between gap-2">
            <button class="py-2 px-4 bg-blue-500 hover:bg-blue-300 text-white font-bold rounded-lg"
                @click="toggleOpenAddPopup">
                <i class="fa fa-plus"></i> Thêm mới môn học
            </button>
            <div>
                <button class="p-2 font-bold text-blue-400 underline" v-if="filterDto.isChanged" @click="resetFilter">
                    Reset bộ lọc
                </button>
                <button class="py-2 px-4 bg-slate-500 hover:bg-slate-300 text-white font-bold rounded-lg"
                    @click="toggleFilterPopup">
                    <i class="fa fa-search	"></i> Filter
                </button>
            </div>

        </div>
        <table id="operator-table">
            <thead>
                <tr>
                    <th class="w-1/12">Id</th>
                    <th class="w-1/12">Tên</th>
                    <th class="w-2/12">Loại</th>
                    <th class="w-3/12">Mô tả</th>
                    <th class="w-2/12">Thêm ngày</th>
                    <th class="w-2/12">Chỉnh sửa ngày</th>
                    <th class="w-2/12">Trạng thái</th>
                    <th class="w-1/12"></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="subject in subjects" :key="subject.id">
                    <td>{{ subject.id }}</td>
                    <td>{{ subject.name }}</td>
                    <td>{{ subject.subjectType }}</td>
                    <td>
                        {{ subject.description }}
                    </td>
                    <td>{{ subject.createAt }}</td>
                    <td>{{ subject.updateAt }}</td>
                    <td>
                        <div :class="getStatusStyle(subject.status)">{{ subject.status }}</div>
                    </td>
                    <td class="relative">
                        <button class="p-2 bg-slate-200 hover:bg-slate-400 font-bold rounded-full"
                            @click.stop="setSelectId(subject.id)">
                            <i class="fa fa-ellipsis-h	"></i>
                        </button>
                        <div v-if="selectId == subject.id"
                            class="absolute right-0 bg-white rounded-lg shadow-lg z-10 w-48 animate-fade-down animate-duration-[400ms] animate-normal font-bold flex flex-col">
                            <!-- Content of your menu -->
                            <button class="hover:bg-slate-200 p-2 rounded-t-lg text-left"
                                @click="handleEdit(subject.id)">
                                <i class="fa fa-edit mr-4"></i>Chỉnh sửa
                            </button>
                            <!-- <li class="hover:bg-slate-200 p-2"></li> -->
                            <button v-if="subject.status == 'Active'"
                                class="hover:bg-slate-200 p-2 rounded-b-lg text-left text-red-500">
                                <i class="fa fa-remove mr-4"></i>Vô hiệu hóa
                            </button>
                            <button v-if="subject.status == 'Inactive'"
                                class="hover:bg-slate-200 p-2 rounded-b-lg text-left  text-green-500">
                                <i class="fa fa fa-check mr-4"></i>Kích hoạt
                            </button>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="flex gap-4 justify-center mt-4" v-if="this.subjects.length > 0">
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
        <generic-popup v-if="isOpenFilterPopup" :closeFunction="toggleFilterPopup" title="Bộ lọc môn học"
            :notOverflow="true">
            <subject-filter-popup :close="toggleFilterPopup" :filterDto="filterDto" :action="handleFilter" />
        </generic-popup>
        <generic-popup v-if="isOpenAddPopup" title="Thêm môn học" :closeFunction="toggleOpenAddPopup">
            <subject-add-edit-popup :close="toggleOpenAddPopup" />
        </generic-popup>
        <generic-popup v-if="isOpenEditPopup" title="Chỉnh sửa môn học" :closeFunction="toggleOpenEditPopup">
            <subject-add-edit-popup :close="toggleOpenEditPopup" :editDto="editDto" />
        </generic-popup>
    </div>
</template>

<script>
import axios from 'axios'
import GenericPopup from '../common/GenericPopup.vue'
import SubjectAddEditPopup from './SubjectAddEditPopup.vue'
import SubjectFilterPopup from './SubjectFilterPopup.vue'
export default {
    components: { GenericPopup, SubjectFilterPopup, SubjectAddEditPopup },
    name: "AdminSubjectList",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            selectId: 0,
            isShowPopup: false,
            isOpenFilterPopup: false,
            isOpenAddPopup : false,
            isOpenEditPopup : false,
            subjects: [
                {
                    id: 1,
                    name: "Toán",
                    subjectType: "Khoa học tự nhiên",
                    description: "Some description go here",
                    createAt: "2000-01-01",
                    updateAt: "2000-01-01",
                    status: "Active"
                },
                {
                    id: 2,
                    name: "Lý",
                    subjectType: "Khoa học tự nhiên",
                    description: "Some description go here",
                    createAt: "2000-01-01",
                    updateAt: "2000-01-01",
                    status: "Active"
                },
                {
                    id: 3,
                    name: "Hóa",
                    subjectType: "Khoa học tự nhiên",
                    description: "Some description go here",
                    createAt: "2000-01-01",
                    updateAt: "2000-01-01",
                    status: "Active"
                },
                {
                    id: 4,
                    name: "Lịch sử",
                    subjectType: "Khoa học xã hội",
                    description: "Some description go here",
                    createAt: "2000-01-01",
                    updateAt: "2000-01-01",
                    status: "Active"
                },
                {
                    id: 5,
                    name: "Địa lý",
                    subjectType: "Khoa học xã hội",
                    description: "Some description go here",
                    createAt: "2000-01-01",
                    updateAt: "2000-01-01",
                    status: "Active"
                },
                {
                    id: 6,
                    name: "Piano",
                    subjectType: "Năng khiếu",
                    description: "Some description go here",
                    createAt: "2000-01-01",
                    updateAt: "2000-01-01",
                    status: "Inactive"
                }
            ],
            filterDto: {
                name: "",
                subjectType: "",
                description: "",
                fromCreateAt: "",
                toCreateAt: "",
                fromUpdateAt: "",
                toUpdateAt: "",
                status: "All",
                isChanged: false
            },
            editDto : {
                name : "",
                subjectType : "",
                description : "",
                status : "Active"
            }
        }
    },
    methods: {
        async fetchSubject(){
            let query = {
                Sorts: {
                    column: "Id",
                    isDesc: true
                },
                Page: 0,
                Limit: 5
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/subject?'+ 
            this.jsonToQueryString(query))
            if (response.data) {
                this.subjects = response.data.items
            }
        },
        resetFilter() {
            this.filterDto = {
                name: "",
                subjectType: "",
                description: "",
                fromCreateAt: "",
                toCreateAt: "",
                fromUpdateAt: "",
                toUpdateAt: "",
                status: "All",
                isChanged: false
            }
        },
        handleEdit(id) {
            const subject = this.subjects.find(o => o.id == id)
            if (subject != null) {
                this.editDto.name = subject.name,
                this.editDto.subjectType = subject.subjectType,
                this.editDto.description = subject.description,
                this.editDto.status = subject.status

                this.toggleOpenEditPopup()
            }
        },
        handleFilter(filterDto, selectedSubjects) {
            console.log(filterDto)
            this.filterDto = JSON.parse(JSON.stringify(filterDto));
            this.filterDto.selectedSubjects = selectedSubjects
        },
        toggleFilterPopup() {
            this.isOpenFilterPopup = !this.isOpenFilterPopup
        },
        toggleOpenAddPopup() {
            this.isOpenAddPopup = !this.isOpenAddPopup
        },
        toggleOpenEditPopup() {
            this.isOpenEditPopup = !this.isOpenEditPopup
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
        getStatusStyle(status) {
            let css = "text-center font-bold text-white rounded-lg p-1"
            switch (status) {
                case "Active":
                    return css + " bg-green-400"
                case "Inactive":
                    return css + " bg-gray-400"
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
        this.fetchSubject()
    }
}
</script>
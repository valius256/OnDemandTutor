<template>
    <div class="p-4 w-full" @click="setSelectId(0)">
        <div class="text-2xl font-bold">
            Quản lý FAQ
        </div>
        <div class="flex place-content-between gap-2 mt-8">
            <button class="py-2 px-4 bg-blue-500 hover:bg-blue-300 text-white font-bold rounded-lg"
                @click="toggleOpenAddPopup">
                <i class="fa fa-plus"></i> Thêm mới FAQ
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
                    <th class="w-2/12">Câu hỏi</th>
                    <th class="w-4/12">Trả lời</th>
                    <th class="w-1/12">Thêm bởi</th>
                    <th class="w-2/12">Thêm ngày</th>
                    <th class="w-2/12">Chỉnh sửa ngày</th>
                    <th class="w-1/12"></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="faq in faqs" :key="faq.id">
                    <td>{{ faq.id }}</td>
                    <td>{{ faq.question }}</td>
                    <td>{{ faq.answer }}</td>
                    <td>
                        {{ faq.createBy?.name }}
                    </td>
                    <td>{{ this.beautifyDatetime(faq.createdDate) }}</td>
                    <td>{{ this.beautifyDatetime(faq.updatedDate) }}</td>
                    <td class="relative">
                        <button class="p-2 bg-slate-200 hover:bg-slate-400 font-bold rounded-full"
                            @click.stop="setSelectId(faq.id)">
                            <i class="fa fa-ellipsis-h	"></i>
                        </button>
                        <div v-if="selectId == faq.id"
                            class="absolute right-0 bg-white rounded-lg shadow-lg z-10 w-48 animate-fade-down animate-duration-[400ms] animate-normal font-bold flex flex-col">
                            <!-- Content of your menu -->
                            <button class="hover:bg-slate-200 p-2 rounded-t-lg text-left" @click="handleEdit(faq.id)">
                                <i class="fa fa-edit mr-4"></i>Chỉnh sửa
                            </button>
                            <!-- <li class="hover:bg-slate-200 p-2"></li> -->
                            <button class="hover:bg-slate-200 p-2 rounded-b-lg text-left text-red-500"
                                @click="handleDeleteFAQ({ confirmation: true, id: faq.id })">
                                <i class="fa fa-remove mr-4"></i>Xóa
                            </button>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="flex gap-4 justify-center mt-4" v-if="this.faqs.length > 0">
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
        <generic-popup v-if="isOpenFilterPopup" :closeFunction="toggleFilterPopup" title="Bộ lọc FAQ"
            :notOverflow="true">
            <FAQFilterPopup :close="toggleFilterPopup" :filterDto="filterDto" :action="handleFilter"
                :operators="operators" />
        </generic-popup>
        <generic-popup v-if="isOpenAddPopup" title="Thêm FAQ" :closeFunction="toggleOpenAddPopup">
            <FAQEditAddPopup :close="toggleOpenAddPopup" :reload="fetchFAQ" />
        </generic-popup>
        <generic-popup v-if="isOpenEditPopup" title="Chỉnh sửa FAQ" :closeFunction="toggleOpenEditPopup">
            <FAQEditAddPopup :close="toggleOpenEditPopup" :editDto="editDto" :reload="fetchFAQ" />
        </generic-popup>
    </div>
</template>

<script>
import GenericPopup from '../../components/common/GenericPopup.vue'
import FAQEditAddPopup from '../../components/Operators/FAQEditAddPopup.vue'
import FAQFilterPopup from '../../components/Operators/FAQFilterPopup.vue'
import axios from 'axios'
export default {
    components: { GenericPopup, FAQEditAddPopup, FAQFilterPopup },
    inject: ['eventBus'],
    name: "FAQManagementPage",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            selectId: 0,
            isShowPopup: false,
            isOpenFilterPopup: false,
            isOpenAddPopup: false,
            isOpenEditPopup: false,
            faqs: [],
            operators: [
                {
                    id: 1,
                    name: "Thomas"
                },
                {
                    id: 2,
                    name: "Arthur"
                },
                {
                    id: 3,
                    name: "John"
                },
            ],
            filterDto: {
                question: "",
                answer: "",
                fromCreateAt: "",
                toCreateAt: "",
                fromUpdateAt: "",
                toUpdateAt: "",
                createdBy: "All",
                isChanged: false
            },
            editDto: {
                id: 0,
                question: "",
                answer: "",
            }
        }
    },
    methods: {
        async fetchOperators() {
            try {
                const response = await axios.get(import.meta.env.VITE_API_URL + '/api/User/all-operators',{
                    headers : {
                        'Authorization' : "Bearer " + localStorage.token
                    }
                })
                if (response.data) {
                    this.operators = response.data.data
                }
            } catch (e) {
                console.log(e)
            }
        },
        async fetchFAQ() {
            let query = {
                "Filter.Question": this.filterDto.question,
                "Filter.Answer": this.filterDto.answer,
                "Filter.CreateFrom": this.filterDto.fromCreateAt,
                "Filter.CreateTo": this.filterDto.toCreateAt,
                "Filter.UpdateFrom": this.filterDto.fromUpdateAt,
                "Filter.UpdateTo": this.filterDto.toUpdateAt,
                Sorts: {
                    column: "Id",
                    isDesc: true
                },
                Page: this.currentPage - 1,
                Limit: this.pageSize
            }
            if (this.filterDto.createdBy != "All") {
                query["Filter.CreateBy"] = this.filterDto.createdBy
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/FAQ/all?' +
                this.jsonToQueryString(query))
            if (response.data) {
                this.faqs = response.data.items
                this.totalPage = Math.ceil(response.data.total / this.pageSize)
            }
        },
        async resetFilter() {
            this.filterDto = {
                question: "",
                answer: "",
                fromCreateAt: "",
                toCreateAt: "",
                fromUpdateAt: "",
                toUpdateAt: "",
                createdBy: "All",
                isChanged: false
            }
            await this.fetchFAQ()
        },
        handleEdit(id) {
            const faq = this.faqs.find(o => o.id == id)
            if (faq != null) {
                this.editDto.id = faq.id
                this.editDto.question = faq.question,
                    this.editDto.answer = faq.answer,

                    this.toggleOpenEditPopup()
            }
        },
        async handleFilter(filterDto) {
            console.log(filterDto)
            this.filterDto = JSON.parse(JSON.stringify(filterDto));
            await this.fetchFAQ()
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
        async handlePageChange() {
            if (this.currentPage > this.totalPage) {
                this.currentPage = this.totalPage
            }
            if (this.currentPage < 1) {
                this.currentPage = 1
            }
            await this.fetchFAQ()
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
        async handleDeleteFAQ(request) {
            if (request.confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn xóa FAQ này không?",
                    method: this.handleDeleteFAQ,
                    params: { confirmation: false, id: request.id }
                })
            } else {
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    await axios.delete(import.meta.env.VITE_API_URL + '/api/FAQ/delete?id=' + request.id, {
                        headers: {
                            "Authorization": "Bearer " + localStorage.token
                        }
                    })
                    this.eventBus.emit("open-result-dialog", {
                        message: "Xóa FAQ thành công",
                        type: "Success"
                    })
                    this.fetchFAQ()
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Có vấn đề xảy ra khi xóa FAQ",
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        },
    },
    mounted() {
        this.fetchFAQ()
        this.fetchOperators()
    }
}
</script>
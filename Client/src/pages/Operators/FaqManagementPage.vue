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
                        {{ faq.createBy.name }}
                    </td>
                    <td>{{ faq.createAt }}</td>
                    <td>{{ faq.updateAt }}</td>
                    <td class="relative">
                        <button class="p-2 bg-slate-200 hover:bg-slate-400 font-bold rounded-full"
                            @click.stop="setSelectId(faq.id)">
                            <i class="fa fa-ellipsis-h	"></i>
                        </button>
                        <div v-if="selectId == faq.id"
                            class="absolute right-0 bg-white rounded-lg shadow-lg z-10 w-48 animate-fade-down animate-duration-[400ms] animate-normal font-bold flex flex-col">
                            <!-- Content of your menu -->
                            <button class="hover:bg-slate-200 p-2 rounded-t-lg text-left"
                                @click="handleEdit(faq.id)">
                                <i class="fa fa-edit mr-4"></i>Chỉnh sửa
                            </button>
                            <!-- <li class="hover:bg-slate-200 p-2"></li> -->
                            <button
                                class="hover:bg-slate-200 p-2 rounded-b-lg text-left text-red-500">
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
            <FAQFilterPopup  :close="toggleFilterPopup" :filterDto="filterDto" :action="handleFilter" :operators="operators" />
        </generic-popup>
        <generic-popup v-if="isOpenAddPopup" title="Thêm FAQ" :closeFunction="toggleOpenAddPopup">
            <FAQEditAddPopup :close="toggleOpenAddPopup" />
        </generic-popup>
        <generic-popup v-if="isOpenEditPopup" title="Chỉnh sửa FAQ" :closeFunction="toggleOpenEditPopup">
            <FAQEditAddPopup :close="toggleOpenEditPopup" :editDto="editDto" />
        </generic-popup>
    </div>
</template>

<script>
import GenericPopup from '../../components/common/GenericPopup.vue'
import FAQEditAddPopup from '../../components/Operators/FAQEditAddPopup.vue'
import FAQFilterPopup from '../../components/Operators/FAQFilterPopup.vue'
export default {
    components: { GenericPopup, FAQEditAddPopup, FAQFilterPopup },
    name: "FAQManagementPage",
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
            faqs: [
                {
                    id : 1,
                    question : "What is love?",
                    answer : "Tình yêu, ái tình hay gọi ngắn là tình là một loạt các cảm xúc, trạng thái tâm lý và thái độ khác nhau dao động từ tình cảm cá nhân đến niềm vui sướng. Tình yêu thường là một cảm xúc thu hút mạnh mẽ và nhu cầu muốn được ràng buộc gắn bó",
                    createAt : "2000-01-01",
                    updateAt : "2024-01-01",
                    createBy : {
                        name : "John"
                    }
                },
                {
                    id : 2,
                    question : "What is love?",
                    answer : "Tình yêu, ái tình hay gọi ngắn là tình là một loạt các cảm xúc, trạng thái tâm lý và thái độ khác nhau dao động từ tình cảm cá nhân đến niềm vui sướng. Tình yêu thường là một cảm xúc thu hút mạnh mẽ và nhu cầu muốn được ràng buộc gắn bó",
                    createAt : "2000-01-01",
                    updateAt : "2024-01-01",
                    createBy : {
                        name : "John"
                    }
                },
                {
                    id : 3,
                    question : "What is love?",
                    answer : "Tình yêu, ái tình hay gọi ngắn là tình là một loạt các cảm xúc, trạng thái tâm lý và thái độ khác nhau dao động từ tình cảm cá nhân đến niềm vui sướng. Tình yêu thường là một cảm xúc thu hút mạnh mẽ và nhu cầu muốn được ràng buộc gắn bó",
                    createAt : "2000-01-01",
                    updateAt : "2024-01-01",
                    createBy : {
                        name : "John"
                    }
                }
            ],
            operators :[
                {
                    id : 1,
                    name : "Thomas"
                },
                {
                    id : 2,
                    name : "Arthur"
                },
                {
                    id : 3,
                    name : "John"
                },
            ],
            filterDto: {
                question: "",
                answer: "",
                fromCreateAt: "",
                toCreateAt: "",
                fromUpdateAt: "",
                toUpdateAt: "",
                createdBy : "All",
                isChanged: false
            },
            editDto : {
                question: "",
                answer: "",
            }
        }
    },
    methods: {
        resetFilter() {
            this.filterDto = {
                question: "",
                answer: "",
                fromCreateAt: "",
                toCreateAt: "",
                fromUpdateAt: "",
                toUpdateAt: "",
                createdBy : "All",
                isChanged: false
            }
        },
        handleEdit(id) {
            const faq = this.faqs.find(o => o.id == id)
            if (faq != null) {
                this.editDto.question = faq.name,
                this.editDto.answer = faq.subjectType,

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
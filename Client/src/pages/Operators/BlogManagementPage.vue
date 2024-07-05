<template>
    <div class="p-4 w-full">
        <div class="text-2xl font-bold">
            Quản lý Blog
        </div>
        <div class="mt-8 flex place-content-between">
            <router-link to="/admin/blogs/editor/0">
                <div class="p-2 text-white bg-green-400 hover:bg-green-200 font-bold rounded-lg">
                    <i class="fa fa-plus mr-4"></i>Thêm blog mới
                </div>
            </router-link>
            <div class="flex gap-2">
                <button class="p-2 font-bold text-blue-400 underline" v-if="filterDto.isChanged" @click="resetFilter">
                    Reset bộ lọc
                </button>
                <button class="py-2 px-4 bg-slate-500 hover:bg-slate-300 text-white font-bold rounded-lg"
                    @click="toggleSortPopup">
                    <i class="fa fa-sort-amount-asc	"></i> Sắp xếp
                </button>

                <button class="py-2 px-4 bg-slate-500 hover:bg-slate-300 text-white font-bold rounded-lg"
                    @click="toggleFilterPopup">
                    <i class="fa fa-search	"></i> Lọc
                </button>
            </div>

        </div>
        <div class="mt-8">
            <div class="p-6 rounded-lg border mt-4" v-for="blog in blogs" :key="blog.id">
                <div class="flex flex-wrap justify-end gap-4">
                    <button class="p-2 text-white bg-blue-400 hover:bg-blue-200 font-bold rounded-lg">
                        <i class="fa fa-eye-slash mr-4"></i>Ẩn blog
                    </button>
                    <router-link :to="`/admin/blogs/editor/${blog.id}`">
                        <div class="p-2 text-white bg-slate-700 hover:bg-slate-500 font-bold rounded-lg">
                            <i class="fa fa-edit mr-4"></i>Chỉnh sửa
                        </div>
                    </router-link>

                </div>
                <div class="flex flex-col lg:flex-row gap-4 mt-4">
                    <img class="w-36 h-36" :src="blog.thumbnail">
                    <div class="w-full">
                        <div class="flex flex-col lg:flex-row lg:place-content-between">
                            <div class="italic">Tạo bởi : {{ blog.createdBy?.name }}</div>
                            <div class="italic">Tạo lúc : {{ this.beautifyDatetime(blog.createAt)  }}</div>
                            <div class="italic">Chỉnh sửa lần cuối : {{ blog.updatedAt }}</div>
                        </div>
                        <button class="font-bold text-xl hover:underline hover:text-purple-600">{{ blog.title
                            }}</button>
                        <div class="max-h-28 overflow-hidden text-ellipsis line-clamp-4">
                            {{ convertHtmlToText(blog.content) }}
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <div class="flex gap-4 justify-center mt-4" v-if="this.blogs.length > 0">
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
        <generic-popup v-if="isOpenFilterPopup" :closeFunction="toggleFilterPopup" title="Bộ lọc Blog">
            <blog-filter-popup :close="toggleFilterPopup" :filterDto="filterDto" :action="handleFilter"
                :operators="operators" />
        </generic-popup>
        <generic-popup v-if="isOpenSortPopup" :closeFunction="toggleSortPopup" title="Sắp xếp Blog">
            <blog-sort-popup :close="toggleSortPopup" :sortDto="sortDto" :action="handleSort" />
        </generic-popup>
    </div>
</template>

<script>
import GenericPopup from '../../components/common/GenericPopup.vue'
import BlogFilterPopup from '../../components/Operators/BlogFilterPopup.vue'
import BlogSortPopup from '../../components/Operators/BlogSortPopup.vue'
import axios from 'axios'

export default {
    components: { GenericPopup, BlogFilterPopup, BlogSortPopup },
    injects : ['eventBus'],
    name: "BlogManagementPage",
    data() {
        return {
            totalPage: 100,
            pageSize: 5,
            currentPage: 1,
            blogs: [],
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
                keyword: "",
                fromCreateAt: "",
                toCreateAt: "",
                fromUpdateAt: "",
                toUpdateAt: "",
                createdBy: "All",
                status: "All",
                isChanged: false
            },
            isOpenFilterPopup: false,
            isOpenSortPopup: false,
            sortDto: {
                isSortAsc: true,
                sortProp: "Id"
            },

        }
    },
    methods: {
        async fetchBlogs() {
            let query = {
                "Filter.Keyword": this.filterDto.keyword,
                "Filter.Status": this.filterDto.status,
                "Filter.FromCreateAt": this.filterDto.fromCreateAt,
                "Filter.ToCreateAt": this.filterDto.toCreateAt,
                "Filter.fromUpdateAt": this.filterDto.fromUpdateAt,
                "Filter.toUpdateAt": this.filterDto.toUpdateAt,
                "Filter.CreateBy.Name": "",
                Sorts: {
                    column: this.sortDto.sortProp,
                    isDesc: !this.sortDto.isSortAsc
                },
                Page: this.currentPage - 1,
                Limit: this.pageSize
            }
            if (this.filterDto.createdBy != "All"){
                query['Filter.CreateBy.Id'] = this.filterDto.createdBy
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/blog?' +
                this.jsonToQueryString(query))
            if (response.data) {
                this.blogs = response.data.items
                this.totalPage = Math.ceil(response.data.total / this.pageSize)
            }
        },
        async handlePageChange() {
            if (this.currentPage > this.totalPage) {
                this.currentPage = this.totalPage
            }
            if (this.currentPage < 1) {
                this.currentPage = 1
            }
            await this.fetchBlogs()
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
        toggleFilterPopup() {
            this.isOpenFilterPopup = !this.isOpenFilterPopup
        },
        toggleSortPopup() {
            this.isOpenSortPopup = !this.isOpenSortPopup
        },
        async resetFilter() {
            this.filterDto = {
                keyword: "",
                fromCreateAt: "",
                toCreateAt: "",
                fromUpdateAt: "",
                toUpdateAt: "",
                createdBy: "All",
                status: "All",
                isChanged: false
            }
            await this.fetchBlogs()
        },
        async handleFilter(filterDto) {
            this.filterDto = JSON.parse(JSON.stringify(filterDto));
            await this.fetchBlogs()
        },
        async handleSort(sortDto) {
            this.sortDto = JSON.parse(JSON.stringify(sortDto));
            await this.fetchBlogs()
            //this.blogs.sort()
        },
        convertHtmlToText(html) {
            const doc = new DOMParser().parseFromString(html, 'text/html');
            return doc.body.textContent || '';
        }
    },
    mounted(){
        this.fetchBlogs()
    }
}
</script>

<style></style>
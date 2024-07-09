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
                <!-- <button class="py-2 px-4 bg-slate-500 hover:bg-slate-300 text-white font-bold rounded-lg"
                    @click="toggleSortPopup">
                    <i class="fa fa-sort-amount-asc	"></i> Sắp xếp
                </button> -->

                <button class="py-2 px-4 bg-slate-500 hover:bg-slate-300 text-white font-bold rounded-lg"
                    @click="toggleFilterPopup">
                    <i class="fa fa-search	"></i> Lọc
                </button>
            </div>

        </div>
        <div class="mt-8">
            <div :class="`p-6 rounded-lg border mt-4 ${blog.isHidden ? 'bg-gray-200' : ''}`" v-for="blog in blogs"
                :key="blog.id">
                <div v-if="blog.isHidden" class="font-bold">Đã ẩn</div>
                <div class="flex flex-wrap justify-end gap-4">
                    <button v-if="blog.isHidden" @click="handleChangeVisibility({confirmation : true, id : blog.id, isHidden : false})" class="p-2 text-white bg-green-400 hover:bg-green-200 font-bold rounded-lg">
                        <i class="fa fa-eye mr-4"></i>Hiện blog
                    </button>
                    <button v-if="blog.isHidden" @click="handleDelete({confirmation : true, id : blog.id})"
                        class="p-2 text-white bg-red-400 hover:bg-red-200 font-bold rounded-lg">
                        <i class="fa fa-trash mr-4"></i>Xóa blog
                    </button>
                    <button v-else class="p-2 text-white bg-blue-400 hover:bg-blue-200 font-bold rounded-lg"
                    @click="handleChangeVisibility({confirmation : true, id : blog.id, isHidden : true})" >
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
                            <div class="italic">Tạo bởi : {{ blog.createBy?.name }}</div>
                            <div class="italic">Tạo lúc : {{ this.beautifyDatetime(blog.createdDate) }}</div>
                            <div class="italic">Chỉnh sửa lần cuối : {{ this.beautifyDatetime(blog.updatedDate) }}</div>
                        </div>
                        <router-link :to="`/admin/blogs/editor/${blog.id}`"
                            class="font-bold text-xl hover:underline hover:text-purple-600">
                            {{ blog.title }}
                        </router-link>
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
    inject: ['eventBus'],
    name: "BlogManagementPage",
    data() {
        return {
            totalPage: 100,
            pageSize: 5,
            currentPage: 1,
            blogs: [],
            operators: [],
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
                "Filter.CreateFrom": this.filterDto.fromCreateAt,
                "Filter.CreateTo": this.filterDto.toCreateAt,
                "Filter.UpdateFrom": this.filterDto.fromUpdateAt,
                "Filter.UpdateTo": this.filterDto.toUpdateAt,
                Sorts: {
                    column: this.sortDto.sortProp,
                    isDesc: !this.sortDto.isSortAsc
                },
                Page: this.currentPage,
                Limit: this.pageSize
            }
            if (this.filterDto.createdBy != "All") {
                query['Filter.CreateBy'] = this.filterDto.createdBy
            }
            if (this.filterDto.status != "All") {
                query['Filter.IsHidden'] = this.filterDto.status
            }
            this.eventBus.emit("open-loading-popup", {
                message: "Vui lòng chờ..."
            })
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            try {
                const response = await axios.get(import.meta.env.VITE_API_URL + '/api/blog?' +
                    this.jsonToQueryString(query))
                if (response.data) {
                    this.blogs = response.data.items
                    this.totalPage = Math.ceil(response.data.total / this.pageSize)
                }
            } catch (e) {
                console.log(e)
            }
            this.eventBus.emit("close-loading-popup")
        },
        async fetchOperators() {
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/User/all?Role=2&Role=3', {
                headers: {
                    "Authorization": "Bearer " + localStorage.token
                }
            })
            if (response.data) {
                this.operators = response.data.data.items
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
        },
        async handleChangeVisibility(option) {
            if (option.confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn " + (option.isHidden ? "ẨN" : "HIỆN") + " Blog này không?",
                    method: this.handleChangeVisibility,
                    params: {confirmation : false, id : option.id, isHidden : option.isHidden }
                })
            } else {
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    const blog = this.blogs.find(b => b.id == option.id)
                    await axios.put(import.meta.env.VITE_API_URL + '/api/Blog/' + blog.id , {
                        id : blog.id,
                        title : blog.title,
                        thumbnail : blog.thumbnail,
                        content : blog.content,
                        isHidden : option.isHidden
                    }, {
                        headers : {
                            'Authorization' : "Bearer " + localStorage.token
                        }
                    })
                    await this.fetchBlogs()
                    this.eventBus.emit("open-result-dialog", {
                        message: "Cập nhật thành công",
                        type: "Success"
                    })
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Đã xảy ra sự cố, vui lòng thử lại sau",
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        },
        async handleDelete(option) {
            if (option.confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn xóa Blog này không?",
                    method: this.handleDelete,
                    params: {confirmation : false, id : option.id }
                })
            } else {
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    await axios.delete(import.meta.env.VITE_API_URL + '/api/Blog/' + option.id , {
                        headers : {
                            'Authorization' : "Bearer " + localStorage.token
                        }
                    })
                    await this.fetchBlogs()
                    this.eventBus.emit("open-result-dialog", {
                        message: "Xóa thành công",
                        type: "Success"
                    })
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Đã xảy ra sự cố, vui lòng thử lại sau",
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        },
    },
    mounted() {
        this.fetchBlogs()
        this.fetchOperators()
    }
}
</script>

<style></style>
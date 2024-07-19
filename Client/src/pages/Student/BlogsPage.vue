<template>
    <div class="">
        <div class="p-4 bg-slate-300">
            <div class="text-4xl font-bold text-center">
                Blogs
            </div>
            <div class="text-xl font-bold text-center">
                Chúng tôi sẽ mang đến cho bạn những thông tin mới nhất và bổ ích nhất
            </div>
            <div class="mt-4 flex justify-center ">
                <input v-model="keyword" placeholder="Tìm kiếm từ khóa..."
                    class="p-2 w-96 bg-gray-50 shadow-md rounded-2xl text-center" @change="fetchBlogs">
            </div>
        </div>


        <div class="flex mt-8 p-4">
            <div class="w-full lg:w-3/4 border-r-2 mr-4">
                <div class="font-bold text-xl ml-4">Khám phá</div>
                <div class="p-4">
                    <div class="p-6 rounded-lg border mt-4 shadow-lg" v-for="blog in blogs" :key="blog.id">
                        <div class="flex flex-col lg:flex-row gap-4 mt-4" v-if="!blog.isHidden">
                            <button @click="$router.push(`/blog-detail/${blog.id}`)"><img class="w-36 h-36" :src="blog.thumbnail"></button>
                            <div class="w-full">
                                <div class="flex flex-col lg:flex-row lg:place-content-between">
                                    <div class="italic">Tạo bởi : {{ blog.createBy?.name }}</div>
                                    <div class="italic">Tạo lúc : {{ this.beautifyDatetime(blog.createdDate) }}</div>
                                    <div class="italic">Chỉnh sửa lần cuối : {{ this.beautifyDatetime(blog.updatedDate)
                                        }}
                                    </div>
                                </div>
                                <router-link :to="`/blog-detail/${blog.id}`"
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
            </div>

            <div class="w-1/4 hidden lg:block">
                <div class="font-bold text-xl">Bài viết mới</div>
                <div class="mt-4 flex flex-col gap-4">
                    <router-link v-for="blog in newBlogs" :key="blog.id" class="font-bold py-2 bg-slate-200 rounded-lg shadow-md hover:bg-slate-50 text-center" :to="`/blog-detail/${blog.id}`">
                        {{ blog.title }}
                    </router-link>
                </div>

            </div>
        </div>
    </div>

</template>

<script>
import axios from 'axios';
import debounce from 'lodash/debounce';
export default {
    name: "FAQPage",
    data() {
        return {
            totalPage: 100,
            pageSize: 5,
            currentPage: 1,
            blogs: [

            ],
            newBlogs: [

            ],
            keyword: "",
        };
    },
    methods: {
        async fetchBlogs() {
            let query = {
                Sorts: {
                    Column: "id",
                    IsDesc: false
                },
                Page : this.currentPage,
                Limit : this.pageSize
            }
            if (this.keyword) {
                query["Filter.Keyword"] = this.keyword
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/Blog/?' +
                this.jsonToQueryString(query))
            if (response.data) {
                this.blogs = response.data.items
                this.totalPage = Math.ceil(response.data.total / this.pageSize)
            }
        },
        async getNewestBlogs() {
            let query = {
                Sorts: {
                    Column: "id",
                    IsDesc: false
                },
                Page: 1,
                Limit: 10
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/Blog/?' +
                this.jsonToQueryString(query))
            if (response.data) {
                this.newBlogs = response.data.items
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
        convertHtmlToText(html) {
            const doc = new DOMParser().parseFromString(html, 'text/html');
            return doc.body.textContent || '';
        },
        debouncedSearch: debounce(async function (event) {
            await this.fetchBlogs();
        }, 50) // Adjust the debounce delay as needed

    },
    mounted() {
        this.fetchBlogs()
        this.getNewestBlogs()
    }
}
</script>

<style></style>
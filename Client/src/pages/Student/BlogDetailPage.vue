<template>
    <div class="p-4 w-full flex" v-if="blog">
        <div class="w-full lg:w-3/4 border-r-2 mr-4">
            <div>
                <div class="text-4xl font-bold">
                    {{ blog.title }}
                </div>
                <div class="italic">{{ this.beautifyDatetime(blog.createdDate) }}</div>

                <div class="mt-8" v-html="blog.content"></div>

                <div class="flex flex-col mt-8">
                    <div class="italic">Tạo bởi : {{ blog.createBy?.name }}</div>
                    <div class="italic">Tạo lúc : {{ this.beautifyDatetime(blog.createdDate) }}</div>
                    <div class="italic">Chỉnh sửa lần cuối : {{ this.beautifyDatetime(blog.updatedDate)
                        }}
                    </div>
                </div>
            </div>

        </div>
        <div class="w-1/4 hidden lg:block">
            <div class="font-bold text-xl">Có thể bạn quan tâm</div>
            <div class="mt-4 flex flex-col gap-2">
                <router-link v-for="blog in newBlogs" :key="blog.id" :to="`/blog-detail/${blog.id}`" @click="refresh(blog.id)"
                    class="font-bold py-2 rounded-lg shadow-md hover:bg-slate-50">
                    <div class="flex flex-col justify-center items-center">
                        <img class="w-48 h-48" :src="blog.thumbnail">
                        {{ blog.title }}
                    </div>
                </router-link>
            </div>

        </div>

    </div>
</template>

<script>
import axios from 'axios';

export default {
    inject: ['eventBus'],
    name: "BlogDetail",
    data() {
        return {
            title: "",
            blogId: 0,
            blog: null,
            newBlogs: [

            ],
        }
    },
    methods: {
        async fetchBlogs(id) {
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            this.eventBus.emit("open-loading-popup", {
                message: "Vui lòng chờ..."
            })
            try {
                const response = await axios.get(import.meta.env.VITE_API_URL + '/api/blog/' + id)
                if (response.data) {
                    this.blog = response.data
                    this.content = this.blog.content
                    this.title = this.blog.title
                    this.newImage = this.blog.thumbnail
                }
            } catch (e) {
                console.log(e)
            }

            this.eventBus.emit("close-loading-popup")
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
                this.newBlogs = this.shuffleArray(response.data.items)
            }
        },
        getMillisecondsFromMinDate(date) {
            // The minimum date value is January 1, 1970, 00:00:00 UTC
            const minDate = new Date(0);
            return date.getTime() - minDate.getTime();
        },
        shuffleArray(array) {
            for (let i = array.length - 1; i > 0; i--) {
                const j = Math.floor(Math.random() * (i + 1));
                [array[i], array[j]] = [array[j], array[i]];
            }
            return array.slice(0,4);
        },
        async refresh(id) {
            this.getNewestBlogs()
            
            if (id != 0) {
                this.fetchBlogs(id)
            }
        }
    },
    mounted() {
        this.blogId = this.$route.params.id
        this.refresh(this.blogId)
    }
}
</script>

<style></style>
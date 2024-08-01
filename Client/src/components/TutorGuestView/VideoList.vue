<template>
    <div class="bg-slate-100 p-8">
        <div class="mt-4 mb-4 rounded-xl shadow-lg bg-white" v-for="video in videos" :key="video.id">
            <div class="w-full py-8 px-16">
                <div class="flex gap-4">
                    <img class="w-16 h-16 rounded-full" :src="video.tutor.avatarImageUrl" />
                    <div>
                        <div class="font-bold">{{ (video.tutor.firstName ?? "") + " " + (video.tutor.lastName ?? "")}}</div>
                        <div class="italic">{{ this.beautifyDatetime(video.createdDate) }}</div>
                    </div>
                </div>
                <div>{{ video.description }}</div>
                <video class="w-full mt-4" controls>
                    <source :src="video.videoUrl" type="video/mp4">
                    Your browser does not support the video tag.
                </video>
            </div>
            <hr>
        </div>
        <div class="flex gap-4 justify-center mt-4 mb-4" v-if="this.videos.length > 0">
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
import axios from 'axios';
export default {
    props : ["tutor","viewingId"],
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            isOpenUploadPopup : false,
            videos: [

            ]
        }
    },
    methods: {
        toggleOpenUploadPopup(){
            this.isOpenUploadPopup = !this.isOpenUploadPopup
        },
        async fetchVideos() {
            try {
                const response = await axios.get(
                    import.meta.env.VITE_API_URL + "/api/TutorVideo",
                    {
                        params: {
                            "Filter.TutorId" : this.tutor.id,
                            Page: this.currentPage,
                            Limit: this.pageSize
                        },
                        headers: {
                            Authorization: "Bearer " + localStorage.token,
                        },
                    }
                );
                this.videos = response.data.items;
                this.totalPage = Math.ceil(response.data.total / this.pageSize);
            } catch (error) {
                console.error("Error fetching tutor subjects:", error);
            }
        },
        async handlePageChange() {
            if (this.currentPage > this.totalPage) {
                this.currentPage = this.totalPage
            }
            if (this.currentPage < 1) {
                this.currentPage = 1
            }
            await this.fetchVideos()
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
        this.fetchVideos()
    }
}
</script>

<style></style>
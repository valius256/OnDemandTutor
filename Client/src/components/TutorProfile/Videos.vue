<template>
    <div>
        <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200">
            Video của bạn
        </div>
        <div v-if="currentUser.tutorStatus == 3">

            <div class="flex justify-center">
                <button @click="toggleOpenUploadPopup"
                    class="font-bold bg-blue-400 text-white rounded-lg hover:bg-blue-200 px-12 py-2">
                    Đăng tải video mới
                </button>
            </div>
            <hr class="mt-4">
            <div class="mt-4" v-for="video in videos" :key="video.id">
                <div class="flex justify-end gap-4 mr-4">
                    <button class="bg-blue-500 hover:bg-blue-200 py-2 px-12 rounded-lg text-white font-bold">
                        <i class="fa fa-edit"></i>
                    </button>
                    <button class="bg-red-500 hover:bg-red-200 py-2 px-12 rounded-lg text-white font-bold">
                        <i class="fa fa-trash"></i>
                    </button>
                </div>
                <div class="w-full pt-2 pb-8 pl-4">
                    <div class="flex gap-4">
                        <img class="w-12 h-12 rounded-full" :src="video.tutor.avatarImageUrl" />
                        <div>
                            <div class="font-bold">{{ (video.tutor.firstName ?? "") + " " + (video.tutor.lastName ??
            "") }}</div>
                            <div class="italic">{{ this.beautifyDatetime(video.createdDate) }}</div>
                        </div>
                    </div>
                    <div v-html="video.description.replace(/\n/g, '<br />')"></div>
                    <video class="w-5/6 mt-4" controls>
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
        <div v-else class="p-8">
            <div class="p-8 bg-red-200 rounded-lg text-center font-bold">
                Bạn cần xác thực tài khoản để sử dụng tính năng này
            </div>
        </div>
        <generic-popup v-if="isOpenUploadPopup" title="Tải video mới lên" :closeFunction="toggleOpenUploadPopup">
            <video-uploader :tutorId="currentUser.id" :close="toggleOpenUploadPopup"
                :action="fetchVideos"></video-uploader>
        </generic-popup>
    </div>
</template>

<script>
import axios from 'axios';
import VideoUploader from './VideoUploader.vue';
import GenericPopup from '../common/GenericPopup.vue';
export default {
    components: { VideoUploader, GenericPopup },
    props: ["currentUser"],
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            isOpenUploadPopup: false,
            videos: [

            ]
        }
    },
    methods: {
        toggleOpenUploadPopup() {
            this.isOpenUploadPopup = !this.isOpenUploadPopup
        },
        async fetchVideos() {
            try {
                const response = await axios.get(
                    import.meta.env.VITE_API_URL + "/api/TutorVideo",
                    {
                        params: {
                            "Filter.TutorId": this.currentUser.id,
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
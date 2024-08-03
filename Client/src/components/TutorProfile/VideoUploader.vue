<template>
    <div class="p-4 bg-white rounded-b-lg w-full max-h-screen overflow-y-auto">
        <input type="file" @change="handleFileUpload" accept="video/*" />
        <video class="mt-4" v-if="videoURL" :src="videoURL" controls width="480" height="280"></video>
        <div class="mt-2">
            <div class="font-bold">Nhập tiêu đề</div>
            <input class="w-full rounded-lg border p-3" placeholder="Nhập tiêu đề" v-model="title">
        </div>
        <div class="mt-2">
            <div class="font-bold">Mô tả video</div>
            <textarea class="w-full rounded-lg border p-3" placeholder="Nhập mô tả" v-model="description"></textarea>
        </div>
        <div class="mt-4 flex justify-center gap-4">
            <button @click="handleUpload(true)" class="px-4 py-2 text-white font-bold bg-blue-400 hover:bg-blue-200 rounded-lg">Đăng</button>
            <button @click="close" class="px-4 py-2 text-white font-bold bg-red-400 hover:bg-red-200 rounded-lg">Hủy
                bỏ</button>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
export default {
    props: ["tutorId", "close", "action"],
    inject: ['eventBus'],
    data() {
        return {
            videoURL: null,
            videoFile: null,
            title: "",
            description: "",
        };
    },
    methods: {
        handleFileUpload(event) {
            const file = event.target.files[0];
            if (file && file.type.startsWith('video/')) {
                this.videoURL = URL.createObjectURL(file);
                this.videoFile = file;
            } else {
                alert('Please select a valid video file.');
            }
        },
        async handleUpload(confirmation) {
            console.log(this.description.split('\n').join('\\n'))
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn đăng tải video này?",
                    method: this.handleUpload,
                    params: false
                })
            } else {
                this.eventBus.emit("open-loading-popup", {
                    message: "Xin chờ 1 lát... Đừng đóng trang này nhé!"
                })
                try {
                    const formData = new FormData();
                    formData.append("file", this.videoFile);
                    const uploadRes = await axios.post(import.meta.env.VITE_API_URL + '/api/Upload/upload-video', formData , {
                        headers: {
                            "Authorization": "Bearer " + localStorage.token,
                            "Content-Type": "multipart/form-data",
                        }
                    })
                    const url = uploadRes.data
                    await axios.post(import.meta.env.VITE_API_URL + '/api/TutorVideo', {
                        videoUrl : url,
                        description : this.description,
                        title : this.title
                    },{
                        headers: {
                            "Authorization": "Bearer " + localStorage.token,
                        }
                    })            
                    this.eventBus.emit("open-result-dialog", {
                        message: "Đăng tải thành công",
                        type: "Success"
                    })
                    this.action()
                    this.close()
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Đã có sự cố xảy ra. Vui lòng thử lại sau",
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        }
    },
};
</script>

<style scoped></style>
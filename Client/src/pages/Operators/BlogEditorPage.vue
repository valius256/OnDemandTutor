<template>
    <div class="p-4 w-full">
        <div class="text-2xl font-bold">
            Chỉnh sửa Blog
        </div>
        <div class="flex gap-4 mt-4">
            <span class="p-2 w-32 font-bold">Tiêu đề</span>
            <input v-model="title" class="p-2 rounded-lg border border-1 w-full" />
        </div>
        <div class="flex gap-4 mt-4">
            <span class="p-2 font-bold">Thêm ảnh thumbnail cho Blog</span>
            <div class="p-4 border border-1 rounded-lg">
                <input type="file" accept="image/*" @change="previewFiles($event)">
                <img v-if="newImage" class="mt-4 w-48 h-48" alt="thumbnail" :src="newImage" />
            </div>
        </div>
        <Editor v-model="content" class="mt-8" editorStyle="height: 390px" />
        <div class="flex flex-col lg:flex-row justify-center mt-8 gap-4">
            <button @click="handleSave({confirmation : false, status : 0})"
                class="rounded-lg bg-blue-400 hover:bg-blue-200 text-white font-bold py-2 px-12">
                Hoàn thành
            </button>
            <button @click="handleSave({confirmation : false, status : 1})" 
                class="rounded-lg bg-gray-400 hover:bg-gray-200 text-white font-bold py-2 px-12">
                Lưu dưới dạng Blog ẩn
            </button>
            <button @click="$router.go(-1)"
                class="rounded-lg bg-red-400 hover:bg-red-200 text-white font-bold py-2 px-12">
                Hủy bỏ
            </button>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import Editor from 'primevue/editor';

export default {
    inject: ['eventBus'],
    components: { Editor },
    name: "BlogEditor",
    data() {
        return {
            content: "Write something here...",
            title: "",
            blogId: 0,
            blog: null,
            newImage: "",
        }
    },
    methods: {
        async fetchBlogs(id) {
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            this.eventBus.emit("open-loading-popup", {
                message: "Vui lòng chờ..."
            })
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/blog/' + id)
            if (response.data) {
                this.blog = response.data
                this.content = this.blog.content
                this.title = this.blog.title
                this.newImage = this.blog.thumbnail
            }
            this.eventBus.emit("close-loading-popup")
        },
        async handleSave(saveOption) {
            if (saveOption.confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn đăng bài Blog này?",
                    method: this.handleSave,
                    params: {confirmation : false, status : saveOption.status}
                })
            } else {
                const request = {
                    id: this.blogId,
                    title: this.title,
                    content: this.content,
                    createById: 1,
                    updateById: 1,
                    createAt: "2024-07-05",
                    updateAt: "2024-07-05",
                    status : saveOption.status
                }
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    if (this.blogId == 0) {
                        await axios.post(import.meta.env.VITE_API_URL + '/api/Blog', request, {
                            headers : {
                                "Authorization" : "bearer " + localStorage.token
                            }
                        })
                        this.eventBus.emit("open-result-dialog", {
                            message: "Tạo Blog thành công",
                            type: "Success"
                        })
                    } else {
                        await axios.put(import.meta.env.VITE_API_URL + '/api/Blog/' + this.blogId, request, {
                            headers : {
                                "Authorization" : "bearer " + localStorage.token
                            }
                        })
                        this.eventBus.emit("open-result-dialog", {
                            message: "Cập nhật Blog thành công",
                            type: "Success"
                        })
                    }
                    this.$router.push("/admin/blogs/manage")
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Đã xảy ra sự cố. Vui lòng thử lại sau",
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        },
        previewFiles(event) {
            const file = event.target.files[0];

            const theReader = new FileReader();
            // Nhớ sử dụng async/await để chờ khi đã convert thành công image sang base64 thì mới bắt đầu gán cho biến newImage
            // đây là 1 kinh nghiệm của mình khi upload multiple ảnh
            theReader.onloadend = async () => {
                this.newImage = await theReader.result;
            };
            theReader.readAsDataURL(file);
        }

    },
    mounted() {
        this.blogId = this.$route.params.id
        if (this.blogId != 0) {
            this.fetchBlogs(this.blogId)
        }
    }
}
</script>

<style></style>
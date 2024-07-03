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
            <button @click="handleSave"
                class="rounded-lg bg-blue-400 hover:bg-blue-200 text-white font-bold py-2 px-12">
                Hoàn thành
            </button>
            <button class="rounded-lg bg-gray-400 hover:bg-gray-200 text-white font-bold py-2 px-12">
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
import Editor from 'primevue/editor';

export default {
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
        handleSave() {
            //console.log(this.content)
        },
        presetEdit() {
            this.blog = {
                id: 1,
                title: "Hello World",
                thumbnail: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSBbf9L4SlvzAvmgPiQmxSO1JaU6oQ92xsDgw&s",
                content: "<h1>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore</h1>. <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore/ Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore",
                createdAt: "2020-01-01 14:02:59",
                createdBy: {
                    name: "Thomas"
                },
                updatedAt: "2020-01-01 19:00:55",
            }
            this.content = this.blog.content
            this.title = this.blog.title
            this.newImage = this.blog.thumbnail
            console.log(this.content)
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
            this.presetEdit()
        }
    }
}
</script>

<style></style>
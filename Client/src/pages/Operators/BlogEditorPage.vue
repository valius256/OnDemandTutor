<template>
    <div class="p-4 w-full">
        <div class="text-2xl font-bold">
            Chỉnh sửa Blog
        </div>
        <Editor v-model="content" class="mt-8"/>
        <div class="flex justify-center mt-8 gap-4">
            <button @click="handleSave" class="rounded-lg bg-blue-400 hover:bg-blue-200 text-white font-bold py-2 px-12">
                Hoàn thành
            </button>
            <button class="rounded-lg bg-gray-400 hover:bg-gray-200 text-white font-bold py-2 px-12">
                Lưu dưới dạng Blog ẩn
            </button>
            <button @click="$router.go(-1)" class="rounded-lg bg-red-400 hover:bg-red-200 text-white font-bold py-2 px-12">
                Hủy bỏ
            </button>
        </div>
    </div>
</template>

<script>
import Editor from 'primevue/editor';

export default {
    components : {Editor},
    name: "BlogEditor",
    data(){
        return {
            content : "",
            blogId : 0
        }
    },
    methods :{
        handleSave(){
        }
    },
    mounted(){
        this.blogId = this.$route.params.id
    }
}
</script>

<style></style>
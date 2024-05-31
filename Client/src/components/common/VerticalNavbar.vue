<template>
    <div>
        <div class="h-screen bg-blue-950 text-white">
            <div class="text-3xl font-bold py-5 px-5">
                <span>On</span>
                <span class="text-blue-600">Demand</span>
                <span>Tutor</span>
            </div>
            <div class="mt-8">
                <div class="flex flex-col font-bold text-white">
                    <router-link to="/admin/accounts" class="px-2 py-4 hover:bg-blue-800">Quản lý tài khoản</router-link>
                    <div class="flex flex-col sub-nav" v-if="$route.path.startsWith('/admin/accounts')">
                        <router-link to="/admin/accounts/students" class="px-8 py-4 hover:bg-blue-800">Học sinh</router-link>
                        <router-link to="/admin/accounts/tutors" class="px-8 py-4 hover:bg-blue-800">Gia sư</router-link>
                    </div>
                    <router-link to="/admin/subjects" class="px-2 py-4 hover:bg-blue-800">Quản lý môn học</router-link>
                    <router-link to="/admin/classes" class="px-2 py-4 hover:bg-blue-800">Quản lý lớp học</router-link>
                    <div class="flex flex-col sub-nav" v-if="$route.path.startsWith('/admin/classes')">
                        <router-link to="/admin/classes/list" class="px-8 py-4 hover:bg-blue-800">Danh sách lớp</router-link>
                        <router-link to="/admin/classes/requests" class="px-8 py-4 hover:bg-blue-800">Yêu cầu nhận lớp</router-link>
                    </div>
                    <router-link to="/admin/blogs" class="px-2 py-4 hover:bg-blue-800">Quản lý Blog</router-link>
                    <router-link to="/admin/faqs" class="px-2 py-4 hover:bg-blue-800">Quản lý FAQ</router-link>
                    <router-link to="/admin/consultation" class="px-2 py-4 hover:bg-blue-800">Yêu cầu tư vấn</router-link>
                    
                </div>
            </div>
            <div class="flex flex-col ">
                <button class="px-2 py-4 font-bold text-white text-left hover:bg-blue-800">Đăng xuất</button>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: "VerticalNavbar"
}
</script>

<style scoped>
.router-link-active {
    background: #0a0030;
}
.sub-nav .router-link-active {
    background: #0c0038;
}
</style>
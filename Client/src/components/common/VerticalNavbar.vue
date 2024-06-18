<template>
    <div>
        <div class="h-screen bg-blue-950 text-white overflow-y-auto">
            <div class="text-3xl font-bold py-5 px-5">
                <span>On</span>
                <span class="text-blue-600">Demand</span>
                <span>Tutor</span>
            </div>
            <div class="mt-8">
                <div class="flex flex-col font-bold text-white">
                    <router-link to="/admin/accounts" class="px-2 py-4 hover:bg-blue-800 ">
                        <i class="fa fa-users text-2xl mr-2"></i>Quản lý tài khoản
                    </router-link>
                    <div class="flex flex-col sub-nav" v-if="$route.path.startsWith('/admin/accounts')">
                        <router-link to="/admin/accounts/students" class="px-8 py-4 hover:bg-blue-800">
                            <i class="fa fa-user text-2xl mr-2"></i>  Học sinh
                        </router-link>
                        <router-link to="/admin/accounts/tutors" class="px-8 py-4 hover:bg-blue-800">
                            <i class="fa fa-graduation-cap text-2xl mr-2"></i> Gia sư
                        </router-link>
                        <router-link to="/admin/accounts/operators" class="px-8 py-4 hover:bg-blue-800">
                            <i class="fa fa-user-o text-2xl mr-2"></i> Vận hành
                        </router-link>
                    </div>
                    <router-link to="/admin/subjects" class="px-2 py-4 hover:bg-blue-800">
                        <i class="fa fa-bookmark text-2xl mr-2"></i>Quản lý môn học
                    </router-link>
                    <!-- <router-link to="/admin/classes" class="px-2 py-4 hover:bg-blue-800">
                        <i class="fa fa-calendar text-2xl mr-2"></i>Quản lý lớp học
                    </router-link> -->
                    <router-link to="/admin/blogs" class="px-2 py-4 hover:bg-blue-800">
                        <i class="fa fa-pencil-square-o text-2xl mr-2"></i>Quản lý Blog
                    </router-link>
                    <router-link to="/admin/faqs" class="px-2 py-4 hover:bg-blue-800">
                        <i class="fa fa-question-circle text-2xl mr-2"></i>Quản lý FAQ
                    </router-link>
                    <router-link to="/admin/consultation" class="px-2 py-4 hover:bg-blue-800">
                        <i class="fa fa-comment-o text-2xl mr-2"></i>Yêu cầu tư vấn
                    </router-link>
                    
                </div>
            </div>
            <div class="flex flex-col ">
                <button class="px-2 py-4 font-bold text-white text-left hover:bg-blue-800">
                    <i class="fa fa-sign-out text-2xl mr-2"></i>Đăng xuất
                </button>
            </div>
        </div>
    </div>
</template>

<script>
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

export default {
    name: "VerticalNavbar",
    components : {FontAwesomeIcon}
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
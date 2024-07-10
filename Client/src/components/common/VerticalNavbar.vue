<template>
    <div>
        <div class="relative">
            <div class="h-screen bg-blue-950 text-white overflow-y-auto change-width"
            :class="{'w-[18rem]' : !isCollapse, 'w-16' : isCollapse}">
                <logo/>
                <div class="mt-8">
                    <div class="flex flex-col font-bold text-white">
                        <router-link to="/admin/transactions" class="px-2 py-4 hover:bg-blue-800 ">
                            <i class="fa fa-dollar text-2xl mr-2"></i>
                            <span v-if="!isCollapse">Quản lý giao dịch</span>
                        </router-link>
                        <div class="flex flex-col sub-nav" v-if="$route.path.startsWith('/admin/transactions')">
                            <router-link to="/admin/transactions/statistic" class="py-4 bg-[#1c2e5f] hover:bg-blue-800" :class="{'px-2' : isCollapse, 'px-8' : !isCollapse}">
                                <i class="fa fa-signal text-2xl mr-2"></i>
                                <span  v-if="!isCollapse" >Thống kê</span>
                            </router-link>
                            <router-link to="/admin/transactions/withdraw" class="py-4 bg-[#1c2e5f] hover:bg-blue-800" :class="{'px-2' : isCollapse, 'px-8' : !isCollapse}">
                                <i class="fa fa-money text-2xl mr-2"></i>
                                <span  v-if="!isCollapse" >Yêu cầu rút tiền</span>
                            </router-link>
                        </div>
                        <router-link to="/admin/accounts" class="px-2 py-4 hover:bg-blue-800 ">
                            <i class="fa fa-users text-2xl mr-2"></i>
                            <span v-if="!isCollapse">Quản lý tài khoản</span>
                        </router-link>
                        <div class="flex flex-col sub-nav" v-if="$route.path.startsWith('/admin/accounts')">
                            <router-link to="/admin/accounts/students" class="py-4 bg-[#1c2e5f] hover:bg-blue-800" :class="{'px-2' : isCollapse, 'px-8' : !isCollapse}">
                                <i class="fa fa-user text-2xl mr-2"></i>
                                <span  v-if="!isCollapse" >Học sinh</span>
                            </router-link>
                            <router-link to="/admin/accounts/tutors" class="py-4 bg-[#1c2e5f] hover:bg-blue-800" :class="{'px-2' : isCollapse, 'px-8' : !isCollapse}">
                                <i class="fa fa-graduation-cap text-2xl mr-2"></i>
                                <span  v-if="!isCollapse" >Gia sư</span>
                            </router-link>
                            <router-link to="/admin/accounts/operators" class=" py-4 bg-[#1c2e5f] hover:bg-blue-800" :class="{'px-2' : isCollapse, 'px-8' : !isCollapse}">
                                <i class="fa fa-gear text-2xl mr-2"></i>
                                <span v-if="!isCollapse" >Vận hành</span>
                            </router-link>
                        </div>
                        <router-link to="/admin/subjects" class="px-2 py-4 hover:bg-blue-800">
                            <i class="fa fa-bookmark text-2xl mr-2"></i>
                            <span v-if="!isCollapse">Quản lý môn học</span>
                        </router-link>
                        <router-link to="/admin/blogs" class="px-2 py-4 hover:bg-blue-800">
                            <i class="fa fa-pencil-square-o text-2xl mr-2"></i>
                            <span v-if="!isCollapse">Quản lý Blog</span>
                        </router-link>
                        <router-link to="/admin/faqs" class="px-2 py-4 hover:bg-blue-800">
                            <i class="fa fa-question-circle text-2xl mr-2"></i>
                            <span v-if="!isCollapse">Quản lý FAQ</span>
                        </router-link>
                        <router-link to="/admin/consultation" class="px-2 py-4 hover:bg-blue-800">
                            <i class="fa fa-comment-o text-2xl mr-2"></i>
                            <span v-if="!isCollapse">Yêu cầu tư vấn</span>
                        </router-link>

                    </div>
                </div>
                <div class="flex flex-col ">
                    <button class="px-2 py-4 font-bold text-white text-left hover:bg-blue-800" @click="handleLogout">
                        <i class="fa fa-sign-out text-2xl mr-2"></i>
                        <span v-if="!isCollapse">Đăng xuất</span>
                    </button>
                </div>
            </div>
            <button class="absolute bg-gray-400 z-10 top-1/2 -right-4 w-4 h-32 rounded-r-xl" @click="toggleCollapse">
                <i v-if="!isCollapse" class="fa fa-mail-reply"></i>
                <i v-if="isCollapse" class="fa fa-mail-forward"></i>
            </button>
        </div>
    </div>
</template>

<script>
import Logo from './Logo.vue'
export default {
  components: { Logo },
    name: "VerticalNavbar",
    inject : ['eventBus'],
    data() {
        return {
            isCollapse: false
        }
    },
    methods: {
        toggleCollapse() {
            this.isCollapse = !this.isCollapse
        },
        handleLogout(){
            this.eventBus.emit("logout")
        }
    },
}
</script>

<style scoped>
.router-link-active {
    background: #0a0030;
}

.sub-nav .router-link-active {
    background: #0c0038;
}

.change-width {
    transition: width 0.3s ease;
}
</style>
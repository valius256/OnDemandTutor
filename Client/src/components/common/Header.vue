<template>
    <div class="shadow-md flex place-content-between bg-blue-50 relative">
        <div class="text-3xl font-bold p-3 ">
            <span>On</span>
            <span class="text-blue-600">Demand</span>
            <span>Tutor</span>
        </div>
        <Navbar />
        <div v-if="responsive" @click="toggleResponsive"
            class="absolute left-0 top-12 bg-blue-50 rounded-b-lg shadow-lg z-[100] w-full animate-fade-down animate-duration-[400ms] animate-normal font-bold flex flex-col lg:hidden">
            <!-- Content of your menu -->
            <router-link class="hover:bg-blue-200 px-4 py-5 text-left" to="/">
                Trang chủ
            </router-link>
            <router-link class="hover:bg-blue-200 px-4 py-5 text-left" to="/tutors">
                Danh sách gia sư
            </router-link>
            <router-link class="hover:bg-blue-200 px-4 py-5 text-left" to="/classes">
                Danh sách lớp
            </router-link>
            <router-link class="hover:bg-blue-200 px-4 py-5 text-left" to="/blogs">
                Blogs
            </router-link>
            <router-link class="hover:bg-blue-200 px-4 py-5 text-left" to="/faqs">
                FAQ
            </router-link>
            <router-link class="hover:bg-blue-200 px-4 py-5 text-left" to="/student/profile">
                Hồ sơ cá nhân
            </router-link>
            <router-link class="hover:bg-blue-200 px-4 py-5 text-left" to="/login">
                Đăng nhập
            </router-link>
        </div>
    </div>
</template>

<script>
import Navbar from './Navbar.vue';

export default {
    name: "Header",
    inject: ['eventBus'],
    components: { Navbar },
    data() {
        return {
            responsive: false
        }
    },
    methods: {
        async getUser() {
            const userPromise = new Promise((resolve) => {
                this.eventBus.emit("get-user", resolve);
            });
            const user = await userPromise;
            this.user = user;
        },
        toggleResponsive() {
            this.responsive = !this.responsive
        }
    },
    mounted() {
        this.getUser()

        this.eventBus.on("header-toggle-responsive", () => {
            this.toggleResponsive()
        })
    }
}
</script>

<style></style>
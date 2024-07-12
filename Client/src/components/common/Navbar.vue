<template>
  <div class="relative">
    <div class="hidden lg:flex gap-4 h-full">
      <router-link class="flex justify-center items-center px-4 hover:bg-slate-300" to="/">Trang chủ</router-link>
      <router-link class="flex justify-center items-center px-4 hover:bg-slate-300" to="/tutors">Danh sách gia
        sư</router-link>
      <router-link class="flex justify-center items-center px-4 hover:bg-slate-300" to="/classes">Danh sách
        lớp</router-link>

      <router-link class="flex justify-center items-center px-4 hover:bg-slate-300" to="/blogs">Blogs</router-link>
      <router-link class="flex justify-center items-center px-4 hover:bg-slate-300" to="/faqs">FAQs</router-link>
      <bell-notification v-if="user" />
      <router-link v-if="user && user.role == 0"
        class="flex justify-center items-center px-4 hover:bg-slate-300 font-bold" to="/student">
        <div class="flex gap-4 items-center">
          <div>{{ user?.firstName }}</div>
          <img class="rounded-full w-8 h-8" :src="user.avatarImageUrl" />
        </div>
      </router-link>
      <router-link v-if="user && user.role == 1"
        class="flex justify-center items-center px-4 hover:bg-slate-300 font-bold" to="/tutor">
        <div class="flex gap-4 items-center">
          <div>{{ user?.firstName }}</div>
          <img class="rounded-full w-8 h-8" :src="user.avatarImageUrl" />
        </div>
      </router-link>
      <div class="flex gap-4 items-center mr-4" to="/login" v-if="!user">
        <button class="rounded-md shadow-md px-2 py-2 font-bold bg-white" @click='$router.push("/login")'>
          Đăng nhập
        </button>
        <button class="rounded-md shadow-md px-2 py-2 font-bold text-white bg-blue-400"
          @click='$router.push("/register")'>
          Đăng ký
        </button>
      </div>
      <!-- <div class="flex gap-4 items-center mr-4" to="/login" v-else>
        <button class="rounded-md shadow-md px-2 py-2 font-bold bg-white" @click="handleLogout">
          Đăng xuất
        </button>
      </div> -->
    </div>
    <div class="flex items-center lg:hidden h-full mr-2 gap-4">
      <bell-notification />
      <button class="shadow-md rounded-md py-2 px-4 bg-white text-2xl font-bold" @click="toggleResponsive">
        <i class="fa fa-reorder	"></i>
      </button>
    </div>

    
  </div>
</template>

<script>
import BellNotification from './BellNotification.vue';

export default {
  components: { BellNotification },
  name: "NavBar",
  inject: ["eventBus"],
  data() {
    return {
      user: null,
    };
  },
  methods: {
    async refresh(){
      this.user = await this.getUserFromToken()
    },
    toggleResponsive(){
      this.eventBus.emit("header-toggle-responsive")
    },
    handleLogout(){
      this.eventBus.emit("logout")
    }
  },
  mounted() {
    this.refresh();

    this.eventBus.on("update-navbar", async () => {
      await this.refresh()
    })
  },
};
</script>

<style scoped>
.router-link-active {
  background: #232369;
  color: white;
}
</style>

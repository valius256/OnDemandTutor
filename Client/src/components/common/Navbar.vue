<template>
  <div class="flex gap-4">
    <router-link
      class="flex justify-center items-center px-4 hover:bg-slate-300"
      to="/"
      >Trang chủ</router-link
    >
    <router-link
      class="flex justify-center items-center px-4 hover:bg-slate-300"
      to="/tutors"
      >Danh sách gia sư</router-link
    >
    <router-link
      class="flex justify-center items-center px-4 hover:bg-slate-300"
      to="/classes"
      >Danh sách lớp</router-link
    >
    <router-link
      class="flex justify-center items-center px-4 hover:bg-slate-300"
      to="/login"
      >Đăng nhập
    </router-link>
    <router-link
      class="flex justify-center items-center px-4 hover:bg-slate-300"
      to="/blogs"
      >Blogs</router-link
    >
    <router-link
      class="flex justify-center items-center px-4 hover:bg-slate-300"
      to="/faqs"
      >FAQs</router-link
    >
    <router-link
      v-if="user && user.role=='Student'"
      class="flex justify-center items-center px-4 hover:bg-slate-300 font-bold"
      to="/student"
    >
      <div class="flex gap-4 items-center">
        <div>{{ user?.name }}</div>
        <img class="rounded-full w-8 h-8" :src="user.avatar" />
      </div>
    </router-link>
    <router-link
      v-if="user && user.role=='Tutor'"
      class="flex justify-center items-center px-4 hover:bg-slate-300 font-bold"
      to="/tutor"
    >
      <div class="flex gap-4 items-center">
        <div>{{ user?.name }}</div>
        <img class="rounded-full w-8 h-8" :src="user.avatar" />
      </div>
    </router-link>
  </div>
</template>

<script>
export default {
  name: "NavBar",
  inject: ["eventBus"],
  data() {
    return {
      user: null,
    };
  },
  methods: {
    async getUser() {
      const userPromise = new Promise((resolve) => {
        this.eventBus.emit("get-user", resolve);
      });
      const user = await userPromise;
      this.user = user;
    },
  },
  mounted() {
    this.getUser();
  },
};
</script>

<style scoped>
.router-link-active {
  background: #232369;
  color: white;
}
</style>

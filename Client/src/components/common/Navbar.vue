<template>
  <div class="flex gap-4">
      <router-link class="flex justify-center items-center px-4 hover:bg-slate-300" to="/">Trang chủ</router-link>
      <router-link class="flex justify-center items-center px-4 hover:bg-slate-300" to="/tutors">Danh sách gia sư</router-link>
      <router-link class="flex justify-center items-center px-4 hover:bg-slate-300" to="/classes">Danh sách lớp</router-link>
      <router-link class="flex justify-center items-center px-4 hover:bg-slate-300" to="/blogs">Blogs</router-link>
      <router-link class="flex justify-center items-center px-4 hover:bg-slate-300" to="/faqs">FAQs</router-link>
  </div>
</template>

<script>
export default {
    name : "NavBar"
}
</script>

<style scoped>
.router-link-active{
  background: #232369;
  color: white;
}
</style>
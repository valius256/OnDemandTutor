<template>
  <div class="flex gap-4">
      <router-link class="flex justify-center items-center px-4" to="/tutors">Danh sách gia sư</router-link>
      <router-link class="flex justify-center items-center px-4" to="/classes">Danh sách lớp</router-link>
      <router-link class="flex justify-center items-center px-4" to="/blogs">Blogs</router-link>
      <router-link class="flex justify-center items-center px-4" to="/faqs">FAQs</router-link>
  </div>
</template>

<script>
export default {
    name : "NavBar"
}
</script>

<style>
.router-link-active{
  background: #cecece;
}
</style>
<template>
  <div class="w-64 bg-blue-50">
    <div>
      <div class="flex flex-col font-bold">
        <router-link to="/tutor/profile" class="px-2 py-4 hover:bg-blue-800">
          <i class="fa fa-user text-2xl mr-2"></i>Thông tin cá nhân
        </router-link>
        <router-link to="/tutor/schedule" class="px-2 py-4 hover:bg-blue-800">
          <i class="fa fa-calendar text-2xl mr-2"></i>Thời khóa biểu
        </router-link>
        <router-link to="/tutor/subject" class="px-2 py-4 hover:bg-blue-800">
          <i class="fa fa-calendar text-2xl mr-2"></i>Môn học của tôi
        </router-link>
        <router-link to="/tutor/myclass" class="px-2 py-4 hover:bg-blue-800">
          <i class="fa fa-group text-2xl mr-2"></i>Lớp học của tôi
        </router-link>
        <router-link to="/tutor/payment" class="px-2 py-4 hover:bg-blue-800">
          <i class="fa fa-money text-2xl mr-2"></i>Thanh toán
        </router-link>
        <router-link to="/tutor/withdraw" class="px-2 py-4 hover:bg-blue-800">
          <i class="fa fa-reply text-2xl mr-2"></i>Yêu cầu rút tiền
        </router-link>
      </div>
    </div>
    <div class="flex flex-col">
      <button
        class="px-2 py-4 font-bold text-left hover:bg-blue-800"
        @click="handleLogout"
      >
        <i class="fa fa-sign-out text-2xl mr-2"></i>Đăng xuất
      </button>
    </div>
  </div>
</template>

<script>
export default {
  name: "TutorProfileNavigator",
  inject: ["eventBus"],
  methods: {
    handleLogout() {
      this.eventBus.emit("logout");
    },
  },
};
</script>

<style scoped>
.router-link-active {
  background: #0a0030;
  color: white;
}

.sub-nav .router-link-active {
  background: #0c0038;
}
</style>

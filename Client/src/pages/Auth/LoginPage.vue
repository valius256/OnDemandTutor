<template>
  <div class="flex w-full">
    <div class="hidden md:flex flex-col items-center justify-center px-8 w-2/5 bg-blue-200">
      <img src="/src/assets/OnDemandTutor.png">
      <logo/>
      <div class="font-bold text-xl">Xin chào bạn!</div>
    </div>
    <div class="w-full md:w-3/5 py-12 px-32">
      <div class="px-4 py-8 bg-white rounded-lg w-full">
        <h2 class="text-2xl font-semibold text-gray-800 mb-6 text-center">
          Đăng nhập vào OnDemandTutor
        </h2>
        <div v-if="error" class="text-red-500">{{ error }}</div>
        <div class="flex flex-col ">
          <div class="mb-4">
            <label for="emailOrPhone" class="block text-gray-700 text-sm font-bold mb-2">Email</label>
            <input id="emailOrPhone" v-model="emailOrPhone" type="text"
              class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
          </div>
          <div class="mb-4 ">
            <label for="txtpassword" class="block text-gray-700 text-sm font-bold mb-2">Mật khẩu</label>
            <input id="txtpassword" v-model="password" type="password"
              class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
          </div>
          <div class="">
            <button @click="login"
              class="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50 w-full">
              Đăng nhập
            </button>
          </div>
          
        </div>
        <div class="text-center mt-16">
            Chưa có tài khoản?
            <router-link class="text-blue-400 underline hover:text-blue-600" to="/register">
              Đăng ký tại đây
            </router-link>
          </div>
      </div>

    </div>

  </div>
</template>

<script>
import Logo from '../../components/common/Logo.vue';

export default {
  components: { Logo },
  name: "LoginForm",
  inject: ["eventBus"],
  mounted() {
    this.eventBus.on("login-set-error", (message) => {
      this.setError(message);
    });
  },
  data() {
    return {
      error: "",
      emailOrPhone: "",
      password: "",
    };
  },
  methods: {
    login() {
      this.eventBus.emit("login", {
        emailOrPhone: this.emailOrPhone,
        password: this.password,
      });
    },
    setError(error) {
      this.error = error;
    },
  },
};
</script>

<style></style>

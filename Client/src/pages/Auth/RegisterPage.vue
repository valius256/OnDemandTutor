<template>
  <div class="flex w-full">
    <div class="hidden md:flex flex-col items-center justify-center px-8 w-2/5 bg-blue-200">
      <img src="/src/assets/OnDemandTutor.png">
      <logo />
      <div class="font-bold text-xl">Xin chào bạn!</div>
    </div>
    <div class="w-full md:w-3/5 py-6 px-32">
      <div class="px-4 py-8 bg-white rounded-lg w-full">
        <h2 class="text-2xl font-semibold text-gray-800 mb-6 text-center">
          Đăng Ký
        </h2>
        <div v-if="error" class="text-red-500">{{ error }}</div>
        <label class="block text-gray-700 text-sm font-bold mb-2">Bạn là :</label>
        <div class="flex gap-8">
          <button
            class="flex gap-2 justify-center w-full border py-1 rounded-lg border-blue-100 hover:bg-blue-300 transition"
            :class="{ 'bg-blue-500 text-white': role == 0 }" @click="setRole(0)">
            <i class="fa fa-graduation-cap py-1"></i>
            <span class="">Học viên</span>
          </button>
          <button
            class="flex gap-2 justify-center w-full border py-1 rounded-lg border-blue-100 hover:bg-blue-300 transition"
            :class="{ 'bg-blue-500 text-white': role == 1 }" @click="setRole(1)">
            <i class="fa fa-book py-1"></i>
            <span>Gia sư</span>
          </button>
        </div>
        <div class="flex flex-col mt-8">
          <div class="flex place-content-between mb-2 gap-4">
            <div class="w-full">
              <label for="txtFirst" class="block text-gray-700 text-sm font-bold mb-2">Họ</label>
              <input id="txtFirst" v-model="firstName" type="text"
                class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
            </div>
            <div class="w-full">
              <label for="txtLast" class="block text-gray-700 text-sm font-bold mb-2">Tên</label>
              <input id="txtLast" v-model="lastName" type="text"
                class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
            </div>
          </div>
          <div class="flex place-content-between mb-2 gap-4">
            <div class="w-full">
              <label for="txtSdt" class="block text-gray-700 text-sm font-bold mb-2">Số điện thoại</label>
              <input id="txtSdt" v-model="phone" type="text"
                class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
            </div>
            <div class="w-full">
              <label for="txtEmail" class="block text-gray-700 text-sm font-bold mb-2">Email</label>
              <input id="txtEmail" v-model="email" type="text"
                class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
            </div>
          </div>
          <div class="flex place-content-between mb-2 gap-4">
            <div class="w-full">
              <label for="txtDob" class="block text-gray-700 text-sm font-bold mb-2">Ngày sinh</label>
              <input id="txtDob" v-model="dob" type="date"
                class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
            </div>
            <div class="w-full">
              <label class="block text-gray-700 text-sm font-bold mb-2">Giới tính</label>
              <div class="flex gap-8 p-2">
                <div class="flex gap-2">
                  <input name="rdGender" v-model="gender" type="radio" :value="1" />
                  <span>Nam</span>
                </div>
                <div class="flex gap-2">
                  <input name="rdGender" v-model="gender" type="radio" :value="0" />
                  <span>Nữ</span>
                </div>
                <div class="flex gap-2">
                  <input name="rdGender" v-model="gender" type="radio" :value="2" />
                  <span>Khác</span>
                </div>
              </div>

            </div>
          </div>
          <div class="flex place-content-between mb-2 gap-4">
            <div class="w-full">
              <label for="txtpassword" class="block text-gray-700 text-sm font-bold mb-2">Mật khẩu</label>
              <input id="txtpassword" v-model="password" type="password"
                class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
            </div>
            <div class="w-full">
              <label for="txtcfmpassword" class="block text-gray-700 text-sm font-bold mb-2">Xác nhận mật khẩu</label>
              <input id="txtcfmpassword" v-model="cfm_password" type="password"
                class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500" />
            </div>
          </div>

          <div class="mt-2">
            <button @click="register"
              class="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50 w-full">
              Đăng Ký
            </button>
          </div>

        </div>
        <div class="text-center mt-4">
          Đã có tài khoản?
          <router-link class="text-blue-400 underline hover:text-blue-600" to="/login">
            Đăng nhập tại đây
          </router-link>
        </div>
      </div>

    </div>

  </div>

</template>

<script>
import axios from 'axios';
import Logo from '../../components/common/Logo.vue';
//import { RouterLink } from 'vue-router';

export default {
  components: { Logo },
  name: "RegisterForm",
  inject: ["eventBus"],
  mounted() {
    this.eventBus.on("register-set-error", (message) => {
      this.setError(message);
    });
  },
  data() {
    return {
      error: "",
      email: "",
      password: "",
      cfm_password: "",
      address: "",
      phone: "",
      dob: null,
      gender: "",
      firstName: "",
      lastName: "",
      role: 0
    };
  },
  methods: {
    async register() {
      if (this.cfm_password != this.password) {
        this.setError("Mật khẩu không khớp");
      } else {
        try {
          this.eventBus.emit("open-loading-popup", {
            message: "Vui lòng chờ..."
          })
          const response = await axios.post(
            import.meta.env.VITE_API_URL + "/api/auth/register",
            {
              name: this.name,
              password: this.password,
              confirmPassword: this.cfm_password,
              phone: this.phone,
              email: this.email,
              dob: this.dob,
              address: this.address,
              sex: this.sex,
              firstName: this.firstName,
              lastName: this.lastName,
              isTutor: this.role == 1
            }
          );
          if (response.data) {
            const loginResponse = await axios.post(
              import.meta.env.VITE_API_URL + "/api/auth/login-firebase",
              {
                email: this.email,
                password: this.password,
              }
            );
            localStorage.setItem("token", loginResponse.data.data.message);
            await this.eventBus.emit("update-everything");
            this.$router.push((this.role == 0 ? "student" : "tutor") + "/profile")
          }
        } catch (e) {
          console.log(e)
          this.setError("Đã xảy ra sự cố. Vui lòng thử lại sau")
        }
        this.eventBus.emit("close-loading-popup")
      }
    },
    setError(error) {
      this.error = error;
    },
    setRole(role) {
      this.role = role
    },

  },
};
</script>
<style></style>

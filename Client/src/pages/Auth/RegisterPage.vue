<template>
  <div class="max-w-md w-full px-6 py-8 bg-white rounded-lg shadow-md">
    <h2 class="text-2xl font-semibold text-gray-800 mb-6">
      Sign up to PhotonPiano
    </h2>
    <div v-if="error" class="text-red-500">{{ error }}</div>
    <div class="flex gap-8">
      <div class="flex flex-col">
        <div class="mb-4">
          <label for="name" class="block text-gray-700 text-sm font-bold mb-2"
            >Name</label
          >
          <input
            id="name"
            v-model="name"
            type="text"
            class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
          />
        </div>
        <div class="mb-4">
          <label for="phone" class="block text-gray-700 text-sm font-bold mb-2"
            >Phone</label
          >
          <input
            id="phone"
            v-model="phone"
            type="text"
            class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
          />
        </div>
        <div class="mb-4">
          <label for="email" class="block text-gray-700 text-sm font-bold mb-2"
            >Email</label
          >
          <input
            id="email"
            v-model="email"
            type="email"
            class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
          />
        </div>
        <div class="mb-4">
          <label
            for="password"
            class="block text-gray-700 text-sm font-bold mb-2"
            >Password</label
          >
          <input
            id="password"
            v-model="password"
            type="password"
            class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
          />
        </div>
        <div class="mb-4">
          <label
            for="password"
            class="block text-gray-700 text-sm font-bold mb-2"
            >Confirm Password</label
          >
          <input
            id="password"
            v-model="cfm_password"
            type="password"
            class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
          />
        </div>
      </div>
      <div class="flex flex-col">
        <div class="mb-4">
          <label for="dob" class="block text-gray-700 text-sm font-bold mb-2"
            >Date of Birth</label
          >
          <input
            id="dob"
            v-model="dob"
            type="date"
            class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
          />
        </div>
        <div class="mb-4">
          <label
            for="address"
            class="block text-gray-700 text-sm font-bold mb-2"
            >Address</label
          >
          <textarea
            id="address"
            v-model="address"
            class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
          ></textarea>
        </div>
        <div class="mb-4">
          <label for="gender" class="block text-gray-700 text-sm font-bold mb-2"
            >Gender</label
          >
          <select
            id="gender"
            v-model="gender"
            class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
          >
            <option value="" disabled selected>Select your gender</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
            <option value="Other">Other</option>
          </select>
        </div>
        <div class="mb-4">
          <label
            for="bankAccount"
            class="block text-gray-700 text-sm font-bold mb-2"
            >Bank Account</label
          >
          <input
            id="bankAccount"
            v-model="bankAccount"
            type="text"
            class="w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-blue-500 focus:border-blue-500"
          />
        </div>
      </div>
    </div>
    <div class="flex items-center justify-center">
      <button
        @click="register"
        class="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50"
      >
        Register
      </button>
    </div>
    <div class="text-center">
      Already have an account?
      <router-link
        class="underline text-blue-400 hover:text-blue-600"
        to="/login"
      >
        Sign in here
      </router-link>
    </div>
  </div>
</template>

<script>
//import { RouterLink } from 'vue-router';

export default {
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
      bankAccount: "",
    };
  },
  methods: {
    register() {
      if (this.cfm_password != this.password) {
        this.setError("Password and confirm password didn't match");
      } else {
        this.eventBus.emit("register", {
          name: this.name,
          password: this.password,
          phone: this.phone,
          email: this.email,
          dob: this.dob,
          address: this.address,
          gender: this.gender,
          bankAccount: this.bankAccount ? this.bankAccount : null,
        });
      }
    },
    switchToLogin() {
      this.eventBus.emit("open-register-popup");
      this.eventBus.emit("open-login-popup");
    },
    setError(error) {
      this.error = error;
    },
  },
};
</script>
<style></style>

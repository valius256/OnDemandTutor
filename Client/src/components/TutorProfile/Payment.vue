<template>
  <div>
    <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200">
      Thông tin thanh toán
    </div>
    <div class="flex justify-center mb-8">
      <div class="text-3xl font-bold py-1">
        Số dư hiện tại :
        <span class="text-green-200 p-1 bg-green-600 rounded-lg">
          {{
            currentUser.balance.toLocaleString("vi-VN", {
              style: "currency",
              currency: "VND",
            })
          }}
        </span>
      </div>
    </div>
    <div class="m-8">
      <div class="flex justify-center gap-4">
        <button
          class="p-2 text-xl font-bold text-white bg-blue-400 hover:bg-blue-200 rounded-lg"
          @click="openModal"
        >
          Nạp tiền
        </button>
      </div>
      <div
        v-if="showModal"
        class="fixed inset-0 flex items-center justify-center bg-gray-800 bg-opacity-50"
      >
        <div class="bg-white p-8 rounded-lg">
          <h2 class="text-xl font-bold mb-4">Nạp tiền vào ví</h2>
          <div class="mb-4">
            <label class="block text-gray-700 text-sm font-bold mb-2">
              Số tiền muốn nạp
            </label>
            <input
              v-model="amount"
              type="number"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
            />
          </div>
          <div class="flex justify-end">
            <button
              class="mr-4 p-2 text-xl font-bold text-white bg-blue-400 hover:bg-blue-200 rounded-lg"
              @click="deposit"
            >
              Nạp tiền
            </button>
            <button
              class="p-2 text-xl font-bold text-white bg-red-400 hover:bg-red-200 rounded-lg"
              @click="closeModal"
            >
              Hủy
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "TutorProfilePayment",
  props: ["currentUser"],
  data() {
    return {
      showModal: false,
      amount: 0,
    };
  },
  methods: {
    openModal() {
      this.showModal = true;
    },
    closeModal() {
      this.showModal = false;
    },
    async deposit() {
      try {
        const response = await axios.post(
          import.meta.env.VITE_API_URL + "/api/User/deposit",
          { amount: this.amount },
          {
            headers: {
              Authorization: "Bearer " + localStorage.token,
            },
          }
        );
        if (response.data) {
          this.currentUser.balance = response.data.newBalance;
          this.closeModal();
        }
      } catch (error) {
        console.error("Failed to deposit money:", error);
      }
    },
  },
};
</script>

<style>
/* Add any additional styles if necessary */
</style>

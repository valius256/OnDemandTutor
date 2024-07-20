<template>
  <div
    class="p-4 bg-white rounded-b-lg w-full overflow-y-auto max-h-[30rem] overflow-x-auto"
  >
    <div class="flex">
      <span class="w-48 p-1 font-bold">Nhập số tiền muốn nạp</span>
      <input
        v-model="amount"
        class="p-1 border rounded-lg"
        placeholder="Nhập số tiền"
      />
    </div>
    <div class="flex mt-4">
      <span class="w-48 p-1 font-bold">Số tài khoản</span>
      <input
        v-model="bankAccountNumber"
        class="p-1 border rounded-lg"
        placeholder="Nhập số tài khoản"
      />
    </div>
    <div class="flex mt-4">
      <span class="w-48 p-1 font-bold">Ngân hàng</span>
      <input
        v-model="bankName"
        class="p-1 border rounded-lg"
        placeholder="Nhập tên ngân hàng"
      />
    </div>
    <div class="flex justify-center mt-4 gap-3">
      <button
        @click="createRechargeRequest"
        class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg"
      >
        Xác nhận
      </button>
      <button
        @click="close"
        class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg"
      >
        Hủy bỏ
      </button>
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "RechargePopup",
  props: ["close", "action"],
  data() {
    return {
      amount: 0,
      bankAccountNumber: "",
      bankName: "",
    };
  },
  methods: {
    async createRechargeRequest() {
      try {
        const response = await axios.post(
          import.meta.env.VITE_API_URL + "/api/Payment/create-recharge",
          {
            amount: this.amount,
            notes: "Recharge request",
            returnUrl: window.location.href,
          },
          {
            headers: {
              Authorization: "Bearer " + localStorage.token,
            },
          }
        );
        this.close();
        this.action();
        // Handle response as needed
        if (response.data.status === "success") {
          // Assuming response contains a URL to redirect for payment
          window.location.href = response.data.data;
        }
      } catch (error) {
        console.error("Error during recharge request:", error);
        // Handle error as needed
      }
    },
  },
};
</script>

<style></style>

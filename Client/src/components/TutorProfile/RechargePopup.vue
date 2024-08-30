<template>
  <div class="p-4 bg-white rounded-b-lg w-full overflow-y-auto max-h-[30rem] overflow-x-auto">
    <div class="flex">
      <span class="w-48 p-1 font-bold">Nhập số tiền muốn nạp</span>
      <input v-model="amount" class="p-1 border rounded-lg" placeholder="Nhập số tiền" />
    </div>
    <div class="flex justify-center mt-4 gap-3">
      <button @click="createRechargeRequest" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">
        Xác nhận
      </button>
      <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">
        Hủy bỏ
      </button>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import GenericPopup from "../common/GenericPopup.vue";

export default {
  components: { GenericPopup },
  inject : ['eventBus'],
  name: "RechargePopup",
  props: ["close", "action"],
  data() {
    return {
      amount: 0,
      bankAccountNumber: "",
      bank: null,
      isOpenBankPopup: false,
    };
  },
  methods: {
    toggleBankPopup() {
      this.isOpenBankPopup = !this.isOpenBankPopup;
    },
    handleSelectBank(bank) {
      if (bank != null) {
        this.bank = bank;
        this.isOpenBankPopup = false;
      }
    },
    async createRechargeRequest(confirmation) {
      if (confirmation) {
        this.eventBus.emit("open-confirmation-popup", {
          message: "Bạn có chắc chắn muốn nạp " + this.amount.toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                    }) + " vào tài khoản?",
          method: this.createRechargeRequest,
          params: false
        })
      } else {
        this.eventBus.emit("open-loading-popup", {
          message: "Vui lòng chờ..."
        })
        try {
          const response = await axios.post(
            import.meta.env.VITE_API_URL + "/api/Payment/create-recharge",
            {
              amount: this.amount,
              notes: "Nạp tiền tự do " + new Date(),
              returnUrl: window.location.href,
            },
            {
              headers: {
                Authorization: "Bearer " + localStorage.token,
              },
            }
          );
          this.eventBus.emit("open-result-dialog", {
            message: "Thành công, đang điều hướng bạn về trang thanh toán",
            type: "Success"
          })
          window.location.href = response.data
        } catch (e) {
          console.log(e)
          this.eventBus.emit("open-result-dialog", {
            message: "Không thể thực hiện được. Vui lòng thử lại sau",
            type: "Error"
          })
        }
        this.eventBus.emit("close-loading-popup")
      }
    },
  },
};
</script>

<style></style>

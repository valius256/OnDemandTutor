<template>
  <div>
    <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200">
      Thông tin thanh toán
    </div>
    <div class="flex justify-center mb-8">
      <div class="text-3xl font-bold py-1">
        <div class="mb-4">Số dư hiện tại</div>
        <div class="text-green-200 p-1 bg-green-600 rounded-lg text-center">
          {{
            balance.toLocaleString("vi-VN", {
              style: "currency",
              currency: "VND",
            })
          }}
        </div>
      </div>
    </div>
    <div class="flex gap-4 justify-center mt-4 text-2xl mb-6">
      <button
        @click="toggleRechargePopup"
        class="mr-6 px-6 py-4 font-bold text-white bg-blue-400 hover:bg-blue-200 rounded-lg"
      >
        Nạp tiền
      </button>
      <button
        @click="toggleWithdrawPopup"
        class="px-6 py-4 font-bold text-white bg-green-400 hover:bg-green-200 rounded-lg"
      >
        Rút tiền
      </button>
    </div>
    <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200">
      Các Slot chưa thanh toán
    </div>
    <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200">
      Lịch sử giao dịch
    </div>
    <div class="px-4 mb-4">
      <table
        class="bg-slate-50 p-6 rounded-xl text-center w-full"
        v-if="transactions.length > 0"
      >
        <thead>
          <th>Code</th>
          <th>Date</th>
          <th>Amount</th>
          <th>Description</th>
        </thead>
        <tbody>
          <tr v-for="transaction in transactions" :key="transaction.id">
            <td>{{ transaction.transactionCode }}</td>
            <td>{{ this.beautifyDatetime(transaction.createdDate) }}</td>
            <td :class="getAmountStyle(transaction.amount)">
              {{
                transaction.amount.toLocaleString("vi-VN", {
                  style: "currency",
                  currency: "VND",
                })
              }}
            </td>
            <td>{{ transaction.notes }}</td>
          </tr>
        </tbody>
      </table>
      <div
        class="flex gap-4 justify-center mt-4"
        v-if="transactions.length > 0"
      >
        <button @click="movePage(false)">
          <i class="fa fa-arrow-left text-2xl"></i>
        </button>
        <div class="flex gap-2">
          <input
            class="border p-1 rounded-md w-16"
            type="number"
            v-model="currentPage"
            min="1"
            @change="handlePageChange"
          />
          <div class="p-1">/ {{ this.totalPage }}</div>
        </div>
        <button @click="movePage(true)">
          <i class="fa fa-arrow-right text-2xl"></i>
        </button>
      </div>
      <div v-else class="text-center italic">Hiện chưa có giao dịch nào</div>
    </div>
    <generic-popup
      v-if="isOpenWithdrawPopup"
      :title="'Tạo yêu cầu rút tiền'"
      :closeFunction="toggleWithdrawPopup"
      :notOverflow="true"
    >
      <request-withdraw-popup
        :close="toggleWithdrawPopup"
        :action="navigateToPayment"
        :balance="balance"
      ></request-withdraw-popup>
    </generic-popup>
    <generic-popup
      v-if="isOpenRechargePopup"
      :title="'Nạp tiền'"
      :closeFunction="toggleRechargePopup"
      :notOverflow="true"
    >
      <recharge-popup
        :close="toggleRechargePopup"
        :action="navigateToPayment"
      ></recharge-popup>
    </generic-popup>
  </div>
</template>

<script>
import axios from "axios";
import GenericPopup from "../common/GenericPopup.vue";
import RequestWithdrawPopup from "./RequestWithdrawPopup.vue";
import RechargePopup from "./RechargePopup.vue"; // Import the new RechargePopup component

export default {
  components: { GenericPopup, RequestWithdrawPopup, RechargePopup },
  props: ["id"],
  name: "TutorProfilePayment",
  data() {
    return {
      totalPage: 100,
      pageSize: 10,
      currentPage: 1,
      balance: 0,
      user: null,
      transactions: [],
      isOpenWithdrawPopup: false,
      isOpenRechargePopup: false, // New state for the recharge popup
    };
  },
  methods: {
    getAmountStyle(amount) {
      let css = "font-bold";
      if (amount < 0) {
        return css + " text-red-400";
      } else {
        return css + " text-green-400";
      }
    },
    async handlePageChange() {
      if (this.currentPage > this.totalPage) {
        this.currentPage = this.totalPage;
      }
      if (this.currentPage < 1) {
        this.currentPage = 1;
      }
      await this.fetchTranscations();
    },
    async movePage(forward) {
      if (forward && this.currentPage < this.totalPage) {
        this.currentPage++;
        await this.handlePageChange();
      } else if (!forward && this.currentPage > 1) {
        this.currentPage--;
        await this.handlePageChange();
      }
    },
    async fetchTranscations() {
      let query = {
        Page: this.currentPage,
        Limit: this.pageSize,
      };
      const response = await axios.get(
        import.meta.env.VITE_API_URL +
          "/api/Transaction/all?" +
          this.jsonToQueryString(query),
        {
          headers: {
            Authorization: "Bearer " + localStorage.token,
          },
        }
      );
      if (response.data) {
        this.transactions = response.data.items;
        this.totalPage = Math.ceil(response.data.total / this.pageSize);
      }
    },
    toggleWithdrawPopup() {
      this.isOpenWithdrawPopup = !this.isOpenWithdrawPopup;
    },
    toggleRechargePopup() {
      this.isOpenRechargePopup = !this.isOpenRechargePopup;
    },
    navigateToPayment() {
      this.$router.push("/tutor/withdraw");
    },
    async fetchUser() {
      console.log(this.id);
      if (this.id) {
        const response = await axios.get(
          import.meta.env.VITE_API_URL + "/api/User/profile?userId=" + this.id
        );
        if (response.data) {
          this.user = response.data.data;
        }
        const balanceResponse = await axios.get(
          import.meta.env.VITE_API_URL + "/api/User/balance",
          {
            headers: {
              Authorization: "Bearer " + localStorage.token,
            },
          }
        );
        if (balanceResponse.data) {
          this.balance = balanceResponse.data.data.balance;
        }
      }
    },
  },
  mounted() {
    this.fetchTranscations();
    this.fetchUser();
  },
};
</script>

<style scoped>
tr td,
th {
  padding: 0.5rem 2rem 0.5rem 2rem;
  border: solid 1px #ffffff;
}
</style>

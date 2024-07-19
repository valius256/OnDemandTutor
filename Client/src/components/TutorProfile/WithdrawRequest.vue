<template>
  <div>
    <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200">
      Yêu cầu rút tiền của bạn
    </div>
    <div class="px-4 mb-4">
      <table
        class="bg-slate-50 p-6 rounded-xl text-center w-full"
        v-if="withdrawRequests.length > 0"
      >
        <thead class="mt-2">
          <th>Số lượng</th>
          <th>Lý do</th>
          <th>Tài khoản nhận</th>
          <th>Ngân hàng</th>
          <th>Tạo ngày</th>
          <th>Duyệt bởi</th>
          <th>Phản hồi</th>
          <th>Cập nhật ngày</th>
          <th>Trạng thái</th>
        </thead>
        <tbody>
          <tr
            v-for="request in withdrawRequests"
            :key="request.id"
            class="border-2 border-white"
          >
            <td class="p-2">
              {{
                request.amount.toLocaleString("vi-VN", {
                  style: "currency",
                  currency: "VND",
                })
              }}
            </td>
            <td class="break-words w-32">{{ request.description }}</td>
            <td>{{ request.bankAccountNumber }}</td>
            <td class="break-words w-32">{{ request.bankName }}</td>
            <td class="break-words w-32">
              {{ this.beautifyDatetime(request.createdDate) }}
            </td>
            <td>{{ request.operator?.name }}</td>
            <td class="break-words w-32">{{ request.reply }}</td>
            <td class="break-words w-32">
              {{ this.beautifyDatetime(request.updatedDate) }}
            </td>
            <td>
              <div :class="getStatusStyle(request.status)">
                {{ getStatusDisplay(request.status) }}
              </div>
            </td>
          </tr>
        </tbody>
      </table>
      <div
        class="flex gap-4 justify-center mt-4"
        v-if="withdrawRequests.length > 0"
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
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "WithdrawRequest",
  props: ["id"],
  data() {
    return {
      totalPage: 100,
      pageSize: 10,
      currentPage: 1,
      withdrawRequests: [],
    };
  },
  methods: {
    async fetchWithdraw() {
      let query = {
        Page: this.currentPage,
        Limit: this.pageSize,
      };
      const response = await axios.get(
        import.meta.env.VITE_API_URL +
          "/api/RequestWithDraw/all?" +
          this.jsonToQueryString(query),
        {
          headers: {
            Authorization: "Bearer " + localStorage.token,
          },
        }
      );
      if (response.data) {
        this.withdrawRequests = response.data.items;
        this.totalPage = Math.ceil(response.data.total / this.pageSize);
      }
    },
    async handlePageChange() {
      if (this.currentPage > this.totalPage) {
        this.currentPage = this.totalPage;
      }
      if (this.currentPage < 1) {
        this.currentPage = 1;
      }
      await this.fetchWithdraw();
    },
    async movePage() {
      if (forward && this.currentPage < this.totalPage) {
        this.currentPage++;
        await this.handlePageChange();
      } else if (!forward && this.currentPage > 1) {
        this.currentPage--;
        await this.handlePageChange();
      }
    },
    getStatusStyle(status) {
      let css = "rounded-lg font-bold text-white px-3 py-2";
      if (status == 0) {
        return css + " bg-gray-400";
      } else if (status == 1) {
        return css + " bg-green-400";
      } else {
        return css + " bg-red-400";
      }
    },
    getStatusDisplay(status) {
      if (status == 0) {
        return "Đang duyệt";
      } else if (status == 1) {
        return "Thành công";
      } else {
        return "Thất bại";
      }
    },
  },
  mounted() {
    this.fetchWithdraw();
  },
};
</script>

<style></style>

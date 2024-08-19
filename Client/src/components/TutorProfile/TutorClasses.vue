<template>
  <div class="">
    <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200">
        Lớp học bạn đã tạo
      </div>
    <div v-if="currentUser.tutorStatus == 3">
      <div class="flex justify-center">
        <button @click="this.$router.push('/tutor/myclass/create')"
          class="font-bold bg-blue-400 text-white rounded-lg hover:bg-blue-200 px-12 py-2">
          Tạo lớp mới
        </button>
      </div>
      <hr class="mt-4">
      <div class="px-8 py-2" v-if="!isOpenClassDetailPopup">
        <class-list :currentUser="currentUser" :handlePageChange="handlePageChange" :movePage="movePage" :classes="classes"
          :toggleClassDetailPopup="toggleClassDetailPopup"
          :pageModel="{ page: currentPage, total: totalPage }"></class-list>
      </div>
      <div v-else>
        <button class="ml-8 mt-8 px-8 py-2 bg-blue-400 font-bold text-white rounded-lg" @click="toggleClassDetailPopup">
          Trở về
        </button>
        <class-detail-popup :classId="selectedClass" :close="toggleClassDetailPopup"></class-detail-popup>
      </div>
    </div>
    <div v-else class="p-8">
      <div class="p-8 bg-red-200 rounded-lg text-center font-bold">
        Bạn cần xác thực tài khoản để sử dụng tính năng này

      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import GenericPopup from "../common/GenericPopup.vue";
import ClassDetailPopup from "./ClassDetailPopup.vue";
import ClassList from '../common/ClassList.vue';
export default {
  components: { GenericPopup, ClassDetailPopup, ClassList },
  name: "TutorClasses",
  props: ['id','currentUser'],
  data() {
    return {
      totalPage: 100,
      pageSize: 10,
      currentPage: 1,
      selectedClass: 0,
      isOpenClassDetailPopup: false,
      classes: [],
    };
  },
  methods: {
    async fetchData() {
      let query = {
        Page: this.currentPage,
        Limit: this.pageSize,
      };
      let queryStr = this.jsonToQueryString(query);
      const response = await axios.get(
        import.meta.env.VITE_API_URL + "/api/Class/tutor?" + queryStr,
        {
          headers: {
            Authorization: "Bearer " + localStorage.token,
          },
        }
      );
      if (response.data) {
        this.classes = response.data.items;
        this.totalPage = Math.ceil(response.data.total / this.pageSize);
      }
    },
    getMethodStyle(method) {
      let general = "ml-4 rounded-lg px-3 py-1 text-white font-bold";
      switch (method) {
        case "Online":
          return general + " bg-green-400";
        default:
          return general + " bg-gray-400";
      }
    },
    getStatusStyleHeader(status) {
      let general = "font-bold text-center py-4 rounded-t-lg text-white";
      switch (status) {
        case 0:
          return general + " bg-cyan-500";
        case 1:
          return general + " bg-green-400";
        default:
          return general + " bg-gray-400";
      }
    },
    getStatusStyle(status) {
      let general = "ml-3 rounded-lg px-3 py-1 font-bold";
      switch (status) {
        case 0:
          return general + " text-blue-400";
        case 1:
          return general + " text-green-400";
        default:
          return general + " text-gray-400";
      }
    },
    getStatusDisplay(status) {
      switch (status) {
        case 0:
          return "Sắp bắt đầu";
        case 1:
          return "Đang diễn ra";
        case 2:
          return "Đã kết thúc";
        default:
          return "Không rõ";
      }
    },
    async handlePageChange() {
      if (this.currentPage > this.totalPage) {
        this.currentPage = this.totalPage;
      }
      if (this.currentPage < 1) {
        this.currentPage = 1;
      }
      await this.fetchData();
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
    toggleClassDetailPopup(id) {
      scrollTo(0, 0);
      this.selectedClass = id;
      this.isOpenClassDetailPopup = !this.isOpenClassDetailPopup;
    },
  },
  mounted() {
    this.fetchData();
  },
};
</script>

<style></style>

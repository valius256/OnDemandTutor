<template>
  <div class="p-4 bg-white rounded-b-lg flex flex-col w-screen lg:w-auto">
    <div class="flex gap-4 w-full">
      <div class="w-full">
        <div class="mb-8" v-if="slot.class">
          <div>
            <span class="font-bold">Tên lớp :</span>
            <span class="ml-4">{{ slot.class.name }}</span>
          </div>
          <hr />
        </div>
        <div>
          <span class="font-bold">Môn học :</span>
          <span class="font-bold text-blue-400 ml-4">{{
          slot.subject?.name
        }}</span>
        </div>
        <hr />
        <div class="mt-8 gap-8 flex place-content-between">

          <div>
            <div class="">
              <span class="font-bold">Bắt đầu :</span>
              <span class="ml-4">{{ beautifyDatetime(slot.startTime) }}</span>
            </div>
            <div>
              <span class="font-bold">Kết thúc :</span>
              <span class="ml-4">{{ beautifyDatetime(slot.endTime) }}</span>
            </div>
            <div>
              <span class="font-bold">Tổng thời lượng :</span>
              <span class="ml-4">{{ calcDuration() }} tiếng</span>
            </div>
          </div>

          <div>
            <div class="">
              <span class="font-bold">Địa điểm :</span>
              <span class="ml-4">{{ slot.teachAddress }}</span>
            </div>
            <div class="">
              <span class="font-bold">Phương thức :</span>
              <span v-if="slot.isOnline" class="ml-4 font-bold text-green-500">Online</span>
              <span v-else class="ml-4 font-bold text-gray-500">Offline</span>
            </div>
          </div>
        </div>

        <hr />

      </div>
    </div>
    <hr />
    <div class="mt-4">
      <div class="max-h-64 overflow-x-auto overflow-y-auto relative ">
        <table class="mt-2 text-center">
          <thead class="border-b sticky top-0 z-10 bg-white bg-opacity-50">
            <tr>
              <th class="max-w-[16rem] min-w-[16rem] ">Học sinh</th>
              <th class="max-w-[8rem] min-w-[8rem] ">Email</th>
              <th class="max-w-[8rem] min-w-[8rem] ">SDT</th>
              <th class="max-w-[16rem] min-w-[16rem] ">Đánh giá</th>
            </tr>
          </thead>
          <tbody class="">
            <tr v-for="student in slotStudents" :key="student.user.id">
              <td class="p-2">
                <div class="flex gap-2 items-center">
                  <img class="w-16 h-16 rounded-full" :src="student.user.avatarImageUrl">
                  <span class="font-bold">{{ (student.user.firstName ?? "") + " " + (student.user.lastName ?? "") }}</span>
                </div>

              </td>
              <td class="p-2">{{ student.user.email }}</td>
              <td class="p-2">{{ student.user.phone }}</td>
              <td class="w-48 text-wrap">
                <star-rating class="flex justify-center" :star-size="20" :rating="student.rating" :round-start-rating="false"
                            :read-only="true" />
                {{ student.feedback }}
              </td>
            </tr>
            
          </tbody>
        </table>
      </div>

      <div class="flex gap-4 justify-center mt-4" v-if="this.slotStudents.length > 0">
        <button @click="movePage(false)">
          <i class="fa fa-arrow-left text-2xl"></i>
        </button>
        <div class="flex gap-2 ">
          <input class="border p-1 rounded-md w-16" type="number" v-model="currentPage" min="1"
            @change="handlePageChange">
          <div class="p-1"> / {{ this.totalPage }}</div>
        </div>
        <button @click="movePage(true)">
          <i class="fa fa-arrow-right text-2xl"></i>
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import StarRating from 'vue-star-rating'

export default {
  name: "SlotDetailPopup",
  props: ["slot", "close"],
  components : {StarRating},
  data() {
    return {
      totalPage: 100,
      pageSize: 5,
      currentPage: 1,
      slotStudents: [],
    };
  },
  methods: {
    calcDuration() {
      const startTime = new Date(this.slot.startTime);
      const endTime = new Date(this.slot.endTime);
      return (endTime - startTime) / 3600000;
    },
    beautifyDatetime(datetime) {
      const options = {
        year: "numeric",
        month: "long",
        day: "numeric",
        hour: "2-digit",
        minute: "2-digit",
      };
      return new Date(datetime).toLocaleDateString("vi-VN", options);
    },
    async fetchSlotStudents() {
      console.log("Fetching slot students for slotId:", this.slot.id);
      if (this.slot.id) {
        try {
          const response = await axios.get(
            `${import.meta.env.VITE_API_URL}/api/SlotStudent/${this.slot.id}?page=${this.currentPage}&limit=${this.pageSize}`
          );
          this.slotStudents = response.data.items;
          this.totalPage = Math.ceil(response.data.items.length / this.pageSize)
        } catch (error) {
          console.error("Error fetching slot students:", error);
        }
      }
    },
    async handlePageChange() {
      if (this.currentPage > this.totalPage) {
        this.currentPage = this.totalPage
      }
      if (this.currentPage < 1) {
        this.currentPage = 1
      }
      await this.fetchSlotStudents()
    },
    async movePage(forward) {
      if (forward && this.currentPage < this.totalPage) {
        this.currentPage++
        await this.handlePageChange()
      } else if (!forward && this.currentPage > 1) {
        this.currentPage--
        await this.handlePageChange()
      }
    },
  },
  mounted() {
    this.fetchSlotStudents();
    console.log(this.slot)
  },
};
</script>

<style></style>

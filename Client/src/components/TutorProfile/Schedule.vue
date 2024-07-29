<template>
  <div>
    <div v-if="currentUser.tutorStatus == 3">
      <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200">
        Slot học tiếp theo
      </div>
      <div class="m-8">
        <div class="flex gap-4 mb-2">
          <div class="text-xl font-bold py-1">
            <span class="mr-4">Số dư hiện tại : </span>
            <span class="text-green-200 p-1 bg-green-600 rounded-lg">{{
      balance.toLocaleString("vi-VN", {
        style: "currency",
        currency: "VND",
      })
    }}</span>
          </div>
        </div>
        <div v-if="upcomingSlot">
          <div class="p-4 bg-blue-100 rounded-lg">
            <div>
              <span class="font-bold">Trạng thái : </span>
              <span :class="getSlotStatus(upcomingSlot.startTime, upcomingSlot.endTime)
        .style
      ">
                {{
      getSlotStatus(upcomingSlot.startTime, upcomingSlot.endTime)
        .display
    }}
              </span>
            </div>
            <div class="flex place-content-between mt-4">
              <div>
                <span class="mr-4 font-bold">Bắt đầu :</span>
                <span class="mr-4">{{
        beautifyDatetime(upcomingSlot.startTime)
      }}</span>
              </div>
              <div>
                <span class="mr-4 font-bold">Kết thúc :</span>
                <span class="mr-4">{{
        beautifyDatetime(upcomingSlot.endTime)
      }}</span>
              </div>
              <div>
                <span class="mr-4 font-bold">Tổng thời lượng :</span>
                <span class="mr-4">{{ calcDuration(upcomingSlot).toFixed(2) }} tiếng</span>
              </div>
            </div>
          </div>
        </div>
        <div v-else class="italic">Hiện không còn slot nào</div>
      </div>
      <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200">
        Thời khóa biểu
      </div>
      <div class="flex justify-center mr-8 mb-4">
        <button class="px-32 py-2 font-bold text-xl text-white bg-blue-400 hover:bg-blue-200 rounded-lg" @click="openAddSlotModal">
          Thêm slot
        </button>
      </div>
      <time-table :slots="slots" :fetching="getUserSlots" :viewDetail="openSlotDetailPopup" :role="'tutor'"/>
    </div>
    <div v-else class="p-8">
      <div class="p-8 bg-red-200 rounded-lg text-center font-bold">
        Bạn cần xác thực tài khoản để sử dụng tính năng này

      </div>
    </div>
    <add-slot-modal :showModal="showModal" @close="showModal = false" @add="addNewSlot" :currentUser="currentUser" />
    <generic-popup v-if="isOpenSlotDetailPopup" title="Chi tiết buổi học" :closeFunction="closeSlotDetailPopup"
      :notOverflow="true">
      <slot-detail-popup :slot="selectingSlot" :close="closeSlotDetailPopup" :action="refresh" />
    </generic-popup>
  </div>
</template>

<script>
import axios from "axios";
import TimeTable from "../StudentProfile/TimeTable.vue";
import AddSlotModal from "./AddSlotModal.vue";
import GenericPopup from "../common/GenericPopup.vue";
import SlotDetailPopup from "./SlotDetailPopup.vue";

export default {
  components: { TimeTable, GenericPopup, SlotDetailPopup, AddSlotModal },
  name: "TutorProfileSchedule",
  props: ["currentUser"],
  data() {
    return {
      balance: 0,
      isOpenSlotDetailPopup: false,
      slots: [],
      upcomingSlot: null,
      showModal: false,
      selectingSlot: null,
    };
  },
  methods: {
    getStatusDisplay(status) {
      let css = "px-4 py-1 text-white font-bold rounded-lg text-center";
      switch (status) {
        case "Active":
          return {
            css: css + " bg-green-500",
            display: "Đang diễn ra",
          };
        case "Pending":
          return {
            css: css + " bg-gray-500",
            display: "Chờ gia sư",
          };
        case "Finished":
          return {
            css: css + " bg-blue-500",
            display: "Đã hoàn thành",
          };
        default:
          return {
            css: css + " bg-gray-500",
            display: "Không rõ",
          };
      }
    },
    async getClosestSlot() {
      const userId = this.currentUser.id;
      const column = "startTime"; // Example column name
      const isDesc = false; // We want to sort in ascending order to find the upcoming slot

      try {
        const response = await axios.get(
          `${import.meta.env.VITE_API_URL
          }/api/Slot?Filter.UserId=${userId}&Sorts[column]=${column}&Sorts[isDesc]=${isDesc}`,
          {
            headers: {
              Authorization: `Bearer ${localStorage.token}`,
            },
          }
        );

        if (response.data && response.data.items) {
          const slots = response.data.items;
          const now = new Date();
          // Find the closest upcoming slot
          this.upcomingSlot = slots.find(
            (slot) => new Date(slot.startTime) > now
          );
          console.log(
            "sssssssssssssssssssssssssssssssssssssssssss" + this.upcomingSlot
          );
          console.log(this.upcomingSlot);
        } else {
          this.upcomingSlot = null; // Ensure upcomingSlot is null if no slots are found
        }
      } catch (error) {
        console.error("Error fetching user slots:", error);
        this.upcomingSlot = null; // Handle errors by setting upcomingSlot to null
      }
    },
    getSlotStatus(startTime, endTime) {
      let generalCss = "p-2 text-white font-bold rounded-lg";
      const time = new Date(startTime);
      const timeEnd = new Date(endTime);
      const present = new Date();
      if (time > present) {
        return {
          style: generalCss + " bg-gray-500",
          display: "Sắp bắt đầu",
        };
      } else if (time <= present && present < timeEnd) {
        return {
          style: generalCss + " bg-green-500",
          display: "Đang diễn ra",
        };
      } else {
        return {
          style: generalCss + " bg-gray-500",
          display: "Đã qua",
        };
      }
    },
    async fetchBalance() {
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
    },
    async getUserSlots() {
      const userId = this.currentUser.id;
      try {
        const response = await axios.get(
          `${import.meta.env.VITE_API_URL
          }/api/Slot?get-student-slots-tutor?Filter.TutorId=${userId}&Page=1&Limit=100`,
          {
            headers: {
              Authorization: `Bearer ${localStorage.token}`,
            },
          }
        );
        console.log(response.data); // Log the response data for debugging
        if (response.data && response.data.items) {
          this.slots = response.data.items;
        } else {
          this.slots = []; // Ensure slots is an array even if the response is empty
        }
      } catch (error) {
        console.error("Error fetching user slots:", error);
        this.slots = []; // Handle errors by setting slots to an empty array
      }
    },
    addNewSlot(newSlot) {
      this.slots.push(newSlot);
      this.upcomingSlot = this.getClosestSlot(this.slots);
    },
    async refresh() {
      try {
        await this.getUserSlots();
        await this.fetchBalance();
        await this.getClosestSlot();
      } catch (e) {
        console.log(e);
      }
    },
    openSlotDetailPopup(slot) {
      this.selectingSlot = slot;
      this.isOpenSlotDetailPopup = true;
    },
    closeSlotDetailPopup() {
      this.isOpenSlotDetailPopup = false;
    },
    calcDuration(slot) {
      const startTime = new Date(slot.startTime);
      const endTime = new Date(slot.endTime);
      return (endTime - startTime) / 3600000;
    },
    openAddSlotModal() {
      this.showModal = true;
    },
  },
  mounted() {
    this.refresh();
  },
};
</script>

<style>
/* Add any additional styles if necessary */
</style>

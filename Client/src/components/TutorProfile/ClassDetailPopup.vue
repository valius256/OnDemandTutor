<template>
  
  <div class="py-4 px-8 relative" v-if="this.class">
    <div class="font-bold text-3xl">{{ this.class.name }}</div>
    <div class="mt-4 flex flex-col md:flex-row md:place-content-between gap-4">
      <div>
        <div class="flex">
          <span>Gia sư :</span>
          <button class="font-bold text-blue-400 underline ml-4">
            {{ this.class.tutor.firstName + " " + this.class.tutor.lastName }}
          </button>
        </div>
        <div class="mt-2">
          <span class="">Thời gian :</span>
          <span class="font-bold ml-4">
            {{ this.class.startTime?.substring(0, 10) }} đến
            {{ this.class.endTime?.substring(0, 10) }}
          </span>
        </div>
        <div class="mt-2">
          <span class="">Môn học :</span>
          <span class="font-bold ml-4">{{ this.class.subject.name }}</span>
        </div>
      </div>
      <div>
        <div>
          <span class="">Địa điểm :</span>
          <span class="font-bold ml-3">{{ this.class.location }}</span>
        </div>
        <div class="mt-2">
          <span class="">Hình thức :</span>
          <span :class="getMethodStyle(this.class.method)">
            {{ this.class.method }}
          </span>
        </div>
        <div class="mt-2">
          <span class="">Trạng thái :</span>
          <span :class="getStatusStyle(this.class.status)">
            {{ getStatusDisplay(this.class.status) }}
          </span>
        </div>
      </div>
    </div>
    <hr class="mt-4" />
    <div class="font-bold my-4">Thời khóa biểu :</div>
    <time-table
      :slots="this.slots"
      :fetching="getUserSlots"
      :view-detail="openSlotDetailPopup"
      :role="'tutor'"
    />
    <hr class="mt-4" />
    <div class="font-bold my-4">Các buổi học :</div>
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
      <div
        v-for="(slot, index) in this.slots"
        :key="index"
        class="p-2 border-4 shadow-md mb-2 rounded-lg"
        :class="{
          'border-blue-300':
            this.compareDate(new Date(slot.endTime), new Date()) < 0,
        }"
      >
        <div class="font-bold text-center text-xl">Buổi {{ index + 1 }}</div>
        <hr />
        <div class="flex flex-col gap-2">
          <div class="flex flex-col gap-2 min-h-48">
            <div class="text-sm">
              <span class="font-bold">Bắt đầu :</span>
              <span class="ml-4">{{ beautifyDatetime(slot.startTime) }}</span>
            </div>
            <div class="text-sm">
              <span class="font-bold">Kết thúc :</span>
              <span class="ml-4">{{ beautifyDatetime(slot.endTime) }}</span>
            </div>
            <div class="text-sm">
              <span class="font-bold">Địa điểm :</span>
              <span class="ml-4">{{ slot.teachAddress }}</span>
            </div>
            <div class="text-sm mt-1">
              <span class="font-bold">Hình thức :</span>
              <span
                :class="getMethodStyle(slot.isOnline ? 'Online' : 'Offline')"
              >
                {{ slot.isOnline ? "Online" : "Offline" }}
              </span>
            </div>
            <div class="text-sm">
              <span class="font-bold">Thời lượng :</span>
              <span class="ml-4">{{ calcDuration(slot) }} tiếng</span>
            </div>
          </div>
          <button
            @click="openSlotDetailPopup(slot)"
            class="p-1 bg-blue-300 font-bold text-white rounded-lg"
          >
            Xem chi tiết
          </button>
        </div>
      </div>
    </div>
    <generic-popup
      v-if="isOpenSlotDetailPopup"
      title="Chi tiết buổi học"
      :closeFunction="closeSlotDetailPopup"
      :notOverflow="true"
    >
      <slot-detail-popup :slot="selectingSlot" :close="closeSlotDetailPopup" />
    </generic-popup>
  </div>
</template>

<script>
import axios from "axios";
import TimeTable from "../StudentProfile/TimeTable.vue";
import GenericPopup from "../common/GenericPopup.vue";
import SlotDetailPopup from "./SlotDetailPopup.vue";

export default {
  components: {
    TimeTable,
    GenericPopup,
    SlotDetailPopup,
  },
  name: "ClassDetailPopup",
  props: ["classId", "close"],
  data() {
    return {
      class: null,
      slots: [],
      selectingSlot: null,
      isOpenSlotDetailPopup: false,
    };
  },
  methods: {
    handleClose() {
      this.close(0);
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
    calcDuration(slot) {
      const startTime = new Date(slot.startTime);
      const endTime = new Date(slot.endTime);
      return ((endTime - startTime) / 3600000).toFixed(2);
    },
    async getClassDetail() {
      const response = await axios.get(
        `${import.meta.env.VITE_API_URL}/api/Class/${this.classId}`,
        {
          headers: {
            Authorization: "Bearer " + localStorage.token,
          },
        }
      );
      if (response.data) {
        this.class = response.data;
      }
    },
    async getUserSlots() {
      const response = await axios.get(
        `${import.meta.env.VITE_API_URL}/api/Slot?Filter.ClassId=${
          this.classId
        }&Sorts[column]=startTime&Sorts[isDesc]=true`,
        {
          headers: {
            Authorization: "Bearer " + localStorage.token,
          },
        }
      );
      if (response.data.items) {
        this.slots = response.data.items;
      }
    },
    openSlotDetailPopup(slot) {
      this.selectingSlot = slot;
      this.isOpenSlotDetailPopup = true;
    },
    closeSlotDetailPopup() {
      this.isOpenSlotDetailPopup = false;
    },
    async refresh() {
      await this.getClassDetail();
      await this.getUserSlots();
    },
  },
  mounted() {
    this.refresh();
  },
};
</script>

<style></style>

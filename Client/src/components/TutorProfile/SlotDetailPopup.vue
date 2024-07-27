<template>
  <div class="p-4 bg-white rounded-b-lg w-full">
    <div class="flex gap-4">
      <div>
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
        <div class="mt-8">
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
        <hr />
        <div class="mt-4">
          <span class="font-bold">Địa điểm :</span>
          <span class="ml-4">{{ slot.teachAddress }}</span>
        </div>
        <div class="">
          <span class="font-bold">Phương thức :</span>
          <span v-if="slot.isOnline" class="ml-4 font-bold text-green-500"
            >Online</span
          >
          <span v-else class="ml-4 font-bold text-gray-500">Offline</span>
        </div>
      </div>
      <div>
        <img
          class="w-48 h-48"
          :src="
            slot.createdBy?.avatarImageUrl ?? '/src/assets/noavatar.jpg'
          "
        />
        <div class="mt-2 text-center">
          <div>Gia sư</div>
          <div class="font-bold text-2xl">
            {{
              (slot.createdBy?.firstName ?? "") +
              " " +
              (slot.createdBy?.lastName ?? "" )
            }}
          </div>
        </div>
        <div class="">
          <div>
            <span class="font-bold">Email : </span>
            <span class="italic">{{ slot.createdBy?.email }}</span>
          </div>
          <div>
            <span class="font-bold">Phone : </span>
            <span class="italic">{{ slot.createdBy?.phone }}</span>
          </div>
        </div>
      </div>
    </div>
    <hr />
    <div class="mt-4">
      <span class="font-bold">Học sinh đăng ký:</span>
      <ul class="mt-2">
        <li v-for="student in slotStudents" :key="student.userId">
          User ID: {{ student.userId }}, Feedback: {{ student.feedback }}
        </li>
      </ul>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "SlotDetailPopup",
  props: ["slot", "close"],
  data() {
    return {
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
            `${import.meta.env.VITE_API_URL}/api/SlotStudent/${this.slot.id}`,
            {
              headers: {
                Accept: "application/json",
                Authorization: `Bearer ${localStorage.token}`,
              },
            }
          );
          console.log("API response:", response.data);
          this.slotStudents = response.data;
          console.log("slotStudents updated:", this.slotStudents);
        } catch (error) {
          console.error("Error fetching slot students:", error);
        }
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

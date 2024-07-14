<!-- SlotDetail.vue -->
<template>
  <div>
    <button v-for="slot in slots" :key="slot.id" :class="slotClass(slot)" :style="slotStyle(slot)" @click="viewDetail(slot)">
      <div v-if="slotHeight(slot) > 45">
        <div v-if="slotHeight(slot) > 90 && slot.slot.class" class="font-bold">
          {{ slot.slot.class.name  }}
        </div>
        {{ formatTime(slot.slot.startTime) }}<br />
        {{ formatTime(slot.slot.endTime) }}

      </div>

    </button>
  </div>
</template>

<script>
export default {
  props: {
    slots: Array,
    shiftZoomSize: Number,
    getDistanceInMin: Function,
    viewDetail : Function
  },
  methods: {
    formatTime(dateStr) {
      const date = new Date(dateStr);
      const hours = date.getHours().toString().padStart(2, "0");
      const minutes = date.getMinutes().toString().padStart(2, "0");
      return `${hours}:${minutes}`;
    },
    slotClass(slot) {
      return `rounded-lg absolute w-full text-white text-center ${this.getSlotStyle(
        slot
      )}`;
    },
    slotStyle(slot) {
      const startTime = new Date(slot.slot.startTime);
      const endTime = new Date(slot.slot.endTime);
      const durationInHour = (endTime - startTime) / 3600000;
      const distanceInMin = this.getDistanceInMin(this.shiftZoomSize);
      const top =
        8 +
        ((startTime.getHours() + startTime.getMinutes() / 60) * 40 * 60) /
        distanceInMin;
      const height = (durationInHour * 40 * 60) / distanceInMin;
      return { top: `${top}px`, height: `${height}px` };
    },
    slotHeight(slot) {
      const startTime = new Date(slot.slot.startTime);
      const endTime = new Date(slot.slot.endTime);
      return (
        (((endTime - startTime) / 3600000) * 40 * 60) /
        this.getDistanceInMin(this.shiftZoomSize)
      );
    },
    getSlotStyle(slot) {
      let bg = "";
      if (slot.paymentStatus == 0 && this.compareDate(new Date(slot.slot.startTime), new Date()) < 0) {
        bg = "bg-red-400";
      } else if (slot.paymentStatus == 1 && this.compareDate(new Date(slot.slot.endTime), new Date()) < 0) {
        bg = "bg-green-400";
      } else if (slot.paymentStatus == 1) {
        bg = "bg-blue-400";
      } else {
        bg = "bg-gray-400";
      }
      return bg + " flex justify-center items-center";
    },
  },
};
</script>

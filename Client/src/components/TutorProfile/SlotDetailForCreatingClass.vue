<!-- SlotDetail.vue -->
<template>
  <div>
    <button @click="handleClickDay" class="absolute inset-0 hover:bg-slate-50" 
    :class="{'bg-slate-200' : day == dayPicked}"></button>
    <button v-for="slot in slots" :key="slot.id" class="rounded-lg absolute w-full text-white text-center bg-gray-400 flex justify-center items-center" 
      :class="{'bg-orange-400' : slot.isClass , 'shadow-xl shadow-blue-400 border-b-4 border-l-4 border-orange-800' : slot.isSelected}" 
      :disabled="!slot.isClass"
      :style="slotStyle(slot)"
      @click="handleSelect(slot)">
      <div class="absolute inset-0" v-if="slot.isOnline">
        <div class="relative">
          <div class="absolute bg-green-400 p-2 right-1 top-1 rounded-full"></div>
        </div>
      </div>
      <div class="absolute inset-0" v-if="slot.isClass">
        <div class="relative">
          <button @click="handleRemove(slot)" class="absolute text-red-500 text-3xl p-2 -right-2 -top-4">
            <i class="fa fa-remove"></i>
          </button>
        </div>
      </div>
      <div v-if="slotHeight(slot) > 45">
        <div v-if="slotHeight(slot) > 90 && slot.class" class="font-extrabold">
          {{ slot.class?.name }}
        </div>
        <div v-if="slotHeight(slot) > 90 && !slot.class" class="italic">
          {{ slot.subject?.name }}
        </div>
        {{ formatTime(slot.startTime) }}<br />
        {{ formatTime(slot.endTime) }}
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
    day : String,
    dayPicked : String,
    setPickedDay : Function,
  },
  inject : ['eventBus'],
  data(){
    return {
    }
  },
  methods: {
    formatTime(dateStr) {
      const date = new Date(dateStr);
      const hours = date.getHours().toString().padStart(2, "0");
      const minutes = date.getMinutes().toString().padStart(2, "0");
      return `${hours}:${minutes}`;
    },
    slotStyle(slot) {
      const startTime = new Date(slot.startTime);
      const endTime = new Date(slot.endTime);
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
      const startTime = new Date(slot.startTime);
      const endTime = new Date(slot.endTime);
      return (
        (((endTime - startTime) / 3600000) * 40 * 60) /
        this.getDistanceInMin(this.shiftZoomSize)
      );
    },
    handleClickDay(){
      this.setPickedDay(this.day)
    },
    handleRemove(slot){
      this.eventBus.emit("class-creator-remove-slot", {start : slot.startTime, end : slot.endTime})
    },
    handleSelect(slot){
      this.eventBus.emit("class-creator-select-slot", {slot : slot, isSelect : !slot.isSelected})
    }
  },

};
</script>
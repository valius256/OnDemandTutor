<template>
  <div class="max-w-md w-full px-6 py-8 bg-white rounded-lg shadow-md animate-jump-in">
    <div v-if='type == "Success"' class="flex justify-center text-green-500 ">
      <i class="fa fa-check text-7xl"></i>
    </div>
    <div v-if='type == "Error"' class="flex justify-center text-red-500 ">
      <i class="fa fa-remove text-7xl"></i>
    </div>
    <div v-if='type != "Success" && type != "Error"' class="flex justify-center text-blue-500 ">
      <i class="fa fa-information text-7xl"></i>

    </div>
    <h2 class="text-2xl font-semibold text-gray-800 mb-6 text-center">{{ this.message }}</h2>

    <div class="mt-8 flex justify-center">
      <button @click="handleClose" class="bg-blue-300 pl-12 pr-12 pt-4 pb-4 rounded-lg font-bold hover:bg-blue-500">Đóng</button>
    </div>
  </div>

</template>

<script>
//import { RouterLink } from 'vue-router';

export default {
  name: "ResultForm",
  inject: ["eventBus"],
  props: ['message', 'type','callback'],
  mounted() {

  },
  methods: {
    handleClose() {
      //console.log(this.callback)
      if (typeof this.callback === 'function') {
        this.callback();
      } 
      this.eventBus.emit("close-result-dialog")
    },
  }
}
</script>
<style></style>
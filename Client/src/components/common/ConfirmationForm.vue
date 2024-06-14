<template>
  <div class="max-w-md w-full px-6 py-8 bg-white rounded-lg shadow-md">
    <h2 class="text-2xl font-semibold text-gray-800 mb-6">{{this.message}}</h2>
    <div class="flex justify-center gap-20">
      <button @click="clickYes" class="bg-green-300 pl-12 pr-12 pt-4 pb-4 rounded-lg font-bold hover:bg-green-500">YES</button>
      <button @click="clickNo" class="bg-red-300 pl-12 pr-12 pt-4 pb-4 rounded-lg font-bold hover:bg-red-500">NO</button>

    </div>
  </div>

</template>

<script>
//import { RouterLink } from 'vue-router';

export default {
  name: "ConfirmationForm",
  inject: ["eventBus"],
  props : ['message','callback','params','method'],
  mounted() {

  },
  methods: {
    clickYes(){
      this.eventBus.emit("open-confirmation-popup","")
      this.eventBus.emit(this.callback,this.params)
      this.method(this.params)
    },
    clickNo(){
      this.eventBus.emit("open-confirmation-popup","")
    }
  }
}
</script>
<style></style>
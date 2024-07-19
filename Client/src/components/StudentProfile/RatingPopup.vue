<template>
  <div class="p-4 bg-white rounded-b-lg w-full">
    <div class="flex flex-col w-96">
        <div class="font-bold text-center">
            Đánh giá của bạn
        </div>
        <star-rating :rating="5" class="flex justify-center" @update:rating="rating = $event" />
    </div>
    <div class="flex flex-col w-96 mt-8">
        <div class="font-bold text-center">
            Vui lòng cho biết feedback<br> của bạn về {{ (slotId ? "buổi học" : "") + (classId ? "lớp học" : "") }} này
        </div>
        <textarea v-model="feedback" class="h-32 border rounded-lg"></textarea>
        <button class="mt-8 font-bold text-white bg-blue-400 rounded-lg py-2">Xác nhận</button>
    </div>
  </div>
</template>

<script>
import StarRating from 'vue-star-rating'
export default {
    name : "RatingPopup",
    props : ['slotId','classId'],
    components : {StarRating},
    data(){
        return {
            rating : 0,
            feedback : ""
        }
    }
}
</script>

<style>

</style>
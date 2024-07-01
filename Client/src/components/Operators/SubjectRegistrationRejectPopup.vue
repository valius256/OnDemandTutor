<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-y-auto max-h-[30rem] overflow-x-auto">
        <div>
            <span class="p-1 font-bold">Hãy cho biết vì sao bạn từ chối đơn đăng ký này</span><br>
            <textarea v-model="reason" class="p-1 border rounded-lg w-full" placeholder="Nhập lý do"/>
        </div>
        <div class="flex justify-center mt-4 gap-3">
            <button @click="handleFilter" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác
                nhận</button>
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>

    </div>
</template>

<script>
export default {
    name: "SubjectRegistrationRejectReason",
    props: ['close', 'tutorId'],
    data() {
        return {
            reason: "",
        }
    },
    methods: {

    },
}
</script>

<style></style>
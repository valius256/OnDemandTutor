<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-y-auto">
        <div class="flex flex-col gap-2 h-96 overflow-y-auto">
            <button v-for="bank in banks" :key="bank.id" class="hover:bg-gray-200 rounded-lg" @click="action(bank)">
                <div class="flex">
                    <img class="w-32" :src="bank.logo">
                    <span class="text-left">{{ bank.shortName }}</span>
                </div>
            </button>
        </div>

        <div class="flex justify-center mt-8 gap-3">
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>
    </div>
</template>

<script>
import axios from 'axios'
export default {
    name: "RequestWithdrawPopup",
    props: ['close', 'action'],
    data() {
        return {
            banks: []
        }
    },
    methods: {
        async fetchBank() {
            const response = await axios.get("https://api.vietqr.io/v2/banks")
            if (response.data) {
                this.banks = response.data.data
            }
        }
    },
    mounted() {
        this.fetchBank()
    }
}
</script>

<style></style>
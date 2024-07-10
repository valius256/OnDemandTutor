<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-y-auto max-h-[30rem] overflow-x-auto">
        <div class="flex">
            <span class="w-48 p-1 font-bold">Nhập số tiền muốn rút</span>
            <input v-model="amount" class="p-1 border rounded-lg" placeholder="Nhập từ khóa tìm kiếm" />
        </div>
        <div class="italic text-red-400 text-sm font-bold">* Số tiền rút tối thiểu 5.000đ và tối đa 1 tỷ đồng</div>
        <div class="flex mt-4">
            <span class="w-48 p-1 font-bold">Số tài khoản</span>
            <input v-model="bankAccountNumber" class="p-1 border rounded-lg" placeholder="Nhập số tài khoản" />
        </div>
        <div class="flex mt-4">
            <span class="w-48 p-1 font-bold">Ngân hàng</span>
            <button class="font-bold underline text-blue-400" @click="toggleBankPopup" v-if="!bank">Chọn ngân
                hàng</button>
            <button class="font-bold" @click="toggleBankPopup" v-if="bank">
                <img class="w-32" :src="bank.logo">
                <div>{{ bank.shortName }}</div>
            </button>
        </div>
        <div class="flex mt-4">
            <span class="w-48 p-1 font-bold">Lý do rút tiền</span>
            <textarea v-model="reason" class="p-1 border rounded-lg " placeholder="Nhập lý do" />
        </div>
        <div class="flex justify-center mt-4 gap-3">
            <button @click="createWithdrawRequest(true)" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác
                nhận</button>
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>
        <generic-popup :title="'Chọn ngân hàng'" :closeFunction="toggleBankPopup" v-if="isOpenBankPopup">
            <bank-selector-popup :close="toggleBankPopup" :action="handleSelectBank"></bank-selector-popup>
        </generic-popup>
    </div>
</template>

<script>
import axios from 'axios'
import GenericPopup from '../common/GenericPopup.vue'
import BankSelectorPopup from '../common/BankSelectorPopup.vue'
export default {
    components: { GenericPopup, BankSelectorPopup },
    name: "RequestWithdrawPopup",
    inject: ['eventBus'],
    props: ['close', 'action'],
    data() {
        return {
            amount: 0,
            bankAccountNumber: "",
            bank: "",
            reason: "",
            isOpenBankPopup: false
        }
    },
    methods: {
        toggleBankPopup() {
            this.isOpenBankPopup = !this.isOpenBankPopup
        },
        handleSelectBank(bank) {
            if (bank != null) {
                this.bank = bank
                this.isOpenBankPopup = false;
            }
        },
        async createWithdrawRequest(confirmation) {
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn tạo yêu cầu rút tiền?",
                    method: this.createWithdrawRequest,
                    params: false
                })
            } else {
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    await axios.post(import.meta.env.VITE_API_URL + '/api/RequestWithDraw/create-withdraw', {
                        amount : this.amount,
                        bankAccountNumber : this.bankAccountNumber,
                        bankName : this.bank.shortName,
                        description : this.reason
                    }, {
                        'headers' : {
                            'Authorization' : "Bearer " + localStorage.token
                        }
                    })
                    this.isOpenBankPopup = false;
                    this.close()
                    this.action()
                    this.eventBus.emit("open-result-dialog", {
                        message: "Gửi yêu cầu thành công",
                        type: "Success"
                    })
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Đã xảy ra sự cố vui lòng thử lại sau.",
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        },
    },
    mounted() {
    }
}
</script>

<style></style>
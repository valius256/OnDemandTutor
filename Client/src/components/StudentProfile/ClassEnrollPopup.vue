<template>
    <div class="p-4 bg-white rounded-b-lg w-full" v-if="this.class">
        <div class="flex flex-col w-96">
            <div class="font-semibold text-center">
                Bạn đang chuẩn bị tham gia lớp
            </div>
            <div class="font-bold text-center text-2xl">
                {{ this.class.name }}
            </div>
            <div class="mt-4 italic text-center">
                Theo quy định, bạn sẽ phải cọc trước 20% tổng thời lượng các slot nhân với giá dịch vụ theo giờ của gia
                sư.
            </div>
            <div class="mt-2 italic text-center font-bold">
                Để tham gia lớp này bạn phải trả cọc
            </div>
            <div class="mt-2 italic text-center font-bold text-4xl text-red-500">
                {{ calcMoney().toLocaleString('vi-VN', {
        style: 'currency',
        currency: 'VND',
    }) }}
            </div>
            <div class="flex flex-col mt-2 w-full">
                <hr>
                <div class="italic text-center">
                    Vui lòng chọn phương thức thanh toán
                </div>
                <hr>
                <div class="flex flex-col gap-2 mt-4">
                    <div class="flex gap-4">
                        <input type="radio" v-model="paymentMethod" :value="0">
                        <span class="text-center">Trừ trực tiếp số dư</span>
                        <span class="text-center text-green-400 font-bold">({{ balance.toLocaleString('vi-VN', {
        style: 'currency',
        currency: 'VND',
                            }) }})</span>
                    </div>
                    <div class="flex gap-4">
                        <input type="radio" v-model="paymentMethod" :value="1">
                        <span class="text-center">Thanh toán bằng VnPay</span>
                    </div>
                </div>
            </div>
        </div>
        <div class="flex flex-col w-96 mt-4">
            <button class="mt-8 font-bold text-white bg-blue-400 hover:bg-blue-200 rounded-lg py-2" @click="handleConfirm">Xác nhận</button>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
export default {
    name: "ClassEnrollPopup",
    inject: ['eventBus'],
    props: ['classId'],
    data() {
        return {
            paymentMethod: 0,
            class: null,
            balance: 0,

        }
    },
    methods: {
        calcMoney() {
            var money = 0;
            for (var slot of this.class.slots) {
                money += this.calcDuration(slot) * this.class.tutor.tutorFeePerHour * 20 / 100
            }
            return money;
        },
        calcDuration(slot) {
            const startTime = new Date(slot.startTime);
            const endTime = new Date(slot.endTime);
            return (endTime - startTime) / 3600000;
        },
        async getClassDetail() {
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/Class/' + this.classId, {
                headers: {
                    'Authorization': "Bearer " + localStorage.token
                }
            })
            if (response.data) {
                this.class = response.data
            }

            const balanceResponse = await axios.get(import.meta.env.VITE_API_URL + '/api/User/balance', {
                headers: {
                    'Authorization': "Bearer " + localStorage.token
                }
            })
            if (balanceResponse.data) {
                this.balance = balanceResponse.data.data.balance
            }
        },
        async handleVnpay(confirmation) {
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn thanh toán bằng VnPay?",
                    method: this.handleVnpay,
                    params: false
                })
            } else {
                const request = {
                    orderDescription: "Tiền cọc cho lớp " + this.classId,
                    classId: this.classId,
                    isFullPay: false,
                    returnPage: import.meta.env.VITE_FE_URL + "/student/myclass"
                }
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    const url = await axios.post(import.meta.env.VITE_API_URL + '/api/Payment/create-payment-class', request,{
                        headers: {
                            'Authorization': "Bearer " + localStorage.token
                        }
                    })
                    var paymentUrl = url.data
                    window.location.href = paymentUrl
                    this.eventBus.emit("open-result-dialog", {
                        message: "Đang điều hướng bạn qua trang thanh toán",
                        type: "Success"
                    })
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Có vấn đề xảy ra khi gửi yêu cầu",
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        },
        async handleConfirm(){
            //console.log(this.paymentMethod)
            if (this.paymentMethod == 1){
                await this.handleVnpay(true)
            }
        }
    },
    mounted() {
        this.getClassDetail()
        console.log(this.class)
    }
}
</script>

<style></style>
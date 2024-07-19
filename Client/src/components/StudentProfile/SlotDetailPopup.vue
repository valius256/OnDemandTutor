<template>
    <div class="p-4 bg-white rounded-b-lg w-full">
        <div class="flex gap-4">
            <div>
                <div class="mb-8" v-if="slot.slot.class">
                    <div>
                        <span class="font-bold">Tên lớp :</span>
                        <span class="ml-4">{{ slot.slot.class.name }}</span>
                    </div>
                    <hr>
                </div>
                <div>
                    <span class="font-bold">Môn học :</span>
                    <span class="font-bold text-blue-400 ml-4">{{ this.slot.slot.subject.name }}</span>
                </div>
                <hr>
                <div class="mt-8">
                    <span class="font-bold">Bắt đầu :</span>
                    <span class="ml-4">{{ this.beautifyDatetime(this.slot.slot.startTime) }}</span>
                </div>
                <div>
                    <span class="font-bold">Kết thúc :</span>
                    <span class="ml-4">{{ this.beautifyDatetime(this.slot.slot.endTime) }}</span>
                </div>
                <div>
                    <span class="font-bold">Tổng thời lượng :</span>
                    <span class="ml-4">{{ calcDuration() }} tiếng</span>
                </div>
                <hr>
                <div class="mt-4">
                    <span class="font-bold">Địa điểm :</span>
                    <span class="ml-4">{{ this.slot.slot.teachAddress }}</span>
                </div>
                <div class="">
                    <span class="font-bold">Phương thức :</span>
                    <span v-if="this.slot.slot.isOnline" class="ml-4 font-bold text-green-500">Online</span>
                    <span v-else class="ml-4 font-bold text-gray-500">Offline</span>
                </div>
            </div>
            <div>
                <img class="w-48 h-48" :src="this.slot.slot.createdBy.avatarImageUrl ?? '/src/assets/noavatar.jpg'">
                <div class="mt-2 text-center">
                    <div>Gia sư</div>
                    <div class="font-bold text-2xl">
                        {{ (this.slot.slot.createdBy.firstName ?? "") + " " + (this.slot.slot.createdBy.lastName ?? "")
                        }}
                    </div>
                </div>

                <div class="">
                    <div>
                        <span class="font-bold">Email : </span>
                        <span class="italic">{{ this.slot.slot.createdBy.email }}</span>
                    </div>
                    <div>
                        <span class="font-bold">Phone : </span>
                        <span class="italoc">{{ this.slot.slot.createdBy.phone }}</span>
                    </div>
                </div>
            </div>
        </div>
        <div v-if="!isAboutToPay">
            <div v-if="slot.paymentStatus == 0" class="mt-4 flex place-content-between">
                <div>
                    <span class="p-2 text-red-400 font-bold ">Bạn chưa thanh toán Slot này : </span>
                    <span class="p-2 text-red-500 font-bold text-2xl">{{ (calcDuration() *
                    slot.slot.createdBy.tutorFeePerHour).toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                    }) }}</span>
                </div>
                <button @click="isAboutToPay = true"
                    class="p-2 rounded-lg bg-blue-400 hover:bg-blue-200 font-bold text-white">Thanh toán
                    ngay</button>
            </div>
            <div v-if="slot.paymentStatus == 1" class="mt-4">
                <span class="p-2 text-blue-400 font-bold ">Bạn đã thanh toán Slot này</span>
            </div>
        </div>

        <div v-else class="flex flex-col mt-2 w-full">
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
                <button @click="handlePay"
                    class="p-2 rounded-lg bg-blue-400 hover:bg-blue-200 font-bold text-white">Thanh toán ngay</button>
            </div>
        </div>
        <div class="flex flex-col justify-center mt-2"
            v-if="new Date(slot.slot.endTime) < new Date() && !slot.slot.class && !slot.rating && slot.paymentStatus != -1">
            <div class="text-sm italic text-center">Bạn đã hoàn tất buổi học này. Hãy để lại feedback về gia sư nhé!
            </div>
            <button class="bg-cyan-600 hover:bg-cyan-400 text-white font-bold p-2 rounded-lg"
                @click="toggleIsOpenRatingPopup">
                Đánh giá gia sư
            </button>

        </div>
        <generic-popup v-if="isOpenRatingPopup" title="Đánh giá slot học" :closeFunction="toggleIsOpenRatingPopup">
            <rating-popup :slotId="slot.id"></rating-popup>
        </generic-popup>
    </div>
</template>

<script>
import axios from 'axios';
import GenericPopup from '../common/GenericPopup.vue';
import RatingPopup from './RatingPopup.vue';
export default {
    name: "SlotDetailPopup",
    components: { GenericPopup, RatingPopup },
    inject: ['eventBus'],
    props: ['slot', 'close','action'],
    data() {
        return {
            isOpenRatingPopup: false,
            balance: 0,
            isAboutToPay: false,
            paymentMethod: 0
        }
    },
    methods: {
        calcDuration() {
            const startTime = new Date(this.slot.slot.startTime);
            const endTime = new Date(this.slot.slot.endTime);
            return (endTime - startTime) / 3600000;
        },
        toggleIsOpenRatingPopup() {
            this.isOpenRatingPopup = !this.isOpenRatingPopup
        },
        async fetchBalance() {
            const balanceResponse = await axios.get(import.meta.env.VITE_API_URL + '/api/User/balance', {
                headers: {
                    'Authorization': "Bearer " + localStorage.token
                }
            })
            if (balanceResponse.data) {
                this.balance = balanceResponse.data.data.balance
            }
        },
        async handleDeductBalance(confirmation) {
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn thanh toán bằng số dư tài khoản?",
                    method: this.handleDeductBalance,
                    params: false
                })
            } else {
                const request = {
                    orderDescription: "Thanh toán cho slot " + this.slot.slot.id,
                    slotId: this.slot.slot.id,
                    returnUrl: import.meta.env.VITE_FE_URL + "/student/schedule"
                }
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    const url = await axios.post(import.meta.env.VITE_API_URL + '/api/Payment/create-payment-slot-user-balance', request, {
                        headers: {
                            'Authorization': "Bearer " + localStorage.token
                        }
                    })
                    this.action()
                    this.close()
                    //var paymentUrl = url.data
                    //window.location.href = paymentUrl
                    this.eventBus.emit("open-result-dialog", {
                        message: "Thanh toán thành công",
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
        async handleVnpay(confirmation) {
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn thanh toán bằng VnPay?",
                    method: this.handleVnpay,
                    params: false
                })
            } else {
                const request = {
                    orderDescription: "Thanh toán cho slot " + this.slot.slot.id,
                    slotId: this.slot.slot.id,
                    returnUrl: import.meta.env.VITE_FE_URL + "/student/schedule"
                }
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    const url = await axios.post(import.meta.env.VITE_API_URL + '/api/Payment/create-payment-slot', request, {
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
        async handlePay() {
            if (this.paymentMethod == 0) {
                await this.handleDeductBalance(true)
            }
            if (this.paymentMethod == 1) {
                await this.handleVnpay(true)
            }
        }
    },
    mounted() {
        this.fetchBalance()
    }
}
</script>

<style></style>
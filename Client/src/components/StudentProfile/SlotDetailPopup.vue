<template>
    <div class="p-4 bg-white rounded-b-lg w-full" v-if="slotData">
        <div class="flex gap-4">
            <div>
                <div class="mb-8" v-if="slot.slot.class">
                    <div>
                        <span class="font-bold">Tên lớp :</span>
                        <span class="ml-4">{{ slot.slot.class.name }}</span>
                    </div>
                    <hr>
                </div>
                <div class="flex place-content-between">
                    <div>
                        <span class="font-bold">Môn học :</span>
                        <span class="font-bold text-blue-400 ml-4">
                            {{ slot.slot.subject?.name }}
                        </span>
                    </div>
                    <div>
                        <span :class="getSlotStyle(slot.slot).style">
                            {{ getSlotStyle(slot.slot).display }}
                        </span>
                    </div>
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
                <div class="">
                    <span class="font-bold">Số học sinh :</span>
                    <span class="ml-4">{{ totalStudent }} / {{ this.slot.slot.numberOfStudents }}</span>
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
            <div class="italic text-center text-sm">
                Bạn cần trả trước để đặt gia sư
            </div>
            <span class="text-center text-red-500 font-bold text-xl">
                {{ (calcDuration() * slot.slot.createdBy.tutorFeePerHour).toLocaleString('vi-VN', {
        style: 'currency',
        currency: 'VND',
    }) }}
            </span>
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
                    class="p-2 rounded-lg bg-blue-400 hover:bg-blue-200 font-bold text-white">Thanh toán
                    ngay</button>
            </div>
        </div>
        <button @click="handleLeave(true)"
            v-if="!isAboutToPay && this.compareDate(new Date(slot.slot.startTime), new Date()) > 0 && !slot.slot.class && !this.checkEnrollSlot()"
            class="w-full p-2 rounded-lg bg-red-500 hover:bg-red-300 font-bold text-white">
            Rời buổi học
        </button>
        <button @click="isAboutToPay = true"
            v-if="!isAboutToPay && this.compareDate(new Date(slot.slot.startTime), new Date()) > 0 && slot.paymentStatus == -2 && !slot.slot.class && this.checkEnrollSlot()"
            class="w-full p-2 rounded-lg bg-blue-400 hover:bg-blue-200 font-bold text-white">
            Đặt ngay
        </button>
        <div class="flex flex-col justify-center mt-2"
            v-if="new Date(slot.slot.endTime) < new Date() && !slot.slot.class && !slot.rating && slot.paymentStatus >= 0">
            <div class="text-sm italic text-center">Bạn đã hoàn tất buổi học này. Hãy để lại feedback về gia sư nhé!
            </div>
            <button v-if="(!slot.rating || !slot.feedback)"
                class="bg-cyan-600 hover:bg-cyan-400 text-white font-bold p-2 rounded-lg"
                @click="toggleIsOpenRatingPopup">
                Đánh giá gia sư
            </button>

        </div>
        <generic-popup v-if="isOpenRatingPopup" title="Đánh giá slot học" :closeFunction="toggleIsOpenRatingPopup">
            <rating-popup :slotId="slot.slot.id" :action="action" :close="close"></rating-popup>
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
    props: ['slot', 'close', 'action'],
    data() {
        return {
            isOpenRatingPopup: false,
            balance: 0,
            isAboutToPay: false,
            paymentMethod: 0,
            slotData: null,
            currentUser: null,
            totalStudent: 0,
        }
    },
    methods: {
        async fetchSlotStudents() {
            if (this.slot.slot.id) {
                try {
                    const response = await axios.get(
                        `${import.meta.env.VITE_API_URL}/api/SlotStudent/${this.slot.slot.id}`
                    );
                    this.totalStudent = response.data.total
                } catch (error) {
                    console.error("Error fetching slot students:", error);
                }
            }
        },
        calcDuration() {
            const startTime = new Date(this.slot.slot.startTime);
            const endTime = new Date(this.slot.slot.endTime);
            return (endTime - startTime) / 3600000;
        },
        toggleIsOpenRatingPopup() {
            this.isOpenRatingPopup = !this.isOpenRatingPopup
        },
        async fetchBalance() {
            var user = await this.getUserFromToken()
            if (user != null) {
                const balanceResponse = await axios.get(import.meta.env.VITE_API_URL + '/api/User/balance', {
                    headers: {
                        'Authorization': "Bearer " + localStorage.token
                    }
                })
                if (balanceResponse.data) {
                    this.balance = balanceResponse.data.data.balance
                }
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
                    let message = "Có vấn đề xảy ra khi gửi yêu cầu"
                    var errorMessage = e.response.data?.errors[0]?.errorMessage
                    if (errorMessage && errorMessage.includes("conflict")) {
                        message = "Bạn không thể tham gia buổi học này do bị trùng lịch với 1 buổi học khác. Hãy thử liên hệ các gia sư để có thể sắp xếp lịch học hợp lý"
                    }
                    if (errorMessage && errorMessage.includes("Balance")) {
                        message = "Dell đủ số dư"
                    }
                    this.eventBus.emit("open-result-dialog", {
                        message: message,
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
                    let message = "Có vấn đề xảy ra khi gửi yêu cầu"
                    var errorMessage = e.response.data?.errors[0]?.errorMessage
                    if (errorMessage && errorMessage.includes("conflict")) {
                        message = "Bạn không thể tham gia buổi học này do bị trùng lịch với 1 buổi học khác. Hãy thử liên hệ các gia sư để có thể sắp xếp lịch học hợp lý"
                    }
                    if (errorMessage && errorMessage.includes("Balance")) {
                        message = "Dell đủ số dư"
                    }
                    this.eventBus.emit("open-result-dialog", {
                        message: message,
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        },
        async handlePay() {
            var user = await this.getUserFromToken()
            if (user == null) {
                this.eventBus.emit("open-result-dialog", {
                    message: "Vui lòng đăng nhập trước khi thanh toán",
                    type: "Information"
                })
                this.$router.push("/login")
                return;
            }
            if (this.paymentMethod == 0) {
                await this.handleDeductBalance(true)
            }
            if (this.paymentMethod == 1) {
                await this.handleVnpay(true)
            }
        },
        async getSlotData() {
            console.log(this.slot)
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/Slot/' + this.slot.slot.id, {
                headers: {
                    'Authorization': "Bearer " + localStorage.token
                }
            })
            if (response.data) {
                this.slotData = response.data
            }
        },
        checkEnrollSlot() {
            return this.slotData.slotStudents.find(s => s.userId == this.currentUser.id) == null
        },
        async getCurrentUser() {
            this.currentUser = await this.getUserFromToken()
        },
        getSlotStyle(slot) {
            let bg = "font-bold ";
            let display = "";
            if (slot.slotStatus == 0) {
                bg += "text-gray-300";
                display = "Sắp diễn ra"
            } else if (slot.slotStatus == 1) {
                bg += "text-green-400";
                display = "Đang diễn ra"
            } else if (slot.slotStatus == 2) {
                bg += "text-black";
                display = "Đã hủy"
            } else if (slot.slotStatus == 3) {
                bg += "text-blue-400";
                display = "Đã hoàn tất"
            }
            return {
                style: bg, display: display
            };
        },
        async handleLeave(confirmation) {
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: "Bạn có chắc chắn muốn rời buổi học này?" + (this.slot.slot.slotStatus == 2 ? 
                    " Bạn sẽ được hoàn tiền vì gia sư đã chủ động hủy buổi học này" : "Bạn sẽ không được hoàn tiền."),
                    method: this.handleLeave,
                    params: false
                })
            } else {
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    await axios.put(import.meta.env.VITE_API_URL + '/api/SlotStudent/' + this.slot.slot.id + '/leave',{
                    }, {
                        headers: {
                            "Authorization": "Bearer " + localStorage.token
                        }
                    })
                    this.eventBus.emit("open-result-dialog", {
                        message: "Rời buổi học thành công",
                        type: "Success"
                    })
                    await this.action()
                    this.close()
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Không thể thực hiện. Vui lòng thử lại sau",
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        }
    },
    mounted() {
        this.fetchBalance()
        this.getCurrentUser()
        this.getSlotData()
        this.fetchSlotStudents()
    }
}
</script>

<style></style>
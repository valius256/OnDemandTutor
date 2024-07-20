<template>
    <div class="w-full -ml-4 relative min-h-screen" v-if="tutor">
        <div class="px-4 pb-32 bg-slate-300">
            <div class="flex flex-wrap lg:place-content-between pt-2">
                <div class="flex gap-2">
                    <button @click="$router.go(-1)"
                        class="p-2 bg-blue-300 hover:bg-blue-100 font-bold text-white rounded-lg">Trở về</button>
                    <router-link to="/tutor/profile">
                        <div class="p-2 bg-cyan-500 hover:bg-cyan-300 font-bold text-white rounded-lg">Xem hồ sơ đầy đủ
                        </div>
                    </router-link>

                </div>

            </div>
        </div>

        <div class="ml-8 -mt-24 flex gap-4">
            <img class="w-48 h-48 rounded-full " :src="tutor.user.avatarImageUrl" />
            <div class="mt-8">
                <div class="font-bold text-4xl  ">{{ (tutor.user.firstName ?? "") + " " + (tutor.user.lastName ?? "") }}</div>
                <div class="text-lg  mt-8">{{ tutor.user.address }}</div>
            </div>

        </div>
        <div class="ml-6 flex flex-col lg:flex-row gap-4">
            <div class="w-full lg:w-1/3 border-r-2">
                <div class="font-bold text-xl border-b-2">Thông tin cá nhân</div>
                <div class="mt-4 flex flex-col gap-4">
                    <div>
                        <span class="font-bold">Tiểu sử : </span>
                        <span class="italic">{{ tutor.user.bio }}</span>
                    </div>
                    <div>
                        <span class="font-bold">Ngày sinh : </span>
                        <span>{{ tutor.user.dob?.substring(0,10) ?? "" }}</span>
                    </div>
                    <div>
                        <span class="font-bold">Tuối tác : </span>
                        <span>{{ new Date().getFullYear() - new Date(tutor.user.dob).getFullYear() }}</span><br>
                    </div>
                    <div>
                        <span class="font-bold">Địa chỉ : </span>
                        <span>{{ tutor.user.address }}</span>
                    </div>
                    <div>
                        <span class="font-bold">Giá dịch vụ (VND/h) : </span>
                        <span>{{ (tutor.user.tutorFeePerHour.toLocaleString('vi-VN', {
                            style: 'currency',
                            currency: 'VND',
                        })) }}
                        </span>
                    </div>
                    <div class="flex gap-4 ">
                        <span class="font-bold">Đánh giá : </span>
                        <span>
                            <star-rating :rating="tutor.user.rating" :round-start-rating="false" :read-only="true"
                                :star-size="20" />
                        </span>
                    </div>
                    <div>
                        <span class="font-bold">Mô tả khác : </span>
                        <span>{{ tutor.user.scheduleDesciption }}</span>
                    </div>
                </div>
            </div>
            <div class="w-full lg:w-2/3">
                <div class="flex gap-4">
                    <div class="font-bold text-2xl">Đăng ký môn : </div>
                    <div class="font-bold text-2xl text-green-400">{{ tutor.subject.name }}</div>
                </div>
                <div class="mt-8">
                    <div class="mb-8">
                        <div class="font-bold text-xl">
                            Mô tả kinh nghiệm trong môn học
                        </div>
                        <div class="italic">
                            {{ tutor.otherDescription }}
                        </div>
                    </div>

                    <div v-for="degree in tutor.degrees" class="" :key="degree.id">
                        <div class="font-bold text-xl">
                            {{ degree.name }}
                        </div>
                        <div class="mt-2 w-5/6">
                            <img :src="degree.degreeImgUrl" />
                        </div>
                        <div class="flex flex-col gap-2 mb-8 mt-4">
                            <div>
                                <span class="font-bold">Số bằng</span>
                                <span class="ml-4">{{ degree.degreeNumber }}</span>
                            </div>
                            <div>
                                <span class="font-bold">Ngày cấp</span>
                                <span class="ml-4">{{ degree.issuranceDate }}</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="flex justify-center py-4 bg-slate-400 sticky bottom-0 bg-opacity-50">
            <div class="flex gap-4">
                <button @click="handleApprove(true)"
                    class="py-2 px-12 bg-green-500 hover:bg-green-300 font-bold text-white rounded-lg">Duyệt</button>
                <button @click="toggleRejectPopup"
                    class="py-2 px-12 bg-red-500 hover:bg-red-300 font-bold text-white rounded-lg">Từ chối</button>
            </div>
        </div>

        <generic-popup v-if="isOpenRejectPopup" :closeFunction="toggleRejectPopup" title="Lý do từ chối">
            <subject-registration-reject-popup :close="toggleRejectPopup" :tutorId="tutor.user.id" :id="tutor.id" :subjectId="tutor.subject.id" />
        </generic-popup>
    </div>
</template>

<script>
import axios from 'axios'
import StarRating from 'vue-star-rating'
import GenericPopup from '../../components/common/GenericPopup.vue'
import SubjectRegistrationRejectPopup from '../../components/Operators/SubjectRegistrationRejectPopup.vue'

export default {
    name: "SubjectRegistrationDetailPage",
    inject : ['eventBus'],
    components: { StarRating, GenericPopup, SubjectRegistrationRejectPopup },
    data() {
        return {
            tutor: null,
            isOpenRejectPopup: false,
        }
    },
    methods: {
        toggleRejectPopup() {
            this.isOpenRejectPopup = !this.isOpenRejectPopup
        },
        async fetchData() {
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/TutorSubject/' +
                this.$route.params.id)
            if (response.data) {
                this.tutor = response.data
            }
        },
        async handleApprove(confirmation) {
            if (confirmation) {
                this.eventBus.emit("open-confirmation-popup", {
                    message: `Bạn có chắc chắn muốn duyệt đơn đăng ký này?`,
                    method: this.handleApprove,
                    params: false
                })
            } else {
                this.eventBus.emit("open-loading-popup", {
                    message: "Vui lòng chờ..."
                })
                try {
                    const request = {
                        id: this.tutor.id,
                        userId : this.tutor.user.id,
                        subjectId : this.tutor.subject.id,
                        status : 3
                    }
                    await axios.put(import.meta.env.VITE_API_URL + '/api/TutorSubject/' + this.tutor.id, request, {
                        headers: {
                            "Authorization": "Bearer " + localStorage.token
                        }
                    })
                    this.eventBus.emit("open-result-dialog", {
                        message: "Cập nhật thành công",
                        type: "Success"
                    })
                    this.$router.push("/admin/subjects/registration")
                } catch (e) {
                    console.log(e)
                    this.eventBus.emit("open-result-dialog", {
                        message: "Đã gặp sự cố. Vui lòng thử lại sau",
                        type: "Error"
                    })
                }
                this.eventBus.emit("close-loading-popup")
            }
        }
    },
    mounted() {
        this.fetchData()
    }
}
</script>

<style></style>
<template>
    <div class="w-full -ml-4" v-if="tutor">
        <div class="px-4 pb-32 bg-slate-300">
            <div class="flex flex-wrap lg:place-content-between pt-2">
                <div class="flex gap-2">
                    <button @click="$router.go(-1)" class="p-2 bg-blue-300 hover:bg-blue-100 font-bold text-white rounded-lg">Trở về</button>
                    <router-link to="/tutor/profile">
                        <div class="p-2 bg-cyan-500 hover:bg-cyan-300 font-bold text-white rounded-lg">Xem hồ sơ đầy đủ</div>
                    </router-link>
                    
                </div>
                <div class="flex gap-2">
                    <button class="py-2 px-6 bg-green-500 hover:bg-green-300 font-bold text-white rounded-lg">Duyệt</button>
                    <button @click="toggleRejectPopup" class="py-2 px-6 bg-red-500 hover:bg-red-300 font-bold text-white rounded-lg">Từ chối</button>
                </div>
            </div>
        </div>

        <div class="ml-8 -mt-24 flex gap-4">
            <img class="w-48 h-48 rounded-full " :src="tutor.user.avatar" />
            <div class="mt-8">
                <div class="font-bold text-4xl  ">{{ tutor.user.name }}</div>
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
                        <span>{{ tutor.user.dob }}</span>
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
                        <span>{{ (tutor.user.price.toLocaleString('vi-VN', {
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
                        <span>{{ tutor.user.otherDescription }}</span>
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
                            <img :src="degree.degreeImageUrl" />
                        </div>
                        <div class="flex flex-col gap-2 mb-8 mt-4">
                            <div>
                                <span class="font-bold">Số bằng</span>
                                <span class="ml-4">{{ degree.number }}</span>
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
        <generic-popup v-if="isOpenRejectPopup" :closeFunction="toggleRejectPopup" title="Lý do từ chối">
            <subject-registration-reject-popup :close="toggleRejectPopup" :tutorId="tutor.id"/>
        </generic-popup>
    </div>
</template>

<script>
import StarRating from 'vue-star-rating'
import GenericPopup from '../../components/common/GenericPopup.vue'
import SubjectRegistrationRejectPopup from '../../components/Operators/SubjectRegistrationRejectPopup.vue'

export default {
    name: "SubjectRegistrationDetailPage",
    components: { StarRating,GenericPopup, SubjectRegistrationRejectPopup },
    data() {
        return {
            tutor: {
                id : 1,
                user: {
                    name: "Nguyễn Văn A",
                    phone: "0987654321",
                    email: "abc@gmail.com",
                    avatar: "/src/assets/noavatar.jpg",
                    address: "Long Thạnh Mỹ, Q9, TPHCM",
                    dob: "2000-01-01",
                    bio: "Mot cay lam chang nen non. Ba cay chum lai len hon nui cao",
                    otherDescription: "Kinh nghiệm trong lĩnh vực gia sư 10 năm",
                    rating: 4.5,
                    price: 100000
                },
                teachingSubject: [
                    {
                        id: 1,
                        name: "Toán"
                    },
                    {
                        id: 2,
                        name: "Tiếng Việt"
                    }
                ],
                subject: {
                    name: "Hóa học"
                },
                otherDescription: "some thing here",
                degrees: [
                    {
                        id: 1,
                        name: "Bằng đại học",
                        degreeImageUrl: "https://m.media-amazon.com/images/I/91P-zQwO+-L.jpg",
                        number: 12345,
                        issuranceDate: "2024-01-01"
                    },
                    {
                        id: 2,
                        name: "Bằng tiến sĩ hóa học",
                        degreeImageUrl: "https://i.fbcd.co/products/resized/resized-750-500/1-a13569940017b5386bde69bb118cb096e1cbfbf17aec2fa75856394b5384f18b.jpg",
                        number: 12345,
                        issuranceDate: "2024-01-01"
                    }
                ]
            },
            isOpenRejectPopup : false,
        }
    },
    methods : {
        toggleRejectPopup(){
            this.isOpenRejectPopup = !this.isOpenRejectPopup
        },
    }
}
</script>

<style></style>
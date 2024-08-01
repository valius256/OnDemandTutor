<template>
  <div v-if="user">
    <div class="p-4 bg-slate-700 text-2xl font-bold text-white flex flex-col lg:flex-row lg:place-content-between"
      v-if="user.tutorStatus == 1 && currentUser.role > 1">
      <div>Tài khoản gia sư này cần được xác thực</div>
      <div class="flex gap-3">
        <button @click="handleApprove(true)" class="bg-green-400 hover:bg-green-200 text-white font-bold px-4 py-2 rounded-lg">Xác thực</button>
        <button @click="handleApprove(false)" class="bg-red-400 hover:bg-red-200 text-white font-bold px-4 py-2 rounded-lg">Từ chối</button>
      </div>
    </div>
    <div class="py-8 bg-slate-200 flex justify-center gap-8">
      <img :src="user.avatarImageUrl" class="w-48">
      <div class="flex flex-col justify-end">
        <div class="font-bold text-4xl">{{ (user.firstName ?? "") + " " + (user.lastName ?? "") }}</div>
        <Navbar :viewingId="viewingId"></Navbar>
      </div>

    </div>
    <div class="w-full">
      <profile :id="viewingId" :tutor="user" v-if="$route.path == '/tutor-guest/' + viewingId + '/profile'"></profile>
      <schedule :id="viewingId" :tutor="user" v-if="$route.path == '/tutor-guest/' + viewingId + '/schedule'">
      </schedule>
      <subject :id="viewingId" :tutor="user" v-if="$route.path == '/tutor-guest/' + viewingId + '/subject'"></subject>
      <video-list :id="viewingId" :tutor="user" v-if="$route.path == '/tutor-guest/' + viewingId + '/videos'"></video-list>
      <!-- <tutor-classes :id="viewingId"
          v-if="$route.path == '/tutor/myclass' + viewingId"
        ></tutor-classes> -->
    </div>
  </div>
</template>

<script>
import axios from "axios";
import Navbar from "../../components/TutorGuestView/Navbar.vue";
import Profile from "../../components/TutorGuestView/Profile.vue";
import Schedule from "../../components/TutorGuestView/Schedule.vue";
import Subject from "../../components/TutorGuestView/Subject.vue";
import TutorClasses from "../../components/TutorProfile/TutorClasses.vue";
import VideoList from '../../components/TutorGuestView/VideoList.vue';
export default {
  name: "ProfilePage",
  inject : ['eventBus'],
  components: {
    Profile,
    Schedule,
    Navigator,
    Subject,
    TutorClasses,
    Navbar,
    VideoList,
  },
  data() {
    return {
      user: null,
      viewingId: 0,
      currentUser: null,
    };
  },
  methods: {
    async refresh() {
      this.viewingId = this.$route.params.id
      const response = await axios.get(
        import.meta.env.VITE_API_URL + "/api/User/profile?userId=" + this.viewingId)

      this.user = response.data.data
      this.currentUser = await this.getUserFromToken()
    },
    async handleApprove(isApprove) {
      this.eventBus.emit("open-loading-popup", {
        message: "Vui lòng chờ..."
      })
      try {
        await axios.patch(import.meta.env.VITE_API_URL + '/api/User/change-status', {
          id: this.viewingId,
          status: isApprove ? 3 : 2
        }, {
          headers: {
            "Authorization": "Bearer " + localStorage.token
          }
        })
        this.eventBus.emit("open-result-dialog", {
          message: "Cập nhật thành công",
          type: "Success"
        })
        this.$router.push("/admin/accounts/tutors/registration")
      } catch (e) {
        console.log(e)
        this.eventBus.emit("open-result-dialog", {
          message: "Đã gặp sự cố. Vui lòng thử lại sau",
          type: "Error"
        })
      }
      this.eventBus.emit("close-loading-popup")
    }
  },
  mounted() {
    this.refresh();
  },
};
</script>
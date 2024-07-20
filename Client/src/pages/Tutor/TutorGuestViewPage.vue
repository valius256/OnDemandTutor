<template>
  <div v-if="user">
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
export default {
  name: "ProfilePage",
  components: {
    Profile,
    Schedule,
    Navigator,
    Subject,
    TutorClasses,
    Navbar,
  },
  data() {
    return {
      user: null,
      viewingId: 0,
    };
  },
  methods: {
    async refresh() {
      this.viewingId = this.$route.params.id
      const response = await axios.get(
        import.meta.env.VITE_API_URL + "/api/User/profile?userId=" + this.viewingId)

      this.user = response.data.data
    },
  },
  mounted() {
    this.refresh();
  },
};
</script>
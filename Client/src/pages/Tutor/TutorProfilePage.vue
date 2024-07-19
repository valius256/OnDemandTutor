<template>
  <div class="flex" v-if="user">
    <Navigator></Navigator>
    <div class="w-full">
      <profile :id="user.id" v-if="$route.path == '/tutor/profile'"></profile>
      <schedule
        :id="user.id"
        :currentUser="user"
        v-if="$route.path == '/tutor/schedule'"
      ></schedule>
      <subject
        :id="user.id"
        :currentUser="user"
        v-if="$route.path == '/tutor/subject'"
      ></subject>
      <payment
        :id="user.id"
        :currentUser="user"
        v-if="$route.path == '/tutor/payment'"
      ></payment>
      <preview
        :id="user.id"
        :currentUser="user"
        v-if="$route.path == '/tutor/preview'"
      ></preview>
    </div>
  </div>
</template>

<script>
import Navigator from "../../components/TutorProfile/Navigator.vue";
import Payment from "../../components/TutorProfile/Payment.vue";
import Profile from "../../components/TutorProfile/Profile.vue";
import Schedule from "../../components/TutorProfile/Schedule.vue";
import Preview from "../../components/TutorProfile/Preview.vue";
import Subject from "../../components/TutorProfile/Subject.vue";

export default {
  name: "ProfilePage",
  components: { Profile, Schedule, Payment, Navigator, Preview, Subject },
  data() {
    return {
      user: null,
    };
  },
  methods: {
    async refresh() {
      this.user = await this.getUserFromToken();
    },
  },
  mounted() {
    this.refresh();
  },
};
</script>

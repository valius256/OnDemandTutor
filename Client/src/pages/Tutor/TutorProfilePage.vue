<template>
  <div class="flex" v-if="user">
    <Navigator></Navigator>
    <div class="w-full">
      <profile :id="user.id" v-if="$route.path == '/tutor/profile'"></profile>
      <schedule :id="user.id" :currentUser="user" v-if="$route.path == '/tutor/schedule'"></schedule>
      <subject :id="user.id" :currentUser="user" v-if="$route.path == '/tutor/subject'"></subject>
      <tutor-classes :id="user.id" :currentUser="user" v-if="$route.path == '/tutor/myclass/list'"></tutor-classes>
      <class-creator :id="user.id" :currentUser="user" v-if="$route.path == '/tutor/myclass/create'"></class-creator>
      <payment :id="user.id" :currentUser="user" v-if="$route.path == '/tutor/payment'"></payment>
      <Videos :currentUser="user" v-if="$route.path == '/tutor/videos'"></Videos>
      <withdraw-request :id="user.id" :currentUser="user" v-if="$route.path == '/tutor/withdraw'"></withdraw-request>
    </div>
  </div>
</template>

<script>
import Navigator from "../../components/TutorProfile/Navigator.vue";
import Payment from "../../components/StudentProfile/Payment.vue";
import Profile from "../../components/TutorProfile/Profile.vue";
import Schedule from "../../components/TutorProfile/Schedule.vue";
import Subject from "../../components/TutorProfile/Subject.vue";
import TutorClasses from "../../components/TutorProfile/TutorClasses.vue";
import WithdrawRequest from "../../components/StudentProfile/WithdrawRequest.vue";
import Videos from "../../components/TutorProfile/Videos.vue";
import ClassCreator from '../../components/TutorProfile/ClassCreator.vue';
export default {
  name: "ProfilePage",
  components: {
    Profile,
    Schedule,
    Navigator,
    Payment,
    Subject,
    TutorClasses,
    WithdrawRequest,
    Videos,
    ClassCreator
  },
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

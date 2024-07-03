<template>
  <div class="flex">
    <Navigator></Navigator>
    <div class="w-full">
      <profile v-if="$route.path == '/tutor/profile'"></profile>
      <schedule v-if="$route.path == '/tutor/schedule'"></schedule>
      <payment v-if="$route.path == '/tutor/payment'"></payment>
    </div>
  </div>
</template>

<script>
import Navigator from "../../components/TutorProfile/Navigator.vue";
import Payment from "../../components/TutorProfile/Payment.vue";
import Profile from "../../components/TutorProfile/Profile.vue";
import Schedule from "../../components/TutorProfile/Schedule.vue";

export default {
  name: "ProfilePage",
  components: { Profile, Schedule, Payment, Navigator },
};
</script>

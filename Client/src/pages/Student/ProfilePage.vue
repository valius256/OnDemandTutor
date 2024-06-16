<template>
    <div class="flex">
        <Navigator></Navigator>
        <div class="w-full ">
            <profile v-if="$route.path == '/student/profile'"></profile>
            <schedule v-if="$route.path == '/student/schedule'"></schedule>
            <payment v-if="$route.path == '/student/payment'"></payment>
        </div>
    </div>

</template>

<script>
import Navigator from '../../components/StudentProfile/Navigator.vue';
import Payment from '../../components/StudentProfile/Payment.vue'
import Profile from '../../components/StudentProfile/Profile.vue'
import Schedule from '../../components/StudentProfile/Schedule.vue'

export default {
    name: "ProfilePage",
    components: { Profile, Schedule, Payment, Navigator },
}
</script>

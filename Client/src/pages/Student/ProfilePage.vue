<template>
    <div class="flex" v-if="user">
        <Navigator></Navigator>
        <div class="w-full ">
            <profile :id="user.id" v-if="$route.path == '/student/profile'"></profile>
            <schedule :id="user.id" v-if="$route.path == '/student/schedule'"></schedule>
            <payment :id="user.id" v-if="$route.path == '/student/payment'"></payment>
            <withdraw-request :id="user.id" v-if="$route.path == '/student/withdraw'"></withdraw-request>
        </div>
    </div>

</template>

<script>
import Navigator from '../../components/StudentProfile/Navigator.vue';
import Payment from '../../components/StudentProfile/Payment.vue'
import Profile from '../../components/StudentProfile/Profile.vue'
import Schedule from '../../components/StudentProfile/Schedule.vue'
import WithdrawRequest from '../../components/StudentProfile/WithdrawRequest.vue';

export default {
    name: "ProfilePage",
    components: { Profile, Schedule, Payment, Navigator, WithdrawRequest },
    data() {
        return {
            user : null
        }
    },
    methods : {
        async refresh(){
            this.user = await this.getUserFromToken()
        },
    },
    mounted(){
        this.refresh()
    }
}
</script>

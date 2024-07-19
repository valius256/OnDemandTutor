<template>
    <div class="-ml-4 w-full"  v-if="user">
        <div class="text-2xl font-bold p-8 bg-gray-300 ">
            Chào mừng trở lại, {{ (user.firstName ?? "") + " " + (user.lastName ?? "") }}
        </div>
        <div class="flex justify-end mr-4">
            <button class="text-blue-400 font-bold underline" @click="toggleOpenChangePass">Đổi mật khẩu</button>
        </div>
        <generic-popup v-if="isOpenChangePassPopup" title="Đổi mật khẩu" :closeFunction="toggleOpenChangePass">
            <change-password-popup :userId="user.id" :close="toggleOpenChangePass"></change-password-popup>
        </generic-popup>
    </div>

</template>

<script>
import ChangePasswordPopup from '../../components/common/ChangePasswordPopup.vue'
import GenericPopup from '../../components/common/GenericPopup.vue'
export default {
    name: "AdminDashboard",
    components : {ChangePasswordPopup, GenericPopup},
    data() { 
    
        return {
            user: null,
            isOpenChangePassPopup : false,
        }
    },
    methods: {
        async getUser() {
            this.user = await this.getUserFromToken()
        },
        toggleOpenChangePass(){
            this.isOpenChangePassPopup = !this.isOpenChangePassPopup
        }
    },
    mounted() {
        this.getUser()
    }
}
</script>

<style></style>
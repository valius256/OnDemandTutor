<template>
    <div class="p-4 w-full">
        <div class="text-2xl font-bold">
            Quản lý gia sư
        </div>
        <div class="mt-8 flex ">
            <router-link class="px-16 py-2 bg-gray-50 rounded-t-xl " to="/admin/accounts/tutors/list">Danh sách gia
                sư</router-link>
            <router-link class="px-16 py-2 bg-gray-50 rounded-t-xl" to="/admin/accounts/tutors/registration">Xác thực gia sư</router-link>
        </div>
        <div class="rounded-b-xl shadow-lg p-2 ">
            <tutor-list v-if="$route.path == '/admin/accounts/tutors/list'"></tutor-list>
            <tutor-registration v-if="$route.path == '/admin/accounts/tutors/registration'"></tutor-registration>
        </div>
    </div>
</template>

<script>
import TutorList from '../../components/Operators/TutorList.vue'
import TutorRegistration from '../../components/Operators/TutorRegistration.vue'
export default {
  components: { TutorList, TutorRegistration },
    name: "TutorManagementPage",
    
}
</script>

<style scoped>
.router-link-active {
    background: #b9b9b9
}
</style>
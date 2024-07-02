<template>
    <div class="p-4 w-full">
        <div class="text-2xl font-bold">
            Quản lý môn học
        </div>
        <div class="mt-8 flex ">
            <router-link class="px-16 py-2 bg-gray-50 rounded-t-xl " to="/admin/subjects/list">Danh sách môn học</router-link>
            <router-link class="px-16 py-2 bg-gray-50 rounded-t-xl" to="/admin/subjects/registration">Danh sách đăng ký</router-link>
        </div>
        <div class="rounded-b-xl shadow-lg p-2 ">
            <subject-list v-if="$route.path == '/admin/subjects/list'"></subject-list>
            <subject-registration v-if="$route.path == '/admin/subjects/registration'"></subject-registration>
        </div>
    </div>
</template>

<script>
import SubjectList from '../../components/Operators/SubjectList.vue'
import SubjectRegistration from '../../components/Operators/SubjectRegistration.vue'
export default {
  components: {SubjectList, SubjectRegistration },
    name: "SubjectManagement",
    
}
</script>

<style scoped>
.router-link-active {
    background: #b9b9b9
}
</style>
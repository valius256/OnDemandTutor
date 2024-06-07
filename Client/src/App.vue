<script>
import AdminLayout from './layouts/AdminLayout.vue';
import CustomerLayout from './layouts/CustomerLayout.vue';

export default {
  name: "App",
  components: { CustomerLayout, AdminLayout },
  data(){
    return {
      user : {
        role : "Admin"
      }
    }
  }
}
</script>

<template>
  <div>

    <div v-if='user.role == "Student"'>
      <CustomerLayout />
    </div>
    <div v-if='user.role == "Admin"'>
      <AdminLayout />
    </div>
  </div>

</template>

<style></style>
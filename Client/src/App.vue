<script>
import Authorization from './components/common/Authorization.vue';
import AdminLayout from './layouts/AdminLayout.vue';
import CustomerLayout from './layouts/CustomerLayout.vue';

export default {
  name: "App",
  inject : ['eventBus'],
  components: { CustomerLayout, AdminLayout, Authorization },
  data() {
    return {
      user: null
    }
  },
  mounted() {
    this.eventBus.emit("get-user", (user) => {
      this.user = user;
    });
  }
}
</script>

<template>
  <div>
    <Authorization>
      <div v-if='user?.role == "Student"'>
        <CustomerLayout />
      </div>
      <div v-if='user?.role == "Admin"'>
        <AdminLayout />
      </div>
    </Authorization>
  </div>

</template>

<style></style>
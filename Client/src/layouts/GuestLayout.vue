<template>
  <div>
    <Header class="sticky top-0 z-[100]"/>
    <RouterView />
    <Footer />
  </div>
</template>

<script>
import Header from "../components/common/Header.vue";
import Footer from "../components/common/Footer.vue";

export default {
  name: "GuestLayout",
  components: { Header, Footer },
};
</script>

<style></style>

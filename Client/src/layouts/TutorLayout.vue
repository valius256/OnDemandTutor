<template>
  <div>
    <Header class="sticky left-0 z-[100]" />
    <RouterView class="min-h-screen max-w-screen overflow-x-auto" />
    <Footer />
  </div>
</template>

<script>
import Header from "../components/common/Header.vue";
import Footer from "../components/common/Footer.vue";

export default {
  name: "TutorLayout",
  components: { Header, Footer },
};
</script>

<style></style>

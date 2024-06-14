<template>
  <div>
    <Header />
    <RouterView />
    <Footer />
  </div>
</template>

<script>
import Header from "../components/common/Header.vue";
import Footer from "../components/common/Footer.vue";
export default {
  name: "OperatorLayout",
  components: { Header, Footer },
};
</script>

<style></style>

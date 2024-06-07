<template>
    <div class="flex gap-4">
        <VerticalNavbar/>
        <RouterView class="overflow-y-auto h-screen"/>
    </div>
</template>

<script>
import VerticalNavbar from '../components/common/VerticalNavbar.vue';

export default {
    name : "AdminLayout",
    components : {VerticalNavbar}
}
</script>

<style>

</style>
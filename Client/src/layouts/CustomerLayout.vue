<template>
    <div>
        <Header/>
        <RouterView />
    </div>
</template>

<script>
import Header from '../components/common/Header.vue';

export default {
    name : "CustomerLayout",
    components : {Header}
}
</script>

<style>

</style>
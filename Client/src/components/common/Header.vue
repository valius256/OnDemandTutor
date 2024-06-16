<template>
    <div class="shadow-md flex place-content-between bg-blue-50 ">
        <div class="text-3xl font-bold p-3 ">
            <span>On</span>
            <span class="text-blue-600">Demand</span>
            <span>Tutor</span>
        </div>
        <Navbar />
    </div>
</template>

<script>
import Navbar from './Navbar.vue';

export default {
    name: "Header",
    inject: ['eventBus'],
    components: { Navbar },
    data() {
        
    },
    methods: {
        async getUser() {
            const userPromise = new Promise((resolve) => {
                this.eventBus.emit("get-user", resolve);
            });
            const user = await userPromise;
            this.user = user;
        },
    },
    mounted() {
        this.getUser()
    }
}
</script>

<style></style>
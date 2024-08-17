<template>
    <div class="relative " ref="notificationWrapper">
        <div class="flex items-center h-full">
            <button class="text-xl relative" @click="toggleNotificationPopup">
                <i class="fa fa-bell"></i>
                <div v-if="unViewedNoti > 0"
                    class="absolute -top-2 -right-4 text-sm w-6 h-6 font-bold text-white rounded-full bg-blue-600 flex items-center justify-center">
                    {{ unViewedNoti }}
                </div>
            </button>

        </div>

        <div v-if="isOpenNoti"
            class="absolute right-0 rounded-xl bg-white animate-fade-down animate-duration-[400ms] animate-normal flex flex-col  w-96 overflow-y-auto h-96 shadow-md">
            <div class="italic p-2 text-center" v-if="notifications.length == 0">Bạn hiện không có thông báo nào</div>
            <button v-for="noti in notifications" :key="noti.id" @click="handleViewNoti(noti.id)"
                class="p-4 flex place-content-between hover:bg-slate-100 text-left"
                :class="{ 'bg-slate-200 font-bold': !noti.isViewed }">
                <div class="flex gap-4">
                    <img class="w-16 h-16 rounded-full" :src="noti.refImageUrl ?? '/src/assets/noavatar.jpg'">
                    <div>
                        <div>
                            {{ noti.content }}
                        </div>
                        <div class="text-sm font-normal italic">
                            {{ this.beautifyDatetime(noti.createdDate) }}
                        </div>

                    </div>
                </div>
                <div v-if="!noti.isViewed" class="w-4 h-4 bg-blue-400 rounded-full">
                </div>
            </button>
        </div>
    </div>
</template>

<script>
import axios from 'axios'
export default {
    name: "Bell",
    data() {
        return {
            isOpenNoti: false,
            unViewedNoti : 0,
            notifications: [

            ]
        }
    },
    methods: {
        toggleNotificationPopup() {
            this.isOpenNoti = !this.isOpenNoti
        },
        handleClickOutside(event) {
            if (!this.$refs.notificationWrapper?.contains(event.target)) {
                this.isOpenNoti = false;
            }
        },
        async fetchNoti() {
            try {
                const response = await axios.get(import.meta.env.VITE_API_URL + "/api/Notification", {
                    headers: {
                        'Authorization': "Bearer " + localStorage.token,
                    },
                });
                this.notifications = response.data.items
                this.unViewedNoti = this.notifications.filter(n => !n.isViewed).length 
            }

            catch (e) {
                console.log(e)
            }
        },
        async handleViewNoti(id){
            try {
                await axios.put(import.meta.env.VITE_API_URL + "/api/Notification/" + id, null, {
                    headers: {
                        'Authorization': "Bearer " + localStorage.token,
                    },
                });
                var noti = this.notifications.find(n => n.id == id)
                if (noti && noti.refUrl){
                    this.$router.push(noti.refUrl)
                }
                await this.fetchNoti()
            }

            catch (e) {
                console.log(e)
            }
        }
    },
    mounted() {
        document.addEventListener('click', this.handleClickOutside);
        this.fetchNoti()
    },
    beforeDestroy() {
        document.removeEventListener('click', this.handleClickOutside);
    }
}
</script>

<style></style>
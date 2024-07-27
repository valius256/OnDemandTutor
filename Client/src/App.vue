<template>
  <div>
    <Authorization>
      <div v-if="user?.role == 0 || !user">
        <StudentLayout />
      </div>
      <div v-else-if="user?.role == 1">
        <TutorLayout />
      </div>
      <!-- <div v-else-if="user?.role == 2">
        <OperatorLayout />
      </div> -->
      <div v-else-if="user?.role == 3 || user?.role == 2">
        <AdminLayout />
      </div>
      <div v-else>
        <UnexpectedLayout />
      </div>
    </Authorization>
    <!-- Add BubbleChat component -->
    <BubbleChat
      :messages="messages"
      textField="text"
      senderNameField="name"
      avatarLinkField="avatar"
      position="right"
    />
  </div>
</template>

<script>
import Authorization from "./components/common/Authorization.vue";
import StudentLayout from "./layouts/StudentLayout.vue";
import AdminLayout from "./layouts/AdminLayout.vue";
import UnexpectedLayout from "./layouts/UnexpectedLayout.vue";
import TutorLayout from "./layouts/TutorLayout.vue";
import OperatorLayout from "./layouts/OperatorLayout.vue";
import GuestLayout from "./layouts/GuestLayout.vue";
import BubbleChat from "./components/BubbleChat.vue"; // Import BubbleChat

export default {
  name: "App",
  inject: ["eventBus"],
  components: {
    Authorization,
    StudentLayout,
    AdminLayout,
    TutorLayout,
    OperatorLayout,
    GuestLayout,
    UnexpectedLayout,
    BubbleChat, // Register BubbleChat
  },
  data() {
    return {
      user: null,
      messages: [
        { name: "John", text: "Hello!", avatar: "link_to_avatar" },
        { name: "Doe", text: "Hi!", avatar: "link_to_avatar" },
      ],
    };
  },
  methods: {
    async getUser() {
      this.user = await this.getUserFromToken();
    },
    clearUser() {
      console.log("Clearing user data");
      this.user = null;
    },
    checkUser() {
      console.log("Checking user data");
      if (this.user === undefined || this.user === null) {
        console.log("user is undefined or null");
      } else if (this.user.role === undefined) {
        console.log("user.role is undefined");
      } else {
        console.log("user.role is", this.user.role);
      }
    },
  },
  mounted() {
    //console.log("App component mounted");
    //console.log("Token found in localStorage");
    this.getUser();
    this.eventBus.on("update-app-user", async () => {
      console.log("Received update-app-user event");
      await this.getUser();
      this.checkUser(); // Call checkUser after user data is fetched
    });
  },
  destroyed() {
    console.log("App component destroyed");
    this.clearUser(); // Clear user data when component is destroyed
  },
};
</script>

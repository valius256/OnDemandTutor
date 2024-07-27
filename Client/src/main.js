import { createApp } from "vue";
import "./index.css";
import App from "./App.vue";
import router from "./router";
import mitt from "mitt";
import utilities from "./utilities.vue";
import BubbleChat from "vue-bubble-chat"; // Import vue-bubble-chat

const app = createApp(App);
const eventBus = mitt();

app.use(router);
app.provide("eventBus", eventBus);
app.mixin(utilities);
app.use(BubbleChat); // Use vue-bubble-chat

app.mount("#app");

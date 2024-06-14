import { createApp } from "vue";
import "./index.css";
import App from "./App.vue";
import router from "./router";
import mitt from "mitt";
import utilities from "./utilities.vue";

const app = createApp(App);
const eventBus = mitt();

app.use(router);
app.provide("eventBus", eventBus);
app.mixin(utilities);
app.mount("#app");

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import mitt from 'mitt'
import './index.css'

const app = createApp(App);
const eventBus = mitt();

app.use(router);
app.provide('eventBus', eventBus);
app.mount('#app')

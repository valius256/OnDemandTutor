import HomePage from '../pages/HomePage.vue'
import TutorsPage from '../pages/TutorsPage.vue'
import ClassesPage from '../pages/ClassesPage.vue'
import BlogsPage from '../pages/BlogsPage.vue'
import FaqsPage from '../pages/FaqPage.vue'
import {createRouter , createWebHistory} from 'vue-router';

const routes = [
    {
        path: '/',
        name: 'Home',
        component: HomePage
    },
    {
        path: '/tutors',
        name: 'TutorPage',
        component: TutorsPage
    },
    {
        path: '/classes',
        name: 'ClassesPage',
        component: ClassesPage
    },
    {
        path: '/blogs',
        name: 'BlogsPage',
        component: BlogsPage
    },
    {
        path: '/faqs',
        name: 'FaqsPage',
        component: FaqsPage
    }
]

const router = createRouter({
    history : createWebHistory("/"),
    routes
})

export default router
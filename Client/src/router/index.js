import HomePage from '../pages/Student/HomePage.vue'
import TutorsPage from '../pages/Student/TutorsPage.vue'
import ClassesPage from '../pages/Student/ClassesPage.vue'
import BlogsPage from '../pages/Student/BlogsPage.vue'
import FaqsPage from '../pages/Student/FaqPage.vue'
import StudentManagementPage from '../pages/Operators/StudentManagementPage.vue'
import ClassManagementPage from '../pages/Operators/ClassManagementPage.vue'
import TutorManagementPage from '../pages/Operators/TutorManagementPage.vue'
import SubjectManagementPage from '../pages/Operators/SubjectManagementPage.vue'
import BlogManagementPage from '../pages/Operators/BlogManagementPage.vue'
import FaqManagementPage from '../pages/Operators/FaqManagementPage.vue'
import ClassRequestPage from '../pages/Operators/ClassRequestPage.vue'
import ConsultationPage from '../pages/Operators/ConsultationPage.vue'
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
    },
    {
        path: '/admin/accounts',
        name: 'AccountManagement',
        redirect: '/admin/accounts/students',
        children: [
            {
              path: 'students',
              component: StudentManagementPage
            },
            {
              path: 'tutors',
              component: TutorManagementPage
            }
          ]
    },
    {
        path: '/admin/classes',
        name: 'ClassManagementPage',
        redirect: '/admin/classes/list',
        children: [
            {
              path: 'list',
              component: ClassManagementPage
            },
            {
              path: 'requests',
              component: ClassRequestPage
            }
          ]
    },
    {
        path: '/admin/subjects',
        name: 'SubjectManagementPage',
        component: SubjectManagementPage
    },
    {
        path: '/admin/blogs',
        name: 'BlogManagementPage',
        component: BlogManagementPage
    },
    {
        path: '/admin/faqs',
        name: 'FaqManagementPage',
        component: FaqManagementPage
    },
    {
        path: '/admin/consultation',
        name: 'ConsultationPage',
        component: ConsultationPage
    },
]

const router = createRouter({
    history : createWebHistory("/"),
    routes
})

export default router
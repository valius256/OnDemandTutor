import HomePage from "../pages/Student/HomePage.vue";
import TutorsPage from "../pages/Student/TutorsPage.vue";
import ClassesPage from "../pages/Student/ClassesPage.vue";
import LoginPage from "../pages/Auth/LoginPage.vue";
import RegisterPage from "../pages/Auth/RegisterPage.vue";
// import ForgotPasswordPage from "../pages/Auth/ForgotPasswordPage.vue";
import BlogsPage from "../pages/Student/BlogsPage.vue";
import FaqsPage from "../pages/Student/FaqPage.vue";
import StudentProfile from "../pages/Student/ProfilePage.vue";
// import ClassDetailPage from "../pages/Student/ClassDetailPage.vue";
// import CreateClassPage from "../pages/Student/CreateClassPage.vue";
import StudentManagementPage from "../pages/Operators/StudentManagementPage.vue";
import OperatorManagementPage from "../pages/Operators/OperatorMangementPage.vue";
import TutorManagementPage from "../pages/Operators/TutorManagementPage.vue";
import SubjectManagementPage from "../pages/Operators/SubjectManagementPage.vue";
import SubjectRegistrationDetailPage from "../pages/Operators/SubjectRegistrationDetailPage.vue";
import BlogManagementPage from "../pages/Operators/BlogManagementPage.vue";
import BlogEditorPage from "../pages/Operators/BlogEditorPage.vue"
import FaqManagementPage from "../pages/Operators/FaqManagementPage.vue";
import ConsultationPage from "../pages/Operators/ConsultationPage.vue";
import { createRouter, createWebHistory } from "vue-router";
const routes = [
  {
    path: "/",
    name: "Home",
    component: HomePage,
  },
  {
    path: "/tutors",
    name: "TutorPage",
    component: TutorsPage,
  },
  {
    path: "/classes",
    name: "ClassesPage",
    component: ClassesPage,
  },
  {
    path: "/login",
    name: "LoginPage",
    component: LoginPage,
  },
  {
    path: "/register",
    name: "RegisterPage",
    component: RegisterPage,
  },
  // {
  //   path: "/forgotPassword",
  //   name: "ForgotPassword",
  //   component: ForgotPasswordPage,
  // },
  {
    path: "/blogs",
    name: "BlogsPage",
    component: BlogsPage,
  },
  {
    path: "/faqs",
    name: "FaqsPage",
    component: FaqsPage,
  },
  // {
  //   path: "/classDetail/:id",
  //   name: "ClassDetailPage",
  //   component: ClassDetailPage,
  // },
  // {
  //   path: "/editClass/:id",
  //   name: "CreateClassPage",
  //   component: CreateClassPage,
  // },
  {
    path: "/student",
    name: "StudentProfilePage",
    redirect : "/student/profile",
    children: [
      {
        path: "profile",
        component: StudentProfile,
      },
      {
        path: "schedule",
        component: StudentProfile,
      },
      {
        path: "payment",
        component: StudentProfile,
      },
    ],
  },
  {
    path: "/admin/accounts",
    name: "AccountManagement",
    redirect: "/admin/accounts/students",
    children: [
      {
        path: "students",
        component: StudentManagementPage,
      },
      {
        path: "tutors",
        redirect: "/admin/accounts/tutors/list",
        children : [
          {
            path : "list",
            component: TutorManagementPage,
          },
          {
            path : "registration",
            component: TutorManagementPage,
          }
        ]
      },
      {
        path: "operators",
        component: OperatorManagementPage,
      },
    ],
  },
  // {
  //   path: "/admin/classes",
  //   name: "ClassManagementPage",
  //   component: ClassManagementPage,
  // },
  {
    path: "/admin/subjects",
    name: "SubjectManagementPage",
    redirect : "/admin/subjects/list",
    children : [
      {
        path : "list",
        component: SubjectManagementPage,
      },
      {
        path : "registration",
        component: SubjectManagementPage,
      },
      {
        path : "detail/:id",
        component: SubjectRegistrationDetailPage,
      }
    ]
  },
  {
    path: "/admin/blogs",
    name: "BlogManagementPage",
    redirect : "/admin/blogs/manage",
    children : [
      {
        path : "manage",
        component: BlogManagementPage,
      },
      {
        path : "editor/:id",
        component: BlogEditorPage,
      },
    ]
  },
  {
    path: "/admin/faqs",
    name: "FaqManagementPage",
    component: FaqManagementPage,
  },
  {
    path: "/admin/consultation",
    name: "ConsultationPage",
    component: ConsultationPage,
  },
];

const router = createRouter({
  history: createWebHistory("/"),
  routes,
});

export default router;

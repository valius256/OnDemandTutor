<template>
    <div class="p-4 flex gap-4">
        <div class="w-1/4">
            a
        </div>
        <div class="w-3/4">
            v
        </div>
    </div>
</template>

<script>
export default {
    name: "ClassManagementPage",
    data() {
        return {
            classes: [
                {
                    id: 1,
                    name: "Luyện thi IELTS nâng cao",
                    startDate: "2024-01-01",
                    endDate: "2024-12-01",
                    method: "Online",
                    location: "Q9, TPHCM",
                    subject: {
                        name: "Tiếng Anh"
                    },
                    studentNumber: 10,
                    maxStudentNumber: 20,
                    tutor : {
                        name : "Thomas"
                    },
                    status : "NotYet"
                },
                {
                    id: 2,
                    name: "Luyện thi TOEFL cơ bản",
                    startDate: "2024-02-01",
                    endDate: "2024-10-01",
                    method: "Offline",
                    location: "Q3, TPHCM",
                    subject: {
                        name: "Tiếng Anh"
                    },
                    studentNumber: 15,
                    maxStudentNumber: 25,
                    tutor : {
                        name : "Thomas"
                    },
                    status : "NotYet"
                },
                {
                    id: 3,
                    name: "Luyện thi đại học môn Toán",
                    startDate: "2024-03-01",
                    endDate: "2024-09-01",
                    method: "Online",
                    location: "Q7, TPHCM",
                    subject: {
                        name: "Toán"
                    },
                    studentNumber: 12,
                    maxStudentNumber: 30,
                    tutor : {
                        name : "Thomas"
                    },
                    status : "OnGoing"
                },
                {
                    id: 4,
                    name: "Luyện thi đại học môn Lý",
                    startDate: "2024-04-01",
                    endDate: "2024-11-01",
                    method: "Offline",
                    location: "Q5, TPHCM",
                    subject: {
                        name: "Vật Lý"
                    },
                    studentNumber: 8,
                    maxStudentNumber: 20,
                    tutor : {
                        name : "Arthur"
                    },
                    status : "NotYet"
                },
                {
                    id: 5,
                    name: "Khóa học lập trình Python cơ bản",
                    startDate: "2024-05-01",
                    endDate: "2024-08-01",
                    method: "Online",
                    location: "Q10, TPHCM",
                    subject: {
                        name: "Khoa học máy tính"
                    },
                    studentNumber: 20,
                    maxStudentNumber: 30,
                    tutor : {
                        name : "Arthur"
                    },
                    status : "Finished"
                },
                {
                    id: 6,
                    name: "Khóa học lập trình Java nâng cao",
                    startDate: "2024-06-01",
                    endDate: "2024-12-01",
                    method: "Offline",
                    location: "Q1, TPHCM",
                    subject: {
                        name: "Khoa học máy tính"
                    },
                    studentNumber: 18,
                    maxStudentNumber: 25,
                    tutor : {
                        name : "Arthur"
                    },
                    status : "NotYet"
                },
                {
                    id: 7,
                    name: "Khóa học quản lý dự án PMP",
                    startDate: "2024-07-01",
                    endDate: "2024-12-01",
                    method: "Online",
                    location: "Q2, TPHCM",
                    subject: {
                        name: "Quản lý dự án"
                    },
                    studentNumber: 25,
                    maxStudentNumber: 30,
                    tutor : {
                        name : "John"
                    },
                    status : "NotYet"
                }
            ]

        }
    }
}
</script>

<style></style>
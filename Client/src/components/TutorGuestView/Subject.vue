<template>
    <div>
        <div class="px-6 py-8">
            <div v-if="tutorSubjects.length === 0">Gia sư có môn học nào.</div>
            <div v-else class="flex flex-wrap gap-4 justify-center">
                <div v-for="tutorSubject in tutorSubjects" :key="tutorSubject.id" @click="selectSubject(tutorSubject)"
                    class="cursor-pointer px-8 py-4 text-xl bg-blue-400 rounded-lg hover:bg-blue-200 text-white font-bold">
                    {{ tutorSubject.subject.name }}
                </div>
            </div>
        </div>

        <div v-if="selectedSubject" class="mt-6 px-6 py-8 bg-slate-50 rounded-lg flex flex-col gap-4">
            <h3 class="text-xl font-bold mb-4">{{ selectedSubject.subject.name }}</h3>
            <hr>
            <div><span class="font-bold">Ngày đăng kí:</span> {{ formatDate(selectedSubject.createdDate) }}</div>
            <div><span class="font-bold">Mô tả:</span> {{ selectedSubject.description || "Không có mô tả" }}</div>
            <div class="font-bold">Bằng cấp:</div>
            <div class="flex flex-wrap gap-2">
                <div v-for="(degree, index) in tutorSubject.degrees" :key="index" class="flex flex-col">
                    <img :src="degree.degreeImgUrl" class="w-96 h-96 object-cover rounded-lg" />
                    <div><span class="font-bold mr-4">Tên bằng</span>{{ degree.tutorDegreeName }}</div>
                    <div><span class="font-bold mr-4">Mã bằng</span>{{ degree.degreeNumber }}</div>
                    <div><span class="font-bold mr-4">Ngày phát</span>{{ degree.issuranceDate }}</div>
                </div>

            </div>
            <!-- <div>
          Video:
          <a :href="selectedSubject.videoLink" target="_blank">{{
          selectedSubject.videoLink
        }}</a>
        </div> -->
        </div>


    </div>
</template>

<script>
import axios from "axios";

export default {
    props: ['tutor'],
    inject: ['eventBus'],
    data() {
        return {
            subjects: [], // List of subjects
            tutorSubjects: [], // List of tutor subjects
            tutorSubject: null,
            selectedSubject: null, // Currently selected subject
            qualificationPreview: [],
            videoPreview: null,
        };
    },
    methods: {
        async fetchSubjects() {
            try {
                const response = await axios.get(
                    import.meta.env.VITE_API_URL + "/api/TutorSubject",
                    {
                        params: {
                            "Filter.TutorName": `${this.tutor.firstName ?? ""} ${this.tutor.lastName ?? ""}`,
                            "Sorts[column]": "string",
                            "Sorts[isDesc]": true,
                        },
                        headers: {
                            Authorization: "Bearer " + localStorage.token,
                        },
                    }
                );
                this.tutorSubjects = response.data.items;
                this.tutorSubjects = this.tutorSubjects.filter(ts => ts.status == 3)
            } catch (error) {
                console.error("Error fetching tutor subjects:", error);
            }
        },
        async fetchTutorSubjectDetail(id) {
            try {
                const response = await axios.get(
                    import.meta.env.VITE_API_URL + "/api/TutorSubject/" + id,
                    {
                        headers: {
                            Authorization: "Bearer " + localStorage.token,
                        },
                    }
                );
                this.tutorSubject = response.data;
            } catch (error) {
                console.error("Error fetching tutor subjects:", error);
            }
        },
        formatDate(dateString) {
            const date = new Date(dateString);
            return date.toLocaleDateString();
        },
        async selectSubject(subject) {
            this.selectedSubject = subject;
            await this.fetchTutorSubjectDetail(subject.id)
        },
        getMillisecondsFromMinDate(date) {
            // The minimum date value is January 1, 1970, 00:00:00 UTC
            const minDate = new Date(0);
            return date.getTime() - minDate.getTime();
        },
    },
    mounted() {
        this.fetchSubjects();
    },
};
</script>

<style scoped>
/* Add your styles here */
</style>
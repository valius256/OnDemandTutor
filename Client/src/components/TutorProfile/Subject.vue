<template>
  <div>
    <div v-if="currentUser.tutorStatus == 3">

      <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200">
        Môn học của bạn
      </div>
      <div class="px-6 py-8">
        <div v-if="tutorSubjects.length === 0">Bạn chưa có môn học nào.</div>
        <div v-else class="flex flex-wrap gap-4">
          <div v-for="tutorSubject in tutorSubjects" :key="tutorSubject.id" @click="selectSubject(tutorSubject)"
            class="cursor-pointer p-4 bg-blue-100 rounded-lg hover:bg-blue-200">
            {{ tutorSubject.subject.name }}
          </div>
        </div>
      </div>

      <div v-if="selectedSubject" class="mt-6 px-6 py-8 bg-slate-50 rounded-lg">
        <h3 class="text-xl font-bold mb-4">{{ selectedSubject.subject.name }}</h3>
        <div>Ngày đăng kí: {{ formatDate(selectedSubject.createdDate) }}</div>
        <div>Mô tả: {{ selectedSubject.description || "Không có mô tả" }}</div>
        <div>Trạng thái: {{ getStatus(selectedSubject.status) }}</div>
        <div v-if="selectedSubject.status === 2">
          Lý do từ chối: {{ selectedSubject.reasonReject || "Không có lý do" }}
        </div>
        <div>Bằng cấp:</div>
        <div class="flex flex-wrap gap-2">
          <img v-for="(img, index) in selectedSubject.qualification" :key="index" :src="img"
            class="w-24 h-24 object-cover rounded-lg" />
        </div>
        <!-- <div>
        Video:
        <a :href="selectedSubject.videoLink" target="_blank">{{
        selectedSubject.videoLink
      }}</a>
      </div> -->
      </div>

      <div class="mt-6 px-6 py-8 bg-slate-50 rounded-lg">
        <h3 class="text-xl font-bold mb-4">Đăng kí môn học mới</h3>
        <form @submit.prevent="registerSubject">
          <div class="mb-4">
            <label for="subjectName" class="block font-bold mb-2">Tên môn học</label>
            <select id="subjectName" v-model="newSubject.subjectId" class="w-full rounded border border-gray-200 p-2"
              required>
              <option v-for="subject in subjects" :key="subject.id" :value="subject.id">
                {{ subject.name }}
              </option>
            </select>
          </div>
          <div class="mb-4">
            <label for="qualification" class="block font-bold mb-2">Bằng cấp</label>
            <input type="file" id="qualification" multiple accept="image/*" @change="handleQualificationUpload"
              class="w-full rounded border border-gray-200 p-2" />
            <div class="flex flex-wrap gap-2 mt-2">
              <div v-for="(qualification, index) in newSubject.qualification" :key="index">
                <img :src="qualificationPreview[index]" class="w-24 h-24 object-cover rounded-lg" />
                <div class="flex flex-col gap-2">
                  <input v-model="qualification.degreeName" class="p-1 border rounded-lg" placeholder="Tên bằng cấp">
                  <input v-model="qualification.degreeNumber" class="p-1 border rounded-lg" placeholder="Số bằng cấp">
                  <input v-model="qualification.date" class="p-1 border rounded-lg" placeholder="Ngày hiệu lực"
                    type="date">
                  <!-- <button @click="removeQualification(index)"
                  class="p-1 border rounded-lg bg-red-500 text-white">Remove</button> -->
                </div>
              </div>
            </div>
          </div>
          <!-- <div class="mb-4">
          <label for="videoLink" class="block font-bold mb-2">Upload video</label>
          <input type="file" id="videoLink" accept="video/*" @change="handleVideoUpload"
            class="w-full rounded border border-gray-200 p-2" />
        </div> -->
          <div class="mb-4">
            <label for="description" class="block font-bold mb-2">Mô tả</label>
            <textarea id="description" v-model="newSubject.description"
              class="w-full rounded border border-gray-200 p-2"></textarea>
          </div>
          <button type="submit" class="p-2 font-bold text-white bg-green-400 hover:bg-green-200 rounded-lg">
            Đăng kí
          </button>
        </form>
      </div>
    </div>
    <div v-else class="p-8">
      <div class="p-8 bg-red-200 rounded-lg text-center font-bold">
        Bạn cần xác thực tài khoản để sử dụng tính năng này

      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  props: {
    currentUser: {
      type: Object,
      required: true,
    },
  },
  inject: ['eventBus'],
  data() {
    return {
      subjects: [], // List of subjects
      tutorSubjects: [], // List of tutor subjects
      selectedSubject: null, // Currently selected subject
      newSubject: {
        subjectId: "",
        qualification: [

        ],
        video: null,
        description: "",
      },
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
              "Filter.TutorName": `${this.currentUser.firstName ?? ""} ${this.currentUser.lastName ?? ""}`,
              "Sorts[column]": "string",
              "Sorts[isDesc]": true,
            },
            headers: {
              Authorization: "Bearer " + localStorage.token,
            },
          }
        );
        this.tutorSubjects = response.data.items;
      } catch (error) {
        console.error("Error fetching tutor subjects:", error);
      }
    },
    async fetchAllSubjects() {
      try {
        const response = await axios.get(
          import.meta.env.VITE_API_URL + "/api/Subject",
          {
            params: {
              "Sorts[column]": "name",
              "Sorts[isDesc]": false,
            },
            headers: {
              Authorization: "Bearer " + localStorage.token,
            },
          }
        );
        this.subjects = response.data.items;
      } catch (error) {
        console.error("Error fetching subjects:", error);
      }
    },
    selectSubject(subject) {
      this.selectedSubject = subject;
    },
    handleQualificationUpload(event) {
      const files = event.target.files;
      if (files.length > 10) {
        alert("Bạn chỉ có thể tải lên tối đa 10 ảnh.");
        return;
      }
      this.newSubject.qualification = Array.from(files).map((file) => ({
        degreeName: '',
        degreeNumber: '',
        date: '',
        file: file
      }));
      this.qualificationPreview = Array.from(files).map((file) =>
        URL.createObjectURL(file)
      );
      console.log(this.newSubject.qualification)
      console.log(this.qualificationPreview)
    },
    handleVideoUpload(event) {
      const file = event.target.files[0];
      if (file.size > 16 * 1024 * 1024) {
        alert("Video không được vượt quá 16 MB.");
        return;
      }
      this.newSubject.video = file;
      this.videoPreview = URL.createObjectURL(file);
    },
    async registerSubject() {
      try {
        this.eventBus.emit("open-loading-popup", {
          message: "Vui lòng chờ..."
        })
        const tutorSubjectResponse = await axios.post(
          import.meta.env.VITE_API_URL + "/api/TutorSubject",
          {
            userId: this.currentUser.id,
            subjectId: this.newSubject.subjectId,
            description: this.newSubject.description,
          },
          {
            headers: {
              Authorization: "Bearer " + localStorage.token,
            },
          }
        );

        const tutorSubjectId = tutorSubjectResponse.data.id;

        for (const file of this.newSubject.qualification) {
          const formData = new FormData();
          formData.append("file", file.file);
          const fileName = "Degree_of_" + this.currentUser.id + "_" + this.getMillisecondsFromMinDate(new Date());
          const uploadDegree = await axios.post(
            import.meta.env.VITE_API_URL + "/api/Upload/upload-image?fileName=" + fileName,
            formData,
            {
              headers: {
                Authorization: "Bearer " + localStorage.token,
                "Content-Type": "multipart/form-data",
              },
            })
          await axios.post(
            import.meta.env.VITE_API_URL + "/api/TutorDegree",
            {
              tutorId: this.currentUser.id,
              subjectId: this.newSubject.subjectId,
              degreeImgUrl: uploadDegree.data,
              description: "",
              tutorDegreeName: file.degreeName,
              degreeNumber: file.degreeNumber,
              issuranceDate: file.date
            },
            {
              headers: {
                Authorization: "Bearer " + localStorage.token,
              },
            }
          );
        }
        this.newSubject = {
          subjectId: "",
          qualification: [],
          video: null,
          description: "",
        };
        this.qualificationPreview = [];
        this.videoPreview = null;
        this.eventBus.emit("open-result-dialog", {
          message: "Gửi thành công",
          type: "Success"
        })
        await this.fetchSubjects();
      } catch (error) {
        console.error("Error registering subject:", error);
        this.eventBus.emit("open-result-dialog", {
          message: "Có lỗi xảy ra khi gửi yêu cầu",
          type: "Error"
        })
      }
      this.eventBus.emit("close-loading-popup")
    },
    formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleDateString();
    },
    getStatus(status) {
      switch (status) {
        case 0:
          return "Đang chờ xử lý";
        case 1:
          return "Đã vô hiệu hóa";
        case 2:
          return "Đã từ chối";
        case 3:
          return "Đã được chấp nhận";
        default:
          return "Không xác định";
      }
    },
    getMillisecondsFromMinDate(date) {
      // The minimum date value is January 1, 1970, 00:00:00 UTC
      const minDate = new Date(0);
      return date.getTime() - minDate.getTime();
    },
  },
  mounted() {
    this.fetchSubjects();
    this.fetchAllSubjects();
  },
};
</script>

<style scoped>
/* Add your styles here */
</style>

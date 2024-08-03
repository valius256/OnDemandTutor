<template>
  <div>
    <div class="text-2xl font-bold px-6 py-8 bg-slate-200">
      Môn học của bạn
    </div>
    <div v-if="currentUser.tutorStatus == 3" class="space-y-8 p-6 bg-gray-100 min-h-screen">
      <div class="bg-white shadow-lg rounded-lg p-6">
        <div v-if="tutorSubjects.length === 0" class="text-gray-500">Bạn chưa có môn học nào.</div>
        <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          <div v-for="tutorSubject in tutorSubjects" :key="tutorSubject.id" @click="selectSubject(tutorSubject)"
            class="cursor-pointer p-4 bg-blue-50 rounded-lg hover:bg-blue-100 transition duration-300 ease-in-out transform hover:scale-105">
            <div class="text-xl font-semibold text-gray-800">{{ tutorSubject.subject.name }}</div>
          </div>
        </div>
      </div>

      <div v-if="selectedSubject" class="bg-white shadow-lg rounded-lg p-6 space-y-4">
        <h3 class="text-2xl font-extrabold text-gray-800 mb-4">{{ selectedSubject.subject.name }}</h3>
        <div class="text-gray-600">Ngày đăng kí: {{ formatDate(selectedSubject.createdDate) }}</div>
        <div class="text-gray-600">Mô tả: {{ selectedSubject.description || "Không có mô tả" }}</div>
        <div class="text-gray-600">Trạng thái: {{ getStatus(selectedSubject.status) }}</div>
        <div v-if="selectedSubject.status === 2" class="text-red-500">
          Lý do từ chối: {{ selectedSubject.reasonReject || "Không có lý do" }}
        </div>
        <div class="text-gray-800 font-semibold">Bằng cấp:</div>
        <div class="flex flex-col gap-2">
          <div v-for="degree in selectedSubject.degrees" :key="degree.id" class="flex gap-8">
            <img :src="degree.degreeImgUrl" class="w-1/3 object-cover rounded-lg border border-gray-200" />
            <div>
              <span class="font-bold">Tên bằng cấp : </span> {{ degree.tutorDegreeName }}<br>
              <span class="font-bold">Số bằng cấp : </span> {{ degree.degreeNumber }}<br>
              <span class="font-bold">Ngày cấp : </span> {{ degree.issuranceDate }}
            </div>
          </div>
        </div>
      </div>

      <div class="bg-white shadow-lg rounded-lg p-6">
        <h3 class="text-2xl font-extrabold text-gray-800 mb-4">Đăng kí môn học mới</h3>
        <form @submit.prevent="registerSubject" class="space-y-4">
          <div>
            <label for="subjectName" class="block text-gray-700 font-bold mb-2">Tên môn học</label>
            <select id="subjectName" v-model="newSubject.subjectId"
              class="w-full rounded-lg border border-gray-300 p-3 focus:ring-2 focus:ring-blue-200" required>
              <option v-for="subject in subjects" :key="subject.id" :value="subject.id">
                {{ subject.name }}
              </option>
            </select>
          </div>
          <div>
            <label for="qualification" class="block text-gray-700 font-bold mb-2">Bằng cấp</label>
            <input type="file" id="qualification" multiple accept="image/*" @change="handleQualificationUpload"
              class="w-full rounded-lg border-gray-300 p-3 focus:ring-2 focus:ring-blue-200" />
            <div class="flex flex-col gap-4 mt-2">
              <div v-for="(qualification, index) in newSubject.qualification" :key="index" class="space-y-2">
                <img :src="qualificationPreview[index]" class="w-64 h-32 object-cover rounded-lg border-gray-300" />
                <input v-model="qualification.degreeName" class="w-full border p-2 border-gray-300 rounded-lg"
                  placeholder="Tên bằng cấp">
                <input v-model="qualification.degreeNumber" class="w-full border p-2 border-gray-300 rounded-lg"
                  placeholder="Số bằng cấp">
                <input v-model="qualification.date" class="w-full p-2 border border-gray-300 rounded-lg"
                  placeholder="Ngày hiệu lực" type="date">
              </div>
            </div>
          </div>
          <div>
            <label for="description" class="block text-gray-700 font-bold mb-2">Mô tả</label>
            <textarea id="description" v-model="newSubject.description"
              class="w-full border rounded-lg border-gray-300 p-3 focus:ring-2 focus:ring-blue-200"></textarea>
          </div>
          <button type="submit" class="w-full p-3 font-bold text-white bg-green-500 hover:bg-green-400 rounded-lg">
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
    async selectSubject(subject) {
      await this.fetchTutorSubjectDetail(subject.id)
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
        this.selectedSubject = response.data;
      } catch (error) {
        console.error("Error fetching tutor subjects:", error);
      }
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

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
        <div class="text-gray-600 flex gap-2">
          Ngày đăng kí:
          <div>{{ formatDate(selectedSubject.createdDate) }}</div>
        </div>
        <div class="text-gray-600 flex gap-2">
          <div class="w-24">Mô tả:</div>
          <div v-if="editingSubject != selectedSubject.id">
            {{ selectedSubject.description || "Không có mô tả" }}
          </div>
          <textarea v-else class="w-full border-b focus:outline-none" v-model="selectedSubject.description"></textarea>
        </div>
        <div>
          Trạng thái:
          <span :class="getStatusStyle(selectedSubject.status)">{{ getStatus(selectedSubject.status) }}</span>
        </div>
        <div v-if="selectedSubject.status === 2">
          Lý do từ chối:
          <span class="text-red-500 font-bold">
            {{ selectedSubject.reasonReject || "Không có lý do" }}
          </span>
        </div>
        <div class="text-gray-800 font-semibold">Bằng cấp:</div>
        <div class="flex flex-col gap-2">
          <div v-for="(degree, index) in selectedSubject.degrees" :key="degree.id">
            <div v-if="editingSubject == selectedSubject.id" class="flex justify-end gap-4">
              <button @click="openFileDialogForUpdate(degree.id)"
                class="font-bold text-white rounded-lg px-4 py-1 bg-blue-400 hover:bg-blue-200">
                Cập nhật ảnh
              </button>
              <button @click="removeADegree(index)"
                class="font-bold text-white rounded-lg px-4 py-1 bg-red-500 hover:bg-red-200">
                Xóa bằng cấp
              </button>
            </div>
            <div class="flex gap-8">
              <img :src="degree.degreeImgUrl" class="w-1/3 object-cover rounded-lg border border-gray-200" />
              <div>
                <div class="flex gap-2">
                  <span class="w-32 font-bold">Tên bằng cấp : </span>
                  <span v-if="editingSubject != selectedSubject.id">{{ degree.tutorDegreeName }}</span>
                  <input v-else class="border-b focus:outline-none" v-model="degree.tutorDegreeName">
                </div>
                <div class="flex gap-2">
                  <span class="w-32 font-bold">Số bằng cấp : </span>
                  <span v-if="editingSubject != selectedSubject.id">{{ degree.degreeNumber }}</span>
                  <input v-else class="border-b focus:outline-none" v-model="degree.degreeNumber">
                </div>
                <div class="flex gap-2">
                  <span class="w-32 font-bold">Ngày cấp : </span>
                  <span v-if="editingSubject != selectedSubject.id">{{ degree.issuranceDate }}</span>
                  <input v-else type="date" class="border-b focus:outline-none" v-model="degree.issuranceDate">
                </div>
              </div>
              <input type="file" :ref="'fileUpdateInput' + degree.id"
                @change="handleQualificationUpdateUpload($event, degree.id)" class="hidden" accept="image/*" />
            </div>
            <hr class="mt-2">
          </div>

          <input type="file" ref="fileAddInput" multiple @change="handleQualificationUpdateUpload" class="hidden"
            accept="image/*" />
          <button class="bg-blue-500 hover:bg-blue-300 py-2 rounded-lg font-bold text-white my-4"
            v-if="editingSubject == selectedSubject.id" @click="openFileDialogForAddMore">
            Thêm bằng khác
          </button>
        </div>
        <div class="flex justify-end gap-4" v-if="selectedSubject.status < 3">
          <button class="font-bold text-white px-4 py-2 bg-yellow-400 hover:bg-yellow-200 rounded-lg"
            v-if="selectedSubject.status == 2">
            Gửi lại yêu cầu
          </button>
          <button class="font-bold text-white px-4 py-2 bg-blue-400 hover:bg-blue-200 rounded-lg"
            @click="editingSubject = selectedSubject.id" v-if="editingSubject != selectedSubject.id">
            Chỉnh sửa
          </button>
          <button v-if="editingSubject == selectedSubject.id" @click="updateSubject(true)"
            class="font-bold text-white px-4 py-2 bg-green-400 hover:bg-green-200 rounded-lg">
            Xác nhận
          </button>
          <button class="font-bold text-white px-4 py-2 bg-red-400 hover:bg-red-200 rounded-lg"
            @click="editingSubject = 0" v-if="editingSubject == selectedSubject.id">
            Hủy bỏ
          </button>
          <button class="font-bold text-white px-4 py-2 bg-red-500 hover:bg-blue-200 rounded-lg">Xóa</button>
        </div>

        <div v-if="selectedSubject.status === 3" class="flex justify-end gap-4">
          <button class="font-bold text-white px-4 py-2 bg-red-500 hover:bg-blue-200 rounded-lg">
            Không còn dậy môn này nữa
          </button>
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
      editingSubject: 0,
      qualificationPreview: [],
      videoPreview: null,
    };
  },
  methods: {
    openFileDialogForUpdate(id) {
      this.$refs['fileUpdateInput' + id][0].click(); // Trigger the file input click event for the specific degree
    },
    openFileDialogForAddMore() {
      this.$refs.fileAddInput.click()
    },
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
    handleQualificationUpdateUpload(event, updateId) {
      const files = event.target.files;
      if (files.length > 10) {
        alert("Bạn chỉ có thể tải lên tối đa 10 ảnh.");
        return;
      }
      if (updateId) {
        const degreeIndex = this.selectedSubject.degrees.findIndex(d => d.id = updateId)
        if (degreeIndex >= 0) {
          this.selectedSubject.degrees[degreeIndex].degreeImgUrl = URL.createObjectURL(files[0])
          this.selectedSubject.degrees[degreeIndex].newImgFile = files[0]
        }
      } else {
        for (var file of files) {
          this.selectedSubject.degrees.push({
            tutorDegreeName: "",
            degreeNumber: "",
            issuranceDate: null,
            degreeImgUrl: URL.createObjectURL(file),
            newImgFile: file
          })
        }

      }
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
    async updateSubject(option) {
      if (option) {
        this.eventBus.emit("open-confirmation-popup", {
          message: "Bạn có chắc chắn muốn xóa yêu cầu tư vấn này?",
          method: this.updateSubject,
          params: false
        })
      } else {
        this.eventBus.emit("open-loading-popup", {
          message: "Vui lòng chờ..."
        })
        try {
          for (const degree of this.selectedSubject.degrees) {
            if (degree.newImgFile) {
              const formData = new FormData();
              formData.append("file", degree.newImgFile);
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
              degree.degreeImgUrl = uploadDegree.data
            }
          }
          await axios.put(
            import.meta.env.VITE_API_URL + "/api/TutorSubject",
            {
              id: this.selectedSubject.id,
              description: this.selectedSubject.description,
              degrees: this.selectedSubject.degrees
            },
            {
              headers: {
                Authorization: "Bearer " + localStorage.token,
              },
            }
          );
          this.eventBus.emit("open-result-dialog", {
            message: "Gửi thành công",
            type: "Success"
          })
          this.editingSubject = 0
          await this.getSubjects();
        } catch (e) {
          console.log(e)
          this.eventBus.emit("open-result-dialog", {
            message: "Không thể gửi yêu cầu. Vui lòng thử lại sau",
            type: "Error"
          })
        }
        this.eventBus.emit("close-loading-popup")
      }
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
    getStatusStyle(status) {
      const css = "font-bold "
      switch (status) {
        case 0:
          return css + "text-gray-400";
        case 1:
          return css + "text-black";
        case 2:
          return css + "text-red-400";
        case 3:
          return css + "text-green-400";
        default:
          return css + "text-gray-500";
      }
    },
    getMillisecondsFromMinDate(date) {
      // The minimum date value is January 1, 1970, 00:00:00 UTC
      const minDate = new Date(0);
      return date.getTime() - minDate.getTime();
    },
    async getSubjects() {
      await this.fetchSubjects();
      await this.fetchAllSubjects();

      this.subjects = this.subjects.filter(s => !this.tutorSubjects.find(ts => ts.id == s.id))
    },
    removeADegree(index) {
      this.selectedSubject.degrees.splice(index, 1)

    },
  },
  mounted() {
    this.getSubjects()
  },
};
</script>

<style scoped>
/* Add your styles here */
</style>

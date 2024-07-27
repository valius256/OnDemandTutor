<template>
  <div>
    <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200">
      Thông tin cá nhân
    </div>
    <div class="flex justify-end" v-if="user">
      <div class="flex gap-4">
        <button @click="openEditMode" v-if="!editMode && checkOwner()"
          class="mr-6 p-2 font-bold text-white bg-blue-400 hover:bg-blue-200 rounded-lg">
          Chỉnh sửa
        </button>
        <!-- Button to send verification request -->
        <button @click="openVerificationDialog" v-if="!editMode"
          class="mr-6 p-2 font-bold text-white bg-yellow-400 hover:bg-yellow-200 rounded-lg">
          Gửi yêu cầu xác thực
        </button>
        <button @click="handleUpdate(true)" v-if="editMode"
          class="p-2 font-bold text-white bg-green-400 hover:bg-green-200 rounded-lg">
          Xác nhận
        </button>
        <button @click="closeEditMode" v-if="editMode"
          class="mr-6 p-2 font-bold text-white bg-red-400 hover:bg-red-200 rounded-lg">
          Hủy bỏ
        </button>
      </div>
    </div>
    <div class="flex gap-8 p-6" v-if="user">
      <div class="flex flex-col items-center">
        <img v-if="!this.file" class="max-w-64 min-w-64 h-64 rounded-full"
          :src="user.avatarImageUrl ?? '/src/assets/noavatar.jpg'" />
        <img v-else class="max-w-64 min-w-64 h-64 rounded-full" :src="imageBase64" />
        <button v-if="checkOwner() && !this.file"
          class="p-2 font-bold text-white bg-blue-400 hover:bg-blue-200 rounded-lg" @click="uploadImage">
          Cập nhật ảnh
        </button>
        <div v-if="checkOwner() && this.file" class="flex gap-4">
          <button class="p-2 font-bold text-white bg-green-400 hover:bg-green-200 rounded-lg"
            @click="handleChangeAvatar(true)">
            Xác nhận
          </button>
          <button v-if="checkOwner()" class="p-2 font-bold text-white bg-red-400 hover:bg-red-200 rounded-lg"
            @click="this.file = null">
            Hủy bỏ
          </button>
        </div>

        <input type="file" ref="fileInput" @change="onFileChange" class="hidden" accept="image/*" />
      </div>
      <table class="ml-4 bg-slate-50 p-6 rounded-xl w-full">
        <tbody v-if="!this.editMode">
          <tr>
            <td>Họ</td>
            <td>{{ user.firstName }}</td>
          </tr>
          <tr>
            <td>Tên</td>
            <td>{{ user.lastName }}</td>
          </tr>
          <tr>
            <td>Email</td>
            <td>{{ user.email }}</td>
          </tr>
          <tr>
            <td>Số điện thoại</td>
            <td>{{ user.phone }}</td>
          </tr>
          <tr>
            <td>Ngày sinh</td>
            <td>{{ user.dob?.substring(0, 10) }}</td>
          </tr>
          <tr>
            <td>Địa chỉ</td>
            <td>{{ user.address }}</td>
          </tr>
          <tr>
            <td>Giới tính</td>
            <td>{{ user.sex }}</td>
          </tr>
        </tbody>
        <tbody v-else>
          <tr>
            <td>First Name</td>
            <td>
              <input class="w-full rounded border border-gray-200 p-1" type="text" v-model="editDto.firstName" />
            </td>
          </tr>
          <tr>
            <td>Last Name</td>
            <td>
              <input class="w-full rounded border border-gray-200 p-1" type="text" v-model="editDto.lastName" />
            </td>
          </tr>
          <tr>
            <td>Email</td>
            <td>{{ user.email }}</td>
          </tr>
          <tr>
            <td>Phone</td>
            <td>
              <input class="w-full rounded border border-gray-200 p-1" type="text" v-model="editDto.phone" />
            </td>
          </tr>
          <tr>
            <td>Date of Birth</td>
            <td>
              <input class="w-full rounded border border-gray-200 p-1" type="date" v-model="editDto.dob" />
            </td>
          </tr>
          <tr>
            <td>Address</td>
            <td>
              <input class="w-full rounded border border-gray-200 p-1" type="text" v-model="editDto.address" />
            </td>
          </tr>
          <tr>
            <td>Gender</td>
            <td>
              <select class="w-full rounded border border-gray-200 p-1" v-model="editDto.gender">
                <option :value="1">Male</option>
                <option :value="0">Female</option>
                <option :value="2">Other</option>
              </select>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div class="bg-white shadow-md rounded-lg p-6 w-full mt-8">
      <h2 class="text-2xl font-semibold mb-4">Đánh giá từ các học viên trước</h2>
      <div class="flex">
        <button @click="setFeedbackMode(0)" class="rounded-lg w-full p-2" :class="{ 'bg-gray-300': feedbackMode == 0 }">
          Đánh giá buổi học
        </button>
        <button @click="setFeedbackMode(1)" class="rounded-lg w-full p-2" :class="{ 'bg-gray-300': feedbackMode == 1 }">
          Đánh giá lớp học
        </button>
      </div>
      <div v-if="feedbackMode == 0 && studentSlots">
        <div  v-for="slot in studentSlots.slotStudents" :key="slot.id">
          <div class="p-4 flex gap-4">
            <img class="w-24 h-24 rounded-full" :src="slot.user.avatarImageUrl">
            <div>
              <div class="font-bold">{{ (slot.user.firstName ?? "") + " " + (slot.user.lastName ?? "") }}</div>
              <star-rating :star-size="20" :rating="slot.rating" :round-start-rating="false" :read-only="true" />
              <div class="mt-2">
                {{ slot.feedback }}
              </div>
            </div>
          </div>
          <hr>
        </div>

      </div>
      <div v-if="feedbackMode == 1 && studentClasses">
        <div v-for="class_ in studentClasses.studentClasses" :key="class_.id">
          <div class="p-4 flex gap-4">
            <img class="w-24 h-24 rounded-full" :src="class_.student.avatarImageUrl">
            <div>
              <div class="font-bold">{{ (class_.student.firstName ?? "") + " " + (class_.student.lastName ?? "") }}
              </div>
              <star-rating :star-size="20" :rating="class_.student.rating" :round-start-rating="false"
                :read-only="true" />
              <div class="mt-2" v-if="class_.student.feedback">
                {{ class_.student.feedback }}
              </div>
            </div>
          </div>
          <hr>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "TutorProfile",
  inject: ["eventBus"],
  props: ["id"],
  data() {
    return {
      user: null,
      loginedUser: null,
      editDto: {
        firstName: "",
        lastName: "",
        phone: "",
        dob: "",
        address: "",
        gender: 0,
      },
      editMode: false,
      imageBase64: null,
      file: null,
      totalPage: 100,
      pageSize: 5,
      currentPage: 1,
      feedbackMode: 0,
      currentUser: null,
      studentSlots: [],
      studentClasses: [],
    };
  },
  methods: {
    closeEditMode() {
      this.editMode = false;
    },
    openEditMode() {
      this.editMode = true;
      this.editDto.firstName = this.user.firstName;
      this.editDto.lastName = this.user.lastName;
      this.editDto.phone = this.user.phone;
      this.editDto.dob = this.user.dob;
      this.editDto.address = this.user.address;
      this.editDto.gender =
        this.user.sex == "Male" ? 1 : this.user.sex == "Female" ? 0 : 2;
    },
    async refresh() {
      this.loginedUser = await this.getUserFromToken();
      const response = await axios.get(
        import.meta.env.VITE_API_URL + "/api/User/profile?userId=" + this.id,
        {
          headers: {
            Authorization: "Bearer " + localStorage.token,
          },
        }
      );
      if (response.data) {
        this.user = response.data.data;
      }
      this.feedbackMode = 0;
      await this.handlePageChange()
    },
    async handleUpdate(confirmation) {
      if (confirmation) {
        this.eventBus.emit("open-confirmation-popup", {
          message: "Bạn có chắc chắn muốn cập nhật thông tin hồ sơ?",
          method: this.handleUpdate,
          params: false,
        });
      } else {
        const request = {
          id: this.user.id,
          firstName: this.editDto.firstName,
          lastName: this.editDto.lastName,
          address: this.editDto.address,
          phone: this.editDto.phone,
          sex: this.editDto.gender,
          dob: this.editDto.dob,
        };
        this.eventBus.emit("open-loading-popup", {
          message: "Vui lòng chờ...",
        });
        try {
          await axios.post(
            import.meta.env.VITE_API_URL + "/api/User/update-profile",
            request,
            {
              headers: {
                Authorization: "Bearer " + localStorage.token,
              },
            }
          );
          this.eventBus.emit("open-result-dialog", {
            message: "Cập nhật thành công",
            type: "Success",
          });
          await this.refresh();
          this.eventBus.emit("update-everything");
          this.closeEditMode();
        } catch (e) {
          console.log(e);
          this.eventBus.emit("open-result-dialog", {
            message: "Có sự cố xảy ra. Vui lòng thử lại sau",
            type: "Error",
          });
        }
        this.eventBus.emit("close-loading-popup");
      }
    },
    checkOwner() {
      if (this.loginedUser == null || this.user == null) return false;
      if (this.loginedUser.id == this.user.id) return true;
      return false;
    },
    openVerificationDialog() {
      this.eventBus.emit("open-confirmation-popup", {
        message: "Bạn có chắc chắn muốn gửi yêu cầu xác thực?",
        method: this.sendVerificationRequest,
        params: false,
      });
    },
    async sendVerificationRequest() {
      this.eventBus.emit("open-loading-popup", {
        message: "Đang gửi yêu cầu xác thực...",
      });
      try {
        await axios.patch(
          //TODO: Change the API URL
          import.meta.env.VITE_API_URL + "/api/User/change-status",
          { id: this.user.id, status : 1 },
          {
            headers: {
              Authorization: "Bearer " + localStorage.token,
            },
          }
        );
        this.eventBus.emit("open-result-dialog", {
          message: "Yêu cầu xác thực đã được gửi thành công",
          type: "Success",
        });
      } catch (e) {
        console.log(e);
        this.eventBus.emit("open-result-dialog", {
          message: "Đã xảy ra sự cố. Vui lòng thử lại sau",
          type: "Error",
        });
      }
      this.eventBus.emit("close-loading-popup");
    },
    async uploadImage() {
      this.$refs.fileInput.click();
    },
    onFileChange(event) {
      this.file = event.target.files[0];
      console.log(this.file);
      this.convertToBase64();
    },
    async handleChangeAvatar(confirmation) {
      if (confirmation) {
        this.eventBus.emit("open-confirmation-popup", {
          message: "Bạn có chắc chắn muốn cập nhật ảnh hồ sơ chứ?",
          method: this.handleChangeAvatar,
          params: false,
        });
      } else {
        this.eventBus.emit("open-loading-popup", {
          message: "Vui lòng chờ...",
        });
        try {
          if (this.file) {
          }
          const fileName =
            "Avartar_of_" +
            this.user.id +
            "_" +
            this.getMillisecondsFromMinDate(new Date());
          const formData = new FormData();
          formData.append("file", this.file);
          const response = await axios.post(
            import.meta.env.VITE_API_URL +
            "/api/Upload/upload-image?fileName=" +
            fileName,
            formData,
            {
              headers: {
                Authorization: "Bearer " + localStorage.token,
                "Content-Type": "multipart/form-data",
              },
            }
          );
          console.log(response.data);
          await axios.post(
            import.meta.env.VITE_API_URL + "/api/User/update-avatar",
            {
              url: response.data,
            },
            {
              headers: {
                Authorization: "Bearer " + localStorage.token,
              },
            }
          );
          await this.refresh();
          this.file = null;
          this.eventBus.emit("update-everything");
          this.eventBus.emit("open-result-dialog", {
            message: "Cập nhật thành công",
            type: "Success",
          });
        } catch (e) {
          console.log(e);
          this.eventBus.emit("open-result-dialog", {
            message: "Đã xảy ra sự cố. Vui lòng thử lại sau",
            type: "Error",
          });
        }
        this.eventBus.emit("close-loading-popup");
      }
    },
    getMillisecondsFromMinDate(date) {
      // The minimum date value is January 1, 1970, 00:00:00 UTC
      const minDate = new Date(0);
      return date.getTime() - minDate.getTime();
    },
    convertToBase64() {
      const reader = new FileReader();

      reader.onload = (event) => {
        this.imageBase64 = event.target.result;
      };

      reader.readAsDataURL(this.file);
    },
    async fetchClassFeedback() {
      const response = await axios.get(
        `${import.meta.env.VITE_API_URL
        }/api/Class/tutor-class-student?TutorId=${this.id}&Page=${this.currentPage}&Limit=${this.pageSize}`
      )
      if (response.data && response.data.items) {
        this.studentClasses = response.data.items[0];
        this.totalPage = Math.ceil(response.data.total / this.pageSize)
      }
    },
    async fetchSlotFeedback() {
      const response = await axios.get(
        `${import.meta.env.VITE_API_URL
        }/api/Slot/tutor-slot-student?TutorId=${this.id}&Page=${this.currentPage}&Limit=${this.pageSize}`
      )
      if (response.data && response.data.items) {
        this.studentSlots = response.data.items[0];
        this.totalPage = Math.ceil(response.data.total / this.pageSize)
      }
    },
    async handlePageChange() {
      if (this.currentPage > this.totalPage) {
        this.currentPage = this.totalPage
      }
      if (this.currentPage < 1) {
        this.currentPage = 1
      }
      if (this.feedbackMode == 0) {
        await this.fetchSlotFeedback()
      } else {
        await this.fetchClassFeedback()
      }
    },
    async movePage(forward) {
      if (forward && this.currentPage < this.totalPage) {
        this.currentPage++
        await this.handlePageChange()
      } else if (!forward && this.currentPage > 1) {
        this.currentPage--
        await this.handlePageChange()
      }
    },
    async setFeedbackMode(mode) {
      this.feedbackMode = mode
      this.currentPage = 0
      await this.handlePageChange()
    }
  },
  mounted() {
    this.refresh();
  },
};
</script>

<style scoped>
tr td,
th {
  padding: 0.5rem 2rem 0.5rem 2rem;
  border: solid 1px #ffffff;
}
</style>

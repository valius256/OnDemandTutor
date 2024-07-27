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
        <button @click="openVerificationDialog" v-if="!editMode && canSendVerificationRequest()" :class="[
      'mr-6 p-2 font-bold text-white rounded-lg',
      getVerificationButtonClass(),
    ]">
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
          <tr>
            <td>Giá dạy mỗi giờ</td>
            <td>{{ user.tutorFeePerHour }}</td>
          </tr>
          <tr>
            <td>Lịch dạy</td>
            <td>{{ user.scheduleDescription }}</td>
          </tr>
          <tr>
            <td>Trạng thái gia sư</td>
            <td>{{ getTutorStatusText() }}</td>
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
          <tr>
            <td>Giá dạy mỗi giờ</td>
            <td>
              <input class="w-full rounded border border-gray-200 p-1" type="number"
                v-model="editDto.tutorFeePerHour" />
            </td>
          </tr>
          <tr>
            <td>Lịch dạy</td>
            <td>
              <textarea class="w-full rounded border border-gray-200 p-1"
                v-model="editDto.scheduleDescription"></textarea>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <previous-feedback :tutorId="id" />
  </div>
</template>

<script>
import axios from "axios";
import PreviousFeedback from './PreviousFeedback.vue';

export default {
  components: { PreviousFeedback },
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
        tutorFeePerHour: 0,
        scheduleDescription: "",
      },
      editMode: false,
      imageBase64: null,
      file: null,
      currentUser: null,
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
      this.editDto.tutorFeePerHour = this.user.tutorFeePerHour;
      this.editDto.scheduleDescription = this.user.scheduleDescription;
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
          tutorFeePerHour: this.editDto.tutorFeePerHour,
          scheduleDescription: this.editDto.scheduleDescription,
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
    canSendVerificationRequest() {
      return [0, 2].includes(this.user.tutorStatus);
    },
    getVerificationButtonClass() {
      if (this.user.tutorStatus === 1) return "bg-gray-400 hover:bg-gray-200";
      return "bg-yellow-400 hover:bg-yellow-200";
    },
    getTutorStatusText() {
      switch (this.user.tutorStatus) {
        case 0:
          return "Unverified";
        case 1:
          return "Verification Request Sent";
        case 2:
          return "Verification Request Rejected";
        case 3:
          return "Verified";
        case -4:
          return "Banned";
        default:
          return "Unknown Status";
      }
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
          { id: this.user.id, status: 1 },
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
        await this.refresh();
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

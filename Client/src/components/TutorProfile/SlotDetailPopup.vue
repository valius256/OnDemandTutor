<template>
  <div class="p-4 bg-white rounded-b-lg flex flex-col w-screen lg:w-auto">
    <div class="flex gap-4 justify-center">
      <button v-if="!isEdit && slot.slotStatus == 0" @click="toggleEditMode"
        class="px-4 py-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Chỉnh sửa</button>
      <button v-if="isEdit && slot.slotStatus == 0" @click="updateSlot"
        class="px-4 py-2 bg-green-400 hover:bg-green-200 font-bold text-white rounded-lg">Xác nhận</button>
      <button v-if="isEdit && slot.slotStatus == 0" @click="toggleEditMode"
        class="px-4 py-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy bỏ</button>
      <button v-if="slot.slotStatus == 0 && !slot.classId" @click="handleCancel" class="px-4 py-2 bg-red-600 hover:bg-red-300 font-bold text-white rounded-lg">Hủy Slot</button>
      <button v-if="slot.slotStatus == 2" @click="handleCancel" class="px-4 py-2 bg-green-600 hover:bg-green-300 font-bold text-white rounded-lg">Mở lại Slot</button>
      <button v-if="slot.slotStatus == 2" @click="handleDelete" class="px-4 py-2 bg-red-600 hover:bg-red-300 font-bold text-white rounded-lg">Xóa Slot</button>
    </div>
    <div v-if="!isEdit">
      <div class="flex gap-4 w-full">
        <div class="w-full">
          <div class="mb-8" v-if="slot.class">
            <div>
              <span class="font-bold">Tên lớp :</span>
              <span class="ml-4">{{ slot.class.name }}</span>
            </div>
            <hr />
          </div>
          <div class="flex place-content-between">
            <div>
              <span class="font-bold">Môn học :</span>
              <span class="font-bold text-blue-400 ml-4">
                {{ slot.subject?.name }}
              </span>
            </div>
            <div>
              <span :class="getSlotStyle(slot).style">
                {{ getSlotStyle(slot).display }}
              </span>
            </div>
          </div>

          <hr />
          <div class="mt-8 gap-8 flex place-content-between">

            <div>
              <div class="">
                <span class="font-bold">Bắt đầu :</span>
                <span class="ml-4">{{ beautifyDatetime(slot.startTime) }}</span>
              </div>
              <div>
                <span class="font-bold">Kết thúc :</span>
                <span class="ml-4">{{ beautifyDatetime(slot.endTime) }}</span>
              </div>
              <div>
                <span class="font-bold">Tổng thời lượng :</span>
                <span class="ml-4">{{ calcDuration() }} tiếng</span>
              </div>
            </div>

            <div>
              <div class="">
                <span class="font-bold">Địa điểm :</span>
                <span class="ml-4">{{ slot.teachAddress }}</span>
              </div>
              <div class="">
                <span class="font-bold">Phương thức :</span>
                <span v-if="slot.isOnline" class="ml-4 font-bold text-green-500">Online</span>
                <span v-else class="ml-4 font-bold text-gray-500">Offline</span>
              </div>
              <div class="">
                <span class="font-bold">Số học sinh :</span>
                <span class="ml-4">{{ totalStudent }} / {{ slot.numberOfStudents }}</span>
              </div>
            </div>
          </div>

          <hr />

        </div>
      </div>
      <hr />
      <div class="mt-4">
        <div class="max-h-64 overflow-x-auto overflow-y-auto relative ">
          <table class="mt-2 text-center">
            <thead class="border-b sticky top-0 z-10 bg-white bg-opacity-50">
              <tr>
                <th class="max-w-[16rem] min-w-[16rem] ">Học sinh</th>
                <th class="max-w-[8rem] min-w-[8rem] ">Email</th>
                <th class="max-w-[8rem] min-w-[8rem] ">SDT</th>
                <th v-if="!slot.class" class="max-w-[16rem] min-w-[16rem] ">Đánh giá</th>
                <th v-if="slot.class" class="max-w-[16rem] min-w-[16rem] ">Thanh toán</th>
              </tr>
            </thead>
            <tbody class="">
              <tr v-for="student in slotStudents" :key="student.user.id">
                <td class="p-2">
                  <div class="flex gap-2 items-center">
                    <img class="w-16 h-16 rounded-full" :src="student.user.avatarImageUrl">
                    <span class="font-bold">{{ (student.user.firstName ?? "") + " " + (student.user.lastName ?? "")
                      }}</span>
                  </div>

                </td>
                <td class="p-2">{{ student.user.email }}</td>
                <td class="p-2">{{ student.user.phone }}</td>
                <td v-if="!slot.class" class="w-48 text-wrap">
                  <star-rating class="flex justify-center" :star-size="20" :rating="student.rating"
                    :round-start-rating="false" :read-only="true" />
                  {{ student.feedback }}
                </td>
                <td v-if="slot.class" class="font-bold"
                  :class="{ 'text-red-400': student.paymentStatus == 0, 'text-blue-400': student.paymentStatus == 1 }">
                  {{ student.paymentStatus == 1 ? "Đã thanh toán" : "Chưa thanh toán" }}
                </td>
              </tr>

            </tbody>
          </table>
        </div>

        <div class="flex gap-4 justify-center mt-4" v-if="this.slotStudents.length > 0">
          <button @click="movePage(false)">
            <i class="fa fa-arrow-left text-2xl"></i>
          </button>
          <div class="flex gap-2 ">
            <input class="border p-1 rounded-md w-16" type="number" v-model="currentPage" min="1"
              @change="handlePageChange">
            <div class="p-1"> / {{ this.totalPage }}</div>
          </div>
          <button @click="movePage(true)">
            <i class="fa fa-arrow-right text-2xl"></i>
          </button>
        </div>
      </div>
    </div>
    <div v-else>
      <div class="flex place-content-between gap-4 mt-4">
        <div>
          <div class="flex items-center gap-4">
            <div class="w-24">Ngày :</div>
            <input v-model="editingDate" class="p-1 border rounded-lg" type="date">
          </div>
          <div class="flex items-center gap-4 mt-2">
            <div class="w-24">Giờ bắt đầu :</div>
            <input v-model="editingStartHour" class="p-1 border rounded-lg" type="time">
          </div>
          <div class="flex items-center gap-4 mt-2">
            <div class="w-24">Giờ kết thúc :</div>
            <input v-model="editingEndHour" class="p-1 border rounded-lg" type="time">
          </div>
        </div>
        <div>
          <div class="flex items-center gap-4">
            <div class="w-28">Môn học :</div>
            <select v-model="editDto.subjectId" class="p-1 border rounded-lg">
              <option v-for="tutorSubject in subjects" :key="tutorSubject.id" :value="tutorSubject.subject.id">
                {{ tutorSubject.subject.name }}
              </option>
            </select>
          </div>
          <div class="flex items-center gap-4 mt-2">
            <div class="w-28">Địa điểm :</div>
            <input v-model="editDto.teachAddress" class="p-1 border rounded-lg" type="text">
          </div>
          <div class="flex items-center gap-4 mt-2">
            <div class="w-28">Phương thức :</div>
            <select v-model="editDto.isOnline" class="p-1 border rounded-lg">
              <option :value="true">Online</option>
              <option :value="false">Offline</option> />
            </select>
          </div>
          <div class="flex items-center gap-4 mt-2">
            <div class="w-28">Số học sinh :</div>
            <input v-model="editDto.numberOfStudents" class="p-1 border rounded-lg" type="number" min="1">
          </div>
        </div>
      </div>
    </div>

  </div>
</template>

<script>
import axios from "axios";
import StarRating from 'vue-star-rating'

export default {
  name: "SlotDetailPopup",
  props: ["slot", "close", "tutorId", "refresh"],
  inject: ['eventBus'],
  components: { StarRating },
  data() {
    return {
      totalPage: 100,
      pageSize: 5,
      currentPage: 1,
      slotStudents: [],
      subjects: [],
      totalStudent: 0,
      isEdit: false,
      editingDate: null,
      editingStartHour: null,
      editingEndHour: null,
      editDto: {
        id: 0,
        startTime: null,
        endTime: null,
        teachAddress: "",
        subjectId: 0,
        isOnline: false,
        numberOfStudents: 1
      }
    };
  },
  methods: {
    async fetchSubjects() {
      try {
        const response = await axios.get(
          import.meta.env.VITE_API_URL + "/api/TutorSubject",
          {
            params: {
              "Filter.TutorId": this.tutorId,
              Status: 3, Page: 1, Limit: 100
            },
            headers: {
              Authorization: "Bearer " + localStorage.token,
            },
          }
        );
        // Map the response to the desired structure
        this.subjects = response.data.items
      } catch (error) {
        console.error("Error fetching subjects:", error);
      }
    },
    formatDatetime(date, time) {
      if (date && time) {
        return `${date} ${time}:00`;
      }
      return '';
    },
    setupEdit() {
      this.editDto.id = this.slot.id
      this.editDto.teachAddress = this.slot.teachAddress
      this.editDto.subjectId = this.slot.subjectId
      this.editDto.numberOfStudents = this.slot.numberOfStudents
      this.editDto.isOnline = this.slot.isOnline

      const startTime = new Date(this.slot.startTime)
      const endTime = new Date(this.slot.endTime)
      this.editingDate = this.toSqlDateString(startTime)
      this.editingStartHour = `${startTime.getHours().toString().padStart(2, '0')}:${startTime.getMinutes().toString().padStart(2, '0')}`
      this.editingEndHour = `${endTime.getHours().toString().padStart(2, '0')}:${endTime.getMinutes().toString().padStart(2, '0')}`
      console.log(this.editingEndHour)
      this.editDto.startTime = this.formatDatetime(this.editingDate, this.editingStartHour)
      this.editDto.endTime = this.formatDatetime(this.editingDate, this.editingEndHour)
    },
    toggleEditMode() {
      if (this.isEdit) {
        this.isEdit = false
      } else {
        this.setupEdit()
        this.isEdit = true
      }
    },
    calcDuration() {
      const startTime = new Date(this.slot.startTime);
      const endTime = new Date(this.slot.endTime);
      return (endTime - startTime) / 3600000;
    },
    async fetchSlotStudents() {
      if (this.slot.id) {
        try {
          const response = await axios.get(
            `${import.meta.env.VITE_API_URL}/api/SlotStudent/${this.slot.id}?page=${this.currentPage}&limit=${this.pageSize}`
          );
          this.slotStudents = response.data.items;
          this.totalStudent = response.data.total
          this.totalPage = Math.ceil(this.totalStudent / this.pageSize)
        } catch (error) {
          console.error("Error fetching slot students:", error);
        }
      }
    },
    async handlePageChange() {
      if (this.currentPage > this.totalPage) {
        this.currentPage = this.totalPage
      }
      if (this.currentPage < 1) {
        this.currentPage = 1
      }
      await this.fetchSlotStudents()
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
    getSlotStyle(slot) {
      let bg = "font-bold ";
      let display = "";
      if (slot.slotStatus == 0) {
        bg += "text-gray-300";
        display = "Sắp diễn ra"
      } else if (slot.slotStatus == 1) {
        bg += "text-green-400";
        display = "Đang diễn ra"
      } else if (slot.slotStatus == 2) {
        bg += "text-red";
        display = "Đã hủy"
      } else if (slot.slotStatus == 3) {
        bg += "text-blue-400";
        display = "Đã hoàn tất"
      }
      return {
        style: bg, display: display
      };
    },
    async updateSlot() {
      try {
        this.eventBus.emit("open-loading-popup", {
          message: "Vui lòng chờ..."
        })
        // Set the tutor ID
        this.editDto.startTime = this.formatDatetime(this.editingDate, this.editingStartHour)
        this.editDto.endTime = this.formatDatetime(this.editingDate, this.editingEndHour)
        await axios.put(
          import.meta.env.VITE_API_URL + "/api/Slot",
          this.editDto,
          {
            headers: {
              Authorization: "Bearer " + localStorage.token,
            },
          }
        );
        this.eventBus.emit("open-result-dialog", {
          message: "Cập nhật Slot thành công",
          type: "Success"
        })
        this.toggleEditMode()
        this.refresh()
      } catch (error) {
        console.error("Error adding slot:", error);
        var message = error.response.data?.errors[0]?.errorMessage
        let displayMessage = ""
        if (!message) {
          displayMessage = "Có sự cố xảy ra. Vui lòng thử lại sau!"
        } else if (message.includes("conflict")) {
          displayMessage = "Đã trùng lặp với 1 slot trước đó, vui lòng kiểm tra lại"
        } else if (message.includes("15")) {
          displayMessage = "Slot có thời lượng tối thiểu 15 phút"
        } else if (message.includes("4")) {
          displayMessage = "Slot có thời lượng tối đa 4 tiếng"
        } else if (message.includes("smaller")) {
          displayMessage = "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc"
        } else if (message.includes("future")) {
          displayMessage = "Thời gian bắt đầu phải trong tương lai"
        } else if (message.includes("student")) {
          displayMessage = "Số lượng học sinh hạn mức phải từ 1 đến 100"
        }
        this.eventBus.emit("open-result-dialog", {
          message: displayMessage,
          type: "Error"
        })
      }
      this.eventBus.emit("close-loading-popup")
    },
    async handleCancel(confirmation) {
      const message = this.slot.slotStatus == 2 ? "Bạn có chắc chắn muốn mở lại buổi học này?" : "Bạn có chắc chắn muốn hủy buổi học này?"
      if (confirmation) {
        this.eventBus.emit("open-confirmation-popup", {
          message: message,
          method: this.handleCancel,
          params: false
        })
      } else {
        this.eventBus.emit("open-loading-popup", {
          message: "Vui lòng chờ..."
        })
        try {
          await axios.put(import.meta.env.VITE_API_URL + '/api/Slot/' + this.slot.id + '/cancel', null, {
            headers: {
              "Authorization": "Bearer " + localStorage.token
            }
          })
          this.eventBus.emit("open-result-dialog", {
            message: "Cập nhật thành công",
            type: "Success"
          })
          await this.refresh()
          this.close()
        } catch (e) {
          console.log(e)
          this.eventBus.emit("open-result-dialog", {
            message: "Không thể thực hiện. Vui lòng thử lại sau",
            type: "Error"
          })
        }
        this.eventBus.emit("close-loading-popup")
      }
    },
    async handleDelete(confirmation) {
      if (confirmation) {
        this.eventBus.emit("open-confirmation-popup", {
          message: "Bạn có chắc chắn muốn xóa vĩnh viễn slot học này. Toàn bộ tiền học sẽ được trả lại cho các học viên và không thể hoàn tác?",
          method: this.handleDelete,
          params: false
        })
      } else {
        this.eventBus.emit("open-loading-popup", {
          message: "Vui lòng chờ..."
        })
        try {
          await axios.delete(import.meta.env.VITE_API_URL + '/api/Slot/' + this.slot.id, {
            headers: {
              "Authorization": "Bearer " + localStorage.token
            }
          })
          this.eventBus.emit("open-result-dialog", {
            message: "Xóa thành công",
            type: "Success"
          })
          await this.refresh()
          this.close()
        } catch (e) {
          console.log(e)
          this.eventBus.emit("open-result-dialog", {
            message: "Không thể thực hiện. Vui lòng thử lại sau",
            type: "Error"
          })
        }
        this.eventBus.emit("close-loading-popup")
      }
    }
  },
  mounted() {
    this.fetchSubjects()
    this.fetchSlotStudents();
  },
};
</script>

<style></style>

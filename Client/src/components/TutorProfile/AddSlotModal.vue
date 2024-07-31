<template>
  <GenericPopup v-if="showModal" :title="'Thêm slot mới'" :closeFunction="closeModal">
    <div class="modal-body bg-white p-4">
      <form @submit.prevent="addSlot">
        <div class="flex justify-center gap-4">
          <div>
            <div class="form-group mb-4">
              <label for="date" class="block font-bold">Ngày</label>
              <input type="date" id="date" v-model="date" required class="w-full p-2 border border-gray-300 rounded" />
            </div>
            <div class="form-group mb-4">
              <label for="startTime" class="block font-bold">Bắt đầu:</label>
              <input type="time" id="startTime" v-model="startHour" required
                class="w-full p-2 border border-gray-300 rounded" />
            </div>
            <div class="form-group mb-4">
              <label for="endTime" class="block font-bold">Kết thúc:</label>
              <input type="time" id="endTime" v-model="endHour" required
                class="w-full p-2 border border-gray-300 rounded" />
            </div>
          </div>
          <div>
            <div class="form-group mb-4">
              <label for="teachAddress" class="block font-bold">Địa chỉ dạy:</label>
              <input type="text" id="teachAddress" v-model="newSlot.teachAddress"
                class="w-full p-2 border border-gray-300 rounded" />
            </div>
            <div class="form-group mb-4">
              <label for="numberOfStudents" class="block font-bold">Số lượng học sinh hạn mức:</label>
              <input type="number" min="1" id="numberOfStudents" v-model="newSlot.numberOfStudents" required
                class="w-full p-2 border border-gray-300 rounded" />
            </div>
            <div class="form-group mb-4">
              <label for="subjectId" class="block font-bold">Môn học:</label>
              <select id="subjectId" v-model="newSlot.subjectId" required
                class="w-full p-2 border border-gray-300 rounded">
                <option v-for="subject in subjects" :value="subject.id" :key="subject.id">
                  {{ subject.name }}
                </option>
              </select>
            </div>
            <div class="form-group mb-4 flex gap-8">
              <label for="isOnline" class="block font-bold">Học online:</label>
              <input type="checkbox" id="isOnline" v-model="newSlot.isOnline" class="mt-1" />
            </div>
          </div>
        </div>
        <button class="bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg py-2 w-full" type="submit">Thêm
          slot</button>
      </form>
    </div>

  </GenericPopup>
</template>

<script>
import axios from "axios";
import GenericPopup from "../common/GenericPopup.vue";

export default {
  components: {
    GenericPopup,
  },
  props: ["showModal", "currentUser"],
  inject: ['eventBus'],
  data() {
    return {
      date: "",
      startHour: "",
      endHour: "",
      newSlot: {
        startTime: "",
        endTime: "",
        teachAddress: "",
        numberOfStudents: 1,
        subjectId: null,
        isOnline: false,
      },
      subjects: [], // Array to hold subjects
    };
  },
  methods: {
    closeModal() {
      this.$emit("close");
    },
    formatDatetime(date, time) {
      if (date && time) {
        return `${date} ${time}:00`;
      }
      return '';
    },
    async addSlot() {
      try {
        this.eventBus.emit("open-loading-popup", {
          message: "Vui lòng chờ..."
        })
        // Set the tutor ID
        this.newSlot.startTime = this.formatDatetime(this.date, this.startHour)
        this.newSlot.endTime = this.formatDatetime(this.date, this.endHour)
        console.log(this.newSlot);
        const response = await axios.post(
          import.meta.env.VITE_API_URL + "/api/Slot",
          this.newSlot,
          {
            headers: {
              Authorization: "Bearer " + localStorage.token,
            },
          }
        );
        this.$emit("add", response.data);
        // Reset the form after successful submission
        this.closeModal();
        this.eventBus.emit("open-result-dialog", {
          message: "Tạo Slot thành công",
          type: "Success"
        })
      } catch (error) {
        console.error("Error adding slot:", error);
        var message = error.response.data?.errors[0]?.errorMessage
        console.log(message)
        let displayMessage = ""
        if (!message){
          displayMessage = "Có sự cố xảy ra. Vui lòng thử lại sau!"
        } else if (message.includes("conflict")){
          displayMessage = "Đã trùng lặp với 1 slot trước đó, vui lòng kiểm tra lại"
        }else if (message.includes("15")){
          displayMessage = "Slot có thời lượng tối thiểu 15 phút"
        }else if (message.includes("4")){
          displayMessage = "Slot có thời lượng tối đa 4 tiếng"
        }else if (message.includes("smaller")){
          displayMessage = "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc"
        }else if (message.includes("future")){
          displayMessage = "Thời gian bắt đầu phải trong tương lai"
        }else if (message.includes("student")){
          displayMessage = "Số lượng học sinh hạn mức phải từ 1 đến 100"
        }
        this.eventBus.emit("open-result-dialog", {
          message: displayMessage,
          type: "Error"
        })
      }
      this.eventBus.emit("close-loading-popup")
    },
    async fetchSubjects() {
      try {
        const tutorName = `${this.currentUser.firstName} ${this.currentUser.lastName}`;
        const response = await axios.get(
          import.meta.env.VITE_API_URL + "/api/TutorSubject",
          {
            params: {
              "Filter.TutorName": tutorName,
              Status: 3,
              "Sorts[column]": "string",
              "Sorts[isDesc]": true,
            },
            headers: {
              Authorization: "Bearer " + localStorage.token,
            },
          }
        );

        // Map the response to the desired structure
        this.subjects = response.data.items.map((item) => ({
          id: item.subject.id,
          name: item.subject.name,
        }));
      } catch (error) {
        console.error("Error fetching subjects:", error);
      }
    },
  },
  mounted() {
    this.fetchSubjects();
  },
  watch: {
    showModal(newVal) {
      if (newVal) {
        this.fetchSubjects();
      }
    },
  },
};
</script>

<style scoped></style>

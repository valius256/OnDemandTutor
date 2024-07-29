<template>
  <GenericPopup v-if="showModal" :title="'Thêm slot mới'" :closeFunction="closeModal">
    <div class="modal-body bg-white p-4">
      <form @submit.prevent="addSlot">
        <div class="flex justify-center gap-4">
          <div>
            <div class="form-group mb-4">
              <label for="date" class="block font-bold">Ngày</label>
              <input type="date" id="date" v-model="newSlot.startTime" required
                class="w-full p-2 border border-gray-300 rounded" />
            </div>
            <div class="form-group mb-4">
              <label for="startTime" class="block font-bold">Bắt đầu:</label>
              <input type="time" id="startTime" v-model="newSlot.startTime" required
                class="w-full p-2 border border-gray-300 rounded" />
            </div>
            <div class="form-group mb-4">
              <label for="endTime" class="block font-bold">Kết thúc:</label>
              <input type="time" id="endTime" v-model="newSlot.endTime" required
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
              <input type="number" id="numberOfStudents" v-model="newSlot.numberOfStudents" required
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
        <button class="bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg py-2 w-full" type="submit">Thêm slot</button>
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
  data() {
    return {
      newSlot: {
        startTime: "",
        endTime: "",
        teachAddress: "",
        numberOfStudents: 0,
        subjectId: null,
        isOnline: false,
        createById: this.currentUser.id, // Replace with the actual tutor ID
        classId: null, // Replace with actual class ID if needed
        actualEndTime: "", // This could be dynamically set or calculated if needed
      },
      subjects: [], // Array to hold subjects
    };
  },
  methods: {
    closeModal() {
      this.$emit("close");
    },
    async addSlot() {
      try {
        // Set the tutor ID
        this.newSlot.createById = this.currentUser.id;

        // Ensure actualEndTime matches endTime if it is not provided by the user
        if (!this.newSlot.actualEndTime) {
          this.newSlot.actualEndTime = this.newSlot.endTime;
        }
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
        this.newSlot = {
          startTime: "",
          endTime: "",
          teachAddress: "",
          numberOfStudents: 0,
          subjectId: null,
          isOnline: false,
          createById: 0,
          classId: null,
          actualEndTime: "",
          //paymentStatus: 0,
        };

        this.closeModal();
      } catch (error) {
        console.error("Error adding slot:", error);
      }
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

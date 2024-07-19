<template>
  <GenericPopup
    v-if="showModal"
    :title="'Thêm slot mới'"
    :closeFunction="closeModal"
  >
    <div class="modal-body">
      <form @submit.prevent="addSlot">
        <div class="form-group">
          <label for="startTime">Bắt đầu:</label>
          <input
            type="datetime-local"
            id="startTime"
            v-model="newSlot.startTime"
            required
          />
        </div>
        <div class="form-group">
          <label for="endTime">Kết thúc:</label>
          <input
            type="datetime-local"
            id="endTime"
            v-model="newSlot.endTime"
            required
          />
        </div>
        <div class="form-group">
          <label for="teachAddress">Địa chỉ dạy:</label>
          <input type="text" id="teachAddress" v-model="newSlot.teachAddress" />
        </div>
        <div class="form-group">
          <label for="numberOfStudents">Số lượng học sinh hạn mức:</label>
          <input
            type="number"
            id="numberOfStudents"
            v-model="newSlot.numberOfStudents"
            required
          />
        </div>
        <div class="form-group">
          <label for="subjectId">Môn học:</label>
          <select id="subjectId" v-model="newSlot.subjectId" required>
            <option
              v-for="subject in subjects"
              :value="subject.id"
              :key="subject.id"
            >
              {{ subject.name }}
            </option>
          </select>
        </div>
        <div class="form-group">
          <label for="isOnline">Học online:</label>
          <input type="checkbox" id="isOnline" v-model="newSlot.isOnline" />
        </div>
        <button type="submit">Thêm slot</button>
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
        console.log(
          "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
        );
        const response = await axios.post(
          import.meta.env.VITE_API_URL + "/api/Slot",
          this.newSlot,
          {
            headers: {
              Authorization: "Bearer " + localStorage.token,
            },
          }
        );
        console.log(
          "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb"
        );
        this.$emit("add", response.data);
        console.log(
          "cccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc"
        );
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

<style scoped>
.modal-body {
  padding: 10px 20px;
}

.form-group {
  margin-bottom: 1rem;
}

.form-group label {
  display: block;
  font-weight: bold;
}

.form-group input,
.form-group select {
  width: 100%;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-sizing: border-box;
}

.form-group button {
  background-color: #5cb85c;
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.form-group button:hover {
  background-color: #4caf50;
}
</style>

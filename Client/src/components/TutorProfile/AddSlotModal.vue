<template>
  <div class="modal" v-if="showModal">
    <div class="modal-content">
      <!-- Modal header -->
      <div class="modal-header">
        <span class="modal-title">Thêm slot mới</span>
        <span class="close" @click="closeModal">&times;</span>
      </div>
      <!-- Modal body -->
      <div class="modal-body">
        <!-- Form for adding a new slot -->
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
            <input
              type="text"
              id="teachAddress"
              v-model="newSlot.teachAddress"
            />
          </div>
          <!-- Add other fields as needed -->
          <button type="submit">Thêm slot</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      newSlot: {
        startTime: "",
        endTime: "",
        teachAddress: "",
        // Add other fields here according to your slot model
      },
    };
  },
  props: ["showModal"],
  methods: {
    closeModal() {
      this.$emit("close");
    },
    addSlot() {
      // Emit an event to parent component to add the new slot
      this.$emit("add", this.newSlot);
      // Optionally, reset the form fields
      this.newSlot = {
        startTime: "",
        endTime: "",
        teachAddress: "",
        // Initialize other fields as needed
      };
      this.closeModal();
    },
  },
};
</script>

<style scoped>
/* Add your modal CSS styles here */
.modal {
  display: block;
  position: fixed;
  z-index: 999;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  overflow: auto;
  background-color: rgba(0, 0, 0, 0.4);
}

.modal-content {
  background-color: #fefefe;
  margin: 15% auto;
  padding: 20px;
  border: 1px solid #888;
  width: 50%;
}

.modal-header {
  padding: 2px 16px;
  background-color: #5cb85c;
  color: white;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.modal-title {
  font-size: 20px;
}

.close {
  cursor: pointer;
  font-size: 25px;
}

.close:hover,
.close:focus {
  color: #000;
  text-decoration: none;
  cursor: pointer;
}

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

.form-group input {
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

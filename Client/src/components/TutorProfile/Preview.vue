<template>
  <div>
    <span @click="handleClick" class="wrapper"> Edit <EditOutlined /> </span>
    <a-modal
      v-model:visible="showRespondModal"
      width="40vw"
      @cancel="handleCancel"
      footer="{null}"
    >
      <div class="edit-form">
        <h3>Edit Profile</h3>
        <a-form layout="vertical" @submit="handleSubmit">
          <a-form-item
            v-if="type === EditProfileEnum.NAME"
            label="Name"
            :rules="[nameRules]"
          >
            <a-input
              v-model="formInput.name"
              placeholder="Enter tutor's name"
            />
          </a-form-item>
          <a-form-item
            v-if="type === EditProfileEnum.SPECIALIZATIONS"
            label="Specializations"
          >
            <a-select
              mode="tags"
              v-model="formInput.specializations"
              placeholder="Enter tutor's specializations"
            />
          </a-form-item>
          <a-button type="primary" html-type="submit">Submit changes</a-button>
        </a-form>
      </div>
    </a-modal>
  </div>
</template>

<script>
import { defineComponent, ref } from "vue";
import { EditOutlined } from "@ant-design/icons-vue";
import {
  Modal as AModal,
  Form as AForm,
  Input as AInput,
  Select as ASelect,
  Button as AButton,
} from "ant-design-vue";
import { EditProfileEnum } from "../../enum/EditProfileEnum";
import { openNotification } from "../../utils/Alert";

export default defineComponent({
  name: "EditProfile",
  components: {
    EditOutlined,
    AModal,
    AForm,
    AInput,
    ASelect,
    AButton,
  },
  props: {
    type: {
      type: String,
      required: true,
    },
  },
  setup(props) {
    const showRespondModal = ref(false);
    const user = ref({
      id: 1,
      name: "John Doe",
      expertise: ["Math", "Science"],
    }); // Mock user data
    const formInput = ref({});

    const handleClick = () => {
      showRespondModal.value = true;
    };

    const handleCancel = () => {
      showRespondModal.value = false;
    };

    const handleSubmit = () => {
      if (!user.value) {
        return openNotification("Invalid tutor info", "", "warning");
      }

      const value = formInput.value[props.type.toLowerCase()];
      let request = { ...user.value };
      let successMsg = "";

      switch (props.type) {
        case EditProfileEnum.NAME:
          request.name = value;
          successMsg = "Tutor's name updated to " + value;
          break;
        case EditProfileEnum.SPECIALIZATIONS:
          request.expertise = value;
          successMsg = "Tutor's specializations updated to " + value;
          break;
        default:
          return openNotification("No edit input found", "", "warning");
      }

      user.value = request;
      openNotification("Profile update", successMsg, "success");
    };

    return {
      showRespondModal,
      user,
      formInput,
      handleClick,
      handleCancel,
      handleSubmit,
      EditProfileEnum,
      nameRules: [{ required: true, message: "Enter tutor's name!" }],
    };
  },
});
</script>

<style scoped>
.wrapper {
  color: #8a2be2;
  padding: 4px 8px;
  cursor: pointer;
}
.wrapper:hover {
  color: #696969;
  background: #87ceeb;
  border: 1px solid #d3d3d3;
  border-radius: 4px;
}
.edit-form {
  padding: 12px 20px;
}
</style>

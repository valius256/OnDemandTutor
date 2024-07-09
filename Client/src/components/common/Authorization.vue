<template>
  <div>
    <slot></slot>
    <div v-if="isOpenConfirmPopup" class="popup-overlay">
      <div class="sticky top-1/4 flex justify-center">
        <div class="relative">
          <button class="absolute right-0 mt-2 mr-2 w-8 h-8 bg-red-400 text-white rounded-full z-[50]"
            @click="toggleOpenOnfirmPopup">
            X
          </button>
          <ConfirmationForm :message="this.cfmMessage" :callback="this.cfmCallBack" :params="cfmParams"
            :method="cfmMethod" />
        </div>
      </div>
    </div>
    <div v-if="isOpenResultDialog" class="popup-overlay">
      <div class="sticky top-1/4 flex justify-center">
        <div class="relative">
          <button class="absolute right-0 mt-2 mr-2 w-8 h-8 bg-red-400 text-white rounded-full z-[50]"
            @click="closeResultDialog">
            X
          </button>
          <ResultDialog :message="this.rsMesage" :type="this.rsType" />
        </div>
      </div>
    </div>
    <div v-if="isOpenLoadingPopup" class="popup-overlay">
      <div class="sticky top-1/4 flex justify-center">
        <LoadingPopup :message="ldMessage" />
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import ConfirmationForm from "./ConfirmationForm.vue";
import ResultDialog from "./ResultDialog.vue";
import LoadingPopup from "./LoadingPopup.vue";

export default {
  name: "Authourization",
  inject: ["eventBus"],
  components: { ConfirmationForm, ResultDialog, LoadingPopup },
  data() {
    return {
      cfmMessage: "",
      cfmCallBack: "",
      cfmMethod: null,
      cfmParams: null,
      isOpenConfirmPopup: false,
      isOpenResultDialog: false,
      isOpenLoadingPopup: false,
      rsMesage: "",
      rsType: "",
      ldMessage: "",
      user: {
        name: "Hello World",
        avatar: "/src/assets/noavatar.jpg",
        role: "Student",
      },
    };
  },
  mounted() {
    this.eventBus.on("open-login-popup", () => {
      this.toggleLoginPopup();
    });
    this.eventBus.on("open-register-popup", () => {
      this.toggleRegsiterPopup();
    });
    this.eventBus.on("open-confirmation-popup", (request) => {
      this.toggleOpenOnfirmPopup(
        request.message,
        request.callback,
        request.params,
        request.method
      );
    });
    this.eventBus.on("open-result-dialog", (request) => {
      this.openResultDialog(request.message, request.type);
    });
    this.eventBus.on("close-result-dialog", () => {
      this.closeResultDialog();
    });
    this.eventBus.on("open-loading-popup", (request) => {
      this.openLoadingPopup(request.message);
    });
    this.eventBus.on("close-loading-popup", () => {
      this.closeLoadingPopup();
    });
    this.eventBus.on("update-everything", () => {
      this.updateApp()
    })
    this.eventBus.on("get-current-user", (callback) => {
      callback(this.user);
    });
    this.eventBus.on("login", (loginDto) => {
      this.login(loginDto);
    });
    this.eventBus.on("logout", () => {
      this.logout();
    });
    this.eventBus.on("register", (registerDto) => {
      this.register(registerDto);
    });
    this.updateApp();
  },
  methods: {
    updateApp() {
      this.eventBus.emit("update-header");
      this.eventBus.emit("update-navbar");
      this.eventBus.emit("update-app-user");
    },
    toggleLoginPopup() {
      this.isOpenLoginPopup = !this.isOpenLoginPopup;
    },
    toggleRegsiterPopup() {
      this.isOpenRegisterPopup = !this.isOpenRegisterPopup;
    },
    openLoadingPopup(message) {
      this.ldMessage = message;
      this.isOpenLoadingPopup = true;
    },
    closeLoadingPopup() {
      this.isOpenLoadingPopup = false;
    },
    toggleOpenOnfirmPopup(message, callback, params, method) {
      if (this.isOpenConfirmPopup) {
        this.isOpenConfirmPopup = false;
        this.cfmMessage = "";
        this.cfmParams = null;
        this.cfmMethod = null;
      } else {
        this.isOpenConfirmPopup = true;
        this.cfmMessage = message;
        this.cfmCallBack = callback;
        this.cfmParams = params;
        this.cfmMethod = method;
      }
    },
    openResultDialog(message, type) {
      this.rsMesage = message;
      (this.rsType = type), (this.isOpenResultDialog = true);
    },
    closeResultDialog() {
      this.isOpenResultDialog = false;
    },
    async login(loginDto) {
      try {
        this.eventBus.emit("open-loading-popup", {
          message: "Vui lòng chờ..."
        })
        const response = await axios.post(
          import.meta.env.VITE_API_URL + "/api/auth/login-firebase",
          {
            email: loginDto.emailOrPhone,
            password: loginDto.password,
          }
        );
        if (response.data) {
          if (response.data.data.code == "400") {
            this.eventBus.emit(
              "login-set-error",
              "Email hoặc mật khẩu không chính xác"
            );
          } else {
            localStorage.setItem("token", response.data.data.message);
            this.updateApp();
            this.$router.push("/")
          }
        }
      } catch (e) {
        this.eventBus.emit(
          "login-set-error",
          "Đã xảy ra lỗi. Vui lòng thử lại sau!"
        );
      }
      this.eventBus.emit("close-loading-popup")
    },
    async register(registerDto) {
      try {
        const response = await axios.post(
          import.meta.env.VITE_API_URL + "/api/auth/register",
          {
            name: registerDto.name,
            password: registerDto.password,
            phone: registerDto.phone,
            email: registerDto.email,
            dob: registerDto.dob,
            address: registerDto.address,
            gender: registerDto.gender,
            bankAccount: registerDto.bankAccount,
          }
        );
        if (response.data) {
          await this.login({
            emailOrPhone: registerDto.phone,
            password: registerDto.password,
          });
          this.toggleRegsiterPopup();
          this.toggleLoginPopup();
        }
      } catch (e) {
        if (e.response.data) {
          this.eventBus.emit(
            "register-set-error",
            e.response.data.ErrorMessage
          );
        } else {
          this.eventBus.emit(
            "register-set-error",
            "Something went wrong. Please try again later!"
          );
        }
      }
    },
    async getUser(token) {
      // this.eventBus.emit("open-loading-popup", {
      //   message: "Loading......"
      // })
      const response = await axios.get(
        import.meta.env.VITE_API_URL + "/api/auth/who-am-i",
        {
          headers: {
            Authorization: "Bearer " + token,
          },
        }
      );
      //this.eventBus.emit("close-loading-popup")
      if (response.data) {
        return response.data.data;
      }
      return null;
    },
    async getOperator() {
      if (!localStorage.token) {
        this.$router.push("/");
      } else {
        const user = await this.getUser(localStorage.token);
        if (user && user.role == "Operator") {
          return user;
        } else {
          this.$router.push("/");
        }
      }
    },
    logout() {
      localStorage.removeItem("token");
      window.location.reload()
      //this.updateApp();
    },
  },
};
</script>
<style></style>

<template>
  <div class="min-h-screen bg-blue-50 overflow-x-hidden">
    <main>
      <section class="bg-blue-200 text-white py-32 relative flex flex-col lg:flex-row gap-4">
        <div class="absolute inset-0 flex ">
          <div class="relative">
            <div class="bg-[rgb(116,168,218)] w-[400px] h-full transform skew-x-12 -ml-24">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>
          <div class="relative">
            <div class="bg-[#a3cdf5] w-[200px] h-full transform skew-x-12 -ml-24">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>
          <div class="relative">
            <div class="bg-[#c4e2fd] w-[100px] h-full transform skew-x-12">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>
        </div>
        <div class="container mx-auto text-center relative z-10  animate-fade-down">
          <h2 class="text-4xl font-bold mb-8">
            Tìm Gia Sư Hoàn Hảo Cho Bạn Ngay Hôm Nay
          </h2>
          <p class="text-xl mb-12">
            Các gia sư chuyên môn ở nhiều lĩnh vực khác nhau sẵn sàng giúp bạn
            đạt được mục tiêu học tập của mình.
          </p>
          <button class="bg-white text-blue-600 py-3 px-6 rounded-full text-lg font-bold">
            Bắt Đầu Ngay
          </button>
        </div>
        <img class="mr-4 relative" src="https://www.greatschools.org/gk/wp-content/uploads/2010/01/Looking-for-a-tutor.jpg">
        
      </section>
      <section id="popular-tutors" class="relative">
        <div class="absolute inset-0 flex justify-end">
          <div class="relative">
            <div class="bg-[#deefff] w-[100px] h-full transform -skew-x-12">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>
          <div class="relative">
            <div class="bg-[#d6ebff] w-[300px] h-full transform -skew-x-12 -mr-48">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>
          <div class="relative">
            <div class="bg-[#c5e3ff] w-[400px] h-full transform -skew-x-12 -mr-48">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>   
        </div>

        <div class="relative mx-auto text-center py-20 w-10/12">
          <h2 class="text-3xl font-bold mb-10">Các Gia Sư Tiêu Biểu</h2>
          <div class="flex flex-wrap -mx-4">
            <div v-for="tutor in tutors" :key="tutor.tutor.id" class="w-full lg:w-1/4 md:w-1/2 px-4 mb-8 transition duration-200 hover:scale-110">
              <div class="bg-white rounded-lg shadow p-6">
                <router-link to="/tutor">
                  <img :src="tutor.tutor.avatarImageUrl ?? '/src/assets/noavatar.jpg'" />
                </router-link>
                <p class="mt-4 font-bold">{{ tutor.tutor.fullName }}</p>
                <p class="flex justify-center my-2">
                  <star-rating :star-size="20" :rating="tutor.tutor.rating" :round-start-rating="false"
                    :read-only="true" />
                </p>
                <p class="mt-2 italic">
                  {{ tutor.numberOfStudentClass + tutor.numberOfBooker }} lượt đặt dạy
                </p>
                <button class="mt-3 bg-slate-50 text-blue-600 py-3 px-6 rounded-full text-lg font-bold">
                  Đặt ngay
                </button>
              </div>
            </div>
          </div>
          <button class="mt-3 bg-white hover:bg-slate-200 text-blue-600 py-3 px-12 rounded-full text-lg font-bold" @click="$router.push('/tutors')">
            Xem thêm gia sư
          </button>
        </div>
      </section>

      <section id="features" class="bg-blue-100 py-20 relative">
        <div class="absolute inset-0 flex ">
          <div class="relative">
            <div class="bg-[#b5dbff] w-[400px] h-full transform skew-x-12 -ml-24">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>
          <div class="relative">
            <div class="bg-[#c5e3ff] w-[200px] h-full transform skew-x-12 -ml-24">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>
          <div class="relative">
            <div class="bg-[#d6ebff] w-[100px] h-full transform skew-x-12">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>
        </div>
        <div class="relative container mx-auto text-center">
          <h2 class="text-3xl font-bold mb-10">Tính Năng Của Chúng Tôi</h2>
          <div class="flex flex-wrap -mx-4 ">
            <div class="w-full md:w-1/3 px-4 mb-8 flex transition duration-200 hover:scale-110">
              <div class="bg-white rounded-lg shadow rounded-t-lg ">
                <h3 class="text-2xl font-bold mb-4 py-3 px-6 bg-blue-300 rounded-t-lg ">Gia sư chuyên môn cao</h3>
                <p class="px-6 pb-6">
                  Chúng tôi chỉ thuê những gia sư có kinh nghiệm và là chuyên
                  gia trong lĩnh vực của họ.
                </p>
              </div>
            </div>
            <div class="w-full md:w-1/3 px-4 mb-8 flex transition duration-200 hover:scale-110">
              <div class="bg-white rounded-lg shadow">
                <h3 class="text-2xl font-bold mb-4 py-3 px-6 bg-blue-300 rounded-t-lg ">
                  Sắp xếp lịch học linh hoạt
                </h3>
                <p class="px-6 pb-6">
                  Trang web chúng tôi cho phép các bạn đặt lịch học với gia sư
                  một cách linh hoạt và thuận tiện nhất cho cả hai
                </p>
              </div>
            </div>
            <div class="w-full md:w-1/3 px-4 mb-8 flex transition duration-200 hover:scale-110">
              <div v class="bg-white rounded-lg shadow">
                <h3 class="text-2xl font-bold mb-4 py-3 px-6 bg-blue-300 rounded-t-lg ">Giá cả phải chăng</h3>
                <p class="px-6 pb-6">
                  Nhận những dịch vụ gia sư theo yêu cầu tốt nhất với giá cả hợp
                  lý.
                </p>
              </div>
            </div>
          </div>
        </div>
      </section>

      <section id="how-it-works" class=" py-20 relative">
        <div class="absolute inset-0 flex justify-end">
          <div class="relative">
            <div class="bg-[#deefff] w-[100px] h-full transform -skew-x-12">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>
          <div class="relative">
            <div class="bg-[#d6ebff] w-[300px] h-full transform -skew-x-12 -mr-48">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>
          <div class="relative">
            <div class="bg-[#c5e3ff] w-[400px] h-full transform -skew-x-12 -mr-48">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>   
        </div>
        <div class="container mx-auto text-center relative">
          <h2 class="text-3xl font-bold mb-10">Cách Thức Hoạt Động</h2>
          <div class="flex flex-wrap -mx-4">
            <div class="w-full md:w-1/3 px-4 mb-8 flex transition duration-200 hover:scale-110">
              <div class="bg-white rounded-lg shadow">
                <h3 class="text-2xl font-bold mb-4 py-3 px-6 bg-blue-300 rounded-t-lg">1. Đăng ký</h3>
                <p class="px-6 pb-6">
                  Tạo một tài khoản và nói cho chúng tôi biết về nhu cầu dạy kèm
                  của bạn.
                </p>
              </div>
            </div>
            <div class="w-full md:w-1/3 px-4 mb-8 flex transition duration-200 hover:scale-110">
              <div class="bg-white rounded-lg shadow">
                <h3 class="text-2xl font-bold mb-4 py-3 px-6 bg-blue-300 rounded-t-lg">2. Chọn Gia Sư</h3>
                <p class="px-6 pb-6">
                  Duyệt qua danh sách các gia sư có trình độ của chúng tôi và
                  chọn một gia sư phù hợp nhất với nhu cầu của bạn.
                </p>
              </div>
            </div>
            <div class="w-full md:w-1/3 px-4 mb-8 flex transition duration-200 hover:scale-110">
              <div class="bg-white rounded-lg shadow">
                <h3 class="text-2xl font-bold mb-4 py-3 px-6 bg-blue-300 rounded-t-lg">3. Bắt Đầu Học</h3>
                <p class="px-6 pb-6">
                  Lên lịch buổi học đầu tiên và bắt đầu quá trình chinh phục mục
                  tiêu học tập của bạn.
                </p>
              </div>
            </div>
          </div>
        </div>
      </section>



      <section id="how-to-tutor" class="bg-blue-100 py-20 relative">
        <div class="absolute inset-0 flex ">
          <div class="relative">
            <div class="bg-[#b5dbff] w-[400px] h-full transform skew-x-12 -ml-24">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>
          <div class="relative">
            <div class="bg-[#c5e3ff] w-[200px] h-full transform skew-x-12 -ml-24">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>
          <div class="relative">
            <div class="bg-[#d6ebff] w-[100px] h-full transform skew-x-12">
              <!-- This div creates the trapezoid shape -->
            </div>
          </div>
        </div>
        <div class="container mx-auto text-center relative">
          <h2 class="text-3xl font-bold mb-10">Bạn Muốn Trở Thành Gia Sư?</h2>
          <div class="flex flex-wrap -mx-4">
            <div class="w-full md:w-1/3 px-4 mb-8 flex transition duration-200 hover:scale-110">
              <div class="bg-white rounded-lg shadow">
                <h3 class="text-2xl font-bold mb-4 py-3 px-6 bg-blue-300 rounded-t-lg ">1. Tạo tài khoản</h3>
                <p class="px-6 pb-6">
                  Tạo một tài khoản gia sư và cung cấp các loại bằng cấp của
                  bạn.
                </p>
              </div>
            </div>
            <div class="w-full md:w-1/3 px-4 mb-8 flex transition duration-200 hover:scale-110">
              <div class="bg-white rounded-lg shadow">
                <h3 class="text-2xl font-bold mb-4 py-3 px-6 bg-blue-300 rounded-t-lg ">2. Cập nhật hồ sơ</h3>
                <p class="px-6 pb-6">
                  Quảng bá chuyên môn và dịch vụ học tập của bạn bằng các video
                  học tập để thu hút học viên. Họ sẽ tìm đến bạn và đăng ký học
                </p>
              </div>
            </div>
            <div class="w-full md:w-1/3 px-4 mb-8 flex transition duration-200 hover:scale-110">
              <div class="bg-white rounded-lg shadow">
                <h3 class="text-2xl font-bold mb-4 py-3 px-6 bg-blue-300 rounded-t-lg ">3. Bắt Đầu Dạy</h3>
                <p class="px-6 pb-6">Bắt đầu làm việc dựa trên lịch dạy của bạn</p>
              </div>
            </div>
          </div>
        </div>
      </section>
      <section id="contact" class="bg-blue-500 text-white py-20 relative">
        <div class="absolute inset-0 z-0 bg-contact-banner bg-cover bg-no-repeat opacity-50"></div>
        <div class="container mx-auto text-center relative">
          <h2 class="text-3xl font-bold mb-10">Liên Hệ Với Chúng Tôi</h2>
          <p class="mb-10">
            Bạn có bất cứ thắc mắc nào không? Hãy để lại thông tin liên lạc.
            Chúng tôi sẽ liên hệ với bạn sớm nhất có thể để tư vấn nhé!
          </p>
          <div class="w-1/2 mx-auto p-10 flex flex-col gap-4 rounded-xl bg-slate-300 text-black">
            <input type="text" placeholder="Tên của bạn" class="bg-white py-3 px-4 rounded-full mb-4 md:mb-0"
              v-model="consultationName" />
            <input type="email" placeholder="Email của bạn" class="bg-white py-3 px-4 rounded-full mb-4 md:mb-0"
              v-model="consultationEmail" />
            <input type="tel" placeholder="Số điện thoại" class="bg-white py-3 px-4 rounded-full mb-4 md:mb-0"
              v-model="consultationPhone" />
            <textarea placeholder="Nội dung tin nhắn" class="bg-white py-3 px-4 rounded-lg h-32 mb-6"
              v-model="consultationMessage"></textarea>
          </div>
          <button type="submit" @click="sendConsultationRequest(true)"
            class="font-bold mt-4 bg-white text-blue-600 py-3 px-6 rounded-full text-lg hover:bg-slate-200">
            Gửi tin nhắn
          </button>
        </div>
      </section>
    </main>
  </div>
</template>

<script>
import axios from "axios";
import Footer from "../../components/common/Footer.vue";
import StarRating from 'vue-star-rating'

export default {
  name: "HomePage",
  inject: ['eventBus'],
  components: { Footer, StarRating },
  data() {
    return {
      consultationName: "",
      consultationEmail: "",
      consultationPhone: "",
      consultationMessage: "",
      tutors: [

      ],
    };
  },
  methods: {
    async sendConsultationRequest(confirmation) {
      if (confirmation) {
        this.eventBus.emit("open-confirmation-popup", {
          message: "Bạn có chắc chắn muốn gửi yêu cầu tư vấn?",
          method: this.sendConsultationRequest,
          params: false
        })
      } else {
        const request = {
          name: this.consultationName,
          email: this.consultationEmail,
          phone: this.consultationPhone,
          consultationContent: this.consultationMessage
        }
        this.eventBus.emit("open-loading-popup", {
          message: "Vui lòng chờ..."
        })
        try {
          await axios.post(import.meta.env.VITE_API_URL + '/api/ConsultationControllers/register', request)
          this.consultationName = "",
            this.consultationEmail = ""
          this.consultationPhone = ""
          this.consultationMessage = ""
          this.eventBus.emit("open-result-dialog", {
            message: "Gửi yêu cầu thành công",
            type: "Success"
          })
        } catch (e) {
          console.log(e)
          this.eventBus.emit("open-result-dialog", {
            message: "Có vấn đề xảy ra khi gửi yêu cầu",
            type: "Error"
          })
        }
        this.eventBus.emit("close-loading-popup")
      }
    },
    async fetchOutstandingTutors() {
      let query = {
        Page: 1,
        Limit: 4,
      }
      //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
      const response = await axios.get(import.meta.env.VITE_API_URL + '/api/User/outstanding-tutors?' +
        this.jsonToQueryString(query))
      if (response.data) {
        this.tutors = response.data.data.items
      }
    },
  },
  mounted() {
    this.fetchOutstandingTutors()
  }
};
</script>

<style scoped>
.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
}
</style>

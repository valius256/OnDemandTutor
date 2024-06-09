<template>
  <div class="min-h-screen bg-blue-50">
    <main>
      <section class="bg-blue-500 text-white py-20 relative">
        <div class="absolute inset-0 z-0 bg-home-banner bg-cover bg-center bg-no-repeat opacity-80"></div>
        <div class="container mx-auto text-center relative z-10">
          <h2 class="text-4xl font-bold mb-8">Tìm Gia Sư Hoàn Hảo Cho Bạn Ngay Hôm Nay</h2>
          <p class="text-xl mb-12">Các gia sư chuyên môn ở nhiều lĩnh vực khác nhau sẵn sàng giúp bạn đạt được mục tiêu
            học tập của mình.</p>
          <button class="bg-white text-blue-600 py-3 px-6 rounded-full text-lg font-bold">Bắt Đầu Ngay</button>
        </div>
      </section>
      <section id="features" class="py-20">
        <div class="container mx-auto text-center">
          <h2 class="text-3xl font-bold mb-10">Tính Năng Của Chúng Tôi</h2>
          <div class="flex flex-wrap -mx-4">
            <div class="w-full md:w-1/3 px-4 mb-8">
              <div class="bg-white rounded-lg shadow p-6">
                <h3 class="text-2xl font-bold mb-4">Gia sư chuyên môn cao</h3>
                <p>Chúng tôi chỉ thuê những gia sư có kinh nghiệm và là chuyên gia trong lĩnh vực của họ.</p>
              </div>
            </div>
            <div class="w-full md:w-1/3 px-4 mb-8">
              <div class="bg-white rounded-lg shadow p-6">
                <h3 class="text-2xl font-bold mb-4">Sắp xếp lịch học linh hoạt</h3>
                <p>Trang web chúng tôi cho phép các bạn đặt lịch học với gia sư một cách linh hoạt và thuận tiện nhất
                  cho cả hai</p>
              </div>
            </div>
            <div class="w-full md:w-1/3 px-4 mb-8">
              <div class="bg-white rounded-lg shadow p-6">
                <h3 class="text-2xl font-bold mb-4">Giá cả phải chăng</h3>
                <p>Nhận những dịch vụ gia sư theo yêu cầu tốt nhất với giá cả hợp lý.</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      <section id="how-it-works" class="bg-blue-100 py-20">
        <div class="container mx-auto text-center">
          <h2 class="text-3xl font-bold mb-10">Cách Thức Hoạt Động</h2>
          <div class="flex flex-wrap -mx-4">
            <div class="w-full md:w-1/3 px-4 mb-8">
              <div class="bg-white rounded-lg shadow p-6">
                <h3 class="text-2xl font-bold mb-4">1. Đăng ký</h3>
                <p>Tạo một tài khoản và nói cho chúng tôi biết về nhu cầu dạy kèm của bạn.</p>
              </div>
            </div>
            <div class="w-full md:w-1/3 px-4 mb-8">
              <div class="bg-white rounded-lg shadow p-6">
                <h3 class="text-2xl font-bold mb-4">2. Chọn Gia Sư</h3>
                <p>Duyệt qua danh sách các gia sư có trình độ của chúng tôi và chọn một gia sư phù hợp nhất với nhu cầu
                  của bạn.</p>
              </div>
            </div>
            <div class="w-full md:w-1/3 px-4 mb-8">
              <div class="bg-white rounded-lg shadow p-6">
                <h3 class="text-2xl font-bold mb-4">3. Bắt Đầu Học</h3>
                <p>Lên lịch buổi học đầu tiên và bắt đầu quá trình chinh phục mục tiêu học tập của bạn.</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      <section id="popular-tutors" class="py-20">
        <div class="container mx-auto text-center">
          <h2 class="text-3xl font-bold mb-10">Các Gia Sư Tiêu Biểu</h2>
          <div class="flex flex-wrap -mx-4">
            <div v-for="tutor in tutors" :key="tutor.id" class="w-full lg:w-1/4 md:w-1/2 px-4 mb-8">
              <div class="bg-white rounded-lg shadow p-6">
                <img :src="tutor.avatar" />
                <p class="mt-4 font-bold">{{ tutor.name }}</p>
                <p class="mt-2 italic">{{ tutor.bookerNumber }} người đã đặt dạy</p>
                <button class="mt-3 bg-slate-50 text-blue-600 py-3 px-6 rounded-full text-lg font-bold">Đặt
                  ngay</button>
              </div>
            </div>

          </div>
        </div>
      </section>

      <section id="how-to-tutor" class="bg-blue-100 py-20">
        <div class="container mx-auto text-center">
          <h2 class="text-3xl font-bold mb-10">Bạn Muốn Trở Thành Gia Sư?</h2>
          <div class="flex flex-wrap -mx-4">
            <div class="w-full md:w-1/3 px-4 mb-8">
              <div class="bg-white rounded-lg shadow p-6">
                <h3 class="text-2xl font-bold mb-4">1. Tạo tài khoản</h3>
                <p>Tạo một tài khoản gia sư và cung cấp các loại bằng cấp của bạn.</p>
              </div>
            </div>
            <div class="w-full md:w-1/3 px-4 mb-8">
              <div class="bg-white rounded-lg shadow p-6">
                <h3 class="text-2xl font-bold mb-4">2. Cập nhật hồ sơ</h3>
                <p>Quảng bá chuyên môn và dịch vụ học tập của bạn bằng các video học tập để thu hút học viên. Họ sẽ tìm
                  đến bạn và đăng ký học</p>
              </div>
            </div>
            <div class="w-full md:w-1/3 px-4 mb-8">
              <div class="bg-white rounded-lg shadow p-6">
                <h3 class="text-2xl font-bold mb-4">3. Bắt Đầu Dạy</h3>
                <p>Bắt đầu làm việc dựa trên lịch dạy của bạn</p>
              </div>
            </div>
          </div>
        </div>
      </section>
      <section id="contact" class="bg-blue-500 text-white py-20 relative">
        <div class="absolute inset-0 z-0 bg-contact-banner bg-cover bg-no-repeat opacity-50"></div>
        <div class="container mx-auto text-center relative">
          <h2 class="text-3xl font-bold mb-10">Liên Hệ Với Chúng Tôi</h2>
          <p class="mb-10">Bạn có bất cứ thắc mắc nào không? Hãy để lại thông tin liên lạc. Chúng tôi sẽ liên hệ với bạn
            sớm nhất có thể để tư vấn nhé!</p>
          <div class="w-1/2 mx-auto p-10 flex flex-col gap-4 rounded-xl bg-slate-300 text-black">
            <input type="text" placeholder="Tên của bạn" class="bg-white py-3 px-4 rounded-full mb-4 md:mb-0">
            <input type="email" placeholder="Email của bạn" class="bg-white py-3 px-4 rounded-full mb-4 md:mb-0">
            <input type="tel" placeholder="Số điện thoại" class="bg-white py-3 px-4 rounded-full mb-4 md:mb-0">
            <textarea placeholder="Nội dung tin nhắn" class="bg-white py-3 px-4 rounded-lg h-32 mb-6"></textarea>
          </div>
          <button type="submit"
            class="font-bold mt-4 bg-white text-blue-600 py-3 px-6 rounded-full text-lg hover:bg-slate-200">Gửi tin
            nhắn</button>
        </div>
      </section>
    </main>

  </div>
</template>

<script>
import Footer from '../../components/common/Footer.vue';

export default {
  name: 'HomePage',
  components: { Footer },
  data() {
    return {
      tutors: [
        {
          id: 1,
          name: "Nguyen Van A",
          avatar: '/src/assets/noavatar.jpg',
          bookerNumber: 10
        },
        {
          id: 2,
          name: "Nguyen Van A",
          avatar: '/src/assets/noavatar.jpg',
          bookerNumber: 10
        },
        {
          id: 3,
          name: "Nguyen Van A",
          avatar: '/src/assets/noavatar.jpg',
          bookerNumber: 10
        },
        {
          id: 4,
          name: "Nguyen Van A",
          avatar: '/src/assets/noavatar.jpg',
          bookerNumber: 10
        },
      ]
    }
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
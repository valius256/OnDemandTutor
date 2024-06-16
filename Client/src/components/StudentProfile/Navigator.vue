<template>
    <div class="w-64 bg-blue-50">
        <div>
            <div class="flex flex-col font-bold">

                <router-link to="/student/profile" class="px-2 py-4 hover:bg-blue-800">
                    <i class="fa fa-bookmark text-2xl mr-2"></i>Thông tin cá nhân
                </router-link>
                <router-link to="/student/schedule" class="px-2 py-4 hover:bg-blue-800">
                    <i class="fa fa-calendar text-2xl mr-2"></i>Thời khóa biểu
                </router-link>
                <router-link to="/student/payment" class="px-2 py-4 hover:bg-blue-800">
                    <i class="fa fa-pencil-square-o text-2xl mr-2"></i>Thanh toán
                </router-link>

            </div>
        </div>
        <div class="flex flex-col ">
            <button class="px-2 py-4 font-bold  text-left hover:bg-blue-800">
                <i class="fa fa-sign-out text-2xl mr-2"></i>Đăng xuất
            </button>
        </div>
    </div>      
</template>

<script>

export default {
    name: "StudentProfileNavigator"
}
</script>

<style scoped>
.router-link-active {
    background: #0a0030;
    color: white;
}

.sub-nav .router-link-active {
    background: #0c0038;
}
</style>
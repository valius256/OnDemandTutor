<template>
    <div class="p-4 bg-white rounded-b-lg w-full flex flex-col">
        <div class="flex flex-col">
            <div class="flex ">
                <span class="w-48 p-1 font-bold">Nhập mật khẩu cũ</span>
                <input v-model="oldPass" class="p-1 border rounded-lg" placeholder="Nhập mật khẩu cũ" type="password" />
            </div>
            <div class="flex mt-4">
                <span class="w-48 p-1 font-bold">Nhập mật khẩu mới</span>
                <input v-model="newPass" class="p-1 border rounded-lg" placeholder="Nhập mật khẩu mới"
                    type="password" />
            </div>
            <div class="flex mt-4">
                <span class="w-48 p-1 font-bold">Xác nhận mật khẩu</span>
                <input v-model="cfmPass" class="p-1 border rounded-lg" placeholder="Xác nhận mật khẩu"
                    type="password" />
            </div>
            <button class="mt-4  hover:bg-blue-200 rounded-lg py-2 bg-blue-400 font-bold text-white">Xác nhận</button>
        </div>

    </div>
</template>

<script>
export default {
    name: "ChangePasswordPopup",
    props: ['userId'],
    data() {
        return {
            oldPass: "",
            newPass: "",
            cfmPass: ""
        }
    }
}
</script>

<style></style>
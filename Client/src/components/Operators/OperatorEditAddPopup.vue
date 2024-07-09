<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-x-auto">
        <div class="flex gap-3">
            <div>
                <div class="flex ">
                    <span class="w-16 p-1 font-bold">Tên</span>
                    <input v-model="name" class="p-1 border rounded-lg" placeholder="Nhập tên" />
                </div>
                <div class="flex mt-4">
                    <span class="w-16 p-1 font-bold">Email</span>
                    <input v-model="email" class="p-1 border rounded-lg" placeholder="Nhập email" />
                </div>
            </div>
            <div>
                <div class="flex ">
                    <span class="w-24 p-1 font-bold">SDT</span>
                    <input v-model="phone" class="p-1 border rounded-lg" placeholder="Nhập SDT" />
                </div>
                <div class="flex mt-4" v-if="!editDto">
                    <span class="w-24 p-1 font-bold">Mật khẩu</span>
                    <input type="password" v-model="password" class="p-1 border rounded-lg"
                        placeholder="(Không bắt buộc)" />
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Quyền</span>
                    <select v-model="role" class="p-1 border rounded-lg" placeholder="Nhập email">
                        <option :value="3">
                            Admin
                        </option>
                        <option :value="2">
                            Operator
                        </option>
                    </select>
                </div>
            </div>
        </div>
        <div class="flex justify-center mt-4 gap-3">
            <button class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác nhận</button>
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>
    </div>
</template>

<script>
export default {
    name: "OperatorEditAddPopup",
    props: ['title', 'close', 'editDto'],
    data() {
        return {
            error: "",
            name: "",
            email: "",
            password: "",
            cfm_password: "",
            phone: "",
            role: "Operator",
        }
    },
    methods: {
        presetEdit() {
            if (this.editDto != null) {
                this.name = this.editDto.name
                this.email = this.editDto.email
                this.role = this.editDto.role
                this.phone = this.editDto.phone
            }
        },
        refresh() {
            if (this.editDto != null) {
                this.presetEdit();
            }
        }
    },
    mounted() {
        this.refresh()
    }
}
</script>

<style></style>
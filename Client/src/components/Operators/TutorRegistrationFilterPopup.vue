<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-y-auto max-h-[30rem] overflow-x-auto">
        <div class="flex gap-3">
            <div>
                <div class="flex ">
                    <span class="w-24 p-1 font-bold">Tên</span>
                    <input v-model="formFilterDto.name" class="p-1 border rounded-lg" placeholder="Nhập tên" />
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Email</span>
                    <input v-model="formFilterDto.email" class="p-1 border rounded-lg" placeholder="Nhập email" />
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">SDT</span>
                    <input v-model="formFilterDto.phone" class="p-1 border rounded-lg" placeholder="Nhập SDT" />
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Địa chỉ</span>
                    <input v-model="formFilterDto.address" class="p-1 border rounded-lg" placeholder="Nhập địa chỉ" />
                </div>
            </div>
            <div>                
                <div class="flex">
                    <span class="w-24 p-1 font-bold">Giới tính</span>
                    <select v-model="formFilterDto.gender" class="p-1 border rounded-lg">
                        <option value="All">Tất cả</option>
                        <option value="Male">Nam</option>
                        <option value="Female">Nữ</option>
                        <option value="Other">Khác</option>
                    </select>
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Ngày sinh</span>
                    <div>
                        <div class="flex">
                            <span class="w-10 p-1">Từ</span>
                            <input type="date" v-model="formFilterDto.fromDob" class="p-1 border rounded-lg"
                                placeholder="Nhập địa chỉ" />
                        </div>
                        <div class="flex mt-2">
                            <span class="w-10 p-1">Đến</span>
                            <input type="date" v-model="formFilterDto.toDob" class="p-1 border rounded-lg"
                                placeholder="Nhập địa chỉ" />
                        </div>
                    </div>
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Tạo ngày</span>
                    <div>
                        <div class="flex">
                            <span class="w-10 p-1">Từ</span>
                            <input type="date" v-model="formFilterDto.fromJoinDate" class="p-1 border rounded-lg"
                                placeholder="Nhập địa chỉ" />
                        </div>
                        <div class="flex mt-2">
                            <span class="w-10 p-1">Đến</span>
                            <input type="date" v-model="formFilterDto.toJoinDate" class="p-1 border rounded-lg"
                                placeholder="Nhập địa chỉ" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="flex justify-center mt-4 gap-3">
            <button @click="handleFilter" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác
                nhận</button>
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>

    </div>
</template>

<script>
export default {
    name: "TutorRegistrationFilterer",
    props: ['close', 'filterDto', 'action'],
    data() {
        return {
            formFilterDto: {
                gender: "All",
                name: "",
                email: "",
                phone: "",
                address: "",
                fromDob: null,
                toDob: null,
                fromJoinDate: null,
                toJoinDate: null,
                isChanged : false
            },

        }
    },
    methods: {
        preset() {
            if (this.filterDto != null) {
                this.formFilterDto = JSON.parse(JSON.stringify(this.filterDto));
            }
        },
        handleFilter() {
            this.formFilterDto.isChanged = true;
            this.action(this.formFilterDto)
            this.close()
        },
        toggleShowSubjectList() {
            this.isShowSubjectList = !this.isShowSubjectList
        }
    },
    mounted() {
        this.preset()
    }
}
</script>

<style></style>
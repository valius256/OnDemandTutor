<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-y-auto max-h-[30rem] overflow-x-auto">
        <div class="flex gap-3">
            <div>
                <div class="flex ">
                    <span class="w-24 p-1 font-bold">Tên</span>
                    <input v-model="formFilterDto.name" class="p-1 border rounded-lg" placeholder="Nhập tên" />
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Loại</span>
                    <input v-model="formFilterDto.subjectType" class="p-1 border rounded-lg" placeholder="Nhập loại môn học" />
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Mô tả</span>
                    <input v-model="formFilterDto.description" class="p-1 border rounded-lg" placeholder="Nhập mô tả" />
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Trạng thái</span>
                    <select v-model="formFilterDto.status" class="p-1 border rounded-lg">
                        <option value="All">Tất cả</option>
                        <option value="Active">Active</option>
                        <option value="Inactive">Inactive</option>
                    </select>
                </div>
            </div>
            <div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Ngày tạo</span>
                    <div>
                        <div class="flex">
                            <span class="w-10 p-1">Từ</span>
                            <input type="date" v-model="formFilterDto.fromCreateAt" class="p-1 border rounded-lg"
                                placeholder="Nhập địa chỉ" />
                        </div>
                        <div class="flex mt-2">
                            <span class="w-10 p-1">Đến</span>
                            <input type="date" v-model="formFilterDto.toCreateAt" class="p-1 border rounded-lg"
                                placeholder="Nhập địa chỉ" />
                        </div>
                    </div>
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Ngày chỉnh sửa</span>
                    <div>
                        <div class="flex">
                            <span class="w-10 p-1">Từ</span>
                            <input type="date" v-model="formFilterDto.fromUpdateAt" class="p-1 border rounded-lg"
                                placeholder="Nhập địa chỉ" />
                        </div>
                        <div class="flex mt-2">
                            <span class="w-10 p-1">Đến</span>
                            <input type="date" v-model="formFilterDto.toUpdateAt" class="p-1 border rounded-lg"
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
    name: "StudentFilterer",
    props: ['close', 'filterDto', 'action'],
    data() {
        return {
            formFilterDto: {
                name : "",
                subjectType : "",
                description : "",
                fromCreateAt : "",
                toCreateAt : "",
                fromUpdateAt : "",
                toUpdateAt : "",
                status : "All",
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
            this.formFilterDto.isChanged = true
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
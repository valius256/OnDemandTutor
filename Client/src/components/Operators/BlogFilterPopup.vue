<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-y-auto max-h-[30rem] overflow-x-auto">
        <div class="flex gap-3">
            <div>
                <div class="flex ">
                    <span class="w-24 p-1 font-bold">Từ khóa</span>
                    <input v-model="formFilterDto.keyword" class="p-1 border rounded-lg" placeholder="Nhập từ khóa tìm kiếm" />
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Tạo bởi</span>
                    <select v-model="formFilterDto.createdBy" class="p-1 border rounded-lg" placeholder="Nhập mô tả">
                        <option value="All">Tất cả</option>
                        <option :value="operator.id" v-for="operator in operators" :key="operator.id">
                            {{ operator.firstName + " " + (operator.lastName ?? " ") }}
                        </option>
                    </select>
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Trạng thái</span>
                    <select v-model="formFilterDto.status" class="p-1 border rounded-lg">
                        <option value="All">Tất cả</option>
                        <option value="Active">Công Khai</option>
                        <option value="Inactive">Đã Ẩn</option>
                    </select>
                </div>
            </div>
            <div>
                <div class="flex">
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
            <button @click="handleFilter" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác nhận</button>
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>
    </div>
</template>

<script>
export default {
    name: "BlogFilterer",
    props: ['close', 'filterDto', 'action','operators'],
    data() {
        return {
            formFilterDto: {
                keyword : "",
                fromCreateAt : "",
                toCreateAt : "",
                fromUpdateAt : "",
                toUpdateAt : "",
                createdBy : "All",
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
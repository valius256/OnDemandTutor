<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-y-auto max-h-[30rem] overflow-x-auto">
        <div class="flex gap-3">
            <div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Câu hỏi</span>
                    <input v-model="formFilterDto.question" class="p-1 border rounded-lg" placeholder="Nhập câu hỏi" />
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Trả lời</span>
                    <input v-model="formFilterDto.answer" class="p-1 border rounded-lg" placeholder="Nhập câu trả lời" />
                </div>
                <div class="flex mt-4">
                    <span class="w-24 p-1 font-bold">Thêm bởi</span>
                    <select v-model="formFilterDto.createdBy" class="p-1 border rounded-lg" placeholder="Nhập mô tả">
                        <option value="All">Tất cả</option>
                        <option :value="operator.id" v-for="operator in operators" :key="operator.id">
                            {{ operator.name }}
                        </option>
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
    name: "FAQFilterer",
    props: ['close', 'filterDto', 'action','operators'],
    data() {
        return {
            formFilterDto: {
                question : "",
                answer : "",
                createdBy : "All",
                fromCreateAt : "",
                toCreateAt : "",
                fromUpdateAt : "",
                toUpdateAt : "",
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
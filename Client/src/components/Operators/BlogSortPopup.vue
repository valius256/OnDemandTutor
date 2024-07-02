<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-y-auto max-h-[30rem] overflow-x-auto">
        <div class="flex flex-col gap-3 w-96">
            <div class="flex mt-4">
                <span class="w-48 p-1 font-bold">Sắp xếp theo</span>
                <select v-model="formSortDto.sortProp" class="p-1 border rounded-lg w-full">
                    <option value="Id">Id</option>
                    <option value="CreatedBy">Người tạo</option>
                    <option value="CreatedAt">Thời gian tạo</option>
                    <option value="UpdatedAt">Thời gian chỉnh sửa</option>
                </select>
            </div>
            <div class="flex mt-4">
                <span class="w-48 p-1 font-bold">Thứ tự</span>
                <select v-model="formSortDto.isSortAsc" class="p-1 border rounded-lg w-full">
                    <option :value="true">Tăng dần</option>
                    <option :value="false">Giảm dần</option>
                </select>
            </div>
        </div>
        <div class="flex justify-center mt-4 gap-3">
            <button @click="handleSort" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác
                nhận</button>
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>
    </div>
</template>

<script>
export default {
    name: "BlogSorter",
    props : ['action','sortDto','close'],
    data() {
        return {
            formSortDto: {
                isSortAsc : true,
                sortProp : ""
            },
            
        }
    },
    methods: {
        preset() {
            if (this.sortDto != null) {
                this.formSortDto = JSON.parse(JSON.stringify(this.sortDto));
            }
        },
        handleSort() {
            this.action(this.formSortDto)
            this.close()
        },
    },
    mounted() {
        this.preset()
    }
}
</script>

<style></style>
<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-x-auto">
        <div class="flex flex-col">
            <div class="flex ">
                <span class="w-24 p-1 font-bold">Tên</span>
                <input v-model="name" class="p-1 border rounded-lg" placeholder="Nhập tên" />
            </div>
            <div class="flex mt-4">
                <span class="w-24 p-1 font-bold">Loại</span>
                <input v-model="subjectType" class="p-1 border rounded-lg" placeholder="Nhập thể loại" />
            </div>
            <div class="flex mt-4">
                <span class="w-24 p-1 font-bold">Mô tả</span>
                <input v-model="description" class="p-1 border rounded-lg" placeholder="Nhập mô tả" />
            </div>
            <div class="flex mt-4">
                <span class="w-24 p-1 font-bold">Trang thái</span>
                <select v-model="status" class="p-1 border rounded-lg">
                    <option>
                        Active
                    </option>
                    <option>
                        Inactive
                    </option>
                </select>
            </div>
            <div>


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
import GenericPopup from '../common/GenericPopup.vue'
export default {
    components: { GenericPopup },
    name: "OperatorEditAddPopup",
    props: ['title', 'close', 'editDto'],
    data() {
        return {
            error: "",
            name: "",
            subjectType: "",
            description: "",
            status: "Active",
        }
    },
    methods: {
        presetEdit() {
            if (this.editDto != null) {
                this.name = this.editDto.name
                this.subjectType = this.editDto.subjectType
                this.description = this.editDto.description
                this.status = this.editDto.status
            }
        },
        refresh() {
            if (this.editDto != null) {
                this.presetEdit();
            }
        },

    },
    mounted() {
        this.refresh()
    }
}
</script>

<style></style>
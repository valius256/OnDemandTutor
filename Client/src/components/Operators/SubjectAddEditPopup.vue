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
                <div class="p-1 border rounded-lg">
                    <input type="radio" v-model="isEnable" :value="true">
                    Hoạt động
                    <input type="radio" v-model="isEnable" :value="false">
                    Không hoạt động
                </div>
            </div>
            <div>


            </div>
        </div>
        <div class="flex justify-center mt-4 gap-3">
            <button v-if="!editDto" @click="handleAdd" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác nhận</button>
            <button v-else @click="handleUpdate" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác nhận</button>
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>

    </div>
</template>

<script>
import axios from 'axios'
import GenericPopup from '../common/GenericPopup.vue'
export default {
    components: { GenericPopup },
    inject : ['eventBus'],
    name: "OperatorEditAddPopup",
    props: ['title', 'close', 'editDto', 'reload'],
    data() {
        return {
            error: "",
            id : 0,
            name: "",
            subjectType: "",
            description: "",
            isEnable: true,
        }
    },
    methods: {
        presetEdit() {
            if (this.editDto != null) {
                this.name = this.editDto.name
                this.subjectType = this.editDto.subjectType
                this.description = this.editDto.description
                this.isEnable = this.editDto.isEnable
            }
        },
        refresh() {
            if (this.editDto != null) {
                this.presetEdit();
            }
        },
        async handleAdd() {
            const request = {
                name: this.name,
                subjectType: this.subjectType,
                description: this.description,
                isEnable : this.isEnable,
            }
            this.eventBus.emit("open-loading-popup", {
                message: "Vui lòng chờ..."
            })
            try {

                await axios.post(import.meta.env.VITE_API_URL + `/api/subject`, request, {
                    headers : {
                        "Authorization" : "Bearer " + localStorage.token
                    }
                })

                this.eventBus.emit("open-result-dialog", {
                    message: "Tạo môn học thành công",
                    type: "Success"
                })
                this.reload()
                this.close()
            } catch (e) {
                console.log(e)
                this.eventBus.emit("open-result-dialog", {
                    message: "Có vấn đề xảy ra khi cố thêm môn học",
                    type: "Error"
                })
            }
            this.eventBus.emit("close-loading-popup")
        },
        async handleUpdate() {
            const request = {
                id : this.editDto.id,
                name: this.name,
                subjectType: this.subjectType,
                description: this.description,
                isEnable: this.isEnable,
            }
            this.eventBus.emit("open-loading-popup", {
                message: "Vui lòng chờ..."
            })
            try {

                await axios.put(import.meta.env.VITE_API_URL + `/api/subject/${this.editDto.id}`, request, {
                    headers : {
                        "Authorization" : "Bearer " + localStorage.token
                    }
                })

                this.eventBus.emit("open-result-dialog", {
                    message: "Cập nhật môn học thành công",
                    type: "Success"
                })
                this.reload()
                this.close()
            } catch (e) {
                console.log(e)
                this.eventBus.emit("open-result-dialog", {
                    message: "Có vấn đề xảy ra khi cố cập nhật môn học",
                    type: "Error"
                })
            }
            this.eventBus.emit("close-loading-popup")
        },
    },
    mounted() {
        this.refresh()
    }
}
</script>

<style></style>
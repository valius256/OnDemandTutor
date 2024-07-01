<template>
    <div class="p-4 bg-white rounded-b-lg w-full overflow-y-auto max-h-[30rem] overflow-x-auto">
        <div class="flex gap-3">
            <div>
                <div class="flex ">
                    <span class="w-24 p-1 font-bold">Tên gia sư</span>
                    <input v-model="formFilterDto.name" class="p-1 border rounded-lg" placeholder="Nhập tên" />
                </div>
            </div>
            <div>
                <div class="flex">
                    <span class="w-24 p-1 font-bold">Tạo ngày</span>
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
            </div>
        </div>
        <div class="mt-4">
            <div class="flex ">
                <span class="w-24 p-1 font-bold">Môn dậy</span>
                <div class="flex flex-wrap gap-2 w-96">
                    <div v-for="subject in selectedSubjects" :key="subject.id" class="p-1 border rounded-xl"
                        :style="{ 'border-color': subject.color }">
                        {{ subject.name }}
                        <button @click="removeSubject(subject.id)">
                            <i class="fa fa-remove ml-2"></i>
                        </button>
                    </div>
                    <button class="p-1 border rounded-xl" @click.stop="toggleShowSubjectList">
                        <span>Thêm môn dậy</span>
                        <i class="fa fa-plus ml-2"></i>
                    </button>
                </div>
            </div>
        </div>
        <div class="flex justify-center mt-4 gap-3">
            <button @click="handleFilter" class="p-2 bg-blue-400 hover:bg-blue-200 font-bold text-white rounded-lg">Xác
                nhận</button>
            <button @click="close" class="p-2 bg-red-400 hover:bg-red-200 font-bold text-white rounded-lg">Hủy
                bỏ</button>
        </div>

        <generic-popup v-if="isShowSubjectList" title="Chọn môn học" :closeFunction="hideSubjectList">
            <subject-list-for-filter-popup :close="hideSubjectList" :selectFunction="selectSubject" />
        </generic-popup>
    </div>
</template>

<script>
import GenericPopup from '../common/GenericPopup.vue';
import SubjectListForFilterPopup from './SubjectListForFilterPopup.vue';
export default {
    components: { SubjectListForFilterPopup, GenericPopup },
    name: "SubjectRegistrationFilterer",
    props: ['close', 'filterDto', 'action'],
    data() {
        return {
            formFilterDto: {
                name: "",
                fromCreateDate : null,
                toCreateDate : null,
                isChanged : false
            },
            selectedSubjects: [],
            isShowSubjectList: false,

        }
    },
    methods: {
        preset() {
            if (this.filterDto != null) {
                this.formFilterDto = JSON.parse(JSON.stringify(this.filterDto));
                this.selectedSubjects = this.filterDto.selectedSubjects
            }
        },
        handleFilter() {
            this.formFilterDto.isChanged = true;
            this.action(this.formFilterDto, this.selectedSubjects)
            this.close()
        },
        selectSubject(id, name) {
            const existedSubject = this.selectedSubjects.find(s => s.id == id)
            if (!existedSubject) {
                const randomHex = Math.floor(Math.random() * 0xFFFFFF).toString(16).padStart(6, '0');
                this.selectedSubjects.push({
                    id: id,
                    name: name,
                    color: `#${randomHex}`
                })
            }
        },
        removeSubject(id) {
            this.selectedSubjects = this.selectedSubjects.filter(s => s.id != id)
        },
        hideSubjectList() {
            this.isShowSubjectList = false;
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
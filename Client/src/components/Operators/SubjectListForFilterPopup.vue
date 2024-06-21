<template>
    <div class="p-4 bg-white rounded-b-lg w-full">
        <div class="flex gap-3 justify-center">
            <input class="p-1 border rounded-lg" v-model="keyword" placeholder="Tìm kiếm" />
            <button class="p-1 rounded-lg bg-gray-400"><i class="fa fa-search"></i></button>
        </div>
        <div class="h-96 overflow-y-auto mt-4 flex flex-col">
            <button v-for="subject in subjects" :key="subject.id" class="hover:bg-slate-200 font-bold rounded-lg p-2" @click="handleSelect(subject.id,subject.name)">
                {{ subject.name }}
            </button>
        </div>
    </div>
</template>

<script>
export default {
    name: "SubjectListForFilter",
    props : ['selectFunction','close'],
    data() {
        return {
            subjects: [
                {
                    id: 1,
                    name: "Toán"
                },
                {
                    id: 2,
                    name: "Vật lý"
                },
                {
                    id: 3,
                    name: "Hóa học"
                },
                {
                    id: 4,
                    name: "Tiếng Anh"
                },
                {
                    id: 5,
                    name: "Tiếng Nhật"
                },
                {
                    id: 6,
                    name: "Tiếng Trung"
                },
                {
                    id: 7,
                    name: "Piano"
                },
                {
                    id: 8,
                    name: "Guitar"
                }
            ],
        }
    },
    methods : {
        handleSelect(id,name){
            this.selectFunction(id,name),
            this.close()
        }
    }

}
</script>

<style></style>
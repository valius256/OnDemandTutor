<template>
    <div class="px-8 py-2">
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            <div v-for="class_ in classes" :key="class_.id" class="shadow-md rounded-lg">
                <div :class="getStatusStyleHeader(class_.status)">
                    {{ class_.name }}
                </div>
                <div class="p-4 max-h-72 overflow-y-auto">
                    <ul class="px-4 flex flex-col gap-2">
                        <li>
                            <span class="font-bold">Gia sư :</span>
                            <button class="ml-3 font-bold text-blue-400 underline">
                                {{ (class_.tutor.firstName ?? "") ?? (class_.tutor.lastName ?? "") }}
                            </button>
                        </li>
                        <li>
                            <span class="font-bold">Thời gian :</span>
                            <span class="ml-3">
                                {{ (class_.startTime?.substring(0, 10)) ?? "" }} đến
                                {{ (class_.endTime?.substring(0, 10)) ?? "" }}
                            </span>
                        </li>
                        <li>
                            <span class="font-bold">Môn học :</span>
                            <span class="ml-3">{{ class_.subject.name }}</span>
                        </li>
                        <li v-if="class_.method == 'Offline'">
                            <span class="font-bold">Địa điểm :</span>
                            <span class="ml-3">{{ class_.location }}</span>
                        </li>
                        <li>
                            <span class="font-bold">Hình thức :</span>
                            <span :class="getMethodStyle(class_.method)">{{ class_.method }}</span>
                        </li>
                        <li>
                            <span class="font-bold">Trạng thái :</span>
                            <span :class="getStatusStyle(class_.status)">
                                {{ getStatusDisplay(class_.status) }}
                            </span>
                        </li>
                        <!-- <li>
                                <span class="font-bold">Giá cả :</span>
                                <span class="ml-3 text-red-500 font-bold">
                                    {{ class_.tutor.price.toLocaleString('vi-VN', {
                            style: 'currency',
                            currency: 'VND',
                        }) }} / h
                                </span>
                            </li> -->
                    </ul>
                </div>
                <div class="flex justify-center gap-2 my-2">
                    <button v-if="class_.status == 2 && !class_.feedback && currentUser"
                        class="bg-blue-500 text-white font-bold p-2 rounded-lg"
                        @click="toggleClassRatingPopup(class_.id)">Đánh giá gia sư</button>
                    <button @click="toggleClassDetailPopup(class_.id)"
                        class="bg-blue-500 text-white font-bold p-2 rounded-lg">Xem thêm</button>
                </div>
            </div>
        </div>
        <div class="flex gap-4 justify-center mt-4" v-if="classes.length > 0">
            <button @click="movePage(false)">
                <i class="fa fa-arrow-left text-2xl"></i>
            </button>
            <div class="flex gap-2 ">
                <input class="border p-1 rounded-md w-16" type="number" v-model="currentPage" min="1"
                    @change="handleClassPageChange">
                <div class="p-1"> / {{ this.pageModel.total }}</div>
            </div>
            <button @click="movePage(true)">
                <i class="fa fa-arrow-right text-2xl"></i>
            </button>
        </div>
    </div>

</template>

<script>

export default {
    inject: ['eventBus'],
    name: "ClassList",
    props: ['classes', 'handlePageChange', 'movePage', 'currentUser', 'toggleClassDetailPopup', 'pageModel', 'toggleClassRatingPopup'],
    data() {
        return {
            currentPage: 0,
            selectedClass: 0,
        }
    },
    methods: {
        getMethodStyle(method) {
            let general = "ml-4 rounded-lg px-3 py-1 text-white font-bold"
            switch (method) {
                case "Online":
                    return general + " bg-green-400"
                default:
                    return general + " bg-gray-400"
            }
        },
        getStatusStyleHeader(status) {
            let general = "font-bold text-center py-4 rounded-t-lg text-white"
            switch (status) {
                case 0:
                    return general + " bg-cyan-500"
                case 1:
                    return general + " bg-green-400"
                default:
                    return general + " bg-gray-400"
            }
        },
        getStatusStyle(status) {
            let general = "ml-3 rounded-lg px-3 py-1 font-bold";
            switch (status) {
                case 0:
                    return general + " text-blue-400";
                case 1:
                    return general + " text-green-400";
                case 3:
                    return general + " text-red-400";
                default:
                    return general + " text-gray-400";
            }
        },
        getStatusDisplay(status) {
            console.log(status)
            switch (status) {
                case 0:
                    return "Sắp bắt đầu";
                case 1:
                    return "Đang diễn ra";
                case 2:
                    return "Đã kết thúc";
                case 3:
                    return "Đã hủy";
                default:
                    return "Không rõ";
            }
        },
        async handleClassPageChange() {
            await this.handlePageChange()
            this.currentPage = this.pageModel.page
        }

    },
    mounted() {
        this.currentPage = this.pageModel.page
    }
}
</script>

<style></style>
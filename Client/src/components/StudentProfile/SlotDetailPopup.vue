<template>
    <div class="p-4 bg-white rounded-b-lg w-full">
        <div class="flex gap-4">
            <div>
                <div class="mb-8" v-if="slot.slot.class">
                    <div>
                        <span class="font-bold">Tên lớp :</span>
                        <span class="ml-4">{{ slot.slot.class.name }}</span>
                    </div>
                    <div>
                        <span class="font-bold">Buổi thứ :</span>
                        <span class="ml-4">1/10</span>
                    </div>
                    <hr>
                </div>
                <div>
                    <span class="font-bold">Môn học :</span>
                    <span class="font-bold text-blue-400 ml-4">{{ this.slot.slot.subject.name }}</span>
                </div>
                <hr>
                <div class="mt-8">
                    <span class="font-bold">Bắt đầu :</span>
                    <span class="ml-4">{{ this.beautifyDatetime(this.slot.slot.startTime) }}</span>
                </div>
                <div>
                    <span class="font-bold">Kết thúc :</span>
                    <span class="ml-4">{{ this.beautifyDatetime(this.slot.slot.endTime) }}</span>
                </div>
                <div>
                    <span class="font-bold">Tổng thời lượng :</span>
                    <span class="ml-4">{{ calcDuration() }} tiếng</span>
                </div>
                <hr>
                <div class="mt-4">
                    <span class="font-bold">Địa điểm :</span>
                    <span class="ml-4">{{ this.slot.slot.teachAddress }}</span>
                </div>
                <div class="">
                    <span class="font-bold">Phương thức :</span>
                    <span v-if="this.slot.slot.isOnline" class="ml-4 font-bold text-green-500">Online</span>
                    <span v-else class="ml-4 font-bold text-gray-500">Offline</span>
                </div>
            </div>
            <div>
                <img class="w-48 h-48" :src="this.slot.slot.createdBy.avatarImageUrl ?? '/src/assets/noavatar.jpg'">
                <div class="mt-2 text-center">
                    <div>Gia sư</div>
                    <div class="font-bold text-2xl">
                        {{ (this.slot.slot.createdBy.firstName ?? "") + " " + (this.slot.slot.createdBy.lastName ?? "")
                        }}
                    </div>
                </div>

                <div class="">
                    <div>
                        <span class="font-bold">Email : </span>
                        <span class="italic">{{ this.slot.slot.createdBy.email }}</span>
                    </div>
                    <div>
                        <span class="font-bold">Phone : </span>
                        <span class="italoc">{{ this.slot.slot.createdBy.phone }}</span>
                    </div>
                </div>
            </div>
        </div>

        <div v-if="slot.paymentStatus == 0" class="mt-4 flex place-content-between">
            <span class="p-2 text-red-400 font-bold ">Bạn chưa thanh toán Slot này</span>
            <button class="p-2 rounded-lg bg-blue-400 hover:bg-blue-200 font-bold text-white">Thanh toán ngay</button>
        </div>
        <div v-else class="mt-4">
            <span class="p-2 text-blue-400 font-bold ">Bạn đã thanh toán Slot này</span>
        </div>
        <div class="flex flex-col justify-center mt-2" v-if="new Date(slot.slot.endTime) < new Date() && !slot.slot.class">
            <div class="text-sm italic text-center">Bạn đã hoàn tất buổi học này. Hãy để lại feedback về gia sư nhé!</div>
            <button class="bg-cyan-600 hover:bg-cyan-400 text-white font-bold p-2 rounded-lg">
                Đánh giá gia sư
            </button>
        </div>

    </div>
</template>

<script>
export default {
    name: "SlotDetailPopup",
    props: ['slot', 'close'],
    methods: {
        calcDuration() {
            const startTime = new Date(this.slot.slot.startTime);
            const endTime = new Date(this.slot.slot.endTime);
            return (endTime - startTime) / 3600000;
        }
    }
}
</script>

<style></style>
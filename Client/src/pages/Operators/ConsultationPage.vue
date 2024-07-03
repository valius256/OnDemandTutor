<template>
    <div class="p-4 w-full" @click="setSelectId(0)">
        <div class="text-2xl font-bold">
            Yêu cầu tư vấn
        </div>
        <div class="mt-12">
            <table id="operator-table" class="table-auto overflow-x-auto">
                <thead>
                    <tr>
                        <th class="w-1/12">Id</th>
                        <th class="w-2/12">Phone</th>
                        <th class="w-3/12">Email</th>
                        <th class="w-4/12">Message</th>
                        <th class="w-2/12">Status</th>
                        <th class="w-1/12"></th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="request in requests" :key="request.id">
                        <td>{{ request.id }}</td>
                        <td>{{ request.phone }}</td>
                        <td class="break-all">{{ request.email }}</td>
                        <td>{{ request.message }}</td>
                        <td>
                            <div :class="getStatusStyle(request.status)">{{ request.status }}</div>
                        </td>
                        <td class="relative">
                            <button class="p-2 bg-slate-200 hover:bg-slate-400 font-bold rounded-full"
                                @click.stop="setSelectId(request.id)">
                                <i class="fa fa-ellipsis-h	"></i>
                            </button>
                            <div v-if="selectId == request.id"
                                class="absolute right-0 bg-white rounded-lg shadow-lg z-10 w-48 animate-fade-down animate-duration-[400ms] animate-normal font-bold flex flex-col">
                                <!-- Content of your menu -->
                                <button v-if='request.status == "Pending"' class="hover:bg-slate-200 p-2 rounded-t-lg text-left text-green-400"
                                    @click="handleResolve(request.id)">
                                    <i class="fa fa-check mr-4"></i>Đã giải quyết
                                </button>
                                <button v-if='request.status == "Done"' class="hover:bg-slate-200 p-2 rounded-t-lg text-left text-red-400"
                                    @click="handleResolve(request.id)">
                                    <i class="fa fa-close mr-4"></i>Chưa giải quyết
                                </button>
                                <!-- <li class="hover:bg-slate-200 p-2"></li> -->
                                <button class="hover:bg-slate-200 p-2 rounded-b-lg text-left">
                                    <i class="fa fa-trash mr-4"></i>Xóa
                                </button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="flex gap-4 justify-center mt-4" v-if="this.requests.length > 0">
            <button @click="movePage(false)">
                <i class="fa fa-arrow-left text-2xl"></i>
            </button>
            <div class="flex gap-2 ">
                <input class="border p-1 rounded-md w-16" type="number" v-model="currentPage" min="1"
                    @change="handlePageChange">
                <div class="p-1"> / {{ this.totalPage }}</div>
            </div>
            <button @click="movePage(true)">
                <i class="fa fa-arrow-right text-2xl"></i>
            </button>
        </div>
    </div>
</template>

<script>
export default {
    name: "ConsultationPage",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            selectId : 0,
            requests: [
                {
                    id: 1,
                    phone: "0987654321",
                    email: "hungttse173643@fpt.edu.vn",
                    message: "SOS",
                    status: "Pending"
                },
                {
                    id: 2,
                    phone: "0987654321",
                    email: "hungttse173643@fpt.edu.vn",
                    message: "The break-words utility in Tailwind CSS can be applied directly to a td tag to ensure that content within the cell breaks and wraps as needed. However, Tailwind CSS uses the break-all utility instead of break-words. If you need to break words within table cells, you can use break-all or other relevant utilities for text wrapping and overflow handling",
                    status: "Done"
                }
            ],

        }
    },
    methods: {
        getStatusStyle(status) {
            let css = "text-center font-bold text-white rounded-lg"
            switch (status) {
                case "Pending":
                    return css + " bg-red-400"
                case "Done":
                    return css + " bg-green-400"
            }
        },
        async handlePageChange() {
            if (this.currentPage > this.totalPage) {
                this.currentPage = this.totalPage
            }
            if (this.currentPage < 1) {
                this.currentPage = 1
            }
            //await this.fetchRegistration(this.currentPage, this.pageSize, this.keyword_name)
        },
        async movePage(forward) {
            if (forward && this.currentPage < this.totalPage) {
                this.currentPage++
                await this.handlePageChange()
            } else if (!forward && this.currentPage > 1) {
                this.currentPage--
                await this.handlePageChange()
            }
        },
        setSelectId(id) {
            if (id == this.selectId) {
                this.selectId = 0
            } else {
                this.selectId = id
                this.isShowPopup = true
            }
        },
    }
}
</script>

<style></style>
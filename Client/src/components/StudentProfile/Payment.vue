<template>
    <div>
        <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200 ">
            Thông tin thanh toán
        </div>
        <div class="flex justify-center mb-8">
            <div class="text-3xl font-bold py-1">
                <div class="mb-4">Số dư hiện tại </div>
                <div class="text-green-200 p-1 bg-green-600 rounded-lg text-center">
                    {{ user.balance.toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                    }) }} </div>
            </div>
        </div>
        <div class="flex gap-4 justify-center mt-4 text-2xl mb-6">
            <button @click="null"
                class="mr-6 px-6 py-4 font-bold text-white bg-blue-400 hover:bg-blue-200 rounded-lg">Nạp tiền</button>
            <button @click="null" class="px-6 py-4 font-bold text-white bg-green-400 hover:bg-green-200 rounded-lg">Rút
                tiền</button>
        </div>
        <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200 ">
            Lịch sử giao dịch
        </div>
        <div class="px-4 mb-4">
            <table class="bg-slate-50 p-6 rounded-xl text-center w-full" v-if="transactions.length > 0">
                <thead>
                    <th>Id</th>
                    <th>Code</th>
                    <th>Date</th>
                    <th>Amount</th>
                    <th>Description</th>
                </thead>
                <tbody>
                    <tr v-for="transaction in transactions" :key="transaction.id">
                        <td>{{ transaction.id }}</td>
                        <td>{{ transaction.code }}</td>
                        <td>{{ transaction.date }}</td>
                        <td :class="getAmountStyle(transaction.amount)">{{ transaction.amount }}</td>
                        <td>{{ transaction.description }}</td>
                    </tr>
                </tbody>
            </table>
            <div class="flex gap-4 justify-center mt-4" v-if="transactions.length > 0">
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
            <div v-else class="text-center italic">
                Hiện chưa có giao dịch nào
            </div>
        </div>

    </div>

</template>

<script>
export default {
    name: "StudentProfilePayment",
    data() {
        return {
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
            user: {
                balance: 100000
            },
            transactions: [
                {
                    id: 1,
                    code: 129389102,
                    date: "2024-01-01 12:00:01",
                    amount: 80000,
                    description: "Nap cho slot 1"
                },
                {
                    id: 2,
                    code: 2189479,
                    date: "2024-01-03 12:00:01",
                    amount: -80000,
                    description: "Tru tien slot 1"
                },
            ]
        }
    },
    methods : {
        getAmountStyle(amount) {
            let css = "font-bold"
            if (amount < 0){
                return css + " text-red-400"
            } else {
                return css + " text-green-400"
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
    }
}
</script>

<style scoped>
tr td,
th {
    padding: 0.5rem 2rem 0.5rem 2rem;
    border: solid 1px #ffffff
}
</style>
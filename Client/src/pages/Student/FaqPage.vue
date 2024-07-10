<template>
    <div class="container mx-auto py-8">
        <h1 class="text-3xl font-bold mb-6 text-center">Những câu hỏi thường gặp (FAQ)</h1>
        <div class="mb-4 flex justify-end">
            <input v-model="keyword" placeholder="Tìm kiếm..." class="p-2 w-64 bg-gray-100 rounded-lg" @change="debouncedSearch">
        </div>
        <div v-for="(faq, index) in faqs" :key="index" class="mb-4">
            <button @click="toggle(index)" class="w-full text-left p-4 bg-gray-100 rounded-lg shadow-md">
                <h2 class="text-xl font-semibold">{{ faq.question }}</h2>
            </button>
            <div v-if="activeIndex === index"
                class="animate-flip-down animate-fill-both p-4 bg-white border border-gray-200 rounded-lg shadow-md mt-2">
                <p>{{ faq.answer }}</p>
            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import debounce from 'lodash/debounce';

export default {
    name: "FAQPage",
    data() {
        return {
            activeIndex: null,
            faqs: [

            ],
            keyword : "",
        };
    },
    methods: {
        async FetchFAQ() {
            let query = {
                Sorts: {
                    Column: "id",
                    IsDesc: false
                },
            }
            if (this.keyword){
                query["Filter.Question"] = this.keyword
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/FAQ/all?' +
                this.jsonToQueryString(query))
            if (response.data) {
                this.faqs = response.data.items
            }
        },
        toggle(index) {
            this.activeIndex = this.activeIndex === index ? null : index;
        },
        debouncedSearch: debounce(async function (event) {
            await this.FetchFAQ();
        }, 300) // Adjust the debounce delay as needed
    },
    mounted() {
        this.FetchFAQ()
    }
};
</script>

<style>
/* Add any additional custom styles here */
.change-height {
    transition: height 0.3s ease;
}
</style>
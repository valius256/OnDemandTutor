<template>
    <div class="p-4 bg-white rounded-b-lg w-full">
        <div class="flex gap-3 justify-center">
            <input class="p-1 border rounded-lg" v-model="keyword" placeholder="Tìm kiếm" />
            <button class="p-1 rounded-lg bg-gray-400 hover:bg-gray-200" @click="fetchSubject"><i class="fa fa-search"></i></button>
        </div>
        <div class="h-96 overflow-y-auto mt-4 flex flex-col">
            <button v-for="subject in subjects" :key="subject.id" class="hover:bg-slate-200 font-bold rounded-lg p-2" @click="handleSelect(subject.id,subject.name)">
                {{ subject.name }}
            </button>
        </div>
    </div>
</template>

<script>
import axios from 'axios'

export default {
    name: "SubjectListForFilter",
    props : ['selectFunction','close'],
    data() {
        return {
            keyword : "",
            subjects: [
                
            ],
        }
    },
    methods : {
        async fetchSubject(){
            let query = {
                "Filter.Name" : this.keyword
            }
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/subject?'+ 
            this.jsonToQueryString(query))
            if (response.data) {
                this.subjects = response.data.items
            }
        },
        handleSelect(id,name){
            this.selectFunction(id,name),
            this.close()
        }
    },
    mounted(){
        this.fetchSubject()
    }

}
</script>

<style></style>
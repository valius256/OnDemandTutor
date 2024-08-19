<template>
    <div class="">
        <div class="text-2xl font-bold mb-6 px-6 py-8 bg-slate-200 ">
            Lớp học bạn đã tham gia
        </div>
        <class-list v-if="!isOpenClassDetailPopup" :classes="classes" :handlePageChange="handlePageChange" :movePage="movePage" :currentUser="user" :toggleClassDetailPopup="toggleClassDetailPopup" :pageModel="{total : totalPage, page : currentPage}"></class-list>
        <div v-else>
            <button class="ml-8 px-8 py-2 bg-blue-400 font-bold text-white rounded-lg"
                @click="toggleClassDetailPopup">Trở
                về</button>
            <ClasssDetailPopup :classId="selectedClass" :close="toggleClassDetailPopup" :action="fetchData"></ClasssDetailPopup>
        </div>
        <generic-popup title="Đánh giá lớp học" v-if="isOpenRatingPopup" :closeFunction="toggleClassRatingPopup">
            <rating-popup :classId="selectedClass" :close="toggleClassRatingPopup" :action="fetchData"></rating-popup>
        </generic-popup>
    </div>
</template>

<script>
import axios from 'axios'
import ClassList from '../common/ClassList.vue'
import ClasssDetailPopup from './ClassDetailPopup.vue'
import GenericPopup from '../common/GenericPopup.vue'
export default {
    components: { ClassList, ClasssDetailPopup, GenericPopup },
    inject: ['eventBus'],
    name: "StudentClasses",
    data() {
        return {
            classes: [],
            isOpenClassDetailPopup: false,
            isOpenRatingPopup: false,
            user : null,
            totalPage: 100,
            pageSize: 10,
            currentPage: 1,
        }
    },
    methods: {
        async fetchData() {
            let query = {
                Page: this.currentPage,
                Limit: this.pageSize
            }
            let queryStr = this.jsonToQueryString(query)
            //console.log(import.meta.env.VITE_API_URL + '/api/subject?' + this.jsonToQueryString(query))
            const response = await axios.get(import.meta.env.VITE_API_URL + '/api/Class/student?' +
                queryStr, {
                headers: {
                    "Authorization": "Bearer " + localStorage.token
                }
            })
            if (response.data) {
                this.classes = response.data.items
                this.totalPage = Math.ceil(response.data.total / this.pageSize)
            }

        },
        async getUser(){
            this.user = await this.getUserFromToken()
        },
        toggleClassDetailPopup(id) {
            scrollTo(0, 0)
            this.selectedClass = id
            this.isOpenClassDetailPopup = !this.isOpenClassDetailPopup
        },
        toggleClassRatingPopup(id) {
            this.selectedClass = id
            this.isOpenRatingPopup = !this.isOpenRatingPopup
        },
        async handlePageChange() {
            if (this.currentPage > this.totalPage) {
                this.currentPage = this.totalPage
            }
            if (this.currentPage < 1) {
                this.currentPage = 1
            }
            await this.fetchData()
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

    },
    mounted() {
        this.getUser()
        this.fetchData()
    }
}
</script>

<style></style>
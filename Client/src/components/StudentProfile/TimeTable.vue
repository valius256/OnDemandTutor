<template>
    <div class="p-12">
        <div class="flex flex-wrap lg:flex-row gap-16">
            <div class="text-2xl">Timetable:</div>
            <div class="font-bold flex gap-8">
                <button @click="moveWeek(false)"
                    class="bg-gray-300 rounded-xl px-2 hover:bg-slate-100  text-5xl ">◄</button>
                <select v-model="selectedWeek" @change="handleSelectedWeekChange"
                    class="text-xl font-normal  rouded-lg rouded-lg border">
                    <option v-for="week in weeksInYear" :key="week.start" :value="week.start">
                        <div v-if="week.start">
                            {{ week.start.toLocaleDateString() + " - " + week.end.toLocaleDateString() }}
                        </div>
                    </option>
                </select>
                <button @click="moveWeek(true)"
                    class="bg-gray-300 rounded-xl px-2 hover:bg-slate-100  text-5xl ">►</button>
            </div>
            <select v-model="selectedYear" @change="handleSelectedYearChange"
                class="text-xl font-normal rouded-lg border px-4">
                <option v-for="year in years" :key="year" :value="year">{{ year }}</option>
            </select>
            <div class="flex gap-2">
                <button @click="setZoom(1)" class="bg-gray-300 rounded-xl px-4 hover:bg-slate-100  ">
                    <i class="fa fa-search-plus	"></i>
                </button>
                <button @click="setZoom(-1)" class="bg-gray-300 rounded-xl px-4 hover:bg-slate-100 ">
                    <i class="fa fa-search-minus"></i>
                </button>
            </div>
        </div>
        <div>
            <table class="w-full border-collapse">
                <thead>
                    <tr>
                        <th class="py-2"></th>
                        <th v-for="day in daysInWeek" :key="day.dayInWeek" class="py-2">
                            {{ day.dayInWeek }}<br>{{ day.specificDay }}
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="">
                        <td class=" border-r-2 w-16 flex flex-col items-start">
                            <div class="mb-4" v-for="shift in shifts" :key="shift">
                                {{ formatTime(shift.hour) }} : {{ formatTime(shift.min) }}
                            </div>
                        </td>
                        <td class="relative border-r-2" v-for="day in daysInWeek" :key="day.dayInWeek"
                        :class="{'bg-slate-100': compareDateToToday(day.specificDay)}">
                            <slot-detail :slots="getSlotsByDay(day.specificDay)" 
                                :shiftZoomSize="shiftZoomSize" :getDistanceInMin="getDistanceInMin" :viewDetail="viewDetail"/>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>

<script>
import SlotDetail from './SlotDetail.vue';
export default {
  components: { SlotDetail },
    name: "StudentTimeTable",
    props: ['slots','fetching','viewDetail','isGuest'],
    data() {
        return {
            daysInWeek: [
                { dayInWeek: "Sun", specificDay: "07/01" },
                { dayInWeek: "Mon", specificDay: "01/01" },
                { dayInWeek: "Tue", specificDay: "02/01" },
                { dayInWeek: "Wed", specificDay: "03/01" },
                { dayInWeek: "Thu", specificDay: "04/01" },
                { dayInWeek: "Fri", specificDay: "05/01" },
                { dayInWeek: "Sat", specificDay: "06/01" },
            ],
            weeksInYear: [
                {
                    start: null,
                    end: null
                }
            ],
            years: [

            ],
            shifts: [
                { hour: 0, min: 0 },
                { hour: 1, min: 0 },
                { hour: 2, min: 0 }
            ],
            selectedWeek: null,
            selectedYear: new Date().getFullYear(),
            shiftZoomSize: 3
        }
    },
    methods: {
        async handleSelectedWeekChange() {
            for (let i = 0; i < 7; i++) {
                const nextDay = new Date(this.selectedWeek);
                nextDay.setDate(this.selectedWeek.getDate() + i);
                const dateStr = this.toSqlDateString(nextDay)
                console.log(dateStr)
                this.daysInWeek[i].specificDay = this.sqlDateStringToSlashFormat(dateStr)
            }
            let endDate = new Date(this.selectedWeek)
            endDate.setDate(this.selectedWeek.getDate() + 7)
            console.log(this.daysInWeek)
            await this.fetching(this.toSqlDateString(this.selectedWeek), this.toSqlDateString(endDate))
            //await this.fetchLessons(this.selectedWeek, endDate)
        },
        async handleSelectedYearChange() {
            this.weeksInYear = this.getWeeksOfYear(this.selectedYear)
            this.selectedWeek = this.weeksInYear[0].start
            await this.handleSelectedWeekChange()
        },

        moveWeek(forward) {
            const currentIndex = this.weeksInYear.findIndex((week) => {
                return this.compareDate(week.start, this.selectedWeek) === 0;
            });
            if (forward && currentIndex < this.weeksInYear.length - 1) {
                this.selectedWeek = this.weeksInYear[currentIndex + 1].start
                this.handleSelectedWeekChange()
            } else if (!forward && currentIndex > 0) {
                this.selectedWeek = this.weeksInYear[currentIndex - 1].start
                this.handleSelectedWeekChange()
            }
        },
        async refresh() {
            this.weeksInYear = this.getWeeksOfYear(this.selectedYear)
            this.years = this.getYears()
            this.selectedWeek = this.getFirstDayOfWeek()
            this.getShifts()
            await this.handleSelectedWeekChange()
        },
        formatTime(value) {
            return String(value).padStart(2, '0');
        },
        getDistanceInMin(zoomSize) {
            switch (zoomSize) {
                case 1:
                    return 15;
                case 2:
                    return 30;
                case 3:
                    return 60;
                case 4:
                    return 120;
                case 5:
                    return 240;
                default:
                    return 60; // Default value if shiftZoomSize is not set correctly
            }
        },
        getShifts() {
            let distanceInMin = this.getDistanceInMin(this.shiftZoomSize);

            this.shifts = []
            const minutesInDay = 24 * 60;

            for (let i = 0; i < minutesInDay; i += distanceInMin) {
                const hour = Math.floor(i / 60);
                const min = i % 60;
                this.shifts.push({ hour, min });
            }
        },
        setZoom(zoomAmount) {
            this.shiftZoomSize += zoomAmount
            if (this.shiftZoomSize < 1 || this.shiftZoomSize > 5) {
                this.shiftZoomSize -= zoomAmount
            }
            this.getShifts()
        },
        getSlotsByDay(date) {
            const dateToCompare = (new Date(this.slashDateFormatToSqlDateString(date)).getDate())
            //console.log(new Date(this.slots[0].slot.startTime).getDate(), dateToCompare)
            //console.log(this.slots.filter(s => new Date(s.slot.startTime).getDate() == dateToCompare))
            return this.slots.filter(s => new Date(s.slot.startTime).getDate() == dateToCompare)
        },
        compareDateToToday(date){
            const dateToCompare = (this.slashDateFormatToSqlDateString(date))
            const today = new Date()
            const todayDateString = `${today.getFullYear()}-${String((today.getMonth() + 1)).padStart(2,'0')}-${String(today.getDate()).padStart(2,'0')}`
            console.log(dateToCompare, todayDateString)
            return dateToCompare == todayDateString
        }
    },
    mounted() {
        this.refresh()
    }
}
</script>

<style></style>
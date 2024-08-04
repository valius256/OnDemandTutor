<template></template>

<script>
import axios from 'axios';
export default {
  inject: ['eventBus'],
  methods: {
    async getUserFromToken() {
      if (localStorage.token) {
        try {
          const response = await axios.get(
            import.meta.env.VITE_API_URL + "/api/auth/who-am-i",
            {
              headers: {
                Authorization: "Bearer " + localStorage.token,
              },
            }
          );
          var user = response.data.data;
          if (!user.isActive) {
            this.eventBus.emit("open-result-dialog", {
              message: "Tài khoản của bạn đã bị ngưng hoạt động. Lý do : " + user.deaActiveReason,
              type: "Error",
              callback: this.clearToken
            })
          }
          return user
        } catch (e) {
          console.log(e)
          //console.log("Token can't be used")
          this.eventBus.emit("open-result-dialog", {
            message: "Phiên đã hết hạn. Vui lòng đăng nhập lại",
            type: "Error",
            callback: this.clearToken
          })
        }
      }
      //this.eventBus.emit("close-loading-popup")

      return null
    },
    clearToken() {
      this.$router.push("/login");
      localStorage.removeItem("token")
      this.eventBus.emit("update-everything")
    },
    getWeeksOfYear(year) {
      const weeks = [];

      const startDate = new Date(year, 0, 1); // Adjusted month value to 0 (January)
      const firstSunday = startDate.getDate() + (7 - startDate.getDay());
      startDate.setDate(firstSunday);

      while (startDate.getFullYear() === year) {
        const endDate = new Date(startDate);
        endDate.setDate(startDate.getDate() + 6);

        weeks.push({
          start: new Date(startDate), // Create new Date objects for start and end
          end: new Date(endDate), // to avoid referencing the same object
        });

        startDate.setDate(startDate.getDate() + 7);
      }

      return weeks;
    },
    getFirstDayOfWeek() {
      const today = new Date();
      const dayOfWeek = today.getDay();
      const firstDay = new Date(today);
      firstDay.setDate(today.getDate() - dayOfWeek);

      // Set hours, minutes, seconds, and milliseconds to zero
      firstDay.setHours(0, 0, 0, 0);

      return firstDay;
    },
    getYears() {
      const years = [];
      let year = new Date().getFullYear();
      years.push(year);
      for (var i = 0; i < 3; i++) {
        year -= 1;
        years.push(year);
      }
      return years;
    },
    compareDate(date1, date2) {
      if (date1 > date2) return 1;
      if (date1 < date2) return -1;
      return 0;
    },
    toSqlDateString(date) {
      const year = date.getFullYear(); // Get the year (4 digits)
      const month = String(date.getMonth() + 1).padStart(2, "0"); // Get the month (0-11) and pad with leading zero if needed
      const day = String(date.getDate()).padStart(2, "0"); // Get the day of the month and pad with leading zero if needed
      return `${year}-${month}-${day}`; // Concatenate the year, month, and day with hyphens
    },
    toFullSqlDateString(date) {
      const year = date.getFullYear(); // Get the year (4 digits)
      const month = String(date.getMonth() + 1).padStart(2, "0"); // Get the month (0-11) and pad with leading zero if needed
      const day = String(date.getDate()).padStart(2, "0"); // Get the day of the month and pad with leading zero if needed
      const hour = String((date.getHours())).padStart(2, "0")
      const min = String((date.getMinutes())).padStart(2, "0")
      const sec = String((date.getSeconds())).padStart(2, "0")
      return `${year}-${month}-${day} ${hour}:${min}:${sec}`; // Concatenate the year, month, and day with hyphens
    },
    toTimeString(date){
      const hour = String((date.getHours())).padStart(2, "0")
      const min = String((date.getMinutes())).padStart(2, "0")
      const sec = String((date.getSeconds())).padStart(2, "0")
      return `${hour}:${min}:${sec}`; 
    },
    sqlDateStringToSlashFormat(dateString) {
      const date = new Date(dateString)
      return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`; // Concatenate the year, month, and day with hyphens
    },
    slashDateFormatToSqlDateString(dateStr) {
      const dateParts = dateStr.split("/");
      if (dateParts[0].length == 1) {
        dateParts[0] = "0" + dateParts[0];
      }
      if (dateParts[1].length == 1) {
        dateParts[1] = "0" + dateParts[1];
      }
      return `${dateParts[2]}-${dateParts[1]}-${dateParts[0]}`;
    },
    jsonToQueryString(jsonObject) {
      return Object.keys(jsonObject)
        .map(key => {
          let value = jsonObject[key];
          if (typeof value === 'object') {
            value = JSON.stringify(value);
          }
          return `${encodeURIComponent(key)}=${encodeURIComponent(value)}`;
        })
        .join('&');
    },
    //For example 2024-07-03T21:58:53.1949788
    beautifyDatetime(datetimeStr) {
      if (datetimeStr) {
        return datetimeStr.substring(8, 10) + "/" + datetimeStr.substring(5, 7) + "/" + datetimeStr.substring(0, 4) + " lúc " + datetimeStr.substring(11, 19)
      }
      return ""
    },
    formatDatetime(date, time) {
      if (date && time) {
        return `${date} ${time}:00`;
      }
      return '';
    },
    // Define other global methods here if needed
  },
};
</script>

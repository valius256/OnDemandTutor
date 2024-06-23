<template></template>

<script>
export default {
  methods: {
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
    // Define other global methods here if needed
  },
};
</script>

<template>
    <div class="w-full ">
      <div>
        <div class="flex flex-wrap gap-4 font-bold">
          <router-link :to="'/tutor-guest/' + viewingId + '/profile'" class="px-4 py-2 rounded-lg hover:bg-blue-800">
            <i class="fa fa-user text-2xl mr-2"></i>Thông tin gia sư
          </router-link>
          <router-link :to="'/tutor-guest/' + viewingId + '/videos'" class="px-4 py-2 rounded-lg hover:bg-blue-800">
            <i class="fa fa-film text-2xl mr-2"></i>Video
          </router-link>
          <router-link :to="'/tutor-guest/' + viewingId + '/schedule'" class="px-4 py-2 rounded-lg hover:bg-blue-800">
            <i class="fa fa-calendar text-2xl mr-2"></i>Thời khóa biểu
          </router-link>
          <router-link :to="'/tutor-guest/' + viewingId + '/subject'" class="px-4 py-2 rounded-lg hover:bg-blue-800">
            <i class="fa fa-calendar text-2xl mr-2"></i>Môn học
          </router-link>
          <router-link :to="'/tutor-guest/' + viewingId + '/class'" class="px-4 py-2 rounded-lg hover:bg-blue-800">
            <i class="fa fa-group text-2xl mr-2"></i>Lớp học
          </router-link>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: "TutorProfileNavigator",
    inject: ["eventBus"],
    props : ['viewingId'],
    methods: {

    },
  };
  </script>
  
  <style scoped>
  .router-link-active {
    background: #0a0030;
    color: white;
  }
  
  .sub-nav .router-link-active {
    background: #0c0038;
  }
  </style>
  
<template>
    <div class="popup-overlay ">
        <div class="flex flex-col items-center animate-jump-in w-screen lg:w-auto" 
        :class="{'overflow-y-auto' : !notOverflow}">
            <div class="flex place-content-between gap-8 bg-slate-700 p-2 rounded-t-lg w-full">
                <div class="font-bold text-white">{{ title }}</div>
                <button class="w-8 h-8 bg-red-400 text-white rounded-full" @click="closeFunction">X</button>
            </div>
            <slot></slot>
        </div>
    </div>
</template>

<script>
export default {
    name: "GenericPopup",
    props : ['title','closeFunction','notOverflow']
}
</script>

<style></style>
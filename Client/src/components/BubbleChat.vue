<template>
  <div class="bubble_chat_wrapper" :style="{ [position]: 0 }">
    <transition name="slide">
      <div class="bubble_chat_wrapper__rolled_up" v-show="!windowMode">
        <div class="messages_list">
          <div
            class="messages_list__item"
            v-for="(msgItem, index) in activeMessages"
            @click="openChat"
            :key="index + '_bubbles'"
            :class="{
              'hiding-out': index === 0 && messageUpdating,
              'showing-in': index === 2 && messageUpdating,
              'shake-it': index === 1 && messageUpdating,
            }"
          >
            <div class="messages_list__item___sender">
              <img
                v-if="msgItem[avatarLinkField]"
                :src="msgItem[avatarLinkField]"
                :alt="msgItem[senderNameField]"
              />
              <span v-if="!msgItem[avatarLinkField]">
                {{
                  msgItem[senderNameField]
                    ? msgItem[senderNameField].charAt(0).toUpperCase()
                    : "?"
                }}
              </span>
            </div>
            <div class="messages_list__item___message">
              <span class="name">{{ msgItem[senderNameField] }}</span>
              <span>{{ msgItem[textField] }}</span>
            </div>
          </div>
        </div>
      </div>
    </transition>

    <transition name="slide">
      <div class="bubble_chat_wrapper__rolled_down" v-show="windowMode">
        <div class="bubble_chat_wrapper__rolled_down__list" ref="scrollWrapper">
          <div class="scroll">
            <div
              v-for="(msgObj, index) in fullMesagesList"
              :key="index"
              :class="{ mine: msgObj.isMine }"
            >
              <div>
                <img
                  v-if="msgObj[avatarLinkField]"
                  :src="msgObj[avatarLinkField]"
                  alt="avatar"
                />
                <span class="no_image" v-if="!msgObj[avatarLinkField]">
                  {{
                    msgObj[senderNameField]
                      ? msgObj[senderNameField].charAt(0).toUpperCase()
                      : "?"
                  }}
                </span>
                <span class="name">{{ msgObj[senderNameField] }}</span>
                <span class="msg">{{ msgObj[textField] }}</span>
              </div>
            </div>
          </div>
        </div>
        <div class="bubble_chat_wrapper__rolled_down__input">
          <textarea
            v-model="newMessageText"
            placeholder="Use 'Shift + Enter' to send"
            v-on:keyup.enter.shift="sendMessage"
          ></textarea>
        </div>
      </div>
    </transition>

    <div class="bubble_chat_wrapper__control" @click="openChat">
      <span
        class="new_mesages"
        v-show="!windowMode && unredMessagesCount > 0"
        >{{ unredMessagesCount }}</span
      >
      <svg
        v-if="!windowMode"
        xmlns="http://www.w3.org/2000/svg"
        xmlns:xlink="http://www.w3.org/1999/xlink"
        version="1.1"
        id="Capa_1"
        x="0px"
        y="0px"
        viewBox="0 0 524.184 524.184"
        style="
          enable-background: new 0 0 524.184 524.184;
          fill: #fff;
          -webkit-transform: scale(0.6);
          -ms-transform: scale(0.6);
          transform: scale(0.6);
        "
        xml:space="preserve"
      >
        <g>
          <path
            d="M483.606,75.31H40.542C18.34,75.31,0,93.65,0,115.852v292.48c0,22.201,18.34,40.542,40.542,40.542h443.064c22.201,0,40.542-18.34,40.542-40.542v-292.48C525.113,93.65,506.772,75.31,483.606,75.31z M332.057,273.193L500.981,142.88v261.591L332.057,273.193z M41.507,99.442h442.098c7.722,0,14.479,5.792,16.41,12.549L276.07,285.741c-8.688,6.757-23.167,6.757-30.889,0L24.132,113.921C26.063,106.199,32.82,99.442,41.507,99.442z M24.132,404.471V144.81l166.994,129.348L24.132,404.471z M41.507,426.672c-1.931,0-3.861,0-4.826-0.965l174.716-136.105l19.306,15.444c8.688,6.757,19.306,9.653,29.924,9.653c10.618,0,22.201-2.896,29.924-9.653l21.236-16.41l176.646,137.07c-1.931,0.965-2.896,0.965-4.826,0.965H41.507z"
          />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
          <g />
        </g>
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
      </svg>

      <svg
        v-if="windowMode"
        xmlns="http://www.w3.org/2000/svg"
        xmlns:xlink="http://www.w3.org/1999/xlink"
        version="1.1"
        id="Capa_1"
        x="0px"
        y="0px"
        viewBox="0 0 47.971 47.971"
        style="
          enable-background: new 0 0 47.971 47.971;
          fill: #fff;
          -webkit-transform: scale(0.3);
          -ms-transform: scale(0.3);
          transform: scale(0.3);
        "
        xml:space="preserve"
      >
        <g>
          <path
            d="M28.228,23.986L47.092,5.122c1.172-1.171,1.172-3.071,0-4.242c-1.172-1.172-3.07-1.172-4.242,0L23.986,19.744L5.121,0.88c-1.172-1.172-3.07-1.172-4.242,0c-1.172,1.171-1.172,3.071,0,4.242l18.865,18.864L0.879,42.85c-1.172,1.171-1.172,3.071,0,4.242C1.465,47.677,2.233,47.97,3,47.97s1.535-0.293,2.121-0.879l18.865-18.864L42.85,47.091c0.586,0.586,1.354,0.879,2.121,0.879s1.535-0.293,2.121-0.879c1.172-1.171,1.172-3.071,0-4.242L28.228,23.986z"
          />
        </g>
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
        <g />
      </svg>
    </div>
  </div>
</template>

<script>
export default {
  name: "BubbleChat",
  props: {
    position: {
      type: String,
      default: "right",
      validator: (value) => value === "right" || value === "left",
    },
    messages: {
      type: Array,
      default: () => [],
    },
    textField: {
      type: String,
      default: "text",
    },
    senderNameField: {
      type: String,
      default: "name",
    },
    avatarLinkField: {
      type: String,
    },
  },
  data() {
    return {
      messageUpdating: false,
      windowMode: false,
      newMessageText: "",
      fullMesagesList: [],
      checkedMessagesIndex: 0,
    };
  },
  computed: {
    activeMessages() {
      if (this.fullMesagesList.length < 3) {
        return this.fullMesagesList;
      }
      return this.fullMesagesList.slice(-3);
    },
    unredMessagesCount() {
      return this.fullMesagesList.length - this.checkedMessagesIndex;
    },
  },
  methods: {
    sendMessage() {
      this.$emit("send", this.newMessageText);
      this.fullMesagesList.push({
        [this.senderNameField]: "Me",
        [this.textField]: this.newMessageText,
        isMine: true,
      });
      this.newMessageText = "";
      this.scrollDownMessagesList();
    },
    scrollDownMessagesList() {
      const wrapper = this.$refs.scrollWrapper;
      this.$nextTick(() => {
        wrapper.scrollTop = wrapper.scrollHeight;
      });
    },
    openChat() {
      this.windowMode = !this.windowMode;
      if (this.windowMode) {
        this.checkedMessagesIndex = this.fullMesagesList.length;
        this.scrollDownMessagesList();
      }
    },
  },
  watch: {
    messages: {
      handler(newVal) {
        if (newVal.length) {
          this.fullMesagesList.push({
            ...newVal[newVal.length - 1],
            isMine: false,
          });
          if (this.windowMode) {
            this.checkedMessagesIndex = this.fullMesagesList.length;
            this.scrollDownMessagesList();
          }
          this.messageUpdating = true;
          setTimeout(() => {
            this.messageUpdating = false;
          }, 700);
        }
      },
      deep: true,
    },
  },
};
</script>

<style scoped>
.bubble_chat_wrapper {
  position: fixed;
  bottom: 0;
  width: 300px;
  height: 400px;
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
  font-family: "Arial", sans-serif;
}

.bubble_chat_wrapper__control {
  background-color: #007bff;
  color: #fff;
  padding: 10px;
  cursor: pointer;
  display: flex;
  justify-content: center;
  align-items: center;
  border-radius: 50%;
  width: 50px;
  height: 50px;
}

.bubble_chat_wrapper__rolled_up,
.bubble_chat_wrapper__rolled_down {
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 5px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
  overflow: hidden;
}

.bubble_chat_wrapper__rolled_up {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
}

.messages_list {
  display: flex;
  flex-direction: column;
  width: 100%;
  max-height: 300px;
  overflow-y: auto;
}

.messages_list__item {
  padding: 10px;
  border-bottom: 1px solid #ddd;
  display: flex;
  align-items: center;
  cursor: pointer;
}

.messages_list__item___sender {
  width: 30px;
  height: 30px;
  border-radius: 50%;
  background: #007bff;
  color: #fff;
  display: flex;
  justify-content: center;
  align-items: center;
  margin-right: 10px;
  font-size: 14px;
}

.messages_list__item___sender img {
  width: 100%;
  height: 100%;
  border-radius: 50%;
}

.messages_list__item___message .name {
  font-weight: bold;
  display: block;
  margin-bottom: 2px;
}

.bubble_chat_wrapper__rolled_down {
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
}

.bubble_chat_wrapper__rolled_down__list {
  flex: 1;
  overflow-y: auto;
  padding: 10px;
  background: #f5f5f5;
}

.bubble_chat_wrapper__rolled_down__list .scroll {
  display: flex;
  flex-direction: column;
}

.bubble_chat_wrapper__rolled_down__list div {
  margin-bottom: 10px;
  display: flex;
}

.bubble_chat_wrapper__rolled_down__list div.mine {
  justify-content: flex-end;
}

.bubble_chat_wrapper__rolled_down__list div img,
.bubble_chat_wrapper__rolled_down__list div .no_image {
  width: 30px;
  height: 30px;
  border-radius: 50%;
  margin-right: 10px;
}

.bubble_chat_wrapper__rolled_down__list div .no_image {
  background: #007bff;
  color: #fff;
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 14px;
}

.bubble_chat_wrapper__rolled_down__input {
  padding: 10px;
  background: #fff;
  border-top: 1px solid #ddd;
}

.bubble_chat_wrapper__rolled_down__input textarea {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 5px;
  resize: none;
}

.slide-enter-active,
.slide-leave-active {
  transition: all 0.3s ease;
}

.slide-enter,
.slide-leave-to {
  height: 0;
  opacity: 0;
  overflow: hidden;
}
</style>

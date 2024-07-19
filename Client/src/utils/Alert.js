// src/utils/Alert.js
import { notification } from 'ant-design-vue';
import _ from 'lodash';

export const AlertType = Object.freeze({
    INFO: "info",
    ERROR: "error",
    WARNING: "warning",
    SUCCESS: "success"
});

export const openNotification = (title, description, type = AlertType.INFO, placement = "topRight") => {
    let desc = description;
    const other_desc = _.get(description, "response.data.message", undefined) || _.get(description, "response.data", undefined);
    if (type === AlertType.ERROR && other_desc) {
        desc = other_desc;
    }
    return notification.open({
        message: title,
        description: desc,
        type,
        placement,
    });
};

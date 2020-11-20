let autoCompleteIntegration = {};
autoCompleteIntegration = function () {
    let initAutoComplete = function () {
        let cityAutoComplete = $("input[type=text]#City").get(0);
        let jobRoleAutoComplete = $("input[type=text]#jobtitle").get(0);

        if (cityAutoComplete) {
            autocomplete(cityAutoComplete, $("input[type=hidden]#hdnCity").get(0));
        }
        if (jobRoleAutoComplete) {
            autocomplete(jobRoleAutoComplete, $("input[type=hidden]#hiddenjobtitle").get(0));
        }
    };

    let refreshAutoComplete = function (_this, cb) {
        switch (_this.target.id.trim().toUpperCase()) {
            case "CITY":
                SendAJAXRequest(`/Home/GetCityListChar/?cityFirstChar=${_this.target.value}`, "Get", {}, "JSON", function (response) {
                    let data = new Array();
                    if (response && response.length > 0) {
                        response.forEach(function (item, i) {
                            data.push({ label: item.city, value: item.cityCode });
                        });
                    }
                    cb(data);
                });
                break;
            case "JOBTITLE":
                SendAJAXRequest('/Home/GetJobTitleList?jobFirstChar=' + _this.target.value, 'GET', {}, 'JSON', (response) => {
                    let data = new Array();
                    if (response && response.length > 0) {
                        response.forEach(function (item, i) {
                            data.push({ label: item.jobTitleName, value: item.jobTitleId });
                        });
                    }
                    cb(data);
                });
                break;
            default:
                break;
        }
    };

    return {
        initAutoComplete: initAutoComplete,
        refreshAutoComplete: refreshAutoComplete
    };
}();

$(document).ready(function () {
    autoCompleteIntegration.initAutoComplete();
});

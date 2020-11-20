function SearchCity(_this) {
    let filters = $("#hdnFilters").val();
    let jFilters = new Object();
    if (filters) {
        jFilters = JSON.parse(filters);
        if (_this.checked) {
            jFilters.City.push(_this.value);
        }
        else {
            let indexToRm = jFilters.City.indexOf(_this.value);
            if (indexToRm > -1) {
                jFilters.City.splice(indexToRm, 1);
            }
        }
    }
    getFilteredData(jFilters);
}

function SearchTitle(_this) {
    var filters = $("#hdnFilters").val();
    let jFilters = new Object();
    if (filters) {
        jFilters = JSON.parse(filters);
        if (_this.checked) {
            jFilters.JobCategory.push(_this.value);
        }
        else {
            let indexToRm = jFilters.JobCategory.indexOf(_this.value);
            if (indexToRm > -1) {
                jFilters.JobCategory.splice(indexToRm, 1);
            }
        }
    }
    getFilteredData(jFilters);
}

function getFilteredData(filters) {
    let queryString = '?';
    for (let key in filters) {
        let val = filters[key];
        if (Array.isArray(filters[key])) {
            val = filters[key].join(",");
        }
        val = val ? val : "";
        queryString += `${key}=${val}&`;
    }
    queryString = queryString.slice(0, -1);
    window.location.href = `/SearchResume/SearchResumeList/${queryString}`;
}

$(document).ready(function () {
    $("input[type=text].filter-search-box").keyup(function () {
        let query = this.value.toLowerCase();
        $(this).parent().find("ul li").each(function (i, ele) {
            let text = $(ele).text().toLowerCase();
            if (text === '' || text.indexOf(query) > -1) {
                $(ele).show();
            }
            else {
                $(ele).hide();
            }
        });
    });
});
$(document).ready(function () {

    SendAJAXRequest(`/Dashboard/GetAdminDashboard`, "GET", {}, "html", function (resp) {
        if (resp && resp !== "") {
            $("div#contentHolder").html(resp);
        }
        else {
            return false;
        }
    });
});

function ManageUsers() {
    SendAJAXRequest(`/Dashboard/GetAllUsers`, "GET", {}, "html", function (resp) {
        if (resp && resp !== "") {
            $("div#contentHolder").html(resp);
        }
        else {
            return false;
        }
    });
}

function GetSuccessStoryVideo() {
    SendAJAXRequest(`/SuccessStoryVideo/GetSuccessStoryVideo`, "GET", {}, "html", function (resp) {
        if (resp && resp !== "") {
            $("div#contentHolder").html(resp);
        }
        else {
            return false;
        }
    });
}

function GetJobRoles() {
    SendAJAXRequest(`/JobTitle/GetJobTitle`, "GET", {}, "html", function (resp) {
        if (resp && resp !== "") {
            $("div#contentHolder").html(resp);
        }
        else {
            return false;
        }
    });
}
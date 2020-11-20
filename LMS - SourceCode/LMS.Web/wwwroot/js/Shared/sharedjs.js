let sharable = {};


sharable = (function () {
    let logOutUser = function () {
        SendAJAXRequest(`/Auth/Logout`, "GET", {}, "json", function (resp) {
            window.location.reload(true);
        });
    };

    return {
        logOutUser: logOutUser
    };
})();
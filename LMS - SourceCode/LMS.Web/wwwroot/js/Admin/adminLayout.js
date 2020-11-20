let adminLayout = {};

adminLayout = (function () {
    let initialize = function () {
        getNotificationsCounter();
    };

    let getNotificationsCounter = function () {
        SendAJAXRequest('/Notifications/GetNotificationsCounter/', 'GET', {}, 'JSON', (data) => {
            if (data && data.counts) {
                if (data.counts.totalNotifications> 0) {
                    $("i#aggregate-notification-bell").text(data.counts.totalNotifications);
                    $("i#aggregate-notification-bell").show();
                }
                else {
                    $("i#aggregate-notification-bell").hide();
                }

                if (data.counts.newAddedUsersCount > 0) {
                    $("span#manage-users-badge").text(data.counts.newAddedUsersCount);
                    $("span#manage-users-badge").show();
                    //$("span#manage-users-badge").click(function (evt) {
                    //    evt.preventDefault();
                    //    evt.stopPropagation();
                    //    window.location.href = "/Dashboard/GetAllUsers/?uv=true";
                    //});
                }
                else {
                    $("span#manage-users-badge").hide();
                }
            }
        });
    };

    return {
        initialize: initialize
    };

})();

$(document).ready(function () {
    adminLayout.initialize();
});

$(window).scroll(function () {
    var scroll = $(window).scrollTop();
    console.log(scroll);
    if (scroll >= 60) {
        //console.log('a');
        $(".popupdata").addClass("scrollPopup");
    } else {
        //console.log('a');
        $(".popupdata").removeClass("scrollPopup");
    }
});
$(document).ready(function () {
    SendAJAXRequest('/Home/GetJobCategory/', 'GET', {}, 'JSON', (data) => {
        if (data) {
            var jobCategory = $('#jobCategory');
            $(jobCategory).empty();
            for (var i = 0; i < data.length; i++) {
                $(jobCategory).append('<li><a href="/Home/AllJobsByCategory/?id=' + data[i].jobIndustryAreaId + '" class="jslogin"><span>' + data[i].jobIndustryAreaName + '</span></a></li>');

            }
        } else {
            warnignPopup('Error');
        }
            
    });
    SendAJAXRequest('/Home/TalentConnectLink/', 'GET', {}, 'JSON', (data) => {
        if (data) {
            $("#TalentConnectLink").attr("href", data);
        } else {
            warnignPopup('Error');
        }

    });

    SendAJAXRequest('/Home/CandidateBulkUpload/', 'GET', {}, 'JSON', (data) => {
        if (data) {
            $("#CandidateBulkUpload").attr("href", data);
        } else {
            warnignPopup('Error');
        }

    });

    SendAJAXRequest('/Home/TPRegistrationGuide/', 'GET', {}, 'JSON', (data) => {
        if (data) {
            $("#TPRegistrationGuide").attr("href", data);
        } else {
            warnignPopup('Error');
        }

    });
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
let dashboard = {};

dashboard = (function () {

    let setFeaturedJobsPrecedence = function () {
        $('input[type=checkbox][name=featured]').change(function () {
            if ($('input[type=checkbox][name=featured]').is(":checked")) {
                $('input[type=number][name=displayorder]').removeAttr('disabled');
            } else {
                $('input[type=number][name=displayorder]').val(0);
                $('input[type=number][name=displayorder]').attr('disabled', true);
            }
        });
        $('input[type=checkbox][name=featured]').change();
    };

    let getJobs = function () {
        let year = $("select#jobListYearFilter").val();
        year = (year && year !== "") ? year : new Date().getFullYear();

        let employer = $("select#employerFilter").val();
        employer = (employer && employer !== "") ? employer : "";
        $('#loader').show();
        SendAJAXRequest(`/Dashboard/GetJobs?year=${year}&employer=${employer}`, "GET", {}, "html", function (resp) {
            if (resp && resp !== "") {
                $('#loader').hide();
                $("div#jobsContainer").html(resp);
                if ($("select#jobListYearFilter").val() !== year) {
                    $("select#jobListYearFilter").val(year);
                }
            }
            else {
                return false;
            }
        });
    };

    let populateJobOnForm = function (jobId) {
        $('#loader').show();
        SendAJAXRequest(`/Dashboard/GetJobScreenById?jobId=${jobId}`, "GET", {}, "html", function (resp) {
            if (resp && resp !== "") {
                $('#loader').hide();
                $("div#editJob").find("div.modal-body").html(resp);
                $("div#editJob").modal({
                    backdrop: "static"
                });
                setFeaturedJobsPrecedence();
                //multiselector.initSelector(
                //    $('select#jobRole'),
                //    {
                //        nonSelectedText: 'Select job role',
                //        isDisableSelector: false
                //    },
                //    $("input[type=hidden]#hdnJobRoleIds")
                //);
            }
            else {
                return false;
            }
        });
    };

    let updateJob = function (data) {
        let bodyContent = $("#cke_jobDetails iframe").contents().find("body").html();
        data.jobDetails = bodyContent;
         SendAJAXRequest(`/Dashboard/UpdateJobDetails`, "POST", data, "JSON", function (resp) {
            if (resp && resp.isUpdated) {
                $("div#editJob").modal("hide");
                $("div.modal-backdrop.fade").removeClass("in");
                $("div.modal-backdrop.fade").addClass("out");
                $("div.modal-backdrop.fade").css("pointerEvents", "none");
                $("body").css("overflow", "auto");
                getJobs();
            }
            else {
                return false;
            }
        });
    };

    let getCity = function (_this) {
        var StateId = _this;
        if (StateId !== "") {
            var ddlCity = $('#city');

            SendAJAXRequest(`/Dashboard/CityDetails/?stateCode=${StateId}`, 'GET', {}, 'JSON', (d) => {
                if (d) {
                    ddlCity.empty(); // Clear the plese wait  
                    var valueofcity = "";
                    var v = "<option value=" + valueofcity + ">Select City</option>";
                    $.each(d, function (i, v1) {
                        v += "<option value=" + v1.cityCode + ">" + v1.city + "</option>";
                    });
                    $("#city").html(v);
                } else {
                    warnignPopup('Error!');
                }
            });

        }
    };

    return {
        getJobs: getJobs,
        populateJobOnForm: populateJobOnForm,
        updateJob: updateJob,
        getCity: getCity
    };

})();


function yearChanged() {
    dashboard.getJobs();
}
function stateChange(_this) {
    dashboard.getCity(_this);
}

function employerChanged() {
    dashboard.getJobs();
}

function populateJobOnForm(jobId) {
    dashboard.populateJobOnForm(jobId);
}

function updateJob(_this) {
    let forms = $(_this).parent().parent().find("form");
    let formsData = ResolveFormData(forms);
    
    if (formsData[0].CompanyName === "") {
        warnignPopup('Please insert company name');
        return false;
    }
    else if (formsData[0].JobTitleByEmployer === "") {
        warnignPopup('Please insert job title');
        return false;
    }
    else if (formsData[0].HiringCriteria === "") {
        warnignPopup('Please insert hiring criteria');
        return false;
    }
    dashboard.updateJob(formsData[0]);
}

$(function () {
    dashboard.getJobs();
});

//$(function () {
//    dashboard.getCity();
//});
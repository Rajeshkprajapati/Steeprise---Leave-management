$(document).ready(function () {
    
        SendAJAXRequest(`/JobSeekerManagement/GetJobseekerDashboard`, "GET", {}, "html", function (resp) {
            if (resp && resp !== "") {
                $("div#contentHolder").html(resp);
            }
            else {
                return false;
            }
        });
});
function JobseekerAppliedJobs() {
    SendAJAXRequest(`/JobSeekerManagement/GetJobseekerAppliedJobs`, "GET", {}, "html", function (resp) {
        if (resp && resp !== "") {
            $("div#contentHolder").html(resp);
        }
        else {
            return false;
        }
    });
}

function JobseekerViewedProfile() {
    SendAJAXRequest(`/JobSeekerManagement/GetJobseekerViewedProfile`, "GET", {}, "html", function (resp) {
        if (resp && resp !== "") {
            $("div#contentHolder").html(resp);
        }
        else {
            return false;
        }
    });
}
function DeleteAppliedJob(id) {
    var data = "";
    SendAJAXRequest("/JobSeekerManagement/DeleteAppliedJob/?JobPostId=" + id + "", 'POST', data, 'JSON', function (result) {
        if (result) {
            let icon = 'fa fa-thumbs-up';
            updatedsucessfully(result, icon);
        } else {
            warnignPopup('Error');
        }
    });
}

function EmployerFollowers() {
    SendAJAXRequest(`/JobSeekerManagement/EmployerFollowers`, "GET", {}, "html", function (resp) {
        if (resp && resp !== "") {
            $("div#contentHolder").html(resp);
        }
        else {
            return false;
        }
    });
}

function UnfollowComapny(EmployerId) {
    var data = "";
    SendAJAXRequest("/JobSeekerManagement/UnfollowCompany/?EmployerId=" + EmployerId + "", 'POST', data, 'JSON', function (result) {
        if (result) {
            //let icon = 'fa fa-thumbs-up';
            //updatedsucessfully(result, icon);
            //alert("Done");
            InformationDialog('Unfollowed', 'You are no longer follower of the company');
            //location.reload();
        } else {
            warnignPopup('Error');
        }
    });
}

function ManageResume() {
    SendAJAXRequest(`/JobSeekerManagement/ManageResume`, "GET", {}, "html", function (resp) {
        if (resp && resp !== "") {
            $("div#contentHolder").html(resp);
        }
        else {
            return false;
        }
    });
}

function JobseekerContactedDetails() {
    SendAJAXRequest(`/JobSeekerManagement/ContactedDetails`, "GET", {}, "html", function (resp) {
        if (resp && resp !== "") {
            $("div#contentHolder").html(resp);
        }
        else {
            return false;
        }
    });
}
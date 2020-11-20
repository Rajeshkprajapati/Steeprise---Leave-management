function ApplyJobs(id) {
    //var data = { jobPostId: id };
    var currentUrl = window.location.href;

    $('#confimationModel').modal('hide');
    $('#loader').show();
    SendAJAXRequest('/Job/ApplyJob/?jobPostid=' + id + '&currentUrl=' + currentUrl, 'get', {}, 'json', (result) => {
        $('#loader').hide();
        if (result && result.returnUrl) {
            window.location.href = result.returnUrl;
        }
        else if (result === 'Job applied' || result === 'Congratulations! Job applied successfully.') {
            $("#iconPopup").addClass('fa fa-thumbs-up');
            $('#tagiging').html("Congratulation!");
            $('#tagginMessage').html(result);
            $('#myModal').modal({
                dismissible: true
            });
            $('#btnSuccess').attr('onclick', 'ReloadPage()');
            //$('#btnSuccess').removeAttr('data-dismiss','modal');
            $('#myModal').modal('show');
            $("#myModal").removeClass("open");
            $("#myModal").addClass("in");
        }
        else if (result === 'You have already applied this job') {
            $("#iconPopup").addClass('fa fa-exclamation-triangle');

            $('#tagiging').html("Alert!");
            $('#tagginMessage').html(result);
            //$('#my-modal').modal({
            //    show: true,
            //    closeOnEscape: true
            //});
            $('#myModal').modal({
                dismissible: true
            });
            $('#myModal').modal('show');
            $("#myModal").removeClass("open");
            $("#myModal").addClass("in");
        }

        else if (result === 'Oops! Applicable For Job Seeker Only.') {
            $("#iconPopup").addClass('fa fa-exclamation');

            $('#tagiging').html("Alert");
            $('#tagginMessage').html(result);
            //$('#my-modal').modal({
            //    show: true,
            //    closeOnEscape: true
            //});
            $('#myModal').modal({
                dismissible: true
            });
            $('#myModal').modal('show');
            $("#myModal").removeClass("open");
            $("#myModal").addClass("in");
        }
        else if (result === 'To apply job please complete your profile') {
            $("#btnSuccess").hide();
            $("#btnWarning").show();
            $("#iconPopup").addClass('fa fa-exclamation');
            $('#tagiging').html("Alert");
            $('#tagginMessage').html(result);
            //$('#my-modal').modal({
            //    show: true,
            //    closeOnEscape: true
            //});
            $('#myModal').modal({
                dismissible: true
            });
            $('#myModal').modal('show');
            $("#myModal").removeClass("open");
            $("#myModal").addClass("in");
        }
        else if (result === 'Please login to apply this job') {
            window.location.href = '/Auth/';
        }
        else {
            $("#iconPopup").addClass('fa fa-exclamation');
            //$('#iconPopup').addClass("fa fa-exclamation");
            $('#btnSuccess').attr('onclick', 'ReloadPage()');
            $('#tagiging').html("Warning");
            $('#tagginMessage').html('Failed to Apply Job');
            $('#myModal').modal({
                dismissible: false
            });
            $('#myModal').modal('show');
            $("#myModal").removeClass("open");
            $("#myModal").addClass("in");

        }
    });
}
function showModal() {
    $('#myModal').modal('show');
}
function RedirectProfile() {
    window.location.href = '/JobSeekerManagement/Profile/';
}
function ConfrimationFoJobApply(id) {

    $('#confimationModel').modal({
        dismissible: true
    });
    $('#applyJobsbutton').attr('onclick', 'ApplyJobs(' + id + ')');
    $('#confimationModel').modal('show');
    $("#confimationModel").removeClass("open");
    $("#confimationModel").addClass("in");
}
function redirectLogin() {
    window.location.href = '/Auth/';
}
function ReloadPage() {
    location.reload();
}
function WarningPopup() {
    ErrorDialog("Login Required", "Please login or regiseter to apply job");
    return false;
}
$(document).ready(function () {
    pageSize = 6;
    incremSlide = 6;
    startPage = 0;
    numberPage = 0;
});
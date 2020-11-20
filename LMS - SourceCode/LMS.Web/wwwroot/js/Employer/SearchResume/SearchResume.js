function SendMail(JobseekerId,JobSeekerName) {
    var data = { userEmail: JobseekerId, JobSeekerName: JobSeekerName };
    $('#loader').show();
    SendAJAXRequest(`/SearchResume/SendMessage/?userEmail=${JobseekerId}&&JobSeekerName=${JobSeekerName}`, 'GET', {}, 'JSON', (result) => {
        if (result) {
            $('#loader').hide();
            document.getElementById('result').innerHTML = result.errorMessage;
            $("#result").show();
        } else {
            $('#loader').hide();
            $("#iconPopup").addClass('fa fa-exclamation-triangle');
            $('#tagiging').html("Alert!");
            $('#tagginMessage').html('Error in Sending Email!');
            $('#myModal').modal({
                dismissible: true
            });
            $('#myModal').modal('show');
            $("#myModal").removeClass("open");
            $("#myModal").addClass("in");
            //location.reload(true);
        }
    }, null, null);
}

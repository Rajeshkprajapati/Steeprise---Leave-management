let dashboard = (function () {
    let init = function () {
        $('#tableJobSeekersList').dataTable({
            aoColumnDefs: [
                {
                    bSortable: false,
                    aTargets: [0, 1, 2, 3, 4, 5, 6]
                },
                {
                    bSearchable: false,
                    aTargets: [-1]
                }
            ]
        });
    };

    let getCandidateDetailForm = function (uId) {
        SendAJAXRequest(`/Dashboard/CandidateDetail/?userid=${uId}`, 'GET', {}, 'html', (result) => {
            if (result && result !== "") {
                $("div#editCandidate").find("div.modal-body").html(result);
                $("div#editCandidate").modal({
                    backdrop: "static"
                });
            } else {
                return false;
            }
        });
    };

    let updateCandidate = function (data) {
        SendAJAXRequest(`/Dashboard/UpdateCandidate`, "POST", data, "JSON", function (resp) {
            if (resp && resp.isUpdated) {
                closeModalManually($("div#editCandidate"));
                let icon = "fa fa-thumbs-up";
                let msg = "Candidate updated successfully";
                //sucessfullyPopupWR(msg, icon);
                mysuccesspopup(msg, icon);
            }
            else {
                return false;
            }
        });
    };

    let deleteCandidate = function (uId) {
        SendAJAXRequest(`/Dashboard/DeleteCandidate/?userid=${uId}`, 'GET', {}, 'JSON', (result) => {
            if (result && result.isSuccess) {
                let icon = 'fa fa-thumbs-up';
                let msg = 'Candidate Deleted';
                //sucessfullyPopupWR(msg, icon);
                mysuccesspopup(msg, icon);
                //location.reload(true);
            }
        });
    };

    return {
        init: init,
        getCandidateDetailForm: getCandidateDetailForm,
        deleteCandidate: deleteCandidate,
        updateCandidate: updateCandidate
    };
})();

function getCandidateDetails(uId) {
    dashboard.getCandidateDetailForm(uId);
}

function updateCandidate(_this) {
    let forms = $(_this).parent().parent().find("form");
    let formsData = ResolveFormData(forms);
    if (formsData[0].FirstName === "") {
        warnignPopup('Please insert first name.');
        return false;
    }
    else if (formsData[0].LastName === "") {
        warnignPopup('Please insert last name.');
        return false;
    }
   dashboard.updateCandidate(formsData[0]);
}

function deletedata(uId) {
    dashboard.deleteCandidate(uId);
}

const mysuccesspopup = (Message, icon) => {
    let options = {
        backdrop: "static",
        show: true
    };
    $("#wriconPopup").addClass(icon);
    $('#wrtagiging').html("Congratulation!!");
    $('#wrtagginMessage').html(Message);
    $('#WRSuccessPopup').modal(options);
};

$(document).ready(function () {
    dashboard.init();
});


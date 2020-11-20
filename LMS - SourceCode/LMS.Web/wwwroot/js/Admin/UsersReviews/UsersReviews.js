$(document).ready(function () {

    $('#dataTable').dataTable({
        // 'columnDefs': [{ "searchable": false, 'orderable': false, 'targets': 5 }]
        aoColumnDefs: [
            {
                bSortable: false,
                aTargets: [-1]
            },
            {
                bSearchable: false,
                aTargets: [-1]
            }
        ]
    });

});
function deletedata(ReviewId) {
    SendAJAXRequest(`/UsersReviews/DeleteUsersReview/?Id=${ReviewId}`, 'GET', {}, 'JSON', (result) => {
        if (result) {
            let icon = 'fa fa-thumbs-up';
            updatedsucessfully(result, icon)
            //location.reload(true);
        } else {
            warnignPopup('Error')
        }
    });
}

   
    
function Approve(ReviewId) {
    $('#userId').val(ReviewId);
    if (ConfrimationMessage()) {
        $('#loader').show();
    } else {
        return false;
    }
       //SendAJAXRequest(`/UsersReviews/ApproveUser/?Id=${ReviewId}`, 'GET', {}, 'JSON', (result) => {
       //     if (result) {
       //         let icon = 'fa fa-thumbs-up';
       //         updatedsucessfully(result, icon)
       //         //location.reload(true);
       //     } else {
       //         warnignPopup('Error')
       //     }
       // });
    
}

$('#btnYes').click(function () {
    var id = $('#userId').val();
   ConfrimationMessageClose();
   
    SendAJAXRequest(`/UsersReviews/ApproveUser/?Id=${id}`, 'GET', {}, 'JSON', (result) => {
            if (result) {
                let icon = 'fa fa-thumbs-up';
                updatedsucessfully(result, icon)
                //location.reload(true);
            } else {
                warnignPopup('Error')
            }
        });
});

function edit(_this) {
    var row = $(_this).closest('tr').find('td');
    $('#error_sp_msg').hide();
    $('.modal-body #ReviewId').val(row[0].innerText);
    $('.modal-body #ReviewName').val(row[1].innerText);
    $('.modal-body #ReviewEmail').val(row[2].innerText);
    //$('.modal-body #ReviewTagLine').val(row[4].innerText);
    $('.modal-body #ReviewMessage').val(row[3].innerText);
}

function Updatedata(_this) {
    event.preventDefault();
    //var formdata = ResolveFormData(_this);
    let id = $('.modal-body #ReviewId').val();
    let name = $('.modal-body #ReviewName').val();
    let email = $('.modal-body #ReviewEmail').val();
    let tagline = $('.modal-body #ReviewTagLine').val();
    let message = $('.modal-body #ReviewMessage').val();

    var data = { Id: id, Name: name, Email: email, Tagline: tagline, Message: message };

    SendAJAXRequest('/UsersReviews/UpdateUserReview/', 'POST', data, 'JSON', (result) => {
        if (result === true) {
            let icon = 'fa fa-thumbs-up';
            updatedsucessfully('done', icon)
            //location.reload(true);
        } else {
            warnignPopup('Error')
        }
    });
}

$("#JobTitelName").keypress(function (e) {
    $("#error_sp_msg").remove();
    var k = e.keyCode,
        $return = ((k > 64 && k < 91) || (k > 96 && k < 123) || k === 8 || k === 32 || (k >= 48 && k <= 57));
    if (!$return) {
        $("<span/>", {
            "id": "error_sp_msg",
            "html": "Special characters not allowed !!!!!"
        }).insertAfter($(this));
        return false;
    }

});



$('#PopUpModal').on('hidden.bs.modal', function () {
    $("#error_sp_msg").remove();

});

function ConfrimationMessage() {

    $('#confimationModel').modal({
        dismissible: true
    });
    $('#confimationModel').modal('show');
    $("#confimationModel").addClass("open");
    $("#confimationModel").addClass("in");
}

function ConfrimationMessageClose() {

    $("div#confimationModel").modal("hide");
    $("div.modal-backdrop.fade").removeClass("in");
    $("div.modal-backdrop.fade").addClass("out");

    //$("#confimationModel").removeClass('modal fade');
    //$('#confimationModel').remove('close');
    //$("#confimationModel").removeClass("open");
    //$("#confimationModel").removeClass("in");
}

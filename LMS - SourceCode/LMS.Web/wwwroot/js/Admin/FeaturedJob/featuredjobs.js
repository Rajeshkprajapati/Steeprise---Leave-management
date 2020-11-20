$(document).ready(function () {

    $('#dataTable').dataTable({
        // 'columnDefs': [{ "searchable": false, 'orderable': false, 'targets': 5 }]
        "ordering":false,
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
function edit(_this) {
    $('#PopUpModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var titlename = button.data('name');// Extract info from data-* attributes

        var modal = $(this);
        modal.find('.modal-title').text(titlename + ' Record');
        var row = $(_this).closest('tr').find('td');
        if (titlename === 'Edit') {
            modal.find('.modal-body #JobPostId').val(row[0].innerText);
            modal.find('.modal-body #jobTitle').val(row[1].innerText);
            modal.find('.modal-body #companyname').val(row[2].innerText);
            modal.find('.modal-body #displayOrder').val(row[3].innerText);
            $('#error_sp_msg').hide();
            $("#SaveRec").hide();
            $("#Update").show();
        }
        //else {
        //    modal.find('.modal-body #DesignationId').val('');
        //    modal.find('.modal-body #Designation').val('');
        //    modal.find('.modal-body #Abbrivation').val('');
        //    $("#SaveRec").show();
        //    $("#Update").hide();
        //}
    });

}
function Updatedata() {
    //JobTitle: $('#jobTitle').val(), Companyname: $('#companyname').val(),
    //let data = { JobPostId: $('#JobPostId').val(),  FeaturedJobDisplayOrder: $('#displayOrder').val() };    //let data = { JobPostId: $('#JobPostId').val(),  FeaturedJobDisplayOrder: $('#displayOrder').val() };
    let JobPostId = $('#JobPostId').val();
    let FeaturedJobDisplayOrder = $('#displayOrder').val();

    SendAJAXRequest(`/ManageJobs/UpdateFeaturedJobDisplayOrder/?JobPostId=${JobPostId}&FeaturedJobDisplayOrder=${FeaturedJobDisplayOrder}`, 'GET', {}, 'json', (result) => {
        if (result) {
            let icon = 'fa fa-thumbs-up';
            updatedsucessfully(result.msg, icon);
            //location.reload(true);
        }
    });
}
function deletedata(jobpostid) {
    SendAJAXRequest(`/ManageJobs/DeleteFeaturedJob/?jobpostid=${jobpostid}`, 'GET', {}, 'JSON', (result) => {
        if (result) {
            let icon = 'fa fa-thumbs-up';
            updatedsucessfully(result.msg, icon);
            //location.reload(true);
        } else {
            warnignPopup("Unable to delete");

        }
    });
}


//function AddNew() {
//    //Hide the update button
//    $('#PopUpModal').on('show.bs.modal', function () {
//        var modal = $(this);
//        modal.find('.modal-body #DesignationId').val('');
//        modal.find('.modal-body #Designation').val('');
//        modal.find('.modal-body #Abbrivation').val('');
//        $('#error_sp_msg').hide();
//        $("#Update").hide();
//        $("#SaveRec").show();
//    });
//}

//function Adddata() {
//    var data = { designation: $('#Designation').val(), Abbrivation: $('#Abbrivation').val() };

//    SendAJAXRequest('/Designation/AddDesigantion/', 'POST', data, 'JSON', (result) => {
//        if (result) {
//            let icon = 'fa fa-thumbs-up';
//            updatedsucessfully(result.msg, icon)
//            location.reload(true);
//        } else {
//            warnignPopup("Unable to Add Designation")

//        }
//    });
//}

$("#Designation,#Abbrivation").each(function () {
    $(this).keypress(function (e) {
        $("#error_sp_msg").remove();
        var k = e.keyCode,
            $return = ((k > 64 && k < 91) || (k > 96 && k < 123) || k === 8 || k === 32);
        if (!$return) {
            $("<span/>", {
                "id": "error_sp_msg",
                "html": "Special characters/numbers not allowed !!!!!",
                "style": "color:red"
            }).insertAfter($(this));
            return false;
        }
    });
});



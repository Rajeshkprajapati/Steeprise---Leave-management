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
        ],
        "bLengthChange": false
    });

});
function edit(_this) {
    //console.log(userid)
    $('#PopUpModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var titlename = button.data('name');// Extract info from data-* attributes

        var modal = $(this);
        modal.find('.modal-title').text('Edit Record of ' + titlename);
        var row = $(_this).closest('tr').find('td');

        //for (var i = 1; i < row.length-1; i++) {
        //    console.log(row[i].innerText);
        modal.find('#JobTitleId').val(row[0].innerText);
        modal.find('#JobTitelName').val(row[1].innerText);
        $("#SaveRec").hide();
        $("#Update").show();
        $("#updatebtn").removeClass('display-content');
    });

};
function Updatedata(_this) {
    //alert($('#UserId').val());
    let jtitleId = $('#JobTitleId').val().trim();
    let JobTitleName = $('#JobTitelName').val().trim();
    if (JobTitleName === "") {
        warnignPopup('Please fill job title name');
        return false;
    }
    var data = { JobTitleId: jtitleId === "" ? 0 : jtitleId, JobTitleName: $('#JobTitelName').val() };
    SendAJAXRequest('/JobTitle/InsertUpdateJobTitle/', 'POST', data, 'JSON', (result) => {
        if (result) {
            //alert(result);
            //location.reload(true);
            let icon = 'fa fa-thumbs-up';
            updatedsucessfully(result, icon) 
            //location.reload(true);
        } else {
           warnignPopup('Error')
        }
    });
}
function deletedata(JobTitleId) {
        SendAJAXRequest(`/JobTitle/DeleteJobTitle/?jobTitleId=${JobTitleId}`, 'GET', {},'JSON', (result) => {
            if (result) {
                let icon = 'fa fa-thumbs-up';
                updatedsucessfully(result, icon) 
                //location.reload(true);
            } else {
                warnignPopup('Error')
            }
        });
}
    

    
 
function AddNew() {
    //console.log(userid)
    $('#PopUpModal').on('show.bs.modal', function () {
        $("#Update").hide();
        $("#SaveRec").show();
        $("#updatebtn").addClass('display-content');
        $("#JobTitelName").val('');
        $("#JobTitleId").val('');
        $("#PopUpModalLabel").text("Add Job Title");
 });

};
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



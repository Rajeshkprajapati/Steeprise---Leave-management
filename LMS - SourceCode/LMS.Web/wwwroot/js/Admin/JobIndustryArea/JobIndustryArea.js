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
function edit(_this) {
    //console.log(userid)
    $('#PopUpModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var titlename = button.data('name');// Extract info from data-* attributes

        var modal = $(this);
        modal.find('.modal-title').text('Edit Record');
        var row = $(_this).closest('tr').find('td');

        //for (var i = 1; i < row.length-1; i++) {
        //    console.log(row[i].innerText);
        modal.find('.modal-body #JobIndustryAreaId').val(row[1].innerText);
        modal.find('.modal-body #JobIndustryAreaName').val(row[2].innerText);
        $("#SaveRec").hide();
        $("#Update").show();
       });

};
function Updatedata(_this) {
    //alert($('#UserId').val());
    let jAreaId = $('#JobIndustryAreaId').val().trim();
    var data = { JobIndustryAreaId: jAreaId === "" ? 0 : jAreaId, JobIndustryAreaName: $('#JobIndustryAreaName').val() };
    SendAJAXRequest('/JobIndustryArea/UpdateJobIndustryArea/', 'POST', data, 'JSON', (result) => {
        if (result) {
            let icon = 'fa fa-thumbs-up';
            updatedsucessfully(result, icon)
            //location.reload(true);
            
        } else {
            warnignPopup('Error')
        }
    });

}
function deletedata(JobIndustryAreaId) {
    
        SendAJAXRequest(`/JobIndustryArea/DeleteJobIndustryArea/?jobIndustryAreaId=${JobIndustryAreaId}`, 'GET', {}, 'JSON', (result) => {
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
        $("#JobIndustryAreaName").val('');
        $("#JobIndustryAreaId").val('');
        $("#PopUpModalLabel").text("Add Job Industry Area")
     });

};
$("#JobIndustryAreaName").keypress(function (e) {
    $("#error_sp_msg").remove();
    var k = e.keyCode,
        $return = ((k > 64 && k < 91) || (k > 96 && k < 123) || k === 8 || k === 32 || (k >= 48 && k <= 57));
    if (!$return) {
        $("<span/>", {
            "id": "error_sp_msg",
            "html": "* Special characters not allowed !!!!!"
        }).insertAfter($(this));
        return false;
    }

})




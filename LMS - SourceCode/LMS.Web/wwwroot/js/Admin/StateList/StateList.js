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
        $('div#StateRow').show();
        $('#StateCode').attr('readonly', true);
        $('#Country').css('pointerEvents', 'none');
        $('#Country').attr('readonly', true);
        var modal = $(this);
        modal.find('.modal-title').text('Edit Record of ' + titlename);
        var row = $(_this).closest('tr').find('td');

        //for (var i = 1; i < row.length-1; i++) {
        //    console.log(row[i].innerText);
        modal.find('.modal-body #Country').val(row[0].innerText);
        modal.find('.modal-body #StateCode').val(row[2].innerText);
        modal.find('.modal-body #StateName').val(row[1].innerText);
        $("#SaveRec").hide();
        $("#Update").show();
    });

}
function Updatedata(_this) {
    let CountryCode = $('#Country').val().trim();
    var data = { CountryCode: CountryCode, StateCode: $('#StateCode').val().trim(), State: $('#StateName').val().trim() };
        SendAJAXRequest('/ManageCityState/UpdateState/', 'POST', data, 'JSON', (result) => {
            if (result) {
                //alert(result);
                //location.reload(true);
                let icon = 'fa fa-thumbs-up';
                updatedsucessfully(result, icon);
                //location.reload(true);
            } else {
                warnignPopup('Error');
            }
        });
    }
   

function AddData(_this) {
    //alert($('#UserId').val());
    let CountryCode = $('#Country').val().trim();
    let StateCode = $('#StateCode').val().trim();
    let StateName = $('#StateName').val().trim();
    if (StateCode === "") {
        warnignPopup('Please fill state code');
        return false;
    }
    if (StateName === "") {
        warnignPopup('Please fill state name');
        return false;
    }
    if (CountryCode !== "0") {
        var data = { CountryCode: CountryCode, StateCode: $('#StateCode').val().trim(), State: $('#StateName').val().trim() };
        SendAJAXRequest('/ManageCityState/InsertState/', 'POST', data, 'JSON', (result) => {
            if (result) {
                //alert(result);
                //location.reload(true);
                if (result === 'State code already exist') {

                    warnignPopup(result);
                }
                else {
                    let icon = 'fa fa-thumbs-up';
                    updatedsucessfully(result, icon);
                }

                //location.reload(true);
            } else {
                warnignPopup('Error');
            }
        });
    }
    else {
        warnignPopup('Please select country');
    }
}

function deletedata(CountryCode, stateCode) {
    var data = { CountryCode: CountryCode, StateCode: stateCode };
    SendAJAXRequest('/ManageCityState/DeleteState/', 'POST', data, 'JSON', (result) => {
        if (result) {
            let icon = 'fa fa-thumbs-up';
            updatedsucessfully(result, icon);
            //location.reload(true);
        } else {
            warnignPopup('Error');
        }
    });
}




function AddNew() {
    //console.log(userid)
    $('#PopUpModal').on('show.bs.modal', function () {
        $("#Update").hide();
        $("#SaveRec").show();
        //$('div#StateRow').hide();
        $('#Country').attr('readonly', false);
        $('#Country').css('pointerEvents','auto');
        
        $('#StateCode').attr('readonly', false);
       $("#StateCode").val('');
        $("#StateName").val('');
        $("#PopUpModalLabel").text("Add state");
    });

}
$("#StateName","#StateCode").keypress(function (e) {
    $("#error_sp_msg").remove();
    var k = e.keyCode,
        $return = (k > 64 && k < 91 || k > 96 && k < 123 || k === 8 || k === 32 || k >= 48 && k <= 57);
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

function ConfrimationDeleteState(countryId, stateId) {
    $('#confimationDeleteModel').modal({
        dismissible: true
    });
    $('#btndelete').attr('onclick', 'deletedata("' + countryId + '","' + stateId + '")');
    $('#confimationDeleteModel').modal('show');
    $("#confimationDeleteModel").addClass("open");
    $("#confimationDeleteModel").addClass("in");
}
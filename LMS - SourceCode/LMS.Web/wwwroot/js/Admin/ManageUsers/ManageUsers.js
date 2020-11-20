$(document).ready(function () {
    $('#Role').css('pointerEvents', 'none');
    $('#dataTable').dataTable({
        "order": [],
        'columnDefs': [
            { "searchable": true, 'orderable': true, 'targets': 6 }
        ]
  });

    $('#dataTable_paginate').addClass('data-table-pasiganation');
    $('#dataTable_length').addClass('data-table-lenthFilter');
    $('#dataTable_filter').addClass('data-table-SearchFilter');

    $('#btnYes').click(function () {
        var id= $('#userId').val();
        var emailId = $('#EmailId').val();
        var firstName = $('#FName').val();
        var data = { Userid: id, Email: emailId, FirstName: firstName };
        ConfrimationMessageClose();
        $('#loader').show();  
        SendAJAXRequest('/Dashboard/ApproveUser/', 'POST', data, 'JSON', (result) => {
            if (result && result.status) {               
                $('#loader').hide();
               let icon = 'fa fa-thumbs-up';              
                updatedsucessfully(result.msg, icon);
            } else {
                $('#loader').hide();
                warnignPopup(result.msg);
             }
        });
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
        $('#error_sp_msg').hide();
        //for (var i = 1; i < row.length-1; i++) {
        //    console.log(row[i].innerText);
        modal.find('#UserId').val(row[0].innerText);
        modal.find('#FName').val(row[1].innerText);
        modal.find('#LName').val(row[2].innerText);
        modal.find('#Email').val(row[3].innerText);
        //modal.find('.modal-body #psd').val(row[4].innerText);
        //modal.find('.modal-body #Role').val(row[5].innerText);
        $('#Role').val(row[5].innerText);
        //var objSelect = document.getElementById("Role");
        //objSelect.options[0].text = row[5].innerText;
        //objSelect.options[0].text = true;

        //}

    });

};


function deletedata(userid) {
        SendAJAXRequest(`/Dashboard/DeleteUsersById/?userid=${userid}`, 'GET', {}, 'JSON', (result) => {
       
            if (result) {
                let icon = 'fa fa-thumbs-up';
                updatedsucessfully(result, icon);
            } else {
                warnignPopup('Error');
            }
        });
}
   
   

function Updatedata() {
    //alert($('#UserId').val());
    var data = { Userid: $('#UserId').val(), FirstName: $('#FName').val(), LastName: $('#LName').val(), Email: $('#Email').val(), RoleName: $("#Role option:selected").val(), Password: $("#psd").val() };
    SendAJAXRequest('/Dashboard/Updatedata/', 'POST', data, 'JSON', (result) => {
        if (result) {
            let icon = 'fa fa-thumbs-up';
            updatedsucessfully(result, icon);
            //location.reload(true);
        } else {
            warnignPopup('Error');
        }
    });
    
}

function approveuser(id, _this) { 
    var row = $(_this).closest('tr').find('td');
    //var data = { Userid: id, Email: row[3].innerText };
    $('#userId').val(id);
    $('#EmailId').val(row[3].innerText);
    //$('#FName').val(row[1].innerText);
    $('#FName').val($(row[1]).find("label").text());
    if (ConfrimationMessage()) {
        $('#loader').show();       
    } else {
        return false;
    }
}
$("#FName,#LName").each(function () {
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
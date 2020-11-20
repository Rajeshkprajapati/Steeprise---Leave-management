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
        "bLengthChange": false,
        
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
        modal.find('#Id').val(row[1].innerText);
        modal.find('#TitelName').val(row[2].innerText);
        modal.find('#StoryType').val(row[3].innerText);
        modal.find('#videoLink').val(row[4].innerText);
        modal.find('#displayOrder').val(row[5].innerText);
        //var dropdown = row[3].innerText
        //alert(dropdown);
        $("#SaveRec").hide();
        $("#Update").show();
        $("#updatebtn").removeClass('display-content');
    });

};
function Updatedata(_this) {
    //formData = new FormData();
    //formData.append('Id', $('#Id').val());
    //formData.append('Title', $('#TitelName').val());
    //formData.append('Type', $('#StoryType').val());
    //formData.append('VideoFile', $('#videoLink').val());
    let TitelName = $('#TitelName').val().trim();
    let StoryType = $('#StoryType').val().trim();
    let videoLink = $('#videoLink').val().trim();
    let displayOrder = $('#displayOrder').val().trim();
    if (TitelName === "") {
        warnignPopup('Please fill titel name');
        return false;
    }
    if (StoryType === "0") {
        warnignPopup('Please select story type');
        return false;
    }
    if (videoLink === "") {
        warnignPopup('Please insert video link');
        return false;
    }
    if (displayOrder === "") {
        warnignPopup('Please fill display order');
        return false;
    }
    let Id = $('#Id').val().trim();
    var data = { Id: Id === "" ? 0 : Id, Title: TitelName, Type: StoryType, VideoFile: videoLink, DisplayOrder: displayOrder };
    SendAJAXRequest('/SuccessStoryVideo/InsertUpdateSuccessStoryVideo/', 'POST', data, "JSON", (result) => {
        if (result) {
            let icon = 'fa fa-thumbs-up';
            let Message = "Successfully Done";
            updatedsucessfully(Message, icon);
            //$('#btnSuccess').removeAttr('onclick');
            //location.reload(true);
        } else {
            warnignPopup('Faild to success');
            return false;
        }
    });

}

function deletedata(id) {
    SendAJAXRequest(`/SuccessStoryVideo/DeleteSuccessStoryVideo/?id=${id}`, 'GET', {}, "JSON", (result) => {
        if (result) {
            let icon = 'fa fa-thumbs-up';
            updatedsucessfully(result, icon)
            //location.reload(true);
        } else {
            warnignPopup('Unable to delete')
            return false;
        }
    });
}
    

    
function AddNew() {
    //console.log(userid)
    $('#PopUpModal').on('show.bs.modal', function () {
        $("#Update").hide();
        $("#updatebtn").addClass('display-content');
        $("#SaveRec").show();
        $("#TitelName").val('');
        //$("#StoryType").val('0');
        $("#videoLink").val('');
        $("#displayOrder").val('');
        $("#Id").val('');
        $("#PopUpModalLabel").text("Add Success Story")
        
    });

};
$("#TitelName").keypress(function (e) {
    $("#error_sp_msg").remove();
    var k = e.keyCode,
        $return = ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
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
//$('#btnSuccess').click(function () {
//    GetSuccessStoryVideo();
//     $("#alertpopup").modal("hide");
//    $("div.modal-backdrop.fade").removeClass("in");
//    $("div.modal-backdrop.fade").addClass("out");
//});


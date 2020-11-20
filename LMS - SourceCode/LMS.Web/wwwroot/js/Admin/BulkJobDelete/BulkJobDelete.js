$(document).ready(function () {

    $('#dataTable').dataTable({
        order: [],
        dom: 'Bfrtip',
        buttons: [
            'excel'
        ],
        //searching: false
    });
});

function SearchBulkJob() {

   let FY = $("#ddlFinancialYear option:selected").val();
    let statecode = $("#StateId option:selected").val();
    //if (statecode === "") {
    //    warnignPopup('Please select state name');
    //    return false;
    //}
    let CompanyId = $("#CompanyId option:selected").val();
    if (CompanyId === "") {
        warnignPopup('Please select company name');
        return false;
    }
    let CityId = $("#CityId option:selected").val();
    //var data = { FY: FY, statecode: statecode, CompanyId: CompanyId};
    $('#loader').show(); 
    SendAJAXRequest(`/Dashboard/SearchBulkJobList?FY=${FY}&statecode=${statecode}&CompanyId=${CompanyId}&citycode=${CityId}`, "GET", {}, "html", function (resp) {
        if (resp && resp !== "") {
            $('#loader').hide();
            $("div#searchlistbulk").html(resp);
        }
        else {
            return false;
        }
    });
}


function deletedata(id) {
    //let data = { JobPostId: id };
    let FY = $("#ddlFinancialYear option:selected").val();
    let statecode = $("#StateId option:selected").val();
    let CompanyId = $("#CompanyId option:selected").val();
    let CityId = $("#CityId option:selected").val();
    let JobPostId = $("#tableDelete").val();
    if (JobPostId === "") {
        warnignPopup('Please select records to proceed');
        return false;
    }
    SendAJAXRequest(`/Dashboard/DeleteBulkJobs?JobPostId=${JobPostId}&statecode=${statecode}&CompanyId=${CompanyId}&CityId=${CityId}&FY=${FY}`, 'POST', {}, 'html', function (resp){
        if (resp && resp !== "") {
            $('#loader').hide();
            sucessfullyPopupWR('Done','fa fa-thumbs-up');
            $("div#searchlistbulk").html(resp);
        }
        else {
            return false;
        }
    });
}

$("#StateId").change(function () {
    var StateId = $(this).val();
    if (StateId !== "") {
        var ddlCity = $('#CityId');

        SendAJAXRequest(`/Dashboard/CityListId/?StateCode=${StateId}`, 'GET', {}, 'JSON', (d) => {
            if (d) {
                ddlCity.empty(); // Clear the plese wait  
                var valueofcity = "";
                var v = "<option value=" + valueofcity + ">Select City</option>";
                $.each(d, function (i, v1) {
                    v += "<option value=" + v1.cityCode + ">" + v1.city + "</option>";
                });
                $("#CityId").html(v);
            } else {
                return error;
            }
        });

    }
});

function SelectJob(_this, e)
{

    var allSelectedValues = "";
    var isFirst = true;

    $(".checkBoxClass:checked").each(function () {

        if (isFirst === true) {
            isFirst = false;

            allSelectedValues = $(this).val();
        } else {
            allSelectedValues = allSelectedValues + "," + $(this).val();
        }

    });

    $('#tableDelete').val(allSelectedValues);
};

function SelectAll() {
    var allSelectedValues = "";
    var isFirst = true;

    $(".checkBoxClass").each(function () {

        if (isFirst === true) {
            isFirst = false;

            allSelectedValues = $(this).val();
            $(".checkBoxClass").prop('checked', false);
        } else {
            allSelectedValues = allSelectedValues + "," + $(this).val();
            
            $(".checkBoxClass").prop('checked', true);
        }

    });
    $('#allselectcheckbox').removeAttr('onchange');
    $('#allselectcheckbox').attr('onchange','DSelectAll()');
    $('#tableDelete').val(allSelectedValues);
}

function DSelectAll() {
    var allSelectedValues = "";
    var isFirst = true;

    $(".checkBoxClass").each(function () {

        if (isFirst === true) {
            isFirst = false;

            allSelectedValues = $(this).val();
            $(".checkBoxClass").prop('checked', false);
        } else {
            allSelectedValues = allSelectedValues + "," + $(this).val();

            $(".checkBoxClass").prop('checked', false);
        }

    });
    
    $('#allselectcheckbox').removeAttr('onchange');
    $('#allselectcheckbox').attr('onchange', 'SelectAll()');
    $('#tableDelete').val('');
}
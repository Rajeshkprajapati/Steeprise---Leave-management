
let mastercity = {};

mastercity = (function () {
    let addnew = function () {
        $('#cityModal').find('form').get(0).reset();
        $('button[name=update][id=Update]').hide();
        $('button[name=add][id=Add]').show();

        $('label#PopUpModalLabel').text('Add Record');
        $('input[type=text][name=CityCode]').prop('readonly', false);
    }
    let editrow = function (_this) {
        $('label#PopUpModalLabel').text('Edit Record');
        $('div#cityModal').find('form').get(0).reset();
        $('button[name=add][id=Add]').hide();
        $('button[name=update][id=Update]').show();

        $('input[type=text][name=CityCode]').prop('readonly', true);

        var row = $(_this).closest('tr').find('td');
        $('select[name=StateCode][id=StateCode]').val(row[0].innerText);
        $('input[type=text][name=City]').val(row[1].innerText);
        $('input[type=text][name=CityCode]').val(row[2].innerText);
    }

    let deletecity = function (citycode, statecode) {
        SendAJAXRequest(`/ManageCityState/DeleteCity/?citycode=${citycode}&statecode=${statecode}`, 'GET', {}, 'JSON', (resp) => {
            if (resp && resp.msg) {
                let message = "City Deleted Successfully";
                let icon = 'fa fa-thumbs-up';
                updatedsucessfully(message, icon);
            } else {
                let message = "Unable to Delete City";
                let icon = 'fa fa-exclamation';
                warnignPopup(message, icon);
            }
        });
    }

    let adddata = function (data) {
        SendAJAXRequest(`/ManageCityState/AddCity`, 'POST', data, 'JSON', (resp) => {
            if (resp && resp.msg) {
                let message = "City Added Successfully";
                let icon = 'fa fa-thumbs-up';
                updatedsucessfully(message, icon);
            } else {
                let message = "City Code is already exist! Please verify";
                let icon = 'fa fa-exclamation';
                warnignPopup(message, icon);
            }
        });
    }

    let updatedata = function (data) {
        SendAJAXRequest(`/ManageCityState/UpdateCity`, 'POST', data, 'JSON', (resp) => {
            if (resp && resp.msg) {
                let message = "City Update Successfully";
                let icon = 'fa fa-thumbs-up';
                updatedsucessfully(message, icon);
            } else {
                let message = "City Code is already exist! Please verify";
                let icon = 'fa fa-exclamation';
                warnignPopup(message, icon);
            }
        });
    }

    return {
        addnew: addnew,
        editrow: editrow,
        deletecity: deletecity,
        adddata: adddata,
        updatedata: updatedata,
    }

})();

function addnew() {
    mastercity.addnew();
}

function edit(_this) {
    mastercity.editrow(_this);
}

function addcity(_this) {
    let form = $(_this).parent().parent().find("form");
    let formData = ResolveFormData(form);
    if (formData[0].StateCode == "--Select State--" || formData[0].City == "" || formData[0].CityCode == "") {
        let message = "Please enter a valid data";
        let icon = 'fa fa-exclamation';
        warnignPopup(message, icon);
        return false;
    }
    mastercity.adddata(formData[0]);
}
function updatecity(_this) {
    let form = $(_this).parent().parent().find("form");
    let formData = ResolveFormData(form);
    if (formData[0].StateCode == "--Select State--" || formData[0].City == ""  || formData[0].CityCode == "" ) {
        let message = "Please enter a valid data";
        let icon = 'fa fa-exclamation';
        warnignPopup(message, icon);
        return false;
    }
    mastercity.updatedata(formData[0]);
}

function deletedata(cityid, stateid) {
    mastercity.deletecity(cityid, stateid);
}

function ConfrimationCityDeleteMessage(cityid, stateid) {
    let options = {
        backdrop: 'static',
        show: true
    };
    $('#btndelete').attr('onclick', 'deletedata("' + cityid + '","' + stateid + '")');
    $("#confimationDeleteModel").addClass("open");
    $("#confimationDeleteModel").addClass("in");
    $('#confimationDeleteModel').modal(options);
}

$(function () {
    $('#dataTable').dataTable({
        //'columnDefs': [{ "searchable": false, 'orderable': false, 'targets': 5 }],

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
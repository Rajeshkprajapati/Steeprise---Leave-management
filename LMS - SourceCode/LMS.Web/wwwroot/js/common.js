function SendAJAXRequest(actionUrl, method, datatoSend, dType, cb, customHeaders, isFileUpload) {
    $.ajax({
        url: actionUrl,
        data: isFileUpload ? datatoSend : JSON.stringify(datatoSend),
        dataType: dType,//"JSON" ===  dType.toUpperCase() ? "" : dType,
        contentType: isFileUpload ? false : "application/json; charset=utf-8",
        type: method,
        processData: isFileUpload ? false : true,
        beforeSend: (xhr) => {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
            if (customHeaders && customHeaders.length > 0) {
                for (let i = 0; i < customHeaders.length; i++) {
                    for (let h in customHeaders[i]) {
                        xhr.setRequestHeader(h, customHeaders[i][h]);
                    }
                }
            }
        },
        success: function (dt, res1, res2) {
            cb(dt);
        },
        failure: function (response) {
            cb(null);
        },
        error: function (response) {
            if (response && response.responseText.indexOf("returnUrl") > -1) {
                let data = JSON.parse(response.responseText);
                if (data.returnUrl) {
                    window.location.href = data.returnUrl;
                }
            }
            else {
                cb(null);
            }
        }
    });
}


function ResolveFormData(forms) {
    formsData = [];
    for (let i = 0; i < forms.length; i++) {
        let frmData = $(forms[i]).serializeArray();
        if (frmData) {
            dataObj = new Object();
            for (itm of frmData) {
                if (!dataObj.hasOwnProperty(itm.name)) {
                    dataObj[itm.name] = itm.value;
                }
            }
            formsData.push(dataObj);
        }
    }
    return formsData;
}

function resetForm($form) {
    $form.find('input:text, input:password, input:file, select, textarea').val('');
    $form.find('input:radio, input:checkbox')
        .removeAttr('checked').removeAttr('selected');
}

function bindDropDownOptions(selector, dataArr, valueField, textField) {
    if (selector) {
        selector.empty();
        if (dataArr && dataArr.length > 0) {
            for (let i = 0; i < dataArr.length; i++) {
                selector.append(`<option value="${dataArr[i][valueField]}">${dataArr[i][textField]} </option>`);
            }
        }
    }
}

function bindRadioButtons(selector, dataArr, valueField, textField) {
    if (dataArr && dataArr.length > 0) {
        let selectorLength = selector.length;
        let dataLen = dataArr.length;
        for (let i = 0; i < selectorLength; i++) {
            if (i < dataLen) {
                let html = $(selector[i]).parent().find("input[type=radio]");
                if (html && html !== '') {
                    $(selector[i]).parent().html(html);
                    $(selector[i]).val(dataArr[i][valueField]);
                    $(selector[i]).after(dataArr[i][textField]);
                }
            }
        }
    }
}

function replaceNullOrUndefinedToEmpty(obj) {
    for (let key in obj) {
        if (obj[key] === null || obj[key] === undefined) {
            obj[key] = "";
        }
    }
    return obj;
}

function closeModalManually(modal) {
    $(modal).modal("hide");
    $(modal).removeClass("in");
    $("div.modal-backdrop.fade.in").remove();
    $("body").removeClass("modal-open");
}

const Months = [
    'Jan',
    'Feb',
    'Mar',
    'Apr',
    'May',
    'Jun',
    'Jul',
    'Aug',
    'Sep',
    'Oct',
    'Nov',
    'Dec'
];

const Colors = [
    "#800000",
    "#DD5600",
    "orange",
    "#fd7e14",
    "#4ECDC4",
    "grey",
    "purple"
];
function closeAlertPop() {
    $("#popwithoutRedirect").modal("hide");
    $("div.modal-backdrop.fade").removeClass("in");
    $("div.modal-backdrop.fade").addClass("out");
}
function dissmissModal() {

    $("#alertpopup").modal("hide");
    $("div.modal-backdrop.fade").removeClass("in");
    $("div.modal-backdrop.fade").addClass("out");

    //$('#alertpopup').modal('close');
    //$("#alertpopup").removeClass("close");
    location.reload(true);
}
function ConfrimationDeleteMessage(id) {
    let options = {
        backdrop: 'static',
        show: true
    };
    $('#btndelete').attr('onclick', 'deletedata(' + id + ')');
    $("#confimationDeleteModel").addClass("open");
    $("#confimationDeleteModel").addClass("in");
    $('#confimationDeleteModel').modal(options);
}


function ConfirmationDialog(title, msg, cb, data) {

    $.MessageBox({
        buttonDone: "Yes",
        buttonFail: "No",
        message: msg,
        customClass: "custom-success",
        title: title,
    }).done(function () {
        cb(data);
    }).fail(function () {
        return false;
    });

}

function InformationDialog(title, msg) {

    $.MessageBox({
        buttonDone: 'OK',
        message: msg,
        title: title,
        customClass: "custom-success",
    });

}

function ErrorDialog(title, msg) {

    $.MessageBox({
        buttonDone: 'OK',
        message: msg,
        title: title,
        customClass: 'custom-failed',
    });


}

function PromptBoxDialog() {

    $.MessageBox({
        input: true,
        message: "What's your name?"
    }).done(function (data) {
        if ($.trim(data)) {
            $.MessageBox("Hi <b>" + data + "</b>!");
        } else {
            $.MessageBox("You are shy, aren't you?");
        }
    });

    $('.messagebox_title').css("background", "#033c73").css("color", "white");
}

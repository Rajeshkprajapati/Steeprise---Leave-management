$(document).ready(function () {

    $('#empSubmit').attr("disabled", true);
    $('#btnSubmit').attr("disabled", true);


    //SendAJAXRequest(`/Auth/GetCategory`, 'GET', {}, 'JSON', (result) => {
    //    if (result && result.isFound) {
    //        //bindRadioButtons($('input[type=radio][name=RoleId]'), result.list.filter(r => r.isEmp === false), 'roleId', 'roleName');
    //        //bindRadioButtons($('input[type=radio][id=RoleId]'), result.list.filter(r => r.isEmp === true), 'roleId', 'roleName');
    //    } 
    //});

    $('input[type=radio][value="2"]').prop('checked', true);//For Student Checked
    //$('#CandidateId').show();
    $('input[type=radio][value="3"]').prop('checked', true);//For Corporate Checked

    $('input[type=radio][name=RoleId]').change(function () {
        CandidateInputBox();
    });

    $('label.otpclass').css('pointerEvents', 'none');

    $('#CandidateEmail').blur(function () {
        $('label.otpclass').css('pointerEvents', 'auto');
        $('label.otpclass').css('cursor', 'pointer');
    });

    $('#empEmail').blur(function () {
        $('label.otpclass').css('pointerEvents', 'auto');
        $('label.otpclass').css('cursor', 'pointer');
    });

    //Disable all field if candidate id is empty
    disabledField('firstnameStudent');
    disabledField('lastnameStudent');
    disabledField('CandidateEmail');
    disabledField('cotptxtbox');
    disabledField('customFile1');
    disabledField('CandidatePsd');
    disabledField('Candidatecnfpsd');

});

function CandidateInputBox() {

    if ($("input[name='RoleId'][value='2']").prop("checked")) {
        $('#error_sp_msg').hide();
        $("#CandidateId").attr("placeholder", "Please provide Sector Skills Council Nasscom CandidateId");
        $("#CandidateId").attr("title", "Please provide Sector Skills Council Nasscom CandidateId");
        $('#CandidateId').show();
        $("#CandidateId").attr("readonly", false);
        $('#CandidateId').val('');
        $('#firstnameStudent').val('');
        $("#firstnameStudent").attr("readonly", true);
        $('#lastnameStudent').val('');
        $("#lastnameStudent").attr("readonly", true);
        $('#firstname').val('');
        $('#lastname').val('');
        $("#btnSubmit").attr("disabled", true);
        $('#candidatewarning').hide();
    } else if ($("input[name='RoleId'][value='5']").prop("checked")) {

        $('#error_sp_msg').hide();
        $("#CandidateId").attr("placeholder", "Please provide Sector Skills Council Nasscom Training Partner Id");
        $("#CandidateId").attr("title", "Please provide Sector Skills Council Nasscom Training Partner Id");
        $('#CandidateId').val('');
        $('#CandidateId').show();
        $('#candidatewarning').hide();
        $('#firstname').val('');
        $('#lastname').val('');
        $("#CandidateId").attr("readonly", false);
        $('#firstnameStudent').val('');
        $("#firstnameStudent").attr("readonly", true);
        $('#lastnameStudent').val('');
        $("#lastnameStudent").attr("readonly", true);
        $("#btnSubmit").attr("disabled", true);
    } else {
        $('#CandidateId').val('');
        $('#firstname').val('');
        $('#lastname').val('');
        $('#CandidateId').hide();
        $("#btnSubmit").attr("disabled", false);
        $('#candidatewarning').hide();
        
        //$('#CandidateId').css('display', 'none');
    }
}

$("#CandidateId").blur(function () {
    var id = $('#CandidateId').val();

    //alert($('#CandidateId').prop('readonly'));
    if (id !== "" && !$('#CandidateId').prop('readonly')) {
        let url = "";
        var data = $('input[name="RoleId"]:checked').val();
        if (data === "2") { //2 is student from Radiobutton
            url = "/Auth/GetCandidateDeatils/?id=" + id;
        }
        else {
            url = "/Auth/GetTPDeatils/?id=" + id;
        }

        SendAJAXRequest(url, 'get', {}, "json", (result) => {
            if (result && result.isFound) {
                $('#firstnameStudent').attr('readonly', false);
                $('#lastnameStudent').attr('readonly', false);
                $('#firstnameStudent').val(result.user.firstName);
                $('#lastnameStudent').val(result.user.lastName);
                $('#hdnbatchnumber').val(result.user.batchNumber);
                //$("#btnSubmit").attr("disabled", false);
                $("#CandidateId").attr("readonly", true);
                $('#candidatewarning').hide();

                //Enable rest of input
                enableField('firstnameStudent');
                enableField('lastnameStudent');
                enableField('CandidateEmail');
                enableField('cotptxtbox');
                enableField('customFile1');
                enableField('CandidatePsd');
                enableField('Candidatecnfpsd');

            } else {
                $('#candidatewarning').show();
                $('#candidatewarning').text(result.errMsg);

                //Disable the rest of input
                disabledField('firstnameStudent');
                disabledField('lastnameStudent');
                disabledField('CandidateEmail');
                disabledField('cotptxtbox');
                disabledField('customFile1');
                disabledField('CandidatePsd');
                disabledField('Candidatecnfpsd');
            }
        });

    } else {
        $('#candidatewarning').hide();
    }
});

function disabledField(selector) {
    $(`input[id=${selector}]`).prop('disabled', true);
}
function enableField(selector) {
    $(`input[id=${selector}]`).prop('disabled', false);
}

//Traning Partner Registartion

$("#TrainingPartnerID").blur(function () {
    let id = $('#TrainingPartnerID').val();

    //alert($('#CandidateId').prop('readonly'));
    if (id !== "" && !$('#TrainingPartnerID').prop('readonly')) {

        SendAJAXRequest(`/Auth/GetTPDeatils/?id=${id}`, 'GET', {}, "JSON", (result) => {
            if (result && result.isFound) {
                $('#firstnameStudent').val(result.user.firstName);
                $('#lastnameStudent').val(result.user.lastName);
                //$('#Batchnumber').val(result.user.batchnumber);
                $("#btnSubmit").attr("disabled", false);
                $("#TrainingPartnerID").attr("readonly", true);
                $('#tpwarning').hide();
            } else {
                $('#tpwarning').show();
                $('#tpwarning').text(result.errMsg);
            }
        });
    } else {
        $('#tpwarning').hide();
    }
});

function GetOtp() {
    //var idtag = $(_this).attr('id');
    //console.log(idtag);
    if ($('#empEmail').val() !== '') {
        let email = $("#empEmail").val();
        $("#otptxtbox").attr('display', 'none');
        $('#empEmail').attr('readonly', true);
        let RoleId = $("#EmpRegistrationId:checked").val();
       let Name = $("#Companyname").val();
        //alert(EmpRegisterdRoleId);
        let data = { Email: email, RoleId: RoleId, Name:Name };
        SendAJAXRequest('/Auth/GenrateOtp/?Email=' + email + '&&RoleId=' + RoleId + '&&Name=' + Name, 'get', {}, "json", (result) => {
            if (result && result.isOtp === true) {
                $('#lblOtpMsg').css('display', 'block');
                $('#lblOtpMsg').text('OTP has been sent to your email');
                $('#submitGenrate').css('display', 'block');
                $('#otpGenrate').css('display', 'none');
                $('.otpsection').attr('display', 'block');
                $('#reGenrate').css('display', 'none');
            } else if (result && result.isOtp === false) {
                $('#lblOtpMsg').css('display','block');
                $('#lblOtpMsg').text(result.msg);//Email already exist
                $('#empEmail').attr('readonly', false);
            }
            else {
                $('#lblOtpMsg').text('Unable to Send OTP');
            }
        });
    } else {
        //$('#tagginMessage').html('Please enter email!');
        warnignPopup("Please enter email!");
        return false;
    }
}

function submitOtp() {
    let email = $("#empEmail").val();
    let otp = $("#otptxtbox").val();
    if (email !== '' && otp !== '') {
        let data = { Email: email, Otp: otp };
        SendAJAXRequest('/Auth/SubmitOtp/', 'post', data, "json", (result) => {
            if (result && result.matchOtp === true) {
                $('#lblOtpMsg').text('Email is verified');
                $('#empSubmit').attr("disabled", false);
                $('#submitGenrate').css('display', 'none');
            }
            else {
                $('#reGenrate').css('display', 'block');
                $('#submitGenrate').css('display', 'none');
                $('#lblOtpMsg').text('OTP did not matched');
            }
        });
    }
}

function cGetOtp() {
    //var idtag = $(_this).attr('id');
    //console.log(idtag);

    if ($('#CandidateEmail').val() !== '') {
        let email = $("#CandidateEmail").val();
        $("#cotptxtbox").attr('display', 'none');
        $('#CandidateEmail').attr('readonly', true);
        let Name = $("#firstnameStudent").val();
        //let RoleId = $("#RegisterRoleId").val();
        let RoleId = $("#RegisterRoleId:checked").val();
        SendAJAXRequest('/Auth/GenrateOtp/?Email=' + email + '&&Name=' + Name + '&&RoleId=' + RoleId, 'get', {}, "json", (result) => {
            if (result && result.isOtp === true) {
                $('#cotpGenrate').css('display', 'none');
                $('#clblOtpMsg').css('display', 'block');
                $('#clblOtpMsg').text('OTP has been sent to your email');
                $('#csubmitGenrate').css('display', 'block');
                $('#creGenrate').css('display', 'none');
                $('.otpsection').css('cssText', 'display : block !important;');
            } else if (result && result.isOtp === false) {
                $('#clblOtpMsg').css('display', 'block');
                $('#clblOtpMsg').text(result.msg);//Email already exist
                disabledField('btnSubmit');
                $('#CandidateEmail').attr('readonly', false);
            }
            else {
                $('#clblOtpMsg').text('Unable to send OTP');
            }
        });

    } else {
        //$("#iconPopup").addClass('fa fa-exclamation-triangle');
        //$('#tagiging').html("Alert!");
        //$('#tagginMessage').html('Please enter email!');
        //$('#myModal').modal({
        //    dismissible: true
        //});
        //$('#myModal').modal('show');
        //$("#myModal").removeClass("open");
        //$("#myModal").addClass("in");
        warnignPopup("Please enter email!");
        return false;
    }
}

function csubmitOtp() {
    let email = $("#CandidateEmail").val();
    let otp = $("#cotptxtbox").val();
    if (otp !== '' && email !== '') {
        let data = { Email: email, Otp: otp };
        SendAJAXRequest('/Auth/SubmitOtp/', 'post', data, "json", (result) => {
            if (result && result.matchOtp === true) {
                $('#clblOtpMsg').text('Email is verified');
                $('#btnSubmit').attr("disabled", false);
                $('#csubmitGenrate').css('display', 'none');
            }
            else {
                $('#creGenrate').css('display', 'block');
                $('#csubmitGenrate').css('display', 'none');
                $('#clblOtpMsg').text('OTP did not matched');
            }
        });
    }
}

//For Chars Only
SpecialChar($('#firstnameStudent'));
SpecialChar($('#lastnameStudent'));
//For Text and Numbers
SpecialCharNotAllowed($('#Companyname'));


//for numbers
$("#otptxtbox,#cotptxtbox,#TrainingPartnerID").each(function () {
    $(this).keypress(function (e) {
        $("#error_sp_msg").remove();
        let k = e.keyCode,
            $return = (k >= 48 && k <= 57);
        if (!$return) {
            $("<span/>", {
                "id": "error_sp_msg",
                "html": "*Numbers Only",
                "style": "color:red;font-size: 0.7rem;"
            }).insertAfter($(this));
            return false;
        }
    });
});

function RegisterCandidate(_this) {
    event.preventDefault();
    let forms = ResolveFormData($(_this));
    let formData = new FormData();
    for (let key in forms[0]) {
        formData.append(key, forms[0][key]);
    }
    if ($("input[name=ImageFile]").val()) {
        let fileUpload = $("input[name=ImageFile]").get(0);
        let files = fileUpload.files;
        formData.append("ImageFile", files[0]);
    }
    $('#loader').show();  
    SendAJAXRequest('/Auth/SubmitRegistration/', 'POST', formData, 'JSON', (result) => {
        if (result && result.isRegistered) {
            $('#loader').hide();  
            //$('#lblsuccessmsg').text(result.msg);
            //$('#successmsg').css('display', 'block');
            $("#profilelabel").text('');//Clear the label of Picture upload
            $('#clblOtpMsg').css('display', 'none');//clear the label Email varified
            $(_this).get(0).reset();
            let icon = 'fa fa-thumbs-up';
            //let Message = "Registration successful";
            sucessfullyPopupWR(result.msg, icon);
        } else {
            $('#loader').hide();  
            //$('#lblfailuremsg').text(result.msg);
            //$('#failmsg').css('display', 'block');
            //let icon = 'fa fa-exclamation';
            //let Message = "Registration failed";
            //$('.modal-content').css('max-width', '50%');
            warnignPopup(result.msg);
        }
    }, null, true);
}


function RegisterEmployee(_this) {
    event.preventDefault();
    let forms = ResolveFormData($(_this));
    let formData = new FormData();
    for (let key in forms[0]) {
        formData.append(key, forms[0][key]);
    }
    if ($("#customFile2").val()) {
        let fileUpload = $("#customFile2").get(0);
        let files = fileUpload.files;
        formData.append("ImageFile", files[0]);
    }
    $('#loader').show();  
    SendAJAXRequest('/Auth/SubmitEmpRegistration/', 'POST', formData, 'JSON', (result) => {
        if (result && result.isRegistered) {
            $('#loader').hide();  
            //$('#lblsuccessmsg').text(result.msg);
            sucessfullyPopupWR(result.msg, 'fa fa-thumbs-up');
            //$('#successmsg').css('display', 'block');
            $("#profilelabe2").text('');//Clear the label of Picture upload
            $('#lblOtpMsg').css('display', 'none');//clear the label Email varified
            $(_this).get(0).reset();
            
            //let icon = 'fa fa-thumbs-up';
            //let Message = "Registration successfully";
            //updatedsucessfully(result.msg, icon);
            //location.reload(true);
        } else {
            $('#loader').hide();  
            //$('#lblfailuremsg').text(result.msg);
            //$('#failmsg').css('display', 'block');
            //let icon = 'fa fa-exclamation';
            //let Message = "Registration failed";
           // $('.modal-content').css('max-width','50%');
            warnignPopup(result.msg);
        }
    }, null, true);
}

//function isEmail(email) {
//    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
//    warnignPopup("Please enter valid email!");
//    return false;
//}

$('#empEmail,#CandidateEmail').on('focusout', function () {
    var re = /([A-Z0-9a-z_-][^@])+?@[^$#<>?]+?\.[\w]{2,4}/.test(this.value);
    if (!re) {
        warnignPopup("Please enter valid email!");
    } else {
        $('#error').hide();
    }
});
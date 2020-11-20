function changePassword() {
    debugger;
    $('#PasswordForm').submit(function (e) {
        e.preventDefault();
    });

    //var formData = ResolveFormData(_this);

    let oldPassword = $('input[name=OldPassword]').val();
    let Password = $('input[name=Password]').val();
    //var formData = new FormData();
    //formData.append('OldPassword', oldPassword);
    //formData.append('Password', Password);

    var formData = {
        oldPassword,Password
    }

    SendAJAXRequest("/Auth/ChangePassword/", 'post', formData, 'json', function(result) {
        if (result === true) {
            debugger
            InformationDialog('Information','Password successfully changed');
        } else {
            ErrorDialog('Error', 'Current password is not correct');            
        }
    });
    resetForm($('#PasswordForm'));
    return false;
}

function UpdateEmpDetail() {
    $("#empProfile").submit(function (e) {
        e.preventDefault();
    });
    let Cname = $('input[name=CompanyName]').val();
    let ContactPerson = $('input[name=Fullname]').val();
    let email = $('input[name=Email]').val();
    let phone = $('input[name=MobileNo]').val();
    let address = $('input[name=Address]').val();

    var formData = new FormData();
    if ($("#profilepic").val()) {
        var fileUpload = $("#profilepic").get(0);
        var files = fileUpload.files;
        formData.append("ImageFile", files[0]);
    }

    formData.append('CompanyName', Cname);
    formData.append('FullName', ContactPerson);
    formData.append('Email', email);
    formData.append('MobileNo', phone);
    formData.append('Address1', address);    
    

    SendAJAXRequest("/EmployerManagement/UpdateProfile/", "POST", formData, "JSON", function (result) {
        if (result === true) {
            debugger;
            InformationDialog('Information', 'Profile details added/updated successfully');
            //location.reload(true);
        } else {
            ErrorDialog('Error', 'Failed to update profile details!');
        }
    }, null, true);

    return false;
}
//For Alphabet only
$("#ContactPerson").each(function () {
    $(this).keypress(function (e) {
        $("#error_sp_msg").remove();
        var k = e.keyCode,
            $return = ((k > 64 && k < 91) || (k > 96 && k < 123) || k === 8 || k === 32);
        if (!$return) {
            $("<span/>", {
                "id": "error_sp_msg",
                "html": "Special characters/numbers are not allowed !!!!!",
                "style": "color:red"
            }).insertAfter($(this));
            return false;
        }
    });
});
//for numbers only
$("#phoneNumber").each(function () {
    $(this).keypress(function (e) {
        $("#error_sp_msg").remove();
        var k = e.keyCode,
            $return = (k >= 48 && k <= 57);
        if (!$return) {
            $("<span/>", {
                "id": "error_sp_msg",
                "html": "Special characters/alphabets are not allowed !!!!!",
                "style": "color:red"
            }).insertAfter($(this));
            return false;
        }
    });
});

//For alphabet
function SpecialChar(selector) {
    $(selector).each(function () {
        $(this).keypress(function (e) {
            $("#error_sp_msg").remove();
            var k = e.keyCode,
                $return = ((k > 64 && k < 91) || (k > 96 && k < 123) || k === 8 || k === 32);
            if (!$return) {
                $("<span/>", {
                    "id": "error_sp_msg",
                    "html": "Special characters/numbers are not allowed !!!!!",
                    "style": "color:red;font-size: 0.7rem"
                }).insertAfter($(this));
                return false;
            }
        });
    });
}
//For Numbers only
function SpecialCharAndAlphabet(selector) {
    $(selector).keypress(function (e) {
        $("#error_sp_msg").remove();
        var k = e.keyCode,
            $return = (k >= 48 && k <= 57);
        if (!$return) {
            $("<span/>", {
                "id": "error_sp_msg",
                "html": "Special characters/alphabets are not allowed !!!!!",
                "style": "color:red;font-size: 0.7rem;"
            }).insertAfter($(this));
            return false;
        }
    });
}

// For Alphabet and numbers
function SpecialCharNotAllowed(selector) {
    $(selector).keypress(function (event) {
        //var event = e.keyCode,
        $return = ((event.charCode > 64 && event.charCode < 91) || (event.charCode > 96 && event.charCode < 123) || event.charCode === 8 || event.charCode === 32 || (event.charCode >= 48 && event.charCode <= 57));
        if (!$return) {
            $("", {
            }).insertAfter($(this));
            return false;
        }
    });
}


//Common Popup
//function updatedsucessfully(Message, icon) {
//    $("#alertpopup").addClass("in");
//    $("#iconPopup").addClass(icon);
//    $('#tagiging').html("Congratulation!!");
//    $('#tagginMessage').html(Message);
//    $('#alertpopup').modal({
//        dismissible: false
//    });
//    $('#alertpopup').modal('show');
//    $("#alertpopup").removeClass("open");
//    $("#alertpopup").addClass("in");
//}
const options = {
    backdrop: 'static',
    show: true
};
function updatedsucessfully(Message, icon) {   
    $("#iconPopup").addClass(icon);
    $('#tagiging').html("Congratulation!!");
    $('#tagginMessage').html(Message);
    $('#alertpopup').modal(options);

}

//pop up without reload page
function sucessfullyPopupWR(Message, icon) {
    $("#wriconPopup").addClass(icon);
    $('#wrtagiging').html("Congratulation!!");
    $('#wrtagginMessage').html(Message);
    $('#WRSuccessPopup').modal(options);
}
//warning pop up
function warnignPopup(Message) {
    $('#popmessage').html(Message);
    $('#popwithoutRedirect').modal(options);
    $('#popwithoutRedirect').modal('show');
    $("#popwithoutRedirect").removeClass("open");
    $("#popwithoutRedirect").addClass("in");
}

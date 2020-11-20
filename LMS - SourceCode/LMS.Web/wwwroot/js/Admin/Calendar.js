let initCalendar = function (selector, date) {
    let dPicker = tail.DateTime(selector, {
        dateFormat: "YYYY-mm-dd",
        timeFormat: false,
        position: "bottom",
        closeButton: false,
        dateEnd: new Date(),
        dateStart: new Date('01/01/2015')
    })
        .on("open", () => {
            isCalendarOpen = true;
        })
        .on("close", () => {
            isCalendarOpen = false;
        })
    dPicker.selectDate(date.getFullYear(), date.getMonth(), date.getDate());
    return dPicker;
};

function toggleStartCalendar() {
    if (startDateCalendar) {
        startDateCalendar.toggle();
    }
}

function toggleEndCalendar() {
    if (endDateCalendar) {
        endDateCalendar.toggle();
    }
}

let startDateCalendar = initCalendar('#picdate', new Date());
startDateCalendar.on("change", () => {
});

let endDateCalendar = initCalendar('#endpicdate', new Date());

function TDate() {
    var UserDate = document.getElementById("picdate").value;
    var ToDate = document.getElementById("endpicdate").value;
    if (UserDate > ToDate) {
        var d = new Date();
        var month = d.getMonth() + 1;
        var day = d.getDate();
        var output = d.getFullYear() + "-" + (month < 10 ? '0' : '') + month + '-' + (day < 10 ? '0' : '') + day;
        $("#endpicdate").val(output);
      //alert("The Date must be Bigger or Equal to today date");
      let icon = 'fa fa-exclamation';
      let message = "Selected date must be equal or grater than From date";
      warnignPopup(message, icon);
     
    }
    return true;
}
$("#btnPopupWithoutReload").click(function () {
    $('#popwithoutRedirect').modal('hide');
});


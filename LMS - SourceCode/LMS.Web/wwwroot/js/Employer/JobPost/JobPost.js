$(document).ready(function () {

    //country india on load    
    var ddlState = $('#ddlState');

    SendAJAXRequest(`/JobManagement/StateDetails/?CountryCode=${'IN'}`, 'GET', {}, 'JSON', (d) => {
        if (d) {
            ddlState.empty(); // Clear the please wait  
            var valueofstate = "";
            var v = "<option value=" + valueofstate + ">Select State</option>";
            $.each(d, function (i, v1) {
                v += "<option value=" + v1.stateCode + ">" + v1.state + "</option>";
            });
            $("#ddlState").html(v);
            $(".chosen-select-no-single").trigger("chosen:updated");
        } else {
            warnignPopup('Error!');
        }
    });

    //

    $("#ddlCountry").change(function () {
        var CountryId = $(this).val();
        if (CountryId !== "") {
            var ddlState = $('#ddlState');

            SendAJAXRequest(`/JobManagement/StateDetails/?CountryCode=${CountryId}`, 'GET', {}, 'JSON', (d) => {
                if (d) {
                    ddlState.empty(); // Clear the please wait  
                    var valueofstate = "";
                    var v = "<option value=" + valueofstate + ">Select State</option>";
                    $.each(d, function (i, v1) {
                        v += "<option value=" + v1.stateCode + ">" + v1.state + "</option>";
                    });
                    $("#ddlState").html(v);
                    $(".chosen-select-no-single").trigger("chosen:updated");
                } else {
                    warnignPopup('Error!');

                }
            });

        }

    });

    //City Bind By satate id  
    $("#ddlState").change(function () {
        var StateId = $(this).val();
        if (StateId !== "") {
            var ddlCity = $('#ddlCity');

            SendAJAXRequest(`/JobManagement/CityDetails/?stateCode=${StateId}`, 'GET', {}, 'JSON', (d) => {
                if (d) {
                    ddlCity.empty(); // Clear the plese wait  
                    var valueofcity = "";
                    var v = "<option value=" + valueofcity + ">Select City</option>";
                    $.each(d, function (i, v1) {
                        v += "<option value=" + v1.cityCode + ">" + v1.city + "</option>";
                    });
                    $("#ddlCity").html(v);
                    $(".chosen-select-no-single").trigger("chosen:updated");
                } else {
                    warnignPopup('Error!');
                }
            });

        }
    });

    $("#salary").keypress(function (e) {
        if (e.which !== 8 && e.which !== 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });

    $("input[type=radio][id=radioJobType]").change(function () {
        if (this.checked) {
            switch (this.value) {
                case "2":
                    $("select[name=minExp]").parent().css("display", "block");
                    $("select[name=maxExp]").parent().css("display", "block");
                    break;
                case "1":
                case "3":
                    if (this.value === "1") {
                        $("select[name=minExp]").val(0);
                        $("select[name=maxExp]").val(0);
                    }
                    else {
                        $("select[name=minExp]").val(-1);
                        $("select[name=maxExp]").val(-1);
                    }
                    $("select[name=minExp]").parent().css("display", "none");
                    $("select[name=maxExp]").parent().css("display", "none");
                    break;
                default:
                    break;
            }
        }
    });
    $("input[type=radio][id=radioJobType][value=3]").prop("checked", true).change();

    $("select[name=minExp]").change(function () {
        let minExp = parseInt(this.value);
        $("select[name=maxExp] option").each(function (i, o) {
            if (parseInt(o.value) < minExp) {
                $(o).prop("disabled", true);
            }
            else {
                $(o).removeAttr("disabled");
            }
        });
    });
    $('#otherJobIndustrydiv').hide();
    $("select[name=JobIndustryAreaId]").change(function () {
        let industryname = $(this).children('option:selected').text();
        if (industryname == 'Other') {
            $('#otherJobIndustrydiv').show();
        } else {
            $('#otherJobIndustrydiv').hide();
        }
    });

    initializeCalendars(true, true);
    //initializeMultiSelectJobRoles();

});


//function initializeMultiSelectJobRoles() {
//    multiselector.initSelector(
//        $('select#ddlJobRoles'),
//        {
//            nonSelectedText: 'Select job role',
//            maxSelection:5
//        },
//        $("input[type=hidden]#hdnJobTitleId"),
//        ","
//    );
//}


function initializeCalendars(isStartPicker, isEndPicker) {
    if (isStartPicker) {
        let sDate = new Date();
        //  start job 
        initCalendar("input[type=date]#startDate", sDate, sDate);
        $("input[type=date]#startDate").data()
            .on("open", () => {
            })
            .on("close", () => {
            })
            .on("change", () => {
                initializeCalendars(false, true);
            });
    }
    if (isEndPicker) {
        //  end job
        let sd = $("input[type=date]#startDate").data().fetchDate();
        initCalendar("input[type=date]#endDate",
            new Date(sd.setMonth(sd.getMonth() + 1)),
            new Date(sd.setMonth(sd.getMonth() - 1)));

        $("input[type=date]#endDate").data()
            .on("open", () => {
            })
            .on("close", () => {
            })
            .on("change", () => {

            });
    }
}

function initCalendar(selector, date, startDate) {    
    let cal = tail.DateTime(selector, {
        dateFormat: "YYYY-mm-dd",
        timeFormat: false,
        position: "bottom",
        closeButton: false,
        dateStart: startDate ? startDate : new Date('01/01/2015')
    });
    cal.selectDate(date);
    cal.reload();
    $(selector).data(cal);
}

function AddJobPost(_this) {
    //debugger;
    $('#JobPostForm').submit(function (e) {
        e.preventDefault();
    });

    let jobtitle = $('select[name=JobTitleId]').val().toString();
    if (jobtitle.length <= 0) {        
        ErrorDialog('Error', 'Please select one or more job title');
        $(".chosen-select").trigger("chosen:open");
        return false;
    }
    let state = $('select[name=StateCode]').val();
    if (state == null || state == "") {
        ErrorDialog('Error', 'Please select state');
        $("selct#ddlState .chosen-select-no-single").trigger("chosen:open");
        return false;
    }
    let city = $('select[name=CityCode]').val();
    if (city == null || city == "") {
        ErrorDialog('Error', 'Please select city');
        $("select#ddlCity .chosen-select-no-single").trigger("chosen:open");
        return false;
    }    
    let jdetails = CKEDITOR.instances['JobDetails'].getData();
    if (jdetails == null || jdetails == "") {
        ErrorDialog('Error', 'Please fill job details');
        return false;
    }    
    var formData = {
        JobIndustryAreaId: $('select[name=JobIndustryAreaId]').val(),
        JobTitleByEmployer: $('input[name=JobTitleByEmployer]').val(),
        JobTitleId: $('select[name=JobTitleId]').val().toString(),
        EmploymentStatusId: $('select[name=EmploymentStatusId]').val(),
        JobType: $('input[name=JobType]').val(),
        Skills: $('input[name=Skills]').val(),
        HiringCriteria: $('input[name=HiringCriteria]').val(),
        CTC: $('input[name=CTC]').val(),
        PositionStartDate: $('input[name=PositionStartDate]').val(),
        PositionEndDate: $('input[name=PositionEndDate]').val(),
        CountryCode: $('select[name=CountryCode]').val(),
        NoPosition: $('input[name=NoPosition]').val(),
        StateCode: $('select[name=StateCode]').val(),
        CityCode: $('select[name=CityCode]').val(),
        ContactPerson: $('input[name=ContactPerson]').val(),
        Mobile: $('input[name=Mobile]').val(),
        SPOCEmail: $('input[name=SPOCEmail]').val(),
        IsWalkin: $('select[name=IsWalkin]').val(),
        JobDetails: CKEDITOR.instances['JobDetails'].getData()
    };

    SendAJAXRequest('/JobManagement/AddJobPost/', 'POST', formData, 'JSON', (resp) => {
        if (resp) {            
            InformationDialog('Information','Job Post Successful');            
        } else {            
            ErrorDialog('Error', 'Job Post Failed');            
        }
    });
    resetForm($('#JobPostForm'));
    //$('#JobPostForm')[0].reset();
    CKEDITOR.instances['JobDetails'].setData("");
    initializeCalendars(true, true);
    return false;
}

function toggleCalendar(_this) {
    //$(_this).parent().parent().find("input[type=date]").data().toggle();
    $(_this).find("input[type=date]").data().toggle();
}

SpecialChar($('#spoc'));
SpecialCharAndAlphabet($('#spocContact,#nobuerofpostion,#annumSalary,#Experience'));
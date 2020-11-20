let dobCalendar = null;
$(document).ready(function () {
    $("#customImageFile").change(function () {
        $("#profileImagelabel").text(this.files[0].name);
    });


    //......................
    function initCalendar(date) {
        dobCalendar = tail.DateTime("input#DOB[type=date]", {
            dateFormat: "YYYY-mm-dd",
            timeFormat: false,
            position: "bottom",
            closeButton: false,
            dateEnd: new Date() - 1,
            dateStart: new Date('01/01/1950')
        })
            .on("open", () => {

            })
            .on("close", () => {

            })
            .on("change", () => {

            });
        //dobCalendar.selectDate(date.getFullYear(), date.getMonth(), date.getDate());
    };
    let startDateCalendar = initCalendar('input#DOB[type=date]', new Date());
    $("input[type=radio][name=IsCurrentOrganization]").change(function () {
        if (this.value === "true") {
            $("select[name=WorkingTill]").val('-1');
            $("select[name=WorkingTill]").prop('readonly', true);
            $("select[name=WorkingTill]").css('pointerEvents', 'none');
            $("select[name=NoticePeriod]").prop('readonly', false);
            $("select[name=NoticePeriod]").css('pointerEvents', 'auto');
        }
        else {
            $("select[name=WorkingTill]").prop('readonly', false);
            $("select[name=WorkingTill]").css('pointerEvents', 'auto');
            $("select[name=NoticePeriod]").prop('readonly', true);
            $("select[name=NoticePeriod]").css('pointerEvents', 'none');
        }
    });

    $("button[type=button][data-dismiss=modal]").click(function () {
        //$(this).closest("div[role=dialog].modal").modal('close');

        $(this).closest("div[role=dialog].modal").modal("hide");
        $("div.modal-backdrop.fade").removeClass("in");
        $("div.modal-backdrop.fade").addClass("out");
    });

    $("a[data-toggle=modal]").click(function () {

        $($(this).attr('data-target')).modal({
            dismissible: true
        });
        $($(this).attr('data-target')).modal('show');
        $($(this).attr('data-target')).removeClass("open");
        $($(this).attr('data-target')).addClass("in");
        $($(this).attr('data-target')).css("top", "unset");
        $($(this).attr('data-target')).css("opacity", "unset");
        $($(this).attr('data-target')).css("transform", "none");
        $($(this).attr('data-target')).css("z-index", "");
    });


    SendAJAXRequest("/JobSeekerManagement/ProfileData/", "GET", {}, "JSON", function (result) {
        if (result !== null) {
            if (result.Skills !== null && result.Skills.SkillSets !== null) {
                $('label#txtSkillsValues').text(result.Skills.SkillSets);
            }
            $('label#txtProfileSummary').text(result.PersonalDetails.ProfileSummary);
            $('label#usrDataProfileSummary').text(result.PersonalDetails.AboutMe);
            $('#personaldob').text(result.PersonalDetails.DOB);
            $('#personalLinkedinProfile').text(result.PersonalDetails.LinkedinProfile);
            //$('#LinkedinProfile').text(result.PersonalDetails.LinkedinProfile);
            $('#personalemail').text(result.PersonalDetails.Email);
            $('#personaladdress').text(result.PersonalDetails.Address1);
            $('#personalphone').text(result.PersonalDetails.MobileNo);
            $('#personalMaritalSatatusName').text(result.PersonalDetails.MaritalStatusName);
            $('#personalgender').text(result.PersonalDetails.Gender);            
            


            let index = result.PersonalDetails.Resume.lastIndexOf('_');//To show the resume name
            $('#Resumename').text(result.PersonalDetails.Resume.substr(index + 1));
            if (result.PersonalDetails.Resume) {
                $('#Resumefile').attr('href', result.PersonalDetails.Resume);
            }

            $('#candidateid').text(result.PersonalDetails.CandidateId);
            $('#SSCJobRole').text(result.PersonalDetails.SSCJobRole);
            $('#profileimage').attr('src', result.PersonalDetails.ProfilePic);

            //Binding Preferred Location dropdown
            bindDropDownOptions($('#location1'), result.Cities, 'CityCode', 'City');
            bindDropDownOptions($('#location2'), result.Cities, 'CityCode', 'City');
            bindDropDownOptions($('#location3'), result.Cities, 'CityCode', 'City');

            //set location
            let check = result.Cities.filter(i => i.CityCode === result.PersonalDetails.PreferredLocation1);
            //Setting city for others city value
            let cityvalue = result.Cities.filter(c => c.City === 'Other');
            if (check.length > 0 || result.PersonalDetails.PreferredLocation1 === null) {
                $('#location1').val(result.PersonalDetails.PreferredLocation1);
            } else {
                $('#divlocation1').css('display', 'block');
                $('#locationtxt1').val(result.PersonalDetails.PreferredLocation1);
                $('#location1').val(cityvalue[0].CityCode);
            }

            check = result.Cities.filter(i => i.CityCode === result.PersonalDetails.PreferredLocation2);
            if (check.length > 0 || result.PersonalDetails.PreferredLocation2 === null) {
                $('#location2').val(result.PersonalDetails.PreferredLocation2);
            } else {
                $('#divlocation2').css('display', 'block');
                $('#locationtxt2').val(result.PersonalDetails.PreferredLocation2);
                $('#location2').val(cityvalue[0].CityCode);
            }

            check = result.Cities.filter(i => i.CityCode === result.PersonalDetails.PreferredLocation3);
            if (check.length > 0 || result.PersonalDetails.PreferredLocation3 === null) {
                $('#location3').val(result.PersonalDetails.PreferredLocation3);
            } else {
                $('#divlocation3').css('display', 'block');
                $('#locationtxt3').val(result.PersonalDetails.PreferredLocation3);
                $('#location3').val(cityvalue[0].CityCode);
            }

            let cityname1 = result.Cities.filter(c => c.CityCode === result.PersonalDetails.PreferredLocation1);
            let cityname2 = result.Cities.filter(c => c.CityCode === result.PersonalDetails.PreferredLocation2);
            let cityname3 = result.Cities.filter(c => c.CityCode === result.PersonalDetails.PreferredLocation3);
            let l1 = result.PersonalDetails.PreferredLocation1 === null ? '' : result.PersonalDetails.PreferredLocation1;
            let l2 = result.PersonalDetails.PreferredLocation2 === null ? '' : result.PersonalDetails.PreferredLocation2;
            let l3 = result.PersonalDetails.PreferredLocation3 === null ? '' : result.PersonalDetails.PreferredLocation3;
            let preferredlocation = `<p>Location 1 : ${cityname1.length > 0 ? cityname1[0].City : l1} <br /> 
                                        Location 2 : ${cityname2.length > 0 ? cityname2[0].City : l2} <br /> 
                                        Location 3 : ${cityname3.length > 0 ? cityname3[0].City : l3}</p >`;

            $('#preferredlocationdata').append(preferredlocation);


            $('#hdnCurrentSalary').val(result.PersonalDetails.CTC);
            $('#hdnExpectedSalary').val(result.PersonalDetails.ECTC);
            $('#hdnAboutMe').val(result.PersonalDetails.AboutMe);
            $('#hdnJobCategory').val(result.PersonalDetails.JobIndustryArea);
            $('#hdnEmploymentStatus').val(result.PersonalDetails.EmploymentStatus);
            $('#hdnCountry').val(result.PersonalDetails.Country);
            $('#hdnState').val(result.PersonalDetails.State);
            $('#hdnCity').val(result.PersonalDetails.City);
            $("#hdnJobTitleId").val(result.PersonalDetails.JobTitleId);
            $('#hdnMaritalStatus').val(result.PersonalDetails.MaritalStatus);
            $("input#hdnTotalExperience[type=hidden]").val(result.PersonalDetails.TotalExperience);
            $("#usrProfileCTC").text(result.PersonalDetails.CTC);
            $("#usrEmploymentStatus").text(result.PersonalDetails.EmploymentStatusName);
            if (result.PersonalDetails.IsJobAlert === true) {
                $('#btnJobAlert').prop("checked", true);
            }
            if (result.ExperienceDetails !== null) {
                if (result.ExperienceDetails.length > 0) {
                    for (var i = 0; i < result.ExperienceDetails.length; i++) {
                        let o = replaceNullOrUndefinedToEmpty(result.ExperienceDetails[i]);
                        let item = $(`<p class="profile-detail-box">
                            <strong>${o.Designation}</strong>&nbsp;&nbsp;<i class="material-icons customehover" onclick="EditExperience()" style="font-size:15px;">create</i><br />
                            ${o.Organization}<br />
                            ${o.WorkingFrom} to ${o.WorkingTill}<br />
                            Available to join in ${o.NoticePeriod} days<br />
                            ${o.JobProfile}
                            </p>`);
                        item.data('rowData', o);
                        $("#addEmployment").append(item);
                    }
                }
            }

            if (result.EducationalDetails !== null) {
                if (result.EducationalDetails.length > 0) {
                    for (var j = 0; j < result.EducationalDetails.length; j++) {
                        let o = replaceNullOrUndefinedToEmpty(result.EducationalDetails[j]);
                        let item = $(`<p class="profile-detail-box">
                            <strong>${o.CourseName == 'Other' ? o.OtherCourseName : o.CourseName}</strong>&nbsp;&nbsp;<i class="material-icons customehover" onclick="EditEducation()" style="font-size:15px">create</i><br />
                            ${o.University}<br />
                            ${o.Specialization} <br />
                            Graduated in ${o.PassingYear} <br />
                            with ${o.Percentage} %
                            </p>`);
                        item.data('rowData', o);
                        $("#addEducation").append(item);
                    }
                }
            }

            if (result.ITSkills !== null) {
                if (result.ITSkills.length > 0) {
                    for (var k = 0; k < result.ITSkills.length; k++) {
                        let skills = result.ITSkills[k];
                        let item = $(`<tr>
                            <td hidden>${skills.Id}</td>
                            <td>${skills.Skill}</td>
                            <td>${skills.SkillVersion}</td>
                            <td>${skills.LastUsed}</td>
                            <td>${skills.ExperienceYear}</td>
                            <td class="action">
                            <a onclick="EditITSkills(this)"><i class="fa fa-pencil"></i> Edit</a>
                             <a class="delete" onclick="DeleteITSkill(${skills.Id})"><i class="fa fa-remove"></i> Delete</a>
                           </td>
                            </tr>`);
                        item.data('rowData', skills);
                        $("#ITSkills").append(item);
                    }
                }
            }

            if (result.PersonalDetails.ProfileScore !== 0) {
                let item = $(`<div class="progress-bar" role="progressbar" style="width: ${result.PersonalDetails.ProfileScore}%;" aria-valuenow="${result.PersonalDetails.ProfileScore}" aria-valuemin="0" aria-valuemax="100">${result.PersonalDetails.ProfileScore}%</div>`);
                //item.data('rowData', skills);
                $("#ProgressDiv").append(item);
                   }
        }
    });

    $('#CourseCategory').change(function () {
        var categoryID = $(this).val();
        bindCourse(categoryID);
    });

    //bind county
    $("#ddlCountry").change(function () {
        var CountryId = $(this).val();
        if (CountryId !== "") {
            bindCountry(CountryId);
        }
    });

    //City Bind By satate id  
    $("#ddlState").change(function () {
        var StateId = $(this).val();
        if (StateId !== "") {
            bindstate(StateId);
        }
    });
    //initCalendar();

    //preferred location
    $("#location1").change(function () {
        let txt = $(this).val();
        if (txt === 'other') {
            $('#divlocation1').css('display', 'block');
        } else {
            $('#divlocation1').css('display', 'none');
        }
    });
    $("#location2").change(function () {
        let txt = $(this).val();
        if (txt === 'other') {
            $('#divlocation2').css('display', 'block');
        } else {
            $('#divlocation2').css('display', 'none');
        }
    });
    $("#location3").change(function () {
        let txt = $(this).val();
        if (txt === 'other') {
            $('#divlocation3').css('display', 'block');
        } else {
            $('#divlocation3').css('display', 'none');
        }
    });

    $("input[type=radio][id=radioJobType]").change(function () {
        if (this.checked) {
            switch (this.value) {
                case "1":
                    $("select[name=years]").val(0);
                    $("select[name=months]").val(0);
                    $("select[name=years]").parent().css("display", "none");
                    $("select[name=months]").parent().css("display", "none");
                    $("#CurrentSalary").attr("disabled", true);
                    break;
                case "2":
                    $("select[name=years]").parent().css("display", "block");
                    $("select[name=months]").parent().css("display", "block");
                    $("#CurrentSalaryLabel").css("display", "block");
                    $("#CurrentSalary").attr("disabled", false);
                    break;
                default:
                    break;
            }
        }
    });
    $("input[type=radio][id=radioJobType][value=1]").prop("checked", true).change();

    
    $("#ddlcourse").on("change", function () {
        //alert(this.value);
        $('input[type=text][id=othercoursename]').val('');
        let coursename = $(this).children('option:selected').text();
        if (coursename === 'Other') {
            $("#othercoursediv").show();
            //$("#othercoursediv").css("display", 'block');
        } else {
            $("#othercoursediv").hide();
            //$("#othercoursediv").css("display", 'none');
        }
    });

});

function bindCourse(categoryID, courseValue) {
    var ddlCourse = $("#ddlcourse");
    if (categoryID !== '') {
        SendAJAXRequest(`/JobSeekerManagement/GetCourse?categoryID=${categoryID}`, 'GET', {}, "JSON", function (result) {
            if (result !== null) {
                ddlCourse.empty();
                let value = "";
                let course = "<option value=" + value + ">Select Course</option>";
                $.each(result, function (d, i) {
                    if (courseValue === i.id.toString()) {
                        course += "<option value=" + i.id + " selected>" + i.name + "</option>";
                    } else {
                        course += "<option value=" + i.id + ">" + i.name + "</option>";
                    }
                });
                $("#ddlcourse").html(course);
            }
            else {
                ddlCourse.empty();//No course Found for this category.
            }
        });
        //$('#ddlcourse').change();
    }
}

function AddJobProfile(_this) {
    let data = ResolveFormData($(_this));
    if (data && data.length > 0) {
        let dataArr = [];
        data.forEach(function (f) {
            f.AnnualSalary = `${f.AnnualSalaryInLakhs} Lakhs ${f.AnnualSalaryInThousands} Thousands`;
            f.WorkingFrom = `${Months[f.WorkingFromMonth]}, ${f.WorkingFromYear}`;
            //f.IsCurrentOrganisation = f.IsCurrentOrganisation === 'true' ? true : false;
            //f.IsCurrentOrganisation = f.IsCurrentOrganisation === 'on' ? true : false;
            //f.WorkingTill = !f.WorkingTill ? 'Present' : f.WorkingTill;
            f.WorkingTill = f.WorkingTill === '-1' ? 'Present' : f.WorkingTill;
            f.Skills = {
                SkillSets: f.Skills
            };
            delete f.AnnualSalaryInLakhs;
            delete f.AnnualSalaryInThousands;
            delete f.WorkingFromMonth;
            delete f.WorkingFromYear;
            dataArr.push(f);
        });

        $('#employementModalCenter.modal').modal('hide');
        $("#employementModalCenter.modal").removeClass("close");
        $("#employementModalCenter.modal").removeClass("in");

        SendAJAXRequest("/JobSeekerManagement/AddExperienceDetail/", "POST", dataArr, "JSON", function (result) {
            //debugger;
            //console.log(result);
            if (result === true) {
                let icon = 'fa fa-thumbs-up';
                let Message = "Experience details added/updated successfully";
                updatedsucessfully(Message, icon);

            }
            else {
                let icon = 'fa fa-exclamation';
                let Message = "Experience details could not added/updated";
                updatedsucessfully(Message, icon);
            }
        });

    }
}

function bindCountry(CountryId) {
    var ddlState = $('#ddlState');
    SendAJAXRequest(`/JobSeekerManagement/StateDetails/?CountryCode=${CountryId}`, 'GET', {}, 'JSON', (d) => {
        if (d) {
            ddlState.empty(); // Clear the please wait  

            var value = "";
            var v = "<option value=" + value + ">Select State</option>";
            $.each(d, function (i, v1) {
                if ($('#hdnState').val() === v1.stateCode) {
                    v += "<option value=" + v1.stateCode + " selected>" + v1.state + "</option>";
                } else {
                    v += "<option value=" + v1.stateCode + ">" + v1.state + "</option>";
                }
            });
            $("#ddlState").html(v);
        } else {
            alert('Error!');
        }

    });
}

function bindstate(StateId) {
    let ddlCity = $('#ddlCity');

    SendAJAXRequest(`/JobSeekerManagement/CityDetails/?stateCode=${StateId}`, 'GET', {}, 'JSON', (d) => {
        if (d) {
            ddlCity.empty(); // Clear the plese wait  

            var value = "";
            var v = "<option value=" + value + ">Select City</option>";
            $.each(d, function (i, v1) {
                if ($('#hdnCity').val() === v1.cityCode) {
                    v += "<option value=" + v1.cityCode + " selected>" + v1.city + "</option>";
                } else {
                    v += "<option value=" + v1.cityCode + ">" + v1.city + "</option>";
                }
            });
            $("#ddlCity").html(v);
        } else {
            alert('Error');
        }
    });
}


function AddEducation(_this) {
    let data = ResolveFormData($(_this));
    $('#educationModalCenter').modal('hide');
    $("#educationModalCenter").removeClass("close");
    $("#othercoursediv").hide();
    if (data && data.length > 0) {
        SendAJAXRequest("/JobSeekerManagement/AddEducationDetail/", "POST", data, "JSON", function (result) {
            if (result && result.isSuccess) {
                if (result.msg) {
                    window.location.href = result.msg;
                }
                else {
                    $('.element.style').hide();
                    let Message = "Education details added/updated successfully";
                    let icon = 'fa fa-thumbs-up';
                    updatedsucessfully(Message, icon);
                }
            }
            else {
                let Message = "Your education details could not insert/update";
                let icon = 'fa-exclamation';
                updatedsucessfully(Message, icon);
            }
        });
    }
}

function AddProfileDetail() {
    var dob = $('#DOB').val();
    var email = $('#emailId1').val();
    var address = $('#txtAddress').val();
    var phn = $('#phoneNumber1').val();
    var maritalStatus = $('#ddlMaritalStatus').val();
    var gender;
    if ($("#btnMale").is(":checked")) {
        gender = 'male';
    }
    else {
        gender = 'female';
    }
    var currentSalary = $('#CurrentSalary').val();
    var expectedSalary = $('#ExpectedSalary').val();
    var abountMe = $('#AboutMe').val();
    var jobCategory = $('#ddlJobCategory').val();
    var employmentStatus = $('#ddlEmploymentStatus').val();
    var country = $('#ddlCountry').val();
    var state = $('#ddlState').val();
    var city = $('#ddlCity').val();
    var userIdData = $('#userId').val();
    let jobtitle = $('select[name=JobTitle]').val();
    let exp = 0;
    let expInYears = $("select[name=years]").val();
    let expInMonths = $("select[name=months]").val();
    if ($("input[type=radio][id=radioJobType]").val() === "1") {
        exp = `${expInYears}.${expInMonths}`;
    }
    if (jobtitle === ""){
        let Message = "Please select JobTitle";
        warnignPopup(Message);
        return false;
    }
    if (country === "") {
        let Message = "Please select country";
        warnignPopup(Message);
        return false;
    }
    if (state === "") {
        let Message = "Please select State";
        warnignPopup(Message);
        return false;
    }
    if (city === "") {
        let Message = "Please select City";
        warnignPopup(Message);
        return false;
    }
    let LinkedinProfile = $('#LinkedinProfile').val();
    var data = {
        DOB: dob, Email: email, Address1: address,
        MobileNo: phn, MaritalStatus: maritalStatus, Gender: gender,
        ECTC: expectedSalary, AboutMe: abountMe, JobIndustryArea: jobCategory,
        EmploymentStatus: employmentStatus, Country: country, State: state, City: city,
        CTC: currentSalary, userId: userIdData,
        TotalExperience: exp, LinkedinProfile: LinkedinProfile, JobTitleId: jobtitle
    };
    $('#profileModalCenter').modal('hide');
    $("#profileModalCenter").removeClass("close");
    SendAJAXRequest("/JobSeekerManagement/AddProfileDetail/", "POST", data, "JSON", function (result) {
        if (result === true) {
            let icon = 'fa fa-thumbs-up';
            let Message = "Personal details added/updated successfully";
            updatedsucessfully(Message, icon);

        }
    });
}

function AddProfileSummary() {
    let profileData = $('#txtareaprofileSummary').val();
    let userIdData = $('#userId').val();
    $('#ProfileSummaryModalCenter').modal('hide');
    $("#ProfileSummaryModalCenter").removeClass("close");
    if (profileData !== "") {
        var data = {};
        SendAJAXRequest("/JobSeekerManagement/AddProfileSummary/?profile=" + profileData + "&userId=" + userIdData + "", "POST", data, "JSON", function (result) {
            if (result === true) {
                let icon = 'fa fa-thumbs-up';
                let messagedata = "Profile summary added/updated successfully";
                updatedsucessfully(messagedata, icon);

            }
        });
    }
}

function AddProfileSkills() {

    let value = $.trim($('#skillsValue').val());
    //let value = $('#skillsValue').tagEditor('getTags')[0].tags;
    let data = { SkillSets: value };
    if (value && value !== null) {
        SendAJAXRequest("/JobSeekerManagement/AddSkillDetails/", "POST", data, "JSON", function (result) {
            if (result && result.isSuccess) {
                if (result.msg) {
                    window.location.href = result.msg;
                }
                else {
                    $('#skillsModalCenter').modal('hide');
                    $("#skillsModalCenter").addClass("close");
                    let icon = 'fa fa-thumbs-up';
                    let Message = "Skill updated successfully";
                    updatedsucessfully(Message, icon);
                    //InformationDialog('Information', Message);
                }
            } else {
                let icon = 'fa fa-exclamation';
                let message = "Error in Adding Skills!";
                warnignPopup(message, icon);
            }
        });
    } else {
        let icon = 'fa fa-exclamation';
        let message = "Enter valid skills only";
        warnignPopup(message, icon);

    }   
}


function UploadResume(inputId) {
    let fileUpload = $("#" + inputId).get(0);
    let files = fileUpload.files;
    let format = files[0].name.split('.').pop();
    if (format === "pdf" || format === "doc" || format === "docx") {
        let formData = new FormData();
        formData.append('resume', files[0]);
        SendAJAXRequest('/JobSeekerManagement/UploadFileValue/', 'POST', formData, 'JSON', (result) => {
            if (result) {
                let message = "Resume upload successfully";
                let icon = 'fa fa-thumbs-up';
                updatedsucessfully(message, icon);

            } else {
                let icon = 'fa fa-exclamation';
                let message = "Error in Uploading Resume!";
                warnignPopup(message, icon);

            }
        }, null, true);
    } else {
        let icon = 'fa fa-exclamation';
        let message = "Allowed File formats doc,docx and pdf!";
        warnignPopup(message, icon);
    }

}

function UploadProfilePicture(_this) {
    let pictureFile = $(_this).get(0);
    let picture = pictureFile.files;
    let formData = new FormData();
    formData.append('profilepic', picture[0]);
    SendAJAXRequest(`/JobSeekerManagement/UploadProfilePicture/`, 'POST', formData, 'JSON', (result) => {
        if (result) {
            let message = "Profile Picture updated successfully";
            let icon = 'fa fa-thumbs-up';
            updatedsucessfully(message, icon);
        } else {
            let icon = 'fa fa-exclamation';
            let message = "Error in updating Profile Picture!";
            updatedsucessfully(message, icon);
        }
    }, null, true);
}


function EditExperience() {
    let rowData = $(event.target).parent().data('rowData');
    for (let key in rowData) {
        let e = $("#employementModalCenter.modal").find(`[name=${key}]`);
        if (e && e.is('input[type=radio]')) {
            for (let i = 0; i < e.length; i++) {
                if (e[i].value === rowData[key].toString()) {
                    let iscurrentorganization = rowData[key];
                    if (!iscurrentorganization) {
                        $('#employementModalCenter.modal').find('[name=NoticePeriod]').val(-1);
                        $('#employementModalCenter.modal').find('[name=NoticePeriod]').prop('readonly', true);
                        $('#employementModalCenter.modal').find('[name=NoticePeriod]').css('pointerEvents', 'none');
                    } else {
                        $('#employementModalCenter.modal').find('[name=NoticePeriod]').prop('readonly', false);
                        $('#employementModalCenter.modal').find('[name=NoticePeriod]').css('pointerEvents', 'auto');
                    }
                    $(e[i]).prop('checked', true);
                }
            }
        }
        else {
            switch (key) {
                case "WorkingFrom":
                    let workingexp = rowData[key].split(',');
                    $('#employementModalCenter.modal').find('[name=WorkingFromYear]').val(workingexp[1].trim());
                    $('#employementModalCenter.modal').find('[name=WorkingFromMonth]').val(Months.indexOf(workingexp[0]));
                    break;
                case "AnnualSalary":
                    let salary = rowData[key].split(' ');
                    $('#employementModalCenter.modal').find('[name=AnnualSalaryInLakhs]').val(salary[0]);
                    $('#employementModalCenter.modal').find('[name=AnnualSalaryInThousands]').val(salary[2]);
                    break;
                case "WorkingTill":
                    let workingtill = rowData[key];
                    if (workingtill === 'Present') {
                        $('#employementModalCenter.modal').find('[name=WorkingTill]').val(-1);
                        $('#employementModalCenter.modal').find('[name=WorkingTill]').prop('readonly', true);
                        $('#employementModalCenter.modal').find('[name=WorkingTill]').css('pointerEvents', 'none');
                    }
                    else {
                        $('#employementModalCenter.modal').find('[name=WorkingTill]').prop('readonly', false);
                        $('#employementModalCenter.modal').find('[name=WorkingTill]').css('pointerEvents', 'auto');
                        $('#employementModalCenter.modal').find('[name=WorkingTill]').val(workingtill);
                    }
                    break;
                default:
                    e.eq(0).val(rowData[key]);
                    break;
            }
        }
    }



    $('#employementModalCenter.modal').modal({
        dismissible: true
    });
    $('#employementModalCenter.modal').modal('show');
    $("#employementModalCenter.modal").removeClass("open");
    $("#employementModalCenter.modal").addClass("in");

}

function EditEducation() {
    $("#othercoursediv").hide();
    let rowData = $(event.target).parent().data('rowData');
    for (let key in rowData) {
        let e = $("#educationModalCenter.modal").find(`[name=${key}]`);
        if (e && e.is('input[type=radio]')) {
            for (let i = 0; i < e.length; i++) {
                if (e[i].value === rowData[key].toString()) {
                    $(e[i]).prop('checked', true);
                }
            }
        }
        else {
            //bindDropDownOptions($('#educationModalCenter.modal').find('[name=Course]'), resp.data.Cities, 'CityCode', 'City');
            if (key === "Qualification") {
                bindCourse(rowData[key], rowData['Course']);
            }
            if (key === "OtherCourseName") {
                //$('input[type=text][id=othercoursename]').val('');
                if (rowData['CourseName'] == 'Other' &&(rowData["OtherCourseName"] != null || rowData["OtherCourseName"] == '')) {
                    $("#othercoursediv").show();
                } else {
                    $("#othercoursediv").hide();
                }
            }
            e.eq(0).val(rowData[key]);

        }
    }


    $('#educationModalCenter').modal({
        dismissible: true
    });
    $('#educationModalCenter').modal('show');
    $("#educationModalCenter").removeClass("open");
    $("#educationModalCenter").addClass("in");

}

function EditProfileData() {

    $('#DOB').val($('#personaldob').text());
    $('#LinkedinProfile').val($('#personalLinkedinProfile').text());
    $('#emailId').val($('#email').text());
    $('#txtAddress').val($('#personaladdress').text());
    $('#phoneNumber1').val($('#personalphone').text());
    $('#ddlMaritalStatus').val($('#hdnMaritalStatus').val());
    $('#userId').val();
    var gender = $('#personalgender').text();
    if (gender === 'Male' || gender === "male") {
        $('#btnMale').prop("checked", true);
    }
    else {
        $('#btnFemale').prop("checked", true);
    }
    $('#CurrentSalary').val($('#hdnCurrentSalary').val());
    $('#ExpectedSalary').val($('#hdnExpectedSalary').val());
    $('#AboutMe').val($('#hdnAboutMe').val());
    $('#ddlJobCategory').val($('#hdnJobCategory').val());
    $('#ddlEmploymentStatus').val($('#hdnEmploymentStatus').val());
    $('#ddlCountry').val($('#hdnCountry').val());
    $('select[name=JobTitle]').val($('#hdnJobTitleId').val());
    //$('#ddlJobTitle').val($('#hdnJobTitleId').val());
    //$(".chosen-select-no-single").trigger("chosen:updated");
    //alert($('#ddlCountry').val($('#hdnCountry').val()));


    if ($('#ddlCountry').val() !== "") {
        var CountryCode = $('#ddlCountry').val();
        bindCountry(CountryCode);
    }
    if ($('#hdnState').val() !== "") {
        var stateCode = $('#hdnState').val();
        bindstate(stateCode);
    }

    let exp = $("input#hdnTotalExperience[type=hidden]").val().split(".");
    if (exp && exp.length === 2) {
        if (exp[0] === 0 && exp[1] === 0) {
            $("input[type=radio][id=radioJobType][value=1]")
                .prop("checked", true).change();
        }
        else {
            $("select[name=years]").val(exp[0]);
            $("select[name=months]").val(exp[1]);
            $("input[type=radio][id=radioJobType][value=2]")
                .prop("checked", true).change();
        }
    }


    $('#profileModalCenter').modal({
        dismissible: false
    });
    $('#profileModalCenter').modal('show');
    $("#profileModalCenter").removeClass("open");
    $("#profileModalCenter").addClass("in");
}

function EditProfileSkills() {
    let value = $('label#txtSkillsValues').text();
    if (value !== null && value !== "") {
        $('#skillsValue').tagEditor('destroy');
        //let skillsarray = value.split(',');

        $('#skillsValue').tagEditor({
            initialTags: value.split(',')
        });
    }

    $('#skillsModalCenter').modal({
        dismissible: true
    });
    $('#skillsModalCenter').modal('show');
    $("#skillsModalCenter").removeClass("open");
    $("#skillsModalCenter").addClass("in");
}

function EditITSkills(_this) {
    var row = $(_this).closest('tr').find('td');
    $('#ITSkillId').val(row[0].innerText);
    $('#ITSkill').val(row[1].innerText);
    $('#SkillVersion').val(row[2].innerText);
    $('#LastUsed').val(row[3].innerText);
    $('#ExperienceYear').val(row[4].innerText);
    

   $('#ITSkillsModel').modal({
        dismissible: true
    });
    $('#ITSkillsModel').modal('show');
    $("#ITSkillsModel").removeClass("open");
    $("#ITSkillsModel").addClass("in");
}

function AddITSkills(){
    $('#ITSkillsModel').modal({
        dismissible: true
    });
    $('#ITSkillsModel').modal('show');
    $("#ITSkillsModel").removeClass("open");
    $("#ITSkillsModel").addClass("in");
}
function DeleteITSkill(id) {
    var data = "";
    SendAJAXRequest("/JobSeekerManagement/DeleteITSkill/?ITSkillId=" + id + "", 'POST', data, 'JSON', function (result) {
        if (result) {
            let icon = 'fa fa-thumbs-up';
            updatedsucessfully(result, icon);
        } else {
            warnignPopup('Error');
        }
    });
}

function UpdateITSKill()
{
    let ITSkillId = $('#ITSkillId').val();
    let ITSkill = $('#ITSkill').val();
    let SkillVersion = $('#SkillVersion').val();
    let LastUsed = $('#LastUsed').val();
    let ExperienceYear = $('#ExperienceYear option:selected').val();
    let ExperienceMonth = $('#ExperienceMonth option:selected').val();
    var data = {
        Id: ITSkillId, Skill: ITSkill, SkillVersion: SkillVersion,
        LastUsed: LastUsed, ExperienceYear: ExperienceYear,
        ExperienceMonth: ExperienceMonth
    };
    SendAJAXRequest("/JobSeekerManagement/UpdateITSkill/", 'POST', data, 'JSON', function (result) {
    if (result) {
            let icon = 'fa fa-thumbs-up';
        updatedsucessfully("Successfully done", icon);
          } else {
            warnignPopup('Error');
        }
    });
}

function EditProfileSummary() {
    var value = $('#txtProfileSummary').text();
    if (value !== null && value !== "") {
        $('#txtareaprofileSummary').val(value);
    }
    $('#ProfileSummaryModalCenter').modal({
        dismissible: true
    });
    $('#ProfileSummaryModalCenter').modal('show');
    $("#ProfileSummaryModalCenter").removeClass("open");
    $("#ProfileSummaryModalCenter").addClass("in");
}

function AddPreferredLocation() {
    let value1 = $('#location1').val();
    let value2 = $('#location2').val();
    let value3 = $('#location3').val();
    let preferlocation = [];
    if (value1 !== null && value1 !== '') {
        if (value1 === 'other') {
            value1 = $('#locationtxt1').val();
        }
        preferlocation.push(value1);
    } if (value2 !== null && value2 !== '') {
        if (value2 === 'other') {
            value2 = $('#locationtxt2').val();
        }
        preferlocation.push(value2);
    } if (value3 !== null && value3 !== '') {
        if (value3 === 'other') {
            value3 = $('#locationtxt3').val();
        }
        preferlocation.push(value3);
    }

    SendAJAXRequest(`/JobSeekerManagement/AddPreferredLocation`, 'POST', preferlocation, 'JSON', (result) => {
        if (result && result.isSuccess) {
            //Unload the modal
            $('#locationModalCenter').modal('hide');
            $("#locationModalCenter").addClass('close');
            //show custom popup
            let icon = 'fa fa-thumbs-up';
            let Message = "Preferred Location added/updated successfully";
            updatedsucessfully(Message, icon);
            //alert('data saved');
        } else {
            $('#locationModalCenter').modal('hide');
            $("#locationModalCenter").addClass('close');
            let icon = 'fa fa-exclamation';
            let Message = "Failed to add/update Preferred Location";
            updatedsucessfully(Message, icon);
        }
    });
}

//const mysuccesspopup = (Message, icon) => {
//    let options = {
//        backdrop: "static",
//        show: true
//    };
//    $("#wriconPopup").addClass(icon);
//    $('#wrtagiging').html("Congratulation!!");
//    $('#wrtagginMessage').html(Message);
//    $('#WRSuccessPopup').modal(options);
//};


var modaloptions = {
    backdrop: "static",
    show: true
};

//function EditPreferredLocation() {
//    //Show the location modal
//    $('#locationModalCenter').modal(modaloptions);
//    $('#locationModalCenter').modal('show');
//    $("#locationModalCenter").removeClass("open");
//    $("#locationModalCenter").addClass("in");
//}


function ShowModal(modalname) {

    $($(`#${modalname}`)).modal(modaloptions);
    $($(`#${modalname}`)).modal('show');
    $($(`#${modalname}`)).removeClass("open");
    $($(`#${modalname}`)).addClass("in");

    if (modalname === 'employementModalCenter' || modalname === 'educationModalCenter') {
        $($(`#${modalname}`)).find('form').get(0).reset();
    }
}



//For alphabet
SpecialChar($('#AboutMe'));

SpecialCharAndAlphabet($("#phoneNumber1, #ExpectedSalary, #CurrentSalary, #GradingSystemPercentages"));


$('#ExpectedSalary').blur(function () {
    if ($('#ExpectedSalary').val() !== "") {
        $("#ExpectedSalary").val(parseFloat($("#ExpectedSalary").val(), 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
    }
});

$('#CurrentSalary').blur(function () {
    if ($('#CurrentSalary').val() !== "") {
        $("#CurrentSalary").val(parseFloat($("#CurrentSalary").val(), 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
    }
});

$("#resumedonewithlabel").click(function () {
    labelID = $(this).attr('for');
    $('#' + labelID).trigger('click');
});
$("#ResumeUploadWithLabel").click(function () {
    $("#uplaodFile").click();
});
$("#iconclickresume").click(function () {
    $("#uplaodFile").click();
});

$("#usrprofilepicupload").click(function () {
    $("#customImageFile").click();
});

$("#btnJobAlert").change(function () {
    let isAlert;
    if (this.checked) {
        isAlert = 1;
    }
    else {
        isAlert = 0;
    }
    data = "";
    SendAJAXRequest("/JobSeekerManagement/JobsAlert/?isAlert=" + isAlert + "", 'POST', data, 'JSON', function (result) {
        if (result) {

            //let icon = 'fa fa-thumbs-up';
            //updatedsucessfully(result, icon);
        } else {
            warnignPopup('Error');
        }
    });
});

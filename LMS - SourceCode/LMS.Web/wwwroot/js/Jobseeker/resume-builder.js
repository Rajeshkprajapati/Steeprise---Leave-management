function GenerateCV(_this) {
    let html = $('div#resumeContainer')[0].outerHTML;
    SendAJAXRequest("/ResumeBuilder/CreateResume", "POST", html, "json", function (resp) {
        if (resp && resp.isSuccess) {
            window.location.href = "/ResumeBuilder/DownloadResume";
        }
        else {
            return false;
        }
    });
}

function savePersonalDetails(_this, ev) {
    ev.preventDefault();
    let data = ResolveFormData($(_this));
    if (data && data.length > 0) {
        let dataArr = [];
        data.forEach(function (f) {
            dataArr.push(f);
        });

        SendAJAXRequest("/ResumeBuilder/SavePersonalDetails", "POST", dataArr, "json", function (resp) {
            if (resp && resp.isSuccess) {
                $("a[href=#Employment]").click();
            }
            else {
                return false;
            }
        });
    }
    else {
        warnignPopup("Some issues with user's employment details, please contact your admin")
        return false;
    }
}

function saveExperienceDetails(_this) {
    let data = ResolveFormData($(_this.parentNode).find('form'));
    if (data && data.length > 0) {
        let dataArr = [];
        let skills = $(_this.parentNode).find('input[type=hidden][name=Skills]').val();
        let skillItems = skills.toUpperCase().split(",");
        data.forEach(function (f) {
            f.AnnualSalary = `${f.AnnualSalaryInLakhs} Lakhs ${f.AnnualSalaryInThousands} Thousands`;
            f.WorkingFrom = `${Months[f.WorkingFromMonth]}, ${f.WorkingFromYear}`;
            f.IsCurrentOrganization = f.WorkingTill !== '' ? false : true;
            f.Skills = {
                SkillSets: f.Skills
            };
            delete f.AnnualSalaryInLakhs;
            delete f.AnnualSalaryInThousands;
            delete f.WorkingFromMonth;
            delete f.WorkingFromYear;
            dataArr.push(f);

            //  Skills handling here
            let tempSkills = f.Skills.SkillSets.split(",");
            if (tempSkills && tempSkills.length > 0) {
                tempSkills.forEach(function (s, i) {
                    if (skillItems.indexOf(s.trim().toUpperCase()) < 0) {
                        skillItems.push(s.trim().toUpperCase());
                    }
                });
            }
        });


        SendAJAXRequest("/ResumeBuilder/SaveExperienceDetails", "POST", { ExperienceDetails: dataArr, Skills: { SkillSets: skillItems.join() } }, "json", function (resp) {
            if (resp && resp.isSuccess) {
                $("a[href=#Education]").click();
            }
            else {
                return false;
            }
        });
    }
    else {
        warnignPopup("Some issues with user's employment details, please contact your admin")
        return false;

    }
}

function createExperiencDetailsForm(_this) {
    let finalSaveButton = $(_this.parentNode.parentNode).find("#btnSaveEmploymentInfo");
    if (finalSaveButton) {
        finalSaveButton.remove();
    }
    $(_this).parent().parent().append($(`<div class="formrow other-employment-title"><h6>Another Employment</h6></div>`));
    let forms = $(_this).parent().parent().find('form');
    forms.each(function (i, f) {
        $(f).find("a#btnAddAnOtherExperience").hide();
    });
    let lForm = forms.eq(forms.length - 1).clone();
    lForm.attr('id', 'frmExperienceDetails_' + forms.length);
    lForm.find("input[type=hidden][name=Id]").val("0");
    lForm.find("a#btnAddAnOtherExperience").show();
    lForm.get(0).reset();
    $(_this).parent().parent().append(lForm);
    $(_this).parent().parent().append($(finalSaveButton));
}

function saveEducationDetails(_this) {
    let data = ResolveFormData($(_this.parentNode).find('form'));
    if (data && data.length > 0) {
        SendAJAXRequest("/ResumeBuilder/SaveEducationDetails", "POST", data, "json", function (resp) {
            if (resp && resp.isSuccess) {
                window.location.href = "/ResumeBuilder/ResumePreview";
            }
            else {
                return false;
            }
        });
    }
    else {
        warnignPopup("Some issues with user's educational details, please contact your admin")
        return false;
    }
}

function createEducationalDetailsForm(_this) {
    let finalSaveButton = $(_this.parentNode.parentNode).find("#btnSaveEducationInfo");
    if (finalSaveButton) {
        finalSaveButton.remove();
    }
    $(_this).parent().parent().append($(`<div class="formrow other-education-title"><h6>Another Education</h6></div>`));
    let forms = $(_this).parent().parent().find('form');
    forms.each(function (i, f) {
        $(f).find("a#btnAddAnOtherAcademic").hide();
    });
    let lForm = forms.eq(forms.length - 1).clone();
    lForm.attr('id', 'frmEducationalDetails_' + forms.length);
    lForm.find("input[type=hidden][name=Id]").val("0");
    lForm.find("a#btnAddAnOtherAcademic").show();
    lForm.get(0).reset();
    $(_this).parent().parent().append(lForm);
    $(_this).parent().parent().append($(finalSaveButton));
}

function getStates(_this) {
    SendAJAXRequest("/ResumeBuilder/GetStates?countryCode=" + _this.value, "GET", {}, "JSON", function (resp) {
        if (resp && resp.isSuccess) {
            bindDropDownOptions($('form#personalDetails').find('[name=State]'), resp.States, 'StateCode', 'State');
        }
        else {
            return false;
        }
    });
}

function getCities(_this) {
    SendAJAXRequest("/ResumeBuilder/GetCities?stateCode=" + _this.value, "GET", {}, "JSON", function (resp) {
        if (resp && resp.isSuccess) {
            bindDropDownOptions($('form#personalDetails').find('[name=City]'), resp.Cities, 'CityCode', 'City');
        }
        else {
            return false;
        }
    });
}

function changeCourseCategory(_this) {
    let form = $(_this).closest('form');
    SendAJAXRequest("/ResumeBuilder/GetCourses?cCategory=" + _this.value, "GET", {}, "JSON", function (resp) {
        if (resp && resp.isSuccess) {
            if (resp.Courses && resp.Courses.length > 0) {
                $(form).find('[name=Course]').closest('.formrow').show();
                bindDropDownOptions($(form).find('[name=Course]'), resp.Courses, 'Id', 'Name');
            }
            else {
                $(form).find('[name=Course]').closest('.formrow').hide();
            }
        }
        else {
            return false;
        }
    });
}

(function () {
    let dobCalendar = tail.DateTime("input#DOB[type=date]", {
        dateFormat: "YYYY-mm-dd",
        timeFormat: false,
        position: "top",
        closeButton: false,
        dateEnd: new Date()
    })
        .on("open", () => {

        })
        .on("close", () => {

        })
        .on("change", () => {

        });

    SpecialChar($('form#personalDetails').find('[name=FirstName]'));
    SpecialChar($('form#personalDetails').find('[name=LastName]'));
    SpecialCharNotAllowed($('form#personalDetails').find('[name=Address1]'));
    SpecialCharNotAllowed($('form#personalDetails').find('[name=Address2]'));
    SpecialCharNotAllowed($('form#personalDetails').find('[name=Address3]'));
    SpecialCharAndAlphabet($('form#personalDetails').find('[name=MobileNo]'));
    SendAJAXRequest("/ResumeBuilder/GetUserDetails", "GET", {}, "JSON", function (resp) {
        if (resp && resp.isSuccess) {

            //  Personal Details
            bindDropDownOptions($('form#personalDetails').find('[name=Country]'), resp.data.Countries, 'CountryCode', 'Country');
            bindDropDownOptions($('form#personalDetails').find('[name=State]'), resp.data.States, 'StateCode', 'State');
            bindDropDownOptions($('form#personalDetails').find('[name=City]'), resp.data.Cities, 'CityCode', 'City');

            bindRadioButtons($('form#personalDetails').find('[name=Gender]'), resp.data.Gender, 'GenderCode', 'Gender');
            bindRadioButtons($('form#personalDetails').find('[name=MaritalStatus]'), resp.data.MaritalStatus, 'StatusCode', 'Status');
            for (let key in resp.data.uDetail.PersonalDetails) {
                let e = $('form#personalDetails').find('[name=' + key + ']');
                if (e && e.is('input[type=radio]')) {
                    for (let i = 0; i < e.length; i++) {
                        if (e[i].value === resp.data.uDetail.PersonalDetails[key]) {
                            $(e[i]).prop('checked', true);
                        }
                    }
                }
                else {
                    e.eq(0).val(resp.data.uDetail.PersonalDetails[key]);
                }
            }

            //  Professional Details

            bindDropDownOptions($('form#frmExperienceDetails_0').find('[name=WorkLocation]'), resp.data.AllCities, 'CityCode', 'City');
            bindDropDownOptions($('form#frmExperienceDetails_0').find('[name=Industry]'), resp.data.AllIndustries, 'IndustryId', 'Name');
            if (resp.data.uDetail.Skills && resp.data.uDetail.Skills.SkillSets) {
                $('div#Employment').find('input[type=hidden][name=Skills]').val(resp.data.uDetail.Skills.SkillSets);
            }
            if (resp.data.uDetail.ExperienceDetails && resp.data.uDetail.ExperienceDetails.length > 0) {
                resp.data.uDetail.ExperienceDetails.forEach(function (obj, ind) {

                    for (let key in obj) {
                        let e = $('form#frmExperienceDetails_' + ind).find('[name=' + key + ']');
                        e.eq(0).val(obj[key]);
                    }

                    //Setting salary
                    if (obj.AnnualSalary) {
                        let salary = obj.AnnualSalary.split(' ');
                        let e = $('form#frmExperienceDetails_' + ind).find('[name=AnnualSalaryInLakhs]');
                        e.eq(0).val(salary[0]);
                        let e1 = $('form#frmExperienceDetails_' + ind).find('[name=AnnualSalaryInThousands]');
                        e1.eq(0).val(salary[2]);
                    }

                    //Setting WorkingFrom 
                    if (obj.WorkingFrom) {
                        let workingexp = obj.WorkingFrom.split(',');
                        let e2 = $('form#frmExperienceDetails_' + ind).find('[name=WorkingFromYear]');
                        e2.eq(0).val(workingexp[1].trim());
                        let e3 = $('form#frmExperienceDetails_' + ind).find('[name=WorkingFromMonth]');
                        e3.eq(0).val(Months.indexOf(workingexp[0]));
                    }

                    //Setting the skills
                    if (obj.Skills) {
                        //let e4 = $('form#frmExperienceDetails_' + ind).find('[name=Skills]');
                        //e4.eq(0).val(obj.Skills.SkillSets);
                        $('form#frmExperienceDetails_' + ind).find('[name=Skills]').tagEditor('destroy');

                        $('form#frmExperienceDetails_' + ind).find('[name=Skills]').tagEditor({
                            initialTags: obj.Skills.SkillSets.split(',')
                        });
                        //$('form#frmExperienceDetails_' + ind).find('[name=Skills]').css('border', '1px solid');
                    }
                    if (resp.data.uDetail.ExperienceDetails.length > (ind + 1)) {
                        $('form#frmExperienceDetails_' + ind).find("a#btnAddAnOtherExperience").click();
                    }
                });
            }

            //  Educational Details

            bindDropDownOptions($('form#frmEducationalDetails_0').find('[name=Qualification]'), resp.data.CourseCategories, 'Id', 'Name');
            bindRadioButtons($('form#frmEducationalDetails_0').find('[name=CourseType]'), resp.data.CourseTypes, 'TypeId', 'Type');
            if (resp.data.uDetail.EducationalDetails && resp.data.uDetail.EducationalDetails.length > 0) {
                resp.data.uDetail.EducationalDetails.forEach(function (obj, ind) {
                    for (let key in obj) {
                        let e = $('form#frmEducationalDetails_' + ind).find('[name=' + key + ']');
                        if (e && e.is('input[type=radio]')) {
                            for (let i = 0; i < e.length; i++) {
                                if (e[i].value === obj[key]) {
                                    $(e[i]).prop('checked', true);
                                }
                            }
                        }
                        else {
                            if (key === 'Qualification') {
                                SendAJAXRequest("/ResumeBuilder/GetCourses?cCategory=" + obj[key], "GET", {}, "JSON", function (resp) {
                                    if (resp && resp.isSuccess) {
                                        bindDropDownOptions($('form#frmEducationalDetails_' + ind).find('[name=Course]'), resp.Courses, 'Id', 'Name');
                                        $('form#frmEducationalDetails_' + ind).find('[name=Course]').eq(0).val(obj["Course"]);
                                    }
                                });
                            }
                            e.eq(0).val(obj[key]);
                        }
                    }
                    if (resp.data.uDetail.EducationalDetails.length > (ind + 1)) {
                        $('form#frmEducationalDetails_' + ind).find('[name=Qualification]').change();
                        $('form#frmEducationalDetails_' + ind).find("a#btnAddAnOtherAcademic").click();
                    }
                });
            }
        }
    }
        //,[{ Accept:'application/json;profile="https://en.wikipedia.org/wiki/PascalCase"'}]
    );
})();
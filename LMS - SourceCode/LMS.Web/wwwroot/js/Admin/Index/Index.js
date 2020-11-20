$(document).ready(function () {
    initializeEvents();
    var dateget = new Date();
    var d = new Date(dateget),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    var picdate = [year, month, day].join('-');
    $("#picdate").val(picdate);
    getTilesData(picdate, picdate);
    getGraphData();
});

function getTilesData(sDate, eDate) {
    SendAJAXRequest(`/Dashboard/DashboardData/?date=${sDate}&endDate=${eDate}`, 'GET', {}, 'JSON', (result) => {
        if (result) {
            BindTiles(result.table[0]);
        } else {
            warnignPopup("Unable to get dashboard data");
        }
    });
}

function getGraphData() {
    let year = $('#yearofdata').val();
    let gender = $('#genderApplied').val();
    let state = $('#statewise').val();
    SendAJAXRequest(`/Dashboard/GetGraphData/?year=${year}&gender=${gender}&state=${state}`, 'GET', {}, 'JSON', (result) => {
        if (result) {
            BindCharts(result);
        } else {
            warnignPopup("Unable to get graph data");
        }
    });
}

function getUsers(userType) {
    let startDay = $("#picdate").val();
    let EndDay = $("#endpicdate").val();
    window.open(`${window.location.origin}/Dashboard/GetAllUserRegistrations/?registrationType=${userType}&sDate=${startDay}&eDate=${EndDay}`, "_blank");
}

function initializeEvents() {
    $("#studentdaywise").on("click", function () {
        getUsers("Student");
    });

    $("#Employerdaywise").on("click", function () {
        getUsers("Corporate");
    });

    $("#stuffToday").on("click", function () {
        getUsers("Staffing Partner");
    });

    $("#tpToday").on("click", function () {
        getUsers("Training Partner");
    });

    $("#jobPostDatewise").on("click", function () {
        let startDay = $("#picdate").val();
        let endDay = $("#endpicdate").val();
        window.open(`${window.location.origin}/Dashboard/GetJobsInDateRange/?startDay=${startDay}&endDay=${endDay}`, "_blank");
    });

    $("#resumePost").on("click", function () {
        let startDate = $("#picdate").val();
        let endDate = $("#endpicdate").val();
        window.open(`${window.location.origin}/Dashboard/GetAppliedJobsInRange/?startDate=${startDate}&endDate=${endDate}`, "_blank");
    });

    $("#yearofdata,#genderApplied,#statewise").change(function () {
        getGraphData();
    });
}

function resetGraphsContainer() {
    $("div#jobPostedGraphContainer").find("canvas").remove();
    $("div#userRegistrationGraphContainer").find("canvas").remove();
    $("div#appliedJobsGraphContainer").find("canvas").remove();
    $("div#activJobsGraphContainer").find("canvas").remove();

    $("#noJobsPostedGraphAvailable").hide();
    $("#noUserRegistrationGraphAvailable").hide();
    $("#noAppliedJobsGraphAvailable").hide();
    $("#noActiveJobsGraphAvailable").hide();

    $("div#jobPostedGraphContainer").append($('<canvas id="activeJobs"> </canvas>'));
    $("div#userRegistrationGraphContainer").append($('<canvas id="userRegistrations"> </canvas>'));
    $("div#appliedJobsGraphContainer").append($('<canvas id="appliedJobs"> </canvas>'));
    $("div#activJobsGraphContainer").append($('<canvas id="closedJobs"> </canvas>'));

}

function BindCharts(result) {

    resetGraphsContainer();
    let bgColors = ["#878BB6", "#73162d", "#9939a3", "#4ACAB4", "#c0504d", "#8064a2", "#772c2a", "#f2ab71", "#2ab881", "#4f81bd", "#bfbfbf", "#000066"];
    let labels = Months;

    // Job Post
    if (result.table && result.table.length > 0) {
        let data = new Array();
        labels.forEach(function (l, i) {
            data.push(0);
        });
        result.table.forEach(function (d, i) {
            data[d.month - 1] = d.totalJobPost;
        });
        let jobPostedChart = new Chart(document.getElementById("activeJobs"), {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [
                    {
                        data: data,
                        backgroundColor: bgColors
                    }]
            },
            options: {
                responsive: true,
                legend: {
                    display: false
                },
                scales: {
                    yAxes: [
                        {
                            ticks: {
                                beginAtZero: true,
                                fontStyle: "bold"
                            }
                        }
                    ],
                    xAxes: [
                        {
                            barPercentage: 0.6,
                            ticks: {
                                fontStyle: "bold"
                            }
                        }
                    ]
                },
                onClick: function (e, c) {
                    let year = $('#yearofdata').val();
                    let state = $('#statewise').val();
                    let target = $(e.target);
                    let ele = c[0];
                    let xVal = this.data.labels[ele._index];
                    let yVal = this.data.datasets[0].data[ele._index];
                    window.open(`${window.location.origin}/Dashboard/MonthlyJobs/?month=${Months.indexOf(xVal)+1}&year=${year}&state=${state}`, "_blank");
                }
            }
        });
    }
    else {
        $("#noJobsPostedGraphAvailable").show();
    }

    //  Applied Jobs
    if (result.table1 && result.table1.length > 0) {
        let data = new Array();
        labels.forEach(function (l, i) {
            data.push(0);
        });
        result.table1.forEach(function (d, i) {
            data[d.month - 1] = d.totalJobApplied;
        });
        let appliedJobsChart = new Chart(document.getElementById("appliedJobs"), {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [
                    {
                        data: data,
                        backgroundColor: bgColors
                    }]
            },
            options: {
                responsive: true,
                legend: {
                    display: false
                },
                scales: {
                    yAxes: [
                        {
                            ticks: {
                                beginAtZero: true,
                                fontStyle: "bold"
                            }
                        }
                    ],
                    xAxes: [
                        {
                            barPercentage: 0.6,
                            ticks: {
                                fontStyle: "bold"
                            }
                        }
                    ]
                },
                onClick: function (e, c) {
                    let year = $('#yearofdata').val();
                    let gender = $('#genderApplied').val();
                    let state = $('#statewise').val();

                    let target = $(e.target);
                    let ele = c[0];
                    let xVal = this.data.labels[ele._index];
                    let yVal = this.data.datasets[0].data[ele._index];
                    window.open(`${window.location.origin}/Dashboard/MonthlyAppliedJobs/?month=${Months.indexOf(xVal) + 1}&year=${year}&gender=${gender}&state=${state}`, "_blank");
                }
            }
        });
    }
    else {
        $("#noAppliedJobsGraphAvailable").show();
    }
    //  Active/Close Jobs
    if (result.table2 && result.table2.length > 0) {
        let data = new Array();
        labels.forEach(function (l, i) {
            data.push(0);
        });
        result.table2.forEach(function (d, i) {
            data[d.month - 1] = d.totalActiveClosedJobs;
        });
        
        let lineChartActiveJobsChart = new Chart(document.getElementById("closedJobs"), {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [
                    {
                        data: data,
                        backgroundColor: bgColors
                    }]
            },
            options: {
                responsive: true,
                legend: {
                    display: false
                },
                scales: {
                    yAxes: [
                        {
                            ticks: {
                                beginAtZero: true,
                                fontStyle: "bold"
                            }
                        }
                    ],
                    xAxes: [
                        {
                            barPercentage: 0.6,
                            ticks: {
                                fontStyle: "bold"
                            }
                        }
                    ]
                },
                onClick: function (e, c) {
                    let year = $('#yearofdata').val();
                    let state = $('#statewise').val();

                    let target = $(e.target);
                    let ele = c[0];
                    let xVal = this.data.labels[ele._index];
                    let yVal = this.data.datasets[0].data[ele._index];
                    window.open(`${window.location.origin}/Dashboard/MonthlyJobs/?month=${Months.indexOf(xVal) + 1}&year=${year}&state=${state}&activeJobs=false`, "_blank");
                }
            }
        });
    }
    else {
        $("#noActiveJobsGraphAvailable").show();
    }

    //  Monthly User Registration
    if (result.table3 && result.table3.length > 0) {
        let data = new Array();
        labels.forEach(function (l, i) {
            data.push(0);
        });
        result.table3.forEach(function (d, i) {
            data[d.month - 1] = d.totalRegistration;
        });
        
        let uRegistrationsChart = new Chart(document.getElementById("userRegistrations"), {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [
                    {
                        data: data,
                        backgroundColor: bgColors
                    }]
            },
            options: {
                responsive: true,
                legend: {
                    display: false
                },
                scales: {
                    yAxes: [
                        {
                            ticks: {
                                beginAtZero: true,
                                fontStyle: "bold"
                            }
                        }
                    ],
                    xAxes: [
                        {
                            barPercentage: 0.6,
                            ticks: {
                                fontStyle: "bold"
                            }
                        }
                    ]
                },
                onClick: function (e, c) {
                    let year = $('#yearofdata').val();
                    let state = $('#statewise').val();
                    let gender = $('#genderApplied').val();
                    let target = $(e.target);
                    let ele = c[0];
                    let xVal = this.data.labels[ele._index];
                    let yVal = this.data.datasets[0].data[ele._index];
                    window.open(`${window.location.origin}/Dashboard/MonthlyRegisteredUsers/?month=${Months.indexOf(xVal) + 1}&year=${year}&state=${state}&gender=${gender}`, "_blank");
                }
            }
        });
    }
    else {
        $("#noUserRegistrationGraphAvailable").show();
    }
}

function BindTiles(result) {
    $('#lblEmpRegistration').text(result.totalEmployeer);
    $('#lblJobPost').text(result.totalJobPost);
    $('#lblResumePost').text(result.totalResumePost);
    $('#lblStuffingPartner').text(result.stuffingPartner);
    $('#lblTraningPartner').text(result.traningPartner);
    $('#lblStudentRegistration').text(result.totalStudent);
}

function getDateRange() {
    let picdate = $("#picdate").val();
    let endpicdate = $("#endpicdate").val();
    getTilesData(picdate, endpicdate);
}
$("selct#yearofdata .chosen-select-no-single").trigger(function () {
    alert('data');
});
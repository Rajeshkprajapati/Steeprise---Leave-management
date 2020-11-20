//  Refere this:    https://www.sitepoint.com/introduction-chart-js-2-0-six-examples/

let dashboard = {};

dashboard.demandAggregation = (function () {

    let graphs = {};

    let globalFilter = {};

    let initialize = function () {

        multiselector.initSelector(
            $('select#ddlDemandByEmployer'),
            {
                nonSelectedText: 'Select Employer',
                allSelectedText: "All Employer Selected",
                maxSelection: 5,
                onDropdownHidden: function (evt) {
                    globalFilter.selectedJobEmployers = $("input[type=hidden]#hdnDemandByEmployer").val();
                    loadAllGraphs();
                },
                onSelectAll: function () {
                    $("input[type=hidden]#hdnDemandByEmployer").val('');
                }
            },
            $("input[type=hidden]#hdnDemandByEmployer"),
            ","
        );

        multiselector.initSelector(
            $('select#ddlDemandByJobRoles'),
            {
                nonSelectedText: 'Select Job Role',
                allSelectedText: "All Job Role Selected",
                maxSelection: 5,
                onDropdownHidden: function (evt) {
                    globalFilter.selectedJobRoles = $("input[type=hidden]#hdnDemandByJobRoles").val();
                    loadAllGraphs();
                },
                onSelectAll: function () {
                    $("input[type=hidden]#hdnDemandByJobRoles").val('');
                }
            },
            $("input[type=hidden]#hdnDemandByJobRoles"),
            ","
        );

        multiselector.initSelector(
            $('select#ddlDemandByStates'),
            {
                nonSelectedText: 'Select State',
                allSelectedText: "All States Selected",
                maxSelection: 5,
                onDropdownHidden: function (evt) {
                    globalFilter.selectedJobStates = $("input[type=hidden]#hdnDemandByStates").val();
                    loadAllGraphs();
                },
                onSelectAll: function () {
                    $("input[type=hidden]#hdnDemandByStates").val('');
                }
            },
            $("input[type=hidden]#hdnDemandByStates"),
            ","
        );

        $("button.multiselect-clear-filter").click(function () {
            let selector = $(this).parent().parent().parent().parent().parent().parent().find('select');
            if (selector && selector.length > 0) {
                $(selector).multiselect("clearSelection");
                switch (selector.attr('id')) {
                    case 'ddlDemandByStates':
                        $("input[type=hidden]#hdnDemandByStates").val('');
                        break;
                    case 'ddlDemandByJobRoles':
                        $("input[type=hidden]#hdnDemandByJobRoles").val('');
                        break;
                    case 'ddlDemandByEmployer':
                        $("input[type=hidden]#hdnDemandByEmployer").val('');
                        break;
                    default:
                        break;
                }
            }
        });


        loadAllGraphs();
    };

    let demandAggregationDataOnQuarter = function () {
        resetGraphContainer($("canvas#demandByQuarterBar").eq(0)[0]);
        SendAJAXRequest(`/Dashboard/GetDemandAggregationDataOnQuarter?Employers=${globalFilter.selectedJobEmployers}&FinancialYear=${globalFilter.selectedYear}&UserRole=${globalFilter.selectedRole}&JobRoles=${globalFilter.selectedJobRoles}&JobStates=${globalFilter.selectedJobStates}`, "GET", {}, "json", function (resp) {
            if (resp && resp.isSuccess) {
                manipulateDemandAggregationGraphOnQuarter(resp.data[0]);
            }
            else {
                return false;
            }
        });
    };

    let demandAggregationDataOnJobRole = function () {
        resetGraphContainer($("canvas#demandByJobRolesBar").eq(0)[0]);
        SendAJAXRequest(`/Dashboard/GetDemandAggregationDataOnJobRole?Employers=${globalFilter.selectedJobEmployers}&FinancialYear=${globalFilter.selectedYear}&UserRole=${globalFilter.selectedRole}&JobRoles=${globalFilter.selectedJobRoles}&JobStates=${globalFilter.selectedJobStates}`, "GET", {}, "json", function (resp) {
            if (resp && resp.isSuccess) {
                manipulateDemandAggregationGraphOnJobRoles(resp.data);
            }
            else {
                return false;
            }
        });
    };

    let demandAggregationDataOnState = function () {
        resetGraphContainer($("canvas#demandByStatesBar").eq(0)[0]);
        SendAJAXRequest(`/Dashboard/GetDemandAggregationDataOnState?Employers=${globalFilter.selectedJobEmployers}&FinancialYear=${globalFilter.selectedYear}&UserRole=${globalFilter.selectedRole}&JobRoles=${globalFilter.selectedJobRoles}&JobStates=${globalFilter.selectedJobStates}`, "GET", {}, "json", function (resp) {
            if (resp && resp.isSuccess) {
                manipulateDemandAggregationGraphOnStates(resp.data);
            }
            else {
                return false;
            }
        });
    };

    let demandAggregationDataOnEmployer = function () {
        resetGraphContainer($("canvas#demandByEmployersBar").eq(0)[0]);
        SendAJAXRequest(`/Dashboard/GetDemandAggregationDataOnEmployer?Employers=${globalFilter.selectedJobEmployers}&FinancialYear=${globalFilter.selectedYear}&UserRole=${globalFilter.selectedRole}&JobRoles=${globalFilter.selectedJobRoles}&JobStates=${globalFilter.selectedJobStates}`, "GET", {}, "json", function (resp) {
            if (resp && resp.isSuccess) {
                manipulateDemandAggregationGraphOnEmployer(resp.data);
            }
            else {
                return false;
            }
        });
    };

    let manipulateDemandAggregationGraphOnQuarter = function (response) {
        let _container = $("canvas#demandByQuarterBar");
        if (response) {
            let type = "bar";
            let container = _container.eq(0)[0].getContext("2d");
            let labels = new Array();
            for (let key in response.DemandAggregations) {
                labels.push(key);
            }
            let data = new Array();
            let dataItem = new Object();
            let cIndex = -1;
            dataItem.data = new Array();
            dataItem.backgroundColor = new Array();
            //dataItem.label = response.Year;
            for (let key of labels) {
                dataItem.data.push(response.DemandAggregations[key]);
                cIndex++;
                if (cIndex >= Colors.length) {
                    cIndex = 0;
                }
                dataItem.backgroundColor.push(Colors[cIndex]);
            }
            data.push(dataItem);
            graphs[$(_container).attr("id")] = drawGraph(container, labels, data, type);
        }
        else {
            showNoDataMessage(_container);
        }
    };

    let manipulateDemandAggregationGraphOnJobRoles = function (response) {
        let _container = $("canvas#demandByJobRolesBar");
        if (response && response.length > 0) {
            let type = "bar";
            let container = _container.eq(0)[0].getContext("2d");
            let labels = new Array();
            for (let key in response[0].DemandAggregations) {
                labels.push(key);
            }
            let data = new Array();
            response.forEach(function (resp, i) {
                let dataItem = new Object();
                let cIndex = -1;
                dataItem.data = new Array();
                dataItem.backgroundColor = new Array();
                dataItem.label = resp.JobRole;
                for (let key of labels) {
                    dataItem.data.push(resp.DemandAggregations[key]);
                    cIndex++;
                    if (cIndex >= Colors.length) {
                        cIndex = 0;
                    }
                    dataItem.backgroundColor.push(Colors[cIndex]);
                }
                data.push(dataItem);
            });

            graphs[$(_container).attr("id")] = drawGraph(container, labels, data, type);
        }
        else {
            showNoDataMessage(_container);
        }
    };

    let manipulateDemandAggregationGraphOnStates = function (response) {
        let _container = $("canvas#demandByStatesBar");
        if (response && response.length > 0) {
            let type = "bar";
            let container = _container.eq(0)[0].getContext("2d");
            let labels = new Array();
            for (let key in response[0].DemandAggregations) {
                labels.push(key);
            }
            let data = new Array();
            response.forEach(function (resp, i) {
                let dataItem = new Object();
                let cIndex = -1;
                dataItem.data = new Array();
                dataItem.backgroundColor = new Array();
                dataItem.label = resp.State;
                for (let key of labels) {
                    dataItem.data.push(resp.DemandAggregations[key]);
                    cIndex++;
                    if (cIndex >= Colors.length) {
                        cIndex = 0;
                    }
                    dataItem.backgroundColor.push(Colors[cIndex]);
                }
                data.push(dataItem);
            });
            graphs[$(_container).attr("id")] = drawGraph(container, labels, data, type);
        }
        else {
            showNoDataMessage(_container);
        }
    };

    let manipulateDemandAggregationGraphOnEmployer = function (response) {
        let _container = $("canvas#demandByEmployersBar");
        if (response && response.length > 0) {
            let type = "bar";
            let container = _container.eq(0)[0].getContext("2d");
            let labels = new Array();
            for (let key in response[0].DemandAggregations) {
                labels.push(key);
            }
            let data = new Array();
            response.forEach(function (resp, i) {
                let dataItem = new Object();
                let cIndex = -1;
                dataItem.data = new Array();
                dataItem.backgroundColor = new Array();
                dataItem.label = resp.Company;
                for (let key of labels) {
                    dataItem.data.push(resp.DemandAggregations[key]);
                    cIndex++;
                    if (cIndex >= Colors.length) {
                        cIndex = 0;
                    }
                    dataItem.backgroundColor.push(Colors[cIndex]);
                }
                data.push(dataItem);
            });
            graphs[$(_container).attr("id")] = drawGraph(container, labels, data, type);
        }
        else {
            showNoDataMessage(_container);
        }
    };

    let drawGraph = function (container, labels, data, gType) {

        displayColorInformation(labels, data);
        return new Chart(container, {
            type: gType,
            data: {
                labels: labels,
                datasets: data
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
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
                tooltips: {
                    callbacks: {

                    }
                },
                onClick: function (e, c) {
                    let target = $(e.target);
                    let ele = this.getElementAtEvent(e);
                    let label = null;
                    let dataSetLabel = null;
                    let dataSetValue = 0;
                    if (ele && ele.length > 0) {
                        label = this.data.labels[ele[0]._index];
                        dataSetLabel = this.data.datasets[ele[0]._datasetIndex].label;
                        dataSetLabel = dataSetLabel ? dataSetLabel : label;
                        dataSetValue = this.data.datasets[ele[0]._datasetIndex].data[ele[0]._index];
                    }
                    let onBasis = target.attr("data-forWhichBasis");
                    window.open(`${window.location.origin}/Dashboard/ViewDemandAggregationDetails/?onBasis=${onBasis}&value=${dataSetLabel}&Employers=${globalFilter.selectedJobEmployers}&FinancialYear=${globalFilter.selectedYear}&UserRole=${globalFilter.selectedRole}&JobRoles=${globalFilter.selectedJobRoles}&JobStates=${globalFilter.selectedJobStates}`, "_blank");
                }
            }
        });
    };

    let displayColorInformation = function (labels, dataSets) {
        if ($("div.demand-aggregation-colors-info").find("span").length < 1) {
            let colorSContainer = $('<span>');
            labels.forEach(function (l, i) {

                switch (l) {
                    case "Q1":
                        l += " (AMJ)";
                        break;
                    case "Q2":
                        l += " (JAS)";
                        break;
                    case "Q3":
                        l += " (OND)";
                        break;
                    case "Q4":
                        l += " (JFM)";
                        break;
                    default:
                        break;
                }

                let ele = $("<i>");
                ele.addClass("fa fa-circle");
                ele.attr("aria-hidden", "true");
                ele.css("color", dataSets[0].backgroundColor[i]);
                let textEle = $("<label>");
                textEle.css("color", "black");
                textEle.text(l);
                ele.append(textEle);
                colorSContainer.append(ele);
            });
            $("div.demand-aggregation-colors-info").append(colorSContainer);
        }
    };

    let resetGraphContainer = function (container) {
        let containerPersistAttributes = {
            id: $(container).attr("id"),
            "data-forWhichBasis": $(container).attr("data-forWhichBasis")
        };
        let parent = $(container).parent();
        parent.find("label.error-class").css("display", "none");
        $(container).remove();
        let canvas = $("<canvas>");
        for (let key in containerPersistAttributes) {
            canvas.attr(key, containerPersistAttributes[key]);
        }
        parent.append(canvas);
    };

    let showNoDataMessage = function (container) {
        container.parent().find("label.error-class").css("display", "block");
        container.parent().find("label.error-class").text("No data available to draw a graph");
        container.css("display", "none");
    };

    let resolveFilters = function () {
        if (!globalFilter.selectedYear) {
            globalFilter.selectedYear = $("select#ddlFinancialYear").val();
        }
        if (!globalFilter.selectedRole) {
            globalFilter.selectedRole = $("select#ddlUserRole").val();
        }
        if (!globalFilter.selectedJobEmployers) {
            globalFilter.selectedJobEmployers = $("input[type=hidden]#hdnDemandByEmployer").val();
        }
        if (!globalFilter.selectedJobRoles) {
            globalFilter.selectedJobRoles = $("input[type=hidden]#hdnDemandByJobRoles").val();
        }
        if (!globalFilter.selectedJobStates) {
            globalFilter.selectedJobStates = $("input[type=hidden]#hdnDemandByStates").val();
        }
    };

    let loadAllGraphs = function () {
        resolveFilters();
        demandAggregationDataOnJobRole();
        demandAggregationDataOnState();
        demandAggregationDataOnQuarter();
        demandAggregationDataOnEmployer();
    };

    return {
        demandAggregationDataOnJobRole: demandAggregationDataOnJobRole,
        demandAggregationDataOnState: demandAggregationDataOnState,
        demandAggregationDataOnEmployer: demandAggregationDataOnEmployer,
        initialize: initialize,
        globalFilter: globalFilter,
        loadAllGraphs: loadAllGraphs
    };
})();



function yearChanged(_this) {
    dashboard.demandAggregation.globalFilter.selectedYear = _this.value;
    dashboard.demandAggregation.loadAllGraphs();
}

function roleChangedToFilterGraph(_this) {
    dashboard.demandAggregation.globalFilter.selectedRole = _this.value;
    dashboard.demandAggregation.loadAllGraphs();
}

function exportDemandAggregationReport(_this) {
    let selYear = dashboard.demandAggregation.globalFilter.selectedYear;
    let selRole = dashboard.demandAggregation.globalFilter.selectedRole;
    let selStates = dashboard.demandAggregation.globalFilter.selectedJobStates;
    let selJRoles = dashboard.demandAggregation.globalFilter.selectedJobRoles;
    let selEmployers = dashboard.demandAggregation.globalFilter.selectedJobEmployers;
    _this.href = `\DownloadDemandAggregation\?Employers=${selEmployers}&FinancialYear=${selYear}&UserRole=${selRole}&JobRoles=${selJRoles}&JobStates=${selStates}`;
    return true;
}

$(function () {
    dashboard.demandAggregation.initialize();
});
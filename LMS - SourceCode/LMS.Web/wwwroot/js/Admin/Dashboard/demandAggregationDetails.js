let demandAggregationDetails = {};

demandAggregationDetails = (function () {
    let initiateDataTables = function () {
        $('table#tableDemandAggregation').dataTable({          
            lengthChange: false,
            "order": [],
            columnDefs: [
                { "searchable": false, "targets": 7 },
                { "searchable": false, "targets": 8 },
                { "searchable": false, "targets": 9 },
                { "searchable": false, "targets": 10 },
                { "searchable": false, "targets": 11}
            ]
        });
    };

    initiateDataTables();
    return {
        //  Nothing to return
    };
})();
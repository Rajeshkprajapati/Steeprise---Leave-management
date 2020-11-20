
$(document).ready(function () {
    $('#dataTable').DataTable({
        "order": [[6, "desc"]],
        "processing": true,
        "serverSide": true,
        "searching": true,
        "paging": true,
        "ajax": {
            url: '/PlacedCandidate/GetFilteredItems',
            pages: 5 //number of pages to cache
        },
        "columns": [
            { "data": "candidateID", "name": "CandidateID"},
            { "data": "candidateName", "name": "CandidateName"},
            { "data": "candidateEmail", "name": "CandidateEmail"},
            { "data": "castecategory", "name": "Castecategory"},
            { "data": "educationAttained", "name": "EducationAttained"},
            { "data": "employerspocEmail", "name": "EmployerspocEmail"},
            // For Student.CreatedDate
            {
                "data": "createdDate", "name":"CreatedDate",
                "render": function (data) {
                    var date = new Date(data);
                    //return date.toLocaleString();
                    return date.toGMTString().slice(' ', 25);
                }
            }
        ],
        language: {
            processing: `<div id="loader" hidden>< img id="imgloader" src="~/images/ajax-loader1.gif" /></div>`,
            zeroRecords: "No matching records found"
        },
        //"bPaginate": false,
        "bLengthChange": false,
        //"bFilter": true,
        //"bInfo": false,
        //"bAutoWidth": false
    });
        
    
});
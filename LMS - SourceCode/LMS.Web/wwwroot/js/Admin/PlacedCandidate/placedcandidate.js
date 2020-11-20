
let placedCandidate = (function validateUploadedFiles(_this) {

    let validateUploadedFiles = function (_this) {
        let fileExtensions = ['xls', 'xlsx'];
        let files = $(_this).find(("input[type=file]"));
        for (let i = 0; i < files.length; i++) {
            let file = files[i];
            let fileName = file.value;
            if (fileName.length === 0) {
                $('#success').hide();
                $('#message').text('Please select a file');
                return false;
            }
            else {
                var extension = fileName.replace(/^.*\./, '');
                if ($.inArray(extension, fileExtensions) === -1) {
                    $('#success').hide();
                    $('#message').text('Please select only excel files.');
                    return false;
                }
            }
        }
        return true;
    };

    return {
        validateUploadedFiles: validateUploadedFiles
    };
})();

function uploadCandidate(_this) {

    if (!placedCandidate.validateUploadedFiles(_this)) {
        return false;
    }
}

function fileSelected(_this) {
    $('#fileName').text(_this.files[0].name);
}

$(function () {
    //$('#dataTable').dataTable({
    //    //'columnDefs': [{ "searchable": false, 'orderable': false, 'targets': 5 }],        
    //    "order": [[11, "desc"]]
    //});
});
let bulkJobSeekerUpload  = (function () {

    let validateUploadedFiles = function (_this) {
        let fileExtensions = ['xls', 'xlsx'];
        let files = $(_this).find(("input[type=file]"));
        for (let i = 0; i < files.length; i++) {
            let file = files[i];
            let fileName = file.value;
            if (fileName.length === 0) {
                $('#message').text('Please select a file');
                return false;
            }
            else {
                var extension = fileName.replace(/^.*\./, '');
                if ($.inArray(extension, fileExtensions) === -1) {
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

function uploadJobSeekers(_this) {

    /*  Checking only validation if we use form submit to Server Side instead of ajax call, need to comment this
    "if" block and uncomment below "If" block, if we need to upload a file via ajax call. */

    if (!bulkJobSeekerUpload.validateUploadedFiles(_this)) {
        return false;
    }
    //if (bulkJobPost.expose.validateUploadedFiles()) {
    //    bulkJobPost.expose.uploadJobPostingFile();
    //}
}

function fileSelected(_this) {
    $('#fileName').text(_this.files[0].name);
}


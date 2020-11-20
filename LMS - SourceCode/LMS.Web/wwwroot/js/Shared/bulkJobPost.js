/*  Jquery selector example in some special cases;

1. For tag :    <div id = 'element_123_wrapper_text'> My sample DIV</div >
    
    1.1 The Operator ^ - Match elements that starts with given value
        div[id^="element_123"] {

        }

    1.2 The Operator $ - Match elements that ends with given value
        div[id$="wrapper_text"] {

        }

    1.3 The Operator * - Match elements that have an attribute containing a given value
        div[id*="wrapper_text"] {

        }

*/

let bulkJobPost = {};

function uploadJobs(_this) {

    /*  Checking only validation if we use form submit to Server Side instead of ajax call, need to comment this
    "if" block and uncomment below "If" block, if we need to upload a file via ajax call. */

    if (!bulkJobPost.expose.validateUploadedFiles(_this)) {
        return false;
    }
    //if (bulkJobPost.expose.validateUploadedFiles()) {
    //    bulkJobPost.expose.uploadJobPostingFile();
    //}
}

function fileSelected(_this) {
    $('#fileName').text(_this.files[0].name);
}

bulkJobPost.expose = (function () {

    let init = function () {
        $("input[type=checkbox][name=inBackground]").change(function () {
            if (this.checked) {
                this.value = true;
            }
            else {
                this.value = false;
            }
        });
    };

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

    let uploadJobPostingFile = function () {
        let files = $("input[type=file]");
        files.each(function (i, file) {
            let fData = new FormData();
            let _file = file.files[0];
            fData.append(_file.name, _file);
            //  Need to make ajax call here to upload a file.
        });
    };

    return {
        init: init,
        validateUploadedFiles: validateUploadedFiles,
        uploadJobPostingFile: uploadJobPostingFile
    };
})();

$(document).ready(function () {
    bulkJobPost.expose.init();
});

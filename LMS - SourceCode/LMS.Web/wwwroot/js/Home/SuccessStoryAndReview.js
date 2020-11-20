$("#ImgUrl").change(function () {
    $("#profilelabel").text(this.files[0].name);
});
$("#areaCommet").on('keyup', function () {
    var words = this.value.match(/\S+/g).length;
    if (words > 100) {
        var trimmed = $(this).val().split(/\s+/, 100).join(" ");
        $(this).val(trimmed + " ");
    }
    else {
        //$('#display_count').text(words);
        //$('#word_left').text(100 - words);
    }
});
$("#txtTagline").on('keyup', function () {
    var words = this.value.match(/\S+/g).length;

    if (words > 30) {
        var trimmed = $(this).val().split(/\s+/, 30).join(" ");
        $(this).val(trimmed + " ");
    }
});
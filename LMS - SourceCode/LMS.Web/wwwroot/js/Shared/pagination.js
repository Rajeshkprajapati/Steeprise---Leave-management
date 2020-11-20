$(document).ready(function () {
    //pageSize = 3;
    //incremSlide = 3;
    //startPage = 0;
    //numberPage = 0;

    var pageCount = $(".listing").length / pageSize;
    var totalSlidepPage = Math.floor(pageCount / incremSlide);

    for (var i = 0; i < pageCount; i++) {
        $("#pagin").append('<li><a>' + (i + 1) + '</a></li> ');
        if (i > pageSize) {
            $("#pagin li").eq(i).hide();
        }
    }

    var prev = $('.prev').click(function () {
        startPage -= 3;
        incremSlide -= 3;
        numberPage--;
        slide();
    });

    prev.hide();

    var next = $('.next').click(function () {
        startPage += 3;
        incremSlide += 3;
        numberPage++;
        slide();
    });
    $("#pagin li").first().find("a").addClass("current-page");

    slide = function (sens) {
        $("#pagin li").show();

        for (t = startPage; t < incremSlide; t++) {
            $("#pagin li").eq(t + 1).show();
        }
        if (startPage == 0) {
            next.show();
            prev.hide();
        } else if (numberPage == totalSlidepPage) {
            next.hide();
            prev.show();
        } else {
            next.show();
            prev.show();
        }


    }

    showPage = function (page) {
        $(".listing").hide();
        $(".listing").each(function (n) {
            if (n >= pageSize * (page - 1) && n < pageSize * page)
                $(this).show();
        });
    }

    showPage(1);
    $("#pagin li a").eq(0).addClass("current-page");

    $("#pagin li a").click(function () {
        $("#pagin li a").removeClass("current-page");
        $(this).addClass("current-page");
        showPage(parseInt($(this).text()));
    });
});
$(document).ready(function () {
    $("select[name=minExp]").change(function () {
        let minExp = parseInt(this.value);
        $("select[name=maxExp] option").each(function (i, o) {
            if (parseInt(o.value) < minExp) {
                $(o).prop("disabled", true);
            }
            else {
                $(o).removeAttr("disabled");
            }
        });
    });

    
    

    //multiselector.initSelector(
    //    $('select#ddlJobRoles'),
    //    {
    //        nonSelectedText: 'Select job role',
    //    },
    //    $("input[type=hidden]#hdnJobRoleIds"),
    //    ","
    //);

    $('.slidepartners').slick({
        slidesToShow: 1,
        slidesToScroll: 6,
        autoplay: true,
        autoplaySpeed: 2500,
        arrows: false,
        dots: false,
        //cssEase: 'linear',
        pauseOnHover: true,
        responsive: [{
            breakpoint: 768,
            settings: {
                slidesToShow: 1
            }
        }, {
            breakpoint: 520,
            settings: {
                slidesToShow: 1
            }
        }]
    });
});

function EmployerFollower(id) {
    if (id === 0) {
        ErrorDialog("Login Required","Please login or regiseter to follow company");
        return false;
    }
    else {
        var data = "";
        SendAJAXRequest("/Home/EmployerFollower/?EmployerId=" + id + "", 'POST', data, 'JSON', function (result) {
            if (result) {
                InformationDialog('Information', 'Successfully done');
                location.reload(true);
            } else {
                ErrorDialog('Error','Please try again');
            }
        });
    }
}

var slideIndex = 0;
var intraval = 2000;
showSlides();
function showSlides() {
        var i;
        var slides = document.getElementsByClassName("mySlides");
        var dots = document.getElementsByClassName("mySlides");
        for(i = 0; i < slides.length; i++) {
            slides[i].style.display = "none";
         }
        slideIndex++;
        if (slideIndex > slides.length) { slideIndex = 1 }
        for (i = 0; i < dots.length; i++) {
            dots[i].className = dots[i].className.replace(" active", "");
        }
        slides[slideIndex - 1].style.display = "";
        dots[slideIndex - 1].className += " active";
       
        setTimeout(showSlides, intraval);
}
$('#myslideUl').hover(function () {
    intraval = 10000;
});

$('#myslideUl').mouseout(function () {
   intraval = 2000;
});
// Write your Javascript code.



$(document).ready(function () {

    $("#LightBoxBackCH1").click(function () {
        LightBoxOff();
    });

});



function LightBoxOn()
{
    $("#LightBoxBackCH1").css("width", $(window).width() + "px");
    $("#LightBoxBackCH1").css("height", $(window).height() + "px");

    $("#LightBoxBackCH1").css("top", "0px");
    $("#LightBoxBackCH1").css("left", "0px");

    $("body").css("overflow", "hidden");
    $("#LightBoxBackCH1").css("display", "block");
}


function LightBoxOff() {
    $("#LightBoxBackCH1").css("width", "0px");
    $("#LightBoxBackCH1").css("height", "0px");

    $("#LightBoxBackCH1").css("top", "-200px");
    $("#LightBoxBackCH1").css("left", "-200px");

    $("body").css("overflow", "");
    $("#LightBoxBackCH1").css("display", "none");

    $("#LightBoxCH1").html("");
}


function LightBoxContent(data_content)
{
    $("#LightBoxCH1").html(data_content);
}





function PreloaderOn(selector)
{
    $(selector).html("<img alt='' src='/images/preloader2.gif' style='width:10%; margin:auto; margin:20px 0 0 45%;' />");
}

function PreloaderOff() {
}




function NotAuthorized() {
    window.location = "/Account/Login";
}


function ProccessErrorReq(req) {

    //if not authorized go to login
    if (req.status === 401) {
        window.location = "/Account/Login";
    }

}


function shuffle(a) {
    var j, x, i;
    for (i = a.length - 1; i > 0; i--) {
        j = Math.floor(Math.random() * (i + 1));
        x = a[i];
        a[i] = a[j];
        a[j] = x;
    }
    return a;
}

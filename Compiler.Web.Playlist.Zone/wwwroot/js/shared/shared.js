

$(document).ready(function () {

    InitSearchBar("#SearchBarTxt");


    if (window.location.hash.length > 0) {
        var hashStr = window.location.hash.replace("#", "/");

        PreloaderOn(".main-content");
        $.get(hashStr, function (data) {
            $(".main-content").html(data);
            PreloaderOff();
        });
    }
    else {
        PreloaderOn(".main-content");
        $.get("/Home/Start", function (data) {
            $(".main-content").html(data);
            PreloaderOff();
        });
    }

    WindowResize();
});


$(window).resize(function () {
    WindowResize();
});






/* ----------- MENU ------------------------- */


//!!!menu
$(".mmHome").on("click", function () {

    $(".navbar-collapse").removeClass("in");

    PreloaderOn(".main-content");
    $.get('/Home/Start', function (data) {

        window.location.hash = "";
        $(".main-content").html(data);
        PreloaderOff();
    });
});


//!!!menu
$(".mmSearch").on("click", function () {

    $(".navbar-collapse").removeClass("in");

    PreloaderOn(".main-content");
    $.get('/Search/Index', function (data) {

        window.location.hash = "search";
        $(".main-content").html(data);
        PreloaderOff();
    });
});

//!!!menu
$(".mmCharts").on("click", function () {

    $(".navbar-collapse").removeClass("in");

    PreloaderOn(".main-content");
    $.get('/Charts/Index', function (data) {

        window.location.hash = "charts";
        $(".main-content").html(data);
        PreloaderOff();
    });
});



//!!!menu
$(".mmPlaylists").on("click", function () {

    $(".navbar-collapse").removeClass("in");

    PreloaderOn(".main-content");

    $.ajax({
        type: "GET",
        url: '/Playlists/Index',
        contentType: "application/html; charset=utf-8",
        data: {},
        timeout: 1000,
        dataType: "html",
        success: function (result) {
            window.location.hash = "playlists";
            $(".main-content").html(result);
            PreloaderOff();
        },
        error: function (result) { ProccessErrorReq(result); }
    });

});


//!!!menu
$(".mmTags").on("click", function () {

    $(".navbar-collapse").removeClass("in");

    PreloaderOn(".main-content");
    $.get('/Tags/Index', function (data) {

        window.location.hash = "tags";
        $(".main-content").html(data);
        PreloaderOff();
    });
});


//!!!menu
$(".mmAccount").on("click", function () {

    $(".navbar-collapse").removeClass("in");

    $.get('/Account/Info', function (data) {

        window.location.hash = "account";
        $(".main-content").html(data);
        PreloaderOff();
    });
});


//!!!menu
$(".mmSignOut").on("click", function () {
    window.location = "/Account/SignOut";
});



/*------- MENU --------------------------------*/



function WindowResize() {

    $(".player-mch").css("top", ($(window).height() - $(".player-mch").height() - 35) + "px");
    $(".player-mch").css("left", ($(window).width() - $(".player-mch").width() - 50) + "px");
    $(".main-content").css("height", ($(window).height() - 85) + "px");


    //determine if player is big
    var isBig = true;
    if ($(".player-mch").width() > 500) {
        isBig = true;
    }
    else {
        isBig = false;
    }

    //if player is big - then check if window is became small to shring player
    if (isBig) {
        if ($(window).width() < 600) {
            $(".player-mch .minimize-btn").text("-");
            $(".player-mch .minimize-btn").trigger("click");
        }
        else {
            $(".player-mch .minimize-btn").text("+");
            $(".player-mch .minimize-btn").trigger("click");
        }
    }
}
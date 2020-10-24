

$(document).ready(function () {

    
    $(".player-mch .close-btn").on("click", function () {
        ClosePlayer();
    });


    $(".player-mch .minimize-btn").on("click", function () {

        if ($(this).text() == "-") {

            $(this).text("+");
            $(".player-mch .player-content").css("display", "none");
            $(".player-mch").css("width", "320px");
            $(".player-mch").css("height", "280px");

            $("#playerCH1").css("width", "300px");

            $("#player").css("width", "300px");
            $("#player").css("height", "200px");


            var TopOffset = ($(document).scrollTop() + $(window).height() - $(".player-mch").height() - 40);
            var LeftOffset = ($(window).width() - $(".player-mch").width() - 50);
            $(".player-mch").css("top", TopOffset + "px");
            $(".player-mch").css("left", LeftOffset + "px");
        }
        else {
            $(this).text("-");

            $("#playerCH1").css("width", "360px");

            $("#player").css("width", "360px");
            $("#player").css("height", "260px");

            $(".player-mch .player-content").css("display", "block");
            $(".player-mch").css("width", "650px");
            $(".player-mch").css("height", "340px");


            var TopOffset = ($(document).scrollTop() + $(window).height() - $(".player-mch").height() - 40);
            var LeftOffset = ($(window).width() - $(".player-mch").width() - 50);
            $(".player-mch").css("top", TopOffset + "px");
            $(".player-mch").css("left", LeftOffset + "px");
        }

    });
    



    $(".plr-btn-next").on("click", function () {
        playNext();
    });

    $(".plr-btn-prev").on("click", function () {
        playPrev();
    });

    $(".plr-btn-play").on("click", function () {
        pauseVideo();
    });

    $(".plr-btn-shuffle").on("click", function () {
        shufflePlaylist();
    });


    $(".plst-Itm").click(function () {
        playSong($(this).attr("data-id"), $(this).attr("data-song"));

        $(".plst-Itm").css("background-color", "#dedede");
        $(this).css("background-color", "#59caf1");
    });


    $(".player-mch").draggable();
});



$(document).scroll(function () {
    var TopOffset = ($(this).scrollTop() + $(window).height() - $(".player-mch").height() - 30);
    var LeftOffset = ($(window).width() - $(".player-mch").width() - 50);

    $(".player-mch").css("top", TopOffset + "px");
    $(".player-mch").css("left", LeftOffset + "px");
});






//-----------------------------------------------------------------
// PLAYBACK
//-----------------------------------------------------------------

var CURR_SONG = "";
var CURR_STATUS = "";

// 2. This code loads the IFrame Player API code asynchronously.
var tag = document.createElement('script');

tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);



// 3. This function creates an <iframe> (and YouTube player)
//    after the API code downloads.
var player;
function onYouTubeIframeAPIReady() {

    player = new YT.Player('player', {
        height: '330',
        width: '540',
        videoId: '',
        playerVars: { 'autoplay': 0, 'controls': 0 },
        events: {
            'onReady': onPlayerReady,
            'onStateChange': onPlayerStateChange
        }
    });
}


// 4. The API will call this function when the video player is ready.
function onPlayerReady(event) {
    event.target.playVideo();
}


// 5. The API calls this function when the player's state changes.
//    The function indicates that when playing a video (state=1),
//    the player should play for six seconds and then stop.
var done = false;
function onPlayerStateChange(event) {

    //console.log("event.data -->" + event.data);
    if (event.data == 0) {
        playNext();
    }

    if (event.data == YT.PlayerState.PLAYING && !done) {
        //setTimeout(stopVideo, 6000);
        done = true;
    }
}


function stopVideo() {
    player.stopVideo();
    CURR_STATUS = "STOP";
}

function pauseVideo() {
    if (CURR_STATUS == "PLAY") {
        $(".plr-btn-play").html("play");
        player.pauseVideo();
        CURR_STATUS = "PAUSE";
    }
    else {
        $(".plr-btn-play").html("pause");
        player.playVideo();
        CURR_STATUS = "PLAY";
    }
}


function playSong(song_id, song_guid) {
    CURR_SONG = song_id;
    player.loadVideoById(song_guid, 0, 100);
    CURR_STATUS = "PLAY";
}


//try to call child if applicable
function playNext() {
    try {
        playNextSong();
    }
    catch (ex) {
        console.log(ex);
    }
}


//try to call child if applicable
function playPrev() {
    try {
        playPrevSong();
    }
    catch (ex) {
        console.log(ex);
    }
}



function shufflePlaylist() {


    var plstItems = new Array();

    $(".plst-Itm").each(function () {
        plstItems.push($(this).clone());
    });


    var newPlstItems = shuffle(plstItems);

    $(".plst-Itm").off();
    $(".pls-controller").html("");
    for (i = 0; i < newPlstItems.length; i++) {
        $(".pls-controller").append(newPlstItems[i]);
    }

    $(".plst-Itm").on("click", function () {
        OnSongClick($(this));
    });


    OnSongClick($(".plst-Itm").first());
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


function OnSongClick(object) {
    playSong($(object).attr("data-id"), $(object).attr("data-song"));
    $(".plst-Itm").css("background-color", "#dedede");
    $(object).css("background-color", "#59caf1");
}


//-----------------------------------------------------------------
// PLAYBACK
//-----------------------------------------------------------------





//-----------------------------------------------------------------
// PLAYER
//-----------------------------------------------------------------


function PlayerNavigatePlaySong(song_id)
{
    OpenPlayer();

    $.get("/player/song/" + song_id, function (data) {
        $(".player-content").html(data);
    });
}


function OpenPlayer() {
    $(".player-mch").css("display", "block");
}

function ClosePlayer() {
    stopVideo();
    $(".player-mch").css("display", "none");
}




function playNextSong() {

    var isPlay = false;
    $(".plst-Itm").each(function () {

        if (isPlay) {
            isPlay = false;

            playSong($(this).attr("data-id"), $(this).attr("data-song"));

            $(".plst-Itm").css("background-color", "#dedede");
            $(this).css("background-color", "#59caf1");

            return false;
        }
        else {

            if ($(this).attr("data-id") == CURR_SONG) {
                isPlay = true;
            }
        }
    });
}



function playPrevSong() {

    var isPlay = false;
    $($(".plst-Itm").get().reverse()).each(function () {

        if (isPlay) {
            isPlay = false;

            playSong($(this).attr("data-id"), $(this).attr("data-song"));

            $(".plst-Itm").css("background-color", "#dedede");
            $(this).css("background-color", "#59caf1");

            return false;
        }
        else {

            if ($(this).attr("data-id") == CURR_SONG) {
                isPlay = true;
            }
        }

    });
}


//-----------------------------------------------------------------
// PLAYER
//-----------------------------------------------------------------


// load song into player //!!!!!!!!!!!!!!!
//
function PlayerNavigatePlaySong(song_id)
{
    //window.location = '/Player/Song/' + song_id;

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

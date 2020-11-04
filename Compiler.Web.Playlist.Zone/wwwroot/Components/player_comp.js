
const e = React.createElement;

var CURR_SONG = "";
var CURR_STATUS = "";

var player;



class MusicPlayer extends React.Component {

    constructor(props) {
        super(props);

        this.state = { liked: false };
    }

    componentDidMount() {
        PlayerReady();
    }

    render() {
        
        return (
            <div className="player-mch">

                <div className="player-header">
                    <div className="player-ttl"></div>

                    <div className="player-nav-cnt">
                        <div className="close-btn">X</div>
                        <div className="minimize-btn">-</div>
                    </div>
                </div>


                <div className="player-cont">

                    <div id="playerCH1" className="player-child-cont">

                        <div className="player-controls">
                            <div className="plr-btn plr-btn-prev"><img alt="Prev" src="/images/prev_icn.png"/></div>
                            <div className="plr-btn plr-btn-play"><img alt="Prev" src="/images/pause_icn.png" /></div>
                            <div className="plr-btn plr-btn-next"><img alt="Prev" src="/images/next_icn.png" /></div>
                            <div className="plr-btn plr-btn-shuffle"><img alt="Prev" src="/images/shuffle_icn.png" /></div>
                        </div>

                        <div className="player-name"></div>
                        <div id="player" className="main-back player-back"></div>
                    </div>

                    <div className="player-content">
                    </div>
                </div>

            </div>
        );

    }
}

const domContainer = document.querySelector('.musicPlayerCH');
ReactDOM.render(e(MusicPlayer), domContainer);






function PlayerReady() {

    // 2. This code loads the IFrame Player API code asynchronously.
    var tag = document.createElement('script');
    tag.src = "https://www.youtube.com/iframe_api";
    var firstScriptTag = document.getElementsByTagName('script')[0];
    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);



    $(".player-mch .close-btn").on("click", function () {
        ClosePlayer();
    });

    $(".player-mch .minimize-btn").on("click", function () {

        if ($(this).text() === "-") {

            $(this).text("+");
            $(".player-mch .player-content").css("display", "none");
            $(".player-mch").css("width", "340px");
            $(".player-mch").css("height", "280px");

            $("#playerCH1").css("width", "300px");

            $("#player").css("width", "300px");
            $("#player").css("height", "200px");


            //var TopOffset = ($(document).scrollTop() + $(window).height() - $(".player-mch").height() - 40);
            //var LeftOffset = ($(window).width() - $(".player-mch").width() - 50);
            //$(".player-mch").css("top", TopOffset + "px");
            //$(".player-mch").css("left", LeftOffset + "px");

            PositionPlayer();
        }
        else {
            $(this).text("-");

            $("#playerCH1").css("width", "320px");
            $("#player").css("width", "320px");
            $("#player").css("height", "260px");


            $(".player-mch .player-content").css("display", "block");
            $(".player-mch").css("width", "650px");
            $(".player-mch").css("height", "340px");


            //var TopOffset = ($(document).scrollTop() + $(window).height() - $(".player-mch").height() - 40);
            //var LeftOffset = ($(window).width() - $(".player-mch").width() - 50);
            //$(".player-mch").css("top", TopOffset + "px");
            //$(".player-mch").css("left", LeftOffset + "px");

            PositionPlayer();
        }

    });




    $(".plr-btn-next").on("click", function () {
        playNextSong();
    });

    $(".plr-btn-prev").on("click", function () {
        playPrevSong();
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


    $(document).scroll(function () {
        var TopOffset = ($(this).scrollTop() + $(window).height() - $(".player-mch").height() - 30);
        var LeftOffset = ($(window).width() - $(".player-mch").width() - 50);

        $(".player-mch").css("top", TopOffset + "px");
        $(".player-mch").css("left", LeftOffset + "px");
    });

}



// YT PLAYER EVENTS
//
//-------------------------------------------------------------------------------
function onYouTubeIframeAPIReady() {

    player = new YT.Player('player', {
        width: '320',
        height: '280',
        videoId: '',
        playerVars: { 'autoplay': 0, 'controls': 0 },
        events: {
            'onReady': onPlayerReady,
            'onStateChange': onPlayerStateChange,
            'onError': onPlayerError
        }
    });
}


function onPlayerError(event) {
    console.log('Error: ' + event.data);
    playNextSong();
}

function onPlayerReady(event) {
    event.target.playVideo();
}

var done = false;
function onPlayerStateChange(event) {

    console.log("evt --> " + event.data);

    //console.log("event.data -->" + event.data);
    if (event.data == 0) {
        playNextSong();
    }

    if (event.data == YT.PlayerState.PLAYING && !done) {
        done = true;
    }
}

function stopVideo() {
    player.stopVideo();
    CURR_STATUS = "STOP";
}

function pauseVideo() {
    if (CURR_STATUS == "PLAY") {
        //$(".plr-btn-play").html("play");
        $(".plr-btn-play").html("<img alt='Play' src='/images/play_icn.png' />");
        player.pauseVideo();
        CURR_STATUS = "PAUSE";
    }
    else {
        //$(".plr-btn-play").html("pause");
        $(".plr-btn-play").html("<img alt='Pause' src='/images/pause_icn.png' />");
        player.playVideo();
        CURR_STATUS = "PLAY";
    }
}

function playSong(song_id, song_guid) {
    CURR_SONG = song_id;
    player.loadVideoById(song_guid, 0, 100);
    CURR_STATUS = "PLAY";
}









function PlayerNavigatePlaySong(song_id) {
    OpenPlayer();

    $.get("/player/song/" + encodeURIComponent(song_id), function (data) {
        $(".player-content").html(data);
    });
}

function OpenPlayer() {
    $(".player-mch").css("display", "block");
    PositionPlayer();
}

function ClosePlayer() {
    stopVideo();
    $(".player-mch").css("display", "none");
}

function PositionPlayer() {
    var TopOffset = ($(document).scrollTop() + $(window).height() - $(".player-mch").height() - 40);
    var LeftOffset = ($(window).width() - $(".player-mch").width() - 50);
    $(".player-mch").css("top", TopOffset + "px");
    $(".player-mch").css("left", LeftOffset + "px");
}





//PLAYLIST CONTROLLER
//
//----------------------------------------------------------------------------

function playNextSong() {

    try {
    
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
    catch (ex) {
        console.log(ex);
    }
}



function playPrevSong() {

    try {

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

    var i;
    for (i = 0; i < newPlstItems.length; i++) {
        $(".pls-controller").append(newPlstItems[i]);
    }

    $(".plst-Itm").on("click", function () {
        OnSongClick($(this));
    });


    OnSongClick($(".plst-Itm").first());
}


function OnSongClick(object) {
    playSong($(object).attr("data-id"), $(object).attr("data-song"));


    $(".plst-Itm").each(function () {
        $(this).removeClass("plst-Itm-High-Back"); //.addClass("plst-Itm-Norm-Back");
    });
    

    $(object).addClass("plst-Itm-High-Back");



    //$(".plst-Itm").css("background-color", "#dedede");
    //$(object).css("background-color", "#59caf1");
}



function AddSongToPlaylist(current_song_id) {

    LightBoxOn();

    $.get("/Playlists/AddSongToPlaylist/" + encodeURIComponent(current_song_id), function (data) {
        LightBoxContent(data);
    })
    .done(function () {
    })
    .fail(function () {
        NotAuthorized();
    })
    .always(function () {
    });

}

//----------------------------------------------------------------------------
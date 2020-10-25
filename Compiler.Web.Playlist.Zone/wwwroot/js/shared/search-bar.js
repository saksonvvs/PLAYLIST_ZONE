

function InitSearchBar(controlName)
{   
    $(controlName).on("keyup", function (event) {
        var keyVal = $(controlName).val()
        if (event.keyCode == 13) {
            StartSearch(keyVal);
        }
    });

    StartSearch = function (keyVal) {
        PreloaderOn(".main-content");

        $.get("/Search/Keyword/" + keyVal, function (data) {
            $(".main-content").html(data);
        });
    }
}
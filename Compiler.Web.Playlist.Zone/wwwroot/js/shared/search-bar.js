

function InitSearchBar(controlName)
{
    $(controlName).on("keyup", function (event) {
        var keyVal = $("#SearchBarTxt").val()
        if (event.keyCode == 13) {
            StartSearch(keyVal);
        }
    });

    StartSearch = function (keyVal) {
        PreloaderOn(controlName);

        $.get("/Search/Keyword/" + keyVal, function (data) {
            $(controlName).html(data);
        });
    }
}
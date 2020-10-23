// Write your JavaScript code.


$(document).ready(function () {
    $(".mainContLf").css("height", ($(window).height()-110) + "px");
    $(".mainContRt").css("height", ($(window).height()-110) + "px");
});


$(window).resize(function () {
    $(".mainContLf").css("height", ($(window).height()-110) + "px");
    $(".mainContRt").css("height", ($(window).height()-110) + "px");
});




function SetupDropDown(pControlId, pServiceUrl) {

    $.ajax({
        type: "GET",
        url: pServiceUrl,
        contentType: "application/json",
        data: {},
        timeout: 1000,
        dataType: "json",
        success: function (result) {

            var obj = jQuery.parseJSON(result);
            for (var i = 0; i < obj.length; i++) {
                
                $(pControlId).append(
                    $('<option></option>').val(obj[i].Id).html(obj[i].Name)
                );
            }

        },
        error: function (result) { console.log('error - SetupDropDown'); }
    });

    /*$.get(pServiceUrl, function (data) {
        
        var obj = jQuery.parseJSON(data);

        //console.log("-->" + obj.length + "-->");
        for (var i = 0; i < obj.length; i++) {
            //console.log("<option value='" + obj[i].Id + "'>" + obj[i].Name + "</option>");
           
            $(pControlId).append(
                $('<option></option>').val(obj[i].Id).html(obj[i].Name)
            );
        }

    });*/

}



function GetData(pServiceUrl) {

    $.ajax({
        type: "GET",
        url: pServiceUrl,
        contentType: "application/json",
        data: {},
        timeout: 1000,
        dataType: "json",
        success: function (result) {
            
            var obj = jQuery.parseJSON(result);
            callbackGetData(obj);
        },
        error: function (result) { console.log('error - GetData'); }
    });
    
}



function DataList_NameUid(pJson, pSelector, pItemClass) {

    $(pSelector).html("");
    for (var i = 0; i < pJson.length; i++) {
        $(pSelector).append("<div class='" + pItemClass +"' data-Uid='"+ pJson[i].Uid +"' >" + pJson[i].Name + "</div>");
    }

}
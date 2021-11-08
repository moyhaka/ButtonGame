// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    console.log("Page is ready");

    $(document).on("click",".game-button", function (event) {
        event.preventDefault();

        var buttonNumber = $(this).val();
        console.log("button" + buttonNumber + " was clicked");
        doButtonUpdate(buttonNumber);
    })

    function doButtonUpdate(buttonNumber){
        $.ajax({
            datatype: "json",
            method: 'POST',
            url: '/button/ShowOneButton',
            data: {
                "buttonNumber": buttonNumber
            },
            success: function (data) {

                //data should be a json object consisting of a buttonString and messageString
                console.log(data);
                $("#" + buttonNumber).html(data.part1);
                $("#messageArea").html(data.part2);
            }
        })
    }
})


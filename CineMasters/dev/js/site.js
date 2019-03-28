// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function disableFreeSeats() {
    $(".checkbox > input").each(function () {
        if ($(this).attr('data-status') == "free") {
            $(this).attr('disabled', true);
        }
    });
}

function ableFreeSeats() {
    $(".checkbox > input").each(function () {
        if ($(this).attr('data-status') == "free") {
            $(this).removeAttr('disabled');
        }
    });
}
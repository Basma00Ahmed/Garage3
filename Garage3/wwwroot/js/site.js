// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$().ready(function () {
    $("#licenseplateForm").validate({
        rules: {
            regNo: {
                required: true,
                minlength: 3,
                maxlength: 7
            }
        },
        messages: {
            regNo: {
                required: "Don't try to take a shortcut Romen :)", //"Please provide a license plate number",
                minlength: "Romen, try to enter 3 characters at least ;)"//"The licence plate number must be at least 3 characters long"
            }
        }
    });

});
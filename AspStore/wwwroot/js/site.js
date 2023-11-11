// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    $(".modal").modal("show");
    togglePasswordIcons();
})

function togglePasswordIcons(){
    $("#togglePassword").click(function () {
        $(this).toggleClass("bi-eye-fill bi-eye-slash-fill");
    });
}
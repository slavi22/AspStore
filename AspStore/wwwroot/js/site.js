// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    $(".modal").modal("show");
    console.log($("#search-bar").height())
    console.log($("#search-bar").width())
    togglePasswordIcons();
})

function togglePasswordIcons(){
    $(".password-icon").click(function () {
        let showPasswordToggled = $(".password-icon").hasClass("bi-eye-fill")?"text":"password";
        $(".password").attr("type", showPasswordToggled);
        $(".password-icon").toggleClass("bi-eye-fill bi-eye-slash-fill");
    });
}
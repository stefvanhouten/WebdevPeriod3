document.addEventListener("DOMContentLoaded", function (event) {
    document.getElementById("nav-toggle").addEventListener("click", (e) => {
        document.getElementById("side-nav").classList.toggle("hidden");
    });
});

$(() => {
    $(".dropdown .dropdown-toggle").on("click", function () {
        $(this).siblings(".dropdown-content").toggleClass("hidden");
    });
});

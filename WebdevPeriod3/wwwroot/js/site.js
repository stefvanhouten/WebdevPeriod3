$(() => {
    $("#nav-toggle").on("click", () => {
        $("#side-nav").toggleClass("hidden");
    });

    $(".dropdown .dropdown-toggle").on("click", function () {
        $(this).siblings(".dropdown-content").toggleClass("hidden");
    });
});

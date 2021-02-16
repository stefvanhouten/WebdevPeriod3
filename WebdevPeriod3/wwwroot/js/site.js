document.addEventListener("DOMContentLoaded", function (event) {
    document.getElementById("nav-toggle").addEventListener("click", (e) => {
        document.getElementById("side-nav").classList.toggle("hidden");
    });
    document.getElementById("dropdown").addEventListener("click", (e) => {
        document.getElementById("dropdown-content").classList.toggle("hidden");
    });
});


document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("nav-toggle").addEventListener("click", () => {
        document.getElementById("side-nav").classList.toggle("hidden");
    });
    document.getElementById("dropdown").addEventListener("click", () => {
        document.getElementById("dropdown-content").classList.toggle("hidden");
    });
});


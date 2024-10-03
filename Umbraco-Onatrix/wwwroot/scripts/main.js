const hamburgerMenu = document.getElementById("btn-menu");
const nav = document.querySelector(".nav-menu");

hamburgerMenu.addEventListener("click", () => {
    nav.classList.toggle("mobile");
});
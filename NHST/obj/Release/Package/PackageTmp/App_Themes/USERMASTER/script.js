function getDOM() {
  const showDropDown = document.querySelectorAll("[data-show-dropdown]");
  const allDropDown = document.querySelectorAll(".dropdown");
  const sidebarBtn = document.querySelector("[data-sidebar]");
  const sidebar = document.querySelector(".sidebar");
  const logoMain = document.querySelector(".header-logo");
  const sidebarFoot = document.querySelector(".sidebar .foot");
  const main = document.querySelector(".main");
  const statusBar = document.querySelector(".status-bar");

  return {
    sidebarBtn,
    showDropDown,
    allDropDown,
    sidebar,
    logoMain,
    sidebarFoot,
    main,
    statusBar,
  };
}
window.addEventListener("DOMContentLoaded", function () {
    const DOM = getDOM();
    console.log(DOM)

  DOM.showDropDown.forEach((item) => {
    item.addEventListener("click", () => {
      const getDrop = item.querySelector(".dropdown");
      getDrop.classList.toggle("active");
    });
  });

  DOM.sidebarBtn.addEventListener("click", () => {
    DOM.sidebar.classList.toggle("active");
    DOM.sidebarFoot.classList.toggle("active");
    DOM.logoMain.classList.toggle("active");
    DOM.statusBar.classList.toggle("active");
    DOM.main.classList.toggle("active");
  });
});

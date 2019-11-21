$("#manageUserMenu").click(function () {
    var el = document.getElementById('ui-basic');
    if (hasClass(el, 'show')) {
        el.classList.remove("show");
    } else {
        el.classList.add("show");
    }
});

$("#manageUser").click(function () {
    var el = document.getElementById('auth');
    if (hasClass(el, 'show')) {
        el.classList.remove("show");
    } else {
        el.classList.add("show");
    }
});

function hasClass(element, cls) {
    return (' ' + element.className + ' ').indexOf(' ' + cls + ' ') > -1;
}

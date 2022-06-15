var _URL = window.location;
$(document).ready(function () {
    //$("#sidebar-menu").find('a[href="' + URL + '"]').addClass('current-page');

    var activebutton = $("#sidebar-menu").find('a.nav-link').filter(function () {
        return this.href == _URL.href.replace("#", "");
    });
    activebutton.addClass('active');

    activebuttonparent = activebutton.parent().parent().parent();
    if (activebuttonparent.hasClass("nav-item")) {
        activebuttonparent.addClass('menu-open');
        //activebuttonparent.find('a.nav-group-header').addClass('active');
    }
});
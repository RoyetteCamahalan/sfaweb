
var Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000
});

function fnScroll(e, cls, isjs) {
    if (isjs == 1) {
        if (cls == 1) {
            e = "." + e;
        } else {
            e = "#" + e;
        }
        $('html, body').animate({
            scrollTop: ($(e).first().offset().top)
        }, 500);
    } else {
        if (cls == 1) {
            var elements = document.getElementsByClassName(e);
            ctrl = elements[0];
        } else {
            ctrl = document.getElementById(e);
        }
        ctrl.scrollIntoView(true);
    }
}
function fnFireEvent(e, id) {
    console.log(id + e);
    var ct = document.getElementById(id);
    console.log(ct);
    if (e == "click") {
        $(ct).trigger("click");
    }
}
function fnFireEventClass(e, id) {
        $(id).trigger("click");
}

function fnShowLoader(id) {
    $(document.getElementById(id)).attr("style", "display:inherit");

} function fnHideLoader(id) {
    console.log('hide loader: ' + id);
    $(document.getElementById(id)).attr("style", "display:none");
}

function imgError(me) {
    // place here the alternative image
    var AlterNativeImg = "images/noimage.png";
    this.onerror = null;
    // to avoid the case that even the alternative fails        
    if (AlterNativeImg != me.src)
        me.src = AlterNativeImg;
}
function fnshowimgpreview(me, target) {
    console.log($(me).attr("src"));
    $(target).attr("src", $(me).attr("src"));
}
function fnshowalert(t, msg) {
    Toast.fire({
        icon: t,
        title: msg
    });
}
function fnOpenInNewTab(target) {
    console.log(target);
    window.open(target, '_blank');
}
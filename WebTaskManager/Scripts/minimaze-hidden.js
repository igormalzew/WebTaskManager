var minWidth = 720;
var minHeight = 630;
$(window).resize(function () {
    var docWidth = document.documentElement.clientWidth;
    var docHeight = document.documentElement.clientHeight;

    if (docWidth < minWidth || docHeight < minHeight) {
        $(".minimaze-hidden").hide();
    } else {
        $(".minimaze-hidden").show();
    }
});
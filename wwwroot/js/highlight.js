var ctrlKey = false;
$('#mainTable').on('click', 'tbody tr', function (event) {
    if (ctrlKey) {
        $(this).addClass('highlight');
    } else {
        $(this).addClass('highlight').siblings().removeClass('highlight');
    }
});
$(document).keydown(function (event) {
    //console.log(event);
    if (!event.ctrlKey) {
        ctrlKey = false;
        return true;
    }
    ctrlKey = true;
    event.preventDefault();
});
$(document).keyup(function (event) {
    //console.log(event);
    if (!event.ctrlKey) {
        ctrlKey = false;
        return true;
    }
});
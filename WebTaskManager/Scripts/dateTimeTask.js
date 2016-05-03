$(function () {
    $('#dateTimeTask').daterangepicker({
        "singleDatePicker": true,
        "locale": {
            "format": "DD.MM.YYYY"
        },
        "startDate": moment(),
        "minDate": moment()
    }, function (start, end, label) {
    });
});
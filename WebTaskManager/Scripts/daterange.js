$(function () {
    function cb(start, end) {
        $('#reportrange').html(start.format('L') + ' - ' + end.format('L'));
    }
    cb(moment(), moment());

    $('#reportrange').daterangepicker({
        ranges: {
            'Сегодня': [moment(), moment()],
            'Завтра': [moment().add(1, 'days'), moment().add(1, 'days')],
            'Месяц': [moment(), moment().add(1, 'months')],
            'Год': [moment(), moment().add(1,'years')]
        },
        "opens": "right",
        "locale": {
            "applyLabel": "Готово",
            "cancelLabel": "Отмена",
            "fromLabel": "От",
            "toLabel": "До",
            "customRangeLabel": "Указать диапазон"
        }
    }, cb);

});
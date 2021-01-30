$(document).ready(function () {
    setDAM();
});

function setDAM() {

    var dt = $('.dashboard-item .dam .dam-date');

    dt.datepicker("destroy");
    dt.removeClass("hasDatepicker calendarclass");
    dt.unbind();

    dt.datepicker()
        .on('change', function (e) {
            DAM_applyDate(dt.val());
        });

    $('.dashboard-item .dam .btn-refresh').click(function () {    
        DAM_applyDate(dt.val());
    });

}
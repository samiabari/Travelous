$(document).ready(function () {
    $('#StartDate').datetimepicker({
        minDate: moment(),
        format: 'DD/MM/YYYY'
       
    });
    $('#EndDate').datetimepicker({
        useCurrent: false,
        format: 'DD/MM/YYYY'//Important! See issue #1075
       
    });
    $("#StartDate").on("dp.change", function (e) {
        $('#EndDate').data("DateTimePicker").minDate(e.date);
    });
    $("#EndDate").on("dp.change", function (e) {
        $('#StartDate').data("DateTimePicker").maxDate(e.date);
    });
  
} );


$(document).ready(function () {

    $('#ddlCountry').change(function () {
        debugger;
        $.ajax({
            type: "POST",

            url: "/Home/GetCityIdByCountryId",

            data: { countryId: $('#ddlCountry').val() },

            datatype: "json",

            traditional: true,
         
            success: function (data) {
                var city = "<select id='ddlCity'  name='CityId' class = 'form-control border-radius border-right' >";
                city = city + '<option value="">--Select--</option>';
                for (var i = 0; i < data.length; i++) {
                    city = city + '<option value=' + data[i].Value + '>' + data[i].Text + '</option>';
                }
                city = city + '</select>';
                $('#City').html(city);
            }
        });
    });
   
});
$(document).ready(function () {
    var currentCurrency = "USD";

    const settings = {
        "async": true,
        "crossDomain": true,
        "url": "",
        "method": "GET",
    };

    $("#currency").change(function () {
        var subtotalInUsd = $("#subtotal").text();
        var newCurrency = $("#currency").val();
        settings.url = "https://v6.exchangerate-api.com/v6/83413174eab34df2b0a4f888/pair/" + currentCurrency + "/" + newCurrency
        $.ajax(settings).done(function (response) {
            console.log(response);
            var newSubtotal = parseFloat(response.conversion_rate) * parseFloat(subtotalInUsd);
            console.log(newSubtotal);
            $("#subtotal").html(newSubtotal.valueOf());
            currentCurrency = newCurrency;
        });
    });
});
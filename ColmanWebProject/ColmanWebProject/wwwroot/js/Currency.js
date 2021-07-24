$(document).ready(function () {
    const settings = {
        "async": true,
        "crossDomain": true,
        "url": "https://currency-exchange.p.rapidapi.com/exchange?to=USD&from=EUR&q=1.0",
        "method": "GET",
        "headers": {
            "x-rapidapi-key": "4efe20136bmsh8e1ec2eca8f0781p13147cjsn9ed19cd8e579",
            "x-rapidapi-host": "currency-exchange.p.rapidapi.com"
        }
    };

    $("#currency").click(function () {
        var subtotalInUsd = $("#subtotal").val;
        $.ajax(settings).done(function (response) {
            console.log(response);
            $("#subtotal").html(response * subtotalInUsd);
        });
    });
});
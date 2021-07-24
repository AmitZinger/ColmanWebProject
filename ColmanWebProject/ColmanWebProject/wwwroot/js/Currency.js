$(document).ready(function () {
        // set endpoint and your API key
        endpoint = 'convert';
   const access_key = 'e527b201671beb38bf99768ba9604794';

        // define from currency, to currency, and amount
        from = 'EUR';
        to = 'GBP';
        amount = '10';

        // execute the conversion using the "convert" endpoint:
    $.ajax({
        url: 'https://data.fixer.io/api/latest?access_key=' + access_key,
        //type: "get",
           // url: 'https://data.fixer.io/api/' + endpoint + '?access_key=' + access_key + '&from=' + from + '&to=' + to + '&amount=' + amount,
            dataType: 'jsonp',
            success: function (json) {

                // access the conversion result in json.result
                alert(json.result);
                console.log(json.result);

            }
        });
});
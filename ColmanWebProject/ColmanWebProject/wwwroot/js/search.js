$(document).ready(function () {
    $('#searchForm').submit(function (e) {
        e.preventDefault();
        var searchValue = $('#searchValue').val();
        var currentLocation = $(location).attr("href");

        if (currentLocation.includes("Products?") || currentLocation.includes("Products/SearchWithFullView")) {
            $.ajax({
                url: "/Products/SearchWithPartialView",
                data: { queryTitle: searchValue }
            }).done(function (data) {
                $('#results').html(data)
            })
        } else {
            window.location.replace("/Products/SearchWithFullView?queryTitle=" + searchValue);
        }
    })

    $('#multiSearch').submit(function (e) {
        e.preventDefault();
        var searchByName = $('#searchByName').val();
        var searchByCategory = $('#searchByCategory').val();
        var searchByPriceFrom = $('#searchByPriceFrom').val();
        var searchByPriceTo = $('#searchByPriceTo').val();

        $.ajax({
            url: "/Products/SearchWithMulti",
            data: { name: searchByName, category: searchByCategory, priceFrom: searchByPriceFrom, priceTo: searchByPriceTo}
        }).done(function (data) {
            $('#multiSearch')[0].reset();
            $('#results').html(data);
        })
       
    })
})

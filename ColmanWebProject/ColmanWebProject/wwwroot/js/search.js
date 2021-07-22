$(document).ready(function () {
    $('#searchForm').submit(function (e) {
        e.preventDefault();
        var searchValue = $('#searchValue').val();
        var currentLocation = $(location).attr("href");

        if (currentLocation.includes("Products")) {
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

    $('#categoriesSearch').submit(function (e) {
        e.preventDefault();
        var searchbytype = $('#searchByTypeName').val();

        $.ajax({
            url: "/Categories/SearchByTypeName",
            data: { typeName: searchbytype }
        }).done(function (data) {
            $('#partial').html(data);
        })
    })

    $('#customersSearch').submit(function (e) {
        e.preventDefault();
        var searchbyname = $('#searchByName').val();
        window.location.replace("/Customers/SearchByName?name=" + searchbyname);
    })

    $('#productsSearch').submit(function (e) {
        e.preventDefault();
        var searchbyname = $('#searchByName').val();

        $.ajax({
            url: "/Products/SearchByName",
            data: { name: searchbyname }
        }).done(function (data) {
            $('#partialManageProduct').html(data);
        })
    })
})

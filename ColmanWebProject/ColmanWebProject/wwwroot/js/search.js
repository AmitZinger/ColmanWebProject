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
            //$.ajax({
            //    url: "/Products/SearchWithFullView",
            //    data: { queryTitle: searchValue }
            //}).done(function (data) {
            //    location.href()
            //})
            window.location.replace("/Products/SearchWithFullView?queryTitle=" + searchValue);
        }
     })
})

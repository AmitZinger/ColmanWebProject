$(document).ready(function () {

    $("#addBtn").click(function () {
        $.ajax({
            url: $(this).attr("formaction"),
        }).done(function (msg) {
            $("#AddContent").html(msg);
            $("#add-contact").modal("show");
        })
    });

    $("body").on("click", "#saveAdd", function () {
        var form = $('form');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        $.ajax({
            type: "post",
            url: "/Categories/Create",
            data: {
                __RequestVerificationToken: token,
                category: {
                    Type: $("#Type").val(),
                    SubType: $("#SubType").val(),
                    Description: $("#Description").val()
                }
            },
            dataType: "html",
            success: function (result) {
                $("#add-contact").modal("hide");
                $("#partial").html(result);
            }
        });
        return false;
    });


    $("#addProductBtn").click(function () {
        $.ajax({
            url: $(this).attr("formaction"),
        }).done(function (msg) {
            $("#AddProductContent").html(msg);
            $("#add-product-contact").modal("show");
        })
    });

    $("body").on("click", "#saveProductAdd", function () {
        var form = $('form');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        $.ajax({
            type: "post",
            url: "/Products/Create",
            data: {
                __RequestVerificationToken: token,
                product: {
                    Name: $("#Name").val(),
                    Price: $("#Price").val(),
                    Stock: $("#Stock").val(),
                    Description: $("#Description").val(),
                    CategoryId: $("#CategoryId").val()
                }
            },
            dataType: "html",
            success: function (result) {
                $("#add-product-contact").modal("hide");
                $("#partialManageProduct").html(result);
            }
        });
        return false;
    });

})
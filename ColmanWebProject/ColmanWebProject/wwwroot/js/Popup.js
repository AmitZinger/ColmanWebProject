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
        $("#errorCategory").empty();
        var form = $('form');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        formData = new FormData();
        formData.append("__RequestVerificationToken", token);
        formData.append("Type", $("#Type").val());
        formData.append("SubType", $("#SubType").val());
        formData.append("Description", $("#Description").val());
        formData.append("ImageFile", $("#ImageFile")[0].files[0]);

        $.ajax({
            type: "post",
            url: "/Categories/Create",
            processData: false,
            contentType: false,
            data: formData,
            dataType: "html",
            success: function (result) {
                if (result.indexOf("error") == -1) {
                    $("#add-contact").modal("hide");
                    $("#partial").html(result);
                }
                else
                {
                    var jsonResult = JSON.parse(result);
                    $("#errorCategory").append(jsonResult.html);
                    $("#errorCategory").css('visibility', 'visible');
                }
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
        $("#errorProduct").empty();
        var form = $('form');
        var token = $('input[name="__RequestVerificationToken"]', form).val();

        formData = new FormData();
        formData.append("__RequestVerificationToken", token);
        formData.append("Name", $("#Name").val());
        formData.append("Price", $("#Price").val());
        formData.append("Stock", $("#Stock").val());
        formData.append("Description", $("#Description").val());
        formData.append("CategoryId", $("#CategoryId").val());
        formData.append("ImageFile", $("#ImageFile")[0].files[0]);

        $.ajax({
            type: "post",
            url: "/Products/Create",
            processData: false,
            contentType: false,
            data: formData,
            dataType: "html",
            success: function (result) {
                if (result.indexOf("error") == -1) {
                    $("#add-product-contact").modal("hide");
                    $("#partialManageProduct").html(result);
                }
                else
                {
                    var jsonResult = JSON.parse(result);
                    $("#errorProduct").append(jsonResult.html);
                    $("#errorProduct").css('visibility', 'visible');
                }
            }
        });
        return false;
    });


    $("#addStoreBtn").click(function () {
        $.ajax({
            url: $(this).attr("formaction"),
        }).done(function (msg) {
            $("#AddStoreContent").html(msg);
            $("#add-store-contact").modal("show");
        })
    });

    $("body").on("click", "#saveStoreAdd", function () {
        $("#error").empty();
        var form = $('form');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        $.ajax({
            type: "post",
            url: "/Stores/Create",
            data: {
                __RequestVerificationToken: token,
                store: {
                    Lontitude: $("#Lontitude").val(),
                    Latitude: $("#Latitude").val(),
                    OpeningHour: $("#OpeningHour").val(),
                    ClosingHour: $("#ClosingHour").val()
                }
            },
            dataType: "html",
            success: function (result) {
                if (result.indexOf("error") == -1) {
                    $("#add-store-contact").modal("hide");
                    $("#partialManageStore").html(result);
                }
                else {
                    var jsonResult = JSON.parse(result);
                    $("#error").append(jsonResult.html);
                    $("#error").css('visibility', 'visible');
                }
            }
        });
        return false;
    });
})
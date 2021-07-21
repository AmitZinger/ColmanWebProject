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
                $("#add-product-contact").modal("hide");
                $("#partialManageProduct").html(result);
            }
        });
        return false;
    });

})
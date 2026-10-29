$(document).on("keyup change", "#Quantity, #MRP, #Net, #ItemFinalRate, #ItemDiscount", function () {

    var qty = parseFloat($("#Quantity").val()) || 0;
    var mrp = parseFloat($("#MRP").val()) || 0;
    var net = parseFloat($("#Net").val()) || 0;
    var finalRate = parseFloat($("#ItemFinalRate").val()) || 0;
    var discount = parseFloat($("#ItemDiscount").val()) || 0;

    var amount = 0;

    if (finalRate > 0) {
        amount = qty * finalRate;
    }

    else if (net > 0) {
        amount = qty * net;
    }

    else if (mrp > 0) {
        amount = qty * mrp;
    }

    if (discount > 0) {
        amount = amount - ((amount * discount) / 100);
    }

    if (amount < 0) {
        amount = 0;
    }

    $("#ItemTotalAmount").val(amount.toFixed(2));
});

$(document).off("click", ".btn-submit").on("click", ".btn-submit", function () {
    if (!fnItemValidation()) return;

    var mode = $("#modal-quotation-item").data("mode");
    var row = $("#modal-quotation-item").data("row");

    var data = {
        ItemName: $("#ItemName").val(),
        Quantity: $("#Quantity").val(),
        UnitOfMeasure: $("#UnitOfMeasureId option:selected").text(),
        UnitOfMeasureId: $("#UnitOfMeasureId").val(),
        Discount: $("#ItemDiscount").val(),
        MRP: $("#MRP").val(),
        Net: $("#Net").val(),
        FinalRate: $("#ItemFinalRate").val(),
        Amount: $("#ItemTotalAmount").val()
    };

    if (mode === "edit" && row) {
        $(document).trigger("item-updated", [data, row]);
    } else {
        $(document).trigger("item-created", [data]);
    }

    $('.modal').modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();

    CalculateTotal();
});

function fnItemValidation() {

    var isValid = true;

    var itemName = $.trim($("#ItemName").val());
    var quantity = $.trim($("#Quantity").val());
    var unitOfMeasureId = $.trim($("#UnitOfMeasureId").val());
    var discount = $.trim($("#ItemDiscount").val());
    var amount = $.trim($("#ItemTotalAmount").val());
    var mrp = $.trim($("#MRP").val());
    var net = $.trim($("#Net").val());

    // Item Name
    if (itemName == null || itemName == "" || itemName == undefined) {
        $("#ItemName").next("span").text("Item Name is required!");
        isValid = false;
    } else {
        $("#ItemName").next("span").text("");
    }

    // Quantity
    if (quantity == null || quantity == "" || quantity == undefined) {
        $("#spnQty").text("Quantity is required!");
        isValid = false;
    }
    else if (parseFloat(quantity) <= 0) {
        $("#spnQty").text("Quantity must be greater than 0!");
        isValid = false;
    }
    else {
        if (unitOfMeasureId == null || unitOfMeasureId == "" || unitOfMeasureId == undefined) {
            $("#spnQty").text("Unit Of Measure is required!");
            isValid = false;
        }
        else {
            $("#spnQty").text("");
        }
    }

    // Discount
    if (discount != null && discount != "" && discount != undefined) {
        if (isNaN(discount)) {
            $("#ItemDiscount").next("span").text("Discount must be numeric!");
            isValid = false;
        }
        else if (parseFloat(discount) < 0 || parseFloat(discount) > 100) {
            $("#ItemDiscount").next("span").text("Discount must be between 1% to 100%!");
            isValid = false;
        }
        else {
            $("#ItemDiscount").next("span").text("");
        }
    }
    else {
        $("#ItemDiscount").next("span").text("");
    }


    //MRP
    if (mrp == null || mrp == "" || mrp == undefined || mrp == 0) {
        $("#MRP").next("span").text("MRP is required! Or RRP is required!");
        isValid = false;
    }
    else if (parseFloat(mrp) <= 0) {
        $("#MRP").next("span").text("RRP must be greater than 0!");
        isValid = false;
    }
    else {
        $("#MRP").next("span").text("");
    }


    //Net
    if (net == null || net == "" || net == undefined || net == 0) {
        $("#Net").next("span").text("Net Amount is required!");
        isValid = false;
    }
    else if (parseFloat(net) <= 0) {
        $("#Net").next("span").text("Net Amount must be greater than 0!");
        isValid = false;
    }
    else {
        $("#Net").next("span").text("");
    }

    // Amount
    if (amount == null || amount == "" || amount == undefined || amount == 0) {
        $("#ItemTotal").next("span").text("Amount is required!");
        isValid = false;
    }
    else if (parseFloat(amount) <= 0) {
        $("#ItemTotal").next("span").text("Amount must be greater than 0!");
        isValid = false;
    }
    else {
        $("#ItemTotal").next("span").text("");
    }

    return isValid;
}
$(document).on("keyup change", "#LabourQuantity, #LabourFinalRate", function () {

    var row = $(this).closest(".modal-body");

    var qty = parseFloat(row.find("#LabourQuantity").val()) || 0;
    var FinalRate = parseFloat(row.find("#LabourFinalRate").val()) || 0;

    var amount = qty * FinalRate;

    if (amount < 0) {
        amount = 0;
    }

    row.find("#LabourTotal").val(amount.toFixed(2));
});


$(document).off("click", ".btn-submit").on("click", ".btn-submit", function () {
    if (!fnLabourValidation()) return;
    var mode = $("#modal-labour-item").data("mode");
    var row = $("#modal-labour-item").data("row");
    var data = {
        Description: $("#Description").val(),
        Quantity: $("#LabourQuantity").val(),
        UnitOfMeasure: $("#UnitOfMeasureId option:selected").text(),
        UnitOfMeasureId: $("#UnitOfMeasureId").val(),
        FinalRate: $("#LabourFinalRate").val(),
        Amount: $("#LabourTotal").val()
    };
    if (mode === "edit" && row) {
        $(document).trigger("labour-updated", [data, row]);
    } else {
        $(document).trigger("labour-created", [data]);
    }

    $('.modal').modal('hide');
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();

    CalculateTotal();
});


function fnLabourValidation() {
    var isValid = true;

    var description = $.trim($("#Description").val());
    var quantity = $.trim($("#LabourQuantity").val());
    var unitOfMeasureId = $.trim($("#UnitOfMeasureId").val());
    var amount = $.trim($("#LabourTotal").val());
    var finalRate = $.trim($("#LabourFinalRate").val());
  

    // name Name
    if (description == null || description == "" || description == undefined) {
        $("#Description").next("span").text("Description is required!");
        isValid = false;
    } else {
        $("#Description").next("span").text("");
    }

    // Quantity
    if (quantity == null || quantity == "" || quantity == undefined) {
        $("#LabourQuantity").closest(".col").find(".qty-error").text("Quantity is required!");
        isValid = false;
    }
    else if (parseFloat(quantity) <= 0) {
        $("#LabourQuantity").closest(".col").find(".qty-error").text("Quantity must be greater than 0!");
        isValid = false;
    }
    else {
        $("#LabourQuantity").closest(".col").find(".qty-error").text("");
    }


    // Unit Of Measure
    if (unitOfMeasureId == null || unitOfMeasureId == "" || unitOfMeasureId == undefined) {
        $("#UnitOfMeasureId").closest(".col").find(".unit-error").text("Unit Of Measure is required!");
        isValid = false;
    }
    else {
        $("#UnitOfMeasureId").closest(".col").find(".unit-error").text("");
    }
    

    //Final Rate
    if (finalRate == null || finalRate == "" || finalRate == undefined || finalRate == 0) {
        $("#LabourFinalRate").next("span").text("Final Rate is required!");
        isValid = false;
    }
    else if (parseFloat(finalRate) <= 0) {
        $("#LabourFinalRate").next("span").text("Final Rate must be greater than 0!");
        isValid = false;
    }
    else {
        $("#LabourFinalRate").next("span").text("");
    }

    // Amount
    if (amount == null || amount == "" || amount == undefined) {
        $("#LabourTotal").next("span").text("Amount is required!");
        isValid = false;
    }
    else if (parseFloat(amount) <= 0) {
        $("#LabourTotal").next("span").text("Amount must be greater than 0!");
        isValid = false;
    }
    else {
        $("#LabourTotal").next("span").text("");
    }

    return isValid;
}
$(document).ready(function () {
    $('#PhoneNumber').mask('+91 00000 00000', {
        translation: {
            '0': { pattern: /[0-9]/ }
        },
        onKeyPress: function (val, e, field, options) {
            if (!val.startsWith('+91')) {
                field.val('+91 ' + val.replace(/[^\d]/g, '').substring(0, 10).replace(/(\d{5})(\d{0,5})/, '$1 $2'));
            }
        }
    });

    SetExpiryDate();

    $("#EffectiveDate").on("change", SetExpiryDate);

    $(".add-room").on("click", function () {
        $("#roomContainer").append(ItemTemplate());
    });

    $(".clear-rooms").on("click", function () {
        $("#roomContainer").empty();
        CalculateTotal();
    });

    $(".add-room").trigger("click");

    // Room Item Modal
    $(document).on("click", ".add-item", function () {
        $(".room").removeClass("active"); // clear previous
        $(this).closest(".room").addClass("active"); // mark current room

        $.ajax({
            url: '/Quotation/AddQuotationItem',
            type: 'GET',
            beforeSend: function () {
                toggleLoading(true);
            },
            success: function (response) {
                $("#divCreateItem").empty().append(response);
                $("#modal-quotation-item").modal("show");
            },
            error: function () {
                alert("Error loading item modal!");
            },
            complete: function () {
                toggleLoading(false);
            }
        });
    });

    $(document).on("item-created", function (e, data) {
        // Find the active room
        var activeRoom = $(".room.active");
        var tbody = activeRoom.find(".item-table tbody");

        // Remove "No record found!" row if present
        tbody.find("tr td.text-danger").closest("tr").remove();

        // Discount text
        var discountText = data.Discount > 0 ? data.Discount + " %" : "0 %";
        var finalRateText = data.FinalRate > 0 ? "₹ " + data.FinalRate : "";

        // Build row HTML
        var newRow = `
                        <tr>                            
                            <td>${data.ItemName}</td>
                            <td>${data.Quantity} ${data.UnitOfMeasure}</td>
                            <td class="text-right">₹ ${data.MRP}</td>
                            <td class="text-right">₹ ${data.Net}</td>
                            <td class="text-right">${finalRateText}</td>
                            <td class="text-right">${discountText}</td>
                            <td class="amount text-right">₹ ${data.Amount}</td>
                            <td hidden>${data.UnitOfMeasureId || ""}</td>
                            <td hidden>${data.Id || ""}</td>
                            <td hidden>${data.QuotationRoomId || ""}</td>
                            <td class="text-center">
                                <a href="javascript:void(0);" class="text-info btn-sm edit-item" data-id="${data.Id}">
                                    <i class="fas fa-edit"></i>
                                </a>
                            </td>
                            <td class="text-center">
                                <a href="javascript:void(0);" class="text-danger btn-sm delete-item" data-id="${data.Id}">
                                    <i class="fas fa-trash"></i>
                                </a>
                            </td>
                        </tr>
        `;

        // Append to the correct room’s table        
        tbody.append(newRow);

        // Recalculate subtotal for this room
        var fabricTotal = 0;
        tbody.find("tr .amount").each(function () {
            var amountText = $(this).text().replace("₹", "").trim();
            fabricTotal += parseFloat(amountText) || 0;
        });

        activeRoom.find(".room-subtotal-item").text("Fabric: ₹ " + fabricTotal.toFixed(2));

        CalculateTotal();
    });

    $(document).on("click", ".edit-item", function () {
        $(".room").removeClass("active");
        $(this).closest(".room").addClass("active");
        var row = $(this).closest("tr");
        // Extract values from row
        var itemData = {
            ItemName: row.find("td:eq(0)").text(),
            Quantity: parseFloat(row.find("td:eq(1)").text().replace(/[^\d.]/g, "")) || 0,
            MRP: row.find("td:eq(2)").text().replace("₹", "").trim(),
            Net: row.find("td:eq(3)").text().replace("₹", "").trim(),
            FinalRate: row.find("td:eq(4)").text().replace("₹", "").trim(),
            Discount: row.find("td:eq(5)").text().replace("%", "").trim(),
            Amount: row.find("td:eq(6)").text().replace("₹", "").trim(),
            UnitOfMeasureId: row.find("td:eq(7)").text(),
            Id: row.find("td:eq(8)").text(),
            QuotationRoomId: row.find("td:eq(9)").text(),
        };

        $.ajax({
            url: '/Quotation/EditQuotationItem',
            type: 'POST',
            data: itemData,
            beforeSend: function () {
                toggleLoading(true);
            },
            success: function (response) {
                $("#divCreateItem").empty().append(response);

                $("#modal-quotation-item").data("mode", "edit");
                $("#modal-quotation-item").data("row", row);

                $("#modal-quotation-item").modal("show");
            },
            error: function () {
                alert("Error loading item modal!");
            },
            complete: function () {
                toggleLoading(false);
            }
        });
    });

    $(document).on("item-updated", function (e, data, row) {
        var discountText = data.Discount > 0 ? parseFloat(data.Discount).toFixed(2) + " %" : "0.00 %";
        var finalRateText = data.FinalRate > 0 ? "₹ " + parseFloat(data.FinalRate).toFixed(2) : "0.00";
        var mrpText = data.MRP > 0 ? "₹ " + parseFloat(data.MRP).toFixed(2) : "0.00";
        var netText = data.Net > 0 ? "₹ " + parseFloat(data.Net).toFixed(2) : "0.00";

        row.find("td:eq(0)").text(data.ItemName);
        row.find("td:eq(1)").text(data.Quantity + " " + data.UnitOfMeasure);
        row.find("td:eq(2)").text(mrpText);
        row.find("td:eq(3)").text(netText);
        row.find("td:eq(4)").text(finalRateText);
        row.find("td:eq(5)").text(discountText);
        row.find("td:eq(6)").addClass("amount").text("₹ " + data.Amount);
        row.find("td:eq(7)").text(data.UnitOfMeasureId);
        row.find("td:eq(8)").text(data.Id);
        row.find("td:eq(9)").text(data.QuotationRoomId);

        // recalc subtotal
        var tbody = row.closest("tbody");
        var fabricTotal = 0;
        tbody.find("tr .amount").each(function () {
            fabricTotal += parseFloat($(this).text().replace("₹", "").trim()) || 0;
        });
        //row.closest(".room").find(".room-subtotal-item").text("Total: ₹ " + fabricTotal.toFixed(2));
        row.closest(".room").find(".room-subtotal-item").text("Fabric: ₹ " + fabricTotal.toFixed(2));
        CalculateTotal();
    });

    // Labour Section
    $(document).on("click", ".add-labour", function () {
        $(".room").removeClass("active"); // clear previous
        $(this).closest(".room").addClass("active"); // mark current room
        $.ajax({
            url: '/Quotation/AddLabour',
            type: 'GET',
            beforeSend: function () {
                toggleLoading(true);
            },
            success: function (response) {
                $("#divCreateLabour").empty().append(response);
                $("#modal-labour-item").modal("show");
            },
            error: function () {
                alert("Error loading labour modal!");
            },
            complete: function () {
                toggleLoading(false);
            }
        });
    });

    $(document).on("labour-created", function (e, data) {
        // Find the active room
        var activeRoom = $(".room.active");
        var tbody = activeRoom.find(".labour-table tbody");

        // Remove "No record found!" row if present
        tbody.find("tr td.text-danger").closest("tr").remove();

        var finalRateText = data.FinalRate > 0 ? "₹ " + data.FinalRate : "";

        // Build row HTML
        var newRow = `
                        <tr>                            
                            <td>${data.Description}</td>
                            <td>${data.Quantity} ${data.UnitOfMeasure}</td>
                            <td>${finalRateText}</td>
                            <td class="amount text-right">₹ ${data.Amount}</td>
                            <td hidden>${data.UnitOfMeasureId || ""}</td>
                            <td hidden>${data.Id || ""}</td>
                            <td hidden>${data.QuotationRoomId || ""}</td>
                            <td class="text-center">
                                <a href="javascript:void(0);" class="text-info btn-sm edit-labour" data-id="${data.Id}">
                                    <i class="fas fa-edit"></i>
                                </a>
                            </td>
                            <td class="text-center">
                                <a href="javascript:void(0);" class="text-danger btn-sm delete-labour" data-id="${data.Id}">
                                    <i class="fas fa-trash"></i>
                                </a>
                            </td>
                        </tr>
        `;

        // Append to the correct room’s table
        tbody.append(newRow);

        // Recalculate subtotal for this room
        var grandLabourTotal = 0;
        tbody.find("tr .amount").each(function () {
            var amountText = $(this).text().replace("₹", "").trim();
            grandLabourTotal += parseFloat(amountText) || 0;
        });

        activeRoom.find(".room-subtotal-labour").text("Labour: ₹ " + grandLabourTotal.toFixed(2));
        CalculateTotal();
    });

    $(document).on("click", ".edit-labour", function () {

        $(".room").removeClass("active");
        $(this).closest(".room").addClass("active");
        var row = $(this).closest("tr");

        // Extract values from row
        var labourData = {
            Description: row.find("td:eq(0)").text(),
            Quantity: parseFloat(row.find("td:eq(1)").text().replace(/[^\d.]/g, "")) || 0,
            FinalRate: row.find("td:eq(2)").text().replace("₹", "").trim(),
            Amount: row.find("td:eq(3)").text().replace("₹", "").trim(),
            UnitOfMeasureId: row.find("td:eq(4)").text(),
            Id: row.find("td:eq(5)").text(),
            QuotationRoomId: row.find("td:eq(6)").text(),
        };

        $.ajax({
            url: '/Quotation/EditQuotationLabour',
            type: 'POST',
            data: labourData,
            beforeSend: function () {
                toggleLoading(true);
            },
            success: function (response) {
                $("#divCreateLabour").empty().append(response);

                $("#modal-labour-item").data("mode", "edit");
                $("#modal-labour-item").data("row", row);

                $("#modal-labour-item").modal("show");
            },
            error: function () {
                alert("Error loading labour modal!");
            },
            complete: function () {
                toggleLoading(false);
            }
        });
    });

    $(document).on("labour-updated", function (e, data, row) {
        var finalRateText = data.FinalRate > 0 ? "₹ " + data.FinalRate : "";

        row.find("td:eq(0)").text(data.Description);
        row.find("td:eq(1)").text(data.Quantity + " " + data.UnitOfMeasure);
        row.find("td:eq(2)").text(finalRateText);
        row.find("td:eq(3)").addClass("amount").text("₹ " + data.Amount);
        row.find("td:eq(4)").text(data.UnitOfMeasureId);
        row.find("td:eq(5)").text(data.Id);
        row.find("td:eq(6)").text(data.QuotationRoomId);

        // recalc subtotal
        var tbody = row.closest("tbody");
        var grandLabourTotal = 0;
        tbody.find("tr .amount").each(function () {
            grandLabourTotal += parseFloat($(this).text().replace("₹", "").trim()) || 0;
        });
        row.closest(".room").find(".room-subtotal-labour").text("Labour: ₹ " + grandLabourTotal.toFixed(2));
        CalculateTotal();
    });

    $(document).on("click", ".delete-item", function () {
        // Mark the active room
        $(".room").removeClass("active");
        $(this).closest(".room").addClass("active");

        // Find the row
        var row = $(this).closest("tr");

        // Remove the row
        row.remove();

        // Optionally recalc subtotal for this room
        var tbody = row.closest("tbody");
        var fabricTotal = 0;
        tbody.find("tr .amount").each(function () {
            fabricTotal += parseFloat($(this).text().replace("₹", "").trim()) || 0;
        });
        row.closest(".room").find(".room-subtotal-item").text("Fabric: ₹ " + fabricTotal.toFixed(2));

        CalculateTotal();
    });

    $(document).on("click", ".delete-labour", function () {
        // Mark the active room
        $(".room").removeClass("active");
        $(this).closest(".room").addClass("active");

        // Find the row
        var row = $(this).closest("tr");

        // Remove the row
        row.remove();

        // Optionally recalc subtotal for this room
        var tbody = row.closest("tbody");
        var labourGrandTotal = 0;
        tbody.find("tr .amount").each(function () {
            labourGrandTotal += parseFloat($(this).text().replace("₹", "").trim()) || 0;
        });
        row.closest(".room").find(".room-subtotal-labour").text("Labour: ₹ " + labourGrandTotal.toFixed(2));

        CalculateTotal();
    });

    // Quotation Save
    $('.btn-save').on('click', function () {
        if (!fnValidation()) {
            return;
        }
        $('#loading-element').show();
        toggleLoading(true);
        var mQuotation = {
            Id: $('#Id').val(),
            ClientId: $('#ClientId').val(),
            PropertyId: $('#PropertyId').val(),
            Name: $('#Name').val(),
            PhoneNumber: $('#PhoneNumber').val(),
            PropertyNumber: $('#PropertyNumber').val(),
            SocietyName: $('#SocietyName').val(),
            LandMark: $('#LandMark').val(),
            Area: $('#Area').val(),
            City: $('#City').val(),
            State: $('#State').val(),
            EffectiveDate: $('#EffectiveDate').val(),
            ExpiryDate: $('#ExpiryDate').val(),
            GrandTotal: $('#GrandTotal').val(),
            Amount: $('#FinalAmount').val(),
            Discount: $('#FinalDiscount').val(),
            TransportCharges: $('#TransportCharges').val(),
            FabricGrandTotal: $('#FabricGrandTotal').val(),
            LabourGrandTotal: $('#LabourGrandTotal').val(),
            QuotationRooms: []
        };

        var mQuotationRooms = [];

        // =======================
        // FABRIC ROOMS (Items)
        // =======================
        $('.room').each(function () {
            var room = $(this);

            var mQuotationRoom = {
                Id: room.find('.room-item-name').data('id'),
                RoomName: room.find('.room-item-name').val(),
                RoomItemTotal: room.find('.room-subtotal-item').text().replace('Fabric: ₹', '').replace(/,/g, '').trim(),
                RoomLabourTotal: room.find('.room-subtotal-labour').text().replace('Labour: ₹', '').replace(/,/g, '').trim(),
                QuotationItems: [],
                QuotationLabours: []
            };

            // Items inside this Fabric room
            room.find('.item-table tbody tr').each(function () {
                var td = $(this).find("td");
                var cellText = $(td[1]).text().trim();

                var item = {
                    ItemName: $(td[0]).text().trim(),
                    Quantity: parseFloat(cellText.replace(/[^\d.]/g, "")) || 0,
                    MRP: $(td[2]).text().replace('₹', '').replace(/,/g, '').trim(),
                    Net: $(td[3]).text().replace('₹', '').replace(/,/g, '').trim(),
                    FinalRate: $(td[4]).text().replace('₹', '').replace(/,/g, '').trim(),
                    Discount: $(td[5]).text().replace('%', '').trim(),
                    Amount: $(td[6]).text().replace('₹', '').replace(/,/g, '').trim(),
                    UnitOfMeasureId: $(td[7]).text().trim(),
                    Id: $(td[8]).text().trim(),
                    QuotationRoomId: $(td[9]).text().trim(),
                };

                mQuotationRoom.QuotationItems.push(item);
            });

            //mQuotationRooms.push(mQuotationRoom);
            // Labours inside this Labour room
            room.find('.labour-table tbody tr').each(function () {
                var td = $(this).find("td");
                var cellText = $(td[1]).text().trim();

                var labour = {
                    Description: $(td[0]).text().trim(),
                    Quantity: parseFloat(cellText.replace(/[^\d.]/g, "")) || 0,
                    FinalRate: $(td[2]).text().replace('₹', '').replace(/,/g, '').trim(),
                    Amount: $(td[3]).text().replace('₹', '').replace(/,/g, '').trim(),
                    UnitOfMeasureId: $(td[4]).text().trim(),
                    Id: $(td[5]).text().trim(),
                    QuotationRoomId: $(td[6]).text().trim(),
                };

                mQuotationRoom.QuotationLabours.push(labour);
            });
            mQuotationRooms.push(mQuotationRoom);
        });

        mQuotation.QuotationRooms = mQuotationRooms;

        $.ajax({
            url: '/Quotation/SaveQuotation',
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(mQuotation),
            success: function (result) {
                $('#loading-element').hide();
                if (result.IsSuccess) {
                    showToastr("success", result.Message);
                    setTimeout(function () {
                        window.location.href = "/Quotation/Index";
                    }, 3000); // 3 seconds delay
                } else {
                    showToastr("error", result.Message);
                }
            },
            error: function () {
                $('#loading-element').hide();
                showToastr("error", "Something went wrong!");
            }
        });
    });
});

function fnValidation() {
    var isValid = true;

    var name = $.trim($("#Name").val());
    var phoneNumber = $.trim($("#PhoneNumber").val());
    var societyName = $.trim($("#SocietyName").val());
    var landMark = $.trim($("#LandMark").val());
    var area = $.trim($("#Area").val());
    var city = $.trim($("#City").val());
    var state = $.trim($("#State").val());
    var propertyNumber = $.trim($("#PropertyNumber").val());


    // propertyNumber
    if (propertyNumber == null || propertyNumber == "" || propertyNumber == undefined) {
        $("#PropertyNumber").next("span").text("Property Number is required!");
        isValid = false;
    } else {
        $('#PropertyNumber').next('span').text("");
    }

    // SocietyName
    if (societyName == null || societyName == "" || societyName == undefined) {
        $("#SocietyName").next("span").text("Society Name is required!");
        isValid = false;
    } else {
        $('#SocietyName').next('span').text("");
    }

    // landMark
    if (landMark == null || landMark == "" || landMark == undefined) {
        $("#LandMark").next("span").text("Landmark is required!");
        isValid = false;
    } else {
        $('#LandMark').next('span').text("");
    }

    // area
    if (area == null || area == "" || area == undefined) {
        $("#Area").next("span").text("Area is required!");
        isValid = false;
    } else {
        $('#Area').next('span').text("");
    }

    // City
    if (city == null || city == "" || city == undefined) {
        $("#City").next("span").text("City is required!");
        isValid = false;
    } else {
        $('#City').next('span').text("");
    }


    // State
    if (state == null || state == "" || state == undefined) {
        $("#State").next("span").text("State is required!");
        isValid = false;
    } else {
        $('#State').next('span').text("");
    }

    // Name
    if (name == null || name == "" || name == undefined) {
        $("#Name").next("span").text("Name is required!");
        isValid = false;
    } else {
        $('#Name').next('span').text("");
    }

    // PhoneNumber
    if (phoneNumber == null || phoneNumber == "" || phoneNumber == undefined) {
        $("#PhoneNumber").next("span").text("Phone Number is required!");
        isValid = false;
    } else {
        var digits = phoneNumber.replace(/\D/g, '');
        if (digits.length !== 12) {
            $("#PhoneNumber").next("span").text("Phone Number must be exactly 10 digits!");
            isValid = false;
        } else {
            $("#PhoneNumber").next("span").text("");
        }
    }

    return isValid;
}

function SetExpiryDate() {
    var effectiveDate = $("#EffectiveDate").val();
    if (effectiveDate) {
        var date = new Date(effectiveDate);
        date.setDate(date.getDate() + 15);

        var yyyy = date.getFullYear();
        var mm = ("0" + (date.getMonth() + 1)).slice(-2);
        var dd = ("0" + date.getDate()).slice(-2);

        $("#ExpiryDate").val(yyyy + "-" + mm + "-" + dd);
    }
}

function formatNumber(v) { return (isNaN(v) ? 0 : parseFloat(v)).toFixed(2); }

function CalculateTotal() {

    var fabricGrandTotal = 0;
    var labourGrandTotal = 0;

    $(".room-subtotal-item").each(function () {
        let match = $(this).text().match(/([\d,.]+)/);
        if (match) {
            let subtotalItem = parseFloat(match[1].replace(/,/g, "")) || 0;
            fabricGrandTotal += subtotalItem;
        }
    });
    $(".room-subtotal-labour").each(function () {
        let match = $(this).text().match(/([\d,.]+)/);
        if (match) {
            let subtotalLabour = parseFloat(match[1].replace(/,/g, "")) || 0;
            labourGrandTotal += subtotalLabour;
        }
    });

    // GRAND TOTAL
    $("#FabricGrandTotal").val(formatNumber(fabricGrandTotal));
    $("#LabourGrandTotal").val(formatNumber(labourGrandTotal));
    $("#GrandTotal").val(formatNumber(fabricGrandTotal + labourGrandTotal));

    // FINAL AMOUNT
    CalculateFinalTotal();
}

$(document).on("click", ".remove-room", function () {
    $(this).closest(".room").remove();
    CalculateTotal();
});

$(document).on("keyup change", "#FinalDiscount", function () {
    CalculateFinalTotal();
});

$(document).on("keyup change", "#FinalDiscount, #TransportCharges", function () {
    CalculateFinalTotal();
});

function ItemTemplate() {
    return `
            <div class="room mb-3">
                <div class="room-header row mb-2">
                    <div class="col-6">
                        <input type="text" class="form-control room-item-name" placeholder="Room name (e.g., Hall, Master Bed)">
                    </div>
                    <div class="col-6">
                        <a href="javascript:void(0)" class="btn btn-sm btn-danger btn-compact ms-2 remove-room float-right ml-1">Remove</a>
                        <a href="javascript:void(0)" class="btn btn-sm btn-outline-secondary btn-compact add-labour float-right ml-1">+ Add Labour</a>
                        <a href="javascript:void(0)" class="btn btn-sm btn-outline-primary btn-compact add-item float-right">+ Add Item</a>
                    </div>
                </div>
                <div class="me-3 small-muted room-subtotal-item">Fabric: ₹ 0.00</div>
                 <div class="windows">
                    <div class="table-responsive mt-3">
                        <table class="table table-bordered table-sm item-table">
                            <thead class="thead-light">
                                <tr>
                                    <th class="text-center">Item Name</th>
                                    <th class="text-center">Quantity</th>
                                    <th class="text-center">MRP/RRP</th>
                                    <th class="text-center">Net Rate</th>
                                    <th class="text-center">Final Rate</th>
                                    <th class="text-center">Discount</th>
                                    <th class="text-center">Amount</th>
                                    <th class="text-center" hidden>UnitOfMeasureId</td>
                                    <th class="text-center" hidden>Id</td>
                                    <th class="text-center" hidden>QuotationRoomId</td>
                                    <th class="text-center" colspan="2">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td colspan="12" class="text-center text-danger">No record found!</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="me-3 small-muted room-subtotal-labour">Labour: ₹ 0.00</div>
                <div class="windows">
                    <div class="table-responsive mt-3">
                        <table class="table table-bordered table-sm labour-table">
                            <thead class="thead-light">
                                <tr>
                                    <th class="text-center">Description</th>
                                    <th class="text-center">Quantity</th>
                                    <th class="text-center">Final Rate</th>
                                    <th class="text-center">Amount</th>
                                    <th class="text-center" hidden>UnitOfMeasureId</td>
                                    <th class="text-center" hidden>Id</td>
                                    <th class="text-center" hidden>QuotationRoomId</td>
                                    <th class="text-center" colspan="2">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td colspan="9" class="text-center text-danger">No record found!</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>               
            </div>`;
}

function CalculateFinalTotal() {
    var grandTotal = parseFloat($("#GrandTotal").val()) || 0;
    var discount = parseFloat($("#FinalDiscount").val()) || 0;
    var transportCharges = parseFloat($("#TransportCharges").val()) || 0;

    //grandTotal = fabricTotal + grandLabourTotal

    // FINAL AMOUNT
    //var total = grandTotal - discount;
    var total = grandTotal + transportCharges - discount;

    if (total < 0) {
        total = 0;
    }

    $("#FinalAmount").val(total.toFixed(2));
}
$(document).off('click', '.btn-submit').on('click', '.btn-submit', function (e) {
    var quotationId = $(this).data('quotation-id');
    e.preventDefault();
    fnHandleAjaxRequest({
        url: '/Quotation/CreateQuotationPDFTitle',
        method: 'POST',
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        processData: true,
        data: {
            QuotationId: $(this).data('quotation-id'),
            Id: $(this).data('id'),
            FabricPDFTitle: $('#FabricPDFTitle').val(),
            LabourPDFTitle: $('#LabourPDFTitle').val()
        },
        onSuccess: function (response) {
            if (response.StatusCode === "Ok") {
                showToastr(response.NotificationMsg,response.NotificationType);
                $('#modal-quotation-PDF-Title').modal('hide');
                //Call PDF Download                
                fnGenerateQuote(quotationId);                
            }
            else if (response.StatusCode === "Unauthorized") {
                window.location.href = response.RedirectUrl;
            }
            else {
                showToastr(response.NotificationMsg,response.NotificationType);
            }
        },
        onError: function () {
            showToastr('Internal Server Error!','error');
        }
    });
});

function fnDownloadExcel(base64String) {
    const link = document.createElement("a");
    link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + base64String;
    link.download = "Quotation.xlsx"; // or .xls depending on format
    link.click();
}


function fnGenerateQuote(id) {
    fnHandleAjaxRequest({
        url: `/Quotation/GenerateQuote`,
        type: 'GET',
        data: { id: id },
        dataType: 'json',
        onSuccess: function (response) {
            if (response) {
                if (response.StatusCode == "Ok") {
                    fnDownloadExcel(response.Base64String);
                    $('#divCreate').empty();
                } else if (response.StatusCode == "Unauthorized") {
                    showToastr('error', 'Session Expired! We will redirect you to the login page in 3 seconds.');
                    setTimeout(function () { window.location.href = response.RedirectUrl; }, 3000);
                } else if (response.StatusCode == "BadRequest") {
                    showToastr('error', response.NotificationMsg);
                } else if (response.StatusCode == "InternalServerError") {
                    showToastr('error', response.Message);
                }
            }
        },
        onError: function () {
            showToastr("Failed to generate quote!");
        }
    });
}
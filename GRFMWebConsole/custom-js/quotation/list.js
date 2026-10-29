$(function () {

    $(document).off('click', '.icon-edit').on('click', '.icon-edit', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        if (!id) return;
        window.location.href = '/Quotation/Get/' + id;
    });

    $(document).off('click', '.icon-delete').on('click', '.icon-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnDelete(id);
    });

    $(document).off('click', '.add-rate').on('click', '.add-rate', function (e) {
        e.preventDefault();
        window.location.href = '/Quotation/Create/'
    });


    $(document).off('click', '.icon-excel').on('click', '.icon-excel', function (e) {
        e.preventDefault();
        var quotationid = $(this).data('id');
        var fabricGrandTotal = $(this).data('fabric-grand-total');
        var labourGrandTotal = $(this).data('labour-grand-total');
        fnGetQuotationPDFTitle(quotationid, fabricGrandTotal, labourGrandTotal);
    });


    // Reset pagination and submit the form on Enter key in search fields
    $(document).off('keyup', '.search-text').on('keyup', '.search-text', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            resetPagination();
            $('#frmList').trigger('submit');
        }
    });
    // Submit form when page size filter changes
    $(document).off('change', '.search-dropdown').on('change', '.search-dropdown', function () {
        $('#frmList').trigger('submit');
    });

    $(document).off('change', '.search-pagination').on('change', '.search-pagination', function () {
        $('#frmList').trigger('submit');
    });

    $(document).off('click', '.navigation').on('click', '.navigation', function (e) {
        e.preventDefault();
        var direction = $(this).data('direction').trim();
        if (direction != null && direction != undefined && direction != '') {
            triggerPageNavigation(direction);
        }
        $('#frmList').trigger('submit');
    });
});

function fnDelete(id) {
    fnHandleAjaxRequest({
        url: `/Quotation/Delete/${id}`,
        dataType: 'json',
        onSuccess: function (response) {
            if (response) {
                showToastr(response.NotificationType, response.NotificationMsg);
                $('#frmList').submit();
            }
        },
        onError: function () {
            showToastr("Failed to delete stock.");
        }
    });
}

function fnGetQuotationPDFTitle(id, fabricGrandTotal, labourGrandTotal) {
    fnHandleAjaxRequest({
        url: '/Quotation/CreateQuotationPDFTitle',
        data: { id: id, fabricGrandTotal: fabricGrandTotal, labourGrandTotal: labourGrandTotal },
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-quotation-PDF-Title').modal('show');
        },
        onError: function () {
            showToastr("Failed to load form.");
        }
    });
}

function triggerPageNavigation(btn) {
    const config = window.paginationConfig;
    const currentSkip = parseInt($('#Pagination_Skip').val()) || 0;

    let newSkip = currentSkip;

    switch (btn) {
        case "First":
            if (config.currentPage > 1) newSkip = 0;
            break;

        case "Next":
            if (config.currentPage < config.totalPage) newSkip += config.pageSize;
            break;

        case "Previous":
            if (currentSkip >= config.pageSize) newSkip -= config.pageSize;
            break;

        case "Last":
            if (config.currentPage < config.totalPage) newSkip = (config.totalPage - 1) * config.pageSize;
            break;

        default:
            return;
    }

    if (newSkip !== currentSkip || btn === "First") {
        $('#Pagination_Skip').val(newSkip);
        $('#frmList').submit();
    }


}

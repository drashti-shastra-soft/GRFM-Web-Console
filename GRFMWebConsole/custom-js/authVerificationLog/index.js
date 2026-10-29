$(document).ready(function () {
    $(".btn-submit").click(function () {
        if (fnFormValidation()) {
            $("#frmLogin").submit();
        }
    });

    $(".btn-resent-otp").click(function () {
        fnResendOTP();
    });


    var notificationType = $.trim($('#NotificationType').val());
    var notificationMsg = $.trim($('#NotificationMsg').val());

    if (notificationType != null && notificationType != undefined && notificationType != '') {
        if (notificationMsg != null && notificationMsg != undefined && notificationMsg != '') {
            showToastr(notificationType, notificationMsg);
        }
    }
});

function fnFormValidation() {
    var isValid = true;
    var OTP = $.trim($("#OTP").val());

    if (OTP === "" || OTP == null || OTP == undefined) {
        $("#OTP").next("span").text("OTP is required!");
        $(".LoginFormSection span").removeClass("d-none");
        isValid = false;
    } else if (!/^\d{6}$/.test(OTP)) {
        $("#OTP").next("span").text("OTP must be exactly 6 digits!");
        $(".LoginFormSection span").removeClass("d-none");
        isValid = false;
    } else {
        $("#OTP").next("span").text("");
    }

    return isValid;
}

function fnResendOTP() {
    $("#loading-element").show();
    $.ajax({
        url: '/AuthVerificationLog/ResendOTP',
        type: 'POST',
        success: function (response) {
            $("#loading-element").show();
            if (response.success) {
                showToastr("success", "OTP resent successfully");
            } else {
                showToastr("success", "OTP Resent successfully");
            }
        },
        error: function () {
            showToastr("error", "Something went wrong while resending OTP.");
            $("#loading-element").hide();
        },
        complete: function () {
            $("#loading-element").hide();
        }
    });
}

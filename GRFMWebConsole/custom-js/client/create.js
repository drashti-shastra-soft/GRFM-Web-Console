var emailAddressRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
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

    $('#AlternateNumber').mask('+91 00000 00000', {
        translation: {
            '0': { pattern: /[0-9]/ }
        },
        onKeyPress: function (val, e, field, options) {
            if (!val.startsWith('+91')) {
                field.val('+91 ' + val.replace(/[^\d]/g, '').substring(0, 10).replace(/(\d{5})(\d{0,5})/, '$1 $2'));
            }
        }
    });

    $('#FirstName').on('input', function () {
        this.value = this.value.replace(/[^a-zA-Z\s]/g, '');
    });

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });

    $('#EmailAddress').on('input', function () {
        let val = $(this).val();

        // Allow only valid email characters
        val = val.replace(/[^a-zA-Z0-9@._\-]/g, '');

        // Prevent double @ symbols
        const parts = val.split('@');
        if (parts.length > 2) { val = parts[0] + '@' + parts[1]; }

        $(this).val(val);
    }); 
});

function fnValidation() {
    var isValid = true;

    var name = $.trim($("#Name").val());
    var phoneNumber = $.trim($("#PhoneNumber").val());
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    var emailAddress = $.trim($("#EmailAddress").val());


    // Name
    if (name == null || name == "" || name == undefined) {
        $("#Name").next("span").text("Name is required!");
        isValid = false;
    } else {
        $('#Name').next('span').text("");
    }

    if (phoneNumber == null || phoneNumber == "" || phoneNumber == undefined) {
        $("#PhoneNumber").next("span").text("phone Number is required!");
        isValid = false;
    } else {
        var digits = phoneNumber.replace(/\D/g, '');
        if (digits.length !== 12) {
            $("#PhoneNumber").next("span").text("phone Number must be exactly 10 digits!");
            isValid = false;
        } else {
            $("#PhoneNumber").next("span").text("");
        }
    }

    // Email Address
    if (emailAddress == null || emailAddress == "" || emailAddress == undefined) {
        $("#EmailAddress").next("span").text("Email Address is required!");
        isValid = false;
    } else if (!emailRegex.test(emailAddress)) {
        $("#EmailAddress").next("span").text("Invalid Email Format!");
        isValid = false;
    } else {
        $("#EmailAddress").next("span").text("");
    }

    return isValid;
}

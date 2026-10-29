$(document).ready(function () {

    $(".btn-submit").click(function () {
        if (fnValidation()) {
            $('#frmCreate').submit();
        }
    });
});

function fnValidation() {
    var isValid = true;

    var propertyName = $.trim($("#PropertyNumber").val());
    var clientId = $.trim($("#ClientId").val());
    var societyName = $.trim($("#SocietyName").val());
    var landMark = $.trim($("#LandMark").val());
    var area = $.trim($("#Area").val());
    var city = $.trim($("#City").val());
    var state = $.trim($("#State").val());
    var zipCode = $.trim($("#ZipCode").val());


    // propertyName
    if (propertyName == null || propertyName == "" || propertyName == undefined) {
        $("#PropertyNumber").next("span").text("Property Number is required!");
        isValid = false;
    } else {
        $('#PropertyNumber').next('span').text("");
    }

    // clientId
    if (clientId == null || clientId == "" || clientId == undefined) {
        $("#ClientId").next("span").text("Client is required!");
        isValid = false;
    } else {
        $('#ClientId').next('span').text("");
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
        $("#LandMark").next("span").text("LandMark is required!");
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

    // ZipCode
    if (zipCode == null || zipCode == "" || zipCode == undefined) {
        $("#ZipCode").next("span").text("Zip Code is required!");
        isValid = false;
    } else {
        $('#ZipCode').next('span').text("");
    }
    return isValid;
}

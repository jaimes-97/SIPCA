$(document).ready(function () {
    alert("asd");

    $("#aplicaIVA").click(function () {
        alert("asd");
        if ($("#aplicaIVA").is(':checked')) {
            $("#porcentajeIVA").val(0.15);
        } else {
            
        }
    });

});
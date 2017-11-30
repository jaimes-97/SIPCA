$(function () {

    $("a[data-modal=pedidoDeta]").on("click", function () {
        $("#pedidoDetaModalContent").load(this.href, function () {
            $("#pedidoDetaModal").modal({ keyboard: true }, "show");

            $(document).on("submit", "#pedidoDetachoice", function () {


                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: $(this).serialize(),
                    success: function (result) {
                        if (result.success) {
                            $("#pedidoDetaModal").modal("hide");
                            location.reload();
                        } else {
                            $("#MessageToClient").text("No se pudo actualizar el registro.");
                        }
                    },
                    error: function () {
                        $("#MessageToClient").text("Información inconsistente, no se pudo actualizar el registro .");
                    }
                });
                return false;
            });
        });
        return false;
    });




});









$(function () {

    $("a[data-modal=pedidoDetaElim]").on("click", function () {
        $("#pedidoDetaModalContentElim").load(this.href, function () {
            $("#pedidoDetaModalElim").modal({ keyboard: true }, "show");

            $(document).on("submit", "#pedidoDetachoiceElim", function () {


                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: $(this).serialize(),
                    success: function (result) {
                        if (result.success) {
                            $("#pedidoDetaModalElim").modal("hide");
                            location.reload();
                        } else {
                            $("#MessageToClient").text("No se pudo actualizar el registro.");
                        }
                    },
                    error: function () {
                        $("#MessageToClient").text("Información inconsistente, no se pudo actualizar el registro .");
                    }
                });
                return false;
            });
        });
        return false;
    });




});








function numberWithCommas(yourNumber) {
    //Seperates the components of the number
    var n = yourNumber.toString().split(".");
    //Comma-fies the first part
    n[0] = n[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //Combines the two sections
    return n.join(".");
}

function recalculatePart() {
    var quantity = parseInt(document.getElementById("Cantidad").value).toFixed(0);
    var unitPrice = parseFloat(document.getElementById("PrecioVenta").value).toFixed(4);

    if (isNaN(quantity)) {
        quantity = 0;
    }

    if (isNaN(unitPrice)) {
        unitPrice = 0;
    }

    document.getElementById("Cantidad").value = quantity;
    document.getElementById("PrecioVenta").value = unitPrice;

    document.getElementById("Total").value = numberWithCommas((quantity * unitPrice).toFixed(4));
}

$(document).ready(function () {
    $(document).on('change', '.productSelect', function () {
        var prodId = $('option:selected', this).attr('value');
        $.ajax({
            url: "/DetallePedidoes/GetProductInfo?productId=" + prodId,
            type: 'GET'
        }).done(function (price) {
            $('.tbPrecio').val(price);
        });
    });

    $(document).on('change', '.cantidad', function () {
        recalculatePart();
    });

    $(document).on('mouseover', '.btnSave', function () {
        $('.cantidad').trigger('change');
    });
    //btnSave
});
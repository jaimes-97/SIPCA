$(function () {

    $("a[data-modal=lote]").on("click", function () {
        $("#loteModalContent").load(this.href, function () {
            $("#loteModal").modal({ keyboard: true }, "show");
           
            $(document).on("submit", "#lotechoice", function (e) {
                e.preventDefault();
                var costo = parseFloat($("#Costo").val());
                var subtotal = parseFloat($("#Subtotal").val());
                console.log("Costo: " + costo + " Sub: " + subtotal);

                var lote = {
                    "Existencia": $("#Cantidad").val(),
                    "Cantidad": $("#Cantidad").val(),
                    "CompraId": $("#CompraId").val(),
                    "ProductoId": $("#ProductoId").val(),
                    "aplicaIVA": $("#aplicaIVA").val(),
                    "porcentajeIVA": $("#porcentajeIVA").val(),
                    "Costo": costo,
                    "Subtotal": subtotal

                }
                $.post('/Lotes/Create', lote).done(function (data) {
                    location.reload();
                });
                
               
              /*  var $f= $(e.target)
                var form = $f.serialize();
                console.log(form);
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: form,
                    success: function (result) {
                        if (result.success) {
                            $("#loteModal").modal("hide");
                            location.reload();
                        } else {
                            $("#MessageToClient").text("No se pudo actualizar el registro.");
                        }
                    },
                    error: function () {
                        $("#MessageToClient").text("Información inconsistente, no se pudo actualizar el registro .");
                    }
                });*/
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
    var unitPrice = parseFloat(document.getElementById("Costo").value).toFixed(4);

    if (isNaN(quantity)) {
        quantity = 0;
    }

    if (isNaN(unitPrice)) {
        unitPrice = 0;
    }

    document.getElementById("Cantidad").value = quantity;
    document.getElementById("Costo").value = unitPrice;

    document.getElementById("Subtotal").value = numberWithCommas((quantity * unitPrice).toFixed(4));
}

$(document).ready(function () {
  /*  $(document).on('change', '.productSelect', function () {
        var prodId = $('option:selected', this).attr('value');
        $.ajax({
            url: "/DetalleFacturas/GetProductInfo?productId=" + prodId,
            type: 'GET'
        }).done(function (price) {
            $('.costo').val(price);
        });
    });*/

    $(document).on('change', '.cantidad, .costo', function () {
        recalculatePart();
    });

    $(document).on('mouseover', '.btnSave', function () {
        $('.cantidad').trigger('change');
    });
    //btnSave
});
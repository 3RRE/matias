$(document).ready(function () {   
    $(".listaTbody").mCustomScrollbar({
        autoHideScrollbar: true,
        scrollbarPosition: "outside",
        theme: "dark"
    });
    
    $(document).on("click", ".esconder", function () {
        var tbody = $(this).data("body");
        var texto = $(this).parent().parent().find("input").val();
        ocultartable(tbody);

    });
    $('.eventos input').on('keyup', function (ev) {
        var texto = $(this).val();
        var tbody = $(this).data("body");
        searchTable(tbody, texto);
    });
    $(document).on("click", "div.solofiltro1 tr", function () {
        var input = $(this).find("input");
        var tbody = $(this).parent().attr('id');
        var elemento = tbody.slice(5);

        if (input.is(':checked')) {
            $(input).iCheck('toggle');
            toastr.clear();
        } else {
            $(input).iCheck('toggle');
        }
        $("#cant" + elemento).html($("#" + tbody + " tr").find("input:checked").length + "/" + $("#" + tbody + " tr").length);
    });
    $("#btnGuardarDestinatarios").on("click", function (event) {
        var tipoEmail = $('#txtTipoEmail').val();
        var eventosInsertarArray = [];
        $("#tbodyEventos tr input:checked").each(function (index) {
            eventosInsertarArray.push($(this).val());
        });
        var eventosInsertar = eventosInsertarArray.toString();


        var checkeds = $("#tbodyEventos input:checked");
        var eventosn = [];
        var eventosi = [];
        jQuery.each(checkeds, function () {
            eventosn.push($(this).parent().parent().parent().find("span.spnT").text());
            eventosi.push($(this).val());
        });
        var datafinal = {
            TipoMailID: $('#txtTipoEmail').val(),
            TipoMailNombre: nombretitulo,
            TipoMailEventoID: eventosi.toString(),
            TipoMailEventoNombre: eventosn.toString(),
        };
     
        dataAuditoriaJSON(3, "Destinatario/DestinatarioDetalleInsertarJson", "GUARDAR DESTINATARIO PROGRESIVO", datainicial, datafinal);
        $.ajax({
            type: "POST",
            url: basePath + "Destinatario/DestinatarioDetalleInsertarJson",
            data: { tipoEmail: tipoEmail, DestinatariosId: eventosInsertar },
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            success: function (response) {                
                if (response.data) {
                    $('#modalEventos').modal('toggle');
                    toastr.success("Destinatarios Guardados.");
                } else {
                    toastr.error("Error, de conexión");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $.LoadingOverlay("hide");
                toastr.error("Error, de conexión");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            }
        });
    });
});

$('#modalEventos').on('shown.bs.modal', function (e) {
    var checkeds = $("#tbodyEventos input:checked");
    var eventosn = [];
    var eventosi = [];
    jQuery.each(checkeds, function () {
        eventosn.push($(this).parent().parent().parent().find("span.spnT").text());
        eventosi.push($(this).val());
    });
    datainicial = {
        TipoMailID: $('#txtTipoEmail').val(),
        TipoMailNombre: nombretitulo,
        TipoMailEventoID: eventosi.toString(),
        TipoMailEventoNombre: eventosn.toString(),

    };
   
})

$('#modalEventos').on('hidden.bs.modal', function () {
    setCookie("datainicial", "");
    setCookie("datafinal", "");
});

datainicial = "";
nombretitulo = "";
function mostrarModal(i,nombre) {
    $('#txtTipoEmail').val(i);
    nombretitulo = nombre;
    if (typeof tabladata != "undefined") { tabladata.fnDestroy(); }
    var url = basePath + "Destinatario/DestinatarioListadoTipoEmailJson?TipoEmail=" + i;
    var data = {};
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (jqXhr) {
            $.ajax({
                url: basePath + "Destinatario/DestinatarioListadoJson",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(data),
                beforeSend: function () {
                    $.LoadingOverlay("show");
                },
                complete: function () {
                    $.LoadingOverlay("hide");
                },
                success: function (response) {                    
                    if (response.mensaje) {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    };
                    if (response.data) {
                        $("#tbodyEventos").html("");
                        $.each(response.data, function (index, value) {
                            cambio = false;
                            $.each(jqXhr.data, function (key, qwe) {
                                if (qwe.EmailID == value.EmailID) {
                                    cambio = true;
                                }
                            });
                            check = "";
                            if (cambio == true) {
                                check = "checked";
                            }
                            $("#tbodyEventos").append('<tr>' +
                                '<td style="width:10px"><input type="checkbox" ' + check + ' value="' + value.EmailID + '"></td>' +
                                '<td><span class="spnT">' + value.Email + '</span></td>' +
                                '</tr>');
                        });
                        $("#cantEventos").text(response.data.length);
                    }
                    $(".listaTbody").iCheck({
                        checkboxClass: 'icheckbox_square-orange',
                        radioClass: 'iradio_square-red',
                        increaseArea: '10%', // optional
                    });
                    $("#modalEventos").modal("show");
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    if (jqXHR.status === 0) {
                        toastr.error("Not connect: Verify Network.", "Mensaje Servidor");
                    } else if (jqXHR.status == 404) {
                        toastr.error("Requested page not found [404]", "Mensaje Servidor");
                    } else if (jqXHR.status == 500) {
                        toastr.error("Internal Server Error [500].", "Mensaje Servidor");
                    } else if (textStatus === 'parsererror') {
                        toastr.error("Requested JSON parse failed.", "Mensaje Servidor");
                    } else if (textStatus === 'timeout') {
                        toastr.error("Time out error.", "Mensaje Servidor");
                    } else if (textStatus === 'abort') {
                        toastr.error("Ajax request aborted.", "Mensaje Servidor");
                    }
                }
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status === 0) {
                toastr.error("Not connect: Verify Network.", "Mensaje Servidor");
            } else if (jqXHR.status == 404) {
                toastr.error("Requested page not found [404]", "Mensaje Servidor");
            } else if (jqXHR.status == 500) {
                toastr.error("Internal Server Error [500].", "Mensaje Servidor");
            } else if (textStatus === 'parsererror') {
                toastr.error("Requested JSON parse failed.", "Mensaje Servidor");
            } else if (textStatus === 'timeout') {
                toastr.error("Time out error.", "Mensaje Servidor");
            } else if (textStatus === 'abort') {
                toastr.error("Ajax request aborted.", "Mensaje Servidor");
            }
        }
    });
}
function ocultartable(tbody) {
    $("#div" + tbody).LoadingOverlay("show");
    var elemento = tbody.slice(5);
    var table = $("#" + tbody + " tr");
    $(table).each(function (index) {

        $("td", this).each(function (i) {
            var valorInput = $(this).find('input');
            if (valorInput.length > 0) {
                if (!valorInput.is(':checked')) {
                    $(this).parent().toggle();
                }

            }
        });
    });
    $("#cant" + elemento).html($("#" + tbody + " tr:visible").length + "/" + $("#" + tbody + " tr").length);
    $("#div" + tbody).LoadingOverlay("hide");
};
function searchTable(tbody, inputVal) {
    var table = $('#' + tbody);
    table.find('tr').each(function (index, row) {
        var allCells = $(row).find('td');
        if (allCells.length > 0) {
            var found = false;
            allCells.each(function (index, td) {
                var regExp = new RegExp(inputVal, 'i');
                if (regExp.test($(td).text())) {
                    found = true;
                    return false;
                }
            });
            if (found == true) $(row).show(); else $(row).hide();
        }
    });

    var cantidaEncontrado = $("#" + tbody + " tr:visible").length;
    var cantidadTotal = $("#" + tbody + " tr").length;
    var elemento = tbody.slice(5);
    $("#cant" + elemento).html(cantidaEncontrado + "/" + cantidadTotal);
}
VistaAuditoria("Destinatario/DestinatarioConfiguracionVista", "VISTA", 0, "", 3);
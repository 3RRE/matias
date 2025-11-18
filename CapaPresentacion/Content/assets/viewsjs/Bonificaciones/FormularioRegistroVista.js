$(document).ready(function () {
    codsalaxpagar = 0;
    document.getElementById("nro_documento").focus();

    setCookie("datainicial", "");

    $("#btnBuscar").on("click", function () {
        var nroticket = $("#NroTicket").val();
        if ($("#nro_documento").val() == "") {
            toastr.error("Ingrese Nro. de Documento");
            return false;
        }

        if ($("#NroTicket").val() == "") {
            toastr.error("Ingrese Nro. Ticket");
            return false;
        }
        buscarsala();
    });

    $(document).on('keyup', '#nro_documento', function (e) {
        if (e.keyCode == 13) {
            if ($("#nro_documento").val() == "") {
                toastr.error("Ingrese Nro. de Documento");
                return false;
            }
            if ($("#NroTicket").val() == "") {
                toastr.error("Ingrese Nro. Ticket");
                return false;
            }
            buscarsala();
        }
    });

    $(document).on('keyup', '#NroTicket', function (e) {
        if (e.keyCode == 13) {
            if ($("#nro_documento").val() == "") {
                toastr.error("Ingrese Nro. de Documento");
                return false;
            }
            if ($("#NroTicket").val() == "") {
                toastr.error("Ingrese Nro. Ticket");
                return false;
            }
            buscarsala();
        }
    });

    VistaAuditoria("Bonificacion/FormularioRegistro", "VISTA", 0, "", 3);
});

function buscarsala() {
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboSala").html("");
            datoss = result.data;
            if (datoss.length > 1) {
                var html = '';
                var title = "Mensaje Servidor !";
                var tabla = "";
                $.each(datoss, function (index, value) {
                    tabla += '<tr><td>' + value.CodSala + '</td><td>' + value.Nombre + '</td><td><input type="checkbox" class="chk_sala" name="chk_sala"  value="' + index + '"></td></tr>';
                });

                html = '<table style="font-size:9px;margin-bottom:0px;" class="table table-condensed table-hover table-bordered"><thead><tr><th>ID</th><th>Nombre</th><th></th></tr></thead><tbody>' + tabla + '</tbody></table><div style="font-weight: bolder;">Seleccione Sede para Pago, gracias...</div>';

                $.confirm({
                    title: title,
                    content: html,
                    confirmButton: 'Ok',
                    cancelButton: 'Cerrar',
                    confirmButtonClass: 'btn-info',
                    cancelButtonClass: 'btn-danger',
                    confirm: function () {
                        var checkedsala = $('.chk_sala:checked').length;
                        if (checkedsala == 0) {
                            toastr.error("Seleccione una Sede");
                            return false;
                        }
                        if (checkedsala > 1) {
                            toastr.error("Solo puede seleccionar una");
                            return false;
                        }

                        var i = $('.chk_sala:checked').val();
                        codsalaxpagar = datoss[i].CodSala;
                        buscarTicket();
                    },

                    cancel: function () {
                        //close
                    },

                });
            }
            if (datoss.length == 1) {
                codsalaxpagar = datoss[0].CodSala;
                buscarTicket();
            }
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
    return false;
}

function buscarTicket() {
    if ($("#nro_documento").val() == "") {
        toastr.error("Ingrese Nro. de Documento");
        return false;
    }
    if ($("#NroTicket").val() == "") {
        toastr.error("Ingrese Nro. Ticket");
        return false;
    }
    if (codsalaxpagar == 0) {
        toastr.error("Seleccione Sede");
        return false;
    }
    var nrodocumento = $("#nro_documento").val();
    var nroticket = $("#NroTicket").val();
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Bonificaciones/BuscarTicketSolicitud",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ nrodocumento, nroticket }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            //console.log(response)
            var respuesta = response.respuesta;
            var tickets = response.data;
            if (!respuesta) {
                toastr.error(response.mensaje);
                return false;
            }

            if (tickets.length > 1) {

                var html = '';
                var title = "Mensaje Servidor !";
                var tabla = "";
                $.each(tickets, function (index, value) {
                    tabla += '<tr><td>' + moment(value.bon_fecha).format('DD/MM/YYYY') + '</td><td>' + value.bon_apepaterno + ' ' + value.bon_apematerno + ' ' + value.bon_nombre + '</td><td>' + value.bon_ticket + '</td><td style="text-align:right">' + value.bon_monto.toFixed(2) + '</td><td><input type="checkbox" class="chk_ticketi" name="chk_ticket"  value="' + index + '"></td></tr>';
                });

                html = '<table style=" font-size:9px;margin-bottom:0px;" class="table table-condensed table-hover table-bordered"><thead><tr><th>Fecha Reg.</th><th>Nombre</th><th>NroTicket</th><th>Monto</th><th></th></tr></thead><tbody>' + tabla + '</tbody></table><div style="font-weight: bolder;">Seleccione Ticket a pagar y presione ok, gracias...</div>';

                $.confirm({
                    title: title,
                    content: html,
                    confirmButton: 'Ok',
                    cancelButton: 'Cerrar',
                    confirmButtonClass: 'btn-info',
                    cancelButtonClass: 'btn-danger',
                    confirm: function () {
                        var checkeds = $('.chk_ticketi:checked').length;
                        if (checkeds == 0) {
                            toastr.error("Seleccione un ticket");
                            return false;
                        }
                        if (checkeds > 1) {
                            toastr.error("Solo puede seleccionar un ticket");
                            return false;
                        }

                        var i = $('.chk_ticketi:checked').val();
                        tickets[i].bon_fecha = moment(tickets[i].bon_fecha).format('DD/MM/YYYY hh:mm:ss A');
                        cambiarestadoticket(tickets[i]);

                    },

                    cancel: function () {
                        //close
                    },

                });
            }
            else {
                if (tickets.length == 0) {
                    toastr.error("No se encontro Registro");
                }
                else {
                    var html = '';
                    html = '<table style="font-size:9px;margin-bottom:0px;" class="table table-condensed table-hover table-bordered"><thead><tr><th>Fecha</th><th>NroTicket</th><th>Monto</th></tr></thead><tbody><tr><td>' + moment(tickets[0].bon_fecha).format('DD/MM/YYYY hh:mm:ss A') + '</td><td>' + tickets[0].bon_ticket + '</td><td style="text-align:right">' + tickets[0].bon_monto.toFixed(2) + '</td></tr></tbody></table><div style="font-weight: bolder;">Presione ok, gracias...</div>';

                    $.confirm({
                        title: '¿Esta seguro de Continuar?!',
                        content: html,
                        type: 'red',
                        typeAnimated: true,
                        confirmButton: 'Ok',
                        cancelButton: 'Cerrar',
                        confirmButtonClass: 'btn-info',
                        cancelButtonClass: 'btn-danger',
                        draggable: true,
                        confirm: function () {
                            tickets[0].bon_fecha = moment(tickets[0].bon_fecha).format('DD/MM/YYYY hh:mm:ss A');
                            cambiarestadoticket(tickets[0]);
                        },
                        cancel: function () {

                        },
                    });
                }
            }

            dataAuditoria(1, "#formfiltro", 3, "Bonificaciones/BuscarTicketSolicitud", "BOTON BUSCAR NROTICKET");

        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}


function cambiarestadoticket(bonificaciones) {
    if ($("#nro_documento").val() == "") {
        toastr.error("Ingrese Nro. de Documento");
        return false;
    }
    if ($("#NroTicket").val() == "") {
        toastr.error("Ingrese Nro. Ticket");
        return false;
    }

    if (codsalaxpagar == 0) {
        toastr.error("Seleccione Sede");
        return false;
    }
    bonificaciones.CodSala = codsalaxpagar;
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Bonificaciones/CobrarTicket",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ bonificaciones }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var response = result;
            var respuesta = response.data;
            if (respuesta) {
                $("#NroTicket").val("");
                $("#nro_documento").val("");
                toastr.success(response.mensaje);
            }
            else {
                toastr.error(response.mensaje);
            }
        },
        //error: function (request, status, error) {
        //    toastr.error("Error", "Mensaje Servidor");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
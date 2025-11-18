$(document).ready(function () {
   
    ObtenerListaSalas();
  
    setCookie("datainicial", "");

    $('#cboSala').on('select2:select', function (e) {
        if ($("#cboSala").val() != "") {
            document.getElementById("NroTicket").focus();
        }
    });


    $("#btnBuscar").on("click", function () {
        var nroticket = $("#NroTicket").val();
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala o la url de sala no esta registrada");
            return false;
        }

        if ($("#NroTicket").val() == "") {
            toastr.error("Ingrese Nro. Ticket");
            return false;
        }
        buscarTicket(nroticket);
    });

    $(document).on('keyup', '#NroTicket', function (e) {

        var nroticket = $(this).val();
        if (e.keyCode == 13) {

            if ($("#cboSala").val() == "") {
                toastr.error("Seleccione Sala o la url de sala no esta registrada");
                return false;
            }

            if ($("#NroTicket").val() == "") {
                toastr.error("Ingrese Nro. Ticket");
                return false;
            }
            buscarTicket(nroticket);
        }
    });

    VistaAuditoria("TransferenciasAT/FormularioRegistro", "VISTA", 0, "", 3);
});


function ObtenerListaSalas() {
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
            var datos = result.data;
            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.UrlSalaOnline + '"  data-id="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({});
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

function buscarTicket(nroticket) {
    if ($("#cboSala").val() == "") {
        toastr.error("Seleccione Sala o la url de sala no esta registrada");
        return false;
    }
    var idsala = $("#cboSala").find(':selected').data('id');
    if (idsala == "") {
        toastr.error("Sala Seleccionada no tiene codigo registrado");
        return false;
    }

    url = $("#cboSala").val() + "/ClienteDepositoTransferencia/BuscarTicketATSolicitud";

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "TransferenciaAT/ConsultadataTicket",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url,nroticket }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            //console.log(response)
            var permiso = response.permiso;
            var response = response.data;
            var respuesta = response.respuesta;
            var tickets = response.data;
            if (!respuesta) {
                toastr.error(response.mensaje);
                return false;
            }
           
            if (tickets.length > 1) {

                var html = '';
                var title = "Mensaje Servidor !";
                if (permiso) {
                    title = "Tickets Duplicados !";
                    var tabla = "";
                    $.each(tickets, function (index, value) {
                        tabla += '<tr><td>' + moment(value.Tito_fechaini).format('DD/MM/YYYY hh:mm:ss A') + '</td><td>' + value.Tito_NroTicket + '</td><td style="text-align:right">' + value.Tito_MontoTicket + '</td><td>' + value.CodigoMaquina + '</td><td><input type="checkbox" class="chk_ticketi" name="chk_ticket"  value="' + index+'"></td></tr>';
                    });

                    html = '<table style="font-size:9px;margin-bottom:0px;" class="table table-condensed table-hover table-bordered"><thead><tr><th>Fecha Reg.</th><th>NroTicket</th><th>Monto</th><th>Maquina</th><th></th></tr></thead><tbody>' + tabla+'</tbody></table><div style="font-weight: bolder;">Seleccione Ticket a pagar y presione ok, gracias...</div>';
                }
                else {
                    html = '<form action="" class="">' +
                        '<div class="form-group">' +
                        '<label>Ingrese el monto del ticket</label>' +
                        '<input type="text" id="txtmonto" placeholder="Monto" class="name form-control" required />' +
                        '</div>' +
                        '</form>';
                }

                $.confirm({
                    title: title,
                    content: html,
                    confirmButton: 'Ok',
                    cancelButton: 'Cerrar',
                    confirmButtonClass: 'btn-info',
                    cancelButtonClass: 'btn-danger',
                    confirm: function () {
                        if (permiso) {
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
                            tickets[i].Tito_fechaini = moment(tickets[i].Tito_fechaini).format('DD/MM/YYYY hh:mm:ss A');
                            tickets[i].CodSala = $("#cboSala").find(':selected').data('id');
                            cambiarestadoticket(tickets[i]);
                        }
                        else {
                            var monto = $('#txtmonto').val();
                            if (monto == "") {
                                toastr.error("Ingrese Monto");
                                return false;
                            }

                            $.each(tickets, function (i, val) {
                                if (Number(val.Tito_MontoTicket) == Number(monto)) {
                                    val.Tito_fechaini = moment(Tito_fechaini).format('DD/MM/YYYY hh:mm:ss A');
                                    val.CodSala = $("#cboSala").find(':selected').data('id');
                                    cambiarestadoticket(val);
                                    return false;
                                }
                            });
                            toastr.error("Monto ingresado no concuerda con el monto del Ticket.");
                        }
                        
                    },

                    cancel: function () {
                        //close
                    },

                });
            }
            else {
                if (tickets.length == 0) {
                    toastr.error("No se encontro el Ticket Ingresado");
                }
                else
                {
                    var html = '';
                    if (permiso) {
                        html = '<table style="font-size:9px;margin-bottom:0px;" class="table table-condensed table-hover table-bordered"><thead><tr><th>Fecha Reg.</th><th>NroTicket</th><th>Monto</th><th>Maquina</th></tr></thead><tbody><tr><td>' + moment(tickets[0].Tito_fechaini).format('DD/MM/YYYY hh:mm:ss A') + '</td><td>' + tickets[0].Tito_NroTicket + '</td><td style="text-align:right">' + tickets[0].Tito_MontoTicket + '</td><td>' + tickets[0].CodigoMaquina + '</td></tr></tbody></table><div style="font-weight: bolder;">Seleccione Ticket a pagar y presione ok, gracias...</div>';
                    }


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
                            tickets[0].Tito_fechaini = moment(tickets[0].Tito_fechaini).format('DD/MM/YYYY hh:mm:ss A');
                            tickets[0].CodSala = $("#cboSala").find(':selected').data('id');
                            cambiarestadoticket(tickets[0]);
                        },
                        cancel: function () {

                        },
                    });
                }
            }

            dataAuditoria(1, "#formfiltro", 3, "TransferenciaAT/ConsultadataTicket", "BOTON BUSCAR NROTICKET");
            
        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}


function cambiarestadoticket(ticket) {
    if ($("#cboSala").val() == "") {
        toastr.error("Seleccione Sala o la url de sala no esta registrada");
        return false;
    }
    url = $("#cboSala").val() + "/ClienteDepositoTransferencia/CobrarTicketAT";
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "TransferenciaAT/CobrarTicket",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url, ticket }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var response = result.data;
            var respuesta = response.respuesta;
            if (respuesta) {
                $("#NroTicket").val("");
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
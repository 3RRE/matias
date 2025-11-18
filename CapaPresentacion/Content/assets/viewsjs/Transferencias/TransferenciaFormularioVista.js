$(document).ready(function () {
    ipPublicaG = "";
    dataPendiente = "";
    GProgresivo = "";
    nombretabla = "";
    opcioonurl = "";
    ObtenerListaSalas();

    $(document).on('change', '#cboSala', function (e) {
        var ipPublica = $(this).val();
        ipPublicaG = ipPublica;
    });

    $(document).on('click', '.btnEstado', function (e) {
        var id = $(this).data("id");
        dataPendiente = $(this).data("json");
        $("#TransferenciaID").val(id);
        $("#nom_banco").text(dataPendiente.BancoNombre);
        $("#cuenta_banco").text(dataPendiente.NroCuenta);
        $("#full-modal").modal("show");
        opcioonurl = 0;
    });

    $(document).on('click', '.btnGuardar', function (e) {
        var nrooperacion = $("#NroOperacion").val();
        var fechaoperacion = $("#fecha").val();
        var Observacion = $("#observacion").val();
        var imagen = $("#imagenvoucher").val();
        var monto = $("#monto").val();
        var bancocuenta_id = $("#bancocuenta_id").val();
        var permitido = 2000000;
      
        if (nrooperacion == "") {
            toastr.error("Error, Ingrese Nro de Operacion. ");
            return false;
        }
        if (bancocuenta_id == "") {
            toastr.error("Error, Seleccione Nro. Cuenta. ");
            return false;
        }

        if (monto == "") {
            toastr.error("Error, Ingrese Monto. ");
            return false;
        }
        if (fechaoperacion == "") {
            toastr.error("Error, Ingrese Fecha Operacion .");
            return false;
        }
        if (Observacion == "") {
            toastr.error("Error, Ingrese Observacion .");
            return false;
        }
        if ($("#cboSala").val() == "") {
            toastr.error("Error, Seleccione Sala");
            return false;
        }
        if ($("#cliente_id").val() == 0) {
            toastr.error("Error, Seleccione Cliente");
            return false;
        }
        if (imagen == "") {
            toastr.error("Error, Seleccione Imagen");
            return false;
        }
        var fileSize = $('#imagenvoucher')[0].files[0].size;
        if (fileSize > permitido) {
            toastr.error("Error", "El tamaño de la imagen de demasiado grande (2Mb máximo)");
            return false;
        }
        ipPublicaG = $("#cboSala").val();
        cambiarestadopendiente(opcioonurl);
    });

    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
    setCookie("datainicial", "");
    $(".dateOnly1_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });

    $("#btnBuscar").on("click", function () {
        
        if ($("#fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        buscarTransferenciasPendiente();
    });

    $("#btnNuevo").on("click", function () {
        opcioonurl = 1;
        $("#div_datosolicitud").hide();
        $("#cboSala").select2('destroy'); 
        ObtenerListaSalas();
        $("#tableclientes").hide();
        $("#Nrodocumento").val("");
        $("#NroOperacion").val("");
        $("#bancocuenta_id").val("");
        $("#monto").val("");
        $("#observacion").val("");
        $(".footer_g").show();
        $("#tableclientes").empty();
        $("#cliente_id").val(0);
        $("#SolicitudTransferenciaID").val(0);
        $("#cliente_tipodoc").val("");
        $("#imagenvoucher_div").val("Seleccione Imagen");
        $("#imagenvoucher").val("");
        $("#nom_cliente").text("Ingrese Numero de documento y presione ENTER");
        $("#full-modal_transferencia").modal();
    });

    $("#btnNuevoSolicitud").on("click", function () {
        $("#divsoli").html('<div class="alert alert-danger">Buscando Solicitudes....</div>');
        $("#full-modal_solicitud").modal();
    });

    $('#full-modal_solicitud').on('shown.bs.modal', function (e) {
        var addtabla = $("#divsoli");
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Transferencias/BuscarSolicitudesJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({}),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                console.log(response)
                var response = response.data;
                dataAuditoria(1, "#formfiltro", 3, "Transferencias/BuscarSolicitudes", "BOTON SOLICITUDES");
                $(addtabla).empty();
                $(addtabla).append('<table id="ResumenSolicitudes" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
                objetodatatable = $("#ResumenSolicitudes").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "scrollCollapse": true,
                    "scrollX": false,
                    "sScrollX": "100%",
                    "paging": true,
                    "autoWidth": false,
                    "bAutoWidth": true,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response,
                    columns: [

                        { data: "SolicitudID", title: "ID" },
                        {
                            data: "FechaReg", title: "Fecha",
                            "render": function (value) {
                                return moment(value).format('DD/MM/YYYY HH:mm:ss A');
                            }
                        },
                        { data: "nombresala", title: "Sala" },
                        { data: "TipoDocNombre", title: "Tipo Doc." },
                        { data: "ClienteNroDoc", title: "Nro." },
                        {
                            data: null, title: "Cliente", "render": function (value) {

                                return value.ClienteApelPat + " " + value.ClienteApelMat + " " + value.ClienteNombre;
                            }
                        },
                        { data: "BancoNombre", title: "Banco" },
                        { data: "NroCuenta", title: "Cuenta" },
                        {
                            data: "Monto", title: "Monto", className: "tdright", "render": function (value) {

                                return value.toFixed(2);
                            }
                        },
                        {
                            data: null, title: "Accion",
                            "render": function (value) {
                                var butom = "";
                                butom = '<button type="button" class="btn btn-xs btn-success btnsolicitud" data-id="' + value.SolicitudID + '"><i class="glyphicon glyphicon-pencil"></i> Aprobar</button> '
                                return butom;
                            }
                        }
                    ]
                    ,
                    "initComplete": function (settings, json) {



                    },
                    "drawCallback": function (settings) {

                    }
                });
                $('#ResumenSala tbody').on('click', 'tr', function () {
                    $(this).toggleClass('selected');
                });
            },
            error: function (request, status, error) {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    })

    $(document).on("click", ".btnsolicitud", function () {
        var solicitudid = $(this).data("id");
       
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Transferencias/SolicitudIdJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ id: solicitudid }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
                $("#cboSala").select2('destroy'); 
            },

            success: function (response) {
                console.log("dataa",response.data)
                var response_ = response.data;
                dataAuditoria(1, "#formfiltro", 3, "Transferencias/SolicitudIdJson", "BOTON APROBAR");
                var mensaje = response.mensaje;
                var error = response.errormensaje;
                if (response.respuesta) {
                    $("#div_datosolicitud").show();
                    SeleccionarListaSalas(response_.Codsala);
                    $("#tableclientes").hide();
                    $("#Nrodocumento").val(response_.ClienteNroDoc);
                    $("#NroOperacion").val("");
                    $("#bancocuenta_id").val("");
                    $("#monto").val(response_.Monto);
                    $("#observacion").val("");
                    $(".footer_g").hide();
                    $("#tableclientes").empty();
                    $("#cliente_id").val(0);
                    $("#SolicitudTransferenciaID").val(solicitudid);
                    $("#cliente_tipodoc").val("");
                    $("#imagenvoucher_div").val("Seleccione Imagen");
                    $("#imagenvoucher").val("");
                    var nombre = response_.ClienteApelPat + " " + response_.ClienteApelMat + " " + response_.ClienteNombre;
                    $("#nom_cliente").text("");
                    $("#full-modal_solicitud").modal("hide");

                    $(".fechasol").html(moment(response_.FechaReg).format('DD/MM/YYYY HH:mm:ss A'));
                    $(".salasol").html(response_.nombresala);
                    $(".tipodocsol").html(response_.TipoDocNombre);
                    $(".nrosol").html(response_.ClienteNroDoc);
                    $(".clientesol").html(nombre);
                    $(".bancosol").html(response_.BancoNombre);
                    $(".cuentasol").html(response_.NroCuenta);
                    $(".montosol").html("S/." + response_.Monto.toFixed(2));
                    $("#full-modal_transferencia").modal();
                }
                else {
                    toastr.error("Mensaje Servidor", mensaje + " " + error);
                }

            },
            error: function (request, status, error) {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    });

    $('#full-modal_transferencia').on('shown.bs.modal', function (e) {
        if ($("#div_datosolicitud").is(":visible")) {
            buscarcliente($("#Nrodocumento").val());
            $("#monto").attr('readonly', true);
        }
        else {
            $("#monto").attr('readonly', false);
        }
    })

    $(document).on('keyup', '#Nrodocumento', function (e) {
   
        var texto = $(this).val();
        var url = "";
        var columnas = null;
        url = "transferencia/BuscarClientesJson";
        columnas = [
            { data: "ClienteID", title: "ID" },

            { data: "ClienteTipoDoc", title: "Tipo Doc." },
            { data: "ClienteNroDoc", title: "Nro. Doc" },
            {
                data: null, title: "Cliente", "render": function (value) {

                    return value.ClienteApelPat + " " + value.ClienteApelMat + " " + value.ClienteNombre;
                }
            },
           
            {
                data: null,
                title: "<i class='fa fa-th'></i>",
                className: "tdcenter thcenter",
                width: "10px",
                "bSortable": false,
                "render": function (value, type, w) {
                    var span = `<button type="button" title="SELECT" class="btn btn-warning btn-xs btn_seleccionar_persona" data-id="${value.ClienteID}" data-json='${ JSON.stringify(value)}'  data-id="' + emp_id + '" > <i class="fa fa-check-square-o"></i></button> `;
                    return span;
                }
            }
        ]

        if (e.keyCode == 13) {
            $.ajax({
                type: "POST",
                cache: false,
                url: basePath + "Transferencias/BuscarClientesJson",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ valor:texto }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },

                success: function (response) {
                    console.log(response)
                    response = response.data;
                    dataAuditoria(1, "#formfiltro", 3, "Transferencias/PagarTransferenciaPendiente", "BOTON BUSCAR CLIENTE");
                    if (response.length>0) {
                        $("#tableclientes").show();
                        $(".footer_g").hide();
                        $("#data_cliente").hide();

                        var addtabla = $("#tableclientes");
                        $(addtabla).empty();
                        $(addtabla).append('<table id="clientelista" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
                        objetodatatable = $("#clientelista").DataTable({
                            "bDestroy": true,
                            "bSort": true,
                            "scrollCollapse": true,
                            "scrollX": false,
                            "sScrollX": "100%",
                            "paging": true,
                            "autoWidth": false,
                            "bAutoWidth": true,
                            "bProcessing": true,
                            "bDeferRender": true,
                            data: response,
                            columns: columnas,
                            "initComplete": function (settings, json) {



                            },
                            "drawCallback": function (settings) {

                            }
                        });
                    }
                    else {
                        $("#cliente_id").val(0);
                        $("#cliente_tipodoc").val("");
                        toastr.error("No se encontro data", "Mensaje Servidor");
                    }
                },
                //error: function (request, status, error) {
                //    toastr.error("Error De Conexion, Servidor no Encontrado.");
                //},
                complete: function (resul) {
                    $.LoadingOverlay("hide");
                }
            });


        }
    });

    $(document).on("click", ".btn_seleccionar_persona", function () {
        var id = $(this).data("id");
        dataPendiente = $(this).data("json");
        console.log(dataPendiente)
        var nombre = dataPendiente.ClienteApelPat + " " + dataPendiente.ClienteApelMat + " " + dataPendiente.ClienteNombre;
        var tipodoc = dataPendiente.ClienteTipoDoc;
        $("#tableclientes").hide();
        if (!$("#div_datosolicitud").is(":visible")) {
            $("#monto").val("");
        }
        
        $("#data_cliente").show();
        $(".footer_g").show();
        $("#tableclientes").empty();
        $("#nom_cliente").text(nombre);
        $("#cliente_id").val(id);
        $("#cliente_tipodoc").val(tipodoc);
        
        $.ajax({
            type: "POST",
            url: basePath + "Transferencias/BuscarCuentasClientesJson",
            cache: false,
            data: JSON.stringify({ id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                var datos = result.data;
                $("#bancocuenta_id").html("");
                $("#bancocuenta_id").html('<option value="">--seleccione--</option>');
                var bancospan = $(".bancosol").text();
                var cuentaspan = $(".cuentasol").text();
                $.each(datos, function (index, value) {
                    var select = "";
                    if ($("#div_datosolicitud").is(":visible")) {
                        if (value.Banco == bancospan && value.NroCuenta == cuentaspan) {
                            select = "selected";
                        }
                    }

                    $("#bancocuenta_id").append('<option '+select+' data-banco="' + value.Banco + '" data-cuenta="' + value.NroCuenta + '"  value="' + value.BancoCuentaID + '"> ' + value.Banco + ' ' + value.NroCuenta+'</option > ');
                });
            },
            error: function (request, status, error) {
                toastr.error("Error", "Mensaje Servidor");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });



    });

    VistaAuditoria("Transferencias/Formulario", "VISTA", 0, "", 3);
});

function SeleccionarListaSalas(codsala) {
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
            var datos = result.data;
            
            $("#cboSala").html("");
            $.each(datos, function (index, value) {
                var selecc = "";
                if (value.CodSala == codsala) {
                    selecc = "selected";
                }
                $("#cboSala").append('<option ' + selecc+' value="' + value.UrlSalaOnline + '"  data-id="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                dropdownParent: $("#full-modal_transferencia")
            });
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
            $("#cboSala").select2({
                dropdownParent: $("#full-modal_transferencia")
            });
        },
        //error: function (request, status, error) {
        //    toastr.error("Error", "Mensaje Servidor");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
    return false;
}

function buscarTransferenciasPendiente() {
   
    var fechaini = $("#fechaInicio").val();
    var fechafin = $("#fechaFin").val();
    var addtabla = $(".contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Transferencias/BuscarTransferenciasJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ fechaini, fechafin }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            //console.log(response)
            response = response.data;
            dataAuditoria(1, "#formfiltro", 3, "Transferencias/BuscarTransferenciasson", "BOTON BUSCAR");
            $(addtabla).empty();
            $(addtabla).append('<table id="ResumenSala" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#ResumenSala").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "sScrollX": "100%",
                "paging": true,
                "autoWidth": false,
                "bAutoWidth": true,
                "bProcessing": true,
                "bDeferRender": true,
                data: response,
                columns: [

                    { data: "TransferenciaID", title: "ID" },
                    {
                        data: "FechaReg", title: "Fecha",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY hh:mm A');
                        }
                    },
                    { data: "nombresala", title: "Sala" },
                    { data: "TipoDocNombre", title: "Tipo Doc." },
                    { data: "ClienteNroDoc", title: "Nro." },
                    {
                        data: null, title: "Cliente", "render": function (value) {

                            return value.ClienteApelPat + " " + value.ClienteApelMat + " " + value.ClienteNombre;
                        }
                    },
                    { data: "BancoNombre", title: "Banco" },
                    { data: "NroCuenta", title: "Cuenta" },
                    {
                        data: "Monto", title: "Monto", className: "tdright", "render": function (value) {

                            return value.toFixed(2);
                        }
                    },
                    
                ]
                ,
                "initComplete": function (settings, json) {

                    

                },
                "drawCallback": function (settings) {
                   
                }
            });
            $('#ResumenSala tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function cambiarestadopendiente(est) {
    var url=""
   
    url = ipPublicaG + "/ClienteDepositoTransferencia/GuardarTranferenciaIasCliente";
    
    var TransferenciaID = $("#TransferenciaID").val();
    var NroOperacion = $("#NroOperacion").val();
    var fecha = $("#fecha").val();
    var Observacion = $("#observacion").val();
   // dataPendiente = new Object();
    dataPendiente.Codsala = $("#cboSala option:selected").data("id");
    dataPendiente.BancoNombre = $("#bancocuenta_id option:selected").data("banco");
    dataPendiente.NroCuenta = $("#bancocuenta_id option:selected").data("cuenta");
    dataPendiente.NroOperacion = NroOperacion;
    dataPendiente.Observacion = Observacion;
    dataPendiente.FechaOperacion = fecha;
    dataPendiente.ClienteNroDoc = $("#Nrodocumento").val();
    dataPendiente.monto = $("#monto").val();
    dataPendiente.TipoDocNombre = $("#cliente_tipodoc").val();
    dataPendiente.SolicitudTransferenciaID = $("#SolicitudTransferenciaID").val();
    var data = new FormData();
    var files = $("#imagenvoucher").get(0).files;
    data.append("Image", files[0]); 
   
    data.append("BancoNombre", dataPendiente.BancoNombre); 
    data.append("ClienteApelMat", dataPendiente.ClienteApelMat); 
    data.append("ClienteApelPat", dataPendiente.ClienteApelPat); 
    data.append("ClienteID", dataPendiente.ClienteID); 
    data.append("ClienteNombre", dataPendiente.ClienteNombre); 
    data.append("ClienteNroDoc", dataPendiente.ClienteNroDoc); 
    data.append("ClienteTipoDoc", dataPendiente.ClienteTipoDoc); 
    data.append("Codsala", dataPendiente.Codsala); 
    data.append("FechaAct", dataPendiente.FechaAct);
    data.append("FechaOperacion", dataPendiente.FechaOperacion);
    data.append("FechaReg", dataPendiente.FechaReg);
    data.append("NroCuenta", dataPendiente.NroCuenta);
    data.append("NroOperacion", dataPendiente.NroOperacion);
    data.append("Observacion", dataPendiente.Observacion);
    data.append("TipoDocNombre", dataPendiente.TipoDocNombre);
    data.append("monto", dataPendiente.monto);
    data.append("SolicitudTransferenciaID", dataPendiente.SolicitudTransferenciaID);
    
    data.append("url", url);
    //dataPendiente.TransferenciaSala = dataPendiente.TransferenciaID;
    $.ajax({
        type: "POST",
        method: "POST",
        url: basePath + "Transferencias/PagarTransferenciaPendiente",
        cache: false,
        contentType: false, 
        processData: false,
        data: data,
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            //console.log(response)
            response = response.data;
            dataAuditoria(1, "#formfiltro", 3, "Transferencias/PagarTransferenciaPendiente", "BOTON PENDIENTE");
            if (response == true) {
                $("#full-modal_transferencia").modal("hide");
                $("#NroOperacion").val("");
                $("#fecha").val("");
                $("#observacion").val("");
                $(".dateOnly1_").datetimepicker({
                    pickTime: false,
                    format: 'DD/MM/YYYY',
                    defaultDate: dateNow,
                    maxDate: dateNow,
                });
                toastr.success("Accion realizada Correctamente", "Mensaje Servidor");
                buscarTransferenciasPendiente();
            }
            else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function buscarcliente(texto_) {
    var texto = texto_
    var url = "";
    var columnas = null;
    url = "transferencia/BuscarClientesJson";
    columnas = [
        { data: "ClienteID", title: "ID" },

        { data: "ClienteTipoDoc", title: "Tipo Doc." },
        { data: "ClienteNroDoc", title: "Nro. Doc" },
        {
            data: null, title: "Cliente", "render": function (value) {

                return value.ClienteApelPat + " " + value.ClienteApelMat + " " + value.ClienteNombre;
            }
        },

        {
            data: null,
            title: "<i class='fa fa-th'></i>",
            className: "tdcenter thcenter",
            width: "10px",
            "bSortable": false,
            "render": function (value, type, w) {
                var span = `<button type="button" title="SELECT" class="btn btn-warning btn-xs btn_seleccionar_persona" data-id="${value.ClienteID}" data-json='${JSON.stringify(value)}'  data-id="' + emp_id + '" > <i class="fa fa-check-square-o"></i></button> `;
                return span;
            }
        }
    ]

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Transferencias/BuscarClientesJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ valor: texto }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            console.log(response)
            response = response.data;
            dataAuditoria(1, "#formfiltro", 3, "Transferencias/PagarTransferenciaPendiente", "BOTON BUSCAR CLIENTE");
            if (response.length > 0) {
                $("#tableclientes").show();
                $(".footer_g").hide();
                $("#data_cliente").hide();

                var addtabla = $("#tableclientes");
                $(addtabla).empty();
                $(addtabla).append('<table id="clientelista" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
                objetodatatable = $("#clientelista").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "scrollCollapse": true,
                    "scrollX": false,
                    "sScrollX": "100%",
                    "paging": true,
                    "autoWidth": false,
                    "bAutoWidth": true,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response,
                    columns: columnas,
                    "initComplete": function (settings, json) {



                    },
                    "drawCallback": function (settings) {

                    }
                });
            }
            else {
                $("#cliente_id").val(0);
                $("#cliente_tipodoc").val("");
                toastr.error("No se encontro data", "Mensaje Servidor");
            }
        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

$(document).on("click", "#imagenvoucher_div", function () {
    $('#imagenvoucher').click();
});

$(document).on("change", "#imagenvoucher", function () {
    var nombre = $(this).val();
    if (nombre == "") {
        nombre = "Seleccione Imagen";
    }
    $('#imagenvoucher_div').val(nombre);
});
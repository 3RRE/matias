$(document).ready(function () {
    ipPublicaG = "";
    ObtenerListaSalas();
    ObtenerTipocambio();
    ObtenerEstados();
    $("#cboSala").select2();
    $("#cboTipoCambio").select2();
    $("#cboEstado").select2();

    $(".dateOnly").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });

    $(document).on('change', '#cboSala', function (e) {
        var ipPublica = $("#cboSala").val();
        ipPublicaG = ipPublica;
        //console.log(ipPublicaG);
    });

    $('#btnBuscar').click(function () {
        BuscarSolicitudCambio();
    });
    $('#btnNuevo').click(function () {
        window.location.replace(basePath + "GestionCambios/GestionCambiosNuevoSolicitudCambioVista");
    });

    $(document).on('click', '.btnHistorial', function () {
        
        var id_solicitud = $(this).data("id");        
        $('.contenedor_tabla_modal').html("");
        var redirectUrl = basePath + "GestionCambios/GestionCambiosHistorialSolicitudModal/" + id_solicitud;
        var ubicacion = "contenedor_modaltabla";
        $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
            $("#full-modal").modal("show");
            VistaAuditoria("GestionCambios/GestionCambiosHistorialSolicitudModal", "HISTORIAL", 0, "", 3);
        });
        var table = $('#TablaHistorial').DataTable();
        table.order([1, 'asc']).draw();
    });

    $(document).on('click','.btnLineaTiempo',function () {
        var id_solicitud = $(this).data("id");   
        var url = basePath + "GestionCambios/GestionCambioLineaTiempoHistorialSolicitudVista/" + id_solicitud;
        window.location.replace(url);
    });

    $(document).on('click', '.btnEditar', function () {
        var id_solicitud = $(this).data("id");
        var url = basePath + "GestionCambios/GestionCambiosEditarSolicitudCambioVista/" + id_solicitud;
        window.location.replace(url);
    });

    $(document.body).tooltip({ selector: "[title]" });
});
VistaAuditoria("GestionCambios/GestionCambiosSolicitudCambioVista", "VISTA", 0, "", 1);
function BuscarSolicitudCambio()
{
    if ($("#fechaInicio").val() == "") {
        toastr.error('Ingrese una fecha de Inicio');
        return false;
    }
    if ($("#fechaFin").val() == "") {
        toastr.error('Ingrese una fecha de Inicio');
        return false;
    }
    var datavalidar = $("#txtvalidar").val();
    var url = basePath + "GestionCambios/ObtenerListaSolicitudCambioJson?id_sala="+ $("#cboSala").val() +"&fechaInicio=" + $("#fechaInicio").val() + "&fechaFin=" + $("#fechaFin").val() + "&tipoCambio=" + $("#cboTipoCambio").val() + "&estadoSolicitudCambioId=" + $("#cboEstado").val();
    var data = {}; var respuesta = ""; aaaa = "";
    var addcontenedor = $('.contenedor_tabla');
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
        success: function (response) {
            //console.log(response.data)
            respuesta = response.data;
            $(addcontenedor).empty();
            $(addcontenedor).append('<table id="SolicitudCambio" class="table table-condensed table-bordered table-hover"></table>');

            dataAuditoria(1, "#formfiltro", 3, "GestionCambios/ObtenerListaSolicitudCambioJson", "BOTON BUSCAR");

            objetodatatable = $("#SolicitudCambio").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": true,
                "paging": true,
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                data: response.data,
                columns: [
                    { data: "SolicitudId", title: "Solicitud Id" },
                    { data: "SistemaDescripcion", title: "Sistema" },
                    { data: "ModuloDescripcion", title: "Modulo" },
                    { data: "TipoCambioDescripcion", title: "Tipo Cambio" },
                    { data: "EstadoSolicitudCambioDescripcion", title: "Estado Actual" },
                    {
                        data: "FechaRegistro", title: "Fecha de Registro",
                        render: function (value) {
                            return moment(value).format('DD/MM/YYYY h:mm:ss a');
                        }
                    },
                    {
                        data: null, title: "Accion",
                        render: function (value) {
                            if (datavalidar == 0) {
                                if (value.EstadoSolicitudCambioId != 1) {
                                    return '<button type="button" class="btn btn-sm btn-primary btnHistorial" data-id="' + value.SolicitudId + '"><i class="glyphicon glyphicon-calendar"></i></button> ' +
                                        '<button type="button" class="btn btn-sm btn-warning btnLineaTiempo" data-id="' + value.SolicitudId + '"><i class="glyphicon glyphicon-time"></i></button> ';
                                }
                                else {
                                    return '<button type="button" class="btn btn-sm btn-primary btnHistorial" data-id="' + value.SolicitudId + '"><i class="glyphicon glyphicon-calendar"></i></button> ' +
                                        '<button type="button" class="btn btn-sm btn-warning btnLineaTiempo" data-id="' + value.SolicitudId + '"><i class="glyphicon glyphicon-time"></i></button> ' +
                                        '<button type="button" class="btn btn-sm btn-success btnEditar" data-id="' + value.SolicitudId + '"><i class="glyphicon glyphicon-edit"></i></button>' +
                                        '<button type="button" class="btn btn-sm btn-danger btnEliminar" data-id="' + value.SolicitudId + '"><i class="glyphicon glyphicon-trash"></i></button>';
                                }
                            }
                            else {
                                if (value.EstadoSolicitudCambioId == 1) {
                                    return '<button type="button" class="btn btn-sm btn-primary btnHistorial" data-id="' + value.SolicitudId + '"><i class="glyphicon glyphicon-calendar"></i></button> ' +
                                        '<button type="button" class="btn btn-sm btn-warning btnLineaTiempo" data-id="' + value.SolicitudId + '"><i class="glyphicon glyphicon-time"></i></button> ' +
                                        '<button type="button" class="btn btn-sm btn-success btnEditar" data-id="' + value.SolicitudId + '"><i class="glyphicon glyphicon-edit"></i></button> ' +
                                        '<button type="button" class="btn btn-sm btn-danger btnEliminar" data-id="' + value.SolicitudId + '"><i class="glyphicon glyphicon-trash"></i></button>';
                                } else {
                                    return '<button type="button" class="btn btn-sm btn-primary btnHistorial"  data-id="' + value.SolicitudId + '"><i class="glyphicon glyphicon-calendar"></i></button> ' +
                                        '<button type="button" class="btn btn-sm btn-warning btnLineaTiempo"  data-id="' + value.SolicitudId + '"><i class="glyphicon glyphicon-time"></i></button> ' +
                                        '<button type="button" class="btn btn-sm btn-success btnEditar" data-id="' + value.SolicitudId + '"><i class="glyphicon glyphicon-edit"></i></button> ';
                                }
                                
                            }

                            
                        }
                    }
                ],
                "initComplete": function (settings, json) {

                    $('#btnExcel').off("click").on('click', function () {
                        //if ($("#cboSala").val() == "") {
                        //    toastr.error('Seleccione una Sala');
                        //    return false;
                        //}
                        cabecerasnuevas = [];
                        cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
                        cabecerasnuevas.push({ nombre: "Fecha de Inicio", valor: $("#fechaInicio").val() });
                        cabecerasnuevas.push({ nombre: "Fecha de Fin", valor: $("#fechaFin").val() });

                        var ocultar = ["Accion"];//"Accion";
                        funcionbotonesnuevo({
                            botonobjeto: this, ocultar: ocultar,
                            tablaobj: objetodatatable,
                            cabecerasnuevas: cabecerasnuevas,
                        });
                        VistaAuditoria("GestionCambioSoicitudCambio/SolicitudCambioExcel", "EXCEL", 0, "", 3);
                    });
                    
                },
                "drawCallback": function (settings) {
                    $('.btnHistorial').tooltip({
                        title: "Historial"
                    });

                    $('.btnLineaTiempo').tooltip({
                        title: "Linea de Tiempo"
                    });
                    $('.btnEditar').tooltip({
                        title: "Editar"
                    });
                    $('.btnEliminar').tooltip({
                        title: 'Eliminar'
                    });
                }
            });



            $(".btnEliminar").click(function () {
                var id_solicitud = $(this).data("id");
                $.confirm({
                    icon: 'fa fa-spinner fa-spin',
                    title: 'Esta Seguro que desea Eliminar la solicitud ?',
                    theme: 'black',
                    animationBounce: 1.5,
                    columnClass: 'col-md-6 col-md-offset-3',
                    confirmButtonClass: 'btn-info',
                    cancelButtonClass: 'btn-warning',
                    confirmButton: "confirmar",
                    cancelButton: 'Cerrar',
                    content: "",
                    confirm: function () {
                        var url = basePath + "GestionCambios/GestionCambiosEliminarSolicitudCambioJson/" + id_solicitud;
                        $.ajax({
                            type: "POST",
                            cache: false,
                            url: basePath + "GestionCambios/GestionCambiosEliminarSolicitudCambioJson/" + id_solicitud,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ url: url }),
                            beforeSend: function (xhr) {
                                $.LoadingOverlay("show");
                            },
                            success: function (response) {
                                response = response.data;
                                toastr.success("Se ha eliminado la solicitud " + id_solicitud+" con exito");
                                BuscarSolicitudCambio();
                            },
                            error: function (request, status, error) {
                                toastr.error("Error De Conexion, Servidor no Encontrado.");
                            },
                            complete: function (resul) {
                                $.LoadingOverlay("hide");
                            }
                        });
                    },
                    cancel: function () {

                    },

                });
            });
            
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            //if (xmlHttpRequest.status == 400) {
            //    toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            //} else {
            //    toastr.error("Error Servidor", "Mensaje Servidor");
            //}
        }
    });
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
            var datos = result.data;
            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '" data-id="' + value.CodSala + '">' + value.Nombre + '</option>');
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

function ObtenerTipocambio() {
    $.ajax({
        type: "POST",
        url: basePath + "GestionCambios/ListarTipoCambioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            $.each(datos, function (index, value) {
                $("#cboTipoCambio").append('<option value="' + value.TipoCambioId + '"  data-id="' + value.TipoCambioId + '"  >' + value.Descripcion + '</option>');
            });
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function ObtenerEstados() {
    //ListarEstadoSolicitudCambioJson
    $.ajax({
        type: "POST",
        url: basePath + "GestionCambios/ListarEstadoSolicitudCambioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            $.each(datos, function (index, value) {
                $("#cboEstado").append('<option value="' + value.EstadoSolicitudCambioId + '"  data-id="' + value.EstadoSolicitudCambioId + '"  >' + value.Descripcion + '</option>');
            });
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
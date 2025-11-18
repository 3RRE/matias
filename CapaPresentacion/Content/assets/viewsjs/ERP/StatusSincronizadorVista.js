
VistaAuditoria("StatusSincronizador/StatusSincronizadorVista", "VISTA", 0, "", 1);

var arrayAuxi = [];
var dataAuxi = [];
var fechaAux;

$(document).ready(function () {

    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });

    GetSalas();

    $(document).on('click', '#btnBuscar', function (e) {
        ListadoStatusSincronizador();
    });

    $(document).on('click', '#btnExcel', function (e) {
        GenerarExcelStatusSincronizador();
    });

    $(document).on('change', '#cboSala', function (e) {
        arrayAuxi = [];
        renderDataTable([]);
    });

    $(document).on('click', '.btnCierre', function (e) {

        let fecha = $(this).attr("data-fecha");
        var js1 = $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Esta seguro que desea enviar el cierre?',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: "Si",
            cancelButton: 'No',
            content: "",
            confirm: function () {

                UpdateStatusSincronizador(fecha);

            },
            cancel: function () {

            },

        });

    });

    $(document).on('click', '.btnAbrirCierre', function (e) {

        let fecha = $(this).attr("data-fecha");
        var js1 = $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Esta seguro que desea habilitar el cierre?',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: "Si",
            cancelButton: 'No',
            content: "",
            confirm: function () {

                AbrirCierreStatusSincronizador(fecha);

            },
            cancel: function () {

            },

        });

    });

    $(document).on('click', '.btnSincronizar', function (e) {

        let fecha = $(this).attr("data-fecha");

        var js2 = $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Esta seguro que desea sincronizar ? Esto puede tardar hasta 15 minutos',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: "Si",
            cancelButton: 'No',
            content: "",
            confirm: function () {

                SendStatusSincronizador(fecha, false);

            },
            cancel: function () {

            },

        });

    });


    $(document).on('click', '.btnSincronizarTabla', function (e) {
        
        let tabla = $(this).attr("data-tabla");

        var js2 = $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Esta seguro que desea sincronizar la tabla ' + tabla + '? Esto puede tardar hasta unos minutos',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: "Si",
            cancelButton: 'No',
            content: "",
            confirm: function () {

                SendStatusSincronizadorTabla(tabla);

            },
            cancel: function () {

            },

        });

    });

        $(document).on('click', '.btnComparar', function (e) {

            let fecha = $(this).attr("data-fecha");
            fechaAux = fecha;
            MatchStatusSincronizador(fecha);

        });

        $(document).on('click', '#btnArreglar', function (e) {

            var js2 = $.confirm({
                icon: 'fa fa-spinner fa-spin',
                title: '¿Esta seguro que desea volver a sincronizar para corregir la informacion?  Esto puede tardar hasta 15 minutos',
                theme: 'black',
                animationBounce: 1.5,
                columnClass: 'col-md-6 col-md-offset-3',
                confirmButtonClass: 'btn-info',
                cancelButtonClass: 'btn-warning',
                confirmButton: "Si",
                cancelButton: 'No',
                content: "",
                confirm: function () {

                    SendStatusSincronizador(fechaAux, true);

                },
                cancel: function () {

                },

            });

        });

        $("#full-modal_detalle").on("shown.bs.modal", function () {

            renderDataTableModal(dataAuxi);

        });

    });


    function GetSalas() {


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
                    $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
                });
                $("#cboSala").select2({
                    placeholder: "--Seleccione--"

                });
                $("#cboSala").val(null).trigger("change");
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


    function ListadoStatusSincronizador() {

        var codSala = $("#cboSala").val();
        var fechaIni = $("#fechaIni").val();
        var fechaFin = $("#fechaFin").val();

        if (codSala == null) {
            toastr.error("Selecione una sala.", "Mensaje Sistema");
            return false;
        }

        var parametros = JSON.stringify({ salaId: codSala, fechaIni, fechaFin });

        ajaxhr = $.ajax({
            type: "post",
            cache: false,
            url: basePath + "StatusSincronizador/GetStatusSincronizador",
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                if (result.success) {
                    renderDataTable(result.data)
                    arrayAuxi = result.data;
                    toastr.success(result.message, "Mensaje Sistema");
                } else {
                    toastr.error(result.message, "Mensaje Sistema");
                }
            },
            error: function (request, status, error) {
                toastr.error(request.responseText + " " + error, "Mensaje Sistema");
            },
            complete: function (resul) {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            }
        });
        AbortRequest.open()

    }

    function renderDataTable(array) {

        objetodatatable = $("#table").DataTable({
            "bDestroy": true,
            "ordering": true,
            "scrollCollapse": true,
            "scrollX": true,
            "paging": true,
            "aaSorting": [],
            "autoWidth": true,
            "bAutoWidth": true,
            "bProcessing": true,
            "bDeferRender": true,
            "searching": true,
            "bInfo": true, //Dont display info e.g. "Showing 1 to 4 of 4 entries"      
            "bPaginate": true,//Dont want paging      
            data: array
            ,
            columns: [
                {
                    data: "fechaoperacion", title: "Fecha Operacion",
                    "render": function (o) {

                        return moment(o).format('DD/MM/YYYY');
                    },
                },
                {
                    data: "contadores", title: "Contadores",
                    "render": function (o) {

                        var estado = "NO LISTO";
                        var css = "btn-danger";

                        if (o) {
                            estado = "LISTO";
                            css = "btn-success";
                        }

                        return '<span class="label ' + css + '" style="width:100%; display:block;padding:5px 0">' + estado + '</span>';
                    },
                },
                {
                    data: "cuadreticket", title: "Cuadre Ticket",
                    "render": function (o) {

                        var estado = "NO LISTO";
                        var css = "btn-danger";

                        if (o) {
                            estado = "LISTO";
                            css = "btn-success";
                        }

                        return '<span class="label ' + css + '" style="width:100%; display:block;padding:5px 0">' + estado + '</span>';
                    },
                },
                {
                    data: "caja", title: "Caja",
                    "render": function (o) {

                        var estado = "NO LISTO";
                        var css = "btn-danger";

                        if (o) {
                            estado = "LISTO";
                            css = "btn-success";
                        }

                        return '<span class="label ' + css + '" style="width:100%; display:block;padding:5px 0">' + estado + '</span>';
                    },
                },
                {
                    data: "enviado", title: "Enviado",
                    "render": function (o) {

                        var estado = "NO LISTO";
                        var css = "btn-danger";

                        if (o) {
                            estado = "LISTO";
                            css = "btn-success";
                        }

                        return '<span class="label ' + css + '" style="width:100%; display:block;padding:5px 0">' + estado + '</span>';
                    },
                },
                {
                    data: "fechacierre", title: "Fecha cierre",
                    "render": function (o) {

                        return moment(o).format('DD/MM/YYYY HH:mm:ss a');
                    },
                },
                {
                    data: null, title: "Acciones",
                    "render": function (value) {
                        var botones = '<button type="button" class="btn btn-xs btn-info btnCierre" data-toggle="modal" data-target="#full-modal" data-fecha="' + moment(value.fechaoperacion).format('DD/MM/YYYY') + '">Cierre</button>';
                        botones += '&nbsp <button type="button" class="btn btn-xs btn-success btnAbrirCierre" data-toggle="modal" data-target="#full-modal" data-fecha="' + moment(value.fechaoperacion).format('DD/MM/YYYY') + '">Habilitar cierre</button>';
                        botones += '&nbsp <button type="button" class="btn btn-xs btn-primary btnSincronizar" data-toggle="modal" data-target="#full-modal" data-fecha="' + moment(value.fechaoperacion).format('DD/MM/YYYY') + '">Sincronizar</button>';
                        botones += '&nbsp <button type="button" class="btn btn-xs btn-danger btnComparar" data-toggle="modal" data-fecha="' + moment(value.fechaoperacion).format('DD/MM/YYYY') + '">Comparar</button>';
                        return botones;
                    },
                    sortable: false,
                    searchable: false
                }

            ],
            "drawCallback": function (settings) {
            },

            "initComplete": function (settings, json) {

            },
        });
    }


    function UpdateStatusSincronizador(fecha) {

        var codSala = $("#cboSala").val();

        if (codSala == null) {
            toastr.error("Selecione una sala.", "Mensaje Sistema");
            return false;
        }

        var parametros = JSON.stringify({ salaId: codSala, fecha });

        ajaxhr = $.ajax({
            type: "post",
            cache: false,
            url: basePath + "StatusSincronizador/UpdateStatusSincronizador",
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                if (result.success) {
                    toastr.success(result.message, "Mensaje Sistema");
                    ListadoStatusSincronizador();
                } else {
                    toastr.error(result.message, "Mensaje Sistema");
                }
            },
            error: function (request, status, error) {
                toastr.error(request.responseText + " " + error, "Mensaje Sistema");
            },
            complete: function (resul) {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            }
        });
        AbortRequest.open()

    }


    function AbrirCierreStatusSincronizador(fecha) {

        var codSala = $("#cboSala").val();

        if (codSala == null) {
            toastr.error("Selecione una sala.", "Mensaje Sistema");
            return false;
        }

        var parametros = JSON.stringify({ salaId: codSala, fecha });

        ajaxhr = $.ajax({
            type: "post",
            cache: false,
            url: basePath + "StatusSincronizador/AbrirCierreStatusSincronizador",
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                if (result.success) {
                    toastr.success(result.message, "Mensaje Sistema");
                    ListadoStatusSincronizador();
                } else {
                    toastr.error(result.message, "Mensaje Sistema");
                }
            },
            error: function (request, status, error) {
                toastr.error(request.responseText + " " + error, "Mensaje Sistema");
            },
            complete: function (resul) {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            }
        });
        AbortRequest.open()

    }



    function SendStatusSincronizador(fecha, recargar) {

        var codSala = $("#cboSala").val();

        if (codSala == null) {
            toastr.error("Selecione una sala.", "Mensaje Sistema");
            return false;
        }

        var parametros = JSON.stringify({ salaId: codSala, fecha });

        ajaxhr = $.ajax({
            type: "post",
            cache: false,
            url: basePath + "StatusSincronizador/SendStatusSincronizador",
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                if (result.success) {
                    toastr.success(result.message, "Mensaje Sistema");

                    if (recargar) {
                        MatchStatusSincronizador(fecha);
                    } else {
                        ListadoStatusSincronizador();
                    }

                } else {
                    toastr.error(result.message, "Mensaje Sistema");
                }
            },
            error: function (request, status, error) {
                toastr.error(request.responseText + " " + error, "Mensaje Sistema");
            },
            complete: function (resul) {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            }
        });
        AbortRequest.open()

    }

    function MatchStatusSincronizador(fecha) {

        var codSala = $("#cboSala").val();

        if (codSala == null) {
            toastr.error("Selecione una sala.", "Mensaje Sistema");
            return false;
        }

        var parametros = JSON.stringify({ salaId: codSala, fecha });

        var nombreSala = $("#cboSala").find(":selected").text();

        ajaxhr = $.ajax({
            type: "post",
            cache: false,
            url: basePath + "StatusSincronizador/MatchStatusSincronizador",
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $("#textTitle").text(nombreSala + " - " + fecha);
                renderDataTableModal([]);
                $("#full-modal_detalle").modal("hide");
                $.LoadingOverlay("show");
            },
            success: function (result) {
                if (result.success) {
                    toastr.success(result.message, "Mensaje Sistema");
                    dataAuxi = result.data;
                    $("#full-modal_detalle").modal("show");
                } else {
                    toastr.error(result.message, "Mensaje Sistema");
                }
            },
            error: function (request, status, error) {
                $("#full-modal_detalle").modal("hide");
                toastr.error(request.responseText + " " + error, "Mensaje Sistema");
            },
            complete: function (resul) {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            }
        });
        AbortRequest.open()

    }



    function GenerarExcelStatusSincronizador() {

        var nombreSala = $("#cboSala option:selected").text();

        if ($("#cboSala").val() == null) {
            toastr.error("Selecione una sala.", "Mensaje Sistema");
            return false;
        }

        if (arrayAuxi.length === 0) {
            toastr.error("No hay informacion para generar el reporte", "Mensaje Sistema");
            return false;
        }


        var parametros = JSON.stringify({ lista: arrayAuxi, nombreSala });
        ajaxhr = $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "StatusSincronizador/ListadoStatusSincronizadorExcelJson",
            contentType: "application/json; charset=utf-8",
            data: parametros,
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                if (response.respuesta) {
                    var data = response.data;
                    var file = response.excelName;
                    let a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/vnd.ms-excel;base64, " + data;
                    a.download = file;
                    a.click();
                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
                AbortRequest.close()
            }
        });
        AbortRequest.open()
    }


    function renderDataTableModal(array) {

        objetodatatable = $("#tableDetalleStatus").DataTable({
            "bDestroy": true,
            "ordering": true,
            "scrollCollapse": true,
            "scrollX": true,
            "paging": true,
            "aaSorting": [],
            "autoWidth": true,
            "bAutoWidth": true,
            "bProcessing": true,
            "bDeferRender": true,
            "searching": true,
            "bInfo": true, //Dont display info e.g. "Showing 1 to 4 of 4 entries"      
            "bPaginate": true,//Dont want paging      
            data: array
            ,
            columns: [
                {
                    data: "nameTable", title: "Tabla"
                },
                {
                    data: "countData", title: "Registros obtenidos"
                },
                {
                    data: "countTable", title: "Registros migrados",
                    "render": function (o, type, oData) {

                        var estado = "NO LISTO";
                        var css = "btn-danger";

                        if (oData.countData == o) {
                            estado = "LISTO";
                            css = "btn-success";
                        }

                        return '<span class="label ' + css + '" style="width:100%; display:block;padding:5px 0">' + o + '</span>';
                    },
                },
                {
                    data: null, title: "Sincronizar",
                    "render": function (value) {
                        var botones = '<button type="button" style="width:100%;" class="btn btn-xs btn-primary btnSincronizarTabla" data-toggle="modal" data-target="#full-modal" data-tabla="' + value.nameTable + '">Sincronizar</button>';
                        return botones;
                    },
                    sortable: false,
                    searchable: false
                }

            ],
            "drawCallback": function (settings) {
            },

            "initComplete": function (settings, json) {

            },
        });
    }



    function SendStatusSincronizador(fecha, recargar) {

        var codSala = $("#cboSala").val();

        if (codSala == null) {
            toastr.error("Selecione una sala.", "Mensaje Sistema");
            return false;
        }

        var parametros = JSON.stringify({ salaId: codSala, fecha });

        ajaxhr = $.ajax({
            type: "post",
            cache: false,
            url: basePath + "StatusSincronizador/SendStatusSincronizador",
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                if (result.success) {
                    toastr.success(result.message, "Mensaje Sistema");

                    if (recargar) {
                        MatchStatusSincronizador(fecha);
                    } else {
                        ListadoStatusSincronizador();
                    }

                } else {
                    toastr.error(result.message, "Mensaje Sistema");
                }
            },
            error: function (request, status, error) {
                toastr.error(request.responseText + " " + error, "Mensaje Sistema");
            },
            complete: function (resul) {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            }
        });
        AbortRequest.open()

    }
    function SendStatusSincronizadorTabla(tabla) {

        var codSala = $("#cboSala").val();

        if (codSala == null) {
            toastr.error("Selecione una sala.", "Mensaje Sistema");
            return false;
        }

        var parametros = JSON.stringify({ salaId: codSala, fecha:fechaAux, tabla });

        ajaxhr = $.ajax({
            type: "post",
            cache: false,
            url: basePath + "StatusSincronizador/SendStatusSincronizadorTabla",
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                if (result.success) {
                    toastr.success(result.message, "Mensaje Sistema");

                    let objIndex = dataAuxi.findIndex(obj => obj.nameTable == result.data[0].nameTable);
                    if (objIndex) {
                        dataAuxi[objIndex].countData = result.data[0].countData;
                        dataAuxi[objIndex].countTable = result.data[0].countTable;
                        renderDataTableModal(dataAuxi)
                    }

                } else {
                    toastr.error(result.message, "Mensaje Sistema");
                }
            },
            error: function (request, status, error) {
                toastr.error(request.responseText + " " + error, "Mensaje Sistema");
            },
            complete: function (resul) {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            }
        });
        AbortRequest.open();
    }

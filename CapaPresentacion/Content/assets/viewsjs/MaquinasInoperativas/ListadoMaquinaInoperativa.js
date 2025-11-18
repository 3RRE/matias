
var ListaPiezasId = 0
var ListaProblemasId = 0;
var ListaRepuestosId = 0;

$(document).ready(function () {

    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });

    //$(document).on('dp.change', '#fechaIni', function (e) {
    //    $('#fechaFin').data("DateTimePicker").setMinDate(e.date)
    //})
    //$(document).on('dp.change', '#fechaFin', function (e) {
    //    $('#fechaIni').data("DateTimePicker").setMaxDate(e.date)
    //})

    //$("#fechaIni").datetimepicker({
    //    pickTime: false,
    //    format: 'DD/MM/YYYY',
    //    defaultDate: dateNow,
    //    maxDate: dateNow,
    //});

    //$("#fechaFin").datetimepicker({
    //    pickTime: false,
    //    format: 'DD/MM/YYYY',
    //    defaultDate: dateNow,
    //    minDate: dateNow,
    //    maxDate: dateNow,
    //});

    //ListarMaquinaInoperativa();
    ListarMaquinaInoperativaxFechas();

    VistaAuditoria("MIMaquinaInoperativa/ListadoMaquinaInoperativa", "VISTA", 0, "", 3);



    $(document).on("click", "#btnBuscar", function () {

        if ($("#fechaIni").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if ($("#fechaFin").val() == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }

        ListarMaquinaInoperativaxFechas();

    });

    $(document).on("click", "#btnExcel", function () {

        if ($("#fechaIni").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if ($("#fechaFin").val() == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }
        GenerarExcelxFechas();

    });

    $(document).on("click", ".btnNuevo", function () {

        window.location.replace(basePath + "MIMaquinaInoperativa/MaquinaInoperativaInsertarVista");

    });


    $(document).on("click", ".btnDetalle", function () {

        let id = $(this).data("id");
        let url = basePath + "MIMaquinaInoperativa/HistoricoMaquinaInoperativa/" + id;
        window.location.replace(url);

    });


    $(document).on("click", ".btnAtencion", function () {

        let id = $(this).data("id");
        let url = basePath + "MIMaquinaInoperativa/AtencionMaquinaInoperativa/" + id;
        window.location.replace(url);

    });

    $(document).on("click", ".btnDetalleAtencion", function () {

        let id = $(this).data("id");
        let url = basePath + "MIMaquinaInoperativa/ListadoDetalleAtencionMaquinaInoperativa/" + id;
        window.location.replace(url);

    });

    $(document).on("click", ".btnPiezas", function () {

        var id = $(this).data("id");
        ListaPiezasId = id;
        $("#full-modal_pieza_maquina_inoperativa").modal("show");

    });


    $(document).on('shown.bs.modal', '#full-modal_pieza_maquina_inoperativa', function () {

        GetListaPiezas(ListaPiezasId);
    });

    $(document).on("click", ".btnProblemas", function () {

        var id = $(this).data("id");
        ListaProblemasId = id;
        $("#full-modal_problema_maquina_inoperativa").modal("show");

    });

    $(document).on('shown.bs.modal', '#full-modal_problema_maquina_inoperativa', function () {

        GetListaProblemas(ListaProblemasId);
    });

    $(document).on("click", ".btnRepuestos", function () {

        var id = $(this).data("id");
        ListaRepuestosId = id;
        $("#full-modal_repuesto_maquina_inoperativa").modal("show");

    });

    $(document).on('shown.bs.modal', '#full-modal_repuesto_maquina_inoperativa', function () {

        GetListaRepuestos(ListaRepuestosId);
    });



    $('#cboFiltroEstado').on('change', function () {
        objetodatatable = $("#tableMaquinaInoperativa").DataTable({

            "fixedColumns": {
                "leftColumns": 3
            },
            "bDestroy": true,
            "bSort": true,
            "scrollCollapse": true,
            "scrollX": false,
            "sScrollX": "100%",
            "paging": true,
            "ordering": true,
            "aaSorting": [],
            "autoWidth": false,
            "bAutoWidth": true,
            "bProcessing": true,
            "bDeferRender": true,
            data: [],
            columns: [
                { data: "CodMaquinaInoperativa", title: "ID" },
                { data: "MaquinaLey", title: "CodMaquina" },
                { data: "NombreSala", title: "Sala" },
                //{ data: "TecnicoCreado", title: "Tecnico", visible: false },
                //{ data: "ObservacionCreado", title: "Observacion", visible: false },
                //{
                //    data: "CodEstadoInoperativa", title: "Estado Inoperativa",
                //    "render": function (o) {
                //        var estado = "";
                //        var css = "btn-danger";
                //        if (o == 1) {
                //            estado = "Op. Problemas"
                //            css = "btn-success";
                //        }
                //        if (o == 2) {
                //            estado = "Inoperativa"
                //            css = "btn-danger";
                //        }
                //        if (o == 3) {
                //            estado = "Atendida en Sala"
                //            css = "btn-info";
                //        }
                //        return '<span class="label ' + css + ' "style="width:100%; display:block;padding:5px 0"> ' + estado + '</span>';

                //    }
                //},
                //{
                //    data: "CodPrioridad", title: "Prioridad",
                //    "render": function (o) {
                //        var estado = "";
                //        var css = "btn-danger";
                //        if (o == 1) {
                //            estado = "Baja"
                //            css = "btn-success";
                //        }
                //        if (o == 2) {
                //            estado = "Media"
                //            css = "btn-info";
                //        }
                //        if (o == 3) {
                //            estado = "Alta"
                //            css = "btn-danger";
                //        }
                //        return '<span class="label ' + css + '"style="width:100%; display:block;padding:5px 0">' + estado + '</span>';

                //    },

                //},
                //{
                //    data: "FechaModificacion", title: "Fecha Modificacion",
                //    "render": function (o) {
                //        return moment(o).format("DD/MM/YYYY hh:mm");
                //    },
                //    visible: false
                //},
                //{ data: "CodEstadoProceso", title: "Estado Proceso", visible: false },
                //{
                //    data: "FechaEnvioSala", title: "Fecha Envio Sala",
                //    "render": function (o) {
                //        return moment(o).format("DD/MM/YYYY hh:mm");
                //    },
                //    visible: false
                //},
                //{
                //    data: "FechaRecepcionCentral", title: "Fecha Recepcion Central",
                //    "render": function (o) {
                //        return moment(o).format("DD/MM/YYYY hh:mm");
                //    },
                //    visible: false
                //},
                //{
                //    data: "FechaEnvioCentral", title: "Fecha Envio Central",
                //    "render": function (o) {
                //        return moment(o).format("DD/MM/YYYY hh:mm");
                //    },
                //    visible: false
                //},
                //{
                //    data: "ObservacionAtencion", title: "Observacion Atencion", visible: false
                //},
                //{
                //    data: "FechaRecepcionSala", title: "Fecha Recepcion Sala",
                //    "render": function (o) {
                //        return moment(o).format("DD/MM/YYYY hh:mm");
                //    },
                //    visible: false
                //},
                //{
                //    data: "FechaOperativaSala", title: "Fecha Operativa Sala",
                //    "render": function (o) {
                //        return moment(o).format("DD/MM/YYYY hh:mm");
                //    },
                //    visible: false
                //},
                //{
                //    data: "ObservacionOperatividadSala", title: "Observacion Operatividad Sala",
                //    visible: false
                //},
                //{
                //    data: "FechaModificacion", title: "Fecha Modificacion",
                //    "render": function (o) {
                //        return moment(o).format("DD/MM/YYYY hh:mm");
                //    }
                //},
                {
                    data: "FechaCreado", title: "Fecha Creado",
                    "render": function (o) {
                        return moment(o).format("DD/MM/YYYY");
                    },
                },
                {
                    data: "NombreUsuarioCreado", title: "Usuario",
                    visible: false
                },
                {
                    data: "CodEstadoProceso", title: "Estado",
                    "render": function (o) {
                        var estado = "CREADO";
                        var css = "btn-info";
                        if (o == 3) {
                            estado = "ATENDIDA INOPERATIVA"
                            css = "btn-danger";
                        }
                        else if (o == 2) {
                            estado = "ATENDIDA OPERATIVA"
                            css = "btn-primary";
                        }
                        else if (o == 4) {
                            estado = "EN ESPERA SOLICITUD"
                            css = "btn-warning";
                        }
                        else if (o == 5) {
                            estado = "REPUESTOS AGREGADOS"
                            css = "btn-primary";
                        }
                        return '<span class="label ' + css + '" style="width:100%; display:block;padding:5px 0">' + estado + '</span>';

                    }
                },
                {
                    data: "CodMaquinaInoperativa", title: "Acción",
                    "bSortable": false,
                    "render": function (o, type, oData) {

                        return `<button type="button" class="btn btn-xs btn-warning btnDetalle" data-id="${o}"> <span class="glyphicon glyphicon-list-alt" style="margin-right: 5px;"></span>VER HISTORICO</i></button>`;
                    },
                }
            ],
            "drawCallback": function (settings) {
                $('.btnDetalle').tooltip({
                    title: "Historico"
                });
            },

            "initComplete": function (settings, json) {



            },
        });
    })
    $('#tec').on('change', function () {
        if (this.checked) {
            objetodatatable.column(6).visible(true);
        }
        else {
            objetodatatable.column(6).visible(false);
        }
    })
    $('#fmod').on('change', function () {
        if (this.checked) {
            objetodatatable.column(7).visible(true);
        }
        else {
            objetodatatable.column(7).visible(false);
        }
    })
    $('#espro').on('change', function () {
        if (this.checked) {
            objetodatatable.column(8).visible(true);
        }
        else {
            objetodatatable.column(8).visible(false);
        }
    })
    $('#envSa').on('change', function () {
        if (this.checked) {
            objetodatatable.column(9).visible(true);
        }
        else {
            objetodatatable.column(9).visible(false);
        }
    })
    $('#fere').on('change', function () {
        if (this.checked) {
            objetodatatable.column(10).visible(true);
        }
        else {
            objetodatatable.column(10).visible(false);
        }
    })
    $('#feencen').on('change', function () {
        if (this.checked) {
            objetodatatable.column(11).visible(true);
            console.log(objetodatatable.column(11).visible())
        }
        else {
            objetodatatable.column(11).visible(false);
        }
    })
    $('#obsaten').on('change', function () {
        if (this.checked) {
            objetodatatable.column(12).visible(true);
        }
        else {
            objetodatatable.column(12).visible(false);
        }
    })
    $('#fRecepcionSala').on('change', function () {
        if (this.checked) {
            objetodatatable.column(13).visible(true);
        }
        else {
            objetodatatable.column(13).visible(false);
        }
    })
    $('#fOperativaSala').on('change', function () {
        if (this.checked) {
            objetodatatable.column(14).visible(true);
        }
        else {
            objetodatatable.column(14).visible(false);
        }
    })
    $('#obsOperatividadSala').on('change', function () {
        if (this.checked) {
            objetodatatable.column(15).visible(true);
        }
        else {
            objetodatatable.column(15).visible(false);
        }
    })
    $('#fRegistro').on('change', function () {
        if (this.checked) {
            objetodatatable.column(16).visible(true);
        }
        else {
            objetodatatable.column(16).visible(false);
        }
    })
    //$('#fModficacion').on('change', function () {
    //    if (this.checked) {
    //        objetodatatable.column(17).visible(true);
    //    }
    //    else {
    //        objetodatatable.column(17).visible(false);
    //    }
    //})
    $('#usuarrio').on('change', function () {
        if (this.checked) {
            objetodatatable.column(17).visible(true);
        }
        else {
            objetodatatable.column(17).visible(false);
        }
    })


    const checkboxContainer = $('#checkboxContainer');
    const leftArrow = $('#leftArrow');
    const rightArrow = $('#rightArrow');

    function toggleArrows() {
        if (checkboxContainer.scrollLeft() === 0) {
            leftArrow.addClass('disabled');
        } else {
            leftArrow.removeClass('disabled');
        }

        if (checkboxContainer.scrollLeft() + checkboxContainer.width() >= checkboxContainer.prop('scrollWidth')) {
            rightArrow.addClass('disabled');
        } else {
            rightArrow.removeClass('disabled');
        }
    }
    leftArrow.on('click', () => {
        checkboxContainer.animate({ scrollLeft: '-=' + checkboxContainer.width() }, 500);
    });

    rightArrow.on('click', () => {
        checkboxContainer.animate({ scrollLeft: '+=' + checkboxContainer.width() }, 500);

    });

    checkboxContainer.on('scroll', toggleArrows);

    toggleArrows();
});



function ListarMaquinaInoperativa() {
    var url = basePath + "MIMaquinaInoperativa/ListarMaquinaInoperativaxSalasUsuarioJson";
    var data = {}; var respuesta = "";
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
            respuesta = response.data
            objetodatatable = $("#tableMaquinaInoperativa").DataTable({

                "fixedColumns": {
                    "leftColumns": 3
                },
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "sScrollX": "100%",
                "paging": true,
                "ordering": true,
                "aaSorting": [],
                "autoWidth": false,
                "bAutoWidth": true,
                "bProcessing": true,
                "bDeferRender": true,
                data: response.data,
                columns: [
                    { data: "CodMaquinaInoperativa", title: "ID" },
                    { data: "MaquinaLey", title: "CodMaquina" },
                    { data: "NombreSala", title: "Sala" },
                    //{ data: "TecnicoCreado", title: "Tecnico", visible: false },
                    //{ data: "ObservacionCreado", title: "Observacion", visible: false },
                    //{
                    //    data: "CodEstadoInoperativa", title: "Estado Inoperativa",
                    //    "render": function (o) {
                    //        var estado = "";
                    //        var css = "btn-danger";
                    //        if (o == 1) {
                    //            estado = "Op. Problemas"
                    //            css = "btn-success";
                    //        }
                    //        if (o == 2) {
                    //            estado = "Inoperativa"
                    //            css = "btn-danger";
                    //        }
                    //        if (o == 3) {
                    //            estado = "Atendida en Sala"
                    //            css = "btn-info";
                    //        }
                    //        return '<span class="label ' + css + ' "style="width:100%; display:block;padding:5px 0"> ' + estado + '</span>';

                    //    }
                    //},
                    //{
                    //    data: "CodPrioridad", title: "Prioridad",
                    //    "render": function (o) {
                    //        var estado = "";
                    //        var css = "btn-danger";
                    //        if (o == 1) {
                    //            estado = "Baja"
                    //            css = "btn-success";
                    //        }
                    //        if (o == 2) {
                    //            estado = "Media"
                    //            css = "btn-info";
                    //        }
                    //        if (o == 3) {
                    //            estado = "Alta"
                    //            css = "btn-danger";
                    //        }
                    //        return '<span class="label ' + css + '"style="width:100%; display:block;padding:5px 0">' + estado + '</span>';

                    //    },

                    //},
                    //{
                    //    data: "FechaModificacion", title: "Fecha Modificacion",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    },
                    //    visible: false
                    //},
                    //{ data: "CodEstadoProceso", title: "Estado Proceso", visible: false },
                    //{
                    //    data: "FechaEnvioSala", title: "Fecha Envio Sala",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    },
                    //    visible: false
                    //},
                    //{
                    //    data: "FechaRecepcionCentral", title: "Fecha Recepcion Central",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    },
                    //    visible: false
                    //},
                    //{
                    //    data: "FechaEnvioCentral", title: "Fecha Envio Central",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    },
                    //    visible: false
                    //},
                    //{
                    //    data: "ObservacionAtencion", title: "Observacion Atencion", visible: false
                    //},
                    //{
                    //    data: "FechaRecepcionSala", title: "Fecha Recepcion Sala",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    },
                    //    visible: false
                    //},
                    //{
                    //    data: "FechaOperativaSala", title: "Fecha Operativa Sala",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    },
                    //    visible: false
                    //},
                    //{
                    //    data: "ObservacionOperatividadSala", title: "Observacion Operatividad Sala",
                    //    visible: false
                    //},
                    //{
                    //    data: "FechaRegistro", title: "Fecha Registro",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    },
                    //    visible: false
                    //},
                    //{
                    //    data: "FechaModificacion", title: "Fecha Modificacion",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    }
                    //},
                    {
                        data: "NombreUsuarioCreado", title: "Usuario",
                        visible: false
                    },
                    {
                        data: "CodEstadoProceso", title: "Estado",
                        "render": function (o) {
                            var estado = "CREADO";
                            var css = "btn-info";
                            if (o == 3) {
                                estado = "ATENDIDA INOPERATIVA"
                                css = "btn-danger";
                            }
                            else if (o == 2 ) {
                                estado = "ATENDIDA OPERATIVA"
                                css = "btn-primary";
                            }
                            else if (o == 4) {
                                estado = "EN ESPERA SOLICITUD"
                                css = "btn-warning";
                            }
                            else if (o == 5) {
                                estado = "REPUESTOS AGREGADOS"
                                css = "btn-primary";
                            }
                            return '<span class="label ' + css + '" style="width:100%; display:block;padding:5px 0">' + estado + '</span>';

                        }
                    },
                    {
                        data: "CodMaquinaInoperativa", title: "Acción",
                        "bSortable": false,
                        "render": function (o, type, oData) {

                            return `<button type="button" class="btn btn-xs btn-warning btnDetalle" data-id="${o}"> <span class="glyphicon glyphicon-list-alt" style="margin-right: 5px;"></span>VER HISTORICO</i></button>`;
                        },
                    }
                ],
                "drawCallback": function (settings) {
                    $('.btnDetalle').tooltip({
                        title: "Historico"
                    });
                },

                "initComplete": function (settings, json) {



                },
            });

        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {

        }
    });
};

function ObtenerRegistro(id) {

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "MICategoriaPieza/ListarCategoriaPiezaCodJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodCategoriaPieza: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};

function GetListaPiezas(cod) {


    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "MIMaquinaInoperativa/ListarPiezasxMaquinaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ cod }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {

                respuesta = response.data
                objetodatatable = $("#tablePiezaMaquinaInoperativa").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "ordering": true,
                    "scrollCollapse": true,
                    "scrollX": true,
                    "paging": true,
                    "aaSorting": [],
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response.data,
                    columns: [
                        { data: "CodMaquinaInoperativaPiezas", title: "ID" },
                        { data: "NombrePieza", title: "Pieza" },
                        { data: "Cantidad", title: "Cantidad" },
                        {
                            data: "FechaRegistro", title: "Fecha Registro",
                            "render": function (o) {
                                return moment(o).format("DD/MM/YYYY hh:mm");
                            }
                        },
                        {
                            data: "FechaModificacion", title: "Fecha Modificacion",
                            "render": function (o) {
                                return moment(o).format("DD/MM/YYYY hh:mm");
                            }
                        },
                        { data: "CodUsuario", title: "Usuario" },
                        {
                            data: "Estado", title: "Estado",
                            "render": function (o) {
                                var estado = "INACTIVO";
                                var css = "btn-danger";
                                if (o == 1) {
                                    estado = "ACTIVO"
                                    css = "btn-success";
                                }
                                return '<span class="label ' + css + '">' + estado + '</span>';

                            }
                        },
                    ],

                    "initComplete": function (settings, json) {



                    },
                });
            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function GetListaProblemas(cod) {


    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "MIMaquinaInoperativa/ListarProblemasxMaquinaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ cod }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {

                respuesta = response.data
                objetodatatable = $("#tableProblemaMaquinaInoperativa").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "ordering": true,
                    "scrollCollapse": true,
                    "scrollX": true,
                    "paging": true,
                    "aaSorting": [],
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response.data,
                    columns: [
                        { data: "CodMaquinaInoperativaProblemas", title: "ID" },
                        { data: "NombreProblema", title: "Problema" },
                        {
                            data: "FechaRegistro", title: "Fecha Registro",
                            "render": function (o) {
                                return moment(o).format("DD/MM/YYYY hh:mm");
                            }
                        },
                        {
                            data: "FechaModificacion", title: "Fecha Modificacion",
                            "render": function (o) {
                                return moment(o).format("DD/MM/YYYY hh:mm");
                            }
                        },
                        { data: "CodUsuario", title: "Usuario" },
                        {
                            data: "Estado", title: "Estado",
                            "render": function (o) {
                                var estado = "INACTIVO";
                                var css = "btn-danger";
                                if (o == 1) {
                                    estado = "ACTIVO"
                                    css = "btn-success";
                                }
                                return '<span class="label ' + css + '">' + estado + '</span>';

                            }
                        },
                    ],

                    "initComplete": function (settings, json) {



                    },
                });
            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function GetListaRepuestos(cod) {


    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "MIMaquinaInoperativa/ListarRepuestosxMaquinaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ cod }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {

                respuesta = response.data
                objetodatatable = $("#tableRepuestoMaquinaInoperativa").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "ordering": true,
                    "scrollCollapse": true,
                    "scrollX": true,
                    "paging": true,
                    "aaSorting": [],
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response.data,
                    columns: [
                        { data: "CodMaquinaInoperativaRepuestos", title: "ID" },
                        { data: "NombreRepuesto", title: "Repuesto" },
                        { data: "Cantidad", title: "Cantidad" },
                        {
                            data: "FechaRegistro", title: "Fecha Registro",
                            "render": function (o) {
                                return moment(o).format("DD/MM/YYYY hh:mm");
                            }
                        },
                        {
                            data: "FechaModificacion", title: "Fecha Modificacion",
                            "render": function (o) {
                                return moment(o).format("DD/MM/YYYY hh:mm");
                            }
                        },
                        { data: "CodUsuario", title: "Usuario" },
                        {
                            data: "Estado", title: "Estado",
                            "render": function (o) {
                                var estado = "INACTIVO";
                                var css = "btn-danger";
                                if (o == 1) {
                                    estado = "ACTIVO"
                                    css = "btn-success";
                                }
                                return '<span class="label ' + css + '">' + estado + '</span>';

                            }
                        },
                    ],

                    "initComplete": function (settings, json) {



                    },
                });
            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function LimpiarFormValidator() {
    $("#form_registro_maquina_inoperativa").parent().find('div').removeClass("has-error");
    $("#form_registro_maquina_inoperativa").parent().find('i').removeAttr("style").hide();
}


function GenerarExcel() {

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "MIMaquinaInoperativa/HistoricoListadoMaquinaInoperativaDescargarExcelJson",
        contentType: "application/json; charset=utf-8",
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
        }
    });

}


function ListarMaquinaInoperativaxFechas() {
    var fechaIni = $("#fechaIni").val();
    var fechaFin = $("#fechaFin").val();
    var filtroEstado = $("#cboFiltroEstado").val();
    var url = basePath + "MIMaquinaInoperativa/ListarMaquinaInoperativaxSalasUsuarioxFechasJson";
    var data = {}; var respuesta = "";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ fechaIni, fechaFin, filtroEstado }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            respuesta = response.data
            objetodatatable = $("#tableMaquinaInoperativa").DataTable({

                "fixedColumns": {
                    "leftColumns": 3
                },
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "sScrollX": "100%",
                "paging": true,
                "ordering": true,
                "aaSorting": [],
                "autoWidth": false,
                "bAutoWidth": true,
                "bProcessing": true,
                "bDeferRender": true,
                data: response.data,
                columns: [
                    { data: "CodMaquinaInoperativa", title: "ID" },
                    { data: "MaquinaLey", title: "CodMaquina" },
                    { data: "NombreSala", title: "Sala" },
                    //{ data: "TecnicoCreado", title: "Tecnico", visible: false },
                    //{ data: "ObservacionCreado", title: "Observacion", visible: false },
                    //{
                    //    data: "CodEstadoInoperativa", title: "Estado Inoperativa",
                    //    "render": function (o) {
                    //        var estado = "";
                    //        var css = "btn-danger";
                    //        if (o == 1) {
                    //            estado = "Op. Problemas"
                    //            css = "btn-success";
                    //        }
                    //        if (o == 2) {
                    //            estado = "Inoperativa"
                    //            css = "btn-danger";
                    //        }
                    //        if (o == 3) {
                    //            estado = "Atendida en Sala"
                    //            css = "btn-info";
                    //        }
                    //        return '<span class="label ' + css + ' "style="width:100%; display:block;padding:5px 0"> ' + estado + '</span>';

                    //    }
                    //},
                    //{
                    //    data: "CodPrioridad", title: "Prioridad",
                    //    "render": function (o) {
                    //        var estado = "";
                    //        var css = "btn-danger";
                    //        if (o == 1) {
                    //            estado = "Baja"
                    //            css = "btn-success";
                    //        }
                    //        if (o == 2) {
                    //            estado = "Media"
                    //            css = "btn-info";
                    //        }
                    //        if (o == 3) {
                    //            estado = "Alta"
                    //            css = "btn-danger";
                    //        }
                    //        return '<span class="label ' + css + '"style="width:100%; display:block;padding:5px 0">' + estado + '</span>';

                    //    },

                    //},
                    //{
                    //    data: "FechaModificacion", title: "Fecha Modificacion",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    },
                    //    visible: false
                    //},
                    //{ data: "CodEstadoProceso", title: "Estado Proceso", visible: false },
                    //{
                    //    data: "FechaEnvioSala", title: "Fecha Envio Sala",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    },
                    //    visible: false
                    //},
                    //{
                    //    data: "FechaRecepcionCentral", title: "Fecha Recepcion Central",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    },
                    //    visible: false
                    //},
                    //{
                    //    data: "FechaEnvioCentral", title: "Fecha Envio Central",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    },
                    //    visible: false
                    //},
                    //{
                    //    data: "ObservacionAtencion", title: "Observacion Atencion", visible: false
                    //},
                    //{
                    //    data: "FechaRecepcionSala", title: "Fecha Recepcion Sala",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    },
                    //    visible: false
                    //},
                    //{
                    //    data: "FechaOperativaSala", title: "Fecha Operativa Sala",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    },
                    //    visible: false
                    //},
                    //{
                    //    data: "ObservacionOperatividadSala", title: "Observacion Operatividad Sala",
                    //    visible: false
                    //},
                    //{
                    //    data: "FechaModificacion", title: "Fecha Modificacion",
                    //    "render": function (o) {
                    //        return moment(o).format("DD/MM/YYYY hh:mm");
                    //    }
                    //},
                    {
                        data: "FechaCreado", title: "Fecha Creado",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY");
                        },
                    },
                    {
                        data: "NombreUsuarioCreado", title: "Usuario",
                        visible: false
                    },
                    {
                        data: "CodEstadoProceso", title: "Estado",
                        "render": function (o) {
                            var estado = "CREADO";
                            var css = "btn-info";
                            if (o == 3) {
                                estado = "ATENDIDA INOPERATIVA"
                                css = "btn-danger";
                            }
                            else if (o == 2) {
                                estado = "ATENDIDA OPERATIVA"
                                css = "btn-primary";
                            }
                            else if (o == 4) {
                                estado = "EN ESPERA SOLICITUD"
                                css = "btn-warning";
                            }
                            else if (o == 5) {
                                estado = "REPUESTOS AGREGADOS"
                                css = "btn-primary";
                            }
                            return '<span class="label ' + css + '" style="width:100%; display:block;padding:5px 0">' + estado + '</span>';

                        }
                    },
                    {
                        data: "CodMaquinaInoperativa", title: "Acción",
                        "bSortable": false,
                        "render": function (o, type, oData) {

                            return `<button type="button" class="btn btn-xs btn-warning btnDetalle" data-id="${o}"> <span class="glyphicon glyphicon-list-alt" style="margin-right: 5px;"></span>VER HISTORICO</i></button>`;
                        },
                    }
                ],
                "drawCallback": function (settings) {
                    $('.btnDetalle').tooltip({
                        title: "Historico"
                    });
                },

                "initComplete": function (settings, json) {



                },
            });

        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {

        }
    });
};


function GenerarExcelxFechas() {

    var fechaIni = $("#fechaIni").val();
    var fechaFin = $("#fechaFin").val();
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "MIMaquinaInoperativa/HistoricoListadoMaquinaInoperativaDescargarExcelxFechasJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ fechaIni, fechaFin }),
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
        }
    });

}
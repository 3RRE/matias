$(document).ready(function () {



    ListarMaquinaInoperativa();

    VistaAuditoria("MIMaquinaInoperativa/ListadoMaquinaInoperativa", "VISTA", 0, "", 3);


    $(document).on("click", ".btnNuevo", function () {

        window.location.replace(basePath + "MIMaquinaInoperativa/MaquinaInoperativaInsertarVista");

    });


    $(document).on("click", ".btnDetalle", function () {

        let id = $(this).data("id");
        let url = basePath + "MIMaquinaInoperativa/EditarMaquinaInoperativa/" + id;
        window.location.replace(url);

    });


    $(document).on("click", ".btnAtencion", function () {

        let id = $(this).data("id");
        let url = basePath + "MIMaquinaInoperativa/AtencionMaquinaInoperativa/" + id;
        window.location.replace(url);

    });

    $(document).on("click", ".btnDetalleAtencion", function () {

        let id = $(this).data("id");
        let url = basePath + "MIMaquinaInoperativa/AtencionMaquinaInoperativa/" + id;
        window.location.replace(url);

    });

    $(document).on("click", ".btnDetalleAtencion2", function () {

        let id = $(this).data("id");
        let url = basePath + "MIMaquinaInoperativa/AtencionMaquinaInoperativa/" + id;
        window.location.replace(url);

    });

    $(document).on("click", ".btnPiezas", function () {

        var id = $(this).data("id");
        GetListaPiezas(id);
        $("#full-modal_pieza_maquina_inoperativa").modal("show");

    });

    $(document).on("click", ".btnProblemas", function () {

        var id = $(this).data("id");
        GetListaProblemas(id);
        $("#full-modal_problema_maquina_inoperativa").modal("show");

    });

    $(document).on("click", ".btnRepuestos", function () {

        var id = $(this).data("id");
        GetListaRepuestos(id);
        $("#full-modal_repuesto_maquina_inoperativa").modal("show");

    });


});

function ListarMaquinaInoperativa() {
    var url = basePath + "MIMaquinaInoperativa/ListarMaquinaInoperativaCreadaJson";
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
                "bDestroy": true,
                "bSort": true,
                "ordering": true,
                "scrollCollapse": true,
                "scrollX": true,
                "paging": true,
                "aaSorting": [0],
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                data: response.data,
                columns: [
                    { data: "CodMaquinaInoperativa", title: "ID" },
                    { data: "CodMaquinaLey", title: "CodMaquina" },
                    { data: "NombreSala", title: "Sala" },
                    { data: "ObservacionInoperativa", title: "Observacion" },
                    {
                        data: "CodEstadoInoperativa", title: "Estado Inoperativa",
                        "render": function (o) {
                            var estado = "";
                            var css = "btn-danger";
                            if (o == 1) {
                                estado = "Op. Problemas"
                                css = "btn-success";
                            }
                            if (o == 2) {
                                estado = "Inoperativa"
                                css = "btn-danger";
                            }
                            if (o == 3) {
                                estado = "Atendida en Sala"
                                css = "btn-info";
                            }
                            return '<span class="label ' + css + '"style="width:100%; display:block;padding:5px 0"> ' + estado + '</span>';

                        }
                    },
                    {
                        data: "CodPrioridad", title: "Prioridad",
                        "render": function (o) {
                            var estado = "";
                            var css = "btn-danger";
                            if (o == 1) {
                                estado = "Baja"
                                css = "btn-success";
                            }
                            if (o == 2) {
                                estado = "Media"
                                css = "btn-info";
                            }
                            if (o == 3) {
                                estado = "Alta"
                                css = "btn-danger";
                            }
                            return '<span class="label ' + css + '" style="width:100%; display:block;padding:5px 0">' + estado + '</span>';

                        }
                    },
                    { data: "Tecnico", title: "Tecnico" },
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
                            var estado = "CREADO";
                            var css = "btn-info";
                            if (o == 3) {
                                estado = "ATENDIDO"
                                css = "btn-danger";
                            }
                            if (o == 2 || o==4) {
                                estado = "EN ESPERA"
                                css = "btn-primary";
                            }
                            if (o == 4) {
                                estado = "EN ESPERA"
                                css = "btn-warning";
                            }
                            return '<span class="label ' + css + '" style="width:100%; display:block;padding:5px 0">' + estado + '</span>';

                        }
                    },
                    {
                        data: "CodMaquinaInoperativa", title: "Acción",
                        "bSortable": false,
                        "render": function (o, type, oData) {

                            if (oData.Estado == 1) {

                                return `<button type="button" class="btn btn-xs btn-warning btnDetalle" data-id="${o}">EDITAR</i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnAtencion" data-id="${o}">ATENDER</button> 
                                    `;

                            }
                            else {
                                return `<button type="button" class="btn btn-xs btn-warning btnDetalleAtencion" data-id="${o}">ATENDER</i></button> `;
                            }
                        }
                    }
                ],
                "drawCallback": function (settings) {
                    $('.btnEditar').tooltip({
                        title: "Editar"
                    });
                    $('.btnEliminar').tooltip({
                        title: "Eliminar"
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
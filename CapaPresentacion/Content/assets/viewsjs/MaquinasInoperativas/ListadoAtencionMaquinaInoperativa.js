


function ListarMaquinaInoperativa() {
    var url = basePath + "MIMaquinaInoperativa/ListarMaquinaInoperativaJson";
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
                            return '<span class="label ' + css + '">' + estado + '</span>';

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
                            return '<span class="label ' + css + '">' + estado + '</span>';

                        }
                    },
                    { data: "Tecnico", title: "Tecnico" },
                    { data: "CodEstadoProceso", title: "Estado Proceso" },
                    {
                        data: "FechaEnvioSala", title: "Fecha Envio Sala",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY hh:mm");
                        }
                    },
                    {
                        data: "FechaRecepcionCentral", title: "Fecha Recepcion Central",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY hh:mm");
                        }
                    },
                    {
                        data: "FechaEnvioCentral", title: "Fecha Envio Central",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY hh:mm");
                        }
                    },
                    { data: "ObservacionAtencion", title: "Observacion Atencion" },
                    {
                        data: "FechaRecepcionSala", title: "Fecha Recepcion Sala",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY hh:mm");
                        }
                    },
                    {
                        data: "FechaOperativaSala", title: "Fecha Operativa Sala",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY hh:mm");
                        }
                    },
                    { data: "ObservacionOperatividadSala", title: "Observacion Operatividad Sala" },
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
                            var estado = "CREADO";
                            var css = "btn-info";
                            if (o == 3) {
                                estado = "ATENDIDO"
                                css = "btn-danger";
                            }
                            if (o == 2) {
                                estado = "ATENDIDO EN ESPERA"
                                css = "btn-success";
                            }
                            return '<span class="label ' + css + '">' + estado + '</span>';

                        }
                    },
                    {
                        data: "CodMaquinaInoperativa", title: "Acción",
                        "bSortable": false,
                        "render": function (o, type, oData) {

                            if (oData.Estado == 1) {

                                return `<button type="button" class="btn btn-xs btn-warning btnDetalle" data-id="${o}">EDITAR</i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnAtencion" data-id="${o}">ATENCION</button> 
                                    <button type="button" class="btn btn-xs btn-danger btnPiezas" data-id="${o}">PIEZAS</button> 
                                    <button type="button" class="btn btn-xs btn-info btnProblemas" data-id="${o}">PROBLEMAS</i></button> 
                                    <button type="button" class="btn btn-xs btn-success btnRepuestos" data-id="${o}">REPUESTOS</i></button> `;

                            }
                            else {
                                return `<button type="button" class="btn btn-xs btn-warning btnDetalleAtencion" data-id="${o}">DETALLES</i></button> `;
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
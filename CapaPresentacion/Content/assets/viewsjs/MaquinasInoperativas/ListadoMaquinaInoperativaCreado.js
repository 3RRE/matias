$(document).ready(function () {

    ListarMaquinaInoperativaCreado();

    VistaAuditoria("MIMaquinaInoperativa/ListadoMaquinaInoperativaCreado", "VISTA", 0, "", 3);

    $(document).on("click", ".btnAtender", function () {
        let id = $(this).data("id");
        let url = basePath + "MIMaquinaInoperativa/AtenderMaquinaInoperativaVista/" + id;
        window.location.replace(url);
    });
    
    $(document).on("click", ".btnAtenderNuevamente", function () {
        let id = $(this).data("id");
        let url = basePath + "MIMaquinaInoperativa/AtenderMaquinaInoperativaRepuestosAgregadosVista/" + id;
        window.location.replace(url);
    });

    $(document).on("click", "#btnExcel", function () {
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "MIMaquinaInoperativa/ListarMaquinaInoperativaCreadoExcelJson",
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

    });

});

function ListarMaquinaInoperativaCreado() {
    var url = basePath + "MIMaquinaInoperativa/ListarMaquinaInoperativaCreadoJson";
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
                    { data: "MaquinaLey", title: "CodMaquina" },
                    { data: "NombreSala", title: "Sala" },
                    //{ data: "ObservacionInoperativa", title: "Observacion" },
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
                    { data: "TecnicoCreado", title: "Tecnico" },
                    {
                        data: "FechaCreado", title: "Fecha Creación",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY hh:mm");
                        }
                    },
                    { data: "NombreUsuarioCreado", title: "Usuario" },
                    {
                        data: "CodEstadoProceso", title: "Estado",
                        "render": function (o) {

                            var estado = "ERROR";
                            var css = "btn-danger";

                            if (o == 1) {
                                estado = "CREADO";
                                css = "btn-info";
                            } else if (o == 5) {
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

                            if (oData.CodEstadoProceso == 1) {
                                return `<button type="button" class="btn btn-xs btn-warning btnAtender" data-id="${o}">ATENDER</i></button> `;
                            } else if (oData.CodEstadoProceso == 5) {
                                return `<button type="button" class="btn btn-xs btn-warning btnAtenderNuevamente" data-id="${o}">ATENDER NUEVAMENTE</i></button> `;
                            }
                            return ``;

                        }
                    }
                ],
                "drawCallback": function (settings) {
                    $('.btnAtender').tooltip({
                        title: "Atender"
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

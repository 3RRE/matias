$(document).ready(function () {

    ListarMaquinaInoperativaAtendidaOperativa();

    VistaAuditoria("MaquinasInoperativasV2/AtenderSolicitudMaquinaInoperativaVista", "VISTA", 0, "", 3);

    $(document).on("click", ".btnAtender", function () {
        let id = $(this).data("id");
        let url = basePath + "MaquinasInoperativasV2/AtenderSolicitudMaquinaInoperativaVista/" + id;
        window.location.assign(url);
    });


    $(document).on("click", "#btnExcel", function () {
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "MIMaquinaInoperativa/ListarMaquinaInoperativaAtendidaInoperativaExcelJson",
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

function ListarMaquinaInoperativaAtendidaOperativa() {
    var url = basePath + "MIMaquinaInoperativa/ListarMaquinaInoperativaAtendidaInoperativaJson";
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
                            if (o == 3) {
                                estado = "Baja"
                                css = "btn-success";
                            }
                            if (o == 2) {
                                estado = "Normal"
                                css = "btn-info";
                            }
                            if (o == 1) {
                                estado = "Urgente"
                                css = "btn-danger";
                            }
                            return '<span class="label ' + css + '" style="width:100%; display:block;padding:5px 0">' + estado + '</span>';

                        }
                    },
                    {
                        data: "FechaAtendidaInoperativa", title: "Fecha Atención",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY hh:mm:ss a");
                        }
                    },
                    { data: "NombreUsuarioAtendidaInoperativa", title: "Tecnico Atención" },
                    {
                        data: "Estado", title: "Estado",
                        "render": function (o) {
                            var estado = "ATENDIDA INOPERATIVA";
                            var css = "btn-danger";
                          
                            return '<span class="label ' + css + '" style="width:100%; display:block;padding:5px 0">' + estado + '</span>';

                        }
                    },
                    {
                        data: "CodMaquinaInoperativa", title: "Acción",
                        "bSortable": false,
                        "render": function (o, type, oData) {
                            return `<button type="button" class="btn btn-xs btn-warning btnAtender" data-id="${o}">REPARAR</i></button> `;
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

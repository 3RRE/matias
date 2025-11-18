

$(document).ready(function () {

    obtenerListaSalas();

    $(document).on('click', '#btnBuscar', function (e) {

        e.preventDefault();

        var codSala =  $('#cboSala option:selected').data('id');
        obtenerEstadoServicios(codSala);

    });

});


function obtenerListaSalas() {
    return $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            renderSelectSalas(result.data);
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function renderSelectSalas(data) {
    $.each(data, function (index, value) {
        $("#cboSala").append(`<option value="${value.UrlProgresivo == "" ? "" : value.UrlProgresivo}" data-id="${value.CodSala}" data-ipprivada="${value.IpPrivada}" data-puertoservicio="${value.PuertoServicioWebOnline}">${value.Nombre}</option>`)
    });
    renderDataTableEstadoServicios([]);
}

function obtenerEstadoServicios(codSala) {

    ajaxhr = $.ajax({
        type: 'POST',
        cache: false,
        url: basePath + 'EstadoServicios/EstadoServiciosListadoJson',
        contentType: 'application/json: charset=utf-8',
        data: JSON.stringify({ codSala }),
        dataType: 'json',
        beforeSend: function (xhr) {
            $.LoadingOverlay('show');
        },
        success: function (response) {
            AbortRequest.close()

            if (response.respuesta) {
                renderDataTableEstadoServicios(response.data);
            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
        error: function (request, status, error) {
            AbortRequest.close()

            if (request.status == 400) {
                toastr.error(request.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        },
        complete: function (xhr) {
            AbortRequest.close()

            $.LoadingOverlay('hide');

        }
    });
    AbortRequest.open()

    return ajaxhr
}

function renderDataTableEstadoServicios(data) {
    $('#tableEstadoServicios').DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": false,
        "paging": true,
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "aaSorting": [],
        data: data,
        columns: [
            {
                data: "NombreSala", title: "Sala"
            },
            {
                data: "EstadoWebOnline", title: "Estado Web Online",
                "render": function (value) {

                    var estado = "";
                    var css = "btn-info";

                    if (value == 0) {
                        estado = "INACTIVO"
                        css = "btn-danger";
                    }
                    if (value == 1) {
                        estado = "ACTIVO"
                        css = "btn-success";
                    }

                    return '<span class="label ' + css + '"style="width:100%; display:block;padding:5px 0">' + estado + '</span>';
                }
            },
            {
                data: "EstadoGladconServices", title: "Estado Gladcon Services",
                "render": function (value) {

                    var estado = "";
                    var css = "btn-info";

                    if (value == 0) {
                        estado = "INACTIVO"
                        css = "btn-danger";
                    }
                    if (value == 1) {
                        estado = "ACTIVO"
                        css = "btn-success";
                    }

                    return '<span class="label ' + css + '"style="width:100%; display:block;padding:5px 0">' + estado + '</span>';
                }
            },
            {
                data: "FechaRegistro", title: "Fecha Registro",
                "render": function (value) {
                    return moment(value).format('DD/MM/YYYY HH:mm:ss');
                }
            }
        ],
        "initComplete": function (settings, json) {
        }
    });
}

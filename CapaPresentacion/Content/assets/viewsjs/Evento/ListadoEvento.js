
$(document).ready(function () {

    ObtenerListaSalas();
    $("#tableEvento").DataTable();

    $("#btnBuscar").on("click", function () {
        var sala = $("#cboSala").val();
        if (sala == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        ListarEventos();
    });

    $('#btnNuevoEvento').on('click', function (e) {
        window.location.replace(basePath + "Publicidad/EventoInsertarVista");
    });

    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var url = basePath + "Publicidad/EventoModificarVista/" + id;
        window.location.replace(url);
    });

    $(document).on('click', '.btnEliminar', function (e) {
        var id = $(this).data("id");
        var estado = $(this).data("estado");
        var nuevoEstado = (estado == 1 ? 0 : 1);
        var textoConfirm = (estado == 1 ? "¿Desea desactivar el evento?" : "¿Desea activar el evento?");
        var textoBoton = (estado == 1 ? "Sí, desactivar" : "Sí, activar");

        $.confirm({
            icon: 'fa fa-warning',
            title: 'Confirmar Acción',
            theme: 'black',
            content: textoConfirm,
            confirmButton: textoBoton,
            cancelButton: 'Cancelar',
            confirm: function () {
                $.ajax({
                    url: basePath + "Publicidad/ModificarEstadoEventoJson",
                    type: "POST",
                    data: { Id: id, Estado: nuevoEstado },
                    success: function (response) {
                        if (response.respuesta) {
                            toastr.success("Estado modificado", "Mensaje Servidor");
                            ListarEventos(); // Recargar la tabla
                        } else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }
                    }
                });
            }
        });
    });
});

function imgLoader(element) {
    var overlay = $(element).siblings('.overlaySpinner');
    overlay.hide();
}

function ListarEventos() {
    var sala = $("#cboSala").val();
    var url = basePath + "Publicidad/ListadoEventoAdmin";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codSala: sala }),
        beforeSend: function () { $.LoadingOverlay("show"); },
        complete: function () { $.LoadingOverlay("hide"); },
        success: function (response) {
            var respuesta = response.data;
            $("#tableEvento").DataTable({
                "bDestroy": true,
                "bSort": true,
                "paging": true,
                "data": respuesta,
                "columns": [
                    { data: "Titulo", title: "Título" },
                    { data: "NombreSala", title: "Sala" },
                    {
                        data: null,
                        title: "Logo",
                        width: "80px",
                        "render": function (data, type, row) {
                            var urlImagen = row.UrlImagenCompleta + "?v=" + new Date().getTime();
                            return `<div style="position:relative;height:50px; width:80px; background: #eee;">
                                        <div class="overlaySpinner" style="position:absolute;height:50px; width:80px; background: #fff; text-align:center; padding-top:15px;"><i class="fa fa-spinner fa-spin"></i></div>
                                        <img style="object-fit: contain;height:50px;width:80px;" src="${urlImagen}" onLoad="imgLoader(this)" />
                                    </div>`;
                        }
                    },
                    {
                        data: "FechaEvento", title: "Fecha Evento",
                        "render": function (data, type, row) {
                            return data ? moment(data).format("DD-MM-YYYY hh:mm a") : 'N/A';
                        }
                    },
                    {
                        data: "Estado", title: "Estado",
                        "render": function (data, type, row) {
                            return data == 1 ? '<span class="label label-success">Activo</span>' : '<span class="label label-danger">Inactivo</span>';
                        }
                    },
                    {
                        data: "IdEvento",
                        "bSortable": false,
                        "title": "Acción",
                        "render": function (data, type, row) {
                            var btnEditar = `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${data}"><i class="glyphicon glyphicon-pencil"></i></button>`;
                            var btnEstado = (row.Estado == 1)
                                ? `<button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${data}" data-estado="1"><i class="glyphicon glyphicon-trash"></i></button>`
                                : `<button type="button" class="btn btn-xs btn-success btnEliminar" data-id="${data}" data-estado="0"><i class="glyphicon glyphicon-ok"></i></button>`;
                            return btnEditar + ' ' + btnEstado;
                        }
                    }
                ]
            });
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            toastr.error("Error Servidor", "Mensaje Servidor");
        }
    });
};

// Función para cargar el select de salas (copiada de ListadoPublicacion.js)
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
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: true
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
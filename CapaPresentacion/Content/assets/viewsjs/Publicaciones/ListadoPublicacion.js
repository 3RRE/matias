$(document).ready(function () {

    ObtenerListaSalas();
     $("#tablePublicidad").DataTable();
    $("#btnBuscar").on("click", function () {
        var sala = $("#cboSala").val();
        if (sala == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        ListarPublicidad();
    });

    $('#btnNuevaPublicidad').on('click', function (e) {
        window.location.replace(basePath + "Publicidad/PublicidadInsertarVista");
    });

    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var url = basePath + "Publicidad/PublicidadModificarVista/" + id;
        window.location.replace(url);
    });

    // Lógica para eliminación (Desactivar)
    $(document).on('click', '.btnEliminar', function (e) {
        var id = $(this).data("id");
        var estado = $(this).data("estado");
        var nuevoEstado = (estado == 1 ? 0 : 1); // Invierte el estado
        var textoConfirm = (estado == 1 ? "¿Desea desactivar el registro?" : "¿Desea activar el registro?");
        var textoBoton = (estado == 1 ? "Sí, desactivar" : "Sí, activar");

        $.confirm({
            icon: 'fa fa-warning',
            title: 'Confirmar Acción',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-danger',
            cancelButtonClass: 'btn-info',
            confirmButton: textoBoton,
            cancelButton: 'Cancelar',
            content: textoConfirm,
            confirm: function () {
                $.ajax({
                    url: basePath + "Publicidad/ModificarEstadoPublicidadJson",
                    type: "POST",
                    data: { Id: id, Estado: nuevoEstado },
                    success: function (response) {
                        if (response.respuesta) {
                            toastr.success("Estado modificado", "Mensaje Servidor");
                            ListarPublicidad(); // Recargar la tabla
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

function ListarPublicidad() {
    var sala = $("#cboSala").val();
    var url = basePath + "Publicidad/ListadoPublicidadAdmin";
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
            $("#tablePublicidad").DataTable({
                "bDestroy": true,
                "bSort": true,
                "paging": true,
                "data": respuesta,
                "columns": [
                    { data: "Titulo", title: "Título" },
                    { data: "NombreSala", title: "Sala" },
                    { data: "Orden", title: "Orden" },
                    {
                        data: null,
                        title: "Imagen",
                        width: "80px",
                        "render": function (data, type, row) {
                            // Usamos la propiedad UrlImagenCompleta de la entidad
                            var urlImagen = row.UrlImagenCompleta + "?v=" + new Date().getTime(); // Cache busting
                            return `<div style="position:relative;height:50px; width:80px; background: #eee;">
                                        <div class="overlaySpinner" style="position:absolute;height:50px; width:80px; background: #fff; text-align:center; padding-top:15px;"><i class="fa fa-spinner fa-spin"></i></div>
                                        <img style="object-fit: contain;height:50px;width:80px;" src="${urlImagen}" onLoad="imgLoader(this)" />
                                    </div>`;
                        }
                    },
                    {
                        data: null, title: "Vigencia",
                        "render": function (data, type, row) {
                            // Formatear fechas
                            var inicio = row.FechaInicio ? new Date(parseInt(row.FechaInicio.substr(6))).toLocaleDateString() : 'N/A';
                            var fin = row.FechaFin ? new Date(parseInt(row.FechaFin.substr(6))).toLocaleDateString() : 'N/A';
                            return inicio + ' - ' + fin;
                        }
                    },
                    {
                        data: "Estado", title: "Estado",
                        "render": function (data, type, row) {
                            return data == 1 ? '<span class="label label-success">Activo</span>' : '<span class="label label-danger">Inactivo</span>';
                        }
                    },
                    {
                        data: "IdPublicidad",
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
//"use strict";
empleadodatatable = "";

function valideKeyNumber(evt) {
    var code = (evt.which) ? evt.which : evt.keyCode;
    if (code >= 48 && code <= 57 || code == 13) {
        return true;
    }
    return false;
}

function listarContactos() { 
    var url = basePath + "Contacto/ListarTodosLosContactosJSON";
    var data = {}; var respuesta = ""; aaaa = "";
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
            if (response.success) {
                data = response.data;
                createDataTable(data);
            } else {
                toastr.error(response.displayMessage);
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        }
    });
};

function createDataTable(data) {
    objetodatatable = $("#tableContacto").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": true,
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,

        data: data,
        columnDefs: [
            {
                targets: [0,2],
                className: "text-center"
            }
        ],
        aaSorting: [[1, 'asc']],
        columns: [
            { data: "IdContacto", title: "N°" },
            { data: "Nombre", title: "Nombre" },
            { data: "CodigoPais", title: "Código de País" },
            { data: "Numero", title: "Número" },
            {
                data: null, title: "Estado", "render": function (value, type, row) {
                    let select = `<select style="width:100%" class="input-sm selectEstado select${row.IdContacto}" data-id=${row.IdContacto}>`;

                    if (row.Estado == 1) {
                        select += `<option value="1" selected>Activo</option><option value="0">Inactivo</option>`;
                    }
                    else {
                        select += `<option value="1">Activo</option><option value="0" selected>Inactivo</option>`;
                    }
                    select += `</select>`;
                    return select;
                }
            },

            {
                data: "IdContacto",
                title: "Acción",
                "bSortable": false,
                "render": function (o) {
                    let btnEdit = `<button type="button" class="btn btn-xs btn-warning btnEditar" data-nombre="Editar Contacto" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> `;
                    let btnDelete = `<button type="button" class="btn btn-xs btn-danger btnEliminar" data-nombre="Eliminar Contacto" data-id="${o}"><i class="glyphicon glyphicon-trash"></i></button> `;

                    return btnEdit + btnDelete;
                }
            }
        ],
        "drawCallback": function (settings) {
            $('.btnEditar').tooltip({
                title: "Editar Contacto"
            });

            $('.btnEliminar').tooltip({
                title: "Eliminar Contacto"
            });
        },

        "initComplete": function (settings, json) {

            // $('#btnExcel').off("click").on('click', function () {

            //     cabecerasnuevas = [];
            //     definicioncolumnas = [];
            //     var ocultar = [];
            //     ocultar.push("Accion");
            //     funcionbotonesnuevo({
            //         botonobjeto: this, ocultar: ocultar,
            //         tablaobj: objetodatatable,
            //         cabecerasnuevas: cabecerasnuevas,
            //         definicioncolumnas: definicioncolumnas
            //     });
            //     VistaAuditoria("SalaMaestra/SalaMaestravistaExcel", "EXCEL", 0, "", 3);
            // });

        },
    });
    $('.btnEditar').tooltip({
        title: "Editar Contacto"
    });

    $('.btnELiminar').tooltip({
        title: "Eliminar Contacto"
    });
}

$(document).ready(function (){

    VistaAuditoria("Contacto/ContactoListadoVista", "VISTA", 0, "", 3);

    listarContactos();

    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var nombre = $(this).data("nombre");
        $("#hddmodal").val(nombre);
        $('#bodyModal').html("");
        $('#default-modal-label').html(nombre);
        var redirectUrl = basePath + "Contacto/ContactoActualizarVista/" + id;
        var ubicacion = "bodyModal";
        $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
            $("#modalGroup").modal("show");
        });

        //var id = $(this).data("id");
        //var url = basePath + "SalaMaestra/SalaMaestraActualizarVista/" + id;
        //window.location.replace(url);
    });

    $(document).on('click', '.btnEliminar', function (e) {
        var id = $(this).data("id");

        var data = {
            IdContacto: id
        };

        $.confirm({
            title: 'Confirmación',
            content: '¿Desea eliminar el Contacto?',
            confirmButton: 'Sí',
            cancelButton: 'No',
            confirm: function () {
                $.ajax({
                    url: basePath + "Contacto/EliminarContactoJSON",
                    type: "POST",
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    beforeSend: function () {
                        $.LoadingOverlay("show");
                    },
                    complete: function () {
                        $.LoadingOverlay("hide");
                    },
                    success: function (response) {
                        if (response.success) {
                            toastr.success(response.displayMessage);
                            listarContactos();
                        }
                        else {
                            toastr.error(response.displayMessage);
                        }
                    }
                })
            },
            cancel: function () {
                toastr.info("Se cancelo la eliminación.");
            }
        });
    });

    $('#btnNuevoContacto').on('click', function (e) {
        var nombre = $(this).data("nombre");
        $("#hddmodal").val(nombre);
        $('#bodyModal').html("");
        $('#default-modal-label').html(nombre);
        var redirectUrl = basePath + "Contacto/ContactoInsertarVista";
        var ubicacion = "bodyModal";
        $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
            $("#modalGroup").modal("show");
        });
        //window.location.replace(basePath + "SalaMaestra/SalaMaestraInsertarVista");
    });

    $(document).on("change", ".selectEstado", function () {
        let idContacto = $(this).data("id");
        let estado = $(this).val() == 1 ? true : false;
        let dataForm = {
            IdContacto: idContacto,
            Estado: estado
        }
        $.confirm({
            title: 'Confirmación',
            content: '¿Desea cambiar el estado del Contacto?',
            confirmButton: 'Sí',
            cancelButton: 'No',
            confirm: function () {
                $.ajax({
                    url: basePath + "Contacto/ActualizarEstadoContactoJSON",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(dataForm),
                    beforeSend: function () {
                        $.LoadingOverlay("show");
                    },
                    complete: function () {
                        $.LoadingOverlay("hide");
                    },
                    success: function (response) {

                        if (response.success) {
                            toastr.success(response.displayMessage);
                        }
                        else {
                            toastr.error(response.displayMessage);
                            if (estado == 1) {
                                $('.select' + idContacto).val(0);
                            }
                            else {
                                $('.select' + idContacto).val(1);
                            }
                        }
                    }
                })
            },
            cancel: function () {
                if (estado == 1) {
                    $('.select' + idContacto).val(0);
                }
                else {
                    $('.select' + idContacto).val(1);
                }
            }
        });
    });
});
$(document).ready(function () {
    listarSalasMaestras();

    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var nombre = $(this).data("nombre");
        $("#hddmodal").val(nombre);
        $('#bodyModal').html("");
        $('#default-modal-label').html(nombre);
        var redirectUrl = basePath + "SalaMaestra/SalaMaestraActualizarVista/" + id;
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
            codSalaMaestra: id
        };

        $.confirm({
            title: 'Confirmación',
            content: '¿Desea eliminar la Sala Maestra?',
            confirmButton: 'Sí',
            cancelButton: 'No',
            confirm: function () {
                $.ajax({
                    url: basePath + "SalaMaestra/SalaMaestraEliminarJSON",
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
                            listarSalasMaestras();
                        } else {
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

    $('#btnNuevaSalaMaestra').on('click', function (e) {
        var nombre = $(this).data("nombre");
        $("#hddmodal").val(nombre);
        $('#bodyModal').html("");
        $('#default-modal-label').html(nombre);
        var redirectUrl = basePath + "SalaMaestra/SalaMaestraInsertarVista";
        var ubicacion = "bodyModal";
        $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
            $("#modalGroup").modal("show");
        });
    });

    $(document).on("change", ".selectEstado", function () {
        let codigoSalaMaestra = $(this).data("id");
        let estado = $(this).val() == 1 ? true : false;
        let dataForm = {
            CodSalaMaestra: codigoSalaMaestra,
            Estado: estado
        }
        $.confirm({
            title: 'Confirmación',
            content: '¿Desea cambiar el estado de la Sala Maestra?',
            confirmButton: 'Sí',
            cancelButton: 'No',
            confirm: function () {
                $.ajax({
                    url: basePath + "SalaMaestra/SalaMaestraActualizarEstadoJSON",
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
                        } else {
                            toastr.error(response.displayMessage);
                            const newState = estado == 1 ? 0 : 1;
                            $('.select-' + codigoSalaMaestra).val(newState);
                        }
                    }
                })
            },
            cancel: function () {
                const newState = estado == 1 ? 0 : 1;
                $('.select-' + codigoSalaMaestra).val(newState);
            }
        });
    });
});

const listarSalasMaestras = () => {
    var url = basePath + "SalaMaestra/ListarTodasSalasMaestrasJSON";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            if (!response.success) {
                toastr.error(response.displayMessage);
                return;
            }
            createDataTable(response.data);
        },
    });
};

const createDataTable = (data) => {
    objetodatatable = $("#tableSalaMaestra").DataTable({
        bDestroy: true,
        bSort: true,
        scrollCollapse: true,
        scrollX: true,
        paging: true,
        autoWidth: true,
        bProcessing: true,
        bDeferRender: true,
        data: data,
        columnDefs: [
            { className: "text-center", targets: "_all" }
        ],
        aaSorting: [[1, 'asc']],
        columns: [
            { data: "CodSalaMaestra", title: "Código" },
            { data: "Nombre", title: "Nombre" },
            {
                data: null, title: "Estado", "render": function (value, type, row) {
                    let select = `<select style="width:100%" class="input-sm selectEstado select-${row.CodSalaMaestra}" data-id=${row.CodSalaMaestra}>`;
                    select += `<option value="1" ${row.Estado ? 'selected' : ''}>Activo</option><option value="0" ${row.Estado ? '' : 'selected'}>Inactivo</option>`;
                    //if (row.Estado) {
                    //    select += `<option value="1" selected>Activo</option><option value="0">Inactivo</option>`;
                    //} else {
                    //    select += `<option value="1">Activo</option><option value="0" selected>Inactivo</option>`;
                    //}
                    select += `</select>`;
                    return select;
                }
            },
            {
                data: "CodSalaMaestra", bSortable: false, render: function (o) {
                    let btnEdit = `<button type="button" class="btn btn-xs btn-warning btnEditar" data-nombre="Editar Sala Maestra" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> `;
                    let btnDelete = `<button type="button" class="btn btn-xs btn-danger btnEliminar" data-nombre="Eliminar Sala Maestra" data-id="${o}"><i class="glyphicon glyphicon-trash"></i></button> `;
                    return btnEdit + btnDelete;
                }
            }
        ],
        "drawCallback": function (settings) {
            $('.btnEditar').tooltip({
                title: "Editar Sala Maestra"
            });
            $('.btnEliminar').tooltip({
                title: "Eliminar Sala Maestra"
            });
        },
    });
    $('.btnEditar').tooltip({
        title: "Editar Sala Maestra"
    });
    $('.btnEliminar').tooltip({
        title: "Eliminar Sala Maestra"
    });
}


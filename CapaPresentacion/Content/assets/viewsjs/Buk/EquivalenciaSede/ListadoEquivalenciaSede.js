function valideKeyNumber(evt) {
    var code = (evt.which) ? evt.which : evt.keyCode;
    if (code >= 48 && code <= 57 || code == 13) {
        return true;
    }
    return false;
}
let cboEmpresa = $('#cboEmpresa');

$(document).ready(function () {
    obtenerListaEmpresa();
    listarSedes();

    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var nombre = $(this).data("nombre");
        $("#hddmodal").val(nombre);
        $('#bodyModal').html("");
        $('#default-modal-label').html(nombre);
        var redirectUrl = basePath + "EquivalenciaSede/EquivalenciaSedeActualizarVista/" + id;
        var ubicacion = "bodyModal";
        $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
            $("#modalGroup").modal("show");
        });
    });

    $(document).on('click', '.btnEliminar', function (e) {
        var id = $(this).data("id");

        var data = {
            idEquivalenciaSede: id
        };

        $.confirm({
            title: 'Confirmación',
            content: '¿Desea eliminar la Equivalencia de Sede?',
            confirmButton: 'Sí',
            cancelButton: 'No',
            confirm: function () {
                $.ajax({
                    url: basePath + "EquivalenciaSede/EliminarEquivalenciaSedeJSON",
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
                            listarSedes();
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

    $(document).on('click', '#btnNuevaEquivalenciaSede', function (e) {
        var nombre = $(this).data("nombre");
        $("#hddmodal").val(nombre);
        $('#bodyModal').html("");
        $('#default-modal-label').html(nombre);
        var redirectUrl = basePath + "EquivalenciaSede/EquivalenciaSedeInsertarVista";
        var ubicacion = "bodyModal";
        $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
            $("#modalGroup").modal("show");
        });
    });

    $(document).on('click', '#btnBuscarPorEquivalenciaEmpresa', function (e) {
        listarSedes();
    });
});

const obtenerListaEmpresa  = () => {
    $.ajax({
        type: "POST",
        url: basePath + "EquivalenciaEmpresa/ListarTodasLasEquivalenciasEmpresaCorrectasJSON",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        },
        success: function (result) {
            if (result.success) {
                renderSelectEmpresa(result.data)
            }
        },
    });
}

const renderSelectEmpresa = (data) => {
    $.each(data, function (index, value) {
        cboEmpresa.append(`<option value="${value.IdEquivalenciaEmpresa}">${value.Nombre}</option>`)
    });
    cboEmpresa.select2({
        placeholder: "--Seleccione--", allowClear: true, minimumResultsForSearch: 5
    });
    cboEmpresa.val(null).trigger("change");
}

const listarSedes = () => {
    let idEquivalenciaEmpresa = cboEmpresa.val();
    if (idEquivalenciaEmpresa) {
        listarEquivalenciaSedesPorEmpresa(idEquivalenciaEmpresa)
    } else {
        listarEquivalenciaSedes()
    }
}

const listarEquivalenciaSedes = () => {
    let url = basePath + "EquivalenciaSede/ListarTodasLasEquivalenciasSedeJSON";
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
                toastr.error(response.displayMessage)
                return;
            }
            createDataTable(response.data)
        }
    });
};

const listarEquivalenciaSedesPorEmpresa = (idEquivalenciaEmpresa) => {
    let data = {
        idEquivalenciaEmpresa
    }

    let url = basePath + "EquivalenciaSede/ObtenerEquivalenciasSedePorIdEquivalenciaEmpresaJSON";
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
            if (!response.success) {
                toastr.error(response.displayMessage)
                return;
            }
            createDataTable(response.data)
            toastr.success(response.displayMessage)
        }
    });
};

const createDataTable = (data) => {
    objetodatatable = $("#tableEquivalenciaSede").DataTable({
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
            {
                targets: [2, 3],
                className: "text-center"
            }
        ],
        columns: [
            { data: "NombreEmpresa", title: "Empresa" },
            { data: "NombreSede", title: "Sede" },
            { data: "CodSedeOfisis", title: "Cod. Ofisis" },
            {
                data: "IdEquivalenciaSede",
                title: "Acción",
                bSortable: false,
                render: function (id) {
                    let btnEdit = `<button type="button" class="btn btn-xs btn-warning btnEditar" data-nombre="Editar Equivalencia de Sede" data-id="${id}"><i class="glyphicon glyphicon-pencil"></i></button> `;
                    let btnDelete = `<button type="button" class="btn btn-xs btn-danger btnEliminar" data-nombre="Eliminar Equivalencia de Sede" data-id="${id}"><i class="glyphicon glyphicon-trash"></i></button> `;
                    return btnEdit + btnDelete;
                }
            }
        ],
        "drawCallback": function (settings) {
            $('.btnEditar').tooltip({
                title: "Editar Equivalencia de Sede"
            });

            $('.btnEliminar').tooltip({
                title: "Eliminar Equivalencia de Sede"
            });
        }
    });
    $('.btnEditar').tooltip({
        title: "Editar Equivalencia de Sede"
    });
    $('.btnELiminar').tooltip({
        title: "Eliminar Equivalencia de Sede"
    });
}
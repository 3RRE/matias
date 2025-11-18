function valideKeyNumber(evt) {
    var code = (evt.which) ? evt.which : evt.keyCode;
    if (code >= 48 && code <= 57 || code == 13) {
        return true;
    }
    return false;
}

$(document).ready(function () {
    listarEquivalenciaEmpresas();

    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var nombre = $(this).data("nombre");
        $("#hddmodal").val(nombre);
        $('#bodyModal').html("");
        $('#default-modal-label').html(nombre);
        var redirectUrl = basePath + "EquivalenciaEmpresa/EquivalenciaEmpresaActualizarVista/" + id;
        var ubicacion = "bodyModal";
        $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
            $("#modalGroup").modal("show");
        });
    });

    $(document).on('click', '.btnEliminar', function (e) {
        var id = $(this).data("id");

        var data = {
            idEquivalenciaEmpresa: id
        };

        $.confirm({
            title: 'Confirmación',
            content: '¿Desea eliminar la Equivalencia de Empresa?<p><b>Se eliminarán todas las sedes relacionadas a la empresa.</b></p>',
            confirmButton: 'Sí',
            cancelButton: 'No',
            confirm: function () {
                $.ajax({
                    url: basePath + "EquivalenciaEmpresa/EliminarEquivalenciaEmpresaJSON",
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
                            listarEquivalenciaEmpresas();
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

    $(document).on('click', '#btnNuevaEquivalenciaEmpresa', function (e) {
        var nombre = $(this).data("nombre");
        $("#hddmodal").val(nombre);
        $('#bodyModal').html("");
        $('#default-modal-label').html(nombre);
        var redirectUrl = basePath + "EquivalenciaEmpresa/EquivalenciaEmpresaInsertarVista";
        var ubicacion = "bodyModal";
        $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
            $("#modalGroup").modal("show");
        });
    });
});

const listarEquivalenciaEmpresas = () => {
    let url = basePath + "EquivalenciaEmpresa/ListarTodasLasEquivalenciasEmpresaActivasJSON";
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

const createDataTable = (data) => {
    objetodatatable = $("#tableEquivalenciaEmpresa").DataTable({
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
                targets: [1, 2, 3],
                className: "text-center"
            }
        ],
        columns: [
            { data: "Nombre", title: "Empresa" },
            { data: "CodEmpresaOfisis", title: "Cod. Ofisis" },
            { data: "IdEmpresaBuk", title: "ID Buk" },
            {
                data: "IdEquivalenciaEmpresa",
                title: "Acción",
                bSortable: false,
                render: function (id) {
                    let btnEdit = `<button type="button" class="btn btn-xs btn-warning btnEditar" data-nombre="Editar Equivalencia de Empresa" data-id="${id}"><i class="glyphicon glyphicon-pencil"></i></button> `;
                    let btnDelete = `<button type="button" class="btn btn-xs btn-danger btnEliminar" data-nombre="Eliminar Equivalencia de Empresa" data-id="${id}"><i class="glyphicon glyphicon-trash"></i></button> `;
                    return btnEdit + btnDelete;
                }
            }
        ],
        "drawCallback": function (settings) {
            $('.btnEditar').tooltip({
                title: "Editar Equivalencia de Empresa"
            });

            $('.btnEliminar').tooltip({
                title: "Eliminar Equivalencia de Empresa"
            });
        }
    });
    $('.btnEditar').tooltip({
        title: "Editar Equivalencia de Empresa"
    });
    $('.btnELiminar').tooltip({
        title: "Eliminar Equivalencia de Empresa"
    });
}
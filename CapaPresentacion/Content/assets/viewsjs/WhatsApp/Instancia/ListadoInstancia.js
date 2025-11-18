$(document).ready(function (){
    listarInstancias();

    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var nombre = $(this).data("nombre");
        $("#hddmodal").val(nombre);
        $('#bodyModal').html("");
        $('#default-modal-label').html(nombre);
        var redirectUrl = basePath + "Instancia/InstanciaActualizarVista/" + id;
        var ubicacion = "bodyModal";
        $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
            $("#modalGroup").modal("show");
        });
    });

    $(document).on('click', '.btnEliminar', function (e) {
        var id = $(this).data("id");

        var data = {
            idInstancia: id
        };

        $.confirm({
            title: 'Confirmación',
            content: '¿Desea eliminar la Instancia?',
            confirmButton: 'Sí',
            cancelButton: 'No',
            confirm: function () {
                $.ajax({
                    url: basePath + "Instancia/EliminarInstanciaJSON",
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
                            listarInstancias();
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

    $(document).on('click', '#btnNuevaInstancia', function (e) {
        var nombre = $(this).data("nombre");
        $("#hddmodal").val(nombre);
        $('#bodyModal').html("");
        $('#default-modal-label').html(nombre);
        var redirectUrl = basePath + "Instancia/InstanciaInsertarVista";
        var ubicacion = "bodyModal";
        $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
            $("#modalGroup").modal("show");
        });
    });
});

const listarInstancias = () => {
    let url = basePath + "Instancia/ListarTodasLasInstanciasJSON";
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
    objetodatatable = $("#tableInstancia").DataTable({
        bDestroy: true,
        bSort: true,
        scrollCollapse: true,
        scrollX: true,
        paging: true,
        autoWidth: true,
        bProcessing: true,
        bDeferRender: true,
        data: data,
        columns: [
            { data: "NombreSala", title: "Sala" },
            { data: "UrlBase", title: "Url Base" },
            { data: "Instancia", title: "Instancia" },
            { data: "Token", title: "Token" },
            {
                data: "IdInstanciaUltraMsg",
                title: "Acción",
                bSortable: false,
                render: function (id) {
                    let btnEdit = `<button type="button" class="btn btn-xs btn-warning btnEditar" data-nombre="Editar Instancia" data-id="${id}"><i class="glyphicon glyphicon-pencil"></i></button> `;
                    let btnDelete = `<button type="button" class="btn btn-xs btn-danger btnEliminar" data-nombre="Eliminar Instancia" data-id="${id}"><i class="glyphicon glyphicon-trash"></i></button> `;
                    return btnEdit + btnDelete;
                }
            }
        ],
        "drawCallback": function (settings) {
            $('.btnEditar').tooltip({
                title: "Editar Instancia"
            });

            $('.btnEliminar').tooltip({
                title: "Eliminar Instancia"
            });
        }
    });
    $('.btnEditar').tooltip({
        title: "Editar Instancia"
    });
    $('.btnELiminar').tooltip({
        title: "Eliminar Instancia"
    });
}
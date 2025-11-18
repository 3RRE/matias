const btnNuevo = $("#btnNuevo");

$(document).ready(() => {
    getCargos();
})

btnNuevo.on('click', function () {
    const $this = $(this);
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}BotCargos/Guardar`, 'bodyModal')).then(() => {
        $("#full-modal").modal("show");
    });
});

$(document).on("click", ".btnEliminar", function () {
    const $this = $(this);
    var id = $this.data("id");
    $.confirm({
        title: '¿Esta seguro de Continuar?',
        content: '¿Eliminar Cargo?',
        confirmButton: 'Ok',
        cancelButton: 'Cerrar',
        confirmButtonClass: 'btn-success',
        cancelButtonClass: 'btn-danger',
        confirm: () => {
            deleteCargo(id)
        }
    });
});

$(document).on("click", ".btnEditar", function () {
    const $this = $(this);
    const id = $this.data("id");
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}BotCargos/Guardar/${id}`, 'bodyModal')).then(() => {
        $("#full-modal").modal("show");
    });
});

const getCargos = () => {
    const url = `${basePath}BotCargos/GetCargos`;
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            createDataTable(response.data)
            if (!response.success) {
                toastr.error(response.displayMessage, "Mensaje Servidor")
                return;
            }
        }
    });
};

const createDataTable = (data) => {
    objetodatatable = $("#table").DataTable({
        bDestroy: true,
        bSort: true,
        scrollCollapse: true,
        scrollX: true,
        paging: true,
        autoWidth: true,
        bProcessing: true,
        bDeferRender: true,
        columnDefs: [
            {
                targets: "_all",
                className: "text-center"
            }
        ],
        data: data,
        columns: [
            { data: "Id", title: "Código" },
            { data: "NombreArea", title: "Área" },
            { data: "Nombre", title: "Nombre" },
            {
                data: "Id", title: "Acción", bSortable: false,
                render: (value) => {
                    return `
                        <button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${value}" data-nombre="Editar Cargo"><i class="glyphicon glyphicon-pencil"></i></button>
                        <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${value}"><i class="glyphicon glyphicon-trash"></i></button>
                    `;
                }
            }
        ],
        drawCallback: () => {
            $('.btnEditar').tooltip({
                title: "Editar"
            });
            $('.btnEliminar').tooltip({
                title: "Eliminar"
            });
        },
    });
}

const deleteCargo = (id) => {
    $.ajax({
        url: `${basePath}BotCargos/DeleteCargo/${id}`,
        type: "POST",
        contentType: "application/json",
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            if (!response.success) {
                toastr.error(response.displayMessage, "Mensaje Servidor");
                return;
            }
            toastr.success(response.displayMessage, "Mensaje Servidor");
            getCargos();
        }
    });
}

const btnNuevo = $("#btnNuevo");

$(document).ready(() => {
    getContactos();
})

btnNuevo.on('click', function () {
    const $this = $(this);
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}BotContactos/Guardar`, 'bodyModal')).then(() => {
        $("#full-modal").modal("show");
    });
});

$(document).on("click", ".btnEliminar", function () {
    const $this = $(this);
    var id = $this.data("id");
    $.confirm({
        title: '¿Esta seguro de Continuar?',
        content: '¿Eliminar Contacto?',
        confirmButton: 'Ok',
        cancelButton: 'Cerrar',
        confirmButtonClass: 'btn-success',
        cancelButtonClass: 'btn-danger',
        confirm: () => {
            deleteContacto(id)
        }
    });
});

$(document).on("click", ".btnEditar", function () {
    const $this = $(this);
    const id = $this.data("id");
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}BotContactos/Guardar/${id}`, 'bodyModal')).then(() => {
        $("#full-modal").modal("show");
    });
});

const getContactos = () => {
    const url = `${basePath}BotContactos/GetContactos`;
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
            { data: "NombreCargo", title: "Cargo" },
            { data: "Nombre", title: "Nombre" },
            {
                data: null, title: "Celular", bSortable: false,
                render: (value, type, item) => {
                    return `${item.CodigoPaisCelular} - ${item.Celular}`;
                }
            },
            { data: "Correo", title: "Correo" },
            {
                data: "Id", title: "Acción", bSortable: false,
                render: (value) => {
                    return `
                        <button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${value}" data-nombre="Editar Contacto"><i class="glyphicon glyphicon-pencil"></i></button>
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

const deleteContacto = (id) => {
    $.ajax({
        url: `${basePath}BotContactos/DeleteContacto/${id}`,
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
            getContactos();
        }
    });
}

const btnNuevo = $("#btnNuevo");

$(document).ready(() => {
    ObtenerSistemas()
})

btnNuevo.on('click', function () {
    const $this = $(this);
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}Sistema/AgregarEditarSistemaView`, 'bodyModal')).then(() => {
        $("#full-modal").modal("show");
    })
})

$(document).on("click", ".btnEditar", function () {
    const $this = $(this);
    const id = $this.data("id");
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}Sistema/AgregarEditarSistemaView/${id}`, 'bodyModal')).then(() => {
        $("#full-modal").modal("show");
    })
})

$(document).on("click", ".btnEliminar", function () {
    const $this = $(this);
    const id = $this.data("id");
    $.confirm({
        title: '¿Estas seguro de Continuar?',
        content: '¿Eliminar registro',
        confirmButton: 'Ok',
        cancelButton: 'Cerrar',
        confirmButtonClass: 'btn-success',
        cancelButtonClass: 'btn-danger',
        confirm: () => {
            EliminarSistema(id);
        }
    })
})

const ObtenerSistemas = () => {
    const url = `${basePath}Sistema/ObtenerSistemas`;
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            createDataTable(response.data);
            if (!response.success) {
                toastr.error(response.displayMessage, "Mensaje Servidor");
                return;
            }
        },
        error: () => {
            toastr.error("Eror al obtener los sistemas.", "Error");
        }
    })
}

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
            { targets: "_all", className: "text-center" }
        ],
        data: data,
        columns: [
            { data: "Id", title: "Código" },
            { data: "Nombre", title: "Nombre" },
            { data: "Descripcion", title: "Descripción" },
            {
                data: "Id", title: "Acción", bSortable: false, render: (value) => {
                    const btnEdit = `
                        <button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${value}" data-nombre ="Editar Sistema">
                        <i class="glyphicon glyphicon-edit"></i>
                        </button>
                    `;
                    const btnDelete = `
                        <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${value}" data-nombre="Eliminar Sistema">
                        <i class="glyphicon glyphicon-trash"></i>
                        </button>
                    `;
                    return `
                        ${btnEdit}
                    `;
                }
            }
        ]
    })
}

const EliminarSistema = (id) => {
    const data = { id };
    $.ajax({
        url: `${basePath}Sistema/EliminarSistema`,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            if (!response.success) {
                toastr.error(response.displayMessage, "Mensaje Servidor");
                return;
            }
            toastr.success(response.displayMessage, "Mensaje Servidor");
            ObtenerSistemas();
        },
        error: () => {
            toastr.error("Error al eliminar el sistema.", "Error")
        }
    })
}
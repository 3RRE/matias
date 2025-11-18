const btnNuevo = $("#btnNuevo");

$(document).ready(() => {
    getEmpleados();
})

btnNuevo.on('click', function () {
    const $this = $(this);
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}BotEmpleados/Guardar`, 'bodyModal')).then(() => {
        $("#full-modal").modal("show");
    });
});

$(document).on("click", ".btnEliminar", function () {
    const $this = $(this);
    var id = $this.data("id");
    $.confirm({
        title: '¿Esta seguro de Continuar?',
        content: '¿Eliminar Empleado?',
        confirmButton: 'Ok',
        cancelButton: 'Cerrar',
        confirmButtonClass: 'btn-success',
        cancelButtonClass: 'btn-danger',
        confirm: () => {
            deleteEmpleado(id)
        }
    });
});

$(document).on("click", ".btnEditar", function () {
    const $this = $(this);
    const id = $this.data("id");
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}BotEmpleados/Guardar/${id}`, 'bodyModal')).then(() => {
        $("#full-modal").modal("show");
    });
});

const getEmpleados = () => {
    const url = `${basePath}BotEmpleados/GetEmpleados`;
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
            { data: "NumeroDocumento"},
            {
                data: "NombreCompleto",
                render: (value, type) => {
                    if (type === 'display') {
                        return value ? value : '<span class="badge badge-danger">Sin Nombre</span>';
                    }
                    return value || "";
                }
            },
            { data: "Empresa" },
            { data: "Area" },
            { data: "Cargo" },
            { data: "TelefonoParticular" },
            {
                data: "Estado",
                render: (value, type) => {
                    if (type === 'display') {
                        const clase = value ? 'success' : 'danger';
                        const estado = value ? 'Activo' : 'Inactivo';
                        return `<span class="badge badge-${clase}">${estado}</span>`;
                    }
                    return value || "";
                }
            },
            {
                data: "Id", bSortable: false,
                render: (value) => {
                    return `
                        <button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${value}" data-nombre="Editar Empleado"><i class="glyphicon glyphicon-pencil"></i></button>
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

const deleteEmpleado = (id) => {
    $.ajax({
        url: `${basePath}BotEmpleados/DeleteEmpleado/${id}`,
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
            getEmpleados();
        }
    });
}

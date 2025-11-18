const cboSistema = $("#cboSistema")
const btnNuevo = $("#btnNuevo");

$(document).ready(() => {
    ObtenerCredencialesCorreo();
    ObtenerSistemas()
});

const ObtenerSistemas = () => {
    $.when(
        llenarSelect(`${basePath}Sistema/ObtenerSistemas`, {}, "cboSistema", "Id", "Nombre")
    ).then(() => {
        cboSistema.select2({
            placeholder: "Seleccione...",
            multiple: false,
        })
    })
}


cboSistema.on('change', () => {
    const IdSistema = cboSistema.val()
    if (!IdSistema) {
        toastr.waning("Seleccione un sistema", "Aviso")
        return;
    }
    ObtenerCredencialesCorreoPorSistema(IdSistema)
})

const ObtenerCredencialesCorreoPorSistema = (IdSistema) => {
    const url = `${basePath}CredencialCorreo/ObtenerCredencialesCorreoPorSistema`;
    const data = { IdSistema };
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
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
            toastr.error("Error al obtener los correos.", "Error");
        }
    });
};

btnNuevo.on('click', function () {
    const $this = $(this);
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}CredencialCorreo/AgregarEditarCredencialCorreoView`, 'bodyModal')).then(() => {
        $("#full-modal").modal("show");
    });
});

$(document).on("click", ".btnEditar", function () {
    const $this = $(this);
    const id = $this.data("id");
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}CredencialCorreo/AgregarEditarCredencialCorreoView/${id}`, 'bodyModal')).then(() => {
        $("#full-modal").modal("show");
    });
});

$(document).on("click", ".btnEliminar", function () {
    const $this = $(this);
    const id = $this.data("id");
    $.confirm({
        title: '¿Estás seguro de Continuar?',
        content: '¿Eliminar registro?',
        confirmButton: 'Ok',
        cancelButton: 'Cerrar',
        confirmButtonClass: 'btn-success',
        cancelButtonClass: 'btn-danger',
        confirm: () => {
            EliminarCredencialCorreo(id);
        }
    });
});

const ObtenerCredencialesCorreo = () => {
    const url = `${basePath}CredencialCorreo/ObtenerCredencialesCorreo`;
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
            toastr.error("Error al obtener las credenciales.", "Error");
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
            { targets: "_all", className: "text-center" }
        ],
        data: data,
        columns: [
            { data: "Id", title: "Id" },
            { data: "NombreSistema", title: "Sistema" },
            { data: "NombreRemitente", title: "Nombre Remitente" },
            { data: "Correo", title: "Correo" },
            { data: "ServidorSMTP", title: "Servidor SMTP" },
            { data: "PuertoSMTP", title: "Puerto SMTP" },
            { data: "SSLHabilitadoStr", title: "SSL Habilitado" },
            { data: "CuotaDiaria", title: "Cuota Diaria" },
            { data: "Prioridad", title: "Prioridad" },
            { data: "EstadoStr", title: "Estado" },
            {
                data: "Id", title: "Acción", bSortable: false, render: (value) => {
                    const btnEdit = `
                        <button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${value}" data-nombre="Editar Credencial">
                            <i class="glyphicon glyphicon-edit"></i>
                        </button>
                    `;
                    const btnDelete = `
                        <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${value}" data-nombre="Eliminar Credencial">
                            <i class="glyphicon glyphicon-trash"></i>
                        </button>
                    `;
                    return `
                        ${btnEdit}
                    `;
                }
            }
        ]
    });
};
const EliminarCredencialCorreo = (id) => {
    const data = { id };
    $.ajax({
        url: `${basePath}CredencialCorreo/EliminarCredencialCorreo`,
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

            if (cboSistema.val()) {
                ObtenerCredencialesCorreoPorSistema(cboSistema.val())
            } else {
                ObtenerCredencialesCorreo()
            }
        },
        error: () => {
            toastr.error("Error al eliminar la credencial.", "Error");
        }
    });
};
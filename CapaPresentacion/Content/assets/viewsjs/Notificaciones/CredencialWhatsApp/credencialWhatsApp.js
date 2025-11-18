const cboSistema = $("#cboSistema")
const btnNuevo = $("#btnNuevo");

$(document).ready(() => {
    ObtenerCredencialesWhatsApp();
    ObtenerSistemas();
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
    ObtenerCredencialesWhatsAppPorSistema(IdSistema)
})

const ObtenerCredencialesWhatsAppPorSistema = (IdSistema) => {
    const url = `${basePath}CredencialWhatsApp/ObtenerCredencialesWhatsAppPorSistema`;
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
    $.when(DataPostModo1(`${basePath}CredencialWhatsApp/AgregarEditarCredencialWhatsAppView`, 'bodyModal')).then(() => {
        $("#full-modal").modal("show");
    });
});

$(document).on("click", ".btnEditar", function () {
    const $this = $(this);
    const id = $this.data("id");
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}CredencialWhatsApp/AgregarEditarCredencialWhatsAppView/${id}`, 'bodyModal')).then(() => {
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
            EliminarCredencialWhatsApp(id);
        }
    });
});

const ObtenerCredencialesWhatsApp = () => {
    const url = `${basePath}CredencialWhatsApp/ObtenerCredencialesWhatsApp`;
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
            { data: "UrlBase", title: "URL Base" },
            { data: "Instancia", title: "Instancia" },
            { data: "Token", title: "Token" },
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

const EliminarCredencialWhatsApp = (id) => {
    const data = { id };
    $.ajax({
        url: `${basePath}CredencialWhatsApp/EliminarCredencialWhatsApp`,
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
                ObtenerCredencialesWhatsAppPorSistema(cboSistema.val())
            } else {
                ObtenerCredencialesWhatsApp()
            }        },
        error: () => {
            toastr.error("Error al eliminar la credencial.", "Error");
        }
    });
};
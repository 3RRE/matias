const cboSala = $("#cboSala");
const btnNuevo = $("#btnNuevo");

$(document).ready(() => {
    getSalas();
    createDataTable([]);
})

cboSala.on('change', () => {
    const codSala = cboSala.val();
    if (!codSala) {
        toastr.warning("Seleccione una sala", "Aviso")
        return;
    }
    getBaresByCodSala(codSala);
});

btnNuevo.on('click', function() {
    const codSala = cboSala.val()
    if (!codSala) {
        toastr.warning("Seleccione una sala", "Mensaje servidor")
        return;
    }
    const $this = $(this);
    const nombre = $this.data("nombre");
    const data = {
        codSala,
        id: 0
    };
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}Bares/Guardar`, 'bodyModal', data)).then(() => {
        $("#full-modal").modal("show");
    });
});

$(document).on("click", ".btnEliminar", function() {
    const $this = $(this);
    var id = $this.data("id");
    $.confirm({
        title: '¿Esta seguro de Continuar?',
        content: '¿Eliminar Bar?',
        confirmButton: 'Ok',
        cancelButton: 'Cerrar',
        confirmButtonClass: 'btn-success',
        cancelButtonClass: 'btn-danger',
        confirm: () => {
            deleteBar(cboSala.val(), id)
        }
    });
});

$(document).on("click", ".btnEditar", function() {
    const $this = $(this);
    const id = $this.data("id");
    console.log(id)
    const nombre = $this.data("nombre");
    const data = {
        codSala: cboSala.val(),
        id
    };
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}Bares/Guardar`, 'bodyModal', data)).then(() => {
        $("#full-modal").modal("show");
    });
});

const getBaresByCodSala = (codSala) => {
    const url = `${basePath}Bares/GetBares`;
    const data = {
        codSala
    };
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            if (!response.success) {
                toastr.error(response.displayMessage, "Mensaje Servidor")
                return;
            }
            createDataTable(response.data)
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
            { data: "Nombre", title: "Nombre" },
            {
                data: "Estado", title: "Estado",
                render: (value) => {
                    const estado = value ? "ACTIVO" : "INACTIVO";
                    const css = value ? "btn-success" : "btn-danger";
                    return `<span class="label ${css}" style="width:100%; display:block;padding:5px 0">${estado}</span>`;
                },
            },
            {
                data: "Id", title: "Acción", bSortable: false,
                render: (value) => {
                    return `
                        <button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${value}" data-nombre="Editar Bar"><i class="glyphicon glyphicon-pencil"></i></button>
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

const getSalas = () => {
    $.when(
        llenarSelect(`${basePath}Sala/ListadoSalaPorUsuarioJson`, {}, "cboSala", "CodSala", "Nombre")
    ).then(() => {
        cboSala.select2({
            placeholder: "Seleccione ...",
            multiple: false,
        });
    });
}

const deleteBar = (codSala, idBar) => {
    const data = {
        codSala,
        id: idBar
    };
    $.ajax({
        url: `${basePath}Bares/DeleteBar`,
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
            getBaresByCodSala(codSala);
        }
    });
}

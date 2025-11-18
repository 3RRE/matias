const cboSala = $("#cboSala");
const btnNuevo = $("#btnNuevo");

$(document).ready(() => {
    createDataTable([]);
    getSalas();
})

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

cboSala.on('change', () => {
    const codSala = cboSala.val();
    if (!codSala) {
        toastr.warning("Seleccione una sala", "Aviso")
        return;
    }
    getPreguntasByCodSala(codSala);
});

btnNuevo.on('click', function () {
    const $this = $(this);
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}EscPreguntas/AgregarEditarPreguntaView`, 'bodyModal')).then(() => {
        $("#full-modal").modal("show");
    });
});

$(document).on("click", ".btnEditar", function () {
    const $this = $(this);
    const id = $this.data("id");
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}EscPreguntas/AgregarEditarPreguntaView/${id}`, 'bodyModal')).then(() => {
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
            deletePregunta(id);
        }
    });
});

const getPreguntasByCodSala = (codSala) => {
    const url = `${basePath}EscPreguntas/GetPreguntasByCodSala`;
    const data = { codSala };

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
            toastr.error("Error al obtener las preguntas.", "Error");
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
            { data: "Id", title: "Código" },
            { data: "Sala.Nombre", title: "Sala" }, 
            { data: "Texto", title: "Texto" }, 
            {
                data: "EsObligatoriaStr",
                title: "Obligatoria"
            },
            //{
            //    data: "EstadoStr",
            //    title: "Estado",
            //    render: (value, x, row) => {
            //        const css = row.Estado ? "btn-success" : "btn-danger";
            //        return `<span class="label ${css}" style="width:100%; display:block;padding:5px 0">${value}</span>`;
            //    }
            //},
            {
                data: "Id",
                title: "Acción",
                bSortable: false,
                render: (value) => {
                    const btnEdit = `
                        <button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${value}" data-nombre="Editar Pregunta">
                            <i class="glyphicon glyphicon-edit"></i>
                        </button>
                    `;
                    const btnDelete = `
                        <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${value}" data-nombre="Eliminar Pregunta">
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

const deletePregunta = (id) => {
    const data = { id }; 

    $.ajax({
        url: `${basePath}EscPreguntas/DeletePregunta`, 
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

            const codSala = $("#cboSala").val();
            getPreguntasByCodSala(codSala);
        },
        error: () => {
            toastr.error("Error al eliminar la pregunta.", "Error"); 
        }
    });
};
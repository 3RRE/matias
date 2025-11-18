const cboSala = $("#cboSala");

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
    getProductosByCodSala(codSala);
});



$(document).on("click", ".btnAsignar", function () {
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
    $.when(DataPostModo1(`${basePath}Cortesias/ProductoSalaById`, 'bodyModal', data)).then(() => {
        $("#full-modal").modal("show");
    });
});

const getProductosByCodSala = (codSala) => {
    const url = `${basePath}Cortesias/GetProductosSala`;
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
            { data: "NombreTipo", title: "Tipo" },
            { data: "NombreSubTipo", title: "SubTipo" },
            { data: "NombreMarca", title: "Marca" },
            { data: "Nombre", title: "Nombre" },
            { data: "Cantidad", title: "Cantidad" },
            { data: "Precio", title: "Precio" },
            {
                data: "isChecked", title: "Estado",
                render: (value) => {
                    const estado = value ? "ASIGNADO" : "NO ASIGNADO";
                    const css = value ? "btn-success" : "btn-danger";
                    return `<span class="label ${css}" style="width:100%; display:block;padding:5px 0">${estado}</span>`;
                },
            },
            {
                data: "Id", title: "Acción", bSortable: false,
                render: (value) => {
                    return `
                        <button type="button" class="btn btn-xs btn-success btnAsignar" data-id="${value}" data-nombre="Asignar Producto"><i class="glyphicon glyphicon-plus"></i></button>
                    `;
                }
            }
        ],
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
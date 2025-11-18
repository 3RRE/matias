const btnNuevo = $("#btnNuevo");

$(document).ready(() => {
    getProductos();
})

btnNuevo.on('click', function () {
    const $this = $(this);
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}Productos/Guardar`, 'bodyModal')).then(() => {
        $("#full-modal").modal("show");
    });
});

$(document).on("click", ".btnEliminar", function () {
    const $this = $(this);
    var id = $this.data("id");
    $.confirm({
        title: '¿Esta seguro de Continuar?',
        content: '¿Eliminar Producto?',
        confirmButton: 'Ok',
        cancelButton: 'Cerrar',
        confirmButtonClass: 'btn-success',
        cancelButtonClass: 'btn-danger',
        confirm: () => {
            deleteProducto(id)
        }
    });
});

$(document).on("click", ".btnEditar", function () {
    const $this = $(this);
    const id = $this.data("id");
    const nombre = $this.data("nombre");
    $('#bodyModal').html("");
    $('#full-modal-label').html(nombre);
    $.when(DataPostModo1(`${basePath}Productos/Guardar/${id}`, 'bodyModal')).then(() => {
        $("#full-modal").modal("show");
    });
});

const getProductos = () => {
    const url = `${basePath}Productos/GetProductos`;
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
            { data: "NombreTipo", title: "Tipo" },
            { data: "NombreSubTipo", title: "SubTipo" },
            { data: "NombreMarca", title: "Marca" },
            { data: "Nombre", title: "Nombre" },
            {
                data: "ImagenBase64", title: "Foto"
                , "render":
                    function (value) {
                        let myLink = ` <img src="${value}"  width="40" height="40" alt="Producto" />`
                        return myLink;
                    },
                "orderable": false,
                className: "text-center",
            },
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
                        <button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${value}" data-nombre="Editar Producto"><i class="glyphicon glyphicon-pencil"></i></button>
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

const deleteProducto = (id) => {
    $.ajax({
        url: `${basePath}Productos/DeleteProducto/${id}`,
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
            getProductos();
        }
    });
}

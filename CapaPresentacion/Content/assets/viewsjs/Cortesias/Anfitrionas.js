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
    // Dividir el valor de codSala en dos partes
    const [empresa, sala] = codSala.split('-'); // x será '26', y será '003'

    if (!empresa || !sala) {
        toastr.error("Sala no tiene anfitrionas", "Error");
        return;
    }
    getAnfitrionas(empresa,sala);
});



$(document).on("click", ".btnAsignar", function () {
    const $this = $(this);
    const id = $this.data("id");
    const codSala = cboSala.val();

    // Dividir el valor de codSala en dos partes
    const [empresa, sala] = codSala.split('-'); // x será '26', y será '003'

    if (!empresa || !sala) {
        toastr.error("Seleccione la sala correcta", "Error");
        return;
    }

    createAnfitriona(empresa,sala, id);
    //$('#bodyModal').html("");
    //$('#full-modal-label').html(nombre);
    //$.when(DataPostModo1(`${basePath}Cortesias/ProductoSalaById`, 'bodyModal', data)).then(() => {
    //    $("#full-modal").modal("show");
    //});
});

const createAnfitriona = (empresa,sala, id) => {
    const url = `${basePath}Anfitrionas/CreateAnfitriona`;
    const data = {
        empresa,
        sala,
        id
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
            } else {
                console.log(response)
                toastr.success("Anfitriona generada.", "Mensaje Servidor")
            }
        }
    });
};

const getAnfitrionas = (empresa,sala) => {
    const url = `${basePath}Anfitrionas/GetAnfitrionasBySala`;
    const data = {
        empresa,
        sala
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
            /*{ data: "EmpIdBuk", title: "ID BUK" },*/
            { data: "Empresa", title: "Empresa" },
            { data: "Sede", title: "Sede" },
            { data: "EmpNombre", title: "Nombre" },
            { data: "EmpApePaterno", title: "Apellido Paterno" },
            { data: "EmpApeMaterno", title: "Apellido Materno" },
            { data: "EmpNroDocumento", title: "Nro Documento" },
            //{ data: "CoEmpr", title: "Código Empresa" },
            //{ data: "CoSede", title: "Código Sede" },
            { data: "Puesto", title: "Puesto" },
            {
                data: "EmpEstado", title: "Estado",
                render: (value) => {
                    const estado = value ? "Activo" : "Inactivo";
                    const css = value ? "btn-success" : "btn-danger";
                    return `<span class="label ${css}" style="width:100%; display:block;padding:5px 0">${estado}</span>`;
                },
            },
            { data: "EmpCorreo", title: "Correo" },
            {
                data: "EmpNroDocumento", title: "Acción", bSortable: false,
                render: (value, x, oData) => {
                    return `
                        <button type="button" class="btn btn-xs btn-success btnAsignar" data-id="${value}""><i class="glyphicon glyphicon-plus"></i></button>
                    `;
                }
            }
        ],
    });
}

const getSalas = () => {
    $.when(
        llenarSelect(`${basePath}Sala/ListadoSalaPorUsuarioOfisisJson`, {}, "cboSala", "CodOfisis", "Nombre")
    ).then(() => {
        cboSala.select2({
            placeholder: "Seleccione ...",
            multiple: false,
        });
    });
}
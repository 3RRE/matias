const btnBuscar = $('#btnBuscar');
const btnExcel = $('#btnExcel');

$(document).ready(() => {
    createDataTable([]);
})

btnExcel.on('click', () => {
    const filters = getFilters();
    if (!filters) {
        return;
    }
    generateExcelTransaccionesCliente(filters);
})

btnBuscar.on('click', () => {
    const filters = getFilters();
    if (!filters) {
        return;
    }
    getTransaccionesCliente(filters)
})

const generateExcelTransaccionesCliente = (filters) => {
    const url = `${basePath}TtcTransacciones/GenerarExcelTransaccionesDeClientesPorSala`;
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(filters),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            console.log(response)
            const displayMessage = response.displayMessage;
            if (!response.success) {
                toastr.error(displayMessage);
                return;
            }
            toastr.success(displayMessage);
            const dataExcel = response.data.bytes
            const linkExcel = document.createElement('a')
            document.body.appendChild(linkExcel) //required in FF, optional for Chrome
            linkExcel.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + dataExcel
            linkExcel.download = `${response.data.fileName}`
            linkExcel.click()
            linkExcel.remove()
        }
    });
}

const getTransaccionesCliente = (filters) => {
    const url = `${basePath}TtcTransacciones/ObtenerTransaccionesDeClientesPorSala`;
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(filters),
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
                targets: [0],
                width: "50px",

            },
            {
                targets: "_all",
                className: "text-center"
            }
        ],
        data: data,
        columns: [
            { data: "ItemVoucher", title: "Item" },
            { data: "Cliente.NombreCompleto", title: "Cliente" },
            { data: "Cliente.NumeroDocumento", title: "Doc. Identidad" },
            { data: "Tarjeta.MedioPago", title: "Medio de Pago" },
            { data: "Tarjeta.EntidadEmisora", title: "Entidad Emisora" },
            { data: "Tarjeta.Tipo", title: "Tipo" },
            { data: "Monto", title: "Monto (S/)" },
            { data: "Tarjeta.Numero", title: "Nro. Tarjeta" },
            { data: "FechaRegistroStr", title: "Fecha" },
            { data: "Caja.Numero", title: "Caja" },
            { data: "Caja.Turno", title: "Turno" },
        ],
    });
}


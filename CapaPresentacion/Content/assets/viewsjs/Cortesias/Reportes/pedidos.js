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
    generateExcelReportePedidos(filters);
})

btnBuscar.on('click', () => {
    const filters = getFilters();
    if (!filters) {
        return;
    }
    getReportePedidos(filters)
})

const generateExcelReportePedidos = (filters) => {
    const url = `${basePath}ReportesCortesia/GenerarExcelReportePedidos`;
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(filters),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () =>  $.LoadingOverlay("hide"),
        success: (response) => {
            const displayMessage = response.displayMessage;
            if (!response.success) {
                toastr.error(displayMessage);
                return;
            }
            toastr.success(displayMessage);
            const dataExcel = response.bytes
            const linkExcel = document.createElement('a')
            document.body.appendChild(linkExcel) //required in FF, optional for Chrome
            linkExcel.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + dataExcel
            linkExcel.download = `${response.fileInfo.FileName}.xlsx`
            linkExcel.click()
            linkExcel.remove()
        }
    });
}

const getReportePedidos = (filters) => {
    const url = `${basePath}ReportesCortesia/ObtenerReportePedidos`;
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
            { data: "CodSala", title: "Cod. Sala" },
            { data: "Sala", title: "Sala" },
            { data: "IdPedido", title: "Id Pedido" },
            { data: "Productos", title: "Productos" },
            { data: "CodMaquina", title: "Cod. Máquina" },
            { data: "NombreZona", title: "Zona" },
            { data: "Posicion", title: "Posición" },
            { data: "NombreIsla", title: "Isla" },
            { data: "Anfitriona", title: "Anfitriona" },
            { data: "NumeroDocumentoCliente", title: "Nro. Doc. Cliente" },
            { data: "NombreCliente", title: "Cliente" },
        ],
    });
}

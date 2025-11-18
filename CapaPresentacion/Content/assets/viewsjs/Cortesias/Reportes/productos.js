const btnBuscar = $('#btnBuscar');
const btnExcel = $('#btnExcel');
const previewChart = $('#previewChart');
const viewChart = $('#viewChart');
let chartProducts;

$(document).ready(() => {
    createDataTable([]);
    hideChart();
})

btnExcel.on('click', () => {
    const filters = getFilters();
    if (!filters) {
        return;
    }
    generateExcelReporteProductos(filters);
})

btnBuscar.on('click', () => {
    const filters = getFilters();
    if (!filters) {
        return;
    }
    getReporteProductos(filters)
})

const generateExcelReporteProductos = (filters) => {
    const url = `${basePath}ReportesCortesia/GenerarExcelReportePorProducto`;
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

const getReporteProductos = (filters) => {
    const url = `${basePath}ReportesCortesia/ObtenerReportePorProducto`;
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(filters),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            createDataTable(response.data.table)
            createChart(response.data.chart)
            if (!response.success) {
                hideChart();
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
            { data: "NombreTipo", title: "Tipo" },
            { data: "NombreSubTipo", title: "Sub Tipo" },
            { data: "NombreMarca", title: "Marca" },
            { data: "NombreProducto", title: "Producto" },
            { data: "Cantidad", title: "Cantidad" },
        ],
    });
}

const createChart = (dataChart) => {
    clearChart();
    showChart();

    const dataSets = dataChart.DataSets.map(item => ({
        label: item.Sala,
        data: item.Cantidades,
        barPercentage: 1,
    }));

    const data = {
        labels: dataChart.Productos,
        datasets: dataSets
    };

    const config = {
        type: 'bar',
        data: data,
        options: {
            indexAxis: 'y',
            elements: {
                bar: {
                    borderWidth: 2,
                }
            },
            animation: {
                duration: 2000,
                easing: 'easeInOutQuad',
            },
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                },
                title: {
                    display: true,
                    text: 'Productos Consumido por Sala'
                }
            },
            scales: {
                x: {
                    categoryPercentage: 1,
                    barPercentage: 1,
                    ticks: {
                        stepSize: 1
                    }
                },
                y: {
                    beginAtZero: true
                }
            }
        },
    };
    const provenancesCanva = $('#products-chart').get(0).getContext('2d');
    chartProducts = new Chart(provenancesCanva, config)
}

const clearChart = () => {
    hideChart();
    if (chartProducts) {
        chartProducts.destroy();
    }
}

const showChart = () => {
    previewChart.css("display", "none");
    viewChart.css("display", "block");
}

const hideChart = () => {
    previewChart.css("display", "block");
    viewChart.css("display", "none");
}
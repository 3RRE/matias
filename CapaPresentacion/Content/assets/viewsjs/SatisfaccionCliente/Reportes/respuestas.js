const btnBuscar = $('#btnBuscar');
const btnExcel = $('#btnExcel');
const previewChart = $('#previewChart');
const viewChart = $('#viewChart');
let chartResponses;

$(document).ready(() => {
    createDataTable([]);
    hideChart();
})

btnExcel.on('click', () => {
    const filters = getFilters();
    if (!filters) {
        return;
    }
    generateExcelReporteRespuestas(filters);
})

btnBuscar.on('click', () => {
    const filters = getFilters();
    if (!filters) {
        return;
    }
    getReporteRespuestas(filters)
})

const generateExcelReporteRespuestas = (filters) => {
    const url = `${basePath}EscReportes/GenerarExcelReporteDeRespuestas`;
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

const getReporteRespuestas = (filters) => {
    const url = `${basePath}EscReportes/ObtenerReporteDeRespuestas`;
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
            { data: "Pregunta.Sala.CodSala", title: "Cod. Sala" },
            { data: "Pregunta.Sala.Nombre", title: "Sala" },
            { data: "Cliente.NumeroDocumento", title: "Número Documento" },
            { data: "Cliente.NombreCompleto", title: "Cliente" },
            { data: "Pregunta.Texto", title: "Pregunta" },
            { data: "Puntaje", title: "Puntaje" },
            { data: "PuntajeStr", title: "Puntaje Texto" },
            {
                data: "FechaRegistro", title: "Fecha Registro", render: (value) => {
                    return moment(value).format('DD/MM/YYYY HH:mm:ss')
                }
            },
        ],
    });
}

const createChart = (dataChart) => {
    clearChart();
    showChart();


    const totalRegistros = dataChart.DataSets.reduce((sum, item) => sum + item.Cantidades.reduce((a, b) => a + b, 0), 0);

    const dataSets = dataChart.DataSets.map(item => ({
        label: item.Sala,
        data: item.Cantidades,
        barPercentage: 1,
    }));

    const data = {
        labels: dataChart.Puntajes,
        datasets: dataSets
    };

    const config = {
        type: 'bar',
        data: data,
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                },
                title: {
                    display: true,
                    text: `Total de respuestas por Sala (Registros: ${totalRegistros})`
                },
                datalabels: {
                    anchor: 'end',
                    align: 'top',
                    formatter: (value, ctx) => {
                        const dataset = ctx.chart.data.datasets[ctx.datasetIndex].data;
                        const total = dataset.reduce((acc, val) => acc + val, 0);
                        const percentage = total > 0 ? ((value / total) * 100).toFixed(1) + '%' : '0%';
                        return `${value} (${percentage})`;
                    },
                    color: '#000',
                    font: {
                        weight: 'bold'
                    }
                }
            },
            tooltip: {
                callbacks: {
                    label: (tooltipItem) => {
                        const dataset = tooltipItem.dataset.data;
                        const total = dataset.reduce((acc, val) => acc + val, 0);
                        const value = tooltipItem.raw;
                        const percentage = total > 0 ? ((value / total) * 100).toFixed(1) + '%' : '0%';
                        return `${tooltipItem.dataset.label}: ${value} (${percentage})`;
                    }
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
        plugins: [ChartDataLabels]
    };

    const responsesCanva = $('#responses-chart').get(0).getContext('2d');
    chartResponses = new Chart(responsesCanva, config)
}

const clearChart = () => {
    hideChart();
    if (chartResponses) {
        chartResponses.destroy();
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
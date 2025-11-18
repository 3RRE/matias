let arraySalas = []
$(document).ready(function () {
    function getFirstDayOfMonth() {
        const now = moment(); 
        const firstDay = now.startOf('month'); 
        return firstDay.format('DD/MM/YYYY'); 
    }
    
    const dateNowOfMonth = getFirstDayOfMonth(); 
    $(".dateOnlyInicio").datetimepicker({
        pickTime: false, 
        format: 'DD/MM/YYYY', 
        defaultDate: dateNowOfMonth, 
    });
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
    })

    setThemeBrandDarkHighCharts();
    ObtenerListaSalas()

});

$(document).on("click", "#btnBuscar", function () {
    if ($("#cboSala").length == 0 || $("#cboSala").val() == null) {
        toastr.error("Seleccione una sala.")
        return false
    }
    if ($("#fechaInicio").val() === "") {
        toastr.error("Ingrese una fecha de Inicio.")
        return false
    }
    if ($("#fechaFin").val() === "") {
        toastr.error("Ingrese una fecha Fin.")
        return false
    }

    getAllData();

});

function setThemeBrandDarkHighCharts() {

    Highcharts.theme = {
        colors: [
            '#FF6B6B', // Rojo coral suave
            '#FFA07A', // Salmón pastel
            '#FFD166', // Amarillo cálido pastel
            '#06D6A0', // Verde menta pastel
            '#118AB2', // Azul petróleo pastel
            '#9A77CF', // Lila profundo
            '#EF476F', // Rosa intenso
            '#83C5BE'  // Turquesa suave
        ],
        chart: {
            backgroundColor: '#292540',
            style: {
                fontFamily: "'Arial', sans-serif"
            }
        },
        title: {
            style: {
                color: '#ffffff',
                fontSize: '18px'
            }
        },
        subtitle: {
            style: {
                color: '#bbbbbb'
            }
        },
        xAxis: {
            labels: {
                style: {
                    color: '#ffffff'
                }
            },
            gridLineColor: 'rgba(255, 255, 255, 0.2)'
        },
        yAxis: {
            title: {
                style: {
                    color: '#ffffff'
                }
            },
            labels: {
                style: {
                    color: '#ffffff'
                }
            },
            gridLineColor: 'rgba(255, 255, 255, 0.2)'
        },
        legend: {
            itemStyle: {
                color: '#ffffff'
            }
        },
        tooltip: {
            backgroundColor: 'rgba(0, 0, 0, 0.8)',
            style: {
                color: '#ffffff'
            }
        }
    };

    Highcharts.setOptions(Highcharts.theme);
}

function ObtenerListaSalas(value) {
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboSala").html('')
            const codSalaArray = result.data.map(sala => String(sala.CodSala));

            if (result.data) {
                arraySalas = result.data
                arraySelect = [`<option value="${-1}" selected>TODOS</option>`]
                result.data.map(item => arraySelect.push(`<option value="${item.CodSala}">${item.Nombre}</option>`))
                $("#cboSala").html(arraySelect.join(""))
                $("#cboSala").select2({
                    multiple: true, placeholder: "--Seleccione--", allowClear: true
                });
                if (value) {
                    $('#cboSala').val(value).trigger("change");
                } else {
                    $("#cboSala").val(-1).trigger("change")
                }

                getAllData();
            }
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    });
    return false;
}

function getAllData() {
    getDataRecaudacion();
    getDataIncidentesCajasTemporizadas();
    getDataEntesReguladores();
    getDataOcurrenciasLog();
    getDataLudopatas();
}
function showInfo(canvasId, messageId, showChart) {
    const canvas = document.getElementById(canvasId);
    const noDataMessage = document.getElementById(messageId);

    //console.log(canvas)
    //console.log(noDataMessage)

    canvas.style.display = showChart ? "block" : "none";
    noDataMessage.style.display = showChart ? "none" : "block";
}

function getDataRecaudacion() {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();

    $.ajax({
        type: "POST",
        url: basePath + "ESS_Dashboard/ListarDashboardRecaudaciones",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if (response && response.respuesta && response.data.length > 0) {
                generateChartRecaudacion(response.data);
                showInfo("chartRecaudacion", "noDataRecaudacion", true);
            } else {
                showInfo("chartRecaudacion", "noDataRecaudacion", false);
            }
        },
        error: function () {
            showInfo("chartRecaudacion", "noDataRecaudacion", false);
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}
//function generateChartRecaudacion(data) {

//    let salas = {};

//    data.forEach(item => {
//        let sala = item.NombreSala;
//        let estado = item.Estado; // 1 = Participante, 2 = No Participante
//        let funcion = item.NombreFuncion || "Sin Función"; // Si es NULL, se asigna 'Sin Función'

//        if (!salas[sala]) {
//            salas[sala] = { Participante: {}, NoParticipante: {} };
//        }

//        let grupo = (estado === 1) ? "Participante" : "NoParticipante";

//        if (!salas[sala][grupo][funcion]) {
//            salas[sala][grupo][funcion] = 0;
//        }

//        salas[sala][grupo][funcion]++;
//    });

//    let categorias = Object.keys(salas);

//    let seriesData = {};

//    categorias.forEach(sala => {
//        ["Participante", "NoParticipante"].forEach(grupo => {
//            Object.keys(salas[sala][grupo]).forEach(funcion => {
//                if (!seriesData[funcion]) {
//                    seriesData[funcion] = { name: funcion, data: [], stack: grupo };
//                }
//                seriesData[funcion].data.push(salas[sala][grupo][funcion]);
//            });
//        });
//    });

//    let series = Object.values(seriesData);

//    Highcharts.chart('chartRecaudacion', {
//        chart: {
//            type: 'column'
//        },
//        title: {
//            text: 'Total Personal Promedio por Recaudacion',
//            align: 'center'
//        },
//        xAxis: {
//            categories: categorias
//        },
//        yAxis: {
//            allowDecimals: false,
//            min: 0,
//            title: {
//                text: 'Cantidad'
//            }
//        },
//        tooltip: {
//            pointFormat: '<b>{series.name}</b>: {point.y} <br/> Total: {point.stackTotal}'
//        },
//        plotOptions: {
//            column: {
//                stacking: 'normal'
//            }
//        },
//        series: series
//    });
//}
function generateChartRecaudacion(data) {
    let salas = {};

    // Agrupar datos
    data.forEach(item => {
        let sala = item.NombreSala;
        let idRecaudacion = item.IdRecaudacion;
        let idPersonal = item.IdPersonalRecaudacion;

        if (!salas[sala]) {
            salas[sala] = {
                recaudaciones: new Set(),
                personal: new Set()
            };
        }

        salas[sala].recaudaciones.add(idRecaudacion);
        salas[sala].personal.add(idPersonal);
    });

    // Preparar datos para Highcharts
    let categorias = Object.keys(salas);
    let seriesData = categorias.map(sala => {
        let totalRecaudaciones = salas[sala].recaudaciones.size;
        let totalPersonal = salas[sala].personal.size;
        let promedio = totalRecaudaciones > 0 ? (totalPersonal / totalRecaudaciones).toFixed(2) : 0;

        return { name: sala, y: parseFloat(promedio) };
    });

    // Renderizar el gráfico con Highcharts
    Highcharts.chart('chartRecaudacion', {
        chart: {
            type: 'column',
            backgroundColor: '#292540' // Fondo oscuro
        },
        title: {
            text: 'Promedio de Personal por Recaudación',
            align: 'center',
            style: { color: '#ffffff' }
        },
        xAxis: {
            categories: categorias,
            labels: { style: { color: '#ffffff' } }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Promedio de Personal',
                style: { color: '#ffffff' }
            },
            labels: { style: { color: '#ffffff' } },
            gridLineColor: 'rgba(255, 255, 255, 0.2)'
        },
        tooltip: {
            pointFormat: '<b>Promedio</b>: {point.y}',
            backgroundColor: 'rgba(0, 0, 0, 0.8)',
            style: { color: '#ffffff' }
        },
        plotOptions: {
            column: {
                dataLabels: {
                    enabled: true,
                    style: { color: '#ffffff', fontSize: '12px' }
                }
            }
        },
        legend: {
            enabled: false
        },
        series: [{
            name: 'Promedio Personal/Recaudación',
            data: seriesData,
            colorByPoint: true
        }]
    });
}
function getDataIncidentesCajasTemporizadas() {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();

    $.ajax({
        type: "POST",
        url: basePath + "ESS_Dashboard/ListarDashboardCajasTemporizadas",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            const datos = response.data || [];
            if (datos.length > 0) {
                generateChartIncidentesCajasTemporizadas(datos);
                showInfo("chartCajasTemporizadas", "noDataCajasTemporizadas", true);
            } else {
                showInfo("chartCajasTemporizadas", "noDataCajasTemporizadas", false);
            }
        },
        error: function () {
            showInfo("chartCajasTemporizadas", "noDataCajasTemporizadas",false);
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}
function generateChartIncidentesCajasTemporizadas(data) {
    let salas = {};

    data.forEach(item => {
        let sala = item.NombreSala;
        let estado = item.NombreEstado || "Sin Estado";
        let funcion = item.NombreDeficiencia || "Sin Deficiencia";

        if (!salas[sala]) {
            salas[sala] = {};
        }

        if (!salas[sala][estado]) {
            salas[sala][estado] = {};
        }

        if (!salas[sala][estado][funcion]) {
            salas[sala][estado][funcion] = 0;
        }

        salas[sala][estado][funcion]++;
    });

    let categorias = Object.keys(salas);
    let seriesData = {}; 

    categorias.forEach(sala => {
        Object.keys(salas[sala]).forEach(estado => {
            Object.keys(salas[sala][estado]).forEach(funcion => {
                let serieKey = `${estado} - ${funcion}`; 

                if (!seriesData[serieKey]) {
                    seriesData[serieKey] = { name: serieKey, data: [], stack: estado };
                }

                seriesData[serieKey].data.push(salas[sala][estado][funcion]);
            });
        });
    });

    let series = Object.values(seriesData);

    // Renderizar el gráfico con Highcharts
    Highcharts.chart('chartCajasTemporizadas', {
        chart: {
            type: 'column',
            backgroundColor: '#292540' // Fondo oscuro
        },
        title: {
            text: 'Total incidentes en cajas temporizadas',
            align: 'center',
            style: { color: '#ffffff' }
        },
        xAxis: {
            categories: categorias,
            labels: { style: { color: '#ffffff' } }
        },
        yAxis: {
            allowDecimals: false,
            min: 0,
            title: {
                text: 'Cantidad',
                style: { color: '#ffffff' }
            },
            labels: { style: { color: '#ffffff' } },
            gridLineColor: 'rgba(255, 255, 255, 0.2)'
        },
        tooltip: {
            pointFormat: '<b>{series.name}</b>: {point.y} <br/> Total: {point.stackTotal}',
            backgroundColor: 'rgba(0, 0, 0, 0.8)',
            style: { color: '#ffffff' }
        },
        plotOptions: {
            column: {
                stacking: 'normal'
            }
        },
        legend: {
            itemStyle: { color: '#ffffff' }
        },
        series: series
    });
}

function getDataEntesReguladores() {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();

    $.ajax({
        type: "POST",
        url: basePath + "ESS_Dashboard/ListarDashboardEntesReguladoras",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            const datos = response.data || [];
            if (datos.length > 0) {
                generateChartEntesReguladores(datos);
                showInfo("chartEntesReguladores", "noDataEntesReguladores", true);
            } else {
                showInfo("chartEntesReguladores", "noDataEntesReguladores", false);
            }
        },
        error: function () {
            showInfo("chartEntesReguladores", "noDataEntesReguladores", false);
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}
function generateChartEntesReguladores(data) {
    let salas = {};

    data.forEach(item => {
        let sala = item.NombreSala;
        let estado = item.EnteReguladora || "Sin Ente Reguladora";
        let funcion = item.NombreMotivo || "Sin Motivo";

        if (!salas[sala]) {
            salas[sala] = {};
        }

        if (!salas[sala][estado]) {
            salas[sala][estado] = {};
        }

        if (!salas[sala][estado][funcion]) {
            salas[sala][estado][funcion] = 0;
        }

        salas[sala][estado][funcion]++;
    });

    let categorias = Object.keys(salas);
    let seriesData = {};

    categorias.forEach(sala => {
        Object.keys(salas[sala]).forEach(estado => {
            Object.keys(salas[sala][estado]).forEach(funcion => {
                let serieKey = `${estado} - ${funcion}`;

                if (!seriesData[serieKey]) {
                    seriesData[serieKey] = { name: serieKey, data: [], stack: estado };
                }

                seriesData[serieKey].data.push(salas[sala][estado][funcion]);
            });
        });
    });

    let series = Object.values(seriesData);

    // Renderizar el gráfico con Highcharts
    Highcharts.chart('chartEntesReguladores', {
        chart: {
            type: 'column',
            backgroundColor: '#292540' // Fondo oscuro
        },
        title: {
            text: 'Total visitas por entes reguladores',
            align: 'center',
            style: { color: '#ffffff' }
        },
        xAxis: {
            categories: categorias,
            labels: { style: { color: '#ffffff' } }
        },
        yAxis: {
            allowDecimals: false,
            min: 0,
            title: {
                text: 'Cantidad',
                style: { color: '#ffffff' }
            },
            labels: { style: { color: '#ffffff' } },
            gridLineColor: 'rgba(255, 255, 255, 0.2)'
        },
        tooltip: {
            pointFormat: '<b>{series.name}</b>: {point.y} <br/> Total: {point.stackTotal}',
            backgroundColor: 'rgba(0, 0, 0, 0.8)',
            style: { color: '#ffffff' }
        },
        plotOptions: {
            column: {
                stacking: 'normal'
            }
        },
        legend: {
            itemStyle: { color: '#ffffff' }
        },
        series: series
    });
}
//function generateChartEntesReguladores(data) {
//    let salas = {};

//    // Organizar los datos
//    data.forEach(item => {
//        let sala = item.NombreSala;
//        let estado = item.EnteReguladora || "Sin Ente Reguladora";
//        let funcion = item.NombreMotivo || "Sin Motivo";

//        if (!salas[sala]) {
//            salas[sala] = {};
//        }

//        if (!salas[sala][estado]) {
//            salas[sala][estado] = {};
//        }

//        if (!salas[sala][estado][funcion]) {
//            salas[sala][estado][funcion] = 0;
//        }

//        salas[sala][estado][funcion]++;
//    });

//    let categorias = Object.keys(salas);
//    let seriesData = {};

//    categorias.forEach(sala => {
//        Object.keys(salas[sala]).forEach(estado => {
//            Object.keys(salas[sala][estado]).forEach(funcion => {
//                let serieKey = `${estado} - ${funcion}`;

//                if (!seriesData[serieKey]) {
//                    seriesData[serieKey] = {
//                        name: serieKey,
//                        data: [],
//                        stack: estado
//                    };
//                }

//                seriesData[serieKey].data.push({
//                    y: salas[sala][estado][funcion],
//                    estado: estado // Guardamos el estado para mostrarlo en dataLabels
//                });
//            });
//        });
//    });

//    let series = Object.values(seriesData);

//    // Renderizar el gráfico con Highcharts
//    Highcharts.chart('chartEntesReguladores', {
//        chart: {
//            type: 'column',
//            backgroundColor: '#292540' // Fondo oscuro
//        },
//        title: {
//            text: 'Total visitas por entes reguladores',
//            align: 'center',
//            style: { color: '#ffffff' }
//        },
//        xAxis: {
//            categories: categorias,
//            labels: { style: { color: '#ffffff' } }
//        },
//        yAxis: {
//            allowDecimals: false,
//            min: 0,
//            title: {
//                text: 'Cantidad',
//                style: { color: '#ffffff' }
//            },
//            labels: { style: { color: '#ffffff' } },
//            gridLineColor: 'rgba(255, 255, 255, 0.2)'
//        },
//        tooltip: {
//            pointFormat: '<b>{series.name}</b>: {point.y} <br/> Total: {point.stackTotal}',
//            backgroundColor: 'rgba(0, 0, 0, 0.8)',
//            style: { color: '#ffffff' }
//        },
//        plotOptions: {
//            column: {
//                stacking: 'normal',
//                dataLabels: {
//                    enabled: true,
//                    useHTML: true,
//                    style: { color: '#ffffff', fontSize: '10px', textAlign: 'center' },
//                    formatter: function () {
//                        return `<span style="color: #bbb">${this.point.estado}</span>`; // Mostrar el estado debajo de la barra
//                    }
//                }
//            }
//        },
//        legend: {
//            itemStyle: { color: '#ffffff' }
//        },
//        series: series
//    });
//}


//function generateChartPieEntesReguladores(data) {
//    if (!data || data.length === 0) {
//        console.warn("No hay datos para generar el gráfico");
//        return;
//    }

//    let seriesData = {};

//    data.forEach(item => {
//        let ente = item.EnteReguladora || "Sin Ente Reguladora";
//        let motivo = item.NombreMotivo || "Sin Motivo";
//        let key = `${ente} - ${motivo}`;

//        if (!seriesData[key]) {
//            seriesData[key] = { name: key, y: 0 };
//        }

//        seriesData[key].y++;
//    });

//    let series = Object.values(seriesData);

//    Highcharts.chart('chartEntesReguladores', {
//        chart: {
//            type: 'pie',
//            backgroundColor: '#292540'
//        },
//        title: {
//            text: 'Total visitas por entes reguladores',
//            align: 'center',
//            style: { color: '#ffffff' }
//        },
//        tooltip: {
//            pointFormat: '<b>{point.name}</b>: {point.y} visitas ({point.percentage:.1f}%)',
//            backgroundColor: 'rgba(0, 0, 0, 0.8)',
//            style: { color: '#ffffff' }
//        },
//        plotOptions: {
//            pie: {
//                innerSize: '60%', // Para hacerla dona
//                allowPointSelect: true,
//                cursor: 'pointer',
//                dataLabels: {
//                    enabled: true,
//                    format: '<b>{point.name}</b>: {point.y}',
//                    style: { color: '#ffffff' }
//                },
//                showInLegend: true
//            }
//        },
//        legend: {
//            itemStyle: { color: '#ffffff' }
//        },
//        series: [{
//            name: 'Visitas',
//            data: series,
//            colors: [
//                '#FF6B6B', // Rojo coral suave
//                '#FFA07A', // Salmón pastel
//                '#FFD166', // Amarillo cálido pastel
//                '#06D6A0', // Verde menta pastel
//                '#118AB2', // Azul petróleo pastel
//                '#9A77CF', // Lila profundo
//                '#EF476F', // Rosa intenso
//                '#83C5BE'  // Turquesa suave
//            ]
//        }]
//    });
//}

function getDataOcurrenciasLog() {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();

    $.ajax({
        type: "POST",
        url: basePath + "ESS_Dashboard/ListarDashboardOcurrenciasLog",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            const datos = response.data || [];
            if (datos.length > 0) {
                generateChartOcurrenciasLog(datos);
                showInfo("chartOcurrenciasLog", "noDataOcurrenciasLog", true);
            } else {
                showInfo("chartOcurrenciasLog", "noDataOcurrenciasLog", false);
            }
        },
        error: function () {
            showInfo("chartOcurrenciasLog", "noDataOcurrenciasLog", false);
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}
function generateChartOcurrenciasLog(data) {
    let salas = {};

    data.forEach(item => {
        let sala = item.NombreSala;
        let estado = item.NombreEstado || "Sin Estado";
        let funcion = item.NombreTipologia || "Sin Tipologia";

        if (!salas[sala]) {
            salas[sala] = {};
        }

        if (!salas[sala][estado]) {
            salas[sala][estado] = {};
        }

        if (!salas[sala][estado][funcion]) {
            salas[sala][estado][funcion] = 0;
        }

        salas[sala][estado][funcion]++;
    });

    let categorias = Object.keys(salas);
    let seriesData = {};

    categorias.forEach(sala => {
        Object.keys(salas[sala]).forEach(estado => {
            Object.keys(salas[sala][estado]).forEach(funcion => {
                let serieKey = `${estado} - ${funcion}`;

                if (!seriesData[serieKey]) {
                    seriesData[serieKey] = { name: serieKey, data: [], stack: estado };
                }

                seriesData[serieKey].data.push(salas[sala][estado][funcion]);
            });
        });
    });

    let series = Object.values(seriesData);

    // Renderizar el gráfico con Highcharts
    Highcharts.chart('chartOcurrenciasLog', {
        chart: {
            type: 'column',
            backgroundColor: '#292540' // Fondo oscuro
        },
        title: {
            text: 'Total ocurrencias "LOG"',
            align: 'center',
            style: { color: '#ffffff' }
        },
        xAxis: {
            categories: categorias,
            labels: { style: { color: '#ffffff' } }
        },
        yAxis: {
            allowDecimals: false,
            min: 0,
            title: {
                text: 'Cantidad',
                style: { color: '#ffffff' }
            },
            labels: { style: { color: '#ffffff' } },
            gridLineColor: 'rgba(255, 255, 255, 0.2)'
        },
        tooltip: {
            pointFormat: '<b>{series.name}</b>: {point.y} <br/> Total: {point.stackTotal}',
            backgroundColor: 'rgba(0, 0, 0, 0.8)',
            style: { color: '#ffffff' }
        },
        plotOptions: {
            column: {
                stacking: 'normal'
            }
        },
        legend: {
            itemStyle: { color: '#ffffff' }
        },
        series: series
    });
}

function getDataLudopatas() {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();

    $.ajax({
        type: "POST",
        url: basePath + "ESS_Dashboard/ListarDashboardLudopatas",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            const datos = response.data || [];
            if (datos.length > 0) {
                generateChartLudopatas(datos);
                showInfo("chartLudopatas", "noDataLudopatas", true);
            } else {
                showInfo("chartLudopatas", "noDataLudopatas", false);
            }
        },
        error: function () {
            showInfo("chartLudopatas", "noDataLudopatas", false);
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
} function generateChartLudopatas(data) {
    if (!data || data.length === 0) {
        console.warn("No hay datos para generar el gráfico");
        return;
    }

    function convertDateStringToYYYYMMDD(dateString) {
        let match = dateString.match(/\d+/);
        if (!match) return "Fecha Inválida";

        let timestamp = parseInt(match[0]);
        let date = new Date(timestamp);
        return date.toISOString().split("T")[0];
    }

    let salas = {};

    data.forEach(item => {
        let sala = item.NombreSala || "Sin Sala";
        let fecha = item.FechaIngreso ? convertDateStringToYYYYMMDD(item.FechaIngreso) : "Sin Fecha";

        if (!salas[sala]) {
            salas[sala] = {};
        }

        if (!salas[sala][fecha]) {
            salas[sala][fecha] = 0;
        }

        salas[sala][fecha]++;
    });

    let fechas = [...new Set(
        data.map(item => item.FechaIngreso ? convertDateStringToYYYYMMDD(item.FechaIngreso) : "Sin Fecha")
    )].sort();

    let seriesData = Object.keys(salas).map(sala => {
        return {
            name: sala,
            data: fechas.map(fecha => salas[sala][fecha] || 0)
        };
    });

    if (seriesData.length === 0) {
        console.warn("No hay datos válidos para el gráfico");
        return;
    }

    const hasValidData = seriesData.some(serie => serie.data.some(value => value > 0));
    if (!hasValidData) {
        console.warn("Todos los valores son 0, Highcharts no mostrará el gráfico.");
        return;
    }

    Highcharts.chart('chartLudopatas', {
        chart: {
            type: 'line',
            backgroundColor: '#292540'
        },
        title: {
            text: 'Total ludopatas que intentaron ingresar',
            style: { color: '#ffffff' }
        },
        xAxis: {
            categories: fechas,
            labels: { style: { color: '#ffffff' } },
            title: { text: 'Fechas', style: { color: '#ffffff' } }
        },
        yAxis: {
            title: {
                text: 'Cantidad de Ludopatas',
                style: { color: '#ffffff' }
            },
            labels: { style: { color: '#ffffff' } },
            min: 0
        },
        tooltip: {
            shared: true,
            backgroundColor: 'rgba(0, 0, 0, 0.8)',
            style: { color: '#ffffff' }
        },
        plotOptions: {
            line: {
                marker: { enabled: true }
            }
        },
        legend: {
            itemStyle: { color: '#ffffff' }
        },
        series: seriesData
    });
}

function convertDateStringToYYYYMMDD(dateString) {
    // Extraer el número de timestamp dentro de "/Date(1731215520150)/"
    let match = dateString.match(/\d+/);
    if (!match) return "Fecha Inválida"; // Si no hay un número, devolver "Fecha Inválida"

    let timestamp = parseInt(match[0]); // Convertir el timestamp a número
    let date = new Date(timestamp); // Convertir a objeto Date
    return date.toISOString().split("T")[0]; // Formato YYYY-MM-DD
}






// Variable global para mantener el gráfico
var chartHfMaquina = null;

$(document).ready(function () {
    // 1. INICIALIZAR LIBRERÍAS
    $('#selectSala').select2({
        placeholder: "Seleccione una sala"
    }).val('SALA_01').trigger('change'); // Valor por defecto

    // Inicializar DatePicker
    $('#datepickerFecha').datetimepicker({
        format: 'DD/MM/YYYY',
        useCurrent: false,
        locale: 'es'
    });
    // Setear la fecha por defecto (ayer)
  //  $('#datepickerFecha').data("DateTimePicker").date(moment(new Date()).add(-1, 'days'));

    // 2. BOTÓN DE BÚSQUEDA
    $('#btnBuscar').on('click', function () {
        cargarDashboard();
    });

    // 3. EVENTO CLIC EN LA TABLA RANKING
    // (Para cargar la tabla de sustento)
    $('#tableRanking tbody').on('click', 'tr', function () {
        var codMaq = $(this).data('codmaq');
        if (codMaq) {
            // Resaltar fila
            $('#tableRanking tbody tr').removeClass('info');
            $(this).addClass('info');
            cargarLogDetallado(codMaq);
        }
    });

    // Carga inicial al entrar a la página
    //cargarDashboard();
});

// FUNCIÓN PRINCIPAL DE CARGA
function cargarDashboard() {
    var codSala =61// $('#selectSala').val();
    var fechaStr ="11-07-2023"// $('#datepickerFecha').find('input').val(); // Formato DD/MM/YYYY

    if (!codSala) {
        toastr.warning("Por favor, seleccione una sala.");
        return;
    }

    var fecha = moment(fechaStr, 'DD/MM/YYYY').format('YYYY-MM-DD');

    // Cargamos los 2 widgets principales
    cargarKpiGeneral(codSala, fecha);
    cargarRankingYGraficoMaquinas(codSala, fecha);

    // Limpiamos el log detallado
    $('#divLogDetallado').hide();
    $('#logInstruction').html('<i>(Selecciona una máquina del ranking para ver el sustento)</i>');
    $('#loadingLog').hide();
}

// ----------------------------------------------------
// FUNCIONES DE CARGA DE WIDGETS
// ----------------------------------------------------

function cargarKpiGeneral(codSala, fecha) {
    $.ajax({
        url: basePath + "AnalisisDataSala/GetHitFrecGeneral",
        type: "GET",
        data: { codSala: codSala, fecha: fecha },
        beforeSend: function () {
            $('#kpiHitFrequency').html('<i class="fa fa-spinner fa-spin"></i>');
            $('#kpiTotalJuegos').html('<i class="fa fa-spinner fa-spin"></i>');
            $('#kpiTotalHits').html('<i class="fa fa-spinner fa-spin"></i>');
        },
        success: function (response) {
            if (response.success && response.data) {
                var data = response.data;
                // Actualizar los KPIs de texto
                $('#kpiHitFrequency').html(data.HitFrequencyPorcentaje.toFixed(2) + ' %');
                $('#kpiTotalJuegos').html(data.TotalJuegos.toLocaleString()); // Formato 1,000
                $('#kpiTotalHits').html(data.TotalHits.toLocaleString());

            } else {
                toastr.error(response.message || "No se pudieron cargar los KPIs de HF.");
                $('#kpiHitFrequency').html('-- %');
                $('#kpiTotalJuegos').html('--');
                $('#kpiTotalHits').html('--');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            toastr.error("Error de AJAX (KPIs HF): " + textStatus);
        }
    });
}

function cargarRankingYGraficoMaquinas(codSala, fecha) {
    $.ajax({
        url: basePath + "AnalisisDataSala/GetHitFrecPorMaquina",
        type: "GET",
        data: { codSala: codSala, fecha: fecha },
        success: function (response) {
            if (response.success) {
                var $tbody = $('#tableRanking tbody');
                $tbody.empty();

                if (response.data.length === 0) {
                    $tbody.append('<tr><td colspan="4">No se encontraron máquinas.</td></tr>');
                    return;
                }

                // 1. Llenar la tabla de Ranking
                $.each(response.data, function (i, maquina) {
                    var hf = maquina.HitFrequencyPorcentaje.toFixed(2);

                    var $tr = $('<tr data-codmaq="' + maquina.CodMaq + '" style="cursor: pointer;" title="Clic para ver sustento"></tr>');
                    $tr.append('<td>' + maquina.CodMaq + '</td>');
                    $tr.append('<td>' + maquina.TotalJuegos.toLocaleString() + '</td>');
                    $tr.append('<td>' + maquina.TotalHits.toLocaleString() + '</td>');
                    $tr.append('<td>' + hf + ' %</td>');
                    $tbody.append($tr);
                });

                // 2. Preparar datos para el gráfico (Solo el Top 15)
                var top15Data = response.data.slice(0, 15);
                var labels = top15Data.map(function (m) { return m.CodMaq; });
                var dataHf = top15Data.map(function (m) { return m.HitFrequencyPorcentaje.toFixed(2); });

                var ctx = document.getElementById('chartHfPorMaquina').getContext('2d');
                if (chartHfMaquina) chartHfMaquina.destroy();

                chartHfMaquina = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: [
                            {
                                label: 'Hit Frequency (%)',
                                data: dataHf,
                                backgroundColor: 'rgba(54, 162, 235, 0.6)', // Azul
                                borderColor: 'rgba(54, 162, 235, 1)',
                                borderWidth: 1
                            }
                        ]
                    },
                    options: {
                        responsive: true,
                        scales: {
                            y: {
                                beginAtZero: true,
                                title: { display: true, text: 'Hit Frequency (%)' }
                            }
                        },
                        plugins: {
                            legend: { display: false }
                        }
                    }
                });

            } else {
                toastr.error(response.message || "No se pudo cargar el ranking de máquinas HF.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            toastr.error("Error de AJAX (Ranking HF): " + textStatus);
        }
    });
}

function cargarLogDetallado(codMaq) {
    var codSala = 61//$('#selectSala').val();
    var fechaStr = "11-07-2023"//$('#datepickerFecha').find('input').val();
    var fecha = moment(fechaStr, 'DD/MM/YYYY').format('YYYY-MM-DD');

    $('#logInstruction').html('Cargando sustento para la máquina <b>' + codMaq + '</b>...');

    $.ajax({
        url: basePath + "AnalisisDataSala/GetHitFrecLogDetallado",
        type: "GET",
        data: { codSala: codSala, fecha: fecha, codMaq: codMaq },
        beforeSend: function () {
            $('#loadingLog').show();
            $('#divLogDetallado').hide();
        },
        complete: function () {
            $('#loadingLog').hide();
        },
        success: function (response) {
            if (response.success) {
                var $tbody = $('#tableLogDetallado tbody');
                $tbody.empty();
                $('#divLogDetallado').show();

                if (response.data.length === 0) {
                    $tbody.append('<tr><td colspan="8" class="text-center">No se encontraron eventos para esta máquina.</td></tr>');
                    return;
                }

                // Esta es la réplica de tu Excel
                $.each(response.data, function (i, log) {
                    var $tr = $('<tr></tr>');
                    $tr.append('<td>' + moment(log.FechaHora).format('HH:mm:ss') + '</td>');
                    $tr.append('<td>' + log.GamesPlayed + '</td>');
                    $tr.append('<td>' + log.CoinOut + '</td>');
                    $tr.append('<td>' + log.HandPay + '</td>');
                    $tr.append('<td>' + log.Jackpot + '</td>');
                    $tr.append('<td>' + log.CancelCredits + '</td>');

                    // Resaltar los cambios
                    var tdJuego = (log.ConteoGamesPlayed > (response.data[i - 1] ? response.data[i - 1].ConteoGamesPlayed : 0))
                        ? '<strong class="text-success">' + log.ConteoGamesPlayed + '</strong>'
                        : '<span>' + log.ConteoGamesPlayed + '</span>';

                    var tdHit = (log.ConteoHitFrecuency > (response.data[i - 1] ? response.data[i - 1].ConteoHitFrecuency : 0))
                        ? '<strong class="text-success">' + log.ConteoHitFrecuency + '</strong>'
                        : '<span>' + log.ConteoHitFrecuency + '</span>';

                    $tr.append('<td>' + tdJuego + '</td>');
                    $tr.append('<td>' + tdHit + '</td>');

                    $tbody.append($tr);
                });

            } else {
                toastr.error(response.message || "No se pudo cargar el log detallado.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            toastr.error("Error de AJAX (Log HF): " + textStatus);
        }
    });
}
// Variable global para mantener los gráficos y poder destruirlos
var chartKpi = null;
var chartUsoHora = null;
var chartGantt = null;
var tableRankingInstance = null;
$(document).ready(function () {
    // 1. INICIALIZAR LIBRERÍAS
    // (Usando la sintaxis que vi en tus archivos JS)

    // Inicializar Select2 para las salas
    $('#selectSala').select2({
        placeholder: "Seleccione una sala"
        // (Aquí deberías tener un AJAX para llenar las salas,
        // similar a tu 'ListadoSalaMaestraPorUsuarioJson')
    });

    // Inicializar DatePicker
    $('#datepickerFecha').datetimepicker({
        format: 'DD/MM/YYYY',
        useCurrent: true,
        locale: 'es' // (Asegúrate de tener el locale 'es' cargado)
    }); // Fecha de ayer por defecto

    // 2. BOTÓN DE BÚSQUEDA
    $('#btnBuscar').on('click', function () {
        cargarDashboard();
    });

    // 3. EVENTO CLIC EN LA TABLA RANKING
    // (Para cargar el gráfico Gantt)
    $('#tableRanking tbody').on('click', 'tr', function () {
        var codMaq = $(this).data('codmaq'); // (Asumimos que el <tr> tiene un data-codmaq)
        if (codMaq) {
            cargarGraficoGantt(codMaq);
            $('#row-gantt').slideDown();
        }
    });

    // Carga inicial
    // (Asumiendo que quieres cargar el dashboard al inicio)
    // cargarDashboard();

    var resizeTimer;
    $(window).on('resize', function () {
        clearTimeout(resizeTimer);
        // Usamos un delay para no sobrecargar el navegador
        resizeTimer = setTimeout(function () {
            // LLAMAR A LAS DOS FUNCIONES
            S3k_balanceKpiRow();
            S3k_equalizeRow('#row-main-charts');
        }, 200);
    });
});


function S3k_equalizeRow(rowSelector) {
    var $rows = $(rowSelector);

    $rows.each(function () {
        var $cards = $(this).find('.col-md-8 > .block, .col-md-4 > .block'); // Busca los cards
        var maxHeight = 0;

        // 1. Resetea la altura para obtener la altura natural
        $cards.css('min-height', 'auto');

        // 2. Encuentra la altura máxima
        $cards.each(function () {
            if ($(this).outerHeight() > maxHeight) {
                maxHeight = $(this).outerHeight();
            }
        });

        // 3. Aplica la altura máxima a todos los cards de esa fila
        if (maxHeight > 0) {
            $cards.css('min-height', maxHeight + 'px');
        }
    });
}

/**
 * Función ESPECÍFICA: Balancea la fila de KPIs (1 grande, 2 pequeños)
 */
function S3k_balanceKpiRow() {
    var $pieCard = $('#cardPie');
    var $juegoCard = $('#cardJuego');
    var $inactivoCard = $('#cardInactivo');

    // 1. Resetear alturas para recalcular
    $pieCard.css('min-height', 'auto');
    $juegoCard.css('min-height', 'auto');
    $inactivoCard.css('min-height', 'auto');

    // 2. Obtener la altura de referencia (el card del Pie)
    var pieHeight = $pieCard.outerHeight();

    // 3. Obtener el espacio (margin-bottom) entre los dos cards de la derecha
    // .css() devuelve "XXpx", por eso usamos parseInt
    var gap = parseInt($juegoCard.css('margin-bottom'), 10) || 0;

    // 4. Calcular la altura deseada para cada card de KPI
    // (Altura total - espacio) / 2
    var kpiHeight = (pieHeight - gap) / 2;

    // 5. Aplicar las alturas
    if (kpiHeight > 0) {
        // Aseguramos que el card del Pie tenga su propia altura
        $pieCard.css('min-height', pieHeight + 'px');

        // Aplicamos la altura calculada a los otros dos
        $juegoCard.css('min-height', kpiHeight + 'px');
        $inactivoCard.css('min-height', kpiHeight + 'px');
    }
}

// FUNCIÓN PRINCIPAL DE CARGA
function cargarDashboard() {
    var codSala = 61//$('#selectSala').val();
    var fechaStr = "11-07-2023"// $('#datepickerFecha').find('input').val(); // Formato DD/MM/YYYY

    if (!codSala) {
        toastr.warning("Por favor, seleccione una sala.");
        return;
    }

    // Convertimos la fecha a YYYY-MM-DD para la API
    var fecha = moment(fechaStr, 'DD/MM/YYYY').format('YYYY-MM-DD');

    // Cargamos todos los widgets
    cargarKpiGeneral(codSala, fecha);
    cargarUsoPorHora(codSala, fecha);
    cargarRankingMaquinas(codSala, fecha);

    // Limpiamos el Gantt
    if (chartGantt) chartGantt.destroy();
    $('#loadingGantt').hide();
    setTimeout(function () {
        // LLAMAR A LAS DOS FUNCIONES
        S3k_balanceKpiRow();
        S3k_equalizeRow('#row-main-charts');
    }, 500); // 500ms
}

// ----------------------------------------------------
// FUNCIONES DE CARGA DE WIDGETS
// ----------------------------------------------------

function cargarKpiGeneral(codSala, fecha) {
    $.ajax({
        url: basePath + "AnalisisDataSala/GetKpiGeneral", // Llama al Controller MVC
        type: "GET",
        data: { codSala: codSala, fecha: fecha },
        beforeSend: function () {
            $('#kpiTotalJuego').html('<i class="fa fa-spinner fa-spin"></i>');
            $('#kpiTotalInactivo').html('<i class="fa fa-spinner fa-spin"></i>'); },
        success: function (response) {
            if (response.success && response.data) {
                var data = response.data;
                var total = data.TotalSegundosEnJuego + data.TotalSegundosInactivo;
                var porcJuego = (total > 0) ? (data.TotalSegundosEnJuego / total * 100).toFixed(1) : 0;
                var porcInactivo = (total > 0) ? (data.TotalSegundosInactivo / total * 100).toFixed(1) : 0;
                $('#kpiTotalJuego').html((data.TotalSegundosEnJuego / 3600).toFixed(2) + ' hrs');
                $('#kpiTotalInactivo').html((data.TotalSegundosInactivo / 3600).toFixed(2) + ' hrs');
                var ctx = document.getElementById('chartKpiGeneral').getContext('2d');
                if (chartKpi) chartKpi.destroy(); // Destruir el gráfico anterior

                chartKpi = new Chart(ctx, {
                    type: 'doughnut', // Gráfico de Dona
                    data: {
                        labels: [
                            'En Juego (' + porcJuego + '%)',
                            'Inactivo (' + porcInactivo + '%)'
                        ],
                        datasets: [{
                            data: [data.TotalSegundosEnJuego, data.TotalSegundosInactivo],
                            backgroundColor: ['#28a745', '#dc3545'] // Verde y Rojo
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: { position: 'bottom' },
                            title: { display: true, text: 'Ocupación Total (Segundos)' }
                        }
                    }
                });
            } else {
                toastr.error(response.message || "No se pudieron cargar los KPIs.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            toastr.error("Error de AJAX: " + textStatus);
        }
    });
}

function cargarUsoPorHora(codSala, fecha) {
    $.ajax({
        url: basePath + "AnalisisDataSala/GetUtilizacionPorHora",
        type: "GET",
        data: { codSala: codSala, fecha: fecha },
        success: function (response) {
            if (response.success) {
                var data = response.data; // (Viene de IdleTime_ResumenPorHora)
                var dataJuegoObj = [];
                var dataInactivoObj = [];
                var fechaOperativa = moment(fecha, 'YYYY-MM-DD');
                // Preparamos los datos para Chart.js
                $.each(data, function (i, item) {
                    // Asumimos que 'item.HoraDelDia' es un número (ej: 0, 1, ... 23)

                    // 3. Creamos el timestamp para esa hora en ese día
                    // Usamos .clone() para no modificar fechaOperativa
                    var timestamp = fechaOperativa.clone().hour(item.HoraDelDia).valueOf();

                    dataJuegoObj.push({
                        x: timestamp,
                        y: item.SegundosEnJuego / 3600 // Convertir a horas
                    });

                    dataInactivoObj.push({
                        x: timestamp,
                        y: item.SegundosInactivo / 3600 // Convertir a horas
                    });
                });
                var minTime = fechaOperativa.clone().add(7, 'hours').valueOf();
                var maxTime = fechaOperativa.clone().add(31, 'hours').valueOf();
                var ctx = document.getElementById('chartUsoPorHora').getContext('2d');
                if (chartUsoHora) chartUsoHora.destroy();

                chartUsoHora = new Chart(ctx, {
                    type: 'bar', // Gráfico de Barras
                    data: {
                        
                        datasets: [
                            {
                                label: 'Horas En Juego',
                                data: dataJuegoObj,
                                backgroundColor: '#28a745' // Verde
                            },
                            {
                                label: 'Horas Inactivo',
                                data: dataInactivoObj,
                                backgroundColor: '#dc3545' // Rojo
                            }
                        ]
                    },
                    options: {
                        responsive: true,
                        scales: {
                            x: { // Eje X ahora es de TIEMPO
                                type: 'time',
                                time: {
                                    unit: 'hour', // Mostrar por hora
                                    displayFormats: {
                                        hour: 'HH:mm' // Formato '07:00', '08:00'
                                    },
                                    tooltipFormat: 'DD/MM HH:mm' // Formato del hover
                                },
                                min: minTime, // Límite inferior 7am
                                max: maxTime, // Límite superior 7am (día sig)
                                stacked: true,
                                title: {
                                    display: true,
                                    text: 'Hora del Día'
                                }
                            },
                            y: { // Eje Y (sin cambios)
                                stacked: true,
                                title: { display: true, text: 'Horas Totales (Toda la Sala)' }
                            }
                        },
                        // 6. (Opcional pero recomendado) Mejorar el tooltip
                        plugins: {
                            tooltip: {
                                callbacks: {
                                    title: function (context) {
                                        // Muestra la hora en el título del tooltip
                                        var date = moment(context[0].parsed.x);
                                        return 'Hora: ' + date.format('HH:00');
                                    },
                                    label: function (context) {
                                        var label = context.dataset.label || '';
                                        var value = context.parsed.y.toFixed(2);
                                        return label + ': ' + value + ' hrs';
                                    }
                                }
                            }
                        }
                    }
                });
            }
        }
    });
}

function cargarRankingMaquinas(codSala, fecha) {
    $.ajax({
        url: basePath + "AnalisisDataSala/GetRankingMaquinas",
        type: "GET",
        data: { codSala: codSala, fecha: fecha },
        success: function (response) {
            if (tableRankingInstance) {
                tableRankingInstance.destroy();
            }
            var $tbody = $('#tableRanking tbody');
            $tbody.empty(); // Limpiar tabla
            if (response.success) {
                

                if (response.data.length === 0) {
                    $tbody.append('<tr><td colspan="3">No se encontraron máquinas.</td></tr>');
                    return;
                }

                $.each(response.data, function (i, maquina) {
                    var juego = (maquina.TotalSegundosEnJuego / 3600).toFixed(2); // Horas
                    var inactivo = (maquina.TotalSegundosInactivo / 3600).toFixed(2); // Horas

                    // Añadimos 'data-codmaq' para el evento click
                    var $tr = $('<tr data-codmaq="' + maquina.CodMaq + '" style="cursor: pointer;"></tr>');
                    $tr.append('<td>' + maquina.CodMaq + '</td>');
                    $tr.append('<td>' + juego + ' hrs</td>');
                    $tr.append('<td>' + inactivo + ' hrs</td>');
                    $tbody.append($tr);
                });

                tableRankingInstance = $('#tableRanking').DataTable({
                    "responsive": true,     // Hace la tabla responsiva
                    "paging": true,         // Habilita paginación
                    "lengthChange": false,  // Oculta el selector de "mostrar X entradas"
                    "pageLength": 10,       // Muestra 10 máquinas por página
                    "searching": true,      // Habilita el buscador
                    "ordering": true,       // Habilita ordenar columnas
                    "info": true,           // Muestra "Mostrando X de Y"
                    "autoWidth": false,
                    "order": [[1, "desc"]], // Ordena por T. Juego (columna índice 1) descendente
                });
            } else {
                toastr.error(response.message || "No se pudo cargar el ranking de máquinas.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            toastr.error("Error de AJAX (Ranking): " + textStatus);
        }
    });
}

// ... (El resto de tu JS: document.ready, cargarKpiGeneral, etc., están bien) ...

function cargarGraficoGantt(codMaq) {
    var codSala = 61//$('#selectSala').val();
    var fechaStr = "11/07/2023"//$('#datepickerFecha').find('input').val();
    var fecha = moment(fechaStr, 'DD/MM/YYYY').format('YYYY-MM-DD');

    $('#ganttInstruction').html('Cargando línea de tiempo para la máquina <b>' + codMaq + '</b>...');

    $.ajax({
        url: basePath + "AnalisisDataSala/GetTimelineMaquina",
        type: "GET",
        data: { codSala: codSala, fecha: fecha, codMaq: codMaq },
        beforeSend: function () {
            $('#loadingGantt').show();
            if (chartGantt) chartGantt.destroy();
        },
        complete: function () {
            $('#loadingGantt').hide();
        },
        success: function (response) {
            // El 'response.data' es el JSON que me pegaste
            if (response.success) {
                var dataJuego = [];
                var dataInactivo = [];
                var labelMaquina = 'Máquina ' + codMaq;
                // ----------------------------------------------------
                // --- ¡AQUÍ ESTÁ LA CORRECCIÓN! ---
                // ----------------------------------------------------

                // 1. Esta función "limpia" la fecha de Microsoft
                function parseMicrosoftDate(msDate) {
                    if (!msDate) return null;
                    // Extrae el número (ej. 1689094800000) de "/Date(1689094800000)/"
                    var timestamp = parseInt(msDate.replace(/[^0-9 +]/g, ''));
                    // Moment.js SÍ entiende el timestamp (milisegundos)
                    return moment(timestamp);
                }

                $.each(response.data, function (i, item) {

                    var fechaInicio = parseMicrosoftDate(item.FechaHoraInicio);
                    var fechaFin = parseMicrosoftDate(item.FechaHoraFin);

                    // 2. Creamos un OBJETO en lugar de un array [inicio, fin]
                    var bloqueObjeto = {
                        x: [fechaInicio.valueOf(), fechaFin.valueOf()], // El valor 'x' es el rango de tiempo
                        y: labelMaquina // El valor 'y' es la etiqueta a la que pertenece
                    };

                    if (item.EstadoEnJuego) {
                        dataJuego.push(bloqueObjeto); // <--- Añadimos el objeto
                    } else {
                        dataInactivo.push(bloqueObjeto); // <--- Añadimos el objeto
                    }
                });
                console.log(dataJuego)
                console.log(dataInactivo)
                // ----------------------------------------------------
                // --- FIN DE LA CORRECCIÓN ---
                // ----------------------------------------------------

                // Definimos el inicio y fin del día operativo (7am a 7am)
                var fechaOperativa = moment(fecha, 'YYYY-MM-DD');
                var minTime = fechaOperativa.clone().add(7, 'hours').valueOf();
                var maxTime = fechaOperativa.clone().add(31, 'hours').valueOf(); // 7am del día sig.


                console.log("Fecha Operativa:", fecha);
                console.log("MinTime (timestamp):", minTime, "->", moment(minTime).format('DD/MM/YYYY HH:mm'));
                console.log("MaxTime (timestamp):", maxTime, "->", moment(maxTime).format('DD/MM/YYYY HH:mm'));
                console.log("Datos JUEGO:", dataJuego);
                console.log("Datos INACTIVO:", dataInactivo);

                var ctx = document.getElementById('chartTimelineGantt').getContext('2d');
                if (chartGantt) chartGantt.destroy();
                chartGantt = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        datasets: [
                            {
                                label: 'En Juego',
                                data: dataJuego, // <--- 4. Usamos el nuevo array de OBJETOS
                                backgroundColor: '#28a745'
                            },
                            {
                                label: 'Inactivo',
                                data: dataInactivo, // <--- 4. Usamos el nuevo array de OBJETOS
                                backgroundColor: '#dc3545'
                            }
                        ]
                    },
                    options: {
                        indexAxis: 'y',
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            title: { display: true, text: 'Línea de Tiempo (Gantt) - ' + codMaq },

                            // 5. El tooltip AHORA debe leer 'context.raw.x' en lugar de 'context.raw'
                            tooltip: {
                                callbacks: {
                                    label: function (context) {
                                        var label = context.dataset.label || '';
                                        if (label) {
                                            label += ': ';
                                        }

                                        // 'context.raw.x' contiene el [inicio, fin]
                                        var start = context.raw.x[0];
                                        var end = context.raw.x[1];

                                        var format = 'HH:mm:ss';

                                        var duracion = moment.duration(moment(end).diff(moment(start)));
                                        var duracionStr = duracion.hours() + 'h ' + duracion.minutes() + 'm ' + duracion.seconds() + 's';

                                        return label +
                                            moment(start).format(format) +
                                            ' - ' +
                                            moment(end).format(format) +
                                            ' (' + duracionStr + ')';
                                    }
                                }
                            }
                        },
                        scales: {
                            x: {
                                type: 'time',
                                time: {
                                    parser: 'HH:mm',
                                    unit: 'hour',
                                    displayFormats: {
                                        hour: 'HH:mm'
                                    }
                                },
                                min: minTime,
                                max: maxTime,
                                title: {
                                    display: true,
                                    text: 'Hora del Día (7:00 AM a 7:00 AM)'
                                }
                            },
                            y: {
                                // stacked: true // <--- 6. BORRAMOS ESTO, ya no es necesario.
                            }
                        }
                    }
                });
            } else {
                toastr.error(response.message || "No se pudo cargar la línea de tiempo.");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            toastr.error("Error de AJAX: ");
        }
    });
}
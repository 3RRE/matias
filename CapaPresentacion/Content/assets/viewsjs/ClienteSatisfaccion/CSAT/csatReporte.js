// =============================
// CSAT Reporte
// =============================

// === Variables globales ===
var chartCsat = null;
$(document).ready(function () {
    $(document).on("change", "#filtroTendencia", function () {
        csatAplicarFiltro();
    });
})
// === Cargar CSAT (AJAX) ===
function csatCargar() {
    $.ajax({
        type: "POST",
        url: basePath + "/ConfigClienteSatisfaccion/DataNCSAT",
        data: JSON.stringify({
            fechaInicio: $("#startDate").val(),
            fechaFin: $("#endDate").val(),
            salaId: $("#cboSala").val()
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: () => $.LoadingOverlay && $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay && $.LoadingOverlay("hide"),
        success: function (res) {
            if (!res || !res.success) return;

            // Guardar en memoria global
            dataCsat = res.data;

            
            // Aplicar filtro inicial con tablets seleccionadas
            csatAplicarFiltro();
        },
        error: function (err) {
        }
    });
}

// === Aplicar filtro de tablets seleccionados ===
function csatAplicarFiltro() {
    if (!dataCsat) return;

    $("#csat_encuestados").text(dataCsat.respuestasUnicas)
    var filtrado = $.extend(true, {}, dataCsat);

    // 🔹 Filtrar por tablets
    if (selectedTablets.length > 0) {
        filtrado.csat.dataCsatActual = (dataCsat.csat.dataCsatActual || [])
            .filter(r => selectedTablets.includes(r.IdTablet));
        filtrado.csat.dataCsatAnterior = (dataCsat.csat.dataCsatAnterior || [])
            .filter(r => selectedTablets.includes(r.IdTablet));
    }

    // Procesar snapshot
    const csatActual = csatProcesarRespuestas(filtrado.csat.dataCsatActual || []);
    const csatAnterior = csatProcesarRespuestas(filtrado.csat.dataCsatAnterior || []);
    const comparacion = csatCalcularComparacion(csatActual, csatAnterior);

    csatRenderSnapshot({
        periodoActual: filtrado.periodoActual,
        periodoAnterior: filtrado.periodoAnterior,
        csatActual,
        csatAnterior,
        comparacion
    });

    // 🔹 Renderizar serie (según filtro seleccionado)
    const modo = $("#filtroTendencia").val() || "diario";
    const serie = csatAgruparRespuestas(
        filtrado.csat.dataCsatActual || [],
        modo
    );

    csatRenderTendenciaDiaria(serie, modo);
}




// === Recalcular CSAT diario/por mes desde data cruda ===
// === Agrupar y calcular CSAT por día o mes ===
function csatAgruparRespuestas(respuestas, modo = "diario") {
    if (!respuestas || respuestas.length === 0) return [];

    let grupos = {};

    respuestas.forEach(r => {
        const fecha = moment(r.FechaRespuesta).startOf("day");
        let key;

        if (modo === "diario") {
            key = fecha.format("YYYY-MM-DD");
        } else if (modo === "mensual") {
            key = fecha.format("YYYY-MM");
        } else {
            key = fecha.format("YYYY-MM-DD"); // default
        }

        if (!grupos[key]) grupos[key] = [];
        grupos[key].push(r.Valor);
    });

    let serie = Object.entries(grupos).map(([periodo, valores]) => {
        const total = valores.length;
        const cantMuyInsatisfecho = valores.filter(v => v === 1).length;
        const cantInsatisfecho = valores.filter(v => v === 2).length;
        const cantNeutral = valores.filter(v => v === 3).length;
        const cantSatisfecho = valores.filter(v => v === 4).length;
        const cantMuySatisfecho = valores.filter(v => v === 5).length;
        const csat = total > 0 ? ((cantSatisfecho + cantMuySatisfecho) * 100 / total) : 0;

        return {
            Periodo: periodo,
            TotalRespuestas: total,
            CantMuyInsatisfecho: cantMuyInsatisfecho,
            CantInsatisfecho: cantInsatisfecho,
            CantNeutral: cantNeutral,
            CantSatisfecho: cantSatisfecho,
            CantMuySatisfecho: cantMuySatisfecho,
            CSAT: csat
        };
    });

    // 🔹 ordenar por fecha
    serie.sort((a, b) => moment(a.Periodo).toDate() - moment(b.Periodo).toDate());

    return serie;
}


// === Procesar respuestas ===
function csatProcesarRespuestas(respuestas) {
    const counts = { 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 };
    respuestas.forEach(r => {
        counts[r.Valor] = (counts[r.Valor] || 0) + 1;
    });

    const total = Object.values(counts).reduce((a, b) => a + b, 0);
    const porcentajes = {};
    for (let i = 1; i <= 5; i++) {
        porcentajes[i] = total > 0 ? (counts[i] * 100 / total) : 0;
    }

    const csat = total > 0 ? ((counts[4] + counts[5]) * 100 / total) : 0;

    return {
        TotalRespuestas: total,
        CantMuyInsatisfecho: counts[1],
        CantInsatisfecho: counts[2],
        CantNeutral: counts[3],
        CantSatisfecho: counts[4],
        CantMuySatisfecho: counts[5],
        MuyInsatisfecho: porcentajes[1],
        Insatisfecho: porcentajes[2],
        Neutral: porcentajes[3],
        Satisfecho: porcentajes[4],
        MuySatisfecho: porcentajes[5],
        CSAT: csat
    };
}

// === Comparación entre periodos ===
function csatCalcularComparacion(actual, anterior) {
    return {
        csatDelta: (actual.CSAT - (anterior.CSAT || 0)),
        deltaMuyInsatisfecho: (actual.MuyInsatisfecho - (anterior.MuyInsatisfecho || 0)),
        deltaInsatisfecho: (actual.Insatisfecho - (anterior.Insatisfecho || 0)),
        deltaNeutral: (actual.Neutral - (anterior.Neutral || 0)),
        deltaSatisfecho: (actual.Satisfecho - (anterior.Satisfecho || 0)),
        deltaMuySatisfecho: (actual.MuySatisfecho - (anterior.MuySatisfecho || 0)),
    };
}

// === Render snapshot ===
function csatRenderSnapshot({ periodoActual, periodoAnterior, csatActual, csatAnterior, comparacion }) {
    $("#csat_range_current").text(formateaRango(periodoActual.fechaInicio, periodoActual.fechaFin));
    $("#csat_range_prev").text(formateaRango(periodoAnterior.fechaInicio, periodoAnterior.fechaFin));

    $("#csat_value").text(csatActual.CSAT.toFixed(1) + "%");
    $("#csat_respuestas").text(csatActual.TotalRespuestas);
    $("#csat_value_ant").text(csatAnterior.CSAT.toFixed(2));

    const diff = comparacion.csatDelta;
    const $icon = $("#csat_icon"), $diff = $("#csat_diff");
    $icon.removeClass().css("color", "");
    if (diff > 0) {
        $icon.addClass("glyphicon glyphicon-arrow-up").css("color", "green");
        $diff.text("+" + diff.toFixed(2) + " pts").css("color", "green");
    } else if (diff < 0) {
        $icon.addClass("glyphicon glyphicon-arrow-down").css("color", "red");
        $diff.text(diff.toFixed(2) + " pts").css("color", "red");
    } else {
        $icon.addClass("glyphicon glyphicon-minus").css("color", "gray");
        $diff.text("0").css("color", "gray");
    }

    csatPintarBarra("muyInsatisfecho", csatActual.CantMuyInsatisfecho, csatActual.MuyInsatisfecho, comparacion.deltaMuyInsatisfecho);
    csatPintarBarra("insatisfecho", csatActual.CantInsatisfecho, csatActual.Insatisfecho, comparacion.deltaInsatisfecho);
    csatPintarBarra("neutral", csatActual.CantNeutral, csatActual.Neutral, comparacion.deltaNeutral);
    csatPintarBarra("satisfecho", csatActual.CantSatisfecho, csatActual.Satisfecho, comparacion.deltaSatisfecho);
    csatPintarBarra("muySatisfecho", csatActual.CantMuySatisfecho, csatActual.MuySatisfecho, comparacion.deltaMuySatisfecho);
}

// === Pintar barra ===
function csatPintarBarra(tipo, count, pct, deltaPct) {
    pct = safeNum(pct); deltaPct = safeNum(deltaPct);
    $(`#cnt_${tipo}`).text(count ?? 0);
    $(`#bar_${tipo}`).css("width", pct + "%").text(pct.toFixed(2) + "%");

    const cls = deltaPct > 0 ? "text-success" : deltaPct < 0 ? "text-danger" : "text-muted";
    const sign = deltaPct > 0 ? "+" : "";
    $(`#trend_${tipo}`)
        .removeClass("text-success text-danger text-muted")
        .addClass(cls)
        .text(`${sign}${deltaPct.toFixed(2)}%`);
}

// === Render tendencia diaria ===
function csatRenderTendenciaDiaria(serie, modo) {
    const labels = serie.map(x =>
        modo === "mensual"
            ? moment(x.Periodo).format("MMM YYYY")
            : moment(x.Periodo).format("DD MMM")
    );

    const csatData = serie.map(x => x.CSAT || 0);

    const ctx = $("#csat_chartTendencia")[0];
    if (chartCsat) chartCsat.destroy();
    chartCsat = new Chart(ctx, {
        type: 'line',
        data: {
            labels,
            datasets: [{
                label: `CSAT (${modo})`,
                data: csatData,
                borderWidth: 2,
                fill: false,
                borderColor: "rgba(54, 162, 235, 1)",
                backgroundColor: "rgba(54, 162, 235, 0.2)",
                tension: 0.3
            }]
        },
        options: {
            responsive: true,
            scales: { y: { beginAtZero: true, suggestedMax: 100 } }
        }
    });
}

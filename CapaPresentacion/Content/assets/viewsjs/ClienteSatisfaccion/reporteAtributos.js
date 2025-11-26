// ==============================
// === Variables globales =======
// ==============================
let chartIndicadorLine;

// ===============================
// === AJAX para cargar datos ====
// ===============================
function ejecutarFuncion(indicadorKey, start, end, sala) {
    $.ajax({
        type: "POST",
        url: basePath + "/ConfigClienteSatisfaccion/DataIndicador",
        data: JSON.stringify({
            fechaInicio: start,
            fechaFin: end,
            salaId: sala,
            indicador: indicadorKey.toUpperCase()
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: () => $.LoadingOverlay && $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay && $.LoadingOverlay("hide"),
        success: function (res) {
            if (!res || !res.success) return;

            dataAtributo = res.data;
            $(".titulo").text(res.data.nombrePregunta);
            atributoAplicarFiltro();
        },
        error: function (err) {
            console.error("Error en ejecutarFuncion", err);
        }
    });
}

// ==============================
// === Procesar respuestas ======
// ==============================
function atributoProcesarRespuestas(respuestas) {
    const counts = { 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 };
    respuestas.forEach(r => {
        counts[r.Valor] = (counts[r.Valor] || 0) + 1;
    });

    const total = Object.values(counts).reduce((a, b) => a + b, 0);
    const porcentajes = {};
    for (let i = 1; i <= 5; i++) {
        porcentajes[i] = total > 0 ? (counts[i] * 100 / total) : 0;
    }

    // 🔹 Fórmula de CSAT aplicada al Indicador de atributos
    const indicador = total > 0 ? ((counts[4] + counts[5]) * 100 / total) : 0;

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
        Indicador: indicador
    };
}

// ==============================
// === Comparación ==============
// ==============================
function atributoCalcularComparacion(actual, anterior) {
    return {
        indicadorDelta: (actual.Indicador - (anterior.Indicador || 0)),
        deltaMuyInsatisfecho: (actual.MuyInsatisfecho - (anterior.MuyInsatisfecho || 0)),
        deltaInsatisfecho: (actual.Insatisfecho - (anterior.Insatisfecho || 0)),
        deltaNeutral: (actual.Neutral - (anterior.Neutral || 0)),
        deltaSatisfecho: (actual.Satisfecho - (anterior.Satisfecho || 0)),
        deltaMuySatisfecho: (actual.MuySatisfecho - (anterior.MuySatisfecho || 0))
    };
}

// ==============================
// === Agrupar Respuestas =======
// ==============================
function atributoAgruparRespuestas(respuestas, modo = "diario") {
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
            key = fecha.format("YYYY-MM-DD");
        }

        if (!grupos[key]) grupos[key] = [];
        grupos[key].push(r);
    });

    let serie = Object.entries(grupos).map(([periodo, lista]) => {
        const procesado = atributoProcesarRespuestas(lista);
        return {
            Periodo: periodo,
            ...procesado
        };
    });

    serie.sort((a, b) => moment(a.Periodo).toDate() - moment(b.Periodo).toDate());
    return serie;
}

// ==============================
// === Aplicar filtro ===========
// ==============================
function atributoAplicarFiltro() {
    if (!dataAtributo) return;
    let filtrado = $.extend(true, {}, dataAtributo);

    // 🔹 Filtrar por tablets si aplica
    if (selectedTablets.length > 0) {
        filtrado.indicador.indicadorActual = (dataAtributo.indicador.indicadorActual || [])
            .filter(r => selectedTablets.includes(r.IdTablet));

        filtrado.indicador.indicadorAnterior = (dataAtributo.indicador.indicadorAnterior || [])
            .filter(r => selectedTablets.includes(r.IdTablet));
    }

    // Procesar snapshot
    const actual = atributoProcesarRespuestas(filtrado.indicador.indicadorActual || []);
    const anterior = atributoProcesarRespuestas(filtrado.indicador.indicadorAnterior || []);
    const comparacion = atributoCalcularComparacion(actual, anterior);

    atributoRenderSnapshot({
        periodoActual: filtrado.periodoActual,
        periodoAnterior: filtrado.periodoAnterior,
        actual,
        anterior,
        comparacion
    });

    // 🔹 Renderizar serie diaria o mensual
    const modo = $("#filtroTendencia").val() || "diario";
    const serie = atributoAgruparRespuestas(
        filtrado.indicador.indicadorActual || [],
        modo
    );

    atributoRenderTendencia(serie, modo);
}

// ==============================
// === Render Snapshot ==========
// ==============================
function atributoRenderSnapshot({ periodoActual, periodoAnterior, actual, anterior, comparacion }) {
    $("#atributo_range_current").text(formateaRango(periodoActual.fechaInicio, periodoActual.fechaFin));
    $("#atributo_range_prev").text(formateaRango(periodoAnterior.fechaInicio, periodoAnterior.fechaFin));

    $("#atributo_value").text(actual.Indicador.toFixed(1) + "%");
    $("#atributo_respuestas").text(actual.TotalRespuestas);
    $("#atributo_value_ant").text(anterior.Indicador.toFixed(1) + "%");

    const diff = comparacion.indicadorDelta;
    const $icon = $("#atributo_icon"), $diff = $("#atributo_diff");
    $icon.removeClass().css("color", "");
    if (diff > 0) {
        $icon.addClass("glyphicon glyphicon-arrow-up").css("color", "green");
        $diff.text("+" + diff.toFixed(1) + " pts").css("color", "green");
    } else if (diff < 0) {
        $icon.addClass("glyphicon glyphicon-arrow-down").css("color", "red");
        $diff.text(diff.toFixed(1) + " pts").css("color", "red");
    } else {
        $icon.addClass("glyphicon glyphicon-minus").css("color", "gray");
        $diff.text("0").css("color", "gray");
    }

    atributoPintarBarra("muyInsatisfecho", actual.CantMuyInsatisfecho, actual.MuyInsatisfecho, comparacion.deltaMuyInsatisfecho);
    atributoPintarBarra("insatisfecho", actual.CantInsatisfecho, actual.Insatisfecho, comparacion.deltaInsatisfecho);
    atributoPintarBarra("neutral", actual.CantNeutral, actual.Neutral, comparacion.deltaNeutral);
    atributoPintarBarra("satisfecho", actual.CantSatisfecho, actual.Satisfecho, comparacion.deltaSatisfecho);
    atributoPintarBarra("muySatisfecho", actual.CantMuySatisfecho, actual.MuySatisfecho, comparacion.deltaMuySatisfecho);
}

// ==============================
// === Pintar Barra =============
// ==============================
function atributoPintarBarra(tipo, count, pct, deltaPct) {
    pct = safeNum(pct); deltaPct = safeNum(deltaPct);

    $(`#cnt_atr_${tipo}`).text(count ?? 0);
    $(`#bar_atr_${tipo}`).css("width", pct + "%").text(pct.toFixed(2) + "%");

    const cls = deltaPct > 0 ? "text-success" : deltaPct < 0 ? "text-danger" : "text-muted";
    const sign = deltaPct > 0 ? "+" : "";
    $(`#trend_atr_${tipo}`)
        .removeClass("text-success text-danger text-muted")
        .addClass(cls)
        .text(`${sign}${deltaPct.toFixed(2)}%`);
}

// ==============================
// === Render tendencia =========
// ==============================
function atributoRenderTendencia(serie, modo) {
    const labels = serie.map(x =>
        modo === "mensual"
            ? moment(x.Periodo).format("MMM YYYY")
            : moment(x.Periodo).format("DD MMM")
    );

    const valores = serie.map(x => x.Indicador || 0);

    const ctx = $("#atributo_chartTendencia")[0];
    if (chartIndicadorLine) chartIndicadorLine.destroy();

    chartIndicadorLine = new Chart(ctx, {
        type: 'line',
        data: {
            labels,
            datasets: [{
                label: `Indicador (${modo})`,
                data: valores,
                borderWidth: 2,
                fill: false,
                borderColor: "rgba(255, 99, 132, 1)",
                backgroundColor: "rgba(255, 99, 132, 0.2)",
                tension: 0.3
            }]
        },
        options: {
            responsive: true,
            scales: { y: { beginAtZero: true, suggestedMax: 100 } }
        }
    });
}

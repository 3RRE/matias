// =============================
// NPS Reporte
// =============================

// === Variables globales ===
var npsChart = null; // gráfico de tendencia mensual
$(document).ready(function () {
    $(document).on("change", "#filtroTendenciaNps", function () {
        npsAplicarFiltro();
    });

})
// === Función para calcular NPS a partir de respuestas crudas ===
function npsCalcular(respuestas) {
    var total = respuestas.length;

    var detractores = respuestas.filter(r => r.Valor === 1 || r.Valor === 2).length;
    var pasivos = respuestas.filter(r => r.Valor === 3 ).length;
    var promotores = respuestas.filter(r => r.Valor === 5 || r.Valor === 4).length;

    var pctDetractores = total > 0 ? (detractores * 100) / total : 0;
    var pctPasivos = total > 0 ? (pasivos * 100) / total : 0;
    var pctPromotores = total > 0 ? (promotores * 100) / total : 0;

    var nps = Math.round(pctPromotores - pctDetractores);

    return {
        total,
        detractores, pasivos, promotores,
        pctDetractores, pctPasivos, pctPromotores,
        nps
    };
}

// === AJAX para cargar data NPS ===
function npsCargar() {
    // reset UI mínima antes de cargar
    npsResetBar("detractores");
    npsResetBar("pasivos");
    npsResetBar("promotores");
    $("#nps_value").text("—");

    $.ajax({
        type: "POST",
        url: basePath + "/ConfigClienteSatisfaccion/DataNPS",
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

            // guardar en memoria global
            dataNps = res.data;

            // aplicar filtro inicial (si hay tablets seleccionadas)
            npsAplicarFiltro();
        },
        error: function (req, status, err) {
            console.error("Error DataNPS:", err);
        }
    });
}

// === Aplicar filtro por tablets seleccionados ===
function npsAplicarFiltro() {
    if (!dataNps) return;
    $("#nps_encuestados").text(dataNps.respuestasUnicas)

    var filtrado = $.extend(true, {}, dataNps);

    if (selectedTablets.length > 0) {
        filtrado.nps.npsActual = (dataNps.nps.npsActual || []).filter(r => selectedTablets.includes(r.IdTablet));
        filtrado.nps.npsAnterior = (dataNps.nps.npsAnterior || []).filter(r => selectedTablets.includes(r.IdTablet));
    }

    npsRenderSnapshot(filtrado);

    // 🔹 generar serie a partir de respuestas actuales
    const modo = $("#filtroTendenciaNps").val() || "diario";
    const serie = npsAgruparRespuestas(filtrado.nps.npsActual || [], modo);

    npsRenderTendencia(serie, modo);
}


// === Render snapshot ===
function npsRenderSnapshot(data) {
    var pa = data.periodoActual || {};
    var pp = data.periodoAnterior || {};

    var actual = npsCalcular(data.nps.npsActual || []);
    var anterior = npsCalcular(data.nps.npsAnterior || []);

    $("#nps_range_current").text("Período: " + formateaRango(pa.fechaInicio, pa.fechaFin));
    $("#nps_range_prev").text(formateaRango(pp.fechaInicio, pp.fechaFin));
    $("#nps_total_respuestas").text(actual.total);

    // NPS
    $("#nps_value").text(actual.nps);
    $("#nps_value_ant").text(anterior.nps);
    var diferencia = actual.nps - anterior.nps;

    var $icon = $("#nps_icon"), $diff = $("#nps_diff");
    $icon.removeClass().css("color", "");
    if (diferencia > 0) {
        $icon.addClass("glyphicon glyphicon-arrow-up").css("color", "green");
        $diff.text("+" + diferencia).css("color", "green");
    } else if (diferencia < 0) {
        $icon.addClass("glyphicon glyphicon-arrow-down").css("color", "red");
        $diff.text(diferencia).css("color", "red");
    } else {
        $icon.addClass("glyphicon glyphicon-minus").css("color", "gray");
        $diff.text("0").css("color", "gray");
    }

    // Barras
    npsPintarBarra("detractores", actual.detractores, actual.pctDetractores, anterior.pctDetractores);
    npsPintarBarra("pasivos", actual.pasivos, actual.pctPasivos, anterior.pctPasivos);
    npsPintarBarra("promotores", actual.promotores, actual.pctPromotores, anterior.pctPromotores);
}

// === Reset barra ===
function npsResetBar(tipo) {
    $("#cnt_" + tipo).text("0");
    $("#bar_" + tipo).css("width", "0%").text("0.00%");
    $("#trend_" + tipo)
        .removeClass("text-success text-danger text-muted")
        .addClass("text-muted")
        .text("= 0.00%");
}

// === Pintar barra ===
function npsPintarBarra(tipo, count, pct, pctAnt) {
    var p = safeNum(pct);
    var pa = safeNum(pctAnt);
    $("#cnt_" + tipo).text(count ?? 0);
    $("#bar_" + tipo).css("width", p + "%").text(p.toFixed(2) + "%");

    var deltaPct = p - pa;
    var cls = deltaPct > 0 ? "text-success" : deltaPct < 0 ? "text-danger" : "text-muted";
    var sign = deltaPct > 0 ? "+" : "";
    $("#trend_" + tipo)
        .removeClass("text-success text-danger text-muted")
        .addClass(cls)
        .text(sign + deltaPct.toFixed(2) + "%");
}

// === Tendencia mensual ===
function npsRenderTendencia(serie, modo) {
    var labels = serie.map(x =>
        modo === "mensual"
            ? moment(x.Periodo, "YYYY-MM").format("MMM YYYY")
            : moment(x.Periodo, "YYYY-MM-DD").format("DD MMM")
    );

    var npsData = serie.map(x => x.NPS || 0);

    var ctx = document.getElementById("nps_chartTendencia");
    if (!ctx) return;

    if (npsChart) npsChart.destroy();
    npsChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels,
            datasets: [{
                label: `NPS (${modo})`,
                data: npsData,
                borderWidth: 2,
                fill: false,
                borderColor: "rgba(255, 99, 132, 1)",
                backgroundColor: "rgba(255, 99, 132, 0.2)",
                tension: 0.3
            }]
        },
        options: {
            responsive: true,
            plugins: { legend: { display: true }, tooltip: { enabled: true } },
            scales: { y: { beginAtZero: true, suggestedMin: 0, suggestedMax: 100 } }
        }
    });
}


function npsAgruparRespuestas(respuestas, modo = "diario") {
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
        grupos[key].push(r.Valor);
    });

    // 🔹 calcular NPS por cada periodo
    let serie = Object.entries(grupos).map(([periodo, valores]) => {
        const total = valores.length;
        const detractores = valores.filter(v => v === 1 || v === 2).length;
        const pasivos = valores.filter(v => v === 3).length;
        const promotores = valores.filter(v => v === 4 || v === 5).length;

        const pctDetractores = total > 0 ? (detractores * 100) / total : 0;
        const pctPromotores = total > 0 ? (promotores * 100) / total : 0;
        const nps = Math.round(pctPromotores - pctDetractores);

        return {
            Periodo: periodo,
            Total: total,
            Detractores: detractores,
            Pasivos: pasivos,
            Promotores: promotores,
            NPS: nps
        };
    });

    // 🔹 ordenar la serie por fecha
    serie.sort((a, b) => moment(a.Periodo).toDate() - moment(b.Periodo).toDate());

    return serie;
}

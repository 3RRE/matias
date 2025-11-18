// =============================
// 📌 OBTENER COMENTARIOS NPS
// =============================
function ObtenerComentariosNps(start, end, salaId) {
    if (!start || !end || !salaId) {
        console.warn("⚠️ Falta información para obtener comentarios NPS");
        return;
    }

    $.ajax({
        type: "POST",
        url: basePath + "/ConfigClienteSatisfaccion/ListarEncuestadosConComentarios",
        data: JSON.stringify({
            fechaInicio: start,
            fechaFin: end,
            salaId: salaId,
            tabletIds: selectedTablets // 👈 se envían los tablets seleccionados
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: () => $.LoadingOverlay && $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay && $.LoadingOverlay("hide"),
        success: function (res) {
            if (!res || !res.success) {
                toastr.warning(res.displayMessage || "No se encontraron comentarios NPS");
                return;
            }

            // ✅ guardar en memoria global
            dataComentarioNps = res.data || [];

            // ✅ render inicial
            aplicarFiltroNps();
        },
        error: function (err) {
            console.error("❌ Error al obtener comentarios NPS", err);
            toastr.error("Error en la consulta de comentarios NPS");
        }
    });
}

// =============================
// 📌 RENDER PRINCIPAL DEL DASHBOARD
// =============================
function renderDashboard(data) {
    if (!Array.isArray(data)) {
        console.warn("⚠️ Data inválida en renderDashboard");
        return;
    }

    // Totales
    let total = data.length;
    let detractores = data.filter(e => e.Clasificacion === "Detractor").length;
    let neutrales = data.filter(e => e.Clasificacion === "Neutral").length;
    let promotores = data.filter(e => e.Clasificacion === "Promotor").length;

    $("#totalRespuestas").text(total);
    $("#totalDetractores").text(detractores);
    $("#totalNeutrales").text(neutrales);
    $("#totalPromotores").text(promotores);

    // Porcentajes
    let pctDet = total ? Math.round((detractores / total) * 100) : 0;
    let pctNeu = total ? Math.round((neutrales / total) * 100) : 0;
    let pctPro = total ? Math.round((promotores / total) * 100) : 0;

    $("#barDetractores").css("width", pctDet + "%").text(pctDet + "%");
    $("#barNeutrales").css("width", pctNeu + "%").text(pctNeu + "%");
    $("#barPromotores").css("width", pctPro + "%").text(pctPro + "%");

    // Lista de encuestados
    const $lista = $("#listaEncuestados");
    $lista.empty();

    if (total === 0) {
        $lista.append("<div class='alert alert-info'>No hay encuestas registradas en este rango.</div>");
        return;
    }

    data.forEach(e => {
        let color = e.Clasificacion === "Promotor" ? "success" :
            e.Clasificacion === "Detractor" ? "danger" : "warning";

        let comentariosHtml = "";
        if (e.Comentarios && e.Comentarios.length > 0) {
            // 👇 Agrupar por Pregunta
            const agrupados = {};
            e.Comentarios.forEach(c => {
                if (!agrupados[c.Pregunta]) {
                    agrupados[c.Pregunta] = [];
                }
                agrupados[c.Pregunta].push(c);
            });

            comentariosHtml = `
        <ul class="list-group">
            ${Object.keys(agrupados).map(preg => `
                <li class="list-group-item">
                    <strong>${preg}</strong><br>
                    <ul>
                        ${agrupados[preg].map(c => `
                            <li>
                                ${c.OpcionTexto || ""}
                                ${c.Comentario ? ` → <em>${c.Comentario}</em>` : ""}
                            </li>
                        `).join("")}
                    </ul>
                </li>
            `).join("")}
        </ul>
    `;
        }


        $lista.append(`
            <div class="panel panel-${color}">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <span class="label label-${color}">${e.ValorRespuesta}</span>
                        ${e.Clasificacion} - ${e.Nombre}
                        <small class="pull-right">
                            ${new Date(e.FechaRespuesta).toLocaleString()}
                        </small>
                    </h4>
                </div>
                <div class="panel-body">
                    ${comentariosHtml || "<p class='text-muted'>Sin comentarios adicionales</p>"}
                    <hr>
                    <p><strong>DNI:</strong> ${e.NroDocumento}</p>
                    <p><strong>Correo:</strong> ${e.Correo}</p>
                    <p><strong>Celular:</strong> ${e.Celular}</p>
                </div>
            </div>
        `);
    });
}

// =============================
// 📌 APLICAR FILTRO NPS (por tablets seleccionadas)
// =============================
function aplicarFiltroNps() {
    if (!Array.isArray(dataComentarioNps)) {
        console.warn("⚠️ No hay data en dataComentarioNps");
        return;
    }

    let filtrados = [];

    // Si hay tablets seleccionadas -> filtrar
    if (Array.isArray(selectedTablets) && selectedTablets.length > 0) {
        filtrados = dataComentarioNps.filter(e => selectedTablets.includes(e.IdTablet));
    } else {
        // Si no hay ninguna tablet seleccionada, mostrar todo
        filtrados = dataComentarioNps;
    }

    // Renderizar la vista con el dataset filtrado
    renderDashboard(filtrados);
}

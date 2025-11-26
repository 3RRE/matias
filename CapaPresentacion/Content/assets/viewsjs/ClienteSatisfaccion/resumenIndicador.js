const caritas = {
    1: "Content/assets/images/nps/muyinsatisfecho.gif",
    2: "Content/assets/images/nps/insatisfecho.gif",
    3: "Content/assets/images/nps/regular.gif",
    4: "Content/assets/images/nps/satisfecho.gif",
    5: "Content/assets/images/nps/muysatisfecho.gif"
}
function resumenIndicadores(start, end, sala) {
    $.ajax({
        type: "POST",
        url: basePath + "/ConfigClienteSatisfaccion/ResumenIndicadores",
        data: JSON.stringify({
            fechaInicio: start,
            fechaFin: end,
            salaId: sala,
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: () => $.LoadingOverlay?.("show"),
        complete: () => $.LoadingOverlay?.("hide"),
        success: function (res) {
            if (!res?.success || !res.data) return;

            //  Llama a la función para renderizar el resumen
            resumenAplicarFiltro(res.data, selectedTablets);
        },
        error: function (err) {
            console.error("Error en resumenIndicadores:", err);
        }
    });
}



function resumenAplicarFiltro(dataIndicadores, selectedTablets = []) {
    if (!Array.isArray(dataIndicadores)) return;

    const $tableBody = $("#tablaResumen tbody");
    $tableBody.empty();

    dataIndicadores.forEach(item => {
        const { indicador, pregunta, respuestas } = item;
        const todasRespuestas = Array.isArray(respuestas) ? respuestas : [];

        const filtradas = selectedTablets.length > 0
            ? todasRespuestas.filter(r => selectedTablets.includes(r.IdTablet))
            : todasRespuestas;

        const total = filtradas.length;

        //  Calcular totales básicos
        let conteo = { 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 };
        let porcentajes = { 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 };
        let indicadorFinal = 0;

        if (total > 0) {
            // Calcular conteo y porcentajes
            conteo = [1, 2, 3, 4, 5].reduce((acc, val) => {
                acc[val] = filtradas.filter(r => r.Valor === val).length;
                return acc;
            }, {});

            for (let val = 1; val <= 5; val++) {
                porcentajes[val] = ((conteo[val] / total) * 100).toFixed(1);
            }


            //  Calcular indicador usando tu función
            const resultado = indicadorPorcentaje(filtradas);
            indicadorFinal = resultado.Indicador.toFixed(1);
        }

        //  Generar fila HTML
        const fila = `
            <tr>
                <td>${pregunta || indicador || "(Sin nombre)"}</td>
                <td> ${total} </td>

               ${[1, 2, 3, 4, 5].map(val => `
                <td class="text-center">
                    <div style="display:flex; flex-direction:column; justify-content:center; align-items:center; gap:4px;">
                        <img style="width:50px;" src="${basePath}/${caritas[val]}" />
                        ${porcentajes[val]}%
                    </div>
                </td>
            `).join("")}


                <td class="text-center font-bold">${indicadorFinal}</td>
            </tr>`;

        $tableBody.append(fila);
    });
}





function indicadorPorcentaje(respuestas) {
    const counts = { 1: 0, 2: 0, 3: 0, 4: 0, 5: 0 };
    respuestas.forEach(r => {
        counts[r.Valor] = (counts[r.Valor] || 0) + 1;
    });

    const total = Object.values(counts).reduce((a, b) => a + b, 0);
    const porcentajes = {};
    for (let i = 1; i <= 5; i++) {
        porcentajes[i] = total > 0 ? (counts[i] * 100 / total) : 0;
    }

    // Fórmula de CSAT aplicada al Indicador de atributos
    const indicador = total > 0 ? ((counts[4] + counts[5]) * 100 / total) : 0;

    return {
        Indicador: indicador
    };
}
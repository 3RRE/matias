let preguntas = [];
var dataPreguntas = []; // aquí va la data del backend

$(document).ready(function () {

    // =============================
    //  Cargar preguntas desde backend
    // =============================
    function cargarPreguntas() {
        $.ajax({
            type: "POST",
            url: basePath + "/ConfigClienteSatisfaccion/ObtenerPreguntasConOpcionesYFlujo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (res) {
                if (res.success) {
                    preguntas = res.data || [];
                    renderPreguntas(preguntas);
                } else {
                    toastr.error("Error: " + res.displayMessage);
                }
            },
            error: function (err) {
                console.error("Error al cargar preguntas:", err);
            }
        });
    }

    // =============================
    //  Render preguntas agrupadas por Orden
    // =============================

    function renderPreguntas(lista) {
        const $container = $("#preguntas-container");
        $container.empty();

        lista.sort((a, b) => a.Orden - b.Orden).forEach(p => {
            const collapseId = `collapse_${p.IdPregunta}`;

            const opcionesHtml = (p.Opciones || []).map(o => {
                let flujoLabel = "";
                if (o.Flujo && o.Flujo.IdPreguntaSiguiente) {
                    flujoLabel = `<span class="label label-success">→ ${o.Flujo.IdPreguntaSiguiente}</span>`;
                }
                return `
                <li class="list-group-item">
                    ${o.Texto} ${flujoLabel}
                    <div class="pull-right">
                        <button class="btn btn-xs btn-primary btnDefinirFlujo" 
                            data-idpregunta="${p.IdPregunta}" 
                            data-idopcion="${o.IdOpcion}">
                            Definir flujo
                        </button>
                        <button class="btn btn-xs btn-danger btnQuitarFlujo" 
                            data-idpregunta="${p.IdPregunta}" 
                            data-idopcion="${o.IdOpcion}">
                            Quitar
                        </button>
                    </div>
                </li>
            `;
            }).join("");

            const $card = $(`
            <div class="panel panel-default pregunta-item" style="padding:unset" data-id="${p.IdPregunta}">
                <div class="panel-heading">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" href="#${collapseId}">
                            <input type="number" class="orden-input" value="${p.Orden}" 
                                style="width:60px; text-align:center; margin-right:10px;" /> 
                            ${p.Texto} 
                            <span class="label label-info">ID: ${p.IdPregunta}</span>
                        </a>
                    </h4>
                </div>
                <div id="${collapseId}" class="panel-collapse collapse">
                    <div class="panel-body p-0">
                        <ul class="list-group">${opcionesHtml}</ul>
                    </div>
                </div>
            </div>
        `);

            $container.append($card);
        });
    }

    // =============================
    // Guardar orden manual
    // =============================
    $("#btnGuardarOrden").click(function () {
        let nuevoOrden = [];
        $(".pregunta-item").each(function () {
            const id = $(this).data("id");
            const orden = parseInt($(this).find(".orden-input").val(), 10) || 0;
            nuevoOrden.push({ IdPregunta: id, Orden: orden });
        });

        console.log("Nuevo orden:", nuevoOrden);

        // 👉 Aquí va tu AJAX para guardar en BD
        /*
        $.ajax({
            type: "POST",
            url: basePath + "/ConfigClienteSatisfaccion/ReordenarPreguntas",
            data: JSON.stringify(nuevoOrden),
            contentType: "application/json; charset=utf-8",
            success: function(res) {
                toastr.success("Orden guardado");
            }
        });
        */
    });

    // =============================
    // Modal de flujo
    // =============================
    $(document).on("click", ".btnDefinirFlujo", function () {
        const idPregunta = $(this).data("idpregunta");
        const idOpcion = $(this).data("idopcion");

        $("#flujoModal").data("idpregunta", idPregunta).data("idopcion", idOpcion);

        $("#flujoPreguntaSiguiente").empty();
        preguntas.forEach(p => {
            $("#flujoPreguntaSiguiente").append(`<option value="${p.IdPregunta}">${p.Orden} - ${p.Texto}</option>`);
        });
        console.log(preguntas)
        $("#flujoModal").modal("show");
    });

    $("#btnGuardarFlujo").click(function () {
        const idPregunta = $("#flujoModal").data("idpregunta");
        const idOpcion = $("#flujoModal").data("idopcion");
        const siguiente = $("#flujoPreguntaSiguiente").val();

        console.log("Guardar flujo", { idPregunta, idOpcion, siguiente });

        $("#flujoModal").modal("hide");
    });



    // =============================
    //  Init
    // =============================
    cargarPreguntas();
});

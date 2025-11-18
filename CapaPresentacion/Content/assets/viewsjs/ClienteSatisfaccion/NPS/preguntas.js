// 🔹 Mapeo de texto → animaciones Lottie
const animacionesMap = {
    "Nada probable": "https://lottie.host/655803e4-0256-415d-9a6a-8d8baafa2be4/5TqYWy1Yji.lottie",
    "Poco probable": "https://lottie.host/b7d45573-1280-4570-9d4f-00a38e06cb81/pYQgLgldEV.lottie",
    "Probable": "https://lottie.host/e2201f53-db94-4b18-86aa-5702d2f31bdb/6Amtw7U0sU.lottie",
    "Bastante probable": "https://lottie.host/8e1a7620-447e-4b5e-a970-e3a49eb89c26/V58hFSHZ6U.lottie",
    "Muy probable": "https://lottie.host/79ec4263-fa66-4803-a841-ceba2de1ac46/H7XJFZvqgV.lottie",
    "SÍ": "https://lottie.host/9b7813a8-6d62-4797-87fd-2e2ac1459d43/aLsHQeAYXD.lottie",
    "NO": "https://lottie.host/8ccbf928-f7b6-469e-95c7-650595be5cdc/Y6EcLi6IMm.lottie"
};

// 📌 Variables globales
let datosUsuario = {}; // datos iniciales de pantalla
let respuestas = [];   // respuestas de preguntas
let preguntas = [];
let flujo = [];

$(document).ready(function () {
    basePath = $("#BasePath").val();

    // 🔹 Evento: botón comenzar
    $(document).on("click", "#btnIniciar", function () {
        const tipoDoc = $("#tipoDocumento").val();
        const numeroDoc = $("#numeroDocumento").val().trim();

        if (!tipoDoc) {
            alert("⚠️ Selecciona un tipo de documento");
            return;
        }
        if (!numeroDoc) {
            alert("⚠️ Ingresa el número de documento");
            return;
        }

        // Guardar datos iniciales sin afectar progreso
        datosUsuario = {
            tipoDocumento: tipoDoc,
            numeroDocumento: numeroDoc
        };
        console.log("Datos iniciales:", datosUsuario);

        // Ocultar pantalla inicial y cargar encuesta
        $("#pantalla-inicial").remove();

        obtenerPreguntas().done(function (response) {
            if (response.success && response.data) {
                preguntas = response.data.Preguntas || response.data.preguntas || [];
                flujo = response.data.Flujo || response.data.flujo || [];
                inicializarEncuesta(preguntas, flujo);
                $("#formEncuesta").show();
            } else {
                $("#formEncuesta").html("<h4 class='text-center text-danger'>❌ No se encontraron preguntas</h4>");
            }
        });
    });
});


// 🔹 AJAX para traer preguntas desde backend
function obtenerPreguntas() {
    return $.ajax({
        type: "POST",
        url: basePath + "/ClienteSatisfaccion/GetPreguntasNPS",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        error: (request, status, error) => console.error("Error en la petición:", error)
    });
}


// 🔹 Inicializar encuesta
function inicializarEncuesta(preguntas, flujo) {
    $("#formEncuesta").empty();

    preguntas.forEach((p, idx) => {
        $("#formEncuesta").append(renderPregunta(p, idx));
    });

    $(".pregunta").first().addClass("active");

    // Evento: mostrar input comentario si corresponde
    $(document).on("change", "input[type=radio]", function () {
        const tieneComentario = $(this).data("comentario");
        const pregunta = $(this).closest(".pregunta");

        // Ocultar todos los comentarios de esa pregunta
        pregunta.find(".comentario").hide().val("");

        // Mostrar solo el comentario de la opción seleccionada
        if (tieneComentario) {
            $(this).closest(".opcion-wrap").find(".comentario").show();
        }
    });

    // Evento: botón siguiente
    $(document).on("click", ".btn-siguiente", function () {
        avanzarPregunta($(this).closest(".pregunta"));
    });
}


// 🔹 Renderizar una pregunta
function renderPregunta(p, idx) {
    const tieneAnimaciones = p.Opciones.some(o => animacionesMap[o.Texto]);

    let html = `
      <div class="pregunta ${idx === 0 ? 'active' : ''}" data-id="${p.IdPregunta}" data-orden="${p.Orden}">
        <h4 class="pregunta-title">${p.Texto}</h4>
        <div class="${tieneAnimaciones ? 'opciones-flex' : 'opciones-list'}">
    `;

    p.Opciones.forEach(o => {
        const animacionUrl = animacionesMap[o.Texto] || null;
        const inputComentario = o.TieneComentario
            ? `<input 
                  type="text" 
                  class="form-control comentario" 
                  id="comentario-${p.IdPregunta}-${o.idOpcion}" 
                  name="comentario-${p.IdPregunta}-${o.idOpcion}" 
                  placeholder="Escribe tu comentario..." 
                  style="display:none;">`
            : "";

        if (animacionUrl) {
            // Opción con animación
            html += `
              <div class="opcion-wrap">
                <label class="opcion-cuadro">
                  <input type="radio" name="preg-${p.IdPregunta}" value="${o.idOpcion}" data-comentario="${o.TieneComentario}">
                  <dotlottie-wc 
                    src="${animacionUrl}" 
                    autoplay loop speed="1" 
                    class="animacion-lottie">
                  </dotlottie-wc>
                  <span class="texto-opcion">${o.Texto}</span>
                </label>
                ${inputComentario}
              </div>
            `;
        } else {
            // Opción normal
            html += `
              <div class="opcion-wrap">
                <div class="radio opcion-animada">
                  <label style="width:100%;padding:15px 35px;">
                    <input type="radio" name="preg-${p.IdPregunta}" value="${o.idOpcion}" data-comentario="${o.TieneComentario}">
                    <span class="texto-opcion">${o.Texto}</span>
                  </label>
                </div>
                ${inputComentario}
              </div>
            `;
        }
    });

    html += `
        </div>
        <button type="button" class="btn btn-siguiente btn-block">Siguiente</button>
      </div>
    `;
    return html;
}


// 🔹 Avanzar entre preguntas y guardar respuesta
function avanzarPregunta(container) {
    const idPregunta = container.data("id");
    const ordenPregunta = container.data("orden");
    const seleccion = container.find("input[type=radio]:checked");

    if (seleccion.length === 0) {
        alert("⚠️ Debes seleccionar una opción");
        return;
    }

    const idOpcion = parseInt(seleccion.val());
    let comentario = null;

    if (seleccion.data("comentario")) {
        comentario = container.find(".comentario:visible").val().trim();
        if (comentario === "") {
            alert("⚠️ Debes escribir un comentario");
            return;
        }
    }

    // 📌 Guardar respuesta
    respuestas.push({ idPregunta: parseInt(idPregunta), idOpcion, comentario });
    console.log("Respuestas:", respuestas);

    // 🔹 Buscar siguiente pregunta en flujo
    let siguienteId = null;

    if (flujo && flujo.length > 0) {
        const regla = flujo.find(f => f.IdPreguntaActual === idPregunta && f.IdOpcion === idOpcion);
        if (regla) {
            siguienteId = regla.IdPreguntaSiguiente;
        }
    }

    // 🔹 Si no hay flujo → avanzar por el siguiente orden disponible
    if (!siguienteId) {
        const siguientePregunta = preguntas
            .filter(p => p.Orden > ordenPregunta)
            .sort((a, b) => a.Orden - b.Orden)[0];

        if (siguientePregunta) {
            siguienteId = siguientePregunta.IdPregunta;
        }
    }

    // 🔹 Mostrar progreso (solo sobre preguntas, no datos iniciales)
    const progreso = Math.round((respuestas.length / preguntas.length) * 100);
    $("#progressBar").css("width", progreso + "%").text(progreso + "%");

    // 🔹 Mostrar siguiente pregunta o terminar
    container.removeClass("active");

    if (siguienteId) {
        $(`.pregunta[data-id='${siguienteId}']`).addClass("active");
    } else {
        $("#progressBar").css("width", "100%").text("100%");
        console.log("✅ Datos del usuario:", datosUsuario);
        console.log("✅ Respuestas finales:", respuestas);

        $("#formEncuesta").html("<h3 class='text-center text-success'>✅ Gracias por completar la encuesta</h3>");

        // 👉 Aquí podrías enviar datosUsuario + respuestas al backend
        const encuesta = {
            IdSala: 3,
            IdTablet: 1,
            NroDocumento: datosUsuario.numeroDocumento,
            TipoDocumento: datosUsuario.tipoDocumento,
            IdTipoEncuesta: 1
        };

        $.ajax({
            type: "POST",
            url: basePath + "/ClienteSatisfaccion/GuardarEncuestaConPreguntasNormal",
            data: JSON.stringify({ encuesta, respuestas }),
            contentType: "application/json",
            success: function (res) {
                console.log("Guardado en backend:", res);
            }
        });
    }
}

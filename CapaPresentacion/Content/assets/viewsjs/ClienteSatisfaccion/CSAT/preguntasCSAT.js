// 🔹 Mapeo de texto → animaciones Lottie
//const animacionesMap = {
//    "Muy insatisfecho": "https://lottie.host/655803e4-0256-415d-9a6a-8d8baafa2be4/5TqYWy1Yji.lottie",
//    "Insatisfecho": "https://lottie.host/b7d45573-1280-4570-9d4f-00a38e06cb81/pYQgLgldEV.lottie",
//    "Regular": "https://lottie.host/e2201f53-db94-4b18-86aa-5702d2f31bdb/6Amtw7U0sU.lottie",
//    "Satisfecho": "https://lottie.host/8e1a7620-447e-4b5e-a970-e3a49eb89c26/V58hFSHZ6U.lottie",
//    "Muy satisfecho": "https://lottie.host/79ec4263-fa66-4803-a841-ceba2de1ac46/H7XJFZvqgV.lottie",

//    "Nada probable": "https://lottie.host/655803e4-0256-415d-9a6a-8d8baafa2be4/5TqYWy1Yji.lottie",
//    "Poco probable": "https://lottie.host/b7d45573-1280-4570-9d4f-00a38e06cb81/pYQgLgldEV.lottie",
//    "Me es indiferente": "https://lottie.host/e2201f53-db94-4b18-86aa-5702d2f31bdb/6Amtw7U0sU.lottie",
//    "Probable": "https://lottie.host/8e1a7620-447e-4b5e-a970-e3a49eb89c26/V58hFSHZ6U.lottie",
//    "Muy probable": "https://lottie.host/79ec4263-fa66-4803-a841-ceba2de1ac46/H7XJFZvqgV.lottie",

//    "SÍ": "https://lottie.host/9b7813a8-6d62-4797-87fd-2e2ac1459d43/aLsHQeAYXD.lottie",
//    "NO": "https://lottie.host/8ccbf928-f7b6-469e-95c7-650595be5cdc/Y6EcLi6IMm.lottie"
//};


const animacionesMap = {
    "Muy insatisfecho": "Content/assets/images/nps/muyinsatisfecho.gif",
    "Insatisfecho": "Content/assets/images/nps/insatisfecho.gif",
    "Regular": "Content/assets/images/nps/regular.gif",
    "Satisfecho": "Content/assets/images/nps/satisfecho.gif",
    "Muy satisfecho": "Content/assets/images/nps/muysatisfecho.gif",

    "Nada probable": "Content/assets/images/nps/muyinsatisfecho.gif",
    "Poco probable": "Content/assets/images/nps/insatisfecho.gif",
    "Me es indiferente": "Content/assets/images/nps/regular.gif",
    "Probable": "Content/assets/images/nps/satisfecho.gif",
    "Muy probable": "Content/assets/images/nps/muysatisfecho.gif",

    "SÍ": "Content/assets/images/nps/like.gif",
    "NO": "Content/assets/images/nps/dislike.gif"
};

//  Variables globales
let datosUsuario = {}; // datos iniciales de pantalla
let respuestas = [];   // respuestas de preguntas
let preguntas = [];
let flujo = [];

let nombreEncuestado=""

$(document).ready(function () {
    basePath = $("#BasePath").val();

    $.getJSON(basePath + "/Content/assets/nps/country.json", function (json) {
        renderSelectPaises(json);
    });
    const $button = $("#fullscreen-toggle");
    function enterFullscreen() {
        document.documentElement.requestFullscreen();
    }
    function exitFullscreen() {
        document.exitFullscreen();
    }
    $button.on("click", function () {
        if (!document.fullscreenElement) {
            enterFullscreen();
        } else {
            exitFullscreen();
        }
    });

    // Detectar cambio de estado (por ESC o acción del sistema)
    $(document).on("fullscreenchange", function () {
        if (!document.fullscreenElement) {
            $button.html(`${expandIcon} Pantalla completa`);
        } else {
            $button.html(`${compressIcon} Salir de pantalla completa`);
        }
    });


    const salaId = $("#salaId").val();
    const idTablet = $("#idTablet").val();

    if (!salaId || !idTablet) {
        // Ocultar todo el contenedor de la encuesta
        $(".encuesta-wrapper").hide();

        // Mostrar mensaje de error
        $("body").append(`
            <div class="text-center" style="padding: 50px; color: red; font-size: 20px;">
                ❌ No se obtuvieron los datos necesarios para continuar con la encuesta.
            </div>
        `);
        return; // detener ejecución
    }
    datosUsuario.IdSala = salaId;
    datosUsuario.IdTablet = idTablet;

    ValidarDatosTablet(salaId, idTablet).done(function (response) {
        if (!response.success) {
            $(".encuesta-wrapper").hide();

            // Mostrar mensaje de error
            $("body").append(`
            <div class="text-center" style="padding: 50px; color: red; font-size: 20px;">
                ❌ El id de la tablet no pertenece a la sala.
            </div>
        `);
            return;
        } else {
            

            $("#nombreSala").text(response.data.nombre)
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        $(".encuesta-wrapper").hide();

        $("body").append(`
            <div class="text-center" style="padding: 50px; color: red; font-size: 20px;">
                ❌ Error en la petición.<br>
                Sala ingresada: <b>${salaId}</b><br>
                Tablet ingresada: <b>${idTablet}</b><br>
            </div>
        `);
    });
   
    $(document).on("click", "#btnIniciar", function () {
        const tipoDoc = $("#tipoDocumento").val();
        const numeroDoc = $("#numeroDocumento").val().trim();
        const celular = $("#celular").val().trim();
        const correo = $("#correo").val().trim();
        const codigo = $("#cboPais").val();
        const mayorEdad = $("#chkMayorEdad").is(":checked");

        // Validación tipo documento
        if (!tipoDoc) {
            toastr.warning("⚠️ Selecciona un tipo de documento", "Validación");
            return;
        }
        if (!numeroDoc) {
              
            toastr.warning(" Ingresa el número de documento", "Validación");
            return;
        }


        // ===========================================================
        //  VALIDACIÓN DEL DOCUMENTO SEGÚN TIPO
        // ===========================================================
        let regexDocumento;

        switch (tipoDoc) {
            case "1": // DNI
                regexDocumento = /^\d{8}$/;
                break;

            case "2": // Pasaporte
                regexDocumento = /^[A-Za-z0-9]{6,12}$/;
                break;

            case "3": // Carnet de Extranjería
                regexDocumento = /^(?:\d{8}|[A-Za-z]\d{8})$/;
                break;
        }

        if (!regexDocumento.test(numeroDoc)) {
            toastr.error("⚠️ Ingresa un número de documento válido", "Validación");
            return;
        }

        if (!mayorEdad) {
            toastr.warning("⚠️ Debes confirmar que eres mayor de edad y aceptar políticas de privacidad", "Validación");
            return;
        }

        // Validación celular
        if (celular) {
            if (!codigo) {
                toastr.warning("Elija un código telefónico", "Validación");
                return
            }  
            const regexCelular = /^\d{9}$/;
            if (!regexCelular.test(celular)) {
                toastr.error("⚠️ Ingresa un número de celular válido", "Validación");
                return;
            }
        }

        // Validación correo
        if (correo) {
            const regexCorreo = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!regexCorreo.test(correo)) {
                toastr.error("⚠️ Ingresa un correo válido", "Validación");
                return;
            }
        }

        // ===========================================================
        // 🚨 VerificarConfiguracionCliente se ejecuta PARA TODOS
        // ===========================================================
        $.ajax({
            url: basePath + "/ClienteSatisfaccion/VerificarConfiguracionCliente",
            type: "POST",
            data: {
                idSala: salaId,
                nroDoc: numeroDoc
            },
            success: function (resp) {
                if (resp && resp.success) {

                    // ===========================================================
                    // 🚨 obtenerDatosCliente SOLO si el documento es DNI
                    // ===========================================================
                    if (tipoDoc === "1") {
                        obtenerDatosCliente(numeroDoc).done(function (respCliente) {

                            if (respCliente && respCliente.respuesta && respCliente.data && respCliente.data.length > 0 && respCliente.data[0].Nombre != "") {

                                const cliente = respCliente.data[0];

                                datosUsuario = {
                                    tipoDocumento: tipoDoc,
                                    numeroDocumento: numeroDoc,
                                    celular: celular,
                                    correo: correo,
                                    nombre: cliente.NombreCompleto || "",
                                    idSala: salaId,
                                    idTablet: idTablet,
                                    codigo: codigo
                                };
                                nombreEncuestado = cliente.Nombre;

                                iniciarEncuesta();
                            } else {
                                toastr.error(" No se encontró un cliente con ese DNI.", "Validación");
                            }

                        });

                    } else {
                        //  Pasaporte y Carnet → NO se busca datos del cliente
                        datosUsuario = {
                            tipoDocumento: tipoDoc,
                            numeroDocumento: numeroDoc,
                            celular: celular,
                            correo: correo,
                            nombre: "",
                            idSala: salaId,
                            idTablet: idTablet,
                            codigo: codigo
                        };

                        iniciarEncuesta();
                    }

                } else {
                    toastr.error(resp.displayMessage || "⚠️ No puedes responder la encuesta hoy.", "Validación");
                }
            },
            error: function () {
                toastr.error("❌ Error al verificar la configuración del cliente", "Error");
            }
        });
    });


    // ===========================================================
    //  Función para iniciar la encuesta
    // ===========================================================
    function iniciarEncuesta() {
        $("#pantalla-inicial").hide();

        obtenerPreguntasCSAT().done(function (response) {
            if (response.success && response.data) {
                preguntas = response.data.Preguntas || [];
                flujo = response.data.Flujo || [];
                inicializarEncuesta(preguntas, flujo);
                $("#formEncuesta").show();
            } else {
                $("#formEncuesta").html("<h4 class='text-center text-danger'>❌ No se encontraron preguntas</h4>");
            }
        });
    }

   


});

$(document).on("click", ".btn-cancelar", function () {
    $("#modalCancelarEncuesta").modal("show");
});

$(document).on("click", "#btnConfirmarCancelar", function () {
    $("#modalCancelarEncuesta").modal("hide");

    //  Guardar lo que haya hasta el momento
    const encuesta = {
        IdSala: datosUsuario.idSala,
        IdTablet: datosUsuario.idTablet,
        NroDocumento: datosUsuario.numeroDocumento,
        TipoDocumento: datosUsuario.tipoDocumento,
        Nombre: datosUsuario.nombre,
        Correo: datosUsuario.correo,
        Celular: datosUsuario.celular,
        Codigo: datosUsuario.codigo,
        IdTipoEncuesta: 1,
    };

    $.ajax({
        type: "POST",
        url: basePath + "/ClienteSatisfaccion/GuardarEncuestaConPreguntasNormal",
        data: JSON.stringify({ encuesta, respuestas }),
        contentType: "application/json",
        success: function (res) {
            $("#progressBar").css("width", "100%").text("100%");
            // Mostrar pantalla final de gracias
            $("#formEncuesta").html(`
                <h3 class='text-center text-success'> ¡GRACIAS ${nombreEncuestado} POR TU TIEMPO!</h3>
                <p class='text-center text-success' style='font-size:24px;'>Tu opinión es clave para optimizar nuestros servicios.</p>
                <br/>
            `);
            lanzarConfetiYRecargar()
        },
        error: function () {
            toastr.error("Error al guardar encuesta cancelada", "Servidor");
        }
    });
});
function primeraPalabra(texto) {
    if (!texto) return ""; //  si es null, undefined o vacío retorna vacío
    return texto.trim().split(" ")[0];
}

function getQueryParam(name) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(name);
}
//Obtener datos cliente
function obtenerDatosCliente(dni) {
    return $.ajax({
        type: 'POST',
        url: basePath + "/AsistenciaCliente/GetListadoClienteCoincidencia",
        data: JSON.stringify({ coincidencia: dni }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        error: (request, status, error) => console.error("Error en la petición:", error)
    })
}

function ValidarDatosTablet(salaId, tabletId) {
    return $.ajax({
        type: 'POST',
        url: basePath + "ClienteSatisfaccion/ValidarDatosEncuesta",
        data: JSON.stringify({ salaId, tabletId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        error: (request, status, error) => console.error("Error en la petición:", error)
    })
}


// 🔹 AJAX para traer preguntas desde backend
function obtenerPreguntasCSAT() {
    return $.ajax({
        type: "POST",
        url: basePath + "/ClienteSatisfaccion/GetPreguntasCSAT",
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
    $(document).off("click", ".btn-siguiente");
    $(document).off("click", ".btn-cancelar");
    $(document).off("change", "input[type=radio], input[type=checkbox]");
    preguntas.forEach((p, idx) => {
        $("#formEncuesta").append(renderPregunta(p, idx));
    });

    $(".pregunta").first().addClass("active");

    // Evento: mostrar/ocultar input comentario en radio o checkbox
    $(document).on("change", "input[type=radio], input[type=checkbox]", function () {
        const $pregunta = $(this).closest(".pregunta");
        const $comentario = $(this).closest(".opcion-wrap").find(".comentario");
        const tieneComentario = $(this).data("comentario");

        if ($(this).attr("type") === "radio") {
            $pregunta.find(".comentario").hide().val("");

            if (tieneComentario && $(this).is(":checked")) {
                $comentario.show();
            }
        } else {
            if (tieneComentario && $(this).is(":checked")) {
                $comentario.show();
            } else {
                $comentario.hide().val("");
            }
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
    const inputType = p.Multi ? "checkbox" : "radio";

    let html = `
      <div class="pregunta ${idx === 0 ? 'active' : ''}" 
           data-id="${p.IdPregunta}" 
           data-orden="${p.Orden}" 
           data-multi="${p.Multi}">
        <h4 class="pregunta-title">${p.Texto}</h4>
        <div class="${tieneAnimaciones ? 'opciones-flex' : 'opciones-list'}">
    `;

    p.Opciones.forEach(o => {
        const animacionUrl = animacionesMap[o.Texto] || null;
        const inputComentario = o.TieneComentario
            ? `<input type="text" class="form-control comentario" 
                      id="comentario-${p.IdPregunta}-${o.idOpcion}" 
                      placeholder="Escribe tu comentario..." 
                      style="display:none;">`
            : "";

        if (animacionUrl) {
            // 🔹 Reemplazado <dotlottie-wc> por <img> para mostrar GIFs
            html += `
              <div class="opcion-wrap">
                <label class="opcion-cuadro">
                  <input type="${inputType}" name="preg-${p.IdPregunta}" value="${o.idOpcion}" data-comentario="${o.TieneComentario}">
                  <img style="width: 75%;" src="${basePath}/${animacionUrl}" alt="${o.Texto}" class="animacion-gif" />
                  <span class="texto-opcion">${o.Texto}</span>
                </label>
                ${inputComentario}
              </div>
            `;
        } else {
            html += `
              <div class="opcion-wrap">
                <div class="radio opcion-animada">
                  <label style="width:100%;padding:5px 35px;">
                    <input type="${inputType}" name="preg-${p.IdPregunta}" value="${o.idOpcion}" data-comentario="${o.TieneComentario}">
                    <span class="texto-opcion">${o.Texto}</span>
                  </label>
                </div>
                ${inputComentario}
              </div>
            `;
        }
    });

    html += `</div>`;

    //  Botón siguiente
    html += `<button type="button" class="btn btn-siguiente btn-block" >Siguiente</button>`;

    //  A partir de la segunda pregunta agregamos botón cancelar
    if (idx > 0) {
        html += `<button type="button" class="btn btn-link text-danger btn-cancelar" style="position: absolute;top: 0;right: 0;">✖ </button>`;
    }

    html += `</div>`;
    return html;
}


// 🔹 Avanzar entre preguntas y guardar respuesta
function avanzarPregunta(container) {
    const idPregunta = parseInt(container.data("id"));
    const ordenPregunta = container.data("orden");
    const esMulti = container.data("multi") === true || container.data("multi") === "true";

    //  Selección de opciones
    const seleccionados = container.find("input:checked");

    if (seleccionados.length === 0) {
        toastr.warning("⚠️ Debes seleccionar al menos una opción");
        return;
    }

    let valido = true; 
    let respuestasTemp = []; 

    //  Guardar respuestas
    seleccionados.each(function () {
        const idOpcion = parseInt($(this).val());
        let comentario = null;

        if ($(this).data("comentario")) {
            comentario = $(this).closest(".opcion-wrap").find(".comentario:visible").val().trim();
            if (comentario === "") {
                toastr.warning(" Debes escribir un comentario en la opción seleccionada");
                valido = false;
                return false; // corta el each
            }
        }

        respuestasTemp.push({ idPregunta, idOpcion, comentario });
    });
    if (!valido) {
        return; //  detiene toda la función
    }
    respuestas.push(...respuestasTemp);
    //  Buscar siguiente pregunta en flujo
    let siguienteId = null;
    // El flujo solo aplica a preguntas de opción única
    if (!esMulti && flujo && flujo.length > 0) {
        const idOpcionSeleccionada = parseInt(seleccionados.val());
        const regla = flujo.find(f => f.IdPreguntaActual === idPregunta && f.IdOpcion === idOpcionSeleccionada);
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

    // 🔹 Mostrar progreso
    const progreso = Math.round((respuestas.length / preguntas.length) * 100);
    $("#progressBar").css("width", progreso + "%").text(progreso + "%");

    // 🔹 Mostrar siguiente pregunta o terminar
    container.removeClass("active");

    if (siguienteId) {
        $(`.pregunta[data-id='${siguienteId}']`).addClass("active");
    } else {
        $("#progressBar").css("width", "100%").text("100%");
        $("#formEncuesta").html(`
            <h3 class='text-center text-success'>¡✅ GRACIAS ${nombreEncuestado} POR TU TIEMPO!</h3>
            <br/>
            <p class='text-center text-success' style='font-size: 24px;'> <strong>Ya ganaste 01 opción para el </strong></p>
            <p class="text-center text-success" style='font-size: 24px;'><strong>Sorteo en "Extra Win". ¡SUERTE!</strong></p>
        `);

        // 👉 Enviar datos al backend
        const encuesta = {
            IdSala: datosUsuario.idSala,
            IdTablet: datosUsuario.idTablet,
            NroDocumento: datosUsuario.numeroDocumento,
            TipoDocumento: datosUsuario.tipoDocumento,
            Nombre: datosUsuario.nombre,
            Correo: datosUsuario.correo,
            Celular: datosUsuario.celular,
            Codigo: datosUsuario.codigo,
            IdTipoEncuesta: 1,
        };

        $.ajax({
            type: "POST",
            url: basePath + "/ClienteSatisfaccion/GuardarEncuestaConPreguntasNormal",
            data: JSON.stringify({ encuesta, respuestas }),
            contentType: "application/json",
            success: function (res) {
                lanzarConfetiYRecargar()
            }
        });
    }
    }


function obtenerPrimeraPalabra(texto) {
    if (!texto) return "";

    const palabras = texto.trim().split(/\s+/);
    return palabras.length > 0 ? palabras[0] : "";
}

// 👉 Función para lanzar confeti y recargar
function lanzarConfetiYRecargar() {
    //  Confetti por 5 segundos
    var duration = 5 * 1000;
    var animationEnd = Date.now() + duration;
    var defaults = { startVelocity: 30, spread: 360, ticks: 60, zIndex: 9999 };

    function randomInRange(min, max) {
        return Math.random() * (max - min) + min;
    }

    var interval = setInterval(function () {
        var timeLeft = animationEnd - Date.now();

        if (timeLeft <= 0) {
            return clearInterval(interval);
        }

        var particleCount = 50 * (timeLeft / duration);
        confetti(Object.assign({}, defaults, {
            particleCount,
            origin: { x: randomInRange(0.1, 0.9), y: Math.random() - 0.2 }
        }));
    }, 250);

    //  Refrescar la página después de 10 segundos
    setTimeout(() => {
        reiniciarEncuesta()
    }, 10000);
}


function reiniciarEncuesta() {
    datosUsuario = {
        IdSala: datosUsuario.IdSala,
        IdTablet: datosUsuario.IdTablet,
        tipoDocumento: null,
        numeroDocumento: null,
        celular: null,
        correo: null,
        nombre: null,
        codigo:null
    };
    // 🔹 Resetear variables globales
    respuestas = [];
   
    // 🔹 Restaurar barra de progreso
    $("#progressBar").css("width", "0%").text("0%");

    // 🔹 Limpiar preguntas del formulario y ocultarlo
    $("#formEncuesta").empty().hide();

    // 🔹 Limpiar inputs del formulario inicial
    $("#tipoDocumento").val("");
    $("#numeroDocumento").val("");
    $("#celular").val("");
    $("#correo").val("");
    $("#cboPais").val(null).trigger("change");
    $("#chkMayorEdad").prop("checked", false);

    // 🔹 Volver a mostrar pantalla inicial
    $("#pantalla-inicial").fadeIn(500).addClass("active");

    // 🔹 Asegurar que no quede ninguna pregunta activa
    $(".pregunta").removeClass("active");
    $(document).off("click", ".btn-siguiente");
    $(document).off("click", ".btn-cancelar");
    $(document).off("change", "input[type=radio], input[type=checkbox]");
    // 🔹 Volver a mostrar título de sala (si lo tienes)
    $("#nombreSala").text($("#nombreSala").text());
}




function renderSelectPaises(data) {
    const $select = $("#cboPais");
    $select.empty();

    if (Array.isArray(data) && data.length) {

        $select.append(`<option></option>`);

        data.forEach(p => {
            $select.append(`
                <option style="color:black" value="${p.CodigoTelefonico}" data-flag="${p.Bandera}">
                    ${p.Nombre} (+${p.CodigoTelefonico})
                </option>
            `);
        });

        $select.select2({
            placeholder: "--Seleccione país--",
            allowClear: true,
            width: "100%",
            templateResult: templatePais,
            templateSelection: templatePais,
        });

    } else {
        toastr.warning("No hay países disponibles.");
    }
}

function templatePais(option) {
    if (!option.id) return option.text;

    const flag = $(option.element).data("flag");

    return $(`
        <span style="display:flex; align-items:center;">
            <img src="${flag}" width="25" height="18" style="margin-right:8px; border:1px solid #ddd;" />
            ${option.text}
        </span>
    `);
}


const cboSala = $("#cboSala");
const btnNuevo = $("#btnNuevo");
const defaultConfiguration = {
    Id: 0,
    TipoValidacionEnvioRespuesta: 1,
    TiempoEsperaRespuesta: 1440,
    MensajeTiempoEsperaRespuesta: "Tiene que esperar para poder enviar otra respuesta.",
    EnvioMaximoDiario: 1,
    MensajeEnvioMaximoDiario: 'Ya supero los envíos diarios disponibles.',
    EnvioMaximoMensual: 10,
    MensajeEnvioMaximoMensual: 'Ya supero los envíos mensuales disponibles.',
    RespuestasAnonimas: false,
    EncuestaActiva: true,
}

$(document).ready(() => {
    getSalas();
})

const getSalas = () => {
    $.when(
        llenarSelect(`${basePath}Sala/ListadoSalaPorUsuarioJson`, {}, "cboSala", "CodSala", "Nombre")
    ).then(() => {
        cboSala.select2({
            placeholder: "Seleccione ...",
            multiple: false,
        });
    });
}

cboSala.on('change', () => {
    const codSala = cboSala.val();
    if (!codSala) {
        toastr.warning("Seleccione una sala", "Aviso")
        return;
    }
    GetConfiguracionByCodSala(codSala);
});

const GetConfiguracionByCodSala = (codSala) => {
    const url = `${basePath}EscConfiguraciones/GetConfiguracionByCodSala`;
    const data = { codSala };

    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            const configuracion = response.data.Id > 0 ? response.data : defaultConfiguration;
            mostrarConfiguracion(configuracion);
            if (!response.success) {
                defaultConfiguration.CodSala = codSala;
                guardarConfiguracion(defaultConfiguration)
                return;
            }
        },
        error: () => toastr.error("Error al obtener las preguntas.", "Error")
    });
};

function mostrarConfiguracion(configuracion) {
    const html = `
        <input type="hidden" id="IdConfiguration" value="${configuracion.Id}">
        <div class="col-md-6">
            <div class="card mb-4">
                <div class="form-group slider-container" style="justify-self: right;padding: 8px;margin-bottom: -50px;">
                <label style="font-size: 13px; vertical-align: -webkit-baseline-middle;">Diario</label>
                <label class="switch">
                        <input type="checkbox" id="toggleConfiguracion">
                        <span class="slider round"></span>
                    </label>
                <label style="font-size: 13px; vertical-align: -webkit-baseline-middle;">Tiempo</label>

                </div>
                <div id="envio-tiempo-espera" style="display: none;">
                    <div class="card-header bg-primary text-white">
                        <h5 class="card-title mb-0">Configuración de Tiempo de Espera</h5>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="TiempoEsperaRespuesta">Tiempo Espera Respuesta (minutos)</label>
                            <input type="number" class="form-control" id="TiempoEsperaRespuesta" min="1" value="${configuracion.TiempoEsperaRespuesta}">
                        </div>
                        <div class="form-group">
                            <label for="MensajeTiempoEsperaRespuesta">Mensaje Tiempo Espera Respuesta</label>
                            <textarea class="form-control textarea" id="MensajeTiempoEsperaRespuesta">${configuracion.MensajeTiempoEsperaRespuesta}</textarea>
                        </div>
                    </div>
                </div>
                <div id="envio-diario">
                
                    <div class="card-header bg-primary text-white">
                        <h5 class="card-title mb-0">Configuración de Envíos Diarios</h5>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="EnvioMaximoDiario">Envío Máximo Diario</label>
                            <input type="number" class="form-control" id="EnvioMaximoDiario" min="1" value="${configuracion.EnvioMaximoDiario}">
                        </div>
                        <div class="form-group">
                            <label for="MensajeEnvioMaximoDiario">Mensaje Envío Máximo Diario</label>
                            <textarea class="form-control textarea" id="MensajeEnvioMaximoDiario">${configuracion.MensajeEnvioMaximoDiario}</textarea>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-6">
            <div class="card mb-4">
                <div class="card-header bg-primary text-white">
                    <h5 class="card-title mb-0">Configuración de Envíos Mensuales</h5>
                </div>
                <div class="card-body">
                    <div class="form-group">
                        <label for="EnvioMaximoMensual">Envío Máximo Mensual</label>
                        <input type="number" class="form-control" id="EnvioMaximoMensual" min="1" value="${configuracion.EnvioMaximoMensual}">
                    </div>
                    <div class="form-group">
                        <label for="MensajeEnvioMaximoMensual">Mensaje Envío Máximo Mensual</label>
                        <textarea class="form-control textarea" id="MensajeEnvioMaximoMensual">${configuracion.MensajeEnvioMaximoMensual}</textarea>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-6">
            <div class="card mb-4">
                <div class="card-header bg-primary text-white">
                    <h5 class="card-title mb-0">Opciones Adicionales</h5>
                </div>
                <div class="card-body">
                    <div class="form-group slider-container">
                        <label>Respuestas Anónimas</label>
                        <label class="switch">
                            <input type="checkbox" id="RespuestasAnonimas" ${configuracion.RespuestasAnonimas ? "checked" : ""}>
                            <span class="slider round"></span>
                        </label>
                    </div>
                    <div class="form-group slider-container">
                        <label>Encuesta Activa</label>
                        <label class="switch">
                            <input type="checkbox" id="EncuestaActiva" ${configuracion.EncuestaActiva ? "checked" : ""}>
                            <span class="slider round"></span>
                        </label>
                    </div>
                </div>
            </div>
        </div>
    `;

    $("#content").html(html);
    $("#btnGuardar").show();

    const toggleSwitch = $("#toggleConfiguracion");
    const envioTiempoEspera = $("#envio-tiempo-espera");
    const envioDiario = $("#envio-diario");
    envioDiario.show();
    envioTiempoEspera.hide();
    toggleSwitch.on("change", function () {
        if (this.checked) {
            envioTiempoEspera.show();
            envioDiario.hide();
        } else {
            envioDiario.show();
            envioTiempoEspera.hide();
        }
    });
}
$("#btnGuardar").click(function () {
    const codSala = cboSala.val();
    const toggleSwitch = $("#toggleConfiguracion");

    if (!codSala) {
        toastr.warning('Seleccione una sala para poder guardar la configuración.', 'Aviso')
        return;
    }

    const tipoValidacionEnvioRespuesta = toggleSwitch.is(":checked") ? 2 : 1;
    const configuracion = {
        CodSala: codSala,
        Id: $("#IdConfiguration").val(),
        TipoValidacionEnvioRespuesta: toggleSwitch.is(":checked") ? 2 : 1,
        TiempoEsperaRespuesta: $("#TiempoEsperaRespuesta").val(),
        MensajeTiempoEsperaRespuesta: $("#MensajeTiempoEsperaRespuesta").val(),
        EnvioMaximoDiario: $("#EnvioMaximoDiario").val(),
        MensajeEnvioMaximoDiario: $("#MensajeEnvioMaximoDiario").val(),
        EnvioMaximoMensual: $("#EnvioMaximoMensual").val(),
        MensajeEnvioMaximoMensual: $("#MensajeEnvioMaximoMensual").val(),
        RespuestasAnonimas: $("#RespuestasAnonimas").is(":checked"),
        EncuestaActiva: $("#EncuestaActiva").is(":checked")
    };

    guardarConfiguracion(configuracion);
});

function guardarConfiguracion(configuracion) {
    const url = `${basePath}EscConfiguraciones/SaveConfiguracion`;

    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(configuracion),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            if (response.success) {
                toastr.success("Configuración guardada correctamente", "Éxito");
                $("#IdConfiguration").val(response.id);
            } else {
                toastr.error(response.displayMessage, "Error");
            }
        },
        error: () => toastr.error("Error al guardar la configuración", "Error")
    });
}



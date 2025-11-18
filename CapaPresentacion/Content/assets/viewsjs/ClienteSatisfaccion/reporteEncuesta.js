// === GLOBAL STATE (compartido por todos los módulos) ===
const basePath = window.basePath || $("#BasePath").val() || "";
let activeTabId = null;
let selectedTablets = [];

let dataCsat = null;
let dataNps = null;
let dataAtributo = null;
let dataComentarioNps = [];
// Tabs -> clave de atributos (coincide con tu reporteAtributos.js)
const tabsMap = {
    //"link-tab-amabilidad": "AMABILIDAD",
    //"link-tab-soporte": "SOPORTE",
    //"link-tab-shows": "SHOWS",
    //"link-tab-alimentos": "ALIMENTOS",
    //"link-tab-cortesias": "CORTESIAS",
    //"link-tab-promociones": "PROMOCIONES",
    //"link-tab-sorteos": "SORTEOS",
    //"link-tab-horarios": "HORARIOS",
    //"link-tab-caja": "CAJA",
    //"link-tab-variedad": "VARIEDAD",
    //"link-tab-ruleta": "RULETA",
    //"link-tab-derby": "DERBY"
};

// === HELPERS COMUNES ===
function safeNum(v) { const n = parseFloat(v); return isNaN(n) ? 0 : n; }
function pad2(n) { return (n < 10 ? '0' : '') + n; }
function formateaRango(ini, fin) {
    if (!ini || !fin) return "";
    const a = moment(ini), b = moment(fin);
    if (!a.isValid() || !b.isValid()) return "";
    return a.locale('es').format("DD MMM") + " - " + b.locale('es').format("DD MMM YYYY");
}

// === AJAX comunes ===
function obtenerListaSalas() {
    return $.ajax({
        type: "POST", url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false, contentType: "application/json; charset=utf-8", dataType: "json",
        beforeSend: () => $.LoadingOverlay && $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay && $.LoadingOverlay("hide")
    });
}

function obtenerTabletsSala(salaId) {
    return $.ajax({
        type: "POST", url: basePath + "ConfigClienteSatisfaccion/ObtenerTabletsSala",
        cache: false, contentType: "application/json; charset=utf-8", data: JSON.stringify({ salaId }),
        dataType: "json",
        beforeSend: () => $.LoadingOverlay && $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay && $.LoadingOverlay("hide")
    });
}

// === RENDER UI: Salas & Tablets ===
function renderSelectSalas(data) {
    const $select = $("#cboSala");
    $select.empty();
    if (Array.isArray(data) && data.length) {
        data.forEach(s => $select.append(`<option value="${s.CodSala}">${s.Nombre}</option>`));
        $select.select2({ placeholder: "--Seleccione--", allowClear: true, width: "100%" });
        // seleccionar 1ra sala por defecto (si no hay selección previa)
        if (!$select.val()) {
            $select.val(String(data[0].CodSala)).trigger("change");
        }
    } else {
        $("#uiContainer").hide();
        $("#mensajeSinSalas").remove();
        $("#filtros-container").append(`
      <div id="mensajeSinSalas" class="alert alert-warning text-center mt-3">
        No tiene salas asignadas
      </div>
    `);
    }
}

function renderizarTablets(tablets) {
    const $container = $("#tablets");
    $container.empty();
    if (!Array.isArray(tablets) || tablets.length === 0) {
        $container.append("<p>No hay tablets registradas.</p>");
        selectedTablets = []; // sin tablets
        return;
    }
    // dibujar checkboxes + restaurar selección previa
    tablets.forEach(tab => {
        const checked = selectedTablets.includes(tab.IdTablet) ? "checked" : "";
        $container.append(`
      <div class="form-check">
        <input class="form-check-input" type="checkbox" id="tablet_${tab.IdTablet}" value="${tab.IdTablet}" ${checked}>
        <label class="form-check-label" for="tablet_${tab.IdTablet}">${tab.Nombre}</label>
      </div>
    `);
    });

    
}

// === DISPARADOR GENERAL POR TAB ACTIVO ===
function triggerActiveModule() {
    if (!activeTabId) {
        activeTabId = $('.nav.nav-tabs li.active a').attr('id') || activeTabId;
        if (!activeTabId) return;
    }

    const start = $("#startDate").val();
    const end = $("#endDate").val();
    const sala = $("#cboSala").val();

    // NPS
    if (activeTabId === "link-tab-nps") {
        if (typeof npsCargar === "function") npsCargar();
        return;
    }

    // CSAT
    if (activeTabId === "link-tab-csat") {
        if (typeof csatCargar === "function") csatCargar();
        return;
    }

    // COMENTARIOS NPS
    if (activeTabId === "tab-gestion-nps") {
        if (typeof ObtenerComentariosNps === "function") {
            ObtenerComentariosNps(start, end, sala); // 👈 ya usa tablets globales
        }
        return;
    }

    // Atributos dinámicos
    const $tab = $("#" + activeTabId);
    const indicador = $tab.data("indicador");
    const idPregunta = $tab.data("idpregunta");

    if (indicador && typeof ejecutarFuncion === "function") {
        ejecutarFuncion(indicador.toLowerCase(), start, end, sala, idPregunta);
    }
}



// === INIT FECHAS + QUICK FILTERS (30 días) ===
function initFechasYQuick() {
    const hoy = moment();
    const hace30 = moment().subtract(29, 'days');

    $("#startDate").datetimepicker({ format: 'DD/MM/YYYY', maxDate: hoy, useCurrent: false, defaultDate: hace30 });
    $("#endDate").datetimepicker({ format: 'DD/MM/YYYY', maxDate: hoy, useCurrent: false, defaultDate: hoy });

    $("#quick-filters button").on("click", function () {
        let filter = $(this).data("filter");
        let start, end = moment();
        switch (filter) {
            case "day": start = moment().subtract(1, 'days'); break;
            case "week": start = moment().subtract(7, 'days'); break;
            case "month": start = moment().subtract(1, 'months'); break;
            case "quarter": start = moment().subtract(3, 'months'); break;
            case "year": start = moment().subtract(1, 'years'); break;
            default: start = hace30;
        }
        $('#startDate').data("DateTimePicker").date(start);
        $('#endDate').data("DateTimePicker").date(end);
        $("#quick-filters button").removeClass("btn-primary").addClass("btn-default");
        $(this).removeClass("btn-default").addClass("btn-primary");
    });
}

// === INIT SALAS & TABLETS + ENLACES ===
function initSalasYTablets() {
    return obtenerListaSalas().then(result => {
        const lista = (result && result.data) ? result.data : [];
        renderSelectSalas(lista);

        const salaId = $("#cboSala").val();
        if (!salaId) return;

        return obtenerTabletsSala(salaId).done(res => {
            if (res && res.success) { renderizarTablets(res.data); } else { $("#tablets").html("<p>No se encontraron tablets.</p>"); }
        });
    });
}

// === BINDINGS GLOBALES ===
function bindEventosGlobales() {
    // Cambiar sala => recargar tablets y refrescar módulo activo
    $(document).on("change", "#cboSala", function () {
        const salaId = $(this).val();
        obtenerTabletsSala(salaId).done(res => {
            if (res && res.success) { renderizarTablets(res.data); }
            // después de redibujar checkboxes, volvemos a calcular selectedTablets desde UI
            selectedTablets = $(".form-check-input:checked").map(function () { return parseInt($(this).val()); }).get();
            triggerActiveModule();
        });
    });

    // Check/uncheck tablets => guardar selección y aplicar al tab activo
    // Check/uncheck tablets => guardar selección y aplicar al tab activo
    $(document).on("change", ".form-check-input", function () {
        selectedTablets = $(".form-check-input:checked")
            .map(function () { return parseInt($(this).val()); })
            .get();

        if (activeTabId === "link-tab-nps" && typeof npsAplicarFiltro === "function") {
            npsAplicarFiltro();
        }
        if (activeTabId === "link-tab-csat" && typeof csatAplicarFiltro === "function") {
            csatAplicarFiltro();
        }

        if (activeTabId === "tab-gestion-nps" && typeof ObtenerComentariosNps === "function") {
            const start = $("#startDate").val();
            const end = $("#endDate").val();
            const sala = $("#cboSala").val();
            ObtenerComentariosNps(start, end, sala); // 👈 reconsulta con tablets seleccionadas
        }

        // 🔹 Si el tab activo es un atributo dinámico
        if (tabsMap[activeTabId] && typeof atributoAplicarFiltro === "function") {
            atributoAplicarFiltro(tabsMap[activeTabId]);
        }
    });



    // Aplicar (fechas/sala) => ejecuta módulo activo
    $("#applyFilter").on("click", function () {
        const start = $("#startDate").val();
        const end = $("#endDate").val();
        const sala = $("#cboSala").val();
        if (!start || !end || !sala) { toastr.warning("Selecciona ambas fechas y una sala"); return; }
        if (!activeTabId) { toastr.warning("No hay un tab seleccionado"); return; }
        triggerActiveModule();
    });

    // Cambios de tab => marcar tab activo y disparar su carga
    $(document).on('shown.bs.tab', 'a[data-toggle="tab"]', function (e) {
        activeTabId = $(e.target).attr("id");
        triggerActiveModule();
    });
}


function renderTabsAtributos(indicadores) {
    const $menu = $("#dropdown-atributos"); // tu <ul class="dropdown-menu">
    $menu.empty();

    indicadores.forEach((item, i) => {
        const indicador = (item.Indicador || "").toLowerCase(); // 👈 aseguramos string
        const tabId = `link-tab-${indicador}-${item.IdPregunta}`;
        tabsMap[tabId] = item.Indicador;

        $menu.append(`
        <li>
          <a href="#tab-atributo" role="tab" data-toggle="tab"
             id="${tabId}"
             data-indicador="${item.Indicador || ""}"
             data-idpregunta="${item.IdPregunta}">
            ${item.Indicador }
          </a>
        </li>
    `);
    });

}
function obtenerPreguntasAtributo() {
    return $.ajax({
        type: "POST",
        url: basePath + "ConfigClienteSatisfaccion/ObtenerPreguntaAtributos", // 👈 tu endpoint
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: () => $.LoadingOverlay && $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay && $.LoadingOverlay("hide")
    });
}


// === ENTRADA PRINCIPAL ===
$(document).ready(function () {
    initFechasYQuick();

    obtenerPreguntasAtributo().done(res => {

        // res debería tener: { success: true, data: [ ... ] }
        if (res && res.success && Array.isArray(res.data)) {
            renderTabsAtributos(res.data);
        } else {
            toastr.error(res.message || "No se pudo cargar las preguntas");
        }
    });
    initSalasYTablets().then(() => {
        // Determinar tab activo de entrada (si ya existe uno marcado por Bootstrap)
        activeTabId = $('.nav.nav-tabs li.active a').attr('id') || activeTabId;
        // Primera carga automática
        triggerActiveModule();
    });
    bindEventosGlobales();
 
    $(document).on("click", "#btnExcel", function () {
        let fechaIni = $("#startDate").val();
        let fechaFin = $("#endDate").val();
        let sala = $("#cboSala").val();;
        if (!sala) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }

        $.ajax({
            type: "POST",
            url: basePath + "ConfigClienteSatisfaccion/ExportarEncuestasExcel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ salaId: sala, fechaInicio: fechaIni, fechaFin: fechaFin }),
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                if (response.respuesta) {
                    let data = response.data;       // Excel en base64
                    let file = response.excelName;  // Nombre del archivo
                    let a = document.createElement('a');
                    a.href = "data:application/vnd.ms-excel;base64," + data;
                    a.download = file;
                    a.click();
                } else {
                    toastr.error(response.mensaje || "No se pudo generar el Excel", "Error");
                }
            },
            error: function () {
                toastr.error("Error de conexión con el servidor");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            }
        });
    });


});

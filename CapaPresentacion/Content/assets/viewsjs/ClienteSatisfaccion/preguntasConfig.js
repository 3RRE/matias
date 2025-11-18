$(document).ready(function () {
    let preguntas = [];

    // =============================
    // 📌 CARGAR PREGUNTAS
    // =============================
    function cargarPreguntas() {
        $.post(basePath + "/ConfigClienteSatisfaccion/ObtenerPreguntas", { tipoEncuesta: 1 }, function (res) {
            if (res.success) {
                preguntas = res.data;
                renderPreguntas();
            } else {
                toastr.error("Error al cargar preguntas");
            }
        });
    }

    $("#startDate").datetimepicker({
        format: "DD/MM/YYYY",
        useCurrent: false
    });

    $("#endDate").datetimepicker({
        format: "DD/MM/YYYY",
        useCurrent: false
    });

    //// 🔹 Cuando cambia startDate, endDate no puede ser menor
    //$("#startDate").on("dp.change", function (e) {
    //    $("#endDate").data("DateTimePicker").minDate(e.date);
    //});

    //// 🔹 Cuando cambia endDate, startDate no puede ser mayor
    //$("#endDate").on("dp.change", function (e) {
    //    $("#startDate").data("DateTimePicker").maxDate(e.date);
    //});

    // =============================
    // 📌 RENDER PREGUNTAS
    // =============================
    function renderPreguntas() {
        $("#listCSAT, #listNPS, #listNormales, #listAtributo").empty();

        preguntas.filter(p => p.Indicador === "CSAT").forEach(p => {
            $("#listCSAT").append(renderCard(p, "CSAT"));
        });

        preguntas.filter(p => p.Indicador === "NPS").forEach(p => {
            $("#listNPS").append(renderCard(p, "NPS"));
        });

        preguntas.filter(p => !p.Indicador && !p.Random).forEach(p => {
            $("#listNormales").append(renderCard(p, "NORMAL"));
        });

        preguntas.filter(p => p.Random).forEach(p => {
            $("#listAtributo").append(renderCard(p, "ATRIBUTO"));
        });
    }




    // =============================
    // 📌 RENDER CARD
    // =============================
    function renderCard(pregunta, modo = "NORMAL") {

        //botones para preguntas normales
        //<button class="btn btn-xs btn-danger btnEliminarPregunta"><i class="glyphicon glyphicon-trash"></i> Eliminar</button>

        //boton para eliminar opciones de una pregunta normal
        //<button class="btn btn-xs btn-danger btnEliminarOpcion"><i class="glyphicon glyphicon-trash"></i></button>

        let card = `
<div class="cardPregunta panel panel-default" style="padding:0 !important" data-id="${pregunta.IdPregunta}">
  <div class="panel-heading" style="display:flex;justify-content:space-between;align-items:center;">
    <!-- 🔹 Texto de la pregunta con toggle -->
    <a id="opcionPregunta" href="#collapse-${pregunta.IdPregunta}" data-toggle="collapse" style="flex:1; cursor:pointer; text-decoration:none;">
      <p style="max-width:650px; font-weight: 600;">${pregunta.Texto}</p>
      ${!pregunta.Activo ? '<span class="label label-danger" style="margin-left:10px;">Inactiva</span>' : ""}
    </a>

    <!-- 🔹 Botones a la derecha -->
    <div class="acciones">
      ${modo === "NORMAL" ? `
        <button class="btn btn-xs btn-info btnEditarPregunta"><i class="glyphicon glyphicon-edit"></i> Editar</button>
        <button class="btn btn-xs btn-success btnAgregarOpcion"><i class="glyphicon glyphicon-plus"></i> Opción</button>
        <button class="btn btn-xs btn-danger btnEliminarPregunta"><i class="glyphicon glyphicon-trash"></i> Eliminar</button>
      ` : ""}
      
      ${modo === "ATRIBUTO" ? `
        <button class="btn btn-xs btn-info btnEditarPregunta"><i class="glyphicon glyphicon-edit"></i> Editar</button>
        <button class="btn btn-xs ${pregunta.Activo ? "btn-warning" : "btn-success"} btnTogglePregunta">
          ${pregunta.Activo
                    ? '<i class="glyphicon glyphicon-ban-circle"></i> Desactivar'
                    : '<i class="glyphicon glyphicon-ok"></i> Activar'}
        </button>
      ` : ""}
    </div>
  </div>

  <!-- 🔹 Opciones colapsadas -->
  <div id="collapse-${pregunta.IdPregunta}" class="panel-collapse collapse">
    <div class="panel-body">
      <ul class="list-unstyled opciones-list">
        ${pregunta.Opciones.map(o => `
          <li data-id="${o.idOpcion}" style="padding:5px 0;border-bottom:1px solid #f0f0f0;">
            <span>${o.Texto} ${o.TieneComentario ? "(+comentario)" : ""}</span>
            <div class="pull-right">
              ${modo === "NORMAL" ? `
                <button class="btn btn-xs btn-info btnEditarOpcion"><i class="glyphicon glyphicon-pencil"></i></button>
                <button class="btn btn-xs btn-danger btnEliminarOpcion"><i class="glyphicon glyphicon-trash"></i></button>
              ` : ""}
            </div>
          </li>
        `).join("")}
      </ul>
    </div>
  </div>
</div>`;
        return card;
    }




    // Activar / Desactivar Pregunta
    $(document).on("click", ".btnTogglePregunta", function () {
        let id = $(this).closest(".cardPregunta").data("id");
        $.post(basePath + "/ConfigClienteSatisfaccion/TogglePregunta", { idPregunta: id }, function (res) {
            if (res.success) {
                toastr.success("Se actulizó la pregunta correctamente.");
                cargarPreguntas();
            } else {
                toastr.error(res.displayMessage || "Error al actualizar estado");
            }
        });
    });



    // =============================
    // 📌 EVENTOS PREGUNTAS
    // =============================

    // Crear Normal
    $("#btnCrearNormal").click(function () {
        limpiarModal();
        $("#campoIndicador").hide();
        $("#campoMulti").show();
        $("#modalPregunta").modal("show").data("tipo", "NORMAL");
    });

    // Crear Atributo
    $("#btnCrearAtributo").click(function () {
        limpiarModal();
        $("#campoIndicador").show();
        $("#campoMulti").hide(); // siempre false en atributo

        $("#modalPregunta").modal("show").data("tipo", "ATRIBUTO");
        $("#startDate").val("")
        $("#endDate").val("")
        $("#campoFechas").show();
    });

   


    // Editar CSAT
    // Editar CSAT
    $("#btnEditarCSAT").click(function () {
        let pregunta = preguntas.find(p => p.Indicador === "CSAT"); // tu JSON ya trae "Indicador"
        if (!pregunta) return toastr.error("No existe pregunta CSAT");

        $("#modalPregunta").data("tipo", "CSAT");
        $("#modalPregunta").data("indicador", pregunta.Indicador); // 👈 guardo el indicador en data
        $("#tituloModalPregunta").text("Editar Pregunta CSAT");
        $("#idPregunta").val(pregunta.IdPregunta);
        $("#textoPregunta").val(pregunta.Texto);
        $("#campoIndicador").hide();
        $("#multiPregunta").prop("checked", false).closest(".form-group").hide();
        $("#modalPregunta").modal("show");
    });


    // Editar NPS
    $("#btnEditarNPS").click(function () {
        let pregunta = preguntas.find(p => p.Indicador === "NPS"); // buscamos la de NPS
        if (!pregunta) return toastr.error("No existe pregunta NPS");

        $("#modalPregunta").data("tipo", "NPS");
        $("#modalPregunta").data("indicador", pregunta.Indicador); // 👈 guardo el indicador
        $("#tituloModalPregunta").text("Editar Pregunta NPS");
        $("#idPregunta").val(pregunta.IdPregunta);
        $("#textoPregunta").val(pregunta.Texto);

        // Ocultamos campos que no aplican
        $("#campoIndicador").hide();
        $("#multiPregunta").prop("checked", false).closest(".form-group").hide();

        $("#modalPregunta").modal("show");
    });



    // Editar Pregunta (Normales / Atributos)
    $(document).on("click", ".btnEditarPregunta", function () {
        let id = $(this).closest(".cardPregunta").data("id");
        let pregunta = preguntas.find(p => p.IdPregunta === id);

        $("#idPregunta").val(pregunta.IdPregunta);
        $("#textoPregunta").val(pregunta.Texto);
        $("#indicadorPregunta").val(pregunta.Indicador || "");
        $("#multiPregunta").prop("checked", pregunta.Multi);

        $("#campoIndicador").toggle(pregunta.Random);
        $("#campoMulti").toggle(!pregunta.Random);

        // 🔹 Mostrar fechas solo si es ATRIBUTO
        if (pregunta.Random) {
            $("#campoFechas").show();
           
            let fechaInicio = parseNetDate(pregunta.FechaInicio);
            $("#startDate").val(fechaInicio ? fechaInicio.format("DD/MM/YYYY") : "");
            let fechaFinal = parseNetDate(pregunta.FechaFin);

            $("#endDate").val(fechaFinal ? fechaFinal.format("DD/MM/YYYY") : "");
        } else {
            $("#campoFechas").hide();
            $("#startDate").val("");
            $("#endDate").val("");
        }

        $("#modalPregunta").modal("show").data("tipo", "EDITAR");
    });

    $("#indicadorPregunta").on("input", function () {
        this.value = this.value.replace(/[0-9]/g, "");
    });

    // Guardar Pregunta
    $("#formPregunta").submit(function (e) {
        e.preventDefault();
        let tipo = $("#modalPregunta").data("tipo");

        let indicador = $("#modalPregunta").data("indicador") || "";

        let data = {
            IdPregunta: $("#idPregunta").val(),
            Texto: $("#textoPregunta").val(),
            Multi: $("#multiPregunta").is(":checked"),
            Activo: true,
            Indicador: indicador, // 👈 mantenemos el valor de CSAT o NPS,
            IdTipoEncuesta:1
        };

        console.log(tipo)
        let url = "";

        if (tipo === "NORMAL") {
            let maxOrden = Math.max(...preguntas.map(p => p.Orden), 0);
            url = basePath + "/ConfigClienteSatisfaccion/CrearPreguntaNormal";
            data.Multi = $("#multiPregunta").is(":checked");
            data.Indicador = "";
            data.Random = false;
            data.Activo = true;
            
            data.Orden = maxOrden + 1;



        } else if (tipo === "ATRIBUTO") {
            let maxOrden = Math.max(...preguntas.map(p => p.Orden), 0);
            url = basePath + "/ConfigClienteSatisfaccion/CrearPreguntaAtributo";
            data.Indicador = $("#indicadorPregunta").val();
            data.Multi = false;
            data.Orden = maxOrden + 1;
            data.Random = true;
            data.Activo = true;
            const fechaInicio = $("#startDate").val();
            const fechaFin = $("#endDate").val();
            const indicador = $("#indicadorPregunta").val();
            if (!indicador) {
                toastr.error("Debe ingresar codigo de la pregunta");
                return;
            }
            if (!fechaInicio || !fechaFin) {
                toastr.error("Debe ingresar fecha de inicio y fecha de fin para atributos");
                return; // 🚫 detener el submit
            }

        } else if (tipo === "CSAT" || tipo === "NPS") {
            // 🚫 No mandamos Indicador ni Multi, solo Texto
            url = basePath + "/ConfigClienteSatisfaccion/EditarPregunta";

        } else if (tipo === "EDITAR") {
            url = basePath + "/ConfigClienteSatisfaccion/EditarPregunta";
            data.Multi = $("#multiPregunta").is(":checked");
            data.Indicador = $("#campoIndicador").is(":visible")
                ? $("#indicadorPregunta").val()
                : "";

            if ($("#campoFechas").is(":visible")) {
                data.FechaInicio = $("#startDate").val() || null;
                data.FechaFin = $("#endDate").val() || null;
            }
        }

        $.post(url, data, function (res) {
            if (res.success) {
                $("#modalPregunta").modal("hide");
                toastr.success("Exito en la operación" );
                cargarPreguntas();
            } else {
                toastr.error(res.displayMessage || "Error al guardar");
            }
        });
    });


    // Eliminar Pregunta
    $(document).on("click", ".btnEliminarPregunta", function () {
        if (!confirm("¿Eliminar esta pregunta?")) return;
        let id = $(this).closest(".cardPregunta").data("id");

        $.post(basePath + "/ConfigClienteSatisfaccion/EliminarPregunta", { idPregunta: id, tipo: "NORMAL" }, function (res) {
            if (res.success) {
                toastr.success("Se eliminó la pregunta.");
                cargarPreguntas();
            }
            else {
                toastr.error(res.displayMessage);
                }
        });
    });

    // =============================
    // 📌 EVENTOS OPCIONES
    // =============================

    // Crear Opción
    $(document).on("click", ".btnAgregarOpcion", function () {
        let idPregunta = $(this).closest(".cardPregunta").data("id");
        $("#idOpcion").val("");
        $("#idPreguntaOpcion").val(idPregunta);
        $("#textoOpcion").val("");
        $("#tieneComentarioOpcion").prop("checked", false);
        $("#modalOpcion").modal("show").data("modo", "CREAR");
    });

    // Editar Opción
    $(document).on("click", ".btnEditarOpcion", function () {
        let idOpcion = $(this).closest("li").data("id");
        let preguntaId = $(this).closest(".cardPregunta").data("id");
        let opcion = preguntas.find(p => p.IdPregunta === preguntaId)
            .Opciones.find(o => o.idOpcion === idOpcion);

        $("#idOpcion").val(opcion.idOpcion);
        $("#idPreguntaOpcion").val(opcion.idPregunta);
        $("#textoOpcion").val(opcion.Texto);
        $("#tieneComentarioOpcion").prop("checked", opcion.TieneComentario);

        $("#modalOpcion").modal("show").data("modo", "EDITAR");
    });

    // Guardar Opción
    $("#formOpcion").submit(function (e) {
        e.preventDefault();
        let data = {
            IdOpcion: $("#idOpcion").val(),
            IdPregunta: $("#idPreguntaOpcion").val(),
            Texto: $("#textoOpcion").val(),
            TieneComentario: $("#tieneComentarioOpcion").is(":checked")
        };

        let url = $("#modalOpcion").data("modo") === "CREAR"
            ? basePath + "/ConfigClienteSatisfaccion/CrearOpcion"
            : basePath + "/ConfigClienteSatisfaccion/EditarOpcion";

        $.post(url, data, function (res) {
            if (res.success) {
                $("#modalOpcion").modal("hide");
                toastr.success("Operación exitosa.");
                cargarPreguntas();
            } else {
                toastr.error(res.displayMessage || "Error en opción");
            }
        });
    });

    // Eliminar Opción
    $(document).on("click", ".btnEliminarOpcion", function () {
        if (!confirm("¿Eliminar esta opción?")) return;

        let idOpcion = $(this).closest("li").data("id"); // 🔹 va al <li> más cercano
        console.log(idOpcion);

        $.post(basePath+"/ConfigClienteSatisfaccion/EliminarOpcion", { idOpcion: idOpcion }, function (res) {
            if (res.success) {
                cargarPreguntas();
                toastr.success("Se eliminó la opción correctamente.");

            } 
            else {
                toastr.error(res.displayMessage);

            }
        });
    });

    // =============================
    // 📌 UTILS
    // =============================
    function limpiarModal() {
        $("#idPregunta").val("");
        $("#textoPregunta").val("");
        $("#indicadorPregunta").val("");
        $("#multiPregunta").prop("checked", false).prop("disabled", false);
        $("#campoIndicador, #campoMulti").hide();
    }

    // Inicial
    cargarPreguntas();
});
function parseNetDate(netDate) {
    if (!netDate) return null;
    const match = /\/Date\((\-?\d+)\)\//.exec(netDate);
    if (!match) return null;
    return moment(parseInt(match[1]));
}
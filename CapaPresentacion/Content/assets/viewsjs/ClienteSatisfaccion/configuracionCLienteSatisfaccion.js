$(document).ready(function () {
    const basePath = $("#BasePath").val() || "";

    // === 1. Evento: al cambiar de sala, cargar configuraciones ===
    $("#cboSala").on("change", function () {
        const idSala = $(this).val();

        cargarConfiguraciones(idSala);
    });


     obtenerListaSalas().then(result => {
        const lista = (result && result.data) ? result.data : [];
        renderSelectSalas(lista);

        const salaId = $("#cboSala").val();
        if (!salaId) return;
         cargarConfiguraciones(salaId);
       
    });





    // === 2. Cargar configuraciones por sala ===
    function cargarConfiguraciones(idSala) {
        $.ajax({
            type: "POST",
            url: basePath + "/ConfigClienteSatisfaccion/ListadoConfiguraciones",
            data: JSON.stringify({ idSala }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: () => $.LoadingOverlay("show"),
            complete: () => $.LoadingOverlay("hide"),
            success: function (res) {
                if (res.success) {
                    renderConfiguraciones(res.data, idSala);
                } else {
                    toastr.warning(res.displayMessage, "Configuraciones");
                }
            },
            error: function (xhr, status, error) {
                toastr.error("Error al cargar configuraciones: " + error, "Servidor");
            }
        });
    }


    //SALAS
    function obtenerListaSalas() {
        return $.ajax({
            type: "POST", url: basePath + "Sala/ListadoSalaPorUsuarioJson",
            cache: false, contentType: "application/json; charset=utf-8", dataType: "json",
            beforeSend: () => $.LoadingOverlay && $.LoadingOverlay("show"),
            complete: () => $.LoadingOverlay && $.LoadingOverlay("hide")
        });
    }
    // Renderizar Slas combo
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

    // === 3. Renderizar configuraciones ===
    function renderConfiguraciones(lista, idSala) {
        const $container = $("#preguntas-container");
        $container.empty();

        if (!lista || lista.length === 0) {
            $container.html(`
                <div class="alert alert-warning">
                    No hay configuraciones registradas para esta sala.
                </div>
            `);
            return;
        }

        let html = `
            <table class="table table-bordered table-striped align-middle">
                <thead>
                    <tr>
                        <th>Configuración</th>
                        <th class="text-center">Activo</th>
                        <th class="text-center">Acción</th>
                    </tr>
                </thead>
                <tbody>`;

        lista.forEach(item => {
            html += `
                <tr data-id="${item.IdConfiguracion}">
                    <td>${item.ClaveConfig}</td>
                    <td class="text-center">
                        <input type="checkbox" class="toggle-valor form-check-input" ${item.ValorBit ? "checked" : ""}>
                    </td>
                    <td class="text-center">
                        <button 
                            class="btn btn-primary btn-sm btn-guardar" 
                            data-id="${item.IdConfiguracion}" 
                            data-sala="${idSala}">
                            Guardar
                        </button>
                    </td>
                </tr>`;
        });

        html += `</tbody></table>`;
        $container.html(html);
    }

    // === 4. Guardar valor (insert/update) ===
    $(document).on("click", ".btn-guardar", function () {
        const idConfiguracion = $(this).data("id");
        const idSala = $(this).data("sala");
        const $row = $(this).closest("tr");
        const nuevoValor = $row.find(".toggle-valor").is(":checked");

        $.ajax({
            type: "POST",
            url: basePath + "/ConfigClienteSatisfaccion/ActualizarValorBit",
            data: JSON.stringify({ idConfiguracion, nuevoValor, idSala }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: () => $.LoadingOverlay("show"),
            complete: () => $.LoadingOverlay("hide"),
            success: function (res) {
                if (res.success) {
                    toastr.success(res.displayMessage, "Configuración");
                } else {
                    toastr.warning(res.displayMessage, "Configuración");
                }
            },
            error: function (xhr, status, error) {
                toastr.error("Error al actualizar: " + error, "Servidor");
            }
        });
    });

    // === 5. Al inicio no se hace ninguna petición ===
    // Solo se mostrará un mensaje inicial
    $("#preguntas-container").html(`
        <div class="alert alert-info">
            Selecciona una sala para cargar sus configuraciones.
        </div>
    `);
});

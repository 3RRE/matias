$(document).ready(function () {
    let tablets = [];
    let salaSeleccionada = null;

    // =============================
    //  OBTENER LISTA SALAS
    // =============================
    function obtenerListaSalas() {
        return $.ajax({
            type: "POST",
            url: basePath + "Sala/ListadoSalaPorUsuarioJson",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: () => $.LoadingOverlay && $.LoadingOverlay("show"),
            complete: () => $.LoadingOverlay && $.LoadingOverlay("hide")
        });
    }

    function renderSelectSalas(data) {
        const $select = $("#cboSala");
        $select.empty();

        if (Array.isArray(data) && data.length) {
            $select.append(`<option></option>`);

            data.forEach(s =>
                $select.append(`<option value="${s.CodSala}">${s.Nombre}</option>`)
            );

            $select.select2({
                placeholder: "--Seleccione--",
                allowClear: true,
                width: "100%"
            });

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



    // =============================
    //  CARGAR TABLETS
    // =============================
    function cargarTablets() {
        if (!salaSeleccionada) return;

        $.ajax({
            type: "POST",
            url: basePath + "/ConfigClienteSatisfaccion/ObtenerTabletsSala",
            data: JSON.stringify({ salaId: salaSeleccionada }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: () => $.LoadingOverlay && $.LoadingOverlay("show"),
            complete: () => $.LoadingOverlay && $.LoadingOverlay("hide"),
            success: function (res) {
                if (res.success) {
                    tablets = res.data;
                    toastr.success("Se obtuvieron tablets")
                    renderTablets();
                } else {
                    toastr.warning("Error al cargar tablets")

                }
            },
            error: err => console.error("Error en tablets", err)
        });
    }

    // =============================
    //  RENDER TABLE
    // =============================
    function renderTablets() {
        $("#tablaTablets").empty();
        tablets.forEach(t => {
            $("#tablaTablets").append(`
            <tr data-id="${t.IdTablet}">
                <td>${t.IdTablet}</td>
                <td>${t.Nombre}</td>
                <td>
                    <span class="label ${t.Activa ? "label-success" : "label-danger"}">
                        ${t.Activa ? "Activo" : "Inactivo"}
                    </span>
                </td>
                <td>
                    <button class="btn btn-xs btn-info btnEditarTablet">Editar</button>
                    <button class="btn btn-xs btn-primary btnVerUrl">Ver URL</button>
                </td>
            </tr>
        `);
        });
    }

    $(document).on("click", ".btnVerUrl", function () {
        let id = $(this).closest("tr").data("id");
        let url = `${basePath}ClienteSatisfaccion/CSATView?salaid=${salaSeleccionada}&idtablet=${id}`;

        $("#urlTabletTexto").val(url); 
        $("#urlTabletLink").attr("href", url); 

        $("#modalUrlTablet").modal("show");
    });
    // =============================
    // NUEVA TABLET
    // =============================
    $("#btnNuevaTablet").click(function () {
        limpiarModal();
        $("#tituloModalTablet").text("Nueva Tablet");
        $("#modalTablet").modal("show").data("modo", "CREAR");
    });

    // =============================
    // EDITAR TABLET
    // =============================
    $(document).on("click", ".btnEditarTablet", function () {
        let id = $(this).closest("tr").data("id");
        let tablet = tablets.find(t => t.IdTablet === id);

        $("#idTablet").val(tablet.IdTablet);
        $("#nombreTablet").val(tablet.Nombre);
        $("#activoTablet").prop("checked", tablet.Activa);

        $("#tituloModalTablet").text("Editar Tablet");
        $("#modalTablet").modal("show").data("modo", "EDITAR");
    });

    // =============================
    //  GUARDAR (CREAR / EDITAR)
    // =============================
    $("#formTablet").submit(function (e) {
        e.preventDefault();

        let modo = $("#modalTablet").data("modo");
        let data = {
            idTablet: $("#idTablet").val(),
            nombre: $("#nombreTablet").val(),
            activa: $("#activoTablet").is(":checked"),
            salaId: salaSeleccionada
        };

        let url = modo === "CREAR"
            ? basePath + "/ConfigClienteSatisfaccion/CrearTablet"
            : basePath + "/ConfigClienteSatisfaccion/EditarTablet";
        
        if (salaSeleccionada == null) {
            toastr.error("Debe seleccionar una sala")
            return
        }
        $.ajax({
            type: "POST",
            url: url,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (res) {
                if (res.success) {
                    $("#modalTablet").modal("hide");
                    toastr.success(res.displayMessage)
                    cargarTablets();

                } else {
                    toastr.error(res.displayMessage)
                    //alert(res.displayMessage || "Error al guardar");
                }
            }
        });
    });

    // =============================
    //  UTILS
    // =============================
    function limpiarModal() {
        $("#idTablet").val("");
        $("#nombreTablet").val("");
        $("#activoTablet").prop("checked", true);
    }

    // =============================
    //  INIT
    // =============================
    obtenerListaSalas().then(result => {
        const lista = (result && result.data) ? result.data : [];
        renderSelectSalas(lista);
    });

    $("#cboSala").change(function () {
        salaSeleccionada = $(this).val();
        cargarTablets();
    });
});

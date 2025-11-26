let listaGlobalJefes = [];
$(document).ready(function () {
    cargarSalas();
    $.getJSON(basePath + "/Content/assets/nps/country.json", function (json) {
        renderSelectPaises(json);
    });
    $("#cboSala").change(function () {
        const idSala = Number($(this).val());
        if (idSala > 0) {
            listarJefesSala(idSala);
        } else {    
            $("#tableJefes").empty();
        }
    });

    $("#btnNuevoJefe").click(function () {
        const salaSeleccionada = Number($("#cboSala").val());

        if (salaSeleccionada === 0) {
            toastr.warning("Seleccione una sala antes de crear un nuevo jefe.");
            return;
        }

        limpiarModal();
        $("#titleModal").text("Nuevo Jefe de Sala");
        $("#modalJefeSala").modal("show");
    });

    $("#btnGuardar").click(function () {
        guardarJefeSala();
    });

});


function listarJefesSala(idSala) {
    $.ajax({
        type: "POST",
        url: basePath + "/ConfigClienteSatisfaccion/ListarJefesSala",
        data: JSON.stringify({ idSala }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            renderListadoJefes(res.lista);
            listaGlobalJefes = res.lista;
            if (!res.success) {
                toastr.warning(res.displayMessage);
            }
        },
        error: function (err) {
            toastr.error("Error: " + res.displayMessage);
        },
        beforeSend: () => $.LoadingOverlay && $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay && $.LoadingOverlay("hide")
    });
}


function renderListadoJefes(lista) {

    const $tbody = $("#tableJefes");
    $tbody.empty();

    if (!lista || lista.length === 0) {
        $tbody.append(`
            <tr>
                <td colspan="7" class="text-center text-danger">
                    No hay jefes de sala registrados
                </td>
            </tr>
        `);
        return;
    }

    lista.forEach(jefe => {

        $tbody.append(`
            <tr>
                <td>${jefe.IdJefeSala}</td>
                <td>${jefe.Nombres}</td>
                <td>${jefe.Apellidos}</td>
                <td>${jefe.Codigo}</td>
                <td>${jefe.Celular}</td>
                <td>${ moment(jefe.FechaCreacion).format("DD/MM/YYYY HH:mm a") }</td>


                <td>
                    <button class="btn btn-sm btn-warning" onclick="abrirEditar(${jefe.IdJefeSala})">
                        Editar
                    </button>
                    <button class="btn btn-sm btn-danger" onclick="eliminarJefe(${jefe.IdJefeSala})">
                        Eliminar
                    </button>
                </td>
            </tr>
        `);
    });
}


function cargarSalas() {
    $.ajax({
        type: "POST",
        url: basePath + "/Sala/ListadoSalaPorUsuarioJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            const $cboSala = $("#cboSala");
            $cboSala.empty().append(`<option value="0">Seleccione</option>`);

            if (res.data.length > 0 ) {
                res.data.forEach(s => {
                    const option = `<option value="${s.CodSala}">${s.Nombre}</option>`;
                    $cboSala.append(option);
                });
            }
        },
        beforeSend: () => $.LoadingOverlay && $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay && $.LoadingOverlay("hide")
    });
}


function abrirEditar(id) {

    const jefe = listaGlobalJefes.find(x => x.IdJefeSala === id);

    if (!jefe) {
        toastr.error("No se encontró el jefe seleccionado.");
        return;
    }

    $("#IdJefeSala").val(jefe.IdJefeSala);
    $("#Nombres").val(jefe.Nombres);
    $("#Apellidos").val(jefe.Apellidos);
    $("#cboPais").val(jefe.Codigo).trigger("change");
    $("#Celular").val(jefe.Celular);

    $("#titleModal").text("Editar Jefe de Sala");
    $("#modalJefeSala").modal("show");
}


function guardarJefeSala() {
    const salaSeleccionada = Number($("#cboSala").val());

    if (salaSeleccionada === 0) {
        toastr.warning("Seleccione una sala antes de guardar.");
        return;
    }
    if (!validarFormulario()) {
        toastr.error("Complete los campos correctamente.");
        return;
    }
    const jefeSala = {
        IdJefeSala: $("#IdJefeSala").val(),
        Nombres: $("#Nombres").val(),
        Apellidos: $("#Apellidos").val(),
        Codigo: $("#cboPais").val(),
        Celular: $("#Celular").val(),
        SalaId: salaSeleccionada 
    };

    const url = jefeSala.IdJefeSala > 0
        ? "/ConfigClienteSatisfaccion/EditarJefeSala"
        : "/ConfigClienteSatisfaccion/CrearJefeSala";

    $.ajax({
        type: "POST",
        url: basePath + url,
        data: JSON.stringify(jefeSala),
        contentType: "application/json",
        success: function (res) {
            if (res.success) {
                toastr.success(res.displayMessage);
                $("#modalJefeSala").modal("hide");
                listarJefesSala($("#cboSala").val());
            } else {
                toastr.warning(res.displayMessage);
            }
        }
    });
}

function confimarEliminarJefe() {
    $.confirm({
        title: `Hola`,
        content: `¿Estás seguro de asignar todos los permisos para el rol de <b>hola</b>?`,
        confirmButton: 'Sí, asignar',
        cancelButton: 'Cancelar',
        confirmButtonClass: 'btn-success',
        confirm: function () {
        },
        cancel: function () {
        }
    })
}
function eliminarJefe(id) {

    $.confirm({
        title: `Advertencia`,
        content: `¿Esta seguro que desea eliminar este registro?`,
        confirmButton: 'Sí',
        cancelButton: 'No',
        confirmButtonClass: 'btn-danger',
        confirm: function () {
            $.ajax({
                type: "POST",
                url: basePath + "/ConfigClienteSatisfaccion/EliminarJefeSala",
                data: JSON.stringify({ jefeSala: id }),
                contentType: "application/json",
                success: function (res) {
                    if (res.success) {
                        toastr.success(res.displayMessage);
                        listarJefesSala($("#cboSala").val());
                    } else {
                        toastr.error(res.displayMessage);
                    }
                },

            });
        },
        cancel: function () {
        }
    })

    
}


function limpiarModal() {
    $("#IdJefeSala").val("");
    $("#Nombres").val("");
    $("#Apellidos").val("");
    $("#Correo").val("");
}
function renderSelectPaises(data) {
    const $select = $("#cboPais");
    $select.empty();

    if (Array.isArray(data) && data.length) {

        $select.append(`<option></option>`);

        data.forEach(p => {
            $select.append(`
                <option value="${p.CodigoTelefonico}" data-flag="${p.Bandera}">
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
            dropdownParent: $("#modalJefeSala"),
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


function setError(id, message) {
    const $group = $("#" + id);
    $group.removeClass("has-success").addClass("has-error");
    $group.find(".help-block").remove();
    $group.append(`<span class="help-block">${message}</span>`);
}

function setSuccess(id) {
    const $group = $("#" + id);
    $group.removeClass("has-error").addClass("has-success");
    $group.find(".help-block").remove();
}

function setErrorSelect2(id) {
    $("#" + id).next(".select2").find(".select2-selection")
        .css("border", "1px solid #a94442"); // rojo
}

function setSuccessSelect2(id) {
    $("#" + id).next(".select2").find(".select2-selection")
        .css("border", "1px solid #3c763d"); // verde
}

function validarFormulario() {
    let valido = true;

    const nombres = $("#Nombres").val().trim();
    const apellidos = $("#Apellidos").val().trim();
    const codigo = $("#cboPais").val();
    const celular = $("#Celular").val().trim();

    // Nombres
    if (nombres.length < 2) {
        setError("groupNombres", "Ingrese un nombre válido.");
        valido = false;
    } else {
        setSuccess("groupNombres");
    }

    // Apellidos
    if (apellidos.length < 2) {
        setError("groupApellidos", "Ingrese un apellido válido.");
        valido = false;
    } else {
        setSuccess("groupApellidos");
    }

    // Código país
    if (!codigo) {
        setError("groupCodigo", "Seleccione un código.");
        setErrorSelect2("cboPais");
        valido = false;
    } else {
        setSuccess("groupCodigo");
        setSuccessSelect2("cboPais");
    }

    // Celular
    if (celular.length < 9 || !/^[0-9]+$/.test(celular)) {
        setError("groupCelular", "Ingrese un celular válido.");
        valido = false;
    } else {
        setSuccess("groupCelular");
    }

    return valido;
}

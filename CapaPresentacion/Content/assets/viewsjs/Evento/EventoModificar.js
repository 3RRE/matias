$(document).ready(function () {

    CargarDatosFormulario(evento);

    // El ID del formulario ya coincide ("frmModificarEvento")
    $("#frmModificarEvento").bootstrapValidator({
        container: '#messages',
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            Titulo: { validators: { notEmpty: { message: 'El título es obligatorio' } } },
            CodSala: { validators: { notEmpty: { message: 'Debe seleccionar una sala' } } }, // Valida por el "name"
            FechaEvento: { validators: { notEmpty: { message: 'La fecha del evento es obligatoria' } } }
        }
    }).on('success.field.bv', function (e, data) {
        e.preventDefault();
        let $parent = data.element.parents('.form-group');
        $parent.removeClass('has-success');
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });

    // --- Helpers de Input ---
    $(document).on('keypress', '.soloNumeros', function (event) {
        let regex = new RegExp("^[0-9]+$");
        let key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) { event.preventDefault(); return false; }
    });
    $(document).on('keypress', '.UpperCase', function (event) {
        $input = $(this);
        setTimeout(function () { $input.val($input.val().toUpperCase()); }, 0);
    });

    // --- Botones de Acción ---
    $('.btnListar').on('click', function (e) {
        window.location.replace(basePath + "Publicidad/EventoVista");
    });

    $(document).on('click', '.btnGuardar', function (e) {
        e.preventDefault();

        // Ahora el validador SÍ está adjunto al formulario
        let validar = $("#frmModificarEvento").data('bootstrapValidator').validate();
        if (validar.isValid()) {

            let file = $('#RutaImagen')[0].files[0];
            if (file) {
                let permitido = 5242880; // 5MB
                if (file.size > permitido) {
                    toastr.error("La imagen no debe pesar más de 5MB", "Mensaje Servidor");
                    return;
                }
            }

            let dataForm = new FormData(document.getElementById("frmModificarEvento"));

            $.ajax({
                url: basePath + "Publicidad/ModificarEventoJson",
                type: "POST",
                method: "POST",
                contentType: false,
                data: dataForm,
                cache: false,
                processData: false,
                beforeSend: function () { $.LoadingOverlay("show"); },
                complete: function () { $.LoadingOverlay("hide"); },
                success: function (response) {
                    if (response.respuesta) {
                        $.confirm({
                            icon: 'fa fa-check',
                            title: 'Mensaje Servidor',
                            theme: 'black',
                            animationBounce: 1.5,
                            content: response.mensaje,
                            confirmButtonClass: 'btn-info',
                            cancelButtonClass: 'btn-danger',
                            confirmButton: 'Ir a Listado',
                            cancelButton: 'Cancelar',
                            confirm: function () {
                                let url = basePath + "Publicidad/EventoVista";
                                window.location.href = url;
                            }
                        });
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    toastr.error("Error en la solicitud", "Mensaje Servidor");
                }
            });
        }
    });
});

function CargarDatosFormulario(data) {
    // Formatear fecha para input type="datetime-local" (YYYY-MM-DDThh:mm)
    var fechaEvento = '';
    if (data.FechaEvento) {
        var d = new Date(parseInt(data.FechaEvento.substr(6)));
        d.setMinutes(d.getMinutes() - d.getTimezoneOffset()); // Ajuste de zona horaria
        fechaEvento = d.toISOString().slice(0, 16);
    }

    $("#IdEvento").val(data.IdEvento);
    $("#RutaArchivoLogoAnt").val(data.RutaImagen);
    $("#Titulo").val(data.Titulo);
    $("#Descripcion").val(data.Descripcion);
    $("#UrlDireccion").val(data.UrlDireccion);

    // Carga las salas
    if (typeof llenarSelect === "function") {
        llenarSelect(basePath + "Sala/ListadoSalaPorUsuarioJson", {}, "cboSala", "CodSala", "Nombre", data.CodSala);
    } else {
        ObtenerListaSalasParaEditar(data.CodSala);
    }

    // Inicializa Select2
    $("#cboSala").select2({
        multiple: false, placeholder: "--Seleccione--", allowClear: true
    });

    // ****** INICIO DE CORRECCIÓN ******
    // Se eliminó la línea que borraba el valor de la sala
    // $("#cboSala").val(null).trigger("change"); // <-- ESTA LÍNEA ES INCORRECTA
    // ****** FIN DE CORRECCIÓN ******

    $("#FechaEvento").val(fechaEvento);
    $("#Estado").val(data.Estado).trigger('change'); // Asegúrate de que el select de Estado exista

    var urlImagen = data.UrlImagenCompleta;
    urlImagen += "?v=" + new Date().getTime();
    $("#imgActual").attr("src", urlImagen);
}

function ObtenerListaSalasParaEditar(codSalaSeleccionada) {
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var datos = result.data;
            $("#cboSala").empty().append('<option value="">--Seleccione--</option>'); // Limpia el select
            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            // Asigna el valor DESPUÉS de poblar el select
            $("#cboSala").val(codSalaSeleccionada).trigger("change");
        },
        error: function () {
            toastr.error("Error al cargar salas", "Mensaje Servidor");
        }
    });
}
$(document).ready(function () {


    // Cargar Datos Iniciales
    CargarDatosFormulario(publicidad);

    // Validadores
    $("#frmModificarPublicidad").bootstrapValidator({
        container: '#messages',
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            Titulo: { validators: { notEmpty: { message: 'El título es obligatorio' } } },
            CodSala: { validators: { notEmpty: { message: 'Debe seleccionar una sala' } } },
            // RutaImagen no es obligatoria al modificar
            FechaInicio: { validators: { notEmpty: { message: 'La fecha de inicio es obligatoria' } } },
            FechaFin: { validators: { notEmpty: { message: 'La fecha de fin es obligatoria' } } },
            Orden: { validators: { notEmpty: { message: 'El orden es obligatorio' } } }
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
        window.location.replace(basePath + "Publicidad/PublicidadVista");
    });

    $(document).on('click', '.btnGuardar', function (e) {
        e.preventDefault();

        let validar = $("#frmModificarPublicidad").data('bootstrapValidator').validate();
        if (validar.isValid()) {

            let file = $('#RutaImagen')[0].files[0];
            if (file) {
                // Si hay archivo, validar tamaño
                let permitido = 5242880; // 5MB
                if (file.size > permitido) {
                    toastr.error("La imagen no debe pesar más de 5MB", "Mensaje Servidor");
                    return;
                }
            }

            let dataForm = new FormData(document.getElementById("frmModificarPublicidad"));

            $.ajax({
                url: basePath + "Publicidad/ModificarPublicidadJson",
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
                                let url = basePath + "Publicidad/PublicidadVista";
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
    // Formatear fechas para input type="date" (YYYY-MM-DD)
    var fechaInicio = data.FechaInicio ? new Date(parseInt(data.FechaInicio.substr(6))).toISOString().split('T')[0] : '';
    var fechaFin = data.FechaFin ? new Date(parseInt(data.FechaFin.substr(6))).toISOString().split('T')[0] : '';

    $("#IdPublicidad").val(data.IdPublicidad);
    $("#RutaArchivoLogoAnt").val(data.RutaImagen); // Guardamos la imagen antigua
    $("#Titulo").val(data.Titulo);
    llenarSelect(basePath + "Sala/ListadoSalaPorUsuarioJson", {}, "cboSala", "CodSala", "Nombre", data.CodSala);
    $("#cboSala").select2({
        multiple: false, placeholder: "--Seleccione--", allowClear: true
    });
    $("#cboSala").val(null).trigger("change");
    $("#FechaInicio").val(fechaInicio);
    $("#FechaFin").val(fechaFin);
    $("#Orden").val(data.Orden);
    $("#UrlEnlace").val(data.UrlEnlace);
    $("#Estado").val(data.Estado).trigger('change');

    var urlImagen = data.UrlImagenCompleta;

    // 2. Agregamos un "cache-buster" para evitar que el navegador muestre una imagen vieja
    urlImagen += "?v=" + new Date().getTime();

    // 3. Asignamos la URL al <img> tag que creamos en la vista
    $("#imgActual").attr("src", urlImagen);
}
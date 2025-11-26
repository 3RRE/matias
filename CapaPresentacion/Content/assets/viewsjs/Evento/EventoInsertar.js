
$(document).ready(function () {
    ObtenerListaSalas();

    // Validadores
    $("#frmRegistroEvento").bootstrapValidator({
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
            RutaImagen: { validators: { notEmpty: { message: 'La imagen es obligatoria' } } },
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

        let validar = $("#frmRegistroEvento").data('bootstrapValidator').validate();
        if (validar.isValid()) {

            let file = $('#RutaImagen')[0].files[0];
            if (!file) {
                toastr.warning("Seleccione una imagen para el evento", "Mensaje Servidor");
                return;
            }
            let permitido = 5242880; // 5MB
            if (file.size > permitido) {
                toastr.error("La imagen no debe pesar más de 5MB", "Mensaje Servidor");
                return;
            }

            let dataForm = new FormData(document.getElementById("frmRegistroEvento"));

            $.ajax({
                url: basePath + "Publicidad/InsertarEventoJson", // Apunta al controller de Publicidad
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
                        $("form input,select,textarea").val("");
                        $("#CodSala").val(null).trigger("change");

                        $.confirm({
                            icon: 'fa fa-check',
                            title: 'Mensaje Servidor',
                            theme: 'black',
                            animationBounce: 1.5,
                            content: response.mensaje,
                            confirmButtonClass: 'btn-info',
                            cancelButtonClass: 'btn-warning',
                            confirmButton: 'Ir a Listado',
                            cancelButton: 'Seguir Registrando',
                            confirm: function () {
                                let url = basePath + "Publicidad/EventoVista";
                                window.location.href = url;
                            },
                            cancel: function () {
                                $("#frmRegistroEvento").data('bootstrapValidator').resetForm(true);
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

function ObtenerListaSalas() {
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboSala").val(null).trigger("change");
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
    return false;
}
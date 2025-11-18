
$(document).ready(function () {
    localStorage.removeItem("moduloActivo");
    $(".login-form")
        .bootstrapValidator({
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                contrasena: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese su Contraseña, Obligatorio'
                        }
                    }
                }
            }
        })
        .on('success.field.bv', function (e, data) {
            var $parent = data.element.parents('.form-group');
            // Remove the has-success class
            $parent.removeClass('has-success');
            // Hide the success icon
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
        });

    $('.btnCambiarContrasena').on('click', function (e) {
        $(".login-form").data('bootstrapValidator').resetForm();
        var validar = $(".login-form").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var contrasena = $("#contrasena").val();

            var url = basePath + "Usuario/CambiarContrasena";
            var data = { usuPassword: contrasena };
            DataPostSend(url, data, true).done(function (response) {
                if (response) {
                    var data = response.respuesta;
                    if (data) {
                        toastr.success("Contraseña actualizada.", "Mensaje Servidor");
                        var redirectUrl = basePath + "Home/Login";
                        setTimeout(function () {
                            window.location.replace(redirectUrl);
                        }, 1500);
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                } else {
                    toastr.error("Error no se logro conectar al servidor", "Mensaje Servidor");
                }
            }).fail(function (x) {
                toastr.error("Error Fallo Servidor", "Mensaje Servidor");
            });


        }

    });
    $('#contrasena').focus();
    $(".login-form input").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            $('.btnCambiarContrasena').click();
        }

    });
});
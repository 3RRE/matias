
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
                cuenta: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese su Cuenta, Obligatorio'
                        }
                    }
                },
                email: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese su Correo Electronico, Obligatorio'
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

    $('.btnRestablecerContrasena').on('click', function (e) {
        $(".login-form").data('bootstrapValidator').resetForm();
        var validar = $(".login-form").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var cuenta = $("#cuenta").val();
            var email = $("#email").val();

            var url = basePath + "Usuario/RestablecerContrasena";
            var data = { cuenta: cuenta, email: email };
            DataPostSend(url, data, true).done(function (response) {
                if (response) {
                    var data = response.respuesta;
                    if (data) {
                        toastr.success("Se restableció la contraseña,Revise su correo electronico.", "Mensaje Servidor");
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
    $('#cuenta').focus();
    $(".login-form input").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            $('.btnRestablecerContrasena').click();
        }

    });
});
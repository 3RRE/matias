$(document).ready(function () {

    $(".login-form")
        .bootstrapValidator({
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                usuario: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese su Usuario, Obligatorio'
                        }
                    }
                },
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

    $('.btnIngresar').on('click', function (e) {
        $(".login-form").data('bootstrapValidator').resetForm();
        var validar = $(".login-form").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var username = $("#usuario").val();
            var password = $("#contrasena").val();

            var url = basePath + "Super/ValidacionLogin";
            var data = { username: username, password: password };
            DataPostSend(url, data, true).done(function (response) {
                if (response) {
                    if (response.status == "success") {
                        toastr.success(response.mensaje, "Mensaje Servidor");
                        var redirectUrl = basePath + "Super/control";
                        if (getCookie("urlredireccion") != "") { redirectUrl = getCookie("urlredireccion"); deleteCookie("urlredireccion") }
                        $("#blockLogin").hide();
                        $('#body-container').css('margin-top', '15%');
                        setTimeout(function () {
                            window.location.replace(redirectUrl);
                        }, 1000);
                    } else if (response.status == "failed") {
                        toastr.error(response.message, "Mensaje Servidor");
                    } else {
                        toastr.error("Solicitud no autorizada", "Mensaje Servidor");
                    }
                } else {
                    toastr.error("Error no se logro conectar al servidor", "Mensaje Servidor");
                }
            }).fail(function (x) {
                toastr.error("Error Fallo Servidor", "Mensaje Servidor");
            });
        }
    });

    $('#usuario').focus();

    $(".login-form input").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            $('.btnIngresar').click();
        }

    });
});
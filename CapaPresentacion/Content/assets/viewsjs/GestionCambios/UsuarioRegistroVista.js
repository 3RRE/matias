$(document).ready(function () {
    $("#frmRegistroUsuario")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                UsuarioNombre: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Nombre de Usuario, Obligatorio'
                        }
                    }
                },
                UsuarioContraseña: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Contraseña, Obligatorio'
                        }
                    }
                }
            }
        })
        .on('success.field.bv', function (e, data) {
            e.preventDefault();
            var $parent = data.element.parents('.form-group');
            // Remove the has-success class
            $parent.removeClass('has-success');
            // Hide the success icon
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();


        });

    $("#txtUsuarioID").val(usuario.UsuarioID);
    $("#lblNombreEmpleado").text(usuario.NombreEmpleado);
    $("#txtEmpleadoID").val(usuario.EmpleadoID);
    $("#txtusuario").val(usuario.UsuarioNombre);
    $("#txtContrasenaAnt").val(usuario.UsuarioContraseña);
    $("#txtTipoUsuarioID").val(usuario.TipoUsuarioID);
    $("#txtFailedAttemps").val(usuario.FailedAttempts);

    $('.btnGuardar').on('click', function (e) {
        $("#frmRegistroUsuario").data('bootstrapValidator').resetForm();
        var validar = $("#frmRegistroUsuario").data('bootstrapValidator').validate();
        if (validar.isValid()) {

            //var contraseñaCant = $("#txtContrasena").val().length;
            var contraseña = $("#txtContrasena").val();            

            var user = $("#txtusuario").val();
            var url = basePath + "Usuario/UsuarioCoincidenciaObtenerJson";

            var data = { usuarioNombre: user, usuarioID: usuario.UsuarioID, condicion: 1 };
            DataPostSend(url, data, false).done(function (response) {

                if (response) {
                    if (response.respuesta) {
                        var data = response.respuesta;

                        if (data.UsuarioID > 0) {
                            toastr.error("Usuario ya registrado, por favor escriba otro usuario", "Mensaje Servidor");
                        } else {
                            var url = basePath + "Usuario/UsuarioActualizarIdJason";
                            var redirectUrl = basePath + "Usuario/UsuarioListadoVista";
                            var dataForm = $('#frmRegistroUsuario').serializeFormJSON();
                            enviarDataPost(url, dataForm, false, false, redirectUrl, false);

                        }

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

    $('.btnListar').on('click', function (e) {
        window.location.replace(basePath + "Usuario/UsuarioListadoVista");
    });

    $("input").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            $('.btnGuardar').click();
        }

    });
    
});

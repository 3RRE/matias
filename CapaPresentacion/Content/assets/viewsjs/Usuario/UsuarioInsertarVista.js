
$(document).ready(function () {
    $("#frmNuevoUsuario")
        .bootstrapValidator({
            container: '#messages',
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                EmpleadoID: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                UsuarioNombre: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                UsuarioContraseña: {
                    validators: {
                        notEmpty: {
                            message: ''
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

    llenarSelect(basePath + "Empleado/EmpleadoListarPorNoUsadosJson", {}, "cboEmpleadoID", "EmpleadoID", "NombreCompleto");
    $(document).on('change', '#cboEmpleadoID', function (e) {        
        var id = $(this).val();
        var url = basePath + "Empleado/UsuarioEmpleadoIdObtenerJson";
        var data = { empleadiId: id };
        DataPostSend(url, data, false).done(function (response) {
            if (response) {
                if (response.respuesta) {
                    var data = response.respuesta;
                    if (data.UsuarioID > 0) {
                        $("#txtusuario").val("");
                        $("#txtContrasena").val("");
                        $("#cboEmpleadoID").val("");
                        toastr.error("Este Empleado, Ya tiene Un Usuario Registrado;Seleccione otro.", "Mensaje Servidor");                        
                    }
                    else {
                        var EmpleadoID = data.UsuarioID;
                        $.ajax({
                            url: basePath + "Usuario/UsuarioGenerarJson",
                            type: "POST",
                            contentType: "application/json",
                            data: JSON.stringify({ EmpleadoID: id }),
                            beforeSend: function () {
                            },
                            complete: function () {
                            },
                            success: function (response) {
                                $("#txtusuario").val(response.UsuarioNombre);
                                $("#txtContrasena").val(response.UsuarioContraseña);
                            },
                            error: function (xmlHttpRequest, textStatus, errorThrow) {
                            }
                        });
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

    });

    $('.btnGuardar').on('click', function (e) {
        $("#frmNuevoUsuario").data('bootstrapValidator').resetForm();
        var validar = $("#frmNuevoUsuario").data('bootstrapValidator').validate();
        if (validar.isValid()) {                       

            var user = $("#txtusuario").val();
            var contrasena = $("#txtContrasena").val();
            var url = basePath + "Usuario/UsuarioCoincidenciaObtenerJson";
            var data = { usuarioNombre: user, usuarioID: 0, condicion: 0 };
            DataPostSend(url, data, false).done(function (response) {
                if (response) {
                    if (response.respuesta) {
                        var data = response.respuesta;

                        if (data.UsuarioID > 0) {
                            //toastr.error("Usuario ya registrado, por favor escriba otro usuario", "Mensaje Servidor");
                            $("#txtusuario").val(user + Math.floor(Math.random() * 100));
                            toastr.error("Nombre de Usuario ya registrado; Se ha generado un nuevo usuario, Guarde nuevamente.", "Mensaje Servidor");
                        }
                        else {
                            console.log("a");
                            var url = basePath + "Usuario/UsuarioGuardarJson";
                            var redirectUrl = basePath + "Usuario/UsuarioListadoVista";
                            var dataForm = $('#frmNuevoUsuario').serializeFormJSON();
                            setCookie("datainicial", "");
                            $.when(dataAuditoria(1, "#frmNuevoUsuario", 3, "Usuario/UsuarioGuardarJson", "NUEVO USUARIO")).then(function (response, textStatus) {
                                enviarDataPost(url, dataForm, false, false, redirectUrl, true);
                            });
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
VistaAuditoria("Usuario/UsuarioInsertarVista", "VISTA", 0, "", 3);

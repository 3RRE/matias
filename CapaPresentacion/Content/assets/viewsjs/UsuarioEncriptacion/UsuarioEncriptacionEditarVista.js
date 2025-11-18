$(document).ready(function () {
    $("#txtFechaInicio").datetimepicker({
        format: 'DD/MM/YYYY h:mm:ss a',
    });
    $("#txtFechaFin").datetimepicker({
        format: 'DD/MM/YYYY h:mm:ss a',
    });

    $(document).on('click', '.btnListar', function () {
        var url = basePath + "UsuarioEncriptacion/UsuarioEncriptacionListarVista";
        window.location.replace(url);
    });

    $("#txtId").val(usuario.Id);
    $("#txtUsuarioNombre").val(usuario.UsuarioNombre);
    $("#txtFechaInicio").val(moment(usuario.FechaIni).format('DD/MM/YYYY h:mm:ss a'));
    $("#txtFechaFin").val(moment(usuario.FechaFin).format('DD/MM/YYYY h:mm:ss a'));
    if (usuario.Estado == true) {
        $("#cboEstado").val(1);
    }
    else {
        $("#cboEstado").val(0);
    }

    $(".btnGuardar").click(function () {
        var validar = $("#frmNuevo");
        if ($("#cboEstado").val() == 1) {
            $("#txtEstado").val(true);
        } else {
            $("#txtEstado").val(false);
        }


        $("#frmNuevo").data('bootstrapValidator').resetForm();
        var validar = $("#frmNuevo").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "UsuarioEncriptacion/UsuarioEncriptacionEditarJson";
            var dataForm = $('#frmNuevo').serializeFormJSON();
            var redirectUrl = basePath + "UsuarioEncriptacion/UsuarioEncriptacionListarVista";
            setCookie("datainicial", "");
            $.when(dataAuditoria(1, "#frmNuevo", 3, "UsuarioEncriptacion/UsuarioEncriptacionEditarJson", "EDITAR USUARIO ENCRIPTADO")).then(function (response, textStatus) {
                enviarDataPost(url, dataForm, false, false, redirectUrl, true);
            });
        }
    });
});

$("#frmNuevo")
    .bootstrapValidator({
        //container: '#messages',
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            TecnicoId: {
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
            UsuarioPassword: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            FechaIni: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            FechaFin: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            Estado1: {
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
        $parent.removeClass('has-success');
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });

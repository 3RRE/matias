$(document).ready(function () {

    $.when(llenarSelect(basePath + "UsuarioEncriptacion/TecnicoListarJson", {}, "cboTecnico", "Id", "Nombre", "")).then(function (response, textStatus) {
        $("#cboTecnico").select2();
    });
    var dateNow = new Date();
    dateNow.setDate(dateNow.getDate());
    $("#txtFechaInicio").datetimepicker({
        format: 'DD/MM/YYYY h:mm:ss a',
        defaultDate: dateNow
    });
    var dateNowHasta = new Date();
    dateNowHasta.setDate(dateNowHasta.getDate() + 1);
    $("#txtFechaFin").datetimepicker({
        format: 'DD/MM/YYYY h:mm:ss a',
        defaultDate: dateNowHasta
    });

    $("#cboTecnico").change(function () {
        var Id = $(this).val();
        var nombrefull = $(this).find("option:selected").text();
        $("#txtUsuarioNombre").val(nombrefull);
    });

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
            var url = basePath + "UsuarioEncriptacion/UsuarioEncriptacionInsertarJson";
            var dataForm = $('#frmNuevo').serializeFormJSON();
            var redirectUrl = basePath + "UsuarioEncriptacion/UsuarioEncriptacionListarVista";            
            setCookie("datainicial", "");
            $.when(dataAuditoria(1, "#frmNuevo", 3, "UsuarioEncriptacion/UsuarioEncriptacionInsertarJson", "NUEVO USUARIO ENCRIPTADO")).then(function (response, textStatus) {
                enviarDataPost(url, dataForm, false, false, redirectUrl, true);
            });

            $("select.select2-hidden-accessible").val(null).trigger("change");
            $('input').val('');
            $.when(llenarSelect(basePath + "UsuarioEncriptacion/TecnicoListarJson", {}, "cboTecnico", "EmpleadoId", "Nombre", "")).then(function (response, textStatus) {
                $("#cboTecnico").select2();
            });
        }
    });

    $(document).on('click', '.btnListar', function () {
        var url = basePath + "UsuarioEncriptacion/UsuarioEncriptacionListarVista";
        window.location.replace(url);
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
            EmpleadoId: {
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
$(document).ready(function () {
    $("#frmNuevoSistema")
        .bootstrapValidator({
            container: '#messages',
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                Descripcion: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                Version: {
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

    

    $('.btnGuardar').on('click', function (e) {
        $("#frmNuevoSistema").data('bootstrapValidator').resetForm();
        var validar = $("#frmNuevoSistema").data('bootstrapValidator').validate();

        if (validar.isValid()) {                       
            
            var url = basePath + "GestionCambios/SistemaGuardarJson";
            var redirectUrl = basePath + "GestionCambios/GestionCambiosSistemaListadoVista";
            var dataForm = $('#frmNuevoSistema').serializeFormJSON();
            $.when(dataAuditoria(1, "#frmNuevoSistema", 3, "GestionCambios/SistemaGuardarJson", "NUEVO SISTEMA")).then(function (response, textStatus) {
                enviarDataPost(url, dataForm, false, false, redirectUrl, true);
            });
        }

    });

    $('.btnListar').on('click', function (e) {
        window.location.replace(basePath + "GestionCambios/GestionCambiosSistemaListadoVista");
    });

    $("input").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            $('.btnGuardar').click();
        }

    });    

    VistaAuditoria("GestionCambios/GestionCambioSistemaInsertar", "VISTA", 0, "", 3);
});

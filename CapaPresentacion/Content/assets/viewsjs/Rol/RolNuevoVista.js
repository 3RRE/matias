
$(document).ready(function () {
    $("#frmNuevoRol")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                WEB_RolNombre: {
                    validators: {
                        notEmpty: {
                            message: ' '
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
        $("#frmNuevoRol").data('bootstrapValidator').resetForm();
        var validar = $("#frmNuevoRol").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "Rol/GuardarRol";
            var redirectUrl = basePath + "Rol/ListadoRol";
            var dataForm = $('#frmNuevoRol').serializeFormJSON();
      
            $.when(dataAuditoria(1, "#frmNuevoRol", 3, "Rol/GuardarRol", "NUEVO ROL")).then(function (response, textStatus) {
                enviarDataPost(url, dataForm, false, false, redirectUrl, true);
            });
        }
    });

    $('.btnListar').on('click', function (e) {
        window.location.replace(basePath + "Rol/ListadoRol");
    });

    $("input").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            $('.btnGuardar').click();
        }

    });
    VistaAuditoria("Rol/NuevoRol", "VISTA", 0, "", 3);
});


$(document).ready(function () {
    $("#frmRegistroRol")
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

    $("#txtRolID").val(rol.WEB_RolID);
    $("#txtNombre").val(rol.WEB_RolNombre);
    $("#txtDescripcion").val(rol.WEB_RolDescripcion);

    $.when(VistaAuditoria("Rol/RegistroRol", "VISTA", 1, "#frmRegistroRol", 3)).then(function (response, textStatus) {
        dataAuditoria(0, "#frmRegistroRol", 3);
    });

    $('.btnGuardar').on('click', function (e) {
        $("#frmRegistroRol").data('bootstrapValidator').resetForm();
        var validar = $("#frmRegistroRol").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "Rol/ActualizarRol";
            var redirectUrl = basePath + "Rol/ListadoRol";
            var dataForm = $('#frmRegistroRol').serializeFormJSON();
           
            $.when(dataAuditoria(1, "#frmRegistroRol", 3, "Rol/ActualizarRol", "ACTUALIZAR ROL")).then(function (response, textStatus) {
                enviarDataPost(url, dataForm, false, false, redirectUrl, false);
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
});

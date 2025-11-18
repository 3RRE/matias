$(document).ready(function () {
    $('#cboEstado').val(destinatario.estado);
    $('#txtNombre').val(destinatario.Nombre);
    $('#txtEmail').val(destinatario.Email);
    $('#txtEmailID').val(destinatario.EmailID);

    $.when(VistaAuditoria("Destinatario/DestinatarioEditarVista", "VISTA", 1, "#frmEditar", 3)).then(function (response, textStatus) {
        dataAuditoria(0, "#frmEditar", 3);
    });

    $('#btnGuardar').on('click', function (e) {
        $("#frmEditar").data('bootstrapValidator').resetForm();
        var validar = $("#frmEditar").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "Destinatario/DestinatarioEditarJson";

            $.when(dataAuditoria(1, "#frmEditar", 3, "Destinatario/DestinatarioEditarJson", "ACTUALIZAR DESTINATARIO")).then(function (response, textStatus) {
                var data = $('#frmEditar').serializeFormJSON();
                $.ajax({
                    url: url,
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(data),
                    beforeSend: function () {
                        $.LoadingOverlay("show");
                    },
                    complete: function () {
                        $.LoadingOverlay("hide");
                    },
                    success: function (response) {
                        if (response.data) {
                            toastr.success("Destinatario Editado", "Mensaje Servidor");
                        }
                        else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrow) {
                        toastr.error("Error Servidor", "Mensaje Servidor");
                    }
                });
            });

            
        }
    });
    $('#btnListado').on('click', function (e) {
        window.location.replace(basePath + "Destinatario/DestinatarioListadoVista");
    });
});
$("#frmEditar")
    .bootstrapValidator({
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            Estado: {
                validators: {
                    notEmpty: {
                        message: 'Seleccione Estado, Obligatorio'
                    }
                }
            },
            Nombre: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese Nombre, Obligatorio'
                    }
                }
            },
            Email: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese Correo Electronico, Obligatorio'
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
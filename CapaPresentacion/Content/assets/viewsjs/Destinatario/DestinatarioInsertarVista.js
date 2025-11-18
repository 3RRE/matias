$(document).ready(function () {         
    $('#btnGuardar').on('click', function (e) {
        $("#frmNuevo").data('bootstrapValidator').resetForm();
        var validar = $("#frmNuevo").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "Destinatario/DestinatarioInsertarJson";
            var data = $('#frmNuevo').serializeFormJSON();

            $.when(dataAuditoria(1, "#frmNuevo", 3, "Destinatario/DestinatarioInsertarJson", "NUEVO DESTINATARIO")).then(function (response, textStatus) {
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
                            $('#cboEstado').val("");
                            $('#txtNombre').val("");
                            $('#txtEmail').val("");
                            toastr.success("Destinatario Guardado", "Mensaje Servidor");
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

VistaAuditoria("Destinatario/DestinatarioInsertarVista", "VISTA", 0, "", 3);
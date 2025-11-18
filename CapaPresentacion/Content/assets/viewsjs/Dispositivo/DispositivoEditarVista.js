$(document).ready(function () {
    $('#cboEstado').val(dispositivo.EsActivo);
    $('#txtMac').val(dispositivo.Mac);
    $('#txtDispositivoId').val(dispositivo.DispositivoId); 

    $.when(VistaAuditoria("Dispositivo/DispositivoEditarVista", "VISTA", 1, "#frmEditar", 3)).then(function (response, textStatus) {
        dataAuditoria(0, "#frmEditar", 3);
    });

    $('#btnGuardar').on('click', function (e) {
        $("#frmEditar").data('bootstrapValidator').resetForm();
        var validar = $("#frmEditar").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "Dispositivo/DispositivoEditarJson";

            $.when(dataAuditoria(1, "#frmEditar", 3, "Dispositivo/DispositivoEditarJson", "ACTUALIZAR DESTINATARIO")).then(function (response, textStatus) {
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
                            toastr.success("Dispositivo Editado", "Mensaje Servidor");
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
        window.location.replace(basePath + "Dispositivo/DispositivoListadoVista");
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
            EsActivo: {
                validators: {
                    notEmpty: {
                        message: 'Seleccione Estado, Obligatorio'
                    }
                }
            },
            Mac: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese MAC, Obligatorio'
                    }
                }
            }, 
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

function check(e) {
    tecla = (document.all) ? e.keyCode : e.which;

    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }

    // Patron de entrada, en este caso solo acepta numeros y letras
    patron = /[A-Za-z0-9]/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}
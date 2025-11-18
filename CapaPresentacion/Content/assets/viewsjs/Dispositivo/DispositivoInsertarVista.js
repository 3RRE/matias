$(document).ready(function () {         
    $('#btnGuardar').on('click', function (e) {
        $("#frmNuevo").data('bootstrapValidator').resetForm();
        var validar = $("#frmNuevo").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "Dispositivo/DispositivoInsertarJson";
            var data = $('#frmNuevo').serializeFormJSON();

            $.when(dataAuditoria(1, "#frmNuevo", 3, "Dispositivo/DispositivoInsertarJson", "NUEVO DISPOSITIVO")).then(function (response, textStatus) {
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
                            $('#txtMax').val(""); 
                            toastr.success("Dispositivo Guardado", "Mensaje Servidor");
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

VistaAuditoria("Dispositivo/DispositivoInsertarVista", "VISTA", 0, "", 3);
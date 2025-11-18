const btnGuardar = $("#btnGuardar");
const formRegistro = $("#form_registro");

const getSistemas = () => {
    $.when(
        llenarSelect(
            `${basePath}Sistema/ObtenerSistemas`, {}, "cboSistemaSelect", "Id", "Nombre", parseInt($('#IdSistema').val()))
    ).then(() => {
        $('#cboSistemaSelect').select2({
            placeholder: "Seleccione un sistema...",
            multiple: false,
            dropdownParent: $('#full-modal')
        });
    });
};

btnGuardar.off('click').on('click', () => {
    formRegistro.data('bootstrapValidator').resetForm();
    const validar = formRegistro.data('bootstrapValidator').validate();
    if (!validar.isValid()) {
        return;
    }
    const data = formRegistro.serializeFormJSON();
    data.SSLHabilitado = $("#toggleSSLHabilitado").is(":checked") ? 1 : 0;
    data.Estado = $("#toggleEstado").is(":checked") ? 1 : 0;
    save(data);
});

const save = (data) => {
    const url = `${basePath}CredencialCorreo/SaveCredencialCorreo`;
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            if (!response.success) {
                toastr.error(response.displayMessage, "Mensaje Servidor");
                return;
            }
            if (cboSistema.val()) {
                ObtenerCredencialesCorreoPorSistema(cboSistema.val())
            } else {
                ObtenerCredencialesCorreo()
            }
            toastr.success(response.displayMessage, "Mensaje Servidor");
            $('#full-modal').modal('hide');
        }
    });
};

formRegistro.bootstrapValidator({
    excluded: [':disabled', ':hidden', ':not(:visible)'],
    feedbackIcons: {
        valid: 'icon icon-check',
        invalid: 'icon icon-cross',
        validating: 'icon icon-refresh'
    },
    fields: {
        IdSistema: {
            validators: {
                notEmpty: {
                    message: 'El sistema es requerido.'
                }
            }
        },
        NombreRemitente: {
            validators: {
                notEmpty: {
                    message: 'El nombre del remitente es requerido.'
                }
            }
        },
        Correo: {
            validators: {
                notEmpty: {
                    message: 'El correo es requerido.'
                },
                emailAddress: {
                    message: 'El correo no es válido.'
                }
            }
        },
        ClaveSMTP: {
            validators: {
                notEmpty: {
                    message: 'La clave SMTP es requerida.'
                }
            }
        },
        ServidorSMTP: {
            validators: {
                notEmpty: {
                    message: 'El servidor SMTP es requerido.'
                }
            }
        },
        PuertoSMTP: {
            validators: {
                notEmpty: {
                    message: 'El puerto SMTP es requerido.'
                },
                numeric: {
                    message: 'El puerto SMTP debe ser un número.'
                }
            }
        },
        CuotaDiaria: {
            validators: {
                notEmpty: {
                    message: 'La cuota diaria es requerida.'
                },
                numeric: {
                    message: 'La cuota diaria debe ser un número.'
                }
            }
        },
        Prioridad: {
            validators: {
                notEmpty: {
                    message: 'La prioridad es requerida.'
                },
                numeric: {
                    message: 'La prioridad debe ser un número.'
                }
            }
        }
    }
}).on('success.field.bv', (e, data) => {
    e.preventDefault();
    var $parent = data.element.parents('.form-group');
    $parent.removeClass('has-success');
    $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
});

$("input").keydown((e) => {
    if (e.keyCode == 13) {
        e.preventDefault();
        btnGuardar.click();
    }
});


$(document).ready(() => {
    getSistemas();
})

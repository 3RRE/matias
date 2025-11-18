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
    save(data);
});

const save = (data) => {
    const url = `${basePath}CredencialWhatsApp/SaveCredencialWhatsApp`;
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
                ObtenerCredencialesWhatsAppPorSistema(cboSistema.val())
            } else {
                ObtenerCredencialesWhatsApp()
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
        UrlBase: {
            validators: {
                notEmpty: {
                    message: 'La URL base es requerida.'
                }
            }
        },
        Instancia: {
            validators: {
                notEmpty: {
                    message: 'La instancia es requerida.'
                }
            }
        },
        Token: {
            validators: {
                notEmpty: {
                    message: 'El token es requerido.'
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

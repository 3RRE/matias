const btnGuardar = $("#btnGuardar");
const formRegistro = $("#form_registro");

btnGuardar.off('click').on('click', () => {
    formRegistro.data('bootstrapValidator').resetForm();
    const validar = formRegistro.data('bootstrapValidator').validate();
    if (!validar.isValid()) {
        return;
    }
    const data = formRegistro.serializeFormJSON();
    save(data);
})

const save = (data) => {
    const url = `${basePath}Sistema/SaveSistema`;
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
            toastr.success(response.displayMessage, "Mensaje Servidor");
            $('#full-modal').modal('hide');
            ObtenerSistemas();
        }
    })
}

formRegistro.bootstrapValidator({
    excluded: [':disabled', ':hidden', ':not(:visible)'],
    feedbackIcons: {
        valid: 'icon icon-check',
        invalid: 'icon icon-cross',
        validating: 'icon icon-refresh'
    },
    fields: {
        Nombre: {
            validators: {
                notEmpty: {
                    message: 'El nombre es requerido.'
                }
            }
        },
        Descripcion: {
            validators: {
                notEmpty: {
                    message: 'La descripción es requerida.'
                }
            }
        }
    }
}).on('success.field.bv', (e, data) => {
    e.preventDefault();
    var $parent = data.element.parents('.form-group');
    $parent.removeClass('has-success');
    $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide()
})

$("input").keydown((e) => {
    if (e.keyCode == 13) {
        e.preventDefault();
        btnGuardar.click();
    }
})
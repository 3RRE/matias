const btnGuardar = $("#btnGuardar");
const formRegistro = $('#form_registro');
const cboArea = $('#cboArea');

const getAreas = () => {
    $.when(
        llenarSelect(`${basePath}BotAreas/GetAreas`, {}, "cboArea", "Id", "Nombre", parseInt($('#IdArea').val()))
    ).then(() => {
        cboArea.select2({
            placeholder: "Seleccione ...",
            multiple: false,
            dropdownParent: $('#full-modal')
        });
    });
}

$(document).ready(() => {
    getAreas();
})

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
    const url = `${basePath}BotCargos/SaveCargo`;
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
            getCargos();
            toastr.success(response.displayMessage, "Mensaje Servidor");
            $('#full-modal').modal('hide');
        }
    });
}

formRegistro.bootstrapValidator({
    excluded: [':disabled', ':hidden', ':not(:visible)'],
    feedbackIcons: {
        valid: 'icon icon-check',
        invalid: 'icon icon-cross',
        validating: 'icon icon-refresh'
    },
    fields: {
        IdArea: {
            validators: {
                notEmpty: {
                    message: ' '
                }
            }
        },
        Nombre: {
            validators: {
                notEmpty: {
                    message: ' '
                }
            }
        }
    }
}).on('success.field.bv', (e, data) => {
    e.preventDefault();
    let $parent = data.element.parents('.form-group');
    $parent.removeClass('has-success');
    $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
});

$("input").keydown((e) => {
    if (e.keyCode == 13) {
        e.preventDefault();
        btnGuardar.click();
    }
});

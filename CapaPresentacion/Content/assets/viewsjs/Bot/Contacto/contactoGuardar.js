const btnGuardar = $("#btnGuardar");
const btnBuscarEmpleado = $("#btnBuscarEmpleado");
const formRegistro = $('#form_registro');
const cboCargo = $('#cboCargo');

const getCargos = () => {
    llenarSelect2Agrupado({
        url: `${basePath}BotCargos/GetCargos`,
        select: "cboCargo",
        campoId: "Id",
        campoAgrupacion: "NombreArea",
        campoValor: "Nombre",
        selectedVal: parseInt($('#IdCargo').val()),
        dropdownParent: $('#full-modal')
    })
}

const searchEmployeeByDni = (dni) => {
    const url = `${basePath}BotEmpleados/GetEmpleadoByDocumentNumber`;
    const data = {
        documentNumber: dni
    }
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            if (!response.success) {
                toastr.error(response.displayMessage);
                cleanEmployeeData();
                return;
            }
            toastr.success(response.displayMessage);
            setEmployeeData(response.data)
        }
    });
}

const setEmployeeData = (employee) => {
    $('#cboCargo').val(employee.IdCargo).trigger("change");
    $('#Nombre').val(employee.NombreCompleto);
    $('#CodigoPaisCelular').val(employee.CodigoPaisCelular);
    $('#Celular').val(employee.TelefonoParticular);
    $('#Correo').val(employee.Email);
}

const cleanEmployeeData = () => {
    cboCargo.val(null).trigger("change");
    $('#Nombre').val('');
    $('#CodigoPaisCelular').val('');
    $('#Celular').val('');
    $('#Correo').val('');
}

btnGuardar.off('click').on('click', () => {
    formRegistro.data('bootstrapValidator').resetForm();
    const validar = formRegistro.data('bootstrapValidator').validate();

    if (!validar.isValid()) {
        return;
    }

    const data = formRegistro.serializeFormJSON();

    save(data);
});

btnBuscarEmpleado.off('click').on('click', () => {
    const dni = $('#dni').val();
    if (!dni) {
        toastr.warning('Tiene que ingrese un DNI.')
        return;
    }
    searchEmployeeByDni(dni);
});

const save = (data) => {
    const url = `${basePath}BotContactos/SaveContacto`;
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
            getContactos();
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
        IdCargo: {
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
        },
        CodigoPaisCelular: {
            validators: {
                notEmpty: {
                    message: ' '
                }
            }
        },
        Celular: {
            validators: {
                notEmpty: {
                    message: ' '
                }
            }
        },
        Correo: {
            validators: {
                notEmpty: {
                    message: ' '
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
    getCargos();
})

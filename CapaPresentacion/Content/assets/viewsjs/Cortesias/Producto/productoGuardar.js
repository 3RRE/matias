const btnGuardar = $("#btnGuardar");
const formRegistro = $('#form_registro');
const cboSubTipo = $('#cboSubTipo');
const cboMarca = $('#cboMarca');

const getSubTipos = () => {
    llenarSelect2Agrupado({
        url: `${basePath}SubTipos/GetSubTipos`,
        select: "cboSubTipo",
        campoId: "Id",
        campoAgrupacion: "NombreTipo",
        campoValor: "Nombre",
        selectedVal: parseInt($('#IdSubTipo').val()),
        dropdownParent: $('#full-modal')
    })
}

const getMarcas = () => {
    $.when(
        llenarSelect(`${basePath}Marcas/GetMarcas`, {}, "cboMarca", "Id", "Nombre", parseInt($('#IdMarca').val()))
    ).then(() => {
        cboMarca.select2({
            placeholder: "Seleccione ...",
            multiple: false,
            dropdownParent: $('#full-modal')
        });
    });
}

btnGuardar.off('click').on('click', () => {
    formRegistro.data('bootstrapValidator').resetForm();
    const validar = formRegistro.data('bootstrapValidator').validate();

    if (!validar.isValid()) {
        return;
    }

    var formData = new FormData($('#form_registro')[0]);

    save(formData);
});

const save = (data) => {
    const url = `${basePath}Productos/SaveProducto`;
    $.ajax({
        url: url,
        type: "POST",
        processData: false, 
        contentType: false, 
        data: data,
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            if (!response.success) {
                toastr.error(response.displayMessage, "Mensaje Servidor");
                return;
            }
            getProductos();
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
        idSubTipo: {
            validators: {
                notEmpty: {
                    message: ' '
                }
            }
        },
        idMarca: {
            validators: {
                notEmpty: {
                    message: ' '
                }
            }
        },
        nombre: {
            validators: {
                notEmpty: {
                    message: ' '
                }
            }
        },
        estado: {
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
    getSubTipos();
    getMarcas();
})

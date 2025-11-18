const formEquivalenciaEmpresa = $("#formEquivalenciaEmpresa");
const btnGuardarModal = $("#btnGuardarModal");

const actualizarEquivalenciaEmpresa = (data) => {
    let url = basePath + "EquivalenciaEmpresa/ActualizarEquivalenciaEmpresaJSON";
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
            if (!response.success) {
                toastr.error(response.displayMessage);
                return;
            }
            listarEquivalenciaEmpresas();
            toastr.success(response.displayMessage);
            $('#modalGroup').modal('hide');
            $('#formEquivalenciaEmpresa input').val('');
        }
    });
}

$(document).ready(function () {

    formEquivalenciaEmpresa.bootstrapValidator({
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            UrlBase: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese url base.'
                    }
                }
            },
            EquivalenciaEmpresa: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese instancia.'
                    }
                }
            },
            Token: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese token.'
                    }
                }
            }
        }
    }).on('success.field.bv', function (e, data) {
        e.preventDefault();
        var $parent = data.element.parents('.form-group');
        // Remove the has-success class
        $parent.removeClass('has-success');
        // Hide the success icon
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });

    $("input").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            btnGuardarModal.click();
        }
    });

    btnGuardarModal.off('click');
    btnGuardarModal.on('click', function (e) {
        formEquivalenciaEmpresa.data('bootstrapValidator').resetForm();
        let validar = formEquivalenciaEmpresa.data('bootstrapValidator').validate();
        if (!validar.isValid()) {
            return;
        }
        let dataForm = formEquivalenciaEmpresa.serializeFormJSON();

        if (dataForm.CodEmpresaOfisis <= 0) {
            toastr.warning("Cod. OFISIS debe ser un número mayor a cero (0).");
            return;
        }

        if (dataForm.IdEmpresaBuk <= 0) {
            toastr.warning("Id BUK debe ser un número mayor a cero (0).");
            return;
        }

        actualizarEquivalenciaEmpresa(dataForm);
    });
});

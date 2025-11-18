const formSalaMaestra = $("#formSalaMaestra");
const btnGuardarModal = $("#btnGuardarModal");
const modal = $("#modalGroup");

$(document).ready(function () {
    $("#cboEstado").select2({
        placeholder: "--Seleccione--",
        dropdownParent: modal
    })
});

const actualizarSakaMaestra = (data) => {
    let url = basePath + "SalaMaestra/SalaMaestraActualizarJSON";
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
            listarSalasMaestras();
            toastr.success(response.displayMessage);
            $('#modalGroup').modal('hide');
            $('#formEquivalenciaEmpresa input').val('');
        }
    });
}

$(document).ready(function () {
    formSalaMaestra.bootstrapValidator({
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
                        message: 'Ingrese nombre de la sala maestra.'
                    }
                }
            },
            Estado: {
                validators: {
                    notEmpty: {
                        message: 'Seleccione un estado.'
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

    btnGuardarModal.off('click').on('click', function (e) {
        formSalaMaestra.data('bootstrapValidator').resetForm();
        let validar = formSalaMaestra.data('bootstrapValidator').validate();
        if (!validar.isValid()) {
            return;
        }
        let dataForm = formSalaMaestra.serializeFormJSON();

        actualizarSakaMaestra(dataForm);
    });
});

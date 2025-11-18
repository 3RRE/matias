$(document).ready(function () {
    const formInstancia = $("#formInstancia");
    const btnGuardarModal = $("#btnGuardarModal");

    formInstancia.bootstrapValidator({
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
            Instancia: {
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
        formInstancia.data('bootstrapValidator').resetForm();
        let validar = formInstancia.data('bootstrapValidator').validate();
        if (!validar.isValid()) {
            return;
        }
        let url = basePath + "Instancia/ActualizarInstanciaJSON";
        let  dataForm = formInstancia.serializeFormJSON();
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ instancia: dataForm }),
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
                listarInstancias();
                toastr.success(response.displayMessage);
                $('#modalGroup').modal('hide');
                $('#formInstancia input').val('');
            }
        });
    });
});

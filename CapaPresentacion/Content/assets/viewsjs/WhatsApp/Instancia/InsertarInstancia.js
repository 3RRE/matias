$(document).ready(function () {
    const formInstancia = $("#formInstancia");
    const cboSala = $("#cboSala");
    const btnGuardarModal = $("#btnGuardarModal");

    const obtenerListaSalas = () => {
        $.ajax({
            type: "POST",
            url: basePath + "Sala/ListadoSalaPorUsuarioJson",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            },
            success: function (result) {
                renderSelectSalas(result.data)
            },
        });
    }

    const renderSelectSalas = (data) => {
        $.each(data, function (index, value) {
            cboSala.append(`<option value="${value.CodSala}">${value.Nombre}</option>`)
        });
        cboSala.select2({
            placeholder: "--Seleccione--", allowClear: true, minimumResultsForSearch: 5, dropdownParent: $('#modalGroup')
        });
        cboSala.val(null).trigger("change");
    }

    obtenerListaSalas()

    formInstancia.bootstrapValidator({
        container: '#messages',
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
            },
            CodSala: {
                validators: {
                    notEmpty: {
                        message: 'Seleccione sala.'
                    }
                }
            }
        }
    }).on('success.field.bv', function (e, data) {
        e.preventDefault();
        let $parent = data.element.parents('.form-group');
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
        let url = basePath + "Instancia/InsertarInstanciaJSON";
        let dataForm = formInstancia.serializeFormJSON();
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

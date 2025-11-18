const formEquivalenciaSede = $("#formEquivalenciaSede");
const btnGuardarModal = $("#btnGuardarModal");
const idEquivalenciaEmpresa = $("#IdEquivalenciaEmpresa").val();
const cboEquivalenciaEmpresa = $("#cboEquivalenciaEmpresa");

const actualizarEmpresasEquivalencia = (data) => {
    let url = basePath + "EquivalenciaSede/ActualizarEquivalenciaSedeJSON";
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
            listarSedes();
            toastr.success(response.displayMessage);
            $('#modalGroup').modal('hide');
            $('#formEquivalenciaSede input').val('');
        }
    });
}

const obtenerListaEmpresasEquivalencia = () => {
    $.ajax({
        type: "POST",
        url: basePath + "EquivalenciaEmpresa/ListarTodasLasEquivalenciasEmpresaCorrectasJSON",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        },
        success: function (result) {
            if (result.success) {
                renderSelectEmpresasEquivalencias(result.data)
            }
        },
    });
}

const renderSelectEmpresasEquivalencias = (data) => {
    $.each(data, function (index, value) {
        cboEquivalenciaEmpresa.append(`<option value="${value.IdEquivalenciaEmpresa}">${value.Nombre}</option>`)
    });
    cboEquivalenciaEmpresa.select2({
        placeholder: "--Seleccione--", allowClear: true, minimumResultsForSearch: 5, dropdownParent: $('#modalGroup')
    });
    cboEquivalenciaEmpresa.val(idEquivalenciaEmpresa).trigger('change');
}

$(document).ready(function () {
    obtenerListaEmpresasEquivalencia()

    formEquivalenciaSede.bootstrapValidator({
        container: '#messages',
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            CodSedeOfisis: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese codigo de sede en OFISIS.'
                    }
                }
            },
            NombreSede: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese nombre.'
                    }
                }
            },
            IdEquivalenciaEmpresa: {
                validators: {
                    notEmpty: {
                        message: 'Seleccione empresa.'
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
        formEquivalenciaSede.data('bootstrapValidator').resetForm();
        let validar = formEquivalenciaSede.data('bootstrapValidator').validate();
        if (!validar.isValid()) {
            return;
        }

        let dataForm = formEquivalenciaSede.serializeFormJSON();

        if (dataForm.CodSedeOfisis <= 0) {
            toastr.warning("Cod. OFISIS debe ser un número mayor a cero (0).");
            return;
        }

        actualizarEmpresasEquivalencia(dataForm);
    });
});

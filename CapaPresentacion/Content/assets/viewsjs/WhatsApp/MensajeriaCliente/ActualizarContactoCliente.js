$(document).ready(function () {

    $("#formContacto").bootstrapValidator({
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            CodigoPais: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese Código de País, Obligatorio.'
                    },
                    numeric: {
                        message: 'Ingrese un valor numerico.'
                    }
                }
            },
            Celular1: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese Número, Obligatorio.'
                    },
                    numeric: {
                        message: 'Ingrese un valor numerico.'
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
            $('#btnGuardarModal').click();
        }
    });


    $('#btnGuardarModal').off('click');
    $('#btnGuardarModal').on('click', function (e) {
        $("#formContacto").data('bootstrapValidator').resetForm();
        var validar = $("#formContacto").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "AsistenciaCliente/ActualizarContactoCliente";
            var dataForm = $('#formContacto').serializeFormJSON();
            $.ajax({
                url: url,
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(dataForm),
                beforeSend: function () {
                    $.LoadingOverlay("show");
                },
                complete: function () {
                    $.LoadingOverlay("hide");
                },
                success: function (response) {
                    if (response.success) {
                        btnBuscar.trigger('click');
                        toastr.success(response.displayMessage);
                        $('#modalGroup').modal('hide');
                        $('#formContacto input').val('');
                    } else {
                        toastr.error(response.displayMessage);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    if (jqXHR.status === 0) {
                        toastr.error("Not connect: Verify Network.", "Mensaje Servidor");
                    } else if (jqXHR.status == 404) {
                        toastr.error("Requested page not found [404]", "Mensaje Servidor");
                    } else if (jqXHR.status == 500) {
                        toastr.error("Internal Server Error [500].", "Mensaje Servidor");
                    } else if (textStatus === 'parsererror') {
                        toastr.error("Requested JSON parse failed.", "Mensaje Servidor");
                    } else if (textStatus === 'timeout') {
                        toastr.error("Time out error.", "Mensaje Servidor");
                    } else if (textStatus === 'abort') {
                        toastr.error("Ajax request aborted.", "Mensaje Servidor");
                    }
                }
            });
        }
    });
});

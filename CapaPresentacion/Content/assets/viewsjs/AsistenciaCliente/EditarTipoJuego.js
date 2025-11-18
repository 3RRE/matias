$(document).ready(function () {
    $("#Id").val(tipoJuego.Id)
    $("#Nombre").val(tipoJuego.Nombre)
    $("#Descripcion").val(tipoJuego.Descripcion)
    $("#cboEstado").val(tipoJuego.Estado.trim() == "A" ? "A" : "I")
    $(".mySelect").select2({
        multiple: false, placeholder: "--Seleccione--"
    })
    $('.btnListar').on('click', function (e) {
        window.location.replace(basePath + "AsistenciaCliente/ListadoTipoJuego");
    });
    $(document).on('click', '.btnGuardar', function (e) {
        e.preventDefault();
        $("#form_registro_tipojuego").data('bootstrapValidator').resetForm();
        let validar = $("#form_registro_tipojuego").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            let dataForm = $("#form_registro_tipojuego").serializeForm();
            $.ajax({
                url: basePath + "AsistenciaCliente/EditarTipoJuegoJson",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(dataForm),
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (response) {
                    if (response.respuesta) {
                        toastr.success(response.mensaje, "Mensaje Servidor")
                        setTimeout(function () {
                            window.location.href = basePath + "AsistenciaCliente/EditarTipoJuego/" + tipoJuego.Id;;
                        }, 2000);
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor")
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                }
            });
        }
    })
    $(document).on('keypress', '.UpperCase', function (event) {
        $input = $(this);
        setTimeout(function () {
            $input.val($input.val().toUpperCase());
        }, 50);
    })
})

$("#form_registro_tipojuego")
    .bootstrapValidator({
        container: '#messages',
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            Id: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            Nombre: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
        }
    })
    .on('success.field.bv', function (e, data) {
        e.preventDefault();
        var $parent = data.element.parents('.form-group');
        // Remove the has-success class
        $parent.removeClass('has-success');
        // Hide the success icon
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();


    });
$.fn.serializeForm = function () {

    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
}
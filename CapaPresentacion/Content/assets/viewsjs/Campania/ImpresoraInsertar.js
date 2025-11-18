$(document).ready(function () {
    ObtenerListaSalasRegistro();

    $('.btnListar').on('click', function (e) {
        window.location.replace(basePath + "CampaniaImpresora/ListadoImpresoras");
    });

    $('.btnGuardar').on('click', function (e) {
        $("#frmRegistroImpresora").data('bootstrapValidator').resetForm();
        var validar = $("#frmRegistroImpresora").data('bootstrapValidator').validate();
        if (validar.isValid()) {
          
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            lugar = "CampaniaImpresora/InsertarImpresoraJson";
            accion = "NUEVA IMPRESORA";
            urlenvio = basePath + "CampaniaImpresora/InsertarImpresoraJson";

            var dataForm = $('#frmRegistroImpresora').serializeFormJSON();
            $.when(dataAuditoria(1, "#frmRegistroImpresora", 3, lugar, accion)).then(function (response, textStatus) {
                $.ajax({
                    url: urlenvio,
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
                        if (response.respuesta) {
                            $("form input,select,textarea").val("");
                            $("#cboEstado").val(1);
                            $("#cbo_sala_id").val(null).trigger("change");
                            $.confirm({
                                icon: 'fa fa-spinner fa-spin',
                                title: 'Mensaje Servidor',
                                theme: 'black',
                                animationBounce: 1.5,
                                columnClass: 'col-md-6 col-md-offset-3',
                                confirmButtonClass: 'btn-info',
                                cancelButtonClass: 'btn-warning',
                                confirmButton: 'Ir a Listado ',
                                cancelButton: 'Seguir Registrando',
                                content: false,
                                confirm: function () {
                                    let url = basePath + "CampaniaImpresora/ListadoImpresoras";
                                    window.location.href = url;
                                },
                                cancel: function () {
                                }
                            });
                            
                        }
                        else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrow) {
                        
                    }
                });
            });

        }

    });

});

function ObtenerListaSalasRegistro() {
    $("#cbo_sala_id").html("");
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cbo_sala_id").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cbo_sala_id").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: true,
            });
            $("#cbo_sala_id").val(null).trigger("change");
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
    return false;
}

$("#frmRegistroImpresora")
    .bootstrapValidator({
        container: '#messages',
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            nombre: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            ip: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            puerto: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },

            sala_id: {
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
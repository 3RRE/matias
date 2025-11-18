$(document).ready(function () {
    // if(logoSala){
    //     let logo=ObtenerImgDrive(logoSala)
    //     logo.then(response=>{
    //         $(".img_logo").attr('src','data:image/png;base64,'+response.data)
    //     })
    // }

    $("#correo,#correo2").keydown(function (e) {
        if (e.keyCode == 192) {
            event.preventDefault();
            return false;
        }
    });

    if(logoSala){
        $(".img_logo").attr('src',logoSala)
    }
    $(document).on('click', "#btnSend", function (e) {
        $("#complaintsForm").data('bootstrapValidator').resetForm();
        let validar = $("#complaintsForm").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            if($("#correo").val() != $("#correo2").val()){
                $("#correo,#correo2").parent().addClass("has-error");
                $("#correo,correo2").parent().addClass("has-feedback");
                $(".form-control-feedback[data-bv-icon-for=correo]").removeClass("icon icon-check");
                $(".form-control-feedback[data-bv-icon-for=correo]").addClass("icon icon-cross");
                $(".form-control-feedback[data-bv-icon-for=correo]").show();
                $(".form-control-feedback[data-bv-icon-for=correo2]").removeClass("icon icon-check");
                $(".form-control-feedback[data-bv-icon-for=correo2]").addClass("icon icon-cross");
                $(".form-control-feedback[data-bv-icon-for=correo2]").show();
                toastr.error("Los campos de E-mail ser iguales","Mensaje")
                return false;
            }
            else if($("#correo").val()==''){
                $("#correo").parent().addClass("has-error");
                $("#correo").parent().addClass("has-feedback");
                $(".form-control-feedback[data-bv-icon-for=correo]").removeClass("icon icon-check");
                $(".form-control-feedback[data-bv-icon-for=correo]").addClass("icon icon-cross");
                $(".form-control-feedback[data-bv-icon-for=correo]").show();
                toastr.error("El campo de E-mail No puede estar vacío","Mensaje")
                return false;
            }
            else if($("#correo2").val()==''){
                $("##correo2").parent().addClass("has-error");
                $("##correo2").parent().addClass("has-feedback");
                $(".form-control-feedback[data-bv-icon-for=correo2]").removeClass("icon icon-check");
                $(".form-control-feedback[data-bv-icon-for=correo2]").addClass("icon icon-cross");
                $(".form-control-feedback[data-bv-icon-for=correo2]").show();
                toastr.error("El campo de confirmacion de E-mail no puede estar vacío","Mensaje")
                return false;
            }
            else {
                var dataForm = $('#complaintsForm').serializeFormJSON();
                $.ajax({
                    url: basePath + "Reclamacion/ReclamacionNuevoJson",
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
    
                            toastr.success(response.mensaje, "Mensaje Servidor");
                            setTimeout(function () {
                               window.location.reload(true);
                            }, 2500);
    
                        }
                        else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrow) {
                        toastr.error("Error Servidor", "Mensaje Servidor");
                    }
                });
            }
        }
        else {
            toastr.error("Complete los Campos Obligatorios", "Mensaje Servidor");
        }
       
    });

});
$("#complaintsForm")
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
            direccion: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            documento: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            telefono: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            correo: {
                validators: {
                    emailAddress: {
                        message: ''
                    }, 
                }
            },
            correo2: {
                validators: {
                    emailAddress: {
                        message: ''
                    },
                },
            },
            descripcion: {
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
            detalle: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            pedido: {
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
        let $parent = data.element.parents('.form-group');
        // Remove the has-success class
        $parent.removeClass('has-success');
        // Hide the success icon
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });
    function ObtenerImgDrive(RutaArchivoLogo){
        let dataForm={ RutaArchivoLogo:RutaArchivoLogo}
        return $.ajax({
            type: "POST",
            url: basePath + "Sala/GetImgPorIdDrive",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                // $.LoadingOverlay("show");
            },
            success: function (result) {
                return result
            },
            error: function (request, status, error) {
            },
            complete: function (resul) {
                // $.LoadingOverlay("hide");
            }
        });
    }
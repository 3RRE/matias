$(document).ready(function(){
    ObtenerListaSalas();
    ObtenerListaTipoOcurrencia();
    ObtenerListaTipoDocumento();
    $('#fechaOcurrencia').val(moment().format("DD-MM-YYYY"));
    $('#horaOcurrencia').val(moment().format("hh:mm:ss A"));
    $("#fechaOcurrencia").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        autoclose: true,
    });
    $("#horaOcurrencia").datetimepicker({
        format: 'hh:mm:ss A',
        autoclose: true,
        pickDate: false
    });
    $(document).on('keypress','.UpperCase',function (event) {
        $input=$(this);
        setTimeout(function () {
         $input.val($input.val().toUpperCase());
        },0);
    })
    $(document).on('keypress','.Number',function (event) {
        var regex = new RegExp("^[0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    })
    $(document).on('click','.btnGuardar',function(e){
        e.preventDefault();
        $("#form_registro_ocurrencia").data('bootstrapValidator').resetForm();
        let validar = $("#form_registro_ocurrencia").data('bootstrapValidator').validate();
        if(validar.isValid()){
            let dataForm = $("#form_registro_ocurrencia").serializeForm();
            dataForm["Fecha"]=dataForm["fechaOcurrencia"] + " "+dataForm["horaOcurrencia"];
            $.ajax({
                url: basePath + "Ocurrencias/GuardarOcurrenciaJson",
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
                    if(response.respuesta){
                        toastr.success(response.mensaje,"Mensaje Servidor")
                        setTimeout(function () {
                            window.location.reload();
                        }, 2000);
                    }else{
                        toastr.error(response.mensaje,"Mensaje Servidor")
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    console.log(errorThrow)
                }
            });
            // $.ajax({
            //     url: basePath + "Ocurrencias/GuardarOcurrenciaJson",
            //     type: "POST",
            //     cache: false,
            //     contentType: "application/json",
            //     data: JSON.stringify(dataForm),
            //     beforeSend: function () {
            //         $.LoadingOverlay("show");
            //     },
            //     complete: function () {
            //         $.LoadingOverlay("hide");
            //     },
            //     success: function (response) {
            //         console.log(response);
            //     },
            //     error: function (xmlHttpRequest, textStatus, errorThrow) {
            //         console.log(textStatus)
            //     }
            // });
        }
    })
})
function ObtenerListaSalas() {
    $.ajax({
        type: "POST",
        url: basePath + "Ocurrencias/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            $("#cboSala").append('<option value="">---Seleccione---</option>');
            $.each(datos, function (index, value) {
                $("#cboSala").append(`<option value="${value.CodSala}">${value.Nombre}</option>`);
            });
            $("#cboSala").select2({
                placeholder: "--Seleccione--",
            });
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
function ObtenerListaTipoOcurrencia() {
    $.ajax({
        type: "POST",
        url: basePath + "Ocurrencias/GetListadoTipoOcurrenciaJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            $("#cboTipoOcurrencia").append('<option value="">---Seleccione---</option>');
            $.each(datos, function (index, value) {
                $("#cboTipoOcurrencia").append(`<option value="${value.Id}">${value.Nombre}</option>`);
            });
            $("#cboTipoOcurrencia").select2({
                placeholder: "--Seleccione--",
            });
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
function ObtenerListaTipoDocumento() {
    $.ajax({
        type: "POST",
        url: basePath + "Ocurrencias/TipoDocumentoListarJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            $("#cboTipoDocumento").append('<option value="">---Seleccione---</option>');
            $.each(datos, function (index, value) {
                $("#cboTipoDocumento").append(`<option value="${value.DOIID}">${value.DESCRIPCION.toUpperCase()}</option>`);
            });
            $("#cboTipoDocumento").select2({
                placeholder: "--Seleccione--",
            });
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
$("#form_registro_ocurrencia")
.bootstrapValidator({
    container: '#messages',
    excluded: [':disabled', ':hidden', ':not(:visible)'],
    // feedbackIcons: {
    //     valid: 'icon icon-check',
    //     invalid: 'icon icon-cross',
    //     validating: 'icon icon-refresh'
    // },
    fields: {
        Nombres: {
            validators: {
                notEmpty: {
                    message: ''
                },  
            }
        },
        ApelPat: {
            validators: {
                notEmpty: {
                    message: ''
                },  
            }
        },
        ApelMat: {
            validators: {
                notEmpty: {
                    message: ''
                },  
            }
        },
        TipoDocId: {
            validators: {
                notEmpty: {
                    message: ''
                },  
            }
        },
        NroDoc: {
            validators: {
                notEmpty: {
                    message: ''
                },  
            }
        },
        CodSala: {
            validators: {
                notEmpty: {
                    message: ''
                },  
            }
        },
        JefeSala: {
            validators: {
                notEmpty: {
                    message: ''
                },  
            }
        },
        SeInformoA: {
            validators: {
                notEmpty: {
                    message: ''
                },  
            }
        },
        TipoOcurrenciaId: {
            validators: {
                notEmpty: {
                    message: ''
                },  
            }
        },
        Descripcion: {
            validators: {
                notEmpty: {
                    message: ''
                },  
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
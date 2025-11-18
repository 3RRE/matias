$(document).ready(function(){
    $("#checkbox").prop('checked', false);;
    $(".mySelect").select2({
        multiple: false, placeholder: "--Seleccione--"
    })
    ObtenerListaSalas();
    $(document).on('change','#cboTipoCorreo',function(e){
        e.preventDefault();
 
        let CodTipoCorreo=$(this).val();//1.- Remitente 2.- Destinatario

        $("#divInformacionServidor input").val("");
        $("#cboSSL").val(1).trigger("change");
        if(CodTipoCorreo==1){
            $("#divInformacionServidor").show();
        }
        else{
            $("#divInformacionServidor").hide();
        }
    })
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
        $("#form_registro_correo").data('bootstrapValidator').resetForm();
        let validar = $("#form_registro_correo").data('bootstrapValidator').validate();
        if(validar.isValid()){
            let dataForm = $("#form_registro_correo").serializeForm();
            console.log(dataForm);
            $.ajax({
                url: basePath + "Ocurrencias/GuardarCorreoJson",
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
                        let url=basePath + "Ocurrencias/ListadoCorreo";
                        setTimeout(function () {
                            window.location.href=url;
                        }, 2000);
                    }else{
                        toastr.error(response.mensaje,"Mensaje Servidor")
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                }
            });
        }
    })
    $(document).on('click','#checkbox',function(e){
        if($("#checkbox").is(':checked') ){
            $("#cboSala > option").prop("selected","selected");
        }else{
            $("#cboSala > option").removeAttr("selected");
        }
        $("#cboSala").trigger("change");
    })
    $(document).on('change','#cboSala',function(e){
        e.preventDefault()
        let data=$(this).val()
        if(data){
            let allOptions = $(this).find('option');
            if(data.length==allOptions.length){
                $("#checkbox").prop('checked', true);;
            }
            else{
                $("#checkbox").prop('checked', false);;
            }
        }
    })
    $('.btnListar').on('click', function (e) {
        window.location.replace(basePath + "Ocurrencias/ListadoCorreo");
    });
})
function ObtenerListaSalas() {
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
            // $("#cboSala").append('<option value="all">Seleccionar Todo</option>');
            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                placeholder: "--Seleccione--", allowClear: true
            });
            // $("#cboSala").val(null).trigger("change");
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
$("#form_registro_correo")
.bootstrapValidator({
    container: '#messages',
    excluded: [':disabled', ':hidden', ':not(:visible)'],
    // feedbackIcons: {
    //     valid: 'icon icon-check',
    //     invalid: 'icon icon-cross',
    //     validating: 'icon icon-refresh'
    // },
    fields: {
        Nombre: {
            validators: {
                notEmpty: {
                    message: ''
                },  
            }
        },
        Email: {
            validators: {
                // notEmpty: {
                //     message: ''
                // },  
                emailAddress: {
                    message: ''
                },
                callback: {
                    callback: function(value, validator, $field) {
                        let returned=true
                        if (value === '') {
                            returned = false
                        }
                        let emailRegex = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
                        if (!emailRegex.test(value)) {
                            returned=false
                        } 
                        return returned
                    }
                }
            }
        },
        'arraySalas': {
            validators: {
                notEmpty: {
                    message: ''
                },
            }
        },
        CodTipoCorreo: {
            validators: {
                notEmpty: {
                    message: ''
                },  
            }
        },
        Password: {
            validators: {
                callback: {
                    callback: function(value, validator, $field) {
                        let CorreoTipo = $("#cboTipoCorreo option:selected").val();
                        let returned=false;
                        if(CorreoTipo==2){
                            returned=true
                        }
                        else{
                            if(value==""){
                                returned=false
                            }
                            else{
                                returned=true
                            }
                        }
                        return returned
                    }
                }
            }
        },
        Puerto: {
            validators: {
                callback: {
                    callback: function(value, validator, $field) {
                        let CorreoTipo = $("#cboTipoCorreo option:selected").val();
                        let returned=false;
                        if(CorreoTipo==2){
                            returned=true
                        }
                        else{
                            if(value==""){
                                returned=false
                            }
                            else{
                                returned=true
                            }
                        }
                        return returned
                    }
                }
            }
        },
        Smtp: {
            validators: {
                callback: {
                    callback: function(value, validator, $field) {
                        let CorreoTipo = $("#cboTipoCorreo option:selected").val();
                        let returned=false;
                        if(CorreoTipo==2){
                            returned=true
                        }
                        else{
                            if(value==""){
                                returned=false
                            }
                            else{
                                returned=true
                            }
                        }
                        return returned
                    }
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
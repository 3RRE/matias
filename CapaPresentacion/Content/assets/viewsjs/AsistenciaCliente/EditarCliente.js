var Ubigeo_Pais_Id = 'PE';

$(document).ready(function () {
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
    $(".dateFechaEmision").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        maxDate: dateNow,
    });
    $(".dateFechaDosis").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        maxDate: dateNow,
    });
    $("#Id").val(cliente.Id);
    $("#NroDoc").val(cliente.NroDoc);
    $("#NombreCompleto").val(cliente.NombreCompleto);
    $("#Nombre").val(cliente.Nombre);
    $("#ApelPat").val(cliente.ApelPat);
    $("#ApelMat").val(cliente.ApelMat);
    $("#Celular1").val(cliente.Celular1);
   
    $("#Celular2").val(cliente.Celular2);
    $("#Mail").val(cliente.Mail);
   
    $("#cboGenero").val(cliente.Genero);
    $("#FechaNacimiento").val(moment(cliente.FechaNacimiento).utcOffset(-5).format('DD/MM/YYYY'));
    $(".TieneNombreCompleto").hide();
    $(".NoTieneNombreCompleto").hide();
    $("#nrodosis").val(cliente.nro_dosis);
    let fecha_ultima_dosis=moment(cliente.fecha_ultima_dosis).format('DD/MM/YYYY')
    if(fecha_ultima_dosis=='31/12/1752'){
        $("#fechadosis").val();
    }
    else{
        $("#fechadosis").val(fecha_ultima_dosis);
    }
    let fecha_emision=moment(cliente.fecha_emision).format('DD/MM/YYYY')
    console.log(fecha_emision)
    if(fecha_emision=='31/12/1752'||fecha_emision=='31/12/0000'){
        $("#fechaemision").val();
    }
    else{
        $("#fechaemision").val(fecha_emision);
    }
    ObtenerListaSalas(cliente.sala_vacunacion);

    if(cliente.ApelPat!=''){
        $(".NoTieneNombreCompleto").show();
    }
    else{
        $(".TieneNombreCompleto").show();

    }
    // if(cliente.NombreCompleto==''){
    //     $(".NoTieneNombreCompleto").show()
    // }
    // else{
    //     $(".TieneNombreCompleto").show()
    // }
    $(".mySelect").select2({
        multiple: false, placeholder: "--Seleccione--"
    })
    if (buscar) {
        var celucrip = maskara(cliente.Celular1);
        $("#Celular1_").val(celucrip);
        var mailcrip = maskara(cliente.Mail);
        $("#Mail_").val(mailcrip);
    }

    ObtenerDataSelects();

    $(document).on('change','#cboDepartamento',function(e){
        e.preventDefault()
        let DepartamentoID = $("#cboDepartamento option:selected").val();
        ObtenerListaProvincias(DepartamentoID);
    })
    $(document).on('change','#cboProvincia',function(e){
        e.preventDefault()
        let DepartamentoID = $("#cboDepartamento option:selected").val();
        let ProvinciaID = $("#cboProvincia option:selected").val();
        ObtenerListaDistritos(DepartamentoID,ProvinciaID)
    })
    $('.btnListar').on('click', function (e) {
        window.location.replace(basePath + "AsistenciaCliente/ListadoCliente");
    });
    $(document).on('click','.btnGuardar',function(e){
        e.preventDefault();
        $("#form_registro_cliente").data('bootstrapValidator').resetForm();
        let validar = $("#form_registro_cliente").data('bootstrapValidator').validate();
        if(validar.isValid()){
            let dataForm = $("#form_registro_cliente").serializeForm();
            // let UbigeoProcDepartamento=$("#cboDepartamento :selected").text();
            // let UbigeoProcProvincia=$("#cboProvincia :selected").text();
            // let UbigeoProcDistrito=$("#cboDistrito :selected").text();
            // dataForm["UbigeoProcDepartamento"]=UbigeoProcDepartamento;
            // dataForm["UbigeoProcProvincia"]=UbigeoProcProvincia;
            // dataForm["UbigeoProcDistrito"]=UbigeoProcDistrito;

            var CodigoUbigeo = $("#cboPais").find(':selected').attr('data-codubigeo')

            dataForm["CodigoUbigeo"] = CodigoUbigeo

            $.ajax({
                url: basePath + "AsistenciaCliente/EditarClienteJson",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(dataForm),
                beforeSend: function () {
                    $.LoadingOverlay("show")
                },
                complete: function () {
                    $.LoadingOverlay("hide")
                },
                success: function (response) {
                    if(response.respuesta){
                        toastr.success(response.mensaje,"Mensaje Servidor")
                        setTimeout(function () {
                            window.location.href=basePath + "AsistenciaCliente/EditarCliente/" + cliente.Id;;
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

    $(document).on('keypress', '#NroDoc', function (event) {
        var tipo = $("#cboTipoDocumento").val();
        if (tipo == 3) {
            var regex = new RegExp("^[a-zA-Z0-9-_]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        } else {
            var regex = new RegExp("^[0-9]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        }

    })
    $("#cboTipoDocumento").change(function () {
        $("#NroDoc").val("");
    });

    $(document).on('keypress','#Celular1,#Celular2',function (event) {
        var regex = new RegExp("^[0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    })
    $(document).on('keypress','#Nombre,#ApelPat,#ApelMat',function (event) {
        var regex = new RegExp("^[a-zA-Z ]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    })
    $(document).on('keypress','.UpperCase',function (event) {
        $input=$(this);
        setTimeout(function () {
         $input.val($input.val().toUpperCase());
        },50);
    })

    $(document).on('change', '#cboPais', function (event) {
        event.preventDefault()

        var paisId = $(this).val()

        listenerChangeCountry(paisId)
    })
    
})
function ObtenerDataSelects(){
    let dataForm={ ubigeo:ubigeo}
    $.ajax({
        type: "POST",
        url: basePath + "AsistenciaCliente/GetDataSelects",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data:JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.respuesta) {

                var dataPaises = result.data.dataPaises;

                let dataUbigeo=result.data.dataUbigeo;
                let dataTipoCliente=result.data.dataTipoCliente;
                let dataTipoFrecuencia=result.data.dataTipoFrecuencia;
                let dataTipoDocumento=result.data.dataTipoDocumento;
                let dataProvincias=result.data.dataProvincias;
                let dataDistritos=result.data.dataDistritos;
                let dataTipoJuego = result.data.dataTipoJuego;

                if (dataPaises.length > 0) {
                    $.each(dataPaises, function (index, value) {
                        $("#cboPais").append(`<option value="${value.PaisId}" data-codubigeo="${value.CodUbigeo}">${value.Nombre}</option>`)
                    });
                }

                if(dataUbigeo.length>0){
                    $.each(dataUbigeo, function (index, value) {
                        $("#cboDepartamento").append('<option value="' + value.DepartamentoId + '"  >' + value.Nombre + '</option>');
                    });
                    if(dataProvincias&&dataDistritos){
                        $.each(dataProvincias, function (index, value) {
                            $("#cboProvincia").append('<option value="' + value.ProvinciaId + '">' + value.Nombre + '</option>');
                        });
                        $.each(dataDistritos, function (index, value) {
                            $("#cboDistrito").append('<option value="' + value.CodUbigeo + '"  >' + value.Nombre + '</option>');
                        });
                    }
                }
                if(dataTipoCliente.length>0){
                    $.each(dataTipoCliente, function (index, value) {
                        $("#cboTipoCliente").append('<option value="' + value.Id + '"  >' + value.Nombre + '</option>');
                    });
                }
                if(dataTipoFrecuencia.length>0){
                    $.each(dataTipoFrecuencia, function (index, value) {
                        $("#cboTipoFrecuencia").append('<option value="' + value.Id + '"  >' + value.Nombre + '</option>');
                    });
                }
                if(dataTipoDocumento.length>0){
                    $.each(dataTipoDocumento, function (index, value) {
                        $("#cboTipoDocumento").append('<option value="' + value.Id + '"  >' + value.Nombre + '</option>');
                    });  
                }
                if(dataTipoJuego.length>0){
                    $.each(dataTipoJuego, function (index, value) {
                        $("#cboTipoJuego").append('<option value="' + value.Id + '"  >' + value.Nombre + '</option>');
                    });  
                }
            }
            else{
                toastr.error(result.mensaje,"Mensaje Servidor");
            }
            console.log(ubigeo.DepartamentoId)
            if(ubigeo.DepartamentoId!=0){
                $("#cboDepartamento").val(ubigeo.DepartamentoId);
                $("#cboProvincia").val(ubigeo.ProvinciaId);
                $("#cboDistrito").val(ubigeo.CodUbigeo);
            }
            else{
                $("#cboDepartamento").val("");
            }
            $("#cboTipoDocumento").val(cliente.TipoDocumentoId)
            $("#cboTipoCliente").val(cliente.ClienteSala.TipoClienteId)
            $("#cboTipoFrecuencia").val(cliente.ClienteSala.TipoFrecuenciaId)
            $("#cboTipoJuego").val(cliente.ClienteSala.TipoJuegoId)
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
            setCountry(cliente.PaisId)
        }
    });
    return false;
}
function ObtenerListaProvincias(DepartamentoID) {
    if(DepartamentoID){
        let dataForm={ DepartamentoID:DepartamentoID}
        $.ajax({
            type: "POST",
            url: basePath + "AsistenciaCliente/GetListadoProvincia",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                $("#cboProvincia,#cboDistrito").html("");
                $("#cboProvincia,#cboDistrito").append('<option value="">---Seleccione---</option>');
                if(result.respuesta){
                    var datos = result.data;
                    $.each(datos, function (index, value) {
                        $("#cboProvincia").append('<option value="' + value.ProvinciaId + '"  >' + value.Nombre + '</option>');
                    });
                }
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
  
}
function ObtenerListaDistritos(DepartamentoID,ProvinciaID) {
    if(DepartamentoID&&ProvinciaID){
        let dataForm={ DepartamentoID:DepartamentoID,ProvinciaID:ProvinciaID}
        $.ajax({
            type: "POST",
            url: basePath + "AsistenciaCliente/GetListadoDistrito",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                $("#cboDistrito").html("");
                $("#cboDistrito").append('<option value="">---Seleccione---</option>');
                if(result.respuesta){
                    var datos = result.data;
                    $.each(datos, function (index, value) {
                        $("#cboDistrito").append('<option value="' + value.CodUbigeo + '"  >' + value.Nombre + '</option>');
                    });
                
                }
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
  
};


function ObtenerListaSalas(sala_vacunacion) {
    $("#cboSalaVacunacion").html("");
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaActivasSinSeguridad",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                var select = "";
                if (value.CodSala == sala_vacunacion) {
                    select = "selected";
                }
                $("#cboSalaVacunacion").append('<option ' + select+' value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSalaVacunacion").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: true
            });
            //$("#cboSalaVacunacion").val(null).trigger("change");
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

function setCountry(paisId) {

    $('#cboPais').val(paisId).trigger('change')

    listenerChangeCountry(paisId)
}

function listenerChangeCountry(paisId) {

    var disabled = paisId != Ubigeo_Pais_Id ? true : false
    var cboDepartamento = $('#cboDepartamento')
    var cboProvincia = $('#cboProvincia')
    var cboDistrito = $('#cboDistrito')

    cboDepartamento.prop('disabled', disabled)
    cboProvincia.prop('disabled', disabled)
    cboDistrito.prop('disabled', disabled)

    if (disabled) {
        cboDepartamento.val("").trigger('change')
        cboProvincia.val("").trigger('change')
        cboDistrito.val("").trigger('change')

        removeHasError(cboDepartamento)
        removeHasError(cboProvincia)
        removeHasError(cboDistrito)
    }
}

function removeHasError(element) {
    element.parent('.has-feedback').removeClass('has-error').find('.form-control-feedback').css('display', 'none')
}

$("#form_registro_cliente")
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
        TipoDocumentoId: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
        NroDoc: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        }
        ,
        Nombre: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
        Genero: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
        Mail: {
            validators: {
                //notEmpty: {
                //    message: ''
                //}
            }
        },
        Celular1: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
        FechaNacimiento: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
        //'ClienteSala.TipoClienteId': {
        //    validators: {
        //        notEmpty: {
        //            message: ''
        //        }
        //    }
        //},
        //'ClienteSala.TipoFrecuenciaId': {
        //    validators: {
        //        notEmpty: {
        //            message: ''
        //        }
        //    }
        //},
        DepartamentoId: {
            validators: {
                //notEmpty: {
                //    message: ''
                //}
            }
        },
        ProvinciaId: {
            validators: {
                //notEmpty: {
                //    message: ''
                //}
            }
        },
        UbigeoProcedenciaId: {
            validators: {
                //notEmpty: {
                //    message: ''
                //}
            }
        },
        sala_vacunacion: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
        nro_dosis: {
            validators: {
                notEmpty: {
                    message: ''
                },
            }
        },
        Mail_: {
            validators: {
                //notEmpty: {
                //    message: ''
                //}
            }
        },
        Celular1_: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
        fecha_ultima_dosis: {
            validators: {
                //notEmpty: {
                //    message: ''
                //}
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


function maskara(str) {
    var censored=""
    if (str.length < 5) {
        censored = str.slice(0, 3) + '*'.repeat(str.length).slice(-3);
    }
    else {
        censored = '*'.repeat(str.length);
    }
    return censored;
}


$(function () {
    //Extends JQuery Methods to get current cursor postion in input text.
    //GET CURSOR POSITION
    jQuery.fn.getCursorPosition = function () {
        if (this.lengh == 0) return -1;
        return $(this).getSelectionStart();
    }

    jQuery.fn.getSelectionStart = function () {
        if (this.lengh == 0) return -1;
        input = this[0];

        var pos = input.value.length;

        if (input.createTextRange) {
            var r = document.selection.createRange().duplicate();
            r.moveEnd('character', input.value.length);
            if (r.text == '') pos = input.value.length;
            pos = input.value.lastIndexOf(r.text);
        } else if (typeof (input.selectionStart) != "undefined") pos = input.selectionStart;

        return pos;
    }

    //Bind Key Press event with password field    
    $("#Mail_").keypress(function (e) {
        setTimeout(function () {
            maskPassword(e)
        }, 500);
    });

    $("#Mail_").keydown(function (e) {
        if (e.keyCode == 8) {
            setTimeout(function () {
                maskPassword(e)
            }, 1);
        }
    });

    $("#Mail_").bind("paste", function (e) {
        setTimeout(function () {
            maskPassword(e)
        }, 500);
    });


    //Bind Key Press event with password field    
    $("#Celular1_").keypress(function (e) {
        setTimeout(function () {
            maskPasswordCelu(e)
        }, 500);
    });

    $("#Celular1_").keydown(function (e) {
        if (e.keyCode == 8) {
            setTimeout(function () {
                maskPasswordCelu(e)
            }, 1);
        }
    });

    $("#Celular1_").bind("paste", function (e) {
        setTimeout(function () {
            maskPasswordCelu(e)
        }, 500);
    });


});

function generateStars(n) {
    var stars = '';
    for (var i = 0; i < n; i++) {
        stars += '*';
    }
    return stars;
}

function maskPassword(e) {
    var text = $('#Mail').val();
    var stars = $('#Mail').val().length;
    var unicode = e.keyCode ? e.keyCode : e.charCode;
    //$("#keycode").html(unicode);

    //Get Current Cursor Position on Password Textbox
    var curPos = $("#Mail_").getCursorPosition();
    var PwdLength = $("#Mail_").val().length;

    if (unicode != 9 && unicode != 13 && unicode != 37 && unicode != 40 && unicode != 37 && unicode != 39) {
        //If NOT <Back Space> OR <DEL> Then...
        if (unicode != 8 && unicode != 46) {
            text = text + String.fromCharCode(unicode);
            stars += 1;
        }
        //If Press <Back Space> Or <DEL> Then...
        else if ((unicode == 8 || unicode == 46) && stars != PwdLength) {
            stars -= 1;
            text = text.substr(0, curPos) + text.substr(curPos + 1);
        }
        //Set New String on both input fields
        $('#Mail').val(text);
        $('#Mail_').val(generateStars(stars));
    }
}

function maskPasswordCelu(e) {
    var text = $('#Celular1').val();
    var stars = $('#Celular1').val().length;
    var unicode = e.keyCode ? e.keyCode : e.charCode;
    $("#keycode").html(unicode);

    //Get Current Cursor Position on Password Textbox
    var curPos = $("#Celular1_").getCursorPosition();
    var PwdLength = $("#Celular1_").val().length;

    if (unicode != 9 && unicode != 13 && unicode != 37 && unicode != 40 && unicode != 37 && unicode != 39) {
        //If NOT <Back Space> OR <DEL> Then...
        if (unicode != 8 && unicode != 46) {
            text = text + String.fromCharCode(unicode);
            stars += 1;
        }
        //If Press <Back Space> Or <DEL> Then...
        else if ((unicode == 8 || unicode == 46) && stars != PwdLength) {
            stars -= 1;
            text = text.substr(0, curPos) + text.substr(curPos + 1);
        }
        //Set New String on both input fields
        $('#Celular1').val(text);
        $('#Celular1_').val(generateStars(stars));
    }
}
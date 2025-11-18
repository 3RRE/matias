var Ubigeo_Pais_Id = 'PE';

$(document).ready(function () {
    console.log(nombre)
    $("#Nombre").val(nombre)
    $("#ApelPat").val(apelpat)
    $("#ApelMat").val(apelmat)
    $("#NroDoc").val(nrodoc)
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
    $(".mySelect").append('<option value="">---Seleccione---</option>');
    $(".mySelect").select2({
        multiple: false, placeholder: "--Seleccione--"
    })
    ObtenerDataSelects();
    ObtenerListaSalas()

    $(document).on('click','.btnGuardar',function(e){
        e.preventDefault();
        $("#form_registro_cliente").data('bootstrapValidator').resetForm();
        let validar = $("#form_registro_cliente").data('bootstrapValidator').validate();
        if(validar.isValid()){
            let dataForm = $("#form_registro_cliente").serializeForm();

            var UbigeoProcPais = $("#cboPais :selected").text();
            let UbigeoProcDepartamento=$("#cboDepartamento :selected").text();
            let UbigeoProcProvincia=$("#cboProvincia :selected").text();
            let UbigeoProcDistrito = $("#cboDistrito :selected").text();

            var CodigoUbigeo = $("#cboPais").find(':selected').attr('data-codubigeo')

            dataForm["UbigeoProcPais"] = UbigeoProcPais;
            dataForm["UbigeoProcDepartamento"]=UbigeoProcDepartamento;
            dataForm["UbigeoProcProvincia"]=UbigeoProcProvincia;
            dataForm["UbigeoProcDistrito"] = UbigeoProcDistrito;

            dataForm["CodigoUbigeo"] = CodigoUbigeo

            dataForm["EnviaNotificacionWhatsapp"] = $('#chkNotificacionWhatsapp').prop('checked');
            dataForm["EnviaNotificacionSms"] = $('#chkNotificacionSms').prop('checked');
            dataForm["EnviaNotificacionEmail"] = $('#chkNotificacionEmail').prop('checked');
            dataForm["LlamadaCelular"] = $('#chkLlamadaCelular').prop('checked');
            $.ajax({
                url: basePath + "AsistenciaCliente/GuardarCliente",
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
                        let url='';
                        if(redirect){
                            url = basePath+"AsistenciaCliente/RegistroAsistencia"
                            localStorage.removeItem("IdClienteInsertado")
                            localStorage.setItem("IdClienteInsertado",response.idInsertado)
                        }
                        else{
                            url = basePath + "AsistenciaCliente/ListadoCliente";
                        }
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
        },0);
    })

    $(document).on('change', '#cboPais', function (event) {
        event.preventDefault()

        var paisId = $(this).val()

        listenerChangeCountry(paisId)
    })
})
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
                        $("#cboProvincia").append('<option value="' + value.ProvinciaId + '">' + value.Nombre + '</option>');
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
                        $("#cboDistrito").append('<option value="' + value.CodUbigeo + '">' + value.Nombre + '</option>');
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
function ObtenerDataSelects(){
    $.ajax({
        type: "POST",
        url: basePath + "AsistenciaCliente/GetDataSelects",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if(result.respuesta){

                var dataPaises = result.data.dataPaises;

                let dataUbigeo=result.data.dataUbigeo;
                let dataTipoCliente=result.data.dataTipoCliente;
                let dataTipoFrecuencia=result.data.dataTipoFrecuencia;
                let dataTipoDocumento=result.data.dataTipoDocumento;
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
                    if(tipodoc=1){
                        $("#cboTipoDocumento").val(tipodoc)
                    }
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
            // $(".mySelect").select2({
            //     multiple: false,placeholder: "--Seleccione--"
            // });
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
            setCountry(Ubigeo_Pais_Id)
        }
    });
    return false;
}


function ObtenerListaSalas() {
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
                $("#cboSalaVacunacion").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSalaVacunacion").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboSalaVacunacion").val(null).trigger("change");
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
        ApelPat: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
        ApelMat: {
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
                 //},  
                
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
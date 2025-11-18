
$(document).ready(function () {
    $("#frmRegistroSala")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                Nombre: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese nombre, Obligatorio'
                        }
                    }
                },
                NombreCorto: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese nombre corto, Obligatorio'
                        }
                    }
                }, 
                latitud: {
                    validators: {
                        regexp: {
                            regexp: /^[\-\+]?([0-8]?\d{1}|[0-8]?\d{1}\.\d{1,15}|90|90\.0{1,15})$/
                        }
                    }
                },
                longitud: {
                    validators: {
                        regexp: {
                            regexp: /^[\-\+]?(0?\d{1,2}|0?\d{1,2}\.\d{1,15}|1[0-7]?\d{1}|1[0-7]?\d{1}\.\d{1,15}|180|180\.0{1,15})$/
                        }
                    }
                },
                DepartamentoId: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                ProvinciaId: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                CodUbigeo: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                PuertoSignalr: {
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
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
    $("#RutaArchivoLogoAnt").val(sala.RutaArchivoLogo)
    $("#txtCodSala").val(sala.CodSala);
    $("#txtCodSala_").val(sala.CodSala);
    $("#txtNombre").val(sala.Nombre);
    $("#txtNombreCorto").val(sala.NombreCorto);
    $("#txtUrlProgresivo").val(sala.UrlProgresivo);
    $("#txtUrlBoveda").val(sala.UrlBoveda);
    $("#txtUrlSalaOnline").val(sala.UrlSalaOnline);
    $("#txtlatitud").val(sala.latitud);
    $("#txtlongitud").val(sala.longitud);
    $("#txtcorreo").val(sala.correo)
    $("#Direccion").val(sala.Direccion)
    $("#CodEmpresa").val(sala.CodEmpresa)
    $("#FechaAniversario").val(moment(sala.FechaAniversario).format('DD/MM/YYYY'))
    $("#EmpresaRazonSocial").val(sala.Empresa.RazonSocial)
    $("#cboTipo").val(sala.tipo)
    $("#cboSalaMaestra").val(sala.codSalaMaestra)
    $("#IpPublica").val(sala.IpPublica)
    $("#PuertoSignalr").val(sala.PuertoSignalr)
    $(".mySelect").select2({
        multiple: false, placeholder: "--Seleccione--"
    })
    ObtenerDataSelects();
    $.when(VistaAuditoria("Sala/SalaModificarVista", "VISTA", 1, "#frmRegistroSala",3)).then(function (response, textStatus) {
        dataAuditoria(0, "#frmRegistroSala",3);
    });
    if (sala.RutaArchivoLogo) {

        /*
        let logo = ObtenerImgDrive(sala.RutaArchivoLogo)
        logo.then(response=>{
            $("#LogoSala").attr('src','data:image/png;base64,'+response.data)
        })*/

        var logoSala = sala.RutaArchivoLogo != "" ? "https://drive.google.com/uc?id=" + sala.RutaArchivoLogo : "";
        $("#LogoSala").attr('src', logoSala)

    }
   
    RutaArchivoLogo.onchange = evt => {
        const [file] = RutaArchivoLogo.files
        if (file) {
            LogoSala.src = URL.createObjectURL(file)
        }
    }
    // $(document).on('change','#RutaArchivoLogo',function (e) {
    //     const [file]=this.files[0]
    //     if(file){
    //         // blah.src = URL.createObjectURL(file)
    //         $("#LogoSala").attr('src',URL.createObjectURL(file))
    //     }
    // })
    // $('.btnGuardar').on('click', function (e) {
    //     $("#frmRegistroSala").data('bootstrapValidator').resetForm();
    //     var validar = $("#frmRegistroSala").data('bootstrapValidator').validate();
    //     if (validar.isValid()) {
    //         var url = basePath + "Sala/SalaModificarJson";
    //         var redirectUrl = basePath + "Sala/SalaVista";
    //         var dataForm = $('#frmRegistroSala').serializeFormJSON();
    //         $.when(dataAuditoria(1, "#frmRegistroSala", 3, "Sala/SalaModificarJson", "ACTUALIZAR SALA")).then(function (response, textStatus) {
    //             enviarDataPost(url, dataForm, false, false, redirectUrl, false);
               
    //         });
            
    //     }

    // });
    $('.btnGuardar').on('click', function (e) {
        let permitido=4194304;
        let file=$('#RutaArchivoLogo')[0].files[0]
        let archivoValidado=false
        if(file){
            let fileSize = file.size;
            if(fileSize<permitido){
                archivoValidado=true
            }
        }
        else{
            //no se edito el logo
            archivoValidado=true
        }

        //Correo validar
        let correoValidar = $("#txtcorreo").val();
        if (correoValidar.length > 0) {
            let regex = new RegExp("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$");
            if (!regex.test(correoValidar)) {
                //event.preventDefault();
                toastr.error("Correo invalido", "Mensaje Servidor")
                return false;
            }
        }

        //envio post
        if(archivoValidado){
            let validar = $("#frmRegistroSala").data('bootstrapValidator').validate();
            if(validar.isValid()){
                let dataForm = new FormData(document.getElementById("frmRegistroSala"));
                $.ajax({
                    url: basePath + "Sala/SalaModificarJson",
                    type: "POST",
                    method: "POST",
                    contentType: false,
                    data: dataForm,
                    cache: false,
                    processData: false,
                    beforeSend: function () {
                    },
                    complete: function () {
                    },
                    success: function (response) {
                        if(response.respuesta){
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
                                    let url = basePath + "Sala/SalaVista";
                                    window.location.href=url;
                                },
                                cancel: function () {
                                }
                            });
                            // toastr.success(response.mensaje,"Mensaje Servidor")
                            // setTimeout(function () {
                            //     window.location.reload()
                            // }, 2000);
                        }else{
                            toastr.error(response.mensaje,"Mensaje Servidor")
                        }
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrow) {
                    }
                });
            }
        }
        else{
            toastr.error("Error en logo","Mensaje Servidor")
        }
    });
    $('.btnListar').on('click', function (e) {
        window.location.replace(basePath + "Sala/SalaVista");
    });
    $(document).on('click', '.btnModal', function (e) {
        var nombre = $(this).data("nombre");
        $("#hddmodal").val(nombre);
        $('#bodyModal').html("");
        $('#default-modal-label').html("Mantenimiento " + nombre);
        var redirectUrl = basePath + "Sala/Mantenimiento" + nombre;
        var ubicacion = "bodyModal";
        DataPostModo1(redirectUrl, ubicacion);
        $("#modalGroup").modal("show");

    });
    $('#modalGroup').on('shown.bs.modal', function () {
        var nombre = $("#hddmodal").val();
        setTimeout('listar' + nombre + '()', 1);
    });
    $(document).on('keypress','.soloNumeros',function (event) {
        let regex = new RegExp("^[0-9]+$");
        let key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    })
    $(document).on('keypress','.soloTexto',function (event) {
        let regex = new RegExp("^[a-zA-Z ]+$");
        let key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
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
    
});
function ObtenerDataSelects(){
    let dataForm={ ubigeo:ubigeo}
    $.ajax({
        type: "POST",
        url: basePath + "Sala/GetDataSelects",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data:JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if(result.respuesta){
                let dataUbigeo=result.data.dataUbigeo;
                let dataProvincias=result.data.dataProvincias;
                let dataDistritos = result.data.dataDistritos;
                let dataSalasMaestras = result.data.dataSalasMaestras;
                $("#cboSalaMaestra").append('<option value=""></option>');
                $("#cboDepartamento").append('<option value=""></option>');
                $("#cboProvincia").append('<option value=""></option>');
                $("#cboDistrito").append('<option value=""></option>');
                if (dataSalasMaestras.length > 0) {
                    $.each(dataSalasMaestras, function (index, value) {
                        $("#cboSalaMaestra").append('<option value="' + value.CodSalaMaestra + '"  >' + value.Nombre + '</option>');
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
            }
            else{
                toastr.error(result.mensaje,"Mensaje Servidor");
            }
            if (sala.CodSalaMaestra) {
                $("#cboSalaMaestra").val(sala.CodSalaMaestra);
            }
            if(ubigeo.DepartamentoId!=0){
                $("#cboDepartamento").val(ubigeo.DepartamentoId);
                $("#cboProvincia").val(ubigeo.ProvinciaId);
                $("#cboDistrito").val(ubigeo.CodUbigeo);
            } else{
                $("#cboDepartamento").val("");
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
function ObtenerListaProvincias(DepartamentoID) {
    if(DepartamentoID){
        let dataForm={ DepartamentoID:DepartamentoID}
        $.ajax({
            type: "POST",
            url: basePath + "Sala/GetListadoProvincia",
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
                    let datos = result.data;
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
            url: basePath + "Sala/GetListadoDistrito",
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
                    let datos = result.data;
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
            $.LoadingOverlay("show");
        },
        success: function (result) {
            return result
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}


$(document).ready(function() {
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
    $(".mySelect").append('<option value="">---Seleccione---</option>');
    $(".mySelect").select2({
        multiple: false, placeholder: "--Seleccione--"
    })
    ObtenerDataSelects()
    RutaArchivoLogo.onchange = evt => {
        const [file] = RutaArchivoLogo.files
        if (file) {
            LogoSala.src = URL.createObjectURL(file)
        }
    }
    $(document).on('click','.btnGuardar', function (e) {
        e.preventDefault()
        /*
        let permitido=4194304;
        let file=$('#RutaArchivoLogo')[0].files[0]
        if(file){
            let fileSize = file.size;
            if(fileSize<permitido){
                let validar = $("#frmRegistroSala").data('bootstrapValidator').validate();
                if(validar.isValid()){
                    let dataForm = new FormData(document.getElementById("frmRegistroSala"));
                    console.log(dataForm)
                    $.ajax({
                        url: basePath + "Sala/InsertarSalaJson",
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
                                $("form input,select,textarea").val(""); 
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
                            }else{
                                toastr.error(response.mensaje,"Mensaje Servidor")
                            }
                        },
                        error: function (xmlHttpRequest, textStatus, errorThrow) {
                        }
                    });
                }
                
            }
        }
        else{
            toastr.warning("Seleccione una imagen para logo","Mensaje Servidor")
        }*/


        let validar = $("#frmRegistroSala").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            let dataForm = new FormData(document.getElementById("frmRegistroSala"));
            console.log(dataForm)
            $.ajax({
                url: basePath + "Sala/InsertarSalaJson",
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
                    if (response.respuesta) {
                        $("form input,select,textarea").val("");
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
                                window.location.href = url;
                            },
                            cancel: function () {
                            }
                        });
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor")
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                }
            });
        }




    })
    // $(document).on('click','.btnGuardar',function(e){
    //     console.log('click')
    //     e.preventDefault();
    //     $("#frmRegistroSala").data('bootstrapValidator').resetForm();
    //     let validar = $("#frmRegistroSala").data('bootstrapValidator').validate();
    //     if(validar.isValid()){
    //         let dataForm = $("#frmRegistroSala").serializeForm();
    //         console.log(dataForm);

    //         $.ajax({
    //             url: basePath + "Sala/InsertarSalaJson",
    //             type: "POST",
    //             contentType: "application/json",
    //             data: JSON.stringify(dataForm),
    //             beforeSend: function () {
    //             },
    //             complete: function () {
    //             },
    //             success: function (response) {
    //                 if(response.respuesta){
    //                     $("form input,select,textarea").val("");    
    //                     $.confirm({
    //                         icon: 'fa fa-spinner fa-spin',
    //                         title: 'Mensaje Servidor',
    //                         theme: 'black',
    //                         animationBounce: 1.5,
    //                         columnClass: 'col-md-6 col-md-offset-3',
    //                         confirmButtonClass: 'btn-info',
    //                         cancelButtonClass: 'btn-warning',
    //                         confirmButton: 'Ir a Listado ',
    //                         cancelButton: 'Seguir Registrando',
    //                         content: false,
    //                         confirm: function () {
    //                             let url = basePath + "Sala/SalaVista";
    //                             window.location.href=url;
    //                         },
    //                         cancel: function () {
    //                         }
    //                     });
    //                     // toastr.success(response.mensaje,"Mensaje Servidor")
    //                     // url = basePath + "Sala/SalaVista";
    //                     // setTimeout(function () {
    //                     //     window.location.href=url;
    //                     // }, 2000);
    //                 }else{
    //                     toastr.error(response.mensaje,"Mensaje Servidor")
    //                 }
    //             },
    //             error: function (xmlHttpRequest, textStatus, errorThrow) {
    //             }
    //         });
    //     }
    // })
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
    $('.btnListar').on('click', function (e) {
        window.location.replace(basePath + "Sala/SalaVista");
    });
    $('#PuertoSignalr').val('3602');
})
function ObtenerDataSelects(){
    $.ajax({
        type: "POST",
        url: basePath + "Sala/GetDataSelects",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if(result.respuesta){
                let dataUbigeo=result.data.dataUbigeo;
                let dataEmpresas=result.data.dataEmpresas;
                let dataSalasMaestras = result.data.dataSalasMaestras;
                if(dataUbigeo.length>0){
                    $.each(dataUbigeo, function (index, value) {
                        $("#cboDepartamento").append('<option value="' + value.DepartamentoId + '"  >' + value.Nombre + '</option>');
                    });
                }
                if(dataEmpresas.length>0){
                    $.each(dataEmpresas, function (index, value) {
                        $("#cboEmpresa").append('<option value="' + value.CodEmpresa + '"  >' + value.RazonSocial + '</option>');
                    });
                }
                if (dataSalasMaestras.length > 0) {
                    $.each(dataSalasMaestras, function (index, value) {
                        $("#cboSalaMaestra").append('<option value="' + value.CodSalaMaestra + '"  >' + value.Nombre + '</option>');
                    });
                }
            }
            else{
                toastr.error(result.mensaje,"Mensaje Servidor");
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
$("#frmRegistroSala")
.bootstrapValidator({
    container: '#messages',
    excluded: [':disabled', ':hidden', ':not(:visible)'],
    feedbackIcons: {
        valid: 'icon icon-check',
        invalid: 'icon icon-cross',
        validating: 'icon icon-refresh'
    },
    fields: {
        CodSala: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
        
        CodEmpresa: {
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
        NombreCorto: {
            validators: {
                notEmpty: {
                    message: ''
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
    let $parent = data.element.parents('.form-group');
    // Remove the has-success class
    $parent.removeClass('has-success');
    // Hide the success icon
    $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();


});
$.fn.serializeForm = function () {

    let o = {};
    let a = this.serializeArray();
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
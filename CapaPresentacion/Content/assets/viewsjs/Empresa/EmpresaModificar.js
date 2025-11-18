
$(document).ready(function () {
    $("#frmRegistroEmpresa")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                CodEmpresa: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                RazonSocial: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                Ruc: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                DepartamentoId: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                ProvinciaId: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                CodUbigeo: {
                    validators: {
                        notEmpty: {
                            message: ' '
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
    $("#TipoEmpresa").val(empresa.TipoEmpresa);
    $("#CodConsorcio").val(empresa.CodConsorcio);
    // $("#Cbo").val(empresa.CodUbigeo);
    $("#RutaArchivoLogoAnt").val(empresa.RutaArchivoLogoAnt);
    $("#CodEmpresa").val(empresa.CodEmpresa);
    $("#RazonSocial").val(empresa.RazonSocial);
    $("#Ruc").val(empresa.Ruc);
    $("#Direccion").val(empresa.Direccion);

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
        //envio post
        if(archivoValidado){
            let validar = $("#frmRegistroEmpresa").data('bootstrapValidator').validate();
            if(validar.isValid()){
                let dataForm = new FormData(document.getElementById("frmRegistroEmpresa"));
                $.ajax({
                    url: basePath + "Empresa/EmpresaModificarJson",
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
                                    let url = basePath + "Empresa/EmpresaVista";
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
        console.log("btnListar");
        window.location.replace(basePath + "Empresa/EmpresaVista");
    });
    $(".mySelect").select2({
        multiple: false, placeholder: "--Seleccione--"
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
    ObtenerDataSelects();
});
function ObtenerDataSelects(){
    let dataForm={ ubigeo:ubigeo}
    $.ajax({
        type: "POST",
        url: basePath + "Empresa/GetDataSelects",
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
                let dataDistritos=result.data.dataDistritos;
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
            if(ubigeo.DepartamentoId!=0){
                $("#cboDepartamento").val(ubigeo.DepartamentoId);
                $("#cboProvincia").val(ubigeo.ProvinciaId);
                $("#cboDistrito").val(ubigeo.CodUbigeo);
            }
            else{
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
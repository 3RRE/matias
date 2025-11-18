$(document).ready(function () {
    $(".mySelect").append('<option value="">---Seleccione---</option>');
    $(".mySelect").select2({
        multiple: false, placeholder: "--Seleccione--"
    })
    ObtenerDepartamentos()

    $(document).on('change', '#cboDepartamento', function (e) {
        e.preventDefault()
        let DepartamentoID = $("#cboDepartamento option:selected").val();
        ObtenerListaProvincias(DepartamentoID);
    })
    $(document).on('change', '#cboProvincia', function (e) {
        e.preventDefault()
        let DepartamentoID = $("#cboDepartamento option:selected").val();
        let ProvinciaID = $("#cboProvincia option:selected").val();
        ObtenerListaDistritos(DepartamentoID, ProvinciaID)
    })
    $(document).on('keypress', '.soloNumeros', function (event) {
        let regex = new RegExp("^[0-9]+$");
        let key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    })
    $(document).on('keypress', '.soloTexto', function (event) {
        let regex = new RegExp("^[a-zA-Z ]+$");
        let key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    })
    $(document).on('keypress', '.UpperCase', function (event) {
        $input = $(this);
        setTimeout(function () {
            $input.val($input.val().toUpperCase());
        }, 0);
    })
    $('.btnListar').on('click', function (e) {
        window.location.replace(basePath + "Empresa/EmpresaVista");
    });
    $(document).on('click', '.btnGuardar', function (e) {
        e.preventDefault()
        let permitido = 4194304;
        let file = $('#RutaArchivoLogo')[0].files[0]
        if (file) {
            let fileSize = file.size;
            if (fileSize < permitido) {
                let validar = $("#frmRegistroEmpresa").data('bootstrapValidator').validate();
                if (validar.isValid()) {
                    let dataForm = new FormData(document.getElementById("frmRegistroEmpresa"));

                    $.ajax({
                        url: basePath + "Empresa/InsertarEmpresaJson",
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
                                $("#cboDepartamento").val(null).trigger("change");
                                $("#cboProvincia").val(null).trigger("change");
                                $("#cboDistrito").val(null).trigger("change");
                                $("#TipoEmpresa").val(1);
                                $("#CodConsorcio").val(1);
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
                                        window.location.href = url;
                                    },
                                    cancel: function () {
                                    }
                                });
                                // toastr.success(response.mensaje,"Mensaje Servidor")
                                // url = basePath + "Empresa/EmpresaVista";
                                // setTimeout(function () {
                                //     window.location.href=url;
                                // }, 2000);
                            } else {
                                toastr.error(response.mensaje, "Mensaje Servidor")
                            }
                        },
                        error: function (xmlHttpRequest, textStatus, errorThrow) {
                        }
                    });
                }

            }
        }
        else {
            toastr.warning("Seleccione una imagen para logo", "Mensaje Servidor")
        }
    })
})
function ObtenerDepartamentos() {
    $.ajax({
        type: "POST",
        url: basePath + "Empresa/GetListadoDepartamento",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.data) {
                let dataUbigeo = result.data;

                if (dataUbigeo.length > 0) {
                    $.each(dataUbigeo, function (index, value) {
                        $("#cboDepartamento").append('<option value="' + value.DepartamentoId + '"  >' + value.Nombre + '</option>');
                    });
                }
            }
            else {
                toastr.error(result.mensaje, "Mensaje Servidor");
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
    if (DepartamentoID) {
        let dataForm = { DepartamentoID: DepartamentoID }
        $.ajax({
            type: "POST",
            url: basePath + "Empresa/GetListadoProvincia",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                $("#cboProvincia,#cboDistrito").html("");
                $("#cboProvincia,#cboDistrito").append('<option value="">---Seleccione---</option>');
                if (result.respuesta) {
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
function ObtenerListaDistritos(DepartamentoID, ProvinciaID) {
    if (DepartamentoID && ProvinciaID) {
        let dataForm = { DepartamentoID: DepartamentoID, ProvinciaID: ProvinciaID }
        $.ajax({
            type: "POST",
            url: basePath + "Empresa/GetListadoDistrito",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                $("#cboDistrito").html("");
                $("#cboDistrito").append('<option value="">---Seleccione---</option>');
                if (result.respuesta) {
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

$("#frmRegistroEmpresa").bootstrapValidator({
    container: '#messages',
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
                    message: ''
                }
            }
        },
        RazonSocial: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
        Ruc: {
            validators: {
                notEmpty: {
                    message: ''
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


$(document).ready(function () {

    ListarBanda();

    ObtenerListaDepartamento();

    //$("#cboPais").append('<option value="">---Seleccione---</option>');
    $("#cboDepartamento").append('<option value="">---Seleccione---</option>');
    $("#cboProvincia").append('<option value="">---Seleccione---</option>');
    $("#cboDistrito").append('<option value="">---Seleccione---</option>');
    $("#cboEstado").val("");
    /*
    ObtenerListaPais();
    ObtenerListaProvincia();
    ObtenerListaDistrito();*/

    $(document).on('change', '#cboDepartamento', function (e) {
        e.preventDefault()
        let DepartamentoID = $("#cboDepartamento option:selected").val();
        ObtenerListaProvincia(DepartamentoID);
    })
    $(document).on('change', '#cboProvincia', function (e) {
        e.preventDefault()
        let DepartamentoID = $("#cboDepartamento option:selected").val();
        let ProvinciaID = $("#cboProvincia option:selected").val();
        ObtenerListaDistrito(DepartamentoID, ProvinciaID)
    })


    $(document).on("click", ".btnEditar", function () {
        LimpiarFormValidator();
        var id = $(this).data("id");
        $("#textBanda").text("Editar");
        ObtenerRegistro(id);
        $("#full-modal_banda").modal("show");
    });

    VistaAuditoria("CALBanda/ListadoBanda", "VISTA", 0, "", 3);

    $('#btnNuevo').on('click', function (e) {
        LimpiarFormValidator();
        $("#textBanda").text("Nuevo");

        $("#banda_id").val(0);
        $("#descripcion").val("");
        $("#cboPais").val("");
        $("#cboDepartamento").val("");
        $("#cboProvincia").val("");
        $("#cboDistrito").val("");
        $("#cboEstado").val("");
        $("#full-modal_banda").modal("show");
    });

    $(document).on("click", ".btnEliminar", function () {
        var elemento = $(this);
        var id = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar Banda?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "CALBanda/BandaEliminarJson",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({ id }),
                    beforeSend: function () {
                        $.LoadingOverlay("show");
                    },
                    complete: function () {
                        $.LoadingOverlay("hide");
                    },
                    success: function (response) {
                        if (response.respuesta) {
                            ListarBanda();
                            toastr.success(response.mensaje, "Mensaje Servidor");
                        }
                        else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrow) {
                        toastr.error("Error Servidor", "Mensaje Servidor");
                    }
                });

            },

            cancel: function () {
                //close
            },

        });



    });

    $("#form_registro_banda")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                descripcion: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                departamentoID: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                provinciaID: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                codUbigeo: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                estado: {
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
            $parent.removeClass('has-success');
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
        });

    $('.btnGuardar').on('click', function (e) {
        $("#form_registro_banda").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_banda").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#banda_id").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "CALBanda/BandaEditarJson";
                accion = "ACTUALIZAR BANDA";
                urlenvio = basePath + "CALBanda/BandaEditarJson";
            }
            else {
                lugar = "CALBanda/BandaGuardarJson";
                accion = "NUEVO BANDA";
                urlenvio = basePath + "CALBanda/BandaGuardarJson";
            }

            var dataForm = $('#form_registro_banda').serializeFormJSON();

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
                    ListarBanda();
                },
                success: function (response) {
                    if (response.respuesta) {
                        $('#banda_id').val("0");
                        $('#nombre').val("");
                        $('#descripcion').val("");
                        $('#estado').val("0");
                        $("#full-modal_banda").modal("hide");
                        //$("#btnBuscar").click();
                        LimpiarFormValidator();
                        toastr.success("Banda Guardado", "Mensaje Servidor");
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

    });


    $(document).on("click", "#btnExcel", function () {
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "CALBanda/BandaDescargarExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                if (response.respuesta) {
                    var data = response.data;
                    var file = response.excelName;
                    let a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/vnd.ms-excel;base64, " + data;
                    a.download = file;
                    a.click();
                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });

});

function ListarBanda() {
    var url = basePath + "CALBanda/ListarBandaJson";
    var data = {}; var respuesta = "";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            respuesta = response.data
            objetodatatable = $("#tableBanda").DataTable({
                "bDestroy": true,
                "bSort": true,
                "ordering": true,
                "scrollCollapse": true,
                "scrollX": true,
                "paging": true,
                "aaSorting": [],
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                data: response.data,
                columns: [
                    { data: "BandaID", title: "ID" },
                    { data: "Descripcion", title: "Descripcion" },
                    //{ data: "CodUbigeo", title: "Codigo Ubigeo" },
                    { data: "Pais", title: "Pais" },
                    { data: "Departamento", title: "Departamento" },
                    { data: "Provincia", title: "Provincia" },
                    { data: "Distrito", title: "Distrito" },
                    {
                        data: "Estado", title: "Estado",
                        "render": function (o) {
                            var estado = "INACTIVO";
                            var css = "btn-danger";
                            if (o == true) {
                                estado = "ACTIVO"
                                css = "btn-success";
                            }
                            return '<span class="label ' + css + '">' + estado + '</span>';

                        }
                    },
                    /*{
                        data: "FechaRegistro", title: "Fecha Registro",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY hh:mm");
                        }
                    },*/

                    {
                        data: "BandaID", title: "Acción",
                        "bSortable": false,
                        "render": function (o) {
                            return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
                        }
                    }
                ],
                "drawCallback": function (settings) {
                    $('.btnEditar').tooltip({
                        title: "Editar"
                    });
                },

                "initComplete": function (settings, json) {



                },
            });
            $('.btnEditar').tooltip({
                title: "Editar"
            });

        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {

        }
    });
};

function ObtenerListaDepartamento() {
    $.ajax({
        type: "POST",
        url: basePath + "CALBanda/GetListadoDepartamento",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboDepartamento,#cboProvincia,#cboDistrito").html("");
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
    return true;
}
function ObtenerListaProvincia(DepartamentoID) {
    if (DepartamentoID) {
        let dataForm = { DepartamentoID: DepartamentoID }
        $.ajax({
            type: "POST",
            url: basePath + "CALBanda/GetListadoProvincia",
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
        return true;
    }

}
function ObtenerListaDistrito(DepartamentoID, ProvinciaID) {
    if (DepartamentoID && ProvinciaID) {
        let dataForm = { DepartamentoID: DepartamentoID, ProvinciaID: ProvinciaID }
        $.ajax({
            type: "POST",
            url: basePath + "CALBanda/GetListadoDistrito",
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
        return true;
    }

}


function ObtenerDataSelects(codUbigeo) {
    console.log("ajax" + codUbigeo);
    $.ajax({
        type: "POST",
        url: basePath + "CALBanda/GetDataSelects",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodUbigeo: codUbigeo }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.respuesta) {
                $("#cboDepartamento,#cboProvincia,#cboDistrito").html("");
                let dataUbigeo = result.data.dataUbigeo;
                let dataDepartamentos = result.data.dataDepartamentos;
                let dataProvincias = result.data.dataProvincias;
                let dataDistritos = result.data.dataDistritos;
                if (dataDepartamentos) {
                    if (dataDepartamentos.length > 0) {
                    $.each(dataDepartamentos, function (index, value) {
                        $("#cboDepartamento").append('<option value="' + value.DepartamentoId + '"  >' + value.Nombre + '</option>');
                    });
                    if (dataProvincias && dataDistritos) {
                        $.each(dataProvincias, function (index, value) {
                            $("#cboProvincia").append('<option value="' + value.ProvinciaId + '">' + value.Nombre + '</option>');
                        });
                        $.each(dataDistritos, function (index, value) {
                            $("#cboDistrito").append('<option value="' + value.CodUbigeo + '"  >' + value.Nombre + '</option>');
                        });
                    }
                    }
                }

                console.log(dataUbigeo.DepartamentoId)
                if (codUbigeo != 0) {
                    $("#cboDepartamento").val(dataUbigeo.DepartamentoId);
                    $("#cboProvincia").val(dataUbigeo.ProvinciaId);
                    $("#cboDistrito").val(dataUbigeo.CodUbigeo);
                }
                else {
                    $("#cboDepartamento").val("");
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

function ObtenerRegistro(id) {

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CALBanda/ListarBandaIdJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ BandaID: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {

                response = response.data;

                console.log("button" + parseInt(response.CodUbigeo));
                $("#banda_id").val(response.BandaID);
                $("#nombre").val(response.Nombre);
                $("#descripcion").val(response.Descripcion);
                $("#cboEstado").val(response.Estado);
                ObtenerDataSelects(parseInt(response.CodUbigeo));
            }
            /*
            $("#cboDepartamento").trigger("change");
            $("#cboProvincia").val(parseInt(response.codProvincia));
            $("#cboProvincia").trigger("change");
            $("#cboDistrito").val(parseInt(response.codDistrito));
           

            let dataDepartamento = parseInt(response.codDepartamento);
            let dataProvincia = parseInt(response.codProvincia);
            let dataDistrito = parseInt(response.codDistrito);
            ObtenerListas(dataDepartamento, dataProvincia, dataDistrito);
            */


        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};


function LimpiarFormValidator() {
    $("#form_registro_banda").parent().find('div').removeClass("has-error");
    $("#form_registro_banda").parent().find('i').removeAttr("style").hide();
}
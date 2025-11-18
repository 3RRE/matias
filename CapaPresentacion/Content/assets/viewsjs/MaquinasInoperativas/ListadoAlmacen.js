

$(document).ready(function () {
    LimpiarFormValidator();
    ListarAlmacen();
    ObtenerListaSalas();

    $(document).on("click", ".btnEditar", function () {
        LimpiarFormValidator();
        var id = $(this).data("id");
        $("#textAlmacen").text("Editar");
        ObtenerRegistro(id);
        $("#full-modal_almacen").modal("show");
    });

    VistaAuditoria("MIAlmacen/ListadoAlmacen", "VISTA", 0, "", 3);

    $('#btnNuevo').on('click', function (e) {
        //$("#form_registro_almacen").validator("destroy");
        //$('#form_registro_almacen').bootstrapValidator('resetForm', true);

        //var form = $('#form_registro_almacen')[0];
        //$(form).removeClass('has-error');
        //form.reset();
        LimpiarFormValidator();

        $("#textAlmacen").text("Nueva");

        $("#cod_almacen").val(0);
        $("#nombre").val("");
        $("#descripcion").val("");
        $("#cboEstado").val("");
        $("#cboSala").val(null).trigger("change");
        $("#full-modal_almacen").modal("show");
    });

    $(document).on("click", ".btnEliminar", function () {
        var elemento = $(this);
        var cod = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar esta  Almacen?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "MIAlmacen/AlmacenEliminarJson",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({ cod }),
                    beforeSend: function () {
                        $.LoadingOverlay("show");
                    },
                    complete: function () {
                        VistaAuditoria("MIAlmacen/EliminarAlmacen", "VISTA", 0, "", 3);
                        $.LoadingOverlay("hide");
                    },
                    success: function (response) {
                        if (response.respuesta) {
                            ListarAlmacen();
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

    $("#form_registro_almacen")
        .bootstrapValidator({
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
                            message: ' '
                        }
                    }
                },
                codSala: {
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
        $("#form_registro_almacen").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_almacen").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#cod_almacen").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "MIAlmacen/AlmacenEditarJson";
                accion = "ACTUALIZAR ALMACEN";
                urlenvio = basePath + "MIAlmacen/AlmacenEditarJson";
                VistaAuditoria("MIAlmacen/EditarAlmacen", "VISTA", 0, "", 3);
            }
            else {
                lugar = "MIAlmacen/AlmacenGuardarJson";
                accion = "NUEVO ALMACEN";
                urlenvio = basePath + "MIAlmacen/AlmacenGuardarJson";
                VistaAuditoria("MIAlmacen/NuevoAlmacen", "VISTA", 0, "", 3);
            }

            var dataForm = $('#form_registro_almacen').serializeFormJSON();

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
                    ListarAlmacen();
                },
                success: function (response) {
                    if (response.respuesta) {
                        $("#full-modal_almacen").modal("hide");
                        $('#cod_almacen').val("0");
                        $('#nombre').val("");
                        $('#descripcion').val("");
                        $('#estado').val("0");
                        $("#cboSala").val(null).trigger("change");
                        LimpiarFormValidator();
                        toastr.success(" Almacen Guardada", "Mensaje Servidor");
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
            url: basePath + "MIAlmacen/AlmacenDescargarExcelJson",
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

function ListarAlmacen() {
    var url = basePath + "MIAlmacen/ListarAlmacenJson";
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
            objetodatatable = $("#tableAlmacen").DataTable({
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
                    { data: "CodAlmacen", title: "ID" },
                    { data: "Nombre", title: "Nombre" },
                    { data: "Descripcion", title: "Descripcion" },
                    { data: "NombreSala", title: "Sala" },
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
                        data: "CodAlmacen", title: "Acción",
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
                    $('.btnEliminar').tooltip({
                        title: "Eliminar"
                    });
                },

                "initComplete": function (settings, json) {



                },
            });

        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {

        }
    });
};

function ObtenerRegistro(id) {

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "MIAlmacen/ListarAlmacenCodJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodAlmacen: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {

                response = response.data;

                $("#cod_almacen").val(response.CodAlmacen);
                $("#nombre").val(response.Nombre);
                $("#descripcion").val(response.Descripcion);
                $("#cboSala").val(response.CodSala).trigger("change");
                $("#cboEstado").val(response.Estado);
            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};


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

            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_almacen')

            });
            $("#cboSala").val(null).trigger("change");
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


function LimpiarFormValidator() {
    $("#form_registro_almacen").parent().find('div').removeClass("has-error");
    $("#form_registro_almacen").parent().find('i').removeAttr("style").hide();
}
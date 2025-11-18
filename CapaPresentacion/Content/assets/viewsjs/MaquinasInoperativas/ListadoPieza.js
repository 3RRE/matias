

$(document).ready(function () {

    ListarPieza();
    ObtenerListaCategoriaPieza();

    $(document).on("click", ".btnEditar", function () {
        LimpiarFormValidator();
        var id = $(this).data("id");
        $("#textPieza").text("Editar");
        ObtenerRegistro(id);
        $("#full-modal_pieza").modal("show");
    });

    VistaAuditoria("MIPieza/ListadoPieza", "VISTA", 0, "", 3);

    $('#btnNuevo').on('click', function (e) {
        LimpiarFormValidator();
        $("#textPieza").text("Nueva");

        $("#cod_pieza").val(0);
        $("#nombre").val("");
        $("#descripcion").val("");
        $("#cboEstado").val("");
        $("#cboCategoriaPieza").val(null).trigger("change");
        $("#full-modal_pieza").modal("show");
    });

    $(document).on("click", ".btnEliminar", function () {
        var elemento = $(this);
        var cod = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar esta  Pieza?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "MIPieza/PiezaEliminarJson",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({ cod }),
                    beforeSend: function () {
                        $.LoadingOverlay("show");
                    },
                    complete: function () {
                        $.LoadingOverlay("hide");
                    },
                    success: function (response) {
                        if (response.respuesta) {
                            ListarPieza();
                            toastr.success(response.mensaje, "Mensaje Servidor");
                            VistaAuditoria("MIPieza/EliminarPieza", "VISTA", 0, "", 3);
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

    $("#form_registro_pieza")
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
                codCategoriaPieza: {
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
        $("#form_registro_pieza").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_pieza").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#cod_pieza").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "MIPieza/PiezaEditarJson";
                accion = "ACTUALIZAR PIEZA";
                urlenvio = basePath + "MIPieza/PiezaEditarJson";
                VistaAuditoria("MIPieza/EditarPieza", "VISTA", 0, "", 3);
            }
            else {
                lugar = "MIPieza/PiezaGuardarJson";
                accion = "NUEVA PIEZA";
                urlenvio = basePath + "MIPieza/PiezaGuardarJson";
                VistaAuditoria("MIPieza/NuevoPieza", "VISTA", 0, "", 3);
            }

            var dataForm = $('#form_registro_pieza').serializeFormJSON();

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
                    ListarPieza();
                },
                success: function (response) {
                    if (response.respuesta) {
                        $('#cod_pieza').val("0");
                        $('#nombre').val("");
                        $('#descripcion').val("");
                        $('#estado').val("0");
                        $("#cboCategoriaPieza").val(null).trigger("change");
                        $("#full-modal_pieza").modal("hide");
                        toastr.success(" Pieza Guardada", "Mensaje Servidor");
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
            url: basePath + "MIPieza/PiezaDescargarExcelJson",
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

function ListarPieza() {
    var url = basePath + "MIPieza/ListarPiezaJson";
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
            objetodatatable = $("#tablePieza").DataTable({
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
                    { data: "CodPieza", title: "ID" },
                    { data: "Nombre", title: "Nombre" },
                    { data: "Descripcion", title: "Descripcion" },
                    { data: "NombreCategoriaPieza", title: "Categoria Pieza" },
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
                        data: "CodPieza", title: "Acción",
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
        url: basePath + "MIPieza/ListarPiezaCodJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodPieza: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {

                response = response.data;

                $("#cod_pieza").val(response.CodPieza);
                $("#nombre").val(response.Nombre);
                $("#descripcion").val(response.Descripcion);
                $("#cboCategoriaPieza").val(response.CodCategoriaPieza).trigger("change");
                $("#cboEstado").val(response.Estado);
            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};


function ObtenerListaCategoriaPieza() {
    $.ajax({
        type: "POST",
        url: basePath + "MICategoriaPieza/ListarCategoriaPiezaActiveJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboCategoriaPieza").append('<option value="' + value.CodCategoriaPieza + '"  >' + value.Nombre + '</option>');
            });
            $("#cboCategoriaPieza").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_pieza')

            });
            $("#cboCategoriaPieza").val(null).trigger("change");
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
    $("#form_registro_pieza").parent().find('div').removeClass("has-error");
    $("#form_registro_pieza").parent().find('i').removeAttr("style").hide();
}
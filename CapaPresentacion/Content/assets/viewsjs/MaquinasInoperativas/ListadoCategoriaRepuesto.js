

$(document).ready(function () {

    ListarCategoriaRepuesto();

    $(document).on("click", ".btnEditar", function () {
        LimpiarFormValidator();
        var id = $(this).data("id");
        $("#textCategoriaRepuesto").text("Editar");
        ObtenerRegistro(id);
        $("#full-modal_categoria_repuesto").modal("show");
    });

    VistaAuditoria("MICategoriaRepuesto/ListadoCategoriaRepuesto", "VISTA", 0, "", 3);

    $('#btnNuevo').on('click', function (e) {
        LimpiarFormValidator();
        $("#textCategoriaRepuesto").text("Nueva");

        $("#cod_categoria_repuesto").val(0);
        $("#nombre").val("");
        $("#descripcion").val("");
        $("#cboEstado").val("");
        $("#full-modal_categoria_repuesto").modal("show");
    });

    $(document).on("click", ".btnEliminar", function () {
        var elemento = $(this);
        var cod = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar esta Categoria Repuesto?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "MICategoriaRepuesto/CategoriaRepuestoEliminarJson",
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
                            ListarCategoriaRepuesto();
                            toastr.success(response.mensaje, "Mensaje Servidor");
                            VistaAuditoria("MICategoriaRepuesto/EliminarCategoriaRepuesto", "VISTA", 0, "", 3);
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

    $("#form_registro_categoria_repuesto")
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
        $("#form_registro_categoria_repuesto").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_categoria_repuesto").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#cod_categoria_repuesto").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "MICategoriaRepuesto/CategoriaRepuestoEditarJson";
                accion = "ACTUALIZAR CATEGORIA REPUESTO";
                urlenvio = basePath + "MICategoriaRepuesto/CategoriaRepuestoEditarJson";
                VistaAuditoria("MICategoriaRepuesto/EditarCategoriaRepuesto", "VISTA", 0, "", 3);
            }
            else {
                lugar = "MICategoriaRepuesto/CategoriaRepuestoGuardarJson";
                accion = "NUEVA CATEGORIA REPUESTO";
                urlenvio = basePath + "MICategoriaRepuesto/CategoriaRepuestoGuardarJson";
                VistaAuditoria("MICategoriaRepuesto/NuevoCategoriaRepuesto", "VISTA", 0, "", 3);
            }

            var dataForm = $('#form_registro_categoria_repuesto').serializeFormJSON();

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
                    ListarCategoriaRepuesto();
                },
                success: function (response) {
                    if (response.respuesta) {
                        $('#cod_categoria_repuesto').val("0");
                        $('#nombre').val("");
                        $('#descripcion').val("");
                        $('#estado').val("0");
                        $("#full-modal_categoria_repuesto").modal("hide");
                        toastr.success("Categoria Repuesto Guardada", "Mensaje Servidor");
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
            url: basePath + "MICategoriaRepuesto/CategoriaRepuestoDescargarExcelJson",
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

function ListarCategoriaRepuesto() {
    var url = basePath + "MICategoriaRepuesto/ListarCategoriaRepuestoJson";
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
            objetodatatable = $("#tableCategoriaRepuesto").DataTable({
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
                    { data: "CodCategoriaRepuesto", title: "ID" },
                    { data: "Nombre", title: "Nombre" },
                    { data: "Descripcion", title: "Descripcion" },
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
                        data: "CodCategoriaRepuesto", title: "Acción",
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
        url: basePath + "MICategoriaRepuesto/ListarCategoriaRepuestoCodJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodCategoriaRepuesto: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {

                response = response.data;

                $("#cod_categoria_repuesto").val(response.CodCategoriaRepuesto);
                $("#nombre").val(response.Nombre);
                $("#descripcion").val(response.Descripcion);
                $("#cboEstado").val(response.Estado);
            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};

function LimpiarFormValidator() {
    $("#form_registro_categoria_repuesto").parent().find('div').removeClass("has-error");
    $("#form_registro_categoria_repuesto").parent().find('i').removeAttr("style").hide();
}
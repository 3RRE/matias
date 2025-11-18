

$(document).ready(function () {

    ListarProblema();
    ObtenerListaCategoriaProblema();

    $(document).on("click", ".btnEditar", function () {
        LimpiarFormValidator();
        var id = $(this).data("id");
        $("#textProblema").text("Editar");
        ObtenerRegistro(id);
        $("#full-modal_problema").modal("show");
    });

    VistaAuditoria("MIProblema/ListadoProblema", "VISTA", 0, "", 3);

    $('#btnNuevo').on('click', function (e) {
        LimpiarFormValidator();
        $("#textProblema").text("Nueva");

        $("#cod_problema").val(0);
        $("#nombre").val("");
        $("#descripcion").val("");
        $("#cboEstado").val("");
        $("#cboCategoriaProblema").val(null).trigger("change");
        $("#full-modal_problema").modal("show");
    });

    $(document).on("click", ".btnEliminar", function () {
        var elemento = $(this);
        var cod = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar esta  Problema?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "MIProblema/ProblemaEliminarJson",
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
                            ListarProblema();
                            toastr.success(response.mensaje, "Mensaje Servidor");
                            VistaAuditoria("MIProblema/EliminarProblema", "VISTA", 0, "", 3);
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

    $("#form_registro_problema")
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
                codCategoriaProblema: {
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
        $("#form_registro_problema").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_problema").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#cod_problema").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "MIProblema/ProblemaEditarJson";
                accion = "ACTUALIZAR PROBLEMA";
                urlenvio = basePath + "MIProblema/ProblemaEditarJson";
                VistaAuditoria("MIProblema/EditarProblema", "VISTA", 0, "", 3);
            }
            else {
                lugar = "MIProblema/ProblemaGuardarJson";
                accion = "NUEVO PROBLEMA";
                urlenvio = basePath + "MIProblema/ProblemaGuardarJson";
                VistaAuditoria("MIProblema/NuevoProblema", "VISTA", 0, "", 3);
            }

            var dataForm = $('#form_registro_problema').serializeFormJSON();

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
                    ListarProblema();
                },
                success: function (response) {
                    if (response.respuesta) {
                        $('#cod_problema').val("0");
                        $('#nombre').val("");
                        $('#descripcion').val("");
                        $('#estado').val("0");
                        $("#cboCategoriaProblema").val(null).trigger("change");
                        $("#full-modal_problema").modal("hide");
                        toastr.success(" Problema Guardada", "Mensaje Servidor");
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
            url: basePath + "MIProblema/ProblemaDescargarExcelJson",
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

function ListarProblema() {
    var url = basePath + "MIProblema/ListarProblemaJson";
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
            objetodatatable = $("#tableProblema").DataTable({
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
                    { data: "CodProblema", title: "ID" },
                    { data: "Nombre", title: "Nombre" },
                    { data: "Descripcion", title: "Descripcion" },
                    { data: "NombreCategoriaProblema", title: "Categoria Problema" },
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
                        data: "CodProblema", title: "Acción",
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
        url: basePath + "MIProblema/ListarProblemaCodJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodProblema: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {

                response = response.data;

                $("#cod_problema").val(response.CodProblema);
                $("#nombre").val(response.Nombre);
                $("#descripcion").val(response.Descripcion);
                $("#cboCategoriaProblema").val(response.CodCategoriaProblema).trigger("change");
                $("#cboEstado").val(response.Estado);
            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};


function ObtenerListaCategoriaProblema() {
    $.ajax({
        type: "POST",
        url: basePath + "MICategoriaProblema/ListarCategoriaProblemaActiveJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboCategoriaProblema").append('<option value="' + value.CodCategoriaProblema + '"  >' + value.Nombre + '</option>');
            });
            $("#cboCategoriaProblema").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_problema')

            });
            $("#cboCategoriaProblema").val(null).trigger("change");
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
    $("#form_registro_problema").parent().find('div').removeClass("has-error");
    $("#form_registro_problema").parent().find('i').removeAttr("style").hide();
}
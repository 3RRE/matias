

$(document).ready(function () {

    ListarRepuesto();
    ObtenerListaCategoriaRepuesto();

    $(document).on("click", ".btnEditar", function () {
        LimpiarFormValidator();
        var id = $(this).data("id");
        $("#textRepuesto").text("Editar");
        ObtenerRegistro(id);
        $("#full-modal_repuesto").modal("show");
    });

    $(document).on('keypress', '#costoReferencial', function (event) {
        var regex = new RegExp("^[0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    })

    VistaAuditoria("MIRepuesto/ListadoRepuesto", "VISTA", 0, "", 3);

    $('#btnNuevo').on('click', function (e) {
        LimpiarFormValidator();
        $("#textRepuesto").text("Nueva");

        $("#cod_repuesto").val(0);
        $("#costoReferencial").val(0);
        $("#nombre").val("");
        $("#descripcion").val("");
        $("#cboEstado").val("");
        $("#cboCategoriaRepuesto").val(null).trigger("change");
        $("#full-modal_repuesto").modal("show");
    });

    $(document).on("click", ".btnEliminar", function () {
        var elemento = $(this);
        var cod = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar esta  Repuesto?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "MIRepuesto/RepuestoEliminarJson",
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
                            ListarRepuesto();
                            toastr.success(response.mensaje, "Mensaje Servidor");
                            VistaAuditoria("MIRepuesto/EliminarRepuesto", "VISTA", 0, "", 3);
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

    $("#form_registro_repuesto")
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
                costoReferencial: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                codCategoriaRepuesto: {
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
        $("#form_registro_repuesto").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_repuesto").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#cod_repuesto").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "MIRepuesto/RepuestoEditarJson";
                accion = "ACTUALIZAR REPUESTO";
                urlenvio = basePath + "MIRepuesto/RepuestoEditarJson";
                VistaAuditoria("MIRepuesto/EditarRepuesto", "VISTA", 0, "", 3);
            }
            else {
                lugar = "MIRepuesto/RepuestoGuardarJson";
                accion = "NUEVO REPUESTO";
                urlenvio = basePath + "MIRepuesto/RepuestoGuardarJson";
                VistaAuditoria("MIRepuesto/NuevoRepuesto", "VISTA", 0, "", 3);
            }

            var dataForm = $('#form_registro_repuesto').serializeFormJSON();

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
                    ListarRepuesto();
                },
                success: function (response) {
                    if (response.respuesta) {
                        $('#cod_repuesto').val("0");
                        $('#costoReferencial').val("0");
                        $('#nombre').val("");
                        $('#descripcion').val("");
                        $('#estado').val("0");
                        $("#cboCategoriaRepuesto").val(null).trigger("change");
                        $("#full-modal_repuesto").modal("hide");
                        toastr.success(" Repuesto Guardada", "Mensaje Servidor");
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
            url: basePath + "MIRepuesto/RepuestoDescargarExcelJson",
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

function ListarRepuesto() {
    var url = basePath + "MIRepuesto/ListarRepuestoJson";
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
            objetodatatable = $("#tableRepuesto").DataTable({
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
                    { data: "CodRepuesto", title: "ID" },
                    { data: "Nombre", title: "Nombre" },
                    { data: "Descripcion", title: "Descripcion" },
                    { data: "CostoReferencial", title: "Costo Referencial" },
                    { data: "NombreCategoriaRepuesto", title: "Categoria Repuesto" },
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
                        data: "CodRepuesto", title: "Acción",
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
        url: basePath + "MIRepuesto/ListarRepuestoCodJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodRepuesto: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {

                response = response.data;

                $("#cod_repuesto").val(response.CodRepuesto);
                $("#nombre").val(response.Nombre);
                $("#descripcion").val(response.Descripcion);
                $("#costoReferencial").val(response.CostoReferencial);
                $("#cboCategoriaRepuesto").val(response.CodCategoriaRepuesto).trigger("change");
                $("#cboEstado").val(response.Estado);
            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};


function ObtenerListaCategoriaRepuesto() {
    $.ajax({
        type: "POST",
        url: basePath + "MICategoriaRepuesto/ListarCategoriaRepuestoActiveJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboCategoriaRepuesto").append('<option value="' + value.CodCategoriaRepuesto + '"  >' + value.Nombre + '</option>');
            });
            $("#cboCategoriaRepuesto").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_repuesto')

            });
            $("#cboCategoriaRepuesto").val(null).trigger("change");
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
    $("#form_registro_repuesto").parent().find('div').removeClass("has-error");
    $("#form_registro_repuesto").parent().find('i').removeAttr("style").hide();
}
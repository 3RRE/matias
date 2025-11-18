

$(document).ready(function () {

    ListarCodigoPersona();
    ObtenerListaCodigo();

    $(document).on("click", ".btnEditar", function () {
        LimpiarFormValidator();
        var id = $(this).data("id");
        $("#textCodigoPersona").text("Editar");
        ObtenerRegistro(id);
        $("#full-modal_codigo_persona").modal("show");
    });

    VistaAuditoria("CALCodigoPersona/ListadoCodigoPersona", "VISTA", 0, "", 3);

    $('#btnNuevo').on('click', function (e) {
        LimpiarFormValidator();
        $("#textCodigoPersona").text("Nuevo");

        $("#codigo_persona_id").val(0);
        $("#tipoPersona").val("");
        $("#tipoPersona").attr('readonly', false);
        //$("#cboCodigo").prop("disabled", false);
        $("#cboCodigo").val(null).trigger("change");

        $("#cboEditable").select2({
            multiple: false, placeholder: "--Seleccione--", dropdownParent: $("#full-modal_codigo_persona")
        })

        $("#cboEditable").val(null).trigger("change");
        $("#full-modal_codigo_persona").modal("show");
    });

    $(document).on("click", ".btnEliminar", function () {
        var elemento = $(this);
        var id = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar Codigo Persona?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "CALCodigoPersona/CodigoPersonaEliminarJson",
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
                            ListarCodigoPersona();
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

    $("#form_registro_codigo_persona")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                tipoPersona: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                codigoID: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                editable: {
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
        $("#form_registro_codigo_persona").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_codigo_persona").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#codigo_persona_id").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "CALCodigoPersona/CodigoPersonaEditarJson";
                accion = "ACTUALIZAR CODIGO PERSONA";
                urlenvio = basePath + "CALCodigoPersona/CodigoPersonaEditarJson";
            }
            else {
                lugar = "CALCodigoPersona/CodigoPersonaGuardarJson";
                accion = "NUEVO CODIGO PERSONA";
                urlenvio = basePath + "CALCodigoPersona/CodigoPersonaGuardarJson";
            }

            var dataForm = $('#form_registro_codigo_persona').serializeFormJSON();

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
                    ListarCodigoPersona();
                },
                success: function (response) {
                    if (response.respuesta) {
                        $('#codigo_persona_id').val("0");
                        $('#alerta').val("");
                        $('#accion').val("");
                        $('#color').val("");
                        $("#full-modal_codigo_persona").modal("hide");
                        //$("#btnBuscar").click();
                        LimpiarFormValidator();
                        toastr.success("Codigo Persona Guardado", "Mensaje Servidor");
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
            url: basePath + "CALCodigoPersona/CodigoPersonaDescargarExcelJson",
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

function ListarCodigoPersona() {
    var url = basePath + "CALCodigoPersona/ListarCodigoPersonaJson";
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
            objetodatatable = $("#tableCodigoPersona").DataTable({
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
                    { data: "Id", title: "ID" },
                    { data: "TipoPersona", title: "Tipo Persona" },
                    { data: "CodigoNombre", title: "Codigo Nombre" },
                    /*{
                        data: "FechaRegistro", title: "Fecha Registro",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY hh:mm");
                        }
                    },*/

                    {
                        data: "Id", title: "Acción",
                        "bSortable": false,
                        "render": function (o, value, oData) {
                            return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}" data-editable="${oData.Editable}"><i class="glyphicon glyphicon-pencil"></i></button>  
                                    <button style="width:40px;heigth:40px;" type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `; 
                            
                            
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


function ObtenerRegistro(id) {

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CALCodigoPersona/ListarCodigoPersonaIdJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodigoPersonaID: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            response = response.data;

            $("#codigo_persona_id").val(response.Id);
            $("#tipoPersona").val(response.TipoPersona);
            $("#cboCodigo").val(response.CodigoID).trigger("change");
            $("#cboEditable").val(response.Editable).trigger("change");

            if (response.Editable == 0) {
                //$("#cboCodigo").prop("disabled", true);
                $("#tipoPersona").attr('readonly', true);

            } else {
                //$("#cboCodigo").prop("disabled", false);
                $("#tipoPersona").attr('readonly', false);

            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};

function ObtenerListaCodigo() {
    $.ajax({
        type: "POST",
        url: basePath + "CALCodigo/ListarCodigoJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboCodigo").append('<option value="' + value.CodigoID + '"  >' + value.Alerta + '</option>');
            });
            $("#cboCodigo").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_codigo_persona')

            });
            $("#cboCodigo").val(null).trigger("change");
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
    $("#form_registro_codigo_persona").parent().find('div').removeClass("has-error");
    $("#form_registro_codigo_persona").parent().find('i').removeAttr("style").hide();
}


$(document).ready(function () {

    ListarCodigo();

    $(document).on("click", ".btnEditar", function () {
        LimpiarFormValidator();
        var id = $(this).data("id");
        $("#textCodigo").text("Editar");
        ObtenerRegistro(id);
        $("#full-modal_codigo").modal("show");
    });

    VistaAuditoria("CALCodigo/ListadoCodigo", "VISTA", 0, "", 3);

    $('#btnNuevo').on('click', function (e) {
        LimpiarFormValidator();
        $("#textCodigo").text("Nuevo");

        $("#codigo_id").val(0);
        $("#nombre").val("");
        $("#cboEstado").val(1);
        $("#full-modal_codigo").modal("show");
    });

    $(document).on("click", ".btnEliminar", function () {
        var elemento = $(this);
        var id = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar Codigo?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "CALCodigo/CodigoEliminarJson",
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
                            ListarCodigo();
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

    $("#form_registro_codigo")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                alerta: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                accion: {
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
        $("#form_registro_codigo").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_codigo").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#codigo_id").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "CALCodigo/CodigoEditarJson";
                accion = "ACTUALIZAR CODIGO";
                urlenvio = basePath + "CALCodigo/CodigoEditarJson";
            }
            else {
                lugar = "CALCodigo/CodigoGuardarJson";
                accion = "NUEVO CODIGO";
                urlenvio = basePath + "CALCodigo/CodigoGuardarJson";
            }


            //console.log($('#form_registro_codigo').serializeFormJSON());

            var dataForm = $('#form_registro_codigo').serializeFormJSON();
            //console.log($('#color').val());
            $.extend(dataForm, { Color: $('#color').val() });
            //var result = JSON.stringify(dataForm);
            //console.log(result);

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
                    ListarCodigo();
                },
                success: function (response) {
                    if (response.respuesta) {
                        $('#codigo_id').val("0");
                        $('#alerta').val("");
                        $('#accion').val("");
                        $('#color').val("#ffffff");
                        $("#full-modal_codigo").modal("hide");
                        //$("#btnBuscar").click();
                        LimpiarFormValidator();
                        toastr.success("Codigo Guardado", "Mensaje Servidor");
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
            url: basePath + "CALCodigo/CodigoDescargarExcelJson",
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

function ListarCodigo() {
    var url = basePath + "CALCodigo/ListarCodigoJson";
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
            objetodatatable = $("#tableCodigo").DataTable({
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
                    { data: "CodigoID", title: "ID" },
                    { data: "Alerta", title: "Alerta" },
                    { data: "Accion", title: "Accion" },
                    {
                        data: "Color", title: "Color",
                        "render": function (o) {
                            return `<input type="color" value="${o}" disabled>` } 
                    },
                    /*{
                        data: "FechaRegistro", title: "Fecha Registro",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY hh:mm");
                        }
                    },*/

                    {
                        data: "CodigoID", title: "Acción",
                        "bSortable": false,
                        "render": function (o) {
                            return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button>  `;
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
        url: basePath + "CALCodigo/ListarCodigoIdJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodigoID: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            response = response.data;

            $("#codigo_id").val(response.CodigoID);
            $("#alerta").val(response.Alerta);
            $("#accion").val(response.Accion);
            $("#color").val(response.Color);

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};


function LimpiarFormValidator() {
    $("#form_registro_codigo").parent().find('div').removeClass("has-error");
    $("#form_registro_codigo").parent().find('i').removeAttr("style").hide();
}
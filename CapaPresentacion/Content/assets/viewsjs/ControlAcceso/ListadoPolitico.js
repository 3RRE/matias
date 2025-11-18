

$(document).ready(function () {


    $("#cboTipoDocumento").select2({
        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_politico')

    });
    ObtenerTipoDocumento();


    ListarPolitico();


    $("#cboEstado").select2({
        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_politico')

    });
    $("#cboEstado").val(null).trigger("change");

    ObtenerListaCargoPolitco();

    $(document).on('keypress', '#dni,#meses', function (event) {
        var regex = new RegExp("^[0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });

    $(document).on("click", ".btnEditar", function () {

        LimpiarFormValidator();
        var id = $(this).data("id");
        $("#textPolitico").text("Editar");
        ObtenerRegistro(id);
    });

    VistaAuditoria("CALPolitico/ListadoPolitico", "VISTA", 0, "", 3);

    $('#btnNuevo').on('click', function (e) {

        LimpiarFormValidator();
        $("#textPolitico").text("Nuevo");

        $("#politico_id").val(0);
        $("#nombres").val("");
        $("#apellidos").val("");
        $("#dni").val("");
        $("#entidadEstatal").val("");
        $("#meses").val("");
        $("#cboEstado").val(null).trigger("change");
        $("#cboCargoPolitico").val(null).trigger("change");
        $("#cboTipoDocumento").val(null).trigger("change");
        $("#full-modal_politico").modal("show");
    });

    $(document).on("click", ".btnEliminar", function () {
        var elemento = $(this);
        var id = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar Politico?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "CALPolitico/PoliticoEliminarJson",
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
                            ListarPolitico();
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

    $("#form_registro_politico")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                nombres: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                apellidos: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                dni: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                entidadEstatal: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                meses: {
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
                cargoPoliticoID: {
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
        $("#form_registro_politico").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_politico").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#politico_id").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "CALPolitico/PoliticoEditarJson";
                accion = "ACTUALIZAR POLITICO";
                urlenvio = basePath + "CALPolitico/PoliticoEditarJson";
            }
            else {
                lugar = "CALPolitico/PoliticoGuardarJson";
                accion = "NUEVO POLITICO";
                urlenvio = basePath + "CALPolitico/PoliticoGuardarJson";
            }

            var dataForm = $('#form_registro_politico').serializeFormJSON();

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
                    ListarPolitico();
                },
                success: function (response) {
                    if (response.respuesta) {
                        $('#politico_id').val("0");
                        $('#nombre').val("");
                        $('#estado').val("0");
                        $("#full-modal_politico").modal("hide");
                        //$("#btnBuscar").click();
                        LimpiarFormValidator();
                        toastr.success("Politico Guardado", "Mensaje Servidor");
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
            url: basePath + "CALPolitico/PoliticoDescargarExcelJson",
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

function ListarPolitico() {
    var url = basePath + "CALPolitico/ListarPoliticoJson";
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
            objetodatatable = $("#tablePolitico").DataTable({
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
                    //{ data: "PoliticoID", title: "ID" },
                    { data: "Nombres", title: "Nombres" },
                    { data: "Apellidos", title: "Apellidos" },
                    { data: "Dni", title: "DOI" },
                    { data: "EntidadEstatal", title: "Entidad Estatal" },
                    { data: "Meses", title: "Meses" },
                    { data: "cargoPoliticoNombre", title: "Cargo Politico" },
                    {
                        data: "Estado", title: "Estado",
                        "render": function (o) {
                            var estado = "INACTIVO";
                            var css = "btn-danger";
                            if (o == 1) {
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
                        data: "PoliticoID", title: "Acción",
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


function ObtenerListaCargoPolitco() {
    $.ajax({
        type: "POST",
        url: basePath + "CALCargoPolitico/ListarCargoPoliticoJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboCargoPolitico").append('<option value="' + value.CargoPoliticoID + '"  >' + value.Nombre + '</option>');
            });
            $("#cboCargoPolitico").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_politico')

            });
            $("#cboCargoPolitico").val(null).trigger("change");
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
        url: basePath + "CALPolitico/ListarPoliticoIdJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ PoliticoID: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {
                response = response.data;
                $("#politico_id").val(response.PoliticoID);
                $("#nombres").val(response.Nombres);
                $("#apellidos").val(response.Apellidos);
                $("#dni").val(response.Dni);
                $("#entidadEstatal").val(response.EntidadEstatal);
                $("#meses").val(response.Meses);
                $("#cboEstado").val(response.Estado == true ? "true" : "false").trigger("change");
                $("#cboCargoPolitico").val(response.CargoPoliticoID).trigger("change");
                $("#cboTipoDocumento").val(response.TipoDOI).trigger("change");
                $("#full-modal_politico").modal("show");
            }


        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};


function LimpiarFormValidator() {
    $("#form_registro_politico").parent().find('div').removeClass("has-error");
    $("#form_registro_politico").parent().find('i').removeAttr("style").hide();
}


function ObtenerTipoDocumento() {
    $.ajax({
        type: "POST",
        url: basePath + "AsistenciaCliente/GetListadoTipoDocumento",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboTipoDocumento").append('<option value="' + value.Id + '"  >' + value.Nombre + '</option>');
            });
            $("#cboTipoDocumento").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_politico')

            });
            $("#cboTipoDocumento").val(null).trigger("change");
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
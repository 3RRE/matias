

$(document).ready(function () {


    $("#cboTipoDocumento").select2({
        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_persona_entidad_publica')

    });
    ObtenerTipoDocumento();


    ListarPersonaEntidadPublica();

    $("#cboEstado").select2({
        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_persona_entidad_publica')

    });
    $("#cboEstado").val(null).trigger("change");

    ObtenerListaCargoEntidad();
    ObtenerListaEntidadPublica();

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
        $("#textPersonaEntidadPublica").text("Editar");
        ObtenerRegistro(id);
        $("#full-modal_persona_entidad_publica").modal("show");
    });

    VistaAuditoria("CALPersonaEntidadPublica/ListadoPersonaEntidadPublica", "VISTA", 0, "", 3);

    $('#btnNuevo').on('click', function (e) {
        LimpiarFormValidator();
        $("#textPersonaEntidadPublica").text("Nuevo");

        $("#persona_entidad_publica_id").val(0);
        $("#nombres").val("");
        $("#apellidos").val("");
        $("#dni").val("");
        $("#meses").val("");
        $("#cboEstado").val(null).trigger("change");
        $("#cboCargoEntidad").val(null).trigger("change");
        $("#cboEntidadPublica").val(null).trigger("change");
        $("#cboTipoDocumento").val(null).trigger("change");
        $("#full-modal_persona_entidad_publica").modal("show");
    });

    $(document).on("click", ".btnEliminar", function () {
        var elemento = $(this);
        var id = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar Persona Entidad Publica?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "CALPersonaEntidadPublica/PersonaEntidadPublicaEliminarJson",
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
                            ListarPersonaEntidadPublica();
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

    $("#form_registro_persona_entidad_publica")
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
                cargoEntidadID: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                entidadPublicaID: {
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
        $("#form_registro_persona_entidad_publica").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_persona_entidad_publica").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#persona_entidad_publica_id").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "CALPersonaEntidadPublica/PersonaEntidadPublicaEditarJson";
                accion = "ACTUALIZAR PERSONA ENTIDAD PUBLICA";
                urlenvio = basePath + "CALPersonaEntidadPublica/PersonaEntidadPublicaEditarJson";
            }
            else {
                lugar = "CALPersonaEntidadPublica/PersonaEntidadPublicaGuardarJson";
                accion = "NUEVO PERSONA ENTIDAD PUBLICA";
                urlenvio = basePath + "CALPersonaEntidadPublica/PersonaEntidadPublicaGuardarJson";
            }

            var dataForm = $('#form_registro_persona_entidad_publica').serializeFormJSON();

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
                    ListarPersonaEntidadPublica();
                },
                success: function (response) {
                    if (response.respuesta) {
                        $('#persona_entidad_publica_id').val("0");
                        $('#nombre').val("");
                        $('#estado').val("0");
                        $("#full-modal_persona_entidad_publica").modal("hide");
                        LimpiarFormValidator();
                        //$("#btnBuscar").click();
                        toastr.success("Persona Entidad Publica Guardado", "Mensaje Servidor");
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
            url: basePath + "CALPersonaEntidadPublica/PersonaEntidadPublicaDescargarExcelJson",
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
                        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_persona_entidad_publica')

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

function ListarPersonaEntidadPublica() {
    var url = basePath + "CALPersonaEntidadPublica/ListarPersonaEntidadPublicaJson";
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
            objetodatatable = $("#tablePersonaEntidadPublica").DataTable({
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
                    //{ data: "PersonaEntidadPublicaID", title: "ID" },
                    { data: "Nombres", title: "Nombres" },
                    { data: "Apellidos", title: "Apellidos" },
                    { data: "EntidadPublicaNombre", title: "Entidad Publica" },
                    { data: "Dni", title: "DOI" },
                    { data: "CargoEntidadNombre", title: "Cargo Entidad" },
                    { data: "Meses", title: "Meses" },
                    {
                        data: "FechaRegistro", title: "Fecha Registro",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY");
                        }
                    },
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

                    {
                        data: "PersonaEntidadPublicaID", title: "Acción",
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

function ObtenerListaCargoEntidad() {
    $.ajax({
        type: "POST",
        url: basePath + "CALCargoEntidad/ListarCargoEntidadJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboCargoEntidad").append('<option value="' + value.CargoEntidadID + '"  >' + value.Nombre + '</option>');
            });
            $("#cboCargoEntidad").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_persona_entidad_publica')

            });
            $("#cboCargoEntidad").val(null).trigger("change");
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

function ObtenerListaEntidadPublica() {
    $.ajax({
        type: "POST",
        url: basePath + "CALEntidadPublica/ListarEntidadPublicaJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboEntidadPublica").append('<option value="' + value.EntidadPublicaID + '"  >' + value.Nombre + '</option>');
            });
            $("#cboEntidadPublica").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_persona_entidad_publica')

            });
            $("#cboEntidadPublica").val(null).trigger("change");
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
        url: basePath + "CALPersonaEntidadPublica/ListarPersonaEntidadPublicaIdJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ PersonaEntidadPublicaID: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {
                response = response.data;
                $("#persona_entidad_publica_id").val(response.PersonaEntidadPublicaID);
                $("#nombres").val(response.Nombres);
                $("#apellidos").val(response.Apellidos);
                $("#dni").val(response.Dni);
                $("#meses").val(response.Meses);
                $("#cboEstado").val(response.Estado).trigger("change");
                $("#cboCargoEntidad").val(response.CargoEntidadID).trigger("change");
                $("#cboEntidadPublica").val(response.EntidadPublicaID).trigger("change");
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
    $("#form_registro_persona_entidad_publica").parent().find('div').removeClass("has-error");
    $("#form_registro_persona_entidad_publica").parent().find('i').removeAttr("style").hide();
}
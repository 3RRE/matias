

$(document).ready(function () {

    ListarTimador();


    $("#cboEstado").select2({
        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_timador')

    });
    $("#cboSustentoLegal, #cboConAtenuante, #cboTipoDocumento").select2({
        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_timador')
    });
    $("#cboSustentoLegalIncidencia, #cboEstadoIncidencia").select2({
        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_timador_incidencia')
    });

    $('#cboConAtenuante').on('change', function () {
        const valor = $(this).val();

        if (valor === "true") {
            $('#containerDescripcionAtenuante').removeClass('hidden');
        } else {
            $('#containerDescripcionAtenuante').addClass('hidden');
            $('#descripcionAtenuante').val('');
        }
    });

    $(document).on('keypress', '#dni', function (event) {
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
        $("#textTimador").text("Editar");
        ObtenerRegistro(id);
        $("#full-modal_timador").modal("show");
    });

    $(document).on("click", ".btnEditarIncidencia", function () {
        LimpiarFormValidator();
        var id = $(this).data("id");
        $("#timador_incidencia_id_incidencia").val(0);
        $("#textTimadorIncidencia").text("Editar");
        ObtenerRegistroIncidencia(id);
        $("#full-modal_timador_incidencia").modal("show");
    });

    $(document).on("click", ".btnAgregarIncidencia", function () {
        LimpiarFormValidator();

        var id = $(this).data("id");
        var timadorid = $(this).data("timadorid");
        $("#textTimadorIncidencia").text("Nueva");
        ObtenerListaSalaIncidencia();
        $("#timador_incidencia_id").val(id);
        $("#timador_incidencia_id_incidencia").val(0);
        $("#observacionIncidencia").val("");
        $("#cboSustentoLegalIncidencia").val(null).trigger("change");
        $("#cboEstadoIncidencia").val(null).trigger("change");

        $("#detallesIncidencia").hide();

        $("#full-modal_timador_incidencia").modal("show");
    });

    $(document).on("click", ".btnEliminarIncidencia", function () {
        var elemento = $(this);
        var id = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar Incidencia?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "CALPersonaProhibidoIngreso/IncidenciaEliminarJson",
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

                            ListarTimador();
                            $("#full-modal_timador_incidencias").modal("hide");
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


    $('.btnGuardarIncidencia').on('click', function (e) {
        $("#form_registro_timador_incidencia").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_timador_incidencia").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#timador_incidencia_id_incidencia").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "CALPersonaProhibidoIngreso/IncidenciaEditarJson";
                accion = "ACTUALIZAR INCIDENCIA";
                urlenvio = basePath + "CALPersonaProhibidoIngreso/IncidenciaEditarJson";
            }
            else {
                lugar = "CALPersonaProhibidoIngreso/IncidenciaGuardarJson";
                accion = "NUEVA INCIDENCIA";
                urlenvio = basePath + "CALPersonaProhibidoIngreso/IncidenciaGuardarJson";
            }

            //var dataForm = $('#form_registro_timador_incidencia').serializeFormJSON();
            let dataForm = new FormData(document.getElementById("form_registro_timador_incidencia"))

            //console.log(dataForm);
            //console.log(id);

            
            $.ajax({
                url: urlenvio,
                type: "POST",
                method: "POST",
                contentType: false,
                data: dataForm,
                cache: false,
                processData: false,
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (response) {
                    if (response.respuesta) {
                        LimpiarFormValidator();
                        // $("form input,select,textarea").val(""); 
                        //window.location.reload()
                        ListarTimador();
                        $("#full-modal_timador_incidencia").modal("hide");
                        $("#full-modal_timador_incidencias").modal("hide");
                        toastr.success(response.mensaje, "Mensaje Servidor");
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor")
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                }
            });
            /*
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
                    ListarTimador();
                },
                success: function (response) {
                    if (response.respuesta) {
                        $('#timador_id').val("0");
                        $('#nombre').val("");
                        $('#estado').val("0");
                        $("#full-modal_timador").modal("hide");
                        //$("#btnBuscar").click();
                        toastr.success("Timador Guardado", "Mensaje Servidor");
                    }
                    else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    toastr.error("Error Servidor", "Mensaje Servidor");
                }
            });*/

        }

    });


    $(document).on("click", ".btnVerIncidencias", function () {
        LimpiarFormValidator();
        var id = $(this).data("id");
        $("#tableTimadorIncidenciasID").val(id);
        $("#full-modal_timador_incidencias").modal("show");
    });

    $(document).on('shown.bs.modal', '#full-modal_timador_incidencias', function () {
        var id = $("#tableTimadorIncidenciasID").val();
        GetIncidencias(id);
    })

    ObtenerListaBanda();
    ObtenerTipoDocumento();
    ObtenerListaTipoTimador();

    VistaAuditoria("CALPersonaProhibidoIngreso/ListadoPersonaProhibidoIngreso", "VISTA", 0, "", 3);

    $('#btnNuevo').on('click', function (e) {
        LimpiarFormValidator();
        ObtenerListaSala();
        $("#textTimador").text("Nuevo");

        $("#timador_id").val(0);
        $("#nombre").val("");
        $("#apellidoPaterno").val("");
        $("#apellidoMaterno").val("");
        $("#dni").val("");
        $("#observacion").val("");
        $("#cboEstado").val(null).trigger("change");
        $("#cboProhibir").val("0").trigger("change");
        $("#cboSustentoLegal").val(null).trigger("change");
        $("#cboTipoDocumento").val(null).trigger("change");
        $("#cboBanda").val(null).trigger("change");
        $("#cboSala").val(null).trigger("change");
        $("#cboTipoTimador").val(null).trigger("change");
        //$("#cboSala").attr("disabled", false);

        $("#cboConAtenuante").val('false').trigger("change");
        $('#containerdescripcionAtenuante').addClass('hidden');
        $('#descripcionAtenuante').val('');

        $("#detallesIncidencia").show();


        $("#full-modal_timador").modal("show");
    });

    $(document).on("click", ".btnEliminar", function () {
        var elemento = $(this);
        var id = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar Persona Prohibido Ingreso?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "CALPersonaProhibidoIngreso/PersonaProhibidoIngresoEliminarJson",
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
                            ListarTimador();
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

    $("#form_registro_timador")
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
                apellidoPaterno: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                apellidoMaterno: {
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
                codSala: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                bandaID: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                tipoTimadorID: {
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
                sustentoLegal: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                descripcionAtenuante: {
                    validators: {
                        callback: {
                            message: ' ',
                            callback: function (value, validator, $field) {
                                const estado = $('#cboConAtenuante').val();
                                if (estado === "true") {
                                    return $.trim(value) !== '';
                                }
                                return true;
                            }
                        }
                    }
                }
            }
        })
        .on('success.field.bv', function (e, data) {
            e.preventDefault();
            var $parent = data.element.parents('.form-group');
            $parent.removeClass('has-success');
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
        });



    $("#form_registro_timador_incidencia")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                codSala: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                observacion: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                sustentoLegal: {
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
        $("#form_registro_timador").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_timador").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#timador_id").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "CALPersonaProhibidoIngreso/PersonaProhibidoIngresoEditarJson";
                accion = "ACTUALIZAR PERSONA PROHIBIDO INGRESO";
                urlenvio = basePath + "CALPersonaProhibidoIngreso/PersonaProhibidoIngresoEditarJson";
            }
            else {
                lugar = "CALPersonaProhibidoIngreso/PersonaProhibidoIngresoGuardarJson";
                accion = "NUEVO PERSONA PROHIBIDO INGRESO";
                urlenvio = basePath + "CALPersonaProhibidoIngreso/PersonaProhibidoIngresoGuardarJson";
            }

            //var dataForm = $('#form_registro_timador').serializeFormJSON();
            let dataForm = new FormData(document.getElementById("form_registro_timador"))

            $.ajax({
                url: urlenvio,
                type: "POST",
                method: "POST",
                contentType: false,
                data: dataForm,
                cache: false,
                processData: false,
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (response) {
                    if (response.respuesta) {
                        LimpiarFormValidator();
                        // $("form input,select,textarea").val(""); 
                        window.location.reload()
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor")
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                }
            });
            /*
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
                    ListarTimador();
                },
                success: function (response) {
                    if (response.respuesta) {
                        $('#timador_id').val("0");
                        $('#nombre').val("");
                        $('#estado').val("0");
                        $("#full-modal_timador").modal("hide");
                        //$("#btnBuscar").click();
                        toastr.success("Timador Guardado", "Mensaje Servidor");
                    }
                    else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    toastr.error("Error Servidor", "Mensaje Servidor");
                }
            });*/

        }

    });


    $(document).on("click", "#btnExcel", function () {
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "CALPersonaProhibidoIngreso/PersonaProhibidoIngresoDescargarExcelJson",
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

function ListarTimador() {
    var url = basePath + "CALPersonaProhibidoIngreso/ListarPersonaProhibidoIngresoJson";
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
            objetodatatable = $("#tableTimador").DataTable({
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
                    //{ data: "TimadorID", title: "ID" },
                    { data: "NombreCompleto", title: "Nombre Completo" },
                    { data: "DNI", title: "DOI" },
                    {
                        data: "FechaRegistro", title: "Fecha Registro",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY");
                        }
                    },/*
                    {
                        data: "Imagen", title: "Foto"
                        , "render":
                            function (value) {
                                let myLink = ` <img src="data:image/png;base64, ${value}"  width="40" height="40" alt="Ludópata" />`
                                return myLink;
                            },
                        "orderable": false,
                        className: "text-center",
                    },*/
                    {
                        data: "Foto", title: "Foto"
                        , "render":
                            function (value) {
                                let myLink = ` <img src="${value}"  width="40" height="40" alt="PersonaProhibidoIngreso" />`
                                return myLink;
                            },
                        "orderable": false,
                        className: "text-center",
                    },
                    { data: "EmpleadoNombreCompleto", title: "Usuario" },
                    { data: "SalaNombreCompuesto", title: "Sala" },
                    /*
                    {
                        data: "SustentoLegal", title: "Sustento Legal",
                        "render": function (o) {
                            var estado = "--";
                            var css = "btn-info";
                            if (o == 0) {
                                estado = "SIN SUSTENTO LEGAL"
                                css = "btn-danger";
                            }
                            if (o == 1) {
                                estado = "CON SUSTENTO LEGAL"
                                css = "btn-success";
                            }
                            return '<span class="label ' + css + '">' + estado + '</span>';

                        }
                    },*/
                    /*{
                        data: "FechaRegistro", title: "Fecha Registro",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY hh:mm");
                        }
                    },*/
                    {
                        data: "ConAtenuante", title: "Atenuante",
                        "render": function (o) {
                            var estado = "--";
                            var css = "btn-info";
                            if (o == 0) {
                                estado = "SIN ATENUANTE"
                                css = "btn-danger";
                            }
                            if (o == 1) {
                                estado = "CON ATENUANTE"
                                css = "btn-success";
                            }
                            return `<span class="label${css}">${estado}</span>`;

                        }
                    },
                    {
                        data: "TimadorID", title: "Acción",
                        "bSortable": false,
                        "render": function (o) {
                            return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> 
                                    <button type="button" class="btn btn-xs btn-primary btnAgregarIncidencia" data-id="${o}"><i class="glyphicon glyphicon-zoom-in"></i></button>
                                    <button type="button" class="btn btn-xs btn-success btnVerIncidencias" data-id="${o}"><i class="glyphicon glyphicon-eye-open"></i></button>`;
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

function ObtenerListaSala() {
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
            $("#cboSala").html("");
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_timador')
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

function ObtenerListaSalaIncidencia() {
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
            $("#cboSalaIncidencia").html("");
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboSalaIncidencia").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSalaIncidencia").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_timador_incidencia')
            });
            $("#cboSalaIncidencia").val(null).trigger("change");
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

function ObtenerListaTipoTimador() {
    $.ajax({
        type: "POST",
        url: basePath + "CALTipoTimador/ListarTipoTimadorJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboTipoTimador").append('<option value="' + value.TipoTimadorID + '"  >' + value.Descripcion + '</option>');
            });
            $("#cboTipoTimador").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_timador')

            });
            $("#cboTipoTimador").val(null).trigger("change");
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

function ObtenerListaBanda() {
    $.ajax({
        type: "POST",
        url: basePath + "CALBanda/ListarBandaJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboBanda").append('<option value="' + value.BandaID + '"  >' + value.Descripcion + '</option>');
            });
            $("#cboBanda").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_timador')

            });
            $("#cboBanda").val(null).trigger("change");
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
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_timador')

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

function ObtenerRegistro(id) {
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CALPersonaProhibidoIngreso/ListarPersonaProhibidoIngresoIdJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ TimadorID: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            if (response.respuesta) {

                response = response.data;

                $("#timador_id").val(response.TimadorID);
                $("#nombre").val(response.Nombre);
                $("#apellidoPaterno").val(response.ApellidoPaterno);
                $("#apellidoMaterno").val(response.ApellidoMaterno);
                $("#dni").val(response.DNI);
                $("#observacion").val(response.Observacion);
                $("#File").val('');
                $("#File").text(response.Foto);
                $("#Foto").val(response.Foto);
                $("#Imagen").val(response.Imagen);
                $("#cboEstado").val(response.Estado).trigger("change");
                $("#cboProhibir").val(response.Prohibir).trigger("change");
                $("#cboSustentoLegal").val(response.SustentoLegal).trigger("change");
                $("#cboBanda").val(response.BandaID).trigger("change");
                $("#cboTipoDocumento").val(response.TipoDOI).trigger("change");
                $("#cboTipoTimador").val(response.TipoTimadorID).trigger("change");

                $("#cboConAtenuante").val(response.ConAtenuante.toString()).trigger("change");
                if (response.ConAtenuante) {
                    $("#descripcionAtenuante").val(response.DescripcionAtenuante);
                }

                $("#cboSala").html("");
                $("#cboSala").append('<option value="'+response.CodSala+'"  >' + response.SalaNombre + '</option>');
                $("#cboSala").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_timador')
                });
                $("#cboSala").val(response.CodSala).trigger("change");
                //$("#cboSala").attr("disabled", true);

                $("#detallesIncidencia").hide();
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};

function ObtenerRegistroIncidencia(id) {
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CALPersonaProhibidoIngreso/ListarIncidenciaId",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            if (response.respuesta) {

                response = response.data;

                $("#timador_incidencia_id_incidencia").val(response.TimadorIncidenciaID);

                $("#timador_incidencia_id").val(response.TimadorID);
                //$("#timadorIncidenciaID").val(response.TimadorIncidenciaID);
                $("#observacionIncidencia").val(response.Observacion);
                $("#cboSustentoLegalIncidencia").val(response.SustentoLegal).trigger("change");

                $("#cboSalaIncidencia").html("");
                $("#cboSalaIncidencia").append('<option value="' + response.CodSala + '"  >' + response.SalaNombre + '</option>');
                $("#cboSalaIncidencia").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_timador_incidencia')
                });
                $("#cboSalaIncidencia").val(response.CodSala).trigger("change");

                $("#cboEstadoIncidencia").val(response.Estado).trigger("change");
                                
                $("#detallesIncidencia").hide();
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};

function GetIncidencias(TimadorID) {

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CALPersonaProhibidoIngreso/ListarIncidenciasxPersonaProhibidoIngreso",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ TimadorID }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {

                respuesta = response.data
                objetodatatable = $("#tableIncidencias").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "ordering": true,
                    "scrollCollapse": true,
                    "scrollX": true,
                    "paging": true,
                    "aaSorting": [],
                    "autoWidth": true,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response.data,
                    columns: [
                        { data: "SalaNombre", title: "Sala" },
                        { data: "Observacion", title: "Observacion" },
                        {
                            data: "SustentoLegal", title: "Sustento Legal",
                            "render": function (o) {
                                var estado = "--";
                                var css = "btn-info";
                                if (o == 0) {
                                    estado = "SIN SUSTENTO LEGAL"
                                    css = "btn-danger";
                                }
                                if (o == 1) {
                                    estado = "CON SUSTENTO LEGAL"
                                    css = "btn-success";
                                }
                                return '<span class="label ' + css + '">' + estado + '</span>';

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
                            data: "TimadorIncidenciaID", title: "Acción",
                            "bSortable": false,
                            "render": function (o, value, oData) {
                                return `<button type="button" class="btn btn-xs btn-warning btnEditarIncidencia" data-id="${o}" data-timadorid="${oData.TimadorID}"><i class="glyphicon glyphicon-pencil"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnEliminarIncidencia" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
                            }
                        }
                    ],

                    "initComplete": function (settings, json) {



                    },
                });
            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function LimpiarFormValidator() {
    $("#form_registro_timador").parent().find('div').removeClass("has-error");
    $("#form_registro_timador").parent().find('i').removeAttr("style").hide();
}
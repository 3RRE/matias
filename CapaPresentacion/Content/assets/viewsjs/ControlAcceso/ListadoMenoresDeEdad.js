

$(document).ready(function () {


    $("#cboTipoDocumento").select2({
        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_robastackersbilletero')

    });
    ObtenerTipoDocumento();


    ListarRobaStackersBilletero();


    $("#cboEstado").select2({
        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_robastackersbilletero')

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
        $("#textRobaStackersBilletero").text("Editar");
        ObtenerRegistro(id);
        $("#full-modal_robastackersbilletero").modal("show");
    });


    VistaAuditoria("CALRobaStackersBilletero/ListadoRobaStackersBilletero", "VISTA", 0, "", 3);

    $('#btnNuevo').on('click', function (e) {
        LimpiarFormValidator();
        ObtenerListaSala();
        $("#textRobaStackersBilletero").text("Nuevo");

        $("#robastackersbilletero_id").val(0);
        $("#nombre").val("");
        $("#apellidoPaterno").val("");
        $("#apellidoMaterno").val("");
        $("#dni").val("");
        $("#observacion").val("");
        $("#cboEstado").val(null).trigger("change");
        $("#cboSala").val(null).trigger("change");
        $("#cboTipoDocumento").val(null).trigger("change");
        $("#cboSala").attr("disabled", false);
        $("#full-modal_robastackersbilletero").modal("show");
    });

    $(document).on("click", ".btnEliminar", function () {
        var elemento = $(this);
        var id = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar Roba Stackers Billetero?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "CALRobaStackersBilletero/RobaStackersBilleteroEliminarJson",
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
                            ListarRobaStackersBilletero();
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

    $("#form_registro_robastackersbilletero")
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
                doi: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                sala: {
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
        $("#form_registro_robastackersbilletero").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_robastackersbilletero").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#robastackersbilletero_id").val();
            console.log(id)
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "CALMenorDeEdad/EditarRegistroMenorEdad";
                accion = "Agregar registro menor de edad";
                urlenvio = basePath + "CALMenorDeEdad/EditarRegistroMenorEdad";
            }
            else {
                lugar = "CALMenorDeEdad/MenorDeEdadGuardarJson";
                accion = "Agregar registro menor de edad";
                urlenvio = basePath + "CALMenorDeEdad/MenorDeEdadGuardarJson";
            }

            //var dataForm = $('#form_registro_robastackersbilletero').serializeFormJSON();
            let dataForm = new FormData(document.getElementById("form_registro_robastackersbilletero"))
            console.log(dataForm)
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
                    ListarRobaStackersBilletero();
                },
                success: function (response) {
                    if (response.respuesta) {
                        $('#robastackersbilletero_id').val("0");
                        $('#nombre').val("");
                        $('#estado').val("0");
                        $("#full-modal_robastackersbilletero").modal("hide");
                        //$("#btnBuscar").click();
                        toastr.success("RobaStackersBilletero Guardado", "Mensaje Servidor");
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
            url: basePath + "CALMenorDeEdad/RegistroMenorDeEdadExcel",
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

function ListarRobaStackersBilletero() {
    var url = basePath + "CALMenorDeEdad/listarMenoresDeEdad";
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
            objetodatatable = $("#tableRobaStackersBilletero").DataTable({
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
                    //{ data: "RobaStackersBilleteroID", title: "ID" },
                    { data: "Nombre", title: "Nombres" },
                    { data: "ApellidoPaterno", title: "Apellido Paterno" },
                    { data: "ApellidoMaterno", title: "Apellido Materno" },
                    { data: "Doi", title: "DOI" },
                    { data: "NombreTipoDoi", title: "Tipo documento" },
                    {
                        data: "FechaRegistro", title: "Fecha Registro",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY");
                        }
                    },
                    { data: "NombreEmpleado", title: "Usuario" },
                    { data: "NombreSala", title: "Sala" },
                    {
                        data: "Estado", title: "Estado",
                        "render": function (o) {
                            if (o == 0) {
                                return "Inactivo"
                            } else {
                                return "Activo"
                            }
                        }
                    },
                    
                    {
                        data: "IdMenor", title: "Acción",
                        "bSortable": false,
                        "render": function (o) {
                            return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
                                    
                                    `;
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
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_robastackersbilletero')
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


function ObtenerRegistro(id) {

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CALMenorDeEdad/ListarMenrEdadIdJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ id:id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            console.log(response)
            if (response.respuesta) {

                response = response.data;

                $("#robastackersbilletero_id").val(response.IdMenor);
                $("#nombre").val(response.Nombre);
                $("#apellidoPaterno").val(response.ApellidoPaterno);
                $("#apellidoMaterno").val(response.ApellidoMaterno);
                $("#doi").val(response.Doi);
                $("#cboEstado").val(response.Estado).trigger("change");
                $("#cboTipoDocumento").val(response.TipoDoi).trigger("change");


                $("#cboSala").html("");
                $("#cboSala").append('<option value="' + response.Sala + '"  >' + response.NombreSala + '</option>');
                $("#cboSala").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_robastackersbilletero')
                });
                $("#cboSala").val(response.Sala).trigger("change");
                //$("#cboSala").attr("disabled", true);

            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};


function LimpiarFormValidator() {
    $("#form_registro_robastackersbilletero").parent().find('div').removeClass("has-error");
    $("#form_registro_robastackersbilletero").parent().find('i').removeAttr("style").hide();
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
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_robastackersbilletero')

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
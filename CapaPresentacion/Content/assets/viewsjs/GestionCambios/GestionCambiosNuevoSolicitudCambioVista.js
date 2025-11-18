$(document).ready(function () {
    $("#frmNuevoSolicitudCambio")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                SalaId: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                Descripcion: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                cboSistema: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                TipoCambioId: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                ModuloId: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                }
            }
        })
        .on('success.field.bv', function (e, data) {
            e.preventDefault();
            var $parent = data.element.parents('.form-group');
            // Remove the has-success class
            $parent.removeClass('has-success');
            // Hide the success icon
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
        });
    ObtenerSistemas();
    ObtenerTipoCambio();
    ObtenerListaSalas();

    $('#cboSistema').select2();
    $('#cboModulo').select2();
    $('#cboTipoCambio').select2();
    $("#cboSala").select2();

    $("#btnListar").click(function () {
        window.location.replace(basePath +"/GestionCambios/GestionCambiosSolicitudCambioVista")
    });

    $('#btnBorrador').click(function () {
        $("#frmNuevoSolicitudCambio").data('bootstrapValidator').resetForm();
        var validar = $("#frmNuevoSolicitudCambio").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "GestionCambios/GuardarSolicitudCambioBorradorJson";
            var redirectUrl = basePath + "GestionCambios/GestionCambiosSolicitudCambioVista";
            var dataForm = $('#frmNuevoSolicitudCambio').serializeFormJSON();
           
            $.when(dataAuditoria(1, "#frmNuevoSolicitudCambio", 3, "GestionCambios/GuardarSolicitudCambioBorradorJson", "NUEVO BORRADOR")).then(function (response, textStatus) {
                enviarDataPost(url, dataForm, false, false, redirectUrl, true);
            });
        }
    });

    $('#btnEnviar').click(function () {
        $("#frmNuevoSolicitudCambio").data('bootstrapValidator').resetForm();
        var validar = $("#frmNuevoSolicitudCambio").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "GestionCambios/EnviarSolicitudCambioJson";
            var redirectUrl = basePath + "GestionCambios/GestionCambiosSolicitudCambioVista";
            var dataForm = $('#frmNuevoSolicitudCambio').serializeFormJSON();
           
            $.when(dataAuditoria(1, "#frmNuevoSolicitudCambio", 3, "GestionCambios/EnviarSolicitudCambioJson", "ENVIAR")).then(function (response, textStatus) {
                enviarDataPost(url, dataForm, false, false, redirectUrl, true);
            });
        }
    });

    $("#cboSistema").change(function () {
        var id = $("#cboSistema").val();
        $('#cboModulo').empty();
        $('#cboModulo').append('<option value="">--Seleccione--</option>');
        $.ajax({
            type: "POST",
            url: basePath + "GestionCambios/BuscarModuloSistemaJson?id="+id,
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                var datos = result.data;
                $.each(datos, function (index, value) {
                    $("#cboModulo").append('<option value="' + value.ModuloId + '"  data-id="' + value.ModuloId + '"  >' + value.Descripcion + '</option>');
                });
            },
            error: function (request, status, error) {
                toastr.error("Error", "Mensaje Servidor");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }            
        });
    });

    $('.modal1').on('shown.bs.modal', function () {
        //var nombre = $("#hddmodal").val();
        //setTimeout('listar' + nombre + '()', 1);
        debugger
        var table = $('#TablaHistorial').DataTable();
        table
            .order([1, 'asc'])
            .draw();
    });
});

function ObtenerSistemas() {
    $.ajax({
        type: "POST",
        url: basePath + "GestionCambios/CargarListaSistemasJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            $.each(datos, function (index, value) {
                $("#cboSistema").append('<option value="' + value.SistemaId + '"  data-id="' + value.SistemaId + '"  >' + value.Descripcion + '</option>');
            });
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function ObtenerTipoCambio() {
    $.ajax({
        type: "POST",
        url: basePath + "GestionCambios/ListarTipoCambioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            $.each(datos, function (index, value) {
                $("#cboTipoCambio").append('<option value="' + value.TipoCambioId + '"  data-id="' + value.TipoCambioId + '"  >' + value.Descripcion + '</option>');
            });
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function ObtenerListaSalas() {
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
            var datos = result.data;
            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '" data-id="' + value.CodSala + '">' + value.Nombre + '</option>');
            });
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
VistaAuditoria("GestionCambios/GestionCambiosNuevoSolicitudCambioVista", "VISTA", 0, "", 3);
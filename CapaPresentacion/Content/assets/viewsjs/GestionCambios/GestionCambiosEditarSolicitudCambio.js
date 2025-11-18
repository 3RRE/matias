$(document).ready(function () {
    ObtenerEstados();
    $("#txtDescripcion").val(solicitud.Descripcion);
    $("#txtSolicitudId").val(solicitud.SolicitudId);
    $("#cboEstadoCambio").select2();
    $("#frmEditarSolicitudCambio")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                EstadoSolicitudCambioId: {
                    validators: {
                        notEmpty: {
                            message: 'Seleccione un Estado'
                        }
                    }
                },
                Descripcion: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese una descripcion'
                        }
                    }
                },
                Observacion: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese una Observación'
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

    $("#btnListar").click(function () {
        window.location.replace(basePath + "/GestionCambios/GestionCambiosSolicitudCambioVista")
    });

    $("#btnModificar").click(function () {
        $("#frmEditarSolicitudCambio").data('bootstrapValidator').resetForm();
        var validar = $("#frmEditarSolicitudCambio").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "GestionCambios/GestionCambiosModificarSolicitudCambioJson";
            var redirectUrl = basePath + "GestionCambios/GestionCambiosSolicitudCambioVista";
            var dataForm = $('#frmEditarSolicitudCambio').serializeFormJSON();
            var validar = $("#txtvalidar").val();
            var textoDescripcion = "";
            if (validar == 0) {
                textoDescripcion = "EDITAR SOLICITUD CAMBIO";
            }
            else {
                textoDescripcion = "REVISION DE SOLICITUD";
            }
            $.when(dataAuditoria(1, "#frmEditarSolicitudCambio", 3, "GestionCambios/GestionCambiosEditarSolicitudCambioVista", textoDescripcion)).then(function (response, textStatus) {
                enviarDataPost(url, dataForm, false, false, redirectUrl, false);
            });
        }
    });

    $.when(VistaAuditoria("GestionCambios/GestionCambiosEditarSolicitudCambioVista", "VISTA", 1, "#frmEditarSolicitudCambio", 3)).then(function (response, textStatus) {
        dataAuditoria(0, "#frmEditarSolicitudCambio", 3);
    });

    //llenarSelect(basePath + "Empleado/CargoListarJson", {}, "cboCargo", "CargoID", "Descripcion", empleado.CargoID);

});

function ObtenerEstados() {
    //ListarEstadoSolicitudCambioJsonç
    var validar = $("#txtvalidar").val();
    $.ajax({
        type: "POST",
        url: basePath + "GestionCambios/ListarEstadoSolicitudCambioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            $.each(datos, function (index, value) {
                if (validar == 0) {
                    if (value.EstadoSolicitudCambioId != 1 && value.EstadoSolicitudCambioId != 2) {
                    } else {
                        $("#cboEstadoCambio").append('<option value="' + value.EstadoSolicitudCambioId + '"  data-id="' + value.EstadoSolicitudCambioId + '"  >' + value.Descripcion + '</option>');
                    }
                } else {
                    if (value.EstadoSolicitudCambioId == 1) {
                    } else {
                        $("#cboEstadoCambio").append('<option value="' + value.EstadoSolicitudCambioId + '"  data-id="' + value.EstadoSolicitudCambioId + '"  >' + value.Descripcion + '</option>');
                    }
                }
                
                $("#cboEstadoCambio").val(solicitud.EstadoSolicitudCambioId);
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
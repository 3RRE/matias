$(document).ready(function () {
    ObtenerSistemas();
    $('#cboSistema').select2();
    $("#frmNuevoModulo")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                SistemaId: {
                    validators: {
                        notEmpty: {
                            message: 'Elija un Sistema'
                        }
                    }
                },
                Descripcion: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese un Descripción'
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

    $('.btnGuardar').on('click', function () {
        $("#frmNuevoModulo").data('bootstrapValidator').resetForm();
        var validar = $("#frmNuevoModulo").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "GestionCambios/ModuloGuardarJson";
            var redirectUrl = basePath + "GestionCambios/GestionCambiosListadoModuloVista";
            var dataForm = $('#frmNuevoModulo').serializeFormJSON();
            setCookie("datainicial", "");
            $.when(dataAuditoria(1, "#frmNuevoModulo", 3, "GestionCambios/ModuloGuardarJson", "NUEVO MODULO")).then(function (response, textStatus) {
                enviarDataPost(url, dataForm, false, false, redirectUrl, true);
            });

        }
    });
    $('.btnListar').on('click', function () {
        window.location.replace(basePath + "GestionCambios/GestionCambiosListadoModuloVista");
    });
    VistaAuditoria("GestionCambios/GestionCambiosNuevoModuloVista", "VISTA", 0, "", 3);
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

$(document).ready(function () {
    $.when(llenarSelect(basePath + "Empleado/EmpleadoListarJson", {}, "cboEmpleado", "EmpleadoID", "nombre")).then(function (response, textStatus) {
        $("#cboEmpleado").select2();
    });
    $.when(llenarSelect(basePath + "Tecnico/AreaTechListarJson", {}, "cboAreaTech", "AreaTechId", "Descripcion")).then(function (response, textStatus) {
        $("#cboAreaTech").select2();
    });
    $.when(llenarSelect(basePath + "Tecnico/NivelTecnicoListarJson", {}, "cboNivelTecnico", "NivelTecnicoId", "Descripcion")).then(function (response, textStatus) {
        $("#cboNivelTecnico").select2();
    });

    $(document).on('click', '.btnListar', function () {
        var url = basePath + "Tecnico/TecnicoListarVista";
        window.location.replace(url);
    });

    $(document).on('click', '.btnGuardar', function () {

        $("#frmNuevo").data('bootstrapValidator').resetForm();
        var validar = $("#frmNuevo").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "Tecnico/TecnicoInsertarJson";
            var redirectUrl = basePath + "Tecnico/TecnicoListarVista";
            var dataForm = $('#frmNuevo').serializeFormJSON();
            setCookie("datainicial", "");
            $.when(dataAuditoria(1, "#frmNuevo", 3, "Tecnico/TecnicoInsertarJson", "NUEVO TECNICO")).then(function (response, textStatus) {
                enviarDataPost(url, dataForm, false, false, redirectUrl, true);
            });
        }
    });

    VistaAuditoria("Tecnico/TecnicoInsertarVista", "VISTA", 0, "", 3);

});

$("#frmNuevo")
    .bootstrapValidator({
        //container: '#messages',
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            EmpleadoId: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            AreaTechId: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            NivelTecnicoId: {
                validators: {
                    notEmpty: {
                        message: ''
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

$(document).ready(function () {
    $("#frmEditarSistema")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                Descripcion: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese La Descripcion'
                        }
                    }
                },
                
                Version: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Version, Obligatorio'
                        }
                    }
                },
                
                
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
    $("#txtSistemaId").val(sistema.SistemaId);
    
    $("#txtDescripcion").val(sistema.Descripcion);
    $("#txtVersion").val(sistema.Version);
    $("#txtFechaRegistro").val(sistema.FechaRegistro);   
    $('input').iCheck('update');

    $.when(VistaAuditoria("GestionCambios/GestionCambiosSistemaEditarVista", "VISTA", 1, "#frmEditarSistema", 3)).then(function (response, textStatus) {
        dataAuditoria(0, "#frmEditarSistema", 3);
    });

    $('.btnGuardar').on('click', function (e) {
        $("#frmEditarSistema").data('bootstrapValidator').resetForm();
        var validar = $("#frmEditarSistema").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "GestionCambios/SistemaEditarJson";
            var redirectUrl = basePath + "GestionCambios/GestionCambiosSistemaListadoVista";
            var dataForm = $('#frmEditarSistema').serializeFormJSON();
            setCookie("datainicial", "");
            $.when(dataAuditoria(1, "#frmEditarSistema", 3, "GestionCambios/SistemaEditarJson", "EDITAR SISTEMA")).then(function (response, textStatus) {
                enviarDataPost(url, dataForm, false, false, redirectUrl, false);
            });
        }

    });

    $('.btnListar').on('click', function (e) {
        window.location.replace(basePath + "GestionCambios/GestionCambiosSistemaListadoVista");
    });

    $("input").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            $('.btnGuardar').click();
        }

    });

    $(document).on('click', '.btnModal', function (e) {
        var nombre = $(this).data("nombre");
        $("#hddmodal").val(nombre);
        $('#bodyModal').html("");
        $('#default-modal-label').html("Mantenimiento " + nombre);
        var redirectUrl = basePath + "Empleado/Mantenimiento" + nombre+"ParcialVista";
        var ubicacion = "bodyModal";
   
        $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
            $("#modalGroup").modal("show");
        });
    });

    $('#modalGroup').on('shown.bs.modal', function () {
        var nombre = $("#hddmodal").val();
        setTimeout('listar' + nombre + '()', 1);
    });
    var dateNow = new Date();
    dateNow.setDate(dateNow.getDate() - 6570);
    $(".dateOnlyEmpleado").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });

   
});


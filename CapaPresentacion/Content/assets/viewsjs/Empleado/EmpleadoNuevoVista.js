$(document).ready(function () {
    $("#frmNuevoEmpleado")
        .bootstrapValidator({ 
            //container: '#messages',
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                CargoID: {
                    validators: {
                        notEmpty: {
                            message: 'ingrese Cargo, Obligatorio'
                        }
                    }
                },
                ApellidosPaterno: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Apellido Paterno, Obligatorio'
                        }
                    }
                },
                ApellidosMaterno: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Apellido Materno, Obligatorio'
                        }
                    }
                },
                Nombres: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Nombres, Obligatorio'
                        }
                    }
                },
                FechaNacimiento: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Fecha Nacimiento, Obligatorio'
                        },
                        date: {
                            format: 'DD/MM/YYYY',
                            message: 'La Fecha de Nacimiento no es valida'
                        }

                    }
                },
                DOIID: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Tipo Documento, Obligatorio'
                        }
                    }
                },
                DOI: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese nro de Documento, Obligatorio'
                        }                        
                    }
                },
                Telefono: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese nro Telefono, Obligatorio'
                        }
                    }
                },
                Direccion: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Direccion De Domicilio, Obligatorio'
                        }
                    }
                },
                MailJob: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Correo Electronico, Obligatorio'
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
    
    llenarSelect(basePath + "Empleado/CargoListarJson", {}, "cboCargo", "CargoID", "Descripcion");
    llenarSelect(basePath + "Empleado/TipoDocumentoListarJson", {}, "cboTipoDocumento", "DOIID", "DESCRIPCION"); 

    $('.btnGuardar').on('click', function (e) {
        $("#genMasculino").val("M");
        $("#genFemenino").val("F");
        $("#frmNuevoEmpleado").data('bootstrapValidator').resetForm();
        var validar = $("#frmNuevoEmpleado").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = basePath + "Empleado/EmpleadoGuardarJson";
            var redirectUrl = basePath + "Empleado/EmpleadoListadoVista";
            var dataForm = $('#frmNuevoEmpleado').serializeFormJSON();
            debugger
            
            
            setCookie("datainicial","");
            $.when(dataAuditoria(1, "#frmNuevoEmpleado", 3, "Empleado/EmpleadoGuardarJson", "NUEVO EMPLEADO")).then(function (response, textStatus) {
                enviarDataPost(url, dataForm, false, false, redirectUrl, true);
            });
        }

    });

    $('.btnListar').on('click', function (e) {
        window.location.replace(basePath + "Empleado/EmpleadoListadoVista");
    });

    $("input").keydown(function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            $('.btnGuardar').click();
        }

    });

    $(document).on('click', '.btnModal', function (e) {
        var nombre = $(this).data("nombre");
        console.log(nombre);
        $("#hddmodal").val(nombre);
        $('#bodyModal').html("");
        $('#default-modal-label').html("Mantenimiento " + nombre);
        var redirectUrl = basePath + "Empleado/Mantenimiento" + nombre + "ParcialVista";
        console.log(redirectUrl);
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

    VistaAuditoria("Empleado/EmpleadoNuevoVista", "VISTA", 0, "", 3);
});

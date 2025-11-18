
//"use strict";
empleadodatatable = "";
function afterTableInitialization(settings, json) {
    tableAHORA = settings.oInstance.api();

}


function ListarEmpleados() {
    var url = basePath + "Empleado/EmpleadoListarJson";
    var data = {}; var respuesta = ""; aaaa = "";
    ajaxhr = $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            //console.log(response.data)
            respuesta = response.data
            var roles = response.roles;
            var rolUsuarios = response.rolUsuarios;

            objetodatatable = $("#tableEmpleado").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "sScrollX": "100%",
                "paging": true,
                "ordering": true,
                "aaSorting": [],
                "autoWidth": false,
                "bAutoWidth": true,
                "bProcessing": true,
                "bDeferRender": true,
                "initComplete": function (settings, json) {

                    //   afterTableInitialization(settings,json)
                    $('button#excel,a#pdf,a#imprimir').off("click").on('click', function () {
                        ocultar = ["Accion"];//array de columnas para ocultar , usar titulo de columna
                        columna_cambio = [{
                            nombre: "Estado",
                            render: function (o) {
                                valor = "";
                                if (o == 1) {
                                    valor = "Habilitado";
                                }
                                else { valor = "Deshabilitado"; }
                                return valor;
                            }
                        }]
                        cabecerasnuevas = [];
                        //cabecerasnuevas.push({ nombre: "cabecera", valor: "vdfcs" });
                        //tituloreporte = "Reporte Empleados";
                        funcionbotonesnuevo({
                            botonobjeto: this, tablaobj: objetodatatable, ocultar: ocultar/*, tituloreporte: tituloreporte*/, cabecerasnuevas: cabecerasnuevas, columna_cambio: columna_cambio
                        });
                        VistaAuditoria("Empleado/EmpleadoListadoExcel", "EXCEL", 0, "", 3);
                    });
                },
                data: response.data,
                columns: [
                    { data: "nombre", title: "Nombres" },
                    { data: "CargoNombre", title: "Cargo" },
                    { data: "MailJob", title: "Correo" },
                    { data: "UsuarioNombre", title: "Usuario" },
                    {
                        data: "UsuarioID", title: "Rol",
                        "render": function (usuarioId) {
                            var rolesAsignados = [];
                            $.each(roles, function (key, rol) {
                                $.each(rolUsuarios, function (index, rolUsuario) {
                                    if (rolUsuario.UsuarioID == usuarioId && rolUsuario.WEB_RolID == rol.WEB_RolID) {
                                        var rolName = rol.WEB_RolNombre;
                                        rolesAsignados.push(rolName);
                                    }
                                });
                            });

                            return rolesAsignados.join(', ');
                        }
                    },
                    {
                        data: "EstadoEmpleado", title: "Estado",
                        "render": function (o,i,j) {
                            var selectedOptions = "";
                            //console.log(j)
                            var id = j.EmpleadoID;
                            if (o == 1) {
                                selectedOptions = '<option value="1" selected>Habilitado</option>' +
                                    '<option value="0">Deshabilitado</option>';
                            } else {
                                selectedOptions = '<option value="1">Habilitado</option>' +
                                    '<option value="0" selected>Deshabilitado</option>';
                            }
                            return '<select class="form-control input-sm selectEmp auditoriaFocus" data-id="' + id + '" id="EstadoEmpleado" name="EstadoEmpleado">' + selectedOptions + '</select>';
                        }
                    },
                    {
                        data: "EmpleadoID",
                        "bSortable": false,
                        "render": function (o) {
                            return '<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="' + o + '"><i class="glyphicon glyphicon-pencil"></i></button> ' +
                                '<button type="button" class="btn btn-xs btn-info btnUsuario" data-id="' + o + '"><i class="glyphicon glyphicon-user"></i></button>' +
                                //' <button type="button" class="btn btn-xs btn-success btnEliminarfoto" data-id="' + o + '"><i class="glyphicon glyphicon-picture"></i></button>' +
                                ' <button type="button" class="btn btn-xs btn-warning btnimei" data-id="' + o + '"><i class="glyphicon glyphicon-phone"></i></button>';
                        }
                    }
                ],
                "drawCallback": function (settings) {
                    $('.btnEditar').tooltip({
                        title: "Editar"
                    });
                    $('.btnUsuario').tooltip({
                        title: "Usuario"
                    });
                    //$('.btnEliminarfoto').tooltip({
                    //    title: "Eliminar Foto"
                    //});
                    $('.btnimei').tooltip({
                        title: "Equipo"
                    });
                }
            });

            $('#tableEmpleado tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            //if (xmlHttpRequest.status == 400) {
            //    toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            //} else {
            //    toastr.error("Error Servidor", "Mensaje Servidor");
            //}
        }
    });
    AbortRequest.open()
};

VistaAuditoria("Empleado/EmpleadoListadoVista", "VISTA", 0, "", 3);
$(document).ready(function () {
    ListarEmpleados();
    $('#btnNuevoEmpleado').on('click', function (e) {
        window.location.replace(basePath + "Empleado/EmpleadoNuevoVista");
    });

    $(document).on('click', '.selectEmp', function (e) {
        var id = $(this).data("id");
        $('#Id_Empleado').remove();
        $(this).parent("td").append('<input type="hidden" class="record" name="Id_Empleado" id="Id_Empleado" value="'+id+'" >');
        dataAuditoria(0, $(this).parent("td"), 3);
        console.log("__",id)
    });

    $(document).on('change', '.selectEmp', function (e) {
        var estado = $(this).val();
        var texto = $(this).find("option:selected").text();
        var idEmpleado = jQuery(this).closest('td').next('td').find("button.btnEditar").data("id");
        var data = { EmpleadoId: idEmpleado, EstadoEmpleado: estado, EstadoEmpleado_text: texto }
        dataAuditoria(1, $(this).parent("td"), 3, "Empleado/EstadoEmpleadoActualizarJson", "CAMBIAR ESTADO");
        var url = basePath + "Empleado/EstadoEmpleadoActualizarJson";
        DataPostWithoutChange(url, data, false);
    });

    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var url = basePath + "Empleado/EmpleadoRegistroVista/Registro" + id;
        window.location.replace(url);
    });

    $(document).on('click', '.btnUsuario', function (e) {
        var id = $(this).data("id");
        var url = basePath + "Empleado/UsuarioEmpleadoIdObtenerJson";
        var data = { empleadiId: id };
        DataPostSend(url, data, false).done(function (response) {

            if (response) {
                if (response.respuesta) {
                    var data = response.respuesta;
                    var contentText;
                    var redirectUrl;
                    var btnText;
                    if (data.UsuarioID == 0) {
                        contentText = "No tiene un Usuario Registrado, ¿Desea Registrar Usuario?";
                        redirectUrl = basePath + "Usuario/UsuarioInsertarVista";
                        btnText = "Registrar Usuario";

                    } else {
                        contentText = "Empleado con Usuario Registrado : " + data.UsuarioNombre;
                        redirectUrl = basePath + "Usuario/UsuarioRegistroVista/Registro" + data.UsuarioID;
                        btnText = "Detalle Usuario";
                    }

                    var js = $.confirm({
                        icon: 'fa fa-spinner fa-spin',
                        title: 'Mensaje Servidor',
                        theme: 'black',
                        animationBounce: 1.5,
                        columnClass: 'col-md-6 col-md-offset-3',
                        confirmButtonClass: 'btn-info',
                        cancelButtonClass: 'btn-warning',
                        confirmButton: btnText,
                        cancelButton: 'Cerrar',
                        content: contentText,
                        confirm: function () {
                            window.location.replace(redirectUrl);
                        },
                        cancel: function () {

                        },
                    });
                } else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            } else {
                toastr.error("Error no se logro conectar al servidor", "Mensaje Servidor");
            }
        }).fail(function (x) {
           // toastr.error("Error Fallo Servidor", "Mensaje Servidor");
        });
    });

    //$(document).on('click', '.btnEliminarfoto', function (e) {
    //    var id = $(this).data("id");
    //    var url = basePath + "AsistenciaEmpleado/EmpleadoIdObtenerJson";
    //    var data = { emp_id: id };
    //    DataPostSend(url, data, false).done(function (response) {

    //        if (response) {
    //            if (response.respuesta) {
    //                var data = response.respuesta;
    //                var imagenstring;
    //                if (data.emp_foto !=null) {
    //                    imagenstring = '<img style="width: 200px;height: 246px;" src="data:image/png;base64,' + data.emp_foto+'">';

    //                }
    //                else {
    //                    imagenstring = '<img style="width: 200px;height: 246px;" src="'+basePath+'Content/assets/images/cbimage.jpg"><div>Sin Imagen</div>';
    //                }

    //                var url_ = basePath + "AsistenciaEmpleado/EmpleadoEliminarImagenJson";
    //                var contentText;
    //                let dataForm={
    //                    emp_id:data.EmpleadoID
    //                }
    //                contentText = imagenstring+"¿Esta seguro de ELIMINAR Imagen de Empleado?";
    //                var js = $.confirm({
    //                    icon: 'fa fa-spinner fa-spin',
    //                    title: 'Mensaje Servidor',
    //                    theme: 'black',
    //                    animationBounce: 1.5,
    //                    columnClass: 'col-md-6 col-md-offset-3',
    //                    confirmButtonClass: 'btn-info',
    //                    cancelButtonClass: 'btn-warning',
    //                    confirmButton: "Aceptar",
    //                    cancelButton: 'Cerrar',
    //                    content: contentText,
    //                    confirm: function () {
    //                        DataPostSend(url_, dataForm, false).done(function (response) {
    //                            if (response) {
    //                                if (response.respuesta) {
    //                                    toastr.success(response.mensaje, "Mensaje Servidor");
    //                                } else {
    //                                    toastr.error(response.mensaje, "Mensaje Servidor");
    //                                }
    //                            } else {
    //                                toastr.error("Error no se logro conectar al servidor", "Mensaje Servidor");
    //                            }
    //                        }).fail(function (x) {
    //                            // toastr.error("Error Fallo Servidor", "Mensaje Servidor");
    //                        });
    //                    },
    //                    cancel: function () {

    //                    },
    //                });
    //            } else {
    //                toastr.error(response.mensaje, "Mensaje Servidor");
    //            }
    //        } else {
    //            toastr.error("Error no se logro conectar al servidor", "Mensaje Servidor");
    //        }
    //    }).fail(function (x) {
    //        // toastr.error("Error Fallo Servidor", "Mensaje Servidor");
    //    });

     
       
       
    //});

    ////dispositivo
    $(document).on("click", ".btnimei", function () {
        var emp_id = $(this).data("id");
        var dataForm = { emp_id: emp_id };
        var url = basePath +"AsistenciaEmpleado/EmpleadoDispositivoIdObtenerJson";
        DataPostSend(url, dataForm, false).done(function (response) {

            if (response) {
                if (response.respuesta) {
                    var empleadodis = response.data;
                    $("#emd_imei").val(empleadodis.emd_imei);
                    if (empleadodis.emd_id == 0) {
                        $("#emd_estado").val(1);
                    }
                    else {
                        $("#emd_estado").val(empleadodis.emd_estado);
                    }
                    $("#form_dispositivo #emd_id").val(empleadodis.emd_id);
                    $("#form_dispositivo #emp_idd").val(emp_id);
                    $("#modalFormularioDispositivo").modal("show");
                } else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            } else {
                toastr.error("Error no se logro conectar al servidor", "Mensaje Servidor");
            }
        }).fail(function (x) {
            // toastr.error("Error Fallo Servidor", "Mensaje Servidor");
        });
      
    })

    $(document).on('click', ".btn-guardarDispositivo", function (e) {
       
        $("#form_dispositivo").data('bootstrapValidator').resetForm();
        var validar = $("#form_dispositivo").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var dataForm = $('#form_dispositivo').serializeFormJSON();
            var url = "";
            if ($("#emd_id").val() == 0) {
                url = basePath +"AsistenciaEmpleado/EmpleadoDispositivoNuevoJson";
            }
            else {
                url = basePath +"AsistenciaEmpleado/EmpleadoDispositivoEditarJson";
            }
            DataPostSend(url, dataForm, false).done(function (response) {
                console.log(response);
                if (response) {
                    if (response.respuesta) {
                        $("#modalFormularioDispositivo").modal("hide");
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                } else {
                    toastr.error("Error no se logro conectar al servidor", "Mensaje Servidor");
                }
            }).fail(function (x) {
                // toastr.error("Error Fallo Servidor", "Mensaje Servidor");
            });
            
        } else {
            messageResponse({
                text: "Complete los campos Obligatorios",
                type: "error"
            })
        }
    });


    $("#form_dispositivo")
        .bootstrapValidator({
            //container: '#messages',
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                emd_imei: {
                    validators: {
                        notEmpty: {
                            message: 'ingrese IMEI, Obligatorio'
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

});
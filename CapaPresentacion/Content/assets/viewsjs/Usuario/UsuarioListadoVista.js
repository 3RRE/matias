
//"use strict";
VistaAuditoria("Usuario/UsuarioListadoVista", "VISTA", 0, "", 3);
$(document).ready(function () {
    listarUsuarios();

    $('#btnNuevoUsuario').on('click', function (e) {
        window.location.replace(basePath + "Usuario/UsuarioInsertarVista");
    });

    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var url = basePath + "Usuario/UsuarioRegistroVista/Registro" + id;
        window.location.replace(url);
    });

    $(document).on('click', '.btnUsuario', function (e) {
        var id = $(this).data("id");
        var url = basePath + "Usuario/UsuarioEmpleadoIDObtenerJson";
        var data = { usuarioId: id };
        DataPostSend(url, data, false).done(function (response) {
            if (response) {
                var data = response.respuesta;
                if (data.UsuarioID > 0) {
                    var data = response.respuesta;
                    window.location.replace(basePath + "Empleado/EmpleadoRegistroVista/Registro" + data.EmpleadoID);
                } else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }

            } else {
                toastr.error("Error no se logro conectar al servidor", "Mensaje Servidor");
            }
        }).fail(function (x) {
            toastr.error("Error Fallo Servidor", "Mensaje Servidor");
        });

    });
    empleadodatatable = "";

    function listarUsuarios() {

        var url = basePath + "Usuario/UsuarioListadoJson";
        var data = {};
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
                var roles = response.roles;
                var rolUsuarios = response.rolUsuarios;

                objetodatatable = $("#tableUsuario").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "scrollCollapse": true,
                    "scrollX": false,
                    "paging": true,
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response.data,
                    columns: [
                        { data: "NombreEmpleado", title: "Nombre Empleado" },
                        { data: "UsuarioNombre", title: "Usuario" },
                        { data: "FailedAttempts", title: "Intentos Fallidos" },
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
                            data: null,
                            "bSortable": false,
                            "render": function (value, type, oData, meta) {
                                var botones = '<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="' + oData.UsuarioID + '"><i class="glyphicon glyphicon-pencil"></i></button> ' +
                                    '<button type="button" style="display:none;" class="btn btn-xs btn-info btnUsuario" data-id="' + oData.UsuarioID + '"><i class="glyphicon glyphicon-user"></i></button>';
                                var atributo = (oData.FailedAttempts >= 5) ? { "clase": "btn btn-xs btn-danger", "title": "¿Desbloquear?", "posicion": "top", "name": "link-desbloquear-usuario" } : { "clase": "btn btn-xs btn-default", "title": "¿Bloquear?", "posicion": "top", "name": "link-bloquear-usuario" };
                                var si = ' <div class="btn-group btnUsu"><button data-id ="' + oData.UsuarioID + '" data-nombre="' + oData.UsuarioNombre + '" class="' + atributo.clase + '" title = "' + atributo.title + '" data-placement="' + atributo.posicion + '" name="' + atributo.name + '"><i class="glyphicon glyphicon-lock"></i></button> </div>';
                                return botones + si;
                            }
                        }
                    ]

                    ,
                    "initComplete": function (settings, json) {

                       $('#btnExcel').off("click").on('click', function () {

                            cabecerasnuevas = [];
                           ocultar = [];
                           ocultar.push("Accion");
                            definicioncolumnas = [];

                            funcionbotonesnuevo({
                                botonobjeto: this, ocultar: ocultar,
                                tablaobj: objetodatatable,
                                cabecerasnuevas: cabecerasnuevas,
                                definicioncolumnas: definicioncolumnas
                            });
                            VistaAuditoria("Empleado/EmpleadoListadoExcel", "EXCEL", 0, "", 3);
                        });



                    },
                    "drawCallback": function (settings) {
                        $('.btnEditar').tooltip({
                            title: "Editar"
                        });
                        $('.btnUsu').tooltip({
                            title: "Usuario"
                        });
                    }
                });
            },


            error: function (xmlHttpRequest, textStatus, errorThrow) {
                console.log("errorrrrrrrr");
            }
            ,

        });
    };


    //Bloquear/Desbloquear Usuarios
    $(document).on('click', 'button[name="link-bloquear-usuario"]', function (e) {
        var usuario = $(this);
        var $button = $(this);
        var usuarioID = usuario.attr("data-id");
        var usuarioNombre = usuario.attr("data-nombre");
        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Desea bloquear al usuario ' + usuarioNombre + ' ?',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: 'OK',
            cancelButton: 'Cancelar',
            content: false,
            confirm: function () {
                $.LoadingOverlay("show");
                $.post(basePath + 'Usuario/UsuarioBloquearJson', { UsuarioID: usuarioID })
                    .done(function (response) {
                        
                        if (response) {
                           var datainicial = {
                                UsuarioID: usuarioID,
                                UsuarioNombre: usuarioNombre,
                                UsuarioBloqueado: "NO"
                            };
                           var datafinal = {
                                UsuarioID: usuarioID,
                                UsuarioNombre: usuarioNombre,
                                UsuarioBloqueado: "SI"
                            };
                            dataAuditoriaJSON(3, "Usuario/UsuarioBloquearJson", "BLOQUEO USUARIO", datainicial, datafinal);

                            var tr = $button.closest("tr");
                            tr.find('td').eq(2).text(5);
                            usuario.removeAttr('class')
                                .addClass('btn btn-xs btn-danger')
                                .tooltip('destroy')
                                .attr('title', '¿Desbloquear?')
                                .attr('name', 'link-desbloquear-usuario')
                                .attr('data-placement', 'top')
                                .tooltip();
                            toastr.success('El usuario ' + usuarioNombre.toLowerCase() + ' ha sido bloqueado con éxito.');
                        } else {
                            toastr.error("Ocurrió un error intenta denuevo.");
                        }
                    })
                    .fail(function () {
                        $.LoadingOverlay("hide")
                        toastr.error("Ocurrió un error intenta denuevo.");
                    })
                    .always($.LoadingOverlay("hide"));
            },
            cancel: function () {
            }
        });

    });

    $(document).on('click', 'button[name="link-desbloquear-usuario"]', function (e) {
        var usuario = $(this);
        var $button = $(this);
        var usuarioID = usuario.attr("data-id");
        var usuarioNombre = usuario.attr("data-nombre");

        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Desea desbloquear al usuario ' + usuarioNombre + ' ?',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: 'OK',
            cancelButton: 'Cancelar',
            content: false,
            confirm: function () {
                $.LoadingOverlay("show");
                $.post(basePath + 'Usuario/UsuarioDesbloquearJson', { UsuarioID: usuarioID })
                    .done(function (response) {
                        if (response) {
                            var tr = $button.closest("tr");
                            tr.find('td').eq(2).text(0);
                            var datainicial = {
                                UsuarioID: usuarioID,
                                UsuarioNombre: usuarioNombre,
                                UsuarioBloqueado: "SI"
                            };
                            var datafinal = {
                                UsuarioID: usuarioID,
                                UsuarioNombre: usuarioNombre,
                                UsuarioBloqueado: "NO"
                            };
                           
                            dataAuditoriaJSON(3, "Usuario/UsuarioDesbloquearJson", "DESBLOQUEO USUARIO", datainicial, datafinal);

                            usuario.removeAttr('class')
                                .addClass('btn btn-xs btn-default')
                                .tooltip('destroy')
                                .attr('title', '¿Bloquear?')
                                .attr('name', 'link-bloquear-usuario')
                                .attr('data-placement', 'top')
                                .tooltip();
                            toastr.success('El usuario ' + usuarioNombre.toLowerCase() + ' ha sido desbloqueado con éxito.');
                        } else {
                            toastr.error("Ocurrió un error intenta denuevo.");
                        }
                    })
                    .fail(function () {
                        toastr.error("Ocurrió un error intenta denuevo.");
                        $.LoadingOverlay("hide")
                    })
                    .always($.LoadingOverlay("hide"));
            },
            cancel: function () {
            }
        });
    });
});
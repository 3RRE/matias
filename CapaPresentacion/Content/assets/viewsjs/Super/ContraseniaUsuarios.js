
//VistaAuditoria("Super/CambiarContraseniaVista ", "VISTA", 0, "", 3);
$(document).ready(function () {
    listarUsuarios();

    $("#UsuarioContraseña").keypress(function (e) {
        
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            $(".btnGuardar").trigger("click");
        }
    });

    $(document).on('click', '.btnEditar', function (e) {

        let id = $(this).data("id");
        let row = $(this).data("row");
        $("#RowID").val(row);
        $("#UsuarioID").val(id);
        $("#full-modal_cambiar").modal("show");
    });

    $('#full-modal_cambiar').on('show.bs.modal', function () {
        $('button.bv-hidden-submit').remove();
        $('#UsuarioContraseña').focus();

    })

    

    $("#form_registro_cambiar")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                UsuarioContraseña: {
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
        e.preventDefault()
        $("#form_registro_cambiar").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_cambiar").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            urlenvio = basePath + "Super/UsuarioCambiarContrasenia";


            console.log('entra');

            var dataForm = $('#form_registro_cambiar').serializeFormJSON();

            $.ajax({
                url: urlenvio,
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(dataForm),
                beforeSend: function () {
                    $.LoadingOverlay("show");
                    console.log('intenta');
                },
                complete: function () {
                    $.LoadingOverlay("hide");
                    console.log('completo');
                },
                success: function (response) {
                    if (response.respuesta) {

                        console.log('todo gud');

                        let id = $("#UsuarioID").val();
                        let row = $("#RowID").val();

                        let selectedRows = objetodatatable.rows({ search: 'applied' })
                            .data();
                        let selected = selectedRows.filter((item) => item.UsuarioID == id)[0];

                        selected.FailedAttempts = 0
                        objetodatatable.row(row).data(selected)

                        $('#UsuarioID').val("0");
                        $('#UsuarioContraseña').val("");
                        $("#full-modal_cambiar").modal("hide");
                        toastr.success("Contraseña Cambiada", "Mensaje Servidor");
                    }
                    else {
                        console.log('todo f');
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    console.log('error');
                    toastr.error("Error Servidor", "Mensaje Servidor");
                }
            });

        }

    });


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
                $.post(basePath + 'Super/UsuarioBloquearJson', { UsuarioID: usuarioID })
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
                            dataAuditoriaJSON(3, "Super/UsuarioBloquearJson", "BLOQUEO USUARIO", datainicial, datafinal);

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
                $.post(basePath + 'Super/UsuarioDesbloquearJson', { UsuarioID: usuarioID })
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

                            dataAuditoriaJSON(3, "Super/UsuarioDesbloquearJson", "DESBLOQUEO USUARIO", datainicial, datafinal);

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
                        "render": function (value, type, oData, y) {
                            var botones = `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${oData.UsuarioID}" data-row="${y.row}" data-col="${y.col}"><i class="glyphicon glyphicon-pencil"></i></button> `;
                            var atributo = (oData.FailedAttempts >= 5) ? { "clase": "btn btn-xs btn-danger", "title": "¿Desbloquear?", "posicion": "top", "name": "link-desbloquear-usuario" } : { "clase": "btn btn-xs btn-default", "title": "¿Bloquear?", "posicion": "top", "name": "link-bloquear-usuario" };
                            var si = ' <div class="btn-group btnUsu"><button data-id ="' + oData.UsuarioID + '" data-nombre="' + oData.UsuarioNombre + '" class="' + atributo.clase + '" title = "' + atributo.title + '" data-placement="' + atributo.posicion + '" name="' + atributo.name + '"><i class="glyphicon glyphicon-lock"></i></button> </div>';

                            //console.log(botones +si);

                            return botones + si;
                        }
                    }
                ]

                ,
                "initComplete": function (settings, json) {




                },
                "drawCallback": function (settings) {
                    $('.btnEditar').tooltip({
                        title: "Editar"
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
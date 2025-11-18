
VistaAuditoria("Usuario/CambiarContraseniaVista ", "VISTA", 0, "", 3);
$(document).ready(function () {
    listarUsuarios();

    $(document).on('click', '.btnEditar', function (e) {

        let id = $(this).data("id");
        let row = $(this).data("row");
        $("#RowID").val(row);
        $("#UsuarioID").val(id);
        $("#full-modal_cambiar").modal("show");
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
                                var botones = `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${oData.UsuarioID}" data-row="${y.row}" data-col="${y.col}"><i class="glyphicon glyphicon-pencil"></i></button> ` ;
                                var atributo = (oData.FailedAttempts >= 5) ? { "clase": "btn btn-xs btn-danger", "title": "¿Desbloquear?", "posicion": "top", "name": "link-desbloquear-usuario" } : { "clase": "btn btn-xs btn-default", "title": "¿Bloquear?", "posicion": "top", "name": "link-bloquear-usuario" };
                                return botones;
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
        $("#form_registro_cambiar").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_cambiar").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            lugar = "Usuario/UsuarioCambiarContrasenia";
            accion = "CAMBIAR CONTRASEÑA";
            urlenvio = basePath + "Usuario/UsuarioCambiarContrasenia";
            



            var dataForm = $('#form_registro_cambiar').serializeFormJSON();

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
                },
                success: function (response) {
                    if (response.respuesta) {

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
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    toastr.error("Error Servidor", "Mensaje Servidor");
                }
            });

        }

    });


});
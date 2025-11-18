VistaAuditoria("Rol/ListadoRol", "VISTA", 0, "", 3);
"use strict";

$(document).ready(function () {

    listarRol();

    $('#btnNuevoRol').on('click', function (e) {
        window.location.replace(basePath + "Rol/NuevoRol");
    });

    $(document).on('click', '.selectEmp', function (e) {
        var id = $(this).data("id");
        var nombre = $(this).data("nombre");
        $('#Id_Rol').remove();
        $('#Nombre_Rol').remove();
        $(this).parent("td").append('<input type="hidden" class="record" name="Id_Rol" id="Id_Rol" value="' + id + '" >');
        $(this).parent("td").append('<input type="hidden" class="record" name="Nombre_Rol" id="Nombre_Rol" value="' + nombre + '" >');
        dataAuditoria(0, $(this).parent("td"), 3);
    });

    $(document).on('change', '.selectEmp', function (e) {
        var estado = $(this).val();
        var idRol = jQuery(this).closest('td').next('td').find("button.btnEditar").data("id");
        var data = { rolId: idRol, estado: estado }
        dataAuditoria(1, $(this).parent("td"), 3, "Rol/ActualizarEstadoRol", "CAMBIAR ESTADO");
        var url = basePath + "Rol/ActualizarEstadoRol";
        DataPostWithoutChange(url, data, false);
    });

    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var url = basePath + "Rol/RegistroRol/Registro" + id;
        window.location.replace(url);
    });

    $(document).on('click', '.btnEliminar', function (e) {
        var id = $(this).data("id");
        var nombre = $(this).data("nombre");
        var data = { rolId: id }
        var url = basePath + "Rol/DeleteRolId";
        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: 'Mensaje Servidor',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: 'SI',
            cancelButton: 'No',
            content: "¿Esta Seguro de Eliminar el Registro?",
            confirm: function () {
                DataPostSend(url, data, false).done(function (response) {
                    if (response) {
                        if (response.respuesta) {
                            toastr.success("Registro Eliminado, Correctamente", "Mensaje Servidor");

                            var datafinal = {
                                Id_Rol: id,
                                Nombre_Rol: nombre,
                            };
                            dataAuditoriaJSON(3, "Rol/DeleteRolId", "ELIMINAR ROL", "", datafinal);
                            setTimeout(function () {
                                location.reload();
                            }, 1000);

                        } else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }

                    } else {
                        toastr.error("Error no se logro conectar al servidor", "Mensaje Servidor");
                    }
                }).fail(function (x) {
                    toastr.error("Error Fallo Servidor", "Mensaje Servidor");
                });
            },
            cancel: function () {

            }
        });



    });
    var objetodatatable = null;
    function listarRol() {
        var url = basePath + "Rol/GetListadoRol";
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

                objetodatatable = $("#tableRol").DataTable({
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
                        { data: "WEB_RolNombre", title: "Nombre" },
                        { data: "WEB_RolDescripcion", title: "Descripcion" },
                        {
                            data: "WEB_RolEstado", title: "Estado", "width": "120px",
                            "render": function (o, i, j) {
                                var selectedOptions = "";
                                var id = j.WEB_RolID;
                                var nombre = j.WEB_RolNombre;
                                if (o == 1) {
                                    selectedOptions = '<option value="1" selected>Habilitado</option>' +
                                        '<option value="0">Deshabilitado</option>';
                                } else {
                                    selectedOptions = '<option value="1">Habilitado</option>' +
                                        '<option value="0" selected>Deshabilitado</option>';
                                }
                                return '<select class="form-control input-sm selectEmp" data-nombre="'+nombre+'" data-id="' + id + '" id="EstadoRol" name="EstadoRol">' + selectedOptions + '</select>';
                            }
                        },
                        {
                            data: "WEB_RolID",
                            "bSortable": false,
                            "render": function (o, i, j) {
                                var nombre = j.WEB_RolNombre;
                                return '<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="' + o + '"><i class="glyphicon glyphicon-pencil"></i></button> ' +
                                    '<button type="button" class="btn btn-xs btn-danger btnEliminar"  data-nombre="' + nombre +'" data-id="' + o + '"><i class="glyphicon glyphicon-trash"></i></button>';
                            }
                        }
                    ],
                    "drawCallback": function (settings) {
                        $('.btnEditar').tooltip({
                            title: "Editar"
                        });
                        $('.btnEliminar').tooltip({
                            title: "Eliminar"
                        });
                    }
                    ,
                    "initComplete": function (settings, json) {

                        $('a#excel,a#pdf,a#imprimir').on('click', function () {

                            var ocultar = "Accion";
                            funcionbotones({
                                botonobjeto: this, ocultar: ocultar, tablaobj: objetodatatable

                            });


                        });

                    },
                });



            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
                console.log("errorrrrrrrr");
            }
        });
    };

});

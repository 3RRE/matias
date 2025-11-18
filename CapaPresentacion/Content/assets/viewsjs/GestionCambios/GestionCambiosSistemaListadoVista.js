
//"use strict";

$(document).ready(function () {
    listarSistemas();

    $('#btnNuevoSistema').on('click', function (e) {
        window.location.replace(basePath + "GestionCambios/GestionCambiosSistemaInsertarVista");
    });

    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var url = basePath + "GestionCambios/GestionCambiosSistemaEditarVista/Registro" + id;
        window.location.replace(url);
    });

    $(document).on('click', '.btnUsuario', function (e) {
        var id = $(this).data("id");
        
        var data = { usuarioId: id };
       
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
       

    });
    empleadodatatable = "";

    function listarSistemas() {

        var url = basePath + "GestionCambios/SistemaListadoJson";
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

                objetodatatable = $("#tableSistema").DataTable({
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
                       
  
                          { data: "Descripcion", title: "Descripcion" },
  
                          { data: "Version", title: "Version" },
  
                        {
                            data: "FechaRegistro", title: "FechaRegistro",
                            "render": function (value) {
                                return moment(value).format('DD/MM/YYYY');
                            }
                        },
  
                        {
                            data: "FechaModificacion", title: "FechaModificacion",
                            "render": function (value) {
                                return moment(value).format('DD/MM/YYYY');
                            }
                        },
                            
                        {
                            data: "Estado", title: "Estado",
                            
                            "render": function (o, i, j) {
                                var selectedOptions = "";
                                //console.log(j)
                                var id = j.SistemaId;
                                if (o == 1) {
                                    selectedOptions = '<option value="1" selected>Habilitado</option>' +
                                        '<option value="0">Inactivo</option>';
                                } else {
                                    selectedOptions = '<option value="1">Habilitado</option>' +
                                        '<option value="0" selected>Inactivo</option>';
                                }
                                return '<select class="form-control input-sm selectSistema auditoriaFocus" data-id="' + id + '" id="Estado" name="Estado">' + selectedOptions + '</select>';
                            }
                        },
                        {
                            data: "SistemaId",title:"Accion",
                            "bSortable": false,
                            "render": function (value) {
                                return '<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="' + value + '"><i class="glyphicon glyphicon-pencil"></i></button>';
                            }
                        }
                    ]

                    ,
                    "initComplete": function (settings, json) {
                        $('#btnExcel').off("click").on('click', function () {
                           cabecerasnuevas = [];
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
                           ocultar = [];
                           ocultar.push("Accion");
                            definicioncolumnas = [];

                            funcionbotonesnuevo({
                                botonobjeto: this, ocultar: ocultar,
                                tablaobj: objetodatatable,
                                cabecerasnuevas: cabecerasnuevas, columna_cambio: columna_cambio,
                                
                           });
                           VistaAuditoria("GestionCambios/GestionCambiosSistemaListadoExcel", "EXCEL", 0, "", 3);
                        });



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

    $(document).on('click', '.selectSistema', function (e) {
        var id = $(this).data("id");
        $('#Id_Sistema').remove();
        $(this).parent("td").append('<input type="hidden" class="record" name="Id_Sistema" id="Id_Sistema" value="' + id + '" >');
        dataAuditoria(0, $(this).parent("td"), 4);
    });

    $(document).on('change', '.selectSistema', function (e) {
        var estado = $(this).val();
        var texto = $(this).find("option:selected").text();
        var idEmpleado = jQuery(this).closest('td').next('td').find("button.btnEditar").data("id");
        var data = { SistemaId: idEmpleado, Estado: estado, EstadoEmpleado_text: texto }
        dataAuditoria(1, $(this).parent("td"), 4, "GestionCambios/EstadoSistemaActualizarJson", "CAMBIAR ESTADO");
        var url = basePath + "GestionCambios/EstadoSistemaActualizarJson";
        DataPostWithoutChange(url, data, false);
    });

    VistaAuditoria("GestionCambios/SistemaListado", "VISTA", 0, "", 3);
});
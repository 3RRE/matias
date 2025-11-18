
//"use strict";
empleadodatatable = "";


function afterTableInitialization(settings, json) { 
    tableAHORA = settings.oInstance.api(); 
}


function ListarSalas() { 
    var url = basePath + "Sala/ListadoTodosSala";
    var data = {}; var respuesta = ""; aaaa = "";
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
            //console.log(response.data)
            respuesta = response.data
            console.log(respuesta)
            objetodatatable = $("#tableSala").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "paging": true,
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                "initComplete": function (settings, json) {
                     
                    $('a#excel,a#pdf,a#imprimir').off("click").on('click', function () {
                        ocultar = ["Accion"];
                        tituloreporte = "Reporte Salas";
                        funcionbotones({
                            botonobjeto: this, tablaobj: objetodatatable, ocultar: ocultar, tituloreporte: tituloreporte
                        });
                    });
                },
                data: response.data,
                columnDefs: [
                    {
                        targets: [0],
                        className: "text-center"
                    }
                ],
                aaSorting: [[1, 'asc']],
                columns: [
                    {
                        data: null,
                        title: '<i class="glyphicon glyphicon-th-list"></i>',
                        "render": {
                            _: function (value, type, row) {
                                return row.EsPrincipal ? '<i class="glyphicon glyphicon-ok-circle fa-xl text-success"></i>' : '<i class="glyphicon glyphicon-remove-circle text-danger"></i>';
                            },
                            sort: "EsPrincipal"
                        }
                    },
                    { data: "Nombre", title: "Sala" },
                    { data: "NombreCorto", title: "Nombre Corto" },
                    { data: "UrlProgresivo", title: "Url Progresivo" }, 
                    { data: "UrlSalaOnline", title: "Url Sala Online" },
                    {
                        title: "Tipo" ,
                        data: null,
                        "bSortable": false,
                        "render": function (value,type,row) {
                            let span=''
                            if(row.tipo==0){
                                span='SALA'
                            }
                            else if(row.tipo==1){
                                span='RESTAURANTE'
                            }
                            else if(row.tipo==2){
                                span='HOTEL'
                            }
                            else if (row.tipo == 3) {
                                span = 'CANAL ONLINE'
                            }
                            else{
                                span='SALA'
                            }
                            return span
                        }
                    },
                    {
                        data: null, title: "Estado", "render": function (value, type, row) {
                            let select = `<select style="width:100%" class="input-sm selectEstado select${row.CodSala}" data-id=${row.CodSala}>`;

                            if (row.Estado == 1) {
                                select += `<option value="1" selected>Activo</option><option value="0">Inactivo</option>`;
                            }
                            else {
                                select += `<option value="1">Activo</option><option value="0" selected>Inactivo</option>`;
                            }
                            select += `</select>`;
                            return select;   
                        }
                    }, 
                   
                    {
                        data: "CodSala",
                        "bSortable": false,
                        "render": function (o) {
                            return '<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="' + o + '"><i class="glyphicon glyphicon-pencil"></i></button> ' ;
                        }
                    }
                ],
                "drawCallback": function (settings) {
                    $('.btnEditar').tooltip({
                        title: "Editar"
                    });
                },

                "initComplete": function (settings, json) {

                    // $('#btnExcel').off("click").on('click', function () {

                    //     cabecerasnuevas = [];
                    //     definicioncolumnas = [];
                    //     var ocultar = [];
                    //     ocultar.push("Accion");
                    //     funcionbotonesnuevo({
                    //         botonobjeto: this, ocultar: ocultar,
                    //         tablaobj: objetodatatable,
                    //         cabecerasnuevas: cabecerasnuevas,
                    //         definicioncolumnas: definicioncolumnas
                    //     });
                    //     VistaAuditoria("Sala/SalavistaExcel", "EXCEL", 0, "", 3);
                    // });
                    
                },
            });
            $('.btnEditar').tooltip({
                title: "Editar"
            });

        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        }
    });
};


$(document).ready(function (){

    ListarSalas();
    $(document).on("click", "#btnExcel", function () {
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Sala/ListadoTodosSalaExportarExcel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                if (response.respuesta) {
                    let data = response.data;
                    let file = response.excelName;
                    let a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/vnd.ms-excel;base64, " + data;
                    a.download = file;
                    a.click();
                }
            },
            error: function (request, status, error) {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    });
    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var url = basePath + "Sala/SalaModificarVista/" + id;
        window.location.replace(url);
    });

    $(document).on('click', '.btnUsuario', function (e) {
        var id = $(this).data("id");
        var url = basePath + "Sala/GetUsuarioSalaID";
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
                        contentText = "Sala con Usuario Registrado : " + data.UsuarioNombre;
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
            toastr.error("Error Fallo Servidor", "Mensaje Servidor");
        });

    });
    VistaAuditoria("Sala/Salavista", "VISTA", 0, "",3);
    $('#btnNuevaSala').on('click', function (e) {
        window.location.replace(basePath + "Sala/SalaInsertarVista");
    });
    $(document).on("change", ".selectEstado", function () {
        let CodSala = $(this).data("id");
        let Estado = $(this).val();
        let dataForm = {
            CodSala: CodSala,
            Estado: Estado
        }
        $.confirm({
            title: 'Confirmación',
            content: '¿Desea cambiar el estado del registro?',
            confirmButton: 'Si',
            cancelButton: 'No',
            confirm: function () {
                $.ajax({
                    url: basePath+ "Sala/SalaModificarEstadoJson",
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
                            toastr.success("Registro Editado", "Mensaje Servidor");
                        }
                        else {
                            if (Estado == 1) {
                                $('.select' + CodSala).val(0);
                            }
                            else {
                                $('.select' + CodSala).val(1);
                            }
                        }
                    }
                })
            },
            cancel: function () {
                if (Estado == 1) {
                    $('.select' + CodSala).val(0);
                }
                else {
                    $('.select' + CodSala).val(1);
                }
            }
        });
    });
});
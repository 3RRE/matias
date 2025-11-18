$(document).ready(function(){
    ListarTipoCliente();
    $('#btnNuevoRegistro').on('click', function (e) {
        window.location.replace(basePath + "AsistenciaCliente/RegistroTipoCliente");
    });
    $(document).on('click', '.btnEditar', function (e) {
        let id = $(this).data("id");
        let url = basePath + "AsistenciaCliente/EditarTipoCliente/" + id;
        window.location.replace(url);
    });
    $(document).on("click", "#btnExcel", function () {
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "AsistenciaCliente/GetListadoTipoClienteExcel",
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
})
function ListarTipoCliente(){
    let url = basePath + "AsistenciaCliente/GetListadoTipoCliente";
    let data = {};
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
            if(response.respuesta){
                if(response.data.length>0){
                    objetodatatable = $("#tableTipoCliente").DataTable({
                        "bDestroy": true,
                        "bSort": false,
                        "scrollCollapse": true,
                        "scrollX": false,
                        "sScrollX": "100%",
                        "paging": true,
                        "autoWidth": false,
                        "bAutoWidth": true,
                        "bProcessing": true,
                        "bDeferRender": true,
                        data: response.data,
                        columns: [
                            {
                                data:"Nombre", title: "Nombre"
                            },
                            {
                                data:"Descripcion", title: "Descripcion"
                            },
                            {
                                data:null, title: "Estado","render":function(value,type,oData,metadata){
                                    let span="";
                                    if(oData.Estado.trim()=="A"){
                                        span="ACTIVO"
                                    }else{
                                        span="INACTIVO"
                                    }
                                    return span;
                                }
                            },
                            {
                                data: null,
                                title:"ACCIONES",
                                "bSortable": false,
                                "render": function (value, type, oData, meta) {
                                    let botones = '<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="' + oData.Id + '"><i class="glyphicon glyphicon-pencil"></i> Editar</button> ';
                                    return botones;
                                }
                            }
                        ],
                        "initComplete": function (settings, json) {
                            // $(".selectEstado").select2();
                        },
                        "drawCallback": function (settings) {
                        }
                    });
                }
                else{
                    objetodatatable = $("#tableTipoCliente").DataTable({
                        "bDestroy": true,
                        "bSort": false,
                        "scrollCollapse": true,
                        "scrollX": false,
                        "sScrollX": "100%",
                        "paging": true,
                        "autoWidth": false,
                        "bAutoWidth": true,
                        "bProcessing": true,
                        "bDeferRender": true,
                        data: {},
                        columns: [
                            {title: "Nombre"},
                            {title: "Descripcion"},
                            {title: "Estado"},
                            {title:"ACCIONES",}
                        ],
                        "initComplete": function (settings, json) {
                        },
                        "drawCallback": function (settings) {
                        }
                    });
                }
            }
            else{
                toastr.error(response.mensaje,'Mensaje Servidor');
            }
           
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            console.log("errorrrrrrrr");
        }
    });
}
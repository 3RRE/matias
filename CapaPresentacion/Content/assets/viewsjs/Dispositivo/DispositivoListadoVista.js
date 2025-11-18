$(document).ready(function () {   
    ListarDispositivos();
    $('#btnNuevo').on('click', function (e) {
        window.location.replace(basePath + "Dispositivo/DispositivoInsertarVista");
    });  
    $(document).on('click', '.btnEditar', function () {     
        window.location.replace(basePath + "Dispositivo/DispositivoEditarVista?id=" + $(this).data('id'));
    }); 
});
VistaAuditoria("Dispositivo/DispositivoListadoVista", "VISTA", 0, "", 3);

function ListarDispositivos() {
    var url = basePath + "Dispositivo/DispositivoListadoJson";
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
            objetodatatable = $("#table").DataTable({
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
                    { data: "Mac", title: "Mac" }, 
                    {
                        data: "EsActivo", title: "Estado",
                        "render": function (o) {
                            var estadoStr = "";
                            if (o == 1) {
                                estadoStr = 'Activo';
                            } else {
                                estadoStr = 'InActivo';
                            }
                            return estadoStr;
                        }
                    }, 
                    {
                        data: "DispositivoId",
                        title: "Opciones",
                        "bSortable": false,
                        "render": function (o) {
                            return '<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="' + o + '"><i class="glyphicon glyphicon-pencil"></i></button> ';
                        }
                    }

                ],
                "drawCallback": function (settings) {
                    $('.btnEditar').tooltip({
                        title: "Editar"
                    });
                }
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
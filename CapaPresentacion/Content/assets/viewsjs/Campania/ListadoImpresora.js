$(document).ready(function () {

    ListarImpresoraServer();
 
    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var url = basePath + "CampaniaImpresora/ImpresoraModificarVista/" + id;
        window.location.replace(url);
    });

    VistaAuditoria("CampaniaImpresora/ListadoImpresoras", "VISTA", 0, "", 3);

    $('#btnNuevo').on('click', function (e) {
        window.location.replace(basePath + "CampaniaImpresora/ImpresoraNuevoVista");
    });
  
});

function ListarImpresora() {
    var url = basePath + "CampaniaImpresora/ListarImpresorasJson";
    var data = {}; var respuesta = "";
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
            objetodatatable = $("#tableimpresora").DataTable({
                "bDestroy": true,
                "bSort": true,
                "ordering": true,
                "scrollCollapse": true,
                "scrollX": true,
                "paging": true,
                "aaSorting": [],
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                data: response.data,
                columns: [
                    { data: "id", title: "ID" },
                    { data: "sala_nombre", title: "Nombre" },
                    { data: "nombre", title: "Nombre" },
                    { data: "ip", title: "IP" },
                    { data: "puerto", title: "Puerto" },

                    {
                        data: "id",
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
                },

                "initComplete": function (settings, json) {

                   

                },
            });
            $('.btnEditar').tooltip({
                title: "Editar"
            });

        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
           
        }
    });
};

function ListarImpresoraServer() {
    objetodatatable = $("#tableimpresora").DataTable({
        "bDestroy": true,
        "bSort": true,
        "ordering": true,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": true,
        "aaSorting": [],
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        //data: response.data,
        "serverSide": true,
        "searching": { regex: true },
        "processing": true,
        "ajax": {
            url: basePath + "CampaniaImpresora/ListarImpresorasServerJson",
            type: "POST",
            data: function (data) {
                return JSON.stringify(data);
            },
            dataType: "json",
            processData: false,
            contentType: "application/json;charset=UTF-8"
        },
        columns: [
            { data: "id", title: "ID" },
            { data: "sala_nombre", title: "Sala" },
            { data: "nombre", title: "Nombre" },
            { data: "ip", title: "IP" },
            { data: "puerto", title: "Puerto" },

            {
                data: "id",
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
        },
        "fnDrawCallback": function (oSettings) {
           
        },
        "initComplete": function (settings, json) {



        },
    });
    $('.btnEditar').tooltip({
        title: "Editar"
    });
};
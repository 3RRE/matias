$(document).ready(function () {
    ListarModulos();
    $('#btnNuevoModulo').on('click', function () {
        window.location.replace(basePath + "GestionCambios/GestionCambiosNuevoModuloVista");
    });

    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var url = basePath + "GestionCambios/GestionCambiosEditarModuloVista/" + id;
        window.location.replace(url);
    });
    VistaAuditoria("GestionCambios/GestionCambiosListadoModuloVista", "VISTA", 0, "", 3);
});

function ListarModulos() {
    var url = basePath + "GestionCambios/CargarListaModuloJson";
    var data = {}; var respuesta = ""; aaaa = "";
    var addtabla = $(".contenedor_tabla");
    var estados = [
        { "value": "0", "nombre": "Deshabilitado" },
        { "value": "1", "nombre": "Habilitado" }
    ];
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
            respuesta = response.data
            $(addtabla).empty();
            $(addtabla).append('<table id="tableEmpleado" class="table table-condensed table-bordered table-hover"></table>');
            objetodatatable = $("#tableEmpleado").DataTable({
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
                    { data: "ModuloId", title: "Id" },
                    { data: "SistemaDescripcion", title: "Descripcion" },
                    { data: "Descripcion", title: "Descripcion" },
                    { data:"estadomodulo",title:"Estado"},
                    {
                        data: "FechaRegistro", title: "FechaRegistro",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY');
                        }
                    },
                    {
                        data: null, title: "Accion",
                        render: function (value) {
                            return '<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="' + value.ModuloId + '"><i class="glyphicon glyphicon-pencil"></i></button>';
                        }
                    }
                ],
                "initComplete": function (settings, json) {
                    $('button#excel,a#pdf,a#imprimir').off("click").on('click', function () {
                        ocultar = ["Accion"];//array de columnas para ocultar , usar titulo de columna
                        cabecerasnuevas = [];
                        funcionbotonesnuevo({
                            botonobjeto: this, tablaobj: objetodatatable, ocultar: ocultar/*, tituloreporte: tituloreporte*/, cabecerasnuevas: cabecerasnuevas
                        });
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
        }
    });
};


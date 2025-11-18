$(document).ready(function () {

    $(window).on('load', function (){
    var addtabla = $(".contenedor_linkFichas");
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaActivasSinSeguridad",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            $(addtabla).empty();
            $(addtabla).append('<table id="tableLinksFichas" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#tableLinksFichas").DataTable({
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
                data: datos,
                columns: [

                    { data: "Nombre", title: "Sala" },
                    {
                        data: null, title: "Url Ficha",
                        "render": function (value, type, oData, meta) {
                            return basePath + "FichasNuevo?id=" + oData.CodSala;
                        }
                    },
                    {
                        data: null, title: "Accion",
                        "render": function (value, type, oData, meta) {
                            var butom = ` <a class="btn btn-xs btn-warning" target="_blank" href="${basePath}FichasNuevo?id=${oData.CodSala}"><i class="fa external-link-alt"></i> Ir a Link</a>`;
                            return butom;
                        }
                    }
                ]
                ,
                "initComplete": function (settings, json) {



                },
                "drawCallback": function (settings) {

                }
            });

            $('#tableFichaes tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
        }
    });
    });
});
$(document).ready(function () {
    var solicitud_id = $("#IDSolicitud").val();
    HistorialSolicitud(solicitud_id);
});

function HistorialSolicitud(idsolicitud) {
    var addtablaModal = $(".contenedor_tabla_modal");
    var url = basePath + "GestionCambios/ObtenerBusquedaHistorialSolicitudCambioJson?id=" + idsolicitud;
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
            respuesta = response.data;
            //$(addtablaModal).empty();
            //$(addtablaModal).append('<table id="TablaHistorial" class="table table-condensed table-bordered table-hover" style="width:100%;"></table>');
            objetodatatable = $(".TablaHistorial").DataTable({
                "bDestroy": true,
                "bSort": true,
                "paging": true,
                "scrollX": false,
                "sScrollX": "100%",
                "scrollCollapse": true,
                "bProcessing": true,
                "bDeferRender": false,
                "autoWidth": false,
                "bAutoWidth": false,
                data: response.data,
                columns: [
                    //{ data: "HistorialId", title: "HistorialID", order:[0] },
                    //{ data: "EstadoNuevo", title: "Estado Nuevo" },
                    //{
                    //    data: null, title: "Fecha Respuesta",
                    //    "render": function (value) {
                    //        return moment(value).format('DD/MM/YYYY');
                    //    }
                    //},
                    //{ data: "Observacion", title: "Observacion" }
                    { data: "HistorialId" },
                    { data: "EstadoNuevo" },
                    {
                        data: null,
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY');
                        }
                    },
                    { data: "Observacion" }
                ]
            });

        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            //if (xmlHttpRequest.status == 400) {
            //    toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            //} else {
            //    toastr.error("Error Servidor", "Mensaje Servidor");
            //}
        }
    });
}
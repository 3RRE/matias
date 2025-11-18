$(document).ready(function () {
    var solicitud_id = $("#IDSolicitud").val();
    ListarHistoriales(solicitud_id);

    $("#btnListar").click(function () {
        window.location.replace(basePath + "GestionCambios/GestionCambiosSolicitudCambioVista");
    });
});
VistaAuditoria("GestionCambios/GestionCambioLineaTiempoHistorialSolicitudVista", "LINEA DE TIEMPO", 0, "", 3);
function ListarHistoriales(idhistorial)
{
    var addHistorial = $(".contenedor_timeline");
    $(addHistorial).empty();
    var url = basePath + "GestionCambios/ObtenerBusquedaHistorialSolicitudCambioJson?id=" + idhistorial;
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
            respuesta = response.data;
            $(addHistorial).empty();
            var datos = response.data;
            
            $.each(datos, function (index, value) {
                $(addHistorial).append('<li class="highlight-color-green highlight-color-green-icon">\n' +
                    '                            <time class="c_tmtime" datetime="2013-04-11T12:04"><span>' + moment(value.FechaRespuesta).format('DD/MM/YYYY') + '</span> <span>' + moment(value.FechaRespuesta).format('h:mm a')+'</span>\n' +
                    '                            </time>\n' +
                    '                            <span class="icon icon-social-envato"></span>\n' +
                    '                            <div class="c_tmlabel">\n' +
                    '                                <div class="c_tmlabel_inner">\n' +
                    '                                    <h2>' + value.EstadoNuevo+'</h2>\n' +
                    '                                    <p>Observación: '+value.Observacion+'</p>\n' +
                    '                                </div>\n' +
                    '                            </div>\n' +
                    '                        </li>');
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
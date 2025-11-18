$(document).ready(function () {
    $(".date").datetimepicker({
        pickTime: true,
        format: "DD/MM/YYYY hh:mm a"
    });

    $("#table").DataTable({
        destroy: true,
        sort: true, 
        scrollCollapse: true,
        scrollX: true,
        paging: true,
        autoWidth: false,
        processing: true,
        deferRender: true,
        data: [],
        columns: [
            {
                data: "Item",
                title: "Item"
            },
            {
                data: "Fecha_Apertura",
                title: "Fecha Apertura",
                render: function (value) {
                    return window.moment(value).format('DD/MM/YYYY hh:mm a');
                }
            },
            {
                data: "Tipo_venta",
                title: "Tipo Venta"
            },
            {
                data: "Punto_venta",
                title: "Punto Venta",
                class: "highlight"
            },
            {
                data: "Tito_fechaini",
                title: "Tito Fecha Ini",
                render: function (value) {
                    return window.moment(value).format('DD/MM/YYYY hh:mm a');
                }
            },
            {
                data: "Tito_NroTicket",
                title: "Tito Nro. Ticket",
                class: "highlight"
            },
            {
                data: "Tito_MontoTicket",
                title: "Tito Monto Ticket",
                class: "highlight"
            },
            {
                data: "Tito_MTicket_NoCobrable",
                title: "Tito Monto Ticket No Cobrable"
            },
            {
                data: "Tipo_venta_fin",
                title: "Tipo Venta Fin"
            },
            {
                data: "Punto_venta_fin",
                title: "Punto Venta Fin",
                class: "highlight"
            },
            {
                data: "Tito_fechafin",
                title: "Tito Fecha Fin",
                render: function (value) {
                    return window.moment(value).format('DD/MM/YYYY hh:mm a');
                }
            },
            {
                data: "codclie",
                title: "Cód. Cliente"
            },
            {
                data: "tipo_ticket",
                title: "Tipo Ticket"
            },
            {
                data: "Estado",
                title: "Estado",
                class: "highlight"
            },
            {
                data: "IdTipoMoneda",
                title: "Id. Tipo Moneda"
            },
            {
                data: "Motivo",
                title: "Motivo"
            },
            {
                data: "IdTipoPago",
                title: "Id. Tipo Pago"
            },
            {
                data: "Tipo_Proceso",
                title: "Tipo Proceso"
            },
            {
                data: "r_Estado",
                title: "R. Estado"
            },
            {
                data: "Tipo_Ingreso",
                title: "Tipo Ingreso"
            },
            {
                data: "Fecha_Apertura_Real",
                title: "Fecha Apertura Real",
                render: function (value) {
                    return window.moment(value).format('DD/MM/YYYY hh:mm a');
                }
            },
            {
                data: "PuntoVentaMin",
                title: "Punto Venta Min."
            },
            {
                data: "fecha_reactiva",
                title: "Fecha Reactiva",
                render: function (value) {
                    return window.moment(value).format('DD/MM/YYYY hh:mm a');
                }
            },
            {
                data: "turno",
                title: "Turno"
            },
            {
                data: "codCaja",
                title: "Cód. Caja"
            },
            {
                data: "player_tracking",
                title: "Player Tracking"
            }
        ],
        initComplete: function () {
            $(".table thead th").removeClass('highlight');
        }
    });

    ObtenerListaSalas();

    $("#cboSala").select2();

    $("#btnBuscar").click(function () {
        var url = $("#cboSala").val();
        if (!url.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        } 
        if (url === "") {
            window.toastr.error("Seleccione una Sala válida.");
            return false;
        }
        var data = {};
        var nroTicket = $.trim($("#txtNroTicket").val());

        if (nroTicket === "") {
            window.toastr.error("El campo Nro. Ticket es obligatorio.");
            return false;
        }   

        data["nroTicket"] = nroTicket;

        Buscar(url, serialize(data));

        return true;
    });
});

function Buscar(url, data) {
    //if (url.substr(-1) === '/') {
    //    url = url.substr(0, url.length - 1);
    //}
    if (!url.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url = url + "/servicio/ConsultaTicketRegistrado?" + data;
    dataAuditoria(1, "#formfiltro", 3, url + "/servicio/ConsultaTicketRegistrado", "BOTON BUSCAR");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "/CallCenter/ConsultaTicketRegistradoJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            var resp = response.data;
            var dataTable = $('#table').dataTable();
            dataTable.fnClearTable();
            if (resp.length > 0) {
                dataTable.fnAddData(resp);
            } else {
                window.toastr.warning("No se encontraron datos.");
            }
        },
        error: function (request, status, error) {
            window.toastr.error(error);
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}

function ObtenerListaSalas() {
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var data = result.data;
            $.each(data, function (index, value) {
                $("#cboSala").append('<option value="' + value.UrlProgresivo + '"  data-id="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
        },
        error: function (request, status, error) {
            window.toastr.error(error);
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}
VistaAuditoria("CallCenter/TicketRegistradoVista", "VISTA", 0, "", 3);
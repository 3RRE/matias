let estadosTicket={
    '1':'NO COBRADO',
    '2':'COBRADO',
    '3':'VENCIDO',
    'default':'ANULADO'
}
let tiposTicket={
    '1':'NORMAL',
    '2':'PAGO MANUAL',
    '3':'',
    '4':'PROMOCIONAL',//CORTESIA, aca se llamara promocional
    'default':'PROMOCIONAL'
}
$(document).ready(function () {
    const LIMITE_DIAS=2
    let fechaInicio=new Date()
    let fechaFin=new Date()
    fechaInicio = new Date(fechaInicio.setDate(fechaInicio.getDate()-LIMITE_DIAS))
    $("#fechaIni").datetimepicker({
        format: 'DD/MM/YYYY',
        defaultDate: moment(fechaFin).format('DD/MM/YYYY'),
        pickTime: false,
        minDate:fechaInicio,
        maxDate: fechaFin,
    });

    $("#fechaFin").datetimepicker({
        format: 'DD/MM/YYYY',
        defaultDate: moment(fechaFin).format('DD/MM/YYYY'),
        pickTime: false,
        // minDate:fechaInicio,
        // maxDate: fechaFin,
    });

    $(document).on('dp.change','#fechaFin',function(e){
        fechaInicio=new Date(e.date._d)
        fechaFin=new Date(e.date._d)
        fechaInicio=fechaInicio.setDate(fechaInicio.getDate()-LIMITE_DIAS)
        $('#fechaIni').data("DateTimePicker").setMinDate(new Date(fechaInicio))
        $('#fechaIni').data("DateTimePicker").setMaxDate(new Date(fechaFin))
        $('#fechaIni').data('DateTimePicker').setDate(new Date(fechaFin))
    })
    $(document).on("click", "#btnExcel", function () {
        let url = $("#cboSala").val()
        if (!url.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        } 
        let codSala=$("#cboSala option:selected").data('id')
        if (url === "") {
            window.toastr.error("Seleccione una Sala válida.")
            return false;
        }
        let dataForm = {
            fechaIni:$("#fechaIni").val(), 
            fechaFin:$("#fechaFin").val(),
            url:url+'/servicio/ConsultaTicketRegistradoPorFechas',
            codSala:codSala
        }
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "/CallCenter/ConsultaTicketRegistradosPorFechasExcel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataForm),
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
                else{
                    toastr.error(response.mensaje);
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
                title: "Tipo Ticket",
                render:function (value) {
                    return tiposTicket[value]||tiposTicket['default']
                }
            },
            {
                data: "Estado",
                title: "Estado",
                class: "highlight",
                render:function (value) {
                    return estadosTicket[value]||estadosTicket['default']
                }
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
        let url = $("#cboSala").val()
        if (!url.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        } 
        let codSala=$("#cboSala option:selected").data('id')
        if (url === "") {
            window.toastr.error("Seleccione una Sala válida.")
            return false;
        }
        let dataForm = {
            fechaIni:$("#fechaIni").val(), 
            fechaFin:$("#fechaFin").val(),
            url:url+'/servicio/ConsultaTicketRegistradoPorFechas',
            codSala:codSala
        }

        Buscar(dataForm)

        return true;
    });
});

function Buscar(data) {
 
    dataAuditoria(1, "#formfiltro", 3, data.url + "/servicio/ConsultaTicketRegistradoPorFechasJson", "BOTON BUSCAR");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "/CallCenter/ConsultaTicketRegistradoPorFechasJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            let resp = response.data;
            let dataTable = $('#table').dataTable();
            dataTable.fnClearTable();
            if (resp.length > 0) {
                dataTable.fnAddData(resp);
            } else {
                window.toastr.warning("No se encontraron datos.");
            }
        },
        error: function (request, status, error) {
            window.toastr.error(error);
            $.LoadingOverlay("hide");
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
            let data = result.data;
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
VistaAuditoria("CallCenter/TicketBolsaVista", "VISTA", 0, "", 3);
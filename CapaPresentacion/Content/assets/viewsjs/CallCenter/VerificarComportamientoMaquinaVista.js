$(document).ready(function() {
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
                data: "Cod_Cont_OL",
                title: "Cod_Cont_OL"
            },
            {
                data: "Fecha",
                title: "Fecha",
                render: function(value) {
                    return window.moment(value).format('DD/MM/YYYY');
                }
            },
            {
                data: "Hora",
                title: "Hora",
                render: function(value) {
                    return window.moment(value).format('hh:mm a');
                }
            },
            {
                data: "CodMaq",
                title: "Cód. Máquina"
            },
            {
                data: "CoinIn",
                title: "CoinIn"
            },
            {
                data: "CoinOut",
                title: "CoinOut"
            },
            {
                data: "HandPay",
                title: "HandPay"
            },
            {
                data: "CurrentCredits",
                title: "CurrentCredits"
            },
            {
                data: "Monto",
                title: "Monto",
            },
            {
                data: "EftIn",
                title: "EftIn",
            }, 
            {
                data: "CancelCredits",
                title: "CancelCredits",
                class: "highlight"
            },
            {
                data: "Jackpot",
                title: "Jackpot",
            },
            {
                data: "GamesPlayed",
                title: "GamesPlayed"
            },
            {
                data: "TrueIn",
                title: "TrueIn"
            },
            {
                data: "TrueOut",
                title: "TrueOut"
            },
            {
                data: "TotalDrop",
                title: "TotalDrop"
            }, 
            {
                data: "NroTiket",
                title: "NroTiket"
            },
            {
                data: "TicketIn",
                title: "TicketIn",
                class: "highlight"
            },
            {
                data: "TicketOut",
                title: "TicketOut"
            },
            {
                data: "TicketBonusIn",
                title: "TicketBonusIn"
            },
            {
                data: "TicketBonusOut",
                title: "TicketBonusOut"
            },
            {
                data: "MontoTiket",
                title: "MontoTiket"
            }, 
            {
                data: "codevento",
                title: "Cód. Evento",
                class: "highlight"
            }, 
            {
                data: "Token",
                title: "Token"
            },
            {
                data: "crc",
                title: "crc",
            }, 
            {
                data: "tmpebw",
                title: "tmpebw",
            }, 
            {
                data: "tapebw",
                title: "tapebw",
            }, 
            {
                data: "tappw",
                title: "tappw",
            }, 
            {
                data: "tmppw",
                title: "tmppw",
            }             
           
            //{
            //    data: "codcli",
            //    title: "Cód. Cliente"
            //},
            //{
            //    data: "CONT_OLN_REMOTO",
            //    title:"CONT_OLN_REMOTO"
            //},           
        ],
        initComplete: function () {
            $(".table thead th").removeClass('highlight');
        }
    });

    ObtenerListaSalas();

    $("#cboSala").select2();

    $("#btnBuscar").click(function() {
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
        var maquina = $.trim($("#txtMaquina").val());

        if (maquina === "") {
            window.toastr.error("El campo Máquina es obligatorio.");
            return false;
        }
        data["maquina"] = maquina;
        var cantidad = $.trim($("#txtCantidad").val());
        if (cantidad !== "") {
            cantidad = parseInt(cantidad, 10);
            if (isNaN(cantidad)) {
                window.toastr.error("El valor del campo Cantidad no es válido.");
                return false;
            }
            data["cantidad"] = cantidad;
        }

        var fechaInicio = $.trim($("#txtFechaInicio").val());
        var fechaFin = $.trim($("#txtFechaFin").val());

        if (fechaInicio !== "" && fechaFin === "") {
            window.toastr.error("El campo Fecha Fin es obligatorio.");
            return false;
        }

        if (fechaFin !== "" && fechaInicio === "") {
            window.toastr.error("El campo Fecha Inicio es obligatorio.");
            return false;
        }

        if (fechaInicio !== "") {
            fechaInicio = window.moment(fechaInicio, "DD/MM/YYYY hh:mm a");
            if (!fechaInicio.isValid()) {
                window.toastr.error("El valor del campo Fecha Inicio no es válido.");
                return false;
            }
            fechaInicio = fechaInicio.format("YYYY-MM-DD HH:mm:ss");
            data["fechaInicio"] = fechaInicio;
         
        }

        if (fechaFin !== "") {
            fechaFin = window.moment(fechaFin, "DD/MM/YYYY hh:mm a");
            if (!fechaFin.isValid()) {
                window.toastr.error("El valor del campo Fecha Fin no es válido.");
                return false;
            }

            fechaFin = fechaFin.format("YYYY-MM-DD HH:mm:ss");

            data["fechaFin"] = fechaFin;
        }

        Buscar(url, serialize(data));
       
        return true;
    });
});

function Buscar(url, data) {
    //if (url.substr(-1) === '/') {
    //    url = url.substr(0, url.length - 1) + data;
    //}
    if (!url.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url = url + "/servicio/ConsultaVerificarComportamientoMaquina?" + data;
    dataAuditoria(1, "#formfiltro", 3, url + "/servicio/ConsultaVerificarComportamientoMaquina", "BOTON BUSCAR");

    

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "/CallCenter/ConsultaVerificarComportamientoMaquinaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            debugger
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
VistaAuditoria("CallCenter/VerificarComportamientoMaquinaVista", "VISTA", 0, "", 3);
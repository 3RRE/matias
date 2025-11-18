const CANTIDAD_DIAS = 10;
const DATE_FORMAT = 'DD/MM/YYYY HH:mm:ss';
const btnBuscar = $("#btnBuscar");
const btnGenerarExcel = $("#btnGenerarExcel");
const cboSala = $("#cboSala");
const btnSincronizarContadores = $("#btnSincronizarContadores");

const generarExcel = (codSala, fechaIni, fechaFin) => {
    const url = basePath + "Sunat/GenerarExcelContadoresSunat";
    const data = {
        codSala,
        fechaIni,
        fechaFin
    };

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            let displayMessage = response.displayMessage;
            if (!response.success) {
                toastr.error(displayMessage);
                return;
            }
            toastr.success(displayMessage);
            let dataExcel = response.bytes
            let linkExcel = document.createElement('a')
            document.body.appendChild(linkExcel) //required in FF, optional for Chrome
            linkExcel.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + dataExcel
            linkExcel.download = `${response.fileInfo.FileName}.xlsx`
            linkExcel.click()
            linkExcel.remove()
        }
    });
}

const buscarContadores = (codSala, fechaIni, fechaFin) => {
    const url = basePath + "Sunat/ObtenerContadoresSunat";
    const data = {
        codSala,
        fechaIni,
        fechaFin
    };

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
            if (!response.success) {
                toastr.error(response.displayMessage, "Aviso");
                return;
            }
            createDataTable(response.data);
            toastr.success(response.displayMessage, "Resultado");
        }
    });
}

const sincronizarContadores = (salaId, diaContadorSunat) => {
    const url = basePath + "Sunat/SincronizarContadoresSunat";
    const data = {
        salaId,
        diaContadorSunat
    };

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
            if (!response.success) {
                toastr.error(response.displayMessage, "Aviso");
                return;
            }
            createDataTable(response.data);
            toastr.success(response.displayMessage, "Resultado");
        }
    });
}

const createDataTable = (data) => {
    objetodatatable = $("#tableContadores").DataTable({
        bDestroy: true,
        bSort: true,
        scrollCollapse: true,
        scrollX: true,
        paging: true,
        autoWidth: true,
        bProcessing: true,
        bDeferRender: true,
        data: data,
        columns: [
            { data: "CodSala", title: "Cod Sala" },
            { data: "Sala", title: "Sala" },
            {
                data: "FechaMigracion", title: "Fecha Migracion",
                render: function (value) {
                    return moment(value).format(DATE_FORMAT);
                }
            },
            {
                data: "Fecha", title: "Fecha",
                render: function (value) {
                    return moment(value).format(DATE_FORMAT);
                }
            },
            { data: "Trama", title: "Trama" },
            {
                data: "Cereo", title: "Cereo",
                render: function (value) {
                    return value ? 1 : 0;
                }
            },
            { data: "IdConSunat", title: "Id Con Sunat" },
            { data: "Envio", title: "Envio" },
            {
                data: "FechaEnvio", title: "Fecha Envio",
                render: function (value) {
                    return moment(value).format(DATE_FORMAT);
                }
            },
            { data: "IdCereo", title: "Id Cereo" },
            {
                data: "FechaProceso", title: "Fecha Proceso",
                render: function (value) {
                    return moment(value).format(DATE_FORMAT);
                }
            },
            { data: "Motivo", title: "Motivo" },
            { data: "IdConfSunat", title: "Id Conf Sunat" },
            { data: "BandBusq", title: "Band Busq" },
            { data: "Cabecera", title: "Cabecera" },
            { data: "DGJM", title: "DGJM" },
            { data: "CodMaq", title: "Cod Maq" },
            { data: "FechaTrama", title: "Fecha Trama" },
            { data: "Reserva1", title: "Reserva 1" },
            { data: "Moneda", title: "Moneda" },
            { data: "Denominacion", title: "Denominacion" },
            { data: "CoinInFinal", title: "Coin In Final" },
            { data: "CoinOutFinal", title: "Coin Out Final" },
            { data: "PagoManualFinal", title: "Pago Manual Final" },
            { data: "OtroFinal", title: "Otro Final" },
            { data: "TipoCambio", title: "Tipo Cambio" },
        ],
    });
}

btnBuscar.on('click', () => {
    const codSala = cboSala.val();
    if (!codSala) {
        toastr.warning("Seleccione por los menos una sala", "Aviso")
    }
    var fechaIni = $("#fechaIniContadores").val();
    var fechaFin = $("#fechaFinContadores").val();

    buscarContadores(codSala, fechaIni, fechaFin);

});

btnGenerarExcel.on('click', () => {
    const codSala = cboSala.val();
    if (!codSala) {
        toastr.warning("Seleccione por los menos una sala", "Aviso")
    }
    var fechaIni = $("#fechaIniContadores").val();
    var fechaFin = $("#fechaFinContadores").val();

    generarExcel(codSala, fechaIni, fechaFin)
});

btnSincronizarContadores.on('click', () => {
    const codSala = cboSala.val();
    if (!codSala) {
        toastr.warning("Seleccione por los menos una sala", "Aviso")
    }
    var dia = $("#fechaInicioContadores").val();

    sincronizarContadores(codSala, dia)
});

$(document).ready(function () {
    createDataTable([]);
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
    $.when(
        llenarSelect(basePath + "Sala/ListadoSalaActivasSinSeguridad", {}, "cboSala", "CodSala", "Nombre")
    ).then(() => {
        cboSala.select2({
            placeholder: "Seleccione ...",
            multiple: false,
        });
    });


});

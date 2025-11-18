const DATE_FORMAT = 'DD/MM/YYYY HH:mm:ss';
const btnBuscar = $("#btnBuscar");
const btnGenerarExcel = $("#btnGenerarExcel");
const cboSala = $("#cboSala");

const generarExcel = (codSala, fechaInicio, fechaFin) => {
    const url = basePath + "EventosSignificativos/GenerarExcelEventosSignificativos";
    const data = {
        codSala,
        fechaInicio,
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

const listarEventosSignificativos = (codSala, fechaInicio, fechaFin) => {
    const url = basePath + "EventosSignificativos/ObtenerEventosSignificativos";
    const data = {
        codSala,
        fechaInicio,
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

const createDataTable = (data) => {
    objetodatatable = $("#tablaEventosSignificativos").DataTable({
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
            { data: "IdEventoSignificativo", title: "Id Evento Significativo" },
            { data: "Cod_Even_OL", title: "Código evento Ol" },
            {
                data: "Fecha", title: "Fecha",
                render: function (value) {
                    return moment(value).format(DATE_FORMAT);
                }
            },
            {
                data: "Hora", title: "Hora",
                render: function (value) {
                    return moment(value).format(DATE_FORMAT);
                }
            },
            { data: "CodTarjeta", title: "Código Tarjeta" },
            { data: "CodMaquina", title: "Código Máquina" },
            { data: "Cod_Evento", title: "Código Evento" },
            { data: "NombreEvento", title: "Nombre del evento" },
        ],
    });
}

btnBuscar.on('click', () => {
    const codSala = cboSala.val();
    if (!codSala) {
        toastr.warning("Seleccione por los menos una sala", "Aviso")
    }
    var fechaInicio = $("#fechaInicioEventosSignificativos").val();
    var fechaFin = $("#fechaFinEventosSignificativos").val();

    listarEventosSignificativos(codSala, fechaInicio, fechaFin);
});

btnGenerarExcel.on('click', () => {
    const codSala = cboSala.val();
    if (!codSala) {
        toastr.warning("Seleccione por los menos una sala", "Aviso")
    }
    var fechaInicio = $("#fechaInicioEventosSignificativos").val();
    var fechaFin = $("#fechaFinEventosSignificativos").val();

    generarExcel(codSala, fechaInicio, fechaFin)
});

//btnSincronizarContadores.on('click', () => {
//    const codSala = cboSala.val();
//    if (!codSala) {
//        toastr.warning("Seleccione por los menos una sala", "Aviso")
//    }
//    var dia = $("#fechaInicioContadores").val();

//    sincronizarContadores(codSala, dia)
//});

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

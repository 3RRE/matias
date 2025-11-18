// arguments
var maxDaysQuery = 31

var RepoLRNominal = {
    dates: [],
    data: []
}

var ChartLRNominalRoom = {
    title: "Logs Online",
    room: {},
    series: [],
    categories: [],
    colors: [],
    types: [],
    maxYaxis: 0,
    setRoom: function (room) {
        this.room = room
        this.title = `Logs Online ${room.name}`
    },
    buildSeries: function () {
        var repoSO = RepoLRNominal.data.find(x => x.Tipo == 1)
        var repoEV = RepoLRNominal.data.find(x => x.Tipo == 2)
        var repoAB = RepoLRNominal.data.find(x => x.Tipo == 3)

        var rnlso = repoSO ? repoSO.Salas.find(x => x.SalaId == this.room.id).Logs.map(x => x.Total) : []
        var rnlev = repoEV ? repoEV.Salas.find(x => x.SalaId == this.room.id).Logs.map(x => x.Total) : []
        var rnlab = repoAB ? repoAB.Salas.find(x => x.SalaId == this.room.id).Logs.map(x => x.Total) : []
        var rnlmerge = [...rnlso, ...rnlev, ...rnlab]

        var series = []
        var colors = []
        var types = []

        var nameSO = 'Servicio Online'
        var nameEV = 'Eventos'
        var nameAB = 'Alerta Billeteros'

        if (rnlso.length > 0) {
            series.push({
                name: nameSO,
                data: rnlso
            })

            types.push({
                key: 1,
                name: nameSO
            })

            colors.push('#D9534F')
        }

        if (rnlev.length > 0) {
            series.push({
                name: nameEV,
                data: rnlev
            })

            types.push({
                key: 2,
                name: nameEV
            })

            colors.push('#5CB85C')
        }

        if (rnlab.length > 0) {
            series.push({
                name: nameAB,
                data: rnlab
            })

            types.push({
                key: 3,
                name: nameAB
            })

            colors.push('#F0AD4E')
        }

        this.series = series
        this.colors = colors
        this.types = types
        this.maxYaxis = Math.max(...rnlmerge)
    },
    buildCategories: function () {
        this.categories = RepoLRNominal.dates
    },
    buildChart: function () {
        this.buildSeries()
        this.buildCategories()
    },
    renderChart: function (selectorChart) {
        this.buildChart()

        var options = {
            series: this.series,
            chart: {
                height: 600,
                type: 'line',
                toolbar: {
                    show: true,
                    tools: {
                        zoom: false,
                        zoomin: false,
                        zoomout: false,
                        pan: false,
                        reset: false
                    }
                }
            },
            colors: this.colors,
            dataLabels: {
                enabled: true
            },
            stroke: {
                curve: 'smooth'
            },
            title: {
                text: this.title,
                align: 'center',
                margin: 30
            },
            grid: {
                borderColor: '#e7e7e7',
                row: {
                    colors: ['#f3f3f3', 'transparent'],
                    opacity: 0.5
                }
            },
            markers: {
                size: 1
            },
            xaxis: {
                categories: this.categories,
                title: {
                    text: 'Fechas'
                }
            },
            yaxis: {
                title: {
                    text: 'Total'
                },
                min: 0,
                max: this.maxYaxis
            },
            legend: {
                show: true,
                showForSingleSeries: true,
                position: 'top',
                horizontalAlign: 'center',
                floating: true,
                offsetY: -20,
                offsetX: 0
            }
        }

        var chart = new ApexCharts(selectorChart, options)

        chart.render()

        this.types.forEach(type => {
            chart.hideSeries(type.name)

            if (type.key == this.room.report) {
                chart.showSeries(type.name)
            }
        })
    }
}

// obtener salas asignadas
function obtenerSalasAsignadas() {
    return $.ajax({
        type: "POST",
        url: `${basePath}/Sala/ListadoSalaPorUsuarioJson`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            return response
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

// render salas asignadas
function renderSalasAsignadas(data) {
    var element = "#cboSalas"

    data.forEach(item => {
        $(element).append(`<option value="${item.CodSala}">${item.Nombre}</option>`)
    })

    jQuery(element).multiselect({
        multiple: true,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        maxHeight: 400,
        buttonContainer: '<div></div>',
        buttonClass: '',
        templates: {
            button: '<div class="form-control form-multiselect input-sm multiselect" data-toggle="dropdown"><span class="multiselect-selected-text"></span></div>'
        },
        nonSelectedText: '--Seleccione--',
        nSelectedText: 'seleccionados',
        allSelectedText: 'Todo seleccionado',
        selectAllText: 'Seleccionar todos'
    })
}

// render log types
function renderLogsTypes() {
    var element = "#cboTipos"

    jQuery(element).multiselect({
        multiple: true,
        enableFiltering: false,
        enableCaseInsensitiveFiltering: false,
        includeSelectAllOption: true,
        maxHeight: 400,
        buttonContainer: '<div></div>',
        buttonClass: '',
        templates: {
            button: '<div class="form-control form-multiselect input-sm multiselect" data-toggle="dropdown"><span class="multiselect-selected-text"></span></div>'
        },
        nonSelectedText: '--Seleccione--',
        nSelectedText: 'seleccionados',
        allSelectedText: 'Todo seleccionado',
        selectAllText: 'Seleccionar todos'
    })
}

// listar logs reporte nominal
function listarLogsReporteNominal(params) {
    var url = `${basePath}/LogAlertaBilleteros/ObtenerLogsReporteNominal`
    var data = JSON.stringify(params)

    renderTipoReporteNominal([], [])
    showReportNominalWrap(false)

    ajaxhr = $.ajax({
        type: "POST",
        url: url,
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")


        },
        success: function (response) {
            if (response.success) {
                renderTipoReporteNominal(response.dates, response.data)

                RepoLRNominal.dates = response.dates
                RepoLRNominal.data = response.data
            }
            AbortRequest.close()

        },
        complete: function () {
            $.LoadingOverlay("hide")
            AbortRequest.close()
            showReportNominalWrap(true)
        }
    })

    AbortRequest.open()

    return ajaxhr

}

// render logs tipo reporte nominal
function renderTipoReporteNominal(dates, data) {

    var report_nominal_wrap = $('#report_nominal_wrap')
    var navTabsItems = ''
    var contentItems = ''

    var tableReportType = {
        1: {
            tab_id: 'tab_so',
            table_wrap: 'table_so_nominal_wrap'
        },
        2: {
            tab_id: 'tab_ev',
            table_wrap: 'table_ev_nominal_wrap'
        },
        3: {
            tab_id: 'tab_ab',
            table_wrap: 'table_ab_nominal_wrap'
        }
    }

    data.forEach((item, index) => {
        var tabActive = index == 0 ? 'active': ''
        var tableReport = tableReportType[item.Tipo] ? tableReportType[item.Tipo] : ''
        var logReport = renderLogsReporteNominal(dates, item.Salas)

        navTabsItems += `
        <li role="presentation" class="${tabActive}">
            <a href="#${tableReport.tab_id}" aria-controls="${tableReport.tab_id}" role="tab" data-toggle="tab">${item.Nombre}</a>
        </li>
        `

        contentItems += `
        <div role="tabpanel" class="tab-pane ${tabActive}" id="${tableReport.tab_id}">
            <div id="${tableReport.table_wrap}">
                ${logReport}
            </div>
        </div>
        `
    })

    var navTabs = `
    <ul class="nav nav-tabs" style="margin:0" role="tablist">
        ${navTabsItems}
    </ul>
    <div class="tab-content">
        ${contentItems}
    </div>
    `

    report_nominal_wrap.html(navTabs)
}

// render logs reporte nominal
function renderLogsReporteNominal(dates, items) {

    var thDates = ''
    var trRooms = ''

    dates.forEach(date => {
        thDates += `
        <th>${date}</th>
        `
    })

    items.forEach(item => {

        var tdLogs = ''

        item.Logs.forEach(log => {
            if (log.Total == 0) {
                tdLogs += `
                <td class="log-off">${log.Total}</td>
                `
            } else {
                tdLogs += `
                <td class="log-on">
                    <span class="log-action button_logs" data-room="${item.SalaId}" data-type="${item.Tipo}" data-date="${log.Fecha}">${log.Total}</span>
                </td>
                `
            }
        })

        trRooms += `
        <tr class="tr-td">
            <td class="text-left">
                <div class="room-dflex">
                    <span class="log-room">${item.Sala}</span>
                    <span class="log-chart-action button_chart_logs" data-room="${item.SalaId}" data-type="${item.Tipo}">
                        <i class="fa fa-line-chart"></i>
                    </span>
                </div>
            </td>
            ${tdLogs}
        </tr>
        `
    })

    var tableNominal = `
    <div class="table-responsive">
        <table id="tableNominal" class="table-nr disable-select">
            <thead>
                <tr class="tr-th">
                    <th>Salas</th>
                    ${thDates}
                </tr>
            </thead>
            <tbody>
                ${trRooms}
            </tbody>
        </table>
    </div>
    `

    return tableNominal
}

$(document).on("click", "#tableNominal tbody tr .text-left", function () {
    $(this).parent().toggleClass('highlighted-row');
});

// listar room logs
function listarRoomLogs(params) {
    var url = `${basePath}/LogAlertaBilleteros/ObtenerSalaLogsFecha`
    var data = JSON.stringify(params)

    renderRoomLogs([])

    $.ajax({
        type: "POST",
        url: url,
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.success) {
                renderRoomLogs(response.data)
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")

            $('#modal_room_logs').modal('show')
        }
    })
}

// render room logs
function renderRoomLogs(logs) {
    $("#tableRoomLogs").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": false,
        "paging": true,
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": false,
        "aaSorting": [],
        data: logs,
        columns: [
            {
                data: "Id", title: "ID"
            },
            {
                data: "NombreSala",
                title: "Sala"
            },
            {
                data: "FechaRegistro",
                title: "Fecha Reg.",
                render: function (value)
                {
                    return `${moment(value).format('DD/MM/YYYY hh:mm:ss A')}`
                }
            },
            {
                data: "Tipo",
                title: "Tipo",
                render: function (value) {
                    var labelTypes = {
                        1: '<span class="label label-danger">Servicio Online</span>',
                        2: '<span class="label label-success">Eventos</span>',
                        3: '<span class="label label-warning">Alerta Billeteros</span>'
                    }

                    return labelTypes[value] ? labelTypes[value] : ''
                }
            },
            {
                data: null,
                title: "Cod Máquina",
                render: function (item) {
                    var codMaq = ''

                    if (item.Tipo == 2 || item.Tipo == 3) {
                        var json = JSON.parse(item.Descripcion)

                        codMaq = json.CodMaquina
                    }

                    return codMaq
                }
            },
            {
                data: "Preview",
                title: "Información",
                render: function (value) {
                    return `${value.substr(0, 34)} ...`
                }
            },
            {
                data: null,
                title: "Acción",
                render: function (item) {
                    var buttons = `<button type="button" class="btn btn-xs btn-primary button_detalle" title="Detalle" data-id="${item.Id}">Detalle</button>`

                    return buttons
                },
                searchable: false,
                orderable: false
            }
        ],
        columnDefs: [
            {
                targets: "_all",
                className: "text-center"
            }
        ]
    })
}

// obtener detalle logs
function obtenerDetalleLogs(logId) {
    var url = `${basePath}/LogAlertaBilleteros/GetAlertaBilleteroxId`
    var data = JSON.stringify({ Id: logId })

    clearDetailLogAB()

    $.ajax({
        type: "POST",
        url: url,
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.respuesta) {
                renderDetalleLogs(response.data)
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")

            $('#modal_detail_logs').modal('show')
        }
    })
}

// render detalle logs
function renderDetalleLogs(item) {
    var wrapElementSO = $('#wrapElementSO')
    var wrapElementEV = $('#wrapElementEV')
    var wrapElementAB = $('#wrapElementAB')
    var log_fecha_registro = $('#log_fecha_registro')
    var log_tipo = $('#log_tipo')
    var logTypes = {
        1: 'Servicio Online',
        2: 'Eventos',
        3: 'Alerta Billeteros'
    }

    var logDate = moment(item.FechaRegistro).format('DD/MM/YYYY hh:mm:ss A')
    var nameType = logTypes[item.Tipo] ? logTypes[item.Tipo] : ''
    var jsonParse = JSON.parse(item.Descripcion)
    item.Jsonp = jsonParse

    wrapElementSO.hide()
    wrapElementEV.hide()
    wrapElementAB.hide()

    log_fecha_registro.val(logDate)
    log_tipo.val(nameType)

    if (item.Tipo == 1) {
        renderDetailLogSO(item)
        wrapElementSO.show()
    }

    if (item.Tipo == 2) {
        renderDetailLogEV(item)
        wrapElementEV.show()
    }

    if (item.Tipo == 3) {
        renderDetailLogAB(item)
        wrapElementAB.show()
    }
}

function renderDetailLogSO(item) {
    var so_sala = $('#so_sala')
    var so_descripcion = $('#so_descripcion')
    var so_info_adicional = $('#so_info_adicional')

    so_sala.val(item.NombreSala)
    so_descripcion.val(item.Jsonp.Descripcion)
    so_info_adicional.val(item.Jsonp.InfoAdicional)
}

function renderDetailLogEV(item) {
    var ev_cod_even_ol = $('#ev_cod_even_ol')
    var ev_sala = $('#ev_sala')
    var ev_fecha_online = $('#ev_fecha_online')
    var ev_codigo_tarjeta = $('#ev_codigo_tarjeta')
    var ev_codigo_maquina = $('#ev_codigo_maquina')
    var ev_evento = $('#ev_evento')
    var ev_table_devices = $('#ev_table_devices')

    ev_cod_even_ol.val(item.Jsonp.Cod_Even_OL)
    ev_sala.val(item.NombreSala)
    ev_fecha_online.val(moment(item.Jsonp.Fecha).format('DD/MM/YYYY hh:mm:ss A'))
    ev_codigo_tarjeta.val(item.Jsonp.CodTarjeta)
    ev_codigo_maquina.val(item.Jsonp.CodMaquina)
    ev_evento.val(item.Jsonp.Evento)

    var listDetail = item.Jsonp.ListaEventoDispositivo

    var trDetail = ''

    listDetail.forEach(detail => {
        trDetail += `
        <tr>
            <td class="text-center">${detail.EventoId}</td>
            <td class="text-center">${moment(detail.FechaRegistro).format('DD/MM/YYYY hh:mm:ss A')}</td>
            <td class="text-center">${detail.DispositivoNombre}</td>
            <td class="text-center">${detail.Usuario ? detail.Usuario : ''}</td>
        </tr>
        `
    })

    ev_table_devices.find('tbody').html(trDetail)
}

function renderDetailLogAB(item) {
    var ab_alerta_id = $('#ab_alerta_id')
    var ab_sala = $('#ab_sala')
    var ab_fecha = $('#ab_fecha')
    var ab_codigo_maquina = $('#ab_codigo_maquina')
    var ab_descripcion_alerta = $('#ab_descripcion_alerta')
    var ab_bill_billetero = $('#ab_bill_billetero')
    var ab_bill_parcial = $('#ab_bill_parcial')
    var ab_table_devices = $('#ab_table_devices')

    ab_alerta_id.val(item.Jsonp.AlertaID)
    ab_sala.val(item.NombreSala)
    ab_fecha.val(item.Jsonp.fecha_registro)
    ab_codigo_maquina.val(item.Jsonp.CodMaquina)
    ab_descripcion_alerta.val(item.Jsonp.descripcion_alerta)
    ab_bill_billetero.val(item.Jsonp.contador_bill_billetero.toFixed(2))
    ab_bill_parcial.val(item.Jsonp.contador_bill_parcial.toFixed(2))

    var listDetail = item.Jsonp.ListaAlertaDispositivo

    var trDetail = ''

    listDetail.forEach(detail => {
        trDetail += `
        <tr>
            <td class="text-center">${detail.AlertaId}</td>
            <td class="text-center">${moment(detail.FechaRegistro).format('DD/MM/YYYY hh:mm:ss A')}</td>
            <td class="text-center">${detail.DispositivoNombre}</td>
            <td class="text-center">${detail.Usuario ? detail.Usuario : ''}</td>
        </tr>
        `
    })

    ab_table_devices.find('tbody').html(trDetail)
}

function clearDetailLogAB() {
    // registro
    var log_fecha_registro = $('#log_fecha_registro')
    var log_tipo = $('#log_tipo')

    // servicio online
    var so_sala = $('#so_sala')
    var so_descripcion = $('#so_descripcion')
    var so_info_adicional = $('#so_info_adicional')

    // evento
    var ev_cod_even_ol = $('#ev_cod_even_ol')
    var ev_sala = $('#ev_sala')
    var ev_fecha_online = $('#ev_fecha_online')
    var ev_codigo_tarjeta = $('#ev_codigo_tarjeta')
    var ev_codigo_maquina = $('#ev_codigo_maquina')
    var ev_evento = $('#ev_evento')
    var ev_table_devices = $('#ev_table_devices')

    // alerta billetero
    var ab_alerta_id = $('#ab_alerta_id')
    var ab_sala = $('#ab_sala')
    var ab_fecha = $('#ab_fecha')
    var ab_codigo_maquina = $('#ab_codigo_maquina')
    var ab_descripcion_alerta = $('#ab_descripcion_alerta')
    var ab_bill_billetero = $('#ab_bill_billetero')
    var ab_bill_parcial = $('#ab_bill_parcial')
    var ab_table_devices = $('#ab_table_devices')

    // registro
    log_fecha_registro.val('')
    log_tipo.val('')

    // servicio online
    so_sala.val('')
    so_descripcion.val('')
    so_info_adicional.val('')

    // evento
    ev_cod_even_ol.val('')
    ev_sala.val('')
    ev_fecha_online.val('')
    ev_codigo_tarjeta.val('')
    ev_codigo_maquina.val('')
    ev_evento.val('')
    ev_table_devices.find('tbody').html('')

    // alerta billetero
    ab_alerta_id.val('')
    ab_sala.val('')
    ab_fecha.val('')
    ab_codigo_maquina.val('')
    ab_descripcion_alerta.val('')
    ab_bill_billetero.val('')
    ab_bill_parcial.val('')
    ab_table_devices.find('tbody').html('')
}

// show report nominal
function showReportNominalWrap(status) {
    var elementTable = $('#report_nominal_wrap')

    if (status) {
        elementTable.show()
    }
    else {
        elementTable.hide()
    }
}

// excel logs reporte nominal
function excelLogsReporteNominal(params) {
    var url = `${basePath}/LogAlertaBilleteros/ExcelLogsReporteNominal`
    var data = JSON.stringify(params)

    $.ajax({
        type: "POST",
        url: url,
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.success) {
                var data = response.data
                var file = response.fileName
                var a = document.createElement('a')
                a.target = '_self'
                a.href = `data:application/vnd.ms-excel;base64, ${data}`
                a.download = file
                a.click()

                toastr.success(response.message)
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

// document ready
$(document).ready(function () {

    // show table nominal
    showReportNominalWrap(false)

    // obtener salas asignadas
    obtenerSalasAsignadas().done(response => {
        if (response.data) {
            renderSalasAsignadas(response.data)
        }
    })

    // render logs types
    renderLogsTypes()

    // datetimepicker
    $(".datepkr_only").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow
    })

    // on change from date
    $(document).on('dp.change', '#fromDate', function (event) {
        var date = new Date(event.date)

        $('#toDate').data("DateTimePicker").setMinDate(date)
    })

    // on change to date
    $(document).on('dp.change', '#toDate', function (event) {
        var date = new Date(event.date)

        $('#fromDate').data("DateTimePicker").setMaxDate(date)
    })

    // on change room
    $(document).on('change', '#cboSalas', function (event) {
        event.preventDefault()

        renderTipoReporteNominal([], [])
        showReportNominalWrap(false)
    })

    // on click search
    $(document).on('click', '#button_search', function (event) {
        event.preventDefault()

        var rooms = $('#cboSalas').val()
        var types = $('#cboTipos').val()
        var fromDate = $('#fromDate').val()
        var toDate = $('#toDate').val()

        toastr.remove()

        if (!rooms) {
            toastr.warning("Seleccione una o más salas")

            return false
        }

        if (!types) {
            toastr.warning("Seleccione uno o más tipos")

            return false
        }

        if (!fromDate) {
            toastr.warning("Seleccione una fecha inicio")

            return false
        }

        if (!toDate) {
            toastr.warning("Seleccione una fecha final")

            return false
        }

        if (gapTwoDates(fromDate, toDate) >= maxDaysQuery) {
            toastr.warning(`Por favor, seleccione fechas de ${maxDaysQuery} días de diferencia`)

            return false
        }

        var args = {
            rooms,
            types,
            fromDate,
            toDate
        }

        listarLogsReporteNominal(args)
    })

    // on click log action
    $(document).on('click', '.button_logs', function (event) {
        event.preventDefault()

        var room = $(this).attr('data-room')
        var type = $(this).attr('data-type')
        var date = $(this).attr('data-date')

        toastr.remove()

        if (!room) {
            toastr.warning("Seleccione una sala")

            return false
        }

        if (!type) {
            toastr.warning("Seleccione un tipo")

            return false
        }

        if (!date) {
            toastr.warning("Seleccione una fecha")

            return false
        }

        var args = {
            room,
            type,
            date
        }

        listarRoomLogs(args)
    })

    // on click detalle
    $(document).on('click', '.button_detalle', function (event) {
        event.preventDefault()

        var logId = $(this).attr('data-id')

        toastr.remove()

        if (!logId) {
            toastr.warning("Seleccione un registro")

            return false
        }

        obtenerDetalleLogs(logId)
    })

    // on click excel
    $(document).on('click', '#button_excel', function (event) {
        event.preventDefault()

        var rooms = $('#cboSalas').val()
        var types = $('#cboTipos').val()
        var fromDate = $('#fromDate').val()
        var toDate = $('#toDate').val()

        toastr.remove()

        if (!rooms) {
            toastr.warning("Seleccione una o más salas")

            return false
        }

        if (!types) {
            toastr.warning("Seleccione uno o más tipos")

            return false
        }

        if (!fromDate) {
            toastr.warning("Seleccione una fecha inicio")

            return false
        }

        if (!toDate) {
            toastr.warning("Seleccione una fecha final")

            return false
        }

        if (gapTwoDates(fromDate, toDate) >= maxDaysQuery) {
            toastr.warning(`Por favor, seleccione fechas de ${maxDaysQuery} días de diferencia`)

            return false
        }

        var args = {
            rooms,
            types,
            fromDate,
            toDate
        }

        excelLogsReporteNominal(args)
    })

    // on click chart logs
    $(document).on('click', '.button_chart_logs', function (event) {
        event.preventDefault()

        var room = $(this).attr('data-room')
        var type = $(this).attr('data-type')

        toastr.remove()

        if (!room) {
            toastr.warning("Seleccione una sala")

            return false
        }

        if (!type) {
            toastr.warning("Seleccione un tipo")

            return false
        }

        var repoRoom = RepoLRNominal.data.find(x => x.Tipo == type).Salas.find(x => x.SalaId == room)

        if (!repoRoom) {
            toastr.warning("No se encontraron datos")

            return false
        }

        $('#chart_wrap').html('<div id="chart"></div>')

        var chartRoom = {
            id: repoRoom.SalaId,
            name: repoRoom.Sala,
            report: repoRoom.Tipo
        }

        ChartLRNominalRoom.setRoom(chartRoom)

        $('#modal_room_chart_logs').modal('show')
        $.LoadingOverlay("show")
    })

    $("#modal_room_chart_logs").on('shown.bs.modal', function () {
        ChartLRNominalRoom.renderChart(document.querySelector("#chart"))
        $.LoadingOverlay("hide")
    })
})
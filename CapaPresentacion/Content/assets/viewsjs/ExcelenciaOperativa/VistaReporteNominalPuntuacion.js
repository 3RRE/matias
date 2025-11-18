var ListaVersion = []
var ListaReporte = []
var VersionReporte = 1

// Obtener Salas Asignadas
function getAssignedRooms() {

    var element = $("#room")

    $.ajax({
        type: "POST",
        url: `${basePath}Sala/ListadoSalaPorUsuarioJson`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
            element.html(`<option value="">--Seleccione--</option>`)
        },
        success: function (response) {
            var items = response.data

            $.each(items, function (index, item) {
                element.append(`<option value="${item.CodSala}">${item.Nombre}</option>`)
            })
        },
        error: function () {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

function getFichaJson(type) {
    var url = ""
    var fileHash = ((+new Date) + Math.random() * 100).toString(32)

    if (type == 1) {
        url = `${basePath}/Content/data/EOJefeOperativo.json?v=${fileHash}`

        if (VersionReporte == 3) {
            url = `${basePath}/Content/data/EOJefeOperativoV3.json?v=${fileHash}`
        }
        if (VersionReporte == 4) {
            url = `${basePath}/Content/data/EOJefeOperativoV4.json?v=${fileHash}`
        }
        if (VersionReporte == 5) {
            url = `${basePath}/Content/data/EOJefeOperativoV5.json?v=${fileHash}`
        }
    }
    else if (type == 2) {
        url = `${basePath}/Content/data/EOGerenteUnidad.json?v=${fileHash}`
    }

    if (!url) {
        toastr.warning("Ingrese Url de la ficha")

        return false
    }

    return $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

function getNominalReport(params) {
    var data = {
        roomCode: params.roomCode,
        typeCode: params.typeCode,
        startDate: params.startDate,
        endDate: params.endDate
    }

    var type = params.typeCode

    VersionReporte = 1
    ListaVersion = []
    tableNominalReport(type, [], [])

    $.ajax({
        type: "POST",
        url: `${basePath}ExcelenciaOperativa/ListarReporteNominalPuntuacion`,
        data: JSON.stringify({ ...data }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) { 
            if (response.status) {
                var dataV1 = response.data.filter(x => x.FichaVersion == 1)
                var dataV3 = response.data.filter(x => x.FichaVersion == 3)
                var dataV4 = response.data.filter(x => x.FichaVersion == 4)
                var dataV5 = response.data.filter(x => x.FichaVersion == 5)

                ListaReporte = response.data

               
                if (dataV5.length > 0) {
                    ListaVersion.push({ key: 5, name: 'Version 5' })
                }
                if (dataV4.length > 0) {
                    ListaVersion.push({ key: 4, name: 'Version 4' })
                }
                if (dataV3.length > 0) {
                    ListaVersion.push({ key: 3, name: 'Version 3' })
                }
                if (dataV1.length > 0) {
                    ListaVersion.push({ key: 1, name: 'Version 1' })
                }

                if (ListaVersion.length > 0) {
                    VersionReporte = ListaVersion[0].key
                }

                var data = response.data.filter(x => x.FichaVersion == VersionReporte)

                getFichaJson(type).done(function (items) {
                    tableNominalReport(type, items, data)
                })
            }
            else {
                toastr.warning(response.message, "Mensaje Servidor")
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

function renderTabsVersion() {
    var wrapFichaVersion = $('#wrapFichaVersion')
    var tabsContent = ''
    var liVersions = ''

    ListaVersion.forEach(version => {
        liVersions += `
        <li class="${version.key == VersionReporte ? 'active' : ''}">
            <button type="button" class="btn-cutab change--version" data-version="${version.key}">${version.name}</button>
        </li>
        `
    })

    if (ListaVersion.length > 1) {
        tabsContent = `
        <ul class="nav nav-tabs" style="margin: 0; border: 0">
            ${liVersions}
        </ul>
        `
    }

    wrapFichaVersion.html(tabsContent)
}

function tableNominalReport(type, items, data)
{
    renderTabsVersion()

    var theadWrap = $("#theadWrap")
    var tbodyWrap = $("#tbodyWrap")
    var tfootWrap = $("#tfootWrap")

    var typesOE = {
        1: 'Jefe Operativo',
        2: 'Gerente de Unidad'
    }

    var typeOE = typesOE[type] ? typesOE[type] : ''

    theadWrap.html("")
    tbodyWrap.html("")
    tfootWrap.html("")

    if (data.length == 0) {
        return false
    }

    // head
    theadWrap.append(`
    <tr class="tr-th tr-sticky" id="columnsWrap">
        <th colspan="2">EXCELENCIA OPERATIVA DE SALA - ${typeOE}</th>
    </tr>
    `)

    data.map(dataItem => {
        $("#columnsWrap").append(`<th ficha-id="${dataItem.FichaId}">${moment(dataItem.Fecha).format('DD/MM/YYYY')}</th>`)
    })

    // body
    items.map((category, indexA) => {

        tbodyWrap.append(`
        <tr class="tr-th" id="categoriesWrap${indexA}">
            <th class="text-center" colspan="2">${category.Nombre}</th>
        </tr>
        `)

        data.map(dataItem => {
            $(`#categoriesWrap${indexA}`).append(`<th ficha-id="${dataItem.FichaId}">Puntuación</th>`)
        })

        category.Items.map((item, indexB) => {

            tbodyWrap.append(`
            <tr class="tr-td" id="itemsWrap${indexA}${indexB}" item-code="${item.Codigo}">
                <th>${indexB + 1}</th>
                <td class="text-left">${item.Nombre}</td>
            </tr>
            `)

            data.map(dataItem => {
                $(`#itemsWrap${indexA}${indexB}`).append(`<td ficha-id="${dataItem.FichaId}" indexa="${indexA}" indexb="${indexB}"></td>`)
            })

        })

    })

    // data
    data.map(dataItem => {
        
        dataItem.Categorias.map((category, indexA) => {

            category.Items.map((item, indexB) => {

                
                 if (item.TipoRespuesta == 7) {
                    var targetElement = tbodyWrap.find(`[ficha-id="${dataItem.FichaId}"][indexa="${indexA}"][indexb="${indexB}"]`);

                    if (item.Observacion.trim() !== '') {
                        var openModalButton = $('<button>', {
                            class: 'open-modal-button',
                            text: 'Ver comentario'
                        });

                        targetElement.append(openModalButton);

                        openModalButton.on('click', function () {
                            var modalContent = ` ${item.Observacion}`;

                            $('#modalComentarioDetalle .modal-body').html(modalContent);
                            $('#modalComentarioDetalle').modal('show');
                        });
                    }
                    else {
                        targetElement.append('<p class="not-comment">Sin comentario</p>');
                    }
                }
                else {
                    if (item.Codigo) {
                        tbodyWrap.find(`[item-code="${item.Codigo}"] [ficha-id="${dataItem.FichaId}"]`).html(item.PuntuacionObtenida)
                    }
                    else {
                        if (type == 1 && indexA == 0) {
                            if (indexB <= 5) {
                                tbodyWrap.find(`[ficha-id="${dataItem.FichaId}"][indexa="${indexA}"][indexb="${indexB}"]`).html(item.PuntuacionObtenida)
                            }
                            else {
                                tbodyWrap.find(`[ficha-id="${dataItem.FichaId}"][indexa="${indexA}"][indexb="${indexB + 1}"]`).html(item.PuntuacionObtenida)
                            }
                        }
                        else {
                            tbodyWrap.find(`[ficha-id="${dataItem.FichaId}"][indexa="${indexA}"][indexb="${indexB}"]`).html(item.PuntuacionObtenida)
                        }
                    }
                    
                }

            })

        })

    })

    // foot
    tfootWrap.append(`
    <tr class="tr-th" id="baseScoreWrap">
        <th class="bg-tfoot text-right" colspan="2">Puntuacion base</th>
    </tr>
    <tr class="tr-th" id="scoreWrap">
        <th class="bg-tfoot text-right" colspan="2">Puntuación Obtenida</th>
    </tr>
    <tr class="tr-th" id="percentageWrap">
        <th class="bg-tfoot text-right" colspan="2">Porcentaje</th>
    </tr>
    `)

    data.map(dataItem => {
        $("#baseScoreWrap").append(`<th ficha-id="${dataItem.FichaId}">${dataItem.PuntuacionBase}</th>`)
        $("#scoreWrap").append(`<th ficha-id="${dataItem.FichaId}">${dataItem.PuntuacionObtenida}</th>`)
        $("#percentageWrap").append(`<th ficha-id="${dataItem.FichaId}">${(dataItem.Porcentaje * 100).toFixed(1)}%</th>`)
    })
}

function downloadExcelNominal(params) {
    var data = {
        roomCode: params.roomCode,
        typeCode: params.typeCode,
        startDate: params.startDate,
        endDate: params.endDate
    }

    toastr.clear()

    /* ajax */
    $.ajax({
        type: "POST",
        url: `${basePath}ExcelenciaOperativa/DescargarExcelNominalPuntuacion`,
        data: JSON.stringify({ ...data }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.status) {
                var data = response.data
                var fileName = response.fileName
                let a = document.createElement('a')

                a.target = '_self'
                a.href = `data:application/vnd.ms-excel;base64,${data}`
                a.download = fileName
                a.click()
            }
            else {
                toastr.warning(response.message, "Mensaje Servidor")
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
    /* ajax */
}

$(document).ready(function () {

    // get rooms
    getAssignedRooms()

    // select2
    $("#room").select2()
    $("#type").select2()

    // datetimepicker
    $(".datepicker_only").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow
    })

    $(document).on('dp.change', '#startDate', function (e) {
        var date = new Date(e.date)
        $('#endDate').data("DateTimePicker").setMinDate(date)
    })

    $(document).on('dp.change', '#endDate', function (e) {
        var date = new Date(e.date)
        $('#startDate').data("DateTimePicker").setMaxDate(date)
    })

    // change room
    $(document).on('change', '#room', function (event) {
        event.preventDefault()

        tableNominalReport(null, [], [])
    })

    // change type
    $(document).on('change', '#type', function (event) {
        event.preventDefault()

        tableNominalReport(null, [], [])
    })

    // button search
    $(document).on('click', '#buttonSearchReport', function (event) {
        event.preventDefault()

        var roomCode = $("#room").val()
        var typeCode = $("#type").val()
        var startDate = $("#startDate").val()
        var endDate = $("#endDate").val()

        toastr.clear()

        if (!roomCode) {
            toastr.warning("Seleccione una sala")

            return false
        }

        if (!typeCode) {
            toastr.warning("Seleccione un tipo")

            return false
        }

        if (!startDate) {
            toastr.warning("Seleccione una fecha inicio")

            return false
        }

        if (!endDate) {
            toastr.warning("Seleccione una fecha fin")

            return false
        }

        args = {
            roomCode,
            typeCode,
            startDate,
            endDate
        }

        getNominalReport(args)
    })

    // button excel
    $(document).on('click', '#buttonDownloadExcel', function (event) {
        event.preventDefault()

        var roomCode = $("#room").val()
        var typeCode = $("#type").val()
        var startDate = $("#startDate").val()
        var endDate = $("#endDate").val()

        toastr.clear()

        if (!roomCode) {
            toastr.warning("Seleccione una sala")

            return false
        }

        if (!typeCode) {
            toastr.warning("Seleccione un tipo")

            return false
        }

        if (!startDate) {
            toastr.warning("Seleccione una fecha inicio")

            return false
        }

        if (!endDate) {
            toastr.warning("Seleccione una fecha fin")

            return false
        }

        args = {
            roomCode,
            typeCode,
            startDate,
            endDate
        }

        downloadExcelNominal(args)
    })

    $(document).on('click', '.change--version', function (event) {
        event.preventDefault()

        var typeCode = $("#type").val()
        var versionKey = $(this).attr('data-version')

        toastr.clear()

        if (!typeCode) {
            toastr.warning("Seleccione un tipo")

            return false
        }

        if (!versionKey) {
            toastr.warning("Seleccione una version")

            return false
        }

        VersionReporte = versionKey

        var data = ListaReporte.filter(x => x.FichaVersion == VersionReporte)

        tableNominalReport(typeCode, [], [])

        getFichaJson(typeCode).done(function (items) {
            tableNominalReport(typeCode, items, data)
        })
    })
})
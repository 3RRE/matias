let itemsExcel = []
$(document).ready(function () {
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: "YYYY-MM-DD",
        defaultDate: dateNow,
        maxDate: dateNow
    })
    $("#txtFechaSincronizacion").datetimepicker({
        pickTime: false,
        format: "YYYY-MM-DD",
        defaultDate: dateNow,
        maxDate: dateNow
    })
    $("#cboSala").select2({
        placeholder:'Seleccione Sala'
    })
    $("#cboProgresivo").select2({
        placeholder:'Seleccione Misterioso'
    })
    listarSalas().then(result => {
        renderSalas(result.data)
    })
    $(document).on('change', '#cboSala', function (e) {
        e.preventDefault()
        let salas = $(this).val()
        if (salas.length > 0) {
            listarProgesivosPorSala(salas).then(result => {
                if (result) {
                    renderProgresivos(result)
                }
            })
        }
        else {
            //limpiar combos
            $("#cboProgresivo").select2('destroy')
            $("#cboProgresivo").html('')
            $("#cboProgresivo").append('<option><option>')
            $("#cboProgresivo").select2({
                placeholder: 'Seleccione Misterioso'
            })
        }
    })
    $(document).on('click', '#btnBuscar', function (e) {
        itemsExcel = []
        let progresivos = $("#cboProgresivo").val()
        if (progresivos.length > 0) {
            listarReporteProgresivos(progresivos).then(response => {
                if (response) {
                    itemsExcel = response
                    renderReporte(response)
                    $('#overlaySearch').hide()
                }
                else {
                    toastr.error('No se encontraron resultados', 'Mensaje')
                }
            })
        }
        else {
            toastr.warning('Seleccione progresivo', 'Mensaje')
        }
    })
    $(document).on('click', '#btnExcel', function (e) {
        if (itemsExcel.length > 0) {
            let fechaIni = $("#txtFechaInicio").val().split('-')
            let fechaFin = $("#txtFechaFin").val().split('-')
            let periodo = `De ${fechaIni[2]}/${fechaIni[1]}/${fechaIni[0]} al ${fechaFin[2]}/${fechaFin[1]}/${fechaFin[0]}`
            let dataExcel = {
                Periodo: periodo,
                Items: itemsExcel,
                FechaIni: fechaIni,
                FechaFin: fechaFin
            }
            GenerarExcel(dataExcel)
        }
    })
})
function listarSalas() {
    ajaxhr = $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            return result
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    })
    AbortRequest.open()
    return ajaxhr
}
function renderSalas(data) {
    $("#cboSala").select2('destroy')
    $("#cboSala").html('')
    if (data) {
        let span = []
        data.map(item => {
            span.push(`<option value="${item.CodSala}" >${item.Nombre}</option>`)
        })
        $("#cboSala").prepend(span.join(''))
        $("#cboSala").select2({
            allowClear: true,
            placeholder: 'Seleccione Sala'
        })
    }
}
function listarProgesivosPorSala(listaSalas) {
    let dataForm = {
        salas: listaSalas
    }
    ajaxhr = $.ajax({
        type: "POST",
        url: basePath + "ControlProgresivo/ListarProgresivosSala",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            return result
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    })

    AbortRequest.open()

    return ajaxhr
}
function renderProgresivos(data) {
    $("#cboProgresivo").select2('destroy')
    $("#cboProgresivo").html('')
    if (data) {
        let span = []
        data.forEach(item => {
            const abrev = item.ClaseProgresivo === 'Terceros' ? 'T' : 'L'; 
            span.push(`<option value="${item.CodSala}-${item.CodSalaProgresivo}">${item.NombreSala} - ${item.Nombre} - (${abrev}) </option>`)
        })
        $("#cboProgresivo").prepend(span.join(''))
        $("#cboProgresivo").select2({
            allowClear: true,
            placeholder: 'Seleccione Misterioso'
        })
    }
}
function listarReporteProgresivos(listaProgresivos) {
    if (listaProgresivos) {
        let salas = $("#cboSala").val()
        let fechaInicio = $("#txtFechaInicio").val()
        let fechaFin = $("#txtFechaFin").val()
        let dataForm = {
            salas: salas,
            misteriosos: listaProgresivos,
            fechainicial: fechaInicio,
            fechaFinal: fechaFin
        }
        ajaxhr = $.ajax({
            type: "POST",
            url: basePath + "ControlProgresivo/getConsolidadoProgresivo",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                return result
            },
            error: function (request, status, error) {
            },
            complete: function (resul) {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            }
        })
        AbortRequest.open()
        return ajaxhr
    }
}
function renderReporte(data) {
    $("#contenedorReporte").html('')
    if (data) {
        let dataAgrupada = groupByMisterioso(data)
        let spanMisterioso = dataAgrupada.map(x => {
            let tituloMisterioso = `<h6 class="" style="text-transform:uppercase;text-align:center;">${x.ProgresivoCompleto}</h6>`
            let dataMisterioso = x.data
            let span = dataMisterioso.map((item, index) => {
                let dataError = item.dataError
                let strError = ''
                if(dataError){
                    strError = ` <span class="text-danger">(${dataError.Descripcion} - ${moment(dataError.FechaRegistro).format('YYYY-MM-DD HH:mm:ss A')})</span>`
                }
                let titulo = `<h5 class="" style="textTransform:uppercase">${moment(item.fecha).format('DD/MM/YYYY')} - <span class="text-primary">Coin In : ${item.CoinIn}</span>${strError}</h5>`
                //tabla Principal
                let dataBody = item.data
                let bodyTablaPrincipal = []
                if (dataBody) {
                    if(dataBody.length==0){
                        bodyTablaPrincipal.push(`<tr>
                                                    <td colspan="9" class="text-center"> No se encontraron resultados </td>
                                                </tr>`)
                    }
                    else{
                        bodyTablaPrincipal = dataBody.map(row => {
                            return `
                            <tr>
                                <td class="text-right">${row.NroPozo}</td>
                                <td>${row.Pozo}</td>
                                <td class="text-right">${row.Incremento}</td>
                                <td class="text-right">${row.subido}</td>
                                <td class="text-right">${row.basePozoHist}</td>
                                <td class="text-right">${row.Jackpot}</td>
                                <td class="text-right">${row.TotalDisplay}</td>
                                <td class="text-right">${row.RepSala}</td>
                                <td class="text-right">${Number(row.TotalDisplay - row.RepSala).toFixed(3)}</td>
                            </tr>
                        `
                        })
                    }
                }
                let tablaPrincipal = `
                    <div class="table-responsive">
                        <table id="table-principal-${index}" style="background: #fff"
                            class="table table-condensed table-bordered table-striped table-hover no-margin table-fixed">
                            <thead>
                                <tr>
                                    <th>Nro Pozo</th>		
                                    <th>Pozo</th>		
                                    <th class="sum">% Inc.</th>
                                    <th class="sum">Subido</th>
                                    <th class="sum">Base</th>
                                    <th class="sum">Jackpots</th>
                                    <th class="sum">Tot. Display</th>
                                    <th class="sum">Rep. Salas</th>
                                    <th class="sum">Diferencia</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${bodyTablaPrincipal.join('')}
                            </tbody>
                        </table>
                    </div>
                    `
                let htmltabla = `
                    <div id="progresivo-container">
                        <div class="mb-2">
                         ${titulo}
                        </div>
                        ${tablaPrincipal}
                    </div>
                `
                return htmltabla
            })
            let htmlAgrupado = `
                <div class="panel panel-info">
                    <div class="panel-heading">${tituloMisterioso}</div>
                    <div class="panel-body">${span.join('')}</div>
                </div>`
            return htmlAgrupado
        })
        $("#contenedorReporte").html(spanMisterioso.join(''))
    }
}
function renderReporte2(data) {
    $("#contenedorReporte").html('')
    if (data) {
        let span = data.map((item, index) => {
            let titulo = `<h5 class="" style="textTransform:uppercase">${item.Empresa} - ${item.Sala} - ${item.Misterioso} - <span class="text-primary">Coin In : ${item.CoinIn}</span>  ${moment(item.fecha).format('DD/MM/YYYY')}</h5>`
            //tabla Principal
            let data = item.data
            let bodyTablaPrincipal = []
            if (data) {
                bodyTablaPrincipal = data.map(row => {
                    return `
                        <tr>
                            <td>${row.NroPozo}</td>
                            <td>${row.Pozo}</td>
                            <td class="text-right">${row.Incremento}</td>
                            <td class="text-right">${row.subido}</td>
                            <td class="text-right">${row.basePozoHist}</td>
                            <td class="text-right">${row.Jackpot}</td>
                            <td class="text-right">${row.TotalDisplay}</td>
                            <td class="text-right">${row.RepSala}</td>
                            <td class="text-right">${Number(row.TotalDisplay - row.RepSala).toFixed(3)}</td>
                        </tr>
                    `
                })

            }
            let tablaPrincipal = `
            <div class="table-responsive">
                <table id="table-principal-${index}" style="background: #fff"
                    class="table table-condensed table-bordered table-striped table-hover no-margin table-fixed">
                    <thead>
                        <tr>
                            <th>Nro Pozo</th>		
                            <th>Pozo</th>		
                            <th class="sum">% Inc.</th>
                            <th class="sum">Subido</th>
                            <th class="sum">Base</th>
                            <th class="sum">Jackpots</th>
                            <th class="sum">Tot. Display</th>
                            <th class="sum">Rep. Salas</th>
                            <th class="sum">Diferencia</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${bodyTablaPrincipal.join('')}
                    </tbody>
                    <tfoot class="simple-table-footer">
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </tfoot>
                </table>
            </div>
            `
            let html = `
            <div id="progresivo-container">
                <div class="mb-2">
                 ${titulo}
                </div>
                ${tablaPrincipal}
            </div>
        `
            return html
        })

        $("#contenedorReporte").html(span.join(''))
        data.map((item, index) => {
            $("#table-principal-" + index).DataTable({
                "oLanguage": {
                    "sEmptyTable": "No hay registros",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sInfo": "Mostrando _START_ de _END_ de _TOTAL_ registros.",
                    "sSearch": "Buscar",
                    "oPaginate": {
                        "sPrevious": "Anterior",
                        "sNext": "Siguiente"
                    }
                },
                "bDestroy": true,
                bInfo: true,
                autoWidth: false,
                paging: true,
                searching: true,
                ordering: true,
                columnDefs: [
                    { width: "100px", targets: 0, className: 'text-right' },
                    { width: "150px", targets: 1, className: 'text-right' },
                    { width: "250px", targets: 2, className: 'text-right' },
                    { width: "100px", targets: 3, className: 'text-right' },
                    { width: "150px", targets: 4, className: 'text-right' },
                    { width: "100px", targets: 5, className: 'text-right' },
                    { width: "100px", targets: 6, className: 'text-right' },
                    { width: "100px", targets: 7, className: 'text-right' },
                    { width: "100px", targets: 8, className: 'text-right' }
                ],
                initComplete: function (settings, json) {
                    this.api().columns('.sum').every(function () {
                        var column = this;
                        // Remove the formatting to get integer data for summation
                        var intVal = function (i) {
                            if (typeof i === 'string') {
                                if ($(i).text() == '' || $(i).text() == undefined) {
                                    return i.replace(/[\$,]/gi, '') * 1;
                                } else {
                                    return Number($(i).text());
                                }
                            } else {
                                return typeof i === 'number' ? i : 0;
                            }
                            //return typeof i === 'string' ?
                            //i.replace(/[\$,]/gi, '')*1 : typeof i === 'number' ? i : 0;
                        };

                        if (column.data().length > 0) {
                            var sum = column
                                .data()
                                .reduce(function (a, b) {
                                    return intVal(a) + intVal(b);
                                });
                            $(column.footer()).addClass("text-right").css({ "font-weight": "bold" }).html(intVal(sum).toFixed(2));
                        } else {
                            $(column.footer()).addClass("text-right").css({ "font-weight": "bold" }).html(Number("0").toFixed(2));
                        }

                    });
                }
            })
        })
    }
}
function groupByMisterioso(data) {
    let groupedData=data.reduce((result, item) => {
        const key = item.ProgresivoCompleto
        if (!result[key]) {
            result[key] = []
        }
        result[key].push(item)
        return result
    }, {})
    let groupedArray = Object.keys(groupedData).map(key => ({
        ProgresivoCompleto: key,
        data: groupedData[key]
    }))
    return groupedArray
}
function GenerarExcel(data) {
    if (data) {
        let jsonData = JSON.stringify(data)
        let url = "ControlProgresivo/ExportToExcelConsolidadoContolProgresivo"
        ajaxhr = $.ajax({
            type: "POST",
            cache: false,
            url: basePath + url,
            contentType: "application/json; charset=utf-8",
            //dataType: "json",
            data: JSON.stringify({ jsonData: jsonData }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                if (response.respuesta) {
                    var data = response.data;
                    var file = response.excelName;
                    let a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/vnd.ms-excel;base64, " + data;
                    a.download = file;
                    a.click();
                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            //error: function (request, status, error) {
            //    toastr.error("Error De Conexion, Servidor no Encontrado.");
            //},
            complete: function (resul) {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            }
        })
        AbortRequest.open()
    }
}
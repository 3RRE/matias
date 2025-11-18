let consultaPorVpn=false
let ipPublica
let ipPrivada
let ipPublicaAlterna
let seEncontroAlterna=false
let urlsResultado=[]
let delay=60000
let timerId=''
// Listar Salas
function obtenerListaSalas(){
    return $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (result) {
            return result;
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    })
}
function renderSelectSalas(data){
    let element=$("#cboSalas")
    $.each(data, function (index, value) {
        element.append(`<option value="${value.UrlProgresivo==""?"":value.UrlProgresivo}" data-id="${value.CodSala}" data-ipprivada="${value.IpPrivada}" data-puertoservicio="${value.PuertoServicioWebOnline}">${value.Nombre}</option>`)
    });
}
function getPingSalas(){
    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath +"Progresivo/EchoPingSalasUsuario",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
        },
        success: function (response) {             
            return response
        },
        error: function (request, status, error) {            
        },
        complete: function (resul) {
        }
    })
}
function getUrl(url){
    if(url){
        try{
            let uri=new URL(url)
            return uri

        }catch(ex){
            return false
        }
    }
    return false
}
// function ListarSalas() {
//     var element = $("#cboSalas")

//     $.ajax({
//         type: "POST",
//         url: `${basePath}Sala/ListadoSalaPorUsuarioJson`,
//         cache: false,
//         contentType: "application/json; charset=utf-8",
//         dataType: "json",
//         beforeSend: function () {
//             $.LoadingOverlay("show")
//         },
//         success: function (response) {
//             var items = response.data

//             $.each(items, function (index, item) {
//                 element.append(`<option value="${item.CodSala}">${item.Nombre}</option>`)
//             })
//         },
//         error: function () {
//             toastr.error("Error", "Mensaje Servidor")
//         },
//         complete: function () {
//             $.LoadingOverlay("hide")
//         }
//     })
// }

// Obtener Cajas por Sala
function ListarCajasPorSala(roomCode) {
    var element = $("#cboCajas")

    $.ajax({
        type: "POST",
        url: `${basePath}Reportes/ListarCajas`,
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ roomCode: roomCode }),
        dataType: "json",
        beforeSend: function () {
            $.LoadingOverlay("show")
            element.html('')
        },
        success: function (response) {
            if (response.status === 1) {
                var items = response.data

                $.each(items, function (index, item) {
                    element.append(`<option value="${item.Cod_Caja}">${item.Nombre}</option>`)
                })

                toastr.success(response.message, "Mensaje Servidor")
            } else if (response.status === 0) {
                toastr.warning(response.message, "Mensaje Servidor")
            } else if (response.status === 2) {
                toastr.error(response.message, "Mensaje Servidor")
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}
function ListarCajasPorSalaVpn(urlPrivada,urlPublica){
    let element=$("#cboCajas")
    $.ajax({
        type:'POST',
        url:`${basePath}Reportes/ListarCajasVpn`,
        cache:false,
        contentType:"application/json; chartset=utf-8",
        data:JSON.stringify({urlPrivada:urlPrivada,urlPublica:urlPublica}),
        dataType:'json',
        beforeSend:function(){
            $.LoadingOverlay('show')
            element.html('')
        },
        success:function(response){
            if(response.status===1){
                let items=response.data
                $.each(items,function(index,item){
                    element.append(`<option value="${item.Cod_Caja}">${item.Nombre}</option>`)
                })
                toastr.success(response.message,'Mensaje Servidor')
            }else if(response.status===0){
                toastr.warning(response.message,'Mensaje Servidor')
            }else if(response.status===2){
                toastr.error(response.message,'Mensaje Servidor')
            }
        },
        complete:function(){
            $.LoadingOverlay('hide')
        }
    })
}

function ListarTITOCajas(params) {

    var data = {
        RoomCode: params.roomCode,
        BoxCode: params.boxCode,
        TurnCode: params.turnCode,
        OpeningDateStart: params.openingDateStart,
        OpeningDateEnd: params.openingDateEnd
    }

    ajaxhr = $.ajax({
        type: "POST",
        url: `${basePath}Reportes/ListarTITOCajas`,
        data: JSON.stringify({ args: data }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.status === 1) {
                var items = response.data
                var summary = response.summary

                RenderTITOCajas(items)
                RenderResumenTITOCajas(summary)

                $('#blockTITOCajas').removeClass('hidden')
                $('#blockResumenTITOCajas').removeClass('hidden')

            } else if (response.status === 0) {
                toastr.warning(response.message, "Mensaje Servidor")
            } else if (response.status === 2) {
                toastr.error(response.message, "Mensaje Servidor")
            }
        },
        complete: function () {
            AbortRequest.close()
            $.LoadingOverlay("hide")
        }
    })

    AbortRequest.open()
}
function ListarTITOCajasVpn(params,urlPublica,urlPrivada) {

    let args = {
        RoomCode: params.roomCode,
        BoxCode: params.boxCode,
        TurnCode: params.turnCode,
        OpeningDateStart: params.openingDateStart,
        OpeningDateEnd: params.openingDateEnd
    }
    let dataForm={
        urlPublica:urlPublica,
        urlPrivada:urlPrivada,
        args:args
    }

    ajaxhr = $.ajax({
        type: "POST",
        url: `${basePath}Reportes/ListarTITOCajasVpn`,
        data: JSON.stringify(dataForm),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.status === 1) {
                var items = response.data
                var summary = response.summary

                RenderTITOCajas(items)
                RenderResumenTITOCajas(summary)

                $('#blockTITOCajas').removeClass('hidden')
                $('#blockResumenTITOCajas').removeClass('hidden')

            } else if (response.status === 0) {
                toastr.warning(response.message, "Mensaje Servidor")
            } else if (response.status === 2) {
                toastr.error(response.message, "Mensaje Servidor")
            }
        },
        complete: function () {
            AbortRequest.close()
            $.LoadingOverlay("hide")
        }
    })

    AbortRequest.open()
}
function ExcelTITOCajas(params) {

    var data = {
        RoomCode: params.roomCode,
        BoxCode: params.boxCode,
        BoxName: params.boxName,
        TurnCode: params.turnCode,
        OpeningDateStart: params.openingDateStart,
        OpeningDateEnd: params.openingDateEnd
    }

    ajaxhr = $.ajax({
        type: "POST",
        url: `${basePath}Reportes/ExcelTITOCajas`,
        data: JSON.stringify({ args: data }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.status === 1) {
                var data = response.data
                var file = response.filename
                var a = document.createElement('a')
                a.target = '_self'
                a.href = `data:application/vnd.ms-excel;base64, ${data}`
                a.download = file
                a.click()

                toastr.success(response.message, "Mensaje Servidor")
            } else if (response.status === 0) {
                toastr.warning(response.message, "Mensaje Servidor")
            } else if (response.status === 2) {
                toastr.error(response.message, "Mensaje Servidor")
            }
        },
        complete: function () {
            AbortRequest.close()
            $.LoadingOverlay("hide")
        }
    })

    AbortRequest.open()
}
function ExcelTITOCajasVpn(params,urlPublica,urlPrivada) {

    let args = {
        RoomCode: params.roomCode,
        BoxCode: params.boxCode,
        BoxName: params.boxName,
        TurnCode: params.turnCode,
        OpeningDateStart: params.openingDateStart,
        OpeningDateEnd: params.openingDateEnd
    }
    let dataForm={
        args:args,
        urlPublica:urlPublica,
        urlPrivada:urlPrivada
    }

    ajaxhr = $.ajax({
        type: "POST",
        url: `${basePath}Reportes/ExcelTITOCajasVpn`,
        data: JSON.stringify(dataForm),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.status === 1) {
                var data = response.data
                var file = response.filename
                var a = document.createElement('a')
                a.target = '_self'
                a.href = `data:application/vnd.ms-excel;base64, ${data}`
                a.download = file
                a.click()

                toastr.success(response.message, "Mensaje Servidor")
            } else if (response.status === 0) {
                toastr.warning(response.message, "Mensaje Servidor")
            } else if (response.status === 2) {
                toastr.error(response.message, "Mensaje Servidor")
            }
        },
        complete: function () {
            AbortRequest.close()
            $.LoadingOverlay("hide")
        }
    })

    AbortRequest.open()
}

function RenderTITOCajas(items) {

    var statusTicket = {
        0: { name: 'ANULADO' },
        1: { name: 'PENDIENTE' },
        2: { name: 'PAGADO' },
        3: { name: 'VENCIDOS' }
    }

    var processTypeCodes = {
        1: { name: 'Ventas' },
        2: { name: 'Compras' },
        5: { name: 'Pagos Manuales' },
        4: { name: 'Cortesía' }
    }

    var gendersCodes = {
        'F': 'Femenino',
        'M': 'Masculino'
    }

    $("#tableTITOCajas").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": false,
        "paging": true,
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "aaSorting": [],
        data: items,
        columns: [
            {
                data: "ClienteDni", title: "DNI"
            },
            {
                data: "Cliente", title: "Cliente"
            },
            {
                data: "ClienteGenero", title: "Género", render: function (value) {
                    return gendersCodes[value] ? gendersCodes[value] : ''
                }
            },
            {
                data: "ClienteFechaNacimiento", title: "F. Nacimiento", render: function (value) {
                    var birthDate = ""
                    var dateFormat = "DD/MM/YYYY"
                    var date = moment(value).format(dateFormat)

                    if (value && moment(date, dateFormat, true).isValid()) {

                        if (date !== "01/01/1753") birthDate = date
                    }

                    return birthDate
                }
            },
            {
                data: "ClienteCorreo", title: "Correo"
            },
            {
                data: "ClienteTelefono", title: "Teléfono"
            },
            {
                data: "idTipoProceso", title: "Tipo de Proceso", render: function (value) {
                    return processTypeCodes[value] ? processTypeCodes[value].name : ''
                }
            },
            {
                data: "Fecha_Proceso", title: "Fecha y Hora Ticket",
                "render": function (value) {
                    return `${moment(value).format('DD/MM/YYYY')} ${moment(value).format('h:mm:ss a')}`
                }
            },
            {
                data: "TipoPago_desc", title: "Tipo de Pago"
            },
            {
                data: "TipoOrigen", title: "Tipo Origen"
            },
            {
                data: "LugarOrigen", title: "Lugar Origen"
            },
            {
                data: "CodigoExtra", title: "Código Extra"
            },
            {
                data: "Ticket", title: "Nro. Ticket"
            },
            {
                data: "Personal", title: "Personal"
            },
            {
                data: "EstadoTiket", title: "Estado", render: function (value) {
                    return statusTicket[value] ? statusTicket[value].name : ''
                }
            },
            {
                data: "Motivo", title: "Promoción"
            },
            {
                data: "Monto_Dinero_desc", title: "Monto Din."
            }
        ],
        columnDefs: [
            {
                targets: [0, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15],
                className: "text-center"
            },
            {
                targets: [16],
                className: "text-right"
            }
        ]
    })
}

function RenderResumenTITOCajas(summary) {

    var boxName = []
    var turnName = []

    var empresaText = $("#empresaText")
    var salaText = $("#salaText")
    var datePrinted = $("#datePrinted")
    var hourPrinted = $("#hourPrinted")
    var aperturaFecha = $("#aperturaFecha")
    var cajaNombres = $("#cajaNombres")
    var turnoNombres = $("#turnoNombres")
    var employeeText = $("#employeeText")

    var ventasCantidad = $("#ventasCantidad")
    var ventasTotal = $("#ventasTotal")
    var comprasCantidad = $("#comprasCantidad")
    var comprasTotal = $("#comprasTotal")
    var pagosCantidad = $("#pagosCantidad")
    var pagosTotal = $("#pagosTotal")
    var cortesiaCantidad = $("#cortesiaCantidad")
    var cortesiaTotal = $("#cortesiaTotal")
    var ticketsCantidad = $("#ticketsCantidad")
    var ticketsTotal = $("#ticketsTotal")

    $('#cboCajas option:selected').each(function (index, element) {
        boxName.push($(element).text())
    })

    $('#cboTurnos option:selected').each(function (index, element) {
        turnName.push($(element).text())
    })

    empresaText.html(summary.company)
    salaText.html(summary.room)
    datePrinted.html(summary.printDate)
    hourPrinted.html(summary.printHour)
    aperturaFecha.html(`${summary.dateProcessStart} - ${summary.dateProcessEnd}`)
    cajaNombres.html(boxName.join(', '))
    turnoNombres.html(turnName.join(', '))
    employeeText.html(summary.employee)

    ventasCantidad.html(summary.cantidadVentas)
    comprasCantidad.html(summary.cantidadCompras)
    pagosCantidad.html(summary.cantidadPagos)
    cortesiaCantidad.html(summary.cantidadCortesias)
    ticketsCantidad.html(summary.cantidadTickets)

    ventasTotal.html(summary.totalVentas)
    comprasTotal.html(summary.totalCompras)
    pagosTotal.html(summary.totalPagos)
    cortesiaTotal.html(summary.totalCortesias)
    ticketsTotal.html(summary.totalTickets)
}

function diffTwoDates(firstDate, secondDate) {
    var date1 = moment(firstDate, 'DD/MM/YYYY')
    var date2 = moment(secondDate, 'DD/MM/YYYY')

    var diffDays = date2.diff(date1, 'days')

    return diffDays
}

$(document).ready(function () {

    clearTimeout(timerId)
    timerId = setTimeout(function request() {
        getPingSalas().then(result=>{
            urlsResultado=result
        })
        timerId = setTimeout(request, delay);
    }, delay)
    //ListarSalas()
    obtenerListaSalas().then(result=>{
        if(result.data){
            renderSelectSalas(result.data)
            getPingSalas().then(response=>{
                urlsResultado=response
            })
        }
    })

    $("#cboSalas").select2()

    $("#cboCajas").select2({
        multiple: true,
        allowClear: true,
        placeholder: "Seleccione una opción"
    })

    $("#cboTurnos").select2({
        multiple: true,
        allowClear: true,
        placeholder: "Seleccione una opción"
    }).val([]).trigger('change')

    // set date
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow
    })

    $(document).on('dp.change', '#fechaAperturaInicio', function (e) {
        var date = new Date(e.date)
        $('#fechaAperturaFin').data("DateTimePicker").setMinDate(date)
    })

    $(document).on('dp.change', '#fechaAperturaFin', function (e) {
        var date = new Date(e.date)
        $('#fechaAperturaInicio').data("DateTimePicker").setMaxDate(date)
    })

    $(document).on('change', '#cboSalas', function (event) {
        event.preventDefault()
        if($("#cboSalas").val()==0){
            return false
        }
        if(urlsResultado.length==0){
            toastr.warning("Inténtelo de nuevo en unos momentos",'Mensaje Servidor')
            $("#cboSalas").val('0').trigger('change')
            return false
        }
      
        toastr.remove()

        RenderTITOCajas([])

        $('#blockTITOCajas').addClass('hidden')
        $('#blockResumenTITOCajas').addClass('hidden')

        ipPublica = $(this).val();
        ipPrivada=$("#cboSalas option:selected").data('ipprivada')
        let puertoServicio=$("#cboSalas option:selected").data('puertoservicio')
        ipPrivada=ipPrivada+':'+puertoServicio
        let uri=getUrl(ipPublica)
        const obj=urlsResultado.find(item=>item.uri==ipPublica)

        let roomCode = $("#cboSalas option:selected").data('id')
        console.log(roomCode)
        if (!roomCode) {
            toastr.warning("Seleccione una sala")
            return false
        }
        if(uri&&obj.respuesta){
            // let urlPublica=ipPublica+'/servicio/ListarCajas?roomCode='+roomCode
            consultaPorVpn=false
            ListarCajasPorSala(roomCode)
        }
        else{
            consultaPorVpn=true
            ipPublicaAlterna=urlsResultado.find(x=>x.respuesta)
            ipPublicaAlterna = getUrl("http://190.187.44.222:9895")
            let urlPrivada=ipPrivada+'/servicio/ListarCajas?roomCode='+roomCode
            let urlPublica=ipPublicaAlterna+"/servicio/ListarCajasVpn"
            ListarCajasPorSalaVpn(urlPrivada,urlPublica)
        }
    })

    $(document).on('click', '#btnBuscar', function (event) {
        event.preventDefault()

        var roomCode = $('#cboSalas option:selected').data('id')
        var boxCode = $('#cboCajas').val()
        var turnCode = $('#cboTurnos').val()
        var openingDateStart = $('#fechaAperturaInicio').val()
        var openingDateEnd = $('#fechaAperturaFin').val()

        toastr.remove()

        if (!roomCode) {
            toastr.warning("Seleccione una sala")
            return false
        }

        if (!boxCode) {
            toastr.warning("Seleccione una caja")
            return false
        }

        if (!turnCode) {
            toastr.warning("Seleccione un turno")
            return false
        }

        if (!openingDateStart) {
            toastr.warning("Seleccione una fecha inicio")
            return false
        }

        if (!openingDateEnd) {
            toastr.warning("Seleccione una fecha fin")
            return false
        }

        if (diffTwoDates(openingDateStart, openingDateEnd) > 5) {
            toastr.warning("Por favor, seleccione fechas de 5 días de diferencia")
            return false
        }

        RenderTITOCajas([])

        $('#blockTITOCajas').addClass('hidden')
        $('#blockResumenTITOCajas').addClass('hidden')

        let params = {
            roomCode,
            boxCode,
            turnCode,
            openingDateStart,
            openingDateEnd
        }
        if(consultaPorVpn){
            let urlPublica=`${ipPublicaAlterna}/servicio/ListarDetalleTITOCajasVpn`
            let urlPrivada=`${ipPrivada}/servicio/ListarDetalleTITOCajas`
            ListarTITOCajasVpn(params,urlPublica,urlPrivada)
        }
        else{
            ListarTITOCajas(params)
        }
    })

    $(document).on('click', '#btnExcel', function (event) {
        event.preventDefault()

        var roomCode = $('#cboSalas option:selected').data('id')
        var boxCode = $('#cboCajas').val()
        var turnCode = $('#cboTurnos').val()
        var openingDateStart = $('#fechaAperturaInicio').val()
        var openingDateEnd = $('#fechaAperturaFin').val()

        toastr.remove()

        if (!roomCode) {
            toastr.warning("Seleccione una sala")
            return false
        }

        if (!boxCode) {
            toastr.warning("Seleccione una caja")
            return false
        }

        if (!turnCode) {
            toastr.warning("Seleccione un turno")
            return false
        }

        if (!openingDateStart) {
            toastr.warning("Seleccione una fecha inicio")
            return false
        }

        if (!openingDateEnd) {
            toastr.warning("Seleccione una fecha fin")
            return false
        }

        if (diffTwoDates(openingDateStart, openingDateEnd) > 5) {
            toastr.warning("Por favor, seleccione fechas de 5 días de diferencia")
            return false
        }

        var boxName = []

        $('#cboCajas option:selected').each(function (index, element) {
            boxName.push($(element).text())
        })

        let params = {
            roomCode,
            boxCode,
            boxName,
            turnCode,
            openingDateStart,
            openingDateEnd
        }
        if(consultaPorVpn){
            let urlPublica=`${ipPublicaAlterna}/servicio/ListarDetalleTITOCajasVpn`
            let urlPrivada=`${ipPrivada}/servicio/ListarDetalleTITOCajas`
            ExcelTITOCajasVpn(params,urlPublica,urlPrivada)
        }
        else{
            ExcelTITOCajas(params)
        }
    })
})
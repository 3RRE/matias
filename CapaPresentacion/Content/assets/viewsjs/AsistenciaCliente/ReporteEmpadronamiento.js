// Obtener Salas Asignadas al Usuario
function listarSalasAsignadas()
{
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
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

function renderSalasAsignadas(items)
{
    var element = $('#sala_id')

    element.html(`<option value="">--Seleccione una opción--</option>`)

    items.map(item => {
        element.append(`<option value="${item.CodSala}">${item.Nombre}</option>`)
    })

    element.select2({
        multiple: false,
        placeholder: "--Seleccione una opción--",
        allowClear: true,
        width: "100%"
    })
}

function listarEmpadronamientoCliente(params)
{
    var args = {
        roomId: params.roomId,
        fromDate: params.fromDate,
        toDate: params.toDate
    }

    $.ajax({
        type: "POST",
        url: `${basePath}ClienteEmpadronamiento/ListarEmpadronamientoCliente`,
        data: JSON.stringify(args),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function ()
        {
            $.LoadingOverlay("show")
        },
        success: function (response)
        {
            if (response.status == 1)
            {
                var data = response.data

                renderEmpadronamientoCliente(data)
            }
            else if (response.status == 2)
            {
                toastr.warning(response.message, "Mensaje Servidor")
            }
            else if (response.status == 0)
            {
                toastr.error(response.message, "Mensaje Servidor")
            }
        },
        complete: function ()
        {
            $.LoadingOverlay("hide")
        }
    })
}

function renderEmpadronamientoCliente(data)
{
    $("#tableEmpadronamientoCliente").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": false,
        "paging": true,
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "aaSorting": [],
        data: data,
        columns: [
            {
                data: "NroDoc", title: "DNI"
            },
            {
                data: "NombreCompleto", title: "Nombres y Apellidos"
            },
            {
                data: "ZonaNombreIn", title: "Zona"
            },
            {
                data: "entrega_dni", title: "Entregó documento de identidad", render: function (data) {
                    return data ? 'SI' : 'NO'
                }
            },
            {
                data: "reniec", title: "Verificación DNI", render: function (data) {
                    return data ? 'SI' : 'NO'
                }
            },
            {
                data: "apuestaImportante", title: "Apuesta", render: function (data) {
                    return data.toFixed(2)
                }
            },
            {
                data: "observacion", title: "Observaciones", render: function (data) {
                    return truncateString(data, 100)
                }
            },
            {
                data: "Estado", title: "Estatus", render: function (data) {
                    return data ? 'Completado' : 'Pendiente'
                }
            },
            {
                data: "FechaSalida", title: "Hora de salida", render: function (data, type, row) {
                    return row.Estado == 1 ? `${moment(data).format('hh:mm:ss A')}` : '---'
                }
            }
        ],
        columnDefs: [
            {
                targets: [0, 3, 4, 7, 8],
                className: "text-center"
            },
            {
                targets: [5],
                className: "text-right"
            },
            {
                targets: [6],
                className: "text-wrap"
            }
        ]
    })
}

function excelEmpadronamientoCliente(params)
{
    var args = {
        roomId: params.roomId,
        fromDate: params.fromDate,
        toDate: params.toDate
    }

    $.ajax({
        type: "POST",
        url: `${basePath}ClienteEmpadronamiento/ExcelEmpadronamientoCliente`,
        data: JSON.stringify(args),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function ()
        {
            $.LoadingOverlay("show")
        },
        success: function (response)
        {
            if (response.status == 1)
            {
                var data = response.data
                var file = response.fileName
                var a = document.createElement('a')

                a.target = '_self'
                a.href = `data:application/vnd.ms-excel;base64, ${data}`
                a.download = file
                a.click()

                toastr.success(response.message, "Mensaje Servidor")
            }
            else if (response.status == 2)
            {
                toastr.warning(response.message, "Mensaje Servidor")
            }
            else if (response.status == 0)
            {
                toastr.error(response.message, "Mensaje Servidor")
            }
        },
        complete: function ()
        {
            $.LoadingOverlay("hide")
        }
    })
}

// document ready
$(document).ready(function () {

    // listar salas asignadas
    listarSalasAsignadas().done(function (response) {
        if (response.data) {
            renderSalasAsignadas(response.data)
        }
    })

    // datetimepicker
    $(".date_only").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow
    })

    // on change fecha inicio
    $(document).on('dp.change', '#fromDate', function (event) {
        var date = new Date(event.date)

        $('#toDate').data("DateTimePicker").setMinDate(date)
    })

    // on change fecha fin
    $(document).on('dp.change', '#toDate', function (event) {
        var date = new Date(event.date)

        $('#fromDate').data("DateTimePicker").setMaxDate(date)
    })

    // on change sala
    $(document).on('change', '#sala_id', function (event) {
        event.preventDefault()

        renderEmpadronamientoCliente([])
    })

    // on click search
    $(document).on('click', '#btn_search', function (event) {
        event.preventDefault()

        var roomId = $('#sala_id').val()
        var fromDate = $('#from_date').val()
        var toDate = $('#to_date').val()

        toastr.clear()

        if (!roomId) {
            toastr.warning("Seleccione una sala")

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

        /*if (gapTwoDates(fromDate, toDate) >= 30) {
            toastr.warning("Por favor, seleccione fechas de 30 días de diferencia")

            return false
        }*/

        var args = {
            roomId,
            fromDate,
            toDate
        }

        listarEmpadronamientoCliente(args)
    })

    // on click excel
    $(document).on('click', '#btn_excel', function (event) {
        event.preventDefault()

        var roomId = $('#sala_id').val()
        var fromDate = $('#from_date').val()
        var toDate = $('#to_date').val()

        toastr.clear()

        if (!roomId) {
            toastr.warning("Seleccione una sala")

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

        var args = {
            roomId,
            fromDate,
            toDate
        }

        excelEmpadronamientoCliente(args)
    })
})
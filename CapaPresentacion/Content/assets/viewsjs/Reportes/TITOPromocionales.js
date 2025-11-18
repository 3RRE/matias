// Listar Salas
function ListarSalas() {
    var element = $("#cboSalas")

    $.ajax({
        type: "POST",
        url: `${basePath}Sala/ListadoSalaPorUsuarioJson`,
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $.LoadingOverlay("show")
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

function ListarTITOPromocionales(roomCode, startDate, endDate) {
    $.ajax({
        type: "POST",
        url: `${basePath}Reportes/ListarTITOPromocionales`,
        data: JSON.stringify({ roomCode: roomCode, startDate: startDate, endDate: endDate }),
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

                RenderTITOPromocionales(items)
                $('#blockTITOPromocionales').removeClass('hidden')

            } else if (response.status === 0) {
                toastr.warning(response.message, "Mensaje Servidor")
            } else if (response.status === 2) {
                toastr.error(response.message, "Mensaje Servidor")
            }
        },
        error: function () {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

function ExcelTITOPromocionales(roomCode, startDate, endDate) {
    $.ajax({
        type: "POST",
        url: `${basePath}Reportes/ExcelTITOPromocionales`,
        data: JSON.stringify({ roomCode: roomCode, startDate: startDate, endDate: endDate }),
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
        error: function () {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

function RenderTITOPromocionales(items) {

    $("#tableTITOPromocionales").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": false,
        "paging": true,
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        data: items,
        columns: [
            {
                data: "TitoCortesia", title: "Cortesía", render: function (value) {
                    return value.toFixed(2)
                }
            },
            {
                data: "TitoCortesiaNoDest", title: "Cortesía No Dest", render: function (value) {
                    return value.toFixed(2)
                }
            },
            {
                data: "TitoPromocion", title: "Promoción", render: function (value) {
                    return value.toFixed(2)
                }
            },
            {
                data: "TitoPromocionNoDest", title: "Promoción No Dest", render: function (value) {
                    return value.toFixed(2)
                }
            },
            {
                data: "Estado", title: "Estado", render: function (value) {
                    return value
                }
            },
            {
                data: "FechaTicketIni", title: "Fecha Ticket Inicio",
                "render": function (value) {
                    return `${moment(value).format('DD/MM/YYYY')} ${moment(value).format('h:mm:ss a')}`
                }
            },
            {
                data: "FechaTicketFin", title: "Fecha Ticket Fin",
                "render": function (value) {
                    return `${moment(value).format('DD/MM/YYYY')} ${moment(value).format('h:mm:ss a')}`
                }
            }
        ],
        columnDefs: [
            {
                targets: '_all',
                className: 'text-center'
            }
        ]
    })
}

$(document).ready(function () {
    ListarSalas()

    $("#cboSalas").select2()

    // set date
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow
    })

    $(document).on('change', '#cboSalas', function (event) {
        event.preventDefault()

        RenderTITOPromocionales([])
        $('#blockTITOPromocionales').addClass('hidden')
    })

    $(document).on('click', '#btnBuscar', function (event) {
        event.preventDefault()

        var roomCode = $('#cboSalas').val()
        var startDate = $('#fechaInicio').val()
        var endDate = $('#fechaFinal').val()

        if (!roomCode) {
            toastr.warning("Seleccione una sala")
            return false
        }

        if (!startDate) {
            toastr.warning("Ingrese una fecha inicio")
            return false
        }

        if (!endDate) {
            toastr.warning("Seleccione una fecha final")
            return false
        }

        ListarTITOPromocionales(roomCode, startDate, endDate)
    })

    $(document).on('click', '#btnExcel', function (event) {
        event.preventDefault()

        var roomCode = $('#cboSalas').val()
        var startDate = $('#fechaInicio').val()
        var endDate = $('#fechaFinal').val()

        if (!roomCode) {
            toastr.warning("Seleccione una sala")
            return false
        }

        if (!startDate) {
            toastr.warning("Ingrese una fecha inicio")
            return false
        }

        if (!endDate) {
            toastr.warning("Seleccione una fecha final")
            return false
        }

        ExcelTITOPromocionales(roomCode, startDate, endDate)
    })
})
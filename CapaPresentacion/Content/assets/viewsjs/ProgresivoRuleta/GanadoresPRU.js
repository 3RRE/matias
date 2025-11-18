function ObtenerSalas() {
    var element = $("#selectSalas")

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
            var items = response.data ?? []

            $.each(items, function (_, item) {
                element.append(`<option value="${item.CodSala}">${item.Nombre}</option>`)
            })
        },
        error: function () {
            toastr.error("Error al cargar los datos. Por favor, vuelve a intentarlo.")

            $.LoadingOverlay("hide")
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

function ObtenerRuletas() {
    var element = $("#selectRuletas")
    var salaId = $('#selectSalas').val()

    element.html(`<option value="">--Seleccione--</option>`)

    if (!salaId) {
        return
    }

    var data = {
        salaId: parseInt(salaId)
    }

    $.ajax({
        type: "POST",
        url: `${basePath}ProgresivoRuleta/SeleccionarRuletasPRU`,
        data: JSON.stringify(data),
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            var items = response.data ?? []

            $.each(items, function (_, item) {
                element.append(`<option value="${item.Id}">${item.Nombre}</option>`)
            })
        },
        error: function () {
            toastr.error("Error al cargar los datos. Por favor, vuelve a intentarlo.")

            $.LoadingOverlay("hide")
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

function ObtenerRecords(data) {
    $.ajax({
        type: "POST",
        url: `${basePath}ProgresivoRuleta/ObtenerGanadoresPRU`,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            RenderRecords([])

            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.success) {
                var items = response.data

                RenderRecords(items)

                $('#blockTableRecords').removeClass('hidden')

            } else {
                toastr.warning(response.message, "Mensaje Servidor")
            }
        },
        error: function () {
            toastr.error("Error al cargar los datos. Por favor, vuelve a intentarlo.")

            $.LoadingOverlay("hide")
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

function ExcelRecords(data) {
    $.ajax({
        type: "POST",
        url: `${basePath}ProgresivoRuleta/ExcelGanadoresPRU`,
        data: JSON.stringify(data),
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
                a.href = `data:application/vnd.ms-excel;base64,${data}`
                a.download = file
                a.click()

                toastr.success(response.message, "Mensaje Servidor")
            } else {
                toastr.warning(response.message, "Mensaje Servidor")
            }
        },
        error: function () {
            toastr.error("Error al cargar los datos. Por favor, vuelve a intentarlo.")

            $.LoadingOverlay("hide")
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

function RenderRecords(items) {
    $('#tableRecords').DataTable({
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
                data: "Id",
                title: "Id"
            },
            {
                data: null,
                title: "Sala",
                render: function (value) {
                    return value?.Sala?.Nombre ?? ''
                }
            },
            {
                data: null,
                title: "Ruleta",
                render: function (value) {
                    return value?.Ruleta?.Nombre ?? ''
                }
            },
            {
                data: "CodMaquina",
                title: "Cod. Máquina"
            },
            {
                data: "Monto",
                title: "Monto",
                render: function (value) {
                    return value.toFixed(2)
                }
            },
            {
                data: "FechaGanador",
                title: "Fecha y Hora",
                render: function (value) {
                    return `${moment(value).format('DD/MM/YYYY')} ${moment(value).format('H:mm:ss')}`
                }
            },
            {
                data: "EsAcreditado",
                title: "Acreditado",
                render: function (value) {
                    switch (value) {
                        case 1:
                            return "Ganador";
                        case 2:
                            return "Ganador 2";
                        default:
                            return "No acreditado";
                    }
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
    ObtenerSalas()

    $("#selectSalas").select2({
        multiple: false,
        allowClear: true,
        placeholder: "Seleccione una opción"
    })

    $("#selectRuletas").select2({
        multiple: false,
        allowClear: true,
        placeholder: "Seleccione una opción"
    })

    $(".date-only").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow
    })

    $(document).on('change', '#selectSalas', function (event) {
        event.preventDefault()

        RenderRecords([])
        ObtenerRuletas()

        $('#blockTableRecords').addClass('hidden')
    })

    $(document).on('click', '#btnBuscar', function (event) {
        event.preventDefault()

        var salaId = $('#selectSalas').val()
        var ruletaId = $('#selectRuletas').val()
        var fechaInicial = $('#fechaInicial').val()
        var fechaFinal = $('#fechaFinal').val()

        toastr.remove()

        if (!salaId) {
            toastr.warning("Seleccione una sala")

            return
        }

        if (!fechaInicial) {
            toastr.warning("Ingrese una fecha inicial")

            return
        }

        if (!fechaFinal) {
            toastr.warning("Seleccione una fecha final")

            return
        }

        var data = {
            salaId: parseInt(salaId),
            ruletaId: parseInt(ruletaId),
            fechaInicial,
            fechaFinal
        }

        ObtenerRecords(data)
    })

    $(document).on('click', '#btnExcel', function (event) {
        event.preventDefault()

        var salaId = $('#selectSalas').val()
        var ruletaId = $('#selectRuletas').val()
        var fechaInicial = $('#fechaInicial').val()
        var fechaFinal = $('#fechaFinal').val()

        toastr.remove()

        if (!salaId) {
            toastr.warning("Seleccione una sala")

            return
        }

        if (!fechaInicial) {
            toastr.warning("Ingrese una fecha inicial")

            return
        }

        if (!fechaFinal) {
            toastr.warning("Seleccione una fecha final")

            return
        }

        var data = {
            salaId: parseInt(salaId),
            ruletaId: parseInt(ruletaId),
            fechaInicial,
            fechaFinal
        }

        ExcelRecords(data)
    })
})
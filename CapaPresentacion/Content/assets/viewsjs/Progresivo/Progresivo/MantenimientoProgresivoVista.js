// Get Url
function getUrl(baseUrl) {
    var url = ""

    try {
        url = new URL(baseUrl)
    }
    catch (exception) {
        url = ""
    }

    return url
}

// Obtener Salas Asignadas
function obtenerSalasAsignadas() {
    ajaxhr = $.ajax({
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
            AbortRequest.close()
            $.LoadingOverlay("hide")
        }
    })
    AbortRequest.open()
    return ajaxhr
}

// Render Salas Asignadas
function renderSalasAsignadas(data) {
    var element = $("#cboSala")

    $.each(data, function (index, value) {
        element.append(`<option value="${value.CodSala}" data-index="${index}">${value.Nombre}</option>`)
    })

    element.select2({
        multiple: false,
        placeholder: "--Seleccione--",
        allowClear: true,
        width: "100%"
    });

    element.val(null).trigger("change")
}

// Listar Progresivos
function listarProgresivos(params) {

    var data = {
        salaId: params.room
    }

    ajaxhr = $.ajax({
        type: "POST",
        url: `${basePath}Progresivo/ListarProgresivos`,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            renderProgresivos([])
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.status == 1)
            {
                var items = response.data

                renderProgresivos(items)
            }

            if (response.status == 2)
            {
                toastr.warning(response.message, "Mensaje Servidor")
            }

            if (response.status == 0)
            {
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

// Render Progresivos
function renderProgresivos(items) {
    $("#tableProgresivos").DataTable({
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
                data: "WEB_Nombre", title: "Nombre Progresivo"
            },
            {
                data: "WEB_Url", title: "Url Progresivo"
            },
            {
                data: "WEB_Estado", title: "Estado", render: function (data) {

                    var status = '<span class="label btn-danger">INACTIVO</span>'

                    if (data == 1) {
                        status = '<span class="label btn-success">ACTIVO</span>'
                    }

                    return status
                }
            },
            {
                data: "WEB_FechaRegistro", title: "Fecha Registro", render: function (data) {
                    return moment(data).format('DD/MM/YYYY')
                }
            },
            {
                data: "WEB_Url", title: "Acción", render: function (data) {

                    var url = getUrl(data)
                    var hostname = ""

                    if (url) {
                        hostname = `${url.protocol}//${url.hostname}`
                    }

                    var buttons = `
                    <button type="button" class="btn btn-xs btn-success button_reiniciar" data-url="${hostname}">REINICIAR</button>
                    <button type="button" class="btn btn-xs btn-danger button_obtener_fecha" data-url="${hostname}">CAMBIAR HORA</button>
                    `

                    return buttons
                },
                sortable: false,
                searchable: false
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

function reiniciarProgresivo(params)
{
    var data = {
        salaId: params.salaId,
        urlProgresivo: params.urlProgresivo
    }

    ajaxhr = $.ajax({
        type: "POST",
        url: `${basePath}Progresivo/ReiniciarProgresivo`,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.status == 1)
            {
                toastr.success(response.data, "Mensaje Servidor")
            }

            if (response.status == 2)
            {
                toastr.warning(response.message, "Mensaje Servidor")
            }

            if (response.status == 0)
            {
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

function obtenerFechaProgresivo(params)
{
    var data = {
        salaId: params.salaId,
        urlProgresivo: params.urlProgresivo
    }

    ajaxhr = $.ajax({
        type: "POST",
        url: `${basePath}Progresivo/ObtenerFechaProgresivo`,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            setViewFechaProgresivo()
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.status == 1)
            {
                viewFechaProgresivo(data.urlProgresivo, response.data)

                toastr.success(response.message, "Mensaje Servidor")
            }

            if (response.status == 2)
            {
                toastr.warning(response.message, "Mensaje Servidor")
            }

            if (response.status == 0)
            {
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

function viewFechaProgresivo(urlProgresivo, data)
{
    var progresivo_url = $('#progresivo_url')
    var progresivo_fecha = $('#progresivo_fecha')
    var progresivo_hora = $('#progresivo_hora')

    progresivo_url.val(urlProgresivo)
    progresivo_fecha.val(moment(data.date).format('DD/MM/YYYY'))
    progresivo_hora.val(moment(data.date).format('HH:mm:ss'))

    $("#modal_progresivo_fechahora").modal('show')
}

function setViewFechaProgresivo()
{
    var progresivo_url = $('#progresivo_url')
    var progresivo_fecha = $('#progresivo_fecha')
    var progresivo_hora = $('#progresivo_hora')

    progresivo_url.val(null)
    progresivo_fecha.data("DateTimePicker").setDate(null)
    progresivo_hora.data("DateTimePicker").setDate(null)
}

function actualizarFechaProgresivo(params)
{
    var hora = params.progresivo_hora.substring(0, 8)
    var listaHora = hora.split(':')
    var urlFechaHora = `${params.progresivo_fecha}/${listaHora.join('/')}`

    var data = {
        salaId: params.sala_id,
        urlProgresivo: params.progresivo_url,
        urlFechaHora: urlFechaHora
    }

    ajaxhr = $.ajax({
        type: "POST",
        url: `${basePath}Progresivo/ActualizarFechaProgresivo`,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.status == 1)
            {
                toastr.success(response.data, "Mensaje Servidor")

                $("#modal_progresivo_fechahora").modal('hide')
            }

            if (response.status == 2)
            {
                toastr.warning(response.message, "Mensaje Servidor")
            }

            if (response.status == 0)
            {
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

// document ready
$(document).ready(function () {
    // obtener salas asignadas
    obtenerSalasAsignadas().done(response => {
        if (response.data) {
            renderSalasAsignadas(response.data)
        }
    })

    // init render progresivos
    renderProgresivos([])

    // datetimepicker fecha progresivo
    $('#progresivo_fecha').datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        autoclose: true,
        defaultDate: dateNow
    })

    // datetimepicker hora progresivo
    $('#progresivo_hora').datetimepicker({
        pickDate: false,
        format: 'HH:mm:ss',
        autoclose: true,
        defaultDate: dateNow
    })

    // change sala
    $(document).on('change', '#cboSala', function (event) {
        event.preventDefault()

        renderProgresivos([])
    })

    // button search
    $(document).on('click', '#button_search', function (event) {
        event.preventDefault()

        var room = $('#cboSala').val()

        toastr.remove()

        if (!room) {
            toastr.warning("Seleccione una sala")

            return false
        }

        var args = {
            room
        }

        listarProgresivos(args)
    })

    // button reiniciar
    $(document).on('click', '.button_reiniciar', function (event) {
        event.preventDefault()

        var salaId = $('#cboSala').val()
        var urlProgresivo = $(this).attr('data-url')
        //urlProgresivo = "http://192.168.1.40"

        toastr.remove()

        if (!salaId) {
            toastr.warning("Seleccione una sala")

            return false
        }

        if (!urlProgresivo) {
            toastr.warning("El Progresivo no tiene URL")

            return false
        }

        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: 'Mensaje Servidor',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: "Reiniciar",
            cancelButton: 'Cerrar',
            content: `¿Está seguro de reiniciar el progresivo ${urlProgresivo}?`,
            confirm: function () {
                var args = {
                    salaId,
                    urlProgresivo
                }

                reiniciarProgresivo(args)
            }
        })
    })

    // button obtener hora
    $(document).on('click', '.button_obtener_fecha', function (event) {
        event.preventDefault()

        var salaId = $('#cboSala').val()
        var urlProgresivo = $(this).attr('data-url')
        //urlProgresivo = "http://192.168.1.40"

        toastr.remove()

        if (!salaId) {
            toastr.warning("Seleccione una sala")

            return false
        }

        if (!urlProgresivo) {
            toastr.warning("El Progresivo no tiene URL")

            return false
        }

        var args = {
            salaId,
            urlProgresivo
        }

        obtenerFechaProgresivo(args)
    })

    // button Actualizar Progresivo Fecha Hora
    $(document).on('click', '#button_update_date', function (event) {
        event.preventDefault()

        var sala_id = $('#cboSala').val()
        var progresivo_url = $('#progresivo_url').val()
        var progresivo_fecha = $('#progresivo_fecha').val()
        var progresivo_hora = $('#progresivo_hora').val()
        //urlProgresivo = "http://192.168.1.40"

        toastr.remove()

        if (!sala_id) {
            toastr.warning("Por favor, seleccione una sala")

            return false
        }

        if (!progresivo_url) {
            toastr.warning("El Progresivo no tiene URL")

            return false
        }

        if (!progresivo_fecha) {
            toastr.warning("Por favor, seleccione una fecha")

            return false
        }

        if (!progresivo_hora) {
            toastr.warning("Por favor, seleccione una hora")

            return false
        }

        var args = {
            sala_id,
            progresivo_url,
            progresivo_fecha,
            progresivo_hora
        }

        actualizarFechaProgresivo(args)
    })
})
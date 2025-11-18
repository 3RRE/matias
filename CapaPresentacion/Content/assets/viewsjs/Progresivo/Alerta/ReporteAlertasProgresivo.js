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
    })

    element.val(null).trigger("change")
}

// Listar Alertas Progresivo
function listarAlertasProgresivo(params)
{
    var data = {
        salaId: params.room,
        fechaInicio: params.fromDate,
        fechaFin: params.toDate
    }

    ajaxhr = $.ajax({
        type: "POST",
        url: `${basePath}AlertaProgresivo/ListarAlertasProgresivo`,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function ()
        {
            renderAlertasProgresivo([])

            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.status == 1)
            {
                var items = response.data

                const tipo = $("#cboTipo").val(); 
                const itemsFiltered = (tipo == 0) ? items : items.filter(x => x.Tipo == tipo);

                renderAlertasProgresivo(itemsFiltered)
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
        complete: function ()
        {
            AbortRequest.close()
            $.LoadingOverlay("hide")
        }
    })
    AbortRequest.open()
}

function renderAlertasProgresivo(items)
{
    $("#tableAlertasProgresivo").DataTable({
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
                data: "SalaNombre",
                title: "Sala"
            },
            {
                data: "ProgresivoNombre",
                title: "Progresivo"
            },
            {
                data: "Descripcion",
                title: "Descripción"
            },
            {
                data: "FechaRegistro",
                title: "Fecha Registro",
                render: function (data)
                {
                    return moment(data).format('DD/MM/YYYY HH:mm:ss')
                }
            },
            {
                data: "Id",
                title: "Acción",
                render: function (data)
                {
                    var buttons = `
                    <button type="button" class="btn btn-xs btn-success button_render_detail" data-id="${data}">Detalle</button>
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

function excelAlertasProgresivo(params)
{
    var data = {
        salaId: params.room,
        fechaInicio: params.fromDate,
        fechaFin: params.toDate
    }

    ajaxhr = $.ajax({
        type: "POST",
        url: `${basePath}AlertaProgresivo/ExcelAlertasProgresivo`,
        data: JSON.stringify(data),
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

            if (response.status == 2)
            {
                toastr.warning(response.message, "Mensaje Servidor")
            }

            if (response.status == 0)
            {
                toastr.error(response.message, "Mensaje Servidor")
            }
        },
        complete: function ()
        {
            AbortRequest.close()
            $.LoadingOverlay("hide")
        }
    })
    AbortRequest.open()
}

function obtenerAlertaProgresivo(alertaId)
{
    var data = {
        alertaId
    }

    ajaxhr = $.ajax({
        type: "POST",
        url: `${basePath}AlertaProgresivo/ObtenerAlertaProgresivo`,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        complete: function () {
            AbortRequest.close()
            $.LoadingOverlay("hide")
        }
    })
    AbortRequest.open()
    return ajaxhr
}

// document ready
$(document).ready(function () {

    var AppAlertaProgresivo = new Vue({
        el: '#AppAlertaProgresivo',
        data: {
            alert: null,
            detalleActual: null,
            detalleAnterior: null,
            diferentes: null,
        },
        methods: {
            pozoName: function (pozo) {
                var pozos = {
                    1: 'Pozo Superior',
                    2: 'Pozo Medio',
                    3: 'Pozo Inferior'
                }

                return pozos[pozo.TipoPozo] ? pozos[pozo.TipoPozo] : ''
            },
            alertClassTable: function (detalle) {
                return {
                    'table-alertp-current': detalle.ProActual == true,
                    'table-alertp-old': detalle.ProActual == false
                }
            },
            classChange: function (current, attribute) {
                return {
                    'change-current': current && this.diferentes.some(x => x.Campo == attribute),
                    'change-old': !current && this.diferentes.some(x => x.Campo == attribute)
                }
            },
            classChangePozo: function (current, attribute, index) {
                return {
                    'change-current': current && this.diferentes.some(x => x.Campo == attribute && x.Pozo == index + 1),
                    'change-old': !current && this.diferentes.some(x => x.Campo == attribute && x.Pozo == index + 1)
                }
            }
        },
        created: function () {
            this.$on('setAlert', function (alert) {
                this.alert = alert
                this.detalleActual = alert.Detalle.find(x => x.ProActual == true)
                this.detalleAnterior = alert.Detalle.find(x => x.ProActual == false)
            }.bind(this))

            this.$on('setDifferents', function (differents) {
                this.diferentes = differents
            }.bind(this))
        }
    })

    // init render Alertas Progresivo
    renderAlertasProgresivo([])

    // obtener salas asignadas
    obtenerSalasAsignadas().done(response => {
        if (response.data) {
            renderSalasAsignadas(response.data)
        }
    })

    // date only
    $(".date--only").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow
    })

    // on change sala
    $(document).on('change', '#cboSala', function (event) {
        event.preventDefault()

        renderAlertasProgresivo([])
    })

    // button search
    $(document).on('click', '#button_search', function (event) {
        event.preventDefault()

        var room = $('#cboSala').val()
        var fromDate = $('#fromDate').val()
        var toDate = $('#toDate').val()

        toastr.remove()

        if (!room) {
            toastr.warning("Por favor, seleccione una sala")

            return false
        }

        if (!fromDate) {
            toastr.warning("Por favor, seleccione una fecha inicio")

            return false
        }

        if (!toDate) {
            toastr.warning("Por favor, seleccione una fecha fin")

            return false
        }

        var args = {
            room,
            fromDate,
            toDate
        }

        listarAlertasProgresivo(args)
    })

    // button search
    $(document).on('click', '#button_excel', function (event) {
        event.preventDefault()

        var room = $('#cboSala').val()
        var fromDate = $('#fromDate').val()
        var toDate = $('#toDate').val()

        toastr.remove()

        if (!room) {
            toastr.warning("Por favor, seleccione una sala")

            return false
        }

        if (!fromDate) {
            toastr.warning("Por favor, seleccione una fecha inicio")

            return false
        }

        if (!toDate) {
            toastr.warning("Por favor, seleccione una fecha fin")

            return false
        }

        var args = {
            room,
            fromDate,
            toDate
        }

        excelAlertasProgresivo(args)
    })

    // on click detail
    $(document).on('click', '.button_render_detail', function (event) {
        event.preventDefault()

        var alertaId = $(this).attr('data-id')

        toastr.remove()

        if (!alertaId) {
            toastr.warning("Por favor, seleccione una registro de alertas")

            return false
        }

        var modalDetalle = $('#modal_pro_alerta_detalle')

        obtenerAlertaProgresivo(alertaId).done(function (response) {
            if (response.success) {
                AppAlertaProgresivo.$emit('setAlert', response.data)
                AppAlertaProgresivo.$emit('setDifferents', response.diferentes)
            } else {
                toastr.warning(response.message, "Mensaje Servidor")
            }

        }).then(function () {
            modalDetalle.modal('show')
        })
    })

    // cboTipo search
    $(document).on('change', '#cboTipo', function (event) {
        event.preventDefault()

        renderAlertasProgresivo([])

    })

})
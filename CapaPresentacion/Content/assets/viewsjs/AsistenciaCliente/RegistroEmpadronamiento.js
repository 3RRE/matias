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

function listarZonasAsignadas(sala_id)
{
    var args = {
        salaId: sala_id,
    }

    return $.ajax({
        type: "POST",
        url: `${basePath}/Zona/ListarZonasPorSala`,
        data: JSON.stringify(args),
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

function buscarEmpadronamientoCliente(document_number)
{
    var args = {
        documentNumber: document_number,
    }

    return $.ajax({
        type: "POST",
        url: `${basePath}/ClienteEmpadronamiento/BuscarEmpadronamientoCliente`,
        data: JSON.stringify(args),
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

function guardarEmpadronamientoCliente(params)
{
    var args = {
        cliente_id: params.customer_id,
        cod_sala: params.sala_id,
        ZonaIdIn: params.zona_id,
        entrega_dni: params.emc_entrego_dni,
        apuestaImportante: params.emc_apuesta,
        observacion: params.emc_observacion,
        reniec: params.from_reniec
    }

    return $.ajax({
        type: "POST",
        url: `${basePath}/ClienteEmpadronamiento/GuardarEmpadronamientoClienteV2`,
        data: JSON.stringify({ empadronamiento: args }),
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

function registrarFechaHoraSalida(data)
{
    var args = {
        empadronamientoId: data.id
    }

    return $.ajax({
        type: "POST",
        url: `${basePath}/ClienteEmpadronamiento/RegistrarFechaHoraSalida`,
        data: JSON.stringify(args),
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

function renderSalasAsignadas(items, parent = '')
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
        width: "100%",
        dropdownParent: parent
    })

    if (items.length == 1) {
        element.val(items[0].CodSala).trigger('change')
    }
}

function renderZonasAsignadas(items, parent = '')
{
    var element = $('#zona_id')

    element.html(`<option value="">--Seleccione una opción--</option>`)

    items.map(item => {
        element.append(`<option value="${item.Id}">${item.Nombre}</option>`)
    })

    element.select2({
        multiple: false,
        placeholder: "--Seleccione una opción--",
        allowClear: true,
        width: "100%",
        dropdownParent: parent
    })
}

function renderNuevoEmpadronamiento(data, fromReniec)
{
    var customer_id = $('#customer_id')
    var customer_dni = $('#customer_dni')
    var customer_fullname = $('#customer_fullname')
    var from_reniec = $('#from_reniec')

    customer_id.val(data.cliente_id)
    customer_dni.val(data.DNI)
    customer_fullname.val(data.NombreCompleto)
    from_reniec.val(fromReniec)
}

function renderEmpadronamientoCliente(data)
{
    var items = []

    if (data) {
        items.push(data)
    }

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
        data: items,
        columns: [
            {
                data: "NroDoc", title: "DNI"
            },
            {
                data: "NombreCompleto", title: "Nombres y Apellidos"
            },
            {
                data: "ZonaNombreIn", title: "Zona", render: function (data, type, row) {
                    return `${row.NombreSala} - ${data}`
                }
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

function resetFormEmpadronamiento()
{
    var customer_id = $('#customer_id')
    var customer_dni = $('#customer_dni')
    var customer_fullname = $('#customer_fullname')
    var zona_id = $('#zona_id')
    var emc_entrego_dni = $('#emc_entrego_dni')
    var emc_apuesta = $('#emc_apuesta')
    var emc_observacion = $('#emc_observacion')
    var from_reniec = $('#from_reniec')

    customer_id.val(null)
    customer_dni.val(null)
    customer_fullname.val(null)
    zona_id.val(null).trigger('change')
    emc_entrego_dni.val(null)
    emc_apuesta.val(null)
    emc_observacion.val(null)
    from_reniec.val(null)
}

function validateNumber(event)
{
    var pattern = /^[0-9]$/

    return pattern.test(event.key)
}

// document ready
$(document).ready(function () {

    var modalGuardarEMC = $('#modal_guardar_empadronamiento')
    var formEMC = $("#form_empadronamiento_cliente")
    var boxTableEMC = $('#boxTableEMC')

    // listar salas asignadas
    listarSalasAsignadas().done(function (response) {
        if (response.data) {
            renderSalasAsignadas(response.data, modalGuardarEMC)
        }
    })

    $('#document_number').trigger('focus')

    modalGuardarEMC.on('hidden.bs.modal', function () {
        $('#document_number').trigger('focus')
    })

    // validate form
    formEMC.bootstrapValidator({
        container: '#messages',
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            sala_id: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            zona_id: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            emc_entrego_dni: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            emc_apuesta: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            }
        }
    }).on('success.field.bv', function (event, data) {
        event.preventDefault()

        var parent = data.element.parents('.form-group')

        parent.removeClass('has-success')
        parent.find(`.form-control-feedback[data-bv-icon-for="${data.field}"]`).hide()
    })

    // on keypress document number
    $(document).on('keypress', '#document_number', function (event) {
        var code = event.keyCode || event.which

        if (code == 13)
        {
            $("#btn_buscar_empadronamiento").trigger("click")
        }

        if (!validateNumber(event)) {
            return false
        }
    })

    // on click buscar
    $(document).on('click', '#btn_buscar_empadronamiento', function (event) {
        event.preventDefault()

        var elementInput = $('#document_number')
        var document_number = elementInput.val().trim()

        toastr.remove()
        elementInput.trigger('focus')

        if (!document_number) {
            toastr.warning("Por favor, ingrese un número de documento", "Advertencia")

            return false
        }

        if (document_number.length < 8) {
            toastr.warning("Por favor, ingrese un número de documento igual o mayor a 8 dígitos", "Advertencia")

            return false
        }

        boxTableEMC.addClass('hide')

        renderEmpadronamientoCliente(null)

        buscarEmpadronamientoCliente(document_number).done(function (response) {

            if (response.status == 1)
            {
                var empadronamiento = response.data

                $.confirm({
                    icon: 'fa fa-spinner fa-spin',
                    title: `Registrar salida`,
                    content: `El número de documento <b>${empadronamiento.NroDoc}</b> se encuentra registrado con fecha y hora ${moment(empadronamiento.fecha).format('DD/MM/YYYY hh:mm:ss A')} en <b>${empadronamiento.NombreSala}</b>. ¿Quiere registrar su salida?`,
                    confirmButton: 'Sí, registrar salida',
                    cancelButton: 'Cancelar',
                    confirmButtonClass: 'btn-success',
                    animationBounce: 1.5,
                    confirm: function () {
                        registrarFechaHoraSalida(response.data).done(function (response) {
                            if (response.status == 1)
                            {
                                renderEmpadronamientoCliente(response.data)

                                boxTableEMC.removeClass('hide')
                                toastr.success(response.message, "Mensaje Servidor")
                            }
                            else
                            {
                                toastr.error(response.message, "Mensaje Servidor")
                            }
                        })
                    }
                })

                elementInput.val('')
            }
            else if (response.status == 2)
            {
                renderNuevoEmpadronamiento(response.data, response.fromReniec)

                elementInput.val('')
                modalGuardarEMC.modal('show')
            }
            else if (response.status == 3)
            {
                toastr.warning(response.message, "Mensaje Servidor")
            }
            else
            {
                toastr.error(response.message, "Mensaje Servidor")
            }

        }).then(function () {
            formEMC.data('bootstrapValidator').resetForm()
        })
    })

    // on change sala
    $(document).on('change', '#sala_id', function (event) {
        event.preventDefault()

        var sala_id = $(this).val()

        renderZonasAsignadas([], modalGuardarEMC)

        if (sala_id)
        {
            listarZonasAsignadas(sala_id).done(function (response) {
                if (response.status) {
                    renderZonasAsignadas(response.data, modalGuardarEMC)
                }
            })
        }
    })

    // on click guardar
    $(document).on('click', '#btn_guardar_empadronamiento', function (event) {
        event.preventDefault()

        formEMC.data('bootstrapValidator').resetForm()
        var validate = formEMC.data('bootstrapValidator').validate()

        if (validate.isValid())
        {
            var customer_id = $('#customer_id').val()
            var sala_id = $('#sala_id').val()
            var zona_id = $('#zona_id').val()
            var emc_entrego_dni = $('#emc_entrego_dni').val()
            var emc_apuesta = $('#emc_apuesta').val()
            var emc_observacion = $('#emc_observacion').val()
            var from_reniec = $('#from_reniec').val()

            if (!customer_id) {
                toastr.warning('Por favor, ingrese un cliente', "Mensaje Servidor")

                return false
            }

            emc_entrego_dni = emc_entrego_dni == 1 ? true : false

            var args = {
                customer_id,
                sala_id,
                zona_id,
                emc_entrego_dni,
                emc_apuesta,
                emc_observacion,
                from_reniec
            }

            $.confirm({
                icon: 'fa fa-spinner fa-spin',
                title: 'Registro empadronamiento',
                content: '¿Esta seguro de enviar registro para empadronamiento?',
                confirmButton: 'Sí, enviar registro',
                cancelButton: 'Cancelar',
                confirmButtonClass: 'btn-success',
                animationBounce: 1.5,
                confirm: function () {
                    guardarEmpadronamientoCliente(args).done(function (response) {
                        if (response.status == 1)
                        {
                            renderEmpadronamientoCliente(response.data)

                            modalGuardarEMC.modal('hide')
                            resetFormEmpadronamiento()
                            boxTableEMC.removeClass('hide')
                            toastr.success(response.message, "Mensaje Servidor")
                        }
                        else if (response.status == 2)
                        {
                            toastr.error(response.message, "Mensaje Servidor")
                        }
                        else if (response.status == 3) {
                            toastr.warning(response.message, "Mensaje Servidor")
                        }
                        else {
                            toastr.error(response.message, "Mensaje Servidor")
                        }
                    })
                }
            })
        }
        else
        {
            toastr.clear()
            toastr.warning("Complete los campos obligatorios", "Mensaje Servidor")
        }
    })
})
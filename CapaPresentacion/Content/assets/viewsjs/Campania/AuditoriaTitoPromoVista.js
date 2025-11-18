// Data Auditoria TITOS Promocionales
var dataTitos = {
    items: [],
    tickets: [],
    setItems: function (items) {
        this.items = items
    },
    getItems: function () {
        return this.items
    },
    getItem: function (transactionDate) {
        return this.items.find(item => item.transactionDate === transactionDate)
    },
    setTickets: function (tickets) {
        this.tickets = tickets
    },
    pushTickets: function (transactionDate, item, nroticket, amount) {
        var tito = this.getItem(transactionDate).itemsDiff.find(ticket => ticket.D00H_Item == item && ticket.Ticket == nroticket && ticket.Monto_Dinero == amount)

        if (tito != null) {
            this.tickets.push(tito)
        }
    },
    removeTicket: function (item, nroticket, amount) {
        this.tickets = this.tickets.filter(ticket => ticket.D00H_Item !== item && ticket.Ticket !== nroticket && ticket.Monto_Dinero !== amount)
    },
    getTickets: function () {
        return this.tickets
    }
}

// Obtener Salas Asignadas al Usuario
function obtenerListaSalas()
{
    return $.ajax({
        type: "POST",
        url: `${basePath}/Sala/ListadoSalaPorUsuarioJson`,
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
            return response;
        },
        complete: function ()
        {
            $.LoadingOverlay("hide")
        }
    })
}

// render salas asignadas en un select
function renderSelectSalasAudiPromo(data)
{
    $.each(data, function (index, value)
    {
        $("#cboSalaAudiPromo").append(`<option value="${value.CodSala}" data-urlprogresivo="${value.UrlProgresivo}" data-id="${value.CodSala}" data-ipprivada="${value.IpPrivada}" data-puertoservicio="${value.PuertoServicioWebOnline}">${value.Nombre}</option>`)
    })

    $("#cboSalaAudiPromo").select2({
        multiple: false,
        placeholder: "--Seleccione--",
        allowClear: true,
        width: "100%"
    });

    $("#cboSalaAudiPromo").val(null).trigger("change")
}

// ATP Listado
function listarAuditoriaPromocion(params)
{
    // args
    var args = {
        RoomCode: params.room,
        OpeningDateStart: params.fromDate,
        OpeningDateEnd: params.toDate
    }

    var url = `${basePath}Campania/ListarAuditoriaTITOPromo`
    var data = JSON.stringify({ args: args })

    renderAuditoriaPromocion([])

    ajaxhr = $.ajax({
        type: "POST",
        url: url,
        data: data,
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
                var items = response.data

                dataTitos.setItems(items)
                renderAuditoriaPromocion(items)
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
            AbortRequest.close()
            $.LoadingOverlay("hide")
        }
    })

    AbortRequest.open()
}

// Renders Auditoria TITO Promocionales
function renderAuditoriaPromocion(items)
{
    $("#tableAuditoriaPromocion").DataTable({
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
                data: null, title: "Item", render: function (data, type, row, meta) {
                    return meta.row + 1
                }
            },
            {
                data: "transactionDate", title: "Fecha Operación"
            },
            {
                data: "quantityBox", title: "Cantidad"
            },
            {
                data: "amountBox", title: "Importe Soles", render: function (data) {
                    return data.toFixed(2)
                }
            },
            {
                data: "quantityIas", title: "Cantidad"
            },
            {
                data: "amountIas", title: "Importe Soles", render: function (data) {
                    return data.toFixed(2)
                }
            },
            {
                data: "quantityDiff", title: "Cantidad"
            },
            {
                data: "amountDiff", title: "Importe Soles", render: function (data) {
                    return data.toFixed(2)
                }
            },
            {
                data: null, title: "Acciones", render: function (data) {

                    var buttons = `
                    <button type="button" class="btn btn-xs btn-primary atp_generados_caja" title="Generados en Caja" data-transactiondate="${data.transactionDate}"><i class="fa fa-barcode"></i></button>
                    <button type="button" class="btn btn-xs btn-success atp_enlazados_ias" title="Enlazados en IAS" data-transactiondate="${data.transactionDate}"><i class="fa fa-database"></i></button>
                    <button type="button" class="btn btn-xs btn-warning atp_diferencia" title="Diferencia" data-transactiondate="${data.transactionDate}"><i class="fa fa-server"></i></button>
                    `;

                    return buttons;
                }
            },
        ],
        columnDefs: [
            {
                targets: "_all",
                className: "text-center"
            }
        ]
    })
}

// ATP abrir generados
function openATPGeneradosCaja(transactionDate)
{
    var titos = dataTitos.getItem(transactionDate)

    toastr.clear()

    if (!titos) {
        toastr.warning('La fecha de operación seleccionada no tiene registros')

        return false
    }

    $('#modal_atp_generados_caja').modal('show')

    renderATPGeneradosCaja(titos)
}

// ATP abrir enlazados
function openATPEnlazadosIAS(transactionDate)
{
    var titos = dataTitos.getItem(transactionDate)

    toastr.clear()

    if (!titos) {
        toastr.warning('La fecha de operación seleccionada no tiene registros')

        return false
    }

    $('#modal_atp_enlazados_ias').modal('show')

    renderATPEnlazadosIAS(titos)
}

// ATP abrir diferencia
function openATPDiferencia(transactionDate)
{
    var titos = dataTitos.getItem(transactionDate)

    toastr.clear()

    if (!titos) {
        toastr.warning('La fecha de operación seleccionada no tiene registros')

        return false
    }

    $('#modal_atp_diferencias').modal('show')

    renderATPDiferencias(titos)
}

// ATP render generados
function renderATPGeneradosCaja(titos)
{
    $("#tableATPGeneradosCaja").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": false,
        "paging": true,
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "aaSorting": [],
        data: titos.itemsBox,
        columns: [
            {
                data: "FechaApertura", title: "Fecha Apertura", render: function (data) {
                    return `${moment(data).format('DD/MM/YYYY hh:mm:ss A')}`
                }
            },
            {
                data: "Fecha_Proceso_Inicio", title: "Fecha Ticket", render: function (data) {
                    return `${moment(data).format('DD/MM/YYYY hh:mm:ss A')}`
                }
            },
            {
                data: "Ticket", title: "Nro"
            },
            {
                data: "Monto_Dinero", title: "Monto", render: function (data) {
                    return data.toFixed(2)
                }
            },
            {
                data: "Cliente", title: "Cliente"
            },
            {
                data: "ClienteDni", title: "Nro. Doc"
            },
            {
                data: "ClienteTelefono", title: "Teléfono"
            },
            {
                data: "Personal", title: "Usuario Reg."
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

// ATP render enlazados
function renderATPEnlazadosIAS(titos)
{
    var instanciasCode = {
        1: 'Procedimiento Manual',
        2: 'Instancia por Auditoria'
    }

    $("#tableATPEnlazadosIAS").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": false,
        "paging": true,
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "aaSorting": [],
        data: titos.itemsIas,
        columns: [
            {
                data: "FechaApertura", title: "Fecha Apertura", render: function (data) {
                    return `${moment(data).format('DD/MM/YYYY hh:mm:ss A')}`
                }
            },
            {
                data: "fecharegsala", title: "Fecha Ticket", render: function (data) {
                    return `${moment(data).format('DD/MM/YYYY hh:mm:ss A')}`
                }
            },
            {
                data: "nroticket", title: "Nro"
            },
            {
                data: "monto", title: "Monto", render: function (data) {
                    return data.toFixed(2)
                }
            },
            {
                data: "estado", title: "Instancia", render: function (data) {
                    return instanciasCode[data] ? instanciasCode[data] : ''
                }
            },
            {
                data: "Campania.nombre", title: "Campaña"
            },
            {
                data: "NombreCompleto", title: "Cliente"
            },
            {
                data: "NroDoc", title: "Nro. Doc"
            },
            {
                data: "Celular1", title: "Teléfono"
            },
            {
                data: "nombre_usuario", title: "Usuario Reg."
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

// ATP render diferencias
function renderATPDiferencias(titos)
{
    dataTitos.setTickets([])

    $("#tableATPDiferencias").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": false,
        "paging": true,
        "autoWidth": false,
        "bProcessing": true,
        //"bDeferRender": true,
        "aaSorting": [],
        data: titos.itemsDiff,
        columns: [
            {
                data: null, title: `<input type="checkbox" class="icheck_all hidden_">`, render: function (data) {
                    return `<input type="checkbox" class="icheck_single" data-transactiondate="${moment(data.FechaApertura).format('DD/MM/YYYY')}" data-item="${data.D00H_Item}" data-nroticket="${data.Ticket}" data-amount="${data.Monto_Dinero}">`
                }
            },
            {
                data: "FechaApertura", title: "Fecha Apertura", render: function (data) {
                    return `${moment(data).format('DD/MM/YYYY hh:mm:ss A')}`
                }
            },
            {
                data: "Fecha_Proceso_Inicio", title: "Fecha Ticket", render: function (data) {
                    return `${moment(data).format('DD/MM/YYYY hh:mm:ss A')}`
                }
            },
            {
                data: "Ticket", title: "Nro"
            },
            {
                data: "Monto_Dinero", title: "Monto", render: function (data) {
                    return data.toFixed(2)
                }
            },
            {
                data: "Cliente", title: "Cliente"
            },
            {
                data: "ClienteDni", title: "Nro. Doc"
            },
            {
                data: "ClienteTelefono", title: "Teléfono"
            },
            {
                data: "Personal", title: "Usuario Reg."
            }
        ],
        columnDefs: [
            {
                targets: 0,
                orderable: false,
                searchable: false
            },
            {
                targets: "_all",
                className: "text-center"
            }
        ],
        drawCallback: function () {
            $('.icheck_all, .icheck_single').iCheck({
                checkboxClass: 'icheckbox_square-blue icheckbox_bg-white'
            })
        }
    })
}

// ATP no enlazados a campania
function renderATPNoEnlazadosCampania(tickets)
{
    $("#tableATPNoEnlazadosCampania").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": false,
        "paging": true,
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "aaSorting": [],
        data: tickets,
        columns: [
            {
                data: "FechaApertura", title: "Fecha Apertura", render: function (data) {
                    return `${moment(data).format('DD/MM/YYYY hh:mm:ss A')}`
                }
            },
            {
                data: "Fecha_Proceso_Inicio", title: "Fecha Ticket", render: function (data) {
                    return `${moment(data).format('DD/MM/YYYY hh:mm:ss A')}`
                }
            },
            {
                data: "Ticket", title: "Nro"
            },
            {
                data: "Monto_Dinero", title: "Monto", render: function (data) {
                    return data.toFixed(2)
                }
            },
            {
                data: "Cliente", title: "Cliente"
            },
            {
                data: "ClienteDni", title: "Nro. Doc"
            },
            {
                data: "ClienteTelefono", title: "Teléfono"
            },
            {
                data: "Personal", title: "Usuario Reg."
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

// Get ATP Enlazar Tickets
function getATPEnlazarTickets()
{
    var tickets = dataTitos.getTickets()
    var room_id = $("#cboSalaAudiPromo").val()

    toastr.clear()

    if (tickets.length == 0) {
        toastr.warning('Seleccione uno o más Tickets')

        return false
    }

    if (!room_id) {
        toastr.warning('Seleccione una Sala')

        return false
    }
    $("#SalaId").val(room_id)
    var data = {
        room_id
    }

    $.ajax({
        type: "POST",
        url: `${basePath}Campania/GetATPEnlazarTickets`,
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
                renderATPEnlazarTickets(response)
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

// render campanias activas
function renderSelectCampaniasActivas(element, items, modalParent)
{
    element.html(`<option value="">Seleccione una opción</option>`)

    $.each(items, function (index, item)
    {
        element.append(`<option value="${item.id}">${item.nombre}</option>`)
    })

    element.select2({
        multiple: false,
        placeholder: "--Seleccione--",
        allowClear: true,
        width: "100%",
        dropdownParent: modalParent
    });

    element.val(null).trigger("change");
}

// Render ATP Enlazar Tickets
function renderATPEnlazarTickets(response)
{
    var campaigns = response.campaigns
    var withclient = response.withclient

    $('#documentNumber').val('')
    $('#apt_customer_id').val('')
    $('#atp_document_mumber').val('')
    $('#atp_customer_fullname').val('')

    $('#atp_validate_client_view').addClass('hide')
    $('#atp_client_view').addClass('hide')
    $("#btn_proceder_enlace").addClass("hide")

    if (withclient)
    {
        $('#apt_customer_id').val('')
        $('#atp_validate_client_view').removeClass('hide')
    }
    else
    {
        $('#apt_customer_id').val(0)
        $('#atp_validate_client_view').addClass('hide')
        $("#btn_proceder_enlace").removeClass("hide")
    }

    var element = $('#atp_campanias_activas')
    var modalParent = $('#modal_atp_enlazar_tickets')

    renderSelectCampaniasActivas(element, campaigns, modalParent)

    modalParent.modal('show')
}

// Proceder a enlazar tickets
function initProcederEnlaceTITOs(campania_id, customer_id)
{
    var sala_id = $("#cboSalaAudiPromo").val()
    var fromDate = $('#fromDateAudiPromo').val()
    var toDate = $('#toDateAudiPromo').val()

    // Data
    var tickets = dataTitos.getTickets()

    var data = {
        sala_id,
        campania_id,
        customer_id,
        tickets
    }

    var args = {
        room: sala_id,
        fromDate: fromDate,
        toDate: toDate
    }

    $.ajax({
        type: "POST",
        url: `${basePath}Campania/ATPEnlazarTickets`,
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
                var noEnlazados = response.dataOut

                toastr.success(response.message, "Mensaje Servidor")

                $('#modal_atp_enlazar_tickets').modal('hide')
                $('#modal_atp_diferencias').modal('hide')

                listarAuditoriaPromocion(args)

                if (noEnlazados.length > 0)
                {
                    $('#modal_atp_noenlazados_campania').modal('show')

                    renderATPNoEnlazadosCampania(noEnlazados)
                }

                dataTitos.setTickets([])

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

// excel auditoria promocion
function excelAuditoriaPromocion(params)
{
    var args = {
        RoomCode: params.room,
        OpeningDateStart: params.fromDate,
        OpeningDateEnd: params.toDate
    }

    ajaxhr = $.ajax({
        type: "POST",
        url: `${basePath}Campania/ExcelAuditoriaTITOPromo`,
        data: JSON.stringify({ parameters: args }),
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
            AbortRequest.close()
            $.LoadingOverlay("hide")
        }
    })

    AbortRequest.open()
}

// validate customer
function validateCustomer(documentNumber)
{
    documentNumber = documentNumber.trim()

    toastr.clear()

    if (!documentNumber) {
        toastr.warning('Ingrese número de documento')

        return false
    }

    if (documentNumber.length < 8) {
        toastr.warning('Número de documento mínimo 8 dígitos')

        return false
    }

    //with
    $("#atp_document_mumber").val('')
    $("#atp_customer_fullname").val('')
    $("#apt_customer_id").val('')

    // without
    $("#txtNombrevalidar").val('')
    $("#txtApelPatvalidar").val('')
    $("#txtApelMatvalidar").val('')
    $("#txtNroDocvalidar").val('')

    $.ajax({
        type: "POST",
        url: basePath + "CampaniaCliente/GetClienteCoincidencia",
        data: JSON.stringify({ coincidencia: documentNumber }),
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
            var status = response.respuesta
            var customer = response.data

            if (status)
            {
                $("#atp_document_mumber").val(customer.NroDoc)
                $("#atp_customer_fullname").val(customer.NombreCompleto)
                $("#apt_customer_id").val(customer.Id)

                $("#atp_validate_client_view").addClass("hide")
                $("#atp_client_view").removeClass("hide")
                $("#btn_proceder_enlace").removeClass("hide")
            }
            else
            {
                $("#txtNombrevalidar").val(customer.Nombre)
                $("#txtApelPatvalidar").val(customer.ApelPat)
                $("#txtApelMatvalidar").val(customer.ApelMat)
                $("#txtNroDocvalidar").val(customer.NroDoc)

                $("#modal_atp_nuevo_cliente").modal("show")

                toastr.error(response.mensaje, "Mensaje Servidor")
            }
        },
        complete: function ()
        {
            $.LoadingOverlay("hide")
        }
    })
}

// Guardar Nuevo Campania Cliente
function guardarNuevoCampaniaCliente()
{
    var sala_id = $("#campaniaidsalaid").val()
    var urlenvio = ""
    var lugar = ""
    var accion = ""

    lugar = "CampaniaCliente/CampaniaGuardarClienteJson"
    accion = "NUEVO CLIENTE";
    urlenvio = basePath + "CampaniaCliente/CampaniaGuardarClienteJson"

    var dataForm = $('#form_registro_clientevalidar').serializeFormJSON()

    $.when(dataAuditoria(1, "#form_registro_clientevalidar", 3, lugar, accion)).then(function (response, textStatus) {
        $.ajax({
            url: urlenvio,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(dataForm),
            beforeSend: function ()
            {
                $.LoadingOverlay("show")
            },
            success: function (response)
            {
                if (response.respuesta)
                {
                    if (response.idcliente.Id > 0)
                    {
                        $("#apt_customer_id").val(response.idcliente.Id)
                        $("#atp_document_mumber").val(response.idcliente.NroDoc)
                        $("#atp_customer_fullname").val(response.idcliente.NombreCompleto)

                        $('#txtNombrevalidar').val('')
                        $('#txtApelPatvalidar').val('')
                        $('#txtApelMatvalidar').val('')
                        $('#txtNroDocvalidar').val('')
                        $('#txtCelular1validar').val('')
                        $('#txtFechaNacimientovalidar').data("DateTimePicker").setDate(moment())
                        $('#txtemailvalidar').val('')

                        $("#atp_validate_client_view").addClass("hide")
                        $("#atp_client_view").removeClass("hide")
                        $("#btn_proceder_enlace").removeClass("hide")

                        $("#modal_atp_nuevo_cliente").modal("hide")

                        toastr.success("Cliente Registrado", "Mensaje Servidor")
                    }
                    else
                    {
                        toastr.error(response.mensaje, "Mensaje Servidor")
                    }
                }
                else
                {
                    toastr.error(response.mensaje, "Mensaje Servidor")
                }
            },
            complete: function ()
            {
                $.LoadingOverlay("hide")
            }
        })
    })
}

// document ready
$(document).ready(function ()
{
    // obtener salas asignadas
    obtenerListaSalas().then(response => {
        if (response.data)
        {
            renderSelectSalasAudiPromo(response.data)
        }
    })

    // datetimepicker
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow
    })

    // on change select sala
    $(document).on('change', '#cboSalaAudiPromo', function (event) {
        event.preventDefault()

        renderAuditoriaPromocion([])
    })

    // on change fecha inicio
    $(document).on('dp.change', '#fromDateAudiPromo', function (event) {
        var date = new Date(event.date)

        $('#toDateAudiPromo').data("DateTimePicker").setMinDate(date)
    })

    // on change fecha fin
    $(document).on('dp.change', '#toDateAudiPromo', function (event) {
        var date = new Date(event.date)

        $('#fromDateAudiPromo').data("DateTimePicker").setMaxDate(date)
    })

    // on click ATP search
    $(document).on('click', '.btn_audipromo_search', function (event) {
        event.preventDefault()

        var room = $('#cboSalaAudiPromo').val()
        var fromDate = $('#fromDateAudiPromo').val()
        var toDate = $('#toDateAudiPromo').val()

        toastr.clear()

        if (!room) {
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

        if (gapTwoDates(fromDate, toDate) >= 15) {
            toastr.warning("Por favor, seleccione fechas de 15 días de diferencia")

            return false
        }

        var args = {
            room,
            fromDate,
            toDate
        }

        listarAuditoriaPromocion(args)

    })

    // on click ATP Generados en Caja
    $(document).on('click', '.atp_generados_caja', function (event) {
        event.preventDefault()

        var transactionDate = $(this).attr('data-transactiondate')

        toastr.clear()

        if (!transactionDate) {
            toastr.warning('Seleccione una fecha de operación')

            return false
        }

        openATPGeneradosCaja(transactionDate)
    })

    // on click ATP Enlazados en Caja
    $(document).on('click', '.atp_enlazados_ias', function (event) {
        event.preventDefault()

        var transactionDate = $(this).attr('data-transactiondate')

        toastr.clear()

        if (!transactionDate) {
            toastr.warning('Seleccione una fecha de operación')

            return false
        }

        openATPEnlazadosIAS(transactionDate)
    })

    // on click ATP Diferencia
    $(document).on('click', '.atp_diferencia', function (event) {
        event.preventDefault()

        var transactionDate = $(this).attr('data-transactiondate')

        toastr.clear()

        if (!transactionDate) {
            toastr.warning('Seleccione una fecha de operación')

            return false
        }

        openATPDiferencia(transactionDate)
    })

    // on click ATP Enlazar TITOS
    $(document).on('click', '.btn_enlazar_titos', function (event) {
        event.preventDefault()

        getATPEnlazarTickets()
    })

    // on click ATP Proceder Enlazar TITOS
    $(document).on('click', '.btn_proceder_enlace', function (event) {
        event.preventDefault()

        var campania_id = $('#atp_campanias_activas').val()
        var customer_id = $('#apt_customer_id').val()

        toastr.clear()

        if (!campania_id) {
            toastr.warning('Seleccione una campaña')

            return false
        }

        if (!customer_id) {
            toastr.warning('Seleccione un cliente')

            return false
        }

        $.confirm({
            title: '¿Está seguro de Proceder con el Enlace?',
            content: '',
            confirmButton: 'SI',
            cancelButton: 'NO',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function ()
            {
                initProcederEnlaceTITOs(campania_id, customer_id)
            },
            cancel: function ()
            {
                //TODO
            }
        })
    })

    // on ifChecked All
    $(document).on('ifChecked', 'input.icheck_all[type="checkbox"]', function (event) {
        event.preventDefault()

        var table = $("#tableATPDiferencias").DataTable()

        table.rows({ 'search': 'applied' }).every(function () {
            var node = this.node()
            var element = $(node).find(`input.icheck_single[type="checkbox"]`)

            if (element.is(":not(:checked)"))
            {
                var transactionDate = element.attr('data-transactiondate')
                var item = element.attr('data-item')
                var nroticket = element.attr('data-nroticket')
                var amount = element.attr('data-amount')

                dataTitos.pushTickets(transactionDate, item, nroticket, amount)

                $(node).addClass('selected')
                element.prop('checked', true).iCheck('update')
            }
        })

    })

    // on ifUnchecked All
    $(document).on('ifUnchecked', 'input.icheck_all[type="checkbox"]', function (event) {
        event.preventDefault()

        var table = $("#tableATPDiferencias").DataTable()

        table.rows().every(function () {
            var node = this.node()
            var element = $(node).find(`input.icheck_single[type="checkbox"]`)

            if (element.is(":checked"))
            {
                var item = element.attr('data-item')
                var nroticket = element.attr('data-nroticket')
                var amount = element.attr('data-amount')

                dataTitos.removeTicket(item, nroticket, amount)

                $(node).removeClass('selected')
                element.prop('checked', false).iCheck('update')
            }
        })
    })

    // on ifChecked Single
    $(document).on('ifChecked', 'input.icheck_single[type="checkbox"]', function (event) {

        var element = $(event.target)
        var transactionDate = element.attr('data-transactiondate')
        var item = element.attr('data-item')
        var nroticket = element.attr('data-nroticket')
        var amount = element.attr('data-amount')

        dataTitos.pushTickets(transactionDate, item, nroticket, amount)

        $(element).parents('tr').addClass('selected')
    })

    // on ifUnchecked Single
    $(document).on('ifUnchecked', 'input.icheck_single[type="checkbox"]', function (event) {

        var element = $(event.target)
        var item = element.attr('data-item')
        var nroticket = element.attr('data-nroticket')
        var amount = element.attr('data-amount')

        dataTitos.removeTicket(item, nroticket, amount)

        $(element).parents('tr').removeClass('selected')
    })

    // on click ATP Excel
    $(document).on('click', '.btn_audipromo_excel', function (event) {
        event.preventDefault()

        var room = $('#cboSalaAudiPromo').val()
        var fromDate = $('#fromDateAudiPromo').val()
        var toDate = $('#toDateAudiPromo').val()

        toastr.clear()

        if (!room) {
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

        if (gapTwoDates(fromDate, toDate) >= 15) {
            toastr.warning("Por favor, seleccione fechas de 15 días de diferencia")

            return false
        }

        var args = {
            room,
            fromDate,
            toDate
        }

        excelAuditoriaPromocion(args)
    })

    // on click validate client
    $(document).on('click', '.button_validate_client', function (event) {
        event.preventDefault()

        var documentNumber = $("#documentNumber").val()

        validateCustomer(documentNumber)
    })

    // on keyup document number
    $(document).on('keypress', '#documentNumber', function (event) {
        var key = event.keyCode || event.which

        if (key == 13)
        {
            var documentNumber = $(this).val()

            validateCustomer(documentNumber)
        }
    })

    // validate form nuevo cliente
    $("#form_registro_clientevalidar").bootstrapValidator({
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            Nombre: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            ApelPat: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            ApelMat: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            // Celular1: {
            //     validators: {
            //         notEmpty: {
            //             message: 'Ingrese Telefono, Obligatorio'
            //         }
            //     }
            // },
            NroDoc: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            FechaNacimiento: {
                validators: {
                    notEmpty: {
                        message: ' '
                    },
                    date: {
                        format: 'DD/MM/YYYY',
                        message: ' '
                    }
                }
            },
        }
    }).on('success.field.bv', function (e, data) {
        e.preventDefault()

        var $parent = data.element.parents('.form-group')

        // Remove the has-success class
        $parent.removeClass('has-success')

        // Hide the success icon
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide()
    })

    // Guardar Nuevo Cliente
    $('.btnGuardarclientenuevoValidar').on('click', function (event) {
        event.preventDefault()

        $("#form_registro_clientevalidar").data('bootstrapValidator').resetForm()
        var validar = $("#form_registro_clientevalidar").data('bootstrapValidator').validate()

        if (validar.isValid())
        {
            guardarNuevoCampaniaCliente()
        }
    })
})
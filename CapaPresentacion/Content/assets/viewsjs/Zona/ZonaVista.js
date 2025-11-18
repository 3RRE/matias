// ZonaVista Js
function listarSalas(element, modal, isModify = false, salaId = -1)
{
    $.ajax({
        type: "POST",
        url: `${basePath}Sala/ListadoSalaPorUsuarioJson`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        cache: false,
        beforeSend: function ()
        {
            element.html(`<option value="">--Seleccione una opción--</option>`)

            $.LoadingOverlay("show")
        },
        success: function (response)
        {
            var items = response.data

            $.each(items, function (index, item)
            {
                element.append(`<option value="${item.CodSala}" selected>${item.Nombre}</option>`)
            })
        },
        complete: function ()
        {
            element.select2({
                multiple: false,
                placeholder: "--Seleccione una opción--",
                allowClear: true,
                width: "100%",
                dropdownParent: modal
            });

            element.val(null).trigger("change")

            if (isModify) {
                element.val(salaId).trigger("change")
            }

            $.LoadingOverlay("hide")
        }
    })
}

function listarZonas()
{
    $.ajax({
        type: "POST",
        url: `${basePath}Zona/ListarZona`,
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
            if (response.status)
            {
                var data = response.data

                renderTablaZona(data)
            }
            else
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

function renderTablaZona(data)
{
    $("#tableZone").DataTable({
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
                data: null, title: "#", render: function (data, type, row, meta) {
                    return meta.row + 1
                }
            },
            {
                data: "Nombre", title: "Nombre"
            },
            {
                data: "SalaNombre", title: "Sala"
            },
            {
                data: "Estado", title: "Estado", render: function (data, type, row) {
                    var checked = data ? 'checked' : ''

                    return `
                    <div class="toggle-custom">
                        <input type="checkbox" class="toggle_status" data-id="${row.Id}" ${checked} />
                    </div>
                    `
                }
            },
            {
                data: "Id", title: "Acciones",
                "render": function (data) {
                    return `
                    <button type="button" class="btn btn-xs btn-warning btn_editar_zona" title="Modificar" data-id="${data}">
                        <i class="glyphicon glyphicon-pencil"></i>
                    </button>
                    `
                },
                orderable: false,
                searchable: false
            }
        ],
        columnDefs: [
            {
                targets: '_all',
                className: 'text-center'
            }
        ],
        drawCallback: function () {
            $('.toggle_status').bootstrapToggle({
                size: 'small',
                on: 'Activo',
                off: 'Inactivo'
            })
        }
    })
}

function ObtenerZona(zonaId)
{
    return $.ajax({
        type: "POST",
        url: `${basePath}Zona/ObtenerZona`,
        data: JSON.stringify({ zonaId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function ()
        {
            $.LoadingOverlay("show")
        },
        complete: function ()
        {
            $.LoadingOverlay("hide")
        }
    })
}

function setDataFormZona(data)
{
    var elementId = $('#u_zona_id')
    var elementName = $('#u_zona_nombre')
    var elementStatus = $('#u_zona_estado')

    elementId.val(data.Id)
    elementName.val(data.Nombre)
    elementStatus.html(`
    <option value="1" ${data.Estado ? 'selected' : ''}>Activo</option>
    <option value="0" ${!data.Estado ? 'selected' : ''}>Inactivo</option>
    `)
}

function resetFormAgregarZona()
{
    var elementId = $('#c_sala_id')
    var elementName = $('#c_zona_nombre')
    var elementStatus = $('#c_zona_estado')

    elementId.val(null)
    elementName.val(null)
    elementStatus.val(1)
}

function GuardarZona(params)
{
    var data = {
        SalaId: params.SalaId,
        Nombre: params.Nombre,
        Estado: params.Estado
    }

    $.ajax({
        type: "POST",
        url: `${basePath}Zona/GuardarZona`,
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
            if (response.status)
            {
                listarZonas()
                resetFormAgregarZona()

                $('#modal_agregar_zona').modal('hide')
                toastr.success(response.message, 'Mensaje Servidor')
            }
        },
        complete: function ()
        {
            $.LoadingOverlay("hide")
        }
    })
}

function ActualizarZona(params)
{
    var data = {
        Id: params.Id,
        SalaId: params.SalaId,
        Nombre: params.Nombre,
        Estado: params.Estado
    }

    $.ajax({
        type: "POST",
        url: `${basePath}Zona/ActualizarZona`,
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
            if (response.status)
            {
                listarZonas()

                $('#modal_editar_zona').modal('hide')
                toastr.success(response.message, 'Mensaje Servidor')
            }
        },
        complete: function ()
        {
            $.LoadingOverlay("hide")
        }
    })
}

function ActualizarEstadoZona(params)
{
    var data = {
        estado: params.Estado,
        zonaId: params.Id
    }

    $.ajax({
        type: "POST",
        url: `${basePath}Zona/ActualizarEstadoZona`,
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
            if (response.status)
            {
                toastr.success(response.message, 'Mensaje Servidor')
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
    listarZonas()

    $(document).on('click', '.btn_agregar_zona', function (event) {
        event.preventDefault()

        var elementSala = $('#c_sala_id')
        var elementModal = $('#modal_agregar_zona')

        listarSalas(elementSala, elementModal)
        elementModal.modal('show')
    })

    $(document).on('click', '.btn_editar_zona', function (event) {
        event.preventDefault()

        var zonaId = $(this).attr('data-id')
        var elementSala = $('#u_sala_id')
        var elementModal = $('#modal_editar_zona')

        ObtenerZona(zonaId).done(function (response) {
            if (response.status)
            {
                var data = response.data

                listarSalas(elementSala, elementModal, true, data.SalaId)
                setDataFormZona(data)
            }
        }).then(function () {
            elementModal.modal('show')
        })
    })

    $(document).on('click', '#btn_guardar_zona', function (event) {
        event.preventDefault()

        var salaId = $('#c_sala_id').val()
        var zonaNombre = $('#c_zona_nombre').val()
        var zonaEstado = $('#c_zona_estado').val()

        toastr.clear()

        if (!salaId) {
            toastr.warning('Seleccione una Sala')

            return false
        }

        if (!zonaNombre) {
            toastr.warning('Ingrese un Nombre')

            return false
        }

        if (!zonaEstado) {
            toastr.warning('Seleccione un Estado')

            return false
        }

        var args = {
            SalaId: salaId,
            Nombre: zonaNombre,
            Estado: zonaEstado
        }

        GuardarZona(args)
    })

    $(document).on('click', '#btn_actualizar_zona', function (event) {
        event.preventDefault()

        var zonaId = $('#u_zona_id').val()
        var salaId = $('#u_sala_id').val()
        var zonaNombre = $('#u_zona_nombre').val()
        var zonaEstado = $('#u_zona_estado').val()

        toastr.clear()

        if (!zonaId) {
            toastr.warning('Seleccione una Zona')

            return false
        }

        if (!salaId) {
            toastr.warning('Seleccione una Sala')

            return false
        }

        if (!zonaNombre) {
            toastr.warning('Ingrese un Nombre')

            return false
        }

        if (!zonaEstado) {
            toastr.warning('Seleccione un Estado')

            return false
        }

        var args = {
            Id: zonaId,
            SalaId: salaId,
            Nombre: zonaNombre,
            Estado: zonaEstado
        }

        ActualizarZona(args)
    })

    $(document).on('change', '.toggle_status', function (event) {
        event.preventDefault()

        var element = event.target
        var zonaId = $(element).attr('data-id')
        var checked = element.checked

        toastr.clear()

        if (!zonaId) {
            toastr.warning('Seleccione una Zona')

            return false
        }

        var args = {
            Id: zonaId,
            Estado: checked
        }

        ActualizarEstadoZona(args)
    })
})
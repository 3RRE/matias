

function listarCorreosSala() {
    $.ajax({
        type: "POST",
        url: `${basePath}Sala/ListadoCorreosSala`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.status) {
                var data = response.data

                renderTablaCorreoSala(data)
            }
            else {
                toastr.error(response.message, "Mensaje Servidor")
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

function renderTablaCorreoSala(data) {
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
                data: "IdCorreoSala", title: "Id Correo Sala"
            },
            {
                data: "SalaId", title: "SalaId"
            },
            {
                data: "Nombre", title: "Nombre Sala"
            },
           
            {
                data: "Correo", title: "Correo Electrónico"
            },
            {
                data: "Contrasenia", title: "Contraseña"
            },
            {
                data: "IdCorreoSala", title: "Acciones",
                "render": function (data) {
                    console.log(data)
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

function ObtenerdetalleCorreoSala(salaId) {
    console.log(JSON.stringify({ salaId }))
    return $.ajax({
        type: "POST",
        url: `${basePath}Sala/ObtenerdetalleCorreoSala`,
        data: JSON.stringify({ salaId }),
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

function setDataFormZona(data) {
    console.log(data)
    var elementId = $('#salaId')
    var elementCorreoId = $('#idSalaCorreo')
    var elementName = $('#salaNombre')
    var elementCorreo = $('#correo')
    var elementPass = $('#pass')

    elementCorreoId.val(data.IdCorreoSala)
    elementId.val(data.SalaId)
    elementName.val(data.Nombre)
    elementCorreo.val(data.Correo)
    elementPass.val(data.Contrasenia)
}



function ActualizarZona(params) {

    $.ajax({
        type: "POST",
        url: `${basePath}Sala/ActualizarCorreoSala`,
        data: JSON.stringify(params),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.status) {
                listarCorreosSala()

                $('#modal_editar_zona').modal('hide')
                toastr.success(response.message, 'Mensaje Servidor')
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}
// document ready
$(document).ready(function () {
    listarCorreosSala()
    $('#salaNombre').focus(function () {
        $(this).blur();
    });
    $(document).on('click', '.btn_agregar_zona', function (event) {
        event.preventDefault()

        var elementSala = $('#c_sala_id')
        var elementModal = $('#modal_agregar_zona')

        listarSalas(elementSala, elementModal)
        elementModal.modal('show')
    })

    $(document).on('click', '.btn_editar_zona', function (event) {
        event.preventDefault()

        var salaId = $(this).attr('data-id')
        var elementSala = $('#u_sala_id')
        var elementModal = $('#modal_editar_zona')

        ObtenerdetalleCorreoSala(salaId).done(function (response) {
            if (response.status) {
                var data = response.data
                console.log('data')
                console.log(data)
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

        var idSalaCorreo = $('#idSalaCorreo').val()
        var salaId = $('#salaId').val()
        var salaNombre = $('#salaNombre').val()
        var correo = $('#correo').val()
        var pass = $('#pass').val()

        toastr.clear()

        var args = {
            IdCorreoSala: idSalaCorreo,
            Correo: correo,
            SalaId: salaId,
            Nombre: salaNombre,
            Contrasenia: pass
        }
        if (correo != '' || pass != '') {
            ActualizarZona(args)

        }
    })

})
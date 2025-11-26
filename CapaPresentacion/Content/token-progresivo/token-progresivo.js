var TokenProgresivo = {
    isModule: function (module) {
        var response = false
        var moduleName = 'Progresivo'

        if (module === moduleName) {
            response = true
        }

        // free is false
        // response = false

        return response
    },
    isValid: function () {
        $.ajax({
            type: "POST",
            url: `${basePath}SeguridadProgresivo/IsValidToken`,
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                $.LoadingOverlay("show")
            },
            success: function (response) {
                if (!response.status) {
                    TokenProgresivo.getRooms()
                    TokenProgresivo.openModal()
                }
            },
            complete: function () {
                $.LoadingOverlay("hide")
            }
        })
    },
    openModal: function () {
        $('#modalTokenProgresivo').modal('show')
    },
    closeModal: function () {
        $('#modalTokenProgresivo').modal('hide')
    },
    validate: function (token) {
        $.ajax({
            type: "POST",
            url: `${basePath}SeguridadProgresivo/ValidateToken`,
            data: JSON.stringify({ token: token }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            cache: false,
            beforeSend: function () {
                toastr.clear()
                $.LoadingOverlay("show")
            },
            success: function (response) {
                if (response.status) {
                    TokenProgresivo.closeModal()
                    $('#token_progresivo').val("")
                    toastr.success(response.message, "Mensaje Servidor", { timeOut: 0, closeButton: true, positionClass: "toast-bottom-left", extendedTimeOut: 0 })
                } else {
                    toastr.error(response.message, "Mensaje Servidor", { timeOut: 0, closeButton: true, positionClass: "toast-bottom-left", extendedTimeOut: 0 })
                }
            },
            complete: function () {
                $.LoadingOverlay("hide")
            }
        })
    },
    getRooms: function () {
        var element = $("#cboRoomsInProgresivo")

        $.ajax({
            type: "POST",
            url: `${basePath}Sala/ListadoSalaPorUsuarioJson`,
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
                element.html(`<option value="">--Seleccione una Sala--</option>`)
                $.LoadingOverlay("show")
            },
            success: function (response) {
                var items = response.data

                $.each(items, function (index, item) {
                    element.append(`<option value="${item.CodSala}">${item.Nombre}</option>`)
                })
            },
            complete: function () {
                $.LoadingOverlay("hide")
            }
        })
    },
    request: function (room) {
        toastr.clear()
        chat.server.pedirToken(room)

        toastr.success('Solicitud de Token enviada, espere por favor', 'Mensaje Servidor', { timeOut: 0, closeButton: true, positionClass: "toast-bottom-left", extendedTimeOut: 0 })
    },
    see: function () {
        toastr.clear()
        chat.server.verToken()
    }
}

$(document).ready(function () {
    // request token progresivo
    $(document).on('click', '#buttonRequestTokenProgresivo', function (event) {
        event.preventDefault()

        var room = $('#cboRoomsInProgresivo').val()

        if (!room) {
            toastr.clear()
            toastr.warning('Seleccione una Sala', 'Mensaje Servidor', { timeOut: 0, closeButton: true, positionClass: "toast-bottom-left", extendedTimeOut: 0 })

            return false
        }

        TokenProgresivo.request(room)
    })

    // validate token progresivo
    $(document).on('click', '#buttonValidateTokenProgresivo', function (event) {
        event.preventDefault()

        var token = $('#token_progresivo').val()

        if (!token) {
            toastr.clear()
            toastr.warning('Ingrese Token', 'Mensaje Servidor', { timeOut: 0, closeButton: true, positionClass: "toast-bottom-left", extendedTimeOut: 0 })

            return false
        }

        TokenProgresivo.validate(token)
    })

    // view token
    $(document).on('click', '#buttonViewTokenProgresivo', function (event) {
        event.preventDefault()

        TokenProgresivo.see()
    })

    // select2 rooms
    $("#cboRoomsInProgresivo").select2({
        width: "100%",
        dropdownParent: $("#modalTokenProgresivo")
    })

})
// listar app setttings
function listarAppSettings() {

    renderTableAppSettings([])

    $.ajax({
        type: "POST",
        url: `${basePath}/Settings/ListarAppSettings`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.success) {
                renderTableAppSettings(response.data)
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

// render table app settings
function renderTableAppSettings(items) {
    $("#tableAppSettings").DataTable({
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
                data: "AS_Key",
                title: "Llave"
            },
            {
                data: "AS_Value",
                title: "Valor"
            },
            {
                data: "AS_Description",
                title: "Descripción"
            },
            {
                data: "AS_Updated",
                title: "Actualización",
                render: function (value) {
                    var updateDate = ""
                    var dateFormat = "DD/MM/YYYY HH:mm:ss"
                    var date = moment(value).format(dateFormat)

                    var noDates = [
                        '31/12/1752',
                        '01/01/1753'
                    ]

                    if (value && moment(date, dateFormat, true).isValid()) {
                        updateDate = date

                        if (noDates.includes(moment(value).format("DD/MM/YYYY"))) {
                            updateDate = '---'
                        }
                    }

                    return updateDate
                }
            },
            {
                data: null,
                title: "Acciones",
                render: function (item) {
                    return `
                    <button type="button" class="btn btn-xs btn-warning button_edit_setting" title="Modificar" data-key="${item.AS_Key}">
                        <i class="glyphicon glyphicon-pencil"></i>
                    </button>
                    <button type="button" class="btn btn-xs btn-danger button_delete_setting" title="Eliminar" data-key="${item.AS_Key}">
                        <i class="glyphicon glyphicon-trash"></i>
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
        ]
    })
}

// obtener app settings
function obtenerAppSetting(asKey) {
    return $.ajax({
        type: "POST",
        url: `${basePath}/Settings/ObtenerAppSetting`,
        data: JSON.stringify({ asKey }),
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

// render form edit settings
function renderFormEditSetting(item) {
    var UAS_Key = $('#UAS_Key')
    var UAS_Value = $('#UAS_Value')
    var UAS_Description = $('#UAS_Description')

    UAS_Key.val(item.AS_Key)
    UAS_Value.val(item.AS_Value)
    UAS_Description.val(item.AS_Description)
}

// clear form settings
function resetFormAddSetting() {
    var SAS_Key = $('#SAS_Key')
    var SAS_Value = $('#SAS_Value')
    var SAS_Description = $('#SAS_Description')

    SAS_Key.val(null)
    SAS_Value.val(null)
    SAS_Description.val(null)
}

function resetFormEditSetting() {
    var UAS_Key = $('#UAS_Key')
    var UAS_Value = $('#UAS_Value')
    var UAS_Description = $('#UAS_Description')

    UAS_Key.val(null)
    UAS_Value.val(null)
    UAS_Description.val(null)
}

// save app setting
function saveAppSetting(params) {
    $.ajax({
        type: "POST",
        url: `${basePath}/Settings/GuardarAppSetting`,
        data: JSON.stringify(params),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.success) {
                listarAppSettings()
                resetFormAddSetting()

                $('#modal_add_setting').modal('hide')
                toastr.success(response.message)
            } else {
                toastr.error(response.message)
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

// update app setting
function updateAppSetting(params) {
    $.ajax({
        type: "POST",
        url: `${basePath}/Settings/ActualizarAppSetting`,
        data: JSON.stringify(params),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.success) {
                listarAppSettings()

                $('#modal_edit_setting').modal('hide')
                toastr.success(response.message)
            } else {
                toastr.error(response.message)
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

// delete app setting
function deleteAppSetting(asKey) {
    $.ajax({
        type: "POST",
        url: `${basePath}/Settings/EliminarAppSetting`,
        data: JSON.stringify({ asKey }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.success) {
                listarAppSettings()

                toastr.success(response.message)
            } else {
                toastr.error(response.message)
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}

// document ready
$(document).ready(function () {

    // listar app settings
    listarAppSettings()

    // on click add setting
    $(document).on('click', '.button_add_setting', function (event) {
        event.preventDefault()

        var elementModal = $('#modal_add_setting')

        elementModal.modal('show')
    })

    // on click edit setting
    $(document).on('click', '.button_edit_setting', function (event) {
        event.preventDefault()

        var asKey = $(this).attr('data-key')
        var elementModal = $('#modal_edit_setting')

        if (!asKey) {
            toastr.warning('Seleccione una llave')

            return false
        }

        resetFormEditSetting()

        obtenerAppSetting(asKey).done(function (response) {
            if (response.success) {
                renderFormEditSetting(response.data)
            }
        }).then(function () {
            elementModal.modal('show')
        })
    })

    // on click store setting
    $(document).on('click', '#button_store_setting', function (event) {
        event.preventDefault()

        var SAS_Key = $('#SAS_Key').val()
        var SAS_Value = $('#SAS_Value').val()
        var SAS_Description = $('#SAS_Description').val()

        toastr.remove()

        if (!SAS_Key) {
            toastr.warning('Ingrese una llave')

            return false
        }

        if (!SAS_Value) {
            toastr.warning('Ingrese un valor')

            return false
        }

        if (!SAS_Description) {
            toastr.warning('Ingrese una descripción')

            return false
        }

        var args = {
            AS_Key: SAS_Key,
            AS_Value: SAS_Value,
            AS_Description: SAS_Description
        }

        saveAppSetting(args)
    })

    // on click update setting
    $(document).on('click', '#button_update_setting', function (event) {
        event.preventDefault()

        var UAS_Key = $('#UAS_Key').val()
        var UAS_Value = $('#UAS_Value').val()
        var UAS_Description = $('#UAS_Description').val()

        toastr.remove()

        if (!UAS_Key) {
            toastr.warning('Ingrese una llave')

            return false
        }

        if (!UAS_Value) {
            toastr.warning('Ingrese un valor')

            return false
        }

        if (!UAS_Description) {
            toastr.warning('Ingrese una descripción')

            return false
        }

        var args = {
            AS_Key: UAS_Key,
            AS_Value: UAS_Value,
            AS_Description: UAS_Description
        }

        updateAppSetting(args)
    })

    // on click delete setting
    $(document).on('click', '.button_delete_setting', function (event) {
        event.preventDefault()

        var asKey = $(this).attr('data-key')

        if (!asKey) {
            toastr.warning('Seleccione una llave')

            return false
        }

        $.confirm({
            title: `Hola`,
            content: `¿Estás seguro de eliminar la configuración <b>${asKey}</b>?`,
            confirmButton: 'Sí, eliminar',
            cancelButton: 'Cancelar',
            confirmButtonClass: 'btn-success',
            confirm: function () {
                deleteAppSetting(asKey)
            },
            cancel: function () {}
        })
    })
})
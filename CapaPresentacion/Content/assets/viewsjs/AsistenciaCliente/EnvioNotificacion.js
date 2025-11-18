const btnSearch = $("#btnBuscar");
const txtNumeroDocumento = $("#numeroDocumento");
const txtNombres = $("#nombres");
const txtApellidoPaterno = $("#apellidoPaterno");
const txtApellidoMaterno = $("#apellidoMaterno");
const inputs = $('#numeroDocumento, #nombres, #apellidoPaterno, #apellidoMaterno');
const NOTIFICATION_TYPE = {
    whatsapp: 1,
    sms: 2,
    email: 3,
    llamada: 4
}

$(document).ready(() => {
    createDataTable([]);
    txtNumeroDocumento.on('input', toggleFields);
    txtNombres.on('input', toggleFields);
    txtApellidoPaterno.on('input', toggleFields);
    txtApellidoMaterno.on('input', toggleFields);
    inputs.on('keydown', function (event) {
        handleEnterKey(event);
    });
})

btnSearch.on('click', function () {
    const filters = {
        Nombres: txtNombres.val(),
        ApellidoPaterno: txtApellidoPaterno.val(),
        ApellidoMaterno: txtApellidoMaterno.val(),
        NumeroDocumento: txtNumeroDocumento.val(),
    }

    const validationResult = validateFilters(filters);
    if (!validationResult.isValid) {
        toastr.warning(validationResult.message);
        return;
    }

    getClients(filters);
});

const getClients = (filters) => {
    const url = `${basePath}AsistenciaCliente/ObtenerClientesParaEnvioNotificacion`;
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(filters),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            createDataTable(response.data)
            if (!response.success) {
                toastr.error(response.displayMessage, "Mensaje Servidor")
                return;
            }
        }
    });
};

const updateSentNotification = (data) => {
    const url = `${basePath}AsistenciaCliente/ActualizarEnvioNotificacionCliente`;
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(data),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            if (response.success) {
                toastr.success(response.displayMessage)
            } else {
                toastr.error(response.displayMessage)
            }
        }
    });
};

const createDataTable = (data) => {
    objetodatatable = $("#table").DataTable({
        bDestroy: true,
        bSort: true,
        scrollCollapse: true,
        scrollX: true,
        paging: true,
        autoWidth: true,
        bProcessing: true,
        bDeferRender: true,
        columnDefs: [
            {
                targets: "_all",
                className: "text-center"
            }
        ],
        data: data,
        columns: [
            {
                data: null, sortable: false, title: `<i class="glyphicon glyphicon-check"></i> WSP`,
                render: function (row) {
                    const tieneObservacion = row.EsLudopata || row.EsProhibido || row.EsRobaStacker;
                    const isChecked = row.EnviaNotificacionWhatsapp ? 'checked' : '';
                    const check = tieneObservacion ?
                        `<input type="checkbox" name="radioNotificacionWhatsapp" class="icheck_single" disabled=true>` :
                        `<input type="checkbox" name="radioNotificacionWhatsapp" class="icheck_single" data-id-cliente="${row.IdCliente}" data-cod-sala="${row.codSala}" data-tipo-notificacion=${NOTIFICATION_TYPE.whatsapp} ${isChecked}>`
                    return check;
                }
            },
            {
                data: null, sortable: false, title: `<i class="glyphicon glyphicon-check"></i> SMS`,
                render: function (row) {
                    const tieneObservacion = row.EsLudopata || row.EsProhibido || row.EsRobaStacker;
                    const isChecked = row.EnviaNotificacionSms ? 'checked' : '';
                    const check = tieneObservacion ?
                        `<input type="checkbox" name="radioNotificacionSms" class="icheck_single" disabled=true>` :
                        `<input type="checkbox" name="radioNotificacionSms" class="icheck_single" data-id-cliente="${row.IdCliente}" data-cod-sala="${row.codSala}" data-tipo-notificacion=${NOTIFICATION_TYPE.sms} ${isChecked}>`
                    return check;
                }
            },
            {
                data: null, sortable: false, title: `<i class="glyphicon glyphicon-check"></i> LLAMADA`,
                render: function (row) {
                    const tieneObservacion = row.EsLudopata || row.EsProhibido || row.EsRobaStacker;
                    const isChecked = row.LlamadaCelular ? 'checked' : '';
                    const check = tieneObservacion ?
                        `<input type="checkbox" name="radioNotificacionLlamada" class="icheck_single" disabled=true>` :
                        `<input type="checkbox" name="radioNotificacionLlamada" class="icheck_single" data-id-cliente="${row.IdCliente}" data-cod-sala="${row.codSala}" data-tipo-notificacion=${NOTIFICATION_TYPE.llamada} ${isChecked}>`
                    return check;
                }
            },
            {
                data: null, sortable: false, title: `<i class="glyphicon glyphicon-check"></i> EMAIL`,
                render: function (row) {
                    const tieneObservacion = row.EsLudopata || row.EsProhibido || row.EsRobaStacker;
                    const isChecked = row.EnviaNotificacionEmail ? 'checked' : '';
                    const check = tieneObservacion ?
                        `<input type="checkbox" name="radioNotificacionEmail" class="icheck_single" disabled=true>` :
                        `<input type="checkbox" name="radioNotificacionEmail" class="icheck_single" data-id-cliente="${row.IdCliente}" data-cod-sala="${row.codSala}" data-tipo-notificacion=${NOTIFICATION_TYPE.email} ${isChecked}>`
                    return check;
                }
            },
            { data: "codSala", title: "Cod. Sala" },
            { data: "NombreSala", title: "Sala" },
            { data: "NroDoc", title: "Número Documento" },
            { data: "NombreCliente", title: "Cliente" },
            { data: "TipoRegistro", title: "Tipo de Registro" },
            {
                data: null, title: "Observación",
                render: function (row) {
                    const tieneObservacion = row.EsLudopata || row.EsProhibido || row.EsRobaStacker;
                    const badgeLudopata = row.EsLudopata ? `<span class="badge badge-ludopata">Ludopata</span>` : '';
                    const badgeProhibido = row.EsProhibido ? `<span class="badge badge-prohibido">Prohibido</span>` : '';
                    const badgeRobaStacker = row.EsRobaStacker ? `<span class="badge badge-roba-stacker">Roba Stacker</span>` : '';
                    return tieneObservacion ? `${badgeLudopata}${badgeProhibido}${badgeRobaStacker}` : '-';
                },
            },
        ],
        drawCallback: function () {
            setICheck();
        }
    });
}

// on check
$(document).on('ifChecked', 'input.icheck_single[type="checkbox"]', function (event) {
    const element = $(event.target)
    const data = {
        idCliente: element.data("id-cliente"),
        codSala: element.data("cod-sala"),
        tipoNotificacion: Number.parseInt(element.data("tipo-notificacion")),
        enviaNotificacion: true
    }
    updateSentNotification(data);
})

// on uncheck
$(document).on('ifUnchecked', 'input.icheck_single[type="checkbox"]', function (event) {
    const element = $(event.target)
    const data = {
        idCliente: element.data("id-cliente"),
        codSala: element.data("cod-sala"),
        tipoNotificacion: Number.parseInt(element.data("tipo-notificacion")),
        enviaNotificacion: false
    }
    updateSentNotification(data);
})

const setICheck = () => {
    const configICheckWhatsapp = {
        checkboxClass: 'icheckbox_square-green icheckbox_bg-white',
        increaseArea: '20%',
        disabledClass: 'check-disabled'
    }

    const configICheckSms = {
        checkboxClass: 'icheckbox_square-blue icheckbox_bg-white',
        increaseArea: '20%',
        disabledClass: 'check-disabled'
    }

    const configICheckLlamada = {
        checkboxClass: 'icheckbox_square-yellow icheckbox_bg-white',
        increaseArea: '20%',
        disabledClass: 'check-disabled'
    }

    const configICheckEmail = {
        checkboxClass: 'icheckbox_square-orange icheckbox_bg-white',
        increaseArea: '20%',
        disabledClass: 'check-disabled'
    }
    $('input[type=checkbox][name=radioNotificacionWhatsapp]').iCheck(configICheckWhatsapp);
    $('input[type=checkbox][name=radioNotificacionSms]').iCheck(configICheckSms);
    $('input[type=checkbox][name=radioNotificacionLlamada]').iCheck(configICheckLlamada);
    $('input[type=checkbox][name=radioNotificacionEmail]').iCheck(configICheckEmail);
}

const validateFilters = (filters) => {
    const { Nombres, ApellidoPaterno, ApellidoMaterno, NumeroDocumento } = filters;

    if (NumeroDocumento) {
        if (Nombres || ApellidoPaterno || ApellidoMaterno) {
            return {
                isValid: false,
                message: "Si se proporciona un número de documento, el resto de los campos debe estar en blanco."
            };
        }
        return { isValid: true, message: "Filtros válidos." };
    }

    if (!Nombres || (!ApellidoPaterno && !ApellidoMaterno)) {
        return {
            isValid: false,
            message: "Si no se proporciona un número de documento, se deben incluir al menos el nombre y uno de los apellidos."
        };
    }

    return { isValid: true, message: "Filtros válidos." };
}

const handleEnterKey = (event) => {
    if (event.keyCode === 13) {
        event.preventDefault();
        btnSearch.click();
    }
};

const toggleFields = () => {
    const isNombreFilled =
        txtNombres.val().trim() !== '' ||
        txtApellidoPaterno.val().trim() !== '' ||
        txtApellidoMaterno.val().trim() !== '';

    if (txtNumeroDocumento.val().trim() !== '') {
        txtNombres.prop('disabled', true).val('');
        txtApellidoPaterno.prop('disabled', true).val('');
        txtApellidoMaterno.prop('disabled', true).val('');
    } else if (isNombreFilled) {
        txtNumeroDocumento.prop('disabled', true).val('');
    } else {
        txtNumeroDocumento.prop('disabled', false);
        txtNombres.prop('disabled', false);
        txtApellidoPaterno.prop('disabled', false);
        txtApellidoMaterno.prop('disabled', false);
    }
}
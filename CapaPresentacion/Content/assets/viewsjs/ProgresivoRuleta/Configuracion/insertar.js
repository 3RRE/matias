const form = $("#formRuletaProgresivoConfiguracion");
const btnGuardarModal = $("#btnGuardar");
const cboSala = $("#cboSalaUser");
let instancePosiciones;
let instanceCodMaquinas;

const setDatePicker = () => {
    const now = moment();
    $('.only-time').datetimepicker({
        format: 'HH:mm',          // Solo hora
        stepping: 15,             // Intervalo de 15 minutos
        icons: {
            time: 'fa fa-clock',
            up: 'fa fa-chevron-up',
            down: 'fa fa-chevron-down'
        },
        defaultDate: now,
        useCurrent: true,
        pickDate: false
    });
}

const setTagInput = () => {
    const rebuildTags = (instanceTagInput, items) => {
        instanceTagInput._clean();
        for (var i = 0; i < items.length; i++) {
            instanceTagInput._buildItem(items[i]);
            instanceTagInput._insert(items[i]);
        }
        instanceTagInput.next().find('.close-item').remove();
    }

    $('#SlotHexValues').inputTags({
        minLength: 8,
        maxLength: 8,
        max: 100,
        errors: languajeTagInput,
        init: function ($elem) {
            $elem.next().find('.inputTags-field').attr('name', 'inputTags-maquinas').attr('id', 'input-tag-maquinas');
            instanceCodMaquinas = $elem;
        },
        create: function ($elem) {
            const items = Array.from({ length: $elem.tags.length }, (_, i) => i + 1);
            rebuildTags(instancePosiciones, items);
        },
        destroy: function ($elem) {
            const items = Array.from({ length: $elem.tags.length }, (_, i) => i + 1);
            rebuildTags(instancePosiciones, items);
        },
    });

    $('#SlotHexPositions').inputTags({
        minLength: 1,
        maxLength: 3,
        max: 100,
        errors: languajeTagInput,
        editable: false,
        init: function ($elem) {
            $elem.next().find('.inputTags-field').attr('name', 'inputTags-posiciones').attr('id', 'input-tag-posiciones').attr('readonly', 'true').css({
                'border': 'none !important',
                'outline': 'none !important'
            });
            $elem.next().find('.close-item').remove();
            instancePosiciones = $elem;
        },
        create: function ($elem) {
            $elem.next().find('.close-item').remove();
        },
    });
}

const getSalas = () => {
    $.when(
        llenarSelect(`${basePath}Sala/ListadoSalaPorUsuarioJson`, {}, "cboSalaUser", "CodSala", "Nombre")
    ).then(() => {
        cboSala.select2({
            placeholder: "Seleccione ...",
            multiple: false,
            dropdownParent: $('#modalGroup')
        });
    });
}

const setComboSelect2 = (combo, data = {}) => {
    if (!existCombo(combo)) {
        return;
    }
    combo.select2({
        placeholder: "Seleccione ...",
        multiple: false,
        data: data
    });
}

const existCombo = (combo) => {
    return combo.length;
}

const setValidation = () => {
    form.bootstrapValidator({
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            CodSala: {
                validators: {
                    notEmpty: {
                        message: 'Seleccione una sala.'
                    }
                }
            },
            NombreRuleta: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese un nombre para la ruleta.'
                    }
                }
            },
            TotalSlots: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese la cantidad de maquinas.'
                    },
                    greaterThan: {
                        value: 1,
                        inclusive: true,
                        message: 'Debe ser mayor que 1.'
                    }
                }
            },
            SlotHexValues: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese los códigos de las máquinas.'
                    }
                }
            },
            MinSlotsPlaying: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese el minimo de máquinas.'
                    },
                    greaterThan: {
                        value: 1,
                        inclusive: true,
                        message: 'Debe ser mayor que 1.'
                    }
                }
            },
            CoinInPercent: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese un porcentaje.'
                    },
                    numeric: {
                        message: 'Debe ser un número (se permiten decimales).'
                    },
                    between: {
                        min: 0,
                        max: 1,
                        message: 'Debe estar entre 0 y 1.'
                    }
                }
            },
            MinBet: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese un apuesta minima.'
                    },
                    greaterThan: {
                        value: 1,
                        inclusive: true,
                        message: 'Debe ser mayor que 1.'
                    }
                }
            },
            Ip: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese una direccion IP.'
                    },
                    regexp: {
                        regexp: /^(25[0-5]|2[0-4][0-9]|1?[0-9]{1,2})(\.(25[0-5]|2[0-4][0-9]|1?[0-9]{1,2})){3}$/,
                        message: 'Ingrese una IP válida. Ejemplo: 192.168.1.1'
                    }
                }
            },
            HoraInicio: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese una hora de inicio.'
                    }
                }
            },
            HoraFin: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese una hora fin.'
                    }
                }
            }
        }
    }).on('success.field.bv', function (e, data) {
        e.preventDefault();
        let $parent = data.element.parents('.form-group');
        // Remove the has-success class
        $parent.removeClass('has-success');
        // Hide the success icon
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });
}

const guardarConfiguracion = (data) => {
    let url = `${basePath}PruConfiguraciones/CrearConfiguracion`;
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: function (response) {
            if (!response.success) {
                toastr.error(response.displayMessage);
                return;
            }
            toastr.success(response.displayMessage);
            const filters = {
                codSala: data.CodSala,
            };
            getConfigurations(filters)
            $('#modalGroup').modal('hide');
        }
    });
}

const setEvents = () => {
    btnGuardarModal.off('click').on('click', function (e) {
        form.data('bootstrapValidator').resetForm();
        const validar = form.data('bootstrapValidator').validate();
        if (!validar.isValid()) {
            return;
        }

        let data = form.serializeFormJSON();
        data.StatusOk = parseInt(data.StatusOk);
        const array = Array.from({ length: instanceCodMaquinas.tags.length }, (_, i) => i + 1);
        data.Posiciones = array.join(',');
        guardarConfiguracion(data);
    });

    $('input').not('.inputTags-field').on('keydown', function (e) {
        if (e.keyCode === 13) {
            e.preventDefault();
            btnGuardarModal.click();
        }
    });
}

$(document).ready(() => {
    setDatePicker();
    setTagInput();
    getSalas();
    setValidation();
    setEvents();
})

//combos
const cboSala = $("#cboSala");
const cboTipoCliente = $("#cboTipoCliente");
const cboTipoFrecuencia = $("#cboTipoFrecuencia");
const cboTipoJuego = $("#cboTipoJuego");

//botones
const btnBuscar = $("#btnBuscar");
const btnSeleccionados = $("#btnObtenerSeleccionados");
const btnQuitarImagen = $("#btnQuitarImagen");
const btnSendMessage = $('#btnSendMessage');


//imagen
const inputImage = $('#image');
const imagePreview = $('#imagePreview');
const containerImage = $('#containerImage');

//constantes
const TODOS = '-1';
let anterior = new Array(4);

$.each(anterior, function (index, value) {
    anterior[index] = [];
});

$(document).ready(() => {
    initPopovers();
    removeImage();
    renderTableClient();
    getSalasByUser();
    getClientTypes();
    getFrecuencyTypes();
    getGameTypes(); 
});

//#region Events
$(document).on('click', '.btnEditar', function (e) {
    let id = $(this).data("id");
    let nombre = $(this).data("nombre");
    $("#hddmodal").val(nombre);
    $('#bodyModal').html("");
    $('#default-modal-label').html(nombre);
    let redirectUrl = basePath + "MensajeriaCliente/ActualizarContactoClienteVista/" + id;
    let ubicacion = "bodyModal";
    $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
        $("#modalGroup").modal("show");
    });
});

btnBuscar.on("click", () => {

    if (!cboSala.val() && !cboTipoCliente.val() && !cboTipoFrecuencia.val() && !cboTipoJuego.val()) {
        toastr.info("Seleccione al menos uno de los filtros para realizar la búsqueda");
        return;
    }

    let valuesSalas = cboSala.val() ?? [];
    let valuesTipoCliente = cboTipoCliente.val() ?? [];
    let valuesTipoFrecuencia = cboTipoFrecuencia.val() ?? [];
    let valuesTipoJuego = cboTipoJuego.val() ?? [];

    if (valuesSalas.includes(TODOS) || valuesSalas.length == 0) {
        valuesSalas = getAllValuesOfSelect(cboSala);
        valuesSalas = removeElementOfArray(valuesSalas, TODOS);
    }
    if (valuesTipoCliente.includes(TODOS)) {
        valuesTipoCliente = [];
    }
    if (valuesTipoFrecuencia.includes(TODOS)) {
        valuesTipoFrecuencia = [];
    }
    if (valuesTipoJuego.includes(TODOS)) {
        valuesTipoJuego = [];
    }

    let data = {
        filtroSala: valuesSalas,
        filtroTipoCliente: valuesTipoCliente,
        filtroTipoFrecuencia: valuesTipoFrecuencia,
        filtroTipoJuego: valuesTipoJuego,
    }

    $.ajax({
        type: "POST",
        url: basePath + "MensajeriaCliente/FiltrarClienteJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if (response.success) {
                renderTableClient(response.data);
            } else {
                toastr.error(response.displayMessage);
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
})

btnSeleccionados.on("click", () => {
    let table = $("#tableClient").DataTable()
    let selectedRows = getSelectedRows(table);

    if (selectedRows.length == 0) {
        toastr.info("Seleccione al menos un cliente para enviar mensajes.")
        return;
    }
});

btnQuitarImagen.on("click", () => {
    removeImage();
})

btnSendMessage.on("click", (e) => {
    let message = $('#txtMensaje').val();
    let imageFile = $('#image')[0].files[0];

    if (!message && !imageFile) {
        toastr.info("Ingrese un mensaje o seleccione una imagen.")
        return;
    }

    let table = $("#tableClient").DataTable()
    let clients = getSelectedRows(table);

    if (clients.length == 0) {
        toastr.info("Seleccione al menos un cliente para enviar el mensaje.")
        return;
    }

    let aux = clients.length == 1 ? 'cliente' : 'clientes';

    $.confirm({
        title: 'Confirmación',
        content: `¿Seguro que desea enviar el mensaje a ${clients.length} ${aux}?<br><br>
            <ul>
                <li>Solo se enviará el mensaje a los clientes que tengan registrado el código de país.</li>
                <li>Aquellos que no tengan código de país y comiencen con 9 y tengan 9 digitos se le agregara el código país +51</li>
                <li>Si no tiene número, se tratará de usar el numero alternativo, siempre y cuando cumpla las condiciones anteriores.</li>
            <ul>`,
        confirmButton: 'Sí',
        cancelButton: 'No',
        confirm: function () {
            sendMessage(clients, message);
        },
        cancel: function () {
            toastr.info("Se cancelo el envío del mensaje.");
        }
    });
});

$('#image').change(function () {
    let file = this.files[0];
    if (file) {
        let mixedfile = file['type'].split("/");
        let filetype = mixedfile[0]; // (image, video)
        if (filetype == "image") {
            let reader = new FileReader();
            reader.onload = function (e) {
                showImage();
                imagePreview.show().attr("src", e.target.result);
            }
            reader.readAsDataURL(this.files[0]);
        } else {
            toastr.info("Solo se permiten imagenes con el formato jpg, jpeg, gif, png, webp o bmp");
        }
    }
})

$('input[type="checkbox"]').on('change', (evt) => {
    let element = $(evt.target);
    let isChecked = element.is(':checked');

    let check = element.data("id");

    switch (check) {
        case 'sala': manipulateSelects(cboSala, isChecked); break;
        case 'tipoCliente': manipulateSelects(cboTipoCliente, isChecked); break;
        case 'tipoFrecuencia': manipulateSelects(cboTipoFrecuencia, isChecked); break;
        case 'tipoJuego': manipulateSelects(cboTipoJuego, isChecked); break;
    }
});

$('.select-filter').on("change", function (e) {
    let values = $(this).val();

    if (!values) {
        return;
    }

    let position = $(this).data('position');

    let optionAllSelected = !anterior[position].includes(TODOS) && values.includes(TODOS);
    let gottenAllAndSelectOther = anterior[position].includes(TODOS) && values.includes(TODOS);

    if (optionAllSelected) {
        $(this).val(TODOS);
    }

    if (gottenAllAndSelectOther) {
        values.splice(values.indexOf(TODOS), 1);
        $(this).val(values);
    }

    anterior[position] = values;
});

// on ifChecked All
$(document).on('ifChecked', 'input.icheck_all[type="checkbox"]', function (event) {
    event.preventDefault()

    let table = $("#tableClient").DataTable()

    table.rows({ 'search': 'applied' }).every(function () {
        let node = this.node()
        let element = $(node).find(`input.icheck_single[type="checkbox"]`)

        if (element.is(":not(:checked)")) {
            $(node).addClass('selected')
            element.prop('checked', true).iCheck('update')
        }
    })

})

// on ifUnchecked All
$(document).on('ifUnchecked', 'input.icheck_all[type="checkbox"]', function (event) {
    event.preventDefault()

    let table = $("#tableClient").DataTable()

    table.rows().every(function () {
        let node = this.node()
        let element = $(node).find(`input.icheck_single[type="checkbox"]`)

        if (element.is(":checked")) {
            $(node).removeClass('selected')
            element.prop('checked', false).iCheck('update')
        }
    })
})

// on ifChecked Single
$(document).on('ifChecked', 'input.icheck_single[type="checkbox"]', function (event) {
    let element = $(event.target)
    $(element).parents('tr').addClass('selected')
})

// on ifUnchecked Single
$(document).on('ifUnchecked', 'input.icheck_single[type="checkbox"]', function (event) {
    let element = $(event.target)
    $(element).parents('tr').removeClass('selected')
})
//#endregion

//#region Methods
const sendMessage = (ids, message) => {

    let imageFile = $('#image')[0].files[0];

    let url = `${basePath}MensajeriaCliente/EnviarMensajeMultiple`;

    let formData = new FormData();

    formData.append('image', imageFile);
    formData.append('ids', JSON.stringify(ids));
    formData.append('message', message);
    formData.append('withImage', Boolean(imageFile));

    $.ajax({
        url: url,
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            console.log(response)
            if (response.success) {
                toastr.success(response.displayMessage);
                //resetMessage();
            } else {
                toastr.error(response.displayMessage);
            }
        }
    });
}

const renderTableClient = (data) => {
    let empty = "-----------";
    $("#tableClient").DataTable({
        bDestroy: true,
        bSort: true,
        scrollCollapse: true,
        scrollX: true,
        paging: true,
        autoWidth: true,
        bProcessing: true,
        //"bDeferRender": true,
        aaSorting: [],
        data: data,
        columns: [
            {
                data: null, title: `<input type="checkbox" name="radioCliente" class="icheck_all">`, render: function (data) {
                    return `<input type="checkbox" name="radioCliente" class="icheck_single" data-id="${data.idCliente}" data-cod-sala="${data.idSala}">`
                }
            },
            {
                data: null, title: "Nombre", render: function (data) {

                    let completeName = `${data.nombreCliente} ${data.apellidoPaternoCliente} ${data.apellidoMaternoCliente}`
                    completeName = completeName.trim();
                    if (!completeName) {
                        return empty;
                    }
                    return completeName;
                }
            },
            {
                data: null, title: "Cod. País", render: function (data) {
                    return data.codigoPais ? data.codigoPais : empty;
                }
            },
            {
                data: null, title: "Número", render: function (data) {
                    return data.numero ? data.numero : empty;
                }
            },
            {
                data: null, title: "Número Alternativo", render: function (data) {
                    return data.numeroAlternativo ? data.numeroAlternativo : empty;
                }
            },
            {
                data: null, title: "Sala", render: function (data) {
                    return data.nombreSala ? data.nombreSala : empty;
                }
            },
            {
                data: null, title: "T. Cliente", render: function (data) {
                    return data.tipoCliente ? data.tipoCliente : empty;
                }
            },
            {
                data: null, title: "T. Frecuencia", render: function (data) {
                    return data.tipoFrecuencia ? data.tipoFrecuencia : empty;
                }
            },
            {
                data: null, title: "T. Juego", render: function (data) {
                    return data.tipoJuego ? data.tipoJuego : empty;
                }
            },
            {
                data: "idCliente", title: "Acción", bSortable: false, render: function (o) {
                    let btnUpdatePhone = `<button type="button" class="btn btn-xs btn-warning btnEditar" data-nombre="Actualizar Contacto" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> `;

                    return btnUpdatePhone;
                }
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
            setICheck();
        }
    })
}

const getSalasByUser = () => {
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if (response.data.length) {
                data = response.data;
                setSelect(cboSala, data, "CodSala", "Nombre", true);
            } else {
                toastr.error('No se pudo obtener la lista de salas del Usuario.')
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

const getClientTypes = () => {
    $.ajax({
        type: "POST",
        url: basePath + "AsistenciaCliente/GetListadoTipoCliente",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if (response.respuesta) {
                data = response.data;
                setSelect(cboTipoCliente, data, "Id", "Nombre", true);
            } else {
                toastr.error('No se pudo obtener la lista de Tipos de Cliente.')
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

const getFrecuencyTypes = () => {
    $.ajax({
        type: "POST",
        url: basePath + "AsistenciaCliente/GetListadoTipoFrecuencia",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if (response.respuesta) {
                data = response.data;
                setSelect(cboTipoFrecuencia, data, "Id", "Nombre", true);
            } else {
                toastr.error('No se pudo obtener la lista de Tipos de Frecuencia.')
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

const getGameTypes = () => {
    $.ajax({
        type: "POST",
        url: basePath + "AsistenciaCliente/GetListadoTipoJuego",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if (response.respuesta) {
                data = response.data;
                setSelect(cboTipoJuego, data, "Id", "Nombre", true);
            } else {
                toastr.error('No se pudo obtener la lista de Tipos de Juego.')
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
//#endregion

//#region Utils
const resetMessage = () => {
    removeImage();
    $('#txtMensaje').val('');
}

const setICheck = () => {
    const configICheck = {
        checkboxClass: 'icheckbox_square-blue icheckbox_bg-white',
        increaseArea: '20%'
    }
    $('input[type=checkbox][name=radioCliente]').iCheck(configICheck);
}

const initPopovers = () => {
    initPoppover($('#popover-help'), $('#popover-help-content'), "Ayuda para formatear Texto");
}

const initPoppover = (item, itemContent, title) => {
    item.popover({
        title: title,
        html: true,
        animation: true,
        delay: {
            show: 0,
            hide: 150
        },
        placement: 'left',
        trigger: 'hover',
        content: function () {
            return itemContent.html();
        }
    });
}

const removeImage = () => {
    inputImage.val('');
    imagePreview.attr('src', '');;
    imagePreview.css('display', 'none');
    containerImage.css('display', 'none');
}

const showImage = () => {
    imagePreview.removeAttr('style');
    containerImage.removeAttr('style');
}

const getSelectedRows = (table) => {
    let selectedRows = [];

    table.rows().every(function () {
        let node = this.node()
        let element = $(node).find(`input.icheck_single[type="checkbox"]`)

        if (element.is(":checked")) {
            let idCliente = element.data('id');
            let codSala = element.data('cod-sala');

            selectedRows.push({ idCliente, codSala });
        }
    });
    return selectedRows;
}

const setSelect = (cbo, data, id, value, isMultiple) => {
    cbo.empty();
    $.each(data, function (index, item) {
        cbo.append('<option value="' + item[id] + '">' + item[value] + '</option>');
    });
    cbo.append(`<option value="${TODOS}">TODOS</option>`);
    cbo.select2({
        placeholder: "--Seleccione--", allowClear: true, multiple: isMultiple
    });
    cbo.val(null).trigger("change");
}

const manipulateSelects = (cbo, isAllSelect) => {
    if (isAllSelect) {
        selectAllSelectItems(cbo, true);
    } else {
        unSelectAllSelectItems(cbo, true);
    }
}

const selectAllSelectItems = (cbo, isMultiple) => {
    cbo.select2('destroy').find('option').prop('selected', 'selected').end().select2({ placeholder: "--Seleccione--", allowClear: true, multiple: isMultiple });
}

const unSelectAllSelectItems = (cbo, isMultiple) => {
    cbo.select2('destroy').find('option').prop('selected', false).end().select2({ placeholder: "--Seleccione--", allowClear: true, multiple: isMultiple });
}

const getAllValuesOfSelect = (cbo) => {
    let allValues = [];
    let options = cbo[0].options;
    for (let i = 0; i < options.length; i++) {
        allValues.push(options[i].value);
    }
    return allValues;
}

const removeElementOfArray = (arr, elementToRemove) => {
    let index = arr.indexOf(elementToRemove);

    if (index !== -1) {
        arr.splice(index, 1);
    }

    return arr;
}

const valideKeyNumber = (evt) => {
    let code = (evt.which) ? evt.which : evt.keyCode;
    if (code >= 48 && code <= 57 || code == 13) {
        return true;
    }
    return false;
}
//#endregion
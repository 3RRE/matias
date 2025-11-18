const btnBuscar = $('#btnBuscar');
const btnNuevo = $('#btnNuevo');
const languajeTagInput = {
    empty: "Advertencia, no puede agregar una etiqueta vacía.",
    minLength: "Advertencia, su etiqueta debe tener al menos %s caracteres..",
    maxLength: "Advertencia, su etiqueta no debe exceder %s caracteres.",
    max: "Advertencia, el número de etiquetas no debe exceder %s elementos.",
    email: "Advertencia, la dirección de correo electrónico que ingresó no es válida.",
    exists: "¡Advertencia, esta etiqueta ya existe!",
    autocomplete_only: "Advertencia, debe seleccionar un valor de la lista.",
    timeout: 8e3
};

$(document).ready(() => {
    createDataTable([]);
})

$(document).on('click', '#btnNuevo', function (e) {
    const nombre = $(this).data("nombre");
    $("#hddmodal").val(nombre);
    $('#bodyModal').html("");
    $('#default-modal-label').html(nombre);
    var redirectUrl = basePath + "PruConfiguraciones/ProgresivoRuletaConfiguracionInsertarVista";
    var ubicacion = "bodyModal";
    $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
        $("#modalGroup").modal("show");
    });
})

$(document).on('click', '.btnEditar', function (e) {
    const id = $(this).data("id");
    const nombre = $(this).data("nombre");
    $("#hddmodal").val(nombre);
    $('#bodyModal').html("");
    $('#default-modal-label').html(nombre);
    const redirectUrl = basePath + "PruConfiguraciones/ProgresivoRuletaConfiguracionActualizarVista/" + id;
    const ubicacion = "bodyModal";
    $.when(DataPostModo1(redirectUrl, ubicacion)).then(function (response, textStatus) {
        $("#modalGroup").modal("show");
    });
});

btnBuscar.on('click', () => {
    const filters = getFilters();
    if (!filters) {
        return;
    }
    getConfigurations(filters)
})

const getConfigurations = (filters) => {
    const url = `${basePath}PruConfiguraciones/ObtenerConfiguracionesPorCodSala`;
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
                targets: [0],
                width: "50px",

            },
            {
                targets: "_all",
                className: "text-center"
            }
        ],
        data: data,
        columns: [
            {
                data: "Sala.Nombre", title: "Sala",
                render: function (value, v, item) {
                    const { Sala } = item;
                    return `${Sala.CodSala} - ${Sala.Nombre}`;
                }
            },
            { data: "IdRuleta", title: "Id Ruleta" },
            { data: "NombreRuleta", title: "Nombre" },
            { data: "TotalSlots", title: "Cantidad de Máquinas" },
            //{ data: "CodMaquinas", title: "Máquinas" },
            { data: "MinSlotsPlaying", title: "Mínimo Maquina" },
            { data: "CoinInPercent", title: "Porcentaje" },
            { data: "MinBet", title: "Apuesta Mínima" },
            { data: "Ip", title: "IP" },
            {
                data: "StatusOk", title: "Status",
                render: function (value) {
                    const icon = value ? "las la-check-circle" : "las la-times - circle";
                    const color = value ? "success" : "danger";
                    return `<span class="label label-${color} fs-5"><i class="${icon}"></i><span>`;
                }
            },
            { data: "HoraInicioStr", title: "Hora Inicio" },
            { data: "HoraFinStr", title: "Hora Fin" },
            {
                data: "IdRuleta",
                title: "Acción",
                bSortable: false,
                render: function (id) {
                    let btnEdit = `<button type="button" class="btn btn-xs btn-warning btnEditar" data-nombre="Editar Configuración Progresivo Ruleta" data-id="${id}"><i class="glyphicon glyphicon-pencil"></i></button> `;
                    return btnEdit;
                }
            }
        ],
    });
}

$(document).on('keydown', '.inputTags-field', function (event) {
    const key = event.which || event.keyCode;
    if (key === 13) {
        event.preventDefault();
    }
});

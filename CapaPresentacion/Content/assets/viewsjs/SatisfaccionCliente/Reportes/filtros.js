const inputDesde = $('#fechaDesde');
const inputHasta = $('#fechaHasta');
const cboSala = $("#cboSala");

$(document).ready(() => {
    getSalas();
    setDateFilter();
    
    setComboSelect2(cboSala);
})

const getSalas = () => {
    $.when(
        llenarSelect(`${basePath}Sala/ListadoSalaPorUsuarioJson`, {}, "cboSala", "CodSala", "Nombre")
    ).then(() => {
        cboSala.select2({
            placeholder: "Seleccione ...",
            multiple: true,
        });
    });
}

const getFilters = () => {
    const filters = {
        codsSala: cboSala.val(),
        fechaInicio: inputDesde.val(),
        fechaFin: inputHasta.val(),
    };

    const { codsSala, fechaInicio, fechaFin } = filters;

    if (!fechaInicio) {
        toastr.warning('Tiene que ingresar la fecha desde cuando desea consultar.', '¡Aviso!')
        return;
    }

    if (!fechaFin) {
        toastr.warning('Tiene que ingresar la fecha hasta cuando desea consultar.', '¡Aviso!')
        return;
    }

    if (!codsSala) {
        toastr.warning('Tiene que seleccionar al menos una sala.', '¡Aviso!')
        return;
    }

    return filters;
}

const setDateFilter = () => {
    $(".dateFilter").datetimepicker({
        pickTime: true,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow
    });
}

const setComboSelect2 = (combo, data = {}) => {
    if (!existCombo(combo)) {
        return;
    }
    combo.select2({
        placeholder: "Seleccione ...",
        multiple: true,
        data: data
    });
}

const existCombo = (combo) => {
    return combo.length;
}
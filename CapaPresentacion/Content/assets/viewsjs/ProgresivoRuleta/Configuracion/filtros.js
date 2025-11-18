const cboSala = $("#cboSala");

$(document).ready(() => {
    getSalas();
    setComboSelect2(cboSala);
})

const getSalas = () => {
    $.when(
        llenarSelect(`${basePath}Sala/ListadoSalaPorUsuarioJson`, {}, "cboSala", "CodSala", "Nombre")
    ).then(() => {
        cboSala.select2({
            placeholder: "Seleccione ...",
            multiple: false,
        });
    });
}

const getFilters = () => {
    const filters = {
        codSala: cboSala.val(),
    };

    const { codSala } = filters;

    if (!codSala) {
        toastr.warning('Tiene que seleccionar una sala.', '¡Aviso!')
        return;
    }

    return filters;
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
const inputDesde = $('#fechaDesde');
const inputHasta = $('#fechaHasta');
const cboSala = $("#cboSala");
const cboTipo = $("#cboTipo");
const cboSubTipo = $("#cboSubTipo");
const cboProducto = $("#cboProducto");
const cboMaquina = $("#cboMaquina");

$(document).ready(() => {
    getSalas();
    getTipos();
    setDateFilter();
    
    setComboSelect2(cboSala);
    setComboSelect2(cboTipo);
    setComboSelect2(cboSubTipo);
    setComboSelect2(cboProducto);
    setComboSelect2(cboMaquina);
    resetCombo(cboSubTipo);
    resetCombo(cboProducto);
    resetCombo(cboMaquina);
})

cboSala.on('change', () => {
    if (!existCombo(cboMaquina)) {
        return;
    }
    const codsSala = cboSala.val();
    if (!codsSala) {
        cleanCombo(cboMaquina)
        return;
    }
    getMaquinas(codsSala);
});

cboTipo.on('change', () => {
    const idsTipo = cboTipo.val();
    if (!idsTipo) {
        cleanCombo(cboSubTipo)
        cleanCombo(cboProducto)
        return;
    }
    getSubTipos(idsTipo);
});

cboSubTipo.on('change', () => {
    const idsSubTipo = cboSubTipo.val();
    if (!idsSubTipo) {
        cleanCombo(cboProducto)
        return;
    }
    getProductos(idsSubTipo);
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

const getTipos = () => {
    $.when(
        llenarSelect(`${basePath}Tipos/GetTipos`, {}, "cboTipo", "Id", "Nombre")
    ).then(() => {
        cboTipo.select2({
            placeholder: "Seleccione ...",
            multiple: true
        });
    });
}

const getSubTipos = (idsTipo) => {
    const idsSubTipo = cboSubTipo.val();
    llenarSelect2Agrupado({
        url: `${basePath}SubTipos/GetSubTiposByIdsTipo`,
        data: { idsTipo },
        select: "cboSubTipo",
        campoId: "Id",
        campoAgrupacion: "NombreTipo",
        campoValor: "Nombre",
        isMultiple: true,
        selectedVal: idsSubTipo
    })
}

const getProductos = (idsSubTipo) => {
    const idsProducto = cboProducto.val();
    llenarSelect2Agrupado({
        url: `${basePath}Productos/GetProductosByIdsSubTipo`,
        data: { idsSubTipo },
        select: "cboProducto",
        campoId: "Id",
        campoAgrupacion: "NombreTipo",
        campoValor: "Nombre",
        isMultiple: true,
        selectedVal: idsProducto
    })
}

const getMaquinas = (codsSala) => {
    const idsMaquinas = cboMaquina.val();
    llenarSelect2Agrupado({
        url: `${basePath}Cortesias/GetMaquinasByCodsSala`,
        data: { codsSala },
        select: "cboMaquina",
        campoId: "Id",
        campoAgrupacion: "Sala",
        campoValor: "CodMaquina",
        isMultiple: false,
        selectedVal: idsMaquinas
    })
}

const getFilters = () => {
    const filters = {
        desde: inputDesde.val(),
        hasta: inputHasta.val(),
        codsSala: cboSala.val(),
        idsTipo: cboTipo.val(),
        idsSubTipo: cboSubTipo.val(),
        idsProducto: cboProducto.val(),
    };

    if (existCombo(cboMaquina)) {
        filters.idSalaMaquina = cboMaquina.val();
    }

    const { desde, hasta, codsSala } = filters;

    if (!desde) {
        toastr.warning('Tiene que ingresar la fecha desde cuando desea consultar.', '¡Aviso!')
        return;
    }

    if (!hasta) {
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
        format: 'DD/MM/YYYY HH:mm',
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

const resetCombo = (combo) => {
    if (!existCombo(combo)) {
        return;
    }
    combo.html("");
    combo.append('<option value="">Cargando ...</option>');
    combo.prop('disabled', true);
}

const cleanCombo = (combo) => {
    if (!existCombo(combo)) {
        return;
    }
    combo.empty();
    combo.prop('disabled', true);
}

const existCombo = (combo) => {
    return combo.length;
}
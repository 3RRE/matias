/* ProgresivoTercerosMain.js
 * Vista: ProgresivoTerceros/Index
 * Depende de: jQuery, DataTables, (opcional) select2, moment + datetimepicker
 */

(function ($) {
    'use strict';

    var basePath = $('#BasePath').val() || ''; // viene del _Layout
    var ENDPOINTS = {
        listar: basePath + 'ProgresivoTerceros/Listar',
        guardar: basePath + 'ProgresivoTerceros/Guardar',
        eliminar: basePath + 'ProgresivoTerceros/Eliminar',
        excel: basePath + 'ProgresivoTerceros/ExportarExcel',
        salas: basePath + 'ProgresivoTerceros/ListarSalas',
        progresivos: basePath + 'ProgresivoTerceros/ListarProgresivos'
    };

    var $table, dt;

    function toDateOrNull(v) {
        return v ? v : null;
    }

    function ajaxGet(url, data) {
        return $.ajax({
            url: url,
            type: 'GET',
            data: data,
            dataType: 'json'
        });
    }

    function ajaxPost(url, data) {
        return $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify(data),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        });
    }

    function showToastrOk(msg) { if (window.toastr) toastr.success(msg); }
    function showToastrWarn(msg) { if (window.toastr) toastr.warning(msg); }
    function showToastrErr(msg) { if (window.toastr) toastr.error(msg); }

    function initSelect2() {
        if ($.fn.select2) {
            $('#cboSala,#cboProgresivo,#frmSala,#frmProgresivo').select2({ width: '100%' });
        }
    }

    function initDates() {
        var $dates = $('.dateOnly_');
        if ($.fn.datetimepicker) {
            $dates.datetimepicker({
                format: 'DD/MM/YYYY',
                useCurrent: false,
                icons: {
                    time: 'glyphicon glyphicon-time',
                    date: 'glyphicon glyphicon-calendar',
                    up: 'glyphicon glyphicon-chevron-up',
                    down: 'glyphicon glyphicon-chevron-down',
                    previous: 'glyphicon glyphicon-chevron-left',
                    next: 'glyphicon glyphicon-chevron-right',
                    today: 'glyphicon glyphicon-screenshot',
                    clear: 'glyphicon glyphicon-trash',
                    close: 'glyphicon glyphicon-remove'
                }
            });
        }
    }

    function setColorPreview(val) {
        if (/^#([0-9a-f]{3}|[0-9a-f]{6})$/i.test(val)) {
            $('#colorPreview').css('background', val);
        } else {
            $('#colorPreview').css('background', 'transparent');
        }
    }

    function cargarSalas($combo) {
        $combo.empty().append('<option value="">--Seleccione--</option>');
        return ajaxGet(ENDPOINTS.salas).done(function (r) {
            if (r && r.data) {
                $.each(r.data, function (_, it) {
                    $combo.append('<option value="' + it.CodSala + '">' + (it.NombreSala || it.Nombre || it.CodSala) + '</option>');
                });
            }
        });
    }

    function cargarProgresivos($combo, codSala) {
        $combo.empty().append('<option value="">--Seleccione--</option>');
        return ajaxGet(ENDPOINTS.progresivos, { codSala: codSala || '' }).done(function (r) {
            if (r && r.data) {
                $.each(r.data, function (_, it) {
                    $combo.append('<option value="' + it.CodProgresivo + '">' + (it.Nombre || ('Prog ' + it.CodProgresivo)) + '</option>');
                });
            }
        });
    }

    function initTabla() {
        $table = $('#tableProgresivoTerceros');
        dt = $table.DataTable({
            processing: true,
            serverSide: false, 
            searching: false,
            paging: true,
            ordering: false,
            ajax: function (data, callback) {
                var filtros = getFiltros();
                ajaxGet(ENDPOINTS.listar, filtros)
                    .done(function (r) {
                        var rows = (r && r.data) || [];
                        callback({ data: rows, recordsTotal: rows.length, recordsFiltered: rows.length });
                    })
                    .fail(function () {
                        showToastrErr('No se pudo obtener el listado.');
                        callback({ data: [], recordsTotal: 0, recordsFiltered: 0 });
                    });
            },
            columns: [
                {
                    data: null,
                    title: 'Acciones',
                    render: function (_, __, row) {
                        var id = row.CodSalaProgresivo || row.Id || 0;
                        return '' +
                            '<div class="btn-group btn-group-xs">' +
                            '  <button type="button" class="btn btn-primary btn-edit" data-id="' + id + '"><span class="glyphicon glyphicon-pencil"></span></button>' +
                            '  <button type="button" class="btn btn-danger btn-del" data-id="' + id + '"><span class="glyphicon glyphicon-trash"></span></button>' +
                            '</div>';
                    }
                },
                { data: 'NombreSala', title: 'Sala', defaultContent: '' },
                { data: 'Nombre', title: 'Progresivo', defaultContent: '' },
                { data: 'NroPozos', title: 'N° Pozos', defaultContent: 0 },
                { data: 'NroJugadores', title: 'N° Jugadores', defaultContent: 0 },
                { data: 'SubidaCreditos', title: 'Subida Créditos', defaultContent: 0 },
                {
                    data: 'FechaInstalacion', title: 'F. Instalación',
                    render: function (v) { return v ? v.toString().substring(0, 10) : ''; }
                },
                {
                    data: 'FechaDesinstalacion', title: 'F. Desinst.',
                    render: function (v) { return v ? v.toString().substring(0, 10) : ''; }
                },
                {
                    data: 'Estado', title: 'Estado',
                    render: function (v) {
                        return (v === 1 || v === true) ? '<span class="label label-success">Habilitado</span>' : '<span class="label label-default">Deshabilitado</span>';
                    }
                }
            ],
            language: {
                url: basePath + 'Content/assets/js/optional/datatables/i18n/Spanish.json'
            }
        });

        $table.on('click', '.btn-edit', function () {
            var id = $(this).data('id');
            var row = dt.row($(this).closest('tr')).data();
            abrirEdicion(row, id);
        });

        $table.on('click', '.btn-del', function () {
            var id = $(this).data('id');
            $('#delId').val(id);
            $('#modalEliminar').modal('show');
        });
    }

    function getFiltros() {
        return {
            codSala: $('#cboSala').val(),
            codProgresivo: $('#cboProgresivo').val(),
            fechaInicio: $('#fechaInicio').val(),
            fechaFin: $('#fechaFin').val()
        };
    }

    function reloadTabla() {
        if (dt) dt.ajax.reload(null, false);
    }

    function limpiarForm() {
        $('#CodSalaProgresivo').val('0');
        $('#frmSala').val('').trigger('change');
        $('#frmProgresivo').val('').trigger('change');
        $('#frmNroPozos').val('');
        $('#frmNroJugadores').val('');
        $('#frmSubidaCreditos').val('');
        $('#frmFechaInstalacion').val('');
        $('#frmFechaDesinstalacion').val('');
        $('#frmSigla').val('');
        $('#frmColorHexa').val('');
        setColorPreview('');
        $('#frmUrl').val('');
        $('#frmNombre').val('');
        $('#frmTipoProgresivo').val('');
        $('#frmActivo').prop('checked', true);
        $('#frmEstado').val('1');
    }

    function abrirNuevo() {
        limpiarForm();
        $('#modalProgresivoTerceroForm').modal('show');
    }

    function abrirEdicion(row) {
        limpiarForm();
        if (!row) return;
        $('#CodSalaProgresivo').val(row.CodSalaProgresivo || 0);
        $('#frmSala').val(row.CodSala).trigger('change');
        $('#frmProgresivo').val(row.CodProgresivo).trigger('change');
        $('#frmNroPozos').val(row.NroPozos);
        $('#frmNroJugadores').val(row.NroJugadores);
        $('#frmSubidaCreditos').val(row.SubidaCreditos);
        $('#frmFechaInstalacion').val(row.FechaInstalacion ? row.FechaInstalacion.toString().substring(0, 10) : '');
        $('#frmFechaDesinstalacion').val(row.FechaDesinstalacion ? row.FechaDesinstalacion.toString().substring(0, 10) : '');
        $('#frmSigla').val(row.Sigla);
        $('#frmColorHexa').val(row.ColorHexa);
        setColorPreview(row.ColorHexa);
        $('#frmUrl').val(row.Url);
        $('#frmNombre').val(row.Nombre);
        $('#frmTipoProgresivo').val(row.TipoProgresivo);
        $('#frmActivo').prop('checked', !!row.Activo);
        $('#frmEstado').val(row.Estado != null ? row.Estado : 1);
        $('#modalProgresivoTerceroForm').modal('show');
    }

    function getFormData() {
        return {
            CodSalaProgresivo: parseInt($('#CodSalaProgresivo').val() || '0', 10),
            CodSala: parseInt($('#frmSala').val() || '0', 10),
            CodProgresivo: parseInt($('#frmProgresivo').val() || '0', 10),
            NroPozos: parseInt($('#frmNroPozos').val() || '0', 10),
            NroJugadores: parseInt($('#frmNroJugadores').val() || '0', 10),
            SubidaCreditos: parseInt($('#frmSubidaCreditos').val() || '0', 10),
            FechaInstalacion: toDateOrNull($('#frmFechaInstalacion').val()),
            FechaDesinstalacion: toDateOrNull($('#frmFechaDesinstalacion').val()),
            ColorHexa: $('#frmColorHexa').val(),
            Sigla: $('#frmSigla').val(),
            Url: $('#frmUrl').val(),
            Nombre: $('#frmNombre').val(),
            TipoProgresivo: $('#frmTipoProgresivo').val(),
            Activo: $('#frmActivo').is(':checked'),
            Estado: parseInt($('#frmEstado').val() || '1', 10)
        };
    }

    function guardar() {
        var data = getFormData();

        if (!data.CodSala) { showToastrWarn('Seleccione una sala.'); return; }
        if (!data.CodProgresivo) { showToastrWarn('Seleccione un progresivo.'); return; }

        ajaxPost(ENDPOINTS.guardar, data)
            .done(function (r) {
                if (r && r.respuesta) {
                    showToastrOk(r.mensaje || 'Guardado correctamente.');
                    $('#modalProgresivoTerceroForm').modal('hide');
                    reloadTabla();
                } else {
                    showToastrWarn((r && r.mensaje) || 'No se pudo guardar.');
                }
            })
            .fail(function () {
                showToastrErr('Error al guardar.');
            });
    }

    function eliminarConfirmado() {
        var id = $('#delId').val();
        if (!id || id === '0') { $('#modalEliminar').modal('hide'); return; }
        ajaxPost(ENDPOINTS.eliminar, { id: parseInt(id, 10) })
            .done(function (r) {
                if (r && r.respuesta) {
                    showToastrOk(r.mensaje || 'Eliminado correctamente.');
                    reloadTabla();
                } else {
                    showToastrWarn((r && r.mensaje) || 'No se pudo eliminar.');
                }
            })
            .fail(function () {
                showToastrErr('Error al eliminar.');
            })
            .always(function () {
                $('#modalEliminar').modal('hide');
            });
    }

    function exportarExcel() {
        var q = $.param(getFiltros());
        window.location = ENDPOINTS.excel + (q ? ('?' + q) : '');
    }

    function bindEvents() {
        $('#btnBuscar').on('click', reloadTabla);
        $('#btnExcel').on('click', exportarExcel);
        $('#btnNuevo').on('click', abrirNuevo);
        $('#btnGuardar').on('click', guardar);
        $('#btnEliminarConfirm').on('click', eliminarConfirmado);

        $('#frmColorHexa').on('input', function () { setColorPreview(this.value); });

        $('#cboSala').on('change', function () {
            cargarProgresivos($('#cboProgresivo'), $(this).val());
        });

        $('#frmSala').on('change', function () {
            cargarProgresivos($('#frmProgresivo'), $(this).val());
        });
    }

    $(function () {
        initSelect2();
        initDates();
        setColorPreview($('#frmColorHexa').val() || '');

        $.when(
            cargarSalas($('#cboSala')),
            cargarSalas($('#frmSala'))
        ).done(function () {
            cargarProgresivos($('#cboProgresivo'), $('#cboSala').val());
            cargarProgresivos($('#frmProgresivo'), $('#frmSala').val());
        });

        initTabla();
        bindEvents();
    });

})(jQuery);

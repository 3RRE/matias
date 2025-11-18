// Content/assets/viewsjs/Progresivo/ProgresivoTerceros/SalasProgresivoTerceros.js
(function ($) {
    'use strict';

    var urlBase = $('#pt-urlBase').val() || '';
    var token = $('input[name="__RequestVerificationToken"]').val();

    // Buscar por filtro
    $('#frmFiltro').on('submit', function (e) {
        e.preventDefault();
        var codSala = $('#txtCodSala').val() || 0;
        if (parseInt(codSala, 10) > 0) {
            $.get(urlBase + '/ListarSalaPorSala', { codSala: codSala }, function (r) {
                if (!r || !r.Status) { alert((r && r.Msg) || 'Error'); return; }
                pintarTabla(r.Value || []);
            }).fail(function () { alert('Error de comunicación'); });
        } else {
            pintarTabla([]);
        }
    });

    // Limpiar
    $('#btnLimpiar').on('click', function () {
        $('#txtCodSala').val('');
        pintarTabla([]);
    });

    // Pintar tabla
    function pintarTabla(lista) {
        var $tb = $('#tblSalas tbody').empty();
        (lista || []).forEach(function (s) {
            $tb.append(
                '<tr>' +
                '<td>' + (s.CodSalaProgresivo || '') + '</td>' +
                '<td>' + (s.Nombre || '') + '</td>' +
                '<td>' + (s.CodSala || '') + '</td>' +
                '<td>' + (s.CodProgresivo || '') + '</td>' +
                '<td>' + (s.NroPozos || 0) + '</td>' +
                '<td>' + (s.Activo ? '<span class="label label-success">Activo</span>' : '<span class="label label-default">Inactivo</span>') + '</td>' +
                '<td>' +
                '<a class="btn btn-xs btn-info" href="' + urlBase + '/Detalle/' + (s.CodSalaProgresivo || 0) + '">' +
                '<i class="fa fa-eye"></i> Ver' +
                '</a> ' +
                '<button class="btn btn-xs btn-primary btn-edit" data-id="' + (s.CodSalaProgresivo || 0) + '">' +
                '<i class="fa fa-pencil"></i> Editar' +
                '</button> ' +
                '<button class="btn btn-xs btn-danger btn-del" data-id="' + (s.CodSalaProgresivo || 0) + '">' +
                '<i class="fa fa-trash"></i> Eliminar' +
                '</button>' +
                '</td>' +
                '</tr>'
            );
        });
    }

    // Abrir modal nuevo
    $('#btnNuevaSala').on('click', function () {
        limpiarFormSala();
        $('#mdlSalaTitle').text('Nueva sala progresivo');
        $('#mdlSala').modal('show');
    });

    // Editar
    $('#tblSalas').on('click', '.btn-edit', function () {
        var id = $(this).data('id');
        $.get(urlBase + '/ObtenerSala', { id: id }, function (r) {
            if (!r || !r.Status) { alert((r && r.Msg) || 'No encontrado'); return; }
            llenarFormSala(r.Value || {});
            $('#mdlSalaTitle').text('Editar sala progresivo');
            $('#mdlSala').modal('show');
        }).fail(function () { alert('Error de comunicación'); });
    });

    // Eliminar (borrado lógico)
    $('#tblSalas').on('click', '.btn-del', function () {
        var id = $(this).data('id');
        if (!confirm('¿Eliminar esta sala (borrado lógico)?')) return;
        $.ajax({
            url: urlBase + '/EliminarSala',
            method: 'POST',
            headers: { 'RequestVerificationToken': token },
            data: { id: id },
            success: function (r) {
                if (!r || !r.Status) { alert((r && r.Msg) || 'Error'); return; }
                $('#frmFiltro').submit();
            },
            error: function () { alert('Error de comunicación'); }
        });
    });

    // Guardar (crear/editar)
    $('#frmSala').on('submit', function (e) {
        e.preventDefault();
        var obj = leerFormSala();
        var esEdicion = !!obj.CodSalaProgresivo && obj.CodSalaProgresivo > 0;
        var url = urlBase + (esEdicion ? '/EditarSala' : '/CrearSala');

        $.ajax({
            url: url,
            method: 'POST',
            headers: { 'RequestVerificationToken': token },
            data: obj,
            success: function (r) {
                if (!r || !r.Status) { alert((r && r.Msg) || 'Error'); return; }
                $('#mdlSala').modal('hide');
                $('#frmFiltro').submit();
            },
            error: function () { alert('Error de comunicación'); }
        });
    });

    // Helpers formulario
    function limpiarFormSala() {
        $('#CodSalaProgresivo').val('');
        $('#CodSala').val($('#txtCodSala').val() || '');
        $('#CodProgresivo, #NroPozos, #NroJugadores, #SubidaCreditos, #CodProgresivoWO, #CodTipoConfiguracionProgresivo').val('');
        $('#Nombre, #TipoProgresivo, #Sigla, #ColorHexa, #Url, #NombreSala, #RazonSocial').val('');
        $('#FechaInstalacion, #FechaDesinstalacion').val('');
        $('#Activo').prop('checked', true);
        $('#Estado').val(1);
    }

    function llenarFormSala(s) {
        $('#CodSalaProgresivo').val(s.CodSalaProgresivo || 0);
        $('#CodSala').val(s.CodSala || '');
        $('#CodProgresivo').val(s.CodProgresivo || '');
        $('#NroPozos').val(s.NroPozos || 0);
        $('#NroJugadores').val(s.NroJugadores || 0);
        $('#SubidaCreditos').val(s.SubidaCreditos || 0);
        $('#Nombre').val(s.Nombre || '');
        $('#TipoProgresivo').val(s.TipoProgresivo || '');
        $('#Sigla').val(s.Sigla || '');
        $('#ColorHexa').val(s.ColorHexa || '');
        $('#Url').val(s.Url || '');
        $('#CodProgresivoWO').val(s.CodProgresivoWO || 0);
        $('#CodTipoConfiguracionProgresivo').val(s.CodTipoConfiguracionProgresivo || 0);
        $('#NombreSala').val(s.NombreSala || '');
        $('#RazonSocial').val(s.RazonSocial || '');
        $('#Activo').prop('checked', !!s.Activo);
        $('#Estado').val(s.Estado != null ? s.Estado : 1);
        $('#FechaInstalacion').val(formatDateInput(s.FechaInstalacion));
        $('#FechaDesinstalacion').val(formatDateInput(s.FechaDesinstalacion));
    }

    function leerFormSala() {
        return {
            CodSalaProgresivo: parseInt($('#CodSalaProgresivo').val() || 0, 10),
            CodSala: parseInt($('#CodSala').val() || 0, 10),
            CodProgresivo: parseInt($('#CodProgresivo').val() || 0, 10),
            NroPozos: parseInt($('#NroPozos').val() || 0, 10),
            NroJugadores: parseInt($('#NroJugadores').val() || 0, 10),
            SubidaCreditos: parseInt($('#SubidaCreditos').val() || 0, 10),
            Nombre: $('#Nombre').val(),
            TipoProgresivo: $('#TipoProgresivo').val(),
            Sigla: $('#Sigla').val(),
            ColorHexa: $('#ColorHexa').val(),
            Url: $('#Url').val(),
            CodProgresivoWO: parseInt($('#CodProgresivoWO').val() || 0, 10),
            CodTipoConfiguracionProgresivo: parseInt($('#CodTipoConfiguracionProgresivo').val() || 0, 10),
            NombreSala: $('#NombreSala').val(),
            RazonSocial: $('#RazonSocial').val(),
            Activo: $('#Activo').is(':checked'),
            Estado: parseInt($('#Estado').val() || 1, 10),
            FechaInstalacion: $('#FechaInstalacion').val(),
            FechaDesinstalacion: $('#FechaDesinstalacion').val()
        };
    }

    function formatDateInput(val) {
        if (!val) return '';
        var d = new Date(val);
        if (isNaN(d)) return '';
        var m = (d.getMonth() + 1).toString().padStart(2, '0');
        var day = d.getDate().toString().padStart(2, '0');
        return d.getFullYear() + '-' + m + '-' + day;
    }

    // Autocargar si vino codSala en ViewBag (desde hidden)
    $(function () {
        var codSalaVB = $('#pt-codSalaVB').val();
        if (codSalaVB) {
            $('#txtCodSala').val(codSalaVB);
            $('#frmFiltro').submit();
        }
    });

})(jQuery);

(function () {
    var PT = window.PT || {};
    var codSala = PT.codSala || 0;
    var defaultClase = PT.defaultClase || '';
    var baseSalaUrl = PT.baseSalaUrl || '';
    var exportUrl = PT.exportUrl || '';
    var msgOk = PT.msgOk || '';
    var msgErr = PT.msgErr || '';

    var $ = window.jQuery || function () {
        return {
            on: function () { }, val: function () { }, addClass: function () { }, removeClass: function () { },
            text: function () { }, show: function () { }, hide: function () { }, is: function () { return false; },
            length: 0
        };
    };
    var toastr = window.toastr || { success: function () { }, error: function () { }, warning: function () { } };

    var btnBuscar = document.getElementById('btnBuscar');
    var cboSala = document.getElementById('cboSala');
    var cboClase = document.getElementById('cboClase');

    var cardLinktek = document.getElementById('cardLinktek');
    var cardTerceros = document.getElementById('cardTerceros');
    var btnRegistrar = document.getElementById('btnRegistrarTerceros');
    var btnExcel = document.getElementById('btnExcel');

    var tblLinktek = '#tblLinktek';
    var tblTerceros = '#tblTerceros';

    var pendingDelete = null;

    if (msgOk) toastr.success(msgOk);
    if (msgErr) toastr.error(msgErr);

    function goToSala(id) {
        if (!id || id === "0") { toastr.warning('Seleccione una sala.'); return; }
        var clase = cboClase ? (cboClase.value || '') : '';
        var url = baseSalaUrl + encodeURIComponent(id);
        if (clase) url += '?clase=' + encodeURIComponent(clase);
        window.location.href = url;
    }
    function buscar() { if (cboSala) goToSala(cboSala.value); }

    function isDT(selector) {
        try { return $.fn && $.fn.DataTable && $.fn.DataTable.isDataTable(selector); }
        catch (e) { return false; }
    }
    function initDT(selector) {
        if (!$.fn || !$.fn.DataTable) return;
        var el = $(selector);
        if (!el.length || isDT(selector)) return;
        el.DataTable({
            paging: true,
            searching: true,
            info: true,
            lengthChange: true,
            pageLength: 10,
            order: [],
            autoWidth: false,
            language: { url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json' }
        });
    }
    function adjustVisibleDT() {
        setTimeout(function () {
            try { if ($(tblLinktek).is(':visible') && isDT(tblLinktek)) $(tblLinktek).DataTable().columns.adjust(); } catch (e) { }
            try { if ($(tblTerceros).is(':visible') && isDT(tblTerceros)) $(tblTerceros).DataTable().columns.adjust(); } catch (e) { }
        }, 120);
    }

    function refreshRegistrarBtn() {
        if (!btnRegistrar) return;
        btnRegistrar.style.display = (cboClase && cboClase.value === 'Terceros') ? '' : 'none';
        btnRegistrar.disabled = (cboSala ? (cboSala.value === "0" || !cboSala.value) : true);
    }

    if (btnExcel) $('#btnExcel').on('click', function () {
        var sala = cboSala ? (cboSala.value || '') : '';
        if (!sala || sala === "0") { toastr.warning('Seleccione una sala.'); return; }
        var clase = cboClase ? (cboClase.value || 'Linktek') : 'Linktek';
        var url = exportUrl + '?id=' + encodeURIComponent(sala) + '&clase=' + encodeURIComponent(clase);
        window.location.href = url;
    });

    function isEmpty(v) { return v === null || v === undefined || v === ''; }
    function isNonNegInt(v) { return /^\d+$/.test(v); }
    function setErr($inp, msgId, show, msg) {
        var $m = $('#' + msgId);
        if (show) { $inp.addClass('is-invalid'); $m.text(msg || $m.text()).show(); }
        else { $inp.removeClass('is-invalid'); $m.hide().text(''); }
    }

    $('#frmRegistrar').on('submit', function (e) {
        var hasErr = false;

        var $poz = $('#rgNroPozos');
        var $jug = $('#rgNroJug');
        var $sub = $('#rgSubidaCreditos');
        var $cprog = $('#rgCodProg');
        var $tconf = $('#rgTipoConf');

        if (isEmpty($poz.val()) || !isNonNegInt($poz.val())) { setErr($poz, 'msg_rgNroPozos', true, 'Campo obligatorio y numérico (≥ 0).'); hasErr = true; } else setErr($poz, 'msg_rgNroPozos', false);
        if (isEmpty($jug.val()) || !isNonNegInt($jug.val())) { setErr($jug, 'msg_rgNroJug', true, 'Campo obligatorio y numérico (≥ 0).'); hasErr = true; } else setErr($jug, 'msg_rgNroJug', false);
        if (isEmpty($sub.val()) || !isNonNegInt($sub.val())) { setErr($sub, 'msg_rgSubidaCreditos', true, 'Campo obligatorio y numérico (≥ 0).'); hasErr = true; } else setErr($sub, 'msg_rgSubidaCreditos', false);

        if (!isEmpty($cprog.val()) && !isNonNegInt($cprog.val())) { setErr($cprog, 'msg_rgCodProg', true); hasErr = true; } else setErr($cprog, 'msg_rgCodProg', false);
        if (!isEmpty($tconf.val()) && !isNonNegInt($tconf.val())) { setErr($tconf, 'msg_rgTipoConf', true); hasErr = true; } else setErr($tconf, 'msg_rgTipoConf', false);

        if (hasErr) {
            e.preventDefault();
            toastr.error('Corrige los campos marcados para guardar.');
        }
    });

    $(document).on('input change', '[id^=rg]', function () {
        var id = this.id;
        var msgId = 'msg_' + id;
        setErr($(this), msgId, false);
    });

    $(document).on('show.bs.modal', '#modalRegistrarTercero', function () {
        var salaSel = (cboSala && cboSala.value) ? cboSala.value : "0";
        $('#hidCodSala').val(salaSel);
        if (!salaSel || salaSel === "0") { toastr.warning('Seleccione una sala antes de registrar.'); return false; }
        $('#frmRegistrar')[0].reset();
        $('#rgCodProg').val(0);
        $('.val-msg').hide();
        $('.is-invalid').removeClass('is-invalid');
        $('#rgChkActivo').prop('checked', true);
        $('#rgChkEstado').prop('checked', true);
    });
    $(document).on('shown.bs.modal', '#modalRegistrarTercero', function () { $('#rgNombre').trigger('focus'); });

    $(document).on('click', '.btnEditarTercero', function () {
        var id = $(this).data('id');
        var sala = cboSala ? (cboSala.value || '0') : '0';
        if (!sala || sala === '0') { toastr.warning('Seleccione una sala.'); return; }

        $.getJSON(`${basePath}ProgresivoTerceros/Obtener`, { codSala: sala, id: id })
            .done(function (res) {
                if (!res || !res.ok) { toastr.error(res && res.msg ? res.msg : 'No se pudo obtener el registro.'); return; }
                var v = res.value || {};
                $('#edCodSalaProgresivo').val(v.CodSalaProgresivo);
                $('#edCodSala').val(v.CodSala);

                $('#edNombre').val(v.Nombre || '');
                $('#edCodProg').val(typeof v.CodProgresivo === 'number' ? v.CodProgresivo : 0);
                $('#edCodProgWO').val(v.CodProgresivoWO || 0); 

                $('#edNroPozos').val(v.NroPozos || 0);
                $('#edNroJug').val(v.NroJugadores || 0);
                $('#edSubidaCreditos').val(v.SubidaCreditos || 0);
                $('#edTipoConf').val(v.CodTipoConfiguracionProgresivo || 0);

                $('#edChkActivo').prop('checked', !!v.Activo);
                $('#edChkEstado').prop('checked', v.Estado === 1);

                $('.val-msg').hide();
                $('.is-invalid').removeClass('is-invalid');

                $('#modalEditarTercero').modal('show');
            })
            .fail(function () { toastr.error('Error de comunicación.'); });
    });

    $('#frmEditar').on('submit', function (e) {
        var hasErr = false;
        function chkNum(id, msgId) {
            var $i = $('#' + id), v = $i.val();
            if (v !== '' && !/^\d+$/.test(v)) { setErr($i, msgId, true, 'Ingrese un número válido (≥ 0).'); hasErr = true; } else setErr($i, msgId, false);
        }
        chkNum('edCodProg', 'msg_edCodProg');
        chkNum('edNroPozos', 'msg_edNroPozos');
        chkNum('edNroJug', 'msg_edNroJug');
        chkNum('edSubidaCreditos', 'msg_edSubidaCreditos');
        chkNum('edTipoConf', 'msg_edTipoConf');

        if (hasErr) {
            e.preventDefault();
            toastr.error('Corrige los campos marcados para guardar.');
        }
    });

    $(document).on('input change', '[id^=ed]', function () {
        var id = this.id;
        var msgId = 'msg_' + id;
        setErr($(this), msgId, false);
    });

    $(document).on('click', '.btnEliminarTercero', function () {
        var id = $(this).data('id');
        var nombre = $(this).data('nombre') || '';
        var sala = cboSala ? (cboSala.value || '0') : '0';
        var clase = cboClase ? (cboClase.value || 'Terceros') : 'Terceros';

        if (!sala || sala === '0') { toastr.warning('Seleccione una sala.'); return; }

        pendingDelete = { id: id, sala: sala, clase: clase, nombre: nombre };
        $('#delNombre').text(nombre || id);
        $('#modalConfirmDelete').modal('show');
    });

    $('#btnConfirmDelete').on('click', function () {
        if (!pendingDelete) return;

        $('#hidDelId').val(pendingDelete.id);
        $('#hidDelSala').val(pendingDelete.sala);
        $('#hidDelClase').val(pendingDelete.clase);

        $(this).prop('disabled', true);
        $('#frmEliminar').trigger('submit');
        $('#modalConfirmDelete').modal('hide');

        setTimeout(() => $('#btnConfirmDelete').prop('disabled', false), 800);
    });

    $('#modalConfirmDelete').on('hidden.bs.modal', function () {
        pendingDelete = null;
    });

    $(function () {
        if (cboClase) {
            if (defaultClase) $('#cboClase').val(defaultClase).trigger('change');
            else if (!cboClase.value) $('#cboClase').val('Linktek').trigger('change');
        }

        if (cboClase && cboClase.value === 'Terceros') {
            cardTerceros.classList.remove('d-none');
            cardLinktek.classList.add('d-none');
        } else {
            cardLinktek.classList.remove('d-none');
            cardTerceros.classList.add('d-none');
        }

        if (btnBuscar) btnBuscar.addEventListener('click', buscar);

        refreshRegistrarBtn();
        if (cboClase) $(cboClase).on('change', function () {
            if (cboClase.value === 'Terceros') {
                cardTerceros.classList.remove('d-none');
                cardLinktek.classList.add('d-none');
            } else {
                cardLinktek.classList.remove('d-none');
                cardTerceros.classList.add('d-none');
            }
            refreshRegistrarBtn();
            adjustVisibleDT();
        });

        //if ($.fn && $.fn.select2 && $('#cboSala').length) {
        //    var $sala = $('#cboSala');
        //    $sala.select2({
        //        theme: 'bootstrap',
        //        width: '100%',
        //        language: 'es',
        //        placeholder: '-- Seleccione sala --',
        //        allowClear: true,
        //        dropdownParent: $('body')
        //    });

        //    if (codSala && $sala.find('option[value="' + codSala + '"]').length) {
        //        $sala.val(String(codSala)).trigger('change.select2');
        //    }

        //    $sala.on('change', function () { refreshRegistrarBtn(); });
        //}

        initDT(tblLinktek);
        initDT(tblTerceros);
        adjustVisibleDT();
    });
})();


var parametros_detalle_sup;
var parametros_detalle_med;
var parametros_detalle_inf;
var v_mensaje_error = "";
var v_ocurrio_error = 0;
var v_codigo_cabecera = '0';
var validacion_cajas_texto = 0;
ipPublicaG = "";
PorgresivoId = -1;
SalaId = -1;
PorgresivoStr = "";
SalaStr = "";
DataHistoricaCabecera = {};
DataHistoricaPozosActuales = [];
DataHistoricaDetallePozos = [];
let urlsResultado=[]
let consultaPorVpn=false
let ipPrivada
let ipPublicaAlterna
let delay=60000
let timerId = ''

// Registro Progresivo Misterioso
var RPMisterioso = {
    registroProgresivo: {},
    alertaProgresivo: {},
    imagenes: [],
    detalle: {},
    pozos: [],
    detallePozos: [],
    misteriosoDetalle: {},
    progresivoDetalle: {},
    getMWPImagen: function (progresivoImagenID) {
        var self = this
        var imagen = ''

        if (self.imagenes.length > 0) {
            var wpImagen = self.imagenes.find(x => x.ID == progresivoImagenID)

            if (wpImagen != null) {
                imagen = wpImagen.Descripcion
            }
        }

        return imagen
    },
    getDifficultyName: function (dificultad) {
        var keyDifficulty = {
            5: 'Facil',
            6: 'Normal',
            7: 'Dificil'
        }

        return keyDifficulty[dificultad] ? keyDifficulty[dificultad] : ''
    },
    getMisteriosoWP: function () {
        var self = this
        var itemDetalle = {}

        if (self.detalle.ProgresivoID > 0) {
            var listPozos = []

            self.pozos.forEach(function (pozo) {
                var detallePozo = self.detallePozos.find(x => x.ProgresivoID == pozo.ProgresivoID && x.DetalleProgresivoID == pozo.DetalleProgresivoID)

                if (detallePozo.ProgresivoID > 0 && detallePozo.DetalleProgresivoID > 0) {
                    var itemPozo = {
                        ProgresivoID: pozo.ProgresivoID,
                        DetalleProgresivoID: pozo.DetalleProgresivoID,
                        PozoID: pozo.PozoID,
                        Actual: pozo.Actual,
                        Anterior: pozo.Anterior,
                        ActualOculto: pozo.ActualOculto,
                        AnteriorOculto: pozo.AnteriorOculto,
                        Fecha: moment(pozo.Fecha).format('YYYY-MM-DDTHH:mm:ss.SSS'),
                        TipoPozo: pozo.TipoPozo,
                        Estado: pozo.Estado,
                        MontoMin: detallePozo.MontoMin,
                        MontoBase: detallePozo.MontoBase,
                        MontoMax: detallePozo.MontoMax,
                        IncPozo1: detallePozo.IncPozo1,
                        IncPozo2: detallePozo.IncPozo2,
                        MontoOcMin: detallePozo.MontoOcMin,
                        MontoOcMax: detallePozo.MontoOcMax,
                        IncOcPozo1: detallePozo.IncOcPozo1,
                        IncOcPozo2: detallePozo.IncOcPozo2,
                        Parametro: detallePozo.Parametro,
                        Punto: detallePozo.Punto,
                        Prob1: detallePozo.Prob1,
                        Prob2: detallePozo.Prob2,
                        Indice: detallePozo.Indice,
                        EstadoInicial: detallePozo.EstadoInicial,
                        Dificultad: detallePozo.Dificultad,
                        RsJugadores: detallePozo.RsJugadores,
                        RsApuesta: detallePozo.RsApuesta,
                        Dificultad_desc: detallePozo.Dificultad_desc,
                        Estado_desc: detallePozo.Estado_desc,
                        TrigMin: detallePozo.TrigMin,
                        TrigMax: detallePozo.TrigMax,
                        Top: detallePozo.Top,
                        TopAnt: detallePozo.TopAnt,
                        TMin: detallePozo.TMin,
                        TMax: detallePozo.TMax,
                        DetalleId: 0,
                        ProActual: false
                    }

                    listPozos.push(itemPozo)
                }
            })

            itemDetalle = {
                Id: 0,
                ProgresivoID: self.detalle.ProgresivoID,
                NroPozos: self.detalle.NroPozos,
                PorCredito: self.detalle.PorCredito,
                BaseOculto: self.detalle.BaseOculto,
                FechaIni: moment(self.detalle.FechaIni).format('YYYY-MM-DDTHH:mm:ss.SSS'),
                FechaFin: moment(self.detalle.FechaFin).format('YYYY-MM-DDTHH:mm:ss.SSS'),
                NroJugadores: self.detalle.NroJugadores,
                ProgresivoImagenID: self.detalle.ProgresivoImagenID,
                PagoCaja: self.detalle.PagoCaja,
                DuracionPantalla: self.detalle.DuracionPantalla,
                Simbolo: self.detalle.Simbolo,
                Estado: self.detalle.Estado,
                FechaIni_desc: self.detalle.FechaIni_desc,
                FechaFin_desc: self.detalle.FechaFin_desc,
                indice: self.detalle.indice,
                Estado_desc: self.detalle.Estado_desc,
                ProgresivoImagen_desc: self.detalle.ProgresivoImagen_desc,
                RegHistorico: self.detalle.RegHistorico,
                ProgresivoImagenNombre: self.getMWPImagen(self.detalle.ProgresivoImagenID),
                ProgresivoIDOnline: parseInt(PorgresivoId),
                ProgresivoNombreOnline: PorgresivoStr,
                SalaId: parseInt(SalaId),
                FechaRegistro: '',
                ProActual: false,
                Pozos: listPozos
            }
        }

        return itemDetalle
    },
    setDetalles: function () {
        this.misteriosoDetalle = this.getMisteriosoWP()
        this.progresivoDetalle = this.getMisteriosoWP()
    },
    getChangesDetalle: function () {
        var itemDetalle = {
            Simbolo: $('#txtMoneda').val(),
            PagoCaja: $('#cboLugarPago').val() == '1' ? true : false,
            DuracionPantalla: parseInt($('#txtDuracion').val()),
            NroJugadores: parseInt($('#txtNumJugadores').val()),
            NroPozos: parseInt($('#txtNroPozos').val()),
            ProgresivoImagenID: parseInt($('#cboImagen').val()),
            Estado: parseInt($('#cboEstado').val()),
            BaseOculto: $('#chkPozoOculto').is(':checked') ? true : false,
            RegHistorico: $('#chkRegHistorico').is(':checked') ? true : false
        }

        return itemDetalle
    },
    getChangesPozo: function (pozo) {
        var checksPozo = {
            1: 'chkPozoSuperior',
            2: 'chkPozoMedio',
            3: 'chkPozoInferior'
        }

        var keysPozo = {
            1: 'PS',
            2: 'PM',
            3: 'PI'
        }

        var checksPozoActual = {
            1: 'chkModificarPozoSuperior',
            2: 'chkModificarPozoMedio',
            3: 'chkModificarPozoInferior'
        }

        var itemPozo = {
            MontoBase: pozo.MontoBase,
            MontoMin: pozo.MontoMin,
            MontoMax: pozo.MontoMax,
            IncPozo1: pozo.IncPozo1,
            RsApuesta: pozo.RsApuesta,
            RsJugadores: pozo.RsJugadores,
            MontoOcMin: pozo.MontoOcMin,
            MontoOcMax: pozo.MontoOcMax,
            IncOcPozo1: pozo.IncOcPozo1,
            Dificultad: pozo.Dificultad,
            Actual: pozo.Actual,
            CheckPozo: false,
            CheckPozoActual: false
        }

        var checkPozo = checksPozo[pozo.TipoPozo] ? checksPozo[pozo.TipoPozo] : ''

        if ($(`#${checkPozo}`).is(':checked')) {
            var keyPozo = keysPozo[pozo.TipoPozo] ? keysPozo[pozo.TipoPozo] : ''

            itemPozo.MontoBase = parseFloat($(`#txt${keyPozo}PremioBase`).val())
            itemPozo.MontoMin = parseFloat($(`#txt${keyPozo}PremioMinimo`).val())
            itemPozo.MontoMax = parseFloat($(`#txt${keyPozo}PremioMaximo`).val())
            itemPozo.IncPozo1 = parseFloat($(`#txt${keyPozo}IncPozoNormal`).val())
            itemPozo.RsApuesta = parseInt($(`#txt${keyPozo}RsApuesta`).val())
            itemPozo.RsJugadores = parseInt($(`#txt${keyPozo}RsJugadores`).val())
            itemPozo.MontoOcMin = parseFloat($(`#txt${keyPozo}MontoOcultoMin`).val())
            itemPozo.MontoOcMax = parseFloat($(`#txt${keyPozo}MontoOcultoMax`).val())
            itemPozo.IncOcPozo1 = parseFloat($(`#txt${keyPozo}IncPozoOculto`).val())
            itemPozo.Dificultad = parseInt($(`#cbo${keyPozo}Dificultad`).val())

            var keyPozoActual = checksPozoActual[pozo.TipoPozo] ? checksPozoActual[pozo.TipoPozo] : ''

            if ($(`#${keyPozoActual}`).is(':checked')) {
                itemPozo.Actual = parseFloat($(`#txt${keyPozo}PozoActual`).val())
            }

            itemPozo.CheckPozo = $(`#${checkPozo}`).is(':checked') ? true : false
            itemPozo.CheckPozoActual = $(`#${keyPozoActual}`).is(':checked') ? true : false
        }

        return itemPozo
    },
    changesProgresivoDetalle: function () {
        var self = this
        var changeDetalle = self.getChangesDetalle()
        
        self.progresivoDetalle.Simbolo = changeDetalle.Simbolo
        self.progresivoDetalle.PagoCaja = changeDetalle.PagoCaja
        self.progresivoDetalle.DuracionPantalla = changeDetalle.DuracionPantalla
        self.progresivoDetalle.NroJugadores = changeDetalle.NroJugadores
        self.progresivoDetalle.NroPozos = changeDetalle.NroPozos
        self.progresivoDetalle.ProgresivoImagenID = changeDetalle.ProgresivoImagenID
        self.progresivoDetalle.Estado = changeDetalle.Estado
        self.progresivoDetalle.BaseOculto = changeDetalle.BaseOculto
        self.progresivoDetalle.RegHistorico = changeDetalle.RegHistorico
        self.progresivoDetalle.ProActual = true

        self.progresivoDetalle.ProgresivoImagenNombre = self.getMWPImagen(changeDetalle.ProgresivoImagenID)

        self.progresivoDetalle.Pozos.forEach(function (pozo) {
            var changePozo = self.getChangesPozo(pozo)

            pozo.MontoBase = changePozo.MontoBase
            pozo.MontoMin = changePozo.MontoMin
            pozo.MontoMax = changePozo.MontoMax
            pozo.IncPozo1 = changePozo.IncPozo1
            pozo.RsApuesta = changePozo.RsApuesta
            pozo.RsJugadores = changePozo.RsJugadores
            pozo.MontoOcMin = changePozo.MontoOcMin
            pozo.MontoOcMax = changePozo.MontoOcMax
            pozo.IncOcPozo1 = changePozo.IncOcPozo1
            pozo.Dificultad = changePozo.Dificultad
            pozo.Actual = changePozo.Actual
            pozo.ProActual = true
            pozo.CheckPozo = changePozo.CheckPozo
            pozo.CheckPozoActual = changePozo.CheckPozoActual

            pozo.Dificultad_desc = self.getDifficultyName(changePozo.Dificultad)
        })
    },
    dataProgresivo: function () {
        this.registroProgresivo = this.progresivoDetalle
        this.alertaProgresivo = {
            SalaId: parseInt(SalaId),
            SalaNombre: SalaStr,
            ProgresivoNombre: PorgresivoStr,
            Descripcion: 'Cambios en la configuración desde IAS',
            Detalle: [
                this.misteriosoDetalle,
                this.progresivoDetalle
            ]
        }
    },
    buildDetalles: function () {
        this.setDetalles()
        this.changesProgresivoDetalle()
        this.dataProgresivo()
    },
    sendChangesToServer: function () {
        this.buildDetalles()

        var data = {
            registroProgresivo: this.registroProgresivo,
            alertaProgresivo: this.alertaProgresivo
        }

        ajaxhr = $.ajax({
            type: "POST",
            url: `${basePath}/RegistroProgresivo/GuardarRegistroProgresivo`,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            cache: false,
            beforeSend: function () {
                $.LoadingOverlay("show")
            },
            success: function (response) {
                if (response.success) {
                    console.log(response.message)
                }
            },
            complete: function () {
                AbortRequest.close()
                $.LoadingOverlay("hide")
            }
        })
        AbortRequest.open()
    }
}
// Registro Progresivo Misterioso

// Historial Registro Progresivo
var HistorialRP = {
    pozoTypes: [
        {
            key: 1,
            name: 'Pozo Superior'
        },
        {
            key: 2,
            name: 'Pozo Medio'
        },
        {
            key: 3,
            name: 'Pozo Inferior'
        }
    ],
    pozoIndicators: [
        {
            key: 1,
            name: 'Premio base'
        },
        {
            key: 2,
            name: 'Premio mínimo'
        },
        {
            key: 3,
            name: 'Premio máximo'
        },
        {
            key: 4,
            name: 'Inc. pozo (Ej. 0.01 = 1%)'
        },
        {
            key: 5,
            name: 'Restricción de Apuesta (Créditos)'
        },
        {
            key: 6,
            name: 'Nro de Jugadores'
        },
        {
            key: 7,
            name: 'Monto oculto mínimo'
        },
        {
            key: 8,
            name: 'Monto oculto máximo'
        },
        {
            key: 9,
            name: 'Inc. pozo oculto (Ej. 0.01 = 1%)'
        },
        {
            key: 10,
            name: 'Dificultad'
        },
        {
            key: 11,
            name: 'Pozo actual'
        }
    ],
    selectedRP: {},
    selectedRO: {},
    getLastRegistroProgresivo: function (salaId, progresivoId) {
        var data = {
            salaId,
            progresivoId
        }

        ajaxhr =  $.ajax({
            type: "POST",
            url: `${basePath}/RegistroProgresivo/ObtenerUltimoRegistroProgresivo`,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            cache: false,
            beforeSend: function () {
                $.LoadingOverlay("show")
            },
            complete: function () {
                AbortRequest.close()
                $.LoadingOverlay("hide")
            }
        })
        AbortRequest.open()
        return ajaxhr
    },
    getRegistrosProgresivos: function (salaId, progresivoId) {
        var data = {
            salaId,
            progresivoId
        }

        ajaxhr = $.ajax({
            type: "POST",
            url: `${basePath}/RegistroProgresivo/ListarRegistrosProgresivos`,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            cache: false,
            beforeSend: function () {
                $.LoadingOverlay("show")
            },
            complete: function () {
                AbortRequest.close()
                $.LoadingOverlay("hide")
            }
        })
        AbortRequest.open()
        return ajaxhr
    },
    getRegistroProgresivo: function (salaId, detalleId) {
        var data = {
            salaId,
            detalleId
        }

        ajaxhr = $.ajax({
            type: "POST",
            url: `${basePath}/RegistroProgresivo/ObtenerRegistroProgresivo`,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            cache: false,
            beforeSend: function () {
                $.LoadingOverlay("show")
            },
            complete: function () {
                AbortRequest.close()
                $.LoadingOverlay("hide")
            }
        })
        AbortRequest.open()
        return ajaxhr
    },
    setRegistroProgresivo: function (item) {
        this.selectedRP = item
    },
    renderRegistroProgresivo: function (item) {
        var self = this

        var hrpd_sala = $('#hrpd_sala')
        var hrpd_progresivo = $('#hrpd_progresivo')
        var hrpd_fecharegistro = $('#hrpd_fecharegistro')
        var hrpd_moneda = $('#hrpd_moneda')
        var hrpd_lugarpago = $('#hrpd_lugarpago')
        var hrpd_duracionpantalla = $('#hrpd_duracionpantalla')
        var hrpd_nrojugadores = $('#hrpd_nrojugadores')
        var hrpd_nropozos = $('#hrpd_nropozos')
        var hrpd_imagen = $('#hrpd_imagen')
        var hrpd_estado = $('#hrpd_estado')
        var hrpd_tablepozos = $('#hrpd_tablepozos')
        var tableContent = ''
        var theadContent = ''
        var tbodyContent = ''
        var thPozos = ''
        var tdPozos = ''
        var arrayIndicators = self.pozoIndicators

        hrpd_sala.html(SalaStr)
        hrpd_progresivo.html(item.ProgresivoNombreOnline)
        hrpd_fecharegistro.html(moment(item.FechaRegistro).format('DD/MM/YYYY HH:mm:ss'))
        hrpd_moneda.html(item.Simbolo)
        hrpd_lugarpago.html(item.PagoCaja ? '' : 'Pagos en maquina')
        hrpd_duracionpantalla.html(item.DuracionPantalla)
        hrpd_nrojugadores.html(item.NroJugadores)
        hrpd_nropozos.html(item.NroPozos)
        hrpd_imagen.html(item.ProgresivoImagenNombre)
        hrpd_estado.html(item.Estado ? 'Activo' : 'Inactivo')

        item.Pozos.forEach(function (pozo) {
            thPozos += `
            <th align="center">${self.pozoTypes.find(x => x.key == pozo.TipoPozo)?.name ?? ''}</th>
            `
        })

        arrayIndicators.forEach(function (indicator) {

            var indicatorPozos = []

            item.Pozos.forEach(function (pozo) {
                var indicatorPozoValue = null

                switch (indicator.key) {
                    case 1:
                        indicatorPozoValue = pozo.MontoBase

                        break;
                    case 2:
                        indicatorPozoValue = pozo.MontoMin

                        break;
                    case 3:
                        indicatorPozoValue = pozo.MontoMax

                        break;
                    case 4:
                        indicatorPozoValue = pozo.IncPozo1

                        break;
                    case 5:
                        indicatorPozoValue = pozo.RsApuesta

                        break;
                    case 6:
                        indicatorPozoValue = pozo.RsJugadores

                        break;
                    case 7:
                        indicatorPozoValue = pozo.MontoOcMin

                        break;
                    case 8:
                        indicatorPozoValue = pozo.MontoOcMax

                        break;
                    case 9:
                        indicatorPozoValue = pozo.IncOcPozo1

                        break;
                    case 10:
                        indicatorPozoValue = pozo.Dificultad_desc

                        break;
                    case 11:
                        indicatorPozoValue = pozo.Actual

                        break;
                }

                indicatorPozos.push({
                    indicator: indicator.key,
                    pozo: pozo.TipoPozo,
                    value: indicatorPozoValue
                })
            })

            indicator.pozos = indicatorPozos
        })

        theadContent = `
        <tr>
            <th></th>
            ${thPozos}
        </tr>
        `

        arrayIndicators.forEach(function (indicator) {

            tdPozos = ''

            indicator.pozos.forEach(function (pozo) {
                tdPozos += `
                <td align="center">${pozo.value ?? ''}</td>
                `
            })
            
            tbodyContent += `
            <tr>
                <td>${indicator.name}</td>
                ${tdPozos}
            </tr>
            `
        })

        tableContent = `
        <div class="table-responsive">
            <table class="table-history">
                <thead>
                    ${theadContent}
                </thead>
                <tbody>
                    ${tbodyContent}
                </tbody>
            </table>
        </div>
        `

        hrpd_tablepozos.html(tableContent)
    },
    renderRegistrosProgresivos: function (items) {
        $("#table_registros_progresivos").DataTable({
            "bDestroy": true,
            "bSort": true,
            "scrollCollapse": true,
            "scrollX": false,
            "paging": true,
            "autoWidth": false,
            "bProcessing": true,
            "bDeferRender": true,
            "aaSorting": [],
            data: items,
            columns: [
                {
                    data: "FechaRegistro",
                    title: "Fecha Registro",
                    render: function (value) {
                        return moment(value).format('DD/MM/YYYY HH:mm:ss')
                    }
                },
                {
                    data: null,
                    title: "Acción",
                    render: function (item) {
                        var buttons = `
                        <button type="button" class="btn btn-xs btn-success view_registro_progresivo" data-id="${item.Id}" data-room="${item.SalaId}">Ver</button>
                        `

                        return buttons
                    },
                    sortable: false,
                    searchable: false
                }
            ],
            columnDefs: [
                {
                    targets: "_all",
                    className: "text-center"
                }
            ]
        })
    },
    clearRegistroProgresivo: function () {
        var hrpd_sala = $('#hrpd_sala')
        var hrpd_progresivo = $('#hrpd_progresivo')
        var hrpd_fecharegistro = $('#hrpd_fecharegistro')
        var hrpd_moneda = $('#hrpd_moneda')
        var hrpd_lugarpago = $('#hrpd_lugarpago')
        var hrpd_duracionpantalla = $('#hrpd_duracionpantalla')
        var hrpd_nrojugadores = $('#hrpd_nrojugadores')
        var hrpd_nropozos = $('#hrpd_nropozos')
        var hrpd_imagen = $('#hrpd_imagen')
        var hrpd_estado = $('#hrpd_estado')
        var hrpd_tablepozos = $('#hrpd_tablepozos')

        hrpd_sala.html('')
        hrpd_progresivo.html('')
        hrpd_fecharegistro.html('')
        hrpd_moneda.html('')
        hrpd_lugarpago.html('')
        hrpd_duracionpantalla.html('')
        hrpd_nrojugadores.html('')
        hrpd_nropozos.html('')
        hrpd_imagen.html('')
        hrpd_estado.html('')
        hrpd_tablepozos.html('')
    },
    getLastRegistroOnline: function (salaId, progresivoId) {
        var data = {
            salaId,
            progresivoId
        }

        ajaxhr =  $.ajax({
            type: "POST",
            url: `${basePath}/RegistroProgresivo/ObtenerUltimoRegistroOnline`,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            cache: false,
            beforeSend: function () {
                $.LoadingOverlay("show")
            },
            complete: function () {
                AbortRequest.close()
                $.LoadingOverlay("hide")
            }
        })
        AbortRequest.open()
        return ajaxhr
    },
    getRegistrosOnline: function (salaId, progresivoId) {
        var data = {
            salaId,
            progresivoId
        }

        ajaxhr = $.ajax({
            type: "POST",
            url: `${basePath}/RegistroProgresivo/ListarRegistrosOnline`,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            cache: false,
            beforeSend: function () {
                $.LoadingOverlay("show")
            },
            complete: function () {
                AbortRequest.close()
                $.LoadingOverlay("hide")
            }
        })
        AbortRequest.open()
        return ajaxhr
    },
    getRegistroOnline: function (salaId, detalleId) {
        var data = {
            salaId,
            detalleId
        }

        ajaxhr = $.ajax({
            type: "POST",
            url: `${basePath}/RegistroProgresivo/ObtenerRegistroOnline`,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            cache: false,
            beforeSend: function () {
                $.LoadingOverlay("show")
            },
            complete: function () {
                AbortRequest.close()
                $.LoadingOverlay("hide")
            }
        })
        AbortRequest.open()
        return ajaxhr
    },
    setRegistroOnline: function (item) {
        this.selectedRO = item
    },
    renderRegistroOnline: function (item) {
        var self = this

        var hrod_sala = $('#hrod_sala')
        var hrod_progresivo = $('#hrod_progresivo')
        var hrod_fecharegistro = $('#hrod_fecharegistro')
        var hrod_moneda = $('#hrod_moneda')
        var hrod_lugarpago = $('#hrod_lugarpago')
        var hrod_duracionpantalla = $('#hrod_duracionpantalla')
        var hrod_nrojugadores = $('#hrod_nrojugadores')
        var hrod_nropozos = $('#hrod_nropozos')
        var hrod_imagen = $('#hrod_imagen')
        var hrod_estado = $('#hrod_estado')
        var hrod_tablepozos = $('#hrod_tablepozos')
        var tableContent = ''
        var theadContent = ''
        var tbodyContent = ''
        var thPozos = ''
        var tdPozos = ''
        var arrayIndicators = self.pozoIndicators

        hrod_sala.html(SalaStr)
        hrod_progresivo.html(item.ProgresivoNombreOnline)
        hrod_fecharegistro.html(moment(item.FechaRegistro).format('DD/MM/YYYY HH:mm:ss'))
        hrod_moneda.html(item.Simbolo)
        hrod_lugarpago.html(item.PagoCaja ? '' : 'Pagos en maquina')
        hrod_duracionpantalla.html(item.DuracionPantalla)
        hrod_nrojugadores.html(item.NroJugadores)
        hrod_nropozos.html(item.NroPozos)
        hrod_imagen.html(item.ProgresivoImagenNombre)
        hrod_estado.html(item.Estado ? 'Activo' : 'Inactivo')

        item.Pozos.forEach(function (pozo) {
            thPozos += `
            <th align="center">${self.pozoTypes.find(x => x.key == pozo.TipoPozo)?.name ?? ''}</th>
            `
        })

        arrayIndicators.forEach(function (indicator) {

            var indicatorPozos = []

            item.Pozos.forEach(function (pozo) {
                var indicatorPozoValue = null

                switch (indicator.key) {
                    case 1:
                        indicatorPozoValue = pozo.MontoBase

                        break;
                    case 2:
                        indicatorPozoValue = pozo.MontoMin

                        break;
                    case 3:
                        indicatorPozoValue = pozo.MontoMax

                        break;
                    case 4:
                        indicatorPozoValue = pozo.IncPozo1

                        break;
                    case 5:
                        indicatorPozoValue = pozo.RsApuesta

                        break;
                    case 6:
                        indicatorPozoValue = pozo.RsJugadores

                        break;
                    case 7:
                        indicatorPozoValue = pozo.MontoOcMin

                        break;
                    case 8:
                        indicatorPozoValue = pozo.MontoOcMax

                        break;
                    case 9:
                        indicatorPozoValue = pozo.IncOcPozo1

                        break;
                    case 10:
                        indicatorPozoValue = pozo.Dificultad_desc

                        break;
                    case 11:
                        indicatorPozoValue = pozo.Actual

                        break;
                }

                indicatorPozos.push({
                    indicator: indicator.key,
                    pozo: pozo.TipoPozo,
                    value: indicatorPozoValue
                })
            })

            indicator.pozos = indicatorPozos
        })

        theadContent = `
        <tr>
            <th></th>
            ${thPozos}
        </tr>
        `

        arrayIndicators.forEach(function (indicator) {

            tdPozos = ''

            indicator.pozos.forEach(function (pozo) {
                tdPozos += `
                <td align="center">${pozo.value ?? ''}</td>
                `
            })

            tbodyContent += `
            <tr>
                <td>${indicator.name}</td>
                ${tdPozos}
            </tr>
            `
        })

        tableContent = `
        <div class="table-responsive">
            <table class="table-history">
                <thead>
                    ${theadContent}
                </thead>
                <tbody>
                    ${tbodyContent}
                </tbody>
            </table>
        </div>
        `

        hrod_tablepozos.html(tableContent)
    },
    renderRegistrosOnline: function (items) {
        $("#table_registros_online").DataTable({
            "bDestroy": true,
            "bSort": true,
            "scrollCollapse": true,
            "scrollX": false,
            "paging": true,
            "autoWidth": false,
            "bProcessing": true,
            "bDeferRender": true,
            "aaSorting": [],
            data: items,
            columns: [
                {
                    data: "FechaRegistro",
                    title: "Fecha Registro",
                    render: function (value) {
                        return moment(value).format('DD/MM/YYYY HH:mm:ss')
                    }
                },
                {
                    data: null,
                    title: "Acción",
                    render: function (item) {
                        var buttons = `
                        <button type="button" class="btn btn-xs btn-success view_registro_online" data-id="${item.Id}" data-room="${item.SalaId}">Ver</button>
                        `

                        return buttons
                    },
                    sortable: false,
                    searchable: false
                }
            ],
            columnDefs: [
                {
                    targets: "_all",
                    className: "text-center"
                }
            ]
        })
    },
    clearRegistroOnline: function () {
        var hrod_sala = $('#hrod_sala')
        var hrod_progresivo = $('#hrod_progresivo')
        var hrod_fecharegistro = $('#hrod_fecharegistro')
        var hrod_moneda = $('#hrod_moneda')
        var hrod_lugarpago = $('#hrod_lugarpago')
        var hrod_duracionpantalla = $('#hrod_duracionpantalla')
        var hrod_nrojugadores = $('#hrod_nrojugadores')
        var hrod_nropozos = $('#hrod_nropozos')
        var hrod_imagen = $('#hrod_imagen')
        var hrod_estado = $('#hrod_estado')
        var hrod_tablepozos = $('#hrod_tablepozos')

        hrod_sala.html('')
        hrod_progresivo.html('')
        hrod_fecharegistro.html('')
        hrod_moneda.html('')
        hrod_lugarpago.html('')
        hrod_duracionpantalla.html('')
        hrod_nrojugadores.html('')
        hrod_nropozos.html('')
        hrod_imagen.html('')
        hrod_estado.html('')
        hrod_tablepozos.html('')
    },
    replaceRegistroProgresivo: function (item) {

        var comboImagen = $("#cboImagen")

        comboImagen.html('')
        LimpiarCabecera()
        LimpiarDetalle()

        var keysPozos = {
            1: 'PS',
            2: 'PM',
            3: 'PI'
        }

        if (item) {
            comboImagen.html(`<option value="${item.ProgresivoImagenID}">${item.ProgresivoImagenNombre}</option>`)

            $('#hid_cod_cab_prog').val(item.ProgresivoID)

            $('#txtMoneda').val(item.Simbolo)
            $('#cboLugarPago').val(item.PagoCaja ? 1 : 0)
            $('#txtDuracion').val(item.DuracionPantalla)
            $('#txtNumJugadores').val(item.NroJugadores)
            $('#txtNroPozos').val(item.NroPozos)
            $('#cboImagen').val(item.ProgresivoImagenID)
            $('#cboEstado').val(item.Estado)
            
            if (item.BaseOculto) {
                $('#chkPozoOculto').iCheck('check')

                $('#txtPSPremioBase').attr('disabled', 'disabled')
                $('#txtPMPremioBase').attr('disabled', 'disabled')
                $('#txtPIPremioBase').attr('disabled', 'disabled')

            } else {
                $('#chkPozoOculto').iCheck('uncheck')

                if ($('#chkPozoSuperior').is(":checked")) {
                    $('#txtPSPremioBase').removeAttr('disabled')
                }

                if ($('#chkPozoMedio').is(":checked")) {
                    $('#txtPMPremioBase').removeAttr('disabled')
                }

                if ($('#chkPozoInferior').is(":checked")) {
                    $('#txtPIPremioBase').removeAttr('disabled')
                }
            }

            if (item.RegHistorico) {
                $('#chkRegHistorico').iCheck('check')
            } else {
                $('#chkRegHistorico').iCheck('uncheck')
            }

            item.Pozos.forEach(function (pozo) {

                var keyPozo = keysPozos[pozo.TipoPozo] ? keysPozos[pozo.TipoPozo] : ''

                $(`#txt${keyPozo}PremioBase`).val(pozo.MontoBase)
                $(`#txt${keyPozo}PremioMinimo`).val(pozo.MontoMin)
                $(`#txt${keyPozo}PremioMaximo`).val(pozo.MontoMax)
                $(`#txt${keyPozo}IncPozoNormal`).val(pozo.IncPozo1)
                $(`#txt${keyPozo}RsApuesta`).val(pozo.RsApuesta)
                $(`#txt${keyPozo}RsJugadores`).val(pozo.RsJugadores)
                $(`#txt${keyPozo}MontoOcultoMin`).val(pozo.MontoOcMin)
                $(`#txt${keyPozo}MontoOcultoMax`).val(pozo.MontoOcMax)
                $(`#txt${keyPozo}IncPozoOculto`).val(pozo.IncOcPozo1)
                $(`#cbo${keyPozo}Dificultad`).val(pozo.Dificultad)
                $(`#txt${keyPozo}PozoActual`).val(pozo.Actual)

                if (pozo.TipoPozo == 1) {
                    $('#hid_cod_det_prog_sup').val(pozo.DetalleProgresivoID)
                    $('#valor_pozo_superior_actual').val(pozo.Actual)

                    ManipularCajasTextoPozoSuperior(pozo.Estado)

                    $('#chkPozoSuperior').prop("checked", (pozo.Estado == 1 ? true : false))
                }

                if (pozo.TipoPozo == 2) {
                    $('#hid_cod_det_prog_med').val(pozo.DetalleProgresivoID)
                    $('#valor_pozo_medio_actual').val(pozo.Actual)

                    ManipularCajasTextoPozoMedio(pozo.Estado)

                    $('#chkPozoMedio').prop('checked', (pozo.Estado == 1 ? true : false))
                }

                if (pozo.TipoPozo == 3) {
                    $('#hid_cod_det_prog_inf').val(pozo.DetalleProgresivoID)
                    $('#valor_pozo_inferior_actual').val(pozo.Actual)

                    ManipularCajasTextoPozoInferior(pozo.Estado)

                    $('#chkPozoInferior').prop('checked', (pozo.Estado == 1 ? true : false))
                }

            })
        }
    }
}
// Historial Registro Progresivo

function sleepFor(sleepDuration) {
    var now = new Date().getTime();
    while (new Date().getTime() < now + sleepDuration) { /* do nothing */ }
}
function VerificarLocalStorage(){
    if(typeof(Storage)!=="undefined"){
        //POZO SUPERIOR
        if(!localStorage.getItem('chkModificarPozoSuperior')){
            localStorage.setItem('chkModificarPozoSuperior','false');
           document.getElementById('chkModificarPozoSuperior').checked=false;
        }
        else{
            var checked=localStorage.getItem('chkModificarPozoSuperior');
            if(checked==='true'){
                document.getElementById('chkModificarPozoSuperior').checked=true;
            }
            else{
                document.getElementById('chkModificarPozoSuperior').checked=false;
            }
        }
        //POZO MEDIO
        if(!localStorage.getItem('chkModificarPozoMedio')){
            localStorage.setItem('chkModificarPozoMedio','false');
            document.getElementById('chkModificarPozoMedio').checked=false;
        }
        else{
            var checked=localStorage.getItem('chkModificarPozoMedio');
            if(checked==='true'){
                document.getElementById('chkModificarPozoMedio').checked=true;
            }
            else{
                document.getElementById('chkModificarPozoMedio').checked=false;
            }
        }
        //POZO INFERIOR
        if(!localStorage.getItem('chkModificarPozoInferior')){
            localStorage.setItem('chkModificarPozoInferior','false');
            document.getElementById('chkModificarPozoInferior').checked =false;
        }
        else{
            var checked=localStorage.getItem('chkModificarPozoInferior');
            if(checked==='true'){
                document.getElementById('chkModificarPozoInferior').checked=true;
            }
            else{
                document.getElementById('chkModificarPozoInferior').checked=false;
            }
        }
    }
    else{
        alert("Este Navegador no soporta LocalStorage, por favor Actualizarlo");
    }
}
VistaAuditoria("Progresivo/ProgresivoRegistroVista", "VISTA",0,"",1);
$(document).ready(function () {
    getPingSalas().then(response=>{
        urlsResultado=response
    })
    clearTimeout(timerId)
    timerId = setTimeout(function request() {
        getPingSalas().then(result=>{
            urlsResultado=result
        })
        timerId = setTimeout(request, delay);
    }, delay)
    VerificarLocalStorage();
    
    obtenerListaSalas().then(result=>{
        if(result.data){
            renderSelectSalas(result.data)
            getPingSalas().then(response=>{
                urlsResultado=response
            })
        }
    })


    VerificarLocalStorage();
    $('#txtPSPremioBase').keyup(function () {
        if($("#chkModificarPozoSuperior").is(':checked')){
            $('#txtPSPozoActual').val($('#txtPSPremioBase').val());
        }
    });
    $('#txtPMPremioBase').keyup(function () {
        if($("#chkModificarPozoMedio").is(':checked')){
            $('#txtPMPozoActual').val($('#txtPMPremioBase').val());
        }
    });
    $('#txtPIPremioBase').keyup(function () {
        if($("#chkModificarPozoInferior").is(':checked')){
            $('#txtPIPozoActual').val($('#txtPIPremioBase').val());
        }
    });
    function TotalProgresivo() {
        valores = $('input[name=pozocheck]:checked').length;
        $('#txtNroPozos').val(valores);
    }

    $('#btnGrabar').click(function (e) {
        e.preventDefault()
        if ($("#cboSala").val() == 0) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if ($("#cboProgresivo").val() == "") {
            toastr.error("Seleccione Progresivo", "Mensaje Servidor");
            return false;
        }
        var acumVal = 0;
        if ($('#chkPozoSuperior').is(":checked")) {
            acumVal++;
        }
        if ($('#chkPozoMedio').is(":checked")) {
            acumVal++;
        }
        if ($('#chkPozoInferior').is(":checked")) {
            acumVal++;
        }
        if (acumVal == 0) {
            toastr.error("Habilite al menos un pozo para grabar los datos", "Mensaje Servidor");
            return false;
        }
        ValidarCajasTexto();
        return false;
    });

    $('#chkPozoOculto').click(function () {
        if ($('#chkPozoSuperior').is(":checked")) {
            if (!$('#chkPozoOculto').is(":checked")) {
                $('#txtPSPremioBase').removeAttr('disabled');
            }
            else {
                $('#txtPSPremioBase').attr('disabled', 'disabled');
            }
        }
        if ($('#chkPozoMedio').is(":checked")) {
            if (!$('#chkPozoOculto').is(":checked")) {
                $('#txtPMPremioBase').removeAttr('disabled');
            }
            else {
                $('#txtPMPremioBase').attr('disabled', 'disabled');
            }
        }
        if ($('#chkPozoInferior').is(":checked")) {
            if (!$('#chkPozoOculto').is(":checked")) {
                $('#txtPIPremioBase').removeAttr('disabled');
            }
            else {
                $('#txtPIPremioBase').attr('disabled', 'disabled');
            }
        }
    });

    $('#chkPozoSuperior').click(function () {
        if ($('#chkPozoSuperior').is(":checked")) {
            ManipularCajasTextoPozoSuperior(1);
        }
        else {
            ManipularCajasTextoPozoSuperior(0);
        }
        TotalProgresivo();
    });

    $('#chkPozoMedio').click(function () {
        if ($('#chkPozoMedio').is(":checked")) {
            ManipularCajasTextoPozoMedio(1);
        }
        else {
            ManipularCajasTextoPozoMedio(0);
        }
        TotalProgresivo();
    });

    $('#chkPozoInferior').click(function () {
        if ($('#chkPozoInferior').is(":checked")) {
            ManipularCajasTextoPozoInferior(1);
        }
        else {
            ManipularCajasTextoPozoInferior(0);
        }
        TotalProgresivo();
    });

    //--------------------------------------
    $(document).on('change', '#cboSala', function (e) {

        SalaId = $(this).find('option:selected').attr('data-id')
        SalaStr = $(this).find('option:selected').text().trim()
        PorgresivoId = -1
        PorgresivoStr = ''

        $('#cboProgresivo').html('<option value="">--Seleccione--</option>')

        if ($("#cboSala").val() == 0) {
            SalaId = -1
            SalaStr = ''

            return false
        }

        if(urlsResultado.length==0){
            toastr.warning("Inténtelo de nuevo en unos momentos",'Mensaje Servidor')
            $("#cboSala").val('0').trigger('change')
            return false
        }
      
        toastr.remove()

        var ipPublica = $(this).val();
        ipPublicaG = ipPublica;
        ipPrivada=$("#cboSala option:selected").data('ipprivada')
        let puertoServicio=$("#cboSala option:selected").data('puertoservicio')
        ipPrivada=ipPrivada+':'+puertoServicio
        let uri=getUrl(ipPublica)
        const obj=urlsResultado.find(item=>item.uri==ipPublica)
        if(uri&&obj.respuesta){
            let urlPublica=ipPublica+'/servicio/listadoprogresivos'
            consultaPorVpn=false
            llenarSelectAPIProgresivo(urlPublica, {}, "cboProgresivo", "WEB_PrgID", "WEB_Nombre");
        }
        else{
            consultaPorVpn=true
            // ipPublicaAlterna=urlsResultado.find(x=>x.respuesta)
            //ipPublicaAlterna=getUrl("http://localhost:9895")
            ipPublicaAlterna = getUrl("http://190.187.44.222:9895")
            let urlPrivada=ipPrivada+'/servicio/listadoprogresivos'
            let urlPublica=ipPublicaAlterna+"/servicio/listadoprogresivosVpn"
            getProgresivosVpn(urlPrivada,urlPublica).then(response=>{
                if(response.length>0){
                    renderSelectProgresivos(response)
                }
            })
        }
    });
    $(document).on('change', '#cboProgresivo', function (e) {

        SalaId = $('#cboSala').find('option:selected').data('id')
        SalaStr = $('#cboSala').find('option:selected').text().trim()
        PorgresivoId = $(this).val()
        PorgresivoStr = $(this).find('option:selected').text().trim()
        
        if ($("#cboProgresivo").val() == "") {
            PorgresivoId = -1
            PorgresivoStr = ''

            toastr.error(result, "Seleccione Progresivo");
        }

        if(consultaPorVpn){
            ObtenerListaImagenesVpn()
        }
        else{
            ObtenerListaImagenes()

        }
        LimpiarCabecera();
        LimpiarDetalle();

        ManipularCajasTextoPozoSuperior(0);
        ManipularCajasTextoPozoMedio(0);
        ManipularCajasTextoPozoInferior(0);  

       
    });

    $(".checkbox-form .icheck-square").iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-red',
        increaseArea: '20%' // optional
    });   
    $(".icheck-line input").each(function () {
        var self = $(this),
            label = self.next(),
            label_text = label.text();

        label.remove();
        self.iCheck({
            checkboxClass: 'icheckbox_line-blue',
            radioClass: 'iradio_line-purple',
            insert: '<div class="icheck_line-icon"></div>' + label_text
        });
    });

    // Historial Registro Progresivo
    $(document).on('click', '#btn_historial_progresivo', function (event) {
        event.preventDefault()

        var salaId = parseInt(SalaId)
        var progresivoId = parseInt(PorgresivoId)

        toastr.remove()

        if (!salaId || salaId <= 0) {
            toastr.warning("Seleccione una sala")

            return false
        }

        if (!progresivoId || progresivoId <= 0) {
            toastr.warning("Seleccione un progresivo")

            return false
        }

        HistorialRP.clearRegistroProgresivo()
        
        HistorialRP.getLastRegistroProgresivo(salaId, progresivoId).done(function (response) {
            if (response.success) {
                HistorialRP.setRegistroProgresivo(response.data)
                HistorialRP.renderRegistroProgresivo(response.data)

                $('#modal_historial_progresivo').modal('show')
            } else {
                toastr.warning(response.message)
            }
        })
    })

    $(document).on('click', '#button_registros_progresivos', function (event) {
        event.preventDefault()

        var salaId = parseInt(SalaId)
        var progresivoId = parseInt(PorgresivoId)

        toastr.remove()

        if (!salaId || salaId <= 0) {
            toastr.warning("Seleccione una sala")

            return false
        }

        if (!progresivoId || progresivoId <= 0) {
            toastr.warning("Seleccione un progresivo")

            return false
        }

        HistorialRP.renderRegistrosProgresivos([])

        HistorialRP.getRegistrosProgresivos(salaId, progresivoId).done(function (response) {
            if (response.success) {
                HistorialRP.renderRegistrosProgresivos(response.data)

                $('#modal_registros_progresivos').modal('show')
            } else {
                toastr.warning(response.message)
            }
        })
    })

    $(document).on('click', '.view_registro_progresivo', function (event) {
        event.preventDefault()

        var salaId = $(this).attr('data-room')
        var detalleId = $(this).attr('data-id')

        toastr.remove()

        if (!salaId || salaId <= 0) {
            toastr.warning("Seleccione una sala")

            return false
        }

        if (!detalleId || detalleId <= 0) {
            toastr.warning("Seleccione un registro")

            return false
        }

        HistorialRP.clearRegistroProgresivo()

        HistorialRP.getRegistroProgresivo(salaId, detalleId).done(function (response) {
            if (response.success) {
                HistorialRP.setRegistroProgresivo(response.data)
                HistorialRP.renderRegistroProgresivo(response.data)

                $('#modal_registros_progresivos').modal('hide')
            } else {
                toastr.warning(response.message)
            }
        })
    })

    $(document).on('click', '#button_seleccionar_progresivo', function (event) {
        event.preventDefault()

        var salaId = HistorialRP.selectedRP.SalaId
        var detalleId = HistorialRP.selectedRP.Id

        toastr.remove()

        if (!salaId || salaId <= 0) {
            toastr.warning("Seleccione una sala")

            return false
        }

        if (!detalleId || detalleId <= 0) {
            toastr.warning("Seleccione un registro")

            return false
        }

        HistorialRP.getRegistroProgresivo(salaId, detalleId).done(function (response) {
            if (response.success) {
                HistorialRP.replaceRegistroProgresivo(response.data)

                $('#modal_historial_progresivo').modal('hide')
            } else {
                toastr.warning(response.message)
            }
        })
    })

    // ServicioWebOnline
    $(document).on('click', '#btn_historial_online', function (event) {
        event.preventDefault()

        var salaId = parseInt(SalaId)
        var progresivoId = parseInt(PorgresivoId)

        toastr.remove()

        if (!salaId || salaId <= 0) {
            toastr.warning("Seleccione una sala")

            return false
        }

        if (!progresivoId || progresivoId <= 0) {
            toastr.warning("Seleccione un progresivo")

            return false
        }

        HistorialRP.clearRegistroOnline()

        HistorialRP.getLastRegistroOnline(salaId, progresivoId).done(function (response) {
            if (response.success) {
                HistorialRP.setRegistroOnline(response.data)
                HistorialRP.renderRegistroOnline(response.data)

                $('#modal_historial_online').modal('show')
            } else {
                toastr.warning(response.message)
            }
        })

    })

    $(document).on('click', '#button_registros_online', function (event) {
        event.preventDefault()

        var salaId = parseInt(SalaId)
        var progresivoId = parseInt(PorgresivoId)

        toastr.remove()

        if (!salaId || salaId <= 0) {
            toastr.warning("Seleccione una sala")

            return false
        }

        if (!progresivoId || progresivoId <= 0) {
            toastr.warning("Seleccione un progresivo")

            return false
        }

        HistorialRP.renderRegistrosOnline([])

        HistorialRP.getRegistrosOnline(salaId, progresivoId).done(function (response) {
            if (response.success) {
                HistorialRP.renderRegistrosOnline(response.data)

                $('#modal_registros_online').modal('show')
            } else {
                toastr.warning(response.message)
            }
        })
    })

    $(document).on('click', '.view_registro_online', function (event) {
        event.preventDefault()

        var salaId = $(this).attr('data-room')
        var detalleId = $(this).attr('data-id')

        toastr.remove()

        if (!salaId || salaId <= 0) {
            toastr.warning("Seleccione una sala")

            return false
        }

        if (!detalleId || detalleId <= 0) {
            toastr.warning("Seleccione un registro")

            return false
        }

        HistorialRP.clearRegistroOnline()

        HistorialRP.getRegistroOnline(salaId, detalleId).done(function (response) {
            if (response.success) {
                HistorialRP.setRegistroOnline(response.data)
                HistorialRP.renderRegistroOnline(response.data)

                $('#modal_registros_online').modal('hide')
            } else {
                toastr.warning(response.message)
            }
        })
    })

    $(document).on('click', '#button_seleccionar_online', function (event) {
        event.preventDefault()

        var salaId = HistorialRP.selectedRO.SalaId
        var detalleId = HistorialRP.selectedRO.Id

        toastr.remove()

        if (!salaId || salaId <= 0) {
            toastr.warning("Seleccione una sala")

            return false
        }

        if (!detalleId || detalleId <= 0) {
            toastr.warning("Seleccione un registro")

            return false
        }

        HistorialRP.getRegistroOnline(salaId, detalleId).done(function (response) {
            if (response.success) {
                HistorialRP.replaceRegistroProgresivo(response.data)

                $('#modal_historial_online').modal('hide')
            } else {
                toastr.warning(response.message)
            }
        })
    })
    // Historial Registro Progresivo

});

// --------------------------------------------------------------------
// ----------------------------- LISTADOS -----------------------------
// --------------------------------------------------------------------



function ObtenerListaSalas() {
    comboImagen = $("#cboImagen");
    comboImagen.find('option').remove();
    ajaxhr = $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {              
            var datos = result.data;
            $.each(datos, function (index, value) {           
                $("#cboSala").append('<option value="' + value.UrlProgresivo + '"  data-id="' + value.CodSala + '" data-ipprivada="'+value.IpPrivada+'" data-puertoservicio="'+value.PuertoServicioWebOnline+'"  >' + value.Nombre + '</option>');
            });
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
    return false;
}

// --------------------------------------------------------------------
// ----------------------------- GRABADO -----------------------------
// --------------------------------------------------------------------




// --------------------------------------------------------------------------------
// ----------------------------- VALIDACIONES BASICAS -----------------------------
// --------------------------------------------------------------------------------

function ManipularCajasTextoPozoSuperior(accion) {
    if (accion == 0) { $('#txtPSPremioBase').attr('disabled', 'disabled'); } else { if (!$('#chkPozoOculto').is(":checked")) { $('#txtPSPremioBase').removeAttr('disabled'); } else { $('#txtPSPremioBase').attr('disabled', 'disabled'); } }
    if (accion == 0) { $('#txtPSPremioMinimo').attr('disabled', 'disabled'); } else { $('#txtPSPremioMinimo').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSPremioMaximo').attr('disabled', 'disabled'); } else { $('#txtPSPremioMaximo').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSIncPozoNormal').attr('disabled', 'disabled'); } else { $('#txtPSIncPozoNormal').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSMontoOcultoMin').attr('disabled', 'disabled'); } else { $('#txtPSMontoOcultoMin').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSMontoOcultoMax').attr('disabled', 'disabled'); } else { $('#txtPSMontoOcultoMax').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSIncPozoOculto').attr('disabled', 'disabled'); } else { $('#txtPSIncPozoOculto').removeAttr('disabled'); }
    if (accion == 0) { $('#cboPSDificultad').attr('disabled', 'disabled'); } else { $('#cboPSDificultad').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSRsApuesta').attr('disabled', 'disabled'); } else { $('#txtPSRsApuesta').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSRsJugadores').attr('disabled', 'disabled'); } else { $('#txtPSRsJugadores').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSPozoActual').attr('disabled', 'disabled') } else { $('#txtPSPozoActual').removeAttr('disabled'); }

}

function ManipularCajasTextoPozoMedio(accion) {
    if (accion == 0) { $('#txtPMPremioBase').attr('disabled', 'disabled'); } else { if (!$('#chkPozoOculto').is(":checked")) { $('#txtPMPremioBase').removeAttr('disabled'); } else { $('#txtPMPremioBase').attr('disabled', 'disabled'); } }
    if (accion == 0) { $('#txtPMPremioMinimo').attr('disabled', 'disabled'); } else { $('#txtPMPremioMinimo').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMPremioMaximo').attr('disabled', 'disabled'); } else { $('#txtPMPremioMaximo').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMIncPozoNormal').attr('disabled', 'disabled'); } else { $('#txtPMIncPozoNormal').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMMontoOcultoMin').attr('disabled', 'disabled'); } else { $('#txtPMMontoOcultoMin').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMMontoOcultoMax').attr('disabled', 'disabled'); } else { $('#txtPMMontoOcultoMax').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMIncPozoOculto').attr('disabled', 'disabled'); } else { $('#txtPMIncPozoOculto').removeAttr('disabled'); }
    if (accion == 0) { $('#cboPMDificultad').attr('disabled', 'disabled'); } else { $('#cboPMDificultad').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMRsApuesta').attr('disabled', 'disabled'); } else { $('#txtPMRsApuesta').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMRsJugadores').attr('disabled', 'disabled'); } else { $('#txtPMRsJugadores').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMPozoActual').attr('disabled', 'disabled'); } else { $('#txtPMPozoActual').removeAttr('disabled'); }

}

function ManipularCajasTextoPozoInferior(accion) {
    if (accion == 0) { $('#txtPIPremioBase').attr('disabled', 'disabled'); } else { if (!$('#chkPozoOculto').is(":checked")) { $('#txtPIPremioBase').removeAttr('disabled'); } else { $('#txtPIPremioBase').attr('disabled', 'disabled'); } }
    if (accion == 0) { $('#txtPIPremioMinimo').attr('disabled', 'disabled'); } else { $('#txtPIPremioMinimo').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIPremioMaximo').attr('disabled', 'disabled'); } else { $('#txtPIPremioMaximo').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIIncPozoNormal').attr('disabled', 'disabled'); } else { $('#txtPIIncPozoNormal').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIMontoOcultoMin').attr('disabled', 'disabled'); } else { $('#txtPIMontoOcultoMin').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIMontoOcultoMax').attr('disabled', 'disabled'); } else { $('#txtPIMontoOcultoMax').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIIncPozoOculto').attr('disabled', 'disabled'); } else { $('#txtPIIncPozoOculto').removeAttr('disabled'); }
    if (accion == 0) { $('#cboPIDificultad').attr('disabled', 'disabled'); } else { $('#cboPIDificultad').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIRsApuesta').attr('disabled', 'disabled'); } else { $('#txtPIRsApuesta').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIRsJugadores').attr('disabled', 'disabled'); } else { $('#txtPIRsJugadores').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIPozoActual').attr('disabled', 'disabled'); } else { $('#txtPIPozoActual').removeAttr('disabled'); }

}

function LimpiarCabecera() {
    $('#txtMoneda').val('S/.');
    $('#txtDuracion').val('0');
    $('#cboLugarPago')[0].selectedIndex = 0;
    $('#txtNumJugadores').val('0');
    $('#cboImagen')[0].selectedIndex = 0;
    $('#txtNroPozos').val('0');
    $('#cboEstado')[0].selectedIndex = 0;
    $('#chkPozoOculto').iCheck('uncheck');
    $('#chkRegHistorico').iCheck('uncheck');
}

function LimpiarDetalle() {
    $('#chkPozoSuperior').prop('checked', false);
    $('#chkPozoMedio').prop('checked', false);
    $('#chkPozoInferior').prop('checked', false);
    $('#txtPSPremioBase').val('0');
    $('#txtPMPremioBase').val('0');
    $('#txtPIPremioBase').val('0');
    $('#txtPSPremioMinimo').val('0');
    $('#txtPMPremioMinimo').val('0');
    $('#txtPIPremioMinimo').val('0');
    $('#txtPSPremioMaximo').val('0');
    $('#txtPMPremioMaximo').val('0');
    $('#txtPIPremioMaximo').val('0');
    $('#txtPSIncPozoNormal').val('0');
    $('#txtPMIncPozoNormal').val('0');
    $('#txtPIIncPozoNormal').val('0');
    $('#txtPSMontoOcultoMin').val('0');
    $('#txtPMMontoOcultoMin').val('0');
    $('#txtPIMontoOcultoMin').val('0');
    $('#txtPSMontoOcultoMax').val('0');
    $('#txtPMMontoOcultoMax').val('0');
    $('#txtPIMontoOcultoMax').val('0');
    $('#txtPSIncPozoOculto').val('0');
    $('#txtPMIncPozoOculto').val('0');
    $('#txtPIIncPozoOculto').val('0');
    $("#cboPSDificultad")[0].selectedIndex = 0;
    $("#cboPMDificultad")[0].selectedIndex = 0;
    $("#cboPIDificultad")[0].selectedIndex = 0;
    $('#txtPSPozoActual').val('0');
    $('#txtPMPozoActual').val('0');
    $('#txtPIPozoActual').val('0');
    $('#txtPSRsApuesta').val('0');
    $('#txtPMRsApuesta').val('0');
    $('#txtPIRsApuesta').val('0');
    $('#txtPSRsJugadores').val('0');
    $('#txtPMRsJugadores').val('0');
    $('#txtPIRsJugadores').val('0');
}

// --------------------------------------------------------------------------------
// ------------------------- URL acceso servicio Progresivo -----------------------
// --------------------------------------------------------------------------------
function getUrlApi(controler) {
    console.log("http://" + getHost() + ":8888/api/" + controler);
    return "http://" + getHost() + ":8888/api/" + controler;
}
function getHost() {
    var loc = window.location;
    var host1 = loc.host.substring(0, loc.host.lastIndexOf(':'));
    return host1;
}
function getUrlApiAparameter(controler, parameter) {

    return "http://" + window.location.hostname + ":8888/api/" + controler + parameter;
}
function getToken() {
    //var ptoken = sessionStorage.getItem("tokenKey");
    var ptoken = "aWNjd3Jrbmo6Li4xOkNocm9tZSA2Ni4wLjMzNTk6aWNjd3Jrbmo=";
    return ptoken;
}

function getHeader() {
    var token1 = getToken();
    var headers = {};
    if (token1)
        headers = { 'X-Auth-Token': token1 };
    return headers;
}
/**Nuevos Metodos */

function getPingSalas() {
    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Progresivo/EchoPingSalasUsuario",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
        },
        success: function (response) {
            return response
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
        }
    });
}
function getProgresivosVpn(urlPrivada,urlPublica){
    ajaxhr = $.ajax({
        data:JSON.stringify({urlPrivada:urlPrivada,urlPublica:urlPublica}),
        type:"POST",
        cache:false,
        url:basePath+'/Progresivo/listadoprogresivosVpn',
        contentType:"application/json: charset=utf-8",
        dataType:"json",
        beforeSend:function(xhr){
            $.LoadingOverlay('show')
        },
        success:function(response){
            return response
        },
        complete:function(xhr){
            $.LoadingOverlay('hide')
            AbortRequest.close()
        }
    })
    AbortRequest.open()
    return ajaxhr
}
function renderSelectProgresivos(data){
    $("#cboProgresivo").html("")
    if(data){
        $("#cboProgresivo").append('<option value="">--Seleccione--</option>');      
        $.each(data, function (index, value) {  
            $("#cboProgresivo").append(`
                <option 
                    value="${value["WEB_PrgID"]}" 
                    data-id="${value["WEB_Url"]}">
                    ${value["WEB_Nombre"]}
                </option>`)                     
        }); 
    }
}
function getUrl(url){
    if(url){
        try{
            let uri=new URL(url)
            return uri

        }catch(ex){
            return false
        }
    }
    return false
}
function obtenerListaSalas() {
    ajaxhr = $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            return result;
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
    return ajaxhr
}
function renderSelectSalas(data){
    $.each(data, function (index, value) {
        $("#cboSala").append(`<option value="${value.UrlProgresivo==""?"":value.UrlProgresivo}" data-id="${value.CodSala}" data-ipprivada="${value.IpPrivada}" data-puertoservicio="${value.PuertoServicioWebOnline}">${value.Nombre}</option>`)
    });
}

/**Listados */
//Forma Normal
function ObtenerListaImagenes() {    
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url = ipPublicaG + "/servicio/getListarImagenes/" + PorgresivoId;
    comboImagen = $("#cboImagen");
    comboImagen.find('option').remove();  
    console.log(url);
    ajaxhr = $.ajax({        
        type: "POST",
        url: basePath + "Progresivo/ProgresivoListarImagenesJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",        
        data: JSON.stringify({ url: url }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            RPMisterioso.imagenes = result.data;
            console.log(result);
            if (result.data == null) {
                toastr.error("No se encontraron datos.Consulte al administrador", "Mensaje Servidor");
                return false;
            }
            result = result.data;
            if (result == "No es posible conectar con el servidor remoto") {
                toastr.error(result, "Mensaje Servidor");
                return false;
            }
            var entidad = eval(result);
            $(entidad).each(function () {
                var option = $(document.createElement('option'));

                option.text(this.Descripcion);
                option.val(this.ID);                
                comboImagen.append(option);
            });
            ObtenerProgresivoActivo();

        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error, "Mensaje Servidor");
            return false;
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
    return false;
}
function ObtenerProgresivoActivo() {    
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    //var url = getUrlApi("RegistroProgresivo/ObtenerProgresivoActivo");    
    var url = ipPublicaG + "/servicio/ObtenerProgresivoActivo/" + PorgresivoId; 
    $.ajax({        
        type: "POST",
        url: basePath + "Progresivo/ProgresivoActivoObtenerJson",
        cache: false,
        headers: getHeader(),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        success: function (result) {   
            DataHistoricaCabecera = result.data;
            RPMisterioso.detalle = result.data;
            if (result.data == null) {
                toastr.error("No se han encontrado datos", "Mensaje Servidor");
                return false;
            }            
            result = result.data;
            var entidad = result;
            if (entidad["ProgresivoID"] == 0) {
                $('#hid_cod_cab_prog').val('0');
                ObtenerPozosActuales(0);
                LimpiarCabecera();
                LimpiarDetalle();
                return false;
            }
            else {
                $('#hid_cod_cab_prog').val(entidad["ProgresivoID"]);
                $('#txtNumJugadores').val(entidad["NroJugadores"]);
                var base_oculto = entidad["BaseOculto"];
                if (base_oculto == 0) {
                    $('#chkPozoOculto').iCheck('uncheck');
                    if ($('#chkPozoSuperior').is(":checked")) {
                        $('#txtPSPremioBase').removeAttr('disabled');
                    }
                    if ($('#chkPozoMedio').is(":checked")) {
                        $('#txtPMPremioBase').removeAttr('disabled');
                    }
                    if ($('#chkPozoInferior').is(":checked")) {
                        $('#txtPIPremioBase').removeAttr('disabled');
                    }
                }
                else {
                    $('#chkPozoOculto').iCheck('check');
                    $('#txtPSPremioBase').attr('disabled', 'disabled');
                    $('#txtPMPremioBase').attr('disabled', 'disabled');
                    $('#txtPIPremioBase').attr('disabled', 'disabled');
                }
                if (entidad["RegHistorico"]) {
                    $('#chkRegHistorico').iCheck('check');
                }
                else {
                    $('#chkRegHistorico').iCheck('uncheck');
                }                                
                $('#cboLugarPago').val((entidad["PagoCaja"]) ? 1 : 0);
                $('#txtNroPozos').val(entidad["NroPozos"]);
                $('#txtDuracion').val(entidad["DuracionPantalla"]);
                $('#cboEstado').val(entidad["Estado"]);
                $('#cboImagen').val(entidad["ProgresivoImagenID"]);
                ObtenerPozosActuales(entidad["ProgresivoID"]);

                return false;
            }
        },        
        error: function (request, status, error) {            
            toastr.error(request.responseText + " " + error, "Mensaje Servidor");
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        }
    });
    return false;
}
function ObtenerPozosActuales(codigo) {    
    LimpiarDetalle();   
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    //url = getUrlApiAparameter("RegistroProgresivo/ListarPozosActuales", parametros);
    var url = ipPublicaG + "/servicio/ListarPozosActuales/" + PorgresivoId + "/" + codigo;  
    console.log("pozo actual: "+ url);
    $.ajax({
        type: "POST",
        url: basePath + "Progresivo/ProgresivoPozosActualesObtenerJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        success: function (result) {
            DataHistoricaPozosActuales = result.data;
            RPMisterioso.pozos = result.data;
            result = result.data;
            var lista = result;
            if (lista.length > 0) {
                $.each(lista, function (indice, valor) {
                    if (valor['TipoPozo'] == 1) { $('#txtPSPozoActual').val(valor['Actual']); $('#hid_cod_det_prog_sup').val(valor['DetalleProgresivoID']);$("#valor_pozo_superior_actual").val(valor['Actual']); }
                    if (valor['TipoPozo'] == 2) { $('#txtPMPozoActual').val(valor['Actual']); $('#hid_cod_det_prog_med').val(valor['DetalleProgresivoID']);$("#valor_pozo_medio_actual").val(valor['Actual']); }
                    if (valor['TipoPozo'] == 3) { $('#txtPIPozoActual').val(valor['Actual']); $('#hid_cod_det_prog_inf').val(valor['DetalleProgresivoID']);$("#valor_pozo_inferior_actual").val(valor['Actual']); }
                });
                ObtenerDetallesProgresivo(codigo,1);
                return false;
            }
            else {
                $('#hid_cod_det_prog_sup').val('0');
                $('#hid_cod_det_prog_med').val('0');
                $('#hid_cod_det_prog_inf').val('0');
                return false;
            }
        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error, "Mensaje Servidor");
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        }
    });
}
function ObtenerDetallesProgresivo(codProg, estado) {    
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    //var url = getUrlApiAparameter("RegistroProgresivo/ListarDetallesProgresivo", parametros);
    var url = ipPublicaG + "/servicio/ListarDetallesProgresivo/" + PorgresivoId + "/" + codProg +"/"+estado;
    console.log(url);
    $.ajax({
        type: "POST",
        url: basePath + "Progresivo/ProgresivoDetalleListarJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        success: function (result) {                          
            DataHistoricaDetallePozos = result.data;
            RPMisterioso.detallePozos = result.data;
            $.each(DataHistoricaDetallePozos, function (index, value) {
                if (value.TipoPozo==1) {
                    value.Actual = $('#txtPSPozoActual').val();
                }
                if (value.TipoPozo == 2) {
                    value.Actual = $('#txtPMPozoActual').val();
                }
                if (value.TipoPozo == 3) {
                    value.Actual = $('#txtPIPozoActual').val();
                }
            });
            result = result.data;
            var lista = result;
            $('#hid_cod_det_prog_sup').val('0');
            $('#hid_cod_det_prog_med').val('0');
            $('#hid_cod_det_prog_inf').val('0');
            if (lista.length > 0) {
                $.each(lista, function (indice, valor) {
                    if (valor['TipoPozo'] == 1) {
                        $('#hid_cod_det_prog_sup').val(valor['DetalleProgresivoID']);
                        $('#txtPSRsApuesta').val(valor['RsApuesta']);
                        $('#txtPSRsJugadores').val(valor['RsJugadores']);
                        $('#txtPSPremioBase').val(valor['MontoBase']);
                        $('#txtPSPremioMinimo').val(valor['MontoMin']);
                        $('#txtPSPremioMaximo').val(valor['MontoMax']);
                        $('#txtPSIncPozoNormal').val(valor['IncPozo1']);
                        $('#txtPSMontoOcultoMin').val(valor['MontoOcMin']);
                        $('#txtPSMontoOcultoMax').val(valor['MontoOcMax']);
                        $('#txtPSIncPozoOculto').val(valor['IncOcPozo1']);
                        $('#cboPSDificultad').val(valor['Dificultad']);
                        ManipularCajasTextoPozoSuperior(valor['Estado']);
                        $('#chkPozoSuperior').prop("checked", ((valor['Estado'] == 1) ? true : false));
                    }
                    if (valor['TipoPozo'] == 2) {
                        $('#hid_cod_det_prog_med').val(valor['DetalleProgresivoID']);
                        $('#txtPMRsApuesta').val(valor['RsApuesta']);
                        $('#txtPMRsJugadores').val(valor['RsJugadores']);
                        $('#txtPMPremioBase').val(valor['MontoBase']);
                        $('#txtPMPremioMinimo').val(valor['MontoMin']);
                        $('#txtPMPremioMaximo').val(valor['MontoMax']);
                        $('#txtPMIncPozoNormal').val(valor['IncPozo1']);
                        $('#txtPMMontoOcultoMin').val(valor['MontoOcMin']);
                        $('#txtPMMontoOcultoMax').val(valor['MontoOcMax']);
                        $('#txtPMIncPozoOculto').val(valor['IncOcPozo1']);
                        $('#cboPMDificultad').val(valor['Dificultad']);
                        ManipularCajasTextoPozoMedio(valor['Estado']);
                        $('#chkPozoMedio').prop("checked", ((valor['Estado'] == 1) ? true : false));
                    }
                    if (valor['TipoPozo'] == 3) {
                        $('#hid_cod_det_prog_inf').val(valor['DetalleProgresivoID']);
                        $('#txtPIRsApuesta').val(valor['RsApuesta']);
                        $('#txtPIRsJugadores').val(valor['RsJugadores']);
                        $('#txtPIPremioBase').val(valor['MontoBase']);
                        $('#txtPIPremioMinimo').val(valor['MontoMin']);
                        $('#txtPIPremioMaximo').val(valor['MontoMax']);
                        $('#txtPIIncPozoNormal').val(valor['IncPozo1']);
                        $('#txtPIMontoOcultoMin').val(valor['MontoOcMin']);
                        $('#txtPIMontoOcultoMax').val(valor['MontoOcMax']);
                        $('#txtPIIncPozoOculto').val(valor['IncOcPozo1']);
                        $('#cboPIDificultad').val(valor['Dificultad']);
                        ManipularCajasTextoPozoInferior(valor['Estado']);
                        $('#chkPozoInferior').prop("checked", ((valor['Estado'] == 1) ? true : false));
                    }
                });
                dataAuditoria(0, "#formProgresivo", 1);
               
                return false;
            }
        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error, "Mensaje Servidor");
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        }
    });
}
//Vpn
function ObtenerListaImagenesVpn(){
    var urlPublica = ipPublicaAlterna.href + "/servicio/getListarImagenesVpn/" + PorgresivoId;
    let urlPrivada=ipPrivada+"/servicio/getListarImagenes/"+PorgresivoId
    comboImagen = $("#cboImagen");
    comboImagen.find('option').remove();  
    ajaxhr = $.ajax({        
        type: "POST",
        url: basePath + "Progresivo/ProgresivoListarImagenesJsonVpn",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",        
        data: JSON.stringify({ urlPublica: urlPublica,urlPrivada:urlPrivada }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            RPMisterioso.imagenes = result.data;
            console.log(result);
            if (result.data == null) {
                toastr.error("No se encontraron datos.Consulte al administrador", "Mensaje Servidor");
                return false;
            }
            result = result.data;
            if (result == "No es posible conectar con el servidor remoto") {
                toastr.error(result, "Mensaje Servidor");
                return false;
            }
            var entidad = eval(result);
            $(entidad).each(function () {
                var option = $(document.createElement('option'));

                option.text(this.Descripcion);
                option.val(this.ID);                
                comboImagen.append(option);
            });
            ObtenerProgresivoActivoVpn();

        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error, "Mensaje Servidor");
            return false;
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
    return false;
}
function ObtenerProgresivoActivoVpn() {    

       
    var urlPublica = ipPublicaAlterna.href + "/servicio/ObtenerProgresivoActivoVpn/" + PorgresivoId; 
    var urlPrivada = ipPrivada + "/servicio/ObtenerProgresivoActivo/" + PorgresivoId; 
    $.ajax({        
        type: "POST",
        url: basePath + "Progresivo/ProgresivoActivoObtenerJsonVpn",
        cache: false,
        headers: getHeader(),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ urlPublica: urlPublica,urlPrivada:urlPrivada }),
        success: function (result) {   
            DataHistoricaCabecera = result.data;
            RPMisterioso.detalle = result.data;
            if (result.data == null) {
                toastr.error("No se han encontrado datos", "Mensaje Servidor");
                return false;
            }            
            result = result.data;
            var entidad = result;
            if (entidad["ProgresivoID"] == 0) {
                $('#hid_cod_cab_prog').val('0');
                ObtenerPozosActualesVpn(0);
                LimpiarCabecera();
                LimpiarDetalle();
                return false;
            }
            else {
                $('#hid_cod_cab_prog').val(entidad["ProgresivoID"]);
                $('#txtNumJugadores').val(entidad["NroJugadores"]);
                var base_oculto = entidad["BaseOculto"];
                if (base_oculto == 0) {
                    $('#chkPozoOculto').iCheck('uncheck');
                    if ($('#chkPozoSuperior').is(":checked")) {
                        $('#txtPSPremioBase').removeAttr('disabled');
                    }
                    if ($('#chkPozoMedio').is(":checked")) {
                        $('#txtPMPremioBase').removeAttr('disabled');
                    }
                    if ($('#chkPozoInferior').is(":checked")) {
                        $('#txtPIPremioBase').removeAttr('disabled');
                    }
                }
                else {
                    $('#chkPozoOculto').iCheck('check');
                    $('#txtPSPremioBase').attr('disabled', 'disabled');
                    $('#txtPMPremioBase').attr('disabled', 'disabled');
                    $('#txtPIPremioBase').attr('disabled', 'disabled');
                }
                if (entidad["RegHistorico"]) {
                    $('#chkRegHistorico').iCheck('check');
                }
                else {
                    $('#chkRegHistorico').iCheck('uncheck');
                }                                
                $('#cboLugarPago').val((entidad["PagoCaja"]) ? 1 : 0);
                $('#txtNroPozos').val(entidad["NroPozos"]);
                $('#txtDuracion').val(entidad["DuracionPantalla"]);
                $('#cboEstado').val(entidad["Estado"]);
                $('#cboImagen').val(entidad["ProgresivoImagenID"]);
                ObtenerPozosActualesVpn(entidad["ProgresivoID"]);

                return false;
            }
        },        
        error: function (request, status, error) {            
            toastr.error(request.responseText + " " + error, "Mensaje Servidor");
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        }
    });
    return false;
}
function ObtenerPozosActualesVpn(codigo) {  
    LimpiarDetalle();   
      //url = getUrlApiAparameter("RegistroProgresivo/ListarPozosActuales", parametros);
    var urlPublica = ipPublicaAlterna.href + "/servicio/ListarPozosActualesVpn/" + PorgresivoId + "/" + codigo;  
    var urlPrivada = ipPrivada + "/servicio/ListarPozosActuales/" + PorgresivoId + "/" + codigo;  
    $.ajax({
        type: "POST",
        url: basePath + "Progresivo/ProgresivoPozosActualesObtenerJsonVpn",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ urlPublica: urlPublica,urlPrivada:urlPrivada }),
        success: function (result) {
            DataHistoricaPozosActuales = result.data;
            RPMisterioso.pozos = result.data;
            result = result.data;
            var lista = result;
            if (lista.length > 0) {
                $.each(lista, function (indice, valor) {
                    if (valor['TipoPozo'] == 1) { $('#txtPSPozoActual').val(valor['Actual']); $('#hid_cod_det_prog_sup').val(valor['DetalleProgresivoID']);$("#valor_pozo_superior_actual").val(valor['Actual']); }
                    if (valor['TipoPozo'] == 2) { $('#txtPMPozoActual').val(valor['Actual']); $('#hid_cod_det_prog_med').val(valor['DetalleProgresivoID']);$("#valor_pozo_medio_actual").val(valor['Actual']); }
                    if (valor['TipoPozo'] == 3) { $('#txtPIPozoActual').val(valor['Actual']); $('#hid_cod_det_prog_inf').val(valor['DetalleProgresivoID']);$("#valor_pozo_inferior_actual").val(valor['Actual']); }
                });
                ObtenerDetallesProgresivoVpn(codigo,1);
                return false;
            }
            else {
                $('#hid_cod_det_prog_sup').val('0');
                $('#hid_cod_det_prog_med').val('0');
                $('#hid_cod_det_prog_inf').val('0');
                return false;
            }
        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error, "Mensaje Servidor");
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        }
    });
}
function ObtenerDetallesProgresivoVpn(codProg, estado) {    
    var urlPublica = ipPublicaAlterna + "/servicio/ListarDetallesProgresivoVpn/" + PorgresivoId + "/" + codProg +"/"+estado;
    var urlPrivada = ipPrivada + "/servicio/ListarDetallesProgresivo/" + PorgresivoId + "/" + codProg +"/"+estado;
    $.ajax({
        type: "POST",
        url: basePath + "Progresivo/ProgresivoDetalleListarJsonVpn",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ urlPublica: urlPublica,urlPrivada:urlPrivada }),
        success: function (result) {                          
            DataHistoricaDetallePozos = result.data;
            RPMisterioso.detallePozos = result.data;
            $.each(DataHistoricaDetallePozos, function (index, value) {
                if (value.TipoPozo==1) {
                    value.Actual = $('#txtPSPozoActual').val();
                }
                if (value.TipoPozo == 2) {
                    value.Actual = $('#txtPMPozoActual').val();
                }
                if (value.TipoPozo == 3) {
                    value.Actual = $('#txtPIPozoActual').val();
                }
            });
            result = result.data;
            var lista = result;
            $('#hid_cod_det_prog_sup').val('0');
            $('#hid_cod_det_prog_med').val('0');
            $('#hid_cod_det_prog_inf').val('0');
            if (lista.length > 0) {
                $.each(lista, function (indice, valor) {
                    if (valor['TipoPozo'] == 1) {
                        $('#hid_cod_det_prog_sup').val(valor['DetalleProgresivoID']);
                        $('#txtPSRsApuesta').val(valor['RsApuesta']);
                        $('#txtPSRsJugadores').val(valor['RsJugadores']);
                        $('#txtPSPremioBase').val(valor['MontoBase']);
                        $('#txtPSPremioMinimo').val(valor['MontoMin']);
                        $('#txtPSPremioMaximo').val(valor['MontoMax']);
                        $('#txtPSIncPozoNormal').val(valor['IncPozo1']);
                        $('#txtPSMontoOcultoMin').val(valor['MontoOcMin']);
                        $('#txtPSMontoOcultoMax').val(valor['MontoOcMax']);
                        $('#txtPSIncPozoOculto').val(valor['IncOcPozo1']);
                        $('#cboPSDificultad').val(valor['Dificultad']);
                        ManipularCajasTextoPozoSuperior(valor['Estado']);
                        $('#chkPozoSuperior').prop("checked", ((valor['Estado'] == 1) ? true : false));
                    }
                    if (valor['TipoPozo'] == 2) {
                        $('#hid_cod_det_prog_med').val(valor['DetalleProgresivoID']);
                        $('#txtPMRsApuesta').val(valor['RsApuesta']);
                        $('#txtPMRsJugadores').val(valor['RsJugadores']);
                        $('#txtPMPremioBase').val(valor['MontoBase']);
                        $('#txtPMPremioMinimo').val(valor['MontoMin']);
                        $('#txtPMPremioMaximo').val(valor['MontoMax']);
                        $('#txtPMIncPozoNormal').val(valor['IncPozo1']);
                        $('#txtPMMontoOcultoMin').val(valor['MontoOcMin']);
                        $('#txtPMMontoOcultoMax').val(valor['MontoOcMax']);
                        $('#txtPMIncPozoOculto').val(valor['IncOcPozo1']);
                        $('#cboPMDificultad').val(valor['Dificultad']);
                        ManipularCajasTextoPozoMedio(valor['Estado']);
                        $('#chkPozoMedio').prop("checked", ((valor['Estado'] == 1) ? true : false));
                    }
                    if (valor['TipoPozo'] == 3) {
                        $('#hid_cod_det_prog_inf').val(valor['DetalleProgresivoID']);
                        $('#txtPIRsApuesta').val(valor['RsApuesta']);
                        $('#txtPIRsJugadores').val(valor['RsJugadores']);
                        $('#txtPIPremioBase').val(valor['MontoBase']);
                        $('#txtPIPremioMinimo').val(valor['MontoMin']);
                        $('#txtPIPremioMaximo').val(valor['MontoMax']);
                        $('#txtPIIncPozoNormal').val(valor['IncPozo1']);
                        $('#txtPIMontoOcultoMin').val(valor['MontoOcMin']);
                        $('#txtPIMontoOcultoMax').val(valor['MontoOcMax']);
                        $('#txtPIIncPozoOculto').val(valor['IncOcPozo1']);
                        $('#cboPIDificultad').val(valor['Dificultad']);
                        ManipularCajasTextoPozoInferior(valor['Estado']);
                        $('#chkPozoInferior').prop("checked", ((valor['Estado'] == 1) ? true : false));
                    }
                });
                dataAuditoria(0, "#formProgresivo", 1);
               
                return false;
            }
        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error, "Mensaje Servidor");
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        }
    });
}
/**Grabados */
function ValidarCajasTexto() {
    validacion_cajas_texto = 1;
    if ($('#chkPozoSuperior').is(":checked")) {
        if ($.trim($('#txtPSPremioBase').val()) == '') { toastr.error('Ingrese monto base'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPSPremioBase').val()) <= 0) { toastr.error('El monto base debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSPremioMinimo').val()) == '') { toastr.error('Ingrese premio minimo'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPSPremioMinimo').val()) <= 0) { toastr.error('El premio minimo debe ser mayor a cero'); resp = 0; return resp; }
        if ($.trim($('#txtPSPremioMaximo').val()) == '') { toastr.error('Ingrese premio maximo'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPSPremioMaximo').val()) <= 0) { toastr.error('El premio maximo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSIncPozoNormal').val()) == '') { toastr.error('Ingrese incremento en el pozo normal'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPSIncPozoNormal').val()) <= 0) { toastr.error('El incremento en el pozo normal debe ser mayor a cero'); resp = 0; return false; }
        if ($('#chkPozoOculto').is(":checked")) {
            if ($.trim($('#txtPSMontoOcultoMin').val()) == '') { toastr.error('Ingrese monto oculto minimo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPSMontoOcultoMin').val()) <= 0) { toastr.error('El monto oculto minimo debe ser mayor a cero'); resp = 0; return false; }
            if ($.trim($('#txtPSMontoOcultoMax').val()) == '') { toastr.error('Ingrese monto oculto maximo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPSMontoOcultoMax').val()) <= 0) { toastr.error('El monto oculto maximo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
            if ($.trim($('#txtPSIncPozoOculto').val()) == '') { toastr.error('Ingrese incremento de pozo oculto maximo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPSIncPozoOculto').val()) <= 0) { toastr.error('El incremento del pozo oculto debe ser mayor a cero'); validacion_cajas_texto = 0; return resp; }
        }
    }
    if ($('#chkPozoMedio').is(":checked")) {
        if ($.trim($('#txtPSPremioBase').val()) == '') { toastr.error('Ingrese monto base'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPMPremioBase').val()) <= 0) { toastr.error('El monto base debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSPremioMinimo').val()) == '') { toastr.error('Ingrese premio minimo'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPMPremioMinimo').val()) <= 0) { toastr.error('El premio minimo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSPremioMaximo').val()) == '') { toastr.error('Ingrese premio maximo'); validacion_cajas_texto = 0; return resp; }
        if (($('#txtPMPremioMaximo').val()) <= 0) { toastr.error('El premio maximo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSIncPozoNormal').val()) == '') { toastr.error('Ingrese incremento en el pozo normal'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPMIncPozoNormal').val()) <= 0) { toastr.error('El incremento en el pozo normal debe ser mayor a cero'); resp = 0; return false; }
        if ($('#chkPozoOculto').is(":checked")) {
            if ($.trim($('#txtPSMontoOcultoMin').val()) == '') { toastr.error('Ingrese monto oculto minimo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPMMontoOcultoMin').val()) <= 0) { toastr.error('El monto oculto minimo debe ser mayor a cero'); resp = 0; return false; }
            if ($.trim($('#txtPSMontoOcultoMax').val()) == '') { toastr.error('Ingrese monto oculto maximo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPMMontoOcultoMax').val()) <= 0) { toastr.error('El monto oculto maximo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
            if ($.trim($('#txtPSIncPozoOculto').val()) == '') { toastr.error('Ingrese incremento de pozo oculto maximo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPMIncPozoOculto').val()) <= 0) { toastr.error('El incremento del pozo oculto debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        }
    }
    if ($('#chkPozoInferior').is(":checked")) {
        if ($.trim($('#txtPSPremioBase').val()) == '') { toastr.error('Ingrese monto base'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPIPremioBase').val()) <= 0) { toastr.error('El monto base debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSPremioMinimo').val()) == '') { toastr.error('Ingrese premio minimo'); validacion_cajas_texto = 0; return resp; }
        if (($('#txtPIPremioMinimo').val()) <= 0) { toastr.error('El premio minimo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSPremioMaximo').val()) == '') { toastr.error('Ingrese premio maximo'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPIPremioMaximo').val()) <= 0) { toastr.error('El premio maximo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSIncPozoNormal').val()) == '') { toastr.error('Ingrese incremento en el pozo normal'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPIIncPozoNormal').val()) <= 0) { toastr.error('El incremento en el pozo normal debe ser mayor a cero'); resp = 0; return false; }
        if ($('#chkPozoOculto').is(":checked")) {
            if ($.trim($('#txtPSMontoOcultoMin').val()) == '') { toastr.error('Ingrese monto oculto minimo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPIMontoOcultoMin').val()) <= 0) { toastr.error('El monto oculto minimo debe ser mayor a cero'); resp = 0; return false; }
            if ($.trim($('#txtPSMontoOcultoMax').val()) == '') { toastr.error('Ingrese monto oculto maximo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPIMontoOcultoMax').val()) <= 0) { toastr.error('El monto oculto maximo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
            if ($.trim($('#txtPSIncPozoOculto').val()) == '') { toastr.error('Ingrese incremento de pozo oculto maximo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPIIncPozoOculto').val()) <= 0) { toastr.error('El incremento del pozo oculto debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        }
    }
    ManipularCajasTextoPozoSuperior(0);
    ManipularCajasTextoPozoMedio(0);
    ManipularCajasTextoPozoInferior(0);
    if(consultaPorVpn){
        GrabarCabeceraVpn();
    }
    else{
        GrabarCabecera();
    }
}

function GrabarCabecera() {
    var flag = GuardarHistoricoProgresivo();
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    if (flag) {                
        var lista = new Array();
        lista.push($('#hid_cod_cab_prog').val());
        lista.push($('#txtNroPozos').val());
        lista.push('0');
        lista.push($('#txtNumJugadores').val());
        lista.push($('#cboImagen').val());
        lista.push($('#cboLugarPago').val());
        lista.push($('#txtDuracion').val());
        lista.push($('#txtMoneda').val());
        lista.push($('#cboEstado').val());
        lista.push(($('#chkPozoOculto').is(":checked")) ? '1' : '0');
        lista.push(($('#chkRegHistorico').is(":checked")) ? '1' : '0');
        //var url = getUrlApi("RegistroProgresivo/GuardarCabecera");
        var url = ipPublicaG + "/servicio/GuardarCabecera?codProgresivo=" + PorgresivoId;
        var parametros = JSON.stringify({ lista: lista, url: url });
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Progresivo/ProgresivoGuardarCabeceraJson",
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                result = result.data;
                var v_codigo_cabecera = result;
                $('#hid_cod_cab_prog').val(v_codigo_cabecera);
                var listaPozos = new Array();
                /*POZO SUPERIOR*/
                if ($('#chkPozoSuperior').is(":checked")) {
                    var detalleSup = new Object();
                    detalleSup.ProgresivoID = v_codigo_cabecera;
                    detalleSup.DetalleProgresivoID = $('#hid_cod_det_prog_sup').val();
                    detalleSup.TipoPozo = 1;
                    detalleSup.MontoMin = parseFloat($('#txtPSPremioMinimo').val());
                    detalleSup.MontoBase = parseFloat($('#txtPSPremioBase').val());
                    detalleSup.MontoMax = parseFloat($('#txtPSPremioMaximo').val());
                    detalleSup.IncPozo1 = parseFloat($('#txtPSIncPozoNormal').val());
                    detalleSup.IncPozo2 = 0;
                    detalleSup.MontoOcMin = parseFloat($('#txtPSMontoOcultoMin').val());
                    detalleSup.MontoOcMax = parseFloat($('#txtPSMontoOcultoMax').val());
                    detalleSup.IncOcPozo1 = parseFloat($('#txtPSIncPozoOculto').val());
                    detalleSup.IncOcPozo2 = 0;
                    detalleSup.Estado = 1;
                    detalleSup.Parametro = false;
                    detalleSup.Punto = 0;
                    detalleSup.Prob1 = 0;
                    detalleSup.Prob2 = 0;
                    detalleSup.EstadoInicial = 1;
                    detalleSup.Dificultad = $('#cboPSDificultad').val();
                    detalleSup.RsApuesta = $('#txtPSRsApuesta').val();
                    detalleSup.RsJugadores = $('#txtPSRsJugadores').val();
                    listaPozos.push(detalleSup);
                }
                /*POZO MEDIO*/
                if ($('#chkPozoMedio').is(":checked")) {
                    var detalleMed = new Object();
                    detalleMed.ProgresivoID = v_codigo_cabecera;
                    detalleMed.DetalleProgresivoID = $('#hid_cod_det_prog_med').val();
                    detalleMed.TipoPozo = 2;
                    detalleMed.MontoMin = parseFloat($('#txtPMPremioMinimo').val());
                    detalleMed.MontoBase = parseFloat($('#txtPMPremioBase').val());
                    detalleMed.MontoMax = parseFloat($('#txtPMPremioMaximo').val());
                    detalleMed.IncPozo1 = parseFloat($('#txtPMIncPozoNormal').val());
                    detalleMed.IncPozo2 = 0;
                    detalleMed.MontoOcMin = parseFloat($('#txtPMMontoOcultoMin').val());
                    detalleMed.MontoOcMax = parseFloat($('#txtPMMontoOcultoMax').val());
                    detalleMed.IncOcPozo1 = parseFloat($('#txtPMIncPozoOculto').val());
                    detalleMed.IncOcPozo2 = 0;
                    detalleMed.Estado = 1;
                    detalleMed.Parametro = false;
                    detalleMed.Punto = 0;
                    detalleMed.Prob1 = 0;
                    detalleMed.Prob2 = 0;
                    detalleMed.EstadoInicial = 1;
                    detalleMed.Dificultad = $('#cboPMDificultad').val();
                    detalleMed.RsApuesta = $('#txtPMRsApuesta').val();
                    detalleMed.RsJugadores = $('#txtPMRsJugadores').val();
                    listaPozos.push(detalleMed);
                }
                /*POZO INFERIOR*/
                if ($('#chkPozoInferior').is(":checked")) {
                    var detalleInf = new Object();
                    detalleInf.ProgresivoID = v_codigo_cabecera;
                    detalleInf.DetalleProgresivoID = $('#hid_cod_det_prog_inf').val();
                    detalleInf.TipoPozo = 3;
                    detalleInf.MontoMin = parseFloat($('#txtPIPremioMinimo').val());
                    detalleInf.MontoBase = parseFloat($('#txtPIPremioBase').val());
                    detalleInf.MontoMax = parseFloat($('#txtPIPremioMaximo').val());
                    detalleInf.IncPozo1 = parseFloat($('#txtPIIncPozoNormal').val());
                    detalleInf.IncPozo2 = 0;
                    detalleInf.MontoOcMin = parseFloat($('#txtPIMontoOcultoMin').val());
                    detalleInf.MontoOcMax = parseFloat($('#txtPIMontoOcultoMax').val());
                    detalleInf.IncOcPozo1 = parseFloat($('#txtPIIncPozoOculto').val());
                    detalleInf.IncOcPozo2 = 0;
                    detalleInf.Estado = 1;
                    detalleInf.Parametro = false;
                    detalleInf.Punto = 0;
                    detalleInf.Prob1 = 0;
                    detalleInf.Prob2 = 0;
                    detalleInf.EstadoInicial = 1;
                    detalleInf.Dificultad = $('#cboPIDificultad').val();
                    detalleInf.RsApuesta = $('#txtPIRsApuesta').val();
                    detalleInf.RsJugadores = $('#txtPIRsJugadores').val();
                    listaPozos.push(detalleInf);
                }
                GuardarDetalles(listaPozos);
                return false;
            },
            error: function (request, status, error) {
                toastr.error(request.responseText + " " + error);
                //MostrarMensaje(request.responseText + " " + error);
                return false;
            },
            complete: function (result) {
                // enviar a IAS historial progresivo
                RPMisterioso.sendChangesToServer()
            }
        });
        return false;
    }   
    else {
        toastr.error("Error","Servidor no Disponible.");
    }
}

function APiRegistrarPozos(listaResp) {    
    var lista = new Array();
    var cant_reg = listaResp.length;
    for (i = 0; i < cant_reg; i++) {
        var tipo_pozo = listaResp[i].split("_")[0];
        var detalle_cod = listaResp[i].split("_")[1];
        if (tipo_pozo == 1) {
            if($("#chkModificarPozoSuperior").is(':checked')){
                lista.push($('#txtPSPozoActual').val());
                $('#hid_cod_det_prog_sup').val($('#txtPSPozoActual').val());
                localStorage.setItem('chkModificarPozoSuperior','true');
            }
            else{
                lista.push($('#valor_pozo_superior_actual').val());   
                $('#hid_cod_det_prog_sup').val($('#valor_pozo_superior_actual').val());
                localStorage.setItem('chkModificarPozoSuperior','false');
            }
        }
        else if (tipo_pozo == 2) {
            if($("#chkModificarPozoMedio").is(':checked')){
                lista.push($('#txtPMPozoActual').val());
                $('#hid_cod_det_prog_med').val($('#txtPMPozoActual').val());
                localStorage.setItem('chkModificarPozoMedio','true');
            }
            else{
                lista.push($('#valor_pozo_medio_actual').val());
                $('#hid_cod_det_prog_med').val($('#valor_pozo_medio_actual').val());
                localStorage.setItem('chkModificarPozoMedio','false');
            }
           
        }
        else {
            if($("#chkModificarPozoInferior").is(':checked')){
                lista.push($('#txtPIPozoActual').val());
                $('#hid_cod_det_prog_inf').val($('#txtPIPozoActual').val());
                localStorage.setItem('chkModificarPozoInferior','true');
            }
            else{
                lista.push($('#valor_pozo_inferior_actual').val());
                $('#hid_cod_det_prog_inf').val($('#valor_pozo_inferior_actual').val());
                localStorage.setItem('chkModificarPozoInferior','false');
            }
           
        }
    }

    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url = ipPublicaG + "/servicio/GuardarPozo?codProgresivo=" + PorgresivoId;    
    var parametros = JSON.stringify({ lista:lista,url:url });
    //var url = getUrlApi("RegistroPozo/GuardarPozo");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Progresivo/ProgresivoPozoInsertarJson",
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {             
            EnviarCorreo();
            dataAuditoria(1, "#formProgresivo", 1, "Progresivo/ProgresivoGuardarCabeceraJson","BOTON GRABAR");
            toastr.success('Se grabaron los datos correctamente');
            ObtenerProgresivoActivo();
            LimpiarCabecera();
            LimpiarDetalle();
            return false;
        },
        error: function (request, status, error) {           
            toastr.error(request.responseText + " " + error);
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        },
        complete: function (resul) {
        }
    });
}

function GuardarDetalles(listaPozos) {
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url = ipPublicaG + "/servicio/GuardarDetalles?codProgresivo=" + PorgresivoId;    
    var parametros = JSON.stringify({ listaPozos: listaPozos,url:url });    
    //var parametros = JSON.stringify(listaPozos);        
    $.ajax({
        type: "post",
        cache: false,
        //url: url,
        url: basePath + "Progresivo/ProgresivoDetallesGuardarJson",
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {             
            result = result.data;
            //crear metodo para evitar que se guarde el pozo
            // if (RegistrarPozos == 1) {
                APiRegistrarPozos(result);
            // }
            return false;
        },
        error: function (request, status, error) {            
            toastr.error(request.responseText + " " + error);
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        },
        complete: function (resul) {
        }
    });
}

function GrabarCabeceraVpn() {
    let flag = GuardarHistoricoProgresivo();
    if (!ipPublicaAlterna.href.trim()) {
        toastr.error("No se encontro url alterna para realizar la operacion", "Mensaje Servidor");
        return false;
    } 
    if (flag) {                
        var lista = new Array();
        lista.push($('#hid_cod_cab_prog').val());
        lista.push($('#txtNroPozos').val());
        lista.push('0');
        lista.push($('#txtNumJugadores').val());
        lista.push($('#cboImagen').val());
        lista.push($('#cboLugarPago').val());
        lista.push($('#txtDuracion').val());
        lista.push($('#txtMoneda').val());
        lista.push($('#cboEstado').val());
        lista.push(($('#chkPozoOculto').is(":checked")) ? '1' : '0');
        lista.push(($('#chkRegHistorico').is(":checked")) ? '1' : '0');
        //var url = getUrlApi("RegistroProgresivo/GuardarCabecera");
        let urlPublica = ipPublicaAlterna.href + "/servicio/GuardarCabeceraVpn?codProgresivo=" + PorgresivoId;
        let urlPrivada = ipPrivada + "/servicio/GuardarCabecera?codProgresivo=" + PorgresivoId;
        let parametros = JSON.stringify({ lista: lista, urlPublica: urlPublica,urlPrivada:urlPrivada });
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Progresivo/ProgresivoGuardarCabeceraJsonVpn",
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                result = result.data;
                var v_codigo_cabecera = result;
                $('#hid_cod_cab_prog').val(v_codigo_cabecera);
                var listaPozos = new Array();
                /*POZO SUPERIOR*/
                if ($('#chkPozoSuperior').is(":checked")) {
                    var detalleSup = new Object();
                    detalleSup.ProgresivoID = v_codigo_cabecera;
                    detalleSup.DetalleProgresivoID = $('#hid_cod_det_prog_sup').val();
                    detalleSup.TipoPozo = 1;
                    detalleSup.MontoMin = parseFloat($('#txtPSPremioMinimo').val());
                    detalleSup.MontoBase = parseFloat($('#txtPSPremioBase').val());
                    detalleSup.MontoMax = parseFloat($('#txtPSPremioMaximo').val());
                    detalleSup.IncPozo1 = parseFloat($('#txtPSIncPozoNormal').val());
                    detalleSup.IncPozo2 = 0;
                    detalleSup.MontoOcMin = parseFloat($('#txtPSMontoOcultoMin').val());
                    detalleSup.MontoOcMax = parseFloat($('#txtPSMontoOcultoMax').val());
                    detalleSup.IncOcPozo1 = parseFloat($('#txtPSIncPozoOculto').val());
                    detalleSup.IncOcPozo2 = 0;
                    detalleSup.Estado = 1;
                    detalleSup.Parametro = false;
                    detalleSup.Punto = 0;
                    detalleSup.Prob1 = 0;
                    detalleSup.Prob2 = 0;
                    detalleSup.EstadoInicial = 1;
                    detalleSup.Dificultad = $('#cboPSDificultad').val();
                    detalleSup.RsApuesta = $('#txtPSRsApuesta').val();
                    detalleSup.RsJugadores = $('#txtPSRsJugadores').val();
                    listaPozos.push(detalleSup);
                }
                /*POZO MEDIO*/
                if ($('#chkPozoMedio').is(":checked")) {
                    var detalleMed = new Object();
                    detalleMed.ProgresivoID = v_codigo_cabecera;
                    detalleMed.DetalleProgresivoID = $('#hid_cod_det_prog_med').val();
                    detalleMed.TipoPozo = 2;
                    detalleMed.MontoMin = parseFloat($('#txtPMPremioMinimo').val());
                    detalleMed.MontoBase = parseFloat($('#txtPMPremioBase').val());
                    detalleMed.MontoMax = parseFloat($('#txtPMPremioMaximo').val());
                    detalleMed.IncPozo1 = parseFloat($('#txtPMIncPozoNormal').val());
                    detalleMed.IncPozo2 = 0;
                    detalleMed.MontoOcMin = parseFloat($('#txtPMMontoOcultoMin').val());
                    detalleMed.MontoOcMax = parseFloat($('#txtPMMontoOcultoMax').val());
                    detalleMed.IncOcPozo1 = parseFloat($('#txtPMIncPozoOculto').val());
                    detalleMed.IncOcPozo2 = 0;
                    detalleMed.Estado = 1;
                    detalleMed.Parametro = false;
                    detalleMed.Punto = 0;
                    detalleMed.Prob1 = 0;
                    detalleMed.Prob2 = 0;
                    detalleMed.EstadoInicial = 1;
                    detalleMed.Dificultad = $('#cboPMDificultad').val();
                    detalleMed.RsApuesta = $('#txtPMRsApuesta').val();
                    detalleMed.RsJugadores = $('#txtPMRsJugadores').val();
                    listaPozos.push(detalleMed);
                }
                /*POZO INFERIOR*/
                if ($('#chkPozoInferior').is(":checked")) {
                    var detalleInf = new Object();
                    detalleInf.ProgresivoID = v_codigo_cabecera;
                    detalleInf.DetalleProgresivoID = $('#hid_cod_det_prog_inf').val();
                    detalleInf.TipoPozo = 3;
                    detalleInf.MontoMin = parseFloat($('#txtPIPremioMinimo').val());
                    detalleInf.MontoBase = parseFloat($('#txtPIPremioBase').val());
                    detalleInf.MontoMax = parseFloat($('#txtPIPremioMaximo').val());
                    detalleInf.IncPozo1 = parseFloat($('#txtPIIncPozoNormal').val());
                    detalleInf.IncPozo2 = 0;
                    detalleInf.MontoOcMin = parseFloat($('#txtPIMontoOcultoMin').val());
                    detalleInf.MontoOcMax = parseFloat($('#txtPIMontoOcultoMax').val());
                    detalleInf.IncOcPozo1 = parseFloat($('#txtPIIncPozoOculto').val());
                    detalleInf.IncOcPozo2 = 0;
                    detalleInf.Estado = 1;
                    detalleInf.Parametro = false;
                    detalleInf.Punto = 0;
                    detalleInf.Prob1 = 0;
                    detalleInf.Prob2 = 0;
                    detalleInf.EstadoInicial = 1;
                    detalleInf.Dificultad = $('#cboPIDificultad').val();
                    detalleInf.RsApuesta = $('#txtPIRsApuesta').val();
                    detalleInf.RsJugadores = $('#txtPIRsJugadores').val();
                    listaPozos.push(detalleInf);
                }
                GuardarDetallesVpn(listaPozos);
                return false;
            },
            error: function (request, status, error) {
                toastr.error(request.responseText + " " + error);
                //MostrarMensaje(request.responseText + " " + error);
                return false;
            },
            complete: function (result) {
                // enviar a IAS historial progresivo
                RPMisterioso.sendChangesToServer()
            }
        });
        return false;
    }   
    else {
        toastr.error("Error","Servidor no Disponible.");
    }
}
function GuardarDetallesVpn(listaPozos) {
    if (!ipPublicaAlterna.href.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    let urlPublica = ipPublicaAlterna.href + "/servicio/GuardarDetallesVpn?codProgresivo=" + PorgresivoId;    
    let urlPrivada = ipPrivada + "/servicio/GuardarDetalles?codProgresivo=" + PorgresivoId;    
    var parametros = JSON.stringify({ listaPozos: listaPozos,urlPublica:urlPublica,urlPrivada:urlPrivada });    
    $.ajax({
        type: "post",
        cache: false,
        //url: url,
        url: basePath + "Progresivo/ProgresivoDetallesGuardarJsonVpn",
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {             
            result = result.data;
            //crear metodo para evitar que se guarde el pozo
            // if (RegistrarPozos == 1) {
                APiRegistrarPozosVpn(result);
            // }
            return false;
        },
        error: function (request, status, error) {            
            toastr.error(request.responseText + " " + error);
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        },
        complete: function (resul) {
        }
    });
}
function APiRegistrarPozosVpn(listaResp) {    
    var lista = new Array();
    var cant_reg = listaResp.length;
    for (i = 0; i < cant_reg; i++) {
        var tipo_pozo = listaResp[i].split("_")[0];
        var detalle_cod = listaResp[i].split("_")[1];
        if (tipo_pozo == 1) {
            if($("#chkModificarPozoSuperior").is(':checked')){
                lista.push($('#txtPSPozoActual').val());
                $('#hid_cod_det_prog_sup').val($('#txtPSPozoActual').val());
                localStorage.setItem('chkModificarPozoSuperior','true');
            }
            else{
                lista.push($('#valor_pozo_superior_actual').val());   
                $('#hid_cod_det_prog_sup').val($('#valor_pozo_superior_actual').val());
                localStorage.setItem('chkModificarPozoSuperior','false');
            }
        }
        else if (tipo_pozo == 2) {
            if($("#chkModificarPozoMedio").is(':checked')){
                lista.push($('#txtPMPozoActual').val());
                $('#hid_cod_det_prog_med').val($('#txtPMPozoActual').val());
                localStorage.setItem('chkModificarPozoMedio','true');
            }
            else{
                lista.push($('#valor_pozo_medio_actual').val());
                $('#hid_cod_det_prog_med').val($('#valor_pozo_medio_actual').val());
                localStorage.setItem('chkModificarPozoMedio','false');
            }
           
        }
        else {
            if($("#chkModificarPozoInferior").is(':checked')){
                lista.push($('#txtPIPozoActual').val());
                $('#hid_cod_det_prog_inf').val($('#txtPIPozoActual').val());
                localStorage.setItem('chkModificarPozoInferior','true');
            }
            else{
                lista.push($('#valor_pozo_inferior_actual').val());
                $('#hid_cod_det_prog_inf').val($('#valor_pozo_inferior_actual').val());
                localStorage.setItem('chkModificarPozoInferior','false');
            }
           
        }
    }

    if (!ipPublicaAlterna.href.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    let urlPublica = ipPublicaAlterna.href + "/servicio/GuardarPozoVpn?codProgresivo=" + PorgresivoId;    
    let urlPrivada = ipPrivada + "/servicio/GuardarPozo?codProgresivo=" + PorgresivoId;    
    let parametros = JSON.stringify({ lista:lista,urlPublica:urlPublica,urlPrivada:urlPrivada });
    //var url = getUrlApi("RegistroPozo/GuardarPozo");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Progresivo/ProgresivoPozoInsertarJsonVpn",
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {             
            EnviarCorreo();
            dataAuditoria(1, "#formProgresivo", 1, "Progresivo/ProgresivoGuardarCabeceraJsonVpn","BOTON GRABAR");
            toastr.success('Se grabaron los datos correctamente');
            ObtenerProgresivoActivoVpn();
            LimpiarCabecera();
            LimpiarDetalle();
            return false;
        },
        error: function (request, status, error) {           
            toastr.error(request.responseText + " " + error);
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        },
        complete: function (resul) {
        }
    });
}

function GuardarHistoricoProgresivo() {
    var obj = {
        ProgresivoActivo:DataHistoricaCabecera,
        PozosActuales:DataHistoricaPozosActuales,
        DetalleProgresivo:DataHistoricaDetallePozos,
    };
    var objStr = JSON.stringify(obj)
    var parametros = JSON.stringify({ objStr: objStr, codSala: SalaId, codProgresivo: PorgresivoId });    
    var state = false;
    $.ajax({
        type: "post",
        cache: false,
        async: false,
        url: basePath + "Progresivo/ProgresivoHistoricoInsertarJson",
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {              
            if (result.data==true) {
                state= true;
            }else {
                state =false;
            }
        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error);
            return false;
        },
        complete: function (resul) {
        }
    }); 
    return state;
}

function EnviarCorreo() {    
    var lista = new Array();
    lista.push($('#hid_cod_cab_prog').val());
    lista.push($('#txtNroPozos').val());
    lista.push('0');
    lista.push($('#txtNumJugadores').val());
    lista.push($('#cboImagen').val());
    lista.push($('#cboLugarPago').val());
    lista.push($('#txtDuracion').val());
    lista.push($('#txtMoneda').val());
    lista.push($('#cboEstado').val());
    lista.push(($('#chkPozoOculto').is(":checked")) ? '1' : '0');
    lista.push(($('#chkRegHistorico').is(":checked")) ? '1' : '0');
    var listaPozosEmail = new Array();
    /*POZO SUPERIOR*/
    if ($('#chkPozoSuperior').is(":checked")) {
        var detalleSup = new Object();
        detalleSup.ProgresivoID = v_codigo_cabecera;
        detalleSup.DetalleProgresivoID = $('#hid_cod_det_prog_sup').val();
        detalleSup.TipoPozo = 1;
        detalleSup.MontoMin = parseFloat($('#txtPSPremioMinimo').val());
        detalleSup.MontoBase = parseFloat($('#txtPSPremioBase').val());
        detalleSup.MontoMax = parseFloat($('#txtPSPremioMaximo').val());
        detalleSup.IncPozo1 = parseFloat($('#txtPSIncPozoNormal').val());
        detalleSup.MontoOcMin = parseFloat($('#txtPSMontoOcultoMin').val());
        detalleSup.MontoOcMax = parseFloat($('#txtPSMontoOcultoMax').val());
        detalleSup.IncOcPozo1 = parseFloat($('#txtPSIncPozoOculto').val());      
        detalleSup.Dificultad = $('#cboPSDificultad').val();
        detalleSup.RsApuesta = $('#txtPSRsApuesta').val();
        detalleSup.RsJugadores = $('#txtPSRsJugadores').val();
        detalleSup.Actual= $('#txtPSPozoActual').val();
        listaPozosEmail.push(detalleSup);
    }
    /*POZO MEDIO*/
    if ($('#chkPozoMedio').is(":checked")) {
        var detalleMed = new Object();
        detalleMed.ProgresivoID = v_codigo_cabecera;
        detalleMed.DetalleProgresivoID = $('#hid_cod_det_prog_med').val();
        detalleMed.TipoPozo = 2;
        detalleMed.MontoMin = parseFloat($('#txtPMPremioMinimo').val());
        detalleMed.MontoBase = parseFloat($('#txtPMPremioBase').val());
        detalleMed.MontoMax = parseFloat($('#txtPMPremioMaximo').val());
        detalleMed.IncPozo1 = parseFloat($('#txtPMIncPozoNormal').val());
        detalleMed.MontoOcMin = parseFloat($('#txtPMMontoOcultoMin').val());
        detalleMed.MontoOcMax = parseFloat($('#txtPMMontoOcultoMax').val());
        detalleMed.IncOcPozo1 = parseFloat($('#txtPMIncPozoOculto').val());
        detalleMed.Dificultad = $('#cboPMDificultad').val();
        detalleMed.RsApuesta = $('#txtPMRsApuesta').val();
        detalleMed.RsJugadores = $('#txtPMRsJugadores').val();
        detalleMed.Actual = $('#txtPMPozoActual').val();
        listaPozosEmail.push(detalleMed);
    }
    /*POZO INFERIOR*/
    if ($('#chkPozoInferior').is(":checked")) {
        var detalleInf = new Object();
        detalleInf.ProgresivoID = v_codigo_cabecera;
        detalleInf.DetalleProgresivoID = $('#hid_cod_det_prog_inf').val();
        detalleInf.TipoPozo = 3;
        detalleInf.MontoMin = parseFloat($('#txtPIPremioMinimo').val());
        detalleInf.MontoBase = parseFloat($('#txtPIPremioBase').val());
        detalleInf.MontoMax = parseFloat($('#txtPIPremioMaximo').val());
        detalleInf.IncPozo1 = parseFloat($('#txtPIIncPozoNormal').val());
        detalleInf.MontoOcMin = parseFloat($('#txtPIMontoOcultoMin').val());
        detalleInf.MontoOcMax = parseFloat($('#txtPIMontoOcultoMax').val());
        detalleInf.IncOcPozo1 = parseFloat($('#txtPIIncPozoOculto').val());
        detalleInf.Dificultad = $('#cboPIDificultad').val();
        detalleInf.RsApuesta = $('#txtPIRsApuesta').val();
        detalleInf.RsJugadores = $('#txtPIRsJugadores').val();
        detalleInf.Actual = $('#txtPIPozoActual').val();
        listaPozosEmail.push(detalleInf);
    }
   
    var obj = {
        ProgresivoActivo: DataHistoricaCabecera,
        PozosActuales: DataHistoricaPozosActuales,
        DetalleProgresivo: DataHistoricaDetallePozos,
    };
    var parametros = JSON.stringify({ obj: obj, Sala: SalaStr, Progresivo: PorgresivoStr, listaPozos: listaPozosEmail, lista: lista});
    var state = false;
    $.ajax({
        type: "post",
        cache: false,
        async: false,
        url: basePath + "Progresivo/ProgresivoCorreoEnviarJson",
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {            
            if (result.data ==true) {
                toastr.success('Se Envio Correo Correctamente');
            }
            else {
                toastr.error('No Se Envio Correo Correctamente');
            }
        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error);
            return false;
        },
        complete: function (resul) {
        }
    });
    return state;
}
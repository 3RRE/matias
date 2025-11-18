var itemAnterior;
var itemModificando;
var carga;
$(document).ready(function () {

    $(".dateOnlyFechaInicio").datetimepicker({

        pickTime: false,
        format: 'DD/MM/YYYY'
      
    });
    carga = 0;
    ipPublicaG = "";
    ipPublicaGProgresivo = "";
    ObtenerListaSalas();
    $("#cboSala").select2();
    $("#cboProgresivo").select2();
    $(document).on('change', '#cboSala', function (e) {
        var ipPublica = $(this).val();
        ipPublicaG = ipPublica;
        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }
    });
    $("#btnBuscar").on("click", function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        buscarCajasXFechaInicio();

    });   
    
    var delayedFn, blurredFrom; 
    var lista = [];
    $(document).on("click", ".estado", function () { 
        claseInput = 'estado'; 
    });
    $(document).on("click", ".tcambio", function () { 
        claseInput = 'tcambio'; 
    });
    
    
    $('#txtfechaInicio').val('01/01/2018');

    $(document).on('change', '.EstadoCaja', function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        var item = $(this).attr("data-id");
        var id_caja_abierto = $("#" + item + "estado").val();
        if (id_caja_abierto == 1) {
            $("#ModalCompartido").modal({
                backdrop: 'static',
                keyboard: false
            });
            $("#idItemCaja").val(item);
        }        
    });

    $(document).on('click', '.btnSi', function () {

        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }

        var item = $("#idItemCaja").val();
        var url = ipPublicaG + "/servicio/ModificarProcedimientoAlmacenado?itemCaja=" + item;
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "CallCenter/ModificarProcedimientoAlmacenadoJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ url: url }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                var msj = "";
                var resp = response.data;
                msj = response.mensaje != null ? response.mensaje : '';
                if (msj != "") {
                    toastr.error("Error De Conexion, Servidor no Encontrado.");
                } else {
                    if (resp.length > 0) {
                        var itm = $("#idItemCaja").val();
                        toastr.success('Se abrira la Caja ' + itm, 'Mensaje Servidor');
                        modificarCajaSP(itm);
                        $('#ModalCompartido').modal('hide');
                    } else {
                        toastr.error("Error De Conexion, Servidor no Encontrado.");
                    }
                    
                }
            },
            error: function (request, status, error) {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });



    });

    $(document).on('click', '.btnNo', function () {
        $('#ModalCompartido').modal('hide');
        var item = $("#idItemCaja").val();
        var estadoIni = $("#" + item + "estado").attr("data-inicial");
        $("#" + item + "estado").val(estadoIni);
        $("#idItemCaja").val("");
    });

    $(document).on('click', '#btnMostrarCaja', function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }
        var url = ipPublicaG + "/servicio/RestaurarProcedimientoAlmacenado";
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "CallCenter/RestaurarProcedimientoAlmacenadoJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ url: url }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                var msj = "";
                var resp = response.data;
                msj = response.mensaje != null ? response.mensaje : '';
                if (msj != "") {
                    toastr.error("Error De Conexion, Servidor no Encontrado.");
                } else {
                    toastr.success('Se mostrara la Caja Actual', 'Mensaje Servidor');
                }
            },
            error: function (request, status, error) {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    });
    
    
});

$(document).on("click", ".InsertaTipoFichas", function () {
    var item = $(this).attr("data-id");    
    InsertarTipoFichas(item); 
});
datainicial = "";
function buscarCajasXFechaInicio() {
    var fechaFinOpcional = "&fechaFin="

    if ($("#txtfechaFin").val() == "") {
        fechaFinOpcional = "";
    }
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    }
    var url = ipPublicaG + "/servicio/ObtenerCajas?fechaInicio=" + $("#txtfechaInicio").val()
        + fechaFinOpcional + $("#txtfechaFin").val();

    if ($("#txtfechaInicio").val() =="") {
        toastr.error("Ingrese Fecha de Inicio.");
    }

    dataAuditoria(1, "#formfiltro", 3, "CallCenter/ConsultaObtenerCajasXFechaInicioJson", "BOTON BUSCAR");
    console.log(url);
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath +"CallCenter/ConsultaObtenerCajasXFechaInicioJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {             
            response = response.data;
            objetodatatable = $("#table").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "sScrollX": "100%",
                "paging": true,
                "autoWidth": false,
                "bAutoWidth": true,
                "bProcessing": true,
                "bDeferRender": true,
                data: response,

                columns: [
                    {
                        data: "item", title: "Inserta Fichas",
                        "render": function (value) {
                            return '<button data-id = "'+value +'" type="button" class="btn btn-success btn-sm col-md-12 col-xs-12 InsertaTipoFichas">Inserta tipo Fichas </button> ';
                        }
                    },
                    { data: "item", title: "item" },
                    {
                        data: "tipo_cambio", title: "tipo_cambio",
                        "render": function (value,i,j) {
                            return '<input class="form-control input-sm  tcambio" type="Number" step=".01" data-inicial = "' + j.tipo_cambio + '" id="' + j.item + 'tcambio" data-id="' + j.item + '" value="' + j.tipo_cambio + '">';

                        }
                    },
                    //{
                    //    data: "tipo_cambio", title: "tipo_cambio", "visible": false,
                        
                    //},
                    
                    {
                        data: "estado", title: "estado",
                        "render": function (value,i,j) { 
                            //actualEstado = $("#" + value.item + "estado").val();
                            /*return '<input class="form-control input-sm  estado" type="Number" min="1" step="1" data-inicial = "' + value.estado + '" id="' + value.item + 'estado" data-id="' + value.item + '" value="' + value.estado + '"> ';*/

                            var seleccionarAnulado = '';
                            var seleccionarAbierto = '';
                            var seleccionarCerrado = '';

                            if (j.estado == 0) {
                                seleccionarAnulado = 'selected="selected"';
                            }
                            if (j.estado == 1) {
                                seleccionarAbierto = 'selected="selected"';
                            }
                            if (j.estado == 2) {
                                seleccionarCerrado = 'selected="selected"';
                            }
                            return '<select data-inicial = "' + j.estado + '" name="estado" id="' + j.item + 'estado"  data-id="' + j.item + '" class="selectpicker form-control input-sm EstadoCaja"> <option ' + seleccionarAnulado + ' value="0" >Anulado</option> <option ' + seleccionarAbierto + 'value="1">Abierto</option><option ' + seleccionarCerrado + ' value="2">Cerrado</option></select >';
                        }
                    },
                    { data: "cod_empresa", title: "cod_empresa" },
                    { data: "cod_sala", title: "cod_sala" },
                    {
                        data: "fecha_apertura", title: "fecha_apertura",
                        "render": function (value, i, j) { 
                           var actualFecha = moment(value).format('DD/MM/YYYY');
                            return '<input readonly="readonly" class="form-control input-sm  dateOnlyFechaInicio"  data-inicial = "' + actualFecha + '" id="' + j.item + 'fecha_apertura" data-id="' + j.item + '" value="' + actualFecha + '">';
                           
                        }
                    },
                    {
                        data: "fecha_cierre", title: "fecha_cierre",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY');
                        }
                    }, 
                    //{ data: "turno", title: "turno" },
                    {
                        data: "turno", title: "turno",
                        "render": function (value, i, j) {
                            var seleccionarAnulado = '';
                            var seleccionarAbierto = '';
                            var seleccionarCerrado = '';
                            if (j.turno == 1) {
                                seleccionarAnulado = 'selected="selected"';
                            }
                            if (j.turno == 2) {
                                seleccionarAbierto = 'selected="selected"';
                            }
                            if (j.turno == 3) {
                                seleccionarCerrado = 'selected="selected"';
                            }
                            return '<select data-inicial = "' + j.turno + '" name="turno" id="' + j.item + 'turno"  data-id="' + j.item + '" class="selectpicker form-control input-sm TurnoCaja"> <option ' + seleccionarAnulado + ' value="1" >Turno 1</option> <option ' + seleccionarAbierto + 'value="2">Turno 2</option><option ' + seleccionarCerrado + ' value="3">Turno 3</option></select >';
                        }
                    },
                    { data: "cod_caja", title: "cod_caja" },
                    { data: "cod_personal", title: "cod_personal" },
                    { data: "tipo_caja", title: "tipo_caja" },
                    { data: "compras", title: "compras" },
                    { data: "billete_fallas", title: "billete_fallas" },
                    { data: "monedas_fallas", title: "monedas_fallas" },
                    { data: "sobrantes", title: "sobrantes" },
                    { data: "faltantes", title: "faltantes" },
                    { data: "vales", title: "vales" },
                    { data: "vales_maquina", title: "vales_maquina" },
                    { data: "premios", title: "premios" },
                    { data: "otros", title: "otros" },
                    { data: "monto_inicial_dinero", title: "monto_inicial_dinero" },
                    { data: "saldo_precuadre", title: "saldo_precuadre" },
                    { data: "total_saldo_inicial", title: "total_saldo_inicial" },
                    { data: "total_saldo_final", title: "total_saldo_final" },
                    { data: "total_saldo_ingresos", title: "total_saldo_ingresos" },
                    { data: "total_saldo_salidas", title: "total_saldo_salidas" },
                    { data: "total_saldo_pagomanual", title: "total_saldo_pagomanual" },
                    { data: "saldo_tarjetas", title: "saldo_tarjetas" },
                    { data: "cod_registro_usuario", title: "cod_registro_usuario" },
                    { data: "caja_usuario", title: "caja_usuario" },
                    { data: "caja_usuario_nro", title: "caja_usuario_nro" },
                    { data: "desc_caja", title: "desc_caja" },
                    { data: "monto_final_dinero", title: "monto_final_dinero" },
                    { data: "monto_reposicion_fondo_caja", title: "monto_reposicion_fondo_caja" },
                    { data: "monto_reposicion_caja_fondo", title: "monto_reposicion_caja_fondo" },
                    { data: "monto_arrastre_deuda", title: "monto_arrastre_deuda" },
                    { data: "monto_bar_cortesia", title: "monto_bar_cortesia" },
                    { data: "monto_bar_venta", title: "monto_bar_venta" },
                    { data: "expediente", title: "expediente" },
                    { data: "B_10", title: "B_10" },
                    { data: "B_20", title: "B_20" },
                    { data: "B_50", title: "B_50" },
                    { data: "B_100", title: "B_100" },
                    { data: "B_200", title: "B_200" },
                    { data: "M_001", title: "M_001" },
                    { data: "M_005", title: "M_005" },
                    { data: "M_01", title: "M_01" },
                    { data: "M_02", title: "M_02" },
                    { data: "M_05", title: "M_05" },
                    { data: "M_1", title: "M_1" },
                    { data: "M_2", title: "M_2" },
                    { data: "M_5", title: "M_5" },
                    { data: "bd_1", title: "bd_1" },
                    { data: "bd_5", title: "bd_5" },
                    { data: "BD_10", title: "BD_10" },
                    { data: "BD_20", title: "BD_20" },
                    { data: "BD_50", title: "BD_50" },
                    { data: "BD_100", title: "BD_100" },
                    { data: "BD_200", title: "BD_200" },
                    { data: "Pres_Sal", title: "Pres_Sal" },
                    { data: "Pres_Otros", title: "Pres_Otros" },
                    { data: "Devolucion_Sal", title: "Devolucion_Sal" },
                    { data: "Devolucion_Otros", title: "Devolucion_Otros" },
                    { data: "MontoOtros", title: "MontoOtros" },
                    { data: "ValorSR", title: "ValorSR" },
                    { data: "Bingo1", title: "Bingo1" },
                    { data: "Bingo2", title: "Bingo2" },
                    { data: "Telefono1", title: "Telefono1" },
                    { data: "Telefono2", title: "Telefono2" },
                    { data: "Otros1", title: "Otros1" },
                    { data: "Otros2", title: "Otros2" },
                    { data: "Observaciones", title: "Observaciones" },
                    { data: "monto_inicial_dinero_dolares", title: "monto_inicial_dinero_dolares" },
                    { data: "monto_final_dinero_dolares", title: "monto_final_dinero_dolares" },
                    { data: "monto_reposicion_fondo_caja_dolares", title: "monto_reposicion_fondo_caja_dolares" },
                    { data: "monto_reposicion_caja_fondo_dolares", title: "monto_reposicion_caja_fondo_dolares" },
                    { data: "D_PreCuadre", title: "D_PreCuadre" },
                    { data: "D_PreCuadreDolares", title: "D_PreCuadreDolares" },
                    { data: "D_Asignados", title: "D_Asignados" },
                    { data: "D_AsignadosDolares", title: "D_AsignadosDolares" },
                    { data: "D_Dolares", title: "D_Dolares" },
                    { data: "D_Otros_Ingresos", title: "D_Otros_Ingresos" },
                    { data: "D_Promocion", title: "D_Promocion" },
                    { data: "D_Cortesia", title: "D_Cortesia" },
                    { data: "T_Vendidos", title: "T_Vendidos" },
                    { data: "T_Comprados", title: "T_Comprados" },
                    { data: "T_Cortesia", title: "T_Cortesia" },
                    { data: "T_PagoManual", title: "T_PagoManual" },
                    { data: "T_Promocion", title: "T_Promocion" },
                    { data: "monto_venta_normal", title: "monto_venta_normal" },
                    { data: "monto_venta_tcredito", title: "monto_venta_tcredito" },
                    { data: "monto_compras", title: "monto_compras" },
                    { data: "monto_pagosmanuales", title: "monto_pagosmanuales" },
                    { data: "monto_compras_maq_normal", title: "monto_compras_maq_normal" },
                    { data: "monto_compras_maq_ingresomanual", title: "monto_compras_maq_ingresomanual" }
                ]
                ,
                "rowCallback": function (row, data) {
                    
                    $('td:eq(0)', row).css('background-color', '#F3F781');
                    $('td:eq(1)', row).css('background-color', '#F3F781');
                    $('td:eq(2)', row).css('background-color', '#F3F781');


                    $(row).focusin(function () {
                        
                        var item = this.cells[1].innerText;     
                        if (carga != 0) {
                            var turnoIni = $("#" + itemAnterior + "turno").attr("data-inicial");
                            var estadoIni = $("#" + itemAnterior + "estado").attr("data-inicial");
                            var fecha_aperturaIni = $("#" + itemAnterior + "fecha_apertura").attr("data-inicial");
                            
                           // console.log('uitem => ' +item + "estado");
                            var estadoSeleccionado = document.getElementById(item + "estado");   
                            var estadoSeleccionadoTurno = document.getElementById(item + "turno"); 
                            
                            var tipoCambioIni = $("#" + itemAnterior + "tcambio").attr("data-inicial");
                            var estadoFin = $("#" + itemAnterior + "estado").val();
                            var turnoFin = $("#" + itemAnterior + "turno").val();
                            var tipoCambioFin = $("#" + itemAnterior + "tcambio").val(); 
                            var fecha_aperturaFin = $("#" + itemAnterior + "fecha_apertura").val(); 

                            valor = "";
                            if (estadoIni == 0) {
                                valor = "Anulado";
                            }
                            if (estadoIni == 1) {
                                valor = "Abierto";
                            }
                            if (estadoIni == 2) {
                                valor = "Cerrado";
                            }
                            valorTurno = "";
                            if (turnoIni == 1) {
                                valor = "Turno 1";
                            }
                            if (turnoIni == 2) {
                                valor = "Turno 2";
                            }
                            if (turnoIni == 3) {
                                valor = "Turno 3";
                            }

                             datainicial = {
                                Item: item,
                                Estado: valor,
                                TipoCambio: tipoCambioIni,
                                Sala: $("#cboSala option:selected").text(),
                                 CodSala: $("#cboSala option:selected").data('id'),
                                 Turno: valorTurno,
                                 Fecha_apertura: fecha_aperturaIni
                            };


                            if (itemAnterior != item && (estadoIni != estadoFin || tipoCambioIni != tipoCambioFin || turnoIni != turnoFin || fecha_aperturaIni != fecha_aperturaFin) )
                            {
                                modificarCaja(itemAnterior);
                            }
                        } else {
                            carga = 1;
                        }
                    });
                    
                    $(row).focusout(function () {
                
                        var item = this.cells[1].innerText; 
                        itemAnterior = item; 
                    });
                },
                "initComplete": function (settings, json) {
                    $('#btnExcel').off("click").on('click', function () {
                        cabecerasnuevas = [];
                        cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
                        cabecerasnuevas.push({ nombre: "Fecha de Inicio", valor: $("#txtfechaInicio").val() });
                        cabecerasnuevas.push({ nombre: "Fecha Final", valor: $("#txtfechaFin").val() });
                        definicioncolumnas = [];
                        definicioncolumnas.push({ nombre: "tipo_cambio", tipo: "decimal", alinear: "center"});

                        columna_cambio = [{
                            nombre: "estado",
                            render: function (o) {
                                valor = "";
                                if (o == 0) {
                                    valor = "Anulado";
                                }
                                if (o == 1) {
                                    valor = "Abierto";
                                }
                                if (o == 2) {
                                    valor = "Cerrado";
                                }
                                return valor;
                            }
                        },
                        {
                            nombre: "tipo_cambio",
                            render: function (o) {
                                valor = o;
                                return valor;
                            }
                        }

                        ];

                        var ocultar = ["INSERTA FICHAS"];
                        funcionbotonesnuevo({
                            botonobjeto: this, ocultar: ocultar,
                            tablaobj: objetodatatable,
                            cabecerasnuevas: cabecerasnuevas,
                            definicioncolumnas: definicioncolumnas,
                            columna_cambio: columna_cambio
                        });
                        VistaAuditoria("CallCenter/CallCenterObtenerCajasExcel", "EXCEL", 0, "", 3);
                    });
                }
            });
        },
        error: function (request, status, error) {            
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
            $(".dateOnlyFechaInicio").datetimepicker({
                pickTime: false,
                format: 'DD/MM/YYYY'
            });
        }
    });
}

function getSelectedOption(sel) {
    var opt;
    for (var i = 0, len = sel.options.length; i < len; i++) {
        opt = sel.options[i];
        if (opt.selected === true) {
            break;
        }
    }
    return opt;
}

function ObtenerListaSalas() {
    comboImagen = $("#cboImagen");
    comboImagen.find('option').remove();
    $.ajax({
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
                $("#cboSala").append('<option value="' + value.UrlProgresivo + '"  data-id="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
    return false;
};

function modificarCajaSP(item) {
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    }
    itemModificando = item;
    var idEs = "#" + item + "estado";
    var idC = "#" + item + "tcambio";
     var url = ipPublicaG + "/servicio/ModificarCaja?estado=" + $("#" + item + "estado").val() + "&tipoCambio=" + $("#" + item + "tcambio").val() + "&item=" + item + "&turno=" + $("#" + item + "turno").val() + "&fecha_apertura=" + $("#" + item + "fecha_apertura").val();
    var datafinal = {
        Item: item,
        Estado: $("#" + item + "estado").val(),
        TipoCambio: $("#" + item + "tcambio").val(),
        Sala: $("#cboSala option:selected").text(),
        CodSala: $("#cboSala option:selected").data('id'),
        Turno: $("#" + item + "turno").val(),
        Fecha_apertura: $("#" + item + "fecha_apertura").val()
    };
    dataAuditoriaJSON(3, "CallCenter/ConsultaObtenerModificarCaja", "MODIFICAR CAJA", datainicial, datafinal);
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CallCenter/ConsultaObtenerModificarCaja",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            response = response.data;

            toastr.success("Se ha actualizado el registro <b style='text-transform: uppercase'>" + item + "</b><br>" + response);
            $("#" + itemModificando + "estado").attr('data-inicial', $("#" + item + "estado").val());
            $("#" + itemModificando + "tcambio").attr('data-inicial', $("#" + item + "tcambio").val());
            $("#" + itemModificando + "turno").attr('data-inicial', $("#" + item + "turno").val()); 
            $("#" + itemModificando + "fecha_apertura").attr('data-inicial', $("#" + item + "fecha_apertura").val());

            nuevo = undefined;
            cambioE = false;
            cambioT = false;
            cambioTu = false;
            cambioF = false;
        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
  
function modificarCaja(item) { 
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    }

    itemModificando = item;
    var idEs = "#" + item + "estado";
    var idC = "#" + item + "tcambio";
     
    var focusAotro = true;
     
    var js2 = $.confirm({
        icon: 'fa fa-spinner fa-spin',
        title: 'Esta Seguro que desea Actualizar la Caja ' + item +' ?',
        theme: 'black',
        animationBounce: 1.5,
        columnClass: 'col-md-6 col-md-offset-3',
        confirmButtonClass: 'btn-info',
        cancelButtonClass: 'btn-warning',
        confirmButton: "confirmar",
        cancelButton: 'Cerrar',
        content: "",
        confirm: function () {
            debugger;
            var url = ipPublicaG + "/servicio/ModificarCaja?estado=" + $("#" + item + "estado").val() + "&tipoCambio=" + $("#" + item + "tcambio").val() + "&item=" + item + "&turno=" + $("#" + item + "turno").val() + "&fecha_apertura=" + $("#" + item + "fecha_apertura").val();
            var datafinal = {
                Item: item,
                Estado: $("#" + item + "estado").val(),
                TipoCambio: $("#" + item + "tcambio").val(),
                Sala: $("#cboSala option:selected").text(),
                CodSala: $("#cboSala option:selected").data('id'),
                Turno: $("#" + item + "turno").val(),
                Fecha_apertura: $("#" + item + "fecha_apertura").val()
            };
            dataAuditoriaJSON(3, "CallCenter/ConsultaObtenerModificarCaja", "MODIFICAR CAJA", datainicial, datafinal);
            $.ajax({
                type: "POST",
                cache: false,
                url: basePath + "CallCenter/ConsultaObtenerModificarCaja",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ url: url }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (response) {
                    response = response.data; 
                    toastr.success("Se ha actualizado el registro <b style='text-transform: uppercase'>" + item + "</b><br>" + response);
                    $("#" + itemModificando + "estado").attr('data-inicial', $("#" + item + "estado").val());
                    $("#" + itemModificando + "tcambio").attr('data-inicial', $("#" + item + "tcambio").val());
                    $("#" + itemModificando + "turno").attr('data-inicial', $("#" + item + "turno").val());
                    $("#" + itemModificando + "fecha_apertura").attr('data-inicial', $("#" + item + "fecha_apertura").val());
                    nuevo = undefined;
                    cambioE = false;
                    cambioT = false;
                    cambioTu = false;
                    cambioF = false;
                },
                error: function (request, status, error) {
                    toastr.error("Error De Conexion, Servidor no Encontrado.");
                },
                complete: function (resul) {
                    $.LoadingOverlay("hide");
                }
                });
            
        },
        cancel: function () {  
            var turnoIni = $("#" + itemModificando + "turno").attr("data-inicial");
            var estadoIni = $("#" + itemModificando + "estado").attr("data-inicial");
            var tipoCambioIni = $("#" + itemModificando + "tcambio").attr("data-inicial");
            var fecha_aperturaIni = $("#" + itemModificando + "fecha_apertura").attr("data-inicial");
            $("#" + itemModificando + "turno").val(turnoIni);
            $("#" + itemModificando + "estado").val(estadoIni);
            $("#" + itemModificando + "tcambio").val(tipoCambioIni);
            $("#" + itemModificando + "fecha_apertura").val(fecha_aperturaIni);
        }
    });
    
}



function InsertarTipoFichas(item) { 
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    }
    var js2 = $.confirm({
        icon: 'fa fa-spinner fa-spin',
        title: 'Esta Seguro que desea agregar tipo de fichas a la Caja  ' + item + ' ?',
        theme: 'black',
        animationBounce: 1.5,
        columnClass: 'col-md-6 col-md-offset-3',
        confirmButtonClass: 'btn-info',
        cancelButtonClass: 'btn-warning',
        confirmButton: "confirmar",
        cancelButton: 'Cerrar',
        content: "",
        confirm: function () {
            var url = ipPublicaG + "/servicio/InsertaDetalleTipoFicha?item=" + item;
            var datafinal = {
                Item: item,
                Sala: $("#cboSala option:selected").text(),
                CodSala: $("#cboSala option:selected").data('id'),
            };
            dataAuditoriaJSON(3, "CallCenter/InsertaDetalleTipoFicha", "INSERTAR FICHAS", datainicial, datafinal);
            $.ajax({
                type: "POST",
                cache: false,
                url: basePath + "CallCenter/InsertaDetalleTipoFicha",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ url: url }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (response) {
                    response = response.data;
                    if (response == true) {
                        toastr.success("Se agregaron tipo de fichas para la caja");
                    }
                    else {
                        toastr.warning("No hay fichas para agregar");
                    }  
                },
                error: function (request, status, error) {
                    toastr.error("Error De Conexion, Servidor no Encontrado.");
                },
                complete: function (resul) {
                    $.LoadingOverlay("hide");
                }
            });

        },
        cancel: function () { 

        },

    });

}
VistaAuditoria("CallCenter/CallCenterObtenerCajas", "VISTA", 0, "", 3);
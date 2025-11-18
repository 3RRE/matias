var itemAnterior
var itemModificando
var carga
$(document).ready(function () {
    carga = 0;
    ipPublicaG = "";
    nombretabla = "";
   
    ObtenerListaSalas();
    $("#cboSala").select2();
    $("#cboTabla").select2();
    $(document).on('change', '#cboSala', function (e) {
        var ipPublica = $(this).val();
        ipPublicaG = ipPublica;
        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }
    });

    $(document).on('change', '#cboTabla', function (e) {
        nombretabla = $(this).val();
        MostrarTablas();
    });
    $(".dateOnly").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
   
    $("#btnBuscar").on("click", function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if ($("#cboTabla").val() == "") {
            toastr.error("Seleccione una Tabla, Mensaje Servidor");
            return false;
        }
        buscarTicket();
    }); 
   
    
});

function buscarTicket() {
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    }
    if (nombretabla == "Procesos_TITO") {
        dataAuditoria(1, "#formfiltro", 3, "CallCenter/ConsultaObtenerProcesosTitoJson", "BOTON BUSCAR");
        var url = ipPublicaG + "/servicio/ObtenerProcesosTito?fechaInicio=" + $("#pro_ti_fechaInicio").val() +
            "&fechaFin=" + $("#pro_ti_fechaFin").val() + "&procc_Cod_Ticket=" + $("#pro_ti_cod_ticket").val() +
            "&procc_Nro_Maquina=" + $("#pro_ti_nro_maquina").val() + "&procc_Item_Caja="+ $("#pro_ti_item_caja").val();
        if ($("#pro_ti_fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        var addtabla = $(".contenedor_tabla");
        var addbtn = $(".contenedor_btn");
        var addbtnexcel = $(".contenedor_btn_excel");
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "CallCenter/ConsultaObtenerProcesosTitoJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ url: url }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                response = response.data;
                $(addbtn).empty();
                $(addbtn).append('<button type="button" id="btnEliminarProcTito" class="btn btn-danger btn-sm col-md-12 col-xs-12"><span class="glyphicon glyphicon-file"></span>Eliminar</button >');
                $(addbtnexcel).empty();
                $(addbtnexcel).append('<button type="button" id="btnExcel" class="btn btn-primary btn-sm col-md-12 col-xs-12"><span class="glyphicon glyphicon-file" ></span >Excel</button >');
                $(addtabla).empty();
                $(addtabla).append('<table id="table1" class="table table-condensed table-bordered table-hover"></table>');
                objetodatatable = $("#table1").DataTable({
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
                        { data: "PROCC_Cod_Proceso", title: "PROCC_Cod_Proceso" },
                        { data: "PROCC_Id_Tipo_Proceso", title: "PROCC_Id_Tipo_Proceso" },
                        {
                            data: "PROCC_Fecha_Apertura", title: "PROCC_Fecha_Apertura",
                            "render": function (value) {
                                return moment(value).format('DD/MM/YYYY h:mm:ss a');
                            }
                        },
                        {
                            data: "PROCC_Fecha_Proceso", title: "PROCC_Fecha_Proceso",
                            "render": function (value) {
                                return moment(value).format('DD/MM/YYYY h:mm:ss a');
                            }
                        },
                        {
                            data: "PROCC_Fecha_Emision", title: "PROCC_Fecha_Emision",
                            "render": function (value) {
                                return moment(value).format('DD/MM/YYYY h:mm:ss a');
                            }
                        },
                        { data: "PROCC_Id_Tipo_Documento", title: "PROCC_Id_Tipo_Documento" },
                        { data: "PROCC_Nro_Documento", title: "PROCC_Nro_Documento" },
                        { data: "PROCC_Cod_Cliente", title: "PROCC_Cod_Cliente" },
                        { data: "PROCC_Cod_Ticket", title: "PROCC_Cod_Ticket" },
                        { data: "PROCC_Nro_Maquina", title: "PROCC_Nro_Maquina" },
                        { data: "PROCC_Cod_Tarjeta", title: "PROCC_Cod_Tarjeta" },
                        { data: "PROCC_Desc_Concepto", title: "PROCC_Desc_Concepto" },
                        {
                            data: "PROCC_Monto_Dinero", title: "PROCC_Monto_Dinero",
                            "render": function (i,j,value) {
                                return '<input  type="text" data-inicial = "' + value.PROCC_Monto_Dinero + '" data-id="' + value.PROCC_Cod_Proceso + '"  step="0.1" id="' + value.PROCC_Cod_Proceso + '_MontoDinero" class="form-control input-sm " value="' + value.PROCC_Monto_Dinero + '">';
                            },
                        },
                        { data: "PROCC_Igv", title: "PROCC_Igv" },
                        { data: "PROCC_Sub_Total", title: "PROCC_Sub_Total" },
                        {
                            data: "PROCC_Total", title: "PROCC_Total",
                            "render": function (i,j,value) {
                                return '<input  type="text" data-inicial = "' + value.PROCC_Total + '" data-id="' + value.PROCC_Cod_Proceso +'"  step="0.1" id="' + value.PROCC_Cod_Proceso + '_Total" class="form-control input-sm " value="' + value.PROCC_Total + '">';
                            }
                        },
                        { data: "PROCC_ImpuestoPM", title: "PROCC_ImpuestoPM" },
                        {
                            data: "PROCC_Letras", title: "PROCC_Letras",
                            "render": function (i,j,value) {
                                return '<input type="text" data-inicial = "' + value.PROCC_Letras + '" data-id="' + value.PROCC_Cod_Proceso +'" step="any" id="' + value.PROCC_Cod_Proceso + '_Letras" class="form-control input-sm " value="' + value.PROCC_Letras + '">';
                            }
                        },
                        { data: "PROCC_Id_TipoPago", title: "PROCC_Id_TipoPago" },
                        { data: "PROCC_desc_TipoPago", title: "PROCC_desc_TipoPago" },
                        { data: "PROCC_Nro_Tarjeta_Credito", title: "PROCC_Nro_Tarjeta_Credito" },
                        { data: "PROCC_idTipoMoneda", title: "PROCC_idTipoMoneda" },
                        { data: "PROCC_Tipo_Cambio", title: "PROCC_Tipo_Cambio" },
                        { data: "PROCC_Estado_Transaccion", title: "PROCC_Estado_Transaccion" },
                        { data: "PROCC_Usuario", title: "PROCC_Usuario" },
                        { data: "PROCC_Item_Caja", title: "PROCC_Item_Caja" },
                        { data: "PROCC_Serie_Doc", title: "PROCC_Serie_Doc" },
                        { data: "PROCC_Nro_Doc", title: "PROCC_Nro_Doc" },
                        {
                            data: "PROCC_Fecha_Anulacion", title: "PROCC_Fecha_Anulacion",
                            "render": function (value) {
                                return moment(value).format('DD/MM/YYYY h:mm:ss a');
                            }
                        },
                        { data: "procc_razon_social", title: "procc_razon_social" },
                        { data: "procc_ruc", title: "procc_ruc" },
                        { data: "procc_nro_caja", title: "procc_nro_caja" },
                        { data: "PROCC_CODPER_ACCESO", title: "PROCC_CODPER_ACCESO" },
                        { data: "PROCC_OBSERVACION", title: "PROCC_OBSERVACION" },
                        { data: "MontoNoCobrablePromo", title: "MontoNoCobrablePromo" },
                        { data: "procc_N_Comprobante", title: "procc_N_Comprobante" },
                        { data: "Procc_ModalidadPago", title: "Procc_ModalidadPago" },
                        { data: "IdTipoPago", title: "IdTipoPago" },
                        { data: "PROCC_MONTO_DOLARES", title: "PROCC_MONTO_DOLARES" },
                        { data: "procc_tipo_cliente_destino", title: "procc_tipo_cliente_destino" },
                        { data: "PROCC_idTipoMoneda_paga", title: "PROCC_idTipoMoneda_paga" },
                        { data: "PROCC_monto_paga", title: "PROCC_monto_paga" },
                        { data: "procc_motivo_pm", title: "procc_motivo_pm" },
                        { data: "procc_tipo_venta_ini", title: "procc_tipo_venta_ini" }
                    ]
                    ,
                    "rowCallback": function (row, data) {

                        $(row).focusin(function () {
                            var item = this.cells[0].innerText;
                            if (carga != 0) {

                                var MontoDineroIni = $("#" + itemAnterior + "_MontoDinero").attr("data-inicial");
                                var MontoDineroFin = $("#" + itemAnterior + "_MontoDinero").val();

                                var TotalIni = $("#" + itemAnterior + "_Total").attr("data-inicial");
                                var Totalfin = $("#" + itemAnterior + "_Total").val();

                                var LetrasIni = $("#" + itemAnterior + "_Letras").attr("data-inicial");                                
                                var LetrasFin = $("#" + itemAnterior + "_Letras").val();

                                if (itemAnterior != item && (TotalIni != Totalfin || LetrasIni != LetrasFin || MontoDineroIni != MontoDineroFin)) {
                                    ModificarProcesoTito(itemAnterior);
                                }
                            } else {
                                carga = 1;
                            }
                        });

                        $(row).focusout(function () {
                            var item = this.cells[0].innerText;
                            itemAnterior = item;
                        });
                    }
                    ,"initComplete": function (settings, json) {

                        $('#btnExcel').off("click").on('click', function () {

                            cabecerasnuevas = [];
                            cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
                            cabecerasnuevas.push({ nombre: "Tabla", valor: $("#cboTabla option:selected").text() });

                            columna_cambio = [
                                {
                                    nombre: "PROCC_Total",
                                    render: function (o) {
                                        return o;
                                    }
                                },
                                {
                                    nombre: "PROCC_Monto_Dinero",
                                    render: function (o) {
                                        return o;
                                    }
                                },
                                {
                                    nombre: "PROCC_Letras",
                                    render: function (o) {
                                        return o;
                                    }
                                }
                            ]

                            var ocultar = [];//"Accion";
                            funcionbotonesnuevo({
                                botonobjeto: this, ocultar: ocultar,
                                tablaobj: objetodatatable,
                                cabecerasnuevas: cabecerasnuevas, columna_cambio: columna_cambio
                            });
                            VistaAuditoria("CallCenter/ConsultaObtenerProcesosTitoJsonExcel", "EXCEL", 0, "", 3);
                        });

                    },
                });
                $('#table1 tbody').on('click', 'tr', function () {
                    $(this).toggleClass('selected');
                });
                $("#btnEliminarProcTito").click(function () {
                    var i = 0;
                    var dataArr = [];
                    var rows = $('tr.selected');
                    var rowData = objetodatatable.rows(rows).data();
                    $.each($(rowData), function (key, value) {
                        dataArr.push(value["PROCC_Cod_Proceso"]);
                    });

                    if (dataArr.length != 0) {
                        var js2 = $.confirm({
                            icon: 'fa fa-spinner fa-spin',
                            title: 'Esta Seguro que desea Eliminar los Tickets ?',
                            theme: 'black',
                            animationBounce: 1.5,
                            columnClass: 'col-md-6 col-md-offset-3',
                            confirmButtonClass: 'btn-info',
                            cancelButtonClass: 'btn-warning',
                            confirmButton: "confirmar",
                            cancelButton: 'Cerrar',
                            content: "",
                            confirm: function () {
                                for (i; i < dataArr.length; i++) {
                                    EliminarProcesoTito(dataArr[i], objetodatatable);
                                }

                            },
                            cancel: function () {

                            },

                        });
                    }


                });

            },
            error: function (request, status, error) {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    }
    else if (nombretabla == "Detalle_movaux_por_maquina") { 
        dataAuditoria(1, "#formfiltro", 3, "CallCenter/ConsultaObtenerDetalle_movaux_por_maquinaJson", "BOTON BUSCAR");
        var url = ipPublicaG + "/servicio/ObtenerDetalle_movaux_por_maquina?fechaInicio=" + $("#det_mov_maq_fechaInicio").val() +
            "&maq_alterno=" + $("#det_mov_maq_alterno").val() + "&item=" + $("#det_mov_maq_item").val();
        if ($("#det_mov_maq_fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        var addtabla = $(".contenedor_tabla");
        var addbtn = $(".contenedor_btn");
        var addbtnexcel = $(".contenedor_btn_excel");
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "CallCenter/ConsultaObtenerDetalle_movaux_por_maquinaJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ url: url }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                response = response.data;
                $(addbtn).empty();
                $(addbtnexcel).empty();
                $(addbtnexcel).append('<button type="button" id="btnExcel" class="btn btn-primary btn-sm col-md-12 col-xs-12"><span class="glyphicon glyphicon-file" ></span >Excel</button >');
                $(addtabla).empty();
                $(addtabla).append('<table id="table2" class="table table-condensed table-bordered table-hover"></table>');

                objetodatatable = $("#table2").DataTable({
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
                        { data: "item_maqui", title: "item_maqui" },
                        { data: "item", title: "item" },
                        { data: "idtipoficha", title: "idtipoficha" },
                        { data: "idtipomoneda", title: "idtipomoneda" },
                        { data: "valorficha", title: "valorficha" },
                        { data: "nro", title: "nro" },
                        { data: "maq_alterno", title: "maq_alterno" },
                        { data: "modelo", title: "modelo" },
                        { data: "salida", title: "salida" },
                        { data: "ingreso", title: "ingreso" },
                        {
                            data: "pagomanual", title: "pagomanual",
                            "render": function (i,j,value) {
                                return '<input class="form-control" data-inicial = "'+ value.pagomanual + '"  id="' + value.item_maqui + '_pagomanual" data-id="' + value.item_maqui + '" value="' + value.pagomanual + '" />';
                            }
                        },
                        { data: "valor_es_un", title: "valor_es_un" },
                        { data: "estado", title: "estado" },
                        { data: "tipomaquina", title: "tipomaquina" },
                        { data: "item_pmr", title: "item_pmr" },
                        { data: "CancelldCredit_Fin", title: "CancelldCredit_Fin" },
                        { data: "JackPot_Fin", title: "JackPot_Fin" },
                        {
                            data: "hora_registro", title: "hora_registro",
                            "render": function (value) {
                                return moment(value).format('DD/MM/YYYY h:mm:ss a');
                            }
                        },
                        { data: "estado_registro", title: "estado_registro" },
                        { data: "codPer", title: "codPer" },
                        { data: "Cod_Proceso", title: "Cod_Proceso" },
                        { data: "Cod_Caja", title: "Cod_Caja" },
                        { data: "NroTicket", title: "NroTicket" },
                        { data: "cliente_dni", title: "cliente_dni" },
                        { data: "serie_num", title: "serie_num" },
                        { data: "cod_asignacion_ficha", title: "cod_asignacion_ficha" },
                        { data: "IngresoManual", title: "IngresoManual" },
                        { data: "CAJERA_GSI", title: "CAJERA_GSI" },
                        { data: "var_tablet", title: "var_tablet" }
                    ],
                    "rowCallback": function (row, data) {
                        $(row).focusin(function () {
                            var item = this.cells[0].innerText;
                            if (carga != 0) {
                                //debugger
                                console.log("entro");
                                
                                var pagomanual_ini = $("#" + itemAnterior + "_pagomanual").attr("data-inicial");

                                var estadoSeleccionado = document.getElementById(item + "_pagomanual");

                                var pagomanual_Fin = $("#" + itemAnterior + "_pagomanual").val();
                                
                                if (itemAnterior != item && (pagomanual_ini != pagomanual_Fin)) {
                                    ModificarDetalleMovaux(itemAnterior);
                                }
                            } else {
                                carga = 1;
                            }
                        });

                        $(row).focusout(function () {
                            var item = this.cells[0].innerText;
                            itemAnterior = item;
                        });
                    },
                    "initComplete": function (settings, json) {

                        $('#btnExcel').off("click").on('click', function () {

                            cabecerasnuevas = [];
                            cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
                            cabecerasnuevas.push({ nombre: "Tabla", valor: $("#cboTabla option:selected").text() });

                            //pagomanual
                            columna_cambio = [{
                                nombre: "pagomanual",
                                render: function (o) {
                                    return o;
                                }
                            }
                            ]

                            var ocultar = [];//"Accion";
                            funcionbotonesnuevo({
                                botonobjeto: this, ocultar: ocultar,
                                tablaobj: objetodatatable,
                                cabecerasnuevas: cabecerasnuevas, columna_cambio: columna_cambio
                            });
                            VistaAuditoria("CallCenter/ConsultaObtenerDetalle_movaux_por_maquinaJsonExcel", "EXCEL", 0, "", 3);
                        });

                    },
                });

            },
            error: function (request, status, error) {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    }
    else if (nombretabla == "Det0001TTO_00H") {
        dataAuditoria(1, "#formfiltro", 3, "CallCenter/ConsultaObtenerDet0001TTO_00HJson", "BOTON BUSCAR");
        var url = ipPublicaG + "/servicio/ObtenerDet0001TTO_00H?fechaInicio=" + $("#det_tt0_fechaInicio").val() +
            "&Punto_venta=" + $("#det_tt0_Punto_venta").val() + "&Punto_venta_fin=" + $("#det_tt0_Punto_venta_fin").val() +
            "&Tito_NroTicket=" + $("#det_tt0_Tito_NroTicket").val() + "&fechaFin=" + $("#det_tt0_fechaFin").val();
        if ($("#det_tt0_fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        var addtabla = $(".contenedor_tabla");
        var addbtn = $(".contenedor_btn");
        var addbtnexcel = $(".contenedor_btn_excel");
        var estados = [
            { "value": "0", "nombre": "Anulado" },
            { "value": "1", "nombre": "No Cobrado" },
            { "value": "2", "nombre": "Cobrado" },
            { "value": "3", "nombre": "Vencido" }
        ];
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "CallCenter/ConsultaObtenerDet0001TTO_00HJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ url: url }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                response = response.data;
                $(addbtn).empty();
                $(addbtn).append('<button type="button" id="btnEliminarDet00H" class="btn btn-danger btn-sm col-md-12 col-xs-12"><span class="glyphicon glyphicon-file"></span>Eliminar</button >');
                $(addbtnexcel).empty();
                $(addbtnexcel).append('<button type="button" id="btnExcel" class="btn btn-primary btn-sm col-md-12 col-xs-12"><span class="glyphicon glyphicon-file" ></span >Excel</button >');
                $(addtabla).empty();
                $(addtabla).append('<table id="table3" class="table table-condensed table-bordered table-hover"></table>');
                objetodatatable = $("#table3").DataTable({
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
                        { data: "Item", title: "Item" },
                        {
                            data: "Fecha_Apertura", title: "Fecha_Apertura",
                            "render": function (value) {
                                return moment(value).format('DD/MM/YYYY h:mm:ss a');
                            }
                        },
                        { data: "Tipo_venta", title: "Tipo_venta" },
                        { data: "Punto_venta", title: "Punto_venta" },
                        {
                            data: "Tito_fechaini", title: "Tito_fechaini",
                            "render": function (value) {
                                return moment(value).format('DD/MM/YYYY h:mm:ss a');
                            }
                        },
                        { data: "Tito_NroTicket", title: "Tito_NroTicket" },
                        {
                            data: "Tito_MontoTicket", title: "Tito_MontoTicket",
                            "render": function (value,i,j) {
                                return '<input type="Number" data-inicial="' + j.Tito_MontoTicket + '" data-id="' + j.Item + '" step="0.1" id="' + j.Item + '_MontoTicket" class="form-control input-sm " value="' + j.Tito_MontoTicket + '">';
                            }
                            
                        },
                        { data: "Tito_MTicket_NoCobrable", title: "Tito_MTicket_NoCobrable" },
                        { data: "Tipo_venta_fin", title: "Tipo_venta_fin" },
                        { data: "Punto_venta_fin", title: "Punto_venta_fin" },
                        {
                            data: "Tito_fechafin", title: "Tito_fechafin",
                            "render": function (value) {
                                return moment(value).format('DD/MM/YYYY h:mm:ss a');
                            }
                        },
                        { data: "codclie", title: "codclie" },
                        { data: "tipo_ticket", title: "tipo_ticket" },
                        {
                            data: "Estado", title: "Estado",                           
                            "render": function (i,j,value) {
                                var $select = $("<select></select>", {
                                    "id": value.Item + "_Estado",
                                    "class": "form-control",
                                    "data-inicial": value.Estado,
                                    "data-id": value.Item

                                });
                                var i = 0;
                                for (i; i < estados.length; i++) {
                                    var $option = $("<option></option>", {
                                        "text": estados[i].nombre,
                                        "value": estados[i].value
                                    });
                                    //debugger
                                    if (value.Estado == estados[i].value) {
                                        $option.attr("selected", "selected");
                                    }                                    
                                    $select.append($option);
                                }
                                return $select.prop("outerHTML");
                            }
                        },
                        { data: "IdTipoMoneda", title: "IdTipoMoneda" },
                        { data: "Motivo", title: "Motivo" },
                        { data: "IdTipoPago", title: "IdTipoPago" },
                        { data: "Tipo_Proceso", title: "Tipo_Proceso" },
                        { data: "r_Estado", title: "r_Estado" },
                        { data: "Tipo_Ingreso", title: "Tipo_Ingreso" },
                        {
                            data: "Fecha_Apertura_Real", title: "Fecha_Apertura_Real",
                            "render": function (value) {
                                if (moment(value).format('DD/MM/YYYY') == "31/12/0000") {
                                    return 'Vacio';
                                } else {
                                    return moment(value).format('DD/MM/YYYY h:mm:ss a');
                                }
                            }
                        },
                        { data: "PuntoVentaMin", title: "PuntoVentaMin" },
                        {
                            data: "fecha_reactiva", title: "fecha_reactiva",
                            "render": function (value) {                   
                                if (moment(value).format('DD/MM/YYYY') == "31/12/0000") {
                                    return 'Vacio';
                                } else {
                                    return moment(value).format('DD/MM/YYYY h:mm:ss a');
                                }
                            }
                        },
                        { data: "turno", title: "turno" },
                        { data: "codCaja", title: "codCaja" },
                        { data: "player_tracking", title: "player_tracking" },
                    ]
                    ,
                    "rowCallback": function (row, data) {

                        $(row).focusin(function () {
                            var item = this.cells[0].innerText;
                            if (carga != 0) {

                                var MontoIni = $("#" + itemAnterior + "_MontoTicket").attr("data-inicial");
                                var Montofin = $("#" + itemAnterior + "_MontoTicket").val();

                                var EstadoIni = $("#" + itemAnterior + "_Estado").attr("data-inicial");
                                var EstadoFin = $("#" + itemAnterior + "_Estado").val();

                                if (itemAnterior != item && (MontoIni != Montofin || EstadoIni != EstadoFin)) {
                                    ModificarDet0001TTO_00H(itemAnterior);
                                }
                            } else {
                                carga = 1;
                            }
                        });

                        $(row).focusout(function () {
                            var item = this.cells[0].innerText;
                            itemAnterior = item;
                        });
                    },
                    "initComplete": function (settings, json) {

                        $('#btnExcel').off("click").on('click', function () {

                            cabecerasnuevas = [];
                            cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
                            cabecerasnuevas.push({ nombre: "Tabla", valor: $("#cboTabla option:selected").text() });

                            columna_cambio = [
                                {
                                    nombre: "Tito_MontoTicket",
                                    render: function (o) {
                                        return o;
                                    }
                                },
                                {
                                    nombre: "estado",
                                    render: function (o) {
                                        valor = "";
                                        if (o == 0) {
                                            valor = "Anulado";
                                        }
                                        if (o == 1) {
                                            valor = "No Cobrado";
                                        }
                                        if (o == 2) {
                                            valor = "Cobrado";
                                        }
                                        if (o == 3) {
                                            valor = "Vencido";
                                        }
                                        return valor;
                                    }
                                }
                            
                            ]

                            var ocultar = [];//"Accion";
                            funcionbotonesnuevo({
                                botonobjeto: this, ocultar: ocultar,
                                tablaobj: objetodatatable,
                                cabecerasnuevas: cabecerasnuevas, columna_cambio: columna_cambio
                            });
                            VistaAuditoria("CallCenter/ConsultaObtenerDet0001TTO_00HJsonExcel", "EXCEL", 0, "", 3);
                        });

                    },
                });
                $('#table3 tbody').on('click', 'tr', function () {
                    $(this).toggleClass('selected');
                });
                $("#btnEliminarDet00H").click(function () {
                    var i = 0;
                    var dataArr = [];
                    var rows = $('tr.selected');
                    var rowData = objetodatatable.rows(rows).data();
                    $.each($(rowData), function (key, value) {
                        dataArr.push(value["Item"]);
                    });
                    if (dataArr.length != 0) {
                        $.confirm({
                            icon: 'fa fa-spinner fa-spin',
                            title: 'Esta Seguro que desea Eliminar los Tickets ?',
                            theme: 'black',
                            animationBounce: 1.5,
                            columnClass: 'col-md-6 col-md-offset-3',
                            confirmButtonClass: 'btn-info',
                            cancelButtonClass: 'btn-warning',
                            confirmButton: "confirmar",
                            cancelButton: 'Cerrar',
                            content: "",
                            confirm: function () {
                                for (i; i < dataArr.length; i++) {
                                    EliminarDet0001TTO_00H(dataArr[i], objetodatatable);
                                }
                            },
                            cancel: function () {

                            },

                        });
                    }
                });
            },
            error: function (request, status, error) {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    }
    else if (nombretabla == "Det0001TTO") {
        dataAuditoria(1, "#formfiltro", 3, "CallCenter/ConsultaObtenerDet0001TTOJson", "BOTON BUSCAR");
        var url = ipPublicaG + "/servicio/ObtenerDet0001TTO?fechaInicio=" + $("#det_tito_fechaInicio").val() + "&Tito_NroTicket=" + $("#det_tito_Tito_NroTicket").val() + "&PuntoVenta=" + $("#det_tito_Tito_PuntoVenta").val() + "&item=" + $("#det_tito_item").val();
        if ($("#det_tito_fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        var addtabla = $(".contenedor_tabla");
        var addbtn = $(".contenedor_btn");
        var addbtnexcel = $(".contenedor_btn_excel");
        var estados = [
            { "value": "0", "nombre": "Anulado" },
            { "value": "1", "nombre": "No Cobrado" },
            { "value": "2", "nombre": "Cobrado" },
            { "value": "3", "nombre": "Vencido" }
        ];
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "CallCenter/ConsultaObtenerDet0001TTOJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ url: url }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                response = response.data;
                $(addbtn).empty();
                $(addbtn).append('<button type="button" id="btnEliminarDet00H" class="btn btn-danger btn-sm col-md-12 col-xs-12"><span class="glyphicon glyphicon-file"></span>Eliminar</button >');
                $(addbtnexcel).empty();
                $(addbtnexcel).append('<button type="button" id="btnExcel" class="btn btn-primary btn-sm col-md-12 col-xs-12"><span class="glyphicon glyphicon-file" ></span >Excel</button >');
                $(addtabla).empty();
                $(addtabla).append('<table id="table4" class="table table-condensed table-bordered table-hover"></table>');
                
                objetodatatable = $("#table4").DataTable({
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
                        { data: "Item", title: "Item" },
                        { data: "Tito_NroTicket", title: "Tito_NroTicket" },
                        { data: "Tito_MontoTicket", title: "Tito_MontoTicket" },
                        { data: "Tito_MTicket_NoCobrable", title: "Tito_MTicket_NoCobrable" },
                        {
                            data: "Estado", title: "Estado",
                            "render": function (i,j,value) {
                                
                                var $select = $("<select></select>", {
                                    "id": value.Item + "_Estado",
                                    "class": "form-control",
                                    "data-inicial": value.Estado,
                                    "data-id": value.Item

                                });
                                var i = 0;
                                for (i; i < estados.length; i++) {
                                    var $option = $("<option></option>", {
                                        "text": estados[i].nombre,
                                        "value": estados[i].value
                                    });
                                    if (value.Estado == estados[i].value) {
                                        $option.attr("selected", "selected");
                                    }
                                    $select.append($option);
                                }
                                return $select.prop("outerHTML");
                            },
                        },
                        { data: "IdTipoMoneda", title: "IdTipoMoneda" },
                        { data: "IdTipoPago", title: "IdTipoPago" },
                        {
                            data: "FechaReg", title: "FechaReg",
                            "render": function (value) {
                                return moment(value).format('DD/MM/YYYY h:mm:ss a');
                            }
                        },
                        { data: "tipo_ticket", title: "tipo_ticket" },
                        { data: "PuntoVenta", title: "PuntoVenta" },
                        { data: "PuntoVentaMin", title: "PuntoVentaMin" },
                        { data: "Bloqueo", title: "Bloqueo" },
                        { data: "Binario", title: "Binario" },
                        { data: "impreso", title: "impreso" }
                    ],
                    "rowCallback": function (row, data) {

                        $(row).focusin(function () {
                            var item = this.cells[0].innerText;
                            if (carga != 0) {

                                var EstadoIni = $("#" + itemAnterior + "_Estado").attr("data-inicial");
                                var EstadoFin = $("#" + itemAnterior + "_Estado").val();

                                if (itemAnterior != item && (EstadoIni != EstadoFin)) {
                                    ModificarDet0001TTO(itemAnterior);
                                }
                            } else {
                                carga = 1;
                            }
                        });

                        $(row).focusout(function () {
                            var item = this.cells[0].innerText;
                            itemAnterior = item;
                        });
                    }
                    ,
                    "initComplete": function (settings, json) {

                        $('#btnExcel').off("click").on('click', function () {

                            cabecerasnuevas = [];
                            cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
                            cabecerasnuevas.push({ nombre: "Tabla", valor: $("#cboTabla option:selected").text() });

                            columna_cambio = [{
                                nombre: "estado",
                                render: function (o) {
                                    valor = "";
                                    if (o == 0) {
                                        valor = "Anulado";
                                    }
                                    if (o == 1) {
                                        valor = "No Cobrado";
                                    }
                                    if (o == 2) {
                                        valor = "Cobrado";
                                    }
                                    if (o == 3) {
                                        valor = "Vencido";
                                    }
                                    return valor;
                                }
                            }

                            ]

                            var ocultar = [];//"Accion";
                            funcionbotonesnuevo({
                                botonobjeto: this, ocultar: ocultar,
                                tablaobj: objetodatatable,
                                cabecerasnuevas: cabecerasnuevas, columna_cambio: columna_cambio
                            });
                            VistaAuditoria("CallCenter/ConsultaObtenerDet0001TTOJsonExcel", "EXCEL", 0, "", 3);
                        });

                    },
                });
                $('#table4 tbody').on('click', 'tr', function () {
                    $(this).toggleClass('selected');
                });
                $("#btnEliminarDet00H").click(function () {
                    //console.log(objetodatatable.rows('.selected').data().length + "Seleccionados");
                    var i = 0;
                    var dataArr = [];
                    var rows = $('tr.selected');
                    var rowData = objetodatatable.rows(rows).data();
                    $.each($(rowData), function (key, value) {
                        dataArr.push(value["Item"]);
                    });
                    if (dataArr.length != 0) {
                        var js2 = $.confirm({
                            icon: 'fa fa-spinner fa-spin',
                            title: 'Esta Seguro que desea Eliminar los Tickets ?',
                            theme: 'black',
                            animationBounce: 1.5,
                            columnClass: 'col-md-6 col-md-offset-3',
                            confirmButtonClass: 'btn-info',
                            cancelButtonClass: 'btn-warning',
                            confirmButton: "confirmar",
                            cancelButton: 'Cerrar',
                            content: "",
                            confirm: function () {
                                for (i; i < dataArr.length; i++) {
                                    EliminarDet0001TTO(dataArr[i], objetodatatable);
                                }

                            },
                            cancel: function () {

                            },

                        });
                    }
                });
                

            },
            error: function (request, status, error) {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    }
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
}

function MostrarTablas() {
    if (nombretabla == "Procesos_TITO") {
        $("#det_mov_maq").addClass("hidden");
        $("#det_tt0").addClass("hidden");
        $("#det_tito").addClass("hidden");
        $("#proc_tito").removeClass("hidden");
    }
    else if (nombretabla == "Detalle_movaux_por_maquina") {

        $("#det_tt0").addClass("hidden");
        $("#det_tito").addClass("hidden");
        $("#proc_tito").addClass("hidden");
        $("#det_mov_maq").removeClass("hidden");
    }
    else if (nombretabla == "Det0001TTO_00H") {

        $("#det_tito").addClass("hidden");
        $("#proc_tito").addClass("hidden");
        $("#det_mov_maq").addClass("hidden");
        $("#det_tt0").removeClass("hidden");
    }
    else if (nombretabla == "Det0001TTO") {
        $("#proc_tito").addClass("hidden");
        $("#det_mov_maq").addClass("hidden");
        $("#det_tt0").addClass("hidden");
        $("#det_tito").removeClass("hidden");        
    }
}

function ModificarProcesoTito(item) {

    itemModificando = item;

    var js2;
        js2 = $.confirm({
        icon: 'fa fa-spinner fa-spin',
        title: 'Esta Seguro que desea Actualizar el Ticket '+ item +'?',
        theme: 'black',
        animationBounce: 1.5,
        columnClass: 'col-md-6 col-md-offset-3',
        confirmButtonClass: 'btn-info',
        cancelButtonClass: 'btn-warning',
        confirmButton: "confirmar",
        cancelButton: 'Aún No',
        content: "",
            confirm: function () {
                var url = ipPublicaG + "/servicio/ModificarProcesoTito?dinero=" + $("#" + item + "_MontoDinero").val() + "&total=" + $("#" + item + "_Total").val() + "&letras=" + "'"+$("#" + item + "_Letras").val()+"'" + "&codProceso=" + item;

            $.ajax({
                type: "POST",
                cache: false,
                url: basePath + "CallCenter/ConsultaModificarProcesoTitoJson",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ url: url }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (response) {
                    response = response.data;
                    //console.log(response);
                    //toastr.success(response);
                    if (response == '"Registros Afectados: 1"') {
                        toastr.success("Se ha actualizado el registro " + item);
                        $("#" + itemModificando + "_Total").attr('data-inicial', $("#" + item + "_Total").val());
                        $("#" + itemModificando + "_Letras").attr('data-inicial', $("#" + item + "_Letras").val());
                        $("#" + itemModificando + "_MontoDinero").attr('data-inicial', $("#" + item + "_MontoDinero").val());
                    }
                    else {
                        toastr.error("Error al momento de actualizar, revise los campos.")
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
            var TotalIni = $("#" + itemModificando + "_Total").attr("data-inicial");
            var LetrasIni = $("#" + itemModificando + "_Letras").attr("data-inicial");
            var DineroIni = $("#" + itemModificando + "_MontoDinero").attr("data-inicial");
            $("#" + itemModificando + "_Total").val(TotalIni);
            $("#" + itemModificando + "_Letras").val(LetrasIni);
            $("#" + itemModificando + "_MontoDinero").val(DineroIni);
        },

    });

        var datainicial = {
            Item: itemModificando,
            Total: $("#" + itemModificando + "_Total").attr("data-inicial"),
            Letras: $("#" + itemModificando + "_Letras").attr("data-inicial"),
            Tabla: $("#cboTabla option:selected").text(),
            CodSala: $("#cboSala option:selected").data('id'),
            Sala: $("#cboSala option:selected").text(),
            FechaIni: $("#pro_ti_fechaInicio").val(),
            FechaFin: $("#pro_ti_fechaFin").val(),
            CdigoTicket: $("#pro_ti_cod_ticket").val(),
            NroMaquina: $("#pro_ti_nro_maquina").val(),
            ItemCaja: $("#pro_ti_item_caja").val(),
        };
        var datafinal = {
            Item: itemModificando,
            Total: $("#" + item + "_Total").val(),
            Letras: $("#" + item + "_Letras").val(),
            Tabla: $("#cboTabla option:selected").text(),
            CodSala: $("#cboSala option:selected").data('id'),
            Sala: $("#cboSala option:selected").text(),
            FechaIni: $("#pro_ti_fechaInicio").val(),
            FechaFin: $("#pro_ti_fechaFin").val(),
            CdigoTicket: $("#pro_ti_cod_ticket").val(),
            NroMaquina: $("#pro_ti_nro_maquina").val(),
            ItemCaja: $("#pro_ti_item_caja").val(),
        };
        dataAuditoriaJSON(3, "CallCenter/ConsultaModificarProcesoTitoJson", "MODIFICAR  PROCESOTITO", datainicial, datafinal);
}
function ModificarDetalleMovaux(item) {
    
    itemModificando = item;
    
    
    var js2 = $.confirm({
        icon: 'fa fa-spinner fa-spin',
        title: 'Esta Seguro que desea Actualizar el Ticket ' + item + '?',
        theme: 'black',
        animationBounce: 1.5,
        columnClass: 'col-md-6 col-md-offset-3',
        confirmButtonClass: 'btn-info',
        cancelButtonClass: 'btn-warning',
        confirmButton: "confirmar",
        cancelButton: 'Aún No',
        content: "",
        confirm: function () {
            var url = ipPublicaG + "/servicio/ModificarDetalle_movaux_por_maquina?monto=" + $("#" + item + "_pagomanual").val() + "&item=" + item;

            $.ajax({
                type: "POST",
                cache: false,
                url: basePath + "CallCenter/ConsultaModificarDetalle_movaux_por_maquinaJson",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ url: url }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (response) {
                    response = response.data;
                    //console.log(response);
                    //toastr.success(response);
                    if (response == '"Registros Afectados: 1"') {
                        toastr.success("Se ha actualizado el registro " + item);
                        $("#" + itemModificando + "_pagomanual").attr('data-inicial', $("#" + item + "_pagomanual").val());
                    }
                    else {
                        toastr.error("Error al momento de actualizar, revise los campos.")
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
            var estadoIni = $("#" + itemModificando + "_pagomanual").attr("data-inicial");
            $("#" + itemModificando + "_pagomanual").val(estadoIni);
        },

    });
    var datainicial = {
        Item: itemModificando,
        PagoManual: $("#" + itemModificando + "_pagomanual").attr("data-inicial"),
        Tabla: $("#cboTabla option:selected").text(),
        CodSala: $("#cboSala option:selected").data('id'),
        Sala: $("#cboSala option:selected").text(),
        FechaIni: $("#pro_ti_fechaInicio").val(),
        FechaFin: $("#pro_ti_fechaFin").val(),
        ItemFiltro: $("#det_mov_maq_item").val(),
        MaquinaAlterna: $("#det_mov_maq_alterno").val(),
      
    };
    var datafinal = {
        Item: itemModificando,
        PagoManual: $("#" + itemModificando + "_pagomanual").val(),
        Tabla: $("#cboTabla option:selected").text(),
        CodSala: $("#cboSala option:selected").data('id'),
        Sala: $("#cboSala option:selected").text(),
        FechaIni: $("#pro_ti_fechaInicio").val(),
        FechaFin: $("#pro_ti_fechaFin").val(),
        ItemFiltro: $("#det_mov_maq_item").val(),
        MaquinaAlterna: $("#det_mov_maq_alterno").val(),
    };
    dataAuditoriaJSON(3, "CallCenter/ConsultaModificarDetalle_movaux_por_maquinaJson", "MODIFICAR  DETALLEMOVAUX_POR_MAQUINA", datainicial, datafinal);
}
function ModificarDet0001TTO_00H(item) {

    itemModificando = item;

    var js2;
    js2 = $.confirm({
        icon: 'fa fa-spinner fa-spin',
        title: 'Esta Seguro que desea Actualizar el Ticket ' + item + '?',
        theme: 'black',
        animationBounce: 1.5,
        columnClass: 'col-md-6 col-md-offset-3',
        confirmButtonClass: 'btn-info',
        cancelButtonClass: 'btn-warning',
        confirmButton: "confirmar",
        cancelButton: 'Aún No',
        content: "",
        confirm: function () {
            var url = ipPublicaG + "/servicio/ModificarDet0001TTO_00H?monto=" + $("#" + item + "_MontoTicket").val() + "&estado=" + $("#" + item + "_Estado").val() + "&item=" + item;

            $.ajax({
                type: "POST",
                cache: false,
                url: basePath + "CallCenter/ConsultaModificarDet0001TTO_00HJson",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ url: url }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (response) {
                    response = response.data;
                    if (response == '"Registros Afectados: 1"') {
                        toastr.success("Se ha actualizado el registro " + item);
                        $("#" + itemModificando + "_MontoTicket").attr('data-inicial', $("#" + item + "_MontoTicket").val());
                        $("#" + itemModificando + "_Estado").attr('data-inicial', $("#" + item + "_Estado").val());
                    }
                    else {
                        toastr.error("Error al momento de actualizar, revise los campos.")
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
            var EstadoIni = $("#" + itemModificando + "_Estado").attr("data-inicial");
            var MontoIni = $("#" + itemModificando + "_MontoTicket").attr("data-inicial");
            $("#" + itemModificando + "_Estado").val(EstadoIni);
            $("#" + itemModificando + "_MontoTicket").val(MontoIni);
        },

    });
    var datainicial = {
        Item: itemModificando,
        MontoTicket: $("#" + itemModificando + "_MontoTicket").attr("data-inicial"),
        Tabla: $("#cboTabla option:selected").text(),
        CodSala: $("#cboSala option:selected").data('id'),
        Sala: $("#cboSala option:selected").text(),
        FechaIni: $("#pro_ti_fechaInicio").val(),
        PuntoVenta: $("#det_tt0_Punto_venta").val(),
        PuntoVentaFin: $("#det_tt0_Punto_venta_fin").val(),
        NroTicket: $("#det_tt0_Tito_NroTicket").val(),
    };
    var datafinal = {
        Item: itemModificando,
        MontoTicket: $("#" + itemModificando + "_MontoTicket").val(),
        Tabla: $("#cboTabla option:selected").text(),
        CodSala: $("#cboSala option:selected").data('id'),
        Sala: $("#cboSala option:selected").text(),
        FechaIni: $("#pro_ti_fechaInicio").val(),
        PuntoVenta: $("#det_tt0_Punto_venta").val(),
        PuntoVentaFin: $("#det_tt0_Punto_venta_fin").val(),
        NroTicket: $("#det_tt0_Tito_NroTicket").val(),
    };
    dataAuditoriaJSON(3, "CallCenter/ConsultaModificarDet0001TTO_00HJson", "MODIFICAR  DET0001TTO_00H", datainicial, datafinal);
}
function ModificarDet0001TTO(item) {

    itemModificando = item;

    var js2;
    js2 = $.confirm({
        icon: 'fa fa-spinner fa-spin',
        title: 'Esta Seguro que desea Actualizar el Ticket ' + item + '?',
        theme: 'black',
        animationBounce: 1.5,
        columnClass: 'col-md-6 col-md-offset-3',
        confirmButtonClass: 'btn-info',
        cancelButtonClass: 'btn-warning',
        confirmButton: "confirmar",
        cancelButton: 'Aún No',
        content: "",
        confirm: function () { 
            var url = ipPublicaG + "/servicio/ModificarDet0001TTO?item="+ item +"&estado="+ $("#" + item + "_Estado").val();

            $.ajax({
                type: "POST",
                cache: false,
                url: basePath + "CallCenter/ConsultaModificarDet0001TTOJson",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ url: url }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (response) {
                    response = response.data;
                    //console.log(response);
                    //toastr.success(response);
                    if (response == '"Registros Afectados: 1"') {
                        toastr.success("Se ha actualizado el registro " + item);
                        $("#" + itemModificando + "_Estado").attr('data-inicial', $("#" + item + "_Estado").val());
                    }
                    else {
                        toastr.error("Error al momento de actualizar, revise los campos.")
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
            var EstadoIni = $("#" + itemModificando + "_Estado").attr("data-inicial");
            $("#" + itemModificando + "_Estado").val(EstadoIni);
        },

    });

    var estados = [
        { "value": "0", "nombre": "Anulado" },
        { "value": "1", "nombre": "No Cobrado" },
        { "value": "2", "nombre": "Cobrado" },
        { "value": "3", "nombre": "Vencido" }
    ];

    var datainicial = {
        Item: itemModificando,   
        Estado: estados[$("#" + itemModificando + "_Estado").attr("data-inicial")].nombre ,
        Tabla: $("#cboTabla option:selected").text(),
        CodSala: $("#cboSala option:selected").data('id'),
        Sala: $("#cboSala option:selected").text(),
        FechaIni: $("#pro_ti_fechaInicio").val(),
        FechaFin: $("#det_tito_fechaFin").val(),
        Item: $("#det_tito_item").val(),
        NroTicket: $("#det_tito_Tito_NroTicket").val(),
        PuntoVenta: $("#det_tito_Tito_PuntoVenta").val(),
    };
    var datafinal = {
        Item: itemModificando,
        Estado: estados[$("#" + itemModificando + "_Estado").val()].nombre ,
        Tabla: $("#cboTabla option:selected").text(),
        CodSala: $("#cboSala option:selected").data('id'),
        Sala: $("#cboSala option:selected").text(),
        FechaIni: $("#pro_ti_fechaInicio").val(),
        FechaFin: $("#det_tito_fechaFin").val(),
        Item: $("#det_tito_item").val(),
        NroTicket: $("#det_tito_Tito_NroTicket").val(),
        PuntoVenta: $("#det_tito_Tito_PuntoVenta").val(),
    };
    dataAuditoriaJSON(3, "CallCenter/ConsultaModificarDet0001TTOJson", "MODIFICAR  DET0001TTO", datainicial, datafinal);
}

function EliminarProcesoTito(item, objetodatatable) {
    var url = ipPublicaG + "/servicio/EliminarProcesoTito?codProceso=" + item;
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CallCenter/ConsultaEliminarProcesoTitoJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            response = response.data;
            if (response == '"Registros Afectados: 1"') {
                toastr.success("Se ha Eliminado el registro " + item);
                var datafinal = {
                    CodProcesos: item,
                };
                dataAuditoriaJSON(3, "CallCenter/ConsultaEliminarProcesoTitoJson", "ELIMINAR PROCESOTITO", "", datafinal);
            }
            else {
                toastr.error("Error al momento de actualizar, revise los campos.")
            }
            objetodatatable.row('.selected').remove().draw(false);
        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
function EliminarDet0001TTO_00H(item, objetodatatable) {
    var url = ipPublicaG + "/servicio/EliminarDet0001TTO_00H?item=" + item;
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CallCenter/ConsultaEliminarDet0001TTO_00HJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            response = response.data;

            //toastr.success(response);
            if (response == '"Registros Afectados: 1"') {
                toastr.success("Se ha Eliminado el registro " + item);
                var datafinal = {
                    CodProceso: item,
                };
                dataAuditoriaJSON(3, "CallCenter/ConsultaEliminarDet0001TTO_00HJson", "ELIMINAR DET0001TTO_00H", "", datafinal);
            }
            else {
                toastr.error("Error al momento de actualizar, revise los campos.")
            }
            objetodatatable.row('.selected').remove().draw(false);
        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
function EliminarDet0001TTO(item, objetodatatable) {
    var url = ipPublicaG + "/servicio/EliminarDet0001TTO?item=" + item;
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CallCenter/ConsultaEliminarDet0001TTOJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            response = response.data;
            if (response == '"Registros Afectados: 1"') {
                var datafinal = {
                    CodProceso: item,
                };
                dataAuditoriaJSON(3, "CallCenter/ConsultaEliminarDet0001TTOJson", "ELIMINAR DET0001TTO", "", datafinal);
                toastr.success("Se ha Eliminado el registro " + item);
            }
            else {
                toastr.error("Error al momento de actualizar, revise los campos.")
            }
            objetodatatable.row('.selected').remove().draw(false);
        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
VistaAuditoria("CallCenter/CallCenterModificarTickets", "VISTA", 0, "", 3);
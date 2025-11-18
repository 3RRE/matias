function ListarConsulta() {
    var txtModulo = $("#cbomodulo").val();
    var txtHora1 = $("#txtFecha").val() + ' ' + $("#txtHora1").val();
    var txtHora2 = $("#txtFecha1").val() + ' ' + $("#txtHora2").val();

    var url = basePath + "Seguridad/ListarAuditoria";
    var data = { horaIni: txtHora1, horaFin: txtHora2 };
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        deferRender: true,
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            objetodatatable.clear();
            objetodatatable.rows.add(response.data).draw();
            $('.btnmodalAuditoria').tooltip({
                title: "Ver"
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
           
        }
    });
    $('table').css('width', '100%');
    $('.dataTables_scrollHeadInner').css('width', '100%');
    $('.dataTables_scrollFootInner').css('width', '100%');
}

//"use strict";

$(document).ready(function () {

    //$('#btnExcel').on('click', function (e) {
    //    //porc = generarporcentajescolumnas('tablaReporte')
    //    e.preventDefault();
    //    accion = $(this).attr("id");
    //    url = "Seguridad/ReporteAuditoriaRangoFechasBtnaccion";
    //    idtabla = $('.right-column-content .row table tbody').closest("table").attr("id");
    //    if ($('#' + idtabla).dataTable().fnSettings().fnRecordsTotal() > 0) {

    //        var txtHora1 = $("#txtFecha").val() + ' ' + $("#txtHora1").val();
    //        var txtHora2 = $("#txtFecha1").val() + ' ' + $("#txtHora2").val();
    //        var txtModulo = $("#cbomodulo").val();


    //        $('#formpend #fecha').val(txtHora1);
    //        $('#formpend #fechahasta').val(txtHora2);
    //        $('#formpend #modulo').val(txtModulo);


    //        //$('#formpend input[name="porcentajes"]').remove()
    //        //$('#formpend').append($('<input type="text">').attr("name", "porcentajes").val(JSON.stringify(porcentajes)))
    //        $('#formpend #accion').val(accion);
    //        if (accion == "imprimir") {
    //            $('#formpend').attr('target', '_blank');
    //        }
    //        $('#formpend').attr("action", basePath + url);

    //        $('#formpend').submit();
    //        $('#formpend #accion').val("");
    //    } else {
    //        toastr.error("No hay datos", "Mensaje Servidor");
    //    }

    //});
    ///fin excel,pdf mprimir

    fechaMin = moment(new Date()).format('YYYY-MM-DD');
    var dateMin = new Date(fechaMin);
    dateMin.setDate(dateMin.getDate() - 0);
    //console.log(dateMin)
    $(".dateOnly1").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        showToday: true,
        defaultDate: moment(new Date())
    });

    $(".dateOnly2").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        showToday: true,
        useCurrent: false,
        //minDate: dateMin,
        defaultDate: moment(new Date())
    });



    $(".dateOnly4").datetimepicker({
        pickDate: false,
        autoclose: true,
        showSeconds: true,
        pickSeconds: true,
        defaultDate: moment(dateNow).subtract(2,"m"),
        ampm: true, // FOR AM/PM FORMAT
        format: 'hh:mm:ss A'
    });

    $(".dateOnly5").datetimepicker({
        pickDate: false,
        autoclose: true,
        defaultDate: moment(dateNow),
        ampm: true, // FOR AM/PM FORMAT
        format: 'hh:mm:ss A'
    });


    ////iniciar datatable
    objetodatatable = $("#tablaReporte").on('order.dt', function () {
        $('table').css('width', '100%');
        $('.dataTables_scrollHeadInner').css('width', '100%');
        $('.dataTables_scrollFootInner').css('width', '100%');
    })
        .on('search.dt', function () {
            $('table').css('width', '100%');
            $('.dataTables_scrollHeadInner').css('width', '100%');
            $('.dataTables_scrollFootInner').css('width', '100%');
        })
        .on('page.dt', function () {
            $('table').css('width', '100%');
            $('.dataTables_scrollHeadInner').css('width', '100%');
            $('.dataTables_scrollFootInner').css('width', '100%');
        }).DataTable({
            "bDestroy": true,
            "bSort": true,
            "paging": true,
            "scrollX": true,
            "sScrollX": "100%",
            "scrollCollapse": true,
            "bProcessing": true,
            "bDeferRender": true,
            "autoWidth": false,
            "lengthMenu": [[10, 50, 200, -1], [10, 50, 200, "All"]],
            "pageLength": 10,
            "aaSorting": [],
            "language": {
                "emptyTable": "Presione Buscar, para Encontrar los registros"
            },
            //data: response.data,
            columns: [
                { data: "fechaRegistro", title: "Fecha y Hora", className: "tdcenter" },
                { data: "usuario", title: "Usuario", className: "tdcenter" },
                { data: "proceso", title: "Proceso", },
                //{ data: "descripcion", title: "Descripcion", className: "tdright" },
                //{ data: "subsistema", title: "Subsistema", className: "tdright" },
                { data: "sala", title: "Sala", },
                //{ data: "ip", title: "IP", className: "tdright" },
                {
                    data: "codAuditoria", title: "Data",
                    "bSortable": false,
                    "render": function (o) {
                        return '<button type="button" class="btn btn-xs btn-warning btnmodalAuditoria" data-id="' + o + '"><i class="glyphicon glyphicon-pencil"></i></button>';
                    }
                }
            ],


            "initComplete": function (settings, json) {

                $('#btnExcel').off("click").on('click', function () {
                    if (objetodatatable.rows({ filter: 'applied' }).count()) {
                        cabecerasnuevas = [];
                        cabecerasnuevas.push({ nombre: "Fecha Inicial", valor: $("#txtFecha").text() + " " + $("#txtHora1").val() });
                        cabecerasnuevas.push({ nombre: "Fecha Inicial", valor: $("#txtFecha1").text() + " " + $("#txtHora2").val() });

                        definicioncolumnas = [];
                        //definicioncolumnas.push({ nombre: "Monto", tipo: "decimal", alinear: "right", sumar: "true" });

                        var ocultar = ["data"];//"Accion";
                        funcionbotonesnuevo({
                            botonobjeto: this, ocultar: ocultar,
                            tablaobj: objetodatatable,
                            cabecerasnuevas: cabecerasnuevas,
                            definicioncolumnas: definicioncolumnas
                        });
                    }
                    else
                    {
                        toastr.error("No hay datos")
                    }
            })

            },

        });
    
    //////////////FIN INICIO DATATABLE

    $("body").on("click", "#btnBuscar", function () {
        ListarConsulta();
    });


    $(document).on('click', '.btnmodalAuditoria', function (e) {
        var id = $(this).data("id");
        var url = basePath + "Seguridad/DataAuditoria/Registro" + id;
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({}),
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                respuesta = response.dataResultado;
                var dataUsuarioAuditoria = JSON.parse(respuesta.usuariodata);
               
                if (dataUsuarioAuditoria) {
                    $("#txtCodEmpleado").val(dataUsuarioAuditoria.EmpleadoID);
                    $("#txtEmpleado").val(dataUsuarioAuditoria.NombreEmpleado);
                    $("#txtCodUsuario").val(dataUsuarioAuditoria.UsuarioID);
                    $("#txtUsuario").val(dataUsuarioAuditoria.UsuarioNombre);
                    $("#txtPassword").val(dataUsuarioAuditoria.UsuarioContraseña);
                    $("#txtCodTipoUsuario").val(dataUsuarioAuditoria.TipoUsuarioID);
                    //$("#txtTipoUsuario").val(dataUsuarioAuditoria.EmpleadoID);
                    $("#txtFechaRegistro").val(dataUsuarioAuditoria.FechaRegistro);
                    $("#txtIntentos").val(dataUsuarioAuditoria.FailedAttempts);
                    var estado = "";
                    if (dataUsuarioAuditoria.Estado == 1) {
                        estado = "Activo";
                    }
                    else {
                        estado = "Inactivo";
                    }
                    $("#txtEstado").val(estado);
                    
                }
                var dataInicialAuditoria_ = respuesta.datainicial;
                var dataFinalAuditoria_ = respuesta.datafinal;
                if (dataInicialAuditoria_ != "" && dataFinalAuditoria_ != "") {
                    $("#tabdata").show();
                }
                else {
                    $("#tabdata").hide();
                }

                if (respuesta.formularioID == 2) {
                    $.ajax({
                        url: basePath + "Content/assets/auditoriatxt/reporteprogresivo.txt",
                        dataType: "html",
                        success: function (data) {
                            $("#default-tabs-justified-profile").html(data);
                        },
                        complete: function () {
                            if (dataFinalAuditoria_) {
                                var dataFinalAuditoria = JSON.parse(respuesta.datafinal);
                                $("#txtSalaF").val(dataFinalAuditoria.cboSala_text);
                                $("#txtProgresivoF").val(dataFinalAuditoria.cboProgresivo_text);
                                $("#txtMaquinaF").val(dataFinalAuditoria.txtMaquina);
                                $("#txtPozosF").val(dataFinalAuditoria.cboPozos_text);
                                $("#txtRegistroAF").val(dataFinalAuditoria.RegA);
                                $("#txtRegistroDF").val(dataFinalAuditoria.RegD);
                              
                            }
                        }
                    });

                }
                if (respuesta.formularioID == 1) {
                    $.ajax({
                        url: basePath + "Content/assets/auditoriatxt/registroprogresivo.txt",
                        dataType: "html",
                        success: function (data) {
                            $("#default-tabs-justified-profile").html(data);
                        },
                        complete: function () {
                            if (dataInicialAuditoria_) {
                                var dataInicialAuditoria = JSON.parse(respuesta.datainicial);
                                console.log(dataInicialAuditoria)
                                $("#txtSalaA").val(dataInicialAuditoria.cboSala_text);
                                $("#txtProgresivoA").val(dataInicialAuditoria.cboProgresivo_text);
                                $("#txtMonedaA").val(dataInicialAuditoria.txtMoneda);
                                $("#txtLugarPA").val(dataInicialAuditoria.cboLugarPago_text);
                                $("#txtDuracionA").val(dataInicialAuditoria.txtDuracion);
                                $("#txtNroJugadoresA").val(dataInicialAuditoria.txtNumJugadores);
                                $("#txtNroPozosA").val(dataInicialAuditoria.txtNroPozos);
                                $("#txtImagenA").val(dataInicialAuditoria.cboImagen_text);
                                $("#txtEstadoA").val(dataInicialAuditoria.cboEstado_text);
                                var sipozo = "";
                                if (dataInicialAuditoria.chkPozoOculto == 1) {
                                    sipozo = "SI";
                                }
                                else {
                                    sipozo = "NO";
                                }
                                $("#txtpozoA").val(sipozo);
                                var siregistro = "";
                                if (dataInicialAuditoria.chkRegHistorico == 1) {
                                    siregistro = "SI";
                                }
                                else {
                                    siregistro = "NO";
                                }
                                $("#txtRegistrarA").val(siregistro);
                                //pozos
                                if (dataInicialAuditoria.chkPozoSuperior == 1) {
                                    $("#chkPozoSuperiorA").removeClass();
                                    $("#chkPozoSuperiorA").addClass("fa fa-check-square");

                                    $("#txtPSPremioBaseA").val(dataInicialAuditoria.txtPSPremioBase);
                                    $("#txtPSPremioMinimoA").val(dataInicialAuditoria.txtPSPremioMinimo);
                                    $("#txtPSPremioMaximoA").val(dataInicialAuditoria.txtPSPremioMaximo);
                                    $("#txtPSIncPozoNormalA").val(dataInicialAuditoria.txtPSIncPozoNormal);
                                    $("#txtPSRsApuestaA").val(dataInicialAuditoria.txtPSRsApuesta);
                                    $("#txtPSRsJugadoresA").val(dataInicialAuditoria.txtPSRsJugadores);
                                    $("#txtPSMontoOcultoMinA").val(dataInicialAuditoria.txtPSMontoOcultoMin);
                                    $("#txtPSMontoOcultoMaxA").val(dataInicialAuditoria.txtPSMontoOcultoMax);
                                    $("#txtPSIncPozoOcultoA").val(dataInicialAuditoria.txtPSIncPozoOculto);
                                }
                                else {
                                    $("#chkPozoSuperiorA").removeClass();
                                    $("#chkPozoSuperiorA").addClass("fa fa-square");

                                    $("#txtPSPremioBaseA").val(0);
                                    $("#txtPSPremioMinimoA").val(0);
                                    $("#txtPSPremioMaximoA").val(0);
                                    $("#txtPSIncPozoNormalA").val(0);
                                    $("#txtPSRsApuestaA").val(0);
                                    $("#txtPSRsJugadoresA").val(0);
                                    $("#txtPSMontoOcultoMinA").val(0);
                                    $("#txtPSMontoOcultoMaxA").val(0);
                                    $("#txtPSIncPozoOcultoA").val(0);
                                }


                                $("#cboPSDificultadA").val(dataInicialAuditoria.cboPSDificultad_text);
                                $("#txtPSPozoActualA").val(dataInicialAuditoria.txtPSPozoActual);
                                ///////////////////////////////
                                if (dataInicialAuditoria.chkPozoMedio == 1) {
                                    $("#chkPozoMedioA").removeClass();
                                    $("#chkPozoMedioA").addClass("fa fa-check-square");

                                    $("#txtPMPremioBaseA").val(dataInicialAuditoria.txtPMPremioBase);
                                    $("#txtPMPremioMinimoA").val(dataInicialAuditoria.txtPMPremioMinimo);
                                    $("#txtPMPremioMaximoA").val(dataInicialAuditoria.txtPMPremioMaximo);
                                    $("#txtPMIncPozoNormalA").val(dataInicialAuditoria.txtPMIncPozoNormal);
                                    $("#txtPMRsApuestaA").val(dataInicialAuditoria.txtPMRsApuesta);
                                    $("#txtPMRsJugadoresA").val(dataInicialAuditoria.txtPMRsJugadores);
                                    $("#txtPMMontoOcultoMinA").val(dataInicialAuditoria.txtPMMontoOcultoMin);
                                    $("#txtPMMontoOcultoMaxA").val(dataInicialAuditoria.txtPMMontoOcultoMax);
                                    $("#txtPMIncPozoOcultoA").val(dataInicialAuditoria.txtPMIncPozoOculto);
                                }
                                else {
                                    $("#chkPozoMedioA").removeClass();
                                    $("#chkPozoMedioA").addClass("fa fa-square");

                                    $("#txtPMPremioBaseA").val(0);
                                    $("#txtPMPremioMinimoA").val(0);
                                    $("#txtPMPremioMaximoA").val(0);
                                    $("#txtPMIncPozoNormalA").val(0);
                                    $("#txtPMRsApuestaA").val(0);
                                    $("#txtPMRsJugadoresA").val(0);
                                    $("#txtPMMontoOcultoMinA").val(0);
                                    $("#txtPMMontoOcultoMaxA").val(0);
                                    $("#txtPMIncPozoOcultoA").val(0);
                                }

                                $("#cboPMDificultadA").val(dataInicialAuditoria.cboPMDificultad_text);
                                $("#txtPMPozoActualA").val(dataInicialAuditoria.txtPMPozoActual);
                                ///////////////////////////////////
                                if (dataInicialAuditoria.chkPozoInferior == 1) {
                                    $("#chkPozoInferiorA").removeClass();
                                    $("#chkPozoInferiorA").addClass("fa fa-check-square");

                                    $("#txtPIPremioBaseA").val(dataInicialAuditoria.txtPIPremioBase);
                                    $("#txtPIPremioMinimoA").val(dataInicialAuditoria.txtPIPremioMinimo);
                                    $("#txtPIPremioMaximoA").val(dataInicialAuditoria.txtPIPremioMaximo);
                                    $("#txtPIIncPozoNormalA").val(dataInicialAuditoria.txtPIIncPozoNormal);
                                    $("#txtPIRsApuestaA").val(dataInicialAuditoria.txtPIRsApuesta);
                                    $("#txtPIRsJugadoresA").val(dataInicialAuditoria.txtPIRsJugadores);
                                    $("#txtPIMontoOcultoMinA").val(dataInicialAuditoria.txtPIMontoOcultoMin);
                                    $("#txtPIMontoOcultoMaxA").val(dataInicialAuditoria.txtPIMontoOcultoMax);
                                    $("#txtPIIncPozoOcultoA").val(dataInicialAuditoria.txtPIIncPozoOculto);
                                }
                                else {
                                    $("#chkPozoInferiorA").removeClass();
                                    $("#chkPozoInferiorA").addClass("fa fa-square");

                                    $("#txtPIPremioBaseA").val(0);
                                    $("#txtPIPremioMinimoA").val(0);
                                    $("#txtPIPremioMaximoA").val(0);
                                    $("#txtPIIncPozoNormalA").val(0);
                                    $("#txtPIRsApuestaA").val(0);
                                    $("#txtPIRsJugadoresA").val(0);
                                    $("#txtPIMontoOcultoMinA").val(0);
                                    $("#txtPIMontoOcultoMaxA").val(0);
                                    $("#txtPIIncPozoOcultoA").val(0);
                                }

                                $("#cboPIDificultadA").val(dataInicialAuditoria.cboPIDificultad_text);
                                $("#txtPIPozoActualA").val(dataInicialAuditoria.txtPIPozoActual);
                            }


                            if (dataFinalAuditoria_) {

                                var dataFinalAuditoria = JSON.parse(respuesta.datafinal);
                                console.log(dataFinalAuditoria)
                                $("#txtSalaF").val(dataFinalAuditoria.cboSala_text);
                                if (dataInicialAuditoria.cboSala_text != dataFinalAuditoria.cboSala_text) {
                                    $("#txtSalaF").addClass("rojodiferente");
                                }
                                $("#txtProgresivoF").val(dataFinalAuditoria.cboProgresivo_text);
                                if (dataInicialAuditoria.cboProgresivo_text != dataFinalAuditoria.cboProgresivo_text) {
                                    $("#txtProgresivoF").addClass("rojodiferente");
                                }
                                $("#txtMonedaF").val(dataFinalAuditoria.txtMoneda);
                                if (dataInicialAuditoria.txtMoneda != dataFinalAuditoria.txtMoneda) {
                                    $("#txtMonedaF").addClass("rojodiferente");
                                }
                                $("#txtLugarPF").val(dataFinalAuditoria.cboLugarPago_text);
                                if (dataInicialAuditoria.cboLugarPago_text != dataFinalAuditoria.cboLugarPago_text) {
                                    $("#txtLugarPF").addClass("rojodiferente");
                                }
                                $("#txtDuracionF").val(dataFinalAuditoria.txtDuracion);
                                if (dataInicialAuditoria.txtDuracion != dataFinalAuditoria.txtDuracion) {
                                    $("#txtDuracionF").addClass("rojodiferente");
                                }
                                $("#txtNroJugadoresF").val(dataFinalAuditoria.txtNumJugadores);
                                if (dataInicialAuditoria.txtNumJugadores != dataFinalAuditoria.txtNumJugadores) {
                                    $("#txtNroJugadoresF").addClass("rojodiferente");
                                }
                                $("#txtNroPozosF").val(dataFinalAuditoria.txtNroPozos);
                                if (dataInicialAuditoria.txtNroPozos != dataFinalAuditoria.txtNroPozos) {
                                    $("#txtNroPozosF").addClass("rojodiferente");
                                }
                                $("#txtImagenF").val(dataFinalAuditoria.cboImagen_text);
                                if (dataInicialAuditoria.cboImagen_text != dataFinalAuditoria.cboImagen_text) {
                                    $("#txtImagenF").addClass("rojodiferente");
                                }
                                $("#txtEstadoF").val(dataFinalAuditoria.cboEstado_text);
                                if (dataInicialAuditoria.cboEstado_text != dataFinalAuditoria.cboEstado_text) {
                                    $("#txtEstadoF").addClass("rojodiferente");
                                }
                                var sipozo = "";
                                if (dataFinalAuditoria.chkPozoOculto == 1) {
                                    sipozo = "SI";
                                }
                                else {
                                    sipozo = "NO";
                                }
                                $("#txtpozoF").val(sipozo);
                                if (dataInicialAuditoria.chkPozoOculto != dataFinalAuditoria.chkPozoOculto) {
                                    $("#txtpozoF").addClass("rojodiferente");
                                }
                                var siregistro = "";
                                if (dataFinalAuditoria.chkRegHistorico == 1) {
                                    siregistro = "SI";
                                }
                                else {
                                    siregistro = "NO";
                                }
                                $("#txtRegistrarF").val(siregistro);
                                if (dataInicialAuditoria.chkRegHistorico != dataFinalAuditoria.chkRegHistorico) {
                                    $("#txtRegistrarF").addClass("rojodiferente");
                                }
                                //POZO FINAL
                                if (dataFinalAuditoria.chkPozoSuperior == 1) {
                                    $("#chkPozoSuperiorF").removeClass();
                                    $("#chkPozoSuperiorF").addClass("fa fa-check-square");

                                    $("#txtPSPremioBaseF").val(dataFinalAuditoria.txtPSPremioBase);
                                    if (dataInicialAuditoria.txtPSPremioBase != dataFinalAuditoria.txtPSPremioBase) {
                                        $("#txtPSPremioBaseF").addClass("rojodiferente");
                                    }
                                    $("#txtPSPremioMinimoF").val(dataFinalAuditoria.txtPSPremioMinimo);
                                    if (dataInicialAuditoria.txtPSPremioMinimo != dataFinalAuditoria.txtPSPremioMinimo) {
                                        $("#txtPSPremioMinimoF").addClass("rojodiferente");
                                    }
                                    $("#txtPSPremioMaximoF").val(dataFinalAuditoria.txtPSPremioMaximo);
                                    if (dataInicialAuditoria.txtPSPremioMaximo != dataFinalAuditoria.txtPSPremioMaximo) {
                                        $("#txtPSPremioMaximoF").addClass("rojodiferente");
                                    }
                                    $("#txtPSIncPozoNormalF").val(dataFinalAuditoria.txtPSIncPozoNormal);
                                    if (dataInicialAuditoria.txtPSIncPozoNormal != dataFinalAuditoria.txtPSIncPozoNormal) {
                                        $("#txtPSIncPozoNormalF").addClass("rojodiferente");
                                    }
                                    $("#txtPSRsApuestaF").val(dataFinalAuditoria.txtPSRsApuesta);
                                    if (dataInicialAuditoria.txtPSRsApuesta != dataFinalAuditoria.txtPSRsApuesta) {
                                        $("#txtPSRsApuestaF").addClass("rojodiferente");
                                    }
                                    $("#txtPSRsJugadoresF").val(dataFinalAuditoria.txtPSRsJugadores);
                                    if (dataInicialAuditoria.txtPSRsJugadores != dataFinalAuditoria.txtPSRsJugadores) {
                                        $("#txtPSRsJugadoresF").addClass("rojodiferente");
                                    }
                                    $("#txtPSMontoOcultoMinF").val(dataFinalAuditoria.txtPSMontoOcultoMin);
                                    if (dataInicialAuditoria.txtPSMontoOcultoMin != dataFinalAuditoria.txtPSMontoOcultoMin) {
                                        $("#txtPSMontoOcultoMinF").addClass("rojodiferente");
                                    }
                                    $("#txtPSMontoOcultoMaxF").val(dataFinalAuditoria.txtPSMontoOcultoMax);
                                    if (dataInicialAuditoria.txtPSMontoOcultoMax != dataFinalAuditoria.txtPSMontoOcultoMax) {
                                        $("#txtPSMontoOcultoMaxF").addClass("rojodiferente");
                                    }
                                    $("#txtPSIncPozoOcultoF").val(dataFinalAuditoria.txtPSIncPozoOculto);
                                    if (dataInicialAuditoria.txtPSIncPozoOculto != dataFinalAuditoria.txtPSIncPozoOculto) {
                                        $("#txtPSIncPozoOcultoF").addClass("rojodiferente");
                                    }
                                }
                                else {
                                    $("#chkPozoSuperiorF").removeClass();
                                    $("#chkPozoSuperiorF").addClass("fa fa-square");

                                    $("#txtPSPremioBaseF").val(0);
                                    $("#txtPSPremioBaseF").addClass("rojodiferente");
                                    $("#txtPSPremioMinimoF").val(0);
                                    $("#txtPSPremioMinimoF").addClass("rojodiferente");
                                    $("#txtPSPremioMaximoF").val(0);
                                    $("#txtPSPremioMaximoF").addClass("rojodiferente");
                                    $("#txtPSIncPozoNormalF").val(0);
                                    $("#txtPSIncPozoNormalF").addClass("rojodiferente");
                                    $("#txtPSRsApuestaF").val(0);
                                    $("#txtPSRsApuestaF").addClass("rojodiferente");
                                    $("#txtPSRsJugadoresF").val(0);
                                    $("#txtPSRsJugadoresF").addClass("rojodiferente");
                                    $("#txtPSMontoOcultoMinF").val(0);
                                    $("#txtPSMontoOcultoMinF").addClass("rojodiferente");
                                    $("#txtPSMontoOcultoMaxF").val(0);
                                    $("#txtPSMontoOcultoMaxF").addClass("rojodiferente");
                                    $("#txtPSIncPozoOcultoF").val(0);
                                    $("#txtPSIncPozoOcultoF").addClass("rojodiferente");
                                }


                                $("#cboPSDificultadF").val(dataFinalAuditoria.cboPSDificultad_text);
                                if (dataInicialAuditoria.cboPSDificultad_text != dataFinalAuditoria.cboPSDificultad_text) {
                                    $("#cboPSDificultadF").addClass("rojodiferente");
                                }
                                $("#txtPSPozoActualF").val(dataFinalAuditoria.txtPSPozoActual);
                                if (dataInicialAuditoria.txtPSPozoActual != dataFinalAuditoria.txtPSPozoActual) {
                                    $("#txtPSPozoActualF").addClass("rojodiferente");
                                }
                                ///////////////////////////////
                                if (dataFinalAuditoria.chkPozoMedio == 1) {
                                    $("#chkPozoMedioF").removeClass();
                                    $("#chkPozoMedioF").addClass("fa fa-check-square");

                                    $("#txtPMPremioBaseF").val(dataFinalAuditoria.txtPMPremioBase);
                                    if (dataInicialAuditoria.txtPMPremioBase != dataFinalAuditoria.txtPMPremioBase) {
                                        $("#txtPMPremioBaseF").addClass("rojodiferente");
                                    }
                                    $("#txtPMPremioMinimoF").val(dataFinalAuditoria.txtPMPremioMinimo);
                                    if (dataInicialAuditoria.txtPMPremioMinimo != dataFinalAuditoria.txtPMPremioMinimo) {
                                        $("#txtPMPremioMinimoF").addClass("rojodiferente");
                                    }
                                    $("#txtPMPremioMaximoF").val(dataFinalAuditoria.txtPMPremioMaximo);
                                    if (dataInicialAuditoria.txtPMPremioMaximo != dataFinalAuditoria.txtPMPremioMaximo) {
                                        $("#txtPMPremioMaximoF").addClass("rojodiferente");
                                    }
                                    $("#txtPMIncPozoNormalF").val(dataFinalAuditoria.txtPMIncPozoNormal);
                                    if (dataInicialAuditoria.txtPMIncPozoNormal != dataFinalAuditoria.txtPMIncPozoNormal) {
                                        $("#txtPMIncPozoNormalF").addClass("rojodiferente");
                                    }
                                    $("#txtPMRsApuestaF").val(dataFinalAuditoria.txtPMRsApuesta);
                                    if (dataInicialAuditoria.txtPMRsApuesta != dataFinalAuditoria.txtPMRsApuesta) {
                                        $("#txtPMRsApuestaF").addClass("rojodiferente");
                                    }
                                    $("#txtPMRsJugadoresF").val(dataFinalAuditoria.txtPMRsJugadores);
                                    if (dataInicialAuditoria.txtPMRsJugadores != dataFinalAuditoria.txtPMRsJugadores) {
                                        $("#txtPMRsJugadoresF").addClass("rojodiferente");
                                    }
                                    $("#txtPMMontoOcultoMinF").val(dataFinalAuditoria.txtPMMontoOcultoMin);
                                    if (dataInicialAuditoria.txtPMMontoOcultoMin != dataFinalAuditoria.txtPMMontoOcultoMin) {
                                        $("#txtPMMontoOcultoMinF").addClass("rojodiferente");
                                    }
                                    $("#txtPMMontoOcultoMaxF").val(dataFinalAuditoria.txtPMMontoOcultoMax);
                                    if (dataInicialAuditoria.txtPMMontoOcultoMax != dataFinalAuditoria.txtPMMontoOcultoMax) {
                                        $("#txtPMMontoOcultoMaxF").addClass("rojodiferente");
                                    }
                                    $("#txtPMIncPozoOcultoF").val(dataFinalAuditoria.txtPMIncPozoOculto);
                                    if (dataInicialAuditoria.txtPMIncPozoOculto != dataFinalAuditoria.txtPMIncPozoOculto) {
                                        $("#txtPMIncPozoOcultoF").addClass("rojodiferente");
                                    }
                                }
                                else {
                                    $("#chkPozoMedioF").removeClass();
                                    $("#chkPozoMedioF").addClass("fa fa-square");

                                    $("#txtPMPremioBaseF").val(0);
                                    $("#txtPMPremioBaseF").addClass("rojodiferente");
                                    $("#txtPMPremioMinimoF").val(0);
                                    $("#txtPMPremioMinimoF").addClass("rojodiferente");
                                    $("#txtPMPremioMaximoF").val(0);
                                    $("#txtPMPremioMaximoF").addClass("rojodiferente");
                                    $("#txtPMIncPozoNormalF").val(0);
                                    $("#txtPMIncPozoNormalF").addClass("rojodiferente");
                                    $("#txtPMRsApuestaF").val(0);
                                    $("#txtPMRsApuestaF").addClass("rojodiferente");
                                    $("#txtPMRsJugadoresF").val(0);
                                    $("#txtPMRsJugadoresF").addClass("rojodiferente");
                                    $("#txtPMMontoOcultoMinF").val(0);
                                    $("#txtPMMontoOcultoMinF").addClass("rojodiferente");
                                    $("#txtPMMontoOcultoMaxF").val(0);
                                    $("#txtPMMontoOcultoMaxF").addClass("rojodiferente");
                                    $("#txtPMIncPozoOcultoF").val(0);
                                    $("#txtPMIncPozoOcultoF").addClass("rojodiferente");
                                }

                                $("#cboPMDificultadF").val(dataFinalAuditoria.cboPMDificultad_text);
                                if (dataInicialAuditoria.cboPMDificultad_text != dataFinalAuditoria.cboPMDificultad_text) {
                                    $("#cboPMDificultadF").addClass("rojodiferente");
                                }
                                $("#txtPMPozoActualF").val(dataFinalAuditoria.txtPMPozoActual);
                                if (dataInicialAuditoria.txtPMPozoActual != dataFinalAuditoria.txtPMPozoActual) {
                                    $("#txtPMPozoActualF").addClass("rojodiferente");
                                }
                                ///////////////////////////////////
                                if (dataFinalAuditoria.chkPozoInferior == 1) {
                                    $("#chkPozoInferiorF").removeClass();
                                    $("#chkPozoInferiorF").addClass("fa fa-check-square");

                                    $("#txtPIPremioBaseF").val(dataFinalAuditoria.txtPIPremioBase);
                                    if (dataInicialAuditoria.txtPIPremioBase != dataFinalAuditoria.txtPIPremioBase) {
                                        $("#txtPIPremioBaseF").addClass("rojodiferente");
                                    }
                                    $("#txtPIPremioMinimoF").val(dataFinalAuditoria.txtPIPremioMinimo);
                                    if (dataInicialAuditoria.txtPIPremioMinimo != dataFinalAuditoria.txtPIPremioMinimo) {
                                        $("#txtPIPremioMinimoF").addClass("rojodiferente");
                                    }
                                    $("#txtPIPremioMaximoF").val(dataFinalAuditoria.txtPIPremioMaximo);
                                    if (dataInicialAuditoria.txtPIPremioMaximo != dataFinalAuditoria.txtPIPremioMaximo) {
                                        $("#txtPIPremioMaximoF").addClass("rojodiferente");
                                    }
                                    $("#txtPIIncPozoNormalF").val(dataFinalAuditoria.txtPIIncPozoNormal);
                                    if (dataInicialAuditoria.txtPIIncPozoNormal != dataFinalAuditoria.txtPIIncPozoNormal) {
                                        $("#txtPIIncPozoNormalF").addClass("rojodiferente");
                                    }
                                    $("#txtPIRsApuestaF").val(dataFinalAuditoria.txtPIRsApuesta);
                                    if (dataInicialAuditoria.txtPIRsApuesta != dataFinalAuditoria.txtPIRsApuesta) {
                                        $("#txtPIRsApuestaF").addClass("rojodiferente");
                                    }
                                    $("#txtPIRsJugadoresF").val(dataFinalAuditoria.txtPIRsJugadores);
                                    if (dataInicialAuditoria.txtPIRsJugadores != dataFinalAuditoria.txtPIRsJugadores) {
                                        $("#txtPIRsJugadoresF").addClass("rojodiferente");
                                    }
                                    $("#txtPIMontoOcultoMinF").val(dataFinalAuditoria.txtPIMontoOcultoMin);
                                    if (dataInicialAuditoria.txtPIMontoOcultoMin != dataFinalAuditoria.txtPIMontoOcultoMin) {
                                        $("#txtPIMontoOcultoMinF").addClass("rojodiferente");
                                    }
                                    $("#txtPIMontoOcultoMaxF").val(dataFinalAuditoria.txtPIMontoOcultoMax);
                                    if (dataInicialAuditoria.txtPIMontoOcultoMax != dataFinalAuditoria.txtPIMontoOcultoMax) {
                                        $("#txtPIMontoOcultoMaxF").addClass("rojodiferente");
                                    }
                                    $("#txtPIIncPozoOcultoF").val(dataFinalAuditoria.txtPIIncPozoOculto);
                                    if (dataInicialAuditoria.txtPIIncPozoOculto != dataFinalAuditoria.txtPIIncPozoOculto) {
                                        $("#txtPIIncPozoOcultoF").addClass("rojodiferente");
                                    }
                                }
                                else {
                                    $("#chkPozoInferiorF").removeClass();
                                    $("#chkPozoInferiorF").addClass("fa fa-square");

                                    $("#txtPIPremioBaseF").val(0);
                                    $("#txtPIPremioBaseF").addClass("rojodiferente");
                                    $("#txtPIPremioMinimoF").val(0);
                                    $("#txtPIPremioMinimoF").addClass("rojodiferente");
                                    $("#txtPIPremioMaximoF").val(0);
                                    $("#txtPIPremioMaximoF").addClass("rojodiferente");
                                    $("#txtPIIncPozoNormalF").val(0);
                                    $("#txtPIIncPozoNormalF").addClass("rojodiferente");
                                    $("#txtPIRsApuestaF").val(0);
                                    $("#txtPIRsApuestaF").addClass("rojodiferente");
                                    $("#txtPIRsJugadoresF").val(0);
                                    $("#txtPIRsJugadoresF").addClass("rojodiferente");
                                    $("#txtPIMontoOcultoMinF").val(0);
                                    $("#txtPIMontoOcultoMinF").addClass("rojodiferente");
                                    $("#txtPIMontoOcultoMaxF").val(0);
                                    $("#txtPIMontoOcultoMaxF").addClass("rojodiferente");
                                    $("#txtPIIncPozoOcultoF").val(0);
                                    $("#txtPIIncPozoOcultoF").addClass("rojodiferente");
                                }

                                $("#cboPIDificultadF").val(dataFinalAuditoria.cboPIDificultad_text);
                                if (dataInicialAuditoria.cboPIDificultad_text != dataFinalAuditoria.cboPIDificultad_text) {
                                    $("#cboPIDificultadF").addClass("rojodiferente");
                                }
                                $("#txtPIPozoActualF").val(dataFinalAuditoria.txtPIPozoActual);
                                if (dataInicialAuditoria.txtPIPozoActual != dataFinalAuditoria.txtPIPozoActual) {
                                    $("#txtPIPozoActualF").addClass("rojodiferente");
                                }
                            }
                        }
                    });
                    
                    
                }

                if (respuesta.formularioID == 3) {

                    $.ajax({
                        url: basePath + "Content/assets/auditoriatxt/general.txt",
                        dataType: "html",
                        success: function (data) {
                            $("#default-tabs-justified-profile").html(data);
                        },
                        complete: function () {

                            if (dataInicialAuditoria_ != "" && dataFinalAuditoria_ == "") {
                                $("#divdatainicial").show();
                                $("#divdatafinal").hide();
                                $("#dataI").html("Informacion Encontrada");
                                $("#divdatainicial").removeClass().addClass("col-md-12 col-sm-12 col-xs-12");
                                $("#tabdata").show();
                            }
                            if (dataInicialAuditoria_ == "" && dataFinalAuditoria_ != "") {
                                $("#divdatainicial").hide();
                                $("#divdatafinal").show();
                                $("#dataF").html("Informacion Encontrada");
                                $("#divdatafinal").removeClass().addClass("col-md-12 col-sm-12 col-xs-12");
                                $("#tabdata").show();
                            }

                            if (dataInicialAuditoria_ != "" && dataFinalAuditoria_ != "") {
                                $("#divdatainicial").show();
                                $("#divdatafinal").show();
                                $("#dataI").html("Data Inicial Encontrada");
                                $("#dataF").html("Data Final Encontrada");
                                $("#divdatafinal").removeClass().addClass("col-md-6 col-sm-6 col-xs-12");
                                $("#tabdata").show();
                            }


                            if (dataInicialAuditoria_) {
                                var preI = "";
                                var dataInicialAuditoria = [];
                                dataInicialAuditoria.push(JSON.parse(respuesta.datainicial));

                                $.each(dataInicialAuditoria, function (i, valor) {
                                    $.each(valor, function (key, value) {
                                        preI += '<div class="col-md-6 col-sm-6 col-xs-12">' +
                                            '<div class="input-group">' +
                                            '<span class="input-group-addon redback">' +
                                            key +
                                            '</span>'+
                                            '<input type="text" class="form-control input-sm azul" name="' + key + '_Ini" id="' + key +'_Ini" disabled value="' + value + '">' +
                                            '</div>' +
                                            '</div>';
                                    });
                                });
                                $("#divrowIni").html(preI);
                            }

                            if (dataFinalAuditoria_) {
                                var preF = "";
                                var dataFinalAuditoria = [];
                                console.log(JSON.parse(respuesta.datafinal),"aaaa")
                                dataFinalAuditoria.push(JSON.parse(respuesta.datafinal));
                                $.each(dataFinalAuditoria, function (i, valor) {
                                    $.each(valor, function (key, value) {
                                        preF += '<div class="col-md-6 col-sm-6 col-xs-12">'+
                                                    '<div class="input-group">'+
                                                        '<span class="input-group-addon redback">'+
                                                            key+
                                                        '</span>'+
                                        '<input type="text" class="form-control input-sm azul" name="' + key + '_Fin" id="' + key +'_Fin" disabled value="' + value+'">' +
                                                     '</div>' +
                                                  '</div>';
                                    });
                                });
                                $("#divrowFin").html(preF);
                            }
                        }
                    });

                }
                //var DdataInicial = [];
                //var theadInicio = "";
                //var tbodyInicio = "";
                //var datospreprocesadoinicio = respuesta.datainicial;
                //console.log(JSON.stringify(datospreprocesadoinicio))
                //if (datospreprocesadoinicio) {
                //    var formattedDataI = datospreprocesadoinicio.replace(/\\/g, '');
                //    var sillyString = formattedDataI.substr(1);
                //    var incio = sillyString.slice(0, -1);
                //    var stringi = incio;

                //    $("#dataInicial").text(stringi);
                //    $("#divinicial").show();
                //}
                //else {
                //    $("#dataInicial").text("");
                //    $("#divinicial").hide();
                //}

                //var DdataFinal = [];
                //var theadFinal = "";
                //var tbodyFinal = "";
                //var datospreprocesadofinal = respuesta.datafinal;

                //if (datospreprocesadofinal != "" && datospreprocesadofinal != '"{}"') {
                //    var formattedDataF = datospreprocesadofinal.replace(/\\/g, '');
                //    var sillyStringF = formattedDataF.substr(1);
                //    var final = sillyStringF.slice(0, -1);

                //    var string = final;
                    
                //    $("#dataFinal").text(string);
                //    $("#divfinal").show();
                //}
                //else {
                //    //theadFinal = "<th>Sin Data</th>";
                //    $("#dataFinal").text("");
                //    $("#divfinal").hide();
                //}
                

                $("#fechaSpan").text(moment(respuesta.fechaRegistro).format("DD-MM-YYYY hh:mm:ss a"));
                $("#procesoSpan").text(respuesta.proceso);
                $("#tabdata1").click();
                $("#modalAuditoria").modal("show");
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
               
            }
        });
    });




});

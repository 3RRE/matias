let remoteService = 'http://192.168.1.32:9000';

const cmbSala = $("#cmbSala");
const btnBusquedaPorInstante = $('#btnBusquedaPorInstante');
const btnBusquedaPorHora = $('#btnBusquedaPorHora');

const tableConteo = $('#tableConteo');
const tHeadTableConteo = $('#tHeadTableConteo');
const tBodyTableConteo = $('#tBodyTableConteo');

const tableDetalle = $('#tableDetalle');

const tableDetalleXHora = $('#tableDetalleXHora');
const tHeadTableDetalleXHora = $('#tHeadTableDetalleXHora');
const tBodyTableDetalleXHora = $('#tBodyTableDetalleXHora');

let tipoExcell = 0;
let detalle = [];
let ArrNoImprimir = [];
let dataTable2;

$(document).ready(function () {
    getSalas();
    setDateTimePicker();
    setICheck();
    tableConteo.DataTable();
});

//#region Contantes
const uris = {
    ExportToExcel: basePath + 'Reporte9050/ExportToExcelReport'
};

const languageDataTable = {
    "sEmptyTable": "No hay registros",
    "sLengthMenu": "Mostrar _MENU_ registros",
    "sInfo": "Mostrando _START_ de _END_ de _TOTAL_ registros.",
    "sSearch": "Buscar",
    "oPaginate": {
        "sPrevious": "Anterior",
        "sNext": "Siguiente"
    }
};

const columnDefsDataTable = [{
    "targets": 'no-sort',
    "orderable": false
}];
//#endregion

//#region Eventos

cmbSala.on('select2:select', function () {
    setButtonsDisabled(false);
});
cmbSala.on('select2:clear select2:unselecting', function () {
    setButtonsDisabled(true);
});

$('input[type=radio][name=radioDiaSemana]').on('ifChecked', function (e) {
    switch ($(this).val()) {
        case 'dias':
            $("#DivSemana").fadeOut('slow');
            break;
        case 'semanas':
            $("#DivSemana").fadeIn('slow');
            break;
    }
});

$(document).on('click', '#btnBusquedaPorInstante', function (e) {
    $("#TituloModalParametros").text("");
    $("#TituloModalParametros").text("Parametros de Busqueda X Instante");
    $('#ModalParametrosBusqueda').modal('show');
    var tipo = $(this).data("tipo");
    tipoExcell = tipo;
    $("#tipoInstanteHora").val(tipo);
    $("#CurrentC").val(0.1);
    $("#Resultados").text("");
    $("#Resultados").text("Resultados X Instante");
});

$(document).on('click', '#btnBusquedaPorHora', function (e) {
    $("#TituloModalParametros").text("");
    $("#TituloModalParametros").text("Parametros de Busqueda X Hora");
    $('#ModalParametrosBusqueda').modal('show');
    var tipo = $(this).data("tipo");
    tipoExcell = tipo;
    $("#tipoInstanteHora").val(tipo);
    $("#CurrentC").val('');
    $("#Resultados").text("");
    $("#Resultados").text("Resultados X Hora");
});

$(document).on('click', '#btnConsultar', function (e) {
    var fechaini = $("#FechaInicio").val();
    var fechafin = $("#FechaFin").val();
    if (!fechaini) {
        toastr.error('Seleccione Fecha Inicio.', 'Servidor');
        return;
    }
    if (!fechafin) {
        toastr.error('Seleccione Fecha Fin.', 'Servidor');
        return;
    }
    var codSala = cmbSala.val();
    var CurrentC = $("#CurrentC").val();
    if (!codSala) {
        toastr.error('Seleccione Sala.', 'Servidor');
        return;
    }
    var tipoIH = $("#tipoInstanteHora").val();
    if (tipoIH == 1) {
        if (!CurrentC || CurrentC <= 0) {
            toastr.error('Ingrese valor de Current Credit mayor a 0.', 'Servidor');
            return;
        }
    }
    if ($("#checkDef").is(':checked')) {
        horaini = "";
        horafin = "";
    }
    //var horaInicialSemanas = moment(new Date("2018-01-12 " + $("#HoraInicialSemanas").val())).format("HH");
    //var horaFinalSemanas = moment(new Date("2018-01-12 " + $("#HoraFinalSemanas").val())).format("HH");
    var horaInicialSemanas = $("#HoraInicialSemanas").val();
    var horaFinalSemanas = $("#HoraFinalSemanas").val();
    if ($('input[type=radio][name=radioDiaSemana]:checked').val() == "semanas") {
        if (!horaInicialSemanas) {
            toastr.error('Ingrese hora incial.', 'Servidor');
            return;
        }
        if (!horaFinalSemanas) {
            toastr.error('Ingrese hora final.', 'Servidor');
            return;
        }
        if (fechaini == fechafin) {
            if (parseInt(horaInicialSemanas) >= parseInt(horaFinalSemanas)) {
                toastr.error('La hora final tiene que ser mayor a la hora inicial.', 'Servidor');
                return;
            }
        }
    }

    fechaini = moment(fechaini, 'DD/MM/YYYY HH:mm').format("YYYY/MM/DD HH:mm");
    if ($('input[type=radio][name=radioDiaSemana]:checked').val() == "semanas") {
        fechaini = moment(fechaini).subtract(1, 'd').format('YYYY-MM-DD HH:mm');
        fechaini = moment(fechaini, 'YYYY-MM-DD HH:mm').format("YYYY/MM/DD HH:mm");
    }
    fechafin = moment(fechafin, 'DD/MM/YYYY HH:mm').format("YYYY/MM/DD HH:mm");
    $.ajax({
        type: 'GET',
        url: basePath + '/Reporte9050/ObtenerContadores',
        //url: '/ReporteGerencia/GetObtenerDatos',
        data: { fechaInicio: fechaini, fechaFin: fechafin, codSala: codSala, CurrentC: CurrentC, tipo: tipoIH },
        dataType: 'json',
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response1) {
            //response1 = responseFake;
            var response = response1.cabecera;
            detalle = response1.detalle;
            if (response.length > 0) {
                if ($.fn.DataTable.isDataTable('#tableConteo')) {
                    tableConteo.DataTable().destroy();
                }
                if ($('input[type=radio][name=radioDiaSemana]:checked').val() == "dias") {
                    var fechas = [...new Set(response.map(item => item.Fecha))];
                    var horas = [8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 0, 1, 2, 3, 4, 5, 6, 7];
                    strHead = "<tr><th>Fecha</th>";
                    for (var i = 0; i < 24; i++) {
                        strHead += "<th class=" + 'no-sort' + ">" + horas[i] + ":00 </th>";
                    }
                    strHead += "</tr>";
                    tHeadTableConteo.html("");
                    tHeadTableConteo.append(strHead);
                    strBodyTotal = "";
                    $.each(fechas, function (index, value) {
                        var days1 = ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'];
                        var fecha1 = moment(value).format("YYYY-MM-DD");
                        var de = new Date(fecha1);
                        de.setDate(de.getDate() + 1);
                        var dayName1 = days1[de.getDay()];
                        //strBody = "<tr><td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + 'todos' + ">" + moment(value).format("DD-MM-YYYY") + "</td>";
                        strBody = '<tr><td><i style="color:steelblue;" class="fa fa-gear fa-fw ModalDetalleXHora" data-fecha="' + value + '"></i><h8 style="padding-left:10px;" class="ModalDetalle" data-fecha="' + value + '" data-hora="todos">' + moment(value).format("YYYY-MM-DD") + '(' + dayName1 + ')</h8></td>';
                        //strBody = "<tr><td><i class="+'ModalDetalleXHora'+" data-fecha=" + value + "></i><h8 class="+'ModalDetalle'+" data-fecha=" + value + " data-hora="+'todos'+">" + moment(value).format("DD-MM-YYYY") + "</h8></td>";
                        $.each(horas, function (index1, value1) {
                            var celda = response.find(x => x.Fecha == value && x.Hora == value1);
                            if (celda == undefined) {
                                strBody += "<td class=" + 'ModalDetalle ' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                            } else {
                                //strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                strBody += '<td class="ModalDetalle " data-fecha="' + celda.Fecha + '" data-hora="' + celda.Hora + '">' + celda.Maquinas + '</td>';
                            }
                        });
                        strBody += "</tr>";
                        strBodyTotal += strBody;
                    });
                    tBodyTableConteo.html("");
                    tBodyTableConteo.append(strBodyTotal);
                    $('#tHeadTableConteo > tr> th:nth-child(1)').css({ 'min-width': '150px', 'max-width': '150px' });
                    
                    dataTable2 = tableConteo.DataTable({
                        paginate: true,
                        filter: false,
                        scrollX: true,
                        info: true,
                        bInfo: true,
                        processing: false,
                        serverSide: false,
                        responsive: false,
                        autoWidth: true,
                        bSort: true,
                        columnDefs: columnDefsDataTable,
                        scrollCollapse: true,
                        language: languageDataTable,
                        order: [[0, 'asc']]
                    });
                    //$(".ModalDetalle").addClass("dt-center");
                }
                else {
                    if ($('input[type=radio][name=radioTipoMostrar]:checked').val() == "cronologico") {
                        var filtroDias = [];
                        var days = ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'];

                        $('#checkDias :checked').each(function () {
                            filtroDias.push($(this).val());
                        });
                        var fechas = [...new Set(response.map(item => item.Fecha))];
                        fechas.splice(-1, 1);
                        //var horas = [8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 0, 1, 2, 3, 4, 5, 6, 7];
                        var horas = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23];
                        strHead = "<tr><th>Fecha</th>";
                        for (var i = 0; i < 24; i++) {
                            if (fechas.length == 1) {
                                if (horas[i] >= horaInicialSemanas && horas[i] <= horaFinalSemanas) {
                                    strHead += "<th class=" + 'no-sort' + ">" + horas[i] + ":00 </th>";
                                }
                            } else {
                                strHead += "<th class=" + 'no-sort' + ">" + horas[i] + ":00 </th>";
                            }
                        }
                        strHead += "</tr>";
                        tHeadTableConteo.html("");
                        tHeadTableConteo.append(strHead);
                        strBodyTotal = "";
                        $.each(fechas, function (index, value) {
                            var fecha = moment(value).format("YYYY-MM-DD");
                            var d = new Date(fecha);
                            d.setDate(d.getDate() + 1);
                            var dayName = days[d.getDay()];
                            if (filtroDias.indexOf(dayName) > -1) {
                                //strBody = "<tr><td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + 'todos' + ">" + "(" + dayName + ") " + moment(value).format("DD-MM-YYYY") + "</td>";
                                //strBody = '<tr><td><i style="color:steelblue;" class="fa fa-gear fa-fw ModalDetalleXHora" data-fecha="' + value + '"></i><h8 style="padding-left:10px;" class="ModalDetalle" data-fecha="' + value + '" data-hora="todos">(' + dayName+')' + moment(value).format("DD-MM-YYYY") + '</h8></td>';
                                strBody = '<tr><td><i style="color:steelblue;" class="fa fa-gear fa-fw ModalDetalleXHora" data-fecha="' + value + '"></i><h8 style="padding-left:10px;" class="ModalDetalle" data-fecha="' + value + '" data-hora="todos">' + moment(value).format("YYYY/MM/DD") + '(' + dayName + ')</h8></td>';
                                if (index == 0) {
                                    $.each(horas, function (index1, value1) {
                                        if (fechas.length == 1) {
                                            if (value1 >= horaInicialSemanas && value1 <= horaFinalSemanas) {
                                                if (value1 <= 7) {
                                                    var celda = response.find(x => moment(x.Fecha).format("YYYY-MM-DD") == moment(value).subtract(1, 'd').format('YYYY-MM-DD') && x.Hora == value1);
                                                    if (celda == undefined) {
                                                        strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                    } else {
                                                        strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                    }
                                                } else {
                                                    var celda = response.find(x => x.Fecha == value && x.Hora == value1);
                                                    if (celda == undefined) {
                                                        strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                    } else {
                                                        strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                    }
                                                }
                                            }
                                        } else {
                                            if (value1 <= horaFinalSemanas) {
                                                if (value1 <= 7) {
                                                    var celda = response.find(x => moment(x.Fecha).format("YYYY-MM-DD") == moment(value).subtract(1, 'd').format('YYYY-MM-DD') && x.Hora == value1);
                                                    if (celda == undefined) {
                                                        strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                    } else {
                                                        strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                    }
                                                } else {
                                                    var celda = response.find(x => x.Fecha == value && x.Hora == value1);
                                                    if (celda == undefined) {
                                                        strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                    } else {
                                                        strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                    }
                                                }
                                            } else {
                                                strBody += "<td data-fecha=" + 0 + " data-hora=" + 0 + ">x</td>";
                                            }
                                        }

                                    });
                                }
                                if (index == fechas.length - 1 && fechas.length > 1) {
                                    $.each(horas, function (index1, value1) {
                                        if (value1 >= horaInicialSemanas) {
                                            if (value1 <= 7) {
                                                var celda = response.find(x => moment(x.Fecha).format("YYYY-MM-DD") == moment(value).subtract(1, 'd').format('YYYY-MM-DD') && x.Hora == value1);
                                                if (celda == undefined) {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                } else {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                }
                                            } else {
                                                var celda = response.find(x => x.Fecha == value && x.Hora == value1);
                                                if (celda == undefined) {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                } else {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                }
                                            }
                                        } else {
                                            strBody += "<td data-fecha=" + 0 + " data-hora=" + 0 + ">x</td>";
                                        }
                                    });
                                }
                                if (index != (fechas.length - 1) && index != 0) {
                                    $.each(horas, function (index1, value1) {
                                        if (value1 <= 7) {
                                            var celda = response.find(x => moment(x.Fecha).format("YYYY-MM-DD") == moment(value).subtract(1, 'd').format('YYYY-MM-DD') && x.Hora == value1);
                                            if (celda == undefined) {
                                                strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                            } else {
                                                strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                            }
                                        } else {
                                            var celda = response.find(x => x.Fecha == value && x.Hora == value1);
                                            if (celda == undefined) {
                                                strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                            } else {
                                                strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                            }
                                        }
                                    });
                                }
                                strBody += "</tr>";
                                strBodyTotal += strBody;
                            }
                        });

                        tBodyTableConteo.html("");
                        tBodyTableConteo.append(strBodyTotal);
                        $('#tHeadTableConteo > tr> th:nth-child(1)').css({ 'min-width': '150px', 'max-width': '150px' });

                        dataTable2 = tableConteo.DataTable({
                            paginate: true,
                            filter: false,
                            info: false,
                            scrollX: true,
                            processing: false,
                            serverSide: false,
                            responsive: false,
                            autoWidth: true,
                            bSort: true,
                            columnDefs: columnDefsDataTable,
                            scrollCollapse: true,
                            language: languageDataTable,
                            order: [[0, 'asc']]
                        });
                    }
                    else {
                        var filtroDias = [];
                        var days = ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'];
                        $('#checkDias :checked').each(function () {
                            filtroDias.push($(this).val());
                        });
                        var fechas = [...new Set(response.map(item => item.Fecha))];
                        fechas.splice(-1, 1);
                        if (parseInt(horaInicialSemanas) > 12 && parseInt(horaInicialSemanas) < 24) {
                            if (parseInt(horaInicialSemanas) <= parseInt(horaFinalSemanas)) {
                                var horas = [8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 0, 1, 2, 3, 4, 5, 6, 7];
                                strHead = "<tr><th>Fecha</th>";
                                for (var i = parseInt(horaInicialSemanas); i <= parseInt(horaFinalSemanas); i++) {
                                    strHead += "<th class=" + 'no-sort' + ">" + i + ":00 </th>";
                                }
                                strHead += "</tr>";
                                tHeadTableConteo.html("");
                                tHeadTableConteo.append(strHead);
                                strBodyTotal = "";
                                $.each(fechas, function (index, value) {
                                    var fecha = moment(value).format("YYYY-MM-DD");
                                    var d = new Date(fecha);
                                    d.setDate(d.getDate() + 1);
                                    var dayName = days[d.getDay()];
                                    if (filtroDias.indexOf(dayName) > -1) {
                                        strBody = '<tr><td><i style="color:steelblue;" class="fa fa-gear fa-fw ModalDetalleXHora" data-fecha="' + value + '"></i><h8 style="padding-left:10px;" class="ModalDetalle" data-fecha="' + value + '" data-hora="todos">' + moment(value).format("YYYY/MM/DD") + '(' + dayName + ')</h8></td>';
                                        $.each(horas, function (index1, value1) {
                                            if (value1 >= horaInicialSemanas && value1 <= horaFinalSemanas) {
                                                var celda = response.find(x => x.Fecha == value && x.Hora == value1);
                                                if (celda == undefined) {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                } else {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                }
                                            }
                                        });
                                        strBody += "</tr>";
                                        strBodyTotal += strBody;
                                    }
                                });

                                tBodyTableConteo.html("");
                                tBodyTableConteo.append(strBodyTotal);
                                $('#tHeadTableConteo > tr> th:nth-child(1)').css({ 'min-width': '150px', 'max-width': '150px' });
                            }
                            else {
                                var horas = [];
                                strHead = "<tr><th>Fecha</th>";
                                for (var i = parseInt(horaInicialSemanas); i < 24; i++) {
                                    strHead += "<th class=" + 'no-sort' + ">" + i + ":00 </th>";
                                    horas.push(i);
                                }
                                for (var i = 0; i <= parseInt(horaFinalSemanas); i++) {
                                    strHead += "<th class=" + 'no-sort' + ">" + i + ":00 </th>";
                                    horas.push(i);
                                }
                                strHead += "</tr>";
                                tHeadTableConteo.html("");
                                tHeadTableConteo.append(strHead);
                                strBodyTotal = "";
                                $.each(fechas, function (index, value) {
                                    var fecha = moment(value).format("YYYY-MM-DD");
                                    var d = new Date(fecha);
                                    d.setDate(d.getDate() + 1);
                                    var dayName = days[d.getDay()];
                                    if (filtroDias.indexOf(dayName) > -1) {
                                        strBody = '<tr><td><i style="color:steelblue;" class="fa fa-gear fa-fw ModalDetalleXHora" data-fecha="' + value + '"></i><h8 style="padding-left:10px;" class="ModalDetalle" data-fecha="' + value + '" data-hora="todos">' + moment(value).format("YYYY/MM/DD") + '(' + dayName + ')</h8></td>';
                                        $.each(horas, function (index1, value1) {
                                            if (value1 >= parseInt(horaInicialSemanas) && value1 < 24) {
                                                var celda = response.find(x => x.Fecha == value && x.Hora == value1);
                                                if (celda == undefined) {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                } else {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                }
                                            }
                                            if (value1 >= 0 && value1 <= 7) {
                                                var celda = response.find(x => x.Fecha == value && x.Hora == value1);
                                                if (celda == undefined) {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                } else {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                }
                                            }
                                            if (value1 > 7 && value1 <= parseInt(horaFinalSemanas)) {
                                                var celda = response.find(x => moment(x.Fecha).format("YYYY-MM-DD") == moment(value).add(1, 'd').format('YYYY-MM-DD') && x.Hora == value1);
                                                if (celda == undefined) {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                } else {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                }
                                            }
                                        });
                                        strBody += "</tr>";
                                        strBodyTotal += strBody;
                                    }
                                });

                                tBodyTableConteo.html("");
                                tBodyTableConteo.append(strBodyTotal);
                                $('#tHeadTableConteo > tr> th:nth-child(1)').css({ 'min-width': '150px', 'max-width': '150px' });
                            }
                        }
                        if (parseInt(horaInicialSemanas) >= 00 && parseInt(horaInicialSemanas) <= 12) {
                            if (parseInt(horaInicialSemanas) <= parseInt(horaFinalSemanas)) {
                                var horas = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23];
                                strHead = "<tr><th>Fecha</th>";
                                for (var i = parseInt(horaInicialSemanas); i <= parseInt(horaFinalSemanas); i++) {
                                    strHead += "<th class=" + 'no-sort' + ">" + i + ":00 </th>";
                                }
                                strHead += "</tr>";
                                tHeadTableConteo.html("");
                                tHeadTableConteo.append(strHead);
                                strBodyTotal = "";
                                $.each(fechas, function (index, value) {
                                    var fecha = moment(value).format("YYYY-MM-DD");
                                    var d = new Date(fecha);
                                    d.setDate(d.getDate() + 1);
                                    var dayName = days[d.getDay()];
                                    if (filtroDias.indexOf(dayName) > -1) {
                                        strBody = '<tr><td><i style="color:steelblue;" class="fa fa-gear fa-fw ModalDetalleXHora" data-fecha="' + value + '"></i><h8 style="padding-left:10px;" class="ModalDetalle" data-fecha="' + value + '" data-hora="todos">' + moment(value).format("YYYY/MM/DD") + '(' + dayName + ')</h8></td>';
                                        $.each(horas, function (index1, value1) {
                                            if (value1 >= parseInt(horaInicialSemanas) && value1 <= parseInt(horaFinalSemanas)) {
                                                if (value1 >= 0 && value1 <= 7) {
                                                    var celda = response.find(x => moment(x.Fecha).format("YYYY-MM-DD") == moment(value).subtract(1, 'd').format('YYYY-MM-DD') && x.Hora == value1);
                                                    if (celda == undefined) {
                                                        strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                    } else {
                                                        strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                    }
                                                }
                                                if (value1 > 7 && value1 < 24) {
                                                    var celda = response.find(x => x.Fecha == value && x.Hora == value1);
                                                    if (celda == undefined) {
                                                        strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                    } else {
                                                        strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                    }
                                                }
                                            }
                                        });
                                        strBody += "</tr>";
                                        strBodyTotal += strBody;
                                    }
                                });

                                tBodyTableConteo.html("");
                                tBodyTableConteo.append(strBodyTotal);
                                $('#tHeadTableConteo > tr> th:nth-child(1)').css({ 'min-width': '150px', 'max-width': '150px' });
                            }
                            else {
                                var horas = [];
                                strHead = "<tr><th>Fecha</th>";
                                for (var i = parseInt(horaInicialSemanas); i < 24; i++) {
                                    strHead += "<th class=" + 'no-sort' + ">" + i + ":00 </th>";
                                    horas.push(i);
                                }
                                for (var i = 0; i <= parseInt(horaFinalSemanas); i++) {
                                    strHead += "<th class=" + 'no-sort' + ">" + i + ":00 </th>";
                                    horas.push(i);
                                }

                                strHead += "</tr>";
                                tHeadTableConteo.html("");
                                tHeadTableConteo.append(strHead);
                                strBodyTotal = "";
                                $.each(fechas, function (index, value) {
                                    var fecha = moment(value).format("YYYY-MM-DD");
                                    var d = new Date(fecha);
                                    d.setDate(d.getDate() + 1);
                                    var dayName = days[d.getDay()];
                                    if (filtroDias.indexOf(dayName) > -1) {
                                        strBody = '<tr><td><i style="color:steelblue;" class="fa fa-gear fa-fw ModalDetalleXHora" data-fecha="' + value + '"></i><h8 style="padding-left:10px;" class="ModalDetalle" data-fecha="' + value + '" data-hora="todos">' + moment(value).format("YYYY/MM/DD") + '(' + dayName + ')</h8></td>';
                                        $.each(horas, function (index1, value1) {
                                            if (value1 >= 8 && value1 <= 23) {
                                                var celda = response.find(x => x.Fecha == value && x.Hora == value1);
                                                if (celda == undefined) {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                } else {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                }
                                            }
                                            if (value1 >= parseInt(horaInicialSemanas) && value1 <= 7) {
                                                var celda = response.find(x => x.Fecha == value && x.Hora == value1);
                                                if (celda == undefined) {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                } else {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                }
                                            }
                                            if (value1 >= 0 && value1 <= parseInt(horaFinalSemanas)) {
                                                //var celda = response.find(x => moment(x.Fecha).format("YYYY-MM-DD") == moment(value).add(1, 'd').format('YYYY-MM-DD') && x.Hora == value1);
                                                var celda = response.find(x => x.Fecha == value && x.Hora == value1);
                                                if (celda == undefined) {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + value + " data-hora=" + value1 + ">" + 0 + "</td>";
                                                } else {
                                                    strBody += "<td class=" + 'ModalDetalle' + " data-fecha=" + celda.Fecha + " data-hora=" + celda.Hora + ">" + celda.Maquinas + "</td>";
                                                }
                                            }
                                        });
                                        strBody += "</tr>";
                                        strBodyTotal += strBody;
                                    }
                                });

                                tBodyTableConteo.html("");
                                tBodyTableConteo.append(strBodyTotal);
                                $('#tHeadTableConteo > tr> th:nth-child(1)').css({ 'min-width': '150px', 'max-width': '150px' });
                            }
                        }
                        dataTable2 = tableConteo.DataTable({
                            paginate: true,
                            filter: false,
                            info: false,
                            scrollX: true,
                            processing: false,
                            serverSide: false,
                            responsive: false,
                            autoWidth: true,
                            bSort: true,
                            columnDefs: columnDefsDataTable,
                            scrollCollapse: true,
                            language: languageDataTable,
                            order: [[0, 'asc']]
                        });
                    }
                }
            } else {
                if ($.fn.DataTable.isDataTable('#tableConteo')) {
                    tableConteo.DataTable().clear().draw();
                }
                toastr.error('No hay datos en esa fecha.', 'Servidor');
            }
            $('#ModalParametrosBusqueda').modal('hide');
        }
    });
    //setTimeout(function () {
    //}, 500);

});

$(document).on('click', '.ModalDetalle', function (e) {
    checkAllCheckboxByName('checkboxFiltroDetalle');
    ArrNoImprimir = [];
    ArrNoImprimir.push("Fecha");
    ArrNoImprimir.push("Hora");
    ArrNoImprimir.push("CodContadoresOnline");
    //ArrNoImprimir.push("CurrentCredits");       
    ArrNoImprimir.push("GamesPlayed");
    ArrNoImprimir.push("CancelCredits");
    ArrNoImprimir.push("Bill");
    ArrNoImprimir.push("Token");
    ArrNoImprimir.push("FechaReal");
    $("#btnci").prop("checked", true);
    $("#btnco").prop("checked", true);
    $("#btncp").prop("checked", true);
    var fecha = $(this).data("fecha");
    var hora = $(this).data("hora");
    listarDetalleModal(fecha, hora);
    $('#ModalDetalle').modal('show');
});

$(document).on('click', '.ModalDetalleXHora', function (e) {
    var fecha = $(this).data("fecha");
    listarDetalleModalXHora(fecha);
    $('#ModalDetalleXHora').modal('show');
});

$('#btnci').on('ifChecked ifUnchecked', function () {
    if ($(this).is(':checked')) {
        objetodatatable.column(3).visible(true);
        var index = ArrNoImprimir.indexOf("CoinIn");
        if (index > -1) {
            ArrNoImprimir.splice(index, 1);
        }
    } else {
        objetodatatable.column(3).visible(false);
        ArrNoImprimir.push("CoinIn");
    }
});
$('#btnco').on('ifChecked ifUnchecked', function () {
    if ($(this).is(':checked')) {
        objetodatatable.column(4).visible(true);
        var index = ArrNoImprimir.indexOf("CoinOut");
        if (index > -1) {
            ArrNoImprimir.splice(index, 1);
        }
    }
    else {
        objetodatatable.column(4).visible(false);
        ArrNoImprimir.push("CoinOut");
    }
});
$('#btncp').on('ifChecked ifUnchecked', function () {
    if ($(this).is(':checked')) {
        objetodatatable.column(5).visible(true);
        var index = ArrNoImprimir.indexOf("Promedio");
        if (index > -1) {
            ArrNoImprimir.splice(index, 1);
        }
    }
    else {
        objetodatatable.column(5).visible(false);
        ArrNoImprimir.push("Promedio");
    }
});
$('#btncc').on('ifChecked ifUnchecked', function () {
    if ($(this).is(':checked')) {
        objetodatatable.column(6).visible(true);
        var index = ArrNoImprimir.indexOf("CurrentCredits");
        if (index > -1) {
            ArrNoImprimir.splice(index, 1);
        }
    }
    else {
        objetodatatable.column(6).visible(false);
        ArrNoImprimir.push("CurrentCredits");
    }
});
$('#btnma').on('ifChecked ifUnchecked', function () {
    if ($(this).is(':checked')) {
        objetodatatable.column(1).visible(true);
        var index = ArrNoImprimir.indexOf("Marca");
        if (index > -1) {
            ArrNoImprimir.splice(index, 1);
        }
    }
    else {
        objetodatatable.column(1).visible(false);
        ArrNoImprimir.push("Marca");
    }
});
$('#btnmo').on('ifChecked ifUnchecked', function () {
    if ($(this).is(':checked')) {
        objetodatatable.column(2).visible(true);
        var index = ArrNoImprimir.indexOf("Modelo");
        if (index > -1) {
            ArrNoImprimir.splice(index, 1);
        }
    }
    else {
        objetodatatable.column(2).visible(false);
        ArrNoImprimir.push("Modelo");
    }
});

$('#checkDef').click(function () {
    if ($(this).is(':checked')) {
        $("#HoraInicial").attr("disabled", true);
        $("#HoraFinal").attr("disabled", true);
        $("#HoraInicial").val("");
        $("#HoraFinal").val("");
    }
    else {
        $("#HoraInicial").attr("disabled", false);
        $("#HoraFinal").attr("disabled", false);
    }
});
$("#ExportToExcel").click(function () {
    if (!dataTable2) {
        toastr.warning("Primero seleccione una sala y consulte.")
        return;
    }
    if (dataTable2.data().length > 0) {
        var data = [];
        for (var i = 0; i < dataTable2.rows().data().length; i++) {
            var objtemp = {};
            var objStr = '{';
            var tmp = dataTable2.rows().data()[i];
            for (var j = 0; j < tmp.length; j++) {
                if (j == 0 && tmp[0].length > 22) {
                    tmp[0] = $(tmp[0]).text();
                }
                var tmpheader = dataTable2.column(j).header();
                objStr += '"' + $(tmpheader).text() + '":"' + tmp[j] + '",'
            }
            objStr = objStr.substring(0, objStr.length - 1);
            objStr += '}';
            //objtemp.push(JSON.parse(objStr));                                
            data.push(JSON.parse(objStr));
        }
        var nombre = 'Resultados';
        if (tipoExcell == 1) {
            nombre = 'Resultados X Instante';
        }
        if (tipoExcell == 2) {
            nombre = 'Resultados X Hora';
        }
        let codSala = cmbSala.val();
        var obj = {
            CodSala: codSala,
            Title: 'Resultados',
            Filename: nombre,
            JsonContent: JSON.stringify(data),
            //IgnoredColumns: JSON.stringify(['CodTipoSorteo'])
        };

        ExportToExcel(obj, uris.ExportToExcel);
    }
});
$("#ExportToExcelDetalle").click(function () {
    if (objetodatatable.rows().data().length > 0) {
        var data = [];
        for (var i = 0; i < objetodatatable.rows().data().length; i++) {
            data.push(objetodatatable.rows().data()[i]);
        }
        data.push({ CodMaq: " ", Marca: " ", Modelo: "Total:", CoinIn: $("#totalCI").text(), CoinOut: $("#totalCO").text(), Promedio: $("#totalPM").text(), CurrentCredits: $("#totalCC").text() });
        let codSala = cmbSala.val();
        var obj = {
            CodSala: codSala,
            Title: $("#TituloModal").text(),
            Filename: 'Detalle',
            JsonContent: JSON.stringify(data),
            IgnoredColumns: JSON.stringify(ArrNoImprimir)
        };

        ExportToExcel(obj, uris.ExportToExcel);
    }
});
$("#ExportToExcelDetalleXHora").click(function () {
    if (dataTableDetalleXHora.rows().data().length > 0) {
        var data = [];
        for (var i = 0; i < dataTableDetalleXHora.rows().data().length; i++) {
            var objtemp = {};
            var objStr = '{';
            var tmp = dataTableDetalleXHora.rows().data()[i];
            for (var j = 0; j < tmp.length; j++) {
                var tmpheader = dataTableDetalleXHora.column(j).header();
                objStr += '"' + $(tmpheader).text() + '":"' + tmp[j] + '",'
            }
            objStr = objStr.substring(0, objStr.length - 1);
            objStr += '}';
            //objtemp.push(JSON.parse(objStr));                
            data.push(JSON.parse(objStr));
        }
        let codSala = cmbSala.val();
        var obj = {
            CodSala: codSala,
            Title: 'Resultados Detalle',
            Filename: 'Resultados_Detalle',
            JsonContent: JSON.stringify(data)
            //IgnoredColumns: JSON.stringify(['CodTipoSorteo'])
        };

        ExportToExcel(obj, uris.ExportToExcel);
    }
});
//#endregion

//#region Metodos
const checkAllCheckboxByName = (name) => {
    $(`input[type=checkbox][name=${name}]`).iCheck('check');
}

const setICheck = () => {
    const configICheck = {
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-blue',
        increaseArea: '20%'
    }
    $('input[type=radio][name=radioDiaSemana]').iCheck(configICheck);
    $('input[type=radio][name=radioTipoMostrar]').iCheck(configICheck);
    $('input[type=checkbox][name=checkboxDias]').iCheck(configICheck);
    $('input[type=checkbox][name=checkboxFiltroDetalle]').iCheck(configICheck);
}

const setDateTimePicker = () => {
    $(".onlyDate").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        ignoreReadonly: true,
        autoclose: false,
        language: 'es'
    })
    $(".onlyHour").datetimepicker({
        pickDate: false,
        pickTime: true,
        format: 'HH',
        ignoreReadonly: true,
        autoclose: false,
        language: 'es',
        pickHour: false
    })
}

const setButtonsDisabled = (disabled) => {
    btnBusquedaPorHora.prop('disabled', disabled);
    btnBusquedaPorInstante.prop('disabled', disabled);
}

const getSalas = () => {
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            cmbSala.append('<option value=""></option>');
            $.each(datos, function (index, value) {
                cmbSala.append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            cmbSala.select2({
                placeholder: "Seleccione una Sala", allowClear: true
            });
            cmbSala.val(null).trigger("change");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}

const ExportToExcel = (data, url) => {
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json;charset=UTF-8",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            let message = response.displayMessage;
            if (response.success) {
                let dataExcel = response.fileContent
                let linkExcel = document.createElement('a')
                document.body.appendChild(linkExcel)
                linkExcel.href = `data:${response.fileMimeType};base64,` + dataExcel
                linkExcel.download = response.fileName
                linkExcel.click()
                linkExcel.remove()
                toastr.success(message)
            } else {
                toastr.warning(message)
            }
        }
    });
}

const listarDetalleModal = (fecha, hora) => {
    var dataModal = [];
    if (hora == "todos") {
        $("#TituloModal").text("Detalle " + moment(fecha).format("DD/MM/YYYY"));
        dataModal1 = detalle.filter(x => x.Fecha == fecha);
        var result = [];
        dataModal2 = dataModal1.reduce(function (res, value) {
            if (!res[value.CodMaq]) {
                res[value.CodMaq] = {
                    CodMaq: value.CodMaq,
                    Maquina: value.Maquina,
                    NroSerie: value.NroSerie,
                    Sala: value.Sala,
                    Marca: value.Marca,
                    Modelo: value.Modelo,
                    CoinIn: 0,
                    CoinOut: 0,
                    Promedio: 0,
                    CurrentCredits: 0
                };
                result.push(res[value.CodMaq]);
            }
            res[value.CodMaq].CoinIn += value.CoinIn;
            res[value.CodMaq].CoinOut += value.CoinOut;
            res[value.CodMaq].Promedio += value.Promedio;
            res[value.CodMaq].CurrentCredits += value.CurrentCredits;
            return res;
        },
            {});
        rest = [];
        $.each(dataModal2,
            function (index, value) {
                value.CoinIn = parseFloat(value.CoinIn).toFixed(2);
                value.CoinOut = parseFloat(value.CoinOut).toFixed(2);
                value.Promedio = parseFloat(value.Promedio).toFixed(2);
                value.CurrentCredits = parseFloat(value.CurrentCredits).toFixed(2);
                rest.push(value);
            });
        dataModal = rest;
    } else {
        $("#TituloModal").text("Detalle " + moment(fecha).format("DD/MM/YYYY") + " " + hora + ":00 Hrs");
        dataModal = detalle.filter(x => x.Fecha == fecha && x.Hora == hora);
    }
    objetodatatable = tableDetalle.DataTable({
        bDestroy: true,
        bSort: true,
        scrollCollapse: false,
        scrollX: false,
        paging: true,
        autoWidth: false,
        bProcessing: true,
        bDeferRender: true,
        data: dataModal,
        language: languageDataTable,
        columns: [
            { data: "CodMaq", title: "Maquina", className: "dt-center" },
            { data: "Marca", title: "Marca", className: "dt-center" },
            { data: "Modelo", title: "Modelo", className: "dt-center", },
            { data: "CoinIn", title: "Coin In", className: "dt-center", "width": "300px" },
            { data: "CoinOut", title: "Coin Out", className: "dt-center", "width": "300px" },
            {
                data: "Promedio", title: "Promedio de Apuesta", className: "dt-center",
                render: function (data) {
                    return parseFloat(data).toFixed(2);
                }
            },
            { data: "CurrentCredits", title: "Current Credit", className: "dt-center", }
        ],
        footerCallback: function (row, data, start, end, display) {
            var api = this.api(), data;

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            };

            // Total over all pages
            total = api
                .column(3)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            total1 = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            total2 = api
                .column(5)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            total3 = api
                .column(6)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            // Total over this page
            pageTotal = api
                .column(3, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            pageTotal1 = api
                .column(4, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            pageTotal2 = api
                .column(5, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            pageTotal3 = api
                .column(6, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            $(api.column(3).footer()).html(
                pageTotal.toFixed(2) + ' ( ' + total.toFixed(2) + ' total)'
            );
            $(api.column(4).footer()).html(
                pageTotal1.toFixed(2) + ' ( ' + total1.toFixed(2) + ' total)'
            );
            $(api.column(5).footer()).html(
                pageTotal2.toFixed(2) + ' ( ' + total2.toFixed(2) + ' total)'
            );
            $(api.column(6).footer()).html(
                pageTotal3.toFixed(2) + ' ( ' + total3.toFixed(2) + ' total)'
            );
        },
        initComplete: function (settings, json) {
            $('a#excel,a#pdf,a#imprimir').on('click', function () {
                var ocultar = "Accion";
                funcionbotones({
                    botonobjeto: this,
                    ocultar: ocultar,
                    tablaobj: objetodatatable
                });
            });
        }
    });
};

const listarDetalleModalXHora = (fecha) => {
    if ($.fn.DataTable.isDataTable('#tableDetalleXHora')) {
        tableDetalleXHora.DataTable().destroy();
    }
    $("#TituloModalXHora").text("Detalle " + moment(fecha).format("DD/MM/YYYY"));
    dataXFecha = detalle.filter(x => x.Fecha == fecha);
    var maquinas = [...new Set(dataXFecha.map(item => {
        return {
            CodMaq: item.CodMaq,
            NroSerie: item.NroSerie
        };
    }
    ))];
    var horas = [8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 0, 1, 2, 3, 4, 5, 6, 7];
    var strHead = "<tr><th>Maquina</th><th>NroSerie</th>";
    for (var i = 0; i < 24; i++) {
        strHead += "<th class=" + 'no-sort' + ">" + horas[i] + ":00 </th>";
    }
    strHead += "</tr>";
    tHeadTableDetalleXHora.html("");
    tHeadTableDetalleXHora.append(strHead);
    var strBodyTotal = "";
    $.each(maquinas, function (index, value) {
        var strBody = "<tr><td>" + value.CodMaq + "</td><td>" + value.NroSerie + "</td>";
        $.each(horas, function (index1, hora) {
            var celda = detalle.find(x => x.CodMaq == value.CodMaq && x.Hora == hora);
            if (celda == undefined) {
                strBody += "<td>" + 0 + "</td>";
            } else {
                strBody += "<td>" + 1 + "</td>";
            }
        });
        strBody += "</tr>";
        strBodyTotal += strBody;
    });
    tBodyTableDetalleXHora.html("");
    tBodyTableDetalleXHora.append(strBodyTotal);
    tableDetalleXHora.css({ 'min-width': '500px', 'max-width': '500px' });
    dataTableDetalleXHora = tableDetalleXHora.DataTable({
        paginate: true,
        filter: false,
        info: false,
        bInfo: true,
        scrollX: true,
        processing: false,
        serverSide: false,
        responsive: false,
        autoWidth: true,
        bSort: true,
        columnDefs: columnDefsDataTable,
        scrollCollapse: true,
        language: languageDataTable
    });
}
//#endregion


let consultaPorVpn = false
let ipPublica
let ipPrivada
let ipPublicaAlterna
let seEncontroAlterna = false
let urlsResultado = []
let delay = 60000
let timerId = ''
let idSala;
let ipSala;
var arrayFake = []
$(document).ready(function () {
    //obtenerDiscosUnique()
    $('#container').hide()

    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
    renderEquipoInfo([])
    $("#cboSala").select2()

    Highcharts.setOptions({
        lang: {
            months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            weekdays: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            shortMonths: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            decimalPoint: ',',
            thousandsSep: '.',
            loading: 'Cargando...',
            numericSymbols: ['k', 'M', 'G', 'T', 'P', 'E'],
            resetZoom: 'Reiniciar zoom',
            resetZoomTitle: 'Reiniciar zoom al nivel original',
            thousandsSep: '.',
            zoomIn: 'Aumentar zoom',
            zoomOut: 'Reducir zoom',

        }
    });

    obtenerListaSalas().then(result => {
        if (result.data) {
            renderSelectSalas(result.data)

        }
    })

    $(document).on('change', '#cboSala', function (e) {
        e.preventDefault()

        idSala = $("#cboSala option:selected").data('id')
        ipSala = $("#cboSala option:selected").data('ipprivada')
        ipPublica = $(this).val();
        ipPrivada = $("#cboSala option:selected").data('ipprivada')

        let puertoServicio = $("#cboSala option:selected").data('puertoservicio')
        ipPrivada = ipPrivada + ':' + puertoServicio

    })

    $(document).on('click', '#btnBuscar', function (e) {
        e.preventDefault()

        obtenerDiscosSala(idSala)

    })



    $(document).on("click", "#btnExcel", function () {
        let fechaIni = $("#fechaInicio").val();
        let fechaFin = $("#fechaFin").val();
        let sala = idSala;
        if (sala == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Equipo/ReporteEquipoDescargarExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ codSala: sala, fechaIni, fechaFin }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                if (response.respuesta) {
                    let data = response.data;
                    let file = response.excelName;
                    let a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/vnd.ms-excel;base64, " + data;
                    a.download = file;
                    a.click();
                }
            },
            error: function (request, status, error) {
                //toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });

    $(document).on('click', '#btnConsultar', function (e) {
        e.preventDefault();
        let uri = getUrl(ipPublica)
        obtenerDiscosUnique(ipPublica);
    })

    $(document).on('click', '#btnVerListado', function () {
        $('#container').hide()
        $('#container-listadoEquipo').show()

    })

    $(document).on('click', '#btnVerGrafico', function (e) {
        $('#container-listadoEquipo').hide()
        $('#container').show()
    })


});

function obtenerDiscosSala(idSala) {
    $.LoadingOverlay("show");
    var fechaIni = $("#fechaInicio").val();
    var fechaFin = $("#fechaFin").val();
    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Equipo/ListadoEquipoInfo",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codSala: idSala, fechaIni, fechaFin }),
        beforeSend: function (xhr) {
        },
        success: function (response) {
            console.log("soy el post")
            console.log(response)
            renderEquipoInfo(response.data)
            renderGrafico(response.data)
            $.LoadingOverlay("hide");
        },
        error: function (request, status, error) {
            toastr.error("No tiene permisos", "Mensaje Servidor");
            $.LoadingOverlay("hide");

        },
        complete: function (resul) {
        }
    });
}

function obtenerListaSalas() {
    return $.ajax({
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
            $.LoadingOverlay("hide");
        }
    });
}

function renderSelectSalas(data) {
    $.each(data, function (index, value) {
        $("#cboSala").append(`<option value="${value.UrlProgresivo == "" ? "" : value.UrlProgresivo}" data-id="${value.CodSala}" data-ipprivada="${value.IpPrivada}" data-puertoservicio="${value.PuertoServicioWebOnline}">${value.Nombre}</option>`)
    });
}



function renderEquipoInfo(data) {
    var dataFinal = data
    //var RegA = $("#RegA").val();
    //var RegD = $("#RegD").val();
    objetodatatable = $("#tableEquipo").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": false,
        "sScrollX": "100%",
        "paging": true,
        "ordering": true,
        "aaSorting": [],
        "autoWidth": false,
        "bAutoWidth": true,
        "bProcessing": true,
        "bDeferRender": true,
        data: dataFinal,
        columns: [
            {
                data: "IpEquipo", title: "IP",

            },
            {
                data: "MemoriaTotal", title: "Capacidad Ram",

            },
            { data: "MemoriaDisponible", title: "Capacidad Libre" },
            { data: "MemoriaUsada", title: "Capacidad ocupada" },
            { data: "PorcentajeUsoRam", title: "Porcentaje uso Ram" },
            {
                data: "PorcentajeCpu", title: "Porcentaje uso CPU",
                "render": function (o) {
                    var value = o.substring(0, o.length - 2);
                    var valorReal = parseInt(value);
                    var render =
                        //valorReal <= 5
                        //    ?
                        //    '<div class="emergencia" >' + o + ' </div>'
                        //    :
                            '<div >' + o + ' </div>'
                    return render;
                }
            },
            { data: "ProcesosCpu", title: "Procesos en uso de la CPU" },
            {
                data: "FechaRegistro", title: "Fecha Registro",
                "render": function (o) {

                    return moment(o).format("DD/MM/YYYY hh:mm a");
                }
            },
            //{
            //    data: null,
            //    "bSortable": false,
            //    "render": function (value) {
            //        var botones = '<button type="button" class="btn btn-xs btn-success btnVer" data-toggle="modal" data-target="#full-modal" data-RegD="' + RegD + '" data-RegA="' + RegA + '" data-Fecha="' + value.Fecha + '" data-maquina="' + value.SlotID + '" data-id="' + value.ProgresivoID + '"><i class="glyphicon  glyphicon-search"></i></button>';
            //        botones += '<button type="button" class="btn btn-xs btn-danger btnVerContadores" data-fecha="' + value.Fecha + '" data-maquina="' + value.SlotID + '" data-id="' + value.ProgresivoID + '"><i class="glyphicon  glyphicon-calendar"></i></button>';
            //        return botones;
            //    }
            //}
        ]
        ,
        //"initComplete": function (settings, json) {

        //    $('#btnExcel').off("click").on('click', function () {

        //        cabecerasnuevas = [];
        //        cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
        //        cabecerasnuevas.push({ nombre: "Progresivo", valor: $("#cboProgresivo option:selected").text() });
        //        cabecerasnuevas.push({ nombre: "Máquina", valor: $("#txtMaquina").val() });
        //        cabecerasnuevas.push({ nombre: "Pozos", valor: $("#cboPozos option:selected").text() });

        //        definicioncolumnas = [];
        //        definicioncolumnas.push({ nombre: "Monto", tipo: "decimal", alinear: "right", sumar: "true" });

        //        var ocultar = [];//"Accion";
        //        funcionbotonesnuevo({
        //            botonobjeto: this, ocultar: ocultar,
        //            tablaobj: objetodatatable,
        //            cabecerasnuevas: cabecerasnuevas,
        //            definicioncolumnas: definicioncolumnas
        //        });
        //    });

        //},
    });

}

const renderGrafico = (data) => {

    var arrayFake = data


    var options = {

        title: {
            text: 'Porcentaje de uso RAM y CPU'
        },
        xAxis: {
            type: 'datetime',
            title: {
                text: 'Fechas'
            },
            minPadding: 0.05,
            maxPadding: 0.05,
        },
        yAxis: {
            title: {
                text: 'Porcentaje'
            },
            tickInterval: 10

        },
        tooltip: {
            formatter: function () {
                let tooltipText = '<b>' + Highcharts.dateFormat('%d %b %Y %H:%M:%S', this.x) + '</b><br/>';

                if (this.series.name === 'CPU') {
                    tooltipText += 'Uso de CPU: ' + this.y + '%<br/>';
                    tooltipText += 'Procesos: ' + this.point.procesos + '<br/>';
                } else {
                    tooltipText += 'Uso de RAM: ' + this.y + '%<br/>';
                    tooltipText += 'Espacio total: ' + this.point.capacidadTotal + ' GB<br/>';
                    tooltipText += 'Espacio usado: ' + this.point.capacidadUsada + ' GB<br/>';
                    tooltipText += 'Espacio libre: ' + this.point.capacidadLibre + ' GB<br/>';
                }

                return tooltipText;
            }
        },
        series: [],
        chart: {
            zoomType: "xy",
            height: 700

        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle',
            itemMarginBottom:10,
            itemStyle: {
                fontSize: '16px',
               
            }
        },
        plotOptions: {
            line: {
                marker: {
                    enabled: true
                }
            }
        },
        responsive: {
            rules: [{
                condition: {
                    maxWidth: 500
                },
                chartOptions: {
                    legend: {
                        layout: 'horizontal',
                        align: 'center',
                        verticalAlign: 'bottom'
                    }
                }
            }]
        },
    };

    var series = {};
    series["CPU"] = []
    series["RAM"] = []
    for (var i = 0; i < arrayFake.length; i++) {
         
        var fechaRegistro = new Date(parseInt(arrayFake[i].FechaRegistro.substr(6)));
        var offsetMinutos = fechaRegistro.getTimezoneOffset();

        series["CPU"].push({
            x: fechaRegistro.getTime() - offsetMinutos * 60 * 1000 ,
            y: parseFloat(arrayFake[i].PorcentajeCpu),
            procesos: arrayFake[i].ProcesosCpu
        })
        series["RAM"].push({
            x: fechaRegistro.getTime() - offsetMinutos * 60 * 1000,
            y: parseFloat(arrayFake[i].PorcentajeUsoRam),
            capacidadLibre: parseFloat(arrayFake[i].MemoriaDisponible),
            capacidadTotal: parseFloat(arrayFake[i].MemoriaTotal),
            capacidadUsada: parseFloat(arrayFake[i].MemoriaUsada),

        })


    }
    console.log(series)

    for (var equipo in series) {
        options.series.push({
            name: equipo,
            data: series[equipo],
            pointInterval: 24
        });
    }
    console.log(options)
    $(function () {
        $('#container').highcharts(options);

    });
}




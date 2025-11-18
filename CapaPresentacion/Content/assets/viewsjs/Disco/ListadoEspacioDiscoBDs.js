
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
    renderDiscoss([])
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
        //console.log(idSala)
        ipPublica = $(this).val();
        ipPrivada = $("#cboSala option:selected").data('ipprivada')

        let puertoServicio = $("#cboSala option:selected").data('puertoservicio')
        ipPrivada = ipPrivada + ':' + puertoServicio

    })

    $(document).on('click', '#btnBuscar', function (e) {
        e.preventDefault()
        obtenerEspacioDiscosBD();

    })


    $(document).on('click', '#btnVerListado', function () {
        $('#container').hide()
        $('#container-listadoDisco').show()

    })

    $(document).on('click', '#btnVerGrafico', function (e) {
        $('#container-listadoDisco').hide()
        $('#container').show()
    })


    $(document).on('click', '.btnLimpiarLog', function (e) {

        var nombreBD = $(this).data("nombrebd");
        var nombreLog = $(this).data("nombrelog");

        var js2;
        js2 = $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Está seguro que desea limpiar el log de la base de datos ' + nombreBD + '?',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: "Confirmar",
            cancelButton: 'Aún No',
            content: "",
            confirm: function () {

                LimpiarLogsConsulta(nombreBD, nombreLog);

            },
            cancel: function () {

            },

        });
    })

});


function LimpiarLogsConsulta(nombreBD,nombreLog) {

    var salaId = $("#cboSala").find(':selected').data("id");

    if (!salaId) {
        toastr.error("Seleccione la sala.", "Mensaje Servidor");
        return;
    }

    if (salaId <= 0) {
        toastr.error("Seleccione la sala.", "Mensaje Servidor");
        return;
    }

    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "/Disco/LimpiarLogsConsulta",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ salaId,nombreBD, nombreLog }),
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {

            if (response.success) {
                toastr.success(response.message, "Mensaje Servidor");
                obtenerEspacioDiscosBD();
            } else {
                toastr.success(response.message, "Mensaje Servidor");
            }

            $.LoadingOverlay("hide");

        },
        error: function (request, status, error) {
            console.log(error);
        },
        complete: function (resul) {
        }
    });
}


function obtenerEspacioDiscosBD() {

    var salaId = $("#cboSala").find(':selected').data("id");

    if (!salaId) {
        toastr.error("Seleccione la sala.", "Mensaje Servidor");
        return;
    }

    if (salaId <= 0) {
        toastr.error("Seleccione la sala.", "Mensaje Servidor");
        return;
    }

    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "/Disco/ConsultaEspacioDiscoBDs", 
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ salaId }),
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if (response.success) {
                renderDiscoss(response.data)
                renderGrafico(response.data)
            }
            $.LoadingOverlay("hide");

        },
        error: function (request, status, error) {
            console.log(error);
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


function renderDiscoss(data) {
    var dataFinal = data

    objetodatatable = $("#tableDisco").DataTable({
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
                data: "NombreBD", title: "Nombre BD",
            },
            {
                data: "EspacioBD", title: "Espacio Usado BD",
            },
            {
                data: "NombreLog", title: "Nombre Log",
            },
            {
                data: "EspacioLog", title: "Espacio Usado Log BD",
                "render": function (o) {
                    var valorReal = parseInt(o);
                    var render =
                        valorReal >= 2000
                            ?
                            '<div class="emergencia">' + o + '</div>'
                            :
                            '<div class="normal">' + o + '</div>'
                    return render;
                }
            },
            {
                data: "FechaCreacion", title: "Fecha Creacion",
                "render": function (o) {

                    return moment(o).format("DD/MM/YYYY hh:mm a");
                }
            },
            {
                data: "Id", title: "Accion",
                "render": function (value, type, oData) {

                    var botones = '<button type="button" class="btn btn-xs btn-success btnLimpiarLog" data-nombrebd="' + oData.NombreBD + '" data-nombrelog="' + oData.NombreLog + '"><i class="glyphicon  glyphicon-retweet"></i> LIMPIAR LOG</button>';
                    return botones;
                }
            }
        ]
        ,
        "initComplete": function (settings, json) {

            $('#btnExcel').off("click").on('click', function () {

                cabecerasnuevas = [];
                cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });

                columna_cambio = [
                    {
                        nombre: "Espacio Usado Log BD",
                        render: function (o) {
                            return o;
                        }
                    }
                ]

                var ocultar = ["Accion"];//"Accion";
                funcionbotonesnuevo({
                    botonobjeto: this, ocultar: ocultar,
                    tablaobj: objetodatatable,
                    cabecerasnuevas: cabecerasnuevas,
                    columna_cambio: columna_cambio
                });
            });

        },
        
    });

}

const renderGrafico = (data) => {

    var arrayFake = data

    var arrayBDs = arrayFake.map(x => x.NombreBD);


    var options = {

        title: {
            text: 'Capacidad de BDs'
        },
        xAxis: {
            categories: arrayBDs,
            title: {
                text: 'BDs'
            },
            minPadding: 0.05,
            maxPadding: 0.05,
        },
        yAxis: {
            title: {
                text: 'MB'
            },
            tickInterval: 10

        },
        tooltip: {
            headerFormat: '<b>{series.name}</b><br>',
            pointFormat: 'Capacidad en uso: {point.y} MB'
        },
        series: [],
        chart: {
            type: "bar",
            zoomType: "xy",
            height: 700

        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
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

    for (var i = 0; i < arrayFake.length; i++) {
        var disco = arrayFake[i].NombreBD;
        if (!series[disco]) {
            series[disco] = [];
        }
        series[disco].push(
            {
                x: i,
                y: parseFloat(arrayFake[i].EspacioLog),
            }
        /*]*/);
    }

    for (var disco in series) {
        options.series.push({
            name: disco,
            data: series[disco],
            pointInterval: 24
        });
    }

    $(function () {
        $('#container').highcharts(options);

    });
}




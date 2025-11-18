
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
var arrayFake =[]
$(document).ready(function () {

   
    //obtenerDiscosUnique()
    $('#containerF').hide()

    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
    renderDiscoss([])
    //$("#cboSala").select2()
    obtenerSalasAsignadas().done(response => {
        if (response.data) {
            renderSalasAsignadas(response.data)
        }
    })
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

    /* *************/
    //clearTimeout(timerId)
    //timerId = setTimeout(function request() {
    //    getPingSalas().then(result => {
    //        urlsResultado = result
    //    })
    //    timerId = setTimeout(request, delay);
    //}, delay)


    //obtenerListaSalas().then(result => {
    //    if (result.data) {
    //        renderSelectSalas(result.data)
    //        getPingSalas().then(response => {
    //            urlsResultado = response
    //        })
    //    }
    //})

    /* *************/

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

        var rooms = $('#cboSalas').val()

        if (!rooms) {
            toastr.warning("Seleccione una o más salas")

            return false
        }
        console.log(rooms)
        //obtenerDiscosSala(idSala)
        obtenerDiscosSalas(rooms)

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
            url: basePath + "Disco/ReporteDiscosDescargarExcelJson",
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
        //const obj = urlsResultado.find(item => item.uri == ipPublica)
        //if (uri && obj.respuesta) {
        //    obtenerDiscosUnique(ipPublica)
        //}
        //          toastr.error("No se encontro informacion","Mensaje Servidor")
        //listadoprogresivos
        //else {
        //    ipPublicaAlterna = getUrl("http://190.187.44.222:9895")
        //    let urlPrivada = ipPrivada + '/servicio/listadoprogresivos'
        //    let urlPublica = ipPublicaAlterna + "/servicio/listadoprogresivosVpn"
        //    obtenerDiscosUniqueVpn(urlPrivada, urlPublica)

        //}



        obtenerDiscosUnique(ipPublica);

    })

    $(document).on('click', '#btnVerListado', function () {
        $('#containerF').hide()
        $('#container-listadoDisco').show()

    })

    $(document).on('click', '#btnVerGrafico', function (e) {
        $('#container-listadoDisco').hide()
        $('#containerF').show()
    })


});

/*CONSULTAR VPN */
function obtenerDiscosUniquevPN(urlPrivada, urlPublica) {
    return $.ajax({
        type: "POST",
        data: JSON.stringify({ urlPrivada: urlPrivada, urlPublica: urlPublica }),
        cache: false,
        //url: "http://localhost:9895/" + "servicio/DiscosActivos", /*${ip}/servicio/DiscosActivos*/
        url: basePath + "/Disco/ConsultaDiscosActivos", /*${ip}/servicio/DiscosActivos*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
        },
        success: function (response) {
            console.log(response.data)
            console.log('Nuevo disco')
            guardarDiscoNuevo(response.data)
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
        }
    });
}
/*CONSULTAR */
function obtenerDiscosUnique(ip) {
    $.LoadingOverlay("show");

    return $.ajax({
        type: "POST",
        cache: false,
        //url: "http://localhost:9895/" + "servicio/DiscosActivos", /*${ip}/servicio/DiscosActivos*/
        url: ip + "/servicio/DiscosActivosConsulta", /*${ip}/servicio/DiscosActivos*/
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
        },
        success: function (response) {
            console.log(response.data)
            console.log('Nuevo disco')
            guardarDiscoNuevo(response.data)
            $.LoadingOverlay("hide");

        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
        }
    });
}

function guardarDiscoNuevo(data) {
    console.log(data)
    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Disco/AgregarDiscosSala",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        beforeSend: function (xhr) {
        },
        success: function (response) {
            console.log(response)
            renderDiscoss(response.data)
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
        }
    });
}




function obtenerDiscosSalas(salas) {
    $.LoadingOverlay("show");
    var fechaIni = $("#fechaInicio").val();
    var fechaFin = $("#fechaFin").val();
    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Disco/ListadoDiscosSalas",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codSalas: salas, fechaIni, fechaFin }),
        beforeSend: function (xhr) {
        },
        success: function (response) {
            console.log("soy el post")
            console.log(response)
            renderDiscoss(response.data)
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


function obtenerDiscosSala(idSala) {
    $.LoadingOverlay("show");
    var fechaIni = $("#fechaInicio").val();
    var fechaFin = $("#fechaFin").val();
    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Disco/ListadoDisco",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codSala: idSala, fechaIni, fechaFin }),
        beforeSend: function (xhr) {
        },
        success: function (response) {
            console.log("soy el post")
            console.log(response)
            renderDiscoss(response.data)
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

function getUrl(url) {
    if (url) {
        try {
            let uri = new URL(url)
            return uri

        } catch (ex) {
            return false
        }
    }
    return false
}

function renderDiscoss(data) {
    var dataFinal = data
    //var RegA = $("#RegA").val();
    //var RegD = $("#RegD").val();
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
                data: "nombreSala", title: "Nombre Sala",

            },
            {
                data: "nombreDisco", title: "Nombre",

            },
            { data: "ipServidor", title: "IpServidor" },
            { data: "seudonimoDisco", title: "Seudonimo" },
            { data: "sistemaDisco", title: "Sistema disco" },
            { data: "tipoDisco", title: "tipo disco" },
            {
                data: "capacidadLibre", title: "Espacio libre",
                "render": function (o) {
                    var value = o.substring(0, o.length - 2);
                    var valorReal = parseInt(value);
                    var render =
                        valorReal <= 5
                            ?
                            '<div class="emergencia" >' + o + ' </div>'
                            :
                            '<div class="normal">' + o + ' </div>'
                    return render;
                }
            },
            { data: "capacidadEnUso", title: "Espacio en uso" },
            { data: "capacidadTotal", title: "Espacio total" },

            {
                data: "fechaRegistro", title: "Fecha Registro",
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
            text: 'Capacidad de discos'
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
                text: 'GB'
            },
            tickInterval: 10

        },
        tooltip: {
            headerFormat: '<b>{series.name}</b><br>',
            pointFormat: '{point.x:%d %b %Y}<br> Capacidad en uso: {point.y} GB<br> Capacidad total: {point.capacidadTotal} GB <br>Capacidad libre: {point.capacidadLibre} GB'
        },
        series: [],
        chart: {
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

    //var series = {};

    var salas = {};
    for (var i = 0; i < arrayFake.length; i++) {
        var sala = arrayFake[i].nombreSala;
        if (!salas[sala]) {
            salas[sala] = [];
        }
        salas[sala].push(arrayFake[i]);
    }

    var buttonsHtml = '';
    for (var sala in salas) {
        buttonsHtml += '<button class="salaButton" data-sala="' + sala + '">' + sala + '</button>';
    }
    $('#salaButtons').html(buttonsHtml);

    $('.salaButton').click(function () {
        var salaSeleccionada = $(this).data('sala');
        var dataSala = salas[salaSeleccionada];
        mostrarGrafico(dataSala);
    });
};

const mostrarGrafico = (data) => {
    var options = {

        title: {
            text: 'Capacidad de discos'
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
                text: 'GB'
            },
            tickInterval: 10

        },
        tooltip: {
            headerFormat: '<b>{series.name}</b><br>',
            pointFormat: '{point.x:%d %b %Y}<br> Capacidad en uso: {point.y} GB<br> Capacidad total: {point.capacidadTotal} GB <br>Capacidad libre: {point.capacidadLibre} GB'
        },
        series: [],
        chart: {
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

    for (var i = 0; i < data.length; i++) {
        var disco = data[i].nombreDisco;
        if (!series[disco]) {
            series[disco] = [];
        }
        var fechaRegistro = new Date(parseInt(data[i].fechaRegistro.substr(6)));
        var offsetMinutos = fechaRegistro.getTimezoneOffset();

        series[disco].push({
            x: fechaRegistro.getTime() - offsetMinutos * 60 * 1000,
            y: parseFloat(data[i].capacidadEnUso),
            capacidadLibre: parseFloat(data[i].capacidadLibre),
            capacidadTotal: parseFloat(data[i].capacidadTotal),
        });
    }

    for (var disco in series) {
        options.series.push({
            name: disco,
            data: series[disco],
            pointInterval: 24
        });
    }

    $('#container').highcharts(options);
};



//const mostrarGrafico = (data) => {pass 
//    var options = optionsRes

//    var series = {};

//    for (var i = 0; i < data.length; i++) {
//        var disco = data[i].nombreDisco;
//        if (!series[disco]) {
//            series[disco] = [];
//        }
//        var fechaRegistro = new Date(parseInt(data[i].fechaRegistro.substr(6)));
//        var offsetMinutos = fechaRegistro.getTimezoneOffset();

//        series[disco].push({
//            x: fechaRegistro.getTime() - offsetMinutos * 60 * 1000,
//            y: parseFloat(data[i].capacidadEnUso),
//            capacidadLibre: parseFloat(data[i].capacidadLibre),
//            capacidadTotal: parseFloat(data[i].capacidadTotal),
//        });
//    }

//    for (var disco in series) {
//        options.series.push({
//            name: disco,
//            data: series[disco],
//            pointInterval: 24
//        });
//    }

//    $('#container').highcharts(options);
//};


// obtener salas asignadas
function obtenerSalasAsignadas() {
    return $.ajax({
        type: "POST",
        url: `${basePath}/Sala/ListadoSalaPorUsuarioJson`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            return response
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}


// render salas asignadas
function renderSalasAsignadas(data) {
    var element = "#cboSalas"

    data.forEach(item => {
        $(element).append(`<option value="${item.CodSala}">${item.Nombre}</option>`)
    })

    jQuery(element).multiselect({
        multiple: true,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        maxHeight: 400,
        buttonContainer: '<div></div>',
        buttonClass: '',
        templates: {
            button: '<div class="form-control form-multiselect input-sm multiselect" data-toggle="dropdown"><span class="multiselect-selected-text"></span></div>'
        },
        nonSelectedText: '--Seleccione--',
        nSelectedText: 'seleccionados',
        allSelectedText: 'Todo seleccionado',
        selectAllText: 'Seleccionar todos'
    })
}

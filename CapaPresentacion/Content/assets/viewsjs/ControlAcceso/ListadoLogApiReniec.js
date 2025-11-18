$(document).ready(function () {
    /*
    $(".dateOnly_").datetimepicker({
        minViewMode: 1,
        pickTime: false,
        format: 'MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
    */
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });


    $("#rowSala").hide();
    $("#rowEstado").hide();

    $("#btnBuscar").on("click", function () {
        if ($("#fecha").val() == "") {
            toastr.error("Ingrese una fecha.");
            return false;
        }
        ListarLogApiReniecxFechas();

        $("#rowSala").show();
        $("#rowEstado").show();


    });

    $("#btnGraficos").on("click", function () {


        graficoUsuariosxSalaPrueba();
        //graficoUsuariosxSala();
    });

    $(document).on('change', '#cboEstado', function (e) {
        var estado = $('#cboEstado').val();
        var table = $('#resumenLogs').DataTable();
        table.search(estado).draw();
    })


    $(document).on('change', '#cboSala', function (e) {
        var sala = $('#cboSala').val();
        var table = $('#resumenLogs').DataTable();
        table.search(sala).draw();
    })

    $(document).on("click", "#btnExcel", function () {
        let fechaIni = $("#fechaInicio").val();
        let fechaFin = $("#fechaFin").val();
        let sala = $("#cboSala").val();
        if (sala == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        let listasala = $("#cboSala").val();
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "AsistenciaCliente/GetListadoCLientesSalaExcel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ArraySalaId: listasala, fechaIni, fechaFin }),
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



});

function ListarTiposLogApiReniec() {
    $.ajax({
        type: "POST",
        url: basePath + "CALLogApiReniec/ListarAllLogApiReniecJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                multiple: true, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboSala").val(null).trigger("change");
        },
        error: function (request, status, error) {
            //toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
    return false;
}

function ListarLogApiReniec() {
    var fecha = $("#fecha").val();
    var tipo = $("#cboTipo").val();
    var addtabla = $(".contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CALLogApiReniec/ListarAllLogApiReniecJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ channel: tipo, fecha }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {


            
            console.log(response.data)
            console.log(response.listaEstado)
            $("#cboEstado").html("");
            $("#cboEstado").append('<option value="">--Seleccione--</option>');
            if (response.listaEstado) {
                let lista = response.listaEstado;

                if (lista.length > 0) {
                    $.each(lista, function (index, value) {
                        $("#cboEstado").append('<option value="' + value + '"  >' + value + '</option>');
                    });
                }
            }
            else {
                console.log("error estado")
            }

            console.log(response.data)
            response = response.data;
            $(addtabla).empty();
            $(addtabla).append('<table id="resumenLogs" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#resumenLogs").DataTable({
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
                    { data: "FechaRegistro", title: "Fecha Registro" },
                    { data: "Sala", title: "Sala" },
                    { data: "Usuario", title: "Usuario" },
                    { data: "NroDoc", title: "Nro. documento" },
                    { data: "Estado", title: "Estado" },
                ],
                "initComplete": function (settings, json) {
                },
                "drawCallback": function (settings) {
                }
            });
            $('#ResumenSala tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });


        },
        error: function (request, status, error) {
            //toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}


function ListarLogApiReniecxFechas() {
    var fechaIni = $("#fechaIni").val();
    var fechaFin = $("#fechaFin").val();
    var tipo = $("#cboTipo").val();
    var addtabla = $(".contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CALLogApiReniec/ListarAllLogApiReniecFiltroFechasJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ channel: tipo, fechaIni, fechaFin }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {



            console.log(response.data)
            console.log(response.listaEstado)
            $("#cboEstado").html("");
            $("#cboEstado").append('<option value="">--Seleccione--</option>');
            if (response.listaEstado) {
                let lista = response.listaEstado;

                if (lista.length > 0) {
                    $.each(lista, function (index, value) {
                        $("#cboEstado").append('<option value="' + value + '"  >' + value + '</option>');
                    });
                }
            }
            else {
                console.log("error estado")
            }


            console.log(response.listaSala)
            $("#cboSala").html("");
            $("#cboSala").append('<option value="">--Seleccione--</option>');
            if (response.listaSala) {
                let lista = response.listaSala;

                if (lista.length > 0) {
                    $.each(lista, function (index, value) {
                        $("#cboSala").append('<option value="' + value + '"  >' + value + '</option>');
                    });
                }
            }
            else {
                console.log("error sala")
            }

            console.log(response.data)
            response = response.data;
            $(addtabla).empty();
            $(addtabla).append('<table id="resumenLogs" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#resumenLogs").DataTable({
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
                "aaSorting": [],
                data: response,
                columns: [
                    {
                        data: "FechaRegistro", title: "Fecha Registro",
                        "render": function (o) {
                            
                            return moment(o).format('DD-MM-YYYY HH:mm:ss');

                        } },
                    { data: "Sala", title: "Sala" },
                    { data: "Usuario", title: "Usuario" },
                    { data: "NroDoc", title: "Nro. documento" },
                    { data: "Estado", title: "Estado" },
                ],
                "initComplete": function (settings, json) {
                },
                "drawCallback": function (settings) {
                }
            });
            $('#ResumenSala tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });


        },
        error: function (request, status, error) {
            //toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}


function graficoUsuariosxSalaPrueba() {


    var fechaIni = $("#fechaIni").val();
    var fechaFin = $("#fechaFin").val();
    var tipo = $("#cboTipo").val();
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CALLogApiReniec/GraficoUsuariosxSalaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ channel: tipo, fechaIni, fechaFin }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {


            console.log(response.data);

            let jsonFinalString = '{"data":[{"nombreSala":"Excalibur","cantidad":20},{"nombreSala":"Gangas","cantidad":10},{"nombreSala":"Damasco","cantidad":30}]}';

            //response = response.data;
            graficoUsuariosxSala(JSON.stringify({ data:response}));

        },
        error: function (request, status, error) {
            //toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });

}

function graficoUsuariosxSala(jsonFinalString) {



    var jsonFinal = $.parseJSON(jsonFinalString);
    jsonFinal = jsonFinal.data;
    console.log(jsonFinal)

    const startYear = 1960,
        endYear = 2018,
        btn = document.getElementById('play-pause-button'),
        input = document.getElementById('play-range'),
        nbr = 20;

    let dataset, chart;




    function getData(year) {


        //while (chart.series.length)
        //    chart.series[0].remove();

        //console.log(dataset)

        //const output = Object.entries(dataset)
        //    .map(country => {
        //        const [countryName, countryData] = country;
        //        return [countryName, Number(countryData[year])];
        //    })
        //    .sort((a, b) => b[1] - a[1]);

        //console.log(output)

        console.log("JSON")
        console.log(jsonFinal)
        console.log(jsonFinal.data)
        console.log(jsonFinal.data.data)

        const outputa = Object.entries(jsonFinal.data)
            .map(sala => {
                const [salaName, countryData] = sala;
                return [jsonFinal.data[salaName].nombreSala, jsonFinal.data[salaName].cantidad];
            })
            .sort((a, b) => b[1] - a[1]);

        console.log(outputa)

        return [outputa[0], outputa.slice(0, nbr)];
    }

    function getSubtitle() {
        const population = (getData(input.value)[0][1] / 1000000000).toFixed(2);
        return `<span style="font-size: 80px">${input.value}</span>
        <br>
        <span style="font-size: 22px">
            Total: <b>: ${population}</b> billion
        </span>`;
    }

    (async () => {

        dataset = await fetch(
            'https://demo-live-data.highcharts.com/population.json'
        ).then(response => response.json());


        chart = Highcharts.chart('container', {
            chart: {
                animation: {
                    duration: 500
                },
                marginRight: 50
            },
            title: {
                text: 'Usuarios nuevos por Sala',
                align: 'left'
            },
            subtitle: {
                useHTML: true,
                text: "Usuarios registrados por sala",
                floating: true,
                align: 'right',
                verticalAlign: 'middle',
                y: -20,
                x: -100
            },

            legend: {
                enabled: false
            },
            xAxis: {
                type: 'category'
            },
            yAxis: {
                opposite: true,
                tickPixelInterval: 150,
                title: {
                    text: null
                }
            },
            plotOptions: {
                series: {
                    animation: false,
                    groupPadding: 0,
                    pointPadding: 0.1,
                    borderWidth: 0,
                    colorByPoint: true,
                    dataSorting: {
                        enabled: true,
                        matchByName: true
                    },
                    type: 'bar',
                    dataLabels: {
                        enabled: true
                    }
                }
            },
            series: [
                {
                    type: 'bar',
                    name: startYear,
                    data: getData(startYear)[1]
                }
            ],
            responsive: {
                rules: [{
                    condition: {
                        maxWidth: 550
                    },
                    chartOptions: {
                        xAxis: {
                            visible: false
                        },
                        subtitle: {
                            x: 0
                        },
                        plotOptions: {
                            series: {
                                dataLabels: [{
                                    enabled: true,
                                    y: 8
                                }, {
                                    enabled: true,
                                    format: '{point.name}',
                                    y: -8,
                                    style: {
                                        fontWeight: 'normal',
                                        opacity: 0.7
                                    }
                                }]
                            }
                        }
                    }
                }]
            }
        });
    })();


}

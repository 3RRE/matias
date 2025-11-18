
$(document).ready(function () {
    ObtenerLista();

    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
    $("#btnBuscar").on("click", function () {
        if ($("#cboSala").val() == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if ($("#fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if ($("#fechaFin").val() == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }
        buscarListadoAsistencia();
    });

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
            url: basePath + "ReporteAsistenciaEmpleado/AsistenciaDescargarExcelJson",
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
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });
    datosG = [];

    $("#btn_gps").on('click', function () {
        $("#seccion-mapa_puntos").html("");
        var fechaini = $("#fechaInicio").val();
        var fechafin = $("#fechaFin").val();
        let sala = $("#cboSala").val();
        if (sala == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }

        if (fechaini == "") {
            console.log(fechaini,"esto")
            toastr.error("Complete los campos Obligatorios(Fechas)", "Mensaje Servidor");
           
            return false;
        }

        if (fechafin == "") {
            console.log(fechaini, "esto")
            toastr.error("Complete los campos Obligatorios(Fechas)", "Mensaje Servidor");

            return false;
        }
        if (datosG.length != 0) {
            $("#modalmapapuntos").modal("show");
        }
        else {
            toastr.error("Complete los campos Obligatorios", "Mensaje Servidor");
        }
    });

    $('#modalmapapuntos').on('shown.bs.modal', function (e) {
        var fechaini = $("#fecha_ini").val();
        var fechafin = $("#fecha_fin").val();
        var puntos_gps = [];
        if (fechaini == "" || fechafin == "") {
            toastr.error("Complete los campos Obligatorios(Fechas)", "Mensaje Servidor");
            $("#modalmapapuntos").modal("hide");
            return false;
        }

        if (datosG.length == 0) {
            toastr.error("No se encontraron Registros", "Mensaje Servidor");
            $("#modalmapapuntos").modal("hide");
            return false;
        }

        $("#seccion-mapa_puntos").append('<div id="divMapa_puntos" style="height:400px;width:100%;border:4px solid #808080"></div>');

        var asistenciadata = datosG;
        $(asistenciadata).each(function (ind, opt) {
            var empleado = opt.emp_nombre + ' ' + opt.emp_ape_paterno + ' ' + opt.emp_ape_materno;
            $("#seccion-mapa_puntos").append('<div id="marker' + opt.ema_id + '" title="Marker' + opt.ema_id + '"><img src="' + basePath + 'Content/assets/images/finder.png" /></div>');
            $("#seccion-mapa_puntos").append('<div class="overlay" id="tittle' + opt.ema_id + '"><span class="badge badge-soft-warning">' + empleado + '</span></div>');
            puntos_gps.push({ latitud: opt.ema_latitud, longitud: opt.ema_longitud, ema_id: opt.ema_id })
        });

        $("#divMapa_puntos").html("");
        var map_ = '';
        if (puntos_gps.length > 0) {

            map_ = new ol.Map({
                target: 'divMapa_puntos',
                layers: [
                    new ol.layer.Tile({
                        source: new ol.source.OSM()
                    })
                ],
                view: new ol.View({
                    projection: "EPSG:4326",
                    //center: [parseFloat(response.data[0].loc_longitud), parseFloat(response.data[0].loc_latitud)],
                    center: [parseFloat(puntos_gps[0].longitud), parseFloat(puntos_gps[0].latitud)],
                    zoom: 16,
                    minzoom: 1,
                    maxzoom: 18
                })
            });

            $(puntos_gps).each(function (ind, opt) {
                var marker2 = new ol.Overlay({
                    scale: 0.7,
                    position: [parseFloat(opt.longitud), parseFloat(opt.latitud)],
                    positioning: 'center-center',
                    element: document.getElementById('marker' + opt.ema_id),
                    stopEvent: false
                });
                map_.addOverlay(marker2);
                var tittle = new ol.Overlay({
                    position: [parseFloat(opt.longitud), parseFloat(opt.latitud)],
                    element: document.getElementById('tittle' + opt.ema_id)
                });
                map_.addOverlay(tittle);
            });
        };
    });

    $(document).on('click', ".btn-mapa", function () {
        var latitud = $(this).data("latitud");
        var longitud = $(this).data("longitud");
        var empleado = $(this).data("empleado");
        $("#latitud").val(latitud);
        $("#longitud").val(longitud);
        $("#empleado").val(empleado);
        $("#modalmapa").modal("show");
    });

    $('#modalmapa').on('shown.bs.modal', function (e) {
        var latitud = $("#latitud").val();
        var longitud = $("#longitud").val();
        var empleado = $("#empleado").val();
        $("#seccion-mapa").append('<div id="marker" title="Marker"><img src="' + basePath + 'Content/assets/images/finder.png" /></div>');
        $("#seccion-mapa").append('<div class="overlay" id="tittle"><span class="badge badge-soft-warning">' + empleado + '</span></div>');
        var puntos = [];
        puntos.push({ latitud: latitud, longitud: longitud, empleado: empleado });
   
        mapa(puntos);
    });
});

function ObtenerLista() {
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
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                multiple: true, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboSala").val(null).trigger("change");
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

function buscarListadoAsistencia() {
    var listasala = $("#cboSala").val();
    var fechaIni = $("#fechaInicio").val();
    var fechaFin = $("#fechaFin").val();
    var addtabla = $(".contenedor_tabla");
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "ReporteAsistenciaEmpleado/GetListadoAsistenciaSalaFiltros",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ArraySalaId: listasala, fechaIni, fechaFin }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            console.log(response.data)
            datosG = response.data;
            response = response.data;
            let columnas = ColumnasDatatable();
            $(addtabla).empty();
            $(addtabla).append('<table id="ResumenSala" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#ResumenSala").DataTable({
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
                columns: columnas,
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
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
}

function ColumnasDatatable(permiso) {
    let obj = [
        {
            data: "emp_id",
            title: "ID",
            width: "60px",
            className: "text-center",
        },
        {
            data: "emp_nombre",
            title: "Nombre y Apellidos",
            className: 'align-left',
            "render": function (value, df, opt) {
                var span = opt.emp_nombre + ' ' + opt.emp_ape_paterno + ' ' + opt.emp_ape_materno;
                return span;
            }
        },

        {
            data: "local",
            title: "Local",
        },
        {
            data: "ema_fecha",
            title: "Fecha",
            "render": function (value) {
                return moment(value).format("DD-MM-YYYY");
            }
        },
        {
            data: "ema_fecha",
            title: "Hora",
            "render": function (value) {
                return moment(value).format("hh:mm:ss A");
            }
        },
        {
            data: "ema_asignado",
            title: "Estado",
            className: 'reorder',
            "render": function (value) {
                var estado = value;
                var mensaje_estado = "";
                if (estado === 1) {
                    estado = "success";
                    mensaje_estado = "Asignado";
                } else {
                    estado = "warning";
                    mensaje_estado = "No Asignado";
                }
                var span = '<span class="badge badge-soft-' + estado + ' ">' + mensaje_estado + '</span>';
                return span;
            }
        },
        {
            data: "emp_id",
            title: '<i class="fa fa-map-marker"></i>',
            "bSortable": false,
            "render": function (value, type, oData) {
                var span = '';
                var emp_id = value;
                var span = '<button data-toggle="tooltip" data-placement="top" data-original-title="UBICACION" data-skin-class="tooltip-base" class="btn btn-soft-info btn-sm btn-mapa" data-emaid="' + oData.ema_id + '" data-id="' + emp_id + '" data-latitud="' + oData.ema_latitud + '" data-longitud="' + oData.ema_longitud + '" data-empleado="' + oData.emp_nombre + ' ' + oData.emp_ape_paterno + '"><i class="fa fa-map-marker"></i></button>';
                return span;
            }
        }
    ]
    return obj
}


function mapa(puntos) {
    var map = '';
    $("#divMapa").html("");

    if (puntos.length > 0) {
        console.log(puntos[0])
        map = new ol.Map({
            target: 'divMapa',
            layers: [
                new ol.layer.Tile({
                    source: new ol.source.OSM()
                })
            ],
            view: new ol.View({
                projection: "EPSG:4326",
                //center: [parseFloat(response.data[0].loc_longitud), parseFloat(response.data[0].loc_latitud)],
                center: [parseFloat(puntos[0].longitud), parseFloat(puntos[0].latitud)],
                zoom: 16,
                minzoom: 1,
                maxzoom: 18
            })
        });
        var washingtonWebMercator;
        washingtonWebMercator = [parseFloat(puntos[0].longitud), parseFloat(puntos[0].latitud)];

        var marker2 = new ol.Overlay({
            scale: 0.7,
            position: washingtonWebMercator,
            positioning: 'center-center',
            element: document.getElementById('marker'),
            stopEvent: false
        });
        map.addOverlay(marker2);
        var tittle = new ol.Overlay({
            position: washingtonWebMercator,
            element: document.getElementById('tittle')
        });
        map.addOverlay(tittle);
    };

};
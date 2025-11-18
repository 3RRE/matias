
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

  
    datosG = [];

    $(document).on('click', ".btn-detalle", function () {
        $("#rep_fecha").val(moment($(this).data('fecha')).format("DD-MM-YYYY") + "  " + moment($(this).data('fecha')).format("hh:mm:ss A"));
        $("#rep_descripcion").val($(this).data('descripcion'));
        $("#rep_titulo").val($(this).data('titulo'));
        $("#mod_nombreVisita").html($(this).data('empleado'));
        $("#rep_id").val($(this).data('id'));
        $("#divContentImages").html("");
        $("#spn_nroimagen").html("");
        $('#modalreportedetalleVisita').modal("show");
    })

    $('#modalreportedetalleVisita').on('shown.bs.modal', function (e) {
        var rep_id = $("#rep_id").val();
        if (rep_id == 0) {
            return false;
        }
        var dataForm = {
            vis_id: rep_id
        };

        $.ajax({
            type: "POST",
            url: basePath + "ReporteVisita/GetReporteVisitaDetalleJson",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                console.log(response)
                var datos = response.data;
                $("#divContentImages").html("");
                $.each(datos, function (index, value) {
                    $("#divContentImages").append('<div class="col-md-4 col-xs-4">' +
                        '<div data-id="' + value.visd_id + '" class="img_reporte imgdiv' + value.visd_id + '" style ="cursor:pointer">' +
                        '<img class="imgfoto imgFotodetalle' + value.visd_id + ' btn-badge_reportedetalle" data-index="' + (index + 1) + '" data-descripcion="' + value.descripcion + '" src="' + basePath + 'Content/assets/images/logo-white-small.png" style="width:100%;height:100%"/>' +
                        '<span class="badge badge-soft-success" style="margin-top:5px;"> Foto ' + (index + 1) + '</span> <span class="badge badge-soft-warning" style="margin-top:5px;">' + value.nombre + '</span>' +

                        '</div>' +
                        '</div>');

                });

                setTimeout(function () {
                    $.each($(".img_reporte"), function (index) {
                        var id = $(this).data("id");
                        init_MostrarImagen(id);
                    });

                }, 1000);
            },
            error: function (request, status, error) {
                toastr.error("Error", "Mensaje Servidor");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
       

    });

    $(document).on('click', ".btn-badge_reportedetalle", function () {
        $("#rep_descripcionImagen").val($(this).data('descripcion'));
        $("#spn_nroimagen").html($(this).data('index'));
    });

    $(document).on("click", "#btnExcel", function () {
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
        var listasala = $("#cboSala").val();
        var fechaini = $("#fechaInicio").val();
        var fechafin = $("#fechaFin").val();
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "ReporteVisita/VisitaDescargarExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ArraySalaId: listasala, fechaini, fechafin }),
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
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "ReporteVisita/GetReporteVisitaJson",
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
            $.LoadingOverlay("hide");
        }
    });
}

function ColumnasDatatable(permiso) {
    let obj = [
        {
            data: "vis_id",
            title: "ID",
            width: "60px",
            className: "text-center",
        },
        {
            data: "empleado_id",
            title: "Emp. ID",
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
            data: "titulo",
            title: "Titulo",
            "bSortable": true,
        },
        {
            data: "fechaRegistro",
            title: "Fecha",
            "bSortable": false,
            "render": function (value) {
                return moment(value).format("DD-MM-YYYY");
            }
        },
        {
            data: "fechaRegistro",
            title: "Hora",
            "bSortable": false,
            "render": function (value) {
                return moment(value).format("hh:mm:ss A");
            }
        },
        {
            data: "imei",
            title: "IMEI",
            "bSortable": false,
            className: 'reorder',
            "render": function (value) {
                var imei = value;
                var span = '<span class="badge badge-soft-warning">' + imei + '</span>';
                return span;
            }
        },
        {
            data: "vis_id",
            title: "Acciones",
            "bSortable": false,
            "render": function (value, type, oData) {
                var span = '';
                var nombre = oData.emp_nombre + ' ' + oData.emp_ape_paterno + ' ' + oData.emp_ape_materno;
                var span = '<button data-toggle="tooltip" data-placement="top" data-original-title="Detalle"  class="btn  btn-sm btn-detalle" data-id="' + value + '" data-fecha="' + oData.fechaRegistro + '" data-descripcion="' + oData.descripcion + '" data-titulo="' + oData.titulo + '" data-empleado="' + nombre + '"><i class="fa fa-file"></i> DETALLE</button>';
                return span;
            }
        }
    ]
    return obj
}

function init_MostrarImagen(id) {
    $.ajax({
        type: "POST",
        url: basePath + "ReporteVisita/GetReporteVisitaDetalleIdJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ visd_id:id}),
        beforeSend: function (xhr) {
            //$.LoadingOverlay("show");
        },
        success: function (response) {
            var imagen = response.data;
            if (imagen != null) {
                $('.imgFotodetalle' + id).attr('src', "data: image/png;base64," + imagen + "");
            }
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            //$.LoadingOverlay("hide");
        }
    });
    return false;
}

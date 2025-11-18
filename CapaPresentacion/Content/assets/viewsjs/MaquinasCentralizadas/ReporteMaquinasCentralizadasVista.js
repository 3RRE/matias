
VistaAuditoria("Maquina/ReporteMaquinaVista", "VISTA", 0, "", 1);

var arrayAuxi = [];

$(document).ready(function () {

    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });

    GetSalas();

    $(document).on('click', '#btnBuscar', function (e) {
        ListadoMaquinasCentralizadas();
    });

    $(document).on('click', '#btnExcel', function (e) {
        GenerarExcelMaquinasCentralizadas();
    });

    $(document).on('change', '#cboSala', function (e) {
        arrayAuxi = [];
        renderDataTable([]); 
    });
});


function GetSalas() {


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
                placeholder: "--Seleccione--"

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


function ListadoMaquinasCentralizadas() {

    var codSala = $("#cboSala").val();
    var fechaIni = $("#fechaIni").val();
    var fechaFin = $("#fechaFin").val();

    if (codSala  == null) {
        toastr.error("Selecione una sala.", "Mensaje Sistema");
        return false;
    }

    var parametros = JSON.stringify({ codSala, fechaIni, fechaFin });

    ajaxhr = $.ajax({
        type: "post",
        cache: false,
        url: basePath + "MaquinasCentralizadas/ListadoMaquinasCentralizadasJson",
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.respuesta) {
                renderDataTable(result.data)
                arrayAuxi = result.data;
                toastr.success(result.mensaje, "Mensaje Sistema");
            } else {
                toastr.error(result.mensaje, "Mensaje Sistema");
            }
        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error, "Mensaje Sistema");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()

}

function renderDataTable(array) {

    objetodatatable= $("#table").DataTable({
        "bDestroy": true,
        "ordering": true,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": true,
        "aaSorting": [],
        "autoWidth": true,
        "bAutoWidth": true,
        "bProcessing": true,
        "bDeferRender": true,
        "searching": true,
        "bInfo": true, //Dont display info e.g. "Showing 1 to 4 of 4 entries"      
        "bPaginate": true,//Dont want paging      
        data: array
        ,
        columns: [
            { data: "sala", title: "Sala" },
            { data: "tipo_maquina", title: "tipo maquina" },
            { data: "codigo", title: "codigo" },
            { data: "serie", title: "serie" },
            { data: "zona", title: "zona" },
            { data: "isla", title: "isla" },
            { data: "posicion", title: "posicion" },
            { data: "marca", title: "marca  " },
            { data: "modelo_comercial", title: "modelo comercial" },
            { data: "codigo_modelo", title: "codigo modelo" },
            { data: "juego", title: "juego" },
            { data: "progresivo", title: "progresivo" },
            { data: "propietario", title: "propietario" },
            { data: "tipo_contrato", title: "tipo contrato" },
            { data: "moneda", title: "moneda" },
            { data: "medio_juego", title: "medio_juego" },
            {
                data: "token", title: "token",
                "render": function (o) {
                    return o.toFixed(2);
                },
            },
            { data: "dias_trabajados", title: "dias trabajados" },
            {
                data: "coin_in", title: "coin in",
                "render": function (o) {
                    return o.toFixed(2);
                },
            },
            {
                data: "a_coin_in", title: "a coin in",
                "render": function (o) {
                    return o.toFixed(2);
                },
            },
            {
                data: "win", title: "win",
                "render": function (o) {
                    return o.toFixed(2);
                },
            },
            {
                data: "media", title: "media",
                "render": function (o) {
                    return o.toFixed(2);
                },
            },
            {
                data: "hold", title: "hold",
                "render": function (o) {
                    return o.toFixed(2);
                },
            },
            {
                data: "avbet_x_game", title: "avbet x game",
                "render": function (o) {
                    return o.toFixed(2);
                },
            },
            {
                data: "games_played", title: "games played",
                "render": function (o) {
                    return o.toFixed(0);
                },
            },
            { data: "tiempo_juego", title: "tiempo juego" },
            { data: "mistery", title: "mistery" },
            { data: "tipo_progresivo", title: "tipo progresivo" },
            { data: "pozos", title: "pozos" },
            {
                data: "porcentaje_teorico", title: "porcentaje devolucion",
                "render": function (o) {
                    return o.toFixed(2)+'%';
                },
            },
            {
                data: "rtp_retorno_teorico", title: "retorno teorico",
                "render": function (o) {
                    return o + '%';
                },
            },
            {
                data: "incremento_progresivo", title: "incremento progresivo",
                "render": function (o) {
                    return o + '%';
                },
            },
            {
                data: "estado", title: "Estado",
                "render": function (o) {

                    var estado = "INACTIVO";
                    var css = "btn-danger";

                    if (o) {
                        estado = "ACTIVO";
                        css = "btn-info";
                    }

                    return '<span class="label ' + css + '" style="width:100%; display:block;padding:5px 0">' + estado + '</span>';

                }
            }
        ],
        "drawCallback": function (settings) {
        },

        "initComplete": function (settings, json) {

        },
    });
}

function GenerarExcelMaquinasCentralizadas() {

    if ($("#cboSala").val() == null) {
        toastr.error("Selecione una sala.", "Mensaje Sistema");
        return false;
    }

    if (arrayAuxi.length === 0) {
        toastr.error("No hay informacion para generar el reporte", "Mensaje Sistema");
        return false;
    }


    var parametros = JSON.stringify({ lista: arrayAuxi });
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "MaquinasCentralizadas/ListadoMaquinasCentralizadasExcelJson",
        contentType: "application/json; charset=utf-8",
        data: parametros,
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if (response.respuesta) {
                var data = response.data;
                var file = response.excelName;
                let a = document.createElement('a');
                a.target = '_self';
                a.href = "data:application/vnd.ms-excel;base64, " + data;
                a.download = file;
                a.click();
            }
            else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
            AbortRequest.close()
        }
    });
    AbortRequest.open()
}

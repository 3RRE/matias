var arrayProblemas = [];

$(document).ready(function () {

    LoadClasificacionProblemas();

    $("#cboClasificacionProblemas").change(function () {

        LoadDataTableReporteCategoriaProblemas([]);

        //if ($("#cboListaProblemas").val()) {
        //    arrayProblemas = $("#cboListaProblemas").val();
        //}

        //let cod = $(this).val();
        //if (cod) {
        //    $("#cboListaProblemas").empty();
        //    LoadListaProblemas(cod);
        //}
    });

    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });

    $(document).on("click", "#btnBuscar", function () {

        if ($("#fechaIni").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if ($("#fechaFin").val() == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }

        fechaIni = $("#fechaIni").val();
        fechaFin = $("#fechaFin").val();
        arrayProblemas = $("#cboClasificacionProblemas").val();

        ListarReporteCategoriaProblemasxFechas(fechaIni, fechaFin,arrayProblemas);

    });

    $(document).on("click", "#btnExcel", function () {

        if ($("#fechaIni").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if ($("#fechaFin").val() == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }

        fechaIni = $("#fechaIni").val();
        fechaFin = $("#fechaFin").val();
        listaCategoriaProblema = $("#cboClasificacionProblemas").val();

        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "MIMaquinaInoperativa/ReporteCategoriaProblemasExcelJsonxFechas",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ fechaIni, fechaFin, listaCategoriaProblema }),
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
            }
        });


    });


});

function LoadClasificacionProblemas() {

    $.ajax({
        type: "POST",
        url: basePath + "MICategoriaProblema/ListarCategoriaProblemaActiveJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboClasificacionProblemas").append('<option value="' + value.CodCategoriaProblema + '"  >' + value.Nombre + '</option>');
            });
            $("#cboClasificacionProblemas").select2({
                multiple: true, placeholder: "--Todos--", allowClear: true

            });
            $("#cboClasificacionProblemas").val(null).trigger("change");
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
            //LimpiarFormValidator();
        }
    });
    return false;
}

//function LoadListaProblemas(lista) {


//    $.ajax({
//        type: "POST",
//        url: basePath + "MIProblema/ListarProblemaxCategoriaListaJson",
//        cache: false,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        data: JSON.stringify({ listaCategoriaProblema: lista }),
//        beforeSend: function (xhr) {
//            $.LoadingOverlay("show");
//        },
//        success: function (result) {
//            var datos = result.data;

//            $.each(datos, function (index, value) {
//                $("#cboListaProblemas").append('<option value="' + value.CodProblema + '"  >' + value.Nombre + '</option>');
//                arrayProblemas.push(value.CodProblema);
//            });
//            $("#cboListaProblemas").select2({
//                multiple: true, placeholder: "--Seleccione--", allowClear: true
//            });
//            $("#cboListaProblemas").val(null).trigger("change");
//        },
//        error: function (request, status, error) {
//            toastr.error("Error", "Mensaje Servidor");
//        },
//        complete: function (resul) {
//            $.LoadingOverlay("hide");

//            $("#cboListaProblemas").val(arrayProblemas);
//            $("#cboListaProblemas").change();
//            //LimpiarFormValidator();
//        }
//    });
//    return false;
//}

function LoadDataTableReporteCategoriaProblemas(data) {
    objetodatatable = $("#tableReporteCategoriaProblemas").DataTable({

        "fixedColumns": {
            "leftColumns": 3
        },
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
        data: data,
        columns: [
            { data: "NombreCategoriaProblema", title: "Nombre Categoria Problema" },
            { data: "NombreProblema", title: "Nombre Problema" },
            { data: "NombreSala", title: "Sala" },
            { data: "MaquinaLey", title: "Maquina" },
            { data: "MaquinaModelo", title: "Modelo" },
            { data: "MaquinaLinea", title: "Linea" },
            { data: "MaquinaJuego", title: "Juego" },
            { data: "MaquinaNumeroSerie", title: "Numero Serie" },
            { data: "MaquinaPropietario", title: "Propietario" },
            { data: "MaquinaFicha", title: "Ficha" },
            { data: "MaquinaMarca", title: "Marca" },
            { data: "MaquinaToken", title: "Token" },   
            {
                data: "FechaInoperativa", title: "Fecha Reportado",
                "render": function (o) {
                    return moment(o).format("DD/MM/YYYY hh:mm:ss");
                },
            }, 
            {
                data: "CodEstadoProceso", title: "Fecha Resuelto",
                "render": function (o,type,oData) {

                    if (o == 2) {
                        if (oData.CodEstadoReparacion == 1) {
                            return moment(oData.FechaAtendidaInoperativaAprobado).format("DD/MM/YYYY hh:mm:ss");
                        } else {
                            return moment(oData.FechaAtendidaOperativa).format("DD/MM/YYYY HH:mm:ss");
                        }
                    } else {
                        return "No resuelto";
                    }


                },
            },
            //{
            //    data: "NombreUsuarioCreado", title: "Usuario",
            //    visible: false
            //},
            {
                data: "CodEstadoProceso", title: "Estado",
                "render": function (o) {
                    var estado = "CREADO";
                    var css = "btn-info";
                    if (o == 3) {
                        estado = "ATENDIDA INOPERATIVA"
                        css = "btn-danger";
                    }
                    else if (o == 2) {
                        estado = "ATENDIDA OPERATIVA"
                        css = "btn-primary";
                    }
                    else if (o == 4) {
                        estado = "EN ESPERA SOLICITUD"
                        css = "btn-warning";
                    }
                    else if (o == 5) {
                        estado = "REPUESTOS AGREGADOS"
                        css = "btn-primary";
                    }
                    return '<span class="label ' + css + '" style="width:100%; display:block;padding:5px 0">' + estado + '</span>';

                }
            },
            //{
            //    data: "CodMaquinaInoperativa", title: "Acción",
            //    "bSortable": false,
            //    "render": function (o, type, oData) {

            //        return `<button type="button" class="btn btn-xs btn-warning btnDetalle" data-id="${o}"> <span class="glyphicon glyphicon-list-alt" style="margin-right: 5px;"></span>VER HISTORICO</i></button>`;
            //    },
            //}
        ],
        "drawCallback": function (settings) {
            $('.btnDetalle').tooltip({
                title: "Historico"
            });
        },

        "initComplete": function (settings, json) {



        },
    });
}


function ListarReporteCategoriaProblemasxFechas(fechaIni, fechaFin, listaCategoriaProblema) {

    $.ajax({
        type: "POST",
        url: basePath + "MIMaquinaInoperativa/ReporteCategoriaProblemasListaJsonxFechas",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ fechaIni, fechaFin, listaCategoriaProblema }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.respuesta) {
                LoadDataTableReporteCategoriaProblemas(result.data);
            }
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
            //LimpiarFormValidator();
        }
    });
    return false;
}
let objetodatatable
$(document).ready(function () {
    ObtenerListaSalas();
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
    });

    $(".dateOnly_fechaini").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
    });

    $(".dateOnly_fechafin").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
    });

    setCookie("datainicial", "");
    $("#btnBuscar").on("click", function () {
        if ($("#fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if ($("#fechaFin").val() == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }
        if ($("#cboSala").val() == null) {
            toastr.error("Seleccione Sala.", "Mensaje Servidor");
            return false;
        }
        buscarFichasSintomatologicas();
    });
    ;


    $(document).on('click', "#btnPdf", function (e) {
        var doc = $(this).data("doc");
        let a = document.createElement('a');
        a.target = '_self';
        a.href = basePath + "FichaSintomatologica/FichaSintomatologicaRespuestaPDFDescarga?doc=" + doc + "&todo=true";
        a.click();
    });

    $(document).on('click', '#btnDescargaMultiple', function (e) {
        var listasala = $("#cboSala").val();
        var fechaini = $("#fechaInicio").val();
        var fechafin = $("#fechaFin").val();

        if (listasala == null) {

            toastr.error("Seleccione Sala.", "Mensaje Servidor");
            return false;
        } else {

        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "FichaSintomatologica/GenerarFisicoFichaSaludPdfJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                if (response.respuesta) {
                    let data = response.data;
                    let file = response.filename;
                    let a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/pdf;base64, " + data;
                    a.download = file;
                    a.click();
                }
            },
            error: function (request, status, error) {
               
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
            }
    })

    $(document).on("click", "#btnExcel", function () {
        var fechaini = $("#fechaInicio").val();
        var fechafin = $("#fechaFin").val();
        var listasala = $("#cboSala").val();

        if (listasala == null) {

            toastr.error("Seleccione Sala.", "Mensaje Servidor");
            return false;
        } else {

            $.ajax({
                type: "POST",
                cache: false,
                url: basePath + "FichaSintomatologica/ReporteFichaSintomatologicaDescargarExcelJson",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (response) {
                    if (response.respuesta) {
                        dataAuditoria(1, "#formfiltro", 3, "FichaSintomatologica/ReporteFichaSintomatologicaDescargarExcelJson", "BOTON EXCEL");
                        var data = response.data;
                        var file = response.excelName;
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
        }


    });

    VistaAuditoria("FichaSintomatologica/FichaReporteVista", "VISTA", 0, "", 3);


});

function ObtenerListaSalas() {
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

function buscarFichasSintomatologicas() {
    var listasala = $("#cboSala").val();


    var fechaini = $("#fechaInicio").val();
    var fechafin = $("#fechaFin").val();
    var addtabla = $(".contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "FichaSintomatologica/FichaSintomatologicaListarxSalaFechaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            console.log(response)
            response = response.data;

            dataAuditoria(1, "#formfiltro", 3, "FichaSintomatologica/FichaSintomatologicaListarxSalaFechaJson", "BOTON BUSCAR");
            $(addtabla).empty();
            $(addtabla).append('<table id="tablefichas" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#tablefichas").DataTable({
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

                    { data: "FichaId", title: "ID" },
                    { data: "Fecha", title: "Fecha" },
                    { data: "Empresa", title: "Empresa" },
                    { data: "Sala", title: "Sala" },
                    { data: "DOI", title: "Nro. Doc." },
                    {
                        data: "Nombre", title: "Nombres", "render": function (value, x, y) {
                            return y.Nombre + " " + y.ApellidoPaterno + " " + y.ApellidoMaterno;
                        }
                    },
                    { data: "Area", title: "Area de Trabajo" },

                    {
                        data: null, title: "Accion",
                        "render": function (value) {
                            var estado = value.Estado;
                            var butom = ` <button type="button" class="btn btn-xs btn-warning btnDetalles" data-json='${JSON.stringify(value)}' data-id="${value.FichaId}" data-hash="${value.hash}"><i class="fa fa-file-pdf-o"></i> VER</button>`;
                            return butom;
                        }
                    }
                ]
                ,
                "initComplete": function (settings, json) {



                },
                "drawCallback": function (settings) {

                }
            });

            $('#tablefichas tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

$(document).on("click", ".btnDetalles", function () {
    var id = $(this).data("id");
    $("#fichaId").val(id);
    ObtenerRegistro(id);
    $("#full-modalDetalleFicha").modal("show");
});

function ObtenerRegistro(id) {
    let div = $("#divAdjuntos")
    div.html("")
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "FichaSintomatologica/FichaSintomatologicaIdObtenerJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ id: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            if (response.respuesta) {
                var imagen = response.imagen;
                let dataSala = response.dataSala;
                let dataEmpresa = response.dataEmpresa;
                response = response.data;
                
                if (dataSala.RutaArchivoLogo) {
                    $(".imgreclamacion").attr("src", "https://drive.google.com/uc?id=" + dataSala.RutaArchivoLogo)
                }
                else {
                    $(".imgreclamacion").attr("src", basePath + "Content/assets/images/no_image.jpg")
                }
                $("#tspnRazonSocial").text(dataEmpresa.RazonSocial.toUpperCase());
                $("#tspnRUC").text(dataEmpresa.Ruc.toUpperCase());
                $("#tspnDireccionEmpresa").text(dataEmpresa.Direccion);



                $("#spnFecha").text(response.Fecha);
                $("#spnEmpresa").text(response.Empresa.toUpperCase());
                $("#spnRuc").text(response.RUC.toUpperCase());
                $("#spnNombre").text(response.Nombre.toUpperCase());
                $("#spnApellidoPaterno").text(response.ApellidoPaterno.toUpperCase());
                $("#spnApellidoMaterno").text(response.ApellidoMaterno.toUpperCase());
                $("#spnDOI").text(response.DOI.toUpperCase());
                $("#spnDireccion").text(response.Direccion.toUpperCase());
                $("#spnCelular").text(response.Celular.toUpperCase());
                $("#spnArea").text(response.Area);
                $("#spnTemperaturaIngreso").text(response.TemperaturaIngreso);
                $("#spnTemperaturaSalida").text(response.TemperaturaSalida);
                $('#signo1Ingreso').prop('checked', response.Signo1Ingreso);
                $('#signo2Ingreso').prop('checked', response.Signo2Ingreso);
                $('#signo3Ingreso').prop('checked', response.Signo3Ingreso);
                $('#signo4Ingreso').prop('checked', response.Signo4Ingreso);
                $('#signo5Ingreso').prop('checked', response.Signo5Ingreso);
                $('#signo6Ingreso').prop('checked', response.Signo6Ingreso);
                $('#signo1Salida').prop('checked', response.Signo1Salida);
                $('#signo2Salida').prop('checked', response.Signo2Salida);
                $('#signo3Salida').prop('checked', response.Signo3Salida);
                $('#signo4Salida').prop('checked', response.Signo4Salida);
                $('#signo5Salida').prop('checked', response.Signo5Salida);
                $('#signo6Salida').prop('checked', response.Signo6Salida);
                $('#terminos').prop('checked', true);
                $('#btnPdf').data('doc', id);
                var canvas = document.getElementById("canvas");
                if (response.Firma) {
                    $("#canvas").attr("src", "data:image/png;base64," + response.Firma)
                }
                else {
                    $("#canvas").attr("src", basePath + "Content/assets/images/no_image.jpg")
                }

            }
            else {
                toast.error(response.mensaje, "Mensaje Servidor")
                $("#full-modalDetalleFicha").modal("hide")
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
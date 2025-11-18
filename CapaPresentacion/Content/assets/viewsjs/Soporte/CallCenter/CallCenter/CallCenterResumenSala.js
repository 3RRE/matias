$(document).ready(function () {
    ipPublicaG = "";

    GProgresivo = "";
    nombretabla = "";
   
    ObtenerListaSalas();
    $("#cboSala").select2();
    $("#cboProgresivo").select2();

    $(document).on('change', '#cboSala', function (e) {
        var ipPublica = $(this).val();
        ipPublicaG = ipPublica;
        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }
    });

    $(document).on('click', '.btnEditar', function (e) {
        var id_res = $(this).data("id");
        var url = basePath + "CallCenter/CallCenterModificarResumenSalaVista/" + id_res + "?ip=" + ipPublicaG;
        window.location.replace(url);
    });

    
    $(".dateOnly").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
   
    $("#btnBuscar").on("click", function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if ($("#fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        buscarResumenSala();
    });
});

function buscarResumenSala() {
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    }
    var url = ipPublicaG + "/servicio/ObtenerResumenSala?fechaInicio=" + $("#fechaInicio").val() + "&fechaFin=" + $("#fechaFin").val();
    var addtabla = $(".contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CallCenter/ConsultaObtenerResumenSalaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        
        success: function (response) {
            
            response = response.data;
            dataAuditoria(1, "#formfiltro", 3, "CallCenter/ConsultaObtenerResumenSalaJson", "BOTON BUSCAR");
            $(addtabla).empty();
            $(addtabla).append('<table id="ResumenSala" class="table table-condensed table-bordered table-hover"></table>');
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
                columns: [

                    { data: "codResumen", title: "codResumen" },
                    {
                        data: "fechaOperacion", title: "fechaOperacion",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY');
                        }
                    },
                    { data: "estado", title: "estado" },
                    { data: "usuario", title: "usuario" },
                    { data: "saldoinicialsoles", title: "saldoinicialsoles" },
                    { data: "fondocaja", title: "fondocaja" },
                    { data: "salidaotros", title: "salidaotros" },
                    { data: "ingresootros", title: "ingresootros" },
                    { data: "prestamoaotrasala", title: "prestamoaotrasala" },
                    { data: "prestamodeotrasala", title: "prestamodeotrasala" },
                    { data: "devolucionaotrasala", title: "devolucionaotrasala" },
                    { data: "devoluciondeotrasala", title: "devoluciondeotrasala" },
                    { data: "prestamoaoficina", title: "prestamoaoficina" },
                    { data: "refuerzodefondo", title: "refuerzodefondo" },
                    {
                        data: "fecharegistro", title: "fecharegistro",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY');
                        }
                    },
                    { data: "sobrantes", title: "sobrantes" },
                    { data: "faltantes", title: "faltantes" },
                    { data: "sorteos", title: "sorteos" },
                    { data: "monedasfallas", title: "monedasfallas" },
                    { data: "visa", title: "visa" },
                    { data: "efectivofinal", title: "efectivofinal" },
                    { data: "efectivoSoles", title: "efectivoSoles" },
                    { data: "efectivoDolares", title: "efectivoDolares" },
                    { data: "mastercard", title: "mastercard" },
                    { data: "otrastarjetas", title: "otrastarjetas" },
                    {
                        data: null, title: "Accion",
                        "render": function (value) {
                            return '<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="' + value.codResumen + '"><i class="glyphicon glyphicon-pencil"></i></button> ';
                        }
                    }
                ]
                ,
                "initComplete": function (settings, json) {

                    $('#btnExcel').off("click").on('click', function () {

                        cabecerasnuevas = [];
                        cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
                        
                        var ocultar = ["Accion"];//"Accion";
                        funcionbotonesnuevo({
                            botonobjeto: this, ocultar: ocultar,
                            tablaobj: objetodatatable,
                            cabecerasnuevas: cabecerasnuevas,
                        });
                        VistaAuditoria("CallCenter/ConsultaObtenerResumenSalaJsonExcel", "EXCEL", 0, "", 3);
                    });

                },
                "drawCallback": function (settings) {
                    $('.btnEditar').tooltip({
                        title: "Editar"
                    });
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
function ObtenerListaSalas() {
    comboImagen = $("#cboImagen");
    comboImagen.find('option').remove();
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
                $("#cboSala").append('<option value="' + value.UrlProgresivo + '"  data-id="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
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
function MostrarTablas() {
    if (nombretabla == "Procesos_TITO") {
        $("#det_mov_maq").addClass("hidden");
        $("#det_tt0").addClass("hidden");
        $("#det_tito").addClass("hidden");
        $("#proc_tito").removeClass("hidden");
    }
    else if (nombretabla == "Detalle_movaux_por_maquina") {

        $("#det_tt0").addClass("hidden");
        $("#det_tito").addClass("hidden");
        $("#proc_tito").addClass("hidden");
        $("#det_mov_maq").removeClass("hidden");
    }
    else if (nombretabla == "Det0001TTO_00H") {

        $("#det_tito").addClass("hidden");
        $("#proc_tito").addClass("hidden");
        $("#det_mov_maq").addClass("hidden");
        $("#det_tt0").removeClass("hidden");
    }
    else if (nombretabla == "Det0001TTO") {
        $("#proc_tito").addClass("hidden");
        $("#det_mov_maq").addClass("hidden");
        $("#det_tt0").addClass("hidden");
        $("#det_tito").removeClass("hidden");        
    }
}
VistaAuditoria("CallCenter/CallCenterResumenSalaVista", "VISTA", 0, "", 3);
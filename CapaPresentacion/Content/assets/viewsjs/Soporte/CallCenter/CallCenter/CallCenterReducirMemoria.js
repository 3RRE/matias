$(document).ready(function () {
    ipPublicaG = "";

    $("#cboSala").select2({ dropdownAutoWidth: true});

    ObtenerListaSalas();

    $(document).on('change', '#cboSala', function (e) {
        var ipPublica = $(this).val();
        ipPublicaG = ipPublica;
        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }
        console.log(ipPublica);
    });

    $('.btn-reducir-1').on('click', function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }

        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Reducir Memoria?',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: "confirmar",
            cancelButton: 'Aún No',
            content: "",
            confirm: function () {
                var url = ipPublicaG + "/servicio/Memoria_Bajar1_Inicio";

                var nombresala = $("#cboSala option:selected").text();
                var idsala = $("#cboSala option:selected").data("id");
                var datafinal = {
                    SalaNombre: nombresala,
                    SalaID: idsala,
                    SalaUrl: ipPublicaG
                };
                dataAuditoriaJSON(3, "CallCenter/ConsultaMemoria_Bajar1_InicioJson", "REDUCIR MEMORIA INICIO", "", datafinal);

                $.ajax({
                    type: "POST",
                    cache: false,
                    url: basePath + "CallCenter/ConsultaMemoria_Bajar1_InicioJson",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ url: url }),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (response) {
                        
                        response = response.data;
                        console.log(response);
                        if (response == '1') {
                            toastr.success("Se redujo la memoria");
                        }
                        else {
                            toastr.error("Error al reducir la memoria, Servidor no Encontrado.");
                        }
                    },
                    error: function (request, status, error) {
                        toastr.error("Error De Conexion, Servidor no Encontrado.");
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide");
                    }
                });

            },
            cancel: function () {
            },
        });
    });

    $('.btn-reducir-2').on('click', function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }
        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Reducir Memoria?',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: "confirmar",
            cancelButton: 'Aún No',
            content: "",
            confirm: function () {
                var url = ipPublicaG + "/servicio/Memoria_Bajar1_Fin";
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: basePath + "CallCenter/ConsultaMemoria_Bajar1_FinJson",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ url: url }),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (response) {
                        var nombresala = $("#cboSala option:selected").text();
                        var idsala = $("#cboSala option:selected").data("id");
                        var datafinal = {
                            SalaNombre: nombresala,
                            SalaID: idsala,
                            SalaUrl: ipPublicaG
                        };
                        dataAuditoriaJSON(3, "CallCenter/ConsultaMemoria_Bajar1_FinJson", "REDUCIR MEMORIA FIN", "", datafinal);

                        response = response.data;
                        console.log(response);
                        if (response == '1') {
                            toastr.success("Se redujo la memoria");
                        }
                        else {
                            toastr.error("Error al reducir la memoria, Servidor no Encontrado.");
                        }
                    },
                    error: function (request, status, error) {
                        toastr.error("Error De Conexion, Servidor no Encontrado.");
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide");
                    }
                });

            },
            cancel: function () {
            },
        });
    });

    $('.btn-reducir-3').on('click', function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }
        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Reducir Log?',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: "confirmar",
            cancelButton: 'Aún No',
            content: "",
            confirm: function () {
                var url = ipPublicaG + "/servicio/ReducirLog";
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: basePath + "CallCenter/ConsultaReducirLogJson",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ url: url }),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (response) {
                        var nombresala = $("#cboSala option:selected").text();
                        var idsala = $("#cboSala option:selected").data("id");
                        var datafinal = {
                            SalaNombre: nombresala,
                            SalaID: idsala,
                            SalaUrl: ipPublicaG
                        };
                        dataAuditoriaJSON(3, "CallCenter/ConsultaReducirLogJson", "REDUCIR LOG", "", datafinal);
                        response = response.data;
                        console.log(response);

                        if (response == null) {
                            toastr.error("Error al reducir el Log, Servidor no Encontrado.");
                        }
                        else {
                            toastr.success("Se ha reducido el Log");
                        }
                    },
                    error: function (request, status, error) {
                        toastr.error("Error De Conexion, Servidor no Encontrado.");
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide");
                    }
                });

            },
            cancel: function () {
            },
        });
    });

    $("#btnBuscar").on("click", function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }
        buscarResumenSala();
    });
});

function buscarResumenSala() {
    var url = ipPublicaG + "/servicio/ObtenerResumenSala?fechaInicio=" + $("#fechaInicio").val();
    if ($("#pro_ti_fechaInicio").val() == "") {
        toastr.error("Ingrese una fecha de Inicio.");
    }
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
                        data: null, title: "Acción",
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
                        cabecerasnuevas.push({ nombre: "Progresivo", valor: $("#cboProgresivo option:selected").text() });
                        cabecerasnuevas.push({ nombre: "Máquina", valor: $("#txtMaquina").val() });
                        cabecerasnuevas.push({ nombre: "Pozos", valor: $("#cboPozos option:selected").text() });

                        definicioncolumnas = [];
                        definicioncolumnas.push({ nombre: "Monto", tipo: "decimal", alinear: "right", sumar: "true" });

                        var ocultar = [];//"Accion";
                        funcionbotonesnuevo({
                            botonobjeto: this, ocultar: ocultar,
                            tablaobj: objetodatatable,
                            cabecerasnuevas: cabecerasnuevas,
                            definicioncolumnas: definicioncolumnas
                        });
                    });

                },
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
VistaAuditoria("CallCenter/CallCenterReducirMemoriaVista", "VISTA", 0, "", 3);
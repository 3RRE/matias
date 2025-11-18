$(document).ready(function () {
    ipPublicaG = "";
    ipPublicaGProgresivo = "";
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
    $("#btnBuscar").on("click", function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        buscarTicket();
    }); 
});

function buscarTicket() {
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    }
    var url = ipPublicaG + "/servicio/ConsultaTicketRegistradoMovAuxiliar?nroTicket=" + $("#nticket").val();
    console.log(url);
    dataAuditoria(1, "#formfiltro", 3, "CallCenter/ConsultaTicketRegistradoMovAuxiliarListadoJson", "BOTON BUSCAR");
    if ($("#nticket").val() =="") {
        toastr.error("Ingrese Numero de Ticket.");
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath +"CallCenter/ConsultaTicketRegistradoMovAuxiliarListadoJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {             
            response = response.data;
            objetodatatable = $("#table").DataTable({
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
                    { data: "item_maqui", title: "item_maqui"},
                    { data: "item", title: "item" },
                    { data: "idtipoficha", title: "idtipoficha" },
                    { data: "idtipomoneda", title: "idtipomoneda" },
                    { data: "valorficha", title: "valorficha" },
                    { data: "nro", title: "nro" },
                    { data: "maq_alterno", title: "maq_alterno" },
                    { data: "modelo", title: "modelo" },
                    { data: "salida", title: "salida" },
                    { data: "ingreso", title: "ingreso" },
                    { data: "pagomanual", title: "pagomanual" },
                    { data: "valor_es_un", title: "valor_es_un" },
                    { data: "estado", title: "estado" },
                    { data: "tipomaquina", title: "tipomaquina" },
                    { data: "item_pmr", title: "item_pmr" },
                    { data: "CancelldCredit_Fin", title: "CancelldCredit_Fin" },
                    { data: "JackPot_Fin", title: "JackPot_Fin" },
                    {
                        data: "hora_registro", title: "hora_registro",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY h:mm:ss a');
                        }
                    },
                    { data: "estado_registro", title: "estado_registro" },
                    { data: "codPer", title: "codPer" },
                    { data: "Cod_Proceso", title: "Cod_Proceso" },
                    { data: "Cod_Caja", title: "Cod_Caja" },
                    { data: "NroTicket", title: "NroTicket" },
                    { data: "cliente_dni", title: "cliente_dni" },
                    { data: "serie_num", title: "serie_num" },
                    { data: "cod_asignacion_ficha", title: "cod_asignacion_ficha" },
                    { data: "IngresoManual", title: "IngresoManual" },
                    { data: "CAJERA_GSI", title: "CAJERA_GSI" },
                    { data: "var_tablet", title: "var_tablet" }
                ]
                ,
                "rowCallback": function (row, data) {
                    console.log(data.Evento); 7
                    $('td:eq(1)', row).css('background-color', '#F3F781');
                    $('td:eq(6)', row).css('background-color', '#F3F781');
                    $('td:eq(10)', row).css('background-color', '#F3F781');
                    $('td:eq(17)', row).css('background-color', '#F3F781');
                    $('td:eq(22)', row).css('background-color', '#F3F781');
                },
                "initComplete": function (settings, json) {

                    $('#btnExcel').off("click").on('click', function () {

                        cabecerasnuevas = [];
                        cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
                        cabecerasnuevas.push({ nombre: "Progresivo", valor: $("#cboProgresivo option:selected").text() });
                        cabecerasnuevas.push({ nombre: "Máquina", valor: $("#txtMaquina").val() });
                        cabecerasnuevas.push({ nombre: "Pozos", valor: $("#cboPozos option:selected").text() });

                        definicioncolumnas = [];
                        definicioncolumnas.push({ nombre: "Monto", tipo: "decimal",alinear:"right",sumar:"true"});

                        var ocultar = [];//"Accion";
                        funcionbotonesnuevo({
                            botonobjeto: this, ocultar: ocultar,
                            tablaobj: objetodatatable,
                            cabecerasnuevas: cabecerasnuevas,
                            definicioncolumnas: definicioncolumnas
                        });
                        VistaAuditoria("CallCenter/CallCenterVistaExcel", "EXCEL", 0, "", 3);
                    });

                },
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

VistaAuditoria("CallCenter/CallCenterVista", "VISTA", 0, "", 3);



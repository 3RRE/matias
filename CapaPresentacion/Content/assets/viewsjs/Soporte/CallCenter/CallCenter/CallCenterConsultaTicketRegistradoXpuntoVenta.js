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
    $(".fechaInicioConsultaxPuntoVenta").datetimepicker({
        pickTime: true,
        format: 'DD/MM/YYYY h:mm:ss a',
        defaultDate: null,
       
    });
    $("#btnBuscar").on("click", function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        consultaTicketRegistradoXpuntoVenta();
    });
    $('#fechaInicio').val('');
    $('#fechaFin').val('');
    
});
function formatearInputDeFecha(fecha) {
    var date = document.getElementById(fecha);
    function checkValue(str, max) {
        if (str.charAt(0) !== '0' || str == '00') {
            var num = parseInt(str);
            if (isNaN(num) || num <= 0 || num > max) num = 1;
            str = num > parseInt(max.toString().charAt(0)) && num.toString().length == 1 ? '0' + num : num.toString();
        };
        return str;
    };

    date.addEventListener('input', function (e) {
        this.type = 'text';
        var input = this.value;
        if (/\D\/$/.test(input)) input = input.substr(0, input.length - 3);
        var values = input.split('/').map(function (v) {
            return v.replace(/\D/g, '')
        });
        if (values[0]) values[0] = checkValue(values[0], 31);

        if (values[1]) values[1] = checkValue(values[1], 12);
        var output = values.map(function (v, i) {
            return v.length == 2 && i < 2 ? v + ' / ' : v;
        });
        this.value = output.join('').substr(0, 14);
    });

    date.addEventListener('blur', function (e) {
        this.type = 'text';
        var input = this.value;
        var values = input.split('/').map(function (v, i) {
            return v.replace(/\D/g, '')
        });
        var output = '';

        if (values.length == 3) {
            var year = values[2].length !== 4 ? parseInt(values[2]) + 2000 : parseInt(values[2]);
            var month = parseInt(values[0]) - 1;
            var day = parseInt(values[1]);
            var d = new Date(year, month, day);
            if (!isNaN(d)) {
                
                var dates = [d.getMonth() + 1, d.getDate(), d.getFullYear()];
                output = dates.map(function (v) {
                    v = v.toString();
                    return v.length == 1 ? '0' + v : v;
                }).join(' / ');
            };
        };
        this.value = output;
    });
}

function consultaTicketRegistradoXpuntoVenta() {
   
    var cantidad = "&cantidad=";
    var fechaInicio = "&fechaInicio=";
    var fechaFin = "&fechaFin=";
    
    if ($("#puntoVenta").val() =="") {
        toastr.error("Ingrese Punto de Venta.");
    }
    if ($("#cantidad").val() == "") {
        cantidad=""
    }
    if ($("#fechaInicio").val() == "") {
        fechaInicio = ""
    }
    if ($("#fechaFin").val() == "") {
        fechaFin = ""
    }
    dataAuditoria(1, "#formfiltro", 3, "CallCenter/ConsultaTicketRegistradoXpuntoVentaListadoJson", "BOTON BUSCAR");
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url = ipPublicaG + "/servicio/ConsultaTicketRegistradoXpuntoVenta?puntoVenta=" +
        $("#puntoVenta").val()
        + cantidad + $("#cantidad").val()
        + fechaInicio + $("#fechaInicio").val()
        + fechaFin + $("#fechaFin").val();
    
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath +"CallCenter/ConsultaTicketRegistradoXpuntoVentaListadoJson",
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
                
                    { data: "Item", title: "Item"},
                    {
                        data: "Fecha_Apertura", title: "Fecha_Apertura",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY h:mm:ss a');
                        } },
                    { data: "Tipo_venta", title: "Tipo_venta" },
                    { data: "Punto_venta", title: "Punto_venta" },
                    {
                        data: "Tito_fechaini", title: "Tito_fechaini",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY h:mm:ss a');
                        } },
                    { data: "Tito_NroTicket", title: "Tito_NroTicket" },
                    { data: "Tito_MontoTicket", title: "Tito_MontoTicket" },
                    { data: "Tito_MTicket_NoCobrable", title: "Tito_MTicket_NoCobrable" },
                    { data: "Tipo_venta_fin", title: "Tipo_venta_fin" },
                    { data: "Punto_venta_fin", title: "Punto_venta_fin" },
                    {
                        data: "Tito_fechafin", title: "Tito_fechafin",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY h:mm:ss a');
                        } },
                    { data: "codclie", title: "codclie" },
                    { data: "tipo_ticket", title: "tipo_ticket" },
                    { data: "Estado", title: "Estado" },
                    { data: "IdTipoMoneda", title: "IdTipoMoneda" },
                    { data: "Motivo", title: "Motivo" },
                    { data: "IdTipoPago", title: "IdTipoPago" },
                    { data: "Tipo_Proceso", title: "Tipo_Proceso" },
                    { data: "r_Estado", title: "r_Estado" },
                    { data: "Tipo_Ingreso", title: "Tipo_Ingreso" },
                    {
                        data: "Fecha_Apertura_Real", title: "Fecha_Apertura_Real",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY h:mm:ss a');
                        } },
                    { data: "PuntoVentaMin", title: "PuntoVentaMin" },
                    {
                        data: "fecha_reactiva", title: "fecha_reactiva",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY h:mm:ss a');
                        } },
                    { data: "turno", title: "turno" },
                    { data: "codCaja", title: "codCaja" },
                    { data: "player_tracking", title: "player_tracking" },
                ]
                ,
                "rowCallback": function (row, data) {
                    
                    $('td:eq(1)', row).css('background-color', '#F3F781');
                    $('td:eq(3)', row).css('background-color', '#F3F781');
                    $('td:eq(5)', row).css('background-color', '#F3F781');
                    $('td:eq(6)', row).css('background-color', '#F3F781');
                },
                "initComplete": function (settings, json) {

                    $('#btnExcel').off("click").on('click', function () {

                        cabecerasnuevas = [];
                        cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });

                        definicioncolumnas = [];
                        definicioncolumnas.push({ nombre: "Monto", tipo: "decimal",alinear:"right",sumar:"true"});

                        var ocultar = [];//"Accion";
                        funcionbotonesnuevo({
                            botonobjeto: this, ocultar: ocultar,
                            tablaobj: objetodatatable,
                            cabecerasnuevas: cabecerasnuevas,
                            definicioncolumnas: definicioncolumnas
                        });
                        VistaAuditoria("CallCenter/CallCenterConsultaTicketRegistradoXpuntoVentaExcel", "EXCEL", 0, "", 3);
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

VistaAuditoria("CallCenter/CallCenterConsultaTicketRegistradoXpuntoVenta", "VISTA", 0, "", 3);


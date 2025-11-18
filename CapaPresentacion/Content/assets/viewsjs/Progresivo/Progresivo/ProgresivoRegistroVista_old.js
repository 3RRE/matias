
var parametros_detalle_sup;
var parametros_detalle_med;
var parametros_detalle_inf;
var v_mensaje_error = "";
var v_ocurrio_error = 0;
var v_codigo_cabecera = '0';
var validacion_cajas_texto = 0;
ipPublicaG = "";
PorgresivoId = -1;
SalaId = -1;
PorgresivoStr = "";
SalaStr = "";
DataHistoricaCabecera = {};
DataHistoricaPozosActuales = [];
DataHistoricaDetallePozos = [];

function sleepFor(sleepDuration) {
    var now = new Date().getTime();
    while (new Date().getTime() < now + sleepDuration) { /* do nothing */ }
}
function VerificarLocalStorage(){
    if(typeof(Storage)!=="undefined"){
        //POZO SUPERIOR
        if(!localStorage.getItem('chkModificarPozoSuperior')){
            localStorage.setItem('chkModificarPozoSuperior','false');
           document.getElementById('chkModificarPozoSuperior').checked=false;
        }
        else{
            var checked=localStorage.getItem('chkModificarPozoSuperior');
            if(checked==='true'){
                document.getElementById('chkModificarPozoSuperior').checked=true;
            }
            else{
                document.getElementById('chkModificarPozoSuperior').checked=false;
            }
        }
        //POZO MEDIO
        if(!localStorage.getItem('chkModificarPozoMedio')){
            localStorage.setItem('chkModificarPozoMedio','false');
            document.getElementById('chkModificarPozoMedio').checked=false;
        }
        else{
            var checked=localStorage.getItem('chkModificarPozoMedio');
            if(checked==='true'){
                document.getElementById('chkModificarPozoMedio').checked=true;
            }
            else{
                document.getElementById('chkModificarPozoMedio').checked=false;
            }
        }
        //POZO INFERIOR
        if(!localStorage.getItem('chkModificarPozoInferior')){
            localStorage.setItem('chkModificarPozoInferior','false');
            document.getElementById('chkModificarPozoInferior').checked =false;
        }
        else{
            var checked=localStorage.getItem('chkModificarPozoInferior');
            if(checked==='true'){
                document.getElementById('chkModificarPozoInferior').checked=true;
            }
            else{
                document.getElementById('chkModificarPozoInferior').checked=false;
            }
        }
    }
    else{
        alert("Este Navegador no soporta LocalStorage, por favor Actualizarlo");
    }
}
VistaAuditoria("Progresivo/ProgresivoRegistroVista", "VISTA",0,"",1);
$(document).ready(function () {
    //llenarSelect(basePath + "Sala/ListadoSalaPorUsuarioJson", {}, "cboSala", "UrlProgresivo", "Nombre");

    //Verificar si el navegador soporta LocalStorage y Crear Variables
    VerificarLocalStorage();
    // var RegistrarPozos = $("#RegistrarPozo").val();

    //checkbox para modificar pozos
    //   $(document).on('change','.cls_modificar_pozo_check',function(){
    //     if ($(this).is(":checked"))
    //     {
    //         console.log('checked');
    //     }
    //     else{
    //         console.log('unckecked');    
    //     }
    //   })
    ObtenerListaSalas();
    VerificarLocalStorage();
    $('#txtPSPremioBase').keyup(function () {
        if($("#chkModificarPozoSuperior").is(':checked')){
            $('#txtPSPozoActual').val($('#txtPSPremioBase').val());
        }
    });
    $('#txtPMPremioBase').keyup(function () {
        if($("#chkModificarPozoMedio").is(':checked')){
            $('#txtPMPozoActual').val($('#txtPMPremioBase').val());
        }
    });
    $('#txtPIPremioBase').keyup(function () {
        if($("#chkModificarPozoInferior").is(':checked')){
            $('#txtPIPozoActual').val($('#txtPIPremioBase').val());
        }
    });
    $('#btnGrabarPozo').click(APiModificarPozos);

    function TotalProgresivo() {
        valores = $('input[name=pozocheck]:checked').length;
        $('#txtNroPozos').val(valores);
    }

    $('#btnGrabar').click(function () {          
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if ($("#cboProgresivo").val() == "") {
            toastr.error("Seleccione Progresivo", "Mensaje Servidor");
            return false;
        }
        var acumVal = 0;
        if ($('#chkPozoSuperior').is(":checked")) {
            acumVal++;
        }
        if ($('#chkPozoMedio').is(":checked")) {
            acumVal++;
        }
        if ($('#chkPozoInferior').is(":checked")) {
            acumVal++;
        }
        if (acumVal == 0) {
            toastr.error("Habilite al menos un pozo para grabar los datos", "Mensaje Servidor");
            return false;
        }
        ValidarCajasTexto();
        return false;
    });

    $('#chkPozoOculto').click(function () {
        if ($('#chkPozoSuperior').is(":checked")) {
            if (!$('#chkPozoOculto').is(":checked")) {
                $('#txtPSPremioBase').removeAttr('disabled');
            }
            else {
                $('#txtPSPremioBase').attr('disabled', 'disabled');
            }
        }
        if ($('#chkPozoMedio').is(":checked")) {
            if (!$('#chkPozoOculto').is(":checked")) {
                $('#txtPMPremioBase').removeAttr('disabled');
            }
            else {
                $('#txtPMPremioBase').attr('disabled', 'disabled');
            }
        }
        if ($('#chkPozoInferior').is(":checked")) {
            if (!$('#chkPozoOculto').is(":checked")) {
                $('#txtPIPremioBase').removeAttr('disabled');
            }
            else {
                $('#txtPIPremioBase').attr('disabled', 'disabled');
            }
        }
    });

    $('#chkPozoSuperior').click(function () {
        if ($('#chkPozoSuperior').is(":checked")) {
            ManipularCajasTextoPozoSuperior(1);
        }
        else {
            ManipularCajasTextoPozoSuperior(0);
        }
        TotalProgresivo();
    });

    $('#chkPozoMedio').click(function () {
        if ($('#chkPozoMedio').is(":checked")) {
            ManipularCajasTextoPozoMedio(1);
        }
        else {
            ManipularCajasTextoPozoMedio(0);
        }
        TotalProgresivo();
    });

    $('#chkPozoInferior').click(function () {
        if ($('#chkPozoInferior').is(":checked")) {
            ManipularCajasTextoPozoInferior(1);
        }
        else {
            ManipularCajasTextoPozoInferior(0);
        }
        TotalProgresivo();
    });

    //--------------------------------------
    $(document).on('change', '#cboSala', function (e) {
        var ipPublica = $(this).val();
        //ipPublica = ipPublica.substring(0, ipPublica.length - 4);
        //ipPublica = ipPublica + "9895";
        ipPublicaG = ipPublica;
        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            $("#cboProgresivo").html("");
            $("#cboProgresivo").append('<option value="">--Seleccione--</option>');
            $("#cboProgresivo").removeAttr("disabled");
            return false;
        }
        llenarSelectAPIProgresivo(ipPublica + "/servicio/listadoprogresivos", {}, "cboProgresivo", "WEB_PrgID", "WEB_Nombre");
        //var url = basePath + "Empleado/UsuarioEmpleadoIdObtenerJson";
        //var data = { empleadiId: id };
       
    });
    $(document).on('change', '#cboProgresivo', function (e) {  
        if ($("#cboProgresivo").val()=="") {
            toastr.error(result, "Seleccione Progresivo");
        }
        PorgresivoId = $(this).val();      
        SalaId = $("#cboSala option:selected").data("id");
        PorgresivoStr = $("#cboProgresivo option:selected").text();
        SalaStr= $("#cboSala option:selected").text();

        ////eliminar
        //ipPublicaG = "http://192.168.1.47:9899";
        ////fin eliminar
        console.log(PorgresivoId, SalaId, PorgresivoStr, SalaStr);
        ObtenerListaImagenes();
        LimpiarCabecera();
        LimpiarDetalle();

        ManipularCajasTextoPozoSuperior(0);
        ManipularCajasTextoPozoMedio(0);
        ManipularCajasTextoPozoInferior(0);  

       
    });

    $(".checkbox-form .icheck-square").iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-red',
        increaseArea: '20%' // optional
    });   
    $(".icheck-line input").each(function () {
        var self = $(this),
            label = self.next(),
            label_text = label.text();

        label.remove();
        self.iCheck({
            checkboxClass: 'icheckbox_line-blue',
            radioClass: 'iradio_line-purple',
            insert: '<div class="icheck_line-icon"></div>' + label_text
        });
    });
});

// --------------------------------------------------------------------
// ----------------------------- LISTADOS -----------------------------
// --------------------------------------------------------------------

function ObtenerListaImagenes() {    
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    //var url = getUrlApi("ListaImagenesPozo/getListarImagenes");
    var url = ipPublicaG + "/servicio/getListarImagenes/" + PorgresivoId;
    comboImagen = $("#cboImagen");
    comboImagen.find('option').remove();  
    console.log(url);
    ajaxhr = $.ajax({        
        type: "POST",
        url: basePath + "Progresivo/ProgresivoListarImagenesJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",        
        data: JSON.stringify({ url: url }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            console.log(result);
            if (result.data == null) {
                toastr.error("No se encontraron datos.Consulte al administrador", "Mensaje Servidor");
                return false;
            }
            result = result.data;
            if (result == "No es posible conectar con el servidor remoto") {
                toastr.error(result, "Mensaje Servidor");
                return false;
            }
            var entidad = eval(result);
            $(entidad).each(function () {
                var option = $(document.createElement('option'));

                option.text(this.Descripcion);
                option.val(this.ID);                
                comboImagen.append(option);
            });
            ObtenerProgresivoActivo();

        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error, "Mensaje Servidor");
            return false;
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
    return false;
}
function ObtenerProgresivoActivo() {    
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    //var url = getUrlApi("RegistroProgresivo/ObtenerProgresivoActivo");    
    var url = ipPublicaG + "/servicio/ObtenerProgresivoActivo/" + PorgresivoId; 
    $.ajax({        
        type: "POST",
        url: basePath + "Progresivo/ProgresivoActivoObtenerJson",
        cache: false,
        headers: getHeader(),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        success: function (result) {   
            DataHistoricaCabecera = result.data;
            if (result.data == null) {
                toastr.error("No se han encontrado datos", "Mensaje Servidor");
                return false;
            }            
            result = result.data;
            var entidad = result;
            if (entidad["ProgresivoID"] == 0) {
                $('#hid_cod_cab_prog').val('0');
                ObtenerPozosActuales(0);
                LimpiarCabecera();
                LimpiarDetalle();
                return false;
            }
            else {
                $('#hid_cod_cab_prog').val(entidad["ProgresivoID"]);
                $('#txtNumJugadores').val(entidad["NroJugadores"]);
                var base_oculto = entidad["BaseOculto"];
                if (base_oculto == 0) {
                    $('#chkPozoOculto').iCheck('uncheck');
                    if ($('#chkPozoSuperior').is(":checked")) {
                        $('#txtPSPremioBase').removeAttr('disabled');
                    }
                    if ($('#chkPozoMedio').is(":checked")) {
                        $('#txtPMPremioBase').removeAttr('disabled');
                    }
                    if ($('#chkPozoInferior').is(":checked")) {
                        $('#txtPIPremioBase').removeAttr('disabled');
                    }
                }
                else {
                    $('#chkPozoOculto').iCheck('check');
                    $('#txtPSPremioBase').attr('disabled', 'disabled');
                    $('#txtPMPremioBase').attr('disabled', 'disabled');
                    $('#txtPIPremioBase').attr('disabled', 'disabled');
                }
                if (entidad["RegHistorico"]) {
                    $('#chkRegHistorico').iCheck('check');
                }
                else {
                    $('#chkRegHistorico').iCheck('uncheck');
                }                                
                $('#cboLugarPago').val((entidad["PagoCaja"]) ? 1 : 0);
                $('#txtNroPozos').val(entidad["NroPozos"]);
                $('#txtDuracion').val(entidad["DuracionPantalla"]);
                $('#cboEstado').val(entidad["Estado"]);
                $('#cboImagen').val(entidad["ProgresivoImagenID"]);
                ObtenerPozosActuales(entidad["ProgresivoID"]);

                return false;
            }
        },        
        error: function (request, status, error) {            
            toastr.error(request.responseText + " " + error, "Mensaje Servidor");
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        }
    });
    return false;
}
function ObtenerPozosActuales(codigo) {    
    LimpiarDetalle();   
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    //url = getUrlApiAparameter("RegistroProgresivo/ListarPozosActuales", parametros);
    var url = ipPublicaG + "/servicio/ListarPozosActuales/" + PorgresivoId + "/" + codigo;  
    console.log("pozo actual: "+ url);
    $.ajax({
        type: "POST",
        url: basePath + "Progresivo/ProgresivoPozosActualesObtenerJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        success: function (result) {
            DataHistoricaPozosActuales = result.data;
            result = result.data;
            var lista = result;
            if (lista.length > 0) {
                $.each(lista, function (indice, valor) {
                    if (valor['TipoPozo'] == 1) { $('#txtPSPozoActual').val(valor['Actual']); $('#hid_cod_det_prog_sup').val(valor['DetalleProgresivoID']);$("#valor_pozo_superior_actual").val(valor['Actual']); }
                    if (valor['TipoPozo'] == 2) { $('#txtPMPozoActual').val(valor['Actual']); $('#hid_cod_det_prog_med').val(valor['DetalleProgresivoID']);$("#valor_pozo_medio_actual").val(valor['Actual']); }
                    if (valor['TipoPozo'] == 3) { $('#txtPIPozoActual').val(valor['Actual']); $('#hid_cod_det_prog_inf').val(valor['DetalleProgresivoID']);$("#valor_pozo_inferior_actual").val(valor['Actual']); }
                });
                ObtenerDetallesProgresivo(codigo,1);
                return false;
            }
            else {
                $('#hid_cod_det_prog_sup').val('0');
                $('#hid_cod_det_prog_med').val('0');
                $('#hid_cod_det_prog_inf').val('0');
                return false;
            }
        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error, "Mensaje Servidor");
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        }
    });
}
function ObtenerDetallesProgresivo(codProg, estado) {    
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    //var url = getUrlApiAparameter("RegistroProgresivo/ListarDetallesProgresivo", parametros);
    var url = ipPublicaG + "/servicio/ListarDetallesProgresivo/" + PorgresivoId + "/" + codProg +"/"+estado;
    console.log(url);
    $.ajax({
        type: "POST",
        url: basePath + "Progresivo/ProgresivoDetalleListarJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        success: function (result) {                          
            DataHistoricaDetallePozos = result.data;
            $.each(DataHistoricaDetallePozos, function (index, value) {
                if (value.TipoPozo==1) {
                    value.Actual = $('#txtPSPozoActual').val();
                }
                if (value.TipoPozo == 2) {
                    value.Actual = $('#txtPMPozoActual').val();
                }
                if (value.TipoPozo == 3) {
                    value.Actual = $('#txtPIPozoActual').val();
                }
            });
            result = result.data;
            var lista = result;
            $('#hid_cod_det_prog_sup').val('0');
            $('#hid_cod_det_prog_med').val('0');
            $('#hid_cod_det_prog_inf').val('0');
            if (lista.length > 0) {
                $.each(lista, function (indice, valor) {
                    if (valor['TipoPozo'] == 1) {
                        $('#hid_cod_det_prog_sup').val(valor['DetalleProgresivoID']);
                        $('#txtPSRsApuesta').val(valor['RsApuesta']);
                        $('#txtPSRsJugadores').val(valor['RsJugadores']);
                        $('#txtPSPremioBase').val(valor['MontoBase']);
                        $('#txtPSPremioMinimo').val(valor['MontoMin']);
                        $('#txtPSPremioMaximo').val(valor['MontoMax']);
                        $('#txtPSIncPozoNormal').val(valor['IncPozo1']);
                        $('#txtPSMontoOcultoMin').val(valor['MontoOcMin']);
                        $('#txtPSMontoOcultoMax').val(valor['MontoOcMax']);
                        $('#txtPSIncPozoOculto').val(valor['IncOcPozo1']);
                        $('#cboPSDificultad').val(valor['Dificultad']);
                        ManipularCajasTextoPozoSuperior(valor['Estado']);
                        $('#chkPozoSuperior').prop("checked", ((valor['Estado'] == 1) ? true : false));
                    }
                    if (valor['TipoPozo'] == 2) {
                        $('#hid_cod_det_prog_med').val(valor['DetalleProgresivoID']);
                        $('#txtPMRsApuesta').val(valor['RsApuesta']);
                        $('#txtPMRsJugadores').val(valor['RsJugadores']);
                        $('#txtPMPremioBase').val(valor['MontoBase']);
                        $('#txtPMPremioMinimo').val(valor['MontoMin']);
                        $('#txtPMPremioMaximo').val(valor['MontoMax']);
                        $('#txtPMIncPozoNormal').val(valor['IncPozo1']);
                        $('#txtPMMontoOcultoMin').val(valor['MontoOcMin']);
                        $('#txtPMMontoOcultoMax').val(valor['MontoOcMax']);
                        $('#txtPMIncPozoOculto').val(valor['IncOcPozo1']);
                        $('#cboPMDificultad').val(valor['Dificultad']);
                        ManipularCajasTextoPozoMedio(valor['Estado']);
                        $('#chkPozoMedio').prop("checked", ((valor['Estado'] == 1) ? true : false));
                    }
                    if (valor['TipoPozo'] == 3) {
                        $('#hid_cod_det_prog_inf').val(valor['DetalleProgresivoID']);
                        $('#txtPIRsApuesta').val(valor['RsApuesta']);
                        $('#txtPIRsJugadores').val(valor['RsJugadores']);
                        $('#txtPIPremioBase').val(valor['MontoBase']);
                        $('#txtPIPremioMinimo').val(valor['MontoMin']);
                        $('#txtPIPremioMaximo').val(valor['MontoMax']);
                        $('#txtPIIncPozoNormal').val(valor['IncPozo1']);
                        $('#txtPIMontoOcultoMin').val(valor['MontoOcMin']);
                        $('#txtPIMontoOcultoMax').val(valor['MontoOcMax']);
                        $('#txtPIIncPozoOculto').val(valor['IncOcPozo1']);
                        $('#cboPIDificultad').val(valor['Dificultad']);
                        ManipularCajasTextoPozoInferior(valor['Estado']);
                        $('#chkPozoInferior').prop("checked", ((valor['Estado'] == 1) ? true : false));
                    }
                });
                dataAuditoria(0, "#formProgresivo", 1);
               
                return false;
            }
        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error, "Mensaje Servidor");
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        }
    });
}
function ObtenerListaSalas() {
    comboImagen = $("#cboImagen");
    comboImagen.find('option').remove();
    ajaxhr = $.ajax({
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
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
    return false;
}

// --------------------------------------------------------------------
// ----------------------------- GRABADO -----------------------------
// --------------------------------------------------------------------

function ValidarCajasTexto() {
    validacion_cajas_texto = 1;
    if ($('#chkPozoSuperior').is(":checked")) {
        if ($.trim($('#txtPSPremioBase').val()) == '') { toastr.error('Ingrese monto base'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPSPremioBase').val()) <= 0) { toastr.error('El monto base debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSPremioMinimo').val()) == '') { toastr.error('Ingrese premio minimo'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPSPremioMinimo').val()) <= 0) { toastr.error('El premio minimo debe ser mayor a cero'); resp = 0; return resp; }
        if ($.trim($('#txtPSPremioMaximo').val()) == '') { toastr.error('Ingrese premio maximo'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPSPremioMaximo').val()) <= 0) { toastr.error('El premio maximo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSIncPozoNormal').val()) == '') { toastr.error('Ingrese incremento en el pozo normal'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPSIncPozoNormal').val()) <= 0) { toastr.error('El incremento en el pozo normal debe ser mayor a cero'); resp = 0; return false; }
        if ($('#chkPozoOculto').is(":checked")) {
            if ($.trim($('#txtPSMontoOcultoMin').val()) == '') { toastr.error('Ingrese monto oculto minimo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPSMontoOcultoMin').val()) <= 0) { toastr.error('El monto oculto minimo debe ser mayor a cero'); resp = 0; return false; }
            if ($.trim($('#txtPSMontoOcultoMax').val()) == '') { toastr.error('Ingrese monto oculto maximo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPSMontoOcultoMax').val()) <= 0) { toastr.error('El monto oculto maximo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
            if ($.trim($('#txtPSIncPozoOculto').val()) == '') { toastr.error('Ingrese incremento de pozo oculto maximo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPSIncPozoOculto').val()) <= 0) { toastr.error('El incremento del pozo oculto debe ser mayor a cero'); validacion_cajas_texto = 0; return resp; }
        }
    }
    if ($('#chkPozoMedio').is(":checked")) {
        if ($.trim($('#txtPSPremioBase').val()) == '') { toastr.error('Ingrese monto base'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPMPremioBase').val()) <= 0) { toastr.error('El monto base debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSPremioMinimo').val()) == '') { toastr.error('Ingrese premio minimo'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPMPremioMinimo').val()) <= 0) { toastr.error('El premio minimo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSPremioMaximo').val()) == '') { toastr.error('Ingrese premio maximo'); validacion_cajas_texto = 0; return resp; }
        if (($('#txtPMPremioMaximo').val()) <= 0) { toastr.error('El premio maximo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSIncPozoNormal').val()) == '') { toastr.error('Ingrese incremento en el pozo normal'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPMIncPozoNormal').val()) <= 0) { toastr.error('El incremento en el pozo normal debe ser mayor a cero'); resp = 0; return false; }
        if ($('#chkPozoOculto').is(":checked")) {
            if ($.trim($('#txtPSMontoOcultoMin').val()) == '') { toastr.error('Ingrese monto oculto minimo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPMMontoOcultoMin').val()) <= 0) { toastr.error('El monto oculto minimo debe ser mayor a cero'); resp = 0; return false; }
            if ($.trim($('#txtPSMontoOcultoMax').val()) == '') { toastr.error('Ingrese monto oculto maximo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPMMontoOcultoMax').val()) <= 0) { toastr.error('El monto oculto maximo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
            if ($.trim($('#txtPSIncPozoOculto').val()) == '') { toastr.error('Ingrese incremento de pozo oculto maximo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPMIncPozoOculto').val()) <= 0) { toastr.error('El incremento del pozo oculto debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        }
    }
    if ($('#chkPozoInferior').is(":checked")) {
        if ($.trim($('#txtPSPremioBase').val()) == '') { toastr.error('Ingrese monto base'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPIPremioBase').val()) <= 0) { toastr.error('El monto base debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSPremioMinimo').val()) == '') { toastr.error('Ingrese premio minimo'); validacion_cajas_texto = 0; return resp; }
        if (($('#txtPIPremioMinimo').val()) <= 0) { toastr.error('El premio minimo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSPremioMaximo').val()) == '') { toastr.error('Ingrese premio maximo'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPIPremioMaximo').val()) <= 0) { toastr.error('El premio maximo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        if ($.trim($('#txtPSIncPozoNormal').val()) == '') { toastr.error('Ingrese incremento en el pozo normal'); validacion_cajas_texto = 0; return false; }
        if (($('#txtPIIncPozoNormal').val()) <= 0) { toastr.error('El incremento en el pozo normal debe ser mayor a cero'); resp = 0; return false; }
        if ($('#chkPozoOculto').is(":checked")) {
            if ($.trim($('#txtPSMontoOcultoMin').val()) == '') { toastr.error('Ingrese monto oculto minimo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPIMontoOcultoMin').val()) <= 0) { toastr.error('El monto oculto minimo debe ser mayor a cero'); resp = 0; return false; }
            if ($.trim($('#txtPSMontoOcultoMax').val()) == '') { toastr.error('Ingrese monto oculto maximo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPIMontoOcultoMax').val()) <= 0) { toastr.error('El monto oculto maximo debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
            if ($.trim($('#txtPSIncPozoOculto').val()) == '') { toastr.error('Ingrese incremento de pozo oculto maximo'); validacion_cajas_texto = 0; return false; }
            if (($('#txtPIIncPozoOculto').val()) <= 0) { toastr.error('El incremento del pozo oculto debe ser mayor a cero'); validacion_cajas_texto = 0; return false; }
        }
    }
    ManipularCajasTextoPozoSuperior(0);
    ManipularCajasTextoPozoMedio(0);
    ManipularCajasTextoPozoInferior(0);
    GrabarCabecera();
}

function GrabarCabecera() {
    var flag = GuardarHistoricoProgresivo();
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    if (flag) {                
        var lista = new Array();
        lista.push($('#hid_cod_cab_prog').val());
        lista.push($('#txtNroPozos').val());
        lista.push('0');
        lista.push($('#txtNumJugadores').val());
        lista.push($('#cboImagen').val());
        lista.push($('#cboLugarPago').val());
        lista.push($('#txtDuracion').val());
        lista.push($('#txtMoneda').val());
        lista.push($('#cboEstado').val());
        lista.push(($('#chkPozoOculto').is(":checked")) ? '1' : '0');
        lista.push(($('#chkRegHistorico').is(":checked")) ? '1' : '0');
        //var url = getUrlApi("RegistroProgresivo/GuardarCabecera");
        var url = ipPublicaG + "/servicio/GuardarCabecera?codProgresivo=" + PorgresivoId;
        var parametros = JSON.stringify({ lista: lista, url: url });
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Progresivo/ProgresivoGuardarCabeceraJson",
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                result = result.data;
                var v_codigo_cabecera = result;
                $('#hid_cod_cab_prog').val(v_codigo_cabecera);
                var listaPozos = new Array();
                /*POZO SUPERIOR*/
                if ($('#chkPozoSuperior').is(":checked")) {
                    var detalleSup = new Object();
                    detalleSup.ProgresivoID = v_codigo_cabecera;
                    detalleSup.DetalleProgresivoID = $('#hid_cod_det_prog_sup').val();
                    detalleSup.TipoPozo = 1;
                    detalleSup.MontoMin = parseFloat($('#txtPSPremioMinimo').val());
                    detalleSup.MontoBase = parseFloat($('#txtPSPremioBase').val());
                    detalleSup.MontoMax = parseFloat($('#txtPSPremioMaximo').val());
                    detalleSup.IncPozo1 = parseFloat($('#txtPSIncPozoNormal').val());
                    detalleSup.IncPozo2 = 0;
                    detalleSup.MontoOcMin = parseFloat($('#txtPSMontoOcultoMin').val());
                    detalleSup.MontoOcMax = parseFloat($('#txtPSMontoOcultoMax').val());
                    detalleSup.IncOcPozo1 = parseFloat($('#txtPSIncPozoOculto').val());
                    detalleSup.IncOcPozo2 = 0;
                    detalleSup.Estado = 1;
                    detalleSup.Parametro = false;
                    detalleSup.Punto = 0;
                    detalleSup.Prob1 = 0;
                    detalleSup.Prob2 = 0;
                    detalleSup.EstadoInicial = 1;
                    detalleSup.Dificultad = $('#cboPSDificultad').val();
                    detalleSup.RsApuesta = $('#txtPSRsApuesta').val();
                    detalleSup.RsJugadores = $('#txtPSRsJugadores').val();
                    listaPozos.push(detalleSup);
                }
                /*POZO MEDIO*/
                if ($('#chkPozoMedio').is(":checked")) {
                    var detalleMed = new Object();
                    detalleMed.ProgresivoID = v_codigo_cabecera;
                    detalleMed.DetalleProgresivoID = $('#hid_cod_det_prog_med').val();
                    detalleMed.TipoPozo = 2;
                    detalleMed.MontoMin = parseFloat($('#txtPMPremioMinimo').val());
                    detalleMed.MontoBase = parseFloat($('#txtPMPremioBase').val());
                    detalleMed.MontoMax = parseFloat($('#txtPMPremioMaximo').val());
                    detalleMed.IncPozo1 = parseFloat($('#txtPMIncPozoNormal').val());
                    detalleMed.IncPozo2 = 0;
                    detalleMed.MontoOcMin = parseFloat($('#txtPMMontoOcultoMin').val());
                    detalleMed.MontoOcMax = parseFloat($('#txtPMMontoOcultoMax').val());
                    detalleMed.IncOcPozo1 = parseFloat($('#txtPMIncPozoOculto').val());
                    detalleMed.IncOcPozo2 = 0;
                    detalleMed.Estado = 1;
                    detalleMed.Parametro = false;
                    detalleMed.Punto = 0;
                    detalleMed.Prob1 = 0;
                    detalleMed.Prob2 = 0;
                    detalleMed.EstadoInicial = 1;
                    detalleMed.Dificultad = $('#cboPMDificultad').val();
                    detalleMed.RsApuesta = $('#txtPMRsApuesta').val();
                    detalleMed.RsJugadores = $('#txtPMRsJugadores').val();
                    listaPozos.push(detalleMed);
                }
                /*POZO INFERIOR*/
                if ($('#chkPozoInferior').is(":checked")) {
                    var detalleInf = new Object();
                    detalleInf.ProgresivoID = v_codigo_cabecera;
                    detalleInf.DetalleProgresivoID = $('#hid_cod_det_prog_inf').val();
                    detalleInf.TipoPozo = 3;
                    detalleInf.MontoMin = parseFloat($('#txtPIPremioMinimo').val());
                    detalleInf.MontoBase = parseFloat($('#txtPIPremioBase').val());
                    detalleInf.MontoMax = parseFloat($('#txtPIPremioMaximo').val());
                    detalleInf.IncPozo1 = parseFloat($('#txtPIIncPozoNormal').val());
                    detalleInf.IncPozo2 = 0;
                    detalleInf.MontoOcMin = parseFloat($('#txtPIMontoOcultoMin').val());
                    detalleInf.MontoOcMax = parseFloat($('#txtPIMontoOcultoMax').val());
                    detalleInf.IncOcPozo1 = parseFloat($('#txtPIIncPozoOculto').val());
                    detalleInf.IncOcPozo2 = 0;
                    detalleInf.Estado = 1;
                    detalleInf.Parametro = false;
                    detalleInf.Punto = 0;
                    detalleInf.Prob1 = 0;
                    detalleInf.Prob2 = 0;
                    detalleInf.EstadoInicial = 1;
                    detalleInf.Dificultad = $('#cboPIDificultad').val();
                    detalleInf.RsApuesta = $('#txtPIRsApuesta').val();
                    detalleInf.RsJugadores = $('#txtPIRsJugadores').val();
                    listaPozos.push(detalleInf);
                }
                GuardarDetalles(listaPozos);
                return false;
            },
            error: function (request, status, error) {
                toastr.error(request.responseText + " " + error);
                //MostrarMensaje(request.responseText + " " + error);
                return false;
            },
            complete: function (result) {
            }
        });
        return false;
    }   
    else {
        toastr.error("Error","Servidor no Disponible.");
    }
}

function APiModificarPozos() {
    var cant_reg = 0;
    var lista = new Array();
    post = $('#chkPozoSuperior').is(":checked")
    if (post) {
        cant_reg++;
        lista.push($('#txtPSPozoActual').val());
        $('#hid_cod_det_prog_sup').val($('#txtPSPozoActual').val());
    }
    post = $('#chkPozoMedio').is(":checked")
    if (post) {
        cant_reg++;
        lista.push($('#txtPMPozoActual').val());
        $('#hid_cod_det_prog_med').val($('#txtPMPozoActual').val());
    }
    post = $('#chkPozoInferior').is(":checked")
    if (post) {
        cant_reg++;
        lista.push($('#txtPIPozoActual').val());
        $('#hid_cod_det_prog_inf').val($('#txtPIPozoActual').val());
    }
    if (cant_reg == 0) {
        return false;
    }

    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    //var url = getUrlApi("RegistroPozo/GuardarPozo");
    var url = ipPublicaG + "/servicio/GuardarPozo?codProgresivo=" + PorgresivoId;    
    var parametros = JSON.stringify({ lista: lista, url: url });

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Progresivo/ProgresivoPozoInsertarJson",
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",       
        success: function (result) {            
            // var listaResp = result;
            dataAuditoria(1, "#formProgresivo", 1, "Progresivo/ProgresivoPozoInsertarJson", "BOTON MODIFICAR POZO");
            toastr.success('Se grabaron los datos correctamente');
            //MostrarMensaje('Se grabaron los datos correctamente');
            ObtenerProgresivoActivo();
            LimpiarCabecera();
            LimpiarDetalle();
            return false;
        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error);
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        },
        complete: function (resul) {
        }
    });
}

function APiRegistrarPozos(listaResp) {    
    var lista = new Array();
    var cant_reg = listaResp.length;
    for (i = 0; i < cant_reg; i++) {
        var tipo_pozo = listaResp[i].split("_")[0];
        var detalle_cod = listaResp[i].split("_")[1];
        if (tipo_pozo == 1) {
            if($("#chkModificarPozoSuperior").is(':checked')){
                lista.push($('#txtPSPozoActual').val());
                $('#hid_cod_det_prog_sup').val($('#txtPSPozoActual').val());
                localStorage.setItem('chkModificarPozoSuperior','true');
            }
            else{
                lista.push($('#valor_pozo_superior_actual').val());   
                $('#hid_cod_det_prog_sup').val($('#valor_pozo_superior_actual').val());
                localStorage.setItem('chkModificarPozoSuperior','false');
            }
        }
        else if (tipo_pozo == 2) {
            if($("#chkModificarPozoMedio").is(':checked')){
                lista.push($('#txtPMPozoActual').val());
                $('#hid_cod_det_prog_med').val($('#txtPMPozoActual').val());
                localStorage.setItem('chkModificarPozoMedio','true');
            }
            else{
                lista.push($('#valor_pozo_medio_actual').val());
                $('#hid_cod_det_prog_med').val($('#valor_pozo_medio_actual').val());
                localStorage.setItem('chkModificarPozoMedio','false');
            }
           
        }
        else {
            if($("#chkModificarPozoInferior").is(':checked')){
                lista.push($('#txtPIPozoActual').val());
                $('#hid_cod_det_prog_inf').val($('#txtPIPozoActual').val());
                localStorage.setItem('chkModificarPozoInferior','true');
            }
            else{
                lista.push($('#valor_pozo_inferior_actual').val());
                $('#hid_cod_det_prog_inf').val($('#valor_pozo_inferior_actual').val());
                localStorage.setItem('chkModificarPozoInferior','false');
            }
           
        }
    }

    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url = ipPublicaG + "/servicio/GuardarPozo?codProgresivo=" + PorgresivoId;    
    var parametros = JSON.stringify({ lista:lista,url:url });
    //var url = getUrlApi("RegistroPozo/GuardarPozo");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Progresivo/ProgresivoPozoInsertarJson",
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {             
            EnviarCorreo();
            dataAuditoria(1, "#formProgresivo", 1, "Progresivo/ProgresivoGuardarCabeceraJson","BOTON GRABAR");
            toastr.success('Se grabaron los datos correctamente');
            ObtenerProgresivoActivo();
            LimpiarCabecera();
            LimpiarDetalle();
            return false;
        },
        error: function (request, status, error) {           
            toastr.error(request.responseText + " " + error);
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        },
        complete: function (resul) {
        }
    });
}

function GuardarDetalles(listaPozos) {
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url = ipPublicaG + "/servicio/GuardarDetalles?codProgresivo=" + PorgresivoId;    
    var parametros = JSON.stringify({ listaPozos: listaPozos,url:url });    
    //var parametros = JSON.stringify(listaPozos);        
    $.ajax({
        type: "post",
        cache: false,
        //url: url,
        url: basePath + "Progresivo/ProgresivoDetallesGuardarJson",
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {             
            result = result.data;
            //crear metodo para evitar que se guarde el pozo
            // if (RegistrarPozos == 1) {
                APiRegistrarPozos(result);
            // }
            return false;
        },
        error: function (request, status, error) {            
            toastr.error(request.responseText + " " + error);
            //MostrarMensaje(request.responseText + " " + error);
            return false;
        },
        complete: function (resul) {
        }
    });
}

function GuardarHistoricoProgresivo() {
    var obj = {
        ProgresivoActivo:DataHistoricaCabecera,
        PozosActuales:DataHistoricaPozosActuales,
        DetalleProgresivo:DataHistoricaDetallePozos,
    };
    var objStr = JSON.stringify(obj)
    var parametros = JSON.stringify({ objStr: objStr, codSala: SalaId, codProgresivo: PorgresivoId });    
    var state = false;
    $.ajax({
        type: "post",
        cache: false,
        async: false,
        url: basePath + "Progresivo/ProgresivoHistoricoInsertarJson",
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {              
            if (result.data==true) {
                state= true;
            }else {
                state =false;
            }
        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error);
            return false;
        },
        complete: function (resul) {
        }
    }); 
    return state;
}

function EnviarCorreo() {    
    var lista = new Array();
    lista.push($('#hid_cod_cab_prog').val());
    lista.push($('#txtNroPozos').val());
    lista.push('0');
    lista.push($('#txtNumJugadores').val());
    lista.push($('#cboImagen').val());
    lista.push($('#cboLugarPago').val());
    lista.push($('#txtDuracion').val());
    lista.push($('#txtMoneda').val());
    lista.push($('#cboEstado').val());
    lista.push(($('#chkPozoOculto').is(":checked")) ? '1' : '0');
    lista.push(($('#chkRegHistorico').is(":checked")) ? '1' : '0');
    var listaPozosEmail = new Array();
    /*POZO SUPERIOR*/
    if ($('#chkPozoSuperior').is(":checked")) {
        var detalleSup = new Object();
        detalleSup.ProgresivoID = v_codigo_cabecera;
        detalleSup.DetalleProgresivoID = $('#hid_cod_det_prog_sup').val();
        detalleSup.TipoPozo = 1;
        detalleSup.MontoMin = parseFloat($('#txtPSPremioMinimo').val());
        detalleSup.MontoBase = parseFloat($('#txtPSPremioBase').val());
        detalleSup.MontoMax = parseFloat($('#txtPSPremioMaximo').val());
        detalleSup.IncPozo1 = parseFloat($('#txtPSIncPozoNormal').val());
        detalleSup.MontoOcMin = parseFloat($('#txtPSMontoOcultoMin').val());
        detalleSup.MontoOcMax = parseFloat($('#txtPSMontoOcultoMax').val());
        detalleSup.IncOcPozo1 = parseFloat($('#txtPSIncPozoOculto').val());      
        detalleSup.Dificultad = $('#cboPSDificultad').val();
        detalleSup.RsApuesta = $('#txtPSRsApuesta').val();
        detalleSup.RsJugadores = $('#txtPSRsJugadores').val();
        detalleSup.Actual= $('#txtPSPozoActual').val();
        listaPozosEmail.push(detalleSup);
    }
    /*POZO MEDIO*/
    if ($('#chkPozoMedio').is(":checked")) {
        var detalleMed = new Object();
        detalleMed.ProgresivoID = v_codigo_cabecera;
        detalleMed.DetalleProgresivoID = $('#hid_cod_det_prog_med').val();
        detalleMed.TipoPozo = 2;
        detalleMed.MontoMin = parseFloat($('#txtPMPremioMinimo').val());
        detalleMed.MontoBase = parseFloat($('#txtPMPremioBase').val());
        detalleMed.MontoMax = parseFloat($('#txtPMPremioMaximo').val());
        detalleMed.IncPozo1 = parseFloat($('#txtPMIncPozoNormal').val());
        detalleMed.MontoOcMin = parseFloat($('#txtPMMontoOcultoMin').val());
        detalleMed.MontoOcMax = parseFloat($('#txtPMMontoOcultoMax').val());
        detalleMed.IncOcPozo1 = parseFloat($('#txtPMIncPozoOculto').val());
        detalleMed.Dificultad = $('#cboPMDificultad').val();
        detalleMed.RsApuesta = $('#txtPMRsApuesta').val();
        detalleMed.RsJugadores = $('#txtPMRsJugadores').val();
        detalleMed.Actual = $('#txtPMPozoActual').val();
        listaPozosEmail.push(detalleMed);
    }
    /*POZO INFERIOR*/
    if ($('#chkPozoInferior').is(":checked")) {
        var detalleInf = new Object();
        detalleInf.ProgresivoID = v_codigo_cabecera;
        detalleInf.DetalleProgresivoID = $('#hid_cod_det_prog_inf').val();
        detalleInf.TipoPozo = 3;
        detalleInf.MontoMin = parseFloat($('#txtPIPremioMinimo').val());
        detalleInf.MontoBase = parseFloat($('#txtPIPremioBase').val());
        detalleInf.MontoMax = parseFloat($('#txtPIPremioMaximo').val());
        detalleInf.IncPozo1 = parseFloat($('#txtPIIncPozoNormal').val());
        detalleInf.MontoOcMin = parseFloat($('#txtPIMontoOcultoMin').val());
        detalleInf.MontoOcMax = parseFloat($('#txtPIMontoOcultoMax').val());
        detalleInf.IncOcPozo1 = parseFloat($('#txtPIIncPozoOculto').val());
        detalleInf.Dificultad = $('#cboPIDificultad').val();
        detalleInf.RsApuesta = $('#txtPIRsApuesta').val();
        detalleInf.RsJugadores = $('#txtPIRsJugadores').val();
        detalleInf.Actual = $('#txtPIPozoActual').val();
        listaPozosEmail.push(detalleInf);
    }
   
    var obj = {
        ProgresivoActivo: DataHistoricaCabecera,
        PozosActuales: DataHistoricaPozosActuales,
        DetalleProgresivo: DataHistoricaDetallePozos,
    };
    var parametros = JSON.stringify({ obj: obj, Sala: SalaStr, Progresivo: PorgresivoStr, listaPozos: listaPozosEmail, lista: lista});
    var state = false;
    $.ajax({
        type: "post",
        cache: false,
        async: false,
        url: basePath + "Progresivo/ProgresivoCorreoEnviarJson",
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {            
            if (result.data ==true) {
                toastr.success('Se Envio Correo Correctamente');
            }
            else {
                toastr.error('No Se Envio Correo Correctamente');
            }
        },
        error: function (request, status, error) {
            toastr.error(request.responseText + " " + error);
            return false;
        },
        complete: function (resul) {
        }
    });
    return state;
}


// --------------------------------------------------------------------------------
// ----------------------------- VALIDACIONES BASICAS -----------------------------
// --------------------------------------------------------------------------------

function ManipularCajasTextoPozoSuperior(accion) {
    if (accion == 0) { $('#txtPSPremioBase').attr('disabled', 'disabled'); } else { if (!$('#chkPozoOculto').is(":checked")) { $('#txtPSPremioBase').removeAttr('disabled'); } else { $('#txtPSPremioBase').attr('disabled', 'disabled'); } }
    if (accion == 0) { $('#txtPSPremioMinimo').attr('disabled', 'disabled'); } else { $('#txtPSPremioMinimo').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSPremioMaximo').attr('disabled', 'disabled'); } else { $('#txtPSPremioMaximo').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSIncPozoNormal').attr('disabled', 'disabled'); } else { $('#txtPSIncPozoNormal').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSMontoOcultoMin').attr('disabled', 'disabled'); } else { $('#txtPSMontoOcultoMin').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSMontoOcultoMax').attr('disabled', 'disabled'); } else { $('#txtPSMontoOcultoMax').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSIncPozoOculto').attr('disabled', 'disabled'); } else { $('#txtPSIncPozoOculto').removeAttr('disabled'); }
    if (accion == 0) { $('#cboPSDificultad').attr('disabled', 'disabled'); } else { $('#cboPSDificultad').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSRsApuesta').attr('disabled', 'disabled'); } else { $('#txtPSRsApuesta').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSRsJugadores').attr('disabled', 'disabled'); } else { $('#txtPSRsJugadores').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPSPozoActual').attr('disabled', 'disabled') } else { $('#txtPSPozoActual').removeAttr('disabled'); }

}

function ManipularCajasTextoPozoMedio(accion) {
    if (accion == 0) { $('#txtPMPremioBase').attr('disabled', 'disabled'); } else { if (!$('#chkPozoOculto').is(":checked")) { $('#txtPMPremioBase').removeAttr('disabled'); } else { $('#txtPMPremioBase').attr('disabled', 'disabled'); } }
    if (accion == 0) { $('#txtPMPremioMinimo').attr('disabled', 'disabled'); } else { $('#txtPMPremioMinimo').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMPremioMaximo').attr('disabled', 'disabled'); } else { $('#txtPMPremioMaximo').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMIncPozoNormal').attr('disabled', 'disabled'); } else { $('#txtPMIncPozoNormal').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMMontoOcultoMin').attr('disabled', 'disabled'); } else { $('#txtPMMontoOcultoMin').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMMontoOcultoMax').attr('disabled', 'disabled'); } else { $('#txtPMMontoOcultoMax').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMIncPozoOculto').attr('disabled', 'disabled'); } else { $('#txtPMIncPozoOculto').removeAttr('disabled'); }
    if (accion == 0) { $('#cboPMDificultad').attr('disabled', 'disabled'); } else { $('#cboPMDificultad').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMRsApuesta').attr('disabled', 'disabled'); } else { $('#txtPMRsApuesta').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMRsJugadores').attr('disabled', 'disabled'); } else { $('#txtPMRsJugadores').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPMPozoActual').attr('disabled', 'disabled'); } else { $('#txtPMPozoActual').removeAttr('disabled'); }

}

function ManipularCajasTextoPozoInferior(accion) {
    if (accion == 0) { $('#txtPIPremioBase').attr('disabled', 'disabled'); } else { if (!$('#chkPozoOculto').is(":checked")) { $('#txtPIPremioBase').removeAttr('disabled'); } else { $('#txtPIPremioBase').attr('disabled', 'disabled'); } }
    if (accion == 0) { $('#txtPIPremioMinimo').attr('disabled', 'disabled'); } else { $('#txtPIPremioMinimo').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIPremioMaximo').attr('disabled', 'disabled'); } else { $('#txtPIPremioMaximo').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIIncPozoNormal').attr('disabled', 'disabled'); } else { $('#txtPIIncPozoNormal').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIMontoOcultoMin').attr('disabled', 'disabled'); } else { $('#txtPIMontoOcultoMin').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIMontoOcultoMax').attr('disabled', 'disabled'); } else { $('#txtPIMontoOcultoMax').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIIncPozoOculto').attr('disabled', 'disabled'); } else { $('#txtPIIncPozoOculto').removeAttr('disabled'); }
    if (accion == 0) { $('#cboPIDificultad').attr('disabled', 'disabled'); } else { $('#cboPIDificultad').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIRsApuesta').attr('disabled', 'disabled'); } else { $('#txtPIRsApuesta').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIRsJugadores').attr('disabled', 'disabled'); } else { $('#txtPIRsJugadores').removeAttr('disabled'); }
    if (accion == 0) { $('#txtPIPozoActual').attr('disabled', 'disabled'); } else { $('#txtPIPozoActual').removeAttr('disabled'); }

}

function LimpiarCabecera() {
    $('#txtMoneda').val('S/.');
    $('#txtDuracion').val('0');
    $('#cboLugarPago')[0].selectedIndex = 0;
    $('#txtNumJugadores').val('0');
    $('#cboImagen')[0].selectedIndex = 0;
    $('#txtNroPozos').val('0');
    $('#cboEstado')[0].selectedIndex = 0;
    $('#chkPozoOculto').iCheck('uncheck');
    $('#chkRegHistorico').iCheck('uncheck');
}

function LimpiarDetalle() {
    $('#chkPozoSuperior').prop('checked', false);
    $('#chkPozoMedio').prop('checked', false);
    $('#chkPozoInferior').prop('checked', false);
    $('#txtPSPremioBase').val('0');
    $('#txtPMPremioBase').val('0');
    $('#txtPIPremioBase').val('0');
    $('#txtPSPremioMinimo').val('0');
    $('#txtPMPremioMinimo').val('0');
    $('#txtPIPremioMinimo').val('0');
    $('#txtPSPremioMaximo').val('0');
    $('#txtPMPremioMaximo').val('0');
    $('#txtPIPremioMaximo').val('0');
    $('#txtPSIncPozoNormal').val('0');
    $('#txtPMIncPozoNormal').val('0');
    $('#txtPIIncPozoNormal').val('0');
    $('#txtPSMontoOcultoMin').val('0');
    $('#txtPMMontoOcultoMin').val('0');
    $('#txtPIMontoOcultoMin').val('0');
    $('#txtPSMontoOcultoMax').val('0');
    $('#txtPMMontoOcultoMax').val('0');
    $('#txtPIMontoOcultoMax').val('0');
    $('#txtPSIncPozoOculto').val('0');
    $('#txtPMIncPozoOculto').val('0');
    $('#txtPIIncPozoOculto').val('0');
    $("#cboPSDificultad")[0].selectedIndex = 0;
    $("#cboPMDificultad")[0].selectedIndex = 0;
    $("#cboPIDificultad")[0].selectedIndex = 0;
    $('#txtPSPozoActual').val('0');
    $('#txtPMPozoActual').val('0');
    $('#txtPIPozoActual').val('0');
    $('#txtPSRsApuesta').val('0');
    $('#txtPMRsApuesta').val('0');
    $('#txtPIRsApuesta').val('0');
    $('#txtPSRsJugadores').val('0');
    $('#txtPMRsJugadores').val('0');
    $('#txtPIRsJugadores').val('0');
}

// --------------------------------------------------------------------------------
// ------------------------- URL acceso servicio Progresivo -----------------------
// --------------------------------------------------------------------------------
function getUrlApi(controler) {
    console.log("http://" + getHost() + ":8888/api/" + controler);
    return "http://" + getHost() + ":8888/api/" + controler;
}
function getHost() {
    var loc = window.location;
    var host1 = loc.host.substring(0, loc.host.lastIndexOf(':'));
    return host1;
}
function getUrlApiAparameter(controler, parameter) {

    return "http://" + window.location.hostname + ":8888/api/" + controler + parameter;
}
function getToken() {
    //var ptoken = sessionStorage.getItem("tokenKey");
    var ptoken = "aWNjd3Jrbmo6Li4xOkNocm9tZSA2Ni4wLjMzNTk6aWNjd3Jrbmo=";
    return ptoken;
}

function getHeader() {
    var token1 = getToken();
    var headers = {};
    if (token1)
        headers = { 'X-Auth-Token': token1 };
    return headers;
}

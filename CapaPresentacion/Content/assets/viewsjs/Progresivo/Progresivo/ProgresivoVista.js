let consultaPorVpn=false
let ipPublica
let ipPrivada
let ipPublicaAlterna
let seEncontroAlterna=false
let urlsResultado=[]
let delay=60000
let timerId=''
$(document).ready(function () {
    $("#cboSala").select2()
    $("#cboProgresivo").select2()
    $("#table").parent().addClass("tabla__ganadores")
    hiddenTablaGanadores(true)

    clearTimeout(timerId)
    timerId = setTimeout(function request() {
        getPingSalas().then(result=>{
            urlsResultado=result
        })
        timerId = setTimeout(request, delay);
    }, delay)


    obtenerListaSalas().then(result=>{
        if(result.data){
            renderSelectSalas(result.data)
            getPingSalas().then(response=>{
                urlsResultado=response
            })
        }
    })
  
    $(document).on('change', '#cboSala', function (e) {
        e.preventDefault()
        if($("#cboSala").val()==0){
            return false
        }
        if(urlsResultado.length==0){
            toastr.warning("Inténtelo de nuevo en unos momentos",'Mensaje Servidor')
            $("#cboSala").val('0').trigger('change')
            return false
        }
      
        toastr.remove()

        hiddenTablaGanadores(true)
        renderBuscarGanadores([])
        resetFiltroGanadores()

        $("#cboProgresivo").html(`<option value="">--Seleccione--</option>`);
        ipPublica = $(this).val();
        ipPrivada=$("#cboSala option:selected").data('ipprivada')
        let puertoServicio=$("#cboSala option:selected").data('puertoservicio')
        ipPrivada=ipPrivada+':'+puertoServicio
        let uri=getUrl(ipPublica)
        const obj=urlsResultado.find(item=>item.uri==ipPublica)
        if(uri&&obj.respuesta){
            let urlPublica=ipPublica+'/servicio/listadoprogresivos'
            consultaPorVpn = false

            getProgresivos(urlPublica);

        }
        else{
            consultaPorVpn=true
            // ipPublicaAlterna=urlsResultado.find(x=>x.respuesta)
            ipPublicaAlterna = getUrl("http://190.187.44.222:9895")
            let urlPrivada=ipPrivada+'/servicio/listadoprogresivos'
            let urlPublica = ipPublicaAlterna + "/servicio/listadoprogresivosVpn"

            getProgresivosVpn(urlPrivada, urlPublica)

        }
    })
    $(document).on('click','#btnBuscar',function(e){
        e.preventDefault()
        if(consultaPorVpn){
            //buscarGanadoresVpn
            let urlPrivada=ipPrivada+'/servicio/ganadoresjson/'+$("#cboProgresivo").val()
            let urlPublica = ipPublicaAlterna + '/servicio/ganadoresjsonvpn';

            getGanadoresVpn(urlPrivada, urlPublica);
            /*
            getGanadoresVpn(urlPrivada,urlPublica).then(result=>{
                $.LoadingOverlay('show')
                let maquina=$("#txtMaquina").val()
                let pozo=$("#cboPozos").val()
                let dataFinal=result.data
                if(maquina!=""){
                    dataFinal=response.filter(x=>x.SlotID==maquina)
                }
                if(pozo!="0"){
                    dataFinal=response.filter(x=>x.TipoPozo==pozo)
                }
                if(maquina!=""&&pozo!="0"){
                    dataFinal=response.filter(x=>x.SlotID==maquina&&x.TipoPozo==pozo)
                }
                renderBuscarGanadores(dataFinal)
                hiddenTablaGanadores(false)
                $.LoadingOverlay('hide')
            });
            */
        }
        else{
            let urlPublica = ipPublica + "/servicio/ganadoresjson/" + $("#cboProgresivo").val();
            getGanadores(urlPublica);
            /*
            getGanadores(urlPublica).then(result=>{
                $.LoadingOverlay('show')
                let maquina=$("#txtMaquina").val()
                let pozo=$("#cboPozos").val()
                let dataFinal=result.data
                if(maquina!=""){
                    dataFinal=response.filter(x=>x.SlotID==maquina)
                }
                if(pozo!="0"){
                    dataFinal=response.filter(x=>x.TipoPozo==pozo)
                }
                if(maquina!=""&&pozo!="0"){
                    dataFinal=response.filter(x=>x.SlotID==maquina&&x.TipoPozo==pozo)
                }
                renderBuscarGanadores(dataFinal)
                hiddenTablaGanadores(false)
                $.LoadingOverlay('hide')
            });
            */
        }
    })
    $(document).on('click','.btnVer',function(e){
        e.preventDefault()
        let RegD=$("#RegD").val()
        let RegA=$("#RegA").val()
        let Fecha= moment($(this).attr("data-Fecha")).format("DD-MM-YYYY HH:mm:ss");
        let maquina = $(this).attr("data-maquina"); 
        if(consultaPorVpn){
            let urlPublica=`${ipPublicaAlterna}/servicio/DetallesContadoresPremioVpn`
            let urlPrivada=`${ipPrivada}/servicio/DetallesContadoresPremio/${Fecha}/${maquina}/${RegA}/${RegD}`
            getDetalleContadoresPremioVpn(urlPrivada,urlPublica).then(response=>{
                if(response.data.length>0){
                    renderDetalleContadoresPremio(response.data)
                }
            })
        }
        else{
            let urlPublica=`${ipPublica}/servicio/DetallesContadoresPremio/${Fecha}/${maquina}/${RegA}/${RegD}`
            getDetalleContadoresPremio(urlPublica).then(response=>{
                if(response.data.length>0){
                    renderDetalleContadoresPremio(response.data)
                }
            })
        }
    })
    $(document).on('click', '.btnVerContadores', function (e) {  
        e.preventDefault()
        let fecha= $(this).data("fecha");
        let maquina = $(this).data("maquina"); 
        $("#txtFechaHidden").val(fecha)
        $("#txtMaquinaHidden").val(maquina)
        $("#full-modal-contadores").modal('show')
    })
    $("#full-modal-contadores").on("shown.bs.modal", function () {
        let fecha=$("#txtFechaHidden").val()
        let maquina=$("#txtMaquinaHidden").val()
        let _fecha=new Date(moment(fecha).format())
       
      
        $("#txtFechaInicio").datetimepicker({
            format:'DD-MM-YYYY HH:mm',
            formatTime:'HH:mm',
            formatDate:'DD-MM-YYYY',
        });
        $("#txtFechaFin").datetimepicker({
            format:'DD-MM-YYYY HH:mm',
            formatTime:'HH:mm',
            formatDate:'DD-MM-YYYY',
        });
        $("#txtFechaInicio").data("DateTimePicker").setDate(_fecha)
        $("#txtFechaFin").data("DateTimePicker").setDate(_fecha)
   

        $("#tableContadores").empty()
        $(".myDataTable").empty()
        $(".myDataTable").html(`<table class="table table-striped table-hover table-condensed" id="tableContadores"></table>`)

        $("#txtCodMaq").val(maquina) 
        $('table').css('width', '100%');
        $('.dataTables_scrollHeadInner').css('width', '100%');
        $('.dataTables_scrollFootInner').css('width', '100%');
    })
    $(document).on('click','#btnBuscarContadores',function(e){
        e.preventDefault()
   
        if(consultaPorVpn){
            let urlPublica=`${ipPublicaAlterna}/servicio/DetallesContadoresVpn`
            // let urlPrivada=`${ipPrivada}/servicio/DetallesContadores`
            let urlPrivada=`${ipPrivada.replace('9895','8081')}/online/reporte/DetallesContadores`
            getDetalleContadoresVpn(urlPrivada,urlPublica).then(response=>{
                if(response.data.length>0){
                    renderDetalleContadores(response.data)
                }
            })
        }
        else{
            // let urlPublica=`${ipPublica}/servicio/DetallesContadores`
            let urlPublica=`${ipPublica.replace('9895','8081')}/online/reporte/DetallesContadores`
            getDetalleContadores(urlPublica).then(response=>{
                if(response.data.length>0){
                    renderDetalleContadores(response.data)
                }
            })
        }
    })

});
VistaAuditoria("Progresivo/ProgresivoVista", "VISTA", 0, "", 1);


function hiddenTablaGanadores(hidden) {
    if (hidden) {
        $(".tabla__ganadores").addClass("hidden")
    }
    else {
        $(".tabla__ganadores").removeClass("hidden")
    }
}
function resetFiltroGanadores() {
    $("#txtMaquina").val("")
    $("#cboPozos").prop("selectedIndex", 0)
}
function getUrl(url){
    if(url){
        try{
            let uri=new URL(url)
            return uri

        }catch(ex){
            return false
        }
    }
    return false
}
/**Salas */
function echoPingSalas(hostName)
{
    if(hostName.length>0){
        return $.ajax({
            type: "POST",
            cache: false,
            url: basePath +"Progresivo/EchoPingSalas",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ips: hostName }),
            beforeSend: function (xhr) {
            },
            success: function (response) {             
                return response
            },
            error: function (request, status, error) {            
            },
            complete: function (resul) {
            }
        });
    }
    return false
}
function obtenerListaSalas() {
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
            return result;
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
    return ajaxhr
}
function renderSelectSalas(data){
    $.each(data, function (index, value) {
        $("#cboSala").append(`<option value="${value.UrlProgresivo==""?"":value.UrlProgresivo}" data-id="${value.CodSala}" data-ipprivada="${value.IpPrivada}" data-puertoservicio="${value.PuertoServicioWebOnline}">${value.Nombre}</option>`)
    });
}
/**End Salas */
/**Listado Progresivos */
function getProgresivos(urlPublica){
    if (urlPublica) {

        ajaxhr =  $.ajax({
            type: "POST",
            cache: false,
            url: urlPublica + "/servicio/listadoprogresivos",
            contentType: "application/json: charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay('show')
            },
            success: function (response) {
                if (response.length > 0) {
                    renderSelectProgresivos(response)
                }
                else {
                    toastr.error("No se encontro informacion", "Mensaje Servidor")
                }
            },
            complete: function (xhr) {
                AbortRequest.close()
                $.LoadingOverlay('hide')
            }
        })

        AbortRequest.open();
    }
    return false
}
function getProgresivosVpn(urlPrivada, urlPublica) {
    ajaxhr = $.ajax({
        data: JSON.stringify({ urlPrivada: urlPrivada, urlPublica: urlPublica }),
        type: "POST",
        cache: false,
        url: basePath + '/Progresivo/listadoprogresivosVpn',
        contentType: "application/json: charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay('show')
        },
        success: function (response) {
            if (response.length > 0) {
                renderSelectProgresivos(response)
            }
        },
        complete: function (xhr) {
            AbortRequest.close()
            $.LoadingOverlay('hide')
        }
    });

    AbortRequest.open();
   
}
function renderSelectProgresivos(data){
    $("#cboProgresivo").html("")
    if(data){
        $("#cboProgresivo").append('<option value="">--Seleccione--</option>');      
        $.each(data, function (index, value) {  
            $("#cboProgresivo").append(`
                <option 
                    value="${value["WEB_PrgID"]}" 
                    data-id="${value["WEB_Url"]}">
                    ${value["WEB_Nombre"]}
                </option>`)                     
        }); 
    }
}
/**End Listado Progresivos */
/**Ganadores */
function getGanadores(urlPublica){

    renderBuscarGanadores([])

    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: basePath +"Progresivo/ProgresivoGanadoresListadoJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: urlPublica }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {             

            $.LoadingOverlay('show')
            let maquina = $("#txtMaquina").val()
            let pozo = $("#cboPozos").val()
            let dataFinal = result.data
            if (maquina != "") {
                dataFinal = response.filter(x => x.SlotID == maquina)
            }
            if (pozo != "0") {
                dataFinal = response.filter(x => x.TipoPozo == pozo)
            }
            if (maquina != "" && pozo != "0") {
                dataFinal = response.filter(x => x.SlotID == maquina && x.TipoPozo == pozo)
            }
            renderBuscarGanadores(dataFinal)
            hiddenTablaGanadores(false)
            $.LoadingOverlay('hide')
        },
        error: function (request, status, error) {            
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open();
}
function getGanadoresVpn(urlPrivada, urlPublica) {

    renderBuscarGanadores([])

    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: basePath +"Progresivo/ProgresivoGanadoresListadoJsonVpn",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ urlPrivada: urlPrivada,urlPublica:urlPublica }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {             

            $.LoadingOverlay('show')
            let maquina = $("#txtMaquina").val()
            let pozo = $("#cboPozos").val()
            let dataFinal = result.data
            if (maquina != "") {
                dataFinal = response.filter(x => x.SlotID == maquina)
            }
            if (pozo != "0") {
                dataFinal = response.filter(x => x.TipoPozo == pozo)
            }
            if (maquina != "" && pozo != "0") {
                dataFinal = response.filter(x => x.SlotID == maquina && x.TipoPozo == pozo)
            }
            renderBuscarGanadores(dataFinal)
            hiddenTablaGanadores(false)
            $.LoadingOverlay('hide')
        },
        error: function (request, status, error) {            
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open();
}
function renderBuscarGanadores(data) {

    var dataFinal = data

    var RegA = $("#RegA").val();
    var RegD = $("#RegD").val();

    objetodatatable = $("#table").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": false,
        "paging": true,
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "aaSorting": [],     
        data: dataFinal,
        columns: [
            //{ data: "ProgresivoID", title: "Index" },
            {
                data: "TipoPozo", title: "Pozo",
                "render": function (value) {
                    var pozo = "";
                    if (value == 1) {
                        pozo = "Superior";
                    }
                    if (value == 2) {
                        pozo = "Medio ";
                    }
                    if (value == 3) {
                        pozo = "Inferior";
                    }
                    return pozo;
                }
            },
            { data: "SlotID", title: "Maquina" },
            {
                data: "Fecha", title: "Fecha",
                "render": function (value) {
                    return moment(value).format('DD/MM/YYYY');
                }
            },
            {
                data: "Fecha", title: "Hora",
                "render": function (value) {
                    return moment(value).format('h:mm:ss a');
                }
            },
            { data: "Monto", title: "Monto" },
            {
                data: null,
                "bSortable": false,
                "render": function (value) {
                    var botones = '<button type="button" class="btn btn-xs btn-success btnVer" data-toggle="modal" data-target="#full-modal" data-RegD="' + RegD + '" data-RegA="' + RegA + '" data-Fecha="' + value.Fecha + '" data-maquina="' + value.SlotID + '" data-id="' + value.ProgresivoID + '"><i class="glyphicon  glyphicon-search"></i></button>';
                    botones+= '<button type="button" class="btn btn-xs btn-danger btnVerContadores" data-fecha="' + value.Fecha + '" data-maquina="' + value.SlotID + '" data-id="' + value.ProgresivoID + '"><i class="glyphicon  glyphicon-calendar"></i></button>';
                    return botones;
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

}
/**End Ganadores */
/**DetalleContadoresPremio (boton de la LUPA) */
function getDetalleContadoresPremio(urlPublica){
    // let urlPublica=`${ipPublica}/servicio/DetallesContadoresPremio`
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,            
        url: basePath + "Progresivo/ProgresivoGanadoresDetalleContadoresListadoJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json", 
        data: JSON.stringify({ url: urlPublica }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (responseData) {
            return responseData
        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");            
        }
    })
    AbortRequest.open()
    return ajaxhr
}
function getDetalleContadoresPremioVpn(urlPrivada,urlPublica){
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,            
        url: basePath + "Progresivo/ProgresivoGanadoresDetalleContadoresListadoJsonVpn",
        contentType: "application/json; charset=utf-8",
        dataType: "json", 
        data: JSON.stringify({ urlPublica: urlPublica,urlPrivada:urlPrivada }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (responseData) {
            return responseData
        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");            
        }
    })
    AbortRequest.open()
    return ajaxhr
}
function renderDetalleContadoresPremio(response){
    if(response){
        response[response.length - 1].Dif_Bonus1 = 0;
        response[response.length - 1].Dif_Bonus2 = 0;
        objetodatatabledetalle = $("#tableDetalle").DataTable({
            "bDestroy": true,
            "bSort": true,
            "scrollCollapse": true,
            "scrollX": false,
            "paging": true,
            "autoWidth": false,
            "bProcessing": true,
            "bDeferRender": true,
            "aaSorting": [],
            data: response,
            columns: [
                {
                    data: "Fecha", title: "Fecha",
                    "render": function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
                {
                    data: "Hora", title: "Hora",
                    "render": function (value) {
                        return moment(value).format('h:mm:ss a');
                    }
                },
                { data: "CodMaq", title: "Maquina" },
                { data: "codevento", title: "Evento" },
                { data: "Bonus1", title: "Bonus 1" },
                { data: "Bonus2", title: "Bonus 2" },
                {
                    data: "Dif_Bonus1", title: "Bonus 1 Diferencia",
                    "render": function (value) {
                        if (value != 0) {
                            return "<p style=background-color:red;color:white;>" + value + "</p>";
                        }
                        else {
                            return value;
                        }
                    }
                },
                {
                    data: "Dif_Bonus2", title: "Bonus 2 Diferencia",
                    "render": function (value) {
                        if (value != 0) {
                            return "<p style=background-color:red;color:white;>" + value + "</p>";
                        }
                        else {
                            return value;
                        }
                    }
                },
            ],
            bSort: false,
            columnDefs: [
                {
                    targets: [0, 1,2,3,4,5,6],   //first name & last name
                    orderable: false
                },
            ],
            "initComplete": function (settings, json) {
            },
        });
    }
}
/**End DetalleContadoresPremio */
/**DetalleContadores(Contadores_Online BD Tecnologias) */
function getDetalleContadores(urlPublica){
    let codMaq=$("#txtCodMaq").val()
    let fechaInicio=$("#txtFechaInicio").val()
    let fechaFin=$("#txtFechaFin").val()
    let data={
        codMaq:codMaq,
        fechaInicio:fechaInicio,
        fechaFin:fechaFin,
        url:urlPublica
    }
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,            
        url: basePath + "Progresivo/ProgresivoDetalleContadoresListadoJsonV2",
        contentType: "application/json; charset=utf-8",
        dataType: "json", 
        data: JSON.stringify(data),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (responseData) {
            return responseData;
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");            
        }
    });
    AbortRequest.open()
    return ajaxhr
}
function getDetalleContadoresVpn(urlPrivada,urlPublica){
    let codMaq=$("#txtCodMaq").val()
    let fechaInicio=$("#txtFechaInicio").val()
    let fechaFin=$("#txtFechaFin").val()
    let data={
        codMaq:codMaq,
        fechaInicio:fechaInicio,
        fechaFin:fechaFin,
        urlPrivada:urlPrivada,
        urlPublica:urlPublica
    }
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,            
        url: basePath + "Progresivo/ProgresivoDetalleContadoresListadoJsonVpn",
        contentType: "application/json; charset=utf-8",
        dataType: "json", 
        data: JSON.stringify(data),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (responseData) {
            return responseData;
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");            
        }
    });
    AbortRequest.open()
    return ajaxhr
}
function renderDetalleContadores(response){
    if(response.length>0){
        response[response.length - 1].Dif_Bonus1 = 0;
        response[response.length - 1].Dif_Bonus2 = 0;
        objetodatatabledetalle = $("#tableContadores").DataTable({
            "bDestroy": true,
            "bSort": true,
            "scrollCollapse": true,
            "scrollX": false,
            "paging": true,
            "autoWidth": false,
            "bProcessing": true,
            "bDeferRender": true,
            "aaSorting": [],
            data: response,
            columns: [
                {
                    data: "Fecha", title: "Fecha",
                    "render": function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
                {
                    data: "Hora", title: "Hora",
                    "render": function (value) {
                        return moment(value).format('h:mm:ss a');
                    }
                },
                { data: "CodMaq", title: "Maquina" },
                { data: "codevento", title: "Evento" },
                { data: "Bonus1", title: "Bonus 1" },
                { data: "Bonus2", title: "Bonus 2" },
                {
                    data: "Dif_Bonus1", title: "Bonus 1 Diferencia",
                    "render": function (value) {
                        if (value != 0) {
                            return "<p style=background-color:red;color:white;>" + value + "</p>";
                        }
                        else {
                            return value;
                        }
                    }
                },
                {
                    data: "Dif_Bonus2", title: "Bonus 2 Diferencia",
                    "render": function (value) {
                        if (value != 0) {
                            return "<p style=background-color:red;color:white;>" + value + "</p>";
                        }
                        else {
                            return value;
                        }
                    }
                },
                { data: "CurrentCredits", title: "CurrentCredits" },

            ],
            bSort: false,
            columnDefs: [
                {
                    targets: [0, 1,2,3,4,5,6],   //first name & last name
                    orderable: false
                },
            ],
            "initComplete": function (settings, json) {
            },
        });
    }
}
/**End DetalleContadores */
function getPingSalas(){
    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath +"Progresivo/EchoPingSalasUsuario",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
        },
        success: function (response) {             
            return response
        },
        error: function (request, status, error) {            
        },
        complete: function (resul) {
        }
    });
}
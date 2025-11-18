const LIMITE_CREDITOS=5
const LIMITE_MINUTOS=300//5 horas
let  urlsPrueba=['http://192.168.0.100/','http://192.168.0.113']
let consultaPorVpn=false
let ipPublica
let ipPrivada
let ipPublicaAlterna
let seEncontroAlterna=false
let urlsResultado=[]
let delay=60000
let timerId=''

$(document).ready(function () {
    ipPublicaG = "";
    id_progresivo = "";
    ipProgresivo=''
    $("#cboSala").select2();
    $("#cboProgresivo").select2();
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
        $("#cboProgresivo").html("");
        $('.contenedor_tabla').empty();

        id_progresivo = "";

        var ipPublica = $(this).val();
        ipPublicaG = ipPublica;
        ipPrivada=$("#cboSala option:selected").data('ipprivada')
        let puertoServicio=$("#cboSala option:selected").data('puertoservicio')

        ipPrivada=ipPrivada+':'+puertoServicio
        let uri=getUrl(ipPublica)
        const obj = urlsResultado.find(item => item.uri == ipPublica)

        toastr.remove()

        if (!obj) {
            toastr.warning('Por favor, seleccione una sala o espere un momento')

            return false
        }

        if(uri&&obj.respuesta){
            let urlPublica=ipPublica+'/servicio/listadoprogresivos'
            consultaPorVpn=false
            llenarSelectAPIProgresivo__(urlPublica, {}, "cboProgresivo", "WEB_PrgID", "WEB_Nombre");
        }
        else{
            consultaPorVpn=true
              // ipPublicaAlterna=urlsResultado.find(x=>x.respuesta)
              //ipPublicaAlterna=getUrl("http://localhost:9895")
              ipPublicaAlterna = getUrl("http://190.187.44.222:9895")
              let urlPrivada=ipPrivada+'/servicio/listadoprogresivos'
              let urlPublica=ipPublicaAlterna+'/servicio/listadoprogresivosVpn'
              getProgresivosVpn(urlPrivada,urlPublica).then(response=>{
                if(response.length>0){
                    renderSelectProgresivos(response)
                }
              })

        }
    });

    $(document).on('change', '#cboProgresivo', function () {
        id_progresivo = $(this).find(':selected').attr('data-id');
    });

    $("#btnBuscar").click(function () {
        if ($("#cboSala").val() == "" || !$("#cboSala").val()) {
            toastr.warning("Seleccione una Sala");
            return false;
        }
        if ($("#cboProgresivo").val() == "" || !$("#cboProgresivo").val()) {
            toastr.warning("Seleccione un Progresivo");
            return false;
        }

        var Progresivo = $("#cboProgresivo").val();
        let PuertoSignalr=$("#cboSala :selected").data('puertosignalr')
        let CodSala=$("#cboSala :selected").data('id')
        let UrlProgresivo=$("#cboProgresivo :selected").data('urlprogresivo')
        BuscarMaquinas(Progresivo,PuertoSignalr,CodSala,UrlProgresivo);
        
    });

    $("#btnNuevo").click(function () {
        console.log(consultaPorVpn)
        if ($("#cboProgresivo").val() == "" || !$("#cboProgresivo").val()) {
            toastr.warning("Seleccione un Progresivo");
            return false;
        }
        let uriRedireccion=`${basePath}Progresivo/ProgresivoRegistrarMaquina`
        if(consultaPorVpn){
            uriRedireccion+=`?id=${id_progresivo}&url=${ipPublicaG}&consultaVpn=${consultaPorVpn}&urlPrivada=${ipPrivada}&urlPublica=${ipPublicaAlterna}`

            // window.location.replace(uriRedireccion)
            // window.location.replace(basePath + "Progresivo/ProgresivoRegistrarMaquina?id=" + id_progresivo + "&url=" + ipPublicaG);
        }
        else{
            uriRedireccion+=`?id=${id_progresivo}&url=${ipPublicaG}`

          
            // window.location.replace(basePath + "Progresivo/ProgresivoRegistrarMaquina?id=" + id_progresivo + "&url=" + ipPublicaG);
        }
        window.location.href = uriRedireccion
        // console.log(uriRedireccion)
    });

    $(document).on('click', '.btnEditar', function () {
        if ($("#cboProgresivo").val() == "" || !$("#cboProgresivo").val()) {
            toastr.error("Seleccione un Progresivo");
            return false;
        }
        var id_maquina = $(this).data("id");
        var url = ipPublicaG;

        if(consultaPorVpn){
            let uriRedireccion=`${basePath}Progresivo/ProgresivoEditarMaquina?id=${id_maquina}&url=${url}&codProgresivo=${id_progresivo}&consultaVpn=${consultaPorVpn}&urlPrivada=${ipPrivada}&urlPublica=${ipPublicaAlterna}`
            window.location.href = uriRedireccion
        }
        else{
            window.location.href = basePath + "Progresivo/ProgresivoEditarMaquina?id=" + id_maquina + "&url=" + url + "&codProgresivo=" + id_progresivo;
        }
    });

    $(document).on('click','.btnAumentarCreditos',function(e){
        e.preventDefault()
        let puertoSignalr=$(this).data('puertosignalr')
        let codMaq=$(this).data('codmaq')
        let codSala=$(this).data('codsala')
        ipProgresivo=$(this).data('urlprogresivo')
        let maquina=$(this).data('maquina')
        if(puertoSignalr&&codMaq){
            obtenerUltimoAumento(codMaq,codSala).then(item=>{
                if(item.Id==0){
                    let itemDefault={
                        Id:0,
                        CodMaq:codMaq,
                        CodSala:codSala,
                        Cantidad:0,
                        PuertoSignalr:puertoSignalr,
                        FechaUltimoAumento:new Date()
                    }
                    item=itemDefault
                }
                ingresarDataInputs(item,maquina)
                $("#full-modalAumentarCreditos").modal('show')
            })
        }
    })
    $(document).on('click','#btnCancelarAumento',function(e){
        e.preventDefault()
        $("#full-modalAumentarCreditos").modal('hide')
    })
    $(document).on('click','#btnAumentar',function(e){
        e.preventDefault()
        let maquina=$("#CodMaquinaProgresivo").val()
        let cantidadActual=$("#AumentoCantidad").val()
        let cantidadIngresada=$("#CantidadIngresada").val()
        let aumentoId=$("#AumentoId").val()
        let fechaActual=moment(new Date()).format()

        let aumentoCodMaq=$("#AumentoCodMaq").val()
        let aumentoCodSala=$("#AumentoCodSala").val()
        let aumentoPuertoSignalr=$("#AumentoPuertoSignalr").val()
        let urlRegistroAumento=''
        let dataRegistroAumento=null
        let pasaValidacion=true
        //validacion de montos
        if(cantidadActual>=LIMITE_CREDITOS){
            let fechaInicio=$("#AumentoFechaUltimoAumento").val()
            let fechaFin=fechaActual
            let diffInMinutes=moment(fechaFin).diff(moment(fechaInicio),'minutes')
            let diffInSeconds=moment(fechaFin).diff(moment(fechaInicio),'seconds')
            if(diffInMinutes<=LIMITE_MINUTOS){
                pasaValidacion=false
                // toastr.error('No puede realizar un aumento de creditos hasta dentro de ' + parseInt(LIMITE_MINUTOS-diffInMinutes) + ' minutos')
                let limiteEnSegundos=LIMITE_MINUTOS*60
                toastr.error('No puede realizar un aumento de creditos hasta dentro de '+convertirHora(limiteEnSegundos-diffInSeconds)+' horas','Mensaje')
                return false
            }
        }
        if(cantidadActual<=LIMITE_CREDITOS&&cantidadActual>0){
            urlRegistroAumento='Progresivo/ActualizarCantidadEnAumentoCreditoMaquina'
            dataRegistroAumento={
                Id :aumentoId,
                CodMaq :aumentoCodMaq,
                Cantidad :cantidadIngresada,
                CodSala :aumentoCodSala,
                PuertoSignalr:aumentoPuertoSignalr
            }
        }
        if(cantidadActual==0){
            aumentoId=0
            urlRegistroAumento='Progresivo/InsertarAumentoCreditoMaquina'
            dataRegistroAumento={
                Id :aumentoId,
                CodMaq :aumentoCodMaq,
                Cantidad :cantidadIngresada,
                CodSala :aumentoCodSala,
                PuertoSignalr:aumentoPuertoSignalr
            }
        }
        pasaValidacion==pasaValidacion?validarURL(ipProgresivo):pasaValidacion
        if(pasaValidacion){
            // let ipSignalr=obtenerUrlSignalr(ipProgresivo,aumentoPuertoSignalr)
            let ipSignalr=ipProgresivo
            //let ipPublicaProgresivo='http://localhost:9895/servicio/AumentarCreditoMaquina'
            let ipPublicaProgresivo=ipPublicaG+'/servicio/AumentarCreditoMaquina'
            let urlAumentoCreditoIAS='Progresivo/AumentarCreditoMaquinaEnServicio'
            let dataAumentoCredito={
                IdProgresivo:id_progresivo,
                CodMaquina:aumentoCodMaq,
                UrlProgresivoAumento:ipPublicaProgresivo,
                IpSignalr:ipSignalr
            }
            aumentarCreditoMaquina(dataAumentoCredito,urlAumentoCreditoIAS).then(result=>{
                if(result){
                    toastr.success('Se registro aumento un aumento de creditos')
                    registrarMovimietoEnCreditosMaquina(dataRegistroAumento,urlRegistroAumento).then(res=>{
                        if(res.respuesta){
                            ingresarDataInputs(res.data,maquina)
                        }
                    })
                }
                else{
                    toastr.error("Ocurrio un error")
                }
            })
        }
    })
});

function BuscarMaquinas(Progresivo,PuertoSignalr,CodSala,UrlProgresivo) {
    console.log(consultaPorVpn)
    let dataForm={}
    let urlConsulta=basePath
    if(consultaPorVpn){
        dataForm={
            urlPublica:ipPublicaAlterna+'/servicio/ListarMaquinasVpn',
            urlPrivada:ipPrivada+'/servicio/ListarMaquinas?codProgresivo=' + Progresivo + '&maquina=&estado=-1'
        }
        urlConsulta+="Progresivo/ConsultarListaMaquinaProgresivoVpn"
    }
    else{
        dataForm={
            url:ipPublicaG + "/servicio/ListarMaquinas?codProgresivo=" + Progresivo + "&maquina=&estado=-1"
        }
        urlConsulta+="Progresivo/ConsultarListaMaquinaProgresivo"
    }
    var addtabla = $('.contenedor_tabla');
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: urlConsulta,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $(addtabla).empty();

            $.LoadingOverlay("show");
        },
        success: function (response) {
            var msj = "";
            msj = response.mensaje != null ? response.mensaje : '';
            response = response.data;
            $(addtabla).empty();
            $(addtabla).append('<table id="table1" class="table table-condensed table-bordered table-hover"></table>');

            if (msj != "") {
                $(addtabla).empty();

                toastr.error("Error De Conexion, Servidor no Encontrado.");
            } else {
                dataAuditoria(1, "#formfiltro", 3, "Progresivo/ConsultarListaMaquinaProgresivo", "BOTON BUSCAR");
                objetodatatable = $("#table1").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "scrollCollapse": true,
                    "scrollX": true,
                    "paging": true,
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response,
                    columns: [
                        { data: "SlotID", title: "Máquina" },
                        { data: "Juego", title: "Juego" },
                        { data: "nombre_marca", title: "Marca" },
                        { data: "nombre_modelo", title: "Modelo" },
                        { data: "Canal", title: "Canal" },
                        { data: "Toquen", title: "Token" },
                        {
                            data: "Estado", title: "Estado",
                            "render": function (i,j,value) {
                                if (value.Estado != 1) {
                                    return 'Inactivo';
                                }
                                else {
                                    return 'Activo';
                                }
                            }
                        },
                        {
                            data: null, title: "Accion",
                            render: function (value) {
                                let span=`<a class="btn btn-sm btn-warning btnAumentarCreditos" data-codmaq="${value.codigo_alterno}" data-puertosignalr="${PuertoSignalr}" data-codsala="${CodSala}" data-urlprogresivo="${UrlProgresivo}" data-maquina="${value.SlotID}"><i class="glyphicon glyphicon-plus"></i></a>`
                                return '<button type="button" class="btn btn-sm btn-success btnEditar" data-id="' + value.SlotID + '"><i class="glyphicon glyphicon-edit"></i></button> ' +
                                    '<button type="button" class="btn btn-sm btn-danger btnEliminar" data-id="' + value.SlotID + '"><i class="glyphicon glyphicon-trash"></i></button> ' +
                                    span+'<form action = "#" id = "frm_' + value.SlotID + '" >\n' +
                                    '                <input type="hidden" name="SlotID" value="' + value.SlotID + '">\n' +
                                    '            </form>';
                            }
                        }

                    ],

                    "initComplete": function (settings, json) {

                        $('#btnExcel').off("click").on('click', function () {

                            cabecerasnuevas = [];
                            cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
                            cabecerasnuevas.push({ nombre: "Progresivo", valor: $("#cboProgresivo option:selected").text() });

                            columna_cambio = [
                                {
                                    nombre: "estado",
                                    render: function (o) {
                                        valor = "";
                                        if (o == 0) {
                                            valor = "Inactivo";
                                        }
                                        if (o == 1) {
                                            valor = "Activo";
                                        }
                                        return valor;
                                    }
                                }
                            ]
                            var ocultar = ["Accion"];//"Accion";
                            funcionbotonesnuevo({
                                botonobjeto: this, ocultar: ocultar,
                                tablaobj: objetodatatable,
                                cabecerasnuevas: cabecerasnuevas,
                            });
                            VistaAuditoria("Progresivo/ProgresivoListadoMaquinaExcel", "EXCEL", 0, "", 3);
                        });

                    },
                });
                $(".btnEliminar").click(function () {
                    var id_solicitud = $(this).data("id");

                    var url_ = $("#cboSala").val() + "/servicio/EliminarMaquina?codProgresivo=" + $("#cboProgresivo").val();
                    var url = basePath + "Progresivo/ConsultarEliminarMaquinaProgresivo";
                    var redirectUrl = basePath + "Progresivo/ProgresivoListadoMaquina";
                    var dataForm = $('#frm_' + id_solicitud).serializeFormJSON();
                    let datasend={}
                    let uriConsulta=basePath
                    if(consultaPorVpn){
                        uriConsulta+='Progresivo/ConsultarEliminarMaquinaProgresivoVpn'
                        datasend={
                            parametros:dataForm,
                            urlPublica:ipPublicaAlterna + "/servicio/EliminarMaquinaVpn",
                            urlPrivada:ipPrivada + "/servicio/EliminarMaquina?codProgresivo=" + $("#cboProgresivo").val()
                        }
                    }
                    else{
                        uriConsulta+='Progresivo/ConsultarEliminarMaquinaProgresivo'
                        datasend = { parametros: dataForm, url: url_ };
                    }
                    var js2;
                    js2 = $.confirm({
                        icon: 'fa fa-spinner fa-spin',
                        title: 'Esta Seguro que desea Eliminar la Maquina ?' + id_solicitud,
                        theme: 'black',
                        animationBounce: 1.5,
                        columnClass: 'col-md-6 col-md-offset-3',
                        confirmButtonClass: 'btn-info',
                        cancelButtonClass: 'btn-warning',
                        confirmButton: "confirmar",
                        cancelButton: 'Aún No',
                        content: "",
                        confirm: function () {
                            ajaxhr = $.ajax({
                                type: "POST",
                                cache: false,
                                url: uriConsulta,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: JSON.stringify(datasend),
                                beforeSend: function (xhr) {
                                    $.LoadingOverlay("show");
                                },
                                success: function (response) {
                                    //debugger
                                    VistaAuditoria("Progresivo/ConsultarEliminarMaquinaProgresivo" + $("#cboProgresivo").val(), "ELIMINAR", 0, "", 3);
                                    var estado = response.data;
                                    if (estado === 'null') {
                                        toastr.success('Se ha eliminado satisfactoriamente', 'Mensaje Servidor');
                                        //setTimeout(function () {
                                        //    window.location.replace(basePath + "Progresivo/ProgresivoListadoMaquina");
                                        //}, 2200);
                                        var Progresivo = $("#cboProgresivo").val();
                                        BuscarMaquinas(Progresivo);
                                    } else {
                                        toastr.error('No se pudo eliminar la maquina', 'Mensaje Servidor');
                                    }
                                },
                                error: function (request, status, error) {
                                    toastr.error("Error De Conexion, Servidor no Encontrado.");
                                },
                                complete: function (resul) {
                                    AbortRequest.close()
                                    $.LoadingOverlay("hide");
                                }
                            });
                            AbortRequest.open()

                        },
                        cancel: function () {

                        },

                    });

                });

                $('.btnEditar').tooltip({
                    title: "Editar"
                });
                $('.btnEliminar').tooltip({
                    title: 'Eliminar'
                });
                $('.btnAumentarCreditos').tooltip({
                    title: 'Aumentar Creditos'
                });
            }
        },
        error: function (request, status, error) {
            $(addtabla).empty();

            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            AbortRequest.close()

            $.LoadingOverlay("hide");
        }
    });

    AbortRequest.open()
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
        $("#cboSala").append(`<option value="${value.UrlProgresivo==""?"":value.UrlProgresivo}" data-id="${value.CodSala}" data-ipprivada="${value.IpPrivada}" data-puertoservicio="${value.PuertoServicioWebOnline}" data-puertosignalr="${value.PuertoSignalr}">${value.Nombre}</option>`)
    });
}
function getPingSalas(){
    ajaxhr = $.ajax({
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
            AbortRequest.close()
        }
    });
    AbortRequest.open()
    return ajaxhr
}

function llenarSelectAPIProgresivo__(url, data, select, dataId, dataValor, selectVal) {

    if (!url) {
        toastr.error("No se Declaro Url", "Mensaje Servidor");
        return false;
    }
    var mensaje = true;
    ajaxhr = $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: function () {
            $("#" + select).html("");
            $("#" + select).append('<option value="">Cargando...</option>');
            $("#" + select).attr("disabled", "disabled");
            //$.LoadingOverlay("show");
        },
        success: function (response) {
            var datos = response;
            var mensaje = response.mensaje;
            if (datos.length > 0) {
                $("#" + select).html("");
                $("#" + select).append('<option value="">--Seleccione--</option>');
                if (selectVal == "allOption") {
                    $("#" + select).append('<option value="0">Todos</option>');
                }
                $.each(datos, function (index, value) {
                    var selected = "";
                    if ($.isArray(selectVal)) {
                        if (objectFindByKey(selectVal, dataId, value[dataId]) != null) {
                            selected = "selected='selected'";
                        };
                    } else {

                        if (value[dataId] === selectVal) {
                            selected = "selected='selected'";
                        };
                    }
                    //$("#" + select).append('<option value="' + value[dataId] + '" data-id="' + value[dataId] +'" data-urlprogresivo="'+(urlsPrueba[Math.floor(Math.random() * urlsPrueba.length)])+'"  ' + selected + '>' + value[dataValor] + '</option>');
                    $("#" + select).append('<option value="' + value[dataId] + '" data-id="' + value[dataId] +'" data-urlprogresivo="'+value.WEB_Url+'"  ' + selected + '>' + value[dataValor] + '</option>');

                });
                $("#" + select).removeAttr("disabled");
            } else {
                toastr.error("No Hay Data  en " + select, "Mensaje Servidor");
            }
            //if (mensaje !== "") {
            //    toastr.error(mensaje, "Mensaje Servidor");
            //}
        },
        complete: function () {
            AbortRequest.close()
            //$.LoadingOverlay("hide");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mensaje = false;
            //toastr.error("Servidor no responde", "Mensaje Servidor");
            $("#" + select).html("");
            $("#" + select).append('<option value="">--Seleccione--</option>');
            $("#" + select).removeAttr("disabled");
        }
    });

    AbortRequest.open()

    return mensaje;
}

VistaAuditoria("Progresivo/ProgresivoListadoMaquina", "VISTA", 0, "", 3);
function obtenerUltimoAumento(CodMaq,CodSala){
    if(CodMaq&&CodSala){
        let dataForm={
            CodMaq:CodMaq,
            CodSala:CodSala
        }
        ajaxhr = $.ajax({
            url: basePath+"Progresivo/ObtenerUltimoAumento",
            type: "POST",
            contentType: "application/json",
            data:JSON.stringify(dataForm),
            beforeSend: function (xhr) {
            },
            success: function (response) {
               return response
            },
            complete: function (resul) {
                AbortRequest.close()
            }
        })
        AbortRequest.open()
        return ajaxhr
    }
}
function registrarMovimietoEnCreditosMaquina(data,url){
    if(data){
        ajaxhr = $.ajax({
            url: basePath+url,
            type: "POST",
            contentType: "application/json",
            data:JSON.stringify(data),
            beforeSend: function (xhr) {
            },
            success: function (response) {
               return response
            },
            complete: function (resul) {
                AbortRequest.close()
            }
        })
        AbortRequest.open()
        return ajaxhr
    }
}
function aumentarCreditoMaquina(data,url){
    if(data&&url){
        let dataForm=data
        let uriConsulta=basePath
        if(consultaPorVpn){
            uriConsulta+='Progresivo/AumentarCreditoMaquinaEnServicioVpn'
            dataForm.urlPublica=ipPublicaAlterna+'/servicio/AumentarCreditoMaquinaVpn',
            dataForm.urlPrivada=ipPrivada+'/servicio/AumentarCreditoMaquina'
        }
        else{
            uriConsulta+=url
        }
        ajaxhr = $.ajax({
            url:uriConsulta,
            type:"POST",
            contentType:"application/json",
            data:JSON.stringify(dataForm),
            beforeSend:function(){
                $.LoadingOverlay('show')
            },
            success:function(response){
                return response
            },
            complete:function(){
                $.LoadingOverlay('hide')
                AbortRequest.close()
            }
        })
        AbortRequest.open()
        return ajaxhr
    }
}
function validarURL(url){
    try {
        let givenURL = new URL(url)
    } catch (error) {
       return false; 
    }
    return true;
}
function obtenerUrlSignalr(url,puerto){
    if(url){
        let uri=new URL(url)
        uri.port=puerto
        return uri.href
    }
    return false
}
function ingresarDataInputs(item,maquina){
    $("#AumentoId").val('')
    $("#AumentoCodMaq").val('')
    $("#AumentoCodSala").val('')
    $("#AumentoCantidad").val('')
    $("#AumentoPuertoSignalr").val('')
    $("#CodMaquinaProgresivo").val('')
    $("#AumentoFechaUltimoAumento").val('')
    $("#CantidadIngresada").val(1)
    $("#FechaUltimoAumento").val('')
    $("#CantidadActual").val('')
    if(item){
        $("#AumentoId").val(item.Id)
        $("#AumentoCodMaq").val(item.CodMaq)
        $("#AumentoCodSala").val(item.CodSala)
        $("#AumentoCantidad").val(item.Cantidad)
        $("#AumentoPuertoSignalr").val(item.PuertoSignalr)
        $("#CodMaquinaProgresivo").val(maquina)
        $("#AumentoFechaUltimoAumento").val(moment(item.FechaUltimoAumento).format())
        if(item.Id>0){
            $("#FechaUltimoAumento").val(moment(item.FechaUltimoAumento).format("DD/MM/YY hh:mm:ss"))
            $("#CantidadActual").val(item.Cantidad)
        }
    }
   
}
function convertirHora(segundosP) {
    if(segundosP){
        const segundos = (Math.round(segundosP % 0x3C)).toString();
        const horas    = (Math.floor(segundosP / 0xE10)).toString();
        const minutos  = (Math.floor(segundosP / 0x3C ) % 0x3C).toString();
        return `${horas.toString().padStart(2,'0')}:${minutos.toString().padStart(2,'0')}:${segundos.toString().padStart(2,'0')}`
    }
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
function getProgresivosVpn(urlPrivada, urlPublica) {
    ajaxhr = $.ajax({
        data:JSON.stringify({urlPrivada:urlPrivada,urlPublica:urlPublica}),
        type:"POST",
        cache:false,
        url:basePath+'/Progresivo/listadoprogresivosVpn',
        contentType:"application/json: charset=utf-8",
        dataType:"json",
        beforeSend:function(xhr){
            $.LoadingOverlay('show')
        },
        success:function(response){
            return response
        },
        complete: function (xhr) {
            AbortRequest.close()
            $.LoadingOverlay('hide')
        }
    })

    AbortRequest.open()

    return ajaxhr
}
function renderSelectProgresivos(data){
    $("#cboProgresivo").html("")
    if(data){
        $("#cboProgresivo").append('<option value="">--Seleccione--</option>');      
        $.each(data, function (index, value) {  
            $("#cboProgresivo").append(`
                <option 
                    value="${value["WEB_PrgID"]}" 
                    data-id="${value["WEB_PrgID"]}"
                    data-urlprogresivo="${value["WEB_Url"]}">
                    ${value["WEB_Nombre"]}
                </option>`)                     
        }); 
    }
}

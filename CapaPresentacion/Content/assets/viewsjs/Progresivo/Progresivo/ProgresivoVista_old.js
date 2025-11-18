$(document).ready(function () {
    ipPublicaG = "";
    ipPublicaGProgresivo = "";
    //llenarSelect(basePath + "Sala/ListadoSalaPorUsuarioJson", {}, "cboSala", "UrlProgresivo", "Nombre");
    ObtenerListaSalas();
    $("#cboSala").select2();
    $("#cboProgresivo").select2();

    $("#table").parent().addClass("tabla__ganadores")
    hiddenTablaGanadores(true)

    $(document).on('change', '#cboSala', function (e) {
        var ipPublica = $(this).val();

        $("#cboProgresivo").html(`<option value="">--Seleccione--</option>`);

        hiddenTablaGanadores(true)

        renderBuscarGanadores([])
        resetFiltroGanadores()

        if (!ipPublica) {
            toastr.clear()
            toastr.warning(`La sala no tiene definido URL Progresivo`, "Mensaje Servidor");

            return false
        }

        //ipPublica = ipPublica.substring(0, ipPublica.length - 4);
        //ipPublica = ipPublica + "9895";
        if (!ipPublica.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            $("#cboProgresivo").html("");
            $("#cboProgresivo").append('<option value="">--Seleccione--</option>');
            $("#cboProgresivo").removeAttr("disabled");
            return false;
        }
        ipPublicaG = ipPublica;
        //llenarSelectAPIProgresivo(ipPublica + "/servicio/listadoprogresivos", {}, "cboProgresivo", "WEB_PrgID", "WEB_Nombre");
        var url = ipPublica + "/servicio/listadoprogresivos";        
        ajaxhr = $.ajax({
            type: "POST",
            cache: false,
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                var datos = response;
                var mensaje = response.mensaje;
                if (datos.length > 0) {
                    $("#cboProgresivo").html("");
                    $("#cboProgresivo").append('<option value="">--Seleccione--</option>');                    
                    $.each(datos, function (index, value) {                       
                        $("#cboProgresivo").append('<option value="' + value["WEB_PrgID"] + '" data-id="' + value["WEB_Url"] + '">' + value["WEB_Nombre"] + '</option>');
                    });                    
                } else {
                    toastr.error("No Hay Data  en cboProgresivo", "Mensaje Servidor");
                }
            },
            error: function (request, status, error) {                
                toastr.error("Error De Conexion","Mensaje del Servidor.");
            },
            complete: function (resul) {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            }
        });
        AbortRequest.open()

    });
    $(document).on('change', '#cboProgresivo', function (e) {
        var valor = $(this).val();
        ipPublicaGProgresivo = $('#cboProgresivo option[value="' + valor + '"]').data("id")

        hiddenTablaGanadores(true)

        renderBuscarGanadores([])
        resetFiltroGanadores()
    });
    $("#btnBuscar").on("click", function () {
        var cboSala = $("#cboSala")
        if (cboSala.val() == "") {
            toastr.clear()
            toastr.warning(`La sala no tiene definido URL Progresivo`, "Mensaje Servidor");
            return false;
        }
        if ($("#cboProgresivo").val() == "") {
            toastr.error("Seleccione Progresivo", "Mensaje Servidor");
            return false;
        }
        buscarGanadores();
    }); 
    $(document).on('click', '.btnVer', function (e) {  
        // var RegD= $(this).attr("data-RegD");
        // var RegA= $(this).attr("data-RegA");
        let RegD=$("#RegD").val()
        let RegA=$("#RegA").val()
        var Fecha= moment($(this).attr("data-Fecha")).format("DD-MM-YYYY HH:mm:ss");
        var maquina = $(this).attr("data-maquina");    
        ////eliminar
        //ipPublicaGProgresivo = "http://192.168.1.47:9899";
        //Fecha = "08-01-2017 01:24:00";
        //maquina = "00054303";
        ////fin eliminar

        //var url = ipPublicaGProgresivo + "/servicio/DetallesContadoresPremio/" + Fecha + "/" + maquina + "/" + RegA + "/" + RegD;    

        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        } 
        var url = ipPublicaG + "/servicio/DetallesContadoresPremio/" + Fecha + "/" + maquina + "/" + RegA + "/" + RegD;    

        var response = {}

        ajaxhr = $.ajax({
            type: "POST",
            cache: false,            
            url: basePath + "Progresivo/ProgresivoGanadoresDetalleContadoresListadoJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json", 
            data: JSON.stringify({ url: url }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (responseData) {
                response = responseData.data;

                if (response) {

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

                            //$('#btnExcel').off("click").on('click', function () {

                            //    cabecerasnuevas = [];
                            //    cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
                            //    cabecerasnuevas.push({ nombre: "Progresivo", valor: $("#cboProgresivo option:selected").text() });
                            //    cabecerasnuevas.push({ nombre: "Máquina", valor: $("#txtMaquina").val() });
                            //    cabecerasnuevas.push({ nombre: "Pozos", valor: $("#cboPozos option:selected").text() });

                            //    definicioncolumnas = [];
                            //    definicioncolumnas.push({ nombre: "Monto", tipo: "decimal", alinear: "right", sumar: "true" });

                            //    var ocultar = [];//"Accion";
                            //    funcionbotonesnuevo({
                            //        botonobjeto: this, ocultar: ocultar,
                            //        tablaobj: objetodatatabledetalle,
                            //        cabecerasnuevas: cabecerasnuevas,
                            //        definicioncolumnas: definicioncolumnas
                            //    });
                            //});

                        },
                    });

                } else {
                    toastr.error("Solicitud incorrecta", "Mensaje Servidor");
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

    });
    $("#full-modal").on("shown.bs.modal", function () {
        $('table').css('width', '100%');
        $('.dataTables_scrollHeadInner').css('width', '100%');
        $('.dataTables_scrollFootInner').css('width', '100%');
    });
    $(document).on('click', '.btnVerContadores', function (e) {  
        e.preventDefault()
        let fecha= $(this).data("fecha");
        let maquina = $(this).data("maquina"); 
        $("#txtFechaHidden").val(fecha)
        $("#txtMaquinaHidden").val(maquina)
        $("#full-modal-contadores").modal('show')
    });
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
    });
    $(document).on('click','#btnBuscarContadores',function(e){
        e.preventDefault()
        let codMaq=$("#txtCodMaq").val()
        let fechaInicio=$("#txtFechaInicio").val()
        let fechaFin=$("#txtFechaFin").val()
        let uri=obtenerUrlOnline(ipPublicaG,8081)
        // uri="http://localhost"
        let urlOnline=uri+'online/reporte/DetallesContadores'
        let data={
            codMaq:codMaq,
            fechaInicio:fechaInicio,
            fechaFin:fechaFin,
            url:urlOnline
        }
        ajaxhr = $.ajax({
            type: "POST",
            cache: false,            
            url: basePath + "Progresivo/ProgresivoDetalleContadoresListadoJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json", 
            data: JSON.stringify(data),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (responseData) {
                response = responseData.data;
                console.log(response)
                if (response.length>0) {

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

                } else {
                    toastr.error("No se encontraton registros", "Mensaje Servidor");
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
    })
   
});
VistaAuditoria("Progresivo/ProgresivoVista", "VISTA", 0, "", 1);

function buscarGanadores() {
    //ipPublicaG = "http://200.110.47.105:9895";
    //ipPublicaG = "http://192.168.1.47:9899";

    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url = ipPublicaG + "/servicio/ganadoresjson/" + $('#cboProgresivo').val();
    var RegA = $("#RegA").val();
    var RegD = $("#RegD").val();
    if (RegA=="" || RegD=="") {
        toastr.error("Ingrese Registros Antes y Registros Despues.");
    }
    console.log(url);
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: basePath +"Progresivo/ProgresivoGanadoresListadoJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
            hiddenTablaGanadores(true)
            renderBuscarGanadores([])
        },
        success: function (response) {             
            response = response.data;
            $.LoadingOverlay("show");
            var maquina = $("#txtMaquina").val();
            var pozo = $("#cboPozos").val();
            var dataFinal = response;
            if (maquina != "") {
                dataFinal = response.filter(x => x.SlotID == maquina);
            }
            if (pozo != "0") {
                dataFinal = response.filter(x => x.TipoPozo == pozo);
            }
            if (maquina != "" && pozo != "0") {
                dataFinal = response.filter(x => x.SlotID == maquina && x.TipoPozo == pozo);
            }

            //$("#totalRecords").val(dataFinal.length); 
            dataAuditoria(1, "#formfiltro", 2, "Progresivo/ProgresivoGanadoresListadoJson", "BOTON BUSCAR");

            renderBuscarGanadores(dataFinal)

            $.LoadingOverlay("hide");
        },
        error: function (request, status, error) {            
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
            hiddenTablaGanadores(false)
        }
    });
    AbortRequest.open()
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
function obtenerUrlOnline(url,puerto){
    if(url){
        let uri=new URL(url)
        uri.port=puerto
        return uri.href
    }
    return false
}
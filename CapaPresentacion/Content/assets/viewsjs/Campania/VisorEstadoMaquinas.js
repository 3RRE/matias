$(document).ready(function () {
    let delay=10000
    let timerId=''
    let CgId=''
    let CodMaq=''
    let SesionId=''
    let detalleCupones=[]
    ObtenerListaSalas();
    $(document).on('click','.btnSincronizar',function (e) {
        e.preventDefault()
        CodMaq=$(this).data("slot_id")
        CgId=$(this).data("cgid")
        let SesionId=$(this).data("sesionid")
        let UrlSala=$("#txt_sala_id").val()
        let dataForm={
            CodMaq:CodMaq,
            CgId:CgId,
            UrlProgresivoSala:UrlSala,
            SesionId:SesionId,
        }
        $.confirm({
            title: '¿Esta seguro de Continuar?',
            content: 'Seleccione Accion a Realizar',
            buttons: {
                Sincronizar: {
                    text: 'Sincronizar',
                    btnClass: 'btn-red',
                    action:function () {
                        $.ajax({
                            type: "POST",
                            url: basePath + "CampaniaVisorEstadoMaquinas/MigrarContadoresOnlineWebCupones",
                            cache: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data:JSON.stringify(dataForm),
                            beforeSend: function (xhr) {
                                $.LoadingOverlay("show");
                            },
                            success: function (result) {
                                if(result.respuesta){
                                    buscarEquipos(UrlSala)
                                    toastr.success(result.mensaje, "Mensaje Servidor");
                                }
                                else{
                                    toastr.error(result.mensaje, "Mensaje Servidor");
                                }
                            },
                            error: function (request, status, error) {
                                toastr.error("Error", "Mensaje Servidor");
                            },
                            complete: function (resul) {
                                $.LoadingOverlay("hide");
                            }
                        });
                    },
                },
                Cancelar: function () {
                },
                // somethingElse: {
                //     text: 'Something else',
                //     btnClass: 'btn-blue',
                //     keys: ['enter', 'shift'],
                //     action: function(){
                //         $.alert('Something else?');
                //     }
                // }
            }
        });
    })
    $(document).on("click", ".btnLiberar",function (e) {
        e.preventDefault()
        CodMaq=$(this).data("slot_id")
        CgId=$(this).data("cgid")
        let SesionId=$(this).data("sesionid")
        let UrlSala=$("#txt_sala_id").val()
        let dataForm={
            CodMaq:CodMaq,
            CgId:CgId,
            UrlProgresivoSala:UrlSala,
            SesionId:SesionId,
        }
        let CodSala=$('#cbo_sala_id option').filter(':selected').data("codsala")
        $.confirm({
            title: '¿Esta seguro de Continuar?',
            content: 'Seleccione Accion a Realizar',
            buttons: {
                Liberar:{
                    btnClass: 'btn-green',
                    action:function(helloButton){
                        $.when(dataAuditoria(1, "#form_registro_campania", 3, "CampaniaVisorEstadoMaquinas/LiberarMaquinaSalaV2", "LIBERAR MAQUINA")).then(function (response, textStatus) {
                            $.ajax({
                                url: basePath + "CampaniaVisorEstadoMaquinas/LiberarMaquinaSalaV2",
                                type: "POST",
                                contentType: "application/json",
                                data: JSON.stringify(dataForm),
                                beforeSend: function () {
                                    $.LoadingOverlay("show");
                                },
                                complete: function () {
                                    $.LoadingOverlay("hide");
                                },
                                success: function (response) {
                                    if (response.respuesta) {
                                        let cupon=response.data
                                        console.log(cupon)
                                        detalleCupones=response.listaDetalleCuponesImpresos
                                        $("#txtSesionId").val(SesionId)
                                        ObtenerListaImpresorasRegistro(CodSala);
                                        $("#full-modal_detallecupones").modal({backdrop: 'static', keyboard: false})
                                        buscarEquipos(UrlSala)
                                        toastr.success(response.mensaje, "Mensaje Servidor");
                                    }
                                    else {
                                        toastr.error(response.mensaje, "Mensaje Servidor");
                                    }
                                },
                                error: function (xmlHttpRequest, textStatus, errorThrow) {
                                    toastr.error("Error Servidor", "Mensaje Servidor");
                                }
                            });
                        });
                    }
                },
                Reiniciar:{
                    text: 'Reiniciar', // text for button
                    btnClass: 'btn-red',
                    action:function(heyButton){
                        $.when(dataAuditoria(1, "#form_registro_campania", 3, "CampaniaVisorEstadoMaquinas/ReiniciarSesionClienteMaquinaSala", "LIBERAR MAQUINA")).then(function (response, textStatus) {
                            $.ajax({
                                url: basePath + "CampaniaVisorEstadoMaquinas/ReiniciarSesionClienteMaquinaSala",
                                type: "POST",
                                contentType: "application/json",
                                data: JSON.stringify(dataForm),
                                beforeSend: function () {
                                    $.LoadingOverlay("show");
                                },
                                complete: function () {
                                    $.LoadingOverlay("hide");
                                },
                                success: function (response) {
                                    if (response.respuesta) {
                                        toastr.success(response.mensaje, "Mensaje Servidor");
                                    }
                                    else {
                                        toastr.error(response.mensaje, "Mensaje Servidor");
                                    }
                                },
                                error: function (xmlHttpRequest, textStatus, errorThrow) {
                                    toastr.error("Error Servidor", "Mensaje Servidor");
                                }
                            });
                        });
                    }
                },
                Cancelar: {
                    text: 'Cancelar', // text for button
                    btnClass: 'btn-blue', // class for the button
                    isHidden: false, // initially not hidden
                    isDisabled: false, // initially not disabled
                    action: function(){
                        // longhand method to define a button
                        // provides more features
                    }
                },
            }
        });
    });
    $(document).on("click",".btnDetalle", function (e) {
        e.preventDefault()
        CodMaq=$(this).data("slot_id")
        CgId=$(this).data("cgid")
        SesionId=$(this).data('sesionid')
        $("#full-detalleModal").modal("show");
    });
    $(document).on("click",".btnVerCupones",function (e) {
        e.preventDefault()
        let divCantidadCupones=$("#divCantidadCupones")
        divCantidadCupones.html('')
        let CodMaq=$(this).data("slot_id")
        let CgId=$(this).data("cgid")
        let SesionId=$(this).data("sesionid")
        let UrlSala=$("#txt_sala_id").val()
        let span=$(this).next("span")
        let dataForm={
            UrlProgresivoSala: UrlSala,
            CodMaq: CodMaq,
            CgId: CgId,
            SesionId:SesionId
        }
        $.ajax({
            type: "POST",
            url: basePath + "CampaniaVisorEstadoMaquinas/ObtenerDetalleEstadoMaquinaSala",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                if(result.respuesta){
                    span.text(result.cantidadCupones)

                    // divCantidadCupones.html(`
                    // <div class="card bg-light mb-3">
                    // <div class="card-header">${result.cantidadCupones}</div>
                    // </div>
                    // `) 
                    // $("#full-modal_cantidad_cupones").modal('show')

                }
                else{
                    toast.error(result.mensaje,"Mensaje Servidor")
                }
                          
            },
            error: function (request, status, error) {
                toastr.error("Error", "Mensaje Servidor");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    })
    $('#full-detalleModal').on('show.bs.modal', function () {
        let UrlSala=$("#txt_sala_id").val()
        ObtenerDetalleEstado(UrlSala,CodMaq,CgId,SesionId)
     })
    $(document).on("change","#cbo_sala_id",function (e) {
        e.preventDefault()
        let UrlProgresivoSala= $(this).val()
        $("#txt_sala_id").val(UrlProgresivoSala)
        if(UrlProgresivoSala){
            buscarEquipos(UrlProgresivoSala)
            // //Comentar desde aqui
            // clearTimeout(timerId);
            // timerId = setTimeout(function request() {
            // buscarEquipos(UrlProgresivoSala,false)
            // // if (true) {
            // //   //aumentar el intervalo en la próxima ejecución
            // //   delay *= 2;
            // // }
            // timerId = setTimeout(request, delay);
            // }, delay);
            // //hasta aqui
        }
    })
    $(document).on('click','#btnRecargar',function(e){
        e.preventDefault()
        let UrlProgresivoSala = $('#cbo_sala_id option:selected').val()
        if(UrlProgresivoSala){
            buscarEquipos(UrlProgresivoSala)


            // //Comentar desde aqui
            // clearTimeout(timerId);
            // timerId = setTimeout(function request() {
            // buscarEquipos(UrlProgresivoSala,false)
            // // if (true) {
            // //   //aumentar el intervalo en la próxima ejecución
            // //   delay *= 2;
            // // }
            // timerId = setTimeout(request, delay);
            // }, delay);
            // //Hasta aqui
        }
        else{
            toastr.error("Seleccione sala","mensaje servidor")
        }
    })
    $('#full-modal_detallecupones').on('shown.bs.modal', function (e) {
        $(".tabladetallecuponesdiv").empty()
        $(".tabladetallecuponesdiv").html(`
        <table class="table table-bordered table-hover table-condensed">
            <thead>
                <tr>
                    <th>Maquina</th>
                    <th>Serie Ini</th>
                    <th>Serie Fin</th>
                    <th>Coin Out Ini</th>
                    <th>Coin Out Fin</th>
                    <th>HandPay</th>
                    <th>JackPot</th>
                    <th>Current Credit</th>
                    <th>Monto</th>
                    <th>Token</th>
                    <th>Cant. Cupones</th>
                    <th>Fecha Registro</th>
                    <th>Accion</th>
                </tr>
            </thead>
        <tbody>
        </tbody>
        </table>`);
        // $(".tabladetallecuponesdiv").html('<table class="table table-bordered table-hover table-condensed">' +
        //     '<thead>' +
        //     '<tr>' +
        //     '<th>Slot id</th>' +
        //     '<th>Serie Ini</th>' +
        //     '<th>Serie Fin</th>' +
        //     '<th>Coin Out Ini</th>' +
        //     '<th>Coin Out Fin</th>' +
        //     '<th>Current Credit</th>' +
        //     '<th>Monto</th>' +
        //     '<th>Token</th>' +
        //     '<th>Cant. Cupones</th>' +
        //     '<th>Fecha Registro</th>' +
        //     '<th>Accion</th>' +
        //     '</tr>' +
        //     '</thead>' +
        //     '<tbody><tr><td colspan="11"><div class="alert alert-danger"> Detalle Cupones</div></td></tr></tbody>' +
        //     '</table>');
        detalleCuponesRegistrados(detalleCupones);
    });
    $(document).on("click", ".btnCuponesClienteLista", function () {
        let id = $(this).data("id");
        let sala_id = $(this).data("sala_id");
        let cod_cont=$(this).data("codcont")
        let UrlProgresivoSala = $("#txt_sala_id").val()
        let CodSala=$('#cbo_sala_id option').filter(':selected').data("codsala")
        ObtenerListaImpresorasRegistro(CodSala);
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "CampaniaCupones/GetListadoCuponesGenerados",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ Cod_Cont: cod_cont,UrlProgresivoSala:UrlProgresivoSala }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
               
                if (response.respuesta) {
                    $("#tbody_listcupones").html('<tr><td colspan="4">'+
                                                '<div class="alert alert-danger">Sin Registros</div>'+
                                                '</td>'+
                                            '</tr>');
                    var tbody = "";
                    $.each(response.data, function (key, val) {
                        tbody += '<tr><td>' + val.DetGenId + '</td><td>' + val.Serie + '</td><td>' + val.CantidadImpresiones + '</td><td><button type="button" data-id="' + val.DetGenId + '" class="btn btn-danger btn-xs imprimircuponGeneradoIndividual"><span class="fa fa-print"></span></button></td></tr>'
                    });
                    if (response.data.length > 0) {
                        $("#tbody_listcupones").html(tbody);
                    }
                    $("#full-modal_cupones_lista").modal("show");
                    $("#imprimircuponesTodosInput").val(cod_cont)

                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            //error: function (request, status, error) {
            //    toastr.error("Error De Conexion, Servidor no Encontrado.");
            //},
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });
    $(document).on("click", ".imprimircuponesTodos", function (e) {
        e.preventDefault()
       
        // let cgid = $("#imprimircuponesTodosInput").val()
        let ImpresoraId=$("#cbo_impresora2").val()
        if(ImpresoraId==0||ImpresoraId==''){
            toastr.error("Debe seleccionar Impresora a Usar")
            return false
        }
        let Cod_Cont=$("#imprimircuponesTodosInput").val()
        ReimprimirCuponPorCod_Cont(Cod_Cont,ImpresoraId)
        // e.preventDefault()
       
        // // let cgid = $("#imprimircuponesTodosInput").val()
        // let ImpresoraId=$("#cbo_impresora2").val()
        // if(ImpresoraId==0||ImpresoraId==''){
        //     toastr.error("Debe seleccionar Impresora a Usar")
        //     return false
        // }
        // let cgid= $("#txtSesionId").val()
        // if(cgid==0||cgid==''){
        //     toastr.error("Id de Cupon Incorrecto")
        //     return false
        // }
        // itemsImprimir=[]
        // let listaIndidivuales=$(".imprimircuponGeneradoIndividual")
        // $.each(listaIndidivuales, function (index,value) {
        //     itemsImprimir.push($(value).data("id"))
        // })      
        // if(itemsImprimir.length==0){
        //     toastr.error("No se encontraton tickets a imprimir")
        //     return false
        // }
        // //reimpresion=true
      
        // ReimprimirCupon(cgid,ImpresoraId)
    });

    $(document).on("click", ".imprimircuponGeneradoIndividual", function (e) {
        e.preventDefault()
        let ImpresoraId=$("#cbo_impresora2").val()
        if(ImpresoraId==0||ImpresoraId==''){
            toastr.error("Debe seleccionar Impresora a Usar")
            return false
        }
        let id=$(this).data("id")
        ReimprimirCuponPorDetGenId(id,ImpresoraId)
        // e.preventDefault()
        // let ImpresoraId=$("#cbo_impresora2").val()
        // if(ImpresoraId==0||ImpresoraId==''){
        //     toastr.error("Debe seleccionar Impresora a Usar")
        //     return false
        // }

        // let cgid= $("#txtSesionId").val()
        // if(cgid==0||cgid==''){
        //     toastr.error("Id de Cupon Incorrecto")
        //     return false
        // }
        // // let cgid = $("#imprimircuponesTodosInput").val()
        // let id=$(this).data("id")
        
        // itemsImprimir=[]
        // itemsImprimir.push(id)
        // if(itemsImprimir.length==0){
        //     toastr.error("No se encontraton tickets a imprimir")
        //     return false
        // }
        // reimpresion=true
     
        // ReimprimirCupon(cgid,ImpresoraId)
    });
    $(document).on('click','#ImprimirCupones',function(e){
        e.preventDefault()
        printJS({ printable: 'printArea', type: 'html' })
    })
    $(document).on("click","#btnImprimirConsolidado",function(e){
        let IdCupon=$("#txtSesionId").val()
        let UrlProgresivoSala = $("#txt_sala_id").val()
        let dataForm={
            SesionId:IdCupon,
            UrlProgresivoSala:UrlProgresivoSala
        }
        let url = basePath+"CampaniaSorteo/ImprimirConsolidado"
        console.log(dataForm)
        let divPrintArea=$("#printArea")
        divPrintArea.empty()
        $.ajax({
            type: "POST",
            cache: false,
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                console.log(response)
                if(response.respuesta){
                    let cuponGenerado=response.data
                    let span=`
                        <div style="width:100%;display:flex;justify-content:center;padding-right:0;margin-right:0;paddint-left:0;margin-left:0;padding-top:0px;margin-top:0px">
                            <div class="card" style="width: 18rem;">
                                <div class="card-body">
                                    <p class="card-text" style="text-align: center;font-size:12px;">${cuponGenerado.NombreSala}</h5>
                                    <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:12px;">Fecha: <span style="font-weight:normal">${moment(cuponGenerado.Fecha).format("DD-MM-YYYY hh:mm:ss A")}</span></p>
                                    <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:12px;">Slot: <span style="font-weight:normal">${cuponGenerado.SlotId}</span></p>
                                    <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:12px;">Cliente: <span style="font-weight:normal">${cuponGenerado.NombreCliente}</span></p>
                                    <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:12px;">Nro. Documento: <span style="font-weight:normal">${cuponGenerado.DniCliente}</span></p>
                                    <p class="card-text" style="padding-top:0px;margin-top:0px;font-size:12px;">Cupones (${cuponGenerado.CantidadCupones}): <!--<span style="font-weight:normal">${cuponGenerado.Serie}</span>--></p>
                                    <div style="text-align:right;padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;">
                                    <span style="font-size:8px;">${response.SesionId}</span>
                                </div> 
                                    <p style="text-align:center;padding-top:0px;margin-top:0px;opacity:0;">-----------------------------------------------------</p>
                                </div>
                            </div>
                        </div>`
                    divPrintArea.html(span)
                    $('#ImprimirCupones').trigger('click');
                }
                else{
                    toast.error(response.mensaje,"Mensaje Servidor")
                }
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    })
    $(document).on('click','#btnImprimirTodo',function(e){
        e.preventDefault()
        let ImpresoraId=$("#cbo_impresora3").val()
        if(ImpresoraId==0||ImpresoraId==''){
            toastr.error("Debe seleccionar Impresora a Usar")
            return false
        }
        let cgid= $("#txtSesionId").val()
        ReimprimirCuponPorSesionId(cgid,ImpresoraId)
        // e.preventDefault()
        // let ImpresoraId=$("#cbo_impresora3").val()
        // if(ImpresoraId==0||ImpresoraId==''){
        //     toastr.error("Debe seleccionar Impresora a Usar")
        //     return false
        // }
        // let cgid= $("#txtSesionId").val()
        // ReimprimirTodo(cgid,ImpresoraId)
    })
    $(document).on('click','#btnMostrarTodo',function(e){
        e.preventDefault()
        let button=$(this)
        let mostrar= button.data('mostrar')
        console.log(mostrar)
        if(mostrar){
            let tr=$(".isHidden")
            tr.addClass('isVisible')
            tr.removeClass('isHidden')
            tr.css('display','table-row')
            button.data('mostrar',false)
            button.removeClass('btn-success')
            button.find('i').removeClass('fa-eye')

            button.addClass('btn-danger')
            button.find('i').addClass('fa-eye-slash')
        }
        else{
            let tr=$(".isVisible")
            tr.addClass('isHidden')
            tr.removeClass('isVisible')
            tr.css('display','none')
            button.data('mostrar',true)
            button.removeClass('btn-danger')
            button.find('i').removeClass('fa-eye-slash')

            button.addClass('btn-success')
            button.find('i').addClass('fa-eye')
        }
    })
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
            $("#cbo_sala_id").append('<option value="">--Seleccione--</option>')
            $.each(datos, function (index, value) {
                $("#cbo_sala_id").append('<option value="' + value.UrlProgresivo + '" data-codsala="'+value.CodSala+'"  >' + value.Nombre + '</option>');
            });
            $("#cbo_sala_id").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: true
            });
            // $("#cbo_sala_id").val(0).trigger("change");
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
function ObtenerDetalleEstado(UrlSala,CodMaq,CgId,SesionId){
    let dataForm={
        UrlProgresivoSala: UrlSala,
        CodMaq: CodMaq,
        CgId: CgId,
        SesionId:SesionId
    }

    $.ajax({
        type: "POST",
        url: basePath + "CampaniaVisorEstadoMaquinas/ObtenerDetalleEstadoMaquinaSala",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data:JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            let tbodyDetalleEstado=$("#tableDetalleEstado > tbody")
            let span=''
            tbodyDetalleEstado.html("")
            if(result.respuesta){
                var datos = result.data
                datos.map((item)=>{
                    let spanDisplay =''
                    let classHidden=''
                    if(item.CantidadCupones==0){
                        spanDisplay='color:red;display:none'
                        classHidden='isHidden'
                    }else{
                        spanDisplay='color:black;display:table-row'
                    }
                    // if(item.CantidadCupones!=0){
                        span+=`
                        <tr style="${spanDisplay}" class="${classHidden}">
                            <td>${item.CodMaq}</td>
                            <td>${item.CodSala}</td>
                            <td>${item.CoinOutAnterior}</td>
                            <td>${item.CoinOut}</td>
                            <td>${item.HandPay}</td>
                            <td>${item.JackPot}</td>
                            <td>${item.CurrentCredits}</td>
                            <td>${item.Monto}</td>
                            <td>${item.Token}</td>
                            <td>${item.CoinOutIas}</td>
                            <td>${item.CantidadCupones}</td>
                            <td>${moment(item.FechaRegistro).format("DD/MM/YYYY hh:mm A")}</td>
                        </tr>`
                    // }
                  
                })
                tbodyDetalleEstado.html(span)
            }
            else{
                toastr.error(result.mensaje,"Mensaje Servidor")
            }
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
function detalleCuponesRegistrados(dataDetalleCupones) {
    // const dataFiltered=dataDetalleCupones.filter(item=>{
    //     return item.CantidadCuponesImpresos>0
    // })
    // const dataFiltered=dataDetalleCupones
    let addtabla = $(".tabladetallecuponesdiv");
    let id = $("#txtSesionId").val()
    $(addtabla).empty()
    $(addtabla).append('<table id="cuponeslistaclientes" class="table table-condensed table-bordered table-hover" style="width:100%"></table>')
    objetodatatable = $("#cuponeslistaclientes").DataTable({
        "bDestroy": true,
        "bSort": true,
        "ordering": true,
        "scrollCollapse": true,
        "scrollX": false,
        "sScrollX": "100%",
        "paging": true,
        "autoWidth": false,
        "bAutoWidth": true,
        "bProcessing": true,
        "bDeferRender": true,
        "aaSorting": [],
        data: dataDetalleCupones,
        columns: [

            { data: "CodMaq", title: "Maquina" },
            { data: "CodSala", title: "Cod Sala" },
            { data: "SerieIni", title: "Serie Ini" },
            { data: "SerieFin", title: "Serie Fin" },
            {data:"CantidadCuponesImpresos",title:"Cantidad Cupones"},
            { data: "CoinOutAnterior", title: "Coin Out Ini" },
            { data: "CoinOut", title: "Coin Out Fin" },
            { data: "HandPay", title: "HandPay" },
            { data: "JackPot", title: "JackPot" },
            { data: "CurrentCredits", title: "Current Credits" },
            { data: "Monto", title: "Monto" },
            { data: "Token", title: "Token" },
            { data: "CoinOutIas", title: "Coin Out Parametro" },
            {
                data: "FechaRegistro", title: "Fecha Reg.",
                "render": function (value) {
                    return moment(value).format('DD/MM/YYYY hh:mm:ss a');
                }
            },
            
            {
                data: null, title: "Accion",
                "render": function (value) {
                    var butom = "";
                    butom += `<button type="button" class="btn btn-xs btn-primary btnCuponesClienteLista" data-id="${value.DetImpId}" data-sala_id="${value.CodSala}" data-codcont="${value.Cod_Cont}"><i class="fa fa-building"></i> </button>`;
                    return butom;
                }
            }
        ]
        ,
        "initComplete": function (settings, json) {
        },
        "drawCallback": function (settings) {
        }
    })

    $('#cuponeslistaclientes tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
    })
}
function ReimprimirCupon(CgId, IdImpresora){
    let url=basePath
    let dataForm
    url+= "CampaniaSorteo/ReimprimirCupon"
    dataForm={
        CgId:CgId,
        itemsImprimir:itemsImprimir,
        IdImpresora:IdImpresora
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if(response.respuesta){
                toastr.success(response.mensaje,"Mensaje Servidor")
            }
            else{
                toastr.error(response.mensaje,"Mensaje Servidor")
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    })
}
function ObtenerListaImpresorasRegistro(codsala) {
    $("#cbo_impresora2").html("");
    $("#cbo_impresora3").html("");
    $.ajax({
        type: "POST",
        url: basePath + "CampaniaImpresora/ListarImpresorasxUsuarioidJson",
        cache: false,
        data: JSON.stringify({ codsala: codsala}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            
            var option = '<option value="">--Seleccione--</option>';
            $.each(datos, function (index, value) {
                option+='<option value="' + value.id + '"  >' + value.nombre + ' (<span style="font-size:6px">'+value.ip+'</span>)</option>';
            });
            $("#cbo_impresora2").html(option);
            $("#cbo_impresora3").html(option);
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
function ReimprimirTodo(CgId, IdImpresora){
    let url=basePath
    let dataForm
    url+= "CampaniaSorteo/ReimprimirTodo"
    dataForm={
        CgId:CgId,
        IdImpresora:IdImpresora
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if(response.respuesta){
                toastr.success(response.mensaje,"Mensaje Servidor")
            }
            else{
                toastr.error(response.mensaje,"Mensaje Servidor")
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    })
}
function buscarEquipos(UrlSala,loader=true) {
    var addtabla = $(".contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "/CampaniaVisorEstadoMaquinas/ListarEquiposNoLibresJsonV2",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ UrlProgresivoSala: UrlSala }),
        beforeSend: function (xhr) {
            if(loader){
                $.LoadingOverlay("show");
            }
        },

        success: function (response) {
            // console.log(response)
            response = response.data;
            // dataAuditoria(1, "#formfiltro", 3, "/CampaniaVisorEstadoMaquinas/ListarEquiposNoLibresJsonV2", "BOTON BUSCAR");
            $(addtabla).empty();
            $(addtabla).append('<table id="equipostable" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#equipostable").DataTable({
                "bDestroy": true,
                "bSort": true,
                "ordering": true,
                "scrollCollapse": true,
                "scrollX": false,
                "sScrollX": "100%",
                "paging": true,
                "autoWidth": false,
                "bAutoWidth": true,
                "bProcessing": true,
                "bDeferRender": true,
                "aaSorting": [0],
                data: response,
                columns: [
                    { data: "SesionId", title: "SesionId" },  
                    { data: "NroDocumento", title: "NroDoc" },
                    { data: "NombreCliente", title: "Cliente" },
                    { data: null , title: "Fecha",
                    render: function(value){
                        return moment(value.Fecha).format("DD/MM/YYYY hh:mm:ss A")
                        }
                    },
                    { data: "CodMaquina", title: "Maquina" },
                    { data: "CantidadJugadas", title: "Nro. Jugadas" },
                    // { data: "SesionId", title: "SesionId" },
                    { data: null , title: "Cupones",
                    render: function(value){
                        return value.CantidadCupones
                        }
                    },
                    {
                        data: null, title: "Accion",
                        "render": function (value) {
                            if(value.Terminado==0){
                                var butom = "";
                                butom += `<button type="button" class="btn btn-xs btn-danger btnLiberar" title="LIBERAR/REINICIAR" data-slot_id="${value.CodMaquina}" data-cgid="${value.CgId}" data-sesionid="${value.SesionId}"><i class="glyphicon glyphicon-pencil"></i> Liberar/Reiniciar</button>`;
                                butom += ` <button type="button" class="btn btn-xs btn-info btnDetalle" title="DETALLE" data-slot_id="${value.CodMaquina}" data-cgid="${value.CgId}" data-sesionid="${value.SesionId}"><i class="glyphicon glyphicon-list-alt"></i> Detalle</button>`;
                                return butom;
                            }
                            else{
                                return ''
                            }
                        }
                    }
                ]
                ,
                "initComplete": function (settings, json) {
                },
                "drawCallback": function (settings) {
                }
            });
            $('#equipostable tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            if(loader){
                $.LoadingOverlay("hide");
            }
        }
    });
}
function ReimprimirCuponPorDetGenId(DetGenId,IdImpresora){
    let url=basePath
    let dataForm
    // url+= "CampaniaSorteo/ReimprimirCuponPorDetGenId"
    url+= "CampaniaSorteo/ReimprimirCupon"
    let Tipo=1
    let UrlProgresivoSala = $("#txt_sala_id").val()
    dataForm={
        IdImpresion:DetGenId,
        IdImpresora:IdImpresora,
        UrlProgresivoSala:UrlProgresivoSala,
        Tipo:Tipo
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if(response.respuesta){
                toastr.success(response.mensaje,"Mensaje Servidor")
            }
            else{
                toastr.error(response.mensaje,"Mensaje Servidor")
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    })
}
function ReimprimirCuponPorCod_Cont(Cod_Cont,IdImpresora){
    let url=basePath
    let dataForm
    // url+= "CampaniaSorteo/ReimprimirCuponPorCod_Cont"
    url+= "CampaniaSorteo/ReimprimirCupon"
    let Tipo=2
    let UrlProgresivoSala = $("#txt_sala_id").val()
    dataForm={
        IdImpresion:Cod_Cont,
        IdImpresora:IdImpresora,
        UrlProgresivoSala:UrlProgresivoSala,
        Tipo:Tipo
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if(response.respuesta){
                toastr.success(response.mensaje,"Mensaje Servidor")
            }
            else{
                toastr.error(response.mensaje,"Mensaje Servidor")
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    })
}
function ReimprimirCuponPorSesionId(SesionId,IdImpresora){
    let url=basePath
    let dataForm
    // url+= "CampaniaSorteo/ReimprimirCuponPorSesionId"
    url+= "CampaniaSorteo/ReimprimirCupon"
    let UrlProgresivoSala = $("#txt_sala_id").val()
    let Tipo=3
    dataForm={
        IdImpresion:SesionId,
        IdImpresora:IdImpresora,
        UrlProgresivoSala:UrlProgresivoSala,
        Tipo:Tipo
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if(response.respuesta){
                toastr.success(response.mensaje,"Mensaje Servidor")
            }
            else{
                toastr.error(response.mensaje,"Mensaje Servidor")
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    })
}
$(document).ready(function () {
    ObtenerListaSalas()
    $(document).on("change","#cbo_sala_id",function (e) {
        e.preventDefault()
        let UrlProgresivoSala= $(this).val()
        let CodSala=$(this).find(':selected').data('codsala')
        
        // $("#txt_sala_id").val(UrlProgresivoSala)
        if(UrlProgresivoSala){
            SincronizarInformacionMaquinaSala(UrlProgresivoSala,CodSala)
        }
    })
    $(document).on("change",".cboRestringido",function(e){
        e.preventDefault()
        let Restringido= $(this).val()
        let CodSala=$(this).data("codsala")
        let CodMaquina=$(this).data("codmaquina")
        let Juego=$(this).data("juego")
        let Marca=$(this).data("marca")
        let Modelo=$(this).data("modelo")
        let dataForm={
            Restringido:Restringido,
            CodSala:CodSala,
            CodMaquina:CodMaquina,
            Juego:Juego,
            Marca:Marca,
            Modelo:Modelo
        }
        $.ajax({
            type: "POST",
            url: basePath + "CampaniaMaquinaRestringida/EditarEstadoRestriccionMaquina",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show")
            },
            success: function (result) {
                if(result.respuesta){
                    toastr.success(result.mensaje, "Mensaje Servidor")
                }else{
                    toastr.error(result.mensaje, "Mensaje Servidor")
                }
            },
            error: function (request, status, error) {
                toastr.error("Error", "Mensaje Servidor")
            },
            complete: function (resul) {
                $.LoadingOverlay("hide")
            }
        });
    })
})
function ObtenerListaSalas() {
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (result) {
            var datos = result.data
            $("#cbo_sala_id").append('<option value="">--Seleccione--</option>')
            $.each(datos, function (index, value) {
                $("#cbo_sala_id").append('<option data-codsala="'+value.CodSala+'" value="' + value.UrlProgresivo + '"  >' + value.Nombre + '</option>')
            })
            $("#cbo_sala_id").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: true
            })
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    });
    return false;
}
function SincronizarInformacionMaquinaSala(UrlProgresivoSala,CodSala){
    if(UrlProgresivoSala&&CodSala){
        let addtabla = $(".contenedor_tabla");
        let dataForm={
            UrlProgresivoSala: UrlProgresivoSala,
            CodSala:CodSala
        }
        $.ajax({
            type: "POST",
            url: basePath + "CampaniaMaquinaRestringida/ListadoMaquinasRestringidasSalaJSON",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show")
            },
            success: function (result) {
               if(result.respuesta){
                   let data=result.data
                    $(addtabla).empty()
                    $(addtabla).append('<table id="listaMaquinasRestringidas" class="table table-condensed table-bordered table-hover" style="width:100%"></table>')
                    objetodatatable = $("#listaMaquinasRestringidas").DataTable({
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
                        data: data,
                        columns: [
                
                            { data: "CodMaquina", title: "Slot" },
                            { data: "CodSala", title: "Cod Sala" },
                            { data: "Juego", title: "Serie Ini" },
                            { data: "Marca", title: "Serie Fin" },
                            { data: "Modelo", title: "Coin Out Ini" },
                            {
                                data: null, title: "Restringido",
                                "render": function (row,type,oData) {
                                    let span=`
                                        <select 
                                            class="form-control input-sm cboRestringido" 
                                            data-codsala="${oData.CodSala}" 
                                            data-codmaquina="${oData.CodMaquina}" 
                                            data-juego="${oData.Juego}" 
                                            data-marca="${oData.Marca}" 
                                            data-modelo="${oData.Modelo}" 
                                        >`
                                    if(oData.Restringido==1){
                                            span+='<option value="1" selected>SI</option><option value="0">NO</option>'
                                    }
                                    else{
                                        span+='<option value="1">SI</option><option value="0" selected>NO</option>'
                                    }
                                    span+='</select>'
                                    return span
                                }
                            }
                        ]
                        ,
                        "initComplete": function (settings, json) {
                        },
                        "drawCallback": function (settings) {
                        }
                    })
                
                    $('#listaMaquinasRestringidas tbody').on('click', 'tr', function () {
                        $(this).toggleClass('selected');
                    })
                }
                else{
                    toastr.error(result.mensaje, "Mensaje Servidor")
                }
            },
            error: function (request, status, error) {
                toastr.error("Error", "Mensaje Servidor")
            },
            complete: function (resul) {
                $.LoadingOverlay("hide")
            }
            });
    }
}
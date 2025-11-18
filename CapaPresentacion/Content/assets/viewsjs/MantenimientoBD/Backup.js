$(document).on('ready',function(e){
    
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
        minViewMode: 1,
    });
    ListarBackups().then(data=>{
        if(data.data){
            $("#fieldset_backup").show()
            RenderizarDatatable(data.data)
        }
    })
    ObtenerInformacionDatabase().then(data=>{
        if(data){
           RenderizarWidgets(data)
        }
    })
    // $i.datetimepicker('destroy');
    // $(document).on('dp.change','.dateOnly_',function(e){
    //     $(".bootstrap-datetimepicker-widget").removeClass("picker-open")
    //     $(".bootstrap-datetimepicker-widget").css("display",'none')
    // })
    $(document).on('click','.btnBorrarBackup',function(e){
        let NombreArchivo=$(this).data('nombre')
        if(NombreArchivo){
            $.confirm({
                icon: 'fa fa-spinner fa-spin',
                title: '¿Esta seguro de Eliminar el backup? Esta operacion no se podra revertir',
                theme: 'black',
                animationBounce: 1.5,
                columnClass: 'col-md-6 col-md-offset-3',
                confirmButtonClass: 'btn-info',
                cancelButtonClass: 'btn-warning',
                confirmButton: 'SI',
                cancelButton: 'NO',
                content: false,
                confirm: function () {
                    BorrarArchivoBackup(NombreArchivo).then(function(e){
                        ListarBackups().then(data=>{
                            if(data.data){
                                $("#fieldset_backup").show()
                                RenderizarDatatable(data.data)
                            }
                        })
                    })
                },
                cancel: function () {
                }
            });
        }
    })
    $(document).on('click','#btnLimpiarInformacion',function(e){
        e.preventDefault()

        let TableName=$("#cboTablaLimpieza").val()
        let FechaLimpieza=$("#FechaLimpieza").val()
        let Columna=$("#cboTablaLimpieza :selected").data('columna')
        if(TableName==''){
            toastr.error('Seleccione una Tabla','Error')
            return false
        }
        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: `¿Esta seguro de Eliminar la información? Se eliminaran los registros anteriores a la fecha : ${FechaLimpieza}. Esta operacion no se podra revertir`,
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: 'SI',
            cancelButton: 'NO',
            content: false,
            confirm: function () {
                LimpiarInformacion(TableName,FechaLimpieza,Columna).then(function(result){
                    ObtenerInformacionDatabase().then(data=>{
                        if(data){
                           RenderizarWidgets(data)
                        }
                    })
                })
            },
            cancel: function () {
            }
        });
    })
    $(document).on('click','#btnGenerarBackup',function(e){
        e.preventDefault()
        let RutaBackup=$("#RutaBackups").val()
        if(RutaBackup){
            GenerarBackup(RutaBackup).then(function(e){
                ListarBackups().then(data=>{
                    if(data.data){
                        $("#fieldset_backup").show()
                        RenderizarDatatable(data.data)
                    }
                })
            })
        }
    })

})
function ListarBackups(){
    let RutaBackups=$("#RutaBackups").val()
    if(RutaBackups){
        let dataForm={
            RutaBackups:RutaBackups
        }
        return $.ajax({
            url: basePath+"MantenimientoBD/ObtenerListadoBackups",
            type: "POST",
            contentType: "application/json",
            data:JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show")
            },
            success: function (response) {
               return response
    
            },
            complete: function (resul) {
                $.LoadingOverlay("hide")
            }
        })
    }
}
function GenerarBackup(RutaBackups){
    if(RutaBackups){
        let dataForm={
            RutaBackups:RutaBackups
        }
        return $.ajax({
            url: basePath+"MantenimientoBD/GenerarBackup",
            type: "POST",
            contentType: "application/json",
            data:JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show")
            },
            success: function (response) {
                if(response.respuesta){
                    toastr.success(response.mensaje,"Mensaje Servidor")
                }
               return response
            },
            complete: function (resul) {
                $.LoadingOverlay("hide")
            }
        })
    }
}
function RenderizarDatatable(data){
    let addtabla = $(".contenedor_backups")
    addtabla.empty()
    $(addtabla).append('<table id="listaBackups" class="table table-condensed table-bordered table-hover" style="width:100%"></table>')
    objetodatatable = $("#listaBackups").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": true,
        "sScrollX": "100%",
        "paging": true,
        "autoWidth": true,
        "bAutoWidth": true,
        "bProcessing": true,
        "bDeferRender": true,
        data: data,
        columns: [
            { data: "nombre_completo", title: "Backup" },
            { data: "tamanio", title: "Peso(MB)",className:'text-right' ,render:function(value){
                return value.toFixed(2)
            } },
            {
                data:null,title:"Accion",render:function(value,type,oData,y){

                    return `
                        <a href="#" 
                        class="btn btn-sm btnBorrarBackup btn-danger" data-nombre="${oData.nombre_completo}">
                        Borrar</a>
                    `
                }
            }
        ],
        "initComplete": function (settings, json) {
        },
        "drawCallback": function (settings) {
        }
    });
}
function BorrarArchivoBackup(NombreArchivo){
    let RutaBackups=$("#RutaBackups").val()
    if(RutaBackups){
        let dataForm={
            RutaBackups:RutaBackups,
            NombreArchivo:NombreArchivo
        }
        return $.ajax({
            url: basePath+"MantenimientoBD/BorrarArchivoBackup",
            type: "POST",
            contentType: "application/json",
            data:JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show")
            },
            success: function (response) {
               if(response.respuesta){
                toastr.success(response.mensaje,"Mensaje Servidor")
               }
               return response
            },
            complete: function (resul) {
                $.LoadingOverlay("hide")
            }
        })
    }
}
function LimpiarInformacion(TableName,FechaLimpieza,Columna){
    if(TableName,FechaLimpieza){
        let dataForm={
            Tabla:TableName,
            FEcha:FechaLimpieza,
            Columna:Columna
        }
        return $.ajax({
            url: basePath+"MantenimientoBD/LimpiarTabla",
            type: "POST",
            contentType: "application/json",
            data:JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show")
            },
            success: function (response) {
               if(response.respuesta){
                toastr.success(response.mensaje,"Mensaje Servidor")
               }
               return response
            },
            complete: function (resul) {
                $.LoadingOverlay("hide")
            }
        })
    }
}
function ObtenerInformacionDatabase(){
  
    return $.ajax({
        url: basePath+"MantenimientoBD/InformacionDatabase",
        type: "POST",
        contentType: "application/json",
        beforeSend: function (xhr) {
            // $.LoadingOverlay("show")
        },
        success: function (response) {
            return response
        },
        complete: function (resul) {
            // $.LoadingOverlay("hide")
        }
    })
}
function RenderizarWidgets(data){
    let arrayElementos=[]

    let dataWindgetSuperior;
    let dataBD=data.informacionDatabase
    let dataTablas=data.informacionTablas
    let totalSum=0
    dataTablas.map(item=>totalSum+=parseFloat(item.TotalSpaceMB))

    let items = dataTablas.filter((item,idx) => idx < 3)
  
    items.map((item,index)=>{
        arrayElementos.push({
            name:item.TableName,
            size:item.TotalSpaceMB,
            color:ObtenerColor(String(index)),
            ignore:false
        })
    })
    let others=parseFloat(dataBD.row_size_mb)-parseFloat(totalSum)
    arrayElementos.push({
        name:'Otras Tablas',
        size:others.toFixed(2),
        color:ObtenerColor('default'),
        ignore:false
    })
    arrayElementos.push({
        name:dataBD.database_name,
        size:dataBD.row_size_mb,
        color: ObtenerColor('3'),
        ignore:true
    })
    dataWindgetSuperior={
        cantidadTablas:dataTablas.length,
        nombreDatabase:dataBD.database_name,
        pesoBD:dataBD.row_size_mb,
        pesoLOG:dataBD.log_size_mb,
        pesoTotal:dataBD.total_size_mb
    }
    RenderizarWidgetSuperior(dataWindgetSuperior)
    RenderizarWidgetPie(arrayElementos)
}
function RenderizarWidgetPie(data){
    let colors=[]
    $("#contenedor_info").html('')
    let legendArray=[]
    let miscelaneousArray=[]
    if(data){
        data.map(item=>{
            let ignore=item.ignore?'data-ignore="true"':''
            let miscelaneous=`
            <tr class="sales-value-misc-figures-item">
                <td class="sales-value-misc-figures-item-text">
                    ${item.name}
                </td>
                <td class="sales-value-misc-figures-item-value">
                    ${item.size} (MB)
                </td>
            </tr>
            `
            let legend=`
            <div class="sales-value-legend-item" ${ignore}>
                <div class="sales-value-legend-color">
                    <div class="sales-value-legend-color-box" data-color="${item.color}" style="background-color: ${item.color};"></div>
                </div>
                <div class="sales-value-legend-number" data-raw-value="${item.size}">${item.size}</div>
                <div class="sales-value-legend-text">${item.name} (MB)</div>
            </div>
            `
            miscelaneousArray.push(miscelaneous)
            legendArray.push(legend)
        })
       
      
        let generalSpan=`
        <div class="row">
            <div class="col-md-12">
                <div class="block block-size-normal">
                    <div class="block-content-outer">
                        <div class="block-content-inner mCustomScrollbar _mCS_3 mCS-autoHide" style="position: relative; overflow: visible;">
                            <div id="mCSB_3" class="mCustomScrollBox mCS-dark mCSB_vertical mCSB_outside" tabindex="0" style="max-height: none;">
                                <div id="mCSB_3_container" class="mCSB_container" style="position: relative; top: -9px; left: 0px;" dir="ltr">
                                    <div class="sales-value-graph-container" style="margin-top: 15px;">
                                        <div id="sales-value-graph" class="graph graph-size-medium" style="padding: 0px; position: relative;">
                                            <canvas class="flot-base" width="165" height="130" style="direction: ltr; position: absolute; left: 0px; top: 0px; width: 165px; height: 130px;"></canvas><canvas class="flot-overlay" width="165" height="130" style="direction: ltr; position: absolute; left: 0px; top: 0px; width: 165px; height: 130px;"></canvas>
                                        </div>
                                    </div>
                                    <div class="sales-value-legend">
                                        ${legendArray.join('')}

                                    </div>
                                    <div class="sales-value-misc">
                                        <!--<div class="sales-value-misc-intro">
                                            Here Are Your Sales &amp; Averages For This Year:
                                        </div>-->
                                        <div class="sales-value-misc-figures">
                                            <table class="table">
                                                <tbody>
                                                    ${miscelaneousArray.join('')}

                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        `
        $("#contenedor_info").html(generalSpan)
        circloidSalesValueWidget("#sales-value-graph")
    }
   
    
}
function RenderizarWidgetSuperior(data){
    let contenedor=$("#contenedor_widgets")
    contenedor.html('')
    if(data){
        let span=`
        <div class="col-sm-6 col-md-3">
              <div class="c-widget c-widget-quick-info c-widget-size-small highlight-color-blue">
                  <div class="c-widget-icon">
                      <span class="icon icon-axis-rules"></span>
                  </div>
                  <div class="c-wdiget-content-block">
                      <div class="c-widget-content-heading">
                          ${data.cantidadTablas}
                      </div>
                      <div class="c-widget-content-sub">
                          Total de Tablas
                      </div>
                  </div>
              </div>
          </div>
          <div class="col-sm-6 col-md-3">
              <div class="c-widget c-widget-quick-info c-widget-size-small highlight-color-green">
                  <div class="c-widget-icon">
                      <span class="icon icon-coins"></span>
                  </div>
                  <div class="c-wdiget-content-block">
                      <div class="c-widget-content-heading">
                          ${data.pesoTotal}
                      </div>
                      <div class="c-widget-content-sub">
                          Peso Total (MB)
                      </div>
                  </div>
              </div>
          </div>
          <div class="col-sm-6 col-md-3">
              <div class="c-widget c-widget-quick-info c-widget-size-small highlight-color-red">
                  <div class="c-widget-icon">
                      <span class="icon icon-hdd"></span>
                  </div>
                  <div class="c-wdiget-content-block">
                      <div class="c-widget-content-heading">
                          ${data.pesoBD}
                      </div>
                      <div class="c-widget-content-sub">
                          Archivo Mdf (MB)
                      </div>
                  </div>
              </div>
          </div>
          <div class="col-sm-6 col-md-3">
              <div class="c-widget c-widget-quick-info c-widget-size-small highlight-color-yellow">
                  <div class="c-widget-icon">
                      <span class="icon icon-hdd-raid"></span>
                  </div>
                  <div class="c-wdiget-content-block">
                      <div class="c-widget-content-heading">
                          ${data.pesoLOG}
                      </div>
                      <div class="c-widget-content-sub">
                          Archivo Log (MB)
                      </div>
                  </div>
              </div>
          </div>
      `
      contenedor.html(span)
      $("#nombre_bd").text(data.nombreDatabase)
    }
    
}
// function ObtenerInformacionTablas(){
  
//     return $.ajax({
//         url: basePath+"MantenimientoBD/InformacionTablas",
//         type: "POST",
//         contentType: "application/json",
//         beforeSend: function (xhr) {
//             // $.LoadingOverlay("show")
//         },
//         success: function (response) {
//             return response
//         },
//         complete: function (resul) {
//             // $.LoadingOverlay("hide")
//         }
//     })
// }
function circloidSalesValueWidget(placeholder){

    var data = [];
    var dataObj = {};

    var legendItems = $(placeholder).closest(".block").find(".sales-value-legend-item[data-ignore!='true']");
    var legendCount = legendItems.length;

    for (var n = 0; n < legendCount; n++){
        var valueGraph = legendItems.eq(n).find(".sales-value-legend-number").data("raw-value");
        var colorGraph = legendItems.eq(n).find(".sales-value-legend-color-box").data("color");
        var textGraph = legendItems.eq(n).find(".sales-value-legend-text").text();

        // Set the Background color of the legend box
        legendItems.eq(n).find(".sales-value-legend-color-box").css({"background-color":colorGraph});

        // Populate Flot data array
        dataObj = {data: valueGraph, color: colorGraph, label: textGraph};
        data.push(dataObj);
    }

    var options = {
        series: {
            pie: { 
                show: true,
                radius:  1,
                label: false
            }
        },
        legend: {
            show: false
        },
        grid: {
            hoverable: true
        },
        tooltip: true,
        tooltipOpts: {
            content: function(label, xval, yval, flotItem){
                return label + ": <b>$" + yval.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + "</b>"
            },
            shifts: {
                x: -60,
                y: 25
            },
            defaultTheme : false
        }
    };

    // Plot the chart and set options
    var plotChart = $.plot(placeholder, data, options);
    if (isNaN(plotChart.getData()[0].percent)){
        var canvas = plotChart.getCanvas();
        var ctx = canvas.getContext("2d");
        var x = canvas.width / 2;
        var y = canvas.height / 2;
        ctx.textAlign = 'center';
        ctx.fillText('No Data for this date range', x, y);
    }
    $("#mCSB_3_scrollbar_vertical").mCustomScrollbar({
        autoHideScrollbar:true,
        scrollbarPosition: "outside",
        theme:"dark"
    });
}
function ObtenerColor(color){
    let colores={
        '0':'#4596f1',
        '1':'#f17d45',
        '2':'#3FFF33',
        '3':'#FFFFFF',
        'default':'#000000'
    }
    return colores[color]||colores['default']
}
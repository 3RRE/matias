const ADMINISTRATIVO_URI=''
const TOKEN_ADMINISTRATIVO=''
let itemsExcel=[]
let arrayItemsConsolidado=[]
let itemsConsolidado=[]
$(document).ready(function(){
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'YYYY-MM-DD',
        defaultDate: dateNow,
        maxDate: dateNow,
    })
    $("#txtFechaSincronizacion").datetimepicker({
        pickTime: false,
        format: 'YYYY-MM-DD',
        defaultDate: dateNow,
        maxDate: dateNow,
    })
    $("#cboSala").select2({
        placeholder:'Seleccione Sala'
    })
    $("#cboProgresivo").select2({
        placeholder:'Seleccione Misterioso'
    })
    listarSalas().then(result=>{
        renderSalas(result.data)
    })
    $(document).on('change','#cboSala',function(e){
        e.preventDefault()
        let salas=$(this).val()
        if(salas.length>0){
            listarProgesivosPorSala(salas).then(result=>{
                if(result){
                    renderProgresivos(result)
                }
            })
        }
        else{
            //limpiar combos
            $("#cboProgresivo").select2('destroy')
            $("#cboProgresivo").html('')
            $("#cboProgresivo").append('<option><option>')
            $("#cboProgresivo").select2({
                placeholder:'Seleccione Misterioso'
            })
        }
    })
    $(document).on('click','#btnBuscar',function(e){


        itemsExcel = []
        let progresivos=$("#cboProgresivo").val()
        if(progresivos.length>0){
            listarReporteProgresivos(progresivos).then(response=>{
                if(response){
                    itemsExcel=response
                    renderReporte(response)
                    $('#overlaySearch').hide()
                }
                else{
                    toastr.error('No se encontraron resultados','Mensaje')
                }
            })
        }
        else{
            toastr.warning('Seleccione progresivo','Mensaje')
        }
    })
    $(document).on('click','#btnExcel',function(e){
        if(itemsExcel.length>0){
            let fechaIni = $("#txtFechaInicio").val().split('-')
		    let fechaFin = $("#txtFechaFin").val().split('-')
		    let periodo = `De ${fechaIni[2]}/${fechaIni[1]}/${fechaIni[0]} al ${fechaFin[2]}/${fechaFin[1]}/${fechaFin[0]}`
            let dataExcel={
                Periodo:periodo,
                Items:itemsExcel
            }
            // dataExcel.Items[0].dataConsolidado=itemsConsolidado
            GenerarExcel(dataExcel)
            // ExportToExcelAdministrativo('api/progresivo/exporttoexcelreportecontrolprogresivo', JSON.stringify(dataExcel))
			// .then((response) => {
			// 	var blob = new Blob([response], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'});
	        //    	var downloadUrl = URL.createObjectURL(blob);
	        //    	var a = document.createElement("a");
	        //    	a.href = downloadUrl;
	        //    	a.download = 'reporte-control-progresivo.xlsx';
	        //     document.body.appendChild(a);
	        //     a.click();	
			// })
			// .catch((err) => {
			// 	toastr.error('Ocurrio un error','Mensaje')
			// });
        }
    })
    $(document).on('click','#btnSincronizar',function(e){
        e.preventDefault()
        $("#full-modalSincronizar").modal('show')
    })
})
function listarSalas(){
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
            return result
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    })
    AbortRequest.open()
    return ajaxhr
}
function renderSalas(data){
    $("#cboSala").select2('destroy')
    $("#cboSala").html('')
    if(data){
        let span=[]
        data.map(item=>{
            span.push(`<option value="${item.CodSala}" >${item.Nombre}</option>`)
        })
        $("#cboSala").prepend(span.join(''))
        $("#cboSala").select2({
            allowClear:true,
            placeholder:'Seleccione Sala'
        })
    }
}
function listarProgesivosPorSala(listaSalas){
    let dataForm={
        salas:listaSalas
    }
    ajaxhr = $.ajax({
        type: "POST",
        url: basePath + "ControlProgresivo/ListarProgresivosSala",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data:JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            return result
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    })
    AbortRequest.open()
    return ajaxhr
}
function renderProgresivos(data){
    $("#cboProgresivo").select2('destroy')
    $("#cboProgresivo").html('')
    if(data){
        let span=[]
        data.map(item=>{
            span.push(`<option value="${item.CodSala}-${item.CodSalaProgresivo}">${item.NombreSala} - ${item.Nombre} </option>`)
        })
        $("#cboProgresivo").prepend(span.join(''))
        $("#cboProgresivo").select2({
            allowClear:true,
            placeholder:'Seleccione Misterioso'
        })
    }
}
function listarReporteProgresivos(listaProgresivos){
    if(listaProgresivos){
        let salas=$("#cboSala").val()
        let fechaInicio=$("#txtFechaInicio").val()
        let fechaFin=$("#txtFechaFin").val()
        let dataForm={
            salas:salas,
            misteriosos:listaProgresivos,
            fechaIni:fechaInicio,
            fechaFin:fechaFin
        }
        ajaxhr = $.ajax({
            type: "POST",
            url: basePath + "ControlProgresivo/getProgresivoControl",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                return result
            },
            error: function (request, status, error) {
            },
            complete: function (resul) {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            }
        })

        AbortRequest.open()
        return ajaxhr

        // return $.ajax({
        //     url: ADMINISTRATIVO_URI + `api/progresivo/getProgresivoControl`,
        //     //url: ADMINISTRATIVO_URI + `odata/empresa?$select=CodEmpresa,RazonSocial&$filter=(CodEmpresa eq 13) or (CodEmpresa eq 4)&$orderby=RazonSocial&$expand=SalaCollection`,
        //     method: 'POST',
        //     contentType: "application/json; charset=utf-8",
        //     data: JSON.stringify(dataForm),
        //     dataType: "json",
        //     headers: {
        //         "Authorization": "Basic " + TOKEN_ADMINISTRATIVO
        //       },
        //     beforeSend:function(xhr){
        //         $.LoadingOverlay('show')
        //     },
        //     success: function (result) {
        //         return result
        //     },
        //     complete: function (xhr) {
        //         $.LoadingOverlay('hide')
        //     }
        // })
    }
    
}
function renderReporte(data){
    $("#contenedorReporte").html('')
    if(data){
        let span=data.map((item,index)=>{
            let dataError = item.dataError
            let strError = ''
            if(dataError){
                strError = ` <span class="text-danger">(${dataError.Descripcion} - ${moment(dataError.FechaRegistro).format('YYYY-MM-DD HH:mm:ss A')})</span>`
            }
            let titulo=`<h5 class="" style="textTransform:uppercase">${item.Empresa} - ${item.Sala} - ${item.Misterioso} - <span class="text-primary">Coin In : ${item.CoinIn}</span>${strError}</h5>`
            //tabla Principal
            let dataPrincipal=item.data
            let bodyTablaPrincipal=[]
            if(dataPrincipal){
                bodyTablaPrincipal = dataPrincipal.map(row=>{
                    return `
                        <tr>
                            <td class="text-right">${row.NroPozo}</td>
                            <td class="text-left">${row.Pozo}</td>
                            <td class="text-right">${Number(row.Incremento).toFixed(2)}</td>
                            <td class="text-right">${Number(row.subido).toFixed(2)}</td>
                            <td class="text-right">${Number(row.basePozoHist).toFixed(2)}</td>
                            <td class="text-right">${Number(row.Jackpot).toFixed(2)}</td>
                            <td class="text-right">${Number(row.TotalDisplay).toFixed(2)}</td>
                            <td class="text-right">${Number(row.RepSala).toFixed(2)}</td>
                            <td class="text-right">${Number(row.TotalDisplay - row.RepSala).toFixed(2)}</td>
                        </tr>
                    `
                })
                
            }
            let tablaPrincipal=`
            <div class="table-responsive">
                <table id="table-principal-${index}" style="background: #fff"
                    class="table table-condensed table-bordered table-striped table-hover no-margin table-fixed">
                    <thead>
                        <tr>
                            <th>Nro Pozo</th>		
                            <th>Pozo</th>		
                            <th class="sum">% Inc.</th>
                            <th class="sum">Subido</th>
                            <th class="sum">Base</th>
                            <th class="sum">Jackpots</th>
                            <th class="sum">Tot. Display</th>
                            <th class="sum">Rep. Salas</th>
                            <th class="sum">Diferencia</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${bodyTablaPrincipal.join('')}
                    </tbody>
                    <tfoot class="simple-table-footer">
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </tfoot>
                </table>
            </div>
            `
            //tabla Secundaria
            let dataSec=item.dataSec
            let bodyTablaSecundaria=[]
            if(dataSec){
                bodyTablaSecundaria = dataSec.map((row,rowIndex)=>{
                    return `
                    <tr>
                        <td class="text-right">${rowIndex + 1}</td>
                        <!--<td>${row.Fecha.substring(0, 10)}</td>-->
                        <td class="text-right">${moment(row.Fecha).format("YYYY-MM-DD HH:mm:ss A")}</td>
                        <td class="text-right">${row.NroMistery}</td>
                        <td class="text-right">${Number(row.Premio).toFixed(2)}</td>
                        <td class="text-right">${row.baseGanador}</td>
                        <td class="text-right">${Number(row.pagar).toFixed(2)}</td>
                        <td>${row.estado}</td>
                    </tr>
                    `
                })
                
            }
            let tablaSecundaria=`
            <div class="table-responsive">
                <table id="table-secundaria-${index}" style="background: #fff"
                    class="table table-condensed table-bordered table-striped table-hover no-margin">
                    <thead>
                        <tr>
                            <th>N° Premio</th>		
                            <th>Fecha</th>
                            <th class="sum">N° Mystery</th>
                            <th class="sum">Premio</th>
                            <th class="sum">Base</th>
                            <th class="sum">Pagar</th>
                            <th>Estado</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${bodyTablaSecundaria.join('')}
                    </tbody>
                </table>
            </div>
            `
            //tabla consolidado
            let bodyTablaConsolidado=[]
            itemsConsolidado=[]
            if(dataPrincipal && dataSec){
                bodyTablaConsolidado=dataPrincipal.map(item=>{
                    let ganadoresPozo=dataSec.filter(element=>element.NroMistery==item.NroPozo)
                    let totalGanadores=ganadoresPozo?ganadoresPozo.length:0
                    let totalPremio=0,totalBase=0,totalPagar=0
                    if(totalGanadores>0){
                        totalPremio=ganadoresPozo.map(i=>i.Premio).reduce((prev,curr)=>prev+curr,0)
                        totalBase=ganadoresPozo.map(i=>i.baseGanador).reduce((prev,curr)=>prev+curr,0)
                        totalPagar=ganadoresPozo.map(i=>i.pagar).reduce((prev,curr)=>prev+curr,0)
                    }
                    itemsConsolidado.push({
                        "NroPozo":item.NroPozo,
                        "totalGanadores":totalGanadores,
                        "totalPremio":totalPremio,
                        "totalBase":totalBase,
                        "totalPagar":totalPagar
                    })
                    return `
                        <tr>
                            <td class="text-right">${item.NroPozo}</td>
                            <td class="text-right">${totalGanadores}</td>
                            <td class="text-right">${totalPremio.toFixed(2)}</td>
                            <td class="text-right">${totalBase.toFixed(2)}</td>
                            <td class="text-right">${totalPagar.toFixed(2)}</td>
                        </tr>
                    `
                })
            }
            itemsExcel[index].dataConsolidado=itemsConsolidado
            let tablaConsolidado=`
                <div class="table-responsive">
                    <table id="table-consolidado-${index}" style="background:#fff"
                    class="table table-condensed table-bordered table-striped table-hover no-margin">
                        <thead>
                            <tr>
                                <th>Nro. Pozo</th>
                                <th>Cant. Ganadores</th>
                                <th>Total Premio</th>
                                <th>Total Base</th>
                                <th>Total Pagar</th>
                            </tr>
                        </thead>
                        <tbody>
                            ${bodyTablaConsolidado.join('')}
                        </tbody>
                    </table>
                </div>
            `
            let html=`
            <div id="progresivo-container">
                <div class="mb-2">
                 ${titulo}
                </div>
                <hr/>
                <div style="width:100%;display:flex;justify-content:center;align-items:center">
                    <h5>POZOS</h5>
                </div>
                ${tablaPrincipal}
                <hr/>
                <div style="width:100%;display:flex;justify-content:center;align-items:center">
                    <h5>GANADORES</h5>
                </div>
                ${tablaSecundaria}
                <div style="width:100%;display:flex;justify-content:center;align-items:center">
                    <h5>GANADORES POR POZO</h5>
                </div>
                <hr/>
                ${tablaConsolidado}
            </div>
        `
            return html
        })
        
        $("#contenedorReporte").html(span.join(''))
        data.map((item,index)=>{
            $("#table-principal-"+index).DataTable({
                "oLanguage": {
                    "sEmptyTable": "No hay registros",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sInfo": "Mostrando _START_ de _END_ de _TOTAL_ registros.",
                    "sSearch": "Buscar",
                    "oPaginate": {
                        "sPrevious": "Anterior",
                        "sNext": "Siguiente"
                    }
                },
                "bDestroy": true,
                bInfo : true,
                autoWidth: false,
                paging: true,
                searching: true,
                ordering: true,
                columnDefs: [
                    {width: "100px", targets: 0,className:'text-right'},
                    {width: "150px", targets: 1,className:'text-right'},
                    {width: "250px", targets: 2,className:'text-right'},
                    {width: "100px", targets: 3,className:'text-right'},
                    {width: "150px", targets: 4,className:'text-right'},
                    {width: "100px", targets: 5,className:'text-right'},
                    {width: "100px", targets: 6,className:'text-right'},
                    {width: "100px", targets: 7,className:'text-right'},
                    {width: "100px", targets: 8,className:'text-right'}
                ],
                initComplete: function(settings, json) {
                    this.api().columns('.sum').every(function(){
                        var column = this;
                        // Remove the formatting to get integer data for summation
                        var intVal = function ( i ) {
                            if(typeof i === 'string'){
                                if($(i).text() == '' || $(i).text() == undefined) {
                                    return i.replace(/[\$,]/gi, '')*1;
                                } else {
                                    return Number($(i).text());
                                }
                            } else {
                                return typeof i === 'number' ? i : 0;
                            }  
                            //return typeof i === 'string' ?
                                //i.replace(/[\$,]/gi, '')*1 : typeof i === 'number' ? i : 0;
                        };
        
                        if (column.data().length > 0) {
                            var sum = column
                                .data()
                                .reduce(function (a, b) {
                                    return intVal(a) + intVal(b);
                                });
                            $(column.footer()).addClass("text-right").css({ "font-weight": "bold" }).html(intVal(sum).toFixed(2));
                        } else {
                            $(column.footer()).addClass("text-right").css({ "font-weight": "bold" }).html(Number("0").toFixed(2));
                        }
                        
                    });
                }
            })
            $("#table-secundaria-"+index).DataTable({
                "oLanguage": {
                    "sEmptyTable": "No hay registros",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sInfo": "Mostrando _START_ de _END_ de _TOTAL_ registros.",
                    "sSearch": "Buscar",
                    "oPaginate": {
                        "sPrevious": "Anterior",
                        "sNext": "Siguiente"
                    }
                },
                "bDestroy": true,
                bInfo : true,
                autoWidth: false,
                scrollX: false,
                scrollCollapse: false,
                // paging: false,
                searching: true,
                ordering: true,
                columnDefs: [
                    {width: "150px", targets: 0},
                    {width: "250px", targets: 1},
                    {width: "100px", targets: 2},
                    {width: "150px", targets: 3},
                    {width: "100px", targets: 4},
                    {width: "100px", targets: 5},
                    {width: "100px", targets: 6}
                ],
                initComplete: function(settings, json) {
                    this.api().columns('.sum').every(function(){
                        var column = this;
                        // Remove the formatting to get integer data for summation
                        var intVal = function ( i ) {
                            if(typeof i === 'string'){
                                if($(i).text() == '' || $(i).text() == undefined) {
                                    return i.replace(/[\$,]/gi, '')*1;
                                } else {
                                    return Number($(i).text());
                                }
                            } else {
                                return typeof i === 'number' ? i : 0;
                            }  
        
                        };
                        
                        if(column.data().length > 0) {
                            //console.log("paso", column.data())
                            var sum = column
                            .data()
                            .reduce(function (a, b) { 
                                return intVal(a) + intVal(b);
                            });	
        
                            $(column.footer()).html(intVal(sum).toFixed(2));
                        } else {
                            $(column.footer()).html(Number("0").toFixed(2));
                        }
                        
                    });
                }
            })
        })
    }
}
function ExportToExcelAdministrativo(url, data,codsala=-1) {
    
    var promise = new Promise(function(resolve, reject) {
        var xhr = new XMLHttpRequest();
        xhr.open('POST', `${ADMINISTRATIVO_URI}/${url}`, true);
        xhr.responseType = 'blob';
        xhr.setRequestHeader('Authorization', `Basic ${TOKEN_ADMINISTRATIVO}`);
         xhr.setRequestHeader('SalaId', codsala);
        xhr.setRequestHeader('Content-type', 'application/json;charset=UTF-8');

        xhr.onload  = function(e) {
            if (this.status >= 200 && this.status < 300) {
                resolve(this.response);
                //http://stackoverflow.com/questions/16086162/handle-file-download-from-ajax-post
                //application/vnd.ms-excel
                //http://stackoverflow.com/questions/30008114/how-do-i-promisify-native-xhr
            } else {
                if(this.status == 401) {
                    var blob = new Blob([this.response], {type: 'text/plain'});
                    var myReader = new FileReader();
                    myReader.onload = function(e) {
                        let result = JSON.parse(myReader.result);
                        if(result.hasOwnProperty('Authenticated')) {
                            if(!result.Authenticated) {
                                // LoginStore.removeToken();
                                // RouterContainer.get().transitionTo('/login');
                                console.log('sesion terminada')
                            } else {
                                reject("Ud. ¡No esta autorizado!");
                            }
                        }
                    }
                    myReader.readAsText(blob);
                    //reject("Ud. ¡No esta autorizado!");
                } else {
                    reject(xhr.statusText);
                }	            	
            }
        };

        xhr.onerror = function() {
            reject(xhr.statusText);
        }

        xhr.send(JSON.stringify(data));

    });
    return promise
}
function GenerarExcel(data){
    if(data){
        data=JSON.stringify(data)
        let url="ControlProgresivo/ExportToExcelReporteControlProgresivo"
        ajaxhr = $.ajax({
            type: "POST",
            cache: false,
            url: basePath + url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ jsonData:data }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
    
            success: function (response) {
                if (response.respuesta) {
                    var data = response.data;
                    var file = response.excelName;
                    let a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/vnd.ms-excel;base64, " + data;
                    a.download = file;
                    a.click();
                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            //error: function (request, status, error) {
            //    toastr.error("Error De Conexion, Servidor no Encontrado.");
            //},
            complete: function (resul) {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            }
        })
        AbortRequest.open()
    }
}
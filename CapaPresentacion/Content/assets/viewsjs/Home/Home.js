let mostrarDashboard=false
//const URI_OFISIS='http://localhost/ExtranetPJ/'
const URI_OFISIS='http://181.65.130.36:2222/ExtranetPJ/'
let chartTotalClienteSala
let chartPorAnio
let limite=0
$(document).on('ready',function(){
  
    ObtenerDataWidgets().then(data=>{
        RenderizarWidgets(data)
    })
    ObtenerSalasYTotalClientes().then(data=>{
        RenderizarChart(data)
        RenderizarWidgetPorSala(data)
    })
    ObtenerSalas()
    ObtenerListaCumpleaniosTrabajadoresOfisis('').then(result=>{
        if(result){
            RenderilzarListaCumpleaniosOfisis(result)
        }
    })
    $(document).on('change','#cboSalaCumpleanios',function(e){
        e.preventDefault()
        let CodSala=$(this).val()
        ObtenerListaCumpleaniosClientesPorSala(CodSala).then(function(result){
            if(result){
                RenderizarListaCumpleanios(result)
            }
        })
    })
    $(document).on('change','#cboOfisisCumpleanios',function(e){
        e.preventDefault()
        let CO_EMPR=$(this).val()
        if(CO_EMPR=='todos'){
            CO_EMPR=''
        }
        ObtenerListaCumpleaniosTrabajadoresOfisis(CO_EMPR).then(result=>{
            if(result){
                RenderilzarListaCumpleaniosOfisis(result)
            }
        })
    })
    ObtenerEmpresas()
    // renderizarEjemplo()
    let anio=new Date().getFullYear()
    GetTotalClientesPorAnio(anio).then(res=>{
        renderizarEjemplo(res,anio)
    })
    $(document).on('click','.btnAnio',function(e){
        let anio=$(this).text()
        GetTotalClientesPorAnio(anio).then(res=>{
            if(res){
                let arrayCategories=[]
                let arrayData=[]
                months.map((item,index)=>{
                    arrayCategories.push(item)
                    let dataCantidad=res.find(x=>x.mes-1==index)
                    
                    arrayData.push(dataCantidad?dataCantidad.cantidad:0)
                })

                // res.map(item=>{
                //     arrayCategories.push(months[item.mes-1])
                //     arrayData.push(item.cantidad)
                // })
                chartPorAnio.update({
                    chart:{
                        inverted:false,
                        polar:false
                    },
                    subtitle: {
                        text: 'AÑO: '+anio,
                        align: 'center'
                    },
                    xAxis: {
                        categories:arrayCategories,
                    },
                    series: [{
                        type: 'column',
                        name: 'Nuevos Clientes',
                        colorByPoint: true,
                        data:arrayData,
                        showInLegend: false
                    }],
                })
                
            }  
        })
    })
})
function renderizarEjemplo(data,anio){
    let arrayCategories=[]
    let arrayData=[]
    months.map((item,index)=>{
        arrayCategories.push(item)
        let dataCantidad=data.find(x=>x.mes-1==index)
        
        arrayData.push(dataCantidad?dataCantidad.cantidad:0)
    })
   
    // data.map(item=>{
    //     arrayCategories.push(months[item.mes-1])
    //     arrayData.push(item.cantidad)
    // })
    chartPorAnio = Highcharts.chart('container', {
        title: {
            text: 'Nuevos Clientes Por Año',
            align: 'center'
        },
        subtitle: {
            text: 'AÑO: '+anio,
            align: 'center'
        },
        xAxis: {
            categories:arrayCategories,
            // categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
        },
        series: [{
            type: 'column',
            name: 'Nuevos Clientes',
            colorByPoint: true,
            data:arrayData,
            // data: [5412, 4977, 4730, 4437, 3947, 3707, 4143, 3609,
            //     3311, 3072, 2899, 2887],
            showInLegend: false
        }],
        plotOptions: {
            column: {
                dataLabels: {
                    enabled: true,
                    format: '{y}'
                }
            }
        }
    }, function(charts) { // on complete
            charts.renderer.text(`<button class="btn btn-danger btn-sm btnAnio" data-anio="2021">2021</button>
            <button class="btn btn-danger btn-sm btnAnio" data-anio="2022">2022</button>
            <button class="btn btn-danger btn-sm btnAnio" data-anio="2023">2023</button>
            <button class="btn btn-danger btn-sm btnAnio" data-anio="2024">2024</button>`, 10, 20,true)
                .css({
                    color: '#4572A7',
                    fontSize: '16px'
                })
                .add()
    
        }
    );
    
   
}
function ObtenerDataWidgets(){
    return $.ajax({
        url: basePath+"AsistenciaCliente/GetDataWidgets",
        type: "POST",
        contentType: "application/json",
        beforeSend: function (xhr) {
        },
        success: function (response) {
           return response
        },
        complete: function (resul) {
        }
    })
}
function ObtenerSalasYTotalClientes(){
    return $.ajax({
        url: basePath+"AsistenciaCliente/GetListadoSalasYTotalClientes",
        type: "POST",
        contentType: "application/json",
        beforeSend: function (xhr) {
        },
        success: function (response) {
           return response
        },
        complete: function (resul) {
        }
    })
}
function RenderizarChart(res){
    if(res){
        let dataCategories=[]
        let dataValues=[]
        let result=res.result.map(item=>{
            dataCategories.push(item.Nombre)
            dataValues.push(item.CantidadClientes)
        })
        chartTotalClientesSala=Highcharts.chart('contenedorChartTotalClienteSala', {
            chart: {
                type: 'column',
                zoomType: 'y'
            },
            title: {
                text: 'CLIENTES POR SALA'
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                categories: dataCategories,
                title: {
                    text: null
                },
                accessibility: {
                    description: 'Salas'
                }
            },
            yAxis: {
                min: 0,
                tickInterval: 2,
                title: {
                    text: 'Clientes en miles'
                },
                labels: {
                    overflow: 'justify',
                    format: '{value}'
                }
            },
            plotOptions: {
                column: {
                    dataLabels: {
                        enabled: true,
                        format: '{y}'
                    }
                }
            },
            tooltip: {
                valueSuffix: '',
                stickOnContact: true,
                backgroundColor: 'rgba(255, 255, 255, 0.93)'
            },
            legend: {
                enabled: false
            },
            series: [
                {
                    name: 'Clientes',
                    data: dataValues,
                    borderColor: '#5997DE'
                }
            ]
        });
    }
  
}
function ObtenerSalas(){
    $("#cboSalaCumpleanios").html('')
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
        },
        success: function (result) {
            let datos = result.data
            $("#cboSalaCumpleanios").append('<option value="">Seleccione</option>')
            $.each(datos, function (index, value) {
                $("#cboSalaCumpleanios").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>')
            });
            $("#cboSalaCumpleanios").select2({
                multiple: false, placeholder: "--Seleccione--",
            })
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
        }
    });
    return false;
}
function ObtenerListaCumpleaniosClientesPorSala(CodSala){
    if(CodSala){
        let dataForm={
            CodSala:CodSala
        }
        return $.ajax({
            url: basePath+"AsistenciaCliente/GetListaCumpleanios",
            type: "POST",
            contentType: "application/json",
            data:JSON.stringify(dataForm),
            beforeSend: function (xhr) {
            },
            success: function (response) {
               return response
            },
            complete: function (resul) {
            }
        })
    }
}
function RenderizarListaCumpleanios(data){
    if(data){
        $("#tableSalaCumpleanios").html('')
        $(".block-content-inner").mCustomScrollbar("destroy");
        let listaCumpleanios=data.result
        let fechaActual=moment(new Date()).format('DD/MM')

        let arrayCumpleanios=listaCumpleanios.map((item)=> {
            let fechaNacimiento=moment(item.FechaNacimiento).format('DD/MM')
            let cumpleanioDelDia=(fechaNacimiento==fechaActual)
            return `
            <tr class="email-status-unread" data-email-id="1" data-email-url="#" style="color:${(cumpleanioDelDia?'#2b982b':'#000000')};font-weight:${(cumpleanioDelDia?'bolder':'normal')}">
                <td class="email-sender" align="left">
                    ${item.NombreCompleto}
                </td>
                <td class="email-datetime" align="center">
                    ${fechaNacimiento}
                </td>
            </tr>
            `
        })
        $("#tableSalaCumpleanios").append(
            `<tbody>
                ${arrayCumpleanios.join('')}
            </tbody>
            `
        )
    }
}
function RenderizarWidgets(data){
    $("#widgetTotalClientes").html('')
    $("#widgetTotalSalasActivas").html('')
    $("#widgetTotalSalas").html('')
    if(data){
        $("#widgetTotalClientes").html(`
        <div class="c-widget c-widget-quick-info c-widget-size-small highlight-color-blue" style="cursor: pointer;">
            <div class="c-widget-icon">
                <span class="icon icon-user"></span>
            </div>
            <div class="c-wdiget-content-block">
                <div class="c-widget-content-heading" style="font-size:28px;color:blue" >
                    ${data.totalClientes}
                </div>
                <div class="c-widget-content-sub" style="color:black">
                    TOTAL CLIENTES
                </div>
            </div>
        </div>
        `)
        $("#widgetTotalSalasActivas").html(`
        <div class="c-widget c-widget-quick-info c-widget-size-small highlight-color-green" style="cursor: pointer;">
            <div class="c-widget-icon">
                <span class="icon icon-star"></span>
            </div>
            <div class="c-wdiget-content-block">
                <div class="c-widget-content-heading" style="font-size:28px;color:green">
                    ${data.totalSalasActivas}
                </div>
                <div class="c-widget-content-sub" style="color:black">
                    SALAS ACTIVAS
                </div>
            </div>
        </div>
        `)
        $("#widgetTotalSalas").html(`
            <div class="c-widget c-widget-quick-info c-widget-size-small highlight-color-red" style="cursor: pointer;">
                <div class="c-widget-icon">
                    <span class="icon icon-grids"></span>
                </div>
                <div class="c-wdiget-content-block">
                    <div class="c-widget-content-heading" style="font-size:28px;color:red">
                        ${data.totalSalas}
                    </div>
                    <div class="c-widget-content-sub" style="color:black">
                        TOTAL SALAS
                    </div>
                </div>
            </div>
        `)
    }
} 
function ObtenerEmpresas(){
    $("#cboOfisisCumpleanios").html('')
    $.post( URI_OFISIS + "ofisis/ListarEmpresas", function( result ) {
        let datos = result.data
        $("#cboOfisisCumpleanios").append('<option value="">SELECCIONE</option>')
        $("#cboOfisisCumpleanios").append('<option value="todos">TODOS</option>')
        $.each(datos, function (index, value) {
            $("#cboOfisisCumpleanios").append('<option value="' + value.CO_EMPR + '"  >' + value.DE_NOMB + '</option>')
        });
        $("#cboOfisisCumpleanios").select2({
            multiple: false, placeholder: "--SELECCIONE--",
        })
      })
    return false;
}
function ObtenerListaCumpleaniosTrabajadoresOfisis(CO_EMPR){
    return $.post( URI_OFISIS + "ofisis/ListarCumpleaniosOfisis?CO_EMPR="+CO_EMPR, function( result ) {
       return result
      })
}
function RenderilzarListaCumpleaniosOfisis (data){
    if(data){
        console.log(data)
        $("#tableOfisisCumpleanios").html('')
        $(".block-content-inner").mCustomScrollbar("destroy");
        let listaCumpleanios=data.data
        let fechaActual=moment(new Date()).format('DD/MM')
        let arrayCumpleanios=listaCumpleanios.map((item)=> {
            let fechaNacimiento=moment(item.FE_NACI_TRAB).format('DD/MM')
            let cumpleanioDelDia=(fechaNacimiento==fechaActual)
            return `
            <tr class="email-status-unread" data-email-id="1" data-email-url="#" style="color:${(cumpleanioDelDia?'#2b982b':'#000000')};font-weight:${(cumpleanioDelDia?'bolder':'normal')}">
                <td class="email-sender" align="left">
                    ${item.NO_TRAB} ${item.NO_APEL_PATE} ${item.NO_APEL_MATE}
                </td>
                <td class="email-datetime" align="center">
                    ${fechaNacimiento}
                </td>
            </tr>
            `
        })
        $("#tableOfisisCumpleanios").append(
            `<tbody>
                ${arrayCumpleanios.join('')}
            </tbody>
            `
        )
        $('input').iCheck('check');
        // $(".email-item-checkbox").iCheck('check')
    }
}
function RenderizarWidgetPorSala(res){
    let active=false
    let span=''
    let items=res.result.map((item,index)=>{
        let randomColor=getRandomInt(6)
        let randomIcon=getRandomInt(9)
        active=index==0?true:false
        return `
        <div class="item ${(active?"active":"")}">
            <div class="c-widget c-widget-quick-info highlight-color-${arrayColors[randomColor]}" style="cursor: pointer;">
                <div class="c-widget-icon">
                    <span class="icon ${arrayIcons[randomIcon]}"></span>
                </div>
                <div class="c-wdiget-content-block">
                    <div class="c-widget-content-heading" style="font-size:35px;color:${arrayColors[randomColor]}">
                        ${item.CantidadClientes}
                    </div>
                    <div class="c-widget-content-sub" style="color:black">
                        CLIENTES <br> 
                        <span style="font-size:20px;font-weitght:900">${item.Nombre.toUpperCase().replace('SALA ','')}</span>
                    </div>
                </div>
            </div>
        </div>
        `
    })
    $("#carousel-example-generic .carousel-inner").html("")
    $("#carousel-example-generic .carousel-inner").html(items.join(''))

}	
function getRandomInt(max) {
    return Math.floor(Math.random() * max);
  }
let arrayIcons=[
    'icon-button-check',
    'icon-world',
    'icon-yen',
    'icon-waves',
    'icon-three-points',
    'icon-user',
    'icon-sun',
    'icon-social-envato',
    'icon-quote',
    'icon-magnet']
let arrayColors=[
    'blue',
    'green',
    'orange',
    'yellow',
    'red',
    'lime',
    'purple'
]
function GetTotalClientesPorAnio(anio){
    return $.ajax({
        url: basePath+"AsistenciaCliente/GetTotalClientesPorAnio",
        data:JSON.stringify({anio:anio}),
        type: "POST",
        contentType: "application/json",
        beforeSend: function (xhr) {
        },
        success: function (response) {
           return response
        },
        complete: function (resul) {
        }
    })
}
let months = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
"Julio", "Agosto", "Setiembre", "Octubre", "Noviembre", "Diciembre"
]
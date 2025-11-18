$(document).ready(function() {
    ListarData()
})
function ListarData() {
    let url = basePath + "CALLudopata/ReporteLudopatasCliente"
    let data = {}
    let addtabla = $(".contenedor_tabla");
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            respuesta = response.respuesta
            if(respuesta){
                $(addtabla).empty();
                $(addtabla).append('<table id="tableListado" class="table table-striped table-bordered table-hover table-condensed" style="width:100%"></table>');
                objetodatatable = $("#tableListado").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "ordering": true,
                    "scrollCollapse": true,
                    "scrollX": true,
                    "paging": true,
                    "aaSorting": [],
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response.data,
                    columns: [
                        {   data: "LudopataID", title: "LudopataID" },
                        {   data: null, title: "Nombre",
                            'render':function(value,type,oData){
                            return `${oData.Nombre} ${oData.ApellidoPaterno} ${oData.ApellidoMaterno}`
                            } 
                        },
                        {   data:"DNI",title:"NroDoc"},
                        {   data:null,title:"FechaRegistro",
                            'render':function(value,type,oData){
                                let fechaRegistro=moment(oData.FechaRegistro).format("DD/MM/YYYY")
                                if(fechaRegistro=='31/12/1752'||fechaRegistro=='01/01/1753'){
                                    return ''
                                }
                                return moment(oData.FechaRegistro).format("DD/MM/YYYY HH:mm:ss A")
                            }
                        },
                        {   data:null,title:"Estado",
                            'render':function(value,type,oData){
                                let estado = "INACTIVO";
                                let css = "btn-danger";
                                if (oData.Estado == 1) {
                                    estado = "ACTIVO"
                                    css = "btn-success";
                                }
                                 return '<span class="label ' + css + '">' + estado + '</span>';
                            }
                        },
                        {   data:"Cliente.Id",title:"ClienteID"},
                        {   data:null,title:"NombreCliente",
                            'render':function(value,type,oData){
                                return (oData.Cliente.NombreCompleto==''?`${oData.Cliente.Nombre} ${oData.Cliente.ApelPat} ${oData.Cliente.ApelMat}`:oData.Cliente.NombreCompleto)
                            }
                        },
                        {   data:"Cliente.NroDoc",title:"NroDocCliente"},
                        {   data:null,title:"FechaRegistroCliente",
                            'render':function(value,type,oData){
                                let fechaRegistro=moment(oData.Cliente.FechaRegistro).format("DD/MM/YYYY")
                                if(fechaRegistro=='31/12/1752'||fechaRegistro=='01/01/1753'){
                                    return ''
                                }
                                return moment(oData.Cliente.FechaRegistro).format("DD/MM/YYYY HH:mm:ss A")
                            }
                        },
                        {   data:null,title:"FechaCorreo",
                            'render':function(value,type,oData){
                                let fechaEnvio=moment(oData.FechaEnvioCorreo).format("DD/MM/YYYY")
                                if(fechaEnvio=='31/12/1752'||fechaEnvio=='01/01/1753'){
                                    return ''
                                }
                                return moment(oData.FechaEnvioCorreo).format("DD/MM/YYYY HH:mm:ss A")
                            }
                        }
                    ],
                    "drawCallback": function (settings) {
                    },
    
                    "initComplete": function (settings, json) {
                    },
                })
            }
            else{
                toastr.error(respuesta.mensaje,"Mensaje Servidor");
            }
          
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
        }
    })
}
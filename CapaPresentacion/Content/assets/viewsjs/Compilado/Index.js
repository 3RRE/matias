$(document).ready(function () {
    let jsonExample=`[
    {
        "BD_ONLINE": "Data Source=(local);Initial Catalog=BD_ONLINE;Integrated Security=False;User ID=sa;Password=123456"
    },
    {
        "BD_TECNOLOGIAS": "Data Source=(local);Initial Catalog=BD_TECNOLOGIAS;Integrated Security=False;User ID=sa;Password=123456"
    },
    {
        "IntranetExtranetBD_S3K": "Data Source=(local);Initial Catalog=IntranetExtranetBD_S3K;Integrated Security=False;User ID=sa;Password=123456"
    },
    {
        "msdb": "Data Source=(local);Initial Catalog=msdb;Integrated Security=False;User ID=sa;Password=123456"
    },
    {
        "bd_s3k_playertracking": "Data Source=(local);Initial Catalog=bd_s3k_playertracking;Integrated Security=False;User ID=sa;Password=123456"
    },
    {
        "BD_SEGURIDAD_PJ": "Data Source=(local);Initial Catalog=BD_SEGURIDAD_PJ;Integrated Security=False;User ID=sa;Password=123456"
    },
    {
        "DEFAULT": ""
    }
]`
    $("#txtJson").val(jsonExample)
    $(document).on('click','#btnGenerar',function(){
        let jsonStr=$("#txtJson").val()
        let validado=validarJson(jsonStr)
        if(validado){
            generarDLL(jsonStr)
        }
        else{
            toastr.error('Json Incorrecto',"Mensaje Servidor")
        }
    })
})
function validarJson (jsonStr){
    try {
        objeto = JSON.parse(jsonStr);
        return true
    }
    catch (error) {
        return false
    }    
}
function generarDLL(jsonStr){
    let dataForm={
        jsonStr:jsonStr
    }

    $.ajax({
        type: "POST",
        url: basePath + "Compilado/GenerarCompilado",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data:JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if(result.respuesta){
                var data = result.base64Str
                let file = result.fileName
                let a = document.createElement('a')
                a.target = '_self'
                a.href = "data:application/zip;base64, " + data
                a.download = file
                a.click();
            }
           else{
            toastr.error('Ha ocurrido un erro',"Mensaje Servidor")
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
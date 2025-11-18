$(document).ready(function () {
    console.log(logoSala)
    if(logoSala){
        // let logo=ObtenerImgDrive(logoSala)
        // logo.then(response=>{
        //     $(".img_logo").attr('src','data:image/png;base64,'+response.data)
        // })
        $(".img_logo").attr('src',logoSala)
    }
    $(document).on('click', "#btnPdf", function (e) {
        var doc = $(this).data("doc");
        let a = document.createElement('a');
        a.target = '_self';
        a.href = basePath + "Documento/" + doc;
        a.click();
    });
});
function ObtenerImgDrive(RutaArchivoLogo){
    let dataForm={ RutaArchivoLogo:RutaArchivoLogo}
    return $.ajax({
        type: "POST",
        url: basePath + "Sala/GetImgPorIdDrive",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data:JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            // $.LoadingOverlay("show");
        },
        success: function (result) {
            return result
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
            // $.LoadingOverlay("hide");
        }
    });
}


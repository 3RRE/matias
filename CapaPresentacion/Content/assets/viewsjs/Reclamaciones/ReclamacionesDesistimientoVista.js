
$(document).ready(function(e){
  
    let canvas = document.getElementById('signature-pad')

    // Adjust canvas coordinate space taking into account pixel ratio,
    // to make it look crisp on mobile devices.
    // This also causes canvas to be cleared.
    function resizeCanvas() {
        // When zoomed out to less than 100%, for some very strange reason,
        // some browsers report devicePixelRatio as less than 1
        // and only part of the canvas is cleared then.
        let ratio =  Math.max(window.devicePixelRatio || 1, 1)
        canvas.width = canvas.offsetWidth * ratio
        canvas.height = canvas.offsetHeight * ratio
        canvas.getContext("2d").scale(ratio, ratio)
    }
    
    window.onresize = resizeCanvas;
    resizeCanvas();
    
    let signaturePad = new SignaturePad(canvas, {
      backgroundColor: 'rgb(255, 255, 255)' // necessary for saving image as JPEG; can be removed is only saving as PNG or SVG
    })

    if(documentoDesistido==1){
        signaturePad.fromDataURL("data:image/png;base64,"+base64Desistimiento);
        $("#NroDocumentoDesistimiento").val(nroDocumentoDesistimiento)
        $("#NroDocumentoDesistimiento").attr("readonly", "readonly");
    }

    $(document).on('click','#btnLimpiarFirma',function(e){
        e.preventDefault()
        signaturePad.clear()
    })
    $(document).on('click','#btnEnviarDesistimiento',function(e){
        e.preventDefault()
        let nroDocumento=$("#NroDocumentoDesistimiento").val()
        if(nroDocumento==''||nroDocumento==null||nroDocumento==undefined){
            toastr.warning("Debe Ingresar un numero de Documento", "Mensaje Servidor");
            return false
        }
        if (signaturePad.isEmpty()) {
            toastr.warning("Debe Ingresar una firma")
            return false
        }
        
        let data = signaturePad.toDataURL('image/jpeg');
        let dataForm={
            Firma:data,
            NroDocumento:nroDocumento,
            hashReclamacion:hashReclamacion
        }
        $.ajax({
            url: basePath + "Reclamacion/ReclamacionDesistimientoGuardarJson",
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
                    setTimeout(function () {
                        window.location.reload(true);
                    }, 1500);
    
                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        });

    })
   
})
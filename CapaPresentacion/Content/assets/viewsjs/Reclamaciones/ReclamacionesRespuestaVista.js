$(document).ready(function () {

    $(document).on('click', "#btnPdf", function (e) {
        var doc = $(this).data("doc");
        let a = document.createElement('a');
        a.target = '_self';
        a.href = basePath + "DocumentoRespuestaLegal/" + doc;
        a.click();
    });

    $(document).on('click', '.download_attachament', function (event) {
        event.preventDefault()

        var guid = $(this).attr('data-guid')
        var attachament = $(this).attr('data-attachament')

        toastr.remove()

        if (!guid) {
            toastr.warning('Por favor, ingrese código')

            return false
        }

        if (!attachament) {
            toastr.warning('Por favor, seleccione archivo')

            return false
        }

        var data = {
            guid,
            attachament
        }

        $.ajax({
            type: "POST",
            url: `${basePath}Reclamacion/DescargarArchivoAdjunto`,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            cache: false,
            beforeSend: function () {
                $.LoadingOverlay("show")
            },
            success: function (response) {
                if (response.success) {
                    var data = response.data
                    var file = response.filename
                    var a = document.createElement('a')
                    var url = `data:application/octet-stream;base64,${data}`

                    a.target = '_self'
                    a.href = url
                    a.download = file
                    a.click()
                } else {
                    toastr.warning(response.message)
                }
            },
            complete: function () {
                $.LoadingOverlay("hide")
            }
        })
    })
});
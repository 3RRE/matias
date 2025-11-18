$(document).ready(function () {
    ObtenerListaSalas();

    $(document).on("click", ".btnContinuar", function () {
        var id = $("#cboSala").val();
        var redirectUrl = basePath + "ReclamacionesNuevo?id="+id;
        if (id != "") {
            window.location.replace(redirectUrl);
        }
        else {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
        }
    });
});

function ObtenerListaSalas() {
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaActivasSinSeguridad",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboSala").val(null).trigger("change");
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
$(document).ready(function () {
    
    var texareaText = CKEDITOR.replace('cke-editor');
    CargarDatos(texareaText);
    $(".btnGuardar").click(function () {
        if ($.trim($("#linkInterno").val()) == "") {
            toastr.error("El Campo Link Interno Es Obligatorio.", { timeOut: 15000 });
            return false;
        }
        if ($.trim($("#linkExterno").val()) == "") {
            toastr.error("El Campo Link Externo Es Obligatorio.", { timeOut: 15000 });
            return false;
        }
        var arrCantidades = [];
        var arrOrdenes = [];
        var flag = false;
        $('.orden').each(function (i, obj) {
            if ($(obj).val() != "") {
                if (parseInt($(obj).val()) >= 0 && parseInt($(obj).val()) <= 4 && parseFloat($(obj).val()) === parseInt(parseFloat($(obj).val()), 10)) {
                    if (parseInt($(obj).val()) != 0) {
                        var objOrd = { valor: parseInt($(obj).val()), tipo: $(obj).data('tipo') };
                        arrOrdenes.push(objOrd);
                    }
                }
                else {
                    toastr.error("Asignación De Orden " + $(obj).val() + " Invalido.", { timeOut: 15000 });
                    flag = true;
                }
            }
            else {
                $(obj).val(0);
            }
        });
        $('.cantidad').each(function (i, obj) {
            if ($(obj).val() != "") {
                if (parseInt($(obj).val()) >= 0 && parseFloat($(obj).val()) === parseInt(parseFloat($(obj).val()), 10)) {
                    if (parseInt($(obj).val()) != 0) {
                        var objCant = { valor: parseInt($(obj).val()), tipo: $(obj).data('tipo') };
                        arrCantidades.push(objCant);
                    }
                }
                else {
                    toastr.error("Asignación De Numero de Letras " + $(obj).val() + " Invalido.", { timeOut: 15000 });
                    flag = true;
                }
            }
            else {
                $(obj).val(0);
            }
        });
        if (flag) {
            return false;
        }
        var arrTiposOrdenes = arrOrdenes.map(a => a.tipo);
        var arrTiposCantidades = arrCantidades.map(a => a.tipo);
        arrTiposOrdenes = arrTiposOrdenes.sort();
        arrTiposCantidades = arrTiposCantidades.sort();
        if (arrTiposOrdenes.toString() !== arrTiposCantidades.toString()) {
            toastr.error("La Asignación De Letras No Coincide Con La Asignacion Del Orden.", { timeOut: 15000 });
            return false;
        }
        var arrValoresOrdenes = arrOrdenes.map(a => a.valor);
        var arrValoresOrdenesSinDuplicados = $.unique(arrValoresOrdenes);
        if (arrOrdenes.length != arrValoresOrdenesSinDuplicados.length) {
            toastr.error("Asignación De Orden Duplicado.", { timeOut: 15000 });
            return false;
        }
        var sum = arrValoresOrdenes.reduce((a, b) => a + b, 0);
        if (sum <= 0) {
            toastr.error("Ingrese Asignacion De Orden.", { timeOut: 15000 });
            return false;
        }
        var arr = arrValoresOrdenes.sort();
        for (var i = 1; i <= arr.length; i++) {
            if (arr[i - 1] != i) {
                var flag = true;
            }
        }
        if (flag) {
            toastr.error("Error En La Numeracion de Asignacion De Orden.", { timeOut: 15000 });
            return false;
        }
        debugger
        var url = basePath + "ConfiguracionSeguridad/GuardarConfiguracionSeguridadJson";
        var linkInterno = $("#linkInterno").val();
        var linkExterno = $("#linkExterno").val();
        var cantidadLetraNombre = $("#cantidadLetraNombre").val();
        var cantidadLetraApePaterno = $("#cantidadLetraApePaterno").val();
        var cantidadLetraApeMaterno = $("#cantidadLetraApeMaterno").val();
        var cantidadLetraDNI = $("#cantidadLetraDNI").val();
        var ordenNombre = $("#ordenNombre").val();
        var ordenApePaterno = $("#ordenApePaterno").val();
        var ordenApeMaterno = $("#ordenApeMaterno").val();
        var ordenDNI = $("#ordenDNI").val();
        var codWebConfiguracionSeguridad = $("#codWebConfiguracionSeguridad").val();
        var mensajeEmail = texareaText.getData();
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                linkInterno: linkInterno, linkExterno: linkExterno,
                cantidadLetraNombre: cantidadLetraNombre, cantidadLetraApePaterno: cantidadLetraApePaterno, cantidadLetraApeMaterno: cantidadLetraApeMaterno, cantidadLetraDNI: cantidadLetraDNI,
                ordenNombre: ordenNombre, ordenApePaterno: ordenApePaterno, ordenApeMaterno: ordenApeMaterno, ordenDNI: ordenDNI, mensajeEmail: mensajeEmail, codWebConfiguracionSeguridad: codWebConfiguracionSeguridad
            }),
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                if (response.data == true) {
                    toastr.success("Se Registro Correctamente", "Mensaje Servidor");
                    $(".btnGuardar").attr("disabled", true);
                    setTimeout(function () { location.reload(); }, 1000);
                }
                else {
                    toastr.error(response.mensaje, { timeOut: 9000 });
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
            }
        });
    });


});

function CargarDatos(texareaText) {
    var url = basePath + "ConfiguracionSeguridad/ObtenerConfiguracionSeguridadJson";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            //debugger
            if (response.data != null) {
                $("#linkInterno").val(response.data.linkInterno);
                $("#linkExterno").val(response.data.linkExterno);
                $("#cantidadLetraNombre").val(response.data.cantidadLetraNombre);
                $("#cantidadLetraApePaterno").val(response.data.cantidadLetraApePaterno);
                $("#cantidadLetraApeMaterno").val(response.data.cantidadLetraApeMaterno);
                $("#cantidadLetraDNI").val(response.data.cantidadLetraDNI);
                $("#ordenNombre").val(response.data.ordenNombre);
                $("#ordenApePaterno").val(response.data.ordenApePaterno);
                $("#ordenApeMaterno").val(response.data.ordenApeMaterno);
                $("#ordenDNI").val(response.data.ordenDNI);
                $("#codWebConfiguracionSeguridad").val(response.data.codWebConfiguracionSeguridad);
                texareaText.setData(response.data.mensajeEmail);
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
        }
    });
}


function editorCKE() {
    if ($('#cke-editor').length) {
        var dqwd = $('#cke-editor').each(function () {
            CKEDITOR.replace('cke-editor');
        });
        // Turn off automatic editor creation first.
        CKEDITOR.disableAutoInline = true;
    }
}
$(document).ready(function () {
    $("#frmMaquinaProgresivo")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                Canal: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                SlotID: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                Toquen: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        },
                    }
                }
            }
        })
        .on('success.field.bv', function (e, data) {
            e.preventDefault();
            var $parent = data.element.parents('.form-group');
            // Remove the has-success class
            $parent.removeClass('has-success');
            // Hide the success icon
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
        });

    ObtenerListaMarcaMaquina();
    ObtenerJuegoMaquina();
    $("#cboMarca").select2();
    $("#cboModelo").select2();
    $("#cboJuego").select2();

    $("#btnListar").click(function () {
        window.location.replace(basePath + "Progresivo/ProgresivoListadoMaquina");
    });

    $("#cboMarca").change(function () {
        var id_marca = $("#cboMarca").val();

        if (id_marca != "") {
            //ObtenerJuegoMaquina(id_marca);
            ObtenerModeloMaquina(id_marca);
            var str = "";
            $("#cboMarca option:selected").each(function () {
                str += $(this).text() + " ";
            });
            $("#nombre_marca").val(str);
        } else {
            $("#nombre_juego").val("");
            $("#nombre_modelo").val("");
            $("#cboJuego").empty();
            $("#cboModelo").empty();
            $("#cboJuego").append('<option value="">--Seleccione--</option>');
            $("#cboModelo").append('<option value="">--Seleccione--</option>');
        }        
    });

    $("#cboModelo").change(function () {
        var str = "";
        $("#cboModelo option:selected").each(function () {
            str += $(this).text() + " ";
        });
        $("#nombre_modelo").val(str);
    });

    $("#cboJuego").change(function () {
        var str = "";
        $("#cboJuego option:selected").each(function () {
            str += $(this).text() + " ";
        });
        $("#nombre_juego").val(str);
    });

    $('#btnGuardar').click(function () {

        if (!$("#url_sala").val().trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        } 

        $("#frmMaquinaProgresivo").data('bootstrapValidator').resetForm();
        var validar = $("#frmMaquinaProgresivo").data('bootstrapValidator').validate();
        if (validar.isValid()) {

            //
            var url_ = $("#url_sala").val() + "/servicio/GrabarMaquina?codProgresivo=" + $("#codProgresivo").val();
            var url = basePath + "Progresivo/ConsultarGuardarMaquinaProgresivo";
            var redirectUrl = basePath + "Progresivo/ProgresivoListadoMaquina";
            var dataForm = $('#frmMaquinaProgresivo').serializeFormJSON();
            dataForm.Toquen = String($("#txtToquen").val()).split('.').join(',');
            
            //
            var datasend = { parametros: dataForm, url: url_ };
            //enviarDataPost(url, datasend, false, false, redirectUrl, false);
            var js2;
            js2 = $.confirm({
                icon: 'fa fa-spinner fa-spin',
                title: 'Esta Seguro que desea Registrar una nueva Maquina ?',
                theme: 'black',
                animationBounce: 1.5,
                columnClass: 'col-md-6 col-md-offset-3',
                confirmButtonClass: 'btn-info',
                cancelButtonClass: 'btn-warning',
                confirmButton: "confirmar",
                cancelButton: 'Aún No',
                content: "",
                confirm: function () {
                    ajaxhr = $.ajax({
                        type: "POST",
                        cache: false,
                        url: url,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(datasend),
                        beforeSend: function (xhr) {
                            $.LoadingOverlay("show");
                        },
                        success: function (response) {
                            setCookie("datainicial", "");
                            $.when(dataAuditoria(1, "#frmMaquinaProgresivo", 3, "Progresivo/ConsultarGuardarMaquinaProgresivo", "NUEVA MAQUINA PROGRESIVO")).then(function (response, textStatus) {
                                enviarDataPost(url, dataForm, false, false, redirectUrl, true);
                            });
                            var estado = response.data;

                            if (estado === "true") {
                                toastr.success('Se ha registrado satisfactoriamente', 'Mensaje Servidor');
                                setTimeout(function () {
                                    window.location.replace(basePath + "Progresivo/ProgresivoListadoMaquina");
                                }, 2200);
                            } else {
                                toastr.error('No se pudo registrar la maquina', 'Mensaje Servidor');
                            }

                            //response = response.data;
                            
                        },
                        error: function (request, status, error) {
                            toastr.error("Error De Conexion, Servidor no Encontrado.");
                        },
                        complete: function (resul) {
                            AbortRequest.close()
                            $.LoadingOverlay("hide");
                        }
                    });
                    AbortRequest.open()

                },
                cancel: function () {
                    
                },

            });
        }
    });  
});



function ObtenerListaMarcaMaquina() {
    if (!$("#url_sala").val().trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url_ = $("#url_sala").val() + "/servicio/getMarcas?codProgresivo=" + $("#codProgresivo").val();
    ajaxhr = $.ajax({
        type: "POST",
        url: basePath + "Progresivo/ConsultarListaMarcaMaquinaJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url_ }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            $("#cboMarca").empty();
            $("#cboMarca").append('<option value="">--Seleccione--</option>');
            $.each(datos, function (index, value) {
                $("#cboMarca").append('<option value="' + value.MarcaID + '"  data-id="' + value.MarcaID + '"  >' + value.Nombre + '</option>');
            });
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
    return false;
}

function ObtenerModeloMaquina(id) {
    if (!$("#url_sala").val().trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url_ = $("#url_sala").val() + "/servicio/ListarModelosidmarca?codProgresivo=" + $("#codProgresivo").val() + "&idmarca=" + id;
    ajaxhr = $.ajax({
        type: "POST",
        url: basePath + "Progresivo/ConsultarListaModeloMaquinaJson/",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url_ }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            $("#cboModelo").empty();
            $("#cboModelo").append('<option value="">--Seleccione--</option>');
            $.each(datos, function (index, value) {
                $("#cboModelo").append('<option value="' + value.ModeloID + '"  data-id="' + value.ModeloID + '"  >' + value.Nombre + '</option>');
            });
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
    return false;
}

function ObtenerJuegoMaquina() {
    $("#cboJuego").empty();
    $("#cboJuego").append('<option value="">--Seleccione--</option>');
    if (!$("#url_sala").val().trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url_ = $("#url_sala").val() + "/servicio/ListarMaquinas?codProgresivo=" + $("#codProgresivo").val() + "&maquina=&estado=-1";
   ajaxhr =  $.ajax({
        type: "POST",
        url: basePath + "Progresivo/ConsultarListaJuegoMaquinaJson/",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ url: url_ }),
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            $.each(datos, function (index, value) {
                $("#cboJuego").append('<option value="' + value.Nombre + '"  data-id="' + value.Nombre + '"  >' + value.Nombre + '</option>');
            });
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
       complete: function (resul) {
           AbortRequest.close()
            $.LoadingOverlay("hide");
        }
   });
    AbortRequest.open()
    return false;
}

function GuardarMarcaMaquina()
{
    if (!$("#url_sala").val().trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url_ = $("#url_sala").val() + "/servicio/GrabarMarcaMaquina?codProgresivo=" + $("#codProgresivo").val();
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Progresivo/ConsultarGuardarMarcaMaquinaProgresivo",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            'MarcaID': $("#cboMarca").val(),
            'Nombre' : $("#nombre_marca").val(),
            'url' : url_
        }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            response = response.data;
            //window.location.replace(basePath + "Progresivo/ProgresivoListadoMaquina");
            console.log(response);
        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
}
VistaAuditoria("Progresivo/ProgresivoRegistrarMaquina", "VISTA", 0, "", 3);
//function ObtenerMarcaMaquina() {
//    debugger
//    if (!$("#url_sala").val().trim()) {
//        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
//        return false;
//    } 
//    var url_ = $("#url_sala").val() + "/servicio/ObtenerMarcaMaquina?codProgresivo=" + $("#codProgresivo").val() + "&id_marca=" + $("#cboMarca").val();
//    $.ajax({
//        type: "POST",
//        cache: false,
//        url: basePath + "Progresivo/ConsultarObtenerMarcaMaquinaProgresivo",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        data: JSON.stringify({ url: url_ }),
//        beforeSend: function (xhr) {
//            $.LoadingOverlay("show");
//        },
//        success: function (response) {
//            debugger
//            response = response.data;
//            if (response.MarcaID == "0") {
//                GuardarMarcaMaquina();
//            }
//        },
//        error: function (request, status, error) {
//            toastr.error("Error De Conexion, Servidor no Encontrado.");
//        },
//        complete: function (resul) {
//            $.LoadingOverlay("hide");
//        }
//    });
//}

/////nuevoosss

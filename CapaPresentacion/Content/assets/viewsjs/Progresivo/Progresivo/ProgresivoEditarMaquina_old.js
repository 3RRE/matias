var pass = 0
$(document).ready(function () {
    $("#frmEditarMaquinaProgresivo")
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


   
    ListarDatosMaquina();

    $("#cboMarca").select2();
    $("#cboModelo").select2();
    $("#cboJuego").select2();
    $("#cboEstado").select2();
    $("#btnListar").click(function () {
        window.location.replace(basePath + "Progresivo/ProgresivoListadoMaquina");
    });

    $("#cboMarca").change(function () {
        var id_marca = $("#cboMarca").val();

        if (id_marca != "") {
            //ObtenerJuegoMaquina(id_marca);
            ObtenerModeloMaquinaGet(id_marca,-1);
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


    $("#btnGuardar").click(function () {
        if (!$("#url_sala").val().trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }
        $("#frmEditarMaquinaProgresivo").data('bootstrapValidator').resetForm();
        var validar = $("#frmEditarMaquinaProgresivo").data('bootstrapValidator').validate();
        if (validar.isValid()) {

            var url_ = $("#url_sala").val() + "/servicio/ModificarMaquina?codProgresivo=" + $("#codProgresivo").val();
            var url = basePath + "Progresivo/ConsultarModificarMaquinaProgresivo";
            var redirectUrl = basePath + "Progresivo/ProgresivoListadoMaquina";
            var dataForm = $('#frmEditarMaquinaProgresivo').serializeFormJSON();
            dataForm.Toquen = String($("#txtToquen").val()).split('.').join(',');

            //debugger
            var datasend = { parametros: dataForm, url: url_ };
            var js2;
            js2 = $.confirm({
                icon: 'fa fa-spinner fa-spin',
                title: 'Esta Seguro que desea Modificar la Maquina ?',
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
                            dataAuditoria(1, "#frmEditarMaquinaProgresivo", 3, "Progresivo/ConsultarModificarMaquinaProgresivo", "ACTUALIZAR MAQUINA PROGRESIVO");
                            var estado = response.data;
                            if (estado === "true") {
                                toastr.success('Se ha actualizado satisfactoriamente', 'Mensaje Servidor');
                                setTimeout(function () {
                                    window.location.replace(basePath + "Progresivo/ProgresivoListadoMaquina");
                                }, 2200);
                            } else {
                                toastr.error('No se pudo actualizar la maquina', 'Mensaje Servidor');
                            }
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
            //ModificarMaquina
        }
    });   
    
});


function ListarDatosMaquina() {
    
    var idmaquina = $("#txtSlotID").val();
    var urls = $("#url_sala").val();
    var codprogresivo = $("#codProgresivo").val();

    var url_ = urls + "/servicio/ObtenerMaquina?codProgresivo=" + codprogresivo + "&maquina=" + idmaquina;    
    ajaxhr =$.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Progresivo/ConsultarObtenerMaquinaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url_ }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            
            response = response.data;
           // $("#cboMarca").val(response.MarcaId).change();
            $("#txtcodigo_alterno").val(response.codigo_alterno);
            $("#txtToquen").val(response.Toquen);
            $("#txtCanal").val(response.Canal);
            $("#cboEstado").val(response.Estado).change();
            //$("#nombre_marca").val(response.);
            //$("#nombre_modelo").val(response.);
            $("#nombre_juego").val(response.Juego);
            pass = 1;
            
            ObtenerListaMarcaMaquina(response.MarcaId);
            ObtenerModeloMaquinaGet(response.MarcaId, response.ModeloID);
            ObtenerJuegoMaquinaGet(response.Juego);
            //$.when(ObtenerModeloMaquinaGet(response.MarcaId, response.ModeloID),
            //ObtenerJuegoMaquinaGet(response.MarcaId, response.Juego) ).then(function (response, textStatus) {
            //    $.when(VistaAuditoria("Progresivo/ProgresivoEditarMaquina", "VISTA", 1, "#frmEditarMaquinaProgresivo", 3)).then(function (response, textStatus) {
            //        dataAuditoria(0, "#frmEditarMaquinaProgresivo", 3);
            //    });
            //});

           
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
};

function ObtenerListaMarcaMaquina(MarcaId) {
    var url_ = $("#url_sala").val() + "/servicio/getMarcas?codProgresivo=" + $("#codProgresivo").val();
    ajaxhr =$.ajax({
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
            var select = "";
            $.each(datos, function (index, value) {
                if (MarcaId == value.MarcaID) {
                    select = "selected";
                }
                else {
                    select = "";
                }
                $("#cboMarca").append('<option ' + select + ' value="' + value.MarcaID + '"  data-id="' + value.MarcaID + '"  >' + value.Nombre + '</option>');
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

function ObtenerModeloMaquinaGet(marca_id, ModeloID) {
    var url_ = $("#url_sala").val() + "/servicio/ListarModelosidmarca?codProgresivo=" + $("#codProgresivo").val() + "&idmarca=" + marca_id;
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
            var select = "";
            $.each(datos, function (index, value) {
                if (ModeloID == value.ModeloID) {
                    select = "selected";
                }
                else {
                    select = "";
                }
                $("#cboModelo").append('<option ' + select+' value="' + value.ModeloID + '"  data-id="' + value.ModeloID + '"  >' + value.Nombre + '</option>');
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

function ObtenerJuegoMaquinaGet(juego) {
    $("#cboJuego").empty();
    $("#cboJuego").append('<option value="">--Seleccione--</option>');
    var url_ = $("#url_sala").val() + "/servicio/ListarMaquinas?codProgresivo=" + $("#codProgresivo").val() + "&maquina=&estado=-1";
    ajaxhr =$.ajax({
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
                if (juego == value.Nombre) {
                    select = "selected";
                }
                else {
                    select = "";
                }
                $("#cboJuego").append('<option ' + select+' value="' + value.Nombre + '"  data-id="' + value.Nombre + '"  >' + value.Nombre + '</option>');
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


//VARIABLES
var arrayProblemas = [];



$(document).ready(function () {

    obtenerCorreos().done(response => {
        if (response.data) {
            renderCorreos(response.data)
        }
    })

    ObtenerSalasPorUsuario();
    ObtenerClasificacionProblemas();






    //CAMBIO VALOR SELECT CLASIFICACION PROBLEMAS
    $("#cboClasificacionProblemas").change(function () {

        arrayProblemas = $("#cboListaProblemas").val();

        let dataSelectLista = $(this).val();
        if (dataSelectLista) {
            $("#cboListaProblemas").empty();
            CargarListaProblemas(dataSelectLista);
        }
    });

    //CAMBIO VALOR SELECT CLASIFICACION SALAS
    $("#cboSala").change(function () {
        let codSala = $(this).val();
        if (codSala) {
            $('#cboMaquina').empty();
            ListarMaquinasPorIdSala(codSala);
        }
    });

    //CAMBIO VALOR SELECT MAQUINA
    $("#cboMaquina").change(function () {
        let codMaquina = $(this).val();
        if (codMaquina) {
            ObtenerDetalleMaquina(codMaquina);
        }
    });



    //ENVIO FORMULARIO
    $(document).on("click", ".btnGuardar", function () {


        $("#frmRegistroMaquinaInoperativa").data('bootstrapValidator').resetForm();
        var validar = $("#frmRegistroMaquinaInoperativa").data('bootstrapValidator').validate();
        if (validar.isValid()) {

            let listaCorreos = $("#cboCorreos").val();
            let CodMaquina = $("#cboMaquina").val();
            let MaquinaLey = $("#cboMaquina option:selected").text();
            let CodSala = $("#cboSala").val();
            let ObservacionCreado = $("#Observaciones").val();
            let CodEstadoInoperativa = $("#cboEstado").val();
            let CodPrioridad = $("#cboPrioridad").val();
            let CodEstadoProceso = 1;

            let listaProblemas = $("#cboListaProblemas").val();

            let MaquinaModelo = $("#mod").text();
            let MaquinaLinea = $("#lin").text();
            let MaquinaSala = $("#sal").text();
            let MaquinaJuego = $("#jue").text();
            let MaquinaNumeroSerie = $("#numse").text();
            let MaquinaPropietario = $("#pro").text();
            let MaquinaFicha = $("#fic").text();
            let MaquinaMarca = $("#mar").text();
            let MaquinaToken = $("#tok").text();
            let NombreZona = $("#zon").text();

            $.ajax({
                type: "POST",
                url: basePath + "MIMaquinaInoperativa/MaquinaInoperativaGuardarJson",
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ CodMaquina, MaquinaLey, MaquinaModelo, MaquinaLinea, MaquinaSala, NombreZona, MaquinaJuego, MaquinaNumeroSerie, MaquinaPropietario, MaquinaFicha, MaquinaMarca, MaquinaToken, CodSala, ObservacionCreado, CodEstadoInoperativa, CodPrioridad, CodEstadoProceso, listaProblemas, listaCorreos }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (result) {

                    if (result.respuesta) {
                        //enviarCorreoProcesoMaquinaInoperativa(CodSala);
                        toastr.success(result.mensaje, "Mensaje Servidor");
                        window.location.assign(basePath + "MaquinasInoperativasV2/MaquinaInoperativaV2");
                    } else {

                        toastr.error(result.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (request, status, error) {
                    toastr.error("Error", "Mensaje Servidor");
                },
                complete: function (resul) {
                    $.LoadingOverlay("hide");
                }
            });

        }


    });

})




//FUNCIONES



const obtenerCorreos = () => {

    return $.ajax({
        type: "POST",
        url: `${basePath}/MIMaquinaInoperativa/ListarUsuarioCorreos`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            return response
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })

   
}

function renderCorreos(data) {

    var element = "#cboCorreos"
    data.forEach(item => {
        $(element).append(`<option value="${item.CodUsuario}">${item.Mail} (${item.UsuarioNombre})</option>`)
    })

    jQuery(element).multiselect({
        multiple: true,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        maxHeight: 400,
        buttonContainer: '<div></div>',
        buttonClass: '',
        templates: {
            button: '<div class="form-control form-multiselect input-sm multiselect" data-toggle="dropdown"><span class="multiselect-selected-text"></span></div>'
        },
        nonSelectedText: '--Seleccione--',
        nSelectedText: 'seleccionados',
        allSelectedText: 'Todo seleccionado',
        selectAllText: 'Seleccionar todos'
    })
}


const ObtenerSalasPorUsuario = () => {

    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
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
                placeholder: "--Seleccione--"

            });
            $("#cboSala").val(null).trigger("change");
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
            LimpiarFormValidator();
        }
    });
    return false;
}


const ObtenerClasificacionProblemas = () => {
    $.ajax({
        type: "POST",
        url: basePath + "MICategoriaProblema/ListarCategoriaProblemaActiveJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboClasificacionProblemas").append('<option value="' + value.CodCategoriaProblema + '"  >' + value.Nombre + '</option>');
            });
            $("#cboClasificacionProblemas").select2({
                multiple: true, placeholder: "--Seleccione--", allowClear: true

            });
            $("#cboClasificacionProblemas").val(null).trigger("change");
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
            LimpiarFormValidator();
        }
    });
    return false;
}


const CargarListaProblemas = (lista) => {

    $.ajax({
        type: "POST",
        url: basePath + "MIProblema/ListarProblemaxCategoriaListaJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ listaCategoriaProblema: lista }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboListaProblemas").append('<option value="' + value.CodProblema + '"  >' + value.Nombre + '</option>');
            });
            $("#cboListaProblemas").select2({
                multiple: true, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboListaProblemas").val(null).trigger("change");
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");

            $("#cboListaProblemas").val(arrayProblemas);
            $("#cboListaProblemas").change();
            LimpiarFormValidator();
        }
    });
    return false;
}


const ListarMaquinasPorIdSala = (codigoSala) => {
    console.log(codigoSala)
    $.ajax({
        type: "POST",
        url: basePath + "MIMaquinaInoperativa/ListarMaquinasAdministrativoxSala",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ cod: codigoSala }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {

                if ($.trim(value.CodMaquinaLey) != "") {
                    $("#cboMaquina").append('<option value="' + value.CodMaquina + '"  >' + value.CodMaquinaLey + '</option>');
                }

            });
            $("#cboMaquina").select2({
                placeholder: "--Seleccione--",

            });
            $("#cboMaquina").val(null).trigger("change");
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
            LimpiarFormValidator();
        }
    });
    return false;
}

const ObtenerDetalleMaquina = (codigoMaquina) => {
    $.ajax({
        type: "POST",
        url: basePath + "MIMaquinaInoperativa/ListarMaquinaDetalleAdministrativo",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codMaquina: codigoMaquina }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var maquina = result.data;


            $("#lin").text(maquina.NombreLinea);
            $("#numse").text(maquina.NroSerie);
            $("#jue").text(maquina.NombreJuego);
            $("#sal").text(maquina.NombreSala);
            $("#zon").text(maquina.Zona.Nombre);
            $("#mod").text(maquina.NombreModeloMaquina);
            $("#pro").text(maquina.DescripcionContrato);
            $("#fic").text(maquina.NombreFicha);
            $("#mar").text(maquina.NombreMarcaMaquina);
            $("#tok").text(maquina.Token);
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





const LimpiarFormValidator = () => {
        $("#frmRegistroMaquinaInoperativa").parent().find('div').removeClass("has-error");
        $("#frmRegistroMaquinaInoperativa").parent().find('i').removeAttr("style").hide();
        $("#frmRegistroMaquinaInoperativa").parent().find('.fa').removeAttr("style").show();
}









//VALIDACION FORMULARIO
$("#frmRegistroMaquinaInoperativa")
    .bootstrapValidator({
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            codSala: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            codMaquina: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            Linea: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            NroSerie: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            Sala: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            Modelo: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            Propietario: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            Ficha: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            Marca: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            Token: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            codClasificacionProblemas: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            codListaProblemas: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            estado: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            
            Observaciones: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            prioridad: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },


        }
    })
    .on('success.field.bv', function (e, data) {
        e.preventDefault();
        var $parent = data.element.parents('.form-group');
        $parent.removeClass('has-success');
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });



function enviarCorreoProcesoMaquinaInoperativa(CodSala) {



    $.ajax({
        type: "POST",
        url: basePath + "MIMaquinaInoperativa/EnviarCorreoMaquinaInoperativa",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodSala, CodTipo: 1 }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {

            if (result.respuesta) {


                toastr.success(result.mensaje, "Mensaje Servidor");
                console.log("Correos Enviados");

            } else {

                console.log(result.mensaje);
            }




        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

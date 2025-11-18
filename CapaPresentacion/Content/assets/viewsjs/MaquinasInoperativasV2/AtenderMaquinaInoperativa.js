

var arrayProblemas = [];
var arrayProblemasNuevo = [];

$(document).ready(function () {
    obtenerCorreos().done(response => {
        if (response.data) {
            renderCorreos(response.data)
        }
    })
    VistaAuditoria("MIMaquinaInoperativa/MaquinaInoperativaEditarVista", "VISTA", 0, "", 3);


    var array = [];

    GetSalas();
    LoadClasificacionProblemas();
    LoadClasificacionProblemasNuevo();

    let _fechaInoperativa = moment(maquinaInoperativa.FechaInoperativa).format('DD/MM/YYYY hh:mm:ss A');

    $(document).on("click", ".btnListar", function () {

        window.location.assign(basePath + "MaquinasInoperativasV2/ListaMaquinasInoperativasCreadas");

    });

    $("#fechaInoperativa").datetimepicker({
        pickTime: true,
        format: 'DD/MM/YYYY hh:mm:ss A',
        autoclose: true,
        formatDate: 'DD-MM-YYYY HH:mm',
    });

    $("#fechaInoperativa").data("DateTimePicker").setDate(_fechaInoperativa)


    //SET DATA MAQUINA

    $("#cboMaquina").append('<option value="' + maquinaInoperativa.CodMaquina + '"  >' + maquinaInoperativa.MaquinaLey + '</option>');
    $("#cboMaquina").val(maquinaInoperativa.CodMaquina);
    $("#cboMaquina").change();

    $("#lin").text(maquinaInoperativa.MaquinaLinea);
    $("#numse").text(maquinaInoperativa.MaquinaNumeroSerie);
    $("#jue").text(maquinaInoperativa.MaquinaJuego);
    $("#sal").text(maquinaInoperativa.MaquinaSala);
    $("#mod").text(maquinaInoperativa.MaquinaModelo);
    $("#pro").text(maquinaInoperativa.MaquinaPropietario);
    $("#fic").text(maquinaInoperativa.MaquinaFicha);
    $("#mar").text(maquinaInoperativa.MaquinaMarca);
    $("#tok").text(maquinaInoperativa.MaquinaToken);
    $("#Observaciones").val(maquinaInoperativa.ObservacionCreado);


    $("#cboClasificacionProblemas").change(function () {

        arrayProblemas = $("#cboListaProblemas").val();

        let cod = $(this).val();
        if (cod) {
            $("#cboListaProblemas").empty();
            LoadListaProblemas(cod);
        }
    });


    $("#cboClasificacionProblemasNuevo").change(function () {

        arrayProblemasNuevo = $("#cboListaProblemasNuevo").val();

        let cod = $(this).val();
        if (cod) {
            $("#cboListaProblemasNuevo").empty();
            LoadListaProblemasNuevo(cod);
        }
    });



  


    $("#frmRegistroMaquinaInoperativa")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                atencion: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                problemaReal: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                ObservacionAtencionNuevo: {
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

    $(document).on("click", ".btnGuardar", function () {

        console.log(maquinaInoperativa.CodMaquinaInoperativa)
        $("#frmRegistroMaquinaInoperativa").data('bootstrapValidator').resetForm();
        var validar = $("#frmRegistroMaquinaInoperativa").data('bootstrapValidator').validate();
        if (validar.isValid()) {


            let listaCorreos = $("#cboCorreos").val();
            let codMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
            console.log(codMaquinaInoperativa)
            let listaProblemasNuevo = $("#cboListaProblemasNuevo").val();
            let tipoAtencion = $("#cboAtencion").val();
            console.log(tipoAtencion)

            let observacionAtencionNuevo = $("#ObservacionAtencionNuevo").val();
            let ist = $("#IST").val();



            let CodSala = $("#cboSala").val();

            $.ajax({
                type: "POST",
                url: basePath + "MaquinasInoperativasV2/MaquinaInoperativaAtenderSolucionadoJson",
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ codMaquinaInoperativa, tipoAtencion, observacionAtencionNuevo, listaProblemasNuevo, ist,listaCorreos }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (result) {

                    if (result.respuesta) {
                        //if (tipoAtencion == 1) {
                        //    enviarCorreoProcesoMaquinaInoperativa(CodSala, 2, codMaquinaInoperativa);
                        //}
                        //if (tipoAtencion == 2) {
                        //    enviarCorreoProcesoMaquinaInoperativa(CodSala, 3, codMaquinaInoperativa );
                        //}
                        toastr.success(result.mensaje, "Mensaje Servidor");
                        window.location.assign(basePath + "MaquinasInoperativasV2/ListaMaquinasInoperativasCreadas");
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



    //SET DATA

    $("#cboEstado").val(maquinaInoperativa.CodEstadoInoperativa);
    $("#cboEstado").change();
    $("#cboPrioridad").val(maquinaInoperativa.CodPrioridad);
    $("#cboPrioridad").change();
    $("#Tecnico").val(maquinaInoperativa.TecnicoCreado);
        

    //console.log(array);

    $("#tablePieza").DataTable().clear().draw();
    $("#tablePieza").DataTable().destroy();





});


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

function LoadClasificacionProblemasNuevo() {

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
                $("#cboClasificacionProblemasNuevo").append('<option value="' + value.CodCategoriaProblema + '"  >' + value.Nombre + '</option>');
            });
            $("#cboClasificacionProblemasNuevo").select2({
                multiple: true, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboClasificacionProblemasNuevo").val(null).trigger("change");
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

function LoadListaProblemasNuevo(lista) {


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
                $("#cboListaProblemasNuevo").append('<option value="' + value.CodProblema + '"  >' + value.Nombre + '</option>');
            });
            $("#cboListaProblemasNuevo").select2({
                multiple: true, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboListaProblemasNuevo").val(null).trigger("change");
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");

            $("#cboListaProblemasNuevo").val(arrayProblemasNuevo);
            $("#cboListaProblemasNuevo").change();
        }
    });
    return false;
}

function LoadClasificacionProblemas() {

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


            $("#cboClasificacionProblemas").val(listaCategoriaProblemas);
            $("#cboClasificacionProblemas").change();

        }
    });
    return false;
}



function LoadListaProblemas(lista) {


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

            $("#cboListaProblemas").val(listaProblemas);
            $("#cboListaProblemas").change();
        }
    });
    return false;
}



function ListarMaquinas(cod) {

    $.ajax({
        type: "POST",
        url: basePath + "MIMaquinaInoperativa/ListarMaquinasAdministrativoxSala",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ cod }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboMaquina").append('<option value="' + value.CodMaquina + '"  >' + value.CodMaquinaLey + '</option>');
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
            $("#cboMaquina").val(maquinaInoperativa.CodMaquina);
            $("#cboMaquina").change();
        }
    });
    return false;
}





function GetSalas() {


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

            $("#cboSala").val(maquinaInoperativa.CodSala);
            $("#cboSala").change();
        }
    });
    return false;

}




//function enviarCorreoProcesoMaquinaInoperativa(CodSala) {

//    $.ajax({
//        type: "POST",
//        url: basePath + "MIMaquinaInoperativa/EnviarCorreoMaquinaInoperativa",
//        cache: false,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        data: JSON.stringify({ CodSala, CodTipo: 3 }),
//        beforeSend: function (xhr) {
//            $.LoadingOverlay("show");
//        },
//        success: function (result) {

//            if (result.respuesta) {


//                toastr.success(result.mensaje, "Mensaje Servidor");
//                console.log("Correos Enviados");

//            } else {

//                console.log(result.mensaje);
//            }




//        },
//        error: function (request, status, error) {
//            toastr.error("Error", "Mensaje Servidor");
//        },
//        complete: function (resul) {
//            $.LoadingOverlay("hide");
//        }
//    });
//}


function enviarCorreoProcesoMaquinaInoperativa(CodSala, CodTipo, codMaquinaInoperativa) {


    console.log(CodSala, CodTipo, codMaquinaInoperativa)
    $.ajax({
        type: "POST",
        url: basePath + "MIMaquinaInoperativa/EnviarCorreoMaquinaInoperativa",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodSala, CodTipo, codMaquinaInoperativa }),
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

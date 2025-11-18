
var objetodatatable1, objetodatatable2;
var arrayProblemas = [];

$(document).ready(function () {

    VistaAuditoria("MIMaquinaInoperativa/MaquinaInoperativaInsertarVista", "VISTA", 0, "", 3);

    $(".divRepuesto").attr('style', 'display:none');

    let objetodatatablePieza;
    var array = [];
    var arrayRepuesto = [];

    GetSalas();
    LoadClasificacionProblemas();
    LoadClasificacionPiezas();
    LoadClasificacionRepuestos();
    DataTablePiezaUpdate(array);
    DataTableRepuestoUpdate(arrayRepuesto);

    let _fecha = new Date();


    $(document).on("click", ".btnListar", function () {

        window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativa");

    });

    $("#fechaInoperativa").datetimepicker({
        pickTime: true,
        format: 'DD/MM/YYYY hh:mm:ss A',
        autoclose: true,
        formatDate: 'DD-MM-YYYY HH:mm',
    });

    $("#fechaInoperativa").data("DateTimePicker").setDate(_fecha)



    $("#cboSala").change(function () {
        let cod = $(this).val();
        if (cod) {
            $('#cboMaquina').empty();
            ListarMaquinas(cod);
        }
    });

    $("#cboMaquina").change(function () {
        let codMaquina = $(this).val();
        if (codMaquina) {
            GetMaquinaDetalle(codMaquina);
        }
    });

    $("#cboEstado").change(function () {
        let estado = $(this).val();

        if (estado == 1) {
            $("#cboPrioridad").val(2);
            $(".divRepuesto").attr('style', 'display:none');
            arrayRepuesto = [];
        }
        if (estado == 2) {
            $("#cboPrioridad").val(3);
            $(".divRepuesto").attr('style', 'display:none');
            arrayRepuesto = [];
        }
        if (estado == 3) {
            $("#cboPrioridad").val(2);
            $(".divRepuesto").attr('style', 'display:none');
            $("#tableRepuesto").DataTable().clear().draw();
            $("#tableRepuesto").DataTable().destroy();
            DataTableRepuestoUpdate(arrayRepuesto);
        }
    });

    $("#cboClasificacionProblemas").change(function () {

        arrayProblemas = $("#cboListaProblemas").val();

        let cod = $(this).val();
        if (cod) {
            $("#cboListaProblemas").empty();
            LoadListaProblemas(cod);
        }
    });

    $("#cboClasificacionPiezas").change(function () {
        let cod = $(this).val();
        if (cod) {
            $('#cboListaPiezas').empty();
            LoadListaPiezas(cod);
        }
    });

    $("#cboClasificacionRepuestos").change(function () {
        let cod = $(this).val();
        if (cod) {
            $('#cboListaRepuestos').empty();
            LoadListaRepuestos(cod);
        }
    });

    //Pieza


    $(document).on("click", ".btnAbrirModalPieza", function () {


        $("#cboClasificacionPiezas").val(null).trigger("change");
        $("#cboListaPiezas").val(null).trigger("change");
        $("#Cantidad").val(0);


        $("#full-modal_pieza_agregar").modal("show");



    });

    $(document).on("click", ".btnAgregar", function () {

        let codPieza = $("#cboListaPiezas").val();
        let pieza = $("#cboListaPiezas option:selected").text();
        let cantidad = $("#Cantidad").val();

        if (!codPieza) {
            toastr.error("Seleccione una pieza.", "Mensaje Servidor");
            return false;
        }
        if (!pieza) {
            toastr.error("Seleccione una pieza.", "Mensaje Servidor");
            return false;
        }
        if (!cantidad || cantidad < 1) {
            toastr.error("Ingrese una cantidad valida.", "Mensaje Servidor");
            return false;
        }

        let repetido = false;

        array.forEach(function (item, index) {
            if (item.CodPieza == codPieza) {
                repetido = true;
                return false;
            }
        });

        if (repetido) {
            toastr.error("Pieza ya agregada.", "Mensaje Servidor");
            return false;
        }

        var data = { CodPieza: codPieza, Pieza: pieza, Cantidad: cantidad };

        array.push(data);

        console.log(array);

        $("#tablePieza").DataTable().clear().draw();
        $("#tablePieza").DataTable().destroy();
        DataTablePiezaUpdate(array);



        $("#full-modal_pieza_agregar").modal("hide");

    });


    $(document).on("click", ".btnEliminar", function () {


        var id = $(this).data("id");

        array = jQuery.grep(array, function (value) {
            return value.CodPieza != id;
        });

        console.log(array);

        $("#tablePieza").DataTable().clear().draw();
        $("#tablePieza").DataTable().destroy();
        DataTablePiezaUpdate(array);

    });


    $(document).on("click", ".btnEditar", function () {



        $("#full-modal_pieza").modal("show");

        var id = $(this).data("id");


        array.forEach(function (item, index) {
            if (item.CodPieza == id) {
                $("#pieza_id").val(item.CodPieza);
                $("#piezaNombre").val(item.Pieza);
                $("#piezaCantidad").val(item.Cantidad);
                return false;
            }
        });


    });


    $(document).on("click", ".btnPieza", function () {

        let codPieza = $("#pieza_id").val();
        let pieza = $("#piezaNombre").val();
        let cantidad = $("#piezaCantidad").val();


        if (!cantidad || cantidad < 1) {
            toastr.error("Ingrese una cantidad valida.", "Mensaje Servidor");
            return false;
        }

        $("#full-modal_pieza").modal("hide");

        array.forEach(function (item, index) {
            if (item.CodPieza == codPieza) {
                item.Pieza = pieza;
                item.Cantidad = cantidad;
                return false;
            }
        });

        console.log(array);

        $("#tablePieza").DataTable().clear().draw();
        $("#tablePieza").DataTable().destroy();
        DataTablePiezaUpdate(array);

    });


    //Repuesto

    $(document).on("click", ".btnAgregarRepuesto", function () {

        let codRepuesto = $("#cboListaRepuestos").val();
        let repuesto = $("#cboListaRepuestos option:selected").text();
        let cantidad = $("#CantidadRepuesto").val();

        if (!codRepuesto) {
            toastr.error("Seleccione un repuesto.", "Mensaje Servidor");
            return false;
        }
        if (!repuesto) {
            toastr.error("Seleccione un repuesto.", "Mensaje Servidor");
            return false;
        }
        if (!cantidad || cantidad < 1) {
            toastr.error("Ingrese una cantidad valida.", "Mensaje Servidor");
            return false;
        }

        let repetido = false;

        arrayRepuesto.forEach(function (item, index) {
            if (item.CodRepuesto == codRepuesto) {
                repetido = true;
                return false;
            }
        });

        if (repetido) {
            toastr.error("Repuesto ya agregado.", "Mensaje Servidor");
            return false;
        }


        /*

        var data = { CodRepuesto: codRepuesto, Repuesto: repuesto, Cantidad: cantidad };

        arrayRepuesto.push(data);

        console.log(arrayRepuesto);

        $("#tableRepuesto").DataTable().clear().draw();
        $("#tableRepuesto").DataTable().destroy();
        DataTableRepuestoUpdate(arrayRepuesto);
        */


        //CODIGO REPUESTO POR ALMACEN

        DataTableRepuestosxAlmacen(codRepuesto,cantidad);

        $("#full-modal_repuesto_almacen").modal("show");







    });

    $(document).on("click", ".btnPedirRepuesto", function () {

        /*
        let codRepuesto = $("#cboListaRepuestos").val();
        let codAlmacen = $(this).data("id");
        let repuesto = $("#cboListaRepuestos option:selected").text();
        let cantidad = $("#CantidadRepuesto").val();

        var data = { CodRepuesto: codRepuesto, CodAlmacen: codAlmacen, Repuesto: repuesto, Cantidad: cantidad };

        arrayRepuesto.push(data);

        console.log(arrayRepuesto);

        $("#tableRepuesto").DataTable().clear().draw();
        $("#tableRepuesto").DataTable().destroy();
        DataTableRepuestoUpdate(arrayRepuesto);

*/


        $("#almacendestino_sala").val($(this).data("salaid"));
        $("#almacendestino_repuesto").val($(this).data("repuestoid"));
        $("#almacendestino_pieza_repuesto_almacen").val($(this).data("piezarepuestoalmacenid"));
        $("#almacendestino_almacen_origen").val($(this).data("id"));
        //$("#almacendestino_cantidad").val($(this).data("cantidad"));
        $("#almacendestino_cantidad").val($("#CantidadRepuesto").val());
        
        
        LoadAlmacenDestino();




        $("#full-modal_almacen_destino").modal("show");




    });

    $(document).on("click", ".btnElegirAlmacenDestino", function () {





        EnviarRepuesto();

        $("#full-modal_almacen_destino").modal("hide");




    });

    $(document).on("click", ".btnDescontarRepuesto", function () {

        let codRepuesto = $("#cboListaRepuestos").val();
        let codAlmacen = $(this).data("id");
        let repuesto = $("#cboListaRepuestos option:selected").text();
        let cantidad = $("#CantidadRepuesto").val();

        var data = { CodRepuesto: codRepuesto, CodAlmacen: codAlmacen, Repuesto: repuesto, Cantidad: cantidad };

        arrayRepuesto.push(data);

        console.log(arrayRepuesto);

        $("#tableRepuesto").DataTable().clear().draw();
        $("#tableRepuesto").DataTable().destroy();
        DataTableRepuestoUpdate(arrayRepuesto); 

        $("#full-modal_repuesto_almacen").modal("hide");



    });

    $(document).on("click", ".btnEliminarRepuesto", function () {


        var id = $(this).data("id");

        arrayRepuesto = jQuery.grep(arrayRepuesto, function (value) {
            return value.CodRepuesto != id;
        });

        console.log(arrayRepuesto);

        $("#tableRepuesto").DataTable().clear().draw();
        $("#tableRepuesto").DataTable().destroy();
        DataTableRepuestoUpdate(arrayRepuesto);

    });


    $(document).on("click", ".btnEditarRepuesto", function () {



        $("#full-modal_repuesto").modal("show");

        var id = $(this).data("id");


        arrayRepuesto.forEach(function (item, index) {
            if (item.CodRepuesto == id) {
                $("#repuesto_id").val(item.CodRepuesto);
                $("#repuestoNombre").val(item.Repuesto);
                $("#repuestoCantidad").val(item.Cantidad);
                return false;
            }
        });


    });


    $(document).on("click", ".btnRepuesto", function () {

        let codRepuesto = $("#repuesto_id").val();
        let repuesto = $("#repuestoNombre").val();
        let cantidad = $("#repuestoCantidad").val();


        if (!cantidad || cantidad < 1) {
            toastr.error("Ingrese una cantidad valida.", "Mensaje Servidor");
            return false;
        }

        $("#full-modal_repuesto").modal("hide");

        arrayRepuesto.forEach(function (item, index) {
            if (item.CodRepuesto == codRepuesto) {
                item.Repuesto = repuesto;
                item.Cantidad = cantidad;
                return false;
            }
        });

        console.log(arrayRepuesto);

        $("#tableRepuesto").DataTable().clear().draw();
        $("#tableRepuesto").DataTable().destroy();
        DataTableRepuestoUpdate(arrayRepuesto);

    });

    //FIN REPUESTO

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
                fechaInoperativa: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                horaInoperativa: {
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
                Tecnico: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                fechaEnvio: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                horaEnvio: {
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


        $("#frmRegistroMaquinaInoperativa").data('bootstrapValidator').resetForm();
        var validar = $("#frmRegistroMaquinaInoperativa").data('bootstrapValidator').validate();
        if (validar.isValid()) {


            let CodMaquina = $("#cboMaquina").val();
            let MaquinaLey = $("#cboMaquina option:selected").text();
            let CodSala = $("#cboSala").val();
            let ObservacionCreado = $("#Observaciones").val();
            let CodEstadoInoperativa = $("#cboEstado").val();
            let CodPrioridad = $("#cboPrioridad").val();
            let FechaInoperativa = $("#fechaInoperativa").val();
            let SoloHoraInoperativa = $("#horaInoperativa").val();
            let TecnicoCreado = $("#Tecnico").val();
            let CodEstadoProceso = 1;
            let SoloFechaEnvio = $("#fechaEnvio").val();
            let SoloHoraEnvio = $("#horaEnvio").val();

            let listaProblemas = $("#cboListaProblemas").val();
            let listaPiezas = $.map(array, function (x) { return x.CodPieza; });
            let listaPiezasCantidad = $.map(array, function (x) { return x.Cantidad; });
            let listaRepuestos = $.map(arrayRepuesto, function (x) { return x.CodRepuesto; });
            let listaRepuestosCantidad = $.map(arrayRepuesto, function (x) { return x.Cantidad; });
            let listaRepuestosAlmacen = $.map(arrayRepuesto, function (x) { return x.CodAlmacen; });


            let MaquinaModelo = $("#mod").text();
            let MaquinaLinea = $("#lin").text();
            let MaquinaSala = $("#sal").text();
            let MaquinaJuego = $("#jue").text();
            let MaquinaNumeroSerie = $("#numse").text();
            let MaquinaPropietario = $("#pro").text();
            let MaquinaFicha = $("#fic").text();
            let MaquinaMarca = $("#mar").text();
            let MaquinaToken = $("#tok").text();

            $.ajax({
                type: "POST",
                url: basePath + "MIMaquinaInoperativa/MaquinaInoperativaGuardarJson",
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ CodMaquina, MaquinaLey, MaquinaModelo, MaquinaLinea, MaquinaSala, MaquinaJuego, MaquinaNumeroSerie, MaquinaPropietario, MaquinaFicha, MaquinaMarca, MaquinaToken, CodSala, ObservacionCreado, CodEstadoInoperativa, CodPrioridad, FechaInoperativa, SoloHoraInoperativa, TecnicoCreado, CodEstadoProceso, SoloFechaEnvio, SoloHoraEnvio, listaProblemas, listaPiezas, listaRepuestos, listaPiezasCantidad, listaRepuestosCantidad }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (result) {

                    if (result.respuesta) {
                        enviarCorreoProcesoMaquinaInoperativa(CodSala);
                        toastr.success(result.mensaje, "Mensaje Servidor");
                        window.location.replace(basePath + "MIMaquinaInoperativa/MaquinaInoperativaInsertarVista");
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

            /*


            console.log(array.length);

            if (array.length > 0) {

                let CodMaquina = $("#cboMaquina").val();
                let CodMaquinaLey = $("#cboMaquina option:selected").text();
                let CodSala = $("#cboSala").val();
                let ObservacionInoperativa = $("#Observaciones").val();
                let CodEstadoInoperativa = $("#cboEstado").val();
                let CodPrioridad = $("#cboPrioridad").val();
                let SoloFechaInoperativa = $("#fechaInoperativa").val();
                let SoloHoraInoperativa = $("#horaInoperativa").val();
                let Tecnico = $("#Tecnico").val();
                let CodEstadoProceso = 1;
                let SoloFechaEnvio = $("#fechaEnvio").val();
                let SoloHoraEnvio = $("#horaEnvio").val();

                let listaProblemas = $("#cboListaProblemas").val();
                let listaPiezas = $.map(array, function (x) { return x.CodPieza; });
                let listaPiezasCantidad = $.map(array, function (x) { return x.Cantidad; });
                let listaRepuestos = $.map(arrayRepuesto, function (x) { return x.CodRepuesto; });
                let listaRepuestosCantidad = $.map(arrayRepuesto, function (x) { return x.Cantidad; });
                let listaRepuestosAlmacen = $.map(arrayRepuesto, function (x) { return x.CodAlmacen; });


                $.ajax({
                    type: "POST",
                    url: basePath + "MIMaquinaInoperativa/MaquinaInoperativaGuardarJson",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ CodMaquina, CodMaquinaLey, CodSala, ObservacionInoperativa, CodEstadoInoperativa, CodPrioridad, SoloFechaInoperativa, SoloHoraInoperativa, Tecnico, CodEstadoProceso, SoloFechaEnvio, SoloHoraEnvio, listaProblemas, listaPiezas, listaRepuestos, listaPiezasCantidad, listaRepuestosCantidad }),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (result) {

                        if (result.respuesta) {

                            toastr.success(result.mensaje, "Mensaje Servidor");
                            window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativa");
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



                console.log("Guardando");
            } else {
                toastr.error("Falta agregar piezas", "Mensaje Servidor");
            }
            */


        }


    });


});




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
            LimpiarFormValidator();
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

            $("#cboListaProblemas").val(arrayProblemas);
            $("#cboListaProblemas").change();
            LimpiarFormValidator();
        }
    });
    return false;
}

function LoadClasificacionPiezas() {

    $.ajax({
        type: "POST",
        url: basePath + "MICategoriaPieza/ListarCategoriaPiezaActiveJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboClasificacionPiezas").append('<option value="' + value.CodCategoriaPieza + '"  >' + value.Nombre + '</option>');
            });
            $("#cboClasificacionPiezas").select2({
                placeholder: "--Seleccione--", dropdownParent: $("#form_pieza_agregar")

            });
            $("#cboClasificacionPiezas").val(null).trigger("change");
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

function LoadListaPiezas(cod) {

    $.ajax({
        type: "POST",
        url: basePath + "MIPieza/ListarPiezaxCategoriaJson",
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
                $("#cboListaPiezas").append('<option value="' + value.CodPieza + '"  >' + value.Nombre + '</option>');
            });
            $("#cboListaPiezas").select2({
                placeholder: "--Seleccione--", dropdownParent: $("#form_pieza_agregar")

            });
            $("#cboListaPiezas").val(null).trigger("change");
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


function LoadClasificacionRepuestos() {

    $.ajax({
        type: "POST",
        url: basePath + "MICategoriaRepuesto/ListarCategoriaRepuestoActiveJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboClasificacionRepuestos").append('<option value="' + value.CodCategoriaRepuesto + '"  >' + value.Nombre + '</option>');
            });
            $("#cboClasificacionRepuestos").select2({
                placeholder: "--Seleccione--",

            });
            $("#cboClasificacionRepuestos").val(null).trigger("change");
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

function LoadListaRepuestos(cod) {

    $.ajax({
        type: "POST",
        url: basePath + "MIRepuesto/ListarRepuestoxCategoriaJson",
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
                $("#cboListaRepuestos").append('<option value="' + value.CodRepuesto + '"  >' + value.Nombre + '</option>');
            });
            $("#cboListaRepuestos").select2({
                placeholder: "--Seleccione--",

            });
            $("#cboListaRepuestos").val(null).trigger("change");
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

                if ($.trim(value.CodMaquinaLey)!="") {
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


function GetMaquinaDetalle(codMaquina) {

    $.ajax({
        type: "POST",
        url: basePath + "MIMaquinaInoperativa/ListarMaquinaDetalleAdministrativo",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codMaquina }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var maquina = result.data;


            $("#lin").text(maquina.NombreLinea);
            $("#numse").text(maquina.NroSerie);
            $("#jue").text(maquina.NombreJuego);
            $("#sal").text(maquina.NombreSala);
            $("#mod").text(maquina.NombreModeloMaquina);
            $("#pro").text(maquina.DescripcionContrato);
            $("#fic").text(maquina.NombreFicha);
            $("#mar").text(maquina.NombreMarcaMaquina);
            $("#tok").text(maquina.Token);

            /*
            $("#Linea").val(maquina.NombreLinea);
            $("#NroSerie").val(maquina.NroSerie);
            $("#Juego").val(maquina.NombreJuego);
            $("#Sala").val(maquina.NombreSala);
            $("#Modelo").val(maquina.NombreModeloMaquina);
            $("#Propietario").val(maquina.DescripcionContrato);
            $("#Ficha").val(maquina.NombreFicha);
            $("#Marca").val(maquina.NombreMarcaMaquina);
            $("#Token").val(maquina.Token);
            */

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

function DataTablePiezaUpdate(array) {

    objetodatatablePieza = $("#tablePieza").DataTable({
        "bDestroy": true,
        "ordering": false,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": false,
        "aaSorting": [],
        "autoWidth": false,
        "bAutoWidth": true,
        "bProcessing": true,
        "bDeferRender": true,
        "searching": false,
        "bInfo": false, //Dont display info e.g. "Showing 1 to 4 of 4 entries"
        "paging": false,//Dont want paging                
        "bPaginate": false,//Dont want paging      
        data: array
        ,
        columns: [
            { data: "CodPieza", title: "Cod" },
            { data: "Pieza", title: "Pieza" },
            { data: "Cantidad", title: "Cantidad" },
            {
                data: "CodPieza", title: "Accion",
                "bSortable": false,
                "render": function (o, type, oData) {
                    return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
                }
            }
        ],
        "drawCallback": function (settings) {
            $('.btnEditar').tooltip({
                title: "Editar"
            });
            $('.btnEliminar').tooltip({
                title: "Eliminar"
            });
        },

        "initComplete": function (settings, json) {



        },
    });
}


function DataTableRepuestoUpdate(arrayRepuesto) {

    objetodatatablePieza = $("#tableRepuesto").DataTable({
        "bDestroy": true,
        "ordering": false,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": false,
        "aaSorting": [],
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "searching": false,
        "bInfo": false, //Dont display info e.g. "Showing 1 to 4 of 4 entries"
        "paging": false,//Dont want paging                
        "bPaginate": false,//Dont want paging      
        data: arrayRepuesto
        ,
        columns: [
            { data: "Almacen", title: "Almacen" },
            { data: "Repuesto", title: "Repuesto" },
            { data: "Cantidad", title: "Cantidad" },
            /*{
                data: "CodRepuesto", title: "Accion",
                "bSortable": false,
                "render": function (o, type, oData) {
                    return `<button type="button" class="btn btn-xs btn-warning btnEditarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnEliminarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
                }
            }*/
            {
                data: "CodRepuesto", title: "Accion",
                "bSortable": false,
                "render": function (o, type, oData) {

                    return `<button type="button" class="btn btn-xs btn-danger btnEliminarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
                }
            }
        ],
        "drawCallback": function (settings) {
            $('.btnEditar').tooltip({
                title: "Editar"
            });
            $('.btnEliminar').tooltip({
                title: "Eliminar"
            });
        },

        "initComplete": function (settings, json) {



        },
    });
}

function DataTableRepuestosxAlmacen(cod,cantidad) {

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "MIMaquinaInoperativa/BuscarInventarioRepuestoAlmacenes",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ cod, cantidad }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {

                respuesta = response.data;

                objetodatatable1 = $("#tableRepuestoAlmacenPropio").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "ordering": true,
                    "scrollCollapse": true,
                    "scrollX": true,
                    "paging": true,
                    "aaSorting": [],
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response.data,
                    columns: [
                        { data: "NombreSala", title: "Sala" },
                        { data: "NombreAlmacen", title: "Almacen" },
                        { data: "NombrePiezaRepuesto", title: "Repuesto" },
                        { data: "Cantidad", title: "Cantidad" },
                        /*
                        {
                            data: "Estado", title: "Estado",
                            "render": function (o) {
                                var estado = "INACTIVO";
                                var css = "btn-danger";
                                if (o == 1) {
                                    estado = "ACTIVO"
                                    css = "btn-success";
                                }
                                return '<span class="label ' + css + '">' + estado + '</span>';

                            }
                        },*/
                        {
                            data: "CodPiezaRepuestoAlmacen", title: "Accion",
                            "bSortable": false,
                            "render": function (o, type, oData) {
                                return `<button type="button" class="btn btn-xs btn-success btnDescontarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-ok"></i></button>`;
                            }
                        }
                    ],

                    "drawCallback": function (settings) {
                        $('.btnDescontarRepuesto').tooltip({
                            title: "Descontar Repuesto"
                        });
                    },
                    "initComplete": function (settings, json) {



                    },
                });

                objetodatatable2 = $("#tableRepuestoAlmacenAyuda").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "ordering": true,
                    "scrollCollapse": true,
                    "scrollX": true,
                    "paging": true,
                    "aaSorting": [],
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response.data2,
                    columns: [
                        { data: "NombreSala", title: "Sala" },
                        { data: "NombreAlmacen", title: "Almacen" },
                        { data: "NombrePiezaRepuesto", title: "Repuesto" },
                        { data: "Cantidad", title: "Cantidad" },
                        {
                            data: "CodPiezaRepuestoAlmacen", title: "Accion",
                            "bSortable": false,
                            "render": function (o, type, oData) {
                                return `<button type="button" class="btn btn-xs btn-info btnPedirRepuesto" data-id="${oData.CodAlmacen}" data-salaid="${oData.CodSala}"  data-repuestoid="${oData.CodPiezaRepuesto}"  data-piezarepuestoalmacenid="${o}"><i class="glyphicon glyphicon-hand-right"></i></button>`;
                            }
                        }
                    ],

                    "drawCallback": function (settings) {
                        $('.btnPedirRepuesto').tooltip({
                            title: "Pedir Repuesto"
                        });
                    },
                    "initComplete": function (settings, json) {



                    },
                });

            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
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
            LimpiarFormValidator();
        }
    });
    return false;

}


function LoadAlmacenDestino() {
    $('#cboAlmacenDestino').empty();

    $.ajax({
        type: "POST",
        url: basePath + "MIAlmacen/GetAllAlmacenUsuarioActual",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboAlmacenDestino").append('<option value="' + value.CodAlmacen + '"  >' + value.Nombre + '</option>');
            });
            $("#cboAlmacenDestino").select2({
                placeholder: "--Seleccione--", dropdownParent: $("#form_almacen_destino")

            });
            $("#cboAlmacenDestino").val(null).trigger("change");
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


function LimpiarFormValidator() {
    $("#frmRegistroMaquinaInoperativa").parent().find('div').removeClass("has-error");
    $("#frmRegistroMaquinaInoperativa").parent().find('i').removeAttr("style").hide();
    $("#frmRegistroMaquinaInoperativa").parent().find('.fa').removeAttr("style").show();
}

function EnviarRepuesto() {


    //let CodSala = $("#almacendestino_sala").val();
    let CodRepuesto = $("#almacendestino_repuesto").val();
    let CodPiezaRepuestoAlmacen = $("#almacendestino_pieza_repuesto_almacen").val();
    let CodAlmacenOrigen = $("#almacendestino_almacen_origen").val();
    let Cantidad = $("#almacendestino_cantidad").val();
    let CodAlmacenDestino = $("#cboAlmacenDestino").val();




    $.ajax({
        type: "POST",
        url: basePath + "MITraspasoRepuestoAlmacen/TraspasoRepuestoAlmacenGuardarJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodRepuesto, CodAlmacenOrigen, Cantidad, CodAlmacenDestino, CodPiezaRepuestoAlmacen }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {

            if (result.respuesta) {


                toastr.success(result.mensaje, "Mensaje Servidor");
                console.log("Enviado");
                //window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativa");
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

function enviarCorreoProcesoMaquinaInoperativa(CodSala) {



    $.ajax({
        type: "POST",
        url: basePath + "MIMaquinaInoperativa/EnviarCorreoMaquinaInoperativa",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodSala, CodTipo:1 }),
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

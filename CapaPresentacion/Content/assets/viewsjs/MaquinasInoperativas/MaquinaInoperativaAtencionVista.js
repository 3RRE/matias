



let objetodatatablePieza;
var array = [];
var arrayProblema = [];
var arrayRepuesto = [];
var arrayRepuestoAnt = [];
var arrayProblemas = [];
var arrayRepuestosPedidos = [];
var codRepuestoGlobal;
var cantidadGlobal;


$(document).ready(function () {


    VistaAuditoria("MIMaquinaInoperativa/MaquinaInoperativaAtencionVista", "VISTA", 0, "", 3);

    $(".divRepuesto").attr('style', 'display:none');

    if (maquinaInoperativa.Estado == 4) {
        $("#textAtender").text("Atender de todas formas");
    }

    if (maquinaInoperativa.Estado == 2) {
        $("#textAtender").text("Atender deshabilitado");
        $(".btnAtender").prop('disabled', true); 
        $(".btnTest").attr('style', 'display:none');
    }

    console.log("Estado:" + maquinaInoperativa.Estado);


    GetSalas();
    LoadClasificacionProblemas();
    LoadClasificacionPiezas();
    LoadClasificacionRepuestos();
    DataTablePiezaUpdate(array);
    DataTableProblemaUpdate(arrayProblema);
    DataTableRepuestoUpdate(arrayRepuesto);

    let _fecha = new Date();


    let _fechaEnvio = moment(maquinaInoperativa.FechaEnvioSala).format('DD/MM/YYYY');
    let _horaEnvio = moment(maquinaInoperativa.FechaEnvioSala).format('hh:mm:ss A');

    let _fechaInoperativa = moment(maquinaInoperativa.FechaInoperativa).format('DD/MM/YYYY');
    let _horaInoperativa = moment(maquinaInoperativa.FechaInoperativa).format('hh:mm:ss A');

    $(document).on("click", ".btnListar", function () {

        window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativaCreado");

    });

    $("#fechaRecepcion").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        autoclose: true,
        formatDate: 'DD-MM-YYYY',
    });

    $("#fechaRecepcion").data("DateTimePicker").setDate(_fechaInoperativa)

    $("#horaRecepcion").datetimepicker({
        pickDate: false,
        format: 'hh:mm:ss A',
        autoclose: true,
        formatTime: 'HH:mm',
    });

    $("#horaRecepcion").data("DateTimePicker").setDate(_horaInoperativa)

    $("#fechaEnvio").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        autoclose: true,
    });

    $("#fechaEnvio").data("DateTimePicker").setDate(_fechaEnvio)

    $("#horaEnvio").datetimepicker({
        pickDate: false,
        format: 'hh:mm:ss A',
        autoclose: true,
    });

    $("#horaEnvio").data("DateTimePicker").setDate(_horaEnvio)

    //SET DATA
    $("#cboSala").val(maquinaInoperativa.CodSala);
    $("#cboSala").change();

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

    console.log(maquinaInoperativa);

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
            $(".divRepuesto").attr('style', 'display:block');
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
    $(document).on("click", ".btnTest", function () {
        $("#full-modal_repuesto_almacenTest").modal("show");
        $(".containData").hide();
        $(".containMod").show()


    })

    $(document).on("click", ".btnBuscarRepuesto", function () {
        console.log("aca toy")
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

        arrayRepuestosPedidos.forEach(function (item, index) {
            if (item.CodRepuesto == codRepuesto && item.Estado != 3) {
                repetido = true;
                return false;
            }
        });

        //if (repetido) {
        //    toastr.error("Repuesto ya agregado.", "Mensaje Servidor");
        //    return false;
        //}


        //CODIGO REPUESTO POR ALMACEN

        codRepuestoGlobal = codRepuesto;
        cantidadGlobal = cantidad;
        $(".containMod").hide()
        $(".containData").fadeIn();
        DataTableRepuestosxAlmacen(codRepuestoGlobal, cantidadGlobal);

    })
    $(document).on("click", "#btnRegresar", function () {
        $(".containMod").fadeIn()
        $(".containData").hide();

    })



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

        arrayRepuestosPedidos.forEach(function (item, index) {
            if (item.CodRepuesto == codRepuesto && item.Estado != 3) {
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

        codRepuestoGlobal = codRepuesto;
        cantidadGlobal = cantidad;

        $("#full-modal_repuesto_almacen").modal("show");

    });

    $(document).on('shown.bs.modal', '#full-modal_repuesto_almacen', function () {

        DataTableRepuestosxAlmacen(codRepuestoGlobal, cantidadGlobal);
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

        //$("#full-modal_almacen_destino").modal("hide");
        $("#full-modal_repuesto_almacenTest").modal("hide");




    });

    $(document).on("click", ".btnDescontarRepuesto", function () {

        let codRepuesto = $("#cboListaRepuestos").val();
        let codAlmacen = $(this).data("id");
        let repuesto = $("#cboListaRepuestos option:selected").text();
        let cantidad = $("#CantidadRepuesto").val();
        let estado = 0;

        var data = { CodRepuesto: codRepuesto, CodPiezaRepuestoAlmacen: codAlmacen, Repuesto: repuesto, Cantidad: cantidad, Estado: estado, CodAlmacenOrigen: 0, CodAlmacenDestino: 0, Enviar: true };

        arrayRepuestosPedidos.push(data);

        console.log(arrayRepuestosPedidos);

        if (maquinaInoperativa.Estado == 4) {
            $("#textAtender").text("Atender nuevamente");
        }

        $("#tableRepuesto").DataTable().clear().draw();
        $("#tableRepuesto").DataTable().destroy();
        DataTableRepuestoUpdate(arrayRepuestosPedidos);

        // $("#full-modal_repuesto_almacen").modal("hide");
        $("#full-modal_repuesto_almacenTest").modal("hide");


    });

    console.log(arrayRepuestosPedidos);

    $(document).on("click", ".btnEliminarRepuesto", function () {


        var id = $(this).data("id");

        arrayRepuestosPedidos = jQuery.grep(arrayRepuestosPedidos, function (value) {
            return value.CodRepuesto != id || value.Estado == 3;
        });

        if (maquinaInoperativa.Estado == 4) {

            let arrayEnviando = arrayRepuestosPedidos.find(x => x.Enviar == true);
            if (arrayEnviando) {
                $("#textAtender").text("Atender nuevamente");
            } else {
                $("#textAtender").text("Atender de todas formas");
            }
        }


        console.log(arrayRepuestosPedidos);

        $("#tableRepuesto").DataTable().clear().draw();
        $("#tableRepuesto").DataTable().destroy();
        DataTableRepuestoUpdate(arrayRepuestosPedidos);

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

    $(document).on("click", ".btnAtender", function () {


        //$("#frmRegistroMaquinaInoperativa").data('bootstrapValidator').resetForm();
        //var validar = $("#frmRegistroMaquinaInoperativa").data('bootstrapValidator').validate();

        //if (validar.isValid()) {

        if (maquinaInoperativa.Estado == 4) {

            let arrayEnviando = arrayRepuestosPedidos.find(x => x.Enviar == true);
            if (arrayEnviando) {


                AtenderNuevamente();

                console.log("Pidiendo nuevos productos");
            } else {
                AtenderDeTodasFormas();
                console.log("Atediendo de todas formas");
            }

        } else {

            console.log(arrayRepuestosPedidos.length);

            //let CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
            //let CodMaquina = $("#cboMaquina").val();
            //let CodMaquinaLey = $("#cboMaquina option:selected").text();
            //let CodSala = $("#cboSala").val();
            //let ObservacionInoperativa = $("#Observaciones").val();
            //let CodEstadoInoperativa = $("#cboEstado").val();
            //let CodPrioridad = $("#cboPrioridad").val();
            //let SoloFechaInoperativa = $("#fechaInoperativa").val();
            //let SoloHoraInoperativa = $("#horaInoperativa").val();
            //let Tecnico = $("#Tecnico").val();
            //let CodEstadoProceso = 1;
            let SoloFechaRecepcion = $("#fechaRecepcion").val();
            let SoloHoraRecepcion = $("#horaRecepcion").val();
            let SoloFechaEnvio = $("#fechaEnvio").val();
            let SoloHoraEnvio = $("#horaEnvio").val();
            let Observaciones = $("#Observaciones").val();
            let CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
            //let listaProblemas = $("#cboListaProblemas").val();
            //let listaPiezas = $.map(array, function (x) { return x.CodPieza; });
            //let listaPiezasCantidad = $.map(array, function (x) { return x.Cantidad; });
            //let listaRepuestos = $.map(arrayRepuesto, function (x) { return x.CodRepuesto; });
            //let listaRepuestosCantidad = $.map(arrayRepuesto, function (x) { return x.Cantidad; });
            //let listaRepuestosAnt = $.map(arrayRepuestoAnt, function (x) { return x.CodRepuesto; });
            //let listaRepuestosCantidadAnt = $.map(arrayRepuestoAnt, function (x) { return x.Cantidad; });




            let listaRepuestosCodRepuesto = $.map(arrayRepuestosPedidos, function (x) { return x.CodRepuesto; });
            let listaRepuestosCodPiezaRepuestoAlmacen = $.map(arrayRepuestosPedidos, function (x) { return x.CodPiezaRepuestoAlmacen; });
            let listaRepuestosCantidad = $.map(arrayRepuestosPedidos, function (x) { return x.Cantidad; });
            let listaRepuestosEstado = $.map(arrayRepuestosPedidos, function (x) { return x.Estado; });
            let listaRepuestosCodAlmacenOrigen = $.map(arrayRepuestosPedidos, function (x) { return x.CodAlmacenOrigen; });
            let listaRepuestosCodAlmacenDestino = $.map(arrayRepuestosPedidos, function (x) { return x.CodAlmacenDestino; });


            $.ajax({
                type: "POST",
                url: basePath + "MIMaquinaInoperativa/AtenderMaquinaInoperativa",
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ CodMaquinaInoperativa, SoloFechaRecepcion, SoloHoraRecepcion, SoloFechaEnvio, SoloHoraEnvio, Observaciones, listaRepuestosCodRepuesto, listaRepuestosCodPiezaRepuestoAlmacen, listaRepuestosCantidad, listaRepuestosEstado, listaRepuestosCodAlmacenOrigen, listaRepuestosCodAlmacenDestino }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (result) {


                    if (result.respuesta) {
                        toastr.success(result.mensaje, "Mensaje Servidor");
                        window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativaAtencion");
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
        }


    });



    //SET DATA


    //$("#Observaciones").val(maquinaInoperativa.ObservacionAtencion);
    $("#cboEstado").val(maquinaInoperativa.CodEstadoInoperativa);
    $("#cboEstado").change();
    $("#Tecnico").val(maquinaInoperativa.Tecnico);
    $("#fechaEnvio").val(moment(maquinaInoperativa.FechoEnvioSala).format('DD/MM/YYYY'));
    $("#horaEnvio").val(moment(maquinaInoperativa.FechoEnvioSala).format('h:mm:ss A'));


    //Problemas

    listaProblemasArray.forEach(function (item, index) {

        var data = { CodProblema: item.CodProblema, Nombre: item.NombreProblema, Descripcion: item.DescripcionProblema };
        arrayProblema.push(data);

    });

    console.log(arrayProblema);

    $("#tableProblema").DataTable().clear().draw();
    $("#tableProblema").DataTable().destroy();
    DataTableProblemaUpdate(arrayProblema);

    //Piezas

    listaPiezas.forEach(function (item, index) {

        var data = { CodPieza: item.CodPieza, Nombre: item.NombrePieza, Descripcion: item.DescripcionPieza, Cantidad: item.Cantidad };
        array.push(data);

    });

    console.log(array);

    $("#tablePieza").DataTable().clear().draw();
    $("#tablePieza").DataTable().destroy();
    DataTablePiezaUpdate(array);

    //Repuestos

    listaRepuestos.forEach(function (item, index) {

        var data = { CodRepuesto: item.CodRepuesto, Repuesto: item.NombreRepuesto, Cantidad: item.Cantidad };
        arrayRepuesto.push(data);

    });

    console.log(arrayRepuesto);


    listaRepuestos.forEach(function (item, index) {

        var data = { CodRepuesto: item.CodRepuesto, Repuesto: item.NombreRepuesto, Cantidad: item.Cantidad };
        arrayRepuestoAnt.push(data);

    });

    console.log(arrayRepuestoAnt);


    listaRepuestos.forEach(function (item, index) {

        var data = { CodRepuesto: item.CodRepuesto, CodPiezaRepuestoAlmacen: 0, Repuesto: item.NombreRepuesto, Cantidad: item.Cantidad, Estado: item.Estado, CodAlmacenOrigen: 0, CodAlmacenDestino: 0, Enviar: false };

        arrayRepuestosPedidos.push(data);

    });

    console.log(arrayRepuestoAnt);
    console.log(listaRepuestos);
    console.log(arrayRepuestosPedidos);

    $("#tableRepuesto").DataTable().clear().draw();
    $("#tableRepuesto").DataTable().destroy();
    //DataTableRepuestoUpdate(arrayRepuesto);

    /*
    var data = { CodRepuesto: 0, CodPiezaRepuestoAlmacen: 0, Repuesto: "XD", Cantidad: 0, Estado: 3, Enviar:true };

    arrayRepuestosPedidos.push(data);
    */

    DataTableRepuestoUpdate(arrayRepuestosPedidos);


    //FILTROS DATATABLE

    $('input[type="checkbox"]').click(function () {
        $('input[type="checkbox"]').not(this).prop('checked', false);
    });

    $('input[type="checkbox"]').change(function () {
        if (!$(this).prop('checked')) {
            objetodatatable1.search('').draw()
            console.log("se descarmaco")

        }
    });

    $('#filtroPropio').on('change', function () {
        if (!$(this).prop('checked')) {
            objetodatatable1.search('').draw()
            console.log("se descarmaco")
        }
        objetodatatable1.search('propio').draw()
    })

    $('#filtroOtro').on('change', function () {
        objetodatatable1.search('otro').draw()

    })
    $('#filtroTodos').on('change', function () {
        objetodatatable1.search('').draw()

    })

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
                placeholder: "--Seleccione--",

            });
            $("#cboClasificacionPiezas").val(null).trigger("change");
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
                placeholder: "--Seleccione--",

            });
            $("#cboListaPiezas").val(null).trigger("change");
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
                placeholder: "--Seleccione--", dropdownParent: $(".containMod")


            });
            $("#cboClasificacionRepuestos").val(null).trigger("change");
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
                placeholder: "--Seleccione--", dropdownParent: $(".containMod")

            });
            $("#cboListaRepuestos").val(null).trigger("change");
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

            $("#Linea").val(maquina.NombreLinea);
            $("#NroSerie").val(maquina.NroSerie);
            $("#Juego").val(maquina.NombreJuego);
            $("#Sala").val(maquina.NombreSala);
            $("#Modelo").val(maquina.NombreModeloMaquina);
            $("#Propietario").val(maquina.DescripcionContrato);
            $("#Ficha").val(maquina.NombreFicha);
            $("#Marca").val(maquina.NombreMarcaMaquina);
            $("#Token").val(maquina.Token);

            $("#lin").text(maquina.NombreLinea);
            $("#numse").text(maquina.NroSerie);
            $("#jue").text(maquina.NombreJuego);
            $("#sal").text(maquina.NombreSala);
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

function DataTablePiezaUpdate(array) {

    objetodatatablePieza = $("#tablePieza").DataTable({
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
        data: array
        ,
        columns: [
            { data: "CodPieza", title: "Cod" },
            { data: "Nombre", title: "Nombre" },
            { data: "Descripcion", title: "Descripcion" },
            { data: "Cantidad", title: "Cantidad" },
            /*
            {
                data: "CodPieza", title: "Accion",
                "bSortable": false,
                "render": function (o, type, oData) {
                    return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
                }
            }*/
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


function DataTableProblemaUpdate(array) {

    objetodatatableProblema = $("#tableProblema").DataTable({
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
        data: array
        ,
        columns: [
            { data: "CodProblema", title: "Cod" },
            { data: "Nombre", title: "Nombre" },
            { data: "Descripcion", title: "Descripcion" },
            //{ data: "Cantidad", title: "Cantidad" },
            /*
            {
                data: "CodPieza", title: "Accion",
                "bSortable": false,
                "render": function (o, type, oData) {
                    return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
                }
            }*/
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
            { data: "CodRepuesto", title: "Cod" },
            { data: "Repuesto", title: "Repuesto" },
            { data: "Cantidad", title: "Cantidad" },
            {
                data: "Estado", title: "Estado",
                "bSortable": false,
                "render": function (o, type, oData) {




                    if (o == 0) {

                        estado = "EN STOCK"
                        css = "btn-primary";
                        return '<span class="label ' + css + '">' + estado + '</span>';

                    } else

                        if (o == 2) {

                            estado = "ACEPTADO"
                            css = "btn-success";
                            return '<span class="label ' + css + '">' + estado + '</span>';

                        } else
                            if (o == 3) {

                                estado = "RECHAZADO"
                                css = "btn-danger";
                                return '<span class="label ' + css + '">' + estado + '</span>';

                            } else {

                                estado = "PEDIDO"
                                css = "btn-warning";
                                return '<span class="label ' + css + '">' + estado + '</span>';

                            }

                }
            },
            {
                data: "CodRepuesto", title: "Acciones",
                "bSortable": false,
                "render": function (o, type, oData) {

                    if (oData.Enviar) {

                        return `<button type="button" class="btn btn-xs btn-danger btnEliminarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;

                    } else {
                        return ``
                    }
                    //else {

                    //    return `<button type="button" class="btn btn-xs btn-danger btnEliminarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;


                    //}

                    /*
                    if (maquinaInoperativa.Estado == 4 || maquinaInoperativa.Estado == 2) {
                        if (oData.Enviar) {

                            return `<button type="button" class="btn btn-xs btn-danger btnEliminarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;

                        } else {

                            return ``;

                        }
                    } else {

                        return `<button type="button" class="btn btn-xs btn-danger btnEliminarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;

                    }
                    */





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


function EnviarRepuesto() {


    //let CodSala = $("#almacendestino_sala").val();
    let NombreRepuesto = $("#cboListaRepuestos option:selected").text();
    let CodRepuesto = $("#almacendestino_repuesto").val();
    let CodPiezaRepuestoAlmacen = $("#almacendestino_pieza_repuesto_almacen").val();
    let CodAlmacenOrigen = $("#almacendestino_almacen_origen").val();
    let Cantidad = $("#almacendestino_cantidad").val();
    let CodAlmacenDestino = $("#cboAlmacenDestino").val();
    let Estado = 1;


    var data = { CodRepuesto: CodRepuesto, CodPiezaRepuestoAlmacen: CodPiezaRepuestoAlmacen, Repuesto: NombreRepuesto, Cantidad: Cantidad, Estado: Estado, CodAlmacenOrigen: CodAlmacenOrigen, CodAlmacenDestino: CodAlmacenDestino, Enviar: true };

    arrayRepuestosPedidos.push(data);

    if (maquinaInoperativa.Estado == 4) {

        $("#textAtender").text("Atender nuevamente");
    }

    console.log(arrayRepuestosPedidos);

    $("#tableRepuesto").DataTable().clear().draw();
    $("#tableRepuesto").DataTable().destroy();
    DataTableRepuestoUpdate(arrayRepuestosPedidos);

    $("#full-modal_repuesto_almacen").modal("hide");
    $("#full-modal_almacen_destino").modal("hide");
    /*
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


                var data = { CodRepuesto: CodRepuesto, CodAlmacen: CodPiezaRepuestoAlmacen, Repuesto: NombreRepuesto, Cantidad: Cantidad, Estado:Estado };

                arrayRepuesto.push(data);

                console.log(arrayRepuesto);

                $("#tableRepuesto").DataTable().clear().draw();
                $("#tableRepuesto").DataTable().destroy();
                DataTableRepuestoUpdate(arrayRepuesto);


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
    });*/
}

function LimpiarFormValidator() {
    $("#frmRegistroMaquinaInoperativa").parent().find('div').removeClass("has-error");
    $("#frmRegistroMaquinaInoperativa").parent().find('i').removeAttr("style").hide();
    $("#frmRegistroMaquinaInoperativa").parent().find('.fa').removeAttr("style").show();
}



function DataTableRepuestosxAlmacen(cod, cantidad) {




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
                //tipo 1 = propio
                //tipo 2 = otro
                //arrayAlmacenPropio = response.data
                const arrayAlmacenPropio = response.data.map(propio => {
                    return { ...propio, tipo: 'propio' }
                })
                const arrayAlmacenAyuda = response.data2.map(propio => {
                    return { ...propio, tipo: 'otro' }
                })

                const arrayAlmacenTodo = arrayAlmacenPropio.concat(arrayAlmacenAyuda)
                if (arrayAlmacenTodo.length == 0) {
                    $("#nombreRepuesto").text("Sin resultados")
                }
                else {
                    $("#nombreRepuesto").text(arrayAlmacenTodo[0].NombrePiezaRepuesto)
                }
                console.log("aca estaaaa")
                console.log(arrayAlmacenTodo)

                respuesta = response.data;


                objetodatatable1 = $("#tableRepuestoAlmacenPropio").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "dom": 'ltip',
                    "ordering": true,
                    "scrollCollapse": true,
                    "scrollX": true,
                    "paging": true,
                    "aaSorting": [],
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,

                    data: arrayAlmacenTodo,
                    columns: [
                        { data: "NombreSala", title: "Sala" },
                        { data: "NombreAlmacen", title: "Almacen" },
                        /*  { data: "NombrePiezaRepuesto", title: "Repuesto" },*/
                        { data: "Cantidad", title: "Cantidad" },
                        {
                            data: "Tipo", title: "Tipo Almacen",
                            "render": function (o, type, oData) {
                                if (oData.tipo == 'propio') {
                                    return `${oData.tipo}`;
                                }
                                else if (oData.tipo == 'otro') {
                                    return `${oData.tipo}`;
                                }
                            }
                        },
                        {
                            data: "CodPiezaRepuestoAlmacen", title: "Accion",
                            "bSortable": false,
                            "render": function (o, type, oData) {
                                if (oData.tipo == 'propio') {
                                    return `<button type="button" class="btn btn-xs btn-success btnDescontarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-ok"></i></button>`;
                                }
                                else if (oData.tipo == 'otro') {
                                    return `<button type="button" class="btn btn-xs btn-info btnPedirRepuesto" data-id="${oData.CodAlmacen}" data-salaid="${oData.CodSala}"  data-repuestoid="${oData.CodPiezaRepuesto}"  data-piezarepuestoalmacenid="${o}"><i class="glyphicon glyphicon-hand-right"></i></button>`;
                                }
                            }
                        }
                    ],

                    "drawCallback": function (settings) {
                        $('.btnDescontarRepuesto').tooltip({
                            title: "Descontar Repuesto"
                        });
                        $('.btnPedirRepuesto').tooltip({
                            title: "Pedir Repuesto"
                        });
                    },
                    "initComplete": function (settings, json) {
                    },
                });

                objetodatatable1.search('propio').draw();
                //$('#filtroPropio').val($(this).is(':checked'));
                $('#filtroPropio').prop('checked', true);
                $('#filtroOtro').prop('checked', false);
                $('#filtroTodos').prop('checked', false);

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


function AtenderDeTodasFormas() {

    let CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "MITraspasoRepuestoAlmacen/AtencionPendienteEditarMaquinaInoperativa",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodMaquinaInoperativa }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {

                toastr.success(response.mensaje, "Mensaje Servidor");
                window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativaAtencion");
                $(".btnAtenderIgual").attr('style', 'display:none');

            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}




function AtenderNuevamente() {


    let CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;

    let listaRepuestosCodRepuesto = $.map(arrayRepuestosPedidos, function (x) { return x.CodRepuesto; });
    let listaRepuestosCodPiezaRepuestoAlmacen = $.map(arrayRepuestosPedidos, function (x) { return x.CodPiezaRepuestoAlmacen; });
    let listaRepuestosCantidad = $.map(arrayRepuestosPedidos, function (x) { return x.Cantidad; });
    let listaRepuestosEstado = $.map(arrayRepuestosPedidos, function (x) { return x.Estado; });
    let listaRepuestosCodAlmacenOrigen = $.map(arrayRepuestosPedidos, function (x) { return x.CodAlmacenOrigen; });
    let listaRepuestosCodAlmacenDestino = $.map(arrayRepuestosPedidos, function (x) { return x.CodAlmacenDestino; });
    let listaEnviar = $.map(arrayRepuestosPedidos, function (x) { return x.Enviar });


    $.ajax({
        type: "POST",
        url: basePath + "MIMaquinaInoperativa/AtenderMaquinaInoperativaNuevamente",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodMaquinaInoperativa, listaRepuestosCodRepuesto, listaRepuestosCodPiezaRepuestoAlmacen, listaRepuestosCantidad, listaRepuestosEstado, listaRepuestosCodAlmacenOrigen, listaRepuestosCodAlmacenDestino, listaEnviar }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {


            if (result.respuesta) {
                toastr.success(result.mensaje, "Mensaje Servidor");
                window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativaAtencion");
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

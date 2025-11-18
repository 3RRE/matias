

var estadoAlmacenes = false;
var arrayProblemas = [];
var arrayRepuestosPedidos = [];
let codAlmacenGlobal = 0;
var arrayPiezasAnt = [];
$(document).ready(function () {

    VistaAuditoria("MIMaquinaInoperativa/MaquinaInoperativaEditarVista", "VISTA", 0, "", 3);

    $(".divRepuesto").attr('style', 'display:none');

    let objetodatatablePieza;
    var array = [];
    var arrayRepuesto = [];
    var arrayRepuestoAnt = [];

    GetSalas();
    LoadClasificacionProblemas();
    LoadClasificacionPiezas();
    LoadClasificacionRepuestos();
    LoadClasificacionRepuestonuevalogica();
    DataTablePiezaUpdate(array);
    DataTableRepuestoUpdate(arrayRepuestosPedidos);
    RevisarEstadoAlmacenes();
    let _fecha = new Date();

    let _fechaInoperativa = moment(maquinaInoperativa.FechaInoperativa).format('DD/MM/YYYY hh:mm:ss A');
    //let _fechaCreado = moment(maquinaInoperativa.FechaCreado).format('DD/MM/YYYY hh:mm:ss A');

    $(document).on("click", ".btnListar", function () {

        window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativaCreado");

    });

    $("#fechaInoperativa").datetimepicker({
        pickTime: true,
        format: 'DD/MM/YYYY hh:mm:ss A',
        autoclose: true,
        formatDate: 'DD-MM-YYYY HH:mm',
    });

    $("#fechaInoperativa").data("DateTimePicker").setDate(_fechaInoperativa)


    //SET DATA

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

    $("#cboClasificacionRepuestonuevalogica").change(function () {
        let cod = $(this).val();
        if (cod) {
            $('#cboListaRepuestonuevalogica').empty();
            LoadListaRepuestonuevalogica(cod);
        }
    });

    $(document).on("click", ".btnAbrirModalPieza", function () {


        $("#cboClasificacionPiezas").val(null).trigger("change");
        $("#cboListaPiezas").val(null).trigger("change");
        $("#Cantidad").val(0);


        $("#full-modal_pieza_agregar").modal("show");



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

        //console.log(array);

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

        //console.log(array);

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

        //console.log(array);

        $("#tablePieza").DataTable().clear().draw();
        $("#tablePieza").DataTable().destroy();
        DataTablePiezaUpdate(array);

    });



    //Repuesto
    $(document).on("click", ".btnTest", function () {

        if (estadoAlmacenes) {

            $("#full-modal_repuesto_almacenTest").modal("show");
            $(".containData").hide();
            $(".containMod").show();

        } else {

            $("#full-modal_repuestonuevalogica_agregar").modal("show");

        }




    })


    $(document).on("click", ".btnAgregarRepuestonuevalogica", function () {

        let codRepuestonuevalogica = $("#cboListaRepuestonuevalogica").val();
        let Repuestonuevalogica = $("#cboListaRepuestonuevalogica option:selected").text();
        let cantidadRepuestonuevalogica = $("#CantidadRepuestonuevalogica").val();

        if (!codRepuestonuevalogica) {
            toastr.error("Seleccione un repuesto.", "Mensaje Servidor");
            return false;
        }
        if (!Repuestonuevalogica) {
            toastr.error("Seleccione un repuesto.", "Mensaje Servidor");
            return false;
        }
        if (!cantidadRepuestonuevalogica || cantidadRepuestonuevalogica < 1) {
            toastr.error("Ingrese una cantidad valida.", "Mensaje Servidor");
            return false;
        }

        let repetido = false;

        arrayRepuestosPedidos.forEach(function (item, index) {
            if (item.CodRepuesto == codRepuestonuevalogica && item.Estado != 3) {
                repetido = true;
                return false;
            }
        });

        if (repetido) {
            toastr.error("Repuesto ya agregado.", "Mensaje Servidor");
            return false;
        }


        let estado = 0;

        var data = { CodRepuesto: codRepuestonuevalogica, NombreAlmacen: ' - ', CodPiezaRepuestoAlmacen: codRepuestonuevalogica, Repuesto: Repuestonuevalogica, Cantidad: cantidadRepuestonuevalogica, Estado: estado, CodAlmacenOrigen: 0, CodAlmacenDestino: 0, Enviar: true };

        arrayRepuestosPedidos.push(data);

        if (maquinaInoperativa.Estado == 4) {
            $("#textAtender").text("Atender nuevamente");
        }

        $("#tableRepuesto").DataTable().clear().draw();
        $("#tableRepuesto").DataTable().destroy();
        DataTableRepuestoUpdate(arrayRepuestosPedidos);

        $("#full-modal_repuestonuevalogica_agregar").modal("hide");


    });

    $(document).on("click", ".btnBuscarRepuesto", function () {
        //console.log("aca toy")
        let codRepuesto = $("#cboListaRepuestos").val();
        let repuesto = $("#cboListaRepuestos option:selected").text();

        if (!codRepuesto) {
            toastr.error("Seleccione un repuesto.", "Mensaje Servidor");
            return false;
        }
        if (!repuesto) {
            toastr.error("Seleccione un repuesto.", "Mensaje Servidor");
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
        $(".containMod").hide()
        $(".containData").fadeIn();
        DataTableRepuestosxAlmacen(codRepuestoGlobal);

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

        DataTableRepuestosxAlmacen(codRepuestoGlobal);
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

    $(document).on("click", ".btnAgregarCantidad", function () {

        let cantidad = $("#CantidadRepuesto").val();
        //console.log(cantidad);
        //console.log(cantidadTotalGlobal);



        if (cantidad > cantidadTotalGlobal) {
            toastr.error("No existe esa cantidad de repuestos en el almacen", "Mensaje Servidor");
            return false;
        }
        

        let codRepuesto = $("#cboListaRepuestos").val();
        let repuesto = $("#cboListaRepuestos option:selected").text();
        let estado = 0;

        var data = { CodRepuesto: codRepuesto, NombreAlmacen: nombreAlmacenGlobal, CodPiezaRepuestoAlmacen: codAlmacenGlobal, Repuesto: repuesto, Cantidad: cantidad, Estado: estado, CodAlmacenOrigen: 0, CodAlmacenDestino: 0, Enviar: true };

        arrayRepuestosPedidos.push(data);

        //console.log(arrayRepuestosPedidos);

        if (maquinaInoperativa.Estado == 4) {
            $("#textAtender").text("Atender nuevamente");
        }

        $("#tableRepuesto").DataTable().clear().draw();
        $("#tableRepuesto").DataTable().destroy();
        DataTableRepuestoUpdate(arrayRepuestosPedidos);


        //$("#full-modal_almacen_destino").modal("hide");
        $("#full-modal_almacen_destino").modal("hide");
        $("#full-modal_repuesto_almacenTest").modal("hide");




    });

    $(document).on("click", ".btnDescontarRepuesto", function () {

        codAlmacenGlobal = $(this).data("id");
        nombreAlmacenGlobal = $(this).data("nombrealmacen");
        cantidadTotalGlobal = $(this).data("cantidad");

        //let codRepuesto = $("#cboListaRepuestos").val();
        //let repuesto = $("#cboListaRepuestos option:selected").text();
        //let cantidad = $("#CantidadRepuesto").val();
        //let estado = 0;

        //var data = { CodRepuesto: codRepuesto, CodPiezaRepuestoAlmacen: codAlmacenGlobal, Repuesto: repuesto, Cantidad: cantidad, Estado: estado, CodAlmacenOrigen: 0, CodAlmacenDestino: 0, Enviar: true };

        //arrayRepuestosPedidos.push(data);

        //console.log(arrayRepuestosPedidos);

        //if (maquinaInoperativa.Estado == 4) {
        //    $("#textAtender").text("Atender nuevamente");
        //}

        //$("#tableRepuesto").DataTable().clear().draw();
        //$("#tableRepuesto").DataTable().destroy();
        //DataTableRepuestoUpdate(arrayRepuestosPedidos);

        // $("#full-modal_repuesto_almacen").modal("hide");
        //$("#full-modal_repuesto_almacenTest").modal("hide");


        let repetido = false;
        arrayRepuestosPedidos.forEach(function (item, index) {
            if (item.CodPiezaRepuestoAlmacen == codAlmacenGlobal) {
                repetido = true;
            }
        });

        if (repetido) {
            toastr.error("Ya agrego repuestos de este almacen", "Mensaje Servidor");
            return false;
        }

        $("#full-modal_almacen_destino").modal("show");

    });


    $(document).on("click", ".btnEliminarRepuesto", function () {


        var id = $(this).data("id");

        arrayRepuestosPedidos = jQuery.grep(arrayRepuestosPedidos, function (value) {
            return value.CodPiezaRepuestoAlmacen != id;
        });


        //console.log(arrayRepuestosPedidos);

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

        //console.log(arrayRepuesto);

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
                atencion: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                TecnicoAtencion: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                ObservacionAtencion: {
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


            let CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;

            let listaProblemas = $("#cboListaProblemas").val();
            let listaPiezasAnt = $.map(arrayPiezasAnt, function (x) { return x.CodPieza; });
            let listaPiezas = $.map(array, function (x) { return x.CodPieza; });
            let listaPiezasCantidad = $.map(array, function (x) { return x.Cantidad; });
            let listaRepuestos = $.map(arrayRepuestosPedidos, function (x) { return x.CodRepuesto; });
            let listaRepuestosCantidad = $.map(arrayRepuestosPedidos, function (x) { return x.Cantidad; });
            let listaRepuestosCodPiezaRepuestoAlmacen = $.map(arrayRepuestosPedidos, function (x) { return x.CodPiezaRepuestoAlmacen; });
            let listaRepuestosAnt = $.map(arrayRepuestoAnt, function (x) { return x.CodRepuesto; });
            let listaRepuestosCantidadAnt = $.map(arrayRepuestoAnt, function (x) { return x.Cantidad; });

            let TipoAtencion = $("#cboAtencion").val();
            let TecnicoAtencion = $("#TecnicoAtencion").val();
            let ObservacionAtencion = $("#ObservacionAtencion").val();


            let listaRepuestosCodRepuesto = $.map(arrayRepuestosPedidos, function (x) { return x.CodRepuesto; });
            //let listaRepuestosCantidad = $.map(arrayRepuestosPedidos, function (x) { return x.Cantidad; });
            let listaRepuestosEstado = $.map(arrayRepuestosPedidos, function (x) { return x.Estado; });
            let listaRepuestosCodAlmacenOrigen = $.map(arrayRepuestosPedidos, function (x) { return x.CodAlmacenOrigen; });
            let listaRepuestosCodAlmacenDestino = $.map(arrayRepuestosPedidos, function (x) { return x.CodAlmacenDestino; });

            let CodSala = $("#cboSala").val();

            $.ajax({
                type: "POST",
                url: basePath + "MIMaquinaInoperativa/MaquinaInoperativaAtenderJson",
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ CodMaquinaInoperativa, TipoAtencion, ObservacionAtencion, TecnicoAtencion, listaProblemas, listaPiezasAnt, listaPiezas, listaRepuestos, listaPiezasCantidad, listaRepuestosCantidad, listaRepuestosCodPiezaRepuestoAlmacen }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (result) {

                    if (result.respuesta) {
                        if (TipoAtencion == 2) {
                            enviarCorreoProcesoMaquinaInoperativa(CodSala);
                        }
                        toastr.success(result.mensaje, "Mensaje Servidor");
                        window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativaCreado");
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


    //$("#Observaciones").val(maquinaInoperativa.ObservacionInoperativa);
    $("#cboEstado").val(maquinaInoperativa.CodEstadoInoperativa);
    $("#cboEstado").change();
    $("#Tecnico").val(maquinaInoperativa.TecnicoCreado);
    //$("#fechaEnvio").val(moment(maquinaInoperativa.FechoEnvioSala).format('DD/MM/YYYY'));
    //$("#horaEnvio").val(moment(maquinaInoperativa.FechoEnvioSala).format('h:mm:ss A'));


    //Piezas

    listaPiezas.forEach(function (item, index) {

        var data = { CodPieza: item.CodPieza, Pieza: item.NombrePieza, Cantidad: item.Cantidad };
        array.push(data);
        arrayPiezasAnt.push(data);
    });

    //console.log(array);

    $("#tablePieza").DataTable().clear().draw();
    $("#tablePieza").DataTable().destroy();
    DataTablePiezaUpdate(array);

    //Repuestos

    listaRepuestos.forEach(function (item, index) {

        var data = { CodRepuesto: item.CodRepuesto, Repuesto: item.NombreRepuesto, Cantidad: item.Cantidad };
        arrayRepuesto.push(data);

    });

    //console.log(arrayRepuesto);


    listaRepuestos.forEach(function (item, index) {

        var data = { CodRepuesto: item.CodRepuesto, Repuesto: item.NombreRepuesto, Cantidad: item.Cantidad };
        arrayRepuestoAnt.push(data);

    });

    //console.log(arrayRepuestoAnt);


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
                placeholder: "--Seleccione--", dropdownParent: $("#form_pieza_agregar")

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
                placeholder: "--Seleccione--", dropdownParent: $("#form_pieza_agregar")

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
            $("#lin").text(maquina.NombreLinea);
            $("#numse").text(maquina.NroSerie);
            $("#jue").text(maquina.NombreJuego);
            $("#sal").text(maquina.NombreSala);
            $("#mod").text(maquina.NombreModeloMaquina);
            $("#pro").text(maquina.DescripcionContrato);
            $("#fic").text(maquina.NombreFicha);
            $("#mar").text(maquina.NombreMarcaMaquina);
            $("#tok").text(maquina.Token);

            $("#Linea").val(maquina.NombreLinea);
            $("#NroSerie").val(maquina.NroSerie);
            $("#Juego").val(maquina.NombreJuego);
            $("#Sala").val(maquina.NombreSala);
            $("#Modelo").val(maquina.NombreModeloMaquina);
            $("#Propietario").val(maquina.DescripcionContrato);
            $("#Ficha").val(maquina.NombreFicha);
            $("#Marca").val(maquina.NombreMarcaMaquina);
            $("#Token").val(maquina.Token);


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
        data: arrayRepuesto,
        columns: [
            { data: "CodRepuesto", title: "Cod" },
            { data: "NombreAlmacen", title: "Almacen" },
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
                data: "CodPiezaRepuestoAlmacen", title: "Acciones",
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


function DataTableRepuestosxAlmacen(cod) {




    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "MIMaquinaInoperativa/BuscarInventarioRepuestoAlmacenesPropios",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ cod }),
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

                const arrayAlmacenTodo = arrayAlmacenPropio
                if (arrayAlmacenTodo.length == 0) {
                    $("#nombreRepuesto").text("Sin resultados")
                }
                else {
                    $("#nombreRepuesto").text(arrayAlmacenTodo[0].NombrePiezaRepuesto)
                }

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
                        {
                            data: "Cantidad", title: "Cantidad",
                            "render": function (o, type, oData) {

                                let cantidadActual = oData.Cantidad;

                                arrayRepuestosPedidos.forEach(function (item, index) {
                                    if (item.CodPiezaRepuestoAlmacen == oData.CodPiezaRepuestoAlmacen) {
                                        cantidadActual = cantidadActual - item.Cantidad;
                                    }
                                });

                                return cantidadActual;
                            }
                        },
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

                                let cantidadActual = oData.Cantidad;

                                arrayRepuestosPedidos.forEach(function (item, index) {
                                    if (item.CodPiezaRepuestoAlmacen == o) {
                                        cantidadActual = cantidadActual - item.Cantidad;
                                    }
                                });


                                if (oData.tipo == 'propio') {
                                    return `<button type="button" class="btn btn-xs btn-success btnDescontarRepuesto" data-id="${o}" data-nombrealmacen="${oData.NombreAlmacen}" data-cantidad="${cantidadActual}"><i class="glyphicon glyphicon-ok"></i></button>`;
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


            }

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
        data: JSON.stringify({ CodSala, CodTipo: 3 }),
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


function RevisarEstadoAlmacenes() {
    $.ajax({
        type: "POST",
        url: basePath + "MIMaquinaInoperativa/RevisarEstadoAlmacenes",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {

            estadoAlmacenes = result.data;

        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}


function LoadClasificacionRepuestonuevalogica() {

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
                $("#cboClasificacionRepuestonuevalogica").append('<option value="' + value.CodCategoriaRepuesto + '"  >' + value.Nombre + '</option>');
            });
            $("#cboClasificacionRepuestonuevalogica").select2({
                placeholder: "--Seleccione--", dropdownParent: $("#form_repuestonuevalogica_agregar")

            });
            $("#cboClasificacionRepuestonuevalogica").val(null).trigger("change");
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

function LoadListaRepuestonuevalogica(cod) {

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
                $("#cboListaRepuestonuevalogica").append('<option value="' + value.CodRepuesto + '"  >' + value.Nombre + '</option>');
            });
            $("#cboListaRepuestonuevalogica").select2({
                placeholder: "--Seleccione--", dropdownParent: $("#form_repuestonuevalogica_agregar")

            });
            $("#cboListaRepuestonuevalogica").val(null).trigger("change");
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


var arrayProblemas = [];

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
    DataTablePiezaUpdate(array);
    DataTableRepuestoUpdate(arrayRepuesto);

    let _fecha = new Date();


    let _fechaEnvio = moment(maquinaInoperativa.FechaEnvioSala).format('DD/MM/YYYY');
    let _horaEnvio = moment(maquinaInoperativa.FechaEnvioSala).format('hh:mm:ss A');


    let _fechaInoperativa = moment(maquinaInoperativa.FechaInoperativa).format('DD/MM/YYYY');
    let _horaInoperativa = moment(maquinaInoperativa.FechaInoperativa).format('hh:mm:ss A');
    
    $(document).on("click", ".btnListar", function () {

        window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativaAtencion");

    });

    $("#fechaInoperativa").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        autoclose: true,
        formatDate: 'DD-MM-YYYY',
    });

    $("#fechaInoperativa").data("DateTimePicker").setDate(_fechaInoperativa)

    $("#horaInoperativa").datetimepicker({
        pickDate: false,
        format: 'hh:mm:ss A',
        autoclose: true,
        formatTime: 'HH:mm',
    });

    $("#horaInoperativa").data("DateTimePicker").setDate(_horaInoperativa)

    $("#fechaEnvio").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        autoclose: true,
        formatDate: 'DD-MM-YYYY',
    });

    $("#fechaEnvio").data("DateTimePicker").setDate(_fechaEnvio)

    $("#horaEnvio").datetimepicker({
        pickDate: false,
        format: 'hh:mm:ss A',
        autoclose: true,
        formatTime: 'HH:mm',
    });

    $("#horaEnvio").data("DateTimePicker").setDate(_horaEnvio)

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

        var data = { CodRepuesto: codRepuesto, Repuesto: repuesto, Cantidad: cantidad };

        arrayRepuesto.push(data);

        console.log(arrayRepuesto);

        $("#tableRepuesto").DataTable().clear().draw();
        $("#tableRepuesto").DataTable().destroy();
        DataTableRepuestoUpdate(arrayRepuesto);

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


            let CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
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
            let listaRepuestosAnt = $.map(arrayRepuestoAnt, function (x) { return x.CodRepuesto; });
            let listaRepuestosCantidadAnt = $.map(arrayRepuestoAnt, function (x) { return x.Cantidad; });


            $.ajax({
                type: "POST",
                url: basePath + "MIMaquinaInoperativa/InventarioEditarDescontarRepuestos",
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ listaRepuestos, listaRepuestosCantidad, listaRepuestosAnt, listaRepuestosCantidadAnt }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (result) {

                    if (result.respuesta) {

                        toastr.success(result.mensaje, "Mensaje Servidor");

                        if (result.errorInvetario) {
                            $.confirm({
                                icon: 'fa fa-spinner fa-spin',
                                title: result.mensaje,
                                theme: 'black',
                                animationBounce: 1.5,
                                columnClass: 'col-md-6 col-md-offset-3',
                                confirmButtonClass: 'btn-info',
                                cancelButtonClass: 'btn-warning',
                                confirmButton: 'OK',
                                content: false,
                                confirm: function () {

                                },
                                cancel: function () {
                                }
                            });
                        } else {

                            $.ajax({
                                type: "POST",
                                url: basePath + "MIMaquinaInoperativa/MaquinaInoperativaEditarJson",
                                cache: false,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: JSON.stringify({ CodMaquinaInoperativa, CodMaquina, CodMaquinaLey, CodSala, ObservacionInoperativa, CodEstadoInoperativa, CodPrioridad, SoloFechaInoperativa, SoloHoraInoperativa, SoloFechaEnvio, SoloHoraEnvio, Tecnico, CodEstadoProceso, listaProblemas, listaPiezas, listaRepuestos, listaPiezasCantidad, listaRepuestosCantidad }),
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

                let CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
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
                let listaRepuestosAnt = $.map(arrayRepuestoAnt, function (x) { return x.CodRepuesto; });
                let listaRepuestosCantidadAnt = $.map(arrayRepuestoAnt, function (x) { return x.Cantidad; });


                $.ajax({
                    type: "POST",
                    url: basePath + "MIMaquinaInoperativa/InventarioEditarDescontarRepuestos",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ listaRepuestos, listaRepuestosCantidad, listaRepuestosAnt, listaRepuestosCantidadAnt }),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (result) {

                        if (result.respuesta) {

                            toastr.success(result.mensaje, "Mensaje Servidor");

                            if (result.errorInvetario) {
                                $.confirm({
                                    icon: 'fa fa-spinner fa-spin',
                                    title: result.mensaje,
                                    theme: 'black',
                                    animationBounce: 1.5,
                                    columnClass: 'col-md-6 col-md-offset-3',
                                    confirmButtonClass: 'btn-info',
                                    cancelButtonClass: 'btn-warning',
                                    confirmButton: 'OK',
                                    content: false,
                                    confirm: function () {

                                    },
                                    cancel: function () {
                                    }
                                });
                            } else {

                                $.ajax({
                                    type: "POST",
                                    url: basePath + "MIMaquinaInoperativa/MaquinaInoperativaEditarJson",
                                    cache: false,
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    data: JSON.stringify({ CodMaquinaInoperativa, CodMaquina, CodMaquinaLey, CodSala, ObservacionInoperativa, CodEstadoInoperativa, CodPrioridad, SoloFechaInoperativa, SoloHoraInoperativa, SoloFechaEnvio, SoloHoraEnvio, Tecnico, CodEstadoProceso, listaProblemas, listaPiezas, listaRepuestos, listaPiezasCantidad, listaRepuestosCantidad }),
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
            }*/
        }


    });



    //SET DATA


    $("#Observaciones").val(maquinaInoperativa.ObservacionInoperativa);
    $("#cboEstado").val(maquinaInoperativa.CodEstadoInoperativa);
    $("#cboEstado").change();
    $("#Tecnico").val(maquinaInoperativa.Tecnico);
    //$("#fechaEnvio").val(moment(maquinaInoperativa.FechoEnvioSala).format('DD/MM/YYYY'));
    //$("#horaEnvio").val(moment(maquinaInoperativa.FechoEnvioSala).format('h:mm:ss A'));


    //Piezas

    listaPiezas.forEach(function (item, index) {

        var data = { CodPieza: item.CodPieza, Pieza: item.NombrePieza, Cantidad: item.Cantidad };
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


    $("#tableRepuesto").DataTable().clear().draw();
    $("#tableRepuesto").DataTable().destroy();
    DataTableRepuestoUpdate(arrayRepuesto);

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
                placeholder: "--Seleccione--",

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
                placeholder: "--Seleccione--",

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
        data: arrayRepuesto
        ,
        columns: [
            { data: "CodRepuesto", title: "Cod" },
            { data: "Repuesto", title: "Repuesto" },
            { data: "Cantidad", title: "Cantidad" },
            {
                data: "CodRepuesto", title: "Accion",
                "bSortable": false,
                "render": function (o, type, oData) {
                    return `<button type="button" class="btn btn-xs btn-warning btnEditarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnEliminarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
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
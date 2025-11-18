const LIMITE_CREDITOS = 5
const LIMITE_MINUTOS = 300//5 horas
let urlsPrueba = ['http://192.168.0.100/', 'http://192.168.0.113']
let consultaPorVpn = false
let ipPublica
let ipPrivada
let ipPublicaAlterna
let seEncontroAlterna = false
let urlsResultado = []
let delay = 60000
let timerId = ''
var MarcaId = 0
var MarcaNombre = ""

$(document).ready(function () {


    $("#cboEstadoModelo").val("0");

    VistaAuditoria("Progresivo/ProgresivoListadoMarcaModelo", "VISTA", 0, "", 3);

    ipPublicaG = "";
    id_progresivo = "";
    ipProgresivo = ''
    $("#cboSala").select2();
    $("#cboProgresivo").select2();
    clearTimeout(timerId)
    timerId = setTimeout(function request() {
        getPingSalas().then(result => {
            urlsResultado = result
        })
        timerId = setTimeout(request, delay);
    }, delay)


    obtenerListaSalas().then(result => {
        if (result.data) {
            renderSelectSalas(result.data)
            getPingSalas().then(response => {
                urlsResultado = response
            })
        }
    })

    $(document).on('change', '#cboSala', function (e) {
        $("#cboProgresivo").html("");
        var ipPublica = $(this).val();
        ipPublicaG = ipPublica;
        ipPrivada = $("#cboSala option:selected").data('ipprivada')
        let puertoServicio = $("#cboSala option:selected").data('puertoservicio')

        ipPrivada = ipPrivada + ':' + puertoServicio
        let uri = getUrl(ipPublica)
        const obj = urlsResultado.find(item => item.uri == ipPublica)
        if (uri && obj.respuesta) {
            let urlPublica = ipPublica + '/servicio/listadoprogresivos'
            consultaPorVpn = false
            llenarSelectAPIProgresivo__(urlPublica, {}, "cboProgresivo", "WEB_PrgID", "WEB_Nombre");
        }
        else {
            consultaPorVpn = true
            // ipPublicaAlterna=urlsResultado.find(x=>x.respuesta)
            //ipPublicaAlterna=getUrl("http://localhost:9895")
            ipPublicaAlterna = getUrl("http://190.187.44.222:9895")
            let urlPrivada = ipPrivada + '/servicio/listadoprogresivos'
            let urlPublica = ipPublicaAlterna + '/servicio/listadoprogresivosVpn'
            getProgresivosVpn(urlPrivada, urlPublica).then(response => {
                if (response.length > 0) {
                    renderSelectProgresivos(response)
                }
            })

        }
    });

    $(document).on('change', '#cboProgresivo', function () {
        id_progresivo = $(this).find(':selected').attr('data-id');
        LimpiarMarcas();
    });

    $("#btnBuscar").click(function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione una Sala");
            return false;
        }
        if ($("#cboProgresivo").val() == "") {
            toastr.error("Seleccione un Progresivo");
            return false;
        }

        var Progresivo = $("#cboProgresivo").val();
        BuscarMarcas(Progresivo);

    });

    //MARCAS

    $("#btnNuevoMarca").click(function () {

        if ($("#cboProgresivo").val() == "") {
            toastr.error("Seleccione un Progresivo");
            return false;
        }

        $("#textMarca").text("Nueva");
        $("#marca_id").val(0);
        $("#nombreMarca").val("");
        $("#cboEstadoMarca").val("");

        $("#full-modal_detalle_marca").modal('show');

    });

    $(document).on('click', '.btnEditarMarca', function () {

        var idmarca = $(this).data("id");
        var Progresivo = $("#cboProgresivo").val();

        ObtenerMarca(Progresivo, idmarca);

    });

    $("#form_registro_marca")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                estadoMarca: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                nombreMarca: {
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

    $('.btnGuardarMarca').on('click', function (e) {
        $("#form_registro_marca").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_marca").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#marca_id").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {

                console.log("Editar");

                let MarcaID = $("#marca_id").val();
                let Nombre = $("#nombreMarca").val();
                let Estado = $("#cboEstadoMarca").val();
                var Progresivo = $("#cboProgresivo").val();

                let dataForm = {}
                let urlConsulta = basePath
                if (consultaPorVpn) {
                    dataForm = {
                        parametros: {
                            CodMarcaMaquina: MarcaID,
                            Nombre: Nombre,
                            Estado: Estado
                        },
                        urlPublica: ipPublicaAlterna + '/servicio/EditarMarcaVpn',
                        urlPrivada: ipPrivada + '/servicio/EditarMarca?CodProgresivo=' + Progresivo
                    }
                    urlConsulta += "Progresivo/EditarMarcaVpn"
                }
                else {
                    dataForm = {
                        parametros: {
                            CodMarcaMaquina: MarcaID,
                            Nombre: Nombre,
                            Estado: Estado
                        },
                        url: ipPublicaG + '/servicio/EditarMarca?CodProgresivo=' + Progresivo
                    }
                    urlConsulta += "Progresivo/EditarMarca"
                }


                console.log(dataForm);

                ajaxhr = $.ajax({
                    type: "POST",
                    cache: false,
                    url: urlConsulta,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(dataForm),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (response) {
                        var msj = "";
                        msj = response.mensaje != null ? response.mensaje : '';
                        response = response.data;

                        console.log(response);

                        if (response == null) {
                            toastr.error(msj, "Mensaje Servidor");
                        } else {

                            var Progresivo = $("#cboProgresivo").val();
                            BuscarMarcas(Progresivo);

                            $("#full-modal_detalle_marca").modal('hide');

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



            }
            else {

                console.log("Nuevo");

                let Nombre = $("#nombreMarca").val();
                let Estado = $("#cboEstadoMarca").val();
                var Progresivo = $("#cboProgresivo").val();

                let dataForm = {}
                let urlConsulta = basePath
                if (consultaPorVpn) {
                    dataForm = {
                        parametros: {
                            Nombre: Nombre,
                            Estado: Estado
                        },
                        urlPublica: ipPublicaAlterna + '/servicio/GuardarMarcaVpn',
                        urlPrivada: ipPrivada + '/servicio/GuardarMarca?CodProgresivo=' + Progresivo
                    }
                    urlConsulta += "Progresivo/GuardarMarcaVpn"
                }
                else {
                    dataForm = {
                        parametros: {
                            Nombre: Nombre,
                            Estado: Estado
                        },
                        url: ipPublicaG + '/servicio/GuardarMarca?CodProgresivo=' + Progresivo
                    }
                    urlConsulta += "Progresivo/GuardarMarca"
                }


                console.log(dataForm);

                ajaxhr = $.ajax({
                    type: "POST",
                    cache: false,
                    url: urlConsulta,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(dataForm),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (response) {
                        var msj = "";
                        msj = response.mensaje != null ? response.mensaje : '';
                        response = response.data;

                        console.log(response);

                        if (response == null) {
                            toastr.error(msj, "Mensaje Servidor");
                        } else {

                            var Progresivo = $("#cboProgresivo").val();
                            BuscarMarcas(Progresivo);

                            $("#full-modal_detalle_marca").modal('hide');

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


            }




        }

    });

    //MODELOS

    $("#btnNuevoModelo").click(function () {

        if ($("#cboProgresivo").val() == "") {
            toastr.error("Seleccione un Progresivo");
            return false;
        }


        console.log(MarcaId);

        $("#textModelo").text("Nueva");
        $("#modelo_id").val(0);
        $("#nombreModelo").val("");
        $("#cboEstadoModelo").val("");

        $("#full-modal_detalle_modelo").modal('show');

    });

    $(document).on('click', '.btnEditarModelo', function () {

        var idmodelo = $(this).data("id");
        var Progresivo = $("#cboProgresivo").val();

        ObtenerModelo(Progresivo, idmodelo);

    });


    $("#form_registro_modelo")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                estadoModelo: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                nombreModelo: {
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

    $('.btnGuardarModelo').on('click', function (e) {
        $("#form_registro_modelo").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_modelo").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#modelo_id").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {

                console.log("Editar");

                let MarcaID = $("#marca_modelo_id").val();
                let ModeloID = $("#modelo_id").val();
                let Nombre = $("#nombreModelo").val();
                let Estado = $("#cboEstadoModelo").val();
                var Progresivo = $("#cboProgresivo").val();

                let dataForm = {}
                let urlConsulta = basePath
                if (consultaPorVpn) {
                    dataForm = {
                        parametros: {
                            CodMarcaMaquina: MarcaID,
                            CodModeloMaquina: ModeloID,
                            Nombre: Nombre,
                            Estado: Estado
                        },
                        urlPublica: ipPublicaAlterna + '/servicio/EditarModeloVpn',
                        urlPrivada: ipPrivada + '/servicio/EditarModelo?CodProgresivo=' + Progresivo
                    }
                    urlConsulta += "Progresivo/EditarModeloVpn"
                }
                else {
                    dataForm = {
                        parametros: {
                            CodMarcaMaquina: MarcaID,
                            CodModeloMaquina: ModeloID,
                            Nombre: Nombre,
                            Estado: Estado
                        },
                        url: ipPublicaG + '/servicio/EditarModelo?CodProgresivo=' + Progresivo
                    }
                    urlConsulta += "Progresivo/EditarModelo"
                }


                console.log(dataForm);

                ajaxhr = $.ajax({
                    type: "POST",
                    cache: false,
                    url: urlConsulta,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(dataForm),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (response) {
                        var msj = "";
                        msj = response.mensaje != null ? response.mensaje : '';
                        response = response.data;

                        console.log(response);

                        if (response == null) {
                            toastr.error(msj, "Mensaje Servidor");
                        } else {

                            var Progresivo = $("#cboProgresivo").val();
                            $("#marca_modelo_id").val(MarcaId);
                            BuscarModelos(Progresivo, MarcaId);

                            $("#full-modal_detalle_modelo").modal('hide');

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


            }
            else {

                console.log("Nuevo");

                let MarcaID = $("#marca_modelo_id").val();
                let Nombre = $("#nombreModelo").val();
                let Estado = $("#cboEstadoModelo").val();
                var Progresivo = $("#cboProgresivo").val();

                let dataForm = {}
                let urlConsulta = basePath
                if (consultaPorVpn) {
                    dataForm = {
                        parametros: {
                            CodMarcaMaquina: MarcaID,
                            Nombre: Nombre,
                            Estado: MarcaID
                        },
                        urlPublica: ipPublicaAlterna + '/servicio/GuardarModeloVpn',
                        urlPrivada: ipPrivada + '/servicio/GuardarModelo?CodProgresivo=' + Progresivo
                    }
                    urlConsulta += "Progresivo/GuardarModeloVpn"
                }
                else {
                    dataForm = {
                        parametros: {
                            CodMarcaMaquina: MarcaID,
                            Nombre: Nombre,
                            Estado: MarcaID
                        },
                        url: ipPublicaG + '/servicio/GuardarModelo?CodProgresivo=' + Progresivo
                    }
                    urlConsulta += "Progresivo/GuardarModelo"
                }



                //let urlConsulta = basePath
                //if (consultaPorVpn) {
                //    dataForm = {
                //        parametros: {
                //            CodMarcaMaquina: MarcaID,
                //            CodModeloMaquina: ModeloID,
                //            Nombre: Nombre,
                //            Estado: Estado
                //        },
                //        urlPublica: ipPublicaAlterna + '/servicio/EditarModeloVpn',
                //        urlPrivada: ipPrivada + '/servicio/EditarModelo?CodProgresivo=' + Progresivo
                //    }
                //    urlConsulta += "Progresivo/EditarModeloVpn"
                //}
                //else {
                //    dataForm = {
                //        parametros: {
                //            CodMarcaMaquina: MarcaID,
                //            CodModeloMaquina: ModeloID,
                //            Nombre: Nombre,
                //            Estado: Estado
                //        },
                //        url: ipPublicaG + '/servicio/EditarModelo?CodProgresivo=' + Progresivo
                //    }
                //    urlConsulta += "Progresivo/EditarModelo"
                //}

                console.log(dataForm);

                ajaxhr = $.ajax({
                    type: "POST",
                    cache: false,
                    url: urlConsulta,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(dataForm),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (response) {
                        var msj = "";
                        msj = response.mensaje != null ? response.mensaje : '';
                        response = response.data;

                        console.log(response);

                        if (response == null) {
                            toastr.error(msj, "Mensaje Servidor");
                        } else {

                            var Progresivo = $("#cboProgresivo").val();
                            $("#marca_modelo_id").val(MarcaId);
                            BuscarModelos(Progresivo, MarcaId);

                            $("#full-modal_detalle_modelo").modal('hide');

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


            }




        }

    });



    $(document).on('click', '.btnVerModelos', function (e) {
        e.preventDefault();
        MarcaId = $(this).data("id");
        MarcaNombre = $(this).data("nombre");
        $("#full-modal-modelos").modal('show');
    });

    $("#full-modal-modelos").on("shown.bs.modal", function () {

        var Progresivo = $("#cboProgresivo").val();
        $("#marcaNombreSpan").text(MarcaNombre);
        $("#marca_modelo_id").val(MarcaId);
        console.log(MarcaId);
        BuscarModelos(Progresivo, MarcaId);

    });

});

function BuscarMarcas(Progresivo) {

    let dataForm = {}
    let urlConsulta = basePath
    if (consultaPorVpn) {
        dataForm = {
            urlPublica: ipPublicaAlterna + '/servicio/getMarcasVpn',
            urlPrivada: ipPrivada + '/servicio/getMarcas?codProgresivo=' + Progresivo
        }
        urlConsulta += "Progresivo/ConsultarListaMarcaMaquinaJsonVpn"
    }
    else {
        dataForm = {
            url: ipPublicaG + "/servicio/getMarcas?codProgresivo=" + Progresivo
        }
        urlConsulta += "Progresivo/ConsultarListaMarcaMaquinaJson"
    }
    var addtabla = $('.contenedor_tabla');
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: urlConsulta,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            var msj = "";
            msj = response.mensaje != null ? response.mensaje : '';
            response = response.data;
            $(addtabla).empty();
            $(addtabla).append('<table id="table1" class="table table-condensed table-bordered table-hover"></table>');

            if (msj != "") {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            } else {
                //dataAuditoria(1, "#formfiltro", 3, "Progresivo/ConsultarListaMarcaMaquinaJson", "BOTON BUSCAR");
                objetodatatable1 = $("#table1").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "scrollCollapse": true,
                    "scrollX": true,
                    "paging": true,
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response,
                    columns: [
                        { data: "MarcaID", title: "ID" },
                        { data: "Nombre", title: "Nombre" },
                        {
                            data: "Estado", title: "Estado",
                            "render": function (i, j, value) {
                                if (value.Estado != 1) {
                                    return 'Inactivo';
                                }
                                else {
                                    return 'Activo';
                                }
                            }
                        },
                        {
                            data: null, title: "Accion",
                            render: function (value) {
                                let span = `<a class="btn btn-sm btn-warning btnVerModelos" data-id="${value.MarcaID}" data-nombre="${value.Nombre}"><i class="glyphicon glyphicon-search"></i> VER MODELOS</a>`
                                return '<button type="button" class="btn btn-sm btn-success btnEditarMarca" data-id="' + value.MarcaID + '" data-nombre="' + value.Nombre + '"><i class="glyphicon glyphicon-edit"></i> EDITAR</button> ' +
                                    //'<button type="button" class="btn btn-sm btn-danger btnEliminarMarca" data-id="' + value.MarcaID + '" data-nombre="' + value.Nombre + '"><i class="glyphicon glyphicon-trash"></i> ELIMINAR</button> ' +
                                    span
                            }
                        }

                    ],

                    "initComplete": function (settings, json) {

                        $('#btnExcelMarca').off("click").on('click', function () {

                            cabecerasnuevas = [];
                            cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
                            cabecerasnuevas.push({ nombre: "Progresivo", valor: $("#cboProgresivo option:selected").text() });

                            columna_cambio = [
                                {
                                    nombre: "estado",
                                    render: function (o) {
                                        valor = "";
                                        if (o == 0) {
                                            valor = "Inactivo";
                                        }
                                        if (o == 1) {
                                            valor = "Activo";
                                        }
                                        return valor;
                                    }
                                }
                            ]
                            var ocultar = ["Accion"];//"Accion";
                            funcionbotonesnuevo({
                                botonobjeto: this, ocultar: ocultar,
                                tablaobj: objetodatatable1,
                                cabecerasnuevas: cabecerasnuevas,
                            });
                            //VistaAuditoria("Progresivo/ProgresivoListadoMarcaExcel", "EXCEL", 0, "", 3);
                        });

                    },
                });
                $(".btnEliminarMarca").click(function () {

                    var nombremarca = $(this).data("nombre");
                    var MarcaId = $(this).data("id");

                    var js2;
                    js2 = $.confirm({
                        icon: 'fa fa-spinner fa-spin',
                        title: '¿Está seguro que desea eliminar la marca ' + nombremarca +'?' ,
                        theme: 'black',
                        animationBounce: 1.5,
                        columnClass: 'col-md-6 col-md-offset-3',
                        confirmButtonClass: 'btn-info',
                        cancelButtonClass: 'btn-warning',
                        confirmButton: "Confirmar",
                        cancelButton: 'Aún No',
                        content: "",
                        confirm: function () {
                            var Progresivo = $("#cboProgresivo").val();
                            ConsultarExistenModelos(Progresivo, MarcaId);

                        },
                        cancel: function () {

                        },

                    });

                });

                $('.btnEditarMarca').tooltip({
                    title: "Editar"
                });
                $('.btnEliminarMarca').tooltip({
                    title: 'Eliminar'
                });
                $('.btnVerModelos').tooltip({
                    title: 'Ver Modelos'
                });
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
}


function BuscarModelos(Progresivo, MarcaId) {

    let dataForm = {}
    let urlConsulta = basePath
    if (consultaPorVpn) {
        dataForm = {
            urlPublica: ipPublicaAlterna + '/servicio/ListarModelosidmarcaVpn',
            urlPrivada: ipPrivada + '/servicio/ListarModelosidmarca?codProgresivo=' + Progresivo + '&idmarca=' + MarcaId
        }
        urlConsulta += "Progresivo/ConsultarListaModeloMaquinaJsonVpn"
    }
    else {
        dataForm = {
            url: ipPublicaG + "/servicio/ListarModelosidmarca?codProgresivo=" + Progresivo + '&idmarca=' + MarcaId
        }
        urlConsulta += "Progresivo/ConsultarListaModeloMaquinaJson"
    }
    var addtabla2 = $('.contenedor_tabla_modal');
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: urlConsulta,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            var msj = "";
            msj = response.mensaje != null ? response.mensaje : '';
            response = response.data;
            $(addtabla2).empty();
            $(addtabla2).append('<table id="table2" class="table table-condensed table-bordered table-hover"></table>');

            if (msj != "") {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            } else {
                //dataAuditoria(1, "#formfiltro", 3, "Progresivo/ConsultarListaModeloMaquinaJson", "BOTON VER MODELOS");
                objetodatatable2 = $("#table2").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "scrollCollapse": true,
                    "scrollX": true,
                    "paging": true,
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response,
                    columns: [
                        { data: "ModeloID", title: "ID" },
                        { data: "Nombre", title: "Nombre" },
                        {
                            data: "Estado", title: "Estado",
                            "render": function (i, j, value) {
                                if (value.Estado != 1) {
                                    return 'Inactivo';
                                }
                                else {
                                    return 'Activo';
                                }
                            }
                        },
                        {
                            data: null, title: "Accion",
                            render: function (value) {
                                let span = ``
                                return '<button type="button" class="btn btn-sm btn-success btnEditarModelo" data-id="' + value.ModeloID + '" data-nombre="' + value.Nombre + '"><i class="glyphicon glyphicon-edit"></i> EDITAR</button> ' +
                                    //'<button type="button" class="btn btn-sm btn-danger btnEliminarModelo" data-id="' + value.ModeloID + '" data-nombre="' + value.Nombre + '"><i class="glyphicon glyphicon-trash"></i> ELIMINAR</button> ' +
                                    span
                            }
                        }

                    ],

                    "initComplete": function (settings, json) {

                        $('#btnExcelModelo').off("click").on('click', function () {

                            cabecerasnuevas = [];
                            cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
                            cabecerasnuevas.push({ nombre: "Progresivo", valor: $("#cboProgresivo option:selected").text() });
                            cabecerasnuevas.push({ nombre: "Marca", valor: $("#marcaNombreSpan").text() });

                            columna_cambio = [
                                {
                                    nombre: "estado",
                                    render: function (o) {
                                        valor = "";
                                        if (o == 0) {
                                            valor = "Inactivo";
                                        }
                                        if (o == 1) {
                                            valor = "Activo";
                                        }
                                        return valor;
                                    }
                                }
                            ]
                            var ocultar = ["Accion"];//"Accion";
                            funcionbotonesnuevo({
                                tituloarchivo: "Listado_de_Modelos_" + $("#marcaNombreSpan").text(),
                                tituloreporte: "Listado de Modelos",
                                botonobjeto: this, ocultar: ocultar,
                                tablaobj: objetodatatable2,
                                cabecerasnuevas: cabecerasnuevas,
                            });
                            //VistaAuditoria("Progresivo/ProgresivoListadoModeloExcel", "EXCEL", 0, "", 3);
                        });

                    },
                });
                $(".btnEliminarModelo").click(function () {

                    var nombremodelo = $(this).data("nombre");
                    var ModeloId = $(this).data("id");

                    var js2;
                    js2 = $.confirm({
                        icon: 'fa fa-spinner fa-spin',
                        title: '¿Está seguro que desea eliminar el modelo ' + nombremodelo + '?',
                        theme: 'black',
                        animationBounce: 1.5,
                        columnClass: 'col-md-6 col-md-offset-3',
                        confirmButtonClass: 'btn-info',
                        cancelButtonClass: 'btn-warning',
                        confirmButton: "Confirmar",
                        cancelButton: 'Aún No',
                        content: "",
                        confirm: function () {
                            var Progresivo = $("#cboProgresivo").val();
                            EliminarModelo(Progresivo, ModeloId);

                        },
                        cancel: function () {

                        },

                    });

                });

                $('.btnEditar').tooltip({
                    title: "Editar"
                });
                $('.btnEliminar').tooltip({
                    title: 'Eliminar'
                });
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
}

function ConsultarExistenModelos(Progresivo, MarcaId) {

    let dataForm = {}
    let urlConsulta = basePath
    if (consultaPorVpn) {
        dataForm = {
            urlPublica: ipPublicaAlterna + '/servicio/ListarModelosidmarcaVpn',
            urlPrivada: ipPrivada + '/servicio/ListarModelosidmarca?codProgresivo=' + Progresivo + '&idmarca=' + MarcaId
        }
        urlConsulta += "Progresivo/ConsultarListaModeloMaquinaJsonVpn"
    }
    else {
        dataForm = {
            url: ipPublicaG + "/servicio/ListarModelosidmarca?codProgresivo=" + Progresivo + '&idmarca=' + MarcaId
        }
        urlConsulta += "Progresivo/ConsultarListaModeloMaquinaJson"
    }
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: urlConsulta,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            var msj = "";
            msj = response.mensaje != null ? response.mensaje : '';
            response = response.data;

            

            console.log(response.length);

            if (response.length > 0) {
                toastr.error("No es posible eliminar una marca que tiene modelos asignados.", "Mensaje Servidor");
            } else {
                EliminarMarca(Progresivo, MarcaId);
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
}

function EliminarMarca(Progresivo,MarcaId) {
    console.log("Marca eliminada");
    //console.log(consultaPorVpn)
    //let dataForm = {}
    //let urlConsulta = basePath
    //if (consultaPorVpn) {
    //    dataForm = {
    //        urlPublica: ipPublicaAlterna + '/servicio/ListarModelosidmarcaVpn',
    //        urlPrivada: ipPrivada + '/servicio/ListarModelosidmarca?codProgresivo=' + Progresivo + '&idmarca=' + MarcaId
    //    }
    //    urlConsulta += "Progresivo/ConsultarListaModeloMaquinaJsonVpn"
    //}
    //else {
    //    dataForm = {
    //        url: ipPublicaG + "/servicio/ListarModelosidmarca?codProgresivo=" + Progresivo + '&idmarca=' + MarcaId
    //    }
    //    urlConsulta += "Progresivo/ConsultarListaModeloMaquinaJson"
    //}
    //$.ajax({
    //    type: "POST",
    //    cache: false,
    //    url: urlConsulta,
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    data: JSON.stringify(dataForm),
    //    beforeSend: function (xhr) {
    //        $.LoadingOverlay("show");
    //    },
    //    success: function (response) {
    //        var msj = "";
    //        msj = response.mensaje != null ? response.mensaje : '';
    //        response = response.data;

    //        console.log(response.length);

    //    },
    //    error: function (request, status, error) {
    //        toastr.error("Error De Conexion, Servidor no Encontrado.");
    //    },
    //    complete: function (resul) {
    //        $.LoadingOverlay("hide");
    //    }
    //});
}

function EliminarModelo(Progresivo, ModeloId) {
    console.log("Modelo eliminado");
    //console.log(consultaPorVpn)
    //let dataForm = {}
    //let urlConsulta = basePath
    //if (consultaPorVpn) {
    //    dataForm = {
    //        urlPublica: ipPublicaAlterna + '/servicio/ListarModelosidmarcaVpn',
    //        urlPrivada: ipPrivada + '/servicio/ListarModelosidmarca?codProgresivo=' + Progresivo + '&idmarca=' + MarcaId
    //    }
    //    urlConsulta += "Progresivo/ConsultarListaModeloMaquinaJsonVpn"
    //}
    //else {
    //    dataForm = {
    //        url: ipPublicaG + "/servicio/ListarModelosidmarca?codProgresivo=" + Progresivo + '&idmarca=' + MarcaId
    //    }
    //    urlConsulta += "Progresivo/ConsultarListaModeloMaquinaJson"
    //}
    //$.ajax({
    //    type: "POST",
    //    cache: false,
    //    url: urlConsulta,
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    data: JSON.stringify(dataForm),
    //    beforeSend: function (xhr) {
    //        $.LoadingOverlay("show");
    //    },
    //    success: function (response) {
    //        var msj = "";
    //        msj = response.mensaje != null ? response.mensaje : '';
    //        response = response.data;

    //        console.log(response.length);

    //    },
    //    error: function (request, status, error) {
    //        toastr.error("Error De Conexion, Servidor no Encontrado.");
    //    },
    //    complete: function (resul) {
    //        $.LoadingOverlay("hide");
    //    }
    //});
}


function obtenerListaSalas() {
    ajaxhr = $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            return result;
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
    return ajaxhr
}
function renderSelectSalas(data) {
    $.each(data, function (index, value) {
        $("#cboSala").append(`<option value="${value.UrlProgresivo == "" ? "" : value.UrlProgresivo}" data-id="${value.CodSala}" data-ipprivada="${value.IpPrivada}" data-puertoservicio="${value.PuertoServicioWebOnline}" data-puertosignalr="${value.PuertoSignalr}">${value.Nombre}</option>`)
    });
}
function getPingSalas() {
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Progresivo/EchoPingSalasUsuario",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
        },
        success: function (response) {
            return response
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
            AbortRequest.close()
        }
    });
    AbortRequest.open()
    return ajaxhr
}

function llenarSelectAPIProgresivo__(url, data, select, dataId, dataValor, selectVal) {

    if (!url) {
        toastr.error("No se Declaro Url", "Mensaje Servidor");
        return false;
    }
    var mensaje = true;
    ajaxhr = $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: function () {
            $("#" + select).html("");
            $("#" + select).append('<option value="">Cargando...</option>');
            $("#" + select).attr("disabled", "disabled");
            //$.LoadingOverlay("show");
        },
        success: function (response) {
            var datos = response;
            var mensaje = response.mensaje;
            if (datos.length > 0) {
                $("#" + select).html("");
                $("#" + select).append('<option value="">--Seleccione--</option>');
                if (selectVal == "allOption") {
                    $("#" + select).append('<option value="0">Todos</option>');
                }
                $.each(datos, function (index, value) {
                    var selected = "";
                    if ($.isArray(selectVal)) {
                        if (objectFindByKey(selectVal, dataId, value[dataId]) != null) {
                            selected = "selected='selected'";
                        };
                    } else {

                        if (value[dataId] === selectVal) {
                            selected = "selected='selected'";
                        };
                    }
                    //$("#" + select).append('<option value="' + value[dataId] + '" data-id="' + value[dataId] +'" data-urlprogresivo="'+(urlsPrueba[Math.floor(Math.random() * urlsPrueba.length)])+'"  ' + selected + '>' + value[dataValor] + '</option>');
                    $("#" + select).append('<option value="' + value[dataId] + '" data-id="' + value[dataId] + '" data-urlprogresivo="' + value.WEB_Url + '"  ' + selected + '>' + value[dataValor] + '</option>');

                });
                $("#" + select).removeAttr("disabled");
            } else {
                toastr.error("No Hay Data  en " + select, "Mensaje Servidor");
            }
            //if (mensaje !== "") {
            //    toastr.error(mensaje, "Mensaje Servidor");
            //}
        },
        complete: function () {
            AbortRequest.close()
            //$.LoadingOverlay("hide");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mensaje = false;
            //toastr.error("Servidor no responde", "Mensaje Servidor");
            $("#" + select).html("");
            $("#" + select).append('<option value="">--Seleccione--</option>');
            $("#" + select).removeAttr("disabled");
        }
    });
    AbortRequest.open()
    return mensaje;
}

function validarURL(url) {
    try {
        let givenURL = new URL(url)
    } catch (error) {
        return false;
    }
    return true;
}
function getUrl(url) {
    if (url) {
        try {
            let uri = new URL(url)
            return uri

        } catch (ex) {
            return false
        }
    }
    return false
}
function getProgresivosVpn(urlPrivada, urlPublica) {
    ajaxhr = $.ajax({
        data: JSON.stringify({ urlPrivada: urlPrivada, urlPublica: urlPublica }),
        type: "POST",
        cache: false,
        url: basePath + '/Progresivo/listadoprogresivosVpn',
        contentType: "application/json: charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay('show')
        },
        success: function (response) {
            return response
        },
        complete: function (xhr) {
            AbortRequest.close()
            $.LoadingOverlay('hide')
        }
    })
    AbortRequest.open()
    return ajaxhr
}
function renderSelectProgresivos(data) {
    $("#cboProgresivo").html("")
    if (data) {
        $("#cboProgresivo").append('<option value="">--Seleccione--</option>');
        $.each(data, function (index, value) {
            $("#cboProgresivo").append(`
                <option 
                    value="${value["WEB_PrgID"]}" 
                    data-id="${value["WEB_PrgID"]}"
                    data-urlprogresivo="${value["WEB_Url"]}">
                    ${value["WEB_Nombre"]}
                </option>`)
        });
    }
}


function ObtenerMarca(Progresivo,MarcaId) {

    let dataForm = {}
    let urlConsulta = basePath
    if (consultaPorVpn) {
        dataForm = {
            urlPublica: ipPublicaAlterna + '/servicio/obtenerMarcaIdVpn',
            urlPrivada: ipPrivada + '/servicio/obtenerMarcaId?codProgresivo=' + Progresivo + '&idmarca=' + MarcaId
        }
        urlConsulta += "Progresivo/ObtenerMarcaIdVpn"
    }
    else {
        dataForm = {
            url: ipPublicaG + "/servicio/obtenerMarcaId?codProgresivo=" + Progresivo + '&idmarca=' + MarcaId
        }
        urlConsulta += "Progresivo/ObtenerMarcaId"
    }
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: urlConsulta,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            var msj = "";
            msj = response.mensaje != null ? response.mensaje : '';
            response = response.data;

            console.log(response);

            if (response == null) {
                toastr.error(msj,"Mensaje Servidor");
            } else {


                $("#textMarca").text("Editar");
                $("#marca_id").val(response.MarcaID);
                $("#nombreMarca").val(response.Nombre);
                $("#cboEstadoMarca").val(response.Estado == true ? "1" : "0");

                $("#full-modal_detalle_marca").modal('show');

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


};



function ObtenerModelo(Progresivo,  ModeloId) {


    let dataForm = {}
    let urlConsulta = basePath
    if (consultaPorVpn) {
        dataForm = {
            urlPublica: ipPublicaAlterna + '/servicio/obtenerModeloIdVpn',
            urlPrivada: ipPrivada + '/servicio/obtenerModeloId?codProgresivo=' + Progresivo + '&idmodelo=' + ModeloId
        }
        urlConsulta += "Progresivo/ObtenerModeloIdVpn"
    }
    else {
        dataForm = {
            url: ipPublicaG + "/servicio/obtenerModeloId?codProgresivo=" + Progresivo + '&idmodelo=' + ModeloId
        }
        urlConsulta += "Progresivo/ObtenerModeloId"
    }
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: urlConsulta,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            var msj = "";
            msj = response.mensaje != null ? response.mensaje : '';
            response = response.data;

            console.log(response);

            if (response == null) {
                toastr.error(msj, "Mensaje Servidor");
            } else {

                $("#textModelo").text("Editar");
                $("#modelo_id").val(response.ModeloID);
                $("#nombreModelo").val(response.Nombre);
                $("#cboEstadoModelo").val(response.Estado == true ? "1" : "0");

                $("#full-modal_detalle_modelo").modal('show');

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

};

function LimpiarMarcas() {

    var addtabla = $('.contenedor_tabla');
    $(addtabla).empty();
    $(addtabla).append('<table id="table1" class="table table-condensed table-bordered table-hover"></table>');
    objetodatatable1 = $("#table1").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": true,
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        data: [],
        columns: [
            { data: "MarcaID", title: "ID" },
            { data: "Nombre", title: "Nombre" },
            {
                data: "Estado", title: "Estado",
                "render": function (i, j, value) {
                    if (value.Estado != 1) {
                        return 'Inactivo';
                    }
                    else {
                        return 'Activo';
                    }
                }
            },
            {
                data: null, title: "Accion",
                render: function (value) {
                    let span = `<a class="btn btn-sm btn-warning btnVerModelos" data-id="${value.MarcaID}" data-nombre="${value.Nombre}"><i class="glyphicon glyphicon-search"></i> VER MODELOS</a>`
                    return '<button type="button" class="btn btn-sm btn-success btnEditarMarca" data-id="' + value.MarcaID + '" data-nombre="' + value.Nombre + '"><i class="glyphicon glyphicon-edit"></i> EDITAR</button> ' +
                        //'<button type="button" class="btn btn-sm btn-danger btnEliminarMarca" data-id="' + value.MarcaID + '" data-nombre="' + value.Nombre + '"><i class="glyphicon glyphicon-trash"></i> ELIMINAR</button> ' +
                        span
                }
            }

        ],

        "initComplete": function (settings, json) {
        },
    });
}
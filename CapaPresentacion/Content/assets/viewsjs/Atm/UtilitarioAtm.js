$(document).ready(function () {
    ipPublicaG = "";
    id_progresivo = "";
    $("#cboSala").select2();
    ObtenerListaSalas();

    $(document).on('change', '#cboSala', function (e) {
        var ipPublica = $(this).val();
        ipPublicaG = ipPublica;
        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        } 
        llenarSelectAPIProgresivo__(ipPublica + "/servicio/listadoatms", {}, "cboAtm", "ATMID", "Descripcion");
        console.log(ipPublicaG);
    });

    $("#btnBuscar").click(function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione una Sala.");
            return false;
        }
        if ($("#cboAtm").val() == "") {
            toastr.error("Seleccione un Atm.");
            return false;
        }
        if ($("#fechaApertura").val() == "") {
            toastr.error("Ingresar Fecha Apertura.");
            return false;
        }
        if ($("#turno").val() == "") {
            toastr.error("Seleccione un Turno.");
            return false;
        }

        var Atm = $("#cboAtm").val();
        BuscarApertura(Atm);
    });

});

function ObtenerListaSalas() {
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
                $("#cboSala").append('<option value="' + value.UrlProgresivo + '"  data-id="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
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

function llenarSelectAPIProgresivo__(url, data, select, dataId, dataValor, selectVal) {

    if (!url.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var mensaje = true;
    $.ajax({
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
                    $("#" + select).append('<option value="' + value[dataId] + '" data-id="' + value[dataId] + '"   ' + selected + '>' + value[dataValor] + '</option>');

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
            //$.LoadingOverlay("hide");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mensaje = false;

        }
    });
    return mensaje;
}

function ParseDate(dateString) {
    //dd.mm.yyyy, or dd.mm.yy
    var dateArr = dateString.split("/");
    if (dateArr.length == 1) {
        return null;    //wrong format
    }
    //parse time after the year - separated by space
    var spacePos = dateArr[2].indexOf(" ");
    if (spacePos > 1) {
        var timeString = dateArr[2].substr(spacePos + 1);
        var timeArr = timeString.split(":");
        dateArr[2] = dateArr[2].substr(0, spacePos);
        if (timeArr.length == 2) {
            //minutes only
            return new Date(parseInt(dateArr[2]), parseInt(dateArr[1] - 1), parseInt(dateArr[0]), parseInt(timeArr[0]), parseInt(timeArr[1]));
        } else {
            //including seconds
            return new Date(parseInt(dateArr[2]), parseInt(dateArr[1] - 1), parseInt(dateArr[0]), parseInt(timeArr[0]), parseInt(timeArr[1]), parseInt(timeArr[2]))
        }
    } else {
        //gotcha at months - January is at 0, not 1 as one would expect
        return new Date(parseInt(dateArr[2]), parseInt(dateArr[1] - 1), parseInt(dateArr[0]));
    }
}

function BuscarApertura(Atm) {
    var Fecha = ParseDate($("#fechaApertura").val()).toUTCString();
    //var Fecha = $("#fechaApertura").val();
    //var Turno = $("#turno").val();
    var IpAtm = $("#cboSala").val();
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url_ = ipPublicaG + "/servicio/ListarApertura?&IpAtm=" + IpAtm + "&fecha=" + Fecha ;
    var addtabla = $('.contenedor_tabla');

    $.ajax({
        url: basePath + "Atm/ConsultarApertura",
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ url: url_ }),
        async: true,
        processData: false,
        cache: false,
        before: function (data) {

        },
        success: function (response) {
            var msj = "";
            msj = response.mensaje != null ? response.mensaje : '';
            response = JSON.parse(response.data);
            if (response == null) {
                toastr.error("No Existe Apertura en la Fecha Seleccionada.");
                $(addtabla).empty();
                $(addtabla).append('<table id="table1" class="table table-condensed table-bordered table-hover"></table>');
                return;
            }

            let array = [response];
            response = array; 
            $(addtabla).empty();
            $(addtabla).append('<table id="table1" class="table table-condensed table-bordered table-hover"></table>');

            if (msj != "") {
                toastr.error("Error De Conexion, Servidor no Encontrado");
            } else {
                //dataAuditoria(1, "#formfiltro", 3, "Progresivo/ConsultarListaMaquinaProgresivo", "BOTON BUSCAR");
                objetodatatable = $("#table1").DataTable({
                    "bDestroy": true,
                    "bSort": false,
                    "scrollCollapse": true,
                    "scrollX": true,
                    "paging": true,
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response,
                    columns: [
                        { data: "BovedaID", title: "BovedaID", visible: false },

                        { data: "FechaInicial", title: "Fecha Apertura" },
                        {
                            data: "TurnoOperativo", title: "Estado",
                            "render": function (i, j, value) {
                                if (value.TurnoOperativo != 1) {
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
                                return '<button type="button" class="btn btn-sm btn-success btnEditar" data-id="' + value.BovedaID + '" data-turno ="' + value.TurnoOperativo + '"><i class="glyphicon glyphicon-edit" data-id="' + value.BovedaID + '" data-turno ="' + value.TurnoOperativo + '"></i></button> ';
                                //return '<button type="button" class="btn btn-sm btn-success btnEditar" data-id="' + value.BovedaID + '"><i class="glyphicon glyphicon-edit" data-id="' + value.BovedaID + '"></i></button> ';
                            }
                        }

                    ],

                    "initComplete": function (settings, json) {

                    },
                });

                $(".btnEditar").click(function (e) {
                    $("#modalEditarApertura").modal("show");
                    var bovedaId = $(e.target).attr('data-id');
                    $("#txtBovedaIdNueva").val(bovedaId);
                    var turno = $(e.target).attr('data-turno');
                    $("#txtTurno").val(turno);
                });

                $('.btnEditar').tooltip({
                    title: "Editar"
                });
            }
        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

$('#btnAplicar').on('click', function (e) {
    var fechaNueva = ParseDate($("#fechaEditarApertura").val()).toUTCString();
    var bovedaId = $("#txtBovedaIdNueva").val();
    var Turno = $("#txtTurno").val();
    var IpAtm = $("#cboSala").val();
    if (!$("#cboSala").val().trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url_ = $("#cboSala").val() + "/servicio/VerificarApertura?IpAtm=" + IpAtm + "&bovedaId=" + bovedaId + "&fechaNueva=" + fechaNueva;

    $.ajax({
        url: basePath + "Atm/ValidarApertura",
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ url: url_ }),
        async: true,
        processData: false,
        cache: false,
        before: function (data) {

        },
        success: function (response) {
            if (response.data == 1) {
                toastr.error("Ya existe una Apertura con esta Fecha.");
                return;
            }
            else {
                var fechaNueva = ParseDate($("#fechaEditarApertura").val()).toUTCString();
                var Turno = $("#txtTurno").val();
                var bovedaId = $("#txtBovedaIdNueva").val();
                var IpAtm = $("#cboSala").val();
                var url__ = $("#cboSala").val() + "/servicio/ModificarApertura?IpAtm=" + IpAtm + "&bovedaId=" + bovedaId + "&Turno=" + Turno + "&fechaNueva=" + fechaNueva;

                $.confirm({
                    icon: 'fa fa-spinner fa-spin',
                    title: 'Esta Seguro que desea Modificar la Apertura ?',
                    theme: 'black',
                    animationBounce: 1.5,
                    columnClass: 'col-md-6 col-md-offset-3',
                    confirmButtonClass: 'btn-info',
                    cancelButtonClass: 'btn-warning',
                    confirmButton: "confirmar",
                    cancelButton: 'Cancelar',
                    content: "",
                    confirm: function () {
                        $.ajax({
                            url: basePath + "Atm/ModificarApertura",
                            dataType: "json",
                            type: "POST",
                            contentType: 'application/json; charset=utf-8',
                            data: JSON.stringify({ url: url__ }),
                            async: true,
                            processData: false,
                            cache: false,
                            before: function (data) {
                                $.LoadingOverlay("show");
                            },
                            success: function (response) {
                                if (response.data == "true") {
                                    $("#modalEditarApertura").modal("hide");
                                    toastr.success("Se ha actualizado la Apertura satisfactoriamente.', 'Mensaje Servidor'");
                                    setTimeout(function () {
                                        window.location.replace(basePath + "Atm/UtilitarioAtmVista");
                                    }, 2200);
                                } else {
                                    toastr.error('No se pudo actualizar la Apertura.', 'Mensaje Servidor');
                                }
                            },
                            error: function (request, status, error) {
                                toastr.error("Error De Conexion, Servidor no Encontrado.");
                            },
                            complete: function (resul) {
                                $.LoadingOverlay("hide");
                            }
                        });
                    },
                    cancel: function () {

                    },

                });
            }

        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
});
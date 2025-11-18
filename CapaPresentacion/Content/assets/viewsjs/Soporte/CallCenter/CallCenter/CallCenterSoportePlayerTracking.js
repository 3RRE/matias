$(document).ready(function () {
    ipPublicaG = "";
    nombretabla = "";
   
    ObtenerListaSalas();
    $("#cboSala").select2();
    $("#cboClientes").select2();
    $("#cboSorteos").select2();
    
    $("#cboClientesAdicionar").select2({
        dropdownParent: $("#modal-adicionar")
    });
    $("#cboClientes_restar").select2({
        dropdownParent: $("#modal-restar")
    });
    $("#cboPromoSorteo_adicionar").select2({
        dropdownParent: $("#modal-adicionar")
    });
    $("#cboPromoSorteo_restar").select2({
        dropdownParent: $("#modal-restar")
    });

    $(document).on('change', '#cboSala', function (e) {
        var ipPublica = $(this).val();
        ipPublicaG = ipPublica;
        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }
        console.log(ipPublica);
        var url_clientesptk = ipPublicaG + "/servicio/ObtenerClientesPtk";
        var url_promo_sorteo = ipPublicaG + "/servicio/ObtenerPromocionSorteoActivo?estadopromosorteoid=2";
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "CallCenter/ConsultaObtenerClientesPtkJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ url: url_clientesptk }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (result) {
                //debugger
                var msj = "";
                msj = result.mensaje != null ? result.mensaje : '';
                var datos = result.data;
                if (msj != "") {
                    toastr.error("Error De Conexion, Servidor no Encontrado.");
                } else {
                    $.each(datos, function (index, value) {
                        $("#cboClientes").append('<option value="' + value.ClienteID + '"  data-id="' + value.ClienteID + '"  >' + value.Cliente + '</option>');
                        $("#cboClientesAdicionar").append('<option value="' + value.ClienteID + '"  data-id="' + value.ClienteID + '"  >' + value.Cliente + '</option>');
                        $("#cboClientes_restar").append('<option value="' + value.ClienteID + '"  data-id="' + value.ClienteID + '"  >' + value.Cliente + '</option>');
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

        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "CallCenter/ConsultaObtenerPromocionSorteoActivoJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ url: url_promo_sorteo }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                //var datos = result.data;
                var msj = "";
                msj = result.mensaje != null ? result.mensaje : '';
                var datos = result.data;
                if (msj != "") {
                    toastr.error("Error De Conexion, Servidor no Encontrado.");
                } else {
                    $.each(datos, function (index, value) {
                        $("#cboSorteos").append('<option value="' + value.PromoSorteoID + '"  data-id="' + value.PromoSorteoID + '"  >' + value.NombrePromoSorteo + '</option>');
                        $("#cboPromoSorteo_adicionar").append('<option value="' + value.PromoSorteoID + '"  data-id="' + value.PromoSorteoID + '"  >' + value.NombrePromoSorteo + '</option>');
                        $("#cboPromoSorteo_restar").append('<option value="' + value.PromoSorteoID + '"  data-id="' + value.PromoSorteoID + '"  >' + value.NombrePromoSorteo + '</option>');
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

    $(document).on('click', '.btn-adicionar', function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        else if ($("#cboPromoSorteo_adicionar").val() == "") {
            toastr.error("Seleccione una Promo Sorteo.");
            return false;
        } else if ($("#cboClientesAdicionar").val() == "") {
            toastr.error("Seleccione un Cliente.");
            return false;
        }
        else if ($("#cantidad_adicionar").val() == "") {
            toastr.error("Ingrese una Cantidad a adicionar.");
            return false;
        } else if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }
        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Desea adicionar puntos/cupones al cliente?',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: "confirmar",
            cancelButton: 'Aún No',
            content: "",
            confirm: function () {
                var url = ipPublicaG + "/servicio/AdicionarPuntoCupon?cantidad=" + $("#cantidad_adicionar").val() + "&clienteId=" + $("#cboClientesAdicionar").val() + "&promoSorteoId=" + $("#cboPromoSorteo_adicionar").val();
                
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: basePath + "CallCenter/ConsultaAdicionarPuntoCuponJson",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ url: url }),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (response) {
                        dataAuditoria(1, "#formadicionar,#formfiltro", 3, "CallCenter/ConsultaAdicionarPuntoCuponJson", "BOTON ADICIONAR");
                        response = response.data;
                        console.log(response);
                        toastr.success(response);
                        $("#cantidad_adicionar").val("");
                        $("#cboClientesAdicionar").val(0).change();
                        $("#cboPromoSorteo_adicionar").val(0).change();                        
                    },
                    error: function (request, status, error) {
                        toastr.error("Error De Conexion, Servidor no Encontrado.");
                        $.LoadingOverlay("hide");
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide");
                    }
                });

            },
            cancel: function () {
            },
        });

    });

    $(document).on('click', '.btn-restar', function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        else if ($("#cboPromoSorteo_restar").val() == "") {
            toastr.error("Seleccione una Promo Sorteo.");
            return false;
        } else if ($("#cboClientes_restar").val() == "") {
            toastr.error("Seleccione un Cliente.");
            return false;
        }
        else if ($("#cantidad_restar").val() == "") {
            toastr.error("Ingrese una Cantidad a restar.");
            return false;
        } else if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }

        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: '¿Desea quitarle puntos/cupones al cliente?',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: "confirmar",
            cancelButton: 'Aún No',
            content: "",
            confirm: function () {
                var url = ipPublicaG + "/servicio/RestarPuntoCupon?cantidad=" + $("#cantidad_restar").val() + "&clienteId=" + $("#cboClientes_restar").val() + "&promoSorteoId=" + $("#cboPromoSorteo_restar").val();
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: basePath + "CallCenter/ConsultaRestarPuntoCuponJson",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ url: url }),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (response) {
                        dataAuditoria(1, "#formadicionar,#formfiltro", 3, "CallCenter/ConsultaRestarPuntoCuponJson", "BOTON RESTAR");
                        response = response.data;
                        console.log(response);
                        toastr.success(response);
                        $("#cantidad_restar").val("");
                        $("#cboClientes_restar").val(0).change();
                        $("#cboPromoSorteo_restar").val(0).change();
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

    });

   
    $("#btnBuscar").on("click", function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if ($("#fechaIni").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if ($("#fechaFin").val() == "") {
            toastr.error("Ingrese una fecha de Fin");
            return false;
        }
        if ($("#cantidadPuntosCupon").val() == "") {
            toastr.error("Ingrese una cantidad de puntos");
            return false;
        }
        buscarPtk();
    });



    fechaMin = moment(new Date()).format('YYYY-MM-DD');
    var dateMin = new Date(fechaMin);
    dateMin.setDate(dateMin.getDate() - 0);
    $(".dateOnly_soporte_ini").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        showToday: true,
        minDate: dateMin,
        maxDate: moment(new Date()),
        defaultDate: moment(new Date())
    });

    $(".dateOnly_soporte_fin").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        showToday: true,
        useCurrent: false,
        //minDate: dateMin,
        defaultDate: moment(new Date())
    });

    $(".dateOnly_soporte_fin").on("dp.change", function (e) {
        $('.dateOnly_soporte_ini').data("DateTimePicker").destroy();
        $('#fechaIni').val(moment(e.date).format('DD/MM/YYYY'));
        var fechaMin = moment(e.date).format('YYYY/MM/DD');
        var dateMin = new Date(fechaMin);
        dateMin.setDate(dateMin.getDate() - 31);

        $(".dateOnly_soporte_ini").datetimepicker({
            pickTime: false,
            format: 'DD/MM/YYYY',
            showToday: true,
            minDate: dateMin,
            maxDate: moment(e.date).format('DD/MM/YYYY'),
            defaultDate: moment(e.date).format('DD/MM/YYYY')
        });

    });


    //$(".dateOnly_soporte_ini").datetimepicker({
    //    pickTime: false,
    //    format: 'DD/MM/YYYY',
    //    defaultDate: dateNow,
    //    maxDate: datenowmax,
    //    minDate: dateNowMin,
    //});
    //$(".dateOnly_soporte_fin").datetimepicker({
    //    pickTime: false,
    //    format: 'DD/MM/YYYY',
    //    defaultDate: dateNow,
    //    maxDate: datenowmax,
    //    minDate: dateNowMin,
    //});
});

function buscarPtk() {
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    }
    var url = ipPublicaG + "/servicio/ObtenerSeguimientoPuntoCuponGenerado?fechaIni=" + $("#fechaIni").val() +
        "&fechaFin=" + $("#fechaFin").val() + "&cantidadPuntosCupon=" + $("#cantidad_ptos_cupon").val() +
        "&clienteId=" + $("#cboClientes").val() + "&promoSorteoId="+$("#cboSorteos").val();
    var addtabla = $(".contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CallCenter/ObtenerSeguimientoPuntoCuponGenerado",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        
        success: function (response) {

            dataAuditoria(1, "#formfiltro", 3, "CallCenter/ObtenerSeguimientoPuntoCuponGenerado", "BOTON BUSCAR");
            response = response.data;
            $(addtabla).empty();
            $(addtabla).append('<table id="ResumenSala" class="table table-condensed table-bordered table-hover"></table>');
            objetodatatable = $("#ResumenSala").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "sScrollX": "100%",
                "paging": true,
                "autoWidth": false,
                "bAutoWidth": true,
                "bProcessing": true,
                "bDeferRender": true,
                data: response,
                columns: [
                    { data: "NombrePromoSorteo", title: "NombrePromoSorteo" },
                    { data: "DescripcionPromoSorteo", title: "DescripcionPromoSorteo" },
                    { data: "Cliente", title: "Cliente" },
                    { data: "puntocupontotalxregjuego", title: "puntocupontotalxregjuego" },
                    { data: "SlotID", title: "SlotID" },
                    { data: "MontoCoinIn", title: "MontoCoinIn" },
                    { data: "MontoCoinOut", title: "MontoCoinOut" },
                    { data: "MontoHandPay", title: "MontoHandPay" },
                    { data: "TarjetaRFID", title: "TarjetaRFID" },
                    { data: "doi", title: "doi" }
                ]
                ,
                "initComplete": function (settings, json) {

                    $('#btnExcel').off("click").on('click', function () {

                        cabecerasnuevas = [];
                        cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
                        

                        definicioncolumnas = [];
                        definicioncolumnas.push({ nombre: "NombrePromoSorteo", tipo: "string", alinear: "center"});
                        definicioncolumnas.push({ nombre: "MontoCoinIn", tipo: "decimal", alinear: "right", sumar: "true" });
                        definicioncolumnas.push({ nombre: "MontoCoinOut", tipo: "decimal", alinear: "right", sumar: "true" });
                        definicioncolumnas.push({ nombre: "MontoHandPay", tipo: "decimal", alinear: "right", sumar: "true" });
                        

                        var ocultar = [];//"Accion";
                        funcionbotonesnuevo({
                            botonobjeto: this, ocultar: ocultar,
                            tablaobj: objetodatatable,
                            cabecerasnuevas: cabecerasnuevas,
                            definicioncolumnas: definicioncolumnas
                        });
                        VistaAuditoria("CallCenter/CallCenterSoportePlayerTrackingVistaExcel", "EXCEL", 0, "", 3);
                    });

                },
            });
            //$('#ResumenSala tbody').on('click', 'tr', function () {
            //    $(this).toggleClass('selected');
            //});

        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
function ObtenerListaSalas() {
    comboImagen = $("#cboImagen");
    comboImagen.find('option').remove();
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
function ObtenerListaClientes() {
    //ConsultaObtenerClientesPtkJson
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    }
    var url = ipPublicaG + "/servicio/ObtenerClientesPtk";
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CallCenter/ConsultaObtenerClientesPtkJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (result) {
            var datos = result.data;
            $.each(datos, function (index, value) {
                $("#cboClientes").append('<option value="' + value.ClienteID + '"  data-id="' + value.ClienteID + '"  >' + value.Cliente + '</option>');
            });
        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
    //return false;
}

VistaAuditoria("CallCenter/CallCenterSoportePlayerTrackingVista", "VISTA", 0, "", 3);
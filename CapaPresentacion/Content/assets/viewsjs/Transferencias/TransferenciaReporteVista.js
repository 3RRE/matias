$(document).ready(function () {
    ObtenerListaSalas();
   
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
    setCookie("datainicial", "");
    $("#btnBuscar").on("click", function () {
        if ($("#cboSala").val() == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if ($("#fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if ($("#fechaFin").val() == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }
        buscarTransferencias();
    });

    $(document).on("click", "#btnExcel", function () {
        var fechaini = $("#fechaInicio").val();
        var fechafin = $("#fechaFin").val();
        var sala = $("#cboSala").val();
        if (sala == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }

        //var listasala = [];
        //$("#cboSala option:selected").each(function () {
        //    listasala.push($(this).data("id"));
        //});
        var listasala = $("#cboSala").val();
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Transferencias/reporteTransferenciaDescargarExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                dataAuditoria(1, "#formfiltro", 3, "Transferencias/reporteTransferenciaDescargarExcelJson", "BOTON EXCEL");
                if (response.respuesta) {
                    var data = response.data;
                    var file = response.excelName;
                    let a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/vnd.ms-excel;base64, " + data;
                    a.download = file;
                    a.click();
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

    $(document).on("click", ".btnEstado", function () {
        var id = $(this).data("id");
        
       
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Transferencias/DetalleTransferenciaIdJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ id }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                console.log(response)
                if (response.respuesta) {
                    dataAuditoria(1, "#formfiltro", 3, "Transferencias/DetalleTransferenciaIdJson", "BOTON DETALLE");
                    var tickets = response.lista;
                    var datos = response.data;
                    $("#tbodytickets").html("");
                    $("#imagen_").attr("src", "data:image/png;base64," + datos.imagenbase64voucher);
                    $.each(tickets, function (index, value) {
                        $("#tbodytickets").append('<tr><td>' + value.NroTicketTito + '</td><td class="tdright"> S/.' + value.Monto.toFixed(2) + '</td></tr>');
                    });
                    $("#full-modal_detalle").modal("show");
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


    VistaAuditoria("Transferencias/Reporte", "VISTA", 0, "", 3);
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
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                multiple: true, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboSala").val(null).trigger("change");
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

function buscarTransferencias() {
    //var listasala = [];
    //$("#cboSala option:selected").each(function () {
    //    listasala.push($(this).data("id"));
    //});
    var listasala = $("#cboSala").val();

    //var sala = $("#cboSala option:selected").data("id");

    var fechaini = $("#fechaInicio").val();
    var fechafin = $("#fechaFin").val();
    var addtabla = $(".contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Transferencias/ReporteTransferenciasSalaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            console.log(response)
            var total = response.total;
            response = response.data;
            
            dataAuditoria(1, "#formfiltro", 3, "Transferencias/ConsultaTransferenciasSalaJson", "BOTON BUSCAR");
            $(addtabla).empty();
            $(addtabla).append('<table id="ResumenSala" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
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

                    { data: "TransferenciaID", title: "ID" },
                    {
                        data: "FechaReg", title: "Fecha Reg.",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY hh:mm A');
                        }
                    },
                    { data: "nombresala", title: "Sala" },
                    { data: "TipoDocNombre", title: "Tipo Doc." },
                    { data: "ClienteNroDoc", title: "Nro. Doc" },
                    {
                        data: null, title: "Cliente", "render": function (value) {

                            return value.ClienteApelPat + " " + value.ClienteApelMat + " " + value.ClienteNombre;
                        }
                    },
                    { data: "BancoNombre", title: "Banco" },
                    { data: "NroCuenta", title: "Cuenta" },
                    {
                        data: "Monto", title: "Monto", className: "tdright", "render": function (value) {

                            return value.toFixed(2);
                        }
                    },
                    { data: "NroOperacion", title: "Nro. Ope." },
                    {
                        data: "FechaOperacion", title: "Fecha Ope.",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY');
                        }
                    },
                    { data: "UsuarioNombre", title: "Usu. Reg." },
                    { data: "usuariosala", title: "Usu. Sala" },
                    {
                        data: null, title: "Detalle",
                        "render": function (value) {
                            var estado = value.Estado;
                            var butom = "";
                            butom = `<button type="button" class="btn btn-xs btn-warning btnEstado" data-id="${value.TransferenciaID}" data-json='${JSON.stringify(value)}'><i class="glyphicon glyphicon-pencil"></i>  Detalle</button>`
                            return butom;
                        }
                    }
                ]
                ,
                "initComplete": function (settings, json) {



                },
                "drawCallback": function (settings) {

                }
            });
            $("#stmonto").html(total.toFixed(2));
            $('#ResumenSala tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
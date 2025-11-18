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
        buscarDepositos();
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
            url: basePath + "Transferencias/reporteDepositosDescargarExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                dataAuditoria(1, "#formfiltro", 3, "Transferencias/reporteDepositosDescargarExcelJson", "BOTON EXCEL");
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

    VistaAuditoria("Deposito/Reporte", "VISTA", 0, "", 3);

    $(document).on("click", ".tickets", function () {
        var id = $(this).data("id");
        var nombre = $(this).data("nombre");
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Transferencias/TicketsDepositoJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ id }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {

                var registros = response.data;
                if (registros.length > 0) {
                    var datas = "";
                    $.each(registros, function (index, value) {
                        datas += '<div>Ticket : ' + value.NroTicketTito + ' - Monto S/.' + value.Monto + '</div>'
                    });
                    $("#cuenta_banco").html(datas);
                    $("#nom_cliente").html(nombre);
                    $("#full-modal").modal("show");
                }
                else {
                    toastr.warning("No tiene Tickets Registrados.");
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

function buscarDepositos() {
   
    var fechaini = $("#fechaInicio").val();
    var fechafin = $("#fechaFin").val();

    //var listasala = [];
    //$("#cboSala option:selected").each(function () {
    //    listasala.push($(this).data("id"));
    //});
    var listasala = $("#cboSala").val();
    var addtabla = $(".contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Transferencias/ReporteDepositosSalaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            //console.log(response)
            var total = response.total;
            response = response.data;
            dataAuditoria(1, "#formfiltro", 3, "Transferencias/ReporteDepositosSalaJson", "BOTON BUSCAR");
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

                    { data: "DepositoID", title: "ID" },
                    { data: "nombresala", title: "Sala" },
                    {
                        data: "FechaReg", title: "Fecha Reg.",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY hh:mm A');
                        }
                    },
                   
                    { data: "TipoDocNombre", title: "Tipo Doc." },
                    { data: "ClienteNroDoc", title: "Nro. Doc" },
                    {
                        data: null, title: "Cliente", "render": function (value) {

                            return value.ClienteApelPat + " " + value.ClienteApelMat + " " + value.ClienteNombre;
                        }
                    },
                 
                    {
                        data: "Monto", title: "Monto", className: "tdright", "render": function (value) {

                            return value.toFixed(2);
                        }
                    },
                    { data: "NroTickets", title: "Nro. Tickets" ,className: "tdcenter"},
                    { data: "NroOperacion", title: "Nro. Ope." },
                    { data: "UsuarioNombreReg", title: "Usu. Sala Reg.", className: "tdcenter" },
                    //{
                    //    data: "FechaOperacion", title: "Fecha Ope.",
                    //    "render": function (value) {
                    //        return moment(value).format('DD/MM/YYYY');
                    //    }
                    //},
                    {
                        data: null, title: "",
                        "render": function (value) {
                            var butom = "";
                            var empleado = value.ClienteApelPat + " " + value.ClienteApelMat + " " + value.ClienteNombre;
                            butom = '<button type="button" class="btn btn-xs btn-success tickets" data-nombre="' + empleado +'" data-id="' + value.DepositoID + '"><i class="glyphicon glyphicon-pencil"></i> TIckets</button> '
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
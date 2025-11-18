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
            toastr.error("Seleccione Sede", "Mensaje Servidor");
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
        buscarHistorial();
    });

    $(document).on("click", "#btnExcel", function () {
        var fechaini = $("#fechaInicio").val();
        var fechafin = $("#fechaFin").val();
        var sala = $("#cboSala").val();
        if (sala == null) {
            toastr.error("Seleccione Sede", "Mensaje Servidor");
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
            url: basePath + "Bonificaciones/reporteFormularioRegistroDescargarExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                dataAuditoria(1, "#formfiltro", 3, "Bonificaciones/reporteFormularioRegistroDescargarExcelJson", "BOTON EXCEL");
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
            //error: function (request, status, error) {
            //    toastr.error("Error De Conexion, Servidor no Encontrado.");
            //},
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });

    VistaAuditoria("Bonificaciones/Reporte", "VISTA", 0, "", 3);
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

function buscarHistorial() {
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
        url: basePath + "Bonificaciones/reporteFormularioRegistroJson",
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

            dataAuditoria(1, "#formfiltro", 3, "Bonificaciones/reporteFormularioRegistroJson", "BOTON BUSCAR");
            $(addtabla).empty();
            $(addtabla).append('<table id="ResumenSala" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#ResumenSala").DataTable({
                "bDestroy": true,
                "bSort": false,
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

                    { data: "bon_id", title: "ID" },
                    {
                        data: "bon_fecha", title: "Fecha",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY');
                        }
                    },
                    { data: "nombresala", title: "Sede" },
                    { data: "bon_ticket", title: "Nro. Ticket" },
                    {
                        data: "", title: "Colaborador", "render": function (value,x,u) {
                            var nombre = u.bon_apepaterno + " " + u.bon_apematerno + " " + u.bon_nombre;
                            return nombre;
                        }
                    },
                    {
                        data: "bon_monto", title: "Monto", className: "tdright", "render": function (value) {

                            return value.toFixed(2);
                        }
                    },
                    {
                        data: "bon_fecharegistro", title: "Fecha Pago",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY hh:mm:ss A');
                        }
                    },
                    
                    { data: "nombreusuario", title: "Usu. Pago" },
                    {
                        data: "bon_estado", title: "Estado", "render": function (value) {
                            var mensaje = "Pendiente";
                            if (value == 1) {
                                mensaje = "Pagado";
                            }
                            return mensaje;
                        } },
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
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
$(document).ready(function () {
    ObtenerListaSalas();
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        //maxDate: dateNow,
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
        buscarhistorial();
    });

    $(document).on("click", "#btnExcel", function () {
        var fechaini = $("#fechaInicio").val();
        var fechafin = $("#fechaFin").val();
        var sala = $("#cboSala").val();
        if (sala == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }

    
        var listasala = $("#cboSala").val();
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "AlertaBilleteros/ReporteAlertasSalaDescargarExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                dataAuditoria(1, "#formfiltro", 3, "AlertaBilleteros/ReporteAlertasSalaDescargarExcelJson", "BOTON EXCEL");
                if (response.respuesta) {
                    var data = response.data;
                    var file = response.excelName;
                    let a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/vnd.ms-excel;base64, " + data;
                    a.download = file;
                    a.click();
                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
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
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
    return false;
}

function buscarhistorial() {
    //var listasala = [];
    //$("#cboSala option:selected").each(function () {
    //    listasala.push($(this).data("id"));
    //});
    var listasala = $("#cboSala").val();

    //var sala = $("#cboSala option:selected").data("id");

    var fechaini = $("#fechaInicio").val();
    var fechafin = $("#fechaFin").val();
    var addtabla = $(".contenedor_tabla");
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "AlertaBilleteros/ListarAlertasxFechaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            AbortRequest.close()

            console.log(response)
            response = response.data;

            dataAuditoria(1, "#formfiltro", 3, "AlertaBilleteros/ListarAlertasxFechaJson", "BOTON BUSCAR");
            $(addtabla).empty();
            $(addtabla).append('<table id="alertastbl" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#alertastbl").DataTable({
                order: [[0, 'desc']],
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
                    { data: "alts_id", title: "ID" },
                    { data: "CodEmpresa", title: "Cod. Empresa" },
                    { data: "NombreEmpresa", title: "Nombre Empresa" },
                    { data: "CodSala", title: "Cod. Sala" },
                    { data: "NombreSala", title: "Nombre Sala" },
                    { data: "CodMaquina", title: "Cod. Maquina" },
                    { data: "fecha_registro", title: "Fecha Reg. Sala" },
                    { data: "fecha_termino", title: "Fecha Fin Sala" },
                    { data: "descripcion_alerta", title: "Descripción" },
                    {
                        data: "ColorAlerta", title: "Color",
                        "render": function (value) {
                            return '<div style="width: 15px;height: 15px;background: '+value+';border: 1px solid '+value+';border-radius: 17px !important;margin-left: auto;margin-right: auto;"></div>';
                        }
                    },
                    { data: "contador_bill_parcial", title: "Cont. Bill. Parc" },
                    { data: "contador_bill_billetero", title: "Cont. bill." },
                    {
                        data: "estado", title: "Estado" , "render": function (value) {
                            var estado = "Activo";
                            var css = "btn-success";

                            switch (value) {
                                case 0:
                                    estado = "Desactivado"
                                    css = "btn-danger";
                                    break;
                                case 1:
                                    estado = "Activo"
                                    css = "btn-success";
                                    break;

                            }
                            return '<span class="label ' + css + '">' + estado + '</span>';
                        }},
                    {
                        data: "alts_fechareg", title: "Fecha Reg.",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY hh:mm:ss A');
                        }
                    },
                   
                ]
                ,
                "initComplete": function (settings, json) {



                },
                "drawCallback": function (settings) {

                }
            });

            $('#alertastbl tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
            AbortRequest.close()

        }
    });
    AbortRequest.open()

    return ajaxhr
}
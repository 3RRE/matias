
$(document).ready(function () {
    ListarTecnicos();

    $(document).on('click', '#btnNuevo', function () {
        var url = basePath + "Tecnico/TecnicoInsertarVista";
        window.location.replace(url);
    });

    $(document).on('click', '.btnEditar', function () {
        var id = $(this).data("id");
        var url = basePath + "Tecnico/TecnicoEditarVista/Registro" + id;
        window.location.replace(url);
    });

    $(document).on('click', '.selectTecnico', function (e) {
        var id = $(this).data("id");
        $('#Id_Tecnico').remove();
        $(this).parent("td").append('<input type="hidden" class="record" name="Id_Tecnico" id="Id_Tecnico" value="' + id + '" >');
    });

    $(document).on('change', '.selectTecnico', function (e) {
        debugger
        var estado = $(this).val();
        var texto = $(this).find("option:selected").text();
        var TecnicoId = jQuery(this).closest('td').next('td').find("button.btnEditar").data("id");
        var data = { TecnicoId: TecnicoId, TecnicoEstado: estado }
        var url = basePath + "Tecnico/ActualizarEstadoTecnico";
        DataPostWithoutChange(url, data, false);
        //ListarTecnicos();
    });

    VistaAuditoria("Tecnico/TecnicoListarVista", "VISTA", 0, "", 3);

});

function ListarTecnicos() {
    var url = basePath + "Tecnico/TecnicoListarNombreEmpleadoJson";
    var data = {};
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            respuesta = response.data
            objetodatatable = $("#table").DataTable({
                "bDestroy": true,
                "bSort": false,
                "scrollCollapse": true,
                "scrollX": false,
                "paging": true,
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                "initComplete": function (settings, json) {

                    //   afterTableInitialization(settings,json)
                    $('button#excel,a#pdf,a#imprimir').off("click").on('click', function () {
                        ocultar = ["Accion"];//array de columnas para ocultar , usar titulo de columna
                        columna_cambio = [{
                            nombre: "Estado",
                            render: function (o) {
                                valor = "";
                                if (o == 1) {
                                    valor = "Habilitado";
                                }
                                else { valor = "Deshabilitado"; }
                                return valor;
                            }
                        }]
                        cabecerasnuevas = [];
                        //cabecerasnuevas.push({ nombre: "cabecera", valor: "vdfcs" });
                        //tituloreporte = "Reporte Empleados";
                        funcionbotonesnuevo({
                            botonobjeto: this, tablaobj: objetodatatable, ocultar: ocultar/*, tituloreporte: tituloreporte*/, cabecerasnuevas: cabecerasnuevas, columna_cambio: columna_cambio
                        });
                        VistaAuditoria("Empleado/EmpleadoListadoExcel", "EXCEL", 0, "", 3);
                    });
                },
                data: response.data,
                columns: [
                    { data: "NombreCompleto", title: "Nombres" },
                    {
                        data: "Estado", title: "Estado",
                        "render": function (o, i, j) {
                            var selectedOptions = "";
                            //console.log(j)
                            var id = j.EmpleadoID;
                            if (o == 1) {
                                selectedOptions = '<option value="1" selected>Habilitado</option>' +
                                    '<option value="0">Deshabilitado</option>';
                            } else {
                                selectedOptions = '<option value="1">Habilitado</option>' +
                                    '<option value="0" selected>Deshabilitado</option>';
                            }
                            return '<select class="form-control input-sm selectTecnico auditoriaFocus" data-id="' + id + '" id="EstadoEmpleado" name="EstadoEmpleado">' + selectedOptions + '</select>';
                        }
                    },
                    {
                        data: "TecnicoId",
                        "bSortable": false,
                        "render": function (o) {
                            return '<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="' + o + '"><i class="glyphicon glyphicon-pencil"></i></button>';
                        }
                    }
                ],
                "drawCallback": function (settings) {
                    $('.btnEditar').tooltip({
                        title: "Editar"
                    });
                }
            });
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
        }
    });
};


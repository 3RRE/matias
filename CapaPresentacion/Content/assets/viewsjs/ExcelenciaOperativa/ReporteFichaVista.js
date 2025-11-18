// Listar Salas Asignadas
function ListarSalasAsignadas() {
    var element = $("#cboSalas")

    $.ajax({
        type: "POST",
        url: `${basePath}Sala/ListadoSalaPorUsuarioJson`,
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $.LoadingOverlay("show")
            element.html(`<option value="">--Seleccione--</option>`)
        },
        success: function (response) {
            var items = response.data

            $.each(items, function (index, item) {
                element.append(`<option value="${item.CodSala}">${item.Nombre}</option>`)
            })
        },
        error: function () {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}


let objetodatatable
$(document).ready(function () {

    // Listar Salas Asignadas
    //ListarSalasAsignadas()

    //$("#cboSalas").select2()

    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
    });

    $(".dateOnly_fechaini").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
    });

    $(".dateOnly_fechafin").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
    });

    $("#cboTipo").select2({
        multiple: true, placeholder: "--Seleccione--", allowClear: true
    });

    $("#cboTipo").val(null).trigger("change");

    setCookie("datainicial", "");

    $(document).on('click', '#btnBuscar', function (event) {
        event.preventDefault()

        //var roomCode = $('#cboSalas').val()
        var type = $("#cboTipo").val()
        var dateStart = $("#fechaInicio").val()
        var dateEnd = $("#fechaFin").val()

        toastr.clear()

        if (!type) {
            toastr.warning("Seleccione un tipo")

            return false
        }

        if (!dateStart) {
            toastr.warning("Ingrese una fecha de Inicio")

            return false
        }

        if (!dateEnd) {
            toastr.warning("Ingrese una fecha Fin.")

            return false
        }

        buscarFichasExcelenciasOperativas()
    })

    $(document).on('click', '#btnDescargaMultiple', function (e) {
        event.preventDefault()

        //var roomCode = $('#cboSalas').val()
        var type = $("#cboTipo").val()
        var dateStart = $("#fechaInicio").val()
        var dateEnd = $("#fechaFin").val()

        toastr.clear()

        if (!type) {
            toastr.warning("Seleccione un tipo")

            return false
        }

        if (!dateStart) {
            toastr.warning("Ingrese una fecha de Inicio")

            return false
        }

        if (!dateEnd) {
            toastr.warning("Ingrese una fecha Fin.")

            return false
        }

        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "ExcelenciaOperativa/GenerarFisicoFichaEOPdfJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ tipo: type, fechaini: dateStart, fechafin: dateEnd }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                if (response.respuesta) {
                    let data = response.data;
                    let file = response.filename;
                    let a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/pdf;base64, " + data;
                    a.download = file;
                    a.click();
                }
            },
            error: function (request, status, error) {

            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    })

    $(document).on("click", "#btnExcel", function () {
        event.preventDefault()

        //var roomCode = $('#cboSalas').val()
        var type = $("#cboTipo").val()
        var startDate = $("#fechaInicio").val()
        var endDate = $("#fechaFin").val()

        toastr.clear()

        if (!type) {
            toastr.warning("Seleccione un tipo")

            return false
        }

        if (!startDate) {
            toastr.warning("Ingrese una fecha de Inicio")

            return false
        }

        if (!endDate) {
            toastr.warning("Ingrese una fecha Fin.")

            return false
        }

        $.ajax({
            type: "POST",
            cache: false,
            url: `${basePath}ExcelenciaOperativa/ReporteFichaEODescargarExcelJson`,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ types: type, startDate: startDate, endDate: endDate }),
            beforeSend: function ()
            {
                $.LoadingOverlay("show")
            },
            success: function (response) {
                if (response.status)
                {
                    dataAuditoria(1, "#formfiltro", 3, "ExcelenciaOperativa/ReporteFichaEODescargarExcelJson", "BOTON EXCEL")

                    var data = response.data
                    var fileName = response.fileName
                    let a = document.createElement('a')

                    a.target = '_self'
                    a.href = `data:application/vnd.ms-excel;base64,${data}`
                    a.download = fileName
                    a.click()
                }
                else
                {
                    toastr.warning(response.message, "Mensaje Servidor")
                }
            },
            error: function ()
            {
                toastr.error("Error de conexión, Servidor no encontrado")
            },
            complete: function ()
            {
                $.LoadingOverlay("hide")
            }
        })

    })

    VistaAuditoria("ExcelenciaOperativa/FichaReporteVista", "VISTA", 0, "", 3);

    // delete
    $(document).on("click", '.btnEliminar', function (e) {
        event.preventDefault()

        let id = $(this).data("id")

        if (!id) {
            toastr.clear()
            toastr.warning("Seleccione una ficha")

            return false
        }

        if (id) {
            $.confirm({
                icon: 'fa fa-spinner fa-spin',
                title: '¿Esta seguro de Eliminar el registro?',
                theme: 'black',
                animationBounce: 1.5,
                columnClass: 'col-md-6 col-md-offset-3',
                confirmButtonClass: 'btn-info',
                cancelButtonClass: 'btn-warning',
                confirmButton: 'SI',
                cancelButton: 'NO',
                content: false,
                confirm: function () {
                    let dataForm = { id }
                    $.ajax({
                        type: "POST",
                        url: basePath + "ExcelenciaOperativa/EliminarExcelenciaOperativa",
                        cache: false,
                        data: JSON.stringify(dataForm),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: function (xhr) {
                            $.LoadingOverlay("show");
                        },
                        success: function (result) {
                            if (result.respuesta) {
                                toastr.success(result.mensaje, "Mensaje Servidor")
                                buscarFichasExcelenciasOperativas();
                            } else {
                                toastr.error(result.mensaje, "Mensaje Servidor")
                            }
                        },
                        error: function (request, status, error) {
                            toastr.error("Error", "Mensaje Servidor");
                        },
                        complete: function (resul) {
                            $.LoadingOverlay("hide");
                        }
                    });
                },
                cancel: function () {
                }
            });

        }
        else {
            toastr.error("Id Incorrecto", "Mensaje Servidor")
        }
    })

});

function buscarFichasExcelenciasOperativas() {

    //var roomCode = $('#cboSalas').val()
    var listatipo = $("#cboTipo").val();
    var fechaini = $("#fechaInicio").val();
    var fechafin = $("#fechaFin").val();
    var addtabla = $(".contenedor_tabla");

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "ExcelenciaOperativa/FichaEOListarxTipoFechaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ tipo: listatipo, fechaini, fechafin }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            console.log(response)
            response = response.data;

            dataAuditoria(1, "#formfiltro", 3, "ExcelenciaOperativa/FichaEOListarxTipoFechaJson", "BOTON BUSCAR");
            $(addtabla).empty();
            $(addtabla).append('<table id="tablefichas" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#tablefichas").DataTable({
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
                "aaSorting": [],
                data: response,
                columns: [

                    { data: "FichaId", title: "ID" },
                    {
                        data: "Tipo", title: "Tipo", "render": function (value, x, y) {
                            var typeName = "";

                            if (value == 1) {
                                typeName = "Jefe Operativo";
                            }

                            if (value == 2) {
                                typeName = "Gerente de Unidad";
                            }

                            return typeName;
                        }
                    },
                    {
                        data: "Fecha", title: "Fecha", "render": function (data, type, full) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: "PuntuacionBase", title: "Puntuación Base" },
                    { data: "PuntuacionObtenida", title: "Puntuación Obtenida" },
                    {
                        data: "Porcentaje", title: "Porcentaje", "render": function (value, x, y) {
                            return `${(value * 100).toFixed(1)}%`
                        }
                    },
                    { data: "UsuarioNombre", title: "Usuario" },
                    { data: "SalaNombre", title: "Sala" },
                    {
                        data: null, title: "Accion",
                        "render": function (value) {
                            var estado = value.Estado;
                            var buttons = `
                                <button type="button" class="btn btn-xs btn-info btnDetalles" data-json='${JSON.stringify(value)}' data-id="${value.FichaId}" data-hash="${value.hash}" title="Detalle en PDF"><i class="fa fa-file-pdf-o"></i> VER</button>
                                <button type="button" class="btn btn-xs btn-success btnEditar" data-id="${value.FichaId}" data-tipo="${value.Tipo}" title="Modificar"><i class="glyphicon glyphicon-edit"></i></button>
                                <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${value.FichaId}" title="Eliminar"><i class="glyphicon glyphicon-trash"></i></button>
                            `;
                            return buttons;
                        }
                    }
                ],
                columnDefs: [
                    {
                        targets: 3,
                        className: "text-center"
                    },
                    {
                        targets: 4,
                        className: "text-center"
                    },
                    {
                        targets: 5,
                        className: "text-center"
                    }
                ]
                ,
                "initComplete": function (settings, json) {



                },
                "drawCallback": function (settings) {

                }
            });

            $('#tablefichas tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

$(document).on("click", ".btnDetalles", function () {
    var id = $(this).data("id");
    eventBus.$emit('showModalDetalleFicha', id)
});

$(document).on("click", ".btnEditar", function () {
    var id = $(this).data("id");
    var tipo = $(this).data("tipo");

    if (tipo == 1) {
        window.location.replace(basePath + "ExcelenciaOperativa/EditarFichaVistaJOP?FichaId=" + id)
    }

    if (tipo == 2) {
        window.location.replace(basePath + "ExcelenciaOperativa/EditarFichaVistaGU?FichaId=" + id)
    }
});
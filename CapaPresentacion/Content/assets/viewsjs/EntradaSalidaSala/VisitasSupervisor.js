let IdVisitaSupervisor = 0
let arrayEmpleadosSeleccionados = []

$(document).ready(function () {
    ObtenerListaSalas();

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

    $('#OtroMotivo').on('input', function () {
        var maxLengthOtroMotivo = $(this).attr('maxlength');
        var currentLengthOtroMotivo = $(this).val().length;
        $('#charCountOtroMotivo').text(currentLengthOtroMotivo + '/' + maxLengthOtroMotivo);
    })

    $('#Observaciones').on('input', function () {
        var maxLengthObservaciones = $(this).attr('maxlength');
        var currentLengthObservaciones = $(this).val().length;
        $('#charCountObservaciones').text(currentLengthObservaciones + '/' + maxLengthObservaciones);
    })
    $("#btnBuscar").on("click", function () {
        if ($("#cboSala").length == 0 || $("#cboSala").val() == null) {
            toastr.error("Seleccione una sala.")
            return false
        }
        if ($("#fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if ($("#fechaFin").val() == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }
        buscarVisitasSupervisor();
    });
});
toastr.options = {
    "preventDuplicates": true
};


$(document).on('change', '#cboEmpresa', function (e) {
    e.preventDefault()
    empresaSeleccionada = $(this).val()
    //renderTablaEmpleadosSeleccionados(true)
    $('#busqueda').val('')
})

$(document).on('click', '#btnExcel', function (e) {
    e.preventDefault()

    if ($("#cboSala").length == 0 || $("#cboSala").val() == null) {
        toastr.error("Seleccione una sala.")
        return false
    }
    if ($("#fechaInicio").val() === "") {
        toastr.error("Ingrese una fecha de Inicio.")
        return false
    }
    if ($("#fechaFin").val() === "") {
        toastr.error("Ingrese una fecha Fin.")
        return false
    }

    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();
    let dataForm = {
        codsala: listasala,
        fechaini, fechafin
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "VisitasSupervisor/ReporteVisitasSupervisorDescargarExcelJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
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
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
})
$(document).on('click', "#btnNuevo", function (e) {
    e.preventDefault()
    IdVisitaSupervisor = 0;
    limpiarValidadorFormVisitasSupervisor()
    $("#motivoOtros").hide();
    $('#cboSalaVisitasSupervisor').prop('disabled', false);


    $('#textVisitasSupervisor').text('Nuevo')
    $('#IdVisitasSupervisor').val(0)
    $('#cboSalaVisitasSupervisor').val(null)
    $('#cboMotivo').val(null)
    $('#Observaciones').val('')

    $('#FechaIngreso').val(moment().format('DD/MM/YYYY hh:mm A'));
    $('#FechaSalida').val('')
    $('#OtroMotivo').val('')


    renderTablaEmpleadosSeleccionados(true);
    renderSelectSalasModalVisitasSupervisor()
    obtenerListaMotivosPorEstado()


    $('#full-modal_VisitasSupervisor').modal('show')

    var maxLengthDescripcion = $('#Descripcion').attr('maxlength');
    $('#charCountDescripcion').text(0 + '/' + 300);
    var maxLengthDescripcionMotivo = $('#OtroMotivo').attr('maxlength');
    $('#charCountDescripcionMotivo').text(0 + '/' + 100);
    var maxLengthOObservaciones = $('#Observaciones').attr('maxlength');
    $('#charCountObservaciones').text(0 + '/' + 100);

});
$(document).on('click', '#eliminarfechasalida', function (e) {
    e.preventDefault()
    $('#FechaSalida').val('')
})

$("#full-modal_VisitasSupervisor").on("shown.bs.modal", function (e) {
    //renderTablaEmpleadosSeleccionados()
    $("#FechaIngreso").datetimepicker({
        pickDate: true,
        format: 'DD/MM/YYYY HH:mm A',
        pickTime: true
    })
    $("#FechaSalida").datetimepicker({
        pickDate: true,
        format: 'DD/MM/YYYY HH:mm A',
        pickTime: true
    })
    initAutocomplete()
})
//$("#full-modal_VisitasSupervisor").on("hidden.bs.modal", function () {
//    arrayEmpleadosSeleccionados = [];
//});


$(document).on("click", ".btnEditarVisitasSupervisor", function () {
    limpiarValidadorFormVisitasSupervisor();
    IdVisitaSupervisor = $(this).data("id");

    let rowData = objetodatatable
        .rows()
        .data()
        .toArray()
        .find(row => row.IdVisitaSupervisor === IdVisitaSupervisor);

    if (rowData) {
        $('#textVisitasSupervisor').text('Editar');
        $('#busqueda').val('')


        arrayEmpleadosSeleccionados = [...rowData.Empleados];
        renderTablaEmpleadosSeleccionados();

        $('#cboSalaVisitasSupervisor').prop('disabled', true);
        renderSelectSalasModalVisitasSupervisor(rowData.CodSala);
        obtenerListaMotivosPorEstado(rowData.IdMotivo);
        $('#FechaIngreso').val((moment(rowData.FechaIngreso).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.FechaIngreso).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.FechaIngreso).format('DD/MM/YYYY hh:mm A'));
        $('#FechaSalida').val((moment(rowData.FechaSalida).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.FechaSalida).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.FechaSalida).format('DD/MM/YYYY hh:mm A'));
        $('#OtroMotivo').val(rowData.OtroMotivo);
        var currentLengthOtroMotivo = $('#OtroMotivo').val().length;
        var maxLengthOtroMotivo = $('#OtroMotivo').attr('maxlength');
        $('#charCountOtroMotivo').text(currentLengthOtroMotivo + '/' + maxLengthOtroMotivo);

        $('#Observaciones').val(rowData.Observaciones);
        var currentLengthObservaciones = $('#Observaciones').val().length;
        var maxLengthObservaciones = $('#Observaciones').attr('maxlength');
        $('#charCountObservaciones').text(currentLengthObservaciones + '/' + maxLengthObservaciones);


        $('.modal').modal('hide');
        $('#full-modal_VisitasSupervisor').modal('show');

    } else {
        toastr.error("No se encontraron datos para este registro.", "Error");
    }
});

$("#cboSala").on("change", function () {

    var salas = $("#cboSala").val();

    if (salas) {

        if (salas.length > 1) {

            if ((salas.indexOf("-1") > -1)) {
                $("#cboSala").val("-1").trigger("change");
            }
        }

    }
});

$(document).on('change', '#cboMotivo', function (e) {
    let valorSeleccionado = $(this).val()
    if (valorSeleccionado == -1) {
        $("#motivoOtros").show();
        $('#visitasSupervirsorForm').bootstrapValidator('enableFieldValidators', 'OtroMotivo', true);
        $('#visitasSupervirsorForm').bootstrapValidator('updateMessage', 'OtroMotivo', 'notEmpty', '');

    }
    else {
        $("#motivoOtros").hide();
        $('#OtroMotivo').val('')
        $('#visitasSupervirsorForm').bootstrapValidator('enableFieldValidators', 'OtroMotivo', false);
    }
})

$("#visitasSupervirsorForm")
    .bootstrapValidator({
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            CodSala: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            IdMotivo: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            IdMotivo: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            OtroMotivo: {
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
            }
        }
    }).on('success.field.bv', function (e, data) {
        e.preventDefault();
        var $parent = data.element.parents('.form-group');
        $parent.removeClass('has-success');
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });

function buscarVisitasSupervisor() {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();
    let addtabla = $("#contenedor_tabla");

    $.ajax({
        type: "POST",
        url: basePath + "VisitasSupervisor/VisitasSupervisorListarxSalaFechaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            let datos = response.data || [];
            $(addtabla).empty();
            $(addtabla).append('<table id="tableResultado" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');

            objetodatatable = $("#tableResultado").DataTable({
                destroy: true,
                sort: true,
                scrollCollapse: true,
                scrollX: true,
                paging: true,
                autoWidth: true,
                data: datos,
                aaSorting: [[0, 'desc']],
                columns: [
                    { data: "IdVisitaSupervisor", title: "ID" },
                    { data: "NombreSala", title: "Sala" },
                    {
                        data: "FechaRegistro",
                        title: "Fecha",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        data: null,
                        title: "Motivo",
                        render: function (data, type, row) {
                            if (data.Nombre && data.Nombre.trim() !== "") {
                                return data.Nombre;
                            } else {
                                if (data.IdMotivo == -1) {
                                    const otroMotivo = data.OtroMotivo.length > 27 ? data.OtroMotivo.substring(0, 27) + "..." : data.OtroMotivo;
                                    return `Otro (${otroMotivo})`;
                                } else {
                                    return "";
                                }
                            }
                        }
                    },

                    {
                        data: "FechaIngreso",
                        title: "Ingreso",
                        render: function (data) {
                            return moment(data).format('HH:mm A');
                        }
                    },
                    {
                        data: "FechaSalida",
                        title: "Salida",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY') == "01/01/1753" ? "" : moment(data).format('DD/MM/YYYY') == "31/12/1752" ? '<span class="label btn-warning">No definido</span>' : moment(data).format('HH:mm A');
                        }
                    },
                  /*  { data: "Observaciones", title: "Observaciones" },*/
                    {
                        data: null,
                        title: "Observaciones",
                        render: function (data, type, row) {
                            return data.Observaciones.length > 27 ? data.Observaciones.substring(0, 27) + "..." : data.Observaciones;
                        }
                    },
                    {
                        data: "IdVisitaSupervisor",
                        title: "Accion",
                        "render": function (o) {
                            return `
                                            <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-warning btnEditarVisitasSupervisor" data-id="${o}">
                                                <i class="glyphicon glyphicon-pencil"></i>
                                            </button> 
                                            <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-danger btnEliminarRegistro"  data-id="${o}">
                                                <i class="glyphicon glyphicon-remove"></i>
                                            </button>`
                        },
                        className: "text-center",
                        "orderable": false
                    }
                ],
                "drawCallback": function (settings) {
                    $('.btnEditarVisitasSupervisor').tooltip({
                        title: "Editar Registro"
                    });
                    $('.btnEliminarRegistro').tooltip({
                        title: "Eliminar Registro"
                    });
                },
            });
        },
        error: function (request, status, error) {
            toastr.error("Error en la solicitud.", "Mensaje Servidor");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}

$(document).on("click", ".btnEliminarRegistro", function () {
    const $this = $(this);
    var id = $this.data("id");
    $.confirm({
        title: '¿Estás seguro de Continuar?',
        content: '¿Eliminar registro?',
        confirmButton: 'Ok',
        cancelButton: 'Cerrar',
        confirmButtonClass: 'btn-success',
        cancelButtonClass: 'btn-danger',
        confirm: () => {
            deleteVisitasSupervisor(id)
        }
    });
});

const deleteVisitasSupervisor = (idregistro) => {
    const data = {
        id: idregistro
    };
    $.ajax({
        url: `${basePath}VisitasSupervisor/EliminarVisitasSupervisor`,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            if (!response.respuesta) {
                toastr.error(response.mensaje, "Mensaje Servidor");
                return;
            }
            toastr.success(response.mensaje, "Mensaje Servidor");
            buscarVisitasSupervisor();
        }
    });
}
$(document).on('click', '#btnGuardarVisitasSupervisor', function (e) {

    e.preventDefault()
    $("#visitasSupervirsorForm").data('bootstrapValidator').resetForm();
    var validarRegistro = $("#visitasSupervirsorForm").data('bootstrapValidator').validate();

    if (validarRegistro.isValid()) {


        let url = basePath
        if (IdVisitaSupervisor === 0) {
            url += "VisitasSupervisor/GuardarVisitasSupervisor"
        } else {
            url += "VisitasSupervisor/EditarVisitasSupervisor"

        }
        console.log(IdVisitaSupervisor)
        let dataForm = new FormData(document.getElementById("visitasSupervirsorForm"))

        dataForm.delete('IdVisitaSupervisor');
        dataForm.append('IdVisitaSupervisor', IdVisitaSupervisor);

        dataForm.append('CodSala', $('#cboSalaVisitasSupervisor option:selected').val())
        dataForm.append('IdMotivo', $('#cboMotivo option:selected').val())

        dataForm.append('Empleados', JSON.stringify(arrayEmpleadosSeleccionados))
        $.ajax({
            url: url,
            type: "POST",
            method: "POST",
            contentType: false,
            data: dataForm,
            cache: false,
            processData: false,
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                if (response.respuesta) {
                    toastr.success(response.mensaje, "Mensaje Servidor")
                    $("#full-modal_VisitasSupervisor").modal("hide");
                    buscarVisitasSupervisor();
                } else {
                    toastr.error(response.mensaje, "Mensaje Servidor")
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
                toastr.error("Ocurrió un error al guardar el registro.", "Error");
            }
        });
    }


})

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
            $("#cboSala").html('')
            const codSalaArray = result.data.map(sala => String(sala.CodSala));

            if (result.data) {
                arraySalas = result.data
                arraySelect = [`<option value="${-1}" selected>TODOS</option>`]
                result.data.map(item => arraySelect.push(`<option value="${item.CodSala}">${item.Nombre}</option>`))
                $("#cboSala").html(arraySelect.join(""))
                $("#cboSala").select2({
                    multiple: true, placeholder: "--Seleccione--", allowClear: true
                });
                $("#cboSala").val(-1).trigger("change")
            }
            buscarVisitasSupervisor();
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
function renderSelectSalasModalVisitasSupervisor(value) {
    $("#cboSalaVisitasSupervisor").html('')
    if (arraySalas) {
        $("#cboSalaVisitasSupervisor").html(arraySalas.map(item => `<option value="${item.CodSala}">${item.Nombre}</option>`).join(""))
        if (value) {
            $('#cboSalaVisitasSupervisor').val(value);
        } else {
            $("#cboSalaVisitasSupervisor").val(null).trigger("change");
        }
        $("#cboSalaVisitasSupervisor").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_VisitasSupervisor')
        });
    }
}
function obtenerListaEmpresas(value) {
    $.ajax({
        type: "POST",
        url: basePath + "EquivalenciaEmpresa/ListarTodasLasEquivalenciasEmpresaJSON",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboEmpresa").html('')
            if (result.data) {
                $('#cboEmpresa').append('<option></option>')
                $("#cboEmpresa").append(result.data.map(item => `<option value="${item.IdEmpresaBuk}">${item.Nombre}</option>`).join(""))

                $("#cboEmpresa").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_VisitasSupervisor')
                });
                if (value) {
                    shouldTriggerChange = false;
                    $("#cboEmpresa").val(value).trigger('change')
                }
            }
            limpiarValidadorFormVisitasSupervisor();
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    });
    return false;
}


function initAutocomplete() {
    $("#busqueda").autocomplete({
        source: function (request, response) {

            $.ajax({
                url: basePath + "BUKEmpleado/ObtenerEmpleadosActivosPorTerminoxCargo",
                dataType: "json",
                traditional: true,
                data: {
                    term: request.term,
                    idcargo: [181, 142, 79, 81]
                },
                success: function (json) {
                    var results = $.map(json.data, function (item) {
                        return {
                            label: item.NombreCompleto + " - " + item.NumeroDocumento,
                            value: item.NombreCompleto,
                            fullData: item
                        };
                    });
                    response(results);
                },
                error: function () {
                    response([]);
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            let existEmpleado = arrayEmpleadosSeleccionados.find(x => x.IdEmpleado == ui.item.fullData.IdBuk)

            if (!existEmpleado) {
                arrayEmpleadosSeleccionados.push({
                    IdEmpleado: ui.item.fullData.IdBuk,
                    Nombre: ui.item.fullData.Nombres,
                    ApellidoPaterno: ui.item.fullData.ApellidoPaterno,
                    ApellidoMaterno: ui.item.fullData.ApellidoMaterno,
                    TipoDocumento: ui.item.fullData.TipoDocumento,
                    DocumentoRegistro: ui.item.fullData.NumeroDocumento,
                    Cargo: ui.item.fullData.Cargo,
                    IdCargo: ui.item.fullData.IdCargo,
                    Empresa: ui.item.fullData.Empresa,
                    IdEmpresa: ui.item.fullData.IdEmpresa
                })
            } else {
                toastr.warning("Empleado ya seleccionado", "Advertencia")
            }

            renderTablaEmpleadosSeleccionados()
            $('#busqueda').val('')
        },
        open: function (event, ui) {
            $('.ui-autocomplete').appendTo('#AutoCompleteContainer')
            $('.ui-autocomplete').css('z-index', '9999');
            $('.ui-autocomplete').css('position', 'absolute');
            $('.ui-autocomplete').css('top', '');
            $('.ui-autocomplete').css('left', '0');
            $('.ui-autocomplete').css('width', '100%');
        },
        close: function (event, ui) {
            $('#busqueda').val('')
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
            .append("<div>" + item.label + "<br><small>" + item.fullData.Cargo + " - " + item.fullData.Empresa + "</small></div>")
            .appendTo(ul);
    };
}

function renderTablaEmpleadosSeleccionados(value) {
    let element = $('#contenedorTableEmpleadosSeleccionados')
    element.empty()
    if (value) {
        arrayEmpleadosSeleccionados = [];
    }

    let tbody = arrayEmpleadosSeleccionados ? arrayEmpleadosSeleccionados.map(item => `
        <tr>
            <td>${item.Nombre} ${item.ApellidoPaterno} ${item.ApellidoMaterno}</td>
            <td>${item.TipoDocumento.toUpperCase()} - ${item.DocumentoRegistro}</td>
            <td>${item.Cargo}</td>
            <td><a class="btn btn-sm btn-danger quitarEmpleado" data-id="${item.IdEmpleado}">Quitar <span class="glyphicon glyphicon-remove"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered" style="width:100%" id="tableEmpleadosSeleccionadosBienMaterial">
            <thead>
                <tr>
                    <th>Nombres</th>
                    <th>Nro Documento</th>
                    <th>Cargo</th>
                    <th>Acción</th>
                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4" align="center">Agregue un empleado</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)

}

$(document).on('click', '.quitarEmpleado', function (e) {
    e.preventDefault()
    let idSeleccionado = $(this).data('id')
    arrayEmpleadosSeleccionados = arrayEmpleadosSeleccionados.filter(x => x.IdEmpleado != idSeleccionado)
    renderTablaEmpleadosSeleccionados();

})

function limpiarValidadorFormVisitasSupervisor() {
    $("#visitasSupervirsorForm").parent().find('div').removeClass("has-error");
    $("#visitasSupervirsorForm").parent().find('div').removeClass("has-success");
    $("#visitasSupervirsorForm").parent().find('i').removeAttr("style").hide();

}

function obtenerListaMotivosPorEstado(value) {
    $.ajax({
        type: "POST",
        url: basePath + "VisitasSupervisor/ListarMotivoPorEstado",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboMotivo").html('')
            if (result.data) {
                $('#cboMotivo').append('<option></option>')
                $("#cboMotivo").append(result.data.map(item => `<option value="${item.IdMotivo}">${item.Nombre}</option>`).join(""))
                $('#cboMotivo').append('<option value="-1">OTROS</option>')
                $("#cboMotivo").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_VisitasSupervisor')
                });
                if (value) {
                    $("#cboMotivo").val(value).trigger("change");
                }
            }
            limpiarValidadorFormVisitasSupervisor();

        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    });
    return false;
}

$('#eliminarfechasalida').tooltip({
    title: "Limpiar Fecha Salida"
});  
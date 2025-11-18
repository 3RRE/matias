let objetodatatable
let objetodatatable2
let arraySalas = []
let maquinasEnTabla = [];
let datosmaquinas = [];
let accionValor

let id=0
let idsala =0
$(document).ready(function () {
    ObtenerListaSalas();

    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        //maxDate: dateNow,
    });

    $("#Fecha").datetimepicker({
        format: 'DD/MM/YYYY hh:mm A',
        defaultDate: moment(),
        pickTime: true
    })

    $(".dateOnly_fechaini").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        //minDate: dateMin,
    });

    $(".dateOnly_fechafin").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        //maxDate: dateNow,
    });


    setCookie("datainicial", "");
    $("#btnBuscar").on("click", function () {


        if ($("#fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if ($("#fechaFin").val() == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }
        buscarMaquina();

    });

    $(document).on("click", "#btnExcel", function () {
        var fechaini = $("#fechaInicio").val();
        var fechafin = $("#fechaFin").val();
        var sala = $("#cboSala").val();


        var listasala = [];
        $("#cboSala option:selected").each(function () {
            listasala.push($(this).data("id"));
        });
        /* var listasala = $("#cboSala").val();*/

        let ids = []
        if (objetodatatable) {
            let selectedRows = objetodatatable.rows({ search: 'applied' })
                .data();
            selectedRows.map((item) => {
                ids.push(item.id)
            })
        }
        if (ids.length > 0) {
            $.ajax({
                type: "POST",
                cache: false,
                //url: basePath + "EstadoMaquina/ReporteEstadoMaquinasDescargarExcelJson",
                url: basePath + "EstadoMaquina/ReporteEstadoMaquinasDescargarExcelJson",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({
                    codsala: sala, fechaini, fechafin
                }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (response) {
                    if (response.respuesta) {
                        dataAuditoria(1, "#formfiltro", 3, "EstadoMaquina/ReporteEstadoMaquinasDescargarExcelJson", "BOTON EXCEL");
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
        }

    });

    //VistaAuditoria("EstadoMaquina/ListaMaquinaxSalaJson", "VISTA", 0, "", 3);

    VistaAuditoria("Reclamacion/ReclamacionListarVista", "VISTA", 0, "", 3);

});

function buscarMaquina() {
    var listasala = $("#cboSala").val();
    var fechaini = $("#fechaInicio").val();
    var fechafin = $("#fechaFin").val();
    var addtabla = $("#contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "EstadoMaquina/ListaMaquinaxSalaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            jQuery.fn.dataTable.ext.type.order['date-dd-mm-yyyy-pre'] = function (data) {
                if (!data) return 0;
                let dateParts = data.split('/');
                return new Date(dateParts[2], dateParts[1] - 1, dateParts[0]).getTime();
            };
            response = response.data;
            dataAuditoria(1, "#formfiltro", 3, "EstadoMaquina/ListaMaquinaxSalaJson", "BOTON BUSCAR");
            $(addtabla).empty();
            $(addtabla).append('<table id="tablemaquinas" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#tablemaquinas").DataTable({
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
                "aaSorting": [[7, "desc"], [1, "asc"]],
                //"columnDefs": [{ 'targets': 7, type: 'date-euro' }],
                //"order": [7, 'desc'],
                data: response.lista,
                columns: [
                    { data: "id", title: "ID" },
                    { data: "sala", title: "Sala" },
                    { data: "CantMaquinaConectada", title: "Maq Conectadas" },
                    { data: "CantMaquinaNoConectada", title: "Maq No Conectadas" },
                    { data: "CantMaquinaPLay", title: "Maq PLAY" },
                    { data: "CantMaquinaRetiroTemporal", title: "Maq Ret.Temp."},
                    { data: "TotalMaquina", title: "Total" },
                    {
                        data: "FechaOperacion", title: "F. Operacion",
                        "render": function (data) {
                            if (moment(data).format('DD/MM/YYYY') == "01/01/1753" || moment(data).format('DD/MM/YYYY') == "31/12/1752") return moment(data).format('DD/MM/YYYY');
                            return moment(data).format('DD/MM/YYYY');
                        },
                        "type": "date-dd-mm-yyyy"
                    },
                    {
                        data: "FechaCierre", title: "F. Cierre",
                        "render": function (data) { 
                            return (moment(data).format('DD/MM/YYYY') == "01/01/1753" || moment(data).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(data).format('DD/MM/YYYY hh:mm A');
                        }
                    },
                    {
                        data: "Accion",
                        title: "Acción",
                        "render": function (data, type, row) {
                            let buttonClass = data === 1 ? "btn-warning" : "btn-success";
                            let buttonText = data === 1 ? "Agregar" : "Ver detalle";
                            return ` 
                            <button type="button" class="btn btn-xs ${buttonClass} btnRetiroTemporal" data-id="${row.id}"  data-tooltip="${data === 1 ? "Registrar Retiro Temporal" : "Ver Retiro Temporal"}">
                                <span>${buttonText}</span>
                            </button>`
                        },
                        className: "text-center",
                        "orderable": false
                    },
                ],
                "initComplete": function (settings, json) {
                },
                "drawCallback": function (settings) {
                    if ($("#tablemaquinas tbody .total-row").length === 0) {
                        $("#tablemaquinas tbody").append(
                            '<tr class="total-row">' +
                            '<td colspan="2" style="text-align:center;">TOTAL</td>' +
                            '<td>' + response.consolidado.TotalConectadas + '</td>' +
                            '<td>' + response.consolidado.TotalDesconectadas + '</td>' +
                            '<td>' + response.consolidado.TotalMaquinaPLay + '</td>' +
                            '<td>' + response.consolidado.TotalRetiroTemporal + '</td>' + 
                            '<td>' + response.consolidado.TotalMaquinas + '</td>' +
                            '<td></td>' +
                            '<td></td>' +
                            '<td></td>' +
                            '</tr>'
                        );
                        $(".total-row").css({
                            "background-color": "#0b1f46",
                            "color": "white"
                        });
                    }
                    $('.btnRetiroTemporal').each(function () {
                        let tooltipText = $(this).data('tooltip');
                        $(this).tooltip({
                            title: tooltipText,
                        });
                    });
                }
            });

            $("#tbodyconsolidado").html("");
            $("#tbodyconsolidado").html('<tr><td>' + response.consolidado.TotalConectadas + '</td><td>' + response.consolidado.TotalDesconectadas + '</td><td>' + response.consolidado.TotalMaquinaPLay + '</td><td>' + response.consolidado.TotalMaquinas + '</td></tr>')

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

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
            buscarMaquina();
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
function renderSelectSalasModalEstadoMaquina(value) {
    $("#cboSalaEstadoMaquina").html('')
    if (arraySalas) {
        $("#cboSalaEstadoMaquina").html(arraySalas.map(item => `<option value="${item.CodSala}">${item.Nombre}</option>`).join(""))
        if (value) {
            $('#cboSalaEstadoMaquina').val(value);
        } else {
            $("#cboSalaEstadoMaquina").val(null).trigger("change");
        }
        $("#cboSalaEstadoMaquina").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_RetiroTemporal')
        });
    //    limpiarValidadorFormAccionCajaTemporizada()
    }
}

$(document).on("click", ".btnRetiroTemporal", function () {
  $('#full-modal_RetiroTemporal').modal('show');
        
    id = $(this).data("id");
    maquinasEnTabla = [];
    let rowData = objetodatatable
        .rows()
        .data()
        .toArray()
        .find(row => row.id === id); 

    if (rowData) { 
        $('#NombreSala').val(rowData.sala);
        idsala = rowData.sala_id;
        ListarMaquinasPorIdSala(rowData.sala_id);
        $('#FechaOperacion').val((moment(rowData.FechaOperacion).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.FechaOperacion).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.FechaOperacion).format('DD/MM/YYYY'));

        if (rowData.Accion === 1) {
            $('#formFieldset').show()
        } else {
            $('#formFieldset').hide()
        }
    }
    accionValor = rowData.Accion
    obtenerListaRetiroTemporal(rowData.id)
})


$("#full-modal_RetiroTemporal").on("shown.bs.modal", function () {
 
    limpiarValidadorformRetiroTemporal() 
})
 

$(document).on('click', '#btnCancelarRetiroTemporal', function (e) {
    e.preventDefault()
    $("#CodMaquina").val('')
})

$(document).on('click', '#btnGuardarRetiroTemporal', function (e) {
    e.preventDefault();

    let nuevoCodMaquina = $('#CodMaquina').val();
    let existe = false;

    if (!nuevoCodMaquina || nuevoCodMaquina.trim() === "") {
        toastr.warning("El código de máquina no puede estar vacío.", "Mensaje Servidor");
        return;  
    }
    $('#tableListadoRetiroTemporal tbody tr').each(function () {
        let codMaquinaExistente = $(this).find('td').eq(1).text().trim(); 
        if (codMaquinaExistente === nuevoCodMaquina) {
            existe = true;
            return false; 
        }
    });

    if (existe) {
        toastr.warning("El código de máquina ya existe en el listado.", "Mensaje Servidor");
        $("#CodMaquina").val(null).trigger("change");

        return; 
    }

    let dataForm = {
        IdEstadoMaquina: id,
        CodMaquina: nuevoCodMaquina,
        NombreSala: $('#NombreSala').val(),
        CodSala: idsala,
        Fecha: $('#Fecha').val()
    };

    $("#formRetiroTemporal").data('bootstrapValidator').resetForm();
    var validarRegistro = $("#formRetiroTemporal").data('bootstrapValidator').validate();
    if (validarRegistro.isValid()) {
        let url = `${basePath}EstadoMaquina/InsertarRetiroTemporal`; 
        
        $.ajax({
            url: url,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(dataForm),
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
                $("#CodMaquina").val(null).trigger("change");

            },
            success: function (response) {
                if (response.respuesta) {
                    $("#IdEstadoMaquinaDetalle").val(0);
                    $("#CodMaquina").val(null).trigger("change");

                    limpiarValidadorformRetiroTemporal();
                    obtenerListaRetiroTemporal(id);
                    buscarMaquina();
                    toastr.success(response.mensaje, "Mensaje Servidor");
                } else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        });
    }
});



$('#formRetiroTemporal').bootstrapValidator({
    feedbackIcons: {
        valid: 'glyphicon glyphicon-ok',
        invalid: 'glyphicon glyphicon-remove',
        validating: 'glyphicon glyphicon-refresh'
    },
    fields: {
        //CodMaquina: {
        //    validators: {
        //        notEmpty: {
        //            message: ' '
        //        }
        //    }
        //},
    }
}).on('success.field.bv', function (e, data) {
    e.preventDefault();
    var $parent = data.element.parents('.form-group');
    $parent.removeClass('has-success');
    $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
});


function obtenerListaRetiroTemporal(id) {
    $.ajax({
        type: "POST",
        url: basePath + "EstadoMaquina/ListarRetiroTemporal",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ IdEstadoMaquina: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.data) {
                renderTablaRetiroTemporal(result.data) 
                renderCboMaquinasLey();
            }
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
function renderTablaRetiroTemporal(data) {
    let element = $('#contenedorRetiroTemporal')
    element.empty()
    maquinasEnTabla = []
    if (data) {
        data.forEach(function (item) {
            maquinasEnTabla.push(item.CodMaquina);
        });
    }

    let tbody = data ? data.map(item => `
        <tr>
            <td>${item.IdEstadoMaquinaDetalle}</td>
            <td>${item.CodMaquina}</td>
            <td>
                <span style="display:none">${item.FechaRegistro}</span>
                ${moment(item.FechaRegistro).format('DD/MM/YYYY hh:mm A')}
            </td>

           ${accionValor === 1 ?
        `<td><a class="btn btn-sm btn-danger quitarRetiroTemporal" data-id="${item.IdEstadoMaquinaDetalle}" data-codmaquina="${item.CodMaquina}" style="height: 100%">Quitar<span class="glyphicon glyphicon-remove"></span></a></td>`
        : ''}

        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered table-striped" style="width:100%" id="tableListadoRetiroTemporal">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Cod.Maquina</th>
                    <th>Fecha Reg.</th>
                    ${accionValor === 1 ?
                    `<th>Acción</th>`
                     : ''}

                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="${accionValor === 1 ? 4 : 3}" style="text-align: center; vertical-align: middle;">${accionValor === 1 ? "Agregue un registro" : "No hay registro"}</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)
    if (tbody.length > 0) {
        $("#tableListadoRetiroTemporal").DataTable({
            aaSorting: [[0, 'desc']],
        })
    }
}

$(document).on('click', '.quitarRetiroTemporal', function (e) {
    e.preventDefault();

    let idEstadoMaquinaDetalle = $(this).data('id');
    let codMaquina = $(this).data('codmaquina');

        $.confirm({
            title: '¿Estás seguro de Continuar?',
            content: '¿Eliminar registro?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function() {

                $.ajax({
                    url: basePath + "EstadoMaquina/QuitarRetiroTemporal",
                    type: 'POST',
                    contentType: "application/json",
                    data: JSON.stringify({ idEstadoMaquinaDetalle: idEstadoMaquinaDetalle, idEstadoMaquina: id, codMaquina }), 
                    success: function (response) {
                        if (response.respuesta) {
                            obtenerListaRetiroTemporal(id);
                            buscarMaquina();
                            toastr.success(response.mensaje, "Mensaje Servidor");
                        } else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }
                       
                    },
                    error: function (error) {
                        console.error("Error al eliminar el retiro temporal", error);
                    }
                });
            }
        });
});


function limpiarValidadorformRetiroTemporal() {
    $("#formRetiroTemporal").parent().find('div').removeClass("has-error");
    $("#formRetiroTemporal").parent().find('div').removeClass("has-success");
    $("#formRetiroTemporal").parent().find('i').removeAttr("style").hide();
}
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
const ListarMaquinasPorIdSala = (codigoSala) => {
    //console.log(codigoSala)
    $.ajax({
        type: "POST",
        url: basePath + "MIMaquinaInoperativa/ListarMaquinasAdministrativoxSala",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ cod: codigoSala }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            if (datos) {  
                datosmaquinas = datos;
                renderCboMaquinasLey();
               
                $("#CodMaquina").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_RetiroTemporal')
                });
                 
            }
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
function renderCboMaquinasLey() {
    const opciones = datosmaquinas.filter(item => $.trim(item.CodMaquinaLey) != "" && !maquinasEnTabla.includes(item.CodMaquinaLey)).map(item =>
        `<option value="${item.CodMaquinaLey}">${item.CodMaquinaLey}</option>`
    ).join("");
    $("#CodMaquina").html(opciones);
    $("#CodMaquina").val(null).trigger("change");
}
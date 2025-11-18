let all;
var registrosCreados = false;

$(document).ready(function () {
    ObtenerListaSalas()
})


toastr.options = {
    "preventDuplicates": true
};


$(document).on('keypress', '.validadornumero', function (event) {


    var regex = new RegExp("^[0-9]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
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
$("#btnBuscar").on("click", function () {

    buscarReporteRegistroMaquina();

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
            arraySalas = datos;
            var todosCodSala = datos.map(function (item) { return item.CodSala; });
            all = todosCodSala;

            $("#cboSala").append('<option value="-1">--TODOS--</option>');
            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                multiple: true,
                placeholder: "--Seleccione--",
                allowClear: true
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


function buscarReporteRegistroMaquina() {
    var listasala = $("#cboSala").val();
    var addtabla = $("#contenedor_tabla");

    if (!listasala || listasala.length === 0) {
        toastr.warning("Seleccione al menos una sala para buscar.", "Aviso");
        return;
    }

    if ((listasala.indexOf("-1") > -1)) {
        listasala = all;
    }

    $.ajax({
        type: "POST",
        url: basePath + "EstadoMaquina/ListaReporteRegistroMaquinaxSalaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala }),

        beforeSend: function () {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            if (response && response.data && response.data.length > 0) {
                
                var data = response.data;

                $(addtabla).empty();
                $(addtabla).append('<table id="tablemaquinas" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
                var table = $("#tablemaquinas").DataTable({
                    bDestroy: true,
                    bSort: true,
                    scrollCollapse: true,
                    scrollX: false,
                    sScrollX: "100%",
                    paging: true,
                    autoWidth: false,
                    bAutoWidth: true,
                    bProcessing: true,
                    bDeferRender: true,
                    aaSorting: [[0, 'desc']],
                    data: data,
                    columns: [
                        { data: "IdRegistroMaquina", title: "ID" },
                        { data: "NombreSala", title: "Sala" },
                        {
                            data: "CodMaquinaINDECI",
                            title: "Máquinas INDECI",
                            render: function (value, type, oData) {
                                return `<input type="text" class="editable-input validadornumero" value="${value}" data-id="${oData.IdRegistroMaquina}" data-column="CodMaquinaINDECI" data-codsala="${oData.CodSala}" data-codmaquinard="${oData.CodMaquinaRD}" />`;
                            }
                        },
                        {
                            data: "CodMaquinaRD",
                            title: "Máquinas RD",
                            render: function (value, type, oData) {
                                return `<input type="text" class="editable-input validadornumero" value="${value}" data-id="${oData.IdRegistroMaquina}" data-column="CodMaquinaRD" data-codsala="${oData.CodSala}" data-codmaquinaindeci="${oData.CodMaquinaINDECI}" />`;
                            }
                        },

                        { data: "TotalMaquina", title: "Total de Máquinas", className: "total-maquina" },
                       
                    ],
                });

                $('#tablemaquinas tbody').on('blur', '.editable-input', function () {
                    var input = $(this);
                    var newValue = input.val().trim();

                    if (newValue === "") {
                        newValue = "0";
                    }

                    input.val(newValue);

                    var dataActualizar = {
                        IdRegistroMaquina: input.data('id'),
                        CodSala: input.data('codsala'),
                        CodMaquinaINDECI: (input.data('column') === 'CodMaquinaINDECI') ? newValue : input.data('codmaquinaindeci'),
                        CodMaquinaRD: (input.data('column') === 'CodMaquinaRD') ? newValue : input.data('codmaquinard')
                    };

                    if (dataActualizar.CodMaquinaINDECI !== input.data('codmaquinaindeci')) {
                        actualizarRegistroMaquina(dataActualizar,"INDECI");
                    }
                    if (dataActualizar.CodMaquinaRD !== input.data('codmaquinard')) {
                        actualizarRegistroMaquina(dataActualizar,"RD");
                    }
                });

                $('#tablemaquinas tbody').on('keypress', '.editable-input', function (e) {
                    if (e.which === 13) {
                        $(this).blur();
                    }
                });

                $('#tablemaquinas tbody').on('click', 'td.total-maquina', function () {
                    var table = $('#tablemaquinas').DataTable();
                    var row = $(this).closest('tr');
                    var rowData = table.row(row).data();

                    //console.log("Datos de la fila:", rowData);

                    if (!rowData) {
                        toastr.error("No se pudo obtener los datos de la fila.", "Error");
                        return;
                    }

                    var codSala = rowData.CodSala;

                    if (codSala !== undefined && codSala !== null) {
                        $("#full-modal_totalmaquina").modal('show');
                        $("#full-modal_totalmaquina").one("shown.bs.modal", function () {
                            buscarReporteEstadoMaquina(codSala);
                        });
                    } else {
                        toastr.warning("No se encontró el código de sala en la fila seleccionada.", "Aviso");
                    }
                });
            }
        },
        error: function (xhr, status, error) {
            console.error("Error al cargar los datos:", xhr.responseText);
            toastr.error("Error al cargar los datos de las salas.", "Error");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
    });
}


function actualizarRegistroMaquina(dataActualizar, tiporegistro) {
    dataActualizar.TipoRegistroMaquina = tiporegistro;
    $.ajax({
        url: basePath + "EstadoMaquina/ActualizarRegistroMaquina",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataActualizar),
        beforeSend: function () {
            //$.LoadingOverlay("show");
        },
        success: function (response) {
            //$.LoadingOverlay("hide");
            if (response.respuesta) {
                toastr.success("Datos actualizados correctamente.", "Éxito"); 
            } else {
                toastr.error(response.mensaje, "Error");
                buscarReporteRegistroMaquina();  
            }
        },
        error: function (xhr, status, error) {
            $.LoadingOverlay("hide");
            toastr.error("Error al actualizar los datos.", "Error");
            buscarReporteRegistroMaquina(); 
        }
    });
}


function crearSalaRegistroMaquina(salas) {
    $.ajax({
        type: "POST",
        url: basePath + "EstadoMaquina/CrearSalaRegistroMaquina", 
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ salas: salas }), 
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if (response.respuesta) {
                buscarReporteRegistroMaquina();  
            } else {
                toastr.error(response.mensaje, "Error");
            }
        },
        error: function (xhr, status, error) {
            console.error("Error al insertar los registros:", xhr.responseText);
            toastr.error("Error al insertar los registros de las salas.", "Error");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}



$(document).on('click', '#btnExcel', function (e) {
    e.preventDefault()
    if ($("#cboSala").length == 0 || $("#cboSala").val() == null) {
        toastr.error("Seleccione una sala.")
        return false
    }
   

    let listasala = $("#cboSala").val();
   
    let dataForm = {
        codsala: listasala
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "EstadoMaquina/ExcelReporteRegistroMaquinaxSalaJson",
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

function buscarReporteEstadoMaquina(codSala) {
    var addtabla = $("#contenedor_tabla_");


    $.ajax({
        type: "POST",
        url: basePath + "EstadoMaquina/ListaMaquinaxSalaxJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            codsala: [codSala],
        }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            response = response.data;

            $(addtabla).empty();
            $(addtabla).append('<table id="tablemaquinas_" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');

            $("#tablemaquinas_").DataTable({
                "bDestroy": true,
                "data": response.lista,
                aaSorting: [[0, 'desc']],
                "columns": [
                    { data: "id", title: "ID" },
                    { data: "sala", title: "Sala" },
                    { data: "CantMaquinaConectada", title: "Maq Conectadas" },
                    { data: "CantMaquinaNoConectada", title: "Maq No Conectadas" },
                    { data: "CantMaquinaPLay", title: "Maq PLAY" },
                    { data: "CantMaquinaRetiroTemporal", title: "Maq R.Temporal" },
                    { data: "TotalMaquina", title: "Total" }
 
                ]
            });

            toastr.success("Datos de la sala cargados correctamente.", "Éxito");
        },
        error: function () {
            toastr.error("Error al cargar los datos de la sala.", "Error");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}
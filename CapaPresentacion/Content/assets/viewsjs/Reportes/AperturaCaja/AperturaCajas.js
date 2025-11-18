
var TiposCajas = {
    5: 'Fichas',
    12: 'ATM'
}

// document ready
$(document).ready(function () {

    listarSalas()

    $(document).on('click', '#btnBuscar', function (event) {
        var listasala = $("#cboSala").val();
        if (listasala == '') {
            toastr.warning("Por favor, seleccione una sala");
            return
        }

        obtenerReporteAperturaCaja();
    })


    $(document).on('click', '#btnExcel', function (event) {
        var listasala = $("#cboSala").val();
        if (listasala == '') {
            toastr.warning("Por favor, seleccione una sala");
            return
        }

        GenerarExcel();
    })


})



const listarSalas = () => {

    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (result) {
            var datos = result.data
            $("#cboSala").append('<option value="">--Seleccione--</option>')
            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>')
            })
            $("#cboSala").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: true
            })
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    });
}



const GenerarExcel = () => {
    let listasala = $("#cboSala").val()

    $.ajax({
        type: "POST",
        url: basePath + "Reportes/ExcelAperturaCajas",
        cache: false,
        data: JSON.stringify({ salaId: listasala }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (result) {
            if (result.success) {
                let data = result.data;
                let file = result.fileName;
                let a = document.createElement('a');
                a.target = '_self';
                a.href = "data:application/vnd.ms-excel;base64, " + data;
                a.download = file;
                a.click();
            } else {
                toastr.warning(result.message);
            }
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    });
}

//Obtener reporte apertura caja por el id de la sala
const obtenerReporteAperturaCaja = () => {
    
   
    let listasala = $("#cboSala").val()
    
    var addtabla = $(".contenedor_tabla");


    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: `${basePath}/Reportes/ListarAperturaCajas`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ salaId: listasala }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            AbortRequest.close()
            if (response.success) {
                toastr.success('Se obtuvo respuesta de la sala.')
            } else {
                toastr.warning(response.message);
            }
            response = response.data;
            $(addtabla).empty();
            $(addtabla).append('<table id="alertastbl" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#alertastbl").DataTable({
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
                    { data: "CodCaja", title: "Codigo Caja" },
                    { data: "NombreEmpresa", title: "Empresa" },
                    { data: "NombreSala", title: "Sala" },
                    {
                        data: "FechaApertura", title: "Fecha Apertura",
                        "render": function (value) {
                            var fecha = moment(value);

                            if (fecha.format('DD/MM/YYYY') == '01/01/1753' || fecha.format('DD/MM/YYYY') == '31/12/1752') {
                                return '';
                            } else {
                                return fecha.format('DD/MM/YYYY hh:mm:ss A');
                            }
                        }
                    },
                    {
                        data: "FechaCierre", title: "Fecha Cierre",
                        "render": function (value) {
                            var fecha = moment(value);

                            if (fecha.format('DD/MM/YYYY') == '01/01/1753' || fecha.format('DD/MM/YYYY') == '31/12/1752') {
                                return '';
                            } else {
                                return fecha.format('DD/MM/YYYY hh:mm:ss A');
                            }
                        }
                    },
                    { data: "Item", title: "Item" },
                    { data: "Turno", title: "Turno" },
                    {
                        data: "TipoCaja",
                        title: "Tipo Caja",
                        render: function (value) {
                            return TiposCajas[value] ? TiposCajas[value] : 'Otros'
                        }
                    }
                ],
                columnDefs: [
                    {
                        targets: [0, 3, 4, 5, 6, 7],
                        className: "text-center"
                    }
                ],
                "initComplete": function (settings, json) {

                },
                "drawCallback": function (settings) {

                },
            });
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
            AbortRequest.close()

        }
    });
    AbortRequest.open()

    return ajaxhr
}
$(document).ready(function () {

    $("#fechaInicio").datetimepicker({
        format: 'DD/MM/YYYY',
        defaultDate: moment(new Date()).format('DD/MM/YYYY'),
        pickTime: false,
        maxDate: new Date(),

    });
    $("#fechaFin").datetimepicker({
        format: 'DD/MM/YYYY',
        defaultDate: moment(new Date()).format('DD/MM/YYYY'),
        pickTime: false,
        minDate: new Date(),
    });
    $(document).on('dp.change', '#fechaInicio', function (e) {
        $('#fechaFin').data("DateTimePicker").setMinDate(e.date);
    })
    $(document).on('dp.change', '#fechaFin', function (e) {
        $('#fechaInicio').data("DateTimePicker").setMaxDate(e.date)
    });

    $(document).on('click', '#btnBuscar', function (e) {
        e.preventDefault()
       
        if ($("#fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.")
            return false
        }
        if ($("#fechaFin").val() == "") {
            toastr.error("Ingrese una fecha Fin.")
            return false
        }
        if ($("#nrodoc").val() == "") {
            toastr.error("Ingrese un nro de documento")
            return false
        }
        buscarListadoCampaniaCliente();
    })
});

function buscarListadoCampaniaCliente() {
    let fechaIni = $("#fechaInicio").val();
    let fechaFin = $("#fechaFin").val();
    let nrodoc = $("#nrodoc").val();
  
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Campania/ListarCampaniasxClienteJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ fechaIni, fechaFin, nrodoc: nrodoc, }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            console.log(response)
            myDatatable = $("#tableregistro").DataTable({
                "bDestroy": true,
                "bSort": true,
                "ordering": true,
                "scrollCollapse": true,
                "scrollX": false,
                "sScrollX": "100%",
                "paging": true,
                "autoWidth": false,
                "bAutoWidth": true,
                "bProcessing": true,
                "bDeferRender": true,
                "aaSorting": [],
                data: response.data,
                columns: [
                    { data: "NombreCompleto", title: "Cliente" },
                    { data: "NroDoc", title: "Nro. Doc." },
                    { data: "nombreSala", title: "Sala" },
                    
                    { data: "CampaniaNombre", title: "Campaña" },
                    { data: "TipoCampania", title: "Tipo Campaña" },
                    {
                        data: null, title: "Fecha",
                        "render": function (value) {
                            let span = '<span style="display:none">' + value.Fecha + '</span>'
                            return value.TipoCampania == "Sorteo" ? span + moment(value.Fecha).format('DD/MM/YYYY') + " " + moment(value.Hora).format('hh:mm a') : ''

                        }

                    },
                    {
                        data: null, title: "SerieIni",
                        "render": function (value) {
                            return value.TipoCampania == "Sorteo" ? value.SerieIni : ''
                        }
                    },
                    {
                        data: null, title: "SerieFin",
                        "render": function (value) {
                            return value.TipoCampania == "Sorteo" ? value.SerieFin : ''
                        }
                    },
                    {
                        data: null, title: "CantidadCupones",
                        "render": function (value) {
                            return value.TipoCampania == "Sorteo" ? value.CantidadCupones : ''
                        }
                    },
                  
                ]
                ,
                "initComplete": function (settings, json) {
                },
                "drawCallback": function (settings) {
                }
            });
        },
        error: function (request, status, error) {
            
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
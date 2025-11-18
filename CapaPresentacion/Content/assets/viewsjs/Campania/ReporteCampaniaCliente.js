$(document).ready(function(){
    ObtenerListaSalas()
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
        minDate:new Date(),
    });
    $(document).on('dp.change','#fechaInicio',function(e){
        $('#fechaFin').data("DateTimePicker").setMinDate(e.date)
    })
    $(document).on('dp.change','#fechaFin',function(e){
        $('#fechaInicio').data("DateTimePicker").setMaxDate(e.date)
    })
    $(document).on('click','#btnBuscar',function(e){
        e.preventDefault()
        if ($("#cboSala").val() == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor")
            return false
        }
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
        buscarListadoCampaniaCliente()
    })
    $(document).on('click','#btnExcel',function(e){
        e.preventDefault()
        if ($("#cboSala").val() == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor")
            return false
        }
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
        GenerarExcelGeneral()
    })
})
function ObtenerListaSalas() {
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
                $("#cboSala").append('<option value="' + value.CodSala + '" data-urlsala="'+value.UrlProgresivo+'"  >' + value.Nombre + '</option>')
            })
            $("#cboSala").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: true
            })
            // $("#cbo_sala_id").val(0).trigger("change");
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
function buscarListadoCampaniaCliente() {
    let listasala = $("#cboSala").val();
    let fechaIni = $("#fechaInicio").val();
    let fechaFin = $("#fechaFin").val();
    let nrodoc = $("#nrodoc").val();
    let addtabla = $(".contenedor_tabla");
    let urlSala=$('#cboSala option').filter(':selected').data("urlsala")
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CampaniaReporte/ListarCampaniasxClienteJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaIni, fechaFin,nrodoc:nrodoc,UrlProgresivoSala:urlSala}),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            console.log(response)
            $(addtabla).empty();
            $(addtabla).append('<table id="reporteTable" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            myDatatable = $("#reporteTable").DataTable({
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
                    { data: "CampaniaNombre", title: "Campaña" },
                    { data: "TipoCampania", title: "Tipo Campaña" },
                    {
                        data: null, title: "Fecha",
                        "render": function (value) {
                            let span='<span style="display:none">'+value.Fecha+'</span>'
                            return value.TipoCampania=="Sorteo"? span+ moment(value.Fecha).format('DD/MM/YYYY'):''

                        }
                        
                    },
                    { data: null, title: "SerieIni" ,
                        "render":function (value) { 
                            return value.TipoCampania=="Sorteo"?value.SerieIni:''
                        }
                    },
                    { data: null, title: "SerieFin",
                      "render":function (value) {
                        return value.TipoCampania=="Sorteo"?value.SerieFin:''
                      }
                    },
                    { data: null, title: "CantidadCupones",
                      "render":function (value) {
                        return value.TipoCampania=="Sorteo"?value.CantidadCupones:''
                      }
                    },
                    // { data: null, title: "Parametro",
                    //   "render":function (value) {
                    //     return value.TipoCampania=="Sorteo"?value.Parametro:''
                    //   }
                    // },
                ]
                ,
                "initComplete": function (settings, json) {
                },
                "drawCallback": function (settings) {
                }
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
function GenerarExcelGeneral(){
    let listasala = $("#cboSala").val();
    let fechaIni = $("#fechaInicio").val();
    let fechaFin = $("#fechaFin").val();
    let nrodoc = $("#nrodoc").val();
    let url = "CampaniaReporte/ReporteCampaniaClienteExcelJson";
    let urlSala=$('#cboSala option').filter(':selected').data("urlsala")
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaIni, fechaFin,nrodoc:nrodoc, UrlProgresivoSala:urlSala}),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            if (response.respuesta) {
                let data = response.data;
                let file = response.excelName;
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
}
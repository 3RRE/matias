$(document).ready(function(){

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
    // $(".dateOnly_").datetimepicker({
    //     pickTime: false,
    //     format: 'DD/MM/YYYY',
    //     defaultDate: dateNow,
    //     maxDate: dateNow,
    // });
    $(document).on('dp.change','#fechaInicio',function(e){
        $('#fechaFin').data("DateTimePicker").setMinDate(e.date);
    })
    $(document).on('dp.change','#fechaFin',function(e){
        $('#fechaInicio').data("DateTimePicker").setMaxDate(e.date);
    })
    ObtenerListaSalas();
    $("#btnBuscar").on("click", function () {
        var listasala = $("#cboSala").val();
        var fechaIni = $("#fechaInicio").val();
        var fechaFin = $("#fechaFin").val();
        var addtabla = $(".contenedor_tabla");
        if (listasala == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if (fechaIni == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if (fechaFin == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }
       
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Ocurrencias/GetListadoOcurrenciaReporteJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ArraySalaId: listasala, fechaIni, fechaFin }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
    
            success: function (response) {
                let data = response.data;
                let columnas=ColumnasDatatable(data);
                $(addtabla).empty();
                $(addtabla).append('<table id="ResumenSala" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
                objetodatatable = $("#ResumenSala").DataTable({
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
                    data: data,
                    columns: columnas,
                    "initComplete": function (settings, json) {
                    },
                    "drawCallback": function (settings) {
                    }
                });
                $('#ResumenSala tbody').on('click', 'tr', function () {
                    $(this).toggleClass('selected');
                });
            },
            error: function (request, status, error) {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    });
    $(document).on("click", "#btnExcel", function () {
        var listasala = $("#cboSala").val();
        var fechaIni = $("#fechaInicio").val();
        var fechaFin = $("#fechaFin").val();
        if (listasala == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if (fechaIni == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if (fechaFin == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Ocurrencias/GetListadoOcurrenciaReporteExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ArraySalaId: listasala, fechaIni, fechaFin }),
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
            },
            error: function (request, status, error) {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });
    $(document).on('click', ".btnPdf", function (e) {
        let hash = $(this).data("hash");
        let a = document.createElement('a');
        a.target = '_self';
        a.href = basePath + "Ocurrencias/GenerarOcurrenciaPdfJson/?hash=" + hash;;
        a.click();
    });
    $(document).on("click", "#btnPdfMultple", function () {
        var listasala = $("#cboSala").val();
        var fechaIni = $("#fechaInicio").val();
        var fechaFin = $("#fechaFin").val();
        if (listasala == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if (fechaIni == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if (fechaFin == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Ocurrencias/GetListadoOcurrenciaReportePdfJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ArraySalaId: listasala, fechaIni, fechaFin }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                console.log(response);
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
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });
    
})
function ColumnasDatatable(data){
    console.log(data);
    let obj=[
        { 
            title: "Fecha" 
        },
        {
            title: "Nombres",
        },
        {
            title: "Tipo Documento",
        },
        {
            title: "Nro. Documento",
        },
        {
            title: "Tipo Ocurrencia",
        },
        {
            title: "Descripción",
        },
        {
            title: "Sala",
        },
        {
            title: "Jefe Sala",
        },
        {
            title: "Se Informó A",
        },
        {
            title: "Acciones",
        },
    ]
    if(data){
        if(data.length>0){
            obj=[
                { 
                    data: "Fecha", 
                    title: "Fecha",
                    render:function(value,type, oData, meta){
                        return moment(oData.Fecha).format('YYYY/MM/DD hh:mm:ss A')
                    } 
                },
                { 
                    data: "Nombres", 
                    title: "Nombres",
                    render:function(value,type, oData, meta){
                        return oData.ApelPat+ " "+oData.ApelMat+","+oData.Nombres;
                    } 
                },
                { 
                    data: "TipoDocumento.DESCRIPCION", 
                    title: "Tipo Documento" 
                },
                { 
                    data: "NroDoc", 
                    title: "Nro. Documento" 
                },
                { 
                    data: "TipoOcurrencia.Nombre", 
                    title: "Tipo Ocurrencia" 
                },
                { 
                    data: "Descripcion", 
                    title: "Descripción" 
                },
                { 
                    data: "Sala.Nombre", 
                    title: "Sala" 
                },
                { 
                    data: "JefeSala", 
                    title: "Jefe Sala" 
                },
                { 
                    data: "SeInformoA", 
                    title: "Se Informó A" 
                },
                {
                    data: null,
                    title:"ACCIONES",
                    "bSortable": false,
                    "render": function (value, type, oData, meta) {
                        let botones =  `<button type="button" class="btn btn-xs btn-info btnPdf" 
                                            data-id=${oData.Id}
                                            data-hash=${oData.Hash}>
                                            <i class="fa fa-file-pdf-o"></i> Pdf
                                        </button>`;
                        return botones;
                    }
                }
            ]

        }
    }
    return obj
}
function ObtenerListaSalas() {
    $.ajax({
        type: "POST",
        url: basePath + "Ocurrencias/ListadoSalaPorUsuarioJson",
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
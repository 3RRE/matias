$(document).ready(function () {
    ObtenerListaSalas()
    $("#fechaInicio").datetimepicker({
        format: 'DD/MM/YYYY',
        defaultDate: moment(new Date()).format('DD/MM/YYYY'),
        pickTime: false,
        maxDate: new Date(),
    })
    $("#fechaFin").datetimepicker({
        format: 'DD/MM/YYYY',
        defaultDate: moment(new Date()).format('DD/MM/YYYY'),
        pickTime: false,
        minDate:new Date(),
    })
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
        buscarListadoEventos()
    })
    $(document).on('click','.btnDetalle',function(e){
        e.preventDefault()
        let id=$(this).data('id')
        $("#divEventosOnline").hide()
        $("#divEventosTecnologias").hide()
        $("#divAlertaBilleteros").hide()
        mostrarDetalleLogs(id)
        $("#full-modal-DetalleLogs").modal("show")
        
    })
    $(document).on("click", "#btnExcel", function (e) {
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
        let listasala = $("#cboSala").val()
        let FechaInicio = $("#fechaInicio").val()
        let FechaFin = $("#fechaFin").val()
        let Tipo=$("#cboTipo").val()
        let dataForm={ salas: listasala, FechaInicio, FechaFin,Tipo:Tipo }
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "LogAlertaBilleteros/GetLogAlertaBilleterosxFiltrosExcel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:JSON.stringify(dataForm),
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
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>')
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
function buscarListadoEventos() {
    let listasala = $("#cboSala").val()
    let FechaInicio = $("#fechaInicio").val()
    let FechaFin = $("#fechaFin").val()
    let addtabla = $(".contenedor_tabla")
    let Tipo=$("#cboTipo").val()
    ajaxhr = $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "LogAlertaBilleteros/GetLogAlertaBilleterosxFiltros",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ salas: listasala, FechaInicio, FechaFin,Tipo:Tipo }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            AbortRequest.close()

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
                    { data: "Id", title: "ID" },
                    { data: "NombreSala", title: "Sala" },
                    {
                        data: "FechaRegistro", title: "Fecha Reg.",
                        "render": function (value) {
                            let span='<span style="display:none">'+value+'</span>'
                            return span+ moment(value).format('DD/MM/YYYY hh:mm:ss A');
                        }
                    },
                   
                    {
                        data:null, title: "Tipo",
                        "render": function (value) {
                            if(value.Tipo==1){
                                return '<span class="label label-danger">Servicio Online</span>'
                            }
                            else if(value.Tipo==2){
                                return '<span class="label label-success">Eventos</span>'
                            }
                            else if (value.Tipo==3){
                                return '<span class="label label-warning">Alerta Billeteros</span>'
                            }
                            else{
                                return ''
                            }
                        }
                    },
                    {
                        data:null,title:'Cod Maquina',
                        "render":function (value) {
                            if(value.Tipo==2||value.Tipo==3){
                                let jsonDeserializado= JSON.parse(value.Descripcion)
                                return jsonDeserializado.CodMaquina
                            }
                            return ''
                        }
                    },
                    { data: null, title: "Información",
                        "render":function(value){
                            return value.Preview.substr(0, 34) + '...'
                        }
                    },
                    {
                        data: null, title: "Accion",
                        "render": function (value) {
                            var butom = "";
                            butom += ` <button type="button" class="btn btn-xs btn-primary btnDetalle" title="Detalle" data-id="${value.Id}" > Detalle</button>`;
                            return butom;
                        }
                    }
                ]
                ,
                "initComplete": function (settings, json) {
                },
                "drawCallback": function (settings) {
                }
            });
        },
        error: function (request, status, error) {
            AbortRequest.close()

            toastr.error("Error De Conexion, Servidor no Encontrado.");

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
            AbortRequest.close()

        }
    });
    AbortRequest.open()

    return ajaxhr
}
function mostrarDetalleLogs(logId){
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "LogAlertaBilleteros/GetAlertaBilleteroxId",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ Id: logId }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            let myData=response.data
            let jsonDeserializado= JSON.parse(myData.Descripcion)
            console.log(myData)
            console.log(jsonDeserializado)
            $("#FechaRegistro").val(moment(myData.FechaRegistro).format("DD-MM-YYYY HH:mm:ss"))
            if(myData.Tipo==1){
                $("#Tipo").val('Servicio Online')

                $("#Descripcion").val(jsonDeserializado.Descripcion)
                $("#InfoAdicional").val(jsonDeserializado.InfoAdicional)

                $("#divEventosOnline").show()

            }
            else if(myData.Tipo==2){
                $("#contenedor_tablaDetalleEventos > tbody").html('')

                $("#Tipo").val('Evento BD Tecnologias')

                $("#Cod_Even_OL").val(jsonDeserializado.Cod_Even_OL)
                $("#Fecha").val(moment(jsonDeserializado.Fecha).format('DD-MM-YYYY HH:mm:ss'))
                $("#CodTarjeta").val(jsonDeserializado.CodTarjeta)
                $("#CodMaquina").val(jsonDeserializado.CodMaquina)
                $("#Evento").val(jsonDeserializado.Evento)
                let listaDetalle=jsonDeserializado.ListaEventoDispositivo
                let span=[]
                listaDetalle.map((detalle)=>{
                    let usuario=''
                    if(detalle.Usuario){
                        usuario=detalle.Usuario
                    }
                    span.push(`<tr>
                        <td>${detalle.EventoId}</td>
                        <td>${moment(detalle.FechaRegistro).format("DD-MM-YYYY hh:mm:ss A")}</td>
                        <td>${detalle.DispositivoNombre}</td>
                        <td>${usuario}</td>
                    </tr>`)
                })
                $("#contenedor_tablaDetalleEventos > tbody").append(span.join(''))
                $("#divEventosTecnologias").show()

                
            }else if(myData.Tipo==3){
                $("#Tipo").val('Alerta Billeteros')
                $("#contenedor_tablaDetalleAlertas > tbody").html('')
                $("#AlertaID").val(jsonDeserializado.AlertaID)
                $("#FechaAlerta").val(jsonDeserializado.fecha_registro)
                $("#CodMaquinaAlerta").val(jsonDeserializado.CodMaquina)
                $("#DescripcionAlerta").val(jsonDeserializado.descripcion_alerta)
                $("#BillBilleteroAlerta").val(jsonDeserializado.contador_bill_billetero.toFixed(2))
                $("#BillParcialAlerta").val(jsonDeserializado.contador_bill_parcial.toFixed(2))
                $("#SalaAlerta").val(myData.NombreSala)
                
                let listaDetalle=jsonDeserializado.ListaAlertaDispositivo
                let span=[]
                listaDetalle.map((detalle)=>{
                    let usuario=''
                    if(detalle.Usuario){
                        usuario=detalle.Usuario
                    }
                    span.push(`<tr>
                        <td>${detalle.AlertaId}</td>
                        <td>${moment(detalle.FechaRegistro).format("DD-MM-YYYY hh:mm:ss A")}</td>
                        <td>${detalle.DispositivoNombre}</td>
                        <td>${usuario}</td>
                    </tr>`)
                })
                $("#contenedor_tablaDetalleAlertas > tbody").append(span.join(''))
                $("#divAlertaBilleteros").show()

            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
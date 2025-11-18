let myDatatable
$(document).ready(function (){
    const LIMITE_DIAS=7
    let campaniaid
    let table
    let idcupon
    // let fechaInicio=new Date()
    // let fechaFin=new Date()
    // fechaInicio = new Date(fechaInicio.setDate(fechaInicio.getDate()-LIMITE_DIAS))
    ObtenerListaSalas()
    $("#fechaInicio").datetimepicker({
        format: 'DD/MM/YYYY',
        // defaultDate: moment(fechaInicio).format('DD/MM/YYYY'),
        defaultDate:new Date(),
        pickTime: false,
        // minDate:fechaInicio,
        // maxDate: fechaFin,
    });

    $("#fechaFin").datetimepicker({
        format: 'DD/MM/YYYY',
        // defaultDate: moment(fechaFin).format('DD/MM/YYYY'),
        defaultDate: new Date(),
        pickTime: false,
        // minDate:fechaInicio,
        // maxDate: fechaFin,
    });
    // $(document).on('dp.change','#fechaInicio',function(e){
    //     fechaInicio=new Date(e.date._d)
    //     fechaFin=new Date(e.date._d)
    //     fechaFin=fechaFin.setDate(fechaFin.getDate()+LIMITE_DIAS)
    //     $('#fechaInicio').data("DateTimePicker").setMinDate(new Date(fechaInicio))
    //     $('#fechaInicio').data("DateTimePicker").setMaxDate(new Date(fechaFin))

    //     $('#fechaFin').data("DateTimePicker").setMinDate(new Date(fechaInicio))
    //     $('#fechaFin').data("DateTimePicker").setMaxDate(new Date(fechaFin))
    // })
    // $(document).on('dp.change','#fechaFin',function(e){
    //     fechaInicio=new Date(e.date._d)
    //     fechaFin=new Date(e.date._d)
    //     fechaInicio=fechaInicio.setDate(fechaInicio.getDate()-LIMITE_DIAS)
    //     $('#fechaInicio').data("DateTimePicker").setMinDate(new Date(fechaInicio))
    //     $('#fechaInicio').data("DateTimePicker").setMaxDate(new Date(fechaFin))

    //     $('#fechaFin').data("DateTimePicker").setMinDate(new Date(fechaInicio))
    //     $('#fechaFin').data("DateTimePicker").setMaxDate(new Date(fechaFin))
    // })
    $(document).on('click','#btnBuscar',function(e){
        e.preventDefault()
        let fechaInicial= $('#fechaInicio').data("DateTimePicker").date._d
        let fechaFinal= $('#fechaFin').data("DateTimePicker").date._d
        fechaInicial.setDate(fechaInicial.getDate()+LIMITE_DIAS)
        if(fechaFinal<=fechaInicial){
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
            buscarListadoCampaniaSorteo()
        }
        else{
            toastr.warning("El rango maximo permitido por consulta es de 7 dias")
        }
       
    })
        
    // Add event listener for opening and closing details
    $(document).on('click', '#reporteTable tbody td.details-control', function (e) {
        table=myDatatable
        let tr = $(this).closest('tr');
        let row = table.row(tr);
        let campania = row.data().id;
        if (row.child.isShown()) {
            // la fila ya esta abierra, cerrar la fila y cambiar el icono
            row.child.hide();
            tr.removeClass('shown');
            tr.find('i').removeClass('fa fa-angle-double-down'); 
            tr.find('i').addClass('fa fa-angle-double-up'); 
            tr.find('i').css("color","rgb(255, 0, 0)");
            // // This row is already open - close it
            // row.child.hide();
            // tr.removeClass('shown');
        }
        else {
            // Open this row
            if (campania > 0) {
                let fechaIni = $("#fechaInicio").val();
                let fechaFin = $("#fechaFin").val();
                let urlSala=$('#cboSala option').filter(':selected').data("urlsala")
                var dataForm = {
                    CampaniaId: campania,
                    fechaInicio:fechaIni,
                    fechaFin:fechaFin,
                    UrlProgresivoSala: urlSala
                };
                $.ajax({
                    type: "POST",
                    url: basePath + "CampaniaReporte/GetListadoCuponesxCampaniaReporte",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(dataForm),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (response) {
                        let rows = response.data;
                        row.child(detalle(rows,campania)).show();
                        $(".myDatatable"+campania).DataTable({
                            destroy: true,
                            scrollX: true
                        })
                        // Abrir la fila de detalle
                        let td=row.child().find("td:eq(0)");
                        td.css("padding-left",'2%')
                        tr.addClass('shown');
                        tr.find('i').removeClass('fa fa-angle-double-up'); 
                        tr.find('i').addClass('fa fa-angle-double-down');
                        tr.find('i').css("color","rgb(119, 119, 119)");
                        // tr.addClass('shown');
                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
                // responseSimple({
                //     url: "IntranetElemento/GetListadoCuponesxCampania",
                //     data: JSON.stringify(dataForm),
                //     refresh: false,
                //     callBackSuccess: function (response) {
                //         var rows = response.data;
                //         row.child(MostrarDetalle(rows)).show();
                //         tr.addClass('shown');
                //     }
                // })
            }
        }
        // let tr = $(this).closest('tr');
        // let row = table.row( tr );
 
        // if ( row.child.isShown() ) {
        //     // la fila ya esta abierra, cerrar la fila y cambiar el icono
        //     row.child.hide();
        //     tr.removeClass('shown');
        //     tr.find('i').removeClass('fa fa-angle-double-down'); 
        //     tr.find('i').addClass('fa fa-angle-double-up'); 
        //     tr.find('i').css("color","rgb(255, 0, 0)");
            
        // }
        // else {
        //     // Abrir la fila de detalle
        //      row.child( detalle() ).show();
        //     let td=row.child().find("td:eq(0)");
        //     // console.log(td)
        //     td.css("padding-left",'2%')
        //     tr.addClass('shown');
        //     tr.find('i').removeClass('fa fa-angle-double-up'); 
        //     tr.find('i').addClass('fa fa-angle-double-down');
        //     tr.find('i').css("color","rgb(119, 119, 119)");
        // }
    } );
    $(document).on("click", ".btnCuponesdetalle", function () {
        idcupon = $(this).data("id")
        $("#full-modal_detallecupones").modal("show")
    });
    $('#full-modal_detallecupones').on('shown.bs.modal', function (e) {
        $(".tabladetallecuponesdiv").html('<table class="table table-bordered table-hover table-condensed">' +
            '<thead>' +
            '<tr>' +
            '<th>Maquina</th>' +
            '<th>Serie Ini</th>' +
            '<th>Serie Fin</th>' +
            '<th>Coin Out Ini</th>' +
            '<th>Coin Out Fin</th>' +
            '<th>Current Credit</th>' +
            '<th>Monto</th>' +
            '<th>Token</th>' +
            '<th>Cant. Cupones</th>' +
            '<th>Fecha Registro</th>' +
            '<th>Accion</th>' +
            '</tr>' +
            '</thead>' +
            '<tbody><tr><td colspan="11"><div class="alert alert-danger"> Detalle Cupones</div></td></tr></tbody>' +
            '</table>');
        detalleCuponesRegistrados(idcupon);
    });
    $(document).on('click','.btnExcel',function(e) {
        e.preventDefault()
        let fechaInicial= $('#fechaInicio').data("DateTimePicker").date._d
        let fechaFinal= $('#fechaFin').data("DateTimePicker").date._d
        fechaInicial.setDate(fechaInicial.getDate()+LIMITE_DIAS)
        if(fechaFinal<=fechaInicial){
            campaniaid=$(this).data('id')
            GenerarExcel(campaniaid)
        }
        else{
            toastr.warning("El rango maximo permitido por consulta es de 7 dias")
        }
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
function buscarListadoCampaniaSorteo() {
    var listasala = $("#cboSala").val();
    var fechaIni = $("#fechaInicio").val();
    var fechaFin = $("#fechaFin").val();
    var addtabla = $(".contenedor_tabla");
    let urlSala=$('#cboSala option').filter(':selected').data("urlsala")
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CampaniaReporte/ListarCampaniaSorteoPorFechasJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaIni, fechaFin,tipo:1,UrlProgresivoSala:urlSala }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
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
                    {
                        "className":      'details-control',
                        "orderable":      false,
                        "data":           null,
                        "defaultContent": '',
                        "data": "id",
                        "render":function(value){
                            // return '<a href="javascript:void(0);" class="tooltip-info" data-id="' + value + '" data-rel="tooltip" title="Ver Detalle"><span class="blue" ><i class="ace-icon fa fa-search-plus bigger-120"></i></span ></a>'
                            return '<i class="fa fa-angle-double-up" style="color:rgb(255, 0, 0);"></i><span class="sr-only">Detalle</span>';
                         }
                    },
                    { data: "id", title: "ID" },
                    // { data: "nombresala", title: "Sala" },
                    { data: "nombre", title: "Nombre" },
                    {
                        data: "fechareg", title: "Fecha Reg.",
                        "render": function (value) {
                            let span='<span style="display:none">'+value+'</span>'
                            return span+ moment(value).format('DD/MM/YYYY hh:mm:ss A');
                        }
                    },
                   
                    {
                        data: "fechaini", title: "Fecha Ini.",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY');
                        }
                    },
                    {
                        data: "fechafin", title: "Fecha Fin",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY');
                        }
                    },
                    {
                        data: "estado", title: "Estado",class:"tdcenter",
                        "render": function (value) {
                            var estado = "Vencida";
                            var css = "btn-danger";
                            if (value == 1) {
                                estado = "Activa"
                                css = "btn-success";
                            }
                            return '<span class="label '+css+'">'+estado+'</span>';
                        }
                    },
                    {
                        data: "tipo", title: "Tipo", class: "tdcenter",
                        "render": function (value) {
                            var estado = "Promocion";
                            var css = "btn-warning";
                            if (value == 1) {
                                estado = "Sorteo"
                                css = "btn-success";
                            }
                            return '<span class="label ' + css + '">' + estado + '</span>';
                        }
                    },
                    // { data: "usuarionombre", title: "Usu. Reg." },
                    {
                        data: null, title: "Accion",
                        "render": function (value) {
                            var butom = "";
                            butom += ` <button type="button" class="btn btn-xs btn-primary btnExcel" title="Excel" data-id="${value.id}" ><i class="fa fa-user"></i> Excel</button>`;
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
            console.log(myDatatable)
        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
function detalle(row,idcampania){
    let tabla = '';
    if (row.length > 0) {
        tabla += '<div style="width:100%;padding-right:10px;" class="table-responsive"><table class="table myDatatable'+idcampania+' table-bordered table-sm "><thead><tr class="thead-dark">';
        tabla += '<th>SesionId</th>';
        tabla += '<th>Maquina</th>';
        tabla += '<th>Nombre</th>';
        tabla += '<th>NroDoc</th>';
        // tabla += '<th>Fecha Nac.</th>';
        tabla += '<th>Cant. Cupones</th>';
        tabla += '<th>Serie Ini</th>';
        tabla += '<th>Serie Fin</th>';
        tabla += '<th>Fecha</th>';
        tabla += '<th>Mail</th>';
        tabla += '<th>Acciones</th>';
        tabla += '</tr></thead><tbody>';
        $.each(row, function (index, value) {
            tabla += '<tr>';
            tabla += '<td>' + value.SesionId + '</td>';
            tabla += '<td>' + value.SlotId + '</td>';
            tabla += '<td>' + value.NombreCompleto + '</td>';
            tabla += '<td>' + value.NroDoc + '</td>';
            // tabla += '<td>' + moment(value.FechaNacimiento).format('DD-MM-YYYY') + '</td>';
            tabla += '<td>' + value.CantidadCupones + '</td>';
            tabla += '<td>' + value.SerieIni + '</td>';
            tabla += '<td>' + value.SerieFin + '</td>';
            tabla += '<td>' + moment(value.Fecha).format('DD-MM-YYYY hh:mm A')+'</td>';
            tabla += '<td>' + value.Mail + '</td>';
            tabla += '<td><button type="button" class="btn btn-xs btn-primary btnCuponesdetalle" data-id="'+value.SesionId+'"><i class="fa fa-building"></i> Detalle</button></td>';
            tabla += '</tr>';
        });
        tabla += '</tbody></table></div>';
    }
    return tabla;
    //butom += `<button type="button" class="btn btn-xs btn-primary btnCuponesdetalle" data-id="${y.CgId}" data-nombre="${nombre}"><i class="fa fa-building"></i> </button>`;
}
function detalleCuponesRegistrados(cuponid) {

    if(cuponid){
        let addtabla = $(".tabladetallecuponesdiv");
        let urlSala=$('#cboSala option').filter(':selected').data("urlsala")
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "CampaniaCupones/GetListadoCuponesImpresos",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ SesionId: cuponid ,UrlProgresivo:urlSala }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
    
            success: function (response) {
                data = response.data;
                $(addtabla).empty();
                $(addtabla).append('<table id="cuponeslistaclientes" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
                objetodatatable = $("#cuponeslistaclientes").DataTable({
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
                    data: data,
                    columns: [
    
                        { data: "CodMaq", title: "Maquina" },
                        { data: "CodSala", title: "Cod Sala" },
                        { data: "SerieIni", title: "Serie Ini" },
                        { data: "SerieFin", title: "Serie Fin" },
                        {data:"CantidadCuponesImpresos",title:"Cantidad Cupones"},
                        { data: "CoinOutAnterior", title: "Coin Out Ini" },
                        { data: "CoinOut", title: "Coin Out Fin" },
                        { data: "CurrentCredits", title: "Current Credits" },
                        { data: "Monto", title: "Monto" },
                        { data: "Token", title: "Token" },
                        { data: "CoinOutIas", title: "Coin Out Parametro" },
                        {
                            data: "FechaRegistro", title: "Fecha Reg.",
                            "render": function (value) {
                                return moment(value).format('DD/MM/YYYY hh:mm:ss a');
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
            //error: function (request, status, error) {
            //    toastr.error("Error De Conexion, Servidor no Encontrado.");
            //},
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    }
  
}
function GenerarExcel(campaniaid){
    let fechaIni = $("#fechaInicio").val();
    let fechaFin = $("#fechaFin").val();
    let url = "CampaniaReporte/ReporteCampaniaSorteoExcelJson";
    let urlSala=$('#cboSala option').filter(':selected').data("urlsala")
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ campaniaid:campaniaid,fechaIni:fechaIni,fechaFin:fechaFin,UrlProgresivoSala:urlSala }),
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
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
function GenerarExcelGeneral(){
    var listasala = $("#cboSala").val();
    var fechaIni = $("#fechaInicio").val();
    var fechaFin = $("#fechaFin").val();
    let url = "CampaniaReporte/ReporteCampaniaSorteoExcelTodosJson";
    let urlSala=$('#cboSala option').filter(':selected').data("urlsala")
    let ids=[]
    if(myDatatable){
        let selectedRows=myDatatable.rows( {search:'applied'} )
        .data();
        selectedRows.map((item)=>{
            ids.push(item.id)
        })
    }
    console.log(ids)
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ListaCampaniaIds:ids,UrlProgresivoSala:urlSala }),
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
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
$(document).ready(function() {
    let campaniaid
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
        buscarListadoCampaniaPromocion()
    })

    $(document).on('click','#button_excel_1',function(event) {
        event.preventDefault()

        toastr.remove()

        if (!$("#cboSala").val()) {
            toastr.warning("Por favor, seleccione sala")

            return false
        }

        if (!$("#fechaInicio").val()) {
            toastr.warning("Por favor, seleccione fecha inicio")

            return false
        }

        if (!$("#fechaFin").val()) {
            toastr.warning("Por favor, seleccione fecha final")

            return false
        }

        GenerarExcelGeneral()
    })

    $(document).on('click', '.btnTickets', function (e) {
        e.preventDefault()

        let id = $(this).data('id')
        var nombre_campania = $(this).data("nombre")
        var nombre_campania_wrap = $("#nombre_campania")
        var button_camptickets_excel = $("#button_camptickets_excel")

        nombre_campania_wrap.html('')

        toastr.remove()

        if (!id) {
            toastr.warning('Por favor, seleccione una campaña')

            return false
        }

        nombre_campania_wrap.text(nombre_campania)
        button_camptickets_excel.attr('data-id', id)

        mostrarTicketsCampania(id)

        $("#full-modal_tickets").modal("show")
    })

    $(document).on('click','.btnClientes',function(e){
        e.preventDefault()
        campaniaid=$(this).data('id')
        $("#full-modal_clienteRegistrados").modal("show");
    })
    $('#full-modal_clienteRegistrados').on('shown.bs.modal', function (e) {
        $(".tablaclientesdiv").html('<table class="table table-bordered table-hover table-condensed">'+
                                '<thead>'+
                                    '<tr>'+
                                        '<th>Cliente</th>'+
                                        '<th>Nro Documento</th>'+
                                        '<th>Telefono</th>'+
                                        '<th>Fecha Nacimiento</th>'+
                                    '</tr>'+
                                '</thead>'+
                                '<tbody><tr><td colspan="4"><div class="alert alert-danger"> Cliente(s) a Campaña</div></td></tr></tbody>'+
            '</table>');
        mostrarClientesCampania(campaniaid)
    });
    $(document).on('click','.btnExcel',function(e) {
        e.preventDefault()
        campaniaid=$(this).data('id')
        GenerarExcel(campaniaid)
    })

    $(document).on('click', '#button_camptickets_excel', function (event) {
        event.preventDefault()

        var campaniaId = $(this).attr('data-id')

        toastr.remove()

        if (!campaniaId || campaniaId <= 0) {
            toastr.warning('Por favor, seleccione una campaña')

            return false
        }

        GenerarExcel(campaniaId)
    })

    $(document).on('click', '#button_excel_2', function (event) {
        event.preventDefault()

        var salaIds = $('#cboSala').val()
        var fechaInicio = $('#fechaInicio').val()
        var fechaFin = $('#fechaFin').val()

        toastr.remove()

        if (!salaIds) {
            toastr.warning('Por favor, seleccione sala')

            return false
        }

        if (!fechaInicio) {
            toastr.warning('Por favor, seleccione fecha inicio')

            return false
        }

        if (!fechaFin) {
            toastr.warning('Por favor, seleccione fecha final')

            return false
        }

        var arguments = {
            salaIds,
            fechaInicio,
            fechaFin
        }

        GenerarExcelTicketsPromos(arguments)
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
function buscarListadoCampaniaPromocion() {
    var listasala = $("#cboSala").val();
    var fechaIni = $("#fechaInicio").val();
    var fechaFin = $("#fechaFin").val();
    var addtabla = $(".contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CampaniaReporte/ListarCampaniasxFechasJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaIni, fechaFin }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            $(addtabla).empty();
            $(addtabla).append('<table id="reporteTable" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#reporteTable").DataTable({
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

                    { data: "id", title: "ID" },
                    { data: "nombresala", title: "Sala" },
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
                    { data: "usuarionombre", title: "Usu. Reg." },
                    {
                        data: null, title: "Accion",
                        "render": function (value) {
                            var butom = "";
                            butom += ` <button type="button" class="btn btn-xs btn-success btnTickets" title="TICKETS" data-id="${value.id}" data-nombre="${value.nombre}"><i class="glyphicon glyphicon-list-alt"></i>  Tickets</button>`;
                            butom += ` <button type="button" class="btn btn-xs btn-danger btnClientes" title="CLIENTE" data-id="${value.id}" ><i class="fa fa-user"></i> Clientes</button>`;
                            butom += ` <button type="button" class="btn btn-xs btn-primary btnExcel" title="EXCEL TICKETS" data-id="${value.id}" ><i class="fa fa-file-excel-o"></i> Excel</button>`;
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
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
function mostrarTicketsCampania(campaniaid) {

    var tbodytickets_wrap = $("#tbodytickets")

    tbodytickets_wrap.html('')

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CampaniaReporte/CampaniaTicketsObtenerJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ campaniaid: campaniaid }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            tickets = response.respuesta

            if (tickets.length > 0) {
                $.each(tickets, function (index, value) {
                    tbodytickets_wrap.append(`
                    <tr data-index="${index}">
                        <td>${moment(value.fecharegsala).format('DD/MM/YYYY hh:mm:ss A')}</td>
                        <td>${value.nroticket}</td>
                        <td style="text-align:right">${value.monto.toFixed(2)}</td>
                        <td>${value.NombreCompleto}</td>
                        <td>${value.NroDoc}</td>
                        <td>${value.nombre_usuario}</td>
                    </tr>
                    `)
                })
            }
            else {
                tbodytickets_wrap.html('<tr><td colspan="7"><div class="alert alert-danger" style="margin:0px;">No se agregaron tickets</div></td></tr>')
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
function mostrarClientesCampania(campaniaid){
    var addtabla = $(".tablaclientesdiv");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CampaniaReporte/ClientesCampaniaIDObtenerJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ id: campaniaid }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            console.log(response);
            // response = response.respuesta;

            $(addtabla).empty();
            $(addtabla).append('<table id="campaniaslistaclientes" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#campaniaslistaclientes").DataTable({
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
                data: response.respuesta,
                columns: [

                    { data: "id", title: "ID" },

                    {
                        data: "NombreCompleto", title: "Cliente", render: function (x, r, y) {
                            var nombre = y.NombreCompleto == '' ? y.Nombre + ' ' + y.ApelPat + ' ' + y.ApelMat : y.NombreCompleto;
                            return nombre;
                        } },
                    { data: "NroDoc", title: "Nro. Documento" },
                    {
                        data: "FechaNacimiento", title: "Fecha Nac.",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY');
                        }
                    },
                    { data: "Mail", title: "Correo" },
                ]
                ,
                "initComplete": function (settings, json) {



                },
                "drawCallback": function (settings) {

                }
            });

            $('#campaniaslistaclientes tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
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
function GenerarExcelGeneral(){
    var listasala = $("#cboSala").val();
    var fechaIni = $("#fechaInicio").val();
    var fechaFin = $("#fechaFin").val();
    let url = "CampaniaReporte/ReporteCampaniaPromocionExcelTodosJson";
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaIni, fechaFin }),
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
                toastr.warning(response.mensaje, "Mensaje Servidor");
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
function GenerarExcel(campaniaid){
    console.log(campaniaid);
    let url = "CampaniaReporte/ReporteCampaniaPromocionExcelJson";
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ campaniaid:campaniaid }),
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

function GenerarExcelTicketsPromos({ salaIds, fechaInicio, fechaFin }) {
    var arguments = {
        salaIds,
        fechaInicio,
        fechaFin
    }

    $.ajax({
        type: "POST",
        cache: false,
        url: `${basePath}/CampaniaReporte/ExcelTicketsPromocionales`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(arguments),
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.success) {
                var data = response.data
                var file = response.filename
                var a = document.createElement('a')
                a.target = '_self'
                a.href = `data:application/vnd.ms-excel;base64,${data}`
                a.download = file
                a.click()
            }
            else {
                toastr.warning(response.message, "Mensaje Servidor")
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}
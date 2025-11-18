let consultaPorVpn = false
let ipPublica
let ipPrivada
let ipPublicaAlterna
let seEncontroAlterna = false
let urlsResultado = []
let delay = 60000
let timerId = ''
let salaId = 0

let idGlobal,idGlobal2,idGlobal3,idGlobal4

$(document).ready(function () {
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });
    $("#cboSala").select2()
    $("#cboProgresivo").select2()
    hiddenTablaGanadores(true)
    obtenerListaSalas() 

    $(document).on('change', '#cboSala', function (e) {
        e.preventDefault()

        salaId = $("#cboSala option:selected").data('id')
        $("#cboProgresivo").html(`<option value="">--Seleccione--</option>`);


        if (salaId == 0) {
            return false
        }
        
        toastr.remove()

        hiddenTablaGanadores(true)
        resetFiltroGanadores()

        getProgresivosOffline(salaId);
        renderReporteProgresivo([])
    })

    $(document).on('change', '#cboProgresivo', function (event) {
        event.preventDefault()

        renderReporteProgresivo([])
    })

    $(document).on('click', '#btnBuscar', function (e) {
        e.preventDefault()

        toastr.remove()

        salaId = $("#cboSala option:selected").data('id')

        if (salaId == 0) {

            toastr.warning('Seleccione una sala', 'Mensaje')

            return false
        }

        if ($("#cboProgresivo").val() == 0) {

            toastr.warning('Seleccione un progresivo', 'Mensaje')

            return false
        }

        getReporteProgresivo(salaId, $("#cboProgresivo").val());

    })


    $(document).on('click', '.btnVer', function (e) {
        e.preventDefault()
        //let RegD = 10;
        //let RegA = 10;
        //let Fecha = moment($(this).attr("data-Fecha")).format("DD-MM-YYYY HH:mm:ss");
        //let maquina = $(this).attr("data-maquina");
        //getDetalleContadoresOffline(Fecha,maquina, RegA, RegD );
        let id = $(this).attr("data-id");
        getDetallesOffline(id );
    })



    $(document).on("click", "#btnExcel", function () {

      

        let fechaIni = $("#fechaInicio").val();
        let fechaFin = $("#fechaFin").val();
        let sala = salaId;
        let progre = $("#cboProgresivo").val()
        if (sala == 0) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        ajaxhr = $.ajax({
            type: "POST",
            url: basePath + "Progresivo/ExcelProgresivo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ codSala: sala, codProgresivo: progre, fechaini: fechaIni, fechafin: fechaFin }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                
                 if (response.success) {
                    let data = response.data;
                    let file = response.fileName;
                    let a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/vnd.ms-excel;base64, " + data;
                    a.download = file;
                    a.click();
                }
                else {
                    toastr.error("Error", response.mensaje);
                
                }
            },
            error: function (request, status, error) {
                //toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            }
        });
        AbortRequest.open()

    });


    $(document).on('click', '.btnVerContadores', function (e) {
        e.preventDefault()
        let fecha = $(this).data("fecha");
        let maquina = $(this).data("maquina");
        $("#txtFechaHidden").val(fecha)
        $("#txtMaquinaHidden").val(maquina)
        $("#full-modal-contadores").modal('show')
    })
    $("#full-modal-contadores").on("shown.bs.modal", function () {
        let fecha = $("#txtFechaHidden").val()
        let maquina = $("#txtMaquinaHidden").val()
        let _fecha = new Date(moment(fecha).format())


        $("#txtFechaInicio").datetimepicker({
            format: 'DD-MM-YYYY HH:mm',
            formatTime: 'HH:mm',
            formatDate: 'DD-MM-YYYY',
        });
        $("#txtFechaFin").datetimepicker({
            format: 'DD-MM-YYYY HH:mm',
            formatTime: 'HH:mm',
            formatDate: 'DD-MM-YYYY',
        });
        $("#txtFechaInicio").data("DateTimePicker").setDate(_fecha)
        $("#txtFechaFin").data("DateTimePicker").setDate(_fecha)


        $("#tableContadores").empty()
        $(".myDataTable").empty()
        $(".myDataTable").html(`<table class="table table-striped table-hover table-condensed" id="tableContadores"></table>`)

        $("#txtCodMaq").val(maquina)
        $('table').css('width', '100%');
        $('.dataTables_scrollHeadInner').css('width', '100%');
        $('.dataTables_scrollFootInner').css('width', '100%');
    })
    $(document).on('click', '#btnBuscarContadores', function (e) {
        e.preventDefault()

        getDetalleContadoresOffline(Fecha, maquina, RegA, RegD);
        renderDetalleContadores(response.data)
    })

});
VistaAuditoria("Progresivo/ReporteProgresivoVista", "VISTA", 0, "", 1);


function hiddenTablaGanadores(hidden) {
    if (hidden) {
        $(".tabla__ganadores").addClass("hidden")
    }
    else {
        $(".tabla__ganadores").removeClass("hidden")
    }
}
function resetFiltroGanadores() {
    $("#txtMaquina").val("")
    $("#cboPozos").prop("selectedIndex", 0)
}

function obtenerListaSalas() {
    ajaxhr = $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            renderSelectSalas(result.data);
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
    return ajaxhr
}
function renderSelectSalas(data) {
    $.each(data, function (index, value) {
        $("#cboSala").append(`<option value="${value.UrlProgresivo == "" ? "" : value.UrlProgresivo}" data-id="${value.CodSala}" data-ipprivada="${value.IpPrivada}" data-puertoservicio="${value.PuertoServicioWebOnline}">${value.Nombre}</option>`)
    });
}

function renderSelectProgresivos(data) {
    $("#cboProgresivo").html("")
    if (data) {
        $("#cboProgresivo").append('<option value="">--Seleccione--</option>');
        $.each(data, function (index, value) {
            $("#cboProgresivo").append(`
                <option 
                    value="${value["WEB_PrgID"]}" 
                    data-id="${value["WEB_Url"]}">
                    ${value["WEB_Nombre"]}
                </option>`)
        });
    }
}
/**End Listado Progresivos */



function getReporteProgresivo(codSala, codProgresivo) {
    var fechaIni = $("#fechaInicio").val();
    var fechaFin = $("#fechaFin").val();
     
    ajaxhr = $.ajax({
        data: JSON.stringify({ codSala, codProgresivo, fechaIni, fechaFin }),
        type: "POST",
        cache: false,
        url: basePath + '/Progresivo/CabeceraOfflineListadoJson',
        contentType: "application/json: charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay('show')
        },
        success: function (response) {
            if (response.respuesta) {
                toastr.success("Data encontrada", "Mensaje Servidor");
                renderReporteProgresivo(response.data)
            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
        complete: function (xhr) {
            AbortRequest.close()
            $.LoadingOverlay('hide')
        }
    });

    AbortRequest.open();

}


function renderReporteProgresivo(data) {

    //console.log(data);
    //var RegA = $("#RegA").val();
    //var RegD = $("#RegD").val();
    var RegA = 10;
    var RegD = 10;

    objetodatatabledetalle = $("#table").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": false,
        "paging": true,
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "aaSorting": [],
        data: data,
        columns: [
            {
                data: "TipoPozo", title: "Pozo",
                "render": function (value) {
                    var pozo = "";
                    if (value == 1) {
                        pozo = "Superior";
                    }
                    if (value == 2) {
                        pozo = "Medio ";
                    }
                    if (value == 3) {
                        pozo = "Inferior";
                    }
                    return pozo;
                }
            },
            { data: "SlotID", title: "Maquina" },
            {
                data: "Fecha", title: "Fecha",
                "render": function (value) {
                    return moment(value).format('DD/MM/YYYY');
                }
            },
            {
                data: "Fecha", title: "Hora",
                "render": function (value) {
                    return moment(value).format('h:mm:ss a');
                }
            },
            { data: "Monto", title: "Monto" },
            {
                data: null,
                "render": function (value) {
                    var botones = '<button type="button" class="btn btn-xs btn-success btnVer" data-toggle="modal" data-target="#full-modal" data-RegD="' + RegD + '" data-RegA="' + RegA + '" data-Fecha="' + value.Fecha + '" data-maquina="' + value.SlotID + '" data-id="' + value.IdCabeceraProgresivo + '"><i class="glyphicon  glyphicon-search"></i></button>';
                    //botones += '<button type="button" class="btn btn-xs btn-danger btnVerContadores" data-fecha="' + value.Fecha + '" data-maquina="' + value.SlotID + '" data-id="' + value.ProgresivoID + '"><i class="glyphicon  glyphicon-calendar"></i></button>';
                    return botones;
                },
                sortable: false,
                searchable: false
            }

        ],
        bSort: true,
        "initComplete": function (settings, json) {
        },
    });

}



function getProgresivosOffline(codSala) {
    ajaxhr = $.ajax({
        type: "POST",
        url: basePath + "Progresivo/ProgresivoOfflineListadoJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ codSala}),
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {

            if (result.respuesta) {
                renderSelectProgresivos(result.data);
            } else {
                renderSelectProgresivos([]);
                toastr.error(result.mensaje, "Mensaje Servidor");
            }
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
    return ajaxhr
};

function getDetallesOffline(codCabecera){

    ajaxhr = $.ajax({
        type: "POST",
        url: basePath + "Progresivo/DetalleOfflineListadoJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ codCabecera }),
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {

            if (result.respuesta) {
                renderDetalleContadoresPremio(result.data)
            } else {
                renderDetalleContadoresPremio([])
                toastr.error("No se encontro data", "Mensaje Servidor");
            }
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
    return ajaxhr
    

};


function getDetalleContadoresOffline(id, id2, id3, id4) {

    ajaxhr =  $.ajax({
        type: "POST",
        url: basePath + "Progresivo/DetallesContadoresPremioOfflineListadoJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id, id2, id3, id4 }),
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {

            if (result.respuesta) {
                renderDetalleContadoresPremio(result.data)
            } else {
                renderDetalleContadoresPremio([])
                toastr.error("No se encontro data", "Mensaje Servidor");
            }
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
    return ajaxhr


};

function renderDetalleContadoresPremio(response) {
    if (response) {

        if (response.length > 0) {
            response[response.length - 1].Dif_Bonus1 = 0;
            response[response.length - 1].Dif_Bonus2 = 0;
        }

        objetodatatabledetalle = $("#tableDetalle").DataTable({
            "bDestroy": true,
            "bSort": true,
            "scrollCollapse": true,
            "scrollX": false,
            "paging": true,
            "autoWidth": false,
            "bProcessing": true,
            "bDeferRender": true,
            "aaSorting": [],
            data: response,
            columns: [
                {
                    data: "Fecha", title: "Fecha",
                    "render": function (value) {
                        return moment(value).format('DD/MM/YYYY');
                    }
                },
                {
                    data: "Hora", title: "Hora",
                    "render": function (value) {
                        return moment(value).format('h:mm:ss a');
                    }
                },
                { data: "CodMaq", title: "Maquina" },
                { data: "codevento", title: "Evento" },
                { data: "Bonus1", title: "Bonus 1" },
                { data: "Bonus2", title: "Bonus 2" },
                {
                    data: "Dif_Bonus1", title: "Bonus 1 Diferencia",
                    "render": function (value) {
                        if (value != 0) {
                            return "<p style=background-color:red;color:white;>" + value + "</p>";
                        }
                        else {
                            return value;
                        }
                    }
                },
                {
                    data: "Dif_Bonus2", title: "Bonus 2 Diferencia",
                    "render": function (value) {
                        if (value != 0) {
                            return "<p style=background-color:red;color:white;>" + value + "</p>";
                        }
                        else {
                            return value;
                        }
                    }
                },
            ],
            bSort: false,
            columnDefs: [
                {
                    targets: [0, 1, 2, 3, 4, 5, 6],   //first name & last name
                    orderable: false
                },
            ],
            "initComplete": function (settings, json) {
            },
        });
    }
};



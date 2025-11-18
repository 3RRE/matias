


let objetodatatablePieza;
var array = [];
var arrayProblema = [];
var arrayRepuesto = [];
var arrayRepuestoAnt = [];
var arrayProblemas = [];
var arrayRepuestosPedidos = [];

$(document).ready(function () {

    VistaAuditoria("MIMaquinaInoperativa/MaquinaInoperativaEditarVista", "VISTA", 0, "", 3);

    $(".divRepuesto").attr('style', 'display:none');

    GetSalas();

    //DataTableRepuestoTraspasoUpdate(arrayRepuesto);
    DataTableRepuestoTraspasoUpdate(maquinaInoperativa.CodMaquinaInoperativa);
    //DataTableRepuestoCompraUpdate(arrayRepuesto);

    let _fecha = new Date();

    let _fechaInoperativa = moment(maquinaInoperativa.FechaInoperativa).format('DD/MM/YYYY hh:mm:ss A');

    $(document).on("click", ".btnListar", function () {

        window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativaAtendidaInoperativaSolicitud");

    });


    //SET DATA

    $("#cboMaquina").append('<option value="' + maquinaInoperativa.CodMaquina + '"  >' + maquinaInoperativa.MaquinaLey + '</option>');
    $("#cboMaquina").val(maquinaInoperativa.CodMaquina);
    $("#cboMaquina").change();


    $("#lin").text(maquinaInoperativa.MaquinaLinea);
    $("#numse").text(maquinaInoperativa.MaquinaNumeroSerie);
    $("#jue").text(maquinaInoperativa.MaquinaJuego);
    $("#sal").text(maquinaInoperativa.MaquinaSala);
    $("#mod").text(maquinaInoperativa.MaquinaModelo);
    $("#pro").text(maquinaInoperativa.MaquinaPropietario);
    $("#fic").text(maquinaInoperativa.MaquinaFicha);
    $("#mar").text(maquinaInoperativa.MaquinaMarca);
    $("#tok").text(maquinaInoperativa.MaquinaToken);
    $("#Observaciones").val(maquinaInoperativa.ObservacionCreado);





    $(document).on("click", ".btnAceptar", function () {


        var id = $(this).data("id");
        var idpiezarepuestoalmacen = $(this).data("idpiezarepuestoalmacen");
        var idalmacendestino = $(this).data("idalmacendestino");
        var idrepuesto = $(this).data("idrepuesto");
        var cantidad = $(this).data("cantidad");

        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Aceptar traspaso del repuesto?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                AceptarPeticion(id, idpiezarepuestoalmacen, idalmacendestino, idrepuesto, cantidad);

            },

            cancel: function () {
                //close
            },

        });



    });

    $(document).on("click", ".btnRechazar", function () {


        var id = $(this).data("id");


        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Rechazar traspaso del repuesto?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                RechazarPeticion(id);

            },

            cancel: function () {
                //close
            },

        });
    });

});




function ListarMaquinas(cod) {

    $.ajax({
        type: "POST",
        url: basePath + "MIMaquinaInoperativa/ListarMaquinasAdministrativoxSala",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ cod }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboMaquina").append('<option value="' + value.CodMaquina + '"  >' + value.CodMaquinaLey + '</option>');
            });
            $("#cboMaquina").select2({
                placeholder: "--Seleccione--",

            });
            $("#cboMaquina").val(null).trigger("change");
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
            $("#cboMaquina").val(maquinaInoperativa.CodMaquina);
            $("#cboMaquina").change();
        }
    });
    return false;
}


function GetMaquinaDetalle(codMaquina) {

    $.ajax({
        type: "POST",
        url: basePath + "MIMaquinaInoperativa/ListarMaquinaDetalleAdministrativo",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codMaquina }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var maquina = result.data;
            $("#lin").text(maquina.NombreLinea);
            $("#numse").text(maquina.NroSerie);
            $("#jue").text(maquina.NombreJuego);
            $("#sal").text(maquina.NombreSala);
            $("#mod").text(maquina.NombreModeloMaquina);
            $("#pro").text(maquina.DescripcionContrato);
            $("#fic").text(maquina.NombreFicha);
            $("#mar").text(maquina.NombreMarcaMaquina);
            $("#tok").text(maquina.Token);

            $("#Linea").val(maquina.NombreLinea);
            $("#NroSerie").val(maquina.NroSerie);
            $("#Juego").val(maquina.NombreJuego);
            $("#Sala").val(maquina.NombreSala);
            $("#Modelo").val(maquina.NombreModeloMaquina);
            $("#Propietario").val(maquina.DescripcionContrato);
            $("#Ficha").val(maquina.NombreFicha);
            $("#Marca").val(maquina.NombreMarcaMaquina);
            $("#Token").val(maquina.Token);


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


function DataTablePiezaUpdate(array) {

    objetodatatablePieza = $("#tablePieza").DataTable({
        "bDestroy": true,
        "ordering": false,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": false,
        "aaSorting": [],
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "searching": false,
        "bInfo": false, //Dont display info e.g. "Showing 1 to 4 of 4 entries"
        "paging": false,//Dont want paging                
        "bPaginate": false,//Dont want paging      
        data: array
        ,
        columns: [
            { data: "CodPieza", title: "Cod" },
            { data: "Nombre", title: "Nombre" },
            { data: "Descripcion", title: "Descripcion" },
            { data: "Cantidad", title: "Cantidad" },
            /*
            {
                data: "CodPieza", title: "Accion",
                "bSortable": false,
                "render": function (o, type, oData) {
                    return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
                }
            }*/
        ],
        "drawCallback": function (settings) {
            $('.btnEditar').tooltip({
                title: "Editar"
            });
            $('.btnEliminar').tooltip({
                title: "Eliminar"
            });
        },

        "initComplete": function (settings, json) {



        },
    });
}


function DataTableProblemaUpdate(array) {

    objetodatatableProblema = $("#tableProblema").DataTable({
        "bDestroy": true,
        "ordering": false,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": false,
        "aaSorting": [],
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "searching": false,
        "bInfo": false, //Dont display info e.g. "Showing 1 to 4 of 4 entries"
        "paging": false,//Dont want paging                
        "bPaginate": false,//Dont want paging      
        data: array
        ,
        columns: [
            { data: "CodProblema", title: "Cod" },
            { data: "Nombre", title: "Nombre" },
            { data: "Descripcion", title: "Descripcion" },
            //{ data: "Cantidad", title: "Cantidad" },
            /*
            {
                data: "CodPieza", title: "Accion",
                "bSortable": false,
                "render": function (o, type, oData) {
                    return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
                }
            }*/
        ],
        "drawCallback": function (settings) {
            $('.btnEditar').tooltip({
                title: "Editar"
            });
            $('.btnEliminar').tooltip({
                title: "Eliminar"
            });
        },

        "initComplete": function (settings, json) {



        },
    });
}




function DataTableRepuestoUpdate(arrayRepuesto) {

    objetodatatablePieza = $("#tableRepuesto").DataTable({
        "bDestroy": true,
        "ordering": false,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": false,
        "aaSorting": [],
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "searching": false,
        "bInfo": false, //Dont display info e.g. "Showing 1 to 4 of 4 entries"
        "paging": false,//Dont want paging                
        "bPaginate": false,//Dont want paging      
        data: arrayRepuesto
        ,
        columns: [
            { data: "CodRepuesto", title: "Cod" },
            { data: "Repuesto", title: "Repuesto" },
            { data: "Cantidad", title: "Cantidad" },

            {
                data: "Estado", title: "Acción",
                "bSortable": false,
                "render": function (o, type, oData) {

                    if (oData.Estado == 1) {
                        return `<button type="button" class="btn btn-xs btn-success btnAceptar" data-id="${o}" data-idpiezarepuestoalmacen="${oData.CodPiezaRepuestoAlmacen}" data-cantidad="${oData.Cantidad}" data-idalmacendestino="${oData.CodAlmacenDestino}" data-idrepuesto="${oData.CodRepuesto}"><i class="glyphicon glyphicon-check"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnRechazar" data-id="${o}" ><i class="glyphicon glyphicon-remove"></i></button> `;
                    } else {
                        return ``
                    }

                }
            },/*
            {
                data: "CodRepuesto", title: "Accion",
                "bSortable": false,
                "render": function (o, type, oData) {

                    return `<button type="button" class="btn btn-xs btn-danger btnEliminarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;



                }
            }*/
        ],
        "drawCallback": function (settings) {
            $('.btnAceptar').tooltip({
                title: "Aceptar"
            });
            $('.btnRechazar').tooltip({
                title: "Rechazar"
            });
        },

        "initComplete": function (settings, json) {



        },
    });
}


function DataTableRepuestoCompraUpdate(arrayRepuesto) {

    objetodatatablePieza = $("#tableRepuestoCompra").DataTable({
        "bDestroy": true,
        "ordering": false,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": false,
        "aaSorting": [],
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "searching": false,
        "bInfo": false, //Dont display info e.g. "Showing 1 to 4 of 4 entries"
        "paging": false,//Dont want paging                
        "bPaginate": false,//Dont want paging      
        data: arrayRepuesto
        ,
        columns: [
            { data: "CodRepuesto", title: "Cod" },
            { data: "Repuesto", title: "Repuesto" },
            { data: "Cantidad", title: "Cantidad" },

            {
                data: "Estado", title: "Acción",
                "bSortable": false,
                "render": function (o, type, oData) {

                    if (oData.Estado == 1) {
                        return `<button type="button" class="btn btn-xs btn-success btnVerCompra" data-id="${o}" data-idpiezarepuestoalmacen="${oData.CodPiezaRepuestoAlmacen}" data-cantidad="${oData.Cantidad}" data-idalmacendestino="${oData.CodAlmacenDestino}" data-idrepuesto="${oData.CodRepuesto}"><i class="glyphicon glyphicon-check"></i></button> 
                                    `;
                    } else {
                        return ``
                    }

                }
            },/*
            {
                data: "CodRepuesto", title: "Accion", 
                "bSortable": false,
                "render": function (o, type, oData) {

                    return `<button type="button" class="btn btn-xs btn-danger btnEliminarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;



                }
            }*/
        ],
        "drawCallback": function (settings) {
            $('.btnVerCompra').tooltip({
                title: "Ver compra"
            }); 
        },

        "initComplete": function (settings, json) {



        },
    });
}

function GetSalas() {


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

            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                placeholder: "--Seleccione--"

            });
            $("#cboSala").val(null).trigger("change");
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");

            $("#cboSala").val(maquinaInoperativa.CodSala);
            $("#cboSala").change();
        }
    });
    return false;

}


//METODOS REPUESTO TRASPAS


function AceptarPeticion(CodTraspasoRepuestoAlmacen, CodPiezaRepuestoAlmacen, CodAlmacenDestino, CodRepuesto, Cantidad) {
    var codSala = $("#cboSala").val();
    var url = basePath + "MITraspasoRepuestoAlmacen/TraspasoRepuestoAlmacenAceptarJson";
    var data = {}; var respuesta = "";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ CodTraspasoRepuestoAlmacen, CodPiezaRepuestoAlmacen, CodAlmacenDestino, CodRepuesto, Cantidad, codSala, CodMaquinaInoperativa: maquinaInoperativa.CodMaquinaInoperativa }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
            DataTableRepuestoTraspasoUpdate(maquinaInoperativa.CodMaquinaInoperativa);
        },
        success: function (response) {
            if (response.respuesta) {
                console.log(response.mensaje);
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {

        }
    });
}

function RechazarPeticion(CodTraspasoRepuestoAlmacen) {
    var codSala = $("#cboSala").val();
    var url = basePath + "MITraspasoRepuestoAlmacen/TraspasoRepuestoAlmacenRechazarJson";
    var data = {}; var respuesta = "";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ CodTraspasoRepuestoAlmacen, codSala, CodMaquinaInoperativa: maquinaInoperativa.CodMaquinaInoperativa }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
            DataTableRepuestoTraspasoUpdate(maquinaInoperativa.CodMaquinaInoperativa);
        },
        success: function (response) {
            if (response.respuesta) {
                console.log(response.mensaje);
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {

        }
    });
}

function DataTableRepuestoTraspasoUpdate(cod) {
    var url = basePath + "MITraspasoRepuestoAlmacen/ListarTraspasoRepuestoAlmacenJsonxMaquinaInoperativa";
    var data = { cod } ; var respuesta = "";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            respuesta = response.data
            objetodatatable = $("#tableRepuesto").DataTable({
                "bDestroy": true,
                "bSort": true,
                "ordering": true,
                "scrollCollapse": true,
                "scrollX": true,
                "paging": false,
                "aaSorting": [0],
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                "searching": false,
                "bInfo": false, //Dont display info e.g. "Showing 1 to 4 of 4 entries"
                "paging": false,//Dont want paging                
                "bPaginate": false,//Dont want paging   
                data: response.data,
                columns: [
                    { data: "CodTraspasoRepuestoAlmacen", title: "ID" },
                    { data: "NombreSala", title: "Sala" },
                    { data: "NombreAlmacenOrigen", title: "Almacen Origen" },
                    { data: "NombreAlmacenDestino", title: "Almacen Destino" },
                    { data: "NombreRepuesto", title: "Repuesto" },
                    { data: "Cantidad", title: "Cantidad" },
                    {
                        data: "Estado", title: "Estado",
                        "render": function (o) {
                            var estado = "PENDIENTE";
                            var css = "btn-info";
                            if (o == 2) {
                                estado = "RECHAZADO"
                                css = "btn-danger";
                            }
                            if (o == 1) {
                                estado = "ACEPTADO"
                                css = "btn-success";
                            }
                            return '<span class="label ' + css + '">' + estado + '</span>';

                        }
                    },
                    {
                        data: "FechaRegistro", title: "Fecha Registro",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY hh:mm");
                        }
                    },

                    {
                        data: "CodTraspasoRepuestoAlmacen", title: "Acción",
                        "bSortable": false,
                        "render": function (o, type, oData) {

                            if (oData.Estado == 0) {
                                return `<button type="button" class="btn btn-xs btn-success btnAceptar" data-id="${o}" data-idpiezarepuestoalmacen="${oData.CodPiezaRepuestoAlmacen}" data-cantidad="${oData.Cantidad}" data-idalmacendestino="${oData.CodAlmacenDestino}" data-idrepuesto="${oData.CodRepuesto}"><i class="glyphicon glyphicon-check"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnRechazar" data-id="${o}" ><i class="glyphicon glyphicon-remove"></i></button> `;
                            } else {
                                return ``
                            }

                        }
                    }
                ],
                "drawCallback": function (settings) {
                    $('.btnAceptar').tooltip({
                        title: "Aceptar"
                    });
                    $('.btnRechazar').tooltip({
                        title: "Rechazar"
                    });
                },

                "initComplete": function (settings, json) {



                },
            });

        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {

        }
    });
};





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
    DataTablePiezaUpdate(array);
    DataTableRepuestoUpdate(arrayRepuesto);

    let _fecha = new Date();

    let _fechaInoperativa = moment(maquinaInoperativa.FechaInoperativa).format('DD/MM/YYYY hh:mm:ss A');

    $(document).on("click", ".btnListar", function () {

        window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativaAtendidaOperativa");

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

    $("#cboEstado").change(function () {
        let estado = $(this).val();

        if (estado == 1) {
            $("#cboPrioridad").val(2);
            $(".divRepuesto").attr('style', 'display:none');
            arrayRepuesto = [];
        }
        if (estado == 2) {
            $("#cboPrioridad").val(3);
            $(".divRepuesto").attr('style', 'display:none');
            arrayRepuesto = [];
        }
        if (estado == 3) {
            $("#cboPrioridad").val(2);
            $(".divRepuesto").attr('style', 'display:none');
            $("#tableRepuesto").DataTable().clear().draw();
            $("#tableRepuesto").DataTable().destroy();
            DataTableRepuestoUpdate(arrayRepuesto);
        }
    });




    //SET DATA


    $("#Observaciones").val(maquinaInoperativa.ObservacionCreado);
    $("#cboEstado").val(maquinaInoperativa.CodEstadoInoperativa);
    $("#cboEstado").change();
    $("#Tecnico").val(maquinaInoperativa.TecnicoCreado);


    $("#ObservacionAtencion").val(maquinaInoperativa.ObservacionAtencion);
    $("#cboAtencion").val(1);
    $("#cboAtencion").change();
    $("#TecnicoAtencion").val(maquinaInoperativa.TecnicoAtencion);

    //Problemas

    listaProblemasArray.forEach(function (item, index) {

        var data = { CodProblema: item.CodProblema, Nombre: item.NombreProblema, Descripcion: item.DescripcionProblema };
        arrayProblema.push(data);

    });

    console.log(arrayProblema);

    $("#tableProblema").DataTable().clear().draw();
    $("#tableProblema").DataTable().destroy();
    DataTableProblemaUpdate(arrayProblema);

    //Piezas

    listaPiezas.forEach(function (item, index) {

        var data = { CodPieza: item.CodPieza, Nombre: item.NombrePieza, Descripcion: item.DescripcionPieza, Cantidad: item.Cantidad };
        array.push(data);

    });

    console.log(array);

    $("#tablePieza").DataTable().clear().draw();
    $("#tablePieza").DataTable().destroy();
    DataTablePiezaUpdate(array);

    //Repuestos

    listaRepuestos.forEach(function (item, index) {

        var data = { CodRepuesto: item.CodRepuesto, Repuesto: item.NombreRepuesto, Cantidad: item.Cantidad, Estado: item.Estado };
        arrayRepuesto.push(data);

    });

    console.log(arrayRepuesto);


    listaRepuestos.forEach(function (item, index) {

        var data = { CodRepuesto: item.CodRepuesto, Repuesto: item.NombreRepuesto, Cantidad: item.Cantidad, Estado: item.Estado };
        arrayRepuestoAnt.push(data);

    });

    console.log(arrayRepuestoAnt);


    $("#tableRepuesto").DataTable().clear().draw();
    $("#tableRepuesto").DataTable().destroy();
    DataTableRepuestoUpdate(arrayRepuesto);

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
                data: "Estado", title: "Estado",
                "bSortable": false,
                "render": function (o, type, oData) {


                    if (o == 0) {

                        estado = "EN STOCK"
                        css = "btn-primary";
                        return '<span class="label ' + css + '">' + estado + '</span>';

                    } else

                        if (o == 2) {

                            estado = "ACEPTADO"
                            css = "btn-success";
                            return '<span class="label ' + css + '">' + estado + '</span>';

                        } else
                            if (o == 3) {

                                estado = "RECHAZADO"
                                css = "btn-danger";
                                return '<span class="label ' + css + '">' + estado + '</span>';

                            } else {

                                estado = "PEDIDO"
                                css = "btn-warning";
                                return '<span class="label ' + css + '">' + estado + '</span>';

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


$(document).ready(function () {

    ListarTraspasoRepuestoAlmacen();


    $(document).on("click", ".btnAceptar", function () {


        var id = $(this).data("id");
        var idpiezarepuestoalmacen = $(this).data("idpiezarepuestoalmacen");
        var idalmacendestino = $(this).data("idalmacendestino");
        var idrepuesto = $(this).data("idrepuesto");
        var cantidad = $(this).data("cantidad");

        AceptarPeticion(id, idpiezarepuestoalmacen, idalmacendestino, idrepuesto,cantidad);


    });

    $(document).on("click", ".btnRechazar", function () {


        var id = $(this).data("id");


        RechazarPeticion(id);
    });
});

function AceptarPeticion(CodTraspasoRepuestoAlmacen, CodPiezaRepuestoAlmacen, CodAlmacenDestino, CodRepuesto, Cantidad) {
    var url = basePath + "MITraspasoRepuestoAlmacen/TraspasoRepuestoAlmacenAceptarJson";
    var data = {}; var respuesta = "";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ CodTraspasoRepuestoAlmacen, CodPiezaRepuestoAlmacen,CodAlmacenDestino,CodRepuesto,Cantidad }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
            location.reload();
        },
        success: function (response) {
            console.log("Aceptado");
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {

        }
    });
}

function RechazarPeticion(CodTraspasoRepuestoAlmacen) {
    var url = basePath + "MITraspasoRepuestoAlmacen/TraspasoRepuestoAlmacenRechazarJson";
    var data = {}; var respuesta = "";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({CodTraspasoRepuestoAlmacen}),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
            location.reload();
        },
        success: function (response) {
            console.log("Rechazado");
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {

        }
    });
}

function ListarTraspasoRepuestoAlmacen() {
    var url = basePath + "MITraspasoRepuestoAlmacen/ListarTraspasoRepuestoAlmacenJson";
    var data = {}; var respuesta = "";
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
            objetodatatable = $("#tableTraspasoRepuestoAlmacen").DataTable({
                "bDestroy": true,
                "bSort": true,
                "ordering": true,
                "scrollCollapse": true,
                "scrollX": true,
                "paging": true,
                "aaSorting": [0],
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
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

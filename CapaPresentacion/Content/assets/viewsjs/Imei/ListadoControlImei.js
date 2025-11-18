


const listarControlImei = () => {
    var url = basePath + "ControlImei/ListarPendientesImei";
    var data = {};
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
          /*  if (response.success) {*/
            data = response.data;
                crearTablaControlImei(data);
            //} else {
            //    toastr.error(response.displayMessage);
            //}
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        }
    });
}



const crearTablaControlImei = (data) => {
    objetodatatable = $("#tableControlImei").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": false,
        "sScrollX": "100%",
        "paging": true,
        "ordering": true,
        "aaSorting": [],
        "autoWidth": false,
        "bAutoWidth": true,
        "bProcessing": true,
        "bDeferRender": true,

        data: data,
        columnDefs: [
            {
                targets: [0, 2],
                className: "text-center"
            }
        ],
        columns: [
            { data:"NombreUsuario", title:"Nombre Usuario"},
            {
                data: "IdControlImei", title: "Empleado",
                "render": function (o, type, row) {
                    return `${row.ApellidoPaterno}  ${row.ApellidoMaterno}, ${row.NombreEmpleado}`
                }
            },
          
            { data: "Cargo", title: "Cargo" },
            { data: "Imei", title: "Imei" },
            {
                data: "FechaRegistro", title: "Fecha de Registro",
                "render": function (o) {

                    return moment(o).format("DD/MM/YYYY hh:mm a");
                }
            },
            {
                data: "IdControlImei",
                title: "Acción",
                "bSortable": false,
                "render": function (o, type, row) {
                    let btnEdit = `<button type="button" class="btn btn-xs btn-success btnAceptar" data-nombre="Aceptar" data-id="${o}" data-imei="${row.Imei}" data-empleado="${row.IdEmpleado}"><i class="glyphicon glyphicon-ok"></i>&nbspAceptar</button> `;
                    let btnDelete = `<button type="button" class="btn btn-xs btn-danger btnRechazar" data-nombre="Rechazar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i>&nbspRechazar</button> `;

                    return btnEdit + btnDelete;
                }
            }
        ],
        "drawCallback": function (settings) {
            $('.btnEditar').tooltip({
                title: "Editar Contacto"
            });

            $('.btnEliminar').tooltip({
                title: "Eliminar Contacto"
            });
        },

        "initComplete": function (settings, json) {

            // $('#btnExcel').off("click").on('click', function () {

            //     cabecerasnuevas = [];
            //     definicioncolumnas = [];
            //     var ocultar = [];
            //     ocultar.push("Accion");
            //     funcionbotonesnuevo({
            //         botonobjeto: this, ocultar: ocultar,
            //         tablaobj: objetodatatable,
            //         cabecerasnuevas: cabecerasnuevas,
            //         definicioncolumnas: definicioncolumnas
            //     });
            //     VistaAuditoria("SalaMaestra/SalaMaestravistaExcel", "EXCEL", 0, "", 3);
            // });

        },
    });
    $('.btnEditar').tooltip({
        title: "Editar Contacto"
    });

    $('.btnELiminar').tooltip({
        title: "Eliminar Contacto"
    });
}




$(document).ready(function () {

    VistaAuditoria("ControlImei/ImeiControlVista", "VISTA", 0, "", 3);
    listarControlImei();



    $(document).on('click', '.btnAceptar', function (e) {
        var id = $(this).data("id");
        var imei = $(this).data("imei");
        var empleado = $(this).data("empleado");

        var data = {
            empleadoId: empleado,
            usu_imei: imei,
            idControlImei:id

        };

        $.confirm({
            title: 'Confirmación',
            content: '¿Desea aceptar el imei del empleado?',
            confirmButton: 'Sí',
            cancelButton: 'No',
            confirm: function () {
                $.ajax({
                    url: basePath + "ControlImei/AceptarControlImei",
                    type: "POST",
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    beforeSend: function () {
                        $.LoadingOverlay("show");
                    },
                    complete: function () {
                        $.LoadingOverlay("hide");
                    },
                    success: function (response) {
                        if (response.respuesta) {
                            toastr.success(response.mensaje);
                            listarControlImei();
                        }
                        else {
                            toastr.error(response.mensaje);
                        }
                    }
                })
            },
            cancel: function () {
                toastr.info("Se cancelo la acción");
            }
        });
    });


    $(document).on('click', '.btnRechazar', function (e) {
        var id = $(this).data("id");

        var data = {
            idControlImei: id
        };

        $.confirm({
            title: 'Confirmación',
            content: '¿Desea rechazar este imei?',
            confirmButton: 'Sí',
            cancelButton: 'No',
            confirm: function () {
                $.ajax({
                    url: basePath + "ControlImei/RechazarControlImei",
                    type: "POST",
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    beforeSend: function () {
                        $.LoadingOverlay("show");
                    },
                    complete: function () {
                        $.LoadingOverlay("hide");
                    },
                    success: function (response) {
                        if (response.data) {
                            toastr.success(response.mensaje);
                            listarControlImei();
                        }
                        else {
                            toastr.error(response.mensaje);
                        }
                    }
                })
            },
            cancel: function () {
                toastr.info("Se cancelo la eliminación.");
            }
        });
    });

})
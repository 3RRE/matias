VistaAuditoria("SeguridadProgresivo/PeticionesToken", "VISTA", 0, "", 3);

$(document).ready(function () {

    listarPeticiones();

    $(document).on('click', '#btnReload', function (e) {
        listarPeticiones();
    });

    $(document).on('click', '.btnenviarToken', function (e) {
        var id = $(this).data("id");
        var data = { sgn_id: id }
        var url = basePath + "SeguridadProgresivo/GenerarProgresivoToken";
        $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: 'Mensaje Servidor',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-danger',
            cancelButtonClass: 'btn-warning',
            confirmButton: 'SI',
            cancelButton: 'No',
            content: "¿Esta Seguro de Enviar Token?",
            confirm: function () {
                DataPostSend(url, data, false).done(function (response) {
                    if (response) {
                        if (response.respuesta) {
                            toastr.success("Token Enviado, Correctamente", "Mensaje Servidor");
                            enviartoken(id);
                            //$("#btnReload").click();
                            var datafinal = {
                                usuario_id: id,
                            };
                            dataAuditoriaJSON(3, "SeguridadProgresivo/PeticionesToken", "ENVIO TOKEN", "", datafinal);
                            setTimeout(function () {
                                location.reload();
                            }, 1000);

                        } else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }

                    } else {
                        toastr.error("Error no se logro conectar al servidor", "Mensaje Servidor");
                    }
                }).fail(function (x) {
                    toastr.error("Error Fallo Servidor", "Mensaje Servidor");
                });
            },
            cancel: function () {

            }
        });



    });

    function listarPeticiones() {
        var url = basePath + "SeguridadProgresivo/GetListaPeticiones";
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

                objetodatatable = $("#tablepeticiones").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "scrollCollapse": true,
                    "scrollX": false,
                    "paging": true,
                    "autoWidth": false,
                    "order": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    data: response.data,
                    columns: [
                        { data: "sgn_id", title: "ID" },
                        { data: "UsuarioNombre", title: "Usuario" },
                        {
                            data: "RolNombre", title: "Rol"
                        },
                        {
                            data: "SalaNombre", title: "Sala"
                        },
                        {
                            data: "sgn_conection_id", title: "Conexion"
                        },
                        {
                            data: "sgn_fechaUpdate", title: "Fecha",
                            "bSortable": false,
                            "render": function (o, i, j) {
                                return moment(o).format("DD-MM-YYYY hh:mm:ss a");
                            }
                        },
                        {
                            data: "sgn_id",
                            "bSortable": false,
                            "render": function (o, i, j) {
                                return '<button type="button" class="btn btn-xs btn-warning btnenviarToken" data-id="' + o + '"><i class="fa fa-cog"></i> Generar Token</button>';
                            }
                        }

                    ],
                    "initComplete": function (settings, json) {

                    },
                });



            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
                console.log("errorrrrrrrr");
            }
        });
    };

});

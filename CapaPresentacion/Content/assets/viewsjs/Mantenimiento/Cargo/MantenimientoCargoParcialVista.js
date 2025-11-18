
$(document).ready(function () {

    $("#formCargo")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                Descripcion: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Nombre, Obligatorio'
                        }
                    }
                },
                Estado: {
                    validators: {
                        notEmpty: {
                            message: 'Seleccione Estado, Obligatorio'
                        }
                    }
                }
            }
        })
        .on('success.field.bv', function (e, data) {
            e.preventDefault();
            var $parent = data.element.parents('.form-group');
            // Remove the has-success class
            $parent.removeClass('has-success');
            // Hide the success icon
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();


        });


    $(this).off('click', 'button.btnEditarReg');
    $(document).on('click', 'button.btnEditarReg', function () {
        var data = tableCargo.row($(this).parents('tr')).data();
        $("#txtCodigoCargo").val(data['CargoID']);
        $("#txtNombreCargo").val(data['Descripcion']);
        $("#cboEstadoCargo").val(data['Estado']);
        dataAuditoria(0, "#formCargo", 3);
    });

    $(this).off('click', 'button.btnSeleccionar');
    $(document).on('click', 'button.btnSeleccionar', function () {
        var data = $(this).data('id');
        $("#cboCargo").val(data);
        $("#modalGroup").modal("hide");

    });
    $('#btnNuevoModal').on('click', function (e) {        
        $('#formCargo input,#formCargo select').val('');
    });
    $('#btnGuardarModal').off('click');
    $('#btnGuardarModal').on('click', function (e) {
        $("#formCargo").data('bootstrapValidator').resetForm();
        var validar = $("#formCargo").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var url = "";
            if ($("#txtCodigoCargo").val() == "") {
                setCookie("datainicial", "");
                dataAuditoria(1, "#formCargo", 3, "Empleado/CargoGuardarJson", "NUEVO CARGO");
                url = basePath + "Empleado/CargoGuardarJson";
            } else {
                url = basePath + "Empleado/CargoActualizarJson";
                dataAuditoria(1, "#formCargo", 3, "Empleado/CargoActualizarJson", "ACTUALIZAR CARGO");
            }
            var dataForm = $('#formCargo').serializeFormJSON();
            $.ajax({
                url: url,
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(dataForm),
                beforeSend: function () {
                    $.LoadingOverlay("show");
                },
                complete: function () {
                    $.LoadingOverlay("hide");
                },
                success: function (response) {
                    var respuesta = response.respuesta;
                    if (respuesta === true) {
                        listarCargo();
                        $('#formCargo input,#formCargo select').val('');
                        llenarSelect(basePath + "Empleado/CargoListarJson", {}, "cboCargo", "CargoID", "Descripcion", "");
                        toastr.success("Se Registro Correctamente", "Mensaje Servidor");
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {

                    if (jqXHR.status === 0) {
                        toastr.error("Not connect: Verify Network.", "Mensaje Servidor");
                    } else if (jqXHR.status == 404) {
                        toastr.error("Requested page not found [404]", "Mensaje Servidor");
                    } else if (jqXHR.status == 500) {
                        toastr.error("Internal Server Error [500].", "Mensaje Servidor");
                    } else if (textStatus === 'parsererror') {
                        toastr.error("Requested JSON parse failed.", "Mensaje Servidor");
                    } else if (textStatus === 'timeout') {
                        toastr.error("Time out error.", "Mensaje Servidor");
                    } else if (textStatus === 'abort') {
                        toastr.error("Ajax request aborted.", "Mensaje Servidor");
                    }
                }
            });
        }
    });


});

function listarCargo() {
    var url = basePath + "Empleado/CargoMantenimientoListarJson";

    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: {},
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
            $("[data-toggle='tooltip']").tooltip();
        },
        success: function (response) {

            tableCargo = $("#tableCargo").DataTable({
                "bDestroy": true,
                "bSort": true,
                "paging": true,
                "scrollX": false,
                "sScrollX": "100%",
                "scrollCollapse": true,
                "bProcessing": true,
                "bDeferRender": false,
                "autoWidth": false,
                "bAutoWidth": false,
                "lengthMenu": [[10, 50, 200, -1], [10, 50, 200, "All"]],
                "pageLength": 10,
                data: response.data,
                columns: [
                    { data: "CargoID", title: "Codigo", width: "44px", className: "tdcenter", visible: false },
                    { data: "Descripcion", title: "Nombre" },
                    { data: "EstadoString", title: "Estado" },
                    { data: "Estado", title: "Estado", visible: false },

                    {
                        data: "CargoID",
                        "bSortable": false,
                        "width": "60px",
                        className: "tdcenter",
                        "render": function (o) {
                            return '<button type="button" class="btn btn-xs btn-warning btnEditarReg" data-toggle="tooltip" data-original-title="Editar" data-id="' + o + '"><i class="glyphicon glyphicon-pencil"></i></button> ' +
                                '<button type="button" class="btn btn-xs btn-success btnSeleccionar" data-toggle="tooltip" data-original-title="Seleccionar" data-id="' + o + '"><i class="glyphicon glyphicon-check"></i></button>';
                        }
                    }
                ]
            });


        },
        error: function (jqXHR, textStatus, errorThrown) {

            if (jqXHR.status === 0) {
                toastr.error("Not connect: Verify Network.", "Mensaje Servidor");
            } else if (jqXHR.status == 404) {
                toastr.error("Requested page not found [404]", "Mensaje Servidor");
            } else if (jqXHR.status == 500) {
                toastr.error("Internal Server Error [500].", "Mensaje Servidor");
            } else if (textStatus === 'parsererror') {
                toastr.error("Requested JSON parse failed.", "Mensaje Servidor");
            } else if (textStatus === 'timeout') {
                toastr.error("Time out error.", "Mensaje Servidor");
            } else if (textStatus === 'abort') {
                toastr.error("Ajax request aborted.", "Mensaje Servidor");
            }
        }
    });
};
VistaAuditoria("Empleado/CargoMantenimientoListarJson", "VISTA", 0, "", 3);
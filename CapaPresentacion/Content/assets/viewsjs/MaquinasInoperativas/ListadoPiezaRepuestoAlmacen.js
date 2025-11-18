
var changeAlmacenForEdit = false;
var updateForRegister = false;
var newRegister = false;

$(document).ready(function () {

    var modalSalaChange = true;
    var filtroChange = true;
    var cambiarAlmacen = false;
    var cambiarTipo = false;
    var modalEditarChange = true;
    var codSalaAux = 0;
    var codAlmacenAux = 0;
    var codTipoAux = 0;
    var codPiezaRepuestoAux = 0;


    //ListarPiezaRepuestoxAlmacen();
    ObtenerListaSalas();
    ObtenerListaSalasFiltro();

    $("#textTipo").text("Repuesto");
    llenarSelectTipoPiezaRepuesto(basePath + "MIRepuesto/ListarRepuestoActiveJson", 2, "cboPiezaRepuesto", "full-modal_almacen");


    $("#cboAlmacenFiltro").html("");
    $("#cboAlmacenFiltro").append('<option value="">--Seleccione--</option>');
    $("#cboAlmacenFiltro").removeAttr("disabled");
    $("#cboAlmacen").html("");
    $("#cboAlmacen").append('<option value="">--Seleccione--</option>');
    $("#cboAlmacen").removeAttr("disabled");


    $(document).on("click", ".btnEditar", function () {
        LimpiarFormValidator();
        changeAlmacenForEdit = true;
        newRegister = false;
        var id = $(this).data("id");
        $("#textAlmacen").text("Editar");
        //$("#textTipo").text("Pieza o Repuesto");
        ObtenerRegistro(id);



        $("#full-modal_almacen").modal("show");
    });

    VistaAuditoria("MIPiezaRepuestoAlmacen/ListadoPiezaRepuestoAlmacen", "VISTA", 0, "", 3);


    $(document).on('change', '#cboSalaFiltro', function (e) {

        objetodatatable = $("#tablePiezaRepuestoAlmacen").DataTable({
            "bDestroy": true,
            "bSort": true,
            "ordering": true,
            "scrollCollapse": true,
            "scrollX": true,
            "paging": true,
            "aaSorting": [],
            "autoWidth": false,
            "bProcessing": true,
            "bDeferRender": true,
            data: {},
            columns: [
                { data: "NombreSala", title: "Nombre Sala" },
                { data: "NombreAlmacen", title: "Nombre Almacen" },
                //{ data: "NombreTipo", title: "Nombre Tipo" },
                { data: "NombrePiezaRepuesto", title: "Nombre Pieza Repuesto" },
                { data: "Cantidad", title: "Cantidad" },
                {
                    data: "Estado", title: "Estado",
                    "render": function (o) {
                        var estado = "INACTIVO";
                        var css = "btn-danger";
                        if (o == 1) {
                            estado = "ACTIVO"
                            css = "btn-success";
                        }
                        return '<span class="label ' + css + '">' + estado + '</span>';

                    }
                },
                /*{
                    data: "FechaRegistro", title: "Fecha Registro",
                    "render": function (o) {
                        return moment(o).format("DD/MM/YYYY hh:mm");
                    }
                },*/

                {
                    data: "CodPiezaRepuestoAlmacen", title: "Acción",
                    "bSortable": false,
                    "render": function (o) {
                        return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
                    }
                }
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

        if (!filtroChange) {

            var codSala = $(this).val();
            //console.log(codSala);
            if (!codSala) {
                toastr.error("Sala sin codigo sala", "Mensaje Servidor");
                $("#cboAlmacenFiltro").html("");
                $("#cboAlmacenFiltro").append('<option value="">--Seleccione--</option>');
                $("#cboAlmacenFiltro").removeAttr("disabled");
                return false;
            }
            llenarSelectAlmacenes(basePath + "MIAlmacen/GetAllAlmacenxSala", codSala, "cboAlmacenFiltro", "formfiltro",false);
        }

        filtroChange = false;

    });


    $(document).on('change', '#cboAlmacenFiltro', function (e) {

        objetodatatable = $("#tablePiezaRepuestoAlmacen").DataTable({
            "bDestroy": true,
            "bSort": true,
            "ordering": true,
            "scrollCollapse": true,
            "scrollX": true,
            "paging": true,
            "aaSorting": [],
            "autoWidth": false,
            "bProcessing": true,
            "bDeferRender": true,
            data: {},
            columns: [
                { data: "NombreSala", title: "Nombre Sala" },
                { data: "NombreAlmacen", title: "Nombre Almacen" },
                //{ data: "NombreTipo", title: "Nombre Tipo" },
                { data: "NombrePiezaRepuesto", title: "Nombre Pieza Repuesto" },
                { data: "Cantidad", title: "Cantidad" },
                {
                    data: "Estado", title: "Estado",
                    "render": function (o) {
                        var estado = "INACTIVO";
                        var css = "btn-danger";
                        if (o == 1) {
                            estado = "ACTIVO"
                            css = "btn-success";
                        }
                        return '<span class="label ' + css + '">' + estado + '</span>';

                    }
                },
                /*{
                    data: "FechaRegistro", title: "Fecha Registro",
                    "render": function (o) {
                        return moment(o).format("DD/MM/YYYY hh:mm");
                    }
                },*/

                {
                    data: "CodPiezaRepuestoAlmacen", title: "Acción",
                    "bSortable": false,
                    "render": function (o) {
                        return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
                    }
                }
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


    });

    $(document).on('change', '#cboSala', function (e) {


        if (!modalSalaChange) {

            //console.log("cambio cboSala");
            var codSala = $(this).val();
            //console.log(codSala);

            if (!codSala) {
                toastr.error("Sala sin codigo sala", "Mensaje Servidor");
                $("#cboAlmacen").html("");
                $("#cboAlmacen").append('<option value="">--Seleccione--</option>');
                $("#cboAlmacen").attr("disabled", "disabled");
                return false;
            }

            var codAlmacen = $("#cod_almacen").val();

            llenarSelectAlmacenes(basePath + "MIAlmacen/GetAllAlmacenxSala", codSala, "cboAlmacen", "full-modal_almacen", codAlmacen);

            

        }

        modalSalaChange = false;

    });

    $(document).on('change', '#cboTipo', function (e) {



            console.log("cambio cboTipo");
        var codTipo = $(this).val();

            /*
            var codAlmacen = $("#cboAlmacen").val();

            if (!codAlmacen) {
                toastr.error("Seleccione Almacen", "Mensaje Servidor");
            }
            */
        if (codTipo == 0) {


            //$("#textTipo").text("Pieza o repuesto");
            $("#cboPiezaRepuesto").html("");
            //$("#cboPiezaRepuesto").append('<option value="">No hay registros</option>');

        }
        if (codTipo == 1) {



            $("#textTipo").text("Pieza");
                llenarSelectTipoPiezaRepuesto(basePath + "MIPieza/ListarPiezaActiveJson", codTipo, "cboPiezaRepuesto", "full-modal_almacen");
            }

        if (codTipo == 2) {
            $("#textTipo").text("Repuesto");
                llenarSelectTipoPiezaRepuesto(basePath + "MIRepuesto/ListarRepuestoActiveJson", codTipo, "cboPiezaRepuesto", "full-modal_almacen");
            }

    });

    $('#btnNuevo').on('click', function (e) {

        LimpiarFormValidator();
        modalSalaChange = true;
        changeAlmacenForEdit = false;
        newRegister = true;


        $("#cboSala").removeAttr("disabled");
        $("#cboAlmacen").removeAttr("disabled");
        $("#cboPiezaRepuesto").removeAttr("disabled");

        $("#textAlmacen").text("Nuevo");

        $("#cod_pieza_repuesto_almacen").val(0);
        $("#cod_almacen").val(0);
        $("#cboSala").val(null).trigger("change");
        $("#cboAlmacen").val(null).trigger("change");

        $("#cboAlmacen").html("");
        $("#cboAlmacen").append('<option value="">--Seleccione--</option>');
        $("#cboAlmacen").attr("disabled", "disabled");

        $("#cboPiezaRepuesto").val(null).trigger("change");
        //$("#cboTipo").val(0).trigger("change");
        $("#cantidad").val("");


        $("#full-modal_almacen").modal("show");
    });

    $(document).on("click", ".btnEliminar", function () {
        var elemento = $(this);
        var cod = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar este Repuesto de este almacen?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "MIPiezaRepuestoAlmacen/PiezaRepuestoAlmacenEliminarJson",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({ cod }),
                    beforeSend: function () {
                        $.LoadingOverlay("show");
                    },
                    complete: function () {
                        $.LoadingOverlay("hide");
                    },
                    success: function (response) {
                        if (response.respuesta) {
                            ListarPiezaRepuestoxAlmacen();
                            toastr.success(response.mensaje, "Mensaje Servidor");
                            VistaAuditoria("MIPiezaRepuestoAlmacen/EliminarPiezaRepuestoAlmacen", "VISTA", 0, "", 3);
                        }
                        else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrow) {
                        toastr.error("Error Servidor", "Mensaje Servidor");
                    }
                });

            },

            cancel: function () {
                //close
            },

        });



    });


    $("#btnBuscar").on("click", function () {
        if ($("#cboSalaFiltro").val() == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if ($("#cboAlmacenFiltro").val() == null) {
            toastr.error("Seleccione Almacen", "Mensaje Servidor");
            return false;
        }
        //if ($("#cboTipoFiltro").val() == null) {
        //    toastr.error("Seleccione Tipo", "Mensaje Servidor");
        //    return false;
        //}
        newRegister = false;
        ListarPiezaRepuestoxAlmacen();
    });

    $("#form_registro_almacen")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                codSala: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                codAlmacen: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                codTipo: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                cantidad: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                codPiezaRepuesto: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },


            }
        })
        .on('success.field.bv', function (e, data) {
            e.preventDefault();
            var $parent = data.element.parents('.form-group');
            $parent.removeClass('has-success');
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
        });

    $('.btnGuardar').on('click', function (e) {


        if (!$("#cboAlmacen").val()) {

            toastr.error(" Almacen No Seleccionado", "Mensaje Servidor");
            return false;
        }

        $("#form_registro_almacen").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_almacen").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#cod_pieza_repuesto_almacen").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "MIPiezaRepuestoAlmacen/PiezaRepuestoAlmacenEditarJson";
                accion = "ACTUALIZAR PIEZA REPUESTO ALMACEN";
                urlenvio = basePath + "MIPiezaRepuestoAlmacen/PiezaRepuestoAlmacenEditarJson";
                VistaAuditoria("MIPiezaRepuestoAlmacen/EditarPiezaRepuestoAlmacen", "VISTA", 0, "", 3);
            }
            else {
                lugar = "MIPiezaRepuestoAlmacen/PiezaRepuestoAlmacenGuardarJson";
                accion = "NUEVA PIEZA REPUESTO ALMACEN";
                urlenvio = basePath + "MIPiezaRepuestoAlmacen/PiezaRepuestoAlmacenGuardarJson";
                VistaAuditoria("MIPiezaRepuestoAlmacen/NuevoPiezaRepuestoAlmacen", "VISTA", 0, "", 3);
            }

            var dataForm = $('#form_registro_almacen').serializeFormJSON();

            $.ajax({
                url: urlenvio,
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(dataForm),
                beforeSend: function () {
                    $.LoadingOverlay("show");
                },
                complete: function () {
                    $.LoadingOverlay("hide");
                    updateForRegister = true;
                    ListarPiezaRepuestoxAlmacen();
                },
                success: function (response) {
                    if (response.respuesta) {
                        modalSalaChange = true;
                        $("#cod_pieza_repuesto_almacen").val(0);
                        $("#cboSala").val(null).trigger("change");
                        $("#cboAlmacen").val(null).trigger("change");
                        $("#cboPiezaRepuesto").val(null).trigger("change");
                        //$("#cboTipo").val("");
                        $("#cantidad").val("");
                        $("#full-modal_almacen").modal("hide");
                        toastr.success(" Almacen Guardada", "Mensaje Servidor");
                    }
                    else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    toastr.error("Error Servidor", "Mensaje Servidor");
                }
            });

        }

    });


    $(document).on("click", "#btnExcel", function () {
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "MIPiezaRepuestoAlmacen/PiezaRepuestoAlmacenDescargarExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
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
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });

});

function ListarPiezaRepuestoxAlmacen() {
    var url = basePath + "MIPiezaRepuestoAlmacen/ListarPiezaRepuestoAlmacenxTipoxAlmacenJson";
    var data = {}; var respuesta = "";

    var codSala = $("#cboSalaFiltro").val();
    var codTipo = $("#cboTipoFiltro").val();
    var codAlmacen = $("#cboAlmacenFiltro").val();

    if (newRegister) {
        return false;
    }

    if (!codAlmacen) {
        toastr.error("Seleccione Almacen", "Mensaje Servidor");
        return false;
    }
    if (!codTipo) {
        toastr.error("Seleccione Tipo", "Mensaje Servidor");
        return false;
    }

    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ codTipo, codAlmacen }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            respuesta = response.data
            objetodatatable = $("#tablePiezaRepuestoAlmacen").DataTable({
                "bDestroy": true,
                "bSort": true,
                "ordering": true,
                "scrollCollapse": true,
                "scrollX": true,
                "paging": true,
                "aaSorting": [],
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                data: response.data,
                columns: [
                    { data: "NombreSala", title: "Nombre Sala" },
                    { data: "NombreAlmacen", title: "Nombre Almacen" },
                    //{ data: "NombreTipo", title: "Nombre Tipo" },
                    { data: "NombrePiezaRepuesto", title: "Nombre Repuesto" },
                    { data: "Cantidad", title: "Cantidad" },
                    {
                        data: "Estado", title: "Estado",
                        "render": function (o) {
                            var estado = "INACTIVO";
                            var css = "btn-danger";
                            if (o == 1) {
                                estado = "ACTIVO"
                                css = "btn-success";
                            }
                            return '<span class="label ' + css + '">' + estado + '</span>';

                        }
                    },
                    /*{
                        data: "FechaRegistro", title: "Fecha Registro",
                        "render": function (o) {
                            return moment(o).format("DD/MM/YYYY hh:mm");
                        }
                    },*/

                    {
                        data: "CodPiezaRepuestoAlmacen", title: "Acción",
                        "bSortable": false,
                        "render": function (o) {
                            return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
                                    <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
                        }
                    }
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

        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {

            toastr.error("Error "+errorThrow, "Mensaje Servidor");
        }
    });
};

function ObtenerRegistro(id) {

    codSalaAux = 0;
    codAlmacenAux = 0;
    codTipoAux = 0;
    codPiezaRepuestoAux = 0;

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "MIPiezaRepuestoAlmacen/ListarPiezaRepuestoAlmacenCodJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodPiezaRepuestoAlmacen: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {

                response = response.data;


                $("#cod_pieza_repuesto_almacen").val(response.CodPiezaRepuestoAlmacen);
                $("#cod_almacen").val(response.CodAlmacen);
                $("#cantidad").val(response.Cantidad);

                //codTipoAux = response.CodTipo;
                //$("#cboTipo").val(response.CodTipo).trigger("change");

                //codSalaAux = response.codSala;
                $("#cboSala").val(response.CodSala).trigger("change");

                codAlmacenAux = response.CodAlmacen;
                //$("#cboAlmacen").val(response.CodAlmacen).trigger("change");

                codPiezaRepuestoAux = response.CodPiezaRepuesto;
                $("#cboPiezaRepuesto").val(response.CodPiezaRepuesto).trigger("change");

                cambiarAlmacen = true;
                cambiarTipo = true;


                $("#cboSala").attr("disabled", "disabled");

                $("#cboAlmacen").attr("disabled", "disabled");

                $("#cboPiezaRepuesto").attr("disabled", "disabled");

            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};



function ObtenerListaSalas() {


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
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_almacen')

            });
            $("#cboSala").val(null).trigger("change");
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


function ObtenerListaSalasFiltro() {


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
                $("#cboSalaFiltro").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSalaFiltro").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#formfiltro')

            });
            $("#cboSalaFiltro").val(null).trigger("change");
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

function llenarSelectAlmacenes(url, codSala, select, filtro, codAlmacen) {

    if (!codSala) {
        toastr.error("No se Declaro codigo Sala", "Mensaje Servidor");
        return false;
    }
    var mensaje = true;
    var cantAlmacenes = 0;
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify({ codSala }),
        contentType: "application/json",
        beforeSend: function () {
            $("#" + select).html("");
            $("#" + select).append('<option value="">Cargando...</option>');
            $("#" + select).attr("disabled", "disabled");
            //$.LoadingOverlay("show");
        },
        success: function (response) {

            var datos = response.data;
            if (datos.length > 0) {
                $.each(datos, function (index, value) {
                    $("#" + select).append('<option value="' + value.CodAlmacen + '"  >' + value.Nombre + '</option>');
                });
                $("#" + select).select2({
                    placeholder: "--Seleccione--", dropdownParent: $("#" + filtro)

                });
                $("#" + select).val(null).trigger("change");
                $("#" + select).removeAttr("disabled", "disabled");
                cantAlmacenes = datos.length;
                toastr.success(cantAlmacenes + " almacen(es) encontrado(s)", "Mensaje Servidor");
            } else {
                toastr.error("Sala sin almacenes", "Mensaje Servidor");
            }


            //if (cambiarAlmacen) {
            //    //console.log("Llenando Almacenes");
            //    $("#cboAlmacen").val(codAlmacenAux).trigger("change");
            //    codAlmacenAux = 0;
            //    cambiarAlmacen = false;
            //    //$("#cboTipo").val(codTipoAux);
            //}

        },
        complete: function () {
            //toastr.success(cantAlmacenes + " almacenes encontrados", "Mensaje Servidor");
            if (changeAlmacenForEdit) {
                $("#cboAlmacen").val(codAlmacen).trigger("change");
                $("#" + select).attr("disabled", "disabled");
                changeAlmacenForEdit = false;
            } else {
                //$("cboAlmacen").html("");
                //$("cboAlmacen").append('<option value="">--Seleccione--</option>');
                //$("cboAlmacen").removeAttr("disabled");
                $("#cboAlmacen").val(null).trigger("change");
            }
            LimpiarFormValidator();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mensaje = false;
            toastr.error("Error al listar almacenes", "Mensaje Servidor");
            $("#" + select).html("");
            $("#" + select).append('<option value="">--Seleccione--</option>');
            $("#" + select).removeAttr("disabled");
        }
    });
    return mensaje;
}


function llenarSelectTipoPiezaRepuesto(url,codTipo, select, filtro) {

    var mensaje = true;
    var cantAlmacenes = 0;
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify({}),
        contentType: "application/json",
        beforeSend: function () {
            $("#" + select).html("");
            $("#" + select).append('<option value="">Cargando...</option>');
            $("#" + select).attr("disabled", "disabled");
            //$.LoadingOverlay("show");
        },
        success: function (response) {

            var datos = response.data;
            if (datos.length > 0) {


                if (codTipo == 1) {

                    $.each(datos, function (index, value) {
                        $("#" + select).append('<option value="' + value.CodPieza + '"  >' + value.Nombre + '</option>');
                    });
                    $("#" + select).select2({
                        placeholder: "--Seleccione--", dropdownParent: $("#" + filtro)

                    });
                    $("#" + select).val(null).trigger("change");
                    $("#" + select).removeAttr("disabled");
                }
                if (codTipo == 2) {

                    $.each(datos, function (index, value) {
                        $("#" + select).append('<option value="' + value.CodRepuesto + '"  >' + value.Nombre + '</option>');
                    });
                    $("#" + select).select2({
                        placeholder: "--Seleccione--", dropdownParent: $("#" + filtro)

                    });
                    $("#" + select).val(null).trigger("change");
                    $("#" + select).removeAttr("disabled");
                }


                $("#cboPiezaRepuesto").val(null).trigger("change");

                //if (cambiarTipo) {
                //    //console.log("Llenando Almacenes");
                //    $("#cboPiezaRepuesto").val(codPiezaRepuestoAux).trigger("change");
                //    codPiezaRepuestoAux = 0;
                //    cambiarTipo = false;
                //    //$("#cboTipo").val(codTipoAux);
                //}


            } else {
                toastr.error("Sala sin almacenes", "Mensaje Servidor");
            }
        },
        complete: function () {
            //toastr.success(cantAlmacenes + " almacenes encontrados", "Mensaje Servidor");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mensaje = false;
            toastr.error("Error al listar almacenes", "Mensaje Servidor");
            $("#" + select).html("");
            $("#" + select).append('<option value="">--Seleccione--</option>');
            $("#" + select).removeAttr("disabled");
        }
    });
    return mensaje;
}

function LimpiarFormValidator() {
    $("#form_registro_almacen").parent().find('div').removeClass("has-error");
    $("#form_registro_almacen").parent().find('i').removeAttr("style").hide();
}
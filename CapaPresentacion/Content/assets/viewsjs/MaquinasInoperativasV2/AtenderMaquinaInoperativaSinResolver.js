


var array = [];
var arrayProblema = [];
var arrayProblemas = [];
var arrayProblemasNuevo = [];
var listaCategoriasRepuesto = []


$(document).ready(function () {
    obtenerCorreos().done(response => {
        if (response.data) {
            renderCorreos(response.data)
        }
    })

    VistaAuditoria("MIMaquinaInoperativa/MaquinaInoperativaAtenderSolicitudVista", "VISTA", 0, "", 3);



    //console.log("Estado:" + maquinaInoperativa.Estado);


    GetSalas();
    LoadClasificacionProblemas();
    DataTableProblemaUpdate(arrayProblema);
    DataTableProblemaUpdate(arrayProblemasNuevo);
    //LoadClasificacionRepuesto();

    $(document).on("click", ".btnListar", function () {

        window.location.assign(basePath + "MaquinasInoperativasV2/ListaMaquinasInoperativasSinResolver");

    });

    //SET DATA
    $("#cboEstado").val(maquinaInoperativa.CodEstadoInoperativa);
    $("#cboEstado").change();

    $("#cboPrioridad").val(maquinaInoperativa.CodPrioridad);
    $("#cboPrioridad").change();

    $("#cboSala").val(maquinaInoperativa.CodSala);
    $("#cboSala").change();

    $("#cboMaquina").append('<option value="' + maquinaInoperativa.CodMaquina + '"  >' + maquinaInoperativa.MaquinaLey + '</option>');

    $("#cboMaquina").val(maquinaInoperativa.CodMaquina);
    $("#cboMaquina").change();

    //$("#Tecnico").val(maquinaInoperativa.TecnicoCreado);
    $("#TecnicoAtencion").val(maquinaInoperativa.TecnicoAtencion);
    $("#Observaciones").val(maquinaInoperativa.ObservacionCreado);
    $("#ObservacionAtencion").val(maquinaInoperativa.ObservacionAtencion);
    $("#IST").val(maquinaInoperativa.IST);
    $("#OrdenCompra").val(maquinaInoperativa.OrdenCompra.trim() == '' ? 'No tiene' : maquinaInoperativa.OrdenCompra);

    $("#cboAtencion").val(2);
    $("#cboSala").change();

    $("#lin").text(maquinaInoperativa.MaquinaLinea);
    $("#numse").text(maquinaInoperativa.MaquinaNumeroSerie);
    $("#jue").text(maquinaInoperativa.MaquinaJuego);
    $("#sal").text(maquinaInoperativa.MaquinaSala);
    $("#mod").text(maquinaInoperativa.MaquinaModelo);
    $("#pro").text(maquinaInoperativa.MaquinaPropietario);
    $("#fic").text(maquinaInoperativa.MaquinaFicha);
    $("#mar").text(maquinaInoperativa.MaquinaMarca);
    $("#tok").text(maquinaInoperativa.MaquinaToken);



    $("#cboClasificacionProblemas").change(function () {

        arrayProblemas = $("#cboListaProblemas").val();

        let cod = $(this).val();
        if (cod) {
            $("#cboListaProblemas").empty();
            LoadListaProblemas(cod);
        }
    });

    $(document).on('change', '.selectRepuesto', function () {
        const $select = $(this);
        const $td = $select.closest('td');
        const $tr = $select.closest('tr');
        const index = $tr.index();

        if ($select.val()) {
            $td.removeClass('has-error has-feedback');
        } else {
            $td.addClass('has-error has-feedback');
        }
    });


    $("#frmRegistroMaquinaInoperativa")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                Atencion: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                ObservacionAtencionNuevo: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                }


            }
        })
        .on('success.field.bv', function (e, data) {
            e.preventDefault();
            var $parent = data.element.parents('.form-group');
            $parent.removeClass('has-success');
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
        });

    $(document).on("click", ".btnAtender", function (e) {
        e.preventDefault();


        $("#frmRegistroMaquinaInoperativa").data('bootstrapValidator').resetForm();
        var validar = $("#frmRegistroMaquinaInoperativa").data('bootstrapValidator').validate();

        let valido = true;

        $('.selectRepuesto').each(function () {
            const $td = $(this).closest('td');

            if (!$(this).val()) {
                valido = false;
                $td.addClass('has-feedback has-error'); // Aplica al <td>
            } else {
                $td.removeClass('has-feedback has-error');
            }
        });

        if (!valido) {
            // alert("Debe seleccionar un repuesto en todas las filas.");
            return;
        }


        if (validar.isValid() && valido) {


            let listaCorreos = $("#cboCorreos").val();
            let AtencionNuevo = $("#cboAtencionNuevo").val();
            let ObservacionesAtencionNuevo = $("#ObservacionAtencionNuevo").val();
            let CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
            let listaProblemaRepuestos = obtenerListaProblemaRepuestos();
            let CodSala = $("#cboSala").val();

            $.ajax({

                type: "POST",
                url: basePath + "MaquinasInoperativasV2/AtenderMaquinaInoperativa",
                cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ CodMaquinaInoperativa, ObservacionesAtencionNuevo, AtencionNuevo, listaProblemaRepuestos, listaCorreos }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (result) {
                    if (result.respuesta) {

                        if ($("#cboCorreos").val()) {
                            enviarCorreoProcesoMaquinaInoperativa(CodSala);
                        }
                        toastr.success(result.mensaje, "Mensaje Servidor");
                        window.location.assign(basePath + "MaquinasInoperativasV2/ListaMaquinasInoperativasSinResolver");
                    } else {
                        toastr.error(result.mensaje, "Mensaje Servidor");
                    }

                },
                error: function (request, status, error) {
                    console.log('error')
                    toastr.error("Error", "Mensaje Servidor");
                },
                complete: function (resul) {
                    $.LoadingOverlay("hide");
                }
            });
        }


        //if (validar.isValid()) {

        //let CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
        //let CodMaquina = $("#cboMaquina").val();
        //let CodMaquinaLey = $("#cboMaquina option:selected").text();
        //let CodSala = $("#cboSala").val();
        //let ObservacionInoperativa = $("#Observaciones").val();
        //let CodEstadoInoperativa = $("#cboEstado").val();
        //let CodPrioridad = $("#cboPrioridad").val();
        //let SoloFechaInoperativa = $("#fechaInoperativa").val();
        //let SoloHoraInoperativa = $("#horaInoperativa").val();
        //let Tecnico = $("#Tecnico").val();
        //let CodEstadoProceso = 1;


        //console.log("Guardando");


    });





    //Problemas

    listaProblemasArray.forEach(function (item, index) {

        var data = { CodProblema: item.CodProblema, Nombre: item.NombreProblema, Descripcion: item.DescripcionProblema };
        arrayProblema.push(data);

    });

    listaProblemasArrayNuevo.forEach(function (item, index) {

        var data = { CodMaquinaInoperativaProblemas: item.CodMaquinaInoperativaProblemas, CodProblema: item.CodProblema, Nombre: item.NombreProblema, Descripcion: item.DescripcionProblema };
        arrayProblemasNuevo.push(data);

    });

    //console.log(arrayProblema);

    $("#tableProblema").DataTable().clear().draw();
    $("#tableProblema").DataTable().destroy();
    DataTableProblemaUpdate(arrayProblema);

    $("#tableProblemaNuevo").DataTable().clear().draw();
    $("#tableProblemaNuevo").DataTable().destroy();
    DataTableProblemaNuevoUpdate(arrayProblemasNuevo);

    $("#tableProblemaRepuesto").DataTable().clear().draw();
    $("#tableProblemaRepuesto").DataTable().destroy();
    DatatableProblemaRepuestoUpdate(arrayProblemasNuevo);


    //FILTROS DATATABLE

    $('input[type="checkbox"]').click(function () {
        $('input[type="checkbox"]').not(this).prop('checked', false);
    });

    $('input[type="checkbox"]').change(function () {
        if (!$(this).prop('checked')) {
            objetodatatable1.search('').draw()
            //console.log("se descarmaco")

        }
    });

    $('#filtroPropio').on('change', function () {
        if (!$(this).prop('checked')) {
            objetodatatable1.search('').draw()
            //console.log("se descarmaco")
        }
        objetodatatable1.search('propio').draw()
    })

    $('#filtroOtro').on('change', function () {
        objetodatatable1.search('otro').draw()

    })
    $('#filtroTodos').on('change', function () {
        objetodatatable1.search('').draw()

    })

});




const obtenerCorreos = () => {

    return $.ajax({
        type: "POST",
        url: `${basePath}/MIMaquinaInoperativa/ListarUsuarioCorreos`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            return response
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })


}

function renderCorreos(data) {

    var element = "#cboCorreos"
    data.forEach(item => {
        $(element).append(`<option value="${item.CodUsuario}">${item.Mail} (${item.UsuarioNombre})</option>`)
    })

    jQuery(element).multiselect({
        multiple: true,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        maxHeight: 400,
        buttonContainer: '<div></div>',
        buttonClass: '',
        templates: {
            button: '<div class="form-control form-multiselect input-sm multiselect" data-toggle="dropdown"><span class="multiselect-selected-text"></span></div>'
        },
        nonSelectedText: '--Seleccione--',
        nSelectedText: 'seleccionados',
        allSelectedText: 'Todo seleccionado',
        selectAllText: 'Seleccionar todos'
    })
}

function LoadClasificacionProblemas() {

    $.ajax({
        type: "POST",
        url: basePath + "MICategoriaProblema/ListarCategoriaProblemaActiveJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboClasificacionProblemas").append('<option value="' + value.CodCategoriaProblema + '"  >' + value.Nombre + '</option>');
            });
            $("#cboClasificacionProblemas").select2({
                multiple: true, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboClasificacionProblemas").val(null).trigger("change");
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");


            $("#cboClasificacionProblemas").val(listaCategoriaProblemas);
            $("#cboClasificacionProblemas").change();

        }
    });
    return false;
}


function LoadListaProblemas(lista) {


    $.ajax({
        type: "POST",
        url: basePath + "MIProblema/ListarProblemaxCategoriaListaJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ listaCategoriaProblema: lista }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboListaProblemas").append('<option value="' + value.CodProblema + '"  >' + value.Nombre + '</option>');
            });
            $("#cboListaProblemas").select2({
                multiple: true, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboListaProblemas").val(null).trigger("change");
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");

            $("#cboListaProblemas").val(listaProblemas);
            $("#cboListaProblemas").change();
        }
    });
    return false;
}

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

function LoadClasificacionRepuesto(apiTabla) {
    $.ajax({
        type: "POST",
        url: basePath + "MICategoriaRepuesto/ListarCategoriaRepuestoActiveJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            listaCategoriasRepuesto = datos; // Guardamos los datos en la variable global
            apiTabla.column(2).nodes().each(function (node, index) {
                var select = $(node).find('.selectRepuesto');
                if (select.length > 0) {
                    select.empty(); // Limpiamos las opciones existentes
                    select.append('<option value="">--Seleccione--</option>'); // Agregamos una opción por defecto
                    $.each(listaCategoriasRepuesto, function (key, value) {
                        select.append('<option value="' + value.Nombre + '">' + value.Nombre + '</option>');
                    });

                    // Si la fila ya tiene un valor para "Repuesto", lo seleccionamos
                    if (apiTabla.row(index).data().Repuesto) {
                        select.val(apiTabla.row(index).data().Repuesto);
                    }
                }
            });
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            apiTabla.columns.adjust().draw();
            $.LoadingOverlay("hide");
            // Ya no necesitamos manipular el cboCategoriaRepuesto aquí
        }
    });
    return false;
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


function DataTableProblemaNuevoUpdate(array) {

    objetodatatableProblema = $("#tableProblemaNuevo").DataTable({
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

function DatatableProblemaRepuestoUpdate(array) {

    objetodatatableProblemaRepuesto = $("#tableProblemaRepuesto").DataTable({
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
        data: array,
        columns: [
            { data: "CodMaquinaInoperativaProblemas", title: "Cod Maquina-Problema" },
            { data: "Nombre", title: "Problema" },
            {
                data: "Repuesto",
                title: "Nombre de Repuesto",
                render: function (data, type, row) {
                    if (type === 'display') {
                        var select = $('<select id="cboCategoriaRepuesto" class="form-control selectRepuesto"></select>');
                        // Aquí llenaremos las opciones del select dinámicamente
                        return select.prop('outerHTML');
                    }
                    return data; 
                }
            },
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
            var api = this.api();
            LoadClasificacionRepuesto(api);
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
        data: arrayRepuesto,
        columns: [
            { data: "CodPiezaRepuestoAlmacen", title: "Cod" },
            { data: "NombreAlmacen", title: "Almacen" },
            { data: "NombreAlmacenDestino", title: "Almacen Destino" },
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
            },
            {
                data: "CodPiezaRepuestoAlmacen", title: "Acciones",
                "bSortable": false,
                "render": function (o, type, oData) {

                    if (oData.Enviar) {

                        return `<button type="button" class="btn btn-xs btn-danger btnEliminarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;

                    } else {
                        return ``
                    }
                    //else {

                    //    return `<button type="button" class="btn btn-xs btn-danger btnEliminarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;


                    //}

                    /*
                    if (maquinaInoperativa.Estado == 4 || maquinaInoperativa.Estado == 2) {
                        if (oData.Enviar) {

                            return `<button type="button" class="btn btn-xs btn-danger btnEliminarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;

                        } else {

                            return ``;

                        }
                    } else {

                        return `<button type="button" class="btn btn-xs btn-danger btnEliminarRepuesto" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;

                    }
                    */





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




function LimpiarFormValidator() {
    $("#frmRegistroMaquinaInoperativa").parent().find('div').removeClass("has-error");
    $("#frmRegistroMaquinaInoperativa").parent().find('i').removeAttr("style").hide();
    $("#frmRegistroMaquinaInoperativa").parent().find('.fa').removeAttr("style").show();
}



function AtenderDeTodasFormas() {

    let CodMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "MITraspasoRepuestoAlmacen/AtencionPendienteEditarMaquinaInoperativa",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodMaquinaInoperativa }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {

                toastr.success(response.mensaje, "Mensaje Servidor");
                window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativaAtencion");
                $(".btnAtenderIgual").attr('style', 'display:none');

            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}






function enviarCorreoProcesoMaquinaInoperativa(CodSala) {



    $.ajax({
        type: "POST",
        url: basePath + "MIMaquinaInoperativa/EnviarCorreoMaquinaInoperativa",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ CodSala, CodTipo: 4 }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {

            if (result.respuesta) {


                toastr.success(result.mensaje, "Mensaje Servidor");
                console.log("Correos Enviados");

            } else {

                console.log(result.mensaje);
            }




        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function obtenerListaProblemaRepuestos() {
    
    var lista = [];
    $('#tableProblemaRepuesto tbody tr').each(function () {
        var $fila = $(this);
        var data = $('#tableProblemaRepuesto').DataTable().row($fila).data();

        var repuestoSeleccionado = $fila.find('select.selectRepuesto').val(); // obtener el valor seleccionado

        lista.push({
            CodMaquinaInoperativaProblemas: data.CodMaquinaInoperativaProblemas,
            Problema: data.Nombre,
            NombreRepuesto: repuestoSeleccionado // este es el valor del combo
        });
    });

    console.log(lista);
    return lista;
}
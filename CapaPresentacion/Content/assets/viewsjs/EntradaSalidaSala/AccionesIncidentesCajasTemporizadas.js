
//19-11 js
let objetodatatable
let objetodatatable2
let arraySalas = []
let arrayEmpleados = []
let arrayEmpleadosSeleccionados = []
let IdAccionCajaTemporizada = 0
let CboSeleccionado = 0
let CboSeleccionado1 = 0
let CboSeleccionado2 = 0
let CboSeleccionado3 = 0
let params = {}

let idIngresoSeleccionado = null;
$(document).ready(function () {
    ObtenerListaSalas()
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
    })

    $('#MedidaAdoptada').on('input', function () {
        var maxLengthMedidaAdoptada = $(this).attr('maxlength');
        var currentLengthMedidaAdoptada = $(this).val().length;
        $('#charCountMedidaAdoptada').text(currentLengthMedidaAdoptada + '/' + maxLengthMedidaAdoptada);
    })

    $('#DispositivoOtros').on('input', function () {
        var maxLengthDispositivoOtros = $(this).attr('maxlength');
        var currentLengthDispositivoOtros = $(this).val().length;
        $('#charCountDispositivoOtros').text(currentLengthDispositivoOtros + '/' + maxLengthDispositivoOtros);
    })

    let urlParams = new URLSearchParams(window.location.search);
    let codsala = urlParams.get('codsala');
    let ider = urlParams.get('id');
    let fechaini = urlParams.get('fechaInicio');
    let fechafin = urlParams.get('fechaFin');

    if (codsala) {
        ObtenerListaSalas(codsala)
        if (fechaini) {
            $("#fechaInicio").val(fechaini);
        }

        if (fechafin) {
            $("#fechaFin").val(fechafin);
        }
        params = { CodSala: codsala, fechaInicio: fechaini, fechaFin: fechafin, id: ider };
        buscarAccionCajaTemporizada(params);

    } else {
        ObtenerListaSalas()

    }
    // BUSCAR ----------------------------------------------------------
    toastr.options = {
        "preventDuplicates": true
    };

    $(document).on("click", "#btnBuscar", function () {
        if ($("#cboSala").length == 0 || $("#cboSala").val() == null) {
            toastr.error("Seleccione una sala.")
            return false
        }
        if ($("#fechaInicio").val() === "") {
            toastr.error("Ingrese una fecha de Inicio.")
            return false
        }
        if ($("#fechaFin").val() === "") {
            toastr.error("Ingrese una fecha Fin.")
            return false
        }
        buscarAccionCajaTemporizada()
    });
    // NUEVO ------------------------------------------------------------
    $(document).on('click', "#btnNuevo", function () {
        limpiarValidadorFormAccionCajaTemporizada()
        IdAccionCajaTemporizada = 0

        $('#textAccionCajaTemporizada').text('Nuevo')
        $('#IdAccionCajaTemporizada').val(0)
        $('#cboSalaAccionCajaTemporizada').val(null)
        $('#Descripcion').val('')
        $('#DispositivoOtros').val('')
 /*       $('#cboAutorizar').val(null)*/
        $('#cboDeficiencia').val(null)
        $('#cboDispositivo').val(null)

 
        $('#GRFFT').val('')
        $('#RutaImagen').val('') 
        $('#Fecha').val(moment().format('DD/MM/YYYY hh:mm A'));
        $("#Fecha").datetimepicker({
            format: 'DD/MM/YYYY hh:mm A',
            defaultDate: moment(),
            pickTime: true
        }); 
        $('#MedidaAdoptada').val('')

        renderSelectSalasModalAccionCajaTemporizada(true)
        buscarListaDeficienciaPorEstadoACT()
        buscarListaDispositivoPorEstadoACT()
        //obtenerListaEmpleadosPorEstadoAutorizar()
        renderTablaEmpleadosSeleccionados(true)
        $('#full-modal_entradasalidagu').modal('show')

        var maxLength = $('#MedidaAdoptada').attr('maxlength');
        $('#charCountMedidaAdoptada').text(0 + '/' + 300);

        var maxLength = $('#DispositivoOtros').attr('maxlength');
        $('#charCountDispositivoOtros').text(0 + '/' + 300);

    });

    $(document).on('click', '#btnExcel', function (e) {
        e.preventDefault()

        if ($("#cboSala").length == 0 || $("#cboSala").val() == null) {
            toastr.error("Seleccione una sala.")
            return false
        }
        if ($("#fechaInicio").val() === "") {
            toastr.error("Ingrese una fecha de Inicio.")
            return false
        }
        if ($("#fechaFin").val() === "") {
            toastr.error("Ingrese una fecha Fin.")
            return false
        }

        let listasala = $("#cboSala").val();
        let fechaini = $("#fechaInicio").val();
        let fechafin = $("#fechaFin").val();
        let dataForm = {
            codsala: listasala,
            fechaini, fechafin
        }
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "AccionesIncidentesCajasTemporizadas/ReporteAccionesIncidentesCajasTemporizadasExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataForm),
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
    })
    
    // Excel Plantilla ------------------------------------------------------------
    $(document).on('click', '#btnExcel_Plantilla', function (e) {
        e.preventDefault()

        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "AccionesIncidentesCajasTemporizadas/PlantillaAccionesIncidentesCajasTemporizadasDescargarExcel",
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
    })



    //---NUEVO CONTINUANDO-----------------------------------
    $("#full-modal_entradasalidagu").on("shown.bs.modal", function (e) {
        initAutocomplete()
        //renderSelectSalasModalAccionCajaTemporizada()
        //buscarListaDeficienciaPorEstadoACT()



        $(".mySelectbienMaterial").val('')
        $(".mySelectbienMaterial").append('<option value="">---Selecciones---</option>');
         

        $("#cboTipoAccionCajaTemporizada").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_entradasalidagu')
        })
        $("#cboTipoAccionCajaTemporizada").val(null).trigger("change")
        limpiarValidadorFormAccionCajaTemporizada()
    })

    $(document).on('change', '#cboDispositivo', function (e) {
        let valorSeleccionado = $(this).val()
        if (valorSeleccionado == -1) {
            $("#dispositivoOtros").show()
            $('#bienMaterialForm').bootstrapValidator('enableFieldValidators', 'DescripcionDispositivo', true);
            $('#bienMaterialForm').bootstrapValidator('updateMessage', 'DescripcionDispositivo', 'notEmpty', '');
        }
        else {
            $("#dispositivoOtros").hide()
            $('#DescripcionDispositivo').val('')
            $('#bienMaterialForm').bootstrapValidator('enableFieldValidators', 'DescripcionDispositivo', false);
        }
    })
    $('#formCargo').bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            CargoNombre: {
                validators: {
                    notEmpty: {
                        message: 'El nombre es obligatorio'
                    }
                }
            },
            CargoEstado: {
                validators: {
                    notEmpty: {
                        message: 'El estado es obligatorio'
                    }
                }
            }
        }
    });


    $("#bienMaterialForm")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                TipoAccionCajaTemporizada: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                CodSala: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                Fecha: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                IdCategoria: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                IdDeficiencia: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                IdDispositivo: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                FechaIngreso: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                DescripcionDispositivo: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
            }
        }).on('success.field.bv', function (e, data) {
            e.preventDefault();
            var $parent = data.element.parents('.form-group');
            $parent.removeClass('has-success');
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
        });


    $(document).on('click', '#btnGuardarAccionCajaTemporizada', function (e) {

        e.preventDefault()
        // NUEVO VALIDACION ------------------------------------------------------
       


        $("#bienMaterialForm").data('bootstrapValidator').resetForm();
        var validarRegistro = $("#bienMaterialForm").data('bootstrapValidator').validate();

        if (validarRegistro.isValid()) {

            let url = basePath
            if (IdAccionCajaTemporizada == 0) {
                url += "AccionesIncidentesCajasTemporizadas/GuardarAccionesIncidentesCajasTemporizadas"
            } else {
                url += "AccionesIncidentesCajasTemporizadas/EditarAccionesIncidentesCajasTemporizadas"

            }
            let dataForm = new FormData(document.getElementById("bienMaterialForm"))

            dataForm.delete('IdAccionCajaTemporizada');
            dataForm.append('IdAccionCajaTemporizada', IdAccionCajaTemporizada);


            dataForm.append('NombreDeficiencia', $('#cboDeficiencia option:selected').text())
            dataForm.append('NombreDispositivo', $('#cboDispositivo option:selected').text())
            dataForm.append('NombreSala', $('#cboSalaAccionCajaTemporizada option:selected').text())

            if (arrayEmpleadosSeleccionados && arrayEmpleadosSeleccionados.length > 0) {
            dataForm.append('IdAutoriza', arrayEmpleadosSeleccionados[0].IdEmpleado)
            dataForm.append('NombreAutoriza', arrayEmpleadosSeleccionados[0].NombreCompleto)
            
            $.ajax({
                url: url,
                type: "POST",
                method: "POST",
                contentType: false,
                data: dataForm,
                cache: false,
                processData: false,
                beforeSend: function () {
                    $.LoadingOverlay("show");
                },
                complete: function () {
                    $.LoadingOverlay("hide");
                },
                success: function (response) {
                    if (response.respuesta) {
                        limpiarValidadorFormAccionCajaTemporizada();
                        toastr.success(response.mensaje, "Mensaje Servidor")
                        $('#full-modal_entradasalidagu').modal('hide')
                        buscarAccionCajaTemporizada()
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor")
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    toastr.error("Ocurrió un error al guardar el registro.", "Error");
                }
            });

            } else {
                toastr.error('Por favor, seleccione al menos un empleado para guardar el registro.');
            }
        }
    })




    $(document).on("click", ".btnEditarAccionCajaTemporizada", function () {
        limpiarValidadorFormAccionCajaTemporizada();

        IdAccionCajaTemporizada = $(this).data("id");

        let rowData = objetodatatable
            .rows()
            .data()
            .toArray()
            .find(row => row.IdAccionCajaTemporizada === IdAccionCajaTemporizada);


        if (rowData) {
            $('#textAccionCajaTemporizada').text('Editar');
        
            renderSelectSalasModalAccionCajaTemporizada(rowData.CodSala)
            buscarListaDispositivoPorEstadoACT(rowData.IdDispositivo)
            buscarListaDeficienciaPorEstadoACT(rowData.IdDeficiencia)
            arrayEmpleadosSeleccionados = [{
                IdEmpleado: rowData.IdAutoriza,
                NombreCompleto: rowData.NombreAutoriza, 
                NombreDocumentoRegistro: rowData.TipoDocumento,
                DocumentoRegistro: rowData.DocumentoRegistro,
                Cargo: rowData.Cargo 
            }]
             
            renderTablaEmpleadosSeleccionados()

            $('#cboSalaAccionCajaTemporizada').val(rowData.CodSala); 
            $('#cboDispositivo').val(rowData.IdDispositivo); 
            $('#cboDeficiencia').val(rowData.IdDeficiencia);
            $('#Fecha').val((moment(rowData.Fecha).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.Fecha).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.Fecha).format('DD/MM/YYYY hh:mm A'));

            $('#MedidaAdoptada').val(rowData.MedidaAdoptada);
            var currentLengthMedidaAdoptada = $('#MedidaAdoptada').val().length;
            var maxLengthMedidaAdoptada = $('#MedidaAdoptada').attr('maxlength');
            $('#charCountMedidaAdoptada').text(currentLengthMedidaAdoptada + '/' + maxLengthMedidaAdoptada);

            $('#DispositivoOtros').val(rowData.DescripcionDispositivo);
            var currentLengthDispositivoOtros = $('#DispositivoOtros').val().length;
            var maxLengthDispositivoOtros = $('#DispositivoOtros').attr('maxlength');
            $('#charCountDispositivoOtros').text(currentLengthDispositivoOtros + '/' + maxLengthDispositivoOtros);

            $('#full-modal_entradasalidagu').modal('show');

        } else {
            toastr.error("No se encontraron datos para este registro.", "Error");
        }
    });

    $(document).on('click', '#btnAgregarEmpleadoAccionCajaTemporizada', function (e) {
        e.preventDefault()
        $('#full-modal_empleadotabla').modal('show')
    })


    $("#full-modal_empleadotabla").on("shown.bs.modal", function () {
      /*  obtenerListaEmpleadosPorEstadoAutorizar()*/
    })
    $(document).on('click', '.seleccionarEmpleado', function (e) {
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        let elementoSeleccionado = arrayEmpleados.find(x => x.IdEmpleado == idSeleccionado)
        if (elementoSeleccionado) {
            arrayEmpleadosSeleccionados.push({
                IdEmpleado: elementoSeleccionado.IdEmpleado,
                Nombre: elementoSeleccionado.Nombre,
                ApellidoPaterno: elementoSeleccionado.ApellidoPaterno,
                ApellidoMaterno: elementoSeleccionado.ApellidoMaterno,
                IdTipoDocumentoRegistro: elementoSeleccionado.IdTipoDocumentoRegistro,
                TipoDocumento: elementoSeleccionado.TipoDocumento,
                DocumentoRegistro: elementoSeleccionado.DocumentoRegistro,
                IdCargo: elementoSeleccionado.IdCargo,
                NombreCargo: elementoSeleccionado.Cargo.Nombre
            })
        }
        renderTablaEmpleadosSeleccionados()
    })

    $(document).on('click', '.editarCargo', function (e) {
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        let nombre = $(this).data('nombre')
        let estado = $(this).data('estado')
        $("#CargoIdCargo").val(idSeleccionado)
        $("#CargoNombre").val(nombre)
        $("#CargoEstado").val(estado)
        $("#textoCargo").text("Editar ")
    })
    $(document).on('click', '#btnCancelarCargo', function (e) {
        e.preventDefault()
        $("#CargoIdCargo").val(0)
        $("#CargoNombre").val('')
        $("#CargoEstado").val(1)
        $("#textoCargo").text("Agregar ")
    })
    $(document).on('click', '.quitarEmpleado', function (e) {
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        arrayEmpleadosSeleccionados = arrayEmpleadosSeleccionados.filter(x => x.IdEmpleado != idSeleccionado)
        renderTablaEmpleadosSeleccionados()

    })
    // MODAL CATEGORIAS ------------------------------------------------------------
    $(document).on('click', '#agregarCategoria', function (e) {
        e.preventDefault()
        $("#full-modal_categorias").modal('show')
    })
    $("#full-modal_categorias").on("shown.bs.modal", function () {
        obtenerListaCategorias()
    })
    $(document).on('click', '.editarCategoria', function (e) {
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        let nombre = $(this).data('nombre')
        let estado = $(this).data('estado')
        $("#CategoriaIdCategoria").val(idSeleccionado)
        $("#CategoriaNombre").val(nombre)
        $("#CategoriaEstado").val(estado)
        $("#textoCategoria").text("Editar ")
    })
    $(document).on('click', '#btnCancelarCategoria', function (e) {
        e.preventDefault()
        $("#CategoriaIdCategoria").val(0)
        $("#CategoriaNombre").val('')
        $("#CategoriaEstado").val(1)
        $("#textoCategoria").text("Agregar ")
    })
    $(document).on('click', '#btnGuardarEditarCategoria', function (e) {
        e.preventDefault()
        let dataForm = {
            IdCategoria: $('#CategoriaIdCategoria').val(),
            Nombre: $('#CategoriaNombre').val(),
            Estado: $('#CategoriaEstado').val(),
        }
        let url = `${basePath}AccionesIncidentesCajasTemporizadas/InsertarCategoria`
        if ($('#CategoriaIdCategoria').val() > 0) {
            url = `${basePath}AccionesIncidentesCajasTemporizadas/EditarCategoria`
        }
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
                if (response.respuesta) {
                    $("#CategoriaIdCategoria").val(0)
                    $("#CategoriaNombre").val('')
                    $("#CategoriaEstado").val(1)
                    $("#textoCategoria").text("Agregar")
                    obtenerListaCategorias()
                    buscarListaDeficienciaPorEstadoACT()
                    buscarListaDispositivoPorEstadoACT()
                    toastr.success(response.mensaje, "Mensaje Servidor");
                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        });
    })


    //MODAL FORMULARIO MOTIVO ----------------------------------
    $(document).on('click', '#agregarMotivo', function (e) {
        e.preventDefault()
        $("#full-modal_motivos").modal('show')
    })


    //VALIDACION FORMULARIO MOTIVO----------------------
    //$("#form_registro_motivo")
    //    .bootstrapValidator({
    //        excluded: [':disabled', ':hidden', ':not(:visible)'],
    //        feedbackIcons: {
    //            valid: 'icon icon-check',
    //            invalid: 'icon icon-cross',
    //            validating: 'icon icon-refresh'
    //        },
    //        fields: {
    //            Nombre: {
    //                validators: {
    //                    notEmpty: {
    //                        message: ' '
    //                    }
    //                }
    //            },
    //            ApellidoPaterno: {
    //                validators: {
    //                    notEmpty: {
    //                        message: ' '
    //                    }
    //                }
    //            },
    //            DNI: {
    //                validators: {
    //                    notEmpty: {
    //                        message: ' '
    //                    }
    //                }
    //            },
    //            TipoExclusion: {
    //                validators: {
    //                    notEmpty: {
    //                        message: ' '
    //                    }
    //                }
    //            },
    //            TipoDoiID: {
    //                validators: {
    //                    notEmpty: {
    //                        message: ' '
    //                    }
    //                }
    //            },
    //            Estado: {
    //                validators: {
    //                    notEmpty: {
    //                        message: ' '
    //                    }
    //                }
    //            },


    //        }
    //    })
    //    .on('success.field.bv', function (e, data) {
    //        e.preventDefault();
    //        var $parent = data.element.parents('.form-group');
    //        $parent.removeClass('has-success');
    //        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    //    });



    $("#full-modal_motivos").on("shown.bs.modal", function () {
        //obtenerListaCategorias()
        obtenerListaMotivos()
    })
    $(document).on('click', '.editarMotivo', function (e) {
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        let nombre = $(this).data('nombre')
        let estado = $(this).data('estado')
        $("#MotivoIdDeficiencia").val(idSeleccionado)
        $("#MotivoNombre").val(nombre)
        $("#MotivoEstado").val(estado)
        $("#textoMotivo").text("Editar")
    })
    $(document).on('click', '#btnCancelarMotivo', function (e) {
        e.preventDefault()
        $("#MotivoIdDeficiencia").val(0)
        $("#MotivoNombre").val('')
        $("#MotivoEstado").val(1)
        $("#textoMotivo").text("Agregar")
    })
    $(document).on('click', '#btnGuardarEditarMotivo', function (e) {
        e.preventDefault()
        let dataForm = {
            IdDeficiencia: $('#MotivoIdDeficiencia').val(),
            Nombre: $('#MotivoNombre').val(),
            Estado: $('#MotivoEstado').val(),
        }
        let url = `${basePath}AccionesIncidentesCajasTemporizadas/InsertarMotivo`
        if ($('#MotivoIdDeficiencia').val() > 0) {
            url = `${basePath}AccionesIncidentesCajasTemporizadas/EditarMotivo`
        }
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
                if (response.respuesta) {
                    $("#MotivoIdDeficiencia").val(0)
                    $("#MotivoNombre").val('')
                    $("#MotivoEstado").val(1)
                    $("#textoMotivo").text("Agregar")
                    //obtenerListaCategorias()
                    //buscarListaDeficienciaPorEstadoACT()
                    obtenerListaMotivos()
                    toastr.success(response.mensaje, "Mensaje Servidor");
                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        });
    })




});



// MODAL MOTIVO ------------------------------------------------------------------

// FUNCIONES----------------------------------------------------------------------------------
function obtenerListaCategorias() {
    $.ajax({
        type: "POST",
        url: basePath + "AccionesIncidentesCajasTemporizadas/ListarCategoria",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.data) {
                renderTablaCategorias(result.data)
            }
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

function obtenerListaMotivos() {
    $.ajax({
        type: "POST",
        url: basePath + "AccionesIncidentesCajasTemporizadas/ListarMotivo",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.data) {
                renderTablaMotivos(result.data)
            }
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

function renderTablaCategorias(data) {
    let element = $('#contenedorListadoCategorias')
    element.empty()

    let tbody = data ? data.map(item => `
        <tr>
            <td>${item.IdCategoria}</td>
            <td>${item.Nombre}</td>
            <td><span class="label label-${item.Estado == 1 ? 'success' : 'danger'}">${item.Estado == 1 ? 'ACTIVO' : 'INACTIVO'}</span></td>
            <td><a class="btn btn-sm btn-danger editarCategoria" data-id="${item.IdCategoria}" data-nombre="${item.Nombre}" data-estado="${item.Estado}">Editar <span class="glyphicon glyphicon-edit"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered table-striped" style="width:100%" id="tableListadoCategorias">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Nombre</th>
                    <th>Estado</th>
                    <th>Acción</th>
                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4">Agregue una categoria</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)
    if (tbody.length > 0) {
        $("#tableListadoCategorias").DataTable()
    }


}


function renderTablaMotivos(data) {
    let element = $('#contenedorListadoMotivos')
    element.empty()

    let tbody = data ? data.map(item => `
        <tr>
            <td>${item.IdDeficiencia}</td>
            <td>${item.Nombre}</td>
            <td><span class="label label-${item.Estado == 1 ? 'success' : 'danger'}">${item.Estado == 1 ? 'ACTIVO' : 'INACTIVO'}</span></td>
            <td><a class="btn btn-sm btn-danger editarMotivo" data-id="${item.IdDeficiencia}" data-nombre="${item.Nombre}" data-estado="${item.Estado}">Editar <span class="glyphicon glyphicon-edit"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered table-striped" style="width:100%" id="tableListadoMotivos">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Nombre</th>
                    <th>Estado</th>
                    <th>Acción</th>
                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4">Agregue un motivo</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)
    if (tbody.length > 0) {
        $("#tableListadoMotivos").DataTable()
    }


}


//function renderTablaEmpleadosSeleccionados() {
//    let element = $('#contenedorTableEmpleadosSeleccionados')
//    element.empty()

//    let tbody = arrayEmpleadosSeleccionados ? arrayEmpleadosSeleccionados.map(item => `
//        <tr>
//            <td>${item.Nombre} ${item.ApellidoPaterno} ${item.ApellidoMaterno}</td>
//            <td>${item.TipoDocumento} - ${item.DocumentoRegistro}</td>
//            <td>${item.NombreCargo}</td>
//            <td><a class="btn btn-sm btn-danger quitarEmpleado" data-id="${item.IdEmpleado}">Quitar <span class="glyphicon glyphicon-remove"></span></a></td>
//        </tr>
//    `) : []
//    let htmlContent = `
//        <table class="table table-sm table-bordered" style="width:100%" id="tableEmpleadosSeleccionadosAccionCajaTemporizada">
//            <thead>
//                <tr>
//                    <th>Nombres</th>
//                    <th>Nro Documento</th>
//                    <th>Cargo</th>
//                    <th>Acción</th>
//                </tr>
//            </thead>
//            <tbody>
//                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4">Agregue un empleado</td></tr>`}
//            </tbody>
//        </table>
//    `
//    element.html(htmlContent)

//}
function obtenerListaEmpleadosPorEstado() {
    let element = $('#contenedorTableEmpleadosAccionCajaTemporizada')
    element.empty()
    $.ajax({
        type: "POST",
        url: basePath + "BienMaterial/ListarEmpleadoPorEstado",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.data) {
                /*arrayEmpleados = result.data*/
                let tbody = result.data.map(item => `
                        <tr>
                            <td>${item.Nombre} ${item.ApellidoPaterno} ${item.ApellidoMaterno}</td>
                            <td>${item.TipoDocumento} - ${item.DocumentoRegistro}</td>
                            <td>${item.Cargo.Nombre}</td>
                            <td><a class="btn btn-sm btn-primary seleccionarEmpleado" data-id="${item.IdEmpleado}">Seleccionar <span class="glyphicon glyphicon-check"></span></a></td>
                        </tr>
                    `)
                let htmlContent = `
                    <table class="table table-sm table-bordered" style="width: 100%; height: 50%;"  id="tableEmpleadosAccionCajaTemporizada">
                        <thead>
                            <tr>
                                <th>Nombres</th>
                                <th>Nro Documento</th>
                                <th>Cargo</th>
                                <th>Acción</th>
                            </tr>
                        </thead>
                        <tbody>
                            ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4"><p class="text-center text-danger">No se encontraron resultados</p></td></tr>`}
                        </tbody>
                    </table>
                `
                element.html(htmlContent)
                if (tbody.length > 0) {
                    $('#tableEmpleadosAccionCajaTemporizada').DataTable()
                }
            }
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    });
    return false
}

function renderSelectSalasModalAccionCajaTemporizada(value) {
    $("#cboSalaAccionCajaTemporizada").html('')
    if (arraySalas) {
        $("#cboSalaAccionCajaTemporizada").html(arraySalas.map(item => `<option value="${item.CodSala}">${item.Nombre}</option>`).join(""))
        if (value) {
            $('#cboSalaAccionCajaTemporizada').val(value);
        } else {
            $("#cboSalaAccionCajaTemporizada").val(null).trigger("change");
        }
        $("#cboSalaAccionCajaTemporizada").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_entradasalidagu')
        });
        limpiarValidadorFormAccionCajaTemporizada()
    }
}

function buscarListaDeficienciaPorEstadoACT(value) {
    $.ajax({
        type: "POST",
        url: basePath + "AccionesIncidentesCajasTemporizadas/ListarDeficienciaPorEstado",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboDeficiencia").html('')
            if (result.data) {
                $("#cboDeficiencia").html(result.data.map(item => `<option value="${item.IdDeficiencia}">${item.Nombre}</option>`).join(""))
                if (value) {
                    $('#cboDeficiencia').val(value);
                } else {
                    $("#cboDeficiencia").val(null).trigger("change");
                }
                $("#cboDeficiencia").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_entradasalidagu')
                });
                limpiarValidadorFormAccionCajaTemporizada()
            }
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

function buscarListaDispositivoPorEstadoACT(value) {
    $.ajax({
        type: "POST",
        url: basePath + "AccionesIncidentesCajasTemporizadas/ListarDispositivoPorEstado",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboDispositivo").html('')
            if (result.data) {
                $("#cboDispositivo").html(result.data.map(item => `<option value="${item.IdDispositivo}">${item.Nombre}</option>`).join(""))
                $('#cboDispositivo').append('<option value="-1">OTROS</option>')

                $("#cboDispositivo").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_entradasalidagu')
                });
                $("#cboDispositivo").val(null).trigger("change");
                if (value) {
                    $("#cboDispositivo").val(value).trigger("change");
                }
                limpiarValidadorFormAccionCajaTemporizada()
            }
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

function ObtenerListaSalas(value) {
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
            $("#cboSala").html('')
            const codSalaArray = result.data.map(sala => String(sala.CodSala));

            if (result.data) {
                arraySalas = result.data
                arraySelect = [`<option value="${-1}" selected>TODOS</option>`]
                result.data.map(item => arraySelect.push(`<option value="${item.CodSala}">${item.Nombre}</option>`))
                $("#cboSala").html(arraySelect.join(""))
                $("#cboSala").select2({
                    multiple: true, placeholder: "--Seleccione--", allowClear: true
                });
                if (value) {
                    $('#cboSala').val(value).trigger("change");
                } else {
                    $("#cboSala").val(-1).trigger("change")
                }
            }
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
function ObtenerTipoAccionCajaTemporizada() {
    $.ajax({
        type: "POST",
        url: basePath + "AccionesIncidentesCajasTemporizadas/ListadoTipoAccionCajaTemporizadaJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            $("#cboTipoBien").empty();
            $("#cboTipoBien").append('<option value="">Seleccionar</option>');
            $('#cboTipoBien').val(CboSeleccionado1);
            $.each(datos, function (index, value) {
                $("#cboTipoBien").append('<option value="' + value.value + '">' + value.label + '</option>');
            });
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (result) {
            $.LoadingOverlay("hide");
        }
    });
    return false;
}
function buscarAccionCajaTemporizada(params) {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();
    let addtabla = $("#contenedor_tabla");

    if (params) {
        listasala = params.CodSala;
        fechaini = params.fechaInicio;
        fechafin = params.fechaFin;
    }
    $.ajax({
        type: "POST",
        url: basePath + "AccionesIncidentesCajasTemporizadas/AccionCajaTemporizadaListarxSalaFechaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            let datos = response.data || [];
            $(addtabla).empty();
            $(addtabla).append('<table id="tableResultado" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            let highlightId = params?.id; 

            objetodatatable = $("#tableResultado").DataTable({
                destroy: true,
                sort: true,
                scrollCollapse: true,
                scrollX: true,
                paging: true,
                autoWidth: true,
                data: datos,
                aaSorting: [[0, 'desc']],
                columns: [
                    { data: "IdAccionCajaTemporizada", title: "ID" },
                    { data: "NombreSala", title: "Sala" },
                    {
                        data: "FechaRegistro",
                        title: "Fecha Reg",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY') == "01/01/1753" ? "" : moment(data).format('DD/MM/YYYY HH:mm A') == "31/12/1752 23:51 PM" ? '<span class="label btn-warning">No definido</span>' : moment(data).format('DD/MM/YYYY HH:mm A');
                        }
                    }, 
                    {
                        data: "Fecha",
                        title: "Fecha Ocurrida",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY HH:mm A') == "01/01/1753" ? "" : moment(data).format('DD/MM/YYYY HH:mm A');
                        }
                    },
                    { data: "NombreAutoriza", title: "Autoriza" },
                    { data: "NombreDispositivo", title: "Dispositivos" },
                    { data: "NombreDeficiencia", title: "Deficiencias" },
                    //{ data: "MedidaAdoptada", title: "Medidas Adoptadas" },
                    {
                        data: "FechaSolucion",
                        title: "Fecha (Solucion)",
                        render: function (data)
                        {
                            return moment(data).format('DD/MM/YYYY') == "01/01/1753" ? "" : moment(data).format('DD/MM/YYYY HH:mm A') == "31/12/1752 23:51 PM" ? '<span class="label btn-warning">No definido</span>' : moment(data).format('DD/MM/YYYY HH:mm A');
                        }
                    }, 
                    {
                        data: "IdAccionCajaTemporizada",
                        title: "Accion",
                        "render": function (o, value, oData) {
                            return `
                            <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-warning btnEditarAccionCajaTemporizada" data-id="${o}">
                                <i class="glyphicon glyphicon-pencil"></i>
                            </button> 
                            <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-primary btnFinalizarSalidaAccionCajaTemporizada" data-id="${o}">
                               <i class="glyphicon glyphicon-flag"></i>
                            </button>       
                            <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-danger btnEliminar"  data-id="${o}">
                                <i class="glyphicon glyphicon-remove"></i>
                            </button>`
                        },
                        className: "text-center",
                        "orderable": false
                    }
                ],
                 "drawCallback": function (settings) {
                     if (highlightId) {
                         let row = $(`#tableResultado tbody tr td:first-child:contains(${highlightId})`).parent();
                         row.addClass("highlight-row");
                     }
                     $('.btnEditarAccionCajaTemporizada').tooltip({
                        title: "Editar Registro"
                    });

                     $('.btnFinalizarSalidaAccionCajaTemporizada').tooltip({
                        title: "Finalizar Registro"
                    });
                    $('.btnEliminar').tooltip({
                        title: "Eliminar Registro"
                    });
                },
            });
        },
        error: function (request, status, error) {
            toastr.error("Error en la solicitud.", "Mensaje Servidor");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}



$(document).on("click", ".btnEliminar", function () {
    const $this = $(this);
    var id = $this.data("id");
    $.confirm({
        title: '¿Estás seguro de Continuar?',
        content: '¿Eliminar registro?',
        confirmButton: 'Ok',
        cancelButton: 'Cerrar',
        confirmButtonClass: 'btn-success',
        cancelButtonClass: 'btn-danger',
        confirm: () => {
            deleteAccionCajaTemporizada(id)
        }
    });
});

const deleteAccionCajaTemporizada = (idregistro) => {
    const data = {
        id: idregistro
    };
    $.ajax({
        url: `${basePath}AccionesIncidentesCajasTemporizadas/EliminarAccionesIncidentesCajasTemporizadas`,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            if (!response.respuesta) {
                toastr.error(response.mensaje, "Mensaje Servidor");
                return;
            }
            toastr.success(response.mensaje, "Mensaje Servidor");
            buscarAccionCajaTemporizada()
        }
    });
}



//VALIDAR-----------------------------------------
function limpiarValidadorFormAccionCajaTemporizada() {
    $("#bienMaterialForm").parent().find('div').removeClass("has-error");
    $("#bienMaterialForm").parent().find('i').removeAttr("style").hide();
    $("#bienMaterialForm").parent().find('div').removeClass("has-success");
}
// NUEVO EMPLEADO------------------------------------------------------------
$(document).on('click', "#btnNuevoEmpleado", function () {
    limpiarValidadorFormAccionCajaTemporizada()

    $('#textAccionCajaTemporizada').text('Nuevo')
    $('#IdAccionCajaTemporizada').val(0)
    $('#cboSalaAccionCajaTemporizada').val(null)
    $('#Descripcion').val('')
    $('#DescripcionDispositivo').val('')
    $('#cboDeficiencia').val(null)
    $('#cboDispositivo').val(null)

    $('#GRFFT').val('')
    $('#RutaImagen').val('')
    $('#FechaIngreso').val('')
    $('#MedidaAdoptada').val('')
    arrayEmpleadosSeleccionados = []
    limpiarValidadorFormAccionCajaTemporizada()
    $('#full-modal_nuevoempleado').modal('show')
});
$("#full-modal_nuevoempleado").on("shown.bs.modal", function () {

    obtenerListaCargosPorEstado();
    obtenerTiposDocumentos();
})
function obtenerListaCargos() {
    $.ajax({
        type: "POST",
        url: basePath + "AccionesIncidentesCajasTemporizadas/ListarCargo",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.data) {
                renderTablaCargos(result.data)
            }
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
function renderTablaCargos(data) {
    let element = $('#contenedorListadoCargos')
    element.empty()

    let tbody = data ? data.map(item => `
        <tr>
            <td>${item.IdCargo}</td>
            <td>${item.Nombre}</td>
            <td><span class="label label-${item.Estado == 1 ? 'success' : 'danger'}">${item.Estado == 1 ? 'ACTIVO' : 'INACTIVO'}</span></td>
            <td><a class="btn btn-sm btn-danger editarCargo" data-id="${item.IdCargo}" data-nombre="${item.Nombre}" data-estado="${item.Estado}">Editar <span class="glyphicon glyphicon-edit"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered table-striped" style="width:100%" id="tableListadoMotivos">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Nombre</th>
                    <th>Estado</th>
                    <th>Acción</th>
                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4">Agregue un cargo</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)
    if (tbody.length > 0) {
        $("#tableListadoCargos").DataTable()
    }


}

$(document).on('click', '#btnGuardarNuevoEmpleado', function (e) {
    e.preventDefault()
    let dataForm = {
        IdTipoDocumentoRegistro: $('#NuevoEmpleadoIdTipoDocumento').val(),
        DocumentoRegistro: $('#NuevoEmpleadoDocumentoRegistro').val(),
        Nombre: $('#NuevoEmpleadoNombre').val(),
        ApellidoMaterno: $('#NuevoEmpleadoApellidoMaterno').val(),
        ApellidoPaterno: $('#NuevoEmpleadoApellidoPaterno').val(),
        IdCargo: $('#NuevoEmpleadocbCargo').val(),
        Estado: 1,
    }
    $.ajax({
        url: `${basePath}AccionesIncidentesCajasTemporizadas/InsertarEmpleado`,
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
            if (response.respuesta) {
                $("#NuevoEmpleadoIdTipoDocumento").val(1)
                $("#NuevoEmpleadoDocumentoRegistro").val("")
                $("#NuevoEmpleadoNombre").val('')
                $("#NuevoEmpleadoApellidoMaterno").val('')
                $("#NuevoEmpleadoApellidoPaterno").val('')

                toastr.success(response.mensaje, "Mensaje Servidor");
                $('#full-modal_nuevoempleado').modal('hide');
                obtenerListaEmpleadosPorEstadoAutorizar();
            }
            else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            toastr.error("Error Servidor", "Mensaje Servidor");
        }
    });
})

function obtenerListaCargosPorEstado() {
    $.ajax({
        type: "POST",
        url: basePath + "BienMaterial/ListarCargoPorEstado",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#NuevoEmpleadocbCargo").html('')
            if (result.data) {
                $("#NuevoEmpleadocbCargo").html(result.data.map(item => `<option value="${item.IdCargo}">${item.Nombre}</option>`).join(""))
                $("#NuevoEmpleadocbCargo").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_nuevoempleado')
                });
                $("#NuevoEmpleadocbCargo").val(null).trigger("change")
            }
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
$(document).on('click', "#btnCancelarNuevoEmpleado", function () {
    $('#full-modal_nuevoempleado').modal('hide');
});

function obtenerTiposDocumentos() {
    $.ajax({
        type: "POST",
        url: basePath + "AsistenciaCliente/GetListadoTipoDocumento",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#NuevoEmpleadoIdTipoDocumento").html('')
            if (result.data) {
                $("#NuevoEmpleadoIdTipoDocumento").html(result.data.map(item => `<option value="${item.IdCargo}">${item.Nombre}</option>`).join(""))
                $("#NuevoEmpleadoIdTipoDocumento").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_nuevoempleado')
                });
                $("#NuevoEmpleadoIdTipoDocumento").val(null).trigger("change")
            }
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
$(document).on('click', '#agregarCargo', function (e) {
    e.preventDefault()
    $("#full-modal_cargos").modal('show')
    $("#full-modal_cargos").on("shown.bs.modal", function () {
        obtenerListaCargos()
    })
})

$(document).on('click', '#btnGuardarEditarCargo', function (e) {
    e.preventDefault();

    let dataForm = {

        IdCargo: $('#CargoIdCargo').val(),
        Nombre: $('#CargoNombre').val(),
        Estado: $('#CargoEstado').val(),
    }

    $("#formCargo").data('bootstrapValidator').resetForm();
    var validarRegistro = $("#formCargo").data('bootstrapValidator').validate();
    if (validarRegistro.isValid()) {
        let IdRegistro = $("#CargoIdCargo").val()
        let url = basePath
        if (IdRegistro == 0) {
            url += "AccionesIncidentesCajasTemporizadas/InsertarCargo"
        } else {
            url += "AccionesIncidentesCajasTemporizadas/EditarCargo"
        }
        let dataForm4 = new FormData(document.getElementById("formCargo"))
        $.ajax({
            url: url,
            type: "POST",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataForm),
            cache: false,
            processData: false,
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                if (response.respuesta) {
                    limpiarValidadorFormCargo();
                    obtenerListaCargos();
                    obtenerListaCargosPorEstado();
                    toastr.success(response.mensaje, "Mensaje Servidor")

                } else {
                    toastr.error(response.mensaje, "Mensaje Servidor")
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
            }
        });
    }

})
//$(document).on('click', "#btnCancelarCargo", function () {
//    $('#full-modal_cargos').modal('hide');
//});
function limpiarValidadorFormCargo() {
    $("#formCargo").parent().find('div').removeClass("has-error");
    $("#formCargo").parent().find('i').removeAttr("style").hide();

}

$(document).on("click", ".btnFinalizarSalidaAccionCajaTemporizada", function () {
    idAccionCajaTemporizadaSeleccionado = $(this).data("id");
    const mensaje = "¿Estás seguro de finalizar el registro?";
    $("#confirmationMessage").text(mensaje);

    const horaActual = obtenerHoraActual();
    $("#horaInput").val(horaActual); 

    $("#full-modal_confirmacion").modal("show");


    //$("#horaInput").after('<span class="glyphicon glyphicon-calendar"></span>');


    //if (!$("#horaInput").data("DateTimePicker")) {
    //    $("#horaInput").datetimepicker({
    //        pickDate: false,   
    //        format: 'HH:mm',   
    //        defaultDate: new Date(), 
    //        pickTime: true     
    //    });
    //}

});

$("#confirmFinalizar").on("click", function () {
    const horaFinalizada = $("#horaInput").val();  
    if (idAccionCajaTemporizadaSeleccionado) {
        finalizarAccionCajaTemporizada(idAccionCajaTemporizadaSeleccionado, horaFinalizada);
    }
    $("#full-modal_confirmacion").modal("hide");
});

function obtenerHoraActual() {
    const ahora = new Date();
    let horas = ahora.getHours();
    let minutos = ahora.getMinutes();

    minutos = minutos < 10 ? '0' + minutos : minutos;
    horas = horas < 10 ? '0' + horas : horas;
    return `${horas}:${minutos}`;
}

function finalizarAccionCajaTemporizada(id, hora) {
    $.ajax({
        type: "POST",
        url: basePath + "AccionesIncidentesCajasTemporizadas/FinalizarHoraRegistroAccionCajaTemporizada",
        data: JSON.stringify({ idaccioncajatemporizada: id, horaSalida: hora }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if (response.respuesta) {
                toastr.success(response.mensaje, "Exito");
            } else {
                toastr.error(response.mensaje, "Error");
            }
            $("#full-modal_confirmacion").modal("hide");
            buscarAccionCajaTemporizada();
        },
        error: function () {
            toastr.error("No se pudo completar la solicitud.", "Error del servidor");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}


//function obtenerListaEmpleadosPorEstadoAutorizar(value) {
//    $.ajax({
//        type: "POST",
//        url: basePath + "EnteRegulador/ListarEmpleadoPorEstado",
//        cache: false,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        beforeSend: function (xhr) {
//            $.LoadingOverlay("show");
//        },
//        success: function (result) {
//            console.log(result);
//            $("#cboAutorizar").html('')
//            if (result.data) {
//                arrayEmpleados = result.data
//              /*  $("#cboAutorizar").html(result.data.map(item => `<option value="${item.IdEmpleado}">${item.Nombre} ${item.ApellidoPaterno}</option>`).join(""))*/
//                $("#cboAutorizar").html(result.data.map(item => `<option value="${item.Nombre} ${item.ApellidoPaterno}">${item.Nombre} ${item.ApellidoPaterno}</option>`).join(""));
//                if (value) {
//                    $('#cboAutorizar').val(value);
//                } else {
//                    $("#cboAutorizar").val(null).trigger("change");
//                }
//                $("#cboAutorizar").select2({
//                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_entradasalidagu')
//                });
//            }
//        },
//        error: function (request, status, error) {
//            toastr.error("Error", "Mensaje Servidor")
//        },
//        complete: function (resul) {
//            $.LoadingOverlay("hide")
//        }
//    });
//    return false;
//}

function obtenerListaEmpleadosPorEstadoAutorizar(value) {
    $.ajax({
        type: "POST",
        url: basePath + "AccionesIncidentesCajasTemporizadas/ListarEmpleadosGerentesYJefes",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboAutorizar").html('')
            if (result.data) {
                arrayEmpleados = result.data
                /*  $("#cboAutorizar").html(result.data.map(item => `<option value="${item.IdEmpleado}">${item.Nombre} ${item.ApellidoPaterno}</option>`).join(""))*/
                $("#cboAutorizar").html(result.data.map(item => `<option value="${item.IdAutoriza}">${item.NombreAutoriza} - ${item.Cargo}</option>`).join(""));
                if (value) {
                    $('#cboAutorizar').val(value);
                } else {
                    $("#cboAutorizar").val(null).trigger("change");
                }
                $("#cboAutorizar").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_entradasalidagu')
                });
                limpiarValidadorFormAccionCajaTemporizada()
            }
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

function initAutocomplete() {
    $("#busqueda").autocomplete({
        source: function (request, response) {
                $.ajax({
                    url: basePath + "BUKEmpleado/ObtenerEmpleadosActivosPorTerminoSinEmpresa",
                    dataType: "json",
                    data: {
                        term: request.term,
                       // idcargo: 44,
                       // idcargo: 59,
                       // idcargo: 62,
                       // idcargo: 44,
                       ///* [44, 59, 62]*/
                    },
                    success: function (json) {
                        var results = $.map(json.data, function (item) {
                            return {
                                label: item.NombreCompleto + " - " + item.NumeroDocumento,
                                value: item.NombreCompleto,
                                fullData: item
                            };
                        });
                        response(results);
                    },
                    error: function () {
                        response([]);
                    }
                });

    },

        minLength: 1,
        select: function (event, ui) {
            if (arrayEmpleadosSeleccionados.length > 0) {
                toastr.warning("Empleado seleccionado. Solo puedes seleccionar uno.", "Advertencia");
                return; 
            }
            let existEmpleado = arrayEmpleadosSeleccionados.find(x => x.IdEmpleado == ui.item.fullData.IdBuk)

            if (!existEmpleado) {
                arrayEmpleadosSeleccionados.push({
                    IdEmpleado: ui.item.fullData.IdBuk,
                    Nombre: ui.item.fullData.Nombres,
                    ApellidoPaterno: ui.item.fullData.ApellidoPaterno,
                    ApellidoMaterno: ui.item.fullData.ApellidoMaterno,
                    NombreCompleto: ui.item.fullData.NombreCompleto,
                    NombreDocumentoRegistro: ui.item.fullData.TipoDocumento,
                    DocumentoRegistro: ui.item.fullData.NumeroDocumento,
                    Cargo: ui.item.fullData.Cargo,
                    IdCargo: ui.item.fullData.IdCargo,
                    Empresa: ui.item.fullData.Empresa,
                    IdEmpresa: ui.item.fullData.IdEmpresa
                })
            } else {
                toastr.warning("Empleado ya seleccionado", "Advertencia")
            }

            renderTablaEmpleadosSeleccionados()
        },
        open: function (event, ui) {
            $('.ui-autocomplete').appendTo('#AutoCompleteContainer')
            $('.ui-autocomplete').css('z-index', '9999');
            $('.ui-autocomplete').css('position', 'absolute');
            $('.ui-autocomplete').css('top', '');
            $('.ui-autocomplete').css('left', '0');
            $('.ui-autocomplete').css('width', '100%');
        },
        close: function (event, ui) {
            // Limpiar el input después de seleccionar un elemento
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        // // Personaliza cómo se muestra cada elemento en la lista
        return $("<li>")
            .append("<div>" + item.label + "<br><small>" + item.fullData.Cargo + " - " + item.fullData.Empresa + "</small></div>")
            .appendTo(ul);
    };
}

function renderTablaEmpleadosSeleccionados(value) {
    let element = $('#contenedorTableEmpleadosSeleccionados')
    element.empty()
    if (value) {
        arrayEmpleadosSeleccionados = [];
    }
    let tbody = arrayEmpleadosSeleccionados ? arrayEmpleadosSeleccionados.map(item => `
        <tr>
            <td>${item.NombreCompleto}</td>
            <td>${item.NombreDocumentoRegistro.toUpperCase()} - ${item.DocumentoRegistro}</td>
            <td>${item.Cargo}</td>
            <td><a class="btn btn-sm btn-danger quitarEmpleado" data-id="${item.IdEmpleado}">Quitar <span class="glyphicon glyphicon-remove"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered" style="width:100%" id="tableEmpleadosSeleccionadosBienMaterial">
            <thead>
                <tr>
                    <th>Nombres</th>
                    <th>Nro Documento</th>
                    <th>Cargo</th>
                    <th>Acción</th>
                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4" align="center">Agregue un empleado</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)

}

$(document).ready(function () {
    $("#btnExcel_Importar").click(function (e) {
        e.preventDefault();
        $("#fileInput").click();
    });
    $("#fileInput").change(function () {
        let file = this.files[0];

        if (file) {
            let formData = new FormData();
            formData.append("file", file);
            formData.append("fileName", file.name);

            $.ajax({
                url: basePath + "AccionesIncidentesCajasTemporizadas/ImportarExcelAccionesIncidentesCajasTemporizadas", 
                type: "POST",
                data: formData,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.respuesta) {
                        if (response.observaciones.length > 0) {
                            console.log("Observaciones:", response.observaciones);
                        }
                        if (response.excelModificado) {
                            let link = document.createElement("a");
                            link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + response.excelModificado;
                            link.download = response.nombreArchivo;
                            link.click();
                        }
                    }
                },
                error: function (xhr, status, error) { 
                    console.error(xhr.responseText);
                },
                complete: function (resul) {
                    $("#fileInput").val("");
                }
            });
        }
    });
});
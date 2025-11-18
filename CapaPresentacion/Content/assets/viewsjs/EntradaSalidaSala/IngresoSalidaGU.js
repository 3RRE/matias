
//19-11 js
let objetodatatable
let objetodatatable2
let arraySalas = []
let arrayEmpleados = []
let arrayEmpleadosSeleccionados = []
let IdIngresoSalidaGU = 0
let CboSeleccionado = 0
let CboSeleccionado1 = 0
let CboSeleccionado2 = 0
let CboSeleccionado3 = 0
let CboSeleccionado4 = 0
let idIngresoSeleccionado = null;
let shouldTriggerChange = true
let params = {}

$(document).ready(function () {
    //ObtenerListaSalas()
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
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
        buscarIngresoSalidaGU(params);

    } else {
        ObtenerListaSalas()

    }
    // BUSCAR ----------------------------------------------------------
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
        buscarIngresoSalidaGU()
    });

    // Excel Plantilla ------------------------------------------------------------
    $(document).on('click', '#btnExcel_Plantilla', function (e) {
        e.preventDefault()

        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "IngresosSalidasGU/PlantillaIngresosSalidasGUDescargarExcel",
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
    // NUEVO ------------------------------------------------------------
    $(document).on('click', "#btnNuevo", function () {
        limpiarValidadorFormIngresoSalidaGU()
        IdIngresoSalidaGU = 0
        $('#textIngresoSalidaGU').text('Nuevo')
        $('#IdIngresoSalidaGU').val(0)
        $('#cboSalaIngresoSalidaGU').val(null) 
        $('#Descripcion').val('')
        $('#cboMotivo').val(null) 
        $('#DescripcionMotivo').val(null)
        $('#busqueda').val('')
       
        $('#Observaciones').val('')
        arrayEmpleadosSeleccionados = []
        renderTablaEmpleadosSeleccionados(true); 
        $('#HoraIngreso').val(moment().format('DD/MM/YYYY hh:mm A'));
        $("#HoraIngreso").datetimepicker({
            format: 'DD/MM/YYYY hh:mm A',
            defaultDate: moment(),
            pickTime: true
        });
        $('#HoraSalida').val('')
        CboSeleccionado = null
        CboSeleccionado1 = null
        CboSeleccionado2 = null
        CboSeleccionado3 = null
        CboSeleccionado4 = null
        renderSelectSalasModalIngresoSalidaGU()
        buscaobtenerListaMotivoPorEstadorIngresoSalidaGU()
        obtenerListaEmpresas()
        $('#full-modal_entradasalidagu').modal('show')
        var maxLength = $('#Descripcion').attr('maxlength');
        $('#charCountDescripcion').text(0 + '/' + maxLength);
        var maxLengthObservaciones = $('#Observaciones').attr('maxlength');
        $('#charCountObservaciones').text(0 + '/' + maxLengthObservaciones);



    });

    $('#Descripcion').on('input', function () {
        var maxLengthDescripcion = $(this).attr('maxlength');
        var currentLengthDescripcion = $(this).val().length;
        $('#charCountDescripcion').text(currentLengthDescripcion + '/' + maxLengthDescripcion);
    });
    $('#Observaciones').on('input', function () {
        var maxLengthObservaciones = $(this).attr('maxlength');
        var currentLengthDescripcion = $(this).val().length;
        $('#charCountObservaciones').text(currentLengthDescripcion + '/' + maxLengthObservaciones);
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
            url: basePath + "IngresosSalidasGU/ReporteIngresosSalidasGUDescargarExcelJson",
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

    //---NUEVO CONTINUANDO-----------------------------------
    $("#full-modal_entradasalidagu").on("shown.bs.modal", function (e) {

       renderSelectSalasModalIngresoSalidaGU() 

        $(".mySelectbienMaterial").val('')
        $(".mySelectbienMaterial").append('<option value="">---Selecciones---</option>');

        $("#HoraIngreso").datetimepicker({
            pickTime: false,
            format: 'DD/MM/YYYY HH:mm A',
            defaultDate: dateNow,
            pickTime: true
        })
        $("#HoraSalida").datetimepicker({
            pickTime: false,
            format: 'DD/MM/YYYY HH:mm A',
            pickTime: true
        })
       
        $("#cboTipoIngresoSalidaGU").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_entradasalidagu')
        })
        $("#cboTipoIngresoSalidaGU").val(null).trigger("change")
        limpiarValidadorFormIngresoSalidaGU()
        initAutocomplete()
    })

    $('#full-modal_entradasalidagu').on('hidden.bs.modal', function () {
        $("#busqueda").autocomplete("destroy");
    })

    $(document).on('change', '#cboMotivo', function (e) {
        let valorSeleccionado = $(this).val()
        if (valorSeleccionado == -1) {
            $("#MotivoOtros").show()
            $('#IngresoSalidaGUForm').bootstrapValidator('enableFieldValidators', 'DescripcionMotivo', true);
            $('#IngresoSalidaGUForm').bootstrapValidator('updateMessage', 'DescripcionMotivo', 'notEmpty', '');
        }
        else {
            $("#MotivoOtros").hide()
            $('#DescripcionMotivo').val('')
            $('#IngresoSalidaGUForm').bootstrapValidator('enableFieldValidators', 'DescripcionMotivo', false);
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
    // NUEVO VALIDACION ------------------------------------------------------
    $("#IngresoSalidaGUForm")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                CodSala: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },

                IdMotivo: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                Descripcion: {
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
                DescripcionMotivo: {
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
    $(document).on('click', '#btnGuardarIngresoSalidaGU', function (e) {

        e.preventDefault()
                
        $("#IngresoSalidaGUForm").data('bootstrapValidator').resetForm();
        var validarRegistro = $("#IngresoSalidaGUForm").data('bootstrapValidator').validate();

        if (validarRegistro.isValid()) {
             
            let url = basePath
            if (IdIngresoSalidaGU == 0) {
                url += "IngresosSalidasGU/GuardarIngresosSalidasGU"
            } else {
                url += "IngresosSalidasGU/EditarIngresosSalidasGU" 

            } 
            let dataForm = new FormData(document.getElementById("IngresoSalidaGUForm"))

            dataForm.delete('IdIngresoSalidaGU');
            dataForm.append('IdIngresoSalidaGU', IdIngresoSalidaGU);

            dataForm.append('NombreEmpresa', $('#cboEmpresa option:selected').text()) 
            dataForm.append('NombreMotivo', $('#cboMotivo option:selected').text())
            dataForm.append('NombreSala', $('#cboSalaIngresoSalidaGU option:selected').text())
            dataForm.append('Empleados', JSON.stringify(arrayEmpleadosSeleccionados))
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
                        limpiarValidadorFormIngresoSalidaGU();
                        toastr.success(response.mensaje, "Mensaje Servidor") 
                        $("#full-modal_entradasalidagu").modal("hide");
                        buscarIngresoSalidaGU();
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor")
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    toastr.error("Ocurrió un error al guardar el registro.", "Error");
                }
            });
        }
    })




    $(document).on("click", ".btnEditaringresosalidagu", function () {

        limpiarValidadorFormIngresoSalidaGU();

        /* let IdIngresoSalidaGU = parseInt($("#IdIngresoSalidaGU").val()) || 0;*/
        IdIngresoSalidaGU = $(this).data("id"); 

        //carga los datos
        let rowData = objetodatatable
            .rows()
            .data()
            .toArray()
            .find(row => row.IdIngresoSalidaGU === IdIngresoSalidaGU);
             
        if (rowData) {
            $('#textIngresoSalidaGU').text('Editar');


            CboSeleccionado = rowData.CodSala
            CboSeleccionado1 = rowData.TipoIngresoSalidaGU
            CboSeleccionado2 = rowData.IdCategoria
            CboSeleccionado3 = rowData.IdMotivo
            CboSeleccionado4 = rowData.IdEmpresa
            arrayEmpleadosSeleccionados = rowData.Empleados
            renderTablaEmpleadosSeleccionados();
            renderSelectSalasModalIngresoSalidaGU()
            buscaobtenerListaMotivoPorEstadorIngresoSalidaGU(rowData.IdMotivo) 
            obtenerListaEmpresas()
            $('#busqueda').val('')
            
            $('#Descripcion').val(rowData.Descripcion); 
            $('#FechaIngresoSalidaGU').val(rowData.Descripcion);  
            $('#HoraSalida').val((moment(rowData.HoraSalida).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.HoraSalida).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.HoraSalida).format('DD/MM/YYYY HH:mm A'));
            $('#HoraIngreso').val((moment(rowData.HoraIngreso).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.HoraIngreso).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.HoraIngreso).format('DD/MM/YYYY HH:mm A'));
            $('#DescripcionMotivo').val(rowData.DescripcionMotivo); 
            $('#MotivoOtros').val(rowData.DescripcionMotivo); 
            $('#Observaciones').val(rowData.Observaciones); 
            var currentLengthObservaciones = $('#Observaciones').val().length;
            var maxLengthObservaciones = $('#Observaciones').attr('maxlength');
            $('#charCountObservaciones').text(currentLengthObservaciones + '/' + maxLengthObservaciones);
            var currentLength = $('#Descripcion').val().length;
            var maxLength = $('#Descripcion').attr('maxlength');
            $('#charCountDescripcion').text(currentLength + '/' + maxLength);
            $('#full-modal_entradasalidagu').modal('show');

        } else {
            toastr.error("No se encontraron datos para este registro.", "Error");
        }
    });
    $(document).on('click', '.btn_editar_zona2', function (event) {
        event.preventDefault()

        var zonaId = $(this).attr('data-id')
        var elementSala = $('#u_sala_id')
        var elementModal = $('#modal_editar_zona')

        ObtenerZona(zonaId).done(function (response) {
            if (response.status) {
                var data = response.data

                listarSalas(elementSala, elementModal, true, data.SalaId)
                setDataFormZona(data)
            }
        }).then(function () {
            elementModal.modal('show')
        })
    })

    function setDataFormZona(data) {
        var elementId = $('#u_zona_id')
        var elementName = $('#u_zona_nombre')
        var elementStatus = $('#u_zona_estado')

        elementId.val(data.Id)
        elementName.val(data.Nombre)
        elementStatus.html(`
    <option value="1" ${data.Estado ? 'selected' : ''}>Activo</option>
    <option value="0" ${!data.Estado ? 'selected' : ''}>Inactivo</option>
    `)
    }




    $(document).on('click', '#btnAgregarEmpleadoIngresoSalidaGU', function (e) {
        e.preventDefault()
        $('#full-modal_empleadotabla').modal('show')
    })


    $("#full-modal_empleadotabla").on("shown.bs.modal", function () {
        obtenerListaEmpleadosPorEstado()
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
        let url = `${basePath}IngresosSalidasGU/InsertarCategoria`
        if ($('#CategoriaIdCategoria').val() > 0) {
            url = `${basePath}IngresosSalidasGU/EditarCategoria`
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
                    buscaobtenerListaMotivoPorEstadorIngresoSalidaGU()
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
        $("#MotivoIdMotivo").val(idSeleccionado)
        $("#MotivoNombre").val(nombre)
        $("#MotivoEstado").val(estado)
        $("#textoMotivo").text("Editar")
    })
    $(document).on('click', '#btnCancelarMotivo', function (e) {
        e.preventDefault()
        $("#MotivoIdMotivo").val(0)
        $("#MotivoNombre").val('')
        $("#MotivoEstado").val(1)
        $("#textoMotivo").text("Agregar")
    })
    $(document).on('click', '#btnGuardarEditarMotivo', function (e) {
        e.preventDefault()
        let dataForm = {
            IdMotivo: $('#MotivoIdMotivo').val(),
            Nombre: $('#MotivoNombre').val(),
            Estado: $('#MotivoEstado').val(),
        }
        let url = `${basePath}IngresosSalidasGU/InsertarMotivo`
        if ($('#MotivoIdMotivo').val() > 0) {
            url = `${basePath}IngresosSalidasGU/EditarMotivo`
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
                    $("#MotivoIdMotivo").val(0)
                    $("#MotivoNombre").val('')
                    $("#MotivoEstado").val(1)
                    $("#textoMotivo").text("Agregar")
                    //obtenerListaCategorias()
                    //buscaobtenerListaMotivoPorEstadorIngresoSalidaGU()
                    obtenerListaMotivos()
                    buscaobtenerListaMotivoPorEstadorIngresoSalidaGU()
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
        url: basePath + "IngresosSalidasGU/ListarCategoria",
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
        url: basePath + "IngresosSalidasGU/ListarMotivo",
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
            <td>${item.IdMotivo}</td>
            <td>${item.Nombre}</td>
            <td><span class="label label-${item.Estado == 1 ? 'success' : 'danger'}">${item.Estado == 1 ? 'ACTIVO' : 'INACTIVO'}</span></td>
            <td><a class="btn btn-sm btn-danger editarMotivo" data-id="${item.IdMotivo}" data-nombre="${item.Nombre}" data-estado="${item.Estado}">Editar <span class="glyphicon glyphicon-edit"></span></a></td>
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
$(document).on('click', '#eliminarHoraSalida', function (e) {
    e.preventDefault()
    $('#HoraSalida').val('')
})

function renderTablaEmpleadosSeleccionados(value) {
    let element = $('#contenedorTableEmpleadosSeleccionados')
    element.empty()
    if (value) {
        arrayEmpleadosSeleccionados = [];
    } 

    let tbody = arrayEmpleadosSeleccionados ? arrayEmpleadosSeleccionados.map(item => `
        <tr>
            <td>${item.Nombre} ${item.ApellidoPaterno} ${item.ApellidoMaterno}</td>
            <td>${(item.TipoDocumento || item.NombreDocumentoRegistro)?.toUpperCase()} - ${item.DocumentoRegistro}</td>
            <td>${item.NombreCargo || item.Cargo }</td>
            <td><a class="btn btn-sm btn-danger quitarEmpleado" data-id="${item.IdEmpleado}">Quitar <span class="glyphicon glyphicon-remove"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered" style="width:100%" id="tableEmpleadosSeleccionadosIngresoSalidaGU">
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
        </table>
    `
    element.html(htmlContent)

}
function obtenerListaEmpleadosPorEstado() {
    let element = $('#contenedorTableEmpleadosIngresoSalidaGU')
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
                arrayEmpleados = result.data
                let tbody = result.data.map(item => `
                        <tr>
                            <td>${item.Nombre} ${item.ApellidoPaterno} ${item.ApellidoMaterno}</td>
                            <td>${item.TipoDocumento} - ${item.DocumentoRegistro}</td>
                            <td>${item.Cargo.Nombre}</td>
                            <td><a class="btn btn-sm btn-primary seleccionarEmpleado" data-id="${item.IdEmpleado}">Seleccionar <span class="glyphicon glyphicon-check"></span></a></td>
                        </tr>
                    `)
                let htmlContent = `
                    <table class="table table-sm table-bordered" style="width: 100%; height: 50%;"  id="tableEmpleadosIngresoSalidaGU">
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
                    $('#tableEmpleadosIngresoSalidaGU').DataTable()
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

function renderSelectSalasModalIngresoSalidaGU() {
    $("#cboSalaIngresoSalidaGU").html('')
    if (arraySalas) {
        $("#cboSalaIngresoSalidaGU").html(arraySalas.map(item => `<option value="${item.CodSala}">${item.Nombre}</option>`).join(""))
        $('#cboSalaIngresoSalidaGU').val(CboSeleccionado);
        $("#cboSalaIngresoSalidaGU").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_entradasalidagu')
        });
        //$("#cboSalaIngresoSalidaGU").val(null).trigger("change")

    }

}
function obtenerListaEmpresas() {
    $.ajax({
        type: "POST",
        url: basePath + "Empresa/ListadoEmpresa",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboEmpresa").html('')
            if (result.data) {
                $("#cboEmpresa").html(result.data.map(item => `<option value="${item.CodEmpresa}">${item.RazonSocial}</option>`).join(""))
                $('#cboEmpresa').val(CboSeleccionado4);
                $("#cboEmpresa").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_entradasalidagu')
                });
                /* $("#cboEmpresa").val(null).trigger("change")*/
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
function buscaobtenerListaMotivoPorEstadorIngresoSalidaGU(value) {
    $.ajax({
        type: "POST",
        url: basePath + "IngresosSalidasGU/ListarMotivoGUPorEstado",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboMotivo").html('')
            if (result.data) {
                $("#cboMotivo").html(result.data.map(item => `<option value="${item.IdMotivo}">${item.Nombre}</option>`).join(""))
                $('#cboMotivo').append('<option value="-1">OTROS</option>')
               
                $("#cboMotivo").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_entradasalidagu')
                });
                if (value) {
                    $("#cboMotivo").val(value).trigger("change");
                } else {
                    $("#cboMotivo").val(null).trigger("change");

                }
            }
            limpiarValidadorFormIngresoSalidaGU();
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
        //success: function (result) {
        //    var datos = result.data; 
        //    var todosCodSala = datos.map(function (item) { return item.CodSala; }).join(',');

        //    $("#cboSala").append('<option value="' + todosCodSala + '">--TODOS--</option>');

        //    $.each(datos, function (index, value) {
        //        $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
        //    });
        //    $("#cboSala").select2({
        //        multiple: true,
        //        placeholder: "--Seleccione--",
        //        allowClear: true
        //    });
        //    console.log("todosCodSala ", todosCodSala)
        //    $("#cboSala").val(todosCodSala).trigger("change");
        //    buscarMaquina();
        //    buscarConsolidado();
        //},
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    });
    return false;
}
function ObtenerTipoIngresoSalidaGU() {
    $.ajax({
        type: "POST",
        url: basePath + "IngresosSalidasGU/ListadoTipoIngresoSalidaGUJson",
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
function buscarIngresoSalidaGU(params) {
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
        url: basePath + "IngresosSalidasGU/ListarIngresosSalidasGUxSalaJson",
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
            let highlightId = params?.id; // ID del registro que debe resaltarse

            objetodatatable = $("#tableResultado").DataTable({
                destroy: true,
                sort: true,
                scrollCollapse: true,
                scrollX: true,
                paging: true,
                autoWidth: true,
                aaSorting: [[0, 'desc']],
                data: datos,
                columns: [
                    { data: "IdIngresoSalidaGU", title: "ID" },
                    { data: "NombreSala", title: "Sala" },
                    {
                        data: "FechaRegistro",
                        title: "Fecha Reg",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY HH:mm A');
                        }
                    },
                 
                    { data: "NombreMotivo", title: "Motivo" },
                    {
                        data: "HoraIngreso",
                        title: "Hora Ingreso",
                        render: function (data) {
                            return (moment(data).format('DD/MM/YYYY') == "01/01/1753" || moment(data).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(data).format('DD/MM/YYYY hh:mm A');
                        }
                    },
                  
                    {
                        data: "HoraSalida",
                        title: "Hora Salida", 
                        render: function (data) {
                            return (moment(data).format('DD/MM/YYYY') == "01/01/1753" || moment(data).format('DD/MM/YYYY') == "31/12/1752") ? '<span class="label btn-warning">No definido</span>' : moment(data).format('DD/MM/YYYY hh:mm A');
                        }
                    },  
                    {
                        data: "Descripcion",
                        title: "Descripción",
                        render: function (data) {
                            if (data) {
                                return data.length > 20 ? data.substring(0, 20) + "..." : data;
                            }
                            return data == "" ? '<span class="label btn-warning">No definido</span>' : data; 
                        }
                    },
                    {
                        data: "Estado",
                        title: "Estado",
                        "render": function (data, type, row) { 
                            var css = "btn-secondary"; 
                            switch (data) { 
                                case "Finalizado":
                                    css = "btn-success";
                                    break;
                                case "En Espera":
                                    css = "btn-warning";
                                    break;
                                case "Sin Estado":
                                    css = "btn-info";
                                    break; 
                                default:
                                    css = "btn-secondary";
                                    break;
                            }  
                            return '<span class="label ' + css + '">' + data + '</span>';
                        }
                    },
                    {
                        data: "IdIngresoSalidaGU",
                        title: "Accion",
                        "render": function (o, value, oData) {
                            return `
                            <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-warning btnEditaringresosalidagu" data-id="${o}">
                                <i class="glyphicon glyphicon-pencil"></i>
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
                    $('.btnEditaringresosalidagu').tooltip({
                        title: "Editar Ingreso/Salida GU"
                    }); 
                    $('.btnEliminar').tooltip({
                        title: "Eliminar Ingreso/Salida GU"
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
        title: '¿Esta seguro de Continuar?',
        content: '¿Eliminar Ingreso/Salida de GU?',
        confirmButton: 'Ok',
        cancelButton: 'Cerrar',
        confirmButtonClass: 'btn-success',
        cancelButtonClass: 'btn-danger',
        confirm: () => {
            deleteIngresoSalidaGU(id)
        }
    });
});

const deleteIngresoSalidaGU = (idregistro) => {
    const data = {
        id: idregistro
    };
    $.ajax({
        url: `${basePath}IngresosSalidasGU/EliminarIngresosSalidasGU`,
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
            buscarIngresoSalidaGU()
        }
    });
}
 


//VALIDAR-----------------------------------------
function limpiarValidadorFormIngresoSalidaGU() {
    $("#IngresoSalidaGUForm").parent().find('div').removeClass("has-error");
    $("#IngresoSalidaGUForm").parent().find('div').removeClass("has-success");
    $("#IngresoSalidaGUForm").parent().find('i').removeAttr("style").hide();
}
// NUEVO EMPLEADO------------------------------------------------------------
$(document).on('click', "#btnNuevoEmpleado", function () {
    limpiarValidadorFormIngresoSalidaGU()

    $('#textIngresoSalidaGU').text('Nuevo')
    //$('#IdIngresoSalidaGU').val(0)
    //$('#cboSalaIngresoSalidaGU').val(null) 
    //$('#Descripcion').val('')
    //$('#cboMotivo').val(null)
    //$('#cboEmpresa').val(null)
    //$('#GRFFT').val('')
    //$('#RutaImagen').val('')
    //$('#FechaIngreso').val('')
    //$('#Observaciones').val('')
/*    arrayEmpleadosSeleccionados = []*/
    limpiarValidadorFormIngresoSalidaGU()
    $('#full-modal_nuevoempleado').modal('show')
});
$("#full-modal_nuevoempleado").on("shown.bs.modal", function () {

    obtenerListaCargosPorEstado();
    obtenerTiposDocumentos();
})
function obtenerListaCargos() {
    $.ajax({
        type: "POST",
        url: basePath + "IngresosSalidasGU/ListarCargo",
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
        url: `${basePath}IngresosSalidasGU/InsertarEmpleado`,
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
                obtenerListaEmpleadosPorEstado();
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
            url += "IngresosSalidasGU/InsertarCargo"
        } else {
            url += "IngresosSalidasGU/EditarCargo"
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

$(document).on("click", ".btnFinalizaSalidaingresosalidagu", function () {
    idIngresoSalidaGUSeleccionado = $(this).data("id");
    const fechaHora = obtenerFechaHora();
    const mensaje = `¿Está seguro de finalizar el registro a las ${fechaHora}?`;
    $("#confirmationMessage").text(mensaje);

    $("#full-modal_confirmacion").modal("show");
});
$("#confirmFinalizar").on("click", function () {
    if (idIngresoSalidaGUSeleccionado) {
        finalizarIngresoSalidaGU(idIngresoSalidaGUSeleccionado);
    }
    $("#full-modal_confirmacion").modal("hide");
});
function finalizarIngresoSalidaGU(id) {
    $.ajax({
        type: "POST",
        url: basePath + "IngresosSalidasGU/FinalizarHoraRegistroIngresoSalidaGU",
        data: JSON.stringify({ idingresosalidagu: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if (response.respuesta) {
                toastr.success(response.mensaje, "Éxito");
            } else {
                toastr.error(response.mensaje, "Error");
            }
            $("#full-modal_confirmacion").modal("hide");
            buscarIngresoSalidaGU()
        },
        error: function () {
            toastr.error("No se pudo completar la solicitud.", "Error del servidor");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}
function obtenerFechaHora() {
    const ahora = new Date();
    let horas = ahora.getHours();
    const minutos = String(ahora.getMinutes()).padStart(2, '0');
    const periodo = horas >= 12 ? 'PM' : 'AM';

    horas = horas % 12 || 12;

    return `${horas}:${minutos} ${periodo}`;
} 

function initAutocomplete() {
    $("#busqueda").autocomplete({
        source: function (request, response) {

                $.ajax({
                    url: basePath + "BUKEmpleado/ObtenerEmpleadosActivosPorTerminoxCargo", // Reemplaza con la URL de tu endpoint
                    dataType: "json",
                    data: {
                        term: request.term,
                        idcargo: 44
                    },
                    success: function (json) {
                        // Mapea los resultados para el autocomplete
                        var results = $.map(json.data, function (item) {
                            return {
                                // Puedes personalizar qué mostrar y qué valor se selecciona
                                label: item.NombreCompleto + " - " + item.NumeroDocumento,
                                value: item.NombreCompleto,
                                // Guardas todos los datos por si los necesitas después
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
                toastr.warning("Gerente seleccionado. Solo puedes seleccionar uno.", "Advertencia");
                return;
            }
            let existEmpleado = arrayEmpleadosSeleccionados.find(x => x.IdEmpleado == ui.item.fullData.IdBuk)
            if (!existEmpleado) {
                arrayEmpleadosSeleccionados.push({
                    IdEmpleado: ui.item.fullData.IdBuk,
                    Nombre: ui.item.fullData.Nombres,
                    ApellidoPaterno: ui.item.fullData.ApellidoPaterno,
                    ApellidoMaterno: ui.item.fullData.ApellidoMaterno,
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
            $('.ui-autocomplete').css('top', '0');
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
$('#eliminarHoraSalida').tooltip({
    title: "Limpiar Hora Salida"
}); 
$('#agregarMotivo').tooltip({
    title: "Agregar Motivo"
});
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
                url: basePath + "IngresosSalidasGU/ImportarExcelIngresosSalidasGU",
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
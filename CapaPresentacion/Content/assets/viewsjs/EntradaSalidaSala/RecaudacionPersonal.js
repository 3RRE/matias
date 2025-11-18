let objetodatatable
let objetodatatable2
let arraySalas = []
let arrayEmpleados = []
let arrayEmpleadosSeleccionados = []
let EmpleadoSeleccionado = {}
let IdRecaudacionPersonal = 0
let idIngresoSeleccionado = null;
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
        buscarRecaudacionPersonal(params);

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

        buscarRecaudacionPersonal()
    });

    // Excel Plantilla ------------------------------------------------------------
    $(document).on('click', '#btnExcel_Plantilla', function (e) {
        e.preventDefault()

        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "RecaudacionesPersonalParticipante/PlantillaRecaudacionesPersonalParticipanteDescargarExcel",
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
        limpiarValidadorFormRecaudacionPersonal()
        IdRecaudacionPersonal = 0;

        $('#textRecaudacionPersonal').text('Nuevo')
        $('#IdRecaudacionPersonal').val(0)
        $('#cboSalaRecaudacionPersonal').val(null)
        $('#Descripcion').val('')
        $('#NumeroClientes').val(null)
        $('#cboEmpresa').val(null)
        $('#EmpadronamientoInicio').val(null)
        $('#EmpadronamientoFin').val(null)
        $('#RecaudacionFin').val(null)

        $('#RecaudacionInicio').val(moment().format('DD/MM/YYYY hh:mm A'));
        $("#RecaudacionInicio").datetimepicker({
            format: 'DD/MM/YYYY hh:mm A',
            defaultDate: moment(),
            pickTime: true
        });
        $('#EmpadronamientoInicio').val(moment().format('DD/MM/YYYY hh:mm A'));
        $("#EmpadronamientoInicio").datetimepicker({
            format: 'DD/MM/YYYY hh:mm A',
            defaultDate: moment(),
            pickTime: true
        }); 
        $('#Observaciones').val(null)
        $('#cboFuncion').val(null)
        $('#DescripcionFuncion').val(null)
        $('#cboEstadoParticipante').val(null)
        $('#busqueda').val('')
        var maxLengthObservaciones = $('#Observaciones').attr('maxlength');
        $('#charCountObservaciones').text(0 + '/' + maxLengthObservaciones);

        arrayEmpleadosSeleccionados = []
        EmpleadoSeleccionado = {}
        renderTablaEmpleadosSeleccionados(true);  
        renderSelectSalasModalRecaudacionPersonal()
        $("#EmpadronamientoInicio").datetimepicker({
            pickTime: false,
            format: 'DD/MM/YYYY HH:mm A',
            defaultDate: dateNow,
            pickTime: true
        })
        $("#RecaudacionInicio").datetimepicker({
            pickTime: false,
            format: 'DD/MM/YYYY HH:mm A',
            defaultDate: dateNow,
            pickTime: true
        })
        $('#full-modal_recaudacionpersonal').modal('show')
        buscaobtenerListaFuncionPorEstadoLogOcurrencia()

  
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
            url: basePath + "RecaudacionesPersonalParticipante/ReporteRecaudacionesPersonalParticipanteDescargarExcelJson",
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
    $("#full-modal_recaudacionpersonal").on("shown.bs.modal", function (e) {
         
        $(".mySelectbienMaterial").val('')
        $(".mySelectbienMaterial").append('<option value="">---Selecciones---</option>');
        $("#cboFuncion").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_recaudacionpersonal')
        });
        
        $("#RecaudacionFin").datetimepicker({
            pickTime: false,
            format: 'DD/MM/YYYY HH:mm A',
            pickTime: true
        })
       
        $("#EmpadronamientoFin").datetimepicker({
            pickTime: false,
            format: 'DD/MM/YYYY HH:mm A',
            pickTime: true
        }) 
        $("#cboTipoRecaudacionPersonal").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_recaudacionpersonal')
        })
        initAutocomplete()
        limpiarValidadorFormRecaudacionPersonal()
    })
    $(document).on('change', '#cboMotivo', function (e) {
        let valorSeleccionado = $(this).val()
        if (valorSeleccionado == -1) {
            $("#MotivoOtros").show()
        }
        else {
            $("#MotivoOtros").hide()
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
    $("#RecaudacionPersonalForm")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                TipoRecaudacionPersonal: {
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

                NumeroClientes: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                RecaudacionInicio: {
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
    $(document).on('click', '#btnGuardarRecaudacionPersonal', function (e) {

        e.preventDefault() 
        $("#RecaudacionPersonalForm").data('bootstrapValidator').resetForm();
        var validarRegistro = $("#RecaudacionPersonalForm").data('bootstrapValidator').validate();

        if (validarRegistro.isValid()) {

            let url = basePath
            if (IdRecaudacionPersonal == 0) {
                url += "RecaudacionesPersonalParticipante/GuardarRecaudacionesPersonalParticipante"
            } else {
                url += "RecaudacionesPersonalParticipante/EditarRecaudacionesPersonalParticipante"

            }
            let dataForm = new FormData(document.getElementById("RecaudacionPersonalForm"))

            dataForm.delete('IdRecaudacionPersonal');
            dataForm.append('IdRecaudacionPersonal', IdRecaudacionPersonal);
             
            dataForm.append('NombreSala', $('#cboSalaRecaudacionPersonal option:selected').text())
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
                        limpiarValidadorFormRecaudacionPersonal();
                        toastr.success(response.mensaje, "Mensaje Servidor")
                        $("#full-modal_recaudacionpersonal").modal("hide");
                        buscarRecaudacionPersonal();


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

        limpiarValidadorFormRecaudacionPersonal();

        IdRecaudacionPersonal = $(this).data("id"); 

        //carga los datos
        let rowData = objetodatatable
            .rows()
            .data()
            .toArray()
            .find(row => row.IdRecaudacionPersonal === IdRecaudacionPersonal);
             
        if (rowData) {
            $('#textRecaudacionPersonal').text('Editar');
            $('#full-modal_recaudacionpersonal').modal('show');
            $('#busqueda').val('')

            arrayEmpleadosSeleccionados = rowData.Empleados
            renderTablaEmpleadosSeleccionados();
            renderSelectSalasModalRecaudacionPersonal(rowData.CodSala)
            buscaobtenerListaFuncionPorEstadoLogOcurrencia() 
            $("#cboFuncion").val(null).trigger("change");
            $('#DescripcionFuncion').val(rowData.DescripcionFuncion)
            $('#cboEstadoParticipante').val(null)
            $('#Descripcion').val(rowData.Descripcion);
            $('#NumeroClientes').val(rowData.NumeroClientes);
            $('#FechaRecaudacionPersonal').val(rowData.Descripcion);
            
            $('#EmpadronamientoInicio').val((moment(rowData.EmpadronamientoInicio).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.EmpadronamientoInicio).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.EmpadronamientoInicio).format('DD/MM/YYYY hh:mm A'));
            $('#EmpadronamientoFin').val((moment(rowData.EmpadronamientoFin).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.EmpadronamientoFin).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.EmpadronamientoFin).format('DD/MM/YYYY hh:mm A'));
            $('#RecaudacionInicio').val((moment(rowData.RecaudacionInicio).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.RecaudacionInicio).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.RecaudacionInicio).format('DD/MM/YYYY hh:mm A'));
            $('#RecaudacionFin').val((moment(rowData.RecaudacionFin).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.RecaudacionFin).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.RecaudacionFin).format('DD/MM/YYYY hh:mm A'));
            
            $('#Observaciones').val(rowData.Observaciones);
            var currentLengthObservaciones = $('#Observaciones').val().length;
            var maxLengthObservaciones = $('#Observaciones').attr('maxlength');
            $('#charCountObservaciones').text(currentLengthObservaciones + '/' + maxLengthObservaciones);

        } else {
            toastr.error("No se encontraron datos para este registro.", "Error");
        }
    });
   
    function buscaobtenerListaCargoRPPorEstadoRecaudacionPersonal(value) {
        $.ajax({
            type: "POST",
            url: basePath + "RecaudacionesPersonalParticipante/ListarCargoRPPorEstado",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                $("#cboCargo").html('')
                if (result.data) {
                    $("#cboCargo").html(result.data.map(item => `<option value="${item.IdCargo}">${item.Nombre}</option>`).join(""))
                 

                    $("#cboCargo").select2({
                        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_empleadotabla')
                    });
                    if (value) {
                        $("#cboCargo").val(value).trigger("change");
                    } else {
                        $("#cboCargo").val(null).trigger("change");

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
     

    $(document).on('click', '#btnAgregarEmpleadoRecaudacionPersonal', function (e) {
        e.preventDefault()
        $('#full-modal_empleadotabla').modal('show')
    })


    $("#full-modal_empleadotabla").on("shown.bs.modal", function () {
        obtenerListaEmpleadosPorEstado()
        buscaobtenerListaCargoRPPorEstadoRecaudacionPersonal()
    })
    $(document).on('change', '#cboEstadoParticipante', function () {
        let estado = $(this).val();
        if (estado === '1') {
            
            $("#cboFuncion").val(null).trigger("change");

            $('#cboFuncion').prop('disabled', false);
        } else { 
            $("#cboFuncion").val(null).trigger("change");

            $('#cboFuncion').prop('disabled', true);
        }
    });
     
    $(document).on('click', '#btnAgregarEmpleado', function (e) {

        e.preventDefault()
        $("#DetalleRecaudacionPersonal").data('bootstrapValidator').resetForm();
        var validarRegistro = $("#DetalleRecaudacionPersonal").data('bootstrapValidator').validate();
        if (validarRegistro.isValid()) {
            // Validaciones y recolección de información 
            let empleadoSeleccionado = EmpleadoSeleccionado; // El empleado seleccionado desde el autocomplete
            let estadoParticipante = $('#cboEstadoParticipante').val();
            let funcion = estadoParticipante === '1' ? $('#cboFuncion').val() : null; // Función solo si es participante
            let otrofuncion = estadoParticipante === '1' ? $('#DescripcionFuncion').val() : null; // Función solo si es participante
            let cargo = $('#cboCargo').val();
            let nombreCargo = $('#cboCargo option:selected').text();
            let estadoTexto = $('#cboEstadoParticipante option:selected').text();
            let funcionTexto = estadoParticipante === '1' ? $('#cboFuncion option:selected').text() : null;

            if (!empleadoSeleccionado || !empleadoSeleccionado.fullData || !empleadoSeleccionado.fullData.IdBuk) {
                toastr.error('Por favor, seleccione un empleado.');
                return;
            }

            if (!estadoParticipante) {
                toastr.error('Por favor seleccione el estado de participación.');
                return;
            }

            if (estadoParticipante === '1' && !funcion) {
                toastr.error('Por favor seleccione una función para los participantes.');
                return;
            }
            let existEmpleado = arrayEmpleadosSeleccionados.find(x => x.IdEmpleado == empleadoSeleccionado.fullData.IdBuk)
            if (!existEmpleado) {
                arrayEmpleadosSeleccionados.push({
                    IdEmpleado: empleadoSeleccionado.fullData.IdBuk,
                    Nombre: empleadoSeleccionado.fullData.Nombres,
                    ApellidoPaterno: empleadoSeleccionado.fullData.ApellidoPaterno,
                    ApellidoMaterno: empleadoSeleccionado.fullData.ApellidoMaterno,
                    IdTipoDocumentoRegistro: empleadoSeleccionado.fullData.IdTipoDocumentoRegistro,
                    TipoDocumento: empleadoSeleccionado.fullData.TipoDocumento,
                    DocumentoRegistro: empleadoSeleccionado.fullData.NumeroDocumento,
                    IdCargo: empleadoSeleccionado.fullData.IdCargo,
                    NombreCargo: empleadoSeleccionado.fullData.Cargo,
                    Cargo: empleadoSeleccionado.fullData.Cargo,
                    EstadoParticipante: estadoTexto,
                    IdEstadoParticipante: estadoParticipante,
                    NombreFuncion: funcionTexto,
                    IdFuncion: funcion,
                    DescripcionFuncion: otrofuncion,
                    Empresa: empleadoSeleccionado.fullData.Empresa,
                    IdEmpresa: empleadoSeleccionado.fullData.IdEmpresa
                });
            } else {
                toastr.warning("Empleado ya seleccionado", "Advertencia")
            }

            EmpleadoSeleccionado = {}
            $('#busqueda').val('');
            $("#cboFuncion").val(null).trigger("change");
            $("#cboEstadoParticipante").val(null).trigger("change");
            $("#DescripcionFuncion").val("").trigger("change");

            $('#cboCargo').val('');

            renderTablaEmpleadosSeleccionados(); 

        }
        
        
    });

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
        let url = `${basePath}RecaudacionesPersonalParticipante/InsertarCategoria`
        if ($('#CategoriaIdCategoria').val() > 0) {
            url = `${basePath}RecaudacionesPersonalParticipante/EditarCategoria`
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
        let url = `${basePath}RecaudacionesPersonalParticipante/InsertarMotivo`
        if ($('#MotivoIdMotivo').val() > 0) {
            url = `${basePath}RecaudacionesPersonalParticipante/EditarMotivo`
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
        url: basePath + "RecaudacionesPersonalParticipante/ListarCategoria",
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
        url: basePath + "RecaudacionesPersonalParticipante/ListarMotivo",
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

function renderTablaFunciones(data) {
    let element = $('#contenedorListadoFunciones')
    element.empty()

    let tbody = data ? data.map(item => `
        <tr>
            <td>${item.IdFuncion}</td>
            <td>${item.Nombre}</td>
            <td><span class="label label-${item.Estado == 1 ? 'success' : 'danger'}">${item.Estado == 1 ? 'ACTIVO' : 'INACTIVO'}</span></td>
            <td><a class="btn btn-sm btn-danger editarFuncion" data-id="${item.IdFuncion}" data-nombre="${item.Nombre}" data-estado="${item.Estado}">Editar <span class="glyphicon glyphicon-edit"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered table-striped" style="width:100%" id="tableListadoFunciones">
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
        $("#tableListadoFunciones").DataTable()
    }


}
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
            <td>${item.Cargo || item.NombreCargo}</td>
           <td>${item.EstadoParticipante}</td>
           <td>${item.NombreFuncion || 'N/A'}</td>
            <td><a class="btn btn-sm btn-danger quitarEmpleado" data-id="${item.IdEmpleado}">Quitar <span class="glyphicon glyphicon-remove"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered" style="width:100%" id="tableEmpleadosSeleccionadosRecaudacionPersonal">
            <thead>
                <tr>
                    <th>Nombres</th>
                    <th>Nro Documento</th>
                    <th>Cargo</th>
                    <th>Estado</th>
                    <th>Función</th>
                    <th>Acción</th>
                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4">Agregue un empleado</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)

} 
function obtenerListaEmpleadosPorEstado() {
    let element = $('#contenedorTableEmpleadosRecaudacionPersonal')
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
                //arrayEmpleados = result.data
                arrayEmpleados = result.data.map(item => ({
                    ...item,
                    EstadoParticipante: $('input[name="estadoParticipante"]:checked').val(),
                    Funcion: $('#cboFuncion').val(),
                }));
                let tbody = result.data.map(item => `
                        <tr>
                            <td>${item.Nombre} ${item.ApellidoPaterno} ${item.ApellidoMaterno}</td>
                            <td>${item.TipoDocumento} - ${item.DocumentoRegistro}</td>
                    
                              
                            <td><a class="btn btn-sm btn-primary seleccionarEmpleado" data-id="${item.IdEmpleado}">Seleccionar <span class="glyphicon glyphicon-check"></span></a></td>
                        </tr>
                    `)
                let htmlContent = `
                    <table class="table table-sm table-bordered" style="width: 100%; height: 50%;"  id="tableEmpleadosRecaudacionPersonal">
                        <thead>
                            <tr>
                                <th>Nombres</th>
                                <th>Nro Documento</th>
                               
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
                    $('#tableEmpleadosRecaudacionPersonal').DataTable()
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

function renderSelectSalasModalRecaudacionPersonal(value) {
    $("#cboSalaRecaudacionPersonal").html('class="form-control input-sm"')
    if (arraySalas) { 
        $("#cboSalaRecaudacionPersonal").html(arraySalas.map(item => `'class="form-control input-sm"' <option value="${item.CodSala}">${item.Nombre}</option>`).join(""))

        if (value) {
            $('#cboSalaRecaudacionPersonal').val(value);
        } else { 
            $("#cboSalaRecaudacionPersonal").val(null).trigger("change");
        }
        $("#cboSalaRecaudacionPersonal").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_recaudacionpersonal')
        }); 
        limpiarValidadorFormRecaudacionPersonal()

    }

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
 
function buscarRecaudacionPersonal(params) {
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
        url: basePath + "RecaudacionesPersonalParticipante/ListarRecaudacionesPersonalParticipantexSalaJson",
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
                aaSorting: [[0, 'desc']],

                data: datos,
                columns: [
                    { data: "IdRecaudacionPersonal", title: "ID" },
                    { data: "NombreSala", title: "Sala" },
                    {
                        data: "FechaRegistro",
                        title: "Fecha Reg",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY HH:mm A');
                        }
                    }, 
                    {
                        data: "RecaudacionInicio",
                        title: "Recaud Ini",
                        render: function (data) {  
                            return (moment(data).format('DD/MM/YYYY') == "01/01/1753" || moment(data).format('DD/MM/YYYY') == "31/12/1752") ? '<span class="label btn-warning">No definido</span>' : moment(data).format('DD/MM/YYYY hh:mm A');

                        }
                    },  
                    {
                        data: "RecaudacionFin",
                        title: "Recaud Fin",
                        render: function (data) {
                            return (moment(data).format('DD/MM/YYYY') == "01/01/1753" || moment(data).format('DD/MM/YYYY') == "31/12/1752") ? '<span class="label btn-warning">No definido</span>' : moment(data).format('DD/MM/YYYY hh:mm A');


                        }
                    },{
                        data: "EmpadronamientoInicio",
                        title: "Empadron Ini",
                        render: function (data) {
                            return (moment(data).format('DD/MM/YYYY') == "01/01/1753" || moment(data).format('DD/MM/YYYY') == "31/12/1752") ? '<span class="label btn-warning">No definido</span>' : moment(data).format('DD/MM/YYYY hh:mm A');


                        }
                    },{
                        data: "EmpadronamientoFin",
                        title: "Empadron Fin",
                        render: function (data) {
                            return (moment(data).format('DD/MM/YYYY') == "01/01/1753" || moment(data).format('DD/MM/YYYY') == "31/12/1752") ? '<span class="label btn-warning">No definido</span>' : moment(data).format('DD/MM/YYYY hh:mm A');


                        }
                    },
                    { data: "NumeroClientes", title: "Cant Cliente" }, 
                    {
                        data: "IdRecaudacionPersonal",
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
                        title: "Editar Recaudacion-Personal"
                    }); 
                    $('.btnEliminar').tooltip({
                        title: "Eliminar Recaudacion-Personal"
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
            deleteRecaudacionPersonal(id)
        }
    });
});

const deleteRecaudacionPersonal = (idregistro) => {
    const data = {
        id: idregistro
    };
    $.ajax({
        url: `${basePath}RecaudacionesPersonalParticipante/EliminarRecaudacionesPersonalParticipante`,
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
            buscarRecaudacionPersonal()
        }
    });
}



//VALIDAR-----------------------------------------
function limpiarValidadorFormRecaudacionPersonal() {
    $("#RecaudacionPersonalForm").parent().find('div').removeClass("has-error");
    $("#RecaudacionPersonalForm").parent().find('div').removeClass("has-success");
    $("#RecaudacionPersonalForm").parent().find('i').removeAttr("style").hide();
}
// NUEVO EMPLEADO------------------------------------------------------------
$(document).on('click', "#btnNuevoEmpleado", function () {
    limpiarValidadorFormRecaudacionPersonal()

    $('#textRecaudacionPersonal').text('Nuevo')
    $('#IdRecaudacionPersonal').val(0)
    $('#cboSalaRecaudacionPersonal').val(null)
    $('#Descripcion').val('')
    $('#cboMotivo').val(null)  
    $('#FechaIngreso').val('')
    $('#Observaciones').val('')
   
 
    $('#full-modal_nuevoempleado').modal('show')
});
$("#full-modal_nuevoempleado").on("shown.bs.modal", function () { 

    obtenerListaCargosPorEstado();
    obtenerTiposDocumentos();

})
function obtenerListaCargos() {
    $.ajax({
        type: "POST",
        url: basePath + "RecaudacionesPersonalParticipante/ListarCargo",
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
        url: `${basePath}RecaudacionesPersonalParticipante/InsertarEmpleado`,
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
            url += "RecaudacionesPersonalParticipante/InsertarCargo"
        } else {
            url += "RecaudacionesPersonalParticipante/EditarCargo"
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
function limpiarValidadorFormCargo() {
    $("#formCargo").parent().find('div').removeClass("has-error");
    $("#formCargo").parent().find('i').removeAttr("style").hide();

}

$(document).on("click", ".btnFinalizaSalidaingresosalidagu", function () {
    idRecaudacionPersonalSeleccionado = $(this).data("id");
    const fechaHora = obtenerFechaHora();
    const mensaje = `¿Está seguro de finalizar el registro a las ${fechaHora}?`;
    $("#confirmationMessage").text(mensaje);

    $("#full-modal_confirmacion").modal("show");
});
$("#confirmFinalizar").on("click", function () {
    if (idRecaudacionPersonalSeleccionado) {
        finalizarRecaudacionPersonal(idRecaudacionPersonalSeleccionado);
    }
    $("#full-modal_confirmacion").modal("hide");
});
function finalizarRecaudacionPersonal(id) {
    $.ajax({
        type: "POST",
        url: basePath + "RecaudacionesPersonalParticipante/FinalizarHoraRegistroRecaudacionPersonal",
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
            buscarRecaudacionPersonal()
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
$('#full-modal_recaudacionpersonal').on('hidden.bs.modal', function () {
    $("#busqueda").autocomplete("destroy");
})

function initAutocomplete() {
    $("#busqueda").autocomplete({
        source: function (request, response) {
            let idEmpresa = $("#cboEmpresa").val() 
                $.ajax({
                    url: basePath + "BUKEmpleado/ObtenerEmpleadosActivosPorTerminoSinEmpresa", 
                    dataType: "json",
                    data: {
                        term: request.term
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
            EmpleadoSeleccionado = ui.item;
        },
         
        open: function (event, ui) {
            // // Forzar reposicionamiento del menú}

            $('.ui-autocomplete').appendTo('#AutoCompleteContainer')
            $('.ui-autocomplete').css('z-index', '9999');
            $('.ui-autocomplete').css('position', 'absolute');
            $('.ui-autocomplete').css('top', '0');
            $('.ui-autocomplete').css('left', '0');
            $('.ui-autocomplete').css('width', '100%');
        },
        close: function (event, ui) { 
            /*$('#busqueda').val('')*/
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        // // Personaliza cómo se muestra cada elemento en la lista
        return $("<li>")
            .append("<div>" + item.label + "<br><small>" + item.fullData.Cargo + " - " + item.fullData.Empresa + "</small></div>")
            .appendTo(ul);
    };
}
$(document).on('click', '#eliminarEmpadronamientoFin', function (e) {
    e.preventDefault()
    $('#EmpadronamientoFin').val('')
});
$(document).on('click', '#eliminarRecaudacionFin', function (e) {
    e.preventDefault()
    $('#RecaudacionFin').val('')
});
$(document).on('click', '#eliminarEmpadronamientoInicio', function (e) {
    e.preventDefault()
    $('#EmpadronamientoInicio').val('')
})
$(document).on('change', '#cboFuncion', function (e) {
    let valorSeleccionado = $(this).val()
    if (valorSeleccionado == -1) {
        $("#funcionOtros").show()
        $('#DetalleRecaudacionPersonal').bootstrapValidator('enableFieldValidators', 'DescripcionFuncion', true);
        $('#DetalleRecaudacionPersonal').bootstrapValidator('updateMessage', 'DescripcionFuncion', 'notEmpty', '');
      

    }
    else {
        $("#funcionOtros").hide();
        $('#DetalleRecaudacionPersonal').bootstrapValidator('enableFieldValidators', 'DescripcionFuncion', false);
         
        //$('#DetalleRecaudacionPersonal').bootstrapValidator('removeField', 'DescripcionFuncion');
    }
})
$(document).on('click', '#agregarFuncion', function (e) {
    e.preventDefault()
    $("#full-modal_funciones").modal('show')
})
$("#full-modal_funciones").on("shown.bs.modal", function () { 
    obtenerListaFunciones()
})
$("#DetalleRecaudacionPersonal")
    .bootstrapValidator({
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: { 
            DescripcionFuncion: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            }

        }
    }).on('success.field.bv', function (e, data) {
        e.preventDefault();
        var $parent = data.element.parents('.form-group');
        $parent.removeClass('has-success');
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });

//$('#busqueda').bootstrapValidator({
//    excluded: [':disabled', ':hidden', ':not(:visible)'],
//    feedbackIcons: {
//        valid: 'icon icon-check',
//        invalid: 'icon icon-cross',
//        validating: 'icon icon-refresh'
//    },
//    fields: {
//        DescripcionFuncion: {
//            validators: {
//                notEmpty: {
//                    message: ' '
//                }
//            }
//        },
//    }.on('success.field.bv', function (e, data) {
//        e.preventDefault();
//        var $parent = data.element.parents('.form-group');
//        $parent.removeClass('has-success');
//        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
//    });


$(document).on('click', '.editarFuncion', function (e) {
    e.preventDefault()
    let idSeleccionado = $(this).data('id')
    let nombre = $(this).data('nombre')
    let estado = $(this).data('estado')
    $("#FuncionIdFuncion").val(idSeleccionado)
    $("#FuncionNombre").val(nombre)
    $("#FuncionEstado").val(estado)
    $("#textoFuncion").text(" Editar")
})
$(document).on('click', '#btnCancelarFuncion', function (e) {
    e.preventDefault()
    $("#FuncionIdFuncion").val(0)
    $("#FuncionNombre").val('')
    $("#FuncionEstado").val(1)
    $("#textoFuncion").text("Agregar")
})
$(document).on('click', '#btnGuardarEditarFuncion', function (e) {
    e.preventDefault()
    let dataForm = {
        IdFuncion: $('#FuncionIdFuncion').val(),
        Nombre: $('#FuncionNombre').val(),
        Estado: $('#FuncionEstado').val(),
    }
    let url = `${basePath}RecaudacionesPersonalParticipante/InsertarFuncion`
    if ($('#FuncionIdFuncion').val() > 0) {
        url = `${basePath}RecaudacionesPersonalParticipante/EditarFuncion`
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
                $("#FuncionIdFuncion").val(0)
                $("#FuncionNombre").val('')
                $("#FuncionEstado").val(1)
                $("#textoFuncion").text("Agregar")
                 
                obtenerListaFunciones();
                buscaobtenerListaFuncionPorEstadoLogOcurrencia();
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

function obtenerListaFunciones() {
    $.ajax({
        type: "POST",
        url: basePath + "RecaudacionesPersonalParticipante/ListarFuncion",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.data) {
                renderTablaFunciones(result.data)
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
function buscaobtenerListaFuncionPorEstadoLogOcurrencia(value) {
    $.ajax({
        type: "POST",
        url: basePath + "RecaudacionesPersonalParticipante/ListarFuncionPorEstado",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboFuncion").html('')
            if (result.data) {
                $("#cboFuncion").html(result.data.map(item => `<option value="${item.IdFuncion}">${item.Nombre}</option>`).join(""))
                $('#cboFuncion').append('<option value="-1">OTROS</option>')

                $("#cboFuncion").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_recaudacionpersonal')
                });
                if (value) {
                    $("#cboFuncion").val(value).trigger("change");
                } else {
                    $("#cboFuncion").val(null).trigger("change");

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
$('#agregarFuncion').tooltip({
    title: "Agregar Función"
});
$('#eliminarRecaudacionFin').tooltip({
    title: "Limpiar Recaudacion Fin"
});
$('#eliminarEmpadronamientoInicio').tooltip({
    title: "Limpiar Empadronamiento Inicio"
});  
$('#eliminarEmpadronamientoFin').tooltip({
    title: "Limpiar Empadronamiento Fin"
});  
$('#Observaciones').on('input', function () {
    var maxLengthObservaciones = $(this).attr('maxlength');
    var currentLengthDescripcion = $(this).val().length;
    $('#charCountObservaciones').text(currentLengthDescripcion + '/' + maxLengthObservaciones);
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
                url: basePath + "RecaudacionesPersonalParticipante/ImportarExcelRecaudacionesPersonalParticipante",
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
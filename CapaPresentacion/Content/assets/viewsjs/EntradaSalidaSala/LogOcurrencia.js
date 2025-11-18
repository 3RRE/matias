let objetodatatable
let objetodatatable2
let arraySalas = []
let arrayEmpleados = []
let arrayEmpleadosSeleccionados = []
let IdLogOcurrencia = 0
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
        buscarLogOcurrencia(params);

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

        buscarLogOcurrencia()
    });

    // Excel Plantilla ------------------------------------------------------------
    $(document).on('click', '#btnExcel_Plantilla', function (e) {
        e.preventDefault()

        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "LogsOcurrencias/PlantillaLogsOcurrenciasDescargarExcel",
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
        // Limpiar errores de validación
        limpiarValidadorFormLogOcurrencia(); 

        // Cambiar el texto del botón de "Editar" a "Nuevo"
        $('#textLogOcurrencia').text('Nuevo');

        // Restablecer los valores del formulario
        $('#IdLogOcurrencia').val(0);  // ID para nuevo registro
        IdLogOcurrencia = 0;

        // Limpiar los campos de entrada
        $('#cboSalaLogOcurrencia').val(null); 
        $('#FechaSolucion').val(null); 
        $('#FechaRegistro').val(moment().format('DD/MM/YYYY hh:mm A'));
        $('#Fecha').val(moment().format('DD/MM/YYYY hh:mm A'));
        $("#Fecha").datetimepicker({
            format: 'DD/MM/YYYY hh:mm A',
            defaultDate: moment(),
            pickTime: true
        });
        $("#FechaSolucion").datetimepicker({
            pickTime: false,
            format: 'DD/MM/YYYY hh:mm A',
            pickTime: true
        })
        $('#Detalle').val(null);
        $('#cboArea').val("");
        $('#cboTipologia').val("");
        $('#AccionEjecutada').val(null);
        $('#DescripcionArea').val(null);
        $('#DescripcionTipologia').val(null);
        $('#DescripcionActuante').val(null);
        $('#DescripcionComunicacion').val(null);
        $('#charCount').text(0 + '/' + 100);
        $('#charCountDetalle').text(0 + '/' + 300);

        // Limpiar lista de empleados seleccionados (si es necesario)
        arrayEmpleadosSeleccionados = [];

        // Renderizar selects dinámicos si es necesario
        renderSelectSalasModalLogOcurrencia(true);
        buscaobtenerListaAreaPorEstadoLogOcurrencia();
        buscaobtenerListaTipologiaPorEstadoLogOcurrencia();
        buscaobtenerListaActuantePorEstadoLogOcurrencia();
        buscaobtenerListaComunicacionPorEstadoLogOcurrencia();
        buscaobtenerListaEstadoOcurrenciaPorEstadoLogOcurrencia();

        // Abrir el modal para nuevo registro
        $('#full-modal_logocurrencia').modal('show');
        limpiarValidadorFormLogOcurrencia();

    });
    /*agregarArea*/

    $(document).on('click', '#agregarArea', function (e) {
        e.preventDefault()
        $("#full-modal_areas").modal('show')
    })
    $("#full-modal_areas").on("shown.bs.modal", function () {
        obtenerListaAreas()
    })
    /*TIPOLOGIAS*/ 
    $(document).on('click', '#agregarTipologia', function (e) {
        e.preventDefault()
        $("#full-modal_tipologias").modal('show')
    })
    $("#full-modal_tipologias").on("shown.bs.modal", function () {
        obtenerListaTipologias()
    })
    /*agregarActuante*/

    $(document).on('click', '#agregarActuante', function (e) {
        e.preventDefault()
        $("#full-modal_actuantes").modal('show')
    })
    $("#full-modal_actuantes").on("shown.bs.modal", function () {
        obtenerListaActuantes()
    })
    /*agregarComunicacion*/

    $(document).on('click', '#agregarComunicacion', function (e) {
        e.preventDefault()
        $("#full-modal_comunicaciones").modal('show')
    })
    $("#full-modal_comunicaciones").on("shown.bs.modal", function () {
        obtenerListaComunicaciones()
    })
    function obtenerListaAreas() {
        $.ajax({
            type: "POST",
            url: basePath + "LogsOcurrencias/ListarArea",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                if (result.data) {
                    renderTablaAreas(result.data)
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
    function obtenerListaTipologias() {
        $.ajax({
            type: "POST",
            url: basePath + "LogsOcurrencias/ListarTipologia",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                if (result.data) {
                    renderTablaTipologias(result.data)
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
    function obtenerListaActuantes() {
        $.ajax({
            type: "POST",
            url: basePath + "LogsOcurrencias/ListarActuante",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                if (result.data) {
                    renderTablaActuantes(result.data)
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
    function obtenerListaComunicaciones() {
        $.ajax({
            type: "POST",
            url: basePath + "LogsOcurrencias/ListarComunicacion",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                if (result.data) {
                    renderTablaComunicaciones(result.data)
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
    function renderTablaAreas(data) {
        let element = $('#contenedorListadoAreas')
        element.empty()

        let tbody = data ? data.map(item => `
        <tr>
            <td>${item.IdArea}</td>
            <td>${item.Nombre}</td>
            <td><span class="label label-${item.Estado == 1 ? 'success' : 'danger'}">${item.Estado == 1 ? 'ACTIVO' : 'INACTIVO'}</span></td>
            <td><a class="btn btn-sm btn-danger editarArea" data-id="${item.IdArea}" data-nombre="${item.Nombre}" data-estado="${item.Estado}">Editar <span class="glyphicon glyphicon-edit"></span></a></td>
        </tr>
    `) : []
        let htmlContent = `
        <table class="table table-sm table-bordered table-striped" style="width:100%" id="tableListadoAreas">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Nombre</th>
                    <th>Estado</th>
                    <th>Acción</th>
                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4">Agregue una area</td></tr>`}
            </tbody>
        </table>
    `
        element.html(htmlContent)
        if (tbody.length > 0) {
            $("#tableListadoAreas").DataTable()
        } 
    }
    $(document).on('click', '.editarArea', function (e) {
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        let nombre = $(this).data('nombre')
        let estado = $(this).data('estado')
        $("#AreaIdArea").val(idSeleccionado)
        $("#AreaNombre").val(nombre)
        $("#AreaEstado").val(estado)
        $("#textoArea").text("Editar ")
    })
    $(document).on('click', '#btnCancelarArea', function (e) {
        e.preventDefault()
        $("#AreaIdArea").val(0)
        $("#AreaNombre").val('')
        $("#AreaEstado").val(1)
        $("#textoArea").text("Agregar ")
    })
    $(document).on('click', '#btnGuardarEditarArea', function (e) {
        e.preventDefault()
        let dataForm = {
            IdArea: $('#AreaIdArea').val(),
            Nombre: $('#AreaNombre').val(),
            Estado: $('#AreaEstado').val(),
        }
        let url = `${basePath}LogsOcurrencias/InsertarArea`
        if ($('#AreaIdArea').val() > 0) {
            url = `${basePath}LogsOcurrencias/EditarArea`
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
                    $("#AreaIdArea").val(0)
                    $("#AreaNombre").val('')
                    $("#AreaEstado").val(1)
                    $("#textoArea").text("Agregar")
                    obtenerListaAreas()
                    buscaobtenerListaAreaPorEstadoLogOcurrencia()
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
    function renderTablaTipologias(data) {
        let element = $('#contenedorListadoTipologias')
        element.empty()

        let tbody = data ? data.map(item => `
    <tr>
        <td>${item.IdTipologia}</td>
        <td>${item.Nombre}</td>
        <td><span class="label label-${item.Estado == 1 ? 'success' : 'danger'}">${item.Estado == 1 ? 'ACTIVO' : 'INACTIVO'}</span></td>
        <td><a class="btn btn-sm btn-danger editarTipologia" data-id="${item.IdTipologia}" data-nombre="${item.Nombre}" data-estado="${item.Estado}">Editar <span class="glyphicon glyphicon-edit"></span></a></td>
    </tr>
`) : []
        let htmlContent = `
    <table class="table table-sm table-bordered table-striped" style="width:100%" id="tableListadoTipologias">
        <thead>
            <tr>
                <th>Id</th>
                <th>Nombre</th>
                <th>Estado</th>
                <th>Acción</th>
            </tr>
        </thead>
        <tbody>
            ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4">Agregue una area</td></tr>`}
        </tbody>
    </table>
`
        element.html(htmlContent)
        if (tbody.length > 0) {
            $("#tableListadoTipologias").DataTable()
        }
    }
    $(document).on('click', '.editarTipologia', function (e) {
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        let nombre = $(this).data('nombre')
        let estado = $(this).data('estado')
        $("#TipologiaIdTipologia").val(idSeleccionado)
        $("#TipologiaNombre").val(nombre)
        $("#TipologiaEstado").val(estado)
        $("#textoTipologia").text("Editar ")
    })
    $(document).on('click', '#btnCancelarTipologia', function (e) {
        e.preventDefault()
        $("#TipologiaIdTipologia").val(0)
        $("#TipologiaNombre").val('')
        $("#TipologiaEstado").val(1)
        $("#textoTipologia").text("Agregar ")
    })
    $(document).on('click', '#btnGuardarEditarTipologia', function (e) {
        e.preventDefault()
        let dataForm = {
            IdTipologia: $('#TipologiaIdTipologia').val(),
            Nombre: $('#TipologiaNombre').val(),
            Estado: $('#TipologiaEstado').val(),
        }
        let url = `${basePath}LogsOcurrencias/InsertarTipologia`
        if ($('#TipologiaIdTipologia').val() > 0) {
            url = `${basePath}LogsOcurrencias/EditarTipologia`
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
                    $("#TipologiaIdTipologia").val(0)
                    $("#TipologiaNombre").val('')
                    $("#TipologiaEstado").val(1)
                    $("#textoTipologia").text("Agregar")
                    obtenerListaTipologias()
                    buscaobtenerListaTipologiaPorEstadoLogOcurrencia()
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
    function renderTablaActuantes(data) {
        let element = $('#contenedorListadoActuantes')
        element.empty()

        let tbody = data ? data.map(item => `
    <tr>
        <td>${item.IdActuante}</td>
        <td>${item.Nombre}</td>
        <td><span class="label label-${item.Estado == 1 ? 'success' : 'danger'}">${item.Estado == 1 ? 'ACTIVO' : 'INACTIVO'}</span></td>
        <td><a class="btn btn-sm btn-danger editarActuante" data-id="${item.IdActuante}" data-nombre="${item.Nombre}" data-estado="${item.Estado}">Editar <span class="glyphicon glyphicon-edit"></span></a></td>
    </tr>
`) : []
        let htmlContent = `
    <table class="table table-sm table-bordered table-striped" style="width:100%" id="tableListadoActuantes">
        <thead>
            <tr>
                <th>Id</th>
                <th>Nombre</th>
                <th>Estado</th>
                <th>Acción</th>
            </tr>
        </thead>
        <tbody>
            ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4">Agregue una area</td></tr>`}
        </tbody>
    </table>
`
        element.html(htmlContent)
        if (tbody.length > 0) {
            $("#tableListadoActuantes").DataTable()
        }
    }
    $(document).on('click', '.editarActuante', function (e) {
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        let nombre = $(this).data('nombre')
        let estado = $(this).data('estado')
        $("#ActuanteIdActuante").val(idSeleccionado)
        $("#ActuanteNombre").val(nombre)
        $("#ActuanteEstado").val(estado)
        $("#textoActuante").text("Editar ")
    })
    $(document).on('click', '#btnCancelarActuante', function (e) {
        e.preventDefault()
        $("#ActuanteIdActuante").val(0)
        $("#ActuanteNombre").val('')
        $("#ActuanteEstado").val(1)
        $("#textoActuante").text("Agregar ")
    })
    $(document).on('click', '#btnGuardarEditarActuante', function (e) {
        e.preventDefault()
        let dataForm = {
            IdActuante: $('#ActuanteIdActuante').val(),
            Nombre: $('#ActuanteNombre').val(),
            Estado: $('#ActuanteEstado').val(),
        }
        let url = `${basePath}LogsOcurrencias/InsertarActuante`
        if ($('#ActuanteIdActuante').val() > 0) {
            url = `${basePath}LogsOcurrencias/EditarActuante`
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
                    $("#ActuanteIdActuante").val(0)
                    $("#ActuanteNombre").val('')
                    $("#ActuanteEstado").val(1)
                    $("#textoActuante").text("Agregar")
                    obtenerListaActuantes()
                    buscaobtenerListaActuantePorEstadoLogOcurrencia()
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
    function renderTablaComunicaciones(data) {
        let element = $('#contenedorListadoComunicaciones')
        element.empty()

        let tbody = data ? data.map(item => `
    <tr>
        <td>${item.IdComunicacion}</td>
        <td>${item.Nombre}</td>
        <td><span class="label label-${item.Estado == 1 ? 'success' : 'danger'}">${item.Estado == 1 ? 'ACTIVO' : 'INACTIVO'}</span></td>
        <td><a class="btn btn-sm btn-danger editarComunicacion" data-id="${item.IdComunicacion}" data-nombre="${item.Nombre}" data-estado="${item.Estado}">Editar <span class="glyphicon glyphicon-edit"></span></a></td>
    </tr>
`) : []
        let htmlContent = `
    <table class="table table-sm table-bordered table-striped" style="width:100%" id="tableListadoComunicaciones">
        <thead>
            <tr>
                <th>Id</th>
                <th>Nombre</th>
                <th>Estado</th>
                <th>Acción</th>
            </tr>
        </thead>
        <tbody>
            ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4">Agregue una area</td></tr>`}
        </tbody>
    </table>
`
        element.html(htmlContent)
        if (tbody.length > 0) {
            $("#tableListadoComunicaciones").DataTable()
        }
    }
    $(document).on('click', '.editarComunicacion', function (e) {
        e.preventDefault()
        let idSeleccionado = $(this).data('id')
        let nombre = $(this).data('nombre')
        let estado = $(this).data('estado')
        $("#ComunicacionIdComunicacion").val(idSeleccionado)
        $("#ComunicacionNombre").val(nombre)
        $("#ComunicacionEstado").val(estado)
        $("#textoComunicacion").text("Editar ")
    })
    $(document).on('click', '#btnCancelarComunicacion', function (e) {
        e.preventDefault()
        $("#ComunicacionIdComunicacion").val(0)
        $("#ComunicacionNombre").val('')
        $("#ComunicacionEstado").val(1)
        $("#textoComunicacion").text("Agregar ")
    })
    $(document).on('click', '#btnGuardarEditarComunicacion', function (e) {
        e.preventDefault()
        let dataForm = {
            IdComunicacion: $('#ComunicacionIdComunicacion').val(),
            Nombre: $('#ComunicacionNombre').val(),
            Estado: $('#ComunicacionEstado').val(),
        }
        let url = `${basePath}LogsOcurrencias/InsertarComunicacion`
        if ($('#ComunicacionIdComunicacion').val() > 0) {
            url = `${basePath}LogsOcurrencias/EditarComunicacion`
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
                    $("#ComunicacionIdComunicacion").val(0)
                    $("#ComunicacionNombre").val('')
                    $("#ComunicacionEstado").val(1)
                    $("#textoComunicacion").text("Agregar")
                    obtenerListaComunicaciones()
                    buscaobtenerListaComunicacionPorEstadoLogOcurrencia()
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
            url: basePath + "LogsOcurrencias/ReporteLogsOcurrenciasDescargarExcelJson",
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
    $("#full-modal_logocurrencia").on("shown.bs.modal", function (e) {

        $(".mySelectbienMaterial").val('')
        $(".mySelectbienMaterial").append('<option value="">---Selecciones---</option>');

        $("#Fecha").datetimepicker({
            pickTime: false,
            format: 'DD/MM/YYYY HH:mm A',
            defaultDate: dateNow,
            pickTime: true
        }) 
        
        limpiarValidadorFormLogOcurrencia()
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
    $("#LogOcurrenciaForm")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                TipoLogOcurrencia: {
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

                IdEstadoOcurrencia: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                IdArea: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                IdTipologia: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                IdActuante: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                IdComunicacion: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                DescripcionArea: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                DescripcionTipologia: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                DescripcionActuante: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                DescripcionComunicacion: {
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

            }
        })
        .on('success.field.bv', function (e, data) {
            e.preventDefault();
            var $parent = data.element.parents('.form-group');
            $parent.removeClass('has-success');
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
        });


    $(document).on('click', '#btnGuardarLogOcurrencia', function (e) {

        e.preventDefault()
        
        $("#LogOcurrenciaForm").data('bootstrapValidator').resetForm();
        var validarRegistro = $("#LogOcurrenciaForm").data('bootstrapValidator').validate();

        if (validarRegistro.isValid()) {

            let url = basePath
            if (IdLogOcurrencia === 0) {
                url += "LogsOcurrencias/GuardarLogsOcurrencias"
            } else {
                url += "LogsOcurrencias/EditarLogsOcurrencias"

            }
            let dataForm = new FormData(document.getElementById("LogOcurrenciaForm"))

            dataForm.delete('IdLogOcurrencia');
            dataForm.append('IdLogOcurrencia', IdLogOcurrencia);

            dataForm.append('NombreSala', $('#cboSalaLogOcurrencia option:selected').text())
            dataForm.append('NombreArea', $('#cboArea option:selected').text())
            dataForm.append('NombreTipologia', $('#cboTipologia option:selected').text())
            dataForm.append('NombreActuante', $('#cboActuante option:selected').text())
            dataForm.append('NombreComunicacion', $('#cboComunicacion option:selected').text())
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
                        limpiarValidadorFormLogOcurrencia();
                        toastr.success(response.mensaje, "Mensaje Servidor")
                        $("#full-modal_logocurrencia").modal("hide");
                        buscarLogOcurrencia();

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
    $(document).on('change', '#cboArea', function (e) {
        let valorSeleccionado = $(this).val()
        if (valorSeleccionado == -1) {
            $("#areaOtros").show()
            $('#LogOcurrenciaForm').bootstrapValidator('enableFieldValidators', 'DescripcionArea', true);
            $('#LogOcurrenciaForm').bootstrapValidator('updateMessage', 'DescripcionArea', 'notEmpty', '');

        }
        else {
            $("#areaOtros").hide()
            $('#DescripcionArea').val('')
            $('#LogOcurrenciaForm').bootstrapValidator('enableFieldValidators', 'DescripcionArea', false);

        }
    })
    $(document).on('change', '#cboTipologia', function (e) {
        let valorSeleccionado = $(this).val()
        if (valorSeleccionado == -1) {
            $("#tipologiaOtros").show()
            $('#LogOcurrenciaForm').bootstrapValidator('enableFieldValidators', 'DescripcionTipologia', true);
            $('#LogOcurrenciaForm').bootstrapValidator('updateMessage', 'DescripcionTipologia', 'notEmpty', '');

        }
        else {
            $("#tipologiaOtros").hide()
            $('#DescripcionTipologia').val('')
            $('#LogOcurrenciaForm').bootstrapValidator('enableFieldValidators', 'DescripcionTipologia', false);

        }
    })
     $(document).on('change', '#cboActuante', function (e) {
        let valorSeleccionado = $(this).val()
        if (valorSeleccionado == -1) {
            $("#actuanteOtros").show()
            $('#LogOcurrenciaForm').bootstrapValidator('enableFieldValidators', 'DescripcionActuante', true);
            $('#LogOcurrenciaForm').bootstrapValidator('updateMessage', 'DescripcionActuante', 'notEmpty', '');

        }
        else {
            $("#actuanteOtros").hide()
            $('#DescripcionActuante').val('')
            $('#LogOcurrenciaForm').bootstrapValidator('enableFieldValidators', 'DescripcionActuante', false);

        }
    })
    $(document).on('change', '#cboComunicacion', function (e) {
        let valorSeleccionado = $(this).val()
        if (valorSeleccionado == -1) {
            $("#comunicacionOtros").show()
            $('#LogOcurrenciaForm').bootstrapValidator('enableFieldValidators', 'DescripcionComunicacion', true);
            $('#LogOcurrenciaForm').bootstrapValidator('updateMessage', 'DescripcionComunicacion', 'notEmpty', '');

        }
        else {
            $("#comunicacionOtros").hide()
            $('#DescripcionComunicacion').val('')
            $('#LogOcurrenciaForm').bootstrapValidator('enableFieldValidators', 'DescripcionComunicacion', false);

        }
    })

    function buscaobtenerListaAreaPorEstadoLogOcurrencia(value) {
        $.ajax({
            type: "POST",
            url: basePath + "LogsOcurrencias/ListarAreaPorEstado",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                $("#cboArea").html('')
                if (result.data) {
                    $("#cboArea").html(result.data.map(item => `<option value="${item.IdArea}">${item.Nombre}</option>`).join(""))
                    $('#cboArea').append('<option value="-1">OTROS</option>')

                    $("#cboArea").select2({
                        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_logocurrencia')
                    });
                    if (value) {
                        $("#cboArea").val(value).trigger("change");
                    } else {
                        $("#cboArea").val(null).trigger("change");

                    }
                    limpiarValidadorFormLogOcurrencia(); 
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
    function buscaobtenerListaTipologiaPorEstadoLogOcurrencia(value) {
        $.ajax({
            type: "POST",
            url: basePath + "LogsOcurrencias/ListarTipologiaPorEstado",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                $("#cboTipologia").html('')
                if (result.data) {
                    $("#cboTipologia").html(result.data.map(item => `<option value="${item.IdTipologia}">${item.Nombre}</option>`).join(""))
                    $('#cboTipologia').append('<option value="-1">OTROS</option>')

                    $("#cboTipologia").select2({
                        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_logocurrencia')
                    });
                    if (value) {
                        $("#cboTipologia").val(value).trigger("change");
                    } else {
                        $("#cboTipologia").val(null).trigger("change");

                    }
                    limpiarValidadorFormLogOcurrencia(); 
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
    function buscaobtenerListaActuantePorEstadoLogOcurrencia(value) {
        $.ajax({
            type: "POST",
            url: basePath + "LogsOcurrencias/ListarActuantePorEstado",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                $("#cboActuante").html('')
                if (result.data) {
                    $("#cboActuante").html(result.data.map(item => `<option value="${item.IdActuante}">${item.Nombre}</option>`).join(""))
                    $('#cboActuante').append('<option value="-1">OTROS</option>')

                    $("#cboActuante").select2({
                        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_logocurrencia')
                    });
                    if (value) {
                        $("#cboActuante").val(value).trigger("change");
                    } else {
                        $("#cboActuante").val(null).trigger("change");

                    }
                    limpiarValidadorFormLogOcurrencia(); 
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
    function buscaobtenerListaComunicacionPorEstadoLogOcurrencia(value) {
        $.ajax({
            type: "POST",
            url: basePath + "LogsOcurrencias/ListarComunicacionPorEstado",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                $("#cboComunicacion").html('')
                if (result.data) {
                    $("#cboComunicacion").html(result.data.map(item => `<option value="${item.IdComunicacion}">${item.Nombre}</option>`).join(""))
                    $('#cboComunicacion').append('<option value="-1">OTROS</option>')

                    $("#cboComunicacion").select2({
                        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_logocurrencia')
                    });
                    if (value) {
                        $("#cboComunicacion").val(value).trigger("change");
                    } else {
                        $("#cboComunicacion").val(null).trigger("change");

                    }
                    limpiarValidadorFormLogOcurrencia(); 
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
    $('#AccionEjecutada').on('input', function () {
        var maxLength = $(this).attr('maxlength');
        var currentLength = $(this).val().length;
        var remaining = maxLength - currentLength;
        $('#charCount').text(currentLength + '/' + maxLength);
    });
    $('#Detalle').on('input', function () {
        var maxLength = $(this).attr('maxlength');
        var currentLength = $(this).val().length;
        var remaining = maxLength - currentLength;
        $('#charCountDetalle').text(currentLength + '/' + maxLength);
    });
    function buscaobtenerListaEstadoOcurrenciaPorEstadoLogOcurrencia(value) {
        $.ajax({
            type: "POST",
            url: basePath + "LogsOcurrencias/ListarEstadoOcurrenciaPorEstado",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                $("#cboEstadoOcurrencia").html('')
                if (result.data) {
                    $("#cboEstadoOcurrencia").html(result.data.map(item => `<option value="${item.IdEstadoOcurrencia}">${item.Nombre}</option>`).join(""))
          

                    $("#cboEstadoOcurrencia").select2({
                        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_logocurrencia')
                    });
                    if (value) {
                        $("#cboEstadoOcurrencia").val(value).trigger("change");
                    } else {
                        $("#cboEstadoOcurrencia").val(null).trigger("change");

                    }
                    limpiarValidadorFormLogOcurrencia(); 
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

    $(document).on("click", ".btnEditaringresosalidagu", function () {

        limpiarValidadorFormLogOcurrencia();
         
        IdLogOcurrencia = $(this).data("id"); 

        //carga los datos
        let rowData = objetodatatable
            .rows()
            .data()
            .toArray()
            .find(row => row.IdLogOcurrencia === IdLogOcurrencia);
             
        if (rowData) {
            $('#textLogOcurrencia').text('Editar');
            $('#full-modal_logocurrencia').modal('show');

            renderSelectSalasModalLogOcurrencia(rowData.CodSala)
            buscaobtenerListaAreaPorEstadoLogOcurrencia(rowData.IdArea)
            buscaobtenerListaTipologiaPorEstadoLogOcurrencia(rowData.IdTipologia)
            buscaobtenerListaActuantePorEstadoLogOcurrencia(rowData.IdActuante)
            buscaobtenerListaComunicacionPorEstadoLogOcurrencia(rowData.IdComunicacion)
            buscaobtenerListaEstadoOcurrenciaPorEstadoLogOcurrencia(rowData.IdEstadoOcurrencia) 
         
            $('#FechaSolucion').val((moment(rowData.FechaSolucion).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.FechaSolucion).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.FechaSolucion).format('DD/MM/YYYY hh:mm A'));
            $('#Fecha').val((moment(rowData.Fecha).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.Fecha).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.Fecha).format('DD/MM/YYYY hh:mm A'));

            $('#Detalle').val(rowData.Detalle);
            $('#DescripcionArea').val(rowData.DescripcionArea);
            $('#DescripcionTipologia').val(rowData.DescripcionTipologia);
            $('#DescripcionActuante').val(rowData.DescripcionActuante);
            $('#DescripcionComunicacion').val(rowData.DescripcionComunicacion);
            $('#AccionEjecutada').val(rowData.AccionEjecutada);
            var currentLength = $('#AccionEjecutada').val().length;
            var maxLength = $('#AccionEjecutada').attr('maxlength');
            $('#charCount').text(currentLength + '/' + maxLength);
             
            var currentLengthDetalle = $('#Detalle').val().length;
            var maxLengthDetalle = $('#Detalle').attr('maxlength');
            $('#charCountDetalle').text(currentLengthDetalle + '/' + maxLengthDetalle);

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




    $(document).on('click', '#btnAgregarEmpleadoLogOcurrencia', function (e) {
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
        let url = `${basePath}LogsOcurrencias/InsertarCategoria`
        if ($('#CategoriaIdCategoria').val() > 0) {
            url = `${basePath}LogsOcurrencias/EditarCategoria`
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
        let url = `${basePath}LogsOcurrencias/InsertarMotivo`
        if ($('#MotivoIdMotivo').val() > 0) {
            url = `${basePath}LogsOcurrencias/EditarMotivo`
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
        url: basePath + "LogsOcurrencias/ListarCategoria",
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
        url: basePath + "LogsOcurrencias/ListarMotivo",
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


function renderTablaEmpleadosSeleccionados() {
    let element = $('#contenedorTableEmpleadosSeleccionados')
    element.empty()

    let tbody = arrayEmpleadosSeleccionados ? arrayEmpleadosSeleccionados.map(item => `
        <tr>
            <td>${item.Nombre} ${item.ApellidoPaterno} ${item.ApellidoMaterno}</td>
            <td>${item.TipoDocumento} - ${item.DocumentoRegistro}</td>
            <td>${item.NombreCargo}</td>
            <td><a class="btn btn-sm btn-danger quitarEmpleado" data-id="${item.IdEmpleado}">Quitar <span class="glyphicon glyphicon-remove"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered" style="width:100%" id="tableEmpleadosSeleccionadosLogOcurrencia">
            <thead>
                <tr>
                    <th>Nombres</th>
                    <th>Nro Documento</th>
                    <th>Cargo</th>
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
    let element = $('#contenedorTableEmpleadosLogOcurrencia')
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
                    <table class="table table-sm table-bordered" style="width: 100%; height: 50%;"  id="tableEmpleadosLogOcurrencia">
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
                    $('#tableEmpleadosLogOcurrencia').DataTable()
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

function renderSelectSalasModalLogOcurrencia(value) {
    $("#cboSalaLogOcurrencia").html('class="form-control input-sm"')
    if (arraySalas) {
        $("#cboSalaLogOcurrencia").html(arraySalas.map(item => `'class="form-control input-sm"' <option value="${item.CodSala}">${item.Nombre}</option>`).join(""))

        if (value) {
            $('#cboSalaLogOcurrencia').val(value);
        } else {
            $("#cboSalaLogOcurrencia").val(null).trigger("change");
        }
        $("#cboSalaLogOcurrencia").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_logocurrencia')
        });
        //$("#cboSalaLogOcurrencia").val(null).trigger("change")

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

function buscarLogOcurrencia(params) {
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
        url: basePath + "LogsOcurrencias/ListarLogsOcurrenciaxSalaJson",
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
                    { data: "IdLogOcurrencia", title: "ID" },
                    { data: "NombreSala", title: "Sala" },
                    {
                        data: "FechaRegistro",
                        title: "Fec. Reg",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY hh:mm A');
                        }
                    },
                    {
                        data: "Fecha",
                        title: "Fec. Ocurrencia",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY hh:mm A');
                        }
                    },
                    { data: "NombreArea", title: "Area" },
                    { data: "NombreTipologia", title: "Tipologia" },
                    { data: "NombreActuante", title: "Actuante" }, 
                    { data: "NombreComunicacion", title: "Comunicacion" },
                   
                    {
                        data: "FechaSolucion",
                        title: "Fec. Solucion",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY') == "01/01/1753" ? '<span class="label btn-warning">No definido</span>' : moment(data).format('DD/MM/YYYY') == "31/12/1752" ? '<span class="label btn-warning">No definido</span>' : moment(data).format('DD/MM/YYYY hh:mm A');

                        }
                    }, 
                    {
                        data: "EstadoOcurrencia",  
                        title: "Estado",
                        "render": function (data, type, row) {
                            var estado = row.EstadoOcurrencia;  
                            var idEstado = row.IdEstadoOcurrencia;  
                            var css = "btn-secondary";  

                            
                            switch (idEstado) {
                                case 0:
                                    css = "btn-danger";  
                                    break;
                                case 1:
                                    css = "btn-success";  
                                    break;
                                case 2:
                                    css = "btn-warning";  
                                    break;
                                case 3:
                                    css = "btn-info";  
                                    break;
                                case 4:
                                    css = "btn-primary";  
                                    break;
                                default:
                                    css = "btn-secondary";  
                                    break;
                            }

                            
                            return '<span class="label ' + css + '">' + estado + '</span>';
                        }
                    },
                    {
                        data: "IdLogOcurrencia",
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
                        title: "Editar Log Ocurrencia"
                    });
                    $('.btnEliminar').tooltip({
                        title: "Eliminar Log Ocurrencia"
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
        content: '¿Eliminar un registro de Log de Ocurrencia?',
        confirmButton: 'Ok',
        cancelButton: 'Cerrar',
        confirmButtonClass: 'btn-success',
        cancelButtonClass: 'btn-danger',
        confirm: () => {
            deleteLogOcurrencia(id)
        }
    });
});

const deleteLogOcurrencia = (idregistro) => {
    const data = {
        id: idregistro
    };
    $.ajax({
        url: `${basePath}LogsOcurrencias/EliminarLogsOcurrencias`,
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
            buscarLogOcurrencia()
        }
    });
}



//VALIDAR-----------------------------------------
function limpiarValidadorFormLogOcurrencia() { 
    $("#LogOcurrenciaForm").parent().find('div').removeClass("has-error"); 
    $("#LogOcurrenciaForm").parent().find('i').removeAttr("style").hide();
}
// NUEVO EMPLEADO------------------------------------------------------------
$(document).on('click', "#btnNuevoEmpleado", function () {
    limpiarValidadorFormLogOcurrencia()

    $('#textLogOcurrencia').text('Nuevo')
    $('#IdLogOcurrencia').val(0)
    

    $('#cboSalaLogOcurrencia').val(null)
    $('#Descripcion').val('')
    $('#cboMotivo').val(null)
    $('#cboEmpresa').val(null)
    $('#GRFFT').val('')
    $('#RutaImagen').val('')
    $('#FechaIngreso').val('')
    $('#Observaciones').val('')
    arrayEmpleadosSeleccionados = []

    $('#full-modal_nuevoempleado').modal('show')
});
$("#full-modal_nuevoempleado").on("shown.bs.modal", function () {

    obtenerListaCargosPorEstado();
    obtenerTiposDocumentos();
})
function obtenerListaCargos() {
    $.ajax({
        type: "POST",
        url: basePath + "LogsOcurrencias/ListarCargo",
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
        url: `${basePath}LogsOcurrencias/InsertarEmpleado`,
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
            url += "LogsOcurrencias/InsertarCargo"
        } else {
            url += "LogsOcurrencias/EditarCargo"
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
    idLogOcurrenciaSeleccionado = $(this).data("id");
    const fechaHora = obtenerFechaHora();
    const mensaje = `¿Está seguro de finalizar el registro a las ${fechaHora}?`;
    $("#confirmationMessage").text(mensaje);

    $("#full-modal_confirmacion").modal("show");
});
$("#confirmFinalizar").on("click", function () {
    if (idLogOcurrenciaSeleccionado) {
        finalizarLogOcurrencia(idLogOcurrenciaSeleccionado);
    }
    $("#full-modal_confirmacion").modal("hide");
});
function finalizarLogOcurrencia(id) {
    $.ajax({
        type: "POST",
        url: basePath + "LogsOcurrencias/FinalizarHoraRegistroLogOcurrencia",
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
            buscarLogOcurrencia()
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
$('#agregarArea').tooltip({
    title: "Agregar Área"
});
$('#agregarTipologia').tooltip({
    title: "Agregar Tipología"
});
$('#agregarActuante').tooltip({
    title: "Agregar Actuante"
});
$('#agregarComunicacion').tooltip({
    title: "Agregar Comunicación"
});
$('#eliminarfechasolucion').tooltip({
    title: "Limpiar Fecha Solución"
}); 
$(document).on('click', '#eliminarfechasolucion', function (e) {
    e.preventDefault()
    $('#FechaSolucion').val('')
})

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
                url: basePath + "LogsOcurrencias/ImportarExcelLogsOcurrencias",
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

let objetodatatable
let objetodatatable2
let arraySalas = []
let arrayEmpleados = []
let arrayEmpleadosSeleccionados = []
let params = {}
let IdEnteRegulador = 0
let empresaSeleccionada = null
let shouldTriggerChange = true
$(document).ready(function ()
{
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
    })
    $(document).on('click', '#descargarArchivo', function (e) {
        e.preventDefault()
        let href = $(this).attr('href')
        //let text = $(this).data('ruta')
        // console.log($(this))
        // window.open(text, '', 'width=640,height=480');
        window.open(href, '_blank');
    })

    $('#Descripcion').on('input', function () {
        var maxLengthDescripcion = $(this).attr('maxlength');
        var currentLengthDescripcion = $(this).val().length;
        $('#charCountDescripcion').text(currentLengthDescripcion + '/' + maxLengthDescripcion);
    })
    $('#Observaciones').on('input', function () {
        var maxLengthObservaciones = $(this).attr('maxlength');
        var currentLengthObservaciones = $(this).val().length;
        $('#charCountObservaciones').text(currentLengthObservaciones + '/' + maxLengthObservaciones);
    })

    $('#DescripcionMotivo').on('input', function () {
        var maxLengthDescripcionMotivo = $(this).attr('maxlength');
        var currentLengthDescripcionMotivo = $(this).val().length;
        $('#charCountDescripcionMotivo').text(currentLengthDescripcionMotivo + '/' + maxLengthDescripcionMotivo);
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
        buscarEnteRegulador(params);

    } else {
        ObtenerListaSalas()

    }
});
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
        buscarEnteRegulador()
    });

// Excel Plantilla ------------------------------------------------------------
$(document).on('click', '#btnExcel_Plantilla', function (e) {
    e.preventDefault()

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "EntesReguladores/PlantillaEntesReguladoresDescargarExcel",
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
    $(document).on('click', "#btnNuevo", function (e)
    {
        e.preventDefault()
        limpiarValidadorFormEnteRegulador()
        IdEnteRegulador = 0;

        $('#textEnteRegulador').text('Nuevo')
        $('#IdEnteRegulador').val(0)
        $('#cboSalaEnteRegulador').val(null)
        $('#cboAutorizar').val(null)
        $('#Descripcion').val('')
        $('#cboMotivo').val(null)
        $('#Observaciones').val('')
        $('#cboEmpresa').val(null).trigger('change')
     
        $('#DocReferencia').val('')  
        $('#FechaIngreso').val(moment().format('DD/MM/YYYY hh:mm A'));
        $("#FechaIngreso").datetimepicker({
            format: 'DD/MM/YYYY hh:mm A',
            defaultDate: moment(),
            pickTime: true
        }); 
        $('#DescripcionMotivo').val('')

        $('#RutaImagen').val('')
        $('#descargarArchivo').attr('href', '#');
        $('#descargarArchivo').attr('data-ruta', '')
        $('#descargarArchivo').hide();
        $("#ArchivoExiste").hide()  
        $('#descargarArchivo').text('');

        renderTablaEmpleadosSeleccionados(true); 
        renderSelectSalasModalEnteRegulador()
        obtenerListaMotivosPorEstado()
        obtenerListaEmpleadosPorEstado()


        $('#full-modal_enteregulador').modal('show')

        var maxLengthDescripcion = $('#Descripcion').attr('maxlength');
        $('#charCountDescripcion').text(0 + '/' + 300);
        var maxLengthDescripcionMotivo = $('#DescripcionMotivo').attr('maxlength');
        $('#charCountDescripcionMotivo').text(0 + '/' + 100);
        var maxLengthOObservaciones = $('#Observaciones').attr('maxlength');
        $('#charCountObservaciones').text(0 + '/' + 300);

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
        url: basePath + "EntesReguladores/ReporteEntesReguladoresDescargarExcelJson",
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


// MODAL ----------------------------------------------------------

$("#full-modal_enteregulador").on("shown.bs.modal", function (e) {

    
    $('#RutaImagen').val(null); 
    initAutocomplete()
    limpiarValidadorFormEnteRegulador()
})

$("#full-modal_empleadotabla").on("shown.bs.modal", function () {

    ListarPersonaEntidadPublicaMod()
})
$("#enteReguladorForm")
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
            IdAutoriza: {
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
            IdEmpresa: {
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
            Descripcion: {
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
            }
        }
    }).on('success.field.bv', function (e, data) {
        e.preventDefault();
        var $parent = data.element.parents('.form-group');
        $parent.removeClass('has-success');
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });
    $(document).on('click', '#btnGuardarEnteRegulador', function (e) {
        e.preventDefault()
        // NUEVO VALIDACION ------------------------------------------------------
        
        $("#enteReguladorForm").data('bootstrapValidator').resetForm();
        var validarRegistro = $("#enteReguladorForm").data('bootstrapValidator').validate();

        if (validarRegistro.isValid()) {
            let url = basePath
            if (IdEnteRegulador === 0) {
                url += "EntesReguladores/GuardarEntesReguladores"
            } else {
                url += "EntesReguladores/EditarEntesReguladores"
            }

            let dataForm = new FormData(document.getElementById("enteReguladorForm"))

            dataForm.delete('IdEnteRegulador');
            dataForm.append('IdEnteRegulador', IdEnteRegulador);

            dataForm.append('NombreEmpresa', $('#cboEmpresa option:selected').text())
            dataForm.append('NombreAutoriza', $('#cboAutorizar option:selected').text())
            dataForm.append('NombreMotivo', $('#cboMotivo option:selected').text())
            dataForm.append('NombreSala', $('#cboSalaEnteRegulador option:selected').text())
            dataForm.append('PersonasEntidadPublica', JSON.stringify(arrayEmpleadosSeleccionados))
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
                        limpiarValidadorFormEnteRegulador();
                        toastr.success(response.mensaje, "Mensaje Servidor")
                        $('#full-modal_enteregulador').modal('hide');
                        buscarEnteRegulador();
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


$(document).on("click", ".btnEditarbien", function () {
    limpiarValidadorFormEnteRegulador();

    IdEnteRegulador = $(this).data("id");

    let rowData = objetodatatable
        .rows()
        .data()
        .toArray()
        .find(row => row.IdEnteRegulador === IdEnteRegulador);

    if (rowData) {
        $('#textEnteRegulador').text('Editar');
        arrayEmpleadosSeleccionados = rowData.PersonasEntidadPublica;

   
        renderTablaEmpleadosSeleccionados();

        renderSelectSalasModalEnteRegulador(rowData.CodSala);
        obtenerListaMotivosPorEstado(rowData.IdMotivo);
        obtenerListaEmpleadosPorEstado(rowData.IdAutoriza);
        ObtenerListaEntidadPublica(rowData.IdEmpresa);

        $('#FechaIngreso').val(moment(rowData.FechaIngreso).format('DD/MM/YYYY') == "01/01/1753" ? "" : moment(rowData.FechaIngreso).format('DD/MM/YYYY hh:mm A')); 
        $('#DocReferencia').val(rowData.DocReferencia);

        $('#DescripcionMotivo').val(rowData.DescripcionMotivo);
        var currentLengthDescripcionMotivo = $('#DescripcionMotivo').val().length;
        var maxLengthDescripcionMotivo = $('#DescripcionMotivo').attr('maxlength');
        $('#charCountDescripcionMotivo').text(currentLengthDescripcionMotivo + '/' + maxLengthDescripcionMotivo);

        $('#Descripcion').val(rowData.Descripcion);
        var currentLengthDescripcion = $('#Descripcion').val().length;
        var maxLengthDescripcion = $('#Descripcion').attr('maxlength');
        $('#charCountDescripcion').text(currentLengthDescripcion + '/' + maxLengthDescripcion);

        $('#Observaciones').val(rowData.Observaciones);
        var currentLengthObservaciones = $('#Observaciones').val().length;
        var maxLengthObservaciones = $('#Observaciones').attr('maxlength');
        $('#charCountObservaciones').text(currentLengthObservaciones + '/' + maxLengthObservaciones);



        if (rowData.RutaImagen) {
            var timestamp = new Date().getTime();
            var rutaImagenUrl = rowData.RutaImagen + '?timestamp=' + new Date().getTime();
            $('#descargarArchivo').attr('href', rutaImagenUrl);

            // $('#descargarArchivo').attr('target', '_blank');
            var fileName = rowData.RutaImagen.split("/").pop();
            $('#descargarArchivo').text('Ver ' + fileName);
            $('#descargarArchivo').attr('data-ruta', rutaImagenUrl)
            $("#ArchivoExiste").show() 
            $('#descargarArchivo').show();
        } else {
            $('#descargarArchivo').hide();
            $("#ArchivoExiste").hide() 

        }

        $('.modal').modal('hide');
        $('#full-modal_enteregulador').modal('show');

    } else {
        toastr.error("No se encontraron datos para este registro.", "Error");
    }
});


$('#RutaImagen').on('change', function (e) {
    var file = e.target.files[0];

    if (file) {
        var url = URL.createObjectURL(file);
        var fileName = file.name;
        var maxLength = 30;
        var fileNameDisplay = fileName;

        if (fileName.length > maxLength) {
            var partLength = Math.floor((maxLength - 3) / 2);
            fileNameDisplay = fileName.substring(0, partLength) + '...' + fileName.substring(fileName.length - partLength);
        }
        $('#descargarArchivo').attr('data-ruta', url);
        $('#descargarArchivo').attr('href', url);
        $('#descargarArchivo').text('Ver ' + fileNameDisplay);
        $('#descargarArchivo').show();
        $("#ArchivoExiste").show();

    }
});







    $(document).on('click', '#btnAgregarEmpleadoEnteRegulador', function (e) {
        e.preventDefault()
        $('#full-modal_empleadotabla').modal('show')
    })

$(document).on('click', '.seleccionarEmpleado', function (e) {
    e.preventDefault();

    let idSeleccionado = $(this).data('id');
    let elementoSeleccionado = arrayEmpleados.find(x => x.PersonaEntidadPublicaID == idSeleccionado);


    arrayEmpleadosSeleccionados.push({
        PersonaEntidadPublicaID: elementoSeleccionado.PersonaEntidadPublicaID,
        Nombres: elementoSeleccionado.Nombres,
        Apellidos: elementoSeleccionado.Apellidos,
        EntidadPublicaNombre: elementoSeleccionado.EntidadPublicaNombre,
        Dni: elementoSeleccionado.Dni,
        CargoEntidadNombre: elementoSeleccionado.CargoEntidadNombre,
        FechaRegistro: elementoSeleccionado.FechaRegistro
    });

    renderTablaEmpleadosSeleccionados();
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
        arrayEmpleadosSeleccionados = arrayEmpleadosSeleccionados.filter(x => x.PersonaEntidadPublicaID != idSeleccionado)
        renderTablaEmpleadosSeleccionados()

    })

// MODAL FORMULARIO MOTIVO -----------------------------

$(document).on('change', '#cboMotivo', function (e) {
    let valorSeleccionado = $(this).val()
    if (valorSeleccionado == -1) {
        $("#motivoOtros").show()
        $('#enteReguladorForm').bootstrapValidator('enableFieldValidators', 'DescripcionMotivo', true);
        $('#enteReguladorForm').bootstrapValidator('updateMessage', 'DescripcionMotivo', 'notEmpty', '');
    }
    else {
        $("#motivoOtros").hide()
        $('#DescripcionMotivo').val('')
        $('#enteReguladorForm').bootstrapValidator('enableFieldValidators', 'DescripcionMotivo', false);

    }
})

$(document).on('click', '#agregarMotivo', function (e) {
    e.preventDefault()
    $("#full-modal_motivos").modal('show')
})

$("#full-modal_motivos").on("shown.bs.modal", function () {
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

    if ($('#MotivoNombre').val().trim() === '') {
        toastr.error("El campo Nombre es obligatorio", "Mensaje de Error");
        return; 
    }

    let dataForm = {
        IdMotivo: $('#MotivoIdMotivo').val(),
        Nombre: $('#MotivoNombre').val(),
        Estado: $('#MotivoEstado').val(),
    }
    let url = `${basePath}EntesReguladores/InsertarMotivo`
    if ($('#MotivoIdMotivo').val() > 0) {
        url = `${basePath}EntesReguladores/EditarMotivo`
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
                obtenerListaMotivosPorEstado()
                toastr.success(response.mensaje, "Mensaje Servidor");
            }
            else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
            $("#full-modal_motivos").modal('hide')
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            toastr.error("Error Servidor", "Mensaje Servidor");
        }
    });
})






// FUNCIONES ----------------------------------------------------------

function obtenerListaEnteRegulador() {
    $.ajax({
        type: "POST",
        url: basePath + "EntesReguladores/ListarEnteRegulador",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            if (result.data) {
                renderTablaEnteRegulador(result.data)
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
        url: basePath + "EntesReguladores/ListarMotivo",
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

function renderTablaEnteRegulador(data) {
    let element = $('#contenedorListadoEnteRegulador')
    element.empty()

    let tbody = data ? data.map(item => `
        <tr>
            <td>${item.IdEnteRegulador}</td>
            <td>${item.Nombre}</td>
            <td><span class="label label-${item.Estado == 1 ? 'success' : 'danger'}">${item.Estado == 1 ? 'ACTIVO' : 'INACTIVO'}</span></td>
            <td><a class="btn btn-sm btn-danger editarEnteRegulador" data-id="${item.IdEnteRegulador}" data-nombre="${item.Nombre}" data-estado="${item.Estado}">Editar <span class="glyphicon glyphicon-edit"></span></a></td>
        </tr>
    `) : []
    let htmlContent = `
        <table class="table table-sm table-bordered table-striped" style="width:100%" id="tableListadoEnteRegulador">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Nombre</th>
                    <th>Estado</th>
                    <th>Accion</th>
                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4">Agregue una EnteRegulador</td></tr>`}
            </tbody>
        </table>
    `
    element.html(htmlContent)
    if (tbody.length > 0) {
        $("#tableListadoEnteRegulador").DataTable()
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
                    <th>Accion</th>
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

function renderTablaEmpleadosSeleccionados(value) {
    let element = $('#contenedorTableEmpleadosSeleccionados');
    element.empty();


    if (value) {
        arrayEmpleadosSeleccionados = [];
    }

    let tbody = arrayEmpleadosSeleccionados ? arrayEmpleadosSeleccionados.map(item => `
        <tr>
            <td>${item.Nombres} ${item.Apellidos}</td>
            <td>${item.Dni}</td>
            <td>${item.EntidadPublicaNombre}</td>
            <td>${item.CargoEntidadNombre}</td>
     
            <td>
                <a class="btn btn-sm btn-danger quitarEmpleado" data-id="${item.PersonaEntidadPublicaID}">
                    Quitar <span class="glyphicon glyphicon-remove"></span>
                </a>
            </td>
        </tr>
    `) : []

    let htmlContent = `
        <table class="table table-sm table-bordered" style="width:100%" id="tablePersonaEntidadPublicaSeleccionada">
            <thead>
                <tr>
                    <th>Nombres</th>
                    <th>Nro Documento</th>
                    <th>Entidad Publica</th>
                    <th>Cargo</th>
             
                    <th>Accion</th>
                </tr>
            </thead>
            <tbody>
                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="6" class="text-center">Agregue una persona</td></tr>`}
            </tbody>
        </table>
    `;

    element.html(htmlContent);
}





//function obtenerListaEmpleadosPorEstado(value) {
//    $.ajax({
//        type: "POST",
//        url: basePath + "EntesReguladores/ListarEmpleadoPorEstado",
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
//                $("#cboAutorizar").html(result.data.map(item => `<option value="${item.Nombre} ${item.ApellidoPaterno}">${item.Nombre} ${item.ApellidoPaterno}</option>`).join(""));
//                if (value) {
//                    $('#cboAutorizar').val(value);
//                } else {
//                    $('#cboAutorizar').val(null).trigger("change");
//                }

//                $("#cboAutorizar").select2({
//                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_enteregulador')
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

function obtenerListaEmpleadosPorEstado(value) {
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
                $("#cboAutorizar").html(result.data.map(item => `<option value="${item.IdAutoriza}" >${item.NumeroDocumento} - ${item.NombreAutoriza} - ${item.Cargo}</option>`).join(""));
                if (value) {
                    $('#cboAutorizar').val(value);
                } else {
                    $('#cboAutorizar').val(null).trigger("change");
                }
                limpiarValidadorFormEnteRegulador()

                $("#cboAutorizar").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_enteregulador')
                });
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

//function ListarPersonaEntidadPublicaMod() {
//    let element = $('#contenedorTablePersonaEntidadPublica');
//    element.empty();

//    let selectedEntidadPublicaID = $('#cboEmpresa').val();
//    if (!selectedEntidadPublicaID) {
//        toastr.error("Debe seleccionar una entidad publica.", "Error");
//        return;
//    }

//    $.ajax({
//        type: "POST",
//        url: basePath + "CALPersonaEntidadPublica/ListarPersonaEntidadPublicaJson",
//        contentType: "application/json",
//        data: JSON.stringify({}),
//        beforeSend: function () {
//            $.LoadingOverlay("show");
//        },
//        success: function (response) {
//            if (response.data) {
//                let filteredEmpleados = response.data.filter(item => item.EntidadPublicaID == selectedEntidadPublicaID);

//                arrayEmpleados = filteredEmpleados.map(item => ({
//                    ...item,
//                    FechaRegistroFormateada: item.FechaRegistro ? new Date(parseInt(item.FechaRegistro.replace(/\/Date\((\d+)\)\//, '$1'))) : null
//                }));

//                let tbody = arrayEmpleados.map(item => `
//                    <tr>
//                        <td>${item.Nombres}</td>
//                        <td>${item.Dni ? 'DNI - ' + item.Dni : 'OTRO - ' + item.DocumentoRegistro}</td>
//                        <td>${item.CargoEntidadNombre}</td>
//                        <td>
//                            <button type="button" class="btn btn-sm btn-primary seleccionarEmpleado" data-id="${item.PersonaEntidadPublicaID}">
//                                SELECCIONAR <span class="glyphicon glyphicon-check"></span>
//                            </button>
//                        </td>
//                    </tr>
//                `);

//                let htmlContent = `
//                    <div class="table-responsive">
//                        <table class="table table-sm table-bordered" style="width:100%" id="tablePersonaEntidadPublica">
//                            <thead>
//                                <tr>
//                                    <th>NOMBRES</th>
//                                    <th>NRO DOCUMENTO</th>
//                                    <th>CARGO</th>
//                                    <th>ACCION</th>
//                                </tr>
//                            </thead>
//                            <tbody>
//                                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4"><p class="text-center text-danger">No se encontraron resultados</p></td></tr>`}
//                            </tbody>
//                        </table>
//                    </div>
//                `;
//                element.html(htmlContent);

//                if (tbody.length > 0) {
//                    $('#tablePersonaEntidadPublica').DataTable({
//                        "paging": true,
//                        "lengthMenu": [10, 25, 50, 100],
//                    });
//                }
//            } else {
//                toastr.error("No se encontraron datos.", "Mensaje Servidor");
//            }
//        },
//        error: function () {
//            toastr.error("Error al cargar los datos.", "Mensaje Servidor");
//        },
//        complete: function () {
//            $.LoadingOverlay("hide");
//        }
//    });
//}

    
function ListarPersonaEntidadPublicaMod() {
    let element = $('#contenedorTableEmpleadosSeleccionados');
    element.empty();

    // Obtener el ID de la entidad pública seleccionada
    let selectedEntidadPublicaID = $('#cboEmpresa').val();
    if (!selectedEntidadPublicaID) {
        toastr.error("Debe seleccionar una entidad pública.", "Error");
        return;  // Detiene la ejecución si no se selecciona una entidad pública
    }

    // Llamada AJAX para obtener las personas activas
    $.ajax({
        type: "GET",  // El controlador usa GET
        url: basePath + "ESS_EnteRegulador/ObtenerPersonasActivasPorPatronEntesReguladores", // Cambia esta URL según el controlador
        data: {
            entidadPublicaID: selectedEntidadPublicaID, // El ID de la entidad pública
            term: ""  // Puedes agregar un término de búsqueda si lo necesitas
        },
        dataType: "json",
        beforeSend: function () {
            $.LoadingOverlay("show");  // Muestra la capa de carga
        },
        success: function (response) {
            if (response.data) {
                // Filtra las personas que corresponden a la entidad pública seleccionada
                let filteredPersonas = response.data.filter(item => item.EntidadPublicaID == selectedEntidadPublicaID);

                // Procesa los datos de la respuesta
                let arrayPersonas = filteredPersonas.map(item => ({
                    ...item,
                    FechaRegistroFormateada: item.FechaRegistro ? new Date(item.FechaRegistro).toLocaleDateString() : null  // Formatea la fecha si existe
                }));

                // Construcción de las filas de la tabla
                let tbody = arrayPersonas.map(item => `
                    <tr>
                        <td>${item.Nombres} ${item.Apellidos}</td>
                        <td>${item.Dni ? 'DNI - ' + item.Dni : 'OTRO - ' + item.TipoDOI}</td>
                        <td>${item.CargoEntidadNombre ? item.CargoEntidadNombre : 'Sin Cargo'}</td>
                        <td>
                            <button type="button" class="btn btn-sm btn-primary seleccionarEmpleado" data-id="${item.PersonaEntidadPublicaID}">
                                SELECCIONAR <span class="glyphicon glyphicon-check"></span>
                            </button>
                        </td>
                    </tr>
                `);

                // Creación del HTML de la tabla
                let htmlContent = `
                    <div class="table-responsive">
                        <table class="table table-sm table-bordered" style="width:100%" id="tablePersonaEntidadPublica">
                            <thead>
                                <tr>
                                    <th>NOMBRES</th>
                                    <th>NRO DOCUMENTO</th>
                                    <th>CARGO</th>
                                    <th>ACCION</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4"><p class="text-center text-danger">No se encontraron resultados</p></td></tr>`}
                            </tbody>
                        </table>
                    </div>
                `;

                // Inserta el HTML en el contenedor
                element.html(htmlContent);

                // Inicializa DataTable si hay resultados
                if (tbody.length > 0) {
                    $('#tablePersonaEntidadPublica').DataTable({
                        "paging": true,
                        "lengthMenu": [10, 25, 50, 100],
                    });
                }
            } else {
                toastr.error("No se encontraron datos.", "Mensaje Servidor");
            }
        },
        error: function () {
            toastr.error("Error al cargar los datos.", "Mensaje Servidor");
        },
        complete: function () {
            $.LoadingOverlay("hide");  // Oculta la capa de carga
        }
    });
}


//function ListarPersonaEntidadPublicaMod() {
//    let element = $('#contenedorTablePersonaEntidadPublica');
//    element.empty();
//    $.ajax({
//        type: "POST",
//        url: basePath + "CALPersonaEntidadPublica/ListarPersonaEntidadPublicaJson",
//        contentType: "application/json",
//        data: JSON.stringify({}),
//        beforeSend: function () {
//            $.LoadingOverlay("show");
//        },
//        success: function (response) {
//            if (response.data) {
//                arrayEmpleados = response.data.map(item => ({
//                    ...item,
//                    FechaRegistroFormateada: item.FechaRegistro ? new Date(parseInt(item.FechaRegistro.replace(/\/Date\((\d+)\)\//, '$1'))) : null
//                }));

//                let tbody = arrayEmpleados.map(item => `
//                    <tr>
//                        <td>${item.Nombres}</td>
//                        <td>${item.Dni ? 'DNI - ' + item.Dni : 'OTRO - ' + item.DocumentoRegistro}</td>
//                        <td>${item.CargoEntidadNombre}</td>
//                        <td>
//                            <button type="button" class="btn btn-sm btn-primary seleccionarEmpleado" data-id="${item.PersonaEntidadPublicaID}">
//                                SELECCIONAR <span class="glyphicon glyphicon-check"></span>
//                            </button>
//                        </td>
//                    </tr>
//                `);

//                let htmlContent = `
//                    <div class="table-responsive">
//                        <table class="table table-sm table-bordered" style="width:100%" id="tablePersonaEntidadPublica">
//                            <thead>
//                                <tr>
//                                    <th>NOMBRES</th>
//                                    <th>NRO DOCUMENTO</th>
//                                    <th>CARGO</th>
//                                    <th>ACCION</th>
//                                </tr>
//                            </thead>
//                            <tbody>
//                                ${tbody.length > 0 ? tbody.join("") : `<tr><td colspan="4"><p class="text-center text-danger">No se encontraron resultados</p></td></tr>`}
//                            </tbody>
//                        </table>
//                    </div>
//                `;
//                element.html(htmlContent);

//                if (tbody.length > 0) {
//                    $('#tablePersonaEntidadPublica').DataTable({
//                        "paging": true,
//                        "lengthMenu": [10, 25, 50, 100],
//                    });
//                }
//            }
//        },
//        error: function () {
//            toastr.error("Error al cargar los datos.", "Mensaje Servidor");
//        },
//        complete: function () {
//            $.LoadingOverlay("hide");
//        }
//    });
//}



//function ListarPersonaEntidadPublica() {
//    var url = basePath + "CALPersonaEntidadPublica/ListarPersonaEntidadPublicaJson";
//    var data = {}; var respuesta = "";
//    $.ajax({
//        url: url,
//        type: "POST",
//        contentType: "application/json",
//        data: JSON.stringify(data),
//        beforeSend: function () {
//            $.LoadingOverlay("show");
//        },
//        complete: function () {
//            $.LoadingOverlay("hide");
//        },
//        success: function (response) {
//            respuesta = response.data
//            objetodatatable = $("#tablePersonaEntidadPublica").DataTable({
//                "bDestroy": true,
//                "bSort": true,
//                "ordering": true,
//                "scrollCollapse": true,
//                "scrollX": true,
//                "paging": true,
//                "aaSorting": [],
//                "autoWidth": false,
//                "bProcessing": true,
//                "bDeferRender": true,
//                data: response.data,
//                columns: [
//                    //{ data: "PersonaEntidadPublicaID", title: "ID" },
//                    { data: "Nombres", title: "Nombres" },
//                    { data: "Apellidos", title: "Apellidos" },
//                    { data: "EntidadPublicaNombre", title: "Entidad Publica" },
//                    { data: "Dni", title: "DOI" },
//                    { data: "CargoEntidadNombre", title: "Cargo Entidad" },
//                    { data: "Meses", title: "Meses" },
//                    {
//                        data: "FechaRegistro", title: "Fecha Registro",
//                        "render": function (o) {
//                            return moment(o).format("DD/MM/YYYY");
//                        }
//                    },
//                    {
//                        data: "Estado", title: "Estado",
//                        "render": function (o) {
//                            var estado = "INACTIVO";
//                            var css = "btn-danger";
//                            if (o == 1) {
//                                estado = "ACTIVO"
//                                css = "btn-success";
//                            }
//                            return '<span class="label ' + css + '">' + estado + '</span>';

//                        }
//                    },

//                    {
//                        data: "PersonaEntidadPublicaID", title: "Acción",
//                        "bSortable": false,
//                        "render": function (o) {
//                            return `<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="${o}"><i class="glyphicon glyphicon-pencil"></i></button> 
//                                    <button type="button" class="btn btn-xs btn-danger btnEliminar" data-id="${o}"><i class="glyphicon glyphicon-remove"></i></button> `;
//                        }
//                    }
//                ],
//                "drawCallback": function (settings) {
//                    $('.btnEditar').tooltip({
//                        title: "Editar"
//                    });
//                },

//                "initComplete": function (settings, json) {



//                },
//            });
//            $('.btnEditar').tooltip({
//                title: "Editar"
//            });

//        },
//        error: function (xmlHttpRequest, textStatus, errorThrow) {

//        }
//    });
//};  


//function obtenerListaEmpresas(value) {
//    $.ajax({
//        type: "POST",
//        url: basePath + "Empresa/ListadoEmpresa",
//        cache: false,
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        beforeSend: function (xhr) {
//            $.LoadingOverlay("show");
//        },
//        success: function (result) {
//            $("#cboEmpresa").html('')
//            if (result.data) {
//                $("#cboEmpresa").html(result.data.map(item => `<option value="${item.CodEmpresa}">${item.RazonSocial}</option>`).join(""))

//                if (value) {
//                    $('#cboEmpresa').val(value);
//                } else {
//                    $('#cboEmpresa').val(null).trigger("change");
//                }

//                $("#cboEmpresa").select2({
//                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_enteregulador')
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
function obtenerListaMotivosPorEstado(value) {
    $.ajax({
        type: "POST",
        url: basePath + "EntesReguladores/ListarMotivoPorEstado",
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
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_enteregulador')
                });
                if (value) {
                    $('#cboMotivo').val(value).trigger("change");
                } else {
                    $('#cboMotivo').val(null).trigger("change");
                }
                limpiarValidadorFormEnteRegulador()


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

function renderSelectSalasModalEnteRegulador(value) {
    $("#cboSalaEnteRegulador").html('class="form-control input-sm"')
    if (arraySalas) {
        $("#cboSalaEnteRegulador").html(arraySalas.map(item => `'class="form-control input-sm"' <option value="${item.CodSala}">${item.Nombre}</option>`).join(""))

        if (value) {
            $('#cboSalaEnteRegulador').val(value);
        } else {
            $("#cboSalaEnteRegulador").val(null).trigger("change");
        }
        $("#cboSalaEnteRegulador").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_enteregulador')
        });

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

function buscarEnteRegulador(params) {
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
        url: basePath + "EntesReguladores/EnteReguladorListarxSalaFechaJson",
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
                data: datos,
                aaSorting: [[0, 'desc']],
                columns: [
                    { data: "IdEnteRegulador", title: "ID" },
                    { data: "NombreSala", title: "Sala" },
                    {
                        data: "FechaRegistro",
                        title: "Fecha Reg",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    }, 
                    { data: "NombreMotivo", title: "Motivo I/S" },
                    { data: "NombreEmpresa", title: "Empresa / Institucion" },
                    {
                        data: "FechaIngreso",
                        title: "Fecha Ingreso",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        data: "FechaIngreso",
                        title: "Ingreso",
                        render: function (data) {
                            return moment(data).format('HH:mm A');
                        }
                    },
                    {
                        data: "FechaSalida",
                        title: "Salida",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY') == "01/01/1753" ? "" : moment(data).format('DD/MM/YYYY') == "31/12/1752" ? '<span class="label btn-warning">No definido</span>' : moment(data).format('HH:mm A');
                        }
                    },
                    { data: "NombreAutoriza", title: "Autoriza" },
                    {
                        data: "DocReferencia", title: "Doc.Ref.",
                        render: function (data) {
                            return data == "" ? '<span class="label btn-warning">No definido</span>' : data
                        }                    },
                    {
                        data: "IdEnteRegulador",
                        title: "Accion",
                        "render": function (o) {
                            return `
                                            <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-warning btnEditarbien" data-id="${o}">
                                                <i class="glyphicon glyphicon-pencil"></i>
                                            </button> 
                                             <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-primary btnFinalizarSalidabien" data-id="${o}">
                                                 <i class="glyphicon glyphicon-flag"></i>
                                            </button>
                                            <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-danger btnEliminarRegistro"  data-id="${o}">
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

                    $('.btnEditarbien').tooltip({
                        title: "Editar Registro"
                    });

                    $('.btnFinalizarSalidabien').tooltip({
                        title: "Finalizar Registro"
                    });
                    $('.btnEliminarRegistro').tooltip({
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


$(document).on("click", ".btnEliminarRegistro", function () {
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
            deleteEnteRegulador(id)
        }
    });
});

const deleteEnteRegulador = (idregistro) => {
    const data = { 
        id: idregistro
    };
    $.ajax({
        url: `${basePath}EntesReguladores/EliminarEntesReguladores`,
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
            buscarEnteRegulador();
        }
    });
}


// NUEVO EMPLEADO------------------------------------------------------------

$(document).on('change', '#cboEmpresa', function (e) {
    if (!shouldTriggerChange) {
        shouldTriggerChange = true;
        return;
    }
    e.preventDefault()
    empresaSeleccionada = $(this).val()
    /* arrayEmpleadosSeleccionados = []*/
    renderTablaEmpleadosSeleccionados()
    $('#busqueda').val('')
}) 


$('#btnNuevaPersonaEntidad').on('click', function (e) {
    LimpiarFormValidator();
    $("#textPersonaEntidadPublica").text("Nuevo");

    $("#persona_entidad_publica_id").val(0);
   /* $('#IdEnteRegular').val(0)*/
    $("#nombres").val("");
    $("#apellidos").val("");
    $("#dni").val("");
    $("#meses").val("");
    $("#cboEstado").val(null).trigger("change");
    $("#cboCargoEntidad").val(null).trigger("change");
    $("#cboEntidadPublica").val(null).trigger("change");
    $("#cboTipoDocumento").val(null).trigger("change");
    $("#full-modal_persona_entidad_publica").modal("show");
});

$('.btnGuardar').on('click', function (e) {
    $("#form_registro_persona_entidad_publica").data('bootstrapValidator').resetForm();
    var validar = $("#form_registro_persona_entidad_publica").data('bootstrapValidator').validate();
    if (validar.isValid()) {
        var id = $("#persona_entidad_publica_id").val();
        var urlenvio = "";
        var lugar = "";
        var accion = "";
        if (id != 0) {
            lugar = "CALPersonaEntidadPublica/PersonaEntidadPublicaEditarJson";
            accion = "ACTUALIZAR PERSONA ENTIDAD PUBLICA";
            urlenvio = basePath + "CALPersonaEntidadPublica/PersonaEntidadPublicaEditarJson";
        }
        else {
            lugar = "CALPersonaEntidadPublica/PersonaEntidadPublicaGuardarJson";
            accion = "NUEVO PERSONA ENTIDAD PUBLICA";
            urlenvio = basePath + "CALPersonaEntidadPublica/PersonaEntidadPublicaGuardarJson";
        }

        var dataForm = $('#form_registro_persona_entidad_publica').serializeFormJSON();

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
     /*           ListarPersonaEntidadPublicaMod();*/
            },
            success: function (response) {
                if (response.respuesta) {
                    $('#persona_entidad_publica_id').val("0");
                    $('#nombre').val("");
                    $('#estado').val("0");
                    $("#full-modal_persona_entidad_publica").modal("hide");
                    LimpiarFormValidator();
                    //$("#btnBuscar").click();
                    toastr.success("Persona Entidad Publica Guardado", "Mensaje Servidor");
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

$(document).on("click", ".btnEditar", function () {
    LimpiarFormValidator();
    var id = $(this).data("id");
    $("#textPersonaEntidadPublica").text("Editar");
    ObtenerRegistro(id);
    $("#full-modal_persona_entidad_publica").modal("show");
});



$(document).on('click', "#btnNuevoEmpleado", function () {
    limpiarValidadorFormEnteRegulador()

    $('#textEnteRegulador').text('Nuevo')
    $('#IdEnteRegulador').val(0) // 0 para guardar al ajax
    $('#cboSalaEnteRegulador').val(null)
    $('#cboEmpleadoAutorizar').val(null)
    $('#Descripcion').val('')
    $('#cboMotivo').val(null)
    $('#cboEmpresa').val(null)
    $('#DocReferencia').val('')
    $('#RutaImagen').val('')
    $('#FechaIngreso').val('')
    $('#Observaciones').val('')

    $('#full-modal_nuevoempleado').modal('show')
});
$("#full-modal_nuevoempleado").on("shown.bs.modal", function () {

    obtenerListaCargosPorEstado();
    obtenerTiposDocumentos();
    //obtenerListaEmpleadosPorEstado();
    //ListarPersonaEntidadPublica()
})
function obtenerListaCargos() {
    $.ajax({
        type: "POST",
        url: basePath + "EntesReguladores/ListarCargo",
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
        url: `${basePath}EntesReguladores/InsertarEmpleado`,
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
             /*   ListarPersonaEntidadPublica()*/
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
        url: basePath + "EntesReguladores/ListarCargoPorEstado",
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
            url += "EntesReguladores/InsertarCargo"
        } else {
            url += "EntesReguladores/EditarCargo"
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

function limpiarValidadorFormMotivo() {
    $("#formMotivo").parent().find('div').removeClass("has-error");
    $("#formMotivo").parent().find('i').removeAttr("style").hide();

}

$(document).on("click", ".btnFinalizarSalidabien", function () {
    idEnteReguladorSeleccionado = $(this).data("id");
    const mensaje = "\u00BFEst\u00E1s seguro de finalizar el registro?";
    $("#confirmationMessage").text(mensaje);

    const horaActual = obtenerHoraActual();
    $("#horaInput").val(horaActual);  // Setea la hora actual en el campo de input

    $("#full-modal_confirmacion").modal("show");
});

$("#confirmFinalizar").on("click", function () {
    const horaFinalizada = $("#horaInput").val();  // Obtener la hora modificada
    if (idEnteReguladorSeleccionado) {
        finalizarEnteRegulador(idEnteReguladorSeleccionado, horaFinalizada);
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

function finalizarEnteRegulador(id, hora) {
    $.ajax({
        type: "POST",
        url: basePath + "EntesReguladores/FinalizarHoraRegistroEnteRegulador",
        data: JSON.stringify({ identeregulador: id, horaSalida: hora }),
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
            buscarEnteRegulador();
        },
        error: function () {
            toastr.error("No se pudo completar la solicitud.", "Error del servidor");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}



$(document).ready(function () {


    $("#cboTipoDocumento").select2({
        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_persona_entidad_publica')

    });
    ObtenerTipoDocumento();


   /* ListarPersonaEntidadPublica();*/

    $("#cboEstado").select2({
        placeholder: "--Seleccione--", dropdownParent: $('#full-modal_persona_entidad_publica')

    });
    $("#cboEstado").val(null).trigger("change");

    ObtenerListaCargoEntidad();
    ObtenerListaEntidadPublica();

    $(document).on('keypress', '#dni,#meses', function (event) {
        var regex = new RegExp("^[0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });

    $(document).on("click", ".btnEditar", function () {
        LimpiarFormValidator();
        var id = $(this).data("id");
        $("#textPersonaEntidadPublica").text("Editar");
        ObtenerRegistro(id);
        $("#full-modal_persona_entidad_publica").modal("show");
    });

    

    $(document).on("click", ".btnEliminar", function () {
        var elemento = $(this);
        var id = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar Persona Entidad Publica?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {

                $.ajax({
                    url: basePath + "CALPersonaEntidadPublica/PersonaEntidadPublicaEliminarJson",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({ id }),
                    beforeSend: function () {
                        $.LoadingOverlay("show");
                    },
                    complete: function () {
                        $.LoadingOverlay("hide");
                    },
                    success: function (response) {
                        if (response.respuesta) {
                          /*  ListarPersonaEntidadPublica();*/
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

            },

            cancel: function () {
                //close
            },

        });



    });

    $("#form_registro_persona_entidad_publica")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                nombres: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                apellidos: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                dni: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                meses: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                estado: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                cargoEntidadID: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                entidadPublicaID: {
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


});

function ObtenerTipoDocumento() {
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
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboTipoDocumento").append('<option value="' + value.Id + '"  >' + value.Nombre + '</option>');
            });
            $("#cboTipoDocumento").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_persona_entidad_publica')

            });
            $("#cboTipoDocumento").val(null).trigger("change");
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

function ObtenerListaCargoEntidad() {
    $.ajax({
        type: "POST",
        url: basePath + "CALCargoEntidad/ListarCargoEntidadJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboCargoEntidad").append('<option value="' + value.CargoEntidadID + '"  >' + value.Nombre + '</option>');
            });
            $("#cboCargoEntidad").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_persona_entidad_publica')

            });
            $("#cboCargoEntidad").val(null).trigger("change");
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

function ObtenerListaEntidadPublica(value) {
    $.ajax({
        type: "POST",
        url: basePath + "CALEntidadPublica/ListarEntidadPublicaJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboEntidadPublica").append('<option value="' + value.EntidadPublicaID + '"  >' + value.Nombre + '</option>');
            });
            if (value) {
                $('#cboEntidadPublica').val(value);
            } else {
                $("#cboEntidadPublica").val(null).trigger("change");
            }
            $("#cboEntidadPublica").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_persona_entidad_publica')

            });


            $.each(datos, function (index, value) {
                $("#cboEmpresa").append('<option value="' + value.EntidadPublicaID + '"  >' + value.Nombre + '</option>');
            });
            if (value) {
                shouldTriggerChange = false;
                $('#cboEmpresa').val(value);
            } else {
                $("#cboEmpresa").val(null).trigger("change");
            }
            $("#cboEmpresa").select2({
                placeholder: "--Seleccione--", dropdownParent: $('#full-modal_enteregulador')

            });

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

function ObtenerRegistro(id) {

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CALPersonaEntidadPublica/ListarPersonaEntidadPublicaIdJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ PersonaEntidadPublicaID: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {

            if (response.respuesta) {
                response = response.data;
                $("#persona_entidad_publica_id").val(response.PersonaEntidadPublicaID);
                $("#nombres").val(response.Nombres);
                $("#apellidos").val(response.Apellidos);
                $("#dni").val(response.Dni);
                $("#meses").val(response.Meses);
                $("#cboEstado").val(response.Estado).trigger("change");
                $("#cboCargoEntidad").val(response.CargoEntidadID).trigger("change");
                $("#cboEntidadPublica").val(response.EntidadPublicaID).trigger("change");
                $("#cboTipoDocumento").val(response.TipoDOI).trigger("change");
                $("#full-modal_politico").modal("show");
            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};


function LimpiarFormValidator() {
    $("#form_registro_persona_entidad_publica").parent().find('div').removeClass("has-error");
    $("#form_registro_persona_entidad_publica").parent().find('div').removeClass("has-success");
    $("#form_registro_persona_entidad_publica").parent().find('i').removeAttr("style").hide();

}
//VALIDAR-----------------------------------------
function limpiarValidadorFormEnteRegulador() {
    $("#enteReguladorForm").parent().find('div').removeClass("has-error");
    $("#enteReguladorForm").parent().find('div').removeClass("has-success");
    $("#enteReguladorForm").parent().find('i').removeAttr("style").hide();

}

function initAutocomplete() {
    $("#busqueda").autocomplete({
        source: function (request, response) {
            let entidadPublicaID = $("#cboEmpresa").val(); 
            if (entidadPublicaID) {
                $.ajax({
                    url: `${basePath}EntesReguladores/ObtenerPersonasActivasPorPatronEntesReguladores`,
                    dataType: "json",
                    data: {
                        term: request.term, 
                        entidadPublicaID: entidadPublicaID 
                    },
                    success: function (json) {
                        var results = $.map(json.data, function (item) {
                            return {
                        
                                label: item.Nombres + " " + item.Apellidos + " - " + item.Dni,
                                value: item.Nombres + " " + item.Apellidos, 
                                fullData: item 
                            };
                        });

                        if (results.length === 0) {
                            toastr.warning("No se encontraron personas con esos datos", "Advertencia");
                        }

                        response(results);
                    },
                    error: function () {
                        response([]); 
                    }
                });
            } else {
                toastr.warning("Seleccione una entidad pública", "Advertencia");
                response([]); 
            }
        },
        minLength: 1,  
        select: function (event, ui) {

            let existe = arrayEmpleadosSeleccionados.some(item => item.Dni === ui.item.fullData.Dni);

            if (existe) {
                toastr.warning("La persona ya ha sido seleccionada", "Advertencia");
                return; 
            } 

            arrayEmpleadosSeleccionados.push({
                PersonaEntidadPublicaID: ui.item.fullData.PersonaEntidadPublicaID,
                Nombres: ui.item.fullData.Nombres,
                Apellidos: ui.item.fullData.Apellidos,
                Dni: ui.item.fullData.Dni,
                CargoEntidadNombre: ui.item.fullData.CargoEntidadNombre,
                EntidadPublicaNombre: ui.item.fullData.EntidadPublicaNombre,
                FechaRegistro: ui.item.fullData.FechaRegistro,
                TipoDOI: ui.item.fullData.TipoDOI
            });
            renderTablaEmpleadosSeleccionados(); 
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
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
            .append("<div>" + item.label + "<br><small>" + item.fullData.EntidadPublicaNombre + " - " + item.fullData.CargoEntidadNombre + "</small></div>")
            .appendTo(ul);
    };
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
                url: basePath + "EntesReguladores/ImportarExcelEntesReguladores",
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
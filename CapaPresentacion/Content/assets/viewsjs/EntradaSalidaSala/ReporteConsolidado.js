let objetodatatable
let objetodatatable2
let arraySalas = []
let IdAperturaCierreSala = 0
let contadorInterval = null;
CboSeleccionado = 0
CboSeleccionado1 = 0
CboSeleccionado2 = 0
btnFinalizarCierreSala = 0

$(document).ready(function () {
    ObtenerListaSalas()
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
    })

    $('#Observaciones').on('input', function () {
        var maxLengthObservaciones = $(this).attr('maxlength');
        var currentLengthObservaciones = $(this).val().length;
        $('#charCountObservaciones').text(currentLengthObservaciones + '/' + maxLengthObservaciones);
    })

    $('#ObservacionesCierre').on('input', function () {
        var maxLengthObservacionesCierre = $(this).attr('maxlength');
        var currentLengthObservacionesCierre = $(this).val().length;
        $('#charCountObservacionesCierre').text(currentLengthObservacionesCierre + '/' + maxLengthObservacionesCierre);
    })

    $('#myTab a[href="#bienesMateriales"]').tab('show');

    $('#myTab a').on('shown.bs.tab', function (e) {
        switch ($(e.target).attr('href')) {

            case "#bienesMateriales":
                buscarBienesMaterialesConsolidado();
                break;

            case "#entesReguladores":
                buscarEnteReguladorConsolidado();
                break;

            case "#ingresoSalidaGU":
                buscarIngresoSalidaGUConsolidado();
                break;

            case "#recaudacionesPersonalParticipante":
                buscarRecaudacionPersonalConsolidado();
                break;

            case "#aperturaCierreSala":
                buscarAperturaCierreSalaConsolidado();
                break;

            case "#accionesIncidentes":
                buscarAccionCajaTemporizadaConsolidado();
                break;

            case "#logOcurrencias":
                buscarLogOcurrenciaConsolidado();
                break

            case "#recojoRemesas":
                buscarRecojoRemesaConsolidado();
                break

            default:
                toastr.error("No se encontró una acción para esta pestaña.");
                break;
        }
    });
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

    // Determinar la pestaña activa
    let activeTabHref = $('#myTab .active a').attr('href'); // Obtener el href (ID) de la pestaña activa

    if (!activeTabHref) {
        toastr.warning("Por favor, seleccione una pestaña."); // Mostrar un mensaje de error si no hay pestaña activa
        return false; // Detener la ejecución
    }

    // Función para cargar contenido basado en la pestaña activa
    const loadTabContent = {
        "#bienesMateriales": buscarBienesMaterialesConsolidado,
        "#entesReguladores": buscarEnteReguladorConsolidado,
        "#ingresoSalidaGU": buscarIngresoSalidaGUConsolidado,
        "#recaudacionesPersonalParticipante": buscarRecaudacionPersonalConsolidado,
        "#aperturaCierreSala": buscarAperturaCierreSalaConsolidado,
        "#accionesIncidentes": buscarAccionCajaTemporizadaConsolidado,
        "#logOcurrencias": buscarLogOcurrenciaConsolidado,
        "#recojoRemesas": buscarRecojoRemesaConsolidado
    };

    // Verifica si hay una función definida para la pestaña activa
    if (loadTabContent[activeTabHref]) {
        loadTabContent[activeTabHref](); // Llama a la función correspondiente
    } else {
        toastr.error("No se encontró una acción para esta pestaña.");
    }
});

// NUEVO ------------------------------------------------------------
$(document).on('click', "#btnNuevo", function (e) {
    e.preventDefault()
    limpiarValidadoraperturaCierreSalaForm()
    IdAperturaCierreSala = 0;
    $('#btnGuardarAperturaCierreSala').show();

    $('#textAperturaCierreSala').text('Nuevo')
    $('#IdAperturaCierreSala').val(0);
    $('#cboSalaAperturaCierreSala').val(null)
    $('#DateHora').val('')
    $('#DateFecha').val(moment().format('DD/MM/YYYY'));
    $("#DateFecha").datetimepicker({
        format: 'DD/MM/YYYY',
        defaultDate: moment(),
        pickTime: true
    });
    $('#cboJefeSala').val(null)
    $('#Observaciones').val('')
    $('#contadorTiempoRestante').text('');
    $('#contadorTiempoRestante').html("");
    if (contadorInterval !== null) {
        clearInterval(contadorInterval);
        contadorInterval = null;
    }
    /* $(btnFinalizarCierreSala).remove();*/

    renderSelectSalasModalEnteRegulador()
    obtenerListaEmpleadosPorEstado()
    obtenerListaEmpleadosPorEstado2()

    $('#full-modal_aperturacierresala').modal('show')

    var maxLength = $('#Observaciones').attr('maxlength');
    $('#charCountObservaciones').text(0 + '/' + 300);
});


//$(document).on('click', '#btnExcel', function (e) {
//    e.preventDefault()

//    if ($("#cboSala").length == 0 || $("#cboSala").val() == null) {
//        toastr.error("Seleccione una sala.")
//        return false
//    }
//    if ($("#fechaInicio").val() === "") {
//        toastr.error("Ingrese una fecha de Inicio.")
//        return false
//    }
//    if ($("#fechaFin").val() === "") {
//        toastr.error("Ingrese una fecha Fin.")
//        return false
//    }

//    let listasala = $("#cboSala").val();
//    let fechaini = $("#fechaInicio").val();
//    let fechafin = $("#fechaFin").val();
//    let dataForm = {
//        codsala: listasala,
//        fechaini, fechafin
//    }
//    $.ajax({
//        type: "POST",
//        cache: false,
//        url: basePath + "AperturaCierreSala/ReporteAperturaCierreSalaXSalaJsonExcel",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        data: JSON.stringify(dataForm),
//        beforeSend: function (xhr) {
//            $.LoadingOverlay("show");
//        },

//        success: function (response) {
//            if (response.respuesta) {
//                var data = response.data;
//                var file = response.excelName;
//                let a = document.createElement('a');
//                a.target = '_self';
//                a.href = "data:application/vnd.ms-excel;base64, " + data;
//                a.download = file;
//                a.click();
//            }
//            else {
//                toastr.error(response.mensaje, "Mensaje Servidor");
//            }
//        },
//        complete: function (resul) {
//            $.LoadingOverlay("hide");
//        }
//    });
//})


$(document).on("click", ".btnEditarbien", function () {

    IdAperturaCierreSala = $(this).data("id");
    let rowData = objetodatatable
        .rows()
        .data()
        .toArray()
        .find(row => row.IdAperturaCierreSala === IdAperturaCierreSala);

    if (rowData) {
        $('#textAperturaCierreSala').text('Editar');
        $('#btnGuardarAperturaCierreSala').show();

        renderSelectSalasModalEnteRegulador(rowData.CodSala)
        obtenerListaEmpleadosPorEstado(rowData.IdPrevencionistaApertura)
        obtenerListaEmpleadosPorEstado2(rowData.IdJefeSalaApertura)

        $('#cboSalaAperturaCierreSala').val(rowData.CodSala);
        $('#cboPrevencionista').val(rowData.PrevencionistaApertura);
        $('#cboJefeSala').val(rowData.JefeSalaApertura);

        $('#Observaciones').val(rowData.ObservacionesApertura);
        var currentLengthObservaciones = $('#Observaciones').val().length;
        var maxLengthObservaciones = $('#Observaciones').attr('maxlength');
        $('#charCountObservaciones').text(currentLengthObservaciones + '/' + maxLengthObservaciones);
        $('#contadorTiempoRestante').text('')

        const fecha = new Date(parseInt(rowData.Fecha.replace("/Date(", "").replace(")/", "")));
        const horaApertura = new Date(fecha.getTime() + rowData.HoraApertura.TotalMilliseconds);
        $('#DateFecha').val((moment(rowData.Fecha).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.Fecha).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.Fecha).format('DD/MM/YYYY'));

        $('#DateHora').val(horaApertura.toLocaleTimeString());
        const fechaLimiteEditar = new Date(parseInt(rowData.FechaLimiteEditarAperturaCierreSala.replace("/Date(", "").replace(")/", "")));

        if (rowData.HoraCierre && rowData.HoraCierre.TotalMilliseconds > 0) {
            const fechaCierre = new Date(parseInt(rowData.Fecha.replace("/Date(", "").replace(")/", "")));
            const horaCierre = new Date(fechaCierre.getTime() + rowData.HoraCierre.TotalMilliseconds);
            horaCierre.setDate(horaCierre.getDate() + 1);

            const fechaActual = new Date();
            const diffMilisegundos = fechaLimiteEditar - fechaActual;
            if (diffMilisegundos <= 0) {
                $('#btnGuardarAperturaCierreSala').hide();
                $('#contadorTiempoRestante').text('Este registro ya no se puede editar.').removeClass('label-info').addClass('label-danger');
            } else {
                actualizarContador(fechaLimiteEditar, '#contadorTiempoRestante');
            }
        } else {
            $('#contadorTiempoRestante').text('').removeClass('label-info').addClass('label-warning');
        }
        $('.modal').modal('hide');
        $('#full-modal_aperturacierresala').modal('show');
    } else {
        toastr.error("No se encontraron datos para este registro.", "Error");
    }
});
function actualizarContador(fechaLimite, contadorId) {
    if (contadorInterval !== null) {
        clearInterval(contadorInterval);
    }
    contadorInterval = setInterval(function () {
        const fechaActual = new Date();
        const fechaLimiteProxima = new Date(fechaLimite);

        const diffMilisegundos = fechaLimiteProxima - fechaActual;
        if (diffMilisegundos <= 0) {
            $(contadorId).text('Este registro ya no se puede editar.')
                .removeClass('label-info')
                .addClass('label-danger');
            clearInterval(contadorInterval);
            contadorInterval = null;
        } else {
            const diffHoras = Math.floor(diffMilisegundos / (1000 * 60 * 60));
            const diffMinutos = Math.floor((diffMilisegundos % (1000 * 60 * 60)) / (1000 * 60));
            const diffSegundos = Math.floor((diffMilisegundos % (1000 * 60)) / 1000);

            const horas = diffHoras.toString().padStart(2, '0');
            const minutos = diffMinutos.toString().padStart(2, '0');
            const segundos = diffSegundos.toString().padStart(2, '0');

            $(contadorId).text(`Tiempo restante para editar: ${horas}:${minutos}:${segundos}`)
                .removeClass('label-info')
                .addClass('label-danger');
        }
    }, 1000);
}

$(document).on('click', '#btnGuardarAperturaCierreSala', function (e) {
    e.preventDefault()

    $("#aperturaCierreSalaForm")
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
                Fecha: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                HoraApertura: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                IdPrevencionistaApertura: {
                    validators: {
                        notEmpty: {
                            message: ' '
                        }
                    }
                },
                IdJefeSalaApertura: {
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


    $("#aperturaCierreSalaForm").data('bootstrapValidator').resetForm();
    var validarRegistro = $("#aperturaCierreSalaForm").data('bootstrapValidator').validate();

    if (validarRegistro.isValid()) {
        let url = basePath

        if (IdAperturaCierreSala === 0) {
            url += "AperturaCierreSala/GuardarAperturaCierreSala"
        } else {
            url += "AperturaCierreSala/EditarAperturaCierreSala"
        }

        let dataForm = new FormData(document.getElementById("aperturaCierreSalaForm"))

        dataForm.delete('IdAperturaCierreSala');
        dataForm.append('IdAperturaCierreSala', IdAperturaCierreSala);
        dataForm.append('Sala', $('#cboSalaAperturaCierreSala option:selected').text())
        dataForm.append('PrevencionistaApertura', $('#cboPrevencionista option:selected').text())
        dataForm.append('JefeSalaApertura', $('#cboJefeSala option:selected').text())

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
                    limpiarValidadoraperturaCierreSalaForm()
                    toastr.success(response.mensaje, "Mensaje Servidor")
                    $('#full-modal_aperturacierresala').modal('hide')
                    buscarAperturaCierreSala()
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


$(document).on("click", ".btnFinalizarCierreSala", function () {
    IdAperturaCierreSala = $(this).data("id");
    let rowData = objetodatatable
        .rows()
        .data()
        .toArray()
        .find(row => row.IdAperturaCierreSala === IdAperturaCierreSala);

    if (rowData) {
        $('#textCierreSala').text('Finalizar');
        renderSelectSalasModalEnteRegulador(rowData.CodSala)
        obtenerListaEmpleadosPorEstado(rowData.IdPrevencionistaCierre)
        obtenerListaEmpleadosPorEstado2(rowData.IdJefeSalaCierre)

        $('#ObservacionesCierre').val(rowData.ObservacionesCierre);
        var currentLengthObservacionesCierre = $('#ObservacionesCierre').val().length;
        var maxLengthObservacionesCierre = $('#ObservacionesCierre').attr('maxlength');
        $('#charCountObservacionesCierre').text(currentLengthObservacionesCierre + '/' + maxLengthObservacionesCierre);
        $('#contadorTiempoRestanteCierre').text('')

        $('#btnGuardarCierreSala').show();

        $('#cboSalaCierreSala').val(rowData.CodSala);
        $('#cboPrevencionistaCierre').val(rowData.PrevencionistaCierre);
        $('#cboJefeSalaCierre').val(rowData.JefeSalaCierre);
        const fechaLimiteEditar = new Date(parseInt(rowData.FechaLimiteEditarAperturaCierreSala.replace("/Date(", "").replace(")/", "")));

        if (rowData.HoraCierre && rowData.HoraCierre.TotalMilliseconds > 0) {
            const fechaCierre = new Date(parseInt(rowData.Fecha.replace("/Date(", "").replace(")/", "")));
            const horaCierre = new Date(fechaCierre.getTime() + rowData.HoraCierre.TotalMilliseconds);
            $('#DateFechaCierre').val((moment(rowData.Fecha).format('DD/MM/YYYY') == "01/01/1753" || moment(rowData.Fecha).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(rowData.Fecha).format('DD/MM/YYYY hh:mm A'));

            $('#DateHoraCierre').val(horaCierre.toLocaleTimeString());
            const fechaActual = new Date();
            const diffMilisegundos = fechaLimiteEditar - fechaActual;

            if (diffMilisegundos <= 0) {
                $('#btnGuardarCierreSala').hide();
                $('#contadorTiempoRestanteCierre').text('Este registro ya no se puede editar.').removeClass('label-info').addClass('label-danger');
            } else {
                actualizarContador(fechaLimiteEditar, '#contadorTiempoRestanteCierre');
            }
        } else {
            $('#contadorTiempoRestanteCierre').text('').removeClass('label-info').addClass('label-warning');
        }
        $('.modal').modal('hide');
        $('#full-modal_cierresala').modal('show');

    } else {
        toastr.error("No se encontraron datos para este registro.", "Error");
    }
});
$('#full-modal_cierresala').on('hidden.bs.modal', function () {
    clearInterval(contadorInterval);
    contadorInterval = null;

})
$('#full-modal_aperturacierresala').on('hidden.bs.modal', function () {
    clearInterval(contadorInterval);
    contadorInterval = null;
})

$("#full-modal_cierresala").on("shown.bs.modal", function () {
    $("#DateFechaCierre").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        pickTime: true
    });
    $("#DateHoraCierre").datetimepicker({
        pickDate: false,
        format: 'HH:mm',
        defaultDate: dateNow,
        pickTime: true
    })
    limpiarValidadorCierreSalaForm()

})

$("#btnGuardarCierreSala").on("click", function () {
    if (IdAperturaCierreSala) {
        btnFinalizarAperturaCierreSala(IdAperturaCierreSala);
    }
});


$("#cierreSalaForm")
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
            HoraCierre: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            IdPrevencionistaCierre: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            IdJefeSalaCierre: {
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

function Finalizar_CierreSala() {
    let dataForm = new FormData(document.getElementById("cierreSalaForm"))
    limpiarValidadorCierreSalaForm()

    dataForm.delete('IdAperturaCierreSala');
    dataForm.append('IdAperturaCierreSala', IdAperturaCierreSala);


    dataForm.append('Sala', $('#cboSalaCierreSala option:selected').text());
    console.log($('#cboSalaCierreSala').val());
    //dataForm.append('CodSala', $('#cboSalaCierreSala').val());
    //console.log($('#cboSalaCierreSala').val());
    dataForm.append('PrevencionistaCierre', $('#cboPrevencionistaCierre option:selected').text());
    dataForm.append('JefeSalaCierre', $('#cboJefeSalaCierre option:selected').text());
    dataForm.append('ObservacionesCierre', $('#ObservacionesCierre').val());
    dataForm.append('HoraCierre', $('#DateHoraCierre').val());

    $.ajax({
        url: basePath + "AperturaCierreSala/FinalizarAperturaCierreSala",
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
                toastr.success(response.mensaje, "Mensaje Servidor")
                $("#full-modal_cierresala").modal("hide");
                buscarAperturaCierreSala()
            } else {
                toastr.error(response.mensaje, "Mensaje Servidor")
            }
            limpiarValidadorCierreSalaForm()
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            toastr.error("Ocurrió un error al guardar el registro.", "Error");
        }
    });
}

function btnFinalizarAperturaCierreSala(id) {

    $("#cierreSalaForm").data('bootstrapValidator').resetForm();
    var validarRegistro = $("#cierreSalaForm").data('bootstrapValidator').validate();

    if (validarRegistro.isValid()) {

        let rowData = objetodatatable
            .rows()
            .data()
            .toArray()
            .find(row => row.IdAperturaCierreSala === IdAperturaCierreSala);
        if (rowData) {
            if (rowData.HoraCierre && rowData.HoraCierre.TotalMilliseconds > 0) {
                Finalizar_CierreSala()
            } else {
                $.confirm({
                    title: '¿Estás seguro de finalizar el Cierre de Sala?',
                    content: 'Pasado 24 horas de la hora de Cierre, no se podrá editar este registro.',
                    confirmButton: 'Ok',
                    cancelButton: 'Cerrar',
                    confirmButtonClass: 'btn-success',
                    cancelButtonClass: 'btn-danger',
                    confirm: () => {
                        Finalizar_CierreSala()
                    }
                });
            }

        } else {
            toastr.error("No se pudo seleccionar al registro.", "Error");
        }
    }
}



$("#full-modal_aperturacierresala").on("shown.bs.modal", function () {


    $("#DateHora").datetimepicker({
        pickDate: false,
        format: 'HH:mm',
        defaultDate: dateNow,
        pickTime: true
    })

})


//function obtenerListaEmpleadosPorEstado(value) {
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
//            $("#cboPrevencionista").html('')
//            if (result.data) {
//                $("#cboPrevencionista").html(result.data.map(item => `<option value="${item.Nombre} ${item.ApellidoPaterno}">${item.Nombre} ${item.ApellidoPaterno}</option>`).join(""));
//                if (value) {
//                    $('#cboPrevencionista').val(value);
//                } else {
//                    $("#cboPrevencionista").val(null).trigger("change");
//                }
//                $("#cboPrevencionista").select2({
//                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_aperturacierresala')
//                });




//                $("#cboPrevencionistaCierre").html(result.data.map(item =>`<option value="${item.Nombre} ${item.ApellidoPaterno}">${item.Nombre} ${item.ApellidoPaterno}</option>`).join(""));
//                if (value) {
//                    $('#cboPrevencionistaCierre').val(value);
//                } else {
//                    $("#cboPrevencionistaCierre").val(null).trigger("change");
//                }
//                $("#cboPrevencionistaCierre").select2({
//                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_cierresala')
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
        url: basePath + "AperturaCierreSala/ListarEmpleadoBUKcargoPrevencionista",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {

            $("#cboPrevencionista").html('')
            $("#cboPrevencionistaCierre").html('')
            if (result.data) {
                $("#cboPrevencionista").html(result.data.map(item => `<option value="${item.IdBuk}">${item.NombreCompleto} - ${item.Cargo}</option>`).join(""));
                if (value) {
                    $('#cboPrevencionista').val(value);
                } else {
                    $("#cboPrevencionista").val(null).trigger("change");
                }
                $("#cboPrevencionista").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_aperturacierresala')
                });




                $("#cboPrevencionistaCierre").html(result.data.map(item => `<option value="${item.IdBuk}">${item.NombreCompleto} - ${item.Cargo}</option>`).join(""));
                if (value) {
                    $('#cboPrevencionistaCierre').val(value);
                } else {
                    $("#cboPrevencionistaCierre").val(null).trigger("change");
                }
                $("#cboPrevencionistaCierre").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_cierresala')
                });
            }
            limpiarValidadorCierreSalaForm()
            limpiarValidadoraperturaCierreSalaForm()

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

//function obtenerListaEmpleadosPorEstado2(value) {
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
//            $("#cboJefeSala").html('')
//            if (result.data) {
//                $("#cboJefeSala").html(result.data.map(item =>`<option value="${item.Nombre} ${item.ApellidoPaterno}">${item.Nombre} ${item.ApellidoPaterno}</option>`).join(""));
//                if (value) {
//                    $('#cboJefeSala').val(value);
//                } else {
//                    $("#cboJefeSala").val(null).trigger("change");
//                }
//                $("#cboJefeSala").select2({
//                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_aperturacierresala')
//                });



//                $("#cboJefeSalaCierre").html(result.data.map(item =>`<option value="${item.Nombre} ${item.ApellidoPaterno}">${item.Nombre} ${item.ApellidoPaterno}</option>`).join(""));
//                if (value) {
//                    $('#cboJefeSalaCierre').val(value);
//                } else {
//                    $("#cboJefeSalaCierre").val(null).trigger("change");
//                }
//                $("#cboJefeSalaCierre").select2({
//                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_cierresala')
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

function obtenerListaEmpleadosPorEstado2(value) {
    $.ajax({
        type: "POST",
        url: basePath + "AperturaCierreSala/ListarEmpleadoBUKcargoJefeDeSala",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {

            $("#cboJefeSala").html('')
            if (result.data) {
                $("#cboJefeSala").html(result.data.map(item => `<option value="${item.IdBuk}">${item.NombreCompleto} ${item.Cargo}</option>`).join(""));
                if (value) {
                    $('#cboJefeSala').val(value);
                } else {
                    $("#cboJefeSala").val(null).trigger("change");
                }
                $("#cboJefeSala").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_aperturacierresala')
                });


                $("#cboJefeSalaCierre").html(result.data.map(item => `<option value="${item.IdBuk}">${item.NombreCompleto} ${item.Cargo}</option>`).join(""));
                if (value) {
                    $('#cboJefeSalaCierre').val(value);
                } else {
                    $("#cboJefeSalaCierre").val(null).trigger("change");
                }
                $("#cboJefeSalaCierre").select2({
                    placeholder: "--Seleccione--", dropdownParent: $('#full-modal_cierresala')
                });
                limpiarValidadorCierreSalaForm()
                limpiarValidadoraperturaCierreSalaForm()

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
    $("#cboSalaAperturaCierreSala").html('')
    $("#cboSalaCierreSala").html('')

    if (arraySalas) {
        $("#cboSalaAperturaCierreSala").html(arraySalas.map(item => `<option value="${item.CodSala}">${item.Nombre}</option>`).join(""))
        if (value) {
            $('#cboSalaAperturaCierreSala').val(value);
        } else {
            $("#cboSalaAperturaCierreSala").val(null).trigger("change");
        }
        $("#cboSalaAperturaCierreSala").select2({
            placeholder: "--Seleccione--", dropdownParent: $('#full-modal_aperturacierresala')
        });

        $("#cboSalaCierreSala").html(arraySalas.map(item => `<option value="${item.CodSala}">${item.Nombre}</option>`).join(""));
        if (value) {
            $('#cboSalaCierreSala').val(value);
        } else {
            $("#cboSalaCierreSala").val(null).trigger("change");
        }
        $("#cboSalaCierreSala").select2({
            placeholder: "--Seleccione--",
            dropdownParent: $('#full-modal_cierresala')
        });
        limpiarValidadorCierreSalaForm()
        limpiarValidadoraperturaCierreSalaForm()
    }

}

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
                $("#cboSala").val(-1).trigger("change")
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

function buscarAperturaCierreSalaConsolidado() {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();
    let addtabla = $("#contenedor_tabla_aperturaCierreSala");

    $.ajax({
        type: "POST",
        url: basePath + "AperturaCierreSala/AperturaCierreSalaListarJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            let datos = response.data || [];
            let tableData = []; // Aquí almacenaremos las filas generadas

            // Procesar los datos para agregar filas de apertura y cierre por separado
            datos.forEach(function (data) {
                // Verificamos si tiene hora de apertura
                if (data.HoraApertura) {
                    let apertura = {
                        IdAperturaCierreSala: data.IdAperturaCierreSala,
                        Sala: data.Sala,
                        CodSala: data.CodSala,
                        Descripcion: `Apertura por ${data.PrevencionistaApertura}`,
                        Prevencionista: data.PrevencionistaApertura,
                        FechaHora: `${moment(data.Fecha).format("DD/MM/YYYY")} ${moment(data.HoraApertura).format("hh:mm A")}`,
                        Estado: data.HoraCierre && data.PrevencionistaCierre
                            ? `<span class="label btn-success"><i class="glyphicon glyphicon-ok"></i> Completado</span>`
                            : `<span class="label btn-warning"><i class="glyphicon glyphicon-remove"></i> Cierre pendiente</span>`
                    };
                    tableData.push(apertura); // Agregar fila de apertura
                }

                // Verificamos si tiene hora de cierre, si no, no creamos fila de cierre
                if (data.HoraCierre && data.PrevencionistaCierre) {
                    let cierre = {
                        IdAperturaCierreSala: data.IdAperturaCierreSala,
                        Sala: data.Sala,
                        CodSala: data.CodSala,
                        Descripcion: `Cierre por ${data.PrevencionistaCierre}`,
                        Prevencionista: data.PrevencionistaCierre,
                        FechaHora: `${moment(data.Fecha).format("DD/MM/YYYY")} ${moment(data.HoraCierre).format("hh:mm A")}`,
                        Estado: `<span class="label btn-success"><i class="glyphicon glyphicon-ok"></i> Completado</span>`
                    };
                    tableData.push(cierre); // Agregar fila de cierre
                }
            });

            $(addtabla).empty();
            $(addtabla).append('<table id="tableResultado_aperturaCierreSala" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');

            $("#tableResultado_aperturaCierreSala").DataTable({
                destroy: true,
                paging: true,
                data: tableData,
                aaSorting: [[0, 'desc']],
                columns: [
                    { data: "IdAperturaCierreSala", title: "ID" },
                    { data: "Sala", title: "Sala" },
                    { data: "Descripcion", title: "Descripción" },
                    { data: "FechaHora", title: "Fecha y Hora" },
                    { data: "Estado", title: "Estado" },
                    {
                        data: "IdAperturaCierreSala",
                        title: "Accion",
                        render: function (data, type, row) {
                            return `<button type="button" class="btn btn-xs btn-info verDetalle" data-id="${data}" data-codsala="${row.CodSala}">
                                    <i class="glyphicon glyphicon-eye-open"></i> Ver Detalle</button>`;
                        }
                    }

                ]
            });
            $("#tableResultado_aperturaCierreSala").on("click", ".verDetalle", function () {
                let IdAperturaCierreSala = $(this).data("id");
                let CodigoSala = $(this).data("codsala");
                console.log(CodigoSala);
                fechaini = $("#fechaInicio").val();
                fechafin = $("#fechaFin").val();

                let urlDetalle = `${basePath}AperturaCierreSala/AperturaCierreSalaVista?id=${IdAperturaCierreSala}&codsala=${CodigoSala}&fechaInicio=${fechaini}&fechaFin=${fechafin}`;
                window.open(urlDetalle, '_blank');
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

function buscarRecaudacionPersonalConsolidado() {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();
    let addtabla = $("#contenedor_tabla_recaudaciones");

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
            $(addtabla).append('<table id="tableResultado_Recaudacion" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');

            objetodatatable = $("#tableResultado_Recaudacion").DataTable({
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
                        title: "Fecha",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY');
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
                    },
                    {
                        data: "RecaudacionFin",
                        title: "Estado",
                        render: function (data) {
                            if (data && moment(data).format('DD/MM/YYYY') !== "01/01/1753" && moment(data).format('DD/MM/YYYY') !== "31/12/1752") {
                                return `<span class="label btn-success">Completado</span>`;
                            } else {
                                return `<span class="label btn-warning">En curso</span>`;
                            }
                        }
                    },
                    {
                        data: "IdRecaudacionPersonal",
                        title: "Accion",
                        render: function (data, type, row) {
                            return `<button type="button" class="btn btn-xs btn-info verDetalle" data-id="${data}" data-codsala="${row.CodSala}">
                                     <i class="glyphicon glyphicon-eye-open"></i> Ver Detalle</button>`
                        }
                    }
                ],
            });
            $("#tableResultado_Recaudacion").on("click", ".verDetalle", function () {
                let IdRecaudacionPersonal = $(this).data("id");
                let CodigoSala = $(this).data("codsala");
                fechaini = $("#fechaInicio").val();
                fechafin = $("#fechaFin").val();

                let urlDetalle = `${basePath}RecaudacionesPersonalParticipante/RecaudacionesPersonalParticipanteVista?id=${IdRecaudacionPersonal}&codsala=${CodigoSala}&fechaInicio=${fechaini}&fechaFin=${fechafin}`;
                window.open(urlDetalle, '_blank');
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
function buscarAccionCajaTemporizadaConsolidado() {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();
    let addtabla = $("#contenedor_tabla_acciones");

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
            $(addtabla).append('<table id="tableResultadoAcciones" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');

            $("#tableResultadoAcciones").DataTable({
                destroy: true,
                paging: true,
                data: datos,
                aaSorting: [[0, 'desc']],
                columns: [
                    { data: "IdAccionCajaTemporizada", title: "ID" },
                    { data: "NombreSala", title: "Sala" },
                    {
                        data: "Descripcion", title: "Descripción",
                        render: function (data, type, row) {
                            return `Autorizado por ${row.NombreAutoriza}`
                        }
                    },
                    {
                        data: "Fecha", title: "Fecha y Hora",
                        render: function (data) {
                            return moment(data).format("DD/MM/YYYY HH:mm A");
                        }
                    },
                    {
                        data: "FechaSolucion",
                        title: "Estado",
                        render: function (data) {
                            if (data && moment(data).format('DD/MM/YYYY') !== "01/01/1753" && moment(data).format('DD/MM/YYYY') !== "31/12/1752") {
                                return `<span class="label btn-success">Completado</span>`;
                            } else {
                                return `<span class="label btn-warning">En curso</span>`;
                            }
                        }
                    },
                    {
                        data: "IdAccionCajaTemporizada",
                        title: "Accion",
                        render: function (data, type, row) {
                            return `<button type="button" class="btn btn-xs btn-info verDetalle" data-id="${data}" data-codsala="${row.CodSala}">
                                     <i class="glyphicon glyphicon-eye-open"></i> Ver Detalle</button>`
                        }
                    }
                ],
            });
            $("#tableResultadoAcciones").on("click", ".verDetalle", function () {
                let IdAccionCajaTemporizada = $(this).data("id");
                let CodigoSala = $(this).data("codsala");
                fechaini = $("#fechaInicio").val();
                fechafin = $("#fechaFin").val();

                let urlDetalle = `${basePath}AccionesIncidentesCajasTemporizadas/AccionesIncidentesCajasTemporizadasVista?id=${IdAccionCajaTemporizada}&codsala=${CodigoSala}&fechaInicio=${fechaini}&fechaFin=${fechafin}`;
                window.open(urlDetalle, '_blank');
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

function buscarEnteReguladorConsolidado() {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();
    let addtabla = $("#contenedor_tabla_entesReguladores");

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
            $(addtabla).append('<table id="tableResultado_entesReguladores" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');

            $("#tableResultado_entesReguladores").DataTable({
                destroy: true,
                paging: true,
                data: datos,
                aaSorting: [[0, 'desc']],
                columns: [
                    { data: "IdEnteRegulador", title: "ID" },
                    { data: "NombreSala", title: "Sala" },
                    {
                        data: "NombreMotivo",
                        title: "Motivo I/S",
                        render: function (data) {
                            return data ? (data.length > 27 ? `${data.substring(0, 27)}...` : data) : "";
                        }
                    },
                    { data: "NombreAutoriza", title: "Autoriza" },
                    {
                        data: "FechaIngreso",
                        title: "Fecha y Hora",
                        render: function (data) {
                            return moment(data).format("DD/MM/YYYY HH:mm A");
                        }
                    },
                    {
                        data: "FechaSalida",
                        title: "Estado",
                        render: function (data) {
                            if (data && moment(data).format('DD/MM/YYYY') !== "01/01/1753" && moment(data).format('DD/MM/YYYY') !== "31/12/1752") {
                                return `<span class="label btn-success">Completado</span>`;
                            } else {
                                return `<span class="label btn-warning">En curso</span>`;
                            }
                        }
                    },
                    {
                        data: "IdEnteRegulador",
                        title: "Accion",
                        render: function (data, type, row) {
                            return `<button type="button" class="btn btn-xs btn-info verDetalle" data-id="${data}" data-codsala="${row.CodSala}">
                                    <i class="glyphicon glyphicon-eye-open"></i> Ver Detalle
                                  </button>`
                        }
                    }
                ],
            });
            $("#tableResultado_entesReguladores").on("click", ".verDetalle", function () {
            let idEnteRegulador = $(this).data("id");
                let CodigoSala = $(this).data("codsala");
                fechaini = $("#fechaInicio").val();
                fechafin = $("#fechaFin").val();

                let urlDetalle = `${basePath}EntesReguladores/EntesReguladoresVista?id=${idEnteRegulador}&codsala=${CodigoSala}&fechaInicio=${fechaini}&fechaFin=${fechafin}`;
                window.open(urlDetalle, '_blank');
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

function buscarBienesMaterialesConsolidado() {
    let listasala = $("#cboSala").val();
    let idtipobienmaterial = -1;
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();
    let addtabla = $("#contenedor_tabla_bienesMateriales");

    $.ajax({
        type: "POST",
        url: basePath + "BienesMateriales/ListarBienesMaterialesxSalaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, idtipobienmaterial, fechaini, fechafin }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            let datos = response.data || [];
            $(addtabla).empty();
            $(addtabla).append('<table id="tableResultado_bienesMateriales" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');

            $("#tableResultado_bienesMateriales").DataTable({
                destroy: true,
                paging: true,
                data: datos,
                aaSorting: [[0, 'desc']],
                columns: [
                    { data: "IdBienMaterial", title: "ID" },
                    { data: "NombreSala", title: "Sala" },
                    {
                        data: "TipoBienMaterial",
                        title: "Tipo",
                        render: function (data) {
                            let estado = "N/A";
                            let css = "btn-secondary";
                            if (data === 1) { estado = "Interno"; css = "btn-success"; }
                            else if (data === 2) { estado = "Externo"; css = "btn-primary"; }
                            return `<span class="label ${css}">${estado}</span>`;
                        }
                    },
                    {
                        data: "NombreMotivo",
                        title: "Motivo I/S",
                        render: function (data) {
                            return data ? (data.length > 27 ? `${data.substring(0, 27)}...` : data) : "";
                        }
                    },
                    {
                        data: "FechaRegistro",
                        title: "Fecha y Hora",
                        render: function (data) {
                            return moment(data).format("DD/MM/YYYY HH:mm A");
                        }
                    },
                    {
                        data: "FechaSalida",
                        title: "Estado",
                        render: function (data) {
                            if (data && moment(data).format('DD/MM/YYYY') !== "01/01/1753" && moment(data).format('DD/MM/YYYY') !== "31/12/1752") {
                                return `<span class="label btn-success">Completado</span>`;
                            } else {
                                return `<span class="label btn-warning">En curso</span>`;
                            }
                        }
                    },
                    {
                        data: "IdBienMaterial",
                        title: "Accion",
                        render: function (data, type, row) {
                            return `<button type="button" class="btn btn-xs btn-info verDetalle" data-id="${data}" data-codsala="${row.CodSala}">
                                    <i class="glyphicon glyphicon-eye-open"></i> Ver Detalle
                                  </button>`
                        }
                    }
                ]
            });

            $("#tableResultado_bienesMateriales").on("click", ".verDetalle", function () {
                let IdBienMaterial = $(this).data("id");
                let CodigoSala = $(this).data("codsala");
                fechaini = $("#fechaInicio").val();
                fechafin = $("#fechaFin").val();

                let urlDetalle = `${basePath}BienesMateriales/BienesMaterialesVista?id=${IdBienMaterial}&codsala=${CodigoSala}&fechaInicio=${fechaini}&fechaFin=${fechafin}`;
                window.open(urlDetalle, '_blank');
            });

        },
        error: function () {
            toastr.error("Error en la solicitud.", "Mensaje Servidor");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}

function buscarIngresoSalidaGUConsolidado() {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();
    let addtabla = $("#contenedor_tabla_ingresoSalidaGU");

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
            $(addtabla).append('<table id="tableResultado_GU" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');

            objetodatatable = $("#tableResultado_GU").DataTable({
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
                        data: "Empleados",
                        title: "Gerente de Unidad",
                        render: function (data) {
                            if (data && data.length > 0) {
                                let empleado = data[0];
                                return `${empleado.Nombre} ${empleado.ApellidoPaterno} ${empleado.ApellidoMaterno}`;
                            } else {
                                return "N/A";
                            }
                        }
                    }, { data: "NombreMotivo", title: "Motivo" },
                    {
                        data: "FechaRegistro",
                        title: "Hora Ingreso",
                        render: function (data) {
                            return (moment(data).format('DD/MM/YYYY') == "01/01/1753" || moment(data).format('DD/MM/YYYY') == "31/12/1752") ? "" : moment(data).format('DD/MM/YYYY hh:mm A');
                        }
                    },
                    {
                        data: "HoraSalida",
                        title: "Estado",
                        render: function (data) {
                            if (data && moment(data).format('DD/MM/YYYY') !== "01/01/1753" && moment(data).format('DD/MM/YYYY') !== "31/12/1752") {
                                return `<span class="label btn-success">Completado</span>`;
                            } else {
                                return `<span class="label btn-warning">En curso</span>`;
                            }
                        }
                    },
                    {
                        data: "IdIngresoSalidaGU",
                        title: "Accion",
                        render: function (data, type, row) {
                            return `<button type="button" class="btn btn-xs btn-info verDetalle" data-id="${data}" data-codsala="${row.CodSala}">
                                    <i class="glyphicon glyphicon-eye-open"></i> Ver Detalle
                                  </button>`
                        }
                    }
                ],
            });
            $("#tableResultado_GU").on("click", ".verDetalle", function () {
                let IdIngresoSalidaGU = $(this).data("id");
                let CodigoSala = $(this).data("codsala");
                fechaini = $("#fechaInicio").val();
                fechafin = $("#fechaFin").val();

                let urlDetalle = `${basePath}IngresosSalidasGU/IngresosSalidasGUVista?id=${IdIngresoSalidaGU}&codsala=${CodigoSala}&fechaInicio=${fechaini}&fechaFin=${fechafin}`;
                window.open(urlDetalle, '_blank');
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

function buscarLogOcurrenciaConsolidado() {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();
    let addtabla = $("#contenedor_tabla_log");

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
            $(addtabla).append('<table id="tableResultado_Log" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');

            objetodatatable = $("#tableResultado_Log").DataTable({
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
                        title: "Fec. Registro",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY HH:mm A');
                        }
                    },
                    { data: "NombreTipologia", title: "Tipologia" },
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
                        data: "FechaSolucion",
                        title: "Estado",
                        render: function (data) {
                            if (data && moment(data).format('DD/MM/YYYY') !== "01/01/1753" && moment(data).format('DD/MM/YYYY') !== "31/12/1752") {
                                return `<span class="label btn-success">Completado</span>`;
                            } else {
                                return `<span class="label btn-warning">En curso</span>`;
                            }
                        }
                    },
                    {
                        data: "IdLogOcurrencia",
                        title: "Accion",
                        render: function (data, type, row) {
                            return `<button type="button" class="btn btn-xs btn-info verDetalle" data-id="${data}" data-codsala="${row.CodSala}">
                                    <i class="glyphicon glyphicon-eye-open"></i> Ver Detalle
                                  </button>`
                        }
                    }
                ],
            });
            $("#tableResultado_Log").on("click", ".verDetalle", function () {
                let IdLogOcurrencia = $(this).data("id");
                let CodigoSala = $(this).data("codsala");
                fechaini = $("#fechaInicio").val();
                fechafin = $("#fechaFin").val();

                let urlDetalle = `${basePath}LogsOcurrencias/LogsOcurrenciasVista?id=${IdLogOcurrencia}&codsala=${CodigoSala}&fechaInicio=${fechaini}&fechaFin=${fechafin}`;
                window.open(urlDetalle, '_blank');
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

function buscarRecojoRemesaConsolidado() {
    let listasala = $("#cboSala").val();
    let fechaini = $("#fechaInicio").val();
    let fechafin = $("#fechaFin").val();
    let addtabla = $("#contenedor_tabla_recojo");

    $.ajax({
        type: "POST",
        url: basePath + "RecojoRemesas/ListarRecojoRemesasxSalaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            let datos = response.data || [];
            $(addtabla).empty();
            $(addtabla).append('<table id="tableResultado_remesas" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');

            objetodatatable = $("#tableResultado_remesas").DataTable({
                destroy: true,
                sort: true,
                scrollCollapse: true,
                scrollX: true,
                paging: true,
                autoWidth: true,
                aaSorting: [[0, 'desc']],

                data: datos,
                columns: [
                    { data: "IdRecojoRemesa", title: "ID" },
                    { data: "Sala", title: "Sala" },
                    {
                        data: "FechaRegistro",
                        title: "Fec. Registro",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    { data: "PlacaRodaje", title: "Placa/Rodaje" },
                    {
                        data: "FechaIngreso",
                        title: "Fec. Ingreso",
                        render: function (data) {
                            return (moment(data).format('DD/MM/YYYY') == "01/01/1753" || moment(data).format('DD/MM/YYYY') == "31/12/1752") ? '<span class="label btn-warning">No definido</span>' : moment(data).format('DD/MM/YYYY hh:mm A');
                        }
                    },
                    {
                        data: "FechaSalida",
                        title: "Estado",
                        render: function (data) {
                            if (data && moment(data).format('DD/MM/YYYY') !== "01/01/1753" && moment(data).format('DD/MM/YYYY') !== "31/12/1752") {
                                return `<span class="label btn-success">Completado</span>`;
                            } else {
                                return `<span class="label btn-warning">En curso</span>`;
                            }
                        }
                    },
                    {
                        data: "IdRecojoRemesa",
                        title: "Accion",
                        render: function (data, type, row) {
                            return `<button type="button" class="btn btn-xs btn-info verDetalle" data-id="${data}" data-codsala="${row.CodSala}">
                                   <i class="glyphicon glyphicon-eye-open"></i> Ver Detalle
                                 </button>`
                        }
                    }
                ],
            });
            $("#tableResultado_remesas").on("click", ".verDetalle", function () {
                let IdRecojoRemesa = $(this).data("id");
                let CodigoSala = $(this).data("codsala");
                fechaini = $("#fechaInicio").val();
                fechafin = $("#fechaFin").val();

                let urlDetalle = `${basePath}RecojoRemesas/RecojoRemesasVista?id=${IdRecojoRemesa}&codsala=${CodigoSala}&fechaInicio=${fechaini}&fechaFin=${fechafin}`;
                window.open(urlDetalle, '_blank');
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
    let idtipobienmaterial = -1;
    let dataForm = {
        codsala: listasala, idtipobienmaterial,
        fechaini, fechafin
    }

    // Determinar la pestaña activa
    let activeTabHref = $('#myTab .active a').attr('href');
    if (!activeTabHref) {
        toastr.warning("Por favor, seleccione una pestaña.");
        return false;
    }

    // Establecer la URL basada en la pestaña activa
    let url = '';

    switch (activeTabHref) {
        case "#bienesMateriales":
            url = basePath + "BienesMateriales/ReporteBienesMaterialesDescargarExcelJson";
            break;
        case "#entesReguladores":
            url = basePath + "EntesReguladores/ReporteEntesReguladoresDescargarExcelJson";
            break;
        case "#ingresoSalidaGU":
            url = basePath + "IngresosSalidasGU/ReporteIngresosSalidasGUDescargarExcelJson";
            break;
        case "#recaudacionesPersonalParticipante":
            url = basePath + "RecaudacionesPersonalParticipante/ReporteRecaudacionesPersonalParticipanteDescargarExcelJson";
            break;
        case "#aperturaCierreSala":
            url = basePath + "AperturaCierreSala/ReporteAperturaCierreSalaXSalaJsonExcel";
            break;
        case "#accionesIncidentes":
            url = basePath + "AccionesIncidentesCajasTemporizadas/ReporteAccionesIncidentesCajasTemporizadasExcelJson";
            break;
        case "#logOcurrencias":
            url = basePath + "LogsOcurrencias/ReporteLogsOcurrenciasDescargarExcelJson";
            break;
        case "#recojoRemesas":
            url = basePath + "RecojoRemesas/ReporteRecojoRemesasDescargarExcelJson";
            break;
        default:
            toastr.error("No se encontró una acción para esta pestaña.");
            return false;
    }

    $.ajax({
        type: "POST",
        cache: false,
        url, url,
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
            deleteEnteRegulador(id)
        }
    });
});
const deleteEnteRegulador = (idregistro) => {
    const data = {
        id: idregistro
    };
    $.ajax({
        url: `${basePath}AperturaCierreSala/EliminarAperturaCierreSala`,
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
            buscarAperturaCierreSala();
        }
    });
}



function limpiarValidadoraperturaCierreSalaForm() {
    $("#aperturaCierreSalaForm").parent().find('div').removeClass("has-error");
    $("#aperturaCierreSalaForm").parent().find('i').removeAttr("style").hide();
    $("#aperturaCierreSalaForm").parent().find('div').removeClass("has-success");
}

function limpiarValidadorCierreSalaForm() {
    $("#cierreSalaForm").parent().find('div').removeClass("has-error");
    $("#cierreSalaForm").parent().find('i').removeAttr("style").hide();
    $("#cierreSalaForm").parent().find('div').removeClass("has-success");
}
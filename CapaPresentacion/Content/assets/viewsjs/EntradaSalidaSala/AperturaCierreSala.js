let objetodatatable
let objetodatatable2
let arraySalas = []
let IdAperturaCierreSala = 0
let contadorInterval = null;
CboSeleccionado = 0
CboSeleccionado1 = 0
CboSeleccionado2 = 0
btnFinalizarCierreSala = 0
let params = {}

$(document).ready(function () {
    //ObtenerListaSalas()
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
        buscarAperturaCierreSala(params);

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
    buscarAperturaCierreSala()
});
// Excel Plantilla ------------------------------------------------------------
$(document).on('click', '#btnExcel_Plantilla', function (e) {
    e.preventDefault()

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "AperturaCierreSala/PlantillaAperturaCierreSalaDescargarExcel",
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
        url: basePath + "AperturaCierreSala/ReporteAperturaCierreSalaXSalaJsonExcel",
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


//$(document).on("click", ".btnFinalizarCierreSala", function () {
//    $('#full-modal_aperturacierresala').modal('hide');
//    $('#full-modal_cierresala').modal('show');
//})


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
                $("#cboPrevencionista").html(result.data.map(item => `<option value="${item.IdBuk}">${item.NumeroDocumento} -${item.NombreCompleto} - ${item.Cargo}</option>`).join(""));
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
                $("#cboJefeSala").html(result.data.map(item => `<option value="${item.IdBuk}">${item.NumeroDocumento}  - ${item.NombreCompleto} ${item.Cargo}</option>`).join(""));
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

        $("#cboSalaCierreSala").html(arraySalas.map(item =>`<option value="${item.CodSala}">${item.Nombre}</option>`).join(""));
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

function buscarAperturaCierreSala(params) {
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
        url: basePath + "AperturaCierreSala/AperturaCierreSalaListarJson",
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
                    { data: "IdAperturaCierreSala", title: "ID" },
                    { data: "Sala", title: "Sala" },
                    {
                        data: "FechaRegistro",
                        title: "Fech Reg",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        data: "Fecha",
                        title: "Fech Apert/Cierre",
                        render: function (data) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },

                    {
                        data: "HoraApertura",
                        title: "Apert (Hora)",
                        render: function (data) {
                            return moment(data).format('HH:mm') == "00:00" ? "" : moment(data).format('hh:mm A');
                        }
                    }, 
                    {
                        data: "PrevencionistaApertura",
                        title: "Apertura (Preven.)",
                        render: function (data) {
                            if (data) {
                                return data.length > 25 ? data.substring(0, 25) + "..." : data;
                            }
                            return "";
                        }                    },
                    {
                        data: "JefeSalaApertura",
                        title: "Apertura (Jefe Sala)",
                        render: function (data) {
                            if (data) {
                                return data.length > 25 ? data.substring(0, 25) + "..." : data;
                            }
                            return "";
                        }
                    },
                    {
                        data: "HoraCierre",
                        title: "Cierre (Hora)",
                        render: function (data) {
                            return moment(data).format('HH:mm') == "00:00" ? '<span class="label btn-warning">No definido</span>' : moment(data).format('hh:mm A');
                        }
                    },
                    {
                        data: "PrevencionistaCierre",
                        title: "Cierre (Prevenc.)",
                        render: function (data) {
                            if (data) {
                                return data.length > 25 ? data.substring(0, 25) + "..." : data;
                            }
                            return data == "" ? '<span class="label btn-warning">No definido</span>' : data;
                        }
                    },
                    {
                        data: "JefeSalaCierre",
                        title: "Cierre (Jefe Sala)",
                        render: function (data) {
                            if (data) {
                                return data.length > 25 ? data.substring(0, 25) + "..." : data;
                            }
                            return data == "" ? '<span class="label btn-warning">No definido</span>' : data;
                        }
                    },
                    {
                        data: "IdAperturaCierreSala",
                        title: "Accion",
                        "render": function (o) {
                            return `
                                            <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-warning btnEditarbien" data-id="${o}">
                                                <i class="glyphicon glyphicon-log-in"></i>
                                            </button> 
                                             <button style="width:40px; height:40px;" type="button" class="btn btn-xs btn-primary btnFinalizarCierreSala" data-id="${o}">
                                                 <i class="glyphicon glyphicon-log-out"></i>
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
                    $('.btnEditarbien').tooltip({
                        title: "Editar Apertura"
                    });

                    $('.btnFinalizarCierreSala').tooltip({
                        title: "Editar Cierre"
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
                url: basePath + "AperturaCierreSala/ImportarExcelAperturaCierreSala",
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
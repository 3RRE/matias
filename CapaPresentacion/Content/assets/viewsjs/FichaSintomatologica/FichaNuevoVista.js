
// //CANVAS FIRMA
// const $canvas = document.querySelector("#canvas"), $btnLimpiar = document.querySelector("#btnLimpiar");
// const contexto = $canvas.getContext("2d");
// const COLOR_PINCEL = "black";
// const COLOR_FONDO = "white";
// const GROSOR = 2;
// let xAnterior = 0, yAnterior = 0, xActual = 0, yActual = 0;
// const obtenerXReal = (clientX) => clientX - $canvas.getBoundingClientRect().left;
// const obtenerYReal = (clientY) => clientY - $canvas.getBoundingClientRect().top;
// let haComenzadoDibujo = false;

// const limpiarCanvas = () => {
//     contexto.fillStyle = COLOR_FONDO;
//     contexto.fillRect(0, 0, $canvas.width, $canvas.height);
// };
// limpiarCanvas();
// $btnLimpiar.onclick = limpiarCanvas;

// $canvas.addEventListener("mousedown", evento => {
//     xAnterior = xActual;
//     yAnterior = yActual;
//     xActual = obtenerXReal(evento.clientX);
//     yActual = obtenerYReal(evento.clientY);
//     contexto.beginPath();
//     contexto.fillStyle = "black";
//     contexto.fillRect(xActual, yActual, GROSOR, GROSOR);
//     contexto.closePath();
//     haComenzadoDibujo = true;
// });

// $canvas.addEventListener("mousemove", (evento) => {
//     if (!haComenzadoDibujo) {
//         return;
//     }

//     xAnterior = xActual;
//     yAnterior = yActual;
//     xActual = obtenerXReal(evento.clientX);
//     yActual = obtenerYReal(evento.clientY);
//     contexto.beginPath();
//     contexto.moveTo(xAnterior, yAnterior);
//     contexto.lineTo(xActual, yActual);
//     contexto.strokeStyle = COLOR_PINCEL;
//     contexto.lineWidth = GROSOR;
//     contexto.stroke();
//     contexto.closePath();
// });
// ["mouseup", "mouseout"].forEach(nombreDeEvento => {
//     $canvas.addEventListener(nombreDeEvento, () => {
//         haComenzadoDibujo = false;
//     });
// });

//FUNCIONES VISTA

$(document).ready(function () {

    //Canvas
    let canvas = document.getElementById('signature-pad')

    // Adjust canvas coordinate space taking into account pixel ratio,
    // to make it look crisp on mobile devices.
    // This also causes canvas to be cleared.
    function resizeCanvas() {
        // When zoomed out to less than 100%, for some very strange reason,
        // some browsers report devicePixelRatio as less than 1
        // and only part of the canvas is cleared then.
        let ratio =  Math.max(window.devicePixelRatio || 1, 1)
        canvas.width = canvas.offsetWidth * ratio
        canvas.height = canvas.offsetHeight * ratio
        canvas.getContext("2d").scale(ratio, ratio)
    }
    
    window.onresize = resizeCanvas;
    resizeCanvas();
    
    let signaturePad = new SignaturePad(canvas, {
      backgroundColor: 'rgb(255, 255, 255)' // necessary for saving image as JPEG; can be removed is only saving as PNG or SVG
    })

    $("#CodSala").val(codsala);
    $("#nrodocumento").focus();
    $('.decimales').on('input', function () {
        this.value = this.value.replace(/[^0-9,.]/g, '').replace(/,/g, '.');
    });
    $('#complaintsForm').bootstrapValidator('enableFieldValidators', 'temperaturaSalida', false);
    $(document).on('click', ".nro_docbuscar", function (e) {
        var nrodoc = $("#nrodocumento").val();
        if (nrodoc == "") {
            toastr.error("Ingrese Nro Documento", "Mensaje Servidor");
            return false;
        }
        buscarregistroactivo(nrodoc);
        $("#complaintsForm").data('bootstrapValidator').resetForm();
    });

    $(document).on('keypress', function (e) {
        if (e.which == 13) {
            var nrodoc = $("#nrodocumento").val();
            if (nrodoc == "") {
                toastr.error("Ingrese Nro Documento", "Mensaje Servidor");
                return false;
            }
            buscarregistroactivo(nrodoc);
            $("#complaintsForm").data('bootstrapValidator').resetForm();
        }
    });

    $(document).on('click', "#btnSend", function (e) {

        // $("#Firma").val($canvas.toDataURL());
        let dataFirma = signaturePad.toDataURL();//save image to png
        $("#Firma").val(dataFirma);
        
      
        $("#complaintsForm").data('bootstrapValidator').resetForm();
        let validar = $("#complaintsForm").data('bootstrapValidator').validate();
        if (validar.isValid()) {

            if ($("#fichaId").val() > 0) {
                if ($("#Firma").val() == "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASwAAACWCAYAAABkW7XSAAAAAXNSR0IArs4c6QAABGhJREFUeF7t1IEJADAMAsF2/6EtdIuHywRyBu+2HUeAAIGAwDVYgZZEJEDgCxgsj0CAQEbAYGWqEpQAAYPlBwgQyAgYrExVghIgYLD8AAECGQGDlalKUAIEDJYfIEAgI2CwMlUJSoCAwfIDBAhkBAxWpipBCRAwWH6AAIGMgMHKVCUoAQIGyw8QIJARMFiZqgQlQMBg+QECBDICBitTlaAECBgsP0CAQEbAYGWqEpQAAYPlBwgQyAgYrExVghIgYLD8AAECGQGDlalKUAIEDJYfIEAgI2CwMlUJSoCAwfIDBAhkBAxWpipBCRAwWH6AAIGMgMHKVCUoAQIGyw8QIJARMFiZqgQlQMBg+QECBDICBitTlaAECBgsP0CAQEbAYGWqEpQAAYPlBwgQyAgYrExVghIgYLD8AAECGQGDlalKUAIEDJYfIEAgI2CwMlUJSoCAwfIDBAhkBAxWpipBCRAwWH6AAIGMgMHKVCUoAQIGyw8QIJARMFiZqgQlQMBg+QECBDICBitTlaAECBgsP0CAQEbAYGWqEpQAAYPlBwgQyAgYrExVghIgYLD8AAECGQGDlalKUAIEDJYfIEAgI2CwMlUJSoCAwfIDBAhkBAxWpipBCRAwWH6AAIGMgMHKVCUoAQIGyw8QIJARMFiZqgQlQMBg+QECBDICBitTlaAECBgsP0CAQEbAYGWqEpQAAYPlBwgQyAgYrExVghIgYLD8AAECGQGDlalKUAIEDJYfIEAgI2CwMlUJSoCAwfIDBAhkBAxWpipBCRAwWH6AAIGMgMHKVCUoAQIGyw8QIJARMFiZqgQlQMBg+QECBDICBitTlaAECBgsP0CAQEbAYGWqEpQAAYPlBwgQyAgYrExVghIgYLD8AAECGQGDlalKUAIEDJYfIEAgI2CwMlUJSoCAwfIDBAhkBAxWpipBCRAwWH6AAIGMgMHKVCUoAQIGyw8QIJARMFiZqgQlQMBg+QECBDICBitTlaAECBgsP0CAQEbAYGWqEpQAAYPlBwgQyAgYrExVghIgYLD8AAECGQGDlalKUAIEDJYfIEAgI2CwMlUJSoCAwfIDBAhkBAxWpipBCRAwWH6AAIGMgMHKVCUoAQIGyw8QIJARMFiZqgQlQMBg+QECBDICBitTlaAECBgsP0CAQEbAYGWqEpQAAYPlBwgQyAgYrExVghIgYLD8AAECGQGDlalKUAIEDJYfIEAgI2CwMlUJSoCAwfIDBAhkBAxWpipBCRAwWH6AAIGMgMHKVCUoAQIGyw8QIJARMFiZqgQlQMBg+QECBDICBitTlaAECBgsP0CAQEbAYGWqEpQAAYPlBwgQyAgYrExVghIgYLD8AAECGQGDlalKUAIEDJYfIEAgI2CwMlUJSoCAwfIDBAhkBAxWpipBCRAwWH6AAIGMgMHKVCUoAQIGyw8QIJARMFiZqgQlQMBg+QECBDICBitTlaAECBgsP0CAQEbAYGWqEpQAAYPlBwgQyAgYrExVghIg8ACBlFZdWYR+vQAAAABJRU5ErkJggg==") {
                    toastr.error("Complete Firma", "Mensaje Servidor");
                    return false;
                }

            }

            var dataForm = $('#complaintsForm').serializeFormJSON();
            console.log(dataForm)
            $.ajax({
                url: basePath + "FichaSintomatologica/FichaNuevoJson",
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
        
                        toastr.success(response.mensaje, "Mensaje Servidor");
                        setTimeout(function () {
                            window.location.reload(true);
                        }, 1500);
        
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
        else {
            toastr.error("Complete los Campos Obligatorios", "Mensaje Servidor");
        }

    });
    $(document).on('click','#btnLimpiarFirma',function(e){
        e.preventDefault()
        signaturePad.clear()
    })


});

$("#complaintsForm")
    .bootstrapValidator({
        container: '#messages',
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            empresa: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            ruc: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            DOI: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            nombre: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            apellidoPaterno: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            apellidoMaterno: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            celular: {
                validators: {
                    notEmpty: {
                        message: ''
                    },
                }
            },
            direccion: {
                validators: {
                    notEmpty: {
                        message: ''
                    },
                },
            },
            area: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            temperaturaIngreso: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            temperaturaSalida: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            temperaturaSalida: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            terminos: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
          
        }
    })
    .on('success.field.bv', function (e, data) {
        e.preventDefault();
        let $parent = data.element.parents('.form-group');
        $parent.removeClass('has-success');
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });


function buscarregistroactivo(nrodoc) {
    $.ajax({
        url: basePath + "FichaSintomatologica/FichaObtenerData",
        type: "POST",
        contentType: "application/json",

        data: JSON.stringify({ nro_documento: nrodoc }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            console.log(response);
            //toastr.success("Cargando Informacion", "Mensaje Servidor");
            toastr.clear();
            if (response.respuesta) {

                //Agregamos la data del empleado
                $("#empresa").val(response.empleadoEntidad.Empresa);
                $("#ruc").val(response.empleadoEntidad.Ruc);
                $("#EmpleadoId").val(response.empleadoEntidad.EmpleadoID);
                $("#nombre").val(response.empleadoEntidad.Nombres);
                $("#apellidoPaterno").val(response.empleadoEntidad.ApellidosPaterno);
                $("#apellidoMaterno").val(response.empleadoEntidad.ApellidosMaterno);
                $("#celular").val(response.empleadoEntidad.Telefono);
                $("#direccion").val(response.empleadoEntidad.Direccion);
                $("#area").val(response.empleadoEntidad.AreaTrabajo);
                $("#DOI").val(response.empleadoEntidad.DOI);
                //Agregamos la data de la ficha
                if (response.ficha.FichaId > 0) {
                    $("#fichaId").val(response.ficha.FichaId);
                    $("#temperaturaIngreso").val(response.ficha.TemperaturaIngreso);
                    $('#signo1Ingreso').prop('checked', response.ficha.Signo1Ingreso);
                    $('#signo2Ingreso').prop('checked', response.ficha.Signo2Ingreso);
                    $('#signo3Ingreso').prop('checked', response.ficha.Signo3Ingreso);
                    $('#signo4Ingreso').prop('checked', response.ficha.Signo4Ingreso);
                    $('#signo5Ingreso').prop('checked', response.ficha.Signo5Ingreso);
                    $('#signo6Ingreso').prop('checked', response.ficha.Signo6Ingreso);
                    $(".fecha_").text("Fecha :" + moment(response.ficha.FechaIngreso).format("DD/MM/YYYY"));
                    $('#complaintsForm').bootstrapValidator('enableFieldValidators', 'temperaturaSalida', true);
                }
                else {
                    $('#complaintsForm').bootstrapValidator('enableFieldValidators', 'temperaturaSalida', false);
                }
                $("#complaintsForm").data('bootstrapValidator').resetForm();
            }
            else {
                
                toastr.error(response.mensaje, "Mensaje Servidor");
                setTimeout(function () {
                    window.location.reload(true);
                }, 1500);
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            toastr.error("Error Servidor", "Mensaje Servidor");
        }
    });
}
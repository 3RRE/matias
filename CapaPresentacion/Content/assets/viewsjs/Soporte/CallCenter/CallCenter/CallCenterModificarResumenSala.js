$(document).ready(function () {
    $("#frmRegistroResumenSala")
        .bootstrapValidator({
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                fechaOperacion: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Fecha Operacion, Obligatorio'
                        },
                        date: {
                            format: 'DD/MM/YYYY',
                            message: 'La Fecha de Operacion no es valida'
                        }
                    }
                },
                estado: {
                    validators: {
                        notEmpty: {
                            message: 'Elija un estado, Obligatorio'
                        }
                    }
                },
                fondocaja: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese un Fondo Caja, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                salidaotros: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese salida Otros, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                prestamodeotrasala: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Prestamo de Otra Sala, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                devolucionaotrasala: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese devolucion a otra sala, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                refuerzodefondo: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese refuerzo de fondo, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                fecharegistro: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese fecha de Registro, Obligatorio'
                        },
                        date: {
                            format: 'DD/MM/YYYY',
                            message: 'La Fecha de Operacion no es valida'
                        }
                    }
                },
                sorteos: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Sorteos, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                monedasfallas: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Monedas Fallas, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                efectivosoles: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Efectivo en Soles, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                efectivodolares: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Efectivo en Dolares, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                }
                , res_usuario: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Usuario, Obligatorio'
                        }
                    }
                },
                saldoinicial: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Saldo Inicial, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                ingresosotros: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese ingreso otros, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                prestamoaotrasala: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese prestamo a otra sala, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                devoluciondeotrasala: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese devolucion de otra sala, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                prestamoaoficina: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese prestamo a oficina, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },

                sobrantes: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese sobrantes, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                faltantes: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese faltantes, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                visa: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Visa, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },

                efectivofinal: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Efectivo final, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                mastercard: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Mastercard, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },
                otrastarjetas: {
                    validators: {
                        notEmpty: {
                            message: 'Ingrese Otras tarjetas, Obligatorio'
                        },
                        numeric: {
                            message: 'El valor no es un numero', // The default separators
                            thousandsSeparator: '',
                            decimalSeparator: '.'
                        },
                    }
                },


            }
        })
        .on('success.field.bv', function (e, data) {
            e.preventDefault();
            var $parent = data.element.parents('.form-group');
            // Remove the has-success class
            $parent.removeClass('has-success');
            // Hide the success icon
            $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
        });
    DataResumenSala();
    $(".dateOnly").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });

    $('.btnListar').on('click', function (e) {
        window.location.replace(basePath + "CallCenter/CallCenterResumenSalaVista");
    });

    $('.btnGuardar').on('click', function (e) {
        if (!$("#ip_resumen").val().trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        }
        $("#frmRegistroResumenSala").data('bootstrapValidator').resetForm();
        var validar = $("#frmRegistroResumenSala").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            $.confirm({
                icon: 'fa fa-spinner fa-spin',
                title: 'Esta Seguro que desea Actualizar el Resumen Sala',
                theme: 'black',
                animationBounce: 1.5,
                columnClass: 'col-md-6 col-md-offset-3',
                confirmButtonClass: 'btn-info',
                cancelButtonClass: 'btn-warning',
                confirmButton: "confirmar",
                cancelButton: 'Aún No',
                content: "",
                confirm: function () {
                    var url = $("#ip_resumen").val() + "/servicio/ModificarResumenSala?fechaOperacion=" + $("#fechaOperacion").val() +
                        "&estado=" + $("#estado").val() + "&usuario=" + $("#res_usuario").val() + "&saldoinicialsoles=" + $("#saldoinicial").val() +
                        "&fondocaja=" + $("#fondocaja").val() + "&salidaotros=" + $("#salidaotros").val() +"&ingresootros=" + $("#ingresosotros").val() +
                            "&prestamoaotrasala=" + $("#prestamoaotrasala").val() + "&prestamodeotrasala=" + $("#prestamodeotrasala").val() +
                            "&devolucionaotrasala=" + $("#devolucionaotrasala").val() + "&devoluciondeotrasala=" + $("#devoluciondeotrasala").val() +
                            "&prestamoaoficina=" + $("#prestamoaoficina").val() + "&refuerzodefondo=" + $("#refuerzodefondo").val() + "&fecharegistro=" + $("#fecharegistro").val() +
                            "&sobrantes=" + $("#sobrantes").val() + "&faltantes=" + $("#faltantes").val() + "&sorteos=" + $("#sorteos").val() +
                            "&monedasfallas=" + $("#monedasfallas").val() + "&visa=" + $("#visa").val() + "&efectivofinal=" + $("#efectivofinal").val() +
                            "&efectivoSoles=" + $("#efectivosoles").val() + "&efectivoDolares=" + $("#efectivodolares").val() +
                            "&mastercard=" + $("#mastercard").val() + "&otrastarjetas=" + $("#otrastarjetas").val() + "&codResumen=" + $("#id_resumen").val();

                    $.ajax({
                        type: "POST",
                        cache: false,
                        url: basePath + "CallCenter/ConsultaModificarResumenSalaJson",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ url: url }),
                        beforeSend: function (xhr) {
                            $.LoadingOverlay("show");
                        },
                        success: function (response) {
                            response = response.data;
                            if (response == '"Registros Afectados: 1"') {
                                toastr.success("Se ha actualizado los datos correctamente");
                            }
                            else {
                                toastr.error("Error al actualizar los datos, revise los campos");
                            }
                        },
                        error: function (request, status, error) {
                            toastr.error("Error De Conexion, Servidor no Encontrado.");
                        },
                        complete: function (resul) {
                            $.LoadingOverlay("hide");
                        }
                    });

                },
                cancel: function () {
                },
            });
        }
    });
});

function DataResumenSala() {
    if (!$("#ip_resumen").val().trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    }
    var url = $("#ip_resumen").val() + "/servicio/ObtenerResumenSalaId?codResumen=" + $("#id_resumen").val();
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CallCenter/ConsultaObtenerResumenSalaIdJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            response = response.data;
            $("#fechaOperacion").val(moment(response[0].fechaOperacion).format('DD/MM/YYYY'));
            $("#estado").val(response[0].estado).change();
            $("#res_usuario").val(response[0].usuario);
            $("#saldoinicial").val(response[0].saldoinicialsoles);
            $("#fondocaja").val(response[0].fondocaja);
            $("#salidaotros").val(response[0].salidaotros);
            $("#ingresosotros").val(response[0].ingresootros);
            $("#prestamoaotrasala").val(response[0].prestamoaotrasala);
            $("#prestamodeotrasala").val(response[0].prestamodeotrasala);
            $("#devolucionaotrasala").val(response[0].devolucionaotrasala);
            $("#devoluciondeotrasala").val(response[0].devoluciondeotrasala);
            $("#prestamoaoficina").val(response[0].prestamoaoficina);
            $("#refuerzodefondo").val(response[0].refuerzodefondo);
            $("#fecharegistro").val(moment(response[0].fecharegistro).format('DD/MM/YYYY'));
            $("#sobrantes").val(response[0].sobrantes);
            $("#faltantes").val(response[0].faltantes);
            $("#sorteos").val(response[0].sorteos);
            $("#monedasfallas").val(response[0].monedasfallas);
            $("#visa").val(response[0].visa);
            $("#efectivofinal").val(response[0].efectivofinal);
            $("#efectivosoles").val(response[0].efectivoSoles);
            $("#efectivodolares").val(response[0].efectivoDolares);
            $("#mastercard").val(response[0].mastercard);
            $("#otrastarjetas").val(response[0].otrastarjetas);
        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}


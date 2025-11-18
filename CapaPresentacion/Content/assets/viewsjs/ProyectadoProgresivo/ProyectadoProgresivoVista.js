var IdproyectadoProgresivoG = 0;
$(document).ready(function () {
    $("#frmNuevo")
        .bootstrapValidator({
            container: '#messages',
            excluded: [':disabled', ':hidden', ':not(:visible)'],
            feedbackIcons: {
                valid: 'icon icon-check',
                invalid: 'icon icon-cross',
                validating: 'icon icon-refresh'
            },
            fields: {
                IncrementoPozoInferior: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                IncrementoPozoMedio: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                IncrementoPozoSuperior: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                NroMaquina: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                PremioBasePozoInferior: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                PremioBasePozoMedio: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                PremioBasePozoSuperior: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                PremioMaximoPozoInferior: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                PremioMaximoPozoMedio: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                PremioMaximoPozoSuperior: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                PremioMinimoPozoInferior: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                PremioMinimoPozoMedio: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                PremioMinimoPozoSuperior: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                Retencion: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                TipoCambio: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                TotalJugMes: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                Descripcion: {
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                }
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

    function getCases() {
        var element = $("#cboCaso")
        var data = {}

        $.ajax({
            url: `${basePath}/ProyectadoProgresivo/ProyectadoProgresivoListarJson`,
            type: "POST",
            data: JSON.stringify(data),
            contentType: "application/json",
            beforeSend: function () {
                element.html("")
                element.append('<option value="">Cargando...</option>')
                element.attr("disabled", "disabled")
            },
            success: function (response) {
                var cases = response.data;

                element.html("")

                if (cases.length > 0) {
                    element.append('<option value="">--Seleccione--</option>')

                    $.each(cases, function (index, value) {
                        element.append(`<option value="${value.IdProyectadoProgresivo}">${value.Descripcion}</option>`)
                    })
                } else {
                    element.append('<option value="">--No hay casos registrados--</option>')

                    toastr.warning("No hay casos registrados", "Mensaje Servidor")
                }
            },
            complete: function () {
                element.removeAttr("disabled")
                element.select2()
            }
        })
    }

    getCases()

    /*$.when(llenarSelect(basePath + "ProyectadoProgresivo/ProyectadoProgresivoListarJson", {}, "cboCaso", "IdProyectadoProgresivo", "Descripcion", "")).then(function (response, textStatus) {
        $("#cboCaso").select2();
    });*/

    $(document).on('click', '#btnNuevo', function () {
        $("#full-modal").modal();
    });

    $(document).on('click', '#btnEditar', function () {
        if (IdproyectadoProgresivoG == 0) {
        } else {
            $("#full-modal-edit").modal();
            var url = basePath + "ProyectadoProgresivo/ProyectoProgresivoObtenerIdJson";
            $.ajax({
                type: 'POST',
                url: url,
                data: {
                    Id: IdproyectadoProgresivoG
                },
                success: function (response) {
                    var data = response.data;
                    $("#IdProyectadoProgresivo").val(data.IdProyectadoProgresivo);
                    $("#DescripcionEditar").val(data.Descripcion);
                    $("#NroMaquinaEditar").val(data.NroMaquina);
                    $("#TotalJugMesEditar").val(data.TotalJugMes);
                    $("#TipoCambioEditar").val(data.TipoCambio);
                    $("#RetencionEditar").val((data.Retencion * 100).toFixed(2));
                    $("#PremioBasePozoInferiorEditar").val(data.PremioBasePozoInferior);
                    $("#PremioBasePozoMedioEditar").val(data.PremioBasePozoMedio);
                    $("#PremioBasePozoSuperiorEditar").val(data.PremioBasePozoSuperior);
                    $("#PremioMinimoPozoInferiorEditar").val(data.PremioMinimoPozoInferior);
                    $("#PremioMinimoPozoMedioEditar").val(data.PremioMinimoPozoMedio);
                    $("#PremioMinimoPozoSuperiorEditar").val(data.PremioMinimoPozoSuperior);
                    $("#PremioMaximoPozoInferiorEditar").val(data.PremioMaximoPozoInferior);
                    $("#PremioMaximoPozoMedioEditar").val(data.PremioMaximoPozoMedio);
                    $("#PremioMaximoPozoSuperiorEditar").val(data.PremioMaximoPozoSuperior);
                    $("#IncrementoPozoInferiorEditar").val((data.IncrementoPozoInferior * 100).toFixed(2));
                    $("#IncrementoPozoMedioEditar").val((data.IncrementoPozoMedio * 100).toFixed(2));
                    $("#IncrementoPozoSuperiorEditar").val((data.IncrementoPozoSuperior * 100).toFixed(2));
                }
            });
        }
    });

    $(document).on('change', '#cboCaso', function () {
        var IdProyectadoProgresivo = $(this).val();
        var url = basePath + "ProyectadoProgresivo/ProyectoProgresivoObtenerIdJson";
        IdproyectadoProgresivoG = IdProyectadoProgresivo;

        $.ajax({
            type: 'POST',
            url: url,
            data: {
                Id: IdProyectadoProgresivo
            },
            success: function (response) {
                data = response.data;
                NroMaquina = parseInt(data.NroMaquina, 10);
                $("#FechaCaso").html(moment(data.FechaRegistro).format("DD-MM-YYYY"))
                $("#NroMaquinaEncabezado").val(FormatoMillar(NroMaquina));
                $("#TotalJugMesEncabezado").val(FormatoMillar(data.TotalJugMes.toFixed(2)));
                $("#TipoCambioEncabezado").val(FormatoMillar(data.TipoCambio));
                retencionPorcentaje = data.Retencion * 100;
                $("#RetencionEncabezado").val(FormatoMillar(retencionPorcentaje.toFixed(2)));
                //CantidadDias
                CantidadDias = 30;
               
                $(".CantidadDias").val(FormatoMillar(CantidadDias));
                $(".NroMaquinasHoy").val(FormatoMillar(NroMaquina));
                $(".NroMaquinas").val(NroMaquina);
                TotalSolesJug = data.TotalJugMes;
                $("#TotalSolesJug").val(FormatoMillar(TotalSolesJug));
                TotalSolesJugMesProyectado1 = data.TotalJugMes;
                $("#TotalSolesJugMesProyectado1").val(FormatoMillar(TotalSolesJugMesProyectado1));
                SolesJugPorDia = data.TotalJugMes / CantidadDias;
                $("#SolesJugPorDia").val(FormatoMillar(SolesJugPorDia.toFixed(2)));
                $("#SolesJugPorDiaProyectado1").val(FormatoMillar(SolesJugPorDia.toFixed(2)));
                PromedioSolesJugxMaquina = SolesJugPorDia / NroMaquina;
                PromedioSolesJugxMaquina1 = SolesJugPorDia / NroMaquina;
                $("#PromedioSolesJugxMaquina").val(FormatoMillar(PromedioSolesJugxMaquina.toFixed(2)));
                $("#PromedioSolesJugxMaquinaProyectado1").val(FormatoMillar(PromedioSolesJugxMaquina1.toFixed(2)));
                //Utilidad
                utilidad = data.TotalJugMes * data.Retencion;
                $("#UtilidadEncabezado").val(FormatoMillar(utilidad.toFixed(2)));
                //Soles Jugados x Día
                solesJugadoDia = data.TotalJugMes / 30;
                $("#SolesJugadosEncabezado").val(FormatoMillar(solesJugadoDia.toFixed(2)));
                //Promedio S/ Jugad.
                promedioSolesJugad = solesJugadoDia / NroMaquina;
                $("#PromedioSJugadoEncabezado").val(FormatoMillar(promedioSolesJugad.toFixed(2)));
                //Media
                media = utilidad / NroMaquina / 30 / data.TipoCambio;
                $("#MediaSEncabezado").val(FormatoMillar(media.toFixed(2)));
                //$("#PorcentajePremioEncabezado").val();
                //$("#TotalPremiosEncabezado").val();
                /*cuadro amarillo*/
                promedio1 = (data.PremioMinimoPozoInferior + data.PremioMaximoPozoInferior) / 2
                $("#PremioPromedioPozoInferiorParametro").val(FormatoMillar(promedio1.toFixed(2)));
                promedio2 = (data.PremioMinimoPozoMedio + data.PremioMaximoPozoMedio) / 2;
                $("#PremioPromedioMedioParametro").val(FormatoMillar(promedio2.toFixed(2)));
                promedio3 = (data.PremioMinimoPozoSuperior + data.PremioMaximoPozoSuperior) / 2;
                $("#PremioPromedioSuperiorParametro").val(FormatoMillar(promedio3.toFixed(2)));
                avancePromedioInferior = promedio1 - data.PremioBasePozoInferior;
                $("#AvancePromedioPozoInferiorParametro").val(FormatoMillar(avancePromedioInferior.toFixed(2)));
                avancePromedioMedio = promedio2 - data.PremioBasePozoMedio;
                $("#AvancePromedioPozoMedioParametro").val(FormatoMillar(avancePromedioMedio.toFixed(2)));
                avancePromedioSuperior = promedio3 - data.PremioBasePozoSuperior;
                $("#AvancePromedioPozoSuperiorParametro").val(FormatoMillar(avancePromedioSuperior.toFixed(2)));
                $("#TotalPozoInferior").val();
                $("#TotalPozoMedio").val();
                $("#TotalPozoSuperior").val();
                //INCR. DE POZO POR DÍA
                IncPozoActualInferior = solesJugadoDia * data.IncrementoPozoInferior;
                $("#IncPozoActualInferior").val(FormatoMillar(IncPozoActualInferior.toFixed(2)));
                IncPozoActualMedio = solesJugadoDia * data.IncrementoPozoMedio;
                $("#IncPozoActualMedio").val(FormatoMillar(IncPozoActualMedio.toFixed(2)));
                IncPozoActualSuperior = solesJugadoDia * data.IncrementoPozoSuperior;
                $("#IncPozoActualSuperior").val(FormatoMillar(IncPozoActualSuperior.toFixed(2)));
                IncPozoActualDia = IncPozoActualInferior + IncPozoActualMedio + IncPozoActualSuperior;
                $("#IncPozoActualDia").val(FormatoMillar(IncPozoActualDia.toFixed(2)));
                SolesJugadosPremioPozoInferiorParametro = (solesJugadoDia * avancePromedioInferior) / IncPozoActualInferior;
                $("#SolesJugadosPremioPozoInferiorParametro").val(FormatoMillar(SolesJugadosPremioPozoInferiorParametro.toFixed(2)));
                SolesJugadosPremioMedioParametro = (solesJugadoDia * avancePromedioMedio) / IncPozoActualMedio;
                $("#SolesJugadosPremioMedioParametro").val(FormatoMillar(SolesJugadosPremioMedioParametro.toFixed(2)));
                SolesJugadosPremioSuperiorParametro = (solesJugadoDia * avancePromedioSuperior) / IncPozoActualSuperior;
                $("#SolesJugadosPremioSuperiorParametro").val(FormatoMillar(SolesJugadosPremioSuperiorParametro.toFixed(2)));
                MontoOcultoMinimoPozoInferior = (data.PremioBasePozoInferior * (data.PremioMinimoPozoInferior - data.PremioBasePozoInferior)) / avancePromedioInferior;
                $("#MontoOcultoMinimoPozoInferior").val(FormatoMillar(MontoOcultoMinimoPozoInferior.toFixed(2)));
                MontoOcultoMinimoPozoMedio = (data.PremioBasePozoMedio * (data.PremioMinimoPozoMedio - data.PremioBasePozoMedio)) / avancePromedioMedio;
                $("#MontoOcultoMinimoPozoMedio").val(FormatoMillar(MontoOcultoMinimoPozoMedio.toFixed(2)));
                MontoOcultoMinimoPozoSuperior = (data.PremioBasePozoSuperior * (data.PremioMinimoPozoSuperior - data.PremioBasePozoSuperior)) / avancePromedioSuperior;
                $("#MontoOcultoMinimoPozoSuperior").val(MontoOcultoMinimoPozoSuperior.toFixed(2));

                MontoOcultoMaximoPozoInferior = (data.PremioBasePozoInferior * (data.PremioMaximoPozoInferior - data.PremioBasePozoInferior)) / avancePromedioInferior;
                $("#MontoOcultoMaximoPozoInferior").val(FormatoMillar(MontoOcultoMaximoPozoInferior.toFixed(2)));

                MontoOcultoMaximoPozoMedio = (data.PremioBasePozoMedio * (data.PremioMaximoPozoMedio - data.PremioBasePozoMedio)) / avancePromedioMedio;
                $("#MontoOcultoMaximoPozoMedio").val(FormatoMillar(MontoOcultoMaximoPozoMedio.toFixed(2)));

                MontoOcultoMaximoPozoSuperior = (data.PremioBasePozoSuperior * (data.PremioMaximoPozoSuperior - data.PremioBasePozoSuperior)) / avancePromedioSuperior;
                $("#MontoOcultoMaximoPozoSuperior").val(MontoOcultoMaximoPozoSuperior.toFixed(2));

                IncrementoPozoOcultoInferior = (data.PremioBasePozoInferior / SolesJugadosPremioPozoInferiorParametro) * 100;
                $("#IncrementoPozoOcultoInferior").val(FormatoMillar(IncrementoPozoOcultoInferior.toFixed(3)) + "%");
                IncrementoPozoOcultoMedio = (data.PremioBasePozoMedio / SolesJugadosPremioMedioParametro) * 100;
                $("#IncrementoPozoOcultoMedio").val(FormatoMillar(IncrementoPozoOcultoMedio.toFixed(3)) + "%");
                IncrementoPozoOcultoSuperior = (data.PremioBasePozoSuperior / SolesJugadosPremioSuperiorParametro) * 100;
                $("#IncrementoPozoOcultoSuperior").val(FormatoMillar(IncrementoPozoOcultoSuperior.toFixed(3)) + "%");
                PorcentajePremioEncabezado = (data.IncrementoPozoInferior * 100) + (data.IncrementoPozoMedio * 100) + (data.IncrementoPozoSuperior * 100) + IncrementoPozoOcultoInferior + IncrementoPozoOcultoMedio + IncrementoPozoOcultoSuperior;
                $("#PorcentajePremioEncabezado").val(FormatoMillar(PorcentajePremioEncabezado.toFixed(3)) + "%");
                $(".PorcentajePremioProyectado").val(FormatoMillar(PorcentajePremioEncabezado.toFixed(2)) + "%");
                TotalPremios = data.TotalJugMes * (PorcentajePremioEncabezado / 100);
                $("#TotalPremios").val(FormatoMillar(TotalPremios.toFixed(2)));
                $("#TotalPremiosProyectado1").val(FormatoMillar(TotalPremios.toFixed(2)));
                PorcentajeRetorno = 8;
                $("#PorcentajeRetorno").val(FormatoMillar(PorcentajeRetorno.toFixed(2)) + "%");
                PorcentajeRetornoProyectado1 = PorcentajeRetorno - PorcentajePremioEncabezado;
                $("#PorcentajeRetornoProyectado1").val(FormatoMillar(PorcentajeRetornoProyectado1.toFixed(2)) + "%");
                PorcentajeRetornoProyectado2 = PorcentajeRetornoProyectado1;
                $("#PorcentajeRetornoProyectado2").val(FormatoMillar(PorcentajeRetornoProyectado2.toFixed(2)) + "%");
                PorcentajeRetornoProyectado3 = PorcentajeRetornoProyectado1;
                $("#PorcentajeRetornoProyectado3").val(FormatoMillar(PorcentajeRetornoProyectado3.toFixed(2)) + "%");
                UtilidadMes = TotalSolesJug * (PorcentajeRetorno / 100);
                $("#UtilidadMes").val(FormatoMillar(UtilidadMes.toFixed(2)));
                UtilidadMesProyectado1 = TotalSolesJugMesProyectado1 * (PorcentajeRetornoProyectado1 / 100);
                $("#UtilidadMesProyectado1").val(FormatoMillar(UtilidadMesProyectado1.toFixed(2)));

                TotalSolesJugMesProyectado2 = (TotalSolesJugMesProyectado1 * PorcentajeRetorno) / PorcentajeRetornoProyectado1;
                $("#TotalSolesJugMesProyectado2").val(FormatoMillar(TotalSolesJugMesProyectado2.toFixed(2)));

                TotalSolesJugMesProyectado3 = (TotalSolesJugMesProyectado2 + TotalSolesJugMesProyectado2) - TotalSolesJugMesProyectado1;
                $("#TotalSolesJugMesProyectado3").val(FormatoMillar(TotalSolesJugMesProyectado3.toFixed(2)));

                SolesJugPorDiaProyectado2 = TotalSolesJugMesProyectado2 / CantidadDias;
                $("#SolesJugPorDiaProyectado2").val(FormatoMillar(SolesJugPorDiaProyectado2.toFixed(2)));

                SolesJugPorDiaProyectado3 = TotalSolesJugMesProyectado3 / CantidadDias;
                $("#SolesJugPorDiaProyectado3").val(FormatoMillar(SolesJugPorDiaProyectado3.toFixed(2)));

                PromedioSolesJugxMaquinaProyectado2 = SolesJugPorDiaProyectado2 / NroMaquina;
                $("#PromedioSolesJugxMaquinaProyectado2").val(FormatoMillar(PromedioSolesJugxMaquinaProyectado2.toFixed(2)));

                PromedioSolesJugxMaquinaProyectado3 = SolesJugPorDiaProyectado3 / NroMaquina;
                $("#PromedioSolesJugxMaquinaProyectado3").val(FormatoMillar(PromedioSolesJugxMaquinaProyectado3.toFixed(2)));

                TotalPremiosProyectado2 = TotalSolesJugMesProyectado2 * (PorcentajePremioEncabezado / 100);
                $("#TotalPremiosProyectado2").val(FormatoMillar(TotalPremiosProyectado2.toFixed(2)));

                TotalPremiosProyectado3 = TotalSolesJugMesProyectado3 * (PorcentajePremioEncabezado / 100);
                $("#TotalPremiosProyectado3").val(FormatoMillar(TotalPremiosProyectado3.toFixed(2)));

                UtilidadMesProyectado2 = TotalSolesJugMesProyectado2 * (PorcentajeRetornoProyectado2 / 100);
                $("#UtilidadMesProyectado2").val(FormatoMillar(UtilidadMesProyectado2.toFixed(2)));

                UtilidadMesProyectado3 = TotalSolesJugMesProyectado3 * (PorcentajeRetornoProyectado3 / 100);
                $("#UtilidadMesProyectado3").val(FormatoMillar(UtilidadMesProyectado3.toFixed(2)));

                TotalPremiosEncabezado = data.TotalJugMes * (PorcentajePremioEncabezado / 100);
                console.log("TotalPremiosEncabezado", TotalPremiosEncabezado)
                $("#TotalPremiosEncabezado").val(FormatoMillar(TotalPremiosEncabezado.toFixed(2)));

                AvancePromedioPozoOcultoInferior = SolesJugadosPremioPozoInferiorParametro * (IncrementoPozoOcultoInferior / 100);
                $("#AvancePromedioPozoOcultoInferior").val(FormatoMillar(AvancePromedioPozoOcultoInferior.toFixed(2)));
                AvancePromedioPozoOcultoMedio = SolesJugadosPremioMedioParametro * (IncrementoPozoOcultoMedio / 100);
                $("#AvancePromedioPozoOcultoMedio").val(FormatoMillar(AvancePromedioPozoOcultoMedio.toFixed(2)));
                AvancePromedioPozoOcultoSuperior = SolesJugadosPremioSuperiorParametro * (IncrementoPozoOcultoSuperior / 100);
                $("#AvancePromedioPozoOcultoSuperior").val(FormatoMillar(AvancePromedioPozoOcultoSuperior.toFixed(2)));

                IncPozoOcultoInferior = solesJugadoDia * (IncrementoPozoOcultoInferior / 100);
                $("#IncPozoOcultoInferior").val(FormatoMillar(IncPozoOcultoInferior.toFixed(2)));
                IncPozoOcultoMedio = solesJugadoDia * (IncrementoPozoOcultoMedio / 100);
                $("#IncPozoOcultoMedio").val(FormatoMillar(IncPozoOcultoMedio.toFixed(2)));
                IncPozoOcultoSuperior = solesJugadoDia * (IncrementoPozoOcultoSuperior / 100);
                $("#IncPozoOcultoSuperior").val(FormatoMillar(IncPozoOcultoSuperior.toFixed(2)));
                IncPozoOcultoDia = IncPozoOcultoInferior + IncPozoOcultoMedio + IncPozoOcultoSuperior;
                $("#IncPozoOcultoDia").val(FormatoMillar(IncPozoOcultoDia.toFixed(2)));
                TotalIncInferior = IncPozoOcultoInferior + IncPozoActualInferior;
                $("#TotalIncInferior").val(FormatoMillar(TotalIncInferior.toFixed(2)));
                TotalIncMedio = IncPozoActualMedio + IncPozoOcultoMedio;
                $("#TotalIncMedio").val(FormatoMillar(TotalIncMedio.toFixed(2)));
                TotalIncSuperior = IncPozoActualSuperior + IncPozoOcultoSuperior;
                $("#TotalIncSuperior").val(FormatoMillar(TotalIncSuperior.toFixed(2)));

                PozoInferiorPremiosDiaProyectado2 = 3.78;
                $("#PozoInferiorPremiosDiaProyectado2").val(PozoInferiorPremiosDiaProyectado2.toFixed(2));
                PozoInferiorPremiosDiaProyectado3 = 4.88;
                $("#PozoInferiorPremiosDiaProyectado3").val(PozoInferiorPremiosDiaProyectado3.toFixed(2));
                PozoMedioDiasPremioProyectado2 = 4.42;
                $("#PozoMedioDiasPremioProyectado2").val(PozoMedioDiasPremioProyectado2.toFixed(2));
                PozoMedioDiasPremioProyectado3 = 3.41;
                $("#PozoMedioDiasPremioProyectado3").val(PozoMedioDiasPremioProyectado3.toFixed(2));
                PozoSuperiorDiasPremioProyectado2 = 25.31;
                $("#PozoSuperiorDiasPremioProyectado2").val(PozoSuperiorDiasPremioProyectado2.toFixed(2));
                PozoSuperiorDiasPremioProyectado3 = 19.57;
                $("#PozoSuperiorDiasPremioProyectado3").val(PozoSuperiorDiasPremioProyectado3.toFixed(2));

                MediaSoles = UtilidadMes / CantidadDias / NroMaquina;
                $("#MediaSoles").val(FormatoMillar(MediaSoles.toFixed(2)));
                MediaSolesProyectado1 = UtilidadMesProyectado1 / CantidadDias / NroMaquina;
                $("#MediaSolesProyectado1").val(FormatoMillar(MediaSolesProyectado1.toFixed(2)));
                MediaSolesProyectado2 = UtilidadMesProyectado2 / CantidadDias / NroMaquina;
                $("#MediaSolesProyectado2").val(FormatoMillar(MediaSolesProyectado2.toFixed(2)));
                MediaSolesProyectado3 = UtilidadMesProyectado3 / CantidadDias / NroMaquina;
                $("#MediaSolesProyectado3").val(FormatoMillar(MediaSolesProyectado3.toFixed(2)));

                MediaDolares = MediaSoles / 3.25;
                $("#MediaDolares").val(FormatoMillar(MediaDolares.toFixed(2)));
                MediaDolaresProyectado1 = MediaSolesProyectado1 / 3.25;
                $("#MediaDolaresProyectado1").val(FormatoMillar(MediaDolaresProyectado1.toFixed(2)));
                MediaDolaresProyectado2 = MediaSolesProyectado2 / 3.25;
                $("#MediaDolaresProyectado2").val(FormatoMillar(MediaDolaresProyectado2.toFixed(2)));
                MediaDolaresProyectado3 = MediaSolesProyectado3 / 3.25;
                $("#MediaDolaresProyectado3").val(FormatoMillar(MediaDolaresProyectado3.toFixed(2)));


                Premios_Dia1 = TotalIncInferior / (avancePromedioInferior + AvancePromedioPozoOcultoInferior);
                $("#Premios_Dia1").val(FormatoMillar(Premios_Dia1.toFixed(2)));
                Premios_Dia2 = TotalIncMedio / (avancePromedioMedio + AvancePromedioPozoOcultoMedio);
                $("#Premios_Dia2").val(FormatoMillar(Premios_Dia2.toFixed(2)));
                Premios_Dia3 = TotalIncSuperior / (avancePromedioSuperior + AvancePromedioPozoOcultoSuperior);
                $("#Premios_Dia3").val(FormatoMillar(Premios_Dia3.toFixed(3)));


                Premios_Mes1 = Premios_Dia1 * CantidadDias;
                $("#Premios_Mes1").val(FormatoMillar(Premios_Mes1.toFixed(3)));
                Premios_Mes2 = Premios_Dia2 * CantidadDias;
                $("#Premios_Mes2").val(FormatoMillar(Premios_Mes2.toFixed(3)));
                Premios_Mes3 = Premios_Dia3 * CantidadDias;
                $("#Premios_Mes3").val(FormatoMillar(Premios_Mes3.toFixed(3)));

                DiasxPremio1 = 30 / Premios_Mes1;
                $("#DiasxPremio1").val(FormatoMillar(DiasxPremio1.toFixed(3)));
                DiasxPremio2 = 30 / Premios_Mes2;
                $("#DiasxPremio2").val(FormatoMillar(DiasxPremio2.toFixed(3)));
                DiasxPremio3 = 30 / Premios_Mes3;
                $("#DiasxPremio3").val(FormatoMillar(DiasxPremio3.toFixed(3)));

                PremioProm1 = promedio1;
                $("#PremioProm1").val(FormatoMillar(PremioProm1.toFixed(2)));
                PremioProm2 = promedio2;
                $("#PremioProm2").val(FormatoMillar(PremioProm2.toFixed(2)));
                PremioProm3 = promedio3;
                $("#PremioProm3").val(FormatoMillar(PremioProm3.toFixed(2)));


                TotalPremios1 = PremioProm1 * Premios_Dia1 * 30;
                $("#TotalPremios1").val(FormatoMillar(TotalPremios1.toFixed(2)));
                TotalPremios2 = PremioProm2 * Premios_Dia2 * 30;
                $("#TotalPremios2").val(FormatoMillar(TotalPremios2.toFixed(2)));
                TotalPremios3 = PremioProm3 * Premios_Dia3 * 30;
                $("#TotalPremios3").val(FormatoMillar(TotalPremios3.toFixed(2)));

                PorcentajePremioxPozo1 = (TotalPremios1 / TotalPremios) * 100;
                $("#PorcentajePremioxPozo1").val(FormatoMillar(PorcentajePremioxPozo1.toFixed(0)) + "%");
                PorcentajePremioxPozo2 = (TotalPremios2 / TotalPremios) * 100;
                $("#PorcentajePremioxPozo2").val(FormatoMillar(PorcentajePremioxPozo2.toFixed(0)) + "%");
                PorcentajePremioxPozo3 = (TotalPremios3 / TotalPremios) * 100;
                $("#PorcentajePremioxPozo3").val(FormatoMillar(PorcentajePremioxPozo3.toFixed(0)) + "%");

                TotalPremios_ = TotalPremios1 + TotalPremios2 + TotalPremios3;
                $("#TotalPremios_").val(FormatoMillar(TotalPremios_.toFixed(2)));


                TotalPozoInferior = avancePromedioInferior * Premios_Dia1 * CantidadDias;
                $("#TotalPozoInferior").val(FormatoMillar(TotalPozoInferior.toFixed(2)));
                TotalPozoMedio = avancePromedioMedio * Premios_Dia2 * CantidadDias;
                $("#TotalPozoMedio").val(FormatoMillar(TotalPozoMedio.toFixed(2)));
                TotalPozoSuperior = avancePromedioSuperior * Premios_Dia3 * CantidadDias;
                $("#TotalPozoSuperior").val(FormatoMillar(TotalPozoSuperior.toFixed(2)));

                TotalAvancePromedioPozoOcultoInferior = AvancePromedioPozoOcultoInferior * Premios_Dia1 * CantidadDias;
                $("#TotalAvancePromedioPozoOcultoInferior").val(FormatoMillar(TotalAvancePromedioPozoOcultoInferior.toFixed(2)));
                TotalAvancePromedioPozoOcultoMedio = AvancePromedioPozoOcultoMedio * Premios_Dia2 * CantidadDias;
                $("#TotalAvancePromedioPozoOcultoMedio").val(FormatoMillar(TotalAvancePromedioPozoOcultoMedio.toFixed(2)));
                TotalAvancePromedioPozoOcultoSuperior = AvancePromedioPozoOcultoSuperior * Premios_Dia3 * CantidadDias;
                $("#TotalAvancePromedioPozoOcultoSuperior").val(FormatoMillar(TotalAvancePromedioPozoOcultoSuperior.toFixed(2)));

                IncPozoActualMes = IncPozoActualDia * 30;
                $("#IncPozoActualMes").val(FormatoMillar(IncPozoActualMes.toFixed(2)));
                IncPozoOcultoMes = IncPozoOcultoDia * 30;
                $("#IncPozoOcultoMes").val(FormatoMillar(IncPozoOcultoMes.toFixed(2)));

                PozoInferiorPremiosDiaProyectado1 = Premios_Dia1;
                $("#PozoInferiorPremiosDiaProyectado1").val(FormatoMillar(PozoInferiorPremiosDiaProyectado1.toFixed(2)));
                PozoMedioDiasPremioProyectado1 = DiasxPremio2;
                $("#PozoMedioDiasPremioProyectado1").val(FormatoMillar(PozoMedioDiasPremioProyectado1.toFixed(2)));
                PozoSuperiorDiasPremioProyectado1 = DiasxPremio3;
                $("#PozoSuperiorDiasPremioProyectado1").val(FormatoMillar(PozoSuperiorDiasPremioProyectado1.toFixed(2)));

                /*--Parametros-*/

                $("#PremioBasePozoInferiorParametro").val(data.PremioBasePozoInferior);
                $("#PremioBasePozoMedioParametro").val(FormatoMillar(data.PremioBasePozoMedio));
                $("#PremioBasePozoSuperiorParametro").val(data.PremioBasePozoSuperior);
                $("#PremioMinimoPozoInferiorParametro").val(data.PremioMinimoPozoInferior);
                $("#PremioMinimoPozoMedioParametro").val(data.PremioMinimoPozoMedio);
                $("#PremioMinimoPozoSuperiorParametro").val(data.PremioMinimoPozoSuperior);
                $("#PremioMaximoPozoInferiorParametro").val(data.PremioMaximoPozoInferior);
                $("#PremioMaximoPozoMedioParametro").val(data.PremioMaximoPozoMedio);
                $("#PremioMaximoPozoSuperiorParametro").val(data.PremioMaximoPozoSuperior);
                $("#IncremPozoPozoInferiorParametro").val(data.IncrementoPozoInferior);
                $("#IncremPozoPozoMedioParametro").val(data.IncrementoPozoMedio);
                $("#IncremPozoPozoSuperiorParametro").val(data.IncrementoPozoSuperior);
            }
        });

    });

    $(document).on('click', '#btnGuardar', function () {
        /*var Descripcion = $("#Descripcion").val();
        var NroMaquina = $("#NroMaquina").val().replace(".", ",");
        var TotalJugMes = $("#TotalJugMes").val().replace(".", ",");
        var TipoCambio = $("#TipoCambio").val().replace(".", ",");
        var Retencion = $("#Retencion").val().replace(".", ",");
        var PremioBasePozoInferior = $("#PremioBasePozoInferior").val().replace(".", ",");
        var PremioBasePozoMedio = $("#PremioBasePozoMedio").val().replace(".", ",");
        var PremioBasePozoSuperior = $("#PremioBasePozoSuperior").val().replace(".", ",");
        var PremioMinimoPozoInferior = $("#PremioMinimoPozoInferior").val().replace(".", ",");
        var PremioMinimoPozoMedio = $("#PremioMinimoPozoMedio").val().replace(".", ",");
        var PremioMinimoPozoSuperior = $("#PremioMinimoPozoSuperior").val().replace(".", ",");
        var PremioMaximoPozoInferior = $("#PremioMaximoPozoInferior").val().replace(".", ",");
        var PremioMaximoPozoMedio = $("#PremioMaximoPozoMedio").val().replace(".", ",");
        var PremioMaximoPozoSuperior = $("#PremioMaximoPozoSuperior").val().replace(".", ",");
        var IncrementoPozoInferior = $("#IncrementoPozoInferior").val().replace(".", ",");
        var IncrementoPozoMedio = $("#IncrementoPozoMedio").val().replace(".", ",");
        var IncrementoPozoSuperior = $("#IncrementoPozoSuperior").val().replace(".", ",");*/

        var Descripcion = $("#Descripcion").val();
        var NroMaquina = $("#NroMaquina").val();
        var TotalJugMes = $("#TotalJugMes").val();
        var TipoCambio = $("#TipoCambio").val();
        var Retencion = $("#Retencion").val();
        var PremioBasePozoInferior = $("#PremioBasePozoInferior").val();
        var PremioBasePozoMedio = $("#PremioBasePozoMedio").val();
        var PremioBasePozoSuperior = $("#PremioBasePozoSuperior").val();
        var PremioMinimoPozoInferior = $("#PremioMinimoPozoInferior").val();
        var PremioMinimoPozoMedio = $("#PremioMinimoPozoMedio").val();
        var PremioMinimoPozoSuperior = $("#PremioMinimoPozoSuperior").val();
        var PremioMaximoPozoInferior = $("#PremioMaximoPozoInferior").val();
        var PremioMaximoPozoMedio = $("#PremioMaximoPozoMedio").val();
        var PremioMaximoPozoSuperior = $("#PremioMaximoPozoSuperior").val();
        var IncrementoPozoInferior = $("#IncrementoPozoInferior").val();
        var IncrementoPozoMedio = $("#IncrementoPozoMedio").val();
        var IncrementoPozoSuperior = $("#IncrementoPozoSuperior").val();

        var dataForm = {
            Descripcion: Descripcion,
            NroMaquina: parseInt(NroMaquina),
            TotalJugMes: TotalJugMes,
            TipoCambio: TipoCambio,
            Retencion: Retencion,
            PremioBasePozoInferior: PremioBasePozoInferior,
            PremioBasePozoMedio: PremioBasePozoMedio,
            PremioBasePozoSuperior: PremioBasePozoSuperior,
            PremioMinimoPozoInferior: PremioMinimoPozoInferior,
            PremioMinimoPozoMedio: PremioMinimoPozoMedio,
            PremioMinimoPozoSuperior: PremioMinimoPozoSuperior,
            PremioMaximoPozoInferior: PremioMaximoPozoInferior,
            PremioMaximoPozoMedio: PremioMaximoPozoMedio,
            PremioMaximoPozoSuperior: PremioMaximoPozoSuperior,
            IncrementoPozoInferior: IncrementoPozoInferior,
            IncrementoPozoMedio: IncrementoPozoMedio,
            IncrementoPozoSuperior: IncrementoPozoSuperior
        };
        $.each(dataForm, function (key, value) {
            if (value == "") {
                toastr.warning('El campo ' + key + ' esta vacio', 'Mensaje Servidor');
                return false;
            }
        })
        var url = basePath + "ProyectadoProgresivo/ProyectadoProgresivoInsertarJson";
        ajaxhr = $.ajax({
            type: 'POST',
            url: url,
            data: JSON.stringify(dataForm),
            contentType: "application/json",
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                var respuesta = response.respuesta;
                if (respuesta) {
                    toastr.success('Se ha registrado Proyectado Progresivo', 'Mensaje Servidor');
                    $("#frmNuevo")[0].reset();
                    $("#full-modal").modal("hide");
                    /*$.when(llenarSelect(basePath + "ProyectadoProgresivo/ProyectadoProgresivoListarJson", {}, "cboCaso", "IdProyectadoProgresivo", "Descripcion", "")).then(function (response, textStatus) {
                        $("#cboCaso").select2();
                    });*/
                    getCases()
                } else {
                    toastr.warning(response.mensaje, 'Mensaje Servidor');
                }
            }
        });
        AbortRequest.open()
    });

    $(document).on('click', '#btnModificar', function () {
        /*var IdProyectadoProgresivo = $("#IdProyectadoProgresivo").val();
        var Descripcion = $("#DescripcionEditar").val();
        var NroMaquina = $("#NroMaquinaEditar").val().replace(".", ",");
        var TotalJugMes = $("#TotalJugMesEditar").val().replace(".", ",");
        var TipoCambio = $("#TipoCambioEditar").val().replace(".", ",");
        var Retencion = $("#RetencionEditar").val().replace(".", ",");
        var PremioBasePozoInferior = $("#PremioBasePozoInferiorEditar").val().replace(".", ",");
        var PremioBasePozoMedio = $("#PremioBasePozoMedioEditar").val().replace(".", ",");
        var PremioBasePozoSuperior = $("#PremioBasePozoSuperiorEditar").val().replace(".", ",");
        var PremioMinimoPozoInferior = $("#PremioMinimoPozoInferiorEditar").val().replace(".", ",");
        var PremioMinimoPozoMedio = $("#PremioMinimoPozoMedioEditar").val().replace(".", ",");
        var PremioMinimoPozoSuperior = $("#PremioMinimoPozoSuperiorEditar").val().replace(".", ",");
        var PremioMaximoPozoInferior = $("#PremioMaximoPozoInferiorEditar").val().replace(".", ",");
        var PremioMaximoPozoMedio = $("#PremioMaximoPozoMedioEditar").val().replace(".", ",");
        var PremioMaximoPozoSuperior = $("#PremioMaximoPozoSuperiorEditar").val().replace(".", ",");
        var IncrementoPozoInferior = $("#IncrementoPozoInferiorEditar").val().replace(".", ",");
        var IncrementoPozoMedio = $("#IncrementoPozoMedioEditar").val().replace(".", ",");
        var IncrementoPozoSuperior = $("#IncrementoPozoSuperiorEditar").val().replace(".", ",");*/

        var IdProyectadoProgresivo = $("#IdProyectadoProgresivo").val();
        var Descripcion = $("#DescripcionEditar").val();
        var NroMaquina = $("#NroMaquinaEditar").val();
        var TotalJugMes = $("#TotalJugMesEditar").val();
        var TipoCambio = $("#TipoCambioEditar").val();
        var Retencion = $("#RetencionEditar").val();
        var PremioBasePozoInferior = $("#PremioBasePozoInferiorEditar").val();
        var PremioBasePozoMedio = $("#PremioBasePozoMedioEditar").val();
        var PremioBasePozoSuperior = $("#PremioBasePozoSuperiorEditar").val();
        var PremioMinimoPozoInferior = $("#PremioMinimoPozoInferiorEditar").val();
        var PremioMinimoPozoMedio = $("#PremioMinimoPozoMedioEditar").val();
        var PremioMinimoPozoSuperior = $("#PremioMinimoPozoSuperiorEditar").val();
        var PremioMaximoPozoInferior = $("#PremioMaximoPozoInferiorEditar").val();
        var PremioMaximoPozoMedio = $("#PremioMaximoPozoMedioEditar").val();
        var PremioMaximoPozoSuperior = $("#PremioMaximoPozoSuperiorEditar").val();
        var IncrementoPozoInferior = $("#IncrementoPozoInferiorEditar").val();
        var IncrementoPozoMedio = $("#IncrementoPozoMedioEditar").val();
        var IncrementoPozoSuperior = $("#IncrementoPozoSuperiorEditar").val();

        var dataForm = {
            IdProyectadoProgresivo: IdProyectadoProgresivo,
            Descripcion: Descripcion,
            NroMaquina: parseInt(NroMaquina),
            TotalJugMes: TotalJugMes,
            TipoCambio: TipoCambio,
            Retencion: Retencion,
            PremioBasePozoInferior: PremioBasePozoInferior,
            PremioBasePozoMedio: PremioBasePozoMedio,
            PremioBasePozoSuperior: PremioBasePozoSuperior,
            PremioMinimoPozoInferior: PremioMinimoPozoInferior,
            PremioMinimoPozoMedio: PremioMinimoPozoMedio,
            PremioMinimoPozoSuperior: PremioMinimoPozoSuperior,
            PremioMaximoPozoInferior: PremioMaximoPozoInferior,
            PremioMaximoPozoMedio: PremioMaximoPozoMedio,
            PremioMaximoPozoSuperior: PremioMaximoPozoSuperior,
            IncrementoPozoInferior: IncrementoPozoInferior,
            IncrementoPozoMedio: IncrementoPozoMedio,
            IncrementoPozoSuperior: IncrementoPozoSuperior
        };

        $.each(dataForm, function (key, value) {
            if (value == "") {
                toastr.warning('El campo ' + key + ' esta vacio', 'Mensaje Servidor');
                return false;
            }
        })

        var url = basePath + "ProyectadoProgresivo/ProyectadoProgresivoEditarJson";
        ajaxhr = $.ajax({
            type: 'POST',
            url: url,
            data: JSON.stringify(dataForm),
            contentType: "application/json",
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                AbortRequest.close()
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                var respuesta = response.respuesta;
                if (respuesta) {
                    toastr.success('Se ha registrado Proyectado Progresivo', 'Mensaje Servidor');
                    $("#full-modal-edit").modal("hide");
                    /*$.when(llenarSelect(basePath + "ProyectadoProgresivo/ProyectadoProgresivoListarJson", {}, "cboCaso", "IdProyectadoProgresivo", "Descripcion", "")).then(function (response, textStatus) {
                        $("#cboCaso").select2();
                    });*/
                    getCases()
                    $("input").val("");
                } else {
                    toastr.warning(response.mensaje, 'Mensaje Servidor');
                }
            }
        });
        AbortRequest.open()
    });




    //////////////EXCEL

    $('#excel').on('click', function (e) {
        var accionboton = $(this).attr("id");//excel,pdf,imprimir


        //var tipo = $("#txtTipo option:selected").text();
        var CASO = $("#cboCaso option:selected").text();
        tituloreporte = "Simulador Proyectado Progresivo";
        tituloarchivo = "SimuladorProyectadoProgresivo_" + moment().format("DD_MM_YYYY_HH_mm");
        titulo_subtitulo = "";
        cabecerasdefecto = true;
        multiplestablas = true;
        mostrar_headers_tabla = false;
        ocultar = [];
        porcentajes = [];
        cabecerasnuevas = [];
        //cabecerasnuevas.push({ nombre: "Caso", valor: CASO });

        ARRAY_TABLAS = [];
        ////tblEncabezado
        ARRAY_DATOS = generar_arrayjson_tabla("tblEncabezado");
        definicioncolumnas = [];
        definicioncolumnas.push({ nombre: "Col0", tipo: "string", alinear: "left" });
        definicioncolumnas.push({ nombre: "Col1", tipo: "string", alinear: "right" });
        definicioncolumnas.push({ nombre: "Col2", tipo: "string", alinear: "left" });
        definicioncolumnas.push({ nombre: "Col3", tipo: "string", alinear: "right" });
        definicioncolumnas.push({ nombre: "Col4", tipo: "string", alinear: "left" });
        definicioncolumnas.push({ nombre: "Col5", tipo: "string", alinear: "right" });
        cabecerasnuevas = [];
        cabecerasnuevas.push({ nombre: "", valor: "PROYECTADO PROGRESIVO" });
        estilos_tabla = new EstilosReporte();
        objeto_tabla = {
            nombre: "CDSCDS",
            multiplestablas: false,
            definicioncolumnas: definicioncolumnas,
            cabecerasnuevas: cabecerasnuevas,
            datos: ARRAY_DATOS,
            mostrar_headers_tabla: false,
            estilos_reporte: estilos_tabla,
            porcentajes: []
        };
        ARRAY_TABLAS.push(objeto_tabla);
        //////fin tblEncabezado


        ////// tblParametros
        ARRAY_DATOS = generar_arrayjson_tabla("tblParametros");
        definicioncolumnas = [];
        definicioncolumnas.push({ nombre: "NOMBRE", tipo: "string", alinear: "left", colspan: 3 });
        definicioncolumnas.push({ nombre: "POZO INFERIOR", tipo: "string", alinear: "right" });
        definicioncolumnas.push({ nombre: "POZO MEDIO", tipo: "string", alinear: "right" });
        definicioncolumnas.push({ nombre: "POZO SUPERIOR", tipo: "string", alinear: "right" });
        cabecerasnuevas = [];
        cabecerasnuevas.push({ nombre: "SIMULADOR", valor: "PARÁMETROS" });
        estilos_tabla = new EstilosReporte();
        objeto_tabla = {
            nombre: "CDSCDS",
            multiplestablas: false,
            definicioncolumnas: definicioncolumnas,
            cabeceras: cabecerasnuevas,
            datos: ARRAY_DATOS,
            estilos_reporte: estilos_tabla,
            porcentajes: []

        };
        ARRAY_TABLAS.push(objeto_tabla);
        //////fin tblParametros


        ////// tblPremiosPorPozo
        ARRAY_DATOS = generar_arrayjson_tabla("tblPremiosPorPozo");
        definicioncolumnas = [];

        cabecerasnuevas = [];
        cabecerasnuevas.push({ nombre: "SIMULADOR", valor: "PREMIOS POR POZO" });
        estilos_tabla = new EstilosReporte();
        estilos_tabla.tabla_datos_body_alinear = "right";

        objeto_tabla = {
            nombre: "CDSCDS",
            multiplestablas: false,
            definicioncolumnas: definicioncolumnas,
            cabeceras: cabecerasnuevas,
            datos: ARRAY_DATOS,
            estilos_reporte: estilos_tabla,
            porcentajes: []

        };
        ARRAY_TABLAS.push(objeto_tabla);
        //////fin tblPremiosPorPozo

        ARRAY_DATOS = generar_arrayjson_tabla("tblProyectado");
        definicioncolumnas = [];
        definicioncolumnas.push({ nombre: "FECHA", tipo: "string", alinear: "left", colspan: 2 });
        cabecerasnuevas = [];
        cabecerasnuevas.push({ nombre: "", valor: "PROYECTADO" });
        estilos_tabla = new EstilosReporte();
        estilos_tabla.tabla_datos_body_alinear = "right";
        objeto_tabla = {
            nombre: "CDSCDS",
            multiplestablas: false,
            definicioncolumnas: definicioncolumnas,
            cabeceras: cabecerasnuevas,
            datos: ARRAY_DATOS,
            estilos_reporte: estilos_tabla,

        };
        ARRAY_TABLAS.push(objeto_tabla);

        /////GENERAR REPORTE
        cabecerasnuevas = [];
        cabecerasnuevas.push({ nombre: "Caso", valor: CASO });
        definicioncolumnas = [];

        funcion_botones([
            {
                estilos_tabla: estilos_tabla,
                accionboton: accionboton,
                usardatatable: false,
                multiplestablas: multiplestablas,
                datos: ARRAY_TABLAS,
                tituloreporte: tituloreporte,
                titulo_subtitulo: titulo_subtitulo,
                tituloarchivo: tituloarchivo,
                cabecerasnuevas: cabecerasnuevas,
                definicioncolumnas: definicioncolumnas,
                porcentajespdf: [],
                nombrehoja: "SIMULADORPROYECTADOPROGRESIVO",
                ocultar: []       ///array nombre de columnas para no mostrar 
            }
        ]
        );


    })
    ////////////FIN EXCEL


});

$("#Editar")
    .bootstrapValidator({
        container: '#messages',
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            NroMaquina: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            }
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

$.fn.serializeFrmJSON = function () {

    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
}

function FormatoMillar(x) {
    var valor = x.toString();
    valor = valor.replace(",", ".");
    var parts = valor.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}
function generar_arrayjson_tabla(tabla_id) {
    ARRAY_DATOS = [];
    array_cabezas = $("#" + tabla_id + " thead tr:last th");
    var ths = [];
    $(array_cabezas).each(function (i, e) {
        var colspan_th = $(e).attr("colspan") || 1;
        if (colspan_th == 1) {
            var valor = $(e).text() || "Col" + i;
            ths.push(valor);
        }
        else {
            for (iii = 0; iii < colspan_th; iii++) {
                var valor = "Col" + iii;
                ths.push(valor);
            }
        }
    })
    $("#" + tabla_id + " tbody tr").each(function (i, e) {
        var tds = $("td", e);
        objeto = {};
        columna = 0;
        $(tds).each(function (ii, ee) {
            var td = ee;
            var colspan = $(td).attr("colspan") || 1;

            if (colspan > 1) {
                for (iii = 0; iii < colspan; iii++) {
                    objeto[ths[columna]] = "";
                    columna++;
                }
            } else {
                var input = $("input", ee);
                if (input.length > 0) {
                    input_valor = $("input", ee).val();
                } else {
                    input_valor = $(ee).text();
                }
                objeto[ths[columna]] = input_valor;
                columna++;
            }

        });
        ARRAY_DATOS.push(objeto);
    })
    return ARRAY_DATOS;
}
let itemsImprimir=[]
let reimpresion=false
let ipPrivada
let ipPublicaAlterna
let seEncontroAlterna=false
let urlsResultado=[]
let delay=60000
let timerId=''
let consultaPorVpn = false
let tienePermisoModificarMensajeWhatsAppCampania = false;
let tienePermisoModificarNumeroCelularCliente = false;
let tienePermisoReenviarMensajeConCodigoPromocional = false;
let tienePermisoCanjearCodigoPromocional = false;
let tienePermisoVerClientesDeCampania = false;
let tienePermisoGenerarExcelClientesCampaniaWhatsApp = false;

$(document).ready(function () {
    clearTimeout(timerId)
    timerId = setTimeout(function request() {
        getPingSalas().then(result => {
            urlsResultado = result
        })
        timerId = setTimeout(request, delay);
    }, delay)


    obtenerListaSalas().then(result => {
        if (result.data) {
            renderSelectSalas(result.data)
            getPingSalas().then(response => {
                urlsResultado = response
            })
        }
    })

    //ObtenerListaSalas();
    ObtenerListaSalasRegistro();
    setCookie("datainicial", "");

    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        //maxDate: dateNow,
    });

    $(".dateOnly_fechaini").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        //minDate: dateMin,
    });

    $(".dateOnly_fechafin").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        //maxDate: dateNow,
    });
    $("#form_whatsapp").hide()
    $("#codigoRegeneracionForm").hide()
    $("#cboTipoCampania").on("change", function () {
        const tipoCampania = $("#cboTipoCampania").val();
        if (tipoCampania == 2) {
            $('#codigoSeReactiva').prop('checked', false);
            $("#codigoRegeneracionForm").hide()
            $("#duracionCodigoHoras").val(0)
            $("#duracionCodigoDias").val(0)
            $("#form_whatsapp").show()
        }
        else {
            $("#form_whatsapp").hide()
        }
    })
    $('#full-modal_campania').on('hidden.bs.modal', function () {
        $('#cboTipoCampania').val('').trigger('change');
        if ($("#cboTipoCampania").val() != 2) {
            $('#codigoSeReactiva').prop('checked', false);
            $("#codigoRegeneracionForm").hide()
            $("#form_whatsapp").hide()
        }
    });
    $('#codigoSeReactiva').change(function () {
        
        $("#duracionReactivacionCodigoDias").val(0)
        $("#duracionReactivacionCodigoHoras").val(0)

        if ($(this).is(':checked')) {
            $("#codigoRegeneracionForm").show()
        } else {
            $("#codigoRegeneracionForm").hide()
        }
    });


    $("#btnBuscar").on("click", async function () {
        if ($("#cboSala").val() == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        if ($("#fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if ($("#fechaFin").val() == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }
        await buscarCampanias();
    });

    $("#btnNuevo").on("click", function () {
        

        $('.dateOnly_fechaini').data("DateTimePicker").destroy();
        $('.dateOnly_fechafin').data("DateTimePicker").destroy();
        fechaMin = moment(new Date()).format('YYYY-MM-DD');
        var dateMin = new Date(fechaMin);
        dateMin.setDate(dateMin.getDate() + 1);
        $(".dateOnly_fechaini").datetimepicker({
            pickTime: false,
            format: 'DD/MM/YYYY',
            defaultDate: dateNow,
            minDate: dateMin,
        });

        $(".dateOnly_fechafin").datetimepicker({
            pickTime: false,
            format: 'DD/MM/YYYY',
            defaultDate: dateNow,
            //maxDate: dateNow,
        });

        $(".dateOnly_fechaini").on("dp.change", function (e) {
            $('.dateOnly_fechafin').data("DateTimePicker").destroy();
            $('#dateOnly_fechaini').val(moment(e.date).format('DD/MM/YYYY'));
            var fechaMin = moment(e.date).format('YYYY/MM/DD');
            var dateMin = new Date(fechaMin);
            dateMin.setDate(dateMin.getDate() - 0);

            $(".dateOnly_fechafin").datetimepicker({
                pickTime: false,
                format: 'DD/MM/YYYY',
                showToday: true,
                minDate: dateMin,

                defaultDate: moment(e.date).format('DD/MM/YYYY')
            });
            $('#fechafin').data("DateTimePicker").setDate(moment(dateMin));
        });
        $("#textCampania").text("Nueva");
        $("#campaniaid").val(0);
        $("#cboEstado").val("1");
        $("#cboSalaReg").val(null).trigger("change");
        $("#nombre").val("");
        $("#descripcion").val("");
        $('#fechaini').data("DateTimePicker").setDate(moment())
        $('#fechafin').data("DateTimePicker").setDate(moment())
        $("#full-modal_campania").modal("show");
    });

    $(document).on("click", ".btnDetalle", function () {
        var id = $(this).data("id");
        $("#textCampania").text("Editar");
        ObtenerRegistro(id);
        $("#full-modal_campania").modal("show");
    });

    $('.btnGuardar').on('click', function (e) {
        //var dataForm2 = $('#form_registro_campania').serializeFormJSON();
        //console.log(dataForm2)
        //return
        $("#form_registro_campania").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_campania").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var id = $("#campaniaid").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            if (id != 0) {
                lugar = "Campania/CampaniaActualizarJson";
                accion = "ACTUALIZAR CAMPAÑA";
                urlenvio = basePath + "Campania/CampaniaActualizarJson";
            }
            else {
                lugar = "Campania/CampaniaGuardarJson";
                accion = "NUEVO CAMPAÑA";
                urlenvio = basePath + "Campania/CampaniaGuardarJson";
            }

            var dataForm = $('#form_registro_campania').serializeFormJSON();
            if ($('#codigoSeReactiva').is(':checked')) {
                dataForm.codigoSeReactiva = 1
            } else {
                dataForm.codigoSeReactiva = 0
            }
            $.when(dataAuditoria(1, "#form_registro_campania", 3, lugar, accion)).then(function (response, textStatus) {
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
                    },
                    success: function (response) {
                        if (response.respuesta) {
                            $('#id').val("0");
                            $('#nombre').val("");
                            $('#descripcion').val("");
                            $('#duracionCodigoHoras').val(0);
                            $('#duracionCodigoDias').val(0);
                            $('#duracionReactivacionCodigoHoras').val(0);
                            $('#duracionReactivacionCodigoDias').val(0);
                            $('#codigoSeReactiva').prop('checked', false);
                            $('#form_whatsapp').hide();
                            $('#codigoRegeneracionForm').hide();
                            $("#full-modal_campania").modal("hide");
                            $("#btnBuscar").click();
                            toastr.success("Campaña Guardada", "Mensaje Servidor");
                        }
                        else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrow) {
                        toastr.error("Error Servidor", "Mensaje Servidor");
                    }
                });
            });

        }

    });

    $(document).on("click", ".btnTickets", function () {
        var id = $(this).data("id");
        var tipo = $(this).data("tipo");
        if (tipo == 0 || tipo == 2) {
            var nombre = $(this).data("nombre");
            $("#nombre_campania").text(nombre);
            $(".btn_excelticket").data("id", id);
            ObtenerTickets(id);
            $("#full-modal_tickets").modal("show");
        } else {
            toastr.warning("Solo Tipo Promoción o WhatsApp", "Mensaje Servidor");
        }

    });

    //#region Validar Código Promocional
    const promotionalCode = $("#modalCanjearCodigoPromocional_codigoPromocional");
    const documentNumber = $("#modalCanjearCodigoPromocional_numeroDocumento");
    const cboDocumentType = $("#modalCanjearCodigoPromocional_cmbTtipoDocumento");
    const btnCanjearCodigoPromocional = $("#modalCanjearCodigoPromocional_btnCanjearCodigoPromocional");

    $(document).on("click", ".btnModalCodigoPromocional", function () {
        var idCampania = $(this).data("id");
        var nombreCampania = $(this).data("nombre");
        var nombreSala = $(this).data("nombresala");
        var codSala = $(this).data("idsala");
        var estado = $(this).data("estado");

        if (estado == 0) {
            toastr.error("Campaña Vencida", "Mensaje Servidor");
            return;
        }

        $("#modalCanjearCodigoPromocional_nombreSala").val(nombreSala);
        $("#modalCanjearCodigoPromocional_codSala").val(codSala);
        $("#modalCanjearCodigoPromocional_nombreCampania").val(nombreCampania);
        $("#modalCanjearCodigoPromocional_idCampania").val(idCampania);
        promotionalCode.val("");
        documentNumber.val("");
        $("#modalCanjearCodigoPromocional").modal("show");
    });

    $('#modalCanjearCodigoPromocional').on('shown.bs.modal', function (e) {
        promotionalCode.focus();
    });

    $(document).on('keyup', '#modalCanjearCodigoPromocional_codigoPromocional, #modalCanjearCodigoPromocional_numeroDocumento', function (e) {
        if (e.keyCode == 13) {
            btnCanjearCodigoPromocional.click();
        }
    });

    $(document).off("click", "#modalCanjearCodigoPromocional_btnCanjearCodigoPromocional");
    $(document).on("click", "#modalCanjearCodigoPromocional_btnCanjearCodigoPromocional", function () {
        let codigoPromocional = promotionalCode.val();
        let numeroDocumento = documentNumber.val();
        let idTipoDocumento = cboDocumentType.val();
        let codSala = $("#modalCanjearCodigoPromocional_codSala").val();

        if (!idTipoDocumento) {
            toastr.error("Seleccione un Tipo de Documento.");
            return;
        }

        if (!numeroDocumento) {
            toastr.error("Ingrese Número de Documento.");
            return;
        }

        if (!codigoPromocional) {
            toastr.error("Ingrese Código Promocional.");
            return;
        }

        canjearCodigoPormocional(codigoPromocional, numeroDocumento, codSala, idTipoDocumento);
    });

    const renderSelectDocumentType = (data) => {
        $.each(data, function (index, value) {
            cboDocumentType.append(`<option value="${value.Id}">${value.Nombre}</option>`)
        });
        cboDocumentType.select2({
            placeholder: "--Seleccione--", allowClear: true, minimumResultsForSearch: 5, dropdownParent: $('#modalCanjearCodigoPromocional')
        });
        cboDocumentType.val(null).trigger("change");
    }

    const canjearCodigoPormocional = (codigoPromocional, numeroDocumento, codSala, idTipoDocumento) => {
        let data = {
            promotionalCode: codigoPromocional,
            documentNumber: numeroDocumento,
            codSala,
            idDocumentType: idTipoDocumento
        };
        $.ajax({
            type: "POST",
            url: basePath + "CampaniaCliente/CanjearCodigoPromocional",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                let success = response.success;
                if (success) {
                    promotionalCode.val("");
                    documentNumber.val("");
                    cboDocumentType.val(null).trigger("change");;
                    toastr.success(response.displayMessage, "Mensaje Servidor");
                } else {
                    toastr.error(response.displayMessage, "Mensaje Servidor");
                }
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    }

    const obtenerTiposDocumentos = () => {
        return $.ajax({
            type: "POST",
            url: basePath + "AsistenciaCliente/GetListadoTipoDocumento",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.respuesta) {
                    renderSelectDocumentType(result.data);
                }
            }
        });
    }
    obtenerTiposDocumentos();
    //#endregion

    //#region Mensaje WhatsApp de Campaña
    const mensajeWhatsApp = $("#modalMensajeWhatsApp_mensajeWhatsApp");
    const mensajeWhatsAppReactivacion = $("#modalMensajeWhatsApp_mensajeWhatsAppReactivacion");
    const btnGuardarMensajeWhatsApp = $("#modalMensajeWhatsApp_btnGuardarMensajeWhatsApp");
    const codigoPaisMensajePrueba = $("#modalMensajeWhatsApp_codigoPaisMensajePrueba");
    const numeroCelularMensajePrueba = $("#modalMensajeWhatsApp_numeroCelularMensajePrueba");
    let btnWhatsApp;
    let codSalaWhatsApp;
    var contenidoInicialMensajeWhatsApp;
    var contenidoInicialMensajeWhatsAppReactivacion;

    function valideKeyNumber(evt) {
        var code = (evt.which) ? evt.which : evt.keyCode;
        if (code >= 48 && code <= 57 || code == 13) {
            return true;
        }
        return false;
    }

    $(document).on("click", ".btnModalMensajeWhatsApp", function () {
        let idCampania = $(this).data("id");
        let nombreCampania = $(this).data("nombre");
        let nombreSala = $(this).data("nombresala");
        codSalaWhatsApp = $(this).data("idsala");
        let estado = $(this).data("estado");
        let mensaje = $(this).data("mensaje-whatsapp");
        let mensajeReactivacion = $(this).data("mensaje-whatsapp-reactivacion");

        if (estado == 0) {
            toastr.error("Campaña Vencida", "Mensaje Servidor");
            return;
        }

        $("#modalMensajeWhatsApp_nombreSala").val(nombreSala);
        $("#modalMensajeWhatsApp_idCampania").val(idCampania);
        mensajeWhatsApp.val(mensaje);
        mensajeWhatsAppReactivacion.val(mensajeReactivacion);
        contenidoInicialMensajeWhatsApp = mensaje;
        contenidoInicialMensajeWhatsAppReactivacion = mensajeReactivacion;
        $("#modalMensajeWhatsApp").modal("show");
        btnWhatsApp = $(this);
    });

    $(document).off("click", "#modalMensajeWhatsApp_btnGuardarMensajeWhatsApp");
    $(document).on("click", "#modalMensajeWhatsApp_btnGuardarMensajeWhatsApp", function () {
        let mensaje = mensajeWhatsApp.val();
        let mensajeReactivacion = mensajeWhatsAppReactivacion.val();
        let idCampania = $("#modalMensajeWhatsApp_idCampania").val();

        if (!mensaje) {
            toastr.error("Ingrese un mensaje.");
            return;
        }

        guardarMensajeWhatsApp(mensaje, mensajeReactivacion, parseInt(idCampania));
    });

    $(document).off("click", "#modalMensajeWhatsApp_btnEnviarMensajePrueba");
    $(document).on("click", "#modalMensajeWhatsApp_btnEnviarMensajePrueba", function () {
        let codigoPais = codigoPaisMensajePrueba.val()
        let numeroCelular = numeroCelularMensajePrueba.val()
        let idCampania = $("#modalMensajeWhatsApp_idCampania").val();

        if (!codigoPais) {
            toastr.info("Ingrese Código de Pais!!!")
            return;
        }

        if (!numeroCelular) {
            toastr.info("Ingrese Número de Celular!!!")
            return;
        }

        let phoneNumber = `${codigoPais}${numeroCelular}`
        let message = mensajeWhatsApp.val();
        let messageReactivacion = mensajeWhatsAppReactivacion.val();

        if (!message) {
            toastr.info("Ingrese un Mensaje para probar!!!")
            return;
        }

        sendTestMessage(parseInt(idCampania), codSalaWhatsApp, message, messageReactivacion, phoneNumber);
    });

    const sendTestMessage = (idCampania,codSala, message, messageReactivacion, phoneNumber) => {
        let data = {
            codSala,
            message,
            messageReactivacion,
            idCampania,
            phoneNumber
        };

        $.confirm({
            title: 'Confirmación',
            content: `¿Seguro que desea enviar el mensaje de prueba al número ${phoneNumber}?`,
            confirmButton: 'Sí',
            cancelButton: 'No',
            confirm: function () {
                $.ajax({
                    url: basePath + "Campania/EnviarMensajeWhatsAppPrueba",
                    type: "POST",
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    beforeSend: function () {
                        $.LoadingOverlay("show");
                    },
                    complete: function () {
                        $.LoadingOverlay("hide");
                    },
                    success: function (response) {
                        if (response.success) {
                            showConfirmationOfReceiptOfMessage();
                            toastr.success(response.displayMessage);
                        } else {
                            toastr.error(response.displayMessage);
                        }
                    }
                })
            },
            cancel: function () {
                toastr.info("Se canceló el envío del mensaje de prueba.");
            }
        });
    }

    const showConfirmationOfReceiptOfMessage = () => {
        $.confirm({
            title: 'Confirmación',
            content: `¿Recibio el mensaje de la de la forma que esperaba?<p><b><i>*En caso no haya recibido el mensaje, informele al Administrador.</i></b></p>`,
            confirmButton: 'Sí',
            cancelButton: 'No',
            confirm: function () {
                contenidoInicialMensajeWhatsApp = mensajeWhatsApp.val();
                contenidoInicialMensajeWhatsAppReactivacion = mensajeWhatsAppReactivacion.val();
                toastr.success("Ahora ya puede guardar el mensaje")
                btnGuardarMensajeWhatsApp.prop('disabled', false);
            },
            cancel: function () {
                btnGuardarMensajeWhatsApp.prop('disabled', true);
                toastr.info("Revise el mensaje y pruebe nuevamente.");
            }
        });
    }

    const guardarMensajeWhatsApp = (mensaje,mensajeReactivacion, idCampania) => {
        let data = {
            mensajeWhatsApp: mensaje,
            mensajeWhatsAppReactivacion: mensajeReactivacion,
            idCampania
        };
                
        $.ajax({
            type: "POST",
            url: basePath + "Campania/ActualizarMensajeWhatsAppCampania",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                if (response.success) {
                    contenidoInicialMensajeWhatsApp = mensaje;
                    contenidoInicialMensajeWhatsAppReactivacion = mensajeReactivacion;
                    btnWhatsApp.data("mensaje-whatsapp", mensaje);
                    btnWhatsApp.data("mensaje-whatsapp-reactivacion", mensajeReactivacion);
                    btnGuardarMensajeWhatsApp.prop('disabled', true);
                    toastr.success(response.displayMessage, "Mensaje Servidor");
                } else {
                    toastr.error(response.displayMessage, "Mensaje Servidor");
                }
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    }

    $('#modalMensajeWhatsApp').on('show.bs.modal', function () {
        btnGuardarMensajeWhatsApp.prop('disabled', true);
        numeroCelularMensajePrueba.val("");
    });

    $('#modalMensajeWhatsApp').on('hidden.bs.modal', function () {
        mensajeWhatsApp.val("");
        btnGuardarMensajeWhatsApp.prop('disabled', true);
    });
        
    mensajeWhatsApp.on('input', function () {
        btnGuardarMensajeWhatsApp.prop('disabled', $(this).val().trim() !== contenidoInicialMensajeWhatsApp);
    });
    mensajeWhatsAppReactivacion.on('input', function () {
        btnGuardarMensajeWhatsApp.prop('disabled', $(this).val().trim() !== contenidoInicialMensajeWhatsAppReactivacion);
    });
    //#endregion

    //#region Clientes de Campaña de WhatsApp
    const modalClientesCampania = $("#modalClientesCampania");
    const modalEditarCelularCliente = $("#modalEditarCelularCliente");
    const tableClientesRegistrados = $("#modalClientesCampania_tableClientesRegistrados");
    const btnGenerarExcelClientesCampania = $("#modalClientesCampania_btnGenerarExcelClientesCampania");
    
    $(document).on("click", ".btnModalClientesCampania", function () {
        let idCampania = $(this).data("id");
        let nombreCampania = $(this).data("nombre");
        let nombreSala = $(this).data("nombresala");
        let codSala = $(this).data("idsala");

        $("#modalClientesCampania_nombreSala").text(nombreSala);
        $("#modalClientesCampania_codSala").val(codSala);
        $("#modalClientesCampania_nombreCampania").text(nombreCampania);
        $("#modalClientesCampania_idCampania").val(idCampania);
        modalClientesCampania.modal("show");
    });

    $(document).on("click", "#modalClientesCampania_btnGenerarExcelClientesCampania", function () {
        let idCampania = $("#modalClientesCampania_idCampania").val();
        generarExcelClientesCampaniaWhatsApp(idCampania);
    });

    modalClientesCampania.on('show.bs.modal', async function () {
        btnGenerarExcelClientesCampania.hide();
    });

    modalClientesCampania.on('shown.bs.modal', async function () {
        let idCampania = $("#modalClientesCampania_idCampania").val();
        await clientesPorIdCamapania(idCampania);
    });

    modalClientesCampania.on('hide.bs.modal', function () {
        let table = tableClientesRegistrados.DataTable();
        table.clear().draw();
    });

    const clientesPorIdCamapania = async (idCampania) => {
        $.ajax({
            type: "POST",
            url: basePath + "CampaniaCliente/ObtenerClientesDeCampaniaWhatsAppPorIdCampania",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({idCampania}),
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                let data = response.data;
                tienePermisoModificarNumeroCelularCliente = response.permisos.tienePermisoModificarNumeroCelularCliente;
                tienePermisoReenviarMensajeConCodigoPromocional = response.permisos.tienePermisoReenviarMensajeConCodigoPromocional;
                tienePermisoGenerarExcelClientesCampaniaWhatsApp = response.permisos.tienePermisoGenerarExcelClientesCampaniaWhatsApp;
                if (tienePermisoGenerarExcelClientesCampaniaWhatsApp) {
                    btnGenerarExcelClientesCampania.show()
                }
                tableClientesRegistrados.DataTable({
                    bDestroy: true,
                    bSort: true,
                    ordering: true,
                    scrollCollapse: true,
                    scrollX: false,
                    sScrollX: "100%",
                    paging: true,
                    autoWidth: true,
                    bAutoWidth: true,
                    bProcessing: true,
                    bDeferRender: true,
                    data: data,
                    aaSorting: [],
                    columns: [
                        { data: "NombreCompleto", title: "Cliente" },
                        {
                            data: "NroDoc", title: "Doc. Identidad",
                            render: function (value, type, row) {
                                let documentType = row.TipoDocumento;
                                let documentNumber = row.NroDoc;
                                return `${documentType} - ${documentNumber}`
                            }
                        },
                        {
                            data: "NumeroCelular", title: "Número Celular",
                            render: function (value, type, row) {
                                let countryCode = row.CodigoPais;
                                let phoneNumber = row.NumeroCelular;
                                return `${countryCode} - ${phoneNumber}`
                            }
                        },
                        {
                            data: "Codigo", title: "Código",
                            render: function (value, type, row) {
                                let label = row.CodigoCanjeado ? 'label-success' : 'label-danger';
                                return `<span class="label ${label}">${value}</span>`
                            }
                        },
                        {
                            data: "CodigoEnviado", title: "Cod. Enviado",
                            render: function (value) {
                                let label = value ? 'label-success' : 'label-danger';
                                let val = value ? 'Sí' : 'No';
                                return `<span class="label ${label}">${val}</span>`
                            }
                        },
                        {
                            data: null, title: "F. Generación",
                            render: {
                                _: function (value, type, oData, meta) {
                                    return moment(oData.FechaGeneracionCodigo).format('DD/MM/YYYY hh:mm:ss A');
                                },
                                sort: 'FechaGeneracionCodigo'
                            }
                        },
                        {
                            data: "FechaCanjeoCodigo", title: "F. Canjeo",
                            render: function (value) {
                                return value == '/Date(-6847786800000)/' ? '-----' : moment(value).format('DD/MM/YYYY hh:mm:ss A');
                            }
                        },
                        {
                            data: "FechaExpiracionCodigo", title: "F. Expiración",
                            render: function (value) {
                                return moment(value).format('DD/MM/YYYY hh:mm A');
                            }
                        },
                        {
                            data: "CodigoExpirado", title: "Cod. Regenerado",
                            render: function (value) {
                                let label = value ? 'label-success' : 'label-danger';
                                let val = value ? 'Sí' : 'No';
                                return `<span class="label ${label}">${val}</span>`
                            }
                        },
                        {
                            data: "ProcedenciaRegistro", title: "Procedencia Registro",
                            render: function (value) {
                                return value ? value : '-----';
                            }
                        },
                        {
                            data: "MontoRecargado", title: "Monto Recargado",
                            render: {
                                _: function (value, type, oData, meta) {
                                    return `S/${oData.MontoRecargadoStr}`;
                                },
                                sort: 'MontoRecargado'
                            }
                        },
                        {
                            data: "Nacionalidad", title: "Nacionalidad",
                            render: function (value) {
                                return value ? value : '-----';
                            }
                        },
                        {
                            data: null, title: "F. Nacimiento (edad)",
                            render: {
                                _: function (value, type, oData, meta) {
                                    return `${moment(oData.FechaNacimiento).format('DD/MM/YYYY')} (${oData.Edad})`;
                                },
                                sort: 'FechaNacimiento'
                            }
                        },
                        {
                            data: null, title: "Acción",
                            render: function (row, type, oData, y) {
                                let boton = ''
                                if (tienePermisoModificarNumeroCelularCliente) {
                                    boton += `<button type="button" class="btn btn-xs btn-warning btnEditPhoneNumberClient" data-row="${y.row}" data-id="${row.id}" data-codigo-canjeado="${row.CodigoCanjeado}" data-nombre-completo="${row.NombreCompleto}" data-codigo-pais="${row.CodigoPais}" data-numero-celular="${row.NumeroCelular}"><i class="glyphicon glyphicon-pencil"></i></button>`;
                                }
                                if (tienePermisoReenviarMensajeConCodigoPromocional) {
                                    boton += ` <button type="button" class="btn btn-xs btn-success btnResendPromotionalCodeWhatsApp" data-row="${y.row}" data-id="${row.id}" data-codigo-canjeado="${row.CodigoCanjeado}" data-nombre-completo="${row.NombreCompleto}"><i class="lab la-whatsapp"></i> Reenviar Código</button>`;
                                }
                                if (!boton) {
                                    boton = '<span class="label label-danger">Sin permisos</span>'
                                }
                                return boton;
                            }
                        }
                    ]
                });
            }
        });
    }

    const generarExcelClientesCampaniaWhatsApp = (idCampania) => {
        $.ajax({
            type: "POST",
            url: basePath + "CampaniaCliente/GenerarExcelClientesCampaniaWhatsApp",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ idCampania }),
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                let displayMessage = response.displayMessage;
                if (!response.success) {
                    toastr.error(displayMessage);
                    return;
                }
                toastr.success(displayMessage);
                let dataExcel = response.bytes
                let linkExcel = document.createElement('a')
                document.body.appendChild(linkExcel) //required in FF, optional for Chrome
                linkExcel.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + dataExcel
                linkExcel.download = `${response.fileInfo.FileName}.xlsx`
                linkExcel.click()
                linkExcel.remove()
            }
        });
    }

    $(document).on("click", ".btnResendPromotionalCodeWhatsApp", function () {
        let idCampaniaCliente = $(this).data('id');
        let codigoCanjeado = $(this).data('codigo-canjeado');
        let nombreCompleto = $(this).data('nombre-completo');
        let row = $(this).data('row');
        
        if (codigoCanjeado) {
            toastr.info(`Código promocional canjeado, no se puede reenviar el codigo promocional al cliente '${nombreCompleto}'.`)
            return;
        }
        showConfirmationOfReesendPromotionalCode(idCampaniaCliente, nombreCompleto, row)
    })

    const showConfirmationOfReesendPromotionalCode = (idCampaniaCliente, nombreCompleto, row) => {
        $.confirm({
            title: 'Confirmación',
            content: `¿Seguro que desea reenviar el código promocional al cliente ${nombreCompleto}?`,
            confirmButton: 'Sí',
            cancelButton: 'No',
            confirm: function () {
                reesendPromotionalCode(idCampaniaCliente, row)
            },
            cancel: function () {
                toastr.info(`Se canceló el reenvío del código promocional al cliente '${nombreCompleto}'.`);
            }
        });
    }

    const reesendPromotionalCode = (idCampaniaCliente, row) => {
        $.ajax({
            type: "POST",
            url: basePath + "CampaniaCliente/EnviarCodigoPromocionalWhatsApp",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ idCampaniaCliente }),
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                let success = response.success;
                let displayMessage = response.displayMessage;
                if (!success) {
                    toastr.error(displayMessage);
                    return;
                }
                toastr.success(displayMessage);
                let tableClients = tableClientesRegistrados.DataTable();
                let selectedRows = tableClients.rows({ search: 'applied' }).data();
                let selected = selectedRows.filter((item) => item.id == idCampaniaCliente)[0]
                selected.CodigoEnviado = true;
                tableClients.row(row).data(selected)
            }
        });
    }

    let inputCodigoPais = $("#modalEditarCelularCliente_codigoPais");
    let inputNumeroCelular = $("#modalEditarCelularCliente_numeroCelular");
    let hiddenIdCampaniaCliente = $("#modalEditarCelularCliente_idCampaniaCliente");
    let rowClient = {};

    $(document).on("click", ".btnEditPhoneNumberClient", function () {
        let idCampaniaCliente = $(this).data('id');
        let nombreCompleto = $(this).data('nombre-completo');
        let codigoCanjeado = $(this).data('codigo-canjeado');
        let codigoPais = $(this).data('codigo-pais');
        let numeroCelular = $(this).data('numero-celular');

        if (codigoCanjeado) {
            toastr.info(`Código promocional canjeado, no se puede modificar el número de celular al cliente '${nombreCompleto}'.`)
            return;
        }

        rowClient = $(this).data('row');

        $("#modalEditarCelularCliente_nombreCliente").val(nombreCompleto);
        $("#modalEditarCelularCliente_idCampaniaCliente").val(idCampaniaCliente);
        inputCodigoPais.val(codigoPais);
        inputNumeroCelular.val(numeroCelular);
        modalEditarCelularCliente.modal("show");
    });
    
    modalEditarCelularCliente.on('hide.bs.modal', function () {
        $("#modalEditarCelularCliente_nombreCliente").val('');
        $("#modalEditarCelularCliente_idCampaniaCliente").val('');
        inputCodigoPais.val('');
        inputNumeroCelular.val('');
        rowClient = {};
    });

    $(document).on("click", "#modalEditarCelularCliente_btnActualizarNumeroCelularCliente", function () {
        let codigoPais = inputCodigoPais.val();
        let numeroCelular = inputNumeroCelular.val();
        let idCampaniaCliente = hiddenIdCampaniaCliente.val();

        if (!numeroCelular) {
            toastr.info("Ingrese Número de Celular.")
            return;
        }
        if (!codigoPais) {
            toastr.info("Ingrese Código de País.")
            return;
        }
        if (numeroCelular.length < 9) {
            toastr.info("El Número de Celular no puede tener menos de 9 dígitos.")
            return;
        }

        updatePhoneNumberClient(idCampaniaCliente, codigoPais, numeroCelular);
    });

    const updatePhoneNumberClient = (idCampaniaCliente, codigoPais, numeroCelular) => {
        let data = {
            id: idCampaniaCliente,
            CodigoPais: codigoPais,
            NumeroCelular: numeroCelular
        }
        $.ajax({
            type: "POST",
            url: basePath + "CampaniaCliente/ActualizarNumeroCelularCliente",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                let success = response.success;
                let displayMessage = response.displayMessage;
                if (!success) {
                    toastr.error(displayMessage);
                    return;
                }
                toastr.success(displayMessage);
                let tableClients = tableClientesRegistrados.DataTable();
                let selectedRows = tableClients.rows({ search: 'applied' }).data();
                let selected = selectedRows.filter((item) => item.id == idCampaniaCliente)[0]
                selected.CodigoPais = codigoPais;
                selected.NumeroCelular = numeroCelular;
                tableClients.row(rowClient).data(selected)
                modalEditarCelularCliente.modal("hide");
            }
        });
    }
    //#endregion

    $(document).on("click", ".btnValidar", function () {
        var id = $(this).data("id");
        var tipo = $(this).data("tipo");
        $("#SalaId").val($(this).data("idsala"))
        if (tipo == 0 || tipo == 2) {
            var nombre = $(this).data("nombre");
            var nombresala = $(this).data("nombresala");
            var idsala = $(this).data("idsala");
            var estado = $(this).data("estado");
            if (estado == 0) {
                toastr.error("Campaña Vencida", "Mensaje Servidor");
                return false;
            }
            $("#cboSala_registro").val(nombresala);
            $("#sala_idcompania_id").val(idsala);
            $("#nombrecompania_id").val(nombre);
            $("#compania_id").val(id);
            $("#NroTicket").val("");
            $("#NroDocument").val("");
            $("#txt_id_cliente_validar").val("");
            $(".lblvalidar").hide();
            $(".lbl_validar_head").show();
            $("#full-modal_validar").modal("show");
        } else {
            toastr.warning("Solo Tipo Promoción o WhatsApp", "Mensaje Servidor");
        }
    });

    $('#full-modal_validar').on('shown.bs.modal', function (e) {
        $("#NroDocument").focus();
    });

    $(document).on('keyup', '#NroDocument', function (e) {
        if (e.keyCode == 13) {
            if ($("#NroDocument").val() == "") {
                toastr.error("Ingrese Nro. Documento");
                return false;
            }

            if ($("#NroDocument").val().length < 8) {
                toastr.error("Minimo 8 digitos");
                return false;
            }
            $(".btn_buscarvalidar").click();
            //$("#modal_clienteNuevoCampania_validar").modal("show");
        }

    });

    $(document).off("click", ".btn_buscarvalidar");
    $(document).on("click", ".btn_buscarvalidar", function () {

        var nrodoc = $("#NroDocument").val();
        if (nrodoc == "") {
            toastr.error("Ingrese Nro. Documento");
            return false;
        }
        if (nrodoc.length < 8) {
            toastr.error("Minimo 8 digitos");
            return false;
        }
        validarcliente(nrodoc);
        //verificar(id, codsala, nroticket);
    });
    /////////////////////////////////////////////////////validar ticket///////////////////////////////////////////////////////////////////////



    $(document).on('keyup', '#NroTicket', function (e) {

        var nroticket = $(this).val();
        if (e.keyCode == 13) {

            if ($("#NroTicket").val() == "") {
                toastr.error("Ingrese Nro. Ticket");
                return false;
            }
            $(".btnvalidaticket").click();
        }
    });
    $(document).off("click", ".btnvalidaticket");
    $(document).on("click", ".btnvalidaticket", function () {
        var id = $("#compania_id").val();
        var nroticket = $("#NroTicket").val();
        var codsala = $("#sala_idcompania_id").val();
        const nroDoc = $("#textdocument").val();
        if ($("#NroTicket").val() == "") {
            toastr.error("Ingrese Nro. Ticket");
            return false;
        }
        verificar(id, codsala, nroticket, nroDoc);
    });

    $(document).on("click", ".btneliminar", function () {
        var elemento = $(this);
        var id = $(this).data("id");
        var campaniaid = $(this).data("campaniaid");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar ticket de Campaña?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-danger',
            confirm: function () {
                $.when(dataAuditoria(1, "#form_registro_campania", 3, "Campania/QuitarTicketCampania", "QUITAR TICKET")).then(function (response, textStatus) {
                    $.ajax({
                        url: basePath + "Campania/QuitarTicketCampania",
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
                                ObtenerTickets(campaniaid);
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
                });

            },

            cancel: function () {
                //close
            },

        });



    });

    $(document).on("click", "#btnExcel", function () {
        var fechaini = $("#fechaInicio").val();
        var fechafin = $("#fechaFin").val();
        var sala = $("#cboSala").val();
        if (sala == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        var estado = $("#cboEstado_").val();
        //var listasala = [];
        //$("#cboSala option:selected").each(function () {
        //    listasala.push($(this).data("id"));
        //});
        var listasala = $("#cboSala").val();
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "campania/ReporteCampaniasDescargarExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ codsala: listasala, fechaini, fechafin, estado }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                dataAuditoria(1, "#formfiltro", 3, "campania/ReporteCampaniasDescargarExcelJson", "BOTON EXCEL");
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
            //error: function (request, status, error) {
            //    toastr.error("Error De Conexion, Servidor no Encontrado.");
            //},
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });

    $(document).on("click", ".btn_excelticket", function () {
        var campaniaid = $(this).data("id");

        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "campania/ReporteTicketCampaniasDescargarExcelJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ campaniaid }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                dataAuditoria(1, "#formfiltro", 3, "campania/ReporteTicketCampaniasDescargarExcelJson", "BOTON EXCEL");
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
            //error: function (request, status, error) {
            //    toastr.error("Error De Conexion, Servidor no Encontrado.");
            //},
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });

    $(document).on("click", "#btnSalalibre", function () {

        $("#full-modalsalaslibre").modal("show");
    });

    $('#full-modalsalaslibre').on('shown.bs.modal', function (e) {
        buscarSalasLibres();
    });

    $(document).on("change", ".selectestado", function () {
        var elemento = $(this);
        var id = $(this).data("id");
        var CodSala = $(this).data("codsala");
        $.when(dataAuditoria(1, "#form_registro_campania", 3, "Campania/SalaVerificacionTicketPermisoJson", "QUITAR PERMISO VERIFICACION TICKET SALA (LIBRE)")).then(function (response, textStatus) {
            $.ajax({
                url: basePath + "Campania/SalaVerificacionTicketPermisoJson",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ id, CodSala }),
                beforeSend: function () {
                    $.LoadingOverlay("show");
                },
                complete: function () {
                    $.LoadingOverlay("hide");
                },
                success: function (response) {
                    if (response.respuesta) {
                        elemento.data('id', response.respuestaConsulta);
                        toastr.success(response.mensaje, "Mensaje Servidor");
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    buscarSalasLibres();
                }
            });
        });
    });

    $(document).on("change", ".selectsesion", function () {
        var elemento = $(this);
        var id = $(this).data("id");
        var CodSala = $(this).data("codsala");
        $.when(dataAuditoria(1, "#form_registro_campaniasesion", 3, "Campania/SalaCuponAutomaticoPermisoJson", "QUITAR PERMISO GENERACION CUPONES AUTOMATICO POR JUEGO")).then(function (response, textStatus) {
            $.ajax({
                url: basePath + "Campania/SalaCuponAutomaticoPermisoJson",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ id, CodSala }),
                beforeSend: function () {
                    $.LoadingOverlay("show");
                },
                complete: function () {
                    $.LoadingOverlay("hide");
                },
                success: function (response) {
                    if (response.respuesta) {
                        elemento.data('id', response.respuestaConsulta);
                        toastr.success(response.mensaje, "Mensaje Servidor");
                    }
                    else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    buscarSalasLibres();
                }
            });
        });
    });


    VistaAuditoria("Campania/ListadoCampañas", "VISTA", 0, "", 3);

    $(document).on("click", ".btnCliente", function () {
        var id = $(this).data("id");
        var tipo = $(this).data("tipo");
        $("#campania_tipo_").val(tipo);
        var nombre = $(this).data("nombre");
        var sala_id = $(this).data("idsala");
        var UrlProgresivo = $(this).data("urlprogresivo");
        $("#companiacliente_id").val(id);
        $("#url_progresivo").val(UrlProgresivo);
        $("#txt_sala_cliente_campania").val(sala_id);
        $("#txtsala_idcliente").val(sala_id);
        $(".nombre_campaña_cliente").text(nombre);
        $("#nombre_campaña_cliente").text(nombre);

        if (tipo == 0 || tipo == 2) {
            $("#full-modal_seleccionarCLienteAccion").modal("hide");
            $("#full-modal_clienteRegistrados").modal("show");
        } else if (tipo == 1) {
            $("#full-modal_seleccionarCLienteAccion").modal("show");
        }
    });

    $(document).on("click", ".clienteAgregar", function () {
        $("#full-modal_seleccionarCLienteAccion").modal("hide");
        $("#full-modal_clientelistaAgregar").modal("show");
    });


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    $(document).on("click", ".clienteRegistrados", function () {
        $("#full-modal_seleccionarCLienteAccion").modal("hide");
        $("#full-modal_clienteRegistrados").modal("show");
    });

    $('#full-modal_clienteRegistrados').on('shown.bs.modal', function (e) {
        //$(".tablaclientesdiv").html('<table class="table table-bordered table-hover table-condensed" style="width:100%">'+
        //                        '<thead>'+
        //                            '<tr>'+
        //                                '<th>Cliente</th>'+
        //                                '<th>Nro Documento</th>'+
        //                                '<th>Telefono</th>'+
        //                                '<th>Fecha Nacimiento</th>'+
        //                            '</tr>'+
        //                        '</thead>'+
        //                        '<tbody><tr><td colspan="4"><div class="alert alert-danger"> Cliente(s) a Campaña</div></td></tr></tbody>'+
        //    '</table>');
        let tipo = $("#campania_tipo_").val();
        if (tipo == 0 || tipo == 2) {
            clientesRegistrados();
        } else {
            clientesSorteoRegistrados();
        }
    });

    $('#full-modal_clientelistaAgregar').on('shown.bs.modal', function (e) {
        $("#txt_buscarcliente").val("");
        $(".tablaclientesBusquedadiv").html('<div class="alert alert-danger">Seleccione Cliente(s) a Campaña</div>');
    });

    $(document).off("click", ".excelclienteregistrados");
    $(document).on("click", ".excelclienteregistrados", function () {
        var id = $("#companiacliente_id").val();
        var url = "";
        var accion = "";
        let tipo = $("#campania_tipo_").val();
        if (tipo == 0 || tipo == 2) {
            url = "CampaniaCliente/ReporteCampaniasDescargarExcelJson";
            accion = "BOTON EXCEL PROMOCION";
        } else {
            url = "CampaniaSorteo/ReporteCampaniasCuponesDescargarExcelJson";
            accion = "BOTON EXCEL SORTEO";
        }
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ campania_id: id }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                dataAuditoria(1, "#formfiltro", 3, url, accion);
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
            //error: function (request, status, error) {
            //    toastr.error("Error De Conexion, Servidor no Encontrado.");
            //},
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });

    $(document).off("click", ".btn_buscarclientecampania");
    $(document).on("click", ".btn_buscarclientecampania", function () {
        clientesusBusquedaRegistrados();
    });


    $(document).off("click", ".btnQuitarCliente");
    $(document).on("click", ".btnQuitarCliente", function () {
        var elemento = $(this);
        var id = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Quitar Cliente de Campaña?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {
                $.when(dataAuditoria(1, "#form_registro_campania", 3, "CampaniaCliente/QuitarClienteCampania", "QUITAR TICKET")).then(function (response, textStatus) {
                    $.ajax({
                        url: basePath + "CampaniaCliente/QuitarClienteCampania",
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
                                clientesRegistrados();
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
                });

            },

            cancel: function () {
                //close
            },

        });



    });

    $(document).off("click", ".btnAgregarCampaniaCliente");
    $(document).on("click", ".btnAgregarCampaniaCliente", function () {
        var elemento = $(this);
        var cliente_id = $(this).val();
        var campania_id = $(this).data("campaniaid");
        var cmp_clienteid = $(this).data("cmp_clienteid");
        var contenido = "";
        var url = "";
        var accion = "";
        if (!$(this).is(':checked')) {
            contenido = "¿Seguro de Quitar Cliente de Campaña?";
            url = "CampaniaCliente/QuitarClienteCampaniaJson";
            accion = "QUITAR CLIENTE";
        }
        else {
            url = "CampaniaCliente/GuardarClienteCampaniaJson";
            accion = "AGREGAR CLIENTE";
            contenido = "¿Seguro de Agregar Cliente a Campaña?";
        }

        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: contenido,
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {
                $.when(dataAuditoria(1, "#form_registro_campania_cliente", 3, url, accion)).then(function (response, textStatus) {
                    $.ajax({
                        url: basePath + "/" + url,
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({ cliente_id, campania_id, id: cmp_clienteid }),
                        beforeSend: function () {
                            $.LoadingOverlay("show");
                        },
                        complete: function () {
                            $.LoadingOverlay("hide");
                        },
                        success: function (response) {
                            if (response.respuesta) {

                                toastr.success(response.mensaje, "Mensaje Servidor");
                            }
                            else {
                                clientesusBusquedaRegistrados();
                                toastr.error(response.mensaje, "Mensaje Servidor");
                            }
                        },
                        error: function (xmlHttpRequest, textStatus, errorThrow) {
                            clientesusBusquedaRegistrados();
                        }
                    });
                });

            },

            cancel: function () {
                clientesusBusquedaRegistrados();
            },

        });



    });


    $(document).on("click", ".btn_buscarclienteFormnuevocampania", function () {
        $("#txtNombre").val("");
        $("#txtApelPat").val("");
        $("#txtApelMat").val("");
        $("#txtNroDoc").val("");
        $("#txtCelular1").val("");
        $('#txtFechaNacimiento').data("DateTimePicker").setDate(moment())
        $("#full-modal_clienteNuevoCampania").modal("show");

    });

    $('.btnGuardarclientenuevo').on('click', function (e) {
        $("#form_registro_cliente").data('bootstrapValidator').resetForm();
        var validar = $("#form_registro_cliente").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var sala_id = $("#campaniaidsalaid").val();
            var urlenvio = "";
            var lugar = "";
            var accion = "";
            lugar = "CampaniaCliente/CampaniaGuardarClienteJson";
            accion = "NUEVO CLIENTE";
            urlenvio = basePath + "CampaniaCliente/CampaniaGuardarClienteJson";

            var dataForm = $('#form_registro_cliente').serializeFormJSON();
            $.when(dataAuditoria(1, "#form_registro_cliente", 3, lugar, accion)).then(function (response, textStatus) {
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
                    },
                    success: function (response) {
                        if (response.respuesta) {
                            $('#txtNombre').val("");
                            $('#txtApelPat').val("");
                            $('#txtApelMat').val("");
                            $('#txtCelular1').val("");
                            $('#txtFechaNacimiento').data("DateTimePicker").setDate(moment())
                            $("#full-modal_clienteNuevoCampania").modal("hide");
                            $("#txt_buscarcliente").val($("#txtNroDoc").val());

                            $(".btn_buscarclientecampania").trigger("click")
                            toastr.success("Cliente Registrado", "Mensaje Servidor");
                        }
                        else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrow) {

                    }
                });
            });

        }

    });


    $("#btn_parametrosmodal").on("click", function (e) {
        $("#full-modal_parametros").modal("show");
        var urlenvio = "";
        var lugar = "";
        var accion = "";
        lugar = "CampaniaSorteo/CampaniaSorteoParametro";
        accion = "PARAMETROS SALA";
        urlenvio = basePath + "CampaniaSorteo/CampaniaSorteoParametro";

        $.when(dataAuditoria(1, "#form_campania_parametros", 3, lugar, accion)).then(function (response, textStatus) {
            $.ajax({
                url: urlenvio,
                type: "POST",
                contentType: "application/json",
                data: {},
                beforeSend: function () {
                    $.LoadingOverlay("show");
                },
                complete: function () {
                    $.LoadingOverlay("hide");
                },
                success: function (response) {
                    if (response.respuesta && response.dataPermiso) {
                        $("#div_nombresalaparametros").html(response.data.sala.Nombre);
                        $("#txt_tipo_condicion").val(response.data.campania.condicion_juego);
                        if (response.data.campania.condicion_tipo == 0) {
                            response.data.campania.condicion_tipo = "";
                        }
                        $("#cbo_tipo_condicion").val(response.data.campania.condicion_tipo);
                        $("#txt_tipo_condicion_sala").val(response.data.sala.CodSala);
                        $("#full-modal_parametros").modal("show");
                    }
                    else {
                        $("#full-modal_parametros").modal("hide");
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {

                }
            });
        });

    });

    $('#full-modal_parametros').on('shown.bs.modal', function (e) {
        $("#form_campania_parametros").data('bootstrapValidator').resetForm();
    });

    $(".btnGuardarcampania_parametros").on("click", function (e) {
        $("#form_campania_parametros").data('bootstrapValidator').resetForm();
        var id = $("#txt_tipo_condicion_sala").val();
        var validar = $("#form_campania_parametros").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            var urlenvio = "";
            var lugar = "";
            var accion = "";

            lugar = "CampaniaSorteo/Ingresar_CampaniaSorteoParametro";
            accion = "INGRESAR PARAMETRO SALA";
            urlenvio = basePath + "CampaniaSorteo/Ingresar_CampaniaSorteoParametro";

            var dataForm = $('#form_campania_parametros').serializeFormJSON();
            $.when(dataAuditoria(1, "#form_campania_parametros", 3, lugar, accion)).then(function (response, textStatus) {
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
                    },
                    success: function (response) {
                        if (response.respuesta) {
                            $("#div_nombresalaparametros").html("");
                            $("#txt_tipo_condicion").val(0);
                            $("#cbo_tipo_condicion").val("");
                            $("#txt_tipo_condicion_sala").val(0);
                            $("#full-modal_parametros").modal("hide");
                            toastr.success("Se Registro Correctamente", "Mensaje Servidor");
                        }
                        else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }
                    },
                    error: function (xmlHttpRequest, textStatus, errorThrow) {

                    }
                });
            });
        }




    });


    $(document).on('click', '.btnCuponesCliente', function (e) {
        var id = $("#companiacliente_id").val();
        var ClienteId = $(this).data("id");
        $("#txt_cliente_seleccionado").val(ClienteId);
        ObtenerListaImpresorasRegistro($("#txtsala_idcliente").val());
        $.ajax({
            url: basePath + "/AsistenciaCliente/GetClienteId",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ ClienteId }),
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                if (response.respuesta) {
                    var nombre = response.data.NombreCompleto == '' ? response.data.ApelPat + " " + response.data.ApelMat + " " + response.data.Nombre : response.data.NombreCompleto;
                    $("#txt_cliente_seleccionado").val(ClienteId);
                    $("#txt_cliente").val(nombre);
                    $("#txt_slot").val('');
                    $("#txt_win").val('');
                    $(".btngeneradodiv").show();
                    $(".diresultadocupones").hide();
                    $(".resultadocupones").hide();
                    $("#full-modal_cupones_registrar").modal("show");
                }
                else {

                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {

            }
        });

    });

    $('#full-modal_cupones_registrar').on('shown.bs.modal', function (e) {
        $(".spncoinout").text('');
        $(".spncantidad").text('');
        $(".ini").html('');
        $(".fin").html('');
        $(".todos").html('');
        if (MostrarWinCupones.toLowerCase() === 'true') {
            $(".MostrarWinCupones").show()
        }
        else {
            $(".MostrarWinCupones").hide()
        }
        $("#form_campania_cupones").data('bootstrapValidator').resetForm();
    });

    $(document).on('click', '.btnGenerarCupones', function (e) {
        var id = $("#companiacliente_id").val();
        $("#form_campania_cupones").data('bootstrapValidator').resetForm();
        var validar = $("#form_campania_cupones").data('bootstrapValidator').validate();
        let validado = false;
        if (MostrarWinCupones.toLowerCase() === 'true') {
            if ($("#txt_win").val() == '' || $("#txt_win").val() == 0 || isNaN($("#txt_win").val())) {
                validado = false
                toastr.warning('Debe ingresar un valor numerico en WIN', "Mensaje Servidor");
            }
            else {
                validado = validar.isValid()
            }
        }
        else {
            validado = validar.isValid()
        }

        if (validado) {
            var dataForm = $('#form_campania_cupones').serializeFormJSON();
            dataForm.CampaniaId = $("#companiacliente_id").val();
            $.ajax({
                url: basePath + "/CampaniaSorteo/InsertarCuponeGenerarV2",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ cupones: dataForm }),
                beforeSend: function () {
                    $.LoadingOverlay("show");
                },
                complete: function () {
                    $.LoadingOverlay("hide");
                },
                success: function (response) {
                    if (response.respuesta) {
                        toastr.success(response.mensaje, "Mensaje Servidor");
                        $(".spncoinout").text(response.data.condicion_juego);
                        $(".spncantidad").text(response.data.CantidadCupones);
                        $(".ini").html(response.data.serieini);
                        $(".fin").html(response.data.seriefin);
                        $(".todos").html('<div class="text-center"><b>Lista</b></div>' + response.data.lista);
                        $(".btngeneradodiv").hide();
                        if (MostrarWinCupones.toLowerCase() === 'true') {
                            $(".resultadocupones").show();

                            $(".diresultadocupones").show();
                        }
                        else {
                            $(".resultadocupones").hide();

                            $(".diresultadocupones").hide();
                        }

                        itemsImprimir = []
                        reimpresion = false
                        // ObtenerCuponGeneradoPorId(response.data.CgId);
                    }
                    else {

                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {

                }
            });
        }


    });

    $(document).on("click", ".btnCuponesdetalle", function () {
        ObtenerListaImpresorasRegistro($("#txtsala_idcliente").val())
        var idcupon = $(this).data("id");
        var nombre = $(this).data("nombre");
        let idsesion = $(this).data('sesionid')
        $(".nombre_cliente_cupon_detalle").text(nombre);
        $("#txtSesionId").val(idsesion);
        $("#full-modal_detallecupones").modal("show");

    });

    $('#full-modal_detallecupones').on('shown.bs.modal', function (e) {
        $(".tabladetallecuponesdiv").html('<table class="table table-bordered table-hover table-condensed">' +
            '<thead>' +
            '<tr>' +
            '<th>Maquina</th>' +
            '<th>Serie Ini</th>' +
            '<th>Serie Fin</th>' +
            '<th>Coin Out Ini</th>' +
            '<th>Coin Out Fin</th>' +
            '<th>Current Credit</th>' +
            '<th>Monto</th>' +
            '<th>Token</th>' +
            '<th>Cant. Cupones</th>' +
            '<th>Fecha Registro</th>' +
            '<th>Accion</th>' +
            '</tr>' +
            '</thead>' +
            '<tbody><tr><td colspan="11"><div class="alert alert-danger"> Detalle Cupones</div></td></tr></tbody>' +
            '</table>');
        detalleCuponesRegistrados();
    });

    $(document).on("click", ".btnCuponesClienteLista", function () {
        let id = $(this).data("id");
        let sala_id = $(this).data("sala_id");
        let cod_cont = $(this).data("codcont")
        var UrlProgresivoSala = $("#url_progresivo").val()
        // ObtenerListaImpresorasRegistro(sala_id);
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "CampaniaCupones/GetListadoCuponesGenerados",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ Cod_Cont: cod_cont, UrlProgresivoSala: UrlProgresivoSala }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {

                if (response.respuesta) {
                    $("#tbody_listcupones").html('<tr><td colspan="4">' +
                        '<div class="alert alert-danger">Sin Registros</div>' +
                        '</td>' +
                        '</tr>');
                    var tbody = "";
                    $.each(response.data, function (key, val) {
                        tbody += '<tr><td>' + val.DetGenId + '</td><td>' + val.Serie + '</td><td>' + val.CantidadImpresiones + '</td><td><button type="button" data-id="' + val.DetGenId + '" class="btn btn-danger btn-xs imprimircuponGeneradoIndividual"><span class="fa fa-print"></span></button></td></tr>'
                    });
                    if (response.data.length > 0) {
                        $("#tbody_listcupones").html(tbody);
                    }
                    $("#full-modal_cupones_lista").modal("show");
                    $("#imprimircuponesTodosInput").val(cod_cont)

                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            //error: function (request, status, error) {
            //    toastr.error("Error De Conexion, Servidor no Encontrado.");
            //},
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });


    $(document).on("click", ".imprimircuponesTodos", function (e) {
        e.preventDefault()

        // let cgid = $("#imprimircuponesTodosInput").val()
        let ImpresoraId = $("#cbo_impresora2").val()
        if (ImpresoraId == 0 || ImpresoraId == '') {
            toastr.error("Debe seleccionar Impresora a Usar")
            return false
        }
        let Cod_Cont = $("#imprimircuponesTodosInput").val()
        ReimprimirCuponPorCod_Cont(Cod_Cont, ImpresoraId)
        // let cgid= $("#txtSesionId").val()
        // if(cgid==0||cgid==''){
        //     toastr.error("Id de Cupon Incorrecto")
        //     return false
        // }
        // itemsImprimir=[]
        // let listaIndidivuales=$(".imprimircuponGeneradoIndividual")
        // $.each(listaIndidivuales, function (index,value) {
        //     itemsImprimir.push($(value).data("id"))
        // })      
        // if(itemsImprimir.length==0){
        //     toastr.error("No se encontraton tickets a imprimir")
        //     return false
        // }
        //reimpresion=true

        // ReimprimirCupon(cgid,ImpresoraId)
    });

    $(document).on("click", ".imprimircuponGeneradoIndividual", function (e) {
        e.preventDefault()
        let ImpresoraId = $("#cbo_impresora2").val()
        if (ImpresoraId == 0 || ImpresoraId == '') {
            toastr.error("Debe seleccionar Impresora a Usar")
            return false
        }
        let id = $(this).data("id")
        ReimprimirCuponPorDetGenId(id, ImpresoraId)
        // e.preventDefault()
        // let ImpresoraId=$("#cbo_impresora2").val()
        // if(ImpresoraId==0||ImpresoraId==''){
        //     toastr.error("Debe seleccionar Impresora a Usar")
        //     return false
        // }

        // let cgid= $("#txtSesionId").val()
        // if(cgid==0||cgid==''){
        //     toastr.error("Id de Cupon Incorrecto")
        //     return false
        // }
        // // let cgid = $("#imprimircuponesTodosInput").val()
        // let id=$(this).data("id")

        // itemsImprimir=[]
        // itemsImprimir.push(id)
        // if(itemsImprimir.length==0){
        //     toastr.error("No se encontraton tickets a imprimir")
        //     return false
        // }
        // reimpresion=true

        // ReimprimirCupon(cgid,ImpresoraId)
    });

    $(document).on('click', '#ImprimirCupones', function (e) {
        e.preventDefault()
        printJS({ printable: 'printArea', type: 'html' })
    })
    $(document).on("click", "#btnImprimirConsolidado", function (e) {
        let IdCupon = $("#txtSesionId").val()
        let UrlProgresivoSala = $("#url_progresivo").val()
        let dataForm = {
            SesionId: IdCupon,
            UrlProgresivoSala: UrlProgresivoSala
        }
        let url = basePath + "CampaniaSorteo/ImprimirConsolidado"
        let divPrintArea = $("#printArea")
        divPrintArea.empty()
        $.ajax({
            type: "POST",
            cache: false,
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                if (response.respuesta) {
                    let cuponGenerado = response.data
                    let span = `
                        <div style="width:100%;display:flex;justify-content:center;padding-right:0;margin-right:0;paddint-left:0;margin-left:0;padding-top:0px;margin-top:0px">
                            <div class="card" style="width: 18rem;">
                                <div class="card-body">
                                    <p class="card-text" style="text-align: center;font-size:12px;font-weight:bold;">${cuponGenerado.NombreSala}</h5>
                                    <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:12px;font-weight:bold;">Fecha: <span style="font-weight:normal">${moment(cuponGenerado.Fecha).format("DD-MM-YYYY")}</span></p>
                                    <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:12px;font-weight:bold;">Slot: <span style="font-weight:normal">${cuponGenerado.SlotId}</span></p>
                                    <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:12px;font-weight:bold;">Cliente: <span style="font-weight:normal">${cuponGenerado.NombreCliente}</span></p>
                                    <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:12px;font-weight:bold;">Nro. Documento: <span style="font-weight:normal">${cuponGenerado.DniCliente}</span></p>
                                    <p class="card-text" style="padding-top:0px;margin-top:0px;font-size:12px;font-weight:bold;">Cupones : <span style="font-weight:normal">${cuponGenerado.CantidadCupones}</span></p>
                                    <div style="text-align:right;padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;">
                                    <span style="font-size:8px;">${response.SesionId}</span>
                                </div> 
                                    <p style="text-align:center;padding-top:0px;margin-top:0px;opacity:0;">-----------------------------------------------------</p>
                                </div>
                            </div>
                        </div>`
                    divPrintArea.html(span)
                    $('#ImprimirCupones').trigger('click');
                }
                else {
                    toast.error(response.mensaje, "Mensaje Servidor")
                }
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    })
    $(document).on('change', '#cbo_impresora', function (e) {
        let ImpresoraId = $(this).val()
        localStorage.removeItem("ImpresoraSeleccionada")
        localStorage.setItem('ImpresoraSeleccionada', ImpresoraId)
    })
    $(document).on('click', '#btnImprimirTodo', function (e) {
        e.preventDefault()
        let ImpresoraId = $("#cbo_impresora3").val()
        if (ImpresoraId == 0 || ImpresoraId == '') {
            toastr.error("Debe seleccionar Impresora a Usar")
            return false
        }
        let cgid = $("#txtSesionId").val()
        ReimprimirCuponPorSesionId(cgid, ImpresoraId)
    })
});

function ObtenerRegistro(id) {
     
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Campania/CampaniaIDObtenerJson",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ id: id }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },

            success: function (response) {
                response = response.respuesta;
                
                fechaMin = moment(response.fechaini).format('YYYY-MM-DD');
                var dateMin = new Date(fechaMin);
                dateMin.setDate(dateMin.getDate() + 1);

                $('.dateOnly_fechaini').data("DateTimePicker").destroy();
                $(".dateOnly_fechaini").datetimepicker({
                    pickTime: false,
                    format: 'DD/MM/YYYY',
                    defaultDate: dateMin,
                    minDate: dateMin,
                });
                $('.dateOnly_fechafin').data("DateTimePicker").destroy();
                $(".dateOnly_fechafin").datetimepicker({
                    pickTime: false,
                    format: 'DD/MM/YYYY',
                    defaultDate: moment(response.fechafin).format('DD/MM/YYYY'),
                    minDate: dateMin,
                });

                $(".dateOnly_fechaini").on("dp.change", function (e) {
                    $('.dateOnly_fechafin').data("DateTimePicker").destroy();
                    $('#dateOnly_fechaini').val(moment(e.date).format('DD/MM/YYYY'));
                    var fechaMin = moment(e.date).format('YYYY/MM/DD');
                    var dateMin = new Date(fechaMin);
                    dateMin.setDate(dateMin.getDate() - 0);

                    $(".dateOnly_fechafin").datetimepicker({
                        pickTime: false,
                        format: 'DD/MM/YYYY',
                        showToday: true,
                        minDate: dateMin,
                        defaultDate: moment(e.date).format('DD/MM/YYYY')
                    });
                    $('#fechafin').data("DateTimePicker").setDate(moment(dateMin));
                });
                $("#cboEstado").val(response.estado);
                $("#campaniaid").val(response.id);
                $("#cboSalaReg").val(response.sala_id).trigger("change");
                $("#nombre").val(response.nombre);
                $("#descripcion").val(response.descripcion);
                $("#cboTipoCampania").val(response.tipo);
                $('#fechaini').data("DateTimePicker").setDate(moment(response.fechaini));
                $('#fechafin').data("DateTimePicker").setDate(moment(response.fechafin));

                if (response.tipo == 2) {
                    $("#duracionCodigoDias").val(response.duracionCodigoDias);
                    $("#duracionCodigoHoras").val(response.duracionCodigoHoras);
                    $("#form_whatsapp").show();
                    if (response.codigoSeReactiva) {
                        $("#duracionReactivacionCodigoDias").val(response.duracionReactivacionCodigoDias);
                        $("#duracionReactivacionCodigoHoras").val(response.duracionReactivacionCodigoHoras);
                        $('#codigoSeReactiva').prop('checked', response.codigoSeReactiva);
                        $("#codigoRegeneracionForm").show();
                    }
                } else {
                    $("#form_whatsapp").hide();
                    $("#codigoRegeneracionForm").hide();

                }
                


           
            },
            //error: function (request, status, error) {
            //    toastr.error("Error De Conexion, Servidor no Encontrado.");
            //},
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    }

// function ObtenerListaSalas() {
//     $.ajax({
//         type: "POST",
//         url: basePath + "Sala/ListadoSalaPorUsuarioJson",
//         cache: false,
//         contentType: "application/json; charset=utf-8",
//         dataType: "json",
//         beforeSend: function (xhr) {
//             $.LoadingOverlay("show");
//         },
//         success: function (result) {
//             var datos = result.data;

//             $.each(datos, function (index, value) {
//                 $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
//             });
//             $("#cboSala").select2({
//                 multiple: true, placeholder: "--Seleccione--", allowClear: true
//             });
//             $("#cboSala").val(null).trigger("change");
//         },
//         error: function (request, status, error) {
//             toastr.error("Error", "Mensaje Servidor");
//         },
//         complete: function (resul) {
//             $.LoadingOverlay("hide");
//         }
//     });
//     return false;
// }
function obtenerListaSalas() {
    return $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            return result;
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function renderSelectSalas(data){
    $.each(data, function (index, value) {
        $("#cboSala").append(`<option value="${value.CodSala}" data-urlprogresivo="${value.UrlProgresivo}" data-id="${value.CodSala}" data-ipprivada="${value.IpPrivada}" data-puertoservicio="${value.PuertoServicioWebOnline}">${value.Nombre}</option>`)
    });
    $("#cboSala").select2({
        multiple: true, placeholder: "--Seleccione--", allowClear: true
    });
    $("#cboSala").val(null).trigger("change");
}

function getPingSalas(){
    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath +"Progresivo/EchoPingSalasUsuario",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
        },
        success: function (response) {             
            return response
        },
        error: function (request, status, error) {            
        },
        complete: function (resul) {
        }
    });
}

function ObtenerListaSalasRegistro() {
    $("#cboSalaReg").html("");
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
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cboSalaReg").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSalaReg").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: true, dropdownParent: $("#full-modal_campania .modal-content")
            });
            $("#cboSalaReg").val(null).trigger("change");
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

async function buscarCampanias() {
    //var listasala = [];
    //$("#cboSala option:selected").each(function () {
    //    listasala.push($(this).data("id"));
    //});
    var listasala = $("#cboSala").val();

    //var sala = $("#cboSala option:selected").data("id");
    
    var fechaini = $("#fechaInicio").val();
    var fechafin = $("#fechaFin").val();
    var addtabla = $(".contenedor_tabla");
    var estado = $("#cboEstado_").val();
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Campania/ListarCampañasxFechaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin ,estado}),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            tienePermisoModificarMensajeWhatsAppCampania = response.permisos.tienePermisoModificarMensajeWhatsAppCampania;
            tienePermisoCanjearCodigoPromocional = response.permisos.tienePermisoCanjearCodigoPromocional;
            tienePermisoVerClientesDeCampania = response.permisos.tienePermisoVerClientesDeCampania;
            response = response.data;
            let oculto = 'hidden';
            dataAuditoria(1, "#formfiltro", 3, "Campaña/ListarCampañasxFechaJson", "BOTON BUSCAR");
            $(addtabla).empty();
            $(addtabla).append('<table id="campanias" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            var hidden = $.fn.dataTable.absoluteOrder([
                { value: oculto, position: 'top' }
            ]);
            objetodatatable = $("#campanias").DataTable({
                "bDestroy": true,
                "bSort": true,
                "ordering":true,
                "scrollCollapse": true,
                "scrollX": false,
                "sScrollX": "100%",
                "paging": true,
                "autoWidth": false,
                "bAutoWidth": true,
                "bProcessing": true,
                "bDeferRender": true,
                "aaSorting": [],
                data: response,
                orderFixed: {
                    'pre': [10, 'asc']
                },
                columnDefs: [{
                    targets: -1,
                    visible: false,
                    type: hidden
                }],
                columns: [

                    { data: "id", title: "ID" },
                    { data: "nombresala", title: "Sala" },
                    { data: "nombre", title: "Nombre" },
                    {
                        data: "fechareg", title: "Fecha Reg.",
                        "render": function (value) {
                            let span='<span style="display:none">'+value+'</span>'
                            return span+ moment(value).format('DD/MM/YYYY hh:mm:ss A');
                        }
                    },
                   
                    {
                        data: "fechaini", title: "Fecha Ini.",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY');
                        }
                    },
                    {
                        data: "fechafin", title: "Fecha Fin",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY');
                        }
                    },
                    {
                        data: "estado", title: "Estado",class:"tdcenter",
                        "render": function (value) {
                            var estado = "Vencida";
                            var css = "btn-danger";
                            if (value == 1) {
                                estado = "Activa"
                                css = "btn-success";
                            }
                            return '<span class="label '+css+'">'+estado+'</span>';
                        }
                    },
                    {
                        data: "tipo", title: "Tipo", class: "tdcenter",
                        "render": function (value) {
                            var estado = "Promocion";
                            var css = "btn-warning";
                            if (value == 1) {
                                estado = "Sorteo"
                                css = "btn-success";
                            }
                            if (value == 2) {
                                estado = "WhatsApp"
                                css = "btn-primary";
                            }
                            return '<span class="label ' + css + '">' + estado + '</span>';
                        }
                    },
                    { data: "usuarionombre", title: "Usu. Reg." },
                    {
                        data: null, title: "Accion",
                        "render": function (value) {
                            let tipo = value.tipo;
                            var butom = "";
                            //detalle
                            butom += `<button type="button" class="btn btn-xs btn-warning btnDetalle" title="DETALLE" data-id="${value.id}" data-json='${JSON.stringify(value)}'><i class="glyphicon glyphicon-pencil"></i>  Det.</button>`;
                            //codigo
                            if (tipo == 2 && tienePermisoCanjearCodigoPromocional) {
                                butom += ` <button type="button" class="btn btn-xs btn-info btnModalCodigoPromocional" title="VALIDAR CÓDIGO PROMOCIONAL" data-id="${value.id}" data-nombre="${value.nombre}" data-estado="${value.estado}" data-nombresala="${value.nombresala}" data-idsala="${value.sala_id}" data-tipo="${value.tipo}"><i class="glyphicon glyphicon-tag"></i> Código</button>`;
                            }
                            //validar
                            butom += ` <button type="button" class="btn btn-xs btn-primary btnValidar" title="VALIDAR TICKETS" data-id="${value.id}" data-nombre="${value.nombre}" data-estado="${value.estado}" data-nombresala="${value.nombresala}" data-idsala="${value.sala_id}" data-tipo="${value.tipo}"><i class="glyphicon glyphicon-edit"></i> Validar</button>`;
                            //clientes
                            if (tipo == 2 && tienePermisoVerClientesDeCampania) {
                                butom += ` <button type="button" class="btn btn-xs btn-dark btnModalClientesCampania" title="CLIENTES DE LA CAMPAÑA" data-id="${value.id}" data-nombre="${value.nombre}" data-estado="${value.estado}" data-nombresala="${value.nombresala}" data-idsala="${value.sala_id}" data-tipo="${value.tipo}"><i class="las la-users"></i> Clientes</button>`;
                            }
                            //tickets
                            butom += ` <button type="button" class="btn btn-xs btn-success btnTickets" title="TICKETS" data-id="${value.id}" data-nombre="${value.nombre}" data-tipo="${value.tipo}"><i class="glyphicon glyphicon-list-alt"></i>  Tickets</button>`;
                            //mensaje wasap
                            if (tipo == 2 && tienePermisoModificarMensajeWhatsAppCampania) {
                                butom += ` <button type="button" class="btn btn-xs btn-success btnModalMensajeWhatsApp" title="MENSAJE WHATSAPP" data-id="${value.id}" data-nombre="${value.nombre}" data-estado="${value.estado}" data-nombresala="${value.nombresala}" data-idsala="${value.sala_id}" data-tipo="${value.tipo}" data-mensaje-whatsapp="${value.mensajeWhatsApp}" data-mensaje-whatsapp-reactivacion="${value.mensajeWhatsAppReactivacion}"><i class="lab la-whatsapp"></i> Mensaje</button>`;
                            }
                            if (tipo != 2) {
                                butom += ` <button type="button" class="btn btn-xs btn-danger btnCliente" title="CLIENTE" data-id="${value.id}"  data-urlprogresivo="${value.UrlProgresivo}" data-nombre="${value.nombre}" data-estado="${value.estado}" data-nombresala="${value.nombresala}" data-idsala="${value.sala_id}" data-tipo="${value.tipo}"><i class="fa fa-user-plus"></i> </button>`;
                            }
                            return butom;
                        }
                    },
                    {
                        data: null, title: "Oculto",
                        render: function (row) {
                            let tipo = row.tipo;
                            return tipo == 2 ? oculto : '';
                        }
                    }
                ],
                "initComplete": function (settings, json) {},
                "drawCallback": function (settings) { },
                rowCallback: function (row, data) {
                    if (!data.mensajeWhatsApp && data.tipo == 2) {
                        $(row).css({
                            'text-decoration': 'line-through',
                            'color': 'red',
                        });
                        $(row).popover({
                            content: 'Tiene que configurar el mensaje de WhatsApp, haciendo click en el botón "Mensaje".',
                            trigger: 'hover',
                            placement: 'top',
                            placement: 'bottom',
                            container: 'body', // Muestra el popover fuera del contenedor del DataTable
                            boundary: 'viewport'
                        });
                    }
                }
            });
            
            $('#campanias tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function ObtenerTickets(id) {

    var instanciasCode = {
        1: 'Procedimiento Manual',
        2: 'Instancia por Auditoria'
    }

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Campania/CampaniaTicketsObtenerJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ campaniaid: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            tickets = response.respuesta;
            if (tickets.length == 0) {
                $("#tbodytickets").html('<tr><td colspan="8"><div class="alert alert-danger" style="margin:0px;">No se agregaron tickets</div></td></tr>');
            }
            else {
                $("#tbodytickets").html('');
            }
            $.each(tickets, function (index, value) {
                //$("#tbodytickets").append('<tr><td>' + moment(value.fecharegsala).format('DD/MM/YYYY hh:mm:ss A') + '</td><td>' + value.nroticket + '</td><td style="text-align:right">' + value.monto.toFixed(2) + '</td><td>' + value.NombreCompleto + '</td><td>' + value.NroDoc + '</td><td>' + value.Celular1 + '</td><td>' + value.nombre_usuario + '</td><td><button type="button" class="btn btn-xs btn-primary btneliminar" data-campaniaid="' + value.campaña_id + '" data-id="' + value.id + '" ><i class="glyphicon glyphicon-remove"></i> </button></td></tr>');
                $("#tbodytickets").append(`
                <tr>
                    <td>${moment(value.fecharegsala).format('DD/MM/YYYY hh:mm:ss A')}</td>
                    <td>${value.nroticket}</td>
                    <td style="text-align:right">${value.monto.toFixed(2)}</td>
                    <td>${instanciasCode[value.estado] ? instanciasCode[value.estado] : ''}</td>
                    <td>${value.NombreCompleto}</td>
                    <td>${value.NroDoc}</td>
                    <td>${value.Celular1}</td>
                    <td>${value.nombre_usuario}</td>
                    <td>
                        <button type="button" class="btn btn-xs btn-primary btneliminar" data-campaniaid="${value.campaña_id}" data-id="${value.id}">
                            <i class="glyphicon glyphicon-remove"></i>
                        </button>
                    </td>
                </tr>
                `)
            });
        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function verificar(id, codsala, nroticket, nroDoc) {
    let selectedOption=$('#cboSala option[value="'+codsala+'"]').data('urlprogresivo')
    const obj=urlsResultado.find(item=>item.uri==selectedOption)

    if (obj && obj.respuesta) {
        consultaPorVpn=false
    } else{
        consultaPorVpn=true
    }

    if(consultaPorVpn){
        let  uriConsulta = `${basePath}/Campania/BuscarTicketPromocionalVpn`
        ipPublicaAlterna = "http://190.187.44.222:9895"
        let dataForm={
            urlPublica:ipPublicaAlterna,
            urlPrivada:ipPrivada,
            codsala,
            nroticket,
            nroDoc,
            idCampania: id
        }
        $.ajax({
            type: "POST",
            cache: false,
            url: uriConsulta,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                respuesta = response.respuesta;
                var tickets = response.data;
                
                if (response.permiso) {
                    if (respuesta) {
                        $("#full-modal_validar").modal("hide");
                        var title = '¿Esta seguro de Continuar?!';
                        html = '<table style="font-size:12px;margin-bottom:0px;" class="table table-condensed table-hover table-bordered">' +
                            '<thead><tr><th>NroTicket</th><th style="width:20%">Monto</th></tr></thead><tbody><tr><td>' + nroticket + '</td>' +
                            '<td style="text-align:right"><div class="form-group" style="margin-bottom: 0px;">' +
                            '<input name="txtmonto" id="txtmonto" type="text" style="height: 25px;text-align: right;font-size: 15px;width: 75px;" />' +
                            ' </td></tr></tbody></table><div style="font-weight: bolder;">¿Validar Ticket a campaña : ' + $("#nombrecompania_id").val() + '? ...</div></div>';
                        $.confirm({
                            title: title,
                            content: html,
                            confirmButton: 'Ok',
                            cancelButton: 'Cerrar',
                            confirmButtonClass: 'btn-info',
                            cancelButtonClass: 'btn-danger',
                            confirm: function () {
                                tickets[0].codclie = $("#txt_id_cliente_validar").val();
                                tickets[0].Tito_NroTicket = nroticket;
                                tickets[0].Tito_MontoTicket = $("#txtmonto").val();
                                //tickets[0].Tito_fechaini = moment().format('DD/MM/YYYY hh:mm:ss A');
                                tickets[0].Tito_fechaini = moment().format('YYYY-MM-DDTHH:mm:ss.SSS');
                                tickets[0].Fecha_Apertura = moment().format('YYYY-MM-DDTHH:mm:ss.SSS');
                                agregarticket(tickets[0], id, codsala);
    
                            },
                            cancel: function () {
                                //close
                            }
                        });
                        dataAuditoria(1, "#formfiltro", 3, "Campania/BuscarTicketPromocional", "BOTON BUSCAR NROTICKET PROMOCIONAL");
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                } else {
                    if (respuesta) {
                        $("#full-modal_validar").modal("hide");
                        var cliente = "";
                        if (tickets[0].codclie > 0)
                        {
                            cliente = '<div style="font-weight: bolder;">Cliente : ' + tickets[0].Apeclie + ", " + tickets[0].NomClie + ' <br> Nro. Doc. : ' + tickets[0].Dni+'</div>';
                        }
                        var title = '¿Esta seguro de Continuar?!';
                        html = '<table style="font-size:9px;margin-bottom:0px;" class="table table-condensed table-hover table-bordered">' +
                            '<thead><tr><th>Fecha Reg.</th><th>NroTicket</th><th>Monto</th></tr></thead>' +
                            '<tbody><tr><td>' + moment(tickets[0].Tito_fechaini).format('DD/MM/YYYY hh:mm:ss A') + '</td>' +
                            '<td>' + tickets[0].Tito_NroTicket + '</td><td style="text-align:right">' + tickets[0].Tito_MontoTicket.toFixed(2) + '</td></tr>' +
                            '</tbody></table>'+cliente+'<div style="font-weight: bolder;">¿Validar Ticket a campaña : ' + $("#nombrecompania_id").val() + '? ...</div>';
                        $.confirm({
                            title: title,
                            content: html,
                            confirmButton: 'Ok',
                            cancelButton: 'Cerrar',
                            confirmButtonClass: 'btn-info',
                            cancelButtonClass: 'btn-danger',
                            confirm: function () {
                                tickets[0].codclie = $("#txt_id_cliente_validar").val();
                                //tickets[0].Tito_fechaini = moment(tickets[0].Tito_fechaini).format('DD/MM/YYYY hh:mm:ss A');
                                tickets[0].Tito_fechaini = moment(tickets[0].Tito_fechaini).format('YYYY-MM-DDTHH:mm:ss.SSS');
                                tickets[0].Fecha_Apertura = moment(tickets[0].Fecha_Apertura).format('YYYY-MM-DDTHH:mm:ss.SSS');
                                agregarticket(tickets[0], id, codsala);
                            },
                            cancel: function () {
                                //close
                            }
                        });
                        dataAuditoria(1, "#formfiltro", 3, "Campania/BuscarTicketPromocional", "BOTON BUSCAR NROTICKET PROMOCIONAL");
                    }
                    else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                }
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    } else{
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Campania/BuscarTicketPromocional",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ codsala, nroticket }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                respuesta = response.respuesta;
                var tickets = response.data;
                if (response.permiso) {
                    if (respuesta) {
                        $("#full-modal_validar").modal("hide");
                        var title = '¿Esta seguro de Continuar?!';
                        html = '<table style="font-size:12px;margin-bottom:0px;" class="table table-condensed table-hover table-bordered">' +
                            '<thead><tr><th>NroTicket</th><th style="width:20%">Monto</th></tr></thead><tbody><tr><td>' + nroticket + '</td>' +
                            '<td style="text-align:right"><div class="form-group" style="margin-bottom: 0px;">' +
                            '<input name="txtmonto" id="txtmonto" type="text" style="height: 25px;text-align: right;font-size: 15px;width: 75px;" />' +
                            ' </td></tr></tbody></table><div style="font-weight: bolder;">¿Validar Ticket a campaña : ' + $("#nombrecompania_id").val() + '? ...</div></div>';
                        $.confirm({
                            title: title,
                            content: html,
                            confirmButton: 'Ok',
                            cancelButton: 'Cerrar',
                            confirmButtonClass: 'btn-info',
                            cancelButtonClass: 'btn-danger',
                            confirm: function () {
                                tickets[0].codclie = $("#txt_id_cliente_validar").val();
                                tickets[0].Tito_NroTicket = nroticket;
                                tickets[0].Tito_MontoTicket = $("#txtmonto").val();
                                //tickets[0].Tito_fechaini = moment().format('DD/MM/YYYY hh:mm:ss A');
                                tickets[0].Tito_fechaini = moment().format('YYYY-MM-DDTHH:mm:ss.SSS');
                                tickets[0].Fecha_Apertura = moment().format('YYYY-MM-DDTHH:mm:ss.SSS');
                                agregarticket(tickets[0], id, codsala);
                            },
                            cancel: function () {
                                //close
                            }
                        });
                        dataAuditoria(1, "#formfiltro", 3, "Campania/BuscarTicketPromocional", "BOTON BUSCAR NROTICKET PROMOCIONAL");
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                }
                else {
                    if (respuesta) {
                        $("#full-modal_validar").modal("hide");
                        var cliente = "";
                        if (tickets[0].codclie > 0) {
                            //cliente = '<div style="font-weight: bolder;">Cliente : ' + tickets[0].Apeclie + ", " + tickets[0].NomClie + ' <br> Nro. Doc. : ' + tickets[0].Dni+'</div>';
                        }
                        var title = '¿Esta seguro de Continuar?!';
                        html = '<table style="font-size:9px;margin-bottom:0px;" class="table table-condensed table-hover table-bordered">' +
                            '<thead><tr><th>Fecha Reg.</th><th>NroTicket</th><th>Monto</th></tr></thead>' +
                            '<tbody><tr><td>' + moment(tickets[0].Tito_fechaini).format('DD/MM/YYYY hh:mm:ss A') + '</td>' +
                            '<td>' + tickets[0].Tito_NroTicket + '</td><td style="text-align:right">' + tickets[0].Tito_MontoTicket.toFixed(2) + '</td></tr>' +
                            '</tbody></table>'+cliente+'<div style="font-weight: bolder;">¿Validar Ticket a campaña : ' + $("#nombrecompania_id").val() + '? ...</div>';
                        $.confirm({
                            title: title,
                            content: html,
                            confirmButton: 'Ok',
                            cancelButton: 'Cerrar',
                            confirmButtonClass: 'btn-info',
                            cancelButtonClass: 'btn-danger',
                            confirm: function () {
                                tickets[0].codclie = $("#txt_id_cliente_validar").val();
                                //tickets[0].Tito_fechaini = moment(tickets[0].Tito_fechaini).format('DD/MM/YYYY hh:mm:ss A');
                                tickets[0].Tito_fechaini = moment(tickets[0].Tito_fechaini).format('YYYY-MM-DDTHH:mm:ss.SSS');
                                tickets[0].Fecha_Apertura = moment(tickets[0].Fecha_Apertura).format('YYYY-MM-DDTHH:mm:ss.SSS');
                                agregarticket(tickets[0], id, codsala);
                            },
                            cancel: function () {
                                //close
                            }
                        });
                        dataAuditoria(1, "#formfiltro", 3, "Campania/BuscarTicketPromocional", "BOTON BUSCAR NROTICKET PROMOCIONAL");
                    }
                    else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                }
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    }
}

function agregarticket(ticket,id,codSala) {
    $.when(dataAuditoria(1, "#form_registro_campania", 3, "Campania/CampaniaTicketGuardarJson", "AGREGAR TICKET A CAMPAÑA")).then(function (response, textStatus) {
        $.ajax({
            url: basePath + "Campania/CampaniaTicketGuardarJson",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ ticket,id, codSala }),
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                if (response.respuesta) {
                    $("#full-modal_validar").modal("hide");
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
    });
}
function buscarSalasLibres() {
    
    var addtabla = $("#salalibrediv");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Campania/ValidacionListadoSalas",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({  }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            response = response.data;
            $(addtabla).empty();
            $(addtabla).append('<table id="salalibre__" class="table table-condensed table-bordered table-hover table-responsive" style="width:100%"></table>');
            objetodatatable = $("#salalibre__").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "sScrollX": "100%",
                "paging": true,
                "autoWidth": false,
                "bAutoWidth": true,
                "bProcessing": true,
                "bDeferRender": true,
                data: response,
                columns: [

                    { data: "CodSala", title: "Sala" },
                    { data: "CodEmpresa", title: "Cod. Empresa" },
                    { data: "RazonSocial", title: "Empresa" },
                    { data: "nombresala", title: "Sala" },
                    {
                        data: null, title: "Sin Validar Ticket",
                        "render": function (value) {
                            var no = "";
                            var si = "";
                            if (value.id == 0) {
                                no = "selected";
                                si = "";
                            }
                            else {
                                si = "selected";
                                no = "";
                            }

                            var option ='<option value="0" '+no+'>No</option><option value="1" '+si+'>Si</option>'
                            var butom = "";
                            butom += `<select class="form-control input-xs select selectestado" id="sel_${value.CodSala}" data-id="${value.id}" data-codsala="${value.CodSala}" >${option}</select>`;
                            return butom;
                        }
                    },
                     {
                        data: null, title: "Generar Cupon Automatico",
                        "render": function (value) {
                            var no_ = "";
                            var si_ = "";
                            if (value.Salasesion_id == 0) {
                                no_ = "selected";
                                si_ = "";
                            }
                            else {
                                si_ = "selected";
                                no_ = "";
                            }

                            var option_ = '<option value="0" ' + no_ + '>No</option><option value="1" ' + si_ + '>Si</option>'

                            var butom = "";
                            butom += `<select class="form-control input-xs select selectsesion" id="selsesion_${value.CodSala}" data-id="${value.Salasesion_id}" data-codsala="${value.CodSala}" >${option_}</select>`;

                            return butom;
                        }
                    }
                ]
                ,
                "initComplete": function (settings, json) {



                },
                "drawCallback": function (settings) {

                }
            });

            $('#salalibre__ tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        error: function (request, status, error) {
            $("#full-modalsalaslibre").modal("hide");
            $("#btnSalalibre").hide();
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
function clientesRegistrados() {

    var addtabla = $(".tablaclientesdiv");
    var id = $("#companiacliente_id").val();
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CampaniaCliente/ClientesCampaniaIDObtenerJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ id: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            response = response.respuesta;
            $(".div_cargando_reg_lista").hide();
            dataAuditoria(1, "#formfiltro", 3, "CampaniaCliente/ClientesCampaniaIDObtenerJson", "LISTAR CLIENTES");
            $(addtabla).empty();
            $(addtabla).append('<table id="campaniaslistaclientes" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#campaniaslistaclientes").DataTable({
                "bDestroy": true,
                "bSort": true,
                "ordering": true,
                "scrollCollapse": true,
                "scrollX": false,
                "sScrollX": "100%",
                "paging": true,
                "autoWidth": false,
                "bAutoWidth": true,
                "bProcessing": true,
                "bDeferRender": true,
                data: response,
                columns: [

                    { data: "id", title: "ID" },

                    {
                        data: "NombreCompleto", title: "Cliente", render: function (x, r, y) {
                            var nombre = y.NombreCompleto == '' ? y.Nombre + ' ' + y.ApelPat + ' ' + y.ApelMat : y.NombreCompleto;
                            return nombre;
                        } },
                    { data: "NroDoc", title: "Nro. Documento" },
                    {
                        data: "FechaNacimiento", title: "Fecha Nac.",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY');
                        }
                    },
                    { data: "Mail", title: "Correo" },
                   
                    {
                        data: null, title: "Accion",
                        "render": function (value) {
                            var butom = "";
                            butom += `<button type="button" class="btn btn-xs btn-primary btnQuitarCliente" data-id="${value.id}" data-json='${JSON.stringify(value)}'><i class="fa fa-close"></i> </button>`;
                            return butom;
                        }
                    }
                ]
                ,
                "initComplete": function (settings, json) {



                },
                "drawCallback": function (settings) {

                }
            });

            $('#campaniaslistaclientes tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
function clientesSorteoRegistrados() {
    var id = $("#companiacliente_id").val();
    var UrlProgresivoSala = $("#url_progresivo").val();
    dataAuditoria(1, "#formfiltro", 3, "CampaniaCupones/GetListadoCuponesxCampania", "LISTAR CLIENTES CUPONES");
    $(".div_cargando_reg_lista").hide();
    var addtabla = $(".tablaclientesdiv");
    $(addtabla).empty();
    $(addtabla).append('<table id="campaniaslistaclientes" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
    objetodatatablecampania_listado_clientes = $("#campaniaslistaclientes").DataTable({
        "bDestroy": true,
        "bSort": true,
        "ordering": true,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": true,
        "aaSorting": [],
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        //data: response.data,
        "serverSide": true,
        "searching": { regex: true },
        "processing": true,
        "ajax": {
            url: basePath + "CampaniaCupones/GetListadoCuponesxCampaniaV3",
            type: "POST",
            data: function (data) {
                data.CampaniaId = id;
                data.UrlProgresivoSala = UrlProgresivoSala;
                return JSON.stringify(data);
            },
            dataFilter: function (response) {
                var datos = jQuery.parseJSON(response);
                return response;
            },
            dataType: "json",
            processData: false,
            contentType: "application/json;charset=UTF-8"
        },
        columns: [

            { data: "SesionId", title: "Id Sesion" },
            { data: "SlotId", title: "Maquina" },
            {
                data: "NombreCompleto", title: "Cliente", render: function (x, r, y) {
                    var nombre = (y.NombreCompleto == '') ? y.Nombre + ' ' + y.ApelPat + ' ' + y.ApelMat : y.NombreCompleto;
                    return nombre;
                }
            },
            { data: "NroDoc", title: "Nro. Doc." },
            // {
            //     data: "FechaNacimiento", title: "Fecha Nac.",
            //     "render": function (value) {
            //         return moment(value).format('DD/MM/YYYY');
            //     }
            // },
            { data: "CantidadCupones", title: "Cant. Cupones" },
            { data: "SerieIni", title: "Serie Ini." },
            { data: "SerieFin", title: "Serie Fin" },
            {
                data: "Fecha", title: "Fecha Reg.",
                "render": function (value, x, y) {
                    return moment(value).format('DD/MM/YYYY hh:mm A')
                }
            },

            { data: "Mail", title: "Correo" },
            {
                data: null, title: "Accion",
                "render": function (x,w,y) {
                    var butom = "";
                    var nombre = y.NombreCompleto == '' ? y.Nombre + ' ' + y.ApelPat + ' ' + y.ApelMat : y.NombreCompleto;
                    butom += `<button type="button" class="btn btn-xs btn-primary btnCuponesdetalle" data-sesionid=${y.SesionId} data-id="${y.CgId}" data-nombre="${nombre}"><i class="fa fa-building"></i> </button>`;
                    return butom;
                }
            }
        ],
        "drawCallback": function (settings) {
            $('.btnEditar').tooltip({
                title: "Editar"
            });
        },
        "fnDrawCallback": function (oSettings) {

        },
        "initComplete": function (settings, json) {

        },
    });
    $('#campaniaslistaclientes tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
    });
}
function detalleCuponesRegistrados() {

    var addtabla = $(".tabladetallecuponesdiv");
    var id = $("#txtSesionId").val();
    let UrlProgresivoSala = $("#url_progresivo").val();

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CampaniaCupones/GetListadoCuponesImpresos",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ SesionId: id,UrlProgresivo:UrlProgresivoSala}),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            response = response.data;
            // const responseFiltered=response.filter(item=>{
            //     return item.CantidadCuponesImpresos>0
            // })

            dataAuditoria(1, "#formfiltro", 3, "CampaniaCliente/GetListadoCuponesImpresos", "LISTAR CUPONES DETALLE");
            $(addtabla).empty();
            $(addtabla).append('<table id="cuponeslistaclientes" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#cuponeslistaclientes").DataTable({
                "bDestroy": true,
                "bSort": true,
                "ordering": true,
                "scrollCollapse": true,
                "scrollX": false,
                "sScrollX": "100%",
                "paging": true,
                "autoWidth": false,
                "bAutoWidth": true,
                "bProcessing": true,
                "bDeferRender": true,
                "aaSorting": [],
                data: response,
                columns: [

                    { data: "CodMaq", title: "Maquina" },
                    { data: "CodSala", title: "Cod Sala" },
                    { data: "SerieIni", title: "Serie Ini" },
                    { data: "SerieFin", title: "Serie Fin" },
                    {data:"CantidadCuponesImpresos",title:"Cantidad Cupones"},
                    { data: "CoinOutAnterior", title: "Coin Out Ini" },
                    { data: "CoinOut", title: "Coin Out Fin" },
                    { data: "HandPay", title: "HandPay" },
                    { data: "JackPot", title: "JackPot" },
                    { data: "CurrentCredits", title: "Current Credits" },
                    { data: "Monto", title: "Monto" },
                    { data: "Token", title: "Token" },
                    { data: "CoinOutIas", title: "Coin Out Parametro" },
                    {
                        data: "FechaRegistro", title: "Fecha Reg.",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY hh:mm:ss a');
                        }
                    },
                 
                    {
                        data: null, title: "Accion",
                        "render": function (value) {
                            var butom = "";
                            butom += `<button type="button" class="btn btn-xs btn-primary btnCuponesClienteLista" data-id="${value.DetImpId}" data-sala_id="${value.CodSala}" data-codcont="${value.Cod_Cont}"><i class="fa fa-building"></i> </button>`;
                          
                            return butom;
                        }
                    }
                ]
                ,
                "initComplete": function (settings, json) {



                },
                "drawCallback": function (settings) {

                }
            });

            $('#cuponeslistaclientes tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
function clientesusBusquedaRegistrados() {

    var addtabla = $(".tablaclientesBusquedadiv");
    var id = $("#companiacliente_id").val();
    var valor = $("#txt_buscarcliente").val();
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CampaniaCliente/ClientesCampaniaBusquedaObtenerJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ valor: valor }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            response = response.respuesta;
            dataAuditoria(1, "#formfiltro", 3, "CampaniaCliente/ClientesCampaniaBusquedaObtenerJson", "LISTAR CLIENTES");
            $(addtabla).empty();
            $(addtabla).append('<table id="campaniaslistaBusquedaclientes" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#campaniaslistaBusquedaclientes").DataTable({
                "bDestroy": true,
                "bSort": true,
                "ordering": true,
                "scrollCollapse": true,
                "scrollX": false,
                "sScrollX": "100%",
                "paging": true,
                "autoWidth": false,
                "bAutoWidth": true,
                "bProcessing": true,
                "bDeferRender": true,
                data: response,
                columns: [

                    { data: "id", title: "ID" },

                    {
                        data: "NombreCompleto", title: "Cliente", render: function (x,r,y) {
                            var nombre = y.NombreCompleto == '' ? y.Nombre + ' ' + y.ApelPat + ' ' + y.ApelMat : y.NombreCompleto;
                            return nombre;
                        }
                    },
                    { data: "NroDoc", title: "Nro. Documento" },
                    {
                        data: "FechaNacimiento", title: "Fecha Nac.",
                        "render": function (value) {
                            return moment(value).format('DD/MM/YYYY');
                        }
                    },
                    { data: "Mail", title: "Correo" },

                    {
                        data: null, title: '<i class="fa fa-cog"></i>',
                        "render": function (value) {
                            var cmpclienteid = value.CMPcliente_id;
                            var campania_id = value.campania_id;
                            var butom = "";
                            let tipo = $("#campania_tipo_").val();
                            if (tipo == 0 || tipo == 2) {
                                var checked = "";
                                if (cmpclienteid != 0 && campania_id == id) {
                                    checked = "checked";
                                }
                                
                                butom += `<input type="checkbox"id="chk_${value.id}" class="btn btn-xs btn-primary btnAgregarCampaniaCliente" data-campaniaid="${id}" data-cmp_clienteid="${cmpclienteid}"  value="${value.id}" ${checked}/> `;
                            }
                            else {
                                butom += `<button type="button" class="btn btn-xs btn-primary btnCuponesCliente" data-id="${value.id}"><i class="fa fa-building"></i> </button>`;
                            }

                            
                            return butom;
                        }
                    }
                ]
                ,
                "initComplete": function (settings, json) {



                },
                "drawCallback": function (settings) {

                }
            });

            $('#campaniaslistaBusquedaclientes tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
function ObtenerCuponGeneradoPorId(CgId){
    let divPrintArea=$("#printArea")
    divPrintArea.empty()
    if(CgId){
        let url=basePath;
        let dataForm;
        if(reimpresion){
            url+= "CampaniaSorteo/ReImprimirCuponeGenerado"
            dataForm={
                CgId:CgId,
                itemsImprimir:itemsImprimir
            }
        }
        else{
            url+="CampaniaCupones/ImprimirCuponGenerado"
            dataForm={
                CgId:CgId,
            }
        }
        $.ajax({
            type: "POST",
            cache: false,
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(dataForm),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
    
            success: function (response) {
                if(response.respuesta){
                    let cuponGenerado=response.data
                    let detalleCuponGenerado=cuponGenerado.DetalleCuponesGenerados
                    let span=''
                    let indice=0
                    let style=''
                    if(detalleCuponGenerado.length>0){
                        detalleCuponGenerado.map(item=>{
                       
                            if (indice != 0)
                            {
                                style = "page-break-before: always;";
                            }
                            indice++;
                            // span+=`
                            // <div style="width:100%;display:flex;justify-content:center;padding-right:0;margin-right:0;paddint-left:0;margin-left:0;">
                            //     <div class="card" style="width: 18rem;${style}">
                            //         <div class="card-body">
                            //             <p class="card-text" style="text-align: center; padding-bottom:0px;margin-bottom:0px;font-size:10px;">${item.RazonSocialEmpresa}</h5>
                            //             <p class="card-text" style="text-align: center;padding-top:0px;margin-top:0px;font-size:10px;">Ruc: ${item.RucEmpresa}</p>
                            //             <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;font-size:10px;">Sala: ${item.NombreSala}</p>
                            //             <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:10px;">Fecha: ${moment(item.Fecha).format("DD-MM-YYYY hh:mm:ss A")}</p>
                            //             <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:10px;">Serie: ${item.Serie}</p>
                            //             <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:10px;">Slot: ${cuponGenerado.SlotId}</p>
                            //             <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:10px;">Cliente: ${item.NombreCliente}</p>
                            //             <p class="card-text" style="padding-top:0px;margin-top:0px;font-size:10px;">Nro. Documento: ${item.DniCliente}</p>
                            //             <div style="display: flex;
                            //             justify-content: center; padding-bottom: 0px; margin-bottom: 0px;">
                            //                 <div id="barcodeTarget${item.DetGenId}"></div>
                            //             </div> 
                            //             <div style="text-align:right;padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;">
                            //                 <span style="font-size:6px;">${item.DetGenId}</span>
                            //             </div> 
                            //             <p style="text-align:center;padding-top:0px;margin-top:0px;opacity:0;">-----------------------------------------------------</p>
                            //         </div>
                            //     </div>
                            // </div> 
                            // `
                            span+=`
                            <div style="width:100%;display:flex;justify-content:center;padding-right:0;margin-right:0;paddint-left:0;margin-left:0;padding-top:0px;margin-top:0px;${style}">
                                <div class="card" style="width: 18rem;">
                                    <div class="card-body">
                                        <p class="card-text" style="text-align: center; padding-bottom:0px;margin-bottom:0px;font-size:12px;">${item.RazonSocialEmpresa}</h5>
                                        <p class="card-text" style="text-align: center;padding-top:0px;margin-top:0px;font-size:12px;">Ruc: ${item.RucEmpresa}</p>
                                        <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;font-size:12px;">Sala: <span style="font-weight:normal;">${item.NombreSala}</span></p>
                                        <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:12px;">Fecha: <span style="font-weight:normal">${moment(item.Fecha).format("DD-MM-YYYY")}</span></p>
                                        <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:12px;">Serie: <span style="font-weight:normal">${item.Serie}</span></p>
                                        <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:12px;">Slot: <span style="font-weight:normal">${cuponGenerado.SlotId}</span></p>
                                        <p class="card-text" style="padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;font-size:12px;">Cliente: <span style="font-weight:normal">${item.NombreCliente}</span></p>
                                        <p class="card-text" style="padding-top:0px;margin-top:0px;font-size:12px;">Nro. Documento: <span style="font-weight:normal">${item.DniCliente}</span></p>
                                        <div style="display: flex;
                                        justify-content: center; padding-bottom: 0px; margin-bottom: 0px;">
                                            <div style="width:100%" id="barcodeTarget${item.DetGenId}"></div>
                                        </div> 
                                        <div style="text-align:right;padding-bottom:0px;margin-bottom:0px;padding-top:0px;margin-top:0px;">
                                            <span style="font-size:8px;">${item.DetGenId}</span>
                                        </div> 
                                        <p style="text-align:center;padding-top:0px;margin-top:0px;opacity:0;">-----------------------------------------------------</p>
                                    </div>
                                </div>
                            </div> 
                            `
                          
                        })
                        divPrintArea.html(span)
                        //generar barcode
                        detalleCuponGenerado.map(item=>{
                            let barCodeOptions={
                                target:item.DetGenId,
                                value:item.Serie
                            }
                            generateBarcode(barCodeOptions)
                        })
                        $('#ImprimirCupones').trigger('click');
                    }
           
                }
                else{
                    toast.error(response.mensaje,"Mensaje Servidor")
                }
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    }
}
function generateBarcode(options){
    var settings = {
        output:'css',
        bgColor: '#FFFFFF',
        color: '#000000',
        barWidth: '1',
        barHeight: '50',
        moduleSize: '5',
        posX: '10',
        posY: '20',
        addQuietZone: '1'
    };
    var value = options.value;
    var btype = 'code128';
    $("#barcodeTarget"+options.target).html("").show().barcode(value, btype, settings);
    
}
function ObtenerListaImpresorasRegistro(codsala) {
    $("#cbo_impresora").html("");
    $("#cbo_impresora2").html("");
    $("#cbo_impresora3").html("")
    $.ajax({
        type: "POST",
        url: basePath + "CampaniaImpresora/ListarImpresorasxUsuarioidJson",
        cache: false,
        data: JSON.stringify({ codsala: codsala}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            
            var option = '<option value="">--Seleccione--</option>';
            $.each(datos, function (index, value) {
                option+='<option value="' + value.id + '"  >' + value.nombre + ' (<span style="font-size:6px">'+value.ip+'</span>)</option>';
            });
            $("#cbo_impresora").html(option);
            $("#cbo_impresora2").html(option);
            $("#cbo_impresora3").html(option)
            if(datos.length==1){
                $("#cbo_impresora").val(datos[0].id).trigger("change");
            }
            let ImpresoraSeleccionada=localStorage.getItem('ImpresoraSeleccionada')
            if(ImpresoraSeleccionada){
                $("#cbo_impresora").val(ImpresoraSeleccionada).trigger("change");
            }
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
function ReimprimirCupon(CgId, IdImpresora){
    let url=basePath
    let dataForm
    url+= "CampaniaSorteo/ReimprimirCupon"
    dataForm={
        CgId:CgId,
        itemsImprimir:itemsImprimir,
        IdImpresora:IdImpresora
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if(response.respuesta){
                toastr.success(response.mensaje,"Mensaje Servidor")
            }
            else{
                toastr.error(response.mensaje,"Mensaje Servidor")
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    })
}
function ReimprimirTodo(CgId, IdImpresora){
    let url=basePath
    let dataForm
    url+= "CampaniaSorteo/ReimprimirTodo"
    dataForm={
        CgId:CgId,
        IdImpresora:IdImpresora
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if(response.respuesta){
                toastr.success(response.mensaje,"Mensaje Servidor")
            }
            else{
                toastr.error(response.mensaje,"Mensaje Servidor")
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    })
}
function ReimprimirCuponPorDetGenId(DetGenId,IdImpresora){
    let url=basePath
    let dataForm
    // url+= "CampaniaSorteo/ReimprimirCuponPorDetGenId"
    url+= "CampaniaSorteo/ReimprimirCupon"
    let Tipo=1
    let UrlProgresivoSala = $("#url_progresivo").val()
    dataForm={
        IdImpresion:DetGenId,
        IdImpresora:IdImpresora,
        UrlProgresivoSala:UrlProgresivoSala,
        Tipo:Tipo
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if(response.respuesta){
                toastr.success(response.mensaje,"Mensaje Servidor")
            }
            else{
                toastr.error(response.mensaje,"Mensaje Servidor")
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    })
}
function ReimprimirCuponPorCod_Cont(Cod_Cont,IdImpresora){
    let url=basePath
    let dataForm
    // url+= "CampaniaSorteo/ReimprimirCuponPorCod_Cont"
    url+= "CampaniaSorteo/ReimprimirCupon"
    let Tipo=2
    let UrlProgresivoSala = $("#url_progresivo").val()
    dataForm={
        IdImpresion:Cod_Cont,
        IdImpresora:IdImpresora,
        UrlProgresivoSala:UrlProgresivoSala,
        Tipo:Tipo
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if(response.respuesta){
                toastr.success(response.mensaje,"Mensaje Servidor")
            }
            else{
                toastr.error(response.mensaje,"Mensaje Servidor")
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    })
}
function ReimprimirCuponPorSesionId(SesionId,IdImpresora){
    let url=basePath
    let dataForm
    // url+= "CampaniaSorteo/ReimprimirCuponPorSesionId"
    url+= "CampaniaSorteo/ReimprimirCupon"
    let UrlProgresivoSala = $("#url_progresivo").val()
    let Tipo=3
    dataForm={
        IdImpresion:SesionId,
        IdImpresora:IdImpresora,
        UrlProgresivoSala:UrlProgresivoSala,
        Tipo:Tipo
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if(response.respuesta){
                toastr.success(response.mensaje,"Mensaje Servidor")
            }
            else{
                toastr.error(response.mensaje,"Mensaje Servidor")
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    })
}
//#region Validaciones de Formularios
$("#form_registro_campania")
    .bootstrapValidator({
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            id: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            sala_id: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            nombre: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            descripcion: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            fechaini: {
                validators: {
                    notEmpty: {
                        message: ' '
                    },
                    date: {
                        format: 'DD/MM/YYYY',
                        message: 'La Fecha de Inicio no es valida'
                    }

                }
            },
            fechafin: {
                validators: {
                    notEmpty: {
                        message: ' '
                    },
                    date: {
                        format: 'DD/MM/YYYY',
                        message: 'La Fecha Final no es valida'
                    }

                }
            },
            tipo: {
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
            duracionCodigoDias: {
                validators: {                  
                    regexp: {
                        regexp: /^[0-9]+$/,
                        message: 'Solo se permiten números.'
                    },
                    callback: {
                        message: 'Ambos campos no pueden ser 0 al mismo tiempo.',
                        callback: function (value, validator, $field) {
                            if ($("#cboTipoCampania").val() == 2) {
                                var dias = $('#duracionCodigoDias').val();
                                var horas = $('#duracionCodigoHoras').val();
                                return !(dias == 0 && horas == 0);
                            }
                        }
                    }
                }
            },
            duracionCodigoHoras: {
                validators: {
                    notEmpty: {
                        message: 'No puede estar vacio'
                    },
                    regexp: {
                        regexp: /^[0-9]+$/,
                        message: 'Solo se permiten números.'
                    },
                    callback: {
                        message: 'Ambos campos no pueden ser 0 al mismo tiempo.',
                        callback: function (value, validator, $field) {
                            if ($("#cboTipoCampania").val() == 2) {
                                var dias = $('#duracionCodigoDias').val();
                                var horas = $('#duracionCodigoHoras').val();
                                return !(dias == 0 && horas == 0);
                            }
                        }
                    }
                    
                }
            },
            duracionReactivacionCodigoHoras: {
                validators: {
                    notEmpty: {
                        message: ' No puede estar vacio'
                    },
                    regexp: {
                        regexp: /^[0-9]+$/,
                        message: 'Solo se permiten números.'
                    },
                    callback: {
                        message: 'Ambos campos no pueden ser 0 al mismo tiempo.',
                        callback: function (value, validator, $field) {
                            if ($('#codigoSeReactiva').is(':checked')) {
                                var dias = $('#duracionReactivacionCodigoDias').val();
                                var horas = $('#duracionReactivacionCodigoHoras').val();
                                // Comprueba si ambos días y horas son 0
                                return !(dias == 0 && horas == 0);
                            }
                        }
                    }
                }
            },
            duracionReactivacionCodigoDias: {
                validators: {
                    notEmpty: {
                        message: ' No puede estar vacio'
                    },
                    regexp: {
                        regexp: /^[0-9]+$/,
                        message: 'Solo se permiten números.'
                    },
                    callback: {
                        message: 'Ambos campos no pueden ser 0 al mismo tiempo.',
                        callback: function (value, validator, $field) {
                            if ($('#codigoSeReactiva').is(':checked')) {
                                var dias = $('#duracionReactivacionCodigoDias').val();
                                var horas = $('#duracionReactivacionCodigoHoras').val();
                                // Comprueba si ambos días y horas son 0
                                return !(dias == 0 && horas == 0);
                            }
                        }
                    }
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
$("#form_campania_parametros")
    .bootstrapValidator({
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            condicion_tipo: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            condicion_juego: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            txt_tipo_condicion_sala: {
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
        // Remove the has-success class
        $parent.removeClass('has-success');
        // Hide the success icon
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });


$("#form_campania_cupones")
    .bootstrapValidator({
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            Cliente_Id: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            txt_cliente_nombre: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            SlotId: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },

            // Win: {
            //     validators: {
            //         notEmpty: {
            //             message: 'Win ,Campo Obligatorio, Obligatorio'
            //         }
            //     }
            // },
            impresora_id: {
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
        // Remove the has-success class
        $parent.removeClass('has-success');
        // Hide the success icon
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });
$("#form_registro_cliente")
    .bootstrapValidator({
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            Nombre: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            ApelPat: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            ApelMat: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            // Celular1: {
            //     validators: {
            //         notEmpty: {
            //             message: 'Ingrese Telefono, Obligatorio'
            //         }
            //     }
            // },
            NroDoc: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            FechaNacimiento: {
                validators: {
                    notEmpty: {
                        message: ' '
                    },
                    date: {
                        format: 'DD/MM/YYYY',
                        message: ' '
                    }

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
//#endregion


///////////////////////////////////////////////cliente validar ticket//////////////////////////////////////////////////////////

function validarcliente(nrodoc) {

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CampaniaCliente/GetClienteCoincidencia",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({coincidencia: nrodoc }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            respuesta = response.respuesta;
            if (respuesta) {
                $("#textdocument").val(nrodoc);
                $("#textcliente").val(response.data.NombreCompleto);
                $("#txt_id_cliente_validar").val(response.data.Id);

                $(".lblvalidar").show();
                $(".lbl_validar_head").hide();
            }
            else {
                $("#txtNombrevalidar").val(response.data.Nombre);
                $("#txtApelPatvalidar").val(response.data.ApelPat);
                $("#txtApelMatvalidar").val(response.data.ApelMat);
                $("#txtNroDocvalidar").val(response.data.NroDoc);

                toastr.error(response.mensaje, "Mensaje Servidor");
                $("#modal_clienteNuevoCampania_validar").modal("show");
            }
            var empleado = response.data;


        },
        error: function (request, status, error) {
            $(".modal_clienteNuevoCampania_validar").modal("hide");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

$('.btn_limpíar').on('click', function (e) {
    $("#txt_id_cliente_validar").val("");
    $("#NroDocument").val("");
    $("#textdocument").val("");
    $("#textcliente").val("");
    $("#NroTicket").val("");
    $(".lblvalidar").hide();
    $(".lbl_validar_head").show();
    
    
});

$("#form_registro_clientevalidar")
    .bootstrapValidator({
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            Nombre: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            ApelPat: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            ApelMat: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            // Celular1: {
            //     validators: {
            //         notEmpty: {
            //             message: 'Ingrese Telefono, Obligatorio'
            //         }
            //     }
            // },
            NroDoc: {
                validators: {
                    notEmpty: {
                        message: ' '
                    }
                }
            },
            FechaNacimiento: {
                validators: {
                    notEmpty: {
                        message: ' '
                    },
                    date: {
                        format: 'DD/MM/YYYY',
                        message: ' '
                    }

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

$('.btnGuardarclientenuevoValidar').on('click', function (e) {
    //const duracionCuponHoras = $("#duracionCuponHoras")
    //const duracionCuponDias = $("#duracionCuponHoras")
    //const duracionRegeneracionCuponHoras
    //const duracionRegeneracionCuponDias
    //if ($("#cboTipoCampania").val() == 2) {
    //    $("#duracionCuponHoras").val()
    //}
    $("#form_registro_clientevalidar").data('bootstrapValidator').resetForm();
    var validar = $("#form_registro_clientevalidar").data('bootstrapValidator').validate();
    if (validar.isValid()) {
        var sala_id = $("#campaniaidsalaid").val();
        var urlenvio = "";
        var lugar = "";
        var accion = "";
        lugar = "CampaniaCliente/CampaniaGuardarClienteJson";
        accion = "NUEVO CLIENTE";
        urlenvio = basePath + "CampaniaCliente/CampaniaGuardarClienteJson";

        var dataForm = $('#form_registro_clientevalidar').serializeFormJSON();
        $.when(dataAuditoria(1, "#form_registro_clientevalidar", 3, lugar, accion)).then(function (response, textStatus) {
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
                },
                success: function (response) {
           
                    if (response.respuesta) {
                        if (response.idcliente.Id > 0) {
                            //guardarCLienteCampaña(response.idcliente.Id, $("#compania_id").val());
                            $('#txtNombrevalidar').val("");
                            $('#txtApelPatvalidar').val("");
                            $('#txtApelMatvalidar').val("");
                            $('#txtNroDocvalidar').val("");
                            $('#txtCelular1validar').val("");
                            $('#txtemailvalidar').val("");
                            $("#txt_id_cliente_validar").val(response.idcliente.Id);
                            $("#textdocument").val(response.idcliente.NroDoc);
                            $("#textcliente").val(response.idcliente.NombreCompleto);
                          
                            $('#txtFechaNacimientovalidar').data("DateTimePicker").setDate(moment());
                            $(".lblvalidar").show();
                            $(".lbl_validar_head").hide();
                            $("#modal_clienteNuevoCampania_validar").modal("hide");
                            toastr.success("Cliente Registrado", "Mensaje Servidor");
                        }
                        else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }
                        
                    }
                    else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {

                }
            });
        });

    }

});

function guardarCLienteCampaña(cliente_id, campania_id) {
    url = "CampaniaCliente/GuardarClienteCampaniaJson";
    $.ajax({
        url: basePath + "/" + url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ cliente_id, campania_id }),
        beforeSend: function () {
            //$.LoadingOverlay("show");
        },
        complete: function () {
            //$.LoadingOverlay("hide");
        },
        success: function (response) {
            if (response.respuesta) {

                //toastr.success(response.mensaje, "Mensaje Servidor");
            }
            else {
                //clientesusBusquedaRegistrados();
                //toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            //clientesusBusquedaRegistrados();
        }
    });
}

function valideKeyNumber(evt) {
    var code = (evt.which) ? evt.which : evt.keyCode;
    if (code >= 48 && code <= 57 || code == 13) {
        return true;
    }
    return false;
}
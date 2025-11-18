$(document).ready(function () {
    let canSearch = true;
    ObtenerListaSalas();

    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });

    $(".monthdateOnly_").datetimepicker({
        pickTime: false,
        format: 'MM/YYYY',           // Format for Month and Year
        viewMode: 'months',          // Start the picker at the month view
        defaultDate: dateNow,        // Set the default date
        maxDate: dateNow,            // Set the maximum date
        minViewMode: 'months',       // Disable day selection, only month and year are selectable
    });

    $("#btnBuscar").on("click", function () {
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
        //buscarListadoAsistencia();
        ListarClienteServer();
    });

    //#region Tabs
    $(document).on("click", "#tab-multi-sala", function () {
        limpiarSelectTabs();
        handleVisibilityVariasSalas(true);
        handleVisibilityUniqueSala(false);
        handleVisibilityPorMes(false);
        handleVisibilityNotificacion(false);
        $(this).addClass("select");
    })

    $(document).on("click", "#tab-sala-unica", function () {
        limpiarSelectTabs();
        handleVisibilityVariasSalas(false);
        handleVisibilityUniqueSala(true);
        handleVisibilityPorMes(false);
        handleVisibilityNotificacion(false);
        $(this).addClass("select");
    })

    $(document).on("click", "#tab-reporte-por-mes", function () {
        limpiarSelectTabs();
        handleVisibilityVariasSalas(false);
        handleVisibilityUniqueSala(false);
        handleVisibilityPorMes(true);
        handleVisibilityNotificacion(false);
        $(this).addClass("select");
    })

    $(document).on("click", "#tab-reporte-notificacion", function () {
        limpiarSelectTabs();
        handleVisibilityVariasSalas(false);
        handleVisibilityUniqueSala(false);
        handleVisibilityPorMes(false);
        handleVisibilityNotificacion(true);
        $(this).addClass("select");
    })

    const limpiarSelectTabs = () => {
        $("#tab-multi-sala").removeClass("select");
        $("#tab-sala-unica").removeClass("select");
        $("#tab-reporte-por-mes").removeClass("select");
        $("#tab-reporte-notificacion").removeClass("select");
    }

    const handleVisibilityVariasSalas = (show) => {
        const elements = ["#btnExcel", "#btnBuscar", "#content-tab-multi-sala"];
        elements.forEach(selector => $(selector).toggle(show));
    };

    const handleVisibilityUniqueSala = (show) => {
        const elements = ["#btnExcelGlobal", "#content-tab-sala-unica"];
        elements.forEach(selector => $(selector).toggle(show));
    };

    const handleVisibilityPorMes = (show) => {
        const elements = ["#btnExcelGlobalMes", "#btnExcelGlobalMesV2", "#content-tab-reporte-por-mes"];
        elements.forEach(selector => $(selector).toggle(show));
    };

    const handleVisibilityNotificacion = (show) => {
        const elements = ["#btnExcelGlobalNotificacion", "#content-tab-reporte-notificacion"];
        elements.forEach(selector => $(selector).toggle(show));
    };

    $("#tab-multi-sala").click();
    //#endregion

    $(document).on("click", "#btnExcelGlobalMes, #btnExcelGlobalMesV2", function () {
        let verInfoContacto = this.id === "btnExcelGlobalMesV2";
        let fechaIni = $("#fechaIniciomes").val();
        let fechaFin = $("#fechaFinmes").val();
        let sala = $("#cboSalames").val();         
        if (!validateDateRange(fechaIni, fechaFin)) {
            toastr.error("Fecha Fin no puede ser menor a Fecha Inicio", "Mensaje Servidor");
            return false;
        }
        if (sala == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }

        const endpoint = `${basePath}AsistenciaCliente/GetRepoteExcelSalaPorMes`;
        const fechainiStr = `?fechaini=${fechaIni}`;
        const fechafinStr = `&fechafin=${fechaFin}`;
        const salaStr = Array.isArray(sala) && sala.length > 0 ? `&salas=${sala}` : "";
        const verInfoContactoStr = `&verInfoContacto=${verInfoContacto.toString()}`;

        const url = `${endpoint}${fechainiStr}${fechafinStr}${salaStr}${verInfoContactoStr}`;

        var request = new XMLHttpRequest();
        request.open('GET', url, true);
        request.responseType = 'blob';
        $.LoadingOverlay("show");
        request.onload = function (e) {
            //var data = request.response;
            //var blobUrl = window.URL.createObjectURL(data);
            //var downloadLink = document.createElement('a');
            //downloadLink.href = blobUrl;
            //let name_file = "Reportes_" +  fechaIni.replace("/", "_") + "_" + fechaFin.replace("/", "_");
            //downloadLink.download = name_file  || 'download';
            //downloadLink.click();
            //$.LoadingOverlay("hide");
            if (request.status === 200) { // Successful response
                const contentType = request.getResponseHeader("Content-Type");

                if (contentType && contentType.includes("application/zip")) {
                    // The response is the file we expect
                    const data = request.response;
                    const blobUrl = window.URL.createObjectURL(data);
                    const downloadLink = document.createElement('a');
                    downloadLink.href = blobUrl;
                    let name_file = "Reportes_" + fechaIni.replace("/", "_") + "_" + fechaFin.replace("/", "_");
                    downloadLink.download = name_file || 'download';
                    downloadLink.click();
                } else {
                    // The response is not a file; it's likely an error page
                    const reader = new FileReader();
                    reader.onload = function (e) {
                        const htmlContent = e.target.result;
                        // Attempt to detect 403 in the HTML content if not caught by status
                        if (htmlContent.includes("403") || htmlContent.toLowerCase().includes("forbidden")) {
                            toastr.error("No cuentas con los permisos necesarios", "Mensaje Servidor");
                        } else {
                            toastr.error("Ocurrió un error al generar el archivo. Revise los detalles.", "Mensaje Servidor");
                        }
                        console.error("Error details:", htmlContent);
                    };
                    reader.readAsText(request.response);
                }
            } else if (request.status === 403) { // Forbidden error
                toastr.error("No tiene permisos para descargar este archivo.", "Error 403");
            } else { // Other errors
                toastr.error("Ocurrió un error al generar el archivo. Código de error: " + request.status, "Mensaje Servidor");
            }
            $.LoadingOverlay("hide");
        };
        request.send();
    });

    $(document).on("click", "#btnExcelGlobal", function () {
        obtenerClientesPorSala()
    })

    $(document).on("click", "#btnExcel", function () {
        let fechaIni = $("#fechaInicio").val();
        let fechaFin = $("#fechaFin").val();
        let sala = $("#cboSala").val();
        if (sala == null) {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        let listasala = $("#cboSala").val();
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "AsistenciaCliente/GetListadoCLientesSalaExcel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ ArraySalaId: listasala, fechaIni, fechaFin }),
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                if (response.respuesta) {
                    let data = response.data;
                    let file = response.excelName;
                    let a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/vnd.ms-excel;base64, " + data;
                    a.download = file;
                    a.click();
                } else {
                    toastr.error(response.mensaje)
                }
            },
            error: function (request, status, error) {
                //toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });

    });

    $(document).on("click", "#btnExcelGlobalNotificacion", function () {
        const data = {
            codSalas: $("#cboSalaNotificacion").val(),
            fechaInicio: $("#fechaInicioNotificacion").val(),
            fechaFin: $("#fechaFinNotificacion").val(),
            enviaNotificacionWhatsapp: $('#cboEnvioNotificacionWhatsapp').val(),
            enviaNotificacionSms: $('#cboEnvioNotificacionSms').val(),
            enviaNotificacionEmail: $('#cboEnvioNotificacionEmail').val(),
            llamadaCelular: $('#cboLlamadaCelular').val()
        }

        if (!data.codSalas) {
            toastr.warning("Seleccione al menos una sala", "Aviso");
            return;
        }

        if (!data.enviaNotificacionWhatsapp) {
            toastr.warning("Seleccione una opción para WhatsApp", "Aviso");
            return;
        }

        if (!data.enviaNotificacionSms) {
            toastr.warning("Seleccione una opción para SMS", "Aviso");
            return;
        }

        if (!data.llamadaCelular) {
            toastr.warning("Seleccione una opción para Llamada", "Aviso");
            return;
        }

        if (!data.enviaNotificacionEmail) {
            toastr.warning("Seleccione una opción para Email", "Aviso");
            return;
        }

        $.ajax({
            type: "POST",
            url: basePath + "AsistenciaCliente/GetListadoClientesNotificacionExcel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            beforeSend: () => $.LoadingOverlay("show"),
            complete: () => $.LoadingOverlay("hide"),
            success: function (response) {
                if (response.respuesta) {
                    const data = response.data;
                    const file = response.excelName;
                    const a = document.createElement('a');
                    a.target = '_self';
                    a.href = "data:application/vnd.ms-excel;base64, " + data;
                    a.download = file;
                    a.click();
                } else {
                    toastr.error(response.mensaje)
                }
            }
        });
    })
});

function validateDateRange(fechaIni, fechaFin) {
    // Parse the dates
    let [dayIni, yearIni] = fechaIni.split('/');
    let [dayFin, yearFin] = fechaFin.split('/');
    // Create Date objects for comparison
    let iniDate = new Date(yearIni, dayIni - 1); // -1 because JavaScript months are 0-based
    let finDate = new Date(yearFin, dayFin - 1);
    // Check if fechaFin is not earlier than fechaIni
    if (finDate < iniDate) {
        return false;
    }
    return true;
}

function ObtenerListaSalas() {
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: function (result) {
            var datos = result.data;
            $("#cboSalames").append('<option value="todas">Todas las Salas</option>');
            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
                $("#cboSalaUnique").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
                $("#cboSalames").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
                $("#cboSalaNotificacion").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                multiple: true, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboSalaUnique").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboSalames").select2({
                multiple: true, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboSalaNotificacion").select2({
                multiple: true, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboEnvioNotificacionWhatsapp").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: false, minimumResultsForSearch: 5
            });
            $("#cboEnvioNotificacionSms").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: false, minimumResultsForSearch: 5
            });
            $("#cboEnvioNotificacionEmail").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: false, minimumResultsForSearch: 5
            });
            $("#cboLlamadaCelular").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: false, minimumResultsForSearch: 5
            });

            $("#cboSalames").val(null).trigger("change");
            $("#cboSala").val(null).trigger("change");
            $("#cboSalaUnique").val(null).trigger("change");
            $("#cboSalaNotificacion").val(null).trigger("change");
            $("#cboEnvioNotificacionWhatsapp").val(-1).trigger("change");
            $("#cboEnvioNotificacionSms").val(-1).trigger("change");
            $("#cboEnvioNotificacionEmail").val(-1).trigger("change");
            $("#cboLlamadaCelular").val(-1).trigger("change");
        }
    });
    return false;
}

function ListarClienteServer() {
    let tienePermisoVerInformacionContactoCliente = false;

    objetodatatableClientes = $("#tableCliente").DataTable({
        language: {
            searchPlaceholder: "Presione Enter para buscar"
        },
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
            url: basePath + "AsistenciaCliente/GetListadoCLientesSala",
            type: "POST",
            dataType: "json",
            processData: false,
            contentType: "application/json;charset=UTF-8",
            data: function (data) {

                data.ArraySalaId = $("#cboSala").val();
                data.fechaIni = $("#fechaInicio").val();
                data.fechaFin = $("#fechaFin").val();

                return JSON.stringify(data);
            },
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            complete: function (response) {
                $.LoadingOverlay("hide");
                canSearch = true;
                if (response.status === 200) {
                    tienePermisoVerInformacionContactoCliente = response.responseJSON.permisos.verInformacionContactoCliente;
                    objetodatatableClientes.column(6).visible(tienePermisoVerInformacionContactoCliente); // Celular
                    objetodatatableClientes.column(7).visible(tienePermisoVerInformacionContactoCliente); // Correo
                }
            },
        },
        columns: [
            { data: "codSala", title: "cod Sala" },
            { data: "NombreSala", title: "Sala" },
            { data: "TipoDocumento", title: "Tipo Documento" },
            { data: "NroDoc", title: "Nro. documento" },
            { data: "NombreCliente", title: "Cliente" },
            { data: "cantDosis", title: "Cant. Dosis" },
            {
                data: "Celular", title: "Celular", visible: tienePermisoVerInformacionContactoCliente,
                render: function (value, type, row) {
                    const tieneObservacion = row.EsLudopata || row.EsProhibido || row.EsRobaStacker;
                    return tieneObservacion ? '-' : value 
                }
            },
            {
                data: "Mail", title: "Correo", visible: tienePermisoVerInformacionContactoCliente,
                render: function (value, type, row) {
                    const tieneObservacion = row.EsLudopata || row.EsProhibido || row.EsRobaStacker;
                    return tieneObservacion ? '-' : value
                }
            },
            {
                data: "EnviaNotificacionWhatsapp", title: "Notif. WhatsApp", render(value) {
                    const text = value ? 'Sí' : 'No';
                    const type = value ? 'success' : 'danger';
                    return `<span class="badge badge-${type}">${text}</span>`
                },
            },
            {
                data: "EnviaNotificacionSms", title: "Notif. SMS", render(value) {
                    const text = value ? 'Sí' : 'No';
                    const type = value ? 'success' : 'danger';
                    return `<span class="badge badge-${type}">${text}</span>`
                },
            },
            {
                data: "LlamadaCelular", title: "Llamada", render(value) {
                    const text = value ? 'Sí' : 'No';
                    const type = value ? 'success' : 'danger';
                    return `<span class="badge badge-${type}">${text}</span>`
                },
            },
            {
                data: "EnviaNotificacionEmail", title: "Notif. Email", render(value) {
                    const text = value ? 'Sí' : 'No';
                    const type = value ? 'success' : 'danger';
                    return `<span class="badge badge-${type}">${text}</span>`
                },
            },
            {
                data: null, sortable: false, title: "Observación",
                render: function (row) {
                    const tieneObservacion = row.EsLudopata || row.EsProhibido || row.EsRobaStacker;
                    const badgeLudopata = row.EsLudopata ? `<span class="badge badge-ludopata">Ludopata</span>` : '';
                    const badgeProhibido = row.EsProhibido ? `<span class="badge badge-prohibido">Prohibido</span>` : '';
                    const badgeRobaStacker = row.EsRobaStacker ? `<span class="badge badge-roba-stacker">Roba Stacker</span>` : '';
                    return tieneObservacion ? `${badgeLudopata}${badgeProhibido}${badgeRobaStacker}` : '-';
                },
            },
            {
                data: "FechaNacimiento", title: "F. Nac.", "render": function (value, type, oData, meta) {
                    return moment(oData.FechaRegistro).format('DD/MM/YYYY')

                }
            },
            {
                data: "FechaRegistro", title: "F. Reg.", "render": function (value, type, oData, meta) {
                    return moment(oData.FechaRegistro).format('DD/MM/YYYY hh:mm:ss A')

                }
            },
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
    $('#tableCliente_filter input').css('width', '175px');
    $('#tableCliente_filter input').unbind().bind('keyup', function (e) {
        let search = this.value;
        if (e.keyCode === 13 && canSearch) {
            canSearch = false;
            objetodatatableClientes.search(search).draw();
        }
    });
    $('#tableCliente tbody').on('click', 'tr', function () {
        $(this).toggleClass('selected');
    });
}

function buscarListadoAsistencia() {
    var listasala = $("#cboSala").val();
    var fechaIni = $("#fechaInicio").val();
    var fechaFin = $("#fechaFin").val();
    var addtabla = $(".contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "AsistenciaCliente/GetListadoCLientesSala",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ ArraySalaId: listasala, fechaIni, fechaFin }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            console.log(response.data)
            response = response.data;
            $(addtabla).empty();
            $(addtabla).append('<table id="ResumenSala" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#ResumenSala").DataTable({
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
                    { data: "codSala", title: "cod Sala" },
                    { data: "NombreSala", title: "Sala" },
                    { data: "NroDoc", title: "Nro. documento" },
                    { data: "NombreCliente", title: "Cliente" },
                    { data: "cantDosis", title: "Cant. Dosis" },
                    { data: "Celular", title: "Celular" },
                    { data: "Mail", title: "Correo" },
                    {
                        data: "FechaNacimiento", title: "F. Nac.", "render": function (value, type, oData, meta) {
                            return moment(oData.FechaRegistro).format('DD/MM/YYYY')

                        }
                    },
                    {
                        data: "FechaRegistro", title: "F. Reg.", "render": function (value, type, oData, meta) {
                            return moment(oData.FechaRegistro).format('DD/MM/YYYY hh:mm:ss A')

                        }
                    },
                ],
                "initComplete": function (settings, json) {
                },
                "drawCallback": function (settings) {
                }
            });
            $('#ResumenSala tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        error: function (request, status, error) {
            //toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function obtenerClientesPorSala() {
    let sala = $("#cboSalaUnique").val();
    if (sala == null) {
        toastr.error("Seleccione Sala", "Mensaje Servidor");
        return false;
    }
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "AsistenciaCliente/GetAllListadoClienteSala",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ salaId: sala }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if (response.respuesta) {
                let data = response.data;
                let file = response.excelName;
                let a = document.createElement('a');
                a.target = '_self';
                a.href = "data:application/vnd.ms-excel;base64, " + data;
                a.download = file;
                a.click();
            } else {
                toastr.error(response.mensaje)
            }
        },
        error: function (request, status, error) {
            //toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });

}
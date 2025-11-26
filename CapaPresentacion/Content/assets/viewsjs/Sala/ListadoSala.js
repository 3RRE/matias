//"use strict";
empleadodatatable = "";

function afterTableInitialization(settings, json) { 
    tableAHORA = settings.oInstance.api(); 
}

function ListarSalas() { 
    var url = basePath + "Sala/ListadoTodosSala";
    var data = {}; var respuesta = ""; aaaa = "";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            respuesta = response.data
            console.log("LISTAR SALAS",respuesta)
            objetodatatable = $("#tableSala").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "paging": true,
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                "initComplete": function (settings, json) {
                     
                    $('a#excel,a#pdf,a#imprimir').off("click").on('click', function () {
                        ocultar = ["Accion"];
                        tituloreporte = "Reporte Salas";
                        funcionbotones({
                            botonobjeto: this, tablaobj: objetodatatable, ocultar: ocultar, tituloreporte: tituloreporte
                        });
                    });
                },
                data: response.data,
                columnDefs: [
                    {
                        targets: [0],
                        className: "text-center"
                    }
                ],
                aaSorting: [[1, 'asc']],
                columns: [
                    {
                        data: null,
                        title: '<i class="glyphicon glyphicon-th-list"></i>',
                        "render": {
                            _: function (value, type, row) {
                                return row.EsPrincipal ? '<i class="glyphicon glyphicon-ok-circle fa-xl text-success"></i>' : '<i class="glyphicon glyphicon-remove-circle text-danger"></i>';
                            },
                            sort: "EsPrincipal"
                        }
                    },
                    { data: "Nombre", title: "Sala" },
                    { data: "NombreCorto", title: "Nombre Corto" },
                    { data: "UrlProgresivo", title: "Url Progresivo" }, 
                    { data: "UrlSalaOnline", title: "Url Sala Online" },
                    {
                        title: "Tipo" ,
                        data: null,
                        "bSortable": false,
                        "render": function (value,type,row) {
                            let span=''
                            if(row.tipo==0){
                                span='SALA'
                            }
                            else if(row.tipo==1){
                                span='RESTAURANTE'
                            }
                            else if(row.tipo==2){
                                span='HOTEL'
                            }
                            else if (row.tipo == 3) {
                                span = 'CANAL ONLINE'
                            }
                            else{
                                span='SALA'
                            }
                            return span
                        }
                    },
                    {
                        data: null, title: "Estado", "render": function (value, type, row) {
                            let select = `<select style="width:100%" class="input-sm selectEstado select${row.CodSala}" data-id=${row.CodSala}>`;

                            if (row.Estado == 1) {
                                select += `<option value="1" selected>Activo</option><option value="0">Inactivo</option>`;
                            }
                            else {
                                select += `<option value="1">Activo</option><option value="0" selected>Inactivo</option>`;
                            }
                            select += `</select>`;
                            return select;   
                        }
                    }, 
                   
                    {
                        data: "CodSala",
                        "bSortable": false,
                        "render": function (o) {
                            return '<button type="button" class="btn btn-xs btn-warning btnEditar" data-id="' + o + '" title="Editar"><i class="glyphicon glyphicon-pencil"></i></button> ' +
                                   '<button type="button" class="btn btn-xs btn-info btnHoraApertura" data-id="' + o + '" title="Actualizar Hora de Apertura"><i class="glyphicon glyphicon-time"></i></button>';
                        }
                    }
                ],
                "drawCallback": function (settings) {
                    $('.btnEditar').tooltip({
                        title: "Editar"
                    });
                    $('.btnHoraApertura').tooltip({
                        title: "Actualizar Hora de Apertura"
                    });
                },

                "initComplete": function (settings, json) {

                },
            });
            $('.btnEditar').tooltip({
                title: "Editar"
            });
            $('.btnHoraApertura').tooltip({
                title: "Actualizar Hora de Apertura"
            });

        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        }
    });
};

$(document).ready(function (){

    ListarSalas();
    $(document).on("click", "#btnExcel", function () {
        $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Sala/ListadoTodosSalaExportarExcel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
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
                }
            },
            error: function (request, status, error) {
                toastr.error("Error De Conexion, Servidor no Encontrado.");
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    });
    $(document).on('click', '.btnEditar', function (e) {
        var id = $(this).data("id");
        var url = basePath + "Sala/SalaModificarVista/" + id;
        window.location.replace(url);
    });

    $(document).on('click', '.btnUsuario', function (e) {
        var id = $(this).data("id");
        var url = basePath + "Sala/GetUsuarioSalaID";
        var data = { empleadiId: id };
        DataPostSend(url, data, false).done(function (response) {

            if (response) {
                if (response.respuesta) {
                    var data = response.respuesta;
                    var contentText;
                    var redirectUrl;
                    var btnText;
                    if (data.UsuarioID == 0) {
                        contentText = "No tiene un Usuario Registrado, ¿Desea Registrar Usuario?";
                        redirectUrl = basePath + "Usuario/UsuarioInsertarVista";
                        btnText = "Registrar Usuario";

                    } else {
                        contentText = "Sala con Usuario Registrado : " + data.UsuarioNombre;
                        redirectUrl = basePath + "Usuario/UsuarioRegistroVista/Registro" + data.UsuarioID;
                        btnText = "Detalle Usuario";
                    }
                    var js = $.confirm({
                        icon: 'fa fa-spinner fa-spin',
                        title: 'Mensaje Servidor',
                        theme: 'black',
                        animationBounce: 1.5,
                        columnClass: 'col-md-6 col-md-offset-3',
                        confirmButtonClass: 'btn-info',
                        cancelButtonClass: 'btn-warning',
                        confirmButton: btnText,
                        cancelButton: 'Cerrar',
                        content: contentText,
                        confirm: function () {
                            window.location.replace(redirectUrl);
                        },
                        cancel: function () {

                        },

                    });

                } else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }

            } else {
                toastr.error("Error no se logro conectar al servidor", "Mensaje Servidor");
            }
        }).fail(function (x) {
            toastr.error("Error Fallo Servidor", "Mensaje Servidor");
        });

    });
    VistaAuditoria("Sala/Salavista", "VISTA", 0, "",3);
    $('#btnNuevaSala').on('click', function (e) {
        window.location.replace(basePath + "Sala/SalaInsertarVista");
    });
    $(document).on("change", ".selectEstado", function () {
        let CodSala = $(this).data("id");
        let Estado = $(this).val();
        let dataForm = {
            CodSala: CodSala,
            Estado: Estado
        }
        $.confirm({
            title: 'Confirmación',
            content: '¿Desea cambiar el estado del registro?',
            confirmButton: 'Si',
            cancelButton: 'No',
            confirm: function () {
                $.ajax({
                    url: basePath+ "Sala/SalaModificarEstadoJson",
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
                            toastr.success("Registro Editado", "Mensaje Servidor");
                        }
                        else {
                            if (Estado == 1) {
                                $('.select' + CodSala).val(0);
                            }
                            else {
                                $('.select' + CodSala).val(1);
                            }
                        }
                    }
                })
            },
            cancel: function () {
                if (Estado == 1) {
                    $('.select' + CodSala).val(0);
                }
                else {
                    $('.select' + CodSala).val(1);
                }
            }
        });
    });

    $(document).on('click', '.btnHoraApertura', function (e) {
        e.preventDefault();
        var salaId = $(this).data("id");
        abrirModalHoraApertura(salaId);
    });
});

// Función para abrir modal de hora de apertura
function abrirModalHoraApertura(salaId) {
    // Obtener datos de la sala
    var salaData = objetodatatable.rows().data().toArray().find(row => row.CodSala === salaId);
    
    if (!salaData) {
        toastr.error("No se encontró la sala seleccionada", "Error");
        return;
    }

    // Crear el modal dinámicamente si no existe
    if ($('#modalHoraApertura').length === 0) {
        var modalHtml = `
            <div class="modal fade" id="modalHoraApertura" tabindex="-1" role="dialog" aria-labelledby="modalHoraAperturaLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content" data-border-top="multi">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title" id="modalHoraAperturaLabel">
                                <i class="glyphicon glyphicon-time"></i> Actualizar Hora de Apertura
                            </h4>
                        </div>
                        <div class="modal-body">
                            <form id="formHoraApertura">
                                <input type="hidden" id="hdnCodSala" name="CodSala" />
                                <input type="hidden" id="hdnHoraApertura24" name="HoraApertura24" />
                                
                                <div class="form-group">
                                    <label for="txtNombreSala">Sala:</label>
                                    <input type="text" class="form-control input-sm" id="txtNombreSala" readonly />
                                </div>
                                
                                <div class="form-group">
                                    <label for="txtHoraApertura">Hora de Apertura:</label>
                                    <div class="input-group">
                                        <input type="text" class="form-control input-sm" id="txtHoraApertura" name="HoraApertura" />
                                        <span class="input-group-addon input-group-icon">
                                            <span class="glyphicon glyphicon-time"></span>
                                        </span>
                                    </div>
                                </div>
                            </form>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                            <button type="button" class="btn btn-primary" id="btnGuardarHoraApertura">Guardar</button>
                        </div>
                    </div>
                </div>
            </div>
        `;
        $('body').append(modalHtml);
    }
    
    // Llenar los datos del formulario
    $('#hdnCodSala').val(salaData.CodSala);
    $('#txtNombreSala').val(salaData.Nombre);
    
    // Formatear HoraApertura desde el objeto TimeSpan serializado
    var horaAperturaValue = '';
    if (salaData.HoraApertura) {
        // Si es un objeto TimeSpan serializado (tiene propiedades Hours y Minutes)
        if (typeof salaData.HoraApertura === 'object' && salaData.HoraApertura.Hours !== undefined) {
            var hours = salaData.HoraApertura.Hours;
            var minutes = salaData.HoraApertura.Minutes;
            horaAperturaValue = hours.toString().padStart(2, '0') + ':' + minutes.toString().padStart(2, '0');
        } 
        // Si es una cadena en formato HH:mm:ss o HH:mm
        else if (typeof salaData.HoraApertura === 'string') {
            var horaStr = salaData.HoraApertura.toString();
            if (horaStr.indexOf(':') > -1) {
                var partes = horaStr.split(':');
                horaAperturaValue = partes[0].padStart(2, '0') + ':' + partes[1].padStart(2, '0');
            } else {
                horaAperturaValue = horaStr;
            }
        }
    }
    
    // Guardar el valor en formato 24 horas en el campo oculto
    $('#hdnHoraApertura24').val(horaAperturaValue);
    
    // Limpiar el campo antes de configurar el timepicker
    $('#txtHoraApertura').val('');
    
    // Destruir el timepicker si ya existe
    if ($('#txtHoraApertura').data('DateTimePicker')) {
        $('#txtHoraApertura').data('DateTimePicker').destroy();
    }
    
    // Configurar el timepicker con formato de 12 horas (AM/PM)
    $('#txtHoraApertura').datetimepicker({
        pickDate: false,
        format: 'hh:mm A',  // Formato 12 horas con AM/PM
        defaultDate: horaAperturaValue ? moment(horaAperturaValue, 'HH:mm') : moment(),
        pickTime: true
    });
    
    // Si hay valor, establecerlo
    if (horaAperturaValue) {
        $('#txtHoraApertura').data('DateTimePicker').setDate(moment(horaAperturaValue, 'HH:mm'));
    }
    
    // Remover event handlers previos para evitar duplicados
    $('#txtHoraApertura').off('dp.change');
    
    // Evento para actualizar el campo oculto con la hora en formato 24 horas
    $('#txtHoraApertura').on('dp.change', function(e) {
        if (e.date) {
            var hora24 = e.date.format('HH:mm');
            $('#hdnHoraApertura24').val(hora24);
        }
    });
    
    // Mostrar el modal
    $('#modalHoraApertura').modal('show');
}

// Guardar hora de apertura
$(document).on('click', '#btnGuardarHoraApertura', function () {
    var salaId = $('#hdnCodSala').val();
    var horaApertura12 = $('#txtHoraApertura').val(); // Formato 12 horas para mostrar
    var horaApertura24 = $('#hdnHoraApertura24').val(); // Formato 24 horas para guardar
    
    if (!horaApertura12 || !horaApertura24) {
        toastr.warning("Debe ingresar una hora de apertura", "Advertencia");
        return;
    }
    
    $.confirm({
        title: 'Confirmación',
        content: '¿Está seguro de actualizar la hora de apertura a ' + horaApertura12 + '?',
        confirmButton: 'Sí',
        cancelButton: 'No',
        confirm: function () {
            $.ajax({
                url: basePath + "Sala/ActualizaHoraApertura",
                type: "POST",
                data: {
                    salaId: salaId,
                    horaApertura: horaApertura24  // Enviar en formato 24 horas
                },
                beforeSend: function () {
                    $.LoadingOverlay("show");
                },
                complete: function () {
                    $.LoadingOverlay("hide");
                },
                success: function (response) {
                    if (response.status) {
                        toastr.success(response.message, "Mensaje Servidor");
                        $('#modalHoraApertura').modal('hide');
                        ListarSalas(); // Recargar la tabla
                    } else {
                        toastr.error(response.message, "Mensaje Servidor");
                    }
                },
                error: function () {
                    toastr.error("Error al actualizar la hora de apertura", "Error");
                }
            });
        }
    });
});
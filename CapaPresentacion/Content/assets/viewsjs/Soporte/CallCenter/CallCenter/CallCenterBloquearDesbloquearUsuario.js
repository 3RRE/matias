$(document).ready(function () {
    ipPublicaG = "";
    ipPublicaGProgresivo = "";
    ObtenerListaSalas();
    $("#cboSala").select2();
    $("#cboProgresivo").select2();
    $(document).on('change', '#cboSala', function (e) {
        var ipPublica = $(this).val();
        ipPublicaG = ipPublica;
        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        } 
    });
    $("#btnBuscar").on("click", function () {
        if ($("#cboSala").val() == "") {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
            return false;
        }
        buscarUsuarios();
    });

    $(document).on('change', '.selectEmp', function (e) {
        if (!ipPublicaG.trim()) {
            toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
            return false;
        } 
        var select = $(this);
        var id = $(this).data("idusu");
        var nombre = $(this).data("nombreusu");
        var valor = $(this).val();
        console.log(valor, "___as");
        var td = $(this).parent();
        var bloquearDesbloquearUsuario = (valor == 1) ? 'Deshabilitar' : 'Habilitar';
        var js2 = $.confirm({
            icon: 'fa fa-spinner fa-spin',
            title: 'Seguro que desea ' + bloquearDesbloquearUsuario + ' al Usuario <b style="text-transform: uppercase">' + nombre + ' </b>?',
            theme: 'black',
            animationBounce: 1.5,
            columnClass: 'col-md-6 col-md-offset-3',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-warning',
            confirmButton: "confirmar",
            cancelButton: 'Cerrar',
            content: "",
            confirm: function () {
                var url = ipPublicaG + "/servicio/BloquearDesbloquearUsuario?isBloqueado=" + valor + "&codUsuario=" + id;
                
                var actualID = "";
                var actualNombre = "";
                var anteriorID = "";
                var anteriorNombre
                if (valor == 1) {
                    actualID = 1;
                    actualNombre = "Deshabilitado";
                    anteriorID = 0;
                    anteriorNombre = "Habilitado";
                }
                else {
                    actualID = 0;
                    actualNombre = "Habilitado";
                    anteriorID = 1;
                    anteriorNombre = "Deshabilitado";
                }

                var datainicial = {
                    UsuarioID: id,
                    UsuarioNombre: nombre,
                    EstadoEmpleado: anteriorID,
                    EstadoEmpleado_text: anteriorNombre,
                };
                var datafinal = {
                    UsuarioID: id,
                    UsuarioNombre: nombre,
                    EstadoEmpleado: actualID,
                    EstadoEmpleado_text: actualNombre,
                };
                dataAuditoriaJSON(3, "CallCenter/ConsultaObtenerModificarCaja", "CAMBIAR ESTADO", datainicial, datafinal);



                $.ajax({
                    type: "POST",
                    cache: false,
                    url: basePath + "CallCenter/ConsultaObtenerModificarCaja",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ url: url }),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (response) {
                        response = response.data;
                        toastr.success("Se ha actualizado al Usuario <b style='text-transform: uppercase'>" + nombre + "</b> " + response);
                        var color = (valor == 1) ? 'red' : 'green';
                        console.log(color);
                        select.css({ "background": color });
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
                var estado = (valor == 1) ? 0 : 1;
                var colorBloqueadoDesbloqueado = (estado) ? 'background: red;color: white;' : 'background: green;color: white;';
                td.html("");
                var selectedOptions = "";
                if (estado== 1) {
                   selectedOptions = '<option value="0">Habilitado</option>' +
                        '<option value="1" selected>Deshabilitado</option>';
                } else {
                      selectedOptions = '<option value="0" selected>Habilitado</option>' +
                        '<option value="1">Deshabilitado</option>';
               
                }
                td.append('<select style="' + colorBloqueadoDesbloqueado + '" class="form-control input-sm selectEmp" data-idusu="' + id + '" data-nombreusu="' + nombre + '" id="EstadoEmpleado" name="EstadoEmpleado">' + selectedOptions + '</select>');

            },
        });
    });
});
VistaAuditoria("CallCenter/CallCenterBloquearDesbloquearUsuario", "VISTA", 0, "", 3);
function buscarUsuarios() {
    if (!ipPublicaG.trim()) {
        toastr.error("Sala sin URL Asignada", "Mensaje Servidor");
        return false;
    } 
    var url = ipPublicaG + "/servicio/ObtenerUsuarios";

    dataAuditoria(1, "#formfiltro", 3, "CallCenter/ConsultaObtenerUsuariosListadoJson", "BOTON BUSCAR");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "CallCenter/ConsultaObtenerUsuariosListadoJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ url: url }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            response = response.data;
            objetodatatable = $("#table").DataTable({
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
                    { data: "CodU", title: "CodU" },

                    { data: "Nombre", title: "Nombre" },

                    {
                        data: null, title: "IsBloqueado",
                        "render": function (value) {
                            var seleccionado;
                            var nombre = value.Nombre;
                            //console.log(value.IsBloqueado)
                            var colorBloqueadoDesbloqueado = (value.IsBloqueado) ? 'background: red;color: white;' : 'background: green;color: white;';
                            var selectedOptions = "";
                            if (value.IsBloqueado) {
                                selectedOptions = '<option value="0">Habilitado</option>' +
                                    '<option value="1" selected>Deshabilitado</option>';
                            } else {
                                selectedOptions = '<option value="0" selected>Habilitado</option>' +
                                    '<option value="1">Deshabilitado</option>';
                            }
                            return '<select style="' + colorBloqueadoDesbloqueado + '" class="form-control input-sm selectEmp" data-idusu="' + value.CodU + '" data-nombreusu="' + nombre +'" id="EstadoEmpleado" name="EstadoEmpleado">' + selectedOptions + '</select>';

                        }
                    },

                    { data: "CU", title: "CU" },

                    { data: "Estado", title: "Estado" },

                    { data: "CUcierreCaja", title: "CUcierreCaja" },

                    { data: "nivel", title: "nivel" },

                    { data: "Sis", title: "Sis" },

                    { data: "codPer", title: "codPer" },

                    { data: "Usu_nameexterno", title: "Usu_nameexterno" },

                    

                    { data: "ContPassFallado", title: "ContPassFallado" }
                ]
                ,
                "rowCallback": function (row, data) {

                    $('td:eq(0)', row).css('background-color', '#F3F781');
                    $('td:eq(1)', row).css('background-color', '#F3F781');
                    $('td:eq(2)', row).css('background-color', '#F3F781');

                },
                "initComplete": function (settings, json) {

                    $('#btnExcel').off("click").on('click', function () {

                        cabecerasnuevas = [];
                        cabecerasnuevas.push({ nombre: "Sala", valor: $("#cboSala option:selected").text() });
                        cabecerasnuevas.push({ nombre: "Progresivo", valor: $("#cboProgresivo option:selected").text() });
                        cabecerasnuevas.push({ nombre: "Máquina", valor: $("#txtMaquina").val() });
                        cabecerasnuevas.push({ nombre: "Pozos", valor: $("#cboPozos option:selected").text() });

                        definicioncolumnas = [];
                        definicioncolumnas.push({ nombre: "Monto", tipo: "decimal", alinear: "right", sumar: "true" });

                        var ocultar = [];
                        funcionbotonesnuevo({
                            botonobjeto: this, ocultar: ocultar,
                            tablaobj: objetodatatable,
                            cabecerasnuevas: cabecerasnuevas,
                            definicioncolumnas: definicioncolumnas
                        });
                        VistaAuditoria("CallCenter/CallCenterBloquearDesbloquearUsuarioExcel", "EXCEL", 0, "", 3);
                    });

                },
            });
        },
        error: function (request, status, error) {
            toastr.error("Error De Conexion, Servidor no Encontrado.");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
function ObtenerListaSalas() {
    comboImagen = $("#cboImagen");
    comboImagen.find('option').remove();
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
                $("#cboSala").append('<option value="' + value.UrlProgresivo + '"  data-id="' + value.CodSala + '"  >' + value.Nombre + '</option>');
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








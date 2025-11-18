$(document).ready(function () {

    ListarTecnicos();

    $(document).on('click', '#btnNuevo', function () {
        var url = basePath + "UsuarioEncriptacion/UsuarioEncriptacionInsertarVista";
        window.location.replace(url);
    });

    $(document).on('click', '.btnEditar', function (e) {
        
        var id = $(this).data("id");
        var resp = VerificarEmpleadoUsuarioEncriptacionJson(id);
        if (resp === true) {
            var url = basePath + "UsuarioEncriptacion/UsuarioEncriptacionEditarVista?Id=" + id;
            window.location.replace(url);
        } else {
            toastr.error('Este usuario no tiene una contraseña Encriptada', "Mensaje Servidor");
        }
    });
    $(document).on('click', '.btnReiniciar', function (e) {
        var id = $(this).data("id");
        var resp = VerificarEmpleadoUsuarioEncriptacionJson(id);
        if (resp === true) {
            RenovarContraseniaUsuarioEncriptacion(id);
            ListarTecnicos();
        } else {
            toastr.error('Este usuario no tiene una contraseña Encriptada', "Mensaje Servidor");
        }
    });


});
function VerificarEmpleadoUsuarioEncriptacionJson(EmpleadoId) {
    var dataForm = { EmpleadoId: EmpleadoId };
    var Verificar;
    $.ajax({
        type: 'POST',
        async: false,
        contentType: "application/json",
        url: basePath + "UsuarioEncriptacion/VerificarEmpleadoUsuarioEncriptacionJson",
        data: JSON.stringify(dataForm),
        success: function (response) {
            
            var resp = response.respuesta;
            if (resp === true) {
                Verificar = resp;
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
    return Verificar;
}
function RenovarContraseniaUsuarioEncriptacion(EmpleadoId) {
    var dataForm = { EmpleadoId: EmpleadoId };
    $.ajax({
        type: 'POST',
        contentType: "application/json",
        url: basePath + "UsuarioEncriptacion/UsuarioEncriptacionRenovarContraseniaJson",
        data: JSON.stringify(dataForm),
        success: function (response) {
            var resp = response.respuesta;
            var msj = response.mensaje;
            if (resp === true) {
                toastr.success('Se Genero la contraseña Correctamente.', 'Mensaje Servidor');
            } else {
                toastr.error(msj, "Mensaje Servidor");
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
        }
    });
}
function ListarTecnicos() {
    var url = basePath + "UsuarioEncriptacion/TecnicosEncriptacionListarJson";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            respuesta = response.data
            $(".contenedor_usuarios_encriptacion").empty().append('<table id="tableEncript" class="table mb-0 table-bordered table-condensed table-hover table-striped"></table>');
            objetodatatable = $("#tableEncript").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "paging": true,
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                "aaSorting": [],
                oLanguage: { "sEmptyTable": '<div class="alert alert-info" style="padding: 3px;margin-bottom: 0px;border-radius: 0px;">No Existen Registros</div>', },
                "initComplete": function (settings, json) {

                    //   afterTableInitialization(settings,json)
                    $('button#excel,a#pdf,a#imprimir').off("click").on('click', function () {
                        ocultar = ["Accion"];//array de columnas para ocultar , usar titulo de columna
                        columna_cambio = [{
                            nombre: "Estado",
                            render: function (o) {
                                valor = "";
                                if (o === 1) {
                                    valor = "Activo";
                                }
                                else { valor = "InActivo"; }
                                return valor;
                            }
                        }]
                        cabecerasnuevas = [];
                        //cabecerasnuevas.push({ nombre: "cabecera", valor: "vdfcs" });
                        //tituloreporte = "Reporte Empleados";
                        funcionbotonesnuevo({
                            botonobjeto: this, tablaobj: objetodatatable, ocultar: ocultar/*, tituloreporte: tituloreporte*/, cabecerasnuevas: cabecerasnuevas, columna_cambio: columna_cambio
                        });
                    });
                },
                data: response.data,
                columns: [
                    {
                        data: "Id", title: "Nombres", "render": function (o, j, l) {
                            return l.ApellidosPaterno + ' ' + l.ApellidosMaterno + ' ' + l.Nombres;
                        }
                    },
                    {
                        data: "UsuarioNombre", title: "Usuario",
                        "render": function (value) {
                            var usuario = value == "" ? "Sin Usuario" : value;
                            return usuario;
                        }
                    },
                    {
                        data: "UsuarioPassword", title: "Contraseña",
                        "render": function (value) {
                            var password = value == "" ? "Sin Contraseña" : value;
                            return password;
                        }
                    },
                    {
                        data: "FechaIni", title: "Fecha Inicio",
                        render: function (value) {
                            var fecha = moment(value).format('DD/MM/YYYY');
                            if (fecha == "31/12/1752") {
                                return 'Sin Fecha';
                            } else {
                                return moment(value).format('DD/MM/YYYY HH:mm:ss');
                            }

                        }
                    },
                    {
                        data: "FechaFin", title: "Fecha Fin",
                        render: function (value) {
                            var fecha = moment(value).format('DD/MM/YYYY');
                            if (fecha == "31/12/1752") {
                                return 'Sin Fecha';
                            } else {
                                return moment(value).format('DD/MM/YYYY HH:mm:ss');
                            }
                        }
                    },
                    {
                        data: "Estado", title: "Estado",
                        "render": function (o) {
                            if (o == 1) {
                                return 'Activo';
                            } else {
                                return 'Inactivo';
                            }
                        }
                    },
                    {
                        data: null, title: "Accion",
                        "width": "120px",
                        "bSortable": false,
                        "render": function (o) {
                            return '<button type="button" class="btn btn-sm btn-warning btnEditar" data-id="' + o.EmpleadoID + '"><i class="fa fa-pencil-square-o"></i></button> ' +
                                '<button type="button" class="btn btn-sm btn-danger btnReiniciar" data-id="' + o.EmpleadoID + '"><i class="fa fa-gear"></i></button>';
                        }
                    }
                ],
                "drawCallback": function (settings) {
                    $('.btnEditar').tooltip({
                        title: "Editar"
                    });
                    $('.btnReiniciar').tooltip({
                        title: "Reiniciar Contraseña"
                    });
                }
            });
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {

        }
    });
};


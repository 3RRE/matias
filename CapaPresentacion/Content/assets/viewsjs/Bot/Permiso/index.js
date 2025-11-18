$(document).ready(function () {
    color = ['green', 'blue', 'orange', 'yellow', 'red', 'lime'];

    $(document).on('click', '#listaRoles li', function (event) {
        var id = $(this).data("id");
        var nombreRol = $(this).find("a").html();
        $("#nombreRolSpan").text(nombreRol);
        ListadoPermisoRol(id);
    });
    $("#btnPermMenu").on('click', function (event) {
        $("#tabPermMen").click();
    });

    $("#btnPermRol").on('click', function (event) {
        $("#tabPermRol").click();
    });

    $("#btnRolUsu").on('click', function (event) {
        $("#tabRolUsu").click();
    });


    listausuarios();
});
///////LISTADO MENUSSSS//////////////////////////////////

$('#cboRol_').on('change', function (event) {
    var rolid_ = $(this).val();
    if (rolid_) {
        //permisosMenuDis(rolid_);

        // new Menu Permisos
        getMenuRoleKey(rolid_)
    }
});

$(this).off('ifChecked', '#libody input');
$(document).on('ifChecked', '#libody input', function (event) {

    var idRol = $("#cboRol_").val();
    var idPermNombre = jQuery(this).val();
    var dataTitulo = jQuery(this).data("tit");
    var data = { WEB_RolID: idRol, WEB_PMeDataMenu: idPermNombre, WEB_PMeNombre: dataTitulo, WEB_PMeEstado: 1 }
    var url = basePath + "Seguridad/AgregarPermisoMenu";
    var principal = jQuery(this).data("principal");
    //console.log(data)


    if (principal == "1") {
        console.log("entro 1")
        DataPostWithoutChangeMsg(url, data, false, 1);
        jQuery(this).parent().parent().parent().parent().find('input:checkbox:not(:checked)').each(function () {
            $(this).iCheck('check');
        });

    } else {
        if (principal == '2') {
            console.log("entro 2")
            DataPostWithoutChangeMsg(url, data, false, 1);
            var secund = jQuery(this).parent().parent().parent().parent().parent().find('tr.' + idPermNombre).length;
            if (secund > 0) {
                jQuery(this).parent().parent().parent().parent().parent().find('tr.' + idPermNombre).each(function () {
                    $(this).iCheck('check');
                });
            }
        }
        else {
            console.log("entro 3");

            $.ajax({
                url: url,
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(data),
                beforeSend: function () {
                    //$.LoadingOverlay("show");
                },
                complete: function () {
                    //$.LoadingOverlay("hide");
                },
                success: function (response) {
                    var respuesta = response.respuesta;
                    if (respuesta === true) {
                        toastr.success("Se Asigno Permiso", "Mensaje Servidor");
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    if (xmlHttpRequest.status == 400) {
                        toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
                    } else {
                        toastr.error("Error Servidor", "Mensaje Servidor");
                    }
                }
            });

        }

    }
});

$(document).on('ifUnchecked', '#libody input', function (event) {
    var idRol = $("#cboRol_").val();
    var idPermNombre = jQuery(this).val();
    var dataTitulo = jQuery(this).data("tit");
    var data = { WEB_RolID: idRol, WEB_PMeDataMenu: idPermNombre }
    var url = basePath + "Seguridad/QuitarPermisoMenu";
    var principal = jQuery(this).data("principal");
    //DataPostWithoutChange(url, data, false);

    if (principal == "1") {
        DataPostWithoutChangeMsg(url, data, false, 0);
        jQuery(this).parent().parent().parent().parent().find('input:checkbox(:checked)').each(function () {
            $(this).iCheck('uncheck');
        });

    } else {

        if (principal == '2') {
            DataPostWithoutChangeMsg(url, data, false, 0);
            var secund = jQuery(this).parent().parent().parent().parent().parent().find('tr.' + idPermNombre).length;
            if (secund > 0) {
                jQuery(this).parent().parent().parent().parent().parent().find('tr.' + idPermNombre).each(function () {
                    $(this).iCheck('uncheck');
                });
            }
        } else {
            $.ajax({
                url: url,
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(data),
                beforeSend: function () {
                    //$.LoadingOverlay("show");
                },
                complete: function () {
                    //$.LoadingOverlay("hide");
                },
                success: function (response) {
                    var respuesta = response.respuesta;
                    if (respuesta === true) {
                        toastr.warning("Se Quito el Permiso Asignado", "Mensaje Servidor");
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor");
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
                    if (xmlHttpRequest.status == 400) {
                        toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
                    } else {
                        toastr.error("Error Servidor", "Mensaje Servidor");
                    }
                }
            });
        }
    }


});

function DataPostWithoutChangeMsg(url, data, loading, toaster) {

    var mensaje = true;
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: function () {
            if (loading === true) {
                $.LoadingOverlay("show");
            }
        },
        success: function (response) {
            var respuesta = response.respuesta;
            if (respuesta === true) {
                if (toaster == 1) {
                    toastr.success("Se Asigno Permiso", "Mensaje Servidor");
                } else {
                    toastr.warning("Se Quito el Permiso Asignado", "Mensaje Servidor");
                }

            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
        complete: function () {
            if (loading === true) {
                $.LoadingOverlay("show");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mensaje = false;
        }
    });
    return mensaje;
}

function ListadoPrincipales(url, data, loading) {

    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: function () {
            if (loading === true) {
                $.LoadingOverlay("show");
            }
        },
        success: function (response) {
            var mensaje = response.mensaje;
            if (mensaje) {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
            var listado = response.data;
            //console.log(listado)
            if (listado) {
                $.each(listado, function (index, value) {
                    $('#fecha' + value.WEB_ModuloNombre).text(moment(value.WEB_PMeFechaRegistro).format("DD/MM/YYYY"));
                    $('#hora' + value.WEB_ModuloNombre).text(moment(value.WEB_PMeFechaRegistro).format("hh:mm a"));
                });

            }

        },
        complete: function () {
            if (loading === true) {
                $.LoadingOverlay("show");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }
    });
}

$(document).on('click', ".ocultarTr", function (event) {
    var menuOcultar = $(this).data("esconder");
    //console.log(menuOcultar)
    var lis = $("tr." + menuOcultar);
    $.each(lis, function (j) {
        if ($(this).is(':visible')) {
            $(this).hide();
        }
        else {
            if ($(this).hasClass("menu")) {
                $(this).show();
            }
        }
    });
});

$(document).on('click', ".ocultarTrSubmenu", function (event) {
    var menuOcultar = $(this).data("esconder");
    //console.log(menuOcultar)
    var lis = $("tr." + menuOcultar);
    $.each(lis, function (j) {
        if ($(this).is(':visible')) {
            $(this).hide();
        }
        else {
            $(this).show();
        }
    });
});
datamenus = [];
function permisosMenuDis(rol) {
    if (rol != 0) {
        rol = $("#cboRol_").val();
    }
    else {
        rol = rolid;
    }

    var data = { rolId: rol }
    var url = basePath + "Seguridad/ListadoMenusRolId";
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
            var respuesta = response.dataResultado;
            if (response.mensaje) {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
            if (respuesta) {
                $("#libody").html("");
                menus = [];
                $.each(respuesta, function (index, value) {
                    menus.push(value.WEB_PMeDataMenu);
                });
                $(".cabecera").each(function (i) {
                    var total = $(".cabecera").length - 1;
                    var element = $(this);
                    var menu = element.data('menu1');
                    var titulo = element.data('titulo');
                    var hijos = $('.' + menu);
                    var icon = $(this).find('span.main-menu-icon').find('span').attr('class');
                    var check = "";

                    var tr = '';
                    if (hijos.length > 0) {
                        $(hijos).each(function () {
                            var element1 = $(this);
                            var menu1 = element1.data('menu1');
                            var titulo1 = element1.data('titulo');
                            var hijos1 = $('.' + menu1);
                            if (hijos1.length > 0) {
                                // console.log('Padre', '------' + element1.data('menu1'))
                                var existeMenu_ = jQuery.inArray(menu1, menus);
                                if (existeMenu_ >= 0) {
                                    check = "checked";
                                } else {
                                    check = "";
                                }

                                tr = tr + '<tr>' +
                                    '<td style="font-weight: bolder;"><span style="color:red !important;background-color:transparent !important" class="glyphicon glyphicon-star"></span> ' + titulo1 + '</td>' +
                                    '<td>' +
                                    '<label style="float:right"><input type="checkbox"  ' + check + '  data-principal="2" data-tit="' + titulo1 + '" value="' + menu1 + '" name="square-checkbox"></label>' +
                                    '</td>' +
                                    '</tr>';

                                $(hijos1).each(function () {
                                    var element2 = $(this);
                                    var menu2 = element2.data('menu1');
                                    var titulo2 = element2.data('titulo');
                                    //console.log('hijos2', '------' + element2.data('menu1'))

                                    var existeMenu2 = jQuery.inArray(menu2, menus);
                                    if (existeMenu2 >= 0) {
                                        check = "checked";
                                    } else {
                                        check = "";
                                    }

                                    tr = tr + '<tr class="' + menu1 + '">' +
                                        '<td> <span style="color:blue !important;background-color:transparent !important;padding-left: 20px;" class="glyphicon glyphicon-arrow-right"></span> ' + titulo2 + '</td>' +
                                        '<td>' +
                                        '<label style="float:right"><input type="checkbox" ' + check + ' data-tit="' + titulo2 + '" value="' + menu2 + '" name="square-checkbox"></label>' +
                                        '</td>' +
                                        '</tr>';
                                });
                            }
                            else {
                                //console.log('hijo1', '----' + element1.data('menu1'))
                                var existeMenu1 = jQuery.inArray(menu1, menus);
                                if (existeMenu1 >= 0) {
                                    check = "checked";
                                } else {
                                    check = "";
                                }

                                tr = tr + '<tr>' +
                                    '<td style="font-weight: bolder;"><span style="color:red !important;background-color:transparent !important" class="glyphicon glyphicon-star"></span> ' + titulo1 + '</td>' +
                                    '<td>' +
                                    '<label style="float:right"><input type="checkbox"  ' + check + ' data-tit="' + titulo1 + '" value="' + menu1 + '" name="square-checkbox"></label>' +
                                    '</td>' +
                                    '</tr>';
                            }
                        });
                    }


                    var existeMenu = jQuery.inArray(menu, menus);
                    if (existeMenu >= 0) {
                        check = "checked";
                    } else {
                        check = "";
                    }


                    $('#libody').append('<li class="highlight-color-' + color[i] + '  highlight-color-' + color[i] + '-icon">' +

                        '<span class="' + icon + '"></span>' +
                        '<div class="c_tmlabel">' +
                        '<div class="c_tmlabel_inner collaps">' +
                        '<h2>' + titulo + '<label style="float:right"><input type="checkbox" data-tit="' + titulo + '" data-principal="1" value="' + menu + '"  ' + check + ' name="square-checkbox"></label></h2>' +
                        '<table class="table table-condensed table-hover">' +
                        '<tbody>' +
                        tr +
                        '</tbody>' +
                        '</table>' +
                        '</div>' +
                        '</div>' +
                        '</li>');

                    datamenus.push(menu);
                    if (total == i) {
                        $("#libody").iCheck({
                            checkboxClass: 'icheckbox_square-blue',
                            radioClass: 'iradio_square-red',
                            increaseArea: '2%' // optional
                        });

                        var data = { dataMenu: datamenus, rolid: rol }
                        var url = basePath + "seguridad/ListadoFechasPrincipales";
                        ListadoPrincipales(url, data, false);
                    }
                });

            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {

        }
    });

}

//////////////////////////////////////rolessegundotab/////

function ListadoRolestab() {
    var url = basePath + "BotPermisos/ObtenerAcciones";
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify({}),
        contentType: "application/json",
        success: function (response) {
            console.log('racitooooo')
            var roles = response.data;
            if (roles) {
                $("#listaRoles").html("");
                $.each(roles, function (index, value) {
                    $("#listaRoles").append('<li class="" data-id="' + value.Valor + '">' + '<a href="#">' + value.Nombre + '</a>' + '</li>');
                });
            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        }
    });
}

function ListadoPermisoRol(rolid) {
    var url = basePath + "Seguridad/ListadoControladorPermisos";
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify({ rolid: rolid }),
        contentType: "application/json",
        beforeSend: function () {
            $.LoadingOverlay("show");
            $("#contenidopermisos").hide();
            $("#alertapermisos").text("Cargando Permisos del Rol ...");
            $("#alertapermisos").show();

        },
        success: function (response) {
            var controlador = response.controlador;
            var permisosControlador = response.listaPermisoControlador;
            var permisosRol = response.listaPermisosRol;

            if (controlador) {
                $("#bodyPermisosRoles").html("");

                $.each(controlador, function (index, value) {

                    var permisosLista = "";
                    var cantPerm = "0";
                    var permisosChek = "0";
                    $.each(permisosControlador, function (index, valuePC) {
                        var check = "";
                        if (value.WEB_PermControlador == valuePC.WEB_PermControlador) {
                            cantPerm = Number(cantPerm) + 1;
                            var nombrePermiso = valuePC.WEB_PermNombreR ? valuePC.WEB_PermNombreR : valuePC.WEB_PermNombre;

                            $.each(permisosRol, function (key, valuePR) {
                                if (valuePR.WEB_RolID == rolid && valuePR.WEB_PermID == valuePC.WEB_PermID) {
                                    check = "checked";
                                    permisosChek = Number(permisosChek) + 1;
                                }
                            });

                            permisosLista += '<li class="task-list-item">' +
                                '<div class="checkbox"><label>' +
                                '<input id="' + valuePC.WEB_PermID + '_' + rolid + '" ' + check + ' data-todos="0" data-id="' + rolid + "_" + value.WEB_PermControlador + "_" + valuePC.WEB_PermID + '"  type="checkbox" class="task-list-item-checkbox"/> ' +
                                nombrePermiso +
                                '</label></div>' +
                                '</li>';

                            //console.log(nombrePermiso,"_"+value.WEB_PermControlador)
                        };
                    });
                    var todos = "";
                    if (Number(cantPerm) > 0) {
                        if (cantPerm == permisosChek) {
                            todos = "checked";
                        };
                    }
                    var border = "";
                    if (permisosChek != cantPerm) {
                        border = 'border-bottom:2px solid red';
                    }
                    $("#bodyPermisosRoles").append('<div class="col-md-4" style="padding-right: 4px;padding-left: 4px;padding-bottom: 4px;">' +
                        '<div class= "panel panel-success">' +
                        '<div class="panel-heading">' +
                        '<h4 class="panel-title">' +
                        '<a style="font-size: 14px;" class="collapsed" data-toggle="collapse"  href="#' + value.WEB_PermControlador + '">' +
                        value.WEB_PermControlador.replace("Controller", "") +
                        '<span class="icon icon-arrow-down" style="margin-top: -3px;"></span>' +
                        '<span class="badge pull-right" style="margin-right: 5px;margin-top: 0px;width: 46px;' + border + '">' +
                        '<span id="cant_' + value.WEB_PermControlador + '"  >' + permisosChek + '</span> / ' + cantPerm + '</span>' +
                        '</a>' +
                        '</h4>' +
                        '</div>' +
                        '<div id="' + value.WEB_PermControlador + '" class="panel-collapse collapse">' +
                        '<div class="panel-body taskPanel" style="text-transform: uppercase;padding-top: 5px;">' +
                        '<div class="row"><div class="checkbox"> ' +
                        '<label><input type="checkbox" ' + todos + ' id="check_control_' + value.WEB_PermControlador + '" data-todos="1" data-id="' + rolid + "_" + value.WEB_PermControlador + '_" class="task-list-item-checkbox" /> Seleccionar Todos</label>' +
                        '</div></div>' +
                        '<hr style="margin-top:0px;margin-bottom:10px" />' +
                        '<ul class="task-list">' + permisosLista +
                        '</ul>' +
                        '</div>' +
                        '</div>' +
                        '</div>' +
                        '</div>');
                });

                totales();
            }
            else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }

        },
        complete: function () {
            $("#contenidopermisos").show();
            $("#alertapermisos").hide();
            $("ul.task-list").mCustomScrollbar({
                autoHideScrollbar: true,
                scrollbarPosition: "outside",
                theme: "dark",
                setHeight: "210px"
            });

            $("#tab2Permisos").iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '2%' // optional
            });
            $.LoadingOverlay("hide");
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }
    });
}

$(document).on('ifChecked', '#tab2Permisos input:checkbox', function (event) {

    var todos = jQuery(this).data("todos");
    var permiso = [];
    var toaster = "";
    if (todos == "2") {
        $("#lblesperando").show();
        $("#lblcheck").hide();
        $.LoadingOverlay("show");
        setTimeout(function () {
            $("#tab2Permisos").iCheck("destroy");
            $('#tab2Permisos ul.task-list').each(function () {
                $(this).find('input:checkbox:not(:checked)').each(function () {
                    var idinputdata = jQuery(this).data("id");
                    var ids = idinputdata.split("_");
                    var idrolcheck = ids[0];
                    var controladorcheck = ids[1];
                    var idpermisocheck = ids[2];
                    permiso.push({ WEB_RolID: idrolcheck, WEB_PermID: idpermisocheck });
                    jQuery(this).click();
                    var cant = $("#cant_" + controladorcheck).text();
                    $("#cant_" + controladorcheck).text(parseInt(cant) + 1);
                });

            });
            $('input[data-todos="1"]').prop('checked', true);
            $("#tab2Permisos").iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '2%' // optional
            });
            toaster = 0;
            $(".badge.pull-right").css("border-bottom", "0px");
            $.LoadingOverlay("hide");
            if (permiso.length > 0) {
                var url = basePath + "Seguridad/AgregarPermisoRol";
                DataPostWithoutChangePermiso(url, permiso, toaster);
            }
            $("#lblesperando").hide();
            $("#lblcheck").show();

        }, 100);



    }
    else {
        var id = jQuery(this).data("id");
        var ids = id.split("_");
        var idrol = ids[0];
        var controlador = ids[1];
        var idpermiso = ids[2];
        if (todos == "0") {
            permiso.push({ WEB_RolID: idrol, WEB_PermID: idpermiso });
            var cant = $("#cant_" + controlador).text();
            $("#cant_" + controlador).text(parseInt(cant) + 1);
            var nocheked = $('#' + controlador + ' ul.task-list input:checkbox:not(:checked)').length;
            if (nocheked == "0") {
                $("#check_control_" + controlador).iCheck("destroy");
                $("#check_control_" + controlador).click();
                $("#check_control_" + controlador).iCheck({
                    checkboxClass: 'icheckbox_square-blue',
                    radioClass: 'iradio_square-red',
                    increaseArea: '2%' // optional
                });
            }
            toaster = 2;
        }
        if (todos == "1") {
            $("#" + controlador).iCheck("destroy");
            $('#' + controlador + ' ul.task-list input:checkbox:not(:checked)').each(function () {
                var idinputdata = jQuery(this).data("id");
                var ids = idinputdata.split("_");
                var idrolcheck = ids[0];
                var controladorcheck = ids[1];
                var idpermisocheck = ids[2];
                permiso.push({ WEB_RolID: idrolcheck, WEB_PermID: idpermisocheck });
                jQuery(this).click();
                var cant = $("#cant_" + controladorcheck).text();
                $("#cant_" + controladorcheck).text(parseInt(cant) + 1);

            });
            $("#" + controlador).iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '2%' // optional
            });
            toaster = 1;
        }

        if (permiso.length > 0) {
            var url = basePath + "Seguridad/AgregarPermisoRol";
            DataPostWithoutChangePermiso(url, permiso, toaster);
        }
    };



    //console.log(permiso)
});

$(document).on('ifUnchecked', '#tab2Permisos input:checkbox', function (event) {

    var todos = jQuery(this).data("todos");
    var permiso = [];
    var toaster = "";
    if (todos == "2") {
        $("#lblesperando").show();
        $("#lblcheck").hide();
        $.LoadingOverlay("show");
        setTimeout(function () {

            $("#tab2Permisos").iCheck("destroy");
            $('#tab2Permisos ul.task-list').each(function () {
                $(this).find('input:checkbox:checked').each(function () {
                    var idinputdata = jQuery(this).data("id");
                    var ids = idinputdata.split("_");
                    var idrolcheck = ids[0];
                    var controladorcheck = ids[1];
                    var idpermisocheck = ids[2];
                    permiso.push({ WEB_RolID: idrolcheck, WEB_PermID: idpermisocheck });
                    jQuery(this).click();
                    var cant = $("#cant_" + controladorcheck).text();
                    $("#cant_" + controladorcheck).text(parseInt(cant) - 1);
                });

            });
            $('input[data-todos="1"]').prop('checked', false);
            $("#tab2Permisos").iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '2%' // optional
            });
            toaster = 3;

            $(".badge.pull-right").css("border-bottom", "2px solid red");
            $.LoadingOverlay("hide");
            if (permiso.length > 0) {
                var url = basePath + "Seguridad/QuitarPermisoRol";
                DataPostWithoutChangePermiso(url, permiso, toaster);
            }
            $("#lblesperando").hide();
            $("#lblcheck").show();
        }, 100);



    }
    else {
        var id = jQuery(this).data("id");
        var ids = id.split("_");
        var idrol = ids[0];
        var controlador = ids[1];
        var idpermiso = ids[2];
        if (todos == "0") {
            permiso.push({ WEB_RolID: idrol, WEB_PermID: idpermiso });
            var cant = $("#cant_" + controlador).text();
            $("#cant_" + controlador).text(parseInt(cant) - 1);
            var estado = $("#check_control_" + controlador).is(':checked');
            if (estado == true) {
                $("#check_control_" + controlador).iCheck("destroy");
                $("#check_control_" + controlador).click();
                $("#check_control_" + controlador).iCheck({
                    checkboxClass: 'icheckbox_square-blue',
                    radioClass: 'iradio_square-red',
                    increaseArea: '2%' // optional
                });
            }
            toaster = 5;
        }
        if (todos == "1") {
            $("#" + controlador).iCheck("destroy");
            $('#' + controlador + ' ul.task-list input:checkbox:checked').each(function () {
                var idinputdata = jQuery(this).data("id");
                var ids = idinputdata.split("_");
                var idrolcheck = ids[0];
                var controladorcheck = ids[1];
                var idpermisocheck = ids[2];
                permiso.push({ WEB_RolID: idrolcheck, WEB_PermID: idpermisocheck });
                jQuery(this).click();
                var cant = $("#cant_" + controladorcheck).text();
                $("#cant_" + controladorcheck).text(parseInt(cant) - 1);

            });
            $("#" + controlador).iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '2%' // optional
            });
            toaster = 4;
        };

        if (permiso.length > 0) {
            var url = basePath + "Seguridad/QuitarPermisoRol";
            DataPostWithoutChangePermiso(url, permiso, toaster);
        }
    };


    //console.log(permiso)
});

function DataPostWithoutChangePermiso(url, data, toaster) {

    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            var respuesta = response.respuesta;
            //console.log(respuesta,"ase")
            if (respuesta === true) {
                if (toaster == 0) {
                    toastr.success("Se Asigno Todos los Permisos", "Mensaje Servidor");
                }
                if (toaster == 1) {
                    toastr.success("Se Asigno Todos los Permisos de Bloque", "Mensaje Servidor");
                }
                if (toaster == 2) {
                    toastr.success("Se Asigno Permiso", "Mensaje Servidor");
                }

                if (toaster == 3) {
                    toastr.warning("Se Quito Todos los Permisos", "Mensaje Servidor");
                }

                if (toaster == 4) {
                    toastr.warning("Se Quito Todos los Permisos del Bloque", "Mensaje Servidor");
                }

                if (toaster == 5) {
                    toastr.warning("Se Quito el Permiso Asignado", "Mensaje Servidor");
                }

            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
        complete: function () {

            $.LoadingOverlay("hide");
            totales();
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }
    });

}

function totales() {
    var totalno = $('#tab2Permisos ul.task-list input:checkbox:not(:checked)').length;
    var total = $('#tab2Permisos ul.task-list input:checkbox').length;
    var totalsi = $('#tab2Permisos ul.task-list input:checkbox:checked').length;
    $("#fullall").iCheck("destroy");
    if (totalno == "0") {
        $("#fullall").prop('checked', true);
        $("#divfaltanTotal").hide();

    } else {
        $("#fullall").prop('checked', false);
        $("#totalPermisosSpanFaltan").text(totalno);
        $("#divfaltanTotal").show();
    }
    $("#fullall").iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-red',
        increaseArea: '2%' // optional
    });
    $("#totalPermisosSpan").text(totalsi + "/" + total);
}

$('.inputBuscar').on('keyup', function (ev) {
    var texto = $.trim($(this).val());
    searchTable(texto);
});

function searchTable(inputVal) {
    var listado = $('#bodyPermisosRoles .task-list li');
    if (inputVal) {
        listado.hide();
        $("#lblcheck").hide();
        $("#bodyPermisosRoles .col-md-4").hide();
        var controllers = $("#bodyPermisosRoles .col-md-4");
        controllers.each(function (index, div) {
            $(this).find("h4").find("a").removeClass();
            $(this).find("h4").find("a").addClass("collapsed");
            $(this).find("div.row").hide();
            $(this).find("div.panel-collapse.collapse.in").removeClass().addClass("panel-collapse collapse").css("height", "0px");
        });

        var len = $('#bodyPermisosRoles .task-list li label:contains("' + inputVal + '")').length;
        if (len > 0) {
            $(".lblNo").remove();
            $('#bodyPermisosRoles .task-list li label:contains("' + inputVal + '")').each(function () {
                var li = $(this).parent().parent();
                li.show();
                var colmd4 = $(this).parent().parent().parent().parent().parent().parent().parent().parent().parent();
                colmd4.show();
                colmd4.find("h4").find("a").removeClass();
                colmd4.find(".panel-collapse.collapse").removeClass().addClass("panel-collapse collapse in").removeAttr("style");
            });
        } else {
            $('#bodyPermisosRoles').append('<label class="alert alert-danger lblNo" style="margin-bottom:0px;margin-top:0px;width:100%;padding:2px">No se Encontraron Permisos</label>');
        }
    }
    else {
        console.log("vacio");
        $(".lblNo").remove();
        listado.show();
        $("#lblcheck").show();
        $('#bodyPermisosRoles .task-list li').show();
        $('#bodyPermisosRoles .taskPanel div.row').show();
        var element = $(".panel-collapse");
        element.removeClass();
        element.css("height", "");
        element.addClass("panel-collapse collapse");
        element.parent().parent().show();
    }
}

$.expr[":"].contains = $.expr.createPseudo(function (arg) {
    return function (elem) {
        return $(elem).text().toUpperCase().indexOf(arg.toUpperCase()) >= 0;
    };
});

//////////////////////////////////////rolesusuario/////

$(document).on('change', '#tableUsuRol select', function (event) {
    var idusuario = jQuery(this).data("usuid");
    var idRol = jQuery(this).val();
    if (idRol != "") {
        var data = { WEB_RolID: idRol, UsuarioID: idusuario }
        var url = basePath + "RolUsuario/GuardarRolUsuario";
        DataPostWithoutChange(url, data, false);
    } else {
        toastr.error("Seleccione un Rol,para Registrar", "Mensaje Servidor");
    }

});

function listausuarios() {
    var url = basePath + "Rolusuario/ListadoTableUsuarioAsignarRol";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({}),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            var roles = response.roles;
            var usuarios = response.usuarios;
            var rolUsuarios = response.rolUsuarios;

            $("#tableUsuRol").on('page.dt', function () {
                // console.log('Page');
                //$('table').css('height', '250px');
                $('.dataTables_scrollBody').css('height', '250px');
            }).DataTable({
                "bDestroy": true,
                "bSort": true,
                "paging": true,
                "scrollX": false,
                "sScrollX": "100%",
                //"sScrollY": "250px",
                "scrollCollapse": true,
                "bProcessing": true,
                "bDeferRender": true,
                "autoWidth": false,
                "bAutoWidth": true,
                "lengthMenu": [[10, 50, 200, -1], [10, 50, 200, "All"]],
                "pageLength": 10,
                data: usuarios,
                columns: [
                    { data: "UsuarioNombre", title: "Usuario", "width": "250px" },
                    { data: "NombreEmpleado", title: "Nombre Empleado" },
                    {
                        data: "UsuarioID", title: "Rol Usuario",
                        "render": function (usuarioId) {
                            var rolesAsignados = [];
                            $.each(roles, function (key, rol) {
                                $.each(rolUsuarios, function (index, rolUsuario) {
                                    if (rolUsuario.UsuarioID == usuarioId && rolUsuario.WEB_RolID == rol.WEB_RolID) {
                                        var rolName = rol.WEB_RolNombre;
                                        rolesAsignados.push(rolName);
                                    }
                                });
                            });

                            return rolesAsignados.join(', ');
                        }
                    },
                    {
                        data: "UsuarioID", title: "Rol", "width": "150px",
                        "render": function (o) {
                            var selectedOptions = "";
                            selectedOptions += '<option value="">--Seleccione--</option>';
                            $.each(roles, function (keyr, valuer) {
                                var seleccion = "";
                                $.each(rolUsuarios, function (key, value2) {
                                    if (value2.UsuarioID == o && value2.WEB_RolID == valuer.WEB_RolID) {
                                        seleccion = "selected";
                                    }
                                });

                                selectedOptions += '<option ' + seleccion + '  value="' + valuer.WEB_RolID + '">' + valuer.WEB_RolNombre + '</option>';
                            });

                            return '<select data-usuid="' + o + '" style="font-weight: bolder;" class="form-control input-sm selectEmp">' + selectedOptions + '</select>';
                        }
                    }
                ],
                columnDefs: [
                    {
                        targets: 2,
                        visible: false
                    },
                    {
                        targets: 3,
                        searchable: false
                    }
                ]
            });

            $('table').css('width', '100%');
            $('.dataTables_scrollHeadInner').css('width', '100%');
            $('.dataTables_scrollFootInner').css('width', '100%');
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {

        }
    });

};

$("[data-toggle='tooltip']").tooltip();
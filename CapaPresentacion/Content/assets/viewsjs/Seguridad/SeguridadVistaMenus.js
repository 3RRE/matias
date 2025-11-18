// Seguridad Vista Menus JS

function getMenuRoleKey(rol) {
    if (rol != 0) {
        rol = $("#cboRol_").val();
    } else {
        rol = rolid;
    }

    var keys = [];
    var data = { rolId: rol }
    $.ajax({
        url: `${basePath}/Seguridad/ListadoMenusRolId`,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            var listado = response.dataResultado;
            if (listado.length > 0) {
                $.each(listado, function (index, value) {
                    var key = value.WEB_PMeDataMenu
                    keys.push(key)
                });
            }
        },
        complete: function () {
            getMenuPermissions(keys)
            $.LoadingOverlay("hide")
        }
    });
}

function getMenuPermissions(keys) {
    var fileHash = ((+new Date) + Math.random() * 100).toString(32)
    $.ajax({
        url: `${basePath}/Content/menu-square/menu.json?v=${fileHash}`,
        type: "GET",
        contentType: "application/json",
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            response.shift()
            $("#menuPermissions").html("");
            $('#menuPermissions').append(itemMenuPermission(response, keys))

            $("#menuPermissions").iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '2%'
            });
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    });
}

function itemMenuPermission(response, keys) {
    var item = ''
    var items = ''

    response.map(function (menu, index) {

        var checked = keys.includes(menu.key) ? 'checked' : ''

        if (menu.submenu) {
            menu.submenu.map(function (submenu) {

                items += itemSubMenuPermission(menu.key, submenu, 2, keys)

                if (submenu.submenu) {
                    submenu.submenu.map(function (subsubmenu) {

                        items += itemSubMenuPermission(submenu.key, subsubmenu, 3, keys)

                    })
                }
            })
        }

        var menuColors = ['#EB4747', '#4B7BE5', '#83BD75', '#FFC54D', '#646FD4', '#2FA4FF', '#139487', '#FF9F45', '#B8405E', '#2F3A8F', '#ECB365', '#519259', '#EB1D36', '#1C3879', '#18978F', '#9EB23B', '#2155CD', '#FF8C32', '#00C897', '#B762C1', '#FF5959']
        var menuColor = menuColors[index]

        item += `
                <li class="item-menu-permission item-menu-flex" data-key="${menu.key}" data-parent="parent">
                    <span class="msicon ${menu.iconClass}" style="background-color:${menuColor}" data-toggle="collapse" data-target="#collapse${menu.key}" aria-expanded="false"></span>
                    <div class="ms-collapse-wrap" style="border-right-color:${menuColor}">
                        <div class="ms-header-collapse" style="border-bottom-color:${menuColor}">
                            <div class="ms-hc-title" data-toggle="collapse" data-target="#collapse${menu.key}" aria-expanded="false">
                                <h3>${menu.name}</h3>
                            </div>
                            <div class="ms-hc-checkbox">
                                <input class="msi-checkbox" name="square-checkbox" type="checkbox" ${checked} value="${menu.key}" data-name="${menu.name}" data-level="1" />
                            </div>
                        </div>
                        <div class="collapse" id="collapse${menu.key}">
                            <div class="item-menu-permission--items" data-key="${menu.key}">
                                <div class="well table-responsive">
                                    <table class="table table-condensed table-hover">
                                        <tbody>${items}</tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </li>
                `

        items = ''
    })

    return item
}

function itemSubMenuPermission(keyParent, menu, level, keys) {

    var checked = keys.includes(menu.key) ? 'checked' : ''

    var trClass = 'subitem-menu-permission'
    var iconClasItem = 'glyphicon glyphicon-star'
    var spanStyleItem = 'color:red !important;background-color:transparent !important'

    if (level == 3) {
        trClass = 'subsubitem-menu-permission'
        iconClasItem = 'glyphicon glyphicon-arrow-right'
        spanStyleItem = 'color:blue !important;background-color:transparent !important;padding-left: 20px;'
    }

    var item = `
            <tr class="${trClass}" data-key="${menu.key}" data-parent="${keyParent}">
                <td style="font-weight: bolder;">
                    <span style="${spanStyleItem}" class="${iconClasItem}"></span> ${menu.name}
                </td>
                <td>
				    <label style="float:right">
					    <input class="msi-checkbox" name="square-checkbox" type="checkbox" ${checked} value="${menu.key}" data-name="${menu.name}" data-level="${level}" />
                    </label>
				</td>
            </tr>
            `

    return item
}

$(this).off('ifChecked', '#menuPermissions input.msi-checkbox')
$(document).on('ifChecked', '#menuPermissions input.msi-checkbox', function (event) {
    var roleId = $("#cboRol_").val()
    var menuKey = $(this).val()
    var menuName = $(this).data("name");
    var level = $(this).data("level")

    var data = {
        WEB_RolID: roleId,
        WEB_PMeDataMenu: menuKey,
        WEB_PMeNombre: menuName,
        WEB_PMeEstado: 1
    }

    var permissions = []

    permissions.push(data)

    if (level == 1) {

        var elements = $(`.item-menu-permission--items[data-key="${menuKey}"] input.msi-checkbox:checkbox:not(:checked)`)

        elements.each(function (index, element) {

            permissions.push({
                WEB_RolID: roleId,
                WEB_PMeDataMenu: $(element).val(),
                WEB_PMeNombre: $(element).data("name"),
                WEB_PMeEstado: 1
            })

            $(element).prop('checked', true).iCheck('update')
        })
    }

    if (level == 2) {

        var elements = $(`.subsubitem-menu-permission[data-parent="${menuKey}"] input.msi-checkbox:checkbox:not(:checked)`)

        elements.each(function (index, element) {

            permissions.push({
                WEB_RolID: roleId,
                WEB_PMeDataMenu: $(element).val(),
                WEB_PMeNombre: $(element).data("name"),
                WEB_PMeEstado: 1
            })

            $(element).prop('checked', true).iCheck('update')
        })
    }

    // send permissions
    var url = `${basePath}/Seguridad/AgregarPermisoMenu`
    sendDataPermissions(url, permissions, 1)
})

$(this).off('ifUnchecked', '#menuPermissions input.msi-checkbox')
$(document).on('ifUnchecked', '#menuPermissions input.msi-checkbox', function (event) {
    var roleId = $("#cboRol_").val()
    var menuKey = $(this).val()
    var menuName = $(this).data("name");
    var level = $(this).data("level")

    var data = {
        WEB_RolID: roleId,
        WEB_PMeDataMenu: menuKey,
        WEB_PMeNombre: menuName,
        WEB_PMeEstado: 0
    }

    var permissions = []

    permissions.push(data)

    if (level == 1) {

        var elements = $(`.item-menu-permission--items[data-key="${menuKey}"] input.msi-checkbox:checkbox:checked`)

        elements.each(function (index, element) {

            permissions.push({
                WEB_RolID: roleId,
                WEB_PMeDataMenu: $(element).val(),
                WEB_PMeNombre: $(element).data("name"),
                WEB_PMeEstado: 0
            })

            $(element).prop('checked', false).iCheck('update')
        })
    }

    if (level == 2) {

        var elements = $(`.subsubitem-menu-permission[data-parent="${menuKey}"] input.msi-checkbox:checkbox:checked`)

        elements.each(function (index, element) {

            permissions.push({
                WEB_RolID: roleId,
                WEB_PMeDataMenu: $(element).val(),
                WEB_PMeNombre: $(element).data("name"),
                WEB_PMeEstado: 0
            })

            $(element).prop('checked', false).iCheck('update')
        })
    }

    // send permissions
    var url = `${basePath}/Seguridad/QuitarPermisoMenu`
    sendDataPermissions(url, permissions, 0)
})

// send data permissions
function sendDataPermissions(url, permissions, checked) {

    var totalPermissions = permissions.length

    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(permissions),
        contentType: "application/json",
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            var respuesta = response.respuesta;
            if (respuesta === true) {
                if (checked == 1) {
                    var messageText = totalPermissions > 1 ? 'Se Asignaron los Permisos' : 'Se Asigno el Permiso'
                    toastr.success(messageText, "Mensaje Servidor");
                } else if (checked == 0) {
                    var messageText = totalPermissions > 1 ? 'Se Quitaron los Permisos Asignados' : 'Se Quito el Permiso Asignado'
                    toastr.warning(messageText, "Mensaje Servidor");
                }

            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")
        },
        error: function () {
            toastr.error("Error Servidor", "Mensaje Servidor");
        }
    })
}
// Seguridad Vista Permisos JS

document.addEventListener('DOMContentLoaded', function () {
    new Vue({
        el: '#app-permission',
        data: {
            roles: [],
            roleSelected: -1,
            roleSearch: '',
            controllers: [],
            permissionSearch: '',
            allChecked: false,
            loadingControl: true
        },
        computed: {
            filteredRoles: function () {
                return this.roles.filter(role => {
                    return role.name.toLowerCase().includes(this.roleSearch.toLowerCase())
                })
            },
            filteredPermissions: function () {
                return this.controllers.map((controller) => {
                    return { ...controller, actions: controller.actions.filter((action) => action.name.toLowerCase().includes(this.permissionSearch.toLowerCase()) || action.controller.replace('Controller', '').toLowerCase().includes(this.permissionSearch.toLowerCase())) }
                }).filter(controller => controller.actions.length > 0)
            },
            getRole: function () {
                return this.roles.find(role => role.id == this.roleSelected)
            }
        },
        methods: {
            getSecurityRoles: function () {
                var self = this
                var newRoles = []
                $.ajax({
                    url: `${basePath}BotPermisos/ObtenerAcciones`,
                    type: "GET",
                    contentType: "application/json",
                    beforeSend: function () {
                        $.LoadingOverlay("show")
                    },
                    success: function (response) {
                        var roles = response.data
                        if (roles) {
                            roles.map(function (role) {
                                newRoles.push({
                                    id: role.Valor,
                                    name: role.Texto,
                                    description: '',
                                    status: ''
                                })
                            })
                        } else {
                            toastr.error(response.displayMessage, "Mensaje Servidor")
                        }
                    },
                    error: function () {
                        toastr.error("Error Servidor", "Mensaje Servidor")
                    },
                    complete: function () {
                        self.roles = newRoles
                        $.LoadingOverlay("hide")
                    },
                })
            },
            getRolePermissions: function (action) {
                var self = this
                var data = { codAccion: action, rolid: action }
                $.ajax({
                    url: `${basePath}BotPermisos/ObtenerPermisosPorCodAccion`,
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify(data),
                    beforeSend: () => $.LoadingOverlay("show"),
                    complete: () => $.LoadingOverlay("hide"),
                    success: function (response) {
                        var controllers = response.data.areas
                        var controllerPermissions = response.data.cargos
                        var rolePermissions = response.data.permisosAccion

                        if (controllers) {
                            self.setRolePermissions(action, controllers, controllerPermissions, rolePermissions)
                        }
                    },
                    error: function () {
                        toastr.error("Error Servidor", "Mensaje Servidor")
                    },
                })
            },
            setRolePermissions: function (role, controllers, controllerPermissions, rolePermissions) {
                var self = this
                var data = []
                controllers.map(function (controller) {
                    var permissions = []
                    var checkeds = 0
                    controllerPermissions.map(function (controllerPermission) {
                        if (controller.Nombre == controllerPermission.NombreArea) {

                            var checked = rolePermissions.some(function (rolePermission) {
                                return rolePermission.CodAccion == role && rolePermission.IdCargo == controllerPermission.Id
                            })

                            if (checked) checkeds++

                            permissions.push({
                                id: controllerPermission.Id,
                                name: controllerPermission.Nombre,
                                description: controllerPermission.Nombre,
                                controller: controllerPermission.NombreArea,
                                status: true,
                                checked: checked
                            })
                        }
                    })

                    data.push({
                        key: controller.Nombre,
                        name: controller.Nombre,
                        actions: permissions,
                        checked: permissions.length == checkeds ? true : false
                    })
                })

                self.controllers = data
                self.loadingControl = false
            },
            setRoleSelected: function (role) {
                this.loadingControl = true
                this.controllers = []
                this.getRolePermissions(role.id)
                this.roleSelected = role.id
            },
            getTotalActions: function (controllers) {
                return controllers.reduce((accumulator, item) => accumulator + item.actions.length, 0)
            },
            getTotalActionsChecked: function (controllers) {
                return controllers.map((controller) => {
                    return { ...controller, actions: controller.actions.filter((action) => action.checked) }
                }).reduce((accumulator, item) => accumulator + item.actions.length, 0)
            },
            getTotalControlActionsChecked: function (controller) {
                return controller.actions.filter(action => action.checked).length
            },
            getTotalControlActions: function (controller) {
                return controller.actions.length
            },
            setActionChecked: function (controllerKey, actionKey, checked) {
                var controllerIndex = this.controllers.findIndex(controller => controller.key == controllerKey)
                var actionIndex = this.controllers[controllerIndex].actions.findIndex(action => action.id == actionKey)
                this.controllers[controllerIndex].actions[actionIndex].checked = checked
            },
            changeStateAllChecked: function () {
                this.allChecked = false
                if (this.getTotalActionsChecked(this.filteredPermissions) == this.getTotalActions(this.filteredPermissions)) {
                    this.allChecked = true
                }
            },
            sendDataPermissions: function (url, permissions, toaster) {
                var dataLength = permissions.length

                $.ajax({
                    url: url,
                    type: "POST",
                    data: JSON.stringify(permissions),
                    contentType: "application/json",
                    beforeSend: () => $.LoadingOverlay("show"),
                    success: function (response) {
                        var status = response.success
                        if (status) {
                            if (toaster == 1) {
                                toastr.success(`Se asignaron ${dataLength} permisos`, "Mensaje Servidor")
                            }
                            if (toaster == 2) {
                                toastr.success(`Se asignaron ${dataLength} permisos del módulo`, "Mensaje Servidor")
                            }
                            if (toaster == 3) {
                                toastr.success("Se asignó un permiso", "Mensaje Servidor")
                            }
                            if (toaster == 4) {
                                toastr.warning(`Se denegaron ${dataLength} permisos`, "Mensaje Servidor")
                            }
                            if (toaster == 5) {
                                toastr.warning(`Se denegaron ${dataLength} permisos del módulo`, "Mensaje Servidor")
                            }
                            if (toaster == 6) {
                                toastr.warning("Se denegó un permiso", "Mensaje Servidor")
                            }
                        } else {
                            toastr.error(response.displayMessage, "Mensaje Servidor")
                        }
                    },
                    complete: function () {
                        $.LoadingOverlay("hide")
                    }
                })
            },
            isFullFilterChecked: function (filteredPermissions) {
                var isFull = this.getTotalActionsChecked(filteredPermissions) === this.getTotalActions(filteredPermissions)

                return isFull
            },
            isFullChecked: function (controller) {
                var isFull = this.getTotalControlActionsChecked(controller) === this.getTotalControlActions(controller)

                return isFull
            },
            checkedFilterStatus: function (filteredPermissions) {

                var checkeds = this.getTotalActionsChecked(filteredPermissions)
                var all = this.getTotalActions(filteredPermissions)

                return this.checkedLabel(checkeds, all)
            },
            checkedStatus: function (controller) {

                var checkeds = this.getTotalControlActionsChecked(controller)
                var all = this.getTotalControlActions(controller)

                return this.checkedLabel(checkeds, all)
            },
            checkedLabel: function (checkeds, all) {
                return {
                    'rphf-danger': checkeds === 0,
                    'rphf-warning': checkeds > 0 && checkeds < all,
                    'rphf-success': checkeds === all
                }
            },
            getTotalItems: function () {
                return this.controllers.reduce((accumulator, item) => accumulator + item.actions.length, 0)
            },
            getTotalItemsChecked: function () {
                return this.controllers.map((controller) => {
                    return { ...controller, actions: controller.actions.filter((action) => action.checked) }
                }).reduce((accumulator, item) => accumulator + item.actions.length, 0)
            },
            checkedFullStatus: function () {

                var checkeds = this.getTotalItemsChecked()
                var all = this.getTotalItems()

                return this.checkedLabel(checkeds, all)
            }
        },
        created: function () {
            //
        },
        watch: {
            controllers: {
                handler: function () {
                    this.controllers.map(controller => {
                        var checkeds = controller.actions.filter(action => action.checked)
                        controller.checked = controller.actions.length == checkeds.length ? true : false
                    })

                    this.filteredPermissions.map(controller => {
                        var checkeds = controller.actions.filter(action => action.checked)
                        controller.checked = controller.actions.length == checkeds.length ? true : false
                    })

                    this.changeStateAllChecked()
                },
                deep: true
            },
            permissionSearch: {
                handler: function () {
                    this.filteredPermissions.map(controller => {
                        var checkeds = controller.actions.filter(action => action.checked)
                        controller.checked = controller.actions.length == checkeds.length ? true : false
                    })

                    this.changeStateAllChecked()
                }
            }
        },
        mounted: function () {
            this.getSecurityRoles()
        },
        directives: {
            icheck: {
                inserted: function (el, binding, vnode) {
                    var context = vnode.context

                    // iCheck
                    $(el).iCheck({
                        checkboxClass: 'icheckbox_square-blue'
                    })

                    // ifChecked
                    $(el).on('ifChecked', function (event) {

                        var input = event.target
                        var layer = $(input).attr('data-layer')
                        var checked = true
                        var permissions = []
                        var url = `${basePath}/BotPermisos/CrearPermisos`

                        if (layer == 1) {
                            $.confirm({
                                title: `Hola`,
                                content: `¿Estás seguro de asignar todos los permisos para el rol de <b>${context.getRole.name}</b>?`,
                                confirmButton: 'Sí, asignar',
                                cancelButton: 'Cancelar',
                                confirmButtonClass: 'btn-success',
                                confirm: function () {
                                    //#region start level 1
                                    var controllersElement = $(`.controller-permission---item input:checkbox:not(:checked)`)

                                    controllersElement.each(function (index, element) {
                                        var controller = $(element).attr('data-controller')
                                        var elements = $(`.action-permission---item[data-parent="${controller}"] input:checkbox:not(:checked)`)

                                        elements.each(function (key, item) {
                                            permissions.push({
                                                codAccion: $(item).attr('data-role'),
                                                idCargo: $(item).attr('data-permission')
                                            })

                                            $(item).prop('checked', checked).iCheck('update')

                                            context.setActionChecked($(item).attr('data-controller'), $(item).attr('data-permission'), checked)
                                        })

                                        $(element).prop('checked', checked).iCheck('update')
                                    })

                                    $(input).prop('checked', checked).iCheck('update')

                                    // send permissions
                                    context.sendDataPermissions(url, permissions, 1)
                                    //#endregion end level 1
                                },
                                cancel: function () {
                                    $(input).prop('checked', !checked).iCheck('update')
                                }
                            })
                        }

                        if (layer == 2) {
                            $.confirm({
                                title: `Hola`,
                                content: `¿Estás seguro de asignar todos los permisos del módulo <b>${$(input).attr('data-controller').replace('Controller', '')}</b> para el rol de <b>${context.getRole.name}</b>?`,
                                confirmButton: 'Sí, asignar',
                                cancelButton: 'Cancelar',
                                confirmButtonClass: 'btn-success',
                                confirm: function () {
                                    //#region start level 2
                                    var controller = $(input).attr('data-controller')
                                    var elements = $(`.action-permission---item[data-parent="${controller}"] input:checkbox:not(:checked)`)

                                    elements.each(function (index, element) {
                                        permissions.push({
                                            codAccion: $(element).attr('data-role'),
                                            idCargo: $(element).attr('data-permission')
                                        })

                                        $(element).prop('checked', checked).iCheck('update')

                                        context.setActionChecked($(element).attr('data-controller'), $(element).attr('data-permission'), checked)
                                    })

                                    $(input).prop('checked', checked).iCheck('update')

                                    // send permissions
                                    context.sendDataPermissions(url, permissions, 2)
                                    //#endregion end level 2
                                },
                                cancel: function () {
                                    $(input).prop('checked', !checked).iCheck('update')
                                }
                            })
                        }

                        if (layer == 3) {
                            //#region start level 3
                            permissions.push({
                                codAccion: $(input).attr('data-role'),
                                idCargo: $(input).attr('data-permission')
                            })

                            $(input).prop('checked', checked).iCheck('update')

                            context.setActionChecked($(input).attr('data-controller'), $(input).attr('data-permission'), checked)

                            // send permissions
                            context.sendDataPermissions(url, permissions, 3)
                            //#endregion end level 3
                        }
                    })

                    // ifUnchecked
                    $(el).on('ifUnchecked', function (event) {
                        var input = event.target
                        var layer = $(input).attr('data-layer')
                        var checked = false
                        var permissions = []
                        var url = `${basePath}/BotPermisos/EliminarPermisos`

                        if (layer == 1) {
                            $.confirm({
                                title: `Hola`,
                                content: `¿Estás seguro denegar todos los permisos para el rol de <b>${context.getRole.name}</b>?`,
                                confirmButton: 'Sí, denegar',
                                cancelButton: 'Cancelar',
                                confirmButtonClass: 'btn-danger',
                                confirm: function () {
                                    //#region start level 1
                                    var controllersElement = $(`.controller-permission---item input:checkbox:checked`)

                                    controllersElement.each(function (index, element) {
                                        var controller = $(element).attr('data-controller')
                                        var elements = $(`.action-permission---item[data-parent="${controller}"] input:checkbox:checked`)

                                        elements.each(function (key, item) {
                                            permissions.push({
                                                codAccion: $(item).attr('data-role'),
                                                idCargo: $(item).attr('data-permission')
                                            })

                                            $(item).prop('checked', checked).iCheck('update')

                                            context.setActionChecked($(item).attr('data-controller'), $(item).attr('data-permission'), checked)
                                        })
                                        $(element).prop('checked', checked).iCheck('update')
                                    })

                                    $(input).prop('checked', checked).iCheck('update')

                                    // send permissions
                                    context.sendDataPermissions(url, permissions, 4)
                                    //#endregion end level 1
                                },
                                cancel: function () {
                                    $(input).prop('checked', !checked).iCheck('update')
                                }
                            })
                        }

                        if (layer == 2) {
                            $.confirm({
                                title: `Hola`,
                                content: `¿Estás seguro denegar todos los permisos del módulo <b>${$(input).attr('data-controller').replace('Controller', '')}</b> para el rol de <b>${context.getRole.name}</b>?`,
                                confirmButton: 'Sí, denegar',
                                cancelButton: 'Cancelar',
                                confirmButtonClass: 'btn-danger',
                                confirm: function () {
                                    //#region start level 2
                                    var controller = $(input).attr('data-controller')
                                    var elements = $(`.action-permission---item[data-parent="${controller}"] input:checkbox:checked`)

                                    elements.each(function (index, element) {
                                        permissions.push({
                                            codAccion: $(element).attr('data-role'),
                                            idCargo: $(element).attr('data-permission')
                                        })

                                        $(element).prop('checked', checked).iCheck('update')
                                        context.setActionChecked($(element).attr('data-controller'), $(element).attr('data-permission'), checked)
                                    })

                                    $(input).prop('checked', checked).iCheck('update')

                                    // send permissions
                                    context.sendDataPermissions(url, permissions, 5)
                                    //#endregion end level 2
                                },
                                cancel: function () {
                                    $(input).prop('checked', !checked).iCheck('update')
                                }
                            })
                        }

                        if (layer == 3) {
                            //#region start level 3
                            permissions.push({
                                codAccion: $(input).attr('data-role'),
                                idCargo: $(input).attr('data-permission')
                            })

                            $(input).prop('checked', checked).iCheck('update')

                            context.setActionChecked($(input).attr('data-controller'), $(input).attr('data-permission'), checked)

                            // send permissions
                            context.sendDataPermissions(url, permissions, 6)
                            //#endregion end level 3
                        }
                    })
                },
                update: function (el) {
                    $(el).iCheck('update')
                }
            }
        }
    })
})

$(document).on('click', '.collapse__controller', function (event) {
    event.preventDefault()

    var element = $(this)
    var expanded = element.attr('aria-expanded')
    var rpanel = element.closest('.rpanel')

    if (expanded === "true") {
        rpanel.addClass('rpanel-in')
    } else {
        rpanel.removeClass('rpanel-in')
    }
})
//$(document).ready(function () {
//    ListarSalasCargo();
//    setCookie("datainicial", "");
//    VistaAuditoria("AlertaBilleteros/AlertaSalaBilleterosVista", "VISTA", 0, "", 3);
//    $(this).off('ifChecked', '#tabContentCargo input');
//    $(document).on('ifChecked', '#tabContentCargo input', function (event) {
//        var input = jQuery(this);
//        var alt_id = jQuery(this).data("alt_id");
//        var sala_id = jQuery(this).data("sala");
//        var cargo_id = jQuery(this).val();

//        $.ajax({
//            url: basePath + '/AlertaBilleteros/AlertaCargoGuardarJson/',
//            type: 'POST', data: JSON.stringify({ sala_id, alt_id, cargo_id}),
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            beforeSend: function (xhr) {
//                $.LoadingOverlay("show");
//            },
//            success: function (response) {
//                var id = response.id;
//                if (response.respuesta) {
//                    var datainicial = {
//                        sala_id,
//                        alt_id,
//                        cargo_id,
//                    };
//                    var datafinal = {

//                    };
//                    input.data("alt_id", id);
//                    dataAuditoriaJSON(3, "AlertaBilleteros/AlertaCargoGuardarJson", "AGREGAR ALERTA A CARGO EN SALA", datainicial, datafinal);

//                    toastr.success(response.mensaje, "Mensaje Servidor");
//                }
//                else {
//                    toastr.error(response.mensaje, "Mensaje Servidor");
//                }

//            },
//            complete: function (resul) {
//                $.LoadingOverlay("hide");
//            }
//        });
//    });

//    $(this).off('ifUnchecked', '#tabContentCargo input');
//    $(document).on('ifUnchecked', '#tabContentCargo input', function (event) {
//        var input = jQuery(this);
//        var alt_id = jQuery(this).data("alt_id");
//        var sala_id = jQuery(this).data("sala");
//        var cargo_id = jQuery(this).val();
//        if (alt_id == 0) {
//            toastr.error("No se puede Quitar,Error", "Mensaje Servidor");
//            location.reload();
//            return false;
//        }
//        $.ajax({
//            url: basePath + '/AlertaBilleteros/QuitarAlertaCargoJson',
//            type: 'POST', data: JSON.stringify({ sala_id, alt_id, cargo_id }),
//            dataType: "json",
//            contentType: "application/json; charset=utf-8",
//            beforeSend: function (xhr) {
//                $.LoadingOverlay("show");
//            },
//            success: function (response) {
//                var id = response.id;
//                if (response.respuesta) {
//                    var datainicial = {
//                        sala_id,
//                        alt_id,
//                        cargo_id,
//                    };
//                    var datafinal = {

//                    };
//                    input.data("alt_id", id);
//                    dataAuditoriaJSON(3, "AlertaBilleteros/QuitarAlertaCargoJson", "QUITAR ALERTA A CARGO EN SALA", datainicial, datafinal);

//                    toastr.success(response.mensaje, "Mensaje Servidor");
//                }
//                else {
//                    toastr.error(response.mensaje, "Mensaje Servidor");
//                }

//            },
//            complete: function (resul) {
//                $.LoadingOverlay("hide");
//            }
//        });
//    });
//});

//function ListarSalasCargo() {
//    var data = {};
//    var url = basePath + 'AlertaBilleteros/CargoSalaListarJson';
//    $.ajax({
//        url: url,
//        type: "POST",
//        contentType: "application/json",
//        data: JSON.stringify(data),
//        async: false,
//        beforeSend: function (xhr) {
//            $.LoadingOverlay("show");
//        },
//        success: function (response) {
//            var response = response.data;

//            $("#tabSala").html("");
//            $("#tabContentCargo").html("");
//            //debugger;
//            var activo = "";
//            var strippedUl = "";
//            $.each(response, function (index, valor) {
//                var cargos = valor.cargos;
//                //debugger;
//                if (index == 0) { activo = "active"; } else { activo = "";  }
//                if (index % 2 == 0) { strippedUl = "#f5f5f5" } else { strippedUl = "" }
//                $("#tabSala").append('<li style="background-color:' + strippedUl + '"  class="' + activo + '" data-id="' + valor.CodSala + '"><a style="border-bottom:1px solid #e3e3e3; border-top:unset !important;" href="#' + valor.CodSala + '_" data-toggle="tab">' + valor.Nombre + '</a></li>');
//                var cargodiv = "";
//                $.each(cargos, function (indexRS, v) {
//                    var checked = "";
//                    if (v.alt_id > 0) {
//                        checked = "checked";
//                    }

//                    cargodiv += '<div class="col-sm-6 col-md-6 col-lg-4" style="padding-right: 4px;padding-left: 4px;padding-bottom: 4px;height:60px; ">' +
//                        ' <div class="panel-heading" style="background: #fff3df;border: 1px solid #cccccc;border-radius: 5px !important; text-transform: uppercase;">' +
//                        '<label style="display:flex;align-items: center;gap:10px;"> <input ' + checked + ' type="checkbox" value= "' + v.CargoID + '" name= "cargos[]" data-sala="' + valor.CodSala + '" data-alt_id="' + v.alt_id + '"> ' + '<p style="text-align: unset;margin: unset !important;font-size:10px;text-shadow:none;font-weight: 600;">' + v.Descripcion+'</p>' + '' +
//                        '</label></div></div> ';
//                });
//                if (cargodiv == "") {
//                    cargodiv = "<p class='alert alert-danger'>No Hay Cargos Asignados</p>";
//                }
//                $("#tabContentCargo").append('<div id="' + valor.CodSala + '_" class="tab-pane ' + activo + '" data-id="' + valor.CodSala + '" data-nombre="' + valor.Nombre + '">' + cargodiv + '</div>');
//            });

//            $("#tabContentCargo").iCheck({
//                checkboxClass: 'icheckbox_square-blue',
//                radioClass: 'iradio_square-red',
//                increaseArea: '10%'
//            });

//        },
//        complete: function (resul) {
//            $.LoadingOverlay("hide");
//        }
//    });
//};


document.addEventListener('DOMContentLoaded', function () {
    var AppConfiguracionAlerta = new Vue({
        el: '#AppConfiguracionAlerta',
        data: {
            message: "raaa",
            isChecked: true,
            halls: [],
            position: [],
            hallSelect: -1,
            filtroSala: '',
            filtroCargo: '',
            alerta: 0,
            billetero: false,
            evento: false,
            verInfo: true,
            totalCargo: 0,
            agregadoCargo: 0


        },
        computed: {
            datosSalaFiltrados() {
                if (!this.filtroSala) {
                    return this.halls
                }
                const filtroLower = this.filtroSala.toLowerCase();
                console.log(this.halls)
                return this.halls.filter(dato =>
                    dato.Nombre.toLowerCase().includes(filtroLower)
                );
            },
            datosCargoFiltrados() {
                if (!this.filtroCargo) {
                    return this.position
                }
                const filtroLower = this.filtroCargo.toLowerCase();

                return {
                    ...this.position, 
                    cargos: this.position.cargos.filter(dato =>
                        dato.Descripcion.toLowerCase().includes(filtroLower)
                    ),
                };



            },
        },
        methods: {

            asignarBilletero() {
                let tipo
                if (this.alerta == 0) {
                    toastr.warning("Debe configurar un cargo a la sala para poder asignar alertas de billeteros", "Advertencia");
                } else {
                    this.billetero = !this.billetero
                    if (this.billetero && !this.evento) {
                        this.alerta = 1;
                    } else if (this.evento && !this.billetero) {
                        this.alerta = 2;
                    } else if (this.evento && this.billetero) {
                        this.alerta = 3;
                    }
                    else if (!this.evento && !this.billetero) {
                        this.alerta = 4;
                    }
                    else {
                        this.alerta = 0;
                    }
                    console.log(tipo)
                    asignarAlerta(this.hallSelect, this.alerta)
                }
                
            },
            asignarEvento() {
                let tipo

                if (this.alerta == 0) {
                    toastr.warning("Debe configurar un cargo a la sala para poder asignar alertas de evento", "Advertencia");
                } else {
                    this.evento = !this.evento
                    if (this.billetero && !this.evento) {
                        this.alerta = 1;
                    } else if (this.evento && !this.billetero) {
                        this.alerta = 2;
                    } else if (this.evento && this.billetero) {
                        this.alerta = 3;
                    } else if (!this.evento && !this.billetero) {
                        this.alerta = 4;
                    } else {
                        this.alerta = 0; 
                    }
                    asignarAlerta(this.hallSelect, this.alerta)

                }
            },

//            consultarAlertar = false;
//            if(this.position.cargos.some(objeto => objeto.isChecked === true)) {
//                consultarAlertar = true; // Si algun objeto tiene 'tra' en true, establece 'tra' en true
//}
//                console.log(consultarAlertar, "soy el consultar")
//                if (consultarAlertar == false) {
//    consultarCargoSala(this, this.hallSelect)
//}
            toggleCheckbox(positionData) {
                /* 
                 @positionData: Dato del cargo en especifico
                 @funcionamiento :  Al seleccionar un inputCheck en la vista me trae @positionData y cambio el valor de isChecked a su negacion                    
                 */

                if (positionData.isChecked ==true ) {
                    quitarCargoSala(this, this.hallSelect, positionData.CargoID)

                   

                } else {
                    agregarCargoSala(this, this.hallSelect, positionData.alt_id, positionData.CargoID, this.alerta)

                }
                positionData.isChecked = !positionData.isChecked;
                
                this.totalCargo = this.position.cargos.length;
                this.agregadoCargo = this.position.cargos.filter(item => item.isChecked === true).length;

            },
            changePOsition() {

            },
            selectHall(cargo) {
                /*
                 * @cargo: Objeto completo de sala
                 * @funcionamiento: Con @cargo obtengo el id de sala seleccionada y la guardo en hallSelect luego  busca en mi array principal(halls) 
                 * el objeto con el mismo CODSALA y lo guardo en mi arrray position 
                 * */
                this.hallSelect = cargo.CodSala
                const positionsHall = this.halls.find(hall => hall.CodSala === this.hallSelect);
                this.position = positionsHall;
                this.totalCargo = positionsHall.cargos.length;
                this.agregadoCargo = positionsHall.cargos.filter(item => item.isChecked === true).length;
                consultarCargoSala(this, this.hallSelect)
               
            },
            sincronizarOnline() {
                var self = this

                toastr.remove()

                if (!self.hallSelect || self.hallSelect == -1) {

                    toastr.warning('Por favor, seleccione una Sala')

                    return false
                }

                $.confirm({
                    title: `Hola`,
                    content: `¿Estás seguro de sincronizar los destinatarios a Web Online <b>${self.position.Nombre}</b>?`,
                    confirmButton: 'Sí, sincronizar',
                    cancelButton: 'Cancelar',
                    confirmButtonClass: 'btn-success',
                    confirm: function () {
                        sincronizarDestinatariosOnline(self, self.hallSelect)
                    }
                })
            }

        },
        created: function () {
           
        },

        mounted: function () {
            var vm = this;
            setCookie("datainicial", "");
            VistaAuditoria("AlertaBilleteros/AlertaSalaBilleterosVista", "VISTA", 0, "", 3);
            ListarSalasCargo(vm)

        },
        watch: {
           

        },


    })
})




function ListarSalasCargo(vm) {
    /**
     * Trae todas las salas con sus respectivos cargos activos o inactivos
     * */
    var data = {};
    var url = basePath + 'AlertaBilleteros/CargoSalaListarJson';
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        async: false,
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            var response = response.data;
            console.log(response);
            //vm.halls = response
            agregarCheck(vm, response)
            //vm.position=response.cargos
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};

function agregarCheck(vm, data) {
    /**
     * Recibe el this de vue con la data que se obtiene de la consulta a AlertaBilleteros/CargoSalaListarJson 
     * se agregar el campo isChecked a cada cargo de cada sala dependiendo si el alt_id es mayor a 0 (para controlar el input check)
     * */
    const newArray = data.map(item => {
        const cargos = item.cargos.map(cargo => {
            return {
                ...cargo,
                isChecked: cargo.alt_id > 0  ? true :false
            };
        });

        return {
            ...item,
            cargos: cargos
        };
    });
    vm.halls = newArray
}

function quitarCargoSala(vm,sala_id,  cargo_id) {
    console.log(sala_id, cargo_id)
    $.ajax({
        url: basePath + '/AlertaBilleteros/QuitarAlertaCargoJsonV2',
            type: 'POST', data: JSON.stringify({ sala_id, cargo_id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                var id = response.id;
                if (response.respuesta) {
                    var datainicial = {
                        sala_id,
                        cargo_id,
                    };
                    var datafinal = {

                    };
                    consultarCargoSala(vm, sala_id)

                    dataAuditoriaJSON(3, "AlertaBilleteros/QuitarAlertaCargoJson", "QUITAR ALERTA A CARGO EN SALA", datainicial, datafinal);

                    toastr.success(response.mensaje, "Mensaje Servidor");
                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }

            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
}

function agregarCargoSala(vm, sala_id, alt_id, cargo_id, tipo) {
    if (tipo == 0) {
        tipo=3
    }
    console.log(sala_id, alt_id, cargo_id)

    $.ajax({
        url: basePath + '/AlertaBilleteros/AlertaCargoGuardarJson/',
        type: 'POST', data: JSON.stringify({ sala_id, alt_id, cargo_id,tipo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            var id = response.id;
            if (response.respuesta) {
                var datainicial = {
                    sala_id,
                    alt_id,
                    cargo_id,
                };
                var datafinal = {

                };
                consultarCargoSala(vm, sala_id)
                dataAuditoriaJSON(3, "AlertaBilleteros/AlertaCargoGuardarJson", "AGREGAR ALERTA A CARGO EN SALA", datainicial, datafinal);

                toastr.success(response.mensaje, "Mensaje Servidor");
            }
            else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}


function consultarCargoSala(vm, codSala) {

    $.ajax({
        url: basePath + '/AlertaBilleteros/ConsultarAlertasCargo/',
        type: 'POST', data: JSON.stringify({ codSala }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            tipo = response.tipo;
            vm.alerta = response.tipo;

            vm.billetero = false
            vm.evento = false

            if (tipo == 0) {
                vm.billetero = false
                vm.evento = false

                }
            if (tipo == 1) {
                vm.billetero = true
                vm.evento = false

                }
            if (tipo == 2) {
                vm.evento = true
                vm.billetero = false
                }
            if (tipo == 3) {
                vm.billetero = true
                vm.evento = true

            }
           
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}



function asignarAlerta(sala_id, tipo) {

    $.ajax({
        url: basePath + '/AlertaBilleteros/CambiarTipoAlerta/',
        type: 'POST', data: JSON.stringify({ codSala:sala_id, tipo }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if (response.respuesta) {
                var datainicial = {
                    sala_id,
                };
                var datafinal = {

                };
                dataAuditoriaJSON(3, "AlertaBilleteros/AlertaCargoGuardarJson", "AGREGAR ALERTA A CARGO EN SALA", datainicial, datafinal);

                toastr.success(response.mensaje, "Mensaje Servidor");
            }
            else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function sincronizarDestinatariosOnline(vm, salaId) {
    var data = {
        salaId
    }

    $.ajax({
        type: "POST",
        url: `${basePath}/AlertaBilleteros/SincronizarDestinatariosOnline`,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message)
            } else {
                toastr.warning(response.message)
            }
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}
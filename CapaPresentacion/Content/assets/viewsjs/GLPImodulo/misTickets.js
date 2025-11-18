
document.addEventListener('DOMContentLoaded', function () {
    var AppGlpiMisTickets = new Vue({
        el: '#AppGlpiMisTickets',
        data: {
            ticketsAsignados: [],
            ticketsRecibidos: [],
            showModalDetail: false,
            historial: [],
            showModalSeguimiento: false,
            correos: [],
            correosBD: [],
            nuevoCorreo: "",
            correosFiltrados: [],
        },
        computed: {

        },
        methods: {
            obtenerDataSelectsSeguimiento() {
                var vm = this;

                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiSelects/GetSelectSeguimientoTicket",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        var datos = result.data
                        if (result.success) {
                            vm.estadosTicket = datos.estadosTicket;
                            vm.procesos = datos.procesos;
                            //vm.correos = datos.correos;
                            vm.correosBD = datos.correos;


                            console.log(vm.estadosTicket)
                            console.log(vm.procesos)
                            console.log(vm.correos)

                            const cboEstadosTicket = $('#cboEstadoTickets');
                            const cboProcesos = $('#cboProceso');
                            const cboCorreos = $('#cboCorreos');

                            cboEstadosTicket.empty();
                            cboProcesos.empty();
                            cboCorreos.empty();

                            cboEstadosTicket.append('<option value="">Seleccione una opcion</option>');
                            datos.estadosTicket.forEach((item) => {
                                cboEstadosTicket.append(`<option value="${item.Valor}">${item.Texto}</option>`);
                            });
                            cboEstadosTicket.select2({
                                placeholder: "Seleccione",
                                allowClear: true
                            });

                            cboProcesos.append('<option value="">Seleccione una opcion</option>');
                            datos.procesos.forEach((item) => {
                                cboProcesos.append(`<option value="${item.Valor}">${item.Texto}</option>`);
                            });
                            cboProcesos.select2({
                                placeholder: "Seleccione",
                                allowClear: true
                            });

                            cboCorreos.append('<option value="">Seleccione una opcion</option>');
                            datos.correos.forEach((item) => {
                                cboCorreos.append(`<option value="${item.Valor}">${item.Texto}</option>`);
                            });
                            cboCorreos.select2({
                                placeholder: "Seleccione",
                                allowClear: true
                            });
                        }
                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            getFaseDescription(codigoFase) {
                switch (codigoFase) {
                    case 1:
                        return "Creado";
                    case 2:
                        return "Asignado";
                    case 3:
                        return "En Proceso";
                    case 4:
                        return "Finalizado";
                    case 5:
                        return "Cerrado";
                    default:
                        return "Desconocido"; // Manejo de valores no esperados
                }
            },
            renderTickets(tickets) {
                return tickets;
            },
            isAtrasado(fechaTentativa) {
                const fechaActual = new Date();
                const fechaTentativaDate = new Date(parseInt(fechaTentativa.substr(6)));
                return fechaTentativaDate < fechaActual;
            },
            filtrarCorreos() {
                if (this.nuevoCorreo.length > 0) {
                    this.correosFiltrados = this.correosBD.filter(correo =>
                        correo.Texto.toLowerCase().includes(this.nuevoCorreo.toLowerCase())
                    );
                } else {
                    this.correosFiltrados = [];
                }
            },
            seleccionarCorreo(correo) {
                this.nuevoCorreo = correo;
                this.agregarCorreos();
                this.correosFiltrados = []; // Oculta sugerencias después de seleccionar
            },
            agregarCorreos() {
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!this.nuevoCorreo.trim() || !emailRegex.test(this.nuevoCorreo.trim())) {
                    toastr.error("Ingrese un correo válido.");
                    return;
                }
                if (this.correos.some(c => c.Texto === this.nuevoCorreo.trim())) {
                    toastr.warning("El correo ya está en la lista.");
                    return;
                }
                this.correos.push({ Texto: this.nuevoCorreo.trim() });
                this.nuevoCorreo = "";
                this.correosFiltrados = [];
            },
            quitarCorreos(index) {
                this.correos.splice(index, 1);
            },
            obtenerTickets() {
                var vm = this;
                $.ajax({

                    type: "POST",
                    url: basePath + "GlpiTicket/GetTicketsBySalasDeUsuarioYFase",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ faseTicket: 1 }),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        var datos = result.data
                        vm.tickets = datos;
                        console.log('vm.tickets')
                        console.log(vm.tickets)

                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            obtenerTicketsQueAsigne() {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/GetAsignacionTicketPorIdUsuario",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        var datos = result.data

                        //vm.ticketsQueAsigne = datos.ticketsAsignados
                        //vm.ticketsQueAsigne = datos.ticketsAsignados
                        //console.log('tickets q asigne')
                        //console.log(vm.ticketsQueAsigne)
                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            agregarSeguimiento(idTicket) {
                var vm = this;
                const descripcion = $("#txtDescripcion").val();
                var destinatariosText = $("#cboCorreos option:selected").text()

                if (!vm.validarCamposSeguimiento()) {
                    toastr.error("Por favor, complete todos los campos.", "Error de Validación");
                    return;
                }

                if (!descripcion) {
                    toastr.error("La descripción es requerida", "Error");
                    return;
                }
                if (this.correos.length === 0) {
                    toastr.error("Debe agregar al menos un correo válido");
                    esValido = false;
                }

                const data = {
                    //Destinatarios: destinatariosText,
                    Destinatarios: vm.correos.map(c => c.Texto),

                    Descripcion: descripcion,
                    IdProcesoActual: $("#cboProceso").val(),
                    IdEstadoTicketActual: $("#cboEstadoTickets").val(),
                    IdTicket: idTicket
                }
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/CrearSeguimientoTicket",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        var datos = result.data
                        if (result.success) {
                            toastr.success(result.displayMessage, "Exito")
                            vm.showModalSeguimiento = false
                            vm.obtenerTicketsQueAsigne()
                            vm.obtenerTickets()
                            vm.correos = []

                        } else {
                            toastr.error(result.displayMessage, "Error")

                        }
                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            validarCamposSeguimiento() {
                let esValido = true;

                const campos = {
                    cboEstadoTickets: "#cboEstadoTickets",
                    cboProceso: "#cboProceso",
                    cboCorreos: "#cboCorreos",
                };

                Object.entries(campos).forEach(([nombre, selector]) => {
                    const $elemento = $(selector);
                    const valor = $elemento.val();
                    const $grupo = $elemento.closest(".select-container");

                    if (valor === null || valor === "") {
                        $grupo.addClass("has-error");
                        esValido = false;
                    } else {
                        $grupo.removeClass("has-error");
                    }
                    $elemento.on("change", function () {
                        const valorActual = $elemento.val()
                        if (valorActual !== null && valorActual !== "") {
                            $grupo.removeClass("has-error")
                        } else {
                            $grupo.addClass("has-error")
                        }
                    })
                })
                return esValido;
            },
            obtenerTicketsQueMeAsignaron() {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/GetAsignacionTicketPorIdUsuario",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        var datos = result.data
                        vm.ticketsAsignados = datos.ticketsAsignados
                        vm.ticketsRecibidos = datos.ticketsRecibidos
                        console.log('tickets que me asignaron')
                        console.log(vm.ticketsAsignados)
                        console.log(datos)

                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            obtenerDetalleHistorial(idTicket) {
                var vm = this;
                const dataHistorial = {
                    id: idTicket
                }
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/GetSeguimientosTicketPorIdTicket",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(dataHistorial),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        var datos = result.data
                        vm.historial = datos;
                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            openModalDetail(id, item) {
                this.ticketSelect = item;
                console.log("ticket select", this.ticketSelect)
                this.obtenerDetalleHistorial(id)
                this.showModalDetail = true;

                document.documentElement.style.overflow = "hidden";
            },
            openModalSeguimiento(id, item) {
                this.ticketSelect = item;
                this.obtenerDataSelectsSeguimiento();
                /*this.obtenerDetalleHistorial(id);*/
                this.showModalSeguimiento = true;

                document.documentElement.style.overflow = "hidden";
            },
            closeModalDetail() {
                this.showModalDetail = false
                document.documentElement.style.overflow = "auto"
            },
            closeModalSeguimiento() {
                this.showModalSeguimiento = false
                this.nuevoCorreo = "";
                this.correosFiltrados = []
                this.correos = [],
                document.documentElement.style.overflow = "auto"
            },
            getStatusClass(status) {
                switch (status) {
                    case 1:
                        return 'nuevo';
                    case 2:
                        return 'listo';
                    case 3:
                        return 'entregado';
                    default:
                        return '';
                }
            },
        },
        created: function () {
        },

        mounted: function () {
            this.obtenerTicketsQueMeAsignaron();
            //this.obtenerTickets();
            //this.obtenerTicketsQueAsigne();
        },
        watch: {


        },


    })
})


document.addEventListener('DOMContentLoaded', function () {
    var AppGlpiAsignarTicket = new Vue({
        el: '#AppGlpiAsignarTicket',
        data: {
            tabs: [
                {
                    name: "ticketssinAsignar",
                    label: "Tickets sin asignar",
                    iconPath: "M12 8v8m-4-4h8",
                },
                {
                    name: "sinAsignarTickets",
                    label: "Tickets que asigne",
                    iconPath: "M5 13l4 4L19 7",
                },
                {
                    name: "TicketsQueMeAsignaron",
                    label: "Tickets que me asignaron",
                    iconPath: "M5 13l4 4L19 7",
                },

            ],
            activeTab: "ticketssinAsignar",
            tickets: [],
            ticketsQueAsigne: [],
            ticketsQueRecibi: [],
            showModal: false,
            showModalDetailAsignacion: false,
            ticketSelect: {},
            historial: [],
            historialSeg: [],
            dataAsignar: [],
            modalRegistrarEmpleado: false,
            showModalRegistrarEmpleado: false,
            showModalSeguimiento: false,
            showModalAsignacion: false,
            estadosTicket: [],
            procesos: [],
            correos: [],
            correosBD: [],
            nuevoCorreo: "",
            correosFiltrados: [],
            ticketsQueAsigneSeg: [],
            ticketsCreados: [],
            ticketsAsignados: [],
            ticketsRecibidos: [],
            ticketsDetail: [],
            ticketsAsignadosDetail: [],
            ticketsSeguimientosDetail: [],
            showCierreModal: false,
            isEditing: false,
            currentTicketId: null,
            minDate: new Date().toISOString().slice(0, 16),
            modalProceso: false,
            modalEstadoTickets: false,
            showModalMainDetail: false,
            iconFiles: {
                excel: '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 32 32"><defs><linearGradient id="a" x1="4.494" y1="-2092.086" x2="13.832" y2="-2075.914" gradientTransform="translate(0 2100)" gradientUnits="userSpaceOnUse"><stop offset="0" stop-color="#18884f"/><stop offset="0.5" stop-color="#117e43"/><stop offset="1" stop-color="#0b6631"/></linearGradient></defs><title>file_type_excel</title><path d="M19.581,15.35,8.512,13.4V27.809A1.192,1.192,0,0,0,9.705,29h19.1A1.192,1.192,0,0,0,30,27.809h0V22.5Z" style="fill:#185c37"/><path d="M19.581,3H9.705A1.192,1.192,0,0,0,8.512,4.191h0V9.5L19.581,16l5.861,1.95L30,16V9.5Z" style="fill:#21a366"/><path d="M8.512,9.5H19.581V16H8.512Z" style="fill:#107c41"/><path d="M16.434,8.2H8.512V24.45h7.922a1.2,1.2,0,0,0,1.194-1.191V9.391A1.2,1.2,0,0,0,16.434,8.2Z" style="opacity:0.10000000149011612;isolation:isolate"/><path d="M15.783,8.85H8.512V25.1h7.271a1.2,1.2,0,0,0,1.194-1.191V10.041A1.2,1.2,0,0,0,15.783,8.85Z" style="opacity:0.20000000298023224;isolation:isolate"/><path d="M15.783,8.85H8.512V23.8h7.271a1.2,1.2,0,0,0,1.194-1.191V10.041A1.2,1.2,0,0,0,15.783,8.85Z" style="opacity:0.20000000298023224;isolation:isolate"/><path d="M15.132,8.85H8.512V23.8h6.62a1.2,1.2,0,0,0,1.194-1.191V10.041A1.2,1.2,0,0,0,15.132,8.85Z" style="opacity:0.20000000298023224;isolation:isolate"/><path d="M3.194,8.85H15.132a1.193,1.193,0,0,1,1.194,1.191V21.959a1.193,1.193,0,0,1-1.194,1.191H3.194A1.192,1.192,0,0,1,2,21.959V10.041A1.192,1.192,0,0,1,3.194,8.85Z" style="fill:url(#a)"/><path d="M5.7,19.873l2.511-3.884-2.3-3.862H7.758L9.013,14.6c.116.234.2.408.238.524h.017c.082-.188.169-.369.26-.546l1.342-2.447h1.7l-2.359,3.84,2.419,3.905H10.821l-1.45-2.711A2.355,2.355,0,0,1,9.2,16.8H9.176a1.688,1.688,0,0,1-.168.351L7.515,19.873Z" style="fill:#fff"/><path d="M28.806,3H19.581V9.5H30V4.191A1.192,1.192,0,0,0,28.806,3Z" style="fill:#33c481"/><path d="M19.581,16H30v6.5H19.581Z" style="fill:#107c41"/></svg>',
                word: '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" id="Capa_1" style="enable-background:new 0 0 128 128;" version="1.1" viewBox="0 0 128 128" xml:space="preserve"><style type="text/css">.st0{fill:#21A365;} .st1{fill:#107C41;} .st2{fill:#185B37;} .st3{fill:#33C481;} .st4{fill:#17864C;} .st5{fill:#FFFFFF;} .st6{fill:#036C70;} .st7{fill:#1A9BA1;} .st8{fill:#37C6D0;} .st9{fill:#04878B;} .st10{fill:#4F59CA;} .st11{fill:#7B82EA;} .st12{fill:#4C53BB;} .st13{fill:#0F78D5;} .st14{fill:#29A7EB;} .st15{fill:#0358A8;} .st16{fill:#0F79D6;} .st17{fill:#038387;} .st18{fill:#048A8E;} .st19{fill:#C8421D;} .st20{fill:#FF8F6A;} .st21{fill:#ED6B47;} .st22{fill:#891323;} .st23{fill:#AF2131;} .st24{fill:#C94E60;} .st25{fill:#E08195;} .st26{fill:#B42839;} .st27{fill:#0464B8;} .st28{fill:#0377D4;} .st29{fill:#4FD8FF;} .st30{fill:#1681D7;} .st31{fill:#0178D4;} .st32{fill:#042071;} .st33{fill:#168FDE;} .st34{fill:#CA64EA;} .st35{fill:#7E1FAF;} .st36{fill:#AE4BD5;} .st37{fill:#9332BF;} .st38{fill:#7719AA;} .st39{fill:#0078D4;} .st40{fill:#1490DF;} .st41{fill:#0364B8;} .st42{fill:#28A8EA;} .st43{fill:#41A5ED;} .st44{fill:#2C7BD5;} .st45{fill:#195ABE;} .st46{fill:#103E91;} .st47{fill:#2166C3;} .st48{opacity:0.2;}</style><path class="st43" d="M128,34.2H29.6V10.3c0-3.2,2.6-5.8,5.8-5.8h86.7c3.2,0,5.8,2.6,5.8,5.8V34.2z"/><rect class="st44" height="29.8" width="98.4" x="29.6" y="34.2"/><rect class="st45" height="29.8" width="98.4" x="29.6" y="64"/><path class="st46" d="M122.2,123.5H35.5c-3.2,0-5.8-2.6-5.8-5.8V93.8H128v23.9C128,120.9,125.4,123.5,122.2,123.5z"/><path class="st47" d="M59.5,96.5h-53c-3.5,0-6.4-2.9-6.4-6.4V37.9c0-3.5,2.9-6.4,6.4-6.4h53c3.5,0,6.4,2.9,6.4,6.4v52.2  C65.9,93.6,63.1,96.5,59.5,96.5z"/><g><path class="st5" d="M19.3,82.4l-8.9-35.9h7.1l3.5,16.3c0.9,4.4,1.8,8.9,2.4,12.5h0.1c0.6-3.8,1.6-8,2.6-12.6L30,46.5h7L40.6,63   c0.9,4.3,1.7,8.2,2.2,12.1h0.1c0.6-4,1.5-8,2.5-12.4l3.8-16.2h6.8l-9.8,35.9H39l-3.8-16.9c-0.9-4-1.6-7.4-2.1-11.3h-0.1   c-0.6,3.8-1.3,7.3-2.4,11.4l-4.2,16.9H19.3z"/></g><path class="st48" d="M65.9,37.3c0,0.2,0,0.4,0,0.6v52.2c0,3.5-2.9,6.4-6.4,6.4H29.7v5.7h35.2c3.5,0,6.4-2.9,6.4-6.4V43.6  C71.3,40.4,69,37.7,65.9,37.3z"/></svg>',
                pdf: ' <svg xmlns="http://www.w3.org/2000/svg" data-name="Layer 1" id="Layer_1" viewBox="0 0 24 24"><defs><style>.cls-1{fill:#f44336;}.cls-2{fill:#ff8a80;}.cls-3{fill:#ffebee;}</style></defs><title/><path class="cls-1" d="M16.5,22h-9a3,3,0,0,1-3-3V5a3,3,0,0,1,3-3h6.59a1,1,0,0,1,.7.29l4.42,4.42a1,1,0,0,1,.29.7V19A3,3,0,0,1,16.5,22Z"/><path class="cls-2" d="M18.8,7.74H15.2a1.5,1.5,0,0,1-1.5-1.5V2.64a.55.55,0,0,1,.94-.39L19.19,6.8A.55.55,0,0,1,18.8,7.74Z"/><path class="cls-3" d="M7.89,19.13a.45.45,0,0,1-.51-.51V15.69a.45.45,0,0,1,.5-.51.45.45,0,0,1,.5.43.78.78,0,0,1,.35-.32,1.07,1.07,0,0,1,.51-.12,1.17,1.17,0,0,1,.64.18,1.2,1.2,0,0,1,.43.51,2,2,0,0,1,0,1.57A1.2,1.2,0,0,1,8.75,18a.86.86,0,0,1-.35-.3v.91a.5.5,0,0,1-.13.38A.52.52,0,0,1,7.89,19.13Zm1-1.76a.48.48,0,0,0,.38-.18.81.81,0,0,0,.14-.55.82.82,0,0,0-.14-.55.5.5,0,0,0-.38-.17.51.51,0,0,0-.39.17.89.89,0,0,0-.14.55.87.87,0,0,0,.14.55A.48.48,0,0,0,8.92,17.37Z"/><path class="cls-3" d="M12.17,18.11a1.1,1.1,0,0,1-.63-.17,1.22,1.22,0,0,1-.44-.51,2,2,0,0,1,0-1.57,1.22,1.22,0,0,1,.44-.51,1.11,1.11,0,0,1,.63-.18,1.06,1.06,0,0,1,.5.12.91.91,0,0,1,.35.28V14.48a.45.45,0,0,1,.51-.51.49.49,0,0,1,.37.13.5.5,0,0,1,.13.38v3.11a.5.5,0,0,1-1,.08.76.76,0,0,1-.34.32A1.14,1.14,0,0,1,12.17,18.11Zm.33-.74a.48.48,0,0,0,.38-.18.8.8,0,0,0,.15-.55.82.82,0,0,0-.15-.55.5.5,0,0,0-.38-.17.49.49,0,0,0-.38.17.82.82,0,0,0-.15.55.8.8,0,0,0,.15.55A.46.46,0,0,0,12.5,17.37Z"/><path class="cls-3" d="M15.52,18.1a.46.46,0,0,1-.51-.51V16h-.15a.34.34,0,0,1-.39-.38c0-.25.13-.37.39-.37H15a1.2,1.2,0,0,1,.34-.87,1.52,1.52,0,0,1,.92-.36h.17a.39.39,0,0,1,.29,0,.35.35,0,0,1,.15.17.55.55,0,0,1,0,.22.38.38,0,0,1-.09.19.27.27,0,0,1-.18.1h-.08a.66.66,0,0,0-.41.12.41.41,0,0,0-.11.31v.09h.32c.26,0,.39.12.39.37a.34.34,0,0,1-.39.38H16v1.6A.45.45,0,0,1,15.52,18.1Z"/></svg>',
            },
            showModalEstadoTickets: false,
            isEditingEstado: false,
            nuevoEstadoNombre: '',
            estadoEditadoNombre: '',
            estadoEditadoIndex: null,
            showConfirmacionEliminarModal: false,
            proceso: {
                isEditingProceso: false,
                nuevoProcesoNombre: '',
                EditadoNombre: '',
                EditadoIndex: null,
            },
            previewList: [],

            vistaActual: 'lista',
            sortColumn: 'Id',
            sortOrder: 'desc',
            currentPage: 1,
            itemsPerPage: 10, // número de filas por página

        },
        computed: {
            filteredTabs() {
                return this.tabs.filter(tab => {
                    if (tab.name === "ticketssinAsignar") {
                        return this.tickets.length > 0;
                    }
                    if (tab.name === "sinAsignarTickets") {
                        return (this.ticketsAsignados.length > 0);
                    }
                    if (tab.name === "TicketsQueMeAsignaron") {
                        return (this.ticketsRecibidos.length > 0);
                    }
                    return true;
                });
            },
            pages() {
                const total = this.totalPages;
                const current = this.currentPage;
                const maxVisible = 3;
                let start = Math.max(1, current - Math.floor(maxVisible / 2));
                let end = Math.min(total, start + maxVisible - 1);

                // Ajuste cuando se está cerca del final
                if ((end - start + 1) < maxVisible) {
                    start = Math.max(1, end - maxVisible + 1);
                }

                let range = [];
                for (let i = start; i <= end; i++) {
                    range.push(i);
                }
                return range;
            },

            sortedData() {
                if (!this.sortColumn) return this.ticketsCreados;

                return [...this.ticketsCreados].sort((a, b) => {
                    let valA, valB;

                    // soporte para propiedades anidadas
                    if (this.sortColumn.includes('.')) {
                        const keys = this.sortColumn.split('.');
                        valA = keys.reduce((o, k) => (o ? o[k] : null), a);
                        valB = keys.reduce((o, k) => (o ? o[k] : null), b);
                    } else {
                        valA = a[this.sortColumn];
                        valB = b[this.sortColumn];
                    }

                    if (!valA) valA = '';
                    if (!valB) valB = '';

                    if (typeof valA === 'string') valA = valA.toLowerCase();
                    if (typeof valB === 'string') valB = valB.toLowerCase();

                    if (valA < valB) return this.sortOrder === 'asc' ? -1 : 1;
                    if (valA > valB) return this.sortOrder === 'asc' ? 1 : -1;
                    return 0;
                });
            },
            paginatedData() {
                const start = (this.currentPage - 1) * this.itemsPerPage;
                return this.sortedData.slice(start, start + this.itemsPerPage);
            },
            totalPages() {
                return Math.ceil(this.ticketsCreados.length / this.itemsPerPage);
            }
        },
        methods: {
            openModalMainDetail(id, item) {
                this.ticketSelect = item;
                this.previewList = [];

                if (item.Adjunto) {
                    this.loadAttachedFile(item.AdjuntoFullPath);
                }
                //this.obtenerDetalleHistorial(id)
                this.showModalMainDetail = true;
                document.documentElement.style.overflow = "hidden";
            },
            closeModalMainDetail() {
                this.showModalMainDetail = false
                document.documentElement.style.overflow = "auto"
            },
            loadAttachedFile(fileUrl) {
                this.previewList = [];

                const fileName = fileUrl.split("/").pop();

                const fileExtension = fileName.split(".").pop().toLowerCase();
                const fileType = this.getFileType(fileExtension);

                const fileData = {
                    id: Date.now() + Math.random(),
                    name: "Documento Adjunto",
                    type: fileType,
                    icon: this.getFileIcon(fileType),
                    typeClass: fileType,
                    preview: fileType === "image" ? fileUrl : null,
                    url: fileUrl
                };

                this.previewList.push(fileData);
            },
            getFileType(fileExtension) {
                const imageExtensions = ["png", "jpg", "jpeg"];
                const pdfExtensions = ["pdf"];
                const wordExtensions = ["doc", "docx"];
                console.log("fileExtension")
                console.log(fileExtension)

                if (imageExtensions.includes(fileExtension)) {
                    return "image";
                } else if (pdfExtensions.includes(fileExtension)) {
                    return "pdf";
                } else if (wordExtensions.includes(fileExtension)) {
                    return "word";
                } else {
                    return "unsupported";
                }
            },
            getFileIcon(fileType) {
                console.log("fileType getfileicon")
                console.log(fileType)
                switch (fileType) {
                    case "pdf": return this.iconFiles.pdf;
                    case "word": return this.iconFiles.word;
                    default:
                        return '<span>❓</span>';
                }

            },
            openImagePreview(imageUrl) {
                window.open(imageUrl, "_blank");
            },
            openCierreModal(id, item) {
                this.ticketSelectCierre = item;
                console.log(id)
                this.GetTicketById(id)
                this.GetSelectCierreTicket(id);
                this.showCierreModal = true;
                console.log("ticketsDetail.ClasificacionProblema.Nombre")
                console.log(this.ticketsDetail)
                document.documentElement.style.overflow = "hidden";
            },
            closeCierreModal() {
                this.showCierreModal = false;
                document.documentElement.style.overflow = "auto"

            },
            GetSelectCierreTicket(id) {
                var vm = this;

                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiSelects/GetSelectCierreTicket",
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

                            const cboEstadosTicket = $('#cboEstadoTickets');


                            cboEstadosTicket.empty();


                            cboEstadosTicket.append('<option value="">Seleccione una opcion</option>');
                            datos.estadosTicket.forEach((item) => {
                                cboEstadosTicket.append(`<option value="${item.Valor}">${item.Texto}</option>`);
                            });
                            cboEstadosTicket.select2({
                                multiple: false, placeholder: "--Seleccione--", allowClear: true, dropdownParent: $('#formCierreTicket')
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
            cerrarTicket() {
                var vm = this;
                const descripcion = $("#txtDescripcion2").val();

                if (!vm.validarCamposCierre()) {
                    toastr.error("Por favor, complete todos los campos.", "Error de Validación");
                    return;
                }

                if (!descripcion) {
                    toastr.error("La descripción es requerida", "Error");
                    return;
                }
                const data = {
                    Descripcion: descripcion,
                    IdEstadoTicketActual: $("#cboEstadoTickets").val(),
                    FechaTentativaTermino: $("#fechaFin").val(),
                    IdTicket: vm.ticketsDetail.Id
                }

                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/CerrarTicket",
                    cache: false,
                    dataType: "json",
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success(result.displayMessage, "Exito")
                            vm.closeCierreModal()
                            vm.GetTicketsPorIdUsuario()
                            vm.obtenerTickets()

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
            validarCamposCierre() {
                let esValido = true;

                const campos = {
                    cboEstadoTickets: "#cboEstadoTickets",
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
            openEditFormAsignar(ticketId, ticketData) {
                this.openModalAsignacion(ticketId, ticketData)
                console.log("ticketId", ticketId)
                console.log("ticketData", ticketData)
                // Cargar los datos del ticket en el formulario

                // Cambiar el texto del botón de "Registrar" a "Editar"
                this.isEditing = true;
                this.currentTicketId = ticketId;
            },
            loadTicketData(ticketData) {
                // Cargar los datos del ticket en los campos del formulario
                console.log("ticketData cargando", ticketData.Asignacion)
                //console.log("ticketData cboestadotickets", ticketData.EstadoTicket.Id)
                //console.log("ticketData Empleados", ticketData.UsuarioAsignado.Id)
                //console.log("ticketData fechaFin", ticketData.FechaTentativaTermino)


                $("#cboEstadoTickets").val(ticketData.Asignacion.EstadoTicket.Id).trigger("change");
                $("#cboEmpleados").val(ticketData.Asignacion.UsuarioAsignado.Id).trigger("change");
                //$("#fechaFin").val(ticketData.Asignacion.FechaTentativaTermino);

                // Extraer el timestamp numérico
                let timestamp = parseInt(ticketData.Asignacion.FechaTentativaTermino.match(/\d+/)[0]);

                // Crear un objeto Date
                let fechaUTC = new Date(timestamp);

                // Ajustar a UTC-5
                fechaUTC.setHours(fechaUTC.getHours() - 5);

                // Convertir a formato "YYYY-MM-DDTHH:MM"
                let fechaISO = fechaUTC.toISOString().slice(0, 16);

                // Asignar al input
                $("#fechaFin").val(fechaISO);


                // Cargar los correos
                if (typeof ticketData.Correos === "string") {
                    this.correos = ticketData.Correos.split(",").map(correo => correo.trim());
                } else if (Array.isArray(ticketData.Correos)) {
                    this.correos = ticketData.Correos;
                } else {
                    this.correos = [];
                }
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
                //const fechaActual = new Date();
                //const fechaTentativaDate = new Date(parseInt(fechaTentativa.substr(6)));
                //return fechaTentativaDate < fechaActual;

                const timestamp = parseInt(fechaTentativa.replace(/[^0-9]/g, '')); // Extrae el número
                const fechaTentativaDate = new Date(timestamp);
                const fechaActual = new Date();
                return fechaTentativaDate < fechaActual;
            },

            filtrarCorreos() {
                if (this.nuevoCorreo.length > 0) {
                    const todosLosCorreos = this.correosBD.map(correo => correo.Texto.split(',')).flat();
                    console.log(todosLosCorreos)
                    this.correosFiltrados = todosLosCorreos.filter(correo =>
                        correo.toLowerCase().includes(this.nuevoCorreo.toLowerCase())
                    );
                    console.log("correosFiltrados")
                    console.log(this.correosFiltrados, "correosFiltrados")
                } else {
                    this.correosFiltrados = [];
                }
            },
            seleccionarCorreo(correo) {
                this.nuevoCorreo = correo;
                this.agregarCorreos();
                this.correosFiltrados = []; // Oculta sugerencias después de seleccionar
            },
            //agregarCorreos() {
            //    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            //    if (!this.nuevoCorreo.trim() || !emailRegex.test(this.nuevoCorreo.trim())) {
            //        toastr.error("Ingrese un correo válido.");
            //        return;
            //    }
            //    if (this.correos.some(c => c.Texto === this.nuevoCorreo.trim())) {
            //        toastr.warning("El correo ya está en la lista.");
            //        return;
            //    }
            //    this.correos.push({ Texto: this.nuevoCorreo.trim() });
            //    this.nuevoCorreo = "";
            //    this.correosFiltrados = [];
            //},
            agregarCorreos() {
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (this.nuevoCorreo.trim() === "") {
                    toastr.error("Por favor, ingrese un correo válido.");
                    return;
                }
                if (!emailRegex.test(this.nuevoCorreo.trim())) {
                    toastr.error("El correo ingresado no es válido.");
                    return;
                }
                if (this.correos.includes(this.nuevoCorreo.trim())) {
                    toastr.warning("El correo ya está en la lista.");
                    return;
                }
                this.correos.push(this.nuevoCorreo.trim());
                this.nuevoCorreo = "";
            },
            quitarCorreos(index) {
                this.correos.splice(index, 1);
            },
            GetTicketsPorIdUsuario() {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/GetTicketsPorIdUsuario",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        var datos = result.data
                        vm.ticketsAsign
                        ados = datos.ticketsAsignados
                        vm.ticketsRecibidos = datos.ticketsRecibidos
                        vm.ticketsCreados = datos.ticketsCreados;
                        console.log('ticketsCreados')
                        console.log(vm.ticketsCreados)



                        console.log('tickets q asigne')
                        console.log(vm.ticketsQueAsigne)
                        console.log('tickets q recibi')
                        console.log(vm.ticketsQueRecibi)
                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            GetTicketsPorIdUsuario() {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/GetTicketsPorIdUsuario",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.data != null) {
                            var datos = result.data
                            vm.ticketsCreados = datos.ticketsCreados
                            vm.ticketsAsignados = datos.ticketsAsignados
                            vm.ticketsRecibidos = datos.ticketsRecibidos
                            console.log("ticketsCreados")
                            console.log(vm.ticketsCreados)
                            console.log("ticketsAsignados")
                            console.log(vm.ticketsAsignados)
                            console.log("ticketsRecibidos")
                            console.log(vm.ticketsRecibidos)

                            $('#overlaySearch').hide()
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
            GetTicketById(idTicket) {
                var vm = this;
                const dataHistorial = {
                    id: idTicket
                }
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/GetTicketById",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(dataHistorial),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        vm.historial = datos;
                        vm.ticketsAsignadosDetail = datos

                        var datos = result.data
                        var datos2 = result.data.Asignacion
                        var datos3 = result.data.Seguimientos
                        vm.ticketsDetail = datos
                        vm.ticketsAsignadosDetail = datos2
                        vm.ticketsSeguimientosDetail = datos3

                        console.log("ticketsSeguimientosDetail")
                        console.log(vm.ticketsSeguimientosDetail)

                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
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
            agregarSeguimiento(idTicket) {
                var vm = this;
                const descripcion = $("#txtDescripcion").val();

                if (!vm.validarCamposSeguimiento()) {
                    toastr.error("Por favor, complete todos los campos.", "Error de Validación");
                    return;
                }

                if (!descripcion) {
                    toastr.error("La descripción es requerida", "Error");
                    return;
                }

                const data = {
                    Destinatarios: vm.correos,
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
                            vm.GetTicketsPorIdUsuario()
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
                };

                Object.entries(campos).forEach(([nombre, selector]) => {
                    const $elemento = $(selector);
                    const valor = $elemento.val();
                    const $grupo = $elemento.closest(".sheet-input");

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
                            vm.correosBD = datos.correos;


                            const cboEstadosTicket = $('#cboEstadoTickets');
                            const cboProcesos = $('#cboProceso');

                            cboEstadosTicket.empty();
                            cboProcesos.empty();

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
            obtenerDataSelectsAsignacion() {
                var vm = this;
                const ticket = vm.ticketsDetail;
                const isEdit = ticket.Asignacion.Id > 0;

                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiSelects/GetSelectAsignarTicket",
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
                            vm.correosBD = datos.correos;

                            const dataEmpleados = $('#cboEmpleados');
                            dataEmpleados.empty();
                            dataEmpleados.append('<option value="">Seleccione un empleado</option>');
                            result.data.usuariosAsignables.forEach((item) => {
                                dataEmpleados.append(`<option value="${item.Valor}">${item.Texto}</option>`);
                            });
                            dataEmpleados.select2({
                                placeholder: "Seleccione",
                                allowClear: true
                            });

                            //OBTENER CBO ESTADOS TICKET
                            const estadosActivos = datos.estadosTicket.filter(item => item.Estado === true);

                            const cboEstadosTicket = $('#cboEstadoTickets');
                            cboEstadosTicket.empty();
                            cboEstadosTicket.append('<option value="">Seleccione una opcion</option>');

                            estadosActivos.forEach((item) => {
                                cboEstadosTicket.append(`<option value="${item.Valor}">${item.Texto}</option>`);
                            });


                            //vm.estadosTicket.forEach((item) => {
                            //    cboEstadosTicket.append(`<option value="${item.Valor}">${item.Texto}</option>`);
                            //});

                            cboEstadosTicket.select2({
                                placeholder: "Seleccione",
                                allowClear: true
                            });

                            if (isEdit) {
                                vm.loadTicketData(ticket);
                            } else {
                                cboEstadosTicket.val(cboEstadosTicket.find("option:eq(1)").val()).trigger("change");
                            }
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
            getStatusClass(status) {
                switch (status) {
                    case 1:
                        return 'nuevo';
                    case 2:
                        return 'listo';
                    case 3:
                        return 'entregado';
                    case 4:
                        return 'cerrado';
                    default:
                        return '';
                }
            },
            openModalDetail(id, item) {
                this.ticketSelect = item;
                console.log("detalle seguimiento id")
                console.log(id)
                this.GetTicketById(id)
                this.showModal = true;

                document.documentElement.style.overflow = "hidden";
            },
            openModalDetailAsignacion(id, item) {
                this.ticketSelect = item;
                console.log("detalle id")
                console.log(id)
                this.GetTicketById(id)
                this.showModalDetailAsignacion = true;

                document.documentElement.style.overflow = "hidden";
            },
            openModalSeguimiento(id, item) {
                this.ticketSelect = item;
                this.obtenerDataSelectsSeguimiento();
                this.showModalSeguimiento = true;

                document.documentElement.style.overflow = "hidden";
            },
            openModalAsignacion(id, item) {
                this.ticketsDetail = item;
                console.log("item")
                console.log(item)
                this.showModalAsignacion = true;
                this.obtenerDataSelectsAsignacion();
                //this.obtenerEmpleados()

                document.documentElement.style.overflow = "hidden";
            },
            closeModalSeguimiento() {
                this.showModalSeguimiento = false
                this.nuevoCorreo = "";
                this.correosFiltrados = []
                this.correos = [],
                    document.documentElement.style.overflow = "auto"
            },
            closeModalAsignacion() {
                this.isEditing = false;
                this.showModalAsignacion = false
                this.nuevoCorreo = "";
                this.correosFiltrados = []
                this.correos = [],
                    document.documentElement.style.overflow = "auto"
            },
            closeModalDetail() {
                this.showModal = false
                document.documentElement.style.overflow = "auto"
            },
            closeModalDetailAsignacion() {
                this.showModalDetailAsignacion = false
                document.documentElement.style.overflow = "auto"
            },
            openModalEmpleado() {
                this.modalRegistrarEmpleado = true;
                this.showModalRegistrarEmpleado = true;
                this.obtenerRoles();
                this.obtenerCargos()
            },
            closeModalEmpelado() {
                this.modalRegistrarEmpleado = false;
            },
            //MODAL CRUD ESTADO----------------------------------------------------
            openModalEstadoTickets(formContext) {
                var vm = this
                vm.valorSeleccionado = $(`#cboEstadoTickets`).val();

                vm.formContext = formContext;

                let endpoint = '';
                switch (formContext) {
                    case 'formAgregarEditarAsignacion':
                        endpoint = basePath + "GlpiSelects/GetSelectAsignarTicket";
                        break;
                    case 'formCierreTicket':
                        endpoint = basePath + "GlpiSelects/GetSelectCierreTicket";
                        break;
                    case 'formAgregarSeguimiento':
                        endpoint = basePath + "GlpiSelects/GetSelectSeguimientoTicket";
                        break;
                    default:
                        toastr.error("Formulario no reconocido", "Error");
                        return;
                }
                vm.endpoint = endpoint;
                this.cargarEstadosTicket(); 
                vm.showModalEstadoTickets = true;
            },
            closeModalEstadoTickets() {
                this.showModalEstadoTickets = false;
                this.isEditingEstado = false;
                this.nuevoEstadoNombre = '';
                this.estadoEditadoNombre = '';
            },
            cargarEstadosTicket() {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiEstadosTicket/GetEstadosTicket",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            vm.estadosTicket = result.data;
                        } else {
                            toastr.error(result.displayMessage, "Error");
                        }
                    },
                    error: function (request, status, error) {
                        toastr.error("Error al cargar los estados de ticket", "Error");
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            guardarEstadoTicket() {
                var vm = this
                const data = {
                    Nombre: this.nuevoEstadoNombre,
                    Estado: true,
                }
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiEstadosTicket/SaveEstadoTicket",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success(result.displayMessage, "Éxito");
                            vm.cargarEstadosTicket()
                            vm.nuevoEstadoNombre = '';
                            //vm.closeModalEstadoTickets()
                            vm.actualizarSelectDinamico(
                                "#cboEstadoTickets", 
                                vm.endpoint,
                                null, 
                                vm.formContext  
                            );
                        }
                        else {
                            toastr.error(result.displayMessage, "Error");

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
            editarEstadoTicket(index) {
                this.isEditingEstado = true;
                this.estadoEditadoNombre = this.estadosTicket[index].Nombre;
                this.estadoEditadoIndex = index;
            },
            actualizarEstadoTicket() {
                var vm = this;
                const data = {
                    Id: this.estadosTicket[this.estadoEditadoIndex].Id,
                    Nombre: this.estadoEditadoNombre,
                };
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiEstadosTicket/SaveEstadoTicket",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success("Estado actualizado con éxito.", "Éxito");
                            vm.cargarEstadosTicket();
                            //vm.closeModalEstadoTickets();

                            vm.actualizarSelectDinamico(
                                "#cboEstadoTickets",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                            vm.isEditingEstado = false;
                            vm.estadoEditadoIndex = null;
                        } else {
                            toastr.error("Error al actualizar estado", "Error");
                        }
                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor");
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide");
                    }
                });
            },
            actualizarSelectDinamico(selector, endpoint, filterFn = null, formContext = '') {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: endpoint,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        if (result.success) {
                            let datos = result.data.estadosTicket; 
                            if (filterFn && typeof filterFn === 'function') {
                                datos = datos.filter(filterFn);
                            }
                            vm.actualizarSelect(selector, datos, null, formContext ? `#${formContext}` : '');
                        } else {
                            toastr.error(result.displayMessage, "Error");
                        }
                    },
                    error: () => {
                        toastr.error("Error al cargar los datos del select", "Error");
                    }
                });
            },
            actualizarSelect(selectId, datos, valorSeleccionado = null, formId = null) {
                const valorActual = $(selectId).val();
                $(selectId).empty().append('<option value="">--Seleccione--</option>');

                datos.forEach((value) => {
                    $(selectId).append(`<option value="${value.Valor}">${value.Texto}</option>`);
                });

                if (valorSeleccionado !== null) {
                    $(selectId).val(valorSeleccionado).trigger('change');
                } else if (valorActual !== null && valorActual !== '') {
                    $(selectId).val(valorActual).trigger('change');
                }

                const select2Options = {
                    multiple: false,
                    placeholder: "--Seleccione--",
                    allowClear: true,
                };

                if (formId) {
                    select2Options.dropdownParent = $(formId);
                }

                $(selectId).select2(select2Options);
            },
            //MODAL CRUD PROCESO----------------------------------------------------

            openModalProceso(formContext) {
                var vm = this
                vm.valorSeleccionado = $(`#cboProceso`).val();

                vm.formContext = formContext;
                let endpoint = '';
                switch (formContext) {
                    case 'formAgregarSeguimiento':
                        endpoint = basePath + "GlpiSelects/GetSelectSeguimientoTicket";
                        break;
                    default:
                        toastr.error("Formulario no reconocido", "Error");
                        return;
                }
                vm.endpoint = endpoint;
                this.GetProcesos();
                vm.modalProceso = true;
            },
            closeModalProceso() {
                this.modalProceso = false;
                this.proceso.isEditingProceso = false;
                this.proceso.nuevoProcesoNombre = '';
                this.proceso.EditadoNombre = '';
            },
            GetProcesos() {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiProcesos/GetProcesos",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            vm.procesos = result.data;
                        } else {
                            toastr.error(result.displayMessage, "Error");
                        }
                    },
                    error: function (request, status, error) {
                        toastr.error("Error al cargar los estados de ticket", "Error");
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            guardarProceso() {
                var vm = this
                const data = {
                    Nombre: this.proceso.nuevoProcesoNombre,
                    Estado: true,
                }
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiProcesos/SaveProceso",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success("Ticket registrado con éxito.", "Éxito");
                            vm.GetProcesos()
                            vm.proceso.nuevoProcesoNombre = '';
                            //vm.closeModalProceso()
                            vm.actualizarSelectProceso(
                                "#cboProceso",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                        }
                        else {
                            toastr.error("Error al registrar tipo de operacion", "Error");

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
            editarProceso(index) {
                this.proceso.isEditingProceso = true;
                this.proceso.EditadoNombre = this.procesos[index].Nombre;
                this.proceso.EditadoIndex = index;
            },
            actualizarProceso() {
                var vm = this;
                const data = {
                    Id: this.procesos[this.proceso.EditadoIndex].Id,
                    Nombre: this.proceso.EditadoNombre,
                };
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiProcesos/SaveProceso",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success("Proceso actualizado con éxito.", "Éxito");
                            vm.GetProcesos();

                            vm.actualizarSelectProceso(
                                "#cboProceso",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                            vm.proceso.isEditingProceso = false;
                            vm.proceso.EditadoIndex = null;
                        } else {
                            toastr.error("Error al actualizar proceso", "Error");
                        }
                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor");
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide");
                    }
                });
            },
            actualizarSelectProceso(selector, endpoint, filterFn = null, formContext = '') {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: endpoint,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        if (result.success) {
                            let datos = result.data.procesos;
                            if (filterFn && typeof filterFn === 'function') {
                                datos = datos.filter(filterFn);
                            }
                            vm.actualizarSelect(selector, datos, null, formContext ? `#${formContext}` : '');
                        } else {
                            toastr.error(result.displayMessage, "Error");
                        }
                    },
                    error: () => {
                        toastr.error("Error al cargar los datos del select", "Error");
                    }
                });
            },
            //////////////////////////////////////////
            saveAsignacionTicket(isEdit) {
                if (isEdit) {
                    this.editarAsignacionTickect()
                } else {
                    this.asignarTickect()
                }
            },
            asignarTickect() {
                var vm = this;

                if (!vm.validarCamposAsignacion()) {
                    toastr.error("Por favor, complete todos los campos.", "Error de Validación");
                    return;
                }

                if (this.correos.length === 0) {
                    toastr.error("Debe agregar al menos un correo válido");
                    esValido = false;
                }

                const data = {
                    IdEstadoTicket: $("#cboEstadoTickets").val(),
                    IdUsuarioAsignado: $("#cboEmpleados").val(),
                    FechaTentativaTermino: $("#fechaFin").val(),
                    Destinatarios: vm.correos,
                    IdTicket: vm.ticketsDetail.Id,
                    Id: vm.ticketsDetail.Asignacion.Id,
                }
                console.log("ticketselect", vm.ticketSelect)


                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/AsignarTicket",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success(result.displayMessage, "Exito")
                            vm.closeModalAsignacion()
                            vm.GetTicketsPorIdUsuario()
                            vm.obtenerTickets()
                            vm.correos = []

                        } else {
                            toastr.error(result.displayMessage, "Error")
                            //vm.closeModalAsignacion()

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
            editarAsignacionTickect() {
                var vm = this;

                if (!vm.validarCamposAsignacion()) {
                    toastr.error("Por favor, complete todos los campos.", "Error de Validación");
                    return;
                }

                if (this.correos.length === 0) {
                    toastr.error("Debe agregar al menos un correo válido");
                    esValido = false;
                }

                const data = {
                    IdEstadoTicket: $("#cboEstadoTickets").val(),
                    IdUsuarioAsignado: $("#cboEmpleados").val(),
                    FechaTentativaTermino: $("#fechaFin").val(),
                    Destinatarios: vm.correos,
                    IdTicket: vm.ticketsDetail.Id,
                    Id: vm.ticketsDetail.Asignacion.Id,
                }
                console.log("ticketselect", vm.ticketSelect)

                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/EditarAsignacionTicket",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success(result.displayMessage, "Exito")
                            vm.closeModalAsignacion()
                            vm.GetTicketsPorIdUsuario()
                            vm.obtenerTickets()
                        } else {
                            toastr.error(result.displayMessage, "Error")
                            //vm.closeModalAsignacion()

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
            validarCamposAsignacion() {
                let esValido = true;

                const campos = {
                    cboEstadoTickets: "#cboEstadoTickets",
                    cboEmpleados: "#cboEmpleados",
                    fechaFin: "#fechaFin",
                };

                Object.entries(campos).forEach(([nombre, selector]) => {
                    const $elemento = $(selector);
                    const valor = $elemento.val();
                    const $grupo = $elemento.closest(".sheet-input");

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
            obtenerEmpleados() {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiSelects/GetSelectAsignarTicket",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        vm.dataAsignar = result.data
                        console.log(vm.dataAsignar)

                        const dataEmpleados = $('#cboEmpleados');
                        dataEmpleados.empty();
                        dataEmpleados.append('<option value="">Seleccione un empleado</option>');
                        result.data.usuariosAsignables.forEach((item) => {
                            dataEmpleados.append(`<option value="${item.Valor}">${item.Texto}</option>`);
                        });
                        dataEmpleados.select2();

                        const dataEstadoTickets = $('#cboEstadoTicket');
                        dataEstadoTickets.empty();
                        dataEstadoTickets.append('<option value="">Seleccione un empleado</option>');
                        result.data.estadosTicket.forEach((item) => {
                            dataEstadoTickets.append(`<option value="${item.Valor}">${item.Texto}</option>`);
                        });
                        dataEstadoTickets.select2();
                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            toggleModalRegistrarEmpleado(estado) {
                console.log(estado)
                if (estado == true) {
                    this.obtenerCargos();
                    this.obtenerRoles();
                }
                this.modalRegistrarEmpleado = estado;
                this.showModalRegistrarEmpleado = estado;
            },
            crearEmpleado() {
                var vm = this;
                var Nombres = $("#nombresEmpleado").val();
                var ApellidosPaterno = $("#apellidoPaternoEmpleado").val();
                var ApellidosMaterno = $("#apellidoMaternoEmpleado").val();
                var CargoID = $("#cboCargos").val();
                var DOIID = $("#cboTipoDocumento").val();
                var DOI = $("#numeroDocumentoEmpleado").val();
                var Telefono = $("#telefonoEmpleado").val();
                var UsuarioNombre = $("#nombreUsuario").val();
                var UsuarioContraseña = $("#contraseniaUsuario").val();
                var rolId = $("#cboRol").val();

                const data = {
                    Nombres,
                    ApellidosPaterno,
                    ApellidosMaterno,
                    CargoID,
                    DOIID,
                    DOI,
                    Telefono,
                    UsuarioNombre,
                    UsuarioContraseña,
                    rolId
                }
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/CrearEmpleado",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success("Error", result.displayMessage)
                            vm.obtenerEmpleados();
                            vm.modalRegistrarEmpleado = false;
                        } else {
                            toastr.error("Error", result.displayMessage)

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
            obtenerCargos() {
                var vm = this;

                $.ajax({
                    type: "POST",
                    url: basePath + "Empleado/CargoListarJson",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        var datos = result.data

                        const dataCargo = $('#cboCargos');
                        dataCargo.empty();
                        dataCargo.append('<option value="">Seleccione un empleado</option>');
                        datos.forEach((item) => {
                            dataCargo.append(`<option value="${item.CargoID}">${item.Descripcion}</option>`);
                        });
                        dataCargo.select2();

                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            obtenerRoles() {
                var vm = this;

                $.ajax({
                    type: "POST",
                    url: basePath + "RolUsuario/ListadoRolUsuario",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        var datos = result.data

                        const dataRol = $('#cboRol');
                        dataRol.empty();
                        dataRol.append('<option value="">Seleccione un empleado</option>');
                        datos.forEach((item) => {
                            dataRol.append(`<option value="${item.WEB_RolID}">${item.WEB_RolNombre}</option>`);
                        });
                        dataRol.select2();

                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            cambiarVista(vista) {
                this.vistaActual = vista
            },

            sortTable(column) {
                if (this.sortColumn === column) {
                    this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
                } else {
                    this.sortColumn = column;
                    this.sortOrder = 'asc';
                }
            },
        },
        created: function () {
        },

        mounted: function () {

            this.GetTicketsPorIdUsuario();
            this.obtenerTickets();


            //this.obtenerEmpleados();
        },
        watch: {
            filteredTabs: {
                immediate: true,
                handler(newTabs) {
                    if (!newTabs.some(tab => tab.name === this.activeTab)) {
                        this.activeTab = newTabs.length > 0 ? newTabs[0].name : null;
                    }
                }
            }

        },


    })
})

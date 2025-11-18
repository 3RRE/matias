document.addEventListener('DOMContentLoaded', function () {
    var AppGlpiReportes = new Vue({
        el: '#AppGlpiReportes',
        data: {
            tabs: [
                {
                    name: "ticketssinAsignar",
                    label: "Reporte",
                    iconPath: "M12 8v8m-4-4h8",
                },
                {
                    name: "sinAsignarTickets",
                    label: "Dashboard",
                    iconPath: "M5 13l4 4L19 7",
                },
            ],
            activeTab: "ticketssinAsignar",
            tickets: [],
            salas: [],
            ticketsQueAsigneSeg: [],
            ticketsAsignados: [],
            ticketsRecibidos: [],
            historialSeg: [],
            totalTickets: 0,
            estadoChart: null,
            problemaChart: null,
            zonaChart: null,
            filters: {
                startDate: '',
                endDate: '',
                faseTicket: 'todos',
                sala: 'todos',
            },
            fasesTicket: [
                {
                    id: 1,
                    name: "Creado"
                },
                {
                    id: 2,
                    name: "Asigando"
                },
                {
                    id: 3,
                    name: "En Proceso"
                },
                {
                    id: 4,
                    name: "Finalizado"
                },
                {
                    id: 5,
                    name: "Cerrado"
                }
            ],
            showModalDetalle: false,
            showModalDetailAsignacion: false,
            showModalSeguimiento: false,
            ticketsDetail: [],
            ticketsQueAsigne: [],
            ticketsAsignadosDetail: [],
            ticketsSeguimientosDetail: [],
            previewList: [],
            iconFiles: {
                excel: '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 32 32"><defs><linearGradient id="a" x1="4.494" y1="-2092.086" x2="13.832" y2="-2075.914" gradientTransform="translate(0 2100)" gradientUnits="userSpaceOnUse"><stop offset="0" stop-color="#18884f"/><stop offset="0.5" stop-color="#117e43"/><stop offset="1" stop-color="#0b6631"/></linearGradient></defs><title>file_type_excel</title><path d="M19.581,15.35,8.512,13.4V27.809A1.192,1.192,0,0,0,9.705,29h19.1A1.192,1.192,0,0,0,30,27.809h0V22.5Z" style="fill:#185c37"/><path d="M19.581,3H9.705A1.192,1.192,0,0,0,8.512,4.191h0V9.5L19.581,16l5.861,1.95L30,16V9.5Z" style="fill:#21a366"/><path d="M8.512,9.5H19.581V16H8.512Z" style="fill:#107c41"/><path d="M16.434,8.2H8.512V24.45h7.922a1.2,1.2,0,0,0,1.194-1.191V9.391A1.2,1.2,0,0,0,16.434,8.2Z" style="opacity:0.10000000149011612;isolation:isolate"/><path d="M15.783,8.85H8.512V25.1h7.271a1.2,1.2,0,0,0,1.194-1.191V10.041A1.2,1.2,0,0,0,15.783,8.85Z" style="opacity:0.20000000298023224;isolation:isolate"/><path d="M15.783,8.85H8.512V23.8h7.271a1.2,1.2,0,0,0,1.194-1.191V10.041A1.2,1.2,0,0,0,15.783,8.85Z" style="opacity:0.20000000298023224;isolation:isolate"/><path d="M15.132,8.85H8.512V23.8h6.62a1.2,1.2,0,0,0,1.194-1.191V10.041A1.2,1.2,0,0,0,15.132,8.85Z" style="opacity:0.20000000298023224;isolation:isolate"/><path d="M3.194,8.85H15.132a1.193,1.193,0,0,1,1.194,1.191V21.959a1.193,1.193,0,0,1-1.194,1.191H3.194A1.192,1.192,0,0,1,2,21.959V10.041A1.192,1.192,0,0,1,3.194,8.85Z" style="fill:url(#a)"/><path d="M5.7,19.873l2.511-3.884-2.3-3.862H7.758L9.013,14.6c.116.234.2.408.238.524h.017c.082-.188.169-.369.26-.546l1.342-2.447h1.7l-2.359,3.84,2.419,3.905H10.821l-1.45-2.711A2.355,2.355,0,0,1,9.2,16.8H9.176a1.688,1.688,0,0,1-.168.351L7.515,19.873Z" style="fill:#fff"/><path d="M28.806,3H19.581V9.5H30V4.191A1.192,1.192,0,0,0,28.806,3Z" style="fill:#33c481"/><path d="M19.581,16H30v6.5H19.581Z" style="fill:#107c41"/></svg>',
                word: '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" id="Capa_1" style="enable-background:new 0 0 128 128;" version="1.1" viewBox="0 0 128 128" xml:space="preserve"><style type="text/css">.st0{fill:#21A365;} .st1{fill:#107C41;} .st2{fill:#185B37;} .st3{fill:#33C481;} .st4{fill:#17864C;} .st5{fill:#FFFFFF;} .st6{fill:#036C70;} .st7{fill:#1A9BA1;} .st8{fill:#37C6D0;} .st9{fill:#04878B;} .st10{fill:#4F59CA;} .st11{fill:#7B82EA;} .st12{fill:#4C53BB;} .st13{fill:#0F78D5;} .st14{fill:#29A7EB;} .st15{fill:#0358A8;} .st16{fill:#0F79D6;} .st17{fill:#038387;} .st18{fill:#048A8E;} .st19{fill:#C8421D;} .st20{fill:#FF8F6A;} .st21{fill:#ED6B47;} .st22{fill:#891323;} .st23{fill:#AF2131;} .st24{fill:#C94E60;} .st25{fill:#E08195;} .st26{fill:#B42839;} .st27{fill:#0464B8;} .st28{fill:#0377D4;} .st29{fill:#4FD8FF;} .st30{fill:#1681D7;} .st31{fill:#0178D4;} .st32{fill:#042071;} .st33{fill:#168FDE;} .st34{fill:#CA64EA;} .st35{fill:#7E1FAF;} .st36{fill:#AE4BD5;} .st37{fill:#9332BF;} .st38{fill:#7719AA;} .st39{fill:#0078D4;} .st40{fill:#1490DF;} .st41{fill:#0364B8;} .st42{fill:#28A8EA;} .st43{fill:#41A5ED;} .st44{fill:#2C7BD5;} .st45{fill:#195ABE;} .st46{fill:#103E91;} .st47{fill:#2166C3;} .st48{opacity:0.2;}</style><path class="st43" d="M128,34.2H29.6V10.3c0-3.2,2.6-5.8,5.8-5.8h86.7c3.2,0,5.8,2.6,5.8,5.8V34.2z"/><rect class="st44" height="29.8" width="98.4" x="29.6" y="34.2"/><rect class="st45" height="29.8" width="98.4" x="29.6" y="64"/><path class="st46" d="M122.2,123.5H35.5c-3.2,0-5.8-2.6-5.8-5.8V93.8H128v23.9C128,120.9,125.4,123.5,122.2,123.5z"/><path class="st47" d="M59.5,96.5h-53c-3.5,0-6.4-2.9-6.4-6.4V37.9c0-3.5,2.9-6.4,6.4-6.4h53c3.5,0,6.4,2.9,6.4,6.4v52.2  C65.9,93.6,63.1,96.5,59.5,96.5z"/><g><path class="st5" d="M19.3,82.4l-8.9-35.9h7.1l3.5,16.3c0.9,4.4,1.8,8.9,2.4,12.5h0.1c0.6-3.8,1.6-8,2.6-12.6L30,46.5h7L40.6,63   c0.9,4.3,1.7,8.2,2.2,12.1h0.1c0.6-4,1.5-8,2.5-12.4l3.8-16.2h6.8l-9.8,35.9H39l-3.8-16.9c-0.9-4-1.6-7.4-2.1-11.3h-0.1   c-0.6,3.8-1.3,7.3-2.4,11.4l-4.2,16.9H19.3z"/></g><path class="st48" d="M65.9,37.3c0,0.2,0,0.4,0,0.6v52.2c0,3.5-2.9,6.4-6.4,6.4H29.7v5.7h35.2c3.5,0,6.4-2.9,6.4-6.4V43.6  C71.3,40.4,69,37.7,65.9,37.3z"/></svg>',
                pdf: ' <svg xmlns="http://www.w3.org/2000/svg" data-name="Layer 1" id="Layer_1" viewBox="0 0 24 24"><defs><style>.cls-1{fill:#f44336;}.cls-2{fill:#ff8a80;}.cls-3{fill:#ffebee;}</style></defs><title/><path class="cls-1" d="M16.5,22h-9a3,3,0,0,1-3-3V5a3,3,0,0,1,3-3h6.59a1,1,0,0,1,.7.29l4.42,4.42a1,1,0,0,1,.29.7V19A3,3,0,0,1,16.5,22Z"/><path class="cls-2" d="M18.8,7.74H15.2a1.5,1.5,0,0,1-1.5-1.5V2.64a.55.55,0,0,1,.94-.39L19.19,6.8A.55.55,0,0,1,18.8,7.74Z"/><path class="cls-3" d="M7.89,19.13a.45.45,0,0,1-.51-.51V15.69a.45.45,0,0,1,.5-.51.45.45,0,0,1,.5.43.78.78,0,0,1,.35-.32,1.07,1.07,0,0,1,.51-.12,1.17,1.17,0,0,1,.64.18,1.2,1.2,0,0,1,.43.51,2,2,0,0,1,0,1.57A1.2,1.2,0,0,1,8.75,18a.86.86,0,0,1-.35-.3v.91a.5.5,0,0,1-.13.38A.52.52,0,0,1,7.89,19.13Zm1-1.76a.48.48,0,0,0,.38-.18.81.81,0,0,0,.14-.55.82.82,0,0,0-.14-.55.5.5,0,0,0-.38-.17.51.51,0,0,0-.39.17.89.89,0,0,0-.14.55.87.87,0,0,0,.14.55A.48.48,0,0,0,8.92,17.37Z"/><path class="cls-3" d="M12.17,18.11a1.1,1.1,0,0,1-.63-.17,1.22,1.22,0,0,1-.44-.51,2,2,0,0,1,0-1.57,1.22,1.22,0,0,1,.44-.51,1.11,1.11,0,0,1,.63-.18,1.06,1.06,0,0,1,.5.12.91.91,0,0,1,.35.28V14.48a.45.45,0,0,1,.51-.51.49.49,0,0,1,.37.13.5.5,0,0,1,.13.38v3.11a.5.5,0,0,1-1,.08.76.76,0,0,1-.34.32A1.14,1.14,0,0,1,12.17,18.11Zm.33-.74a.48.48,0,0,0,.38-.18.8.8,0,0,0,.15-.55.82.82,0,0,0-.15-.55.5.5,0,0,0-.38-.17.49.49,0,0,0-.38.17.82.82,0,0,0-.15.55.8.8,0,0,0,.15.55A.46.46,0,0,0,12.5,17.37Z"/><path class="cls-3" d="M15.52,18.1a.46.46,0,0,1-.51-.51V16h-.15a.34.34,0,0,1-.39-.38c0-.25.13-.37.39-.37H15a1.2,1.2,0,0,1,.34-.87,1.52,1.52,0,0,1,.92-.36h.17a.39.39,0,0,1,.29,0,.35.35,0,0,1,.15.17.55.55,0,0,1,0,.22.38.38,0,0,1-.09.19.27.27,0,0,1-.18.1h-.08a.66.66,0,0,0-.41.12.41.41,0,0,0-.11.31v.09h.32c.26,0,.39.12.39.37a.34.34,0,0,1-.39.38H16v1.6A.45.45,0,0,1,15.52,18.1Z"/></svg>',
            },
            ticketSelect: {},
            showTable: false,

            // Datos principales
            ticketsInoperativos: 0,
            ticketsOperativos: 0,
            ticketsResueltos: 0,
            porcentajeResolucion: 0,
            tiempoPromedioResolucion: 0,
            ticketsUrgentes: 0,
            ticketsMedios: 0,
            ticketsCorrectivos: 0,
            ticketsPredictivos: 0,

            // Datos de análisis
            analisisPorSala: [],

            currentPage: 1, // Página actual
            itemsPerPage: 10, // Tickets por página

        },
        computed: {
            uniqueFases() {
                return [...new Set(this.tickets.map(ticket => ticket.CodigoFaseTicket))];
            },
            uniqueSalas() {
                return [...new Set(this.salas)];
            },
            filteredTickets() {
                return this.tickets.filter(ticket => {
                    const fechaRegistro = new Date(parseInt(ticket.FechaRegistro.replace('/Date(', '').replace(')/', '')));
                    const startDate = this.filters.startDate ? new Date(this.filters.startDate) : null;
                    const endDate = this.filters.endDate ? new Date(this.filters.endDate + 'T23:59:59') : null;
                    const matchesDate = (!startDate || fechaRegistro >= startDate) && (!endDate || fechaRegistro <= endDate);
                    const matchesFase = this.filters.faseTicket && this.filters.faseTicket !== 'todos' ? ticket.CodigoFaseTicket == this.filters.faseTicket : true;
                    const matchesSala = this.filters.sala && this.filters.sala !== 'todos' ? ticket.Sala.CodSala == this.filters.sala : true;
                    return matchesDate && matchesFase && matchesSala;
                });
            },
            dataTickets() {
                return [...new Set(this.tickets)];
            },
            paginatedTickets() {
                const start = (this.currentPage - 1) * this.itemsPerPage;
                const end = start + this.itemsPerPage;
                return this.filteredTickets.slice(start, end);
            },
            // Total de páginas
            totalPages() {
                return Math.ceil(this.filteredTickets.length / this.itemsPerPage);
            },
        },

        methods: {
            prevPage() {
                if (this.currentPage > 1) this.currentPage--;
            },
            // Cambiar a la página siguiente
            nextPage() {
                if (this.currentPage < this.totalPages) this.currentPage++;
            },
            // Ir a una página específica
            goToPage(page) {
                this.currentPage = page;
            },
            getFormattedDate(date) {
                return date.toISOString().split('T')[0];
            },
            handleFaseClick(ticket) {
                if (ticket.CodigoFaseTicket === 2) {
                    this.openModalDetailAsignacion(ticket.Id, ticket);
                } else if (ticket.CodigoFaseTicket === 3) {
                    this.openModalDetailSeguimiento(ticket.Id, ticket);
                } else {
                    this.openModalDetailTicket(ticket.Id, ticket);
                }
            },
            loadAttachedFile(fileUrl) {
                this.previewList = [];

                // Extraer el nombre del archivo desde la URL
                const fileName = fileUrl.split("/").pop();
                console.log("fileUrl")
                console.log(fileUrl)
                console.log(fileName)

                // Determinar el tipo de archivo
                const fileExtension = fileName.split(".").pop().toLowerCase();
                const fileType = this.getFileType(fileExtension);
                console.log("fileExtension")
                console.log(fileExtension)
                console.log("fileType")
                console.log(fileType)

                const fileData = {
                    id: Date.now() + Math.random(), // ID único
                    name: "Documento Adjunto", // Nombre del archivo
                    type: fileType, // Tipo de archivo (image, pdf, word, etc.)
                    icon: this.getFileIcon(fileType), // Ícono según el tipo
                    typeClass: fileType, // Clase CSS
                    preview: fileType === "image" ? fileUrl : null, // Vista previa para imágenes
                    url: fileUrl
                };
                console.log("fileData")
                console.log(fileData)

                this.previewList.push(fileData);
                console.log("previewList")
                console.log(this.previewList)
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
            openModalDetailTicket(id, item) {
                this.ticketSelect = item;
                this.previewList = [];

                this.obtenerDetalleTicket(id)
                if (item.Adjunto) {
                    this.loadAttachedFile(item.AdjuntoFullPath);
                }
                this.showModalDetalle = true;
                document.documentElement.style.overflow = "hidden";
            },
            openModalDetailAsignacion(id, item) {
                this.ticketSelect = item;
                console.log("ticketSelect")
                console.log(this.ticketSelect)
                this.obtenerDetalleTicket(id)
                this.showModalDetailAsignacion = true;

                document.documentElement.style.overflow = "hidden";
            },
            openModalDetailSeguimiento(id, item) {
                this.ticketSelect = item;
                this.obtenerDetalleTicket(id)
                //this.obtenerDetalleSeguimiento(id)
                //this.obtenerDetalleAsignacion(id)
                this.showModalSeguimiento = true;

                document.documentElement.style.overflow = "hidden";
            },
            closeModalDetailTicket() {
                this.showModalDetalle = false
                document.documentElement.style.overflow = "auto"
            },
            closeModalDetailAsignacion() {
                this.showModalDetailAsignacion = false
                document.documentElement.style.overflow = "auto"
            },
            closeModalDetailSeguimiento() {
                this.showModalSeguimiento = false
                document.documentElement.style.overflow = "auto"
            },
            obtenerDetalleTicket(idTicket) {
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
                        var datos = result.data
                        var datos2 = result.data.Asignacion
                        var datos3 = result.data.Seguimientos
                        vm.ticketsQueAsigne = datos
                        vm.ticketsDetail = datos
                        vm.ticketsAsignadosDetail = datos2
                        vm.ticketsSeguimientosDetail = datos3
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
            formatFecha(fecha) {
                const date = new Date(parseInt(fecha.replace('/Date(', '').replace(')/', '')));
                return date.toLocaleDateString();
            },
            cambiarTab(tabName) {
                // Destruir gráficos antes de cambiar de pestaña
                if (this.estadoChart) {
                    this.estadoChart.destroy();
                }
                if (this.problemaChart) {
                    this.problemaChart.destroy();
                }
                if (this.zonaChart) {
                    this.zonaChart.destroy();
                }


                this.activeTab = tabName;
                if (tabName === 'sinAsignarTickets') {
                    this.$nextTick(() => {
                        this.renderCharts();
                    });
                }
            },
            GetTicketsReportes() {
                var vm = this;
                const codsFases = this.filters.faseTicket === 'todos'
                    ? this.fasesTicket.map(fase => fase.id)
                    : [Number(this.filters.faseTicket)];

                const codsSalas = this.filters.sala === 'todos'
                    ? this.salas.map(s => s.CodSala)
                    : [this.filters.sala];

                const params = {
                    FechaInicio: this.filters.startDate,
                    FechaFin: this.filters.endDate,
                    CodsSalas: codsSalas,
                    FasesTicket: codsFases,
                };
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/GetTicketsReportes",
                    data: JSON.stringify(params),
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        var datos = result.data
                        vm.tickets = datos
                        vm.dataTickets = datos
                        vm.filteredTickets = datos
                        console.log("vm.ticketsreport")
                        console.log(vm.tickets)
                        //vm.calcularKPIs();
                        //vm.calcularAnalisisPorSala();
                        //vm.renderCharts();
                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            inicializarDashboard() {
                this.calcularKPIs();
                this.calcularAnalisisPorSala();
                this.renderCharts();
            },
            GenerarExcelTicketsReportes() {
                var vm = this;

                const codsFases = this.filters.faseTicket === 'todos'
                    ? this.fasesTicket.map(fase => fase.id)
                    : [Number(this.filters.faseTicket)];

                const codsSalas = this.filters.sala === 'todos'
                    ? this.salas.map(s => s.CodSala)
                    : [this.filters.sala];

                const params = {
                    fechaInicio: this.filters.startDate,
                    fechaFin: this.filters.endDate,
                    CodsSalas: codsSalas,
                    FasesTicket: codsFases,
                };
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/GenerarExcelTicketsReportes",
                    data: JSON.stringify(params),
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (response) {
                        if (response.success) {
                            var data = response.bytes;  // El archivo en base64 que viene como 'bytes'
                            var file = response.fileInfo.FileName;  // Nombre del archivo
                            let a = document.createElement('a');
                            a.target = '_self';
                            a.href = "data:application/vnd.ms-excel;base64," + data;
                            a.download = file;
                            a.click();
                        } else {
                            toastr.error(response.displayMessage, "Mensaje Servidor");
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
            obtenerSalas() {
                const self = this; // Guardar una referencia al `this` de Vue
                $.ajax({
                    type: "POST",
                    url: basePath + "Sala/ListadoSalaPorUsuarioJson",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: (result) => { // Usar una función de flecha para mantener el contexto de `this`
                        self.salas = result.data.map(sala => ({
                            CodSala: sala.CodSala,
                            Nombre: sala.Nombre || `Sala ${sala.CodSala}`
                        }));
                        console.log("self.salas", self.salas);
                        console.log("self.salas", self.salas[0].CodSala);
                        console.log("self.salas", self.salas[0].Nombre);
                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor");
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide");
                    }
                });
            },
            //calcularKPIs() {
            //    // KPIs principales
            //    this.totalTickets = this.tickets.length;
            //    this.ticketsInoperativos = this.tickets.filter(t => t.EstadoActual.Nombre === "Inoperativo").length;
            //    this.ticketsOperativos = this.tickets.filter(t => t.EstadoActual.Nombre === "Operativo").length;
            //    this.ticketsResueltos = this.tickets.filter(t =>
            //        t.Seguimientos.some(s => s.EstadoTicketActual.Nombre === "Resuelto")
            //    ).length;

            //    // Cálculo de porcentaje de resolución
            //    this.porcentajeResolucion = ((this.ticketsResueltos / this.totalTickets) * 100).toFixed(1);

            //    // Tiempo promedio de resolución
            //    this.tiempoPromedioResolucion = this.calcularTiempoPromedioResolucion();

            //    // Tickets por nivel de atención
            //    this.ticketsUrgentes = this.tickets.filter(t => t.NivelAtencion.Nombre === "Alto").length;
            //    this.ticketsMedios = this.tickets.filter(t => t.NivelAtencion.Nombre === "Medio").length;

            //    // KPIs por tipo de operación
            //    this.ticketsCorrectivos = this.tickets.filter(t => t.TipoOperacion.Nombre === "Correctivo").length;
            //    this.ticketsPredictivos = this.tickets.filter(t => t.TipoOperacion.Nombre === "Predictivo").length;
            //},

            calcularTiempoPromedioResolucion() {
                const ticketsResueltos = this.tickets.filter(t =>
                    t.Seguimientos.some(s => s.EstadoTicketActual.Nombre === "Resuelto")
                );

                if (ticketsResueltos.length === 0) return 0;

                const tiemposTotales = ticketsResueltos.map(ticket => {
                    const fechaCreacion = new Date(parseInt(ticket.FechaRegistro.substr(6)));
                    const seguimientoResolucion = ticket.Seguimientos.find(s =>
                        s.EstadoTicketActual.Nombre === "Resuelto"
                    );
                    const fechaResolucion = new Date(parseInt(seguimientoResolucion.FechaRegistro.substr(6)));
                    return (fechaResolucion - fechaCreacion) / (1000 * 60 * 60); // Horas
                });

                return (tiemposTotales.reduce((a, b) => a + b, 0) / tiemposTotales.length).toFixed(1);
            },

            renderCharts() {
                if (!this.$refs.estadoChart || !this.$refs.problemaChart || !this.$refs.zonaChart) {
                    console.error("Uno o más elementos <canvas> no están disponibles.");
                    return;
                }

                this.renderEstadoChart();
                this.renderProblemaChart();
                this.renderZonaChart();
            },

            renderEstadoChart() {
                const ctx = this.$refs.estadoChart.getContext('2d');
                const data = {
                    labels: ['Operativos', 'Inoperativos', 'Resueltos'],
                    datasets: [{
                        data: [this.ticketsOperativos, this.ticketsInoperativos, this.ticketsResueltos],
                        backgroundColor: ['#28a745', '#dc3545', '#17a2b8'],
                        borderWidth: 1
                    }]
                };

                this.estadoChart = new Chart(ctx, {
                    type: 'doughnut',
                    data: data,
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: {
                                position: 'bottom'
                            }
                        }
                    }
                });
            },

            renderProblemaChart() {
                const ctx = this.$refs.problemaChart.getContext('2d');
                const problemasAgrupados = this.tickets.reduce((acc, ticket) => {
                    const problema = ticket.ClasificacionProblema.Nombre;
                    acc[problema] = (acc[problema] || 0) + 1;
                    return acc;
                }, {});

                const data = {
                    labels: Object.keys(problemasAgrupados),
                    datasets: [{
                        data: Object.values(problemasAgrupados),
                        backgroundColor: [
                            '#4e73df', '#1cc88a', '#36b9cc', '#f6c23e', '#e74a3b'
                        ]
                    }]
                };

                this.problemaChart = new Chart(ctx, {
                    type: 'pie',
                    data: data,
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: {
                                position: 'bottom'
                            }
                        }
                    }
                });
            },

            renderZonaChart() {
                const ctx = this.$refs.zonaChart.getContext('2d');
                const zonasAgrupadas = this.tickets.reduce((acc, ticket) => {
                    const sala = ticket.Sala.Nombre;
                    acc[sala] = (acc[sala] || 0) + 1;
                    return acc;
                }, {});

                const data = {
                    labels: Object.keys(zonasAgrupadas),
                    datasets: [{
                        label: 'Tickets por Sala',
                        data: Object.values(zonasAgrupadas),
                        backgroundColor: 'rgba(78, 115, 223, 0.2)',
                        borderColor: 'rgba(78, 115, 223, 1)',
                        borderWidth: 1
                    }]
                };

                this.zonaChart = new Chart(ctx, {
                    type: 'bar',
                    data: data,
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        scales: {
                            y: {
                                beginAtZero: true,
                                ticks: {
                                    stepSize: 1
                                }
                            }
                        }
                    }
                });
            },
            calcularKPIs() {
                this.totalTickets = this.tickets.length;
                this.ticketsInoperativos = this.tickets.filter(t => t.EstadoActual.Nombre === "Inoperativo").length;
                this.ticketsOperativos = this.tickets.filter(t => t.EstadoActual.Nombre === "Operativo").length;
                this.ticketsResueltos = this.tickets.filter(t =>
                    t.Seguimientos.some(s => s.EstadoTicketActual.Nombre === "Resuelto")
                ).length;

                this.porcentajeResolucion = ((this.ticketsResueltos / this.totalTickets) * 100).toFixed(1);
                this.tiempoPromedioResolucion = this.calcularTiempoPromedioResolucion();
                this.ticketsUrgentes = this.tickets.filter(t => t.NivelAtencion.Nombre === "Alto").length;
                this.ticketsMedios = this.tickets.filter(t => t.NivelAtencion.Nombre === "Medio").length;
                this.ticketsCorrectivos = this.tickets.filter(t => t.TipoOperacion.Nombre === "Correctivo").length;
                this.ticketsPredictivos = this.tickets.filter(t => t.TipoOperacion.Nombre === "Predictivo").length;
            },

            calcularAnalisisPorSala() {
                const salaAgrupada = {};

                // Agrupar tickets por sala
                this.tickets.forEach(ticket => {
                    const salaNombre = ticket.Sala.Nombre;
                    if (!salaAgrupada[salaNombre]) {
                        salaAgrupada[salaNombre] = {
                            tickets: [],
                            nombre: salaNombre
                        };
                    }
                    salaAgrupada[salaNombre].tickets.push(ticket);
                });

                // Calcular métricas por sala
                this.analisisPorSala = Object.values(salaAgrupada).map(sala => {
                    const tickets = sala.tickets;
                    const ticketsResueltos = tickets.filter(t =>
                        t.Seguimientos.some(s => s.EstadoTicketActual.Nombre === "Resuelto")
                    );

                    // Cálculo de tiempos
                    const tiemposResolucion = ticketsResueltos.map(ticket => {
                        const fechaCreacion = new Date(parseInt(ticket.FechaRegistro.substr(6)));
                        const seguimientoResolucion = ticket.Seguimientos.find(s =>
                            s.EstadoTicketActual.Nombre === "Resuelto"
                        );
                        if (!seguimientoResolucion) return 0;
                        const fechaResolucion = new Date(parseInt(seguimientoResolucion.FechaRegistro.substr(6)));
                        return (fechaResolucion - fechaCreacion) / (1000 * 60 * 60);
                    }).filter(tiempo => tiempo > 0);

                    const tiempoPromedio = tiemposResolucion.length ?
                        (tiemposResolucion.reduce((a, b) => a + b, 0) / tiemposResolucion.length) : 0;

                    const ticketsCriticos = tickets.filter(t => t.NivelAtencion.Nombre === "Alto").length;
                    const ticketsEnProceso = tickets.filter(t =>
                        t.Asignacion && t.Asignacion.EstadoTicket &&
                        t.Asignacion.EstadoTicket.Nombre === "En curso - Asignada"
                    ).length;

                    return {
                        nombre: sala.nombre,
                        tickets: tickets,
                        tiempoPromedioResolucion: tiempoPromedio.toFixed(1),
                        tasaResolucion: ((ticketsResueltos.length / tickets.length) * 100).toFixed(1),
                        ticketsActivos: tickets.length - ticketsResueltos.length,
                        porcentajeCriticos: ((ticketsCriticos / tickets.length) * 100).toFixed(1),
                        porcentajeEnProceso: ((ticketsEnProceso / tickets.length) * 100).toFixed(1),
                        porcentajeResueltos: ((ticketsResueltos.length / tickets.length) * 100).toFixed(1),
                        indicadorAtencion: this.calcularIndicadorAtencion(tickets),
                        tendencia: this.calcularTendencia(tickets)
                    };
                });
            },

            calcularTiempoPromedioResolucion() {
                const ticketsResueltos = this.tickets.filter(t =>
                    t.Seguimientos.some(s => s.EstadoTicketActual.Nombre === "Resuelto")
                );

                if (ticketsResueltos.length === 0) return 0;

                const tiemposTotales = ticketsResueltos.map(ticket => {
                    const fechaCreacion = new Date(parseInt(ticket.FechaRegistro.substr(6)));
                    const seguimientoResolucion = ticket.Seguimientos.find(s =>
                        s.EstadoTicketActual.Nombre === "Resuelto"
                    );
                    if (!seguimientoResolucion) return 0;
                    const fechaResolucion = new Date(parseInt(seguimientoResolucion.FechaRegistro.substr(6)));
                    return (fechaResolucion - fechaCreacion) / (1000 * 60 * 60);
                }).filter(tiempo => tiempo > 0);

                if (tiemposTotales.length === 0) return 0;
                return (tiemposTotales.reduce((a, b) => a + b, 0) / tiemposTotales.length).toFixed(1);
            },

            calcularIndicadorAtencion(tickets) {
                if (!tickets.length) return 0;

                const pesoResolucion = 0.4;
                const pesoCriticos = 0.3;
                const pesoTiempo = 0.3;

                const ticketsResueltos = tickets.filter(t =>
                    t.Seguimientos.some(s => s.EstadoTicketActual.Nombre === "Resuelto")
                );
                const tasaResolucion = ticketsResueltos.length / tickets.length;

                const ticketsCriticos = tickets.filter(t => t.NivelAtencion.Nombre === "Alto");
                const porcentajeCriticos = ticketsCriticos.length / tickets.length;

                const tiemposResolucion = ticketsResueltos
                    .map(ticket => {
                        const fechaCreacion = new Date(parseInt(ticket.FechaRegistro.substr(6)));
                        const seguimientoResolucion = ticket.Seguimientos.find(s =>
                            s.EstadoTicketActual.Nombre === "Resuelto"
                        );
                        if (!seguimientoResolucion) return 0;
                        const fechaResolucion = new Date(parseInt(seguimientoResolucion.FechaRegistro.substr(6)));
                        return (fechaResolucion - fechaCreacion) / (1000 * 60 * 60);
                    })
                    .filter(tiempo => tiempo > 0);

                const tiempoPromedio = tiemposResolucion.length ?
                    tiemposResolucion.reduce((a, b) => a + b, 0) / tiemposResolucion.length : 24;

                const indicador = (
                    (tasaResolucion * pesoResolucion) +
                    ((1 - porcentajeCriticos) * pesoCriticos) +
                    (tiempoPromedio < 24 ? 1 : 24 / tiempoPromedio) * pesoTiempo
                ) * 100;

                return Math.min(100, indicador).toFixed(1);
            },

            calcularTendencia(tickets) {
                const fechaActual = new Date();
                const unaSemanaPasada = new Date(fechaActual - 7 * 24 * 60 * 60 * 1000);
                const dosSemanaPasada = new Date(fechaActual - 14 * 24 * 60 * 60 * 1000);

                const ticketsUltimaSemana = tickets.filter(t =>
                    new Date(parseInt(t.FechaRegistro.substr(6))) > unaSemanaPasada
                ).length;

                const ticketsSemanaPasada = tickets.filter(t => {
                    const fecha = new Date(parseInt(t.FechaRegistro.substr(6)));
                    return fecha <= unaSemanaPasada && fecha > dosSemanaPasada;
                }).length;

                if (ticketsSemanaPasada === 0) return 0;

                const tendencia = ((ticketsUltimaSemana - ticketsSemanaPasada) / ticketsSemanaPasada) * 100;
                return tendencia.toFixed(1);
            },

            getBadgeClass(indicador) {
                if (indicador >= 80) return 'badge-success';
                if (indicador >= 60) return 'badge-warning';
                return 'badge-danger';
            },

            getTrendClass(tendencia) {
                if (tendencia > 0) return 'trend-positive';
                if (tendencia < 0) return 'trend-negative';
                return 'trend-neutral';
            },

            getTrendIcon(tendencia) {
                if (tendencia > 0) return 'glyphicon glyphicon-arrow-up';
                if (tendencia < 0) return 'glyphicon glyphicon-arrow-down';
                return 'glyphicon glyphicon-minus';
            },

            toggleView() {
                this.showTable = !this.showTable;
            },

            showTooltip(event, sala) {
                const tooltip = `
                <div class="tooltip-content">
                    <h4>${sala.nombre}</h4>
                    <hr>
                    <p><strong>Detalles de Rendimiento:</strong></p>
                    <ul>
                        <li>SLA Cumplimiento: ${this.calcularSLACumplimiento(sala.tickets)}%</li>
                        <li>Tickets Pendientes: ${this.calcularTicketsPendientes(sala.tickets)}</li>
                        <li>Tiempo Medio Respuesta: ${this.calcularTiempoMedioRespuesta(sala.tickets)}h</li>
                    </ul>
                </div>
            `;

                $(event.currentTarget).tooltip({
                    title: tooltip,
                    html: true,
                    placement: 'auto',
                    container: 'body'
                }).tooltip('show');
            },

            hideTooltip(event) {
                $(event.currentTarget).tooltip('hide');
            },

            calcularSLACumplimiento(tickets) {
                const ticketsConSLA = tickets.filter(t => {
                    if (!t.Asignacion || !t.Asignacion.FechaTentativaTermino) return false;
                    const fechaTermino = new Date(parseInt(t.Asignacion.FechaTentativaTermino.substr(6)));
                    const fechaResolucion = t.Seguimientos.find(s => s.EstadoTicketActual.Nombre === "Resuelto");
                    if (!fechaResolucion) return false;
                    const fechaReal = new Date(parseInt(fechaResolucion.FechaRegistro.substr(6)));
                    return fechaReal <= fechaTermino;
                });

                if (tickets.length === 0) return 0;
                return ((ticketsConSLA.length / tickets.length) * 100).toFixed(1);
            },

            calcularTicketsPendientes(tickets) {
                return tickets.filter(t =>
                    !t.Seguimientos.some(s => s.EstadoTicketActual.Nombre === "Resuelto")
                ).length;
            },

            calcularTiempoMedioRespuesta(tickets) {
                const tiemposRespuesta = tickets
                    .filter(t => t.Asignacion && t.Asignacion.FechaRegistro)
                    .map(ticket => {
                        const fechaCreacion = new Date(parseInt(ticket.FechaRegistro.substr(6)));
                        const fechaAsignacion = new Date(parseInt(ticket.Asignacion.FechaRegistro.substr(6)));
                        return (fechaAsignacion - fechaCreacion) / (1000 * 60 * 60);
                    });

                if (!tiemposRespuesta.length) return 0;
                return (tiemposRespuesta.reduce((a, b) => a + b, 0) / tiemposRespuesta.length).toFixed(1);
            },
            //calcularKPIs() {
            //    this.totalTickets = this.tickets.length;
            //},
            //renderCharts() {
            //    if (!this.$refs.estadoChart || !this.$refs.problemaChart || !this.$refs.zonaChart) {
            //        console.error("Uno o más elementos <canvas> no están disponibles.");
            //        return;
            //    }

            //    // Destruir gráficos existentes si los hay
            //    if (this.estadoChart) {
            //        this.estadoChart.destroy();
            //    }
            //    if (this.problemaChart) {
            //        this.problemaChart.destroy();
            //    }
            //    if (this.zonaChart) {
            //        this.zonaChart.destroy();
            //    }

            //    this.renderEstadoChart();
            //    this.renderProblemaChart();
            //    this.renderZonaChart();
            //},
            //renderEstadoChart() {
            //    if (this.estadoChart) this.estadoChart.destroy(); // Destruir el gráfico anterior si existe
            //    const ctx = this.$refs.estadoChart.getContext('2d');
            //    const estados = this.tickets.map(ticket => ticket.EstadoActual.Nombre);
            //    const estadoCounts = estados.reduce((acc, estado) => {
            //        acc[estado] = (acc[estado] || 0) + 1;
            //        return acc;
            //    }, {});

            //    this.estadoChart = new Chart(ctx, {
            //        type: 'bar',
            //        data: {
            //            labels: Object.keys(estadoCounts),
            //            datasets: [{
            //                label: 'Tickets por Estado',
            //                data: Object.values(estadoCounts),
            //                backgroundColor: 'rgba(75, 192, 192, 0.2)',
            //                borderColor: 'rgba(75, 192, 192, 1)',
            //                borderWidth: 1
            //            }]
            //        },
            //        options: {
            //            scales: {
            //                y: {
            //                    beginAtZero: true
            //                }
            //            }
            //        }
            //    });
            //},
            //renderProblemaChart() {
            //    if (this.problemaChart) this.problemaChart.destroy(); // Destruir el gráfico anterior si existe
            //    const ctx = this.$refs.problemaChart.getContext('2d');
            //    const problemas = this.tickets.map(ticket => ticket.ClasificacionProblema.Nombre);
            //    const problemaCounts = problemas.reduce((acc, problema) => {
            //        acc[problema] = (acc[problema] || 0) + 1;
            //        return acc;
            //    }, {});

            //    this.problemaChart = new Chart(ctx, {
            //        type: 'pie',
            //        data: {
            //            labels: Object.keys(problemaCounts),
            //            datasets: [{
            //                label: 'Tickets por Tipo de Problema',
            //                data: Object.values(problemaCounts),
            //                backgroundColor: [
            //                    'rgba(255, 99, 132, 0.2)',
            //                    'rgba(54, 162, 235, 0.2)',
            //                    'rgba(255, 206, 86, 0.2)',
            //                    'rgba(75, 192, 192, 0.2)',
            //                    'rgba(153, 102, 255, 0.2)',
            //                    'rgba(255, 159, 64, 0.2)'
            //                ],
            //                borderColor: [
            //                    'rgba(255, 99, 132, 1)',
            //                    'rgba(54, 162, 235, 1)',
            //                    'rgba(255, 206, 86, 1)',
            //                    'rgba(75, 192, 192, 1)',
            //                    'rgba(153, 102, 255, 1)',
            //                    'rgba(255, 159, 64, 1)'
            //                ],
            //                borderWidth: 1
            //            }]
            //        }
            //    });
            //},
            //renderZonaChart() {
            //    if (this.zonaChart) this.zonaChart.destroy(); // Destruir el gráfico anterior si existe
            //    const ctx = this.$refs.zonaChart.getContext('2d');
            //    const zonas = this.tickets.map(ticket => ticket.Sala.Nombre);
            //    const zonaCounts = zonas.reduce((acc, zona) => {
            //        acc[zona] = (acc[zona] || 0) + 1;
            //        return acc;
            //    }, {});

            //    this.zonaChart = new Chart(ctx, {
            //        type: 'line',
            //        data: {
            //            labels: Object.keys(zonaCounts),
            //            datasets: [{
            //                label: 'Tickets por Zona/Sala',
            //                data: Object.values(zonaCounts),
            //                backgroundColor: 'rgba(153, 102, 255, 0.2)',
            //                borderColor: 'rgba(153, 102, 255, 1)',
            //                borderWidth: 1
            //            }]
            //        },
            //        options: {
            //            scales: {
            //                y: {
            //                    beginAtZero: true
            //                }
            //            }
            //        }
            //    });
            //},
            setFilters() {
                const vm = this;
                const today = new Date();
                this.filters.startDate = new Date(today.getFullYear(), today.getMonth(), 1).toISOString().split('T')[0];
                this.filters.endDate = today.toISOString().split('T')[0];
                this.filters.faseTicket = 'todos';
                this.filters.sala = 'todos';
            },
            setFases() {

            }
        },
        created: function () {
            const vm = this;

        },

        mounted: function () {
            this.setFilters()
            //this.obtenerTickets();
            this.setFases()
            this.obtenerSalas()
            //this.inicializarDashboard();

            $("#select_salas").select2({
                multiple: false,
                allowClear: true,
                placeholder: "Seleccione una opción"
            });

            $("#select_salas")
                .select2()
                .on("change", (e) => {
                    this.filters.sala = e.target.value; // sincroniza con Vue
                })
            /*this.GetTicketsReportes()*/


            //this.obtenerTicketsQueAsigne()
            //this.obtenerTicketsQueMeAsignaron();

        },
        watch: {
            salas: {
                handler() {
                    console.log("Los tickets han cambiado, recalculando valores...");
                    //this.filters.startDate = ''
                    //this.filters.endDate = ''
                    this.GetTicketsReportes()
                },
                deep: true
            },
            filteredTickets: {
                handler() {
                    this.filteredTickets = this.dataTickets
                }
            },
            tickets: {
                handler(newVal) {
                    if (newVal && newVal.length > 0) {
                        this.inicializarDashboard();
                    }
                },
                immediate: true
            },
            //'filters.faseTicket': 'GetTicketsReportes',
            //'filters.sala': 'GetTicketsReportes',
            //'filters.startDate': 'GetTicketsReportes',
            //'filters.endDate': 'GetTicketsReportes'
        },
        //props: {
        //    tickets: {
        //        type: Array,
        //        required: true,
        //        default: () => []
        //    },
        //    activeTab: {
        //        type: String,
        //        required: true
        //    }
        //},


    })
})

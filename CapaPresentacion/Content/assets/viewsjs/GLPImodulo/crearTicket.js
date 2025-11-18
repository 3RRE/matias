document.addEventListener('DOMContentLoaded', function () {
    toastr.options = {
        preventDuplicates: true,
    }



    var AppConfiguracionAlerta = new Vue({
        el: '#AppGlpiCrearTicket',
        data: {
            modalOperacion: false,
            modalAtencion: false,
            modalPartida: false,
            modalRubro: false,
            modalSubCategoria: false,
            modalClasificacionProblema: false,
            modalEstadoActual: false,
            modalIdentificador: false,
            dataSelects: {},
            previewList: [],
            nuevoCorreo: "",
            correos: [],
            correosBD: [],
            file: null,
            sheet: false,
          
            tickets: [],
            ticketsfinal: [],
            ticketSelect: {},
            showModal: false,
            showConfirmationModal: false,
            historial: [],
            detalleSeg: [],
            ticketSelect: {},
            showCierreModal: false,
            showConfirmacionCierreModal: false,
            estadosTicket: [],
            cierreTicket: {
                ticketIdToDelete: null,
            },
            isEditing: false,
            currentTicketId: null,
            showModalDetailSeguimiento: false,
            correosFiltrados: [],
            ticketsCreados: [],
            ticketsRecibidos: [],
            ticketsAsignados: [],
            ticketsDetail: [],
            ticketsAsignadosDetail: [],
            ticketsSeguimientosDetail: [],
            iconFiles: {
                excel: '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 32 32"><defs><linearGradient id="a" x1="4.494" y1="-2092.086" x2="13.832" y2="-2075.914" gradientTransform="translate(0 2100)" gradientUnits="userSpaceOnUse"><stop offset="0" stop-color="#18884f"/><stop offset="0.5" stop-color="#117e43"/><stop offset="1" stop-color="#0b6631"/></linearGradient></defs><title>file_type_excel</title><path d="M19.581,15.35,8.512,13.4V27.809A1.192,1.192,0,0,0,9.705,29h19.1A1.192,1.192,0,0,0,30,27.809h0V22.5Z" style="fill:#185c37"/><path d="M19.581,3H9.705A1.192,1.192,0,0,0,8.512,4.191h0V9.5L19.581,16l5.861,1.95L30,16V9.5Z" style="fill:#21a366"/><path d="M8.512,9.5H19.581V16H8.512Z" style="fill:#107c41"/><path d="M16.434,8.2H8.512V24.45h7.922a1.2,1.2,0,0,0,1.194-1.191V9.391A1.2,1.2,0,0,0,16.434,8.2Z" style="opacity:0.10000000149011612;isolation:isolate"/><path d="M15.783,8.85H8.512V25.1h7.271a1.2,1.2,0,0,0,1.194-1.191V10.041A1.2,1.2,0,0,0,15.783,8.85Z" style="opacity:0.20000000298023224;isolation:isolate"/><path d="M15.783,8.85H8.512V23.8h7.271a1.2,1.2,0,0,0,1.194-1.191V10.041A1.2,1.2,0,0,0,15.783,8.85Z" style="opacity:0.20000000298023224;isolation:isolate"/><path d="M15.132,8.85H8.512V23.8h6.62a1.2,1.2,0,0,0,1.194-1.191V10.041A1.2,1.2,0,0,0,15.132,8.85Z" style="opacity:0.20000000298023224;isolation:isolate"/><path d="M3.194,8.85H15.132a1.193,1.193,0,0,1,1.194,1.191V21.959a1.193,1.193,0,0,1-1.194,1.191H3.194A1.192,1.192,0,0,1,2,21.959V10.041A1.192,1.192,0,0,1,3.194,8.85Z" style="fill:url(#a)"/><path d="M5.7,19.873l2.511-3.884-2.3-3.862H7.758L9.013,14.6c.116.234.2.408.238.524h.017c.082-.188.169-.369.26-.546l1.342-2.447h1.7l-2.359,3.84,2.419,3.905H10.821l-1.45-2.711A2.355,2.355,0,0,1,9.2,16.8H9.176a1.688,1.688,0,0,1-.168.351L7.515,19.873Z" style="fill:#fff"/><path d="M28.806,3H19.581V9.5H30V4.191A1.192,1.192,0,0,0,28.806,3Z" style="fill:#33c481"/><path d="M19.581,16H30v6.5H19.581Z" style="fill:#107c41"/></svg>',
                word: '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" id="Capa_1" style="enable-background:new 0 0 128 128;" version="1.1" viewBox="0 0 128 128" xml:space="preserve"><style type="text/css">.st0{fill:#21A365;} .st1{fill:#107C41;} .st2{fill:#185B37;} .st3{fill:#33C481;} .st4{fill:#17864C;} .st5{fill:#FFFFFF;} .st6{fill:#036C70;} .st7{fill:#1A9BA1;} .st8{fill:#37C6D0;} .st9{fill:#04878B;} .st10{fill:#4F59CA;} .st11{fill:#7B82EA;} .st12{fill:#4C53BB;} .st13{fill:#0F78D5;} .st14{fill:#29A7EB;} .st15{fill:#0358A8;} .st16{fill:#0F79D6;} .st17{fill:#038387;} .st18{fill:#048A8E;} .st19{fill:#C8421D;} .st20{fill:#FF8F6A;} .st21{fill:#ED6B47;} .st22{fill:#891323;} .st23{fill:#AF2131;} .st24{fill:#C94E60;} .st25{fill:#E08195;} .st26{fill:#B42839;} .st27{fill:#0464B8;} .st28{fill:#0377D4;} .st29{fill:#4FD8FF;} .st30{fill:#1681D7;} .st31{fill:#0178D4;} .st32{fill:#042071;} .st33{fill:#168FDE;} .st34{fill:#CA64EA;} .st35{fill:#7E1FAF;} .st36{fill:#AE4BD5;} .st37{fill:#9332BF;} .st38{fill:#7719AA;} .st39{fill:#0078D4;} .st40{fill:#1490DF;} .st41{fill:#0364B8;} .st42{fill:#28A8EA;} .st43{fill:#41A5ED;} .st44{fill:#2C7BD5;} .st45{fill:#195ABE;} .st46{fill:#103E91;} .st47{fill:#2166C3;} .st48{opacity:0.2;}</style><path class="st43" d="M128,34.2H29.6V10.3c0-3.2,2.6-5.8,5.8-5.8h86.7c3.2,0,5.8,2.6,5.8,5.8V34.2z"/><rect class="st44" height="29.8" width="98.4" x="29.6" y="34.2"/><rect class="st45" height="29.8" width="98.4" x="29.6" y="64"/><path class="st46" d="M122.2,123.5H35.5c-3.2,0-5.8-2.6-5.8-5.8V93.8H128v23.9C128,120.9,125.4,123.5,122.2,123.5z"/><path class="st47" d="M59.5,96.5h-53c-3.5,0-6.4-2.9-6.4-6.4V37.9c0-3.5,2.9-6.4,6.4-6.4h53c3.5,0,6.4,2.9,6.4,6.4v52.2  C65.9,93.6,63.1,96.5,59.5,96.5z"/><g><path class="st5" d="M19.3,82.4l-8.9-35.9h7.1l3.5,16.3c0.9,4.4,1.8,8.9,2.4,12.5h0.1c0.6-3.8,1.6-8,2.6-12.6L30,46.5h7L40.6,63   c0.9,4.3,1.7,8.2,2.2,12.1h0.1c0.6-4,1.5-8,2.5-12.4l3.8-16.2h6.8l-9.8,35.9H39l-3.8-16.9c-0.9-4-1.6-7.4-2.1-11.3h-0.1   c-0.6,3.8-1.3,7.3-2.4,11.4l-4.2,16.9H19.3z"/></g><path class="st48" d="M65.9,37.3c0,0.2,0,0.4,0,0.6v52.2c0,3.5-2.9,6.4-6.4,6.4H29.7v5.7h35.2c3.5,0,6.4-2.9,6.4-6.4V43.6  C71.3,40.4,69,37.7,65.9,37.3z"/></svg>',
                pdf: ' <svg xmlns="http://www.w3.org/2000/svg" data-name="Layer 1" id="Layer_1" viewBox="0 0 24 24"><defs><style>.cls-1{fill:#f44336;}.cls-2{fill:#ff8a80;}.cls-3{fill:#ffebee;}</style></defs><title/><path class="cls-1" d="M16.5,22h-9a3,3,0,0,1-3-3V5a3,3,0,0,1,3-3h6.59a1,1,0,0,1,.7.29l4.42,4.42a1,1,0,0,1,.29.7V19A3,3,0,0,1,16.5,22Z"/><path class="cls-2" d="M18.8,7.74H15.2a1.5,1.5,0,0,1-1.5-1.5V2.64a.55.55,0,0,1,.94-.39L19.19,6.8A.55.55,0,0,1,18.8,7.74Z"/><path class="cls-3" d="M7.89,19.13a.45.45,0,0,1-.51-.51V15.69a.45.45,0,0,1,.5-.51.45.45,0,0,1,.5.43.78.78,0,0,1,.35-.32,1.07,1.07,0,0,1,.51-.12,1.17,1.17,0,0,1,.64.18,1.2,1.2,0,0,1,.43.51,2,2,0,0,1,0,1.57A1.2,1.2,0,0,1,8.75,18a.86.86,0,0,1-.35-.3v.91a.5.5,0,0,1-.13.38A.52.52,0,0,1,7.89,19.13Zm1-1.76a.48.48,0,0,0,.38-.18.81.81,0,0,0,.14-.55.82.82,0,0,0-.14-.55.5.5,0,0,0-.38-.17.51.51,0,0,0-.39.17.89.89,0,0,0-.14.55.87.87,0,0,0,.14.55A.48.48,0,0,0,8.92,17.37Z"/><path class="cls-3" d="M12.17,18.11a1.1,1.1,0,0,1-.63-.17,1.22,1.22,0,0,1-.44-.51,2,2,0,0,1,0-1.57,1.22,1.22,0,0,1,.44-.51,1.11,1.11,0,0,1,.63-.18,1.06,1.06,0,0,1,.5.12.91.91,0,0,1,.35.28V14.48a.45.45,0,0,1,.51-.51.49.49,0,0,1,.37.13.5.5,0,0,1,.13.38v3.11a.5.5,0,0,1-1,.08.76.76,0,0,1-.34.32A1.14,1.14,0,0,1,12.17,18.11Zm.33-.74a.48.48,0,0,0,.38-.18.8.8,0,0,0,.15-.55.82.82,0,0,0-.15-.55.5.5,0,0,0-.38-.17.49.49,0,0,0-.38.17.82.82,0,0,0-.15.55.8.8,0,0,0,.15.55A.46.46,0,0,0,12.5,17.37Z"/><path class="cls-3" d="M15.52,18.1a.46.46,0,0,1-.51-.51V16h-.15a.34.34,0,0,1-.39-.38c0-.25.13-.37.39-.37H15a1.2,1.2,0,0,1,.34-.87,1.52,1.52,0,0,1,.92-.36h.17a.39.39,0,0,1,.29,0,.35.35,0,0,1,.15.17.55.55,0,0,1,0,.22.38.38,0,0,1-.09.19.27.27,0,0,1-.18.1h-.08a.66.66,0,0,0-.41.12.41.41,0,0,0-.11.31v.09h.32c.26,0,.39.12.39.37a.34.34,0,0,1-.39.38H16v1.6A.45.45,0,0,1,15.52,18.1Z"/></svg>',
            },
            showDeleteTicket: false,
            //return:{
            //    previewList: [],
            //},
            operacion: {
                isEditingOperacion: false,
                nuevoOperacionNombre: '',
                EditadoNombre: '',
                EditadoIndex: null,
            },
            tipoOperacion: [],
            clasificacion: {
                isEditingClasificacion: false,
                nuevoClasificacionNombre: '',
                EditadoNombre: '',
                EditadoIndex: null,
            },
            clasificacionProblemas: [],
            estado: {
                isEditingEstado: false,
                nuevoEstadoNombre: '',
                EditadoNombre: '',
                EditadoIndex: null,
            },
            estadosActual: [],
            identificador: {
                isEditingIdentificador: false,
                nuevoIdentificadorNombre: '',
                EditadoNombre: '',
                EditadoIndex: null,
            },
            identificadores: [],
            nivelatencion: {
                isEditingNivelAtencion: false,
                nuevoNivelAtencionNombre: '#000000',
                nuevocolorNivelAtencion: '',
                EditadoNombre: '',
                EditadoColor: '#000000',
                EditadoIndex: null,
            },
            nivelesAtencion: [],
            partida: {
                isEditingPartida: false,
                nuevoPartidaNombre: '',
                nuevoPartidaCodigo: '',
                EditadoNombre: '',
                EditadoCodigo: '',
                EdittipoGasto: '',
                EditadoIndex: null,
                tipoGasto: '1',
            },
            partidas: [],
            categoria: {
                isEditingCategoria: false,
                nuevoCategoriaNombre: '',
                EditadoNombre: '',
                EditttipoPartida: '',
                EditadoIndex: null,
                tipoPartida: '',
            },
            categoriaData: [],
            subcategoria: {
                isEditingSubCategoria: false,
                nuevoSubCategoriaNombre: '',
                EditadoNombre: '',
                EditadoCategoriaId: null,
                EditadoIndex: null,
            },
            subCategoria: [],
            vistaActual: 'lista',
            sortColumn: 'Id',
            sortOrder: 'desc',
            currentPage: 1,
            itemsPerPage: 10, // número de filas por página

        },
        computed: {
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

   
            truncarTexto(texto, longitudMaxima) {
                if (texto.length > longitudMaxima) {
                    return texto.substring(0, longitudMaxima) + '...';
                }
                return texto;
            },
            obtenerTipoGasto(tipo) {
                if (tipo === 1) return "OPEX";
                if (tipo === 2) return "CAPEX";
                return "Desconocido";
            },
            toggleSheet() {
                
                this.sheet = !this.sheet;
                if (!this.sheet) {
                   
                    this.$nextTick(() => {
                        this.resetForm()
                        console.log('llego 3 reseteado')
                        const elementos = document.querySelectorAll(".select-container.has-error")
                        elementos.forEach(el => el.classList.remove("has-error"))
                    })
                }
                this.correosFiltrados = []
                this.resetForm()
                this.resetFileInput()
                this.obtenerEmpleados()
            },
           
            resetFileInput() {
                this.$refs.fileInput.value = ''; // Resetea el valor del input de archivo
            },
            triggerFileInput() {
                this.$refs.fileInput.click();
                this.previewList = [];

            },
            openImagePreview(imageUrl) {
                window.open(imageUrl, "_blank");
            },
            downloadFile(fileUrl) {
                fetch(fileUrl, { mode: "no-cors" })
                    .then(response => response.blob())
                    .then(blob => {
                        const link = document.createElement("a");
                        link.href = URL.createObjectURL(blob);
                        link.setAttribute("download", fileUrl.split("/").pop());
                        document.body.appendChild(link);
                        link.click();
                        document.body.removeChild(link);
                    })
                    .catch(error => console.error("Error al descargar el archivo:", error));

            },
            openEditForm(ticketId, ticketData) {

                this.sheet = !this.sheet;
                this.loadTicketData(ticketData);
                console.log("ticketData")
                console.log(ticketData)

                this.isEditing = true;
                this.currentTicketId = ticketId;
            },
            loadTicketData(ticketData) {

                $("#cboSalas").val(ticketData.Sala.CodSala).trigger("change");
                $("#cboTipoOperacion").val(ticketData.TipoOperacion.Id).trigger("change");
                $("#cboNivelAtencion").val(ticketData.NivelAtencion.Id).trigger("change");
                $("#cboClasificacionProblema").val(ticketData.ClasificacionProblema.Id).trigger("change");
                $("#cboPartida").val(ticketData.SubCategoria.Categoria.Partida.Id).trigger("change");
                $("#cboCategoria").val(ticketData.SubCategoria.Categoria.Id).trigger("change");
                $("#cboSubCategoria").val(ticketData.SubCategoria.Id).trigger("change");
                $("#cboEstado").val(ticketData.EstadoActual.Id).trigger("change");
                $("#cboIdentificador").val(ticketData.Identificador.Id).trigger("change");
                $("#txtDescripcion").val(ticketData.Descripcion).trigger("change");

                if (typeof ticketData.Correos === "string") {
                    this.correos = ticketData.Correos.split(",").map(correo => correo.trim());
                } else if (Array.isArray(ticketData.Correos)) {
                    this.correos = ticketData.Correos;
                } else {
                    this.correos = [];
                }
                console.log("this.correos")
                console.log(this.correos)

                if (ticketData.AdjuntoFullPath) {
                    this.loadAttachedFile(ticketData.AdjuntoFullPath);
                }
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
                this.correosFiltrados = [];
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
            validarCampos() {
                let esValido = true;

                const campos = {
                    cboSalas: "#cboSalas",
                    cboTipoOperacion: "#cboTipoOperacion",
                    cboNivelAtencion: "#cboNivelAtencion",
                    cboPartida: "#cboPartida",
                    cboCategoria: "#cboCategoria",
                    cboSubCategoria: "#cboSubCategoria",
                    cboClasificacionProblema: "#cboClasificacionProblema",
                    cboEstado: "#cboEstado",
                    cboIdentificador: "#cboIdentificador",
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
            validarCamposCierre() {
                let esValido = true;

                const campos = {
                    cboEstadoTickets: "#cboEstadoTickets",
                    //txtDescripcion2: "#txtDescripcion2",
                    //cboEmpleados: "#cboEmpleados",
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
            openModalDetail(id, item) {
                this.ticketSelect = item;
                this.previewList = [];

                if (item.Adjunto) {
                    this.loadAttachedFile(item.AdjuntoFullPath);
                }
                //this.obtenerDetalleHistorial(id)
                this.showModal = true;
                document.documentElement.style.overflow = "hidden";
            },
            openCierreModal(id, item) {
                this.ticketSelect = item;
                //this.cierreTicket.IdTicket = ticket.Id;
                this.showCierreModal = true;
                this.GetSelectCierreTicket();
                this.obtenerEmpleados()
                document.documentElement.style.overflow = "hidden";
            },
            closeCierreModal() {
                this.showCierreModal = false;
                document.documentElement.style.overflow = "auto"

            },
            openConfirmacionCierreModal(id, item) {
                this.ticketSelectId = id;
                this.ticketSelect = item;
                this.showConfirmacionCierreModal = true;
                document.documentElement.style.overflow = "hidden";
            },
            closeConfirmacionCierreModal() {
                this.showConfirmacionCierreModal = false;
                document.documentElement.style.overflow = "auto"

            },
            openDeleteTicket(id, item) {
                this.ticketSelectId = id;
                this.ticketSelect = item;
                this.showDeleteTicket = true;
                document.documentElement.style.overflow = "hidden";
            },
            closeDeleteTicket() {
                this.showDeleteTicket = false;
                document.documentElement.style.overflow = "auto"

            },
            closeModalDetail() {
                this.showModal = false
                document.documentElement.style.overflow = "auto"
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
            resetForm() {
                // Resetear los valores de los selects
                const selects = [
                    "#cboSalas",
                    "#cboTipoOperacion",
                    "#cboNivelAtencion",
                    "#cboPartida",
                    "#cboCategoria",
                    "#cboSubCategoria",
                    "#cboClasificacionProblema",
                    "#cboEstado",
                    "#cboIdentificador",
                ];

                selects.forEach((select) => {
                    $(select).val("").trigger("change"); // Reiniciar y refrescar select2
                });

                // Limpiar otros campos
                this.previewList = [];
                $("input[type='text']").val(""); // Reiniciar inputs de texto
                $("textarea").val(""); // Reiniciar textareas

                this.correos = [];
                this.file = null;
                this.isEditing = false;
                this.currentTicketId = null;
            },
            resetFormCierre() {
                const selects = [
                    "#cboEstadoTickets",
                    "#txtDescripcion2",
                    "#cboEmpleados",
                ];

                selects.forEach((select) => {
                    $(select).val("").trigger("change");
                });

                this.previewList = [];
                $("input[type='text']").val("");
                $("textarea").val("");
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
                    //IdUsuarioConfirma: $("#cboEmpleados").val(),
                    FechaTentativaTermino: $("#fechaFin").val(),
                    IdTicket: vm.ticketSelect.Id
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
                            vm.closeModalDetail()
                            vm.GetTicketsPorIdUsuario();
                            vm.obtenerSalas()
                            vm.obternetDataSelects()
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
                        var datos = result.data

                        vm.dataAsignar = result.data
                        vm.correosBD = datos.correos;

                        const dataEmpleados = $('#cboEmpleados');
                        dataEmpleados.empty();
                        dataEmpleados.append('<option value="">Seleccione un empleado</option>');
                        result.data.usuariosAsignables.forEach((item) => {
                            dataEmpleados.append(`<option value="${item.Valor}">${item.Texto}</option>`);
                        });
                        dataEmpleados.select2({
                            multiple: false, placeholder: "--Seleccione--", allowClear: true, dropdownParent: $('#formCierreTicket')
                        });

                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
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
            eliminarTicket(deleteId) {
                var vm = this;
                const datadelete = {
                    id: deleteId
                }
                console.log(deleteId)
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/EliminarTicket",

                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(datadelete),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success != false) {
                            toastr.success(result.displayMessage, "Éxito")
                            vm.GetTicketsPorIdUsuario()
                        } else {
                            toastr.error(result.displayMessage, "Mensaje Servidor")
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
            ConfirmarCierreTicket(confirmarCierreId) {
                var vm = this;
                const dataConfirmarCierre = {
                    idTicket: confirmarCierreId
                }
                console.log(dataConfirmarCierre)
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/ConfirmarCierreTicket",

                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(dataConfirmarCierre),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success != false) {
                            toastr.success(result.displayMessage, "Éxito")
                            vm.GetTicketsPorIdUsuario()
                        } else {
                            toastr.error(result.displayMessage, "Mensaje Servidor")
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
            confirmDelete() {
                this.eliminarTicket(this.ticketIdToDelete)
                this.showDeleteTicket = false;
            },
            confirmCierre() {
                this.ConfirmarCierreTicket(this.ticketIdConfirmarCierre)
                this.showConfirmacionCierreModal = false;
            },
            saveTicket(isEdit) {
                if (isEdit) {
                    this.editarTicket();
                } else {
                    this.registrarTicket()
                }
            },
            registrarTicket() {
                var vm = this
                if (!vm.validarCampos()) {
                    toastr.error("Por favor, complete todos los campos.", "Error de Validación");
                    return;
                }

                let esValido = true;

                if (this.correos.length === 0) {
                    toastr.error("Debe agregar al menos un correo válido");
                    esValido = false;
                }

                if (!esValido) return;

                const formData = new FormData();
                formData.append("CodSala", $("#cboSalas").val())
                formData.append("IdTipoOperacion", $("#cboTipoOperacion").val())
                formData.append("IdNivelAtencion", $("#cboNivelAtencion").val())
                formData.append("IdSubCategoria", $("#cboSubCategoria").val())
                formData.append("IdClasificacionProblema", $("#cboClasificacionProblema").val())
                formData.append("IdEstadoActual", $("#cboEstado").val())
                formData.append("IdIdentificador", $("#cboIdentificador").val())
                formData.append("Descripcion", $("#txtDescripcion").val())
                formData.append("file", vm.file ? vm.file[0] : null)
                formData.append("Destinatarios", vm.correos)
                formData.append("Id", vm.currentTicketId)

                console.log("vm.correos")
                console.log(vm.correos)
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/CrearTicket",
                    cache: false,
                    processData: false,
                    contentType: false,
                    data: formData,
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success(result.displayMessage, "Éxito");
                            vm.resetForm();
                            vm.toggleSheet()
                            vm.GetTicketsPorIdUsuario()
                            vm.correos = []
                            //vm.previewList = [];
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
            editarTicket() {
                var vm = this
                const isEdit = vm.currentTicketId != 0
                if (!vm.validarCampos()) {
                    toastr.error("Por favor, complete todos los campos.", "Error de Validación");
                    return;
                }

                let esValido = true;

                if (!this.file && !isEdit) {
                    toastr.error("Debe adjuntar un archivo");
                    esValido = false;
                }

                if (this.correos.length === 0) {
                    toastr.error("Debe agregar al menos un correo válido");
                    esValido = false;
                }

                if (!esValido) return;



                const formData = new FormData();
                formData.append("CodSala", $("#cboSalas").val())
                formData.append("IdTipoOperacion", $("#cboTipoOperacion").val())
                formData.append("IdNivelAtencion", $("#cboNivelAtencion").val())
                formData.append("IdSubCategoria", $("#cboSubCategoria").val())
                formData.append("IdClasificacionProblema", $("#cboClasificacionProblema").val())
                formData.append("IdEstadoActual", $("#cboEstado").val())
                formData.append("IdIdentificador", $("#cboIdentificador").val())
                formData.append("Descripcion", $("#txtDescripcion").val())
                formData.append("file", vm.file ? vm.file[0] : null)
                formData.append("Destinatarios", vm.correos)
                formData.append("Id", vm.currentTicketId)

                console.log("vm.correos")
                console.log(vm.correos)
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/EditarTicket",
                    cache: false,
                    processData: false,
                    contentType: false,
                    data: formData,
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success(result.displayMessage, "Éxito");
                            vm.resetForm();
                            vm.toggleSheet()
                            vm.GetTicketsPorIdUsuario()
                            vm.correos = []
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
                        //console.log("result", result)
                        //console.log("result", result.data)

                        if (result.data != null) {
                            var datos = result.data
                            vm.ticketsCreados = datos.ticketsCreados
                            vm.ticketsAsignados = datos.ticketsAsignados
                            vm.ticketsRecibidos = datos.ticketsRecibidos
                            console.log("ticketsCreados")
                            console.log(vm.ticketsCreados)
                            console.log(vm.ticketsCreados.length)
                            console.log("ticketsAsignados")
                            console.log(vm.ticketsAsignados)
                            console.log("ticketsRecibidos")
                            console.log(vm.ticketsRecibidos)

                            vm.historial = datos.ticketsAsignados.Seguimientos

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

                        var datos = result.data
                        var datos2 = result.data.Asignacion
                        var datos3 = result.data.Seguimientos
                        vm.ticketsQueAsigne = datos
                        vm.ticketsDetail = datos
                        vm.ticketsAsignadosDetail = datos2
                        vm.ticketsSeguimientosDetail = datos3
                        console.log("vm.ticketsAsignadosDetail")
                        console.log(vm.ticketsAsignadosDetail)
                        console.log(vm.ticketsAsignadosDetail.UsuarioAsignado.Nombres)
                        console.log(vm.ticketsAsignadosDetail.UsuarioAsignado.ApellidoPaterno)
                        console.log(vm.ticketsAsignadosDetail.UsuarioAsignado.ApellidoMaterno)


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
                $.ajax({
                    type: "POST",
                    url: basePath + "Sala/ListadoSalaPorUsuarioJson",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        var datos = result.data
                        $("#cboSalas").append('<option value="">--Seleccione--</option>')
                        $.each(datos, function (index, value) {
                            $("#cboSalas").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>')
                        })
                        $("#cboSalas").select2({
                            multiple: false, placeholder: "--Seleccione--", allowClear: true, dropdownParent: $('#formCrearTicket')
                        })

                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            onchageSelect() {
                alert('holaa')
                console.log('raa')
            },
            obternetDataSelects() {
                var vm = this

                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiSelects/GetSelectCrearTicket",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        var datos = result.data
                        vm.datosCargados = datos
                        console.log("vm.datosCargados")
                        console.log(vm.datosCargados)
                        var nivelesAtencion = datos.nivelesAtencion;
                        var estadosActual = datos.estadosActual;
                        var identificadores = datos.identificadores;
                        var partidas = datos.partidas;
                        var categorias = datos.categorias;
                        var subCategorias = datos.subCategorias;
                        var clasificacionProblemas = datos.clasificacionProblemas
                        vm.dataSelects = datos

                        $("#cboTipoOperacion").empty().append('<option value="">--Seleccione--</option>');
                        $("#cboNivelAtencion").empty().append('<option value="">--Seleccione--</option>');
                        $("#cboPartida").empty().append('<option value="">--Seleccione--</option>');
                        $("#cboCategoria").empty().append('<option value="">--Seleccione--</option>');
                        $("#cboSubCategoria").empty().append('<option value="">--Seleccione--</option>');
                        $("#cboClasificacionProblema").empty().append('<option value="">--Seleccione--</option>');
                        $("#cboEstado").empty().append('<option value="">--Seleccione--</option>');
                        $("#cboIdentificador").empty().append('<option value="">--Seleccione--</option>');



                        /*TIPO DE OPERACION SELECT */
                        $("#cboTipoOperacion").append('<option value="">--Seleccione--</option>')
                        $.each(datos.tiposOperacion, function (index, value) {
                            $("#cboTipoOperacion").append('<option value="' + value.Valor + '"  >' + value.Texto + '</option>')
                        })
                        $("#cboTipoOperacion").select2({
                            multiple: false, placeholder: "--Seleccione--", allowClear: true, dropdownParent: $('#formCrearTicket')
                        })

                        /*NIVEL DE ATENCION SELECT */
                        $("#cboNivelAtencion").append('<option value="">--Seleccione--</option>')
                        $.each(nivelesAtencion, function (index, value) {
                            $("#cboNivelAtencion").append('<option value="' + value.Valor + '"  >' + value.Texto + '</option>')
                        })
                        $("#cboNivelAtencion").select2({
                            multiple: false, placeholder: "--Seleccione--", allowClear: true, dropdownParent: $('#formCrearTicket')
                        })
                        /* PARTIDA */
                        $("#cboPartida").append('<option value="">--Seleccione--</option>')
                        $.each(partidas, function (index, value) {
                            $("#cboPartida").append('<option value="' + value.Valor + '"  >' + value.Texto + '</option>')
                        })
                        $("#cboPartida").select2({
                            multiple: false, placeholder: "--Seleccione--", allowClear: true, dropdownParent: $('#formCrearTicket')
                        })
                        /* CATEGORIAS */
                        $("#cboCategoria").append('<option value="">--Seleccione--</option>')

                        $("#cboCategoria").select2({
                            multiple: false, placeholder: "--Seleccione--", allowClear: true, dropdownParent: $('#formCrearTicket')
                        })
                        /* SUB CATEGORIAS */
                        $("#cboSubCategoria").append('<option value="">--Seleccione--</option>')

                        $("#cboSubCategoria").select2({
                            multiple: false, placeholder: "--Seleccione--", allowClear: true, dropdownParent: $('#formCrearTicket')
                        })

                        /* clasificacion problema */
                        $("#cboClasificacionProblema").append('<option value="">--Seleccione--</option>')
                        $.each(clasificacionProblemas, function (index, value) {
                            $("#cboClasificacionProblema").append('<option value="' + value.Valor + '"  >' + value.Texto + '</option>')
                        })
                        $("#cboClasificacionProblema").select2({
                            multiple: false, placeholder: "--Seleccione--", allowClear: true, dropdownParent: $('#formCrearTicket')
                        })

                        /* ESTADO ACTUAL */
                        $("#cboEstado").append('<option value="">--Seleccione--</option>')
                        $.each(estadosActual, function (index, value) {
                            $("#cboEstado").append('<option value="' + value.Valor + '"  >' + value.Texto + '</option>')
                        })
                        $("#cboEstado").select2({
                            multiple: false, placeholder: "--Seleccione--", allowClear: true, dropdownParent: $('#formCrearTicket')
                        })


                        /* ESTADO IDENTIFICADOR */
                        $("#cboIdentificador").append('<option value="">--Seleccione--</option>')
                        $.each(identificadores, function (index, value) {
                            $("#cboIdentificador").append('<option value="' + value.Valor + '"  >' + value.Texto + '</option>')
                        })
                        $("#cboIdentificador").select2({
                            multiple: false, placeholder: "--Seleccione--", allowClear: true, dropdownParent: $('#formCrearTicket')
                        })

                    },
                    error: function (request, status, error) {
                        toastr.error("Error", "Mensaje Servidor")
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            openModalDetailSeguimiento(id) {
                this.GetTicketById(id)
                //this.obtenerTicketsQueAsigne(id)
                console.log("id", id)
                this.showModalDetailSeguimiento = true;

                document.documentElement.style.overflow = "hidden";
            },

            //MODAL CRUD TIPO OPERACION----------------------------------------------------
            openModalOperation(formContext) {
                var vm = this
                vm.valorSeleccionado = $(`#cboTipoOperacion`).val();

                vm.formContext = formContext;
                let endpoint = '';
                switch (formContext) {
                    case 'formCrearTicket':
                        endpoint = basePath + "GlpiSelects/GetSelectCrearTicket";
                        break;
                    default:
                        toastr.error("Formulario no reconocido", "Error");
                        return;
                }
                vm.endpoint = endpoint;
                this.GetTiposOperacion();
                this.modalOperacion = true;
            },
            closeModalOperation() {
                this.modalOperacion = false;
                this.operacion.isEditingOperacion = false;
                this.operacion.nuevoOperacionNombre = '';
                this.operacion.EditadoNombre = '';
            },
            GetTiposOperacion() {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTiposOperacion/GetTiposOperacion",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            vm.tipoOperacion = result.data;
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
            guardarOperacion() {
                var vm = this
                const data = {
                    Nombre: this.operacion.nuevoOperacionNombre,
                    Estado: true,
                }
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTiposOperacion/SaveTipoOperacion",
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
                            vm.GetTiposOperacion()
                            vm.operacion.nuevoOperacionNombre = '';
                            vm.actualizarSelectOperacion(
                                "#cboTipoOperacion",
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
            editarOperacion(index) {
                this.operacion.isEditingOperacion = true;
                this.operacion.EditadoNombre = this.tipoOperacion[index].Nombre;
                this.operacion.EditadoIndex = index;
            },
            actualizarOperacion() {
                var vm = this;
                const data = {
                    Id: this.tipoOperacion[this.operacion.EditadoIndex].Id,
                    Nombre: this.operacion.EditadoNombre,
                };
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTiposOperacion/SaveTipoOperacion",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success("Operacion actualizado con éxito.", "Éxito");
                            vm.GetTiposOperacion();

                            vm.actualizarSelectOperacion(
                                "#cboTipoOperacion",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                            vm.operacion.isEditingOperacion = false;
                            vm.operacion.EditadoIndex = null;
                        } else {
                            toastr.error("Error al actualizar operacion", "Error");
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
            actualizarSelectOperacion(selector, endpoint, filterFn = null, formContext = '') {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: endpoint,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        if (result.success) {
                            let datos = result.data.tiposOperacion;
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
            //MODAL CRUD CLASIFICACION PROBLEMA----------------------------------------------------
            openModalClasProblema(formContext) {
                var vm = this
                vm.valorSeleccionado = $(`#cboClasificacionProblema`).val();

                vm.formContext = formContext;
                let endpoint = '';
                switch (formContext) {
                    case 'formCrearTicket':
                        endpoint = basePath + "GlpiSelects/GetSelectCrearTicket";
                        break;
                    default:
                        toastr.error("Formulario no reconocido", "Error");
                        return;
                }
                vm.endpoint = endpoint;
                this.GetClasificacionProblemas();
                this.modalClasificacionProblema = true;
            },
            closeModalClasProblema() {
                this.modalClasificacionProblema = false;
                this.clasificacion.isEditingClasificacion = false;
                this.clasificacion.nuevoClasificacionNombre = '';
                this.clasificacion.EditadoNombre = '';
            },
            GetClasificacionProblemas() {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiClasificacionProblemas/GetClasificacionProblemas",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            vm.clasificacionProblemas = result.data;
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
            guardarClasificacion() {
                var vm = this
                const data = {
                    Nombre: this.clasificacion.nuevoClasificacionNombre,
                    Estado: true,
                }
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiClasificacionProblemas/SaveClasificacionProblema",
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
                            vm.GetClasificacionProblemas()
                            vm.clasificacion.nuevoClasificacionNombre = '';
                            vm.actualizarSelectClasificacion(
                                "#cboClasificacionProblema",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                        }
                        else {
                            toastr.error("Error al registrar tipo de clasificacion", "Error");

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
            editarClasificacion(index) {
                this.clasificacion.isEditingClasificacion = true;
                this.clasificacion.EditadoNombre = this.clasificacionProblemas[index].Nombre;
                this.clasificacion.EditadoIndex = index;
            },
            actualizarClasificacion() {
                var vm = this;
                const data = {
                    Id: this.clasificacionProblemas[this.clasificacion.EditadoIndex].Id,
                    Nombre: this.clasificacion.EditadoNombre,
                };
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiClasificacionProblemas/SaveClasificacionProblema",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success("Clasificacion actualizado con éxito.", "Éxito");
                            vm.GetClasificacionProblemas();

                            vm.actualizarSelectClasificacion(
                                "#cboClasificacionProblema",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                            vm.clasificacion.isEditingClasificacion = false;
                            vm.clasificacion.EditadoIndex = null;
                        } else {
                            toastr.error("Error al actualizar clasificacion", "Error");
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
            actualizarSelectClasificacion(selector, endpoint, filterFn = null, formContext = '') {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: endpoint,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        if (result.success) {
                            let datos = result.data.clasificacionProblemas;
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

            //MODAL CRUD CLASIFICACION PROBLEMA----------------------------------------------------
            openModalEstadoActual(formContext) {
                var vm = this
                vm.valorSeleccionado = $(`#cboEstado`).val();

                vm.formContext = formContext;
                let endpoint = '';
                switch (formContext) {
                    case 'formCrearTicket':
                        endpoint = basePath + "GlpiSelects/GetSelectCrearTicket";
                        break;
                    default:
                        toastr.error("Formulario no reconocido", "Error");
                        return;
                }
                vm.endpoint = endpoint;
                this.GetEstadosActuales();
                this.modalEstadoActual = true;
            },
            closeModalEstadoActual() {
                this.modalEstadoActual = false;
                this.estado.isEditingEstado = false;
                this.estado.nuevoEstadoNombre = '';
                this.estado.EditadoNombre = '';
            },
            GetEstadosActuales() {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiEstadosActual/GetEstadosActuales",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            vm.estadosActual = result.data;
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
            guardarEstado() {
                var vm = this
                const data = {
                    Nombre: this.estado.nuevoEstadoNombre,
                    Estado: true,
                }
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiEstadosActual/SaveEstadoActual",
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
                            vm.GetEstadosActuales()
                            vm.estado.nuevoEstadoNombre = '';
                            vm.actualizarSelectEstado(
                                "#cboEstado",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                        }
                        else {
                            toastr.error("Error al registrar tipo de estado", "Error");

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
            editarEstado(index) {
                this.estado.isEditingEstado = true;
                this.estado.EditadoNombre = this.estadosActual[index].Nombre;
                this.estado.EditadoIndex = index;
            },
            actualizarEstado() {
                var vm = this;
                const data = {
                    Id: this.estadosActual[this.estado.EditadoIndex].Id,
                    Nombre: this.estado.EditadoNombre,
                };
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiEstadosActual/SaveEstadoActual",
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
                            vm.GetEstadosActuales();

                            vm.actualizarSelectEstado(
                                "#cboEstado",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                            vm.estado.isEditingEstado = false;
                            vm.estado.EditadoIndex = null;
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
            actualizarSelectEstado(selector, endpoint, filterFn = null, formContext = '') {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: endpoint,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        if (result.success) {
                            let datos = result.data.estadosActual;
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
            //MODAL CRUD IDENTIFICADOR----------------------------------------------------
            openModalIdentificador(formContext) {
                var vm = this
                vm.valorSeleccionado = $(`#cboIdentificador`).val();

                vm.formContext = formContext;
                let endpoint = '';
                switch (formContext) {
                    case 'formCrearTicket':
                        endpoint = basePath + "GlpiSelects/GetSelectCrearTicket";
                        break;
                    default:
                        toastr.error("Formulario no reconocido", "Error");
                        return;
                }
                vm.endpoint = endpoint;
                this.GetIdentificadores();
                this.modalIdentificador = true;
            },
            closeModalIdentificador() {
                this.modalIdentificador = false;
                this.identificador.isEditingIdentificador = false;
                this.identificador.nuevoIdentificadorNombre = '';
                this.identificador.EditadoNombre = '';
            },
            GetIdentificadores() {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiIdentificadores/GetIdentificadores",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            vm.identificadores = result.data;
                        } else {
                            toastr.error(result.displayMessage, "Error");
                        }
                    },
                    error: function (request, status, error) {
                        toastr.error("Error al cargar los identificadors de ticket", "Error");
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            guardarIdentificador() {
                var vm = this
                const data = {
                    Nombre: this.identificador.nuevoIdentificadorNombre,
                    Estado: true,
                }
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiIdentificadores/SaveIdentificador",
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
                            vm.GetIdentificadores()
                            vm.identificador.nuevoIdentificadorNombre = '';
                            vm.actualizarSelectIdentificador(
                                "#cboIdentificador",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                        }
                        else {
                            toastr.error("Error al registrar tipo de identificador", "Error");

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
            editarIdentificador(index) {
                this.identificador.isEditingIdentificador = true;
                this.identificador.EditadoNombre = this.identificadores[index].Nombre;
                this.identificador.EditadoIndex = index;
            },
            actualizarIdentificador() {
                var vm = this;
                const data = {
                    Id: this.identificadores[this.identificador.EditadoIndex].Id,
                    Nombre: this.identificador.EditadoNombre,
                };
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiIdentificadores/SaveIdentificador",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success("Identificador actualizado con éxito.", "Éxito");
                            vm.GetIdentificadores();

                            vm.actualizarSelectIdentificador(
                                "#cboIdentificador",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                            vm.identificador.isEditingIdentificador = false;
                            vm.identificador.EditadoIndex = null;
                        } else {
                            toastr.error("Error al actualizar identificador", "Error");
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
            actualizarSelectIdentificador(selector, endpoint, filterFn = null, formContext = '') {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: endpoint,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        if (result.success) {
                            let datos = result.data.identificadores;
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
            //MODAL CRUD NIVEL ATENCION----------------------------------------------------
            openModalAtentionLevel(formContext) {
                var vm = this
                vm.valorSeleccionado = $(`#cboNivelAtencion`).val();

                vm.formContext = formContext;
                let endpoint = '';
                switch (formContext) {
                    case 'formCrearTicket':
                        endpoint = basePath + "GlpiSelects/GetSelectCrearTicket";
                        break;
                    default:
                        toastr.error("Formulario no reconocido", "Error");
                        return;
                }
                vm.endpoint = endpoint;
                this.GetNivelesAtencion();
                this.modalAtencion = true;
            },
            closeModalAtentionLevel() {
                this.modalAtencion = false;
                this.nivelatencion.isEditingNivelAtencion = false;
                this.nivelatencion.nuevoNivelAtencionNombre = '';
                this.nivelatencion.nuevocolorNivelAtencion = '';
                this.nivelatencion.EditadoNombre = '';
                this.nivelatencion.EditadoColor = '';
            },
            GetNivelesAtencion() {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiNivelesAtencion/GetNivelesAtencion",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            vm.nivelesAtencion = result.data;
                        } else {
                            toastr.error(result.displayMessage, "Error");
                        }
                    },
                    error: function (request, status, error) {
                        toastr.error("Error al cargar los nivelatencions de ticket", "Error");
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
            },
            guardarNivelAtencion() {
                var vm = this
                const data = {
                    Nombre: this.nivelatencion.nuevoNivelAtencionNombre,
                    Color: this.nivelatencion.nuevocolorNivelAtencion,
                    Estado: true,
                }
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiNivelesAtencion/SaveNivelAtencion",
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
                            vm.GetNivelesAtencion()
                            vm.nivelatencion.nuevoNivelAtencionNombre = '';
                            vm.nivelatencion.nuevocolorNivelAtencion = '';
                            vm.actualizarSelectNivelAtencion(
                                "#cboNivelAtencion",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                        }
                        else {
                            toastr.error("Error al registrar tipo de nivelatencion", "Error");

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
            editarNivelAtencion(index) {
                this.nivelatencion.isEditingNivelAtencion = true;
                this.nivelatencion.EditadoNombre = this.nivelesAtencion[index].Nombre;
                this.nivelatencion.EditadoColor = this.nivelesAtencion[index].Color;
                this.nivelatencion.EditadoIndex = index;
            },
            actualizarNivelAtencion() {
                var vm = this;
                const data = {
                    Id: this.nivelesAtencion[this.nivelatencion.EditadoIndex].Id,
                    Nombre: this.nivelatencion.EditadoNombre,
                    Color: this.nivelatencion.EditadoColor,
                };
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiNivelesAtencion/SaveNivelAtencion",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success("NivelAtencion actualizado con éxito.", "Éxito");
                            vm.GetNivelesAtencion();
                            vm.actualizarSelectNivelAtencion(
                                "#cboNivelAtencion",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                            vm.nivelatencion.isEditingNivelAtencion = false;
                            vm.nivelatencion.EditadoIndex = null;
                        } else {
                            toastr.error("Error al actualizar nivelatencion", "Error");
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
            actualizarSelectNivelAtencion(selector, endpoint, filterFn = null, formContext = '') {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: endpoint,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        if (result.success) {
                            let datos = result.data.nivelesAtencion;
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
            //MODAL CRUD PARTIDA----------------------------------------------------
            openModalPartida(formContext) {
                var vm = this
                vm.valorSeleccionado = $(`#cboPartida`).val();

                vm.formContext = formContext;
                let endpoint = '';
                switch (formContext) {
                    case 'formCrearTicket':
                        endpoint = basePath + "GlpiSelects/GetSelectCrearTicket";
                        break;
                    default:
                        toastr.error("Formulario no reconocido", "Error");
                        return;
                }
                vm.endpoint = endpoint;
                this.GetPartidas().then(() => {
                    // Inicializar DataTables después de obtener los datos
                    vm.inicializarDataTablePartidas();
                    // Abrir el modal
                    $('#modalPartida').modal('show');
                }); this.modalPartida = true;
            },
            closeModalPartida() {
                this.modalPartida = false;
                this.partida.isEditingPartida = false;
                this.partida.nuevoPartidaNombre = '';
                this.partida.nuevoPartidaCodigo = '';
                this.partida.EditadoNombre = '';
                this.partida.EditadoCodigo = '';
                this.partida.EdittipoGasto = '';
            },
            //GetPartidas() {
            //    var vm = this;
            //    $.ajax({
            //        type: "POST",
            //        url: basePath + "GlpiPartidas/GetPartidas",
            //        cache: false,
            //        contentType: "application/json; charset=utf-8",
            //        dataType: "json",
            //        beforeSend: function (xhr) {
            //            $.LoadingOverlay("show")
            //        },
            //        success: function (result) {
            //            if (result.success) {
            //                vm.partidas = result.data;
            //            } else {
            //                toastr.error(result.displayMessage, "Error");
            //            }
            //        },
            //        error: function (request, status, error) {
            //            toastr.error("Error al cargar los partidas de ticket", "Error");
            //        },
            //        complete: function (resul) {
            //            $.LoadingOverlay("hide")
            //        }
            //    });
            //},
            GetPartidas() {
                var vm = this;
                return new Promise((resolve, reject) => {
                    $.ajax({
                        type: "POST",
                        url: basePath + "GlpiPartidas/GetPartidas",
                        cache: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: function (xhr) {
                            $.LoadingOverlay("show");
                        },
                        success: function (result) {
                            if (result.success) {
                                vm.partidas = result.data; // Almacenar los datos en Vue
                                resolve(); // Resolver la promesa
                            } else {
                                toastr.error(result.displayMessage, "Error");
                                reject();
                            }
                        },
                        error: function (request, status, error) {
                            toastr.error("Error al cargar los partidas de ticket", "Error");
                            reject();
                        },
                        complete: function (resul) {
                            $.LoadingOverlay("hide");
                        }
                    });
                });
            },
            renderDataTable(idTable, data, columns) {
                // Destruir la instancia anterior de DataTable si existe
                if ($.fn.DataTable.isDataTable(`#${idTable}`)) {
                    $(`#${idTable}`).DataTable().destroy();
                }

                // Inicializar DataTable
                $(`#${idTable}`).DataTable({
                    data: data,
                    columns: columns,
                    paging: true,
                    searching: true,
                    ordering: true,
                    info: true,
                    autoWidth: true,
                    responsive: true,

                    destroy: true,
                    sort: true,
                    scrollCollapse: true,
                    scrollX: true,
                    aaSorting: [[0, 'desc']],
                });
            },
            inicializarDataTablePartidas() {
                const vm = this;
                const idTable = 'tablePartidas';
                const columns = [
                    { data: "Id", title: "ID" },
                    { data: "Codigo", title: "Código" },
                    { data: "Nombre", title: "Nombre" },
                    {
                        data: "Id",
                        title: "Acciones",
                        render: function (data, type, row, meta) {
                            return `
                    <button 
                        style="width:40px; height:40px;" 
                        type="button" 
                        class="btn btn-xs btn-warning btnEditarPartida" 
                        data-id="${data}">
                        <i class="glyphicon glyphicon-pencil"></i>
                    </button>
                `;
                        },
                        className: "text-center",
                        orderable: false
                    }

                ];
                this.renderDataTable(idTable, this.partidas, columns);
                $(`#${idTable}`).on('click', '.btnEditarPartida', function () {
                    const id = $(this).data('id');
                    vm.editarPartida(id);
                });
            },
            obtenerTipoGasto(tipoGasto) {
                switch (tipoGasto) {
                    case 1: return "Gasto Operativo";
                    case 2: return "Gasto de Inversión";
                    default: return "No definido";
                }
            },
            guardarPartida() {
                var vm = this
                const data = {
                    Codigo: this.partida.nuevoPartidaCodigo,
                    Nombre: this.partida.nuevoPartidaNombre,
                    TipoGasto: this.partida.tipoGasto,
                    Estado: true,
                }
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiPartidas/SavePartida",
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
                            vm.GetPartidas().then(() => {
                                vm.inicializarDataTablePartidas()
                            })
                            vm.partida.nuevoPartidaNombre = '';
                            vm.partida.nuevoPartidaCodigo = '';
                            vm.partida.tipoGasto = '1';
                            vm.actualizarSelectPartida(
                                "#cboPartida",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                        }
                        else {
                            toastr.error("Error al registrar tipo de partida", "Error");

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
            editarPartida(id) {
                const index = this.partidas.findIndex(p => p.Id === id);
                if (index !== -1) {

                this.partida.isEditingPartida = true;
                this.partida.EditadoNombre = this.partidas[index].Nombre;
                this.partida.EdittipoGasto = this.partidas[index].TipoGasto;
                this.partida.EditadoCodigo = this.partidas[index].Codigo;
                    this.partida.EditadoIndex = index;
                }
                console.log("partida.isEditingPartida")
                console.log(this.partida.isEditingPartida)
            },
            actualizarPartida() {
                var vm = this;
                const data = {
                    Id: this.partidas[this.partida.EditadoIndex].Id,
                    Nombre: this.partida.EditadoNombre,
                    TipoGasto: this.partida.EdittipoGasto,
                    Codigo: this.partida.EditadoCodigo,
                    Estado: true,
                };
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiPartidas/SavePartida",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success("Partida actualizado con éxito.", "Éxito");
                            vm.GetPartidas().then(() => {
                                vm.inicializarDataTablePartidas()
                            })
                            vm.actualizarSelectPartida(
                                "#cboPartida",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                            vm.partida.isEditingPartida = false;
                            vm.partida.EditadoIndex = null;
                        } else {
                            toastr.error("Error al actualizar partida", "Error");
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
            actualizarSelectPartida(selector, endpoint, filterFn = null, formContext = '') {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: endpoint,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        if (result.success) {
                            let datos = result.data.partidas;
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
            //MODAL CRUD CATEGORIA----------------------------------------------------
            openModalRubro(formContext) {
                this.obtenerPartidas()

                var vm = this
                vm.valorSeleccionado = $(`#cboCategoria`).val();

                vm.formContext = formContext;
                let endpoint = '';
                switch (formContext) {
                    case 'formCrearTicket':
                        endpoint = basePath + "GlpiSelects/GetSelectCrearTicket";
                        break;
                    default:
                        toastr.error("Formulario no reconocido", "Error");
                        return;
                }
                vm.endpoint = endpoint;

                this.GetCategorias().then(() => {
                    vm.inicializarDataTableCategorias();
                    $('#modalRubro').modal('show');
                });
                this.modalRubro = true;
            },
            closeModalRubro() {
                this.modalRubro = false;
                this.categoria.isEditingCategoria = false;
                this.categoria.nuevoCategoriaNombre = '';
                this.categoria.EditadoNombre = '';
                this.categoria.EdittipoPartida = '';
            },
            obtenerPartidas() {
                var vm = this

                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiPartidas/GetPartidas",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success("Se obtuvieron todas las partidas", "Éxito");

                            const partidaSelectModal = $('#cboPartidaModal');
                            partidaSelectModal.empty();
                            partidaSelectModal.append('<option value="">Seleccione una partida</option>');
                            result.data.forEach((item) => {
                                partidaSelectModal.append(`<option value="${item.Id}">${item.Nombre}</option>`);
                            });
                            partidaSelectModal.select2({
                                placeholder: "Seleccione una partida",
                                allowClear: true,
                                //dropdownParent: $('#formCrearRubroCategoria')
                            });
                            if (vm.categoria.isEditingCategoria) {
                                const partidaSelectModal = $('#cboPartidaModalEditar');
                                result.data.forEach((item) => {
                                    partidaSelectModal.append(`<option value="${item.Id}">${item.Nombre}</option>`);
                                });
                                partidaSelectModal.val(vm.categoria.EdittipoPartida).trigger('change');
                            }
                        }
                        else {
                            toastr.error("Error al obtener partidas", "Error");

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
            GetCategorias() {
                var vm = this;
                return new Promise((resolve, reject) => {

                    $.ajax({
                        type: "POST",
                        url: basePath + "GlpiCategorias/GetCategorias",
                        cache: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: function (xhr) {
                            $.LoadingOverlay("show")
                        },
                        success: function (result) {
                            if (result.success) {
                                vm.categoriaData = result.data;
                                resolve();
                            } else {
                                toastr.error(result.displayMessage, "Error");
                                reject();

                            }
                        },
                        error: function (request, status, error) {
                            toastr.error("Error al cargar los categorias de ticket", "Error");
                        },
                        complete: function (resul) {
                            $.LoadingOverlay("hide")
                        }
                    });
                });
            },
            inicializarDataTableCategorias() {
                const vm = this;
                const idTable = 'tableCategorias';
                const columns = [
                    { data: "Id", title: "ID" },
                    { data: "Nombre", title: "Categoria" },
                    { data: "NombrePartida", title: "Partida" },
                    {
                        data: "Id",
                        title: "Acciones",
                        render: function (data, type, row, meta) {
                            return `
                    <button 
                        style="width:40px; height:40px;" 
                        type="button" 
                        class="btn btn-xs btn-warning btnEditarCategorias" 
                        data-id="${data}">
                        <i class="glyphicon glyphicon-pencil"></i>
                    </button>
                `;
                        },
                        className: "text-center",
                        orderable: false
                    }
                ];
                this.renderDataTable(idTable, this.categoriaData, columns);
                $(`#${idTable}`).on('click', '.btnEditarCategorias', function () {
                    const id = $(this).data('id');
                    vm.editarCategoria(id);
                });
            },
            obtenerTipoGasto(tipoGasto) {
                switch (tipoGasto) {
                    case 1: return "Gasto Operativo";
                    case 2: return "Gasto de Inversión";
                    default: return "No definido";
                }
            },
            guardarCategoria() {
                var vm = this
                const data = {
                    IdPartida: $("#cboPartidaModal").val(),
                    Nombre: $("#categoriaNombre").val(),
                    Estado: true,
                }
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiCategorias/SaveCategoria",
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
                            vm.GetCategorias().then(() => {
                                vm.inicializarDataTableCategorias()
                            })
                            vm.categoria.categoriaNombre = '';

                            vm.categoria.nuevoCategoriaNombre = '';
                            vm.categoria.tipoPartida = '';
                            vm.actualizarSelectCategoria(
                                "#cboCategoria",
                                vm.endpoint,
                                null,
                                vm.formContext
                            );
                        }
                        else {
                            toastr.error("Error al registrar tipo de categoria", "Error");

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
            editarCategoria(id) {
                const index = this.categoriaData.findIndex(p => p.Id === id);
                if (index !== -1) {
                this.categoria.isEditingCategoria = true;
                this.categoria.EditadoNombre = this.categoriaData[index].Nombre;
                this.categoria.EdittipoPartida = this.categoriaData[index].IdPartida;
                this.categoria.EditadoIndex = index;
                    this.obtenerPartidas();
                }
            },
            actualizarCategoria() {
                var vm = this;
                const data = {
                    Id: this.categoriaData[this.categoria.EditadoIndex].Id,
                    Nombre: this.categoria.EditadoNombre,
                    IdPartida: this.categoria.EdittipoPartida,
                    Estado: true,
                };
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiCategorias/SaveCategoria",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success("Categoria actualizado con éxito.", "Éxito");
                            vm.GetCategorias().then(() => {
                                vm.inicializarDataTableCategorias()
                            })
                            const partidaSeleccionada = vm.categoria.EdittipoPartida;
                            vm.actualizarSelectCategoria(
                                "#cboCategoria",
                                vm.endpoint,
                                //null,
                                (categoria) => {
                                    //console.log("Categoría:", categoria); 
                                    return categoria.IdPadre == partidaSeleccionada;
                                },
                                vm.formContext,
                            );
                            vm.categoria.isEditingCategoria = false;
                            vm.categoria.EditadoIndex = null;
                        } else {
                            toastr.error("Error al actualizar categoria", "Error");
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
            actualizarSelectCategoria(selector, endpoint, filterFn = null, formContext = '') {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: endpoint,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        if (result.success) {
                            let datos = result.data.categorias;
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
            //MODAL CRUD SUB CATEGORIA----------------------------------------------------
            openModalSubCategoria(formContext) {
                this.obtenerCategorias();

                var vm = this
                vm.valorSeleccionado = $(`#cboSubCategoria`).val();

                vm.formContext = formContext;
                let endpoint = '';
                switch (formContext) {
                    case 'formCrearTicket':
                        endpoint = basePath + "GlpiSelects/GetSelectCrearTicket";
                        break;
                    default:
                        toastr.error("Formulario no reconocido", "Error");
                        return;
                }
                vm.endpoint = endpoint;
                this.GetSubCategorias().then(() => {
                    vm.inicializarDataTableSubCategorias()
                    $('#modalSubCategoria').modal('show');
                });
                this.modalSubCategoria = true;
            },
            closeModalSubCategoria() {
                this.modalSubCategoria = false;
                this.subcategoria.isEditingSubCategoria = false;
                this.subcategoria.nuevoSubCategoriaNombre = '';
                this.subcategoria.nuevocolorSubCategoria = '';
                this.subcategoria.EditadoNombre = '';
                this.subcategoria.EditadoColor = '';
            },
            obtenerCategorias() {
                var vm = this

                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiCategorias/GetCategorias",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            toastr.success("Se obtuvieron todas las partidas", "Éxito");

                            const categoriaSelectModal = $('#cboCategoriaModal');
                            categoriaSelectModal.empty();
                            categoriaSelectModal.append('<option value="">Seleccione una partida</option>');
                            result.data.forEach((item) => {
                                categoriaSelectModal.append(`<option value="${item.Id}">${item.Nombre}</option>`);
                            });
                            categoriaSelectModal.select2({
                                placeholder: "Seleccione una partida",
                                allowClear: true,
                                //dropdownParent: $('#formCrearSubCategoria')
                            });
                            if (vm.subcategoria.isEditingSubCategoria) {
                                const categoriaSelectModal = $('#cboCategoriaModalEditar');
                                result.data.forEach((item) => {
                                    categoriaSelectModal.append(`<option value="${item.Id}">${item.Nombre}</option>`);
                                });
                                categoriaSelectModal.val(vm.subcategoria.EditadoCategoriaId).trigger('change');
                            }
                        }
                        else {
                            toastr.error("Error al obtener partidas", "Error");

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
            GetSubCategorias() {
                var vm = this;
                return new Promise((resolve, reject) => {
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiSubCategorias/GetSubCategorias",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show")
                    },
                    success: function (result) {
                        if (result.success) {
                            vm.subCategoriaData = result.data;
                            resolve(); 
                        } else {
                            toastr.error(result.displayMessage, "Error");
                            reject();
                        }
                    },
                    error: function (request, status, error) {
                        toastr.error("Error al cargar los subcategorias de ticket", "Error");
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide")
                    }
                });
                });
            },
            inicializarDataTableSubCategorias() {
                const vm = this;
                const idTable = 'tableSubCategorias';
                const columns = [
                    { data: "Id", title: "ID" },
                    { data: "Nombre", title: "Nombre" },
                    { data: "NombreCategoria", title: "Categoria" },
                    {
                        data: "Id",
                        title: "Acciones",
                        render: function (data, type, row, meta) {
                            return `
                    <button 
                        style="width:40px; height:40px;" 
                        type="button" 
                        class="btn btn-xs btn-warning btnEditarSubCategorias" 
                        data-id="${data}">
                        <i class="glyphicon glyphicon-pencil"></i>
                    </button>
                `;
                        },
                        className: "text-center",
                        orderable: false
                    }
                ];
                this.renderDataTable(idTable, this.subCategoriaData, columns);
                $(`#${idTable}`).on('click', '.btnEditarSubCategorias', function () {
                    const id = $(this).data('id');
                    vm.editarSubCategoria(id);
                });
            },
            guardarSubCategoria() {
                var vm = this
                const data = {
                    IdCategoria: $("#cboCategoriaModal").val(),
                    Nombre: $("#nombreSubCategoria").val(),
                    Estado: true,
                }
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiSubCategorias/SaveSubCategoria",
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
                            vm.GetSubCategorias().then(() => {
                                vm.inicializarDataTableSubCategorias()
                            })

                            //vm.categoria.nuevoCategoriaNombre = '';
                            //vm.categoria.tipoPartida = '';
                            vm.actualizarSelectSubCategoria(
                                "#cboSubCategoria",
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
            editarSubCategoria(id) {
                const index = this.subCategoriaData.findIndex(p => p.Id === id);
                if (index !== -1) {
                this.subcategoria.isEditingSubCategoria = true;
                    this.subcategoria.EditadoNombre = this.subCategoriaData[index].Nombre;
                    this.subcategoria.EditadoCategoriaId = this.subCategoriaData[index].IdCategoria;
                this.subcategoria.EditadoIndex = index;
                    this.obtenerCategorias()
                }
            },
            actualizarSubCategoria() {
                var vm = this;
                const data = {
                    Id: this.subCategoriaData[this.subcategoria.EditadoIndex].Id,
                    Nombre: this.subcategoria.EditadoNombre,
                    IdCategoria: this.subcategoria.EditadoCategoriaId,
                };
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiSubCategorias/SaveSubCategoria",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },
                    success: function (result) {

                        //success: function (result) {
                        //    if (result.success) {
                        //        toastr.success("Categoria actualizado con éxito.", "Éxito");
                        //        vm.GetCategorias();
                        //        const partidaSeleccionada = vm.categoria.EdittipoPartida;
                        //        vm.actualizarSelectCategoria(
                        //            "#cboCategoria",
                        //            vm.endpoint,
                        //            //null,
                        //            (categoria) => {
                        //                //console.log("Categoría:", categoria); 
                        //                return categoria.IdPadre == partidaSeleccionada;
                        //            },
                        //            vm.formContext,
                        //        );
                        //        vm.categoria.isEditingCategoria = false;
                        //        vm.categoria.EditadoIndex = null;
                        //    } else {
                        //        toastr.error("Error al actualizar categoria", "Error");
                        //    }

                        if (result.success) {
                            toastr.success("SubCategoria actualizado con éxito.", "Éxito");
                            vm.GetSubCategorias().then(() => {
                                vm.inicializarDataTableSubCategorias()
                            })
                            const partidaSeleccionada = vm.subcategoria.EditadoCategoriaId;
                            //vm.subcategoria.isEditingSubCategoria = EditadoCategoriaId
                            vm.actualizarSelectSubCategoria(
                                "#cboSubCategoria",
                                vm.endpoint,
                                //null,
                                (subcategoria) => {
                                    return subcategoria.IdPadre == partidaSeleccionada;
                                },
                                vm.formContext
                            );
                            vm.subcategoria.isEditingSubCategoria = false;
                            vm.subcategoria.EditadoIndex = null;
                        } else {
                            toastr.error("Error al actualizar subcategoria", "Error");
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
            actualizarSelectSubCategoria(selector, endpoint, filterFn = null, formContext = '') {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: endpoint,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: (result) => {
                        if (result.success) {
                            let datos = result.data.subCategorias;
                            console.log("datos")
                            console.log(datos)
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
            ///////////////////
            closeModalDetailSeguimiento() {
                this.showModalDetailSeguimiento = false
                document.documentElement.style.overflow = "auto"
            },
            previewFiles(event) {
                const files = event.target.files;
                this.file = files;
                this.previewList = []; // Limpia la lista de previsualización

                Array.from(files).forEach(file => {
                    const fileReader = new FileReader();
                    const fileData = {
                        id: Date.now() + Math.random(),
                        name: file.name,
                        size: `${(file.size / 1024).toFixed(2)} KB`,
                        type: null,
                        icon: '',
                        typeClass: '',
                        preview: null,
                    };

                    if (file.name.endsWith('.xls') || file.name.endsWith('.xlsx')) {
                        // Archivos Excel
                        fileData.type = 'excel';
                        fileData.icon = this.iconFiles.excel;
                        fileData.typeClass = 'excel';
                    } else if (file.name.endsWith('.doc') || file.name.endsWith('.docx')) {
                        // Archivos Word
                        fileData.type = 'word';
                        fileData.icon = this.iconFiles.word;
                        fileData.typeClass = 'word';
                    } else if (file.name.endsWith('.pdf')) {
                        // Archivos PDF
                        fileData.type = 'pdf';
                        fileData.icon = this.iconFiles.pdf;
                        fileData.typeClass = 'pdf';
                    } else if (file.type.startsWith('image/')) {
                        // Archivos de imagen
                        fileData.type = 'image';
                        fileData.typeClass = 'image';
                        fileReader.onload = (e) => {
                            fileData.preview = e.target.result;
                            this.previewList.push(fileData);
                        };
                        fileReader.readAsDataURL(file);
                        return;
                    } else {
                        // Otros tipos no soportados
                        fileData.type = 'unsupported';
                        fileData.icon = '<span>❓</span>';
                        fileData.typeClass = 'unsupported';
                    }

                    this.previewList.push(fileData);
                });
                //this.resetFileInput(); esta limpiando la imagen luego del preview y se guarda vacio 
            },
            actualizarSubCategorias(categoriaId) {
                // Filtrar las subcategorías según el idPadre (categoría seleccionada)
                const subCategoriasFiltradas = this.dataSelects.subCategorias.filter(
                    (subCategoria) => subCategoria.IdPadre == categoriaId
                );

                // Llenar el select de subcategoría con los resultados
                const $subCategoriaSelect = $('#cboSubCategoria');
                $subCategoriaSelect.empty();
                $subCategoriaSelect.append('<option value="">Seleccione una Subcategoría</option>');
                subCategoriasFiltradas.forEach((item) => {
                    $subCategoriaSelect.append(`<option value="${item.Valor}">${item.Texto}</option>`);
                });
                $subCategoriaSelect.select2({
                    placeholder: "Seleccione una Subcategoría",
                    allowClear: true,
                    dropdownParent: $('#formCrearTicket')
                }); // Refrescar Select2 con placeholder
            },
            actualizarCategorias(tipoOperacionId) {
                console.log(tipoOperacionId)
                console.log('categorias oinicial')
                console.log(this.dataSelects.categorias)

                // Filtrar las categorías según el idPadre (tipo de operación)
                const categoriasFiltradas = this.dataSelects.categorias.filter(
                    (categoria) => categoria.IdPadre == tipoOperacionId
                );
                console.log('categorias filtrada')
                console.log(categoriasFiltradas)
                // Llenar el select de categoría con los resultados
                const $categoriaSelect = $('#cboCategoria');
                $categoriaSelect.empty();
                $categoriaSelect.append('<option value="">Seleccione una Categoría</option>');
                categoriasFiltradas.forEach((item) => {
                    $categoriaSelect.append(`<option value="${item.Valor}">${item.Texto}</option>`);
                });
                $categoriaSelect.select2({
                    placeholder: "Seleccione una Categoría",
                    allowClear: true,
                    dropdownParent: $('#formCrearTicket')
                }); // Refrescar Select2 con placeholder


                // Limpiar y deshabilitar subcategorías hasta que se seleccione una categoría
                $('#cboSubCategoria').empty().append('<option value="">Seleccione una Subcategoría</option>').select2({
                    placeholder: "Seleccione una Subcategoría",
                    allowClear: true,
                    dropdownParent: $('#formCrearTicket')
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
                    default:
                        return '';
                }
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

          
            //    guardarCategoriaRubro() {
            //        const vm = this; // Guardar el contexto de Vue
            //        const data = {
            //            IdPartida: $("#tipoPartida").val(),
            //            Nombre: $("#categoriaNombre").val(),
            //        };

            //        $.ajax({
            //            type: "POST",
            //            url: basePath + "GlpiCategorias/SaveCategoria",
            //            cache: false,
            //            contentType: "application/json; charset=utf-8",
            //            dataType: "json",
            //            data: JSON.stringify(data),
            //            beforeSend: function (xhr) {
            //                $.LoadingOverlay("show");
            //            },
            //            success: function (result) {
            //                if (result.success) {
            //                    toastr.success("Categoría registrada con éxito.", "Éxito");
            //                    //vm.closeModalRubro();

            //                    // Obtener los datos actualizados y actualizar solo el select de categorías
            //                    $.ajax({
            //                        type: "POST",
            //                        url: basePath + "GlpiSelects/GetSelectCrearTicket",
            //                        cache: false,
            //                        contentType: "application/json; charset=utf-8",
            //                        dataType: "json",
            //                        success: function (result) {
            //                            const categorias = result.data.categorias;

            //                            // Actualizar solo el select de categorías
            //                            vm.actualizarSelect("#cboCategoria", categorias);
            //                        },
            //                        error: function () {
            //                            toastr.error("Error al cargar las categorías", "Error");
            //                        }
            //                    });
            //                } else {
            //                    toastr.error("Error al registrar la categoría", "Error");
            //                }
            //            },
            //            error: function (request, status, error) {
            //                toastr.error("Error", "Mensaje Servidor");
            //            },
            //            complete: function (resul) {
            //                $.LoadingOverlay("hide");
            //            }
            //        });
            //    },
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

            $('.header-bar ').attr('style', 'z-index: 0 !important;');
            this.GetTicketsPorIdUsuario();
            this.obtenerSalas()
            this.obternetDataSelects()

            $('#cboPartida').on('change', (event) => {
                const selectedId = parseInt(event.target.value);
                this.actualizarCategorias(selectedId);
                console.log(selectedId)
            });
            $('#cboCategoria').on('change', (event) => {
                const selectedId = parseInt(event.target.value);
                this.actualizarSubCategorias(selectedId);
            });
            $('#cboSubCategoria').on('change', (event) => {
                const selectedId = event.target.value;
                console.log(selectedId)
            });

        },
        watch: {
            showModal(newVal) {
                if (newVal) {
                    this.ticketIdToDelete = this.ticketSelect.Id;
                }
            },
            showConfirmacionCierreModal(newVal) {
                if (newVal) {
                    this.ticketIdConfirmarCierre = this.ticketSelect.Id;
                }
            },
            showDeleteTicket(newVal) {
                if (newVal) {
                    this.ticketIdToDelete = this.ticketSelect.Id;
                }
            },
            'categoria.tipoPartida': function (newValue) {
                $('#tipoPartida').val(newValue).trigger('change');
            }
        },


    })
})

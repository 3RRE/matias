
document.addEventListener('DOMContentLoaded', function () {
    var AppConfiguracionAlerta = new Vue({
        el: '#AppGlpiVista',
        data: {
            message: "raaa",
            sheet: false,
            showModal: false,
            modalOperacion: false,
            modalAtencion: false,
            modalPartida: false,
            modalRubro: false,
            modalSubCategoria: false,
            modalClasificacionProblema: false,
            modalEstadoActual: false,
            modalIdentificador: false,
            previewList: [],
            sheetActive: false,
            activeTab: "tickets",
            dataSelects: {},
            tabs: [
                {
                    name: "tickets",
                    label: "Tickets creados",
                    iconPath: "M12 8v8m-4-4h8",
                },
                {
                    name: "sinAsignarTickets",
                    label: "Tickets sin asignar",
                    iconPath: "M5 13l4 4L19 7",
                },
                {
                    name: "misTickets",
                    label: "Mis tickets asignados",
                    iconPath: "M5 13l4 4L19 7",
                },
                {
                    name: "misTickets",
                    label: "Mis tickets asignados",
                    iconPath: "M5 13l4 4L19 7",
                },
            ],
            ticketSelect: {},
            historial: [
              
            ],
            tickets: []
        },
        computed: {
            sortedHistory() {
                return [...this.historial].sort((a, b) => new Date(b.FechaRegistro) - new Date(a.date));
            },
        },
        methods: {
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
            getNivelTexto(nivel) {
                switch (nivel) {
                    case 1:
                        return "Bajo";
                    case 2:
                        return "Normal";
                    case 3:
                        return "Urgente";
                    default:
                        return "";
                }
            },
            sheetToggle() {
                this.sheet = !this.sheet;
                if (this.sheet) {
                    setTimeout(() => {
                        this.sheetActive = true;
                        this.completeSelects()
                      
                    }, 100)
                } else {
                    this.sheetActive = false;
                }
            },
            triggerFileInput() {
                this.$refs.fileInput.click(); 
            },
            openModalDetail(id, item) {
                this.ticketSelect = item;
                console.log(id)
                this.obtenerDetalleHistorial(id)
                this.showModal = true;
                document.documentElement.style.overflow = "hidden";
                
            },
            closeModalDetail() {
                this.showModal = false
                document.documentElement.style.overflow = "auto"
               
            },
           
            obtenerTickets() {
                var vm = this;
                $.ajax({
                    type: "POST",
                    url: basePath + "GlpiTicket/GetTickets",
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
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
            
           
           

        },
        created: function () {
        },

        mounted: function () {
            $('.header-bar ').attr('style', 'z-index: 0 !important;');
            $('.right-column-content').css('padding', 'unset');
          
            this.obtenerTickets();
           

        },
        watch: {


        },


    })
})

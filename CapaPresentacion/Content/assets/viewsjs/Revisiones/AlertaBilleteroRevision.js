$(document).ready(function () {
    //obtenerRevisionAlertasSala()
});






document.addEventListener('DOMContentLoaded', function () {
    var AppIncidentes = new Vue({
        el: '#AppRevisiones',
        data: {
            revisiones: [],
            filtro: '',
            mostrarPanel:true
        },
        computed: {
            datosFiltrados() {
                if (!this.filtro) {
                    return this.revisiones
                }
                const filtroLower = this.filtro.toLowerCase();
                return this.revisiones.filter(dato =>
                    dato.NombreSala.toLowerCase().includes(filtroLower)
                );
            },
            cantidadSalas() {
                return this.revisiones.length
            }
        },
        methods: {
            test: function (event) {
                console.log(event.target.value);
                this.sistemaSelect = event.target.value
                console.log('Sistema seleccionado:', this.sistemaSelect);
            },
            generarExcel: function () {
                generarExcel(this.revisiones,this)
            },
            nuevaConsulta: function () {
                obtenerRevisionAlertasSala(this)
            }
        },
        created: function () {
            //obtenerListadoSistemas()
        },

        mounted: function () {
            var vm = this;
            obtenerRevisionAlertasSala(vm)
        },
        watch: {
            //idSistema: {
            //    handler: function () {
            //        var vm = this
            //        obtenerListadoIncidenciasSistema(vm.idSistema, vm)
            //    }
            //},
            //idIncidencia: function () {
            //    var vm = this
            //    obtenerListadoSolucionesIncidencia(vm.idIncidencia, vm)
            //}

        },


    })
})





const obtenerRevisionAlertasSala = (cv) => {
    $.LoadingOverlay("show");

    return $.ajax({
        type: "POST",
        cache: false, 
        url: basePath + "Revisiones/ConsultaRegistrosAlertaBilleteroxUsuario",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
        },
        success: function (response) {
            console.log("soy el post");
            console.log(response);
            cv.revisiones = response.data;
            console.log(cv.revisiones);

            $.LoadingOverlay("hide");
        },
        error: function (request, status, error) {
            toastr.error("No tiene permisos", "Mensaje Servidor");
            $.LoadingOverlay("hide");

        },
        complete: function (resul) {
        }
    });
}


const generarExcel =(data,cv)=> {
    $.LoadingOverlay("show");
    console.log('data de excel')
    console.log(data)
    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Revisiones/ReporteRevisionesJsonxUsuario",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        beforeSend: function (xhr) {
        },
        success: function (response) {
            
           
                let data = response.data;
                let file = response.excelName;
                let a = document.createElement('a');
                a.target = '_self';
                a.href = "data:application/vnd.ms-excel;base64, " + data;
                a.download = file;
                a.click();
            $.LoadingOverlay("hide");
        },
        error: function (request, status, error) {
            toastr.error("No tiene permisos", "Mensaje Servidor");
            $.LoadingOverlay("hide");

        },
        complete: function (resul) {
        }
    });
}


const renderDiscoss= (data)=> {
    var dataFinal = data
    
    objetodatatable = $("#tablaAlertaBilletero").DataTable({
        "bDestroy": true,
        "bSort": true,
        "scrollCollapse": true,
        "scrollX": false,
        "sScrollX": "100%",
        "paging": true,
        "ordering": true,
        "aaSorting": [],
        "autoWidth": false,
        "bAutoWidth": true,
        "bProcessing": true,
        "bDeferRender": true,
        data: dataFinal,
        columns: [
            {
                data: "CodSala", title: "Codigo de la sala",
            },
           
            {
                data: "NombreSala", title: "Nombre de la sala",
            },
            {
                data: "Registros", title: "Nombre de la sala",
            },
        ],
    });

}
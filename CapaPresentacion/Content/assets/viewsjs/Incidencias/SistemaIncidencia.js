var showInfo = false;
let idIncidenciaResult = 0;
let idSistemaResult = 0;


$(document).ready(function () {


    $("#cboSistemaModal").select2({
        dropdownParent: $('#modalFormularioIncidencia'),
        placeholder: "Seleccione un sistema",
        language: {
            noResults: function () {
                return "No se encontraron resultados";
            },
            searching: function () {
                return "Buscando...";
            },
        }
    })
  



    $(document).on("click", "#btnSistema", function () {
        $("#NombreSistema").val('');
        $("#DescripcionSistema").val('');
        $("#modalFormularioSistema").modal("show");
    })




    })





    const obtenerListadoSistemasModal = () => {
        $.LoadingOverlay("show");

        return $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Incidencias/ListarSistemaIncidencia",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
            },
            success: function (response) {
                console.log("soy el post")
                console.log(response.data);
                $.LoadingOverlay("hide");
                renderSelectSistemaModal(response.data);
            },
            error: function (request, status, error) {
                toastr.error("No tiene permisos", "Mensaje Servidor");
                $.LoadingOverlay("hide");

            },

        })
    }

function beforeSend(xhr) {
        $.LoadingOverlay("show");
    }

    const insertarIncidencia = (data) => {
        return $.ajax({
            type: "POST",
            cache: false,
            url: basePath + "Incidencias/AgregarIncidencia",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            beforeSend: beforeSend(),
            success: function (response) {
                console.log("listado incidencias")
                console.log(response.data);
                toastr.success(response.mensaje, "Incidencia Agregada")

            },
            complete: function () {
                $.LoadingOverlay("hide");
            },


        })
    }

    const renderSelectSistemaModal = (data) => {
        $.each(data, function (index, value) {
            $("#cboSistemaModal").append(`<option  data-id="${value.idSistema}">${value.nombre}</option>`)
        });
    }





document.addEventListener('DOMContentLoaded', function () {
    var AppIncidentes = new Vue({
        el: '#AppIncidente',
        data: {
            sistemas: [],
            incidencias: [],
            soluciones: [],
            sistemaSelect:'',
            incidenciaSelect: '',
            idSistema: 0,
            idIncidencia: 0,
            solucionBoton: false,
            incidenciaVacia: false,
            imagenBusqueda:true
        },
        computed: {

        },
        methods: {
            test: function (event) {
                console.log(event.target.value);
                this.sistemaSelect = event.target.value
                console.log('Sistema seleccionado:', this.sistemaSelect);
            },
            toggleDescription: function (solucion) {
                console.log(solucion)
                solucion.showDescription = !solucion.showDescription
                solucion.isSelected = !solucion.isSelected
            },
            agregarSolucion: function () {

            }
          
        },
        created: function () {
            //obtenerListadoSistemas()
        },
    
        mounted: function () {
            var vm = this;

            $("#cboSistemas").select2({
                placeholder: "Seleccione un sistema",
            });

            $("#cboIncidencias").select2({
                placeholder: "Seleccione una incidencia",
            });


            $(document).on("click", "#btnIncidencia", function () {

                //$("#cboSistemaModal").val(null).trigger('change');
                $("#Nombre").val('');
                $("#Descripcion").val('');
                $('#cboSistemaModal').empty();
                obtenerListadoSistemasModal();
                $("#modalFormularioIncidencia").modal("show");
            });

            $(document).on("click", "#agregarSolucion", function (e) {
                e.preventDefault()
                $("#TituloSolucion").val('');
                $("#DescripcionSolucion").val('');
                $("#modalFormularioSolucion").modal("show");


            });
            $(document).on("click", ".btnGuardarSistema", function () {
                let nombre = $("#NombreSistema").val();
                let descripcion = $("#DescripcionSistema").val();

                if (!nombre) {
                    toastr.error("Ingrese un nombre de sistema", "Mensaje Servidor");
                    return false;
                }
                if (!descripcion) {
                    toastr.error("Ingrese una descripción.", "Mensaje Servidor");
                    return false;
                }
                let data = { nombre: nombre, descripcion: descripcion }
                nuevoSistema(data, vm)

            });

            $(document).on("click", ".btnGuardarSolucion", function () {
                let nombre = $("#TituloSolucion").val();
                let descripcion = $("#DescripcionSolucion").val();

                if (!nombre) {
                    toastr.error("Ingrese un titulo", "Mensaje Servidor");
                    return false;
                }
                if (!descripcion) {
                    toastr.error("Ingrese una descripción.", "Mensaje Servidor");
                    return false;
                }
                let data = { nombre: nombre, descripcion: descripcion, idIncidencia: vm.idIncidencia }
                insertarNuevaSolucion(data,vm)

            });

            $(document).on("click", ".btnGuardarIncidencia", function () {

                let sistema = $("#cboSistemaModal option:selected").data("id");
                let nombre = $("#Nombre").val();
                let descripcion = $("#Descripcion").val();

                if (!sistema) {
                    toastr.error("Seleccione un sistema.", "Mensaje Servidor");
                    return false;
                }
                if (!nombre) {
                    toastr.error("Ingrese un nombre.", "Mensaje Servidor");
                    return false;
                }
                if (!descripcion) {
                    toastr.error("Ingrese uuna descripcion.", "Mensaje Servidor");
                    return false;
                }

                var data = { titulo: nombre, descripcion: descripcion, idSistema: sistema };

                insertarIncidencia(data).then(
                    value => {
                        console.log(value)
                        console.log(sistema)
                        console.log(idSistemaResult)
                        if (vm.idSistema == sistema) {
                            console.log('id finales')
                            console.log(vm.idSistema)
                            console.log(sistema)
                            obtenerListadoIncidenciasSistema(vm.idSistema,vm)

                        }
                    }

                )

                $("#modalFormularioIncidencia").modal("hide");
            });

            obtenerSistemas(vm)

            //Observar cambios a el select de sistemas
            $("#cboSistemas").on("change", function () {
                vm.soluciones = [];
                vm.incidenciaSelect = '';
                let selectedItem = $(this).val();
                let selectedItemText = $(this).find("option:selected").text();
                vm.sistemaSelect = selectedItemText;
                vm.idSistema = selectedItem;
                vm.solucionBoton = false;
                console.log("Sistema seleccionado:", vm.sistemaSelect, vm.idSistema);
            });  


            //Observar cambios a el select de incidencias
            $("#cboIncidencias").on("change", function () {
                let selectedItem = $(this).val();
                let selectedItemText = $(this).find("option:selected").text();
                vm.incidenciaSelect = selectedItemText;
                vm.idIncidencia = selectedItem;
                vm.solucionBoton = true;
                console.log("Sistema seleccionado:", vm.sistemaSelect);
            });   
            
        },
        watch: {
            idSistema: {
                handler: function () {
                    var vm=this
                    obtenerListadoIncidenciasSistema(vm.idSistema,vm)
                }
            },
            idIncidencia: function () {
                var vm = this
                obtenerListadoSolucionesIncidencia(vm.idIncidencia, vm)
            }

        },


        })
})


const obtenerSistemas = (cv) => {

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Incidencias/ListarSistemaIncidencia",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
        },
        success: function (response) {

            cv.sistemas = response.data
            console.log("data de sistemas")
            console.log(cv.sistemas)

        },
        error: function (request, status, error) {
            toastr.error("No tiene permisos", "Mensaje Servidor");
            $.LoadingOverlay("hide");

        },

    })
}



const nuevoSistema = (data,cv) => {
    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Incidencias/AgregarSistemaIncidencia",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            toastr.success(response.mensaje, "Sistema Agregado");

            $("#modalFormularioSistema").modal("hide");
            obtenerSistemas(cv);
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        error: function (request, status, error) {
            toastr.error("No tiene permisos", "Mensaje Servidor");
            $.LoadingOverlay("hide");

        },

    })
}



const obtenerListadoIncidenciasSistema = (idSistema, cv) => {
    cv.imagenBusqueda = false
    cv.incidenciaVacia = false;
    $.LoadingOverlay("show");
    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Incidencias/ListarIncidencia",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ idSistema: idSistema }),
        beforeSend: function (xhr) {
        },
        success: function (response) {
            cv.incidencias = null
            cv.incidencias = response.data
            $.LoadingOverlay("hide");
        },
        error: function (request, status, error) {
            toastr.error("No tiene permisos", "Mensaje Servidor");
            $.LoadingOverlay("hide");

        },

    })
}


const obtenerListadoSolucionesIncidencia = (idIncidencia, cv) => {
    cv.incidenciaVacia = false;

    $.LoadingOverlay("show");
    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Incidencias/ListarSolucionIncidencia",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ idIncidencia: idIncidencia }),
        beforeSend: function (xhr) {
        },
        success: function (response) {
            cv.soluciones = response.data.map(solucion => {
                return { ...solucion, isSelected: false };
            });
            cv.incidenciaVacia = true;

            
            $.LoadingOverlay("hide");
        },
        error: function (request, status, error) {
            toastr.error("No tiene permisos", "Mensaje Servidor");
            $.LoadingOverlay("hide");

        },

    })
}

const insertarNuevaSolucion = (data,cv) => {
    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Incidencias/AgregarSolucionIncidencia",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            console.log("listado incidencias")
            console.log(response.data);
            toastr.success(response.mensaje, "Incidencia Agregada");
            obtenerListadoSolucionesIncidencia(cv.idIncidencia,cv);
            $("#modalFormularioSolucion").modal("hide");

        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        error: function (request, status, error) {
            toastr.error("No tiene permisos", "Mensaje Servidor");
            $.LoadingOverlay("hide");

        },

    })
}
//const { error } = require("jquery");

let fotoOriginal = "";
let fotoDefault = "";
let activeFoto = false;
var timerFoto;

$(document).ready(function () {

    $("#dniBusqueda").focus();

    $("#dniBusqueda").keypress(function(e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if(code==13){
            $("#btnBuscar").trigger("click");
        }
    });
    $("#textBusqueda").text("Busqueda por Nro. de Documento");


    $(document).on('keypress', '#dniBusqueda', function (event) {


        var regex = new RegExp("^[0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }


    });


    $(document).on("click", ".btnVerFoto", function () {

        if (activeFoto) {

            $(".btnVerFoto").removeClass('btn-danger').addClass('btn-primary');
            $(".img1").attr("src", fotoDefault);
            activeFoto = false;

            clearInterval(timerFoto);

        } else {

            $(".btnVerFoto").removeClass('btn-primary').addClass('btn-danger');
            $(".img1").attr("src", fotoOriginal);
            activeFoto = true;

            timerFoto = setInterval(function () {
                $(".btnVerFoto").trigger('click');
            }, 5000);

        }

    });



    // $(document).on("click", "#btnDni", function () {
    //     $("#textBusqueda").text("Busqueda por DNI");
    //     $("#dni").show();
    //     $("#foto").hide();
    // });
    // $(document).on("click", "#btnFoto", function () {
    //     $("#textBusqueda").text("Busqueda por Foto");
    //     $("#dni").hide();
    //     $("#foto").show();
    // });
    $(document).on("click", "#btnBuscar", function (e) {
        e.preventDefault()
        let buscar = $("#dniBusqueda").val().trim()
        if (buscar !== "" && buscar.length >= 8){
            let url=basePath + "CALBusqueda/BuscarGeneralJson?buscar="+buscar
            ajaxhr = $.ajax({
                url: url,
                type: "GET",
                contentType: "application/json",
                beforeSend: function () {
                    $.LoadingOverlay("show");
                },
                complete: function () {
                    AbortRequest.close()
                    $.LoadingOverlay("hide");
                },
                success: function (response) {
                    if (response.respuesta) {
                        let data = response.data
                        imprimirCard(data)
                        enviarPeticionServicioLED(data.Tipo)
                        $("#dniBusqueda").val("")
                        $("#dniBusqueda").focus();
                    } else {
                        toastr.warning(response.mensaje, "Advertencia");
                        $("#dniBusqueda").val("")
                        $("#dniBusqueda").focus();
                    }
                },
                error: function (xmlHttpRequest, textStatus, errorThrow) {
        
                }
            });

            AbortRequest.open()
        }
        else{
            toastr.warning("Debe Ingresar un Nro. de Documento igual o mayor a 8 digitos.","Advertencia");
        }
       
        // imprimirCard();
    });



});

function imprimirCard(data) {

    fotoOriginal = data.Foto;
    fotoDefault = data.FotoDefault;

    $("#RenderData").html("");
    htmlRender=`
    <div class="divcontenedor">
        <div class="contendor">
            <div class="imgBx">
                ${renderizarMensaje(data.dataAdicional, data.Tipo)}
                <img class="img1" src="${data.FotoDefault}" alt="${data.NombreCompleto}">
                <img class="img2" src="${data.ImgFondo}" alt="${data.NombreCompleto}">
            </div>
            <div class="detalles">
                <div class="contenido">
                <br/>
                    <h2 style="font-size:35px;font-weight: 700;" align="center">
                        ${data.Codigo}
                    </h2>
                    <h2 align="center" style="width:100%;font-weight: 500;">
                        ${data.NombreCompleto} (${data.Tipo})
                    </h2>
                    <h3 style="font-size:15px;font-weight: 500;" align="center">
                        DOI : ${data.DNI}
                    </h3>
                    ${renderizarDataAdicional(data.dataAdicional, data.Tipo)}

                    <div align="center">
                        <button type="button" class="btn btn-xs btn-primary btnVerFoto" align="center"><i class="glyphicon glyphicon-camera"></i></button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    `

    $("#RenderData").append(htmlRender);
    $("#divDni").show();
}

const renderizarMensaje = (data, tipo) => {
    console.log(data);
    if (tipo == "Cliente" && data.errorAPI && data.mensaje) {
        return `
        <div class="api-message" align="center" style="">
            <p class="api-message-text">
                ${data.mensaje}<br/>
            </p>
        </div>
        `
    }
    return ''
}

function renderizarDataAdicional(data,tipo){
    if(tipo=="Ludopata"){
        if(data.ContactoID>0){
            return `
            <h3 style="font-size:15px;font-weight: 500;" align="center">
                Telefono : ${data.Telefono}
            </h3>
            <h3 style="font-size:15px;font-weight: 500;" align="center">
                Fecha Inscripcion : ${moment(data.FechaInscripcion).format("DD/MM/YYYY")}
            </h3>
            <div align="center">
                <h2 align="center">Información de Contacto</h2>
                <p style="text-align: center;font-weight: 500;font-size: 15px;">
                    Nombre : ${data.NombreContacto} ${data.ApellidoPaternoContacto} ${data.ApellidoMaternoContacto}<br/>
                    Telefono : ${data.TelefonoContacto}<br/>
                    Celular : ${data.CelularContacto}
                </p>
            </div>
            `
        }
        return ''
    }
    if (tipo == "Prohibido de Ingreso" || tipo == "Problematico" || tipo == "Cliente Alerta" ){
        return `
        <div align="center">
            <h2 align="center">Información Adicional</h2>
            <br/>    
            <p style="text-align: center;font-weight: 500;font-size: 15px;">
                Fecha Inscripcion : ${moment(data.FechaInscripcion).format("DD/MM/YYYY")=="31/12/1752"?"":moment(data.FechaInscripcion).format("DD/MM/YYYY")}<br/>
                Sala : ${data.SalaNombreCompuesto}
            </p>
        </div>
        `
    }
    if(tipo=="Politico"){
        return `
        <div align="center">
            <h2 align="center">Información Adicional</h2>
            <p style="text-align: center;font-weight: 500;font-size: 15px;">
                Fecha Registro : ${moment(data.FechaRegistro).format("DD/MM/YYYY")}<br/>
                Entidad Estatal : ${data.EntidadEstatal}<br/>
                Cargo : ${data.cargoPoliticoNombre}
            </p>
        </div>
        `
    }
    if(tipo=="Entidad Publica"){
        return `
        <div align="center">
            <h2 align="center">Información Adicional</h2>
            <p style="text-align: center;font-weight: 500;font-size: 15px;">
                Fecha Registro : ${moment(data.FechaRegistro).format("DD/MM/YYYY")}<br/>
                Entidad Publica : ${data.EntidadPublicaNombre}<br/>
                Cargo : ${data.CargoEntidadNombre}
            </p>
        </div>
        `
    }
    if (tipo == "R. Stacker Billetero") {
        return `
        <div align="center">
            <h2 align="center">Información Adicional</h2>
            <br/>    
            <p style="text-align: center;font-weight: 500;font-size: 15px;">
                Fecha Inscripcion : ${moment(data.FechaRegistro).format("DD/MM/YYYY") == "31/12/1752" ? "" : moment(data.FechaRegistro).format("DD/MM/YYYY")}<br/>
                Sala : ${data.SalaNombre}
            </p>
        </div>
        `
    }
    if(tipo=="Trabajador"){
        return `
        <div align="center">
            <h2 align="center">Información Adicional</h2>
            <p style="text-align: center;font-weight: 500;font-size: 15px;">
                Empresa : ${data.DE_NOMB}<br/>
                Sede : ${data.DE_SEDE}<br/>
                Puesto : ${data.DE_PUES_TRAB}
            </p>
        </div>
        `
    }
    if(tipo=="Ex Trabajador"){
        return `
        <h3 style="font-size:15px;font-weight: 500;" align="center">
            Fecha Cese : ${moment(data.FE_CESE_TRAB).format("DD/MM/YYYY")}
        </h3>
        <div align="center">
            <h2 align="center">Información Adicional</h2>
            <br/>    
            <p style="text-align: center;font-weight: 500;font-size: 15px;">
                Empresa : ${data.DE_NOMB}<br/>
                Sede : ${data.DE_SEDE}<br/>
                Puesto : ${data.DE_PUES_TRAB}
            </p>
        </div>
        `
    }
    if (tipo == "Persona con Observación") {
        return `
        <div align="center">
            <h2 align="center">Información Adicional</h2>
            <p style="text-align: center;font-weight: 500;font-size: 15px;">
                Mensaje : ${data.Mensaje}<br/>
            </p>
        </div>
        `
    }
    //if (tipo == "Cliente" && data.errorAPI) {
    //    return `
    //    <div align="center">
    //        <h2 align="center" style="color: red">ERROR API</h2>
    //        <p style="text-align: center;font-weight: 500;font-size: 15px;color: red; ">
    //            ${data.mensaje}<br/>
    //        </p>
    //    </div>
    //    `
    //}
    return ''
}
function enviarPeticionServicioLED(tipo){
    let clave=obtenerCodigo(tipo)
    colorLED(clave)
}
function colorLED(clave) {
    $.ajax({
        async: false,
        url: 'http://localhost:3030/api/data/ludopatatipo?comando=' + clave,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            //setTimeout(function(){ colorled(puerto,'A') }, 10000);
        },
        error: function (request, message, error) {
            //createSimpleAlert("Error:", "Servicio ausente.", "error");
        }
    });
}
function obtenerCodigo(tipo){
    let tipos={
        'Ludopata':'L',
        'Prohibido de Ingreso':'H',
        'Problematico':'H',
        'Politico':'P',
        'Entidad Publica':'E',
        'Trabajador':'C',//segun el antiguo sistema les asigna la C de cliente 
        'Ex Trabajador':'C',//segun el antiguo sistema les asigna la C de cliente 
        'Cliente':'C',
        'default':''
    }
    return tipos[tipo]||tipos['default']
}



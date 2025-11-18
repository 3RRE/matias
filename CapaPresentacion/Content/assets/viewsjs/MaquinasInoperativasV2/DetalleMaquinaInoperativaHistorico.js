
let listaUsuarios = [];
let textSearch = '';
let addTextSearch = false;
let arrobaDesaparece = false;
let listaUsuariosEtiquetados = [];
let listaEtiquetas = [];
let cargaPrimeraVez = true;
var stepNumber = 0;

$(document).ready(function () {

    console.log(maquinaInoperativa)

    obtenerCorreos().done(response => {
        if (response.data) {
            renderCorreos(response.data)
        }
    })

    VistaAuditoria("MIMaquinaInoperativa/HistoricoMaquinaInoperativa", "VISTA", 0, "", 3);

    $(document).on("click", ".btnListar", function () {

        window.location.assign(basePath + "MaquinasInoperativasV2/ListadoMaquinaInoperativa");

    });

    $(document).on("click", "#btnExcel", function () {

        GenerarExcel(maquinaInoperativa.CodMaquinaInoperativa);

    });

    if (maquinaInoperativa.CodMaquinaInoperativa == 0) {
        window.location.assign(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativa");
    }

    CargarComentarios();

    $(document).on("click", ".btnCorreos", function () {


        $("#modalCorreos").modal("show");
    });

    $(document).on("click", ".btnCompra", function () {

        const input = document.getElementById("fechaHoraOrdenCompra");
        if (!input.value.trim()) {
            const limaTime = new Date(new Date().toLocaleString("en-US", { timeZone: "America/Lima" }));

            // Formatear a 'YYYY-MM-DDTHH:MM' (este es el formato adecuado para un input tipo datetime-local)
            const pad = num => String(num).padStart(2, '0');
            const formatted = limaTime.getFullYear() + '-' +
                pad(limaTime.getMonth() + 1) + '-' +
                pad(limaTime.getDate());
                //+ 'T' +
                //pad(limaTime.getHours()) + ':' +
                //pad(limaTime.getMinutes());

            // Solo asignar si el campo está vacío
            console.log('se modifico');
            input.value = formatted;
        }
        $("#modalOrdenCompra").modal("show");
    });

    $(document).on("click", ".btnAgregarOrden", function () {

        AgregarOrdenCompra();
    });

    $(document).on("click", ".btnReenviar", function () {

        ReenviarCorreos();
    });


    //GET DATA GUIDE

    //CREADO
    //Fecha
    //Usuario
    //Maquina
    //Modelo
    //Linea
    //Sala
    //Juego
    //Nro Serie
    //Propietario
    //Ficha
    //Marca
    //Token
    //Juego
    //Estado
    //Prioridad
    //Fecha inoperativa
    //Tecnico
    //Observaciones
    //Problemas
    //Piezas

    //ATENDIDA INOPERATIVA
    //Fecha
    //Usuario
    //Tecnico
    //Observaciones
    //Piezas
    //Repuestos

    //ATENCION REVISADA
    //Fecha
    //Usuario
    //Piezas
    //Repuestos

    //SOLICITUDES
    //Fecha
    //Usuario
    //Traspasos : Almacen Origen, Almacen Destino, Repuesto, Cantidad, Estado
    //Compras

    //ATENDIDA OPERATIVA
    //Fecha
    //Usuario
    // MAQUINA FUNCIONANDO OPERATIVAMENTE


    //SET DATA HTML


    const box = document.createElement("div");
    box.innerHTML = ``;


    let arrayProblemas = listaProblemas.map(item => { return `<p>${item.NombreProblema}</p>` })
    let arrayPiezas = listaPiezas.map(item => { return `<p>${item.NombrePieza}</p>` })
    let arrayRepuestos = listaRepuestos.map(item => { return `<p>${item.NombreRepuesto}</p>` })
    let estadoInoperativa = maquinaInoperativa.CodEstadoInoperativa == 1 ? "Op. Problemas" : (maquinaInoperativa.CodEstadoInoperativa == 2 ? "Inoperativa" : "Inoperativa")
    let codPrioridad = maquinaInoperativa.CodPrioridad == 1 ? "Urgente" : (maquinaInoperativa.CodPrioridad == 2 ? "Normal" : "Normal")
    const fechaMs = parseInt(maquinaInoperativa.FechaOrdenCompra.replace(/\/Date\((\-?\d+)\)\//, "$1"));
    const fechaOrdenCompraFormateada = moment(fechaMs).format("DD/MM/YYYY hh:mm a");

    const fechaMoment = moment(fechaMs);
    const fechaMinima = moment("01/01/1900 12:03 am", "DD/MM/YYYY hh:mm a");
    const fechaOrdenCompra = fechaMoment.isAfter(fechaMinima)
        ? fechaMoment.format("DD/MM/YYYY")
        : "No tiene";

    if (maquinaInoperativa.OrdenCompra && maquinaInoperativa.OrdenCompra.trim() !== '') {
        document.getElementById("ordenCompra").value = maquinaInoperativa.OrdenCompra;

    }
    if (fechaOrdenCompra != "No tiene") {
        const fechaOrdenCompraFormateadaNoAM = moment(fechaMs).format("YYYY-MM-DDTHH:mm");
        // Formateamos la fecha en el formato adecuado para datetime-local
        document.getElementById("fechaHoraOrdenCompra").value = fechaOrdenCompraFormateadaNoAM;

    }

    console.log(fechaOrdenCompra)
    if (maquinaInoperativa.CodEstadoProceso >= 1) {
        box.innerHTML += `
                     <div class="section-content discovery active" id="container-comment1">
                          <h2>CREADO</h2>
                          <p><b>Fecha</b>: ${moment(maquinaInoperativa.FechaCreado).format("DD/MM/YYYY hh:mm a")}</p>
                          <p><b>Usuario</b>: ${maquinaInoperativa.NombreUsuarioCreado}</p>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">DATOS MAQUINA</h3>
                              <div class="col-md-12 col-sm-12" style="padding:0;">
                                  <div class="input-group form-group ">
									  <p><b>Ley</b>: ${maquinaInoperativa.MaquinaLey}</p>
									  <p><b>Modelo</b>: ${maquinaInoperativa.MaquinaModelo}</p>
									  <p><b>Linea</b>: ${maquinaInoperativa.MaquinaLinea}</p>
									  <p><b>Sala</b>: ${maquinaInoperativa.MaquinaSala}</p>
									  <p><b>Juego</b>: ${maquinaInoperativa.MaquinaJuego}</p>
									  <p><b>Numero Serie</b>: ${maquinaInoperativa.MaquinaNumeroSerie}</p>
									  <p><b>Propietario</b>: ${maquinaInoperativa.MaquinaPropietario}</p>
									  <p><b>Ficha</b>: ${maquinaInoperativa.MaquinaFicha}</p>
									  <p><b>Marca</b>: ${maquinaInoperativa.MaquinaMarca}</p>
									  <p><b>Token</b>: ${maquinaInoperativa.MaquinaToken}</p>
                                  </div>
                              </div>
                          </fieldset>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">DATOS GENERALES</h3>
                              <div class="col-md-12 col-sm-12" style="padding:0;">
                                  <div class="input-group form-group ">
									  <p><b>Estado Inoperativa</b>: ${estadoInoperativa}</p>
									  <p><b>Prioridad</b>: ${codPrioridad}</p>
									  <p><b>Fecha Inoperativa</b>: ${moment(maquinaInoperativa.FechaInoperativa).format("DD/MM/YYYY hh:mm a")}</p>
									  <p><b>Observaciones</b>: ${maquinaInoperativa.ObservacionCreado}</p>
                                  </div>
                              </div>
                          </fieldset>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">PROBLEMAS</h3>
                              <div class="col-md-12">
                                  <div class="row">
                                      <div class=" col-md-12">
                                          <table id="tableProblemas" class="table table-striped table-bordered table-hover table-condensed highlight-color-red tableProblemas">
                                          </table>
                                      </div>
                                  </div>
                              </div>
                          </fieldset>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">CORREOS ENVIADOS</h3>
                              <div class="col-md-12 col-sm-12" style="padding:0;">
                                  <div class="input-group form-group listaCorreosCreados">

                                  </div>
                              </div>
                          </fieldset>

                     </div>
					 `

    }



    if (maquinaInoperativa.CodEstadoProceso == 3) {
        //ESTADO MAQUINA INOPERATIVA  | ATENDIDA INOPERATIVA 44
        box.innerHTML += `
                     <div class="section-content strategy" id="container-comment3">
                         <h2>ATENDIDA INOPERATIVA</h2>
                          <p><b>Fecha</b>: ${moment(maquinaInoperativa.FechaAtendidaInoperativa).format("DD/MM/YYYY hh:mm a")}</p>
                          <p><b>Usuario</b>: ${maquinaInoperativa.NombreUsuarioAtendidaInoperativa}</p>

                         <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; color:#111111;">
                            <h3 style="margin-bottom:13px; padding:0;">DATOS ATENCION</h3>
                            <div class="col-md-12 col-sm-12" style="padding:0;">
                                <div class="input-group form-group">
                                    <p><b>Tecnico</b>: ${maquinaInoperativa.TecnicoAtencion}</p>
                                    <p><b>Observaciones</b>: ${maquinaInoperativa.ObservacionAtencion}</p>
                                    <p><b>IST</b>: ${maquinaInoperativa.IST}</p>

                                    <p><b>Orden de compra</b>:
                                        <span class="ordenCompraValue">&nbsp${maquinaInoperativa.OrdenCompra.trim() == '' ? 'No tiene' : maquinaInoperativa.OrdenCompra}</span>
                                    </p>

                                    <p><b>Fecha Orden de compra</b>:
                                        <span class="fechaordenCompraValue">&nbsp${fechaOrdenCompra}</span>
                                    </p>
                                </div>
                            </div>
                        </fieldset>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">PROBLEMAS REAL</h3>
                              <div class="col-md-12">
                                  <div class="row">
                                      <div class=" col-md-12">
                                          <table id="tableProblemasNuevo" class="table table-striped table-bordered table-hover table-condensed highlight-color-red tableProblemasNuevo">
                                          </table>
                                      </div>
                                  </div>
                              </div>
                          </fieldset>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">CORREOS ENVIADOS</h3>
                              <div class="col-md-12 col-sm-12" style="padding:0;">
                                  <div class="input-group form-group listaCorreosInoperativas">

                                  </div>
                              </div>
                          </fieldset>

                     </div>
					 `

        box.innerHTML += `
                     <div class="section-content analysis" id="container-comment2">
                         <h2>ATENDIDA OPERATIVA</h2>
                          <p>Sin información.</p>
                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">CORREOS ENVIADOS</h3>
                              <div class="col-md-12 col-sm-12" style="padding:0;">
                                  <div class="input-group form-group listaCorreosOperativas">

                                  </div>
                              </div>
                          </fieldset>
                     </div>
					 `
    }

    if (maquinaInoperativa.CodEstadoProceso == 2) {


        if (maquinaInoperativa.CodEstadoReparacion == 1) {


            //ESTADO MAQUINA INOPERATIVA | ATENDIDA INOPERATIVA 1044
            box.innerHTML += `
                     <div class="section-content strategy" id="container-comment3">
                         <h2>ATENDIDA INOPERATIVA</h2>
                          <p><b>Fecha</b>: ${moment(maquinaInoperativa.FechaAtendidaInoperativa).format("DD/MM/YYYY hh:mm a")}</p>
                          <p><b>Usuario</b>: ${maquinaInoperativa.NombreUsuarioAtendidaInoperativa}</p>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px;color:#111111 ">
                              <h3 style="margin-bottom:13px;" padding:0;">DATOS ATENCION 1</h3>
                             <div class="col-md-12 col-sm-12" style="padding:0;">
                                <div class="input-group form-group">
                                    <p><b>Tecnico</b>: ${maquinaInoperativa.TecnicoAtencion}</p>
                                    <p><b>Observaciones</b>: ${maquinaInoperativa.ObservacionAtencion}</p>
                                    <p><b>IST</b>: ${maquinaInoperativa.IST}</p>
                                    <p><b>Orden de compra</b>: <span class="ordenCompraValue">${maquinaInoperativa.OrdenCompra.trim() == '' ? 'No tiene' : maquinaInoperativa.OrdenCompra}</span></p>
                                    <p><b>Fecha Orden de compra</b>:
                                        <span class="fechaordenCompraValue">&nbsp${fechaOrdenCompra}</span>
                                    </p>
                                </div>
                            </div>
                          </fieldset>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">PROBLEMAS REAL</h3>
                              <div class="col-md-12">
                                  <div class="row">
                                      <div class=" col-md-12">
                                          <table id="tableProblemasNuevo" class="table table-striped table-bordered table-hover table-condensed highlight-color-red tableProblemasNuevo">
                                          </table>
                                      </div>
                                  </div>
                              </div>
                          </fieldset>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">CORREOS ENVIADOS</h3>
                              <div class="col-md-12 col-sm-12" style="padding:0;">
                                  <div class="input-group form-group listaCorreosInoperativas">

                                  </div>
                              </div>
                          </fieldset>

                     </div>
					 `
            //ATENDIDA  OPERATIVA 1044
            box.innerHTML += `
                     <div class="section-content analysis" id="container-comment2">
                         <h2>ATENDIDA OPERATIVA</h2>
                          <p><b>Fecha</b>: ${moment(maquinaInoperativa.FechaAtendidaInoperativaAprobado).format("DD/MM/YYYY hh:mm a")}</p>
                          <p><b>Usuario</b>: ${maquinaInoperativa.NombreUsuarioAtendidaInoperativaAprobado}</p>

                       <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; color:#111111;">
                            <h3 style="margin-bottom:13px; padding:0;">DATOS ATENCION REPARACION</h3>
                            <div class="col-md-12 col-sm-12" style="padding:0;">
                                <div class="input-group form-group">
                                    <p><b>Tecnico</b>: ${maquinaInoperativa.NombreUsuarioAtendidaInoperativaAprobado}</p>
                                    <p><b>Observaciones</b>: ${maquinaInoperativa.ObservacionAtencionNuevo}</p>
                                    <p><b>IST</b>: ${maquinaInoperativa.IST}</p>
                                    <p>
                                        <b>Orden de compra</b>:
                                        <span class="ordenCompraValue">&nbsp;${maquinaInoperativa.OrdenCompra.trim() == '' ? 'No tiene' : maquinaInoperativa.OrdenCompra}</span>
                                    </p>
                                    <p>
                                        <b>Fecha Orden de compra</b>:
                                          <span class="fechaordenCompraValue">&nbsp${fechaOrdenCompra}</span>
                                    </p>
                                </div>
                            </div>
                        </fieldset>


                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">CORREOS ENVIADOS</h3>
                              <div class="col-md-12 col-sm-12" style="padding:0;">
                                  <div class="input-group form-group listaCorreosOperativas">

                                  </div>
                              </div>
                          </fieldset>

                     </div>
					 `
        } else {
            //ATENDIDA INOPERATIVA , OPERATIVA 1045
            box.innerHTML += `
                     <div class="section-content analysis" id="container-comment2">
                         <h2>ATENDIDA OPERATIVA</h2>
                          <p><b>Fecha</b>: ${moment(maquinaInoperativa.FechaAtendidaOperativa).format("DD/MM/YYYY hh:mm a")}</p>
                          <p><b>Usuario</b>: ${maquinaInoperativa.NombreUsuarioAtendidaOperativa}</p>

                        <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; color: #111111;">
                            <h3 style="margin-bottom: 13px;">DATOS ATENCION</h3>
                            <div class="col-md-12 col-sm-12" style="padding:0;">
                                <div class="input-group form-group">
                                    <p><b>Tecnico</b>: ${maquinaInoperativa.TecnicoAtencion}</p>
                                    <p><b>Observaciones</b>: ${maquinaInoperativa.ObservacionAtencion}</p>
                                    <p><b>IST</b>: ${maquinaInoperativa.IST}</p>
                                    <p><b>Orden de compra</b>: <span class="ordenCompraValue">${maquinaInoperativa.OrdenCompra.trim() == '' ? 'No tiene' : maquinaInoperativa.OrdenCompra}</span></p>
                                     <p>
                                        <b>Fecha Orden de compra</b>: 
                                          <span class="fechaordenCompraValue">&nbsp${fechaOrdenCompra}</span>
                                    </p>
                                   
                                </div>
                            </div>
                        </fieldset>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">PROBLEMAS REAL</h3>
                              <div class="col-md-12">
                                  <div class="row">
                                      <div class=" col-md-12">
                                          <table id="tableProblemasNuevo" class="table table-striped table-bordered table-hover table-condensed highlight-color-red tableProblemasNuevo">
                                          </table>
                                      </div>
                                  </div>
                              </div>
                          </fieldset>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">CORREOS ENVIADOS</h3>
                              <div class="col-md-12 col-sm-12" style="padding:0;">
                                  <div class="input-group form-group listaCorreosOperativas">

                                  </div>
                              </div>
                          </fieldset>

                     </div>
					 `
        }
    }



    const line = document.createElement("ul");
    line.innerHTML = ``;


    if (maquinaInoperativa.CodEstadoProceso == 1) {
        stepNumber = 1;

        line.innerHTML += `<li class="step step01 active"><div class="step-inner">CREADO</div></li>`
        $(".step").width("100%");
    } else if (maquinaInoperativa.CodEstadoProceso == 3) {


        stepNumber = 3;

        line.innerHTML += `<li class="step step01 active"><div class="step-inner">CREADO</div></li>
                                                <li class="step step02"><div class="step-inner">ATENDIDA INOPERATIVA</div></li>`
        $(".step").width("50%");


    } else if (maquinaInoperativa.CodEstadoProceso == 2) {

        if (maquinaInoperativa.CodEstadoReparacion == 1) {

            stepNumber = 3;

            line.innerHTML += `<li class="step step01 active"><div class="step-inner">CREADO</div></li>
                                                <li class="step step02"><div class="step-inner">ATENDIDA INOPERATIVA</div></li>
                                                <li class="step step03"><div class="step-inner">ATENDIDA OPERATIVA</div></li>`
            $(".step").width("33%");
        } else {

            stepNumber = 2;

            line.innerHTML += `<li class="step step01 active"><div class="step-inner">CREADO</div></li>
                                                <li class="step step02"><div class="step-inner">ATENDIDA OPERATIVA</div></li>`
            $(".step").width("50%");
        }


    }


    const lineWrapper = document.getElementById("progress-bar-container");
    lineWrapper.appendChild(line);



    $(".step").click(function () {
        $(this).addClass("active").prevAll().addClass("active");
        $(this).nextAll().removeClass("active");
    });

    if (maquinaInoperativa.CodEstadoProceso == 1) {
        $(".step").width("100%");
    } else if (maquinaInoperativa.CodEstadoProceso == 3) {

        $(".step").width("50%");

    } else if (maquinaInoperativa.CodEstadoProceso == 2) {

        if (maquinaInoperativa.CodEstadoReparacion == 1) {
            $(".step").width("33%");
        } else {
            $(".step").width("50%");
        }

    }

    //STEPS

    if (maquinaInoperativa.CodEstadoProceso == 1) {
        $(".step01").click(function () {
            $("#line-progress").css("width", "100%");
            $(".discovery").addClass("active").siblings().removeClass("active");
        });
    } else if (maquinaInoperativa.CodEstadoProceso == 3) {

        $(".step01").click(function () {
            $("#line-progress").css("width", "50%");
            $(".discovery").addClass("active").siblings().removeClass("active");
        });

        $(".step02").click(function () {
            $("#line-progress").css("width", "100%");
            $(".strategy").addClass("active").siblings().removeClass("active");
        });

    } else if (maquinaInoperativa.CodEstadoProceso == 2) {

        if (maquinaInoperativa.CodEstadoReparacion == 1) {
            $(".step01").click(function () {
                $("#line-progress").css("width", "3%");
                $(".discovery").addClass("active").siblings().removeClass("active");
            });

            $(".step02").click(function () {
                $("#line-progress").css("width", "50%");
                $(".strategy").addClass("active").siblings().removeClass("active");
            });

            $(".step03").click(function () {
                $("#line-progress").css("width", "100%");
                $(".analysis").addClass("active").siblings().removeClass("active");
            });
        } else {

            $(".step01").click(function () {
                $("#line-progress").css("width", "50%");
                $(".discovery").addClass("active").siblings().removeClass("active");
            });

            $(".step02").click(function () {
                $("#line-progress").css("width", "100%");
                $(".analysis").addClass("active").siblings().removeClass("active");
            });
        }

    }

    var stepCurrent = 0;
    var step = ".step0" + stepNumber;

    if (maquinaInoperativa.CodEstadoProceso == 3) {
        stepCurrent = ".step02";
    } else {
        stepCurrent = ".step0" + stepNumber;
    }

    //console.log(step)
    //console.log(stepCurrent)

    //CHANGE ACTUAL STATE

    const boxWrapper = document.getElementById("progress-content-section");
    boxWrapper.appendChild(box);
    $(stepCurrent).trigger("click");

    //PROBLEMAS
    renderListaProblemas();
    renderListaProblemasNuevo();


    for (var i = 1; i <= stepNumber; i++) {

        const boxa = document.createElement("div");

        boxa.innerHTML = `<hr>`
        boxa.innerHTML += `<!-- Contenedor Principal -->
	<div class="comments-container" id="comments-div${i}">
        <div class="comments-textarea">
            <div class="boxComment" id="boxComment${i}" data-id=${i} contenteditable="true"></div>
            <button class="button-62 btnComentar" role="button" data-id=${i}> Comentar</button >
        </div>
        <div class="usuarios-popup">
            <ul id="usuarios-list${i}"></ul>
        </div>

		<h1>Comentarios</h1>

        <div id="comments-total${i}">
        </div>

	</div>`

        const boxWrappera = document.getElementById("container-comment" + i);
        boxWrappera.appendChild(boxa);
    }


    //$("#editTeset").on('click', function () {

    //    $('#testEdit').attr('contenteditable', true);

    //});

    $(document).on('keypress', '.boxComment', function (event) {

        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);

        var id = $(this).data("id");

        if (key == '@') {

            ListarUsuarios(id);
            $(".usuarios-popup").addClass('usuarios-popup-open');

            textSearch = '';
            addTextSearch = true;

        } else {

            if (addTextSearch) {
                textSearch = textSearch + key;
                arrobaDesaparece = false;
                //console.log(textSearch);
                FiltrarUsuarios(textSearch, id);
            }
        }

    });

    $(document).on('keydown', '.boxComment', function (event) {

        var key = event.keyCode || event.charCode;

        var id = $(this).data("id");

        if (key == 8 || key == 46) {
            if (addTextSearch) {
                textSearch = textSearch.slice(0, textSearch.length - 1);
                if (arrobaDesaparece) {
                    $(".usuarios-popup").removeClass('usuarios-popup-open');
                    textSearch = '';
                    addTextSearch = false;
                    arrobaDesaparece = false;
                }
                if (textSearch.length < 1) {
                    arrobaDesaparece = true;
                }
                //console.log(textSearch);
                FiltrarUsuarios(textSearch, id);
            }
        }

        if (key == 37 || key == 38 || key == 39 || key == 40 || key == 46 || key == 32) {

            $(".usuarios-popup").removeClass('usuarios-popup-open');
            textSearch = '';
            addTextSearch = false;
        }


    });


    //$('#boxComment').blur(function () {
    //    $(".usuarios-popup").removeClass('usuarios-popup-open');
    //});


    $(".btnComentar").on('click', function () {

        var codEstadoProceso = $(this).data("id");
        var contenido = $("#boxComment" + codEstadoProceso).text().trim();
        if (contenido === '') {
            toastr.error("No puede insertar un comentario vacio.", "Mensaje Servidor");
            return;
        }
        AgregarComentario(codEstadoProceso);
        $("#boxComment" + codEstadoProceso).empty();
    });

    //CORREOS

    renderCorreosHtml();


});



function renderListaProblemas() {
    objetodatatable = $(".tableProblemas").DataTable({
        "bDestroy": true,
        "ordering": false,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": false,
        "aaSorting": [],
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "searching": false,
        "bInfo": false, //Dont display info e.g. "Showing 1 to 4 of 4 entries"
        "paging": false,//Dont want paging                
        "bPaginate": false,//Dont want paging      
        data: listaProblemas,
        columns: [
            { data: "NombreProblema", title: "Problema" },
            { data: "DescripcionProblema", title: "Descripcion" },
            {
                data: "FechaRegistro", title: "Fecha",
                "render": function (o) {
                    return moment(o).format("DD/MM/YYYY hh:mm");
                }
            }
        ],
        "drawCallback": function (settings) {
            $('.btnAceptar').tooltip({
                title: "Aceptar"
            });
            $('.btnRechazar').tooltip({
                title: "Rechazar"
            });
        },

        "initComplete": function (settings, json) {

        },
    });
}

function renderListaProblemasNuevo() {
    objetodatatable = $(".tableProblemasNuevo").DataTable({
        "bDestroy": true,
        "ordering": false,
        "scrollCollapse": true,
        "scrollX": true,
        "paging": false,
        "aaSorting": [],
        "autoWidth": false,
        "bProcessing": true,
        "bDeferRender": true,
        "searching": false,
        "bInfo": false, //Dont display info e.g. "Showing 1 to 4 of 4 entries"
        "paging": false,//Dont want paging                
        "bPaginate": false,//Dont want paging      
        data: listaProblemasNuevo,
        columns: [
            { data: "NombreProblema", title: "Problema" },
            { data: "DescripcionProblema", title: "Descripcion" },
            {
                data: "FechaRegistro", title: "Fecha",
                "render": function (o) {
                    return moment(o).format("DD/MM/YYYY hh:mm");
                }
            }
        ],
        "drawCallback": function (settings) {
            $('.btnAceptar').tooltip({
                title: "Aceptar"
            });
            $('.btnRechazar').tooltip({
                title: "Rechazar"
            });
        },

        "initComplete": function (settings, json) {

        },
    });
}
function GenerarExcel(codMaquinaInoperativa) {

    $.ajax({
        type: "POST",
        cache: false,
        data: JSON.stringify({ codMaquinaInoperativa }),
        url: basePath + "MIMaquinaInoperativa/HistoricoxMaquinaInoperativaDescargarExcelJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            if (response.respuesta) {
                var data = response.data;
                var file = response.excelName;
                let a = document.createElement('a');
                a.target = '_self';
                a.href = "data:application/vnd.ms-excel;base64, " + data;
                a.download = file;
                a.click();
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


function ListarUsuarios(id) {

    listaUsuarios = [];

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Empleado/EmpleadoListarPorUsuariosJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
        },

        success: function (response) {
            if (response.respuesta) {
                listaUsuarios = response.data;
                //console.log(listaUsuarios);
                ImprimirUsuarios(listaUsuarios, id);
            }
            else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
        complete: function (resul) {
        }
    });

}


function ImprimirUsuarios(data, id) {

    $("#usuarios-list" + id).empty();
    $.each(data, function (index, value) {
        $('#usuarios-list' + id).append('<li><button class="button-17" role="button" onClick="enviar(this.innerHTML,' + value.EmpleadoID + ',' + id + ');">' + value.NombreCompleto + '</button ></li>');
    });

}


function FiltrarUsuarios(textSearch, id) {

    let listaUsuariosFiltrados = [];
    $.each(listaUsuarios, function (index, value) {
        if (value.NombreCompleto.toUpperCase().indexOf(textSearch.toUpperCase()) != -1) {
            listaUsuariosFiltrados.push(value);
        }
    });

    ImprimirUsuarios(listaUsuariosFiltrados, id);


}

function enviar(valor, value, id) {

    listaUsuariosEtiquetados.push(value);

    let nombreEtiqueta = "#etiqueta" + value;
    //console.log(nombreEtiqueta);

    let textComment = $('#boxComment' + id).html();

    let textos = textComment.split("@" + textSearch);
    $('#boxComment' + id).html(textos[0]);
    $('#boxComment' + id).append('<span id="etiqueta' + value + '" style="color:#2354ED;cursor:pointer;">' + valor + '</span>&nbsp;');
    $('#boxComment' + id).append(textos[1]);

    $(".usuarios-popup").removeClass('usuarios-popup-open');
    textSearch = '';
    addTextSearch = false;

    listaEtiquetas.push("#etiqueta" + value);

    observarCambios(listaEtiquetas);


}

function observarCambios(listaEtiquetas) {
    const observer = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            if (mutation.type === 'characterData') {
                const nodoModificado = mutation.target;
                const idElementoModificado = obtenerIDDelElemento(nodoModificado);

                //console.log(idElementoModificado);
                eliminarElemento(idElementoModificado.slice(8));
                eliminarElementoPorID(idElementoModificado);
                // Aquí puedes realizar acciones adicionales en respuesta al cambio de texto
            }
        });
    });

    const config = { characterData: true, subtree: true };

    listaEtiquetas.forEach(function (id) {
        observer.observe(document.getElementById(id.substring(1)), config);
    });
}

// Función para obtener el ID del elemento ascendiendo en el árbol DOM
function obtenerIDDelElemento(elemento) {
    while (elemento) {
        if (elemento.id) {
            return elemento.id;
        }
        elemento = elemento.parentNode;
    }
    return null;
}

function eliminarElementoPorID(id) {
    $('#' + id).remove();
}


function eliminarElemento(valor) {

    //console.log(listaEtiquetas);
    //console.log(listaUsuariosEtiquetados);

    listaEtiquetas = $.grep(listaEtiquetas, function (elemento) {
        return !elemento.includes(valor);
    });

    listaUsuariosEtiquetados = $.grep(listaUsuariosEtiquetados, function (elemento) {
        return !elemento.toString().includes(valor);
    });

    //console.log(listaEtiquetas);
    //console.log(listaUsuariosEtiquetados);

}




function AgregarComentario(estadoProceso) {

    codMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
    texto = $('#boxComment' + estadoProceso).html();
    listaEmpleados = listaUsuariosEtiquetados;

    $.ajax({
        type: "POST",
        cache: false,
        data: JSON.stringify({ codMaquinaInoperativa, estadoProceso, texto, listaEmpleados }),
        url: basePath + "MIMaquinaInoperativa/AgregarComentario",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            if (response.respuesta) {
                toastr.success(response.mensaje, "Mensaje Servidor");
                CargarComentarios();
                listaUsuariosEtiquetados = [];
                listaEtiquetas = [];
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


function BorrarComentario(codComentario) {


    $.ajax({
        type: "POST",
        cache: false,
        data: JSON.stringify({ codComentario }),
        url: basePath + "MIMaquinaInoperativa/EliminarComentario",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            if (response.respuesta) {
                toastr.success(response.mensaje, "Mensaje Servidor");
                CargarComentarios();
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

function CargarComentarios() {


    codMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;

    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "MIMaquinaInoperativa/GetAllComentariosxMaquina",
        data: JSON.stringify({ codMaquinaInoperativa }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            // $.LoadingOverlay("show");
        },

        success: function (response) {
            if (response.respuesta) {
                ImprimirComentarios(response.data);
                //console.log(response.data);
            }
            //console.log(response.mensaje);
        },
        complete: function (resul) {
            //$.LoadingOverlay("hide");
        }
    });

}


//line.innerHTML = `
//            <hr>
//            <ul id="comments-list" class="comments-list">
//                <li>
//                    <div class="comment-main-level">
//                        <!-- Avatar -->
//                        <div class="comment-avatar"><img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSjZnwvLvfHXCYbSxQ5Zy5DPqWOHQ3t6VZyhw&usqp=CAU" alt=""></div>
//                        <!-- Contenedor del Comentario -->
//                        <div class="comment-box">
//                            <div class="comment-head">
//                                <i class="fa fa-pencil"></i>
//                                <h6 class="comment-name">Agustin Ortiz</h6>
//                                <span>hace 20 minutos</span>
//                            </div>
//                            <div class="comment-content">
//                                Hola wapa
//                            </div>
//                        </div>
//                    </div>
//                    <!-- Respuestas de los comentarios
//                    <ul class="comments-list reply-list">
//                        <li>
//                            <div class="comment-avatar"><img src="http://i9.photobucket.com/albums/a88/creaticode/avatar_2_zps7de12f8b.jpg" alt=""></div>
//                            <div class="comment-box">
//                                <div class="comment-head">
//                                    <h6 class="comment-name"><a href="http://creaticode.com/blog">Lorena Rojero</a></h6>
//                                    <span>hace 10 minutos</span>
//                                    <i class="fa fa-reply"></i>
//                                    <i class="fa fa-heart"></i>
//                                </div>
//                                <div class="comment-content">
//                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit. Velit omnis animi et iure laudantium vitae, praesentium optio, sapiente distinctio illo?
//                                </div>
//                            </div>
//                        </li>

//                        <li>
//                            <div class="comment-avatar"><img src="http://i9.photobucket.com/albums/a88/creaticode/avatar_1_zps8e1c80cd.jpg" alt=""></div>
//                            <div class="comment-box">
//                                <div class="comment-head">
//                                    <h6 class="comment-name by-author"><a href="http://creaticode.com/blog">Agustin Ortiz</a></h6>
//                                    <span>hace 10 minutos</span>
//                                    <i class="fa fa-reply"></i>
//                                    <i class="fa fa-heart"></i>
//                                </div>
//                                <div class="comment-content">
//                                    Lorem ipsum dolor sit amet, consectetur adipisicing elit. Velit omnis animi et iure laudantium vitae, praesentium optio, sapiente distinctio illo?
//                                </div>
//                            </div>
//                        </li>
//                    </ul> -->
//                </li>
//            </ul>`


function ImprimirComentarios(dataTotal) {




    for (var i = 1; i <= stepNumber; i++) {




        //FILTRO POR ESTADO


        const data = $.grep(dataTotal, function (objeto) {
            return objeto.EstadoProceso === i;
        });


        const divComments = document.getElementById("comments-total" + i);

        divComments.innerHTML = `
    `;

        const line = document.createElement("div");

        line.innerHTML = `
    `;

        if (data.length > 0) {

            line.innerHTML += `
            <hr>
            <ul id="comments-list${i}" class="comments-list">
            </ul>
                `

            divComments.appendChild(line);




            const divax = document.getElementById("comments-list" + i);


            data.forEach(function (elemento, index) {

                const linea = document.createElement("li");

                linea.innerHTML = `

                    <div class="comment-main-level">
                        <!-- Avatar -->
                        <div class="comment-avatar"><img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSjZnwvLvfHXCYbSxQ5Zy5DPqWOHQ3t6VZyhw&usqp=CAU" alt=""></div>
                        <!-- Contenedor del Comentario -->
                        <div class="comment-box">
                            <div class="comment-head">
                                <i class="fa fa-trash borrar-comentario" data-id=${elemento.CodComentario}></i>
                                <h6 class="comment-name">${elemento.NombreCompleto}</h6>
                                <span>${moment(elemento.FechaRegistro).format("DD/MM/YYYY hh:mm:ss a")}</span>
                            </div>
                            <div class="comment-content">
                                ${elemento.Texto}
                            </div>
                        </div>
                    </div>

                `

                divax.appendChild(linea);

            });


        } else {

            line.innerHTML += `
        <hr>
		<h4>Sin comentarios</h4>
`;
            divComments.appendChild(line);
        }

        if (cargaPrimeraVez) {

            if (estadoActual != "0") {
                $(".step0" + estadoActual).click();

                $('html, body').animate({
                    scrollTop: $(document).height()
                }, 1000);
                cargaPrimeraVez = false;
            }
        }

    }


    $(".borrar-comentario").click(function () {
        var cod = $(this).data("id");
        $.confirm({
            title: '¿Esta seguro de Continuar?!',
            content: '¿Borrar comentario?',
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-success',
            cancelButtonClass: 'btn-danger',
            confirm: function () {
                BorrarComentario(cod);

            },
            cancel: function () {
                //close
            },

        });
    });


}




const obtenerCorreos = () => {

    return $.ajax({
        type: "POST",
        url: `${basePath}/MIMaquinaInoperativa/ListarUsuarioCorreos`,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        success: function (response) {
            return response
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })


}

function renderCorreos(data) {

    var element = "#cboCorreos"
    data.forEach(item => {
        $(element).append(`<option value="${item.CodUsuario}">${item.Mail} (${item.UsuarioNombre})</option>`)
    })

    jQuery(element).multiselect({
        multiple: true,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        includeSelectAllOption: true,
        maxHeight: 400,
        buttonContainer: '<div></div>',
        buttonClass: '',
        templates: {
            button: '<div class="form-control form-multiselect input-sm multiselect" data-toggle="dropdown"><span class="multiselect-selected-text"></span></div>'
        },
        nonSelectedText: '--Seleccione--',
        nSelectedText: 'seleccionados',
        allSelectedText: 'Todo seleccionado',
        selectAllText: 'Seleccionar todos'
    })
}


function AgregarOrdenCompra() {


    let codMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
    let ordenCompra = $("#ordenCompra").val();
    let fechaOrdenCompra = $("#fechaHoraOrdenCompra").val()
    fechaOrdenCompra = moment(fechaOrdenCompra).format("DD/MM/YYYY")
    console.log(fechaOrdenCompra, 'primero')
    if (ordenCompra.trim() === "") {

        toastr.error("La orden de compra no puede estar vacia", "Mensaje Servidor");
        return false;
    }

    if (!fechaOrdenCompra) {
        toastr.error("Debe seleccionar una fecha de orden de compra", "Mensaje Servidor");
        return false;
    }

    $.ajax({
        type: "POST",
        cache: false,
        data: JSON.stringify({ codMaquinaInoperativa, ordenCompra, fechaOrdenCompra }),
        url: basePath + "MIMaquinaInoperativa/AgregarOrdenCompra",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            if (response.respuesta) {
                toastr.success(response.mensaje, "Mensaje Servidor");

                $("#modalOrdenCompra").modal("hide");
                $('.ordenCompraValue').text(ordenCompra)
                $('.fechaordenCompraValue').text(fechaOrdenCompra);
                console.log(fechaOrdenCompra, 'segundo')
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


function ReenviarCorreos() {

    let codMaquinaInoperativa = maquinaInoperativa.CodMaquinaInoperativa;
    let listaCorreosAct = $("#cboCorreos").val();

    if (!listaCorreosAct) {

        toastr.error("Selecciona minimo un correo para reenviar", "Mensaje Servidor");
        return false;
    }

    $.ajax({
        type: "POST",
        cache: false,
        data: JSON.stringify({ codMaquinaInoperativa, listaCorreos: listaCorreosAct }),
        url: basePath + "MaquinasInoperativasV2/ReenviarCorreos",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            if (response.respuesta) {
                toastr.success(response.mensaje, "Mensaje Servidor");

                listaCorreos = response.data;

                renderCorreosHtml();
                $("#modalCorreos").modal("hide");
                //$('.ordenCompraValue').text(ordenCompra)
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

function renderCorreosHtml() {


    var listaCorreos1 = listaCorreos.filter(x => x.CodEstadoProceso === 1);
    var listaCorreos2 = listaCorreos.filter(x => x.CodEstadoProceso === 2 || x.CodEstadoProceso === 4);
    var listaCorreos3 = listaCorreos.filter(x => x.CodEstadoProceso === 3);

    var ul1 = $(".listaCorreosCreados");
    ul1.empty();
    //ul1.append("<p><b>Usuario - Correo</b></p>")
    if (listaCorreos1.length > 0) {
        $.each(listaCorreos1, function (idx, elem) {
            ul1.append("<p>" + elem.UsuarioNombre + " - " + elem.UsuarioMail + "</p>")
        })
    } else {
        ul1.append("<p>Sin correos.</p>")
    }

    var ul2 = $(".listaCorreosOperativas");
    ul2.empty();
    //ul2.append("<p><b>Usuario - Correo</b></p>")
    if (listaCorreos2.length > 0) {
        $.each(listaCorreos2, function (idx, elem) {
            ul2.append("<p>" + elem.UsuarioNombre + " - " + elem.UsuarioMail + "</p>")
        })
    } else {
        ul2.append("<p>Sin correos.</p>")
    }

    var ul3 = $(".listaCorreosInoperativas");
    ul3.empty();
    //ul3.append("<p><b>Usuario - Correo</b></p>")
    if (listaCorreos3.length > 0) {
        $.each(listaCorreos3, function (idx, elem) {
            ul3.append("<p>" + elem.UsuarioNombre + " - " + elem.UsuarioMail + "</p>")
        })
    } else {
        ul3.append("<p>Sin correos.</p>")
    }

}

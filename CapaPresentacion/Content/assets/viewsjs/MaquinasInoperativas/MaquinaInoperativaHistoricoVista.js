
let listaUsuarios = [];
let textSearch = '';
let addTextSearch = false;
let arrobaDesaparece = false;
let listaUsuariosEtiquetados = [];
let listaEtiquetas = [];
let cargaPrimeraVez = true;

$(document).ready(function () {

    VistaAuditoria("MIMaquinaInoperativa/HistoricoMaquinaInoperativa", "VISTA", 0, "", 3);
    
    $(document).on("click", ".btnListar", function () {

        window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativa");

    });

    $(document).on("click", "#btnExcel", function () {

        GenerarExcel(maquinaInoperativa.CodMaquinaInoperativa);

    });

    if (maquinaInoperativa.CodMaquinaInoperativa == 0) {
        window.location.replace(basePath + "MIMaquinaInoperativa/ListadoMaquinaInoperativa");
    }

    CargarComentarios();

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


    console.log(maquinaInoperativa);
    //console.log(listaProblemas);
    //console.log(listaPiezas);
    //console.log(listaRepuestos);
    console.log(listaTraspasos);
    //console.log(listaCompras);

    //SET DATA HTML


    const box = document.createElement("div");
    box.innerHTML = ``;


    let arrayProblemas = listaProblemas.map(item => { return `<p>${item.NombreProblema}</p>` })
    let arrayPiezas = listaPiezas.map(item => { return `<p>${item.NombrePieza}</p>` })
    let arrayRepuestos = listaRepuestos.map(item => { return `<p>${item.NombreRepuesto}</p>` })
    let estadoInoperativa = maquinaInoperativa.CodEstadoInoperativa == 1 ? "Op. Problemas" : (maquinaInoperativa.CodEstadoInoperativa == 2 ? "Inoperativa" : "Atendida en Sala")
    let codPrioridad = maquinaInoperativa.CodEstadoInoperativa == 1 ? "Baja" : (maquinaInoperativa.CodEstadoInoperativa == 2 ? "Media" : "Alta")


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
									  <p><b>Juego</b>: ${maquinaInoperativa.MaquinaJuego}</p>
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
									  <p><b>Tecnico</b>: ${maquinaInoperativa.TecnicoCreado}</p>
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
                              <h3 style="margin-bottom:13px;" padding:0;">PIEZAS</h3>
                              <div class="col-md-12">
                                  <div class="row">
                                      <div class=" col-md-12">
                                          <table id="tablePiezas" class="table table-striped table-bordered table-hover table-condensed highlight-color-red tablePiezas">
                                          </table>
                                      </div>
                                  </div>
                              </div>
                          </fieldset>

                     </div>
					 `

    } else {

        box.innerHTML += `
                     <div class="section-content discovery active" id="container-comment1">
                          <h2>CREADO</h2>
                          <p>Sin información.</p>
                     </div>
					 `
    }

    if (maquinaInoperativa.CodEstadoProceso <= 5 && (listaTraspasos.length >= 0)) {

        box.innerHTML += `
                     <div class="section-content strategy" id="container-comment2">
                         <h2>ATENDIDA INOPERATIVA</h2>
                          <p><b>Fecha</b>: ${moment(maquinaInoperativa.FechaAtendidaInoperativa).format("DD/MM/YYYY hh:mm a")}</p>
                          <p><b>Usuario</b>: ${maquinaInoperativa.NombreUsuarioAtendidaInoperativa}</p>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">DATOS GENERALES</h3>
                              <div class="col-md-12 col-sm-12" style="padding:0;">
                                  <div class="input-group form-group ">
									  <p><b>Tecnico</b>: ${maquinaInoperativa.TecnicoAtencion}</p>
									  <p><b>Observaciones</b>: ${maquinaInoperativa.ObservacionAtencion}</p>
                                  </div>
                              </div>
                          </fieldset>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">PIEZAS</h3>
                              <div class="col-md-12">
                                  <div class="row">
                                      <div class=" col-md-12">
                                          <table id="tablePiezas" class="table table-striped table-bordered table-hover table-condensed highlight-color-red tablePiezas">
                                          </table>
                                      </div>
                                  </div>
                              </div>
                          </fieldset>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">REPUESTOS</h3>
                              <div class="col-md-12">
                                  <div class="row">
                                      <div class=" col-md-12">
                                          <table id="tableRepuestos" class="table table-striped table-bordered table-hover table-condensed highlight-color-red tableRepuestos">
                                          </table>
                                      </div>
                                  </div>
                              </div>
                          </fieldset>

                     </div>
					 `

    } else {

        box.innerHTML += `
				     <div class="section-content strategy" id="container-comment2">
                         <h2>ATENDIDA INOPERATIVA</h2>
                          <p>Sin información.</p>
                     </div >
					 `
    }


    if (maquinaInoperativa.CodEstadoProceso <= 5 && (listaTraspasos.length > 0)) {

        box.innerHTML += `
                     <div class="section-content creative" id="container-comment3">
                         <h2>ATENCION REVISADA</h2>
                          <p><b>Fecha</b>: ${moment(maquinaInoperativa.FechaAtendidaInoperativaSolicitado).format("DD/MM/YYYY hh:mm a")}</p>
                          <p><b>Usuario</b>: ${maquinaInoperativa.NombreUsuarioAtendidaInoperativaSolicitado}</p>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">REPUESTOS</h3>
                              <div class="col-md-12">
                                  <div class="row">
                                      <div class=" col-md-12">
                                          <table id="tableRepuestos" class="table table-striped table-bordered table-hover table-condensed highlight-color-red tableRepuestos">
                                          </table>
                                      </div>
                                  </div>
                              </div>
                          </fieldset>

                     </div>
					 `

    } else {

        box.innerHTML += `
                     <div class="section-content creative" id="container-comment3">
                         <h2>ATENCION REVISADA</h2>
                          <p>Sin información.</p>
                     </div>
					 `
    }


    if (maquinaInoperativa.CodEstadoProceso <= 5 && (listaTraspasos.length > 0 || listaCompras.length > 0)) {

        box.innerHTML += `
                     <div class="section-content production" id="container-comment4">
                         <h2>SOLICITUDES</h2>
                          <p><b>Fecha</b>: ${moment(maquinaInoperativa.FechaAtendidaInoperativaAprobado).format("DD/MM/YYYY hh:mm a")}</p>
                          <p><b>Usuario</b>: ${maquinaInoperativa.NombreUsuarioAtendidaInoperativaAprobado}</p>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">TRASPASOS</h3>
                              <div class="col-md-12">
                                  <div class="row">
                                      <div class=" col-md-12">
                                          <table id="tableTraspasos" class="table table-striped table-bordered table-hover table-condensed highlight-color-red">
                                          </table>
                                      </div>
                                  </div>
                              </div>
                          </fieldset>

                     </div>
					 `

    } else {

        box.innerHTML += `
                     <div class="section-content production" id="container-comment4">
                         <h2>SOLICITUDES</h2>
                          <p>Sin información.</p>
                     </div>
					 `
    }

    if (maquinaInoperativa.CodEstadoProceso == 2 && (listaTraspasos.length > 0 || listaCompras.length > 0)) {

        box.innerHTML += `
                     <div class="section-content analysis" id="container-comment5">
                         <h2>ATENDIDA OPERATIVA</h2>
                          <p><b>Fecha</b>: ${moment(maquinaInoperativa.FechaAtendidaOperativa).format("DD/MM/YYYY hh:mm a")}</p>
                          <p><b>Usuario</b>: ${maquinaInoperativa.NombreUsuarioAtendidaOperativa}</p>
                          <p>MAQUINA OPERATIVA TERMINADA.</p>
                     </div>
					 `

    } else if (maquinaInoperativa.CodEstadoProceso == 2 && (listaTraspasos.length == 0 && listaCompras.length == 0)) {

        box.innerHTML += `
                     <div class="section-content analysis" id="container-comment5">
                         <h2>ATENDIDA OPERATIVA</h2>
                          <p><b>Fecha</b>: ${moment(maquinaInoperativa.FechaAtendidaOperativa).format("DD/MM/YYYY hh:mm a")}</p>
                          <p><b>Usuario</b>: ${maquinaInoperativa.NombreUsuarioAtendidaOperativa}</p>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px;color:#111111 ">
                              <h3 style="margin-bottom:13px;" padding:0;">DATOS GENERALES</h3>
                              <div class="col-md-12 col-sm-12" style="padding:0;">
                                  <div class="input-group form-group ">
									  <p><b>Tecnico</b>: ${maquinaInoperativa.TecnicoAtencion}</p>
									  <p><b>Observaciones</b>: ${maquinaInoperativa.ObservacionAtencion}</p>
                                  </div>
                              </div>
                          </fieldset>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">PIEZAS</h3>
                              <div class="col-md-12">
                                  <div class="row">
                                      <div class=" col-md-12">
                                          <table id="tablePiezas" class="table table-striped table-bordered table-hover table-condensed highlight-color-red tablePiezas">
                                          </table>
                                      </div>
                                  </div>
                              </div>
                          </fieldset>

                          <fieldset class="sectionCard" style="padding: 10px 7px; margin-top: 10px; ">
                              <h3 style="margin-bottom:13px;" padding:0;">REPUESTOS</h3>
                              <div class="col-md-12">
                                  <div class="row">
                                      <div class=" col-md-12">
                                          <table id="tableRepuestos" class="table table-striped table-bordered table-hover table-condensed highlight-color-red tableRepuestos">
                                          </table>
                                      </div>
                                  </div>
                              </div>
                          </fieldset>
                     </div>
					 `
    } else {

        box.innerHTML += `
                     <div class="section-content analysis" id="container-comment5">
                         <h2>ATENDIDA OPERATIVA</h2>
                          <p>Sin información.</p>
                     </div>
					 `
    }

    var stepNumber = 0;


    //STEPS

    //$(".step").click(function () {
    //    $(this).addClass("active").prevAll().addClass("active");
    //    $(this).nextAll().removeClass("active");
    //});

    //$(".step01").click(function () {
    //    $("#line-progress").css("width", "3%");
    //    $(".discovery").addClass("active").siblings().removeClass("active");
    //});

    //$(".step02").click(function () {
    //    $("#line-progress").css("width", "25%");
    //    $(".strategy").addClass("active").siblings().removeClass("active");
    //});

    //$(".step03").click(function () {
    //    $("#line-progress").css("width", "50%");
    //    $(".creative").addClass("active").siblings().removeClass("active");
    //});

    //$(".step04").click(function () {
    //    $("#line-progress").css("width", "75%");
    //    $(".production").addClass("active").siblings().removeClass("active");
    //});

    //$(".step05").click(function () {
    //    $("#line-progress").css("width", "100%");
    //    $(".analysis").addClass("active").siblings().removeClass("active");
    //});



    const line = document.createElement("ul");
    line.innerHTML = ``;


    if (maquinaInoperativa.CodEstadoProceso == 1) {
        stepNumber = 1;

        line.innerHTML += `<li class="step step01 active"><div class="step-inner">CREADO</div></li>`
        $(".step").width("100%");
    } else if (maquinaInoperativa.CodEstadoProceso == 2) {

        if (listaTraspasos.length == 0 && listaCompras.length == 0) {

            stepNumber = 2;

            line.innerHTML += `<li class="step step01 active"><div class="step-inner">CREADO</div></li>
                                                <li class="step step02"><div class="step-inner">ATENDIDA OPERATIVA</div></li>`
            $(".step").width("50%");
        } else {

            stepNumber = 5;

            line.innerHTML += `<li class="step step01 active"><div class="step-inner">CREADO</div></li>
                                                <li class="step step02"><div class="step-inner">ATENDIDA INOPERATIVA</div></li>
                                                <li class="step step03"><div class="step-inner">ATENCION REVISADA</div></li>
                                                <li class="step step04"><div class="step-inner">SOLICITUDES</div></li>
                                                <li class="step step05"><div class="step-inner">ATENDIDA OPERATIVA</div></li>`
            $(".step").width("20%");
        }

    } else {
        stepNumber = maquinaInoperativa.CodEstadoProceso;

        if (stepNumber == 3) {

            line.innerHTML += `<li class="step step01 active"><div class="step-inner">CREADO</div></li>
                                                <li class="step step02"><div class="step-inner">ATENDIDA INOPERATIVA</div></li>`
            $(".step").width("50%");
        } else if (stepNumber == 4) {


            line.innerHTML += `<li class="step step01 active"><div class="step-inner">CREADO</div></li>
                                                <li class="step step02"><div class="step-inner">ATENDIDA INOPERATIVA</div></li>
                                                <li class="step step03"><div class="step-inner">ATENCION REVISADA</div></li>`
            $(".step").width("33%");
        } else if (stepNumber == 5) {

            line.innerHTML += `<li class="step step01 active"><div class="step-inner">CREADO</div></li>
                                                <li class="step step02"><div class="step-inner">ATENDIDA INOPERATIVA</div></li>
                                                <li class="step step03"><div class="step-inner">ATENCION REVISADA</div></li>
                                                <li class="step step04"><div class="step-inner">SOLICITUDES</div></li>`
            $(".step").width("25%");

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
    } else if (maquinaInoperativa.CodEstadoProceso == 2) {

        if (listaTraspasos.length == 0 && listaCompras.length == 0) {

            stepNumber = 2;
            $(".step").width("50%");
        } else {

            stepNumber = 5;
            $(".step").width("20%");
        }
    } else {
        stepNumber = maquinaInoperativa.CodEstadoProceso;

        if (stepNumber == 3) {
            $(".step").width("50%");
        } else if (stepNumber == 4) {
            $(".step").width("33%");
        } else if (stepNumber == 5) {
            $(".step").width("25%");

        }

    }

	if (maquinaInoperativa.CodEstadoProceso == 1) {
        stepNumber = 1;
        $(".step01").click(function () {
            $("#line-progress").css("width", "100%");
            $(".discovery").addClass("active").siblings().removeClass("active");
        });

	} else if (maquinaInoperativa.CodEstadoProceso == 2) {


        if (listaTraspasos.length == 0 && listaCompras.length == 0) {

            stepNumber = 2;
            $(".step01").click(function () {
                $("#line-progress").css("width", "50%");
                $(".discovery").addClass("active").siblings().removeClass("active");
            });

            $(".step02").click(function () {
                $("#line-progress").css("width", "100%");
                $(".analysis").addClass("active").siblings().removeClass("active");
            });
        } else {

            stepNumber = 5;

            $(".step01").click(function () {
                $("#line-progress").css("width", "3%");
                $(".discovery").addClass("active").siblings().removeClass("active");
            });

            $(".step02").click(function () {
                $("#line-progress").css("width", "25%");
                $(".strategy").addClass("active").siblings().removeClass("active");
            });

            $(".step03").click(function () {
                $("#line-progress").css("width", "50%");
                $(".creative").addClass("active").siblings().removeClass("active");
            });

            $(".step04").click(function () {
                $("#line-progress").css("width", "75%");
                $(".production").addClass("active").siblings().removeClass("active");
            });

            $(".step05").click(function () {
                $("#line-progress").css("width", "100%");
                $(".analysis").addClass("active").siblings().removeClass("active");
            });
        }

    } else {
        stepNumber = maquinaInoperativa.CodEstadoProceso;

        if (stepNumber == 3) {
            $(".step01").click(function () {
                $("#line-progress").css("width", "50%");
                $(".discovery").addClass("active").siblings().removeClass("active");
            });

            $(".step02").click(function () {
                $("#line-progress").css("width", "100%");
                $(".strategy").addClass("active").siblings().removeClass("active");
            });

        } else if (stepNumber == 4) {

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
                $(".creative").addClass("active").siblings().removeClass("active");
            });

        } else if (stepNumber == 5) {
            $(".step01").click(function () {
                $("#line-progress").css("width", "3%");
                $(".discovery").addClass("active").siblings().removeClass("active");
            });

            $(".step02").click(function () {
                $("#line-progress").css("width", "35%");
                $(".strategy").addClass("active").siblings().removeClass("active");
            });

            $(".step03").click(function () {
                $("#line-progress").css("width", "65%");
                $(".creative").addClass("active").siblings().removeClass("active");
            });

            $(".step04").click(function () {
                $("#line-progress").css("width", "100%");
                $(".production").addClass("active").siblings().removeClass("active");
            });


        }

	}



    //STEPS

    //$(".step").click(function () {
    //    $(this).addClass("active").prevAll().addClass("active");
    //    $(this).nextAll().removeClass("active");
    //});

    //$(".step01").click(function () {
    //    $("#line-progress").css("width", "3%");
    //    $(".discovery").addClass("active").siblings().removeClass("active");
    //});

    //$(".step02").click(function () {
    //    $("#line-progress").css("width", "25%");
    //    $(".strategy").addClass("active").siblings().removeClass("active");
    //});

    //$(".step03").click(function () {
    //    $("#line-progress").css("width", "50%");
    //    $(".creative").addClass("active").siblings().removeClass("active");
    //});

    //$(".step04").click(function () {
    //    $("#line-progress").css("width", "75%");
    //    $(".production").addClass("active").siblings().removeClass("active");
    //});

    //$(".step05").click(function () {
    //    $("#line-progress").css("width", "100%");
    //    $(".analysis").addClass("active").siblings().removeClass("active");
    //});
    if (stepNumber >= 3 && maquinaInoperativa.CodEstadoProceso!=2) {
        stepNumber--;
    }

    var step = ".step0" + stepNumber;

	console.log(step)

	//CHANGE ACTUAL STATE

	const boxWrapper = document.getElementById("progress-content-section");
	boxWrapper.appendChild(box);
    $(step).trigger("click");

    //PROBLEMAS
    renderListaProblemas();

    //PIEZAS
    renderListaPiezas();

    //REPUESTOS
    renderListaRepuestos();

    //TRASPASOS
    renderListaTraspasos();   

    //COMPRAS
    //renderListaCompras();

    for (var i = 1; i <= 5; i++) {

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

        const boxWrappera = document.getElementById("container-comment"+i);
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
                console.log(textSearch);
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
                console.log(textSearch);
                FiltrarUsuarios(textSearch,id);
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

function renderListaPiezas() {
    objetodatatable = $(".tablePiezas").DataTable({
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
        data: listaPiezas,
        columns: [
            { data: "NombrePieza", title: "Pieza" },
            { data: "Cantidad", title: "Cantidad" },
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

function renderListaRepuestos() {
    objetodatatable = $(".tableRepuestos").DataTable({
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
        data: listaRepuestos,
        columns: [
            { data: "NombreRepuesto", title: "Nombre" },
            { data: "Cantidad", title: "Cantidad" },
            {
                data: "Estado", title: "Estado",
                "bSortable": false,
                "render": function (o, type, oData) {


                    if (o == 0) {

                        estado = "EN STOCK"
                        css = "btn-primary";
                        return '<span class="label ' + css + '">' + estado + '</span>';

                    } else

                        if (o == 2) {

                            estado = "ACEPTADO"
                            css = "btn-success";
                            return '<span class="label ' + css + '">' + estado + '</span>';

                        } else
                            if (o == 3) {

                                estado = "RECHAZADO"
                                css = "btn-danger";
                                return '<span class="label ' + css + '">' + estado + '</span>';

                            } else {

                                estado = "PEDIDO"
                                css = "btn-warning";
                                return '<span class="label ' + css + '">' + estado + '</span>';

                            }
                }
            },
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

function renderListaTraspasos() {
    objetodatatable = $("#tableTraspasos").DataTable({
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
        data: listaTraspasos,
        columns: [
            { data: "NombreSala", title: "Sala" },
            { data: "NombreAlmacenOrigen", title: "Almacen Origen" },
            { data: "NombreAlmacenDestino", title: "Almacen Destino" },
            { data: "NombreRepuesto", title: "Repuesto" },
            { data: "Cantidad", title: "Cantidad" },
            {
                data: "Estado", title: "Estado",
                "render": function (o) {
                    console.log(o);
                    var estado = "PENDIENTE";
                    var css = "btn-info";
                    if (o == 2) {
                        estado = "RECHAZADO"
                        css = "btn-danger";
                    }
                    if (o == 1) {
                        estado = "ACEPTADO"
                        css = "btn-success";
                    }
                    return '<span class="label ' + css + '">' + estado + '</span>';

                }
            },
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

function renderListaCompras() {
    objetodatatable = $("#tableCompras").DataTable({
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
        data: listaCompras,
        columns: [
            { data: "NombreSala", title: "Sala" },
            { data: "NombreAlmacenDestino", title: "Almacen Destino" },
            { data: "NombreRepuesto", title: "Repuesto" },
            { data: "Cantidad", title: "Cantidad" },
            {
                data: "Estado", title: "Estado",
                "render": function (o) {
                    var estado = "PENDIENTE";
                    var css = "btn-info";
                    if (o == 2) {
                        estado = "RECHAZADO"
                        css = "btn-danger";
                    }
                    if (o == 1) {
                        estado = "ACEPTADO"
                        css = "btn-success";
                    }
                    return '<span class="label ' + css + '">' + estado + '</span>';

                }
            },
            {
                data: "FechaRegistro", title: "Fecha Registro",
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
                ImprimirUsuarios(listaUsuarios,id);
            }
            else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
        complete: function (resul) {
        }
    });

}


function ImprimirUsuarios(data,id) {

    $("#usuarios-list"+id).empty();
    $.each(data, function (index, value) {
        $('#usuarios-list'+id).append('<li><button class="button-17" role="button" onClick="enviar(this.innerHTML,' + value.EmpleadoID + ','+id+');">' + value.NombreCompleto + '</button ></li>');
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

function enviar(valor,value,id) {

    //console.log(value);

    listaUsuariosEtiquetados.push(value);

    let nombreEtiqueta = "#etiqueta" + value;
    //console.log(nombreEtiqueta);

    let textComment = $('#boxComment'+id).html();

    let textFinalComment = textComment.slice(0, textComment.length - textSearch.length - 1);

    //console.log(textFinalComment+ valor.trim());

    //let textFinal = textFinalComment+ valor.trim();

    $('#boxComment' + id).html(textFinalComment);

    $('#boxComment' + id).append('<span id="etiqueta' + value + '" style="color:#2354ED;cursor:pointer;">@' + valor + '</span>&nbsp;')

    $(".usuarios-popup").removeClass('usuarios-popup-open');
    textSearch = '';
    addTextSearch = false;

    enfocarAlFinal(id);

    listaEtiquetas.push("#etiqueta" + value);

    observarCambios(listaEtiquetas);


}

function enfocarAlFinal(id) {
    // Obtener el último nodo de texto dentro del div
    var ultimoNodo = $('#boxComment'+id).contents().last();

    // Crear un rango de selección
    var rango = document.createRange();
    rango.setStart(ultimoNodo[0], ultimoNodo[0].textContent.length);
    rango.setEnd(ultimoNodo[0], ultimoNodo[0].textContent.length);

    // Crear un objeto de selección y aplicar el rango
    var seleccion = window.getSelection();
    seleccion.removeAllRanges();
    seleccion.addRange(rango);

    // Enfocar el div editable
    $('#boxComment'+id).focus();
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

    console.log(listaEtiquetas);
    console.log(listaUsuariosEtiquetados);

    listaEtiquetas = $.grep(listaEtiquetas, function (elemento) {
        return !elemento.includes(valor);
    });

    listaUsuariosEtiquetados = $.grep(listaUsuariosEtiquetados, function (elemento) {
        return !elemento.toString().includes(valor);
    });

    console.log(listaEtiquetas);
    console.log(listaUsuariosEtiquetados);

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
        data: JSON.stringify({ codComentario}),
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
                console.log(response.data);
            }
            console.log(response.mensaje);
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




    for (var i = 1; i <= 5; i++) {




        //FILTRO POR ESTADO


        const data = $.grep(dataTotal, function (objeto) {
            return objeto.EstadoProceso === i;
        });


        const divComments = document.getElementById("comments-total"+i);

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




            const divax = document.getElementById("comments-list"+i);


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
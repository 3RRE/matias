
var editor1 = $("#tableSala").DataTable();
var editor2 = $("#tableSala2").DataTable();
empleadodatatable = "";
var ipPrivada = "";
var primeraIpPrivada = true;
var primeraCargaVista = true;
var textsIpsPublicas = new Array();
var textsIpsPrivadas = new Array();
var intervalPublica = null
var intervalPrivada = null;


function afterTableInitialization(settings, json) {
    tableAHORA = settings.oInstance.api();
}


function ListarSalasIpPublica() {
    console.log("Intentando conseguir Ips Publicas");
    var url = basePath + "Sala/ListadoPingIpPublica";
    var data = {}; var respuesta = ""; aaaa = "";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: function () {
            editor1.clear().draw();
            $.LoadingOverlay("show");
            overlayIpsPublicas();
        },
        complete: function () {
            overlayLimpiar();
            $.LoadingOverlay("hide");
            console.log("Terminado conseguir Ips Publicas");
            if (primeraCargaVista) {
                ListarSalasIpPrivada(ipPrivada);
                primeraCargaVista = false;
            }
        },
        success: function (response) {
            //console.log(response.data)
            respuesta = response.data
            //console.log(respuesta)
            editor1 = $("#tableSala").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "paging": true,
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                "initComplete": function (settings, json) {

                },
                data: response.data,
                columns: [
                    { data: "CodSala", title: "Codigo" },
                    { data: "Nombre", title: "Sala" },
                    { data: "IpPublica", title: "Ip publica" },
                    {
                        data: "Puerto9895", title: "Puerto 9895",
                        "render": function (o, value, oData) {
                            var estado = "INACTIVO";
                            var css = "btn-danger";
                            if (o == true) {
                                estado = "ACTIVO"
                                css = "btn-success";

                                if (primeraIpPrivada) {
                                    ipPrivada = oData.IpPublica;
                                    //Comentar esta linea de abajo cuando se actualize en todas las salas el servicio
                                    ipPrivada = "190.187.44.222";
                                    primeraIpPrivada = false;
                                    console.log("ïpPrivada para todas las peticiones: "+ipPrivada);
                                }

                            }
                            return '<span class="label ' + css + '">' + estado + '</span>';

                        }
                    },
                    {
                        data: "Puerto2020", title: "Puerto 2020",
                        "render": function (o) {
                            var estado = "INACTIVO";
                            var css = "btn-danger";
                            if (o == true) {
                                estado = "ACTIVO"
                                css = "btn-success";
                            }
                            return '<span class="label ' + css + '">' + estado + '</span>';

                        }
                    },
                    {
                        data: "Puerto8081", title: "Puerto 8081",
                        "render": function (o) {
                            var estado = "INACTIVO";
                            var css = "btn-danger";
                            if (o == true) {
                                estado = "ACTIVO"
                                css = "btn-success";
                            }
                            return '<span class="label ' + css + '">' + estado + '</span>';

                        }
                    }
                    //{ data: "IpPrivada", title: "Ip Privada" },
                    //{ data: "PuertoServicioWebOnline", title: "Puerto Servicio Web Online" },
                    //{ data: "PuertoWebOnline", title: "Puerto Web Online" },
                    //{ data: "CarpetaOnline", title: "Carpeta Online" },
                ],
                "drawCallback": function (settings) {

                },

                "initComplete": function (settings, json) {


                },
            });

        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        }
    });
};


$(document).ready(function () {

    obtenerListaIps();

    VistaAuditoria("Sala/SalaEchoPingVista", "VISTA", 0, "", 3);

    $(document).on("click", "#btnIpsPublicas", function () {
        ListarSalasIpPublica();
    });

    $(document).on("click", "#btnIpsPrivadas", function () {
        ListarSalasIpPrivada(ipPrivada);
    });

    $(document).on("click", ".btnDispositivos", function () {

        var urlPrivada = $(this).data("id");
        var tipo = $(this).data("tipo");
        listarDispositivos(urlPrivada,tipo);
        $("#full-modal_dispositivo").modal("show");

    });

    $(document).on("click", ".btnProgresivos", function () {

        var urlPrivada = $(this).data("id");
        var tipo = $(this).data("tipo");
        listarProgresivos(urlPrivada, tipo);
        $("#full-modal_progresivo").modal("show");

    });

});

function listarDispositivos(urlPrivada, tipo) {
    var url = basePath + "Sala/ListadoDispositivos";
    //ipPrivada = "localhost";
    //urlPrivada = "localhost";
    console.log("Intentando conseguir lista dispositivos desde " + ipPrivada +" hacia "+ urlPrivada);
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({  urlPublica:ipPrivada, urlPrivada, tipo }),
        beforeSend: function () {
            //editor2.clear().draw();

            editor2 = $("#tableDispositivo").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "paging": true,
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                "initComplete": function (settings, json) {

                },
                data: {},
                columns: [
                    { data: "Id", title: "Id" },
                    { data: "Token", title: "Url" },
                    {
                        data: "EstadoPing", title: "Estado",
                        "render": function (o) {
                            var estado = "INACTIVO";
                            var css = "btn-danger";
                            if (o == true) {
                                estado = "ACTIVO"
                                css = "btn-success";
                            }
                            return '<span class="label ' + css + '">' + estado + '</span>';

                        }
                    },
                    //{ data: "IpPrivada", title: "Ip Privada" },
                    //{ data: "PuertoServicioWebOnline", title: "Puerto Servicio Web Online" },
                    //{ data: "PuertoWebOnline", title: "Puerto Web Online" },
                    //{ data: "CarpetaOnline", title: "Carpeta Online" },
                ],
                "drawCallback": function (settings) {

                },

                "initComplete": function (settings, json) {


                },
            });

            $.LoadingOverlay("show");
            //overlayIpsPrivadas();
        },
        complete: function () {
            //overlayLimpiar();
            $.LoadingOverlay("hide");
            console.log("Terminado conseguir lista dispositivos desde " + ipPrivada + " hacia " + urlPrivada);

        },
        success: function (response) {

            if (response.respuesta) {


                //console.log(response.data)
                respuesta = response.data
                //console.log(respuesta)
                editor2 = $("#tableDispositivo").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "scrollCollapse": true,
                    "scrollX": false,
                    "paging": true,
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    "initComplete": function (settings, json) {

                    },
                    data: response.data,
                    columns: [
                        { data: "Id", title: "Id" },
                        { data: "Token", title: "Url" },
                        {
                            data: "EstadoPing", title: "Estado",
                            "render": function (o) {
                                var estado = "INACTIVO";
                                var css = "btn-danger";
                                if (o == true) {
                                    estado = "ACTIVO"
                                    css = "btn-success";
                                }
                                return '<span class="label ' + css + '">' + estado + '</span>';

                            }
                        },
                        //{ data: "IpPrivada", title: "Ip Privada" },
                        //{ data: "PuertoServicioWebOnline", title: "Puerto Servicio Web Online" },
                        //{ data: "PuertoWebOnline", title: "Puerto Web Online" },
                        //{ data: "CarpetaOnline", title: "Carpeta Online" },
                    ],
                    "drawCallback": function (settings) {

                    },

                    "initComplete": function (settings, json) {


                    },
                });

            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");

            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        }
    });
};


function listarProgresivos(urlPrivada, tipo) {
    var url = basePath + "Sala/ListadoProgresivos";
    //ipPrivada = "localhost";
    //urlPrivada = "localhost";
    console.log("Intentando conseguir lista progresivos desde " + ipPrivada + " hacia " + urlPrivada);
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ urlPublica: ipPrivada, urlPrivada, tipo }),
        beforeSend: function () {
            //editor2.clear().draw();

            editor2 = $("#tableProgresivo").DataTable({
                "bDestroy": true,
                "bSort": true,
                "scrollCollapse": true,
                "scrollX": false,
                "paging": true,
                "autoWidth": false,
                "bProcessing": true,
                "bDeferRender": true,
                "initComplete": function (settings, json) {

                },
                data: {},
                columns: [
                    { data: "WEB_PrgID", title: "Id" },
                    { data: "WEB_Nombre", title: "Nombre" },
                    { data: "WEB_Url", title: "Url" },
                    {
                        data: "EstadoPing", title: "Estado",
                        "render": function (o) {
                            var estado = "INACTIVO";
                            var css = "btn-danger";
                            if (o == true) {
                                estado = "ACTIVO"
                                css = "btn-success";
                            }
                            return '<span class="label ' + css + '">' + estado + '</span>';

                        }
                    },
                    //{ data: "IpPrivada", title: "Ip Privada" },
                    //{ data: "PuertoServicioWebOnline", title: "Puerto Servicio Web Online" },
                    //{ data: "PuertoWebOnline", title: "Puerto Web Online" },
                    //{ data: "CarpetaOnline", title: "Carpeta Online" },
                ],
                "drawCallback": function (settings) {

                },

                "initComplete": function (settings, json) {


                },
            });

            $.LoadingOverlay("show");
            //overlayIpsPrivadas();
        },
        complete: function () {
            //overlayLimpiar();
            $.LoadingOverlay("hide");
            console.log("Terminado conseguir lista progresivos desde " + ipPrivada + " hacia " + urlPrivada);

        },
        success: function (response) {

            if (response.respuesta) {


                //console.log(response.data)
                respuesta = response.data
                //console.log(respuesta)
                editor2 = $("#tableProgresivo").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "scrollCollapse": true,
                    "scrollX": false,
                    "paging": true,
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    "initComplete": function (settings, json) {

                    },
                    data: response.data,
                    columns: [
                        { data: "WEB_PrgID", title: "Id" },
                        { data: "WEB_Nombre", title: "Nombre" },
                        { data: "WEB_Url", title: "Url" },
                        {
                            data: "EstadoPing", title: "Estado",
                            "render": function (o) {
                                var estado = "INACTIVO";
                                var css = "btn-danger";
                                if (o == true) {
                                    estado = "ACTIVO"
                                    css = "btn-success";
                                }
                                return '<span class="label ' + css + '">' + estado + '</span>';

                            }
                        },
                        //{ data: "IpPrivada", title: "Ip Privada" },
                        //{ data: "PuertoServicioWebOnline", title: "Puerto Servicio Web Online" },
                        //{ data: "PuertoWebOnline", title: "Puerto Web Online" },
                        //{ data: "CarpetaOnline", title: "Carpeta Online" },
                    ],
                    "drawCallback": function (settings) {

                    },

                    "initComplete": function (settings, json) {


                    },
                });

            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");

            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        }
    });
};


function ListarSalasIpPrivada(ipSala) {
    console.log("Intentando conseguir Ips Privadas en " + ipSala);
    var url = basePath + "Sala/ListadoPingIpPrivada";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ ipSala }),
        beforeSend: function () {
            editor2.clear().draw();
            $.LoadingOverlay("show");
            overlayIpsPrivadas();
        },
        complete: function () {
            overlayLimpiar();
            $.LoadingOverlay("hide");
            console.log("Terminado conseguir Ips Privadas en " + ipSala);

        },
        success: function (response) {

            if (response.respuesta) {


                //console.log(response.data)
                respuesta = response.data
                //console.log(respuesta)
                editor2 = $("#tableSala2").DataTable({
                    "bDestroy": true,
                    "bSort": true,
                    "scrollCollapse": true,
                    "scrollX": false,
                    "paging": true,
                    "autoWidth": false,
                    "bProcessing": true,
                    "bDeferRender": true,
                    "initComplete": function (settings, json) {

                    },
                    data: response.data,
                    columns: [
                        { data: "CodSala", title: "Codigo" },
                        { data: "Nombre", title: "Sala" },
                        { data: "IpPrivada", title: "Ip privada" },
                        {
                            data: "Puerto9895", title: "Puerto 9895",
                            "render": function (o) {
                                var estado = "INACTIVO";
                                var css = "btn-danger";
                                if (o == true) {
                                    estado = "ACTIVO"
                                    css = "btn-success";
                                }
                                return '<span class="label ' + css + '">' + estado + '</span>';

                            }
                        },
                        {
                            data: "Puerto2020", title: "Puerto 2020",
                            "render": function (o) {
                                var estado = "INACTIVO";
                                var css = "btn-danger";
                                if (o == true) {
                                    estado = "ACTIVO"
                                    css = "btn-success";
                                }
                                return '<span class="label ' + css + '">' + estado + '</span>';

                            }
                        },
                        {
                            data: "Puerto8081", title: "Puerto 8081",
                            "render": function (o) {
                                var estado = "INACTIVO";
                                var css = "btn-danger";
                                if (o == true) {
                                    estado = "ACTIVO"
                                    css = "btn-success";
                                }
                                return '<span class="label ' + css + '">' + estado + '</span>';

                            }
                        },
                        {
                            data: "IpPrivada", title: "Acción",
                            "bSortable": false,
                            "render": function (o) {
                                return `<button type="button" id="btnDispositivos" class="btn btn-xs btn-warning btnDispositivos" data-id="${o}" data-tipo="1">Dispositivos</button> 
                                    <button type="button" id="btnProgresivos" class="btn btn-xs btn-danger btnProgresivos" data-id="${o}" data-tipo="2">Progresivos</button> `;
                            }
                        }
                        //{ data: "IpPrivada", title: "Ip Privada" },
                        //{ data: "PuertoServicioWebOnline", title: "Puerto Servicio Web Online" },
                        //{ data: "PuertoWebOnline", title: "Puerto Web Online" },
                        //{ data: "CarpetaOnline", title: "Carpeta Online" },
                    ],
                    "drawCallback": function (settings) {

                    },

                    "initComplete": function (settings, json) {


                    },
                });

            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");

            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        }
    });
};

function obtenerListaIps() {
    console.log("Intentando conseguir Ips Publicas y Privadas");
    var url = basePath + "Sala/ListadoIpsSalas";
    var data = {}; var respuesta = ""; aaaa = "";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: function () {
        },
        complete: function () {
            console.log("Terminado conseguir Ips Publicas y Privadas");
            ListarSalasIpPublica();
        },
        success: function (response) {
            console.log(response.data)
            respuesta = response.data;


            var textoAddPublica = 'Revisando Ip Publica ';
            textsIpsPublicas.push('Iniciando revision Ips Publicas...');
            var textoAddPrivada = 'Revisando Ip Privada ';
            textsIpsPrivadas.push('Iniciando revision Ips Privadas...');

            respuesta.forEach(function (item, index) {

                if (item.UrlProgresivo.trim()) {
                    textsIpsPublicas.push(textoAddPublica+item.UrlProgresivo);
                }

                if (item.IpPrivada.trim()) {
                    textsIpsPrivadas.push(textoAddPrivada +item.IpPrivada);
                }
            });


        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        }
    });
}


function overlayIpsPublicas() {

    var e = $('<div><h1 id="textoOverlayPublica" style="color:#FFFFFF;">Iniciando revision Ips Publicas....</h1></div>');
    $('.loadingoverlay').append(e);

    var point = 0;

    function changeText() {
        $('#textoOverlayPublica').html(textsIpsPublicas[point]);
        if (point < (textsIpsPublicas.length - 1)) {
            point++;
        } else {
            point = 0;
        }

    }

    intervalPublica = setInterval(changeText, 1000);
    changeText();
}

function overlayIpsPrivadas() {

    var e = $('<div><h1 id="textoOverlayPrivada" style="color:#FFFFFF;">Iniciando revision Ips Privadas...</h1></div>');
    $('.loadingoverlay').append(e);

    var point = 0;

    function changeText() {
        $('#textoOverlayPrivada').html(textsIpsPrivadas[point]);
        if (point < (textsIpsPrivadas.length - 1)) {
            point++;
        } else {
            point = 0;
        }

    }

    intervalPrivada = setInterval(changeText, 1000);
    changeText();
}


function overlayLimpiar() {
    $('#textoOverlayPublica').remove();
    $('#textoOverlayPrivada').remove();
    clearInterval(intervalPublica);
    clearInterval(intervalPrivada);
}
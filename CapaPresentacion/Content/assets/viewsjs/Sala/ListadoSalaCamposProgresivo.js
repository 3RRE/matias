
var editor;
empleadodatatable = "";


function afterTableInitialization(settings, json) {
    tableAHORA = settings.oInstance.api();
}


function ListarSalas() {
    var url = basePath + "Sala/ListadoSalaCamposProgresivo";
    var data = {}; var respuesta = ""; aaaa = "";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");

            $(document).on('keypress', '.PuertoServicioWebOnline', function (event) {
                var regex = new RegExp("^[0-9]+$");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            })
            $(document).on('keypress', '.PuertoWebOnline', function (event) {
                var regex = new RegExp("^[0-9]+$");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            })
        },
        success: function (response) {
            //console.log(response.data)
            respuesta = response.data
            //console.log(respuesta)
            editor = $("#tableSala").DataTable({
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
                    { data: "Nombre", title: "Sala" },
                    {
                        data: "IpPrivada", title: "Ip Privada",
                        "render": function (o, value, oData) {
                            return `<input type="text" class="IpPrivada" data-id="${oData.CodSala}" data-tipo="IpPrivada"  value="${o}">`;

                        }
                    }, {
                    data: "PuertoServicioWebOnline", title: "Puerto Servicio Web Online",
                        "render": function (o, value, oData) {
                            return `<input type="text" class="PuertoServicioWebOnline" data-id="${oData.CodSala}" data-tipo="PuertoServicioWebOnline"   value="${o}">`;

                        }
                    }, {
                    data: "PuertoWebOnline", title: "Puerto Web Online",
                        "render": function (o, value, oData) {
                            return `<input type="text" class="PuertoWebOnline" data-id="${oData.CodSala}" data-tipo="PuertoWebOnline"   value="${o}">`;

                        }
                    }, {
                        data: "CarpetaOnline", title: "Carpeta Online",
                        "render": function (o, value, oData) {
                            return `<input type="text" class="CarpetaOnline" data-id="${oData.CodSala}" data-tipo="CarpetaOnline"   value="${o}">`;

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



    ListarSalas();

    var url = basePath + "Sala/ListadoSalaCamposProgresivo";

    VistaAuditoria("Sala/SalaCamposProgresivoVista", "VISTA", 0, "", 3);

    $('#tableSala').on('keypress', 'tbody td input', function (e) {

        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {


            var value = $(this).val();
            //console.log(value);
            var codSala = $(this).data("id");
            //console.log(codSala);
            var tipo = $(this).data("tipo");
            //console.log(tipo);

            if (tipo == "IpPrivada") {


                if (value.includes("http://") || value.includes("https://")) {
                } else {
                    value = "http://" + value;
                }

                var regexIp = new RegExp("(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)");
                var regexUrlHttp = new RegExp("http?:\/\/[\w\-]+(\.[\w\-]+)+[/#?]?.*");
                var regexUrlHttps = new RegExp("https?:\/\/[\w\-]+(\.[\w\-]+)+[/#?]?.*");

                //console.log(regexIp.test(value));
                //console.log(regexUrlHttp.test(value));
                //console.log(regexUrlHttps.test(value));

                if (regexIp.test(value) || regexUrlHttp.test(value) || regexUrlHttps.test(value)) {

                    //console.log(value);

                } else {

                    toastr.error("URL invalida.", "Mensaje Servidor");

                    return false;
                }




            }

            if (tipo == "PuertoServicioWebOnline") {

                if (value < 0 || value > 65536) {
                    toastr.error("Puerto invalido.", "Mensaje Servidor");
                    return false;
                } 

            }

            if (tipo == "PuertoWebOnline") {

                if (value < 0 || value > 65536) {
                    toastr.error("Puerto invalido.", "Mensaje Servidor");
                    return false;
                } 

            }

            if (tipo == "CarpetaOnline") {



            }


            EditarCampo(codSala, tipo, value)


        }


    });



});


function EditarCampo(salaId, nameQuery, value) {

    //console.log(" Intentando ... POST - Value:" + value + " - Tipo:" + nameQuery + " - CodSala:" + salaId);
    $.ajax({
        url: basePath + "Sala/SalaModificarCamposProgresivoJson",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ salaId, nameQuery, value }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
            ListarSalas();
        },
        success: function (response) {
            if (response.respuesta) {
                //console.log("Realizado ... POST - Value:" + value + " - Tipo:" + nameQuery + " - CodSala:" + salaId);
                toastr.success(nameQuery+" editado correctamente.", "Mensaje Servidor");
                VistaAuditoria("Sala/SalaModificarCamposProgresivoJson", "VISTA", 0, "", 3);
            }
            else {
                toastr.error(nameQuery + " no se pudo editar.", "Mensaje Servidor");
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            toastr.error("Error Servidor", "Mensaje Servidor");
        }
    });
}
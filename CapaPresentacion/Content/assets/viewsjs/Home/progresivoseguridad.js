$(function () {
    // Reference the auto-generated proxy for the hub.  
    chat = $.connection.progresivoHub;
    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (name, message) {
        // Add the message to the page. 
        $('#discussion').append('<li><strong>' + htmlEncode(name)
            + '</strong>: ' + htmlEncode(message) + '</li>');
    };
    // Get the user name and store it to prepend to messages.
    chat.client.online = function (count) {
        //console.log(count)
        $(".countusaurios").html(count);
    };

    chat.client.refrescarListaConecciones = function () {
        if ($("#btnReload").length > 0) {
            $("#btnReload").click();
        }
    };

    chat.client.refrescarf5 = function () {
        window.location.href = window.location.href;
    };

    chat.client.cerrarLogin = function () {
        $(".btn_salirLogin").click();
    };

    chat.client.cerrarUsuario = function () {
        $(".btn_salirLogin").click();
    };

    chat.client.mensajeAtodos = function (message) {
        // Add the message to the page. 
        toastr.info(message, 'Mensaje Servidor', { timeOut: 0, closeButton: true, positionClass: "toast-bottom-left", extendedTimeOut: 0, })
    };

    chat.client.mensajeAusuario = function (message) {
        // Add the message to the page. 
        toastr.info(message, 'Mensaje Servidor', { timeOut: 0, closeButton: true, positionClass: "toast-bottom-left", extendedTimeOut: 0, })
    };


    // Start the connection.
    $.connection.hub.start().done(function () {
        console.log($.connection.progresivoHub.connection.id);
        $(".conection_spn").text($.connection.progresivoHub.connection.id)
        //chat.server.validarConexion($.connection.progresivoHub.connection.id)
        let conect_idAct = localStorage.getItem("conect_idAct");
        if (conect_idAct == null) {
            localStorage.setItem("conect_idAct", $.connection.progresivoHub.connection.id);
            chat.server.conectar($.connection.progresivoHub.connection.id)
        }
        else {
            chat.server.desconectar(conect_idAct);
            localStorage.setItem("conect_idAct", $.connection.progresivoHub.connection.id);
            chat.server.conectar($.connection.progresivoHub.connection.id)
        }

        $(document).on("click", ".pedirtoken", function () {
            toastr.clear();
            chat.server.pedirToken();

            toastr.success('Solicitud de Token enviada, espere por favor', 'Mensaje Servidor', { timeOut: 0, closeButton: true, positionClass: "toast-bottom-left", extendedTimeOut: 0, })
        });

        $(document).on("click", ".vertoken", function () {
            toastr.clear();
            chat.server.verToken();
        });
    });
});
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

function enviartoken(sgn_id) {
    chat.server.enviarToken(sgn_id);
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname + "=" + cvalue + "; " + expires + "; path=/";
}
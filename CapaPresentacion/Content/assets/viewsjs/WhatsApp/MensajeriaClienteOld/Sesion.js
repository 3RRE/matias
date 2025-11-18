let socket;
let urlService;

//objetos
const btnLogout = $('#btnLogout');
const btnSendMessage = $('#btnSendMessage');
const chkStatusServerWhatsApp = $('#chkStatusServerWhatsApp');

$(document).ready(() => {
    setWhatsAppServiceStatus(false)
    getServerWhatsApp();
    hideAllSections();
    login();
})

chkStatusServerWhatsApp.bootstrapSwitch({
    onColor: "success",
    offColor: "danger",
    onText: "Encendido",
    offText: "Apagado",
    size: "mini",
    animate: true
});

chkStatusServerWhatsApp.on('switchChange.bootstrapSwitch', function (event, state) {
    $.confirm({
        title: 'Confirmación',
        content: state ? '¿Seguro que desea encender el Servicio de WhatsApp?' : '¿Seguro que desea apagar el Servicio de WhatsApp?',
        confirmButton: 'Sí',
        cancelButton: 'No',
        confirm: function () {
            if (state) {
                startServiceWhatsApp();
            } else {
                stopServiceWhatsApp();
            }
        },
        cancel: function () {
            let action = state ? 'encendido' : 'apagado';
            toastr.info(`Se cancelo el ${action} del Servicio de WhatsApp`);
            setWhatsAppServiceStatus(!state);
        }
    });
});

btnLogout.on("click", (e) => {
    $.confirm({
        title: 'Confirmación',
        content: '¿Seguro que quiere cerrar sesión?',
        confirmButton: 'Sí',
        cancelButton: 'No',
        confirm: function () {
            logout();
        },
        cancel: function () {
            toastr.info("Se cancelo el cierre de sesión.");
        }
    });
});

btnSendMessage.on("click", (e) => {

    let message = $('#txtMensaje').val();

    if (!message) {
        toastr.info("Ingrese un mensaje.")
        return;
    }

    let table = $("#tableClient").DataTable()
    let clients = getSelectedRows(table);

    if (clients.length == 0) {
        toastr.info("Seleccione al menos un cliente para enviar el mensaje.")
        return;
    }

    var aux = clients.length == 1 ? 'cliente' : 'clientes';

    $.confirm({
        title: 'Confirmación',
        content: `¿Seguro que desea enviar el mensaje a ${clients.length} ${aux}?<br><br>
            <ul>
                <li>Solo se enviará el mensaje a los clientes que tengan registrado el código de país.</li>
                <li>Aquellos que no tengan código de país y comiencen con 9 y tengan 9 digitos se le agregara el código país +51</li>
                <li>Si no tiene número, se tratará de usar el numero alternativo, siempre y cuando cumpla las condiciones anteriores.</li>
            <ul>`,
        confirmButton: 'Sí',
        cancelButton: 'No',
        confirm: function () {
            var imageFile = $('#image')[0].files[0];

            if (imageFile) {
                sendMessageWithImage(clients, message);
            } else {
                sendMessageWithOutImage(clients, message);
            }
        },
        cancel: function () {
            toastr.info("Se cancelo el envío del mensaje.");
        }
    });
});

const startServiceWhatsApp = () => {
    let url = `${basePath}WhatsApp/IniciarServicio`;
    $.ajax({
        url: url,
        type: "GET",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.displayMessage)
                connectSocket();
                login();
            } else {
                setWhatsAppServiceStatus(false);
                toastr.error(response.displayMessage)
            }
        }
    });
}

const stopServiceWhatsApp = () => {
    let url = `${basePath}WhatsApp/DetenerServicio`;
    $.ajax({
        url: url,
        type: "GET",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.displayMessage)
                cleanQR();
                hideAllSections();
                showSectionNoService();
                disconnectSocket();
            } else {
                setWhatsAppServiceStatus(true);
                toastr.error(response.displayMessage)
            }
        }
    });
}

const getServerWhatsApp = () => {
    let url = `${basePath}WhatsApp/ObtenerUrlWhatsAppAPI`;
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json",
        success: function (response) {
            urlService = response.url;
            socket = io(urlService);
            checkServerWhatsApp();
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            toastr.error("No sepudo obtener la URI del servidor de WhatsApp, recargue la página. Si el problema persiste comuniquese con su Administrador");
        }
    });
}

const checkServerWhatsApp = () => {
    let url = `${urlService}/ping`;
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json",
        success: function (response) {
            if (response.success) {
                startSocket(socket);
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            hideAllSections();
            startSocket(socket);
            disconnectSocket();
            showSectionNoService();
        }
    });
}

const startSocket = (socket) => {
    socket.on("loading-screen", (info) => {
        $.LoadingOverlay("show");
        let loading = JSON.parse(info);
        toastr.info(`${loading.message} - ${loading.percent}%`)
        console.log("Loading ...");
    });

    socket.on("qr-generate", (qr) => {
        console.log("New QR => " + new Date());
        makeQR(qr)
    });

    socket.on("login-fail", () => {
        $.LoadingOverlay("hide", true);
        console.log("Login Fail");
        toastr.error("Error al iniciar sesión en WhatsApp.")
    });

    socket.on("user-authenticated", () => {
        $.LoadingOverlay("show");
        toastr.success("Autenticado correctamente.")
        console.log("User Authenticathed");
    });

    socket.on("session-ready", (info) => {
        $.LoadingOverlay("hide", true);
        console.log("Session ready");
        toastr.success("Sesión iniciada correctamente.")
        completeSessionInfo(info);
        manipulateVisibilityItems(true);
        cleanQR();
    });

    socket.on("logout", (info) => {
        console.log("Session closed");
        showSectionScanQR();
        manipulateVisibilityItems(false);
        login();
    });
}

const login = () => {
    let url = `${basePath}WhatsApp/Login`;
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            if (response.success) {
                data = response.data;
                manipulateVisibilityItems(data.hasSession);
                if (data.hasSession) {
                    //ACA OBTENIA LOS CLIENTES ACTIVOS
                }
                if (data.qr) {
                    makeQR(data.qr);
                }
                if (data.info) {
                    completeSessionInfo(data.info);
                }
                toastr.success(response.displayMessage);
            } else {
                hideAllSections();
                showSectionNoService();
                toastr.error(response.displayMessage);
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

const logout = () => {
    let url = `${basePath}WhatsApp/Logout`;

    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json",
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.displayMessage);
                //manipulateVisibilityItems(false);
                //login();
            } else {
                hideAllSections();
                showSectionNoService();
                toastr.error(response.displayMessage);
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            hideAllSections();
            showSectionNoService();
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        }
    });
};

const sendMessageWithImage = (ids, message) => {

    var imageFile = $('#image')[0].files[0];

    if (!imageFile) {
        toastr.info("No hay ni una imagen adjunta con el mensaje.")
        return;
    }

    let url = `${basePath}MensajeriaCliente/EnviarMensajeMultipleConImagen`;

    var formData = new FormData();

    let idsString = ids.join(',');

    formData.append('image', imageFile);
    formData.append('ids', idsString);
    formData.append('message', message);

    $.ajax({
        url: url,
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.displayMessage);
                resetMessage();
            } else {
                toastr.error(response.displayMessage);
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            hideAllSections();
            showSectionNoService();
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        }
    });
}

const sendMessageWithOutImage = (ids, message) => {
    
    let url = `${basePath}MensajeriaCliente/EnviarMensajeMultipleSinImagen`;

    let data = {
        ids: ids,
        message: message
    };

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
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.displayMessage);
                resetMessage();
            } else {
                toastr.error(response.displayMessage);
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrow) {
            hideAllSections();
            showSectionNoService();
            if (xmlHttpRequest.status == 400) {
                toastr.error(xmlHttpRequest.responseText, "Mensaje Servidor");
            } else {
                toastr.error("Error Servidor", "Mensaje Servidor");
            }
        }
    });
}

//======================================UTILS==============================================
//Coneccion y desconeccion del socket
const disconnectSocket = () => {
    if (socket) {
       socket.disconnect();
    }
}
const connectSocket = () => {
    if (socket) {
        socket.connect();
    }
}

//para setear el estado del switch
const setWhatsAppServiceStatus = (state) => {
    chkStatusServerWhatsApp.bootstrapSwitch('state', state, true);
}

//para resetar el mensaje
const resetMessage = () => {
    removeImage();
    $('#txtMensaje').val('');
}

//para los popovers
const initPopovers = () => {
    initPoppover($('#popover-info-session'), $('#popover-info-session-content'), "Información de la Sesión");
    initPoppover($('#popover-help'), $('#popover-help-content'), "Ayuda para formatear Texto");
}

const initPoppover = (item, itemContent, title) => {
    item.popover({
        title: title,
        html: true,
        animation: true,
        delay: {
            show: 0,
            hide: 150
        },
        placement: 'left',
        trigger: 'hover',
        content: function () {
            return itemContent.html();
        }
    });
}

//para la informacion de la sesion
const completeSessionInfo = (info) => {
    $('#infoPlataforma').text(info.platform);
    $('#infoUsuario').text(info.pushname);
    $('#infoNumeroTelefono').text(info.wid.user);
    initPopovers();
}

//para el QR
const makeQR = (qr) => {
    cleanQR();
    hideSectionScanQR();
    var qr = new QRious({
        element: document.getElementById('qrcode'),
        value: qr,
        level: 'H',
        padding: null,
        size: 350,
    });
}
const cleanQR = () => {
    showSectionScanQR();
    const canvaQR = document.getElementById('qrcode');
    const ctx = canvaQR.getContext("2d");
    ctx.clearRect(0, 0, canvaQR.width, canvaQR.height);
}

//para la visibilidad
const manipulateVisibilityItems = (hasSession) => {
    if (hasSession) {
        hideSectionQR();
        showSectionButton();
        showSectionFilterClient();
        showSectionMessage();
    } else {
        showSectionQR();
        hideSectionButton();
        hideSectionFilterClient();
        hideSectionMessage();
    }
    hideSectionNoService();
}
const hideAllSections = () => {
    hideSectionButton();
    hideSectionQR();
    hideSectionFilterClient();
    hideSectionMessage();
    hideSectionNoService();
}

//la seccion de que el servicio esta apagado
const showSectionNoService = () => {
    $("#contentNoService").fadeIn("slow");
    setWhatsAppServiceStatus(false);
}
const hideSectionNoService = () => {
    $("#contentNoService").fadeOut();
    setWhatsAppServiceStatus(true);
}

//los botones para buscar
const showSectionButton= () => {
    $("#contentButtonSection").fadeIn("slow");
}
const hideSectionButton = () => {
    $("#contentButtonSection").fadeOut();
}

//el mensaje de escanar qr
const showSectionScanQR = () => {
    $("#loader-container").fadeIn("slow");
}
const hideSectionScanQR = () => {
    $("#loader-container").fadeOut();
}

//la seccion de scaneo de QR
const showSectionQR = () => {
    $("#contentQrSession").fadeIn("slow");
}
const hideSectionQR = () => {
    $("#contentQrSession").fadeOut();
}

//la seccion de filtro de cliente
const showSectionFilterClient = () => {
    $("#contentFilterClientSecction").fadeIn("slow");
}
const hideSectionFilterClient = () => {
    $("#contentFilterClientSecction").fadeOut();
}

//la seccion del envio de mensaje
const showSectionMessage = () => {
    $("#contentMessageSession").fadeIn("slow");
}
const hideSectionMessage = () => {
    $("#contentMessageSession").fadeOut();
}

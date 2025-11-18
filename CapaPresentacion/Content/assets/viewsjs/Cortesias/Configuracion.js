const cboSala = $("#cboSala");

$(document).ready(() => {
    getSalas();
})

cboSala.on('change', () => {
    const codSala = cboSala.val();
    if (!codSala) {
        toastr.warning("Seleccione una sala", "Aviso")
        return;
    }
    getConfiguracion(codSala);
});



const getConfiguracion = (codSala) => {
    const url = `${basePath}Cortesias/GetKeysConfiguracion`;
    const data = {
        codSala
    };
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            if (!response.success) {
                toastr.error(response.displayMessage, "Mensaje Servidor")
                return;
            }

            $("#content").html("");
            $.each(response.data, function (i, item) {


                var data = `

                                <div class="col-md-6 col-sm-12 col-xs-12" style="padding:15px">
                                    <div style="background-color:#eff4f9; padding:15px;border-radius:10px !important;display:flex; flex-direction:column; justify-content: space-between;" id="divKey">
                                        <div style="display:flex;justify-content: space-between;">
                                            <h1 style="word-break:break-all; font-weight: 600;line-height: 25px;">${item.Nombre}</h1>
                                            <label class="switch">
                                                <input type="checkbox" id="chk${item.Nombre}" ${item.Estado ? "checked" : ""} value="${item.Estado}">
                                                <span class="slider round"></span>
                                            </label>
                                        </div>
                                        <input type="text" class="form-control input-sm" id="txt${item.Nombre}" name="${item.Nombre}" placeholder="${item.Nombre}" value="${item.Valor}" style="background-color:#d7ebf9; visibility: ${item.Estado ? (item.Valor >= 0 ? "visible" : "hidden"):"hidden"};">
                                    </div>
                                </div>
                                

                               `;


                var idTxtKey = "#txt" + item.Nombre;
                var idChkKey = "#chk" + item.Nombre;
                $("#content").append(data);

                $(idTxtKey).keydown(function (e) {
                    if (e.keyCode == 13) {
                        var key = item.Nombre;
                        var value = $(this).val();
                        UpdateKeyValueConfiguracion(codSala, item.Nombre, value);
                    }
                });


                $(idChkKey).on('change', () => {

                    const codSala = cboSala.val();
                    if (!codSala) {
                        toastr.warning("Seleccione una sala", "Aviso")
                        return;
                    }

                    const key = item.Nombre;
                    let state = false;
                    let visbility = "hidden";

                    if ($(idChkKey).val() == 'true') {
                        state = false;
                        visbility = "hidden";
                    } else {

                        state = true;
                        if (item.Valor >= 0) {
                            visbility = "visible";
                        }

                    }


                    UpdateKeyStateConfiguracion(codSala, key, state).then(response => {

                        if (response.success) {
                            $(idChkKey).val(state)
                            $(idTxtKey).css("visibility", visbility);
                        } else {
                            $(idChkKey).prop("checked", !state)
                        }
                    });


                });


            });

        }
    });
};

const getSalas = () => {
    $.when(
        llenarSelect(`${basePath}Sala/ListadoSalaPorUsuarioJson`, {}, "cboSala", "CodSala", "Nombre")
    ).then(() => {
        cboSala.select2({
            placeholder: "Seleccione ...",
            multiple: false,
        });
    });
}

const UpdateKeyValueConfiguracion = (codSala, key, value) => {
    const data = {
        codSala,
        key,
        value
    };
    $.ajax({
        url: `${basePath}Cortesias/UpdateKeyValueConfiguracion`,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            if (!response.success) {
                toastr.error(response.message, "Mensaje Servidor");
            }
            toastr.success(response.message, "Mensaje Servidor");
        }
    });
}

function UpdateKeyStateConfiguracion (codSala, key,state){
    const data = {
        codSala,
        key,
        state
    };
    return $.ajax({
        url: `${basePath}Cortesias/UpdateKeyStateConfiguracion`,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        beforeSend: () => $.LoadingOverlay("show"),
        complete: () => $.LoadingOverlay("hide"),
        success: (response) => {
            if (!response.success) {
                toastr.error(response.message, "Mensaje Servidor");
                return;
            }
            toastr.success(response.message, "Mensaje Servidor");
        }
    });
}


let urlsResultado = []
let timerId = ''
let delay = 60000
$(document).ready(function () {
    $("#cboSala").select2();

    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        maxDate: dateNow,
    });



    clearTimeout(timerId)
    timerId = setTimeout(function request() {
        getPingSalas().then(result => {
            urlsResultado = result
        })
        timerId = setTimeout(request, delay);
    }, delay)


    obtenerListaSalas().then(result => {
        if (result.data) {
            renderSelectSalas(result.data)
            getPingSalas().then(response => {
                urlsResultado = response
            })
        }
    })


    $(document).on('click', '#btnBuscar', function () {
        e.preventDefault();
        if ($("#cboSala").val() == 0) {
            return false
        }

        if (urlsResultado.length == 0) {
            toastr.warning("Inténtelo de nuevo en unos momentos", 'Mensaje Servidor')
            $("#cboSala").val('0').trigger('change')
            return false
        }

        toastr.remove();

        ipPublica = $(this).val();
    })
 
})

//Obtener salas del usuario logueado

const obtenerListaSalas=()=> {
    ajaxhr = $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            return result;
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            AbortRequest.close()
            $.LoadingOverlay("hide");
        }
    });
    AbortRequest.open()
    return ajaxhr
}

//Mostrar salas con sus ips

function renderSelectSalas(data) {
    $.each(data, function (index, value) {
        $("#cboSala").append(`<option value="${value.UrlProgresivo == "" ? "" : value.UrlProgresivo}" data-id="${value.CodSala}" data-ipprivada="${value.IpPrivada}" data-puertoservicio="${value.PuertoServicioWebOnline}">${value.Nombre}</option>`)
    });
}


//Obtener ip de la sala para consulta del servicio
const getPingSalas=()=> {
    return $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Progresivo/EchoPingSalasUsuario",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
        },
        success: function (response) {
            return response
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
        }
    });
}
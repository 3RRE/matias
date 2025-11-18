$(document).ready(function () {
    ObtenerListaSalas()
   
})

toastr.options = {
    "preventDuplicates": true
};

$(document).on('click', '#cboTotalMaquina', function () {
    var codSala = $('#cboSala').val();
    if (codSala) {
        $("#full-modal_totalmaquina").modal('show')
        $("#full-modal_totalmaquina").one("shown.bs.modal", function () {
            buscarReporteEstadoMaquina(codSala);
        })
    } else {
        toastr.warning("Debe seleccionar una sala primero.", "Advertencia");
    }
});

$(document).on('click', "#btnAgregarRegistroMaquina", function (e) { 
    e.preventDefault()
    limpiarValidadorFormCargo()

    let dataForm = {
        CodSala: $('#cboSala').val(),
        CodMaquinaINDECI: $('#cboCodigoMaquinaINDECI').val(),
        CodMaquinaRD: $('#cboCodigoMaquinaRD').val(),
        TotalMaquina: $('#cboTotalMaquina').val()
    }

    $("#formRegistroTemporal").data('bootstrapValidator').resetForm();
    var validarRegistro = $("#formRegistroTemporal").data('bootstrapValidator').validate();
    if (validarRegistro.isValid()) {
        let url = basePath
        url += "EstadoMaquina/InsertarRegistroMaquina"
        let dataForm4 = new FormData(document.getElementById("formRegistroTemporal"))
        $.ajax({
            url: url,
            type: "POST",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataForm),
            cache: false,
            processData: false,
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                if (response.respuesta) {
                    $('#cboSala').val(null).trigger('change')
                    $('#cboCodigoMaquinaINDECI').val('')
                    $('#cboCodigoMaquinaRD').val('')
                    $('#cboTotalMaquina').val('')
                    limpiarValidadorFormCargo();
                    toastr.success(response.mensaje, "Mensaje Servidor")

                } else {
                    toastr.error(response.mensaje, "Mensaje Servidor")
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
            }
        });
    }
})

function limpiarValidadorFormCargo() {
    $("#formRegistroTemporal").parent().find('div').removeClass("has-error");
    $("#formRegistroTemporal").parent().find('div').removeClass("has-success");
    $("#formRegistroTemporal").parent().find('i').removeAttr("style").hide();
}

$('#formRegistroTemporal').bootstrapValidator({
    feedbackIcons: {
        valid: 'glyphicon glyphicon-ok',
        invalid: 'glyphicon glyphicon-remove',
        validating: 'glyphicon glyphicon-refresh'
    },
    fields: {
        CodSala: {
            validators: {
                notEmpty: {
                    message: ' '
                }
            }
        },
        CodMaquinaINDECI: {
            validators: {
                notEmpty: {
                    message: ' '
                }
            }
        },
        CodMaquinaRD: {
            validators: {
                notEmpty: {
                    message: ' '
                }
            }
        }
    }
}).on('success.field.bv', function (e, data) {
    e.preventDefault();
    var $parent = data.element.parents('.form-group');
    $parent.removeClass('has-success');
    $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
});




function ObtenerListaSalas() {
    $.ajax({
        type: "POST",
        url: basePath + "Sala/ListadoSalaPorUsuarioJson",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            $("#cboSala").html('')
            const codSalaArray = result.data.map(sala => String(sala.CodSala));

            if (result.data) {
                arraySalas = result.data
                let arraySelect = [];
                result.data.map(item => arraySelect.push(`<option value="${item.CodSala}">${item.Nombre}</option>`))
                $("#cboSala").html(arraySelect.join(""))
                $("#cboSala").select2({
                    multiple: false, placeholder: "--Seleccione--", allowClear: true
                });
                $("#cboSala").val(-1).trigger("change")
            }
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor")
        },
        complete: function (resul) {
            $.LoadingOverlay("hide")
        }
    });
    return false;
}

$(document).on('change', '#cboSala', function () {
    const codSala = $(this).val();
    if (codSala) {
        ObtenerTotalMaquina(codSala);
    } else {
        $('#cboTotalMaquina').val('');
    }
});
 
function ObtenerTotalMaquina(value) {
        let url = basePath;
        url += "EstadoMaquina/ObtenerTotalMaquina";

        $.ajax({
            url: url,
            type: "POST",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ CodSala: value }),
            cache: false,
            processData: false,
            beforeSend: function () {
                $.LoadingOverlay("show");
            },
            complete: function () {
                $.LoadingOverlay("hide");
            },
            success: function (response) {
                if (response.respuesta) {
                    $('#cboTotalMaquina').val(response.totalMaquina);
                    toastr.success(response.mensaje, "Mensaje Servidor");
                } else {
                    $('#cboTotalMaquina').val(''); 
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }
            },
            error: function (xmlHttpRequest, textStatus, errorThrow) {
                $('#cboTotalMaquina').val('');
                toastr.error("Ha ocurrido un error, contacte con el administrador", "Error");
            }
        });
}
function buscarReporteEstadoMaquina(codSala) {
    var addtabla = $("#contenedor_tabla");


    $.ajax({
        type: "POST",
        url: basePath + "EstadoMaquina/ListaMaquinaxSalaxJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({
            codsala: [codSala],
        }),
        beforeSend: function () {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            response = response.data;

            $(addtabla).empty();
            $(addtabla).append('<table id="tablemaquinas" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');

            $("#tablemaquinas").DataTable({
                "bDestroy": true,
                "data": response.lista,
                aaSorting: [[0, 'desc']],
                "columns": [
                    { data: "id", title: "ID" },
                    { data: "sala", title: "Sala" },
                    { data: "CantMaquinaConectada", title: "Maq Conectadas" },
                    { data: "CantMaquinaNoConectada", title: "Maq No Conectadas" },
                    { data: "CantMaquinaPLay", title: "Maq PLAY" },
                    { data: "TotalMaquina", title: "Total" },
                    {
                        data: "FechaRegistro", title: "Fecha Reg.",
                        render: function (value) {
                            return moment(value).format('DD/MM/YYYY hh:mm A');
                        }
                    },
                    { data: "CantMaquinaRetiroTemporal", title: "Maq R.Temporal" }
                ]
            });

            toastr.success("Datos de la sala cargados correctamente.", "Éxito");
        },
        error: function () {
            toastr.error("Error al cargar los datos de la sala.", "Error");
        },
        complete: function () {
            $.LoadingOverlay("hide");
        }
    });
}



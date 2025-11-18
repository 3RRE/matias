$(document).ready(function () {
    ListarSalasCargo()

    setCookie("datainicial", "")

    VistaAuditoria("AlertaProgresivo/ConfiguracionAlertaProgresivoCargo", "VISTA", 0, "", 3)

    $(this).off('ifChecked', '#tabContentCargo input')
    $(document).on('ifChecked', '#tabContentCargo input', function (event) {

        event.preventDefault()

        var input = jQuery(this)
        var alt_id = jQuery(this).data("alt_id")
        var sala_id = jQuery(this).data("sala")
        var cargo_id = jQuery(this).val()

        var data = {
            Id: alt_id,
            CargoId: cargo_id,
            SalaId: sala_id
        }

        ajaxhr = $.ajax({
            url: basePath + '/AlertaProgresivo/GuardarAlertaProgresivoCargo',
            type: 'POST',
            data: JSON.stringify({ alertaCargo: data }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            cache: false,
            beforeSend: function ()
            {
                $.LoadingOverlay("show")
            },
            success: function (response)
            {
                if (response.respuesta)
                {
                    var id = response.id

                    var datainicial = {
                        sala_id,
                        alt_id,
                        cargo_id,
                    }

                    var datafinal = {

                    }

                    input.data("alt_id", id)

                    dataAuditoriaJSON(3, "AlertaProgresivo/GuardarAlertaProgresivoCargo", "AGREGAR ALERTA PROGRESIVO CARGO EN SALA", datainicial, datafinal)

                    toastr.success(response.mensaje, "Mensaje Servidor")
                }
                else
                {
                    toastr.error(response.mensaje, "Mensaje Servidor")
                }

            },
            complete: function ()
            {

                AbortRequest.close()
                $.LoadingOverlay("hide")
            }
        })
        AbortRequest.open()
    })

    $(this).off('ifUnchecked', '#tabContentCargo input');
    $(document).on('ifUnchecked', '#tabContentCargo input', function (event) {

        event.preventDefault()

        var input = jQuery(this)
        var alt_id = jQuery(this).data("alt_id")
        var sala_id = jQuery(this).data("sala")
        var cargo_id = jQuery(this).val()

        if (alt_id == 0)
        {
            toastr.error("No se puede Quitar,Error", "Mensaje Servidor")

            location.reload()

            return false
        }

        var data = {
            alerta_id: alt_id
        }

        ajaxhr = $.ajax({
            url: basePath + '/AlertaProgresivo/QuitarAlertaProgresivoCargo',
            type: 'POST',
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            cache: false,
            beforeSend: function ()
            {
                $.LoadingOverlay("show")
            },
            success: function (response)
            {
                if (response.respuesta)
                {
                    var id = alt_id

                    var datainicial = {
                        sala_id,
                        alt_id,
                        cargo_id,
                    }

                    var datafinal = {

                    }

                    input.data("alt_id", id)

                    dataAuditoriaJSON(3, "AlertaProgresivo/QuitarAlertaProgresivoCargo", "QUITAR ALERTA PROGRESIVO CARGO EN SALA", datainicial, datafinal)

                    toastr.success(response.mensaje, "Mensaje Servidor")
                }
                else
                {
                    toastr.error(response.mensaje, "Mensaje Servidor")
                }

            },
            complete: function ()
            {
                AbortRequest.close()
                $.LoadingOverlay("hide")
            }
        })
        AbortRequest.open()
    })
})

function ListarSalasCargo()
{
    var url = basePath + 'AlertaProgresivo/ListarAlertaProgresivoCargoSala'

    ajaxhr = $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function ()
        {
            $.LoadingOverlay("show")
        },
        success: function (response)
        {
            var response = response.data

            $("#tabSala").html("")
            $("#tabContentCargo").html("")

            var activo = ""

            $.each(response, function (index, valor)
            {
                var cargos = valor.cargos

                if (index == 0) { activo = "active" } else { activo = "" }

                $("#tabSala").append('<li class="' + activo + '" data-id="' + valor.CodSala + '"><a href="#' + valor.CodSala + '_" data-toggle="tab">' + valor.Nombre + '</a></li>')

                var cargodiv = ""

                $.each(cargos, function (indexRS, v) {
                    var checked = ""

                    if (v.alt_id > 0) {
                        checked = "checked"
                    }

                    cargodiv += '<div class="col-md-3 col-sm-4" style="padding-right: 4px;padding-left: 4px;padding-bottom: 4px;">' +
                        '<div style= "margin-bottom:0px" > <div class="panel-heading" style="background: blanchedalmond;  padding: 6px 6px;text-transform: uppercase;">' +
                        '<label> <input ' + checked + ' type="checkbox" value= "' + v.CargoID + '" name= "cargos[]" data-sala="' + valor.CodSala + '" data-alt_id="' + v.alt_id + '"> ' + v.Descripcion + '' +
                        '</label></div></div></div> '
                })

                if (cargodiv == "") {
                    cargodiv = "<p class='alert alert-danger'>No Hay Cargos Asignados</p>"
                }

                $("#tabContentCargo").append('<div id="' + valor.CodSala + '_" class="tab-pane ' + activo + '" data-id="' + valor.CodSala + '" data-nombre="' + valor.Nombre + '">' + cargodiv + '</div>')
            })

            $("#tabContentCargo").iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '10%'
            })
        },
        complete: function ()
        {
            AbortRequest.close()
            $.LoadingOverlay("hide")
        }
    })
    AbortRequest.open()
}
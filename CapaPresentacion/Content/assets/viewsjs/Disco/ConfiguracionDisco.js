$(document).ready(function () {
    ListarSalasCargo();
    setCookie("datainicial", "");
    VistaAuditoria("Disco/ConfiguracionAlertaDisco", "VISTA", 0, "", 3);
    $(this).off('ifChecked', '#tabContentCargo input');
    $(document).on('ifChecked', '#tabContentCargo input', function (event) {
        var input = jQuery(this);
        var alt_id = jQuery(this).data("alt_id");
        var sala_id = jQuery(this).data("sala");
        var cargo_id = jQuery(this).val();

        $.ajax({
            url: basePath + '/Disco/DiscoCargoGuardarJson/',
            type: 'POST', data: JSON.stringify({ sala_id, alt_id, cargo_id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                var id = response.id;
                if (response.respuesta) {
                    var datainicial = {
                        sala_id,
                        alt_id,
                        cargo_id,
                    };
                    var datafinal = {

                    };
                    input.data("alt_id", id);
                    dataAuditoriaJSON(3, "Disco/DiscoCargoGuardarJson", "AGREGAR ALERTA A CARGO EN SALA", datainicial, datafinal);

                    toastr.success(response.mensaje, "Mensaje Servidor");
                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }

            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    });

    $(this).off('ifUnchecked', '#tabContentCargo input');
    $(document).on('ifUnchecked', '#tabContentCargo input', function (event) {
        var input = jQuery(this);
        var alt_id = jQuery(this).data("alt_id");
        var sala_id = jQuery(this).data("sala");
        var cargo_id = jQuery(this).val();
        if (alt_id == 0) {
            toastr.error("No se puede Quitar,Error", "Mensaje Servidor");
            location.reload();
            return false;
        }
        $.ajax({
            url: basePath + '/Disco/QuitarDiscoCargoJson',
            type: 'POST', data: JSON.stringify({ sala_id,alt_id, cargo_id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                var id = response.id;
                if (response.respuesta) {
                    var datainicial = {
                        sala_id,
                        alt_id,
                        cargo_id,
                    };
                    var datafinal = {

                    };
                    input.data("alt_id", id);
                    dataAuditoriaJSON(3, "Disco/QuitarDiscoCargoJson", "QUITAR ALERTA A CARGO EN SALA", datainicial, datafinal);

                    toastr.success(response.mensaje, "Mensaje Servidor");
                }
                else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }

            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    });
});

function ListarSalasCargo() {
    var data = {};
    var url = basePath + 'Disco/CargoSalaListarJson';
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        async: false,
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            var response = response.data;
            $("#tabSala").html("");
            $("#tabContentCargo").html("");
            //debugger;
            var activo = "";
            var strippedUl = "";
            $.each(response, function (index, valor) {
                var cargos = valor.cargos;
                //debugger;
                if (index == 0) { activo = "active" } else { activo = ""; }
                if (index % 2 == 0) { strippedUl = "#f5f5f5" } else { strippedUl = "" }

                $("#tabSala").append('<li style="background-color:' + strippedUl + '" class="' + activo + '" data-id="' + valor.CodSala + '"><a href="#' + valor.CodSala + '_" data-toggle="tab">' + valor.Nombre + '</a></li>');
                var cargodiv = "";
                $.each(cargos, function (indexRS, v) {
                    var checked = "";
                    if (v.alt_id > 0) {
                        checked = "checked";
                    }

                    cargodiv += '<div class="col-sm-6 col-md-6 col-lg-4" style="padding-right: 4px;padding-left: 4px;padding-bottom: 4px;height:60px; ">' +
                        ' <div class="panel-heading"  style="background: #fff3df;border: 1px solid #cccccc;border-radius: 5px !important; text-transform: uppercase;">' +
                        '<label> <input ' + checked + ' type="checkbox" value= "' + v.CargoID + '" name= "cargos[]" data-sala="' + valor.CodSala + '" data-alt_id="' + v.alt_id + '"> ' + v.Descripcion + '' +
                        '</label></div></div> ';
                });
                if (cargodiv == "") {
                    cargodiv = "<p class='alert alert-danger'>No Hay Cargos Asignados</p>";
                }
                $("#tabContentCargo").append('<div id="' + valor.CodSala + '_" class="tab-pane ' + activo + '" data-id="' + valor.CodSala + '" data-nombre="' + valor.Nombre + '">' + cargodiv + '</div>');
            });

            $("#tabContentCargo").iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '10%'
            });

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
};
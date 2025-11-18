$(document).ready(function () {

    setCookie("datainicial", "");
    VistaAuditoria("CampaniaImpresora/AsignarImpresoras", "VISTA", 0, "", 3);

    ObtenerListaSalasRegistro();

    $('#cbo_sala_id').on('change', function (e) {
        var codsala = $(this).val();
        if (codsala != "" && codsala!=null) {
            ObtenerImpresoras(codsala);
        }
        else {
            toastr.error("Seleccione Sala", "Mensaje Servidor");
        }
    });
   
    $(this).off('ifChecked', '#tabContentUsuario input');
    $(document).on('ifChecked', '#tabContentUsuario input', function (event) {
        var input = jQuery(this);
        var impresora_id = jQuery(this).data("impresora_id");
        var usuario_id = jQuery(this).val();

        $.ajax({
            url: basePath + '/CampaniaImpresora/ImpresoraUsuarioGuardarJson/',
            type: 'POST', data: JSON.stringify({ impresora_id, usuario_id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                var id = response.id;
                if (response.respuesta) {
                    var datainicial = {
                        impresora_id,
                        usuario_id,
                    };
                    var datafinal = {

                    };
                    input.data("id", id);
                    dataAuditoriaJSON(3, "CampaniaImpresora/ImpresoraUsuarioGuardarJson", "ASIGNAR IMPRESORA A USUARIO", datainicial, datafinal);

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

    $(this).off('ifUnchecked', '#tabContentUsuario input');
    $(document).on('ifUnchecked', '#tabContentUsuario input', function (event) {
        var input = jQuery(this);
        var impresora_id = jQuery(this).data("impresora_id");
        var usuario_id = jQuery(this).val();
        var id = input.data("id");
        if (id == 0) {
            toastr.error("No se puede Quitar,Error", "Mensaje Servidor");
            location.reload();
            return false;
        }
        $.ajax({
            url: basePath + '/CampaniaImpresora/ImpresoraUsuarioQuitarJson',
            type: 'POST', data: JSON.stringify({ impresora_id, usuario_id,id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (response) {
                var id = response.id;
                if (response.respuesta) {
                    var datainicial = {
                        id,
                        impresora_id,
                        usuario_id,
                    };
                    var datafinal = {

                    };
                    input.data("id", id);
                    dataAuditoriaJSON(3, "CampaniaImpresora/ImpresoraUsuarioGuardarJson", "QUITAR IMPRESORA A USUARIO", datainicial, datafinal);
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

function ObtenerListaSalasRegistro() {
    $("#cbo_sala_id").html("");
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
            var datos = result.data;

            $.each(datos, function (index, value) {
                $("#cbo_sala_id").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cbo_sala_id").select2({
                multiple: false, placeholder: "--Seleccione--", allowClear: true,
            });
            $("#cbo_sala_id").val(null).trigger("change");
        },
        error: function (request, status, error) {
            toastr.error("Error", "Mensaje Servidor");
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
    return false;
}

function ObtenerImpresoras(codsala) {
    $("#tabImpresora").html("");
    $("#tabContentUsuario").html("");
    $.ajax({
        url: basePath+"CampaniaImpresora/ImpresoraSalaUsuarioAsignadoListarJson",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ codsala: codsala }),
        async: false,
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            var response = response.data;
            $("#tabImpresora").html("");
            $("#tabContentUsuario").html("");
            //debugger;
            var activo = "";
            $.each(response, function (index, valor) {
                var usuarios = valor.usuarios;
                //debugger;
                if (index == 0) { activo = "active" } else { activo = ""; }
                $("#tabImpresora").append('<li class="' + activo + '" data-id="' + valor.id + '"><a href="#impresora_' + valor.id + '_" data-toggle="tab">' + valor.nombre + '</a></li>');
                var usuariodiv = "";
                $.each(usuarios, function (indexRS, v) {
                    var checked = "";
                    if (v.id > 0) {
                        checked = "checked";
                    }

                    usuariodiv += '<div class="col-md-3 col-sm-4" style="padding-right: 4px;padding-left: 4px;padding-bottom: 4px;">' +
                        '<div style= "margin-bottom:0px" > <div class="panel-heading" style="background: blanchedalmond;  padding: 6px 6px;text-transform: uppercase;">' +
                        '<label> <input ' + checked + ' type="checkbox" value= "' + v.UsuarioID + '" name= "usuarios[]" data-impresora_id="' + valor.id + '" data-id="' + v.id + '"> ' + v.UsuarioNombre + '' +
                        '</label></div></div></div> ';
                });
                if (usuariodiv == "") {
                    usuariodiv = "<p class='alert alert-danger'>No Hay Usuario</p>";
                }
                $("#tabContentUsuario").append('<div id="impresora_' + valor.id + '_" class="tab-pane ' + activo + '" data-id="' + valor.sala_id + '" data-nombre="' + valor.nombre + '">' + usuariodiv + '</div>');
            });

            if (response.length == 0) {
                $("#tabContentUsuario").html("<p class='alert alert-danger'>No Hay Impresoras Registradas</p>");
            }

            $("#tabContentUsuario").iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '10%'
            });

        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
    return false;
}
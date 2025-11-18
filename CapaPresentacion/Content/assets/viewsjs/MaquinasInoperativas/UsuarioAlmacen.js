VistaAuditoria("MIAlmacen/UsuarioAlmacenVista", "VISTA", 0, "", 3);
$(document).ready(function () {
    //circloidBlocks({
    //    colorcollapsed: "red"
    //});
    ListarAlmacenesSalas();
    ListarAlmacenesSalasGrid();
    llenarSelect(basePath + '/Usuario/UsuarioListadoxSalaJson', "", "cboUsuario", "UsuarioID", "UsuarioNombre", "");
    $("#cboUsuario").select2();

    $(this).off('change', '#cboUsuario');
    $('#cboUsuario').on('change', function () {
        checkboxSalaUsuarioData($(this).val())
    });

    $(this).off('ifChecked', '#tabContentAlmacenes input, #almacenesGrid input');
    $(document).on('ifChecked', '#tabContentAlmacenes input, #almacenesGrid input', function (event) {
        var idUsuario = $('#cboUsuario').val();

        var usuarioNombre = jQuery("#cboUsuario option:selected").text();
        var nombrealmacen = $(this).parent().parent().text();

        var checkeds = $("input:checked");

        var almacenesiF = [];
        var almacennF = [];
        jQuery.each(checkeds, function () {
            almacennF.push($(this).parent().parent().text());
            almacenesiF.push($(this).val());
        });
        almacennF.push(nombrealmacen);
        almacenesiF.push(jQuery(this).val());

        var almacenesi = [];
        var almacenn = [];
        jQuery.each(checkeds, function () {
            almacenn.push($(this).parent().parent().text());
            almacenesi.push($(this).val());
        });


        if (idUsuario != '') {
            var idSala = jQuery(this).attr('data-sala');
            var idAlmacen = jQuery(this).val();
            $.ajax({
                url: basePath + '/MIAlmacen/GetCodAlmacenCodUsuario/',
                type: 'POST',
                data: JSON.stringify({ codUsuario: idUsuario, codAlmacen: idAlmacen }),
                async: false,
                contentType: "application/json",
                success: function (response) {
                    var data = response.data.CodAlmacen;
                    var tipo = "";
                    var id = "";
                    var almacenUsuario = [];
                    if (data == 0) {
                        $.ajax({
                            url: basePath + '/MIAlmacen/AsignarUsuarioAlmacen',
                            type: "POST",
                            contentType: "application/json",
                            data: JSON.stringify({ codUsuario: idUsuario, codAlmacen: idAlmacen }),
                            success: function (response) {
                                if (response.respuesta == true) {

                                    var datainicial = {
                                        UsuarioID: idUsuario,
                                        UsuarioNombre: usuarioNombre,
                                        CodAlmacenesAsignados: almacenesiF.toString(),
                                        AlmacenesAsignadas: almacennF.toString(),

                                    };
                                    var datafinal = {
                                        UsuarioID: idUsuario,
                                        UsuarioNombre: usuarioNombre,
                                        CodAlmacenesAsignados: almacenesi.toString(),
                                        AlmacenesAsignadas: almacenn.toString(),
                                        CodAlmacenRemovido: idAlmacen,
                                        AlmacenRemovido: nombrealmacen
                                    };
                                    dataAuditoriaJSON(3, "MIAlmacen/AsignarUsuarioAlmacen", "AGREGAR ALMACEN", datainicial, datafinal);

                                    //change checked
                                    changeCheckedSalaAlmacen(idSala, idAlmacen, true)

                                    //reload total
                                    totales()
                                    totalesGrid()
                                    totalAlmacenesSala()

                                    toastr.success("Se Asigno Almacen , Correctamente", "Mensaje Servidor");
                                }
                                else {
                                    toastr.success("No se asigno el Almacen , Error", "Mensaje Servidor");
                                }
                            },
                        });
                    }

                    almacenUsuario.push({
                        Id: id,
                        UsuarioId: idUsuario,
                        CodAlmacen: idAlmacen,
                        Estado: true,
                    });



                },
            });
        }
        else {
            toastr.warning("Seleccione un Usuario", "Mensaje Servidor");
        }
    });

    $(this).off('ifUnchecked', '#tabContentAlmacenes input, #almacenesGrid input');
    $(document).on('ifUnchecked', '#tabContentAlmacenes input, #almacenesGrid input', function (event) {
        var idUsuario = $('#cboUsuario').val();
        var usuarioNombre = jQuery("#cboUsuario option:selected").text();
        var nombrealmacen = $(this).parent().parent().text();

        var checkeds = $("input:checked");

        var almacenesiF = [];
        var almacennF = [];
        jQuery.each(checkeds, function () {
            almacennF.push($(this).parent().parent().text());
            almacenesiF.push($(this).val());
        });
        almacennF.push(nombrealmacen);
        almacenesiF.push(jQuery(this).val());

        var almacenesi = [];
        var almacenn = [];
        jQuery.each(checkeds, function () {
            almacenn.push($(this).parent().parent().text());
            almacenesi.push($(this).val());
        });



        if (idUsuario != '') {
            var idSala = jQuery(this).attr('data-sala');
            var idAlmacen = jQuery(this).val();
            //var url_ = remoteService + '/odata/usuarioalmacenes?$filter=SalaId eq ' + idSala + ' and UsuarioId eq ' + idUsuario;
            //console.log("unchecked")
            $.ajax({
                url: basePath + '/MIAlmacen/GetCodAlmacenCodUsuario/',
                type: 'POST',
                data: JSON.stringify({ codUsuario: idUsuario, codAlmacen: idAlmacen }),
                async: false,
                contentType: "application/json",
                success: function (response) {
                    var data = response.data.CodAlmacen;
                    var id = "";
                    var almacenUsuario = [];
                    if (data == 0) {
                        toastr.error("No se Encontro Registro,Actualice la pagina", "Mensaje Servidor");
                    }
                    else {

                        id = data.Id;
                        //console.log(data, "aaaaaaaaaaaaaaa" + data.Id)
                        almacenUsuario.push({
                            Id: id,
                            UsuarioId: idUsuario,
                            SalaId: idAlmacen,
                            Estado: false,
                        });


                        $.ajax({
                            url: basePath + '/MIAlmacen/QuitarUsuarioAlmacen',
                            type: "POST",
                            contentType: "application/json",
                            data: JSON.stringify({ codUsuario: idUsuario, codAlmacen: idAlmacen }),
                            success: function (response) {
                                if (response.respuesta == true) {

                                    var datainicial = {
                                        UsuarioID: idUsuario,
                                        UsuarioNombre: usuarioNombre,
                                        CodAlmacenesAsignados: almacenesiF.toString(),
                                        AlmacenesAsignadas: almacennF.toString(),

                                    };
                                    var datafinal = {
                                        UsuarioID: idUsuario,
                                        UsuarioNombre: usuarioNombre,
                                        CodAlmacenesAsignados: almacenesi.toString(),
                                        AlmacenesAsignadas: almacenn.toString(),
                                        CodAlmacenRemovido: idAlmacen,
                                        AlmacenRemovido: nombrealmacen
                                    };
                                    dataAuditoriaJSON(3, "MIAlmacen/QuitarUsuarioAlmacen", "QUITAR ALMACEN", datainicial, datafinal);

                                    //change checked
                                    changeCheckedSalaAlmacen(idSala, idAlmacen, false)

                                    //reload total
                                    totales()
                                    totalesGrid()
                                    totalAlmacenesSala()

                                    toastr.success("Se Quito Almacen , Correctamente", "Mensaje Servidor");
                                }
                                else {
                                    toastr.success("No se quito el Almacen , Error", "Mensaje Servidor");
                                }
                            },
                        });

                    }

                },
            });

        }
        else {
            toastr.warning("Seleccione un Usuario", "Mensaje Servidor");
        }
    });

    $(document).on('change', '.checkbox-grid input', function (event) {
        $("#result").toggle();
        $("#almacenesGridContent").toggle();
    })
});

function checkboxSalaUsuarioData(id) {
    var usuarioId = id;
    if (usuarioId == '') {
        toastr.warning("Seleccione un Usuario", "Mensaje Servidor");
        limpiarRoles();
        return false;
    }
    var data = { usuarioId: usuarioId };
    $.ajax({
        url: basePath + '/Usuario/UsuarioEmpleadoIDObtenerJson/',
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(data),
        //url: remoteService + '/odata/usuarios?$select=Id,Nombre,EmpleadoId,ClienteId,TipoUsuarioId&$Filter=Estado eq true and Id eq ' + usuarioId,
        contentType: "application/json",
        success: function (data) {
            var tipoUsuarioId = data.respuesta.TipoUsuarioID;
            var personaId = "";
            var tipoPersona = "";
            if (tipoUsuarioId == 1) {
                personaId = data.respuesta.ClienteId;
                tipoPersona = "Cliente";
            }
            else {
                personaId = data.respuesta.EmpleadoID;
                tipoPersona = "Empleado";
            }
            if (personaId != "") {
                $.ajax({
                    url: basePath + 'Empleado/EmpleadoIdObtenerJson',
                    type: 'POST',
                    contentType: "application/json",
                    data: JSON.stringify({ empleadiId: personaId }),
                    success: function (data) {
                        $("#nombrePersona").html(data.respuesta.Nombres + " " + data.respuesta.ApellidosPaterno + " " + data.respuesta.ApellidosMaterno);
                        $("#tipoPersona").html(tipoPersona);
                        $("#telefonoPersona").html(data.respuesta.Telefono);
                        $("#celularPersona").html(data.respuesta.Movil);
                        $("#correoPersona").html(data.respuesta.MailJob);
                        $("#direccionPersona").html(data.respuesta.Direccion);
                        if (data.respuesta.EstadoEmpleado) {
                            $("#estadoPersona").html("Activo");
                            $("#estadoPersona").css("color", "blue")
                        } else {
                            $("#estadoPersona").html("Inactivo");
                            $("#estadoPersona").css("color", "red")
                        }

                    }
                });
            }
        }
    });

    limpiarAlmacenes();
    listaAlmacenUsuario(usuarioId);
    listaAlmacenUsuarioGrid(usuarioId);
    totales();
    totalesGrid();
    totalAlmacenesSala();
}

function listaAlmacenUsuario(usuarioId) {
    var divs = $("#tabContentAlmacenes div.tab-pane");
    $('#tabContentAlmacenes div.tab-pane').iCheck("destroy");
    $('input:checkbox').removeAttr('checked');
    var almacenUsuario = "";

    $.ajax({
        url: basePath + '/MIAlmacen/GetAllAlmacenxUsuario/',
        //url: remoteService + '/odata/usuarioalmacenes?$filter=UsuarioId eq ' + usuarioId + ' and Estado eq true ',
        type: 'POST',
        data: JSON.stringify({ codUsuario: usuarioId }),
        async: false,
        contentType: "application/json",
        success: function (response) {
            if (response.respuesta) {
                almacenUsuario = response.data;
            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
    });

    if (almacenUsuario.length > 0) {
        $.each(divs, function (i, element) {
            var idSala = parseInt($(element).data("id"));
            var nombreSala = $(element).data("nombre");
            var inputs = $('input[data-sala="' + idSala + '"]');

            //$(element).iCheck("destroy");

            $.each(inputs, function (e, htmlinput) {
                htmlinput.checked = false;
                $.each(almacenUsuario, function (index, value) {
                    var id = value.CodAlmacen;
                    if (id == htmlinput.value) {
                        htmlinput.checked = true;
                        return
                    }
                });
            });
            $(element).iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '2%'
            });
        });
    } else {
        $('#tabContentAlmacenes div.tab-pane').iCheck({
            checkboxClass: 'icheckbox_square-blue',
            radioClass: 'iradio_square-red',
            increaseArea: '2%'
        });
    }


}

function listaAlmacenUsuarioGrid(usuarioId) {
    var divGridItem = $("#almacenesGrid .grid-item");
    $('input:checkbox').removeAttr('checked');
    var almacenUsuario = "";
    $.ajax({
        url: basePath + '/MIAlmacen/GetAllAlmacenxUsuario/',
        type: 'POST',
        data: JSON.stringify({ codUsuario: usuarioId }),
        async: false,
        contentType: "application/json",
        success: function (response) {
            if (response.respuesta) {
                almacenUsuario = response.data;
            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
    });

    if (almacenUsuario.length > 0) {
        $.each(divGridItem, function (i, element) {
            var idSala = parseInt($(element).data("id"));
            var inputs = $('input[data-sala="' + idSala + '"]');

            $.each(inputs, function (e, htmlinput) {
                htmlinput.checked = false;
                $.each(almacenUsuario, function (index, value) {
                    var id = value.CodAlmacen;
                    if (id == htmlinput.value) {
                        htmlinput.checked = true;
                        return
                    }
                });
            });

            $(element).iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '2%'
            });
        });
    } else {
        $('#almacenesGrid .grid-item').iCheck({
            checkboxClass: 'icheckbox_square-blue',
            radioClass: 'iradio_square-red',
            increaseArea: '2%'
        });
    }


}

function ListarAlmacenesSalas() {
    var data = {};
    var urlS = basePath + 'MIAlmacen/ListarAlmacenActiveJson';
    var almacenes = [];
    $.ajax({
        url: urlS,
        type: "POST",
        contentType: "application/json",
        data: null,
        async: false,
        success: function (response) {
            var almacenes_ = response.data;
            almacenes.push(almacenes_);
            ////debugger;
        },
    });

    var url = basePath + 'Sala/ListadoSalaPorUsuarioJson';
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        async: false,
        success: function (response) {
            var salas = response.data;
            $("#tabsSalas").html("");
            $("#tabContentAlmacenes").html("");
            //debugger;
            var activo = "";
            $.each(salas, function (index, valor) {
                //debugger;
                if (index == 0) { activo = "active" } else { activo = ""; }
                $("#tabsSalas").append('<li class="liItemE ' + activo + '" data-id="' + valor.CodSala + '"><a href="#' + valor.CodSala + '_" data-toggle="tab" class="d-flex linkItemE"><span class="name-company">' + valor.Nombre + '</span><span class="totalAlmacenesSalaSpan">0/0</span></a></li>');
                var almacendiv = "";
                $.each(almacenes[0], function (indexRS, valorAlmacen) {
                    if (valorAlmacen.CodSala == valor.CodSala) {
                        //debugger;
                        almacendiv += '<div class="col-md-3 col-sm-4" style="padding-right: 4px;padding-left: 4px;padding-bottom: 4px;">' +
                            '<div style= "margin-bottom:0px" > <div class="panel-heading" style="background: blanchedalmond;  padding: 6px 6px;text-transform: uppercase;">' +
                            '<label> <input type="checkbox" value= "' + valorAlmacen.CodAlmacen + '" name= "almacenes[]" data-sala="' + valor.CodSala + '" data-code="' + valor.CodSala + valorAlmacen.CodAlmacen + '"> ' + valorAlmacen.Nombre +
                            '</label></div></div></div> ';
                    }
                });
                if (almacendiv == "") {
                    almacendiv = "<p class='alert alert-danger'>No Hay Almacenes Asignados</p>";
                }
                $("#tabContentAlmacenes").append('<div id="' + valor.CodSala + '_" class="tab-pane ' + activo + '" data-id="' + valor.CodSala + '" data-nombre="' + valor.Nombre + '">' + almacendiv + '</div>');
            });

            $("#tabContentAlmacenes").iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '10%'
            });

        },
    });
};

function ListarAlmacenesSalasGrid() {
    var data = {};
    var urlS = basePath + 'MIAlmacen/ListarAlmacenActiveJson';
    var almacenes = [];
    $.ajax({
        url: urlS,
        type: "POST",
        contentType: "application/json",
        data: null,
        async: false,
        success: function (response) {
            var almacenes_ = response.data;
            almacenes.push(almacenes_);
        },
    });

    var url = basePath + 'Sala/ListadoSalaPorUsuarioJson';
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        async: false,
        success: function (response) {
            var salas = response.data;
            $("#almacenesGrid").html("");

            $.each(salas, function (index, valor) {

                var almacendiv = "";

                $.each(almacenes[0], function (indexRS, valorAlmacen) {
                    if (valorAlmacen.CodSala == valor.CodSala) {

                        almacendiv = '<div class="col-md-3 col-sm-4 grid-item" style="padding-right: 4px;padding-left: 4px;padding-bottom: 4px;" data-id="' + valor.CodSala + '">' +
                            '<div style= "margin-bottom:0px" > <div class="panel-heading" style="background: blanchedalmond;  padding: 6px 6px;text-transform: uppercase;">' +
                            '<label> <input type="checkbox" value= "' + valorAlmacen.CodAlmacen + '" name= "almacenes[]" data-sala="' + valor.CodSala + '" data-code="' + valor.CodSala + valorAlmacen.CodAlmacen + '"> ' + valorAlmacen.Nombre +
                            '</label><span style="display:block;font-size:10px;margin-top:5px">' + valor.Nombre + '</span></div></div></div> ';

                        $("#almacenesGrid").append(almacendiv);
                    }
                });

            });

            $("#almacenesGrid").iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '10%'
            });

        },
    });
};


function limpiarAlmacenes() {
    $('#tabContentAlmacenes div.tab-pane').iCheck("destroy");
    $('input:checkbox').removeAttr('checked');

    $("#nombrePersona").html("");
    $("#tipoPersona").html("");
    $("#telefonoPersona").html("");
    $("#celularPersona").html("");
    $("#correoPersona").html("");
    $("#direccionPersona").html("");
    $("#estadoPersona").html("");

    $('#tabContentAlmacenes div.tab-pane').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-red',
        increaseArea: '2%'
    });
}

function limpiarRoles() {
    $('#tabContentRoles div.tab-pane').iCheck("destroy");
    $('#tabContentRoles input:checkbox').removeAttr('checked');

    $('#tabContentRoles div.tab-pane').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-red',
        increaseArea: '2%'
    });
    $('#tabUsuariosLista').iCheck("destroy");
    $('#tabUsuariosLista input:checkbox').removeAttr('checked');

    $('#tabUsuariosLista').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-red',
        increaseArea: '2%'
    });
    $("#asigSpan").html("")
}

function totales() {
    var totalno = $('#tabContentAlmacenes .tab-pane input:checkbox:not(:checked)').length;
    var total = $('#tabContentAlmacenes .tab-pane input:checkbox').length;
    var totalsi = $('#tabContentAlmacenes .tab-pane input:checkbox:checked').length;

    if (totalno == "0") {
        $(".divfaltanTotal").hide();
    } else {
        $(".totalAlmacenesSpanFaltan_list").text(totalno);
        $(".divfaltanTotal").show();
    }

    $(".divTotal").show();

    $(".totalAlmacenesSpan_list").text(totalsi + "/" + total);
}

function totalesGrid() {
    var totalno = $('#almacenesGrid .grid-item input:checkbox:not(:checked)').length;
    var total = $('#almacenesGrid .grid-item input:checkbox').length;
    var totalsi = $('#almacenesGrid .grid-item input:checkbox:checked').length;

    if (totalno == "0") {
        $(".divfaltanTotal").hide();
    } else {
        $(".totalAlmacenesSpanFaltan").text(totalno);
        $(".divfaltanTotal").show();
    }

    $(".divTotal").show();

    $(".totalAlmacenesSpan").text(totalsi + "/" + total);
}

function totalAlmacenesSala() {

    var checkeds = $("#tabsSalas li");
    var almacenesCheckbox = 0;
    var almacenesCheckboxChecked = 0;
    var almacenesCheckboxNotChecked = 0;

    jQuery.each(checkeds, function () {
        var idSala = $(this).attr('data-id');
        almacenesCheckbox = $(`#tabContentAlmacenes .tab-pane input[data-sala="${idSala}"]:checkbox`).length;
        almacenesCheckboxChecked = $(`#tabContentAlmacenes .tab-pane input[data-sala="${idSala}"]:checkbox:checked`).length;
        almacenesCheckboxNotChecked = $(`#tabContentAlmacenes .tab-pane input[data-sala="${idSala}"]:checkbox:not(:checked)`).length;

        var tabSala = $(`#tabsSalas li[data-id="${idSala}"]`);
        tabSala.find(".totalAlmacenesSalaSpan").text(almacenesCheckboxChecked + "/" + almacenesCheckbox)

        tabSala.css('background', 'transparent');
        tabSala.find('.totalAlmacenesSalaSpan').removeClass('lie-success');

        if (almacenesCheckboxChecked > 0) {
            tabSala.css('background', '#EEE');
            tabSala.css('border', '1px solid #FFF');
            tabSala.find('.totalAlmacenesSalaSpan').addClass('lie-success');
        }
    });


}

function changeCheckedSalaAlmacen(idSala, idAlmacen, checked) {

    var code = idSala + idAlmacen;

    $(`input[data-code="${code}"]`).prop('checked', checked)
    $(`input[data-code="${code}"]`).parent().removeClass('checked')

    if (checked) {
        $(`input[data-code="${code}"]`).parent().addClass('checked')
    }
}
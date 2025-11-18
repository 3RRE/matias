VistaAuditoria("MaquinaAnfitriona/Index", "VISTA", 0, "", 3);
const cboSala = $("#cboSala");
const cboUsuario = $('#cboUsuario');

//const getUsuarios = (codSala) => {
//    llenarSelect2Agrupado({
//        url: `${basePath}MaquinaAnfitriona/GetEmpleadosAnfitriona`,
//        data: {codSala},
//        select: "cboUsuario",
//        campoId: "UsuarioID",
//        campoAgrupacion: "RolNombre",
//        campoValor: "UsuarioNombre",
//    })
//}
$(document).ready(function () {


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

    getSalas();

    cboSala.on('change', () => {
        const codSala = cboSala.val();
        if (!codSala) {
            toastr.warning("Seleccione una sala", "Aviso")
            return;
        }
        getUsers(codSala)
        //getUsuarios(codSala)
        PaintUI(codSala)
    });



    //ListarSalasEmpresas();
    //ListarSalasEmpresasGrid();

    $(this).off('ifChecked', '#tabContentSalas input, #salasGrid input');
    $(document).on('ifChecked', '#tabContentSalas input, #salasGrid input', function (event) {
        var idUsuario = $('#cboUsuario').val();

        var usuarioNombre = jQuery("#cboUsuario option:selected").text();
        var nombresala = $(this).parent().parent().text();

        var checkeds = $("input:checked");

        var salasiF = [];
        var salanF = [];
        jQuery.each(checkeds, function () {
            salanF.push($(this).parent().parent().text());
            salasiF.push($(this).val());
        });
        salanF.push(nombresala);
        salasiF.push(jQuery(this).val());

        var salasi = [];
        var salan = [];
        jQuery.each(checkeds, function () {
            salan.push($(this).parent().parent().text());
            salasi.push($(this).val());
        });


        if (idUsuario != '') {
            var idEmpresa = jQuery(this).attr('data-empresa');
            var idSala = jQuery(this).val();
            $.ajax({
                url: basePath + '/UsuarioSala/UsuarioSalaIdListarJson/',
                type: 'GET', data: { usuarioId: idUsuario, salaId: idSala },
                async: false,
                contentType: "application/json",
                success: function (response) {
                    var data = response.data.SalaId;
                    var tipo = "";
                    var id = "";
                    var salaUsuario = [];
                    if (data == 0) {
                        $.ajax({
                            url: basePath + '/MaquinaAnfitriona/MaquinaAnfitrionaAsginar',
                            type: "POST",
                            contentType: "application/json",
                            data: JSON.stringify({ usuarioId: idUsuario, salaId: idSala }),
                            success: function (response) {
                                if (response.respuesta == true) {

                                    var datainicial = {
                                        UsuarioID: idUsuario,
                                        UsuarioNombre: usuarioNombre,
                                        CodSalasAsignadas: salasiF.toString(),
                                        SalasAsignadas: salanF.toString(),

                                    };
                                    var datafinal = {
                                        UsuarioID: idUsuario,
                                        UsuarioNombre: usuarioNombre,
                                        CodSalasAsignadas: salasi.toString(),
                                        SalasAsignadas: salan.toString(),
                                        CodSalaRemovida: idSala,
                                        SalaRemovida: nombresala
                                    };
                                    dataAuditoriaJSON(3, "MaquinaAnfitriona/MaquinaAnfitrionaAsginar", "AGREGAR MAQUINAS", datainicial, datafinal);

                                    //change checked
                                    changeCheckedEmpresaSala(idEmpresa, idSala, true)

                                    //reload total
                                    totales()
                                    totalesGrid()
                                    totalSalasEmpresa()

                                    toastr.success("Se Asigno Sala , Correctamente", "Mensaje Servidor");
                                }
                                else {
                                    toastr.success("No se asigno la Sala , Error", "Mensaje Servidor");
                                }
                            },
                        });
                    }

                    salaUsuario.push({
                        Id: id,
                        UsuarioId: idUsuario,
                        SalaId: idSala,
                        Estado: true,
                    });



                },
            });
        }
        else {
            toastr.warning("Seleccione un Usuario", "Mensaje Servidor");
        }
    });

    $(this).off('ifUnchecked', '#tabContentSalas input, #salasGrid input');
    $(document).on('ifUnchecked', '#tabContentSalas input, #salasGrid input', function (event) {
        var idUsuario = $('#cboUsuario').val();
        var usuarioNombre = jQuery("#cboUsuario option:selected").text();
        var nombresala = $(this).parent().parent().text();

        var checkeds = $("input:checked");

        var salasiF = [];
        var salanF = [];
        jQuery.each(checkeds, function () {
            salanF.push($(this).parent().parent().text());
            salasiF.push($(this).val());
        });
        salanF.push(nombresala);
        salasiF.push(jQuery(this).val());

        var salasi = [];
        var salan = [];
        jQuery.each(checkeds, function () {
            salan.push($(this).parent().parent().text());
            salasi.push($(this).val());
        });



        if (idUsuario != '') {
            var idEmpresa = jQuery(this).attr('data-empresa');
            var idSala = jQuery(this).val();
            //var url_ = remoteService + '/odata/usuariosalas?$filter=SalaId eq ' + idSala + ' and UsuarioId eq ' + idUsuario;
            //console.log("unchecked")
            $.ajax({
                url: basePath + '/UsuarioSala/UsuarioSalaIdListarJson/',
                type: 'GET',
                data: { usuarioId: idUsuario, salaId: idSala },
                async: false,
                contentType: "application/json",
                success: function (response) {
                    var data = response.data.SalaId;
                    var id = "";
                    var salaUsuario = [];
                    if (data == 0) {
                        toastr.error("No se Encontro Registro,Actualice la pagina", "Mensaje Servidor");
                    }
                    else {

                        id = data.Id;
                        //console.log(data, "aaaaaaaaaaaaaaa" + data.Id)
                        salaUsuario.push({
                            Id: id,
                            UsuarioId: idUsuario,
                            SalaId: idSala,
                            Estado: false,
                        });


                        $.ajax({
                            url: basePath + '/MaquinaAnfitriona/MaquinaAnfitrionaAsginar',
                            type: "POST",
                            contentType: "application/json",
                            data: JSON.stringify({ usuarioId: idUsuario, salaId: idSala }),
                            success: function (response) {
                                if (response.respuesta == true) {

                                    var datainicial = {
                                        UsuarioID: idUsuario,
                                        UsuarioNombre: usuarioNombre,
                                        CodSalasAsignadas: salasiF.toString(),
                                        SalasAsignadas: salanF.toString(),

                                    };
                                    var datafinal = {
                                        UsuarioID: idUsuario,
                                        UsuarioNombre: usuarioNombre,
                                        CodSalasAsignadas: salasi.toString(),
                                        SalasAsignadas: salan.toString(),
                                        CodSalaRemovida: idSala,
                                        SalaRemovida: nombresala
                                    };
                                    dataAuditoriaJSON(3, "MaquinaAnfitriona/MaquinaAnfitrionaAsginar", "QUITAR MAQUINAS", datainicial, datafinal);

                                    //change checked
                                    changeCheckedEmpresaSala(idEmpresa, idSala, false)

                                    //reload total
                                    totales()
                                    totalesGrid()
                                    totalSalasEmpresa()

                                    toastr.success("Se quito la Sala asignada , Correctamente", "Mensaje Servidor");
                                }
                                else {
                                    toastr.success("No se Encontro Registro,Actualice la pagina", "Mensaje Servidor");
                                }
                            },
                        });


                        //                        $.ajax({
                        //    url: remoteService + '/api/usuariosalas',
                        //                            type: 'PUT',

                        //                            contentType: "application/json",
                        //                            data: JSON.stringify(salaUsuario),
                        //                            success: function (response) {
                        //    toastr.warning("Se Quito Sala Asignada , Correctamente", "Mensaje Servidor");
                        //},
                        //                        });
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
        $("#salasGridContent").toggle();
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
                            $("#estadoPersona").css("color", "green")
                        } else {
                            $("#estadoPersona").html("Inactivo");
                            $("#estadoPersona").css("color", "red")
                        }

                    }
                });
            }
        }
    });

    limpiarSalas();
    listaSalaUsuario(usuarioId);
    listaSalaUsuarioGrid(usuarioId);
    totales();
    totalesGrid();
    totalSalasEmpresa();
}

function listaSalaUsuario(usuarioId) {
    var divs = $("#tabContentSalas div.tab-pane");
    $('#tabContentSalas div.tab-pane').iCheck("destroy");
    $('input:checkbox').removeAttr('checked');
    var salaUsuario = "";
    $.ajax({
        url: basePath + '/UsuarioSala/UsuarioSalasListarJson/',
        //url: remoteService + '/odata/usuariosalas?$filter=UsuarioId eq ' + usuarioId + ' and Estado eq true ',
        type: 'GET',
        data: { usuarioId: usuarioId },
        async: false,
        contentType: "application/json",
        success: function (response) {
            if (response.data.length > 0) {
                salaUsuario = response.data;
            } else {
                toastr.error("No se Encontraron Salas Asignadas", "Mensaje Servidor");
            }
        },
    });

    if (salaUsuario.length > 0) {
        $.each(divs, function (i, element) {
            var idEmpresa = parseInt($(element).data("id"));
            var nombreEmpresa = $(element).data("nombre");
            var inputs = $('input[data-empresa="' + idEmpresa + '"]');

            //$(element).iCheck("destroy");

            $.each(inputs, function (e, htmlinput) {
                htmlinput.checked = false;
                $.each(salaUsuario, function (index, value) {
                    var id = value.SalaId;
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
        $('#tabContentSalas div.tab-pane').iCheck({
            checkboxClass: 'icheckbox_square-blue',
            radioClass: 'iradio_square-red',
            increaseArea: '2%'
        });
    }


}

function listaSalaUsuarioGrid(usuarioId) {
    var divGridItem = $("#salasGrid .grid-item");
    $('input:checkbox').removeAttr('checked');
    var salaUsuario = "";
    $.ajax({
        url: basePath + '/UsuarioSala/UsuarioSalasListarJson/',
        type: 'GET',
        data: { usuarioId: usuarioId },
        async: false,
        contentType: "application/json",
        success: function (response) {
            if (response.data.length > 0) {
                salaUsuario = response.data;
            } else {
                toastr.error("No se Encontraron Salas Asignadas", "Mensaje Servidor");
            }
        },
    });

    if (salaUsuario.length > 0) {
        $.each(divGridItem, function (i, element) {
            var idEmpresa = parseInt($(element).data("id"));
            var inputs = $('input[data-empresa="' + idEmpresa + '"]');

            $.each(inputs, function (e, htmlinput) {
                htmlinput.checked = false;
                $.each(salaUsuario, function (index, value) {
                    var id = value.SalaId;
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
        $('#salasGrid .grid-item').iCheck({
            checkboxClass: 'icheckbox_square-blue',
            radioClass: 'iradio_square-red',
            increaseArea: '2%'
        });
    }


}

function ListarSalasEmpresas() {
    var data = {};
    var urlS = basePath + 'Sala/ListadoSala';
    var salas = [];
    $.ajax({
        url: urlS,
        type: "POST",
        contentType: "application/json",
        data: null,
        async: false,
        success: function (response) {
            var salas_ = response.data;
            salas.push(salas_);
            ////debugger;
        },
    });

    var url = basePath + 'Empresa/ListadoEmpresa';
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        async: false,
        success: function (response) {
            var empresas = response.data;
            $("#tabsEmpresas").html("");
            $("#tabContentSalas").html("");
            //debugger;
            var activo = "";
            $.each(empresas, function (index, valor) {
                //debugger;
                if (index == 0) { activo = "active" } else { activo = ""; }
                $("#tabsEmpresas").append('<li class="liItemE ' + activo + '" data-id="' + valor.CodEmpresa + '"><a href="#' + valor.CodEmpresa + '_" data-toggle="tab" class="d-flex linkItemE"><span class="name-company">' + valor.RazonSocial + '</span><span class="totalSalasEmpresaSpan">0/0</span></a></li>');
                var saladiv = "";
                $.each(salas[0], function (indexRS, valorSala) {
                    if (valorSala.CodEmpresa == valor.CodEmpresa) {
                        //debugger;
                        saladiv += '<div class="col-md-3 col-sm-4" style="padding-right: 4px;padding-left: 4px;padding-bottom: 4px;">' +
                            '<div style= "margin-bottom:0px" > <div class="panel-heading" style="background: blanchedalmond;  padding: 6px 6px;text-transform: uppercase;">' +
                            '<label> <input type="checkbox" value= "' + valorSala.CodSala + '" name= "salas[]" data-empresa="' + valor.CodEmpresa + '" data-code="' + valor.CodEmpresa + valorSala.CodSala + '"> ' + valorSala.Nombre +
                            '</label></div></div></div> ';
                    }
                });
                if (saladiv == "") {
                    saladiv = "<p class='alert alert-danger'>No Hay Salas Asignados</p>";
                }
                $("#tabContentSalas").append('<div id="' + valor.CodEmpresa + '_" class="tab-pane ' + activo + '" data-id="' + valor.CodEmpresa + '" data-nombre="' + valor.RazonSocial + '">' + saladiv + '</div>');
            });

            $("#tabContentSalas").iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '10%'
            });

        },
    });
};

function ListarSalasEmpresasGrid() {
    var data = {};
    var urlS = basePath + 'Sala/ListadoSala';
    var salas = [];
    $.ajax({
        url: urlS,
        type: "POST",
        contentType: "application/json",
        data: null,
        async: false,
        success: function (response) {
            var salas_ = response.data;
            salas.push(salas_);
        },
    });

    var url = basePath + 'Empresa/ListadoEmpresa';
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        async: false,
        success: function (response) {
            var empresas = response.data;
            $("#salasGrid").html("");

            $.each(empresas, function (index, valor) {

                var saladiv = "";

                $.each(salas[0], function (indexRS, valorSala) {
                    if (valorSala.CodEmpresa == valor.CodEmpresa) {

                        saladiv = '<div class="col-md-3 col-sm-4 grid-item" style="padding-right: 4px;padding-left: 4px;padding-bottom: 4px;" data-id="' + valor.CodEmpresa + '">' +
                            '<div style= "margin-bottom:0px" > <div class="panel-heading" style="background: blanchedalmond;  padding: 6px 6px;text-transform: uppercase;">' +
                            '<label> <input type="checkbox" value= "' + valorSala.CodSala + '" name= "salas[]" data-empresa="' + valor.CodEmpresa + '" data-code="' + valor.CodEmpresa + valorSala.CodSala + '"> ' + valorSala.Nombre +
                            '</label><span style="display:block;font-size:10px;margin-top:5px">' + valor.RazonSocial + '</span></div></div></div> ';

                        $("#salasGrid").append(saladiv);
                    }
                });

            });

            $("#salasGrid").iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-red',
                increaseArea: '10%'
            });

        },
    });
};


function limpiarSalas() {
    $('#tabContentSalas div.tab-pane').iCheck("destroy");
    $('input:checkbox').removeAttr('checked');

    $("#nombrePersona").html("");
    $("#tipoPersona").html("");
    $("#telefonoPersona").html("");
    $("#celularPersona").html("");
    $("#correoPersona").html("");
    $("#direccionPersona").html("");
    $("#estadoPersona").html("");

    $('#tabContentSalas div.tab-pane').iCheck({
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
    var totalno = $('#tabContentSalas .tab-pane input:checkbox:not(:checked)').length;
    var total = $('#tabContentSalas .tab-pane input:checkbox').length;
    var totalsi = $('#tabContentSalas .tab-pane input:checkbox:checked').length;

    if (totalno == "0") {
        $(".divfaltanTotal").hide();
    } else {
        $(".totalSalasSpanFaltan_list").text(totalno);
        $(".divfaltanTotal").show();
    }

    $(".divTotal").show();

    $(".totalSalasSpan_list").text(totalsi + "/" + total);
}

function totalesGrid() {
    var totalno = $('#salasGrid .grid-item input:checkbox:not(:checked)').length;
    var total = $('#salasGrid .grid-item input:checkbox').length;
    var totalsi = $('#salasGrid .grid-item input:checkbox:checked').length;

    if (totalno == "0") {
        $(".divfaltanTotal").hide();
    } else {
        $(".totalSalasSpanFaltan").text(totalno);
        $(".divfaltanTotal").show();
    }

    $(".divTotal").show();

    $(".totalSalasSpan").text(totalsi + "/" + total);
}

function totalSalasEmpresa() {

    var checkeds = $("#tabsEmpresas li");
    var salasCheckbox = 0;
    var salasCheckboxChecked = 0;
    var salasCheckboxNotChecked = 0;

    jQuery.each(checkeds, function () {
        var idEmpresa = $(this).attr('data-id');
        salasCheckbox = $(`#tabContentSalas .tab-pane input[data-empresa="${idEmpresa}"]:checkbox`).length;
        salasCheckboxChecked = $(`#tabContentSalas .tab-pane input[data-empresa="${idEmpresa}"]:checkbox:checked`).length;
        salasCheckboxNotChecked = $(`#tabContentSalas .tab-pane input[data-empresa="${idEmpresa}"]:checkbox:not(:checked)`).length;

        var tabEmpresa = $(`#tabsEmpresas li[data-id="${idEmpresa}"]`);
        tabEmpresa.find(".totalSalasEmpresaSpan").text(salasCheckboxChecked + "/" + salasCheckbox)

        tabEmpresa.css('background', 'transparent');
        tabEmpresa.find('.totalSalasEmpresaSpan').removeClass('lie-success');

        if (salasCheckboxChecked > 0) {
            tabEmpresa.css('background', '#EEE');
            tabEmpresa.css('border', '1px solid #FFF');
            tabEmpresa.find('.totalSalasEmpresaSpan').addClass('lie-success');
        }
    });


}

function changeCheckedEmpresaSala(idEmpresa, idSala, checked) {

    var code = idEmpresa + idSala;

    $(`input[data-code="${code}"]`).prop('checked', checked)
    $(`input[data-code="${code}"]`).parent().removeClass('checked')

    if (checked) {
        $(`input[data-code="${code}"]`).parent().addClass('checked')
    }
}

// New Usuario Sala JS
function getUsers(codSala) {
    var element = $('#cboUsuario')

    $.ajax({
        url: `${basePath}MaquinaAnfitriona/GetEmpleadosAnfitriona`,
        type: "POST",
        data: JSON.stringify({codSala}),
        contentType: "application/json",
        cache: false,
        beforeSend: function () {
            element.html(`<option value="" data-name="">--Seleccione--</option>`)

            $.LoadingOverlay("show")
        },
        success: function (response) {
            var items = response.data

            items.map(item => {
                element.append(`<option value="${item.UsuarioID}" data-name="${item.NombreEmpleado}">${item.NombreEmpleado}</option>`)
            })
        },
        complete: function () {
            element.select2()

            $.LoadingOverlay("hide")
        }
    })
}

function getUserEmployee(userId) {
    clearUserEmployee()

    toastr.clear()

    if (!userId) {
        toastr.warning("Seleccione un Usuario", "Mensaje Servidor")

        return false
    }

    //$.ajax({
    //    url: `${basePath}Usuario/UsuarioEmpleadoIDObtenerJson`,
    //    type: "POST",
    //    data: JSON.stringify({ usuarioId: userId }),
    //    contentType: "application/json",
    //    cache: false,
    //    success: function (response) {
    //        var item = response.respuesta
    //        var userTypeId = item.TipoUsuarioID
    //        var personId = ""
    //        var personType = ""

    //        if (userTypeId == 1) {
    //            personId = item.ClienteId
    //            personType = "Cliente"
    //        }
    //        else {
    //            personId = item.EmpleadoID;
    //            personType = "Empleado"
    //        }

    //        if (personId) {
    //            $.ajax({
    //                url: `${basePath}Empleado/EmpleadoIdObtenerJson`,
    //                type: "POST",
    //                data: JSON.stringify({ empleadiId: personId }),
    //                contentType: "application/json",
    //                cache: false,
    //                success: function (response) {
    //                    var item = response.respuesta

    //                    $("#nombrePersona").html(`${item.Nombres} ${item.ApellidosPaterno} ${item.ApellidosMaterno}`)
    //                    $("#tipoPersona").html(personType)
    //                    $("#telefonoPersona").html(item.Telefono)
    //                    $("#celularPersona").html(item.Movil)
    //                    $("#correoPersona").html(item.MailJob)
    //                    $("#direccionPersona").html(item.Direccion)

    //                    if (item.EstadoEmpleado) {
    //                        $("#estadoPersona").html("Activo")
    //                        $("#estadoPersona").css("color", "green")
    //                    }
    //                    else {
    //                        $("#estadoPersona").html("Inactivo")
    //                        $("#estadoPersona").css("color", "red")
    //                    }
    //                }
    //            })
    //        }
    //    }
    //})
}

function clearUserEmployee() {
    $("#nombrePersona").html('')
    $("#tipoPersona").html('')
    $("#telefonoPersona").html('')
    $("#celularPersona").html('')
    $("#correoPersona").html('')
    $("#direccionPersona").html('')
    $("#estadoPersona").html('')
}

// document ready
$(document).ready(function () {
    //getUsers()
})

function PaintUI(codSala) {
    var AppRoomUser = new Vue({
        el: '#AppRoomUser',
        data: {
            user: {
                id: null,
                name: ''
            },
            companies: [],
            rooms: [],
            roomes: [],
            companySelected: -1,
            companySearch: '',
            roomSearch: '',
            isViewFull: false,
            companySelectedTemp: -1,
            loadingData: true
        },
        computed: {
            filteredCompanies: function () {
                return this.companies.filter(company => {
                    return company.name.toLowerCase().includes(this.companySearch.toLowerCase())
                })
            },
            filteredRooms: function () {
                return this.roomes.filter(room => {
                    return room.name.toLowerCase().includes(this.roomSearch.toLowerCase()) || room.company.name.toLowerCase().includes(this.roomSearch.toLowerCase()) || room.detail.toLowerCase().includes(this.roomSearch.toLowerCase())
                })
            },
            getCompany: function () {
                return this.companies.find(company => company.code == this.companySelected)
            },
            getRoomesChecks: function () {
                return this.filteredRooms.filter(item => item.checked == true)
            },
            getRoomsChecks: function () {
                return this.rooms.filter(item => item.checked == true)
            },
            isAllChecked: function () {
                return this.getRoomesChecks.length == this.filteredRooms.length
            }
        },
        methods: {
            getDataInitial: function () {
                var self = this

                self.loadingData = true

                $.ajax({
                    url: `${basePath}MaquinaAnfitriona/GetMaquinas`,
                    type: "POST",
                    data: JSON.stringify({ codSala }),
                    contentType: "application/json",
                    cache: false,
                    beforeSend: function () {
                        $.LoadingOverlay("show")
                    },
                    success: function (response) {
                        self.setDataInitial(response)
                    },
                    complete: function () {
                        $.LoadingOverlay("hide")
                    }
                })
            },
            setDataInitial: function (response) {
                var self = this
                var companies = response.data.companies
                var rooms = response.data.rooms

                self.companies = []
                self.rooms = []
                self.roomes = []

                companies.map(item => {
                    self.companies.push({
                        code: item.Id,
                        name: item.Nombre,
                        rooms: [],
                        roomsCheck: 0,
                        roomsTotal: 0
                    })
                })

                rooms.map(item => {
                    self.rooms.push({
                        code: item.Zona,
                        name: item.CodMaquina,
                        detail: item.NombreZona + " - " + item.NombreIsla,
                        company: self.companies.find(company => company.code == item.Zona),
                        checked: false
                    })
                })

                self.companies.map(item => {
                    item.rooms = self.rooms.filter(room => room.code == item.code)
                })

                self.loadingData = false
            },
            setCompanySelected: function (company) {
                this.isViewFull = false
                this.companySelected = company.code
                this.companySelectedTemp = company.code
                this.roomes = company.rooms
                this.roomSearch = ''
            },
            checkCompanyRoom: function (company) {
                return {
                    'litm-count-success': company.roomsCheck === company.roomsTotal && company.roomsTotal > 0,
                    'litm-count-warning': company.roomsCheck < company.roomsTotal && company.roomsTotal > 0,
                    'litm-count-danger': company.roomsCheck === 0 && company.roomsTotal > 0
                }
            },
            checkRoomes: function () {
                var rooms = this.filteredRooms
                var checks = this.getRoomesChecks

                return {
                    'gphf-success': checks.length === rooms.length && rooms.length > 0,
                    'gphf-warning': checks.length < rooms.length && rooms.length > 0,
                    'gphf-danger': checks.length === 0 && rooms.length > 0
                }
            },
            checkRooms: function () {
                var rooms = this.rooms
                var checks = this.getRoomsChecks

                return {
                    'gphf-success': checks.length === rooms.length && rooms.length > 0,
                    'gphf-warning': checks.length < rooms.length && rooms.length > 0,
                    'gphf-danger': checks.length === 0 && rooms.length > 0
                }
            },
            changeViewSingle: function () {
                this.isViewFull = false
                this.companySelected = this.companySelectedTemp
                this.roomes = this.rooms.filter(room => room.company.code == this.companySelected)
                this.companySearch = ''
                this.roomSearch = ''
            },
            changeViewFull: function () {
                this.isViewFull = true
                this.companySelectedTemp = this.companySelected
                this.companySelected = this.companySelected
                this.roomes = this.rooms
                this.companySearch = ''
                this.roomSearch = ''
            },
            setRoomChecked: function (room, checked) {
                var roomIndex = this.rooms.findIndex(item => item.name == room)

                this.rooms[roomIndex].checked = checked
            },
            setUserRoomChecked: function (items) {
                var self = this

                self.rooms.forEach((room, key) => {
                    self.rooms[key].checked = false

                    if (items.some(item => item.CodMaquina == room.name)) {
                        self.rooms[key].checked = true
                    }
                })

                self.setCompanyCounts()
            },
            setCompanyCounts: function () {
                this.companies.map(company => {
                    company.roomsCheck = company.rooms.filter(room => room.checked).length
                    company.roomsTotal = company.rooms.length
                })
            },
            getAssignmentsUserRoom: function (userId) {
                var self = this

                self.setUserRoomChecked([])

                if (!userId) {
                    return false
                }

                const codSala = cboSala.val();
                if (!codSala) {
                    toastr.warning("Seleccione una sala", "Aviso")
                    return;
                }

                $.ajax({
                    url: `${basePath}MaquinaAnfitriona/GetMaquinasAnfitriona`,
                    type: "POST",
                    data: JSON.stringify({ codSala,id: userId }),
                    contentType: "application/json",
                    cache: false,
                    beforeSend: function () {
                        $.LoadingOverlay("show")
                    },
                    success: function (response) {
                        var items = response.data

                        self.setUserRoomChecked(items)
                    },
                    complete: function () {
                        $.LoadingOverlay("hide")
                    }
                })
            },
            sendDataAssignments: function (url, assignments, checked, assigRooms, toaster = '') {
                var self = this
                var salaIds = []
                var salaNames = []
                var usuarioId = self.user.id
                var userName = self.user.name
                var dataLength = assignments.length

                assignments.map(item => {
                    salaIds.push(item.room)
                    salaNames.push(item.alias)
                })

                // Auditoria
                var assigRoomsInit = assigRooms
                var assigRoomsNew = self.rooms.filter(item => item.checked)
                var salaIdsInit = []
                var salaNamesInit = []
                var salaIdsNew = []
                var salaNamesNew = []

                assigRoomsInit.map(item => {
                    salaIdsInit.push(item.code)
                    salaNamesInit.push(item.name)
                })

                assigRoomsNew.map(item => {
                    salaIdsNew.push(item.code)
                    salaNamesNew.push(item.name)
                })
                // End Auditoria

                const codSala = cboSala.val();
                if (!codSala) {
                    toastr.warning("Seleccione una sala", "Aviso")
                    return;
                }

                var data = {
                    codSala,
                    usuarioId,
                    maquinas: salaIds
                }

                $.ajax({
                    url: `${basePath}${url}`,
                    type: "POST",
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    cache: false,
                    beforeSend: function () {
                        $.LoadingOverlay("show")
                    },
                    success: function (response) {
                        var status = response.status

                        if (status) {

                            if (toaster == 'CKL_01') {
                                toastr.success(`Se asignaron ${dataLength} maquinas al usuario ${userName}`, "Mensaje Servidor")
                            }

                            if (toaster == 'CKL_02') {
                                toastr.success(`Se asignó una maquina al usuario ${userName}`, "Mensaje Servidor")
                            }

                            if (toaster == 'UKL_01') {
                                toastr.warning(`Se denegaron ${dataLength} maquinas al usuario ${userName}`, "Mensaje Servidor")
                            }

                            if (toaster == 'UKL_02') {
                                toastr.warning(`Se denegó una maquina al usuario ${userName}`, "Mensaje Servidor")
                            }

                            // Auditoria
                            var dataInitial = {
                                UsuarioID: usuarioId,
                                UsuarioNombre: userName,
                                CodSalasAsignadas: salaIdsInit.toString(),
                                SalasAsignadas: salaNamesInit.toString(),
                            }

                            var dataFinal = {
                                UsuarioID: usuarioId,
                                UsuarioNombre: userName,
                                CodSalasAsignadas: salaIdsNew.toString(),
                                SalasAsignadas: salaNamesNew.toString(),
                                CodigoSalas: salaIds.toString(),
                                NombreSalas: salaNames.toString()
                            }

                            var descriptionProcess = checked ? 'AGREGAR MAQUINA' : 'QUITAR MAQUINA'

                            dataAuditoriaJSON(3, url, descriptionProcess, dataInitial, dataFinal)
                            // End Auditoria

                        } else {
                            toastr.error(response.message, "Mensaje Servidor")
                        }
                    },
                    complete: function () {
                        $.LoadingOverlay("hide")
                    }
                })
            },
            refreshData: function () {
                this.getDataInitial()
            },
            refreshViews: function () {
                if (this.isViewFull) {
                    this.changeViewFull()
                }
                else {
                    this.changeViewSingle()
                }
            }
        },
        created: function () {
            this.$on('setUser', function (user) {
                this.user = user
                this.getAssignmentsUserRoom(user.id)
            }.bind(this))
        },
        mounted: function () {
            this.getDataInitial()
        },
        watch: {
            rooms: {
                handler: function () {
                    this.setCompanyCounts()
                },
                deep: true
            },
            loadingData(newValue) {
                if (!newValue) {
                    this.getAssignmentsUserRoom(this.user.id)
                    this.refreshViews()
                }
            }
        },
        directives: {
            icheck: {
                inserted: function (el, binding, vnode) {
                    var context = vnode.context

                    // iCheck
                    $(el).iCheck({
                        checkboxClass: 'icheckbox_square-blue icheckbox-bg-white'
                    })

                    // ifChecked
                    $(el).on('ifChecked', function (event) {
                        var input = event.target
                        var layer = $(input).attr('data-layer')
                        var checked = true
                        var assignments = []
                        var assigRooms = context.rooms.filter(item => item.checked)
                        var url = 'MaquinaAnfitriona/MaquinaAnfitrionaAsginar'

                        toastr.clear()

                        if (!context.user.id) {
                            toastr.warning('Seleccione un Usuario', 'Mensaje Servidor')

                            return false
                        }

                        if (layer == 1) {
                            var messageConfirm = `¿Estás seguro de asignar todas las maquinas para el usuario <b>${context.user.name}</b>?`

                            if (context.companySelected != -1 && !context.isViewFull) {
                                messageConfirm = `¿Estás seguro de asignar todas las maquinas de ${context.getCompany.name} para el usuario <b>${context.user.name}</b>?`
                            }

                            $.confirm({
                                title: `Hola`,
                                content: messageConfirm,
                                confirmButton: 'Sí, asignar',
                                cancelButton: 'Cancelar',
                                confirmButtonClass: 'btn-success',
                                confirm: function () {
                                    var elements = $(`.room-assign--item input:checkbox:not(:checked)`)

                                    elements.each(function (index, element) {

                                        var roomCode = $(element).attr('data-room')
                                        var roomName = $(element).attr('data-rooname')

                                        console.log(roomName)

                                        assignments.push({
                                            room: roomName,
                                            alias: roomName,
                                            checked: checked
                                        })

                                        $(element).prop('checked', checked).iCheck('update')

                                        context.setRoomChecked(roomName, checked)
                                    })

                                    $(input).prop('checked', checked).iCheck('update')

                                    context.sendDataAssignments(url, assignments, checked, assigRooms, 'CKL_01')
                                },
                                cancel: function () {
                                    $(input).prop('checked', !checked).iCheck('update')
                                }
                            })
                        }

                        if (layer == 2) {
                            var roomCode = $(input).attr('data-room')
                            var roomName = $(input).attr('data-rooname')

                            assignments.push({
                                room: roomName,
                                alias: roomName,
                                checked: checked
                            })

                            $(input).prop('checked', checked).iCheck('update')

                            context.setRoomChecked(roomName, checked)
                            context.sendDataAssignments(url, assignments, checked, assigRooms, 'CKL_02')
                        }
                    })

                    // ifUnchecked
                    $(el).on('ifUnchecked', function (event) {
                        var input = event.target
                        var layer = $(input).attr('data-layer')
                        var checked = false
                        var assignments = []
                        var assigRooms = context.rooms.filter(item => item.checked)
                        var url = 'MaquinaAnfitriona/MaquinaAnfitrionaAsginar'

                        toastr.clear()

                        if (!context.user.id) {
                            toastr.warning('Seleccione un Usuario', 'Mensaje Servidor')

                            return false
                        }

                        if (layer == 1) {
                            var messageConfirm = `¿Estás seguro denegar todas las maquinas para el usuario <b>${context.user.name}</b>?`

                            if (context.companySelected != -1 && !context.isViewFull) {
                                messageConfirm = `¿Estás seguro denegar todas las maquinas de ${context.getCompany.name} para el usuario <b>${context.user.name}</b>?`
                            }

                            $.confirm({
                                title: `Hola`,
                                content: messageConfirm,
                                confirmButton: 'Sí, denegar',
                                cancelButton: 'Cancelar',
                                confirmButtonClass: 'btn-danger',
                                confirm: function () {
                                    var elements = $(`.room-assign--item input:checkbox:checked`)

                                    elements.each(function (index, element) {

                                        var roomCode = $(element).attr('data-room')
                                        var roomName = $(element).attr('data-rooname')

                                        assignments.push({
                                            room: roomName,
                                            alias: roomName,
                                            checked: checked
                                        })

                                        $(element).prop('checked', checked).iCheck('update')

                                        context.setRoomChecked(roomCode, checked)
                                    })

                                    $(input).prop('checked', checked).iCheck('update')

                                    context.sendDataAssignments(url, assignments, checked, assigRooms, 'UKL_01')
                                },
                                cancel: function () {
                                    $(input).prop('checked', !checked).iCheck('update')
                                }
                            })
                        }

                        if (layer == 2) {
                            var roomCode = $(input).attr('data-room')
                            var roomName = $(input).attr('data-rooname')

                            assignments.push({
                                room: roomName,
                                alias: roomName,
                                checked: checked
                            })

                            $(input).prop('checked', checked).iCheck('update')

                            context.setRoomChecked(roomName, checked)
                            context.sendDataAssignments(url, assignments, checked, assigRooms, 'UKL_02')
                        }
                    })
                },
                update: function (el) {
                    $(el).iCheck('update')
                }
            }
        }
    })

    $(document).on('change', '#cboUsuario', function () {

        var userId = $(this).val()
        var userName = $(this).find('option:selected').attr('data-name')
        var divInfoUser = $('#usuarioDatos')
        var divRoom = $('#AppRoomUser')
        var divMessage = $('#salaData')

        var user = {
            id: userId,
            name: userName
        }
        divInfoUser.css("display", "block")
        divRoom.css("display", "block")
        divMessage.css("display", "none")
        getUserEmployee(userId)
        AppRoomUser.$emit('setUser', user)
    })


}



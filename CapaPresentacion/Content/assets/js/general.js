basePath = $("#BasePath").val();

// Arguments
var ajaxhr = null

$(document).ajaxSend(function (e, xhr, opt) {
    //console.log(opt.data, "asas");
    var datos = "";
    //console.log(JSON.stringify(opt));
    if (opt.data == "{}" || opt.data == null || opt.data == "\"\"" || opt.data=="") {
        datos = "{}";
        //console.log("vacio")
    }
    else {
        datos = opt.data;
    }
    setCookie("datafinal", datos);
});

//$(document).ajaxComplete(function () {
//    //console.log(opt.data,"asas");
//    //deleteCookie("datainicial");
//    //deleteCookie("datafinal");
//});
let urlsConErrorSinToastr=
    [
        'http://localhost:3030/api/data/ludopatatipo?comando=',
        basePath+"AsistenciaCliente/GetDataWidgets",
        basePath+"AsistenciaCliente/GetListadoSalasYTotalClientes",
        basePath + "Sala/ListadoSalaPorUsuarioJson",
        basePath+"AsistenciaCliente/GetListaCumpleanios",
        'http://181.65.130.36:2222/ExtranetPJ/',
        basePath+"AsistenciaCliente/GetTotalClientesPorAnio",
    ]
$(document).ajaxError(function (event, XMLHttpRequest, textStatus, errorThrow) {
    let urlError=textStatus.url
    let mostrarToastr=true
    urlsConErrorSinToastr.map(x=>{
        if(urlError.includes(x)){
            mostrarToastr=false
        }
    })
    console.log("ajax error : "+textStatus.url)
    if(mostrarToastr){
        console.log("assa", XMLHttpRequest.responseText)
        if (XMLHttpRequest.status == 400) {
            var noty_id = noty({
                layout: 'topCenter',
                text: XMLHttpRequest.responseText,
                modal: true,
                type: 'error'
            });
        } else if (XMLHttpRequest.status == 403) {
    
            toastr.error("No tiene permisos", "Mensaje Servidor ");
              //confirm("No tiene permisos", function () {
              //    $('.loader_gif').hide()
              //});
            // window.location = response.redireccion;
        }
        else if (XMLHttpRequest.status == 561) {
            $.confirm({
                title: 'El Tiempo de sesion a Terminado',
                content: 'Automaticamente se Cerrara el Sistema en 10 Seg. .',
                autoClose: 'confirm|10000',
                confirmButton: 'Salir',
                cancelButton: 'Loguearse',
                confirm: function () {
                    window.location = basePath;
                },
                cancel: function () {
                },
                onClose: function () {
                    $("#small-modal").modal({
                        backdrop: 'static'
                    });
                }
            });
    
            /*confirm("Su sesi�n ha finalizado", function () {
                window.location = basePath;
            });*/
            // window.location = response.redireccion;
        }
        else if (XMLHttpRequest.status == 0) {
    
            toastr.error("No hay Respuesta del Servidor,<br>Revise la conexion", "Mensaje Servidor");
    
            /*  confirm("No tiene permisos", function () {
      
                  $('.loader_gif').hide()
              });*/
            // window.location = response.redireccion;
        }
        else {
           
            console.log("Servidor No Disponible");
        }
    }
});

//region hora dashboard 
function show5() {
    if (!document.layers && !document.all && !document.getElementById)
        return;

    var Digital = new Date();
    var hours = Digital.getHours();
    var minutes = Digital.getMinutes();
    var seconds = Digital.getSeconds();

    var dn = "PM";
    if (hours < 12)
        dn = "AM";
    if (hours > 12)
        hours = hours - 12;
    if (hours == 0)
        hours = 12;

    if (minutes <= 9)
        minutes = "0" + minutes;
    if (seconds <= 9)
        seconds = "0" + seconds;
    //change font size here to your desire
    myclock =  hours + ":" + minutes + ":"
        + seconds + " " + dn ;
    if (document.layers) {
        document.layers.liveclock.document.write(myclock);
        document.layers.liveclock.document.close();
    }
    else if (document.all)
        liveclock.innerHTML = myclock;
    else if (document.getElementById)
        document.getElementById("liveclock").innerHTML = myclock;
    setTimeout("show5()", 1000);
}

if ($("#fechaHoy").length) {
    var today = moment().format('DD/MM/YYYY');
    document.getElementById("fechaHoy").innerHTML = today;
    window.onload = show5;
}
//fin region  hora dashboard

//seccion cookies
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = cname + "=" + cvalue + "; " + expires + "; path=/";
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

function deleteCookie(cname) {
    setCookie(cname, "", -1);
}

//fin seccion cookies



///////////////////////////////////////////////////////////////////////////////////////////////////

$("#loginFomrSesion")
    .bootstrapValidator({
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            usuario: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese su Usuario, Obligatorio'
                    }
                }
            },
            contrasena: {
                validators: {
                    notEmpty: {
                        message: 'Ingrese su Contrase�a, Obligatorio'
                    }
                }
            }
        }
    })
    .on('success.field.bv', function (e, data) {
        console.log("submit");
        //e.preventDefault();
        var $parent = data.element.parents('.form-group');
        // Remove the has-success class
        $parent.removeClass('has-success');
        // Hide the success icon
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();
    });
//$("#loginFomrSesion").submit(function (ev) { ev.preventDefault(); });

$('#btnAccederSesion').on('click', function (e) {
    $("#loginFomrSesion").data('bootstrapValidator').resetForm();
    var validar = $("#loginFomrSesion").data('bootstrapValidator').validate();
    if (validar.isValid()) {

        var contrasena = $("#contrasena").val();
        var usuario = $("#usuario").val();

        var url = basePath + "Usuario/ValidacionLogin";
        var data = { usuLogin: usuario, usuPassword: contrasena };
        DataPostSend(url, data, true).done(function (response) {
            if (response) {
                var data = response.respuesta;
                if (data) {
                    toastr.success(response.mensaje, "Mensaje Servidor");
                    $("#loginFomrSesion").attr("disabled", true);
                    setTimeout(function () {
                        $("#small-modal").modal("hide");
                    }, 800);

                } else {
                    toastr.error(response.mensaje, "Mensaje Servidor");
                }

            } else {
                toastr.error("Error no se logro conectar al servidor", "Mensaje Servidor");
            }
        }).fail(function (x) {
            toastr.error("Error Fallo Servidor", "Mensaje Servidor");
        });


    }

});

$("#small-modal").on("shown.bs.modal", function () {

    //$(document).keypress(function (e) {
    //    if (e.which == 13) {
    //        $('.btnGuardar').click();
    //    }
    //});

    $("#loginFomrSesion input").keydown(function (e) {
        if (e.keyCode == 13) {
            console.log("login");
            //e.preventDefault();
            $('#btnAccederSesion').click();

        }

    });
});

$(".modal-dialog").draggable({
    handle: ".modal-header"
});

$("#divHisTMov").draggable({
    handle: ".notifications-alert-desc"
});

//menu

if ($("#right-column").length > 0) {
    //Menu(false);
}

function Menu(loading) {
    var data = {}
    var url = basePath + "Usuario/ListadoMenus";

    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: function () {
            if (loading === true) {
                $.LoadingOverlay("show");
            }
        },
        success: function (response) {
            var mensaje = response.mensaje;
            if (mensaje) {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
            var listado = response.dataResultado;
           // console.log(listado)
            if (listado.length > 0) {
                // $(".mainnav li:not(:first)").hide();
                //console.log(listado)
                $.each(listado, function (index, value) {
                    var menu = value.WEB_PMeDataMenu;
                    $('.mainnav li[data-menu1="' + menu + '"]').removeClass('ocult');
                });
            } else {
               $('.mainnav li').addClass('ocult');
            }
            $.LoadingOverlay("hide");


        },
        complete: function () {
            if (loading === true) {
                $.LoadingOverlay("hide");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            
        }
    });
}
//en menu

//AUDITORIA
//function auditoria(ubicacion) {
   
//    if ($("#" + ubicacion + " .auditoria").length > 0) {
//        arrayinputs = [];
//        $('#' + ubicacion + ' input.auditoria:checkbox:checked, #' +ubicacion +' input.auditoria:radio:checked, \
// #'+ ubicacion + ' select.auditoria option:selected,#' + ubicacion + ' textarea.auditoria,#' + ubicacion + ' input.auditoria:text,#' + ubicacion + ' input.auditoria[type=email],#' + ubicacion +' input.auditoria[type=hidden]')
//            .each(function (i, e) {
//                obj = {};

//                var elemento = e;
//                //  obj.type=$(elemento).attr("type");
//                obj.name = $(elemento).attr("name");
//                obj.value = $(elemento).val();
//                //obj.nodeName=$(elemento)[0].nodeName
//                if ($(elemento)[0].nodeName == "OPTION") {

//                    obj.name = $(elemento).closest("select").attr("name")
//                    obj.value_text = $(elemento).text()
//                    obj.type = "select"
//                }
//                // dir(obj)
//                arrayinputs.push(obj)
//                if (obj.type == "select") {
//                    objselecttxt = {}
//                    objselecttxt.name = obj.name + "_text";
//                    objselecttxt.value = $(elemento).text()
//                    arrayinputs.push(objselecttxt)
//                }
//            })
//        objetofinal = {}
//        $(arrayinputs).each(function (i, e) {
//            objetofinal[(e.name).trim()] = e.value;
//        });
//        if ($("#"+ ubicacion + "_nauditoria").val() == "n") {
//            setCookie("datainicial", "");
//        }
//        else {
//            setCookie("datainicial", JSON.stringify(objetofinal));
//        }
        
//    } else {
//        setCookie("datainicial", "");
//    }
//}

//$(document).ajaxSend(function (e, xhr, opt) {
//    var codsala = $(".codSalaAuditoria");
//    if (codsala.length > 0) {
//        var codigo = $(".codSalaAuditoria option:selected").data('id');
//        if (codigo != undefined) {
//            setCookie("codSala", codigo);
//        }
//    }
//    else {
//        setCookie("codSala", "");
//    }
//    //console.log(opt.data, "asas");
//    var datos = "";
//    //console.log(JSON.stringify(opt));
//    if (opt.data == "{}" || opt.data == null || opt.data == "\"\"" || opt.data == "") {
//        datos = "{}";
//        //console.log("vacio")
//    }
//    else {
//        datos = opt.data;
//    }
//    setCookie("datafinal", datos);
//});



//FIN AUDITORIA


//console.log('basePath', basePath)

$("form .icheck-minimal").iCheck({
    checkboxClass: 'icheckbox_minimal-yellow',
    radioClass: 'iradio_minimal-orange',
    increaseArea: '20%' // optional
});
var dateNow = new Date();
dateNow.setDate(dateNow.getDate());
$(".dateOnly").datetimepicker({
    //pickTime: false,
    format: 'DD/MM/YYYY',
    defaultDate: dateNow
});
function llenarSelect(url, data, select, dataId, dataValor, selectVal) {
    if (!url) {
        toastr.error("No se Declaro Url", "Mensaje Servidor");
        return false;
    }
    var mensaje = true;
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: function () {
            $("#" + select).html("");
            $("#" + select).append('<option value="">Cargando...</option>');
            $("#" + select).attr("disabled", "disabled");
            //$.LoadingOverlay("show");
        },
        success: function (response) {
            var datos = response.data;
            var mensaje = response.mensaje; 
            if (datos.length > 0) {
                $("#" + select).html("");
                $("#" + select).append('<option value="">--Seleccione--</option>');
                if (selectVal == "allOption") {
                    $("#" + select).append('<option value="0">Todos</option>');
                }
                $.each(datos, function (index, value) {
                    var selected = "";
                    if ($.isArray(selectVal)) {
                        if (objectFindByKey(selectVal, dataId, value[dataId]) != null) {
                            selected = "selected='selected'";
                        };
                    } else {

                        if (value[dataId] === selectVal) {
                            selected = "selected='selected'";
                        };
                    }
                    $("#" + select).append('<option value="' + value[dataId] + '"    ' + selected + '>' + value[dataValor] + '</option>');

                });
                $("#" + select).removeAttr("disabled");
            } else {
                toastr.error("No Hay Data  en " + select, "Mensaje Servidor");
            }
            //if (mensaje !== "") {
            //    toastr.error(mensaje, "Mensaje Servidor");
            //}
        },
        complete: function () {
            //$.LoadingOverlay("hide");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mensaje = false;
            
        }
    });
    return mensaje;
}

const llenarSelect2Agrupado = (obj) => {
    const { url, data = {}, select, selectedVal, campoId, campoAgrupacion, campoValor, isMultiple = false, dropdownParent } = obj;

    if (!url) {
        toastr.error(`No se declaró para llenar el select ${select}`, "Mensaje Servidor");
        return false;
    }
    let mensaje = true;

    const combo = $(`#${select}`);

    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: () => {
            combo.html("");
            combo.append('<option value="">Cargando...</option>');
            combo.attr("disabled", "disabled");
        },
        success: (response) => {
            if (response.data.length <= 0) {
                toastr.error(`No hay data en ${select}`, "Mensaje Servidor");
                return;
            }
            combo.removeAttr("disabled");

            const dataAgrupada = agruparPorCampo(response.data, campoAgrupacion, campoValor, campoId);
            combo.select2({
                placeholder: "Seleccione ...",
                multiple: isMultiple,
                dropdownParent: dropdownParent,
                data: dataAgrupada
            });

            if (selectedVal) {
                combo.val(selectedVal).trigger('change');
            }
        },
        error: () => {
            mensaje = false;
        }
    });
    return mensaje;
}

const agruparPorCampo = (data, campoAgrupacion, campoValor, campoId) => {
    const agrupado = {};

    data.forEach(item => {
        const clave = item[campoAgrupacion];
        if (!agrupado[clave]) {
            agrupado[clave] = [];
        }
        agrupado[clave].push({
            id: item[campoId],
            text: item[campoValor]
        });
    });

    const resultado = [];
    for (const tipo in agrupado) {
        resultado.push({
            text: tipo,
            children: agrupado[tipo]
        });
    }
    return resultado;
}

function llenarSelectAPIProgresivo(url, data, select, dataId, dataValor, selectVal) {

    if (!url) {
        toastr.error("No se Declaro Url", "Mensaje Servidor");
        return false;
    }
    var mensaje = true;
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: function () {
            $("#" + select).html("");
            $("#" + select).append('<option value="">Cargando...</option>');
            $("#" + select).attr("disabled", "disabled");
            //$.LoadingOverlay("show");
        },
        success: function (response) {            
            var datos = response;
            var mensaje = response.mensaje;
            if (datos.length > 0) {
                $("#" + select).html("");
                $("#" + select).append('<option value="">--Seleccione--</option>');
                if (selectVal == "allOption") {
                    $("#" + select).append('<option value="0">Todos</option>');
                }
                $.each(datos, function (index, value) {
                    var selected = "";
                    if ($.isArray(selectVal)) {
                        if (objectFindByKey(selectVal, dataId, value[dataId]) != null) {
                            selected = "selected='selected'";
                        };
                    } else {

                        if (value[dataId] === selectVal) {
                            selected = "selected='selected'";
                        };
                    }
                    $("#" + select).append('<option value="' + value[dataId] + '"    ' + selected + '>' + value[dataValor] + '</option>');

                });
                $("#" + select).removeAttr("disabled");
            } else {
                toastr.error("No Hay Data  en " + select, "Mensaje Servidor");
            }
            //if (mensaje !== "") {
            //    toastr.error(mensaje, "Mensaje Servidor");
            //}
        },
        complete: function () {
            //$.LoadingOverlay("hide");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mensaje = false;
            $("#" + select).html("");
            $("#" + select).append('<option value="">--Seleccione--</option>');
            $("#" + select).removeAttr("disabled");
            
        }
    });
    return mensaje;
}
function enviarDataPost(url, data, loading, redirect, redirectUrl, limpiar) {

    var mensaje = "";
    return mensaje = $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: function () {
            if (loading === true) {
                $.LoadingOverlay("show");
            }
        },
        success: function (response) {
            var respuesta = response.respuesta;
            if (respuesta === true) {
                if (redirect === true) {
                    window.location.replace(redirectUrl);
                }
                else {

                    $.confirm({
                        icon: 'fa fa-spinner fa-spin',
                        title: 'Mensaje Servidor',
                        theme: 'black',
                        animationBounce: 1.5,
                        columnClass: 'col-md-6 col-md-offset-3',
                        confirmButtonClass: 'btn-info',
                        cancelButtonClass: 'btn-warning',
                        confirmButton: 'Ir a Listado ',
                        cancelButton: 'Seguir Registrando',
                        content: false,
                        confirm: function () {
                            window.location.replace(redirectUrl);
                        },
                        cancel: function () {
                            if (limpiar === true) {
                                $("form input,select,textarea").val("");

                            }

                        }
                    });
                }

            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");
                console.log(response)
            }
        },
        complete: function () {
            if (loading === true) {
                $.LoadingOverlay("hide");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
           
        }
    });
}

function DataPostModo1(url, ubicacion, data = {}) {
    if (!url) {
        toastr.error("No se Declaro Url", "Mensaje Servidor");
        return false;
    }
    return $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: function () {
            //if (loading === true) {
            //    $.LoadingOverlay("show");
            //}
        },
        complete: function () {
            //if (loading === true) {
            //    $.LoadingOverlay("hide");
            //}
        },
        success: function (response) {
            $("#" + ubicacion).html(response);
        },
        error: function (jqXHR, textStatus, errorThrown) {
           
        }
    });
}
//$.fn.serializeFormJSON = function () {

//    var o = {};
//    var a = this.serializeArray();
//    $.each(a, function () {
//        if (o[this.name]) {
//            if (!o[this.name].push) {
//                o[this.name] = [o[this.name]];
//            }
//            o[this.name].push(this.value || '');
//        } else {
//            o[this.name] = this.value || '';
//        }
//    });
//    return o;
//}



$.fn.serializeFormJSON = function () {
    arrayinputs = [];
    $('input:checkbox:checked, input:radio:checked, \
select option:selected, textarea, input:text,input[type=email],input[type=number],input[type=hidden],input[type=password]', $(this))
        .each(function (i, e) {
            obj = {};
            var elemento = e;
            //  obj.type=$(elemento).attr("type");
            obj.name = $(elemento).attr("name");
            obj.value = $(elemento).val();
            //obj.nodeName=$(elemento)[0].nodeName
            if ($(elemento)[0].nodeName == "OPTION") {
                obj.name = $(elemento).closest("select").attr("name")
                obj.value_text = $(elemento).text();
                obj.type = "select";
            }
            // dir(obj)
            arrayinputs.push(obj);
            if (obj.type == "select") {
                objselecttxt = {}
                objselecttxt.name = obj.name + "_text";
                objselecttxt.value = $(elemento).text()
                arrayinputs.push(objselecttxt)
            }
        })
    objetofinal = {}
    $(arrayinputs).each(function (i, e) {
        objetofinal[(e.name).trim()] = e.value;
    });
    return objetofinal;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
function VistaAuditoria(proceso, descripcion, datainicio, clase,codigo) {
    //datainicio :1=llena datainicial ,0 = sin data
    setCookie("codSala", "");
    setCookie("datainicial","");
    setCookie("datafinal", "");
    dataobj = {
        formularioID: codigo,
        datainicial: "",
        datafinal: "",
        codSala: "",
        sala: "",
        proceso: proceso,
        descripcion: descripcion
    };
    $.ajax({
        url: basePath + 'Seguridad/AgregarVisita',
        type: "POST",
        data: JSON.stringify(dataobj),
        contentType: "application/json",
        beforeSend: function () {

        },
        success: function (response) {
            //respuesta = response.respuesta;
            //if (respuesta === true) {
            //    console.log(response.mensaje, "Mensaje Servidor visita");
            //} else {
            //    console.log(response.mensaje, "Mensaje Servidor visita error");
            //}
        },
        complete: function () {
            setCookie("codSala", "");
            setCookie("datafinal", "");
            //console.log("dad_", datainicio)
            if (datainicio == 1) {
                //console.log("dad","1")
                dataAuditoria(0, clase);
            }
            else {
                setCookie("datainicial", "");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }
    });
}

function dataAuditoria(inifin, clase,Formcodigo,proceso,descripcion) {
    //inifin : 0=inicio  -  1=fin , clase : formulario,div etc,Formcodigo:formilarioID
    if (inifin == 0) {
        var data_i = $(clase).serializeFormJSON_();
        //console.log(data_i,"inicial")
        setCookie("datainicial", JSON.stringify(data_i));
    } else {
        var codsala = $(".codSalaAuditoria");
        if (codsala.length > 0) {
            var codigo = $(".codSalaAuditoria option:selected").data('id');
            var codigotexto = "";
            if (codigo != undefined) {
                setCookie("codSala", codigo);
                codigotexto = $(".codSalaAuditoria option:selected").text();
            }
        }
        else {
            setCookie("codSala", "");
        };
        var data_f = $(clase).serializeFormJSON_();
        setCookie("datafinal", JSON.stringify(data_f));
        //console.log(getCookie("datainicial"), "final")
        dataobj = {
            formularioID: Formcodigo,
            datainicial: getCookie("datainicial"),
            datafinal: getCookie("datafinal"),
            codSala: getCookie("codSala"),
            sala: codigotexto,
            proceso: proceso,
            descripcion: descripcion
        };
        $.ajax({
            url: basePath +'Seguridad/AgregarVisita',
            type: "POST",
            data: JSON.stringify(dataobj),
            contentType: "application/json",
            beforeSend: function () {
                
            },
            success: function (response) {
                respuesta = response.respuesta;
                if (respuesta === true) {
                    console.log(response.mensaje, "Mensaje Servidor data");
                } else {
                    console.log(response.mensaje, "Mensaje Servidor data error");
                }
            },
            complete: function () {
                setCookie("codSala", "");
                setCookie("datainicial", getCookie("datafinal"));
                setCookie("datafinal", "");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                
            }
        });
    }
    

}


function dataAuditoriaJSON(Formcodigo, proceso, descripcion,datainicial,datafinal) {
    //Formcodigo:formilarioID
   
    if (datainicial == "") {
        setCookie("datainicial", "");
    }
    else {
        setCookie("datainicial", JSON.stringify(datainicial));
    };
   
    if (datafinal == "") {
        setCookie("datafinal", "");
    }
    else {
        setCookie("datafinal", JSON.stringify(datafinal));
    }
    var codsala = $(".codSalaAuditoria");
    if (codsala.length > 0) {
        var codigo = $(".codSalaAuditoria option:selected").data('id');
        var codigotexto = "";
        if (codigo != undefined) {
            setCookie("codSala", codigo);
            codigotexto = $(".codSalaAuditoria option:selected").text();
        }
    }
    else {
        setCookie("codSala", "");
    };

  
    dataobj = {
        formularioID: Formcodigo,
        datainicial: getCookie("datainicial"),
        datafinal: getCookie("datafinal"),
        codSala: getCookie("codSala"),
        sala: codigotexto,
        proceso: proceso,
        descripcion: descripcion
    };
    $.ajax({
        url: basePath + 'Seguridad/AgregarVisita',
        type: "POST",
        data: JSON.stringify(dataobj),
        contentType: "application/json",
        beforeSend: function () {

        },
        success: function (response) {
            respuesta = response.respuesta;
            if (respuesta === true) {
                console.log(response.mensaje, "Mensaje Servidor data");
            } else {
                console.log(response.mensaje, "Mensaje Servidor data error");
            }
        },
        complete: function () {
            setCookie("codSala", "");
            setCookie("datainicial", getCookie("datafinal"));
            setCookie("datafinal", "");
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }
    });
}

$.fn.serializeFormJSON_ = function () {
    arrayinputs = [];
    $('input:checkbox:checked, input:radio:checked, \
select:not(.excepto) option:selected, textarea, input:text,input[type=email],input[type=number],input.record[type=hidden]', $(this))
        .each(function (i, e) {
            obj = {};

            var elemento = e;
            //  obj.type=$(elemento).attr("type");
            obj.name = $(elemento).attr("name");
            obj.id = $(elemento).attr("id");
            obj.value = $(elemento).val();
            //obj.nodeName=$(elemento)[0].nodeName
            if ($(elemento)[0].nodeName == "OPTION") {

                obj.id = $(elemento).closest("select").attr("id")
                obj.value_text = $(elemento).text()
                obj.type = "select"
            }
            // dir(obj)
            arrayinputs.push(obj)
            if (obj.type == "select") {
                objselecttxt = {}
                objselecttxt.id = obj.id + "_text";
                objselecttxt.value = $(elemento).text()
                arrayinputs.push(objselecttxt)
            }
        })
    objetofinal = {}
    $(arrayinputs).each(function (i, e) {
        objetofinal[(e.id)] = e.value;
    });
    return objetofinal;
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////
function DataPostWithoutChange(url, data, loading) {

    var mensaje = true;
    $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: function () {
            if (loading === true) {
                $.LoadingOverlay("show");
            }
        },
        success: function (response) {
             respuesta = response.respuesta;
            if (respuesta === true) {
                toastr.success(response.mensaje, "Mensaje Servidor");
            } else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
        complete: function () {
            if (loading === true) {
                $.LoadingOverlay("show");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mensaje = false;
            if (jqXHR.status === 0) {
                toastr.error("Not connect: Verify Network.", "Mensaje Servidor");
            } else if (jqXHR.status == 404) {
                toastr.error("Requested page not found [404]", "Mensaje Servidor");
            } else if (jqXHR.status == 500) {
                toastr.error("Internal Server Error [500].", "Mensaje Servidor");
            } else if (textStatus === 'parsererror') {
                toastr.error("Requested JSON parse failed.", "Mensaje Servidor");
            } else if (textStatus === 'timeout') {
                toastr.error("Time out error.", "Mensaje Servidor");
            } else if (textStatus === 'abort') {
                toastr.error("Ajax request aborted.", "Mensaje Servidor");
            }
        }
    });
    return mensaje;
}
function DataPostSend(url, data, loading) {
    if (!url) {
        toastr.error("No se Declaro Url", "Mensaje Servidor");
        return false;
    }
    return $.ajax({
        url: url,
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        beforeSend: function () {
            if (loading === true) {
                $.LoadingOverlay("show");
            }
        },
        complete: function () {
            if (loading === true) {
                $.LoadingOverlay("hide");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {

        }
    });

}

serialize = function (obj) {
    var str = [];
    for (var p in obj)
        if (obj.hasOwnProperty(p)) {
            str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
        }
    return str.join("&");
}

$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    $.each($.fn.dataTable.tables(true), function () {
        $(this).DataTable()
            .columns.adjust()
            .responsive.recalc();
    });
})

function gapTwoDates(firstDate, secondDate) {
    var date1 = moment(firstDate, 'DD/MM/YYYY')
    var date2 = moment(secondDate, 'DD/MM/YYYY')

    var diffDays = date2.diff(date1, 'days')

    return diffDays
}

function truncateString(str, num = 50) {
    if (str.length <= num) {
        return str
    }

    return str.slice(0, num) + '...'
}

// Box Abort Request
var AbortRequest = {
    element: $('#box_abort_request'),
    open: function () {
        if (ajaxhr) {
            this.element.addClass('bar-open')
        }
    },
    close: function () {
        this.ajaxhrNull()
        this.element.removeClass('bar-open')
    },
    abort: function () {
        if (ajaxhr) {
            ajaxhr.abort()
        }

        if (toastr) {
            toastr.clear()
        }

        this.close()
    },
    ajaxhrNull: function () {
        ajaxhr = null
    }
}

// Button abort request
$(document).on('click', '.abort__request', function (event) {
    event.preventDefault()

    AbortRequest.abort()
    
    $.LoadingOverlay("hide")
})
$(document).ready(function () {
    llenarSelect(basePath + "Empleado/TipoDocumentoListarJson", {}, "cboTipoDocumento", "DOIID", "DESCRIPCION");
    llenarSelect(basePath + "AsistenciaCliente/GetListadoTipoFrecuencia", {}, "cboTipoFrecuencia", "Id", "Nombre");

    $(document).on('click', '#btnRegistrarAsistencia', function (e) {
        //e.preventDefault();
        $("#form_registro_").data('bootstrapValidator').resetForm();
        let validar = $("#form_registro_").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            let dataForm = $("#form_registro_").serializeForm();
          
            $.ajax({
                url: basePath + "ClienteEmpadronamiento/GuardarEmpadronamientoCliente",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(dataForm),
                beforeSend: function () {
                },
                complete: function () {
                },
                success: function (response) {
                    if (response.respuesta) {
                        //if (response.cantsalas > 1) {
                        //    var html = '';
                        //    var title = "Seleccione Sala !";
                        //    var tabla = "";
                        //    $.each(datoss, function (index, value) {
                        //        tabla += '<tr><td>' + value.CodSala + '</td><td>' + value.Nombre + '</td><td><input type="checkbox" class="chk_sala" name="chk_sala"  value="' + index + '"></td></tr>';
                        //    });

                        //    html = '<table style="font-size:9px;margin-bottom:0px;" class="table table-condensed table-hover table-bordered"><thead><tr><th>ID</th><th>Nombre</th><th></th></tr></thead><tbody>' + tabla + '</tbody></table>';

                        //    $.confirm({
                        //        title: title,
                        //        content: html,
                        //        confirmButton: 'Ok',
                        //        cancelButton: 'Cerrar',
                        //        confirmButtonClass: 'btn-info',
                        //        cancelButtonClass: 'btn-danger',
                        //        confirm: function () {
                        //            var checkedsala = $('.chk_sala:checked').length;
                        //            if (checkedsala == 0) {
                        //                toastr.error("Seleccione una Sede");
                        //                return false;
                        //            }
                        //            if (checkedsala > 1) {
                        //                toastr.error("Solo puede seleccionar una");
                        //                return false;
                        //            }

                        //            var i = $('.chk_sala:checked').val();
                        //            codsalaxpagar = datoss[i].CodSala;
                        //            buscarTicket();
                        //        },

                        //        cancel: function () {
                        //            //close
                        //        },

                        //    });

                        //}
                        //else {
                            
                        //}
                        
                        toastr.success(response.mensaje, "Mensaje Servidor")
                        setTimeout(function () {
                            window.location.reload();
                        }, 1090);
                    } else {
                        toastr.error(response.mensaje, "Mensaje Servidor")
                    }
                },

            });
        }
    })



    $(document).on('click', '.buscar', function (e) {
        if ($('#txtnrodoc').val() == "") {
            toastr.error("Ingrese Nro Documento", "Mensaje Servidor");
            return;
        }
        if ($('#cboTipoDocumento').val() == "") {
            toastr.error("Seleccione Tipo Documento", "Mensaje Servidor");
            return;
        }

        if ($('#txtnrodoc').val().length > 7) {
            ObtenerDatoPersonales();
        }
        else {
            toastr.error("Minimo 8 caracteres para Nro. Documento", "Mensaje Servidor");
        }
        
    });
    $(document).on('click', '.limpiar', function (e) {
        $("#txt_nombre").val("");
        $("#txt_apepaterno").val("");
        $("#txt_materno").val("");
        $("#txt_nombre").prop('readonly', false);
        $("#txt_apepaterno").prop('readonly', false);
        $("#txt_materno").prop('readonly', false);
        $("#form_registro_").data('bootstrapValidator').resetForm();
        $("#cboTipoFrecuencia").val("");
    });

    $(document).on('keypress', "#txtnrodoc", function (e) {
        var keycode = (e.keyCode ? e.keyCode : e.which);

        if (keycode == '13' && $("#cboTipoDocumento").val() == 1) {
            if ($('#txtnrodoc').val() == "") {
                toastr.error("Ingrese Nro Documento", "Mensaje Servidor");
                return;
            }
            if ($('#cboTipoDocumento').val() == "") {
                toastr.error("Seleccione Tipo Documento", "Mensaje Servidor");
                return;
            }
            if ($('#txtnrodoc').val().length > 7) {
                ObtenerDatoPersonales();
            }
            else {
                toastr.error("Minimo 8 caracteres para Nro. Documento", "Mensaje Servidor");
            }

        }
        
    });


   
});


function ObtenerDatoPersonales() {
    $.ajax({
        type: "POST",
        url: basePath + "ClienteEmpadronamiento/buscarPersona",
        data: JSON.stringify({ nrodoc: $("#txtnrodoc").val(), tipodoc: $("#cboTipoDocumento").val() }),
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (result) {
            var datos = result.data;
            if (result.respuesta) {

                $("#txt_nombre").val(datos.Nombre);
                $("#txt_apepaterno").val(datos.ApellidoPaterno);
                $("#txt_materno").val(datos.ApellidoMaterno);
                $("#txt_nombre").prop('readonly', true);
                $("#txt_apepaterno").prop('readonly', true);
                $("#txt_materno").prop('readonly', true);
                $("#form_registro_").data('bootstrapValidator').resetForm();
                $("#form_registro_").data('bootstrapValidator').validate();
            }
            else {
                $("#txt_nombre").prop('readonly', false);
                $("#txt_apepaterno").prop('readonly', false);
                $("#txt_materno").prop('readonly', false);
                toastr.error(result.mensaje, "Mensaje Servidor");
            }

        },

        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
    return false;
}

$("#form_registro_")
    .bootstrapValidator({
        container: '#messages',
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
          
            TipoDocumentoId: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            NroDoc: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            }
            ,
            Nombre: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            ApelPat: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            ApelMat: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            TipoFrecuenciaId: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
           
        }
    })
    .on('success.field.bv', function (e, data) {
        e.preventDefault();
        var $parent = data.element.parents('.form-group');
        // Remove the has-success class
        $parent.removeClass('has-success');
        // Hide the success icon
        $parent.find('.form-control-feedback[data-bv-icon-for="' + data.field + '"]').hide();


    });

$.fn.serializeForm = function () {

    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
}
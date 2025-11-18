$(document).ready(function () {
    
    let IdClienteInsertado=localStorage.getItem("IdClienteInsertado")
    if(IdClienteInsertado){
        ObtenerDataAsistencia(IdClienteInsertado)
        localStorage.removeItem("IdClienteInsertado")
    }

    let listadoClientes;
    $(".dateOnly_").datetimepicker({
        pickTime: true,
        format: 'DD/MM/YYYY hh:mm:ss A',
        defaultDate: dateNow,
        maxDate: dateNow,
    });

    
    let fuentecliente = localStorage.getItem("fuente");;
    if (fuentecliente) {
        $("#reniec").val(fuentecliente);
    }


    $(document).on('click','#btnBuscar',function(e){
        e.preventDefault();
       

        let coincidencia = $("#coincidencia").val();
        if (coincidencia == "") {
            toastr.error("Complete la información", "Mensaje Servidor");
        }
        if (coincidencia && coincidencia != "") {
            localStorage.removeItem("IdClienteInsertado")
            let dataForm={ 
                coincidencia:coincidencia
            }
            $.ajax({
                type: "POST",
                url: basePath + "AsistenciaCliente/GetListadoClienteCoincidencia",
                cache: false,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(dataForm),
                dataType: "json",
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (response) {
                  if(response.respuesta){
                    listadoClientes=response.data;
                    if(listadoClientes.length>0){
                        toastr.success(response.mensaje, "Mensaje Servidor");
                        $("#divListadoClientes").html('<div class="alert alert-danger">Buscando Clientes....</div>');
                        $("#full-modal_clientes").modal();

                        localStorage.setItem("fuente", 0);
                        $("#reniec").val(0);
                    }
                    else{
                        //Mostrar Formulario de Creacion de Clientes
                        let cliente=response.data;
                        let nombreCliente=""
                        let redirectUrl="";
                        if(cliente.NroDoc){
                            redirectUrl=basePath+"AsistenciaCliente/RegistroCliente?redirect=true&nombre="+cliente.Nombre+"&apelpat="+cliente.ApelPat+"&apelmat="+cliente.ApelMat+"&nrodoc="+cliente.NroDoc;
                            nombreCliente=cliente.Nombre+" "+cliente.ApelPat+" "+ cliente.ApelMat
                        }
                        else{
                            redirectUrl=basePath+"AsistenciaCliente/RegistroCliente?redirect=true"
                        }
                        $.confirm({
                            icon: 'fa fa-spinner fa-spin',
                            title: 'No se encontraron registros de cliente '+nombreCliente+', ¿Desea agregarlo?',
                            theme: 'black',
                            animationBounce: 1.5,
                            columnClass: 'col-md-6 col-md-offset-3',
                            confirmButtonClass: 'btn-info',
                            cancelButtonClass: 'btn-warning',
                            confirmButton: 'SI',
                            cancelButton: 'NO',
                            content: false,
                            confirm: function () {
                               localStorage.setItem("fuente", 1);
                                $("#reniec").val(1);
                                window.location.href=redirectUrl
                            },
                            cancel: function () {
                            }
                        });
                    }
                  }
                  else{
                    toastr.error(response.mensaje, "Mensaje Servidor");
                  }
                },
                error: function (request, status, error) {
                    toastr.error("Error", "Mensaje Servidor");
                },
                complete: function (resul) {
                    $.LoadingOverlay("hide");
                }
            });
        }
    })
    $("#coincidencia").keypress(function(e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if(code==13){
            $("#btnBuscar").trigger("click");
        }
    });
    $(".noKeyPress").keypress(function(e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if(code==13){
           return false;
        }
    })
    $('#full-modal_clientes').on('shown.bs.modal', function (e) {
        var addtabla = $("#divListadoClientes");
        $(addtabla).empty();
        $(addtabla).append('<table id="ListadoClientesSISCLIE" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
        objetodatatable = $("#ListadoClientesSISCLIE").DataTable({
            "bDestroy": true,
            "bSort": true,
            "scrollCollapse": true,
            "scrollX": false,
            "sScrollX": "100%",
            "paging": true,
            "autoWidth": false,
            "bAutoWidth": true,
            "bProcessing": true,
            "bDeferRender": true,
            data: listadoClientes,
            columns: [
                // {
                //     data: "TipoDocumento.Nombre", title: "Tipo Documento",
                // },
                { data: "NroDoc", title: "Nro. Doc." },
                { 
                    data: null, title: "Nombres y Apellidos" ,"render":function(value,type,row){
                        if(row.ApelPat!=''){
                            return `${row.Nombre} ${row.ApelPat} ${row.ApelMat}`
                        }
                        else{
                            return row.NombreCompleto
                        }
                    }
                },
                // { data: null, title: "Estado" ,"render":function(value){
                //     let button="";
                //         if(value.Estado=="A"){
                //             button="ACTIVO";
                //         }else{
                //             button="PENDIENTE";
                //         };
                //         return button;
                //     }
                // },
                {
                    data: null, title: "Accion",
                    "render": function (value) {
                        let butom = "";
                        let disabled = "";
                        // if(value.Estado!="A"){
                        //     disabled="disabled";
                        // }
                        butom = '<button type="button" class="btn btn-xs btn-success btnSeleccionar" data-id="' + value.Id + '" '+disabled+'><i class="glyphicon glyphicon-pencil"></i> Seleccionar</button> '
                        return butom;
                    }
                }
            ]
            ,
            "initComplete": function (settings, json) {
            },
            "drawCallback": function (settings) {
            }
        });
    })
    $(document).on('click','.btnSeleccionar',function (e) {
        e.preventDefault();
        let ClienteId = $(this).data("id");
        ObtenerDataAsistencia(ClienteId);
     
    })
    $(document).on('click','#btnRegistrarAsistencia',function(e){
        e.preventDefault();
        let ClienteId=$("#ClienteId").val();
        $("#form_registro_asistencia").data('bootstrapValidator').resetForm();
        let validar = $("#form_registro_asistencia").data('bootstrapValidator').validate();
        if(validar.isValid()){
            let dataForm=$("#form_registro_asistencia").serializeForm();
            console.log(dataForm);
            $.confirm({
                icon: 'fa fa-spinner fa-spin',
                title: '¿Desea registrar la asistencia?',
                theme: 'black',
                animationBounce: 1.5,
                columnClass: 'col-md-6 col-md-offset-3',
                confirmButtonClass: 'btn-info',
                cancelButtonClass: 'btn-warning',
                confirmButton: 'SI',
                cancelButton: 'NO',
                content: false,
                confirm: function () {
                    $.ajax({
                        url: basePath + "AsistenciaCliente/GuardarAsistenciaCliente",
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify(dataForm),
                        beforeSend: function () {
                        },
                        complete: function () {
                        },
                        success: function (response) {
                            if(response.respuesta){
                                toastr.success(response.mensaje,"Mensaje Servidor")
                                setTimeout(function () {
                                    ObtenerDataAsistencia(ClienteId);
                                }, 2000);
                            }else{
                                toastr.error(response.mensaje,"Mensaje Servidor")
                            }
                        },
                        error: function (xmlHttpRequest, textStatus, errorThrow) {
                        }
                    });
                },
                cancel: function () {
                    toastr.warning("Registro de Asistencia cancelado","Mensaje");
                }
            });
          
        }
        else{
            toastr.error("Complete los campos obligatorios","Mensaje Servidor")
        }
    })

    $(document).on('keypress','.UpperCase',function (event) {
        $input=$(this);
        setTimeout(function () {
         $input.val($input.val().toUpperCase());
        },50);
    })
    $("#ApuestaImportante").bind('keypress', function (event) {
        var regex = new RegExp("^[0-9]+$");
        var regex2 = new RegExp("[.]");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)&&!regex2.test(key)) {
            event.preventDefault();
            return false;
        }
    });

    $(document).on('click', '#btnRegistrarEmpadronamiento', function (e) {
        e.preventDefault();
        let validar = $("#form_registro_asistencia").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            $("#full-modal_empadronamiento").modal("show");
        }
        else {
            toastr.error("Complete los campos obligatorios", "Mensaje Servidor")
        }
       
    });

    $(document).on('click', '#btnguardarempadronamiento', function (e) {
        e.preventDefault();
        let ClienteId = $("#ClienteId").val();
        $("#frm_empadronamiento").data('bootstrapValidator').resetForm();
        let validar = $("#frm_empadronamiento").data('bootstrapValidator').validate();
        if (validar.isValid()) {
            let dataForm = $("#form_registro_asistencia").serializeForm();
            dataForm.observacion = $("#txt_descripcion").val();
            dataForm.entrega_dni = JSON.parse($("#entrega_dni").val());
            dataForm.reniec = JSON.parse($("#reniec").val());
            dataForm.NroDoc = $("#txtNroDocumento").val();

            dataForm.tipocliente_id = $("#cboTipoCliente").val();
            dataForm.tipofrecuencia_id = $("#cboTipoFrecuencia").val();
            dataForm.tipojuego_id = $("#cboTipoJuego").val();


            dataForm.cliente_id = ClienteId;
            console.log(dataForm);
            $.confirm({
                icon: 'fa fa-spinner fa-spin',
                title: '¿Esta seguro de enviar registro para empadronamiento?',
                theme: 'black',
                animationBounce: 1.5,
                columnClass: 'col-md-6 col-md-offset-3',
                confirmButtonClass: 'btn-info',
                cancelButtonClass: 'btn-warning',
                confirmButton: 'SI',
                cancelButton: 'NO',
                content: false,
                confirm: function () {
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
                                toastr.success(response.mensaje, "Mensaje Servidor")
                                setTimeout(function () {
                                    ObtenerDataAsistencia(ClienteId);
                                }, 1500);
                                $("#full-modal_empadronamiento").modal("hide");
                                $("#entrega_dni").val("");
                                $("#reniec").val("");
                                $("#CodMaquina").val("");
                                $("#txt_descripcion").val("");


                            } else {
                                toastr.error(response.mensaje, "Mensaje Servidor")
                            }
                        },
                        error: function (xmlHttpRequest, textStatus, errorThrow) {
                        }
                    });
                },
                cancel: function () {
                    toastr.warning("Registro de Asistencia cancelado", "Mensaje");
                }
            });

        }
        else {
            toastr.error("Complete los campos obligatorios", "Mensaje Servidor")
        }
    })

});
function ObtenerDataAsistencia(ClienteId) {
    $.ajax({
        type: "POST",
        url: basePath + "AsistenciaCliente/GetDataAsistenciaCliente",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data:JSON.stringify({ClienteId:ClienteId}),
        dataType: "json",
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if(response.respuesta)
            {
                $("#contenedorData").show();
                $("#full-modal_clientes").modal("hide");
                $(".select").html('')
                let dataCliente = response.data.dataCliente;
                let dataAsistenciaCliente = response.data.dataAsistenciaCliente;
                let dataTipoFrecuencia=response.data.dataTipoFrecuencia;
                let dataTipoCliente=response.data.dataTipoCliente;
                let dataUltimaAsistencia=response.data.dataUltimaAsistencia;
                let dataTipoJuego=response.data.dataTipoJuego
                
                if (dataAsistenciaCliente.length > 0) {
                    objetodatatable = $("#tableListadoAsistenciaCliente").DataTable({
                        "bDestroy": true,
                        "bSort": true,
                        "scrollCollapse": true,
                        "scrollX": false,
                        "sScrollX": "100%",
                        "paging": true,
                        "autoWidth": false,
                        "bAutoWidth": true,
                        "bProcessing": true,
                        "bDeferRender": true,
                        data: dataAsistenciaCliente,
                        columns: [

                            { data: "Id", title: "ID" },
                            {
                                data: null, title: "Cliente","render":function(value,type, oData, meta){
                                    return oData.Cliente.Nombre
                                }
                            },
                            { data: "Sala.Nombre", title: "sala" },
                            {
                                data: "FechaRegistro", title: "Fecha","render":function(value, type, oData, meta){
                                    return moment(oData.FechaRegistro).format('YYYY/MM/DD hh:mm:ss A')
                                    
                                }
                            },
                        ]
                        ,
                        "initComplete": function (settings, json) {
                        },
                        "drawCallback": function (settings) {
                        }
                    });

                }
                else {
                    objetodatatable = $("#tableListadoAsistenciaCliente").DataTable({
                        "bDestroy": true,
                        "bSort": true,
                        "scrollCollapse": true,
                        "scrollX": false,
                        "sScrollX": "100%",
                        "paging": true,
                        "autoWidth": false,
                        "bAutoWidth": true,
                        "bProcessing": true,
                        "bDeferRender": true,
                        data: {},
                        columns: [
                            { title: "ID" },
                            {
                                title: "Cliente",
                            },
                            { title: "sala" },
                            {
                                title: "Fecha"
                            },
                        ]
                        ,
                        "initComplete": function (settings, json) {
                        },
                        "drawCallback": function (settings) {
                        }
                    });
                }
                if (dataTipoCliente.length > 0) {
                    $.each(dataTipoCliente, function (index, value) {
                        $("#cboTipoCliente").append('<option value="' + value.Id + '"  >' + value.Nombre + '</option>');
                    });

                }
                if (dataTipoFrecuencia.length > 0) {
                    $.each(dataTipoFrecuencia, function (index, value) {
                        $("#cboTipoFrecuencia").append('<option value="' + value.Id + '"  >' + value.Nombre + '</option>');
                    });
                }
                if (dataTipoJuego.length > 0) {
                    $.each(dataTipoJuego, function (index, value) {
                        $("#cboTipoJuego").append('<option value="' + value.Id + '"  >' + value.Nombre + '</option>');
                    });
                }
                $("#CodMaquina").val("");
                if (dataCliente.Id > 0) {
                    console.log(dataCliente.NombreCompleto, "dfasfd");
                    var nombre = "";
                    if (dataCliente.NombreCompleto == "") {
                        nombre = dataCliente.Nombre + " " + dataCliente.ApelPat + " " + dataCliente.ApelMat;
                    }
                    else {
                        nombre = dataCliente.NombreCompleto;
                    }
                    $("#txtNombre").val(nombre)
                    $("#txtTipoDocumento").val(dataCliente.TipoDocumento.Nombre)
                    $("#txtNroDocumento").val(dataCliente.NroDoc)
                    $("#ClienteId").val(dataCliente.Id);
                    $("#txtEmail").val(dataCliente.Mail);
                    $("#txtCelular1").val(dataCliente.Celular1);
                    $("#txtEstado").val(dataCliente.Estado=="A"?"ACTIVO":"PENDIENTE")

                    $("#cboTipoFrecuencia").val(dataCliente.ClienteSala.TipoFrecuenciaId)
                    $("#cboTipoCliente").val(dataCliente.ClienteSala.TipoClienteId)
                    $("#cboTipoJuego").val(dataCliente.ClienteSala.TipoJuegoId)

                    $("#ApuestaImportante").val(dataCliente.ClienteSala.ApuestaImportante==0?'':dataCliente.ClienteSala.ApuestaImportante.toFixed(2))
                    $("#txtTipoCliente").val(dataCliente.ClienteSala.TipoCliente.Nombre)
                    $("#txtTipoFrecuencia").val(dataCliente.ClienteSala.TipoFrecuencia.Nombre)
                    $("#txtTipoJuego").val(dataCliente.ClienteSala.TipoJuego.Nombre)
                }
                if(dataUltimaAsistencia.Id>0){
                    $("#cboTipoFrecuencia").val(dataUltimaAsistencia.TipoFrecuenciaId)
                    $("#cboTipoCliente").val(dataUltimaAsistencia.TipoClienteId)
                    $("#cboTipoJuego").val(dataUltimaAsistencia.TipoJuegoId)
                    $("#ApuestaImportante").val(dataUltimaAsistencia.ApuestaImportante.toFixed(2))
                    $("#txtApuestaImportante").val(dataUltimaAsistencia.ApuestaImportante==0?'':dataUltimaAsistencia.ApuestaImportante.toFixed(2))

                }
                $(".select").select2({
                    multiple: false, placeholder: "--Seleccione--",
                });
            }else{
                toastr.error(response.mensaje,"Mensaje Servidor")
            }
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
$("#form_registro_asistencia")
.bootstrapValidator({
    container: '#messages',
    excluded: [':disabled', ':hidden', ':not(:visible)'],
    feedbackIcons: {
        valid: 'icon icon-check',
        invalid: 'icon icon-cross',
        validating: 'icon icon-refresh'
    },
    fields: {
        TipoFrecuenciaId: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
        TipoClienteId: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
        FechaRegistro: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
        ApuestaImportante: {
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
        CodMaquina: {
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


$("#frm_empadronamiento")
    .bootstrapValidator({
        container: '#messages',
        excluded: [':disabled', ':hidden', ':not(:visible)'],
        feedbackIcons: {
            valid: 'icon icon-check',
            invalid: 'icon icon-cross',
            validating: 'icon icon-refresh'
        },
        fields: {
            entrega_dni: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            reniec: {
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            descripcion: {
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
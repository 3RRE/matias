let objetodatatable
$(document).ready(function () {
    ObtenerListaSalas();
    // $(document).on('click','#checkTodoSalas',function(e){
    //     if($("#checkTodoSalas").is(':checked') ){
    //         $("#cboSala > option").prop("selected","selected");
    //     }else{
    //         $("#cboSala > option").removeAttr("selected");
    //     }
    //     $("#cboSala").trigger("change");
    // })
    // $(document).on('change','#cboSala',function(e){
    //     e.preventDefault()
    //     let data=$(this).val()
    //     if(data){
    //         let allOptions = $(this).find('option');
    //         if(data.length==allOptions.length){
    //             $("#checkTodoSalas").prop('checked', true);
    //         }
    //         else{
    //             $("#checkTodoSalas").prop('checked', false);
    //         }
    //     }
    // })
    $(".dateOnly_").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        //maxDate: dateNow,
    });

    $(".dateOnly_fechaini").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        //minDate: dateMin,
    });

    $(".dateOnly_fechafin").datetimepicker({
        pickTime: false,
        format: 'DD/MM/YYYY',
        defaultDate: dateNow,
        //maxDate: dateNow,
    });

    setCookie("datainicial", "");
    $("#btnBuscar").on("click", function () {
        // if ($("#cboSala").val() == null) {
        //     toastr.error("Seleccione Sala", "Mensaje Servidor");
        //     return false;
        // }
        if ($("#fechaInicio").val() == "") {
            toastr.error("Ingrese una fecha de Inicio.");
            return false;
        }
        if ($("#fechaFin").val() == "") {
            toastr.error("Ingrese una fecha Fin.");
            return false;
        }
        buscarReclamaciones();
    });

    $("#btnlink").on("click", function () {
        // url = basePath +"ReclamacionesSala";
        // window.open(url, '_blank');
        // return false;
        $("#full-modalLinksSalas").modal('show')
    });
    $("#full-modalLinksSalas").on("shown.bs.modal", function () { 
        var addtabla = $(".contenedor_LinksSalas");
        $.ajax({
            type: "POST",
            url: basePath + "Sala/ListadoSalaActivasSinSeguridad",
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function (xhr) {
                $.LoadingOverlay("show");
            },
            success: function (result) {
                var datos = result.data;
                $(addtabla).empty();
                $(addtabla).append('<table id="tableLinksSalas" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
                objetodatatable = $("#tableLinksSalas").DataTable({
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
                    data: datos,
                    columns: [
    
                        { data: "Nombre", title: "Sala" },
                        {
                            data: null, title: "Url Reclamacion",
                            "render": function (value,type, oData, meta) {
                                return basePath + "ReclamacionesNuevo?id="+oData.CodSala;
                            }
                        },
                        {
                            data: null, title: "Accion",
                            "render": function (value,type, oData, meta) {
                                var butom = ` <a class="btn btn-xs btn-warning" target="_blank" href="${basePath}ReclamacionesNuevo?id=${oData.CodSala}"><i class="fa external-link-alt"></i> Ir a Link</a>
                                              <a class="btn btn-xs btn-info btnGenerarQR" target="_blank" data-id="${basePath}ReclamacionesNuevo?id=${oData.CodSala}" data-codsala="${oData.CodSala}" data-nombre="${oData.Nombre}"><i class="fa external-link-alt"></i> Descargar QR</a>`;
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
    
                $('#tablereclamaciones tbody').on('click', 'tr', function () {
                    $(this).toggleClass('selected');
                });
            },
            error: function (request, status, error) {
                toastr.error("Error", "Mensaje Servidor");
                $("#full-modalLinksSalas").modal('hide')
            },
            complete: function (resul) {
                $.LoadingOverlay("hide");
            }
        });
    });

    $(document).on("click", ".btnGenerarQR", function () {
        var uriQR = $(this).data("id");
        var nombreSala = $(this).data("nombre");
        var codSala = $(this).data("codsala");
        GenerarQR(uriQR, nombreSala, codSala);
    });

    $(document).on("click", ".btnAtender", function () {
            var id = $(this).data("id");
            $("#reclamacionid").val(id);
            ObtenerRegistro(id);
        
        
    });
    $(document).on("click", ".btnCargo", function () {
        var id = $(this).data("id");
        ObtenerRegistroCargo(id);
        $("#full-modalCargoReclamacion").modal("show");
    });

    $(document).on("click", "#btnExcel", function () {
        var fechaini = $("#fechaInicio").val();
        var fechafin = $("#fechaFin").val();
        var sala = $("#cboSala").val();
        // if (sala == null) {
        //     toastr.error("Seleccione Sala", "Mensaje Servidor");
        //     return false;
        // }

        var listasala = [];
        $("#cboSala option:selected").each(function () {
            listasala.push($(this).data("id"));
        });
        var listasala = $("#cboSala").val();
        // $.ajax({
        //     type: "POST",
        //     cache: false,
        //     url: basePath + "Reclamacion/ReporteReclamacionDescargarExcelJson",
        //     contentType: "application/json; charset=utf-8",
        //     dataType: "json",
        //     data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        //     beforeSend: function (xhr) {
        //         $.LoadingOverlay("show");
        //     },

        //     success: function (response) {
        //         dataAuditoria(1, "#formfiltro", 3, "Reclamacion/ReporteReclamacionDescargarExcelJson", "BOTON EXCEL");
        //         if (response.respuesta) {
        //             var data = response.data;
        //             var file = response.excelName;
        //             let a = document.createElement('a');
        //             a.target = '_self';
        //             a.href = "data:application/vnd.ms-excel;base64, " + data;
        //             a.download = file;
        //             a.click();
        //         }
        //         else {
        //             toastr.error(response.mensaje, "Mensaje Servidor");
        //         }
        //     },
        //     //error: function (request, status, error) {
        //     //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //     //},
        //     complete: function (resul) {
        //         $.LoadingOverlay("hide");
        //     }
        // });
        let ids=[]
        if(objetodatatable){
            let selectedRows=objetodatatable.rows( {search:'applied'} )
            .data();
            selectedRows.map((item)=>{
                ids.push(item.id)
            })
        }
        if(ids.length>0){
            $.ajax({
                type: "POST",
                cache: false,
                url: basePath + "Reclamacion/ReporteReclamacionDescargarExcelJson",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ codsala: listasala, ArrayReclamacionesId: ids,fechaini, fechafin }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (response) {
                    if (response.respuesta) {
                        dataAuditoria(1, "#formfiltro", 3, "Reclamacion/ReporteReclamacionDescargarExcelJson", "BOTON EXCEL");
                        var data = response.data;
                        var file = response.excelName;
                        let a = document.createElement('a');
                        a.target = '_self';
                        a.href = "data:application/vnd.ms-excel;base64, " + data;
                        a.download = file;
                        a.click();
                    }
                },
                error: function (request, status, error) {
                    toastr.error("Error De Conexion, Servidor no Encontrado.");
                },
                complete: function (resul) {
                    $.LoadingOverlay("hide");
                }
            });
        }

    });

    $(document).on('click', "#btnPdf", function (e) {
        var doc = $(this).data("doc");
        let a = document.createElement('a');
        a.target = '_self';
        a.href = basePath + "Reclamacion/ReclamacionRespuestaPDFDescarga?doc=" + doc+"&todo=true";
        a.click();
    });

    $(document).on('click', ".btnDescargar", function (e) {
        var doc = $(this).data("hash");
        let a = document.createElement('a');
        a.target = '_self';
        a.href = basePath + "Reclamacion/ReclamacionRespuestaPDFDescarga?doc=" + doc;
        a.click();
    });

    VistaAuditoria("Reclamacion/ReclamacionListarVista", "VISTA", 0, "", 3);

    $(".btn_salaenviar").on("click", function () {
        if ($("#salarespuesta").val() == "") {
            toastr.error("Ingrese Acciones Adoptadas para el Cliente", "Mensaje Servidor");
            return false;
        }
        var id = $("#reclamacionid").val();
        var title = 'Mensaje Servidor';
        html = '¿Esta seguro de Continuar?!';
        $.confirm({
            title: title,
            content: html,
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-danger',
            confirm: function () {
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: basePath + "Reclamacion/ReclamacionAtencionSalaJson",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ id, atencionsala:$("#salarespuesta").val() }),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },

                    success: function (response) {
                        console.log(response)
                        if (response.respuesta) {
                            $("#full-modalRegistroReclamacion").modal("hide");
                            toastr.success(response.mensaje, "Mensaje Servidor");
                            buscarReclamaciones()
                        }
                        else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }
                        
                    },
                    error: function (request, status, error) {
                       
                    },
                    complete: function (resul) {
                        $.LoadingOverlay("hide");
                    }
                });

            },

            cancel: function () {
                //close
            },

        });
    });

    $(".btn_legalenviar").on("click", function () {
        if ($("#legalrespuesta").val() == "") {
            toastr.error("Ingrese Respuesta Legal", "Mensaje Servidor");
            return false;
        }
        var id = $("#reclamacionid").val();
        var title = 'Mensaje Servidor';
        html = '¿Esta seguro de Continuar?!';
        $.confirm({
            title: title,
            content: html,
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-danger',
            confirm: function () {
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: basePath + "Reclamacion/ReclamacionAtencionLegalJson",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ id, atencionlegal: $("#legalrespuesta").val() }),
                    beforeSend: function (xhr) {
                        $.LoadingOverlay("show");
                    },

                    success: function (response) {
                        console.log(response)
                        if (response.respuesta) {
                            $("#full-modalRegistroReclamacion").modal("hide");
                            toastr.success(response.mensaje, "Mensaje Servidor");
                            buscarReclamaciones()
                        }
                        else {
                            toastr.error(response.mensaje, "Mensaje Servidor");
                        }

                    },
                    //error: function (request, status, error) {
                    //    toastr.error("Error De Conexion, Servidor no Encontrado.");
                    //},
                    complete: function (resul) {
                        $.LoadingOverlay("hide");
                    }
                });

            },

            cancel: function () {
                //close
            },

        });
    });
    $('#file-upload').change(function () {
        let data = new FormData();
        // let permitido=4194304;
        let permitido=8388608;
        let validarPeso=false;
        let totalPeso=0;
        $.each(jQuery('#file-upload')[0].files, function (i, file) {
            totalPeso += file.size;
            data.append('file-' + i, file);
          
        });
        if(totalPeso <= permitido){
            validarPeso = true
        }
        if(validarPeso){
            let reclamacionid = $("#reclamacionid").val();
            data.append('reclamacionid',reclamacionid)
    
            let title = 'Mensaje Servidor';
            let html = '¿Esta seguro de Continuar?!';
            $.confirm({
                title: title,
                content: html,
                confirmButton: 'Ok',
                cancelButton: 'Cerrar',
                confirmButtonClass: 'btn-info',
                cancelButtonClass: 'btn-danger',
                confirm: function () {
                    $.ajax({
                        url: basePath + "/Reclamacion/ReclamacionSubirAdjuntosJson",
                        data: data,
                        cache: false,
                        contentType: false,
                        processData: false,
                        // dataType: "html",
                        method: 'POST',
                        type: 'POST', // For jQuery < 1.9
                        success: function (response) {  
                            $("#file-upload").val()
                            if(response.respuesta){
                                let div=$("#divAdjuntos")
                                div.html("")
                                let adjuntos = response.adjuntos.split(',');
                                adjuntos.map(item=>{
                                    div.append(`
                                                <a class="btn" 
                                                style="cursor: default;
                                                border: 1px solid #6c757d !important;
                                                border-radius: 15px !important;
                                                color: #000;
                                                margin-bottom:5px;
                                                padding-top: 5px 5px;"
                                                > 
                                                    <span class="badge pull-right deleteAdjunto" 
                                                        style="border-radius: 15px !important;cursor:pointer;"
                                                        data-reclamacionid="${reclamacionid}"
                                                        data-adjunto="${item}">
                                                        <i class="fa fa-trash"></i>
                                                    </span>
                                                    <span class="badge pull-right downloadAdjunto" 
                                                        style="border-radius: 15px !important;cursor:pointer;"
                                                        data-adjunto="${item}">
                                                        <i class="fa fa-download"></i>
                                                    </span>
                                                    ${item}
                                                </a>
                                            `)
                                })
                            } 
                            else{
                                toast.error("No se pudo subir los archivos","Mensaje Servidor")
                            }
                        }
                    });
                },
    
                cancel: function () {
                    //close
                    $("#file-upload").val()
                },
    
            });
        }
        else{
            $("#file-upload").val()
            toastr.error('El tamaño maximo del total de archivos seleccionados es de 8MB','Mensaje Servidor')
        }      
      
    });
    $(document).on('click','.deleteAdjunto',function(e){
        e.preventDefault()
        let reclamacionid=$(this).data('reclamacionid')
        let adjunto=$(this).data('adjunto')
        let dataForm={reclamacionid:reclamacionid,adjunto:adjunto}
        let title = 'Mensaje Servidor';
        let html = '¿Esta seguro de Continuar?!';
        let div=$("#divAdjuntos")
        div.html("")
        $.confirm({
            title: title,
            content: html,
            confirmButton: 'Ok',
            cancelButton: 'Cerrar',
            confirmButtonClass: 'btn-info',
            cancelButtonClass: 'btn-danger',
            confirm: function () {
                $.ajax({
                    type: "POST",
                    cache: false,
                    url: basePath + "Reclamacion/ReclamacionEliminarAdjuntoJson",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(dataForm),
                    success: function (response) {
                        console.log(response)
                        if(response.respuesta){
                            if(response.adjuntos!=''){
                                let adjuntos = response.adjuntos.split(',');
                                adjuntos.map(item=>{
                                    div.append(`
                                                <a class="btn" 
                                                style="cursor: default;
                                                border: 1px solid #6c757d !important;
                                                border-radius: 15px !important;
                                                color: #000;
                                                margin-bottom:5px;
                                                padding-top: 5px 5px;"
                                                > 
                                                    <span class="badge pull-right deleteAdjunto" 
                                                        style="border-radius: 15px !important;cursor:pointer;"
                                                        data-reclamacionid="${reclamacionid}"
                                                        data-adjunto="${item}">
                                                        <i class="fa fa-trash"></i>
                                                    </span>
                                                    <span class="badge pull-right downloadAdjunto" 
                                                        style="border-radius: 15px !important;cursor:pointer;"
                                                        data-adjunto="${item}">
                                                        <i class="fa fa-download"></i>
                                                    </span>
                                                    ${item}
                                                </a>
                                            `)
                                })
                            }
                        }
                        else{
                            toastr.error("No se pudo eliminar el registro","Mensaje Servidor")
                        }
                    }
                })
            },

            cancel: function () {
                //close
                $("#file-upload").val()
            },

        });
     
    })
    $(document).on('click','.downloadAdjunto',function (e) {
        let url= basePath + "Uploads/ReclamacionesAdjuntos/"+$(this).data('adjunto')
        let link=document.createElement('a');
        document.body.appendChild(link);
        link.target="_blank";
        link.href=url ;
        link.click();
    })
    $(document).on('click','#btnDescargaMultiple',function (e) {
        // let data=objetodatatable.rows( {order:'index', search:'applied'} )
        // .data();
        let ids=[]
        if(objetodatatable){
            let selectedRows=objetodatatable.rows( {search:'applied'} )
            .data();
            selectedRows.map((item)=>{
                ids.push(item.id)
            })
        }
        if(ids.length>0){
            $.ajax({
                type: "POST",
                cache: false,
                url: basePath + "Reclamacion/GetListadoREclamacionesReportePdfJson",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ArrayReclamacionesId: ids }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (response) {
                    if (response.respuesta) {
                        let data = response.data;
                        let file = response.filename;
                        let a = document.createElement('a');
                        a.target = '_self';
                        a.href = "data:application/pdf;base64, " + data;
                        a.download = file;
                        a.click();
                    }
                },
                error: function (request, status, error) {
                    toastr.error("Error De Conexion, Servidor no Encontrado.");
                },
                complete: function (resul) {
                    $.LoadingOverlay("hide");
                }
            });
        }
    })

    $(document).on('click','#imprimirCargo',function (e) {
        let ids=$("#idreclamacion").val()
      
        if(ids!=0){
            $.ajax({
                type: "POST",
                cache: false,
                url: basePath + "Reclamacion/ReclamacionCargoPdf",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ id: ids }),
                beforeSend: function (xhr) {
                    $.LoadingOverlay("show");
                },
                success: function (response) {
                    if (response.respuesta) {
                        let data = response.data;
                        let file = response.filename;
                        let a = document.createElement('a');
                        a.target = '_self';
                        a.href = "data:application/pdf;base64, " + data;
                        a.download = file;
                        a.click();
                    }
                },
                error: function (request, status, error) {
                    toastr.error("Error De Conexion, Servidor no Encontrado.");
                },
                complete: function (resul) {
                    $.LoadingOverlay("hide");
                }
            });
        }
    })

    // ifChecked attachament
    $(document).on('ifChecked', '#checkbox_attachament', function (event) {
        var input = event.target
        var checked = true

        UpdateSendAttachament(checked).done(function (response) {
            if (response.success) {
                toastr.success(response.message)
            } else {
                $(input).prop('checked', !checked).iCheck('update')

                toastr.warning(response.message)
            }
        })
    })

    // ifUnchecked attachament
    $(document).on('ifUnchecked', '#checkbox_attachament', function (event) {
        var input = event.target
        var checked = false

        UpdateSendAttachament(checked).done(function (response) {
            if (response.success) {
                toastr.success(response.message)
            } else {
                $(input).prop('checked', !checked).iCheck('update')

                toastr.warning(response.message)
            }
        })
    })
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
            var datos = result.data;
            $.each(datos, function (index, value) {
                $("#cboSala").append('<option value="' + value.CodSala + '"  >' + value.Nombre + '</option>');
            });
            $("#cboSala").select2({
                multiple: true, placeholder: "--Seleccione--", allowClear: true
            });
            $("#cboSala").val(null).trigger("change");
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

function buscarReclamaciones() {
    //var listasala = [];
    //$("#cboSala option:selected").each(function () {
    //    listasala.push($(this).data("id"));
    //});
    var listasala = $("#cboSala").val();

    //var sala = $("#cboSala option:selected").data("id");

    var fechaini = $("#fechaInicio").val();
    var fechafin = $("#fechaFin").val();
    var addtabla = $(".contenedor_tabla");
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Reclamacion/ReclamacionListarxSalaFechaJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ codsala: listasala, fechaini, fechafin }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            console.log(response)
            response = response.data;

            dataAuditoria(1, "#formfiltro", 3, "Reclamacion/ReclamacionListarxSalaFechaJson", "BOTON BUSCAR");
            $(addtabla).empty();
            $(addtabla).append('<table id="tablereclamaciones" class="table table-condensed table-bordered table-hover" style="width:100%"></table>');
            objetodatatable = $("#tablereclamaciones").DataTable({
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
                "aaSorting": [],
                data: response,
                columns: [

                    { data: "id", title: "ID" },
                    { data: "codigo", title: "Código" },
                    { data: "local_nombre", title: "Sala" },
                    { data: "documento", title: "Nro Doc." },
                    { data: "nombre", title: "Cliente" },
                    { data: "telefono", title: "Telefono" },
                    { data: "correo", title: "Correo" },
                    
                    {
                        data: "fecha", title: "Fecha Reg.",
                        "render": function (value) {
                            let span='<span style="display:none">'+value+'</span>'
                            return span+ moment(value).format('DD/MM/YYYY hh:mm:ss A');
                            // return moment(value).format('DD/MM/YYYY hh:mm:ss A');
                        }
                    },

                    
                    { data: "tipo_reclamo", title: "Tipo" },
                    {
                        data: "atencion", title: "Estado",
                        "render": function (value,row,oData) {
                            var estado = "No Atendido";
                            var css = "btn-danger";
                            if(oData.desistimiento==1){
                                estado='Desistido'
                                css='btn-info'
                            }
                            else{
                                switch (value) {
                                    case 0:
                                        estado = "No Atendido"
                                        css = "btn-danger";
                                        break;
                                    case 1:
                                        estado = "Pendiente Legal"
                                        css = "btn-warning";
                                        break;
                                    case 2:
                                        estado = "Atendido"
                                        css = "btn-success";
                                }
                            }
                    
                           
                            return '<span class="label ' + css + '">' + estado + '</span>';
                        }
                    },
                    {
                        data: null, title: "Accion",
                        "render": function (value,row,oData) {
                            var estado = value.Estado;
                            var butom = "";
                            butom += ` <button type="button" class="btn btn-xs btn-warning btnAtender" data-desistido=${oData.desistimiento} data-json='${JSON.stringify(value)}' data-id="${value.id}" data-hash="${value.hash}"><i class="fa fa-file-pdf-o"></i> EDITAR</button>`;
                            butom += ` <button type="button" class="btn btn-xs btn-danger btnDescargar" data-json='${JSON.stringify(value)}' data-id="${value.id}" data-hash="${value.hash}"><i class="fa fa-file-pdf-o"></i> PDF</button>`;
                            butom += ` <button type="button" class="btn btn-xs btn-primary btnCargo" data-id="${value.id}""><i class="fa fa-file-archive"></i> Cargo</button>`;
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

            $('#tablereclamaciones tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });
        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function ObtenerRegistro(id) {
    let div=$("#divAdjuntos")
    div.html("")
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Reclamacion/ReclamacionIdObtenerJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ id: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            if(response.respuesta){
                var imagen = response.imagen;
                let dataSala=response.dataSala;

                response = response.data;
                if(response.desistimiento==1){
                    toastr.warning('Esta reclamacion ha sido desistida por el cliente','Mensaje Servidor')
                    return false
                }
                $("#btnPdf").data('doc',response.hash);
                if(dataSala.RutaArchivoLogo){
                    // let logo=ObtenerImgDrive(dataSala.RutaArchivoLogo)
                    // logo.then(response=>{
                    //     $(".imgreclamacion").attr('src','data:image/png;base64,'+response.data)
                    // })
                    $(".imgreclamacion").attr("src", "https://drive.google.com/uc?id="+dataSala.RutaArchivoLogo)
                }
                else{
                    $(".imgreclamacion").attr("src", basePath +"Content/assets/images/no_image.jpg")
                }
                // $(".imgreclamacion").attr("src", basePath +"Uploads/LogosEmpresas/"+imagen);
                $("#spnfecha").text(moment(response.fecha).format('DD/MM/YYYY hh:mm:ss A'));
                $("#spnCodigo").text(response.codigo.toUpperCase());
                $("#spnrazonsocial").text(response.razon_social.toUpperCase());
                $("#spndireccionempresa").text(response.local_direccion.toUpperCase());
                $("#spnnombrecliente").text(response.nombre.toUpperCase());
                $("#spndireccioncliente").text(response.direccion.toUpperCase());
                $("#spndnicliente").text(response.documento);
                $("#spntelefonocliente").text(response.telefono);
                $("#spncorreo").text(response.correo.toUpperCase());
                $("#spntipobien").text(response.tipo.toUpperCase());
                $("#spnmonto").text(response.monto);
                $("#spndescripcion").text(response.descripcion.toUpperCase());
                $("#spntiporeclamo").text(response.tipo_reclamo.toUpperCase());
                $("#spnlocalnombre").text(response.local_nombre.toUpperCase());
                $("#spnreferencia").text(response.referencia.toUpperCase());
                $("#spndetalle").text(response.detalle.toUpperCase());
                $("#spnpedido").text(response.pedido.toUpperCase());
                $("#salarespuesta").val(response.atencionsala);
                $("#legalrespuesta").val(response.atencionlegal);
                $("#spnpadreomadre").text(response.nombrepadre_madre.toUpperCase());

                var checkbox_attachament_wrap = $('#checkbox_attachament_wrap')
                checkbox_attachament_wrap.html('')

                let mostrar=true;
                dataSala.tipo!=0?$("#mostrarPadreoMadre").show():$("#mostrarPadreoMadre").hide()
                switch (response.atencion) {
                    case 0:
                        $("#salarespuesta").removeAttr("readonly");
                        $("#legalrespuesta").removeAttr("readonly");
                        $(".btn_salaenviar").show();
                        $(".btn_legalenviar").show();
                        $("#divSubirAdjuntos").show();

                        checkbox_attachament_wrap.html(`<input type="checkbox" id="checkbox_attachament" />`)

                        mostrar=true
                        break;
                    case 1:
                        $("#salarespuesta").attr("readonly", "readonly");
                        $("#legalrespuesta").removeAttr("readonly");
                        $(".btn_salaenviar").hide();
                        $(".btn_legalenviar").show();
                        //adjuntos
                        $("#divSubirAdjuntos").show();

                        checkbox_attachament_wrap.html(`<input type="checkbox" id="checkbox_attachament" />`)

                        mostrar=true
                        break;
                    case 2:
                        $("#salarespuesta").attr("readonly", "readonly");
                        $("#legalrespuesta").attr("readonly", "readonly");
                        $(".btn_salaenviar").hide();
                        $(".btn_legalenviar").hide();
                        //ocultar adjuntos
                        $("#divSubirAdjuntos").hide();

                        if (response.enviar_adjunto) {
                            checkbox_attachament_wrap.html(`<span class="check-attach"><i class="fa fa-check"></i></span>`)
                        }

                        mostrar=false
                        break;
                }
                //agregar ADJUNTOS
                let adjunto=response.adjunto
                if(adjunto!=''){
                    let adjuntos = adjunto.split(',');
                    adjuntos.map(item=>{
                        //<i class="fa fa-file-pdf-o"></i>
                        div.append(`
                                <a class="btn" 
                                style="cursor: default;
                                border: 1px solid #6c757d !important;
                                border-radius: 15px !important;
                                color: #000;
                                margin-bottom:5px;
                                padding-top: 5px 5px;"
                                > 
                                    <span class="badge pull-right deleteAdjunto" 
                                        style="border-radius: 15px !important;cursor:pointer;${mostrar?'':'display:none'}"
                                        data-reclamacionid="${id}"
                                        data-adjunto="${item}">
                                        <i class="fa fa-trash"></i>
                                    </span>
                                    <span class="badge pull-right downloadAdjunto" 
                                        style="border-radius: 15px !important;cursor:pointer;"
                                        data-adjunto="${item}">
                                        <i class="fa fa-download"></i>
                                    </span>
                                    ${item}
                                </a>
                            `)
                    })
                }

                if (mostrar) {
                    var sendAttachElement = '#checkbox_attachament'

                    $(sendAttachElement).prop('checked', response.enviar_adjunto)

                    $(sendAttachElement).iCheck({
                        checkboxClass: 'icheckbox_square-blue icheckbox_bg-white'
                    })
                }

                $("#full-modalRegistroReclamacion").modal("show");
            }
            else{
                toast.error(response.mensaje,"Mensaje Servidor")
                $("#full-modalRegistroReclamacion").modal("hide")
            }
        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
function ObtenerRegistroCargo(id) {
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + "Reclamacion/ReclamacionIdObtenerCargoJson",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ id: id }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },
        success: function (response) {
            if(response.respuesta){
                let dataReclamacion=response.data
                let dataEmpresa=response.dataEmpresa
                let dataSala=response.dataSala
                if(dataSala.RutaArchivoLogo){
                    // let logo=ObtenerImgDrive(dataSala.RutaArchivoLogo)
                    // logo.then(response=>{
                    //     $(".imgreclamacion").attr('src','data:image/png;base64,'+response.data)
                    // })
                    $(".imgreclamacion").attr("src", "https://drive.google.com/uc?id="+dataSala.RutaArchivoLogo)
                }
                else{
                    $(".imgreclamacion").attr("src", basePath +"Content/assets/images/no_image.jpg")
                }
                // $(".imgreclamacion").attr("src", basePath +"Uploads/LogosEmpresas/"+dataEmpresa.RutaArchivoLogo);
                $("#spanfecha").text(moment(dataReclamacion.fecha).format('DD/MM/YYYY hh:mm:ss A'));
                $("#spanCodigo").text(dataReclamacion.codigo.toUpperCase());
                $("#spanrazonsocial").text(dataEmpresa.RazonSocial.toUpperCase());
                $("#spandireccionempresa").text(dataEmpresa.Direccion.toUpperCase());

                $("#spancodigo").text(dataReclamacion.codigo.toUpperCase());
                $("#spanempresa").text(dataEmpresa.RazonSocial.toUpperCase());
                $("#spanruc").text(dataEmpresa.Ruc.toUpperCase());
                $("#spandireccion").text(dataEmpresa.Direccion.toUpperCase());
                $("#spancorreodestino").text(dataReclamacion.correo.toUpperCase());
                $("#dpancorreoadjuntos").text(dataReclamacion.direcciones_adjuntas.toUpperCase());
                $("#spansala").text(dataSala.Nombre.toUpperCase());
                $("#spanfechaenvio").text(moment(dataReclamacion.fecha).format('DD/MM/YYYY hh:mm:ss A'));
                if(moment(dataReclamacion.fecha_enviolegal).format('DD/MM/YYYY')!='31/12/1752'){
                    $("#spanfechaenviolegal").text(moment(dataReclamacion.fecha_enviolegal).format('DD/MM/YYYY hh:mm:ss A'));
                }else{
                    $("#spanfechaenviolegal").text('');
                }
                $("#idreclamacion").val(dataReclamacion.id);
                $("#spanusuariolegal").text(dataReclamacion.usuario_legal.toUpperCase());
                
                
            }
            else{
                toast.error(response.mensaje,"Mensaje Servidor")
                $("#full-modalCargoReclamacion").modal("hide")
            }
        },
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}
function ObtenerImgDrive(RutaArchivoLogo){
    let dataForm={ RutaArchivoLogo:RutaArchivoLogo}
    return $.ajax({
        type: "POST",
        url: basePath + "Sala/GetImgPorIdDrive",
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data:JSON.stringify(dataForm),
        beforeSend: function (xhr) {
            // $.LoadingOverlay("show");
        },
        success: function (result) {
            return result
        },
        error: function (request, status, error) {
        },
        complete: function (resul) {
            // $.LoadingOverlay("hide");
        }
    });
}

function GenerarQR(urlSala,nombreSala, codSala) {
    let url = "Reclamacion/GenerarQrReclamacion";
    $.ajax({
        type: "POST",
        cache: false,
        url: basePath + url,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ urlSala, nombreSala, codSala }),
        beforeSend: function (xhr) {
            $.LoadingOverlay("show");
        },

        success: function (response) {
            if (response.respuesta) {
                let data = response.base64String;
                let file = response.filename;
                let a = document.createElement('a');
                a.target = '_self';
                a.href = "data:image/png;base64," + data;
                a.download = file;
                a.click();
            }
            else {
                toastr.error(response.mensaje, "Mensaje Servidor");
            }
        },
        //error: function (request, status, error) {
        //    toastr.error("Error De Conexion, Servidor no Encontrado.");
        //},
        complete: function (resul) {
            $.LoadingOverlay("hide");
        }
    });
}

function UpdateSendAttachament(enviarAdjunto) {
    var reclamacionId = $("#reclamacionid").val()

    toastr.remove()

    if (!reclamacionId) {
        toastr.warning('Por favor, seleccione una Hoja de Reclamación')

        return false
    }

    var data = {
        reclamacionId,
        enviarAdjunto
    }

    return $.ajax({
        type: "POST",
        url: `${basePath}Reclamacion/ActualizarEnviarAdjunto`,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processdata: true,
        cache: false,
        beforeSend: function () {
            $.LoadingOverlay("show")
        },
        complete: function () {
            $.LoadingOverlay("hide")
        }
    })
}